Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports UnityEngine

' Token: 0x020005CA RID: 1482
Public Class DicePalaceFlyingMemoryLevelGameManager
	Inherits LevelProperties.DicePalaceFlyingMemory.Entity

	' Token: 0x17000365 RID: 869
	' (get) Token: 0x06001CF9 RID: 7417 RVA: 0x0010974C File Offset: 0x00107B4C
	Public Shared ReadOnly Property Instance As DicePalaceFlyingMemoryLevelGameManager
		Get
			If DicePalaceFlyingMemoryLevelGameManager.singletonGameManager Is Nothing Then
				DicePalaceFlyingMemoryLevelGameManager.singletonGameManager = New GameObject() With { .name = "GameManager" }.AddComponent(Of DicePalaceFlyingMemoryLevelGameManager)()
			End If
			Return DicePalaceFlyingMemoryLevelGameManager.singletonGameManager
		End Get
	End Property

	' Token: 0x06001CFA RID: 7418 RVA: 0x0010978A File Offset: 0x00107B8A
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.hiddenPosition = MyBase.transform.position
		DicePalaceFlyingMemoryLevelGameManager.singletonGameManager = Me
	End Sub

	' Token: 0x06001CFB RID: 7419 RVA: 0x001097AC File Offset: 0x00107BAC
	Public Overrides Sub LevelInit(properties As LevelProperties.DicePalaceFlyingMemory)
		MyBase.LevelInit(properties)
		Me.patternOrder = properties.CurrentState.flippyCard.patternOrder.GetRandom().Split(New Char() { ","c })
		Me.maxHP = properties.CurrentHealth
		Me.contactDimX = Me.GridDimX + 1
		Me.contactDimY = Me.GridDimY + 1
		Me.GenerateGrid()
	End Sub

	' Token: 0x06001CFC RID: 7420 RVA: 0x00109819 File Offset: 0x00107C19
	Private Sub Update()
		If Me.checkForFlipped Then
			Me.CheckIfFlipped()
		End If
	End Sub

	' Token: 0x06001CFD RID: 7421 RVA: 0x0010982C File Offset: 0x00107C2C
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me.contactPointPrefab = Nothing
		Me.cardPrefab = Nothing
		Me.botPrefab = Nothing
		DicePalaceFlyingMemoryLevelGameManager.singletonGameManager = Nothing
	End Sub

	' Token: 0x06001CFE RID: 7422 RVA: 0x00109850 File Offset: 0x00107C50
	Private Sub GenerateGrid()
		Dim x As Single = Me.cardPrefab.GetComponent(Of Renderer)().bounds.size.x
		Dim y As Single = Me.cardPrefab.GetComponent(Of Renderer)().bounds.size.y
		Dim num As Single = x + 10F
		Dim num2 As Single = y + 10F
		Me.cards = New DicePalaceFlyingMemoryLevelCard(Me.GridDimX - 1, Me.GridDimY - 1) {}
		Me.contactPoints = New DicePalaceFlyingMemoryLevelContactPoint(Me.contactDimX - 1, Me.contactDimY - 1) {}
		For i As Integer = 0 To Me.GridDimY - 1
			For j As Integer = 0 To Me.GridDimX - 1
				Dim vector As Vector3 = New Vector3(CSng(j) * num, CSng((-CSng(i))) * num2)
				Me.cards(j, i) = Global.UnityEngine.[Object].Instantiate(Of DicePalaceFlyingMemoryLevelCard)(Me.cardPrefab)
				Me.cards(j, i).transform.position = vector + MyBase.transform.position
				Me.cards(j, i).transform.parent = MyBase.transform
				Me.AssignCards(j, i)
			Next
		Next
		For k As Integer = 0 To Me.contactDimY - 1
			For l As Integer = 0 To Me.contactDimX - 1
				Dim vector2 As Vector3 = New Vector3(CSng(l) * num - num / 2F, CSng((-CSng(k))) * num2 + num2 / 2F)
				Me.contactPoints(l, k) = Global.UnityEngine.[Object].Instantiate(Of DicePalaceFlyingMemoryLevelContactPoint)(Me.contactPointPrefab)
				Me.contactPoints(l, k).transform.position = vector2 + MyBase.transform.position
				Me.contactPoints(l, k).transform.parent = MyBase.transform
				Me.contactPoints(l, k).Xcoord = l
				Me.contactPoints(l, k).Ycoord = k
			Next
		Next
		MyBase.StartCoroutine(Me.start_game_cr())
	End Sub

	' Token: 0x06001CFF RID: 7423 RVA: 0x00109A94 File Offset: 0x00107E94
	Private Sub AssignCards(x As Integer, y As Integer)
		Dim card As DicePalaceFlyingMemoryLevelCard.Card = DicePalaceFlyingMemoryLevelCard.Card.Flowers
		If Me.patternOrder(Me.patternOrderIndex) = "1A" Then
			card = DicePalaceFlyingMemoryLevelCard.Card.Cuphead
		ElseIf Me.patternOrder(Me.patternOrderIndex) = "1B" Then
			card = DicePalaceFlyingMemoryLevelCard.Card.Chips
		ElseIf Me.patternOrder(Me.patternOrderIndex) = "2A" Then
			card = DicePalaceFlyingMemoryLevelCard.Card.Flowers
		ElseIf Me.patternOrder(Me.patternOrderIndex) = "2B" Then
			card = DicePalaceFlyingMemoryLevelCard.Card.Shield
		ElseIf Me.patternOrder(Me.patternOrderIndex) = "3A" Then
			card = DicePalaceFlyingMemoryLevelCard.Card.Spindle
		ElseIf Me.patternOrder(Me.patternOrderIndex) = "3B" Then
			card = DicePalaceFlyingMemoryLevelCard.Card.Mugman
		End If
		Dim num As Integer = Global.UnityEngine.Random.Range(0, Me.chosenFlippedDownCards.Count)
		Me.cards(x, y).card = card
		Me.cards(x, y).GetComponent(Of SpriteRenderer)().sprite = Me.chosenFlippedDownCards(num)
		Me.chosenFlippedDownCards.Remove(Me.chosenFlippedDownCards(num))
		Me.patternOrderIndex = (Me.patternOrderIndex + 1) Mod Me.patternOrder.Length
	End Sub

	' Token: 0x06001D00 RID: 7424 RVA: 0x00109BEC File Offset: 0x00107FEC
	Private Iterator Function start_game_cr() As IEnumerator
		For i As Integer = 0 To Me.GridDimY - 1
			For j As Integer = 0 To Me.GridDimX - 1
				Me.cards(j, i).FlipUp()
			Next
		Next
		Dim t As Single = 0F
		Dim time As Single = 1.3F
		Dim start As Vector2 = MyBase.transform.position
		While t < time
			Dim val As Single = EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, 0F, 1F, t / time)
			MyBase.transform.position = Vector2.Lerp(start, Me.cardStopRoot.position, val)
			t += CupheadTime.Delta
			Yield Nothing
		End While
		MyBase.transform.position = Me.cardStopRoot.position
		Yield CupheadTime.WaitForSeconds(Me, MyBase.properties.CurrentState.flippyCard.initialRevealTime)
		For k As Integer = 0 To Me.GridDimY - 1
			For l As Integer = 0 To Me.GridDimX - 1
				Me.cards(l, k).EnableCards()
			Next
		Next
		Me.checkForFlipped = True
		If MyBase.properties.CurrentState.bots.botsOn Then
			MyBase.StartCoroutine(Me.spawning_bots_cr())
		End If
		Yield Nothing
		Return
	End Function

	' Token: 0x06001D01 RID: 7425 RVA: 0x00109C08 File Offset: 0x00108008
	Private Iterator Function slide_cr(endPosition As Vector3) As IEnumerator
		Dim t As Single = 0F
		Dim time As Single = 1.3F
		Dim start As Vector2 = MyBase.transform.position
		While t < time
			Dim val As Single = EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, 0F, 1F, t / time)
			MyBase.transform.position = Vector2.Lerp(start, endPosition, val)
			t += CupheadTime.Delta
			Yield Nothing
		End While
		MyBase.transform.position = endPosition
		Return
	End Function

	' Token: 0x06001D02 RID: 7426 RVA: 0x00109C2C File Offset: 0x0010802C
	Private Sub CheckIfFlipped()
		Dim num As Integer = 0
		Dim num2 As Integer = 0
		Dim num3 As Integer = 0
		For i As Integer = 0 To Me.GridDimY - 1
			For j As Integer = 0 To Me.GridDimX - 1
				If Me.cards(j, i).flippedUp AndAlso Not Me.cards(j, i).permanentlyFlipped Then
					num += 1
					Me.cards(j, i).GetComponent(Of Collider2D)().enabled = False
					If num >= 2 Then
						Me.checkForFlipped = False
						If Me.cards(num2, num3).card = Me.cards(j, i).card Then
							Me.matchMade = True
							Me.cards(j, i).permanentlyFlipped = True
							Me.cards(num2, num3).permanentlyFlipped = True
						Else
							Me.matchMade = False
						End If
						MyBase.StartCoroutine(Me.disable_all_cards_cr())
						num = 0
					Else
						num2 = j
						num3 = i
					End If
				End If
			Next
		Next
	End Sub

	' Token: 0x06001D03 RID: 7427 RVA: 0x00109D4C File Offset: 0x0010814C
	Private Iterator Function disable_all_cards_cr() As IEnumerator
		For i As Integer = 0 To Me.GridDimY - 1
			For j As Integer = 0 To Me.GridDimX - 1
				If Not Me.cards(j, i).permanentlyFlipped Then
					Me.cards(j, i).DisableCard()
				Else
					Me.cards(j, i).GetComponent(Of Collider2D)().enabled = False
				End If
			Next
		Next
		Yield CupheadTime.WaitForSeconds(Me, 0.5F)
		MyBase.StartCoroutine(Me.open_timer_cr())
		Yield Nothing
		Return
	End Function

	' Token: 0x06001D04 RID: 7428 RVA: 0x00109D68 File Offset: 0x00108168
	Private Iterator Function open_timer_cr() As IEnumerator
		Dim HPToLower As Single = Me.maxHP / CSng((Me.GridDimX * Me.GridDimY / 2))
		If Me.matchMade Then
			MyBase.StartCoroutine(Me.slide_cr(Me.hiddenPosition))
			Me.matchCounter += 1
			While Me.stuffedToy.currentlyColliding
				Yield Nothing
			End While
			Me.stuffedToy.Open()
			If Me.matchCounter = Me.GridDimX * Me.GridDimY / 2 Then
				Return
			End If
			While MyBase.properties.CurrentHealth >= Me.maxHP - HPToLower * CSng(Me.matchCounter)
				Yield Nothing
			End While
		Else
			Me.stuffedToy.guessedWrong = True
			Yield CupheadTime.WaitForSeconds(Me, 1F)
		End If
		For i As Integer = 0 To Me.GridDimY - 1
			For j As Integer = 0 To Me.GridDimX - 1
				If Not Me.cards(j, i).permanentlyFlipped Then
					Me.cards(j, i).EnableCards()
				End If
			Next
		Next
		MyBase.StartCoroutine(Me.slide_cr(Me.cardStopRoot.position))
		Me.stuffedToy.Closed()
		Me.checkForFlipped = True
		Yield Nothing
		Return
	End Function

	' Token: 0x06001D05 RID: 7429 RVA: 0x00109D84 File Offset: 0x00108184
	Private Sub SpawnBot(xCoord As Integer, yCoord As Integer, moveOnY As Boolean)
		Dim [next] As AbstractPlayerController = PlayerManager.GetNext()
		Dim dicePalaceFlyingMemoryLevelBot As DicePalaceFlyingMemoryLevelBot = Global.UnityEngine.[Object].Instantiate(Of DicePalaceFlyingMemoryLevelBot)(Me.botPrefab)
		dicePalaceFlyingMemoryLevelBot.Init(MyBase.properties.CurrentState.bots, Me.contactPoints(xCoord, yCoord), moveOnY, MyBase.properties.CurrentState.bots.botsHP, [next])
	End Sub

	' Token: 0x06001D06 RID: 7430 RVA: 0x00109DE0 File Offset: 0x001081E0
	Private Iterator Function spawning_bots_cr() As IEnumerator
		Dim p As LevelProperties.DicePalaceFlyingMemory.Bots = MyBase.properties.CurrentState.bots
		Dim spawnPattern As String() = p.spawnOrder.GetRandom().Split(New Char() { ","c })
		Dim number As Integer = 0
		Dim Xcoord As Integer = 0
		Dim Ycoord As Integer = 0
		Dim Yset As Boolean = False
		While True
			For i As Integer = 0 To spawnPattern.Length - 1
				Dim spawnLocation As String() = spawnPattern(i).Split(New Char() { ":"c })
				For Each text As String In spawnLocation
					If text(0) = "U"c Then
						Ycoord = 0
						Yset = True
					ElseIf text(0) = "D"c Then
						Ycoord = Me.contactDimY - 1
						Yset = True
					ElseIf text(0) = "L"c Then
						Xcoord = 0
						Yset = False
					ElseIf text(0) = "R"c Then
						Xcoord = Me.contactDimX - 1
						Yset = False
					Else
						Parser.IntTryParse(text, number)
					End If
				Next
				If Yset Then
					Xcoord = number
				Else
					Ycoord = number
				End If
				Me.SpawnBot(Xcoord, Ycoord, Yset)
				Yield CupheadTime.WaitForSeconds(Me, p.spawnDelay)
			Next
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x040025EB RID: 9707
	Private Shared singletonGameManager As DicePalaceFlyingMemoryLevelGameManager

	' Token: 0x040025EC RID: 9708
	Public contactPoints As DicePalaceFlyingMemoryLevelContactPoint(,)

	' Token: 0x040025ED RID: 9709
	Public contactDimX As Integer

	' Token: 0x040025EE RID: 9710
	Public contactDimY As Integer

	' Token: 0x040025EF RID: 9711
	<SerializeField()>
	Private cardStopRoot As Transform

	' Token: 0x040025F0 RID: 9712
	<SerializeField()>
	Private chosenFlippedDownCards As List(Of Sprite)

	' Token: 0x040025F1 RID: 9713
	<SerializeField()>
	Private contactPointPrefab As DicePalaceFlyingMemoryLevelContactPoint

	' Token: 0x040025F2 RID: 9714
	<SerializeField()>
	Private stuffedToy As DicePalaceFlyingMemoryLevelStuffedToy

	' Token: 0x040025F3 RID: 9715
	<SerializeField()>
	Private cardPrefab As DicePalaceFlyingMemoryLevelCard

	' Token: 0x040025F4 RID: 9716
	<SerializeField()>
	Private botPrefab As DicePalaceFlyingMemoryLevelBot

	' Token: 0x040025F5 RID: 9717
	Private cards As DicePalaceFlyingMemoryLevelCard(,)

	' Token: 0x040025F6 RID: 9718
	Private hiddenPosition As Vector3

	' Token: 0x040025F7 RID: 9719
	Private GridDimX As Integer = 4

	' Token: 0x040025F8 RID: 9720
	Private GridDimY As Integer = 3

	' Token: 0x040025F9 RID: 9721
	Private patternOrderIndex As Integer

	' Token: 0x040025FA RID: 9722
	Private matchCounter As Integer

	' Token: 0x040025FB RID: 9723
	Private maxHP As Single

	' Token: 0x040025FC RID: 9724
	Private space As Single

	' Token: 0x040025FD RID: 9725
	Private checkForFlipped As Boolean

	' Token: 0x040025FE RID: 9726
	Private matchMade As Boolean

	' Token: 0x040025FF RID: 9727
	Private patternOrder As String()
End Class
