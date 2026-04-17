Imports System
Imports System.Collections.Generic
Imports UnityEngine

' Token: 0x02000995 RID: 2453
Public Class MapEquipUIChecklist
	Inherits AbstractMapEquipUICardSide

	' Token: 0x0600395C RID: 14684 RVA: 0x00208E50 File Offset: 0x00207250
	Public Overrides Sub Init(playerID As PlayerId)
		MyBase.Init(playerID)
		Me.darkText = New Color(0.2F, 0.188F, 0.188F)
		Me.lightText = New Color(0.827F, 0.765F, 0.702F)
		Me.disabledText = New Color(0.537F, 0.498F, 0.463F)
		Me.selectableLength = Me.worldSelectionIcons.Length
		For i As Integer = 0 To Me.worldSelectionIcons.Length - 1
			Me.worldSelectionIcons(i).SetIcons("Icons/" + Me.worldPaths(i) + "_dark")
			Me.worldSelectionIcons(i).SetTextColor(Me.darkText)
		Next
		Me.worldSelectionIcons(Me.index).SetIcons("Icons/" + Me.worldPaths(Me.index) + "_light")
		Me.worldSelectionIcons(Me.index).SetTextColor(Me.lightText)
		If Not PlayerData.Data.CheckLevelsCompleted(Level.world1BossLevels) Then
			Me.worldSelectionIcons(Me.worldSelectionIcons.Length - 1).SetTextColor(Me.disabledText)
			Me.worldSelectionIcons(Me.worldSelectionIcons.Length - 2).SetTextColor(Me.disabledText)
			Me.worldSelectionIcons(Me.worldSelectionIcons.Length - 3).SetTextColor(Me.disabledText)
			Me.selectableLength -= 3
		ElseIf Not PlayerData.Data.CheckLevelsCompleted(Level.world2BossLevels) Then
			Me.worldSelectionIcons(Me.worldSelectionIcons.Length - 1).SetTextColor(Me.disabledText)
			Me.worldSelectionIcons(Me.worldSelectionIcons.Length - 2).SetTextColor(Me.disabledText)
			Me.selectableLength -= 2
		ElseIf Not PlayerData.Data.CheckLevelsCompleted(Level.world3BossLevels) Then
			Me.worldSelectionIcons(Me.worldSelectionIcons.Length - 1).SetTextColor(Me.disabledText)
			Me.selectableLength -= 1
		End If
		Me.UpdateList()
	End Sub

	' Token: 0x0600395D RID: 14685 RVA: 0x00209074 File Offset: 0x00207474
	Private Sub SetArrow(showRight As Boolean)
		Me.rightArrow.SetActive(showRight)
		Me.leftArrow.SetActive(Not showRight)
	End Sub

	' Token: 0x0600395E RID: 14686 RVA: 0x00209094 File Offset: 0x00207494
	Public Sub ChangeSelection(direction As Integer)
		Me.index = Mathf.Clamp(Me.index + direction, 0, Me.selectableLength - 1)
		Dim flag As Boolean = False
		Me.skippedOver = False
		If Me.showDLCMenu Then
			If Me.selectableLength < Me.worldSelectionIcons.Length Then
				flag = True
				If Me.DLCIndex = Me.worldNames.Length - 1 AndAlso direction < 0 Then
					Me.DLCIndex = Me.selectableLength - 1
					Me.skippedOver = True
					Me.SetArrow(True)
				ElseIf Me.DLCIndex + direction > Me.selectableLength - 1 Then
					Me.DLCIndex = Me.worldNames.Length - 1
					Me.skippedOver = True
					Me.SetArrow(False)
				ElseIf Me.DLCIndex + direction < 0 Then
					Me.DLCIndex = 0
					Me.SetArrow(True)
				Else
					Me.DLCIndex += direction
				End If
			ElseIf Me.DLCIndex + direction < 0 Then
				Me.DLCIndex = 0
				Me.SetArrow(True)
			ElseIf Me.DLCIndex + direction > Me.worldNames.Length - 1 Then
				Me.DLCIndex = Me.worldNames.Length - 1
				Me.SetArrow(False)
			Else
				Me.DLCIndex += direction
			End If
			Me.ChangeDLCMenu(Me.index, Me.lastIndex)
		End If
		If flag Then
			Dim num As Integer = If((Me.DLCIndex <> Me.worldNames.Length - 1), Me.index, (Me.worldSelectionIcons.Length - 1))
			Me.SetCursorPosition(num, False)
		Else
			Me.SetCursorPosition(Me.index, False)
		End If
	End Sub

	' Token: 0x0600395F RID: 14687 RVA: 0x00209250 File Offset: 0x00207650
	Public Sub SetCursorPosition(index As Integer, openingChecklist As Boolean)
		If openingChecklist Then
			Me.showDLCMenu = (DLCManager.DLCEnabled() AndAlso PlayerData.Data.GetMapData(Scenes.scene_map_world_DLC).sessionStarted) OrElse Me.editorShowDLCChecklist
			If index >= Me.worldNames.Length - 1 Then
				Me.DLCIndex = index
				index = Me.worldSelectionIcons.Length - 1
			Else
				Me.DLCIndex = index
			End If
		End If
		Me.index = index
		If Me.lastIndex <> index Then
			Me.worldSelectionIcons(index).SetIcons("Icons/" + Me.worldPaths(index) + "_light")
			Me.worldSelectionIcons(index).SetTextColor(New Color(0.827F, 0.765F, 0.702F))
			If Not Me.skippedOver Then
				Me.worldSelectionIcons(Me.lastIndex).SetIcons("Icons/" + Me.worldPaths(Me.lastIndex) + "_dark")
				Me.worldSelectionIcons(Me.lastIndex).SetTextColor(New Color(0.2F, 0.188F, 0.188F))
			End If
			AudioManager.Play("menu_equipment_move")
			Me.lastIndex = index
		End If
		If Me.showDLCMenu Then
			If openingChecklist Then
				Me.skippedOver = True
				If Me.DLCIndex < Me.worldNames.Length - 1 Then
					Me.SetArrow(True)
				End If
				Me.ChangeDLCMenu(index, Me.lastIndex)
			End If
			If Me.DLCIndex = 0 Then
				Me.SetArrow(True)
			ElseIf Me.DLCIndex = Me.worldNames.Length - 1 Then
				Me.SetArrow(False)
			End If
		End If
		Me.cursor.SetPosition(Me.worldSelectionIcons(index).transform.position)
		Me.UpdateList()
	End Sub

	' Token: 0x06003960 RID: 14688 RVA: 0x00209420 File Offset: 0x00207820
	Private Sub ChangeDLCMenu(index As Integer, lastIndex As Integer)
		If(index = lastIndex AndAlso (index <= 0 OrElse index >= Me.selectableLength - 1)) OrElse Me.skippedOver Then
			Dim num As Integer = If((Me.DLCIndex <> Me.worldNames.Length - 1), 0, 1)
			Dim num2 As Integer = 0
			Dim flag As Boolean = index >= Me.worldSelectionIcons.Length - 1
			For i As Integer = 0 To Me.worldSelectionIcons.Length - 1
				Dim translationElement As TranslationElement = Localization.Find(Me.worldNames(num).ToString())
				Me.worldSelectionIcons(num2).iconText.text = translationElement.translation.text
				Dim num3 As Integer = If((Me.DLCIndex <> Me.worldNames.Length - 1), 0, 1)
				If i > Me.selectableLength - 1 - num3 AndAlso (Me.DLCIndex <> Me.worldNames.Length - 1 OrElse i <> Me.worldSelectionIcons.Length - 1) Then
					Me.worldSelectionIcons(i).SetTextColor(Me.disabledText)
				End If
				num = (num + 1) Mod Me.worldNames.Length
				num2 = (num2 + 1) Mod Me.worldSelectionIcons.Length
			Next
		End If
	End Sub

	' Token: 0x06003961 RID: 14689 RVA: 0x0020955C File Offset: 0x0020795C
	Private Sub UpdateList()
		Dim list As List(Of Levels) = New List(Of Levels)()
		Dim list2 As List(Of String) = New List(Of String)()
		list.Clear()
		list2.Clear()
		For i As Integer = 0 To Me.checklistItems.Count - 1
			Me.checklistItems(i).gameObject.SetActive(False)
			Me.checklistItems(i).ClearDescription(Me.selectedFinale)
		Next
		For j As Integer = 0 To Me.finaleItems.Count - 1
			Me.finaleItems(j).gameObject.SetActive(False)
			If Me.finaleItems(j).checkMark IsNot Nothing Then
				Me.finaleItems(j).checkMark.enabled = False
				Me.finaleItems(j).ClearDescription(Me.selectedFinale)
			End If
		Next
		Dim flag As Boolean = False
		Select Case If((Not Me.showDLCMenu), Me.index, Me.DLCIndex)
			Case 0
				list.AddRange(Me.world1Levels)
				Me.selectedFinale = False
			Case 1
				list.AddRange(Me.world2Levels)
				Me.selectedFinale = False
			Case 2
				list.AddRange(Me.world3Levels)
				Me.selectedFinale = False
			Case 3
				list.AddRange(Me.finaleLevels)
				Me.selectedFinale = True
			Case 4
				list.AddRange(Me.DLClevels)
				Me.selectedFinale = False
				flag = True
		End Select
		For Each levels As Levels In list
			list2.Add(Level.GetLevelName(levels).Replace("\n", " "))
		Next
		Me.worldTop.SetActive(False)
		Me.finaleTop.SetActive(False)
		Me.localizedTop.SetActive(True)
		Me.worldTopLocalized.SetActive(Not Me.selectedFinale AndAlso Not flag)
		Me.finaleTopLocalized.SetActive(Me.selectedFinale)
		Me.worldTopDLCLocalized.SetActive(flag)
		Me.finaleGrid.SetActive(Me.selectedFinale)
		Dim played As Boolean = PlayerData.Data.GetLevelData(Levels.Saltbaker).played
		For k As Integer = 0 To list2.Count - 1
			If flag Then
				If k <> list2.Count - 1 OrElse (k = list2.Count - 1 AndAlso played) Then
					Me.checklistItems(k).gameObject.SetActive(True)
					Me.checklistItems(k).EnableCheckbox(k < list2.Count - 1)
					Me.checklistItems(k).SetDescription(list(k), list2(k), Me.selectedFinale)
				End If
			ElseIf Not Me.selectedFinale Then
				Me.checklistItems(k).gameObject.SetActive(True)
				Me.checklistItems(k).EnableCheckbox(k < list2.Count - 2)
				Me.checklistItems(k).SetDescription(list(k), list2(k), Me.selectedFinale)
			Else
				Me.finaleItems(k).gameObject.SetActive(True)
				Me.finaleItems(k).SetDescription(list(k), list2(k), Me.selectedFinale)
			End If
		Next
	End Sub

	' Token: 0x040040EA RID: 16618
	Private worldPaths As String() = New String() { "equip_checklist_world_1", "equip_checklist_world_2", "equip_checklist_world_3", "equip_checklist_finale" }

	' Token: 0x040040EB RID: 16619
	Private world1Levels As Levels() = New Levels() { Levels.Veggies, Levels.Slime, Levels.FlyingBlimp, Levels.Flower, Levels.Frogs, Levels.Platforming_Level_1_1, Levels.Platforming_Level_1_2 }

	' Token: 0x040040EC RID: 16620
	Private world2Levels As Levels() = New Levels() { Levels.Baroness, Levels.Clown, Levels.FlyingGenie, Levels.Dragon, Levels.FlyingBird, Levels.Platforming_Level_2_1, Levels.Platforming_Level_2_2 }

	' Token: 0x040040ED RID: 16621
	Private world3Levels As Levels() = New Levels() { Levels.Bee, Levels.Pirate, Levels.SallyStagePlay, Levels.Mouse, Levels.Robot, Levels.FlyingMermaid, Levels.Train, Levels.Platforming_Level_3_1, Levels.Platforming_Level_3_2 }

	' Token: 0x040040EE RID: 16622
	Private finaleLevels As Levels() = New Levels() { Levels.DicePalaceBooze, Levels.DicePalaceChips, Levels.DicePalaceCigar, Levels.DicePalaceDomino, Levels.DicePalaceEightBall, Levels.DicePalaceFlyingHorse, Levels.DicePalaceFlyingMemory, Levels.DicePalaceRabbit, Levels.DicePalaceRoulette, Levels.DicePalaceMain, Levels.Devil }

	' Token: 0x040040EF RID: 16623
	Private DLClevels As Levels() = New Levels() { Levels.OldMan, Levels.RumRunners, Levels.Airplane, Levels.SnowCult, Levels.FlyingCowboy, Levels.Saltbaker }

	' Token: 0x040040F0 RID: 16624
	Private worldNames As String() = New String() { "CheckListWorld1", "CheckListWorld2", "CheckListWorld3", "CheckListFinale", "ChecklistDLC" }

	' Token: 0x040040F1 RID: 16625
	<Header("Headers")>
	<SerializeField()>
	Private worldTop As GameObject

	' Token: 0x040040F2 RID: 16626
	<SerializeField()>
	Private finaleTop As GameObject

	' Token: 0x040040F3 RID: 16627
	<SerializeField()>
	Private localizedTop As GameObject

	' Token: 0x040040F4 RID: 16628
	<SerializeField()>
	Private worldTopLocalized As GameObject

	' Token: 0x040040F5 RID: 16629
	<SerializeField()>
	Private worldTopDLCLocalized As GameObject

	' Token: 0x040040F6 RID: 16630
	<SerializeField()>
	Private finaleTopLocalized As GameObject

	' Token: 0x040040F7 RID: 16631
	<Header("Cursors")>
	<SerializeField()>
	Private cursor As MapEquipUICursor

	' Token: 0x040040F8 RID: 16632
	<Header("Icons")>
	<SerializeField()>
	Private worldSelectionIcons As MapEquipUICardChecklistIcon()

	' Token: 0x040040F9 RID: 16633
	<Header("Bosses + Platforming items")>
	<SerializeField()>
	Private checklistItems As List(Of MapEquipUIChecklistItem)

	' Token: 0x040040FA RID: 16634
	<SerializeField()>
	Private finaleItems As List(Of MapEquipUIChecklistItem)

	' Token: 0x040040FB RID: 16635
	<SerializeField()>
	Private finaleGrid As GameObject

	' Token: 0x040040FC RID: 16636
	<SerializeField()>
	Private rightArrow As GameObject

	' Token: 0x040040FD RID: 16637
	<SerializeField()>
	Private leftArrow As GameObject

	' Token: 0x040040FE RID: 16638
	Private index As Integer

	' Token: 0x040040FF RID: 16639
	Private lastIndex As Integer

	' Token: 0x04004100 RID: 16640
	Private DLCIndex As Integer

	' Token: 0x04004101 RID: 16641
	Private selectedFinale As Boolean

	' Token: 0x04004102 RID: 16642
	Private showDLCMenu As Boolean

	' Token: 0x04004103 RID: 16643
	Private skippedOver As Boolean

	' Token: 0x04004104 RID: 16644
	Private editorShowDLCChecklist As Boolean

	' Token: 0x04004105 RID: 16645
	Private darkText As Color

	' Token: 0x04004106 RID: 16646
	Private lightText As Color

	' Token: 0x04004107 RID: 16647
	Private disabledText As Color

	' Token: 0x04004108 RID: 16648
	Private selectableLength As Integer
End Class
