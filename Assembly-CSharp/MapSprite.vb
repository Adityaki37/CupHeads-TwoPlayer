Imports System
Imports UnityEngine

' Token: 0x02000971 RID: 2417
Public Class MapSprite
	Inherits AbstractPausableComponent

	' Token: 0x1700048B RID: 1163
	' (get) Token: 0x06003849 RID: 14409 RVA: 0x00098849 File Offset: 0x00096C49
	Protected Overridable ReadOnly Property ChangesDepth As Boolean
		Get
			Return True
		End Get
	End Property

	' Token: 0x0600384A RID: 14410 RVA: 0x0009884C File Offset: 0x00096C4C
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.SetLayer(MyBase.GetComponent(Of SpriteRenderer)())
		For Each spriteRenderer As SpriteRenderer In MyBase.GetComponentsInChildren(Of SpriteRenderer)()
			Me.SetLayer(spriteRenderer)
		Next
	End Sub

	' Token: 0x0600384B RID: 14411 RVA: 0x00098891 File Offset: 0x00096C91
	Protected Sub SetLayer(renderer As SpriteRenderer)
		If Not Me.ChangesDepth OrElse renderer Is Nothing Then
			Return
		End If
		renderer.sortingLayerName = "Map"
		renderer.sortingOrder = 0
	End Sub

	' Token: 0x0600384C RID: 14412 RVA: 0x000988C0 File Offset: 0x00096CC0
	Protected Overridable Sub Update()
		Dim position As Vector3 = MyBase.transform.position
		MyBase.transform.position = New Vector3(position.x, position.y, position.y + Me.zOffset)
	End Sub

	' Token: 0x0400402C RID: 16428
	<SerializeField()>
	Protected zOffset As Single
End Class
