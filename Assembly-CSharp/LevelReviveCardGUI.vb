Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000485 RID: 1157
<RequireComponent(GetType(CanvasGroup))>
Public Class LevelReviveCardGUI
	Inherits AbstractMonoBehaviour

	' Token: 0x06001214 RID: 4628 RVA: 0x000A85F8 File Offset: 0x000A69F8
	Protected Overrides Sub Awake()
		MyBase.Awake()
		LevelReviveCardGUI.Current = Me
		Me.canvasGroup = MyBase.GetComponent(Of CanvasGroup)()
		MyBase.gameObject.SetActive(False)
		Me.input = New CupheadInput.AnyPlayerInput(False)
		Me.helpCanvasGroup.alpha = 0F
		Me.ignoreGlobalTime = True
		Me.timeLayer = CupheadTime.Layer.UI
		Me.state = LevelReviveCardGUI.State.Init
	End Sub

	' Token: 0x06001215 RID: 4629 RVA: 0x000A865A File Offset: 0x000A6A5A
	Private Sub OnDestroy()
		LevelReviveCardGUI.Current = Nothing
	End Sub

	' Token: 0x06001216 RID: 4630 RVA: 0x000A8662 File Offset: 0x000A6A62
	Private Sub Update()
		If Me.state <> LevelReviveCardGUI.State.Ready Then
			Return
		End If
		If PlayerManager.GetPlayerInput(Me.deadPlayer).GetButtonDown(13) Then
			Me.RevivePlayer()
			Me.state = LevelReviveCardGUI.State.Exiting
		End If
	End Sub

	' Token: 0x06001217 RID: 4631 RVA: 0x000A8698 File Offset: 0x000A6A98
	Private Sub RevivePlayer()
		TowerOfPowerLevelGameInfo.PLAYER_STATS(CInt(Me.deadPlayer)).HP = 3
		TowerOfPowerLevelGameInfo.PLAYER_STATS(CInt(Me.deadPlayer)).BonusHP = 0
		TowerOfPowerLevelGameInfo.PLAYER_STATS(CInt(Me.deadPlayer)).SuperCharge = 0F
		TowerOfPowerLevelGameInfo.PLAYER_STATS(CInt(Me.deadPlayer)).tokenCount -= 1
		Me.playerOneCard.SetTokenCount()
		Me.playerTwoCard.SetTokenCount()
		SceneLoader.ContinueTowerOfPower()
	End Sub

	' Token: 0x06001218 RID: 4632 RVA: 0x000A8714 File Offset: 0x000A6B14
	Public Sub [In]()
		MyBase.gameObject.SetActive(True)
		Me.playerOneCard.Init(PlayerId.PlayerOne)
		Me.playerTwoCard.Init(PlayerId.PlayerTwo)
		If TowerOfPowerLevelGameInfo.PLAYER_STATS(0).HP = 0 Then
			Me.deadPlayer = PlayerId.PlayerOne
		Else
			Me.deadPlayer = PlayerId.PlayerTwo
		End If
		MyBase.StartCoroutine(Me.in_cr())
	End Sub

	' Token: 0x06001219 RID: 4633 RVA: 0x000A8778 File Offset: 0x000A6B78
	Private Iterator Function in_cr() As IEnumerator
		AudioManager.Play("level_menu_card_up")
		Yield MyBase.TweenValue(0F, 1F, 0.05F, EaseUtils.EaseType.linear, AddressOf Me.SetAlpha)
		Yield New WaitForSeconds(1F)
		AudioManager.Play("player_die_vinylscratch")
		AudioManager.HandleSnapshot(AudioManager.Snapshots.Death.ToString(), 4F)
		If Not Level.IsChessBoss Then
			AudioManager.ChangeBGMPitch(0.7F, 6F)
		End If
		MyBase.TweenValue(0F, 1F, 0.3F, EaseUtils.EaseType.easeOutCubic, AddressOf Me.SetCardValue)
		Yield Nothing
		Me.state = LevelReviveCardGUI.State.Ready
		Return
	End Function

	' Token: 0x0600121A RID: 4634 RVA: 0x000A8793 File Offset: 0x000A6B93
	Private Sub SetAlpha(value As Single)
		Me.canvasGroup.alpha = value
	End Sub

	' Token: 0x0600121B RID: 4635 RVA: 0x000A87A4 File Offset: 0x000A6BA4
	Private Sub SetCardValue(value As Single)
		Me.playerOneCard.SetAlpha(value)
		Me.playerTwoCard.SetAlpha(value)
		Me.helpCanvasGroup.alpha = value
		Me.playerOneCard.transform.SetLocalEulerAngles(Nothing, Nothing, New Single?(Mathf.Lerp(15F, 4F, value)))
		Me.playerTwoCard.transform.SetLocalEulerAngles(Nothing, Nothing, New Single?(Mathf.Lerp(-15F, -4F, value)))
	End Sub

	' Token: 0x04001B85 RID: 7045
	Public Shared Current As LevelReviveCardGUI

	' Token: 0x04001B86 RID: 7046
	<Space(10F)>
	<SerializeField()>
	Private playerOneCard As TowerOfPowerContinueCardGUI

	' Token: 0x04001B87 RID: 7047
	<Space(10F)>
	<SerializeField()>
	Private playerTwoCard As TowerOfPowerContinueCardGUI

	' Token: 0x04001B88 RID: 7048
	<Space(10F)>
	<SerializeField()>
	Private helpCanvasGroup As CanvasGroup

	' Token: 0x04001B89 RID: 7049
	Private state As LevelReviveCardGUI.State

	' Token: 0x04001B8A RID: 7050
	Private input As CupheadInput.AnyPlayerInput

	' Token: 0x04001B8B RID: 7051
	Private canvasGroup As CanvasGroup

	' Token: 0x04001B8C RID: 7052
	Private deadPlayer As PlayerId

	' Token: 0x02000486 RID: 1158
	Private Enum State
		' Token: 0x04001B8E RID: 7054
		Init
		' Token: 0x04001B8F RID: 7055
		Ready
		' Token: 0x04001B90 RID: 7056
		Exiting
	End Enum
End Class
