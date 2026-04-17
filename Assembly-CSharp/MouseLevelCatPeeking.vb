Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020006EA RID: 1770
Public Class MouseLevelCatPeeking
	Inherits MonoBehaviour

	' Token: 0x170003C4 RID: 964
	' (get) Token: 0x060025E7 RID: 9703 RVA: 0x00163399 File Offset: 0x00161799
	Public ReadOnly Property Peek1Threshold As Single
		Get
			Return Me.peek1Threshold
		End Get
	End Property

	' Token: 0x170003C5 RID: 965
	' (get) Token: 0x060025E8 RID: 9704 RVA: 0x001633A1 File Offset: 0x001617A1
	Public ReadOnly Property Peek2Threshold As Single
		Get
			Return Me.peek2Threshold
		End Get
	End Property

	' Token: 0x170003C6 RID: 966
	' (get) Token: 0x060025E9 RID: 9705 RVA: 0x001633A9 File Offset: 0x001617A9
	' (set) Token: 0x060025EA RID: 9706 RVA: 0x001633B1 File Offset: 0x001617B1
	Public Property IsPhase2 As Boolean
		Get
			Return Me.isPhase2
		End Get
		Set(value As Boolean)
			Me.isPhase2 = value
			Me.catAnimator.SetBool("IsPhase2", value)
		End Set
	End Property

	' Token: 0x060025EB RID: 9707 RVA: 0x001633CB File Offset: 0x001617CB
	Public Sub StartPeeking()
		Me.peekRoutine = Me.catPeeking_cr()
		MyBase.StartCoroutine(Me.peekRoutine)
	End Sub

	' Token: 0x060025EC RID: 9708 RVA: 0x001633E6 File Offset: 0x001617E6
	Public Sub StopPeeking()
		MyBase.StopCoroutine(Me.peekRoutine)
	End Sub

	' Token: 0x060025ED RID: 9709 RVA: 0x001633F4 File Offset: 0x001617F4
	Private Iterator Function catPeeking_cr() As IEnumerator
		Dim catTransform As Transform = MyBase.transform
		While True
			Dim isRight As Boolean = Rand.Bool()
			Me.catAnimator.SetBool("IsRight", isRight)
			catTransform.eulerAngles = Vector3.forward * Me.catRotationRange.RandomFloat()
			Me.catAnimator.SetTrigger("Peek")
			Yield Nothing
			Yield Me.catAnimator.WaitForAnimationToEnd(Me, True)
			Yield CupheadTime.WaitForSeconds(Me, Me.catDelay.RandomFloat())
		End While
		Return
	End Function

	' Token: 0x04002E6F RID: 11887
	Private Const CatPeekParameterName As String = "Peek"

	' Token: 0x04002E70 RID: 11888
	Private Const IsRightParameterName As String = "IsRight"

	' Token: 0x04002E71 RID: 11889
	<SerializeField()>
	Private catAnimator As Animator

	' Token: 0x04002E72 RID: 11890
	<SerializeField()>
	Private catDelay As MinMax

	' Token: 0x04002E73 RID: 11891
	<SerializeField()>
	Private catRotationRange As MinMax

	' Token: 0x04002E74 RID: 11892
	<Range(0F, 1F)>
	<SerializeField()>
	Private peek1Threshold As Single

	' Token: 0x04002E75 RID: 11893
	<Range(0F, 1F)>
	<SerializeField()>
	Private peek2Threshold As Single

	' Token: 0x04002E76 RID: 11894
	Private isPhase2 As Boolean

	' Token: 0x04002E77 RID: 11895
	Private peekRoutine As IEnumerator
End Class
