Imports System
Imports UnityEngine

' Token: 0x02000767 RID: 1895
Public Class RetroArcadeUFOTurret
	Inherits AbstractCollidableObject

	' Token: 0x06002946 RID: 10566 RVA: 0x00181134 File Offset: 0x0017F534
	Public Function Create(parent As RetroArcadeUFO, properties As LevelProperties.RetroArcade.UFO, t As Single) As RetroArcadeUFOTurret
		Dim retroArcadeUFOTurret As RetroArcadeUFOTurret = Me.InstantiatePrefab(Of RetroArcadeUFOTurret)()
		retroArcadeUFOTurret.properties = properties
		retroArcadeUFOTurret.parent = parent
		retroArcadeUFOTurret.t = t
		retroArcadeUFOTurret.transform.parent = parent.transform
		retroArcadeUFOTurret.transform.position = parent.transform.position
		Return retroArcadeUFOTurret
	End Function

	' Token: 0x06002947 RID: 10567 RVA: 0x00181188 File Offset: 0x0017F588
	Private Sub FixedUpdate()
		Me.t += CupheadTime.FixedDelta * (Me.properties.projectileSpeed / 600F)
		Dim num As Single = Me.t Mod 1F * 3.1415927F
		Dim vector As Vector2 = New Vector2(Mathf.Cos(num) * 600F / 2F, -Mathf.Sin(num) * 300F / 2F)
		MyBase.transform.SetPosition(New Single?(Me.parent.transform.position.x + vector.x), New Single?(Me.parent.transform.position.y + vector.y), Nothing)
		Dim num2 As Single = MathUtils.DirectionToAngle(New Vector2(Mathf.Cos(num) * 300F / 2F, -Mathf.Sin(num) * 600F / 2F)) + -90F
		MyBase.transform.SetEulerAngles(New Single?(0F), New Single?(0F), New Single?(num2))
	End Sub

	' Token: 0x06002948 RID: 10568 RVA: 0x001812B4 File Offset: 0x0017F6B4
	Public Sub Shoot()
		Me.projectilePrefab.Create(Me.projectileRoot.position, MyBase.transform.eulerAngles.z - -90F, Me.properties.projectileSpeed)
	End Sub

	' Token: 0x04003240 RID: 12864
	Private Const ANGLE_OFFSET As Single = -90F

	' Token: 0x04003241 RID: 12865
	<SerializeField()>
	Private projectilePrefab As BasicProjectile

	' Token: 0x04003242 RID: 12866
	<SerializeField()>
	Private projectileRoot As Transform

	' Token: 0x04003243 RID: 12867
	Private properties As LevelProperties.RetroArcade.UFO

	' Token: 0x04003244 RID: 12868
	Private parent As RetroArcadeUFO

	' Token: 0x04003245 RID: 12869
	Private t As Single
End Class
