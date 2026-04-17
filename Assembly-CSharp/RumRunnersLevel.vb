Imports System
Imports System.Collections
Imports System.Diagnostics
Imports UnityEngine

' Token: 0x02000274 RID: 628
Public Class RumRunnersLevel
	Inherits Level

	' Token: 0x060006F1 RID: 1777 RVA: 0x000723C4 File Offset: 0x000707C4
	Protected Overrides Sub PartialInit()
		Me.properties = LevelProperties.RumRunners.GetMode(MyBase.mode)
		AddHandler Me.properties.OnStateChange, AddressOf MyBase.zHack_OnStateChanged
		AddHandler Me.properties.OnBossDeath, AddressOf MyBase.zHack_OnWin
		MyBase.timeline = Me.properties.CreateTimeline(MyBase.mode)
		Me.goalTimes = Me.properties.goalTimes
		AddHandler Me.properties.OnBossDamaged, AddressOf MyBase.timeline.DealDamage
		MyBase.PartialInit()
	End Sub

	' Token: 0x17000126 RID: 294
	' (get) Token: 0x060006F2 RID: 1778 RVA: 0x0007245A File Offset: 0x0007085A
	Public Overrides ReadOnly Property CurrentLevel As Levels
		Get
			Return Levels.RumRunners
		End Get
	End Property

	' Token: 0x17000127 RID: 295
	' (get) Token: 0x060006F3 RID: 1779 RVA: 0x00072461 File Offset: 0x00070861
	Public Overrides ReadOnly Property CurrentScene As Scenes
		Get
			Return Scenes.scene_level_rum_runners
		End Get
	End Property

	' Token: 0x14000002 RID: 2
	' (add) Token: 0x060006F4 RID: 1780 RVA: 0x00072468 File Offset: 0x00070868
	' (remove) Token: 0x060006F5 RID: 1781 RVA: 0x000724A0 File Offset: 0x000708A0
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnUpperBridgeDestroy As Action(Of Rangef)

	' Token: 0x17000128 RID: 296
	' (get) Token: 0x060006F6 RID: 1782 RVA: 0x000724D8 File Offset: 0x000708D8
	Public Overrides ReadOnly Property BossPortrait As Sprite
		Get
			Select Case Me.properties.CurrentState.stateName
				Case LevelProperties.RumRunners.States.Main
					Return Me._bossPortraitMain
				Case LevelProperties.RumRunners.States.Worm
					Return Me._bossPortraitPhaseTwo
				Case LevelProperties.RumRunners.States.Anteater
					Return Me._bossPortraitPhaseThree
				Case LevelProperties.RumRunners.States.MobBoss
					Return Me._bossPortraitPhaseFour
			End Select
			Global.Debug.LogError("Couldn't find portrait for state " + Me.properties.CurrentState.stateName + ". Using Main.", Nothing)
			Return Me._bossPortraitMain
		End Get
	End Property

	' Token: 0x17000129 RID: 297
	' (get) Token: 0x060006F7 RID: 1783 RVA: 0x00072564 File Offset: 0x00070964
	Public Overrides ReadOnly Property BossQuote As String
		Get
			Select Case Me.properties.CurrentState.stateName
				Case LevelProperties.RumRunners.States.Main
					Return Me._bossQuoteMain
				Case LevelProperties.RumRunners.States.Worm
					Return Me._bossQuotePhaseTwo
				Case LevelProperties.RumRunners.States.Anteater
					Return Me._bossQuotePhaseThree
				Case LevelProperties.RumRunners.States.MobBoss
					Return Me._bossQuotePhaseFour
			End Select
			Global.Debug.LogError("Couldn't find quote for state " + Me.properties.CurrentState.stateName + ". Using Main.", Nothing)
			Return Me._bossQuoteMain
		End Get
	End Property

	' Token: 0x060006F8 RID: 1784 RVA: 0x000725F0 File Offset: 0x000709F0
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me._bossPortraitMain = Nothing
		Me._bossPortraitPhaseTwo = Nothing
		Me._bossPortraitPhaseThree = Nothing
		Me._bossPortraitPhaseFour = Nothing
		If CupheadLevelCamera.Current IsNot Nothing Then
			RemoveHandler CupheadLevelCamera.Current.OnShakeEvent, AddressOf Me.onShakeEventHandler
		End If
	End Sub

	' Token: 0x060006F9 RID: 1785 RVA: 0x00072648 File Offset: 0x00070A48
	Protected Overrides Sub Start()
		MyBase.Start()
		Me.spider.LevelInit(Me.properties)
		Me.worm.LevelInit(Me.properties)
		Me.anteater.LevelInit(Me.properties)
		AddHandler CupheadLevelCamera.Current.OnShakeEvent, AddressOf Me.onShakeEventHandler
	End Sub

	' Token: 0x060006FA RID: 1786 RVA: 0x000726A4 File Offset: 0x00070AA4
	Protected Overrides Sub OnDrawGizmosSelected()
		MyBase.OnDrawGizmosSelected()
		Gizmos.color = Color.magenta
		Gizmos.DrawLine(New Vector3(Me.topPlatformEffectRange.minimum, -1000F), New Vector3(Me.topPlatformEffectRange.minimum, 1000F))
		Gizmos.DrawLine(New Vector3(Me.topPlatformEffectRange.maximum, -1000F), New Vector3(Me.topPlatformEffectRange.maximum, 1000F))
		Gizmos.color = Color.yellow
		Gizmos.DrawLine(New Vector3(Me.middlePlatformEffectRange.minimum, -1000F), New Vector3(Me.middlePlatformEffectRange.minimum, 1000F))
		Gizmos.DrawLine(New Vector3(Me.middlePlatformEffectRange.maximum, -1000F), New Vector3(Me.middlePlatformEffectRange.maximum, 1000F))
	End Sub

	' Token: 0x060006FB RID: 1787 RVA: 0x00072788 File Offset: 0x00070B88
	Protected Overrides Sub OnStateChanged()
		MyBase.OnStateChanged()
		Me.StopAllCoroutines()
		If Me.properties.CurrentState.stateName = LevelProperties.RumRunners.States.Worm Then
			MyBase.StartCoroutine(Me.worm_cr())
		ElseIf Me.properties.CurrentState.stateName = LevelProperties.RumRunners.States.Anteater Then
			MyBase.StartCoroutine(Me.anteater_cr())
		ElseIf Me.properties.CurrentState.stateName = LevelProperties.RumRunners.States.MobBoss Then
			MyBase.StartCoroutine(Me.winFakeout_cr())
		End If
	End Sub

	' Token: 0x060006FC RID: 1788 RVA: 0x00072814 File Offset: 0x00070C14
	Private Iterator Function worm_cr() As IEnumerator
		Me.spider.Die()
		While Me.spider IsNot Nothing
			Yield Nothing
		End While
		Yield CupheadTime.WaitForSeconds(Me, 0.2F)
		Me.worm.Setup()
		Me.worm.StartBarrels()
		Me.ph2SpiderAnimation.gameObject.SetActive(True)
		Yield Me.ph2SpiderAnimation.WaitForAnimationToEnd(Me, "Ph2", False, True)
		Yield CupheadTime.WaitForSeconds(Me, RumRunnersLevel.SpiderTransitionLowerRopeDuration)
		Me.worm.StartWorm(Me.mobIntro.bugGirlDamage)
		Yield CupheadTime.WaitForSeconds(Me, RumRunnersLevel.SpiderTransitionLowerBugDuration)
		Me.ph2SpiderAnimation.SetTrigger("End")
		Dim ph2 As RumRunnersLevelPh2StartAnimation = Me.ph2SpiderAnimation.GetComponent(Of RumRunnersLevelPh2StartAnimation)()
		While Not ph2.dropped
			Yield Nothing
		End While
		Me.worm.introDrop = True
		Return
	End Function

	' Token: 0x060006FD RID: 1789 RVA: 0x00072830 File Offset: 0x00070C30
	Private Iterator Function anteater_cr() As IEnumerator
		Me.worm.StartDeath()
		Yield Me.worm.animator.WaitForAnimationToEnd(Me, "Fall", False, True)
		While Mathf.Abs(Me.worm.transform.position.x) > RumRunnersLevel.AnteaterIntroWormTriggerDistance
			Yield Nothing
		End While
		Me.anteater.gameObject.SetActive(True)
		Yield Me.anteater.animator.WaitForAnimationToEnd(Me, "Intro", False, True)
		Me.anteater.StartAnteater()
		Return
	End Function

	' Token: 0x060006FE RID: 1790 RVA: 0x0007284B File Offset: 0x00070C4B
	Public Sub DestroyMiddleBridge()
		Me.destroyPlatforms(Me.destroyedPlatformsMiddle, Me.destroyedSpritesMiddleA, Me.middlePlatformEffectRange)
	End Sub

	' Token: 0x060006FF RID: 1791 RVA: 0x00072865 File Offset: 0x00070C65
	Public Sub DestroyUpperBridge()
		If Me.OnUpperBridgeDestroy IsNot Nothing Then
			Me.OnUpperBridgeDestroy(Me.topPlatformEffectRange)
		End If
		Me.destroyPlatforms(Me.destroyedPlatformsUpper, Me.destroyedSpritesUpperA, Me.topPlatformEffectRange)
	End Sub

	' Token: 0x06000700 RID: 1792 RVA: 0x0007289C File Offset: 0x00070C9C
	Public Sub ShatterBridges()
		Me.destroyPlatforms(Nothing, Me.destroyedSpritesMiddleB, Nothing)
		Me.destroyPlatforms(Nothing, Me.destroyedSpritesUpperB, Nothing)
		Dim array As Single() = New Single() { 0F, 0.25F, 0.5F, 0.75F }
		array.Shuffle()
		Dim num As Single = 1280F / Me.camera.zoom
		For i As Integer = 0 To array.Length - 1
			Dim num2 As Single = Global.UnityEngine.Random.Range(array(i), array(i) + array(1))
			num2 = MathUtilities.LerpMapping(num2, 0F, 1F, -num * 0.4F, num * 0.4F, True)
			Me.FullscreenDirt(1, New Single?(num2), 0.15F, 0.3F)
		Next
		CupheadLevelCamera.Current.Shake(55F, 0.5F, True)
	End Sub

	' Token: 0x06000701 RID: 1793 RVA: 0x00072974 File Offset: 0x00070D74
	Private Sub destroyPlatforms(colliders As LevelPlatform(), sprites As GameObject(), Optional checkRange As Rangef = Nothing)
		If colliders IsNot Nothing Then
			For Each levelPlatform As LevelPlatform In colliders
				Dim levelPlatform2 As LevelPlatform = Nothing
				Dim num As Integer = Array.IndexOf(Of LevelPlatform)(Me.swapPlatformsMappingBefore, levelPlatform)
				If num >= 0 Then
					levelPlatform2 = Me.swapPlatformsMappingAfter(num)
				End If
				If levelPlatform.GetComponentInChildren(Of LevelPlayerController)() Then
					Dim componentsInChildren As LevelPlayerController() = levelPlatform.GetComponentsInChildren(Of LevelPlayerController)()
					For Each levelPlayerController As LevelPlayerController In componentsInChildren
						If Not checkRange.ContainsExclusive(levelPlayerController.transform.position.x) AndAlso levelPlatform2 IsNot Nothing Then
							levelPlatform2.AddChild(levelPlayerController.transform)
						Else
							levelPlayerController.motor.OnTrampolineKnockUp(RumRunnersLevel.BridgeDestroyBounceHeight)
						End If
					Next
				End If
			Next
			For Each levelPlatform3 As LevelPlatform In colliders
				Global.UnityEngine.[Object].Destroy(levelPlatform3.gameObject)
			Next
		End If
		If sprites IsNot Nothing Then
			For Each gameObject As GameObject In sprites
				gameObject.SetActive(False)
			Next
		End If
	End Sub

	' Token: 0x06000702 RID: 1794 RVA: 0x00072AB0 File Offset: 0x00070EB0
	Private Iterator Function winFakeout_cr() As IEnumerator
		Me.anteater.FakeDeathStart()
		AudioManager.Play("level_announcer_knockout_bell")
		AudioManager.Play("sfx_dlc_rumrun_vx_fakeannouncer_knockout")
		MyBase.StartCoroutine(Me.stingSound_cr())
		Dim bannerPosition As Vector3 = Me.fakeBannerAnimator.transform.position
		bannerPosition += CupheadLevelCamera.Current.transform.position
		Me.fakeBannerAnimator.transform.position = bannerPosition
		Me.fakeBannerAnimator.SetTrigger("Banner")
		Dim dirtCoroutine As Coroutine = MyBase.StartCoroutine(Me.fakeDeathDirt_cr())
		Yield Me.fakeBannerAnimator.WaitForAnimationToEnd(Me, "Banner", False, True)
		MyBase.StopCoroutine(dirtCoroutine)
		Me.anteater.FakeDeathContinue()
		Return
	End Function

	' Token: 0x06000703 RID: 1795 RVA: 0x00072ACC File Offset: 0x00070ECC
	Private Iterator Function fakeDeathDirt_cr() As IEnumerator
		Dim elapsedTime As Single = 0F
		Dim delay As Single = 0.4F
		While True
			Yield CupheadTime.WaitForSeconds(Me, delay)
			elapsedTime += delay
			delay = Mathf.Lerp(0.4F, 0.8F, elapsedTime / 1.5F)
			Me.FullscreenDirt(2, Nothing, -1F, -1F)
		End While
		Return
	End Function

	' Token: 0x06000704 RID: 1796 RVA: 0x00072AE8 File Offset: 0x00070EE8
	Private Iterator Function stingSound_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 0.1F)
		AudioManager.Play("sfx_dlc_rumrun_fake_levelbossdefeatsting")
		Return
	End Function

	' Token: 0x06000705 RID: 1797 RVA: 0x00072B03 File Offset: 0x00070F03
	Public Sub FullscreenDirt(count As Integer, Optional positionX As Single? = Nothing, Optional customInitialDelay As Single = -1F, Optional customIntraDelay As Single = -1F)
		MyBase.StartCoroutine(Me.dirtFX_cr(count, positionX, customInitialDelay, customIntraDelay))
	End Sub

	' Token: 0x06000706 RID: 1798 RVA: 0x00072B18 File Offset: 0x00070F18
	Private Iterator Function dirtFX_cr(count As Integer, positionX As Single?, customInitialDelay As Single, customIntraDelay As Single) As IEnumerator
		Dim DirtRandomizationX As MinMax() = New MinMax() { New MinMax(-50F, 0F), New MinMax(0F, 50F) }
		Dim WaitRange As MinMax = New MinMax(0.08F, 0.12F)
		Dim previousSpawnRange As MinMax = If((Not Rand.Bool()), New MinMax(0.5F, 1F), New MinMax(0F, 0.5F))
		For i As Integer = 0 To count - 1
			Dim initialDelay As Single = WaitRange.RandomFloat()
			If customInitialDelay >= 0F Then
				initialDelay = customInitialDelay
			End If
			Yield CupheadTime.WaitForSeconds(Me, initialDelay)
			Dim position As Vector3 = New Vector3(0F, 360F / Me.camera.zoom)
			If positionX Is Nothing Then
				Dim minMax As MinMax
				If previousSpawnRange.min = 0F Then
					minMax = New MinMax(0.5F, 1F)
				Else
					minMax = New MinMax(0F, 0.5F)
				End If
				position.x = 1280F * minMax.RandomFloat() - 640F
				previousSpawnRange = minMax
			Else
				position.x = positionX.Value
			End If
			DirtRandomizationX.Shuffle()
			For spawn As Integer = 0 To DirtRandomizationX.Length - 1
				Me.fullscreenDirtFX.Create(position + New Vector3(DirtRandomizationX(spawn).RandomFloat(), 0F))
				Dim intraDelay As Single = Global.UnityEngine.Random.Range(0.1F, 0.2F)
				If customIntraDelay >= 0F Then
					intraDelay = Global.UnityEngine.Random.Range(customIntraDelay * 0.8F, customIntraDelay * 1.2F)
				End If
				Yield CupheadTime.WaitForSeconds(Me, intraDelay)
			Next
		Next
		Return
	End Function

	' Token: 0x06000707 RID: 1799 RVA: 0x00072B50 File Offset: 0x00070F50
	Private Sub onShakeEventHandler(amount As Single, time As Single)
		Me.FullscreenDirt(2, Nothing, -1F, -1F)
	End Sub

	' Token: 0x06000708 RID: 1800 RVA: 0x00072B77 File Offset: 0x00070F77
	Protected Overrides Sub PlayAnnouncerReady()
		AudioManager.Play("sfx_dlc_rumrun_vx_fakeannouncer_ready")
	End Sub

	' Token: 0x06000709 RID: 1801 RVA: 0x00072B83 File Offset: 0x00070F83
	Protected Overrides Sub PlayAnnouncerBegin()
		AudioManager.Play("sfx_dlc_rumrun_vx_fakeannouncer_begin")
	End Sub

	' Token: 0x0600070A RID: 1802 RVA: 0x00072B90 File Offset: 0x00070F90
	Protected Overrides Iterator Function knockoutSFX_cr() As IEnumerator
		AudioManager.Play("level_announcer_knockout_bell")
		AudioManager.Play("sfx_DLC_RUMRUN_VX_AnnouncerClearThroat")
		Yield CupheadTime.WaitForSeconds(Me, 1.4F)
		AudioManager.Play("level_boss_defeat_sting")
		Return
	End Function

	' Token: 0x0600070B RID: 1803 RVA: 0x00072BAC File Offset: 0x00070FAC
	Public Shared Function GroundWalkingPosY(position As Vector2, collider As Collider2D, Optional offset As Single = 0F, Optional rayLength As Single = 200F) As Single
		Dim num As Integer = 1048576
		position.x = Mathf.Clamp(position.x, CSng(Level.Current.Left), CSng(Level.Current.Right))
		Dim vector As Vector3 = New Vector3(position.x, position.y)
		Dim raycastHit2D As RaycastHit2D = Physics2D.Raycast(vector, Vector3.down, rayLength, num)
		If raycastHit2D.collider IsNot Nothing Then
			Dim y As Single = raycastHit2D.point.y
			Dim num2 As Single = If((Not collider), 0F, (-collider.offset.y + collider.bounds.size.y / 2F))
			Return y + num2 + offset
		End If
		Return position.y
	End Function

	' Token: 0x04000DE0 RID: 3552
	Private properties As LevelProperties.RumRunners

	' Token: 0x04000DE1 RID: 3553
	Private Shared SpiderTransitionLowerRopeDuration As Single = 0.5F

	' Token: 0x04000DE2 RID: 3554
	Private Shared SpiderTransitionLowerBugDuration As Single = 0.3F

	' Token: 0x04000DE3 RID: 3555
	Private Shared BridgeDestroyBounceHeight As Single = -1F

	' Token: 0x04000DE4 RID: 3556
	Private Shared AnteaterIntroWormTriggerDistance As Single = 500F

	' Token: 0x04000DE6 RID: 3558
	<SerializeField()>
	Private ph2SpiderAnimation As Animator

	' Token: 0x04000DE7 RID: 3559
	<SerializeField()>
	Private spider As RumRunnersLevelSpider

	' Token: 0x04000DE8 RID: 3560
	<SerializeField()>
	Private worm As RumRunnersLevelWorm

	' Token: 0x04000DE9 RID: 3561
	<SerializeField()>
	Private anteater As RumRunnersLevelAnteater

	' Token: 0x04000DEA RID: 3562
	<SerializeField()>
	Private mobIntro As RumRunnersLevelMobIntroAnimation

	' Token: 0x04000DEB RID: 3563
	<SerializeField()>
	Private fullscreenDirtFX As Effect

	' Token: 0x04000DEC RID: 3564
	<SerializeField()>
	Private fakeBannerAnimator As Animator

	' Token: 0x04000DED RID: 3565
	<SerializeField()>
	Private destroyedSpritesMiddleA As GameObject()

	' Token: 0x04000DEE RID: 3566
	<SerializeField()>
	Private destroyedSpritesMiddleB As GameObject()

	' Token: 0x04000DEF RID: 3567
	<SerializeField()>
	Private destroyedSpritesUpperA As GameObject()

	' Token: 0x04000DF0 RID: 3568
	<SerializeField()>
	Private destroyedSpritesUpperB As GameObject()

	' Token: 0x04000DF1 RID: 3569
	<SerializeField()>
	Private destroyedPlatformsMiddle As LevelPlatform()

	' Token: 0x04000DF2 RID: 3570
	<SerializeField()>
	Private destroyedPlatformsUpper As LevelPlatform()

	' Token: 0x04000DF3 RID: 3571
	<SerializeField()>
	Private swapPlatformsMappingBefore As LevelPlatform()

	' Token: 0x04000DF4 RID: 3572
	<SerializeField()>
	Private swapPlatformsMappingAfter As LevelPlatform()

	' Token: 0x04000DF5 RID: 3573
	<SerializeField()>
	Private middlePlatformEffectRange As Rangef

	' Token: 0x04000DF6 RID: 3574
	<SerializeField()>
	Private topPlatformEffectRange As Rangef

	' Token: 0x04000DF7 RID: 3575
	<Header("Boss Info")>
	<SerializeField()>
	Private _bossPortraitMain As Sprite

	' Token: 0x04000DF8 RID: 3576
	<SerializeField()>
	Private _bossPortraitPhaseTwo As Sprite

	' Token: 0x04000DF9 RID: 3577
	<SerializeField()>
	Private _bossPortraitPhaseThree As Sprite

	' Token: 0x04000DFA RID: 3578
	<SerializeField()>
	Private _bossPortraitPhaseFour As Sprite

	' Token: 0x04000DFB RID: 3579
	<SerializeField()>
	Private _bossQuoteMain As String

	' Token: 0x04000DFC RID: 3580
	<SerializeField()>
	Private _bossQuotePhaseTwo As String

	' Token: 0x04000DFD RID: 3581
	<SerializeField()>
	Private _bossQuotePhaseThree As String

	' Token: 0x04000DFE RID: 3582
	<SerializeField()>
	Private _bossQuotePhaseFour As String
End Class
