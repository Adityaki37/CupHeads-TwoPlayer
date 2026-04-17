Imports System
Imports UnityEngine

' Token: 0x0200092E RID: 2350
Public Class MapAudioTimerZone
	Inherits AbstractCollidableObject

	' Token: 0x060036FD RID: 14077 RVA: 0x001FB200 File Offset: 0x001F9600
	Private Sub Start()
		Me.waitTime = Global.UnityEngine.Random.Range(Me.audioDelayRange.minimum, Me.audioDelayRange.maximum)
	End Sub

	' Token: 0x060036FE RID: 14078 RVA: 0x001FB224 File Offset: 0x001F9624
	Private Sub Update()
		If Me.playerCount > 0 Then
			Me.elapsedTime += CupheadTime.Delta
			If Me.elapsedTime > Me.waitTime Then
				AudioManager.Play(Me.audioKey)
				Me.elapsedTime = 0F
				Me.waitTime = Global.UnityEngine.Random.Range(Me.audioDelayRange.minimum, Me.audioDelayRange.maximum)
			End If
		End If
	End Sub

	' Token: 0x060036FF RID: 14079 RVA: 0x001FB29C File Offset: 0x001F969C
	Protected Overrides Sub OnCollision(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollision(hit, phase)
		If Not hit.CompareTag("Player_Map") Then
			Return
		End If
		If phase = CollisionPhase.Enter Then
			Me.playerCount += 1
		ElseIf phase = CollisionPhase.[Exit] Then
			Me.playerCount -= 1
		End If
	End Sub

	' Token: 0x04003F30 RID: 16176
	<SerializeField()>
	Private audioKey As String

	' Token: 0x04003F31 RID: 16177
	<SerializeField()>
	Private audioDelayRange As Rangef

	' Token: 0x04003F32 RID: 16178
	Private playerCount As Integer

	' Token: 0x04003F33 RID: 16179
	Private elapsedTime As Single

	' Token: 0x04003F34 RID: 16180
	Private waitTime As Single
End Class
