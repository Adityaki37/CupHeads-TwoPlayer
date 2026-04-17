Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020008B6 RID: 2230
Public Class FunhousePlatformingLevelJack
	Inherits AbstractPlatformingLevelEnemy

	' Token: 0x1700044A RID: 1098
	' (get) Token: 0x060033FC RID: 13308 RVA: 0x001E2BD0 File Offset: 0x001E0FD0
	' (set) Token: 0x060033FD RID: 13309 RVA: 0x001E2BD8 File Offset: 0x001E0FD8
	Public Property HomingEnabled As Boolean

	' Token: 0x060033FE RID: 13310 RVA: 0x001E2BE1 File Offset: 0x001E0FE1
	Protected Overrides Sub OnStart()
	End Sub

	' Token: 0x060033FF RID: 13311 RVA: 0x001E2BE4 File Offset: 0x001E0FE4
	Protected Overrides Sub Start()
		MyBase.Start()
		Dim flag As Boolean = Rand.Bool()
		MyBase.animator.Play(If((Not flag), "Green_Idle_A", "Pink_Idle_A"))
		Me._canParry = flag
		Me.HomingEnabled = True
		Me.player = PlayerManager.GetNext()
		Me.launchVelocity = Me.homingDirection * MyBase.Properties.jackLaunchVelocity
		MyBase.StartCoroutine(Me.move_cr())
		MyBase.StartCoroutine(Me.switch_cr())
	End Sub

	' Token: 0x06003400 RID: 13312 RVA: 0x001E2C6C File Offset: 0x001E106C
	Protected Overrides Sub Update()
		MyBase.Update()
		Me.CalculateRender()
	End Sub

	' Token: 0x06003401 RID: 13313 RVA: 0x001E2C7A File Offset: 0x001E107A
	Public Sub SelectDirection(fromBottom As Boolean)
		Me.homingDirection = If((Not fromBottom), Vector2.down, Vector2.up)
	End Sub

	' Token: 0x06003402 RID: 13314 RVA: 0x001E2C98 File Offset: 0x001E1098
	Private Iterator Function move_cr() As IEnumerator
		Dim t As Single = 0F
		While t < MyBase.Properties.jacktimeBeforeDeath + MyBase.Properties.jackEaseTime + MyBase.Properties.jacktimeBeforeHoming
			While Not Me.HomingEnabled
				Yield Nothing
			End While
			t += CupheadTime.FixedDelta
			If Me.player IsNot Nothing AndAlso Not Me.player.IsDead Then
				Dim center As Vector3 = Me.player.center
				Dim vector As Vector2 = (center - MyBase.transform.position).normalized
				Dim quaternion As Quaternion = Quaternion.Euler(0F, 0F, MathUtils.DirectionToAngle(vector))
				Dim quaternion2 As Quaternion = Quaternion.Euler(0F, 0F, MathUtils.DirectionToAngle(Me.homingDirection))
				Me.homingDirection = MathUtils.AngleToDirection(Quaternion.Slerp(quaternion2, quaternion, Mathf.Min(1F, CupheadTime.FixedDelta * MyBase.Properties.jackRotationSpeed)).eulerAngles.z)
			End If
			Dim homingVelocity As Vector2 = Me.homingDirection * MyBase.Properties.jackHomingMoveSpeed
			Dim velocity As Vector2 = homingVelocity
			If t < MyBase.Properties.jacktimeBeforeHoming Then
				velocity = Me.launchVelocity
			ElseIf t < MyBase.Properties.jacktimeBeforeHoming + MyBase.Properties.jackEaseTime Then
				Dim num As Single = EaseUtils.EaseOutSine(0F, 1F, (t - MyBase.Properties.jacktimeBeforeHoming) / MyBase.Properties.jackEaseTime)
				velocity = Vector2.Lerp(Me.launchVelocity, homingVelocity, num)
			End If
			MyBase.transform.AddPosition(velocity.x * CupheadTime.FixedDelta, velocity.y * CupheadTime.FixedDelta, 0F)
			Yield New WaitForFixedUpdate()
		End While
		Me.Die()
		Return
	End Function

	' Token: 0x06003403 RID: 13315 RVA: 0x001E2CB4 File Offset: 0x001E10B4
	Private Iterator Function switch_cr() As IEnumerator
		While True
			Yield CupheadTime.WaitForSeconds(Me, Global.UnityEngine.Random.Range(1.5F, 3F))
			MyBase.animator.SetTrigger("OnSwitch")
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06003404 RID: 13316 RVA: 0x001E2CCF File Offset: 0x001E10CF
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		Me.Die()
	End Sub

	' Token: 0x06003405 RID: 13317 RVA: 0x001E2CDF File Offset: 0x001E10DF
	Protected Overrides Sub OnCollision(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollision(hit, phase)
		If hit.GetComponent(Of FunhousePlatformingLevelJack)() IsNot Nothing Then
			Me.Die()
		End If
	End Sub

	' Token: 0x06003406 RID: 13318 RVA: 0x001E2D00 File Offset: 0x001E1100
	Private Sub CalculateRender()
		If CupheadLevelCamera.Current.ContainsPoint(MyBase.transform.position) AndAlso Not Me._enteredScreen Then
			Me._enteredScreen = True
		End If
		If Me._enteredScreen AndAlso Not CupheadLevelCamera.Current.ContainsPoint(MyBase.transform.position, New Vector2(100F, 100F)) Then
			Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
		End If
		If PlatformingLevel.Current IsNot Nothing AndAlso (MyBase.transform.position.x < CSng(PlatformingLevel.Current.Left) - 100F OrElse MyBase.transform.position.x > CSng(PlatformingLevel.Current.Right) + 100F OrElse MyBase.transform.position.y < CSng(PlatformingLevel.Current.Ground) - 100F) Then
			Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
		End If
	End Sub

	' Token: 0x06003407 RID: 13319 RVA: 0x001E2E18 File Offset: 0x001E1218
	Protected Overrides Sub Die()
		AudioManager.Play("funhouse_jack_death")
		Me.emitAudioFromObject.Add("funhouse_jack_death")
		MyBase.Die()
	End Sub

	' Token: 0x04003C47 RID: 15431
	Private Const SCREEN_PADDING As Single = 100F

	' Token: 0x04003C48 RID: 15432
	Private player As AbstractPlayerController

	' Token: 0x04003C49 RID: 15433
	Private _enteredScreen As Boolean

	' Token: 0x04003C4A RID: 15434
	Private homingDirection As Vector2 = Vector2.down

	' Token: 0x04003C4B RID: 15435
	Private launchVelocity As Vector2
End Class
