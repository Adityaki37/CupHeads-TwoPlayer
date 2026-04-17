Imports System
Imports UnityEngine

' Token: 0x02000738 RID: 1848
Public Class RetroArcadeBonusAlien
	Inherits RetroArcadeEnemy

	' Token: 0x06002848 RID: 10312 RVA: 0x00177D90 File Offset: 0x00176190
	Public Function Create(direction As RetroArcadeBonusAlien.Direction, properties As LevelProperties.RetroArcade.Aliens) As RetroArcadeBonusAlien
		Dim retroArcadeBonusAlien As RetroArcadeBonusAlien = Me.InstantiatePrefab(Of RetroArcadeBonusAlien)()
		retroArcadeBonusAlien.transform.position = New Vector2(If((direction <> RetroArcadeBonusAlien.Direction.Left), (-400F), 400F), 270F)
		retroArcadeBonusAlien.properties = properties
		retroArcadeBonusAlien.direction = direction
		retroArcadeBonusAlien.hp = 1F
		Return retroArcadeBonusAlien
	End Function

	' Token: 0x06002849 RID: 10313 RVA: 0x00177DF0 File Offset: 0x001761F0
	Protected Overrides Sub FixedUpdate()
		MyBase.transform.AddPosition(CSng(If((Me.direction <> RetroArcadeBonusAlien.Direction.Right), (-1), 1)) * Me.properties.bonusMoveSpeed * CupheadTime.FixedDelta, 0F, 0F)
		If If((Me.direction <> RetroArcadeBonusAlien.Direction.Left), (MyBase.transform.position.x > 400F), (MyBase.transform.position.x < -400F)) Then
			Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
		End If
	End Sub

	' Token: 0x0400310D RID: 12557
	Private Const SPAWN_X As Single = 400F

	' Token: 0x0400310E RID: 12558
	Private Const SPAWN_Y As Single = 270F

	' Token: 0x0400310F RID: 12559
	Private properties As LevelProperties.RetroArcade.Aliens

	' Token: 0x04003110 RID: 12560
	Private direction As RetroArcadeBonusAlien.Direction

	' Token: 0x02000739 RID: 1849
	Public Enum Direction
		' Token: 0x04003112 RID: 12562
		Left
		' Token: 0x04003113 RID: 12563
		Right
	End Enum
End Class
