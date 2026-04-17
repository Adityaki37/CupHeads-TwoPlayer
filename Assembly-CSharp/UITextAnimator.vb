Imports System
Imports System.Collections
Imports TMPro
Imports UnityEngine
Imports UnityEngine.UI

' Token: 0x02000389 RID: 905
Public Class UITextAnimator
	Inherits AbstractMonoBehaviour

	' Token: 0x06000ABA RID: 2746 RVA: 0x000800EC File Offset: 0x0007E4EC
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.text = MyBase.GetComponent(Of Text)()
		Dim flag As Boolean = Me.text Is Nothing
		Dim flag2 As Boolean = flag
		Me.useTMP = flag
		If flag2 Then
			Me.tmp_text = MyBase.GetComponent(Of TMP_Text)()
			Me.textString = Me.tmp_text.text
		Else
			Me.textString = Me.text.text
		End If
	End Sub

	' Token: 0x06000ABB RID: 2747 RVA: 0x00080158 File Offset: 0x0007E558
	Private Sub Start()
		MyBase.StartCoroutine(Me.anim_cr())
	End Sub

	' Token: 0x06000ABC RID: 2748 RVA: 0x00080167 File Offset: 0x0007E567
	Public Sub SetString(s As String)
		Me.textString = s
	End Sub

	' Token: 0x06000ABD RID: 2749 RVA: 0x00080170 File Offset: 0x0007E570
	Private Iterator Function anim_cr() As IEnumerator
		If Me.useTMP Then
			While True
				Me.tmp_text.text = String.Empty
				For i As Integer = 0 To Me.textString.Length - 1
					Dim tmp_Text As TMP_Text = Me.tmp_text
					Dim text As String = tmp_Text.text
					tmp_Text.text = String.Concat(New Object() { text, "<size=", Me.tmp_text.fontSize + CSng(Global.UnityEngine.Random.Range(-1, 1)), ">", Me.textString(i).ToString(), "</size>" })
				Next
				Yield New WaitForSeconds(Me.frameDelay)
			End While
		Else
			While True
				Me.text.text = String.Empty
				For j As Integer = 0 To Me.textString.Length - 1
					Dim text2 As Text = Me.text
					Dim text As String = text2.text
					text2.text = String.Concat(New Object() { text, "<size=", Me.text.fontSize + Global.UnityEngine.Random.Range(-1, 1), ">", Me.textString(j).ToString(), "</size>" })
				Next
				Yield New WaitForSeconds(Me.frameDelay)
			End While
		End If
		Return
	End Function

	' Token: 0x0400148B RID: 5259
	Private Const DIFFERENCE As Integer = 1

	' Token: 0x0400148C RID: 5260
	<SerializeField()>
	Private frameDelay As Single = 0.07F

	' Token: 0x0400148D RID: 5261
	Private text As Text

	' Token: 0x0400148E RID: 5262
	Private tmp_text As TMP_Text

	' Token: 0x0400148F RID: 5263
	Private textString As String

	' Token: 0x04001490 RID: 5264
	Private useTMP As Boolean
End Class
