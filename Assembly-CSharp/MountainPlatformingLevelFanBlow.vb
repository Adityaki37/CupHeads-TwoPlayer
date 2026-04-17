Imports System
Imports UnityEngine

' Token: 0x020008E3 RID: 2275
Public Class MountainPlatformingLevelFanBlow
	Inherits AbstractCollidableObject

	' Token: 0x06003545 RID: 13637 RVA: 0x001F0B23 File Offset: 0x001EEF23
	Private Sub Start()
		Me.scrollForce = New LevelPlayerMotor.VelocityManager.Force(LevelPlayerMotor.VelocityManager.Force.Type.All, Me.parent.GetSpeed())
	End Sub

	' Token: 0x06003546 RID: 13638 RVA: 0x001F0B3C File Offset: 0x001EEF3C
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If phase <> CollisionPhase.[Exit] Then
			If hit.GetComponent(Of LevelPlayerMotor)() Then
				If Me.parent.fanOn AndAlso Not Me.parent.Dead Then
					Me.FanOn(hit.GetComponent(Of LevelPlayerMotor)())
				Else
					Me.FanOff(hit.GetComponent(Of LevelPlayerMotor)())
				End If
			Else
				Me.FanOff(hit.GetComponent(Of LevelPlayerMotor)())
			End If
		Else
			Me.FanOff(hit.GetComponent(Of LevelPlayerMotor)())
		End If
	End Sub

	' Token: 0x06003547 RID: 13639 RVA: 0x001F0BC7 File Offset: 0x001EEFC7
	Private Sub FanOn(player As LevelPlayerMotor)
		player.AddForce(Me.scrollForce)
	End Sub

	' Token: 0x06003548 RID: 13640 RVA: 0x001F0BD5 File Offset: 0x001EEFD5
	Private Sub FanOff(player As LevelPlayerMotor)
		player.RemoveForce(Me.scrollForce)
	End Sub

	' Token: 0x04003D6E RID: 15726
	<SerializeField()>
	Private parent As MountainPlatformingLevelFan

	' Token: 0x04003D6F RID: 15727
	Private scrollForce As LevelPlayerMotor.VelocityManager.Force
End Class
