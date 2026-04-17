Imports System

' Token: 0x02000749 RID: 1865
Public Class RetroArcadeQShipOrbitingTile
	Inherits AbstractCollidableObject

	' Token: 0x060028A9 RID: 10409 RVA: 0x0017B974 File Offset: 0x00179D74
	Public Function Create(parent As RetroArcadeQShip, angle As Single, properties As LevelProperties.RetroArcade.QShip) As RetroArcadeQShipOrbitingTile
		Dim retroArcadeQShipOrbitingTile As RetroArcadeQShipOrbitingTile = Me.InstantiatePrefab(Of RetroArcadeQShipOrbitingTile)()
		retroArcadeQShipOrbitingTile.transform.position = parent.transform.position + properties.tileRotationDistance * MathUtils.AngleToDirection(angle)
		retroArcadeQShipOrbitingTile.properties = properties
		retroArcadeQShipOrbitingTile.transform.parent = parent.transform
		retroArcadeQShipOrbitingTile.parent = parent
		retroArcadeQShipOrbitingTile.angle = angle
		Dim component As DamageReceiver = retroArcadeQShipOrbitingTile.GetComponent(Of DamageReceiver)()
		AddHandler component.OnDamageTaken, AddressOf retroArcadeQShipOrbitingTile.OnDamageTaken
		Return retroArcadeQShipOrbitingTile
	End Function

	' Token: 0x060028AA RID: 10410 RVA: 0x0017BA00 File Offset: 0x00179E00
	Private Sub FixedUpdate()
		Me.angle += CupheadTime.FixedDelta * Me.parent.TileRotationSpeed
		MyBase.transform.position = Me.parent.transform.position + MathUtils.AngleToDirection(Me.angle) * Me.properties.tileRotationDistance
		MyBase.transform.SetEulerAngles(New Single?(0F), New Single?(0F), New Single?(Me.angle))
	End Sub

	' Token: 0x060028AB RID: 10411 RVA: 0x0017BA9A File Offset: 0x00179E9A
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		Me.parent.ShootProjectile()
	End Sub

	' Token: 0x04003180 RID: 12672
	Private angle As Single

	' Token: 0x04003181 RID: 12673
	Private parent As RetroArcadeQShip

	' Token: 0x04003182 RID: 12674
	Private properties As LevelProperties.RetroArcade.QShip
End Class
