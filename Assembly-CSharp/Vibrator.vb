Imports System
Imports Rewired
Imports UnityEngine

' Token: 0x02000474 RID: 1140
Public Class Vibrator
	Inherits AbstractMonoBehaviour

	' Token: 0x170002BA RID: 698
	' (get) Token: 0x06001175 RID: 4469 RVA: 0x000A5C78 File Offset: 0x000A4078
	' (set) Token: 0x06001176 RID: 4470 RVA: 0x000A5C7F File Offset: 0x000A407F
	Public Shared Property Current As Vibrator

	' Token: 0x06001177 RID: 4471 RVA: 0x000A5C87 File Offset: 0x000A4087
	Public Shared Sub Vibrate(amount As Single, time As Single, player As PlayerId)
		Vibrator.Current._Vibrate(amount, time, player)
	End Sub

	' Token: 0x06001178 RID: 4472 RVA: 0x000A5C96 File Offset: 0x000A4096
	Public Shared Sub StopVibrating(player As PlayerId)
		Vibrator.Current._StopVibrating(player)
	End Sub

	' Token: 0x06001179 RID: 4473 RVA: 0x000A5CA3 File Offset: 0x000A40A3
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Vibrator.Current = Me
	End Sub

	' Token: 0x0600117A RID: 4474 RVA: 0x000A5CB4 File Offset: 0x000A40B4
	Private Sub Update()
		If Not SettingsData.Data.canVibrate Then
			Return
		End If
		For i As Integer = 0 To 2 - 1
			Dim num As Single = Me.durationsLeft(i)
			Dim num2 As Single = Me.currentVibrations(i)
			num -= CupheadTime.Delta
			If num <= 0F Then
				If num2 > 0F Then
					Me.currentVibrations(i) = 0F
					Me._StopVibrating(CType(i, PlayerId))
				End If
			ElseIf num2 <= 0F Then
				Me._StopVibrating(CType(i, PlayerId))
			Else
				Me.durationsLeft(i) = num
			End If
		Next
	End Sub

	' Token: 0x0600117B RID: 4475 RVA: 0x000A5D58 File Offset: 0x000A4158
	Private Sub _Vibrate(amount As Single, time As Single, playerId As PlayerId)
		If Not SettingsData.Data.canVibrate Then
			Return
		End If
		If amount <= 0F OrElse time <= 0F Then
			Me._StopVibrating(playerId)
			Return
		End If
		Me.currentVibrations(CInt(playerId)) = amount
		Me.durationsLeft(CInt(playerId)) = time
		Dim player As Player = ReInput.players.GetPlayer(CInt(playerId))
		For Each joystick As Joystick In player.controllers.Joysticks
			If joystick.supportsVibration Then
				joystick.SetVibration(amount * Vibrator.PlatformMultiplier, amount * Vibrator.PlatformMultiplier)
			End If
		Next
	End Sub

	' Token: 0x0600117C RID: 4476 RVA: 0x000A5E20 File Offset: 0x000A4220
	Private Sub _StopVibrating(playerId As PlayerId)
		If Not SettingsData.Data.canVibrate Then
			Return
		End If
		Dim player As Player = ReInput.players.GetPlayer(CInt(playerId))
		For Each joystick As Joystick In player.controllers.Joysticks
			joystick.StopVibration()
		Next
	End Sub

	' Token: 0x04001B0A RID: 6922
	Private Shared PlatformMultiplier As Single = 1F

	' Token: 0x04001B0C RID: 6924
	Private vibrateCoroutine As Coroutine

	' Token: 0x04001B0D RID: 6925
	Private durationsLeft As Single() = New Single(1) {}

	' Token: 0x04001B0E RID: 6926
	Private currentVibrations As Single() = New Single(1) {}
End Class
