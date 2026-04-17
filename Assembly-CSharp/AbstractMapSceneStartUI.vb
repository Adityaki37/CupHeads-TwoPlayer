Imports System
Imports System.Collections
Imports System.Diagnostics
Imports Rewired
Imports UnityEngine

' Token: 0x02000981 RID: 2433
Public Class AbstractMapSceneStartUI
	Inherits AbstractMonoBehaviour

	' Token: 0x1700049C RID: 1180
	' (get) Token: 0x060038C8 RID: 14536 RVA: 0x00205155 File Offset: 0x00203555
	' (set) Token: 0x060038C9 RID: 14537 RVA: 0x0020515D File Offset: 0x0020355D
	Public Property CurrentState As AbstractMapSceneStartUI.State

	' Token: 0x1400006E RID: 110
	' (add) Token: 0x060038CA RID: 14538 RVA: 0x00205168 File Offset: 0x00203568
	' (remove) Token: 0x060038CB RID: 14539 RVA: 0x002051A0 File Offset: 0x002035A0
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnLoadLevelEvent As Action

	' Token: 0x1400006F RID: 111
	' (add) Token: 0x060038CC RID: 14540 RVA: 0x002051D8 File Offset: 0x002035D8
	' (remove) Token: 0x060038CD RID: 14541 RVA: 0x00205210 File Offset: 0x00203610
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnBackEvent As Action

	' Token: 0x1700049D RID: 1181
	' (get) Token: 0x060038CE RID: 14542 RVA: 0x00205248 File Offset: 0x00203648
	Protected ReadOnly Property Able As Boolean
		Get
			Return Me.CurrentState = AbstractMapSceneStartUI.State.Active AndAlso AbstractEquipUI.Current.CurrentState = AbstractEquipUI.ActiveState.Inactive AndAlso Map.Current.CurrentState = Map.State.Ready AndAlso Not InterruptingPrompt.IsInterrupting() AndAlso (Not(Map.Current IsNot Nothing) OrElse Map.Current.CurrentState <> Map.State.Graveyard)
		End Get
	End Property

	' Token: 0x060038CF RID: 14543 RVA: 0x002052B5 File Offset: 0x002036B5
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.timeLayer = CupheadTime.Layer.UI
		Me.ignoreGlobalTime = True
		Me.canvasGroup = MyBase.GetComponent(Of CanvasGroup)()
		Me.SetAlpha(0F)
	End Sub

	' Token: 0x060038D0 RID: 14544 RVA: 0x002052E2 File Offset: 0x002036E2
	Protected Overridable Sub Start()
		AddHandler PlayerManager.OnPlayerJoinedEvent, AddressOf Me.OnPlayerJoined
		AddHandler PlayerManager.OnPlayerJoinedEvent, AddressOf Me.OnPlayerLeft
	End Sub

	' Token: 0x060038D1 RID: 14545 RVA: 0x00205306 File Offset: 0x00203706
	Private Sub OnDestroy()
		RemoveHandler PlayerManager.OnPlayerJoinedEvent, AddressOf Me.OnPlayerJoined
		RemoveHandler PlayerManager.OnPlayerJoinedEvent, AddressOf Me.OnPlayerLeft
	End Sub

	' Token: 0x060038D2 RID: 14546 RVA: 0x0020532A File Offset: 0x0020372A
	Protected Function GetButtonDown(button As CupheadButton) As Boolean
		Return Me.Able AndAlso Not InterruptingPrompt.IsInterrupting() AndAlso (Me.player IsNot Nothing AndAlso Me.player.GetButtonDown(CInt(button)))
	End Function

	' Token: 0x060038D3 RID: 14547 RVA: 0x00205362 File Offset: 0x00203762
	Private Sub OnPlayerJoined(playerId As PlayerId)
	End Sub

	' Token: 0x060038D4 RID: 14548 RVA: 0x00205364 File Offset: 0x00203764
	Private Sub OnPlayerLeft(playerId As PlayerId)
	End Sub

	' Token: 0x060038D5 RID: 14549 RVA: 0x00205366 File Offset: 0x00203766
	Protected Sub LoadLevel()
		Me.CurrentState = AbstractMapSceneStartUI.State.Loading
		If Me.OnLoadLevelEvent IsNot Nothing Then
			Me.OnLoadLevelEvent()
		End If
		Me.OnLoadLevelEvent = Nothing
	End Sub

	' Token: 0x060038D6 RID: 14550 RVA: 0x0020538C File Offset: 0x0020378C
	Public Sub [In](playerController As MapPlayerController)
		Me.player = playerController.input.actions
		MyBase.StartCoroutine(Me.fade_cr(1F, AbstractMapSceneStartUI.State.Active))
	End Sub

	' Token: 0x060038D7 RID: 14551 RVA: 0x002053B2 File Offset: 0x002037B2
	Public Sub Out()
		MyBase.StartCoroutine(Me.fade_cr(0F, AbstractMapSceneStartUI.State.Inactive))
		If Me.OnBackEvent IsNot Nothing Then
			Me.OnBackEvent()
		End If
		Me.OnBackEvent = Nothing
	End Sub

	' Token: 0x060038D8 RID: 14552 RVA: 0x002053E4 File Offset: 0x002037E4
	Protected Sub SetAlpha(alpha As Single)
		Me.canvasGroup.alpha = alpha
	End Sub

	' Token: 0x060038D9 RID: 14553 RVA: 0x002053F4 File Offset: 0x002037F4
	Private Iterator Function fade_cr([end] As Single, endState As AbstractMapSceneStartUI.State) As IEnumerator
		Dim t As Single = 0F
		Dim start As Single = Me.canvasGroup.alpha
		Me.CurrentState = AbstractMapSceneStartUI.State.Animating
		While t < 0.2F
			Dim val As Single = t / 0.2F
			Me.SetAlpha(Mathf.Lerp(start, [end], val))
			t += Time.deltaTime
			Yield Nothing
		End While
		Me.SetAlpha([end])
		Me.CurrentState = endState
		Return
	End Function

	' Token: 0x04004076 RID: 16502
	<HideInInspector()>
	Public level As String

	' Token: 0x04004078 RID: 16504
	Private canvasGroup As CanvasGroup

	' Token: 0x04004079 RID: 16505
	Private player As Player

	' Token: 0x02000982 RID: 2434
	Public Enum State
		' Token: 0x0400407D RID: 16509
		Inactive
		' Token: 0x0400407E RID: 16510
		Animating
		' Token: 0x0400407F RID: 16511
		Active
		' Token: 0x04004080 RID: 16512
		Loading
	End Enum
End Class
