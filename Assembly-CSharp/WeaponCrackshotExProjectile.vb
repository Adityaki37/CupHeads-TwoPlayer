Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports UnityEngine

' Token: 0x02000A73 RID: 2675
Public Class WeaponCrackshotExProjectile
	Inherits BasicProjectile

	' Token: 0x06003FE1 RID: 16353 RVA: 0x0022D809 File Offset: 0x0022BC09
	Protected Overrides Sub OnDieDistance()
	End Sub

	' Token: 0x06003FE2 RID: 16354 RVA: 0x0022D80B File Offset: 0x0022BC0B
	Protected Overrides Sub OnDieLifetime()
	End Sub

	' Token: 0x17000586 RID: 1414
	' (get) Token: 0x06003FE3 RID: 16355 RVA: 0x0022D80D File Offset: 0x0022BC0D
	Public Overrides ReadOnly Property ParryMeterMultiplier As Single
		Get
			Return If((Me.parryTimeOut <> 0F), 0.1F, 1F)
		End Get
	End Property

	' Token: 0x06003FE4 RID: 16356 RVA: 0x0022D830 File Offset: 0x0022BC30
	Protected Overrides Sub Start()
		MyBase.Start()
		Me._countParryTowardsScore = False
		Me.move = False
		Me.shotNumber = WeaponProperties.LevelWeaponCrackshot.Ex.shotNumber
		MyBase.transform.position += MyBase.transform.right * 120F
		Me.angle = MyBase.transform.eulerAngles.z
		MyBase.transform.eulerAngles = Vector3.zero
		MyBase.transform.localScale = New Vector3(Mathf.Sign(MathUtils.AngleToDirection(Me.angle).x), 1F)
		Me.basePos = MyBase.transform.position
		Me.startPos = MyBase.transform.position
		Me.damageDealer.SetDamage(WeaponProperties.LevelWeaponCrackshot.Ex.collideDamage)
		Me.damageDealer.isDLCWeapon = True
		Me.SetParryable(WeaponProperties.LevelWeaponCrackshot.Ex.isPink)
		AudioManager.FadeSFXVolume("player_weapon_crackshot_turret_loop", 0.0001F, 0.0001F)
	End Sub

	' Token: 0x06003FE5 RID: 16357 RVA: 0x0022D93A File Offset: 0x0022BD3A
	Public Sub GetReplaced()
		Me.LaunchAtTarget()
	End Sub

	' Token: 0x06003FE6 RID: 16358 RVA: 0x0022D944 File Offset: 0x0022BD44
	Private Sub AniEvent_StartSpinSFX()
		AudioManager.Play("player_weapon_crackshot_turret_loop_start")
		Me.emitAudioFromObject.Add("player_weapon_crackshot_turret_loop_start")
		AudioManager.PlayLoop("player_weapon_crackshot_turret_loop")
		Me.emitAudioFromObject.Add("player_weapon_crackshot_turret_loop")
		AudioManager.FadeSFXVolumeLinear("player_weapon_crackshot_turret_loop", 0.3F, 1F)
	End Sub

	' Token: 0x06003FE7 RID: 16359 RVA: 0x0022D999 File Offset: 0x0022BD99
	Public Overrides Sub OnParry(player As AbstractPlayerController)
		If Not Me.parried Then
			Me.LaunchAtTarget()
		Else
			Me.Die()
		End If
	End Sub

	' Token: 0x06003FE8 RID: 16360 RVA: 0x0022D9B8 File Offset: 0x0022BDB8
	Private Sub LaunchAtTarget()
		MyBase.animator.Play("Launch")
		AudioManager.[Stop]("player_weapon_crackshot_turret_loop")
		AudioManager.Play("player_weapon_crackshot_turret_parry")
		Me.emitAudioFromObject.Add("player_weapon_crackshot_turret_parry")
		Dim collider2D As Collider2D = Me.FindTarget()
		If collider2D Then
			Me.angle = MathUtils.DirectionToAngle(collider2D.bounds.center - MyBase.transform.position)
		End If
		Dim effect As Effect = Me.launchFXPrefab.Create(MyBase.transform.position)
		effect.transform.eulerAngles = New Vector3(0F, 0F, Me.angle)
		effect.transform.localScale = New Vector3(-1F, CSng(MathUtils.PlusOrMinus()))
		Me.parried = True
		MyBase.transform.SetEulerAngles(New Single?(0F), New Single?(0F), New Single?(Me.angle))
		MyBase.transform.localScale = New Vector3(-1F, 1F)
		Me.Speed = WeaponProperties.LevelWeaponCrackshot.Ex.parryBulletSpeed
		Me.damageDealer.SetDamage(WeaponProperties.LevelWeaponCrackshot.Ex.parryBulletDamage)
		Me.SetParryable(False)
		Me.parryTimeOut = WeaponProperties.LevelWeaponCrackshot.Ex.parryTimeOut
		Me.move = True
	End Sub

	' Token: 0x06003FE9 RID: 16361 RVA: 0x0022DB0C File Offset: 0x0022BF0C
	Protected Overrides Sub Die()
		MyBase.animator.Play("Explode")
		AudioManager.[Stop]("player_weapon_crackshot_turret_parry")
		AudioManager.Play("player_weapon_crackshot_turret_parryexplode")
		Me.emitAudioFromObject.Add("player_weapon_crackshot_turret_parryexplode")
		MyBase.transform.eulerAngles = New Vector3(0F, 0F, CSng(Global.UnityEngine.Random.Range(0, 360)))
		MyBase.transform.localScale = New Vector3(CSng(MathUtils.PlusOrMinus()), CSng(MathUtils.PlusOrMinus()))
		Me.move = False
		Me.coll.enabled = False
	End Sub

	' Token: 0x06003FEA RID: 16362 RVA: 0x0022DBA2 File Offset: 0x0022BFA2
	Private Sub _OnDieAnimComplete()
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub

	' Token: 0x06003FEB RID: 16363 RVA: 0x0022DBAF File Offset: 0x0022BFAF
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		AudioManager.FadeSFXVolume("player_weapon_crackshot_turret_loop", 0.0001F, 0.25F)
	End Sub

	' Token: 0x06003FEC RID: 16364 RVA: 0x0022DBCC File Offset: 0x0022BFCC
	Private Sub HandleShot()
		Me.shootTimer -= CupheadTime.FixedDelta
		If Me.shootTimer <= 0F Then
			Dim collider2D As Collider2D = Me.FindTarget()
			If collider2D Then
				Dim meterScoreTracker As MeterScoreTracker = New MeterScoreTracker(MeterScoreTracker.Type.Ex)
				Dim basicProjectile As BasicProjectile = Me.childPrefab.Create(MyBase.transform.position, MathUtils.DirectionToAngle(collider2D.bounds.center - MyBase.transform.position), WeaponProperties.LevelWeaponCrackshot.Ex.bulletSpeed)
				Me.childPrefab.Damage = WeaponProperties.LevelWeaponCrackshot.Ex.bulletDamage
				Me.childPrefab.Speed = WeaponProperties.LevelWeaponCrackshot.Ex.bulletSpeed
				Me.childPrefab.PlayerId = Me.PlayerId
				meterScoreTracker.Add(basicProjectile)
				MyBase.StartCoroutine(Me.shoot_stretch_squash_cr())
				Me.shootFXPrefab.Create(MyBase.transform.position + (collider2D.bounds.center - MyBase.transform.position).normalized * 25F)
				AudioManager.Play("player_weapon_crackshot_turret_shoot")
				Me.emitAudioFromObject.Add("player_weapon_crackshot_turret_shoot")
			End If
			Me.shotNumber -= 1
			If Me.shotNumber = 0 Then
				MyBase.animator.SetTrigger("Disappear")
				Me.coll.enabled = False
			Else
				Me.shootTimer += WeaponProperties.LevelWeaponCrackshot.Ex.shootDelay
			End If
		End If
	End Sub

	' Token: 0x06003FED RID: 16365 RVA: 0x0022DD58 File Offset: 0x0022C158
	Private Iterator Function shoot_stretch_squash_cr() As IEnumerator
		MyBase.transform.localScale = New Vector3(Mathf.Sign(MyBase.transform.localScale.x) * 1.2F, Mathf.Sign(MyBase.transform.localScale.y) * 1.2F)
		Yield CupheadTime.WaitForSeconds(Me, 0.041666668F)
		MyBase.transform.localScale = New Vector3(Mathf.Sign(MyBase.transform.localScale.x) * 1.25F, Mathf.Sign(MyBase.transform.localScale.y) * 1.25F)
		Yield CupheadTime.WaitForSeconds(Me, 0.041666668F)
		MyBase.transform.localScale = New Vector3(Mathf.Sign(MyBase.transform.localScale.x) * 1.22F, Mathf.Sign(MyBase.transform.localScale.y) * 1.22F)
		Yield CupheadTime.WaitForSeconds(Me, 0.041666668F)
		MyBase.transform.localScale = New Vector3(Mathf.Sign(MyBase.transform.localScale.x) * 1.16F, Mathf.Sign(MyBase.transform.localScale.y) * 1.16F)
		Yield CupheadTime.WaitForSeconds(Me, 0.041666668F)
		MyBase.transform.localScale = New Vector3(Mathf.Sign(MyBase.transform.localScale.x), Mathf.Sign(MyBase.transform.localScale.y))
		Return
	End Function

	' Token: 0x06003FEE RID: 16366 RVA: 0x0022DD74 File Offset: 0x0022C174
	Protected Overrides Sub FixedUpdate()
		MyBase.FixedUpdate()
		If Me.parried Then
			If Me.parryTimeOut > 0F Then
				Me.parryTimeOut -= CupheadTime.FixedDelta
				If Me.parryTimeOut <= 0F Then
					Me.parryTimeOut = 0F
					Me.SetParryable(True)
				End If
			End If
			Return
		End If
		If MyBase.lifetime < WeaponProperties.LevelWeaponCrackshot.Ex.timeToHoverPoint Then
			Me.basePos = Vector3.Lerp(Me.startPos, Me.startPos + MathUtils.AngleToDirection(Me.angle) * WeaponProperties.LevelWeaponCrackshot.Ex.launchDistance, EaseUtils.EaseOutSine(0F, 1F, MyBase.lifetime / WeaponProperties.LevelWeaponCrackshot.Ex.timeToHoverPoint))
		Else
			If Not Me.timerSet Then
				Me.timerSet = True
				Me.basePos = Me.startPos + MathUtils.AngleToDirection(Me.angle) * WeaponProperties.LevelWeaponCrackshot.Ex.launchDistance
				Me.shootTimer = WeaponProperties.LevelWeaponCrackshot.Ex.shootDelay
			End If
			Me.basePos += Vector3.up * WeaponProperties.LevelWeaponCrackshot.Ex.riseSpeed * CupheadTime.FixedDelta
			Me.HandleShot()
		End If
		If Me.shotNumber > 0 Then
			Dim num As Single = MyBase.lifetime * WeaponProperties.LevelWeaponCrackshot.Ex.hoverSpeed
			MyBase.transform.position = Me.basePos + New Vector3(Mathf.Cos(num + 1.5707964F) * WeaponProperties.LevelWeaponCrackshot.Ex.hoverWidth * -Mathf.Sign(MathUtils.AngleToDirection(Me.angle).x), Mathf.Sin(num * 2F) * WeaponProperties.LevelWeaponCrackshot.Ex.hoverHeight)
		End If
	End Sub

	' Token: 0x06003FEF RID: 16367 RVA: 0x0022DF27 File Offset: 0x0022C327
	Public Function FindTarget() As Collider2D
		Return Me.findBestTarget(AbstractProjectile.FindOverlapScreenDamageReceivers())
	End Function

	' Token: 0x06003FF0 RID: 16368 RVA: 0x0022DF34 File Offset: 0x0022C334
	Private Function findBestTarget(damageReceivers As IEnumerable(Of DamageReceiver)) As Collider2D
		Dim num As Single = Single.MaxValue
		Dim collider2D As Collider2D = Nothing
		Dim vector As Vector2 = MyBase.transform.position
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

	' Token: 0x040046BA RID: 18106
	<SerializeField()>
	Private childPrefab As WeaponCrackshotExProjectileChild

	' Token: 0x040046BB RID: 18107
	<SerializeField()>
	Private shootFXPrefab As Effect

	' Token: 0x040046BC RID: 18108
	<SerializeField()>
	Private launchFXPrefab As Effect

	' Token: 0x040046BD RID: 18109
	<SerializeField()>
	Private coll As Collider2D

	' Token: 0x040046BE RID: 18110
	Private basePos As Vector3

	' Token: 0x040046BF RID: 18111
	Private startPos As Vector3

	' Token: 0x040046C0 RID: 18112
	Private shotNumber As Integer = 5

	' Token: 0x040046C1 RID: 18113
	Private shootTimer As Single

	' Token: 0x040046C2 RID: 18114
	Private timerSet As Boolean

	' Token: 0x040046C3 RID: 18115
	Private parried As Boolean

	' Token: 0x040046C4 RID: 18116
	Private parryTimeOut As Single

	' Token: 0x040046C5 RID: 18117
	Private angle As Single
End Class
