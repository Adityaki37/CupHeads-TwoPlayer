Imports System
Imports UnityEngine

' Token: 0x02000B2A RID: 2858
Public Class PatternString
	' Token: 0x06004542 RID: 17730 RVA: 0x00247B00 File Offset: 0x00245F00
	Public Sub New(patternString As String(), Optional randomizeMain As Boolean = True, Optional randomizeSub As Boolean = True)
		Me.mainPatternString = patternString
		Me.mainIndex = If((Not randomizeMain), 0, Global.UnityEngine.Random.Range(0, patternString.Length))
		Me.subPatternString = Me.mainPatternString(Me.mainIndex).Split(New Char() { ","c })
		Me.subIndex = If((Not randomizeSub), 0, Global.UnityEngine.Random.Range(0, Me.subPatternString.Length))
	End Sub

	' Token: 0x06004543 RID: 17731 RVA: 0x00247B78 File Offset: 0x00245F78
	Public Sub New(patternString As String, Optional randomizeSub As Boolean = True)
		Me.mainIndex = 0
		Me.mainPatternString = New String(0) {}
		Me.mainPatternString(0) = patternString
		Me.subPatternString = Me.mainPatternString(0).Split(New Char() { ","c })
		Me.subIndex = If((Not randomizeSub), 0, Global.UnityEngine.Random.Range(0, Me.subPatternString.Length))
	End Sub

	' Token: 0x06004544 RID: 17732 RVA: 0x00247BE8 File Offset: 0x00245FE8
	Public Sub New(patternString As String(), subSubStringSplitter As Char, Optional randomizeMain As Boolean = True, Optional randomizeSub As Boolean = True)
		Me.mainPatternString = patternString
		Me.mainIndex = If((Not randomizeMain), 0, Global.UnityEngine.Random.Range(0, patternString.Length))
		Me.subPatternString = Me.mainPatternString(Me.mainIndex).Split(New Char() { ","c })
		Me.subIndex = If((Not randomizeSub), 0, Global.UnityEngine.Random.Range(0, Me.subPatternString.Length))
		Me.subsubPatternString = Me.subPatternString(Me.subIndex).Split(New Char() { subSubStringSplitter })
		Me.subSubStringSplitter = subSubStringSplitter
	End Sub

	' Token: 0x06004545 RID: 17733 RVA: 0x00247C8A File Offset: 0x0024608A
	Public Function SubStringLength() As Integer
		Return Me.subPatternString.Length
	End Function

	' Token: 0x06004546 RID: 17734 RVA: 0x00247C94 File Offset: 0x00246094
	Public Sub SetMainStringIndex(value As Integer)
		Me.mainIndex = value Mod Me.mainPatternString.Length
	End Sub

	' Token: 0x06004547 RID: 17735 RVA: 0x00247CA6 File Offset: 0x002460A6
	Public Sub SetSubStringIndex(value As Integer)
		Me.subIndex = value Mod Me.subPatternString.Length
	End Sub

	' Token: 0x06004548 RID: 17736 RVA: 0x00247CB8 File Offset: 0x002460B8
	Public Function GetMainStringIndex() As Integer
		Return Me.mainIndex
	End Function

	' Token: 0x06004549 RID: 17737 RVA: 0x00247CC0 File Offset: 0x002460C0
	Public Function GetSubStringIndex() As Integer
		Return Me.subIndex
	End Function

	' Token: 0x0600454A RID: 17738 RVA: 0x00247CC8 File Offset: 0x002460C8
	Public Function GetSubsubstringLetter(index As Integer) As Char
		Return Me.subsubPatternString(index)(0)
	End Function

	' Token: 0x0600454B RID: 17739 RVA: 0x00247CD8 File Offset: 0x002460D8
	Public Function GetSubsubstringFloat(index As Integer) As Single
		Dim num As Single = 0F
		If Parser.FloatTryParse(Me.subsubPatternString(index), num) Then
			Return num
		End If
		Global.Debug.LogError("Syntax Error in" + Me.subsubPatternString, Nothing)
		Return num
	End Function

	' Token: 0x0600454C RID: 17740 RVA: 0x00247D18 File Offset: 0x00246118
	Private Function GetLetter() As Char
		Return Me.subPatternString(Me.subIndex)(0)
	End Function

	' Token: 0x0600454D RID: 17741 RVA: 0x00247D2D File Offset: 0x0024612D
	Public Function PopLetter() As Char
		Me.IncrementString()
		Return Me.GetLetter()
	End Function

	' Token: 0x0600454E RID: 17742 RVA: 0x00247D3B File Offset: 0x0024613B
	Public Function GetString() As String
		Return Me.subPatternString(Me.subIndex)
	End Function

	' Token: 0x0600454F RID: 17743 RVA: 0x00247D4A File Offset: 0x0024614A
	Public Function PopString() As String
		Me.IncrementString()
		Return Me.GetString()
	End Function

	' Token: 0x06004550 RID: 17744 RVA: 0x00247D58 File Offset: 0x00246158
	Public Function GetFloat() As Single
		Dim num As Single = 0F
		If Parser.FloatTryParse(Me.subPatternString(Me.subIndex), num) Then
			Return num
		End If
		Global.Debug.LogError("Syntax Error in" + Me.mainPatternString, Nothing)
		Return num
	End Function

	' Token: 0x06004551 RID: 17745 RVA: 0x00247D9D File Offset: 0x0024619D
	Public Function PopFloat() As Single
		Me.IncrementString()
		Return Me.GetFloat()
	End Function

	' Token: 0x06004552 RID: 17746 RVA: 0x00247DAC File Offset: 0x002461AC
	Private Function GetInt() As Integer
		Dim num As Integer = 0
		If Parser.IntTryParse(Me.subPatternString(Me.subIndex), num) Then
			Return num
		End If
		Global.Debug.LogError("Syntax Error in" + Me.mainPatternString, Nothing)
		Return num
	End Function

	' Token: 0x06004553 RID: 17747 RVA: 0x00247DED File Offset: 0x002461ED
	Public Function PopInt() As Integer
		Me.IncrementString()
		Return Me.GetInt()
	End Function

	' Token: 0x06004554 RID: 17748 RVA: 0x00247DFC File Offset: 0x002461FC
	Public Sub IncrementString()
		If Me.subIndex < Me.subPatternString.Length - 1 Then
			Me.subIndex += 1
		Else
			Me.mainIndex = (Me.mainIndex + 1) Mod Me.mainPatternString.Length
			Me.subIndex = 0
		End If
		Me.subPatternString = Me.mainPatternString(Me.mainIndex).Split(New Char() { ","c })
		If Me.subsubPatternString IsNot Nothing Then
			Me.subsubPatternString = Me.subPatternString(Me.subIndex).Split(New Char() { Me.subSubStringSplitter })
		End If
	End Sub

	' Token: 0x04004AEB RID: 19179
	Private mainIndex As Integer

	' Token: 0x04004AEC RID: 19180
	Private subIndex As Integer

	' Token: 0x04004AED RID: 19181
	Private subSubStringSplitter As Char

	' Token: 0x04004AEE RID: 19182
	Private mainPatternString As String()

	' Token: 0x04004AEF RID: 19183
	Private subPatternString As String()

	' Token: 0x04004AF0 RID: 19184
	Private subsubPatternString As String()
End Class
