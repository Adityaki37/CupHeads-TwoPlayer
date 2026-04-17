Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000596 RID: 1430
Public Class DevilLevelSkull
	Inherits AbstractProjectile

	' Token: 0x06001B67 RID: 7015 RVA: 0x000FACA8 File Offset: 0x000F90A8
	Public Function Create(pos As Vector2, properties As LevelProperties.Devil.SkullEye) As DevilLevelSkull
		Dim devilLevelSkull As DevilLevelSkull = Me.InstantiatePrefab(Of DevilLevelSkull)()
		devilLevelSkull.properties = properties
		devilLevelSkull.transform.position = pos
		devilLevelSkull.StartCoroutine(devilLevelSkull.main_cr())
		Return devilLevelSkull
	End Function

	' Token: 0x06001B68 RID: 7016 RVA: 0x000FACE2 File Offset: 0x000F90E2
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
		MyBase.OnCollisionPlayer(hit, phase)
	End Sub

	' Token: 0x06001B69 RID: 7017 RVA: 0x000FAD00 File Offset: 0x000F9100
	Protected Overrides Sub FixedUpdate()
		MyBase.FixedUpdate()
		If Not CupheadLevelCamera.Current.ContainsPoint(MyBase.transform.position, New Vector2(1000F, 100F)) Then
			Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
		End If
	End Sub

	' Token: 0x06001B6A RID: 7018 RVA: 0x000FAD4C File Offset: 0x000F914C
	Private Iterator Function main_cr() As IEnumerator
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Start", False, True)
		Dim player As AbstractPlayerController = PlayerManager.GetNext()
		Dim moveDir As Vector2 = (player.transform.position - MyBase.transform.position).normalized
		Dim velocity As Vector2 = moveDir * Me.properties.initialMoveSpeed
		Dim rotation As Single = MathUtils.DirectionToAngle(player.transform.position - MyBase.transform.position)
		Dim t As Single = 0F
		While t < Me.properties.initialMoveDuration
			t += CupheadTime.FixedDelta
			MyBase.transform.AddPosition(velocity.x * CupheadTime.FixedDelta, velocity.y * CupheadTime.FixedDelta, 0F)
			Yield New WaitForFixedUpdate()
		End While
		Dim rotationSpeed As Single = CSng(Rand.PosOrNeg()) * Me.properties.swirlRotationSpeed
		t = 0F
		Dim spiralOrigin As Vector2 = MyBase.transform.position
		While True
			t += CupheadTime.FixedDelta
			rotation += rotationSpeed * CupheadTime.FixedDelta
			MyBase.transform.position = spiralOrigin + MathUtils.AngleToDirection(rotation) * Me.properties.swirlMoveOutwardSpeed * t
			Yield New WaitForFixedUpdate()
		End While
		Return
	End Function

	' Token: 0x040024A2 RID: 9378
	Private properties As LevelProperties.Devil.SkullEye
End Class
