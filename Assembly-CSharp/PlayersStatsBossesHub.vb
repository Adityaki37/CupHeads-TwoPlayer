Imports System

' Token: 0x02000734 RID: 1844
Public Class PlayersStatsBossesHub
	' Token: 0x0600282A RID: 10282 RVA: 0x00176B95 File Offset: 0x00174F95
	Public Sub LoseBonusHP()
		If Me.BonusHP > 0 Then
			Me.BonusHP -= 1
		End If
	End Sub

	' Token: 0x0600282B RID: 10283 RVA: 0x00176BB1 File Offset: 0x00174FB1
	Public Sub LoseHealerHP()
		If Me.healerHP > 0 Then
			Me.healerHP -= 1
		End If
	End Sub

	' Token: 0x040030EA RID: 12522
	Public HP As Integer

	' Token: 0x040030EB RID: 12523
	Public BonusHP As Integer

	' Token: 0x040030EC RID: 12524
	Public SuperCharge As Single

	' Token: 0x040030ED RID: 12525
	Public basePrimaryWeapon As Weapon

	' Token: 0x040030EE RID: 12526
	Public baseSecondaryWeapon As Weapon

	' Token: 0x040030EF RID: 12527
	Public BaseSuper As Super

	' Token: 0x040030F0 RID: 12528
	Public BaseCharm As Charm

	' Token: 0x040030F1 RID: 12529
	Public tokenCount As Integer

	' Token: 0x040030F2 RID: 12530
	Public healerHP As Integer

	' Token: 0x040030F3 RID: 12531
	Public healerHPReceived As Integer

	' Token: 0x040030F4 RID: 12532
	Public healerHPCounter As Integer
End Class
