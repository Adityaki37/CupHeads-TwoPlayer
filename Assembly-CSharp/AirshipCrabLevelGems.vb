Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020004D1 RID: 1233
Public Class AirshipCrabLevelGems
	Inherits ParrySwitch

	' Token: 0x06001502 RID: 5378 RVA: 0x000BCB38 File Offset: 0x000BAF38
	Public Sub Init(properties As LevelProperties.AirshipCrab.Gems, pos As Vector2, angle As Single)
		Me.properties = properties
		MyBase.transform.position = pos
		Me.pink = MyBase.GetComponent(Of SpriteRenderer)().color
		Me.startPos = MyBase.transform.position
		MyBase.transform.SetEulerAngles(New Single?(0F), New Single?(0F), New Single?(-angle))
		Me.velocity = -MyBase.transform.right
	End Sub

	' Token: 0x06001503 RID: 5379 RVA: 0x000BCBBB File Offset: 0x000BAFBB
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.damageDealer = DamageDealer.NewEnemy()
		Me.lastSideHit = AirshipCrabLevelGems.SideHit.None
		Me.parried = False
	End Sub

	' Token: 0x06001504 RID: 5380 RVA: 0x000BCBDC File Offset: 0x000BAFDC
	Private Sub Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x06001505 RID: 5381 RVA: 0x000BCBF4 File Offset: 0x000BAFF4
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, CollisionPhase.Enter)
		Me.damageDealer.DealDamage(hit)
	End Sub

	' Token: 0x06001506 RID: 5382 RVA: 0x000BCC0C File Offset: 0x000BB00C
	Protected Overrides Sub OnCollisionCeiling(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionCeiling(hit, phase)
		If phase = CollisionPhase.Enter AndAlso Me.lastSideHit <> AirshipCrabLevelGems.SideHit.Top Then
			Dim position As Vector3 = MyBase.transform.position
			Dim vector As Vector3 = New Vector3(MyBase.transform.position.x, CSng(Level.Current.Ground), 0F)
			Me.collisionPoint = vector - position
			Me.lastSideHit = AirshipCrabLevelGems.SideHit.Top
			MyBase.StartCoroutine(Me.change_direction_cr(Me.collisionPoint))
		End If
	End Sub

	' Token: 0x06001507 RID: 5383 RVA: 0x000BCC90 File Offset: 0x000BB090
	Protected Overrides Sub OnCollisionWalls(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionCeiling(hit, phase)
		If phase = CollisionPhase.Enter Then
			If MyBase.transform.position.x > 0F Then
				If Me.lastSideHit <> AirshipCrabLevelGems.SideHit.Right Then
					Dim vector As Vector3 = New Vector3(CSng(Level.Current.Left), MyBase.transform.position.y, 0F)
					Dim position As Vector3 = MyBase.transform.position
					Me.collisionPoint = vector - position
					Me.lastSideHit = AirshipCrabLevelGems.SideHit.Right
					MyBase.StartCoroutine(Me.change_direction_cr(Me.collisionPoint))
				End If
			ElseIf Me.lastSideHit <> AirshipCrabLevelGems.SideHit.Left Then
				Dim position2 As Vector3 = MyBase.transform.position
				Dim vector2 As Vector3 = New Vector3(CSng(Level.Current.Right), MyBase.transform.position.y, 0F)
				Me.collisionPoint = vector2 - position2
				Me.lastSideHit = AirshipCrabLevelGems.SideHit.Left
				MyBase.StartCoroutine(Me.change_direction_cr(Me.collisionPoint))
			End If
		End If
	End Sub

	' Token: 0x06001508 RID: 5384 RVA: 0x000BCDA4 File Offset: 0x000BB1A4
	Protected Overrides Sub OnCollisionGround(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionCeiling(hit, phase)
		If phase = CollisionPhase.Enter AndAlso Me.lastSideHit <> AirshipCrabLevelGems.SideHit.Bottom Then
			Dim position As Vector3 = MyBase.transform.position
			Dim vector As Vector3 = New Vector3(MyBase.transform.position.x, CSng(Level.Current.Ceiling), 0F)
			Me.collisionPoint = vector - position
			Me.lastSideHit = AirshipCrabLevelGems.SideHit.Bottom
			MyBase.StartCoroutine(Me.change_direction_cr(Me.collisionPoint))
		End If
	End Sub

	' Token: 0x06001509 RID: 5385 RVA: 0x000BCE28 File Offset: 0x000BB228
	Public Sub PickMovement()
		If Not Me.parried Then
			If Me.currentMovement IsNot Nothing Then
				MyBase.StopCoroutine(Me.currentMovement)
			End If
			Me.currentMovement = MyBase.StartCoroutine(Me.move_cr())
		Else
			If Me.currentMovement IsNot Nothing Then
				MyBase.StopCoroutine(Me.currentMovement)
			End If
			Me.currentMovement = MyBase.StartCoroutine(Me.go_back_cr())
		End If
	End Sub

	' Token: 0x0600150A RID: 5386 RVA: 0x000BCE98 File Offset: 0x000BB298
	Private Iterator Function move_cr() As IEnumerator
		Me.moving = True
		Me.parried = False
		MyBase.GetComponent(Of Collider2D)().enabled = True
		MyBase.GetComponent(Of SpriteRenderer)().color = Me.pink
		While Me.moving
			MyBase.transform.position += Me.velocity * Me.properties.bulletSpeed * CupheadTime.FixedDelta
			Yield Nothing
		End While
		Yield Nothing
		Return
	End Function

	' Token: 0x0600150B RID: 5387 RVA: 0x000BCEB4 File Offset: 0x000BB2B4
	Private Iterator Function change_direction_cr(collisionPoint As Vector3) As IEnumerator
		Me.velocity = 1F * (-2F * Vector3.Dot(Me.velocity, Vector3.Normalize(collisionPoint.normalized)) * Vector3.Normalize(collisionPoint.normalized) + Me.velocity)
		Yield Nothing
		Return
	End Function

	' Token: 0x0600150C RID: 5388 RVA: 0x000BCED8 File Offset: 0x000BB2D8
	Private Iterator Function go_back_cr() As IEnumerator
		Me.velocity = -MyBase.transform.right
		While MyBase.transform.position <> Me.startPos
			MyBase.transform.position = Vector3.MoveTowards(MyBase.transform.position, Me.startPos, Me.properties.bulletSpeed * CupheadTime.Delta)
			Yield Nothing
		End While
		Yield Nothing
		Return
	End Function

	' Token: 0x0600150D RID: 5389 RVA: 0x000BCEF3 File Offset: 0x000BB2F3
	Public Overrides Sub OnParryPrePause(player As AbstractPlayerController)
		MyBase.OnParryPrePause(player)
		player.stats.ParryOneQuarter()
	End Sub

	' Token: 0x0600150E RID: 5390 RVA: 0x000BCF08 File Offset: 0x000BB308
	Public Overrides Sub OnParryPostPause(player As AbstractPlayerController)
		MyBase.OnParryPostPause(player)
		Me.startTimer = True
		Me.parried = True
		Me.moving = False
		MyBase.GetComponent(Of Collider2D)().enabled = False
		MyBase.GetComponent(Of SpriteRenderer)().color = New Color(1F, 1F, 1F, 1F)
		Me.PickMovement()
	End Sub

	' Token: 0x04001E61 RID: 7777
	Public parried As Boolean

	' Token: 0x04001E62 RID: 7778
	Public startTimer As Boolean

	' Token: 0x04001E63 RID: 7779
	Public moving As Boolean

	' Token: 0x04001E64 RID: 7780
	Public lastSideHit As AirshipCrabLevelGems.SideHit

	' Token: 0x04001E65 RID: 7781
	Private properties As LevelProperties.AirshipCrab.Gems

	' Token: 0x04001E66 RID: 7782
	Private damageDealer As DamageDealer

	' Token: 0x04001E67 RID: 7783
	Private pink As Color

	' Token: 0x04001E68 RID: 7784
	Private velocity As Vector3

	' Token: 0x04001E69 RID: 7785
	Private startPos As Vector3

	' Token: 0x04001E6A RID: 7786
	Private collisionPoint As Vector3

	' Token: 0x04001E6B RID: 7787
	Private getCollisionPoint As Boolean

	' Token: 0x04001E6C RID: 7788
	Private currentMovement As Coroutine

	' Token: 0x020004D2 RID: 1234
	Public Enum SideHit
		' Token: 0x04001E6E RID: 7790
		Top
		' Token: 0x04001E6F RID: 7791
		Bottom
		' Token: 0x04001E70 RID: 7792
		Left
		' Token: 0x04001E71 RID: 7793
		Right
		' Token: 0x04001E72 RID: 7794
		None
	End Enum
End Class
