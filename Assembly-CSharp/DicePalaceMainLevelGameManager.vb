Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020005D5 RID: 1493
Public Class DicePalaceMainLevelGameManager
	Inherits LevelProperties.DicePalaceMain.Entity

	' Token: 0x06001D61 RID: 7521 RVA: 0x0010D24C File Offset: 0x0010B64C
	Public Overrides Sub LevelInit(properties As LevelProperties.DicePalaceMain)
		MyBase.LevelInit(properties)
		AddHandler Level.Current.OnIntroEvent, AddressOf Me.StartDice
		Me.kingDiceAni = Me.kingDice.GetComponent(Of Animator)()
		Me.maxSpaces = Me.allBoardSpaces.Length
		Me.GameSetup()
		Me.marker.position = Me.boardSpacesObj(DicePalaceMainLevelGameInfo.PLAYER_SPACES_MOVED).Pivot.position
		Me.marker.rotation = Me.boardSpacesObj(DicePalaceMainLevelGameInfo.PLAYER_SPACES_MOVED).Pivot.rotation
		If Not DicePalaceMainLevelGameInfo.PLAYED_INTRO_SFX Then
			AudioManager.Play("vox_intro")
			Me.emitAudioFromObject.Add("vox_intro")
			DicePalaceMainLevelGameInfo.PLAYED_INTRO_SFX = True
		End If
	End Sub

	' Token: 0x06001D62 RID: 7522 RVA: 0x0010D308 File Offset: 0x0010B708
	Public Sub GameSetup()
		Dim dice As LevelProperties.DicePalaceMain.Dice = MyBase.properties.CurrentState.dice
		Me.dice = Global.UnityEngine.[Object].Instantiate(Of DicePalaceMainLevelDice)(Me.dicePrefab)
		Me.dice.Init(Vector2.zero, dice, Me.pivotPoint1)
		Me.pivotPoint1.position = Me.dice.transform.position
		Me.CheckSafeSpaces()
		Me.CheckHearts()
	End Sub

	' Token: 0x06001D63 RID: 7523 RVA: 0x0010D378 File Offset: 0x0010B778
	Public Sub CheckSafeSpaces()
		For i As Integer = 0 To DicePalaceMainLevelGameInfo.SAFE_INDEXES.Count - 1
			Me.allBoardSpaces(DicePalaceMainLevelGameInfo.SAFE_INDEXES(i)) = DicePalaceMainLevelGameManager.BoardSpaces.FreeSpace
			Me.boardSpacesObj(DicePalaceMainLevelGameInfo.SAFE_INDEXES(i) + 1).Clear = True
		Next
	End Sub

	' Token: 0x06001D64 RID: 7524 RVA: 0x0010D3D0 File Offset: 0x0010B7D0
	Private Sub CheckHearts()
		For i As Integer = 0 To DicePalaceMainLevelGameInfo.HEART_INDEXES.Length - 1
			Me.boardSpacesObj(DicePalaceMainLevelGameInfo.HEART_INDEXES(i) + 1).HasHeart = True
		Next
	End Sub

	' Token: 0x06001D65 RID: 7525 RVA: 0x0010D40B File Offset: 0x0010B80B
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me.dicePrefab = Nothing
	End Sub

	' Token: 0x06001D66 RID: 7526 RVA: 0x0010D41A File Offset: 0x0010B81A
	Private Sub StartDice()
		If Level.IsTowerOfPower Then
			Me.EndBoardGame(Me.dice)
		Else
			MyBase.StartCoroutine(Me.check_for_rolled_cr())
		End If
	End Sub

	' Token: 0x06001D67 RID: 7527 RVA: 0x0010D444 File Offset: 0x0010B844
	Public Sub RevealDice()
		Me.dice.StartRoll()
	End Sub

	' Token: 0x06001D68 RID: 7528 RVA: 0x0010D454 File Offset: 0x0010B854
	Private Iterator Function check_for_rolled_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, MyBase.properties.CurrentState.dice.revealDelay)
		DicePalaceMainLevelGameInfo.IS_FIRST_ENTRY = False
		Me.kingDiceAni.SetTrigger("OnReveal")
		Yield Me.kingDiceAni.WaitForAnimationToEnd(Me.kingDice, "Dice_Reveal", False, True)
		Dim p As LevelProperties.DicePalaceMain.Dice = MyBase.properties.CurrentState.dice
		Dim spacesToMove As Integer = 0
		Dim playingGame As Boolean = True
		While playingGame
			While Me.dice.waitingToRoll
				Yield Nothing
			End While
			spacesToMove = 0
			Dim roll As DicePalaceMainLevelDice.Roll = Me.dice.roll
			If roll <> DicePalaceMainLevelDice.Roll.One Then
				If roll <> DicePalaceMainLevelDice.Roll.Two Then
					If roll = DicePalaceMainLevelDice.Roll.Three Then
						spacesToMove = 3
					End If
				Else
					spacesToMove = 2
				End If
			Else
				spacesToMove = 1
			End If
			Dim spaces As Integer = If((DicePalaceMainLevelGameInfo.PLAYER_SPACES_MOVED + spacesToMove <= 1), 0, (DicePalaceMainLevelGameInfo.PLAYER_SPACES_MOVED + spacesToMove - 1))
			If spaces < Me.maxSpaces AndAlso Me.allBoardSpaces(spaces) <> DicePalaceMainLevelGameManager.BoardSpaces.FreeSpace AndAlso Me.allBoardSpaces(spaces) <> DicePalaceMainLevelGameManager.BoardSpaces.StartOver AndAlso spaces + 1 < Me.boardSpacesObj.Length AndAlso Not Me.boardSpacesObj(spaces + 1).HasHeart Then
				AudioManager.[Stop]("vox_curious")
				AudioManager.Play("vox_laugh")
				Me.emitAudioFromObject.Add("vox_laugh")
			End If
			Yield MyBase.StartCoroutine(Me.MoveMarker(spacesToMove, False))
			DicePalaceMainLevelGameInfo.PLAYER_SPACES_MOVED += spacesToMove
			If DicePalaceMainLevelGameInfo.PLAYER_SPACES_MOVED > Me.maxSpaces Then
				DicePalaceMainLevelGameInfo.PLAYER_SPACES_MOVED = Me.maxSpaces
				playingGame = False
				Me.kingDiceAni.SetBool("IsSafe", True)
				Exit While
			End If
			Dim space As DicePalaceMainLevelGameManager.BoardSpaces = Me.allBoardSpaces(spaces)
			Me.kingDiceAni.SetTrigger("OnEager")
			If playingGame Then
				If space = DicePalaceMainLevelGameManager.BoardSpaces.FreeSpace OrElse DicePalaceMainLevelGameInfo.PLAYER_SPACES_MOVED = Me.previousSpace Then
					AudioManager.Play("vox_curious")
					Me.emitAudioFromObject.Add("vox_curious")
					Me.kingDiceAni.SetBool("IsSafe", True)
				ElseIf space = DicePalaceMainLevelGameManager.BoardSpaces.StartOver Then
					AudioManager.Play("vox_startover")
					Me.emitAudioFromObject.Add("vox_startover")
					AudioManager.[Stop]("vox_curious")
					DicePalaceMainLevelGameInfo.SAFE_INDEXES.Add(spaces)
					Me.boardSpacesObj(spaces + 1).Clear = True
					Me.CheckSafeSpaces()
					Yield MyBase.StartCoroutine(Me.MoveMarker(-Me.maxSpaces, True))
					Me.kingDiceAni.SetBool("IsSafe", True)
					DicePalaceMainLevelGameInfo.PLAYER_SPACES_MOVED = 0
					Yield CupheadTime.WaitForSeconds(Me, p.pauseWhenRolled)
				Else
					Me.previousSpace = DicePalaceMainLevelGameInfo.PLAYER_SPACES_MOVED
					Me.kingDiceAni.SetBool("IsSafe", False)
					DicePalaceMainLevelGameInfo.SAFE_INDEXES.Add(spaces)
					For i As Integer = 0 To DicePalaceMainLevelGameInfo.HEART_INDEXES.Length - 1
						If DicePalaceMainLevelGameInfo.HEART_INDEXES(i) = spaces Then
							If DicePalaceMainLevelGameInfo.PLAYER_ONE_STATS Is Nothing Then
								DicePalaceMainLevelGameInfo.PLAYER_ONE_STATS = New PlayersStatsBossesHub()
							End If
							Dim playerStats As PlayerStatsManager = PlayerManager.GetPlayer(PlayerId.PlayerOne).stats
							If playerStats.Health > 0 Then
								DicePalaceMainLevelGameInfo.PLAYER_ONE_STATS.BonusHP += 1
								playerStats.SetHealth(playerStats.Health + 1)
							End If
							If PlayerManager.Multiplayer Then
								If DicePalaceMainLevelGameInfo.PLAYER_TWO_STATS Is Nothing Then
									DicePalaceMainLevelGameInfo.PLAYER_TWO_STATS = New PlayersStatsBossesHub()
								End If
								Dim stats As PlayerStatsManager = PlayerManager.GetPlayer(PlayerId.PlayerTwo).stats
								If stats.Health > 0 Then
									DicePalaceMainLevelGameInfo.PLAYER_TWO_STATS.BonusHP += 1
									stats.SetHealth(stats.Health + 1)
								End If
							End If
							Me.boardSpacesObj(DicePalaceMainLevelGameInfo.HEART_INDEXES(i) + 1).HasHeart = False
							Me.heart.transform.position = Me.boardSpacesObj(DicePalaceMainLevelGameInfo.HEART_INDEXES(i) + 1).HeartSpacePosition
							Me.heart.SetActive(True)
							AudioManager.Play("pop_up")
							Me.emitAudioFromObject.Add("pop_up")
							Yield CupheadTime.WaitForSeconds(Me, 1.5F)
							Me.heart.SetActive(False)
							DicePalaceMainLevelGameInfo.HEART_INDEXES(i) = -1
							Exit For
						End If
					Next
					Yield MyBase.StartCoroutine(Me.start_mini_boss_cr(Me.SelectLevel(space)))
				End If
				Me.dice.waitingToRoll = True
				Yield Nothing
			End If
			Yield Nothing
		End While
		Me.EndBoardGame(Me.dice)
		Return
	End Function

	' Token: 0x06001D69 RID: 7529 RVA: 0x0010D470 File Offset: 0x0010B870
	Private Iterator Function MoveMarker(spacesToMove As Integer, resetBoard As Boolean) As IEnumerator
		Dim side As Integer = 1
		If spacesToMove < 0 Then
			side = -1
			spacesToMove *= -1
		End If
		For i As Integer = 0 To spacesToMove - 1
			Dim index As Integer = DicePalaceMainLevelGameInfo.PLAYER_SPACES_MOVED + (1 + i) * side
			If index < 0 OrElse index >= Me.boardSpacesObj.Length Then
				Exit For
			End If
			Dim t As Single = 0F
			Dim startPos As Vector3 = Me.marker.position
			Dim endPos As Vector3 = Me.boardSpacesObj(index).Pivot.position
			Dim startRot As Quaternion = Me.marker.rotation
			Dim endRot As Quaternion = Me.boardSpacesObj(index).Pivot.rotation
			If Not resetBoard Then
				Me.markerAnimator.SetTrigger("Move")
				Yield Me.markerAnimator.WaitForAnimationToStart(Me, "Move", False)
			End If
			Dim movement As Single = If((Not resetBoard), 0.3F, 0.083333336F)
			While t < movement
				Me.marker.position = Vector3.Lerp(startPos, endPos, t / movement)
				Me.marker.rotation = Quaternion.Lerp(startRot, endRot, t / movement)
				t += CupheadTime.Delta
				Yield Nothing
			End While
			Me.marker.position = endPos
			Me.marker.rotation = endRot
			AudioManager.Play("counter_move")
			Me.emitAudioFromObject.Add("counter_move")
			If Not resetBoard Then
				Me.markerAnimator.SetTrigger("Marker")
			End If
			Yield CupheadTime.WaitForSeconds(Me, 0.1F)
		Next
		Return
	End Function

	' Token: 0x06001D6A RID: 7530 RVA: 0x0010D49C File Offset: 0x0010B89C
	Private Function SelectLevel(space As DicePalaceMainLevelGameManager.BoardSpaces) As Levels
		Dim levels As Levels = Levels.DicePalaceMain
		Select Case space
			Case DicePalaceMainLevelGameManager.BoardSpaces.Booze
				levels = Levels.DicePalaceBooze
			Case DicePalaceMainLevelGameManager.BoardSpaces.Chips
				levels = Levels.DicePalaceChips
			Case DicePalaceMainLevelGameManager.BoardSpaces.Cigar
				levels = Levels.DicePalaceCigar
			Case DicePalaceMainLevelGameManager.BoardSpaces.Domino
				levels = Levels.DicePalaceDomino
			Case DicePalaceMainLevelGameManager.BoardSpaces.EightBall
				levels = Levels.DicePalaceEightBall
			Case DicePalaceMainLevelGameManager.BoardSpaces.FlyingHorse
				levels = Levels.DicePalaceFlyingHorse
			Case DicePalaceMainLevelGameManager.BoardSpaces.FlyingMemory
				levels = Levels.DicePalaceFlyingMemory
			Case DicePalaceMainLevelGameManager.BoardSpaces.Pachinko
				levels = Levels.DicePalacePachinko
			Case DicePalaceMainLevelGameManager.BoardSpaces.Rabbit
				levels = Levels.DicePalaceRabbit
			Case DicePalaceMainLevelGameManager.BoardSpaces.Roulette
				levels = Levels.DicePalaceRoulette
		End Select
		Return levels
	End Function

	' Token: 0x06001D6B RID: 7531 RVA: 0x0010D558 File Offset: 0x0010B958
	Private Iterator Function start_mini_boss_cr(level As Levels) As IEnumerator
		Me.kingDiceAni.SetTrigger("OnEat")
		DicePalaceMainLevelGameInfo.SetPlayersStats()
		Yield Me.kingDiceAni.WaitForAnimationToStart(Me, "Eat_Screen", False)
		AudioManager.Play("king_dice_eat_screen")
		Me.kingDice.GetComponent(Of SpriteRenderer)().sortingLayerName = SpriteLayer.Player.ToString()
		Me.kingDice.GetComponent(Of SpriteRenderer)().sortingOrder = 2000
		Level.ScoringData.time += Level.Current.LevelTime
		Yield Me.kingDiceAni.WaitForAnimationToEnd(Me, "Eat_Screen", False, True)
		SceneLoader.LoadLevel(level, SceneLoader.Transition.Fade, SceneLoader.Icon.Hourglass, Nothing)
		Yield Nothing
		Return
	End Function

	' Token: 0x06001D6C RID: 7532 RVA: 0x0010D57C File Offset: 0x0010B97C
	Private Sub EndBoardGame(dice1 As DicePalaceMainLevelDice)
		Me.StopAllCoroutines()
		MyBase.StartCoroutine(Me.flashEnd_cr())
		MyBase.StartCoroutine(Me.announcerSfx_cr())
		Me.kingDice.StartKingDiceBattle()
		If dice1 IsNot Nothing Then
			Global.UnityEngine.[Object].Destroy(dice1.gameObject)
		End If
		DicePalaceMainLevelGameInfo.CleanUpRetry()
	End Sub

	' Token: 0x06001D6D RID: 7533 RVA: 0x0010D5D0 File Offset: 0x0010B9D0
	Private Iterator Function flashEnd_cr() As IEnumerator
		Dim endSpace As DicePalaceMainLevelBoardSpace = Me.boardSpacesObj(Me.boardSpacesObj.Length - 1)
		While True
			endSpace.Clear = True
			Yield CupheadTime.WaitForSeconds(Me, Me.endSpaceFlashRate)
			endSpace.Clear = False
			Yield CupheadTime.WaitForSeconds(Me, Me.endSpaceFlashRate)
		End While
		Return
	End Function

	' Token: 0x06001D6E RID: 7534 RVA: 0x0010D5EC File Offset: 0x0010B9EC
	Private Iterator Function announcerSfx_cr() As IEnumerator
		AudioManager.Play("level_announcer_ready")
		AudioManager.Play("level_bell_intro")
		Yield CupheadTime.WaitForSeconds(Me, 2F)
		AudioManager.Play("level_announcer_begin")
		Return
	End Function

	' Token: 0x04002644 RID: 9796
	Private Const MarkerMovementTime As Single = 0.3F

	' Token: 0x04002645 RID: 9797
	Private Const MarkerFastMove As Single = 0.083333336F

	' Token: 0x04002646 RID: 9798
	<SerializeField()>
	Private allBoardSpaces As DicePalaceMainLevelGameManager.BoardSpaces()

	' Token: 0x04002647 RID: 9799
	<SerializeField()>
	Private boardSpacesObj As DicePalaceMainLevelBoardSpace()

	' Token: 0x04002648 RID: 9800
	<SerializeField()>
	Private startSpaceObj As DicePalaceMainLevelBoardSpace

	' Token: 0x04002649 RID: 9801
	<SerializeField()>
	Private endSpaceObj As DicePalaceMainLevelBoardSpace

	' Token: 0x0400264A RID: 9802
	<SerializeField()>
	Private kingDice As DicePalaceMainLevelKingDice

	' Token: 0x0400264B RID: 9803
	<SerializeField()>
	Private dicePrefab As DicePalaceMainLevelDice

	' Token: 0x0400264C RID: 9804
	<SerializeField()>
	Private pivotPoint1 As Transform

	' Token: 0x0400264D RID: 9805
	<SerializeField()>
	Private marker As Transform

	' Token: 0x0400264E RID: 9806
	<SerializeField()>
	Private markerAnimator As Animator

	' Token: 0x0400264F RID: 9807
	<SerializeField()>
	Private endSpaceFlashRate As Single

	' Token: 0x04002650 RID: 9808
	<SerializeField()>
	Private heart As GameObject

	' Token: 0x04002651 RID: 9809
	Private kingDiceAni As Animator

	' Token: 0x04002652 RID: 9810
	Private dice As DicePalaceMainLevelDice

	' Token: 0x04002653 RID: 9811
	Private gameInfo As DicePalaceMainLevelGameInfo

	' Token: 0x04002654 RID: 9812
	Private previousSpace As Integer

	' Token: 0x04002655 RID: 9813
	Private maxSpaces As Integer

	' Token: 0x020005D6 RID: 1494
	Public Enum BoardSpaces
		' Token: 0x04002657 RID: 9815
		Booze
		' Token: 0x04002658 RID: 9816
		Chips
		' Token: 0x04002659 RID: 9817
		Cigar
		' Token: 0x0400265A RID: 9818
		Domino
		' Token: 0x0400265B RID: 9819
		EightBall
		' Token: 0x0400265C RID: 9820
		FlyingHorse
		' Token: 0x0400265D RID: 9821
		FlyingMemory
		' Token: 0x0400265E RID: 9822
		Pachinko
		' Token: 0x0400265F RID: 9823
		Rabbit
		' Token: 0x04002660 RID: 9824
		Roulette
		' Token: 0x04002661 RID: 9825
		FreeSpace
		' Token: 0x04002662 RID: 9826
		StartOver
	End Enum
End Class
