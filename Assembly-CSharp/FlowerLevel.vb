Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000181 RID: 385
Public Class FlowerLevel
	Inherits Level

	' Token: 0x06000456 RID: 1110 RVA: 0x00063084 File Offset: 0x00061484
	Protected Overrides Sub PartialInit()
		Me.properties = LevelProperties.Flower.GetMode(MyBase.mode)
		AddHandler Me.properties.OnStateChange, AddressOf MyBase.zHack_OnStateChanged
		AddHandler Me.properties.OnBossDeath, AddressOf MyBase.zHack_OnWin
		MyBase.timeline = Me.properties.CreateTimeline(MyBase.mode)
		Me.goalTimes = Me.properties.goalTimes
		AddHandler Me.properties.OnBossDamaged, AddressOf MyBase.timeline.DealDamage
		MyBase.PartialInit()
	End Sub

	' Token: 0x170000CD RID: 205
	' (get) Token: 0x06000457 RID: 1111 RVA: 0x0006311A File Offset: 0x0006151A
	Public Overrides ReadOnly Property CurrentLevel As Levels
		Get
			Return Levels.Flower
		End Get
	End Property

	' Token: 0x170000CE RID: 206
	' (get) Token: 0x06000458 RID: 1112 RVA: 0x00063121 File Offset: 0x00061521
	Public Overrides ReadOnly Property CurrentScene As Scenes
		Get
			Return Scenes.scene_level_flower
		End Get
	End Property

	' Token: 0x170000CF RID: 207
	' (get) Token: 0x06000459 RID: 1113 RVA: 0x00063128 File Offset: 0x00061528
	Public Overrides ReadOnly Property BossPortrait As Sprite
		Get
			Select Case Me.properties.CurrentState.stateName
				Case LevelProperties.Flower.States.Main, LevelProperties.Flower.States.Generic
					Return Me._bossPortraitMain
				Case LevelProperties.Flower.States.PhaseTwo
					Return Me._bossPortraitPhaseTwo
				Case Else
					Global.Debug.LogError("Couldn't find portrait for state " + Me.properties.CurrentState.stateName + ". Using Main.", Nothing)
					Return Me._bossPortraitMain
			End Select
		End Get
	End Property

	' Token: 0x170000D0 RID: 208
	' (get) Token: 0x0600045A RID: 1114 RVA: 0x0006319C File Offset: 0x0006159C
	Public Overrides ReadOnly Property BossQuote As String
		Get
			Select Case Me.properties.CurrentState.stateName
				Case LevelProperties.Flower.States.Main, LevelProperties.Flower.States.Generic
					Return Me._bossQuoteMain
				Case LevelProperties.Flower.States.PhaseTwo
					Return Me._bossQuotePhaseTwo
				Case Else
					Global.Debug.LogError("Couldn't find quote for state " + Me.properties.CurrentState.stateName + ". Using Main.", Nothing)
					Return Me._bossQuoteMain
			End Select
		End Get
	End Property

	' Token: 0x0600045B RID: 1115 RVA: 0x0006320F File Offset: 0x0006160F
	Protected Overrides Sub Start()
		MyBase.Start()
		Me.flower.LevelInit(Me.properties)
	End Sub

	' Token: 0x0600045C RID: 1116 RVA: 0x00063228 File Offset: 0x00061628
	Protected Overrides Sub OnLevelStart()
		MyBase.StartCoroutine(Me.flowerPattern_cr())
	End Sub

	' Token: 0x0600045D RID: 1117 RVA: 0x00063238 File Offset: 0x00061638
	Protected Overrides Sub OnStateChanged()
		MyBase.OnStateChanged()
		If Me.properties.CurrentState.stateName = LevelProperties.Flower.States.PhaseTwo Then
			If Level.Current.mode = Level.Mode.Easy Then
				Me.properties.WinInstantly()
				AudioManager.PlayLoop("flower_phase1_death_loop")
				AudioManager.Play("flower_phase1_death_scream")
			Else
				Me.StopAllCoroutines()
				Me.flower.PhaseTwoTrigger()
			End If
		End If
	End Sub

	' Token: 0x0600045E RID: 1118 RVA: 0x000632A5 File Offset: 0x000616A5
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me._bossPortraitMain = Nothing
		Me._bossPortraitPhaseTwo = Nothing
	End Sub

	' Token: 0x0600045F RID: 1119 RVA: 0x000632BC File Offset: 0x000616BC
	Private Iterator Function flowerPattern_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 1F)
		While True
			Yield MyBase.StartCoroutine(Me.nextPattern_cr())
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06000460 RID: 1120 RVA: 0x000632D8 File Offset: 0x000616D8
	Private Iterator Function nextPattern_cr() As IEnumerator
		Select Case Me.properties.CurrentState.NextPattern
			Case LevelProperties.Flower.Pattern.Laser
				Yield MyBase.StartCoroutine(Me.laserAttack_cr())
			Case LevelProperties.Flower.Pattern.PodHands
				Yield MyBase.StartCoroutine(Me.potHands_cr())
			Case LevelProperties.Flower.Pattern.GattlingGun
				Yield MyBase.StartCoroutine(Me.gattlingGun_cr())
			Case Else
				Yield CupheadTime.WaitForSeconds(Me, 1F)
		End Select
		Return
	End Function

	' Token: 0x06000461 RID: 1121 RVA: 0x000632F4 File Offset: 0x000616F4
	Private Iterator Function laserAttack_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, Me.properties.CurrentState.laser.hesitateAfterAttack)
		Me.attacking = True
		Me.flower.StartLaser(AddressOf Me.OnLaserAttackComplete)
		While Me.attacking
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06000462 RID: 1122 RVA: 0x0006330F File Offset: 0x0006170F
	Private Sub OnLaserAttackComplete()
		Me.attacking = False
	End Sub

	' Token: 0x06000463 RID: 1123 RVA: 0x00063318 File Offset: 0x00061718
	Private Iterator Function potHands_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, Me.properties.CurrentState.podHands.hesitateAfterAttack)
		Me.attacking = True
		Me.flower.StartPotHands(AddressOf Me.OnPotHandsAttackComplete)
		While Me.attacking
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06000464 RID: 1124 RVA: 0x00063333 File Offset: 0x00061733
	Private Sub OnPotHandsAttackComplete()
		Me.attacking = False
	End Sub

	' Token: 0x06000465 RID: 1125 RVA: 0x0006333C File Offset: 0x0006173C
	Private Iterator Function gattlingGun_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, Me.properties.CurrentState.gattlingGun.hesitateAfterAttack)
		Me.attacking = True
		Me.flower.StartGattlingGun(AddressOf Me.OnGattlingGunComplete)
		While Me.attacking
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06000466 RID: 1126 RVA: 0x00063357 File Offset: 0x00061757
	Private Sub OnGattlingGunComplete()
		Me.attacking = False
	End Sub

	' Token: 0x04000757 RID: 1879
	Private properties As LevelProperties.Flower

	' Token: 0x04000758 RID: 1880
	<SerializeField()>
	Public flower As FlowerLevelFlower

	' Token: 0x04000759 RID: 1881
	Private attacking As Boolean

	' Token: 0x0400075A RID: 1882
	<Header("Boss Info")>
	<SerializeField()>
	Private _bossPortraitMain As Sprite

	' Token: 0x0400075B RID: 1883
	<SerializeField()>
	Private _bossPortraitPhaseTwo As Sprite

	' Token: 0x0400075C RID: 1884
	<SerializeField()>
	Private _bossQuoteMain As String

	' Token: 0x0400075D RID: 1885
	<SerializeField()>
	Private _bossQuotePhaseTwo As String

	' Token: 0x02000603 RID: 1539
	<Serializable()>
	Public Class Prefabs
	End Class
End Class
