Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020006FC RID: 1788
Public Class OldManLevelBear
	Inherits BasicDamageDealingObject

	' Token: 0x06002649 RID: 9801 RVA: 0x00165AC0 File Offset: 0x00163EC0
	Public Iterator Function fall_cr() As IEnumerator
		Me.thrown = True
		Dim startPos As Vector3 = New Vector3(-520F, 525F)
		Dim endPos As Vector3 = New Vector3(-600F, -90F)
		Dim startScale As Vector3 = New Vector3(1F, 1F)
		Dim endScale As Vector3 = New Vector3(0.15F, 0.15F)
		MyBase.transform.position = startPos
		MyBase.animator.Play("Falling")
		Dim t As Single = 0F
		Me.rend.sortingLayerName = "Background"
		Me.rend.sortingOrder = 1000
		While t < 0.7916667F
			Yield CupheadTime.WaitForSeconds(Me, 0.020833334F)
			t += 0.020833334F
			Dim tn As Single = t / 0.7916667F
			MyBase.transform.position = New Vector3(Mathf.Lerp(startPos.x, endPos.x, EaseUtils.EaseOutSine(0F, 1F, tn)), Mathf.Lerp(startPos.y, endPos.y, EaseUtils.EaseInSine(0F, 1F, tn)))
			MyBase.transform.localScale = Vector3.Lerp(startScale, endScale, EaseUtils.EaseOutSine(0F, 1F, tn))
		End While
		Yield Nothing
		MyBase.transform.localScale = New Vector3(1F, 1F)
		MyBase.animator.Play("FX")
		MyBase.animator.Update(0F)
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "FX", False, True)
		Me.rend.enabled = False
		Me.rend.sortingOrder = 0
		Me.rend.sortingLayerName = "Projectiles"
		Return
	End Function

	' Token: 0x0600264A RID: 9802 RVA: 0x00165ADB File Offset: 0x00163EDB
	Private Sub AnimationEvent_SFX_OMM_BearAttackClawing()
		AudioManager.Play("sfx_dlc_omm_bearattack_clawing")
		Me.emitAudioFromObject.Add("sfx_dlc_omm_bearattack_clawing")
	End Sub

	' Token: 0x0600264B RID: 9803 RVA: 0x00165AF7 File Offset: 0x00163EF7
	Private Sub AnimationEvent_SFX_OMM_BearAttackGrowling()
		AudioManager.Play("sfx_dlc_omm_bearattack_growling")
		Me.emitAudioFromObject.Add("sfx_dlc_omm_bearattack_growling")
	End Sub

	' Token: 0x0600264C RID: 9804 RVA: 0x00165B13 File Offset: 0x00163F13
	Private Sub AnimationEvent_SFX_OMM_BearAttackEnd()
		AudioManager.[Stop]("sfx_dlc_omm_bearattack_growling")
		AudioManager.Play("sfx_dlc_omm_bearattack_end")
		Me.emitAudioFromObject.Add("sfx_dlc_omm_bearattack_end")
	End Sub

	' Token: 0x04002ED1 RID: 11985
	Private Const FALL_TIME As Single = 0.7916667F

	' Token: 0x04002ED2 RID: 11986
	<SerializeField()>
	Private rend As SpriteRenderer

	' Token: 0x04002ED3 RID: 11987
	Public thrown As Boolean
End Class
