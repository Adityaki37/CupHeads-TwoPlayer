Imports System
Imports System.Diagnostics
Imports UnityEngine

' Token: 0x02000A0B RID: 2571
Public Class PlayerJoinEffect
	Inherits AbstractMonoBehaviour

	' Token: 0x06003CC4 RID: 15556 RVA: 0x0021A108 File Offset: 0x00218508
	Public Shared Function Create(playerId As PlayerId, pos As Vector2, mode As PlayerMode, isChalice As Boolean) As PlayerJoinEffect
		Dim playerJoinEffect As PlayerJoinEffect = Global.UnityEngine.[Object].Instantiate(Of PlayerJoinEffect)(Level.Current.LevelResources.joinEffect)
		playerJoinEffect.name = playerJoinEffect.name.Replace("(Clone)", String.Empty)
		playerJoinEffect.Init(playerId, pos, mode, isChalice)
		Return playerJoinEffect
	End Function

	' Token: 0x14000089 RID: 137
	' (add) Token: 0x06003CC5 RID: 15557 RVA: 0x0021A150 File Offset: 0x00218550
	' (remove) Token: 0x06003CC6 RID: 15558 RVA: 0x0021A188 File Offset: 0x00218588
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnPreReviveEvent As AbstractPlayerController.OnReviveHandler

	' Token: 0x1400008A RID: 138
	' (add) Token: 0x06003CC7 RID: 15559 RVA: 0x0021A1C0 File Offset: 0x002185C0
	' (remove) Token: 0x06003CC8 RID: 15560 RVA: 0x0021A1F8 File Offset: 0x002185F8
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnReviveEvent As AbstractPlayerController.OnReviveHandler

	' Token: 0x06003CC9 RID: 15561 RVA: 0x0021A22E File Offset: 0x0021862E
	Protected Overrides Sub Awake()
		MyBase.Awake()
		AddHandler PlayerManager.OnPlayerLeaveEvent, AddressOf Me.OnPlayerLeave
	End Sub

	' Token: 0x06003CCA RID: 15562 RVA: 0x0021A248 File Offset: 0x00218648
	Private Sub Init(playerId As PlayerId, pos As Vector2, mode As PlayerMode, isChalice As Boolean)
		Me.playerId = playerId
		Me.playerMode = mode
		MyBase.animator.SetInteger("Mode", CInt(Me.playerMode))
		If playerId = PlayerId.PlayerOne OrElse playerId <> PlayerId.PlayerTwo Then
			Me.spriteRenderer = Me.cuphead
		Else
			Me.spriteRenderer = Me.mugman
		End If
		If isChalice Then
			Me.spriteRenderer = Me.chalice
		End If
		Me.cuphead.gameObject.SetActive(False)
		Me.mugman.gameObject.SetActive(False)
		Me.chalice.gameObject.SetActive(False)
		Me.spriteRenderer.gameObject.SetActive(True)
		MyBase.transform.position = pos
	End Sub

	' Token: 0x06003CCB RID: 15563 RVA: 0x0021A318 File Offset: 0x00218718
	Public Sub GameOverUnpause()
		MyBase.animator.enabled = True
		Dim component As AnimationHelper = MyBase.GetComponent(Of AnimationHelper)()
		component.IgnoreGlobal = True
		Me.ignoreGlobalTime = True
	End Sub

	' Token: 0x06003CCC RID: 15564 RVA: 0x0021A346 File Offset: 0x00218746
	Private Sub OnReviveStealAnimComplete()
		If Me.OnReviveEvent IsNot Nothing Then
			Me.OnReviveEvent(MyBase.transform.position)
		End If
		Me.OnReviveEvent = Nothing
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub

	' Token: 0x06003CCD RID: 15565 RVA: 0x0021A37B File Offset: 0x0021877B
	Private Sub OnPlayerLeave(id As PlayerId)
		If Me.playerId = id Then
			Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
		End If
	End Sub

	' Token: 0x06003CCE RID: 15566 RVA: 0x0021A394 File Offset: 0x00218794
	Private Sub OnDestroy()
		RemoveHandler PlayerManager.OnPlayerLeaveEvent, AddressOf Me.OnPlayerLeave
	End Sub

	' Token: 0x0400440E RID: 17422
	Public Const NAME As String = "Player_Join"

	' Token: 0x0400440F RID: 17423
	<SerializeField()>
	Private cuphead As SpriteRenderer

	' Token: 0x04004410 RID: 17424
	<SerializeField()>
	Private mugman As SpriteRenderer

	' Token: 0x04004411 RID: 17425
	<SerializeField()>
	Private chalice As SpriteRenderer

	' Token: 0x04004412 RID: 17426
	Private playerId As PlayerId

	' Token: 0x04004413 RID: 17427
	Private spriteRenderer As SpriteRenderer

	' Token: 0x04004414 RID: 17428
	Private playerMode As PlayerMode
End Class
