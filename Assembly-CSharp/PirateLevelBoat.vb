Imports System
Imports System.Collections
Imports System.Diagnostics
Imports UnityEngine

' Token: 0x0200071B RID: 1819
Public Class PirateLevelBoat
	Inherits LevelProperties.Pirate.Entity

	' Token: 0x0600278D RID: 10125 RVA: 0x001734E6 File Offset: 0x001718E6
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.idle = New PirateLevelBoat.IdleManager()
		Me.ully.gameObject.SetActive(False)
		MyBase.GetComponent(Of LevelBossDeathExploder)().enabled = False
	End Sub

	' Token: 0x0600278E RID: 10126 RVA: 0x00173516 File Offset: 0x00171916
	Public Overrides Sub LevelInit(properties As LevelProperties.Pirate)
		MyBase.LevelInit(properties)
		AddHandler properties.OnStateChange, AddressOf Me.OnStateChange
		AddHandler properties.OnBossDeath, AddressOf Me.OnBossDeath
	End Sub

	' Token: 0x0600278F RID: 10127 RVA: 0x00173544 File Offset: 0x00171944
	Private Sub OnStateChange()
		MyBase.animator.Play("Idle")
		Me.StopAllCoroutines()
		If MyBase.properties.CurrentState.cannon.firing Then
			MyBase.StartCoroutine(Me.cannon_cr(MyBase.properties.CurrentState.cannon.delayRange.RandomFloat()))
		End If
	End Sub

	' Token: 0x06002790 RID: 10128 RVA: 0x001735A8 File Offset: 0x001719A8
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me.cannonProjectile = Nothing
		Me.cannonSmokePrefab = Nothing
		Me.projectilePrefab = Nothing
		Me.beamPrefab = Nothing
	End Sub

	' Token: 0x06002791 RID: 10129 RVA: 0x001735CC File Offset: 0x001719CC
	Private Sub OnIdleEnd()
		If Me.idle.loops >= Me.idle.max Then
			MyBase.animator.SetTrigger("OnBlink")
			Return
		End If
		Me.idle.loops += 1
	End Sub

	' Token: 0x06002792 RID: 10130 RVA: 0x00173618 File Offset: 0x00171A18
	Private Sub OnBlink()
		Me.idle.OnBlink()
	End Sub

	' Token: 0x06002793 RID: 10131 RVA: 0x00173628 File Offset: 0x00171A28
	Private Sub FireCannon()
		AudioManager.Play("level_pirate_ship_cannon_fire")
		If Not MyBase.properties.CurrentState.cannon.firing Then
			Return
		End If
		Me.cannonSmokePrefab.Create(New Vector2(Me.cannonRoot.position.x + 50F, Me.cannonRoot.position.y))
		Dim basicProjectile As BasicProjectile = Me.cannonProjectile.Create(Me.cannonRoot.position, 0F, -MyBase.properties.CurrentState.cannon.speed)
		basicProjectile.CollisionDeath.None()
		basicProjectile.DamagesType.OnlyPlayer()
	End Sub

	' Token: 0x06002794 RID: 10132 RVA: 0x001736EC File Offset: 0x00171AEC
	Private Iterator Function cannon_cr(delay As Single) As IEnumerator
		If delay < 1F Then
			delay = 1F
		End If
		While True
			Yield CupheadTime.WaitForSeconds(Me, delay)
			MyBase.animator.Play("Cannon")
		End While
		Return
	End Function

	' Token: 0x06002795 RID: 10133 RVA: 0x0017370E File Offset: 0x00171B0E
	Private Sub ChewSound()
		AudioManager.Play("level_pirate_ship_cannon_chew")
		Me.emitAudioFromObject.Add("level_pirate_ship_cannon_chew")
	End Sub

	' Token: 0x14000045 RID: 69
	' (add) Token: 0x06002796 RID: 10134 RVA: 0x0017372C File Offset: 0x00171B2C
	' (remove) Token: 0x06002797 RID: 10135 RVA: 0x00173764 File Offset: 0x00171B64
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnLaunchPirate As Action

	' Token: 0x06002798 RID: 10136 RVA: 0x0017379A File Offset: 0x00171B9A
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		MyBase.properties.DealDamage(info.damage)
	End Sub

	' Token: 0x06002799 RID: 10137 RVA: 0x001737AD File Offset: 0x00171BAD
	Public Sub StartTransformation()
		AddHandler Me.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
		MyBase.GetComponent(Of LevelBossDeathExploder)().enabled = True
		Me.hasTransformed = True
		Me.StopAllCoroutines()
		MyBase.StartCoroutine(Me.transform_cr())
	End Sub

	' Token: 0x0600279A RID: 10138 RVA: 0x001737EC File Offset: 0x00171BEC
	Private Sub LaunchPirate()
		CupheadLevelCamera.Current.Shake(15F, 1F, False)
		If Me.OnLaunchPirate IsNot Nothing Then
			Me.OnLaunchPirate()
		End If
		Me.SetNewHeight()
	End Sub

	' Token: 0x0600279B RID: 10139 RVA: 0x00173820 File Offset: 0x00171C20
	Private Sub Shoot()
		AudioManager.Play("level_pirate_ship_uvula_shoot")
		Me.emitAudioFromObject.Add("level_pirate_ship_uvula_shoot")
		Me.projectilePrefab.Create(Me.projectileRoot.position, Me.boatProperties.bulletSpeed, Me.boatProperties.bulletRotationSpeed)
	End Sub

	' Token: 0x0600279C RID: 10140 RVA: 0x0017387C File Offset: 0x00171C7C
	Private Sub SetNewHeight()
		Global.UnityEngine.[Object].FindObjectOfType(Of PirateLevelBoatContainer)().EndBobbing()
		MyBase.transform.parent.SetLocalPosition(Nothing, New Single?(70F), Nothing)
	End Sub

	' Token: 0x0600279D RID: 10141 RVA: 0x001738C0 File Offset: 0x00171CC0
	Private Sub OnBossDeath()
		Me.StopAllCoroutines()
		CupheadLevelCamera.Current.ResetShake()
		If Me.hasTransformed Then
			If Me.beam IsNot Nothing Then
				Me.beam.EndBeam()
			End If
			MyBase.animator.SetTrigger("OnDeath")
		Else
			MyBase.animator.SetTrigger("OnEasyDeath")
		End If
	End Sub

	' Token: 0x0600279E RID: 10142 RVA: 0x0017392C File Offset: 0x00171D2C
	Private Iterator Function transform_cr() As IEnumerator
		Me.boatProperties = MyBase.properties.CurrentState.boat
		MyBase.animator.Play("Idle")
		Yield CupheadTime.WaitForSeconds(Me, 0.3F)
		MyBase.animator.SetTrigger("OnTransform")
		AudioManager.Play("level_pirate_boat_transform")
		Me.emitAudioFromObject.Add("level_pirate_boat_transform")
		Yield CupheadTime.WaitForSeconds(Me, Me.boatProperties.winceDuration)
		MyBase.animator.SetTrigger("OnTransformContinue")
		Yield CupheadTime.WaitForSeconds(Me, 1F)
		Me.ully.gameObject.SetActive(True)
		While True
			For count As Integer = 0 To Me.boatProperties.bulletCount - 1
				Yield CupheadTime.WaitForSeconds(Me, Me.boatProperties.attackDelay)
				MyBase.animator.SetTrigger("OnShoot")
			Next
			Yield CupheadTime.WaitForSeconds(Me, Me.boatProperties.bulletPostWait)
			MyBase.animator.SetTrigger("OnBeamStart")
			Yield CupheadTime.WaitForSeconds(Me, Me.boatProperties.beamDelay + 1F)
			MyBase.animator.SetTrigger("OnBeamContinue")
			CupheadLevelCamera.Current.StartShake(2F)
			Me.beam = Me.beamPrefab.Create(Me.beamRoot)
			Yield CupheadTime.WaitForSeconds(Me, Me.boatProperties.beamDuration)
			MyBase.animator.SetTrigger("OnBeamEnd")
			CupheadLevelCamera.Current.EndShake(0.4F)
			Me.beam.EndBeam()
			Me.beam = Nothing
			Yield CupheadTime.WaitForSeconds(Me, Me.boatProperties.beamPostWait)
			MyBase.animator.Play("Transform_Idle")
		End While
		Return
	End Function

	' Token: 0x0600279F RID: 10143 RVA: 0x00173948 File Offset: 0x00171D48
	Private Iterator Function delay_cr(frameDelay As Integer) As IEnumerator
		For i As Integer = 0 To frameDelay - 1
			Yield Nothing
		Next
		MyBase.StartCoroutine(Me.transform_cr())
		Return
	End Function

	' Token: 0x04003057 RID: 12375
	<SerializeField()>
	Private damageReceiver As DamageReceiver

	' Token: 0x04003058 RID: 12376
	<Space(10F)>
	<SerializeField()>
	Private cannonRoot As Transform

	' Token: 0x04003059 RID: 12377
	<SerializeField()>
	Private cannonProjectile As BasicProjectile

	' Token: 0x0400305A RID: 12378
	<SerializeField()>
	Private cannonSmokePrefab As Effect

	' Token: 0x0400305B RID: 12379
	<Space(10F)>
	<SerializeField()>
	Private projectileRoot As Transform

	' Token: 0x0400305C RID: 12380
	<SerializeField()>
	Private beamRoot As Transform

	' Token: 0x0400305D RID: 12381
	<SerializeField()>
	Private projectilePrefab As PirateLevelBoatProjectile

	' Token: 0x0400305E RID: 12382
	<SerializeField()>
	Private beamPrefab As PirateLevelBoatBeam

	' Token: 0x0400305F RID: 12383
	<Space(10F)>
	<SerializeField()>
	Private ully As SpriteRenderer

	' Token: 0x04003060 RID: 12384
	Private idle As PirateLevelBoat.IdleManager

	' Token: 0x04003061 RID: 12385
	Private hasTransformed As Boolean

	' Token: 0x04003062 RID: 12386
	Private beam As PirateLevelBoatBeam

	' Token: 0x04003063 RID: 12387
	Private Const Y_TRANSFORMED As Single = 70F

	' Token: 0x04003065 RID: 12389
	Private boatProperties As LevelProperties.Pirate.Boat

	' Token: 0x0200071C RID: 1820
	Public Class IdleManager
		' Token: 0x060027A1 RID: 10145 RVA: 0x0017397A File Offset: 0x00171D7A
		Public Sub OnBlink()
			Me.max = Global.UnityEngine.Random.Range(20, 61)
			Me.loops = 0
		End Sub

		' Token: 0x04003066 RID: 12390
		Private Const MIN_LOOPS As Integer = 20

		' Token: 0x04003067 RID: 12391
		Private Const MAX_LOOPS As Integer = 60

		' Token: 0x04003068 RID: 12392
		Public loops As Integer

		' Token: 0x04003069 RID: 12393
		Public max As Integer = 20
	End Class
End Class
