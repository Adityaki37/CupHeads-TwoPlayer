Imports System
Imports System.Collections.Generic
Imports System.IO
Imports System.Xml
Imports System.Xml.Serialization

Namespace DialoguerCore
	' Token: 0x02000B6D RID: 2925
	Public Class DialoguerData
		' Token: 0x06004694 RID: 18068 RVA: 0x0024E6CE File Offset: 0x0024CACE
		Public Sub New(globalVariables As DialoguerGlobalVariables, dialogues As List(Of DialoguerDialogue), themes As List(Of DialoguerTheme))
			Me.globalVariables = globalVariables
			Me.dialogues = dialogues
			Me.themes = themes
		End Sub

		' Token: 0x06004695 RID: 18069 RVA: 0x0024E6EC File Offset: 0x0024CAEC
		Public Sub loadGlobalVariablesState(globalVariablesXml As String)
			Dim xmlSerializer As XmlSerializer = New XmlSerializer(GetType(DialoguerGlobalVariables))
			Dim xmlReader As XmlReader = XmlReader.Create(New StringReader(globalVariablesXml))
			Dim dialoguerGlobalVariables As DialoguerGlobalVariables = CType(xmlSerializer.Deserialize(xmlReader), DialoguerGlobalVariables)
			For i As Integer = 0 To dialoguerGlobalVariables.booleans.Count - 1
				If i >= Me.globalVariables.booleans.Count Then
					Exit For
				End If
				Me.globalVariables.booleans(i) = dialoguerGlobalVariables.booleans(i)
			Next
			For j As Integer = 0 To dialoguerGlobalVariables.floats.Count - 1
				If j >= Me.globalVariables.floats.Count Then
					Exit For
				End If
				Me.globalVariables.floats(j) = dialoguerGlobalVariables.floats(j)
			Next
			For k As Integer = 0 To dialoguerGlobalVariables.strings.Count - 1
				If k >= Me.globalVariables.strings.Count Then
					Exit For
				End If
				Me.globalVariables.strings(k) = dialoguerGlobalVariables.strings(k)
			Next
		End Sub

		' Token: 0x04004C70 RID: 19568
		Public globalVariables As DialoguerGlobalVariables

		' Token: 0x04004C71 RID: 19569
		Public dialogues As List(Of DialoguerDialogue)

		' Token: 0x04004C72 RID: 19570
		Public themes As List(Of DialoguerTheme)
	End Class
End Namespace
