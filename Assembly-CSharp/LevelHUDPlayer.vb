Imports System
Imports System.Collections
Imports RektTransform
Imports UnityEngine
Imports UnityEngine.Serialization

' Token: 0x0200048C RID: 1164
Public Class LevelHUDPlayer
	Inherits AbstractPausableComponent

	' Token: 0x170002D2 RID: 722
	' (get) Token: 0x06001242 RID: 4674 RVA: 0x000A92C5 File Offset: 0x000A76C5
	' (set) Token: 0x06001243 RID: 4675 RVA: 0x000A92CD File Offset: 0x000A76CD
	Public Property player As AbstractPlayerController

	' Token: 0x06001244 RID: 4676 RVA: 0x000A92D8 File Offset: 0x000A76D8
	Public Sub Init(player As AbstractPlayerController, Optional startAtOneHealth As Boolean = False)
		Me.player = player
		If player.id = PlayerId.PlayerTwo Then
			Me.SetupPlayerTwo()
		End If
		AddHandler player.stats.OnHealthChangedEvent, AddressOf Me.OnHealthChanged
		AddHandler player.stats.OnSuperChangedEvent, AddressOf Me.OnSuperChanged
		AddHandler player.stats.OnWeaponChangedEvent, AddressOf Me.OnWeaponChanged
		Me.health.Init(Me)
		If startAtOneHealth Then
			Me.health.OnHealthChanged(1)
		End If
		Me.super.Init(Me)
		If TryCast(player, PlanePlayerController) IsNot Nothing Then
			Me.weaponSwitchNotification.gameObject.SetActive(PlayerData.Data.Loadouts.GetPlayerLoadout(player.id).MustNotifySwitchSHMUPWeapon AndAlso Not Level.IsTowerOfPowerMain)
		Else
			Me.weaponSwitchNotification.gameObject.SetActive(PlayerData.Data.Loadouts.GetPlayerLoadout(player.id).MustNotifySwitchRegularWeapon AndAlso Not Level.IsTowerOfPowerMain)
		End If
		Me.weaponSwitchNotification.alpha = 1F
		Me.weaponSwitchTransform = Me.weaponSwitchNotification.GetComponent(Of RectTransform)()
		Me.weaponSwitchStartPosition = Me.weaponSwitchTransform.anchoredPosition
	End Sub

	' Token: 0x06001245 RID: 4677 RVA: 0x000A942C File Offset: 0x000A782C
	Private Sub SetupPlayerTwo()
		MyBase.gameObject.name = "Mugman"
		Dim vector As Vector3 = MyBase.transform.localPosition
		vector.x *= -1F
		MyBase.rectTransform.SetAnchors(New Global.RektTransform.MinMax(New Vector2(1F, 0F), New Vector2(1F, 0F)))
		MyBase.rectTransform.pivot = New Vector2(1F, 0F)
		MyBase.transform.localPosition = vector
		vector = Me.health.rectTransform.localPosition
		vector.x *= -1F
		Me.health.rectTransform.localPosition = vector
		Me.weaponRoot.localPosition = vector
		vector = Me.super.rectTransform.localPosition
		vector.x *= -1F
		Me.super.rectTransform.SetScale(New Single?(-1F), Nothing, Nothing)
		Me.super.rectTransform.localPosition = vector
	End Sub

	' Token: 0x06001246 RID: 4678 RVA: 0x000A955E File Offset: 0x000A795E
	Private Sub OnHealthChanged(health As Integer, playerId As PlayerId)
		Me.health.OnHealthChanged(health)
	End Sub

	' Token: 0x06001247 RID: 4679 RVA: 0x000A956C File Offset: 0x000A796C
	Private Sub OnSuperChanged(super As Single, playerId As PlayerId, playEffect As Boolean)
		Me.super.OnSuperChanged(super)
	End Sub

	' Token: 0x06001248 RID: 4680 RVA: 0x000A957C File Offset: 0x000A797C
	Private Sub OnWeaponChanged(weapon As Weapon)
		Me.weaponIconPrefab.Create(Me.weaponRoot, weapon)
		If Me.weaponSwitchNotification.gameObject.activeSelf Then
			Dim flag As Boolean = PlayerData.Data.Loadouts.GetPlayerLoadout(Me.player.id).MustNotifySwitchSHMUPWeapon OrElse PlayerData.Data.Loadouts.GetPlayerLoadout(Me.player.id).MustNotifySwitchRegularWeapon
			If TryCast(Me.player, PlanePlayerController) IsNot Nothing Then
				PlayerData.Data.Loadouts.GetPlayerLoadout(Me.player.id).MustNotifySwitchSHMUPWeapon = False
			Else
				PlayerData.Data.Loadouts.GetPlayerLoadout(Me.player.id).MustNotifySwitchRegularWeapon = False
			End If
			If flag Then
				PlayerData.SaveCurrentFile()
			End If
			MyBase.StartCoroutine(Me.FadeOutSwitchNotification(0.4F))
		End If
	End Sub

	' Token: 0x06001249 RID: 4681 RVA: 0x000A9670 File Offset: 0x000A7A70
	Private Sub Update()
		If Me.weaponSwitchNotification.gameObject.activeSelf Then
			Me.weaponSwitchTransform.anchoredPosition = Me.weaponSwitchStartPosition + Vector2.up * Mathf.Sin(Time.time * Me.weaponSwitchWobbleSpeed) * Me.weaponSwitchWobbleScale
		End If
	End Sub

	' Token: 0x0600124A RID: 4682 RVA: 0x000A96CE File Offset: 0x000A7ACE
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me.health = Nothing
		Me.super = Nothing
	End Sub

	' Token: 0x0600124B RID: 4683 RVA: 0x000A96E4 File Offset: 0x000A7AE4
	Private Iterator Function FadeOutSwitchNotification(overTime As Single) As IEnumerator
		If overTime > 0F Then
			Dim startAlpha As Single = Me.weaponSwitchNotification.alpha
			Dim timeSpent As Single = 0F
			While timeSpent < overTime
				Me.weaponSwitchNotification.alpha = Mathf.Lerp(startAlpha, 0F, timeSpent / overTime)
				timeSpent += Time.deltaTime
				Yield Nothing
			End While
			Me.weaponSwitchNotification.gameObject.SetActive(False)
		End If
		Return
	End Function

	' Token: 0x04001BB1 RID: 7089
	<SerializeField()>
	Private health As LevelHUDPlayerHealth

	' Token: 0x04001BB2 RID: 7090
	<SerializeField()>
	Private super As LevelHUDPlayerSuper

	' Token: 0x04001BB3 RID: 7091
	<Space(10F)>
	<SerializeField()>
	Private weaponRoot As RectTransform

	' Token: 0x04001BB4 RID: 7092
	<SerializeField()>
	<FormerlySerializedAs("weaponPrefab")>
	Private weaponIconPrefab As LevelHUDWeapon

	' Token: 0x04001BB5 RID: 7093
	<SerializeField()>
	Private weaponSwitchNotification As CanvasGroup

	' Token: 0x04001BB6 RID: 7094
	Public weaponSwitchWobbleSpeed As Single = 1F

	' Token: 0x04001BB7 RID: 7095
	Public weaponSwitchWobbleScale As Single = 1F

	' Token: 0x04001BB9 RID: 7097
	Private weaponSwitchTransform As RectTransform

	' Token: 0x04001BBA RID: 7098
	Private weaponSwitchStartPosition As Vector2
End Class
