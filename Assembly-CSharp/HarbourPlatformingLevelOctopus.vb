Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020008CF RID: 2255
Public Class HarbourPlatformingLevelOctopus
	Inherits PlatformingLevelAutoscrollObject

	' Token: 0x060034B9 RID: 13497 RVA: 0x001EA8EC File Offset: 0x001E8CEC
	Protected Overrides Sub Awake()
		MyBase.Awake()
		AddHandler Me.anchor.OnActivate, AddressOf Me.Switched
		Me.yPosStart = MyBase.transform.position.y
		AddHandler Me.collisionChild.OnPlayerProjectileCollision, AddressOf Me.OnCollisionPlayerProjectile
		AddHandler Me.collisionChild.OnAnyCollision, AddressOf Me.OnCollision
		Me.checkToLock = False
		Me.pinkGem.SetActive(True)
		MyBase.StartCoroutine(Me.gem_shine_switch_cr())
	End Sub

	' Token: 0x060034BA RID: 13498 RVA: 0x001EA97F File Offset: 0x001E8D7F
	Protected Overrides Sub OnCollisionPlayerProjectile(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayerProjectile(hit, phase)
		If Not Me.tuckedDown Then
			Me.timeSinceShot = 0F
			MyBase.animator.SetTrigger("PlayerShooting")
		End If
	End Sub

	' Token: 0x060034BB RID: 13499 RVA: 0x001EA9B0 File Offset: 0x001E8DB0
	Protected Overrides Sub OnCollision(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollision(hit, phase)
		If hit.GetComponent(Of HarbourPlatformingLevelIceberg)() Then
			If Not Me.tuckedDown Then
				MyBase.StartCoroutine(Me.disable_cr())
			End If
			hit.GetComponent(Of HarbourPlatformingLevelIceberg)().DeathParts()
			Global.UnityEngine.[Object].Destroy(hit.gameObject)
		End If
	End Sub

	' Token: 0x060034BC RID: 13500 RVA: 0x001EAA03 File Offset: 0x001E8E03
	Protected Overrides Sub OnCollisionOther(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionOther(hit, phase)
	End Sub

	' Token: 0x060034BD RID: 13501 RVA: 0x001EAA10 File Offset: 0x001E8E10
	Protected Overrides Sub Update()
		MyBase.Update()
		Dim cupheadLevelCamera As CupheadLevelCamera = CupheadLevelCamera.Current
		Dim num As Single = cupheadLevelCamera.autoScrollSpeedMultiplier
		Me.timeSinceShot += CupheadTime.Delta
		If Me.tuckedDown OrElse Me.timeSinceShot > Me.holdSpeedTime Then
			num -= CupheadTime.Delta * (Me.speedupMultiplier - 1F) / Me.deccelerationTime
			num = Mathf.Max(1F, num)
		Else
			num += CupheadTime.Delta * (Me.speedupMultiplier - 1F) / Me.accelerationTime
			num = Mathf.Min(Me.speedupMultiplier, num)
		End If
		cupheadLevelCamera.SetAutoscrollSpeedMultiplier(num)
		If MyBase.animator.GetCurrentAnimatorStateInfo(0).IsName("Idle") Then
			MyBase.animator.speed = num
		Else
			MyBase.animator.speed = 1F
		End If
	End Sub

	' Token: 0x060034BE RID: 13502 RVA: 0x001EAB0C File Offset: 0x001E8F0C
	Private Sub Switched()
		Me.pinkGem.SetActive(False)
		If Me.firstSwitch Then
			MyBase.animator.SetTrigger("StartOctopus")
			Me.StartAutoscroll()
			MyBase.StartCoroutine(Me.start_octopus_cr())
			Me.firstSwitch = False
		Else
			MyBase.animator.SetTrigger("Shoot")
			Me.ShootSFX()
		End If
	End Sub

	' Token: 0x060034BF RID: 13503 RVA: 0x001EAB75 File Offset: 0x001E8F75
	Public Function Started() As Boolean
		Return MyBase.isMoving
	End Function

	' Token: 0x060034C0 RID: 13504 RVA: 0x001EAB80 File Offset: 0x001E8F80
	Private Iterator Function start_octopus_cr() As IEnumerator
		While MyBase.transform.position.x > CupheadLevelCamera.Current.transform.position.x + Me.scrollMinMax.min
			Yield Nothing
		End While
		CupheadLevelCamera.Current.OffsetCamera(True, True)
		MyBase.StartCoroutine(Me.idle_bounce_cr())
		MyBase.animator.SetTrigger("StartTentacles")
		Me.IdleTentaclesSFX()
		MyBase.transform.parent = CupheadLevelCamera.Current.transform
		Yield Nothing
		Return
	End Function

	' Token: 0x060034C1 RID: 13505 RVA: 0x001EAB9C File Offset: 0x001E8F9C
	Private Iterator Function disable_cr() As IEnumerator
		Me.HeadSquishSFX()
		MyBase.animator.SetBool("IsHit", True)
		Me.tuckedDown = True
		Dim endPos As Single = MyBase.transform.position.y - 500F
		Dim speed As Single = 300F
		Dim pos As Vector3 = MyBase.transform.position
		While MyBase.transform.position.y > endPos
			MyBase.transform.AddPosition(0F, -speed * CupheadTime.FixedDelta, 0F)
			Yield Nothing
		End While
		pos = MyBase.transform.position
		Yield CupheadTime.WaitForSeconds(Me, Me.tuckDownDelay)
		Yield Nothing
		While MyBase.transform.position.y < Me.yPosStart
			MyBase.transform.AddPosition(0F, speed * CupheadTime.FixedDelta, 0F)
			Yield Nothing
		End While
		MyBase.transform.position = New Vector3(MyBase.transform.position.x, Me.yPosStart)
		MyBase.animator.SetBool("IsHit", False)
		Me.HeadSquishSFX()
		Me.tuckedDown = False
		Yield Nothing
		Return
	End Function

	' Token: 0x060034C2 RID: 13506 RVA: 0x001EABB8 File Offset: 0x001E8FB8
	Private Iterator Function end_octopus_cr() As IEnumerator
		Me.MoveLoop()
		MyBase.animator.SetTrigger("EndOctopus")
		MyBase.transform.parent = Nothing
		Dim endPos As Single = MyBase.transform.position.y - 1000F
		Dim speed As Single = 100F
		While MyBase.transform.position.y > endPos
			MyBase.transform.AddPosition(0F, -speed * CupheadTime.FixedDelta, 0F)
			Yield Nothing
		End While
		CupheadLevelCamera.Current.OffsetCamera(False, True)
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
		Yield Nothing
		Return
	End Function

	' Token: 0x060034C3 RID: 13507 RVA: 0x001EABD3 File Offset: 0x001E8FD3
	Protected Overrides Sub EndAutoscroll()
		MyBase.StartCoroutine(Me.end_octopus_cr())
	End Sub

	' Token: 0x060034C4 RID: 13508 RVA: 0x001EABE4 File Offset: 0x001E8FE4
	Private Sub Shoot()
		Me.ShootSFX()
		Me.anchor.enabled = False
		Me.puff.Create(Me.projectileRoot.transform.position)
		Me.projectile.Create(Me.projectileRoot.transform.position)
		MyBase.StartCoroutine(Me.gem_timer_cr())
	End Sub

	' Token: 0x060034C5 RID: 13509 RVA: 0x001EAC50 File Offset: 0x001E9050
	Private Iterator Function gem_timer_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, Me.gemOffTime)
		Me.pinkGem.SetActive(True)
		Me.anchor.enabled = True
		Yield Nothing
		Return
	End Function

	' Token: 0x060034C6 RID: 13510 RVA: 0x001EAC6C File Offset: 0x001E906C
	Private Iterator Function gem_shine_switch_cr() As IEnumerator
		Dim order As String = "A1,B1,B2,A2,B1,A1,B2,A2"
		Dim orderIndex As Integer = 0
		While True
			Yield CupheadTime.WaitForSeconds(Me, Global.UnityEngine.Random.Range(0.42F, 0.67F))
			MyBase.animator.Play("Shine_" + order.Split(New Char() { ","c })(orderIndex), 1)
			orderIndex = (orderIndex + 1) Mod order.Split(New Char() { ","c }).Length
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x060034C7 RID: 13511 RVA: 0x001EAC88 File Offset: 0x001E9088
	Private Sub TentacleBackSwitch()
		Dim localPosition As Vector3 = Me.tentacleBack.localPosition
		localPosition.x = If((Not Me.moveTentacles), (Me.tentacleBack.localPosition.x + Me.tentacleOffset), (Me.tentacleBack.localPosition.x - Me.tentacleOffset))
		Me.tentacleBack.localPosition = localPosition
	End Sub

	' Token: 0x060034C8 RID: 13512 RVA: 0x001EACF8 File Offset: 0x001E90F8
	Private Sub TentacleFrontSwitch()
		Me.moveTentacles = Not Me.moveTentacles
		Dim localPosition As Vector3 = Me.tentacleFront.localPosition
		localPosition.x = If((Not Me.moveTentacles), (Me.tentacleFront.localPosition.x + Me.tentacleOffset), (Me.tentacleFront.localPosition.x - Me.tentacleOffset))
		Me.tentacleFront.localPosition = localPosition
	End Sub

	' Token: 0x060034C9 RID: 13513 RVA: 0x001EAD78 File Offset: 0x001E9178
	Private Iterator Function idle_bounce_cr() As IEnumerator
		Dim angle As Single = 0F
		Dim yVelocity As Single = 7F
		Dim sinSize As Single = 2F
		While True
			If MyBase.animator.GetCurrentAnimatorStateInfo(0).IsName("Idle") AndAlso CupheadTime.Delta IsNot 0F Then
				angle += yVelocity * CupheadTime.Delta
				Dim moveY As Vector3 = New Vector3(0F, Mathf.Sin(angle) * sinSize)
				MyBase.transform.localPosition += moveY
				Yield Nothing
			End If
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x060034CA RID: 13514 RVA: 0x001EAD93 File Offset: 0x001E9193
	Private Sub HeadSquishSFX()
		AudioManager.Play("harbour_octopus_head_squish")
		Me.emitAudioFromObject.Add("harbour_octopus_head_squish")
	End Sub

	' Token: 0x060034CB RID: 13515 RVA: 0x001EADAF File Offset: 0x001E91AF
	Private Sub ShootSFX()
		AudioManager.Play("harbour_octopus_shoot")
		Me.emitAudioFromObject.Add("harbour_octopus_shoot")
	End Sub

	' Token: 0x060034CC RID: 13516 RVA: 0x001EADCB File Offset: 0x001E91CB
	Private Sub IdleTentaclesSFX()
		AudioManager.[Stop]("harbour_octopus_move_loop")
		AudioManager.PlayLoop("harbour_octopus_idle_tentacles")
		Me.emitAudioFromObject.Add("harbour_octopus_idle_tentacles")
	End Sub

	' Token: 0x060034CD RID: 13517 RVA: 0x001EADF1 File Offset: 0x001E91F1
	Private Sub MoveLoop()
		AudioManager.[Stop]("harbour_octopus_idle_tentacles")
		AudioManager.PlayLoop("harbour_octopus_move_loop")
		Me.emitAudioFromObject.Add("harbour_octopus_move_loop")
	End Sub

	' Token: 0x060034CE RID: 13518 RVA: 0x001EAE17 File Offset: 0x001E9217
	Private Sub RideStartSFX()
		AudioManager.[Stop]("harbour_octopus_idle_tentacles")
		AudioManager.[Stop]("harbour_octopus_move_loop")
		AudioManager.Play("harbour_octopus_ride_start")
		Me.emitAudioFromObject.Add("harbour_octopus_ride_start")
	End Sub

	' Token: 0x060034CF RID: 13519 RVA: 0x001EAE47 File Offset: 0x001E9247
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me.puff = Nothing
		Me.projectile = Nothing
	End Sub

	' Token: 0x04003CE5 RID: 15589
	<SerializeField()>
	Private puff As Effect

	' Token: 0x04003CE6 RID: 15590
	<SerializeField()>
	Private projectileRoot As Transform

	' Token: 0x04003CE7 RID: 15591
	<SerializeField()>
	Private tentacleFront As Transform

	' Token: 0x04003CE8 RID: 15592
	<SerializeField()>
	Private tentacleBack As Transform

	' Token: 0x04003CE9 RID: 15593
	<SerializeField()>
	Private anchor As ParrySwitch

	' Token: 0x04003CEA RID: 15594
	<SerializeField()>
	Private projectile As HarbourPlatformingLevelOctoProjectile

	' Token: 0x04003CEB RID: 15595
	<SerializeField()>
	Private scrollMinMax As MinMax = New MinMax(-300F, 200F)

	' Token: 0x04003CEC RID: 15596
	<SerializeField()>
	Private pinkGem As GameObject

	' Token: 0x04003CED RID: 15597
	<SerializeField()>
	Private collisionChild As CollisionChild

	' Token: 0x04003CEE RID: 15598
	<SerializeField()>
	Private accelerationTime As Single = 0.3F

	' Token: 0x04003CEF RID: 15599
	<SerializeField()>
	Private holdSpeedTime As Single = 2F

	' Token: 0x04003CF0 RID: 15600
	<SerializeField()>
	Private deccelerationTime As Single = 1F

	' Token: 0x04003CF1 RID: 15601
	<SerializeField()>
	Private speedupMultiplier As Single = 1.2F

	' Token: 0x04003CF2 RID: 15602
	<SerializeField()>
	Private tuckDownDelay As Single = 2F

	' Token: 0x04003CF3 RID: 15603
	<SerializeField()>
	Private gemOffTime As Single = 0.7F

	' Token: 0x04003CF4 RID: 15604
	Private firstSwitch As Boolean = True

	' Token: 0x04003CF5 RID: 15605
	Private timeSinceShot As Single = 1000F

	' Token: 0x04003CF6 RID: 15606
	Private tuckedDown As Boolean

	' Token: 0x04003CF7 RID: 15607
	Private moveTentacles As Boolean

	' Token: 0x04003CF8 RID: 15608
	Private tentacleOffset As Single = 100F

	' Token: 0x04003CF9 RID: 15609
	Private yPosStart As Single

	' Token: 0x04003CFA RID: 15610
	Private Const LOCK_DISTANCE As Single = 600F
End Class
