Imports System
Imports System.Collections.Generic
Imports System.Diagnostics
Imports UnityEngine

' Token: 0x02000A39 RID: 2617
Public Class LevelPlayerWeaponManager
	Inherits AbstractLevelPlayerComponent

	' Token: 0x17000564 RID: 1380
	' (get) Token: 0x06003E49 RID: 15945 RVA: 0x00224055 File Offset: 0x00222455
	' (set) Token: 0x06003E4A RID: 15946 RVA: 0x0022405D File Offset: 0x0022245D
	Public Property IsShooting As Boolean

	' Token: 0x17000565 RID: 1381
	' (get) Token: 0x06003E4B RID: 15947 RVA: 0x00224066 File Offset: 0x00222466
	' (set) Token: 0x06003E4C RID: 15948 RVA: 0x0022406E File Offset: 0x0022246E
	Public Property FreezePosition As Boolean

	' Token: 0x17000566 RID: 1382
	' (get) Token: 0x06003E4D RID: 15949 RVA: 0x00224077 File Offset: 0x00222477
	Public ReadOnly Property ExPosition As Vector2
		Get
			Return Me.exRoot.position
		End Get
	End Property

	' Token: 0x17000567 RID: 1383
	' (get) Token: 0x06003E4E RID: 15950 RVA: 0x00224089 File Offset: 0x00222489
	Public ReadOnly Property CurrentWeapon As AbstractLevelWeapon
		Get
			Return Me.weaponPrefabs.GetWeapon(Me.currentWeapon)
		End Get
	End Property

	' Token: 0x14000095 RID: 149
	' (add) Token: 0x06003E4F RID: 15951 RVA: 0x0022409C File Offset: 0x0022249C
	' (remove) Token: 0x06003E50 RID: 15952 RVA: 0x002240D4 File Offset: 0x002224D4
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnWeaponChangeEvent As LevelPlayerWeaponManager.OnWeaponChangeHandler

	' Token: 0x14000096 RID: 150
	' (add) Token: 0x06003E51 RID: 15953 RVA: 0x0022410C File Offset: 0x0022250C
	' (remove) Token: 0x06003E52 RID: 15954 RVA: 0x00224144 File Offset: 0x00222544
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnBasicStart As Action

	' Token: 0x14000097 RID: 151
	' (add) Token: 0x06003E53 RID: 15955 RVA: 0x0022417C File Offset: 0x0022257C
	' (remove) Token: 0x06003E54 RID: 15956 RVA: 0x002241B4 File Offset: 0x002225B4
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnExStart As Action

	' Token: 0x14000098 RID: 152
	' (add) Token: 0x06003E55 RID: 15957 RVA: 0x002241EC File Offset: 0x002225EC
	' (remove) Token: 0x06003E56 RID: 15958 RVA: 0x00224224 File Offset: 0x00222624
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnSuperStart As Action

	' Token: 0x14000099 RID: 153
	' (add) Token: 0x06003E57 RID: 15959 RVA: 0x0022425C File Offset: 0x0022265C
	' (remove) Token: 0x06003E58 RID: 15960 RVA: 0x00224294 File Offset: 0x00222694
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnExFire As Action

	' Token: 0x1400009A RID: 154
	' (add) Token: 0x06003E59 RID: 15961 RVA: 0x002242CC File Offset: 0x002226CC
	' (remove) Token: 0x06003E5A RID: 15962 RVA: 0x00224304 File Offset: 0x00222704
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnWeaponFire As Action

	' Token: 0x1400009B RID: 155
	' (add) Token: 0x06003E5B RID: 15963 RVA: 0x0022433C File Offset: 0x0022273C
	' (remove) Token: 0x06003E5C RID: 15964 RVA: 0x00224374 File Offset: 0x00222774
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnExEnd As Action

	' Token: 0x1400009C RID: 156
	' (add) Token: 0x06003E5D RID: 15965 RVA: 0x002243AC File Offset: 0x002227AC
	' (remove) Token: 0x06003E5E RID: 15966 RVA: 0x002243E4 File Offset: 0x002227E4
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnSuperEnd As Action

	' Token: 0x1400009D RID: 157
	' (add) Token: 0x06003E5F RID: 15967 RVA: 0x0022441C File Offset: 0x0022281C
	' (remove) Token: 0x06003E60 RID: 15968 RVA: 0x00224454 File Offset: 0x00222854
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnSuperInterrupt As Action

	' Token: 0x17000568 RID: 1384
	' (get) Token: 0x06003E61 RID: 15969 RVA: 0x0022448A File Offset: 0x0022288A
	' (set) Token: 0x06003E62 RID: 15970 RVA: 0x00224492 File Offset: 0x00222892
	Public Property activeSuper As AbstractPlayerSuper

	' Token: 0x06003E63 RID: 15971 RVA: 0x0022449C File Offset: 0x0022289C
	Protected Overrides Sub OnAwake()
		MyBase.OnAwake()
		AddHandler MyBase.basePlayer.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
		AddHandler Level.Current.OnLevelEndEvent, AddressOf Me.OnLevelEnd
		AddHandler MyBase.player.motor.OnDashStartEvent, AddressOf Me.OnDash
		Me.basic = New LevelPlayerWeaponManager.WeaponState()
		Me.ex = New LevelPlayerWeaponManager.ExState()
		Me.weaponsRoot = New GameObject("Weapons").transform
		Me.weaponsRoot.parent = MyBase.transform
		Me.weaponsRoot.localPosition = Vector3.zero
		Me.weaponsRoot.localEulerAngles = Vector3.zero
		Me.weaponsRoot.localScale = Vector3.one
		Me.aim = New GameObject("Aim").transform
		Me.aim.SetParent(MyBase.transform)
		Me.aim.ResetLocalTransforms()
	End Sub

	' Token: 0x06003E64 RID: 15972 RVA: 0x0022459C File Offset: 0x0022299C
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		If Level.Current IsNot Nothing Then
			RemoveHandler Level.Current.OnLevelEndEvent, AddressOf Me.OnLevelEnd
		End If
		If MyBase.player IsNot Nothing AndAlso MyBase.player.motor IsNot Nothing Then
			RemoveHandler MyBase.player.motor.OnDashStartEvent, AddressOf Me.OnDash
		End If
		Me.weaponPrefabs.OnDestroy()
		Me.superPrefabs.OnDestroy()
		Me.exDustEffect = Nothing
		Me.exChargeEffect = Nothing
		Me.WORKAROUND_NullifyFields()
	End Sub

	' Token: 0x06003E65 RID: 15973 RVA: 0x00224644 File Offset: 0x00222A44
	Private Sub FixedUpdate()
		If Not MyBase.player.levelStarted OrElse Not Me.allowInput Then
			Return
		End If
		Me.HandleWeaponSwitch()
		Me.HandleWeaponFiring()
		If MyBase.player.motor.Grounded Then
			Me.ex.airAble = True
		End If
	End Sub

	' Token: 0x06003E66 RID: 15974 RVA: 0x0022469A File Offset: 0x00222A9A
	Private Sub OnEnable()
		Me.EnableInput()
	End Sub

	' Token: 0x06003E67 RID: 15975 RVA: 0x002246A2 File Offset: 0x00222AA2
	Public Sub ForceStopWeaponFiring()
		Me.EndBasic()
	End Sub

	' Token: 0x06003E68 RID: 15976 RVA: 0x002246AA File Offset: 0x00222AAA
	Public Overrides Sub OnLevelEnd()
		Me.EndBasic()
		MyBase.OnLevelEnd()
	End Sub

	' Token: 0x06003E69 RID: 15977 RVA: 0x002246B8 File Offset: 0x00222AB8
	Private Sub OnDash()
		Me.EndBasic()
	End Sub

	' Token: 0x06003E6A RID: 15978 RVA: 0x002246C0 File Offset: 0x00222AC0
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		If Me.ex.firing AndAlso Not MyBase.player.stats.SuperInvincible Then
			Me.ex.firing = False
		End If
	End Sub

	' Token: 0x06003E6B RID: 15979 RVA: 0x002246F3 File Offset: 0x00222AF3
	Public Sub AbortEX()
		Me.ex.firing = False
	End Sub

	' Token: 0x06003E6C RID: 15980 RVA: 0x00224701 File Offset: 0x00222B01
	Public Sub ParrySuccess()
	End Sub

	' Token: 0x06003E6D RID: 15981 RVA: 0x00224704 File Offset: 0x00222B04
	Public Sub LevelInit(id As PlayerId)
		Dim playerLoadout As PlayerData.PlayerLoadouts.PlayerLoadout = PlayerData.Data.Loadouts.GetPlayerLoadout(MyBase.player.id)
		If playerLoadout.charm = Charm.charm_curse AndAlso MyBase.player.stats.CurseCharmLevel > -1 Then
			Dim availableWeaponIDs As Integer() = WeaponProperties.CharmCurse.availableWeaponIDs
			Me.currentWeapon = CType(availableWeaponIDs(Global.UnityEngine.Random.Range(0, availableWeaponIDs.Length)), Weapon)
		Else
			Me.currentWeapon = playerLoadout.primaryWeapon
		End If
		Me.weaponPrefabs.Init(Me, Me.weaponsRoot)
		Me.superPrefabs.Init(MyBase.player)
	End Sub

	' Token: 0x06003E6E RID: 15982 RVA: 0x0022479D File Offset: 0x00222B9D
	Public Sub OnDeath()
		Me.EndBasic()
	End Sub

	' Token: 0x06003E6F RID: 15983 RVA: 0x002247A5 File Offset: 0x00222BA5
	Public Sub EnableInput()
		Me.allowInput = True
	End Sub

	' Token: 0x06003E70 RID: 15984 RVA: 0x002247AE File Offset: 0x00222BAE
	Public Sub DisableInput()
		Me.allowInput = False
		Me.IsShooting = False
	End Sub

	' Token: 0x06003E71 RID: 15985 RVA: 0x002247BE File Offset: 0x00222BBE
	Public Sub EnableSuper(value As Boolean)
		Me.allowSuper = value
	End Sub

	' Token: 0x06003E72 RID: 15986 RVA: 0x002247C7 File Offset: 0x00222BC7
	Private Sub _WeaponFireEx()
		Me.FireEx()
	End Sub

	' Token: 0x06003E73 RID: 15987 RVA: 0x002247CF File Offset: 0x00222BCF
	Private Sub _WeaponEndEx()
		Me.EndEx()
	End Sub

	' Token: 0x06003E74 RID: 15988 RVA: 0x002247D7 File Offset: 0x00222BD7
	Private Sub StartBasic()
		Me.UpdateAim()
		If Not Level.IsChessBoss Then
			Me.weaponPrefabs.GetWeapon(Me.currentWeapon).BeginBasic()
		End If
		If Me.OnBasicStart IsNot Nothing Then
			Me.OnBasicStart()
		End If
	End Sub

	' Token: 0x06003E75 RID: 15989 RVA: 0x00224815 File Offset: 0x00222C15
	Private Sub EndBasic()
		If Me.currentWeapon = Weapon.None Then
			Return
		End If
		Me.weaponPrefabs.GetWeapon(Me.currentWeapon).EndBasic()
		Me.basic.firing = False
	End Sub

	' Token: 0x06003E76 RID: 15990 RVA: 0x0022484A File Offset: 0x00222C4A
	Public Sub TriggerWeaponFire()
		Me.OnWeaponFire()
	End Sub

	' Token: 0x06003E77 RID: 15991 RVA: 0x00224857 File Offset: 0x00222C57
	Public Sub InterruptSuper()
		If Me.OnSuperInterrupt IsNot Nothing Then
			Me.OnSuperInterrupt()
		End If
	End Sub

	' Token: 0x06003E78 RID: 15992 RVA: 0x00224870 File Offset: 0x00222C70
	Private Sub StartEx()
		Me.EndBasic()
		Me.UpdateAim()
		Me.ex.firing = True
		Me.ex.airAble = False
		MyBase.player.stats.OnEx()
		Me.exChargeEffect.Create(MyBase.player.center)
		If Me.OnExStart IsNot Nothing Then
			Me.OnExStart()
		End If
	End Sub

	' Token: 0x06003E79 RID: 15993 RVA: 0x002248DE File Offset: 0x00222CDE
	Private Sub FireEx()
		Me.weaponPrefabs.GetWeapon(Me.currentWeapon).BeginEx()
		If Me.OnExFire IsNot Nothing Then
			Me.OnExFire()
		End If
	End Sub

	' Token: 0x06003E7A RID: 15994 RVA: 0x0022490C File Offset: 0x00222D0C
	Private Sub EndEx()
		Me.ex.firing = False
		If Me.OnExEnd IsNot Nothing Then
			Me.OnExEnd()
		End If
	End Sub

	' Token: 0x06003E7B RID: 15995 RVA: 0x00224930 File Offset: 0x00222D30
	Public Sub CreateExDust(starsEffect As Effect)
		Dim transform As Transform = New GameObject("ExRootTemp").transform
		transform.ResetLocalTransforms()
		transform.position = Me.exRoot.position
		Dim vector As Vector2 = transform.position
		If starsEffect IsNot Nothing Then
			Dim transform2 As Transform = starsEffect.Create(vector).transform
			transform2.SetParent(transform)
			transform2.ResetLocalTransforms()
			transform2.SetParent(Nothing)
			transform2.SetEulerAngles(New Single?(0F), New Single?(0F), New Single?(Me.GetBulletRotation()))
			transform2.localScale = Me.GetBulletScale()
			transform2.AddPositionForward2D(-100F)
		End If
		If Me.exDustEffect IsNot Nothing Then
			Dim transform3 As Transform = Me.exDustEffect.Create(vector).transform
			transform3.SetParent(transform)
			transform3.ResetLocalTransforms()
			transform3.SetParent(Nothing)
			transform3.SetEulerAngles(New Single?(0F), New Single?(0F), New Single?(Me.GetBulletRotation()))
			transform3.localScale = Me.GetBulletScale()
			transform3.AddPositionForward2D(-15F)
		End If
		Global.UnityEngine.[Object].Destroy(transform.gameObject)
	End Sub

	' Token: 0x06003E7C RID: 15996 RVA: 0x00224A64 File Offset: 0x00222E64
	Private Sub StartSuper()
		Me.EndBasic()
		Me.UpdateAim()
		MyBase.player.stats.OnSuper()
		Dim super As Super = PlayerData.Data.Loadouts.GetPlayerLoadout(MyBase.player.id).super
		If MyBase.player.stats.isChalice Then
			If super <> Super.level_super_beam Then
				If super <> Super.level_super_ghost Then
					If super = Super.level_super_invincible Then
						super = Super.level_super_chalice_shield
					End If
				Else
					super = Super.level_super_chalice_iii
				End If
			Else
				super = Super.level_super_chalice_vert_beam
			End If
		End If
		Dim abstractPlayerSuper As AbstractPlayerSuper = Me.superPrefabs.GetPrefab(super).Create(MyBase.player)
		AddHandler abstractPlayerSuper.OnEndedEvent, AddressOf Me.EndSuperFromSuper
		Me.activeSuper = abstractPlayerSuper
		If Me.OnSuperStart IsNot Nothing Then
			Me.OnSuperStart()
		End If
	End Sub

	' Token: 0x06003E7D RID: 15997 RVA: 0x00224B50 File Offset: 0x00222F50
	Private Sub EndSuper()
		If Me.OnSuperEnd IsNot Nothing Then
			Me.OnSuperEnd()
		End If
	End Sub

	' Token: 0x06003E7E RID: 15998 RVA: 0x00224B68 File Offset: 0x00222F68
	Public Sub EndSuperFromSuper()
		Me.EndSuper()
	End Sub

	' Token: 0x06003E7F RID: 15999 RVA: 0x00224B70 File Offset: 0x00222F70
	Private Sub HandleWeaponFiring()
		If MyBase.player.motor.Dashing OrElse MyBase.player.motor.IsHit Then
			Return
		End If
		If MyBase.player.input.actions.GetButtonDown(4) OrElse MyBase.player.motor.HasBufferedInput(LevelPlayerMotor.BufferedInput.Super) OrElse (MyBase.player.stats.Loadout.charm = Charm.charm_EX AndAlso MyBase.player.input.actions.GetButton(3) AndAlso Not Me.ex.firing) Then
			MyBase.player.motor.ClearBufferedInput()
			Dim super As Super = PlayerData.Data.Loadouts.GetPlayerLoadout(MyBase.player.id).super
			If MyBase.player.stats.SuperMeter >= MyBase.player.stats.SuperMeterMax AndAlso super <> Super.None AndAlso Not MyBase.player.stats.ChaliceShieldOn AndAlso Me.allowSuper AndAlso MyBase.player.stats.Loadout.charm <> Charm.charm_EX Then
				Me.StartSuper()
				Return
			End If
			If MyBase.player.stats.CanUseEx AndAlso Me.ex.Able Then
				Me.StartEx()
				Return
			End If
		End If
		If Me.ex.firing OrElse MyBase.player.stats.Loadout.charm = Charm.charm_EX Then
			Return
		End If
		If Me.basic.firing <> MyBase.player.input.actions.GetButton(3) Then
			If MyBase.player.input.actions.GetButton(3) Then
				If PlayerData.Data.Loadouts.GetPlayerLoadout(MyBase.player.id).charm = Charm.charm_curse AndAlso MyBase.player.stats.CurseCharmLevel > -1 Then
					Dim availableWeaponIDs As Integer() = WeaponProperties.CharmCurse.availableWeaponIDs
					Dim num As Integer
					num = CInt(Me.currentWeapon)
					While num = CInt(Me.currentWeapon)
						num = availableWeaponIDs(Global.UnityEngine.Random.Range(0, availableWeaponIDs.Length))
					End While
					Me.SwitchWeapon(CType(num, Weapon))
				Else
					Me.StartBasic()
				End If
			Else
				Me.EndBasic()
			End If
		End If
		Me.basic.firing = MyBase.player.input.actions.GetButton(3)
	End Sub

	' Token: 0x06003E80 RID: 16000 RVA: 0x00224E0D File Offset: 0x0022320D
	Public Sub ResetEx()
		Me.ex.firing = False
	End Sub

	' Token: 0x06003E81 RID: 16001 RVA: 0x00224E1C File Offset: 0x0022321C
	Private Sub HandleWeaponSwitch()
		If MyBase.player.input.actions.GetButtonDown(5) Then
			Dim playerLoadout As PlayerData.PlayerLoadouts.PlayerLoadout = PlayerData.Data.Loadouts.GetPlayerLoadout(MyBase.player.id)
			If(playerLoadout.charm = Charm.charm_curse AndAlso MyBase.player.stats.CurseCharmLevel > -1) OrElse playerLoadout.secondaryWeapon = Weapon.None Then
				Return
			End If
			If Me.currentWeapon = playerLoadout.primaryWeapon Then
				Me.SwitchWeapon(playerLoadout.secondaryWeapon)
			Else
				Me.SwitchWeapon(playerLoadout.primaryWeapon)
			End If
		End If
	End Sub

	' Token: 0x06003E82 RID: 16002 RVA: 0x00224EC4 File Offset: 0x002232C4
	Private Sub SwitchWeapon(weapon As Weapon)
		If weapon = Weapon.None Then
			Return
		End If
		Me.weaponPrefabs.GetWeapon(Me.currentWeapon).EndBasic()
		Me.weaponPrefabs.GetWeapon(Me.currentWeapon).EndEx()
		Me.currentWeapon = weapon
		If Me.OnWeaponChangeEvent IsNot Nothing Then
			Me.OnWeaponChangeEvent(weapon)
		End If
		If MyBase.player.input.actions.GetButton(3) Then
			Me.StartBasic()
		End If
	End Sub

	' Token: 0x06003E83 RID: 16003 RVA: 0x00224F48 File Offset: 0x00223348
	Private Function GetCurrentPose() As LevelPlayerWeaponManager.Pose
		If Me.ex.firing Then
			Return LevelPlayerWeaponManager.Pose.Ex
		End If
		If MyBase.player.motor.Ducking Then
			Return LevelPlayerWeaponManager.Pose.Duck
		End If
		If Not MyBase.player.motor.Grounded Then
			Return LevelPlayerWeaponManager.Pose.Jump
		End If
		If MyBase.player.motor.Locked Then
			If MyBase.player.motor.LookDirection.y > 0 Then
				If MyBase.player.motor.LookDirection.x <> 0 Then
					Return LevelPlayerWeaponManager.Pose.Up_D
				End If
				Return LevelPlayerWeaponManager.Pose.Up
			ElseIf MyBase.player.motor.LookDirection.y < 0 Then
				If MyBase.player.motor.LookDirection.x <> 0 Then
					Return LevelPlayerWeaponManager.Pose.Down_D
				End If
				Return LevelPlayerWeaponManager.Pose.Down
			End If
		ElseIf MyBase.player.motor.LookDirection.x <> 0 Then
			If MyBase.player.motor.LookDirection.y > 0 Then
				Return LevelPlayerWeaponManager.Pose.Up_D_R
			End If
			Return LevelPlayerWeaponManager.Pose.Forward_R
		Else
			If MyBase.player.motor.LookDirection.y < 0 Then
				Return LevelPlayerWeaponManager.Pose.Duck
			End If
			If MyBase.player.motor.LookDirection.y > 0 Then
				Return LevelPlayerWeaponManager.Pose.Up
			End If
		End If
		Return LevelPlayerWeaponManager.Pose.Forward
	End Function

	' Token: 0x06003E84 RID: 16004 RVA: 0x002250DC File Offset: 0x002234DC
	Public Function GetDirectionPose() As LevelPlayerWeaponManager.Pose
		If MyBase.player.motor.Dashing Then
			Return LevelPlayerWeaponManager.Pose.Forward
		End If
		If MyBase.player.motor.LookDirection.y > 0 Then
			If MyBase.player.motor.LookDirection.x <> 0 Then
				Return LevelPlayerWeaponManager.Pose.Up_D
			End If
			Return LevelPlayerWeaponManager.Pose.Up
		Else
			If MyBase.player.motor.LookDirection.y >= 0 Then
				Return LevelPlayerWeaponManager.Pose.Forward
			End If
			If MyBase.player.motor.LookDirection.x <> 0 Then
				Return LevelPlayerWeaponManager.Pose.Down_D
			End If
			Return LevelPlayerWeaponManager.Pose.Down
		End If
	End Function

	' Token: 0x06003E85 RID: 16005 RVA: 0x00225194 File Offset: 0x00223594
	Public Sub UpdateAim()
		Dim directionPose As LevelPlayerWeaponManager.Pose = Me.GetDirectionPose()
		Dim num As Single
		If MyBase.transform.localScale.x > 0F Then
			Select Case directionPose
				Case Else
					num = 0F
				Case LevelPlayerWeaponManager.Pose.Up
					num = 90F
				Case LevelPlayerWeaponManager.Pose.Up_D
					num = 45F
				Case LevelPlayerWeaponManager.Pose.Down
					num = -90F
				Case LevelPlayerWeaponManager.Pose.Down_D
					num = -45F
			End Select
		Else
			Select Case directionPose
				Case Else
					num = 180F
				Case LevelPlayerWeaponManager.Pose.Up
					num = 90F
				Case LevelPlayerWeaponManager.Pose.Up_D
					num = 135F
				Case LevelPlayerWeaponManager.Pose.Down
					num = -90F
				Case LevelPlayerWeaponManager.Pose.Down_D
					num = -135F
			End Select
		End If
		num *= MyBase.player.motor.GravityReversalMultiplier
		Me.aim.SetEulerAngles(New Single?(0F), New Single?(0F), New Single?(num))
	End Sub

	' Token: 0x06003E86 RID: 16006 RVA: 0x002252C4 File Offset: 0x002236C4
	Public Function GetBulletPosition() As Vector2
		Dim vector As Vector2 = MyBase.transform.position
		Dim vector2 As Vector2 = LevelPlayerWeaponManager.ProjectilePosition.[Get](Me.GetCurrentPose(), Me.GetDirectionPose(), MyBase.player.stats.isChalice)
		Return New Vector2(vector.x + vector2.x * MyBase.player.motor.TrueLookDirection.x, vector.y + vector2.y * MyBase.player.motor.GravityReversalMultiplier)
	End Function

	' Token: 0x06003E87 RID: 16007 RVA: 0x00225358 File Offset: 0x00223758
	Public Function GetBulletRotation() As Single
		Dim pose As LevelPlayerWeaponManager.Pose = Me.GetCurrentPose()
		If pose <> LevelPlayerWeaponManager.Pose.Duck Then
			Return Me.aim.eulerAngles.z
		End If
		If MyBase.transform.localScale.x < 0F Then
			Return 180F
		End If
		Return 0F
	End Function

	' Token: 0x06003E88 RID: 16008 RVA: 0x002253B0 File Offset: 0x002237B0
	Public Function GetBulletScale() As Vector3
		Return New Vector3(1F, MyBase.player.motor.TrueLookDirection.x, 1F)
	End Function

	' Token: 0x06003E89 RID: 16009 RVA: 0x002253EC File Offset: 0x002237EC
	Private Sub WORKAROUND_NullifyFields()
		Me.activeSuper = Nothing
		Me.weaponPrefabs = Nothing
		Me.superPrefabs = Nothing
		Me.exDustEffect = Nothing
		Me.exChargeEffect = Nothing
		Me.exRoot = Nothing
		Me.OnWeaponChangeEvent = Nothing
		Me.OnBasicStart = Nothing
		Me.OnExStart = Nothing
		Me.OnSuperStart = Nothing
		Me.OnExFire = Nothing
		Me.OnWeaponFire = Nothing
		Me.OnExEnd = Nothing
		Me.OnSuperEnd = Nothing
		Me.OnSuperInterrupt = Nothing
		Me.basic = Nothing
		Me.ex = Nothing
		Me.weaponsRoot = Nothing
		Me.aim = Nothing
	End Sub

	' Token: 0x0400456D RID: 17773
	<SerializeField()>
	Private weaponPrefabs As LevelPlayerWeaponManager.WeaponPrefabs

	' Token: 0x0400456E RID: 17774
	<SerializeField()>
	Private superPrefabs As LevelPlayerWeaponManager.SuperPrefabs

	' Token: 0x0400456F RID: 17775
	<Space(10F)>
	<SerializeField()>
	Private exDustEffect As Effect

	' Token: 0x04004570 RID: 17776
	<SerializeField()>
	Private exChargeEffect As Effect

	' Token: 0x04004571 RID: 17777
	<SerializeField()>
	Private exRoot As Transform

	' Token: 0x04004574 RID: 17780
	Private currentWeapon As Weapon = Weapon.None

	' Token: 0x04004575 RID: 17781
	Private currentPose As LevelPlayerWeaponManager.Pose

	' Token: 0x0400457F RID: 17791
	Private basic As LevelPlayerWeaponManager.WeaponState

	' Token: 0x04004580 RID: 17792
	Private ex As LevelPlayerWeaponManager.ExState

	' Token: 0x04004581 RID: 17793
	Private weaponsRoot As Transform

	' Token: 0x04004582 RID: 17794
	Private aim As Transform

	' Token: 0x04004583 RID: 17795
	Public allowInput As Boolean = True

	' Token: 0x04004584 RID: 17796
	Private allowSuper As Boolean = True

	' Token: 0x02000A3A RID: 2618
	Public Enum Pose
		' Token: 0x04004587 RID: 17799
		Forward
		' Token: 0x04004588 RID: 17800
		Forward_R
		' Token: 0x04004589 RID: 17801
		Up
		' Token: 0x0400458A RID: 17802
		Up_D
		' Token: 0x0400458B RID: 17803
		Up_D_R
		' Token: 0x0400458C RID: 17804
		Down
		' Token: 0x0400458D RID: 17805
		Down_D
		' Token: 0x0400458E RID: 17806
		Duck
		' Token: 0x0400458F RID: 17807
		Jump
		' Token: 0x04004590 RID: 17808
		Ex
	End Enum

	' Token: 0x02000A3B RID: 2619
	' (Invoke) Token: 0x06003E8B RID: 16011
	Public Delegate Sub OnWeaponChangeHandler(weapon As Weapon)

	' Token: 0x02000A3C RID: 2620
	Public Structure ProjectilePosition
		' Token: 0x06003E8E RID: 16014 RVA: 0x00225480 File Offset: 0x00223880
		Public Shared Function [Get](pose As LevelPlayerWeaponManager.Pose, direction As LevelPlayerWeaponManager.Pose, isChalice As Boolean) As Vector2
			If pose = LevelPlayerWeaponManager.Pose.Jump Then
				Select Case direction
					Case LevelPlayerWeaponManager.Pose.Forward
						Return If((Not isChalice), New Vector2(78F, 64F), New Vector2(85F, 70F))
					Case LevelPlayerWeaponManager.Pose.Up
						Return If((Not isChalice), New Vector2(0F, 158F), New Vector2(22F, 162F))
					Case LevelPlayerWeaponManager.Pose.Up_D
						Return If((Not isChalice), New Vector2(71F, 107F), New Vector2(66F, 117F))
					Case LevelPlayerWeaponManager.Pose.Down
						Return If((Not isChalice), New Vector2(0F, -11F), New Vector2(22F, 2F))
					Case LevelPlayerWeaponManager.Pose.Down_D
						Return If((Not isChalice), New Vector2(71F, 20F), New Vector2(66F, 31F))
				End Select
				Return If((Not isChalice), New Vector2(0F, 0F), New Vector2(0F, 0F))
			End If
			Select Case pose
				Case LevelPlayerWeaponManager.Pose.Forward
					Return If((Not isChalice), New Vector2(78F, 64F), New Vector2(100F, 63F))
				Case LevelPlayerWeaponManager.Pose.Forward_R
					Return If((Not isChalice), New Vector2(70F, 46F), New Vector2(62F, 51F))
				Case LevelPlayerWeaponManager.Pose.Up
					Return If((Not isChalice), New Vector2(27F, 158F), New Vector2(32F, 162F))
				Case LevelPlayerWeaponManager.Pose.Up_D
					Return If((Not isChalice), New Vector2(71F, 107F), New Vector2(78F, 112F))
				Case LevelPlayerWeaponManager.Pose.Up_D_R
					Return If((Not isChalice), New Vector2(73F, 107F), New Vector2(66F, 108F))
				Case LevelPlayerWeaponManager.Pose.Down
					Return If((Not isChalice), New Vector2(28F, -11F), New Vector2(32F, -6F))
				Case LevelPlayerWeaponManager.Pose.Down_D
					Return If((Not isChalice), New Vector2(71F, 20F), New Vector2(78F, 17F))
				Case LevelPlayerWeaponManager.Pose.Duck
					Return If((Not isChalice), New Vector2(102F, 24F), New Vector2(103F, 33F))
				Case Else
					Return If((Not isChalice), New Vector2(0F, 54F), New Vector2(0F, 54F))
			End Select
		End Function
	End Structure

	' Token: 0x02000A3D RID: 2621
	Public Class WeaponState
		' Token: 0x04004591 RID: 17809
		Public state As LevelPlayerWeaponManager.WeaponState.State

		' Token: 0x04004592 RID: 17810
		Public firing As Boolean

		' Token: 0x04004593 RID: 17811
		Public holding As Boolean

		' Token: 0x02000A3E RID: 2622
		Public Enum State
			' Token: 0x04004595 RID: 17813
			Ready
			' Token: 0x04004596 RID: 17814
			Firing
			' Token: 0x04004597 RID: 17815
			Fired
			' Token: 0x04004598 RID: 17816
			Ended
		End Enum
	End Class

	' Token: 0x02000A3F RID: 2623
	Public Class ExState
		' Token: 0x17000569 RID: 1385
		' (get) Token: 0x06003E91 RID: 16017 RVA: 0x00225772 File Offset: 0x00223B72
		Public ReadOnly Property Able As Boolean
			Get
				Return Me.airAble AndAlso Not Me.firing
			End Get
		End Property

		' Token: 0x04004599 RID: 17817
		Public airAble As Boolean = True

		' Token: 0x0400459A RID: 17818
		Public firing As Boolean
	End Class

	' Token: 0x02000A40 RID: 2624
	<Serializable()>
	Public Class WeaponPrefabs
		' Token: 0x06003E93 RID: 16019 RVA: 0x00225794 File Offset: 0x00223B94
		Public Sub Init(weaponManager As LevelPlayerWeaponManager, root As Transform)
			Me.weaponManager = weaponManager
			Me.root = root
			Me.weapons = New Dictionary(Of Weapon, AbstractLevelWeapon)()
			For Each weapon As Weapon In EnumUtils.GetValues(Of Weapon)()
				If weapon.ToString().ToLower().Contains("level") Then
					Me.InitWeapon(weapon)
				End If
			Next
		End Sub

		' Token: 0x06003E94 RID: 16020 RVA: 0x00225805 File Offset: 0x00223C05
		Public Function GetWeapon(weapon As Weapon) As AbstractLevelWeapon
			Return Me.weapons(weapon)
		End Function

		' Token: 0x06003E95 RID: 16021 RVA: 0x00225814 File Offset: 0x00223C14
		Private Sub InitWeapon(id As Weapon)
			Dim abstractLevelWeapon As AbstractLevelWeapon
			If id <> Weapon.level_weapon_peashot Then
				If id <> Weapon.level_weapon_spreadshot Then
					If id <> Weapon.level_weapon_arc Then
						If id <> Weapon.level_weapon_homing Then
							If id <> Weapon.level_weapon_exploder Then
								If id <> Weapon.level_weapon_charge Then
									If id <> Weapon.level_weapon_boomerang Then
										If id <> Weapon.level_weapon_bouncer Then
											If id <> Weapon.level_weapon_wide_shot Then
												If id <> Weapon.level_weapon_upshot Then
													If id <> Weapon.level_weapon_crackshot Then
														If id <> Weapon.None Then
															Return
														End If
														Return
													Else
														abstractLevelWeapon = Me.crackshot
													End If
												Else
													abstractLevelWeapon = Me.upShot
												End If
											Else
												abstractLevelWeapon = Me.wideShot
											End If
										Else
											abstractLevelWeapon = Me.bouncer
										End If
									Else
										abstractLevelWeapon = Me.boomerang
									End If
								Else
									abstractLevelWeapon = Me.charge
								End If
							Else
								abstractLevelWeapon = Me.exploder
							End If
						Else
							abstractLevelWeapon = Me.homing
						End If
					Else
						abstractLevelWeapon = Me.arc
					End If
				Else
					abstractLevelWeapon = Me.spread
				End If
			Else
				abstractLevelWeapon = Me.peashot
			End If
			If abstractLevelWeapon Is Nothing Then
				Return
			End If
			Dim abstractLevelWeapon2 As AbstractLevelWeapon = Global.UnityEngine.[Object].Instantiate(Of AbstractLevelWeapon)(abstractLevelWeapon)
			abstractLevelWeapon2.transform.parent = Me.root.transform
			abstractLevelWeapon2.Initialize(Me.weaponManager, id)
			abstractLevelWeapon2.name = abstractLevelWeapon2.name.Replace("(Clone)", String.Empty)
			Me.weapons(id) = abstractLevelWeapon2
		End Sub

		' Token: 0x06003E96 RID: 16022 RVA: 0x00225994 File Offset: 0x00223D94
		Public Sub OnDestroy()
			Me.peashot = Nothing
			Me.spread = Nothing
			Me.arc = Nothing
			Me.homing = Nothing
			Me.exploder = Nothing
			Me.charge = Nothing
			Me.boomerang = Nothing
			Me.bouncer = Nothing
			Me.wideShot = Nothing
		End Sub

		' Token: 0x0400459B RID: 17819
		<SerializeField()>
		Private peashot As WeaponPeashot

		' Token: 0x0400459C RID: 17820
		<SerializeField()>
		Private spread As WeaponSpread

		' Token: 0x0400459D RID: 17821
		<SerializeField()>
		Private arc As WeaponArc

		' Token: 0x0400459E RID: 17822
		<SerializeField()>
		Private homing As WeaponHoming

		' Token: 0x0400459F RID: 17823
		<SerializeField()>
		Private exploder As WeaponExploder

		' Token: 0x040045A0 RID: 17824
		<SerializeField()>
		Private charge As WeaponCharge

		' Token: 0x040045A1 RID: 17825
		<SerializeField()>
		Private boomerang As WeaponBoomerang

		' Token: 0x040045A2 RID: 17826
		<SerializeField()>
		Private bouncer As WeaponBouncer

		' Token: 0x040045A3 RID: 17827
		<SerializeField()>
		Private wideShot As WeaponWideShot

		' Token: 0x040045A4 RID: 17828
		<SerializeField()>
		Private upShot As WeaponUpshot

		' Token: 0x040045A5 RID: 17829
		<SerializeField()>
		Private crackshot As WeaponCrackshot

		' Token: 0x040045A6 RID: 17830
		Private root As Transform

		' Token: 0x040045A7 RID: 17831
		Private weaponManager As LevelPlayerWeaponManager

		' Token: 0x040045A8 RID: 17832
		Private weapons As Dictionary(Of Weapon, AbstractLevelWeapon)
	End Class

	' Token: 0x02000A41 RID: 2625
	<Serializable()>
	Public Class SuperPrefabs
		' Token: 0x06003E98 RID: 16024 RVA: 0x002259E8 File Offset: 0x00223DE8
		Public Sub Init(player As LevelPlayerController)
		End Sub

		' Token: 0x06003E99 RID: 16025 RVA: 0x002259EC File Offset: 0x00223DEC
		Public Function GetPrefab(super As Super) As AbstractPlayerSuper
			If super <> Super.level_super_beam Then
				If super = Super.level_super_ghost Then
					Return Me.ghost
				End If
				If super = Super.level_super_invincible Then
					Return Me.invincible
				End If
				If super = Super.level_super_chalice_iii Then
					Return Me.chaliceIII
				End If
				If super = Super.level_super_chalice_vert_beam Then
					Return Me.chaliceVertBeam
				End If
				If super = Super.level_super_chalice_shield Then
					Return Me.chaliceShield
				End If
			End If
			Return Me.beam
		End Function

		' Token: 0x06003E9A RID: 16026 RVA: 0x00225A69 File Offset: 0x00223E69
		Public Sub OnDestroy()
			Me.beam = Nothing
			Me.ghost = Nothing
			Me.invincible = Nothing
		End Sub

		' Token: 0x040045A9 RID: 17833
		<SerializeField()>
		Private beam As AbstractPlayerSuper

		' Token: 0x040045AA RID: 17834
		<SerializeField()>
		Private ghost As AbstractPlayerSuper

		' Token: 0x040045AB RID: 17835
		<SerializeField()>
		Private invincible As AbstractPlayerSuper

		' Token: 0x040045AC RID: 17836
		<SerializeField()>
		Private chaliceIII As AbstractPlayerSuper

		' Token: 0x040045AD RID: 17837
		<SerializeField()>
		Private chaliceVertBeam As AbstractPlayerSuper

		' Token: 0x040045AE RID: 17838
		<SerializeField()>
		Private chaliceShield As AbstractPlayerSuper
	End Class
End Class
