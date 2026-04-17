Imports System
Imports UnityEngine

' Token: 0x0200048E RID: 1166
Public Class LevelHUDPlayerSuper
	Inherits AbstractLevelHUDComponent

	' Token: 0x06001253 RID: 4691 RVA: 0x000A9958 File Offset: 0x000A7D58
	Public Overrides Sub Init(hud As LevelHUDPlayer)
		MyBase.Init(hud)
		Me.cardTemplate.Init(MyBase._player.id, MyBase._player.stats.ExCost)
		Me.cards = New LevelHUDPlayerSuperCard(4) {}
		Me.cards(0) = Me.cardTemplate
		For i As Integer = 1 To Me.cards.Length - 1
			Dim vector As Vector3 = Me.cardTemplate.transform.localPosition + New Vector3(18F * CSng(i), 0F, 0F)
			Dim levelHUDPlayerSuperCard As LevelHUDPlayerSuperCard = Global.UnityEngine.[Object].Instantiate(Of LevelHUDPlayerSuperCard)(Me.cardTemplate)
			levelHUDPlayerSuperCard.rectTransform.SetParent(Me.cardTemplate.rectTransform.parent, False)
			levelHUDPlayerSuperCard.rectTransform.localPosition = vector
			levelHUDPlayerSuperCard.Init(MyBase._player.id, MyBase._player.stats.ExCost)
			Me.cards(i) = levelHUDPlayerSuperCard
		Next
		Me.OnSuperChanged(MyBase._player.stats.SuperMeter)
	End Sub

	' Token: 0x06001254 RID: 4692 RVA: 0x000A9A68 File Offset: 0x000A7E68
	Public Sub OnSuperChanged(super As Single)
		For i As Integer = 0 To Me.cards.Length - 1
			Dim num As Single = MyBase._player.stats.SuperMeterMax / CSng(Me.cards.Length)
			Dim num2 As Single = num * CSng(i)
			Me.cards(i).SetAmount(super - num2)
		Next
		If super >= MyBase._player.stats.SuperMeterMax Then
			For Each levelHUDPlayerSuperCard As LevelHUDPlayerSuperCard In Me.cards
				If Not levelHUDPlayerSuperCard.animator.GetBool("Super") Then
					Me.superToReady = True
				Else
					Me.superToReady = False
				End If
			Next
			If Me.superToReady Then
				If(MyBase._player.id = PlayerId.PlayerOne AndAlso Not PlayerManager.player1IsMugman) OrElse (MyBase._player.id = PlayerId.PlayerTwo AndAlso PlayerManager.player1IsMugman) Then
					If Not AudioManager.CheckIfPlaying("player_parry_power_increment_cuphead") Then
						AudioManager.Play("player_parry_power_increment_cuphead")
					End If
				ElseIf Not AudioManager.CheckIfPlaying("player_parry_power_increment_mugman") Then
					AudioManager.Play("player_parry_power_increment_mugman")
				End If
			End If
			For Each levelHUDPlayerSuperCard2 As LevelHUDPlayerSuperCard In Me.cards
				levelHUDPlayerSuperCard2.SetSuper(True)
				If Me.lastSuper <> super Then
					Dim id As PlayerId = MyBase._player.id
					If id <> PlayerId.PlayerOne Then
						If id = PlayerId.PlayerTwo Then
							levelHUDPlayerSuperCard2.animator.Play(If((Not PlayerManager.player1IsMugman), "Mugman_Idle_B", "Cuphead_Idle_B"), 0, 0F)
						End If
					Else
						levelHUDPlayerSuperCard2.animator.Play(If((Not PlayerManager.player1IsMugman), "Cuphead_Idle_B", "Mugman_Idle_B"), 0, 0F)
					End If
				End If
			Next
		Else
			For l As Integer = 0 To Me.cards.Length - 1
				Me.cards(l).SetSuper(False)
				Dim num3 As Single = MyBase._player.stats.SuperMeterMax / CSng(Me.cards.Length)
				If super >= num3 + num3 * CSng(l) Then
					Me.cards(l).SetEx(True)
					If Me.cards(l).animator.GetCurrentAnimatorStateInfo(0).IsName("Cuphead_Spin_A") Then
						If Not AudioManager.CheckIfPlaying("player_parry_power_increment_cuphead") Then
							AudioManager.Play("player_parry_power_increment_cuphead")
						End If
					ElseIf Me.cards(l).animator.GetCurrentAnimatorStateInfo(0).IsName("Mugman_Spin_A") AndAlso Not AudioManager.CheckIfPlaying("player_parry_power_increment_mugman") Then
						AudioManager.Play("player_parry_power_increment_mugman")
					End If
				Else
					Me.cards(l).SetEx(False)
				End If
			Next
		End If
		Me.superToReady = False
		Me.lastSuper = super
	End Sub

	' Token: 0x04001BBE RID: 7102
	Private Const SPACE As Single = 18F

	' Token: 0x04001BBF RID: 7103
	<SerializeField()>
	Private cardTemplate As LevelHUDPlayerSuperCard

	' Token: 0x04001BC0 RID: 7104
	Private cards As LevelHUDPlayerSuperCard()

	' Token: 0x04001BC1 RID: 7105
	Private lastSuper As Single

	' Token: 0x04001BC2 RID: 7106
	Private superToReady As Boolean
End Class
