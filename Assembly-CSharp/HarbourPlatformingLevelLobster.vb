Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020008CD RID: 2253
Public Class HarbourPlatformingLevelLobster
	Inherits PlatformingLevelShootingEnemy

	' Token: 0x060034A4 RID: 13476 RVA: 0x001E8D80 File Offset: 0x001E7180
	Protected Overrides Sub Start()
		MyBase.Start()
		Me.startPositionY = MyBase.transform.position.y
		MyBase.StartCoroutine(Me.start_trigger_cr())
		Me.previousY = MyBase.transform.position.y
		Me.exploder = MyBase.GetComponent(Of LevelBossDeathExploder)()
	End Sub

	' Token: 0x060034A5 RID: 13477 RVA: 0x001E8DE0 File Offset: 0x001E71E0
	Protected Overrides Sub OnDrawGizmos()
		MyBase.OnDrawGizmos()
		Gizmos.color = New Color(0F, 0F, 1F, 1F)
		Gizmos.DrawLine(Me.offTrigger.transform.position, New Vector3(Me.offTrigger.transform.position.x, 5000F, 0F))
		Gizmos.DrawLine(Me.onTrigger.transform.position, New Vector3(Me.onTrigger.transform.position.x, 5000F, 0F))
		Gizmos.color = New Color(0F, 1F, 0F, 0.5F)
		Gizmos.DrawLine(Me.leftBoundary.transform.position, New Vector3(Me.leftBoundary.transform.position.x, 5000F, 0F))
		Gizmos.DrawLine(Me.rightBoundary.transform.position, New Vector3(Me.rightBoundary.transform.position.x, 5000F, 0F))
	End Sub

	' Token: 0x060034A6 RID: 13478 RVA: 0x001E8F20 File Offset: 0x001E7320
	Private Iterator Function attack_cr() As IEnumerator
		MyBase.animator.SetTrigger("OnAttackStart")
		Yield MyBase.animator.WaitForAnimationToStart(Me, "Warning_Loop", False)
		Yield CupheadTime.WaitForSeconds(Me, MyBase.Properties.lobsterWarningTime)
		MyBase.animator.SetTrigger("Attack")
		Me.AttackSFX()
		Yield Nothing
		Yield MyBase.animator.WaitForAnimationToStart(Me, "Attack_Trans_Idle", False)
		Return
	End Function

	' Token: 0x060034A7 RID: 13479 RVA: 0x001E8F3C File Offset: 0x001E733C
	Private Iterator Function start_trigger_cr() As IEnumerator
		While Me._target Is Nothing
			Yield Nothing
		End While
		Me.dist = Me._target.transform.position.x - Me.onTrigger.transform.position.x
		While Me.dist < 0F
			Me.dist = Me._target.transform.position.x - Me.onTrigger.transform.position.x
			Yield Nothing
		End While
		Me.mainCoroutine = MyBase.StartCoroutine(Me.main_cr())
		Me.dist = Me._target.transform.position.x - Me.offTrigger.transform.position.x
		While Me.dist < 0F
			Me.dist = Me._target.transform.position.x - Me.offTrigger.transform.position.x
			Yield Nothing
		End While
		Me.isGone = True
		Yield Nothing
		Return
	End Function

	' Token: 0x060034A8 RID: 13480 RVA: 0x001E8F58 File Offset: 0x001E7358
	Private Iterator Function main_cr() As IEnumerator
		While Not Me.isGone
			Me.direction = If((Me.direction <> PlatformingLevelShootingEnemy.Direction.Right), PlatformingLevelShootingEnemy.Direction.Right, PlatformingLevelShootingEnemy.Direction.Left)
			MyBase.transform.localScale = New Vector3(CSng(If((Me.direction <> PlatformingLevelShootingEnemy.Direction.Left), 1, (-1))), 1F, 1F)
			MyBase.transform.SetPosition(New Single?(If((Me.direction <> PlatformingLevelShootingEnemy.Direction.Left), (CupheadLevelCamera.Current.Bounds.xMin - 350F), (CupheadLevelCamera.Current.Bounds.xMax + 350F))), New Single?(MyBase.Properties.lobsterY), Nothing)
			If If((Me.direction <> PlatformingLevelShootingEnemy.Direction.Left), (MyBase.transform.position.x > Me.rightBoundary.position.x), (MyBase.transform.position.x < Me.leftBoundary.position.x)) Then
				MyBase.transform.SetPosition(Nothing, New Single?(-5000F), Nothing)
				Yield CupheadTime.WaitForSeconds(Me, MyBase.Properties.lobsterOffscreenTime)
			Else
				If If((Me.direction <> PlatformingLevelShootingEnemy.Direction.Left), (MyBase.transform.position.x < Me.leftBoundary.position.x), (MyBase.transform.position.x > Me.rightBoundary.position.x)) Then
					MyBase.transform.SetPosition(New Single?(If((Me.direction <> PlatformingLevelShootingEnemy.Direction.Left), Me.leftBoundary.position.x, Me.rightBoundary.position.x)), Nothing, Nothing)
					Yield MyBase.StartCoroutine(Me.pop_up_cr())
				Else
					MyBase.animator.Play("Idle")
					Me.IdleSFX()
					Me.poppedUp = True
				End If
				While If((Me.direction <> PlatformingLevelShootingEnemy.Direction.Left), (MyBase.transform.position.x < CupheadLevelCamera.Current.Bounds.xMax + -250F), (MyBase.transform.position.x > CupheadLevelCamera.Current.Bounds.xMin - -250F))
					MyBase.transform.AddPosition(MyBase.Properties.lobsterSpeed * CupheadTime.Delta * CSng(If((Me.direction <> PlatformingLevelShootingEnemy.Direction.Left), 1, (-1))), 0F, 0F)
					If If((Me.direction <> PlatformingLevelShootingEnemy.Direction.Left), (MyBase.transform.position.x > Me.rightBoundary.position.x), (MyBase.transform.position.x < Me.leftBoundary.position.x)) Then
						Me.Popdown(False)
						Return
					End If
					Yield Nothing
				End While
				Yield MyBase.StartCoroutine(Me.attack_cr())
				While If((Me.direction <> PlatformingLevelShootingEnemy.Direction.Left), (MyBase.transform.position.x < CupheadLevelCamera.Current.Bounds.xMax + 350F), (MyBase.transform.position.x > CupheadLevelCamera.Current.Bounds.xMin - 350F))
					MyBase.transform.AddPosition(MyBase.Properties.lobsterSpeed * CupheadTime.Delta * CSng(If((Me.direction <> PlatformingLevelShootingEnemy.Direction.Left), 1, (-1))), 0F, 0F)
					If If((Me.direction <> PlatformingLevelShootingEnemy.Direction.Left), (MyBase.transform.position.x > Me.rightBoundary.position.x), (MyBase.transform.position.x < Me.leftBoundary.position.x)) Then
						Me.Popdown(False)
						Return
					End If
					Yield Nothing
				End While
				MyBase.transform.SetPosition(Nothing, New Single?(-5000F), Nothing)
				Yield CupheadTime.WaitForSeconds(Me, MyBase.Properties.lobsterOffscreenTime)
			End If
		End While
		Global.UnityEngine.[Object].Destroy(Me.main.gameObject)
		Return
		Return
	End Function

	' Token: 0x060034A9 RID: 13481 RVA: 0x001E8F73 File Offset: 0x001E7373
	Private Sub Popup()
		MyBase.StartCoroutine(Me.pop_up_cr())
	End Sub

	' Token: 0x060034AA RID: 13482 RVA: 0x001E8F82 File Offset: 0x001E7382
	Private Sub Popdown(dead As Boolean)
		AudioManager.[Stop]("harbour_lobster_idle")
		MyBase.StartCoroutine(Me.pop_down_cr(dead))
	End Sub

	' Token: 0x060034AB RID: 13483 RVA: 0x001E8F9C File Offset: 0x001E739C
	Private Iterator Function pop_up_cr() As IEnumerator
		MyBase.animator.SetTrigger("OnEmerge")
		Me.EmergeSFX()
		Dim t As Single = 0F
		Dim time As Single = 0.6F
		Dim endY As Single = MyBase.Properties.lobsterY
		Dim [end] As Vector2 = New Vector2(MyBase.transform.position.x, endY)
		While t < time
			Dim start As Vector2 = MyBase.transform.position
			[end] = New Vector2(MyBase.transform.position.x, endY)
			Dim val As Single = EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, 0F, 1F, t / time)
			MyBase.transform.position = Vector2.Lerp(start, [end], val)
			t += CupheadTime.Delta
			Yield Nothing
		End While
		MyBase.transform.position = [end]
		Me.poppedUp = True
		Return
	End Function

	' Token: 0x060034AC RID: 13484 RVA: 0x001E8FB8 File Offset: 0x001E73B8
	Private Iterator Function pop_down_cr(dead As Boolean) As IEnumerator
		If Not Me.poppedUp Then
			Return
		End If
		Me.poppedUp = False
		If dead AndAlso Me.mainCoroutine IsNot Nothing Then
			MyBase.StopCoroutine(Me.mainCoroutine)
		End If
		If Me.isGone Then
			MyBase.transform.parent = Nothing
			MyBase.animator.SetTrigger("OnTuck")
		Else
			MyBase.animator.Play("Tuck")
			Me.SinkSFX()
		End If
		If dead Then
			Me.exploder.StartExplosion()
			Yield CupheadTime.WaitForSeconds(Me, 1F)
			Me.exploder.StopExplosions()
		End If
		Dim t As Single = 0F
		Dim time As Single = 1.5F
		Dim start As Vector2 = MyBase.transform.position
		Dim [end] As Vector2 = New Vector2(MyBase.transform.position.x, Me.startPositionY)
		Dim splashDepth As Single = -1200F
		While t < time
			Dim val As Single = EaseUtils.Ease(EaseUtils.EaseType.easeInSine, 0F, 1F, t / time)
			MyBase.transform.position = Vector2.Lerp(start, [end], val)
			If MyBase.transform.position.y <= splashDepth AndAlso Me.previousY > splashDepth Then
				Dim gameObject As GameObject = Global.UnityEngine.[Object].Instantiate(Of GameObject)(Me.splashPrefab, New Vector3(MyBase.transform.position.x, splashDepth, MyBase.transform.position.z), Quaternion.identity)
				gameObject.transform.SetParent(Nothing)
				Me.delay_destroy_cr(gameObject, 10F)
			End If
			t += CupheadTime.Delta
			Me.previousY = MyBase.transform.position.y
			Yield Nothing
		End While
		MyBase.transform.position = [end]
		If Me.isGone Then
			Global.UnityEngine.[Object].Destroy(Me.main.gameObject)
		Else
			MyBase.StartCoroutine(Me.delay_cr(dead))
		End If
		Yield Nothing
		Return
	End Function

	' Token: 0x060034AD RID: 13485 RVA: 0x001E8FDC File Offset: 0x001E73DC
	Private Iterator Function delay_destroy_cr(o As GameObject, t As Single) As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, t)
		Global.UnityEngine.[Object].Destroy(o)
		Return
	End Function

	' Token: 0x060034AE RID: 13486 RVA: 0x001E9005 File Offset: 0x001E7405
	Protected Overrides Sub Die()
		Me.Popdown(True)
	End Sub

	' Token: 0x060034AF RID: 13487 RVA: 0x001E9010 File Offset: 0x001E7410
	Private Iterator Function delay_cr(dead As Boolean) As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, If((Not dead), MyBase.Properties.lobsterOffscreenTime, MyBase.Properties.lobsterTuckTime))
		If Me.isGone Then
			Global.UnityEngine.[Object].Destroy(Me.main.gameObject)
			Return
		End If
		MyBase.Health = MyBase.Properties.Health
		Me.mainCoroutine = MyBase.StartCoroutine(Me.main_cr())
		Return
	End Function

	' Token: 0x060034B0 RID: 13488 RVA: 0x001E9032 File Offset: 0x001E7432
	Private Sub EmergeSFX()
		AudioManager.Play("harbour_lobster_emerge")
		Me.emitAudioFromObject.Add("harbour_lobster_emerge")
	End Sub

	' Token: 0x060034B1 RID: 13489 RVA: 0x001E904E File Offset: 0x001E744E
	Private Sub SinkSFX()
		AudioManager.[Stop]("harbour_lobster_idle")
		AudioManager.Play("harbour_lobster_sink")
		Me.emitAudioFromObject.Add("harbour_lobster_sink")
	End Sub

	' Token: 0x060034B2 RID: 13490 RVA: 0x001E9074 File Offset: 0x001E7474
	Private Sub AttackSFX()
		AudioManager.Play("harbour_lobster_attack")
		Me.emitAudioFromObject.Add("harbour_lobster_attack")
	End Sub

	' Token: 0x060034B3 RID: 13491 RVA: 0x001E9090 File Offset: 0x001E7490
	Private Sub IdleSFX()
		AudioManager.PlayLoop("harbour_lobster_idle")
		Me.emitAudioFromObject.Add("harbour_lobster_idle")
	End Sub

	' Token: 0x060034B4 RID: 13492 RVA: 0x001E90AC File Offset: 0x001E74AC
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me.splashPrefab = Nothing
	End Sub

	' Token: 0x04003CD0 RID: 15568
	<SerializeField()>
	Private main As Transform

	' Token: 0x04003CD1 RID: 15569
	<SerializeField()>
	Private onTrigger As Transform

	' Token: 0x04003CD2 RID: 15570
	<SerializeField()>
	Private offTrigger As Transform

	' Token: 0x04003CD3 RID: 15571
	<SerializeField()>
	Private leftBoundary As Transform

	' Token: 0x04003CD4 RID: 15572
	<SerializeField()>
	Private rightBoundary As Transform

	' Token: 0x04003CD5 RID: 15573
	<SerializeField()>
	Private exploder As LevelBossDeathExploder

	' Token: 0x04003CD6 RID: 15574
	<SerializeField()>
	Private splashPrefab As GameObject

	' Token: 0x04003CD7 RID: 15575
	<SerializeField()>
	Private splashTransform As Transform

	' Token: 0x04003CD8 RID: 15576
	Private poppedUp As Boolean

	' Token: 0x04003CD9 RID: 15577
	Private isGone As Boolean

	' Token: 0x04003CDA RID: 15578
	Private dist As Single = 1000F

	' Token: 0x04003CDB RID: 15579
	Private startPositionY As Single

	' Token: 0x04003CDC RID: 15580
	Private Const OffScreenPadding As Single = 350F

	' Token: 0x04003CDD RID: 15581
	Private Const attackPadding As Single = -250F

	' Token: 0x04003CDE RID: 15582
	Private mainCoroutine As Coroutine

	' Token: 0x04003CDF RID: 15583
	Private previousY As Single

	' Token: 0x04003CE0 RID: 15584
	Private direction As PlatformingLevelShootingEnemy.Direction = PlatformingLevelShootingEnemy.Direction.Right
End Class
