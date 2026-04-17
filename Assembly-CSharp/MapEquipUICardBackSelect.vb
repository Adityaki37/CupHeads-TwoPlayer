Imports System
Imports System.Collections
Imports TMPro
Imports UnityEngine
Imports UnityEngine.UI

' Token: 0x0200098F RID: 2447
Public Class MapEquipUICardBackSelect
	Inherits AbstractMapEquipUICardSide

	' Token: 0x06003937 RID: 14647 RVA: 0x00206DB6 File Offset: 0x002051B6
	Public Sub ChangeSelection(direction As Trilean2)
		Me.index = Me.selectedIcons(Me.index).GetIndexOfNeighbor(direction)
		Me.SetCursorPosition(Me.index)
		Me.UpdateText()
	End Sub

	' Token: 0x06003938 RID: 14648 RVA: 0x00206DE4 File Offset: 0x002051E4
	Public Sub ChangeSlot(direction As Integer)
		Me.cursor.Show()
		Dim num As Integer = CInt(Me.slot)
		Me.slot = CType(Mathf.Repeat(CSng((num + direction)), CSng(EnumUtils.GetCount(Of MapEquipUICard.Slot)())), MapEquipUICard.Slot)
		Me.Setup(Me.slot)
	End Sub

	' Token: 0x06003939 RID: 14649 RVA: 0x00206E28 File Offset: 0x00205228
	Private Sub UpdateText()
		Dim flag As Boolean = False
		Select Case Me.slot
			Case MapEquipUICard.Slot.SHOT_A, MapEquipUICard.Slot.SHOT_B
				flag = PlayerData.Data.IsUnlocked(MyBase.playerID, MapEquipUICardBackSelect.WEAPONS(Me.index))
				Me.titleText.text = If((Not flag), Localization.Translate("EquipItemLocked").text, WeaponProperties.GetDisplayName(MapEquipUICardBackSelect.WEAPONS(Me.index)).ToUpper())
				Me.exText.text = If((Not flag), "? ? ? ? ? ? ? ? ?", WeaponProperties.GetSubtext(MapEquipUICardBackSelect.WEAPONS(Me.index)))
				Me.descriptionText.text = If((Not flag), "? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ?", WeaponProperties.GetDescription(MapEquipUICardBackSelect.WEAPONS(Me.index)))
			Case MapEquipUICard.Slot.SUPER
				flag = PlayerData.Data.IsUnlocked(MyBase.playerID, MapEquipUICardBackSelect.SUPERS(Me.index))
				Dim playerLoadout As PlayerData.PlayerLoadouts.PlayerLoadout = PlayerData.Data.Loadouts.GetPlayerLoadout(MyBase.playerID)
				Dim super As Super = If((playerLoadout.charm <> Charm.charm_chalice), MapEquipUICardBackSelect.SUPERS(Me.index), MapEquipUICardBackSelect.CHALICESUPERS(Me.index))
				Me.titleText.text = If((Not flag), Localization.Translate("EquipItemLocked").text, WeaponProperties.GetDisplayName(MapEquipUICardBackSelect.SUPERS(Me.index)).ToUpper())
				Me.exText.text = If((Not flag), "? ? ? ? ? ? ? ? ?", WeaponProperties.GetSubtext(super))
				Me.descriptionText.text = If((Not flag), "? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ?", WeaponProperties.GetDescription(super))
			Case MapEquipUICard.Slot.CHARM
				flag = PlayerData.Data.IsUnlocked(MyBase.playerID, MapEquipUICardBackSelect.CHARMS(Me.index))
				If MapEquipUICardBackSelect.CHARMS(Me.index) = Charm.charm_curse AndAlso CharmCurse.IsMaxLevel(MyBase.playerID) Then
					Me.titleText.text = Localization.Translate("charm_paladin_name").text
					Me.exText.text = Localization.Translate("charm_paladin_subtext").text
					Me.descriptionText.text = Localization.Translate("charm_paladin_description").text
				ElseIf flag AndAlso MapEquipUICardBackSelect.CHARMS(Me.index) = Charm.charm_curse AndAlso (CharmCurse.CalculateLevel(PlayerId.PlayerOne) > -1 OrElse CharmCurse.CalculateLevel(PlayerId.PlayerTwo) > -1) Then
					Me.titleText.text = Localization.Translate("charm_curse_name").text
					Me.exText.text = Localization.Translate("charm_curse_subtext").text
					Me.descriptionText.text = Localization.Translate("charm_curse_description").text
				ElseIf flag AndAlso MapEquipUICardBackSelect.CHARMS(Me.index) = Charm.charm_curse Then
					Me.titleText.text = Localization.Translate("charm_broken_name").text
					Me.exText.text = Localization.Translate("charm_broken_subtext").text
					Me.descriptionText.text = Localization.Translate("charm_broken_description").text
				Else
					Me.titleText.text = If((Not flag), Localization.Translate("EquipItemLocked").text, WeaponProperties.GetDisplayName(MapEquipUICardBackSelect.CHARMS(Me.index)).ToUpper())
					Me.exText.text = If((Not flag), "? ? ? ? ? ? ? ? ?", WeaponProperties.GetSubtext(MapEquipUICardBackSelect.CHARMS(Me.index)))
					Me.descriptionText.text = If((Not flag), "? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ?", WeaponProperties.GetDescription(MapEquipUICardBackSelect.CHARMS(Me.index)))
				End If
		End Select
		Me.titleText.font = Localization.Instance.fonts(CInt(Localization.language))(9).font
		If flag Then
			Me.exText.font = Localization.Instance.fonts(CInt(Localization.language))(10).font
			Me.descriptionText.font = Localization.Instance.fonts(CInt(Localization.language))(11).fontAsset
		Else
			Me.exText.font = Localization.Instance.fonts(CInt(Localization.language1))(10).font
			Me.descriptionText.font = Localization.Instance.fonts(CInt(Localization.language1))(11).fontAsset
		End If
	End Sub

	' Token: 0x0600393A RID: 14650 RVA: 0x00207330 File Offset: 0x00205730
	Private Sub SetCursorPosition(index As Integer)
		If Me.lastIndex <> index Then
			AudioManager.Play("menu_equipment_move")
			Me.lastIndex = index
		End If
		Me.cursor.SetPosition(Me.selectedIcons(index).transform.position)
		If Not Me.noneUnlocked AndAlso Me.itemSelected Then
			Me.selectionCursor.Show()
		Else
			Me.selectionCursor.Hide()
		End If
	End Sub

	' Token: 0x0600393B RID: 14651 RVA: 0x002073A8 File Offset: 0x002057A8
	Public Sub Setup(slot As MapEquipUICard.Slot)
		Me.slot = slot
		Me.headerText.ApplyTranslation(Localization.Find(MapEquipUICardBackSelect.slotLocalesKey(CInt(slot))), Nothing)
		Dim flag As Boolean = slot = MapEquipUICard.Slot.SUPER
		Dim flag2 As Boolean = DLCManager.DLCEnabled()
		Me.selectedIcons = If((Not flag), If((Not flag2), Me.normalIcons, Me.DLCIcons), Me.superIcons)
		For Each mapEquipUICardBackSelectIcon As MapEquipUICardBackSelectIcon In Me.superIcons
			mapEquipUICardBackSelectIcon.gameObject.SetActive(flag)
		Next
		For Each mapEquipUICardBackSelectIcon2 As MapEquipUICardBackSelectIcon In Me.normalIcons
			mapEquipUICardBackSelectIcon2.gameObject.SetActive(Not flag AndAlso Not flag2)
		Next
		For Each mapEquipUICardBackSelectIcon3 As MapEquipUICardBackSelectIcon In Me.DLCIcons
			mapEquipUICardBackSelectIcon3.gameObject.SetActive(Not flag AndAlso flag2)
		Next
		Me.superIconsBack.enabled = flag
		Me.iconsBack.enabled = Not flag AndAlso Not flag2
		Me.DLCIconsBack.enabled = Not flag AndAlso flag2
		Dim playerLoadout As PlayerData.PlayerLoadouts.PlayerLoadout = PlayerData.Data.Loadouts.GetPlayerLoadout(MyBase.playerID)
		Me.selectionCursor.Hide()
		Me.noneUnlocked = True
		Me.index = -1
		Dim flag3 As Boolean = False
		Me.itemSelected = False
		For l As Integer = 0 To Me.selectedIcons.Length - 1
			Me.selectedIcons(l).Index = l
			Dim text As String = "_000" + (l Mod 8 + 1).ToStringInvariant()
			Dim text2 As String = Weapon.None.ToString()
			Select Case slot
				Case MapEquipUICard.Slot.SHOT_A
					If PlayerData.Data.IsUnlocked(MyBase.playerID, MapEquipUICardBackSelect.WEAPONS(l)) Then
						flag3 = MapEquipUICardBackSelect.WEAPONS(l) = playerLoadout.secondaryWeapon
						Me.noneUnlocked = False
						Me.selectedIcons(l).SetIcons(MapEquipUICardBackSelect.WEAPONS(l), flag3)
					Else
						text2 = WeaponProperties.GetIconPath(Weapon.None) + text
						Me.selectedIcons(l).SetIconsManual(text2, flag3, False)
					End If
					If MapEquipUICardBackSelect.WEAPONS(l) = playerLoadout.primaryWeapon AndAlso playerLoadout.primaryWeapon <> Weapon.None Then
						Me.index = l
						Me.itemSelected = True
					End If
				Case MapEquipUICard.Slot.SHOT_B
					If PlayerData.Data.IsUnlocked(MyBase.playerID, MapEquipUICardBackSelect.WEAPONS(l)) Then
						flag3 = MapEquipUICardBackSelect.WEAPONS(l) = playerLoadout.primaryWeapon
						Me.noneUnlocked = False
						Me.selectedIcons(l).SetIcons(MapEquipUICardBackSelect.WEAPONS(l), flag3)
					Else
						text2 = WeaponProperties.GetIconPath(Weapon.None) + text
						Me.selectedIcons(l).SetIconsManual(text2, flag3, False)
					End If
					If MapEquipUICardBackSelect.WEAPONS(l) = playerLoadout.secondaryWeapon AndAlso playerLoadout.secondaryWeapon <> Weapon.None Then
						Me.index = l
						Me.itemSelected = True
					End If
				Case MapEquipUICard.Slot.SUPER
					If PlayerData.Data.IsUnlocked(MyBase.playerID, MapEquipUICardBackSelect.SUPERS(l)) Then
						Me.noneUnlocked = False
						Me.selectedIcons(l).SetIcons(MapEquipUICardBackSelect.SUPERS(l), flag3)
					Else
						text2 = WeaponProperties.GetIconPath(Super.None) + text
						Me.selectedIcons(l).SetIconsManual(text2, flag3, False)
					End If
					If playerLoadout.super <> Super.None AndAlso (MapEquipUICardBackSelect.SUPERS(l) = playerLoadout.super OrElse (playerLoadout.charm = Charm.charm_chalice AndAlso MapEquipUICardBackSelect.CHALICESUPERS(l) = playerLoadout.super)) Then
						Me.index = l
						Me.itemSelected = True
					End If
				Case MapEquipUICard.Slot.CHARM
					If PlayerData.Data.IsUnlocked(MyBase.playerID, MapEquipUICardBackSelect.CHARMS(l)) Then
						Me.noneUnlocked = False
						If MapEquipUICardBackSelect.CHARMS(l) = Charm.charm_curse Then
							Me.selectedIcons(l).SetIconsManual("Icons/equip_icon_charm_curse_" + (CharmCurse.CalculateLevel(MyBase.playerID) + 1).ToString(), False, True)
						Else
							Me.selectedIcons(l).SetIcons(MapEquipUICardBackSelect.CHARMS(l), flag3)
						End If
					Else
						text2 = WeaponProperties.GetIconPath(Charm.None) + text
						Me.selectedIcons(l).SetIconsManual(text2, flag3, False)
					End If
					If MapEquipUICardBackSelect.CHARMS(l) = playerLoadout.charm AndAlso playerLoadout.charm <> Charm.None Then
						Me.index = l
						Me.itemSelected = True
					End If
			End Select
			If Me.index = -1 Then
				Me.index = 0
			End If
			Me.cursor.SetPosition(Me.selectedIcons(Me.index).transform.position)
			Me.UpdateText()
		Next
		Me.selectionCursor.selectedIndex = -1
		If Not Me.noneUnlocked AndAlso Me.itemSelected Then
			If slot <> Me.lastSlot Then
				Me.selectionCursor.Show()
				Me.selectionCursor.selectedIndex = Me.index
				Me.selectionCursor.SetPosition(Me.selectedIcons(Me.index).transform.position)
			Else
				MyBase.StartCoroutine(Me.set_selection_cursor())
			End If
			Me.cursor.SelectIcon(True)
		End If
		Me.lastSlot = slot
	End Sub

	' Token: 0x0600393C RID: 14652 RVA: 0x002079AC File Offset: 0x00205DAC
	Private Iterator Function set_selection_cursor() As IEnumerator
		MyBase.StartCoroutine(Me.lock_input_cr())
		While Not Me.selectionCursor.animator.GetCurrentAnimatorStateInfo(0).IsName("Off") AndAlso Me.lockInput
			Yield Nothing
		End While
		Me.selectionCursor.Show()
		Me.selectionCursor.selectedIndex = Me.index
		Me.selectionCursor.SetPosition(Me.selectedIcons(Me.index).transform.position)
		Yield Nothing
		Return
	End Function

	' Token: 0x0600393D RID: 14653 RVA: 0x002079C8 File Offset: 0x00205DC8
	Private Iterator Function lock_input_cr() As IEnumerator
		Me.lockInput = True
		Yield New WaitForSeconds(0.2F)
		Me.lockInput = False
		Return
	End Function

	' Token: 0x0600393E RID: 14654 RVA: 0x002079E4 File Offset: 0x00205DE4
	Public Sub Accept()
		AudioManager.Play("menu_equipment_equip")
		Dim playerLoadout As PlayerData.PlayerLoadouts.PlayerLoadout = PlayerData.Data.Loadouts.GetPlayerLoadout(MyBase.playerID)
		Select Case Me.slot
			Case MapEquipUICard.Slot.SHOT_A
				If PlayerData.Data.IsUnlocked(MyBase.playerID, MapEquipUICardBackSelect.WEAPONS(Me.index)) Then
					Me.Selection()
				End If
			Case MapEquipUICard.Slot.SHOT_B
				If PlayerData.Data.IsUnlocked(MyBase.playerID, MapEquipUICardBackSelect.WEAPONS(Me.index)) AndAlso (playerLoadout.primaryWeapon <> MapEquipUICardBackSelect.WEAPONS(Me.index) OrElse playerLoadout.secondaryWeapon <> Weapon.None) Then
					Me.Selection()
				End If
			Case MapEquipUICard.Slot.SUPER
				If PlayerData.Data.IsUnlocked(MyBase.playerID, MapEquipUICardBackSelect.SUPERS(Me.index)) Then
					Me.Selection()
				End If
			Case MapEquipUICard.Slot.CHARM
				If PlayerData.Data.IsUnlocked(MyBase.playerID, MapEquipUICardBackSelect.CHARMS(Me.index)) Then
					Me.Selection()
				End If
		End Select
		Select Case Me.slot
			Case MapEquipUICard.Slot.SHOT_A
				If MapEquipUICardBackSelect.WEAPONS(Me.index) = Weapon.None OrElse Not PlayerData.Data.IsUnlocked(MyBase.playerID, MapEquipUICardBackSelect.WEAPONS(Me.index)) Then
					Me.OnLocked()
					Return
				End If
				If PlayerData.Data.Loadouts.GetPlayerLoadout(MyBase.playerID).secondaryWeapon = MapEquipUICardBackSelect.WEAPONS(Me.index) Then
					PlayerData.Data.Loadouts.GetPlayerLoadout(MyBase.playerID).secondaryWeapon = PlayerData.Data.Loadouts.GetPlayerLoadout(MyBase.playerID).primaryWeapon
				End If
				PlayerData.Data.Loadouts.GetPlayerLoadout(MyBase.playerID).primaryWeapon = MapEquipUICardBackSelect.WEAPONS(Me.index)
			Case MapEquipUICard.Slot.SHOT_B
				If MapEquipUICardBackSelect.WEAPONS(Me.index) = Weapon.None OrElse Not PlayerData.Data.IsUnlocked(MyBase.playerID, MapEquipUICardBackSelect.WEAPONS(Me.index)) OrElse (playerLoadout.primaryWeapon = MapEquipUICardBackSelect.WEAPONS(Me.index) AndAlso playerLoadout.secondaryWeapon = Weapon.None) Then
					Me.OnLocked()
					Return
				End If
				If PlayerData.Data.Loadouts.GetPlayerLoadout(MyBase.playerID).primaryWeapon = MapEquipUICardBackSelect.WEAPONS(Me.index) Then
					PlayerData.Data.Loadouts.GetPlayerLoadout(MyBase.playerID).primaryWeapon = PlayerData.Data.Loadouts.GetPlayerLoadout(MyBase.playerID).secondaryWeapon
				End If
				PlayerData.Data.Loadouts.GetPlayerLoadout(MyBase.playerID).secondaryWeapon = MapEquipUICardBackSelect.WEAPONS(Me.index)
				If Not PlayerData.Data.Loadouts.GetPlayerLoadout(MyBase.playerID).HasEquippedSecondaryRegularWeapon Then
					PlayerData.Data.Loadouts.GetPlayerLoadout(MyBase.playerID).MustNotifySwitchRegularWeapon = True
				End If
				PlayerData.Data.Loadouts.GetPlayerLoadout(MyBase.playerID).HasEquippedSecondaryRegularWeapon = True
			Case MapEquipUICard.Slot.SUPER
				If MapEquipUICardBackSelect.SUPERS(Me.index) = Super.None OrElse Not PlayerData.Data.IsUnlocked(MyBase.playerID, MapEquipUICardBackSelect.SUPERS(Me.index)) Then
					Me.OnLocked()
					Return
				End If
				PlayerData.Data.Loadouts.GetPlayerLoadout(MyBase.playerID).super = MapEquipUICardBackSelect.SUPERS(Me.index)
			Case MapEquipUICard.Slot.CHARM
				If MapEquipUICardBackSelect.CHARMS(Me.index) = Charm.None OrElse Not PlayerData.Data.IsUnlocked(MyBase.playerID, MapEquipUICardBackSelect.CHARMS(Me.index)) Then
					Me.OnLocked()
					Return
				End If
				If MapEquipUICardBackSelect.CHARMS(Me.index) <> Charm.charm_chalice AndAlso PlayerData.Data.Loadouts.GetPlayerLoadout(MyBase.playerID).charm = Charm.charm_chalice Then
					PlayerManager.OnChaliceCharmUnequipped(MyBase.playerID)
				End If
				PlayerData.Data.Loadouts.GetPlayerLoadout(MyBase.playerID).charm = MapEquipUICardBackSelect.CHARMS(Me.index)
		End Select
		Me.Setup(Me.slot)
	End Sub

	' Token: 0x0600393F RID: 14655 RVA: 0x00207E60 File Offset: 0x00206260
	Public Sub Unequip()
		AudioManager.Play("menu_equipment_equip")
		Select Case Me.slot
			Case MapEquipUICard.Slot.SHOT_A
				Me.OnLocked()
			Case MapEquipUICard.Slot.SHOT_B
				If PlayerData.Data.IsUnlocked(MyBase.playerID, MapEquipUICardBackSelect.WEAPONS(Me.index)) AndAlso PlayerData.Data.Loadouts.GetPlayerLoadout(MyBase.playerID).secondaryWeapon <> Weapon.None Then
					Me.Deselect()
					PlayerData.Data.Loadouts.GetPlayerLoadout(MyBase.playerID).secondaryWeapon = Weapon.None
					If PlayerData.Data.Loadouts.GetPlayerLoadout(MyBase.playerID).MustNotifySwitchRegularWeapon Then
						PlayerData.Data.Loadouts.GetPlayerLoadout(MyBase.playerID).HasEquippedSecondaryRegularWeapon = False
						PlayerData.Data.Loadouts.GetPlayerLoadout(MyBase.playerID).MustNotifySwitchRegularWeapon = False
					End If
				Else
					Me.OnLocked()
				End If
			Case MapEquipUICard.Slot.SUPER
				If PlayerData.Data.IsUnlocked(MyBase.playerID, MapEquipUICardBackSelect.SUPERS(Me.index)) AndAlso PlayerData.Data.Loadouts.GetPlayerLoadout(MyBase.playerID).super <> Super.None Then
					Me.Deselect()
					PlayerData.Data.Loadouts.GetPlayerLoadout(MyBase.playerID).super = Super.None
				Else
					Me.OnLocked()
				End If
			Case MapEquipUICard.Slot.CHARM
				If PlayerData.Data.IsUnlocked(MyBase.playerID, MapEquipUICardBackSelect.CHARMS(Me.index)) AndAlso PlayerData.Data.Loadouts.GetPlayerLoadout(MyBase.playerID).charm <> Charm.None Then
					If PlayerData.Data.Loadouts.GetPlayerLoadout(MyBase.playerID).charm = Charm.charm_chalice Then
						PlayerManager.OnChaliceCharmUnequipped(MyBase.playerID)
					End If
					Me.Deselect()
					PlayerData.Data.Loadouts.GetPlayerLoadout(MyBase.playerID).charm = Charm.None
				Else
					Me.OnLocked()
				End If
		End Select
	End Sub

	' Token: 0x06003940 RID: 14656 RVA: 0x00208098 File Offset: 0x00206498
	Private Sub Selection()
		Me.selectedIcons(Me.index).SelectIcon()
		If Me.selectionCursor.selectedIndex >= 0 Then
			Me.selectedIcons(Me.selectionCursor.selectedIndex).UnselectIcon()
		End If
		If Me.cursor.transform.position <> Me.selectionCursor.transform.position Then
			Me.selectionCursor.selectedIndex = Me.index
			Me.selectionCursor.[Select]()
			Me.cursor.SelectIcon(False)
		Else
			Me.cursor.SelectIcon(True)
		End If
	End Sub

	' Token: 0x06003941 RID: 14657 RVA: 0x00208142 File Offset: 0x00206542
	Private Sub Deselect()
		MyBase.StartCoroutine(Me.remove_selection_cr())
		Me.itemSelected = False
	End Sub

	' Token: 0x06003942 RID: 14658 RVA: 0x00208158 File Offset: 0x00206558
	Private Iterator Function remove_selection_cr() As IEnumerator
		Me.selectionCursor.[Select]()
		Yield Me.selectionCursor.animator.WaitForAnimationToEnd(Me, "Select", False, True)
		Me.selectionCursor.Hide()
		Yield Nothing
		Return
	End Function

	' Token: 0x06003943 RID: 14659 RVA: 0x00208173 File Offset: 0x00206573
	Private Sub OnLocked()
		AudioManager.Play("menu_locked")
		Me.selectedIcons(Me.index).OnLocked()
		Me.cursor.OnLocked()
	End Sub

	' Token: 0x040040BE RID: 16574
	Private Shared WEAPONS As Weapon() = New Weapon() { Weapon.level_weapon_peashot, Weapon.level_weapon_spreadshot, Weapon.level_weapon_homing, Weapon.level_weapon_bouncer, Weapon.level_weapon_charge, Weapon.level_weapon_boomerang, Weapon.level_weapon_crackshot, Weapon.level_weapon_wide_shot, Weapon.level_weapon_upshot }

	' Token: 0x040040BF RID: 16575
	Private Shared SUPERS As Super() = New Super() { Super.level_super_beam, Super.level_super_invincible, Super.level_super_ghost }

	' Token: 0x040040C0 RID: 16576
	Private Shared CHALICESUPERS As Super() = New Super() { Super.level_super_chalice_vert_beam, Super.level_super_chalice_shield, Super.level_super_chalice_iii }

	' Token: 0x040040C1 RID: 16577
	Private Shared CHARMS As Charm() = New Charm() { Charm.charm_health_up_1, Charm.charm_super_builder, Charm.charm_smoke_dash, Charm.charm_parry_plus, Charm.charm_health_up_2, Charm.charm_parry_attack, Charm.charm_chalice, Charm.charm_curse, Charm.charm_healer }

	' Token: 0x040040C2 RID: 16578
	Private Shared slotLocalesKey As String() = New String() { "ShotABackTitle", "ShotBBackTitle", "SuperBackTitle", "CharmBackTitle" }

	' Token: 0x040040C3 RID: 16579
	Public lockInput As Boolean

	' Token: 0x040040C4 RID: 16580
	<Header("Text")>
	<SerializeField()>
	Private headerText As LocalizationHelper

	' Token: 0x040040C5 RID: 16581
	<SerializeField()>
	Private titleText As Text

	' Token: 0x040040C6 RID: 16582
	<SerializeField()>
	Private exText As Text

	' Token: 0x040040C7 RID: 16583
	<SerializeField()>
	Private descriptionText As TMP_Text

	' Token: 0x040040C8 RID: 16584
	<Header("Cursors")>
	<SerializeField()>
	Private cursor As MapEquipUICursor

	' Token: 0x040040C9 RID: 16585
	<SerializeField()>
	Private selectionCursor As MapEquipUICardBackSelectSelectionCursor

	' Token: 0x040040CA RID: 16586
	<Header("Backs")>
	<SerializeField()>
	Private iconsBack As Image

	' Token: 0x040040CB RID: 16587
	<SerializeField()>
	Private superIconsBack As Image

	' Token: 0x040040CC RID: 16588
	<SerializeField()>
	Private DLCIconsBack As Image

	' Token: 0x040040CD RID: 16589
	<Header("Icons")>
	<SerializeField()>
	Private normalIcons As MapEquipUICardBackSelectIcon()

	' Token: 0x040040CE RID: 16590
	<Header("Super Icons")>
	<SerializeField()>
	Private superIcons As MapEquipUICardBackSelectIcon()

	' Token: 0x040040CF RID: 16591
	<Header("DLC Icons")>
	<SerializeField()>
	Private DLCIcons As MapEquipUICardBackSelectIcon()

	' Token: 0x040040D0 RID: 16592
	Private index As Integer

	' Token: 0x040040D1 RID: 16593
	Private lastIndex As Integer

	' Token: 0x040040D2 RID: 16594
	Private slot As MapEquipUICard.Slot

	' Token: 0x040040D3 RID: 16595
	Private lastSlot As MapEquipUICard.Slot

	' Token: 0x040040D4 RID: 16596
	Private selectedIcons As MapEquipUICardBackSelectIcon()

	' Token: 0x040040D5 RID: 16597
	Private noneUnlocked As Boolean

	' Token: 0x040040D6 RID: 16598
	Private itemSelected As Boolean
End Class
