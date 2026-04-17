Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000A6C RID: 2668
Public Class WeaponBoomerangProjectile
	Inherits AbstractProjectile

	' Token: 0x1700057D RID: 1405
	' (get) Token: 0x06003FA9 RID: 16297 RVA: 0x0022BC3F File Offset: 0x0022A03F
	Protected Overrides ReadOnly Property DestroyLifetime As Single
		Get
			Return 1000F
		End Get
	End Property

	' Token: 0x06003FAA RID: 16298 RVA: 0x0022BC48 File Offset: 0x0022A048
	Protected Overrides Sub Start()
		MyBase.Start()
		Me.forwardDir = MathUtils.AngleToDirection(MyBase.transform.rotation.eulerAngles.z)
		Me.lateralDir = New Vector2(-Me.forwardDir.y, Me.forwardDir.x)
		Me.lateralDir *= Me.player.motor.TrueLookDirection.x
		Me.DestroyDistance = 0F
		If Me.isEx Then
			Me.trailPositions = New Vector2(5) {}
			For i As Integer = 0 To Me.trailPositions.Length - 1
				Me.trailPositions(i) = MyBase.transform.position
			Next
			MyBase.StartCoroutine(Me.ex_cr())
		Else
			MyBase.StartCoroutine(Me.basic_cr())
		End If
	End Sub

	' Token: 0x06003FAB RID: 16299 RVA: 0x0022BD4C File Offset: 0x0022A14C
	Protected Overrides Sub FixedUpdate()
		MyBase.FixedUpdate()
		If MyBase.dead Then
			Return
		End If
		If Me.wasCaught Then
			Me.Die()
		End If
		If Me.isEx AndAlso Me.hasTurned AndAlso (MyBase.transform.position - Me.player.center).magnitude < Me.player.colliderManager.Width / 2F + MyBase.GetComponent(Of CircleCollider2D)().radius Then
			Me.wasCaught = True
		End If
		Dim flag As Boolean = CupheadLevelCamera.Current.ContainsPoint(MyBase.transform.position, New Vector2(150F, 150F))
		MyBase.GetComponent(Of Collider2D)().enabled = flag
		If Not flag AndAlso Me.headedOffscreen Then
			Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
			Return
		End If
		If Me.isEx Then
			Me.updateTrails()
		End If
	End Sub

	' Token: 0x06003FAC RID: 16300 RVA: 0x0022BE4C File Offset: 0x0022A24C
	Private Sub updateTrails()
		Dim num As Integer = Me.currentPositionIndex - 2
		If num < 0 Then
			num += Me.trailPositions.Length
		End If
		Dim num2 As Integer = Me.currentPositionIndex - 5
		If num2 < 0 Then
			num2 += Me.trailPositions.Length
		End If
		Me.trail1.position = Me.trailPositions(num)
		Me.trail2.position = Me.trailPositions(num2)
		Me.currentPositionIndex = (Me.currentPositionIndex + 1) Mod Me.trailPositions.Length
		Me.trailPositions(Me.currentPositionIndex) = MyBase.transform.position
	End Sub

	' Token: 0x06003FAD RID: 16301 RVA: 0x0022BF10 File Offset: 0x0022A310
	Protected Overrides Sub Die()
		MyBase.transform.eulerAngles = New Vector3(0F, 0F, CSng(Global.UnityEngine.Random.Range(0, 360)))
		MyBase.Die()
		Me.StopAllCoroutines()
		Me.SetInt(AbstractProjectile.[Variant], Me.[variant])
	End Sub

	' Token: 0x06003FAE RID: 16302 RVA: 0x0022BF60 File Offset: 0x0022A360
	Private Iterator Function basic_cr() As IEnumerator
		Dim startPos As Vector2 = MyBase.transform.position
		Dim turnPos As Vector2 = startPos + Me.forwardDir * Me.forwardDistance + Me.lateralDir * Me.lateralDistance * 0.5F
		Dim returnPos As Vector2 = startPos + Me.lateralDir * Me.lateralDistance
		Dim moveTime As Single = Me.forwardDistance / Me.Speed * 1.5707964F
		Yield MyBase.StartCoroutine(Me.move_cr(turnPos, EaseUtils.EaseType.easeOutSine, EaseUtils.EaseType.easeInSine, moveTime))
		Me.hasTurned = True
		Yield MyBase.StartCoroutine(Me.move_cr(returnPos, EaseUtils.EaseType.easeInSine, EaseUtils.EaseType.easeOutSine, moveTime))
		Dim velocity As Vector2 = Me.Speed * -Me.forwardDir
		Me.headedOffscreen = True
		While True
			MyBase.transform.AddPosition(velocity.x * CupheadTime.FixedDelta, velocity.y * CupheadTime.FixedDelta, 0F)
			Yield New WaitForFixedUpdate()
		End While
		Return
	End Function

	' Token: 0x06003FAF RID: 16303 RVA: 0x0022BF7C File Offset: 0x0022A37C
	Private Iterator Function ex_cr() As IEnumerator
		Dim startPos As Vector2 = MyBase.transform.position
		Dim turnPos As Vector2 = startPos + Me.forwardDir * Me.forwardDistance + Me.lateralDir * Me.lateralDistance * 0.5F
		While True
			Dim ease As EaseUtils.EaseType = If((Not Me.hasTurned), EaseUtils.EaseType.easeOutSine, EaseUtils.EaseType.easeInOutSine)
			Dim moveTime As Single = If((Not Me.hasTurned), Me.forwardDistance, (Me.forwardDistance * 2F)) / Me.Speed
			Yield MyBase.StartCoroutine(Me.move_cr(turnPos, ease, ease, moveTime))
			Me.hasTurned = True
			startPos = MyBase.transform.position
			Dim playerPos As Vector2 = Me.player.transform.position
			turnPos = playerPos + (playerPos - startPos).normalized * Me.forwardDistance
		End While
		Return
	End Function

	' Token: 0x06003FB0 RID: 16304 RVA: 0x0022BF98 File Offset: 0x0022A398
	Private Iterator Function move_cr(endPos As Vector2, forwardEaseType As EaseUtils.EaseType, lateralEaseType As EaseUtils.EaseType, time As Single) As IEnumerator
		Dim t As Single = 0F
		Dim startPos As Vector2 = MyBase.transform.localPosition
		Dim relativeEndPos As Vector2 = endPos - startPos
		Dim forwardMovement As Single = Vector2.Dot(Me.forwardDir, relativeEndPos)
		Dim lateralMovement As Single = Vector2.Dot(Me.lateralDir, relativeEndPos)
		While t < time
			While Me.timeUntilUnfreeze > 0F
				Me.timeUntilUnfreeze -= CupheadTime.FixedDelta
				Yield New WaitForFixedUpdate()
			End While
			MyBase.transform.position = startPos + Me.forwardDir * EaseUtils.Ease(forwardEaseType, 0F, forwardMovement, t / time) + Me.lateralDir * EaseUtils.Ease(lateralEaseType, 0F, lateralMovement, t / time)
			t += CupheadTime.FixedDelta
			Yield New WaitForFixedUpdate()
		End While
		MyBase.transform.position = endPos
		Return
	End Function

	' Token: 0x06003FB1 RID: 16305 RVA: 0x0022BFD0 File Offset: 0x0022A3D0
	Protected Overrides Sub OnCollisionEnemy(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionEnemy(hit, phase)
		Dim num As Single = Me.damageDealer.DealDamage(hit)
		If Me.isEx Then
			Me.totalDamage += num
			If Me.totalDamage > Me.maxDamage Then
				Me.Die()
			End If
			If num > 0F Then
				Me.hitFXPrefab.Create(MyBase.transform.position)
				Me.timeUntilUnfreeze = Me.hitFreezeTime
				AudioManager.Play("player_ex_impact_hit")
				Me.emitAudioFromObject.Add("player_ex_impact_hit")
			End If
		End If
	End Sub

	' Token: 0x06003FB2 RID: 16306 RVA: 0x0022C06A File Offset: 0x0022A46A
	Public Sub SetPink(pink As Boolean)
		If pink Then
			Me.SetParryable(True)
			Me.[variant] = 2
		Else
			Me.SetParryable(False)
			Me.[variant] = Global.UnityEngine.Random.Range(0, 2)
		End If
		Me.SetInt(AbstractProjectile.[Variant], Me.[variant])
	End Sub

	' Token: 0x06003FB3 RID: 16307 RVA: 0x0022C0AC File Offset: 0x0022A4AC
	Protected Overrides Sub OnCollisionDie(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionDie(hit, phase)
		If MyBase.tag = "PlayerProjectile" AndAlso phase = CollisionPhase.Enter Then
			If hit.GetComponent(Of DamageReceiver)() AndAlso hit.GetComponent(Of DamageReceiver)().enabled Then
				AudioManager.Play("player_shoot_hit_cuphead")
			Else
				AudioManager.Play("player_weapon_peashot_miss")
			End If
		End If
	End Sub

	' Token: 0x04004685 RID: 18053
	Private Const TurnTimeRatio As Single = 0.4F

	' Token: 0x04004686 RID: 18054
	Public Speed As Single

	' Token: 0x04004687 RID: 18055
	Public forwardDistance As Single

	' Token: 0x04004688 RID: 18056
	Public lateralDistance As Single

	' Token: 0x04004689 RID: 18057
	Public maxDamage As Single

	' Token: 0x0400468A RID: 18058
	Public hitFreezeTime As Single

	' Token: 0x0400468B RID: 18059
	Public player As LevelPlayerController

	' Token: 0x0400468C RID: 18060
	<SerializeField()>
	Private isEx As Boolean

	' Token: 0x0400468D RID: 18061
	<SerializeField()>
	Private trail1 As Transform

	' Token: 0x0400468E RID: 18062
	<SerializeField()>
	Private trail2 As Transform

	' Token: 0x0400468F RID: 18063
	<SerializeField()>
	Private hitFXPrefab As Effect

	' Token: 0x04004690 RID: 18064
	Private trailPositions As Vector2()

	' Token: 0x04004691 RID: 18065
	Private currentPositionIndex As Integer

	' Token: 0x04004692 RID: 18066
	Private Const trailFrameDelay As Integer = 3

	' Token: 0x04004693 RID: 18067
	Private forwardDir As Vector2

	' Token: 0x04004694 RID: 18068
	Private lateralDir As Vector2

	' Token: 0x04004695 RID: 18069
	Private hasTurned As Boolean

	' Token: 0x04004696 RID: 18070
	Private wasCaught As Boolean

	' Token: 0x04004697 RID: 18071
	Private headedOffscreen As Boolean

	' Token: 0x04004698 RID: 18072
	Private totalDamage As Single

	' Token: 0x04004699 RID: 18073
	Private [variant] As Integer

	' Token: 0x0400469A RID: 18074
	Private timeUntilUnfreeze As Single
End Class
