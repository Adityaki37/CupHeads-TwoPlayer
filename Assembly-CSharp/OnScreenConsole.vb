Imports System
Imports System.Collections.Generic
Imports System.Text
Imports UnityEngine

' Token: 0x02000B3F RID: 2879
Public Class OnScreenConsole
	Inherits MonoBehaviour

	' Token: 0x060045B5 RID: 17845 RVA: 0x0024C452 File Offset: 0x0024A852
	Private Sub Awake()
		Global.UnityEngine.[Object].DontDestroyOnLoad(MyBase.gameObject)
		Me.logger = New OnScreenConsole.OnScreenConsoleLogger()
	End Sub

	' Token: 0x060045B6 RID: 17846 RVA: 0x0024C46C File Offset: 0x0024A86C
	Private Sub OnGUI()
		If Me.style Is Nothing Then
			Me.style = New GUIStyle(GUI.skin.GetStyle("Box"))
			Me.style.alignment = TextAnchor.LowerLeft
			Me.style.wordWrap = True
		End If
		For Each text As String In Me.logger.logQueue
			Dim text2 As String = text
			If text.Length > OnScreenConsole.MaximumStringLength Then
				text2 = text.Substring(0, OnScreenConsole.MaximumStringLength)
			End If
			Me.builder.AppendLine(text2)
		Next
		If Me.builder.Length > 0 Then
			Me.builder.Length -= 1
		End If
		Dim num As Integer = CInt((OnScreenConsole.Size.x * CSng(Screen.width)))
		Dim num2 As Integer = CInt((OnScreenConsole.Size.y * CSng(Screen.height)))
		GUI.Box(New Rect(CSng((Screen.width - num)), CSng((Screen.height - num2)), CSng(num), CSng(num2)), Me.builder.ToString(), Me.style)
		Me.builder.Length = 0
	End Sub

	' Token: 0x04004BDC RID: 19420
	Private Shared Size As Vector2 = New Vector2(0.5F, 0.4F)

	' Token: 0x04004BDD RID: 19421
	Private Shared MaximumStringLength As Integer = 500

	' Token: 0x04004BDE RID: 19422
	Private logger As OnScreenConsole.OnScreenConsoleLogger

	' Token: 0x04004BDF RID: 19423
	Private style As GUIStyle

	' Token: 0x04004BE0 RID: 19424
	Private builder As StringBuilder = New StringBuilder()

	' Token: 0x02000B40 RID: 2880
	Private Class OnScreenConsoleLogger
		Implements ILogHandler

		' Token: 0x060045B8 RID: 17848 RVA: 0x0024C5E0 File Offset: 0x0024A9E0
		Public Sub New()
			Global.UnityEngine.Debug.unityLogger.logHandler = Me
		End Sub

		' Token: 0x060045B9 RID: 17849 RVA: 0x0024C613 File Offset: 0x0024AA13
		Public Sub LogFormat(logType As LogType, context As Global.UnityEngine.[Object], format As String, ParamArray args As Object()) Implements UnityEngine.ILogHandler.LogFormat
			Me.addToQueue(String.Format("[{0}, {1}] {2}", logType, Time.frameCount, String.Format(format, args)))
			Me.defaultLogHandler.LogFormat(logType, context, format, args)
		End Sub

		' Token: 0x060045BA RID: 17850 RVA: 0x0024C64D File Offset: 0x0024AA4D
		Public Sub LogException(exception As Exception, context As Global.UnityEngine.[Object]) Implements UnityEngine.ILogHandler.LogException
			Me.defaultLogHandler.LogException(exception, context)
		End Sub

		' Token: 0x060045BB RID: 17851 RVA: 0x0024C65C File Offset: 0x0024AA5C
		Private Sub addToQueue(value As String)
			If Me.logQueue.Count = OnScreenConsole.OnScreenConsoleLogger.QueueSize Then
				Me.logQueue.Dequeue()
			End If
			Me.logQueue.Enqueue(value)
		End Sub

		' Token: 0x04004BE1 RID: 19425
		Private Shared QueueSize As Integer = 15

		' Token: 0x04004BE2 RID: 19426
		Public logQueue As Queue(Of String) = New Queue(Of String)(OnScreenConsole.OnScreenConsoleLogger.QueueSize)

		' Token: 0x04004BE3 RID: 19427
		Private defaultLogHandler As ILogHandler = Global.UnityEngine.Debug.unityLogger.logHandler
	End Class
End Class
