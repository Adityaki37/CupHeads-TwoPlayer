Imports System
Imports System.ComponentModel
Imports System.Text.RegularExpressions
Imports Rewired.Platforms
Imports Rewired.Utils
Imports Rewired.Utils.Interfaces
Imports UnityEngine

Namespace Rewired
	' Token: 0x02000C59 RID: 3161
	<EditorBrowsable(EditorBrowsableState.Never)>
	Public NotInheritable Class InputManager
		Inherits InputManager_Base

		' Token: 0x06004E65 RID: 20069 RVA: 0x0027A090 File Offset: 0x00278490
		Protected Overrides Sub DetectPlatform()
			Me.editorPlatform = EditorPlatform.None
			Me.platform = Platform.Unknown
			Me.webplayerPlatform = WebplayerPlatform.None
			Me.isEditor = False
			Dim text As String = If(SystemInfo.deviceName, String.Empty)
			Dim text2 As String = If(SystemInfo.deviceModel, String.Empty)
			Me.platform = Platform.Windows
		End Sub

		' Token: 0x06004E66 RID: 20070 RVA: 0x0027A0E4 File Offset: 0x002784E4
		Protected Overrides Sub CheckRecompile()
		End Sub

		' Token: 0x06004E67 RID: 20071 RVA: 0x0027A0E6 File Offset: 0x002784E6
		Protected Overrides Function GetExternalTools() As IExternalTools
			Return New ExternalTools()
		End Function

		' Token: 0x06004E68 RID: 20072 RVA: 0x0027A0ED File Offset: 0x002784ED
		Private Function CheckDeviceName(searchPattern As String, deviceName As String, deviceModel As String) As Boolean
			Return Regex.IsMatch(deviceName, searchPattern, RegexOptions.IgnoreCase) OrElse Regex.IsMatch(deviceModel, searchPattern, RegexOptions.IgnoreCase)
		End Function
	End Class
End Namespace
