Imports System
Imports UnityEngine

' Token: 0x02000A49 RID: 2633
Public Class WeaponSparkEffect
	Inherits Effect

	' Token: 0x06003EBD RID: 16061 RVA: 0x002264FC File Offset: 0x002248FC
	Public Sub SetPlayer(player As LevelPlayerController)
		If player.motor.Grounded Then
			Me.player = player
			Dim localScale As Vector3 = MyBase.transform.localScale
			MyBase.transform.parent = player.transform
			MyBase.transform.localScale = localScale
			Me.playerXScale = player.transform.localScale.x
		End If
	End Sub

	' Token: 0x06003EBE RID: 16062 RVA: 0x00226562 File Offset: 0x00224962
	Public Sub BringToFrontOfPlayer()
		MyBase.GetComponent(Of SpriteRenderer)().sortingOrder = 100
	End Sub

	' Token: 0x06003EBF RID: 16063 RVA: 0x00226571 File Offset: 0x00224971
	Private Sub FixedUpdate()
		If Me.player IsNot Nothing AndAlso Not Me.player.motor.Grounded Then
			Me.player = Nothing
			MyBase.transform.parent = Nothing
		End If
	End Sub

	' Token: 0x06003EC0 RID: 16064 RVA: 0x002265AC File Offset: 0x002249AC
	Private Sub LateUpdate()
		If Me.player IsNot Nothing AndAlso Me.player.transform.localScale.x <> Me.playerXScale Then
			MyBase.transform.SetLocalPosition(New Single?(-MyBase.transform.localPosition.x), Nothing, Nothing)
			Me.player = Nothing
			MyBase.transform.parent = Nothing
		End If
	End Sub

	' Token: 0x040045CB RID: 17867
	Private player As LevelPlayerController

	' Token: 0x040045CC RID: 17868
	Private playerXScale As Single
End Class
