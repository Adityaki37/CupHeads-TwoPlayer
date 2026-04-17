Imports System
Imports System.Collections
Imports TMPro
Imports UnityEngine
Imports UnityEngine.UI

' Token: 0x02000482 RID: 1154
Public Class LevelNewPlayerGUI
	Inherits AbstractMonoBehaviour

	' Token: 0x170002C8 RID: 712
	' (get) Token: 0x060011E0 RID: 4576 RVA: 0x000A7347 File Offset: 0x000A5747
	' (set) Token: 0x060011E1 RID: 4577 RVA: 0x000A734E File Offset: 0x000A574E
	Public Shared Property Current As LevelNewPlayerGUI

	' Token: 0x060011E2 RID: 4578 RVA: 0x000A7358 File Offset: 0x000A5758
	Protected Overrides Sub Awake()
		MyBase.Awake()
		If PlayerManager.player1IsMugman Then
			Me.card.sprite = Me.cupheadCard
		End If
		LevelNewPlayerGUI.Current = Me
		Me.canvasGroup = MyBase.GetComponent(Of CanvasGroup)()
		Me.canvasGroup.alpha = 0F
		MyBase.gameObject.SetActive(False)
	End Sub

	' Token: 0x060011E3 RID: 4579 RVA: 0x000A73B4 File Offset: 0x000A57B4
	Private Sub OnDestroy()
		If LevelNewPlayerGUI.Current Is Me Then
			LevelNewPlayerGUI.Current = Nothing
		End If
	End Sub

	' Token: 0x060011E4 RID: 4580 RVA: 0x000A73CC File Offset: 0x000A57CC
	Public Sub Init()
		MyBase.gameObject.SetActive(True)
		If OnlineManager.Instance.[Interface].SupportsMultipleUsers AndAlso OnlineManager.Instance.[Interface].GetUser(PlayerId.PlayerTwo) IsNot Nothing Then
			Me.localizationHelper.ApplyTranslation(Localization.Find("PlayerTwoJoinedWithUser"), New LocalizationHelper.LocalizationSubtext() { New LocalizationHelper.LocalizationSubtext("USERNAME", OnlineManager.Instance.[Interface].GetUser(PlayerId.PlayerTwo).Name, True) })
		End If
		MyBase.StartCoroutine(Me.tweenIn_cr())
		MyBase.StartCoroutine(Me.text_cr())
	End Sub

	' Token: 0x060011E5 RID: 4581 RVA: 0x000A7470 File Offset: 0x000A5870
	Protected Iterator Function tweenIn_cr() As IEnumerator
		MyBase.animator.Play("In")
		Dim t As Single = 0F
		AudioManager.Play("player_joined")
		PauseManager.Pause()
		While t < 0.2F
			Dim val As Single = t / 0.2F
			Me.canvasGroup.alpha = Mathf.Lerp(0F, 1F, val)
			t += Time.deltaTime
			Yield Nothing
		End While
		Me.canvasGroup.alpha = 1F
		Yield New WaitForSeconds(2F)
		MyBase.animator.Play("Out")
		MyBase.StartCoroutine(Me.tweenOut_cr())
		Return
	End Function

	' Token: 0x060011E6 RID: 4582 RVA: 0x000A748C File Offset: 0x000A588C
	Protected Iterator Function tweenOut_cr() As IEnumerator
		Dim t As Single = 0F
		While t < 0.2F
			Dim val As Single = t / 0.2F
			Me.canvasGroup.alpha = Mathf.Lerp(1F, 0F, val)
			t += Time.deltaTime
			Yield Nothing
		End While
		Me.canvasGroup.alpha = 0F
		While InterruptingPrompt.IsInterrupting()
			Yield Nothing
		End While
		PauseManager.Unpause()
		MyBase.gameObject.SetActive(False)
		Return
	End Function

	' Token: 0x060011E7 RID: 4583 RVA: 0x000A74A8 File Offset: 0x000A58A8
	Private Iterator Function text_cr() As IEnumerator
		While True
			Me.text.color = Color.white
			Yield New WaitForSeconds(0.041666668F)
			Me.text.color = If((Not PlayerManager.player1IsMugman), Me.mugmanColor, Me.cupheadColor)
			Yield New WaitForSeconds(0.041666668F)
		End While
		Return
	End Function

	' Token: 0x04001B67 RID: 7015
	<SerializeField()>
	Private background As Image

	' Token: 0x04001B68 RID: 7016
	<SerializeField()>
	Private card As Image

	' Token: 0x04001B69 RID: 7017
	<SerializeField()>
	Private cupheadCard As Sprite

	' Token: 0x04001B6A RID: 7018
	<SerializeField()>
	Private text As TextMeshProUGUI

	' Token: 0x04001B6B RID: 7019
	<SerializeField()>
	Private localizationHelper As LocalizationHelper

	' Token: 0x04001B6C RID: 7020
	<SerializeField()>
	Private cupheadColor As Color

	' Token: 0x04001B6D RID: 7021
	<SerializeField()>
	Private mugmanColor As Color

	' Token: 0x04001B6E RID: 7022
	Private canvasGroup As CanvasGroup
End Class
