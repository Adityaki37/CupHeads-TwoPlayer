Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000555 RID: 1365
Public Class ClownLevelAnimationManager
	Inherits AbstractPausableComponent

	' Token: 0x06001979 RID: 6521 RVA: 0x000E71D9 File Offset: 0x000E55D9
	Protected Overrides Sub Awake()
		MyBase.Awake()
	End Sub

	' Token: 0x0600197A RID: 6522 RVA: 0x000E71E4 File Offset: 0x000E55E4
	Private Sub Start()
		Me.headSprite = Me.headSprite.GetComponent(Of Animator)()
		Me.pivotPoint.position = Me.balloonSprite.position
		MyBase.StartCoroutine(Me.head_cr())
		MyBase.StartCoroutine(Me.balloon_loop_cr())
		For Each animator As Animator In Me.twelveFpsAnimations
			MyBase.StartCoroutine(Me.manual_fps_animation_cr(animator, 0.083333336F))
		Next
		For Each animator2 As Animator In Me.twentyFourFpsAnimations
			MyBase.StartCoroutine(Me.manual_fps_animation_cr(animator2, 0.041666668F))
		Next
	End Sub

	' Token: 0x0600197B RID: 6523 RVA: 0x000E72A0 File Offset: 0x000E56A0
	Private Iterator Function head_cr() As IEnumerator
		While True
			Dim getSeconds As Single = Global.UnityEngine.Random.Range(3F, 8F)
			Me.headSprite.SetTrigger("Continue")
			Yield CupheadTime.WaitForSeconds(Me, getSeconds)
		End While
		Return
	End Function

	' Token: 0x0600197C RID: 6524 RVA: 0x000E72BC File Offset: 0x000E56BC
	Private Iterator Function balloon_loop_cr() As IEnumerator
		Dim loopSize As Single = 20F
		Dim speed As Single = 1F
		Dim angle As Single = 0F
		While True
			Dim pivotOffset As Vector3 = Vector3.left * 2F * loopSize
			angle += speed * CupheadTime.Delta
			If angle > 6.2831855F Then
				Me.invert = Not Me.invert
				angle -= 6.2831855F
			End If
			If angle < 0F Then
				angle += 6.2831855F
			End If
			Dim value As Single
			If Me.invert Then
				Me.balloonSprite.position = Me.pivotPoint.position + pivotOffset
				value = 1F
			Else
				Me.balloonSprite.position = Me.pivotPoint.position
				value = -1F
			End If
			Dim handleRotationX As Vector3 = New Vector3(Mathf.Cos(angle) * value * loopSize, 0F, 0F)
			Dim handleRotationY As Vector3 = New Vector3(0F, Mathf.Sin(angle) * loopSize, 0F)
			Me.balloonSprite.position += handleRotationX + handleRotationY
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x0600197D RID: 6525 RVA: 0x000E72D8 File Offset: 0x000E56D8
	Private Iterator Function manual_fps_animation_cr(ani As Animator, fps As Single) As IEnumerator
		Dim frameTime As Single = 0F
		While True
			frameTime += CupheadTime.Delta
			If frameTime > fps Then
				frameTime -= fps
				ani.enabled = True
				ani.Update(fps)
				ani.enabled = False
			End If
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x04002292 RID: 8850
	<SerializeField()>
	Private headSprite As Animator

	' Token: 0x04002293 RID: 8851
	<SerializeField()>
	Private balloonSprite As Transform

	' Token: 0x04002294 RID: 8852
	<SerializeField()>
	Private pivotPoint As Transform

	' Token: 0x04002295 RID: 8853
	<SerializeField()>
	Private twelveFpsAnimations As Animator()

	' Token: 0x04002296 RID: 8854
	<SerializeField()>
	Private twentyFourFpsAnimations As Animator()

	' Token: 0x04002297 RID: 8855
	Private invert As Boolean
End Class
