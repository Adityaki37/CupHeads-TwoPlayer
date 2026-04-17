Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020008C1 RID: 2241
Public Class FunhousePlatformingLevelWall
	Inherits PlatformingLevelBigEnemy

	' Token: 0x1700044D RID: 1101
	' (get) Token: 0x06003455 RID: 13397 RVA: 0x001E5EFC File Offset: 0x001E42FC
	Public ReadOnly Property IsDead As Boolean
		Get
			Return Me.isDead
		End Get
	End Property

	' Token: 0x06003456 RID: 13398 RVA: 0x001E5F04 File Offset: 0x001E4304
	Protected Overrides Sub OnLock()
		MyBase.OnLock()
		MyBase.StartCoroutine(Me.slide_camera_cr())
	End Sub

	' Token: 0x06003457 RID: 13399 RVA: 0x001E5F1C File Offset: 0x001E431C
	Private Iterator Function slide_camera_cr() As IEnumerator
		MyBase.GetComponent(Of Collider2D)().enabled = True
		CupheadLevelCamera.Current.SetAutoScroll(True)
		CupheadLevelCamera.Current.LockCamera(False)
		Dim dist As Single = CupheadLevelCamera.Current.transform.position.x - MyBase.transform.position.x
		While dist < -500F
			dist = CupheadLevelCamera.Current.transform.position.x - MyBase.transform.position.x
			Yield Nothing
		End While
		CupheadLevelCamera.Current.SetAutoScroll(False)
		CupheadLevelCamera.Current.LockCamera(True)
		Yield Nothing
		Return
	End Function

	' Token: 0x06003458 RID: 13400 RVA: 0x001E5F38 File Offset: 0x001E4338
	Protected Overrides Sub Start()
		MyBase.Start()
		Me.LockDistance = 800F
		MyBase.StartCoroutine(Me.shoot_projectiles_cr(MyBase.Properties.funWallTopDelayRange, True))
		MyBase.StartCoroutine(Me.shoot_projectiles_cr(MyBase.Properties.funWallBottomDelayRange, False))
		If Me.isTongue Then
			MyBase.StartCoroutine(Me.spawn_tongue_cr())
		Else
			MyBase.StartCoroutine(Me.spawn_cars_cr())
		End If
		MyBase.GetComponent(Of Collider2D)().enabled = False
	End Sub

	' Token: 0x06003459 RID: 13401 RVA: 0x001E5FBE File Offset: 0x001E43BE
	Protected Overrides Sub Shoot()
		If Me.isDead Then
			Return
		End If
	End Sub

	' Token: 0x0600345A RID: 13402 RVA: 0x001E5FCC File Offset: 0x001E43CC
	Protected Overrides Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		If Me.isDead Then
			Return
		End If
		MyBase.OnDamageTaken(info)
		MyBase.animator.SetTrigger("eyeHit")
		If Not AudioManager.CheckIfPlaying("funhouse_wall1_eye_hit") Then
			AudioManager.Play("funhouse_wall1_eye_hit")
			Me.emitAudioFromObject.Add("funhouse_wall1_eye_hit")
		End If
	End Sub

	' Token: 0x0600345B RID: 13403 RVA: 0x001E6028 File Offset: 0x001E4428
	Private Iterator Function shoot_projectiles_cr(delay As MinMax, isTop As Boolean) As IEnumerator
		While Not Me.bigEnemyCameraLock
			Yield Nothing
		End While
		While True
			Yield CupheadTime.WaitForSeconds(Me, delay.RandomFloat())
			Dim name As String = If((Not isTop), "Bottom", "Top")
			MyBase.animator.SetTrigger("horn" + name)
			AudioManager.Play("funhouse_wall1_horn_attack")
			Me.emitAudioFromObject.Add("funhouse_wall1_horn_attack")
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x0600345C RID: 13404 RVA: 0x001E6054 File Offset: 0x001E4454
	Private Sub ShootProjectileTop()
		Dim [next] As AbstractPlayerController = PlayerManager.GetNext()
		Dim vector As Vector3 = [next].transform.position - Me.topProjectileRoot.transform.position
		Me.hornEffect.Create(Me.topProjectileRoot.transform.position)
		Me.shootProjectile.Create(Me.topProjectileRoot.transform.position, MathUtils.DirectionToAngle(vector), MyBase.Properties.funWallProjectileSpeed)
	End Sub

	' Token: 0x0600345D RID: 13405 RVA: 0x001E60DC File Offset: 0x001E44DC
	Private Sub ShootProjectileBottom()
		Dim [next] As AbstractPlayerController = PlayerManager.GetNext()
		Dim vector As Vector3 = [next].transform.position - Me.bottomProjectileRoot.transform.position
		Me.hornEffect.Create(Me.bottomProjectileRoot.transform.position)
		Me.shootProjectile.Create(Me.bottomProjectileRoot.transform.position, MathUtils.DirectionToAngle(vector), MyBase.Properties.funWallProjectileSpeed)
	End Sub

	' Token: 0x0600345E RID: 13406 RVA: 0x001E6164 File Offset: 0x001E4564
	Private Iterator Function spawn_cars_cr() As IEnumerator
		While Not Me.bigEnemyCameraLock
			Yield Nothing
		End While
		Dim typeIndex As Integer = 0
		Dim isTop As Boolean = Rand.Bool()
		Dim pos As Vector3 = If((Not isTop), Me.bottomTransform.position, Me.topTransform.position)
		While True
			Dim blockage As GameObject = If((Not isTop), Me.mouthBlockageBottom, Me.mouthBlockageTop)
			MyBase.animator.SetBool("isTop", isTop)
			Yield CupheadTime.WaitForSeconds(Me, MyBase.Properties.funWallCarDelayRange.RandomFloat())
			MyBase.animator.SetBool("isOpen", True)
			Dim name As String = If((Not isTop), "Bottom", "Top")
			Yield MyBase.animator.WaitForAnimationToStart(Me, name + "_Open_Start", False)
			AudioManager.Play("funhouse_wall1_wall_open_start")
			Me.emitAudioFromObject.Add("funhouse_wall1_wall_open_start")
			AudioManager.Play("funhouse_car_honk_sweet")
			Me.SpawnHonk(If((Not isTop), Me.bottomTransform.position.y, Me.topTransform.position.y))
			Yield MyBase.animator.WaitForAnimationToEnd(Me, name + "_Open_Start", False, True)
			blockage.SetActive(False)
			For i As Integer = 0 To 2 - 1
				Dim car As FunhousePlatformingLevelCar = Global.UnityEngine.[Object].Instantiate(Of FunhousePlatformingLevelCar)(Me.carPrefab)
				car.Init(pos, 180F, MyBase.Properties.funWallCarSpeed, typeIndex, True, True)
				car.transform.SetScale(Nothing, New Single?(If(isTop, car.transform.localScale.y, (-car.transform.localScale.y))), Nothing)
				typeIndex = If((typeIndex >= 3), 0, (typeIndex + 1))
				Yield CupheadTime.WaitForSeconds(Me, Me.carDelay)
			Next
			Yield CupheadTime.WaitForSeconds(Me, MyBase.Properties.funWallMouthOpenTime)
			MyBase.animator.SetBool("isOpen", False)
			AudioManager.Play("funhouse_wall1_wall_close")
			Me.emitAudioFromObject.Add("funhouse_wall1_wall_close")
			blockage.SetActive(True)
			isTop = Not isTop
			pos = If((Not isTop), Me.bottomTransform.position, Me.topTransform.position)
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x0600345F RID: 13407 RVA: 0x001E6180 File Offset: 0x001E4580
	Private Sub SpawnHonk(rootY As Single)
		Dim vector As Vector2 = New Vector2(CupheadLevelCamera.Current.Bounds.xMax, rootY)
		Me.honkEffect.Create(vector).transform.parent = CupheadLevelCamera.Current.transform
	End Sub

	' Token: 0x06003460 RID: 13408 RVA: 0x001E61CC File Offset: 0x001E45CC
	Private Iterator Function spawn_tongue_cr() As IEnumerator
		While Not Me.bigEnemyCameraLock
			Yield Nothing
		End While
		Dim isTop As Boolean = Rand.Bool()
		Dim pos As Vector3 = If((Not isTop), Me.bottomTransform.position, Me.topTransform.position)
		While True
			Dim blockage As GameObject = If((Not(pos = Me.bottomTransform.position)), Me.mouthBlockageTop, Me.mouthBlockageBottom)
			MyBase.animator.SetBool("isTop", isTop)
			Yield CupheadTime.WaitForSeconds(Me, MyBase.Properties.funWallTongueDelayRange.RandomFloat())
			MyBase.animator.SetBool("isOpen", True)
			Dim name As String = If((Not isTop), "Bottom", "Top")
			Yield CupheadTime.WaitForSeconds(Me, 0.8F)
			MyBase.animator.SetTrigger("Continue")
			Yield MyBase.animator.WaitForAnimationToEnd(Me, name + "_Open_Start", False, True)
			AudioManager.Play("funhouse_wall1_wall_open_start")
			Me.emitAudioFromObject.Add("funhouse_wall1_wall_open_start")
			blockage.SetActive(False)
			Me.tongue.transform.SetScale(Nothing, New Single?(CSng(If((Not isTop), 1, (-1)))), Nothing)
			Me.tongue.transform.position = pos
			Me.tongue.GetComponent(Of Animator)().SetBool("IsTongue", True)
			AudioManager.Play("funhouse_funwall_tounge_intro")
			Me.emitAudioFromObject.Add("funhouse_funwall_tounge_intro")
			Yield CupheadTime.WaitForSeconds(Me, MyBase.Properties.funWallTongueLoopTime)
			Me.tongue.GetComponent(Of Animator)().SetBool("IsTongue", False)
			AudioManager.Play("funhouse_funwall_tounge_outro")
			Me.emitAudioFromObject.Add("funhouse_funwall_tounge_outro")
			Yield Me.tongue.GetComponent(Of Animator)().WaitForAnimationToEnd(Me, "Outro", False, True)
			Yield CupheadTime.WaitForSeconds(Me, MyBase.Properties.funWallMouthOpenTime)
			MyBase.animator.SetBool("isOpen", False)
			AudioManager.Play("funhouse_wall1_wall_close")
			Me.emitAudioFromObject.Add("funhouse_wall1_wall_close")
			blockage.SetActive(True)
			isTop = Not isTop
			pos = If((Not isTop), Me.bottomTransform.position, Me.topTransform.position)
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06003461 RID: 13409 RVA: 0x001E61E7 File Offset: 0x001E45E7
	Protected Overrides Sub OnPass()
		MyBase.OnPass()
		Me.StopAllCoroutines()
		Me.Die()
	End Sub

	' Token: 0x06003462 RID: 13410 RVA: 0x001E61FC File Offset: 0x001E45FC
	Protected Overrides Sub Die()
		Me.isDead = True
		MyBase.GetComponent(Of Collider2D)().enabled = False
		Me.deadBlockage.SetActive(True)
		If CupheadLevelCamera.Current.autoScrolling Then
			CupheadLevelCamera.Current.SetAutoScroll(False)
		End If
		CupheadLevelCamera.Current.LockCamera(False)
		Me.mouthBlockageBottom.SetActive(False)
		Me.mouthBlockageTop.SetActive(False)
		Me.middleBlockage.SetActive(False)
		If Me.tongue IsNot Nothing Then
			Me.tongue.gameObject.SetActive(False)
		End If
		Me.StopAllCoroutines()
		MyBase.StartCoroutine(Me.explode_cr())
		MyBase.GetComponent(Of Collider2D)().enabled = False
		MyBase.animator.SetTrigger("Dead")
		AudioManager.Play("funhouse_wall_death")
		Me.emitAudioFromObject.Add("funhouse_wall_death")
	End Sub

	' Token: 0x06003463 RID: 13411 RVA: 0x001E62DC File Offset: 0x001E46DC
	Private Iterator Function explode_cr() As IEnumerator
		Me.explosion.StartExplosion()
		Yield CupheadTime.WaitForSeconds(Me, 3F)
		Me.explosion.StopExplosions()
		Return
	End Function

	' Token: 0x06003464 RID: 13412 RVA: 0x001E62F7 File Offset: 0x001E46F7
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me.carPrefab = Nothing
		Me.shootProjectile = Nothing
		Me.hornEffect = Nothing
		Me.honkEffect = Nothing
	End Sub

	' Token: 0x04003C85 RID: 15493
	<SerializeField()>
	Private hornEffect As Effect

	' Token: 0x04003C86 RID: 15494
	<SerializeField()>
	Private honkEffect As Effect

	' Token: 0x04003C87 RID: 15495
	<SerializeField()>
	Private isTongue As Boolean

	' Token: 0x04003C88 RID: 15496
	<SerializeField()>
	Private carPrefab As FunhousePlatformingLevelCar

	' Token: 0x04003C89 RID: 15497
	<SerializeField()>
	Private shootProjectile As BasicProjectile

	' Token: 0x04003C8A RID: 15498
	<SerializeField()>
	Private mouthBlockageTop As GameObject

	' Token: 0x04003C8B RID: 15499
	<SerializeField()>
	Private mouthBlockageBottom As GameObject

	' Token: 0x04003C8C RID: 15500
	<SerializeField()>
	Private middleBlockage As GameObject

	' Token: 0x04003C8D RID: 15501
	<SerializeField()>
	Private deadBlockage As GameObject

	' Token: 0x04003C8E RID: 15502
	<SerializeField()>
	Private tongue As Transform

	' Token: 0x04003C8F RID: 15503
	<SerializeField()>
	Private topTransform As Transform

	' Token: 0x04003C90 RID: 15504
	<SerializeField()>
	Private bottomTransform As Transform

	' Token: 0x04003C91 RID: 15505
	<SerializeField()>
	Private topProjectileRoot As Transform

	' Token: 0x04003C92 RID: 15506
	<SerializeField()>
	Private bottomProjectileRoot As Transform

	' Token: 0x04003C93 RID: 15507
	<SerializeField()>
	Private explosion As LevelBossDeathExploder

	' Token: 0x04003C94 RID: 15508
	Private carDelay As Single = 0.7F
End Class
