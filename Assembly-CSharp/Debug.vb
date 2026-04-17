Imports System
Imports System.Diagnostics
Imports UnityEngine
Imports UnityEngine.Internal

' Token: 0x0200041F RID: 1055
Public Module Debug
	' Token: 0x17000266 RID: 614
	' (get) Token: 0x06000F30 RID: 3888 RVA: 0x00097129 File Offset: 0x00095529
	' (set) Token: 0x06000F31 RID: 3889 RVA: 0x00097130 File Offset: 0x00095530
	Public Property developerConsoleVisible As Boolean
		Get
			Return Global.UnityEngine.Debug.developerConsoleVisible
		End Get
		Set(value As Boolean)
			Global.UnityEngine.Debug.developerConsoleVisible = value
		End Set
	End Property

	' Token: 0x17000267 RID: 615
	' (get) Token: 0x06000F32 RID: 3890 RVA: 0x00097138 File Offset: 0x00095538
	Public ReadOnly Property isDebugBuild As Boolean
		Get
			Return Global.UnityEngine.Debug.isDebugBuild
		End Get
	End Property

	' Token: 0x06000F33 RID: 3891 RVA: 0x0009713F File Offset: 0x0009553F
	<Conditional("UNITY_ASSERTIONS")>
	Public Sub Assert(condition As Boolean)
	End Sub

	' Token: 0x06000F34 RID: 3892 RVA: 0x00097141 File Offset: 0x00095541
	<Conditional("UNITY_ASSERTIONS")>
	Public Sub Assert(condition As Boolean, message As String)
	End Sub

	' Token: 0x06000F35 RID: 3893 RVA: 0x00097143 File Offset: 0x00095543
	<Conditional("UNITY_ASSERTIONS")>
	Public Sub AssertFormat(condition As Boolean, format As String, ParamArray args As Object())
	End Sub

	' Token: 0x06000F36 RID: 3894 RVA: 0x00097145 File Offset: 0x00095545
	Public Sub Break()
		Global.UnityEngine.Debug.Break()
	End Sub

	' Token: 0x06000F37 RID: 3895 RVA: 0x0009714C File Offset: 0x0009554C
	Public Sub ClearDeveloperConsole()
		Global.UnityEngine.Debug.ClearDeveloperConsole()
	End Sub

	' Token: 0x06000F38 RID: 3896 RVA: 0x00097153 File Offset: 0x00095553
	Public Sub DebugBreak()
		Global.UnityEngine.Debug.DebugBreak()
	End Sub

	' Token: 0x06000F39 RID: 3897 RVA: 0x0009715A File Offset: 0x0009555A
	Public Sub DrawLine(start As Vector3, [end] As Vector3)
		Global.UnityEngine.Debug.DrawLine(start, [end])
	End Sub

	' Token: 0x06000F3A RID: 3898 RVA: 0x00097163 File Offset: 0x00095563
	Public Sub DrawLine(start As Vector3, [end] As Vector3, color As Color)
		Global.UnityEngine.Debug.DrawLine(start, [end], color)
	End Sub

	' Token: 0x06000F3B RID: 3899 RVA: 0x0009716D File Offset: 0x0009556D
	Public Sub DrawLine(start As Vector3, [end] As Vector3, color As Color, duration As Single)
		Global.UnityEngine.Debug.DrawLine(start, [end], color, duration)
	End Sub

	' Token: 0x06000F3C RID: 3900 RVA: 0x00097178 File Offset: 0x00095578
	Public Sub DrawLine(start As Vector3, [end] As Vector3, <DefaultValue("Color.white")> color As Color, <DefaultValue("0.0f")> duration As Single, <DefaultValue("true")> depthTest As Boolean)
		Global.UnityEngine.Debug.DrawLine(start, [end], color, duration, depthTest)
	End Sub

	' Token: 0x06000F3D RID: 3901 RVA: 0x00097185 File Offset: 0x00095585
	Public Sub DrawRay(start As Vector3, dir As Vector3)
		Global.UnityEngine.Debug.DrawRay(start, dir)
	End Sub

	' Token: 0x06000F3E RID: 3902 RVA: 0x0009718E File Offset: 0x0009558E
	Public Sub DrawRay(start As Vector3, dir As Vector3, color As Color)
		Global.UnityEngine.Debug.DrawRay(start, dir, color)
	End Sub

	' Token: 0x06000F3F RID: 3903 RVA: 0x00097198 File Offset: 0x00095598
	Public Sub DrawRay(start As Vector3, dir As Vector3, color As Color, duration As Single)
		Global.UnityEngine.Debug.DrawRay(start, dir, color, duration)
	End Sub

	' Token: 0x06000F40 RID: 3904 RVA: 0x000971A3 File Offset: 0x000955A3
	Public Sub DrawRay(start As Vector3, dir As Vector3, <DefaultValue("Color.white")> color As Color, <DefaultValue("0.0f")> duration As Single, <DefaultValue("true")> depthTest As Boolean)
		Global.UnityEngine.Debug.DrawRay(start, dir, color, duration, depthTest)
	End Sub

	' Token: 0x06000F41 RID: 3905 RVA: 0x000971B0 File Offset: 0x000955B0
	Public Sub LogInfo(message As Object, Optional context As Global.UnityEngine.[Object] = Nothing)
		Global.UnityEngine.Debug.Log(message, context)
	End Sub

	' Token: 0x06000F42 RID: 3906 RVA: 0x000971B9 File Offset: 0x000955B9
	Public Sub LogInfoCat(ParamArray args As Object())
		Global.UnityEngine.Debug.Log(String.Concat(args))
	End Sub

	' Token: 0x06000F43 RID: 3907 RVA: 0x000971C6 File Offset: 0x000955C6
	<Conditional("VERBOSE")>
	Public Sub Log(message As Object, Optional context As Global.UnityEngine.[Object] = Nothing)
		Global.UnityEngine.Debug.Log(message, context)
	End Sub

	' Token: 0x06000F44 RID: 3908 RVA: 0x000971CF File Offset: 0x000955CF
	<Conditional("VERBOSE")>
	Public Sub LogCat(ParamArray args As Object())
		Global.UnityEngine.Debug.Log(String.Concat(args))
	End Sub

	' Token: 0x06000F45 RID: 3909 RVA: 0x000971DC File Offset: 0x000955DC
	Public Sub LogError(message As Object, Optional context As Global.UnityEngine.[Object] = Nothing)
		Global.UnityEngine.Debug.LogError(message, context)
	End Sub

	' Token: 0x06000F46 RID: 3910 RVA: 0x000971E5 File Offset: 0x000955E5
	Public Sub LogErrorCat(ParamArray args As Object())
		Global.UnityEngine.Debug.LogError(String.Concat(args))
	End Sub

	' Token: 0x06000F47 RID: 3911 RVA: 0x000971F2 File Offset: 0x000955F2
	Public Sub LogErrorFormat(format As String, ParamArray args As Object())
		Global.UnityEngine.Debug.LogErrorFormat(format, args)
	End Sub

	' Token: 0x06000F48 RID: 3912 RVA: 0x000971FB File Offset: 0x000955FB
	Public Sub LogErrorFormat(context As Global.UnityEngine.[Object], format As String, ParamArray args As Object())
		Global.UnityEngine.Debug.LogErrorFormat(context, format, args)
	End Sub

	' Token: 0x06000F49 RID: 3913 RVA: 0x00097205 File Offset: 0x00095605
	Public Sub LogException(exception As Exception)
		Global.UnityEngine.Debug.LogException(exception)
	End Sub

	' Token: 0x06000F4A RID: 3914 RVA: 0x0009720D File Offset: 0x0009560D
	Public Sub LogException(exception As Exception, context As Global.UnityEngine.[Object])
		Global.UnityEngine.Debug.LogException(exception, context)
	End Sub

	' Token: 0x06000F4B RID: 3915 RVA: 0x00097216 File Offset: 0x00095616
	<Conditional("VERBOSE")>
	Public Sub LogFormat(format As String, ParamArray args As Object())
		Global.UnityEngine.Debug.LogFormat(format, args)
	End Sub

	' Token: 0x06000F4C RID: 3916 RVA: 0x0009721F File Offset: 0x0009561F
	<Conditional("VERBOSE")>
	Public Sub LogFormat(context As Global.UnityEngine.[Object], format As String, ParamArray args As Object())
		Global.UnityEngine.Debug.LogFormat(context, format, args)
	End Sub

	' Token: 0x06000F4D RID: 3917 RVA: 0x00097229 File Offset: 0x00095629
	<Conditional("VERBOSE")>
	Public Sub LogWarning(message As Object, Optional context As Global.UnityEngine.[Object] = Nothing)
		Global.UnityEngine.Debug.LogWarning(message, context)
	End Sub

	' Token: 0x06000F4E RID: 3918 RVA: 0x00097232 File Offset: 0x00095632
	<Conditional("VERBOSE")>
	Public Sub LogWarningCat(ParamArray args As Object())
		Global.UnityEngine.Debug.LogWarning(String.Concat(args))
	End Sub

	' Token: 0x06000F4F RID: 3919 RVA: 0x0009723F File Offset: 0x0009563F
	<Conditional("VERBOSE")>
	Public Sub LogWarningFormat(format As String, ParamArray args As Object())
		Global.UnityEngine.Debug.LogWarningFormat(format, args)
	End Sub

	' Token: 0x06000F50 RID: 3920 RVA: 0x00097248 File Offset: 0x00095648
	<Conditional("VERBOSE")>
	Public Sub LogWarningFormat(context As Global.UnityEngine.[Object], format As String, ParamArray args As Object())
		Global.UnityEngine.Debug.LogWarningFormat(context, format, args)
	End Sub
End Module
