Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020002ED RID: 749
Public Class VeggiesLevel
	Inherits Level

	' Token: 0x0600084E RID: 2126 RVA: 0x00078BE4 File Offset: 0x00076FE4
	Protected Overrides Sub PartialInit()
		Me.properties = LevelProperties.Veggies.GetMode(MyBase.mode)
		AddHandler Me.properties.OnStateChange, AddressOf MyBase.zHack_OnStateChanged
		AddHandler Me.properties.OnBossDeath, AddressOf MyBase.zHack_OnWin
		MyBase.timeline = Me.properties.CreateTimeline(MyBase.mode)
		Me.goalTimes = Me.properties.goalTimes
		AddHandler Me.properties.OnBossDamaged, AddressOf MyBase.timeline.DealDamage
		MyBase.PartialInit()
	End Sub

	' Token: 0x1700015B RID: 347
	' (get) Token: 0x0600084F RID: 2127 RVA: 0x00078C7A File Offset: 0x0007707A
	Public Overrides ReadOnly Property CurrentLevel As Levels
		Get
			Return Levels.Veggies
		End Get
	End Property

	' Token: 0x1700015C RID: 348
	' (get) Token: 0x06000850 RID: 2128 RVA: 0x00078C7D File Offset: 0x0007707D
	Public Overrides ReadOnly Property CurrentScene As Scenes
		Get
			Return Scenes.scene_level_veggies
		End Get
	End Property

	' Token: 0x1700015D RID: 349
	' (get) Token: 0x06000851 RID: 2129 RVA: 0x00078C84 File Offset: 0x00077084
	Public Overrides ReadOnly Property BossPortrait As Sprite
		Get
			Select Case Me.currentBoss
				Case VeggiesLevel.CurrentBoss.Potato
					Return Me._bossPortraitPotato
				Case VeggiesLevel.CurrentBoss.Onion
					Return Me._bossPortraitOnion
				Case VeggiesLevel.CurrentBoss.Carrot
					Return Me._bossPortraitCarrot
			End Select
			Return Me._bossPortraitPotato
		End Get
	End Property

	' Token: 0x1700015E RID: 350
	' (get) Token: 0x06000852 RID: 2130 RVA: 0x00078CD4 File Offset: 0x000770D4
	Public Overrides ReadOnly Property BossQuote As String
		Get
			Select Case Me.currentBoss
				Case VeggiesLevel.CurrentBoss.Potato
					Return Me._bossQuotePotato
				Case VeggiesLevel.CurrentBoss.Onion
					Return Me._bossQuoteOnion
				Case VeggiesLevel.CurrentBoss.Carrot
					Return Me._bossQuoteCarrot
			End Select
			Return "QuoteNone"
		End Get
	End Property

	' Token: 0x06000853 RID: 2131 RVA: 0x00078D24 File Offset: 0x00077124
	Protected Overrides Sub Start()
		MyBase.Start()
		MyBase.StartCoroutine(Me.potatoStart_cr())
		RemoveHandler Me.properties.OnBossDamaged, AddressOf MyBase.timeline.DealDamage
		MyBase.timeline = New Level.Timeline()
		MyBase.timeline.health = 0F
		MyBase.timeline.health += CSng(Me.properties.CurrentState.potato.hp)
		If MyBase.mode <> Level.Mode.Easy Then
			MyBase.timeline.health += CSng(Me.properties.CurrentState.onion.hp)
		End If
		MyBase.timeline.health += CSng(Me.properties.CurrentState.carrot.hp)
		If MyBase.mode <> Level.Mode.Easy Then
			MyBase.timeline.AddEventAtHealth("Onion", MyBase.timeline.GetHealthOfLastEvent() + Me.properties.CurrentState.potato.hp)
		End If
		MyBase.timeline.AddEventAtHealth("Carrot", MyBase.timeline.GetHealthOfLastEvent() + If((MyBase.mode <> Level.Mode.Easy), Me.properties.CurrentState.onion.hp, Me.properties.CurrentState.potato.hp))
	End Sub

	' Token: 0x06000854 RID: 2132 RVA: 0x00078E90 File Offset: 0x00077290
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me.prefabs = Nothing
		Me._bossPortraitCarrot = Nothing
		Me._bossPortraitOnion = Nothing
		Me._bossPortraitPotato = Nothing
	End Sub

	' Token: 0x06000855 RID: 2133 RVA: 0x00078EB4 File Offset: 0x000772B4
	Protected Overrides Sub OnLevelStart()
		MyBase.StartCoroutine(Me.veggiesPattern_cr())
	End Sub

	' Token: 0x06000856 RID: 2134 RVA: 0x00078EC4 File Offset: 0x000772C4
	Private Iterator Function veggiesPattern_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 1F)
		Yield MyBase.StartCoroutine(Me.potato_cr())
		If MyBase.mode <> Level.Mode.Easy Then
			Yield MyBase.StartCoroutine(Me.onion_cr())
		End If
		Yield MyBase.StartCoroutine(Me.carrot_cr())
		Yield MyBase.StartCoroutine(Me.win_cr())
		Return
	End Function

	' Token: 0x06000857 RID: 2135 RVA: 0x00078EE0 File Offset: 0x000772E0
	Private Iterator Function potatoStart_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 1F)
		Me.potato = Me.prefabs.potato.InstantiatePrefab(Of VeggiesLevelPotato)()
		AddHandler Me.potato.OnDamageTakenEvent, AddressOf MyBase.timeline.DealDamage
		Return
	End Function

	' Token: 0x06000858 RID: 2136 RVA: 0x00078EFC File Offset: 0x000772FC
	Private Iterator Function potato_cr() As IEnumerator
		Me.currentBoss = VeggiesLevel.CurrentBoss.Potato
		Me.potato.LevelInitWithGroup(Me.properties.CurrentState.potato)
		While Me.potato.state <> VeggiesLevelPotato.State.Complete
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06000859 RID: 2137 RVA: 0x00078F18 File Offset: 0x00077318
	Private Iterator Function onion_cr() As IEnumerator
		Me.currentBoss = VeggiesLevel.CurrentBoss.Onion
		Yield CupheadTime.WaitForSeconds(Me, 1F)
		Dim v As VeggiesLevelOnion = Me.prefabs.onion.InstantiatePrefab(Of VeggiesLevelOnion)()
		v.LevelInitWithGroup(Me.properties.CurrentState.onion)
		AddHandler v.OnDamageTakenEvent, AddressOf MyBase.timeline.DealDamage
		AddHandler v.OnHappyLeave, AddressOf Me.OnionHappyLeave
		While v.state <> VeggiesLevelOnion.State.Complete
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x0600085A RID: 2138 RVA: 0x00078F33 File Offset: 0x00077333
	Private Sub OnionHappyLeave()
		Me.secretTriggered = True
		MyBase.timeline.DealDamage(CSng(Me.properties.CurrentState.onion.hp))
	End Sub

	' Token: 0x0600085B RID: 2139 RVA: 0x00078F60 File Offset: 0x00077360
	Private Iterator Function beet_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 2F)
		Dim v As VeggiesLevelBeet = Me.prefabs.beet.InstantiatePrefab(Of VeggiesLevelBeet)()
		v.LevelInitWithGroup(Me.properties.CurrentState.beet)
		AddHandler v.OnDamageTakenEvent, AddressOf MyBase.timeline.DealDamage
		While v.state <> VeggiesLevelBeet.State.Complete
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x0600085C RID: 2140 RVA: 0x00078F7C File Offset: 0x0007737C
	Private Iterator Function peas_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 2F)
		Dim v As VeggiesLevelPeas = Me.prefabs.peas.InstantiatePrefab(Of VeggiesLevelPeas)()
		v.LevelInitWithGroup(Me.properties.CurrentState.peas)
		AddHandler v.OnDamageTakenEvent, AddressOf MyBase.timeline.DealDamage
		While v.state <> VeggiesLevelPeas.State.Complete
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x0600085D RID: 2141 RVA: 0x00078F98 File Offset: 0x00077398
	Private Iterator Function carrot_cr() As IEnumerator
		Me.currentBoss = VeggiesLevel.CurrentBoss.Carrot
		Dim v As VeggiesLevelCarrot = Me.prefabs.carrot.InstantiatePrefab(Of VeggiesLevelCarrot)()
		v.LevelInit(Me.properties)
		AddHandler v.OnDamageTakenEvent, AddressOf MyBase.timeline.DealDamage
		While v.state <> VeggiesLevelCarrot.State.Complete
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x0600085E RID: 2142 RVA: 0x00078FB4 File Offset: 0x000773B4
	Private Iterator Function win_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 1F)
		Me.properties.WinInstantly()
		Return
	End Function

	' Token: 0x040010E2 RID: 4322
	Private properties As LevelProperties.Veggies

	' Token: 0x040010E3 RID: 4323
	<Space(10F)>
	<SerializeField()>
	Private prefabs As VeggiesLevel.Prefabs

	' Token: 0x040010E4 RID: 4324
	Private potato As VeggiesLevelPotato

	' Token: 0x040010E5 RID: 4325
	<Header("Boss Info")>
	<SerializeField()>
	Private _bossPortraitPotato As Sprite

	' Token: 0x040010E6 RID: 4326
	<SerializeField()>
	Private _bossPortraitOnion As Sprite

	' Token: 0x040010E7 RID: 4327
	<SerializeField()>
	Private _bossPortraitCarrot As Sprite

	' Token: 0x040010E8 RID: 4328
	<SerializeField()>
	Private _bossQuotePotato As String

	' Token: 0x040010E9 RID: 4329
	<SerializeField()>
	Private _bossQuoteOnion As String

	' Token: 0x040010EA RID: 4330
	<SerializeField()>
	Private _bossQuoteCarrot As String

	' Token: 0x040010EB RID: 4331
	Private currentBoss As VeggiesLevel.CurrentBoss

	' Token: 0x02000838 RID: 2104
	Private Enum CurrentBoss
		' Token: 0x04003966 RID: 14694
		Potato
		' Token: 0x04003967 RID: 14695
		Onion
		' Token: 0x04003968 RID: 14696
		Beet
		' Token: 0x04003969 RID: 14697
		Peas
		' Token: 0x0400396A RID: 14698
		Carrot
	End Enum

	' Token: 0x02000839 RID: 2105
	<Serializable()>
	Public Class Prefabs
		' Token: 0x0400396B RID: 14699
		Public potato As VeggiesLevelPotato

		' Token: 0x0400396C RID: 14700
		Public onion As VeggiesLevelOnion

		' Token: 0x0400396D RID: 14701
		Public beet As VeggiesLevelBeet

		' Token: 0x0400396E RID: 14702
		Public peas As VeggiesLevelPeas

		' Token: 0x0400396F RID: 14703
		Public carrot As VeggiesLevelCarrot
	End Class
End Class
