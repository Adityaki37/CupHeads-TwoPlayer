Imports System
Imports System.Collections.Generic
Imports UnityEngine

' Token: 0x0200069B RID: 1691
Public Class FlyingMermaidLevelSeahorseSpray
	Inherits AbstractPausableComponent

	' Token: 0x060023DA RID: 9178 RVA: 0x00150EA8 File Offset: 0x0014F2A8
	Private Sub Update()
		If Me.ended Then
			Return
		End If
		For Each planePlayerMotor As PlanePlayerMotor In Me.playerInfos.Keys
			If Not(planePlayerMotor Is Nothing) Then
				If Mathf.Abs(planePlayerMotor.transform.position.x - MyBase.transform.position.x) < Me.width / 2F AndAlso planePlayerMotor.player.center.y < Me.topRoot.position.y Then
					Me.playerInfos(planePlayerMotor).force.enabled = True
					Me.playerInfos(planePlayerMotor).timeSinceFx += CupheadTime.Delta
					If Me.playerInfos(planePlayerMotor).timeSinceFx >= Me.playerInfos(planePlayerMotor).fxWaitTime Then
						Dim effect As Effect = Me.effectPrefab.Create(planePlayerMotor.player.center + New Vector3(0F, -40F))
						Dim num As Integer = (Me.playerInfos(planePlayerMotor).lastFxVariant + Global.UnityEngine.Random.Range(0, 3)) Mod 3
						effect.animator.SetInteger("Effect", num)
						Me.playerInfos(planePlayerMotor).lastFxVariant = num
						Me.playerInfos(planePlayerMotor).fxWaitTime = Global.UnityEngine.Random.Range(0.125F, 0.17F)
						Me.playerInfos(planePlayerMotor).timeSinceFx = 0F
					End If
				Else
					Me.playerInfos(planePlayerMotor).force.enabled = False
					Me.playerInfos(planePlayerMotor).fxWaitTime = 0F
				End If
			End If
		Next
	End Sub

	' Token: 0x060023DB RID: 9179 RVA: 0x001510CC File Offset: 0x0014F4CC
	Public Sub Init(properties As LevelProperties.FlyingMermaid.Seahorse)
		For Each abstractPlayerController As AbstractPlayerController In PlayerManager.GetAllPlayers()
			Dim planePlayerController As PlanePlayerController = CType(abstractPlayerController, PlanePlayerController)
			If Not(planePlayerController Is Nothing) Then
				Dim force As PlanePlayerMotor.Force = New PlanePlayerMotor.Force(New Vector2(0F, properties.waterForce), False)
				planePlayerController.motor.AddForce(force)
				Dim playerInfo As FlyingMermaidLevelSeahorseSpray.PlayerInfo = New FlyingMermaidLevelSeahorseSpray.PlayerInfo()
				playerInfo.force = force
				Me.playerInfos(planePlayerController.motor) = playerInfo
			End If
		Next
	End Sub

	' Token: 0x060023DC RID: 9180 RVA: 0x00151178 File Offset: 0x0014F578
	Public Sub [End]()
		Me.ended = True
		For Each abstractPlayerController As AbstractPlayerController In PlayerManager.GetAllPlayers()
			Dim planePlayerController As PlanePlayerController = CType(abstractPlayerController, PlanePlayerController)
			If Not(planePlayerController Is Nothing) Then
				planePlayerController.motor.RemoveForce(Me.playerInfos(planePlayerController.motor).force)
			End If
		Next
	End Sub

	' Token: 0x04002CA3 RID: 11427
	Public width As Single = 20F

	' Token: 0x04002CA4 RID: 11428
	Private playerInfos As Dictionary(Of PlanePlayerMotor, FlyingMermaidLevelSeahorseSpray.PlayerInfo) = New Dictionary(Of PlanePlayerMotor, FlyingMermaidLevelSeahorseSpray.PlayerInfo)()

	' Token: 0x04002CA5 RID: 11429
	<SerializeField()>
	Private effectPrefab As Effect

	' Token: 0x04002CA6 RID: 11430
	<SerializeField()>
	Private topRoot As Transform

	' Token: 0x04002CA7 RID: 11431
	Private ended As Boolean

	' Token: 0x0200069C RID: 1692
	Private Class PlayerInfo
		' Token: 0x04002CA8 RID: 11432
		Public force As PlanePlayerMotor.Force

		' Token: 0x04002CA9 RID: 11433
		Public timeSinceFx As Single

		' Token: 0x04002CAA RID: 11434
		Public fxWaitTime As Single

		' Token: 0x04002CAB RID: 11435
		Public lastFxVariant As Integer = -1
	End Class
End Class
