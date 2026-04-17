Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports UnityEngine

' Token: 0x0200074F RID: 1871
Public Class RetroArcadeBigRobot
	Inherits RetroArcadeEnemy

	' Token: 0x060028CC RID: 10444 RVA: 0x0017BFDC File Offset: 0x0017A3DC
	Public Function Create(xPos As Single, properties As LevelProperties.RetroArcade.Robots, sinOffset As Single, manager As RetroArcadeRobotManager, orbiterPattern As String()) As RetroArcadeBigRobot
		Dim retroArcadeBigRobot As RetroArcadeBigRobot = Me.InstantiatePrefab(Of RetroArcadeBigRobot)()
		retroArcadeBigRobot.t = sinOffset * 3.1415927F * 2F
		Dim num As Single = Me.OFFSCREEN_Y + properties.smallRobotRotationDistance - properties.mainRobotY.min
		retroArcadeBigRobot.properties = properties
		retroArcadeBigRobot.transform.position = New Vector2(xPos, retroArcadeBigRobot.getYPos(retroArcadeBigRobot.t) + num)
		retroArcadeBigRobot.hp = properties.mainRobotHp
		retroArcadeBigRobot.manager = manager
		Dim num2 As Single = sinOffset * 360F
		retroArcadeBigRobot.orbiters = New RetroArcadeOrbiterRobot(2) {}
		For i As Integer = 0 To 3 - 1
			Dim num3 As Integer
			If Parser.IntTryParse(orbiterPattern(i), num3) AndAlso num3 > 0 AndAlso num3 <= Me.orbiterPrefabs.Length Then
				retroArcadeBigRobot.orbiters(i) = retroArcadeBigRobot.orbiterPrefabs(num3 - 1).Create(retroArcadeBigRobot, properties, num2)
				num2 += 120F
			End If
		Next
		retroArcadeBigRobot.MoveY(-num, properties.mainRobotMoveSpeed)
		retroArcadeBigRobot.StartCoroutine(retroArcadeBigRobot.shoot_cr())
		retroArcadeBigRobot.StartCoroutine(retroArcadeBigRobot.orbiterShoot_cr())
		Return retroArcadeBigRobot
	End Function

	' Token: 0x060028CD RID: 10445 RVA: 0x0017C0F5 File Offset: 0x0017A4F5
	Protected Overrides Sub Start()
		MyBase.PointsWorth = Me.properties.pointsGained
		MyBase.PointsBonus = Me.properties.pointsBonus
	End Sub

	' Token: 0x060028CE RID: 10446 RVA: 0x0017C11C File Offset: 0x0017A51C
	Protected Overrides Sub FixedUpdate()
		If Me.movingY OrElse Me.groupDead Then
			Return
		End If
		Me.t += CupheadTime.FixedDelta * (Me.properties.mainRobotMoveSpeed / (Me.properties.mainRobotY.max - Me.properties.mainRobotY.min)) * 3.1415927F
		MyBase.transform.SetPosition(Nothing, New Single?(Me.getYPos(Me.t)), Nothing)
		Dim flag As Boolean = True
		For Each retroArcadeOrbiterRobot As RetroArcadeOrbiterRobot In Me.orbiters
			If Not retroArcadeOrbiterRobot.IsDead Then
				flag = False
			End If
		Next
		If flag Then
			MyBase.StartCoroutine(Me.moveOffscreen_cr())
			Me.groupDead = True
			Me.manager.OnRobotGroupDie()
		End If
	End Sub

	' Token: 0x060028CF RID: 10447 RVA: 0x0017C20C File Offset: 0x0017A60C
	Private Function getYPos(t As Single) As Single
		Return Me.properties.mainRobotY.GetFloatAt(Mathf.Sin(t) * 0.5F + 0.5F)
	End Function

	' Token: 0x060028D0 RID: 10448 RVA: 0x0017C230 File Offset: 0x0017A630
	Private Iterator Function moveOffscreen_cr() As IEnumerator
		MyBase.MoveY(Me.OFFSCREEN_Y + Me.properties.smallRobotRotationDistance - MyBase.transform.position.y, 500F)
		While Me.movingY
			Yield Nothing
		End While
		For Each retroArcadeOrbiterRobot As RetroArcadeOrbiterRobot In Me.orbiters
			Global.UnityEngine.[Object].Destroy(retroArcadeOrbiterRobot.gameObject)
		Next
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
		Return
	End Function

	' Token: 0x060028D1 RID: 10449 RVA: 0x0017C24C File Offset: 0x0017A64C
	Private Iterator Function shoot_cr() As IEnumerator
		While Me.movingY
			Yield Nothing
		End While
		Dim pattern As String() = Me.properties.mainRobotShootString.Split(New Char() { ","c })
		Dim currentIndex As Integer = Global.UnityEngine.Random.Range(0, pattern.Length)
		While Not MyBase.IsDead
			Dim waitTime As Single = 0F
			Parser.FloatTryParse(pattern(currentIndex), waitTime)
			Yield CupheadTime.WaitForSeconds(Me, waitTime)
			If MyBase.IsDead Then
				Exit While
			End If
			Dim shootAngle As Single = MathUtils.DirectionToAngle(PlayerManager.GetNext().center - Me.projectileRoot.position)
			Me.projectilePrefab.Create(Me.projectileRoot.position, Me.properties.mainRobotShootSpeed, shootAngle, Me.properties.mainRobotShotBounce)
		End While
		Return
	End Function

	' Token: 0x060028D2 RID: 10450 RVA: 0x0017C268 File Offset: 0x0017A668
	Private Iterator Function orbiterShoot_cr() As IEnumerator
		While Me.movingY
			Yield Nothing
		End While
		While True
			Yield CupheadTime.WaitForSeconds(Me, Me.properties.smallRobotAttackDelay.RandomFloat())
			Dim aliveOrbiters As List(Of RetroArcadeOrbiterRobot) = New List(Of RetroArcadeOrbiterRobot)()
			For Each retroArcadeOrbiterRobot As RetroArcadeOrbiterRobot In Me.orbiters
				If Not retroArcadeOrbiterRobot.IsDead Then
					aliveOrbiters.Add(retroArcadeOrbiterRobot)
				End If
			Next
			If aliveOrbiters.Count = 0 Then
				Exit For
			End If
			aliveOrbiters.RandomChoice().Shoot()
		End While
		Return
	End Function

	' Token: 0x040031A4 RID: 12708
	<SerializeField()>
	Private orbiterPrefabs As RetroArcadeOrbiterRobot()

	' Token: 0x040031A5 RID: 12709
	<SerializeField()>
	Private projectilePrefab As RetroArcadeRobotBouncingProjectile

	' Token: 0x040031A6 RID: 12710
	<SerializeField()>
	Private projectileRoot As Transform

	' Token: 0x040031A7 RID: 12711
	Private OFFSCREEN_Y As Single = 300F

	' Token: 0x040031A8 RID: 12712
	Private Const MOVE_OFFSCREEN_SPEED As Single = 500F

	' Token: 0x040031A9 RID: 12713
	Private properties As LevelProperties.RetroArcade.Robots

	' Token: 0x040031AA RID: 12714
	Private t As Single

	' Token: 0x040031AB RID: 12715
	Private orbiters As RetroArcadeOrbiterRobot()

	' Token: 0x040031AC RID: 12716
	Private manager As RetroArcadeRobotManager

	' Token: 0x040031AD RID: 12717
	Private groupDead As Boolean
End Class
