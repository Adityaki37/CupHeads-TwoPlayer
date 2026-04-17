Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x0200066E RID: 1646
Public Class FlyingGenieLevelHelixProjectile
	Inherits AbstractProjectile

	' Token: 0x0600229D RID: 8861 RVA: 0x00145378 File Offset: 0x00143778
	Public Function Create(pos As Vector3, properties As LevelProperties.FlyingGenie.Coffin, topOne As Boolean) As FlyingGenieLevelHelixProjectile
		Dim flyingGenieLevelHelixProjectile As FlyingGenieLevelHelixProjectile = TryCast(MyBase.Create(), FlyingGenieLevelHelixProjectile)
		flyingGenieLevelHelixProjectile.properties = properties
		flyingGenieLevelHelixProjectile.transform.position = pos
		flyingGenieLevelHelixProjectile.topOne = topOne
		Return flyingGenieLevelHelixProjectile
	End Function

	' Token: 0x0600229E RID: 8862 RVA: 0x001453AC File Offset: 0x001437AC
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		If Me.damageDealer IsNot Nothing AndAlso phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
		MyBase.OnCollisionPlayer(hit, phase)
	End Sub

	' Token: 0x0600229F RID: 8863 RVA: 0x001453D5 File Offset: 0x001437D5
	Protected Overrides Sub Update()
		MyBase.Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x060022A0 RID: 8864 RVA: 0x001453F3 File Offset: 0x001437F3
	Protected Overrides Sub Start()
		MyBase.Start()
		MyBase.animator.SetBool("OnTop", Me.topOne)
		MyBase.StartCoroutine(Me.moveY_cr())
	End Sub

	' Token: 0x060022A1 RID: 8865 RVA: 0x00145420 File Offset: 0x00143820
	Private Iterator Function moveY_cr() As IEnumerator
		Dim angle As Single = 0F
		Dim xSpeed As Single = Me.properties.heartShotXSpeed
		Dim ySpeed As Single = Me.properties.heartShotYSpeed
		Dim moveX As Vector3 = MyBase.transform.position
		While MyBase.transform.position.x <> -640F
			Dim loopSize As Single
			If Me.topOne Then
				loopSize = Me.properties.heartLoopYSize
				ySpeed = Me.properties.heartShotYSpeed
			Else
				loopSize = -Me.properties.heartLoopYSize
				ySpeed = -Me.properties.heartShotYSpeed
			End If
			angle += ySpeed * CupheadTime.Delta
			Dim moveY As Vector3 = New Vector3(0F, Mathf.Sin(angle + Me.properties.heartLoopYSize) * CupheadTime.Delta * 60F * loopSize / 2F)
			moveX = -MyBase.transform.right * xSpeed * CupheadTime.Delta
			MyBase.transform.position += moveX + moveY
			Yield Nothing
		End While
		Me.Die()
		Yield Nothing
		Return
	End Function

	' Token: 0x04002B48 RID: 11080
	Private properties As LevelProperties.FlyingGenie.Coffin

	' Token: 0x04002B49 RID: 11081
	Private topOne As Boolean
End Class
