Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000743 RID: 1859
Public Class RetroArcadeChaser
	Inherits RetroArcadeEnemy

	' Token: 0x170003D9 RID: 985
	' (get) Token: 0x0600287D RID: 10365 RVA: 0x00179635 File Offset: 0x00177A35
	' (set) Token: 0x0600287E RID: 10366 RVA: 0x0017963D File Offset: 0x00177A3D
	Public Property IsDone As Boolean

	' Token: 0x170003DA RID: 986
	' (get) Token: 0x0600287F RID: 10367 RVA: 0x00179646 File Offset: 0x00177A46
	' (set) Token: 0x06002880 RID: 10368 RVA: 0x0017964E File Offset: 0x00177A4E
	Public Property HomingEnabled As Boolean

	' Token: 0x06002881 RID: 10369 RVA: 0x00179658 File Offset: 0x00177A58
	Public Overridable Function Init(pos As Vector3, launchRotation As Single, launchSpeed As Single, homingMoveSpeed As Single, rotationSpeed As Single, timeBeforeDeath As Single, hp As Single, player As AbstractPlayerController, properties As LevelProperties.RetroArcade.Chasers) As RetroArcadeChaser
		MyBase.ResetLifetime()
		MyBase.ResetDistance()
		MyBase.transform.position = pos
		Me.homingDirection = MathUtils.AngleToDirection(launchRotation)
		Me.launchVelocity = MathUtils.AngleToDirection(launchRotation) * launchSpeed
		MyBase.transform.position = pos
		Me.player = player
		Me.rotationSpeed = rotationSpeed
		Me.homingMoveSpeed = homingMoveSpeed
		Me.timeBeforeDeath = timeBeforeDeath
		Me.HomingEnabled = True
		Me.hp = hp
		Me.StartChaser()
		Return Me
	End Function

	' Token: 0x06002882 RID: 10370 RVA: 0x001796DD File Offset: 0x00177ADD
	Private Sub StartChaser()
		MyBase.StartCoroutine(Me.move_cr())
	End Sub

	' Token: 0x06002883 RID: 10371 RVA: 0x001796EC File Offset: 0x00177AEC
	Private Iterator Function move_cr() As IEnumerator
		Dim t As Single = 0F
		While t < Me.timeBeforeDeath + Me.timeBeforeHoming
			While Not Me.HomingEnabled
				Yield Nothing
			End While
			t += CupheadTime.FixedDelta
			If Me.player IsNot Nothing AndAlso Not Me.player.IsDead Then
				Dim center As Vector3 = Me.player.center
				Dim vector As Vector2 = (center - MyBase.transform.position).normalized
				Dim quaternion As Quaternion = Quaternion.Euler(0F, 0F, MathUtils.DirectionToAngle(vector))
				Dim quaternion2 As Quaternion = Quaternion.Euler(0F, 0F, MathUtils.DirectionToAngle(Me.homingDirection))
				Me.homingDirection = MathUtils.AngleToDirection(Quaternion.Slerp(quaternion2, quaternion, Mathf.Min(1F, CupheadTime.FixedDelta * Me.rotationSpeed)).eulerAngles.z)
			End If
			Dim homingVelocity As Vector2 = Me.homingDirection * Me.homingMoveSpeed
			Me.velocity = homingVelocity
			If t < Me.timeBeforeHoming Then
				Me.velocity = Me.launchVelocity
			ElseIf t < Me.timeBeforeHoming Then
				Dim num As Single = EaseUtils.EaseOutSine(0F, 1F, t - Me.timeBeforeHoming)
				Me.velocity = Vector2.Lerp(Me.launchVelocity, homingVelocity, num)
			End If
			MyBase.transform.AddPosition(Me.velocity.x * CupheadTime.FixedDelta, Me.velocity.y * CupheadTime.FixedDelta, 0F)
			Yield New WaitForFixedUpdate()
		End While
		Dim offset As Single = 100F
		While MyBase.transform.position.x > -640F - offset AndAlso MyBase.transform.position.x < 640F + offset AndAlso MyBase.transform.position.x > -360F - offset AndAlso MyBase.transform.position.x < 360F + offset
			MyBase.transform.position += Me.velocity.normalized * Me.homingMoveSpeed * CupheadTime.FixedDelta
			MyBase.transform.SetEulerAngles(New Single?(0F), New Single?(0F), New Single?(MathUtils.DirectionToAngle(Me.velocity) + 180F))
			Yield New WaitForFixedUpdate()
		End While
		Me.IsDone = True
		Me.Recycle()
		Return
	End Function

	' Token: 0x04003151 RID: 12625
	Private player As AbstractPlayerController

	' Token: 0x04003152 RID: 12626
	Private launchVelocity As Vector2

	' Token: 0x04003153 RID: 12627
	Private homingMoveSpeed As Single

	' Token: 0x04003154 RID: 12628
	Private rotationSpeed As Single

	' Token: 0x04003155 RID: 12629
	Private timeBeforeDeath As Single

	' Token: 0x04003156 RID: 12630
	Private timeBeforeHoming As Single

	' Token: 0x04003157 RID: 12631
	Private homingDirection As Vector2

	' Token: 0x04003159 RID: 12633
	Protected velocity As Vector2
End Class
