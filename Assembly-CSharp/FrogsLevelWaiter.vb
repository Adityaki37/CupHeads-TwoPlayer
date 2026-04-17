Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020006A6 RID: 1702
Public Class FrogsLevelWaiter
	Inherits AbstractMonoBehaviour

	' Token: 0x06002410 RID: 9232 RVA: 0x00152B5E File Offset: 0x00150F5E
	Private Sub Start()
		MyBase.StartCoroutine(Me.waiter_cr())
	End Sub

	' Token: 0x06002411 RID: 9233 RVA: 0x00152B70 File Offset: 0x00150F70
	Private Iterator Function waiter_cr() As IEnumerator
		Dim x As Single = MyBase.transform.localPosition.x
		While True
			MyBase.transform.SetScale(New Single?(1F), Nothing, Nothing)
			Yield MyBase.StartCoroutine(Me.move_cr(x, -x))
			Yield CupheadTime.WaitForSeconds(Me, 2F)
			MyBase.transform.SetScale(New Single?(-1F), Nothing, Nothing)
			Yield MyBase.StartCoroutine(Me.move_cr(-x, x))
			Yield CupheadTime.WaitForSeconds(Me, 2F)
		End While
		Return
	End Function

	' Token: 0x06002412 RID: 9234 RVA: 0x00152B8C File Offset: 0x00150F8C
	Private Iterator Function move_cr(start As Single, [end] As Single) As IEnumerator
		Dim t As Single = 0F
		MyBase.transform.SetLocalPosition(New Single?(start), Nothing, Nothing)
		While t < 8F
			Dim val As Single = t / 8F
			MyBase.transform.SetLocalPosition(New Single?(Mathf.Lerp(start, [end], val)), Nothing, Nothing)
			t += CupheadTime.Delta
			Yield Nothing
		End While
		MyBase.transform.SetLocalPosition(New Single?([end]), Nothing, Nothing)
		Return
	End Function

	' Token: 0x04002CE2 RID: 11490
	Private Const TIME As Single = 8F
End Class
