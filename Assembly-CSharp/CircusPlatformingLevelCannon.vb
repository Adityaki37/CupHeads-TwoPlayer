Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x0200089F RID: 2207
Public Class CircusPlatformingLevelCannon
	Inherits AbstractPausableComponent

	' Token: 0x0600335B RID: 13147 RVA: 0x001DDF9C File Offset: 0x001DC39C
	Private Sub Start()
		Me.goingBackwards = Rand.Bool()
		Me.shootIndex = Global.UnityEngine.Random.Range(0, Me.shootRoots.Length)
		Me.pinkSplits = Me.pinkString.Split(New Char() { ","c })
		Me.pinkIndex = Global.UnityEngine.Random.Range(0, Me.pinkSplits.Length)
		AddHandler MyBase.GetComponent(Of DamageReceiver)().OnDamageTaken, AddressOf Me.OnDamageTaken
		For Each damageReceiver As DamageReceiver In Me.cannons
			AddHandler damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
		Next
		MyBase.StartCoroutine(Me.shoot_cr())
	End Sub

	' Token: 0x0600335C RID: 13148 RVA: 0x001DE050 File Offset: 0x001DC450
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		Me.health -= info.damage
		If Me.health < 0F AndAlso Not Me.isDead Then
			Me.isDead = True
			Me.StopAllCoroutines()
			MyBase.StartCoroutine(Me.slide_off_cr())
		End If
	End Sub

	' Token: 0x0600335D RID: 13149 RVA: 0x001DE0A8 File Offset: 0x001DC4A8
	Private Iterator Function shoot_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 0.2F)
		While True
			While PlayerManager.GetNext().transform.position.x < Me.startTrigger.transform.position.x
				Yield Nothing
			End While
			MyBase.animator.SetInteger("Cannon", Me.shootIndex + 1)
			Yield CupheadTime.WaitForSeconds(Me, Me.projectileDelay)
			If PlayerManager.GetNext().transform.position.x > Me.endTrigger.position.x Then
				While PlayerManager.GetNext().transform.position.x > Me.endTrigger.position.x
					Yield Nothing
				End While
			End If
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x0600335E RID: 13150 RVA: 0x001DE0C4 File Offset: 0x001DC4C4
	Private Sub Shoot()
		Dim circusPlatformingLevelCannonProjectile As CircusPlatformingLevelCannonProjectile = TryCast(Me.projectile.Create(Me.shootRoots(Me.shootIndex).transform.position, 0F, -Me.projectileSpeed), CircusPlatformingLevelCannonProjectile)
		circusPlatformingLevelCannonProjectile.SetColor(Me.pinkSplits(Me.pinkIndex))
		circusPlatformingLevelCannonProjectile.DestroyDistance = 0F
		Me.pinkIndex = (Me.pinkIndex + 1) Mod Me.pinkSplits.Length
		If Me.goingBackwards Then
			If Me.shootIndex > 0 Then
				Me.shootIndex -= 1
			Else
				Me.shootIndex = Me.shootRoots.Length - 1
			End If
		Else
			Me.shootIndex = (Me.shootIndex + 1) Mod Me.shootRoots.Length
		End If
		MyBase.animator.SetInteger("Cannon", 0)
	End Sub

	' Token: 0x0600335F RID: 13151 RVA: 0x001DE1A8 File Offset: 0x001DC5A8
	Private Iterator Function slide_off_cr() As IEnumerator
		MyBase.GetComponent(Of LevelBossDeathExploder)().StartExplosion()
		MyBase.animator.SetTrigger("Droop")
		Dim slideOffSpeed As Single = 500F
		Dim wait As YieldInstruction = New WaitForFixedUpdate()
		While MyBase.transform.position.y < 1220F
			MyBase.transform.AddPosition(0F, slideOffSpeed * CupheadTime.FixedDelta, 0F)
			Yield wait
		End While
		MyBase.GetComponent(Of LevelBossDeathExploder)().StopExplosions()
		Return
	End Function

	' Token: 0x06003360 RID: 13152 RVA: 0x001DE1C4 File Offset: 0x001DC5C4
	Protected Overrides Sub OnDrawGizmos()
		MyBase.OnDrawGizmos()
		Gizmos.DrawLine(New Vector2(Me.startTrigger.transform.position.x, Me.startTrigger.transform.position.y - 1000F), New Vector2(Me.startTrigger.transform.position.x, Me.startTrigger.transform.position.y + 1000F))
		Gizmos.DrawLine(New Vector2(Me.endTrigger.transform.position.x, Me.endTrigger.transform.position.y - 1000F), New Vector2(Me.endTrigger.transform.position.x, Me.endTrigger.transform.position.y + 1000F))
	End Sub

	' Token: 0x06003361 RID: 13153 RVA: 0x001DE2E5 File Offset: 0x001DC6E5
	Private Sub ShootSFX()
		AudioManager.Play("circus_cannon_shoot")
		Me.emitAudioFromObject.Add("circus_cannon_shoot")
	End Sub

	' Token: 0x06003362 RID: 13154 RVA: 0x001DE301 File Offset: 0x001DC701
	Private Sub DroopSFX()
		AudioManager.Play("circus_cannon_droop")
		Me.emitAudioFromObject.Add("circus_cannon_droop")
	End Sub

	' Token: 0x04003BA1 RID: 15265
	Private Const ShootParameterName As String = "Cannon"

	' Token: 0x04003BA2 RID: 15266
	<SerializeField()>
	Private health As Single

	' Token: 0x04003BA3 RID: 15267
	<SerializeField()>
	Private cannons As DamageReceiver()

	' Token: 0x04003BA4 RID: 15268
	<SerializeField()>
	Private shootRoots As Transform()

	' Token: 0x04003BA5 RID: 15269
	<SerializeField()>
	Private projectile As CircusPlatformingLevelCannonProjectile

	' Token: 0x04003BA6 RID: 15270
	<SerializeField()>
	Private projectileSpeed As Single

	' Token: 0x04003BA7 RID: 15271
	<SerializeField()>
	Private projectileDelay As Single

	' Token: 0x04003BA8 RID: 15272
	<SerializeField()>
	Private startTrigger As Transform

	' Token: 0x04003BA9 RID: 15273
	<SerializeField()>
	Private endTrigger As Transform

	' Token: 0x04003BAA RID: 15274
	<SerializeField()>
	Private pinkString As String

	' Token: 0x04003BAB RID: 15275
	Private shootIndex As Integer

	' Token: 0x04003BAC RID: 15276
	Private goingBackwards As Boolean

	' Token: 0x04003BAD RID: 15277
	Private isDead As Boolean

	' Token: 0x04003BAE RID: 15278
	Private pinkSplits As String()

	' Token: 0x04003BAF RID: 15279
	Private pinkIndex As Integer
End Class
