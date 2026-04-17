Imports System
Imports UnityEngine

' Token: 0x0200068C RID: 1676
Public Class FlyingMermaidLevelLaser
	Inherits AbstractCollidableObject

	' Token: 0x06002354 RID: 9044 RVA: 0x0014BDDC File Offset: 0x0014A1DC
	Public Sub SetStoneTime(stoneTime As Single)
		Me.stoneTime = stoneTime
	End Sub

	' Token: 0x06002355 RID: 9045 RVA: 0x0014BDE5 File Offset: 0x0014A1E5
	Public Sub StartLaser()
		If MyBase.GetComponent(Of Collider2D)() Then
			Me.checkCollider = True
		End If
	End Sub

	' Token: 0x06002356 RID: 9046 RVA: 0x0014BDFE File Offset: 0x0014A1FE
	Public Sub StopLaser()
		If MyBase.GetComponent(Of Collider2D)() Then
			Me.checkCollider = False
		End If
	End Sub

	' Token: 0x06002357 RID: 9047 RVA: 0x0014BE17 File Offset: 0x0014A217
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If Me.checkCollider Then
			hit.GetComponent(Of PlanePlayerController)().GetStoned(Me.stoneTime)
		End If
	End Sub

	' Token: 0x04002BF4 RID: 11252
	Private stoneTime As Single = 5F

	' Token: 0x04002BF5 RID: 11253
	Private checkCollider As Boolean
End Class
