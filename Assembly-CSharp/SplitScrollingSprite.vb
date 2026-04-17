Imports System
Imports UnityEngine

' Token: 0x02000B1C RID: 2844
Public Class SplitScrollingSprite
	Inherits ScrollingSprite

	' Token: 0x060044DF RID: 17631 RVA: 0x00247288 File Offset: 0x00245688
	Protected Overrides Sub Start()
		MyBase.Start()
		For Each spriteRenderer As SpriteRenderer In MyBase.copyRenderers
			If Not Me.ignoreSelfWhenHandlingSplitSprites OrElse Not(spriteRenderer.gameObject Is MyBase.gameObject) Then
				For Each sprite As Sprite In Me.splitSprites
					Dim gameObject As GameObject = New GameObject(sprite.name)
					Dim spriteRenderer2 As SpriteRenderer = gameObject.AddComponent(Of SpriteRenderer)()
					spriteRenderer2.sprite = sprite
					spriteRenderer2.sortingLayerID = spriteRenderer.sortingLayerID
					spriteRenderer2.sortingOrder = spriteRenderer.sortingOrder
					gameObject.transform.SetParent(spriteRenderer.transform, False)
					gameObject.transform.localPosition = Me.splitOffset
				Next
			End If
		Next
	End Sub

	' Token: 0x04004AAB RID: 19115
	<SerializeField()>
	Private ignoreSelfWhenHandlingSplitSprites As Boolean

	' Token: 0x04004AAC RID: 19116
	<SerializeField()>
	Private splitOffset As Vector2

	' Token: 0x04004AAD RID: 19117
	<SerializeField()>
	Private splitSprites As Sprite()
End Class
