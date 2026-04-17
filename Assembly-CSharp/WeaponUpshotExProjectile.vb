Imports System
Imports UnityEngine

' Token: 0x02000A8A RID: 2698
Public Class WeaponUpshotExProjectile
	Inherits AbstractProjectile

	' Token: 0x0600407D RID: 16509 RVA: 0x00231B61 File Offset: 0x0022FF61
	Protected Overrides Sub OnDieDistance()
	End Sub

	' Token: 0x17000598 RID: 1432
	' (get) Token: 0x0600407E RID: 16510 RVA: 0x00231B63 File Offset: 0x0022FF63
	Protected Overrides ReadOnly Property DestroyLifetime As Single
		Get
			Return 4F
		End Get
	End Property

	' Token: 0x17000599 RID: 1433
	' (get) Token: 0x0600407F RID: 16511 RVA: 0x00231B6A File Offset: 0x0022FF6A
	Protected Overrides ReadOnly Property DestroyedAfterLeavingScreen As Boolean
		Get
			Return False
		End Get
	End Property

	' Token: 0x06004080 RID: 16512 RVA: 0x00231B70 File Offset: 0x0022FF70
	Protected Overrides Sub Start()
		MyBase.Start()
		Me.damageDealer.SetDamage(WeaponProperties.LevelWeaponUpshot.Ex.damage)
		Me.damageDealer.SetRate(WeaponProperties.LevelWeaponUpshot.Ex.damageRate)
		Me.damageDealer.isDLCWeapon = True
		Me.angle = MathUtils.DirectionToAngle(MyBase.transform.right)
		MyBase.transform.position += MyBase.transform.right * 120F
		MyBase.transform.localScale = New Vector3(CSng(If((MyBase.transform.eulerAngles.z <= 90F OrElse MyBase.transform.eulerAngles.z >= 270F), 1, (-1))), 1F)
		Me.endScale = MyBase.transform.localScale
		MyBase.transform.localScale *= 0.5F
		Me.startScale = MyBase.transform.localScale
		MyBase.transform.eulerAngles = Vector3.zero
		Me.startPos = MyBase.transform.position
		MyBase.animator.Play("EX", 0, Global.UnityEngine.Random.Range(0F, 1F))
		Me.trailPositions = New Vector2(5) {}
		For i As Integer = 0 To Me.trailPositions.Length - 1
			Me.trailPositions(i) = MyBase.transform.position
		Next
	End Sub

	' Token: 0x06004081 RID: 16513 RVA: 0x00231D10 File Offset: 0x00230110
	Protected Overrides Sub FixedUpdate()
		MyBase.FixedUpdate()
		If MyBase.dead Then
			Return
		End If
		If Me.timeUntilUnfreeze > 0F Then
			Me.timeUntilUnfreeze -= CupheadTime.FixedDelta
			Return
		End If
		Me.time += CupheadTime.FixedDelta
		Me.angle += Mathf.Lerp(WeaponProperties.LevelWeaponUpshot.Ex.minRotationSpeed, WeaponProperties.LevelWeaponUpshot.Ex.maxRotationSpeed, Me.time / WeaponProperties.LevelWeaponUpshot.Ex.rotationRampTime) * CupheadTime.FixedDelta * Me.rotateDir
		Me.radius += Mathf.Lerp(WeaponProperties.LevelWeaponUpshot.Ex.minRadiusSpeed, WeaponProperties.LevelWeaponUpshot.Ex.maxRadiusSpeed, Me.time / WeaponProperties.LevelWeaponUpshot.Ex.radiusRampTime) * CupheadTime.FixedDelta
		MyBase.transform.position = Me.startPos + MathUtils.AngleToDirection(Me.angle) * Me.radius
		Dim num As Single = Mathf.Round(Me.time * 24F) / 24F
		MyBase.transform.localScale = Vector3.Lerp(Me.startScale, Me.endScale, num * 5F)
		num *= 0.2F
		Me.trail1.color = New Color(1F, 1F, 1F, 0.5F - num)
		Me.trail2.color = New Color(1F, 1F, 1F, 0.25F - num)
		Me.UpdateTrails()
	End Sub

	' Token: 0x06004082 RID: 16514 RVA: 0x00231E90 File Offset: 0x00230290
	Private Sub UpdateTrails()
		Dim num As Integer = Me.currentPositionIndex - 2
		If num < 0 Then
			num += Me.trailPositions.Length
		End If
		Dim num2 As Integer = Me.currentPositionIndex - 5
		If num2 < 0 Then
			num2 += Me.trailPositions.Length
		End If
		Me.trail1.transform.position = Me.trailPositions(num)
		Me.trail2.transform.position = Me.trailPositions(num2)
		Me.currentPositionIndex = (Me.currentPositionIndex + 1) Mod Me.trailPositions.Length
		Me.trailPositions(Me.currentPositionIndex) = MyBase.transform.position
	End Sub

	' Token: 0x06004083 RID: 16515 RVA: 0x00231F5C File Offset: 0x0023035C
	Protected Overrides Sub OnCollisionEnemy(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionEnemy(hit, phase)
		Dim num As Single = Me.damageDealer.DealDamage(hit)
		Me.totalDamage += num
		If Me.totalDamage > WeaponProperties.LevelWeaponUpshot.Ex.maxDamage Then
			Me.Die()
		End If
		If num > 0F Then
			Me.hitFXPrefab.Create(Vector3.Lerp(MyBase.transform.position, hit.transform.position, 0.5F) + Global.UnityEngine.Random.insideUnitCircle * 20F)
			AudioManager.Play("player_ex_impact_hit")
			Me.emitAudioFromObject.Add("player_ex_impact_hit")
			Me.timeUntilUnfreeze = WeaponProperties.LevelWeaponUpshot.Ex.freezeTime
		End If
	End Sub

	' Token: 0x06004084 RID: 16516 RVA: 0x00232017 File Offset: 0x00230417
	Protected Overrides Sub Die()
		MyBase.Die()
		MyBase.GetComponent(Of Collider2D)().enabled = False
		MyBase.animator.Play("Die")
	End Sub

	' Token: 0x0400473B RID: 18235
	Private timeUntilUnfreeze As Single

	' Token: 0x0400473C RID: 18236
	Private totalDamage As Single

	' Token: 0x0400473D RID: 18237
	Private angle As Single

	' Token: 0x0400473E RID: 18238
	Private time As Single

	' Token: 0x0400473F RID: 18239
	Private radius As Single

	' Token: 0x04004740 RID: 18240
	Private startPos As Vector3

	' Token: 0x04004741 RID: 18241
	Public rotateDir As Single

	' Token: 0x04004742 RID: 18242
	Private startScale As Vector3

	' Token: 0x04004743 RID: 18243
	Private endScale As Vector3

	' Token: 0x04004744 RID: 18244
	Private trailPositions As Vector2()

	' Token: 0x04004745 RID: 18245
	Private currentPositionIndex As Integer

	' Token: 0x04004746 RID: 18246
	<SerializeField()>
	Private trail1 As SpriteRenderer

	' Token: 0x04004747 RID: 18247
	<SerializeField()>
	Private trail2 As SpriteRenderer

	' Token: 0x04004748 RID: 18248
	Private Const trailFrameDelay As Integer = 3

	' Token: 0x04004749 RID: 18249
	<SerializeField()>
	Private hitFXPrefab As Effect
End Class
