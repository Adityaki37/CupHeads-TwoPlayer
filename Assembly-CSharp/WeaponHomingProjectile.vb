Imports System
Imports System.Collections.Generic
Imports UnityEngine

' Token: 0x02000A7D RID: 2685
Public Class WeaponHomingProjectile
	Inherits AbstractProjectile

	' Token: 0x1700058D RID: 1421
	' (get) Token: 0x0600402E RID: 16430 RVA: 0x0022FF02 File Offset: 0x0022E302
	Protected Overrides ReadOnly Property DestroyLifetime As Single
		Get
			Return 100F
		End Get
	End Property

	' Token: 0x0600402F RID: 16431 RVA: 0x0022FF0C File Offset: 0x0022E30C
	Protected Overrides Sub OnCollisionDie(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionDie(hit, phase)
		If MyBase.tag = "PlayerProjectile" AndAlso phase = CollisionPhase.Enter Then
			If hit.GetComponent(Of DamageReceiver)() AndAlso hit.GetComponent(Of DamageReceiver)().enabled Then
				AudioManager.Play("player_shoot_hit_cuphead")
			Else
				AudioManager.Play("player_weapon_homing_impact")
			End If
		End If
	End Sub

	' Token: 0x06004030 RID: 16432 RVA: 0x0022FF78 File Offset: 0x0022E378
	Public Overrides Function Create(position As Vector2, rotation As Single, scale As Vector2) As AbstractProjectile
		Dim weaponHomingProjectile As WeaponHomingProjectile = TryCast(MyBase.Create(position, rotation, scale), WeaponHomingProjectile)
		For i As Integer = 0 To Me.trailPositions.Length - 1
			weaponHomingProjectile.trailPositions(i) = position
			weaponHomingProjectile.trailRotations(i) = MyBase.transform.eulerAngles.z
		Next
		If MathUtils.RandomBool() Then
			Me.trail.SetScale(New Single?(-1F), Nothing, Nothing)
		End If
		If MathUtils.RandomBool() Then
			Me.trail.SetScale(Nothing, New Single?(-1F), Nothing)
		End If
		Return weaponHomingProjectile
	End Function

	' Token: 0x06004031 RID: 16433 RVA: 0x0023003D File Offset: 0x0022E43D
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
	End Sub

	' Token: 0x06004032 RID: 16434 RVA: 0x00230045 File Offset: 0x0022E445
	Protected Overrides Sub OnCollisionOther(hit As GameObject, phase As CollisionPhase)
		If hit.tag = "Parry" Then
			Return
		End If
		MyBase.OnCollisionOther(hit, phase)
	End Sub

	' Token: 0x06004033 RID: 16435 RVA: 0x00230065 File Offset: 0x0022E465
	Protected Overrides Sub OnCollisionEnemy(hit As GameObject, phase As CollisionPhase)
		Me.DealDamage(hit)
		If Me.isEx Then
			AudioManager.Play("player_ex_impact_hit")
			Me.emitAudioFromObject.Add("player_ex_impact_hit")
		End If
		MyBase.OnCollisionEnemy(hit, phase)
	End Sub

	' Token: 0x06004034 RID: 16436 RVA: 0x0023009B File Offset: 0x0022E49B
	Private Sub DealDamage(hit As GameObject)
		Me.damageDealer.DealDamage(hit)
	End Sub

	' Token: 0x06004035 RID: 16437 RVA: 0x002300AC File Offset: 0x0022E4AC
	Protected Overrides Sub Die()
		Me.move = False
		AudioManager.Play("player_weapon_peashot_miss")
		Dim component As EffectSpawner = MyBase.GetComponent(Of EffectSpawner)()
		If component IsNot Nothing Then
			Global.UnityEngine.[Object].Destroy(component)
		End If
		Me.trail.gameObject.SetActive(False)
		MyBase.Die()
	End Sub

	' Token: 0x06004036 RID: 16438 RVA: 0x002300FC File Offset: 0x0022E4FC
	Protected Overrides Sub FixedUpdate()
		MyBase.FixedUpdate()
		Dim state As WeaponHomingProjectile.State = Me.state
		If state <> WeaponHomingProjectile.State.Homing Then
			If state = WeaponHomingProjectile.State.Swirling Then
				Me.UpdateSwirling()
			End If
		Else
			Me.UpdateHoming()
		End If
		Me.trailFollowIndex = (Me.trailFollowIndex + 1) Mod Me.trailFollowFrames
		Me.trailRotation = Me.trailRotations(Me.trailFollowIndex)
		Me.trail.transform.position = Me.trailPositions(Me.trailFollowIndex)
		Me.trailRotations(Me.trailFollowIndex) = Me.rotation
		Me.trailPositions(Me.trailFollowIndex) = MyBase.transform.position
	End Sub

	' Token: 0x06004037 RID: 16439 RVA: 0x002301CC File Offset: 0x0022E5CC
	Private Sub UpdateHoming()
		If Not Me.move Then
			Return
		End If
		Me.t += CupheadTime.FixedDelta
		If Me.target IsNot Nothing AndAlso Me.target.gameObject.activeInHierarchy AndAlso Me.target.isActiveAndEnabled AndAlso Me.t < WeaponProperties.LevelWeaponHoming.Basic.maxHomingTime Then
			Dim num As Single
			num = MathUtils.DirectionToAngle(Me.target.bounds.center - MyBase.transform.position)
			While num > Me.rotation + 180F
				num -= 360F
			End While
			While num < Me.rotation - 180F
				num += 360F
			End While
			Dim num2 As Single = Me.rotationSpeed.min
			If Me.t > Me.timeBeforeEaseRotationSpeed + Me.rotationSpeedEaseTime Then
				num2 = Me.rotationSpeed.max
			ElseIf Me.t > Me.timeBeforeEaseRotationSpeed Then
				num2 = Me.rotationSpeed.GetFloatAt((Me.t - Me.timeBeforeEaseRotationSpeed) / Me.rotationSpeedEaseTime)
			End If
			If Mathf.Abs(num - Me.rotation) < num2 * CupheadTime.FixedDelta Then
				Me.rotation = num
			ElseIf num > Me.rotation Then
				Me.rotation += num2 * CupheadTime.FixedDelta
			Else
				Me.rotation -= num2 * CupheadTime.FixedDelta
			End If
		End If
		Dim vector As Vector3 = MathUtils.AngleToDirection(Me.rotation)
		MyBase.transform.position += vector * Me.speed * CupheadTime.FixedDelta
		If Not CupheadLevelCamera.Current.ContainsPoint(MyBase.transform.position, New Vector2(Me.destroyPadding, Me.destroyPadding)) Then
			Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
		End If
	End Sub

	' Token: 0x06004038 RID: 16440 RVA: 0x002303E8 File Offset: 0x0022E7E8
	Private Sub UpdateSwirling()
		If Not Me.move Then
			Return
		End If
		If Me.player.IsDead Then
			Me.StopSwirling()
			Return
		End If
		Me.t += CupheadTime.FixedDelta
		Dim vector As Vector2 = Me.swirlLaunchPos + MathUtils.AngleToDirection(Me.swirlLaunchRotation) * Me.t * Me.speed
		Dim num As Single = 360F * Me.speed / (Me.swirlDistance * 2F * 3.1415927F)
		Me.swirlRotation += num * CupheadTime.FixedDelta
		Dim vector2 As Vector2 = Me.player.center + MathUtils.AngleToDirection(Me.swirlRotation) * Me.swirlDistance
		If Me.t < Me.swirlEaseTime Then
			Dim vector3 As Vector2 = MyBase.transform.position
			MyBase.transform.position = Vector2.Lerp(vector, vector2, EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, 0F, 1F, Me.t / Me.swirlEaseTime))
			Me.rotation = MathUtils.DirectionToAngle(MyBase.transform.position - vector3)
		Else
			MyBase.transform.position = vector2
			Me.rotation = Me.swirlRotation + 90F
		End If
	End Sub

	' Token: 0x06004039 RID: 16441 RVA: 0x00230558 File Offset: 0x0022E958
	Protected Overrides Sub Update()
		MyBase.Update()
		Me.timeSinceUpdateRotation += CupheadTime.Delta
		If Me.timeSinceUpdateRotation > 0.041666668F Then
			Me.timeSinceUpdateRotation -= 0.041666668F
			Dim vector As Vector2 = Me.trail.transform.position
			MyBase.transform.SetEulerAngles(New Single?(0F), New Single?(0F), New Single?(Me.rotation + Me.spriteRotation))
			Me.trail.SetEulerAngles(New Single?(0F), New Single?(0F), New Single?(Me.trailRotation + Me.trailSpriteRotation))
			Me.trail.position = vector
		End If
	End Sub

	' Token: 0x0600403A RID: 16442 RVA: 0x0023062D File Offset: 0x0022EA2D
	Public Sub FindTarget()
		Me.target = Me.findBestTarget(AbstractProjectile.FindOverlapScreenDamageReceivers())
	End Sub

	' Token: 0x0600403B RID: 16443 RVA: 0x00230640 File Offset: 0x0022EA40
	Private Function findBestTarget(damageReceivers As IEnumerable(Of DamageReceiver)) As Collider2D
		Dim vector As Vector2 = MyBase.transform.position + Me.speed * (Me.timeBeforeEaseRotationSpeed + Me.rotationSpeedEaseTime * 0.75F) * MathUtils.AngleToDirection(Me.rotation)
		Dim num As Single = Single.MaxValue
		Dim collider2D As Collider2D = Nothing
		For Each damageReceiver As DamageReceiver In damageReceivers
			If damageReceiver.gameObject.activeInHierarchy AndAlso damageReceiver.enabled AndAlso damageReceiver.type = DamageReceiver.Type.Enemy Then
				For Each collider2D2 As Collider2D In damageReceiver.GetComponents(Of Collider2D)()
					If collider2D2.isActiveAndEnabled AndAlso CupheadLevelCamera.Current.ContainsPoint(collider2D2.bounds.center, collider2D2.bounds.size / 2F) Then
						Dim sqrMagnitude As Single = (vector - collider2D2.bounds.center).sqrMagnitude
						If sqrMagnitude < num Then
							num = sqrMagnitude
							collider2D = collider2D2
						End If
					End If
				Next
				For Each damageReceiverChild As DamageReceiverChild In damageReceiver.GetComponentsInChildren(Of DamageReceiverChild)()
					For Each collider2D3 As Collider2D In damageReceiverChild.GetComponents(Of Collider2D)()
						If collider2D3.isActiveAndEnabled AndAlso CupheadLevelCamera.Current.ContainsPoint(collider2D3.bounds.center, collider2D3.bounds.size / 2F) Then
							Dim sqrMagnitude2 As Single = (vector - collider2D3.bounds.center).sqrMagnitude
							If sqrMagnitude2 < num Then
								num = sqrMagnitude2
								collider2D = collider2D3
							End If
						End If
					Next
				Next
			End If
		Next
		Return collider2D
	End Function

	' Token: 0x0600403C RID: 16444 RVA: 0x00230894 File Offset: 0x0022EC94
	Public Sub StartSwirling(index As Integer, bulletCount As Integer, spread As Single, player As AbstractPlayerController)
		Me.state = WeaponHomingProjectile.State.Swirling
		Me.swirlLaunchRotation = MyBase.transform.eulerAngles.z + (CSng(index) / CSng((bulletCount - 1)) - 0.5F) * spread
		Me.swirlRotation = MyBase.transform.eulerAngles.z + (CSng(index) / CSng((bulletCount - 1)) - 0.5F) * ((CSng(bulletCount) - 1F) / CSng(bulletCount)) * 360F
		Me.swirlLaunchPos = MyBase.transform.position
		Me.rotation = Me.swirlLaunchRotation
		MyBase.animator.Play("A", 0, CSng(index) / CSng(bulletCount))
		MyBase.animator.Play("Idle", 1, CSng(index) / CSng(bulletCount))
		Me.player = player
	End Sub

	' Token: 0x0600403D RID: 16445 RVA: 0x00230960 File Offset: 0x0022ED60
	Public Sub StopSwirling()
		Me.state = WeaponHomingProjectile.State.Homing
		Me.FindTarget()
		Me.t = 0F
	End Sub

	' Token: 0x040046F3 RID: 18163
	<SerializeField()>
	Private spriteRotation As Single

	' Token: 0x040046F4 RID: 18164
	<SerializeField()>
	Private trailSpriteRotation As Single

	' Token: 0x040046F5 RID: 18165
	<SerializeField()>
	Private trail As Transform

	' Token: 0x040046F6 RID: 18166
	<SerializeField()>
	Private destroyPadding As Single

	' Token: 0x040046F7 RID: 18167
	Public speed As Single

	' Token: 0x040046F8 RID: 18168
	Public rotationSpeed As MinMax

	' Token: 0x040046F9 RID: 18169
	Public timeBeforeEaseRotationSpeed As Single

	' Token: 0x040046FA RID: 18170
	Public rotationSpeedEaseTime As Single

	' Token: 0x040046FB RID: 18171
	Public rotation As Single

	' Token: 0x040046FC RID: 18172
	Public swirlDistance As Single

	' Token: 0x040046FD RID: 18173
	Public swirlEaseTime As Single

	' Token: 0x040046FE RID: 18174
	Public trailFollowFrames As Integer

	' Token: 0x040046FF RID: 18175
	Private state As WeaponHomingProjectile.State

	' Token: 0x04004700 RID: 18176
	Private velocity As Vector2

	' Token: 0x04004701 RID: 18177
	Private t As Single

	' Token: 0x04004702 RID: 18178
	Private move As Boolean = True

	' Token: 0x04004703 RID: 18179
	Private target As Collider2D

	' Token: 0x04004704 RID: 18180
	Private swirlLaunchRotation As Single

	' Token: 0x04004705 RID: 18181
	Private swirlRotation As Single

	' Token: 0x04004706 RID: 18182
	Private player As AbstractPlayerController

	' Token: 0x04004707 RID: 18183
	Private swirlLaunchPos As Vector2

	' Token: 0x04004708 RID: 18184
	Private timeSinceUpdateRotation As Single = 0.041666668F

	' Token: 0x04004709 RID: 18185
	Private trailRotation As Single

	' Token: 0x0400470A RID: 18186
	Public isEx As Boolean

	' Token: 0x0400470B RID: 18187
	Private trailPositions As Vector2() = New Vector2(9) {}

	' Token: 0x0400470C RID: 18188
	Private trailRotations As Single() = New Single(9) {}

	' Token: 0x0400470D RID: 18189
	Private trailFollowIndex As Integer

	' Token: 0x02000A7E RID: 2686
	Public Enum State
		' Token: 0x0400470F RID: 18191
		Homing
		' Token: 0x04004710 RID: 18192
		Swirling
	End Enum
End Class
