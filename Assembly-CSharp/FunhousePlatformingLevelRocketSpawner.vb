Imports System
Imports UnityEngine

' Token: 0x020008BD RID: 2237
Public Class FunhousePlatformingLevelRocketSpawner
	Inherits PlatformingLevelGroundMovementEnemySpawner

	' Token: 0x06003435 RID: 13365 RVA: 0x001E49F4 File Offset: 0x001E2DF4
	Protected Overrides Sub Start()
		MyBase.Start()
		Me.topBottomIndex = Global.UnityEngine.Random.Range(0, Me.topBottomString.Split(New Char() { ","c }).Length)
		Me.directionPattern = Me.patternString.Split(New Char() { ","c })
		Me.directionIndex = Global.UnityEngine.Random.Range(0, Me.directionPattern.Length)
	End Sub

	' Token: 0x06003436 RID: 13366 RVA: 0x001E4A5C File Offset: 0x001E2E5C
	Protected Overrides Sub Spawn()
		If Me.topBottomString.Split(New Char() { ","c })(Me.topBottomIndex)(0) = "T"c Then
			Me.isTop = True
		ElseIf Me.topBottomString.Split(New Char() { ","c })(Me.topBottomIndex)(0) = "B"c Then
			Me.isTop = False
		End If
		Dim flag As Boolean = If((Not Me.chooseSideRandomly), (Me.directionPattern(Me.directionIndex) = "R"), Rand.Bool())
		Me.directionIndex = (Me.directionIndex + 1) Mod Me.directionPattern.Length
		Dim num As Single = If((Not flag), (CupheadLevelCamera.Current.Bounds.xMin - 50F), (CupheadLevelCamera.Current.Bounds.xMax + 50F))
		Dim y As Single = CupheadLevelCamera.Current.transform.position.y
		Dim platformingLevelGroundMovementEnemy As PlatformingLevelGroundMovementEnemy = Me.enemyPrefab.Spawn()
		platformingLevelGroundMovementEnemy.GetComponent(Of FunhousePlatformingLevelRocket)().Init(New Vector2(num, y), Me.isTop, flag)
		platformingLevelGroundMovementEnemy.GoToGround(True, "Idle")
		Me.topBottomIndex = (Me.topBottomIndex + 1) Mod Me.topBottomString.Split(New Char() { ","c }).Length
	End Sub

	' Token: 0x04003C6F RID: 15471
	Private Const Right As String = "R"

	' Token: 0x04003C70 RID: 15472
	<SerializeField()>
	Private topBottomString As String

	' Token: 0x04003C71 RID: 15473
	Private topBottomIndex As Integer

	' Token: 0x04003C72 RID: 15474
	Private isTop As Boolean

	' Token: 0x04003C73 RID: 15475
	Private directionPattern As String()

	' Token: 0x04003C74 RID: 15476
	Private directionIndex As Integer
End Class
