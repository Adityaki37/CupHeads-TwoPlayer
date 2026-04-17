Imports System
Imports System.IO
Imports System.Xml.Serialization
Imports DialoguerEditor
Imports UnityEngine

Namespace DialoguerCore
	' Token: 0x02000B6B RID: 2923
	Public Class DialoguerDataManager
		' Token: 0x06004686 RID: 18054 RVA: 0x0024E4B8 File Offset: 0x0024C8B8
		Public Shared Sub Initialize()
			Dim data As DialogueEditorMasterObject = TryCast(Resources.Load("dialoguer_data_object"), DialogueEditorMasterObjectWrapper).data
			DialoguerDataManager._data = data.getDialoguerData()
		End Sub

		' Token: 0x06004687 RID: 18055 RVA: 0x0024E4E8 File Offset: 0x0024C8E8
		Public Shared Function GetGlobalVariablesState() As String
			Dim xmlSerializer As XmlSerializer = New XmlSerializer(GetType(DialoguerGlobalVariables))
			Dim stringWriter As StringWriter = New StringWriter()
			xmlSerializer.Serialize(stringWriter, DialoguerDataManager._data.globalVariables)
			Return stringWriter.ToString()
		End Function

		' Token: 0x06004688 RID: 18056 RVA: 0x0024E522 File Offset: 0x0024C922
		Public Shared Sub LoadGlobalVariablesState(globalVariablesXml As String)
			DialoguerDataManager._data.loadGlobalVariablesState(globalVariablesXml)
		End Sub

		' Token: 0x06004689 RID: 18057 RVA: 0x0024E52F File Offset: 0x0024C92F
		Public Shared Function GetGlobalFloat(floatId As Integer) As Single
			Return DialoguerDataManager._data.globalVariables.floats(floatId)
		End Function

		' Token: 0x0600468A RID: 18058 RVA: 0x0024E546 File Offset: 0x0024C946
		Public Shared Sub SetGlobalFloat(floatId As Integer, floatValue As Single)
			DialoguerDataManager._data.globalVariables.floats(floatId) = floatValue
		End Sub

		' Token: 0x0600468B RID: 18059 RVA: 0x0024E55E File Offset: 0x0024C95E
		Public Shared Function GetGlobalBoolean(booleanId As Integer) As Boolean
			Return DialoguerDataManager._data.globalVariables.booleans(booleanId)
		End Function

		' Token: 0x0600468C RID: 18060 RVA: 0x0024E575 File Offset: 0x0024C975
		Public Shared Sub SetGlobalBoolean(booleanId As Integer, booleanValue As Boolean)
			DialoguerDataManager._data.globalVariables.booleans(booleanId) = booleanValue
		End Sub

		' Token: 0x0600468D RID: 18061 RVA: 0x0024E58D File Offset: 0x0024C98D
		Public Shared Function GetGlobalString(stringId As Integer) As String
			Return DialoguerDataManager._data.globalVariables.strings(stringId)
		End Function

		' Token: 0x0600468E RID: 18062 RVA: 0x0024E5A4 File Offset: 0x0024C9A4
		Public Shared Sub SetGlobalString(stringId As Integer, stringValue As String)
			DialoguerDataManager._data.globalVariables.strings(stringId) = stringValue
		End Sub

		' Token: 0x0600468F RID: 18063 RVA: 0x0024E5BC File Offset: 0x0024C9BC
		Public Shared Function GetDialogueById(dialogueId As Integer) As DialoguerDialogue
			If DialoguerDataManager._data.dialogues.Count <= dialogueId Then
				Return Nothing
			End If
			Return DialoguerDataManager._data.dialogues(dialogueId)
		End Function

		' Token: 0x04004C6A RID: 19562
		Private Shared _data As DialoguerData
	End Class
End Namespace
