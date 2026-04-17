Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020002AC RID: 684
Public Class SlimeLevel
	Inherits Level

	' Token: 0x06000794 RID: 1940 RVA: 0x00075D34 File Offset: 0x00074134
	Protected Overrides Sub PartialInit()
		Me.properties = LevelProperties.Slime.GetMode(MyBase.mode)
		AddHandler Me.properties.OnStateChange, AddressOf MyBase.zHack_OnStateChanged
		AddHandler Me.properties.OnBossDeath, AddressOf MyBase.zHack_OnWin
		MyBase.timeline = Me.properties.CreateTimeline(MyBase.mode)
		Me.goalTimes = Me.properties.goalTimes
		AddHandler Me.properties.OnBossDamaged, AddressOf MyBase.timeline.DealDamage
		MyBase.PartialInit()
	End Sub

	' Token: 0x1700013B RID: 315
	' (get) Token: 0x06000795 RID: 1941 RVA: 0x00075DCA File Offset: 0x000741CA
	Public Overrides ReadOnly Property CurrentLevel As Levels
		Get
			Return Levels.Slime
		End Get
	End Property

	' Token: 0x1700013C RID: 316
	' (get) Token: 0x06000796 RID: 1942 RVA: 0x00075DD1 File Offset: 0x000741D1
	Public Overrides ReadOnly Property CurrentScene As Scenes
		Get
			Return Scenes.scene_level_slime
		End Get
	End Property

	' Token: 0x1700013D RID: 317
	' (get) Token: 0x06000797 RID: 1943 RVA: 0x00075DD8 File Offset: 0x000741D8
	Public Overrides ReadOnly Property BossPortrait As Sprite
		Get
			Select Case Me.properties.CurrentState.stateName
				Case LevelProperties.Slime.States.Main, LevelProperties.Slime.States.Generic
					Return Me._bossPortraitMain
				Case LevelProperties.Slime.States.BigSlime
					Return Me._bossPortraitBigSlime
				Case LevelProperties.Slime.States.Tombstone
					Return Me._bossPortraitTombstone
				Case Else
					Global.Debug.LogError("Couldn't find portrait for state " + Me.properties.CurrentState.stateName + ". Using Main.", Nothing)
					Return Me._bossPortraitMain
			End Select
		End Get
	End Property

	' Token: 0x1700013E RID: 318
	' (get) Token: 0x06000798 RID: 1944 RVA: 0x00075E58 File Offset: 0x00074258
	Public Overrides ReadOnly Property BossQuote As String
		Get
			Select Case Me.properties.CurrentState.stateName
				Case LevelProperties.Slime.States.Main, LevelProperties.Slime.States.Generic
					Return Me._bossQuoteMain
				Case LevelProperties.Slime.States.BigSlime
					Return Me._bossQuoteBigSlime
				Case LevelProperties.Slime.States.Tombstone
					Return Me._bossQuoteTombstone
				Case Else
					Global.Debug.LogError("Couldn't find quote for state " + Me.properties.CurrentState.stateName + ". Using Main.", Nothing)
					Return Me._bossQuoteMain
			End Select
		End Get
	End Property

	' Token: 0x06000799 RID: 1945 RVA: 0x00075ED6 File Offset: 0x000742D6
	Protected Overrides Sub Start()
		MyBase.Start()
		Me.smallSlime.LevelInit(Me.properties)
		Me.bigSlime.LevelInit(Me.properties)
		Me.tombStone.LevelInit(Me.properties)
	End Sub

	' Token: 0x0600079A RID: 1946 RVA: 0x00075F11 File Offset: 0x00074311
	Protected Overrides Sub OnLevelStart()
		Me.smallSlime.IntroContinue()
		MyBase.StartCoroutine(Me.slimePattern_cr())
	End Sub

	' Token: 0x0600079B RID: 1947 RVA: 0x00075F2C File Offset: 0x0007432C
	Protected Overrides Sub OnStateChanged()
		MyBase.OnStateChanged()
		If Me.properties.CurrentState.stateName = LevelProperties.Slime.States.BigSlime Then
			Me.reachedBigSlimeState = True
			Me.StopAllCoroutines()
			Me.smallSlime.Transform()
		ElseIf Me.properties.CurrentState.stateName = LevelProperties.Slime.States.Tombstone Then
			Me.StopAllCoroutines()
			Me.bigSlime.DeathTransform()
		End If
		If Not Me.reachedBigSlimeState Then
			Me.smallSlime.CurrentPropertyState = Me.properties.CurrentState
		End If
		Me.bigSlime.CurrentPropertyState = Me.properties.CurrentState
	End Sub

	' Token: 0x0600079C RID: 1948 RVA: 0x00075FD0 File Offset: 0x000743D0
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me._bossPortraitBigSlime = Nothing
		Me._bossPortraitMain = Nothing
		Me._bossPortraitTombstone = Nothing
	End Sub

	' Token: 0x0600079D RID: 1949 RVA: 0x00075FF0 File Offset: 0x000743F0
	Private Iterator Function slimePattern_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 1F)
		While True
			Yield MyBase.StartCoroutine(Me.nextPattern_cr())
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x0600079E RID: 1950 RVA: 0x0007600C File Offset: 0x0007440C
	Private Iterator Function nextPattern_cr() As IEnumerator
		Dim p As LevelProperties.Slime.Pattern = Me.properties.CurrentState.NextPattern
		If p <> LevelProperties.Slime.Pattern.Jump Then
			Yield CupheadTime.WaitForSeconds(Me, 1F)
		Else
			Yield Nothing
		End If
		Return
	End Function

	' Token: 0x04000F53 RID: 3923
	Private properties As LevelProperties.Slime

	' Token: 0x04000F54 RID: 3924
	<SerializeField()>
	Private smallSlime As SlimeLevelSlime

	' Token: 0x04000F55 RID: 3925
	<SerializeField()>
	Private bigSlime As SlimeLevelSlime

	' Token: 0x04000F56 RID: 3926
	<SerializeField()>
	Private tombStone As SlimeLevelTombstone

	' Token: 0x04000F57 RID: 3927
	Private reachedBigSlimeState As Boolean

	' Token: 0x04000F58 RID: 3928
	Private damageDealer As DamageDealer

	' Token: 0x04000F59 RID: 3929
	Private damageReceiver As DamageReceiver

	' Token: 0x04000F5A RID: 3930
	<Header("Boss Info")>
	<SerializeField()>
	Private _bossPortraitMain As Sprite

	' Token: 0x04000F5B RID: 3931
	<SerializeField()>
	Private _bossPortraitBigSlime As Sprite

	' Token: 0x04000F5C RID: 3932
	<SerializeField()>
	Private _bossPortraitTombstone As Sprite

	' Token: 0x04000F5D RID: 3933
	<SerializeField()>
	Private _bossQuoteMain As String

	' Token: 0x04000F5E RID: 3934
	<SerializeField()>
	Private _bossQuoteBigSlime As String

	' Token: 0x04000F5F RID: 3935
	<SerializeField()>
	Private _bossQuoteTombstone As String
End Class
