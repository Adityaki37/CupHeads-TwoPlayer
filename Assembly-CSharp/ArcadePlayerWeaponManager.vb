Imports System
Imports System.Collections.Generic
Imports System.Diagnostics
Imports UnityEngine

' Token: 0x020009F7 RID: 2551
Public Class ArcadePlayerWeaponManager
	Inherits AbstractArcadePlayerComponent

	' Token: 0x17000518 RID: 1304
	' (get) Token: 0x06003C3A RID: 15418 RVA: 0x00218C12 File Offset: 0x00217012
	' (set) Token: 0x06003C3B RID: 15419 RVA: 0x00218C1A File Offset: 0x0021701A
	Public Property shotBullet As Boolean

	' Token: 0x17000519 RID: 1305
	' (get) Token: 0x06003C3C RID: 15420 RVA: 0x00218C23 File Offset: 0x00217023
	' (set) Token: 0x06003C3D RID: 15421 RVA: 0x00218C2B File Offset: 0x0021702B
	Public Property IsShooting As Boolean

	' Token: 0x1700051A RID: 1306
	' (get) Token: 0x06003C3E RID: 15422 RVA: 0x00218C34 File Offset: 0x00217034
	' (set) Token: 0x06003C3F RID: 15423 RVA: 0x00218C3C File Offset: 0x0021703C
	Public Property FreezePosition As Boolean

	' Token: 0x1700051B RID: 1307
	' (get) Token: 0x06003C40 RID: 15424 RVA: 0x00218C45 File Offset: 0x00217045
	Public ReadOnly Property ExPosition As Vector2
		Get
			Return Me.exRoot.position
		End Get
	End Property

	' Token: 0x14000080 RID: 128
	' (add) Token: 0x06003C41 RID: 15425 RVA: 0x00218C58 File Offset: 0x00217058
	' (remove) Token: 0x06003C42 RID: 15426 RVA: 0x00218C90 File Offset: 0x00217090
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnBasicStart As Action

	' Token: 0x14000081 RID: 129
	' (add) Token: 0x06003C43 RID: 15427 RVA: 0x00218CC8 File Offset: 0x002170C8
	' (remove) Token: 0x06003C44 RID: 15428 RVA: 0x00218D00 File Offset: 0x00217100
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnExStart As Action

	' Token: 0x14000082 RID: 130
	' (add) Token: 0x06003C45 RID: 15429 RVA: 0x00218D38 File Offset: 0x00217138
	' (remove) Token: 0x06003C46 RID: 15430 RVA: 0x00218D70 File Offset: 0x00217170
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnSuperStart As Action

	' Token: 0x14000083 RID: 131
	' (add) Token: 0x06003C47 RID: 15431 RVA: 0x00218DA8 File Offset: 0x002171A8
	' (remove) Token: 0x06003C48 RID: 15432 RVA: 0x00218DE0 File Offset: 0x002171E0
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnExFire As Action

	' Token: 0x14000084 RID: 132
	' (add) Token: 0x06003C49 RID: 15433 RVA: 0x00218E18 File Offset: 0x00217218
	' (remove) Token: 0x06003C4A RID: 15434 RVA: 0x00218E50 File Offset: 0x00217250
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnWeaponFire As Action

	' Token: 0x14000085 RID: 133
	' (add) Token: 0x06003C4B RID: 15435 RVA: 0x00218E88 File Offset: 0x00217288
	' (remove) Token: 0x06003C4C RID: 15436 RVA: 0x00218EC0 File Offset: 0x002172C0
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnExEnd As Action

	' Token: 0x14000086 RID: 134
	' (add) Token: 0x06003C4D RID: 15437 RVA: 0x00218EF8 File Offset: 0x002172F8
	' (remove) Token: 0x06003C4E RID: 15438 RVA: 0x00218F30 File Offset: 0x00217330
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnSuperEnd As Action

	' Token: 0x06003C4F RID: 15439 RVA: 0x00218F68 File Offset: 0x00217368
	Protected Overrides Sub OnAwake()
		MyBase.OnAwake()
		AddHandler MyBase.basePlayer.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
		AddHandler MyBase.player.motor.OnDashStartEvent, AddressOf Me.OnDash
		Me.basic = New ArcadePlayerWeaponManager.WeaponState()
		Me.ex = New ArcadePlayerWeaponManager.ExState()
		Me.weaponsRoot = New GameObject("Weapons").transform
		Me.weaponsRoot.parent = MyBase.transform
		Me.weaponsRoot.localPosition = Vector3.zero
		Me.weaponsRoot.localEulerAngles = Vector3.zero
		Me.weaponsRoot.localScale = Vector3.one
		Me.aim = New GameObject("Aim").transform
		Me.aim.SetParent(MyBase.transform)
		Me.aim.ResetLocalTransforms()
	End Sub

	' Token: 0x06003C50 RID: 15440 RVA: 0x00219050 File Offset: 0x00217450
	Public Sub ChangeToRocket()
		Me.currentWeapon = Weapon.arcade_weapon_rocket_peashot
		Me.aim.SetLocalPosition(Nothing, New Single?(50F), Nothing)
	End Sub

	' Token: 0x06003C51 RID: 15441 RVA: 0x00219090 File Offset: 0x00217490
	Public Sub ChangeToJetPack()
		Me.aim.SetLocalPosition(Nothing, New Single?(30F), Nothing)
	End Sub

	' Token: 0x06003C52 RID: 15442 RVA: 0x002190C4 File Offset: 0x002174C4
	Private Sub FixedUpdate()
		If Not MyBase.player.levelStarted OrElse Not Me.allowInput Then
			Return
		End If
		Me.HandleWeaponFiring()
		If MyBase.player.motor.Grounded Then
			Me.ex.airAble = True
		End If
	End Sub

	' Token: 0x06003C53 RID: 15443 RVA: 0x00219114 File Offset: 0x00217514
	Private Sub OnEnable()
		Me.EnableInput()
	End Sub

	' Token: 0x06003C54 RID: 15444 RVA: 0x0021911C File Offset: 0x0021751C
	Public Overrides Sub OnLevelEnd()
		Me.EndBasic()
		MyBase.OnLevelEnd()
	End Sub

	' Token: 0x06003C55 RID: 15445 RVA: 0x0021912A File Offset: 0x0021752A
	Private Sub OnDash()
		Me.EndBasic()
	End Sub

	' Token: 0x06003C56 RID: 15446 RVA: 0x00219132 File Offset: 0x00217532
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		If Me.ex.firing Then
			Me.ex.firing = False
		End If
	End Sub

	' Token: 0x06003C57 RID: 15447 RVA: 0x00219150 File Offset: 0x00217550
	Public Sub ParrySuccess()
	End Sub

	' Token: 0x06003C58 RID: 15448 RVA: 0x00219152 File Offset: 0x00217552
	Public Sub LevelInit(id As PlayerId)
		Me.currentWeapon = Weapon.arcade_weapon_peashot
		Me.weaponPrefabs.Init(Me, Me.weaponsRoot)
		Me.superPrefabs.Init(MyBase.player)
	End Sub

	' Token: 0x06003C59 RID: 15449 RVA: 0x00219182 File Offset: 0x00217582
	Public Sub EnableInput()
		Me.allowInput = True
	End Sub

	' Token: 0x06003C5A RID: 15450 RVA: 0x0021918B File Offset: 0x0021758B
	Public Sub DisableInput()
		Me.allowInput = False
		Me.IsShooting = False
	End Sub

	' Token: 0x06003C5B RID: 15451 RVA: 0x0021919B File Offset: 0x0021759B
	Private Sub _WeaponFireEx()
		Me.FireEx()
	End Sub

	' Token: 0x06003C5C RID: 15452 RVA: 0x002191A3 File Offset: 0x002175A3
	Private Sub _WeaponEndEx()
		Me.EndEx()
	End Sub

	' Token: 0x06003C5D RID: 15453 RVA: 0x002191AB File Offset: 0x002175AB
	Private Sub StartBasic()
		Me.UpdateAim()
		Me.weaponPrefabs.GetWeapon(Me.currentWeapon).BeginBasic()
		If Me.OnBasicStart IsNot Nothing Then
			Me.OnBasicStart()
		End If
	End Sub

	' Token: 0x06003C5E RID: 15454 RVA: 0x002191DF File Offset: 0x002175DF
	Private Sub EndBasic()
		If Me.currentWeapon = Weapon.None Then
			Return
		End If
		Me.weaponPrefabs.GetWeapon(Me.currentWeapon).EndBasic()
		Me.basic.firing = False
	End Sub

	' Token: 0x06003C5F RID: 15455 RVA: 0x00219214 File Offset: 0x00217614
	Public Sub TriggerWeaponFire()
		Me.OnWeaponFire()
	End Sub

	' Token: 0x06003C60 RID: 15456 RVA: 0x00219224 File Offset: 0x00217624
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

	' Token: 0x06003C61 RID: 15457 RVA: 0x00219292 File Offset: 0x00217692
	Private Sub FireEx()
		Me.weaponPrefabs.GetWeapon(Me.currentWeapon).BeginEx()
		If Me.OnExFire IsNot Nothing Then
			Me.OnExFire()
		End If
	End Sub

	' Token: 0x06003C62 RID: 15458 RVA: 0x002192C0 File Offset: 0x002176C0
	Private Sub EndEx()
		Me.ex.firing = False
		If Me.OnExEnd IsNot Nothing Then
			Me.OnExEnd()
		End If
	End Sub

	' Token: 0x06003C63 RID: 15459 RVA: 0x002192E4 File Offset: 0x002176E4
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
			transform3.AddPositionForward2D(-100F)
		End If
		Global.UnityEngine.[Object].Destroy(transform.gameObject)
	End Sub

	' Token: 0x06003C64 RID: 15460 RVA: 0x00219415 File Offset: 0x00217815
	Private Sub StartSuper()
	End Sub

	' Token: 0x06003C65 RID: 15461 RVA: 0x00219417 File Offset: 0x00217817
	Private Sub EndSuper()
	End Sub

	' Token: 0x06003C66 RID: 15462 RVA: 0x00219419 File Offset: 0x00217819
	Public Sub EndSuperFromSuper()
		Me.EndSuper()
	End Sub

	' Token: 0x06003C67 RID: 15463 RVA: 0x00219424 File Offset: 0x00217824
	Private Sub HandleWeaponFiring()
		If MyBase.player.motor.Dashing OrElse MyBase.player.motor.IsHit Then
			Return
		End If
		If MyBase.player.input.actions.GetButtonDown(4) Then
			If MyBase.player.stats.SuperMeter >= MyBase.player.stats.SuperMeterMax Then
				Me.StartSuper()
				Return
			End If
			If MyBase.player.stats.CanUseEx AndAlso Me.ex.Able Then
				Me.StartEx()
				Return
			End If
		End If
		If Me.ex.firing Then
			Return
		End If
		If Me.basic.firing <> MyBase.player.input.actions.GetButton(3) Then
			If MyBase.player.input.actions.GetButton(3) Then
				Me.StartBasic()
			Else
				Me.EndBasic()
			End If
		End If
		Me.basic.firing = MyBase.player.input.actions.GetButton(3)
	End Sub

	' Token: 0x06003C68 RID: 15464 RVA: 0x00219554 File Offset: 0x00217954
	Private Function GetCurrentPose() As ArcadePlayerWeaponManager.Pose
		If Me.ex.firing Then
			Return ArcadePlayerWeaponManager.Pose.Ex
		End If
		If Not MyBase.player.motor.Grounded Then
			Return ArcadePlayerWeaponManager.Pose.Jump
		End If
		If MyBase.player.motor.Locked Then
			If MyBase.player.motor.LookDirection.y > 0 Then
				If MyBase.player.motor.LookDirection.x <> 0 Then
					Return ArcadePlayerWeaponManager.Pose.Up_D
				End If
				Return ArcadePlayerWeaponManager.Pose.Up
			ElseIf MyBase.player.motor.LookDirection.y < 0 Then
				If MyBase.player.motor.LookDirection.x <> 0 Then
					Return ArcadePlayerWeaponManager.Pose.Down_D
				End If
				Return ArcadePlayerWeaponManager.Pose.Down
			End If
		ElseIf MyBase.player.motor.LookDirection.x <> 0 Then
			If MyBase.player.motor.LookDirection.y > 0 Then
				Return ArcadePlayerWeaponManager.Pose.Up_D_R
			End If
			Return ArcadePlayerWeaponManager.Pose.Forward_R
		ElseIf MyBase.player.motor.LookDirection.y > 0 Then
			Return ArcadePlayerWeaponManager.Pose.Up
		End If
		Return ArcadePlayerWeaponManager.Pose.Forward
	End Function

	' Token: 0x06003C69 RID: 15465 RVA: 0x002196AC File Offset: 0x00217AAC
	Public Function GetDirectionPose() As ArcadePlayerWeaponManager.Pose
		If MyBase.player.motor.Dashing Then
			Return ArcadePlayerWeaponManager.Pose.Forward
		End If
		If MyBase.player.motor.LookDirection.y > 0 Then
			If MyBase.player.motor.LookDirection.x <> 0 Then
				Return ArcadePlayerWeaponManager.Pose.Up_D
			End If
			Return ArcadePlayerWeaponManager.Pose.Up
		Else
			If MyBase.player.motor.LookDirection.y >= 0 Then
				Return ArcadePlayerWeaponManager.Pose.Forward
			End If
			If MyBase.player.motor.LookDirection.x <> 0 Then
				Return ArcadePlayerWeaponManager.Pose.Down_D
			End If
			Return ArcadePlayerWeaponManager.Pose.Down
		End If
	End Function

	' Token: 0x06003C6A RID: 15466 RVA: 0x00219764 File Offset: 0x00217B64
	Public Sub UpdateAim()
		If MyBase.player.controlScheme = ArcadePlayerController.ControlScheme.Rocket Then
			Me.aim.SetEulerAngles(New Single?(0F), New Single?(0F), New Single?(MathUtils.DirectionToAngle(MyBase.transform.up.normalized)))
		ElseIf MyBase.player.controlScheme = ArcadePlayerController.ControlScheme.Jetpack Then
			Me.aim.SetEulerAngles(Nothing, Nothing, New Single?(MathUtils.DirectionToAngle(MyBase.player.motor.TrueLookDirection)))
		Else
			Me.aim.SetEulerAngles(New Single?(0F), New Single?(0F), New Single?(90F))
		End If
	End Sub

	' Token: 0x06003C6B RID: 15467 RVA: 0x00219843 File Offset: 0x00217C43
	Public Function GetBulletPosition() As Vector2
		Return Me.aim.transform.position
	End Function

	' Token: 0x06003C6C RID: 15468 RVA: 0x0021985C File Offset: 0x00217C5C
	Public Function GetBulletRotation() As Single
		Return Me.aim.eulerAngles.z
	End Function

	' Token: 0x06003C6D RID: 15469 RVA: 0x0021987C File Offset: 0x00217C7C
	Public Function GetBulletScale() As Vector3
		Return New Vector3(1F, MyBase.player.motor.TrueLookDirection.x, 1F)
	End Function

	' Token: 0x040043B1 RID: 17329
	<SerializeField()>
	Private weaponPrefabs As ArcadePlayerWeaponManager.WeaponPrefabs

	' Token: 0x040043B2 RID: 17330
	<SerializeField()>
	Private superPrefabs As ArcadePlayerWeaponManager.SuperPrefabs

	' Token: 0x040043B3 RID: 17331
	<Space(10F)>
	<SerializeField()>
	Private exDustEffect As Effect

	' Token: 0x040043B4 RID: 17332
	<SerializeField()>
	Private exChargeEffect As Effect

	' Token: 0x040043B5 RID: 17333
	<SerializeField()>
	Private exRoot As Transform

	' Token: 0x040043B9 RID: 17337
	Private currentWeapon As Weapon = Weapon.None

	' Token: 0x040043BA RID: 17338
	Private currentPose As ArcadePlayerWeaponManager.Pose

	' Token: 0x040043C2 RID: 17346
	Private basic As ArcadePlayerWeaponManager.WeaponState

	' Token: 0x040043C3 RID: 17347
	Private ex As ArcadePlayerWeaponManager.ExState

	' Token: 0x040043C4 RID: 17348
	Private weaponsRoot As Transform

	' Token: 0x040043C5 RID: 17349
	Private aim As Transform

	' Token: 0x040043C6 RID: 17350
	Private allowInput As Boolean = True

	' Token: 0x020009F8 RID: 2552
	Public Enum Pose
		' Token: 0x040043C8 RID: 17352
		Forward
		' Token: 0x040043C9 RID: 17353
		Forward_R
		' Token: 0x040043CA RID: 17354
		Up
		' Token: 0x040043CB RID: 17355
		Up_D
		' Token: 0x040043CC RID: 17356
		Up_D_R
		' Token: 0x040043CD RID: 17357
		Down
		' Token: 0x040043CE RID: 17358
		Down_D
		' Token: 0x040043CF RID: 17359
		Duck
		' Token: 0x040043D0 RID: 17360
		Jump
		' Token: 0x040043D1 RID: 17361
		Ex
	End Enum

	' Token: 0x020009F9 RID: 2553
	' (Invoke) Token: 0x06003C6F RID: 15471
	Public Delegate Sub OnWeaponChangeHandler(weapon As Weapon)

	' Token: 0x020009FA RID: 2554
	Public Structure ProjectilePosition
		' Token: 0x06003C72 RID: 15474 RVA: 0x002198B5 File Offset: 0x00217CB5
		Public Shared Function [Get](pose As ArcadePlayerWeaponManager.Pose, direction As ArcadePlayerWeaponManager.Pose) As Vector2
			If pose = ArcadePlayerWeaponManager.Pose.Jump Then
				Return New Vector2(0F, 105F)
			End If
			Return New Vector2(4F, 115F)
		End Function
	End Structure

	' Token: 0x020009FB RID: 2555
	Public Class WeaponState
		' Token: 0x040043D2 RID: 17362
		Public state As ArcadePlayerWeaponManager.WeaponState.State

		' Token: 0x040043D3 RID: 17363
		Public firing As Boolean

		' Token: 0x040043D4 RID: 17364
		Public holding As Boolean

		' Token: 0x020009FC RID: 2556
		Public Enum State
			' Token: 0x040043D6 RID: 17366
			Ready
			' Token: 0x040043D7 RID: 17367
			Firing
			' Token: 0x040043D8 RID: 17368
			Fired
			' Token: 0x040043D9 RID: 17369
			Ended
		End Enum
	End Class

	' Token: 0x020009FD RID: 2557
	Public Class ExState
		' Token: 0x1700051C RID: 1308
		' (get) Token: 0x06003C75 RID: 15477 RVA: 0x002198F4 File Offset: 0x00217CF4
		Public ReadOnly Property Able As Boolean
			Get
				Return Me.airAble AndAlso Not Me.firing
			End Get
		End Property

		' Token: 0x040043DA RID: 17370
		Public airAble As Boolean = True

		' Token: 0x040043DB RID: 17371
		Public firing As Boolean
	End Class

	' Token: 0x020009FE RID: 2558
	<Serializable()>
	Public Class WeaponPrefabs
		' Token: 0x06003C77 RID: 15479 RVA: 0x00219915 File Offset: 0x00217D15
		Public Sub Init(weaponManager As ArcadePlayerWeaponManager, root As Transform)
			Me.weaponManager = weaponManager
			Me.root = root
			Me.weapons = New Dictionary(Of Weapon, AbstractArcadeWeapon)()
			Me.InitWeapon(Weapon.arcade_weapon_peashot)
			Me.InitWeapon(Weapon.arcade_weapon_rocket_peashot)
		End Sub

		' Token: 0x06003C78 RID: 15480 RVA: 0x00219946 File Offset: 0x00217D46
		Public Function GetWeapon(weapon As Weapon) As AbstractArcadeWeapon
			Return Me.weapons(weapon)
		End Function

		' Token: 0x06003C79 RID: 15481 RVA: 0x00219954 File Offset: 0x00217D54
		Private Sub InitWeapon(id As Weapon)
			Dim abstractArcadeWeapon As AbstractArcadeWeapon = Me.peashot
			Dim abstractArcadeWeapon2 As AbstractArcadeWeapon = Global.UnityEngine.[Object].Instantiate(Of AbstractArcadeWeapon)(abstractArcadeWeapon)
			abstractArcadeWeapon2.transform.parent = Me.root.transform
			abstractArcadeWeapon2.Initialize(Me.weaponManager, id)
			abstractArcadeWeapon2.name = abstractArcadeWeapon2.name.Replace("(Clone)", String.Empty)
			Me.weapons(id) = abstractArcadeWeapon2
		End Sub

		' Token: 0x040043DC RID: 17372
		<SerializeField()>
		Private peashot As ArcadeWeaponPeashot

		' Token: 0x040043DD RID: 17373
		<SerializeField()>
		Private rocketPeashot As ArcadeWeaponRocketPeashot

		' Token: 0x040043DE RID: 17374
		Private root As Transform

		' Token: 0x040043DF RID: 17375
		Private weaponManager As ArcadePlayerWeaponManager

		' Token: 0x040043E0 RID: 17376
		Private weapons As Dictionary(Of Weapon, AbstractArcadeWeapon)
	End Class

	' Token: 0x020009FF RID: 2559
	<Serializable()>
	Public Class SuperPrefabs
		' Token: 0x06003C7B RID: 15483 RVA: 0x002199C4 File Offset: 0x00217DC4
		Public Sub Init(player As ArcadePlayerController)
		End Sub

		' Token: 0x06003C7C RID: 15484 RVA: 0x002199C6 File Offset: 0x00217DC6
		Public Function GetPrefab(super As Super) As AbstractPlayerSuper
			Return Nothing
		End Function
	End Class
End Class
