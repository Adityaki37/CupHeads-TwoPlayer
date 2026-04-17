Imports System

Namespace TMPro
	' Token: 0x02000CA8 RID: 3240
	Public Structure TMP_LinkInfo
		' Token: 0x06005193 RID: 20883 RVA: 0x00299EEC File Offset: 0x002982EC
		Friend Sub SetLinkID(text As Char(), startIndex As Integer, length As Integer)
			If Me.linkID Is Nothing OrElse Me.linkID.Length < length Then
				Me.linkID = New Char(length - 1) {}
			End If
			For i As Integer = 0 To length - 1
				Me.linkID(i) = text(startIndex + i)
			Next
		End Sub

		' Token: 0x06005194 RID: 20884 RVA: 0x00299F40 File Offset: 0x00298340
		Public Function GetLinkText() As String
			Dim text As String = String.Empty
			Dim textInfo As TMP_TextInfo = Me.textComponent.textInfo
			For i As Integer = Me.linkTextfirstCharacterIndex To Me.linkTextfirstCharacterIndex + Me.linkTextLength - 1
				text += textInfo.characterInfo(i).character
			Next
			Return text
		End Function

		' Token: 0x06005195 RID: 20885 RVA: 0x00299FA0 File Offset: 0x002983A0
		Public Function GetLinkID() As String
			If Me.textComponent Is Nothing Then
				Return String.Empty
			End If
			Return New String(Me.linkID)
		End Function

		' Token: 0x0400547F RID: 21631
		Public textComponent As TMP_Text

		' Token: 0x04005480 RID: 21632
		Public hashCode As Integer

		' Token: 0x04005481 RID: 21633
		Public linkIdFirstCharacterIndex As Integer

		' Token: 0x04005482 RID: 21634
		Public linkIdLength As Integer

		' Token: 0x04005483 RID: 21635
		Public linkTextfirstCharacterIndex As Integer

		' Token: 0x04005484 RID: 21636
		Public linkTextLength As Integer

		' Token: 0x04005485 RID: 21637
		Friend linkID As Char()
	End Structure
End Namespace
