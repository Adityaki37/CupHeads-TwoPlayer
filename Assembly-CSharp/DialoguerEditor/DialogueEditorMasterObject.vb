Imports System
Imports System.Collections.Generic
Imports DialoguerCore
Imports UnityEngine

Namespace DialoguerEditor
	' Token: 0x02000B44 RID: 2884
	<Serializable()>
	Public Class DialogueEditorMasterObject
		' Token: 0x060045C2 RID: 17858 RVA: 0x0024C964 File Offset: 0x0024AD64
		Public Sub New()
			Me.dialogues = New List(Of DialogueEditorDialogueObject)()
			Me.globals = New DialogueEditorGlobalVariablesContainer()
			Me.themes = New DialogueEditorThemesContainer()
			Me.selectorScrollPosition = Vector2.zero
			Me.currentDialogueId = -1
		End Sub

		' Token: 0x17000638 RID: 1592
		' (get) Token: 0x060045C3 RID: 17859 RVA: 0x0024C9B1 File Offset: 0x0024ADB1
		Public ReadOnly Property count As Integer
			Get
				Return Me.dialogues.Count
			End Get
		End Property

		' Token: 0x17000639 RID: 1593
		' (get) Token: 0x060045C4 RID: 17860 RVA: 0x0024C9BE File Offset: 0x0024ADBE
		' (set) Token: 0x060045C5 RID: 17861 RVA: 0x0024C9C6 File Offset: 0x0024ADC6
		Public Property currentDialogueId As Integer
			Get
				Return Me.__currentDialogueId
			End Get
			Set(value As Integer)
				Me.__currentDialogueId = Mathf.Clamp(value, 0, Me.count - 1)
			End Set
		End Property

		' Token: 0x060045C6 RID: 17862 RVA: 0x0024C9E0 File Offset: 0x0024ADE0
		Public Sub addDialogue(count As Integer)
			For i As Integer = 0 To count - 1
				Dim count2 As Integer = Me.dialogues.Count
				Me.dialogues.Add(New DialogueEditorDialogueObject())
				Me.dialogues(count2).id = count2
				Me.currentDialogueId = Me.dialogues(count2).id
			Next
		End Sub

		' Token: 0x060045C7 RID: 17863 RVA: 0x0024CA44 File Offset: 0x0024AE44
		Public Sub removeDialogue(removeCount As Integer)
			If Me.count < 1 Then
				Return
			End If
			For i As Integer = 0 To removeCount - 1
				Dim num As Integer = Me.dialogues.Count - 1
				Me.dialogues.RemoveAt(num)
			Next
			Me.currentDialogueId = Me.currentDialogueId
		End Sub

		' Token: 0x060045C8 RID: 17864 RVA: 0x0024CA96 File Offset: 0x0024AE96
		Public Function getThemeNames() As String()
			Return Me.getThemeNames(False)
		End Function

		' Token: 0x060045C9 RID: 17865 RVA: 0x0024CAA0 File Offset: 0x0024AEA0
		Public Function getThemeNames(includeId As Boolean) As String()
			Dim array As String() = New String(Me.themes.themes.Count - 1) {}
			For i As Integer = 0 To Me.themes.themes.Count - 1
				array(i) = String.Empty
				Dim array3 As String()
				If includeId Then
					Dim array2 As String() = array
					array3 = array2
					Dim num As Integer = i
					Dim num2 As Integer = num
					array2(num) = array3(num2) + Me.themes.themes(i).id + " "
				End If
				Dim array4 As String() = array
				array3 = array4
				Dim num3 As Integer = i
				Dim num4 As Integer = num3
				array4(num3) = array3(num4) + Me.themes.themes(i).name
			Next
			Return array
		End Function

		' Token: 0x060045CA RID: 17866 RVA: 0x0024CB4C File Offset: 0x0024AF4C
		Public Function getDialoguerData() As DialoguerData
			Dim list As List(Of Boolean) = New List(Of Boolean)()
			Dim list2 As List(Of Single) = New List(Of Single)()
			Dim list3 As List(Of String) = New List(Of String)()
			For i As Integer = 0 To Me.globals.booleans.variables.Count - 1
				Dim flag As Boolean
				If Not Boolean.TryParse(Me.globals.booleans.variables(i).variable, flag) Then
				End If
				list.Add(flag)
			Next
			For j As Integer = 0 To Me.globals.floats.variables.Count - 1
				Dim num As Single
				If Not Single.TryParse(Me.globals.floats.variables(j).variable, num) Then
				End If
				list2.Add(num)
			Next
			For k As Integer = 0 To Me.globals.strings.variables.Count - 1
				list3.Add(Me.globals.strings.variables(k).variable)
			Next
			Dim dialoguerGlobalVariables As DialoguerGlobalVariables = New DialoguerGlobalVariables(list, list2, list3)
			Dim list4 As List(Of DialoguerDialogue) = New List(Of DialoguerDialogue)()
			For l As Integer = 0 To Me.dialogues.Count - 1
				Dim dialogueEditorDialogueObject As DialogueEditorDialogueObject = Me.dialogues(l)
				Dim list5 As List(Of AbstractDialoguePhase) = New List(Of AbstractDialoguePhase)()
				For m As Integer = 0 To dialogueEditorDialogueObject.phases.Count - 1
					Dim dialogueEditorPhaseObject As DialogueEditorPhaseObject = dialogueEditorDialogueObject.phases(m)
					Select Case dialogueEditorPhaseObject.type
						Case DialogueEditorPhaseTypes.TextPhase
							list5.Add(New TextPhase(dialogueEditorPhaseObject.text, dialogueEditorPhaseObject.theme, dialogueEditorPhaseObject.newWindow, dialogueEditorPhaseObject.name, dialogueEditorPhaseObject.portrait, dialogueEditorPhaseObject.metadata, dialogueEditorPhaseObject.audio, dialogueEditorPhaseObject.audioDelay, dialogueEditorPhaseObject.rect, dialogueEditorPhaseObject.outs, Nothing, dialogueEditorDialogueObject.id, dialogueEditorPhaseObject.id))
						Case DialogueEditorPhaseTypes.BranchedTextPhase
							list5.Add(New BranchedTextPhase(dialogueEditorPhaseObject.text, dialogueEditorPhaseObject.choices, dialogueEditorPhaseObject.theme, dialogueEditorPhaseObject.newWindow, dialogueEditorPhaseObject.name, dialogueEditorPhaseObject.portrait, dialogueEditorPhaseObject.metadata, dialogueEditorPhaseObject.audio, dialogueEditorPhaseObject.audioDelay, dialogueEditorPhaseObject.rect, dialogueEditorPhaseObject.outs, dialogueEditorDialogueObject.id, dialogueEditorPhaseObject.id))
						Case DialogueEditorPhaseTypes.WaitPhase
							list5.Add(New WaitPhase(dialogueEditorPhaseObject.waitType, dialogueEditorPhaseObject.waitDuration, dialogueEditorPhaseObject.outs))
						Case DialogueEditorPhaseTypes.SetVariablePhase
							list5.Add(New SetVariablePhase(dialogueEditorPhaseObject.variableScope, dialogueEditorPhaseObject.variableType, dialogueEditorPhaseObject.variableId, dialogueEditorPhaseObject.variableSetEquation, dialogueEditorPhaseObject.variableSetValue, dialogueEditorPhaseObject.outs))
						Case DialogueEditorPhaseTypes.ConditionalPhase
							list5.Add(New ConditionalPhase(dialogueEditorPhaseObject.variableScope, dialogueEditorPhaseObject.variableType, dialogueEditorPhaseObject.variableId, dialogueEditorPhaseObject.variableGetEquation, dialogueEditorPhaseObject.variableGetValue, dialogueEditorPhaseObject.outs))
						Case DialogueEditorPhaseTypes.SendMessagePhase
							list5.Add(New SendMessagePhase(dialogueEditorPhaseObject.messageName, dialogueEditorPhaseObject.metadata, dialogueEditorPhaseObject.outs))
						Case DialogueEditorPhaseTypes.EndPhase
							list5.Add(New EndPhase())
						Case Else
							list5.Add(New EmptyPhase())
					End Select
				Next
				Dim list6 As List(Of Boolean) = New List(Of Boolean)()
				For n As Integer = 0 To dialogueEditorDialogueObject.booleans.variables.Count - 1
					Dim flag2 As Boolean
					If Not Boolean.TryParse(dialogueEditorDialogueObject.booleans.variables(n).variable, flag2) Then
					End If
					list6.Add(flag2)
				Next
				Dim list7 As List(Of Single) = New List(Of Single)()
				For num2 As Integer = 0 To dialogueEditorDialogueObject.floats.variables.Count - 1
					Dim num3 As Single
					If Not Single.TryParse(dialogueEditorDialogueObject.floats.variables(num2).variable, num3) Then
					End If
					list7.Add(num3)
				Next
				Dim list8 As List(Of String) = New List(Of String)()
				For num4 As Integer = 0 To dialogueEditorDialogueObject.strings.variables.Count - 1
					list8.Add(dialogueEditorDialogueObject.strings.variables(num4).variable)
				Next
				Dim dialoguerVariables As DialoguerVariables = New DialoguerVariables(list6, list7, list8)
				Dim dialoguerDialogue As DialoguerDialogue = New DialoguerDialogue(dialogueEditorDialogueObject.name, dialogueEditorDialogueObject.startPage, dialoguerVariables, list5)
				list4.Add(dialoguerDialogue)
			Next
			Dim list9 As List(Of DialoguerTheme) = New List(Of DialoguerTheme)()
			For num5 As Integer = 0 To Me.themes.themes.Count - 1
				list9.Add(New DialoguerTheme(Me.themes.themes(num5).name, Me.themes.themes(num5).linkage))
			Next
			Return New DialoguerData(dialoguerGlobalVariables, list4, list9)
		End Function

		' Token: 0x04004BF1 RID: 19441
		Private __currentDialogueId As Integer

		' Token: 0x04004BF2 RID: 19442
		Public generateEnum As Boolean = True

		' Token: 0x04004BF3 RID: 19443
		Public dialogues As List(Of DialogueEditorDialogueObject)

		' Token: 0x04004BF4 RID: 19444
		Public globals As DialogueEditorGlobalVariablesContainer

		' Token: 0x04004BF5 RID: 19445
		Public themes As DialogueEditorThemesContainer

		' Token: 0x04004BF6 RID: 19446
		Public selectorScrollPosition As Vector2
	End Class
End Namespace
