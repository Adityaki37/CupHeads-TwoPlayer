Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports UnityEngine

' Token: 0x0200079C RID: 1948
Public Class RumRunnersLevelSnout
	Inherits AbstractCollidableObject

	' Token: 0x170003FD RID: 1021
	' (get) Token: 0x06002B4E RID: 11086 RVA: 0x001937D8 File Offset: 0x00191BD8
	' (set) Token: 0x06002B4F RID: 11087 RVA: 0x001937E0 File Offset: 0x00191BE0
	Public Property isAttacking As Boolean

	' Token: 0x06002B50 RID: 11088 RVA: 0x001937EC File Offset: 0x00191BEC
	Private Sub Start()
		For Each damageReceiver As DamageReceiver In Me.damageReceivers
			AddHandler damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
		Next
		Me.snoutScale = MyBase.transform.localScale
		MyBase.transform.position = RumRunnersLevelSnout.OffscreenCoord
	End Sub

	' Token: 0x06002B51 RID: 11089 RVA: 0x00193850 File Offset: 0x00191C50
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		Me.parent.DoDamage(info.damage)
	End Sub

	' Token: 0x06002B52 RID: 11090 RVA: 0x00193863 File Offset: 0x00191C63
	Public Sub Setup(properties As LevelProperties.RumRunners)
		Me.properties = properties
		Me.copBallLaunchAnglePattern = New PatternString(properties.CurrentState.copBall.copBallLaunchAngleString, True, True)
	End Sub

	' Token: 0x06002B53 RID: 11091 RVA: 0x0019388C File Offset: 0x00191C8C
	Public Sub Attack(position As Vector3, shadowPosition As Vector2, onLeft As Boolean, attackType As RumRunnersLevelSnout.AttackType)
		Dim vector As Vector3 = position
		vector.x = CSng(If((Not onLeft), Level.Current.Right, Level.Current.Left))
		Dim one As Vector3 = Vector3.one
		one.x *= CSng(If((Not onLeft), (-1), 1))
		Me.dirtEffect.Create(vector, one)
		MyBase.transform.position = position
		Me.shadowTransform.localPosition = shadowPosition
		MyBase.StartCoroutine(Me.attack_cr(onLeft, attackType))
	End Sub

	' Token: 0x06002B54 RID: 11092 RVA: 0x00193920 File Offset: 0x00191D20
	Private Iterator Function attack_cr(onLeft As Boolean, attackType As RumRunnersLevelSnout.AttackType) As IEnumerator
		Dim p As LevelProperties.RumRunners.AnteaterSnout = Me.properties.CurrentState.anteaterSnout
		Me.onLeft = onLeft
		Dim num As Integer = 0
		Dim flag As Boolean = num <> 0
		Me.endTongue = num <> 0
		Me.endNormal = flag
		MyBase.transform.SetScale(New Single?(If((Not onLeft), (-Me.snoutScale.x), Me.snoutScale.x)), New Single?(Me.snoutScale.y), Nothing)
		Me.parent.SetEyeSide(onLeft)
		MyBase.animator.SetBool("Fake", attackType = RumRunnersLevelSnout.AttackType.Fake)
		MyBase.animator.SetBool("Tongue", attackType = RumRunnersLevelSnout.AttackType.Tongue)
		MyBase.animator.SetTrigger("Attack")
		Me.isAttacking = True
		If attackType = RumRunnersLevelSnout.AttackType.Fake OrElse attackType = RumRunnersLevelSnout.AttackType.Tongue Then
			Dim fullOutBoilDelay As Single = p.snoutFullOutBoilDelay
			If fullOutBoilDelay > 0F Then
				Yield MyBase.animator.WaitForAnimationToStart(Me, "FullOutHold", False)
				Yield CupheadTime.WaitForSeconds(Me, fullOutBoilDelay)
			End If
			MyBase.animator.SetTrigger("HoldComplete")
		End If
		If attackType = RumRunnersLevelSnout.AttackType.Tongue Then
			Yield MyBase.animator.WaitForAnimationToStart(Me, "TongueHold", False)
			Yield CupheadTime.WaitForSeconds(Me, p.tongueHoldDuration)
			MyBase.animator.SetBool("Tongue", False)
			While Not Me.endTongue
				Yield Nothing
			End While
		Else
			While Not Me.endNormal
				Yield Nothing
			End While
		End If
		Me.isAttacking = False
		If attackType = RumRunnersLevelSnout.AttackType.Tongue Then
			Yield MyBase.animator.WaitForAnimationToStart(Me, "Off", False)
		Else
			Yield MyBase.animator.WaitForAnimationToEnd(Me, "QuickEnd", False, True)
		End If
		MyBase.transform.position = RumRunnersLevelSnout.OffscreenCoord
		Return
	End Function

	' Token: 0x06002B55 RID: 11093 RVA: 0x00193949 File Offset: 0x00191D49
	Private Sub animationEvent_EndNormalAttack()
		Me.endNormal = True
	End Sub

	' Token: 0x06002B56 RID: 11094 RVA: 0x00193952 File Offset: 0x00191D52
	Private Sub animationEvent_TriggerTongueEyes()
		Me.parent.TriggerEyesTurnaround()
	End Sub

	' Token: 0x06002B57 RID: 11095 RVA: 0x00193960 File Offset: 0x00191D60
	Private Sub animationEvent_EndFakeTongueAttack()
		Dim effect As Effect = Me.fakeTongueSpittleEffect.Create(Me.tonguePokeFXTransform.position)
		If Not Me.onLeft Then
			Dim localScale As Vector3 = effect.transform.localScale
			localScale.x *= -1F
			effect.transform.localScale = localScale
		End If
	End Sub

	' Token: 0x06002B58 RID: 11096 RVA: 0x001939BA File Offset: 0x00191DBA
	Private Sub animationEvent_EndTongueAttack()
		Me.endTongue = True
	End Sub

	' Token: 0x06002B59 RID: 11097 RVA: 0x001939C4 File Offset: 0x00191DC4
	Private Sub animationEvent_FlipIfNecessary()
		Dim num As Single = Me.copBallLaunchAnglePattern.PopFloat()
		MyBase.animator.SetBool("ThrowDown", num < 0F)
	End Sub

	' Token: 0x06002B5A RID: 11098 RVA: 0x001939F8 File Offset: 0x00191DF8
	Private Sub animationEvent_SpawnCopBall()
		Dim copBall As LevelProperties.RumRunners.CopBall = Me.properties.CurrentState.copBall
		Do
			Me.copBallList.RemoveAll(Function(b As RumRunnersLevelCopBall) b Is Nothing OrElse b.leaveScreen)
			If Me.copBallList.Count >= copBall.copBallMaxCount Then
				Me.copBallList(0).leaveScreen = True
			End If
		Loop While Me.copBallList.Count >= copBall.copBallMaxCount
		Dim float As Single = Me.copBallLaunchAnglePattern.GetFloat()
		Dim rumRunnersLevelCopBall As RumRunnersLevelCopBall = Me.copBallPrefab.Spawn()
		Dim num As Single = If((Not Me.onLeft), (180F - float), float)
		rumRunnersLevelCopBall.Init(Me.copballLaunchOrigin.position, MathUtils.AngleToDirection(num), copBall.copBallSpeed, copBall.copBallHP, copBall, Me.copballLaunchOrigin)
		Me.copBallList.Add(rumRunnersLevelCopBall)
	End Sub

	' Token: 0x06002B5B RID: 11099 RVA: 0x00193AEC File Offset: 0x00191EEC
	Private Sub animationEvent_FireCopBall()
		If Me.copBallList(Me.copBallList.Count - 1) IsNot Nothing Then
			Me.copBallList(Me.copBallList.Count - 1).Launch()
		End If
	End Sub

	' Token: 0x06002B5C RID: 11100 RVA: 0x00193B3C File Offset: 0x00191F3C
	Public Sub Death()
		Me.StopAllCoroutines()
		For Each rumRunnersLevelCopBall As RumRunnersLevelCopBall In Me.copBallList
			If rumRunnersLevelCopBall IsNot Nothing Then
				rumRunnersLevelCopBall.Death(False)
			End If
		Next
		MyBase.gameObject.SetActive(False)
	End Sub

	' Token: 0x06002B5D RID: 11101 RVA: 0x00193BB8 File Offset: 0x00191FB8
	Private Sub AnimationEvent_SFX_RUMRUN_P3_AntEater_Attack_Enter()
		If MyBase.animator.GetBool("Tongue") Then
			AudioManager.Play("sfx_dlc_rumrun_p3_anteater_attack_snout_tongue_fullouthold")
			Me.emitAudioFromObject.Add("sfx_dlc_rumrun_p3_anteater_attack_snout_tongue_fullouthold")
		Else
			AudioManager.Play("sfx_dlc_rumrun_p3_anteater_attack_short_enter")
			Me.emitAudioFromObject.Add("sfx_dlc_rumrun_p3_anteater_attack_short_enter")
		End If
	End Sub

	' Token: 0x06002B5E RID: 11102 RVA: 0x00193C13 File Offset: 0x00192013
	Private Sub AnimationEvent_SFX_RUMRUN_P3_AntEater_Attack_Tongue()
		If MyBase.animator.GetBool("Tongue") Then
			AudioManager.Play("sfx_dlc_rumrun_p3_anteater_attack_snout_tongue_attack")
		End If
	End Sub

	' Token: 0x06002B5F RID: 11103 RVA: 0x00193C39 File Offset: 0x00192039
	Private Sub AnimationEvent_SFX_RUMRUN_P3_AntEater_Attack_ShortExit()
		AudioManager.Play("sfx_dlc_rumrun_p3_anteater_attack_short_exit")
		Me.emitAudioFromObject.Add("sfx_dlc_rumrun_p3_anteater_attack_short_exit")
	End Sub

	' Token: 0x06002B60 RID: 11104 RVA: 0x00193C55 File Offset: 0x00192055
	Private Sub AnimationEvent_SFX_RUMRUN_P3_AntEater_Attack_SpitBallCop()
		AudioManager.Play("sfx_dlc_rumrun_p3_anteater_attack_snout_tongue_spitballcop")
		Me.emitAudioFromObject.Add("sfx_dlc_rumrun_p3_anteater_attack_snout_tongue_spitballcop")
	End Sub

	' Token: 0x06002B61 RID: 11105 RVA: 0x00193C71 File Offset: 0x00192071
	Private Sub AnimationEvent_SFX_RUMRUN_P3_BallCop_SpitVocalShouts()
		AudioManager.Play("sfx_dlc_rumrun_p3_ballcop_spitvocalshouts")
		Me.emitAudioFromObject.Add("sfx_dlc_rumrun_p3_ballcop_spitvocalshouts")
	End Sub

	' Token: 0x04003403 RID: 13315
	Private Shared OffscreenCoord As Vector3 = New Vector3(0F, 1500F)

	' Token: 0x04003404 RID: 13316
	<SerializeField()>
	Private copballLaunchOrigin As Transform

	' Token: 0x04003405 RID: 13317
	<SerializeField()>
	Private copBallPrefab As RumRunnersLevelCopBall

	' Token: 0x04003406 RID: 13318
	<SerializeField()>
	Private dirtEffect As Effect

	' Token: 0x04003407 RID: 13319
	<SerializeField()>
	Private parent As RumRunnersLevelAnteater

	' Token: 0x04003408 RID: 13320
	<SerializeField()>
	Private shadowTransform As Transform

	' Token: 0x04003409 RID: 13321
	<SerializeField()>
	Private damageReceivers As DamageReceiver()

	' Token: 0x0400340A RID: 13322
	<SerializeField()>
	Private tonguePokeFXTransform As Transform

	' Token: 0x0400340B RID: 13323
	<SerializeField()>
	Private fakeTongueSpittleEffect As Effect

	' Token: 0x0400340D RID: 13325
	Private properties As LevelProperties.RumRunners

	' Token: 0x0400340E RID: 13326
	Private snoutScale As Vector2

	' Token: 0x0400340F RID: 13327
	Private copBallList As List(Of RumRunnersLevelCopBall) = New List(Of RumRunnersLevelCopBall)()

	' Token: 0x04003410 RID: 13328
	Private onLeft As Boolean

	' Token: 0x04003411 RID: 13329
	Private endNormal As Boolean

	' Token: 0x04003412 RID: 13330
	Private endTongue As Boolean

	' Token: 0x04003413 RID: 13331
	Private copBallLaunchAnglePattern As PatternString

	' Token: 0x0200079D RID: 1949
	Public Enum AttackType
		' Token: 0x04003416 RID: 13334
		Quick
		' Token: 0x04003417 RID: 13335
		Fake
		' Token: 0x04003418 RID: 13336
		Tongue
	End Enum
End Class
