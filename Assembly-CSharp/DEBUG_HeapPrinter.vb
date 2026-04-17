Imports System
Imports System.Text
Imports UnityEngine
Imports UnityEngine.Profiling

' Token: 0x02000B3B RID: 2875
Public Class DEBUG_HeapPrinter
	Inherits MonoBehaviour

	' Token: 0x060045A6 RID: 17830 RVA: 0x0024BE80 File Offset: 0x0024A280
	Private Sub Awake()
		Global.UnityEngine.[Object].DontDestroyOnLoad(MyBase.gameObject)
	End Sub

	' Token: 0x060045A7 RID: 17831 RVA: 0x0024BE90 File Offset: 0x0024A290
	Private Sub OnGUI()
		If Not Me.styleInitialized Then
			Me.styleInitialized = True
			Me.style = New GUIStyle(GUI.skin.box)
			Me.style.alignment = TextAnchor.MiddleRight
			Me.style.fontStyle = FontStyle.Bold
			Me.style.richText = True
			Me.style.fontSize = 24
		End If
		Dim totalMemory As Long = GC.GetTotalMemory(False)
		Dim num As Long = totalMemory - Me.previousMemory
		Me.counter.Add(num)
		If Me.previousMemory > totalMemory Then
			Me.highlightTimer = 0F
		End If
		Dim num2 As Single = DEBUG_HeapPrinter.Size.x
		Dim num3 As Single = DEBUG_HeapPrinter.Size.y
		Dim text As String = String.Empty
		If Me.highlightTimer < DEBUG_HeapPrinter.HighlightTime Then
			Me.highlightTimer += Time.unscaledDeltaTime
			Me.style.fontSize = DEBUG_HeapPrinter.LargeFontSize
			Me.builder.Append("<color=red>")
			text = "</color>"
			num2 *= 2F
			num3 *= 2F
		Else
			Me.style.fontSize = DEBUG_HeapPrinter.SmallFontSize
		End If
		Dim num4 As Long = totalMemory / 1024L
		Dim num5 As Long = Profiler.GetMonoHeapSizeLong() / 1024L
		Me.builder.Append(num4)
		Me.builder.Append(" / ")
		Me.builder.Append(num5)
		Me.builder.Append(vbLf)
		Me.builder.Append((Me.counter.Average() / 1024F).ToString("F2"))
		Me.builder.Append("kb / frame")
		Me.builder.Append(text)
		GUI.Box(New Rect(CSng(Screen.width) - num2, CSng(Screen.height) - num3, num2, num3), Me.builder.ToString(), Me.style)
		Me.builder.Length = 0
		Me.previousMemory = totalMemory
	End Sub

	' Token: 0x04004BCD RID: 19405
	Private Shared Size As Vector2 = New Vector2(250F, 70F)

	' Token: 0x04004BCE RID: 19406
	Private Shared HighlightTime As Single = 3F

	' Token: 0x04004BCF RID: 19407
	Private Shared SmallFontSize As Integer = 24

	' Token: 0x04004BD0 RID: 19408
	Private Shared LargeFontSize As Integer = 50

	' Token: 0x04004BD1 RID: 19409
	Private Shared CounterSize As Integer = 30

	' Token: 0x04004BD2 RID: 19410
	Private styleInitialized As Boolean

	' Token: 0x04004BD3 RID: 19411
	Private style As GUIStyle

	' Token: 0x04004BD4 RID: 19412
	Private previousMemory As Long = Long.MaxValue

	' Token: 0x04004BD5 RID: 19413
	Private highlightTimer As Single = Single.MaxValue

	' Token: 0x04004BD6 RID: 19414
	Private builder As StringBuilder = New StringBuilder(100)

	' Token: 0x04004BD7 RID: 19415
	Private counter As DEBUG_HeapPrinter.CircularCounter = New DEBUG_HeapPrinter.CircularCounter(DEBUG_HeapPrinter.CounterSize)

	' Token: 0x02000B3C RID: 2876
	Private Class CircularCounter
		' Token: 0x060045A9 RID: 17833 RVA: 0x0024C0D5 File Offset: 0x0024A4D5
		Public Sub New(size As Integer)
			Me.values = New Long(size - 1) {}
		End Sub

		' Token: 0x060045AA RID: 17834 RVA: 0x0024C0E9 File Offset: 0x0024A4E9
		Public Sub Add(value As Long)
			Me.values(Me.currentIndex) = value
			Me.currentIndex += 1
			If Me.currentIndex >= Me.values.Length Then
				Me.currentIndex = 0
			End If
		End Sub

		' Token: 0x060045AB RID: 17835 RVA: 0x0024C124 File Offset: 0x0024A524
		Public Function Average() As Single
			Dim num As Long = 0L
			For i As Integer = 0 To Me.values.Length - 1
				num += Me.values(i)
			Next
			Return CSng(num) / CSng(Me.values.Length)
		End Function

		' Token: 0x04004BD8 RID: 19416
		Private values As Long()

		' Token: 0x04004BD9 RID: 19417
		Private currentIndex As Integer
	End Class
End Class
