Imports System
Imports System.Collections
Imports UnityEngine
Imports UnityEngine.UI

' Token: 0x020003F7 RID: 1015
Public Class DevilCutscene
	Inherits Cutscene

	' Token: 0x06000DD1 RID: 3537 RVA: 0x0008F82D File Offset: 0x0008DC2D
	Protected Overrides Sub Start()
		MyBase.Start()
		Me.input = New CupheadInput.AnyPlayerInput(False)
		CutsceneGUI.Current.pause.pauseAllowed = False
		MyBase.StartCoroutine(Me.main_cr())
	End Sub

	' Token: 0x06000DD2 RID: 3538 RVA: 0x0008F860 File Offset: 0x0008DC60
	Private Iterator Function main_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 1.5F)
		Me.arrowVisible = True
		While Not Me.input.GetAnyButtonDown()
			Yield Nothing
		End While
		Me.arrowVisible = False
		MyBase.animator.SetTrigger("Continue")
		Yield CupheadTime.WaitForSeconds(Me, 1.25F)
		Me.optionSelector.Show()
		Return
	End Function

	' Token: 0x06000DD3 RID: 3539 RVA: 0x0008F87B File Offset: 0x0008DC7B
	Public Sub RefuseDevil()
		Me.ConfirmSFX()
		MyBase.StartCoroutine(Me.refuse_devil_cr())
	End Sub

	' Token: 0x06000DD4 RID: 3540 RVA: 0x0008F890 File Offset: 0x0008DC90
	Public Sub JoinDevil()
		Me.ConfirmSFX()
		MyBase.StartCoroutine(Me.join_devil_cr())
	End Sub

	' Token: 0x06000DD5 RID: 3541 RVA: 0x0008F8A8 File Offset: 0x0008DCA8
	Private Iterator Function join_devil_cr() As IEnumerator
		AudioManager.FadeBGMVolume(0F, 0.5F, True)
		AudioManager.PlayBGMPlaylistManually(False)
		Me.evilVersionsBaseGame.SetActive(True)
		Me.evilVersionsDLC.SetActive(False)
		If DLCManager.DLCEnabled() AndAlso (PlayerData.Data.Loadouts.GetPlayerLoadout(PlayerId.PlayerOne).charm = Charm.charm_chalice OrElse (PlayerManager.Multiplayer AndAlso PlayerData.Data.Loadouts.GetPlayerLoadout(PlayerId.PlayerTwo).charm = Charm.charm_chalice)) Then
			Me.evilVersionsBaseGame.SetActive(False)
			Me.evilVersionsDLC.SetActive(True)
		End If
		MyBase.animator.SetTrigger("joinedDevil")
		Yield CupheadTime.WaitForSeconds(Me, 1.25F)
		Me.arrowVisible = True
		While Not Me.input.GetAnyButtonDown()
			Yield Nothing
		End While
		Me.arrowVisible = False
		MyBase.animator.SetTrigger("fadeOut")
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Fade_Out", 1, False, True)
		MyBase.animator.SetTrigger("Continue")
		MyBase.animator.SetTrigger("fadeIn")
		Me.DevilEvilSFX()
		MyBase.StartCoroutine(Me.blink_cr())
		Yield CupheadTime.WaitForSeconds(Me, 10F)
		Me.KillSFX()
		CreditsScreen.goodEnding = False
		Cutscene.Load(Scenes.scene_title, Scenes.scene_cutscene_credits, SceneLoader.Transition.Iris, SceneLoader.Transition.Fade, SceneLoader.Icon.None)
		Return
	End Function

	' Token: 0x06000DD6 RID: 3542 RVA: 0x0008F8C4 File Offset: 0x0008DCC4
	Private Iterator Function blink_cr() As IEnumerator
		While True
			Yield CupheadTime.WaitForSeconds(Me, Global.UnityEngine.Random.Range(3F, 5F))
			MyBase.animator.SetTrigger("Blink")
		End While
		Return
	End Function

	' Token: 0x06000DD7 RID: 3543 RVA: 0x0008F8E0 File Offset: 0x0008DCE0
	Private Iterator Function refuse_devil_cr() As IEnumerator
		MyBase.animator.SetTrigger("refusedDevil")
		Me.DevilAngrySFX()
		Yield CupheadTime.WaitForSeconds(Me, 1.25F)
		Me.arrowVisible = True
		While Not Me.input.GetAnyButtonDown()
			Yield Nothing
		End While
		Me.arrowVisible = False
		Me.KillSFX()
		SceneLoader.LoadLevel(Levels.Devil, SceneLoader.Transition.Iris, SceneLoader.Icon.Hourglass, Nothing)
		Return
	End Function

	' Token: 0x06000DD8 RID: 3544 RVA: 0x0008F8FC File Offset: 0x0008DCFC
	Private Sub Update()
		If Me.arrowVisible Then
			Me.arrowTransparency = Mathf.Clamp01(Me.arrowTransparency + Time.deltaTime / 0.25F)
		Else
			Me.arrowTransparency = 0F
		End If
		Me.arrow.color = New Color(1F, 1F, 1F, Me.arrowTransparency)
	End Sub

	' Token: 0x06000DD9 RID: 3545 RVA: 0x0008F966 File Offset: 0x0008DD66
	Private Sub ConfirmSFX()
		AudioManager.Play("ui_confirm")
	End Sub

	' Token: 0x06000DDA RID: 3546 RVA: 0x0008F972 File Offset: 0x0008DD72
	Private Sub DevilEvilSFX()
		AudioManager.PlayLoop("sfx_hell_fire")
		AudioManager.Play("devil_laugh")
	End Sub

	' Token: 0x06000DDB RID: 3547 RVA: 0x0008F988 File Offset: 0x0008DD88
	Private Sub DevilAngrySFX()
		AudioManager.PlayLoop("sfx_hell_fire")
	End Sub

	' Token: 0x06000DDC RID: 3548 RVA: 0x0008F994 File Offset: 0x0008DD94
	Private Sub KillSFX()
		AudioManager.FadeSFXVolume("sfx_hell_fire", 0F, 4F)
	End Sub

	' Token: 0x06000DDD RID: 3549 RVA: 0x0008F9AA File Offset: 0x0008DDAA
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me.arrow = Nothing
	End Sub

	' Token: 0x04001737 RID: 5943
	Private input As CupheadInput.AnyPlayerInput

	' Token: 0x04001738 RID: 5944
	<SerializeField()>
	Private arrow As Image

	' Token: 0x04001739 RID: 5945
	<SerializeField()>
	Private optionSelector As DevilCutsceneOptionSelector

	' Token: 0x0400173A RID: 5946
	<SerializeField()>
	Private evilVersionsBaseGame As GameObject

	' Token: 0x0400173B RID: 5947
	<SerializeField()>
	Private evilVersionsDLC As GameObject

	' Token: 0x0400173C RID: 5948
	Private arrowTransparency As Single

	' Token: 0x0400173D RID: 5949
	Private arrowVisible As Boolean
End Class
