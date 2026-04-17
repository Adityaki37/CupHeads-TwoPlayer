Imports System
Imports UnityEngine

' Token: 0x02000764 RID: 1892
Public Class RetroArcadeUFOAlien
	Inherits RetroArcadeEnemy

	' Token: 0x170003E5 RID: 997
	' (get) Token: 0x0600293B RID: 10555 RVA: 0x00180870 File Offset: 0x0017EC70
	Public ReadOnly Property NormalizedHpRemaining As Single
		Get
			Return Me.hp / Me.properties.hp
		End Get
	End Property

	' Token: 0x0600293C RID: 10556 RVA: 0x00180884 File Offset: 0x0017EC84
	Public Function Create(parent As RetroArcadeUFO, properties As LevelProperties.RetroArcade.UFO) As RetroArcadeUFOAlien
		Dim retroArcadeUFOAlien As RetroArcadeUFOAlien = Me.InstantiatePrefab(Of RetroArcadeUFOAlien)()
		retroArcadeUFOAlien.properties = properties
		retroArcadeUFOAlien.parent = parent
		retroArcadeUFOAlien.hp = properties.hp
		retroArcadeUFOAlien.transform.parent = parent.transform
		retroArcadeUFOAlien.transform.position = parent.transform.position
		Me.cyclePositionIndex = Global.UnityEngine.Random.Range(0, retroArcadeUFOAlien.properties.cyclePositionX.Length)
		Return retroArcadeUFOAlien
	End Function

	' Token: 0x0600293D RID: 10557 RVA: 0x001808F4 File Offset: 0x0017ECF4
	Protected Overrides Sub Start()
		MyBase.transform.position = MyBase.transform.position + New Vector3(Me.properties.initialPositionX, CSng(Me.properties.alienYPosition), 0F) * Me.parent.transform.localScale.y
		MyBase.PointsWorth = Me.properties.pointsGained
		MyBase.PointsWorth = Me.properties.pointsBonus
	End Sub

	' Token: 0x0600293E RID: 10558 RVA: 0x0018097C File Offset: 0x0017ED7C
	Public Overrides Sub Dead()
		MyBase.Dead()
		Me.parent.OnAlienDie()
	End Sub

	' Token: 0x0600293F RID: 10559 RVA: 0x00180990 File Offset: 0x0017ED90
	Protected Overrides Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		MyBase.OnDamageTaken(info)
		MyBase.transform.position = New Vector3(Me.properties.cyclePositionX(Me.cyclePositionIndex), MyBase.transform.position.y, 0F)
		Me.cyclePositionIndex = (Me.cyclePositionIndex + 1) Mod Me.properties.cyclePositionX.Length
	End Sub

	' Token: 0x04003231 RID: 12849
	Private properties As LevelProperties.RetroArcade.UFO

	' Token: 0x04003232 RID: 12850
	Private parent As RetroArcadeUFO

	' Token: 0x04003233 RID: 12851
	Private cyclePositionIndex As Integer
End Class
