Imports System
Imports UnityEngine

' Token: 0x02000AC4 RID: 2756
Public Class PlayerDamageReceiver
	Inherits DamageReceiver

	' Token: 0x170005CD RID: 1485
	' (get) Token: 0x0600422D RID: 16941 RVA: 0x0023C0B8 File Offset: 0x0023A4B8
	' (set) Token: 0x0600422E RID: 16942 RVA: 0x0023C0C0 File Offset: 0x0023A4C0
	Public Property state As PlayerDamageReceiver.State

	' Token: 0x0600422F RID: 16943 RVA: 0x0023C0C9 File Offset: 0x0023A4C9
	Protected Overrides Sub Awake()
		MyBase.Awake()
		If Me.type <> DamageReceiver.Type.Player Then
		End If
		Me.type = DamageReceiver.Type.Player
		Me.player = MyBase.GetComponent(Of AbstractPlayerController)()
		AddHandler Me.player.OnReviveEvent, AddressOf Me.OnRevive
	End Sub

	' Token: 0x06004230 RID: 16944 RVA: 0x0023C108 File Offset: 0x0023A508
	Private Sub Update()
		If Me.state <> PlayerDamageReceiver.State.Invulnerable Then
			Return
		End If
		If Me.timer > 0F Then
			Me.timer -= CupheadTime.Delta
			If Me.timer <= 0F Then
				Me.Vulnerable()
			End If
		End If
	End Sub

	' Token: 0x06004231 RID: 16945 RVA: 0x0023C160 File Offset: 0x0023A560
	Private Sub HandleChaliceShmupSuper(info As DamageDealer.DamageInfo)
		If Me.player.stats.State = PlayerStatsManager.PlayerState.Super AndAlso Me.player.stats.isChalice AndAlso Me.player.stats.Loadout.super = Super.level_super_ghost Then
			MyBase.TakeDamageBruteForce(info)
			Return
		End If
	End Sub

	' Token: 0x06004232 RID: 16946 RVA: 0x0023C1C0 File Offset: 0x0023A5C0
	Public Overrides Sub TakeDamage(info As DamageDealer.DamageInfo)
		If Me.player.stats.SuperInvincible Then
			Return
		End If
		If info.damage > 0F Then
			Me.HandleChaliceShmupSuper(info)
			If Not MyBase.enabled Then
				Return
			End If
			If info.damageSource = DamageDealer.DamageSource.Pit Then
				If Me.player.damageReceiver.state <> PlayerDamageReceiver.State.Vulnerable Then
					Return
				End If
			ElseIf Not Me.player.CanTakeDamage Then
				Return
			End If
			If Me.timer > 0F Then
				Return
			End If
			Dim num As Single = 1F
			Me.Invulnerable(2F * num)
			MyBase.TakeDamage(info)
			If Me.player.stats.ChaliceShieldOn Then
				Me.player.stats.SetChaliceShield(False)
			End If
		ElseIf info.stoneTime > 0F Then
			MyBase.TakeDamage(info)
		End If
	End Sub

	' Token: 0x06004233 RID: 16947 RVA: 0x0023C2AB File Offset: 0x0023A6AB
	Public Sub OnRevive(pos As Vector3)
		Me.Invulnerable(3F)
	End Sub

	' Token: 0x06004234 RID: 16948 RVA: 0x0023C2B8 File Offset: 0x0023A6B8
	Public Sub Invulnerable(time As Single)
		Me.state = PlayerDamageReceiver.State.Invulnerable
		Me.timer = time
	End Sub

	' Token: 0x06004235 RID: 16949 RVA: 0x0023C2C8 File Offset: 0x0023A6C8
	Public Sub Vulnerable()
		Me.state = PlayerDamageReceiver.State.Vulnerable
		Me.timer = 0F
	End Sub

	' Token: 0x06004236 RID: 16950 RVA: 0x0023C2DC File Offset: 0x0023A6DC
	Public Sub OnDeath()
		Me.state = PlayerDamageReceiver.State.Other
	End Sub

	' Token: 0x06004237 RID: 16951 RVA: 0x0023C2E5 File Offset: 0x0023A6E5
	Public Sub OnWin()
		Me.state = PlayerDamageReceiver.State.Other
	End Sub

	' Token: 0x040048A7 RID: 18599
	Private Const TIME_HIT As Single = 2F

	' Token: 0x040048A8 RID: 18600
	Private Const TIME_REVIVED As Single = 3F

	' Token: 0x040048A9 RID: 18601
	Private player As AbstractPlayerController

	' Token: 0x040048AA RID: 18602
	Private timer As Single

	' Token: 0x02000AC5 RID: 2757
	Public Enum State
		' Token: 0x040048AC RID: 18604
		Vulnerable
		' Token: 0x040048AD RID: 18605
		Invulnerable
		' Token: 0x040048AE RID: 18606
		Other
	End Enum
End Class
