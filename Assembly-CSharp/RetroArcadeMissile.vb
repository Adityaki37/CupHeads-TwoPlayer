Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000745 RID: 1861
Public Class RetroArcadeMissile
	Inherits AbstractCollidableObject

	' Token: 0x06002889 RID: 10377 RVA: 0x0017A2CC File Offset: 0x001786CC
	Public Sub Init(pos As Vector2, rotation As Single, properties As LevelProperties.RetroArcade.Missile, pivot As Vector3)
		MyBase.transform.position = pos
		MyBase.transform.SetEulerAngles(New Single?(0F), New Single?(0F), New Single?(rotation))
		Me.properties = properties
		Me.pivotPoint = pivot
	End Sub

	' Token: 0x0600288A RID: 10378 RVA: 0x0017A31E File Offset: 0x0017871E
	Private Sub Start()
		Me.loopXSize = Me.properties.loopXSize
		Me.loopYSize = Me.properties.loopYSize
		Me.damageDealer = DamageDealer.NewEnemy()
		Me.Deactivate()
	End Sub

	' Token: 0x0600288B RID: 10379 RVA: 0x0017A353 File Offset: 0x00178753
	Private Sub Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x0600288C RID: 10380 RVA: 0x0017A36B File Offset: 0x0017876B
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x0600288D RID: 10381 RVA: 0x0017A389 File Offset: 0x00178789
	Public Sub StartCircle(onRight As Boolean, pivot As Vector3)
		Me.circleAngle = 0F
		Me.pivotPoint = pivot
		MyBase.GetComponent(Of SpriteRenderer)().enabled = True
		MyBase.GetComponent(Of Collider2D)().enabled = True
		MyBase.StartCoroutine(Me.move_in_circle_cr(onRight))
	End Sub

	' Token: 0x0600288E RID: 10382 RVA: 0x0017A3C4 File Offset: 0x001787C4
	Private Iterator Function move_in_circle_cr(onRight As Boolean) As IEnumerator
		Dim handleRotationX As Vector3 = Vector3.zero
		Dim rotateInCir As Single
		If onRight Then
			rotateInCir = -90F
		Else
			rotateInCir = 90F
		End If
		While Me.circleAngle < 6.108652F
			Me.circleAngle += 5F * CupheadTime.Delta
			If onRight Then
				handleRotationX = New Vector3(-Mathf.Sin(Me.circleAngle) * Me.loopXSize, 0F, 0F)
			Else
				handleRotationX = New Vector3(Mathf.Sin(Me.circleAngle) * Me.loopXSize, 0F, 0F)
			End If
			Dim handleRotationY As Vector3 = New Vector3(0F, Mathf.Cos(Me.circleAngle) * Me.loopYSize, 0F)
			MyBase.transform.position = Me.pivotPoint
			MyBase.transform.position += handleRotationX + handleRotationY
			Dim dir As Vector3 = Me.pivotPoint - MyBase.transform.position
			MyBase.transform.SetEulerAngles(New Single?(0F), New Single?(0F), New Single?(MathUtils.DirectionToAngle(dir) + rotateInCir))
			Yield Nothing
		End While
		Me.Deactivate()
		Yield Nothing
		Return
	End Function

	' Token: 0x0600288F RID: 10383 RVA: 0x0017A3E6 File Offset: 0x001787E6
	Private Sub Deactivate()
		MyBase.GetComponent(Of SpriteRenderer)().enabled = False
		MyBase.GetComponent(Of Collider2D)().enabled = False
	End Sub

	' Token: 0x0400315F RID: 12639
	Private properties As LevelProperties.RetroArcade.Missile

	' Token: 0x04003160 RID: 12640
	Private loopYSize As Single

	' Token: 0x04003161 RID: 12641
	Private loopXSize As Single

	' Token: 0x04003162 RID: 12642
	Private circleAngle As Single

	' Token: 0x04003163 RID: 12643
	Private pivotPoint As Vector3

	' Token: 0x04003164 RID: 12644
	Private damageDealer As DamageDealer
End Class
