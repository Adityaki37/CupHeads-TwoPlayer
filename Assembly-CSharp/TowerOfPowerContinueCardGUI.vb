Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports UnityEngine
Imports UnityEngine.UI

' Token: 0x02000489 RID: 1161
<RequireComponent(GetType(CanvasGroup))>
Public Class TowerOfPowerContinueCardGUI
	Inherits AbstractMonoBehaviour

	' Token: 0x06001225 RID: 4645 RVA: 0x000A8C70 File Offset: 0x000A7070
	Protected Overrides Sub Awake()
		MyBase.Awake()
		For Each uiimageAnimationLoop As UIImageAnimationLoop In Me.CardMugmanAnimation
			uiimageAnimationLoop.gameObject.SetActive(False)
		Next
		For Each uiimageAnimationLoop2 As UIImageAnimationLoop In Me.CardCupheadAnimation
			uiimageAnimationLoop2.gameObject.SetActive(False)
		Next
	End Sub

	' Token: 0x06001226 RID: 4646 RVA: 0x000A8D28 File Offset: 0x000A7128
	Public Sub Init(playerId As PlayerId)
		Me.canvas.alpha = 0F
		Me.SetPlayer(playerId)
		Me.player = TowerOfPowerLevelGameInfo.PLAYER_STATS(CInt(playerId))
		Me.SetTitlePlayersName()
		Me.SetTokenCount()
		Me.[Continue].gameObject.SetActive(Me.player.HP = 0)
		Me.CountDown_text.gameObject.SetActive(Me.player.HP = 0)
		Me.SetAnimation()
		If Me.player.HP = 0 Then
			MyBase.StartCoroutine(Me.update_countdown_cr())
		End If
	End Sub

	' Token: 0x06001227 RID: 4647 RVA: 0x000A8DC4 File Offset: 0x000A71C4
	Private Sub SetAnimation()
		If Me.yourPlayerIsMugman Then
			Me.CardMugmanAnimation(If((Me.player.HP <> 0), 0, 1)).gameObject.SetActive(True)
		Else
			Me.CardCupheadAnimation(If((Me.player.HP <> 0), 0, 1)).gameObject.SetActive(True)
		End If
	End Sub

	' Token: 0x06001228 RID: 4648 RVA: 0x000A8E3B File Offset: 0x000A723B
	Private Sub SetPlayer(playerId As PlayerId)
		If playerId = PlayerId.PlayerOne Then
			Me.yourPlayerIsMugman = PlayerManager.player1IsMugman
		Else
			Me.yourPlayerIsMugman = Not PlayerManager.player1IsMugman
		End If
	End Sub

	' Token: 0x06001229 RID: 4649 RVA: 0x000A8E64 File Offset: 0x000A7264
	Private Sub SetTitlePlayersName()
		If Me.yourPlayerIsMugman Then
		End If
	End Sub

	' Token: 0x0600122A RID: 4650 RVA: 0x000A8E90 File Offset: 0x000A7290
	Public Sub SetTokenCount()
		Dim tokenCount As Integer = Me.player.tokenCount
		Me.TokenLeft_text.text = "Token: " + tokenCount
	End Sub

	' Token: 0x0600122B RID: 4651 RVA: 0x000A8EC4 File Offset: 0x000A72C4
	Public Iterator Function update_countdown_cr() As IEnumerator
		While Me.countDown > 0
			Yield CupheadTime.WaitForSeconds(Me, 1F)
			Me.countDown -= 1
			Me.UpdateCountDownText()
		End While
		Yield Nothing
		SceneLoader.ContinueTowerOfPower()
		Return
	End Function

	' Token: 0x0600122C RID: 4652 RVA: 0x000A8EDF File Offset: 0x000A72DF
	Private Sub UpdateCountDownText()
		Me.CountDown_text.text = Me.countDown.ToString()
	End Sub

	' Token: 0x0600122D RID: 4653 RVA: 0x000A8EFD File Offset: 0x000A72FD
	Private Sub OnDestroy()
		Me.[Continue] = Nothing
		Me.CountDown_text = Nothing
		Me.CardCupheadAnimation.Clear()
		Me.CardMugmanAnimation.Clear()
	End Sub

	' Token: 0x0600122E RID: 4654 RVA: 0x000A8F23 File Offset: 0x000A7323
	Public Sub SetAlpha(value As Single)
		Me.canvas.alpha = value
	End Sub

	' Token: 0x04001B9E RID: 7070
	<SerializeField()>
	Private PlayerName As SpriteRenderer

	' Token: 0x04001B9F RID: 7071
	<SerializeField()>
	Private CupheadNameData As Sprite

	' Token: 0x04001BA0 RID: 7072
	<SerializeField()>
	Private CupmanNameData As Sprite

	' Token: 0x04001BA1 RID: 7073
	<Space(10F)>
	<SerializeField()>
	Private TokenLeft_text As Text

	' Token: 0x04001BA2 RID: 7074
	<SerializeField()>
	Private [Continue] As Text

	' Token: 0x04001BA3 RID: 7075
	<SerializeField()>
	Private CountDown_text As Text

	' Token: 0x04001BA4 RID: 7076
	<SerializeField()>
	Private CardCupheadAnimation As List(Of UIImageAnimationLoop) = New List(Of UIImageAnimationLoop)()

	' Token: 0x04001BA5 RID: 7077
	<SerializeField()>
	Private CardMugmanAnimation As List(Of UIImageAnimationLoop) = New List(Of UIImageAnimationLoop)()

	' Token: 0x04001BA6 RID: 7078
	<SerializeField()>
	Private canvas As CanvasGroup

	' Token: 0x04001BA7 RID: 7079
	Private yourPlayerIsMugman As Boolean

	' Token: 0x04001BA8 RID: 7080
	Private countDown As Integer = 10

	' Token: 0x04001BA9 RID: 7081
	Private player As PlayersStatsBossesHub
End Class
