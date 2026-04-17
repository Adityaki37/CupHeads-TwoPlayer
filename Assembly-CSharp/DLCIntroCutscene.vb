Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000402 RID: 1026
Public Class DLCIntroCutscene
	Inherits DLCGenericCutscene

	' Token: 0x06000E3C RID: 3644 RVA: 0x00091BDE File Offset: 0x0008FFDE
	Protected Overrides Sub Start()
		MyBase.Start()
		Me.allowScreenSkip = True
		Me.BGanim.speed = Me.screen4BGScrollSpeed
		Me.screen4ForestScrollSpeed = Me.screen4ForestScrollStartSpeed
		AudioManager.PlayLoop("sfx_dlc_intro_oceanamb_loop")
	End Sub

	' Token: 0x06000E3D RID: 3645 RVA: 0x00091C14 File Offset: 0x00090014
	Protected Overrides Sub OnScreenSkip()
		MyBase.StartCoroutine(Me.skip_title_cr())
	End Sub

	' Token: 0x06000E3E RID: 3646 RVA: 0x00091C24 File Offset: 0x00090024
	Private Iterator Function skip_title_cr() As IEnumerator
		Me.allowScreenSkip = False
		MyBase.IrisIn()
		Yield CupheadTime.WaitForSeconds(Me, 0.9F)
		Me.screens(Me.curScreen).Play("End")
		Return
	End Function

	' Token: 0x06000E3F RID: 3647 RVA: 0x00091C40 File Offset: 0x00090040
	Protected Overrides Sub OnScreenAdvance(which As Integer)
		If which = 0 Then
			Me.canvas.SetActive(True)
			AudioManager.StartBGMAlternate(0)
			AudioManager.[Stop]("sfx_dlc_intro_oceanamb_loop")
		End If
		If which < Me.astralPlanePositions.Length AndAlso Me.astralPlanePositions(which) Then
			Me.astralPlaneController.position = Me.astralPlanePositions(which).position
			Me.astralPlaneController.localScale = Me.astralPlanePositions(which).localScale
		End If
	End Sub

	' Token: 0x06000E40 RID: 3648 RVA: 0x00091CC0 File Offset: 0x000900C0
	Protected Overrides Sub OnContinue()
		Me.allowScreenSkip = False
		If Me.curScreen = 3 AndAlso Not Me.BGanim.GetBool("End") Then
			Me.BGanim.SetBool("End", True)
			MyBase.StartCoroutine(Me.slow_down_bg_cr())
		End If
	End Sub

	' Token: 0x06000E41 RID: 3649 RVA: 0x00091D14 File Offset: 0x00090114
	Private Iterator Function slow_down_bg_cr() As IEnumerator
		Dim wait As YieldInstruction = New WaitForFixedUpdate()
		Dim [end] As Single = Me.screen4BGScrollSpeed / 2F
		While Not Me.BGanim.GetCurrentAnimatorStateInfo(0).IsName("End") AndAlso Not Me.BGanim.GetCurrentAnimatorStateInfo(0).IsName("AltEnd")
			Yield Nothing
		End While
		For Each gameObject As GameObject In Me.screen4Characters
			gameObject.transform.parent = Me.screen4ScrollEnd.transform
		Next
		While Me.BGanim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1F
			Me.BGanim.speed = EaseUtils.EaseOutSine(Me.screen4BGScrollSpeed, [end], Me.BGanim.GetCurrentAnimatorStateInfo(0).normalizedTime)
			Me.screen4ForestScrollSpeed = Me.screen4ForestScrollStartSpeed * (Me.BGanim.speed / Me.screen4BGScrollSpeed)
			For Each gameObject2 As GameObject In Me.screen4Characters
				gameObject2.transform.localPosition += Vector3.right * 6.4F
			Next
			Yield wait
		End While
		Me.screen4ForestScrollSpeed = 0F
		Yield Me.screens(Me.curScreen).WaitForAnimationToStart(Me, "holdforBG", False)
		Me.screens(Me.curScreen).SetTrigger("Continue")
		While Me.screen4Characters(2).transform.localPosition.x < 1420F
			For Each gameObject3 As GameObject In Me.screen4Characters
				gameObject3.transform.localPosition += Vector3.right * 6.4F
			Next
			Yield wait
		End While
		Return
	End Function

	' Token: 0x06000E42 RID: 3650 RVA: 0x00091D30 File Offset: 0x00090130
	Protected Overrides Sub Update()
		MyBase.Update()
		If Me.curScreen = 0 Then
			CupheadCutsceneCamera.Current.SetPosition(Me.cameraPos.transform.position)
		Else
			CupheadCutsceneCamera.Current.SetPosition(Vector3.zero)
		End If
		If Me.curScreen = 3 Then
			Me.screen4Forest.transform.localPosition += Vector3.left * Me.screen4ForestScrollSpeed * CupheadTime.Delta
			Me.screen4Clouds.transform.localPosition += Vector3.left * Me.screen4ForestScrollSpeed * CupheadTime.Delta * 0.5F
			Me.screen4FG.transform.localPosition += Vector3.left * Me.screen4ForestScrollSpeed * CupheadTime.Delta * 1.5F
		End If
	End Sub

	' Token: 0x06000E43 RID: 3651 RVA: 0x00091E4C File Offset: 0x0009024C
	Private Sub LateUpdate()
		If Me.screen4Forest.transform.localPosition.x < -2560F Then
			Me.screen4Forest.transform.localPosition += Vector3.right * 1280F
		End If
		If Me.screen4Clouds.transform.localPosition.x < -2560F Then
			Me.screen4Clouds.transform.localPosition += Vector3.right * 1280F
		End If
		If Me.screen4FG.transform.localPosition.x < -5156F Then
			Me.screen4FG.transform.localPosition += Vector3.right * 4767F
		End If
		If Me.screen4ScrollStart.transform.position.x < -1600F Then
			Me.screen4ScrollStart.enabled = False
			Me.screen4EndLoopBack.enabled = True
		End If
	End Sub

	' Token: 0x0400177D RID: 6013
	<SerializeField()>
	Private canvas As GameObject

	' Token: 0x0400177E RID: 6014
	<SerializeField()>
	Private cameraPos As GameObject

	' Token: 0x0400177F RID: 6015
	<SerializeField()>
	Private BGanim As Animator

	' Token: 0x04001780 RID: 6016
	<SerializeField()>
	Private astralPlaneController As Transform

	' Token: 0x04001781 RID: 6017
	<SerializeField()>
	Private astralPlanePositions As Transform()

	' Token: 0x04001782 RID: 6018
	<SerializeField()>
	Private screen4BGScrollSpeed As Single = 0.1F

	' Token: 0x04001783 RID: 6019
	<SerializeField()>
	Private screen4ForestScrollStartSpeed As Single = 325F

	' Token: 0x04001784 RID: 6020
	Private screen4ForestScrollSpeed As Single

	' Token: 0x04001785 RID: 6021
	<SerializeField()>
	Private screen4Characters As GameObject()

	' Token: 0x04001786 RID: 6022
	<SerializeField()>
	Private screen4Forest As GameObject

	' Token: 0x04001787 RID: 6023
	<SerializeField()>
	Private screen4Clouds As GameObject

	' Token: 0x04001788 RID: 6024
	<SerializeField()>
	Private screen4FG As GameObject

	' Token: 0x04001789 RID: 6025
	<SerializeField()>
	Private screen4ScrollEnd As GameObject

	' Token: 0x0400178A RID: 6026
	<SerializeField()>
	Private screen4ScrollStart As SpriteRenderer

	' Token: 0x0400178B RID: 6027
	<SerializeField()>
	Private screen4EndLoopBack As SpriteRenderer

	' Token: 0x0400178C RID: 6028
	<SerializeField()>
	<Range(-1F, 7F)>
	Private fastForward As Integer = -1
End Class
