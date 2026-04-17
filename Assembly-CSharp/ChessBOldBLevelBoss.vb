Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x0200052D RID: 1325
Public Class ChessBOldBLevelBoss
	Inherits LevelProperties.ChessBOldB.Entity

	' Token: 0x060017F5 RID: 6133 RVA: 0x000D852A File Offset: 0x000D692A
	Public Overrides Sub LevelInit(properties As LevelProperties.ChessBOldB)
		MyBase.LevelInit(properties)
		MyBase.StartCoroutine(Me.intro_cr())
	End Sub

	' Token: 0x060017F6 RID: 6134 RVA: 0x000D8540 File Offset: 0x000D6940
	Private Iterator Function intro_cr() As IEnumerator
		Me.brown = Me.bossTwo.GetComponent(Of SpriteRenderer)().color
		Yield CupheadTime.WaitForSeconds(Me, 4F)
		Me.MoveBosses()
		MyBase.StartCoroutine(Me.wait_to_shoot())
		Yield Nothing
		Return
	End Function

	' Token: 0x060017F7 RID: 6135 RVA: 0x000D855C File Offset: 0x000D695C
	Public Sub HandleHurt(gettingHurt As Boolean)
		Me.bossOne.GetComponent(Of SpriteRenderer)().color = If((Not gettingHurt), Me.brown, Color.red)
		Me.bossTwo.GetComponent(Of SpriteRenderer)().color = If((Not gettingHurt), Me.brown, Color.red)
		Me.isMoving = Not gettingHurt
	End Sub

	' Token: 0x060017F8 RID: 6136 RVA: 0x000D85C8 File Offset: 0x000D69C8
	Public Sub OnStateChanged()
		Dim boss As LevelProperties.ChessBOldB.Boss = MyBase.properties.CurrentState.boss
		Me.moveTime = boss.bossTime
		Me.bulletDelayStringMainIndex = Global.UnityEngine.Random.Range(0, boss.bulletDelayString.Length)
		Me.bulletDelayString = boss.bulletDelayString(Me.bulletDelayStringMainIndex).Split(New Char() { ","c })
		Me.bulletDelayStringIndex = Global.UnityEngine.Random.Range(0, Me.bulletDelayString.Length)
	End Sub

	' Token: 0x060017F9 RID: 6137 RVA: 0x000D863D File Offset: 0x000D6A3D
	Private Sub MoveBosses()
		MyBase.StartCoroutine(Me.move_bosses_cr())
	End Sub

	' Token: 0x060017FA RID: 6138 RVA: 0x000D864C File Offset: 0x000D6A4C
	Private Iterator Function move_bosses_cr() As IEnumerator
		Dim p As LevelProperties.ChessBOldB.Boss = MyBase.properties.CurrentState.boss
		Dim t As Single = 0F
		Dim one As Single = 1F
		Me.moveTime = p.bossTime
		Dim countingUp As Boolean = True
		Dim wait As YieldInstruction = New WaitForFixedUpdate()
		Me.isMoving = True
		While True
			While Not Me.isMoving
				Yield Nothing
			End While
			t += CupheadTime.FixedDelta
			Me.bossOne.transform.SetPosition(Nothing, New Single?(Mathf.Lerp(225F, -225F, If((Not countingUp), (one - t / Me.moveTime), (t / Me.moveTime)))), Nothing)
			Me.bossTwo.transform.SetPosition(Nothing, New Single?(Mathf.Lerp(225F, -225F, If((Not countingUp), (t / Me.moveTime), (one - t / Me.moveTime)))), Nothing)
			If t >= Me.moveTime Then
				countingUp = Not countingUp
				t = 0F
			End If
			Yield wait
		End While
		Return
	End Function

	' Token: 0x060017FB RID: 6139 RVA: 0x000D8668 File Offset: 0x000D6A68
	Private Iterator Function wait_to_shoot() As IEnumerator
		Dim leftOneShoot As Boolean = Rand.Bool()
		Dim p As LevelProperties.ChessBOldB.Boss = MyBase.properties.CurrentState.boss
		Me.OnStateChanged()
		Dim delay As Single = 0F
		While True
			While Not Me.isMoving
				Yield Nothing
			End While
			p = MyBase.properties.CurrentState.boss
			Me.bulletDelayString = p.bulletDelayString(Me.bulletDelayStringMainIndex).Split(New Char() { ","c })
			Parser.FloatTryParse(Me.bulletDelayString(Me.bulletDelayStringIndex), delay)
			Yield CupheadTime.WaitForSeconds(Me, delay)
			Dim boss As GameObject = If((Not leftOneShoot), Me.bossTwo, Me.bossOne)
			Me.Shoot(boss)
			leftOneShoot = Not leftOneShoot
			If Me.bulletDelayStringIndex < Me.bulletDelayString.Length - 1 Then
				Me.bulletDelayStringIndex += 1
			Else
				Me.bulletDelayStringMainIndex = (Me.bulletDelayStringMainIndex + 1) Mod p.bulletDelayString.Length
				Me.bulletDelayStringIndex = 0
			End If
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x060017FC RID: 6140 RVA: 0x000D8684 File Offset: 0x000D6A84
	Private Sub Shoot(boss As GameObject)
		Dim [next] As AbstractPlayerController = PlayerManager.GetNext()
		Dim vector As Vector3 = [next].center - boss.transform.position
		Dim num As Single = MathUtils.DirectionToAngle(vector)
		Me.projectile.Create(boss.transform.position, num, MyBase.properties.CurrentState.boss.bulletSpeed)
	End Sub

	' Token: 0x0400211C RID: 8476
	Private Const Y_POS As Single = 225F

	' Token: 0x0400211D RID: 8477
	<SerializeField()>
	Private projectile As BasicProjectile

	' Token: 0x0400211E RID: 8478
	<SerializeField()>
	Private bossOne As GameObject

	' Token: 0x0400211F RID: 8479
	<SerializeField()>
	Private bossTwo As GameObject

	' Token: 0x04002120 RID: 8480
	Private brown As Color

	' Token: 0x04002121 RID: 8481
	Private isMoving As Boolean

	' Token: 0x04002122 RID: 8482
	Private moveTime As Single

	' Token: 0x04002123 RID: 8483
	Private bulletDelayStringMainIndex As Integer

	' Token: 0x04002124 RID: 8484
	Private bulletDelayString As String()

	' Token: 0x04002125 RID: 8485
	Private bulletDelayStringIndex As Integer
End Class
