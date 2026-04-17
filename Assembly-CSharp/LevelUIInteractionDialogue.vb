Imports System
Imports UnityEngine

' Token: 0x02000487 RID: 1159
Public Class LevelUIInteractionDialogue
	Inherits AbstractUIInteractionDialogue

	' Token: 0x0600121D RID: 4637 RVA: 0x000A89C4 File Offset: 0x000A6DC4
	Public Shared Function Create(properties As AbstractUIInteractionDialogue.Properties, player As PlayerInput, offset As Vector2, Optional glyphOffsetAddition As Single = 0F, Optional tailPosition As LevelUIInteractionDialogue.TailPosition = LevelUIInteractionDialogue.TailPosition.Bottom, Optional playerTarget As Boolean = True) As LevelUIInteractionDialogue
		Dim levelUIInteractionDialogue As LevelUIInteractionDialogue = Global.UnityEngine.[Object].Instantiate(Of LevelUIInteractionDialogue)(Level.Current.LevelResources.levelUIInteractionDialogue)
		levelUIInteractionDialogue.glyphOffsetAddition = glyphOffsetAddition
		levelUIInteractionDialogue.tailPosition = tailPosition
		levelUIInteractionDialogue.Init(properties, player, offset)
		If tailPosition = LevelUIInteractionDialogue.TailPosition.Right Then
			levelUIInteractionDialogue.dialogueOffset = New Vector2(offset.x - levelUIInteractionDialogue.back.sizeDelta.x * 0.5F - 14F, offset.y)
		ElseIf tailPosition = LevelUIInteractionDialogue.TailPosition.Left Then
			levelUIInteractionDialogue.dialogueOffset = New Vector2(offset.x + levelUIInteractionDialogue.back.sizeDelta.x * 0.5F + 14F, offset.y)
		End If
		If Not playerTarget AndAlso LevelUIInteractionDialogue.defaultTarget Is Nothing Then
			LevelUIInteractionDialogue.defaultTarget = GameObject.CreatePrimitive(PrimitiveType.Cube)
			LevelUIInteractionDialogue.defaultTarget.transform.position = Vector3.zero
			LevelUIInteractionDialogue.defaultTarget.transform.localScale = Vector3.zero
			levelUIInteractionDialogue.target = LevelUIInteractionDialogue.defaultTarget.transform
		ElseIf Not playerTarget Then
			levelUIInteractionDialogue.target = LevelUIInteractionDialogue.defaultTarget.transform
		End If
		Return levelUIInteractionDialogue
	End Function

	' Token: 0x170002CD RID: 717
	' (get) Token: 0x0600121E RID: 4638 RVA: 0x000A8AFC File Offset: 0x000A6EFC
	Protected Overrides ReadOnly Property PreferredWidth As Single
		Get
			If Me.tmpText.text.Length = 0 Then
				Return Me.tmpText.preferredWidth + Me.glyph.preferredWidth + 5.3F + Me.glyphOffsetAddition
			End If
			Return Me.tmpText.preferredWidth + Me.glyph.preferredWidth + 27F + Me.glyphOffsetAddition
		End Get
	End Property

	' Token: 0x0600121F RID: 4639 RVA: 0x000A8B67 File Offset: 0x000A6F67
	Protected Overrides Sub Awake()
		MyBase.Awake()
		MyBase.transform.SetParent(LevelHUD.Current.Canvas.transform, False)
	End Sub

	' Token: 0x06001220 RID: 4640 RVA: 0x000A8B8A File Offset: 0x000A6F8A
	Protected Overrides Sub Init(properties As AbstractUIInteractionDialogue.Properties, player As PlayerInput, offset As Vector2)
		MyBase.Init(properties, player, offset)
		Me.UpdatePos()
	End Sub

	' Token: 0x06001221 RID: 4641 RVA: 0x000A8B9B File Offset: 0x000A6F9B
	Private Sub Update()
		Me.UpdatePos()
		Me.UpdateTailPosition()
	End Sub

	' Token: 0x06001222 RID: 4642 RVA: 0x000A8BA9 File Offset: 0x000A6FA9
	Protected Overridable Sub UpdatePos()
		If Me.target IsNot Nothing Then
			MyBase.transform.position = Me.target.position + Me.dialogueOffset
		End If
	End Sub

	' Token: 0x06001223 RID: 4643 RVA: 0x000A8BE8 File Offset: 0x000A6FE8
	Private Sub UpdateTailPosition()
		Dim tailPosition As LevelUIInteractionDialogue.TailPosition = Me.tailPosition
		If tailPosition <> LevelUIInteractionDialogue.TailPosition.Bottom Then
			If tailPosition <> LevelUIInteractionDialogue.TailPosition.Right Then
				If tailPosition = LevelUIInteractionDialogue.TailPosition.Left Then
					Me.leftTail.SetActive(True)
				End If
			Else
				Me.rightTail.SetActive(True)
			End If
		Else
			Me.bottomTail.SetActive(True)
		End If
	End Sub

	' Token: 0x04001B91 RID: 7057
	Private Const TAIL_WIDTH As Single = 14F

	' Token: 0x04001B92 RID: 7058
	Private Const OFFSET_GLYPH As Single = 27F

	' Token: 0x04001B93 RID: 7059
	Private Const OFFSET_GLYPH_ONLY As Single = 5.3F

	' Token: 0x04001B94 RID: 7060
	Private glyphOffsetAddition As Single

	' Token: 0x04001B95 RID: 7061
	Private tailPosition As LevelUIInteractionDialogue.TailPosition

	' Token: 0x04001B96 RID: 7062
	<SerializeField()>
	Private bottomTail As GameObject

	' Token: 0x04001B97 RID: 7063
	<SerializeField()>
	Private leftTail As GameObject

	' Token: 0x04001B98 RID: 7064
	<SerializeField()>
	Private rightTail As GameObject

	' Token: 0x04001B99 RID: 7065
	Private Shared defaultTarget As GameObject

	' Token: 0x02000488 RID: 1160
	Public Enum TailPosition
		' Token: 0x04001B9B RID: 7067
		Right
		' Token: 0x04001B9C RID: 7068
		Left
		' Token: 0x04001B9D RID: 7069
		Bottom
	End Enum
End Class
