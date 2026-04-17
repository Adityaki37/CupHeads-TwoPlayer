Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000653 RID: 1619
Public Class FlyingCowboyLevelMeat
	Inherits LevelProperties.FlyingCowboy.Entity

	' Token: 0x060021A6 RID: 8614 RVA: 0x001386EC File Offset: 0x00136AEC
	Private Sub Start()
		AddHandler Level.Current.OnBossDeathExplosionsEvent, AddressOf Me.onBossDeathExplosionsEventHandler
		Me.damageDealer = DamageDealer.NewEnemy()
		Me.damageReceiver = MyBase.GetComponent(Of DamageReceiver)()
		AddHandler Me.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
		Me.nextBulletSpawnPointA.position = Me.sausageHolderA.position
		Me.nextBulletSpawnPointB.position = Me.sausageHolderB.position
	End Sub

	' Token: 0x060021A7 RID: 8615 RVA: 0x00138769 File Offset: 0x00136B69
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x060021A8 RID: 8616 RVA: 0x00138788 File Offset: 0x00136B88
	Public Overrides Sub LevelInit(properties As LevelProperties.FlyingCowboy)
		MyBase.LevelInit(properties)
		Me.runningSpitBulletParryPattern = New PatternString(properties.CurrentState.sausageRun.bulletParry, True)
		Me.sausageTimeToMoveString = New PatternString(properties.CurrentState.sausageRun.timeTillSwitch, True, True)
		Me.spitBulletParryString = New PatternString(properties.CurrentState.can.bulletParryString, True)
	End Sub

	' Token: 0x060021A9 RID: 8617 RVA: 0x001387F1 File Offset: 0x00136BF1
	Private Sub Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x060021AA RID: 8618 RVA: 0x0013880C File Offset: 0x00136C0C
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		If Me.meatPhase = FlyingCowboyLevelMeat.MeatPhase.Can Then
			AudioManager.Play("sfx_dlc_cowgirl_p3_can_damage_metalimpact")
		End If
		MyBase.properties.DealDamage(info.damage)
		If Not Me.isDead AndAlso MyBase.properties.CurrentHealth <= 0F Then
			Me.die()
		End If
	End Sub

	' Token: 0x060021AB RID: 8619 RVA: 0x00138865 File Offset: 0x00136C65
	Public Sub SelectPhase(meatPhase As FlyingCowboyLevelMeat.MeatPhase)
		MyBase.gameObject.SetActive(True)
		Me.meatPhase = meatPhase
		If meatPhase <> FlyingCowboyLevelMeat.MeatPhase.Can Then
			If meatPhase = FlyingCowboyLevelMeat.MeatPhase.Sausage Then
				Me.Sausage()
			End If
		Else
			Me.Can()
		End If
	End Sub

	' Token: 0x060021AC RID: 8620 RVA: 0x001388A4 File Offset: 0x00136CA4
	Public Sub Sausage()
		Dim position As Vector3 = Me.sausageSpawnPosition.position
		position.y = 42F
		MyBase.transform.position = position
		MyBase.StartCoroutine(Me.sausage_intro_cr())
	End Sub

	' Token: 0x060021AD RID: 8621 RVA: 0x001388E4 File Offset: 0x00136CE4
	Private Iterator Function sausage_intro_cr() As IEnumerator
		Dim p As LevelProperties.FlyingCowboy.SausageRun = MyBase.properties.CurrentState.sausageRun
		Yield CupheadTime.WaitForSeconds(Me, p.mirrorTime)
		MyBase.animator.SetTrigger("Continue")
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Sg_Mirror_Cont", False, True)
		MyBase.StartCoroutine(Me.beans_cr())
		If p.shootBullets Then
			MyBase.StartCoroutine(Me.sausageTurret_cr())
		End If
		MyBase.StartCoroutine(Me.sausageSwitchHeight_cr())
		Return
	End Function

	' Token: 0x060021AE RID: 8622 RVA: 0x001388FF File Offset: 0x00136CFF
	Private Sub animationEvent_RepositionSausage()
		MyBase.StartCoroutine(Me.repositionSausage_cr())
	End Sub

	' Token: 0x060021AF RID: 8623 RVA: 0x00138910 File Offset: 0x00136D10
	Private Iterator Function repositionSausage_cr() As IEnumerator
		AudioManager.PlayLoop("sfx_dlc_cowgirl_p3_sausage_footstep_loop")
		Dim startX As Single = MyBase.transform.position.x
		Dim elapsedTime As Single = 0F
		While Me.meatPhase = FlyingCowboyLevelMeat.MeatPhase.Sausage AndAlso elapsedTime < 4F
			Yield Nothing
			elapsedTime += CupheadTime.Delta
			Dim position As Vector3 = MyBase.transform.position
			position.x = Mathf.Lerp(startX, 340F, elapsedTime / 4F)
			MyBase.transform.position = position
		End While
		MyBase.StartCoroutine(Me.wobble_cr(MyBase.transform, Me.sausageWobbleRadius, Me.sausageWobbleDuration, MyBase.transform.position, FlyingCowboyLevelMeat.MeatPhase.Sausage, False, False))
		Return
	End Function

	' Token: 0x060021B0 RID: 8624 RVA: 0x0013892C File Offset: 0x00136D2C
	Private Iterator Function sausageSwitchHeight_cr() As IEnumerator
		While True
			Dim time As Single = Me.sausageTimeToMoveString.PopFloat()
			Yield CupheadTime.WaitForSeconds(Me, time)
			Dim newFlyingStatus As Boolean = Not Me.isFlying
			MyBase.animator.SetBool("IsFlying", newFlyingStatus)
			If newFlyingStatus Then
				AudioManager.[Stop]("sfx_dlc_cowgirl_p3_sausage_footstep_loop")
			End If
			Dim transitionAnimation As String = If((Not newFlyingStatus), "Sg_Fly_To_Run", "Sg_Run_To_Fly")
			Yield MyBase.animator.WaitForAnimationToEnd(Me, transitionAnimation, False, True)
			If Not newFlyingStatus Then
				AudioManager.PlayLoop("sfx_dlc_cowgirl_p3_sausage_footstep_loop")
			End If
			Me.isFlying = newFlyingStatus
		End While
		Return
	End Function

	' Token: 0x060021B1 RID: 8625 RVA: 0x00138948 File Offset: 0x00136D48
	Private Iterator Function beans_cr() As IEnumerator
		Dim startBeansHealthPercentage As Single = MyBase.properties.CurrentState.healthTrigger
		Dim endBeansPercentage As Single = MyBase.properties.GetNextStateHealthTrigger()
		Dim startBeansHealth As Single = startBeansHealthPercentage * MyBase.properties.TotalHealth
		Dim endBeansHealth As Single = endBeansPercentage * MyBase.properties.TotalHealth
		Dim seventyFivePercentof As Single = startBeansHealth + (endBeansHealth - startBeansHealth) * 0.75F
		Dim p As LevelProperties.FlyingCowboy.SausageRun = MyBase.properties.CurrentState.sausageRun
		Dim groupDelayPattern As PatternString = New PatternString(p.groupBeansDelayString, True, True)
		Dim positionPattern As PatternString = New PatternString(p.beansPositionString, True, False)
		Dim extendTimerPattern As PatternString = New PatternString(p.beansExtendTimer, True)
		Dim positionX As Single = 690F
		While Me.meatPhase = FlyingCowboyLevelMeat.MeatPhase.Sausage
			Dim positionValues As String() = positionPattern.GetString().Split(New Char() { ":"c })
			positionPattern.IncrementString()
			Dim positionY As Single
			Parser.FloatTryParse(positionValues(0), positionY)
			Dim pointingUp As Boolean = positionValues(1) = "U"
			Dim currentPercentage As Single = (MyBase.properties.CurrentHealth - startBeansHealth) / (seventyFivePercentof - startBeansHealth)
			Dim speed As Single = Mathf.Lerp(p.beansSpeed.min, p.beansSpeed.max, currentPercentage)
			Dim beans As FlyingCowboyLevelBeans = Me.beansPrefab.Spawn()
			beans.Init(New Vector3(positionX, positionY), pointingUp, speed, extendTimerPattern.PopFloat())
			If positionPattern.GetSubStringIndex() <> 0 Then
				Dim spawnDelay As Single = Mathf.Lerp(p.beansSpawnDelay.max, p.beansSpawnDelay.min, currentPercentage)
				Yield CupheadTime.WaitForSeconds(Me, spawnDelay)
			Else
				Yield CupheadTime.WaitForSeconds(Me, groupDelayPattern.PopFloat())
			End If
		End While
		Return
	End Function

	' Token: 0x060021B2 RID: 8626 RVA: 0x00138964 File Offset: 0x00136D64
	Private Iterator Function sausageTurret_cr() As IEnumerator
		Dim p As LevelProperties.FlyingCowboy.SausageRun = MyBase.properties.CurrentState.sausageRun
		Dim player As AbstractPlayerController = PlayerManager.GetNext()
		While Me.meatPhase = FlyingCowboyLevelMeat.MeatPhase.Sausage
			MyBase.animator.SetTrigger("OnShoot")
			Me.waitingToShoot = True
			While Me.waitingToShoot
				Yield Nothing
			End While
			Yield CupheadTime.WaitForSeconds(Me, p.bulletDelay)
		End While
		Return
	End Function

	' Token: 0x060021B3 RID: 8627 RVA: 0x00138980 File Offset: 0x00136D80
	Private Sub aniEvent_ShootTurret()
		Me.player = PlayerManager.GetNext()
		Dim sausageRun As LevelProperties.FlyingCowboy.SausageRun = MyBase.properties.CurrentState.sausageRun
		Dim vector As Vector3 = If((Not Me.isFlying), Me.runBottomSpitBulletSpawn.position, Me.runTopSpitBulletSpawn.position)
		Dim vector2 As Vector3 = If((Not Me.isFlying), Me.runBottomSpitBulletEffectSpawn.position, Me.runTopSpitBulletEffectSpawn.position)
		Me.player = PlayerManager.GetNext()
		Dim vector3 As Vector3 = Me.player.transform.position - vector
		Dim num As Single
		num = MathUtils.DirectionToAngle(vector3)
		While num < 0F
			num += 360F
		End While
		Dim num2 As Single
		Dim num3 As Single
		Dim flag As Boolean
		If Me.isFlying Then
			num2 = 180F - sausageRun.bulletTopMaxUpAngle
			num3 = 180F + sausageRun.bulletTopMaxDownAngle
			flag = sausageRun.bulletTopRotateClockwise
		Else
			num2 = 180F - sausageRun.bulletBottomMaxUpAngle
			num3 = 180F + sausageRun.bulletBottomMaxDownAngle
			flag = sausageRun.bulletBottomRotateClockwise
		End If
		num = Mathf.Clamp(num, num2, num3)
		vector3 = MathUtilities.AngleToDirection(num)
		Me.sausageRunSpitBullet.Create(vector, sausageRun.bulletSpeed, sausageRun.bulletRotationSpeed, sausageRun.bulletRotationRadius, vector3, flag, Me.runningSpitBulletParryPattern.PopLetter() = "P"c)
		Dim effect As Effect = Me.sausageRunSpitBulletEffect.Create(vector2)
		If Not Me.isFlying Then
			effect.transform.rotation = Quaternion.Euler(0F, 0F, -30F)
		End If
		Me.waitingToShoot = False
	End Sub

	' Token: 0x060021B4 RID: 8628 RVA: 0x00138B29 File Offset: 0x00136F29
	Public Sub Can()
		MyBase.StartCoroutine(Me.toCan_cr())
	End Sub

	' Token: 0x060021B5 RID: 8629 RVA: 0x00138B38 File Offset: 0x00136F38
	Private Iterator Function toCan_cr() As IEnumerator
		AudioManager.[Stop]("sfx_dlc_cowgirl_p3_sausage_footstep_loop")
		MyBase.animator.SetBool("ToCan", True)
		Yield MyBase.animator.WaitForNormalizedTime(Me, 1F, "SausageToCanEnd", 0, True, False, True)
		MyBase.animator.Play("CanIntro", 0)
		MyBase.animator.Update(0F)
		MyBase.StartCoroutine(Me.repositionCan_cr())
		Dim p As LevelProperties.FlyingCowboy.Can = MyBase.properties.CurrentState.can
		MyBase.StartCoroutine(Me.wobble_cr(Me.canTransform, New Vector2(p.wobbleRadiusX, p.wobbleRadiusY), New Vector2(p.wobbleDurationX, p.wobbleDurationY), Me.canTransform.localPosition, FlyingCowboyLevelMeat.MeatPhase.Can, True, True))
		Return
	End Function

	' Token: 0x060021B6 RID: 8630 RVA: 0x00138B54 File Offset: 0x00136F54
	Private Iterator Function repositionCan_cr() As IEnumerator
		Dim startPosition As Vector3 = MyBase.transform.position
		Dim targetPosition As Vector3 = New Vector3(340F, 61F, startPosition.z)
		Dim elapsedTime As Single = 0F
		While elapsedTime < 3F
			Yield Nothing
			elapsedTime += CupheadTime.Delta
			MyBase.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / 3F)
		End While
		Return
	End Function

	' Token: 0x060021B7 RID: 8631 RVA: 0x00138B70 File Offset: 0x00136F70
	Private Sub animationEvent_StartSausageLinks()
		AudioManager.PlayLoop("sfx_dlc_cowgirl_p3_sausagemeattin_loop")
		Me.emitAudioFromObject.Add("sfx_dlc_cowgirl_p3_sausagemeattin_loop")
		Me.sausageTransforms.SetActive(True)
		MyBase.StartCoroutine(Me.sausageTrain_cr(True))
		MyBase.StartCoroutine(Me.sausageRotation_cr(Me.sausageHolderA, 0))
		MyBase.StartCoroutine(Me.sausageTrain_cr(False))
		MyBase.StartCoroutine(Me.sausageRotation_cr(Me.sausageHolderB, 1))
		If MyBase.properties.CurrentState.can.shootBullets Then
			MyBase.StartCoroutine(Me.shootCanBullets_cr())
		End If
		MyBase.StartCoroutine(Me.beanCanTriggerZone_cr())
	End Sub

	' Token: 0x060021B8 RID: 8632 RVA: 0x00138C1B File Offset: 0x0013701B
	Private Sub animationEvent_TriggerCanBullets()
		Me.canBulletsTriggered = True
	End Sub

	' Token: 0x060021B9 RID: 8633 RVA: 0x00138C24 File Offset: 0x00137024
	Private Iterator Function shootCanBullets_cr() As IEnumerator
		Dim p As LevelProperties.FlyingCowboy.Can = MyBase.properties.CurrentState.can
		Dim [variant] As Integer = 0
		Dim fxVariant As Integer = Global.UnityEngine.Random.Range(0, 3)
		Dim bulletCountPattern As PatternString = New PatternString(p.bulletCount, True, True)
		Me.canBulletsTriggered = False
		While Me.meatPhase = FlyingCowboyLevelMeat.MeatPhase.Can
			Yield CupheadTime.WaitForSeconds(Me, p.shotDelay)
			MyBase.animator.SetTrigger("OnShoot")
			While Not Me.canBulletsTriggered
				Yield Nothing
			End While
			Me.canBulletsTriggered = False
			Dim muzzleFX As Effect = Me.canBulletMuzzleFX.Create(Me.bulletRoot.position)
			muzzleFX.animator.SetInteger("Effect", fxVariant)
			fxVariant = MathUtilities.NextIndex(fxVariant, 3)
			Me.SFX_CanSpitBurningFire()
			While Not Me.canBulletsTriggered
				Yield Nothing
			End While
			Me.canBulletsTriggered = False
			Dim count As Integer = bulletCountPattern.PopInt()
			Dim startAngle As Single = -p.bulletSpreadAngle * 0.5F
			Dim angleIncrement As Single = p.bulletSpreadAngle / CSng((count - 1))
			For i As Integer = 0 To count - 1
				Dim num As Single = startAngle + angleIncrement * CSng(i)
				Dim num2 As Single = 180F - num
				Dim basicProjectile As BasicProjectile = Me.canBullet.Create(Me.bulletRoot.position, num2, p.bulletSpeed)
				Dim flag As Boolean = Me.spitBulletParryString.PopLetter() = "P"c
				basicProjectile.SetParryable(flag)
				basicProjectile.animator.SetInteger("Variant", [variant])
				basicProjectile.animator.Update(0F)
				basicProjectile.animator.Play(0, 0, Global.UnityEngine.Random.Range(0F, 1F))
				basicProjectile.GetComponent(Of SpriteRenderer)().sortingOrder = i
				basicProjectile.transform.SetEulerAngles(New Single?(0F), New Single?(0F), New Single?(-num))
				If Not flag Then
					[variant] = If(([variant] <> 0), 0, 1)
				End If
			Next
		End While
		Me.sausageTransforms.SetActive(False)
		Return
	End Function

	' Token: 0x060021BA RID: 8634 RVA: 0x00138C40 File Offset: 0x00137040
	Private Iterator Function sausageRotation_cr(sausageHolder As Transform, index As Integer) As IEnumerator
		Dim p As LevelProperties.FlyingCowboy.Can = MyBase.properties.CurrentState.can
		Dim holder As Transform = If((index <> 0), Me.sausageHolderB, Me.sausageHolderA)
		Dim topAngle As Single = -p.maxSausageAngle
		Dim bottomAngle As Single = p.maxSausageAngle
		Dim goingUp As Boolean = index = 0
		Dim startAngle As Single = If((Not goingUp), bottomAngle, 0F)
		Dim endAngle As Single = If((Not goingUp), 0F, topAngle)
		Dim sortingOffset As Integer = index * 100
		Dim elapsedTime As Single = 0F
		Dim wait As WaitForFixedUpdate = New WaitForFixedUpdate()
		While Me.meatPhase = FlyingCowboyLevelMeat.MeatPhase.Can
			While elapsedTime < 2F
				If Me.meatPhase <> FlyingCowboyLevelMeat.MeatPhase.Can Then
					Exit While
				End If
				Dim t As Single = If((Not goingUp), (1F - elapsedTime / 2F), (elapsedTime / 2F))
				Dim angle As Single = Mathf.Lerp(startAngle, endAngle, t)
				sausageHolder.transform.SetEulerAngles(Nothing, Nothing, New Single?(angle))
				Dim sortingOrder As Integer
				If angle >= 15F Then
					sortingOrder = FlyingCowboyLevelMeat.LowSausageLinkSortingOrder + sortingOffset
				ElseIf angle <= -15F Then
					sortingOrder = FlyingCowboyLevelMeat.HighSausageLinkSortingOrder + sortingOffset
				Else
					sortingOrder = FlyingCowboyLevelMeat.MidSausageLinkSortingOrder + sortingOffset
				End If
				Dim childCount As Integer = holder.childCount
				For i As Integer = 0 To childCount - 1
					Dim child As Transform = holder.GetChild(i)
					Dim component As SpriteRenderer = child.GetComponent(Of SpriteRenderer)()
					If component IsNot Nothing Then
						component.sortingOrder = sortingOrder + childCount - i
					End If
				Next
				If index = 0 Then
					Me.currentSausageLinkSortingOrderA = sortingOrder
				ElseIf index = 1 Then
					Me.currentSausageLinkSortingOrderB = sortingOrder
				End If
				elapsedTime += CupheadTime.FixedDelta
				Yield wait
			End While
			If(goingUp AndAlso startAngle = 0F) OrElse (Not goingUp AndAlso endAngle = 0F) Then
				goingUp = Not goingUp
			ElseIf Not goingUp AndAlso startAngle = 0F Then
				startAngle = bottomAngle
				endAngle = 0F
			ElseIf goingUp AndAlso startAngle = bottomAngle Then
				startAngle = 0F
				endAngle = topAngle
			End If
			elapsedTime = 0F
		End While
		Return
	End Function

	' Token: 0x060021BB RID: 8635 RVA: 0x00138C6C File Offset: 0x0013706C
	Private Iterator Function sausageTrain_cr(isTypeA As Boolean) As IEnumerator
		Dim p As LevelProperties.FlyingCowboy.Can = MyBase.properties.CurrentState.can
		Dim sausageHolder As Transform = If((Not isTypeA), Me.sausageHolderB, Me.sausageHolderA)
		Dim nextSpawn As Transform = If((Not isTypeA), Me.nextBulletSpawnPointB, Me.nextBulletSpawnPointA)
		Dim sausageMainString As String() = If((Not isTypeA), p.sausageStringB, p.sausageStringA)
		Dim sausageAmountPattern As PatternString = New PatternString(sausageMainString, True, True)
		Dim gapPattern As PatternString = New PatternString(If((Not isTypeA), p.gapDistB, p.gapDistA), True, True)
		Dim sausageCounter As Integer = 0
		Dim sausageMax As Integer = sausageAmountPattern.PopInt()
		Dim previousSausageType As FlyingCowboyLevelMeat.SausageType = FlyingCowboyLevelMeat.SausageType.H1
		Dim previousSausage As FlyingCowboyLevelSausageLink = Nothing
		AudioManager.PlayLoop("sfx_dlc_cowgirl_p3_sausagemeattin_loop")
		Me.emitAudioFromObject.Add("sfx_dlc_cowgirl_p3_sausagemeattin_loop")
		While Me.meatPhase = FlyingCowboyLevelMeat.MeatPhase.Can
			If sausageCounter < sausageMax Then
				Dim flag As Boolean = False
				Dim sausageType As FlyingCowboyLevelMeat.SausageType
				If previousSausageType = FlyingCowboyLevelMeat.SausageType.U1 OrElse previousSausageType = FlyingCowboyLevelMeat.SausageType.U2 OrElse previousSausageType = FlyingCowboyLevelMeat.SausageType.U3 Then
					sausageType = FlyingCowboyLevelMeat.SausageTypeDown.RandomChoice()
					flag = True
				ElseIf sausageMax - sausageCounter < 2 Then
					sausageType = FlyingCowboyLevelMeat.SausageTypeEnd.RandomChoice()
				Else
					sausageType = previousSausageType
					While sausageType = previousSausageType
						sausageType = FlyingCowboyLevelMeat.SausageTypeAny.RandomChoice()
					End While
				End If
				previousSausageType = sausageType
				sausageCounter += 1
				Dim flyingCowboyLevelSausageLink As FlyingCowboyLevelSausageLink = TryCast(Me.sausage.Create(nextSpawn.position, sausageHolder.transform.eulerAngles.z, -p.sausageTrainSpeed), FlyingCowboyLevelSausageLink)
				flyingCowboyLevelSausageLink.transform.parent = sausageHolder
				flyingCowboyLevelSausageLink.Initialize(sausageType, Me.sausageLinkSqueezePoint, If((Not flag), Nothing, previousSausage))
				If flag Then
					flyingCowboyLevelSausageLink.animator.Play("SqueezeLoopDown")
				End If
				previousSausage = flyingCowboyLevelSausageLink
				nextSpawn.parent = flyingCowboyLevelSausageLink.transform
				nextSpawn.localPosition = New Vector3(FlyingCowboyLevelMeat.SausageLinkWidth, 0F)
				Dim component As SpriteRenderer = flyingCowboyLevelSausageLink.GetComponent(Of SpriteRenderer)()
				component.sortingOrder = If((Not isTypeA), Me.currentSausageLinkSortingOrderB, Me.currentSausageLinkSortingOrderA)
			Else
				Dim num As Integer = gapPattern.PopInt() - 1
				Dim num2 As Single = FlyingCowboyLevelMeat.SausageGapWidths(num)
				nextSpawn.localPosition = New Vector3(num2, 0F)
				Dim basicProjectile As BasicProjectile = Me.sausageString.Create(nextSpawn.position, sausageHolder.transform.eulerAngles.z, -p.sausageTrainSpeed)
				basicProjectile.animator.Play(FlyingCowboyLevelMeat.SausageGapAnimationNames(num))
				Dim component2 As SpriteRenderer = basicProjectile.GetComponent(Of SpriteRenderer)()
				component2.sortingOrder = If((Not isTypeA), Me.currentSausageLinkSortingOrderB, Me.currentSausageLinkSortingOrderA)
				basicProjectile.transform.parent = sausageHolder
				nextSpawn.parent = basicProjectile.transform
				nextSpawn.localPosition = New Vector3(FlyingCowboyLevelMeat.SausageLinkWidth, 0F)
				sausageCounter = 0
				sausageMax = sausageAmountPattern.PopInt()
			End If
			While nextSpawn.position.x > sausageHolder.position.x + 175F
				Yield Nothing
			End While
		End While
		Return
	End Function

	' Token: 0x060021BC RID: 8636 RVA: 0x00138C90 File Offset: 0x00137090
	Private Iterator Function beanCanTriggerZone_cr() As IEnumerator
		Dim p As LevelProperties.FlyingCowboy.Can = MyBase.properties.CurrentState.can
		Dim extendTimerPattern As PatternString = New PatternString(p.beanCanExtendTimer, True)
		Dim topSpawnPattern As PatternString = New PatternString(p.beanCanPostionUpper, True, True)
		Dim bottomSpawnPattern As PatternString = New PatternString(p.beanCanPositionLower, True, True)
		Dim timers As Single() = New Single(Me.beanCanTriggerZones.Length - 1) {}
		While True
			Yield Nothing
			For i As Integer = 0 To Me.beanCanTriggerZones.Length - 1
				Dim flag As Boolean = False
				Dim vector As Vector3 = Vector3.zero
				For Each abstractPlayerController As AbstractPlayerController In PlayerManager.GetAllPlayers()
					Dim triggerZone As TriggerZone = Me.beanCanTriggerZones(i)
					If abstractPlayerController IsNot Nothing AndAlso triggerZone.Contains(abstractPlayerController.center) Then
						timers(i) += CupheadTime.Delta
						vector = abstractPlayerController.center
						flag = True
						Exit For
					End If
				Next
				If Not flag Then
					timers(i) = 0F
				ElseIf timers(i) > p.beanCanTriggerTime Then
					timers(i) -= p.beanCanTriggerTime
					Dim flag2 As Boolean = Me.beanCanTriggerZones(i).transform.position.y > 0F
					Dim array As String() = If((Not flag2), bottomSpawnPattern.PopString(), topSpawnPattern.PopString()).Split(New Char() { ":"c })
					Dim num As Single
					Parser.FloatTryParse(array(0), num)
					Dim flag3 As Boolean = array(1) = "U"
					Dim flyingCowboyLevelBeans As FlyingCowboyLevelBeans = Me.beansPrefab.Spawn()
					flyingCowboyLevelBeans.Init(New Vector3(690F, num), flag3, p.beanCanSpeed, extendTimerPattern.PopFloat())
				End If
			Next
		End While
		Return
	End Function

	' Token: 0x060021BD RID: 8637 RVA: 0x00138CAC File Offset: 0x001370AC
	Private Sub die()
		Me.isDead = True
		Me.StopAllCoroutines()
		If Level.Current.mode = Level.Mode.Easy Then
			MyBase.animator.Play("DeathEasy")
		Else
			MyBase.animator.Play("Death")
			MyBase.StartCoroutine(Me.spawnFloatingSausages_cr())
			AudioManager.[Stop]("sfx_dlc_cowgirl_p3_sausagemeattin_loop")
		End If
		For i As Integer = 0 To 2 - 1
			Dim transform As Transform = If((i <> 0), Me.sausageHolderB, Me.sausageHolderA)
			Dim childCount As Integer = transform.childCount
			For j As Integer = 0 To childCount - 1
				Dim child As Transform = transform.GetChild(j)
				If child.position.x > Me.sausageLinkSqueezePoint.position.x Then
					Global.UnityEngine.[Object].Destroy(child.gameObject)
				End If
			Next
		Next
	End Sub

	' Token: 0x060021BE RID: 8638 RVA: 0x00138D94 File Offset: 0x00137194
	Private Iterator Function spawnFloatingSausages_cr() As IEnumerator
		Dim delay As Single = 1F
		Dim animations As String() = New String() { "A", "B", "C" }
		Dim spawnFactors As Single() = New Single() { 0.2F, 0.6F, 0F, 0.8F, 0.4F, 1F }
		Dim spawnFactorIndex As Integer = Global.UnityEngine.Random.Range(0, spawnFactors.Length)
		Dim animationIndex As Integer = Global.UnityEngine.Random.Range(0, animations.Length)
		While True
			Dim factor As Single = spawnFactors(spawnFactorIndex)
			Dim position As Vector3 = Vector3.Lerp(Me.floatingSausageSpawnPointLeft.position, Me.floatingSausageSpawnPointRight.position, factor)
			Dim s As FlyingCowboyFloatingSausages = TryCast(Me.floatingSausage.Create(position), FlyingCowboyFloatingSausages)
			s.SetAnimation(animations(animationIndex))
			spawnFactorIndex = MathUtilities.NextIndex(spawnFactorIndex, spawnFactors.Length)
			animationIndex = MathUtilities.NextIndex(animationIndex, animations.Length)
			Yield CupheadTime.WaitForSeconds(Me, delay)
		End While
		Return
	End Function

	' Token: 0x060021BF RID: 8639 RVA: 0x00138DB0 File Offset: 0x001371B0
	Private Sub onBossDeathExplosionsEventHandler()
		RemoveHandler Level.Current.OnBossDeathExplosionsEvent, AddressOf Me.onBossDeathExplosionsEventHandler
		Dim array As String() = New String() { "A", "B", "C", "H" }
		Dim array2 As String() = New String() { "D", "E", "F", "G", "I" }
		Dim array3 As String() = New String() { "E", "F", "I" }
		For i As Integer = 0 To 2 - 1
			Dim transform As Transform = If((i <> 0), Me.sausageHolderB, Me.sausageHolderA)
			Dim childCount As Integer = transform.childCount
			For j As Integer = 0 To childCount - 1
				Dim child As Transform = transform.GetChild(j)
				If child.name.Contains("String") Then
					Dim effect As Effect = Me.sausageStringDeathEffect.Create(child.GetComponent(Of SpriteRenderer)().bounds.center)
					effect.transform.rotation = child.rotation
					Dim animator As Animator = effect.animator
					Dim currentAnimatorStateInfo As AnimatorStateInfo = child.GetComponent(Of Animator)().GetCurrentAnimatorStateInfo(0)
					If currentAnimatorStateInfo.IsName("String1") Then
						animator.Play(array.RandomChoice())
					ElseIf currentAnimatorStateInfo.IsName("String2") Then
						animator.Play(array2.RandomChoice())
					Else
						animator.Play(array2.RandomChoice())
					End If
				Else
					Dim component As SpriteRenderer = child.GetComponent(Of SpriteRenderer)()
					Me.sausageDeathEffect.Create(component.bounds.center)
				End If
				Global.UnityEngine.[Object].Destroy(child.gameObject)
			Next
		Next
	End Sub

	' Token: 0x060021C0 RID: 8640 RVA: 0x00138F8A File Offset: 0x0013738A
	Private Sub AnimationEvent_SFX_VocalSausageScreaming()
		AudioManager.Play("sfx_dlc_cowgirl_vocal_p3sausagescreaming")
		Me.emitAudioFromObject.Add("sfx_dlc_cowgirl_vocal_p3sausagescreaming")
	End Sub

	' Token: 0x060021C1 RID: 8641 RVA: 0x00138FA6 File Offset: 0x001373A6
	Private Sub AnimationEvent_SFX_CanSlam()
		AudioManager.Play("sfx_DLC_Cowgirl_P3_CanSlam_Transition")
		Me.emitAudioFromObject.Add("sfx_DLC_Cowgirl_P3_CanSlam_Transition")
	End Sub

	' Token: 0x060021C2 RID: 8642 RVA: 0x00138FC2 File Offset: 0x001373C2
	Private Sub AnimationEvent_SFX_CanHoleBurst()
		AudioManager.Play("sfx_dlc_cowgirl_p3_can_holeburst_pop")
		Me.emitAudioFromObject.Add("sfx_dlc_cowgirl_p3_can_holeburst_pop")
	End Sub

	' Token: 0x060021C3 RID: 8643 RVA: 0x00138FDE File Offset: 0x001373DE
	Private Sub SFX_CanSpitBurningFire()
		AudioManager.Play("sfx_dlc_cowgirl_p3_canspitburningfire")
		Me.emitAudioFromObject.Add("sfx_dlc_cowgirl_p3_canspitburningfire")
	End Sub

	' Token: 0x060021C4 RID: 8644 RVA: 0x00138FFA File Offset: 0x001373FA
	Private Sub SFX_CanSpit()
		AudioManager.Play("sfx_dlc_cowgirl_p3_can_spit")
		Me.emitAudioFromObject.Add("sfx_dlc_cowgirl_p3_can_spit")
	End Sub

	' Token: 0x060021C5 RID: 8645 RVA: 0x00139016 File Offset: 0x00137416
	Private Sub AnimationEvent_SFX_SausageBullRoar()
		AudioManager.Play("sfx_dlc_cowgirl_p3_sausagebullroar")
		Me.emitAudioFromObject.Add("sfx_dlc_cowgirl_p3_sausagebullroar")
	End Sub

	' Token: 0x060021C6 RID: 8646 RVA: 0x00139032 File Offset: 0x00137432
	Private Sub AnimationEvent_SFX_SausageBullSpit()
		AudioManager.Play("sfx_dlc_cowgirl_p3_sausagebullspit")
		Me.emitAudioFromObject.Add("sfx_dlc_cowgirl_p3_sausagebullspit")
	End Sub

	' Token: 0x060021C7 RID: 8647 RVA: 0x0013904E File Offset: 0x0013744E
	Private Sub AnimationEvent_SFX_SausageBullWingUp()
		AudioManager.Play("sfx_dlc_cowgirl_p3_sausagebullwingup")
		Me.emitAudioFromObject.Add("sfx_dlc_cowgirl_p3_sausagebullwingup")
	End Sub

	' Token: 0x060021C8 RID: 8648 RVA: 0x0013906A File Offset: 0x0013746A
	Private Sub AnimationEvent_SFX_SausageBullWingDown()
		AudioManager.Play("sfx_dlc_cowgirl_p3_sausagebullwingdown")
		Me.emitAudioFromObject.Add("sfx_dlc_cowgirl_p3_sausagebullwingdown")
	End Sub

	' Token: 0x060021C9 RID: 8649 RVA: 0x00139086 File Offset: 0x00137486
	Private Sub AnimationEvent_SFX_SausageBullRunToFly()
		AudioManager.Play("sfx_dlc_cowgirl_p3_sausagebull_runtofly")
		Me.emitAudioFromObject.Add("sfx_dlc_cowgirl_p3_sausagebull_runtofly")
	End Sub

	' Token: 0x060021CA RID: 8650 RVA: 0x001390A2 File Offset: 0x001374A2
	Private Sub AnimationEvent_SFX_SausageBullPositionTransfer()
		AudioManager.Play("sfx_dlc_cowgirl_p3_sausage_position_transfer")
		Me.emitAudioFromObject.Add("sfx_dlc_cowgirl_p3_sausage_position_transfer")
	End Sub

	' Token: 0x060021CB RID: 8651 RVA: 0x001390C0 File Offset: 0x001374C0
	Private Iterator Function wobble_cr(transform As Transform, wobbleRadius As Vector2, wobbleDuration As Vector2, initialPosition As Vector3, phase As FlyingCowboyLevelMeat.MeatPhase, useLocal As Boolean, easeWobble As Boolean) As IEnumerator
		Dim elapsedEaseTime As Single = 0F
		Dim shadowInitialPosition As Vector3 = Me.shadowTransform.position
		Dim wobbleTimeElapsed As Vector2 = wobbleDuration * 0.5F
		While Me.meatPhase = phase
			If easeWobble AndAlso elapsedEaseTime < 2F Then
				elapsedEaseTime += CupheadTime.Delta
				Dim easeFactor As Single = Mathf.Lerp(0F, 1F, elapsedEaseTime / 2F)
			End If
			wobbleTimeElapsed.x += CupheadTime.Delta
			wobbleTimeElapsed.y += CupheadTime.Delta
			If wobbleTimeElapsed.x >= 2F * wobbleDuration.x Then
				wobbleTimeElapsed.x -= 2F * wobbleDuration.x
			End If
			Dim tx As Single
			If wobbleTimeElapsed.x > wobbleDuration.x Then
				tx = 1F - (wobbleTimeElapsed.x - wobbleDuration.x) / wobbleDuration.x
			Else
				tx = wobbleTimeElapsed.x / wobbleDuration.x
			End If
			If wobbleTimeElapsed.y >= 2F * wobbleDuration.y Then
				wobbleTimeElapsed.y -= 2F * wobbleDuration.y
			End If
			Dim ty As Single
			If wobbleTimeElapsed.y > wobbleDuration.y Then
				ty = 1F - (wobbleTimeElapsed.y - wobbleDuration.y) / wobbleDuration.y
			Else
				ty = wobbleTimeElapsed.y / wobbleDuration.y
			End If
			Dim positionChange As Vector3 = New Vector3(EaseUtils.EaseInOutSine(wobbleRadius.x, -wobbleRadius.x, tx), EaseUtils.EaseInOutSine(wobbleRadius.y, -wobbleRadius.y, ty))
			If useLocal Then
				transform.localPosition = initialPosition + positionChange
			Else
				transform.position = initialPosition + positionChange
			End If
			If Me.meatPhase = FlyingCowboyLevelMeat.MeatPhase.Can AndAlso Not Mathf.Approximately(wobbleRadius.y, 0F) Then
				Dim vector As Vector3 = positionChange
				vector.y *= 0.2F
				Dim vector2 As Vector3 = shadowInitialPosition + vector
				Dim num As Single = 0F
				If vector2.y >= -220F Then
					num = MathUtilities.LerpMapping(vector2.y, -220F, -150F, 0F, 0.65000004F, True)
					vector2.y = -220F
				End If
				Me.shadowTransform.position = vector2
				Dim num2 As Single = 0.1F * (positionChange.y / wobbleRadius.y)
				Dim num3 As Single = 0.8F - num2 - num
				Me.shadowTransform.SetScale(New Single?(num3), New Single?(num3), Nothing)
			End If
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x04002A3E RID: 10814
	Private Shared SausageLinkWidth As Single = 120F

	' Token: 0x04002A3F RID: 10815
	Private Shared SausageTypeAny As FlyingCowboyLevelMeat.SausageType() = New FlyingCowboyLevelMeat.SausageType() { FlyingCowboyLevelMeat.SausageType.H1, FlyingCowboyLevelMeat.SausageType.H2, FlyingCowboyLevelMeat.SausageType.H3, FlyingCowboyLevelMeat.SausageType.H4, FlyingCowboyLevelMeat.SausageType.L5, FlyingCowboyLevelMeat.SausageType.L5, FlyingCowboyLevelMeat.SausageType.L5, FlyingCowboyLevelMeat.SausageType.L5, FlyingCowboyLevelMeat.SausageType.L5, FlyingCowboyLevelMeat.SausageType.L5, FlyingCowboyLevelMeat.SausageType.L5, FlyingCowboyLevelMeat.SausageType.L5, FlyingCowboyLevelMeat.SausageType.L5, FlyingCowboyLevelMeat.SausageType.U1, FlyingCowboyLevelMeat.SausageType.U2, FlyingCowboyLevelMeat.SausageType.U3 }

	' Token: 0x04002A40 RID: 10816
	Private Shared SausageTypeEnd As FlyingCowboyLevelMeat.SausageType() = New FlyingCowboyLevelMeat.SausageType() { FlyingCowboyLevelMeat.SausageType.H1, FlyingCowboyLevelMeat.SausageType.H2, FlyingCowboyLevelMeat.SausageType.H3, FlyingCowboyLevelMeat.SausageType.H4, FlyingCowboyLevelMeat.SausageType.L5, FlyingCowboyLevelMeat.SausageType.L5, FlyingCowboyLevelMeat.SausageType.L5, FlyingCowboyLevelMeat.SausageType.L5, FlyingCowboyLevelMeat.SausageType.L5, FlyingCowboyLevelMeat.SausageType.L5, FlyingCowboyLevelMeat.SausageType.L5, FlyingCowboyLevelMeat.SausageType.L5, FlyingCowboyLevelMeat.SausageType.L5 }

	' Token: 0x04002A41 RID: 10817
	Private Shared SausageTypeDown As FlyingCowboyLevelMeat.SausageType() = New FlyingCowboyLevelMeat.SausageType() { FlyingCowboyLevelMeat.SausageType.D1, FlyingCowboyLevelMeat.SausageType.D2, FlyingCowboyLevelMeat.SausageType.D3 }

	' Token: 0x04002A42 RID: 10818
	Private Shared SausageGapWidths As Single() = New Single() { 146F, 189F, 286F }

	' Token: 0x04002A43 RID: 10819
	Private Shared SausageGapAnimationNames As String() = New String() { "String1", "String2", "String3" }

	' Token: 0x04002A44 RID: 10820
	Private Shared LowSausageLinkSortingOrder As Integer = 20

	' Token: 0x04002A45 RID: 10821
	Private Shared MidSausageLinkSortingOrder As Integer = 40

	' Token: 0x04002A46 RID: 10822
	Private Shared HighSausageLinkSortingOrder As Integer = 60

	' Token: 0x04002A47 RID: 10823
	<Header("Sausage")>
	<SerializeField()>
	Private sausageSpawnPosition As Transform

	' Token: 0x04002A48 RID: 10824
	<SerializeField()>
	Private beansPrefab As FlyingCowboyLevelBeans

	' Token: 0x04002A49 RID: 10825
	<SerializeField()>
	Private sausageRunSpitBullet As FlyingCowboyLevelSpinningBullet

	' Token: 0x04002A4A RID: 10826
	<SerializeField()>
	Private sausageRunSpitBulletEffect As Effect

	' Token: 0x04002A4B RID: 10827
	<SerializeField()>
	Private runTopSpitBulletSpawn As Transform

	' Token: 0x04002A4C RID: 10828
	<SerializeField()>
	Private runTopSpitBulletEffectSpawn As Transform

	' Token: 0x04002A4D RID: 10829
	<SerializeField()>
	Private runBottomSpitBulletSpawn As Transform

	' Token: 0x04002A4E RID: 10830
	<SerializeField()>
	Private runBottomSpitBulletEffectSpawn As Transform

	' Token: 0x04002A4F RID: 10831
	<SerializeField()>
	Private sausageWobbleRadius As Vector2

	' Token: 0x04002A50 RID: 10832
	<SerializeField()>
	Private sausageWobbleDuration As Vector2

	' Token: 0x04002A51 RID: 10833
	<Header("Can")>
	<SerializeField()>
	Private canTransform As Transform

	' Token: 0x04002A52 RID: 10834
	<SerializeField()>
	Private sausageTransforms As GameObject

	' Token: 0x04002A53 RID: 10835
	<SerializeField()>
	Private canBullet As BasicProjectile

	' Token: 0x04002A54 RID: 10836
	<SerializeField()>
	Private canBulletMuzzleFX As Effect

	' Token: 0x04002A55 RID: 10837
	<SerializeField()>
	Private bulletRoot As Transform

	' Token: 0x04002A56 RID: 10838
	<SerializeField()>
	Private shadowTransform As Transform

	' Token: 0x04002A57 RID: 10839
	<SerializeField()>
	Private sausage As BasicProjectile

	' Token: 0x04002A58 RID: 10840
	<SerializeField()>
	Private sausageLinkSqueezePoint As Transform

	' Token: 0x04002A59 RID: 10841
	<SerializeField()>
	Private sausageHolderA As Transform

	' Token: 0x04002A5A RID: 10842
	<SerializeField()>
	Private sausageHolderB As Transform

	' Token: 0x04002A5B RID: 10843
	<SerializeField()>
	Private nextBulletSpawnPointA As Transform

	' Token: 0x04002A5C RID: 10844
	<SerializeField()>
	Private nextBulletSpawnPointB As Transform

	' Token: 0x04002A5D RID: 10845
	<SerializeField()>
	Private floatingSausage As FlyingCowboyFloatingSausages

	' Token: 0x04002A5E RID: 10846
	<SerializeField()>
	Private floatingSausageSpawnPointLeft As Transform

	' Token: 0x04002A5F RID: 10847
	<SerializeField()>
	Private floatingSausageSpawnPointRight As Transform

	' Token: 0x04002A60 RID: 10848
	<SerializeField()>
	Private sausageString As BasicProjectile

	' Token: 0x04002A61 RID: 10849
	<SerializeField()>
	Private beanCanTriggerZones As TriggerZone()

	' Token: 0x04002A62 RID: 10850
	<SerializeField()>
	Private sausageDeathEffect As Effect

	' Token: 0x04002A63 RID: 10851
	<SerializeField()>
	Private sausageStringDeathEffect As Effect

	' Token: 0x04002A64 RID: 10852
	Private player As AbstractPlayerController

	' Token: 0x04002A65 RID: 10853
	Private meatPhase As FlyingCowboyLevelMeat.MeatPhase

	' Token: 0x04002A66 RID: 10854
	Private damageDealer As DamageDealer

	' Token: 0x04002A67 RID: 10855
	Private damageReceiver As DamageReceiver

	' Token: 0x04002A68 RID: 10856
	Private runningSpitBulletParryPattern As PatternString

	' Token: 0x04002A69 RID: 10857
	Private sausageTimeToMoveString As PatternString

	' Token: 0x04002A6A RID: 10858
	Private spitBulletParryString As PatternString

	' Token: 0x04002A6B RID: 10859
	Private isFlying As Boolean

	' Token: 0x04002A6C RID: 10860
	Private waitingToShoot As Boolean

	' Token: 0x04002A6D RID: 10861
	Private isDead As Boolean

	' Token: 0x04002A6E RID: 10862
	Private canBulletsTriggered As Boolean

	' Token: 0x04002A6F RID: 10863
	Private currentSausageLinkSortingOrderA As Integer = FlyingCowboyLevelMeat.MidSausageLinkSortingOrder

	' Token: 0x04002A70 RID: 10864
	Private currentSausageLinkSortingOrderB As Integer = FlyingCowboyLevelMeat.MidSausageLinkSortingOrder + 1

	' Token: 0x02000654 RID: 1620
	Public Enum MeatPhase
		' Token: 0x04002A72 RID: 10866
		Can
		' Token: 0x04002A73 RID: 10867
		Sausage
		' Token: 0x04002A74 RID: 10868
		Switching
	End Enum

	' Token: 0x02000655 RID: 1621
	Public Enum SausageType
		' Token: 0x04002A76 RID: 10870
		H1
		' Token: 0x04002A77 RID: 10871
		H2
		' Token: 0x04002A78 RID: 10872
		H3
		' Token: 0x04002A79 RID: 10873
		H4
		' Token: 0x04002A7A RID: 10874
		L5
		' Token: 0x04002A7B RID: 10875
		U1
		' Token: 0x04002A7C RID: 10876
		U2
		' Token: 0x04002A7D RID: 10877
		U3
		' Token: 0x04002A7E RID: 10878
		D1
		' Token: 0x04002A7F RID: 10879
		D2
		' Token: 0x04002A80 RID: 10880
		D3
	End Enum
End Class
