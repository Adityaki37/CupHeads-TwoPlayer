Imports System
Imports System.Collections.Generic
Imports UnityEngine

Namespace DialoguerEditor
	' Token: 0x02000B48 RID: 2888
	Public Class DialogueEditorPhaseType
		' Token: 0x060045D1 RID: 17873 RVA: 0x0024D131 File Offset: 0x0024B531
		Public Sub New(type As DialogueEditorPhaseTypes, name As String, info As String, iconDark As Texture, iconLight As Texture)
			Me.type = type
			Me.name = name
			Me.info = info
			Me.iconDark = iconDark
			Me.iconLight = iconLight
		End Sub

		' Token: 0x060045D2 RID: 17874 RVA: 0x0024D160 File Offset: 0x0024B560
		Public Shared Function getPhases() As Dictionary(Of Integer, DialogueEditorPhaseType)
			Dim dictionary As Dictionary(Of Integer, DialogueEditorPhaseType) = New Dictionary(Of Integer, DialogueEditorPhaseType)()
			Dim dialogueEditorPhaseType As DialogueEditorPhaseType = New DialogueEditorPhaseType(DialogueEditorPhaseTypes.TextPhase, "Text", "A simple text page with one out-path.", DialogueEditorPhaseType.getDarkIcon("textPhase"), DialogueEditorPhaseType.getLightIcon("textPhase"))
			dictionary.Add(0, dialogueEditorPhaseType)
			dialogueEditorPhaseType = New DialogueEditorPhaseType(DialogueEditorPhaseTypes.BranchedTextPhase, "Branched Text", "A text page with multiple, selectable out-paths.", DialogueEditorPhaseType.getDarkIcon("branchedTextPhase"), DialogueEditorPhaseType.getLightIcon("branchedTextPhase"))
			dictionary.Add(1, dialogueEditorPhaseType)
			dialogueEditorPhaseType = New DialogueEditorPhaseType(DialogueEditorPhaseTypes.WaitPhase, "Wait", "Wait X seconds before progressing.", DialogueEditorPhaseType.getDarkIcon("waitPhase"), DialogueEditorPhaseType.getLightIcon("waitPhase"))
			dictionary.Add(2, dialogueEditorPhaseType)
			dialogueEditorPhaseType = New DialogueEditorPhaseType(DialogueEditorPhaseTypes.SetVariablePhase, "Set Variable", "Set a local or global variable.", DialogueEditorPhaseType.getDarkIcon("setVariablePhase"), DialogueEditorPhaseType.getLightIcon("setVariablePhase"))
			dictionary.Add(3, dialogueEditorPhaseType)
			dialogueEditorPhaseType = New DialogueEditorPhaseType(DialogueEditorPhaseTypes.ConditionalPhase, "Condition", "Moves to an out-path based on a condition.", DialogueEditorPhaseType.getDarkIcon("conditionalPhase"), DialogueEditorPhaseType.getLightIcon("conditionalPhase"))
			dictionary.Add(4, dialogueEditorPhaseType)
			dialogueEditorPhaseType = New DialogueEditorPhaseType(DialogueEditorPhaseTypes.SendMessagePhase, "Message Event", "Dispatch an event which can be easily listened to and handled.", DialogueEditorPhaseType.getDarkIcon("sendMessagePhase"), DialogueEditorPhaseType.getLightIcon("sendMessagePhase"))
			dictionary.Add(5, dialogueEditorPhaseType)
			dialogueEditorPhaseType = New DialogueEditorPhaseType(DialogueEditorPhaseTypes.EndPhase, "End", "Ends the dialogue and calls the dialogue's callback.", DialogueEditorPhaseType.getDarkIcon("endPhase"), DialogueEditorPhaseType.getLightIcon("endPhase"))
			dictionary.Add(6, dialogueEditorPhaseType)
			Return dictionary
		End Function

		' Token: 0x060045D3 RID: 17875 RVA: 0x0024D2B0 File Offset: 0x0024B6B0
		Private Shared Function getDarkIcon(icon As String) As Texture
			Dim text As String = "Assets/Dialoguer/DialogueEditor/Textures/GUI/"
			text += "Dark/"
			text = text + "icon_" + icon + ".png"
			Return Nothing
		End Function

		' Token: 0x060045D4 RID: 17876 RVA: 0x0024D2E4 File Offset: 0x0024B6E4
		Private Shared Function getLightIcon(icon As String) As Texture
			Dim text As String = "Assets/Dialoguer/DialogueEditor/Textures/GUI/"
			text += "Light/"
			text = text + "icon_" + icon + ".png"
			Return Nothing
		End Function

		' Token: 0x04004C1C RID: 19484
		Public type As DialogueEditorPhaseTypes

		' Token: 0x04004C1D RID: 19485
		Public name As String

		' Token: 0x04004C1E RID: 19486
		Public info As String

		' Token: 0x04004C1F RID: 19487
		Public iconDark As Texture

		' Token: 0x04004C20 RID: 19488
		Public iconLight As Texture
	End Class
End Namespace
