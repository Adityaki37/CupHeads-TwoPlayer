Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x0200064A RID: 1610
Public Class FlyingCowboyLevelBeans
	Inherits AbstractProjectile

	' Token: 0x06002115 RID: 8469 RVA: 0x00131E8C File Offset: 0x0013028C
	Public Overridable Sub Init(position As Vector3, pointingUp As Boolean, speed As Single, extendTimer As Single)
		MyBase.ResetLifetime()
		MyBase.ResetDistance()
		MyBase.transform.position = position
		Dim array As GameObject() = If((Not Rand.Bool()), Me.versionB, Me.versionA)
		For Each gameObject As GameObject In array
			gameObject.SetActive(False)
		Next
		If Not pointingUp Then
			MyBase.animator.Play("BottomIdle")
			MyBase.animator.Update(0F)
		End If
		MyBase.animator.Play(0, 0, Global.UnityEngine.Random.Range(0F, 1F))
		MyBase.StartCoroutine(Me.move_cr(speed))
		MyBase.StartCoroutine(Me.extend_cr(extendTimer))
	End Sub

	' Token: 0x06002116 RID: 8470 RVA: 0x00131F4D File Offset: 0x0013034D
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
		MyBase.OnCollisionPlayer(hit, phase)
	End Sub

	' Token: 0x06002117 RID: 8471 RVA: 0x00131F6C File Offset: 0x0013036C
	Private Iterator Function move_cr(speed As Single) As IEnumerator
		Dim wait As WaitForFixedUpdate = New WaitForFixedUpdate()
		While True
			MyBase.transform.position += New Vector3(-speed * CupheadTime.FixedDelta, 0F)
			If MyBase.transform.position.x < -745F Then
				Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
			End If
			Yield wait
		End While
		Return
	End Function

	' Token: 0x06002118 RID: 8472 RVA: 0x00131F90 File Offset: 0x00130390
	Private Iterator Function extend_cr(extendTimer As Single) As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, extendTimer)
		MyBase.animator.SetTrigger("Extend")
		Return
	End Function

	' Token: 0x06002119 RID: 8473 RVA: 0x00131FB2 File Offset: 0x001303B2
	Private Sub SFX_COWGIRL_P3_CanPropellerLoop()
		AudioManager.FadeSFXVolume("sfx_dlc_cowgirl_p3_canpropeller_loop", 0.4F, 0.5F)
		Me.emitAudioFromObject.Add("sfx_dlc_cowgirl_p3_canpropeller_loop")
	End Sub

	' Token: 0x0600211A RID: 8474 RVA: 0x00131FD8 File Offset: 0x001303D8
	Private Sub AnimationEvent_SFX_COWGIRL_P3_CanUnfurl()
		AudioManager.Play("sfx_dlc_cowgirl_p3_canpropeller_unfurl")
		Me.emitAudioFromObject.Add("sfx_dlc_cowgirl_p3_canpropeller_unfurl")
	End Sub

	' Token: 0x040029B3 RID: 10675
	<SerializeField()>
	Private versionA As GameObject()

	' Token: 0x040029B4 RID: 10676
	<SerializeField()>
	Private versionB As GameObject()
End Class
