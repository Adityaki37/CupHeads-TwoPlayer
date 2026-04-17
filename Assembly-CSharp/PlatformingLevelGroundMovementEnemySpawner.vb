Imports System
Imports UnityEngine

' Token: 0x02000868 RID: 2152
Public Class PlatformingLevelGroundMovementEnemySpawner
	Inherits PlatformingLevelEnemySpawner

	' Token: 0x06003204 RID: 12804 RVA: 0x001D32F5 File Offset: 0x001D16F5
	Protected Overrides Sub Awake()
		MyBase.Awake()
		If Not Me.chooseSideRandomly Then
			Me.patternIndex = Global.UnityEngine.Random.Range(0, Me.patternString.Length)
		End If
	End Sub

	' Token: 0x06003205 RID: 12805 RVA: 0x001D3320 File Offset: 0x001D1720
	Protected Overrides Sub Spawn()
		Dim direction As PlatformingLevelGroundMovementEnemy.Direction
		If Me.chooseSideRandomly Then
			direction = If((Not MathUtils.RandomBool()), PlatformingLevelGroundMovementEnemy.Direction.Right, PlatformingLevelGroundMovementEnemy.Direction.Left)
		Else
			Me.patternIndex = (Me.patternIndex + 1) Mod Me.patternString.Length
			direction = If((Me.patternString(Me.patternIndex) <> "L"c), PlatformingLevelGroundMovementEnemy.Direction.Right, PlatformingLevelGroundMovementEnemy.Direction.Left)
		End If
		Dim vector As Vector2 = New Vector2(If((direction <> PlatformingLevelGroundMovementEnemy.Direction.Left), (CupheadLevelCamera.Current.Bounds.xMin - 50F), (CupheadLevelCamera.Current.Bounds.xMax + 50F)), CupheadLevelCamera.Current.Bounds.yMax)
		Dim platformingLevelGroundMovementEnemy As PlatformingLevelGroundMovementEnemy = Me.enemyPrefab.Spawn(vector, direction, Me.destroyEnemyAfterLeavingScreen)
		platformingLevelGroundMovementEnemy.GoToGround(True, "Run")
	End Sub

	' Token: 0x04003A69 RID: 14953
	Public enemyPrefab As PlatformingLevelGroundMovementEnemy

	' Token: 0x04003A6A RID: 14954
	Public chooseSideRandomly As Boolean = True

	' Token: 0x04003A6B RID: 14955
	Public patternString As String = "LR"

	' Token: 0x04003A6C RID: 14956
	Private patternIndex As Integer
End Class
