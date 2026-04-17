Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000366 RID: 870
Public Module AnimatorExtensions
	' Token: 0x060009AA RID: 2474 RVA: 0x0007C344 File Offset: 0x0007A744
	<System.Runtime.CompilerServices.ExtensionAttribute()>
	Public Function GetCurrentClipLength(animator As Animator, Optional layer As Integer = 0) As Single
		Dim currentAnimatorClipInfo As AnimatorClipInfo() = animator.GetCurrentAnimatorClipInfo(layer)
		If currentAnimatorClipInfo.Length = 0 Then
			Return 0F
		End If
		Dim clip As AnimationClip = currentAnimatorClipInfo(0).clip
		Return clip.length
	End Function

	' Token: 0x060009AB RID: 2475 RVA: 0x0007C37A File Offset: 0x0007A77A
	<System.Runtime.CompilerServices.ExtensionAttribute()>
	Public Sub FloorFrame(animator As Animator, Optional layer As Integer = 0)
		AnimatorExtensions.roundFrame(animator, layer, New Func(Of Single, Single)(Mathf.Floor))
	End Sub

	' Token: 0x060009AC RID: 2476 RVA: 0x0007C3A0 File Offset: 0x0007A7A0
	<System.Runtime.CompilerServices.ExtensionAttribute()>
	Public Sub RoundFrame(animator As Animator, Optional layer As Integer = 0)
		AnimatorExtensions.roundFrame(animator, layer, New Func(Of Single, Single)(Mathf.Round))
	End Sub

	' Token: 0x060009AD RID: 2477 RVA: 0x0007C3C8 File Offset: 0x0007A7C8
	Private Sub roundFrame(animator As Animator, layer As Integer, rounder As Func(Of Single, Single))
		Dim currentAnimatorClipInfo As AnimatorClipInfo() = animator.GetCurrentAnimatorClipInfo(layer)
		If currentAnimatorClipInfo.Length = 0 Then
			Return
		End If
		Dim clip As AnimationClip = currentAnimatorClipInfo(0).clip
		Dim frameRate As Single = clip.frameRate
		Dim length As Single = clip.length
		Dim normalizedTime As Single = animator.GetCurrentAnimatorStateInfo(layer).normalizedTime
		Dim num As Single = normalizedTime * length * frameRate
		Dim num2 As Single = rounder(num) / frameRate / length
		animator.Play(0, layer, num2)
	End Sub

	' Token: 0x060009AE RID: 2478 RVA: 0x0007C433 File Offset: 0x0007A833
	<System.Runtime.CompilerServices.ExtensionAttribute()>
	Public Function WaitForAnimationToStart(animator As Animator, parent As MonoBehaviour, animationName As String, Optional waitForEndOfFrame As Boolean = False) As Coroutine
		Return animator.WaitForAnimationToStart(parent, animationName, 0, waitForEndOfFrame)
	End Function

	' Token: 0x060009AF RID: 2479 RVA: 0x0007C440 File Offset: 0x0007A840
	<System.Runtime.CompilerServices.ExtensionAttribute()>
	Public Function WaitForAnimationToStart(animator As Animator, parent As MonoBehaviour, animationName As String, layer As Integer, Optional waitForEndOfFrame As Boolean = False) As Coroutine
		Dim num As Integer = Animator.StringToHash(animator.GetLayerName(layer) + "." + animationName)
		Return animator.WaitForAnimationToStart(parent, num, layer, waitForEndOfFrame)
	End Function

	' Token: 0x060009B0 RID: 2480 RVA: 0x0007C470 File Offset: 0x0007A870
	<System.Runtime.CompilerServices.ExtensionAttribute()>
	Public Function WaitForAnimationToStart(animator As Animator, parent As MonoBehaviour, animationHash As Integer, layer As Integer, Optional waitForEndOfFrame As Boolean = False) As Coroutine
		Return parent.StartCoroutine(AnimatorExtensions.waitForAnimStart_cr(animator, layer, animationHash, waitForEndOfFrame))
	End Function

	' Token: 0x060009B1 RID: 2481 RVA: 0x0007C484 File Offset: 0x0007A884
	Private Iterator Function waitForAnimStart_cr(animator As Animator, layer As Integer, animationHash As Integer, waitForEndOfFrame As Boolean) As IEnumerator
		While animator.GetCurrentAnimatorStateInfo(layer).fullPathHash <> animationHash
			If waitForEndOfFrame Then
				Yield New WaitForEndOfFrame()
			Else
				Yield Nothing
			End If
		End While
		Return
	End Function

	' Token: 0x060009B2 RID: 2482 RVA: 0x0007C4B4 File Offset: 0x0007A8B4
	<System.Runtime.CompilerServices.ExtensionAttribute()>
	Public Function WaitForAnimationToEnd(animator As Animator, parent As MonoBehaviour, Optional waitForEndOfFrame As Boolean = False) As Coroutine
		Return parent.StartCoroutine(AnimatorExtensions.waitForAnimEnd_cr(parent, animator, 0, waitForEndOfFrame))
	End Function

	' Token: 0x060009B3 RID: 2483 RVA: 0x0007C4C8 File Offset: 0x0007A8C8
	Private Iterator Function waitForAnimEnd_cr(parent As MonoBehaviour, animator As Animator, layer As Integer, waitForEndOfFrame As Boolean) As IEnumerator
		Dim current As Integer = animator.GetCurrentAnimatorStateInfo(layer).fullPathHash
		While current = animator.GetCurrentAnimatorStateInfo(layer).fullPathHash
			If waitForEndOfFrame Then
				Yield New WaitForEndOfFrame()
			Else
				Yield Nothing
			End If
		End While
		Return
	End Function

	' Token: 0x060009B4 RID: 2484 RVA: 0x0007C4F1 File Offset: 0x0007A8F1
	<System.Runtime.CompilerServices.ExtensionAttribute()>
	Public Function WaitForAnimationToEnd(animator As Animator, parent As MonoBehaviour, name As String, Optional waitForEndOfFrame As Boolean = False, Optional waitForStart As Boolean = True) As Coroutine
		Return animator.WaitForAnimationToEnd(parent, name, 0, waitForEndOfFrame, waitForStart)
	End Function

	' Token: 0x060009B5 RID: 2485 RVA: 0x0007C500 File Offset: 0x0007A900
	<System.Runtime.CompilerServices.ExtensionAttribute()>
	Public Function WaitForAnimationToEnd(animator As Animator, parent As MonoBehaviour, name As String, layer As Integer, Optional waitForEndOfFrame As Boolean = False, Optional waitForStart As Boolean = True) As Coroutine
		Dim num As Integer = Animator.StringToHash(animator.GetLayerName(layer) + "." + name)
		Return animator.WaitForAnimationToEnd(parent, num, layer, waitForEndOfFrame, waitForStart)
	End Function

	' Token: 0x060009B6 RID: 2486 RVA: 0x0007C532 File Offset: 0x0007A932
	<System.Runtime.CompilerServices.ExtensionAttribute()>
	Public Function WaitForAnimationToEnd(animator As Animator, parent As MonoBehaviour, animationHash As Integer, Optional layer As Integer = 0, Optional waitForEndOfFrame As Boolean = False, Optional waitForStart As Boolean = True) As Coroutine
		Return parent.StartCoroutine(AnimatorExtensions.waitForNamedAnimEnd_cr(parent, animator, animationHash, layer, waitForEndOfFrame, waitForStart))
	End Function

	' Token: 0x060009B7 RID: 2487 RVA: 0x0007C548 File Offset: 0x0007A948
	Private Iterator Function waitForNamedAnimEnd_cr(parent As MonoBehaviour, animator As Animator, animationHash As Integer, layer As Integer, waitForEndOfFrame As Boolean, Optional waitForStart As Boolean = True) As IEnumerator
		If waitForStart Then
			Yield parent.StartCoroutine(AnimatorExtensions.waitForAnimStart_cr(animator, layer, animationHash, waitForEndOfFrame))
		End If
		While animator.GetCurrentAnimatorStateInfo(layer).fullPathHash = animationHash
			If waitForEndOfFrame Then
				Yield New WaitForEndOfFrame()
			Else
				Yield Nothing
			End If
		End While
		Return
	End Function

	' Token: 0x060009B8 RID: 2488 RVA: 0x0007C588 File Offset: 0x0007A988
	<System.Runtime.CompilerServices.ExtensionAttribute()>
	Public Function WaitForNormalizedTime(animator As Animator, parent As MonoBehaviour, normalizedTime As Single, Optional name As String = Nothing, Optional layer As Integer = 0, Optional allowEqualTime As Boolean = False, Optional waitForEndOfFrame As Boolean = False, Optional waitForStart As Boolean = True) As Coroutine
		Dim num As Integer? = Nothing
		If name IsNot Nothing Then
			num = New Integer?(Animator.StringToHash(animator.GetLayerName(layer) + "." + name))
		End If
		Return animator.WaitForNormalizedTime(parent, normalizedTime, num, layer, allowEqualTime, waitForEndOfFrame, waitForStart)
	End Function

	' Token: 0x060009B9 RID: 2489 RVA: 0x0007C5D4 File Offset: 0x0007A9D4
	<System.Runtime.CompilerServices.ExtensionAttribute()>
	Public Function WaitForNormalizedTime(animator As Animator, parent As MonoBehaviour, normalizedTime As Single, animationHash As Integer?, Optional layer As Integer = 0, Optional allowEqualTime As Boolean = False, Optional waitForEndOfFrame As Boolean = False, Optional waitForStart As Boolean = True) As Coroutine
		Return parent.StartCoroutine(AnimatorExtensions.waitForNormalizedTime_cr(parent, animator, normalizedTime, animationHash, layer, allowEqualTime, waitForEndOfFrame, waitForStart, False))
	End Function

	' Token: 0x060009BA RID: 2490 RVA: 0x0007C5FC File Offset: 0x0007A9FC
	<System.Runtime.CompilerServices.ExtensionAttribute()>
	Public Function WaitForNormalizedTimeLooping(animator As Animator, parent As MonoBehaviour, normalizedTimeDecimal As Single, Optional name As String = Nothing, Optional layer As Integer = 0, Optional allowEqualTime As Boolean = False, Optional waitForEndOfFrame As Boolean = False, Optional waitForStart As Boolean = True) As Coroutine
		Dim num As Integer? = Nothing
		If name IsNot Nothing Then
			num = New Integer?(Animator.StringToHash(animator.GetLayerName(layer) + "." + name))
		End If
		Return parent.StartCoroutine(AnimatorExtensions.waitForNormalizedTime_cr(parent, animator, normalizedTimeDecimal, num, layer, allowEqualTime, waitForEndOfFrame, waitForStart, True))
	End Function

	' Token: 0x060009BB RID: 2491 RVA: 0x0007C650 File Offset: 0x0007AA50
	Private Iterator Function waitForNormalizedTime_cr(parent As MonoBehaviour, animator As Animator, normalizedTime As Single, animationHash As Integer?, layer As Integer, allowEqualTime As Boolean, waitForEndOfFrame As Boolean, waitForStart As Boolean, looping As Boolean) As IEnumerator
		If animationHash IsNot Nothing AndAlso waitForStart Then
			Yield parent.StartCoroutine(AnimatorExtensions.waitForAnimStart_cr(animator, layer, animationHash.Value, waitForEndOfFrame))
		End If
		Dim target As Integer
		If animationHash IsNot Nothing Then
			target = animationHash.Value
		Else
			target = animator.GetCurrentAnimatorStateInfo(layer).fullPathHash
		End If
		While True
			Dim stateInfo As AnimatorStateInfo = animator.GetCurrentAnimatorStateInfo(layer)
			Dim num As Single = If((Not looping), stateInfo.normalizedTime, MathUtilities.DecimalPart(stateInfo.normalizedTime))
			If If((Not allowEqualTime), (stateInfo.normalizedTime >= normalizedTime), (stateInfo.normalizedTime > normalizedTime)) OrElse stateInfo.fullPathHash <> target Then
				Exit For
			End If
			If waitForEndOfFrame Then
				Yield New WaitForEndOfFrame()
			Else
				Yield Nothing
			End If
		End While
		Return
	End Function
End Module
