Imports System
Imports UnityEngine

' Token: 0x0200094A RID: 2378
Public Class MapNPCBoatman
	Inherits AbstractMonoBehaviour

	' Token: 0x0600378C RID: 14220 RVA: 0x001FEBF0 File Offset: 0x001FCFF0
	Private Sub Start()
		Me.AddDialoguerEvents()
		Dialoguer.SetGlobalFloat(22, CSng(If((Not PlayerData.Data.GetMapData(Scenes.scene_map_world_DLC).sessionStarted), 0, 1)))
		If PlayerData.Data.CurrentMap = Scenes.scene_map_world_1 Then
			MyBase.GetComponent(Of SpriteRenderer)().sortingOrder = 1000
		End If
		PlayerData.Data.hasUnlockedBoatman = True
		PlayerData.SaveCurrentFile()
	End Sub

	' Token: 0x0600378D RID: 14221 RVA: 0x001FEC58 File Offset: 0x001FD058
	Private Sub OnDestroy()
		Me.RemoveDialoguerEvents()
	End Sub

	' Token: 0x0600378E RID: 14222 RVA: 0x001FEC60 File Offset: 0x001FD060
	Public Sub AddDialoguerEvents()
		AddHandler Dialoguer.events.onMessageEvent, AddressOf Me.OnDialoguerMessageEvent
		AddHandler Dialoguer.events.onStarted, AddressOf Me.OnDialoguerStart
		AddHandler Dialoguer.events.onEnded, AddressOf Me.OnDialoguerEnd
	End Sub

	' Token: 0x0600378F RID: 14223 RVA: 0x001FECB0 File Offset: 0x001FD0B0
	Public Sub RemoveDialoguerEvents()
		RemoveHandler Dialoguer.events.onMessageEvent, AddressOf Me.OnDialoguerMessageEvent
		RemoveHandler Dialoguer.events.onStarted, AddressOf Me.OnDialoguerStart
		RemoveHandler Dialoguer.events.onEnded, AddressOf Me.OnDialoguerEnd
	End Sub

	' Token: 0x06003790 RID: 14224 RVA: 0x001FED00 File Offset: 0x001FD100
	Private Sub SetOptions()
		Dim instance As SpeechBubble = SpeechBubble.Instance
		Dim currentMap As Scenes = PlayerData.Data.CurrentMap
		Select Case currentMap
			Case Scenes.scene_map_world_1
				instance.HideOptionByIndex(0)
			Case Scenes.scene_map_world_2
				instance.HideOptionByIndex(1)
			Case Scenes.scene_map_world_3
				instance.HideOptionByIndex(2)
			Case Else
				If currentMap = Scenes.scene_map_world_DLC Then
					instance.HideOptionByIndex(3)
				End If
		End Select
		If Not PlayerData.Data.GetMapData(Scenes.scene_map_world_2).sessionStarted Then
			instance.HideOptionByIndex(1)
		End If
		If Not PlayerData.Data.GetMapData(Scenes.scene_map_world_3).sessionStarted Then
			instance.HideOptionByIndex(2)
		End If
	End Sub

	' Token: 0x06003791 RID: 14225 RVA: 0x001FEDA8 File Offset: 0x001FD1A8
	Private Sub SelectWorld(metadata As String)
		If Me.selectionMade Then
			Return
		End If
		Dim num As Integer
		Parser.IntTryParse(metadata, num)
		If num > -1 Then
			MyBase.GetComponent(Of MapDialogueInteraction)().enabled = False
			Me.selectionMade = True
			AudioManager.Play("sfx_worldmap_boattravel_accept")
			If num = 3 Then
				If PlayerData.Data.GetMapData(Scenes.scene_map_world_DLC).sessionStarted Then
					SceneLoader.LoadScene(Scenes.scene_map_world_DLC, SceneLoader.Transition.Iris, SceneLoader.Transition.Iris, SceneLoader.Icon.Hourglass, Nothing)
					PlayerData.Data.GetMapData(Scenes.scene_map_world_DLC).enteringFrom = PlayerData.MapData.EntryMethod.Boatman
				Else
					PlayerData.Data.Gift(PlayerId.PlayerOne, Charm.charm_chalice)
					PlayerData.Data.Gift(PlayerId.PlayerTwo, Charm.charm_chalice)
					PlayerData.Data.shouldShowChaliceTooltip = True
					Cutscene.Load(Scenes.scene_level_kitchen, Scenes.scene_cutscene_dlc_intro, SceneLoader.Transition.Iris, SceneLoader.Transition.Iris, SceneLoader.Icon.Hourglass)
					PlayerData.Data.GetMapData(Scenes.scene_map_world_DLC).enteringFrom = PlayerData.MapData.EntryMethod.None
					PlayerData.Data.CurrentMap = Scenes.scene_map_world_DLC
				End If
			ElseIf num <> 0 Then
				If num <> 1 Then
					If num = 2 Then
						PlayerData.Data.GetMapData(Scenes.scene_map_world_3).enteringFrom = PlayerData.MapData.EntryMethod.Boatman
						SceneLoader.LoadScene(Scenes.scene_map_world_3, SceneLoader.Transition.Iris, SceneLoader.Transition.Iris, SceneLoader.Icon.Hourglass, Nothing)
					End If
				Else
					PlayerData.Data.GetMapData(Scenes.scene_map_world_2).enteringFrom = PlayerData.MapData.EntryMethod.Boatman
					SceneLoader.LoadScene(Scenes.scene_map_world_2, SceneLoader.Transition.Iris, SceneLoader.Transition.Iris, SceneLoader.Icon.Hourglass, Nothing)
				End If
			Else
				PlayerData.Data.GetMapData(Scenes.scene_map_world_1).enteringFrom = PlayerData.MapData.EntryMethod.Boatman
				SceneLoader.LoadScene(Scenes.scene_map_world_1, SceneLoader.Transition.Iris, SceneLoader.Transition.Iris, SceneLoader.Icon.Hourglass, Nothing)
			End If
		End If
	End Sub

	' Token: 0x06003792 RID: 14226 RVA: 0x001FEF00 File Offset: 0x001FD300
	Private Sub Update()
		Me.blinkTimer -= CupheadTime.Delta
		If Me.blinkTimer < 0F Then
			Me.blinkTimer = Me.blinkRange.RandomFloat()
			MyBase.animator.SetTrigger("Blink")
		End If
	End Sub

	' Token: 0x06003793 RID: 14227 RVA: 0x001FEF55 File Offset: 0x001FD355
	Private Sub OnDialoguerStart()
		MyBase.animator.SetBool("Talk", True)
	End Sub

	' Token: 0x06003794 RID: 14228 RVA: 0x001FEF68 File Offset: 0x001FD368
	Private Sub OnDialoguerEnd()
		MyBase.animator.SetBool("Talk", False)
	End Sub

	' Token: 0x06003795 RID: 14229 RVA: 0x001FEF7B File Offset: 0x001FD37B
	Private Sub OnDialoguerMessageEvent(message As String, metadata As String)
		If message = "BoatmanSetOptions" Then
			Me.SetOptions()
		End If
		If message = "BoatmanSelection" Then
			Me.SelectWorld(metadata)
		End If
	End Sub

	' Token: 0x04003F9C RID: 16284
	Private Const DIALOGUER_BOATMAN_STATE As Integer = 22

	' Token: 0x04003F9D RID: 16285
	Private Const W1 As Integer = 0

	' Token: 0x04003F9E RID: 16286
	Private Const W2 As Integer = 1

	' Token: 0x04003F9F RID: 16287
	Private Const W3 As Integer = 2

	' Token: 0x04003FA0 RID: 16288
	Private Const WDLC As Integer = 3

	' Token: 0x04003FA1 RID: 16289
	<SerializeField()>
	Private blinkRange As MinMax = New MinMax(2.5F, 4.5F)

	' Token: 0x04003FA2 RID: 16290
	Private blinkTimer As Single

	' Token: 0x04003FA3 RID: 16291
	Private selectionMade As Boolean
End Class
