Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000702 RID: 1794
Public Class OldManLevelGnomeLeader
	Inherits LevelProperties.OldMan.Entity

	' Token: 0x0600267A RID: 9850 RVA: 0x00167C58 File Offset: 0x00166058
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.damageDealer = DamageDealer.NewEnemy()
		Me.damageReceiver = MyBase.GetComponent(Of DamageReceiver)()
		AddHandler Me.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
		MyBase.transform.SetPosition(Nothing, New Single?(Me.baseHeight + Mathf.Sin(Me.GetPosition() * 3.1415927F) * Me.heightRange), Nothing)
	End Sub

	' Token: 0x0600267B RID: 9851 RVA: 0x00167CDA File Offset: 0x001660DA
	Private Sub Update()
		Me.NontargetablePlatformCount()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x0600267C RID: 9852 RVA: 0x00167CF9 File Offset: 0x001660F9
	Public Overrides Sub LevelInit(properties As LevelProperties.OldMan)
		MyBase.LevelInit(properties)
		Me.parryThermometer.gameObject.SetActive(False)
		Me.parryString = New PatternString(properties.CurrentState.gnomeLeader.shotParryString, True)
	End Sub

	' Token: 0x0600267D RID: 9853 RVA: 0x00167D2F File Offset: 0x0016612F
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		MyBase.properties.DealDamage(info.damage)
		If MyBase.properties.CurrentHealth <= 0F Then
			Me.StartDeath()
		End If
	End Sub

	' Token: 0x0600267E RID: 9854 RVA: 0x00167D5D File Offset: 0x0016615D
	Private Sub StartDeath()
		Me.isAlive = False
		Me.coll.enabled = False
		Me.StopAllCoroutines()
		MyBase.animator.SetTrigger("Dead")
		Me.SFX_Death()
		AudioManager.[Stop]("sfx_dlc_omm_p3_ulcer_movement_loop")
	End Sub

	' Token: 0x0600267F RID: 9855 RVA: 0x00167D98 File Offset: 0x00166198
	Public Sub StartGnomeLeader()
		Dim gnomeLeader As LevelProperties.OldMan.GnomeLeader = MyBase.properties.CurrentState.gnomeLeader
		Me.isAlive = True
		Me.pit.SetActive(True)
		Me.stomachPlatforms = New OldManLevelStomachPlatform(Me.platformPositions.Length - 1) {}
		For i As Integer = 0 To Me.stomachPlatforms.Length - 1
			Me.stomachPlatforms(i) = Global.UnityEngine.[Object].Instantiate(Of OldManLevelStomachPlatform)(Me.stomachPlatformPrefab)
			Me.stomachPlatforms(i).transform.position = Me.platformPositions(i).position
			If i < 3 Then
				Me.stomachPlatforms(i).FlipX()
			End If
			Me.stomachPlatforms(i).sparkAnimator = Me.platformPositions(i).GetComponent(Of Animator)()
			Me.stomachPlatforms(i).main = Me
		Next
		MyBase.StartCoroutine(Me.moving_cr())
	End Sub

	' Token: 0x06002680 RID: 9856 RVA: 0x00167E70 File Offset: 0x00166270
	Public Function GetPosition() As Single
		Return Mathf.InverseLerp(CSng(Level.Current.Right) - Me.screenEdgeOffset, CSng(Level.Current.Left) + Me.screenEdgeOffset, MyBase.transform.position.x)
	End Function

	' Token: 0x06002681 RID: 9857 RVA: 0x00167EBC File Offset: 0x001662BC
	Private Iterator Function moving_cr() As IEnumerator
		Dim wait As YieldInstruction = New WaitForFixedUpdate()
		Me.movingRight = MathUtils.RandomBool()
		AudioManager.Play("sfx_dlc_omm_p3_ulcer_introlaugh")
		MyBase.animator.Play(If((Not Me.movingRight), "IntroRight", "IntroLeft"))
		Yield wait
		If Not Me.movingRight Then
			While MyBase.animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.99F
				Yield Nothing
			End While
			MyBase.animator.Play("Idle")
			MyBase.animator.Update(0F)
			MyBase.transform.localScale = New Vector3(1F, 1F)
		Else
			Yield MyBase.animator.WaitForAnimationToStart(Me, "Idle", False)
		End If
		Me.SFX_MoveLoop()
		MyBase.StartCoroutine(Me.gnome_leader_cr())
		Me.timeForScreenCross = MyBase.properties.CurrentState.gnomeLeader.bossMoveTime
		Me.locationEnd = 0F
		Dim animHelper As AnimationHelper = MyBase.animator.GetComponent(Of AnimationHelper)()
		While True
			Me.locationTime = 0F
			Me.locationStart = MyBase.transform.position.x
			If Me.movingRight Then
				Me.locationEnd = CSng(Level.Current.Right) - Me.screenEdgeOffset
			Else
				Me.locationEnd = CSng(Level.Current.Left) + Me.screenEdgeOffset
			End If
			While Me.locationTime < Me.timeForScreenCross
				MyBase.transform.SetPosition(New Single?(Me.GetXPositionAtTimeValue(Me.locationTime)), New Single?(Me.baseHeight + Mathf.Sin(Me.GetPosition() * 3.1415927F) * Me.heightRange), Nothing)
				MyBase.transform.SetEulerAngles(Nothing, Nothing, New Single?(Mathf.Lerp(CSng(If((Not Me.movingRight), (-7), 7)), CSng(If((Not Me.movingRight), 7, (-7))), Me.locationTime / Me.timeForScreenCross)))
				Me.locationTime += CupheadTime.FixedDelta
				If Me.turnTrigger AndAlso Me.locationTime / Me.timeForScreenCross > 0.8F Then
					MyBase.animator.SetTrigger("OnTurn")
					Me.turning = True
					Me.turnTrigger = False
				End If
				If PauseManager.state <> PauseManager.State.Paused Then
					Dim num As Single = Mathf.Sin(Mathf.InverseLerp(Me.locationStart, Me.locationEnd, MyBase.transform.position.x) * 3.1415927F)
					animHelper.Speed = 1F + num * (Me.topAnimSpeed / 24F - 1F)
					Me.SFXLoopVolume = 0.0001F + num * 0.3F
					If Not Me.spitting Then
						AudioManager.FadeSFXVolume("sfx_dlc_omm_p3_ulcer_movement_loop", Me.SFXLoopVolume, 1E-05F)
					End If
				End If
				Yield wait
			End While
			Me.turnTrigger = True
			MyBase.transform.SetPosition(New Single?(Me.locationEnd), Nothing, Nothing)
			Me.movingRight = Not Me.movingRight
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06002682 RID: 9858 RVA: 0x00167ED8 File Offset: 0x001662D8
	Private Function GetXPositionAtTimeValue(time As Single) As Single
		If time > Me.timeForScreenCross Then
			Dim num As Single = time Mod Me.timeForScreenCross / Me.timeForScreenCross
			Return EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, Me.locationEnd, Me.locationStart, num)
		End If
		Dim num2 As Single = time / Me.timeForScreenCross
		Return EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, Me.locationStart, Me.locationEnd, num2)
	End Function

	' Token: 0x06002683 RID: 9859 RVA: 0x00167F34 File Offset: 0x00166334
	Private Sub AniEvent_Turn()
		MyBase.transform.SetScale(New Single?(-MyBase.transform.localScale.x), Nothing, Nothing)
		Me.turning = False
	End Sub

	' Token: 0x06002684 RID: 9860 RVA: 0x00167F80 File Offset: 0x00166380
	Private Function NontargetablePlatformCount() As Integer
		Dim num As Integer = 0
		For i As Integer = 0 To Me.stomachPlatforms.Length - 1
			If Not Me.stomachPlatforms(i).isActivated OrElse Me.stomachPlatforms(i).isTargeted Then
				num += 1
			End If
		Next
		Return num
	End Function

	' Token: 0x06002685 RID: 9861 RVA: 0x00167FD4 File Offset: 0x001663D4
	Private Iterator Function gnome_leader_cr() As IEnumerator
		Dim p As LevelProperties.OldMan.GnomeLeader = MyBase.properties.CurrentState.gnomeLeader
		Dim platformCountToRemove As PatternString = New PatternString(p.platformParryString, True, True)
		Dim shotDelayString As PatternString = New PatternString(p.shotDelayString, True, True)
		Me.sequenceMainIndex = Global.UnityEngine.Random.Range(0, p.shotPlatformString.Length)
		Dim numOfPlatformsToDestroy As Integer = 0
		Dim platformToTarget As Integer = 0
		Dim projectileSpawnsParryable As Boolean = False
		While Me.isAlive
			Me.restartSequence = False
			projectileSpawnsParryable = False
			Me.currentTongue = -1
			Me.sequenceMainIndex = (Me.sequenceMainIndex + 1) Mod p.shotPlatformString.Length
			Dim shotPlatformString As PatternString = New PatternString(p.shotPlatformString(Me.sequenceMainIndex), True)
			shotPlatformString.SetSubStringIndex(-1)
			Me.sequenceIndex = 0
			For i As Integer = 0 To 5 - 1
				Me.sequence(i) = shotPlatformString.PopInt()
			Next
			numOfPlatformsToDestroy = platformCountToRemove.PopInt()
			While Not Me.restartSequence
				Yield CupheadTime.WaitForSeconds(Me, shotDelayString.PopFloat())
				If Me.NontargetablePlatformCount() < 5 AndAlso Not Me.restartSequence Then
					While Me.turning
						Yield Nothing
					End While
					MyBase.animator.SetTrigger("Spit")
					Dim count As Integer = 0
					Do
						platformToTarget = Me.sequence(Me.sequenceIndex)
						Me.sequenceIndex = (Me.sequenceIndex + 1) Mod 5
						count += 1
					Loop While(Not Me.stomachPlatforms(platformToTarget).isActivated OrElse Me.stomachPlatforms(platformToTarget).isTargeted) AndAlso count < 5
					If Me.currentTongue = -1 AndAlso Me.NontargetablePlatformCount() >= numOfPlatformsToDestroy Then
						projectileSpawnsParryable = True
						Me.currentTongue = platformToTarget
						MyBase.StartCoroutine(Me.wait_to_parry_cr())
					Else
						projectileSpawnsParryable = False
					End If
					Me.SFX_PreSpit()
					While Not Me.readyToSpit
						Yield Nothing
					End While
					Yield MyBase.StartCoroutine(Me.shoot_cr(Me.stomachPlatforms(platformToTarget), projectileSpawnsParryable))
				End If
			End While
			Yield CupheadTime.WaitForSeconds(Me, 0.1F)
		End While
		Return
	End Function

	' Token: 0x06002686 RID: 9862 RVA: 0x00167FF0 File Offset: 0x001663F0
	Private Iterator Function wait_to_parry_cr() As IEnumerator
		While Not Me.parryThermometer.isActivated
			Yield Nothing
		End While
		For Each oldManLevelStomachPlatform As OldManLevelStomachPlatform In Me.stomachPlatforms
			oldManLevelStomachPlatform.ActivatePlatform()
		Next
		Me.restartSequence = True
		Return
	End Function

	' Token: 0x06002687 RID: 9863 RVA: 0x0016800B File Offset: 0x0016640B
	Public Sub SpawnParryable(spawnPosition As Vector3)
		Me.parryThermometer.transform.position = spawnPosition
		Me.parryThermometer.gameObject.SetActive(True)
	End Sub

	' Token: 0x06002688 RID: 9864 RVA: 0x00168030 File Offset: 0x00166430
	Private Iterator Function shoot_cr(selectedPlatform As OldManLevelStomachPlatform, projectileSpawnsParryable As Boolean) As IEnumerator
		Dim predictedPos As Single = Me.GetXPositionAtTimeValue(Me.locationTime + 0.5416667F)
		Me.isBehind = False
		Dim willBeMovingRight As Boolean = Me.movingRight
		If Me.locationTime + 0.5416667F > Me.timeForScreenCross Then
			willBeMovingRight = Not willBeMovingRight
		End If
		If willBeMovingRight Then
			Me.isBehind = predictedPos + 250F > selectedPlatform.transform.position.x
		Else
			Me.isBehind = predictedPos - 250F < selectedPlatform.transform.position.x
		End If
		If Me.turning Then
			Me.isBehind = Not Me.isBehind
		End If
		Dim animationName As String
		If Mathf.Abs(predictedPos - selectedPlatform.transform.position.x) < 350F Then
			MyBase.animator.SetTrigger(If((Not Me.isBehind), "SpitForwardClose", "SpitBehindClose"))
			animationName = If((Not Me.isBehind), "Spit_Forward_Close", "Spit_Behind_Close")
			Me.spitVFXAnimator.transform.localPosition = Me.spitRoots(0).transform.localPosition
		Else
			MyBase.animator.SetTrigger(If((Not Me.isBehind), "SpitForward", "SpitBehind"))
			animationName = If((Not Me.isBehind), "Spit_Forward", "Spit_Behind")
			Me.spitVFXAnimator.transform.localPosition = Me.spitRoots(1).transform.localPosition
		End If
		Yield MyBase.animator.WaitForAnimationToStart(Me, animationName, False)
		Me.spitting = True
		AudioManager.FadeSFXVolume("sfx_dlc_omm_p3_ulcer_movement_loop", Mathf.Min(0.25F, Me.SFXLoopVolume), 0.25F)
		Me.SFX_StartSpit()
		Me.readyToSpit = False
		While Not Me.spitFrame
			Yield Nothing
		End While
		Dim p As LevelProperties.OldMan.GnomeLeader = MyBase.properties.CurrentState.gnomeLeader
		Me.spitVFXAnimator.transform.localPosition = New Vector3(Mathf.Abs(Me.spitVFXAnimator.transform.localPosition.x) * CSng(If((Not Me.isBehind), (-1), 1)), Me.spitVFXAnimator.transform.localPosition.y)
		Me.spitVFXAnimator.transform.localScale = New Vector3(Mathf.Sign(Me.spitVFXAnimator.transform.localPosition.x), 1F)
		Dim startPos As Vector3 = Me.spitVFXAnimator.transform.position
		Dim x As Single = selectedPlatform.transform.position.x - startPos.x
		Dim y As Single = selectedPlatform.transform.position.y - startPos.y
		Dim timeToApex As Single = p.shotApexTime
		Dim height As Single = p.shotApexHeight
		Dim apexTime2 As Single = timeToApex * timeToApex
		Dim g As Single = -2F * height / apexTime2
		Dim viY As Single = 2F * height / timeToApex
		Dim viX2 As Single = viY * viY
		Dim sqrtRooted As Single = viX2 + 2F * g * y
		Dim tEnd As Single = (-viY + Mathf.Sqrt(sqrtRooted)) / g
		Dim tEnd2 As Single = (-viY - Mathf.Sqrt(sqrtRooted)) / g
		Dim tEnd3 As Single = Mathf.Max(tEnd, tEnd2)
		Dim velocityX As Single = x / tEnd3
		Dim speed As Vector3 = New Vector3(velocityX, viY)
		selectedPlatform.Anticipation()
		Me.spitVFXAnimator.Play("SpitSmoke")
		Me.spitVFXAnimator.Update(0F)
		Dim projectile As OldManLevelGnomeProjectile = Me.projectilePrefab.Spawn()
		projectile.Init(startPos, speed, g, projectileSpawnsParryable, Me.parryString.PopLetter() = "P"c AndAlso Not projectileSpawnsParryable, selectedPlatform)
		Me.SFX_SpawnProjectile()
		Me.spitFrame = False
		AudioManager.FadeSFXVolume("sfx_dlc_omm_p3_ulcer_movement_loop", Me.SFXLoopVolume, 1F)
		Me.spitting = False
		Return
	End Function

	' Token: 0x06002689 RID: 9865 RVA: 0x00168059 File Offset: 0x00166459
	Private Sub AniEvent_ReadyToSpit()
		Me.readyToSpit = True
	End Sub

	' Token: 0x0600268A RID: 9866 RVA: 0x00168062 File Offset: 0x00166462
	Private Sub AniEvent_Shoot()
		Me.spitFrame = True
	End Sub

	' Token: 0x0600268B RID: 9867 RVA: 0x0016806C File Offset: 0x0016646C
	Protected Overrides Sub OnDrawGizmos()
		MyBase.OnDrawGizmos()
		If Me.platformPositions IsNot Nothing Then
			For Each transform As Transform In Me.platformPositions
				Gizmos.color = Color.red
				Gizmos.DrawWireSphere(transform.position, 50F)
			Next
		End If
	End Sub

	' Token: 0x0600268C RID: 9868 RVA: 0x001680C3 File Offset: 0x001664C3
	Private Sub SFX_MoveLoop()
		AudioManager.PlayLoop("sfx_dlc_omm_p3_ulcer_movement_loop")
		Me.emitAudioFromObject.Add("sfx_dlc_omm_p3_ulcer_movement_loop")
	End Sub

	' Token: 0x0600268D RID: 9869 RVA: 0x001680DF File Offset: 0x001664DF
	Private Sub SFX_PreSpit()
		AudioManager.Play("sfx_dlc_omm_p3_ulcer_bonespitpre")
		Me.emitAudioFromObject.Add("sfx_dlc_omm_p3_ulcer_bonespitpre")
	End Sub

	' Token: 0x0600268E RID: 9870 RVA: 0x001680FB File Offset: 0x001664FB
	Private Sub SFX_StartSpit()
		AudioManager.Play("sfx_dlc_omm_p3_ulcer_spitbonevocal")
		Me.emitAudioFromObject.Add("sfx_dlc_omm_p3_ulcer_spitbonevocal")
	End Sub

	' Token: 0x0600268F RID: 9871 RVA: 0x00168117 File Offset: 0x00166517
	Private Sub SFX_SpawnProjectile()
		AudioManager.Play("sfx_dlc_omm_p3_ulcerspitbone")
		Me.emitAudioFromObject.Add("sfx_dlc_omm_p3_ulcerspitbone")
	End Sub

	' Token: 0x06002690 RID: 9872 RVA: 0x00168133 File Offset: 0x00166533
	Private Sub SFX_Death()
		AudioManager.Play("sfx_dlc_omm_p3_ulcer_deathvocal")
		Me.emitAudioFromObject.Add("sfx_dlc_omm_p3_ulcer_deathvocal")
		AudioManager.FadeSFXVolume("sfx_dlc_omm_p3_stomachacid_amb_loop", 0F, 2F)
	End Sub

	' Token: 0x04002F1E RID: 12062
	Private Const DROP_Y As Single = 5F

	' Token: 0x04002F1F RID: 12063
	Private Const BULLET_COUNT As Integer = 4

	' Token: 0x04002F20 RID: 12064
	Private Const SPIT_DELAY As Single = 0.5416667F

	' Token: 0x04002F21 RID: 12065
	Private Const SPIT_DISTANCE_OFFSET As Single = 250F

	' Token: 0x04002F22 RID: 12066
	Private Const CLOSE_SPIT_ANIM_RANGE As Single = 350F

	' Token: 0x04002F23 RID: 12067
	<SerializeField()>
	Private parryThermometer As OldManLevelParryThermometer

	' Token: 0x04002F24 RID: 12068
	Public splashHandler As OldManLevelSplashHandler

	' Token: 0x04002F25 RID: 12069
	<SerializeField()>
	Private pit As GameObject

	' Token: 0x04002F26 RID: 12070
	<SerializeField()>
	Private spitRoots As Transform()

	' Token: 0x04002F27 RID: 12071
	<SerializeField()>
	Private spitVFXAnimator As Animator

	' Token: 0x04002F28 RID: 12072
	<SerializeField()>
	Private projectilePrefab As OldManLevelGnomeProjectile

	' Token: 0x04002F29 RID: 12073
	<SerializeField()>
	Private stomachPlatformPrefab As OldManLevelStomachPlatform

	' Token: 0x04002F2A RID: 12074
	Private stomachPlatforms As OldManLevelStomachPlatform()

	' Token: 0x04002F2B RID: 12075
	Private currentTongue As Integer = -1

	' Token: 0x04002F2C RID: 12076
	<SerializeField()>
	Public platformPositions As Transform()

	' Token: 0x04002F2D RID: 12077
	Private damageDealer As DamageDealer

	' Token: 0x04002F2E RID: 12078
	Private damageReceiver As DamageReceiver

	' Token: 0x04002F2F RID: 12079
	Private player As AbstractPlayerController

	' Token: 0x04002F30 RID: 12080
	Private isAlive As Boolean

	' Token: 0x04002F31 RID: 12081
	Private movingRight As Boolean

	' Token: 0x04002F32 RID: 12082
	Private readyToSpit As Boolean

	' Token: 0x04002F33 RID: 12083
	Private spitFrame As Boolean

	' Token: 0x04002F34 RID: 12084
	Private spitting As Boolean

	' Token: 0x04002F35 RID: 12085
	Private restartSequence As Boolean

	' Token: 0x04002F36 RID: 12086
	Private sequence As Integer() = New Integer(4) {}

	' Token: 0x04002F37 RID: 12087
	Private sequenceIndex As Integer

	' Token: 0x04002F38 RID: 12088
	Private sequenceMainIndex As Integer

	' Token: 0x04002F39 RID: 12089
	Private locationTime As Single

	' Token: 0x04002F3A RID: 12090
	Private locationStart As Single

	' Token: 0x04002F3B RID: 12091
	Private locationEnd As Single

	' Token: 0x04002F3C RID: 12092
	Private timeForScreenCross As Single

	' Token: 0x04002F3D RID: 12093
	Private turnTrigger As Boolean = True

	' Token: 0x04002F3E RID: 12094
	Private turning As Boolean

	' Token: 0x04002F3F RID: 12095
	Private parryString As PatternString

	' Token: 0x04002F40 RID: 12096
	Private isBehind As Boolean

	' Token: 0x04002F41 RID: 12097
	Private screenEdgeOffset As Single = 200F

	' Token: 0x04002F42 RID: 12098
	<SerializeField()>
	Private baseHeight As Single = 188F

	' Token: 0x04002F43 RID: 12099
	<SerializeField()>
	Private topAnimSpeed As Single = 30F

	' Token: 0x04002F44 RID: 12100
	<SerializeField()>
	Private heightRange As Single

	' Token: 0x04002F45 RID: 12101
	<SerializeField()>
	Private coll As Collider2D

	' Token: 0x04002F46 RID: 12102
	Private SFXLoopVolume As Single
End Class
