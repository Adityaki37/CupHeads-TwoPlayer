Imports System
Imports UnityEngine

' Token: 0x02000A42 RID: 2626
Public Class PlayerScreenEffectController
	Inherits AbstractMonoBehaviour

	' Token: 0x06003E9C RID: 16028 RVA: 0x00225A88 File Offset: 0x00223E88
	Private Sub Update()
		If Not Me.dontCenter Then
			Me.UpdateToCamera()
		End If
	End Sub

	' Token: 0x06003E9D RID: 16029 RVA: 0x00225A9B File Offset: 0x00223E9B
	Private Sub LateUpdate()
		If Not Me.dontCenter Then
			Me.UpdateToCamera()
		End If
	End Sub

	' Token: 0x06003E9E RID: 16030 RVA: 0x00225AB0 File Offset: 0x00223EB0
	Private Sub UpdateToCamera()
		Dim main As Camera = Camera.main
		Dim transform As Transform = main.transform
		MyBase.transform.position = transform.position
		MyBase.transform.localScale = Vector3.one * (main.orthographicSize / 360F)
		MyBase.transform.rotation = transform.rotation
	End Sub

	' Token: 0x06003E9F RID: 16031 RVA: 0x00225B0D File Offset: 0x00223F0D
	Public Sub SetSpriteLayer(index As Integer, layer As SpriteLayer)
		Me.spriteRenderers(index).sortingLayerName = layer.ToString()
	End Sub

	' Token: 0x06003EA0 RID: 16032 RVA: 0x00225B29 File Offset: 0x00223F29
	Public Sub SetSpriteOrder(index As Integer, order As Integer)
		Me.spriteRenderers(index).sortingOrder = order
	End Sub

	' Token: 0x06003EA1 RID: 16033 RVA: 0x00225B3C File Offset: 0x00223F3C
	Public Sub ResetSprites()
		For i As Integer = 0 To Me.spriteRenderers.Length - 1
			Me.spriteRenderers(i).sortingOrder = -2010 - i
			Me.spriteRenderers(i).sortingLayerName = "Player"
			Me.spriteRenderers(i).sprite = Nothing
		Next
	End Sub

	' Token: 0x040045AF RID: 17839
	<SerializeField()>
	Private dontCenter As Boolean

	' Token: 0x040045B0 RID: 17840
	<SerializeField()>
	Private spriteRenderers As SpriteRenderer()
End Class
