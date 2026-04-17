Imports System
Imports System.Collections
Imports System.Diagnostics
Imports UnityEngine

' Token: 0x02000AD3 RID: 2771
Public Class PlayerStatsManager
	Inherits AbstractPlayerComponent

	' Token: 0x170005D9 RID: 1497
	' (get) Token: 0x0600428D RID: 17037 RVA: 0x0023DDCD File Offset: 0x0023C1CD
	' (set) Token: 0x0600428E RID: 17038 RVA: 0x0023DDD4 File Offset: 0x0023C1D4
	Public Shared Property GlobalInvincibility As Boolean

	' Token: 0x170005DA RID: 1498
	' (get) Token: 0x0600428F RID: 17039 RVA: 0x0023DDDC File Offset: 0x0023C1DC
	' (set) Token: 0x06004290 RID: 17040 RVA: 0x0023DDE4 File Offset: 0x0023C1E4
	Public Property HealthMax As Integer

	' Token: 0x170005DB RID: 1499
	' (get) Token: 0x06004291 RID: 17041 RVA: 0x0023DDED File Offset: 0x0023C1ED
	' (set) Token: 0x06004292 RID: 17042 RVA: 0x0023DDF5 File Offset: 0x0023C1F5
	Public Property Health As Integer

	' Token: 0x170005DC RID: 1500
	' (get) Token: 0x06004293 RID: 17043 RVA: 0x0023DDFE File Offset: 0x0023C1FE
	' (set) Token: 0x06004294 RID: 17044 RVA: 0x0023DE06 File Offset: 0x0023C206
	Public Property HealerHP As Integer

	' Token: 0x170005DD RID: 1501
	' (get) Token: 0x06004295 RID: 17045 RVA: 0x0023DE0F File Offset: 0x0023C20F
	' (set) Token: 0x06004296 RID: 17046 RVA: 0x0023DE17 File Offset: 0x0023C217
	Public Property HealerHPReceived As Integer

	' Token: 0x170005DE RID: 1502
	' (get) Token: 0x06004297 RID: 17047 RVA: 0x0023DE20 File Offset: 0x0023C220
	' (set) Token: 0x06004298 RID: 17048 RVA: 0x0023DE28 File Offset: 0x0023C228
	Public Property HealerHPCounter As Integer

	' Token: 0x170005DF RID: 1503
	' (get) Token: 0x06004299 RID: 17049 RVA: 0x0023DE31 File Offset: 0x0023C231
	' (set) Token: 0x0600429A RID: 17050 RVA: 0x0023DE39 File Offset: 0x0023C239
	Public Property CurseCharmLevel As Integer
		Get
			Return Me._curseCharmLevel
		End Get
		Private Set(value As Integer)
			Me._curseCharmLevel = value
		End Set
	End Property

	' Token: 0x170005E0 RID: 1504
	' (get) Token: 0x0600429B RID: 17051 RVA: 0x0023DE42 File Offset: 0x0023C242
	Public ReadOnly Property CurseSmokeDash As Boolean
		Get
			Return Me.Loadout.charm = Charm.charm_curse AndAlso Me.CurseCharmLevel >= 0 AndAlso Me.curseCharmDashCounter = 0
		End Get
	End Property

	' Token: 0x170005E1 RID: 1505
	' (get) Token: 0x0600429C RID: 17052 RVA: 0x0023DE71 File Offset: 0x0023C271
	Public ReadOnly Property CurseWhetsone As Boolean
		Get
			Return Me.Loadout.charm = Charm.charm_curse AndAlso Me.CurseCharmLevel >= 0 AndAlso Me.curseCharmWhetstoneCounter = 0
		End Get
	End Property

	' Token: 0x170005E2 RID: 1506
	' (get) Token: 0x0600429D RID: 17053 RVA: 0x0023DEA0 File Offset: 0x0023C2A0
	' (set) Token: 0x0600429E RID: 17054 RVA: 0x0023DEA8 File Offset: 0x0023C2A8
	Public Property SuperMeterMax As Single

	' Token: 0x170005E3 RID: 1507
	' (get) Token: 0x0600429F RID: 17055 RVA: 0x0023DEB1 File Offset: 0x0023C2B1
	' (set) Token: 0x060042A0 RID: 17056 RVA: 0x0023DEB9 File Offset: 0x0023C2B9
	Public Property SuperMeter As Single

	' Token: 0x170005E4 RID: 1508
	' (get) Token: 0x060042A1 RID: 17057 RVA: 0x0023DEC2 File Offset: 0x0023C2C2
	' (set) Token: 0x060042A2 RID: 17058 RVA: 0x0023DECA File Offset: 0x0023C2CA
	Public Property SuperInvincible As Boolean

	' Token: 0x170005E5 RID: 1509
	' (get) Token: 0x060042A3 RID: 17059 RVA: 0x0023DED3 File Offset: 0x0023C2D3
	' (set) Token: 0x060042A4 RID: 17060 RVA: 0x0023DEDB File Offset: 0x0023C2DB
	Public Property ChaliceShieldOn As Boolean

	' Token: 0x170005E6 RID: 1510
	' (get) Token: 0x060042A5 RID: 17061 RVA: 0x0023DEE4 File Offset: 0x0023C2E4
	Public ReadOnly Property CanGainSuperMeter As Boolean
		Get
			Return Me.Loadout.charm <> Charm.charm_EX AndAlso (Not Me.SuperInvincible OrElse Me.ChaliceShieldOn)
		End Get
	End Property

	' Token: 0x170005E7 RID: 1511
	' (get) Token: 0x060042A6 RID: 17062 RVA: 0x0023DF12 File Offset: 0x0023C312
	' (set) Token: 0x060042A7 RID: 17063 RVA: 0x0023DF1A File Offset: 0x0023C31A
	Public Property ExCost As Single

	' Token: 0x170005E8 RID: 1512
	' (get) Token: 0x060042A8 RID: 17064 RVA: 0x0023DF23 File Offset: 0x0023C323
	' (set) Token: 0x060042A9 RID: 17065 RVA: 0x0023DF2B File Offset: 0x0023C32B
	Public Property Deaths As Integer

	' Token: 0x170005E9 RID: 1513
	' (get) Token: 0x060042AA RID: 17066 RVA: 0x0023DF34 File Offset: 0x0023C334
	' (set) Token: 0x060042AB RID: 17067 RVA: 0x0023DF3C File Offset: 0x0023C33C
	Public Property ParriesThisJump As Integer

	' Token: 0x170005EA RID: 1514
	' (get) Token: 0x060042AC RID: 17068 RVA: 0x0023DF45 File Offset: 0x0023C345
	' (set) Token: 0x060042AD RID: 17069 RVA: 0x0023DF4D File Offset: 0x0023C34D
	Public Property StoneTime As Single

	' Token: 0x170005EB RID: 1515
	' (get) Token: 0x060042AE RID: 17070 RVA: 0x0023DF56 File Offset: 0x0023C356
	' (set) Token: 0x060042AF RID: 17071 RVA: 0x0023DF5E File Offset: 0x0023C35E
	Public Property ReverseTime As Single

	' Token: 0x170005EC RID: 1516
	' (get) Token: 0x060042B0 RID: 17072 RVA: 0x0023DF67 File Offset: 0x0023C367
	Public ReadOnly Property CanUseEx As Boolean
		Get
			Return Me.Loadout.charm = Charm.charm_EX OrElse (Me.SuperMeter >= Me.ExCost AndAlso Me.CanGainSuperMeter)
		End Get
	End Property

	' Token: 0x170005ED RID: 1517
	' (get) Token: 0x060042B1 RID: 17073 RVA: 0x0023DF9B File Offset: 0x0023C39B
	' (set) Token: 0x060042B2 RID: 17074 RVA: 0x0023DFA3 File Offset: 0x0023C3A3
	Public Property Loadout As PlayerData.PlayerLoadouts.PlayerLoadout

	' Token: 0x170005EE RID: 1518
	' (get) Token: 0x060042B3 RID: 17075 RVA: 0x0023DFAC File Offset: 0x0023C3AC
	' (set) Token: 0x060042B4 RID: 17076 RVA: 0x0023DFB4 File Offset: 0x0023C3B4
	Public Property State As PlayerStatsManager.PlayerState

	' Token: 0x170005EF RID: 1519
	' (get) Token: 0x060042B5 RID: 17077 RVA: 0x0023DFBD File Offset: 0x0023C3BD
	' (set) Token: 0x060042B6 RID: 17078 RVA: 0x0023DFC5 File Offset: 0x0023C3C5
	Public Property DiceGameBonusHP As Boolean

	' Token: 0x140000AD RID: 173
	' (add) Token: 0x060042B7 RID: 17079 RVA: 0x0023DFD0 File Offset: 0x0023C3D0
	' (remove) Token: 0x060042B8 RID: 17080 RVA: 0x0023E008 File Offset: 0x0023C408
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnHealthChangedEvent As PlayerStatsManager.OnPlayerHealthChangeHandler

	' Token: 0x140000AE RID: 174
	' (add) Token: 0x060042B9 RID: 17081 RVA: 0x0023E040 File Offset: 0x0023C440
	' (remove) Token: 0x060042BA RID: 17082 RVA: 0x0023E078 File Offset: 0x0023C478
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnSuperChangedEvent As PlayerStatsManager.OnPlayerSuperChangedHandler

	' Token: 0x140000AF RID: 175
	' (add) Token: 0x060042BB RID: 17083 RVA: 0x0023E0B0 File Offset: 0x0023C4B0
	' (remove) Token: 0x060042BC RID: 17084 RVA: 0x0023E0E8 File Offset: 0x0023C4E8
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnWeaponChangedEvent As PlayerStatsManager.OnPlayerWeaponChangedHandler

	' Token: 0x140000B0 RID: 176
	' (add) Token: 0x060042BD RID: 17085 RVA: 0x0023E120 File Offset: 0x0023C520
	' (remove) Token: 0x060042BE RID: 17086 RVA: 0x0023E158 File Offset: 0x0023C558
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnPlayerDeathEvent As PlayerStatsManager.OnPlayerDeathHandler

	' Token: 0x140000B1 RID: 177
	' (add) Token: 0x060042BF RID: 17087 RVA: 0x0023E190 File Offset: 0x0023C590
	' (remove) Token: 0x060042C0 RID: 17088 RVA: 0x0023E1C8 File Offset: 0x0023C5C8
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnPlayerReviveEvent As PlayerStatsManager.OnPlayerDeathHandler

	' Token: 0x140000B2 RID: 178
	' (add) Token: 0x060042C1 RID: 17089 RVA: 0x0023E200 File Offset: 0x0023C600
	' (remove) Token: 0x060042C2 RID: 17090 RVA: 0x0023E238 File Offset: 0x0023C638
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnStoneShake As PlayerStatsManager.OnStoneHandler

	' Token: 0x140000B3 RID: 179
	' (add) Token: 0x060042C3 RID: 17091 RVA: 0x0023E270 File Offset: 0x0023C670
	' (remove) Token: 0x060042C4 RID: 17092 RVA: 0x0023E2A8 File Offset: 0x0023C6A8
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnStoned As PlayerStatsManager.OnStoneHandler

	' Token: 0x060042C5 RID: 17093 RVA: 0x0023E2E0 File Offset: 0x0023C6E0
	Protected Overrides Sub OnAwake()
		MyBase.OnAwake()
		PlayerStatsManager.GlobalInvincibility = False
		PlayerStatsManager.DebugInvincible = False
		Me.SuperInvincible = False
		Me.ChaliceShieldOn = False
		AddHandler MyBase.basePlayer.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
		Dim levelPlayerController As LevelPlayerController = TryCast(MyBase.basePlayer, LevelPlayerController)
		Dim planePlayerController As PlanePlayerController = TryCast(MyBase.basePlayer, PlanePlayerController)
		If levelPlayerController IsNot Nothing Then
			AddHandler levelPlayerController.motor.OnDashStartEvent, AddressOf Me.onDashStartEventHandler
			AddHandler levelPlayerController.motor.OnParryEvent, AddressOf Me.onParryEventHandler
		ElseIf planePlayerController IsNot Nothing Then
			AddHandler planePlayerController.animationController.OnShrinkEvent, AddressOf Me.onShrinkEventHandler
			AddHandler planePlayerController.parryController.OnParryStartEvent, AddressOf Me.onParryEventHandler
		End If
		Dim component As LevelPlayerWeaponManager = MyBase.GetComponent(Of LevelPlayerWeaponManager)()
		If component IsNot Nothing Then
			AddHandler component.OnWeaponChangeEvent, AddressOf Me.OnWeaponChange
			AddHandler component.OnSuperEnd, AddressOf Me.OnSuperEnd
		End If
		Dim component2 As PlanePlayerWeaponManager = MyBase.GetComponent(Of PlanePlayerWeaponManager)()
		If component2 IsNot Nothing Then
			AddHandler component2.OnWeaponChangeEvent, AddressOf Me.OnWeaponChange
		End If
		Me.Deaths = 0
		Me.hardInvincibility = False
	End Sub

	' Token: 0x060042C6 RID: 17094 RVA: 0x0023E424 File Offset: 0x0023C824
	Private Sub OnEnable()
		If Me.superBuilderRoutine IsNot Nothing Then
			MyBase.StopCoroutine(Me.superBuilderRoutine)
		End If
		Me.superBuilderRoutine = Me.charmSuperBuilder_cr()
		MyBase.StartCoroutine(Me.superBuilderRoutine)
	End Sub

	' Token: 0x060042C7 RID: 17095 RVA: 0x0023E456 File Offset: 0x0023C856
	Private Sub FixedUpdate()
		Me.UpdateStone()
		Me.UpdateReverse()
	End Sub

	' Token: 0x060042C8 RID: 17096 RVA: 0x0023E464 File Offset: 0x0023C864
	Public Sub LevelInit()
		AddHandler Level.Current.OnWinEvent, AddressOf Me.OnWin
		AddHandler Level.Current.OnLoseEvent, AddressOf Me.OnLose
		Me.Loadout = PlayerData.Data.Loadouts.GetPlayerLoadout(MyBase.basePlayer.id)
		Me.isChalice = Me.Loadout.charm = Charm.charm_chalice AndAlso Not Level.Current.BlockChaliceCharm(CInt(MyBase.basePlayer.id))
		If Not Level.Current.blockChalice AndAlso (PlayerManager.playerWasChalice(0) OrElse PlayerManager.playerWasChalice(1)) Then
			Me.isChalice = PlayerManager.playerWasChalice(CInt(MyBase.basePlayer.id))
		End If
		If Level.IsDicePalace AndAlso Not DicePalaceMainLevelGameInfo.IS_FIRST_ENTRY Then
			Me.isChalice = MyBase.basePlayer.id = CType(DicePalaceMainLevelGameInfo.CHALICE_PLAYER, PlayerId)
		End If
		If Me.Loadout.charm = Charm.charm_curse Then
			Me.CurseCharmLevel = CharmCurse.CalculateLevel(MyBase.basePlayer.id)
		End If
		Me.ExCost = 10F
		Me.SuperMeterMax = 50F
		Me.CalculateHealthMax()
		Dim playerStats As PlayersStatsBossesHub = Level.GetPlayerStats(MyBase.basePlayer.id)
		If Level.IsInBossesHub AndAlso playerStats IsNot Nothing Then
			Me.Health = playerStats.HP
			Me.SuperMeter = playerStats.SuperCharge
			Me.HealerHP = playerStats.healerHP
			Me.HealerHPReceived = playerStats.healerHPReceived
			Me.HealerHPCounter = playerStats.healerHPCounter
		Else
			Me.Health = Me.HealthMax
			Me.SuperMeter = 0F
		End If
		If Me.Health >= OnlineAchievementData.DLC.Triggers.HP9Trigger Then
			OnlineManager.Instance.[Interface].UnlockAchievement(MyBase.basePlayer.id, OnlineAchievementData.DLC.HP9)
		End If
		Me.UpdateHealerStats()
		If Me.isChalice Then
			If Me.Loadout.super = Super.level_super_beam Then
				Me.Loadout.super = Super.level_super_chalice_vert_beam
			ElseIf Me.Loadout.super = Super.level_super_invincible Then
				Me.Loadout.super = Super.level_super_chalice_shield
			ElseIf Me.Loadout.super = Super.level_super_ghost Then
				Me.Loadout.super = Super.level_super_chalice_iii
			End If
		ElseIf Me.Loadout.super = Super.level_super_chalice_vert_beam Then
			Me.Loadout.super = Super.level_super_beam
		ElseIf Me.Loadout.super = Super.level_super_chalice_shield Then
			Me.Loadout.super = Super.level_super_invincible
		ElseIf Me.Loadout.super = Super.level_super_chalice_iii Then
			Me.Loadout.super = Super.level_super_ghost
		End If
	End Sub

	' Token: 0x060042C9 RID: 17097 RVA: 0x0023E758 File Offset: 0x0023CB58
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		If Level.Current IsNot Nothing Then
			RemoveHandler Level.Current.OnWinEvent, AddressOf Me.OnWin
			RemoveHandler Level.Current.OnLoseEvent, AddressOf Me.OnLose
		End If
		If Me.Loadout IsNot Nothing Then
			If Me.Loadout.super = Super.level_super_chalice_vert_beam Then
				Me.Loadout.super = Super.level_super_beam
			ElseIf Me.Loadout.super = Super.level_super_chalice_shield Then
				Me.Loadout.super = Super.level_super_invincible
			ElseIf Me.Loadout.super = Super.level_super_chalice_iii Then
				Me.Loadout.super = Super.level_super_ghost
			End If
		End If
	End Sub

	' Token: 0x060042CA RID: 17098 RVA: 0x0023E82C File Offset: 0x0023CC2C
	Public Sub UpdateHealerStats()
		If(Me.Loadout.charm = Charm.charm_healer OrElse (Me.Loadout.charm = Charm.charm_curse AndAlso Me.CurseCharmLevel >= 0)) AndAlso Not Level.IsChessBoss Then
			Dim playerStats As PlayersStatsBossesHub = Level.GetPlayerStats(MyBase.basePlayer.id)
			If playerStats IsNot Nothing Then
				Me.HealthMax += playerStats.healerHP
			End If
		End If
	End Sub

	' Token: 0x060042CB RID: 17099 RVA: 0x0023E8A3 File Offset: 0x0023CCA3
	Private Function DjimmiInUse() As Boolean
		Return PlayerData.Data.DjimmiActivatedCurrentRegion() AndAlso Level.Current.AllowDjimmi() AndAlso Level.Current.mode <> Level.Mode.Hard
	End Function

	' Token: 0x060042CC RID: 17100 RVA: 0x0023E8D8 File Offset: 0x0023CCD8
	Private Sub CalculateHealthMax()
		Me.HealthMax = 3
		If Me.Loadout.charm = Charm.charm_health_up_1 AndAlso Not Level.IsChessBoss Then
			Me.HealthMax += WeaponProperties.CharmHealthUpOne.healthIncrease
		ElseIf Me.Loadout.charm = Charm.charm_health_up_2 AndAlso Not Level.IsChessBoss Then
			Me.HealthMax += WeaponProperties.CharmHealthUpTwo.healthIncrease
		ElseIf Me.Loadout.charm = Charm.charm_healer AndAlso Not Level.IsChessBoss Then
			Me.HealthMax += Me.HealerHP
		ElseIf Me.Loadout.charm = Charm.charm_curse AndAlso Me.CurseCharmLevel >= 0 AndAlso Not Level.IsChessBoss Then
			Me.HealthMax += Me.HealerHP
			Me.HealthMax += CharmCurse.GetHealthModifier(Me.CurseCharmLevel)
		ElseIf Me.isChalice Then
			Me.HealthMax += 1
		End If
		If Me.DjimmiInUse() Then
			Me.HealthMax *= 2
		End If
		If Level.IsInBossesHub Then
			Dim playerStats As PlayersStatsBossesHub = Level.GetPlayerStats(MyBase.basePlayer.id)
			If playerStats IsNot Nothing Then
				Me.HealthMax += playerStats.BonusHP
			End If
		End If
		If Me.HealthMax > 9 Then
			Me.HealthMax = 9
		End If
	End Sub

	' Token: 0x060042CD RID: 17101 RVA: 0x0023EA65 File Offset: 0x0023CE65
	Private Sub OnWin()
		PlayerStatsManager.GlobalInvincibility = True
	End Sub

	' Token: 0x060042CE RID: 17102 RVA: 0x0023EA6D File Offset: 0x0023CE6D
	Private Sub OnLose()
		PlayerStatsManager.GlobalInvincibility = True
	End Sub

	' Token: 0x060042CF RID: 17103 RVA: 0x0023EA78 File Offset: 0x0023CE78
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		If Me.SuperInvincible Then
			Return
		End If
		If Me.ChaliceShieldOn Then
			Return
		End If
		If Me.Loadout.charm = Charm.charm_pit_saver AndAlso Not Level.IsChessBoss Then
			If info.damageSource = DamageDealer.DamageSource.Pit Then
				Return
			End If
			Me.SuperMeter += WeaponProperties.CharmPitSaver.meterAmount
			Me.OnSuperChanged(True)
		End If
		If info.stoneTime > 0F Then
			Me.GetStoned(info.stoneTime)
		End If
		If info.damage > 0F Then
			Me.TakeDamage()
		End If
	End Sub

	' Token: 0x060042D0 RID: 17104 RVA: 0x0023EB14 File Offset: 0x0023CF14
	Public Sub GetStoned(time As Single)
		If time > 0F AndAlso Me.StoneTime <= 0F AndAlso Me.timeSinceStoned > 1F Then
			Me.StoneTime = time
			Me.timeSinceStoned = 0F
			Me.OnStoned()
		End If
	End Sub

	' Token: 0x060042D1 RID: 17105 RVA: 0x0023EB6C File Offset: 0x0023CF6C
	Public Sub ReverseControls(reverseTime As Single)
		If Me.timeSinceReversed > 0F AndAlso Me.ReverseTime <= 0F AndAlso Me.timeSinceReversed > 1F Then
			Me.ReverseTime = reverseTime
			Me.timeSinceReversed = 0F
		End If
	End Sub

	' Token: 0x060042D2 RID: 17106 RVA: 0x0023EBBC File Offset: 0x0023CFBC
	Private Sub TakeDamage()
		If Me.SuperInvincible Then
			Return
		End If
		If Me.hardInvincibility Then
			Return
		End If
		If Level.Current.Ending Then
			Return
		End If
		If Me.State <> PlayerStatsManager.PlayerState.Ready AndAlso (Not Me.isChalice OrElse Me.Loadout.super <> Super.level_super_ghost) Then
			Return
		End If
		If Me.StoneTime > 0F Then
			Me.StoneTime = 0F
		End If
		If PlayerStatsManager.GlobalInvincibility OrElse PlayerStatsManager.DebugInvincible Then
			Return
		End If
		Me.Health -= 1
		Dim playerStats As PlayersStatsBossesHub = Level.GetPlayerStats(MyBase.basePlayer.id)
		If Level.IsInBossesHub AndAlso playerStats IsNot Nothing Then
			If playerStats.BonusHP > 0 Then
				playerStats.LoseBonusHP()
			ElseIf playerStats.healerHP > 0 Then
				playerStats.LoseHealerHP()
			End If
			Me.CalculateHealthMax()
		End If
		Me.OnHealthChanged()
		If Me.Health < 3 Then
			Level.ScoringData.numTimesHit += 1
		End If
		Vibrator.Vibrate(1F, 0.2F, MyBase.basePlayer.id)
		If Me.Health <= 0 Then
			Me.OnStatsDeath()
		Else
			MyBase.StartCoroutine(Me.hit_cr())
		End If
	End Sub

	' Token: 0x060042D3 RID: 17107 RVA: 0x0023ED0D File Offset: 0x0023D10D
	Public Sub OnPitKnockUp()
		MyBase.basePlayer.damageReceiver.TakeDamage(New DamageDealer.DamageInfo(1F, DamageDealer.Direction.Neutral, MyBase.transform.position, DamageDealer.DamageSource.Pit))
	End Sub

	' Token: 0x060042D4 RID: 17108 RVA: 0x0023ED3B File Offset: 0x0023D13B
	Public Sub OnDealDamage(damage As Single, dealer As DamageDealer)
		If Me.CanGainSuperMeter Then
			Me.SuperMeter += 0.0625F * damage / dealer.DamageMultiplier
			Me.OnSuperChanged(False)
		End If
	End Sub

	' Token: 0x060042D5 RID: 17109 RVA: 0x0023ED6C File Offset: 0x0023D16C
	Public Sub OnParry(Optional multiplier As Single = 1F, Optional countParryTowardsScore As Boolean = True)
		If(Me.Loadout.charm = Charm.charm_healer OrElse (Me.Loadout.charm = Charm.charm_curse AndAlso Me.CurseCharmLevel >= 0)) AndAlso Not Level.IsChessBoss Then
			If Me.HealerHPReceived < 3 Then
				Me.HealerCharm()
			Else
				Me.SuperChangedFromParry(multiplier)
			End If
		Else
			Me.SuperChangedFromParry(multiplier)
		End If
		If countParryTowardsScore AndAlso Not Level.Current.Ending Then
			Level.ScoringData.numParries += 1
		End If
		OnlineManager.Instance.[Interface].IncrementStat(MyBase.basePlayer.id, "Parries", 1)
		If Level.Current.CurrentLevel <> Levels.Tutorial AndAlso Level.Current.CurrentLevel <> Levels.ShmupTutorial AndAlso (Level.Current.playerMode = PlayerMode.Level OrElse Level.Current.playerMode = PlayerMode.Arcade) Then
			Me.ParriesThisJump += 1
			If Me.ParriesThisJump > PlayerData.Data.GetNumParriesInRow(MyBase.basePlayer.id) Then
				PlayerData.Data.SetNumParriesInRow(MyBase.basePlayer.id, Me.ParriesThisJump)
			End If
			If Me.ParriesThisJump = 5 Then
				OnlineManager.Instance.[Interface].UnlockAchievement(MyBase.basePlayer.id, "ParryChain")
			End If
		End If
		If Me.SuperMeter = Me.SuperMeterMax Then
			AudioManager.Play("player_parry_power_up_full")
		Else
			AudioManager.Play("player_parry_power_up")
		End If
	End Sub

	' Token: 0x060042D6 RID: 17110 RVA: 0x0023EF0A File Offset: 0x0023D30A
	Private Sub SuperChangedFromParry(multiplier As Single)
		If Me.CanGainSuperMeter Then
			Me.SuperMeter += 10F * multiplier
			Me.OnSuperChanged(True)
		End If
	End Sub

	' Token: 0x060042D7 RID: 17111 RVA: 0x0023EF32 File Offset: 0x0023D332
	Public Function NextParryActivatesHealerCharm() As Boolean
		Return Not Level.IsChessBoss AndAlso Me.Loadout.charm = Charm.charm_healer AndAlso Me.HealerHPReceived = Me.HealerHPCounter
	End Function

	' Token: 0x060042D8 RID: 17112 RVA: 0x0023EF68 File Offset: 0x0023D368
	Private Sub HealerCharm()
		Dim num As Integer = Me.HealerHPReceived + 1
		If Me.Loadout.charm = Charm.charm_curse Then
			num = CharmCurse.GetHealerInterval(Me.CurseCharmLevel, Me.HealerHPReceived)
		End If
		Me.HealerHPCounter += 1
		If Me.HealerHPCounter >= num Then
			Me.HealerHP += 1
			Me.HealerHPReceived += 1
			Me.SetHealth(Me.Health + 1)
			Me.OnHealthChanged()
			Me.HealerHPCounter = 0
			Dim levelPlayerController As LevelPlayerController = TryCast(MyBase.basePlayer, LevelPlayerController)
			Dim planePlayerController As PlanePlayerController = TryCast(MyBase.basePlayer, PlanePlayerController)
			If levelPlayerController IsNot Nothing Then
				levelPlayerController.animationController.OnHealerCharm()
			ElseIf planePlayerController IsNot Nothing Then
				planePlayerController.animationController.OnHealerCharm()
			End If
		End If
	End Sub

	' Token: 0x060042D9 RID: 17113 RVA: 0x0023F041 File Offset: 0x0023D441
	Public Sub ParryOneQuarter()
		Me.OnParry(0.25F, True)
	End Sub

	' Token: 0x060042DA RID: 17114 RVA: 0x0023F04F File Offset: 0x0023D44F
	Public Sub ResetJumpParries()
		Me.ParriesThisJump = 0
	End Sub

	' Token: 0x170005F0 RID: 1520
	' (get) Token: 0x060042DB RID: 17115 RVA: 0x0023F058 File Offset: 0x0023D458
	Public ReadOnly Property PartnerCanSteal As Boolean
		Get
			Return Me.Health > 1
		End Get
	End Property

	' Token: 0x060042DC RID: 17116 RVA: 0x0023F064 File Offset: 0x0023D464
	Public Sub OnPartnerStealHealth()
		If Not Me.PartnerCanSteal Then
			Return
		End If
		Me.Health -= 1
		Dim playerStats As PlayersStatsBossesHub = Level.GetPlayerStats(MyBase.basePlayer.id)
		If Level.IsInBossesHub AndAlso playerStats IsNot Nothing Then
			playerStats.LoseBonusHP()
			Me.CalculateHealthMax()
		End If
		Me.OnHealthChanged()
	End Sub

	' Token: 0x060042DD RID: 17117 RVA: 0x0023F0BE File Offset: 0x0023D4BE
	Public Sub OnSuper()
		If Me.Loadout.super <> Super.level_super_invincible OrElse Level.Current.playerMode <> PlayerMode.Level Then
			Me.SuperMeter = 0F
			Me.OnSuperChanged(True)
		End If
		Me.State = PlayerStatsManager.PlayerState.Super
	End Sub

	' Token: 0x060042DE RID: 17118 RVA: 0x0023F0FD File Offset: 0x0023D4FD
	Public Sub OnSuperEnd()
		If Me.Loadout.super = Super.level_super_invincible AndAlso Level.Current.playerMode = PlayerMode.Level Then
			MyBase.StartCoroutine(Me.emptySuper_cr())
		End If
		Me.State = PlayerStatsManager.PlayerState.Ready
	End Sub

	' Token: 0x060042DF RID: 17119 RVA: 0x0023F137 File Offset: 0x0023D537
	Public Sub OnEx()
		If Me.Loadout.charm = Charm.charm_EX Then
			Return
		End If
		Me.SuperMeter -= 10F
		Me.OnSuperChanged(True)
	End Sub

	' Token: 0x060042E0 RID: 17120 RVA: 0x0023F168 File Offset: 0x0023D568
	Private Sub OnWeaponChange(weapon As Weapon)
		If Me.OnWeaponChangedEvent IsNot Nothing Then
			Me.OnWeaponChangedEvent(weapon)
		End If
	End Sub

	' Token: 0x060042E1 RID: 17121 RVA: 0x0023F184 File Offset: 0x0023D584
	Private Sub OnHealthChanged()
		Me.Health = Mathf.Clamp(Me.Health, 0, Me.HealthMax)
		If Me.OnHealthChangedEvent IsNot Nothing Then
			Me.OnHealthChangedEvent(Me.Health, MyBase.basePlayer.id)
		End If
	End Sub

	' Token: 0x060042E2 RID: 17122 RVA: 0x0023F1D0 File Offset: 0x0023D5D0
	Private Sub OnSuperChanged(Optional playEffect As Boolean = True)
		Me.SuperMeter = Mathf.Clamp(Me.SuperMeter, 0F, Me.SuperMeterMax)
		If Me.OnSuperChangedEvent IsNot Nothing Then
			Me.OnSuperChangedEvent(Me.SuperMeter, MyBase.basePlayer.id, playEffect)
		End If
	End Sub

	' Token: 0x060042E3 RID: 17123 RVA: 0x0023F224 File Offset: 0x0023D624
	Private Sub OnStatsDeath()
		AudioManager.Play("player_die")
		MyBase.StartCoroutine(Me.death_sound_cr())
		If Me.OnPlayerDeathEvent IsNot Nothing Then
			Me.OnPlayerDeathEvent(MyBase.basePlayer.id)
		End If
		EventManager.Instance.Raise(PlayerEvent(Of PlayerStatsManager.DeathEvent).[Shared](MyBase.basePlayer.id))
		Me.Deaths += 1
		PlayerData.Data.Die(MyBase.basePlayer.id)
	End Sub

	' Token: 0x060042E4 RID: 17124 RVA: 0x0023F2A6 File Offset: 0x0023D6A6
	Public Sub OnPreRevive()
		If Not Level.IsTowerOfPowerMain OrElse TowerOfPowerLevelGameInfo.CURRENT_TURN > 0 Then
			Me.Health = 1
		End If
	End Sub

	' Token: 0x060042E5 RID: 17125 RVA: 0x0023F2C4 File Offset: 0x0023D6C4
	Public Sub OnRevive()
		Me.OnHealthChanged()
		If Me.OnPlayerReviveEvent IsNot Nothing Then
			Me.OnPlayerReviveEvent(MyBase.basePlayer.id)
		End If
		EventManager.Instance.Raise(PlayerEvent(Of PlayerStatsManager.ReviveEvent).[Shared](MyBase.basePlayer.id))
	End Sub

	' Token: 0x060042E6 RID: 17126 RVA: 0x0023F312 File Offset: 0x0023D712
	Public Sub SetHealth(health As Integer)
		Me.Health = health
		Me.CalculateHealthMax()
		Me.OnHealthChanged()
		If health >= OnlineAchievementData.DLC.Triggers.HP9Trigger Then
			OnlineManager.Instance.[Interface].UnlockAchievement(MyBase.basePlayer.id, OnlineAchievementData.DLC.HP9)
		End If
	End Sub

	' Token: 0x060042E7 RID: 17127 RVA: 0x0023F351 File Offset: 0x0023D751
	Public Sub SetInvincible(superInvincible As Boolean)
		Me.SuperInvincible = superInvincible
	End Sub

	' Token: 0x060042E8 RID: 17128 RVA: 0x0023F35A File Offset: 0x0023D75A
	Public Sub SetChaliceShield(chaliceShield As Boolean)
		Me.ChaliceShieldOn = chaliceShield
	End Sub

	' Token: 0x060042E9 RID: 17129 RVA: 0x0023F363 File Offset: 0x0023D763
	Public Sub AddEx()
		If Me.CanGainSuperMeter Then
			Me.SuperMeter += 10F
			Me.OnSuperChanged(True)
		End If
	End Sub

	' Token: 0x060042EA RID: 17130 RVA: 0x0023F38C File Offset: 0x0023D78C
	Private Sub onDashStartEventHandler()
		If Me.Loadout.charm = Charm.charm_curse AndAlso Me.CurseCharmLevel > -1 Then
			Me.curseCharmDashCounter += 1
			If Me.curseCharmDashCounter > CharmCurse.GetSmokeDashInterval(Me.CurseCharmLevel) Then
				Me.curseCharmDashCounter = 0
			End If
		End If
	End Sub

	' Token: 0x060042EB RID: 17131 RVA: 0x0023F3E8 File Offset: 0x0023D7E8
	Private Sub onShrinkEventHandler()
		If Me.Loadout.charm = Charm.charm_curse AndAlso Me.CurseCharmLevel > -1 Then
			Me.curseCharmDashCounter += 1
			If Me.curseCharmDashCounter > CharmCurse.GetSmokeDashInterval(Me.CurseCharmLevel) Then
				Me.curseCharmDashCounter = 0
			End If
		End If
	End Sub

	' Token: 0x060042EC RID: 17132 RVA: 0x0023F444 File Offset: 0x0023D844
	Private Sub onParryEventHandler()
		If Me.Loadout.charm = Charm.charm_curse AndAlso Me.CurseCharmLevel > -1 Then
			Me.curseCharmWhetstoneCounter += 1
			If Me.curseCharmWhetstoneCounter > CharmCurse.GetWhetstoneInterval(Me.CurseCharmLevel) Then
				Me.curseCharmWhetstoneCounter = 0
			End If
		End If
	End Sub

	' Token: 0x060042ED RID: 17133 RVA: 0x0023F4A0 File Offset: 0x0023D8A0
	Private Sub UpdateStone()
		Dim planePlayerController As PlanePlayerController = TryCast(MyBase.basePlayer, PlanePlayerController)
		If planePlayerController IsNot Nothing Then
			Me.currentMoveDir = planePlayerController.motor.MoveDirection
		End If
		Me.lastMoveDir = Me.currentMoveDir
		Me.timeSinceStoned += CupheadTime.FixedDelta
		If Me.StoneTime <= 0F Then
			Return
		End If
		If(Me.lastMoveDir <> Me.currentMoveDir AndAlso (Me.currentMoveDir.x <> 0 OrElse Me.currentMoveDir.y <> 0)) OrElse MyBase.basePlayer.input.actions.GetAnyButtonDown() Then
			Me.StoneTime -= CupheadTime.Delta
			Me.StoneTime -= 0.1F
			Me.OnStoneShake()
		End If
	End Sub

	' Token: 0x060042EE RID: 17134 RVA: 0x0023F594 File Offset: 0x0023D994
	Private Sub UpdateReverse()
		Dim levelPlayerController As LevelPlayerController = TryCast(MyBase.basePlayer, LevelPlayerController)
		If levelPlayerController Is Nothing Then
			Return
		End If
		Me.timeSinceReversed += CupheadTime.FixedDelta
		If Me.ReverseTime <= 0F Then
			Return
		End If
		Me.ReverseTime -= CupheadTime.Delta
	End Sub

	' Token: 0x060042EF RID: 17135 RVA: 0x0023F5F4 File Offset: 0x0023D9F4
	Public Overrides Sub StopAllCoroutines()
	End Sub

	' Token: 0x060042F0 RID: 17136 RVA: 0x0023F5F8 File Offset: 0x0023D9F8
	Private Iterator Function death_sound_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 0.5F)
		AudioManager.Play("player_die_vinylscratch")
		Return
	End Function

	' Token: 0x060042F1 RID: 17137 RVA: 0x0023F614 File Offset: 0x0023DA14
	Private Iterator Function hit_cr() As IEnumerator
		Me.hardInvincibility = True
		For i As Integer = 0 To 10 - 1
			Yield Nothing
		Next
		Me.hardInvincibility = False
		Return
	End Function

	' Token: 0x060042F2 RID: 17138 RVA: 0x0023F630 File Offset: 0x0023DA30
	Private Iterator Function charmSuperBuilder_cr() As IEnumerator
		While Me.Loadout Is Nothing
			Yield Nothing
		End While
		If(Me.Loadout.charm <> Charm.charm_super_builder AndAlso (Me.Loadout.charm <> Charm.charm_curse OrElse Me.CurseCharmLevel <= -1)) OrElse Level.IsChessBoss Then
			Return
		End If
		Dim delay As Single = 0F
		Dim amount As Single = 0F
		If Me.Loadout.charm = Charm.charm_super_builder Then
			delay = WeaponProperties.CharmSuperBuilder.delay
			amount = WeaponProperties.CharmSuperBuilder.amount
		ElseIf Me.Loadout.charm = Charm.charm_curse AndAlso Me.CurseCharmLevel > -1 Then
			delay = WeaponProperties.CharmCurse.superMeterDelay
			amount = CharmCurse.GetSuperMeterAmount(Me.CurseCharmLevel)
		End If
		While True
			Yield CupheadTime.WaitForSeconds(Me, delay)
			If Me.CanGainSuperMeter Then
				Me.SuperMeter += amount
				Me.OnSuperChanged(False)
			End If
		End While
	End Function

	' Token: 0x060042F3 RID: 17139 RVA: 0x0023F64C File Offset: 0x0023DA4C
	Private Iterator Function emptySuper_cr() As IEnumerator
		While Me.SuperMeter > 0F
			Me.SuperMeter -= Me.SuperMeterMax * CupheadTime.Delta / WeaponProperties.LevelSuperInvincibility.durationFX
			Me.OnSuperChanged(True)
			Yield Nothing
		End While
		Me.SuperMeter = 0F
		Me.OnSuperChanged(True)
		Return
	End Function

	' Token: 0x170005F1 RID: 1521
	' (get) Token: 0x060042F4 RID: 17140 RVA: 0x0023F667 File Offset: 0x0023DA67
	' (set) Token: 0x060042F5 RID: 17141 RVA: 0x0023F66E File Offset: 0x0023DA6E
	Public Shared Property DebugInvincible As Boolean

	' Token: 0x060042F6 RID: 17142 RVA: 0x0023F676 File Offset: 0x0023DA76
	Public Sub DebugAddSuper()
		Me.AddEx()
	End Sub

	' Token: 0x060042F7 RID: 17143 RVA: 0x0023F67E File Offset: 0x0023DA7E
	Public Sub DebugFillSuper()
		Me.SuperMeter = 50F
		Me.OnSuperChanged(True)
	End Sub

	' Token: 0x060042F8 RID: 17144 RVA: 0x0023F694 File Offset: 0x0023DA94
	Public Shared Sub DebugToggleInvincible()
		PlayerStatsManager.DebugInvincible = Not PlayerStatsManager.DebugInvincible
		Dim text As String = If((Not PlayerStatsManager.DebugInvincible), "red", "green")
	End Sub

	' Token: 0x040048F4 RID: 18676
	Public Const HEALTH_MAX As Integer = 3

	' Token: 0x040048F5 RID: 18677
	Public Const HEALTH_TRUE_MAX As Integer = 9

	' Token: 0x040048F6 RID: 18678
	Private Const TIME_HIT As Single = 2F

	' Token: 0x040048F7 RID: 18679
	Private Const TIME_REVIVED As Single = 3F

	' Token: 0x040048F8 RID: 18680
	Private Const SUPER_MAX As Integer = 50

	' Token: 0x040048F9 RID: 18681
	Private Const SUPER_ON_PARRY As Single = 10F

	' Token: 0x040048FA RID: 18682
	Private Const SUPER_ON_DEAL_DAMAGE As Single = 0.0625F

	' Token: 0x040048FB RID: 18683
	Private Const EX_COST As Single = 10F

	' Token: 0x040048FC RID: 18684
	Private Const HEALER_HP_MAX As Integer = 3

	' Token: 0x04004903 RID: 18691
	Public isChalice As Boolean

	' Token: 0x04004904 RID: 18692
	Private _curseCharmLevel As Integer

	' Token: 0x04004905 RID: 18693
	Private curseCharmDashCounter As Integer

	' Token: 0x04004906 RID: 18694
	Private curseCharmWhetstoneCounter As Integer

	' Token: 0x04004910 RID: 18704
	Private timeSinceStoned As Single = 1000F

	' Token: 0x04004911 RID: 18705
	Private timeSinceReversed As Single = 1000F

	' Token: 0x04004912 RID: 18706
	Private hardInvincibility As Boolean

	' Token: 0x0400491D RID: 18717
	Private superBuilderRoutine As IEnumerator

	' Token: 0x0400491E RID: 18718
	Private Const STONE_REDUCTION As Single = 0.1F

	' Token: 0x0400491F RID: 18719
	Private lastMoveDir As Trilean2

	' Token: 0x04004920 RID: 18720
	Private currentMoveDir As Trilean2

	' Token: 0x02000AD4 RID: 2772
	Public Class DeathEvent
		Inherits PlayerEvent(Of PlayerStatsManager.DeathEvent)

	End Class

	' Token: 0x02000AD5 RID: 2773
	Public Class ReviveEvent
		Inherits PlayerEvent(Of PlayerStatsManager.ReviveEvent)

	End Class

	' Token: 0x02000AD6 RID: 2774
	Public Enum PlayerState
		' Token: 0x04004923 RID: 18723
		Ready
		' Token: 0x04004924 RID: 18724
		Super
	End Enum

	' Token: 0x02000AD7 RID: 2775
	' (Invoke) Token: 0x060042FC RID: 17148
	Public Delegate Sub OnPlayerHealthChangeHandler(health As Integer, playerId As PlayerId)

	' Token: 0x02000AD8 RID: 2776
	' (Invoke) Token: 0x06004300 RID: 17152
	Public Delegate Sub OnPlayerSuperChangedHandler(super As Single, playerId As PlayerId, playEffect As Boolean)

	' Token: 0x02000AD9 RID: 2777
	' (Invoke) Token: 0x06004304 RID: 17156
	Public Delegate Sub OnPlayerWeaponChangedHandler(weapon As Weapon)

	' Token: 0x02000ADA RID: 2778
	' (Invoke) Token: 0x06004308 RID: 17160
	Public Delegate Sub OnPlayerDeathHandler(playerId As PlayerId)

	' Token: 0x02000ADB RID: 2779
	' (Invoke) Token: 0x0600430C RID: 17164
	Public Delegate Sub OnStoneHandler()
End Class
