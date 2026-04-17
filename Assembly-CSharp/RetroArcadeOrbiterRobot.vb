Imports System
Imports UnityEngine

' Token: 0x02000752 RID: 1874
Public Class RetroArcadeOrbiterRobot
	Inherits RetroArcadeEnemy

	' Token: 0x060028D8 RID: 10456 RVA: 0x0017C834 File Offset: 0x0017AC34
	Public Function Create(parent As RetroArcadeBigRobot, properties As LevelProperties.RetroArcade.Robots, angle As Single) As RetroArcadeOrbiterRobot
		Dim retroArcadeOrbiterRobot As RetroArcadeOrbiterRobot = Me.InstantiatePrefab(Of RetroArcadeOrbiterRobot)()
		retroArcadeOrbiterRobot.transform.position = parent.transform.position + properties.smallRobotRotationDistance * MathUtils.AngleToDirection(angle)
		retroArcadeOrbiterRobot.properties = properties
		retroArcadeOrbiterRobot.parent = parent
		retroArcadeOrbiterRobot.angle = angle
		retroArcadeOrbiterRobot.hp = properties.smallRobotHp
		Return retroArcadeOrbiterRobot
	End Function

	' Token: 0x060028D9 RID: 10457 RVA: 0x0017C8A0 File Offset: 0x0017ACA0
	Protected Overrides Sub Start()
		MyBase.PointsWorth = Me.properties.pointsGained
		MyBase.PointsBonus = Me.properties.pointsBonus
	End Sub

	' Token: 0x060028DA RID: 10458 RVA: 0x0017C8C4 File Offset: 0x0017ACC4
	Protected Overrides Sub FixedUpdate()
		Me.angle += CupheadTime.FixedDelta * Me.properties.smallRobotRotationSpeed
		MyBase.transform.position = Me.parent.transform.position + MathUtils.AngleToDirection(Me.angle) * Me.properties.smallRobotRotationDistance
	End Sub

	' Token: 0x060028DB RID: 10459 RVA: 0x0017C934 File Offset: 0x0017AD34
	Public Sub Shoot()
		Me.projectilePrefab.Create(Me.projectileRoot.position, -90F, Me.properties.smallRobotShootSpeed)
	End Sub

	' Token: 0x040031B5 RID: 12725
	<SerializeField()>
	Private projectilePrefab As BasicProjectile

	' Token: 0x040031B6 RID: 12726
	<SerializeField()>
	Private projectileRoot As Transform

	' Token: 0x040031B7 RID: 12727
	Private properties As LevelProperties.RetroArcade.Robots

	' Token: 0x040031B8 RID: 12728
	Private angle As Single

	' Token: 0x040031B9 RID: 12729
	Private parent As RetroArcadeBigRobot
End Class
