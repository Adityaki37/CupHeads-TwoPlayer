Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020004EB RID: 1259
Public Class BaronessLevelCupcake
	Inherits BaronessLevelMiniBossBase

	' Token: 0x17000320 RID: 800
	' (get) Token: 0x060015E1 RID: 5601 RVA: 0x000C4529 File Offset: 0x000C2929
	' (set) Token: 0x060015E2 RID: 5602 RVA: 0x000C4531 File Offset: 0x000C2931
	Public Property state As BaronessLevelCupcake.State

	' Token: 0x060015E3 RID: 5603 RVA: 0x000C453C File Offset: 0x000C293C
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.isGoingDown = False
		Me.isGoingRight = False
		Me.xSpeed = Me.changeXSpeed
		Me.patternIndex = 0
		Me.fadeTime = 0.3F
		Me.damageDealer = DamageDealer.NewEnemy()
		AddHandler Me.collisionChild.GetComponent(Of DamageReceiver)().OnDamageTaken, AddressOf Me.OnDamageTaken
		AddHandler Me.collisionChild.GetComponent(Of CollisionChild)().OnPlayerCollision, AddressOf Me.OnCollisionPlayer
	End Sub

	' Token: 0x060015E4 RID: 5604 RVA: 0x000C45C0 File Offset: 0x000C29C0
	Protected Sub Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x060015E5 RID: 5605 RVA: 0x000C45D8 File Offset: 0x000C29D8
	Public Sub Init(properties As LevelProperties.Baroness.Cupcake, pos As Vector2, health As Single)
		Me.properties = properties
		MyBase.transform.position = pos
		Me.health = health
		Me.state = BaronessLevelCupcake.State.Moving
		MyBase.StartCoroutine(Me.select_x_speed_cr())
		MyBase.StartCoroutine(Me.moving_cr())
	End Sub

	' Token: 0x060015E6 RID: 5606 RVA: 0x000C4628 File Offset: 0x000C2A28
	Protected Overrides Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		If Me.health > 0F Then
			MyBase.OnDamageTaken(info)
		End If
		Me.health -= info.damage
		If Me.health < 0F AndAlso Me.state <> BaronessLevelCupcake.State.Dying Then
			Dim damageInfo As DamageDealer.DamageInfo = New DamageDealer.DamageInfo(Me.health, info.direction, info.origin, info.damageSource)
			MyBase.OnDamageTaken(damageInfo)
			Me.state = BaronessLevelCupcake.State.Dying
			Me.StartDeath()
		End If
	End Sub

	' Token: 0x060015E7 RID: 5607 RVA: 0x000C46AD File Offset: 0x000C2AAD
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x060015E8 RID: 5608 RVA: 0x000C46CB File Offset: 0x000C2ACB
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me.splashPrefab = Nothing
	End Sub

	' Token: 0x060015E9 RID: 5609 RVA: 0x000C46DA File Offset: 0x000C2ADA
	Protected Overrides Function hitPauseCoefficient() As Single
		Return If((Not Me.collisionChild.GetComponent(Of DamageReceiver)().IsHitPaused), 1F, 0F)
	End Function

	' Token: 0x060015EA RID: 5610 RVA: 0x000C4700 File Offset: 0x000C2B00
	Private Sub SetLaunchOffset()
		MyBase.transform.position = Me.launchOffset.transform.position
	End Sub

	' Token: 0x060015EB RID: 5611 RVA: 0x000C4720 File Offset: 0x000C2B20
	Private Iterator Function moving_cr() As IEnumerator
		Dim wait As YieldInstruction = New WaitForFixedUpdate()
		Dim curlAni As Boolean = False
		Dim flatAni As Boolean = False
		While True
			If Not Me.isGoingDown Then
				If flatAni Then
					MyBase.StartCoroutine(Me.select_x_speed_cr())
					Yield CupheadTime.WaitForSeconds(Me, Me.properties.hold)
					MyBase.animator.SetTrigger("Continue")
					Yield MyBase.animator.WaitForAnimationToEnd(Me, "Slam_Start", False, True)
					flatAni = False
				End If
				Me.GoingUp()
				curlAni = True
			Else
				If curlAni Then
					Yield MyBase.animator.WaitForAnimationToEnd(Me, "Slam_Curl", False, True)
					curlAni = False
				End If
				Me.GoingDown()
				flatAni = True
			End If
			Yield wait
		End While
		Return
	End Function

	' Token: 0x060015EC RID: 5612 RVA: 0x000C473C File Offset: 0x000C2B3C
	Private Sub GoingUp()
		If MyBase.transform.position.y < 360F - Me.offset Then
			Dim position As Vector3 = MyBase.transform.position
			If Not Me.isGoingRight Then
				position.x -= Me.changeXSpeed * CupheadTime.FixedDelta * Me.hitPauseCoefficient()
			Else
				position.x += Me.changeXSpeed * CupheadTime.FixedDelta * Me.hitPauseCoefficient()
			End If
			position.y += Me.ySpeedUp * CupheadTime.FixedDelta
			MyBase.transform.position = position
			Me.BoundaryCheck()
		Else
			Me.isGoingDown = True
		End If
	End Sub

	' Token: 0x060015ED RID: 5613 RVA: 0x000C4804 File Offset: 0x000C2C04
	Private Sub GoingDown()
		If MyBase.transform.position.y > CSng(Level.Current.Ground) + 120F Then
			If Me.xSpeed = 0F Then
				Me.xSpeed = Me.changeXSpeed
			End If
			Dim position As Vector3 = MyBase.transform.position
			position.y -= Me.ySpeedDown * CupheadTime.FixedDelta * Me.hitPauseCoefficient()
			MyBase.transform.position = position
		Else
			Dim position2 As Vector3 = MyBase.transform.position
			position2.y = CSng(Level.Current.Ground) + 120F
			MyBase.transform.position = position2
			Me.isGoingDown = False
		End If
	End Sub

	' Token: 0x060015EE RID: 5614 RVA: 0x000C48CC File Offset: 0x000C2CCC
	Private Sub BoundaryCheck()
		If MyBase.transform.position.x < -540F AndAlso Not Me.isGoingRight Then
			Me.xSpeed = 0F
			MyBase.transform.SetScale(New Single?(-1F), New Single?(1F), New Single?(1F))
			Me.isGoingRight = True
		ElseIf MyBase.transform.position.x > 540F AndAlso Me.isGoingRight Then
			Me.xSpeed = 0F
			MyBase.transform.SetScale(New Single?(1F), New Single?(1F), New Single?(1F))
			Me.isGoingRight = False
		Else
			Me.xSpeed = Me.changeXSpeed
		End If
	End Sub

	' Token: 0x060015EF RID: 5615 RVA: 0x000C49B8 File Offset: 0x000C2DB8
	Private Iterator Function select_x_speed_cr() As IEnumerator
		Dim pattern As String() = Me.properties.XSpeedString(0).Split(New Char() { ","c })
		Parser.FloatTryParse(pattern(Me.patternIndex), Me.changeXSpeed)
		If Me.patternIndex < pattern.Length - 1 Then
			Me.patternIndex += 1
		Else
			Me.patternIndex = 0
		End If
		Yield Nothing
		Return
	End Function

	' Token: 0x060015F0 RID: 5616 RVA: 0x000C49D3 File Offset: 0x000C2DD3
	Private Sub FireBullets()
		If Me.properties.projectileOn Then
			Me.StartSplashes()
		End If
	End Sub

	' Token: 0x060015F1 RID: 5617 RVA: 0x000C49EC File Offset: 0x000C2DEC
	Private Sub StartSplashes()
		MyBase.StartCoroutine(Me.splash_cr(True, Me.deathRoot.transform.position.x))
		MyBase.StartCoroutine(Me.splash_cr(False, Me.deathRoot.transform.position.x))
	End Sub

	' Token: 0x060015F2 RID: 5618 RVA: 0x000C4A48 File Offset: 0x000C2E48
	Private Iterator Function splash_cr(onLeft As Boolean, posX As Single) As IEnumerator
		Dim originalOffset As Single = If((Not onLeft), (-Me.properties.splashOriginalOffset), Me.properties.splashOriginalOffset)
		Dim offset As Single = If((Not onLeft), (-Me.properties.splashOffset), Me.properties.splashOffset)
		Dim delay As Single = 0.4F
		Dim value As Integer = 0
		For i As Integer = 0 To 3 - 1
			If onLeft Then
				value = i
			ElseIf i = 0 Then
				value = 2
			ElseIf i = 1 Then
				value = 0
			Else
				value = 1
			End If
			Dim splash As Effect = Me.splashPrefab.Create(New Vector2(posX + originalOffset + offset * CSng(i), CSng(Level.Current.Ground)))
			Dim scale As Single = If((Not onLeft), splash.transform.localScale.x, (-splash.transform.localScale.x))
			splash.animator.SetInteger("SplashType", value)
			splash.transform.SetScale(New Single?(scale), Nothing, Nothing)
			Yield CupheadTime.WaitForSeconds(Me, delay)
		Next
		Return
	End Function

	' Token: 0x060015F3 RID: 5619 RVA: 0x000C4A71 File Offset: 0x000C2E71
	Private Sub StartDeath()
		Me.StopAllCoroutines()
		MyBase.StartCoroutine(Me.death_cr())
	End Sub

	' Token: 0x060015F4 RID: 5620 RVA: 0x000C4A88 File Offset: 0x000C2E88
	Private Iterator Function death_cr() As IEnumerator
		Me.StartExplosions()
		Me.collisionChild.GetComponent(Of Collider2D)().enabled = False
		MyBase.transform.position = Me.deathRoot.transform.position
		Me.isDying = True
		MyBase.animator.SetTrigger("Death")
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Cupcake_Death", False, True)
		Me.EndExplosions()
		Me.Die()
		Return
	End Function

	' Token: 0x060015F5 RID: 5621 RVA: 0x000C4AA3 File Offset: 0x000C2EA3
	Private Sub SoundCupcakeJump()
		AudioManager.Play("level_baroness_cupcake_jump")
		Me.emitAudioFromObject.Add("level_baroness_cupcake_jump")
	End Sub

	' Token: 0x060015F6 RID: 5622 RVA: 0x000C4ABF File Offset: 0x000C2EBF
	Private Sub SoundCupcakeLand()
		AudioManager.Play("level_baroness_cupcake_land")
		Me.emitAudioFromObject.Add("level_baroness_cupcake_land")
	End Sub

	' Token: 0x060015F7 RID: 5623 RVA: 0x000C4ADB File Offset: 0x000C2EDB
	Private Sub SoundCupcakeSpin()
		AudioManager.Play("level_baroness_cupcake_spin")
		Me.emitAudioFromObject.Add("level_baroness_cupcake_spin")
	End Sub

	' Token: 0x04001F33 RID: 7987
	Private properties As LevelProperties.Baroness.Cupcake

	' Token: 0x04001F34 RID: 7988
	Private damageDealer As DamageDealer

	' Token: 0x04001F35 RID: 7989
	<SerializeField()>
	Private splashPrefab As Effect

	' Token: 0x04001F36 RID: 7990
	<SerializeField()>
	Private launchOffset As Transform

	' Token: 0x04001F37 RID: 7991
	<SerializeField()>
	Private cupcakeProjectile As BasicProjectile

	' Token: 0x04001F38 RID: 7992
	<SerializeField()>
	Private collisionChild As Transform

	' Token: 0x04001F39 RID: 7993
	<SerializeField()>
	Private deathRoot As Transform

	' Token: 0x04001F3A RID: 7994
	Private health As Single

	' Token: 0x04001F3B RID: 7995
	Private ySpeedUp As Single = 1800F

	' Token: 0x04001F3C RID: 7996
	Private ySpeedDown As Single = 2500F

	' Token: 0x04001F3D RID: 7997
	Private xSpeed As Single

	' Token: 0x04001F3E RID: 7998
	Private changeXSpeed As Single

	' Token: 0x04001F3F RID: 7999
	Private offset As Single = 250F

	' Token: 0x04001F40 RID: 8000
	Private isGoingDown As Boolean

	' Token: 0x04001F41 RID: 8001
	Private isGoingRight As Boolean

	' Token: 0x04001F42 RID: 8002
	Private patternIndex As Integer

	' Token: 0x04001F43 RID: 8003
	Private mainPatternIndex As Integer

	' Token: 0x020004EC RID: 1260
	Public Enum State
		' Token: 0x04001F45 RID: 8005
		Moving
		' Token: 0x04001F46 RID: 8006
		Dying
	End Enum
End Class
