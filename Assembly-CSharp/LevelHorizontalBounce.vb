Imports System
Imports UnityEngine

' Token: 0x020004A4 RID: 1188
Public Class LevelHorizontalBounce
	Inherits AbstractCollidableObject

	' Token: 0x0600135E RID: 4958 RVA: 0x000AB3DC File Offset: 0x000A97DC
	Private Sub Start()
		Me.scrollForce = New LevelPlayerMotor.VelocityManager.Force(LevelPlayerMotor.VelocityManager.Force.Type.All, If((Not Me.onLeft), (-Me.fanForce), Me.fanForce))
	End Sub

	' Token: 0x0600135F RID: 4959 RVA: 0x000AB408 File Offset: 0x000A9808
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		Dim component As LevelPlayerMotor = hit.GetComponent(Of LevelPlayerMotor)()
		If phase <> CollisionPhase.[Exit] Then
			Me.FanOn(component)
		Else
			Me.FanOff(hit.GetComponent(Of LevelPlayerMotor)())
		End If
	End Sub

	' Token: 0x06001360 RID: 4960 RVA: 0x000AB443 File Offset: 0x000A9843
	Private Sub FanOn(player As LevelPlayerMotor)
		player.AddForce(Me.scrollForce)
	End Sub

	' Token: 0x06001361 RID: 4961 RVA: 0x000AB451 File Offset: 0x000A9851
	Private Sub FanOff(player As LevelPlayerMotor)
		player.RemoveForce(Me.scrollForce)
	End Sub

	' Token: 0x04001C79 RID: 7289
	<SerializeField()>
	Private onLeft As Boolean

	' Token: 0x04001C7A RID: 7290
	<SerializeField()>
	Private fanForce As Single = 1F

	' Token: 0x04001C7B RID: 7291
	Private scrollForce As LevelPlayerMotor.VelocityManager.Force
End Class
