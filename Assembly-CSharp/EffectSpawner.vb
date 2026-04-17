Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000B11 RID: 2833
Public Class EffectSpawner
	Inherits AbstractMonoBehaviour

	' Token: 0x060044BB RID: 17595 RVA: 0x00246424 File Offset: 0x00244824
	Private Sub Start()
		MyBase.StartCoroutine(Me.loop_cr())
	End Sub

	' Token: 0x060044BC RID: 17596 RVA: 0x00246434 File Offset: 0x00244834
	Protected Overrides Sub OnDrawGizmos()
		MyBase.OnDrawGizmos()
		Gizmos.color = Color.black
		Gizmos.DrawWireSphere(MyBase.baseTransform.position, 5F)
		Gizmos.color = Color.red
		Dim vector As Vector3 = MyBase.baseTransform.position + Me.offset
		Gizmos.DrawLine(MyBase.transform.position, vector)
		Gizmos.DrawWireSphere(vector, 5F)
	End Sub

	' Token: 0x060044BD RID: 17597 RVA: 0x002464A8 File Offset: 0x002448A8
	Private Iterator Function loop_cr() As IEnumerator
		While True
			Yield CupheadTime.WaitForSeconds(Me, Me.delay)
			Dim t As Transform = Me.effectPrefab.Create(MyBase.transform.position).transform
			t.SetParent(MyBase.transform)
			t.ResetLocalTransforms()
			t.localPosition = Me.offset
			t.SetParent(Nothing)
		End While
		Return
	End Function

	' Token: 0x04004A73 RID: 19059
	<SerializeField()>
	Private effectPrefab As Effect

	' Token: 0x04004A74 RID: 19060
	Public offset As Vector2

	' Token: 0x04004A75 RID: 19061
	Public delay As Single = 1F
End Class
