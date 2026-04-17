Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x0200044A RID: 1098
<RequireComponent(GetType(CanvasGroup))>
Public MustInherit Class AbstractPauseGUI
	Inherits AbstractMonoBehaviour

	' Token: 0x17000290 RID: 656
	' (get) Token: 0x06001062 RID: 4194 RVA: 0x000937BE File Offset: 0x00091BBE
	' (set) Token: 0x06001063 RID: 4195 RVA: 0x000937C6 File Offset: 0x00091BC6
	Public Property state As AbstractPauseGUI.State

	' Token: 0x17000291 RID: 657
	' (get) Token: 0x06001064 RID: 4196 RVA: 0x000937CF File Offset: 0x00091BCF
	Protected Overridable ReadOnly Property LevelInputButton As CupheadButton
		Get
			Return CupheadButton.Pause
		End Get
	End Property

	' Token: 0x17000292 RID: 658
	' (get) Token: 0x06001065 RID: 4197 RVA: 0x000937D2 File Offset: 0x00091BD2
	Protected Overridable ReadOnly Property UIInputButton As CupheadButton
		Get
			Return CupheadButton.EquipMenu
		End Get
	End Property

	' Token: 0x17000293 RID: 659
	' (get) Token: 0x06001066 RID: 4198 RVA: 0x000937D6 File Offset: 0x00091BD6
	Protected Overridable ReadOnly Property CheckedActionSet As AbstractPauseGUI.InputActionSet
		Get
			Return AbstractPauseGUI.InputActionSet.LevelInput
		End Get
	End Property

	' Token: 0x17000294 RID: 660
	' (get) Token: 0x06001067 RID: 4199
	Protected MustOverride ReadOnly Property CanPause As Boolean

	' Token: 0x17000295 RID: 661
	' (get) Token: 0x06001068 RID: 4200 RVA: 0x000937D9 File Offset: 0x00091BD9
	Protected Overridable ReadOnly Property CanUnpause As Boolean
		Get
			Return False
		End Get
	End Property

	' Token: 0x17000296 RID: 662
	' (get) Token: 0x06001069 RID: 4201 RVA: 0x000937DC File Offset: 0x00091BDC
	Protected Overridable ReadOnly Property RespondToDeadPlayer As Boolean
		Get
			Return False
		End Get
	End Property

	' Token: 0x0600106A RID: 4202 RVA: 0x000937DF File Offset: 0x00091BDF
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.canvasGroup = MyBase.GetComponent(Of CanvasGroup)()
		Me.HideImmediate()
	End Sub

	' Token: 0x0600106B RID: 4203 RVA: 0x000937F9 File Offset: 0x00091BF9
	Protected Overridable Sub Update()
		Me.UpdateInput()
	End Sub

	' Token: 0x0600106C RID: 4204 RVA: 0x00093804 File Offset: 0x00091C04
	Private Sub UpdateInput()
		If Not Me.CanPause Then
			Return
		End If
		Dim flag As Boolean = If((Me.CheckedActionSet <> AbstractPauseGUI.InputActionSet.LevelInput), Me.GetButtonDown(Me.UIInputButton), Me.GetButtonDown(Me.LevelInputButton))
		If flag Then
			MyBase.StartCoroutine(Me.ShowPauseMenu())
		End If
	End Sub

	' Token: 0x0600106D RID: 4205 RVA: 0x0009385C File Offset: 0x00091C5C
	Public Iterator Function ShowPauseMenu() As IEnumerator
		If MapEventNotification.Current IsNot Nothing Then
			While MapEventNotification.Current.showing
				Yield Nothing
			End While
		End If
		If Me.state = AbstractPauseGUI.State.Unpaused AndAlso PauseManager.state = PauseManager.State.Unpaused Then
			Me.Pause()
		ElseIf Me.state = AbstractPauseGUI.State.Paused AndAlso Me.CanUnpause Then
			Me.Unpause()
		End If
		Return
	End Function

	' Token: 0x0600106E RID: 4206 RVA: 0x00093877 File Offset: 0x00091C77
	Public Overridable Sub Init(checkIfDead As Boolean, options As OptionsGUI, achievements As AchievementsGUI, restartTowerConfirmGUI As RestartTowerConfirmGUI)
		Me.input = New CupheadInput.AnyPlayerInput(checkIfDead)
	End Sub

	' Token: 0x0600106F RID: 4207 RVA: 0x00093885 File Offset: 0x00091C85
	Public Overridable Sub Init(checkIfDead As Boolean, options As OptionsGUI, achievements As AchievementsGUI)
		Me.input = New CupheadInput.AnyPlayerInput(checkIfDead)
	End Sub

	' Token: 0x06001070 RID: 4208 RVA: 0x00093893 File Offset: 0x00091C93
	Public Overridable Sub Init(checkIfDead As Boolean)
		Me.input = New CupheadInput.AnyPlayerInput(checkIfDead)
	End Sub

	' Token: 0x06001071 RID: 4209 RVA: 0x000938A1 File Offset: 0x00091CA1
	Protected Overridable Sub Pause()
		If Me.state = AbstractPauseGUI.State.Unpaused AndAlso PauseManager.state = PauseManager.State.Unpaused Then
			MyBase.StartCoroutine(Me.pause_cr())
		End If
	End Sub

	' Token: 0x06001072 RID: 4210 RVA: 0x000938C5 File Offset: 0x00091CC5
	Protected Overridable Sub Unpause()
		If Me.state = AbstractPauseGUI.State.Paused Then
			MyBase.StartCoroutine(Me.unpause_cr())
		End If
	End Sub

	' Token: 0x06001073 RID: 4211 RVA: 0x000938E0 File Offset: 0x00091CE0
	Protected Overridable Sub OnPause()
		Me.OnPauseSound()
		If PlatformHelper.GarbageCollectOnPause Then
			GC.Collect()
		End If
	End Sub

	' Token: 0x06001074 RID: 4212 RVA: 0x000938F7 File Offset: 0x00091CF7
	Protected Overridable Sub OnPauseComplete()
	End Sub

	' Token: 0x06001075 RID: 4213 RVA: 0x000938F9 File Offset: 0x00091CF9
	Protected Overridable Sub OnUnpause()
		If PlatformHelper.GarbageCollectOnPause Then
			GC.Collect()
		End If
		Me.OnUnpauseSound()
	End Sub

	' Token: 0x06001076 RID: 4214 RVA: 0x00093910 File Offset: 0x00091D10
	Protected Overridable Sub OnUnpauseComplete()
	End Sub

	' Token: 0x06001077 RID: 4215 RVA: 0x00093914 File Offset: 0x00091D14
	Protected Overridable Sub OnPauseSound()
		AudioManager.HandleSnapshot(AudioManager.Snapshots.Paused.ToString(), 0.15F)
		AudioManager.PauseAllSFX()
	End Sub

	' Token: 0x06001078 RID: 4216 RVA: 0x00093940 File Offset: 0x00091D40
	Protected Overridable Sub OnUnpauseSound()
		AudioManager.SnapshotReset(If((Not Me.isWorldMap), Level.Current.CurrentScene.ToString(), PlayerData.Data.CurrentMap.ToString()), 0.1F)
		AudioManager.UnpauseAllSFX()
	End Sub

	' Token: 0x06001079 RID: 4217 RVA: 0x0009399C File Offset: 0x00091D9C
	Protected Overridable Sub HideImmediate()
		Me.canvasGroup.alpha = 0F
		Me.SetInteractable(False)
	End Sub

	' Token: 0x0600107A RID: 4218 RVA: 0x000939B5 File Offset: 0x00091DB5
	Protected Overridable Sub ShowImmediate()
		Me.canvasGroup.alpha = 1F
		Me.SetInteractable(True)
	End Sub

	' Token: 0x0600107B RID: 4219 RVA: 0x000939CE File Offset: 0x00091DCE
	Private Sub SetInteractable(interactable As Boolean)
		Me.canvasGroup.interactable = interactable
		Me.canvasGroup.blocksRaycasts = interactable
	End Sub

	' Token: 0x0600107C RID: 4220 RVA: 0x000939E8 File Offset: 0x00091DE8
	Protected Iterator Function pause_cr() As IEnumerator
		Vibrator.StopVibrating(PlayerId.PlayerOne)
		Vibrator.StopVibrating(PlayerId.PlayerTwo)
		Me.OnPause()
		Me.PauseGameplay()
		Me.SetInteractable(True)
		Yield MyBase.StartCoroutine(Me.animate_cr(Me.InTime, AddressOf Me.InAnimation, 0F, 1F))
		Me.state = AbstractPauseGUI.State.Paused
		Me.OnPauseComplete()
		Return
	End Function

	' Token: 0x0600107D RID: 4221 RVA: 0x00093A04 File Offset: 0x00091E04
	Protected Iterator Function unpause_cr() As IEnumerator
		Me.OnUnpause()
		Me.SetInteractable(True)
		Me.UnpauseGameplay()
		Yield MyBase.StartCoroutine(Me.animate_cr(Me.OutTime, AddressOf Me.OutAnimation, 1F, 0F))
		Me.state = AbstractPauseGUI.State.Unpaused
		Me.SetInteractable(False)
		Me.OnUnpauseComplete()
		Return
	End Function

	' Token: 0x0600107E RID: 4222 RVA: 0x00093A1F File Offset: 0x00091E1F
	Protected Overridable Sub PauseGameplay()
		PauseManager.Pause()
	End Sub

	' Token: 0x0600107F RID: 4223 RVA: 0x00093A26 File Offset: 0x00091E26
	Protected Overridable Sub UnpauseGameplay()
		PauseManager.Unpause()
	End Sub

	' Token: 0x06001080 RID: 4224 RVA: 0x00093A30 File Offset: 0x00091E30
	Private Iterator Function animate_cr(time As Single, anim As AbstractPauseGUI.AnimationDelegate, start As Single, [end] As Single) As IEnumerator
		anim(0F)
		Me.state = AbstractPauseGUI.State.Animating
		Me.canvasGroup.alpha = start
		Dim t As Single = 0F
		While t < time
			Dim val As Single = t / time
			Me.canvasGroup.alpha = Mathf.Lerp(start, [end], val)
			anim(val)
			t += Time.deltaTime
			Yield Nothing
		End While
		Me.canvasGroup.alpha = [end]
		anim(1F)
		Return
	End Function

	' Token: 0x06001081 RID: 4225
	Protected MustOverride Sub InAnimation(i As Single)

	' Token: 0x06001082 RID: 4226
	Protected MustOverride Sub OutAnimation(i As Single)

	' Token: 0x17000297 RID: 663
	' (get) Token: 0x06001083 RID: 4227 RVA: 0x00093A68 File Offset: 0x00091E68
	Protected Overridable ReadOnly Property InTime As Single
		Get
			Return 0.15F
		End Get
	End Property

	' Token: 0x17000298 RID: 664
	' (get) Token: 0x06001084 RID: 4228 RVA: 0x00093A6F File Offset: 0x00091E6F
	Protected Overridable ReadOnly Property OutTime As Single
		Get
			Return 0.15F
		End Get
	End Property

	' Token: 0x06001085 RID: 4229 RVA: 0x00093A76 File Offset: 0x00091E76
	Protected Function GetButtonDown(button As CupheadButton) As Boolean
		Return(Not(AbstractEquipUI.Current IsNot Nothing) OrElse AbstractEquipUI.Current.CurrentState <> AbstractEquipUI.ActiveState.Active OrElse button <> CupheadButton.EquipMenu) AndAlso Me.input.GetButtonDown(button)
	End Function

	' Token: 0x06001086 RID: 4230 RVA: 0x00093AB6 File Offset: 0x00091EB6
	Protected Sub MenuSelectSound()
		AudioManager.Play("level_menu_select")
	End Sub

	' Token: 0x06001087 RID: 4231 RVA: 0x00093AC2 File Offset: 0x00091EC2
	Protected Sub MenuMoveSound()
		AudioManager.Play("level_menu_move")
	End Sub

	' Token: 0x06001088 RID: 4232 RVA: 0x00093ACE File Offset: 0x00091ECE
	Protected Function GetButton(button As CupheadButton) As Boolean
		Return Me.input.GetButton(button)
	End Function

	' Token: 0x040019B6 RID: 6582
	<SerializeField()>
	Private isWorldMap As Boolean

	' Token: 0x040019B8 RID: 6584
	Protected canvasGroup As CanvasGroup

	' Token: 0x040019B9 RID: 6585
	Private input As CupheadInput.AnyPlayerInput

	' Token: 0x0200044B RID: 1099
	Public Enum State
		' Token: 0x040019BB RID: 6587
		Unpaused
		' Token: 0x040019BC RID: 6588
		Paused
		' Token: 0x040019BD RID: 6589
		Animating
	End Enum

	' Token: 0x0200044C RID: 1100
	Public Enum InputActionSet
		' Token: 0x040019BF RID: 6591
		LevelInput
		' Token: 0x040019C0 RID: 6592
		UIInput
	End Enum

	' Token: 0x0200044D RID: 1101
	' (Invoke) Token: 0x0600108A RID: 4234
	Private Delegate Sub AnimationDelegate(i As Single)
End Class
