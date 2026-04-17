Imports System
Imports UnityEngine

' Token: 0x020009A3 RID: 2467
Public Class MapUIInteractionDialogue
	Inherits AbstractUIInteractionDialogue

	' Token: 0x060039E2 RID: 14818 RVA: 0x0020EEE0 File Offset: 0x0020D2E0
	Public Shared Function Create(properties As AbstractUIInteractionDialogue.Properties, player As PlayerInput, offset As Vector2) As MapUIInteractionDialogue
		Dim mapUIInteractionDialogue As MapUIInteractionDialogue = Global.UnityEngine.[Object].Instantiate(Of MapUIInteractionDialogue)(Map.Current.MapResources.mapUIInteractionDialogue)
		properties.text = String.Empty
		mapUIInteractionDialogue.Init(properties, player, offset)
		Return mapUIInteractionDialogue
	End Function

	' Token: 0x170004B3 RID: 1203
	' (get) Token: 0x060039E3 RID: 14819 RVA: 0x0020EF17 File Offset: 0x0020D317
	Protected Overrides ReadOnly Property PreferredWidth As Single
		Get
			Return Me.tmpText.preferredWidth + Me.glyph.preferredWidth + 10F
		End Get
	End Property

	' Token: 0x060039E4 RID: 14820 RVA: 0x0020EF36 File Offset: 0x0020D336
	Protected Overrides Sub Awake()
		MyBase.Awake()
		MyBase.transform.SetParent(MapUI.Current.sceneCanvas.transform)
		MyBase.transform.ResetLocalTransforms()
	End Sub

	' Token: 0x060039E5 RID: 14821 RVA: 0x0020EF63 File Offset: 0x0020D363
	Private Sub Update()
		Me.UpdatePos()
	End Sub

	' Token: 0x060039E6 RID: 14822 RVA: 0x0020EF6C File Offset: 0x0020D36C
	Private Sub UpdatePos()
		If Me.target Is Nothing Then
			Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
			Return
		End If
		Dim vector As Vector2 = Me.target.position + Me.dialogueOffset
		MyBase.transform.position = vector
	End Sub

	' Token: 0x040041D6 RID: 16854
	Private Const OFFSET_GLYPH As Single = 10F
End Class
