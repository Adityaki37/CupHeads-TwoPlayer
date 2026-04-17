Imports System
Imports UnityEngine

' Token: 0x0200087A RID: 2170
Public Class ForestPlatformingLevelAcornSpawner
	Inherits PlatformingLevelEnemySpawner

	' Token: 0x06003266 RID: 12902 RVA: 0x001D58B0 File Offset: 0x001D3CB0
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.leftRightIndex = Global.UnityEngine.Random.Range(0, Me.leftRightString.Length)
		Me.yPattern = Me.yString.Split(New Char() { ","c })
		Me.yIndex = Global.UnityEngine.Random.Range(0, Me.yPattern.Length)
	End Sub

	' Token: 0x06003267 RID: 12903 RVA: 0x001D590C File Offset: 0x001D3D0C
	Protected Overrides Sub Spawn()
		Me.leftRightIndex = (Me.leftRightIndex + 1) Mod Me.leftRightString.Length
		Dim direction As ForestPlatformingLevelAcorn.Direction = If((Me.leftRightString(Me.leftRightIndex) <> "L"c), ForestPlatformingLevelAcorn.Direction.Right, ForestPlatformingLevelAcorn.Direction.Left)
		Me.yIndex = (Me.yIndex + 1) Mod Me.yPattern.Length
		Dim num As Single = 0F
		Parser.FloatTryParse(Me.yPattern(Me.yIndex), num)
		Dim vector As Vector2 = New Vector2(If((direction <> ForestPlatformingLevelAcorn.Direction.Left), (CupheadLevelCamera.Current.Bounds.xMin - 50F), (CupheadLevelCamera.Current.Bounds.xMax + 50F)), CupheadLevelCamera.Current.Bounds.yMax - num)
		Me.enemyPrefab.Spawn(vector, direction, False)
	End Sub

	' Token: 0x06003268 RID: 12904 RVA: 0x001D59EC File Offset: 0x001D3DEC
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me.enemyPrefab = Nothing
	End Sub

	' Token: 0x04003AC5 RID: 15045
	Public enemyPrefab As ForestPlatformingLevelAcorn

	' Token: 0x04003AC6 RID: 15046
	Public leftRightString As String = "LR"

	' Token: 0x04003AC7 RID: 15047
	Public yString As String = "150,50"

	' Token: 0x04003AC8 RID: 15048
	Private leftRightIndex As Integer

	' Token: 0x04003AC9 RID: 15049
	Private yIndex As Integer

	' Token: 0x04003ACA RID: 15050
	Private yPattern As String()
End Class
