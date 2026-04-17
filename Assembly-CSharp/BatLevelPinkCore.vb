Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x0200050B RID: 1291
Public Class BatLevelPinkCore
	Inherits ParrySwitch

	' Token: 0x060016E5 RID: 5861 RVA: 0x000CDEB1 File Offset: 0x000CC2B1
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.damageDealer = DamageDealer.NewEnemy()
		Me.lastSideHit = BatLevelPinkCore.SideHit.None
	End Sub

	' Token: 0x060016E6 RID: 5862 RVA: 0x000CDECB File Offset: 0x000CC2CB
	Private Sub Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x060016E7 RID: 5863 RVA: 0x000CDEE4 File Offset: 0x000CC2E4
	Public Sub Init(properties As LevelProperties.Bat.BatBouncer, pos As Vector2, angle As Single)
		Me.properties = properties
		MyBase.transform.position = pos
		MyBase.transform.SetEulerAngles(New Single?(0F), New Single?(0F), New Single?(angle))
		MyBase.StartCoroutine(Me.move_cr())
	End Sub

	' Token: 0x060016E8 RID: 5864 RVA: 0x000CDF3C File Offset: 0x000CC33C
	Protected Iterator Function move_cr() As IEnumerator
		Me.velocity = -MyBase.transform.right
		While True
			MyBase.transform.position += Me.velocity * Me.properties.mainBounceSpeed * CupheadTime.Delta
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x060016E9 RID: 5865 RVA: 0x000CDF57 File Offset: 0x000CC357
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		Me.damageDealer.DealDamage(hit)
	End Sub

	' Token: 0x060016EA RID: 5866 RVA: 0x000CDF70 File Offset: 0x000CC370
	Protected Overrides Sub OnCollisionCeiling(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionCeiling(hit, phase)
		If phase = CollisionPhase.Enter AndAlso Me.lastSideHit <> BatLevelPinkCore.SideHit.Top Then
			Dim position As Vector3 = MyBase.transform.position
			Dim vector As Vector3 = New Vector3(MyBase.transform.position.x, CSng(Level.Current.Ground), 0F)
			Me.collisionPoint = vector - position
			Me.lastSideHit = BatLevelPinkCore.SideHit.Top
			MyBase.StartCoroutine(Me.change_direction_cr(Me.collisionPoint))
		End If
	End Sub

	' Token: 0x060016EB RID: 5867 RVA: 0x000CDFF4 File Offset: 0x000CC3F4
	Protected Overrides Sub OnCollisionWalls(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionCeiling(hit, phase)
		If phase = CollisionPhase.Enter Then
			If MyBase.transform.position.x > 0F Then
				If Me.lastSideHit <> BatLevelPinkCore.SideHit.Right Then
					Dim vector As Vector3 = New Vector3(CSng(Level.Current.Left), MyBase.transform.position.y, 0F)
					Dim position As Vector3 = MyBase.transform.position
					Me.collisionPoint = vector - position
					Me.lastSideHit = BatLevelPinkCore.SideHit.Right
					MyBase.StartCoroutine(Me.change_direction_cr(Me.collisionPoint))
				End If
			ElseIf Me.lastSideHit <> BatLevelPinkCore.SideHit.Left Then
				Dim position2 As Vector3 = MyBase.transform.position
				Dim vector2 As Vector3 = New Vector3(CSng(Level.Current.Right), MyBase.transform.position.y, 0F)
				Me.collisionPoint = vector2 - position2
				Me.lastSideHit = BatLevelPinkCore.SideHit.Left
				MyBase.StartCoroutine(Me.change_direction_cr(Me.collisionPoint))
			End If
		End If
	End Sub

	' Token: 0x060016EC RID: 5868 RVA: 0x000CE108 File Offset: 0x000CC508
	Protected Overrides Sub OnCollisionGround(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionCeiling(hit, phase)
		If phase = CollisionPhase.Enter AndAlso Me.lastSideHit <> BatLevelPinkCore.SideHit.Bottom Then
			Dim position As Vector3 = MyBase.transform.position
			Dim vector As Vector3 = New Vector3(MyBase.transform.position.x, CSng(Level.Current.Ceiling), 0F)
			Me.collisionPoint = vector - position
			Me.lastSideHit = BatLevelPinkCore.SideHit.Bottom
			MyBase.StartCoroutine(Me.change_direction_cr(Me.collisionPoint))
		End If
	End Sub

	' Token: 0x060016ED RID: 5869 RVA: 0x000CE18C File Offset: 0x000CC58C
	Protected Iterator Function change_direction_cr(collisionPoint As Vector3) As IEnumerator
		Me.velocity = 1F * (-2F * Vector3.Dot(Me.velocity, Vector3.Normalize(collisionPoint.normalized)) * Vector3.Normalize(collisionPoint.normalized) + Me.velocity)
		Yield Nothing
		Return
	End Function

	' Token: 0x060016EE RID: 5870 RVA: 0x000CE1AE File Offset: 0x000CC5AE
	Public Overrides Sub OnParryPostPause(player As AbstractPlayerController)
		MyBase.OnParryPostPause(player)
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub

	' Token: 0x0400202D RID: 8237
	Private properties As LevelProperties.Bat.BatBouncer

	' Token: 0x0400202E RID: 8238
	Private damageDealer As DamageDealer

	' Token: 0x0400202F RID: 8239
	Private velocity As Vector3

	' Token: 0x04002030 RID: 8240
	Private collisionPoint As Vector3

	' Token: 0x04002031 RID: 8241
	Public lastSideHit As BatLevelPinkCore.SideHit

	' Token: 0x0200050C RID: 1292
	Public Enum SideHit
		' Token: 0x04002033 RID: 8243
		Top
		' Token: 0x04002034 RID: 8244
		Bottom
		' Token: 0x04002035 RID: 8245
		Left
		' Token: 0x04002036 RID: 8246
		Right
		' Token: 0x04002037 RID: 8247
		None
	End Enum
End Class
