Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000A6E RID: 2670
Public Class WeaponBouncerProjectile
	Inherits AbstractProjectile

	' Token: 0x17000580 RID: 1408
	' (get) Token: 0x06003FBD RID: 16317 RVA: 0x0022CA41 File Offset: 0x0022AE41
	Protected Overrides ReadOnly Property DestroyLifetime As Single
		Get
			Return 1000F
		End Get
	End Property

	' Token: 0x06003FBE RID: 16318 RVA: 0x0022CA48 File Offset: 0x0022AE48
	Protected Overrides Sub Start()
		MyBase.Start()
		If Me.isEx Then
			Me.damageDealer.SetDamageSource(DamageDealer.DamageSource.Ex)
			MyBase.StartCoroutine(Me.trail_cr())
		Else
			Select Case Global.UnityEngine.Random.Range(0, 4)
				Case 0
					MyBase.animator.Play("A", 0, Global.UnityEngine.Random.Range(0F, 1F))
				Case 1
					MyBase.animator.Play("B", 0, Global.UnityEngine.Random.Range(0F, 1F))
				Case 2
					MyBase.animator.Play("C", 0, Global.UnityEngine.Random.Range(0F, 1F))
				Case 3
					MyBase.animator.Play("D", 0, Global.UnityEngine.Random.Range(0F, 1F))
			End Select
		End If
	End Sub

	' Token: 0x06003FBF RID: 16319 RVA: 0x0022CB3C File Offset: 0x0022AF3C
	Protected Overrides Sub FixedUpdate()
		MyBase.FixedUpdate()
		If Me.firstUpdateNew Then
			Me.firstUpdateNew = False
			If Me.velocity.y < 0F Then
				Return
			End If
		End If
		If MyBase.dead Then
			Return
		End If
		Me.UpdateInAir()
		If Not Me.isEx AndAlso Not CupheadLevelCamera.Current.ContainsPoint(MyBase.transform.position, New Vector2(150F, 100F)) AndAlso Not CupheadLevelCamera.Current.ContainsPoint(New Vector3(MyBase.transform.position.x, MyBase.transform.position.y - 300F, 0F), New Vector2(150F, 100F)) Then
			Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
		End If
	End Sub

	' Token: 0x06003FC0 RID: 16320 RVA: 0x0022CC28 File Offset: 0x0022B028
	Private Sub UpdateInAir()
		If Me.timeUntilUnfreeze > 0F Then
			Me.timeUntilUnfreeze -= CupheadTime.FixedDelta
			MyBase.transform.position += New Vector3(Me.velocity.x * CupheadTime.FixedDelta, 0F, 0F)
		Else
			Me.velocity.y = Me.velocity.y - Me.gravity * CupheadTime.FixedDelta
			MyBase.transform.position += Me.velocity * CupheadTime.FixedDelta
		End If
	End Sub

	' Token: 0x06003FC1 RID: 16321 RVA: 0x0022CCDC File Offset: 0x0022B0DC
	Protected Overrides Sub OnCollisionGround(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionGround(hit, phase)
		Dim component As LevelPlatform = hit.GetComponent(Of LevelPlatform)()
		If(component Is Nothing OrElse Not component.canFallThrough) AndAlso Me.velocity.y < 0F Then
			Me.HitGround(hit)
		End If
	End Sub

	' Token: 0x06003FC2 RID: 16322 RVA: 0x0022CD2C File Offset: 0x0022B12C
	Protected Overrides Sub OnCollisionOther(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionOther(hit, phase)
		Dim component As LevelPlatform = hit.GetComponent(Of LevelPlatform)()
		If component IsNot Nothing AndAlso Not component.canFallThrough AndAlso Me.velocity.y < 0F Then
			Me.HitGround(hit)
		End If
	End Sub

	' Token: 0x06003FC3 RID: 16323 RVA: 0x0022CD7B File Offset: 0x0022B17B
	Protected Overrides Sub OnCollisionEnemy(hit As GameObject, phase As CollisionPhase)
		If Not Me.isEx Then
			Me.damageDealer.DealDamage(hit)
		End If
		MyBase.OnCollisionEnemy(hit, phase)
	End Sub

	' Token: 0x06003FC4 RID: 16324 RVA: 0x0022CDA0 File Offset: 0x0022B1A0
	Private Sub HitGround(hit As GameObject)
		Dim num As Single = Me.velocity.magnitude * WeaponProperties.LevelWeaponBouncer.Basic.bounceRatio - WeaponProperties.LevelWeaponBouncer.Basic.bounceSpeedDampening
		If num <= 0F OrElse Me.numBounces >= WeaponProperties.LevelWeaponBouncer.Basic.numBounces OrElse Me.isEx Then
			Me.Die()
		Else
			Me.velocity = Me.velocity.normalized * num
			Me.velocity.y = Me.velocity.y * -1F
			Me.numBounces += 1
			Me.timeUntilUnfreeze = 0.041666668F
			MyBase.animator.SetTrigger(If((Not Rand.Bool()), "Bounce_B", "Bounce_A"))
		End If
	End Sub

	' Token: 0x06003FC5 RID: 16325 RVA: 0x0022CE64 File Offset: 0x0022B264
	Private Iterator Function trail_cr() As IEnumerator
		While Not MyBase.dead
			Yield CupheadTime.WaitForSeconds(Me, Me.trailDelay)
			If MyBase.dead Then
				Return
			End If
			Me.trailFxPrefab.Create(MyBase.transform.position + MathUtils.RandomPointInUnitCircle() * Me.trailFxMaxOffset)
		End While
		Return
	End Function

	' Token: 0x06003FC6 RID: 16326 RVA: 0x0022CE80 File Offset: 0x0022B280
	Protected Overrides Sub Die()
		MyBase.Die()
		If Me.isEx Then
			Dim weaponArcProjectileExplosion As WeaponArcProjectileExplosion = Me.exExplosion.Create(MyBase.transform.position, Me.Damage, MyBase.DamageMultiplier, Me.PlayerId)
			weaponArcProjectileExplosion.DamageDealer.SetDamageSource(DamageDealer.DamageSource.Ex)
			Dim meterScoreTracker As MeterScoreTracker = New MeterScoreTracker(MeterScoreTracker.Type.Ex)
			meterScoreTracker.Add(weaponArcProjectileExplosion.DamageDealer)
			AudioManager.Play("player_weapon_bouncer_ex_explosion")
			Me.emitAudioFromObject.Add("player_weapon_bouncer_ex_explosion")
			Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
		Else
			MyBase.transform.SetEulerAngles(Nothing, Nothing, New Single?(CSng(Global.UnityEngine.Random.Range(0, 360))))
		End If
	End Sub

	' Token: 0x0400469B RID: 18075
	<SerializeField()>
	Private isEx As Boolean

	' Token: 0x0400469C RID: 18076
	<SerializeField()>
	Private exExplosion As WeaponArcProjectileExplosion

	' Token: 0x0400469D RID: 18077
	<SerializeField()>
	Private trailFxPrefab As Effect

	' Token: 0x0400469E RID: 18078
	<SerializeField()>
	Private trailFxMaxOffset As Single

	' Token: 0x0400469F RID: 18079
	<SerializeField()>
	Private trailDelay As Single

	' Token: 0x040046A0 RID: 18080
	Public gravity As Single

	' Token: 0x040046A1 RID: 18081
	Public velocity As Vector2

	' Token: 0x040046A2 RID: 18082
	Public weapon As WeaponBouncer

	' Token: 0x040046A3 RID: 18083
	Public bounceRatio As Single

	' Token: 0x040046A4 RID: 18084
	Public bounceSpeedDampening As Single

	' Token: 0x040046A5 RID: 18085
	Private timeUntilUnfreeze As Single

	' Token: 0x040046A6 RID: 18086
	Private Const bounceFreezeTime As Single = 0.041666668F

	' Token: 0x040046A7 RID: 18087
	Private numBounces As Integer

	' Token: 0x040046A8 RID: 18088
	Private firstUpdateNew As Boolean = True
End Class
