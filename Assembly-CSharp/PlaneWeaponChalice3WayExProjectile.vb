Imports System
Imports UnityEngine

' Token: 0x02000ABC RID: 2748
Public Class PlaneWeaponChalice3WayExProjectile
	Inherits AbstractProjectile

	' Token: 0x06004203 RID: 16899 RVA: 0x0023A728 File Offset: 0x00238B28
	Protected Overrides Sub OnDealDamage(damage As Single, receiver As DamageReceiver, damageDealer As DamageDealer)
		MyBase.OnDealDamage(damage, receiver, damageDealer)
		If Me.state = PlaneWeaponChalice3WayExProjectile.State.Idle Then
			Me.Freeze()
			Me.partner.Freeze()
		End If
		AudioManager.Play("player_plane_weapon_ex_chomp")
		Me.emitAudioFromObject.Add("player_plane_weapon_ex_chomp")
	End Sub

	' Token: 0x06004204 RID: 16900 RVA: 0x0023A774 File Offset: 0x00238B74
	Public Sub Freeze()
		Me.state = PlaneWeaponChalice3WayExProjectile.State.Frozen
		Me.timeSinceFrozen = 0F
		Me.deathSpark.transform.localScale = New Vector3(0.5F, 0.5F)
		Me.deathSpark.flipX = Rand.Bool()
		Me.deathSpark.flipY = Rand.Bool()
		Me.deathSpark.transform.eulerAngles = New Vector3(0F, 0F, CSng(Global.UnityEngine.Random.Range(0, 360)))
		MyBase.animator.Play("Spark", 1, 0F)
	End Sub

	' Token: 0x06004205 RID: 16901 RVA: 0x0023A814 File Offset: 0x00238C14
	Public Sub SetArcPosition()
		MyBase.transform.localPosition = New Vector3(Mathf.Sin(EaseUtils.Linear(0.15F, 1F, Me.arcTimer) * 3.1415927F) * Me.arcX, 10F + EaseUtils.Linear(0F, 1F, Me.arcTimer) * 3.1415927F * Me.vDirection * Me.arcY)
	End Sub

	' Token: 0x06004206 RID: 16902 RVA: 0x0023A888 File Offset: 0x00238C88
	Protected Overrides Sub FixedUpdate()
		MyBase.FixedUpdate()
		If MyBase.dead Then
			Return
		End If
		Me.smokeTimer += CupheadTime.FixedDelta
		If Me.smokeTimer > Me.firstSmokeDelay Then
			Me.smokeFX.Create(MyBase.transform.position)
			Me.smokeTimer -= Me.smokeDelay
		End If
		Me.sparkleTimer += CupheadTime.FixedDelta
		If Me.sparkleTimer > Me.sparkleDelay Then
			Me.sparkleFX.Create(MyBase.transform.position + MathUtils.AngleToDirection(CSng(Global.UnityEngine.Random.Range(0, 360))) * Global.UnityEngine.Random.Range(0F, Me.sparkleRadius))
			Me.sparkleTimer -= Me.sparkleDelay
		End If
		Select Case Me.state
			Case PlaneWeaponChalice3WayExProjectile.State.Idle
				Me.SetArcPosition()
				Me.arcTimer += Me.arcSpeed / 3.1415927F * CupheadTime.FixedDelta
				MyBase.transform.localScale = New Vector3(Mathf.Lerp(0.5F, 1F, Me.arcTimer), Mathf.Lerp(0.5F, 1F, Me.arcTimer))
				If Me.arcTimer > 1F Then
					Me.state = PlaneWeaponChalice3WayExProjectile.State.Paused
					Me.damageDealer.SetDamage(Me.damageAfterLaunch)
					Me.CollisionDeath.Enemies = True
					MyBase.transform.localScale = New Vector3(1F, 1F)
				End If
			Case PlaneWeaponChalice3WayExProjectile.State.Frozen
				Me.timeSinceFrozen += CupheadTime.FixedDelta
				If Me.timeSinceFrozen > Me.FreezeTime Then
					Me.state = PlaneWeaponChalice3WayExProjectile.State.Idle
				End If
			Case PlaneWeaponChalice3WayExProjectile.State.Paused
				Me.pauseTime -= CupheadTime.FixedDelta
				If Me.pauseTime <= 0F Then
					Me.FindTarget()
					Me.state = PlaneWeaponChalice3WayExProjectile.State.Launched
					Dim vector As Vector3 = MyBase.transform.parent.position + Vector3.right * Me.xDistanceNoTarget
					MyBase.transform.parent = Nothing
					If Me.target IsNot Nothing AndAlso Me.target.gameObject.activeInHierarchy AndAlso Me.target.isActiveAndEnabled Then
						vector = Me.target.transform.position
						vector.x = Mathf.Clamp(vector.x, MyBase.transform.position.x + Me.minXDistance, vector.x)
					End If
					Me.velocityAfterLaunch = (vector - MyBase.transform.position).normalized
					Me.accelVectorAfterLaunch = Me.velocityAfterLaunch * Me.accelAfterLaunch
					Me.velocityAfterLaunch *= Me.speedAfterLaunch
				End If
			Case PlaneWeaponChalice3WayExProjectile.State.Launched
				MyBase.transform.position += Me.velocityAfterLaunch * CupheadTime.FixedDelta
				Me.velocityAfterLaunch += Me.accelVectorAfterLaunch * CupheadTime.FixedDelta
				If Me.velocityAfterLaunch.x > 0F Then
					If Not MyBase.animator.GetCurrentAnimatorStateInfo(0).IsName("Shoot") Then
						MyBase.animator.Play("Shoot")
						Me.shootFX.Create(MyBase.transform.position + Vector3.left * 20F)
					End If
					Me.magnet.transform.eulerAngles = New Vector3(0F, 0F, MathUtils.DirectionToAngle(Me.velocityAfterLaunch))
				End If
		End Select
	End Sub

	' Token: 0x06004207 RID: 16903 RVA: 0x0023AC91 File Offset: 0x00239091
	Protected Overrides Sub OnCollisionEnemy(hit As GameObject, phase As CollisionPhase)
		Me.DealDamage(hit)
		MyBase.OnCollisionEnemy(hit, phase)
	End Sub

	' Token: 0x06004208 RID: 16904 RVA: 0x0023ACA2 File Offset: 0x002390A2
	Protected Overrides Sub OnCollisionOther(hit As GameObject, phase As CollisionPhase)
		If hit.tag = "Parry" Then
			Return
		End If
		MyBase.OnCollisionOther(hit, phase)
	End Sub

	' Token: 0x06004209 RID: 16905 RVA: 0x0023ACC2 File Offset: 0x002390C2
	Private Sub DealDamage(hit As GameObject)
		Me.damageDealer.DealDamage(hit)
	End Sub

	' Token: 0x0600420A RID: 16906 RVA: 0x0023ACD4 File Offset: 0x002390D4
	Protected Overrides Sub Die()
		MyBase.Die()
		Me.magnet.transform.eulerAngles = Vector3.zero
		Me.magnet.flipX = Rand.Bool()
		Me.deathSpark.transform.localScale = New Vector3(1F, 1F)
		Me.deathSpark.flipX = Rand.Bool()
		Me.deathSpark.flipY = Rand.Bool()
		Me.deathSpark.transform.eulerAngles = New Vector3(0F, 0F, CSng(Global.UnityEngine.Random.Range(0, 360)))
		MyBase.animator.Play("Spark", 1, 0F)
		MyBase.animator.Play(If((Me.ID <> 0), "DieB", "DieA"))
	End Sub

	' Token: 0x0600420B RID: 16907 RVA: 0x0023ADB4 File Offset: 0x002391B4
	Public Sub FindTarget()
		If Me.partner IsNot Nothing AndAlso Me.ID = 1 Then
			Return
		End If
		Dim num As Single = Single.MaxValue
		Dim collider2D As Collider2D = Nothing
		Dim vector As Vector2 = MyBase.transform.parent.position
		For Each damageReceiver As DamageReceiver In Global.UnityEngine.[Object].FindObjectsOfType(Of DamageReceiver)()
			If damageReceiver.gameObject.activeInHierarchy AndAlso damageReceiver.type = DamageReceiver.Type.Enemy AndAlso damageReceiver.transform.position.x >= MyBase.transform.position.x Then
				For Each collider2D2 As Collider2D In damageReceiver.GetComponents(Of Collider2D)()
					If collider2D2.isActiveAndEnabled AndAlso CupheadLevelCamera.Current.ContainsPoint(collider2D2.bounds.center, collider2D2.bounds.size / 2F) Then
						Dim num2 As Single = Mathf.Abs(MathUtils.DirectionToAngle(collider2D2.bounds.center - vector))
						If num2 < num Then
							num = num2
							collider2D = collider2D2
						End If
					End If
				Next
				For Each damageReceiverChild As DamageReceiverChild In damageReceiver.GetComponentsInChildren(Of DamageReceiverChild)()
					For Each collider2D3 As Collider2D In damageReceiverChild.GetComponents(Of Collider2D)()
						If collider2D3.isActiveAndEnabled AndAlso CupheadLevelCamera.Current.ContainsPoint(collider2D3.bounds.center, collider2D3.bounds.size / 2F) Then
							Dim num3 As Single = Mathf.Abs(MathUtils.DirectionToAngle(collider2D3.bounds.center - vector))
							If num3 < num Then
								num = num3
								collider2D = collider2D3
							End If
						End If
					Next
				Next
			End If
		Next
		Me.target = collider2D
		If Me.partner IsNot Nothing Then
			Me.partner.target = collider2D
		End If
	End Sub

	' Token: 0x0600420C RID: 16908 RVA: 0x0023B015 File Offset: 0x00239415
	Public Overrides Sub OnLevelEnd()
	End Sub

	' Token: 0x0400486A RID: 18538
	Private Const Y_OFFSET As Single = 10F

	' Token: 0x0400486B RID: 18539
	Public FreezeTime As Single

	' Token: 0x0400486C RID: 18540
	Private state As PlaneWeaponChalice3WayExProjectile.State

	' Token: 0x0400486D RID: 18541
	Private timeSinceFrozen As Single

	' Token: 0x0400486E RID: 18542
	Private arcTimer As Single

	' Token: 0x0400486F RID: 18543
	Public arcSpeed As Single = 5F

	' Token: 0x04004870 RID: 18544
	Public arcX As Single = 500F

	' Token: 0x04004871 RID: 18545
	Public arcY As Single = 500F

	' Token: 0x04004872 RID: 18546
	Public damageAfterLaunch As Single = 20F

	' Token: 0x04004873 RID: 18547
	Public speedAfterLaunch As Single = 3000F

	' Token: 0x04004874 RID: 18548
	Public accelAfterLaunch As Single = 100F

	' Token: 0x04004875 RID: 18549
	Public minXDistance As Single = 500F

	' Token: 0x04004876 RID: 18550
	Public xDistanceNoTarget As Single = 500F

	' Token: 0x04004877 RID: 18551
	Public ID As Integer

	' Token: 0x04004878 RID: 18552
	Public partner As PlaneWeaponChalice3WayExProjectile

	' Token: 0x04004879 RID: 18553
	Private accelVectorAfterLaunch As Vector3

	' Token: 0x0400487A RID: 18554
	Private velocityAfterLaunch As Vector3

	' Token: 0x0400487B RID: 18555
	Private target As Collider2D

	' Token: 0x0400487C RID: 18556
	Public pauseTime As Single = 0.5F

	' Token: 0x0400487D RID: 18557
	Public vDirection As Single = 1F

	' Token: 0x0400487E RID: 18558
	<SerializeField()>
	Private magnet As SpriteRenderer

	' Token: 0x0400487F RID: 18559
	<SerializeField()>
	Private deathSpark As SpriteRenderer

	' Token: 0x04004880 RID: 18560
	<SerializeField()>
	Private shootFX As Effect

	' Token: 0x04004881 RID: 18561
	<SerializeField()>
	Private smokeFX As Effect

	' Token: 0x04004882 RID: 18562
	<SerializeField()>
	Private sparkleFX As Effect

	' Token: 0x04004883 RID: 18563
	<SerializeField()>
	Private firstSmokeDelay As Single = 0.7F

	' Token: 0x04004884 RID: 18564
	<SerializeField()>
	Private smokeDelay As Single = 0.09F

	' Token: 0x04004885 RID: 18565
	<SerializeField()>
	Private sparkleDelay As Single = 0.15F

	' Token: 0x04004886 RID: 18566
	<SerializeField()>
	Private sparkleRadius As Single = 20F

	' Token: 0x04004887 RID: 18567
	Private smokeTimer As Single

	' Token: 0x04004888 RID: 18568
	Private sparkleTimer As Single

	' Token: 0x02000ABD RID: 2749
	Public Enum State
		' Token: 0x0400488A RID: 18570
		Idle
		' Token: 0x0400488B RID: 18571
		Frozen
		' Token: 0x0400488C RID: 18572
		Paused
		' Token: 0x0400488D RID: 18573
		Launched
	End Enum
End Class
