Imports System
Imports UnityEngine

' Token: 0x02000A10 RID: 2576
Public Class HouseElderKettlePlayerRun
	Inherits MonoBehaviour

	' Token: 0x06003CE1 RID: 15585 RVA: 0x0021AD75 File Offset: 0x00219175
	Private Sub Start()
	End Sub

	' Token: 0x06003CE2 RID: 15586 RVA: 0x0021AD77 File Offset: 0x00219177
	Private Sub Update()
		MyBase.transform.localPosition += New Vector3(-490F, 0F, 0F) * CupheadTime.FixedDelta
	End Sub

	' Token: 0x06003CE3 RID: 15587 RVA: 0x0021ADAD File Offset: 0x002191AD
	Private Sub OnRunDust()
		Me.runEffect.Create(Me.runDustRoot.position)
	End Sub

	' Token: 0x0400442E RID: 17454
	<SerializeField()>
	Private runEffect As Effect

	' Token: 0x0400442F RID: 17455
	<SerializeField()>
	Private runDustRoot As Transform
End Class
