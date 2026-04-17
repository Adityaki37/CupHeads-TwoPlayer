Imports System
Imports UnityEngine

' Token: 0x020007C1 RID: 1985
Public Class SaltbakerLevelBGTrappedCharacter
	Inherits MonoBehaviour

	' Token: 0x06002CE8 RID: 11496 RVA: 0x001A7094 File Offset: 0x001A5494
	Public Sub Setup()
		If PlayerManager.Multiplayer Then
			If PlayerManager.GetPlayer(PlayerId.PlayerOne).stats.isChalice OrElse PlayerManager.GetPlayer(PlayerId.PlayerTwo).stats.isChalice Then
				If PlayerManager.GetPlayer(PlayerId.PlayerOne).stats.isChalice Then
					Me.charID = If((Not PlayerManager.player1IsMugman), SaltbakerLevelBGTrappedCharacter.Character.Cuphead, SaltbakerLevelBGTrappedCharacter.Character.Mugman)
				Else
					Me.charID = If((Not PlayerManager.player1IsMugman), SaltbakerLevelBGTrappedCharacter.Character.Mugman, SaltbakerLevelBGTrappedCharacter.Character.Cuphead)
				End If
			Else
				Me.charID = SaltbakerLevelBGTrappedCharacter.Character.Chalice
			End If
		ElseIf PlayerManager.GetPlayer(PlayerId.PlayerOne).stats.isChalice Then
			Me.charID = If((Not PlayerManager.player1IsMugman), SaltbakerLevelBGTrappedCharacter.Character.Cuphead, SaltbakerLevelBGTrappedCharacter.Character.Mugman)
		Else
			Me.charID = If((PlayerData.Data.Loadouts.GetPlayerLoadout(PlayerId.PlayerTwo).charm <> Charm.charm_chalice), SaltbakerLevelBGTrappedCharacter.Character.Chalice, If((Not PlayerManager.player1IsMugman), SaltbakerLevelBGTrappedCharacter.Character.Mugman, SaltbakerLevelBGTrappedCharacter.Character.Cuphead))
		End If
		For i As Integer = 0 To 3 - 1
			Me.characters(i).SetActive(i = CInt(Me.charID))
		Next
	End Sub

	' Token: 0x0400355F RID: 13663
	<SerializeField()>
	Private characters As GameObject()

	' Token: 0x04003560 RID: 13664
	Private charID As SaltbakerLevelBGTrappedCharacter.Character = SaltbakerLevelBGTrappedCharacter.Character.None

	' Token: 0x04003561 RID: 13665
	Private pOneID As SaltbakerLevelBGTrappedCharacter.Character = SaltbakerLevelBGTrappedCharacter.Character.None

	' Token: 0x04003562 RID: 13666
	Private pTwoID As SaltbakerLevelBGTrappedCharacter.Character = SaltbakerLevelBGTrappedCharacter.Character.None

	' Token: 0x020007C2 RID: 1986
	Private Enum Character
		' Token: 0x04003564 RID: 13668
		None = -1
		' Token: 0x04003565 RID: 13669
		Cuphead
		' Token: 0x04003566 RID: 13670
		Mugman
		' Token: 0x04003567 RID: 13671
		Chalice
	End Enum
End Class
