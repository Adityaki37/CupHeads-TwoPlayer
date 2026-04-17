Imports System
Imports UnityEngine

' Token: 0x0200056C RID: 1388
Public Class ClownLevelRiders
	Inherits AbstractCollidableObject

	' Token: 0x06001A3A RID: 6714 RVA: 0x000F00A8 File Offset: 0x000EE4A8
	Private Sub Start()
		Me.damageDealer = DamageDealer.NewEnemy()
		If Me.inFront Then
			MyBase.animator.SetBool("InFront", True)
		Else
			MyBase.animator.SetBool("InFront", False)
		End If
	End Sub

	' Token: 0x06001A3B RID: 6715 RVA: 0x000F00E7 File Offset: 0x000EE4E7
	Private Sub Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x06001A3C RID: 6716 RVA: 0x000F00FF File Offset: 0x000EE4FF
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x06001A3D RID: 6717 RVA: 0x000F0120 File Offset: 0x000EE520
	Public Sub BackLayers(cartLayer As Integer)
		Me.sprites = MyBase.GetComponentsInChildren(Of SpriteRenderer)()
		For i As Integer = 0 To Me.sprites.Length - 1
			Me.sprites(i).sortingLayerName = "Background"
			Me.sprites(i).sortingOrder = cartLayer
		Next
	End Sub

	' Token: 0x06001A3E RID: 6718 RVA: 0x000F0174 File Offset: 0x000EE574
	Public Sub FrontLayers(cartLayer As Integer)
		Me.sprites = MyBase.GetComponentsInChildren(Of SpriteRenderer)()
		For i As Integer = 0 To Me.sprites.Length - 1
			If Me.sprites(i) Is Me.backRider OrElse Me.sprites(i) Is Me.backSeat Then
				Me.sprites(i).sortingLayerName = "Default"
				Me.sprites(i).sortingOrder = 10 - i
			Else
				Me.sprites(i).sortingLayerName = "Player"
				Me.sprites(i).sortingOrder = cartLayer
			End If
		Next
	End Sub

	' Token: 0x04002356 RID: 9046
	<SerializeField()>
	Private backSeat As SpriteRenderer

	' Token: 0x04002357 RID: 9047
	<SerializeField()>
	Private backRider As SpriteRenderer

	' Token: 0x04002358 RID: 9048
	Public inFront As Boolean

	' Token: 0x04002359 RID: 9049
	Private damageDealer As DamageDealer

	' Token: 0x0400235A RID: 9050
	Private sprites As SpriteRenderer()
End Class
