Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x0200056A RID: 1386
Public Class ClownLevelLights
	Inherits AbstractPausableComponent

	' Token: 0x06001A32 RID: 6706 RVA: 0x000EFAEA File Offset: 0x000EDEEA
	Private Sub Start()
		Me.redLight.enabled = False
		Me.greenLight.enabled = False
		MyBase.StartCoroutine(Me.warning_lights_cr())
	End Sub

	' Token: 0x06001A33 RID: 6707 RVA: 0x000EFB11 File Offset: 0x000EDF11
	Public Sub StartWarningLights()
		AudioManager.PlayLoop("clown_warning_lights_loop")
		Me.emitAudioFromObject.Add("clown_warning_lights_loop")
		Me.redLight.enabled = True
		Me.greenLight.enabled = True
		Me.isOn = True
	End Sub

	' Token: 0x06001A34 RID: 6708 RVA: 0x000EFB4C File Offset: 0x000EDF4C
	Public Sub StopWarningLights()
		AudioManager.[Stop]("clown_warning_lights_loop")
		Me.redLight.enabled = False
		Me.greenLight.enabled = False
		Me.isOn = False
	End Sub

	' Token: 0x06001A35 RID: 6709 RVA: 0x000EFB78 File Offset: 0x000EDF78
	Private Iterator Function warning_lights_cr() As IEnumerator
		Dim t As Single = 0F
		While True
			Me.redLight.color = New Color(1F, 1F, 1F, 1F)
			Me.greenLight.color = New Color(1F, 1F, 1F, 0F)
			If Me.isOn Then
				t = 0F
				While t < 0.083F
					Me.redLight.color = New Color(1F, 1F, 1F, 1F - t / 0.083F)
					Me.greenLight.color = New Color(1F, 1F, 1F, t / 0.083F)
					t += CupheadTime.Delta
					Yield Nothing
				End While
				Me.redLight.color = New Color(1F, 1F, 1F, 0F)
				Me.greenLight.color = New Color(1F, 1F, 1F, 1F)
				t = 0F
				Yield CupheadTime.WaitForSeconds(Me, 0.083F)
				Yield Nothing
				While t < 0.083F
					Me.redLight.color = New Color(1F, 1F, 1F, t / 0.083F)
					Me.greenLight.color = New Color(1F, 1F, 1F, 1F - t / 0.083F)
					t += CupheadTime.Delta
					Yield Nothing
				End While
				Me.redLight.color = New Color(1F, 1F, 1F, 1F)
				Me.greenLight.color = New Color(1F, 1F, 1F, 0F)
				Yield CupheadTime.WaitForSeconds(Me, 0.083F)
				Yield Nothing
			End If
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x04002350 RID: 9040
	<SerializeField()>
	Private redLight As SpriteRenderer

	' Token: 0x04002351 RID: 9041
	<SerializeField()>
	Private greenLight As SpriteRenderer

	' Token: 0x04002352 RID: 9042
	Private Const fadeTime As Single = 0.083F

	' Token: 0x04002353 RID: 9043
	Private isOn As Boolean
End Class
