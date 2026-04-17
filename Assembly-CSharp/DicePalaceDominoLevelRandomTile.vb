Imports System
Imports UnityEngine

' Token: 0x020005BD RID: 1469
Public Class DicePalaceDominoLevelRandomTile
	Inherits AbstractMonoBehaviour

	' Token: 0x06001C90 RID: 7312 RVA: 0x001050A8 File Offset: 0x001034A8
	Public Sub ChangeTile()
		Dim component As SpriteRenderer = MyBase.GetComponent(Of SpriteRenderer)()
		If component Is Nothing Then
			Return
		End If
		component.sprite = Me.sprites(Global.UnityEngine.Random.Range(0, Me.sprites.Length))
	End Sub

	' Token: 0x0400257D RID: 9597
	Public sprites As Sprite()
End Class
