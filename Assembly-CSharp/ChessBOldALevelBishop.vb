Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports UnityEngine

' Token: 0x02000528 RID: 1320
Public Class ChessBOldALevelBishop
	Inherits LevelProperties.ChessBOldA.Entity

	' Token: 0x060017C8 RID: 6088 RVA: 0x000D5EF6 File Offset: 0x000D42F6
	Private Sub Start()
		Me.walls = New List(Of ChessBOldALevelWall)()
		AddHandler Me.pink.OnActivate, AddressOf Me.GotParried
		Me.damageDealer = DamageDealer.NewEnemy()
		MyBase.GetComponent(Of SpriteRenderer)().color = Color.red
	End Sub

	' Token: 0x060017C9 RID: 6089 RVA: 0x000D5F35 File Offset: 0x000D4335
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x060017CA RID: 6090 RVA: 0x000D5F53 File Offset: 0x000D4353
	Private Sub Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x060017CB RID: 6091 RVA: 0x000D5F6C File Offset: 0x000D436C
	Public Overrides Sub LevelInit(properties As LevelProperties.ChessBOldA)
		MyBase.LevelInit(properties)
		AddHandler Level.Current.OnWinEvent, AddressOf Me.Win
		Dim bishop As LevelProperties.ChessBOldA.Bishop = properties.CurrentState.bishop
		If Not bishop.canHurtPlayer Then
			MyBase.GetComponent(Of Collider2D)().enabled = False
		End If
		Dim num As Single = properties.CurrentHealth / CSng(properties.CurrentState.bishop.bishopHealth)
		Me.HPToDecrease = Mathf.Ceil(num)
		MyBase.transform.SetScale(New Single?(bishop.bishopScale), New Single?(bishop.bishopScale), New Single?(bishop.bishopScale))
		Me.pathIndex = 0
		MyBase.StartCoroutine(Me.intro_cr())
		MyBase.StartCoroutine(Me.pink_cr())
		Me.SetValues()
		MyBase.StartCoroutine(Me.turret_cr())
	End Sub

	' Token: 0x060017CC RID: 6092 RVA: 0x000D603E File Offset: 0x000D443E
	Private Sub GotParried()
		MyBase.properties.DealDamage(Me.HPToDecrease)
		MyBase.StartCoroutine(Me.stunned_cr())
	End Sub

	' Token: 0x060017CD RID: 6093 RVA: 0x000D6060 File Offset: 0x000D4460
	Private Iterator Function stunned_cr() As IEnumerator
		Me.RemoveCurrentWalls()
		Me.walls.Clear()
		Me.isStunned = True
		Me.pink.enabled = False
		MyBase.GetComponent(Of SpriteRenderer)().color = Color.yellow
		Yield CupheadTime.WaitForSeconds(Me, MyBase.properties.CurrentState.bishop.stunnedTime)
		MyBase.GetComponent(Of SpriteRenderer)().color = Color.red
		Me.pink.enabled = True
		Me.isStunned = False
		If MyBase.properties.CurrentHealth > 0F Then
			Me.phase += 1
			Me.SetValues()
			Me.SetPathValues()
		End If
		Return
	End Function

	' Token: 0x060017CE RID: 6094 RVA: 0x000D607B File Offset: 0x000D447B
	Private Sub SetValues()
		Me.SetPinkValues()
		Me.SetWallValues()
	End Sub

	' Token: 0x060017CF RID: 6095 RVA: 0x000D6089 File Offset: 0x000D4489
	Private Sub Win()
		Me.StopAllCoroutines()
	End Sub

	' Token: 0x060017D0 RID: 6096 RVA: 0x000D6094 File Offset: 0x000D4494
	Private Iterator Function intro_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 4F)
		Me.SetPathValues()
		Yield Nothing
		Return
	End Function

	' Token: 0x060017D1 RID: 6097 RVA: 0x000D60B0 File Offset: 0x000D44B0
	Public Sub SetPathValues()
		Dim bishopPath As LevelProperties.ChessBOldA.BishopPath = MyBase.properties.CurrentState.bishopPath
		Dim num As Integer = Mathf.Clamp(Me.phase, 0, bishopPath.pathTypeString.Split(New Char() { ","c }).Length - 1)
		Dim num2 As Integer = Mathf.Clamp(Me.phase, 0, bishopPath.pathSpeedString.Split(New Char() { ","c }).Length - 1)
		Dim num3 As Integer = Mathf.Clamp(Me.phase, 0, bishopPath.pathDirString.Split(New Char() { ","c }).Length - 1)
		Parser.FloatTryParse(bishopPath.pathSpeedString.Split(New Char() { ","c })(num2), Me.pathSpeed)
		Me.pathIsClockwise = bishopPath.pathDirString.Split(New Char() { ","c })(num3)(0) = "R"c
		Me.previousPathType = Me.pathType
		Dim c As Char = bishopPath.pathTypeString.Split(New Char() { ","c })(num)(0)
		If c <> "S"c Then
			If c <> "I"c Then
				If c = "Q"c Then
					Me.pathType = ChessBOldALevelBishop.PathType.Square
					MyBase.StartCoroutine(Me.box_cr())
				End If
			Else
				Me.pathType = ChessBOldALevelBishop.PathType.Infinite
				MyBase.StartCoroutine(Me.infinite_cr())
			End If
		Else
			Me.pathType = ChessBOldALevelBishop.PathType.Straight
			MyBase.StartCoroutine(Me.straight_cr())
		End If
	End Sub

	' Token: 0x060017D2 RID: 6098 RVA: 0x000D6228 File Offset: 0x000D4628
	Private Iterator Function move_cr(start As Vector3, [end] As Vector3) As IEnumerator
		Dim t As Single = 0F
		Dim time As Single = Me.pathSpeed
		Dim wait As YieldInstruction = New WaitForFixedUpdate()
		While t < time
			t += CupheadTime.FixedDelta
			MyBase.transform.position = Vector3.Lerp(start, [end], t / time)
			Yield wait
		End While
		Return
	End Function

	' Token: 0x060017D3 RID: 6099 RVA: 0x000D6254 File Offset: 0x000D4654
	Private Iterator Function straight_cr() As IEnumerator
		Dim p As LevelProperties.ChessBOldA.BishopPath = MyBase.properties.CurrentState.bishopPath
		Dim t As Single = Me.lerpPos
		Dim maxTime As Single = Me.pathSpeed
		Dim wait As YieldInstruction = New WaitForFixedUpdate()
		Dim startX As Single = -p.straightPathLength
		Dim endX As Single = p.straightPathLength
		Dim one As Single = 1F
		If Me.previousPathType <> ChessBOldALevelBishop.PathType.Straight Then
			Me.straightValue = If((Not Me.pathIsClockwise), (one - t / maxTime), (t / maxTime))
			Yield MyBase.StartCoroutine(Me.move_cr(MyBase.transform.position, New Vector3(Mathf.Lerp(startX, endX, Me.straightValue), p.straightPathHeight)))
		End If
		While Not Me.isStunned
			If t < maxTime Then
				t += CupheadTime.FixedDelta
				Me.straightValue = If((Not Me.pathIsClockwise), (one - t / maxTime), (t / maxTime))
				MyBase.transform.SetPosition(New Single?(Mathf.Lerp(startX, endX, Me.straightValue)), Nothing, Nothing)
			Else
				Me.pathIsClockwise = Not Me.pathIsClockwise
				t = 0F
			End If
			Yield wait
		End While
		Me.lerpPos = t
		Yield Nothing
		Return
	End Function

	' Token: 0x060017D4 RID: 6100 RVA: 0x000D6270 File Offset: 0x000D4670
	Private Iterator Function infinite_cr() As IEnumerator
		Dim p As LevelProperties.ChessBOldA.BishopPath = MyBase.properties.CurrentState.bishopPath
		Dim wait As YieldInstruction = New WaitForFixedUpdate()
		Dim loopSizeX As Single = p.infinitePathLength
		Dim loopSizeY As Single = p.infinitePathWidth
		Dim speed As Single = Me.pathSpeed
		Dim invert As Boolean = Me.pathIsClockwise
		Me.pivotPoint.transform.SetPosition(New Single?(loopSizeX), New Single?(p.infinitePathHeight), Nothing)
		Dim endPos As Vector3 = Vector3.zero
		Dim pivotOffset As Vector3 = Vector3.left * 2F * loopSizeX
		If Me.previousPathType <> ChessBOldALevelBishop.PathType.Infinite Then
			endPos = If((Not invert), Me.pivotPoint.position, (Me.pivotPoint.position + pivotOffset))
			Dim value As Single = CSng(If((Not invert), (-1), 1))
			Dim handleRotationX As Vector3 = New Vector3(Mathf.Cos(Me.infinityAngle) * value * loopSizeX, 0F, 0F)
			Dim handleRotationY As Vector3 = New Vector3(0F, Mathf.Sin(Me.infinityAngle) * loopSizeY, 0F)
			endPos += handleRotationX + handleRotationY
			Yield MyBase.StartCoroutine(Me.move_cr(MyBase.transform.position, endPos))
		End If
		While Not Me.isStunned
			Me.infinityAngle += speed * CupheadTime.Delta
			If Me.infinityAngle > 6.2831855F Then
				invert = Not invert
				Me.infinityAngle -= 6.2831855F
			End If
			If Me.infinityAngle < 0F Then
				Me.infinityAngle += 6.2831855F
			End If
			Dim value As Single
			If invert Then
				MyBase.transform.position = Me.pivotPoint.position + pivotOffset
				value = 1F
			Else
				MyBase.transform.position = Me.pivotPoint.position
				value = -1F
			End If
			Dim handleRotationX As Vector3 = New Vector3(Mathf.Cos(Me.infinityAngle) * value * loopSizeX, 0F, 0F)
			Dim handleRotationY As Vector3 = New Vector3(0F, Mathf.Sin(Me.infinityAngle) * loopSizeY, 0F)
			MyBase.transform.position += handleRotationX + handleRotationY
			Yield wait
		End While
		Yield Nothing
		Return
	End Function

	' Token: 0x060017D5 RID: 6101 RVA: 0x000D628C File Offset: 0x000D468C
	Private Iterator Function box_cr() As IEnumerator
		Dim p As LevelProperties.ChessBOldA.BishopPath = MyBase.properties.CurrentState.bishopPath
		Dim boxCenter As Single = p.squarePathHeight
		Dim length As Single = p.squarePathLength / 2F
		Dim height As Single = p.squarePathWidth / 2F
		Dim topLeft As Vector3 = New Vector3(boxCenter - length, boxCenter + height)
		Dim topRight As Vector3 = New Vector3(boxCenter + length, boxCenter + height)
		Dim bottomLeft As Vector3 = New Vector3(boxCenter - length, boxCenter - height)
		Dim bottomRight As Vector3 = New Vector3(boxCenter + length, boxCenter - height)
		Dim positions As Vector3() = New Vector3() { topRight, bottomRight, bottomLeft, topLeft }
		Dim incrementBy As Integer = If((Not Me.pathIsClockwise), (-1), 1)
		Dim distance As Single = 0F
		Dim speed As Single = 0F
		Dim endPos As Vector3 = positions(Me.pathIndex)
		If Me.previousPathType <> ChessBOldALevelBishop.PathType.Square Then
			Yield MyBase.StartCoroutine(Me.move_cr(MyBase.transform.position, endPos))
		End If
		While Not Me.isStunned
			Dim wait As YieldInstruction = New WaitForFixedUpdate()
			distance = Vector3.Distance(MyBase.transform.position, endPos)
			speed = distance / Me.pathSpeed
			While MyBase.transform.position <> endPos
				MyBase.transform.position = Vector3.MoveTowards(MyBase.transform.position, endPos, speed * CupheadTime.FixedDelta)
				If Me.isStunned Then
					Exit While
				End If
				Yield wait
			End While
			If Not Me.isStunned Then
				If Me.pathIsClockwise AndAlso Me.pathIndex >= positions.Length - 1 Then
					Me.pathIndex = 0
				ElseIf Not Me.pathIsClockwise AndAlso Me.pathIndex <= 0 Then
					Me.pathIndex = positions.Length - 1
				Else
					Me.pathIndex += incrementBy
				End If
			End If
			endPos = positions(Me.pathIndex)
			Yield Nothing
		End While
		Yield Nothing
		Return
	End Function

	' Token: 0x060017D6 RID: 6102 RVA: 0x000D62A8 File Offset: 0x000D46A8
	Private Sub SetPinkValues()
		Dim pink As LevelProperties.ChessBOldA.Pink = MyBase.properties.CurrentState.pink
		Dim num As Integer = Mathf.Clamp(Me.phase, 0, pink.pinkSpeedString.Split(New Char() { ","c }).Length - 1)
		Dim num2 As Integer = Mathf.Clamp(Me.phase, 0, pink.pinkDirString.Split(New Char() { ","c }).Length - 1)
		Parser.FloatTryParse(pink.pinkSpeedString.Split(New Char() { ","c })(num), Me.pinkSpeed)
		Me.pinkIsClockwise = pink.pinkDirString.Split(New Char() { ","c })(num2)(0) = "R"c
	End Sub

	' Token: 0x060017D7 RID: 6103 RVA: 0x000D6360 File Offset: 0x000D4760
	Private Iterator Function pink_cr() As IEnumerator
		Dim p As LevelProperties.ChessBOldA.Pink = MyBase.properties.CurrentState.pink
		Dim angle As Single = 0F
		Me.pink.transform.SetScale(New Single?(p.pinkScale), New Single?(p.pinkScale), Nothing)
		Dim handleRotationX As Vector3 = Vector3.zero
		Dim handleRotationY As Vector3 = Vector3.zero
		Dim wait As YieldInstruction = New WaitForFixedUpdate()
		While True
			If Me.pinkIsClockwise Then
				angle += Me.pinkSpeed * CupheadTime.FixedDelta
			Else
				angle -= Me.pinkSpeed * CupheadTime.FixedDelta
			End If
			handleRotationX = New Vector3(Mathf.Sin(angle) * p.pinkPathRadius, 0F, 0F)
			handleRotationY = New Vector3(0F, Mathf.Cos(angle) * p.pinkPathRadius, 0F)
			Me.pink.transform.position = MyBase.transform.position
			Me.pink.transform.position += handleRotationX + handleRotationY
			Yield wait
		End While
		Return
	End Function

	' Token: 0x060017D8 RID: 6104 RVA: 0x000D637C File Offset: 0x000D477C
	Private Sub SetWallValues()
		Dim walls As LevelProperties.ChessBOldA.Walls = MyBase.properties.CurrentState.walls
		Me.nullIndex = Mathf.Clamp(Me.phase, 0, walls.wallNullString.Length - 1)
		Dim num As Integer = Mathf.Clamp(Me.phase, 0, walls.wallNumberString.Split(New Char() { ","c }).Length - 1)
		Dim num2 As Integer = Mathf.Clamp(Me.phase, 0, walls.wallSpeedString.Split(New Char() { ","c }).Length - 1)
		Dim num3 As Integer = Mathf.Clamp(Me.phase, 0, walls.wallDirString.Split(New Char() { ","c }).Length - 1)
		Dim num4 As Integer = 0
		Dim num5 As Single = 0F
		Dim array As Integer() = New Integer(walls.wallNullString(Me.nullIndex).Split(New Char() { ","c }).Length - 1) {}
		Parser.IntTryParse(walls.wallNumberString.Split(New Char() { ","c })(num), num4)
		Parser.FloatTryParse(walls.wallSpeedString.Split(New Char() { ","c })(num2), num5)
		Dim flag As Boolean = walls.wallDirString.Split(New Char() { ","c })(num3)(0) = "R"c
		Dim flag2 As Boolean = False
		For i As Integer = 0 To array.Length - 1
			flag2 = Parser.IntTryParse(walls.wallNullString(Me.nullIndex).Split(New Char() { ","c })(i), array(i))
		Next
		Dim num6 As Single = 360F / CSng(num4)
		For j As Integer = 0 To num4 - 1
			Dim flag3 As Boolean = False
			For k As Integer = 0 To array.Length - 1
				If Not flag2 Then
					Exit For
				End If
				If j = array(k) Then
					flag3 = True
					Exit For
				End If
			Next
			If Not flag3 Then
				Dim chessBOldALevelWall As ChessBOldALevelWall = Me.wallPrefab.Spawn()
				chessBOldALevelWall.StartRotate(num6 * CSng(j), Me, walls.wallPathRadius, num5, flag, walls.wallLength)
				chessBOldALevelWall.transform.parent = MyBase.transform
				Me.walls.Add(chessBOldALevelWall)
			End If
		Next
	End Sub

	' Token: 0x060017D9 RID: 6105 RVA: 0x000D65C8 File Offset: 0x000D49C8
	Private Sub RemoveCurrentWalls()
		For Each chessBOldALevelWall As ChessBOldALevelWall In Me.walls
			chessBOldALevelWall.Dead()
		Next
	End Sub

	' Token: 0x060017DA RID: 6106 RVA: 0x000D6624 File Offset: 0x000D4A24
	Private Iterator Function turret_cr() As IEnumerator
		Dim p As LevelProperties.ChessBOldA.BishopPath = MyBase.properties.CurrentState.bishopPath
		Dim player As AbstractPlayerController = PlayerManager.GetNext()
		Dim turretString As String() = p.turretShotDelayString.Split(New Char() { ","c })
		Dim turretIndex As Integer = Global.UnityEngine.Random.Range(0, turretString.Length)
		Dim gotStunned As Boolean = False
		Dim delay As Single = 0F
		While True
			If gotStunned Then
				Parser.FloatTryParse(turretString(turretIndex), delay)
				Yield CupheadTime.WaitForSeconds(Me, delay)
				gotStunned = False
			End If
			Dim dir As Vector3 = player.transform.position - MyBase.transform.position
			Me.turretShot.Create(MyBase.transform.position, MathUtils.DirectionToAngle(dir), p.turretShotSpeed)
			player = PlayerManager.GetNext()
			Parser.FloatTryParse(turretString(turretIndex), delay)
			Yield CupheadTime.WaitForSeconds(Me, delay)
			While Me.isStunned
				gotStunned = True
				Yield Nothing
			End While
		End While
		Return
	End Function

	' Token: 0x040020EF RID: 8431
	<SerializeField()>
	Private turretShot As BasicProjectile

	' Token: 0x040020F0 RID: 8432
	<SerializeField()>
	Private pink As ParrySwitch

	' Token: 0x040020F1 RID: 8433
	<SerializeField()>
	Private pivotPoint As Transform

	' Token: 0x040020F2 RID: 8434
	<SerializeField()>
	Private wallPrefab As ChessBOldALevelWall

	' Token: 0x040020F3 RID: 8435
	Private walls As List(Of ChessBOldALevelWall)

	' Token: 0x040020F4 RID: 8436
	Private damageDealer As DamageDealer

	' Token: 0x040020F5 RID: 8437
	Private pathType As ChessBOldALevelBishop.PathType

	' Token: 0x040020F6 RID: 8438
	Private previousPathType As ChessBOldALevelBishop.PathType

	' Token: 0x040020F7 RID: 8439
	Private pathIsClockwise As Boolean

	' Token: 0x040020F8 RID: 8440
	Private pathSpeed As Single

	' Token: 0x040020F9 RID: 8441
	Private pathIndex As Integer

	' Token: 0x040020FA RID: 8442
	Private positions As Vector3()

	' Token: 0x040020FB RID: 8443
	Private pinkIsClockwise As Boolean

	' Token: 0x040020FC RID: 8444
	Private pinkSpeed As Single

	' Token: 0x040020FD RID: 8445
	Private isStunned As Boolean

	' Token: 0x040020FE RID: 8446
	Private HPToDecrease As Single

	' Token: 0x040020FF RID: 8447
	Private lerpPos As Single

	' Token: 0x04002100 RID: 8448
	Private phase As Integer

	' Token: 0x04002101 RID: 8449
	Private nullIndex As Integer

	' Token: 0x04002102 RID: 8450
	Private straightValue As Single

	' Token: 0x04002103 RID: 8451
	Private infinityAngle As Single

	' Token: 0x02000529 RID: 1321
	Private Enum PathType
		' Token: 0x04002105 RID: 8453
		Straight
		' Token: 0x04002106 RID: 8454
		Infinite
		' Token: 0x04002107 RID: 8455
		Square
		' Token: 0x04002108 RID: 8456
		Pending
	End Enum
End Class
