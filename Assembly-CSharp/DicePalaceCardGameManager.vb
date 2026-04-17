Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports UnityEngine

' Token: 0x020005A4 RID: 1444
Public Class DicePalaceCardGameManager
	Inherits AbstractPausableComponent

	' Token: 0x06001BCF RID: 7119 RVA: 0x000FD78C File Offset: 0x000FBB8C
	Public Sub GameSetup(cardProperties As LevelProperties.DicePalaceCard)
		Me.properties = cardProperties.CurrentState.blocks
		Me.GridDimX = Me.properties.gridWidth
		Me.GridDimY = Me.properties.gridHeight
		Me.SetSize()
		Me.typePattern = Me.properties.cardTypeString.GetRandom().Split(New Char() { ","c })
		Me.amountPattern = Me.properties.cardAmountString.GetRandom().Split(New Char() { ","c })
		Dim position As Vector3 = MyBase.transform.position
		position.y = 360F - Me.gridBlockPrefab.GetComponent(Of Renderer)().bounds.size.y
		position.x = -640F + Me.gridBlockPrefab.GetComponent(Of Renderer)().bounds.size.x
		MyBase.transform.position = position
		Me.selectedPrefab = New DicePalaceCardLevelBlock()
		Me.totalColumns = New List(Of DicePalaceCardLevelColumn)()
		Me.typeIndex = Global.UnityEngine.Random.Range(0, Me.typePattern.Length)
		Me.amountIndex = Global.UnityEngine.Random.Range(0, Me.amountPattern.Length)
		Me.GenerateGrid()
		Me.startingPos = MyBase.transform.position.y
	End Sub

	' Token: 0x06001BD0 RID: 7120 RVA: 0x000FD8F0 File Offset: 0x000FBCF0
	Public Iterator Function start_game_cr() As IEnumerator
		While True
			Me.SpawnColumn()
			Yield CupheadTime.WaitForSeconds(Me, Me.properties.attackDelayRange)
		End While
		Return
	End Function

	' Token: 0x06001BD1 RID: 7121 RVA: 0x000FD90C File Offset: 0x000FBD0C
	Private Sub SetSize()
		Me.hearts.transform.SetScale(New Single?(Me.properties.blockSize), New Single?(Me.properties.blockSize), New Single?(Me.properties.blockSize))
		Me.spades.transform.SetScale(New Single?(Me.properties.blockSize), New Single?(Me.properties.blockSize), New Single?(Me.properties.blockSize))
		Me.clubs.transform.SetScale(New Single?(Me.properties.blockSize), New Single?(Me.properties.blockSize), New Single?(Me.properties.blockSize))
		Me.diamonds.transform.SetScale(New Single?(Me.properties.blockSize), New Single?(Me.properties.blockSize), New Single?(Me.properties.blockSize))
		Me.gridBlockPrefab.transform.SetScale(New Single?(Me.properties.blockSize), New Single?(Me.properties.blockSize), New Single?(Me.properties.blockSize))
		Me.GridSpacing = Me.properties.blockSize
	End Sub

	' Token: 0x06001BD2 RID: 7122 RVA: 0x000FDA6C File Offset: 0x000FBE6C
	Private Sub SpawnColumn()
		Dim num As Integer = 1
		Dim num2 As Integer = -1
		Dim num3 As Single = Me.gridBlockPrefab.GetComponent(Of Renderer)().bounds.size.y / 2F
		Dim num4 As Single = 0F
		Parser.FloatTryParse(Me.amountPattern(Me.amountIndex), num4)
		Dim dicePalaceCardLevelColumn As DicePalaceCardLevelColumn = Global.UnityEngine.[Object].Instantiate(Of DicePalaceCardLevelColumn)(Me.columnObject)
		Me.totalColumns.Add(dicePalaceCardLevelColumn)
		Dim num5 As Integer = Me.totalColumns.Count - 1
		Dim position As Vector3 = Me.totalColumns(num5).transform.position
		position.x = 640F
		position.y = 360F - num3
		Me.totalColumns(num5).transform.position = position
		Dim num6 As Integer = 0
		While CSng(num6) < num4
			If Me.typePattern(Me.typeIndex)(0) = "H"c Then
				Me.selectedPrefab = Me.hearts
			ElseIf Me.typePattern(Me.typeIndex)(0) = "S"c Then
				Me.selectedPrefab = Me.spades
			ElseIf Me.typePattern(Me.typeIndex)(0) = "D"c Then
				Me.selectedPrefab = Me.diamonds
			ElseIf Me.typePattern(Me.typeIndex)(0) = "C"c Then
				Me.selectedPrefab = Me.clubs
			End If
			Me.typeIndex = (Me.typeIndex + 1) Mod Me.typePattern.Length
			Dim dicePalaceCardLevelBlock As DicePalaceCardLevelBlock = Global.UnityEngine.[Object].Instantiate(Of DicePalaceCardLevelBlock)(Me.selectedPrefab)
			Me.totalColumns(num5).blockPieces.Add(dicePalaceCardLevelBlock)
			dicePalaceCardLevelBlock.transform.parent = Me.totalColumns(num5).transform
			Dim position2 As Vector3 = Me.totalColumns(num5).blockPieces(num6).transform.position
			If num6 Mod 2 = 0 AndAlso num6 <> 0 Then
				Me.totalColumns(num5).blockPieces(num6).stopOffsetX = num2
				position2.x = 640F - Me.totalColumns(num5).blockPieces(num6).GetComponent(Of Renderer)().bounds.size.x * CSng(Mathf.Abs(Me.totalColumns(num5).blockPieces(num6).stopOffsetX))
				num2 -= 1
			ElseIf num6 Mod 2 = 1 AndAlso num6 <> 0 Then
				Me.totalColumns(num5).blockPieces(num6).stopOffsetX = num
				position2.x = 640F + Me.totalColumns(num5).blockPieces(num6).GetComponent(Of Renderer)().bounds.size.x * CSng(Mathf.Abs(Me.totalColumns(num5).blockPieces(num6).stopOffsetX))
				num += 1
			Else
				position2.x = 640F
			End If
			position2.y = 360F - num3
			Me.totalColumns(num5).blockPieces(num6).transform.position = position2
			num6 += 1
		End While
		Me.amountIndex = (Me.amountIndex + 1) Mod Me.amountPattern.Length
		MyBase.StartCoroutine(Me.horizontal_moving_column(Me.totalColumns(num5)))
	End Sub

	' Token: 0x06001BD3 RID: 7123 RVA: 0x000FDE24 File Offset: 0x000FC224
	Private Iterator Function horizontal_moving_column(currentColumn As DicePalaceCardLevelColumn) As IEnumerator
		Dim player As AbstractPlayerController = PlayerManager.GetNext()
		Dim offset As Single = Me.gridBlocks(1, 0).transform.position.x - Me.gridBlocks(0, 0).transform.position.x
		Dim playerXPos As Integer = 0
		Dim stopXPos As Integer = 0
		Dim selectStop As Boolean = False
		Dim dist As Single = 0F
		Dim distOffset As Single = 20F
		While currentColumn.transform.position.x <> Me.gridBlocks(stopXPos, 0).transform.position.x
			If player Is Nothing OrElse player.IsDead Then
				player = PlayerManager.GetNext()
			End If
			For i As Integer = 0 To Me.GridDimX - 1 - 1
				If player.transform.position.x > Me.gridBlocks(i, 0).transform.position.x - offset / 2F Then
					If player.transform.position.x < Me.gridBlocks(i + 1, 0).transform.position.x - offset / 2F Then
						playerXPos = i
					ElseIf i + 1 = Me.GridDimX - 1 Then
						playerXPos = i + 1
					End If
				End If
			Next
			dist = Me.gridBlocks(playerXPos, 0).transform.position.x - currentColumn.transform.position.x
			Dim pos As Vector3 = currentColumn.transform.position
			If dist < distOffset Then
				selectStop = True
			End If
			Dim overFlow As Integer = Me.GridDimX - playerXPos - currentColumn.blockPieces.Count
			If selectStop Then
				If Me.gridBlocks(1, 0).transform.position.x > player.transform.position.x AndAlso currentColumn.blockPieces.Count >= 1 Then
					Dim num As Integer
					If currentColumn.blockPieces.Count Mod 2 = 1 Then
						num = currentColumn.blockPieces(currentColumn.blockPieces.Count - 1).stopOffsetX - 1
					Else
						num = currentColumn.blockPieces(currentColumn.blockPieces.Count - 1).stopOffsetX
					End If
					stopXPos = Mathf.Abs(num) - 1
					selectStop = False
				ElseIf Me.gridBlocks(Me.GridDimX - 1, 0).transform.position.x < player.transform.position.x OrElse Mathf.Sign(CSng(overFlow)) = -1F Then
					stopXPos = Me.GridDimX - 1 - Mathf.Abs(currentColumn.blockPieces(currentColumn.blockPieces.Count - 1).stopOffsetX)
					selectStop = False
				Else
					stopXPos = playerXPos
					selectStop = False
				End If
			End If
			pos.x = Mathf.MoveTowards(currentColumn.transform.position.x, Me.gridBlocks(stopXPos, Me.GridDimY - 1).transform.position.x, Me.properties.blockSpeed * CupheadTime.Delta)
			currentColumn.transform.position = pos
			Yield Nothing
		End While
		MyBase.StartCoroutine(Me.vertical_moving_column(currentColumn, stopXPos))
		Yield Nothing
		Return
	End Function

	' Token: 0x06001BD4 RID: 7124 RVA: 0x000FDE48 File Offset: 0x000FC248
	Private Iterator Function vertical_moving_column(currentColumn As DicePalaceCardLevelColumn, stopXPos As Integer) As IEnumerator
		currentColumn.blockCounter = 0
		currentColumn.blockXPos = New Integer(currentColumn.blockPieces.Count - 1) {}
		currentColumn.columnStopYPos = New Integer(currentColumn.blockPieces.Count - 1) {}
		For i As Integer = 0 To currentColumn.blockPieces.Count - 1
			currentColumn.blockXPos(i) = stopXPos + currentColumn.blockPieces(i).stopOffsetX
			For j As Integer = Me.GridDimY - 1 To 0 Step -1
				If Not Me.gridBlocks(currentColumn.blockXPos(i), j).hasBlock Then
					If j > 0 Then
						If Me.gridBlocks(currentColumn.blockXPos(i), j - 1).hasBlock Then
							currentColumn.columnStopYPos(i) = j
							Me.gridBlocks(currentColumn.blockXPos(i), j).hasBlock = True
						End If
					Else
						currentColumn.columnStopYPos(i) = 0
						Me.gridBlocks(currentColumn.blockXPos(i), 0).hasBlock = True
					End If
				End If
			Next
			MyBase.StartCoroutine(Me.drop_block_cr(currentColumn, currentColumn.columnStopYPos(i), i))
		Next
		While currentColumn.blockCounter < currentColumn.blockPieces.Count
			Yield Nothing
		End While
		currentColumn.blockPieces.Clear()
		currentColumn.transform.DetachChildren()
		Global.UnityEngine.[Object].Destroy(currentColumn.gameObject)
		Me.doneDropping = False
		Me.checkAgain = False
		MyBase.StartCoroutine(Me.check_to_drop_blocks())
		While Not Me.doneDropping
			Yield Nothing
		End While
		Me.CheckFullGrid()
		Me.ScaleCheck()
		Me.CheckForTop()
		MyBase.StartCoroutine(Me.check_to_drop_blocks())
		Me.CheckFullGrid()
		Me.ScaleCheck()
		Return
	End Function

	' Token: 0x06001BD5 RID: 7125 RVA: 0x000FDE74 File Offset: 0x000FC274
	Private Iterator Function drop_block_cr(currentColumn As DicePalaceCardLevelColumn, indexToDropTo As Integer, blockToDrop As Integer) As IEnumerator
		While currentColumn.blockPieces(blockToDrop).transform.position.y > Me.gridBlocks(currentColumn.blockXPos(blockToDrop), indexToDropTo).transform.position.y
			Dim pos As Vector3 = currentColumn.blockPieces(blockToDrop).transform.position
			pos.y = Mathf.MoveTowards(currentColumn.blockPieces(blockToDrop).transform.position.y, Me.gridBlocks(currentColumn.blockXPos(blockToDrop), indexToDropTo).transform.position.y, Me.properties.blockDropSpeed * CupheadTime.Delta)
			currentColumn.blockPieces(blockToDrop).transform.position = pos
			Yield Nothing
		End While
		currentColumn.blockPieces(blockToDrop).transform.parent = MyBase.transform
		Me.gridBlocks(currentColumn.blockXPos(blockToDrop), indexToDropTo).blockHeld = currentColumn.blockPieces(blockToDrop)
		currentColumn.blockCounter += 1
		Return
	End Function

	' Token: 0x06001BD6 RID: 7126 RVA: 0x000FDEA4 File Offset: 0x000FC2A4
	Private Iterator Function check_to_drop_blocks() As IEnumerator
		For i As Integer = 0 To Me.GridDimX - 1
			For j As Integer = Me.GridDimY - 1 To 0 Step -1
				If Me.gridBlocks(i, j).hasBlock AndAlso Me.gridBlocks(i, j).Ycoordinate > 0F Then
					Dim num As Integer = j - 1
					Dim num2 As Integer = j + 1
					Dim blockHeld As DicePalaceCardLevelBlock = Me.gridBlocks(i, j).blockHeld
					If Not Me.gridBlocks(i, num).hasBlock Then
						Me.checkAgain = True
						Me.CheckFullGrid()
						MyBase.StartCoroutine(Me.drop_current_cr(i, j, num, blockHeld))
						If Me.gridBlocks(i, num2).hasBlock AndAlso Me.gridBlocks(i, num2).Ycoordinate < CSng(Me.GridDimY) Then
							MyBase.StartCoroutine(Me.drop_current_cr(i, num2, j, Me.gridBlocks(i, num2).blockHeld))
							num2 += 1
						End If
					Else
						Me.checkAgain = False
					End If
				End If
			Next
		Next
		If Not Me.checkAgain Then
			Me.doneDropping = True
		End If
		Yield Nothing
		Return
	End Function

	' Token: 0x06001BD7 RID: 7127 RVA: 0x000FDEC0 File Offset: 0x000FC2C0
	Private Iterator Function drop_current_cr(x As Integer, y As Integer, spaceBelow As Integer, block As DicePalaceCardLevelBlock) As IEnumerator
		If Me.gridBlocks(x, y).blockHeld IsNot Nothing AndAlso Not Me.gridBlocks(x, spaceBelow).hasBlock Then
			If y >= 0 AndAlso Me.gridBlocks(x, y).hasBlock Then
				Me.gridBlocks(x, y).hasBlock = False
				Me.gridBlocks(x, spaceBelow).hasBlock = True
			End If
			While Me.gridBlocks(x, y).blockHeld.transform.position.y <> Me.gridBlocks(x, spaceBelow).transform.position.y
				Dim pos As Vector3 = Me.gridBlocks(x, y).blockHeld.transform.position
				pos.y = Mathf.MoveTowards(Me.gridBlocks(x, y).blockHeld.transform.position.y, Me.gridBlocks(x, spaceBelow).transform.position.y, Me.properties.blockDropSpeed * CupheadTime.Delta)
				Me.gridBlocks(x, y).blockHeld.transform.position = pos
				Yield Nothing
			End While
			Me.gridBlocks(x, y).blockHeld = Nothing
			Me.gridBlocks(x, spaceBelow).blockHeld = block
			Me.CheckFullGrid()
			MyBase.StartCoroutine(Me.check_to_drop_blocks())
			Me.ScaleCheck()
		End If
		Return
	End Function

	' Token: 0x06001BD8 RID: 7128 RVA: 0x000FDEF8 File Offset: 0x000FC2F8
	Public Sub GenerateGrid()
		Me.gridBlocks = New DicePalaceCardLevelGridBlock(Me.GridDimX - 1, Me.GridDimY - 1) {}
		For i As Integer = 0 To Me.GridDimX - 1
			For j As Integer = 0 To Me.GridDimY - 1
				Dim vector As Vector3 = New Vector3(CSng(i) * Me.GridSpacing, CSng(j) * Me.GridSpacing)
				Me.gridBlocks(i, j) = Global.UnityEngine.[Object].Instantiate(Of DicePalaceCardLevelGridBlock)(Me.gridBlockPrefab)
				Me.gridBlocks(i, j).transform.position = vector + MyBase.transform.position
				Me.gridBlocks(i, j).transform.parent = MyBase.transform
				Me.gridBlocks(i, j).Xcoordinate = CSng(i)
				Me.gridBlocks(i, j).Ycoordinate = CSng(j)
			Next
		Next
	End Sub

	' Token: 0x06001BD9 RID: 7129 RVA: 0x000FDFE8 File Offset: 0x000FC3E8
	Private Sub CheckFullGrid()
		For i As Integer = 0 To Me.GridDimX - 1
			For j As Integer = 0 To Me.GridDimY - 1
				If Me.gridBlocks(i, j).blockHeld IsNot Nothing Then
					If Me.gridBlocks(i, j).Xcoordinate < CSng((Me.GridDimX - 2)) AndAlso Me.gridBlocks(i, j).Ycoordinate < CSng((Me.GridDimY - 2)) AndAlso Me.gridBlocks(i, j).hasBlock Then
						Me.DiagonalsUpCheck(i, j, Me.gridBlocks(i, j).blockHeld.suit)
					End If
					If Me.gridBlocks(i, j).Xcoordinate < CSng((Me.GridDimX - 2)) AndAlso Me.gridBlocks(i, j).Ycoordinate >= 2F AndAlso Me.gridBlocks(i, j).hasBlock Then
						Me.DiagonalsDownCheck(i, j, Me.gridBlocks(i, j).blockHeld.suit)
					End If
					If Me.gridBlocks(i, j).Xcoordinate < CSng((Me.GridDimX - 2)) AndAlso Me.gridBlocks(i, j).hasBlock Then
						Me.RowsCheck(i, j, Me.gridBlocks(i, j).blockHeld.suit)
					End If
					If Me.gridBlocks(i, j).Ycoordinate < CSng((Me.GridDimY - 2)) AndAlso Me.gridBlocks(i, j).hasBlock Then
						Me.ColumnsCheck(i, j, Me.gridBlocks(i, j).blockHeld.suit, True)
					End If
				End If
			Next
		Next
	End Sub

	' Token: 0x06001BDA RID: 7130 RVA: 0x000FE1D8 File Offset: 0x000FC5D8
	Private Sub RowsCheck(x As Integer, y As Integer, suit As DicePalaceCardLevelBlock.Suit)
		Dim num As Integer = x + 1
		Dim num2 As Integer = num + 1
		Dim i As Integer = num2 + 1
		If Me.gridBlocks(num, y).blockHeld IsNot Nothing AndAlso Me.gridBlocks(num2, y).blockHeld IsNot Nothing AndAlso Me.gridBlocks(num, y).blockHeld.suit = suit AndAlso Me.gridBlocks(num2, y).blockHeld.suit = suit Then
			Me.DeleteBlock(Me.gridBlocks(x, y))
			Me.DeleteBlock(Me.gridBlocks(num, y))
			Me.DeleteBlock(Me.gridBlocks(num2, y))
			If y < Me.GridDimY - 2 Then
				Me.ColumnsCheck(x, y, suit, True)
				Me.ColumnsCheck(num, y, suit, True)
				Me.ColumnsCheck(num2, y, suit, True)
			End If
			If y >= 2 Then
				Me.ColumnsCheck(x, y, suit, False)
				Me.ColumnsCheck(num, y, suit, False)
				Me.ColumnsCheck(num2, y, suit, False)
			End If
			If num2 < Me.GridDimX - 2 Then
				Me.DiagonalsUpCheck(x, y, suit)
				Me.DiagonalsUpCheck(num, y, suit)
				Me.DiagonalsUpCheck(num2, y, suit)
			End If
			If y >= 2 AndAlso x < Me.GridDimX - 2 Then
				Me.DiagonalsDownCheck(x, y, suit)
				Me.DiagonalsDownCheck(num, y, suit)
				Me.DiagonalsDownCheck(num2, y, suit)
			End If
			While i <= Me.GridDimX - 1
				If Not Me.gridBlocks(i, y).hasBlock OrElse Me.gridBlocks(i, y).blockHeld.suit <> suit Then
					Exit While
				End If
				Me.DeleteBlock(Me.gridBlocks(i, y))
				If i >= Me.GridDimX Then
					Exit While
				End If
				i += 1
			End While
		End If
	End Sub

	' Token: 0x06001BDB RID: 7131 RVA: 0x000FE3C8 File Offset: 0x000FC7C8
	Private Sub ColumnsCheck(x As Integer, y As Integer, suit As DicePalaceCardLevelBlock.Suit, checkingUp As Boolean)
		Dim num As Integer = y + 1
		Dim num2 As Integer = num + 1
		Dim num3 As Integer = y - 1
		Dim num4 As Integer = y - 2
		Dim num5 As Integer
		Dim num6 As Integer
		If checkingUp Then
			num5 = num
			num6 = num2
		Else
			num5 = num3
			num6 = num4
		End If
		If Me.gridBlocks(x, num5).blockHeld IsNot Nothing AndAlso Me.gridBlocks(x, num6).blockHeld IsNot Nothing AndAlso Me.gridBlocks(x, num5).blockHeld.suit = suit AndAlso Me.gridBlocks(x, num6).blockHeld.suit = suit Then
			Me.DeleteBlock(Me.gridBlocks(x, y))
			Me.DeleteBlock(Me.gridBlocks(x, num5))
			Me.DeleteBlock(Me.gridBlocks(x, num6))
			If x < Me.GridDimX - 2 Then
				If y >= 2 Then
					Me.DiagonalsDownCheck(x, y, suit)
				End If
				If num >= 2 Then
					Me.DiagonalsDownCheck(x, num, suit)
				End If
				If num2 >= 2 Then
					Me.DiagonalsDownCheck(x, num2, suit)
				End If
				Me.RowsCheck(x, y, suit)
				Me.RowsCheck(x, num, suit)
				Me.RowsCheck(x, num2, suit)
				If num2 < Me.GridDimY - 2 Then
					Me.DiagonalsUpCheck(x, y, suit)
					Me.DiagonalsUpCheck(x, num, suit)
					Me.DiagonalsUpCheck(x, num2, suit)
				End If
			End If
			Me.ExtraCheck(x, y, checkingUp, suit)
		End If
	End Sub

	' Token: 0x06001BDC RID: 7132 RVA: 0x000FE53C File Offset: 0x000FC93C
	Private Sub DiagonalsUpCheck(x As Integer, y As Integer, suit As DicePalaceCardLevelBlock.Suit)
		Dim num As Integer = x + 1
		Dim num2 As Integer = num + 1
		Dim num3 As Integer = num2 + 1
		Dim num4 As Integer = y + 1
		Dim num5 As Integer = num4 + 1
		Dim num6 As Integer = num5 + 1
		If Me.gridBlocks(num, num4).blockHeld IsNot Nothing AndAlso Me.gridBlocks(num2, num5).blockHeld IsNot Nothing AndAlso Me.gridBlocks(num, num4).blockHeld.suit = suit AndAlso Me.gridBlocks(num2, num5).blockHeld.suit = suit Then
			Me.DeleteBlock(Me.gridBlocks(x, y))
			Me.DeleteBlock(Me.gridBlocks(num, num4))
			Me.DeleteBlock(Me.gridBlocks(num2, num5))
			If num2 < Me.GridDimX - 2 Then
				Me.RowsCheck(x, y, suit)
				Me.RowsCheck(num, num4, suit)
				Me.RowsCheck(num2, num5, suit)
			End If
			If y >= 2 Then
				Me.ColumnsCheck(x, y, suit, False)
			End If
			If num4 >= 2 Then
				Me.ColumnsCheck(num, num4, suit, False)
			End If
			Me.ColumnsCheck(num2, num5, suit, False)
			If num5 >= 2 Then
			End If
			While num3 <= Me.GridDimX - 1 AndAlso num6 <= Me.GridDimY - 1
				If Not Me.gridBlocks(num3, num6).hasBlock OrElse Me.gridBlocks(num3, num6).blockHeld.suit <> suit Then
					Exit While
				End If
				Me.DeleteBlock(Me.gridBlocks(num3, num6))
				If num3 >= Me.GridDimX OrElse num6 >= Me.GridDimY Then
					Exit While
				End If
				num3 += 1
				num6 += 1
			End While
		End If
	End Sub

	' Token: 0x06001BDD RID: 7133 RVA: 0x000FE718 File Offset: 0x000FCB18
	Private Sub DiagonalsDownCheck(x As Integer, y As Integer, suit As DicePalaceCardLevelBlock.Suit)
		Dim num As Integer = x + 1
		Dim num2 As Integer = num + 1
		Dim num3 As Integer = num2 + 1
		Dim num4 As Integer = y - 1
		Dim num5 As Integer = num4 - 1
		Dim num6 As Integer = num5 - 1
		If Me.gridBlocks(num, num4).blockHeld IsNot Nothing AndAlso Me.gridBlocks(num2, num5).blockHeld IsNot Nothing AndAlso Me.gridBlocks(num, num4).blockHeld.suit = suit AndAlso Me.gridBlocks(num2, num5).blockHeld.suit = suit Then
			Me.DeleteBlock(Me.gridBlocks(x, y))
			Me.DeleteBlock(Me.gridBlocks(num, num4))
			Me.DeleteBlock(Me.gridBlocks(num2, num5))
			If num2 < Me.GridDimX - 2 Then
				Me.RowsCheck(x, y, suit)
				Me.RowsCheck(num, num4, suit)
				Me.RowsCheck(num2, num5, suit)
			End If
			If num4 >= 2 Then
				Me.ColumnsCheck(num, num4, suit, False)
			End If
			If num5 >= 2 Then
				Me.ColumnsCheck(num2, num5, suit, False)
			End If
			Me.ColumnsCheck(x, y, suit, False)
			While num3 <= Me.GridDimX - 1 AndAlso num6 >= 1
				If Not Me.gridBlocks(num3, num6).hasBlock OrElse Me.gridBlocks(num3, num6).blockHeld.suit <> suit Then
					Exit While
				End If
				Me.DeleteBlock(Me.gridBlocks(num3, num6))
				If num3 >= Me.GridDimX OrElse num6 <= 1 Then
					Exit While
				End If
				num3 += 1
				num6 -= 1
			End While
		End If
	End Sub

	' Token: 0x06001BDE RID: 7134 RVA: 0x000FE8E0 File Offset: 0x000FCCE0
	Private Sub ExtraCheck(x As Integer, y As Integer, checkingUp As Boolean, suit As DicePalaceCardLevelBlock.Suit)
		Dim i As Integer
		Dim num As Integer
		If checkingUp Then
			i = y + 1
			num = 1
		Else
			i = y - 1
			num = -1
		End If
		While i <= Me.GridDimY - 1
			If Not Me.gridBlocks(x, i).hasBlock OrElse Me.gridBlocks(x, i).blockHeld.suit <> suit Then
				Exit While
			End If
			Me.DeleteBlock(Me.gridBlocks(x, i))
			If i >= Me.GridDimY Then
				Exit While
			End If
			i += num
		End While
	End Sub

	' Token: 0x06001BDF RID: 7135 RVA: 0x000FE988 File Offset: 0x000FCD88
	Private Sub ScaleCheck()
		Dim position As Vector3 = MyBase.transform.position
		Dim i As Integer = 0
		Dim num As Single = 0F
		Dim flag As Boolean = False
		For j As Integer = 0 To Me.GridDimY - 1
			For k As Integer = 0 To Me.GridDimX - 1
				If Me.gridBlocks(k, j).blockHeld IsNot Nothing AndAlso CSng(j) <> Me.currentHeight Then
					num = CSng(j)
					flag = True
					Me.currentHeight = CSng(j)
				End If
			Next
			While i < Me.GridDimX - 1
				If Not(Me.gridBlocks(i, j).blockHeld IsNot Nothing) Then
					Exit While
				End If
				i += 1
				If i = Me.GridDimX - 1 Then
					num = CSng(j)
					flag = True
				End If
			End While
		Next
		If flag Then
			MyBase.StartCoroutine(Me.move_scale_cr(num))
		End If
		MyBase.transform.position = position
	End Sub

	' Token: 0x06001BE0 RID: 7136 RVA: 0x000FEA94 File Offset: 0x000FCE94
	Private Iterator Function move_scale_cr(y As Single) As IEnumerator
		Dim pos As Vector3 = MyBase.transform.position
		Dim speed As Single = 200F
		Me.targetPos = Me.startingPos - Me.gridBlockPrefab.GetComponent(Of Renderer)().bounds.size.y * (y + 1F)
		While MyBase.transform.position.y <> Me.targetPos
			pos.y = Mathf.MoveTowards(MyBase.transform.position.y, Me.targetPos, speed * CupheadTime.Delta)
			MyBase.transform.position = pos
			Yield Nothing
		End While
		Yield Nothing
		Return
	End Function

	' Token: 0x06001BE1 RID: 7137 RVA: 0x000FEAB8 File Offset: 0x000FCEB8
	Private Sub CheckForTop()
		For i As Integer = 0 To Me.GridDimX - 1
			If Me.gridBlocks(i, Me.GridDimY - 1).hasBlock Then
				Me.KillAllBlocks()
			End If
		Next
		Me.checkAgain = False
	End Sub

	' Token: 0x06001BE2 RID: 7138 RVA: 0x000FEB08 File Offset: 0x000FCF08
	Private Sub KillAllBlocks()
		For i As Integer = 0 To Me.GridDimX - 1
			For j As Integer = 0 To Me.GridDimY - 1
				If Me.gridBlocks(i, j).hasBlock Then
					Me.DeleteBlock(Me.gridBlocks(i, j))
				End If
			Next
		Next
		Me.targetPos = Me.startingPos
		Dim position As Vector3 = MyBase.transform.position
		position.y = Me.startingPos
		MyBase.transform.position = position
	End Sub

	' Token: 0x06001BE3 RID: 7139 RVA: 0x000FEB9E File Offset: 0x000FCF9E
	Private Sub DeleteBlock(gridBlock As DicePalaceCardLevelGridBlock)
		If gridBlock.blockHeld IsNot Nothing Then
			gridBlock.blockHeld.DestroyBlock()
			gridBlock.blockHeld = Nothing
			gridBlock.hasBlock = False
		End If
	End Sub

	' Token: 0x040024D8 RID: 9432
	<SerializeField()>
	Private columnObject As DicePalaceCardLevelColumn

	' Token: 0x040024D9 RID: 9433
	<SerializeField()>
	Private hearts As DicePalaceCardLevelBlock

	' Token: 0x040024DA RID: 9434
	<SerializeField()>
	Private spades As DicePalaceCardLevelBlock

	' Token: 0x040024DB RID: 9435
	<SerializeField()>
	Private clubs As DicePalaceCardLevelBlock

	' Token: 0x040024DC RID: 9436
	<SerializeField()>
	Private diamonds As DicePalaceCardLevelBlock

	' Token: 0x040024DD RID: 9437
	<SerializeField()>
	Private gridBlockPrefab As DicePalaceCardLevelGridBlock

	' Token: 0x040024DE RID: 9438
	Private totalColumns As List(Of DicePalaceCardLevelColumn)

	' Token: 0x040024DF RID: 9439
	Public gridBlocks As DicePalaceCardLevelGridBlock(,)

	' Token: 0x040024E0 RID: 9440
	Private properties As LevelProperties.DicePalaceCard.Blocks

	' Token: 0x040024E1 RID: 9441
	Private distanceToPlayerY As Single

	' Token: 0x040024E2 RID: 9442
	Private amountToDropBy As Single

	' Token: 0x040024E3 RID: 9443
	Private startingPos As Single

	' Token: 0x040024E4 RID: 9444
	Private targetPos As Single

	' Token: 0x040024E5 RID: 9445
	Private currentHeight As Single = -1F

	' Token: 0x040024E6 RID: 9446
	Public GridDimX As Integer

	' Token: 0x040024E7 RID: 9447
	Public GridDimY As Integer

	' Token: 0x040024E8 RID: 9448
	Private GridSpacing As Single

	' Token: 0x040024E9 RID: 9449
	Private doneDropping As Boolean

	' Token: 0x040024EA RID: 9450
	Private checkAgain As Boolean = True

	' Token: 0x040024EB RID: 9451
	Private typePattern As String()

	' Token: 0x040024EC RID: 9452
	Private amountPattern As String()

	' Token: 0x040024ED RID: 9453
	Private currentStopYPos As List(Of Integer)

	' Token: 0x040024EE RID: 9454
	Private amountIndex As Integer

	' Token: 0x040024EF RID: 9455
	Private typeIndex As Integer

	' Token: 0x040024F0 RID: 9456
	Private selectedPrefab As DicePalaceCardLevelBlock
End Class
