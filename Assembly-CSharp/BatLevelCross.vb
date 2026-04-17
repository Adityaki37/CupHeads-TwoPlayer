Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000505 RID: 1285
Public Class BatLevelCross
	Inherits AbstractCollidableObject

	' Token: 0x060016BC RID: 5820 RVA: 0x000CC9FC File Offset: 0x000CADFC
	Private Sub Start()
		Me.damageDealer = DamageDealer.NewEnemy()
	End Sub

	' Token: 0x060016BD RID: 5821 RVA: 0x000CCA0C File Offset: 0x000CAE0C
	Public Sub Init(pos As Vector2, properties As LevelProperties.Bat.CrossToss, maxCount As Integer, player As AbstractPlayerController)
		MyBase.transform.position = pos
		Me.startPos = pos
		Me.properties = properties
		Me.maxCount = maxCount
		Me.player = player
		Me.FindPlayer()
		MyBase.StartCoroutine(Me.move_cr())
	End Sub

	' Token: 0x060016BE RID: 5822 RVA: 0x000CCA5F File Offset: 0x000CAE5F
	Private Sub Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x060016BF RID: 5823 RVA: 0x000CCA77 File Offset: 0x000CAE77
	Protected Overrides Sub OnCollisionCeiling(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionCeiling(hit, phase)
		If phase = CollisionPhase.Enter Then
			Me.goBack = True
		End If
	End Sub

	' Token: 0x060016C0 RID: 5824 RVA: 0x000CCA8E File Offset: 0x000CAE8E
	Protected Overrides Sub OnCollisionWalls(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionCeiling(hit, phase)
		If phase = CollisionPhase.Enter Then
			Me.goBack = True
		End If
	End Sub

	' Token: 0x060016C1 RID: 5825 RVA: 0x000CCAA5 File Offset: 0x000CAEA5
	Protected Overrides Sub OnCollisionGround(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionCeiling(hit, phase)
		If phase = CollisionPhase.Enter Then
			Me.goBack = True
		End If
	End Sub

	' Token: 0x060016C2 RID: 5826 RVA: 0x000CCABC File Offset: 0x000CAEBC
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		Me.damageDealer.DealDamage(hit)
	End Sub

	' Token: 0x060016C3 RID: 5827 RVA: 0x000CCAD4 File Offset: 0x000CAED4
	Private Iterator Function move_cr() As IEnumerator
		Dim count As Integer = 0
		Dim startAgain As Boolean = False
		While count < Me.maxCount
			If Not Me.goBack Then
				MyBase.transform.position += Me.velocity * Me.properties.projectileSpeed * CupheadTime.FixedDelta
			Else
				startAgain = False
				Dim vector As Vector2 = MyBase.transform.position
				vector = Vector3.MoveTowards(MyBase.transform.position, Me.startPos, Me.properties.projectileSpeed * CupheadTime.Delta)
				MyBase.transform.position = vector
				If MyBase.transform.position = Me.startPos AndAlso Not startAgain Then
					count += 1
					Me.goBack = False
					Me.FindPlayer()
					startAgain = True
				End If
			End If
			Yield Nothing
		End While
		Me.Die()
		Yield Nothing
		Return
	End Function

	' Token: 0x060016C4 RID: 5828 RVA: 0x000CCAF0 File Offset: 0x000CAEF0
	Private Sub FindPlayer()
		Dim num As Single = Me.player.transform.position.x - MyBase.transform.position.x
		Dim num2 As Single = Me.player.transform.position.y - MyBase.transform.position.y
		Dim num3 As Single = Mathf.Atan2(num2, num) * 57.29578F
		MyBase.transform.SetEulerAngles(New Single?(0F), New Single?(0F), New Single?(num3))
		Me.velocity = MyBase.transform.right
	End Sub

	' Token: 0x060016C5 RID: 5829 RVA: 0x000CCB9F File Offset: 0x000CAF9F
	Private Sub Die()
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub

	' Token: 0x0400200D RID: 8205
	Private properties As LevelProperties.Bat.CrossToss

	' Token: 0x0400200E RID: 8206
	Private player As AbstractPlayerController

	' Token: 0x0400200F RID: 8207
	Private damageDealer As DamageDealer

	' Token: 0x04002010 RID: 8208
	Private velocity As Vector3

	' Token: 0x04002011 RID: 8209
	Private startPos As Vector3

	' Token: 0x04002012 RID: 8210
	Private maxCount As Integer

	' Token: 0x04002013 RID: 8211
	Private goBack As Boolean
End Class
