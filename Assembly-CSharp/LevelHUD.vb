Imports System
Imports UnityEngine

' Token: 0x0200048B RID: 1163
Public Class LevelHUD
	Inherits AbstractMonoBehaviour

	' Token: 0x170002D0 RID: 720
	' (get) Token: 0x06001237 RID: 4663 RVA: 0x000A9092 File Offset: 0x000A7492
	' (set) Token: 0x06001238 RID: 4664 RVA: 0x000A9099 File Offset: 0x000A7499
	Public Shared Property Current As LevelHUD

	' Token: 0x170002D1 RID: 721
	' (get) Token: 0x06001239 RID: 4665 RVA: 0x000A90A1 File Offset: 0x000A74A1
	Public ReadOnly Property Canvas As Canvas
		Get
			Return Me.canvas
		End Get
	End Property

	' Token: 0x0600123A RID: 4666 RVA: 0x000A90AC File Offset: 0x000A74AC
	Protected Overrides Sub Awake()
		MyBase.Awake()
		AddHandler LevelGUI.DebugOnDisableGuiEvent, AddressOf Me.OnDisableGUI
		AddHandler PlayerManager.OnPlayerJoinedEvent, AddressOf Me.OnPlayerJoined
		AddHandler PlayerManager.OnPlayerLeaveEvent, AddressOf Me.OnPlayerLeave
		LevelHUD.Current = Me
	End Sub

	' Token: 0x0600123B RID: 4667 RVA: 0x000A90F8 File Offset: 0x000A74F8
	Private Sub OnDestroy()
		RemoveHandler LevelGUI.DebugOnDisableGuiEvent, AddressOf Me.OnDisableGUI
		RemoveHandler PlayerManager.OnPlayerJoinedEvent, AddressOf Me.OnPlayerJoined
		RemoveHandler PlayerManager.OnPlayerLeaveEvent, AddressOf Me.OnPlayerLeave
		If LevelHUD.Current Is Me Then
			LevelHUD.Current = Nothing
		End If
	End Sub

	' Token: 0x0600123C RID: 4668 RVA: 0x000A914E File Offset: 0x000A754E
	Private Sub Start()
		Me.canvas.worldCamera = CupheadLevelCamera.Current.camera
	End Sub

	' Token: 0x0600123D RID: 4669 RVA: 0x000A9168 File Offset: 0x000A7568
	Public Sub LevelInit()
		Dim player As AbstractPlayerController = PlayerManager.GetPlayer(PlayerId.PlayerOne)
		Me.levelHudTemplate = Global.UnityEngine.[Object].Instantiate(Of LevelHUDPlayer)(Me.cuphead)
		Me.levelHudTemplate.gameObject.SetActive(False)
		If PlayerManager.Multiplayer Then
			Dim player2 As AbstractPlayerController = PlayerManager.GetPlayer(PlayerId.PlayerTwo)
			Me.mugman = Global.UnityEngine.[Object].Instantiate(Of LevelHUDPlayer)(Me.levelHudTemplate)
			Me.mugman.gameObject.SetActive(True)
			Me.mugman.transform.SetParent(Me.cuphead.transform.parent, False)
			Me.mugman.Init(player2, False)
		End If
		Me.cuphead.Init(player, False)
	End Sub

	' Token: 0x0600123E RID: 4670 RVA: 0x000A920C File Offset: 0x000A760C
	Private Sub OnDisableGUI()
		Me.canvas.enabled = False
	End Sub

	' Token: 0x0600123F RID: 4671 RVA: 0x000A921C File Offset: 0x000A761C
	Private Sub OnPlayerJoined(player As PlayerId)
		If player = PlayerId.PlayerTwo Then
			Dim player2 As AbstractPlayerController = PlayerManager.GetPlayer(PlayerId.PlayerTwo)
			Me.mugman = Global.UnityEngine.[Object].Instantiate(Of LevelHUDPlayer)(Me.levelHudTemplate)
			Me.mugman.gameObject.SetActive(True)
			Me.mugman.transform.SetParent(Me.cuphead.transform.parent, False)
			Me.mugman.Init(player2, Not Level.IsTowerOfPowerMain)
		End If
	End Sub

	' Token: 0x06001240 RID: 4672 RVA: 0x000A928E File Offset: 0x000A768E
	Private Sub OnPlayerLeave(player As PlayerId)
		If player = PlayerId.PlayerTwo Then
			Global.UnityEngine.[Object].Destroy(Me.mugman.gameObject)
		End If
	End Sub

	' Token: 0x04001BAD RID: 7085
	<SerializeField()>
	Private canvas As Canvas

	' Token: 0x04001BAE RID: 7086
	<Space(10F)>
	<SerializeField()>
	Private cuphead As LevelHUDPlayer

	' Token: 0x04001BAF RID: 7087
	Private levelHudTemplate As LevelHUDPlayer

	' Token: 0x04001BB0 RID: 7088
	Private mugman As LevelHUDPlayer
End Class
