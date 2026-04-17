Imports System
Imports UnityEngine

' Token: 0x02000735 RID: 1845
Public Class RetroArcadeAlien
	Inherits RetroArcadeEnemy

	' Token: 0x170003D5 RID: 981
	' (get) Token: 0x0600282D RID: 10285 RVA: 0x00176F96 File Offset: 0x00175396
	' (set) Token: 0x0600282E RID: 10286 RVA: 0x00176F9E File Offset: 0x0017539E
	Public Property ColumnIndex As Integer

	' Token: 0x0600282F RID: 10287 RVA: 0x00176FA8 File Offset: 0x001753A8
	Public Function Create(position As Vector2, columnIndex As Integer, manager As RetroArcadeAlienManager, properties As LevelProperties.RetroArcade.Aliens) As RetroArcadeAlien
		Dim retroArcadeAlien As RetroArcadeAlien = Me.InstantiatePrefab(Of RetroArcadeAlien)()
		retroArcadeAlien.transform.position = position
		retroArcadeAlien.properties = properties
		retroArcadeAlien.manager = manager
		retroArcadeAlien.hp = properties.hp
		retroArcadeAlien.ColumnIndex = columnIndex
		Return retroArcadeAlien
	End Function

	' Token: 0x06002830 RID: 10288 RVA: 0x00176FF1 File Offset: 0x001753F1
	Protected Overrides Sub Start()
		MyBase.PointsWorth = Me.properties.pointsGained
		MyBase.PointsBonus = Me.properties.pointsBonus
	End Sub

	' Token: 0x06002831 RID: 10289 RVA: 0x00177018 File Offset: 0x00175418
	Protected Overrides Sub FixedUpdate()
		If Me.movingY Then
			Return
		End If
		MyBase.transform.AddPosition(CSng(If((Me.manager.direction <> RetroArcadeAlien.Direction.Right), (-1), 1)) * Me.manager.moveSpeed * CupheadTime.FixedDelta, 0F, 0F)
	End Sub

	' Token: 0x06002832 RID: 10290 RVA: 0x00177071 File Offset: 0x00175471
	Public Sub MoveY(moveAmount As Single)
		MyBase.MoveY(moveAmount, 500F)
	End Sub

	' Token: 0x06002833 RID: 10291 RVA: 0x0017707F File Offset: 0x0017547F
	Public Overrides Sub Dead()
		MyBase.Dead()
		Me.manager.OnAlienDie(Me)
	End Sub

	' Token: 0x06002834 RID: 10292 RVA: 0x00177093 File Offset: 0x00175493
	Public Sub Shoot()
		Me.bulletPrefab.Create(Me.bulletRoot.position, -90F, Me.properties.bulletSpeed)
	End Sub

	' Token: 0x040030F5 RID: 12533
	Private Const MOVE_Y_SPEED As Single = 500F

	' Token: 0x040030F6 RID: 12534
	<SerializeField()>
	Private bulletPrefab As BasicProjectile

	' Token: 0x040030F7 RID: 12535
	<SerializeField()>
	Private bulletRoot As Transform

	' Token: 0x040030F8 RID: 12536
	Private properties As LevelProperties.RetroArcade.Aliens

	' Token: 0x040030F9 RID: 12537
	Private manager As RetroArcadeAlienManager

	' Token: 0x02000736 RID: 1846
	Public Enum Direction
		' Token: 0x040030FC RID: 12540
		Left
		' Token: 0x040030FD RID: 12541
		Right
	End Enum
End Class
