Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports System.IO
Imports RektTransform
Imports UnityEngine
Imports UnityEngine.U2D
Imports UnityEngine.UI

' Token: 0x02000985 RID: 2437
Public Class AbstractMapCardIcon
	Inherits AbstractMonoBehaviour

	' Token: 0x060038F2 RID: 14578 RVA: 0x00205978 File Offset: 0x00203D78
	Public Sub SetIcons(weapon As Weapon, isGrey As Boolean)
		Dim text As String = AbstractMapCardIcon.DefaultAtlas
		If Array.IndexOf(Of Weapon)(AbstractMapCardIcon.DLCWeapons, weapon) > -1 Then
			text = AbstractMapCardIcon.DLCAtlas
			Dim white As Color = Color.white
			If isGrey Then
				white = New Color(1F, 1F, 1F, 0.5F)
			End If
			Me.iconImage.color = white
		End If
		Me.setIcons(WeaponProperties.GetIconPath(weapon), isGrey, text)
	End Sub

	' Token: 0x060038F3 RID: 14579 RVA: 0x002059E3 File Offset: 0x00203DE3
	Public Sub SetIcons(super As Super, isGrey As Boolean)
		Me.setIcons(WeaponProperties.GetIconPath(super), isGrey, AbstractMapCardIcon.DefaultAtlas)
	End Sub

	' Token: 0x060038F4 RID: 14580 RVA: 0x002059F8 File Offset: 0x00203DF8
	Public Sub SetIcons(charm As Charm, isGrey As Boolean)
		Dim text As String = AbstractMapCardIcon.DefaultAtlas
		If Array.IndexOf(Of Charm)(AbstractMapCardIcon.DLCCharms, charm) > -1 Then
			text = AbstractMapCardIcon.DLCAtlas
		End If
		Me.setIcons(WeaponProperties.GetIconPath(charm), isGrey, text)
	End Sub

	' Token: 0x060038F5 RID: 14581 RVA: 0x00205A30 File Offset: 0x00203E30
	Public Sub SetIconsManual(iconPath As String, isGrey As Boolean, Optional isDLC As Boolean = False)
		Me.setIcons(iconPath, isGrey, If((Not isDLC), AbstractMapCardIcon.DefaultAtlas, AbstractMapCardIcon.DLCAtlas))
	End Sub

	' Token: 0x060038F6 RID: 14582 RVA: 0x00205A50 File Offset: 0x00203E50
	Private Sub setIcons(iconPath As String, isGrey As Boolean, atlasName As String)
		Dim cachedAsset As SpriteAtlas = AssetLoader(Of SpriteAtlas).GetCachedAsset(atlasName)
		Dim list As List(Of Sprite) = New List(Of Sprite)()
		Dim fileName As String = Path.GetFileName(iconPath)
		Dim sprite As Sprite = Me.getSprite(cachedAsset, fileName)
		If sprite IsNot Nothing Then
			list.Add(sprite)
		End If
		For i As Integer = 1 To 4 - 1
			Dim text As String = "_000"
			Dim fileName2 As String = Path.GetFileName(iconPath + text + i)
			Dim sprite2 As Sprite = Me.getSprite(cachedAsset, fileName2)
			If Not(sprite2 Is Nothing) Then
				list.Add(sprite2)
			End If
		Next
		Me.normalIcons = list.ToArray()
		list.Clear()
		If sprite IsNot Nothing Then
			list.Add(sprite)
		End If
		For j As Integer = 1 To 4 - 1
			Dim text2 As String = "_grey_000"
			Dim fileName3 As String = Path.GetFileName(iconPath + text2 + j)
			Dim sprite3 As Sprite = Me.getSprite(cachedAsset, fileName3)
			If Not(sprite3 Is Nothing) Then
				list.Add(sprite3)
			End If
		Next
		Me.greyIcons = list.ToArray()
		Me.icons = If((Not isGrey), Me.normalIcons, Me.greyIcons)
		Me.StopAllCoroutines()
		If iconPath <> WeaponProperties.GetIconPath(Weapon.None) Then
			MyBase.StartCoroutine(Me.animate_cr())
		Else
			Me.SetIcon(Me.icons(0))
		End If
	End Sub

	' Token: 0x060038F7 RID: 14583 RVA: 0x00205BC8 File Offset: 0x00203FC8
	Public Sub SetIcons(iconPath As String)
		Dim cachedAsset As SpriteAtlas = AssetLoader(Of SpriteAtlas).GetCachedAsset("Equip_Icons")
		Dim list As List(Of Sprite) = New List(Of Sprite)()
		Dim fileName As String = Path.GetFileName(iconPath)
		Dim sprite As Sprite = Me.getSprite(cachedAsset, fileName)
		If sprite IsNot Nothing Then
			list.Add(sprite)
		End If
		Dim fileName2 As String = Path.GetFileName(iconPath)
		Dim sprite2 As Sprite = Me.getSprite(cachedAsset, fileName2)
		list.Add(sprite2)
		Me.icons = list.ToArray()
		Me.SetIcon(sprite2)
	End Sub

	' Token: 0x060038F8 RID: 14584 RVA: 0x00205C38 File Offset: 0x00204038
	Public Overridable Sub SelectIcon()
		If MyBase.animator IsNot Nothing Then
			MyBase.animator.Play("Select")
		End If
	End Sub

	' Token: 0x060038F9 RID: 14585 RVA: 0x00205C5B File Offset: 0x0020405B
	Public Overridable Sub UnselectIcon()
		If MyBase.animator IsNot Nothing Then
			MyBase.animator.Play("Unselect")
		End If
	End Sub

	' Token: 0x060038FA RID: 14586 RVA: 0x00205C7E File Offset: 0x0020407E
	Public Overridable Sub OnLocked()
		If MyBase.animator IsNot Nothing Then
			MyBase.animator.Play("Locked")
		End If
	End Sub

	' Token: 0x060038FB RID: 14587 RVA: 0x00205CA4 File Offset: 0x002040A4
	Private Sub SetIcon(sprite As Sprite)
		If sprite Is Nothing Then
			Return
		End If
		Me.iconImage.sprite = sprite
		Me.iconImage.rectTransform.SetSize(sprite.rect.width, sprite.rect.height)
	End Sub

	' Token: 0x060038FC RID: 14588 RVA: 0x00205CF8 File Offset: 0x002040F8
	Private Iterator Function animate_cr() As IEnumerator
		Dim i As Integer = 0
		Dim wait As WaitForSeconds = New WaitForSeconds(0.07F)
		Me.SetIcon(If((Me.icons IsNot Nothing AndAlso Me.icons.Length >= 1), Me.icons(0), Nothing))
		While True
			Yield wait
			If Me.icons Is Nothing OrElse Me.icons.Length < 1 Then
				Me.SetIcon(Nothing)
			Else
				i += 1
				If i > Me.icons.Length - 1 Then
					i = 0
				End If
				Me.SetIcon(Me.icons(i))
			End If
		End While
		Return
	End Function

	' Token: 0x060038FD RID: 14589 RVA: 0x00205D14 File Offset: 0x00204114
	Private Function getSprite(atlas As SpriteAtlas, spriteName As String) As Sprite
		Return atlas.GetSprite(spriteName)
	End Function

	' Token: 0x04004088 RID: 16520
	Private Const FRAME_DELAY As Single = 0.07F

	' Token: 0x04004089 RID: 16521
	Private Shared DLCWeapons As Weapon() = New Weapon() { Weapon.level_weapon_crackshot, Weapon.level_weapon_upshot, Weapon.level_weapon_wide_shot }

	' Token: 0x0400408A RID: 16522
	Private Shared DLCCharms As Charm() = New Charm() { Charm.charm_chalice, Charm.charm_curse, Charm.charm_healer }

	' Token: 0x0400408B RID: 16523
	Private Shared DefaultAtlas As String = "Equip_Icons"

	' Token: 0x0400408C RID: 16524
	Private Shared DLCAtlas As String = "Equip_Icons_DLC"

	' Token: 0x0400408D RID: 16525
	<SerializeField()>
	Private iconImage As Image

	' Token: 0x0400408E RID: 16526
	Private icons As Sprite()

	' Token: 0x0400408F RID: 16527
	Private normalIcons As Sprite()

	' Token: 0x04004090 RID: 16528
	Private greyIcons As Sprite()
End Class
