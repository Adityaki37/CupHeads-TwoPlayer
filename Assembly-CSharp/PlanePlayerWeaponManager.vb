Imports System
Imports System.Collections
Imports System.Diagnostics
Imports UnityEngine

' Token: 0x02000AA3 RID: 2723
Public Class PlanePlayerWeaponManager
	Inherits AbstractPlanePlayerComponent

	' Token: 0x170005B6 RID: 1462
	' (get) Token: 0x0600414C RID: 16716 RVA: 0x002369A2 File Offset: 0x00234DA2
	' (set) Token: 0x0600414D RID: 16717 RVA: 0x002369AA File Offset: 0x00234DAA
	Public Property state As PlanePlayerWeaponManager.State

	' Token: 0x170005B7 RID: 1463
	' (get) Token: 0x0600414E RID: 16718 RVA: 0x002369B3 File Offset: 0x00234DB3
	Public ReadOnly Property CurrentWeapon As AbstractPlaneWeapon
		Get
			Return Me.weapons.GetWeapon(Me.currentWeapon)
		End Get
	End Property

	' Token: 0x170005B8 RID: 1464
	' (get) Token: 0x0600414F RID: 16719 RVA: 0x002369C6 File Offset: 0x00234DC6
	' (set) Token: 0x06004150 RID: 16720 RVA: 0x002369CE File Offset: 0x00234DCE
	Public Property states As PlanePlayerWeaponManager.States

	' Token: 0x170005B9 RID: 1465
	' (get) Token: 0x06004151 RID: 16721 RVA: 0x002369D7 File Offset: 0x00234DD7
	' (set) Token: 0x06004152 RID: 16722 RVA: 0x002369DF File Offset: 0x00234DDF
	Public Property CanInterupt As Boolean

	' Token: 0x06004153 RID: 16723 RVA: 0x002369E8 File Offset: 0x00234DE8
	Private Sub Start()
		Me.weapons.Init(Me)
		Me.states = New PlanePlayerWeaponManager.States()
		AddHandler MyBase.player.animationController.OnExFireAnimEvent, AddressOf Me.OnExAnimFire
		AddHandler MyBase.player.OnReviveEvent, AddressOf Me.OnRevive
		AddHandler MyBase.player.stats.OnPlayerDeathEvent, AddressOf Me.StopSound
		Me.CanInterupt = True
	End Sub

	' Token: 0x06004154 RID: 16724 RVA: 0x00236A62 File Offset: 0x00234E62
	Private Sub FixedUpdate()
		If Me.state = PlanePlayerWeaponManager.State.Inactive OrElse Me.state = PlanePlayerWeaponManager.State.Busy Then
			Return
		End If
		Me.CheckBasic()
		Me.CheckEx()
		Me.HandleWeaponSwitch()
	End Sub

	' Token: 0x06004155 RID: 16725 RVA: 0x00236A90 File Offset: 0x00234E90
	Public Overrides Sub OnLevelStart()
		MyBase.OnLevelStart()
		If MyBase.player.stats.isChalice Then
			Me.currentWeapon = Weapon.plane_chalice_weapon_3way
		ElseIf MyBase.player.stats.Loadout.charm = Charm.charm_curse AndAlso MyBase.player.stats.CurseCharmLevel >= 0 Then
			Dim availableShmupWeaponIDs As Integer() = WeaponProperties.CharmCurse.availableShmupWeaponIDs
			Me.currentWeapon = CType(availableShmupWeaponIDs(Global.UnityEngine.Random.Range(0, availableShmupWeaponIDs.Length)), Weapon)
		End If
		If MyBase.player.stats.StoneTime > 0F Then
			Return
		End If
		Me.state = PlanePlayerWeaponManager.State.Ready
		If MyBase.player.input.actions.GetButton(3) Then
			Me.StartBasic()
		End If
	End Sub

	' Token: 0x06004156 RID: 16726 RVA: 0x00236B57 File Offset: 0x00234F57
	Public Overrides Sub OnLevelEnd()
		MyBase.OnLevelEnd()
		Me.EndBasic()
	End Sub

	' Token: 0x06004157 RID: 16727 RVA: 0x00236B68 File Offset: 0x00234F68
	Public Sub SwitchToWeapon(weapon As Weapon)
		If weapon = Weapon.None Then
			Return
		End If
		Me.weapons.GetWeapon(Me.currentWeapon).EndBasic()
		Me.weapons.GetWeapon(Me.currentWeapon).EndEx()
		If Me.OnWeaponChangeEvent IsNot Nothing Then
			Me.OnWeaponChangeEvent(weapon)
		End If
		Me.currentWeapon = weapon
		If MyBase.player.input.actions.GetButton(3) Then
			Me.StartBasic()
		End If
	End Sub

	' Token: 0x140000A4 RID: 164
	' (add) Token: 0x06004158 RID: 16728 RVA: 0x00236BEC File Offset: 0x00234FEC
	' (remove) Token: 0x06004159 RID: 16729 RVA: 0x00236C24 File Offset: 0x00235024
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnWeaponChangeEvent As PlanePlayerWeaponManager.OnWeaponChangeHandler

	' Token: 0x140000A5 RID: 165
	' (add) Token: 0x0600415A RID: 16730 RVA: 0x00236C5C File Offset: 0x0023505C
	' (remove) Token: 0x0600415B RID: 16731 RVA: 0x00236C94 File Offset: 0x00235094
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnExStartEvent As Action

	' Token: 0x140000A6 RID: 166
	' (add) Token: 0x0600415C RID: 16732 RVA: 0x00236CCC File Offset: 0x002350CC
	' (remove) Token: 0x0600415D RID: 16733 RVA: 0x00236D04 File Offset: 0x00235104
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnExFireEvent As Action

	' Token: 0x140000A7 RID: 167
	' (add) Token: 0x0600415E RID: 16734 RVA: 0x00236D3C File Offset: 0x0023513C
	' (remove) Token: 0x0600415F RID: 16735 RVA: 0x00236D74 File Offset: 0x00235174
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnSuperStartEvent As Action

	' Token: 0x140000A8 RID: 168
	' (add) Token: 0x06004160 RID: 16736 RVA: 0x00236DAC File Offset: 0x002351AC
	' (remove) Token: 0x06004161 RID: 16737 RVA: 0x00236DE4 File Offset: 0x002351E4
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnSuperCountdownEvent As Action

	' Token: 0x140000A9 RID: 169
	' (add) Token: 0x06004162 RID: 16738 RVA: 0x00236E1C File Offset: 0x0023521C
	' (remove) Token: 0x06004163 RID: 16739 RVA: 0x00236E54 File Offset: 0x00235254
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnSuperFireEvent As Action

	' Token: 0x06004164 RID: 16740 RVA: 0x00236E8A File Offset: 0x0023528A
	Private Sub OnRevive(pos As Vector3)
		Me.IsShooting = False
		Me.state = PlanePlayerWeaponManager.State.Ready
		Me.states.basic = PlanePlayerWeaponManager.States.Basic.Ready
		Me.states.ex = PlanePlayerWeaponManager.States.Ex.Ready
		Me.CanInterupt = True
	End Sub

	' Token: 0x06004165 RID: 16741 RVA: 0x00236EB9 File Offset: 0x002352B9
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me.weapons.OnDestroy()
		Me.super = Nothing
	End Sub

	' Token: 0x06004166 RID: 16742 RVA: 0x00236ED4 File Offset: 0x002352D4
	Private Sub CheckBasic()
		If MyBase.player.stats.Loadout.charm = Charm.charm_EX Then
			Return
		End If
		If(MyBase.player.input.actions.GetButtonDown(3) OrElse (MyBase.player.input.actions.GetButtonTimePressed(3) > 0F AndAlso Not Me.IsShooting)) AndAlso MyBase.player.stats.StoneTime <= 0F Then
			If MyBase.player.stats.Loadout.charm = Charm.charm_curse AndAlso MyBase.player.stats.CurseCharmLevel >= 0 AndAlso Not MyBase.player.Shrunk Then
				Dim availableShmupWeaponIDs As Integer() = WeaponProperties.CharmCurse.availableShmupWeaponIDs
				Dim num As Integer
				num = CInt(Me.currentWeapon)
				While num = CInt(Me.currentWeapon)
					num = availableShmupWeaponIDs(Global.UnityEngine.Random.Range(0, availableShmupWeaponIDs.Length))
				End While
				Me.SwitchWeapon(CType(num, Weapon))
			Else
				Me.StartBasic()
			End If
		ElseIf MyBase.player.input.actions.GetButtonUp(3) OrElse (Me.IsShooting AndAlso MyBase.player.stats.StoneTime > 0F) Then
			Me.EndBasic()
		ElseIf Not MyBase.player.input.actions.GetButton(3) AndAlso Me.IsShooting Then
			Me.EndBasic()
		ElseIf(Not MyBase.player.Shrunk AndAlso Me.unshrunkWeapon <> Weapon.None) OrElse (Me.IsShooting AndAlso MyBase.player.Shrunk AndAlso Me.currentWeapon <> Weapon.plane_weapon_peashot) Then
			Me.EndBasic()
			If MyBase.player.input.actions.GetButton(3) Then
				Me.StartBasic()
			End If
		End If
	End Sub

	' Token: 0x06004167 RID: 16743 RVA: 0x002370D8 File Offset: 0x002354D8
	Private Sub StartBasic()
		If(Me.currentWeapon = Weapon.plane_weapon_bomb OrElse Me.currentWeapon = Weapon.plane_chalice_weapon_3way OrElse Me.currentWeapon = Weapon.plane_chalice_weapon_bomb) AndAlso MyBase.player.Shrunk Then
			Me.unshrunkWeapon = Me.currentWeapon
			Me.currentWeapon = Weapon.plane_weapon_peashot
		End If
		Me.weapons.GetWeapon(Me.currentWeapon).BeginBasic()
	End Sub

	' Token: 0x06004168 RID: 16744 RVA: 0x00237154 File Offset: 0x00235554
	Private Sub EndBasic()
		Me.weapons.GetWeapon(Me.currentWeapon).EndBasic()
		If Me.unshrunkWeapon <> Weapon.None Then
			Me.currentWeapon = Me.unshrunkWeapon
			Me.unshrunkWeapon = Weapon.None
		End If
		Me.StopSound(MyBase.player.id)
	End Sub

	' Token: 0x06004169 RID: 16745 RVA: 0x002371B0 File Offset: 0x002355B0
	Private Sub StopSound(id As PlayerId)
		If(id = PlayerId.PlayerOne AndAlso Not PlayerManager.player1IsMugman) OrElse (id = PlayerId.PlayerTwo AndAlso PlayerManager.player1IsMugman) Then
			If AudioManager.CheckIfPlaying("player_plane_weapon_fire_loop_cuphead") Then
				AudioManager.[Stop]("player_plane_weapon_fire_loop_cuphead")
				AudioManager.Play("player_plane_weapon_fire_loop_end_cuphead")
				Me.emitAudioFromObject.Add("player_plane_weapon_fire_loop_end_cuphead")
			End If
		ElseIf AudioManager.CheckIfPlaying("player_plane_weapon_fire_loop_mugman") Then
			AudioManager.[Stop]("player_plane_weapon_fire_loop_mugman")
			AudioManager.Play("player_plane_weapon_fire_loop_end_mugman")
			Me.emitAudioFromObject.Add("player_plane_weapon_fire_loop_end_mugman")
		End If
	End Sub

	' Token: 0x0600416A RID: 16746 RVA: 0x0023724C File Offset: 0x0023564C
	Private Sub CheckEx()
		If Not MyBase.player.stats.CanUseEx OrElse MyBase.player.Parrying OrElse MyBase.player.Shrunk OrElse MyBase.player.stats.StoneTime > 0F Then
			Return
		End If
		If MyBase.player.input.actions.GetButtonDown(4) OrElse MyBase.player.motor.HasBufferedInput(PlanePlayerMotor.BufferedInput.Super) OrElse (MyBase.player.stats.Loadout.charm = Charm.charm_EX AndAlso MyBase.player.input.actions.GetButton(3)) Then
			If MyBase.player.stats.SuperMeter >= MyBase.player.stats.SuperMeterMax AndAlso MyBase.player.stats.Loadout.charm <> Charm.charm_EX Then
				Me.StartSuper()
			Else
				Me.StartEx()
			End If
			MyBase.player.motor.ClearBufferedInput()
		End If
	End Sub

	' Token: 0x0600416B RID: 16747 RVA: 0x00237379 File Offset: 0x00235779
	Private Sub StartEx()
		MyBase.StartCoroutine(Me.ex_cr())
	End Sub

	' Token: 0x0600416C RID: 16748 RVA: 0x00237388 File Offset: 0x00235788
	Private Sub OnExAnimFire()
		Me.states.ex = PlanePlayerWeaponManager.States.Ex.Fire
	End Sub

	' Token: 0x0600416D RID: 16749 RVA: 0x00237398 File Offset: 0x00235798
	Private Iterator Function ex_cr() As IEnumerator
		AudioManager.Play("player_plane_weapon_special_fire")
		Me.state = PlanePlayerWeaponManager.State.Inactive
		Me.states.ex = PlanePlayerWeaponManager.States.Ex.Intro
		Me.CanInterupt = False
		Me.EndBasic()
		MyBase.player.stats.OnEx()
		If Me.OnExStartEvent IsNot Nothing Then
			Me.OnExStartEvent()
		End If
		While Me.states.ex <> PlanePlayerWeaponManager.States.Ex.Fire
			If MyBase.player.stats.StoneTime > 0F Then
				Me.CancelEX()
				Yield Nothing
			End If
			Yield Nothing
		End While
		Me.weapons.GetWeapon(Me.currentWeapon).BeginEx()
		If Me.OnExFireEvent IsNot Nothing Then
			Me.OnExFireEvent()
		End If
		AudioManager.Play("player_plane_up_ex_end")
		Me.states.ex = PlanePlayerWeaponManager.States.Ex.Ending
		While MyBase.animator.GetCurrentAnimatorStateInfo(0).IsName("Ex_End")
			If MyBase.player.stats.StoneTime > 0F Then
				Me.CancelEX()
				Yield Nothing
			End If
			Yield Nothing
		End While
		Me.state = PlanePlayerWeaponManager.State.Ready
		Me.states.ex = PlanePlayerWeaponManager.States.Ex.Ready
		If MyBase.player.input.actions.GetButtonDown(3) Then
			Me.StartBasic()
		End If
		Me.CanInterupt = True
		Return
	End Function

	' Token: 0x0600416E RID: 16750 RVA: 0x002373B3 File Offset: 0x002357B3
	Private Sub CancelEX()
		MyBase.StopCoroutine(Me.ex_cr())
		If MyBase.player.input.actions.GetButtonDown(3) Then
			Me.StartBasic()
		End If
		Me.CanInterupt = True
	End Sub

	' Token: 0x0600416F RID: 16751 RVA: 0x002373E9 File Offset: 0x002357E9
	Public Sub StartSuper()
		MyBase.StartCoroutine(Me.super_cr())
	End Sub

	' Token: 0x06004170 RID: 16752 RVA: 0x002373F8 File Offset: 0x002357F8
	Private Iterator Function super_cr() As IEnumerator
		Me.state = PlanePlayerWeaponManager.State.Inactive
		Me.states.super = PlanePlayerWeaponManager.States.Super.Ready
		Me.CanInterupt = False
		Me.EndBasic()
		MyBase.player.stats.OnSuper()
		Dim s As AbstractPlaneSuper
		If MyBase.player.stats.isChalice Then
			s = Me.chaliceSuper.Create(MyBase.player)
		Else
			s = Me.super.Create(MyBase.player)
		End If
		If Me.OnSuperStartEvent IsNot Nothing Then
			Me.OnSuperStartEvent()
		End If
		While Me.states.super <> PlanePlayerWeaponManager.States.Super.Ending AndAlso Me.states.super <> PlanePlayerWeaponManager.States.Super.Countdown
			Me.states.super = s.State
			Yield Nothing
		End While
		If Me.OnSuperCountdownEvent IsNot Nothing Then
			Me.OnSuperCountdownEvent()
		End If
		While Me.states.super <> PlanePlayerWeaponManager.States.Super.Ending
			Me.states.super = s.State
			Yield Nothing
		End While
		If Me.OnSuperFireEvent IsNot Nothing Then
			Me.OnSuperFireEvent()
		End If
		MyBase.player.stats.OnSuperEnd()
		Me.state = PlanePlayerWeaponManager.State.Ready
		Me.states.super = PlanePlayerWeaponManager.States.Super.Ready
		If MyBase.player.input.actions.GetButtonDown(3) Then
			Me.StartBasic()
		End If
		Me.CanInterupt = True
		Return
	End Function

	' Token: 0x06004171 RID: 16753 RVA: 0x00237413 File Offset: 0x00235813
	Public Function GetBulletPosition() As Vector2
		Return Me.bulletRoot.position
	End Function

	' Token: 0x06004172 RID: 16754 RVA: 0x00237428 File Offset: 0x00235828
	Private Sub HandleWeaponSwitch()
		If MyBase.player.input.actions.GetButtonDown(5) Then
			If MyBase.player.stats.Loadout.charm = Charm.charm_curse AndAlso MyBase.player.stats.CurseCharmLevel >= 0 Then
				Return
			End If
			If Not PlayerData.Data.IsUnlocked(MyBase.player.id, Weapon.plane_weapon_bomb) AndAlso Not Level.IsTowerOfPower Then
				Return
			End If
			If MyBase.player.stats.isChalice Then
				If Me.currentWeapon = Weapon.plane_chalice_weapon_3way Then
					Me.SwitchWeapon(Weapon.plane_chalice_weapon_bomb)
				Else
					Me.SwitchWeapon(Weapon.plane_chalice_weapon_3way)
				End If
			ElseIf Me.currentWeapon = Weapon.plane_weapon_peashot Then
				Me.SwitchWeapon(Weapon.plane_weapon_bomb)
			Else
				Me.SwitchWeapon(Weapon.plane_weapon_peashot)
			End If
		End If
	End Sub

	' Token: 0x06004173 RID: 16755 RVA: 0x00237520 File Offset: 0x00235920
	Private Sub SwitchWeapon(weapon As Weapon)
		Me.EndBasic()
		Me.currentWeapon = weapon
		If Me.OnWeaponChangeEvent IsNot Nothing Then
			Me.OnWeaponChangeEvent(weapon)
		End If
		If MyBase.player.input.actions.GetButton(3) Then
			Me.StartBasic()
		End If
	End Sub

	' Token: 0x040047E1 RID: 18401
	<SerializeField()>
	Private weapons As PlanePlayerWeaponManager.Weapons

	' Token: 0x040047E2 RID: 18402
	<SerializeField()>
	Private super As AbstractPlaneSuper

	' Token: 0x040047E3 RID: 18403
	<SerializeField()>
	Private chaliceSuper As AbstractPlaneSuper

	' Token: 0x040047E4 RID: 18404
	<Space(10F)>
	<SerializeField()>
	Private bulletRoot As Transform

	' Token: 0x040047E5 RID: 18405
	<NonSerialized()>
	Public IsShooting As Boolean

	' Token: 0x040047E6 RID: 18406
	Private currentWeapon As Weapon = Weapon.plane_weapon_peashot

	' Token: 0x040047E9 RID: 18409
	Private unshrunkWeapon As Weapon = Weapon.None

	' Token: 0x02000AA4 RID: 2724
	Public Enum State
		' Token: 0x040047F1 RID: 18417
		Inactive
		' Token: 0x040047F2 RID: 18418
		Ready
		' Token: 0x040047F3 RID: 18419
		Busy
	End Enum

	' Token: 0x02000AA5 RID: 2725
	' (Invoke) Token: 0x06004175 RID: 16757
	Public Delegate Sub OnWeaponChangeHandler(weapon As Weapon)

	' Token: 0x02000AA6 RID: 2726
	<Serializable()>
	Public Class Weapons
		' Token: 0x06004179 RID: 16761 RVA: 0x0023757C File Offset: 0x0023597C
		Public Sub Init(manager As PlanePlayerWeaponManager)
			Me.peashot = Global.UnityEngine.[Object].Instantiate(Of AbstractPlaneWeapon)(Me.peashot)
			Me.peashot.Initialize(manager, 0)
			Me.peashot.transform.SetParent(manager.transform)
			Me.peashot.transform.ResetLocalTransforms()
			Me.bomb = Global.UnityEngine.[Object].Instantiate(Of AbstractPlaneWeapon)(Me.bomb)
			Me.bomb.Initialize(manager, 2)
			Me.bomb.transform.SetParent(manager.transform)
			Me.bomb.transform.ResetLocalTransforms()
			Me.chalice3Way = Global.UnityEngine.[Object].Instantiate(Of AbstractPlaneWeapon)(Me.chalice3Way)
			Me.chalice3Way.Initialize(manager, 3)
			Me.chalice3Way.transform.SetParent(manager.transform)
			Me.chalice3Way.transform.ResetLocalTransforms()
			Me.chaliceBomb = Global.UnityEngine.[Object].Instantiate(Of AbstractPlaneWeapon)(Me.chaliceBomb)
			Me.chaliceBomb.Initialize(manager, 4)
			Me.chaliceBomb.transform.SetParent(manager.transform)
			Me.chaliceBomb.transform.ResetLocalTransforms()
		End Sub

		' Token: 0x0600417A RID: 16762 RVA: 0x0023769C File Offset: 0x00235A9C
		Public Function GetWeapon(weapon As Weapon) As AbstractPlaneWeapon
			If weapon = Weapon.plane_weapon_peashot Then
				Return Me.peashot
			End If
			If weapon = Weapon.plane_weapon_bomb Then
				Return Me.bomb
			End If
			If weapon = Weapon.plane_chalice_weapon_3way Then
				Return Me.chalice3Way
			End If
			If weapon <> Weapon.plane_chalice_weapon_bomb Then
				Return Nothing
			End If
			Return Me.chaliceBomb
		End Function

		' Token: 0x0600417B RID: 16763 RVA: 0x002376F7 File Offset: 0x00235AF7
		Public Sub OnDestroy()
			Me.peashot = Nothing
			Me.bomb = Nothing
		End Sub

		' Token: 0x040047F4 RID: 18420
		Public peashot As AbstractPlaneWeapon

		' Token: 0x040047F5 RID: 18421
		Public bomb As AbstractPlaneWeapon

		' Token: 0x040047F6 RID: 18422
		Public chalice3Way As AbstractPlaneWeapon

		' Token: 0x040047F7 RID: 18423
		Public chaliceBomb As AbstractPlaneWeapon
	End Class

	' Token: 0x02000AA7 RID: 2727
	Public Class States
		' Token: 0x0600417C RID: 16764 RVA: 0x00237707 File Offset: 0x00235B07
		Public Sub New()
			Me.basic = PlanePlayerWeaponManager.States.Basic.Ready
			Me.ex = PlanePlayerWeaponManager.States.Ex.Ready
			Me.super = PlanePlayerWeaponManager.States.Super.Ready
		End Sub

		' Token: 0x170005BA RID: 1466
		' (get) Token: 0x0600417D RID: 16765 RVA: 0x00237724 File Offset: 0x00235B24
		' (set) Token: 0x0600417E RID: 16766 RVA: 0x0023772C File Offset: 0x00235B2C
		Public Property basic As PlanePlayerWeaponManager.States.Basic

		' Token: 0x170005BB RID: 1467
		' (get) Token: 0x0600417F RID: 16767 RVA: 0x00237735 File Offset: 0x00235B35
		' (set) Token: 0x06004180 RID: 16768 RVA: 0x0023773D File Offset: 0x00235B3D
		Public Property ex As PlanePlayerWeaponManager.States.Ex

		' Token: 0x040047FA RID: 18426
		Public super As PlanePlayerWeaponManager.States.Super

		' Token: 0x02000AA8 RID: 2728
		Public Enum Basic
			' Token: 0x040047FC RID: 18428
			Ready
		End Enum

		' Token: 0x02000AA9 RID: 2729
		Public Enum Ex
			' Token: 0x040047FE RID: 18430
			Ready
			' Token: 0x040047FF RID: 18431
			Intro
			' Token: 0x04004800 RID: 18432
			Fire
			' Token: 0x04004801 RID: 18433
			Ending
		End Enum

		' Token: 0x02000AAA RID: 2730
		Public Enum Super
			' Token: 0x04004803 RID: 18435
			Ready
			' Token: 0x04004804 RID: 18436
			Intro
			' Token: 0x04004805 RID: 18437
			Countdown
			' Token: 0x04004806 RID: 18438
			Ending
		End Enum
	End Class
End Class
