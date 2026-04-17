Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Diagnostics
Imports Rewired.Utils.Interfaces
Imports UnityEngine
Imports UnityEngine.UI

Namespace Rewired.Utils
	' Token: 0x02000C5A RID: 3162
	<EditorBrowsable(EditorBrowsableState.Never)>
	Public Class ExternalTools
		Implements IExternalTools

		' Token: 0x06004E6A RID: 20074 RVA: 0x0027A10F File Offset: 0x0027850F
		Public Function GetPlatformInitializer() As Object Implements Rewired.Utils.Interfaces.IExternalTools.GetPlatformInitializer
			Return Nothing
		End Function

		' Token: 0x06004E6B RID: 20075 RVA: 0x0027A112 File Offset: 0x00278512
		Public Function GetFocusedEditorWindowTitle() As String Implements Rewired.Utils.Interfaces.IExternalTools.GetFocusedEditorWindowTitle
			Return String.Empty
		End Function

		' Token: 0x06004E6C RID: 20076 RVA: 0x0027A119 File Offset: 0x00278519
		Public Function LinuxInput_IsJoystickPreconfigured(name As String) As Boolean Implements Rewired.Utils.Interfaces.IExternalTools.LinuxInput_IsJoystickPreconfigured
			Return False
		End Function

		' Token: 0x140000F9 RID: 249
		' (add) Token: 0x06004E6D RID: 20077 RVA: 0x0027A11C File Offset: 0x0027851C
		' (remove) Token: 0x06004E6E RID: 20078 RVA: 0x0027A154 File Offset: 0x00278554
		<DebuggerBrowsable(DebuggerBrowsableState.Never)>
		Public Event XboxOneInput_OnGamepadStateChange As Action(Of UInteger, Boolean) Implements Rewired.Utils.Interfaces.IExternalTools.XboxOneInput_OnGamepadStateChange

		' Token: 0x06004E6F RID: 20079 RVA: 0x0027A18A File Offset: 0x0027858A
		Public Function XboxOneInput_GetUserIdForGamepad(id As UInteger) As Integer Implements Rewired.Utils.Interfaces.IExternalTools.XboxOneInput_GetUserIdForGamepad
			Return 0
		End Function

		' Token: 0x06004E70 RID: 20080 RVA: 0x0027A18D File Offset: 0x0027858D
		Public Function XboxOneInput_GetControllerId(unityJoystickId As UInteger) As ULong Implements Rewired.Utils.Interfaces.IExternalTools.XboxOneInput_GetControllerId
			Return 0UL
		End Function

		' Token: 0x06004E71 RID: 20081 RVA: 0x0027A191 File Offset: 0x00278591
		Public Function XboxOneInput_IsGamepadActive(unityJoystickId As UInteger) As Boolean Implements Rewired.Utils.Interfaces.IExternalTools.XboxOneInput_IsGamepadActive
			Return False
		End Function

		' Token: 0x06004E72 RID: 20082 RVA: 0x0027A194 File Offset: 0x00278594
		Public Function XboxOneInput_GetControllerType(xboxControllerId As ULong) As String Implements Rewired.Utils.Interfaces.IExternalTools.XboxOneInput_GetControllerType
			Return String.Empty
		End Function

		' Token: 0x06004E73 RID: 20083 RVA: 0x0027A19B File Offset: 0x0027859B
		Public Function XboxOneInput_GetJoystickId(xboxControllerId As ULong) As UInteger Implements Rewired.Utils.Interfaces.IExternalTools.XboxOneInput_GetJoystickId
			Return 0UI
		End Function

		' Token: 0x06004E74 RID: 20084 RVA: 0x0027A19E File Offset: 0x0027859E
		Public Sub XboxOne_Gamepad_UpdatePlugin() Implements Rewired.Utils.Interfaces.IExternalTools.XboxOne_Gamepad_UpdatePlugin
		End Sub

		' Token: 0x06004E75 RID: 20085 RVA: 0x0027A1A0 File Offset: 0x002785A0
		Public Function XboxOne_Gamepad_SetGamepadVibration(xboxOneJoystickId As ULong, leftMotor As Single, rightMotor As Single, leftTriggerLevel As Single, rightTriggerLevel As Single) As Boolean Implements Rewired.Utils.Interfaces.IExternalTools.XboxOne_Gamepad_SetGamepadVibration
			Return False
		End Function

		' Token: 0x06004E76 RID: 20086 RVA: 0x0027A1A3 File Offset: 0x002785A3
		Public Sub XboxOne_Gamepad_PulseVibrateMotor(xboxOneJoystickId As ULong, motorInt As Integer, startLevel As Single, endLevel As Single, durationMS As ULong) Implements Rewired.Utils.Interfaces.IExternalTools.XboxOne_Gamepad_PulseVibrateMotor
		End Sub

		' Token: 0x06004E77 RID: 20087 RVA: 0x0027A1A5 File Offset: 0x002785A5
		Public Function PS4Input_GetLastAcceleration(id As Integer) As Vector3 Implements Rewired.Utils.Interfaces.IExternalTools.PS4Input_GetLastAcceleration
			Return Vector3.zero
		End Function

		' Token: 0x06004E78 RID: 20088 RVA: 0x0027A1AC File Offset: 0x002785AC
		Public Function PS4Input_GetLastGyro(id As Integer) As Vector3 Implements Rewired.Utils.Interfaces.IExternalTools.PS4Input_GetLastGyro
			Return Vector3.zero
		End Function

		' Token: 0x06004E79 RID: 20089 RVA: 0x0027A1B3 File Offset: 0x002785B3
		Public Function PS4Input_GetLastOrientation(id As Integer) As Vector4 Implements Rewired.Utils.Interfaces.IExternalTools.PS4Input_GetLastOrientation
			Return Vector4.zero
		End Function

		' Token: 0x06004E7A RID: 20090 RVA: 0x0027A1BA File Offset: 0x002785BA
		Public Sub PS4Input_GetLastTouchData(id As Integer, <System.Runtime.InteropServices.OutAttribute()> ByRef touchNum As Integer, <System.Runtime.InteropServices.OutAttribute()> ByRef touch0x As Integer, <System.Runtime.InteropServices.OutAttribute()> ByRef touch0y As Integer, <System.Runtime.InteropServices.OutAttribute()> ByRef touch0id As Integer, <System.Runtime.InteropServices.OutAttribute()> ByRef touch1x As Integer, <System.Runtime.InteropServices.OutAttribute()> ByRef touch1y As Integer, <System.Runtime.InteropServices.OutAttribute()> ByRef touch1id As Integer) Implements Rewired.Utils.Interfaces.IExternalTools.PS4Input_GetLastTouchData
			touchNum = 0
			touch0x = 0
			touch0y = 0
			touch0id = 0
			touch1x = 0
			touch1y = 0
			touch1id = 0
		End Sub

		' Token: 0x06004E7B RID: 20091 RVA: 0x0027A1D6 File Offset: 0x002785D6
		Public Sub PS4Input_GetPadControllerInformation(id As Integer, <System.Runtime.InteropServices.OutAttribute()> ByRef touchpixelDensity As Single, <System.Runtime.InteropServices.OutAttribute()> ByRef touchResolutionX As Integer, <System.Runtime.InteropServices.OutAttribute()> ByRef touchResolutionY As Integer, <System.Runtime.InteropServices.OutAttribute()> ByRef analogDeadZoneLeft As Integer, <System.Runtime.InteropServices.OutAttribute()> ByRef analogDeadZoneright As Integer, <System.Runtime.InteropServices.OutAttribute()> ByRef connectionType As Integer) Implements Rewired.Utils.Interfaces.IExternalTools.PS4Input_GetPadControllerInformation
			touchpixelDensity = 0F
			touchResolutionX = 0
			touchResolutionY = 0
			analogDeadZoneLeft = 0
			analogDeadZoneright = 0
			connectionType = 0
		End Sub

		' Token: 0x06004E7C RID: 20092 RVA: 0x0027A1F2 File Offset: 0x002785F2
		Public Sub PS4Input_PadSetMotionSensorState(id As Integer, bEnable As Boolean) Implements Rewired.Utils.Interfaces.IExternalTools.PS4Input_PadSetMotionSensorState
		End Sub

		' Token: 0x06004E7D RID: 20093 RVA: 0x0027A1F4 File Offset: 0x002785F4
		Public Sub PS4Input_PadSetTiltCorrectionState(id As Integer, bEnable As Boolean) Implements Rewired.Utils.Interfaces.IExternalTools.PS4Input_PadSetTiltCorrectionState
		End Sub

		' Token: 0x06004E7E RID: 20094 RVA: 0x0027A1F6 File Offset: 0x002785F6
		Public Sub PS4Input_PadSetAngularVelocityDeadbandState(id As Integer, bEnable As Boolean) Implements Rewired.Utils.Interfaces.IExternalTools.PS4Input_PadSetAngularVelocityDeadbandState
		End Sub

		' Token: 0x06004E7F RID: 20095 RVA: 0x0027A1F8 File Offset: 0x002785F8
		Public Sub PS4Input_PadSetLightBar(id As Integer, red As Integer, green As Integer, blue As Integer) Implements Rewired.Utils.Interfaces.IExternalTools.PS4Input_PadSetLightBar
		End Sub

		' Token: 0x06004E80 RID: 20096 RVA: 0x0027A1FA File Offset: 0x002785FA
		Public Sub PS4Input_PadResetLightBar(id As Integer) Implements Rewired.Utils.Interfaces.IExternalTools.PS4Input_PadResetLightBar
		End Sub

		' Token: 0x06004E81 RID: 20097 RVA: 0x0027A1FC File Offset: 0x002785FC
		Public Sub PS4Input_PadSetVibration(id As Integer, largeMotor As Integer, smallMotor As Integer) Implements Rewired.Utils.Interfaces.IExternalTools.PS4Input_PadSetVibration
		End Sub

		' Token: 0x06004E82 RID: 20098 RVA: 0x0027A1FE File Offset: 0x002785FE
		Public Sub PS4Input_PadResetOrientation(id As Integer) Implements Rewired.Utils.Interfaces.IExternalTools.PS4Input_PadResetOrientation
		End Sub

		' Token: 0x06004E83 RID: 20099 RVA: 0x0027A200 File Offset: 0x00278600
		Public Function PS4Input_PadIsConnected(id As Integer) As Boolean Implements Rewired.Utils.Interfaces.IExternalTools.PS4Input_PadIsConnected
			Return False
		End Function

		' Token: 0x06004E84 RID: 20100 RVA: 0x0027A203 File Offset: 0x00278603
		Public Function PS4Input_PadGetUsersDetails(slot As Integer) As Object Implements Rewired.Utils.Interfaces.IExternalTools.PS4Input_PadGetUsersDetails
			Return Nothing
		End Function

		' Token: 0x06004E85 RID: 20101 RVA: 0x0027A206 File Offset: 0x00278606
		Public Function PS4Input_GetLastMoveAcceleration(id As Integer, index As Integer) As Vector3 Implements Rewired.Utils.Interfaces.IExternalTools.PS4Input_GetLastMoveAcceleration
			Return Vector3.zero
		End Function

		' Token: 0x06004E86 RID: 20102 RVA: 0x0027A20D File Offset: 0x0027860D
		Public Function PS4Input_GetLastMoveGyro(id As Integer, index As Integer) As Vector3 Implements Rewired.Utils.Interfaces.IExternalTools.PS4Input_GetLastMoveGyro
			Return Vector3.zero
		End Function

		' Token: 0x06004E87 RID: 20103 RVA: 0x0027A214 File Offset: 0x00278614
		Public Function PS4Input_MoveGetButtons(id As Integer, index As Integer) As Integer Implements Rewired.Utils.Interfaces.IExternalTools.PS4Input_MoveGetButtons
			Return 0
		End Function

		' Token: 0x06004E88 RID: 20104 RVA: 0x0027A217 File Offset: 0x00278617
		Public Function PS4Input_MoveGetAnalogButton(id As Integer, index As Integer) As Integer Implements Rewired.Utils.Interfaces.IExternalTools.PS4Input_MoveGetAnalogButton
			Return 0
		End Function

		' Token: 0x06004E89 RID: 20105 RVA: 0x0027A21A File Offset: 0x0027861A
		Public Function PS4Input_MoveIsConnected(id As Integer, index As Integer) As Boolean Implements Rewired.Utils.Interfaces.IExternalTools.PS4Input_MoveIsConnected
			Return False
		End Function

		' Token: 0x06004E8A RID: 20106 RVA: 0x0027A21D File Offset: 0x0027861D
		Public Function PS4Input_MoveGetUsersMoveHandles(maxNumberControllers As Integer, primaryHandles As Integer(), secondaryHandles As Integer()) As Integer Implements Rewired.Utils.Interfaces.IExternalTools.PS4Input_MoveGetUsersMoveHandles
			Return 0
		End Function

		' Token: 0x06004E8B RID: 20107 RVA: 0x0027A220 File Offset: 0x00278620
		Public Function PS4Input_MoveGetUsersMoveHandles(maxNumberControllers As Integer, primaryHandles As Integer()) As Integer Implements Rewired.Utils.Interfaces.IExternalTools.PS4Input_MoveGetUsersMoveHandles
			Return 0
		End Function

		' Token: 0x06004E8C RID: 20108 RVA: 0x0027A223 File Offset: 0x00278623
		Public Function PS4Input_MoveGetUsersMoveHandles(maxNumberControllers As Integer) As Integer Implements Rewired.Utils.Interfaces.IExternalTools.PS4Input_MoveGetUsersMoveHandles
			Return 0
		End Function

		' Token: 0x06004E8D RID: 20109 RVA: 0x0027A226 File Offset: 0x00278626
		Public Function PS4Input_MoveGetControllerInputForTracking() As IntPtr Implements Rewired.Utils.Interfaces.IExternalTools.PS4Input_MoveGetControllerInputForTracking
			Return IntPtr.Zero
		End Function

		' Token: 0x06004E8E RID: 20110 RVA: 0x0027A22D File Offset: 0x0027862D
		Public Sub GetDeviceVIDPIDs(<System.Runtime.InteropServices.OutAttribute()> ByRef vids As List(Of Integer), <System.Runtime.InteropServices.OutAttribute()> ByRef pids As List(Of Integer)) Implements Rewired.Utils.Interfaces.IExternalTools.GetDeviceVIDPIDs
			vids = New List(Of Integer)()
			pids = New List(Of Integer)()
		End Sub

		' Token: 0x06004E8F RID: 20111 RVA: 0x0027A23D File Offset: 0x0027863D
		Public Function UnityUI_Graphic_GetRaycastTarget(graphic As Object) As Boolean Implements Rewired.Utils.Interfaces.IExternalTools.UnityUI_Graphic_GetRaycastTarget
			Return Not(TryCast(graphic, Graphic) Is Nothing) AndAlso TryCast(graphic, Graphic).raycastTarget
		End Function

		' Token: 0x06004E90 RID: 20112 RVA: 0x0027A25D File Offset: 0x0027865D
		Public Sub UnityUI_Graphic_SetRaycastTarget(graphic As Object, value As Boolean) Implements Rewired.Utils.Interfaces.IExternalTools.UnityUI_Graphic_SetRaycastTarget
			If TryCast(graphic, Graphic) Is Nothing Then
				Return
			End If
			TryCast(graphic, Graphic).raycastTarget = value
		End Sub

		' Token: 0x170007F0 RID: 2032
		' (get) Token: 0x06004E91 RID: 20113 RVA: 0x0027A27D File Offset: 0x0027867D
		Public ReadOnly Property UnityInput_IsTouchPressureSupported As Boolean Implements Rewired.Utils.Interfaces.IExternalTools.UnityInput_IsTouchPressureSupported
			Get
				Return Input.touchPressureSupported
			End Get
		End Property

		' Token: 0x06004E92 RID: 20114 RVA: 0x0027A284 File Offset: 0x00278684
		Public Function UnityInput_GetTouchPressure(ByRef touch As Touch) As Single Implements Rewired.Utils.Interfaces.IExternalTools.UnityInput_GetTouchPressure
			Return touch.pressure
		End Function

		' Token: 0x06004E93 RID: 20115 RVA: 0x0027A28C File Offset: 0x0027868C
		Public Function UnityInput_GetTouchMaximumPossiblePressure(ByRef touch As Touch) As Single Implements Rewired.Utils.Interfaces.IExternalTools.UnityInput_GetTouchMaximumPossiblePressure
			Return touch.maximumPossiblePressure
		End Function
	End Class
End Namespace
