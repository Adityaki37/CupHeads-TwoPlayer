Imports System
Imports UnityEngine
Imports UnityEngine.UI

' Token: 0x02000993 RID: 2451
Public Class MapEquipUICardFront
	Inherits AbstractMapEquipUICardSide

	' Token: 0x170004A7 RID: 1191
	' (get) Token: 0x06003950 RID: 14672 RVA: 0x002086A6 File Offset: 0x00206AA6
	Public ReadOnly Property Slot As MapEquipUICard.Slot
		Get
			Return CType(Me.index, MapEquipUICard.Slot)
		End Get
	End Property

	' Token: 0x06003951 RID: 14673 RVA: 0x002086AE File Offset: 0x00206AAE
	Private Sub Update()
		Me.SetCursorPosition(Me.index)
	End Sub

	' Token: 0x06003952 RID: 14674 RVA: 0x002086BC File Offset: 0x00206ABC
	Private Sub Start()
		AddHandler Localization.OnLanguageChangedEvent, AddressOf Me.OnLanguageChanged
	End Sub

	' Token: 0x06003953 RID: 14675 RVA: 0x002086CF File Offset: 0x00206ACF
	Private Sub OnDestroy()
		RemoveHandler Localization.OnLanguageChangedEvent, AddressOf Me.OnLanguageChanged
	End Sub

	' Token: 0x06003954 RID: 14676 RVA: 0x002086E2 File Offset: 0x00206AE2
	Private Sub OnLanguageChanged()
		Me.ChangeSelection(0)
	End Sub

	' Token: 0x06003955 RID: 14677 RVA: 0x002086EC File Offset: 0x00206AEC
	Public Overrides Sub Init(playerID As PlayerId)
		MyBase.Init(playerID)
		Me.icons = New MapEquipUICardFrontIcon() { Me.weaponA, Me.weaponB, Me.super, Me.item, Me.checklist }
		Me.checklist.SetIconsManual("Icons/equip_icon_list", False, False)
		Me.checkListSelected = False
		Me.Refresh()
		Me.ChangeSelection(0)
	End Sub

	' Token: 0x06003956 RID: 14678 RVA: 0x00208760 File Offset: 0x00206B60
	Public Sub Refresh()
		Me.loadout = PlayerData.Data.Loadouts.GetPlayerLoadout(MyBase.playerID)
		Me.weaponA.SetIcons(Me.loadout.primaryWeapon, False)
		Me.weaponB.SetIcons(Me.loadout.secondaryWeapon, False)
		Me.super.SetIcons(Me.loadout.super, False)
		If Me.loadout.charm = Charm.charm_curse Then
			Me.item.SetIconsManual("Icons/equip_icon_charm_curse_" + (CharmCurse.CalculateLevel(MyBase.playerID) + 1).ToString(), False, True)
		Else
			Me.item.SetIcons(Me.loadout.charm, False)
		End If
	End Sub

	' Token: 0x06003957 RID: 14679 RVA: 0x00208830 File Offset: 0x00206C30
	Public Sub Unequip()
		If Me.icons(Me.index) IsNot Me.weaponA Then
			Me.icons(Me.index).SetIcons(WeaponProperties.GetIconPath(Weapon.None))
			If Me.icons(Me.index) Is Me.weaponB Then
				PlayerData.Data.Loadouts.GetPlayerLoadout(MyBase.playerID).secondaryWeapon = Weapon.None
				If PlayerData.Data.Loadouts.GetPlayerLoadout(MyBase.playerID).MustNotifySwitchRegularWeapon Then
					PlayerData.Data.Loadouts.GetPlayerLoadout(MyBase.playerID).HasEquippedSecondaryRegularWeapon = False
					PlayerData.Data.Loadouts.GetPlayerLoadout(MyBase.playerID).MustNotifySwitchRegularWeapon = False
				End If
			ElseIf Me.icons(Me.index) Is Me.super Then
				PlayerData.Data.Loadouts.GetPlayerLoadout(MyBase.playerID).super = Super.None
			ElseIf Me.icons(Me.index) Is Me.item Then
				If PlayerData.Data.Loadouts.GetPlayerLoadout(MyBase.playerID).charm = Charm.charm_chalice Then
					PlayerManager.OnChaliceCharmUnequipped(MyBase.playerID)
				End If
				PlayerData.Data.Loadouts.GetPlayerLoadout(MyBase.playerID).charm = Charm.None
			Else
				Global.Debug.LogError("Something went wrong", Nothing)
			End If
		Else
			AudioManager.Play("menu_locked")
			Me.cursor.OnLocked()
		End If
		Me.Refresh()
		Me.ChangeSelection(0)
	End Sub

	' Token: 0x06003958 RID: 14680 RVA: 0x002089F0 File Offset: 0x00206DF0
	Public Sub ChangeSelection(direction As Integer)
		If(Me.index <> Me.icons.Length - 1 AndAlso direction <> -1) OrElse (Me.index <> 0 AndAlso direction <> 1) Then
			AudioManager.Play("menu_equipment_move")
		End If
		Me.index = Mathf.Clamp(Me.index + direction, 0, Me.icons.Length - 1)
		Me.SetCursorPosition(Me.index)
		Me.checkListSelected = Me.index = Me.icons.Length - 1
		Dim text As String = String.Empty
		If Me.icons(Me.index) Is Me.weaponA Then
			text = WeaponProperties.GetDisplayName(Me.loadout.primaryWeapon)
			If text.ToUpper() = "ERROR" Then
				text = Localization.Translate("level_weapon_none_name").text
			End If
			Me.title.text = text
		ElseIf Me.icons(Me.index) Is Me.weaponB Then
			text = WeaponProperties.GetDisplayName(Me.loadout.secondaryWeapon)
			If text.ToUpper() = "ERROR" Then
				text = Localization.Translate("level_weapon_none_name").text
			End If
			Me.title.text = text
		ElseIf Me.icons(Me.index) Is Me.super Then
			text = WeaponProperties.GetDisplayName(Me.loadout.super)
			If text.ToUpper() = "ERROR" Then
				text = Localization.Translate("level_super_none_name").text
			End If
			Me.title.text = text
		ElseIf Me.icons(Me.index) Is Me.item Then
			If Me.loadout.charm = Charm.charm_curse Then
				If CharmCurse.CalculateLevel(MyBase.playerID) = -1 Then
					text = Localization.Translate("charm_broken_name").text
				ElseIf CharmCurse.IsMaxLevel(MyBase.playerID) Then
					text = Localization.Translate("charm_paladin_name").text
				Else
					text = Localization.Translate("charm_curse_name").text
				End If
			Else
				text = WeaponProperties.GetDisplayName(Me.loadout.charm)
			End If
			If text.ToUpper() = "ERROR" Then
				text = Localization.Translate("charm_none_name").text
			End If
			Me.title.text = text
		Else
			Me.title.text = Localization.Translate("list_name").text
		End If
		Me.title.font = Localization.Instance.fonts(CInt(Localization.language))(9).font
		For Each outline As Outline In Me.outlines
			outline.enabled = Localization.language = Localization.Languages.Japanese
		Next
	End Sub

	' Token: 0x06003959 RID: 14681 RVA: 0x00208D26 File Offset: 0x00207126
	Private Sub SetCursorPosition(index As Integer)
		If Me.icons Is Nothing OrElse Me.icons.Length <= index Then
			Return
		End If
		Me.cursor.SetPosition(Me.icons(index).transform.position)
	End Sub

	' Token: 0x040040DE RID: 16606
	Public weaponA As MapEquipUICardFrontIcon

	' Token: 0x040040DF RID: 16607
	Public weaponB As MapEquipUICardFrontIcon

	' Token: 0x040040E0 RID: 16608
	Public super As MapEquipUICardFrontIcon

	' Token: 0x040040E1 RID: 16609
	Public item As MapEquipUICardFrontIcon

	' Token: 0x040040E2 RID: 16610
	Public checklist As MapEquipUICardFrontIcon

	' Token: 0x040040E3 RID: 16611
	Public checkListSelected As Boolean

	' Token: 0x040040E4 RID: 16612
	<Space(10F)>
	Public cursor As MapEquipUICursor

	' Token: 0x040040E5 RID: 16613
	Private index As Integer

	' Token: 0x040040E6 RID: 16614
	Private icons As MapEquipUICardFrontIcon()

	' Token: 0x040040E7 RID: 16615
	Public title As Text

	' Token: 0x040040E8 RID: 16616
	Public outlines As Outline()

	' Token: 0x040040E9 RID: 16617
	Private loadout As PlayerData.PlayerLoadouts.PlayerLoadout
End Class
