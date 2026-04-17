Imports System
Imports System.Collections
Imports System.Diagnostics
Imports UnityEngine

' Token: 0x02000A09 RID: 2569
Public Class PlayerDeathEffect
	Inherits AbstractMonoBehaviour

	' Token: 0x06003CAB RID: 15531 RVA: 0x001CA6F8 File Offset: 0x001C8AF8
	Public Function Create(playerId As PlayerId, input As PlayerInput, pos As Vector2, deathCount As Integer, mode As PlayerMode, canParry As Boolean) As PlayerDeathEffect
		Dim playerDeathEffect As PlayerDeathEffect = Global.UnityEngine.[Object].Instantiate(Of PlayerDeathEffect)(Me)
		playerDeathEffect.name = playerDeathEffect.name.Replace("(Clone)", String.Empty)
		playerDeathEffect.Init(playerId, input, pos, deathCount, mode, canParry)
		Return playerDeathEffect
	End Function

	' Token: 0x06003CAC RID: 15532 RVA: 0x001CA738 File Offset: 0x001C8B38
	Public Sub CreateExplosionOnly(playerId As PlayerId, pos As Vector2, mode As PlayerMode)
		If mode <> PlayerMode.Level Then
			If mode = PlayerMode.Plane Then
				Dim playerDeathEffect As PlayerDeathEffect = Global.UnityEngine.[Object].Instantiate(Of PlayerDeathEffect)(Me)
				Dim levelPlayerDeathEffect As LevelPlayerDeathEffect = Global.UnityEngine.[Object].Instantiate(Of LevelPlayerDeathEffect)(playerDeathEffect.explosionPrefab)
				levelPlayerDeathEffect.Init(pos)
				Global.UnityEngine.[Object].Destroy(playerDeathEffect.gameObject)
			End If
		Else
			Dim playerDeathEffect2 As PlayerDeathEffect = Global.UnityEngine.[Object].Instantiate(Of PlayerDeathEffect)(Me)
			Dim levelPlayerDeathEffect2 As LevelPlayerDeathEffect = Global.UnityEngine.[Object].Instantiate(Of LevelPlayerDeathEffect)(playerDeathEffect2.explosionPrefab)
			Dim levelPlayerController As LevelPlayerController = TryCast(PlayerManager.GetPlayer(playerId), LevelPlayerController)
			levelPlayerDeathEffect2.Init(pos, playerId, levelPlayerController.motor.Grounded)
			Global.UnityEngine.[Object].Destroy(playerDeathEffect2.gameObject)
		End If
	End Sub

	' Token: 0x14000087 RID: 135
	' (add) Token: 0x06003CAD RID: 15533 RVA: 0x001CA7C8 File Offset: 0x001C8BC8
	' (remove) Token: 0x06003CAE RID: 15534 RVA: 0x001CA800 File Offset: 0x001C8C00
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnPreReviveEvent As AbstractPlayerController.OnReviveHandler

	' Token: 0x14000088 RID: 136
	' (add) Token: 0x06003CAF RID: 15535 RVA: 0x001CA838 File Offset: 0x001C8C38
	' (remove) Token: 0x06003CB0 RID: 15536 RVA: 0x001CA870 File Offset: 0x001C8C70
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnReviveEvent As AbstractPlayerController.OnReviveHandler

	' Token: 0x06003CB1 RID: 15537 RVA: 0x001CA8A6 File Offset: 0x001C8CA6
	Protected Overrides Sub Awake()
		MyBase.Awake()
		AddHandler Me.parrySwitch.OnActivate, AddressOf Me.OnParrySwitch
		AddHandler PlayerManager.OnPlayerLeaveEvent, AddressOf Me.OnPlayerLeave
	End Sub

	' Token: 0x06003CB2 RID: 15538 RVA: 0x001CA8D7 File Offset: 0x001C8CD7
	Protected Overridable Sub Start()
		MyBase.StartCoroutine(Me.checkOutOfFrame_cr())
	End Sub

	' Token: 0x06003CB3 RID: 15539 RVA: 0x001CA8E8 File Offset: 0x001C8CE8
	Private Sub Init(playerId As PlayerId, input As PlayerInput, pos As Vector2, deathCount As Integer, mode As PlayerMode, canParry As Boolean)
		Me.playerInput = input
		Me.playerId = playerId
		Me.deathCount = deathCount
		If deathCount >= 10 Then
			Me.parrySwitch.gameObject.SetActive(False)
		End If
		Me.playerMode = mode
		Dim player As AbstractPlayerController = PlayerManager.GetPlayer(playerId)
		MyBase.animator.SetInteger("Mode", CInt(Me.playerMode))
		MyBase.animator.SetBool("CanParry", canParry)
		If playerId = PlayerId.PlayerOne OrElse playerId <> PlayerId.PlayerTwo Then
			Me.spriteRenderer = If((Not player.stats.isChalice), If((Not PlayerManager.player1IsMugman), Me.cuphead, Me.mugman), Me.chalice)
		Else
			Me.spriteRenderer = If((Not player.stats.isChalice), If((Not PlayerManager.player1IsMugman), Me.mugman, Me.cuphead), Me.chalice)
		End If
		Me.effect.enabled = Not PlayerManager.GetPlayer(playerId).stats.isChalice
		Me.chaliceEffect.enabled = PlayerManager.GetPlayer(playerId).stats.isChalice
		Me.cuphead.gameObject.SetActive(False)
		Me.mugman.gameObject.SetActive(False)
		Me.chalice.gameObject.SetActive(False)
		Me.spriteRenderer.gameObject.SetActive(True)
		Me.parrySwitch.gameObject.SetActive(canParry)
		MyBase.transform.position = pos
	End Sub

	' Token: 0x06003CB4 RID: 15540 RVA: 0x001CAA90 File Offset: 0x001C8E90
	Private Sub OnAnimationComplete()
		Dim player As AbstractPlayerController = PlayerManager.GetPlayer(Me.playerId)
		Dim levelPlayerController As LevelPlayerController = CType(player, LevelPlayerController)
		Dim levelPlayerDeathEffect As LevelPlayerDeathEffect = Global.UnityEngine.[Object].Instantiate(Of LevelPlayerDeathEffect)(Me.explosionPrefab)
		levelPlayerDeathEffect.Init(MyBase.transform.position, Me.playerId, levelPlayerController.motor.Grounded)
		MyBase.StartCoroutine(Me.float_cr())
	End Sub

	' Token: 0x06003CB5 RID: 15541 RVA: 0x001CAAF0 File Offset: 0x001C8EF0
	Private Sub OnAnimationCompletePlane()
		Dim levelPlayerDeathEffect As LevelPlayerDeathEffect = Global.UnityEngine.[Object].Instantiate(Of LevelPlayerDeathEffect)(Me.explosionPrefab)
		levelPlayerDeathEffect.Init(MyBase.transform.position)
		MyBase.StartCoroutine(Me.float_cr())
	End Sub

	' Token: 0x06003CB6 RID: 15542 RVA: 0x001CAB2C File Offset: 0x001C8F2C
	Public Sub GameOverUnpause()
		MyBase.animator.enabled = True
		Dim component As AnimationHelper = MyBase.GetComponent(Of AnimationHelper)()
		component.IgnoreGlobal = True
		Me.ignoreGlobalTime = True
	End Sub

	' Token: 0x06003CB7 RID: 15543 RVA: 0x001CAB5C File Offset: 0x001C8F5C
	Protected Overridable Sub OnParrySwitch()
		If Me.exiting Then
			Return
		End If
		Me.exiting = True
		Me.StopAllCoroutines()
		Me.parrySwitch.gameObject.SetActive(False)
		If Me.OnPreReviveEvent IsNot Nothing Then
			Me.OnPreReviveEvent(MyBase.transform.position)
		End If
		AudioManager.Play("player_revive")
		AudioManager.Play(If((Not PlayerManager.GetPlayer(Me.playerId).stats.isChalice), "player_revive_thank_you", "player_revive_thank_you_chalice"))
		MyBase.animator.SetTrigger("OnParry")
	End Sub

	' Token: 0x06003CB8 RID: 15544 RVA: 0x001CABFC File Offset: 0x001C8FFC
	Protected Overridable Sub OnReviveParryAnimComplete()
		If Me.OnReviveEvent IsNot Nothing Then
			Me.OnReviveEvent(MyBase.transform.position)
		End If
		Me.OnReviveEvent = Nothing
		Dim player As AbstractPlayerController = PlayerManager.GetPlayer(Me.playerId)
		If player.mode = PlayerMode.Level AndAlso player.stats.isChalice Then
			Dim levelPlayerController As LevelPlayerController = TryCast(player, LevelPlayerController)
			levelPlayerController.motor.OnChaliceRevive()
		End If
		Me.StopAllCoroutines()
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub

	' Token: 0x06003CB9 RID: 15545 RVA: 0x001CAC7C File Offset: 0x001C907C
	Private Sub ReviveOutOfFrame()
		If Me.exiting OrElse Not PlayerManager.Multiplayer Then
			Return
		End If
		Me.exiting = True
		Dim playerId As PlayerId = If((Me.playerId <> PlayerId.PlayerOne), PlayerId.PlayerOne, PlayerId.PlayerTwo)
		Dim player As AbstractPlayerController = PlayerManager.GetPlayer(playerId)
		If player Is Nothing OrElse player.IsDead OrElse Not player.stats.PartnerCanSteal OrElse Level.IsTowerOfPowerMain Then
			Return
		End If
		player.stats.OnPartnerStealHealth()
		Me.StopAllCoroutines()
		If Me.OnPreReviveEvent IsNot Nothing Then
			Me.OnPreReviveEvent(player.center)
		End If
		AudioManager.Play("player_revive")
		AudioManager.Play("player_revive_thank_you")
		MyBase.animator.SetTrigger("OnSteal")
		MyBase.transform.position = player.center
	End Sub

	' Token: 0x06003CBA RID: 15546 RVA: 0x001CAD54 File Offset: 0x001C9154
	Private Sub OnReviveStealAnimComplete()
		If Me.OnReviveEvent IsNot Nothing Then
			Me.OnReviveEvent(MyBase.transform.position)
		End If
		Me.OnReviveEvent = Nothing
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub

	' Token: 0x06003CBB RID: 15547 RVA: 0x001CAD89 File Offset: 0x001C9189
	Private Sub OnPlayerLeave(id As PlayerId)
		If Me.playerId = id Then
			Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
		End If
	End Sub

	' Token: 0x06003CBC RID: 15548 RVA: 0x001CADA2 File Offset: 0x001C91A2
	Private Sub OnDestroy()
		RemoveHandler PlayerManager.OnPlayerLeaveEvent, AddressOf Me.OnPlayerLeave
		Me.explosionPrefab = Nothing
		Me.cuphead = Nothing
		Me.mugman = Nothing
		Me.parrySwitch = Nothing
	End Sub

	' Token: 0x06003CBD RID: 15549 RVA: 0x001CADD1 File Offset: 0x001C91D1
	Public Sub Clean()
		Me.OnDestroy()
	End Sub

	' Token: 0x06003CBE RID: 15550 RVA: 0x001CADDC File Offset: 0x001C91DC
	Protected Iterator Function float_cr() As IEnumerator
		MyBase.animator.SetTrigger("OnIdle")
		Dim floatSpeed As Single = PlayerDeathEffect.FLOAT_SPEEDS(Mathf.Clamp(Me.deathCount, 0, PlayerDeathEffect.FLOAT_SPEEDS.Length - 1))
		While True AndAlso Not Me.exiting
			MyBase.transform.AddPosition(0F, floatSpeed * MyBase.LocalDeltaTime, 0F)
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06003CBF RID: 15551 RVA: 0x001CADF8 File Offset: 0x001C91F8
	Protected Overridable Iterator Function checkOutOfFrame_cr() As IEnumerator
		While True
			If Not CupheadLevelCamera.Current.ContainsPoint(MyBase.transform.position, New Vector2(1000F, 10F)) AndAlso Me.playerInput.actions.GetButtonDown(8) AndAlso Not Me.exiting Then
				Yield New WaitForSeconds(0.1F)
				Me.ReviveOutOfFrame()
			End If
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x040043F8 RID: 17400
	Public Const NAME As String = "Player_Death"

	' Token: 0x040043F9 RID: 17401
	Public Const EFFECT_NAME As String = "Player_Death_Explosion"

	' Token: 0x040043FA RID: 17402
	Private Const PATH As String = "Player/Player_Death"

	' Token: 0x040043FB RID: 17403
	Private Const TIME_TO_SPEED As Single = 1F

	' Token: 0x040043FC RID: 17404
	Private Shared FLOAT_SPEEDS As Single() = New Single() { 125F, 200F, 275F }

	' Token: 0x040043FD RID: 17405
	Private Const REVIVE_Y As Integer = 10

	' Token: 0x040043FE RID: 17406
	Public Const DEATH_COUNT_MAX As Integer = 10

	' Token: 0x040043FF RID: 17407
	<SerializeField()>
	Protected cuphead As SpriteRenderer

	' Token: 0x04004400 RID: 17408
	<SerializeField()>
	Protected mugman As SpriteRenderer

	' Token: 0x04004401 RID: 17409
	<SerializeField()>
	Protected chalice As SpriteRenderer

	' Token: 0x04004402 RID: 17410
	<SerializeField()>
	Protected parrySwitch As PlayerDeathParrySwitch

	' Token: 0x04004403 RID: 17411
	<SerializeField()>
	Private explosionPrefab As LevelPlayerDeathEffect

	' Token: 0x04004404 RID: 17412
	<SerializeField()>
	Private effect As SpriteRenderer

	' Token: 0x04004405 RID: 17413
	<SerializeField()>
	Private chaliceEffect As SpriteRenderer

	' Token: 0x04004406 RID: 17414
	Protected playerId As PlayerId

	' Token: 0x04004407 RID: 17415
	Protected spriteRenderer As SpriteRenderer

	' Token: 0x04004408 RID: 17416
	Protected exiting As Boolean

	' Token: 0x04004409 RID: 17417
	Private playerInput As PlayerInput

	' Token: 0x0400440A RID: 17418
	Private deathCount As Integer

	' Token: 0x0400440B RID: 17419
	Private playerMode As PlayerMode
End Class
