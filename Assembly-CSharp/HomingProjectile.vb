Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000AF6 RID: 2806
Public Class HomingProjectile
	Inherits AbstractProjectile

	' Token: 0x17000615 RID: 1557
	' (get) Token: 0x06004404 RID: 17412 RVA: 0x000B6008 File Offset: 0x000B4408
	' (set) Token: 0x06004405 RID: 17413 RVA: 0x000B6010 File Offset: 0x000B4410
	Public Property HomingEnabled As Boolean

	' Token: 0x06004406 RID: 17414 RVA: 0x000B601C File Offset: 0x000B441C
	Public Function Create(pos As Vector2, launchRotation As Single, launchSpeed As Single, homingMoveSpeed As Single, rotationSpeed As Single, timeBeforeDeath As Single, homingEaseTime As Single, player As AbstractPlayerController) As HomingProjectile
		Return Me.Create(pos, launchRotation, launchSpeed, homingMoveSpeed, rotationSpeed, timeBeforeDeath, 0F, homingEaseTime, player)
	End Function

	' Token: 0x06004407 RID: 17415 RVA: 0x000B6044 File Offset: 0x000B4444
	Public Function Create(pos As Vector2, launchRotation As Single, launchSpeed As Single, homingMoveSpeed As Single, rotationSpeed As Single, timeBeforeDeath As Single, timeBeforeHoming As Single, homingEaseTime As Single, player As AbstractPlayerController) As HomingProjectile
		Dim homingProjectile As HomingProjectile = TryCast(MyBase.Create(), HomingProjectile)
		homingProjectile.homingDirection = MathUtils.AngleToDirection(launchRotation)
		homingProjectile.launchVelocity = MathUtils.AngleToDirection(launchRotation) * launchSpeed
		homingProjectile.transform.position = pos
		homingProjectile.player = player
		homingProjectile.rotationSpeed = rotationSpeed
		homingProjectile.homingMoveSpeed = homingMoveSpeed
		homingProjectile.timeBeforeDeath = timeBeforeDeath
		homingProjectile.timeBeforeHoming = timeBeforeHoming
		homingProjectile.easeTime = homingEaseTime
		homingProjectile.HomingEnabled = True
		Return homingProjectile
	End Function

	' Token: 0x06004408 RID: 17416 RVA: 0x000B60C4 File Offset: 0x000B44C4
	Protected Overrides Sub Start()
		MyBase.Start()
		MyBase.StartCoroutine(Me.move_cr())
	End Sub

	' Token: 0x06004409 RID: 17417 RVA: 0x000B60D9 File Offset: 0x000B44D9
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
		MyBase.OnCollisionPlayer(hit, phase)
	End Sub

	' Token: 0x0600440A RID: 17418 RVA: 0x000B60F8 File Offset: 0x000B44F8
	Private Iterator Function move_cr() As IEnumerator
		Dim t As Single = 0F
		While t < Me.timeBeforeDeath + Me.easeTime + Me.timeBeforeHoming
			While Not Me.HomingEnabled
				Yield Nothing
			End While
			t += CupheadTime.FixedDelta
			If Me.player IsNot Nothing AndAlso Not Me.player.IsDead Then
				Dim center As Vector3 = Me.player.center
				If Me.trackGround Then
					center.y = CSng(Level.Current.Ground)
				End If
				Dim vector As Vector2 = (center - MyBase.transform.position).normalized
				Dim quaternion As Quaternion = Quaternion.Euler(0F, 0F, MathUtils.DirectionToAngle(vector))
				Dim quaternion2 As Quaternion = Quaternion.Euler(0F, 0F, MathUtils.DirectionToAngle(Me.homingDirection))
				Me.homingDirection = MathUtils.AngleToDirection(Quaternion.Slerp(quaternion2, quaternion, Mathf.Min(1F, CupheadTime.FixedDelta * Me.rotationSpeed)).eulerAngles.z)
			End If
			Dim homingVelocity As Vector2 = Me.homingDirection * Me.homingMoveSpeed
			Me.velocity = homingVelocity
			If t < Me.timeBeforeHoming Then
				Me.velocity = Me.launchVelocity
			ElseIf t < Me.timeBeforeHoming + Me.easeTime Then
				Dim num As Single = EaseUtils.EaseOutSine(0F, 1F, (t - Me.timeBeforeHoming) / Me.easeTime)
				Me.velocity = Vector2.Lerp(Me.launchVelocity, homingVelocity, num)
			End If
			If Me.faceMoveDirection Then
				MyBase.transform.SetEulerAngles(New Single?(0F), New Single?(0F), New Single?(MathUtils.DirectionToAngle(Me.velocity) + Me.spriteRotation))
			End If
			MyBase.transform.AddPosition(Me.velocity.x * CupheadTime.FixedDelta, Me.velocity.y * CupheadTime.FixedDelta, 0F)
			Yield New WaitForFixedUpdate()
		End While
		Me.Die()
		Return
	End Function

	' Token: 0x0400499E RID: 18846
	Private player As AbstractPlayerController

	' Token: 0x0400499F RID: 18847
	Private launchVelocity As Vector2

	' Token: 0x040049A0 RID: 18848
	Private homingMoveSpeed As Single

	' Token: 0x040049A1 RID: 18849
	Private rotationSpeed As Single

	' Token: 0x040049A2 RID: 18850
	Private timeBeforeDeath As Single

	' Token: 0x040049A3 RID: 18851
	Private timeBeforeHoming As Single

	' Token: 0x040049A4 RID: 18852
	Private easeTime As Single

	' Token: 0x040049A5 RID: 18853
	Private homingDirection As Vector2

	' Token: 0x040049A7 RID: 18855
	<SerializeField()>
	Private trackGround As Boolean

	' Token: 0x040049A8 RID: 18856
	<SerializeField()>
	Private faceMoveDirection As Boolean

	' Token: 0x040049A9 RID: 18857
	<SerializeField()>
	Private spriteRotation As Single

	' Token: 0x040049AA RID: 18858
	Protected velocity As Vector2
End Class
