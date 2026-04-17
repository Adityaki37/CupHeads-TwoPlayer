Imports System
Imports UnityEngine

' Token: 0x020008BB RID: 2235
Public Class FunhousePlatformingLevelMovingFloor
	Inherits AbstractCollidableObject

	' Token: 0x06003425 RID: 13349 RVA: 0x001E44A0 File Offset: 0x001E28A0
	Private Sub Start()
		If MyBase.transform.localScale.x = 1F Then
			Me.speed = -Me.velocity
		Else
			Me.speed = Me.velocity
		End If
		Me.scrollForce = New LevelPlayerMotor.VelocityManager.Force(LevelPlayerMotor.VelocityManager.Force.Type.Ground, Me.speed)
	End Sub

	' Token: 0x06003426 RID: 13350 RVA: 0x001E44FC File Offset: 0x001E28FC
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If phase = CollisionPhase.Stay Then
			If hit.GetComponent(Of LevelPlayerMotor)() Then
				Me.ScrollOn(hit.GetComponent(Of LevelPlayerMotor)())
			Else
				Me.ScrollOff(hit.GetComponent(Of LevelPlayerMotor)())
			End If
		Else
			Me.ScrollOff(hit.GetComponent(Of LevelPlayerMotor)())
		End If
	End Sub

	' Token: 0x06003427 RID: 13351 RVA: 0x001E4556 File Offset: 0x001E2956
	Private Sub ScrollOn(player As LevelPlayerMotor)
		player.AddForce(Me.scrollForce)
	End Sub

	' Token: 0x06003428 RID: 13352 RVA: 0x001E4564 File Offset: 0x001E2964
	Private Sub ScrollOff(player As LevelPlayerMotor)
		player.RemoveForce(Me.scrollForce)
	End Sub

	' Token: 0x04003C64 RID: 15460
	<SerializeField()>
	Private velocity As Single

	' Token: 0x04003C65 RID: 15461
	Private speed As Single

	' Token: 0x04003C66 RID: 15462
	Private scrollForce As LevelPlayerMotor.VelocityManager.Force
End Class
