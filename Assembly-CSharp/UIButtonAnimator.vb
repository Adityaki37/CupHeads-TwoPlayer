Imports System
Imports System.Collections
Imports System.Text.RegularExpressions
Imports TMPro

' Token: 0x02000386 RID: 902
Public Class UIButtonAnimator
	Inherits AbstractMonoBehaviour

	' Token: 0x17000202 RID: 514
	' (get) Token: 0x06000AAF RID: 2735 RVA: 0x0007FAE6 File Offset: 0x0007DEE6
	' (set) Token: 0x06000AB0 RID: 2736 RVA: 0x0007FAF3 File Offset: 0x0007DEF3
	Protected Property Text As String
		Get
			Return Me.tmpText.text
		End Get
		Set(value As String)
			Me.tmpText.SetText(value)
		End Set
	End Property

	' Token: 0x06000AB1 RID: 2737 RVA: 0x0007FB01 File Offset: 0x0007DF01
	Private Sub Start()
		Me.tmpText = MyBase.GetComponent(Of TextMeshProUGUI)()
		MyBase.StartCoroutine(Me.animate_cr())
	End Sub

	' Token: 0x06000AB2 RID: 2738 RVA: 0x0007FB1C File Offset: 0x0007DF1C
	Private Iterator Function animate_cr() As IEnumerator
		Yield Nothing
		Yield Nothing
		Dim first As String = Me.Text
		Dim second As String = Me.Text
		Dim keys As MatchCollection = Regex.Matches(Me.Text, "{([^}]*)}", RegexOptions.Multiline Or RegexOptions.ExplicitCapture)
		Dim buttons As CupheadButton() = New CupheadButton(keys.Count - 1) {}
		For i As Integer = 0 To keys.Count - 1
			buttons(i) = CType([Enum].Parse(GetType(CupheadButton), keys(i).Value.Substring(1, keys(i).Value.Length - 2)), CupheadButton)
		Next
		For j As Integer = 0 To CupheadInput.pairs.Length - 1
			For k As Integer = 0 To buttons.Length - 1
				Dim inputSymbols As CupheadInput.InputSymbols = CupheadInput.InputSymbolForButton(buttons(k))
				If inputSymbols = CupheadInput.pairs(j).symbol Then
					first = first.Replace("{" + buttons(k).ToString() + "}", CupheadInput.pairs(j).first)
				End If
			Next
		Next
		For l As Integer = 0 To CupheadInput.pairs.Length - 1
			For m As Integer = 0 To buttons.Length - 1
				Dim inputSymbols2 As CupheadInput.InputSymbols = CupheadInput.InputSymbolForButton(buttons(m))
				If inputSymbols2 = CupheadInput.pairs(l).symbol Then
					second = second.Replace("{" + buttons(m).ToString() + "}", CupheadInput.pairs(l).second)
				End If
			Next
		Next
		Me.Text = first
		While True
			Yield CupheadTime.WaitForSeconds(Me, 0.4F)
			Me.Text = second
			Yield CupheadTime.WaitForSeconds(Me, 0.4F)
			Me.Text = first
		End While
		Return
	End Function

	' Token: 0x04001480 RID: 5248
	Private Const FRAME_DELAY As Single = 0.4F

	' Token: 0x04001481 RID: 5249
	Private tmpText As TextMeshProUGUI
End Class
