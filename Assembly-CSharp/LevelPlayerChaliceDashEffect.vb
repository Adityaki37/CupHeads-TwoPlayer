Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000A17 RID: 2583
Public Class LevelPlayerChaliceDashEffect
	Inherits Effect

	' Token: 0x06003D3F RID: 15679 RVA: 0x0021E1EC File Offset: 0x0021C5EC
	Private Sub Start()
		MyBase.StartCoroutine(Me.MoveUntilTail())
	End Sub

	' Token: 0x06003D40 RID: 15680 RVA: 0x0021E1FC File Offset: 0x0021C5FC
	Private Iterator Function MoveUntilTail() As IEnumerator
		Yield New WaitForEndOfFrame()
		Dim xFacing As Single = MyBase.transform.parent.transform.localScale.x
		Dim target As Integer = Animator.StringToHash(MyBase.animator.GetLayerName(0) + ".Start")
		While MyBase.animator.GetCurrentAnimatorStateInfo(0).fullPathHash = target AndAlso MyBase.transform.parent.transform.localScale.x = xFacing
			Yield Nothing
		End While
		If MyBase.transform.parent.transform.localScale.x <> xFacing Then
			MyBase.transform.localPosition = New Vector3(-MyBase.transform.localPosition.x, MyBase.transform.localPosition.y)
		End If
		MyBase.transform.parent = Nothing
		MyBase.transform.localScale = New Vector3(xFacing, 1F)
		Return
	End Function

	' Token: 0x04004491 RID: 17553
	Private t As Single
End Class
