Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports UnityEngine

' Token: 0x020008A6 RID: 2214
Public Class CircusPlatformingLevelMagician
	Inherits AbstractPlatformingLevelEnemy

	' Token: 0x0600338F RID: 13199 RVA: 0x001DF6D4 File Offset: 0x001DDAD4
	Protected Overrides Sub Start()
		MyBase.Start()
		Me.spawnPoints = New List(Of Transform)()
		Me.spawnPoints.AddRange(Me.spawnPointHolder.GetComponentsInChildren(Of Transform)())
		Me.spawnPoints.RemoveAt(0)
		MyBase.StartCoroutine(Me.check_cr())
	End Sub

	' Token: 0x06003390 RID: 13200 RVA: 0x001DF721 File Offset: 0x001DDB21
	Protected Overrides Sub OnStart()
	End Sub

	' Token: 0x06003391 RID: 13201 RVA: 0x001DF724 File Offset: 0x001DDB24
	Private Iterator Function check_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 0.2F)
		Dim player As AbstractPlayerController = PlayerManager.GetNext()
		While player.transform.position.x < Me.startPos.transform.position.x
			If player Is Nothing OrElse player.IsDead Then
				player = PlayerManager.GetNext()
			End If
			Yield Nothing
		End While
		MyBase.StartCoroutine(Me.appear_cr())
		Yield Nothing
		Return
	End Function

	' Token: 0x06003392 RID: 13202 RVA: 0x001DF740 File Offset: 0x001DDB40
	Private Iterator Function appear_cr() As IEnumerator
		Dim player As AbstractPlayerController = PlayerManager.GetNext()
		Yield CupheadTime.WaitForSeconds(Me, MyBase.Properties.magicianAppearDelayRange.RandomFloat())
		While True
			While player.transform.position.x < Me.startPos.transform.position.x OrElse player.transform.position.x > Me.endPos.transform.position.x
				Yield Nothing
			End While
			Me.EnableMagician(True)
			While Not Me.attackTrigger
				Yield Nothing
			End While
			While Not CupheadLevelCamera.Current.ContainsPoint(MyBase.transform.position, New Vector2(0F, 1000F))
				Yield Nothing
			End While
			player = PlayerManager.GetFirst()
			Dim dir As Vector2 = player.transform.position - MyBase.transform.position
			Me.projectileInstance = TryCast(Me.projectile.Create(MyBase.transform.position, MathUtils.DirectionToAngle(dir), MyBase.Properties.ProjectileSpeed), CircusPlatformingLevelMagicianBullet)
			AddHandler Me.projectileInstance.OnProjectileDeath, AddressOf Me.OnProjectileDeath
			While Not Me.disappearTrigger
				Yield Nothing
			End While
			Me.disappearTrigger = False
			Me.attackTrigger = False
			Me.EnableMagician(False)
			While Me.t < MyBase.Properties.magicianAppearDelayRange.RandomFloat()
				Me.t += CupheadTime.Delta
				Yield Nothing
			End While
			Me.t = 0F
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06003393 RID: 13203 RVA: 0x001DF75B File Offset: 0x001DDB5B
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		If Me.projectileInstance IsNot Nothing Then
			RemoveHandler Me.projectileInstance.OnProjectileDeath, AddressOf Me.OnProjectileDeath
		End If
		Me.projectile = Nothing
	End Sub

	' Token: 0x06003394 RID: 13204 RVA: 0x001DF792 File Offset: 0x001DDB92
	Private Sub OnProjectileDeath()
		MyBase.animator.SetTrigger("EndAttack")
	End Sub

	' Token: 0x06003395 RID: 13205 RVA: 0x001DF7A4 File Offset: 0x001DDBA4
	Public Sub Attack()
		Me.attackTrigger = True
	End Sub

	' Token: 0x06003396 RID: 13206 RVA: 0x001DF7AD File Offset: 0x001DDBAD
	Public Sub Disappear()
		Me.disappearTrigger = True
	End Sub

	' Token: 0x06003397 RID: 13207 RVA: 0x001DF7B8 File Offset: 0x001DDBB8
	Protected Overrides Sub OnDrawGizmos()
		MyBase.OnDrawGizmos()
		Gizmos.color = Color.white
		Dim list As List(Of Transform) = New List(Of Transform)()
		list.AddRange(Me.spawnPointHolder.GetComponentsInChildren(Of Transform)())
		list.RemoveAt(0)
		For i As Integer = 0 To list.Count - 1
			Gizmos.DrawWireSphere(list(i).transform.position, 50F)
		Next
		Gizmos.DrawLine(New Vector2(Me.startPos.transform.position.x, Me.startPos.transform.position.y + 1000F), New Vector2(Me.startPos.transform.position.x, Me.startPos.transform.position.y - 1000F))
		Gizmos.DrawLine(New Vector2(Me.endPos.transform.position.x, Me.endPos.transform.position.y + 1000F), New Vector2(Me.endPos.transform.position.x, Me.endPos.transform.position.y - 1000F))
	End Sub

	' Token: 0x06003398 RID: 13208 RVA: 0x001DF938 File Offset: 0x001DDD38
	Private Sub EnableMagician(enabled As Boolean)
		MyBase.GetComponent(Of Animator)().enabled = enabled
		MyBase.GetComponent(Of Collider2D)().enabled = enabled
		MyBase.GetComponent(Of SpriteRenderer)().enabled = enabled
		If enabled Then
			MyBase.transform.position = Me.spawnPoints(Global.UnityEngine.Random.Range(0, Me.spawnPoints.Count)).transform.position
		End If
	End Sub

	' Token: 0x06003399 RID: 13209 RVA: 0x001DF9A0 File Offset: 0x001DDDA0
	Protected Overrides Sub Die()
		AudioManager.Play("circus_generic_death_big")
		Me.emitAudioFromObject.Add("circus_generic_death_big")
		MyBase.Die()
	End Sub

	' Token: 0x0600339A RID: 13210 RVA: 0x001DF9C2 File Offset: 0x001DDDC2
	Private Sub AttackAppearSFX()
		AudioManager.Play("circus_magician_appears")
		Me.emitAudioFromObject.Add("circus_magician_appears")
	End Sub

	' Token: 0x0600339B RID: 13211 RVA: 0x001DF9DE File Offset: 0x001DDDDE
	Private Sub AttackIntroSFX()
		AudioManager.Play("circus_magician_attack_intro")
		Me.emitAudioFromObject.Add("circus_magician_attack_intro")
	End Sub

	' Token: 0x0600339C RID: 13212 RVA: 0x001DF9FA File Offset: 0x001DDDFA
	Private Sub AttackOutroSFX()
		AudioManager.Play("circus_magician_attack_outro")
		Me.emitAudioFromObject.Add("circus_magician_attack_outro")
	End Sub

	' Token: 0x04003BE4 RID: 15332
	Private Const EndAttackParameterName As String = "EndAttack"

	' Token: 0x04003BE5 RID: 15333
	<SerializeField()>
	Private startPos As Transform

	' Token: 0x04003BE6 RID: 15334
	<SerializeField()>
	Private endPos As Transform

	' Token: 0x04003BE7 RID: 15335
	<SerializeField()>
	Private spawnPointHolder As Transform

	' Token: 0x04003BE8 RID: 15336
	<SerializeField()>
	Private projectile As CircusPlatformingLevelMagicianBullet

	' Token: 0x04003BE9 RID: 15337
	Private spawnPoints As List(Of Transform)

	' Token: 0x04003BEA RID: 15338
	Private attackTrigger As Boolean

	' Token: 0x04003BEB RID: 15339
	Private disappearTrigger As Boolean

	' Token: 0x04003BEC RID: 15340
	Private t As Single

	' Token: 0x04003BED RID: 15341
	Private projectileInstance As CircusPlatformingLevelMagicianBullet
End Class
