Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports UnityEngine

' Token: 0x020007EC RID: 2028
Public Class SnowCultLevelJackFrost
	Inherits LevelProperties.SnowCult.Entity

	' Token: 0x06002E74 RID: 11892 RVA: 0x001B633C File Offset: 0x001B473C
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.damageDealer = DamageDealer.NewEnemy()
		Me.damageReceiver = MyBase.GetComponent(Of DamageReceiver)()
		AddHandler Me.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
		AddHandler MyBase.properties.OnBossDeath, AddressOf Me.OnBossDeath
		Me.rend = MyBase.GetComponent(Of SpriteRenderer)()
		Me.blinkCount = Global.UnityEngine.Random.Range(1, 5)
	End Sub

	' Token: 0x06002E75 RID: 11893 RVA: 0x001B63AD File Offset: 0x001B47AD
	Private Sub Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x06002E76 RID: 11894 RVA: 0x001B63C5 File Offset: 0x001B47C5
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		MyBase.properties.DealDamage(info.damage)
	End Sub

	' Token: 0x06002E77 RID: 11895 RVA: 0x001B63D8 File Offset: 0x001B47D8
	Public Sub Intro()
		Me.state = SnowCultLevelJackFrost.States.Intro
	End Sub

	' Token: 0x06002E78 RID: 11896 RVA: 0x001B63E4 File Offset: 0x001B47E4
	Public Sub StartPhase3()
		Me.bucket.SetActive(True)
		MyBase.gameObject.SetActive(True)
		Me.scale = MyBase.transform.localScale
		Me.positionX = MyBase.transform.position.x
		Me.onRight = Rand.Bool()
		Me.ChangeSide()
		Me.rightSideUp = True
		Me.faceOrientation = New PatternString(MyBase.properties.CurrentState.face.faceOrientationString, True, True)
		Me.splitShotPink = New PatternString(MyBase.properties.CurrentState.splitShot.pinkString, True, True)
		Me.shotCoord = New PatternString(MyBase.properties.CurrentState.splitShot.shotCoordString, True, True)
		Me.splitShotPink.SetSubStringIndex(-1)
		Me.shotCoord.SetSubStringIndex(-1)
		Me.shardAngleOffsetString = New PatternString(MyBase.properties.CurrentState.shardAttack.angleOffset, True)
		MyBase.StartCoroutine(Me.remove_platforms_cr())
	End Sub

	' Token: 0x06002E79 RID: 11897 RVA: 0x001B64F8 File Offset: 0x001B48F8
	Private Iterator Function remove_platforms_cr() As IEnumerator
		Dim count As Integer = Me.presetPlatforms.Length
		While count > 0
			For Each snowCultLevelPlatform As SnowCultLevelPlatform In Me.presetPlatforms
				If snowCultLevelPlatform IsNot Nothing AndAlso snowCultLevelPlatform.transform.position.y < Camera.main.transform.position.y - 450F Then
					snowCultLevelPlatform.transform.DetachChildren()
					Global.UnityEngine.[Object].Destroy(snowCultLevelPlatform.gameObject)
					count -= 1
				End If
			Next
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06002E7A RID: 11898 RVA: 0x001B6513 File Offset: 0x001B4913
	Private Sub EndIntro()
		Me.state = SnowCultLevelJackFrost.States.Idle
		Me.boxCollider.enabled = True
	End Sub

	' Token: 0x06002E7B RID: 11899 RVA: 0x001B6528 File Offset: 0x001B4928
	Public Sub CreatePlatforms()
		Me.presetPlatforms = New SnowCultLevelPlatform(Me.platformsPresetPositions.Length - 1) {}
		Me.isClockwise = Rand.Bool()
		Dim platforms As LevelProperties.SnowCult.Platforms = MyBase.properties.CurrentState.platforms
		Me.circlePlatforms = New SnowCultLevelPlatform(platforms.platformNum - 1) {}
		Dim num As Single = 360F / CSng(platforms.platformNum)
		For i As Integer = 0 To platforms.platformNum - 1
			Me.circlePlatforms(i) = Global.UnityEngine.[Object].Instantiate(Of GameObject)(Me.platformPrefab).transform.GetChild(0).GetComponent(Of SnowCultLevelPlatform)()
			Me.circlePlatforms(i).transform.parent.position = Me.platformPivotPoint.transform.position
			Me.circlePlatforms(i).StartRotate(num * CSng(i), New Vector3(Me.platformPivotPoint.transform.position.x, Me.platformPivotPoint.transform.position.y + platforms.pivotPointYOffset), platforms.loopSizeX, platforms.loopSizeY, platforms.platformSpeed, platforms.pivotPointYOffset, Me.isClockwise)
			Me.circlePlatforms(i).SetID(i)
		Next
	End Sub

	' Token: 0x06002E7C RID: 11900 RVA: 0x001B6660 File Offset: 0x001B4A60
	Public Sub CreateAscendingPlatform(i As Integer)
		Me.presetPlatforms(i) = Global.UnityEngine.[Object].Instantiate(Of GameObject)(Me.platformPrefab).transform.GetChild(0).GetComponent(Of SnowCultLevelPlatform)()
		Me.presetPlatforms(i).transform.parent.position = Me.platformsPresetPositions(i).transform.position
		Me.presetPlatforms(i).SetID(i)
	End Sub

	' Token: 0x06002E7D RID: 11901 RVA: 0x001B66C8 File Offset: 0x001B4AC8
	Private Sub AniEvent_CheckBlink()
		Me.blinkCounter += 1
		If Me.blinkCounter >= Me.blinkCount Then
			MyBase.animator.SetTrigger("Blink")
			Me.blinkCount = Global.UnityEngine.Random.Range(1, 5)
			Me.blinkCounter = 0
		End If
	End Sub

	' Token: 0x06002E7E RID: 11902 RVA: 0x001B6718 File Offset: 0x001B4B18
	Public Sub StartSwitch()
		If Me.firstAttack Then
			Me.firstAttack = False
		Else
			MyBase.StartCoroutine(Me.switch_cr())
		End If
	End Sub

	' Token: 0x06002E7F RID: 11903 RVA: 0x001B6740 File Offset: 0x001B4B40
	Private Iterator Function switch_cr() As IEnumerator
		Me.state = SnowCultLevelJackFrost.States.Switch
		Dim p As LevelProperties.SnowCult.Face = MyBase.properties.CurrentState.face
		Dim flippedY As Boolean = False
		Dim c As Char = Me.faceOrientation.PopLetter()
		If(c = "U"c AndAlso MyBase.transform.parent.localScale.y = -1F) OrElse (c = "D"c AndAlso MyBase.transform.parent.localScale.y = 1F) Then
			flippedY = True
		End If
		Dim isFront As Boolean = False
		Dim triggerName As String
		Dim stateName As String
		If Global.UnityEngine.Random.Range(0F, 1F) < 0.25F Then
			triggerName = If((Not flippedY), "FrontSwap", "FrontSwapFlip")
			stateName = If((Not flippedY), "SideSwapFront", "SideSwapFrontFlip")
			isFront = True
		Else
			triggerName = If((Not flippedY), "BackSwap", "BackSwapFlip")
			stateName = If((Not flippedY), "SideSwapBack", "SideSwapBackFlip")
		End If
		MyBase.animator.SetTrigger(triggerName)
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Idle", False, True)
		If isFront Then
			Me.rend.sortingLayerName = "Foreground"
		End If
		If flippedY Then
			Me.rightSideUp = Not Me.rightSideUp
		End If
		Yield MyBase.animator.WaitForAnimationToEnd(Me, stateName, False, True)
		Yield New WaitForEndOfFrame()
		Me.rend.sortingLayerName = "Default"
		Me.state = SnowCultLevelJackFrost.States.Idle
		Return
	End Function

	' Token: 0x06002E80 RID: 11904 RVA: 0x001B675B File Offset: 0x001B4B5B
	Private Sub FlipParentTransformX()
		Me.onRight = Not Me.onRight
		Me.ChangeSide()
	End Sub

	' Token: 0x06002E81 RID: 11905 RVA: 0x001B6774 File Offset: 0x001B4B74
	Private Sub FlipParentTransformXY()
		MyBase.transform.parent.SetScale(Nothing, New Single?(CSng(If((Not Me.rightSideUp), (-1), 1))), Nothing)
		Me.FlipParentTransformX()
	End Sub

	' Token: 0x06002E82 RID: 11906 RVA: 0x001B67C4 File Offset: 0x001B4BC4
	Private Sub ChangeSide()
		MyBase.transform.parent.SetScale(New Single?(If((Not Me.onRight), (-Me.scale.x), Me.scale.x)), Nothing, Nothing)
	End Sub

	' Token: 0x06002E83 RID: 11907 RVA: 0x001B681F File Offset: 0x001B4C1F
	Public Sub StartEyeAttack()
		MyBase.animator.SetTrigger("EyeAttack")
		Me.state = SnowCultLevelJackFrost.States.Eye
	End Sub

	' Token: 0x06002E84 RID: 11908 RVA: 0x001B6838 File Offset: 0x001B4C38
	Private Sub aniEvent_LaunchEye()
		MyBase.StartCoroutine(Me.eye_attack_cr())
	End Sub

	' Token: 0x06002E85 RID: 11909 RVA: 0x001B6848 File Offset: 0x001B4C48
	Private Iterator Function eye_attack_cr() As IEnumerator
		Dim p As LevelProperties.SnowCult.EyeAttack = MyBase.properties.CurrentState.eyeAttack
		Me.activeEyeProjectile = Me.eyeProjectile.Spawn()
		Me.activeEyeProjectile.Init(Me.eyeRoot.position, Me.mouthRoot.position, Me.onRight, Me.rightSideUp, p)
		Me.activeEyeProjectile.main = Me
		While Not Me.activeEyeProjectile.readyToOpenMouth
			Yield Nothing
		End While
		MyBase.animator.SetTrigger("EyeAttackOpenMouth")
		While Not Me.activeEyeProjectile.readyToCloseMouth
			Yield Nothing
		End While
		MyBase.animator.Play("EyeAttackEnd")
		MyBase.animator.Update(0F)
		Me.SFX_SNOWCULT_JackFrostEyeballReturn()
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "EyeAttackEnd", False, True)
		Yield CupheadTime.WaitForSeconds(Me, p.attackDelay)
		Me.state = SnowCultLevelJackFrost.States.Idle
		Return
	End Function

	' Token: 0x06002E86 RID: 11910 RVA: 0x001B6863 File Offset: 0x001B4C63
	Private Sub AniEvent_RemoveEye()
		Me.activeEyeProjectile.ReturnToSnowflake()
	End Sub

	' Token: 0x06002E87 RID: 11911 RVA: 0x001B6870 File Offset: 0x001B4C70
	Public Sub StartShardAttack()
		MyBase.StartCoroutine(Me.shard_attack_cr())
	End Sub

	' Token: 0x06002E88 RID: 11912 RVA: 0x001B6880 File Offset: 0x001B4C80
	Private Iterator Function shard_attack_cr() As IEnumerator
		Me.state = SnowCultLevelJackFrost.States.Shard
		Dim p As LevelProperties.SnowCult.ShardAttack = MyBase.properties.CurrentState.shardAttack
		Dim degrees As Single = 360F / CSng(p.shardNumber)
		Dim loopSizeX As Single = p.circleSizeX
		Dim loopSizeY As Single = p.circleSizeY
		Dim shards As SnowCultLevelShard() = New SnowCultLevelShard(p.shardNumber - 1) {}
		Dim angleOffsetString As String() = p.angleOffset.Split(New Char() { ","c })
		Dim angleOffset As Single = Me.shardAngleOffsetString.PopFloat()
		Dim angleList As List(Of Single) = New List(Of Single)()
		For k As Integer = 0 To p.shardNumber - 1
			angleList.Add((degrees * CSng(k) + angleOffset) Mod 360F)
		Next
		angleList.Sort(Function(a As Single, b As Single) ((a + 90F) Mod 360F).CompareTo((b + 90F) Mod 360F))
		Me.iceCreamGhostRenderer.sortingOrder = -12
		MyBase.animator.SetTrigger("IceCreamAttack")
		Yield MyBase.animator.WaitForAnimationToStart(Me, "IceCream", False)
		Me.SFX_SNOWCULT_JackFrostIcecream()
		Dim wait As YieldInstruction = New WaitForFixedUpdate()
		Dim count As Integer = 0
		Dim sparkleDelay As Single = Global.UnityEngine.Random.Range(0.1F, 0.3F)
		While count < p.shardNumber
			Dim normalizedAngle As Single = Mathf.InverseLerp(0.11627907F, 0.7906977F, MyBase.animator.GetCurrentAnimatorStateInfo(0).normalizedTime)
			sparkleDelay -= CupheadTime.FixedDelta
			If sparkleDelay <= 0F Then
				Dim num As Single = (normalizedAngle + 0.25F) Mod 1F * 3.1415927F * 2F
				Me.iceCreamSparkle.Create(Me.platformPivotPoint.position + p.circleOffsetY * Vector3.up + Vector3.right * MyBase.transform.parent.localScale.x * Mathf.Sin(num) * loopSizeX + Vector3.down * MyBase.transform.parent.localScale.y * Mathf.Cos(num) * loopSizeY)
				sparkleDelay += Global.UnityEngine.Random.Range(0.1F, 0.3F)
			End If
			If CSng(count) < normalizedAngle * CSng(p.shardNumber) Then
				Dim num2 As Single = angleList(count)
				If MyBase.transform.parent.localScale.x < 0F Then
					num2 = 360F - num2
				End If
				If MyBase.transform.parent.localScale.y < 0F Then
					num2 = (num2 + (90F - num2) * 2F) Mod 360F
				End If
				Dim snowCultLevelShard As SnowCultLevelShard = Me.shardPrefab.Spawn()
				snowCultLevelShard.Init(Me.platformPivotPoint.position + p.circleOffsetY * Vector3.up + Vector3.forward * CSng(count) * 0.001F, num2, loopSizeX, loopSizeY, p)
				shards(count) = snowCultLevelShard
				count += 1
			End If
			Yield wait
		End While
		Yield CupheadTime.WaitForSeconds(Me, p.warningLength)
		For l As Integer = 0 To shards.Length - 1
			shards(l).Appear()
		Next
		Yield CupheadTime.WaitForSeconds(Me, p.shardHesitation)
		If Me.isClockwise Then
			For i As Integer = shards.Length - 1 To 0 Step -1
				Yield CupheadTime.WaitForSeconds(Me, p.shardDelay)
				If shards(i) IsNot Nothing Then
					shards(i).LaunchProjectile()
				End If
			Next
		Else
			For j As Integer = 0 To shards.Length - 1
				Yield CupheadTime.WaitForSeconds(Me, p.shardDelay)
				If shards(j) IsNot Nothing Then
					shards(j).LaunchProjectile()
				End If
			Next
		End If
		Yield CupheadTime.WaitForSeconds(Me, p.attackDelay)
		Me.state = SnowCultLevelJackFrost.States.Idle
		Yield Nothing
		Return
	End Function

	' Token: 0x06002E89 RID: 11913 RVA: 0x001B689B File Offset: 0x001B4C9B
	Private Sub AniEvent_SetGhostLayerBehindSnowflakeMiddleLayer()
		Me.iceCreamGhostRenderer.sortingOrder = -17
	End Sub

	' Token: 0x06002E8A RID: 11914 RVA: 0x001B68AA File Offset: 0x001B4CAA
	Public Sub StartMouthShot()
		MyBase.StartCoroutine(Me.split_shot_cr())
	End Sub

	' Token: 0x06002E8B RID: 11915 RVA: 0x001B68BC File Offset: 0x001B4CBC
	Private Iterator Function split_shot_cr() As IEnumerator
		Dim p As LevelProperties.SnowCult.SplitShot = MyBase.properties.CurrentState.splitShot
		Me.state = SnowCultLevelJackFrost.States.SplitShot
		Dim posX As Single = 0F
		Dim posY As Single = 0F
		Dim timesToShoot As Integer = Me.shotCoord.SubStringLength()
		MyBase.animator.SetBool("SplitShot", True)
		Yield MyBase.animator.WaitForAnimationToStart(Me, "SplitShotStart", False)
		Me.SFX_SNOWCULT_JackFrostSplitshotHandwavingStart()
		Yield MyBase.animator.WaitForAnimationToStart(Me, "SplitShotAnti", False)
		Me.SFX_SNOWCULT_JackFrostSplitshotHandwavingLoop()
		For i As Integer = 0 To timesToShoot - 1
			posY = Me.shotCoord.PopFloat()
			posX = CSng(If((Not Me.onRight), 640, (-640)))
			Dim pos As Vector3 = New Vector3(posX, MyBase.transform.position.y + posY)
			Dim dir As Vector3 = pos - Me.splitShotRoot.position
			Dim splitShot As SnowCultLevelSplitShotBullet = If((Me.splitShotPink.PopLetter() <> "P"c), Me.mouthPrefab.Spawn(), Me.mouthPinkPrefab.Spawn())
			splitShot.Init(Me.splitShotRoot.position, MathUtils.DirectionToAngle(dir), p.shotSpeed, p.shatterCount, p.spreadAngle, p)
			splitShot.transform.localScale = New Vector3(MyBase.transform.parent.localScale.x, 1F)
			splitShot.main = Me
			Yield CupheadTime.WaitForSeconds(Me, p.shotDelay - 0.45F)
			If splitShot Then
				splitShot.Grow()
			End If
			Yield CupheadTime.WaitForSeconds(Me, 0.45F)
			If splitShot Then
				MyBase.animator.SetTrigger("SplitShotFire")
				Me.SFX_SNOWCULT_JackFrostSplitshotBucketLaunch()
				While Not Me.fireSplitShot
					Yield Nothing
				End While
				Me.fireSplitShot = False
				If splitShot Then
					splitShot.Fire()
				End If
			End If
		Next
		MyBase.animator.SetBool("SplitShot", False)
		Me.SFX_SNOWCULT_JackFrostSplitshotHandwavingLoopStop()
		Yield CupheadTime.WaitForSeconds(Me, p.attackDelay)
		Me.state = SnowCultLevelJackFrost.States.Idle
		Yield Nothing
		Return
	End Function

	' Token: 0x06002E8C RID: 11916 RVA: 0x001B68D7 File Offset: 0x001B4CD7
	Private Sub AniEvent_FireSplitShot()
		Me.fireSplitShot = True
	End Sub

	' Token: 0x06002E8D RID: 11917 RVA: 0x001B68E0 File Offset: 0x001B4CE0
	Private Sub OnBossDeath()
		Me.dead = True
		MyBase.transform.parent.SetScale(Nothing, New Single?(1F), Nothing)
		MyBase.animator.Play(If((Not Me.rightSideUp), "FlipDeath", "Death"))
	End Sub

	' Token: 0x06002E8E RID: 11918 RVA: 0x001B6945 File Offset: 0x001B4D45
	Private Sub EnableWizardDeathAnimation()
		Me.wizardDeath.SetActive(True)
	End Sub

	' Token: 0x06002E8F RID: 11919 RVA: 0x001B6953 File Offset: 0x001B4D53
	Private Sub AnimationEvent_SFX_SNOWCULT_JackFrostIntroThumblick()
		AudioManager.Play("sfx_dlc_snowcult_p3_snowflake_intro_thumblick")
		Me.emitAudioFromObject.Add("sfx_dlc_snowcult_p3_snowflake_intro_thumblick")
	End Sub

	' Token: 0x06002E90 RID: 11920 RVA: 0x001B696F File Offset: 0x001B4D6F
	Private Sub AnimationEvent_SFX_SNOWCULT_JackFrostEyeballAttack()
		AudioManager.Play("sfx_dlc_snowcult_p3_snowflake_eyeball_attack")
		Me.emitAudioFromObject.Add("sfx_dlc_snowcult_p3_snowflake_eyeball_attack")
	End Sub

	' Token: 0x06002E91 RID: 11921 RVA: 0x001B698B File Offset: 0x001B4D8B
	Private Sub SFX_SNOWCULT_JackFrostEyeballReturn()
		AudioManager.Play("sfx_dlc_snowcult_p3_snowflake_eyeball_return")
		Me.emitAudioFromObject.Add("sfx_dlc_snowcult_p3_snowflake_eyeball_return")
	End Sub

	' Token: 0x06002E92 RID: 11922 RVA: 0x001B69A7 File Offset: 0x001B4DA7
	Private Sub SFX_SNOWCULT_JackFrostIcecream()
		AudioManager.Play("sfx_dlc_snowcult_p3_snowflake_icecreamattack")
		Me.emitAudioFromObject.Add("sfx_dlc_snowcult_p3_snowflake_icecreamattack")
	End Sub

	' Token: 0x06002E93 RID: 11923 RVA: 0x001B69C3 File Offset: 0x001B4DC3
	Private Sub AnimationEvent_SFX_SNOWCULT_JackFrostSideSwap()
		AudioManager.Play("sfx_dlc_snowcult_p3_snowflake_sideswap")
		Me.emitAudioFromObject.Add("sfx_dlc_snowcult_p3_snowflake_sideswap")
	End Sub

	' Token: 0x06002E94 RID: 11924 RVA: 0x001B69DF File Offset: 0x001B4DDF
	Private Sub SFX_SNOWCULT_JackFrostSplitshotHandwavingLoop()
		AudioManager.PlayLoop("sfx_dlc_snowcult_p3_snowflake_splitshot_handwaving_attack_loop")
		Me.emitAudioFromObject.Add("sfx_dlc_snowcult_p3_snowflake_splitshot_handwaving_attack_loop")
	End Sub

	' Token: 0x06002E95 RID: 11925 RVA: 0x001B69FB File Offset: 0x001B4DFB
	Private Sub SFX_SNOWCULT_JackFrostSplitshotHandwavingLoopStop()
		AudioManager.[Stop]("sfx_dlc_snowcult_p3_snowflake_splitshot_handwaving_attack_loop")
	End Sub

	' Token: 0x06002E96 RID: 11926 RVA: 0x001B6A07 File Offset: 0x001B4E07
	Private Sub SFX_SNOWCULT_JackFrostSplitshotHandwavingStart()
		AudioManager.Play("sfx_dlc_snowcult_p3_snowflake_splitshot_handwaving_attack_start")
		Me.emitAudioFromObject.Add("sfx_dlc_snowcult_p3_snowflake_splitshot_handwaving_attack_start")
	End Sub

	' Token: 0x06002E97 RID: 11927 RVA: 0x001B6A23 File Offset: 0x001B4E23
	Private Sub SFX_SNOWCULT_JackFrostSplitshotBucketLaunch()
		AudioManager.Play("sfx_dlc_snowcult_p3_snowflake_splitshot_attack_bucket_launch")
		Me.emitAudioFromObject.Add("sfx_dlc_snowcult_p3_snowflake_splitshot_attack_bucket_launch")
	End Sub

	' Token: 0x06002E98 RID: 11928 RVA: 0x001B6A3F File Offset: 0x001B4E3F
	Private Sub AnimationEvent_SFX_SNOWCULT_JackFrostDeath()
		AudioManager.Play("sfx_dlc_snowcult_p3_snowflake_death")
		Me.emitAudioFromObject.Add("sfx_dlc_snowcult_p3_snowflake_death")
	End Sub

	' Token: 0x04003709 RID: 14089
	Private Const BLINK_LOOP_COUNT_MIN As Integer = 1

	' Token: 0x0400370A RID: 14090
	Private Const BLINK_LOOP_COUNT_MAX As Integer = 5

	' Token: 0x0400370B RID: 14091
	<SerializeField()>
	Private boxCollider As BoxCollider2D

	' Token: 0x0400370C RID: 14092
	<SerializeField()>
	Private mouthPrefab As SnowCultLevelSplitShotBullet

	' Token: 0x0400370D RID: 14093
	<SerializeField()>
	Private mouthPinkPrefab As SnowCultLevelSplitShotBullet

	' Token: 0x0400370E RID: 14094
	<SerializeField()>
	Private shardPrefab As SnowCultLevelShard

	' Token: 0x0400370F RID: 14095
	<SerializeField()>
	Private iceCreamSparkle As Effect

	' Token: 0x04003710 RID: 14096
	<SerializeField()>
	Private eyeProjectile As SnowCultLevelEyeProjectile

	' Token: 0x04003711 RID: 14097
	Private activeEyeProjectile As SnowCultLevelEyeProjectile

	' Token: 0x04003712 RID: 14098
	Public eyeProjectileGuide As Transform

	' Token: 0x04003713 RID: 14099
	<SerializeField()>
	Private eyeRoot As Transform

	' Token: 0x04003714 RID: 14100
	<SerializeField()>
	Private mouthRoot As Transform

	' Token: 0x04003715 RID: 14101
	<SerializeField()>
	Private splitShotRoot As Transform

	' Token: 0x04003716 RID: 14102
	<SerializeField()>
	Private platformPivotPoint As Transform

	' Token: 0x04003717 RID: 14103
	<SerializeField()>
	Private platformPrefab As GameObject

	' Token: 0x04003718 RID: 14104
	<SerializeField()>
	Private platformsPresetPositions As Transform()

	' Token: 0x04003719 RID: 14105
	<SerializeField()>
	Private wizardDeath As GameObject

	' Token: 0x0400371A RID: 14106
	<SerializeField()>
	Private bucket As GameObject

	' Token: 0x0400371B RID: 14107
	<SerializeField()>
	Private iceCreamGhostRenderer As SpriteRenderer

	' Token: 0x0400371C RID: 14108
	Private onRight As Boolean

	' Token: 0x0400371D RID: 14109
	Private rightSideUp As Boolean

	' Token: 0x0400371E RID: 14110
	Private isClockwise As Boolean

	' Token: 0x0400371F RID: 14111
	Private firstAttack As Boolean = True

	' Token: 0x04003720 RID: 14112
	Private positionX As Single

	' Token: 0x04003721 RID: 14113
	Private scale As Vector3

	' Token: 0x04003722 RID: 14114
	Private presetPlatforms As SnowCultLevelPlatform()

	' Token: 0x04003723 RID: 14115
	Private circlePlatforms As SnowCultLevelPlatform()

	' Token: 0x04003724 RID: 14116
	Private player As AbstractPlayerController

	' Token: 0x04003725 RID: 14117
	Private damageDealer As DamageDealer

	' Token: 0x04003726 RID: 14118
	Private damageReceiver As DamageReceiver

	' Token: 0x04003727 RID: 14119
	Public state As SnowCultLevelJackFrost.States

	' Token: 0x04003728 RID: 14120
	Private faceOrientation As PatternString

	' Token: 0x04003729 RID: 14121
	Private splitShotPink As PatternString

	' Token: 0x0400372A RID: 14122
	Private shotCoord As PatternString

	' Token: 0x0400372B RID: 14123
	Private shardAngleOffsetString As PatternString

	' Token: 0x0400372C RID: 14124
	Private rend As SpriteRenderer

	' Token: 0x0400372D RID: 14125
	Private fireSplitShot As Boolean

	' Token: 0x0400372E RID: 14126
	Private blinkCounter As Integer

	' Token: 0x0400372F RID: 14127
	Private blinkCount As Integer

	' Token: 0x04003730 RID: 14128
	Public dead As Boolean

	' Token: 0x020007ED RID: 2029
	Public Enum States
		' Token: 0x04003732 RID: 14130
		Intro
		' Token: 0x04003733 RID: 14131
		Idle
		' Token: 0x04003734 RID: 14132
		Switch
		' Token: 0x04003735 RID: 14133
		Eye
		' Token: 0x04003736 RID: 14134
		Beam
		' Token: 0x04003737 RID: 14135
		Hazard
		' Token: 0x04003738 RID: 14136
		Shard
		' Token: 0x04003739 RID: 14137
		SplitShot
		' Token: 0x0400373A RID: 14138
		Arc
	End Enum
End Class
