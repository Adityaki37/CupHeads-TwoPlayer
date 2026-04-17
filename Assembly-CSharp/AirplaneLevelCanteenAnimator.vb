Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020004B8 RID: 1208
Public Class AirplaneLevelCanteenAnimator
	Inherits LevelProperties.Airplane.Entity

	' Token: 0x060013F7 RID: 5111 RVA: 0x000B16B4 File Offset: 0x000AFAB4
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me.WORKAROUND_NullifyFields()
	End Sub

	' Token: 0x060013F8 RID: 5112 RVA: 0x000B16C4 File Offset: 0x000AFAC4
	Public Overrides Sub LevelInit(properties As LevelProperties.Airplane)
		MyBase.LevelInit(properties)
		Me.level = TryCast(Level.Current, AirplaneLevel)
		Me.curState = properties.CurrentState.stateName
		Me.idleLoops = Global.UnityEngine.Random.Range(3, 6)
		MyBase.StartCoroutine(Me.check_players_cr())
		MyBase.StartCoroutine(Me.handle_canteen_cr())
	End Sub

	' Token: 0x060013F9 RID: 5113 RVA: 0x000B1720 File Offset: 0x000AFB20
	Private Iterator Function check_players_cr() As IEnumerator
		While Me.p1health = -1
			Me.player1 = PlayerManager.GetPlayer(PlayerId.PlayerOne)
			Me.player2 = PlayerManager.GetPlayer(PlayerId.PlayerTwo)
			Me.p1health = If((Not Me.player1), (-1), PlayerManager.GetPlayer(PlayerId.PlayerOne).stats.Health)
			Me.p2health = If((Not Me.player2), (-1), PlayerManager.GetPlayer(PlayerId.PlayerTwo).stats.Health)
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x060013FA RID: 5114 RVA: 0x000B173C File Offset: 0x000AFB3C
	Private Sub OnPlayerHit(dead As Boolean)
		MyBase.animator.Play(If((Not dead), If((Not Me.playerHitAltAnim), "CanteenHitB", "CanteenHitA"), "CanteenOnePlayerDied"))
		MyBase.animator.SetBool("CanteenTrackBoss", False)
		If Not dead Then
			Me.playerHitAltAnim = Not Me.playerHitAltAnim
		End If
	End Sub

	' Token: 0x060013FB RID: 5115 RVA: 0x000B17A4 File Offset: 0x000AFBA4
	Public Overrides Sub OnLevelEnd()
		MyBase.OnLevelEnd()
		If Not Level.Won Then
			MyBase.animator.SetBool("CanteenTrackBoss", False)
			MyBase.animator.Play("CanteenAllPlayersDied")
		End If
	End Sub

	' Token: 0x060013FC RID: 5116 RVA: 0x000B17D8 File Offset: 0x000AFBD8
	Private Iterator Function handle_canteen_cr() As IEnumerator
		While True
			If Level.Won Then
				MyBase.animator.SetBool("CanteenTrackBoss", False)
				MyBase.animator.Play("CanteenWin")
			ElseIf Me.triggerCheer Then
				MyBase.animator.SetBool("CanteenTrackBoss", False)
				MyBase.animator.Play("CanteenCheer")
				Me.triggerCheer = False
			Else
				Me.player1 = PlayerManager.GetPlayer(PlayerId.PlayerOne)
				Me.player2 = PlayerManager.GetPlayer(PlayerId.PlayerTwo)
				If Me.player1 Then
					If PlayerManager.GetPlayer(PlayerId.PlayerOne).stats.Health < Me.p1health Then
						Me.p1health = PlayerManager.GetPlayer(PlayerId.PlayerOne).stats.Health
						Me.OnPlayerHit(Me.p1health = 0)
					ElseIf PlayerManager.GetPlayer(PlayerId.PlayerOne).stats.Health > Me.p1health Then
						MyBase.animator.SetBool("CanteenTrackBoss", False)
						MyBase.animator.Play("CanteenCheer")
					End If
					Me.p1health = PlayerManager.GetPlayer(PlayerId.PlayerOne).stats.Health
				End If
				If Me.player2 Then
					If PlayerManager.GetPlayer(PlayerId.PlayerTwo).stats.Health < Me.p2health Then
						Me.p2health = PlayerManager.GetPlayer(PlayerId.PlayerTwo).stats.Health
						Me.OnPlayerHit(Me.p2health = 0)
					ElseIf PlayerManager.GetPlayer(PlayerId.PlayerTwo).stats.Health > Me.p2health Then
						MyBase.animator.SetBool("CanteenTrackBoss", False)
						MyBase.animator.Play("CanteenCheer")
					End If
					Me.p2health = PlayerManager.GetPlayer(PlayerId.PlayerTwo).stats.Health
				End If
			End If
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x060013FD RID: 5117 RVA: 0x000B17F4 File Offset: 0x000AFBF4
	Private Sub LookAtBoss()
		Select Case MyBase.properties.CurrentState.stateName
			Case LevelProperties.Airplane.States.Main, LevelProperties.Airplane.States.Generic, LevelProperties.Airplane.States.Rocket
				If Me.level.CurrentEnemyPos().x - MyBase.transform.position.x < -250F Then
					MyBase.animator.Play("CanteenLookUpLeft")
					MyBase.animator.SetInteger("CanteenLookUpDir", -1)
					MyBase.animator.SetBool("CanteenTrackBoss", True)
				ElseIf Me.level.CurrentEnemyPos().x - MyBase.transform.position.x > 250F Then
					MyBase.animator.Play("CanteenLookUpRight")
					MyBase.animator.SetInteger("CanteenLookUpDir", 1)
					MyBase.animator.SetBool("CanteenTrackBoss", True)
				Else
					MyBase.animator.Play("CanteenLookUp")
					MyBase.animator.SetInteger("CanteenLookUpDir", 0)
					MyBase.animator.SetBool("CanteenTrackBoss", True)
				End If
			Case LevelProperties.Airplane.States.Terriers
				MyBase.animator.SetBool("CanteenTrackBoss", False)
				Select Case Global.UnityEngine.Random.Range(0, 6)
					Case 0
						MyBase.animator.Play("CanteenLookUpLeft")
					Case 1
						MyBase.animator.Play("CanteenLookUp")
					Case 2
						MyBase.animator.Play("CanteenLookUpRight")
					Case 3
						MyBase.animator.Play("CanteenLookDownRight")
					Case 4
						MyBase.animator.Play("CanteenLookDown")
					Case 5
						MyBase.animator.Play("CanteenLookDownLeft")
				End Select
			Case LevelProperties.Airplane.States.Leader
				If Me.level.ScreenHorizontal() Then
					If Me.level.CurrentEnemyPos().x - MyBase.transform.position.x < -100F Then
						MyBase.animator.Play("CanteenLookUpLeft")
						MyBase.animator.SetInteger("CanteenLookUpDir", -1)
						MyBase.animator.SetBool("CanteenTrackBoss", True)
					ElseIf Me.level.CurrentEnemyPos().x - MyBase.transform.position.x > 100F Then
						MyBase.animator.Play("CanteenLookUpRight")
						MyBase.animator.SetInteger("CanteenLookUpDir", 1)
						MyBase.animator.SetBool("CanteenTrackBoss", True)
					Else
						MyBase.animator.Play("CanteenLookUp")
						MyBase.animator.SetInteger("CanteenLookUpDir", 0)
						MyBase.animator.SetBool("CanteenTrackBoss", True)
					End If
				Else
					Dim num As Integer = Global.UnityEngine.Random.Range(0, 2)
					If num <> 0 Then
						If num = 1 Then
							MyBase.animator.Play("CanteenLookUpRight")
						End If
					Else
						MyBase.animator.Play("CanteenLookUpLeft")
					End If
				End If
		End Select
		Me.lookLoops = Global.UnityEngine.Random.Range(7, 9)
	End Sub

	' Token: 0x060013FE RID: 5118 RVA: 0x000B1B6F File Offset: 0x000AFF6F
	Public Sub ForceLook(target As Vector3, loops As Integer)
		Me.lookLoops = loops
		Me.idleLoops = 1
		Me.idleClipPos = -1
		Me.forceLookTarget = target
	End Sub

	' Token: 0x060013FF RID: 5119 RVA: 0x000B1B90 File Offset: 0x000AFF90
	Private Sub LookInDirection()
		MyBase.animator.SetBool("CanteenTrackBoss", False)
		Select Case CInt(((CDbl(Vector3.SignedAngle(Vector3.up, Me.forceLookTarget - MyBase.transform.position, Vector3.back)) + 202.5) Mod 360.0)) / 45
			Case 0
				MyBase.animator.Play("CanteenLookDown")
			Case 1
				MyBase.animator.Play("CanteenLookDownLeft")
			Case 2, 3
				MyBase.animator.Play("CanteenLookUpLeft")
			Case 4
				MyBase.animator.Play("CanteenLookUp")
			Case 5, 6
				MyBase.animator.Play("CanteenLookUpRight")
			Case 7
				MyBase.animator.Play("CanteenLookDownRight")
		End Select
	End Sub

	' Token: 0x06001400 RID: 5120 RVA: 0x000B1C98 File Offset: 0x000B0098
	Private Sub OnCanteenIdleLoop()
		Me.idleLoops -= 1
		If Me.idleLoops = 0 Then
			Dim num As Integer = Me.idleClipPos
			Select Case num + 1
				Case 0
					Me.LookInDirection()
					MyBase.animator.SetBool("CanteenTrackBoss", False)
				Case 1
					MyBase.animator.SetTrigger("CanteenBlink")
					MyBase.animator.SetBool("CanteenTrackBoss", False)
				Case 2
					If Global.UnityEngine.Random.Range(0, If((MyBase.properties.CurrentState.stateName <> LevelProperties.Airplane.States.Terriers), 10, 4)) = 0 Then
						Me.LookAtBoss()
					Else
						MyBase.animator.SetTrigger("CanteenGlanceAround")
						MyBase.animator.SetBool("CanteenTrackBoss", False)
					End If
				Case 3
					MyBase.animator.SetTrigger("CanteenBlink")
					MyBase.animator.SetBool("CanteenTrackBoss", False)
				Case 4
					Me.LookAtBoss()
			End Select
			Me.idleClipPos = (Me.idleClipPos + 1) Mod 4
			Me.idleLoops = Global.UnityEngine.Random.Range(3, 6)
		End If
	End Sub

	' Token: 0x06001401 RID: 5121 RVA: 0x000B1DD0 File Offset: 0x000B01D0
	Private Sub OnCanteenLookLoop()
		Me.lookLoops -= 1
		If Me.lookLoops <= 0 Then
			MyBase.animator.SetTrigger("CanteenEndLookLoop")
		End If
	End Sub

	' Token: 0x06001402 RID: 5122 RVA: 0x000B1DFC File Offset: 0x000B01FC
	Private Sub Update()
		If MyBase.animator.GetBool("CanteenTrackBoss") Then
			Select Case MyBase.properties.CurrentState.stateName
				Case LevelProperties.Airplane.States.Main, LevelProperties.Airplane.States.Generic, LevelProperties.Airplane.States.Rocket
					If Me.level.CurrentEnemyPos().x - MyBase.transform.position.x < -250F Then
						MyBase.animator.SetInteger("CanteenLookUpDir", -1)
					ElseIf Me.level.CurrentEnemyPos().x - MyBase.transform.position.x > 250F Then
						MyBase.animator.SetInteger("CanteenLookUpDir", 1)
					Else
						MyBase.animator.SetInteger("CanteenLookUpDir", 0)
					End If
				Case LevelProperties.Airplane.States.Leader
					If Me.level.ScreenHorizontal() Then
						If Me.level.CurrentEnemyPos().x - MyBase.transform.position.x < -100F Then
							MyBase.animator.SetInteger("CanteenLookUpDir", -1)
						ElseIf Me.level.CurrentEnemyPos().x - MyBase.transform.position.x > 100F Then
							MyBase.animator.SetInteger("CanteenLookUpDir", 1)
						Else
							MyBase.animator.SetInteger("CanteenLookUpDir", 0)
						End If
					End If
			End Select
		End If
	End Sub

	' Token: 0x06001403 RID: 5123 RVA: 0x000B1FAB File Offset: 0x000B03AB
	Private Sub WORKAROUND_NullifyFields()
		Me.player1 = Nothing
		Me.player2 = Nothing
		Me.level = Nothing
	End Sub

	' Token: 0x04001D16 RID: 7446
	Private Const MIN_IDLE_LOOPS As Integer = 3

	' Token: 0x04001D17 RID: 7447
	Private Const MAX_IDLE_LOOPS As Integer = 6

	' Token: 0x04001D18 RID: 7448
	Private Const MIN_LOOK_LOOPS As Integer = 7

	' Token: 0x04001D19 RID: 7449
	Private Const MAX_LOOK_LOOPS As Integer = 9

	' Token: 0x04001D1A RID: 7450
	Private Const PHASE_ONE_LOOK_ANGLE_THRESHOLD As Single = 250F

	' Token: 0x04001D1B RID: 7451
	Private Const PHASE_THREE_LOOK_ANGLE_THRESHOLD As Single = 100F

	' Token: 0x04001D1C RID: 7452
	Private idleLoops As Integer

	' Token: 0x04001D1D RID: 7453
	Private idleClipPos As Integer

	' Token: 0x04001D1E RID: 7454
	Private lookLoops As Integer

	' Token: 0x04001D1F RID: 7455
	Private forceLookTarget As Vector3

	' Token: 0x04001D20 RID: 7456
	Private player1 As AbstractPlayerController

	' Token: 0x04001D21 RID: 7457
	Private player2 As AbstractPlayerController

	' Token: 0x04001D22 RID: 7458
	Private p1health As Integer = -1

	' Token: 0x04001D23 RID: 7459
	Private p2health As Integer = -1

	' Token: 0x04001D24 RID: 7460
	Private playerHitAltAnim As Boolean

	' Token: 0x04001D25 RID: 7461
	Public triggerCheer As Boolean

	' Token: 0x04001D26 RID: 7462
	Private curState As LevelProperties.Airplane.States

	' Token: 0x04001D27 RID: 7463
	Private level As AirplaneLevel
End Class
