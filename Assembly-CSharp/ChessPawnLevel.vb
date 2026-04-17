Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports UnityEngine

' Token: 0x020000AF RID: 175
Public Class ChessPawnLevel
	Inherits ChessLevel

	' Token: 0x06000206 RID: 518 RVA: 0x0005ADD8 File Offset: 0x000591D8
	Protected Overrides Sub PartialInit()
		Me.properties = LevelProperties.ChessPawn.GetMode(MyBase.mode)
		AddHandler Me.properties.OnStateChange, AddressOf MyBase.zHack_OnStateChanged
		AddHandler Me.properties.OnBossDeath, AddressOf MyBase.zHack_OnWin
		MyBase.timeline = Me.properties.CreateTimeline(MyBase.mode)
		Me.goalTimes = Me.properties.goalTimes
		AddHandler Me.properties.OnBossDamaged, AddressOf MyBase.timeline.DealDamage
		MyBase.PartialInit()
	End Sub

	' Token: 0x17000054 RID: 84
	' (get) Token: 0x06000207 RID: 519 RVA: 0x0005AE6E File Offset: 0x0005926E
	Public Overrides ReadOnly Property CurrentLevel As Levels
		Get
			Return Levels.ChessPawn
		End Get
	End Property

	' Token: 0x17000055 RID: 85
	' (get) Token: 0x06000208 RID: 520 RVA: 0x0005AE75 File Offset: 0x00059275
	Public Overrides ReadOnly Property CurrentScene As Scenes
		Get
			Return Scenes.scene_level_chess_pawn
		End Get
	End Property

	' Token: 0x17000056 RID: 86
	' (get) Token: 0x06000209 RID: 521 RVA: 0x0005AE79 File Offset: 0x00059279
	Public Overrides ReadOnly Property BossPortrait As Sprite
		Get
			Return Me._bossPortraitMain
		End Get
	End Property

	' Token: 0x17000057 RID: 87
	' (get) Token: 0x0600020A RID: 522 RVA: 0x0005AE81 File Offset: 0x00059281
	Public Overrides ReadOnly Property BossQuote As String
		Get
			Return Me._bossQuoteMain
		End Get
	End Property

	' Token: 0x0600020B RID: 523 RVA: 0x0005AE89 File Offset: 0x00059289
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me._bossPortraitMain = Nothing
		RemoveHandler MyBase.OnIntroEvent, AddressOf Me.onIntroEvent
		Me.pawn = Nothing
		Me.pawns = Nothing
	End Sub

	' Token: 0x0600020C RID: 524 RVA: 0x0005AEB8 File Offset: 0x000592B8
	Protected Overrides Sub Start()
		Level.IsChessBoss = True
		MyBase.Start()
		AddHandler MyBase.OnIntroEvent, AddressOf Me.onIntroEvent
		Dim num As Integer = CInt(Me.properties.TotalHealth) / 10
		Me.pawns = New ChessPawnLevelPawn(num - 1) {}
		For i As Integer = 0 To num - 1
			Me.pawns(i) = Me.pawn.Init(Me)
			Me.pawns(i).transform.position = Me.GetPosition(i) + New Vector3(0F, 300F)
			Me.pawns(i).SetIndex(i)
		Next
	End Sub

	' Token: 0x0600020D RID: 525 RVA: 0x0005AF60 File Offset: 0x00059360
	Protected Overrides Sub OnLevelStart()
		MyBase.StartCoroutine(Me.main_cr())
	End Sub

	' Token: 0x0600020E RID: 526 RVA: 0x0005AF6F File Offset: 0x0005936F
	Public Sub TakeDamage()
		Me.properties.DealDamage(10F)
		If Me.properties.CurrentHealth <= 0F Then
			Me.die()
		End If
	End Sub

	' Token: 0x0600020F RID: 527 RVA: 0x0005AF9C File Offset: 0x0005939C
	Private Sub onIntroEvent()
		For i As Integer = 0 To Me.pawns.Length - 1
			Me.pawns(i).StartIntro()
			Me.SFX_KOG_PAWN_IntroJeers()
		Next
	End Sub

	' Token: 0x06000210 RID: 528 RVA: 0x0005AFD8 File Offset: 0x000593D8
	Private Iterator Function main_cr() As IEnumerator
		Dim p As LevelProperties.ChessPawn.Pawn = Me.properties.CurrentState.pawn
		Dim pawnAttackDelay As PatternString = New PatternString(p.pawnAttackDelayString, True)
		Dim pawnDirection As PatternString = New PatternString(p.pawnDirectionString, True)
		Dim pawnOrder As PatternString = New PatternString(p.pawnOrderString, True)
		While True
			Dim pink As Boolean = pawnOrder.PopLetter() = "P"c
			Dim availableList As List(Of Integer) = New List(Of Integer)()
			For j As Integer = 0 To Me.pawns.Length - 1
				If pink = Me.pawns(j).CanParry AndAlso Not Me.pawns(j).inUse Then
					availableList.Add(j)
				End If
			Next
			If availableList.Count > 0 Then
				Dim dir As Single = 0F
				Dim c As Char = pawnDirection.PopLetter()
				If c <> "L"c Then
					If c <> "D"c Then
						If c = "R"c Then
							dir = 1F
						End If
					Else
						dir = 0F
					End If
				Else
					dir = -1F
				End If
				Dim i As Integer = Global.UnityEngine.Random.Range(0, availableList.Count)
				If Mathf.Abs(Me.GetPosition(Me.pawns(availableList(i)).currentIndex).x + dir * p.pawnDropDistance) > 800F Then
					dir = 0F
				End If
				dir *= p.pawnDropDistance
				Me.pawns(availableList(i)).Attack(p.pawnWarningTime, dir, p.pawnDropSpeed, p.pawnRunHesitation, p.pawnRunSpeed, p.pawnReturnDelay)
				Yield CupheadTime.WaitForSeconds(Me, pawnAttackDelay.PopFloat() - p.pawnDelayReduction * CSng(Me.damageCount()))
			End If
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06000211 RID: 529 RVA: 0x0005AFF4 File Offset: 0x000593F4
	Private Sub die()
		If Me.dead Then
			Return
		End If
		Me.dead = True
		Me.SFX_KOG_PAWN_BeatLevelHarp()
		For Each chessPawnLevelPawn As ChessPawnLevelPawn In Me.pawns
			chessPawnLevelPawn.Death()
		Next
	End Sub

	' Token: 0x06000212 RID: 530 RVA: 0x0005B03F File Offset: 0x0005943F
	Private Function damageCount() As Integer
		Return CInt((Me.properties.TotalHealth - Me.properties.CurrentHealth)) / 10
	End Function

	' Token: 0x06000213 RID: 531 RVA: 0x0005B05C File Offset: 0x0005945C
	Public Function GetReturnIndex() As Integer
		Dim list As List(Of Integer) = New List(Of Integer)()
		For i As Integer = 0 To Me.pawns.Length - 1
			list.Add(i)
		Next
		For j As Integer = 0 To Me.pawns.Length - 1
			list.Remove(Me.pawns(j).currentIndex)
		Next
		Return list(Global.UnityEngine.Random.Range(0, list.Count))
	End Function

	' Token: 0x06000214 RID: 532 RVA: 0x0005B0CE File Offset: 0x000594CE
	Public Function GetPosition(index As Integer) As Vector3
		Return New Vector3(-622F + CSng(index) * 180F, 340F)
	End Function

	' Token: 0x06000215 RID: 533 RVA: 0x0005B0E8 File Offset: 0x000594E8
	Public Function ClearToRun(testDir As Single, pos As Vector3) As Boolean
		Dim flag As Boolean = True
		For i As Integer = 0 To Me.pawns.Length - 1
			If Me.pawns(i).speed <> 0F AndAlso Mathf.Sign(Me.pawns(i).speed) = testDir AndAlso Vector3.Distance(Me.pawns(i).transform.position, pos) < 200F Then
				flag = False
			End If
		Next
		Return flag
	End Function

	' Token: 0x06000216 RID: 534 RVA: 0x0005B164 File Offset: 0x00059564
	Private Sub SFX_KOG_PAWN_IntroJeers()
		AudioManager.Play("sfx_DLC_KOG_Pawn_IntroJeers")
	End Sub

	' Token: 0x06000217 RID: 535 RVA: 0x0005B170 File Offset: 0x00059570
	Private Sub SFX_KOG_PAWN_BeatLevelHarp()
		AudioManager.Play("sfx_dlc_kog_pawn_beatlevelharp")
	End Sub

	' Token: 0x04000396 RID: 918
	Private properties As LevelProperties.ChessPawn

	' Token: 0x04000397 RID: 919
	Private Const WAIT_TO_RUN_DIST As Single = 200F

	' Token: 0x04000398 RID: 920
	Private Const SPACING As Single = 180F

	' Token: 0x04000399 RID: 921
	Private Const LEFTMOST_X As Single = -622F

	' Token: 0x0400039A RID: 922
	<Header("Boss Info")>
	<SerializeField()>
	Private _bossPortraitMain As Sprite

	' Token: 0x0400039B RID: 923
	<SerializeField()>
	Private _bossQuoteMain As String

	' Token: 0x0400039C RID: 924
	<SerializeField()>
	Private pawn As ChessPawnLevelPawn

	' Token: 0x0400039D RID: 925
	Private pawns As ChessPawnLevelPawn()

	' Token: 0x0400039E RID: 926
	Private dead As Boolean
End Class
