Imports System
Imports System.Collections
Imports System.Diagnostics
Imports UnityEngine

' Token: 0x02000A91 RID: 2705
Public Class PlanePlayerAnimationController
	Inherits AbstractPlanePlayerComponent

	' Token: 0x1700059F RID: 1439
	' (get) Token: 0x060040A8 RID: 16552 RVA: 0x00232C04 File Offset: 0x00231004
	Private ReadOnly Property activeTransform As Transform
		Get
			If MyBase.player.stats.isChalice Then
				Return Me.chalice
			End If
			If PlayerManager.player1IsMugman AndAlso MyBase.player.id = PlayerId.PlayerOne Then
				Return Me.mugman
			End If
			Return Me.cuphead
		End Get
	End Property

	' Token: 0x170005A0 RID: 1440
	' (get) Token: 0x060040A9 RID: 16553 RVA: 0x00232C54 File Offset: 0x00231054
	' (set) Token: 0x060040AA RID: 16554 RVA: 0x00232C5C File Offset: 0x0023105C
	Public Property spriteRenderer As SpriteRenderer

	' Token: 0x170005A1 RID: 1441
	' (get) Token: 0x060040AB RID: 16555 RVA: 0x00232C65 File Offset: 0x00231065
	' (set) Token: 0x060040AC RID: 16556 RVA: 0x00232C6D File Offset: 0x0023106D
	Public Property ShrinkState As PlanePlayerAnimationController.ShrinkStates

	' Token: 0x170005A2 RID: 1442
	' (get) Token: 0x060040AD RID: 16557 RVA: 0x00232C76 File Offset: 0x00231076
	' (set) Token: 0x060040AE RID: 16558 RVA: 0x00232C7E File Offset: 0x0023107E
	Public Property Shrinking As Boolean

	' Token: 0x140000A0 RID: 160
	' (add) Token: 0x060040AF RID: 16559 RVA: 0x00232C88 File Offset: 0x00231088
	' (remove) Token: 0x060040B0 RID: 16560 RVA: 0x00232CC0 File Offset: 0x002310C0
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnExFireAnimEvent As Action

	' Token: 0x140000A1 RID: 161
	' (add) Token: 0x060040B1 RID: 16561 RVA: 0x00232CF8 File Offset: 0x002310F8
	' (remove) Token: 0x060040B2 RID: 16562 RVA: 0x00232D30 File Offset: 0x00231130
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnShrinkEvent As Action

	' Token: 0x060040B3 RID: 16563 RVA: 0x00232D68 File Offset: 0x00231168
	Private Sub Start()
		AddHandler MyBase.player.weaponManager.OnExStartEvent, AddressOf Me.OnExStart
		AddHandler MyBase.player.weaponManager.OnSuperStartEvent, AddressOf Me.OnSuperStart
		AddHandler MyBase.player.parryController.OnParryStartEvent, AddressOf Me.OnParryStart
		AddHandler MyBase.player.parryController.OnParrySuccessEvent, AddressOf Me.OnParrySuccess
		AddHandler MyBase.player.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
		AddHandler MyBase.player.stats.OnPlayerDeathEvent, AddressOf Me.OnDeath
		AddHandler MyBase.player.OnReviveEvent, AddressOf Me.OnRevive
		AddHandler MyBase.player.stats.OnStoneShake, AddressOf Me.onStoneShake
		AddHandler MyBase.player.stats.OnStoned, AddressOf Me.onStoned
		If Me.spriteRenderer Is Nothing Then
			Me.spriteRenderer = Me.playerSprite.GetComponent(Of SpriteRenderer)()
		End If
		PlayerRecolorHandler.SetChaliceRecolorEnabled(Me.chalice.GetComponent(Of SpriteRenderer)().sharedMaterial, SettingsData.Data.filter = BlurGamma.Filter.Chalice)
		If MyBase.player.stats.Loadout.charm = Charm.charm_curse Then
			Me.curseCharmLevel = CharmCurse.CalculateLevel(MyBase.player.id)
		End If
		If Me.curseCharmLevel > -1 Then
			Me.InitializeCurseFX()
		End If
	End Sub

	' Token: 0x060040B4 RID: 16564 RVA: 0x00232EF7 File Offset: 0x002312F7
	Private Sub OnEnable()
		MyBase.StartCoroutine(Me.flash_cr())
		Me.CheckActivateCurseFX()
	End Sub

	' Token: 0x060040B5 RID: 16565 RVA: 0x00232F0C File Offset: 0x0023130C
	Private Sub OnDisable()
		If Me.paladinShadows(0) Then
			Me.paladinShadows(0).enabled = False
		End If
		If Me.paladinShadows(1) Then
			Me.paladinShadows(1).enabled = False
		End If
	End Sub

	' Token: 0x060040B6 RID: 16566 RVA: 0x00232F59 File Offset: 0x00231359
	Private Sub Update()
		If Me.curseCharmLevel > -1 Then
			Me.HandleCurseFX()
		End If
	End Sub

	' Token: 0x060040B7 RID: 16567 RVA: 0x00232F70 File Offset: 0x00231370
	Private Sub FixedUpdate()
		Me.HandleRotation()
		Me.HandleShrunk()
		Me.SetInteger("Y", MyBase.player.motor.MoveDirection.y)
	End Sub

	' Token: 0x060040B8 RID: 16568 RVA: 0x00232FB4 File Offset: 0x002313B4
	Public Sub LevelInit()
		Dim id As PlayerId = MyBase.player.id
		If id = PlayerId.PlayerOne OrElse id <> PlayerId.PlayerTwo Then
			Me.playerSprite = If((Not MyBase.player.stats.isChalice), If((Not PlayerManager.player1IsMugman), Me.cuphead, Me.mugman), Me.chalice)
		Else
			Me.playerSprite = If((Not MyBase.player.stats.isChalice), If((Not PlayerManager.player1IsMugman), Me.mugman, Me.cuphead), Me.chalice)
		End If
		Me.cuphead.gameObject.SetActive(False)
		Me.mugman.gameObject.SetActive(False)
		Me.chalice.gameObject.SetActive(False)
		If Level.Current.Started Then
			Me.playerSprite.gameObject.SetActive(True)
		End If
	End Sub

	' Token: 0x060040B9 RID: 16569 RVA: 0x002330C0 File Offset: 0x002314C0
	Public Sub PlayIntro()
		Dim text As String = If(((MyBase.player.id <> PlayerId.PlayerOne OrElse Not PlayerManager.player1IsMugman) AndAlso (MyBase.player.id <> PlayerId.PlayerTwo OrElse PlayerManager.player1IsMugman)), "Cuphead", "Mugman")
		If MyBase.player.stats.isChalice Then
			MyBase.animator.Play("Intro_Chalice_" + text + If((MyBase.player.id <> PlayerId.PlayerOne), "_P2", String.Empty))
		ElseIf MyBase.player.stats.Loadout.charm = Charm.charm_chalice AndAlso Not MyBase.player.stats.isChalice Then
			MyBase.animator.Play("Intro_Chalice_" + text + "_Fail")
		Else
			Dim id As PlayerId = MyBase.player.id
			If id = PlayerId.PlayerOne OrElse id <> PlayerId.PlayerTwo Then
				MyBase.animator.Play("Intro")
			Else
				MyBase.animator.Play("Intro_Alt")
			End If
		End If
		Me.spriteRenderer = Me.playerSprite.GetComponent(Of SpriteRenderer)()
		Me.playerSprite.gameObject.SetActive(True)
		If MyBase.gameObject.activeSelf Then
			If Not Level.Current.Started AndAlso MyBase.player.id = PlayerId.PlayerTwo AndAlso MyBase.player.stats.Loadout.charm <> Charm.charm_chalice Then
				Me.playerSprite.SetLocalPosition(New Single?(Me.introRoot.transform.localPosition.x), New Single?(Me.introRoot.transform.localPosition.y), New Single?(0F))
			End If
			MyBase.StartCoroutine(Me.done_intro_cr())
		End If
	End Sub

	' Token: 0x060040BA RID: 16570 RVA: 0x002332CC File Offset: 0x002316CC
	Private Iterator Function done_intro_cr() As IEnumerator
		Yield MyBase.animator.WaitForAnimationToEnd(Me, If((MyBase.player.id <> PlayerId.PlayerOne), "Intro_Alt", "Intro"), False, True)
		MyBase.StartCoroutine(Me.puff_cr())
		Me.CheckActivateCurseFX()
		Yield Nothing
		Return
	End Function

	' Token: 0x060040BB RID: 16571 RVA: 0x002332E8 File Offset: 0x002316E8
	Private Sub CheckActivateCurseFX()
		If Me.curseCharmLevel = 4 AndAlso Me.paladinShadows IsNot Nothing AndAlso Me.paladinShadowSprite.Length = 10 Then
			If Me.paladinShadows(0) IsNot Nothing Then
				Me.paladinShadows(0).enabled = True
			End If
			If Me.paladinShadows(1) IsNot Nothing Then
				Me.paladinShadows(1).enabled = True
			End If
		End If
		Me.showCurseFX = True
	End Sub

	' Token: 0x060040BC RID: 16572 RVA: 0x00233364 File Offset: 0x00231764
	Private Sub ResetPosition()
		Me.playerSprite.SetLocalPosition(New Single?(0F), New Single?(0F), Nothing)
	End Sub

	' Token: 0x060040BD RID: 16573 RVA: 0x0023339C File Offset: 0x0023179C
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		If MyBase.player.stats.Health <= 0 OrElse info.damage <= 0F Then
			Return
		End If
		Me.hitSparkEffect.Create(MyBase.player.center)
		Me.hitDustEffect.Create(MyBase.player.center)
		CupheadLevelCamera.Current.Shake(20F, 0.6F, False)
	End Sub

	' Token: 0x060040BE RID: 16574 RVA: 0x00233413 File Offset: 0x00231813
	Public Sub OnHealerCharm()
		Me.healerCharmEffect.Create(MyBase.player.center, MyBase.transform.localScale, MyBase.player)
		AudioManager.Play("player_charmhealer_extraheart")
	End Sub

	' Token: 0x060040BF RID: 16575 RVA: 0x00233446 File Offset: 0x00231846
	Public Sub SetOldMaterial()
		Me.spriteRenderer.material = Me.tempMaterial
	End Sub

	' Token: 0x060040C0 RID: 16576 RVA: 0x00233459 File Offset: 0x00231859
	Public Sub SetMaterial(m As Material)
		Me.tempMaterial = Me.spriteRenderer.material
		Me.spriteRenderer.material = m
	End Sub

	' Token: 0x060040C1 RID: 16577 RVA: 0x00233478 File Offset: 0x00231878
	Public Function GetMaterial() As Material
		Return Me.spriteRenderer.material
	End Function

	' Token: 0x060040C2 RID: 16578 RVA: 0x00233485 File Offset: 0x00231885
	Public Function GetSpriteRenderer() As SpriteRenderer
		Return Me.spriteRenderer
	End Function

	' Token: 0x060040C3 RID: 16579 RVA: 0x00233490 File Offset: 0x00231890
	Private Sub onStoned()
		Me.ShrinkState = PlanePlayerAnimationController.ShrinkStates.Ready
		MyBase.animator.SetLayerWeight(1, 0F)
		MyBase.StopCoroutine(Me.ex_cr())
		Me.greenPrefab.Create(MyBase.player.center)
		MyBase.animator.Play("Stone_Idle")
		MyBase.animator.ResetTrigger("Breakout")
		MyBase.StartCoroutine(Me.stone_animation_cr())
		MyBase.StartCoroutine(Me.create_poofs_cr())
		Me.isStoned = True
	End Sub

	' Token: 0x060040C4 RID: 16580 RVA: 0x0023351C File Offset: 0x0023191C
	Private Iterator Function stone_animation_cr() As IEnumerator
		While MyBase.player.stats.StoneTime > 0F
			Yield Nothing
		End While
		MyBase.animator.SetTrigger("Breakout")
		Dim animState As AnimatorStateInfo = MyBase.animator.GetCurrentAnimatorStateInfo(0)
		While animState.IsName("Stone_Idle") OrElse animState.IsName("Stone_Shake_A") OrElse animState.IsName("Stone_Shake_B") OrElse animState.IsName("Stone_Shake_C") OrElse animState.IsName("Stone_Shake_C") OrElse animState.IsName("Stone_Shake_D") OrElse animState.IsName("Stone_Shake_E") OrElse animState.IsName("Breakout")
			Yield Nothing
		End While
		Yield Nothing
		Return
	End Function

	' Token: 0x060040C5 RID: 16581 RVA: 0x00233537 File Offset: 0x00231937
	Private Sub Breakout()
		Me.isStoned = False
		Me.breakoutPrefab.Create(MyBase.player.center).transform.parent = MyBase.transform
		MyBase.StopCoroutine(Me.create_poofs_cr())
	End Sub

	' Token: 0x060040C6 RID: 16582 RVA: 0x00233572 File Offset: 0x00231972
	Private Sub onStoneShake()
		MyBase.animator.SetTrigger("Shake")
	End Sub

	' Token: 0x060040C7 RID: 16583 RVA: 0x00233584 File Offset: 0x00231984
	Private Iterator Function create_poofs_cr() As IEnumerator
		Dim t As Single = 0F
		Dim time As Single = 0.1F
		While MyBase.player.stats.StoneTime > 0F
			If Not MyBase.animator.GetCurrentAnimatorStateInfo(0).IsName("Stone_Idle") AndAlso Not MyBase.animator.GetCurrentAnimatorStateInfo(0).IsName("Breakout") Then
				Dim layerName As String = If((Not Rand.Bool()), SpriteLayer.Effects.ToString(), SpriteLayer.Enemies.ToString())
				Dim poof As Effect = Global.UnityEngine.[Object].Instantiate(Of Effect)(Me.poofPrefab)
				poof.transform.position = MyBase.player.center
				poof.animator.SetInteger("Poof", Global.UnityEngine.Random.Range(0, 3))
				poof.GetComponent(Of SpriteRenderer)().sortingLayerName = layerName
				While t < time
					t += CupheadTime.Delta
					Yield Nothing
				End While
				t = 0F
			End If
			Yield Nothing
		End While
		Yield Nothing
		Return
	End Function

	' Token: 0x060040C8 RID: 16584 RVA: 0x002335A0 File Offset: 0x002319A0
	Private Sub HandleRotation()
		Dim num As Single = 0F
		If MyBase.player.motor.MoveDirection.x < 0 Then
			num = 9F
		ElseIf MyBase.player.motor.MoveDirection.x > 0 Then
			num = -9F
		End If
		If MyBase.player.Shrunk AndAlso Not MyBase.player.stats.isChalice Then
			num += 5F * CSng((-CSng(MyBase.player.motor.MoveDirection.x)))
		End If
		Me.rotation = Mathf.Lerp(Me.rotation, num, 7F * CupheadTime.FixedDelta)
		Me.activeTransform.SetEulerAngles(New Single?(0F), New Single?(0F), New Single?(Me.rotation))
	End Sub

	' Token: 0x060040C9 RID: 16585 RVA: 0x002336A0 File Offset: 0x00231AA0
	Private Sub HandleShrunk()
		If Me.ShrinkState = PlanePlayerAnimationController.ShrinkStates.Cooldown Then
			If Me.shrinkCooldownTimeLeft <= 0F Then
				Me.ShrinkState = PlanePlayerAnimationController.ShrinkStates.Ready
			End If
			Me.shrinkCooldownTimeLeft -= CupheadTime.FixedDelta
		End If
		If MyBase.player.Parrying OrElse MyBase.player.WeaponBusy OrElse MyBase.player.stats.StoneTime > 0F OrElse Me.ShrinkState = PlanePlayerAnimationController.ShrinkStates.Cooldown Then
			Return
		End If
		If Me.ShrinkState = PlanePlayerAnimationController.ShrinkStates.Ready AndAlso (MyBase.player.input.actions.GetButtonDown(7) OrElse MyBase.player.input.actions.GetButtonDown(6)) Then
			MyBase.animator.SetLayerWeight(1, 1F)
			MyBase.animator.Play("Shrink_In", 0)
			Me.Shrinking = True
			Me.ShrinkState = PlanePlayerAnimationController.ShrinkStates.Shrunk
			If Me.OnShrinkEvent IsNot Nothing Then
				Me.OnShrinkEvent()
			End If
			If MyBase.player.stats.Loadout.charm = Charm.charm_smoke_dash OrElse MyBase.player.stats.CurseSmokeDash Then
				Me.smokeDashEffect.Create(MyBase.player.center)
			End If
			AudioManager.Play("player_plane_shrink")
		End If
		If Me.ShrinkState = PlanePlayerAnimationController.ShrinkStates.Shrunk AndAlso Not MyBase.player.input.actions.GetButton(7) AndAlso Not MyBase.player.input.actions.GetButton(6) Then
			Me.Shrinking = False
			MyBase.animator.SetLayerWeight(1, 0F)
			MyBase.animator.Play("Shrink_Out", 0)
			Me.ShrinkState = PlanePlayerAnimationController.ShrinkStates.Cooldown
			Me.shrinkCooldownTimeLeft = 0.23300001F
			AudioManager.Play("player_plane_expand")
		End If
	End Sub

	' Token: 0x060040CA RID: 16586 RVA: 0x0023388C File Offset: 0x00231C8C
	Private Iterator Function bomb_cr() As IEnumerator
		Yield Nothing
		MyBase.animator.SetLayerWeight(2, 1F)
		Dim t As Single = 0F
		Dim slowShakeScales As Single() = New Single() { 1F, 1.184F, 1.09F }
		Dim fastShakeScales As Single() = New Single() { 1F, 1.184F, 1.09F, 1.34F, 1.09F, 1.184F }
		While MyBase.player.weaponManager.states.super = PlanePlayerWeaponManager.States.Super.Intro
			Yield Nothing
		End While
		While MyBase.player.weaponManager.states.super = PlanePlayerWeaponManager.States.Super.Countdown
			If t < 0.4F * WeaponProperties.PlaneSuperBomb.countdownTime Then
				Yield Nothing
				t += CupheadTime.Delta
			ElseIf t < 0.7F * WeaponProperties.PlaneSuperBomb.countdownTime Then
				For Each scale As Single In slowShakeScales
					MyBase.transform.SetScale(New Single?(scale), New Single?(scale), Nothing)
					Yield CupheadTime.WaitForSeconds(Me, 0.083333336F)
					t += 0.083333336F
				Next
			Else
				For Each scale2 As Single In fastShakeScales
					MyBase.transform.SetScale(New Single?(scale2), New Single?(scale2), Nothing)
					Yield CupheadTime.WaitForSeconds(Me, 0.041666668F)
					t += 0.041666668F
				Next
			End If
		End While
		MyBase.animator.SetLayerWeight(2, 0F)
		MyBase.transform.SetScale(New Single?(1F), New Single?(1F), Nothing)
		Return
	End Function

	' Token: 0x060040CB RID: 16587 RVA: 0x002338A7 File Offset: 0x00231CA7
	Public Sub SetSpriteVisible(visible As Boolean)
		Me.playerSprite.gameObject.SetActive(visible)
	End Sub

	' Token: 0x060040CC RID: 16588 RVA: 0x002338BC File Offset: 0x00231CBC
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me.breakoutPrefab = Nothing
		Me.poofPrefab = Nothing
		Me.greenPrefab = Nothing
		Me.puffPrefab = Nothing
		Me.hitSparkEffect = Nothing
		Me.hitDustEffect = Nothing
		Me.smokeDashEffect = Nothing
		Me.shrinkEffect = Nothing
		Me.growEffect = Nothing
	End Sub

	' Token: 0x060040CD RID: 16589 RVA: 0x00233910 File Offset: 0x00231D10
	Private Sub SetAlpha(a As Single)
		Dim color As Color = Me.spriteRenderer.color
		color.a = a
		Me.spriteRenderer.color = color
	End Sub

	' Token: 0x060040CE RID: 16590 RVA: 0x0023393D File Offset: 0x00231D3D
	Private Sub OnShrinkInComplete()
		Me.shrinkEffect.Create(MyBase.player.center)
		Me.Shrinking = False
	End Sub

	' Token: 0x060040CF RID: 16591 RVA: 0x0023395D File Offset: 0x00231D5D
	Private Sub OnShrinkOutComplete()
		Me.growEffect.Create(MyBase.player.center)
	End Sub

	' Token: 0x060040D0 RID: 16592 RVA: 0x00233978 File Offset: 0x00231D78
	Private Sub CreatePuff()
		If Me.playerSprite Is Nothing Then
			Return
		End If
		Dim planeLevelEffect As PlaneLevelEffect = TryCast(Me.puffPrefab.Create(Me.playerSprite.position + PlanePlayerAnimationController.PUFF_OFFSET), PlaneLevelEffect)
		If MyBase.player.motor.MoveDirection.x < 0 Then
			planeLevelEffect.speed = 2F
		End If
	End Sub

	' Token: 0x060040D1 RID: 16593 RVA: 0x002339F0 File Offset: 0x00231DF0
	Private Iterator Function puff_cr() As IEnumerator
		Dim delay As Single = 0.17F
		While True
			delay = 0.17F
			If(MyBase.player.motor.MoveDirection.x <> 0 OrElse MyBase.player.motor.MoveDirection.y <> 0) AndAlso MyBase.player.motor.MoveDirection.x >= 0 Then
				delay = 0.07F
			End If
			If MyBase.player.motor.MoveDirection.x >= 0 Then
				Me.CreatePuff()
			End If
			Yield CupheadTime.WaitForSeconds(Me, delay)
		End While
		Return
	End Function

	' Token: 0x060040D2 RID: 16594 RVA: 0x00233A0C File Offset: 0x00231E0C
	Private Sub InitializeCurseFX()
		Me.curseEffectAngle = CSng(Global.UnityEngine.Random.Range(0, 360))
		If Me.curseCharmLevel = 4 Then
			Me.paladinShadowPosition = New Vector3(9) {}
			Me.paladinShadowScale = New Vector3(9) {}
			Me.paladinShadowSprite = New Sprite(9) {}
			For i As Integer = 0 To 10 - 1
				Me.paladinShadowPosition(i) = MyBase.transform.position
				Me.paladinShadowSprite(i) = Me.spriteRenderer.sprite
				Me.paladinShadowScale(i) = MyBase.transform.localScale
			Next
			If Me.paladinShadows IsNot Nothing Then
				Me.paladinShadows(0).transform.position = MyBase.transform.position
				Me.paladinShadows(1).transform.position = MyBase.transform.position
				Me.paladinShadows(0).sprite = Me.spriteRenderer.sprite
				Me.paladinShadows(1).sprite = Me.spriteRenderer.sprite
				Me.paladinShadows(0).transform.parent = Nothing
				Me.paladinShadows(1).transform.parent = Nothing
			End If
		End If
	End Sub

	' Token: 0x060040D3 RID: 16595 RVA: 0x00233B58 File Offset: 0x00231F58
	Private Sub HandleCurseFX()
		If PauseManager.state = PauseManager.State.Paused OrElse Not Me.showCurseFX Then
			Return
		End If
		Me.curseEffectTimer += CupheadTime.Delta
		While Me.curseEffectTimer >= Me.curseEffectDelay
			Dim effect As Effect = Me.curseEffect.Create(MyBase.player.center + MathUtils.AngleToDirection(Me.curseEffectAngle) * Me.curseDistanceRange.RandomFloat())
			effect.transform.localScale = New Vector3(0.8F, 0.8F)
			Dim text As String = Nothing
			If Me.curseCharmLevel < 2 Then
				text = If((Not Rand.Bool()), "Flames", "Cloud") + Global.UnityEngine.Random.Range(0, 3).ToString()
			End If
			If Me.curseCharmLevel = 2 Then
				text = If((Not Rand.Bool()), ("Dizzy" + Global.UnityEngine.Random.Range(0, 4).ToString()), ("Cloud" + Global.UnityEngine.Random.Range(0, 3).ToString()))
			End If
			If Me.curseCharmLevel = 3 Then
				text = "Dizzy" + Global.UnityEngine.Random.Range(0, 4).ToString()
			End If
			If Me.curseCharmLevel = 4 Then
				text = "Sparkle" + Global.UnityEngine.Random.Range(0, 3).ToString()
			End If
			effect.animator.Play(text)
			Me.curseEffectAngle = (Me.curseEffectAngle + Me.curseAngleShiftRange.RandomFloat()) Mod 360F
			Me.curseEffectTimer -= Me.curseEffectDelay
		End While
		If Me.curseCharmLevel = 4 AndAlso Me.paladinShadows IsNot Nothing Then
			For i As Integer = 9 To 0 + 1 Step -1
				Me.paladinShadowPosition(i) = Me.paladinShadowPosition(i - 1)
				Me.paladinShadowScale(i) = Me.paladinShadowScale(i - 1)
				Me.paladinShadowSprite(i) = Me.paladinShadowSprite(i - 1)
			Next
			Me.paladinShadowPosition(0) = MyBase.transform.position
			Me.paladinShadowScale(0) = MyBase.transform.localScale
			Me.paladinShadowSprite(0) = Me.spriteRenderer.sprite
			Me.paladinShadows(0).transform.position = Me.paladinShadowPosition(5)
			Me.paladinShadows(1).transform.position = Me.paladinShadowPosition(9)
			Me.paladinShadows(0).transform.localScale = Me.paladinShadowScale(5)
			Me.paladinShadows(1).transform.localScale = Me.paladinShadowScale(9)
			Me.paladinShadows(0).sprite = Me.paladinShadowSprite(5)
			Me.paladinShadows(1).sprite = Me.paladinShadowSprite(9)
		End If
	End Sub

	' Token: 0x170005A3 RID: 1443
	' (get) Token: 0x060040D4 RID: 16596 RVA: 0x00233EC5 File Offset: 0x002322C5
	Private ReadOnly Property Flashing As Boolean
		Get
			Return MyBase.player.damageReceiver.state = PlayerDamageReceiver.State.Invulnerable
		End Get
	End Property

	' Token: 0x060040D5 RID: 16597 RVA: 0x00233EDC File Offset: 0x002322DC
	Private Iterator Function flash_cr() As IEnumerator
		Dim t As Single = 0F
		While True
			While Me.Flashing
				Me.SetAlpha(0.3F)
				t = 0F
				While t < 0.05F
					If Not Me.Flashing Then
						Me.SetAlpha(1F)
						Exit While
					End If
					t += MyBase.LocalDeltaTime
					Yield Nothing
				End While
				If Not Me.Flashing Then
					Me.SetAlpha(1F)
					Exit While
				End If
				Me.SetAlpha(1F)
				t = 0F
				While t < 0.2F
					If Not Me.Flashing Then
						Me.SetAlpha(1F)
						Exit While
					End If
					t += MyBase.LocalDeltaTime
					Yield Nothing
				End While
				If Not Me.Flashing Then
					Me.SetAlpha(1F)
					Exit While
				End If
			End While
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x060040D6 RID: 16598 RVA: 0x00233EF7 File Offset: 0x002322F7
	Private Sub OnExStart()
		MyBase.StartCoroutine(Me.ex_cr())
	End Sub

	' Token: 0x060040D7 RID: 16599 RVA: 0x00233F08 File Offset: 0x00232308
	Private Iterator Function ex_cr() As IEnumerator
		Dim dir As String = If((MyBase.player.motor.MoveDirection.y > 0), "Up", "Down")
		MyBase.animator.Play("Ex_" + dir)
		If dir = "Up" Then
			AudioManager.Play("player_plane_up_ex")
		End If
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Ex_" + dir, False, True)
		If Me.OnExFireAnimEvent IsNot Nothing Then
			Me.OnExFireAnimEvent()
		End If
		Return
	End Function

	' Token: 0x060040D8 RID: 16600 RVA: 0x00233F23 File Offset: 0x00232323
	Private Sub OnSuperStart()
		MyBase.StartCoroutine(Me.bomb_cr())
	End Sub

	' Token: 0x060040D9 RID: 16601 RVA: 0x00233F34 File Offset: 0x00232334
	Public Sub SetColor(color As Color)
		Dim a As Single = Me.spriteRenderer.color.a
		color.a = a
		Me.spriteRenderer.color = color
	End Sub

	' Token: 0x060040DA RID: 16602 RVA: 0x00233F6C File Offset: 0x0023236C
	Public Sub ResetColor()
		Dim a As Single = Me.spriteRenderer.color.a
		Me.spriteRenderer.color = New Color(1F, 1F, 1F, a)
	End Sub

	' Token: 0x060040DB RID: 16603 RVA: 0x00233FAD File Offset: 0x002323AD
	Public Sub SetColorOverTime(color As Color, time As Single)
		Me.StopColorCoroutine()
		Me.colorCoroutine = Me.setColor_cr(color, time)
		MyBase.StartCoroutine(Me.colorCoroutine)
	End Sub

	' Token: 0x060040DC RID: 16604 RVA: 0x00233FD0 File Offset: 0x002323D0
	Public Sub StopColorCoroutine()
		If Me.colorCoroutine IsNot Nothing Then
			MyBase.StopCoroutine(Me.colorCoroutine)
		End If
		Me.colorCoroutine = Nothing
	End Sub

	' Token: 0x060040DD RID: 16605 RVA: 0x00233FF0 File Offset: 0x002323F0
	Private Iterator Function setColor_cr(color As Color, time As Single) As IEnumerator
		Dim t As Single = 0F
		Dim startColor As Color = Me.spriteRenderer.color
		While t < time
			Dim val As Single = t / time
			Me.SetColor(Color.Lerp(startColor, color, val))
			t += CupheadTime.Delta
			Yield Nothing
		End While
		Me.SetColor(color)
		Yield Nothing
		Return
	End Function

	' Token: 0x060040DE RID: 16606 RVA: 0x0023401C File Offset: 0x0023241C
	Private Sub OnParryStart()
		If Me.isStoned Then
			Me.Breakout()
		End If
		MyBase.animator.SetBool("ParrySuccess", False)
		MyBase.animator.SetBool("ParryPlusCharm", MyBase.player.stats.Loadout.charm = Charm.charm_parry_plus)
		If MyBase.player.stats.Loadout.charm = Charm.charm_parry_attack OrElse MyBase.player.stats.CurseWhetsone Then
			MyBase.animator.Play("ParryAttack")
		Else
			MyBase.animator.Play("Parry")
		End If
	End Sub

	' Token: 0x060040DF RID: 16607 RVA: 0x002340D0 File Offset: 0x002324D0
	Private Sub OnParrySuccess()
		MyBase.animator.SetBool("ParrySuccess", True)
	End Sub

	' Token: 0x060040E0 RID: 16608 RVA: 0x002340E4 File Offset: 0x002324E4
	Private Sub OnDeath(playerId As PlayerId)
		For Each planePlayerDeathPart As PlanePlayerDeathPart In Me.deathPieces
			planePlayerDeathPart.CreatePart(MyBase.player.id, MyBase.transform.position)
		Next
		Me.deathEffect.Create(MyBase.transform.position)
	End Sub

	' Token: 0x060040E1 RID: 16609 RVA: 0x00234144 File Offset: 0x00232544
	Private Sub OnRevive(pos As Vector3)
		Me.SetAlpha(1F)
	End Sub

	' Token: 0x060040E2 RID: 16610 RVA: 0x00234151 File Offset: 0x00232551
	Private Sub SetInteger([integer] As String, value As Integer)
		MyBase.animator.SetInteger([integer], value)
	End Sub

	' Token: 0x060040E3 RID: 16611 RVA: 0x00234160 File Offset: 0x00232560
	Private Sub SetTrigger(trigger As String)
		MyBase.animator.SetTrigger(trigger)
	End Sub

	' Token: 0x0400475B RID: 18267
	Private Const PALADIN_SHADOW_BUFFER_SIZE As Integer = 10

	' Token: 0x0400475C RID: 18268
	Private Const ROTATION_MAX As Single = 9F

	' Token: 0x0400475D RID: 18269
	Private Const SHUNK_ROTATION_ADD As Single = 5F

	' Token: 0x0400475E RID: 18270
	Private Const ROTATION_SPEED As Single = 7F

	' Token: 0x0400475F RID: 18271
	Private Const INTRO_X As Single = -150F

	' Token: 0x04004760 RID: 18272
	Private Const PUFF_DELAY As Single = 0.17F

	' Token: 0x04004761 RID: 18273
	Private Const PUFF_DELAY_MOVING As Single = 0.07F

	' Token: 0x04004762 RID: 18274
	Private Shared PUFF_OFFSET As Vector2 = New Vector3(-50F, 0F)

	' Token: 0x04004763 RID: 18275
	Private Const SHRINK_COOLDOWN As Single = 0.23300001F

	' Token: 0x04004764 RID: 18276
	<SerializeField()>
	Private cuphead As Transform

	' Token: 0x04004765 RID: 18277
	<SerializeField()>
	Private mugman As Transform

	' Token: 0x04004766 RID: 18278
	<SerializeField()>
	Private chalice As Transform

	' Token: 0x04004767 RID: 18279
	<Space(10F)>
	<SerializeField()>
	Private introRoot As Transform

	' Token: 0x04004768 RID: 18280
	<Space(10F)>
	<SerializeField()>
	Private breakoutPrefab As Effect

	' Token: 0x04004769 RID: 18281
	<SerializeField()>
	Private poofPrefab As Effect

	' Token: 0x0400476A RID: 18282
	<SerializeField()>
	Private greenPrefab As Effect

	' Token: 0x0400476B RID: 18283
	<SerializeField()>
	Private puffPrefab As PlaneLevelEffect

	' Token: 0x0400476C RID: 18284
	<Space(10F)>
	<SerializeField()>
	Private hitSparkEffect As Effect

	' Token: 0x0400476D RID: 18285
	<SerializeField()>
	Private hitDustEffect As Effect

	' Token: 0x0400476E RID: 18286
	<SerializeField()>
	Private smokeDashEffect As Effect

	' Token: 0x0400476F RID: 18287
	<SerializeField()>
	Private healerCharmEffect As HealerCharmSparkEffect

	' Token: 0x04004770 RID: 18288
	<SerializeField()>
	Private curseEffect As Effect

	' Token: 0x04004771 RID: 18289
	<Space(10F)>
	<SerializeField()>
	Private shrinkEffect As Effect

	' Token: 0x04004772 RID: 18290
	<SerializeField()>
	Private growEffect As Effect

	' Token: 0x04004773 RID: 18291
	<Space(10F)>
	<SerializeField()>
	Private deathPieces As PlanePlayerDeathPart()

	' Token: 0x04004774 RID: 18292
	<SerializeField()>
	Private deathEffect As PlaneLevelEffect

	' Token: 0x04004775 RID: 18293
	Private playerSprite As Transform

	' Token: 0x04004777 RID: 18295
	Private rotation As Single

	' Token: 0x04004778 RID: 18296
	Private shrinkCooldownTimeLeft As Single

	' Token: 0x04004779 RID: 18297
	Private tempMaterial As Material

	' Token: 0x0400477C RID: 18300
	Private isStoned As Boolean

	' Token: 0x0400477D RID: 18301
	<SerializeField()>
	Private curseEffectDelay As Single = 0.15F

	' Token: 0x0400477E RID: 18302
	<SerializeField()>
	Private curseAngleShiftRange As MinMax = New MinMax(60F, 300F)

	' Token: 0x0400477F RID: 18303
	<SerializeField()>
	Private curseDistanceRange As MinMax = New MinMax(0F, 20F)

	' Token: 0x04004780 RID: 18304
	Private curseEffectAngle As Single

	' Token: 0x04004781 RID: 18305
	Private curseEffectTimer As Single

	' Token: 0x04004782 RID: 18306
	Private curseCharmLevel As Integer = -1

	' Token: 0x04004783 RID: 18307
	Private paladinShadowPosition As Vector3()

	' Token: 0x04004784 RID: 18308
	Private paladinShadowScale As Vector3()

	' Token: 0x04004785 RID: 18309
	Private paladinShadowSprite As Sprite()

	' Token: 0x04004786 RID: 18310
	<SerializeField()>
	Private paladinShadows As SpriteRenderer()

	' Token: 0x04004787 RID: 18311
	Private showCurseFX As Boolean

	' Token: 0x0400478A RID: 18314
	Private colorCoroutine As IEnumerator

	' Token: 0x02000A92 RID: 2706
	Public Enum ShrinkStates
		' Token: 0x0400478C RID: 18316
		Ready
		' Token: 0x0400478D RID: 18317
		Shrunk
		' Token: 0x0400478E RID: 18318
		Cooldown
	End Enum

	' Token: 0x02000A93 RID: 2707
	Private Enum AnimLayers
		' Token: 0x04004790 RID: 18320
		Base
		' Token: 0x04004791 RID: 18321
		Shrunk
		' Token: 0x04004792 RID: 18322
		Bomb
	End Enum
End Class
