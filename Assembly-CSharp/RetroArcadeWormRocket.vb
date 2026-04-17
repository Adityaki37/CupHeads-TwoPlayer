Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x0200076B RID: 1899
Public Class RetroArcadeWormRocket
	Inherits RetroArcadeEnemy

	' Token: 0x06002954 RID: 10580 RVA: 0x00181980 File Offset: 0x0017FD80
	Public Function Create(direction As RetroArcadeWormRocket.Direction, properties As LevelProperties.RetroArcade.Worm) As RetroArcadeWormRocket
		Dim retroArcadeWormRocket As RetroArcadeWormRocket = Me.InstantiatePrefab(Of RetroArcadeWormRocket)()
		retroArcadeWormRocket.transform.position = New Vector2(If((direction <> RetroArcadeWormRocket.Direction.Left), (-330F), 330F), 330F)
		retroArcadeWormRocket.properties = properties
		retroArcadeWormRocket.direction = direction
		retroArcadeWormRocket.hp = 1F
		retroArcadeWormRocket.StartCoroutine(retroArcadeWormRocket.main_cr())
		Return retroArcadeWormRocket
	End Function

	' Token: 0x06002955 RID: 10581 RVA: 0x001819EA File Offset: 0x0017FDEA
	Protected Overrides Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		Me.brokenPiecePrefab.Create(Me.brokenPieceRoot.position, -90F, Me.properties.rocketBrokenPieceSpeed)
	End Sub

	' Token: 0x06002956 RID: 10582 RVA: 0x00181A18 File Offset: 0x0017FE18
	Private Iterator Function main_cr() As IEnumerator
		MyBase.MoveY(-60F, 500F)
		While Me.movingY
			Yield Nothing
		End While
		While (Me.direction = RetroArcadeWormRocket.Direction.Left AndAlso MyBase.transform.position.x > -330F) OrElse (Me.direction = RetroArcadeWormRocket.Direction.Right AndAlso MyBase.transform.position.x < 330F)
			MyBase.transform.AddPosition(CSng(If((Me.direction <> RetroArcadeWormRocket.Direction.Right), (-1), 1)) * Me.properties.rocketSpeed * CupheadTime.FixedDelta, 0F, 0F)
			Yield New WaitForFixedUpdate()
		End While
		MyBase.MoveY(60F, 500F)
		While Me.movingY
			Yield Nothing
		End While
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
		Return
	End Function

	' Token: 0x04003255 RID: 12885
	Private Const SPAWN_X As Single = 330F

	' Token: 0x04003256 RID: 12886
	Private Const OFFSCREEN_Y As Single = 330F

	' Token: 0x04003257 RID: 12887
	Private Const BASE_Y As Single = 270F

	' Token: 0x04003258 RID: 12888
	Private Const MOVE_Y_SPEED As Single = 500F

	' Token: 0x04003259 RID: 12889
	Private direction As RetroArcadeWormRocket.Direction

	' Token: 0x0400325A RID: 12890
	Private properties As LevelProperties.RetroArcade.Worm

	' Token: 0x0400325B RID: 12891
	<SerializeField()>
	Private brokenPiecePrefab As BasicProjectile

	' Token: 0x0400325C RID: 12892
	<SerializeField()>
	Private brokenPieceRoot As Transform

	' Token: 0x0200076C RID: 1900
	Public Enum Direction
		' Token: 0x0400325E RID: 12894
		Left
		' Token: 0x0400325F RID: 12895
		Right
	End Enum
End Class
