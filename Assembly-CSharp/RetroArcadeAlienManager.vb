Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000737 RID: 1847
Public Class RetroArcadeAlienManager
	Inherits LevelProperties.RetroArcade.Entity

	' Token: 0x170003D6 RID: 982
	' (get) Token: 0x06002836 RID: 10294 RVA: 0x001770C9 File Offset: 0x001754C9
	' (set) Token: 0x06002837 RID: 10295 RVA: 0x001770D1 File Offset: 0x001754D1
	Public Property direction As RetroArcadeAlien.Direction

	' Token: 0x170003D7 RID: 983
	' (get) Token: 0x06002838 RID: 10296 RVA: 0x001770DA File Offset: 0x001754DA
	' (set) Token: 0x06002839 RID: 10297 RVA: 0x001770E2 File Offset: 0x001754E2
	Public Property moveSpeed As Single

	' Token: 0x0600283A RID: 10298 RVA: 0x001770EC File Offset: 0x001754EC
	Public Sub StartAliens()
		Me.p = MyBase.properties.CurrentState.aliens
		Me.aliens = New RetroArcadeAlien(Me.p.numColumns - 1, Me.alienPrefabs.Length - 1) {}
		Me.direction = If((Not Rand.Bool()), RetroArcadeAlien.Direction.Right, RetroArcadeAlien.Direction.Left)
		For i As Integer = 0 To Me.aliens.GetLength(0) - 1
			For j As Integer = 0 To Me.aliens.GetLength(1) - 1
				Dim vector As Vector2 = New Vector2(50F * (CSng(i) - CSng((Me.aliens.GetLength(0) - 1)) / 2F), 230F - CSng(j) * 40F + 170F)
				Me.aliens(i, j) = Me.alienPrefabs(j).Create(vector, i, Me, Me.p)
				Me.aliens(i, j).MoveY(-170F)
			Next
		Next
		Me.numDied = 0
		Me.moveSpeed = 640F / Me.p.moveTime
		Me.shotRate = Me.p.shotRate.Clone()
		Me.currentTopRowY = 230F
		MyBase.StartCoroutine(Me.turn_cr())
		MyBase.StartCoroutine(Me.shoot_cr())
		MyBase.StartCoroutine(Me.randomShot_cr())
		MyBase.StartCoroutine(Me.bonus_cr())
	End Sub

	' Token: 0x0600283B RID: 10299 RVA: 0x00177264 File Offset: 0x00175664
	Private Iterator Function turn_cr() As IEnumerator
		While True
			If(Me.direction = RetroArcadeAlien.Direction.Right AndAlso Me.getRightmost().transform.position.x > 320F) OrElse (Me.direction = RetroArcadeAlien.Direction.Left AndAlso Me.getLeftmost().transform.position.x < -320F) Then
				Me.direction = If((Me.direction <> RetroArcadeAlien.Direction.Left), RetroArcadeAlien.Direction.Left, RetroArcadeAlien.Direction.Right)
				Dim num As Single = -40F
				If Me.currentTopRowY - CSng((Me.aliens.GetLength(1) - 1)) * 40F + num < -40F Then
					num = 230F - Me.currentTopRowY
				End If
				For i As Integer = 0 To Me.aliens.GetLength(0) - 1
					If Me.isColumnAlive(i) Then
						For j As Integer = 0 To Me.aliens.GetLength(1) - 1
							Me.aliens(i, j).MoveY(num)
						Next
					End If
				Next
				Me.currentTopRowY += num
			End If
			Yield New WaitForFixedUpdate()
		End While
		Return
	End Function

	' Token: 0x0600283C RID: 10300 RVA: 0x00177280 File Offset: 0x00175680
	Private Iterator Function shoot_cr() As IEnumerator
		Dim columnPattern As String() = Me.p.shotColumnPattern.RandomChoice().Split(New Char() { ","c })
		Dim columnPatternIndex As Integer = Global.UnityEngine.Random.Range(0, columnPattern.Length)
		Yield CupheadTime.WaitForSeconds(Me, Me.shotRate.RandomFloat())
		While True
			columnPatternIndex = (columnPatternIndex + 1) Mod columnPattern.Length
			Dim column As Integer = 0
			Parser.IntTryParse(columnPattern(columnPatternIndex), column)
			column -= 1
			If Me.isColumnAlive(column) Then
				Me.getBottommostInColumn(column).Shoot()
				Yield CupheadTime.WaitForSeconds(Me, Me.shotRate.RandomFloat())
			End If
		End While
		Return
	End Function

	' Token: 0x0600283D RID: 10301 RVA: 0x0017729C File Offset: 0x0017569C
	Private Iterator Function randomShot_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, MathUtils.ExpRandom(Me.p.randomShotAverageTime))
		While True
			Dim column As Integer = Global.UnityEngine.Random.Range(0, Me.aliens.GetLength(0))
			While Not Me.isColumnAlive(column)
				column = Global.UnityEngine.Random.Range(0, Me.aliens.GetLength(0))
			End While
			Me.getBottommostInColumn(column).Shoot()
			Yield CupheadTime.WaitForSeconds(Me, MathUtils.ExpRandom(Me.p.randomShotAverageTime))
		End While
		Return
	End Function

	' Token: 0x0600283E RID: 10302 RVA: 0x001772B8 File Offset: 0x001756B8
	Private Iterator Function bonus_cr() As IEnumerator
		For i As Integer = 0 To Me.p.bonusAppearCount - 1
			Yield CupheadTime.WaitForSeconds(Me, Me.p.bonusAppearTime.RandomFloat())
			Me.bonusAlien.Create(If((Not Rand.Bool()), RetroArcadeBonusAlien.Direction.Right, RetroArcadeBonusAlien.Direction.Left), Me.p)
		Next
		Return
	End Function

	' Token: 0x0600283F RID: 10303 RVA: 0x001772D4 File Offset: 0x001756D4
	Private Function getLeftmost() As RetroArcadeAlien
		For i As Integer = 0 To Me.aliens.GetLength(0) - 1
			For j As Integer = 0 To Me.aliens.GetLength(1) - 1
				If Not Me.aliens(i, j).IsDead Then
					Return Me.aliens(i, j)
				End If
			Next
		Next
		Return Nothing
	End Function

	' Token: 0x06002840 RID: 10304 RVA: 0x00177344 File Offset: 0x00175744
	Private Function getRightmost() As RetroArcadeAlien
		For i As Integer = Me.aliens.GetLength(0) - 1 To 0 Step -1
			For j As Integer = 0 To Me.aliens.GetLength(1) - 1
				If Not Me.aliens(i, j).IsDead Then
					Return Me.aliens(i, j)
				End If
			Next
		Next
		Return Nothing
	End Function

	' Token: 0x06002841 RID: 10305 RVA: 0x001773B4 File Offset: 0x001757B4
	Private Function getTopmost() As RetroArcadeAlien
		For i As Integer = 0 To Me.aliens.GetLength(1) - 1
			For j As Integer = 0 To Me.aliens.GetLength(0) - 1
				If Not Me.aliens(j, i).IsDead Then
					Return Me.aliens(j, i)
				End If
			Next
		Next
		Return Nothing
	End Function

	' Token: 0x06002842 RID: 10306 RVA: 0x00177424 File Offset: 0x00175824
	Private Function getBottommost() As RetroArcadeAlien
		For i As Integer = Me.aliens.GetLength(1) - 1 To 0 Step -1
			For j As Integer = 0 To Me.aliens.GetLength(0) - 1
				If Not Me.aliens(j, i).IsDead Then
					Return Me.aliens(j, i)
				End If
			Next
		Next
		Return Nothing
	End Function

	' Token: 0x06002843 RID: 10307 RVA: 0x00177494 File Offset: 0x00175894
	Private Function isColumnAlive(x As Integer) As Boolean
		For i As Integer = 0 To Me.aliens.GetLength(1) - 1
			If Not Me.aliens(x, i).IsDead Then
				Return True
			End If
		Next
		Return False
	End Function

	' Token: 0x06002844 RID: 10308 RVA: 0x001774D8 File Offset: 0x001758D8
	Private Function getBottommostInColumn(x As Integer) As RetroArcadeAlien
		For i As Integer = Me.aliens.GetLength(1) - 1 To 0 Step -1
			If Not Me.aliens(x, i).IsDead Then
				Return Me.aliens(x, i)
			End If
		Next
		Return Nothing
	End Function

	' Token: 0x06002845 RID: 10309 RVA: 0x0017752C File Offset: 0x0017592C
	Public Sub OnAlienDie(alien As RetroArcadeAlien)
		Me.numDied += 1
		Me.moveSpeed = 640F / (Me.p.moveTime - CSng(Me.numDied) * Me.p.moveTimeDecrease)
		Me.shotRate.max -= Me.p.shotRateDecrease
		Me.shotRate.min -= Me.p.shotRateDecrease
		If Not Me.isColumnAlive(alien.ColumnIndex) Then
			For i As Integer = 0 To Me.aliens.GetLength(1) - 1
				Me.aliens(alien.ColumnIndex, i).MoveY(170F + (230F - Me.aliens(alien.ColumnIndex, 0).transform.position.y))
			Next
		End If
		If Me.numDied >= Me.aliens.Length Then
			Me.StopAllCoroutines()
			MyBase.properties.DealDamageToNextNamedState()
			MyBase.StartCoroutine(Me.waveOver_cr())
		End If
	End Sub

	' Token: 0x06002846 RID: 10310 RVA: 0x00177658 File Offset: 0x00175A58
	Private Iterator Function waveOver_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 1F)
		Dim array As RetroArcadeAlien(,) = Me.aliens
		Dim length As Integer = array.GetLength(0)
		Dim length2 As Integer = array.GetLength(1)
		For i As Integer = 0 To length - 1
			For j As Integer = 0 To length2 - 1
				Dim retroArcadeAlien As RetroArcadeAlien = array(i, j)
				Global.UnityEngine.[Object].Destroy(retroArcadeAlien.gameObject)
			Next
		Next
		Return
	End Function

	' Token: 0x040030FE RID: 12542
	Private Const TOP_ROW_Y As Single = 230F

	' Token: 0x040030FF RID: 12543
	Private Const COLUMN_SPACING As Single = 50F

	' Token: 0x04003100 RID: 12544
	Private Const ROW_SPACING As Single = 40F

	' Token: 0x04003101 RID: 12545
	Private Const TURNAROUND_X As Single = 320F

	' Token: 0x04003102 RID: 12546
	Private Const MIN_Y As Single = -40F

	' Token: 0x04003103 RID: 12547
	Private Const OFFSCREEN_MOVE_Y As Single = 170F

	' Token: 0x04003104 RID: 12548
	<SerializeField()>
	Private alienPrefabs As RetroArcadeAlien()

	' Token: 0x04003105 RID: 12549
	Private aliens As RetroArcadeAlien(,)

	' Token: 0x04003106 RID: 12550
	<SerializeField()>
	Private bonusAlien As RetroArcadeBonusAlien

	' Token: 0x04003109 RID: 12553
	Private shotRate As MinMax

	' Token: 0x0400310A RID: 12554
	Private numDied As Integer

	' Token: 0x0400310B RID: 12555
	Private currentTopRowY As Single

	' Token: 0x0400310C RID: 12556
	Private p As LevelProperties.RetroArcade.Aliens
End Class
