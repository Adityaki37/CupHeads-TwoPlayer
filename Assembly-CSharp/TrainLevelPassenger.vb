Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000822 RID: 2082
Public Class TrainLevelPassenger
	Inherits AbstractPausableComponent

	' Token: 0x0600305D RID: 12381 RVA: 0x001C80C3 File Offset: 0x001C64C3
	Protected Overrides Sub Awake()
		MyBase.Awake()
		MyBase.StartCoroutine(Me.main_cr())
	End Sub

	' Token: 0x0600305E RID: 12382 RVA: 0x001C80D8 File Offset: 0x001C64D8
	Private Iterator Function main_cr() As IEnumerator
		While True
			Yield CupheadTime.WaitForSeconds(Me, Global.UnityEngine.Random.Range(3F, 8F))
			MyBase.animator.SetTrigger("Continue")
		End While
		Return
	End Function
End Class
