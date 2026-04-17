Imports System
Imports System.Collections
Imports System.Diagnostics
Imports UnityEngine

' Token: 0x0200023B RID: 571
Public Class PirateLevel
	Inherits Level

	' Token: 0x06000667 RID: 1639 RVA: 0x0006F88C File Offset: 0x0006DC8C
	Protected Overrides Sub PartialInit()
		Me.properties = LevelProperties.Pirate.GetMode(MyBase.mode)
		AddHandler Me.properties.OnStateChange, AddressOf MyBase.zHack_OnStateChanged
		AddHandler Me.properties.OnBossDeath, AddressOf MyBase.zHack_OnWin
		MyBase.timeline = Me.properties.CreateTimeline(MyBase.mode)
		Me.goalTimes = Me.properties.goalTimes
		AddHandler Me.properties.OnBossDamaged, AddressOf MyBase.timeline.DealDamage
		MyBase.PartialInit()
	End Sub

	' Token: 0x17000116 RID: 278
	' (get) Token: 0x06000668 RID: 1640 RVA: 0x0006F922 File Offset: 0x0006DD22
	Public Overrides ReadOnly Property CurrentLevel As Levels
		Get
			Return Levels.Pirate
		End Get
	End Property

	' Token: 0x17000117 RID: 279
	' (get) Token: 0x06000669 RID: 1641 RVA: 0x0006F925 File Offset: 0x0006DD25
	Public Overrides ReadOnly Property CurrentScene As Scenes
		Get
			Return Scenes.scene_level_pirate
		End Get
	End Property

	' Token: 0x14000001 RID: 1
	' (add) Token: 0x0600066A RID: 1642 RVA: 0x0006F92C File Offset: 0x0006DD2C
	' (remove) Token: 0x0600066B RID: 1643 RVA: 0x0006F964 File Offset: 0x0006DD64
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnWhistleEvent As PirateLevel.WhistleDelegate

	' Token: 0x17000118 RID: 280
	' (get) Token: 0x0600066C RID: 1644 RVA: 0x0006F99C File Offset: 0x0006DD9C
	Public Overrides ReadOnly Property BossPortrait As Sprite
		Get
			Select Case Me.properties.CurrentState.stateName
				Case LevelProperties.Pirate.States.Main, LevelProperties.Pirate.States.Generic
					Return Me._bossPortraitMain
				Case LevelProperties.Pirate.States.Boat
					Return Me._bossPortraitBoat
				Case Else
					Global.Debug.LogError("Couldn't find portrait for state " + Me.properties.CurrentState.stateName + ". Using Main.", Nothing)
					Return Me._bossPortraitMain
			End Select
		End Get
	End Property

	' Token: 0x17000119 RID: 281
	' (get) Token: 0x0600066D RID: 1645 RVA: 0x0006FA10 File Offset: 0x0006DE10
	Public Overrides ReadOnly Property BossQuote As String
		Get
			Select Case Me.properties.CurrentState.stateName
				Case LevelProperties.Pirate.States.Main, LevelProperties.Pirate.States.Generic
					Return Me._bossQuoteMain
				Case LevelProperties.Pirate.States.Boat
					Return Me._bossQuoteBoat
				Case Else
					Global.Debug.LogError("Couldn't find quote for state " + Me.properties.CurrentState.stateName + ". Using Main.", Nothing)
					Return Me._bossQuoteMain
			End Select
		End Get
	End Property

	' Token: 0x0600066E RID: 1646 RVA: 0x0006FA83 File Offset: 0x0006DE83
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.inkOverlay = Me.prefabs.inkOverlay.InstantiatePrefab(Of PirateLevelSquidInkOverlay)()
	End Sub

	' Token: 0x0600066F RID: 1647 RVA: 0x0006FAA4 File Offset: 0x0006DEA4
	Protected Overrides Sub Start()
		MyBase.Start()
		Me.barrel.LevelInit(Me.properties)
		Me.inkOverlay.LevelInit(Me.properties)
		Me.pirate.LevelInit(Me.properties)
		Me.boat.LevelInit(Me.properties)
	End Sub

	' Token: 0x06000670 RID: 1648 RVA: 0x0006FAFB File Offset: 0x0006DEFB
	Protected Overrides Sub OnLevelStart()
		MyBase.StartCoroutine(Me.piratePattern_cr())
	End Sub

	' Token: 0x06000671 RID: 1649 RVA: 0x0006FB0A File Offset: 0x0006DF0A
	Protected Overrides Sub OnStateChanged()
		MyBase.OnStateChanged()
	End Sub

	' Token: 0x06000672 RID: 1650 RVA: 0x0006FB12 File Offset: 0x0006DF12
	Private Sub StartBoat()
		Me.StopAllCoroutines()
		AddHandler Me.boat.OnLaunchPirate, AddressOf Me.OnBoatLaunchPirate
		Me.boat.StartTransformation()
	End Sub

	' Token: 0x06000673 RID: 1651 RVA: 0x0006FB3C File Offset: 0x0006DF3C
	Private Sub OnBoatLaunchPirate()
		MyBase.StartCoroutine(Me.launchPirate_cr())
	End Sub

	' Token: 0x06000674 RID: 1652 RVA: 0x0006FB4B File Offset: 0x0006DF4B
	Private Sub Whistle(creature As PirateLevel.Creature)
		If Me.OnWhistleEvent IsNot Nothing Then
			Me.OnWhistleEvent(creature)
		End If
	End Sub

	' Token: 0x06000675 RID: 1653 RVA: 0x0006FB64 File Offset: 0x0006DF64
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me.prefabs = Nothing
		Me._bossPortraitBoat = Nothing
		Me._bossPortraitMain = Nothing
	End Sub

	' Token: 0x06000676 RID: 1654 RVA: 0x0006FB84 File Offset: 0x0006DF84
	Private Iterator Function piratePattern_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 1F)
		While True
			Yield MyBase.StartCoroutine(Me.nextPattern_cr())
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06000677 RID: 1655 RVA: 0x0006FBA0 File Offset: 0x0006DFA0
	Private Iterator Function nextPattern_cr() As IEnumerator
		Select Case Me.properties.CurrentState.NextPattern
			Case LevelProperties.Pirate.Pattern.Shark
				Yield MyBase.StartCoroutine(Me.shark_cr())
			Case LevelProperties.Pirate.Pattern.Squid
				Yield MyBase.StartCoroutine(Me.squid_cr())
			Case LevelProperties.Pirate.Pattern.DogFish
				Yield MyBase.StartCoroutine(Me.dogFish_cr())
			Case LevelProperties.Pirate.Pattern.Peashot
				Yield MyBase.StartCoroutine(Me.peashot_cr())
			Case LevelProperties.Pirate.Pattern.Boat
				Me.StartBoat()
			Case Else
				Yield New WaitForSeconds(1F)
		End Select
		Return
	End Function

	' Token: 0x06000678 RID: 1656 RVA: 0x0006FBBC File Offset: 0x0006DFBC
	Private Iterator Function squid_cr() As IEnumerator
		Me.Whistle(PirateLevel.Creature.Squid)
		Yield CupheadTime.WaitForSeconds(Me, 1.5F)
		Yield CupheadTime.WaitForSeconds(Me, Me.properties.CurrentState.squid.startDelay)
		Dim squid As PirateLevelSquid = Me.prefabs.squid.InstantiatePrefab(Of PirateLevelSquid)()
		squid.LevelInit(Me.properties)
		While squid.state <> PirateLevelSquid.State.[Exit] AndAlso squid.state <> PirateLevelSquid.State.Die
			Yield Nothing
		End While
		Yield CupheadTime.WaitForSeconds(Me, CSng(Me.properties.CurrentState.squid.endDelay))
		Return
	End Function

	' Token: 0x06000679 RID: 1657 RVA: 0x0006FBD8 File Offset: 0x0006DFD8
	Private Iterator Function shark_cr() As IEnumerator
		Me.Whistle(PirateLevel.Creature.Shark)
		Yield CupheadTime.WaitForSeconds(Me, 1.5F)
		Yield CupheadTime.WaitForSeconds(Me, Me.properties.CurrentState.shark.startDelay)
		Dim shark As PirateLevelShark = Me.prefabs.shark.InstantiatePrefab(Of PirateLevelShark)()
		shark.LevelInitWithGroup(Me.properties.CurrentState.shark)
		While shark.state <> PirateLevelShark.State.Complete
			Yield Nothing
		End While
		Yield CupheadTime.WaitForSeconds(Me, Me.properties.CurrentState.shark.endDelay)
		Return
	End Function

	' Token: 0x0600067A RID: 1658 RVA: 0x0006FBF4 File Offset: 0x0006DFF4
	Private Iterator Function dogFish_cr() As IEnumerator
		Dim secretHitBox As Boolean = False
		Me.Whistle(PirateLevel.Creature.DogFish)
		Yield CupheadTime.WaitForSeconds(Me, 1.5F)
		Me.scope.[In]()
		Yield CupheadTime.WaitForSeconds(Me, Me.properties.CurrentState.dogFish.startDelay)
		Dim properties As LevelProperties.Pirate.DogFish = Me.properties.CurrentState.dogFish
		For i As Integer = 0 To properties.count - 1
			secretHitBox = i = 3
			Dim dogFish As PirateLevelDogFish = Me.prefabs.dogFish.InstantiatePrefab(Of PirateLevelDogFish)()
			dogFish.transform.SetPosition(New Single?(0F), New Single?(-210F), New Single?(0F))
			dogFish.Init(Me.properties, secretHitBox)
			Yield CupheadTime.WaitForSeconds(Me, properties.nextFishDelay)
		Next
		Yield CupheadTime.WaitForSeconds(Me, properties.endDelay)
		Return
	End Function

	' Token: 0x0600067B RID: 1659 RVA: 0x0006FC10 File Offset: 0x0006E010
	Private Iterator Function peashot_cr() As IEnumerator
		Dim properties As LevelProperties.Pirate.Peashot = Me.properties.CurrentState.peashot
		Dim pattern As KeyValue() = KeyValue.ListFromString(properties.patterns(Global.UnityEngine.Random.Range(0, properties.patterns.Length)), New Char() { "P"c, "D"c })
		Me.pirate.StartGun()
		Yield CupheadTime.WaitForSeconds(Me, properties.startDelay)
		For i As Integer = 0 To pattern.Length - 1
			If pattern(i).key = "P" Then
				Dim p As Integer = 0
				While CSng(p) < pattern(i).value
					Yield CupheadTime.WaitForSeconds(Me, properties.shotDelay)
					Me.pirate.FireGun(properties)
					p += 1
				End While
			Else
				Yield CupheadTime.WaitForSeconds(Me, pattern(i).value)
			End If
			Yield Nothing
		Next
		Me.pirate.EndGun()
		Yield CupheadTime.WaitForSeconds(Me, CSng(properties.endDelay))
		Return
	End Function

	' Token: 0x0600067C RID: 1660 RVA: 0x0006FC2C File Offset: 0x0006E02C
	Private Iterator Function launchPirate_cr() As IEnumerator
		Dim p As LevelProperties.Pirate.Boat = Me.properties.CurrentState.boat
		Me.deadPirate.Go(p.pirateFallDelay, p.pirateFallTime)
		Dim t As Single = 0F
		Dim time As Single = 1F
		Dim speed As Single = 1200F
		While t < time
			Dim y As Single = speed * CupheadTime.Delta
			For Each transform As Transform In Me.boatParts
				transform.AddPosition(0F, y, 0F)
			Next
			Me.pirate.transform.AddPosition(0F, y, 0F)
			t += CupheadTime.Delta
			Yield Nothing
		End While
		For Each transform2 As Transform In Me.boatParts
			Global.UnityEngine.[Object].Destroy(transform2.gameObject)
		Next
		Me.pirate.CleanUp()
		Return
	End Function

	' Token: 0x04000C29 RID: 3113
	Private properties As LevelProperties.Pirate

	' Token: 0x04000C2A RID: 3114
	Private Const WHISTLE_ANIM_TIME As Single = 1.5F

	' Token: 0x04000C2C RID: 3116
	<Space(10F)>
	Public pirate As PirateLevelPirate

	' Token: 0x04000C2D RID: 3117
	Public deadPirate As PirateLevelPirateDead

	' Token: 0x04000C2E RID: 3118
	Public boat As PirateLevelBoat

	' Token: 0x04000C2F RID: 3119
	Public barrel As PirateLevelBarrel

	' Token: 0x04000C30 RID: 3120
	Public scope As PirateLevelDogFishScope

	' Token: 0x04000C31 RID: 3121
	Public boatParts As Transform()

	' Token: 0x04000C32 RID: 3122
	<Space(10F)>
	<SerializeField()>
	Private prefabs As PirateLevel.Prefabs

	' Token: 0x04000C33 RID: 3123
	Private inkOverlay As PirateLevelSquidInkOverlay

	' Token: 0x04000C34 RID: 3124
	<Header("Boss Info")>
	<SerializeField()>
	Private _bossPortraitMain As Sprite

	' Token: 0x04000C35 RID: 3125
	<SerializeField()>
	Private _bossPortraitBoat As Sprite

	' Token: 0x04000C36 RID: 3126
	<SerializeField()>
	Private _bossQuoteMain As String

	' Token: 0x04000C37 RID: 3127
	<SerializeField()>
	Private _bossQuoteBoat As String

	' Token: 0x02000731 RID: 1841
	Public Enum Creature
		' Token: 0x040030E3 RID: 12515
		Squid
		' Token: 0x040030E4 RID: 12516
		Shark
		' Token: 0x040030E5 RID: 12517
		DogFish
	End Enum

	' Token: 0x02000732 RID: 1842
	' (Invoke) Token: 0x06002825 RID: 10277
	Public Delegate Sub WhistleDelegate(creature As PirateLevel.Creature)

	' Token: 0x02000733 RID: 1843
	<Serializable()>
	Public Class Prefabs
		' Token: 0x040030E6 RID: 12518
		Public squid As PirateLevelSquid

		' Token: 0x040030E7 RID: 12519
		Public shark As PirateLevelShark

		' Token: 0x040030E8 RID: 12520
		Public dogFish As PirateLevelDogFish

		' Token: 0x040030E9 RID: 12521
		<Space(10F)>
		Public inkOverlay As PirateLevelSquidInkOverlay
	End Class
End Class
