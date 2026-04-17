Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020006E8 RID: 1768
Public Class MouseLevelCatPaw
	Inherits AbstractCollidableObject

	' Token: 0x170003C3 RID: 963
	' (get) Token: 0x060025DC RID: 9692 RVA: 0x00162DB9 File Offset: 0x001611B9
	' (set) Token: 0x060025DD RID: 9693 RVA: 0x00162DC1 File Offset: 0x001611C1
	Public Property state As MouseLevelCatPaw.State

	' Token: 0x060025DE RID: 9694 RVA: 0x00162DCA File Offset: 0x001611CA
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.initialPos = MyBase.transform.localPosition
		Me.damageDealer = DamageDealer.NewEnemy()
	End Sub

	' Token: 0x060025DF RID: 9695 RVA: 0x00162DF3 File Offset: 0x001611F3
	Private Sub Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x060025E0 RID: 9696 RVA: 0x00162E0B File Offset: 0x0016120B
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x060025E1 RID: 9697 RVA: 0x00162E29 File Offset: 0x00161229
	Public Sub Attack(properties As LevelProperties.Mouse.Claw)
		Me.properties = properties
		If Me.state = MouseLevelCatPaw.State.Idle Then
			Me.state = MouseLevelCatPaw.State.Attack
			MyBase.StartCoroutine(Me.attack_cr())
		End If
	End Sub

	' Token: 0x060025E2 RID: 9698 RVA: 0x00162E54 File Offset: 0x00161254
	Private Iterator Function attack_cr() As IEnumerator
		Dim totalMoveTime As Single = 0.584F
		Dim startX As Single = Me.initialPos.x
		Dim endX As Single = Me.initialPos.x + totalMoveTime * Me.properties.moveSpeed
		Dim hitAnim As Integer = Animator.StringToHash(MyBase.animator.GetLayerName(0) + ".Attack_Hit")
		Dim previousAnimationsTime As Single = 0F
		For i As Integer = 0 To 3 - 1
			MyBase.animator.SetTrigger("Attack")
			Dim animationTime As Single = If((i <> 0), 0.167F, 0.25F)
			Dim hitGround As Boolean = False
			While Not hitGround
				Yield New WaitForEndOfFrame()
				Dim animState As AnimatorStateInfo = MyBase.animator.GetCurrentAnimatorStateInfo(0)
				If animState.fullPathHash = hitAnim Then
					hitGround = True
					previousAnimationsTime += animationTime
					Dim moveProgress As Single = previousAnimationsTime / totalMoveTime
					MyBase.transform.SetLocalPosition(New Single?(Mathf.Lerp(startX, endX, moveProgress)), Nothing, Nothing)
					CupheadLevelCamera.Current.Shake(15F, 1F, False)
					Yield CupheadTime.WaitForSeconds(Me, Me.properties.holdGroundTime)
				Else
					Dim num As Single = animState.normalizedTime * animationTime
					Dim num2 As Single = (previousAnimationsTime + num) / totalMoveTime
					MyBase.transform.SetLocalPosition(New Single?(Mathf.Lerp(startX, endX, num2)), Nothing, Nothing)
				End If
			End While
		Next
		MyBase.animator.SetTrigger("Leave")
		MyBase.StartCoroutine(Me.timedAudioCatMeow_cr())
		Dim moveStartX As Single = MyBase.transform.localPosition.x
		Dim moveEndX As Single = Me.initialPos.x
		Dim leaveTime As Single = Mathf.Abs(moveEndX - moveStartX) / Me.properties.leaveSpeed
		Dim t As Single = 0F
		While t < leaveTime
			t += CupheadTime.Delta
			MyBase.transform.SetLocalPosition(New Single?(EaseUtils.Ease(EaseUtils.EaseType.easeInSine, moveStartX, moveEndX, t / leaveTime)), Nothing, Nothing)
			Yield Nothing
		End While
		MyBase.transform.SetLocalPosition(New Single?(moveEndX), Nothing, Nothing)
		Me.state = MouseLevelCatPaw.State.Idle
		Yield Nothing
		Return
	End Function

	' Token: 0x060025E3 RID: 9699 RVA: 0x00162E70 File Offset: 0x00161270
	Private Iterator Function timedAudioCatMeow_cr() As IEnumerator
		Yield New WaitForSeconds(1F)
		AudioManager.Play("level_mouse_cat_claw_end")
		Return
	End Function

	' Token: 0x060025E4 RID: 9700 RVA: 0x00162E84 File Offset: 0x00161284
	Private Sub SoundCatPawAttack()
		AudioManager.Play("level_mouse_cat_paw_attack")
		Me.emitAudioFromObject.Add("level_mouse_cat_paw_attack")
	End Sub

	' Token: 0x060025E5 RID: 9701 RVA: 0x00162EA0 File Offset: 0x001612A0
	Private Sub SoundCatMeowVoice()
		AudioManager.Play("level_mouse_cat_meow_voice")
		Me.emitAudioFromObject.Add("level_mouse_cat_meow_voice")
	End Sub

	' Token: 0x04002E69 RID: 11881
	Private properties As LevelProperties.Mouse.Claw

	' Token: 0x04002E6A RID: 11882
	Private initialPos As Vector2

	' Token: 0x04002E6B RID: 11883
	Private damageDealer As DamageDealer

	' Token: 0x020006E9 RID: 1769
	Public Enum State
		' Token: 0x04002E6D RID: 11885
		Idle
		' Token: 0x04002E6E RID: 11886
		Attack
	End Enum
End Class
