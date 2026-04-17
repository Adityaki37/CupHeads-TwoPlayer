Imports System
Imports UnityEngine

' Token: 0x02000750 RID: 1872
Public Class RetroArcadeBonusRobot
	Inherits RetroArcadeEnemy

	' Token: 0x060028D4 RID: 10452 RVA: 0x0017C70C File Offset: 0x0017AB0C
	Public Function Create(direction As RetroArcadeBonusRobot.Direction, properties As LevelProperties.RetroArcade.Robots) As RetroArcadeBonusRobot
		Dim retroArcadeBonusRobot As RetroArcadeBonusRobot = Me.InstantiatePrefab(Of RetroArcadeBonusRobot)()
		retroArcadeBonusRobot.transform.position = New Vector2(If((direction <> RetroArcadeBonusRobot.Direction.Left), (-400F), 400F), 250F)
		retroArcadeBonusRobot.properties = properties
		retroArcadeBonusRobot.direction = direction
		retroArcadeBonusRobot.hp = properties.bonusHp
		Return retroArcadeBonusRobot
	End Function

	' Token: 0x060028D5 RID: 10453 RVA: 0x0017C76A File Offset: 0x0017AB6A
	Protected Overrides Sub Start()
		MyBase.PointsWorth = Me.properties.pointsGained
		MyBase.PointsBonus = Me.properties.pointsBonus
	End Sub

	' Token: 0x060028D6 RID: 10454 RVA: 0x0017C790 File Offset: 0x0017AB90
	Protected Overrides Sub FixedUpdate()
		MyBase.transform.AddPosition(CSng(If((Me.direction <> RetroArcadeBonusRobot.Direction.Right), (-1), 1)) * Me.properties.bonusMoveSpeed * CupheadTime.FixedDelta, 0F, 0F)
		If If((Me.direction <> RetroArcadeBonusRobot.Direction.Left), (MyBase.transform.position.x > 400F), (MyBase.transform.position.x < -400F)) Then
			Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
		End If
	End Sub

	' Token: 0x040031AE RID: 12718
	Private Const SPAWN_X As Single = 400F

	' Token: 0x040031AF RID: 12719
	Private Const SPAWN_Y As Single = 250F

	' Token: 0x040031B0 RID: 12720
	Private properties As LevelProperties.RetroArcade.Robots

	' Token: 0x040031B1 RID: 12721
	Private direction As RetroArcadeBonusRobot.Direction

	' Token: 0x02000751 RID: 1873
	Public Enum Direction
		' Token: 0x040031B3 RID: 12723
		Left
		' Token: 0x040031B4 RID: 12724
		Right
	End Enum
End Class
