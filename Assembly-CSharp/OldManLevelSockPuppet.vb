Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x0200070E RID: 1806
Public Class OldManLevelSockPuppet
	Inherits AbstractCollidableObject

	' Token: 0x06002708 RID: 9992 RVA: 0x0016DD7D File Offset: 0x0016C17D
	Private Sub Start()
		Me.rootPosition = MyBase.transform.position
		Me.startPosition = MyBase.transform.position
		Me.damageDealer = DamageDealer.NewEnemy()
	End Sub

	' Token: 0x06002709 RID: 9993 RVA: 0x0016DDAC File Offset: 0x0016C1AC
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me.WORKAROUND_NullifyFields()
	End Sub

	' Token: 0x0600270A RID: 9994 RVA: 0x0016DDBC File Offset: 0x0016C1BC
	Private Sub Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
		MyBase.transform.position = Me.rootPosition + Mathf.Sin(Me.wobbleTimer * 3F) * Me.wobbleX * Vector3.right + Mathf.Sin(Me.wobbleTimer * 2F) * Me.wobbleY * Vector3.up
		Me.wobbleTimer += CupheadTime.Delta
	End Sub

	' Token: 0x0600270B RID: 9995 RVA: 0x0016DE55 File Offset: 0x0016C255
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x0600270C RID: 9996 RVA: 0x0016DE73 File Offset: 0x0016C273
	Private Sub AniEvent_IncCmonCount()
		MyBase.animator.SetInteger("CmonCount", MyBase.animator.GetInteger("CmonCount") + 1)
	End Sub

	' Token: 0x0600270D RID: 9997 RVA: 0x0016DE97 File Offset: 0x0016C297
	Private Sub AniEvent_ResetCmonCount()
		MyBase.animator.SetInteger("CmonCount", 0)
	End Sub

	' Token: 0x0600270E RID: 9998 RVA: 0x0016DEAA File Offset: 0x0016C2AA
	Public Function throwPosition() As Vector3
		Return Me.throwPos.position
	End Function

	' Token: 0x0600270F RID: 9999 RVA: 0x0016DEB7 File Offset: 0x0016C2B7
	Public Function catchPosition() As Vector3
		Return Me.catchPos.position
	End Function

	' Token: 0x06002710 RID: 10000 RVA: 0x0016DEC4 File Offset: 0x0016C2C4
	Private Function EaseOvershoot(start As Single, [end] As Single, value As Single, overshoot As Single) As Single
		Dim num As Single = Mathf.Lerp(start, [end], value)
		Return num + Mathf.Sin(value * 3.1415927F) * (([end] - start) * overshoot)
	End Function

	' Token: 0x06002711 RID: 10001 RVA: 0x0016DEF1 File Offset: 0x0016C2F1
	Public Sub MoveToPos(endYPos As Single, distanceToCover As Single)
		If distanceToCover = 0F AndAlso Not Me.entering Then
			Me.ready = True
			Return
		End If
		Me.ready = False
		MyBase.StartCoroutine(Me.move_to_pos_cr(endYPos, distanceToCover))
	End Sub

	' Token: 0x06002712 RID: 10002 RVA: 0x0016DF27 File Offset: 0x0016C327
	Private Function InverseLerpUnclamped(a As Single, b As Single, value As Single) As Single
		Return(value - a) / (b - a)
	End Function

	' Token: 0x06002713 RID: 10003 RVA: 0x0016DF30 File Offset: 0x0016C330
	Private Iterator Function move_to_pos_cr(endYPos As Single, distanceToCover As Single) As IEnumerator
		Dim t As Single = 0F
		Dim startYPos As Single = Me.rootPosition.y
		Dim time As Single = If((distanceToCover <> 1F), Me.moveTimeLong, Me.moveTimeShort)
		If Me.entering Then
			time = 0.5F
			If Me.isLeft Then
				Me.SFX_OMM_P2_PuppetLeftRaiseUp()
				Me.SFX_OMM_P2_PuppetLeftRaiseUpVocal()
			Else
				Me.SFX_OMM_P2_PuppetRightRaiseUp()
				Me.SFX_OMM_P2_PuppetRightRaiseUpVocal()
			End If
		End If
		Dim wait As YieldInstruction = New WaitForFixedUpdate()
		Dim startEndAnimation As Boolean = False
		Dim movingUp As Boolean = endYPos > Me.rootPosition.y
		Dim moveBool As String = If((Not movingUp), If((distanceToCover <> 1F), "MovingDown", "MovingDownShort"), "MovingUp")
		MyBase.animator.SetBool(moveBool, True)
		Dim startAnimation As String = If((Not movingUp), If((distanceToCover <> 1F), "Move_Down_Start", "Move_Down_Short_Start"), "Move_Up_Start")
		If Not Me.dead Then
			If movingUp Then
				Yield MyBase.animator.WaitForAnimationToStart(Me, startAnimation, False)
			Else
				Yield MyBase.animator.WaitForAnimationToStart(Me, startAnimation, False)
			End If
		End If
		Dim wait24fps As WaitForFrameTimePersistent = New WaitForFrameTimePersistent(0.041666668F, False)
		While t < time
			t += CupheadTime.FixedDelta
			Me.rootPosition = New Vector3(Me.startPosition.x + Me.armBowingXModifier * Mathf.Sin(Me.InverseLerpUnclamped(startYPos, endYPos, Me.rootPosition.y) * 3.1415927F), Me.EaseOvershoot(startYPos, endYPos, t / time, Me.moveOvershoot), MyBase.transform.position.z)
			If t / time >= 0.35F AndAlso Not startEndAnimation Then
				MyBase.animator.SetBool(moveBool, False)
				startEndAnimation = True
			End If
			If t / time >= 0.6F AndAlso Me.entering Then
				Me.entering = False
				Me.colliders = MyBase.GetComponentsInChildren(Of Collider2D)()
				For Each collider2D As Collider2D In Me.colliders
					collider2D.enabled = True
				Next
			End If
			Yield wait24fps
		End While
		Me.rootPosition.x = Me.startPosition.x
		Me.armsHolding.GetComponent(Of SpriteRenderer)().sortingLayerName = "Enemies"
		Me.arms.transform.GetChild(0).GetComponent(Of SpriteRenderer)().sortingOrder = 10
		Dim target As Integer = Animator.StringToHash(MyBase.animator.GetLayerName(0) + "." + If((Not movingUp), If((distanceToCover <> 1F), "Move_Down_End", "Move_Down_Short_End"), "Move_Up_End"))
		While MyBase.animator.GetCurrentAnimatorStateInfo(0).fullPathHash = target
			Yield Nothing
		End While
		MyBase.animator.SetBool("TauntA", Not MyBase.animator.GetBool("TauntA"))
		Me.ready = True
		Return
	End Function

	' Token: 0x06002714 RID: 10004 RVA: 0x0016DF59 File Offset: 0x0016C359
	Public Sub StopTaunt()
		MyBase.animator.SetInteger("CmonCount", 5)
	End Sub

	' Token: 0x06002715 RID: 10005 RVA: 0x0016DF6C File Offset: 0x0016C36C
	Private Sub AniEvent_Catch()
		Me.main.CatchBall()
	End Sub

	' Token: 0x06002716 RID: 10006 RVA: 0x0016DF79 File Offset: 0x0016C379
	Public Sub AnIEvent_HoldingBall()
		Me.arms.SetActive(False)
		Me.armsHolding.SetActive(True)
	End Sub

	' Token: 0x06002717 RID: 10007 RVA: 0x0016DF93 File Offset: 0x0016C393
	Public Sub AnIEvent_NotHoldingBall()
		Me.arms.SetActive(True)
		Me.armsHolding.SetActive(False)
	End Sub

	' Token: 0x06002718 RID: 10008 RVA: 0x0016DFB0 File Offset: 0x0016C3B0
	Public Sub Die()
		Me.dead = True
		For Each collider2D As Collider2D In Me.colliders
			collider2D.enabled = False
		Next
		MyBase.animator.Play("Death")
		MyBase.GetComponent(Of LevelBossDeathExploder)().StartExplosion()
		If Me.rootPosition.y > 200F Then
			Me.StopAllCoroutines()
			MyBase.StartCoroutine(Me.move_to_pos_cr(180F, 1F))
		End If
	End Sub

	' Token: 0x06002719 RID: 10009 RVA: 0x0016E037 File Offset: 0x0016C437
	Private Sub AnimationEvent_SFX_OMM_P2_PuppetRightCatch()
		AudioManager.Play("sfx_dlc_omm_p2_puppet_right_catch")
		Me.emitAudioFromObject.Add("sfx_dlc_omm_p2_puppet_right_catch")
	End Sub

	' Token: 0x0600271A RID: 10010 RVA: 0x0016E053 File Offset: 0x0016C453
	Private Sub SFX_OMM_P2_PuppetRightRaiseUp()
		AudioManager.Play("sfx_dlc_omm_p2_puppet_right_raiseup")
		Me.emitAudioFromObject.Add("sfx_dlc_omm_p2_puppet_right_raiseup")
	End Sub

	' Token: 0x0600271B RID: 10011 RVA: 0x0016E06F File Offset: 0x0016C46F
	Private Sub SFX_OMM_P2_PuppetRightRaiseUpVocal()
		AudioManager.Play("sfx_dlc_omm_p2_puppet_right_raiseup_vocal")
		Me.emitAudioFromObject.Add("sfx_dlc_omm_p2_puppet_right_raiseup_vocal")
	End Sub

	' Token: 0x0600271C RID: 10012 RVA: 0x0016E08B File Offset: 0x0016C48B
	Private Sub AnimationEvent_SFX_OMM_P2_PuppetRightThrow()
		AudioManager.Play("sfx_dlc_omm_p2_puppet_right_throw")
		Me.emitAudioFromObject.Add("sfx_dlc_omm_p2_puppet_right_throw")
	End Sub

	' Token: 0x0600271D RID: 10013 RVA: 0x0016E0A7 File Offset: 0x0016C4A7
	Private Sub AnimationEvent_SFX_OMM_P2_PuppetRightThrowVocal()
		AudioManager.Play("sfx_dlc_omm_p2_puppet_right_throw_vocal")
		Me.emitAudioFromObject.Add("sfx_dlc_omm_p2_puppet_right_throw_vocal")
	End Sub

	' Token: 0x0600271E RID: 10014 RVA: 0x0016E0C3 File Offset: 0x0016C4C3
	Private Sub AnimationEvent_SFX_OMM_P2_PuppetLeftCatch()
		AudioManager.Play("sfx_dlc_omm_p2_puppet_left_catch")
		Me.emitAudioFromObject.Add("sfx_dlc_omm_p2_puppet_left_catch")
	End Sub

	' Token: 0x0600271F RID: 10015 RVA: 0x0016E0DF File Offset: 0x0016C4DF
	Private Sub SFX_OMM_P2_PuppetLeftRaiseUpVocal()
		AudioManager.Play("sfx_dlc_omm_p2_puppet_left_raiseup")
		Me.emitAudioFromObject.Add("sfx_dlc_omm_p2_puppet_left_raiseup")
	End Sub

	' Token: 0x06002720 RID: 10016 RVA: 0x0016E0FB File Offset: 0x0016C4FB
	Private Sub SFX_OMM_P2_PuppetLeftRaiseUp()
		MyBase.StartCoroutine(Me.SFX_OMM_P2_PuppetLeftRaiseUpVocal_cr())
	End Sub

	' Token: 0x06002721 RID: 10017 RVA: 0x0016E10C File Offset: 0x0016C50C
	Private Iterator Function SFX_OMM_P2_PuppetLeftRaiseUpVocal_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 0F)
		AudioManager.Play("sfx_dlc_omm_p2_puppet_left_raiseup_vocal")
		Me.emitAudioFromObject.Add("sfx_dlc_omm_p2_puppet_left_raiseup_vocal")
		Return
	End Function

	' Token: 0x06002722 RID: 10018 RVA: 0x0016E127 File Offset: 0x0016C527
	Private Sub AnimationEvent_SFX_OMM_P2_PuppetLeftThrow()
		AudioManager.Play("sfx_dlc_omm_p2_puppet_left_throw")
		Me.emitAudioFromObject.Add("sfx_dlc_omm_p2_puppet_left_throw")
	End Sub

	' Token: 0x06002723 RID: 10019 RVA: 0x0016E143 File Offset: 0x0016C543
	Private Sub AnimationEvent_SFX_OMM_P2_PuppetLeftThrowVocal()
		AudioManager.Play("sfx_dlc_omm_p2_puppet_left_throw_vocal")
		Me.emitAudioFromObject.Add("sfx_dlc_omm_p2_puppet_left_throw_vocal")
	End Sub

	' Token: 0x06002724 RID: 10020 RVA: 0x0016E15F File Offset: 0x0016C55F
	Private Sub WORKAROUND_NullifyFields()
		Me.arms = Nothing
		Me.armsHolding = Nothing
		Me.throwPos = Nothing
		Me.catchPos = Nothing
		Me.damageDealer = Nothing
		Me.main = Nothing
		Me.colliders = Nothing
	End Sub

	' Token: 0x04002FBD RID: 12221
	Private Const MOVING_UP As String = "MovingUp"

	' Token: 0x04002FBE RID: 12222
	Private Const MOVING_DOWN As String = "MovingDown"

	' Token: 0x04002FBF RID: 12223
	Private Const MOVING_DOWN_SHORT As String = "MovingDownShort"

	' Token: 0x04002FC0 RID: 12224
	Private Const MOVE_UP_START As String = "Move_Up_Start"

	' Token: 0x04002FC1 RID: 12225
	Private Const MOVE_DOWN_START As String = "Move_Down_Start"

	' Token: 0x04002FC2 RID: 12226
	Private Const MOVE_DOWN_SHORT_START As String = "Move_Down_Short_Start"

	' Token: 0x04002FC3 RID: 12227
	Private Const MOVE_UP_END As String = "Move_Up_End"

	' Token: 0x04002FC4 RID: 12228
	Private Const MOVE_DOWN_END As String = "Move_Down_End"

	' Token: 0x04002FC5 RID: 12229
	Private Const MOVE_DOWN_SHORT_END As String = "Move_Down_Short_End"

	' Token: 0x04002FC6 RID: 12230
	<SerializeField()>
	Private arms As GameObject

	' Token: 0x04002FC7 RID: 12231
	<SerializeField()>
	Private armsHolding As GameObject

	' Token: 0x04002FC8 RID: 12232
	<SerializeField()>
	Private throwPos As Transform

	' Token: 0x04002FC9 RID: 12233
	<SerializeField()>
	Private catchPos As Transform

	' Token: 0x04002FCA RID: 12234
	Public ready As Boolean

	' Token: 0x04002FCB RID: 12235
	Private damageDealer As DamageDealer

	' Token: 0x04002FCC RID: 12236
	<SerializeField()>
	Private armBowingXModifier As Single

	' Token: 0x04002FCD RID: 12237
	<SerializeField()>
	Private wobbleX As Single = 5F

	' Token: 0x04002FCE RID: 12238
	<SerializeField()>
	Private wobbleY As Single = 5F

	' Token: 0x04002FCF RID: 12239
	Public rootPosition As Vector3

	' Token: 0x04002FD0 RID: 12240
	Public startPosition As Vector3

	' Token: 0x04002FD1 RID: 12241
	<SerializeField()>
	Private wobbleTimer As Single

	' Token: 0x04002FD2 RID: 12242
	<SerializeField()>
	Private moveTimeShort As Single = 0.375F

	' Token: 0x04002FD3 RID: 12243
	<SerializeField()>
	Private moveTimeLong As Single = 0.5F

	' Token: 0x04002FD4 RID: 12244
	<SerializeField()>
	Private moveOvershoot As Single = 0.5F

	' Token: 0x04002FD5 RID: 12245
	<SerializeField()>
	Private isLeft As Boolean

	' Token: 0x04002FD6 RID: 12246
	Private entering As Boolean = True

	' Token: 0x04002FD7 RID: 12247
	Private dead As Boolean

	' Token: 0x04002FD8 RID: 12248
	<SerializeField()>
	Private main As OldManLevelSockPuppetHandler

	' Token: 0x04002FD9 RID: 12249
	Private colliders As Collider2D()
End Class
