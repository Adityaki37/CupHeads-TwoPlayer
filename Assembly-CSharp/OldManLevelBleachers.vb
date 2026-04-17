Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020006FE RID: 1790
Public Class OldManLevelBleachers
	Inherits AbstractPausableComponent

	' Token: 0x06002653 RID: 9811 RVA: 0x00165FEA File Offset: 0x001643EA
	Private Sub Start()
		MyBase.StartCoroutine(Me.move_bleachers_cr())
	End Sub

	' Token: 0x06002654 RID: 9812 RVA: 0x00165FFC File Offset: 0x001643FC
	Private Iterator Function move_bleachers_cr() As IEnumerator
		Dim rightStartPos As Vector3 = Me.gnomeBleacherRight.transform.localPosition
		Dim leftStartPos As Vector3 = Me.gnomeBleacherLeft.transform.localPosition
		Dim rightStepStartPos As Vector3 = rightStartPos
		Dim leftStepStartPos As Vector3 = leftStartPos
		Me.SFX_OMM_P2_PuppetBleachersRaiseUp()
		Me.SFX_OMM_BleachersCrowdLoop()
		Yield Nothing
		AudioManager.FadeSFXVolume("sfx_dlc_omm_p2_bleacherscrowd_loop", 0.15F, 0.5F)
		For i As Integer = 0 To 3 - 1
			Dim t As Single = 0F
			Dim time As Single = Me.enterStepTime
			Dim rightEndPos As Vector3 = Vector3.Lerp(rightStartPos, Me.gnomeBleacherRightEnd.position, 0.5F + CSng(i) * 0.25F)
			Dim leftEndPos As Vector3 = Vector3.Lerp(leftStartPos, Me.gnomeBleacherLeftEnd.position, 0.5F + CSng(i) * 0.25F)
			While t < time + Me.offset
				t += CupheadTime.Delta
				Me.gnomeBleacherRight.transform.localPosition = Vector3.Lerp(rightStepStartPos, rightEndPos, EaseUtils.EaseOutBounce(0F, 1F, Mathf.Clamp((t - Me.offset) / time, 0F, 1F)))
				Me.gnomeBleacherLeft.transform.localPosition = Vector3.Lerp(leftStepStartPos, leftEndPos, EaseUtils.EaseOutBounce(0F, 1F, Mathf.Clamp(t / time, 0F, 1F)))
				Yield Nothing
			End While
			rightStepStartPos = Me.gnomeBleacherRight.transform.localPosition
			leftStepStartPos = Me.gnomeBleacherLeft.transform.localPosition
			Yield CupheadTime.WaitForSeconds(Me, Me.enterStepPause)
		Next
		While Me.level.InPhase2()
			Yield Nothing
		End While
		Me.SFX_OMM_P2_End_BleacherPuppetsLower()
		AudioManager.FadeSFXVolume("sfx_dlc_omm_p2_bleacherscrowd_loop", 0F, 1.5F)
		For j As Integer = 0 To 3 - 1
			Dim t As Single = 0F
			Dim time As Single = Me.exitStepTime
			Dim rightEndPos2 As Vector3 = Vector3.Lerp(Me.gnomeBleacherRightEnd.position, rightStartPos, CSng((j + 1)) * 0.333F)
			Dim leftEndPos2 As Vector3 = Vector3.Lerp(Me.gnomeBleacherLeftEnd.position, leftStartPos, CSng((j + 1)) * 0.333F)
			While t < time + Me.offset
				t += CupheadTime.Delta
				Me.gnomeBleacherRight.transform.localPosition = Vector3.Lerp(rightStepStartPos, rightEndPos2, EaseUtils.EaseOutElastic(0F, 1F, Mathf.Clamp((t - Me.offset) / time, 0F, 1F)))
				Me.gnomeBleacherLeft.transform.localPosition = Vector3.Lerp(leftStepStartPos, leftEndPos2, EaseUtils.EaseOutElastic(0F, 1F, Mathf.Clamp(t / time, 0F, 1F)))
				Yield Nothing
			End While
			rightStepStartPos = Me.gnomeBleacherRight.transform.localPosition
			leftStepStartPos = Me.gnomeBleacherLeft.transform.localPosition
			Yield CupheadTime.WaitForSeconds(Me, Me.exitStepPause)
		Next
		MyBase.gameObject.SetActive(False)
		Yield Nothing
		Return
	End Function

	' Token: 0x06002655 RID: 9813 RVA: 0x00166017 File Offset: 0x00164417
	Private Sub SFX_OMM_P2_PuppetBleachersRaiseUp()
		AudioManager.Play("sfx_dlc_omm_p2_puppet_bleachersraiseup")
		Me.emitAudioFromObject.Add("sfx_dlc_omm_p2_puppet_bleachersraiseup")
	End Sub

	' Token: 0x06002656 RID: 9814 RVA: 0x00166033 File Offset: 0x00164433
	Private Sub SFX_OMM_BleachersCrowdLoop()
		AudioManager.FadeSFXVolume("sfx_dlc_omm_p2_bleacherscrowd_loop", 0.001F, 0.001F)
		AudioManager.PlayLoop("sfx_dlc_omm_p2_bleacherscrowd_loop")
		Me.emitAudioFromObject.Add("sfx_dlc_omm_p2_bleacherscrowd_loop")
	End Sub

	' Token: 0x06002657 RID: 9815 RVA: 0x00166063 File Offset: 0x00164463
	Private Sub SFX_OMM_P2_End_BleacherPuppetsLower()
		AudioManager.[Stop]("sfx_dlc_omm_p2_bleacherscrowd_loop")
		AudioManager.Play("sfx_dlc_omm_p2_end_bleacherpuppetslower")
		Me.emitAudioFromObject.Add("sfx_dlc_omm_p2_end_bleacherpuppetslower")
	End Sub

	' Token: 0x04002EDC RID: 11996
	<SerializeField()>
	Private gnomeBleacherRight As GameObject

	' Token: 0x04002EDD RID: 11997
	<SerializeField()>
	Private gnomeBleacherRightEnd As Transform

	' Token: 0x04002EDE RID: 11998
	<SerializeField()>
	Private gnomeBleacherLeft As GameObject

	' Token: 0x04002EDF RID: 11999
	<SerializeField()>
	Private gnomeBleacherLeftEnd As Transform

	' Token: 0x04002EE0 RID: 12000
	<SerializeField()>
	Private level As OldManLevel

	' Token: 0x04002EE1 RID: 12001
	<SerializeField()>
	Private enterStepTime As Single = 0.6F

	' Token: 0x04002EE2 RID: 12002
	<SerializeField()>
	Private enterStepPause As Single = 0.1F

	' Token: 0x04002EE3 RID: 12003
	<SerializeField()>
	Private exitStepTime As Single = 0.3F

	' Token: 0x04002EE4 RID: 12004
	<SerializeField()>
	Private exitStepPause As Single = 0.05F

	' Token: 0x04002EE5 RID: 12005
	<SerializeField()>
	Private offset As Single = 0.1F
End Class
