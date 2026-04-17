Imports System
Imports UnityEngine

' Token: 0x020009A7 RID: 2471
Public Class LevelSelectScene
	Inherits AbstractMonoBehaviour

	' Token: 0x060039F8 RID: 14840 RVA: 0x0020F3F5 File Offset: 0x0020D7F5
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Cuphead.Init(False)
		CupheadEventSystem.Init()
		Me.UpdatePlayers()
	End Sub

	' Token: 0x060039F9 RID: 14841 RVA: 0x0020F40E File Offset: 0x0020D80E
	Private Sub Update()
		If Input.GetKeyDown(KeyCode.Escape) Then
			Application.Quit()
		End If
	End Sub

	' Token: 0x060039FA RID: 14842 RVA: 0x0020F421 File Offset: 0x0020D821
	Public Sub OnOnePlayerButtonPressed()
		PlayerManager.Multiplayer = False
		Me.UpdatePlayers()
	End Sub

	' Token: 0x060039FB RID: 14843 RVA: 0x0020F42F File Offset: 0x0020D82F
	Public Sub OnTwoPlayersButtonPressed()
		PlayerManager.Multiplayer = True
		Me.UpdatePlayers()
	End Sub

	' Token: 0x060039FC RID: 14844 RVA: 0x0020F440 File Offset: 0x0020D840
	Private Sub UpdatePlayers()
		Dim num As Single = 0.3F
		Me.onePlayerButton.alpha = num
		Me.twoPlayersButton.alpha = num
		If PlayerManager.Multiplayer Then
			Me.twoPlayersButton.alpha = 1F
		Else
			Me.onePlayerButton.alpha = 1F
		End If
	End Sub

	' Token: 0x040041DF RID: 16863
	<SerializeField()>
	Private onePlayerButton As CanvasGroup

	' Token: 0x040041E0 RID: 16864
	<SerializeField()>
	Private twoPlayersButton As CanvasGroup
End Class
