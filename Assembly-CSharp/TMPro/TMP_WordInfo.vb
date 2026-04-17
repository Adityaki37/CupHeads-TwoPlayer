Imports System

Namespace TMPro
	' Token: 0x02000CA9 RID: 3241
	Public Structure TMP_WordInfo
		' Token: 0x06005196 RID: 20886 RVA: 0x00299FC4 File Offset: 0x002983C4
		Public Function GetWord() As String
			Dim text As String = String.Empty
			Dim characterInfo As TMP_CharacterInfo() = Me.textComponent.textInfo.characterInfo
			For i As Integer = Me.firstCharacterIndex To Me.lastCharacterIndex + 1 - 1
				text += characterInfo(i).character
			Next
			Return text
		End Function

		' Token: 0x04005486 RID: 21638
		Public textComponent As TMP_Text

		' Token: 0x04005487 RID: 21639
		Public firstCharacterIndex As Integer

		' Token: 0x04005488 RID: 21640
		Public lastCharacterIndex As Integer

		' Token: 0x04005489 RID: 21641
		Public characterCount As Integer
	End Structure
End Namespace
