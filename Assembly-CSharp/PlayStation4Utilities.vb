Imports System
Imports System.Runtime.InteropServices

' Token: 0x020009CF RID: 2511
Public Module PlayStation4Utilities
	' Token: 0x06003AF5 RID: 15093
	Private Declare Function get_system_service_param Lib "GetParam" (param As Integer, <System.Runtime.InteropServices.OutAttribute()> ByRef value As Integer) As Integer

	' Token: 0x06003AF6 RID: 15094 RVA: 0x00212B84 File Offset: 0x00210F84
	Public Function GetSystemServiceParam(param As Integer) As Integer
		Dim num2 As Integer
		Dim num As Integer = PlayStation4Utilities.get_system_service_param(param, num2)
		If num <> PlayStation4Utilities.SCE_OK Then
			Throw New Exception("Error getting param. Result code: " + num)
		End If
		Return num2
	End Function

	' Token: 0x040042B1 RID: 17073
	Public SCE_OK As Integer

	' Token: 0x040042B2 RID: 17074
	Public SCE_SYSTEM_SERVICE_PARAM_ID_ENTER_BUTTON_ASSIGN As Integer = 1000

	' Token: 0x040042B3 RID: 17075
	Public SCE_SYSTEM_PARAM_ENTER_BUTTON_ASSIGN_CIRCLE As Integer

	' Token: 0x040042B4 RID: 17076
	Public SCE_SYSTEM_PARAM_ENTER_BUTTON_ASSIGN_CROSS As Integer = 1
End Module
