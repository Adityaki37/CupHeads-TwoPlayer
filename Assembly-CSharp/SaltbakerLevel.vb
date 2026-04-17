Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports UnityEngine
Imports UnityStandardAssets.ImageEffects

' Token: 0x0200029D RID: 669
Public Class SaltbakerLevel
	Inherits Level

	' Token: 0x06000759 RID: 1881 RVA: 0x0007460C File Offset: 0x00072A0C
	Protected Overrides Sub PartialInit()
		Me.properties = LevelProperties.Saltbaker.GetMode(MyBase.mode)
		AddHandler Me.properties.OnStateChange, AddressOf MyBase.zHack_OnStateChanged
		AddHandler Me.properties.OnBossDeath, AddressOf MyBase.zHack_OnWin
		MyBase.timeline = Me.properties.CreateTimeline(MyBase.mode)
		Me.goalTimes = Me.properties.goalTimes
		AddHandler Me.properties.OnBossDamaged, AddressOf MyBase.timeline.DealDamage
		MyBase.PartialInit()
	End Sub

	' Token: 0x17000130 RID: 304
	' (get) Token: 0x0600075A RID: 1882 RVA: 0x000746A2 File Offset: 0x00072AA2
	Public Overrides ReadOnly Property CurrentLevel As Levels
		Get
			Return Levels.Saltbaker
		End Get
	End Property

	' Token: 0x17000131 RID: 305
	' (get) Token: 0x0600075B RID: 1883 RVA: 0x000746A9 File Offset: 0x00072AA9
	Public Overrides ReadOnly Property CurrentScene As Scenes
		Get
			Return Scenes.scene_level_saltbaker
		End Get
	End Property

	' Token: 0x17000132 RID: 306
	' (get) Token: 0x0600075C RID: 1884 RVA: 0x000746B0 File Offset: 0x00072AB0
	Public Overrides ReadOnly Property BossPortrait As Sprite
		Get
			Select Case Me.properties.CurrentState.stateName
				Case LevelProperties.Saltbaker.States.Main
					Return Me._bossPortraitMain
				Case LevelProperties.Saltbaker.States.PhaseTwo
					Return Me._bossPortraitPhaseTwo
				Case LevelProperties.Saltbaker.States.PhaseThree
					Return Me._bossPortraitPhaseThree
				Case LevelProperties.Saltbaker.States.PhaseFour
					Return Me._bossPortraitPhaseFour
			End Select
			Global.Debug.LogError("Couldn't find portrait for state " + Me.properties.CurrentState.stateName + ". Using Main.", Nothing)
			Return Me._bossPortraitMain
		End Get
	End Property

	' Token: 0x17000133 RID: 307
	' (get) Token: 0x0600075D RID: 1885 RVA: 0x0007473C File Offset: 0x00072B3C
	Public Overrides ReadOnly Property BossQuote As String
		Get
			Select Case Me.properties.CurrentState.stateName
				Case LevelProperties.Saltbaker.States.Main
					Return Me._bossQuoteMain
				Case LevelProperties.Saltbaker.States.PhaseTwo
					Return Me._bossQuotePhaseTwo
				Case LevelProperties.Saltbaker.States.PhaseThree
					Return Me._bossQuotePhaseThree
				Case LevelProperties.Saltbaker.States.PhaseFour
					Return Me._bossQuotePhaseFour
			End Select
			Global.Debug.LogError("Couldn't find quote for state " + Me.properties.CurrentState.stateName + ". Using Main.", Nothing)
			Return Me._bossQuoteMain
		End Get
	End Property

	' Token: 0x17000134 RID: 308
	' (get) Token: 0x0600075E RID: 1886 RVA: 0x000747C5 File Offset: 0x00072BC5
	' (set) Token: 0x0600075F RID: 1887 RVA: 0x000747CD File Offset: 0x00072BCD
	Public Property playerLost As Boolean

	' Token: 0x06000760 RID: 1888 RVA: 0x000747D8 File Offset: 0x00072BD8
	Protected Overrides Sub Start()
		MyBase.Start()
		Me.trappedCharacter.Setup()
		Me.trappedCharacterPhaseThree.Setup()
		Me.saltbaker.LevelInit(Me.properties)
		Me.saltbakerBouncer.LevelInit(Me.properties)
		Me.saltbakerPillarHandler.LevelInit(Me.properties)
		Me.fires = New List(Of SaltbakerLevelJumper)()
	End Sub

	' Token: 0x06000761 RID: 1889 RVA: 0x0007483F File Offset: 0x00072C3F
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me._bossPortraitMain = Nothing
		Me._bossPortraitPhaseTwo = Nothing
		Me._bossPortraitPhaseThree = Nothing
		Me._bossPortraitPhaseFour = Nothing
	End Sub

	' Token: 0x06000762 RID: 1890 RVA: 0x00074863 File Offset: 0x00072C63
	Protected Overrides Sub OnLose()
		Me.playerLost = True
	End Sub

	' Token: 0x06000763 RID: 1891 RVA: 0x0007486C File Offset: 0x00072C6C
	Protected Overrides Sub OnStateChanged()
		MyBase.OnStateChanged()
		If Me.properties.CurrentState.stateName = LevelProperties.Saltbaker.States.PhaseTwo Then
			Me.StopAllCoroutines()
			MyBase.StartCoroutine(Me.saltbaker.phase_one_to_two_cr())
		ElseIf Me.properties.CurrentState.stateName = LevelProperties.Saltbaker.States.PhaseThree Then
			Me.StopAllCoroutines()
			Me.KillFires()
			Me.saltbaker.OnPhaseThree()
		ElseIf Me.properties.CurrentState.stateName = LevelProperties.Saltbaker.States.PhaseFour Then
			Me.StopAllCoroutines()
			MyBase.StartCoroutine(Me.phase_three_to_four_cr())
		End If
	End Sub

	' Token: 0x06000764 RID: 1892 RVA: 0x00074910 File Offset: 0x00072D10
	Public Sub StartPhase3()
		Me.ClearFires()
		Me.phase3BG.SetActive(True)
		Me.bounds.bottomEnabled = False
		GameObject.Find("Level_Ground").SetActive(False)
		Level.Current.SetBounds(Nothing, Nothing, New Integer?(500), Nothing)
		For Each abstractPlayerController As AbstractPlayerController In PlayerManager.GetAllPlayers()
			Dim levelPlayerController As LevelPlayerController = CType(abstractPlayerController, LevelPlayerController)
			If levelPlayerController IsNot Nothing Then
				levelPlayerController.transform.position = New Vector3(levelPlayerController.transform.position.x, CSng(Level.Current.Ceiling))
				levelPlayerController.motor.DoPostSuperHop()
			End If
		Next
		Me.SpawnCutters()
		MyBase.StartCoroutine(Me.phase_two_to_three_cr())
	End Sub

	' Token: 0x06000765 RID: 1893 RVA: 0x00074A20 File Offset: 0x00072E20
	Private Iterator Function phase_two_to_three_cr() As IEnumerator
		Dim wait As YieldInstruction = New WaitForFixedUpdate()
		Dim t As Single = 0F
		While t < 1F
			t += CupheadTime.FixedDelta
			Me.transitionFader.color = New Color(1F, 1F, 1F, Mathf.InverseLerp(1F, 0F, t))
			Yield wait
		End While
		Me.saltbakerBouncer.gameObject.SetActive(True)
		Me.saltbakerBouncer.StartBouncer(New Vector3(0F, 700F))
		Me.transitionFader.gameObject.SetActive(False)
		Return
	End Function

	' Token: 0x06000766 RID: 1894 RVA: 0x00074A3C File Offset: 0x00072E3C
	Private Iterator Function phase_three_to_four_cr() As IEnumerator
		Me.DestroyRunners()
		Me.saltbakerBouncer.EndBouncer()
		Me.groundCrack.Play("Start")
		MyBase.StartCoroutine(Me.flash_sky_cr())
		Me.tornadoActivator.enabled = True
		Me.phase3to4Transition.StartSaltman()
		While Me.saltbakerBouncer IsNot Nothing
			Yield Nothing
		End While
		Me.saltbakerPillarHandler.gameObject.SetActive(True)
		Me.saltbakerPillarHandler.StartPlatforms()
		Yield CupheadTime.WaitForSeconds(Me, 1F)
		Me.saltbakerPillarHandler.suppressCenterPlatforms = False
		Yield CupheadTime.WaitForSeconds(Me, 0.5F)
		Me.groundCrack.SetTrigger("Continue")
		Me.tornadoActivator.SetTrigger("Continue")
		Me.saltbakerPillarHandler.StartPillarOfDoom()
		While Me.phase3to4Transition.enabled
			Yield Nothing
		End While
		Yield CupheadTime.WaitForSeconds(Me, 1F)
		Me.saltbakerPillarHandler.StartHeart()
		MyBase.StartCoroutine(Me.bg_salt_spillage_cr())
		Global.UnityEngine.Camera.main.cullingMask = Global.UnityEngine.Camera.main.cullingMask Xor 1 << LayerMask.NameToLayer("Renderer")
		Me.phaseFourBlurCamera.SetActive(True)
		Me.phaseFourBlurTexture.gameObject.SetActive(True)
		Dim t As Single = 0F
		While t < Me.phaseFourBlurDimTime
			t += CupheadTime.Delta
			Dim tNormalized As Single = t / Me.phaseFourBlurDimTime
			Me.phaseFourBlurController.blurSize = Mathf.Lerp(0F, Me.phaseFourBlurAmount, tNormalized)
			Me.phaseFourBlurTexture.material.color = Color.Lerp(Color.white, New Color(Me.phaseFourDimAmount, Me.phaseFourDimAmount, Me.phaseFourDimAmount, 1F), tNormalized)
			Yield Nothing
		End While
		Me.phaseFourBlurController.blurSize = Me.phaseFourBlurAmount
		Me.phaseFourBlurTexture.material.color = New Color(Me.phaseFourDimAmount, Me.phaseFourDimAmount, Me.phaseFourDimAmount, 1F)
		Return
	End Function

	' Token: 0x06000767 RID: 1895 RVA: 0x00074A58 File Offset: 0x00072E58
	Private Iterator Function flash_sky_cr() As IEnumerator
		While True
			Dim dimRate As Single = Global.UnityEngine.Random.Range(2F, 4F)
			Me.skyFront.color = Color.white
			While Me.skyFront.color.r > 0F
				Dim c As Single = Me.skyFront.color.r - 0.041666668F * dimRate
				Me.skyFront.color = New Color(c, c, c, 1F)
				Yield CupheadTime.WaitForSeconds(Me, 0.041666668F)
			End While
			Yield CupheadTime.WaitForSeconds(Me, Global.UnityEngine.Random.Range(0.1F, 4F))
		End While
		Return
	End Function

	' Token: 0x06000768 RID: 1896 RVA: 0x00074A74 File Offset: 0x00072E74
	Private Iterator Function bg_salt_spillage_cr() As IEnumerator
		Me.saltSpillageDelay = New PatternString(Me.saltSpillageDelayString, True)
		Me.saltSpillageOrder = New PatternString(Me.saltSpillageOrderString, True)
		While True
			Yield CupheadTime.WaitForSeconds(Me, Me.saltSpillageDelay.PopFloat())
			Me.groundCrack.Play("Spill", Me.saltSpillageOrder.PopInt(), 0F)
		End While
		Return
	End Function

	' Token: 0x06000769 RID: 1897 RVA: 0x00074A90 File Offset: 0x00072E90
	Public Sub SpawnJumpers()
		Dim numberFireJumpers As Integer = Me.properties.CurrentState.jumper.numberFireJumpers
		If numberFireJumpers = 0 Then
			Return
		End If
		Dim num As Single = CSng(Level.Current.Ceiling)
		Dim num2 As Single = CSng(Level.Current.Left)
		Dim num3 As Single = CSng((Level.Current.Width / numberFireJumpers))
		Dim num4 As Single = num2 + num3 / 2F
		Dim jumpDelay As Single = Me.properties.CurrentState.jumper.jumpDelay
		For i As Integer = 0 To numberFireJumpers - 1
			Dim num5 As Single = num4 + num3 * CSng(i)
			Dim vector As Vector3 = New Vector3(num5, num)
			Dim saltbakerLevelJumper As SaltbakerLevelJumper = Me.jumperPrefab.Create(vector, Me, Me.properties.CurrentState.swooper, Me.properties.CurrentState.jumper, jumpDelay * CSng(i), False)
			saltbakerLevelJumper.GetComponent(Of SpriteRenderer)().sortingOrder = i + -5
			Me.fires.Add(saltbakerLevelJumper)
		Next
	End Sub

	' Token: 0x0600076A RID: 1898 RVA: 0x00074B84 File Offset: 0x00072F84
	Public Sub SpawnSwoopers()
		Dim array As Single() = New Single() { CSng(Level.Current.Right) / 2F, CSng(Level.Current.Left) / 2F }
		Dim num As Integer = Me.properties.CurrentState.swooper.numberFireSwoopers
		If num > 2 Then
			Global.Debug.Break()
			num = 2
		End If
		If num = 0 Then
			Return
		End If
		Dim num2 As Single = CSng((Level.Current.Left + Level.Current.Right)) / 2F
		Dim jumpDelay As Single = Me.properties.CurrentState.swooper.jumpDelay
		For i As Integer = 0 To num - 1
			Dim saltbakerLevelJumper As SaltbakerLevelJumper = Me.jumperPrefab.Create(New Vector3(array(i), CSng(Level.Current.Ceiling)), Me, Me.properties.CurrentState.swooper, Me.properties.CurrentState.jumper, jumpDelay * CSng(i), True)
			saltbakerLevelJumper.GetComponent(Of SpriteRenderer)().sortingOrder = 3 + i
			Me.fires.Add(saltbakerLevelJumper)
		Next
	End Sub

	' Token: 0x0600076B RID: 1899 RVA: 0x00074C9C File Offset: 0x0007309C
	Public Sub KillFires()
		For Each saltbakerLevelJumper As SaltbakerLevelJumper In Me.fires
			saltbakerLevelJumper.Die()
		Next
	End Sub

	' Token: 0x0600076C RID: 1900 RVA: 0x00074CF8 File Offset: 0x000730F8
	Public Sub ClearFires()
		For Each saltbakerLevelJumper As SaltbakerLevelJumper In Me.fires
			If saltbakerLevelJumper IsNot Nothing Then
				Global.UnityEngine.[Object].Destroy(saltbakerLevelJumper.gameObject)
			End If
		Next
		Me.fires.Clear()
	End Sub

	' Token: 0x0600076D RID: 1901 RVA: 0x00074D70 File Offset: 0x00073170
	Public Function IsPositionAvailable(pos As Vector3, fire As SaltbakerLevelJumper) As Boolean
		Dim num As Single = fire.GetComponent(Of Collider2D)().bounds.size.x * 1F
		For i As Integer = 0 To Me.fires.Count - 1
			If Me.fires(i) IsNot fire Then
				Dim num2 As Single = pos.x + num
				Dim num3 As Single = pos.x - num
				If num2 > Me.fires(i).GetAimPos().x - num AndAlso num3 < Me.fires(i).GetAimPos().x + num Then
					Return False
				End If
			End If
		Next
		Return True
	End Function

	' Token: 0x0600076E RID: 1902 RVA: 0x00074E34 File Offset: 0x00073234
	Private Sub SpawnCutters()
		Dim cutter As LevelProperties.Saltbaker.Cutter = Me.properties.CurrentState.cutter
		If cutter.cutterCount <= 0 Then
			Return
		End If
		Me.cutters = New List(Of SaltbakerLevelCutter)()
		Dim [next] As AbstractPlayerController = PlayerManager.GetNext()
		Dim num As Single = 50F
		Dim minDistance As Single = 50F
		Dim array As Single() = New Single(1) {}
		Dim flag As Boolean = Rand.Bool()
		Dim list As List(Of Vector2) = New List(Of Vector2)()
		Dim num2 As Single = Mathf.Min(PlayerManager.GetNext().transform.position.x, PlayerManager.GetNext().transform.position.x)
		Dim num3 As Single = Mathf.Max(PlayerManager.GetNext().transform.position.x, PlayerManager.GetNext().transform.position.x)
		list.Add(New Vector2(CSng(Level.Current.Left) + num, num2))
		list.Add(New Vector2(num2, num3))
		list.Add(New Vector2(num3, CSng(Level.Current.Right) - num))
		list.RemoveAll(Function(s As Vector2) Mathf.Abs(s.x - s.y) < minDistance * 2F)
		list.Sort(Function(s1 As Vector2, s2 As Vector2) Mathf.Abs(s1.x - s1.y).CompareTo(Mathf.Abs(s2.x - s2.y)))
		If list.Count = 3 Then
			list.RemoveAt(0)
		End If
		If list.Count = 2 Then
			array(0) = Mathf.Lerp(list(0).x, list(0).y, 0.5F)
			array(1) = Mathf.Lerp(list(1).x, list(1).y, 0.5F)
		End If
		If list.Count = 1 Then
			array(0) = Mathf.Lerp(list(0).x, list(0).y, 0.333F)
			array(1) = Mathf.Lerp(list(0).x, list(0).y, 0.667F)
		End If
		For i As Integer = 0 To cutter.cutterCount - 1
			Dim vector As Vector3 = New Vector3(array(i), CSng(Level.Current.Ground) + 26F)
			Dim saltbakerLevelCutter As SaltbakerLevelCutter = Me.cutterPrefab.Create(vector, cutter.cutterSpeed, flag, i)
			flag = Not flag
			Me.cutters.Add(saltbakerLevelCutter)
		Next
	End Sub

	' Token: 0x0600076F RID: 1903 RVA: 0x000750E0 File Offset: 0x000734E0
	Private Sub DestroyRunners()
		If Me.cutters Is Nothing Then
			Return
		End If
		For Each saltbakerLevelCutter As SaltbakerLevelCutter In Me.cutters
			saltbakerLevelCutter.Sink()
		Next
		Me.cutters.Clear()
	End Sub

	' Token: 0x04000EFF RID: 3839
	Private properties As LevelProperties.Saltbaker

	' Token: 0x04000F00 RID: 3840
	Private Const FIRE_OFFSET_MODIFIER As Single = 1F

	' Token: 0x04000F01 RID: 3841
	<SerializeField()>
	Private phase3BG As GameObject

	' Token: 0x04000F02 RID: 3842
	<SerializeField()>
	Private cracksBG As GameObject()

	' Token: 0x04000F03 RID: 3843
	<SerializeField()>
	Private cutterPrefab As SaltbakerLevelCutter

	' Token: 0x04000F04 RID: 3844
	Private cutters As List(Of SaltbakerLevelCutter)

	' Token: 0x04000F05 RID: 3845
	<SerializeField()>
	Private jumperPrefab As SaltbakerLevelJumper

	' Token: 0x04000F06 RID: 3846
	Private fires As List(Of SaltbakerLevelJumper)

	' Token: 0x04000F07 RID: 3847
	<SerializeField()>
	Private saltbaker As SaltbakerLevelSaltbaker

	' Token: 0x04000F08 RID: 3848
	<SerializeField()>
	Private saltbakerBouncer As SaltbakerLevelBouncer

	' Token: 0x04000F09 RID: 3849
	<SerializeField()>
	Private saltbakerPillarHandler As SaltbakerLevelPillarHandler

	' Token: 0x04000F0A RID: 3850
	<SerializeField()>
	Private skyFront As SpriteRenderer

	' Token: 0x04000F0B RID: 3851
	<SerializeField()>
	Private transitionFader As SpriteRenderer

	' Token: 0x04000F0C RID: 3852
	<SerializeField()>
	Private phase3to4Transition As SaltbakerLevelPhaseThreeToFourTransition

	' Token: 0x04000F0D RID: 3853
	<SerializeField()>
	Private saltSpillageOrderString As String

	' Token: 0x04000F0E RID: 3854
	Private saltSpillageOrder As PatternString

	' Token: 0x04000F0F RID: 3855
	<SerializeField()>
	Private saltSpillageDelayString As String

	' Token: 0x04000F10 RID: 3856
	Private saltSpillageDelay As PatternString

	' Token: 0x04000F11 RID: 3857
	<SerializeField()>
	Private groundCrack As Animator

	' Token: 0x04000F12 RID: 3858
	<SerializeField()>
	Private tornadoActivator As Animator

	' Token: 0x04000F13 RID: 3859
	<SerializeField()>
	Private phaseFourBlurCamera As GameObject

	' Token: 0x04000F14 RID: 3860
	<SerializeField()>
	Private phaseFourBlurTexture As MeshRenderer

	' Token: 0x04000F15 RID: 3861
	<SerializeField()>
	Private phaseFourBlurAmount As Single = 3F

	' Token: 0x04000F16 RID: 3862
	<SerializeField()>
	Private phaseFourDimAmount As Single = 0.8F

	' Token: 0x04000F17 RID: 3863
	<SerializeField()>
	Private phaseFourBlurDimTime As Single = 1F

	' Token: 0x04000F18 RID: 3864
	<SerializeField()>
	Private phaseFourBlurController As BlurOptimized

	' Token: 0x04000F19 RID: 3865
	<Header("Boss Info")>
	<SerializeField()>
	Private _bossPortraitMain As Sprite

	' Token: 0x04000F1A RID: 3866
	<SerializeField()>
	Private _bossPortraitPhaseTwo As Sprite

	' Token: 0x04000F1B RID: 3867
	<SerializeField()>
	Private _bossPortraitPhaseThree As Sprite

	' Token: 0x04000F1C RID: 3868
	<SerializeField()>
	Private _bossPortraitPhaseFour As Sprite

	' Token: 0x04000F1D RID: 3869
	<SerializeField()>
	Private _bossQuoteMain As String

	' Token: 0x04000F1E RID: 3870
	<SerializeField()>
	Private _bossQuotePhaseTwo As String

	' Token: 0x04000F1F RID: 3871
	<SerializeField()>
	Private _bossQuotePhaseThree As String

	' Token: 0x04000F20 RID: 3872
	<SerializeField()>
	Private _bossQuotePhaseFour As String

	' Token: 0x04000F21 RID: 3873
	Private crackOn As Integer

	' Token: 0x04000F22 RID: 3874
	Public yScrollPos As Single

	' Token: 0x04000F23 RID: 3875
	<SerializeField()>
	Private trappedCharacter As SaltbakerLevelBGTrappedCharacter

	' Token: 0x04000F24 RID: 3876
	<SerializeField()>
	Private trappedCharacterPhaseThree As SaltbakerLevelBGTrappedCharacter
End Class
