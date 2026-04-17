Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000A11 RID: 2577
Public Class LevelPlayerAnimationController
	Inherits AbstractLevelPlayerComponent

	' Token: 0x17000527 RID: 1319
	' (get) Token: 0x06003CE5 RID: 15589 RVA: 0x0021AE8F File Offset: 0x0021928F
	' (set) Token: 0x06003CE6 RID: 15590 RVA: 0x0021AE97 File Offset: 0x00219297
	Public Property spriteRenderer As SpriteRenderer

	' Token: 0x06003CE7 RID: 15591 RVA: 0x0021AEA0 File Offset: 0x002192A0
	Private Sub Start()
		If Not Me.chaliceActivated Then
			MyBase.animator.SetLayerWeight(3, 0F)
			MyBase.animator.SetLayerWeight(4, 0F)
		End If
		AddHandler MyBase.basePlayer.OnPlayIntroEvent, AddressOf Me.PlayIntro
		AddHandler MyBase.basePlayer.OnPlatformingLevelAwakeEvent, AddressOf Me.CheckIfChaliceAndActivate
		AddHandler MyBase.player.motor.OnParryEvent, AddressOf Me.OnParryStart
		AddHandler MyBase.player.motor.OnGroundedEvent, AddressOf Me.OnGrounded
		AddHandler MyBase.player.motor.OnDashStartEvent, AddressOf Me.OnDashStart
		AddHandler MyBase.player.motor.OnDashEndEvent, AddressOf Me.OnDashEnd
		AddHandler MyBase.player.motor.OnDoubleJumpEvent, AddressOf Me.ChaliceDoubleJumpFX
		AddHandler MyBase.player.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
		AddHandler MyBase.player.weaponManager.OnExStart, AddressOf Me.OnEx
		AddHandler MyBase.player.weaponManager.OnSuperStart, AddressOf Me.OnSuper
		AddHandler MyBase.player.weaponManager.OnSuperEnd, AddressOf Me.OnSuperEnd
		AddHandler MyBase.player.weaponManager.OnWeaponFire, AddressOf Me.OnShotFired
		AddHandler LevelPauseGUI.OnPauseEvent, AddressOf Me.OnGuiPause
		AddHandler LevelPauseGUI.OnPauseEvent, AddressOf Me.OnGuiUnpause
		Me.lastTrueLookDir = MyBase.player.motor.TrueLookDirection
		Me.SetBool(LevelPlayerAnimationController.Booleans.HasParryCharm, MyBase.player.stats.Loadout.charm = Charm.charm_parry_plus AndAlso Not Level.IsChessBoss)
		PlayerRecolorHandler.SetChaliceRecolorEnabled(Me.chalice.GetComponent(Of SpriteRenderer)().sharedMaterial, SettingsData.Data.filter = BlurGamma.Filter.Chalice)
		If MyBase.player.stats.Loadout.charm = Charm.charm_curse Then
			Me.curseCharmLevel = CharmCurse.CalculateLevel(MyBase.player.id)
		End If
		If Level.Current.LevelType <> Level.Type.Platforming Then
			If MyBase.player.stats.isChalice Then
				If(Level.IsDicePalace AndAlso Not DicePalaceMainLevelGameInfo.IS_FIRST_ENTRY) OrElse SceneLoader.CurrentLevel = Levels.Kitchen OrElse SceneLoader.CurrentLevel = Levels.ChaliceTutorial Then
					Me.CheckIfChaliceAndActivate()
					RemoveHandler MyBase.basePlayer.OnPlayIntroEvent, AddressOf Me.PlayIntro
				ElseIf SceneLoader.CurrentLevel <> Levels.Devil AndAlso SceneLoader.CurrentLevel <> Levels.Saltbaker Then
					Me.StartChaliceIntroHold(False)
				End If
			ElseIf MyBase.player.stats.Loadout.charm = Charm.charm_chalice AndAlso SceneLoader.CurrentLevel <> Levels.Devil AndAlso SceneLoader.CurrentLevel <> Levels.Saltbaker AndAlso SceneLoader.CurrentLevel <> Levels.Kitchen AndAlso SceneLoader.CurrentLevel <> Levels.ChaliceTutorial AndAlso (Not Level.IsDicePalace OrElse DicePalaceMainLevelGameInfo.IS_FIRST_ENTRY) Then
				Me.StartChaliceIntroHold(True)
			End If
		End If
		If SceneLoader.CurrentLevel = Levels.ChaliceTutorial Then
			Me.spriteRenderer.gameObject.layer = 31
			For Each spriteRenderer As SpriteRenderer In Me.chaliceSprites
				spriteRenderer.gameObject.layer = 31
			Next
			For Each spriteRenderer2 As SpriteRenderer In Me.chaliceJumpShootRenderers
				spriteRenderer2.gameObject.layer = 31
			Next
		End If
	End Sub

	' Token: 0x06003CE8 RID: 15592 RVA: 0x0021B284 File Offset: 0x00219684
	Private Sub OnEnable()
		MyBase.StartCoroutine(Me.flash_cr())
	End Sub

	' Token: 0x06003CE9 RID: 15593 RVA: 0x0021B294 File Offset: 0x00219694
	Private Sub OnDisable()
		If Me.paladinShadows(0) Then
			Me.paladinShadows(0).enabled = False
		End If
		If Me.paladinShadows(1) Then
			Me.paladinShadows(1).enabled = False
		End If
	End Sub

	' Token: 0x06003CEA RID: 15594 RVA: 0x0021B2E4 File Offset: 0x002196E4
	Private Sub Update()
		If MyBase.player.IsDead OrElse Not MyBase.player.levelStarted Then
			Return
		End If
		If Me.curseCharmLevel > -1 AndAlso Not Me.showCurseFX AndAlso Not Level.IsChessBoss Then
			Me.InitializeCurseFX()
			Me.showCurseFX = True
		End If
		If MyBase.player.stats.isChalice AndAlso Me.chaliceActivated Then
			Me.ChaliceAimSpriteHandling()
			Me.ChaliceJumpHandling()
			Me.ChaliceJumpShootHandling()
			If Not MyBase.player.motor.Dashing Then
				MyBase.animator.SetLayerWeight(3, 1F)
				If Me.chaliceInvincibleSparklesCoroutine IsNot Nothing Then
					MyBase.StopCoroutine(Me.chaliceInvincibleSparklesCoroutine)
					Me.chaliceInvincibleSparklesCoroutine = Nothing
				End If
			End If
		End If
		If Me.curseCharmLevel > -1 Then
			Me.HandleCurseFX()
		End If
		If Not Me.hitAnimation AndAlso MyBase.player.motor.LookDirection.x <> 0 AndAlso Me.lastTrueLookDir.x <> MyBase.player.motor.TrueLookDirection.x Then
			Me.SetBool(LevelPlayerAnimationController.Booleans.Turning, True)
		Else
			Me.SetBool(LevelPlayerAnimationController.Booleans.Turning, False)
		End If
		Me.lastTrueLookDir = MyBase.player.motor.TrueLookDirection
		Me.SetBool(LevelPlayerAnimationController.Booleans.Grounded, MyBase.player.motor.Grounded)
		Me.SetBool(LevelPlayerAnimationController.Booleans.Locked, MyBase.player.motor.Locked)
		If MyBase.player.motor.Locked Then
			Me.SetInt(LevelPlayerAnimationController.Integers.MoveX, 0)
		Else
			Me.SetInt(LevelPlayerAnimationController.Integers.MoveX, MyBase.player.motor.LookDirection.x)
		End If
		If MyBase.player.motor.Ducking OrElse MyBase.player.motor.IsUsingSuperOrEx Then
			Me.SetInt(LevelPlayerAnimationController.Integers.MoveY, 0)
			Me.SetBool(LevelPlayerAnimationController.Booleans.ChaliceOffIdle, True)
		Else
			Me.SetInt(LevelPlayerAnimationController.Integers.MoveY, MyBase.player.motor.MoveDirection.y)
			Me.SetBool(LevelPlayerAnimationController.Booleans.ChaliceOffIdle, False)
		End If
		Me.SetInt(LevelPlayerAnimationController.Integers.LookX, MyBase.player.motor.LookDirection.x)
		Me.SetInt(LevelPlayerAnimationController.Integers.LookY, MyBase.player.motor.LookDirection.y)
		Me.SetBool(LevelPlayerAnimationController.Booleans.Shooting, MyBase.player.weaponManager.IsShooting)
		Dim num As Single = If((Not MyBase.player.weaponManager.IsShooting AndAlso Me.timeSinceStoppedShooting >= 0.0833F), 0F, 1F)
		If Not MyBase.player.stats.isChalice Then
			MyBase.animator.SetLayerWeight(1, num)
			MyBase.animator.SetLayerWeight(2, If((MyBase.player.motor.LookDirection.y <= 0), 0F, num))
		Else
			If Not MyBase.player.motor.Grounded AndAlso MyBase.animator.GetBool(LevelPlayerAnimationController.Booleans.ChaliceAirEX) Then
				num = 0F
			End If
			If Not Me.ExitingChaliceSuper() Then
				MyBase.animator.SetLayerWeight(4, 1F - num)
			Else
				MyBase.animator.SetLayerWeight(4, 0F)
			End If
			MyBase.animator.SetLayerWeight(5, num)
			MyBase.animator.SetLayerWeight(6, If((MyBase.player.motor.LookDirection.y <= 0), 0F, num))
			If MyBase.player.motor.ChaliceDuckDashed AndAlso Not MyBase.player.motor.Grounded Then
				Me.chaliceFellFromDuckDash = True
			End If
			If MyBase.player.motor.Grounded Then
				Me.chaliceFellFromDuckDash = False
			End If
		End If
		If Me.shooting Then
			Me.timeSinceStoppedShooting = 0F
		Else
			Me.timeSinceStoppedShooting += CupheadTime.Delta
		End If
		Dim flag As Boolean = False
		If Me.fired AndAlso ((MyBase.player.motor.Grounded AndAlso (MyBase.player.motor.LookDirection.x = 0 OrElse MyBase.player.motor.Locked OrElse MyBase.player.motor.LookDirection.y < 0)) OrElse (MyBase.player.stats.isChalice AndAlso Not MyBase.player.motor.ChaliceDoubleJumped)) Then
			Me.SetTrigger(LevelPlayerAnimationController.Triggers.OnFire)
			flag = True
		End If
		Me.fired = False
		Me.shooting = MyBase.player.weaponManager.IsShooting
		If Not Me.shooting AndAlso Not flag Then
			Me.ResetTrigger(LevelPlayerAnimationController.Triggers.OnFire)
		End If
		If MyBase.player.motor.Dashing AndAlso Me.GetBool(LevelPlayerAnimationController.Booleans.Dashing) <> MyBase.player.motor.Dashing Then
			If MyBase.player.stats.isChalice Then
				MyBase.animator.SetLayerWeight(3, 0F)
			End If
			If MyBase.player.stats.isChalice AndAlso MyBase.player.motor.Ducking Then
				Me.ChaliceDuckDashHandling()
			Else
				Me.Play("Dash.Air")
				If MyBase.player.stats.Loadout.charm <> Charm.charm_smoke_dash OrElse Not MyBase.player.stats.CurseSmokeDash OrElse Level.IsChessBoss OrElse (MyBase.player.stats.isChalice AndAlso Not MyBase.player.motor.Ducking) Then
					Me.dashEffect.Create(MyBase.transform.position, MyBase.transform.localScale)
				End If
				If MyBase.player.stats.isChalice Then
					Me.chaliceDashEffectActive = Me.chaliceDashEffect.Create(MyBase.transform.position, MyBase.transform.localScale)
					Me.chaliceDashEffectActive.transform.parent = MyBase.transform
				End If
			End If
		End If
		Me.SetBool(LevelPlayerAnimationController.Booleans.Dashing, MyBase.player.motor.Dashing)
		If Not MyBase.player.motor.Dashing Then
			If MyBase.player.motor.LookDirection.x <> 0 AndAlso Not Me.ExitingChaliceSuper() Then
				MyBase.transform.SetScale(New Single?(MyBase.player.motor.LookDirection.x), Nothing, Nothing)
			End If
		Else
			MyBase.transform.SetScale(New Single?(CSng(MyBase.player.motor.DashDirection)), Nothing, Nothing)
		End If
	End Sub

	' Token: 0x06003CEB RID: 15595 RVA: 0x0021BAD1 File Offset: 0x00219ED1
	Public Sub ResetMoveX()
		Me.SetInt(LevelPlayerAnimationController.Integers.MoveX, 0)
		Me.inScaredIntro = False
	End Sub

	' Token: 0x06003CEC RID: 15596 RVA: 0x0021BAE8 File Offset: 0x00219EE8
	Private Sub ChaliceDoubleJumpFX()
		Dim num As Single = 0F
		If MyBase.player.input.GetAxis(PlayerInput.Axis.X) > 0F OrElse (MyBase.player.input.GetAxis(PlayerInput.Axis.X) > 0F AndAlso MyBase.player.input.GetAxis(PlayerInput.Axis.Y) > 0F) Then
			num = -35F
		ElseIf MyBase.player.input.GetAxis(PlayerInput.Axis.X) < 0F OrElse (MyBase.player.input.GetAxis(PlayerInput.Axis.X) < 0F AndAlso MyBase.player.input.GetAxis(PlayerInput.Axis.Y) > 0F) Then
			num = 35F
		End If
		Dim effect As Effect = Me.chaliceDoubleJumpEffect.Create(MyBase.transform.position)
		effect.transform.SetEulerAngles(Nothing, Nothing, New Single?(num))
	End Sub

	' Token: 0x06003CED RID: 15597 RVA: 0x0021BBE8 File Offset: 0x00219FE8
	Private Sub ChaliceIncrementJumpDescendLoopCounter()
		If MyBase.player.motor.MoveDirection.y < 0 Then
			Me.SetInt(LevelPlayerAnimationController.Integers.ChaliceJumpDescendLoopCounter, Me.GetInt(LevelPlayerAnimationController.Integers.ChaliceJumpDescendLoopCounter) + 1)
		End If
	End Sub

	' Token: 0x06003CEE RID: 15598 RVA: 0x0021BC30 File Offset: 0x0021A030
	Private Sub ChaliceResetJumpDescendLoopCounter()
		Me.SetInt(LevelPlayerAnimationController.Integers.ChaliceJumpDescendLoopCounter, 0)
	End Sub

	' Token: 0x06003CEF RID: 15599 RVA: 0x0021BC40 File Offset: 0x0021A040
	Private Sub ChaliceDuckDashHandling()
		Me.Play("Duck.Duck_Dash")
		AudioManager.Play("chalice_roll")
		If Me.chaliceInvincibleSparklesCoroutine IsNot Nothing Then
			MyBase.StopCoroutine(Me.chaliceInvincibleSparklesCoroutine)
			Me.chaliceInvincibleSparklesCoroutine = Nothing
		End If
		Me.chaliceInvincibleSparklesCoroutine = MyBase.StartCoroutine(Me.chaliceInvincibleSparkle_cr())
	End Sub

	' Token: 0x06003CF0 RID: 15600 RVA: 0x0021BC94 File Offset: 0x0021A094
	Private Iterator Function chaliceInvincibleSparkle_cr() As IEnumerator
		While True
			Dim x As Single = Global.UnityEngine.Random.Range(-MyBase.player.colliderManager.Width, MyBase.player.colliderManager.Width)
			Dim y As Single = Global.UnityEngine.Random.Range(MyBase.player.colliderManager.Height * -0.5F, MyBase.player.colliderManager.Height * 1.5F)
			Me.chaliceDuckDashSparkles.Create(MyBase.player.transform.position + New Vector3(x, y, 0F))
			Yield CupheadTime.WaitForSeconds(Me, 0.05F)
		End While
		Return
	End Function

	' Token: 0x06003CF1 RID: 15601 RVA: 0x0021BCAF File Offset: 0x0021A0AF
	Private Sub ChaliceJumpHandling()
		Me.SetBool(LevelPlayerAnimationController.Booleans.DoubleJump, MyBase.player.motor.ChaliceDoubleJumped)
	End Sub

	' Token: 0x06003CF2 RID: 15602 RVA: 0x0021BCCC File Offset: 0x0021A0CC
	Private Sub ChaliceJumpShootHandling()
		Dim flag As Boolean = (MyBase.player.weaponManager.IsShooting OrElse Me.timeSinceStoppedShooting < 0.0833F) AndAlso Not MyBase.player.motor.Grounded AndAlso Not MyBase.player.motor.Dashing AndAlso Not MyBase.player.motor.ChaliceDoubleJumped AndAlso Not Me.chaliceFellFromDuckDash AndAlso Not Me.GetBool(LevelPlayerAnimationController.Booleans.ChaliceAirEX) AndAlso Not Me.hitAnimation AndAlso Not Me.super
		Me.chaliceJumpShootRenderers(0).enabled = flag
		Me.chaliceJumpShootRenderers(1).enabled = flag
		If Not MyBase.player.motor.Grounded Then
			Me.spriteRenderer.enabled = Not MyBase.player.weaponManager.IsShooting AndAlso Me.timeSinceStoppedShooting >= 0.0833F
			If MyBase.player.motor.ChaliceDoubleJumped OrElse Me.chaliceFellFromDuckDash OrElse MyBase.player.motor.Dashing OrElse Me.GetBool(LevelPlayerAnimationController.Booleans.ChaliceAirEX) OrElse Me.hitAnimation Then
				Me.spriteRenderer.enabled = True
			End If
		End If
	End Sub

	' Token: 0x06003CF3 RID: 15603 RVA: 0x0021BE30 File Offset: 0x0021A230
	Private Sub ChaliceAimSpriteHandling()
		If MyBase.player.motor.Locked Then
			Me.SetInt(LevelPlayerAnimationController.Integers.MoveX, 0)
		Else
			Me.SetInt(LevelPlayerAnimationController.Integers.MoveX, MyBase.player.motor.LookDirection.x)
		End If
		If MyBase.player.weaponManager.IsShooting OrElse Me.GetBool(LevelPlayerAnimationController.Booleans.ChaliceOffIdle) OrElse (Me.GetInt(LevelPlayerAnimationController.Integers.MoveX) <> 0 AndAlso Not MyBase.player.motor.Dashing) OrElse MyBase.player.motor.Dashing OrElse MyBase.player.motor.DashState = LevelPlayerMotor.DashManager.State.[End] OrElse Not MyBase.player.motor.Grounded OrElse Me.inScaredIntro Then
			Me.SwitchChaliceAim(-1)
			Me.spriteRenderer.enabled = True
		ElseIf MyBase.player.motor.LookDirection.x <> 0 Then
			Me.SwitchChaliceAim(2)
			Me.spriteRenderer.enabled = False
			If MyBase.player.motor.LookDirection.y > 0 Then
				Me.SwitchChaliceAim(1)
				Me.spriteRenderer.enabled = False
			ElseIf MyBase.player.motor.LookDirection.y < 0 Then
				Me.SwitchChaliceAim(3)
				Me.spriteRenderer.enabled = False
			End If
		ElseIf MyBase.player.motor.LookDirection.y > 0 Then
			Me.SwitchChaliceAim(0)
			Me.spriteRenderer.enabled = False
		ElseIf MyBase.player.motor.LookDirection.y < 0 Then
			Me.SwitchChaliceAim(4)
			Me.spriteRenderer.enabled = False
		Else
			Me.SwitchChaliceAim(-1)
			Me.spriteRenderer.enabled = True
		End If
	End Sub

	' Token: 0x06003CF4 RID: 15604 RVA: 0x0021C070 File Offset: 0x0021A470
	Private Sub SwitchChaliceAim(spriteToEnable As Integer)
		For i As Integer = 0 To Me.chaliceSprites.Length - 1
			Me.chaliceSprites(i).enabled = i = spriteToEnable
		Next
	End Sub

	' Token: 0x06003CF5 RID: 15605 RVA: 0x0021C0A8 File Offset: 0x0021A4A8
	Private Sub ChaliceEndAirEX()
		Me.SetBool(LevelPlayerAnimationController.Booleans.ChaliceAirEX, False)
		If MyBase.player.stats.isChalice AndAlso Not MyBase.player.motor.Grounded Then
			Dim text As String = Me.exDirection
			If text IsNot Nothing Then
				If Not(text = "Forward") Then
					If Not(text = "Up") AndAlso Not(text = "Down") AndAlso Not(text = "Diagonal_Down") Then
						If text = "Diagonal_Up" Then
							MyBase.animator.Play(Me.ChaliceAirEXRecovery, 3, 0F)
						End If
					Else
						MyBase.animator.Play(Me.ChaliceAirEXRecovery, 3, 0.041666668F)
					End If
				Else
					MyBase.animator.Play(Me.ChaliceAirEXRecovery, 3, 0.083333336F)
				End If
			End If
		End If
	End Sub

	' Token: 0x06003CF6 RID: 15606 RVA: 0x0021C1A4 File Offset: 0x0021A5A4
	Public Sub IsIntroB()
		If Not MyBase.player.stats.isChalice Then
			Me.isIntroB = True
			If(MyBase.player.id = PlayerId.PlayerOne AndAlso PlayerManager.player1IsMugman) OrElse (MyBase.player.id = PlayerId.PlayerTwo AndAlso Not PlayerManager.player1IsMugman) Then
				Me.Play("Boil_Mugman")
			End If
		End If
	End Sub

	' Token: 0x06003CF7 RID: 15607 RVA: 0x0021C210 File Offset: 0x0021A610
	Public Sub CookieFail()
		If Level.Current.CurrentLevel = Levels.Bee AndAlso MyBase.player.id = PlayerId.PlayerTwo Then
			MyBase.transform.position += Vector3.left * 32F
		End If
		Dim text As String = If(((MyBase.player.id <> PlayerId.PlayerOne OrElse Not PlayerManager.player1IsMugman) AndAlso (MyBase.player.id <> PlayerId.PlayerTwo OrElse PlayerManager.player1IsMugman)), "Cuphead", "Mugman")
		Me.Play("Intro_Chalice_" + text + "_Fail")
	End Sub

	' Token: 0x06003CF8 RID: 15608 RVA: 0x0021C2C4 File Offset: 0x0021A6C4
	Public Sub ScaredChalice(showPortal As Boolean)
		Me.SetInt(LevelPlayerAnimationController.Integers.MoveX, 0)
		Me.inScaredIntro = True
		Me.ActivateChaliceAnimationLayers()
		MyBase.animator.Play("Intro_Chalice_Scared", 3)
		If Not showPortal Then
			Return
		End If
		Dim flag As Boolean = (MyBase.player.id = PlayerId.PlayerOne AndAlso PlayerManager.player1IsMugman) OrElse (MyBase.player.id = PlayerId.PlayerTwo AndAlso Not PlayerManager.player1IsMugman)
		Dim text As String = If((Not flag), "Cuphead", "Mugman")
		Me.chaliceIntroAnimation.Create(MyBase.transform.position, flag, True)
	End Sub

	' Token: 0x06003CF9 RID: 15609 RVA: 0x0021C36B File Offset: 0x0021A76B
	Public Sub ForceDirection()
		Me.lastTrueLookDir = MyBase.player.motor.TrueLookDirection
	End Sub

	' Token: 0x06003CFA RID: 15610 RVA: 0x0021C384 File Offset: 0x0021A784
	Private Sub InitializeCurseFX()
		Me.curseEffectAngle = CSng(Global.UnityEngine.Random.Range(0, 360))
		If Me.curseCharmLevel = 4 AndAlso Me.paladinShadows IsNot Nothing Then
			Me.paladinShadowPosition = New Vector3(9) {}
			Me.paladinShadowScale = New Vector3(9) {}
			Me.paladinShadowSprite = New Sprite(9) {}
			For i As Integer = 0 To 10 - 1
				Me.paladinShadowPosition(i) = MyBase.transform.position
				Me.paladinShadowSprite(i) = Me.spriteRenderer.sprite
				Me.paladinShadowScale(i) = MyBase.transform.localScale
			Next
			Me.paladinShadows(0).transform.position = MyBase.transform.position
			Me.paladinShadows(1).transform.position = MyBase.transform.position
			Me.paladinShadows(0).sprite = Me.spriteRenderer.sprite
			Me.paladinShadows(1).sprite = Me.spriteRenderer.sprite
			Me.paladinShadows(0).enabled = True
			Me.paladinShadows(1).enabled = True
			Me.paladinShadows(0).transform.parent = Nothing
			Me.paladinShadows(1).transform.parent = Nothing
		End If
	End Sub

	' Token: 0x06003CFB RID: 15611 RVA: 0x0021C4EC File Offset: 0x0021A8EC
	Private Sub HandleCurseFX()
		If PauseManager.state = PauseManager.State.Paused OrElse Not Me.showCurseFX Then
			Return
		End If
		Me.curseEffectTimer += CupheadTime.Delta
		While Me.curseEffectTimer >= Me.curseEffectDelay
			Dim effect As Effect = Me.curseEffect.Create(MyBase.player.center + MathUtils.AngleToDirection(Me.curseEffectAngle) * Me.curseDistanceRange.RandomFloat())
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
			Me.paladinShadows(0).enabled = Not MyBase.player.motor.Dashing
			Me.paladinShadows(1).enabled = Not MyBase.player.motor.Dashing
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

	' Token: 0x06003CFC RID: 15612 RVA: 0x0021C87F File Offset: 0x0021AC7F
	Public Sub UpdateAnimator()
		Me.Update()
	End Sub

	' Token: 0x06003CFD RID: 15613 RVA: 0x0021C887 File Offset: 0x0021AC87
	Public Overrides Sub OnPause()
		MyBase.OnPause()
		Me.SetAlpha(1F)
	End Sub

	' Token: 0x06003CFE RID: 15614 RVA: 0x0021C89A File Offset: 0x0021AC9A
	Private Sub OnGuiPause()
	End Sub

	' Token: 0x06003CFF RID: 15615 RVA: 0x0021C89C File Offset: 0x0021AC9C
	Private Sub OnGuiUnpause()
	End Sub

	' Token: 0x06003D00 RID: 15616 RVA: 0x0021C89E File Offset: 0x0021AC9E
	Public Sub OnShotFired()
		Me.fired = True
	End Sub

	' Token: 0x06003D01 RID: 15617 RVA: 0x0021C8A7 File Offset: 0x0021ACA7
	Public Sub OnRevive(pos As Vector3)
		MyBase.animator.Play("Jump")
	End Sub

	' Token: 0x06003D02 RID: 15618 RVA: 0x0021C8BC File Offset: 0x0021ACBC
	Public Sub OnGravityReversed()
		MyBase.transform.SetScale(Nothing, New Single?(MyBase.player.motor.GravityReversalMultiplier), Nothing)
	End Sub

	' Token: 0x06003D03 RID: 15619 RVA: 0x0021C8FB File Offset: 0x0021ACFB
	Public Overrides Sub OnLevelStart()
		Me.CheckIfChaliceAndActivate()
	End Sub

	' Token: 0x06003D04 RID: 15620 RVA: 0x0021C903 File Offset: 0x0021AD03
	Public Sub OnLevelWin()
		MyBase.player.damageReceiver.OnWin()
		Me.SetTrigger(LevelPlayerAnimationController.Triggers.OnWin)
	End Sub

	' Token: 0x06003D05 RID: 15621 RVA: 0x0021C920 File Offset: 0x0021AD20
	Private Sub ActivateChaliceAnimationLayers()
		MyBase.animator.SetLayerWeight(3, 1F)
		MyBase.animator.SetLayerWeight(4, 1F)
		Me.SetChaliceSprites()
		Me.chaliceActivated = True
	End Sub

	' Token: 0x06003D06 RID: 15622 RVA: 0x0021C951 File Offset: 0x0021AD51
	Public Sub CheckIfChaliceAndActivate()
		If MyBase.player.stats.isChalice Then
			Me.ActivateChaliceAnimationLayers()
		End If
	End Sub

	' Token: 0x06003D07 RID: 15623 RVA: 0x0021C970 File Offset: 0x0021AD70
	Private Sub StartChaliceIntroHold(fail As Boolean)
		If Level.Current.Started OrElse Level.Current.blockChalice Then
			Return
		End If
		Dim flag As Boolean = (Not PlayerManager.player1IsMugman AndAlso MyBase.player.id = PlayerId.PlayerOne) OrElse (PlayerManager.player1IsMugman AndAlso MyBase.player.id <> PlayerId.PlayerOne)
		If fail Then
			MyBase.animator.Play(If((Not flag), "Intro_Chalice_Mugman_Fail_Start", "Intro_Chalice_Cuphead_Fail_Start"))
		Else
			MyBase.animator.Play("Intro_Chalice_Hold")
			Me.chaliceIntroCurrent = Me.chaliceIntroAnimation.Create(MyBase.transform.position + Vector3.down * MyBase.player.motor.DistanceToGround(), Not flag, False)
			Me.SetChaliceSprites()
		End If
	End Sub

	' Token: 0x06003D08 RID: 15624 RVA: 0x0021CA5C File Offset: 0x0021AE5C
	Public Sub PlayIntro()
		Me.SetBool(LevelPlayerAnimationController.Booleans.Intro, True)
		Dim flag As Boolean = (MyBase.player.id = PlayerId.PlayerOne AndAlso PlayerManager.player1IsMugman) OrElse (MyBase.player.id = PlayerId.PlayerTwo AndAlso Not PlayerManager.player1IsMugman)
		Dim text As String = If((Not flag), "Cuphead", "Mugman")
		If SceneLoader.CurrentLevel <> Levels.Devil AndAlso SceneLoader.CurrentLevel <> Levels.Saltbaker Then
			If MyBase.player.stats.isChalice Then
				MyBase.animator.Play("Idle", 0)
				MyBase.animator.Play("Intro_Chalice_" + text, 3)
				If Me.chaliceIntroCurrent Then
					Me.chaliceIntroCurrent.EndHold()
				End If
				Me.ActivateChaliceAnimationLayers()
			ElseIf MyBase.player.stats.Loadout.charm <> Charm.charm_chalice OrElse Level.Current.blockChalice Then
				Dim text2 As String = String.Empty
				text2 = If((Not Me.isIntroB), "Intro_", "Intro_B_")
				Me.Play(text2 + text)
			End If
		ElseIf Not MyBase.player.stats.isChalice Then
			If MyBase.player.id = PlayerId.PlayerOne Then
				AudioManager.Play("player_scared_intro")
			End If
			Me.inScaredIntro = True
			Me.Play("Intro_Scared")
		End If
	End Sub

	' Token: 0x06003D09 RID: 15625 RVA: 0x0021CBE8 File Offset: 0x0021AFE8
	Public Sub ScaredSprite(facingLeft As Boolean)
		MyBase.animator.enabled = False
		MyBase.enabled = False
		MyBase.player.motor.enabled = False
		If MyBase.player.id = PlayerId.PlayerOne Then
			Me.cuphead.GetComponent(Of SpriteRenderer)().sprite = Me.cupheadScaredSprite
			Me.cuphead.GetComponent(Of SpriteRenderer)().flipX = facingLeft
		Else
			Me.mugman.GetComponent(Of SpriteRenderer)().sprite = Me.mugmanScaredSprite
			Me.mugman.GetComponent(Of SpriteRenderer)().flipX = facingLeft
		End If
	End Sub

	' Token: 0x06003D0A RID: 15626 RVA: 0x0021CC7C File Offset: 0x0021B07C
	Public Sub LevelInit()
		Dim flag As Boolean = (Not PlayerManager.player1IsMugman AndAlso MyBase.player.id = PlayerId.PlayerOne) OrElse (PlayerManager.player1IsMugman AndAlso MyBase.player.id <> PlayerId.PlayerOne)
		Me.SetSprites(flag)
	End Sub

	' Token: 0x06003D0B RID: 15627 RVA: 0x0021CCCC File Offset: 0x0021B0CC
	Public Sub SetSprites(isCuphead As Boolean)
		Me.cuphead.SetActive(isCuphead)
		Me.mugman.SetActive(Not isCuphead)
		Me.chalice.SetActive(False)
		If isCuphead Then
			Me.spriteRenderer = Me.cuphead.GetComponent(Of SpriteRenderer)()
		Else
			Me.spriteRenderer = Me.mugman.GetComponent(Of SpriteRenderer)()
		End If
		Me.tempMaterial = Me.spriteRenderer.material
	End Sub

	' Token: 0x06003D0C RID: 15628 RVA: 0x0021CD3E File Offset: 0x0021B13E
	Private Sub SetChaliceSprites()
		Me.cuphead.SetActive(False)
		Me.mugman.SetActive(False)
		Me.chalice.SetActive(True)
		Me.spriteRenderer = Me.chalice.GetComponent(Of SpriteRenderer)()
	End Sub

	' Token: 0x06003D0D RID: 15629 RVA: 0x0021CD75 File Offset: 0x0021B175
	Public Sub EnableSpriteRenderer()
		Me.spriteRenderer.enabled = True
	End Sub

	' Token: 0x06003D0E RID: 15630 RVA: 0x0021CD84 File Offset: 0x0021B184
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		If MyBase.player.stats.SuperInvincible Then
			Return
		End If
		CupheadLevelCamera.Current.Shake(20F, 0.6F, False)
		If MyBase.player.stats.Health = 4 Then
			AudioManager.Play("player_damage_crack_level1")
		ElseIf MyBase.player.stats.Health = 3 Then
			AudioManager.Play("player_damage_crack_level2")
		ElseIf MyBase.player.stats.Health = 2 Then
			AudioManager.Play("player_damage_crack_level3")
		ElseIf MyBase.player.stats.Health = 1 Then
			AudioManager.Play("player_damage_crack_level4")
		End If
		AudioManager.Play("player_hit")
		Dim grounded As Boolean = MyBase.player.motor.Grounded
		If grounded Then
			Me.Play("Hit.Hit_Ground")
		Else
			Me.Play("Hit.Hit_Air")
		End If
		Me.hitAnimation = True
		Me.hitEffect.Create(MyBase.player.center, MyBase.transform.localScale)
	End Sub

	' Token: 0x06003D0F RID: 15631 RVA: 0x0021CEBA File Offset: 0x0021B2BA
	Public Sub OnHealerCharm()
		Me.healerCharmEffect.Create(MyBase.player.center, MyBase.transform.localScale, MyBase.player)
		AudioManager.Play("sfx_player_charmhealer_extraheart")
	End Sub

	' Token: 0x06003D10 RID: 15632 RVA: 0x0021CEF0 File Offset: 0x0021B2F0
	Private Sub OnDashStart()
		Me.hitAnimation = False
		If(MyBase.player.stats.Loadout.charm = Charm.charm_smoke_dash OrElse MyBase.player.stats.CurseSmokeDash) AndAlso Not Level.IsChessBoss Then
			Me.spriteRenderer.enabled = False
			Me.smokeDashEffect.Create(MyBase.player.center)
		End If
	End Sub

	' Token: 0x06003D11 RID: 15633 RVA: 0x0021CF68 File Offset: 0x0021B368
	Private Sub OnDashEnd()
		If(MyBase.player.stats.Loadout.charm = Charm.charm_smoke_dash OrElse MyBase.player.stats.CurseSmokeDash) AndAlso Not Level.IsChessBoss Then
			Me.spriteRenderer.enabled = True
			Me.smokeDashEffect.Create(MyBase.player.center)
		End If
		If Not MyBase.player.motor.Grounded AndAlso MyBase.player.stats.isChalice Then
			MyBase.animator.Play(If((Not MyBase.player.motor.ChaliceDoubleJumped), Me.ChaliceJumpDescend, Me.ChaliceJumpBall), 3, 0F)
		End If
	End Sub

	' Token: 0x06003D12 RID: 15634 RVA: 0x0021D037 File Offset: 0x0021B437
	Private Sub OnRunDust()
		If MyBase.enabled Then
			Me.runEffect.Create(Me.runDustRoot.position)
		End If
	End Sub

	' Token: 0x06003D13 RID: 15635 RVA: 0x0021D05B File Offset: 0x0021B45B
	Private Sub OnChaliceDashSparkle()
		If MyBase.enabled AndAlso MyBase.player.stats.isChalice Then
			Me.chaliceDashSparkle.Create(Me.sparkleRoot.position)
		End If
	End Sub

	' Token: 0x06003D14 RID: 15636 RVA: 0x0021D094 File Offset: 0x0021B494
	Private Sub OnBurst()
		Me.powerUpBurstEffect.Create(MyBase.player.center)
	End Sub

	' Token: 0x06003D15 RID: 15637 RVA: 0x0021D0AD File Offset: 0x0021B4AD
	Private Sub onHitAnimationComplete()
		Me.hitAnimation = False
	End Sub

	' Token: 0x06003D16 RID: 15638 RVA: 0x0021D0B6 File Offset: 0x0021B4B6
	Public Sub SetSpriteProperties(layer As SpriteLayer, order As Integer)
		Me.spriteRenderer.sortingLayerName = layer.ToString()
		Me.spriteRenderer.sortingOrder = order
	End Sub

	' Token: 0x06003D17 RID: 15639 RVA: 0x0021D0DC File Offset: 0x0021B4DC
	Public Sub ResetSpriteProperties()
		Me.spriteRenderer.sortingLayerName = SpriteLayer.Player.ToString()
		Me.spriteRenderer.sortingOrder = If((MyBase.player.id <> PlayerId.PlayerOne), (-1), 1)
	End Sub

	' Token: 0x06003D18 RID: 15640 RVA: 0x0021D128 File Offset: 0x0021B528
	Private Sub OnParryStart()
		If Me.super Then
			Return
		End If
		If MyBase.player.stats.Loadout.charm = Charm.charm_parry_plus AndAlso Not Level.IsChessBoss Then
			Me.SetBool(LevelPlayerAnimationController.Booleans.HasParryCharm, True)
		End If
		If(MyBase.player.stats.Loadout.charm = Charm.charm_parry_attack OrElse MyBase.player.stats.CurseWhetsone) AndAlso Not MyBase.GetComponent(Of IParryAttack)().AttackParryUsed AndAlso Not Level.IsChessBoss Then
			Me.SetBool(LevelPlayerAnimationController.Booleans.HasParryAttack, True)
		ElseIf MyBase.player.stats.Loadout.charm = Charm.charm_curse Then
			Me.SetBool(LevelPlayerAnimationController.Booleans.HasParryAttack, False)
		End If
		Me.SetTrigger(LevelPlayerAnimationController.Triggers.OnParry)
	End Sub

	' Token: 0x06003D19 RID: 15641 RVA: 0x0021D20C File Offset: 0x0021B60C
	Public Sub OnParrySuccess()
		If MyBase.player.stats.Loadout.charm = Charm.charm_parry_plus AndAlso Not Level.IsChessBoss Then
			Me.SetBool(LevelPlayerAnimationController.Booleans.HasParryCharm, False)
		End If
		If(MyBase.player.stats.Loadout.charm = Charm.charm_parry_attack OrElse MyBase.player.stats.CurseWhetsone) AndAlso Not Level.IsChessBoss Then
			Me.SetBool(LevelPlayerAnimationController.Booleans.HasParryAttack, False)
		End If
		Me.SetAlpha(1F)
		If MyBase.player.stats.isChalice Then
			If Me.chaliceDashEffectActive IsNot Nothing Then
				Global.UnityEngine.[Object].Destroy(Me.chaliceDashEffectActive.gameObject)
			End If
			MyBase.animator.Play("Jump_Launch", 3, 0F)
		End If
	End Sub

	' Token: 0x06003D1A RID: 15642 RVA: 0x0021D2EF File Offset: 0x0021B6EF
	Public Sub OnParryPause()
		If MyBase.gameObject.activeInHierarchy Then
			MyBase.animator.enabled = False
			Me.spriteRenderer.GetComponent(Of LevelPlayerParryAnimator)().StartSet()
		End If
	End Sub

	' Token: 0x06003D1B RID: 15643 RVA: 0x0021D31D File Offset: 0x0021B71D
	Public Sub OnParryAnimEnd()
		Me.ResumeNormanAnim()
	End Sub

	' Token: 0x06003D1C RID: 15644 RVA: 0x0021D325 File Offset: 0x0021B725
	Public Sub _ChaliceStartOnIdle4()
		If MyBase.player.stats.isChalice Then
			Me.SetBool(LevelPlayerAnimationController.Booleans.ChaliceOffIdle, False)
			MyBase.animator.Play("IdleFromFour", 3)
		End If
	End Sub

	' Token: 0x06003D1D RID: 15645 RVA: 0x0021D359 File Offset: 0x0021B759
	Public Sub ResumeNormanAnim()
		Me.spriteRenderer.GetComponent(Of LevelPlayerParryAnimator)().StopSet()
		MyBase.animator.enabled = True
	End Sub

	' Token: 0x06003D1E RID: 15646 RVA: 0x0021D377 File Offset: 0x0021B777
	Private Sub OnGrounded()
		If Not Level.Current.Started Then
			Return
		End If
		AudioManager.Play("player_grounded")
		Me.groundedEffect.Create(MyBase.transform.position, MyBase.transform.localScale)
	End Sub

	' Token: 0x06003D1F RID: 15647 RVA: 0x0021D3B8 File Offset: 0x0021B7B8
	Private Sub OnEx()
		If MyBase.player.stats.isChalice Then
			Me.SetBool(LevelPlayerAnimationController.Booleans.ChaliceOffIdle, True)
		End If
		Me.exDirection = "Forward"
		If MyBase.player.motor.LookDirection.x = 0 AndAlso MyBase.player.motor.LookDirection.y > 0 Then
			Me.exDirection = "Up"
			AudioManager.Play("player_ex_forward_ground")
		ElseIf MyBase.player.motor.LookDirection.x <> 0 AndAlso MyBase.player.motor.LookDirection.y > 0 Then
			Me.exDirection = "Diagonal_Up"
			AudioManager.Play("player_ex_forward_ground")
		ElseIf MyBase.player.motor.LookDirection.x = 0 AndAlso MyBase.player.motor.LookDirection.y < 0 Then
			Me.exDirection = "Down"
			AudioManager.Play("player_ex_forward_ground")
		ElseIf MyBase.player.motor.LookDirection.x <> 0 AndAlso MyBase.player.motor.LookDirection.y < 0 Then
			Me.exDirection = "Diagonal_Down"
			AudioManager.Play("player_ex_forward_ground")
		End If
		If Me.exDirection = "Forward" Then
			AudioManager.Play("player_ex_forward_ground")
		End If
		Dim text As String = "Ex." + Me.exDirection + "_"
		If MyBase.player.motor.Grounded Then
			text += "Ground"
		Else
			text += "Air"
		End If
		Me.Play(text)
		Me.SetBool(LevelPlayerAnimationController.Booleans.ChaliceAirEX, Not MyBase.player.motor.Grounded)
	End Sub

	' Token: 0x06003D20 RID: 15648 RVA: 0x0021D600 File Offset: 0x0021BA00
	Private Sub OnSuper()
		Dim super As Super = PlayerData.Data.Loadouts.GetPlayerLoadout(MyBase.player.id).super
		Me.super = True
		If MyBase.player.stats.isChalice Then
			Me.shooting = False
			Me.ChaliceJumpShootHandling()
		End If
		Me.spriteRenderer.enabled = False
		Me.SwitchChaliceAim(-1)
	End Sub

	' Token: 0x06003D21 RID: 15649 RVA: 0x0021D66C File Offset: 0x0021BA6C
	Private Sub OnSuperEnd()
		Me.super = False
		Me.spriteRenderer.enabled = True
		Me.ResetSpriteProperties()
		If MyBase.player.stats.isChalice Then
			Me.timeSinceStoppedShooting = 1F
			If MyBase.player.stats.Loadout.super = Super.level_super_chalice_shield Then
				MyBase.StartCoroutine(Me.end_chalice_super_cr(If((Not MyBase.player.motor.Grounded), Me.ChaliceSuper2ReturnAir, Me.ChaliceSuper2Return)))
			End If
			If MyBase.player.stats.Loadout.super = Super.level_super_chalice_vert_beam Then
				MyBase.StartCoroutine(Me.end_chalice_super_cr(Me.ChaliceSuper1Return))
			End If
		End If
	End Sub

	' Token: 0x06003D22 RID: 15650 RVA: 0x0021D738 File Offset: 0x0021BB38
	Private Function ExitingChaliceSuper() As Boolean
		Dim shortNameHash As Integer = MyBase.animator.GetCurrentAnimatorStateInfo(3).shortNameHash
		Return shortNameHash = Me.ChaliceSuper1Return OrElse shortNameHash = Me.ChaliceSuper2Return OrElse shortNameHash = Me.ChaliceSuper2ReturnAir
	End Function

	' Token: 0x06003D23 RID: 15651 RVA: 0x0021D780 File Offset: 0x0021BB80
	Private Iterator Function end_chalice_super_cr(animState As Integer) As IEnumerator
		MyBase.animator.Play(animState, 3, 0F)
		MyBase.animator.Update(0F)
		If MyBase.player.weaponManager.allowInput Then
			MyBase.player.weaponManager.DisableInput()
			While MyBase.animator.GetCurrentAnimatorStateInfo(3).shortNameHash = animState
				Yield Nothing
			End While
			MyBase.player.weaponManager.EnableInput()
		End If
		Return
	End Function

	' Token: 0x06003D24 RID: 15652 RVA: 0x0021D7A2 File Offset: 0x0021BBA2
	Private Sub _OnSuperAnimEnd()
		MyBase.player.UnpauseAll(False)
		MyBase.player.motor.OnSuperEnd()
	End Sub

	' Token: 0x06003D25 RID: 15653 RVA: 0x0021D7C0 File Offset: 0x0021BBC0
	Public Sub SetOldMaterial()
		Me.spriteRenderer.material = Me.tempMaterial
	End Sub

	' Token: 0x06003D26 RID: 15654 RVA: 0x0021D7D3 File Offset: 0x0021BBD3
	Public Sub SetMaterial(m As Material)
		Me.tempMaterial = Me.spriteRenderer.material
		Me.spriteRenderer.material = m
	End Sub

	' Token: 0x06003D27 RID: 15655 RVA: 0x0021D7F2 File Offset: 0x0021BBF2
	Public Function GetMaterial() As Material
		Return Me.spriteRenderer.material
	End Function

	' Token: 0x06003D28 RID: 15656 RVA: 0x0021D7FF File Offset: 0x0021BBFF
	Public Function GetSpriteRenderer() As SpriteRenderer
		Return Me.spriteRenderer
	End Function

	' Token: 0x06003D29 RID: 15657 RVA: 0x0021D807 File Offset: 0x0021BC07
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me.dashEffect = Nothing
		Me.groundedEffect = Nothing
		Me.hitEffect = Nothing
		Me.runEffect = Nothing
		Me.smokeDashEffect = Nothing
		Me.powerUpBurstEffect = Nothing
		Me.cupheadScaredSprite = Nothing
		Me.mugmanScaredSprite = Nothing
	End Sub

	' Token: 0x06003D2A RID: 15658 RVA: 0x0021D847 File Offset: 0x0021BC47
	Protected Sub Play(animation As String)
		MyBase.animator.Play(animation, 0, 0F)
	End Sub

	' Token: 0x06003D2B RID: 15659 RVA: 0x0021D85B File Offset: 0x0021BC5B
	Protected Function GetBool(b As Integer) As Boolean
		Return MyBase.animator.GetBool(b)
	End Function

	' Token: 0x06003D2C RID: 15660 RVA: 0x0021D869 File Offset: 0x0021BC69
	Protected Sub SetBool(b As Integer, value As Boolean)
		MyBase.animator.SetBool(b, value)
	End Sub

	' Token: 0x06003D2D RID: 15661 RVA: 0x0021D878 File Offset: 0x0021BC78
	Protected Function GetInt(i As Integer) As Integer
		Return MyBase.animator.GetInteger(i)
	End Function

	' Token: 0x06003D2E RID: 15662 RVA: 0x0021D886 File Offset: 0x0021BC86
	Protected Sub SetInt(i As Integer, value As Integer)
		MyBase.animator.SetInteger(i, value)
	End Sub

	' Token: 0x06003D2F RID: 15663 RVA: 0x0021D895 File Offset: 0x0021BC95
	Protected Sub SetTrigger(t As Integer)
		MyBase.animator.SetTrigger(t)
	End Sub

	' Token: 0x06003D30 RID: 15664 RVA: 0x0021D8A3 File Offset: 0x0021BCA3
	Protected Sub ResetTrigger(t As Integer)
		MyBase.animator.ResetTrigger(t)
	End Sub

	' Token: 0x06003D31 RID: 15665 RVA: 0x0021D8B4 File Offset: 0x0021BCB4
	Private Sub SetAlpha(a As Single)
		Dim color As Color = Me.spriteRenderer.color
		color.a = a
		Me.spriteRenderer.color = color
	End Sub

	' Token: 0x06003D32 RID: 15666 RVA: 0x0021D8E4 File Offset: 0x0021BCE4
	Public Sub SetColor(color As Color)
		Dim a As Single = Me.spriteRenderer.color.a
		color.a = a
		Me.spriteRenderer.color = color
	End Sub

	' Token: 0x06003D33 RID: 15667 RVA: 0x0021D91C File Offset: 0x0021BD1C
	Public Sub ResetColor()
		Dim a As Single = Me.spriteRenderer.color.a
		Me.spriteRenderer.color = New Color(1F, 1F, 1F, a)
	End Sub

	' Token: 0x06003D34 RID: 15668 RVA: 0x0021D95D File Offset: 0x0021BD5D
	Public Sub SetColorOverTime(color As Color, time As Single)
		Me.StopColorCoroutine()
		Me.colorCoroutine = Me.setColor_cr(color, time)
		MyBase.StartCoroutine(Me.colorCoroutine)
	End Sub

	' Token: 0x06003D35 RID: 15669 RVA: 0x0021D980 File Offset: 0x0021BD80
	Public Sub StopColorCoroutine()
		If Me.colorCoroutine IsNot Nothing Then
			MyBase.StopCoroutine(Me.colorCoroutine)
		End If
		Me.colorCoroutine = Nothing
	End Sub

	' Token: 0x06003D36 RID: 15670 RVA: 0x0021D9A0 File Offset: 0x0021BDA0
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

	' Token: 0x17000528 RID: 1320
	' (get) Token: 0x06003D37 RID: 15671 RVA: 0x0021D9C9 File Offset: 0x0021BDC9
	Private ReadOnly Property Flashing As Boolean
		Get
			Return MyBase.player.damageReceiver.state = PlayerDamageReceiver.State.Invulnerable
		End Get
	End Property

	' Token: 0x06003D38 RID: 15672 RVA: 0x0021D9E0 File Offset: 0x0021BDE0
	Private Iterator Function flash_cr() As IEnumerator
		Dim t As Single = 0F
		While True
			While Not Me.Flashing
				Yield True
			End While
			Yield CupheadTime.WaitForSeconds(Me, 0.417F)
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

	' Token: 0x06003D39 RID: 15673 RVA: 0x0021D9FB File Offset: 0x0021BDFB
	Private Sub SoundIntroPowerup()
		If Not Me.intropowerupactive Then
			AudioManager.Play("player_powerup")
			Me.emitAudioFromObject.Add("player_powerup")
			Me.intropowerupactive = True
		End If
	End Sub

	' Token: 0x06003D3A RID: 15674 RVA: 0x0021DA29 File Offset: 0x0021BE29
	Private Sub SoundParryAxe()
		AudioManager.Play("player_parry_axe")
		Me.emitAudioFromObject.Add("player_parry_axe")
	End Sub

	' Token: 0x04004430 RID: 17456
	Private Const PALADIN_SHADOW_BUFFER_SIZE As Integer = 10

	' Token: 0x04004431 RID: 17457
	Private ChaliceSuper1Return As Integer = Animator.StringToHash("Chalice_Super_1_Return")

	' Token: 0x04004432 RID: 17458
	Private ChaliceSuper2Return As Integer = Animator.StringToHash("Chalice_Super_2_Return")

	' Token: 0x04004433 RID: 17459
	Private ChaliceSuper2ReturnAir As Integer = Animator.StringToHash("Chalice_Super_2_Return_Air")

	' Token: 0x04004434 RID: 17460
	Private ChaliceAirEXRecovery As Integer = Animator.StringToHash("Air_EX_Recovery")

	' Token: 0x04004435 RID: 17461
	Private ChaliceJumpBall As Integer = Animator.StringToHash("Jump_Ball")

	' Token: 0x04004436 RID: 17462
	Private ChaliceJumpDescend As Integer = Animator.StringToHash("Jump_Descend")

	' Token: 0x04004437 RID: 17463
	<SerializeField()>
	Private cuphead As GameObject

	' Token: 0x04004438 RID: 17464
	<SerializeField()>
	Private mugman As GameObject

	' Token: 0x04004439 RID: 17465
	<SerializeField()>
	Private chalice As GameObject

	' Token: 0x0400443A RID: 17466
	<SerializeField()>
	Private chaliceSprites As SpriteRenderer()

	' Token: 0x0400443B RID: 17467
	<Space(10F)>
	<SerializeField()>
	Private runDustRoot As Transform

	' Token: 0x0400443C RID: 17468
	<SerializeField()>
	Private sparkleRoot As Transform

	' Token: 0x0400443D RID: 17469
	<Space(10F)>
	<SerializeField()>
	Private dashEffect As Effect

	' Token: 0x0400443E RID: 17470
	<SerializeField()>
	Private groundedEffect As Effect

	' Token: 0x0400443F RID: 17471
	<SerializeField()>
	Private hitEffect As Effect

	' Token: 0x04004440 RID: 17472
	<SerializeField()>
	Private runEffect As Effect

	' Token: 0x04004441 RID: 17473
	<SerializeField()>
	Private curseEffect As Effect

	' Token: 0x04004442 RID: 17474
	<SerializeField()>
	Private smokeDashEffect As Effect

	' Token: 0x04004443 RID: 17475
	<SerializeField()>
	Private healerCharmEffect As HealerCharmSparkEffect

	' Token: 0x04004444 RID: 17476
	<SerializeField()>
	Private powerUpBurstEffect As Effect

	' Token: 0x04004445 RID: 17477
	<SerializeField()>
	Private chaliceDoubleJumpEffect As Effect

	' Token: 0x04004446 RID: 17478
	<SerializeField()>
	Private chaliceDashEffect As Effect

	' Token: 0x04004447 RID: 17479
	Private chaliceDashEffectActive As Effect

	' Token: 0x04004448 RID: 17480
	<SerializeField()>
	Private chaliceDashSparkle As Effect

	' Token: 0x04004449 RID: 17481
	<SerializeField()>
	Private chaliceJumpShootRenderers As SpriteRenderer()

	' Token: 0x0400444A RID: 17482
	<SerializeField()>
	Private chaliceDuckDashMaterial As Material

	' Token: 0x0400444B RID: 17483
	<SerializeField()>
	Private chaliceDuckDashSparkles As Effect

	' Token: 0x0400444C RID: 17484
	Private chaliceInvincibleSparklesCoroutine As Coroutine

	' Token: 0x0400444D RID: 17485
	Private chaliceFellFromDuckDash As Boolean

	' Token: 0x0400444E RID: 17486
	<SerializeField()>
	Private chaliceIntroAnimation As LevelPlayerChaliceIntroAnimation

	' Token: 0x0400444F RID: 17487
	Private chaliceIntroCurrent As LevelPlayerChaliceIntroAnimation

	' Token: 0x04004450 RID: 17488
	<SerializeField()>
	Private cupheadScaredSprite As Sprite

	' Token: 0x04004451 RID: 17489
	<SerializeField()>
	Private mugmanScaredSprite As Sprite

	' Token: 0x04004453 RID: 17491
	Private hitAnimation As Boolean

	' Token: 0x04004454 RID: 17492
	Private super As Boolean

	' Token: 0x04004455 RID: 17493
	Private shooting As Boolean

	' Token: 0x04004456 RID: 17494
	Private fired As Boolean

	' Token: 0x04004457 RID: 17495
	Private intropowerupactive As Boolean

	' Token: 0x04004458 RID: 17496
	Private exDirection As String

	' Token: 0x04004459 RID: 17497
	Private lastTrueLookDir As Trilean2 = New Trilean2(1, 0)

	' Token: 0x0400445A RID: 17498
	Private timeSinceStoppedShooting As Single = 100F

	' Token: 0x0400445B RID: 17499
	Private tempMaterial As Material

	' Token: 0x0400445C RID: 17500
	Private Const STOP_SHOOTING_DELAY As Single = 0.0833F

	' Token: 0x0400445D RID: 17501
	Private isIntroB As Boolean

	' Token: 0x0400445E RID: 17502
	Private chaliceActivated As Boolean

	' Token: 0x0400445F RID: 17503
	Private inScaredIntro As Boolean

	' Token: 0x04004460 RID: 17504
	<SerializeField()>
	Private curseEffectDelay As Single = 0.15F

	' Token: 0x04004461 RID: 17505
	<SerializeField()>
	Private curseAngleShiftRange As MinMax = New MinMax(60F, 300F)

	' Token: 0x04004462 RID: 17506
	<SerializeField()>
	Private curseDistanceRange As MinMax = New MinMax(0F, 20F)

	' Token: 0x04004463 RID: 17507
	Private curseEffectAngle As Single

	' Token: 0x04004464 RID: 17508
	Private curseEffectTimer As Single

	' Token: 0x04004465 RID: 17509
	Private curseCharmLevel As Integer = -1

	' Token: 0x04004466 RID: 17510
	Private paladinShadowPosition As Vector3()

	' Token: 0x04004467 RID: 17511
	Private paladinShadowScale As Vector3()

	' Token: 0x04004468 RID: 17512
	Private paladinShadowSprite As Sprite()

	' Token: 0x04004469 RID: 17513
	<SerializeField()>
	Private paladinShadows As SpriteRenderer()

	' Token: 0x0400446A RID: 17514
	Private showCurseFX As Boolean

	' Token: 0x0400446B RID: 17515
	Private colorCoroutine As IEnumerator

	' Token: 0x02000A12 RID: 2578
	Private NotInheritable Class Booleans
		' Token: 0x0400446C RID: 17516
		Public Shared Dashing As Integer = Animator.StringToHash("Dashing")

		' Token: 0x0400446D RID: 17517
		Public Shared Locked As Integer = Animator.StringToHash("Locked")

		' Token: 0x0400446E RID: 17518
		Public Shared Shooting As Integer = Animator.StringToHash("Shooting")

		' Token: 0x0400446F RID: 17519
		Public Shared Grounded As Integer = Animator.StringToHash("Grounded")

		' Token: 0x04004470 RID: 17520
		Public Shared Turning As Integer = Animator.StringToHash("Turning")

		' Token: 0x04004471 RID: 17521
		Public Shared Intro As Integer = Animator.StringToHash("Intro")

		' Token: 0x04004472 RID: 17522
		Public Shared Dead As Integer = Animator.StringToHash("Dead")

		' Token: 0x04004473 RID: 17523
		Public Shared HasParryCharm As Integer = Animator.StringToHash("HasParryCharm")

		' Token: 0x04004474 RID: 17524
		Public Shared HasParryAttack As Integer = Animator.StringToHash("HasParryAttack")

		' Token: 0x04004475 RID: 17525
		Public Shared ChaliceOffIdle As Integer = Animator.StringToHash("ChaliceOffIdle")

		' Token: 0x04004476 RID: 17526
		Public Shared DoubleJump As Integer = Animator.StringToHash("DoubleJump")

		' Token: 0x04004477 RID: 17527
		Public Shared ChaliceAirEX As Integer = Animator.StringToHash("ChaliceAirEX")
	End Class

	' Token: 0x02000A13 RID: 2579
	Private NotInheritable Class Integers
		' Token: 0x04004478 RID: 17528
		Public Shared MoveX As Integer = Animator.StringToHash("MoveX")

		' Token: 0x04004479 RID: 17529
		Public Shared MoveY As Integer = Animator.StringToHash("MoveY")

		' Token: 0x0400447A RID: 17530
		Public Shared LookX As Integer = Animator.StringToHash("LookX")

		' Token: 0x0400447B RID: 17531
		Public Shared LookY As Integer = Animator.StringToHash("LookY")

		' Token: 0x0400447C RID: 17532
		Public Shared ChaliceJumpDescendLoopCounter As Integer = Animator.StringToHash("ChaliceJumpDescendLoopCounter")
	End Class

	' Token: 0x02000A14 RID: 2580
	Private NotInheritable Class Triggers
		' Token: 0x0400447D RID: 17533
		Public Shared OnJump As Integer = Animator.StringToHash("OnJump")

		' Token: 0x0400447E RID: 17534
		Public Shared OnGround As Integer = Animator.StringToHash("OnGround")

		' Token: 0x0400447F RID: 17535
		Public Shared OnParry As Integer = Animator.StringToHash("OnParry")

		' Token: 0x04004480 RID: 17536
		Public Shared OnWin As Integer = Animator.StringToHash("OnWin")

		' Token: 0x04004481 RID: 17537
		Public Shared OnTurn As Integer = Animator.StringToHash("OnTurn")

		' Token: 0x04004482 RID: 17538
		Public Shared OnFire As Integer = Animator.StringToHash("OnFire")
	End Class

	' Token: 0x02000A15 RID: 2581
	Private Enum AnimLayers
		' Token: 0x04004484 RID: 17540
		Base
		' Token: 0x04004485 RID: 17541
		ShootRun
		' Token: 0x04004486 RID: 17542
		ShootRunDiag
		' Token: 0x04004487 RID: 17543
		ChaliceSpecial
		' Token: 0x04004488 RID: 17544
		ChaliceSync
		' Token: 0x04004489 RID: 17545
		ChaliceShootRun
		' Token: 0x0400448A RID: 17546
		ChaliceShootRunDiag
	End Enum

	' Token: 0x02000A16 RID: 2582
	Private Enum ChaliceAim
		' Token: 0x0400448C RID: 17548
		UpAim
		' Token: 0x0400448D RID: 17549
		DiagUpAim
		' Token: 0x0400448E RID: 17550
		ForwardAim
		' Token: 0x0400448F RID: 17551
		DiagDownAim
		' Token: 0x04004490 RID: 17552
		DownAim
	End Enum
End Class
