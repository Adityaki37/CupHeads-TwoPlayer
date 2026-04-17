Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020007D2 RID: 2002
Public Class SaltbakerLevelPhaseThreeToFourTransition
	Inherits MonoBehaviour

	' Token: 0x06002D71 RID: 11633 RVA: 0x001ACA7B File Offset: 0x001AAE7B
	Public Sub StartSaltman()
		Me.anim.Play("Start")
	End Sub

	' Token: 0x06002D72 RID: 11634 RVA: 0x001ACA8D File Offset: 0x001AAE8D
	Public Sub StartHeart()
		MyBase.StartCoroutine(Me.move_heart_cr())
		Me.anim.Play("Heart", 1, 0F)
	End Sub

	' Token: 0x06002D73 RID: 11635 RVA: 0x001ACAB4 File Offset: 0x001AAEB4
	Private Iterator Function move_heart_cr() As IEnumerator
		Yield Me.anim.WaitForAnimationToStart(Me, "HeartLoop", 1, False)
		Dim start As Vector3 = Me.heart.transform.position
		Dim [end] As Vector3 = start + Vector3.up * 300F
		Dim t As Single = 0F
		While t < 1F
			Me.heart.transform.position = Vector3.Lerp(start, [end], EaseUtils.EaseInSine(0F, 1F, t))
			Yield CupheadTime.WaitForSeconds(Me, 0.083333336F)
			t += 0.083333336F
		End While
		MyBase.enabled = False
		Return
	End Function

	' Token: 0x06002D74 RID: 11636 RVA: 0x001ACACF File Offset: 0x001AAECF
	Private Sub AnimationEvent_SFX_SALTB_Phase3to4_HeartRise()
		AudioManager.Play("sfx_dlc_saltbaker_p3top4transition_heartrise")
	End Sub

	' Token: 0x06002D75 RID: 11637 RVA: 0x001ACADB File Offset: 0x001AAEDB
	Private Sub AnimationEvent_SFX_SALTB_Phase3to4_Transition()
		AudioManager.Play("sfx_dlc_saltbaker_p3top4transition")
	End Sub

	' Token: 0x06002D76 RID: 11638 RVA: 0x001ACAE7 File Offset: 0x001AAEE7
	Private Sub AnimationEvent_SFX_SALTB_Phase3to4_TransitionStart()
		AudioManager.Play("sfx_dlc_saltbaker_p3top4transition_start")
	End Sub

	' Token: 0x040035F7 RID: 13815
	<SerializeField()>
	Private anim As Animator

	' Token: 0x040035F8 RID: 13816
	<SerializeField()>
	Private heart As GameObject
End Class
