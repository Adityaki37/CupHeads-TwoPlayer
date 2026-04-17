Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x0200097F RID: 2431
Public Class MapPlayerMotor
	Inherits AbstractMapPlayerComponent

	' Token: 0x1700049B RID: 1179
	' (get) Token: 0x060038B8 RID: 14520 RVA: 0x002049D0 File Offset: 0x00202DD0
	' (set) Token: 0x060038B9 RID: 14521 RVA: 0x002049D8 File Offset: 0x00202DD8
	Public Property velocity As Vector2

	' Token: 0x060038BA RID: 14522 RVA: 0x002049E4 File Offset: 0x00202DE4
	Private Sub Update()
		If Not MapPlayerController.CanMove() OrElse MyBase.player.state = MapPlayerController.State.Stationary Then
			Me.velocity = Vector2.zero
			Me.axis = Vector2.zero
			MyBase.rigidbody2D.velocity = Vector2.zero
			Return
		End If
		If PauseManager.state = PauseManager.State.Paused Then
			Return
		End If
		Me.HandleInput()
		Dim state As MapPlayerController.State = MyBase.player.state
		If state <> MapPlayerController.State.Walking Then
			If state = MapPlayerController.State.Ladder Then
				Me.MoveLadder()
			End If
		Else
			Me.MoveWalking()
		End If
	End Sub

	' Token: 0x060038BB RID: 14523 RVA: 0x00204A7C File Offset: 0x00202E7C
	Private Sub LateUpdate()
		Dim state As MapPlayerController.State = MyBase.player.state
		If state = MapPlayerController.State.Ladder Then
			Me.ClampPositionLadder()
		End If
	End Sub

	' Token: 0x060038BC RID: 14524 RVA: 0x00204AAC File Offset: 0x00202EAC
	Public Overrides Sub OnPause()
		MyBase.OnPause()
		MyBase.rigidbody2D.velocity = Vector2.zero
	End Sub

	' Token: 0x060038BD RID: 14525 RVA: 0x00204AC4 File Offset: 0x00202EC4
	Private Sub HandleInput()
		If MyBase.player.EquipMenuOpen Then
			Return
		End If
		Me.axis = New Vector2(CSng(MyBase.input.GetAxisInt(PlayerInput.Axis.X, False, False)), CSng(MyBase.input.GetAxisInt(PlayerInput.Axis.Y, False, False)))
		Dim magnitude As Single = Me.axis.magnitude
		If magnitude < 0.0001F Then
			Me.axis = Vector2.zero
		Else
			Me.axis /= magnitude
		End If
	End Sub

	' Token: 0x060038BE RID: 14526 RVA: 0x00204B44 File Offset: 0x00202F44
	Private Sub MoveWalking()
		Me.velocity = Vector2.Lerp(Me.velocity, New Vector2(Me.axis.x * 2.5F, Me.axis.y * 2.5F), CupheadTime.Delta * 100F)
		MyBase.rigidbody2D.velocity = Me.velocity
	End Sub

	' Token: 0x060038BF RID: 14527 RVA: 0x00204BAA File Offset: 0x00202FAA
	Private Sub MoveLadder()
		Me.velocity = New Vector2(0F, Me.axis.y * 2.5F)
		MyBase.rigidbody2D.velocity = Me.velocity
	End Sub

	' Token: 0x060038C0 RID: 14528 RVA: 0x00204BE0 File Offset: 0x00202FE0
	Private Sub ClampPositionLadder()
		Dim mapPlayerLadderObject As MapPlayerLadderObject = MyBase.player.ladderManager.Current
		Dim state As MapPlayerController.State = MyBase.player.state
		If state = MapPlayerController.State.Ladder Then
			MyBase.transform.SetPosition(Nothing, New Single?(Mathf.Clamp(MyBase.transform.position.y, mapPlayerLadderObject.bottom.y, mapPlayerLadderObject.top.y)), Nothing)
		End If
	End Sub

	' Token: 0x060038C1 RID: 14529 RVA: 0x00204C73 File Offset: 0x00203073
	Protected Overrides Sub OnLadderEnter(point As Vector2, ladder As MapPlayerLadderObject, location As MapLadder.Location)
		MyBase.OnLadderEnter(point, ladder, location)
		MyBase.StartCoroutine(Me.onLadderStart_cr(point, location))
	End Sub

	' Token: 0x060038C2 RID: 14530 RVA: 0x00204C8D File Offset: 0x0020308D
	Protected Overrides Sub OnLadderExit(point As Vector2, [exit] As Vector2, location As MapLadder.Location)
		MyBase.OnLadderExit(point, [exit], location)
		MyBase.StartCoroutine(Me.onLadderEnd_cr([exit], location))
	End Sub

	' Token: 0x060038C3 RID: 14531 RVA: 0x00204CA8 File Offset: 0x002030A8
	Private Iterator Function onLadderStart_cr(endPos As Vector2, location As MapLadder.Location) As IEnumerator
		location = If((location <> MapLadder.Location.Top), MapLadder.Location.Top, MapLadder.Location.Bottom)
		Yield MyBase.StartCoroutine(Me.ladder_cr(MyBase.transform.position, endPos, location))
		MyBase.player.LadderEnterComplete()
		Return
	End Function

	' Token: 0x060038C4 RID: 14532 RVA: 0x00204CD4 File Offset: 0x002030D4
	Private Iterator Function onLadderEnd_cr(endPos As Vector2, location As MapLadder.Location) As IEnumerator
		Yield MyBase.StartCoroutine(Me.ladder_cr(MyBase.transform.position, endPos, location))
		MyBase.player.LadderExitComplete()
		Return
	End Function

	' Token: 0x060038C5 RID: 14533 RVA: 0x00204D00 File Offset: 0x00203100
	Private Iterator Function ladder_cr(startPos As Vector2, endPos As Vector2, location As MapLadder.Location) As IEnumerator
		Dim centerPos As Vector2 = New Vector2(Mathf.Lerp(startPos.x, endPos.x, 0.5F), If((location <> MapLadder.Location.Top), (startPos.y + 0.2F), (endPos.y + 0.2F)))
		Dim t As Single = 0F
		Dim time As Single = 0.15F
		While t < time
			Dim val As Single = EaseUtils.Ease(EaseUtils.EaseType.easeOutSine, 0F, 1F, t / time)
			Dim newPos As Vector2 = Vector2.Lerp(startPos, centerPos, val)
			MyBase.transform.SetPosition(New Single?(newPos.x), New Single?(newPos.y), Nothing)
			t += CupheadTime.Delta
			Yield Nothing
		End While
		t = 0F
		While t < time
			Dim val2 As Single = EaseUtils.Ease(EaseUtils.EaseType.easeInSine, 0F, 1F, t / time)
			Dim newPos2 As Vector2 = Vector2.Lerp(centerPos, endPos, val2)
			MyBase.transform.SetPosition(New Single?(newPos2.x), New Single?(newPos2.y), Nothing)
			t += CupheadTime.Delta
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x0400406D RID: 16493
	Private Const SPEED As Single = 2.5F

	' Token: 0x0400406E RID: 16494
	Private Const DIAGONAL_FALLOFF As Single = 0.75F

	' Token: 0x0400406F RID: 16495
	Private Const FALLOFF_SPEED As Single = 100F

	' Token: 0x04004070 RID: 16496
	Public Const INPUT_THRESHOLD As Single = 0.3F

	' Token: 0x04004072 RID: 16498
	Private axis As Vector2

	' Token: 0x04004073 RID: 16499
	Public Const LADDER_ENTER_TIME As Single = 0.3F

	' Token: 0x04004074 RID: 16500
	Public Const LADDER_EXIT_TIME As Single = 0.3F

	' Token: 0x04004075 RID: 16501
	Public Const LADDER_EXIT_JUMP As Single = 0.2F
End Class
