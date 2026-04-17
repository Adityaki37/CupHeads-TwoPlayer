Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020009CE RID: 2510
Public Class PlatformHandlingTitleScreenOverride
	' Token: 0x06003AF2 RID: 15090 RVA: 0x00212A83 File Offset: 0x00210E83
	Public Sub New(startScreenLoadData As StartScreen.InitialLoadData)
		Me.startScreenLoadData = startScreenLoadData
	End Sub

	' Token: 0x06003AF3 RID: 15091 RVA: 0x00212A94 File Offset: 0x00210E94
	Public Iterator Function GetTitleScreenOverrideStatus_cr(parent As MonoBehaviour) As IEnumerator
		Yield Nothing
		Yield Nothing
		Me.startScreenLoadData.forceOriginalTitleScreen = SettingsData.Data.forceOriginalTitleScreen
		Return
	End Function

	' Token: 0x040042AE RID: 17070
	Public Shared XboxOneForceOriginalTitleScreenKey As String = "XboxOne_ForceOriginalTitleScreen"

	' Token: 0x040042AF RID: 17071
	Public Shared UWPForceOriginalTitleScreenKey As String = "UWP_ForceOriginalTitleScreen"

	' Token: 0x040042B0 RID: 17072
	Private startScreenLoadData As StartScreen.InitialLoadData
End Class
