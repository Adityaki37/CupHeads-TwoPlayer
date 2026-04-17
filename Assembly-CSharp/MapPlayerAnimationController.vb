Imports System
Imports System.Linq
Imports UnityEngine

' Token: 0x02000973 RID: 2419
Public Class MapPlayerAnimationController
	Inherits AbstractMapPlayerComponent

	' Token: 0x1700048E RID: 1166
	' (get) Token: 0x0600385B RID: 14427 RVA: 0x00203581 File Offset: 0x00201981
	' (set) Token: 0x0600385C RID: 14428 RVA: 0x00203589 File Offset: 0x00201989
	Public Property direction As MapPlayerAnimationController.Direction

	' Token: 0x1700048F RID: 1167
	' (get) Token: 0x0600385D RID: 14429 RVA: 0x00203592 File Offset: 0x00201992
	' (set) Token: 0x0600385E RID: 14430 RVA: 0x0020359A File Offset: 0x0020199A
	Public Property state As MapPlayerAnimationController.State

	' Token: 0x17000490 RID: 1168
	' (get) Token: 0x0600385F RID: 14431 RVA: 0x002035A3 File Offset: 0x002019A3
	' (set) Token: 0x06003860 RID: 14432 RVA: 0x002035AB File Offset: 0x002019AB
	Public Property spriteRenderer As SpriteRenderer

	' Token: 0x06003861 RID: 14433 RVA: 0x002035B4 File Offset: 0x002019B4
	Public Sub Init(pose As MapPlayerPose)
		Me.Cuphead.enabled = False
		Me.Mugman.enabled = False
		Me.ghostInPortal(0).sortingLayerName = "Effects"
		Me.ghostInPortal(1).sortingLayerName = "Effects"
		Me.portal.sortingLayerName = "Effects"
		Dim id As PlayerId = MyBase.player.id
		If id = PlayerId.PlayerOne OrElse id <> PlayerId.PlayerTwo Then
			Me.spriteRenderer = If((Not PlayerManager.player1IsMugman), Me.Cuphead, Me.Mugman)
			MyBase.animator.SetInteger("Player", 0)
		Else
			MyBase.animator.SetInteger("Player", 1)
			Me.spriteRenderer = If((Not PlayerManager.player1IsMugman), Me.Mugman, Me.Cuphead)
		End If
		Me.spriteRenderer.enabled = True
		Select Case pose
			Case MapPlayerPose.[Default]
				Me.state = MapPlayerAnimationController.State.Idle
			Case MapPlayerPose.Joined, MapPlayerPose.Won
				MyBase.animator.Play(If((Not PlayerManager.playerWasChalice(CInt(MyBase.player.id))), "Jump", "WinChalice_Loop"))
				If PlayerManager.playerWasChalice(CInt(MyBase.player.id)) Then
					If PlayerManager.player1IsMugman Then
						Me.ghostInPortal(CInt(MyBase.player.id)).enabled = False
					Else
						Me.ghostInPortal(CInt((PlayerId.PlayerTwo - MyBase.player.id))).enabled = False
					End If
					If MyBase.player.id = PlayerId.PlayerTwo Then
						MyBase.transform.localScale = New Vector3(-1F, 1F)
					End If
				End If
		End Select
		Me.SetProperties()
	End Sub

	' Token: 0x06003862 RID: 14434 RVA: 0x0020378A File Offset: 0x00201B8A
	Private Sub MovePortalSwapToFront()
		Me.Chalice.sortingLayerName = "Effects"
	End Sub

	' Token: 0x06003863 RID: 14435 RVA: 0x0020379C File Offset: 0x00201B9C
	Private Sub Update()
		If MyBase.player.state = MapPlayerController.State.Stationary Then
			Me.SetStationary()
			Return
		End If
		If Not MapPlayerController.CanMove() Then
			Me.SetStationary()
			Return
		End If
		Dim vector As Vector2 = New Vector2(MyBase.player.input.actions.GetAxis(0), MyBase.player.input.actions.GetAxis(1))
		Me.state = If((vector.magnitude <= 0.3F), MapPlayerAnimationController.State.Idle, MapPlayerAnimationController.State.Walk)
		Me.SetProperties()
		Me.UpdateDjimmiCodeTimer()
	End Sub

	' Token: 0x06003864 RID: 14436 RVA: 0x0020382F File Offset: 0x00201C2F
	Private Sub SetStationary()
		Me.state = MapPlayerAnimationController.State.Idle
		Me.axis.x = 0
		Me.axis.y = 0
		Me.SetProperties()
	End Sub

	' Token: 0x06003865 RID: 14437 RVA: 0x00203860 File Offset: 0x00201C60
	Public Sub CompleteJump()
		MyBase.animator.SetTrigger("OnJumpComplete")
	End Sub

	' Token: 0x06003866 RID: 14438 RVA: 0x00203874 File Offset: 0x00201C74
	Private Sub SetProperties()
		If Me.state = MapPlayerAnimationController.State.Walk Then
			Me.axis.x = MyBase.player.input.GetAxisInt(PlayerInput.Axis.X, False, False)
			Me.axis.y = MyBase.player.input.GetAxisInt(PlayerInput.Axis.Y, False, False)
			If Me.axis.x = -1 Then
				Me.spriteRenderer.transform.SetScale(New Single?(-1F), Nothing, Nothing)
			Else
				Me.spriteRenderer.transform.SetScale(New Single?(1F), Nothing, Nothing)
			End If
		End If
		MyBase.animator.SetInteger("X", Me.axis.x)
		MyBase.animator.SetInteger("Y", Me.axis.y)
		MyBase.animator.SetInteger("Speed", If((Me.state <> MapPlayerAnimationController.State.Idle), 1, 0))
		Me.SetDirectionRotation()
	End Sub

	' Token: 0x06003867 RID: 14439 RVA: 0x002039B0 File Offset: 0x00201DB0
	Private Sub SetDirectionRotation()
		Me.facingUpwards = Me.axis.y > 0
		If Me.axis.x = 1 AndAlso Me.axis.y = 1 Then
			Me.directionRotation = -45F
		ElseIf Me.axis.x = 1 AndAlso Me.axis.y = 0 Then
			Me.directionRotation = -90F
		ElseIf Me.axis.x = 1 AndAlso Me.axis.y = -1 Then
			Me.directionRotation = -135F
		ElseIf Me.axis.x = 0 AndAlso Me.axis.y = 1 Then
			Me.directionRotation = 0F
		ElseIf Me.axis.x = 0 AndAlso Me.axis.y = 0 Then
			Me.directionRotation = 0F
		ElseIf Me.axis.x = 0 AndAlso Me.axis.y = -1 Then
			Me.directionRotation = -180F
		ElseIf Me.axis.x = -1 AndAlso Me.axis.y = 1 Then
			Me.directionRotation = 45F
		ElseIf Me.axis.x = -1 AndAlso Me.axis.y = 0 Then
			Me.directionRotation = 90F
		ElseIf Me.axis.x = -1 AndAlso Me.axis.y = -1 Then
			Me.directionRotation = 135F
		End If
		Me.UpdateDjimmiCode(CInt(Me.directionRotation))
	End Sub

	' Token: 0x06003868 RID: 14440 RVA: 0x00203BF4 File Offset: 0x00201FF4
	Private Sub UpdateDjimmiCode(direction As Integer)
		If direction = -45 OrElse direction = -135 OrElse direction = 45 OrElse direction = 135 Then
			Return
		End If
		If direction = Me.djimmiCodeEntry(Me.djimmiCodeEntry.Length - 1) Then
			Return
		End If
		For i As Integer = 0 To Me.djimmiCodeEntry.Length - 1 - 1
			Me.djimmiCodeEntry(i) = Me.djimmiCodeEntry(i + 1)
			Me.djimmiCodeTimeStamp(i) = Me.djimmiCodeTimeStamp(i + 1)
		Next
		Me.djimmiCodeEntry(Me.djimmiCodeEntry.Length - 1) = direction
		Me.djimmiCodeTimeStamp(Me.djimmiCodeEntry.Length - 1) = 2F
		If Me.djimmiCodeTimeStamp(0) > 0F AndAlso (Me.djimmiCodeEntry.SequenceEqual(Me.djimmiCodeA) OrElse Me.djimmiCodeEntry.SequenceEqual(Me.djimmiCodeB)) Then
			For j As Integer = 0 To Me.djimmiCodeEntry.Length - 1
				Me.djimmiCodeEntry(j) = 0
				Me.djimmiCodeTimeStamp(j) = 0F
			Next
			MyBase.player.TryActivateDjimmi()
		End If
	End Sub

	' Token: 0x06003869 RID: 14441 RVA: 0x00203D20 File Offset: 0x00202120
	Private Sub UpdateDjimmiCodeTimer()
		For i As Integer = 0 To Me.djimmiCodeTimeStamp.Length - 1
			Me.djimmiCodeTimeStamp(i) -= CupheadTime.Delta
		Next
	End Sub

	' Token: 0x0600386A RID: 14442 RVA: 0x00203D60 File Offset: 0x00202160
	Private Sub WalkStepLeft()
		If Me.spriteRenderer Is Me.Cuphead Then
			If Me.current IsNot Nothing Then
				Me.current.PlaySoundRight(True)
			Else
				AudioManager.Play("player_map_walk_one_p1")
			End If
		ElseIf Me.current IsNot Nothing Then
			Me.current.PlaySoundRight(False)
		Else
			AudioManager.Play("player_map_walk_one_p2")
		End If
		Me.dustEffect.Create(MyBase.transform.position, Me.directionRotation, True, Me.spriteRenderer.sortingOrder)
	End Sub

	' Token: 0x0600386B RID: 14443 RVA: 0x00203E0C File Offset: 0x0020220C
	Private Sub WalkStepRight()
		If Me.spriteRenderer Is Me.Cuphead Then
			If Me.current IsNot Nothing Then
				Me.current.PlaySoundRight(True)
			Else
				AudioManager.Play("player_map_walk_one_p1")
			End If
		ElseIf Me.current IsNot Nothing Then
			Me.current.PlaySoundRight(False)
		Else
			AudioManager.Play("player_map_walk_two_p2")
		End If
		Me.dustEffect.Create(MyBase.transform.position, Me.directionRotation, False, Me.spriteRenderer.sortingOrder)
	End Sub

	' Token: 0x0600386C RID: 14444 RVA: 0x00203EB5 File Offset: 0x002022B5
	Private Sub OnTriggerEnter2D(collider As Collider2D)
		If collider.GetComponent(Of MapSpritePlaySound)() Then
			Me.current = collider.GetComponent(Of MapSpritePlaySound)()
		End If
	End Sub

	' Token: 0x0600386D RID: 14445 RVA: 0x00203ED3 File Offset: 0x002022D3
	Private Sub OnTriggerExit2D(collider As Collider2D)
		If collider.GetComponent(Of MapSpritePlaySound)() Then
			Me.current = Nothing
		End If
	End Sub

	' Token: 0x0600386E RID: 14446 RVA: 0x00203EEC File Offset: 0x002022EC
	Private Sub AniEvent_YawnSFX()
		If MyBase.player.id = PlayerId.PlayerOne Then
			AudioManager.Play("worldmap_playeryawn")
			Me.emitAudioFromObject.Add("worldmap_playeryawn")
		End If
	End Sub

	' Token: 0x0600386F RID: 14447 RVA: 0x00203F18 File Offset: 0x00202318
	Private Sub AniEvent_GhostSwapSFX()
		AudioManager.Play("sfx_DLC_WorldMap_GhostSwap")
		Me.emitAudioFromObject.Add("sfx_DLC_WorldMap_GhostSwap")
	End Sub

	' Token: 0x0400402F RID: 16431
	Private Const DJIMMI_CODE_LENGTH As Integer = 16

	' Token: 0x04004030 RID: 16432
	Private Const MAX_TIME_FOR_DJIMMI_CODE As Single = 2F

	' Token: 0x04004031 RID: 16433
	Private djimmiCodeA As Integer() = New Integer() { 0, 90, -180, -90, 0, 90, -180, -90, 0, 90, -180, -90, 0, 90, -180, -90 }

	' Token: 0x04004032 RID: 16434
	Private djimmiCodeB As Integer() = New Integer() { 0, -90, -180, 90, 0, -90, -180, 90, 0, -90, -180, 90, 0, -90, -180, 90 }

	' Token: 0x04004033 RID: 16435
	Public facingUpwards As Boolean

	' Token: 0x04004034 RID: 16436
	<SerializeField()>
	Private Cuphead As SpriteRenderer

	' Token: 0x04004035 RID: 16437
	<SerializeField()>
	Private Mugman As SpriteRenderer

	' Token: 0x04004036 RID: 16438
	<SerializeField()>
	Private Chalice As SpriteRenderer

	' Token: 0x04004037 RID: 16439
	<SerializeField()>
	Private ghostInPortal As SpriteRenderer()

	' Token: 0x04004038 RID: 16440
	<SerializeField()>
	Private portal As SpriteRenderer

	' Token: 0x04004039 RID: 16441
	<SerializeField()>
	Private dustEffect As MapPlayerDust

	' Token: 0x0400403D RID: 16445
	Private current As MapSpritePlaySound

	' Token: 0x0400403E RID: 16446
	Private axis As Trilean2

	' Token: 0x0400403F RID: 16447
	Private onBridge As Boolean

	' Token: 0x04004040 RID: 16448
	Private directionRotation As Single

	' Token: 0x04004041 RID: 16449
	Private djimmiCodeEntry As Integer() = New Integer(15) {}

	' Token: 0x04004042 RID: 16450
	Private djimmiCodeTimeStamp As Single() = New Single(15) {}

	' Token: 0x02000974 RID: 2420
	Public Enum Direction
		' Token: 0x04004044 RID: 16452
		Left
		' Token: 0x04004045 RID: 16453
		Right
	End Enum

	' Token: 0x02000975 RID: 2421
	Public Enum State
		' Token: 0x04004047 RID: 16455
		Idle
		' Token: 0x04004048 RID: 16456
		Walk
	End Enum
End Class
