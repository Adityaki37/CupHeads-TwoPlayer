Imports System
Imports UnityEngine

' Token: 0x02000B14 RID: 2836
Public Class PlaneLevelEffect
	Inherits Effect

	' Token: 0x060044C5 RID: 17605 RVA: 0x002468E3 File Offset: 0x00244CE3
	Private Sub Update()
		MyBase.transform.AddPosition(-300F * CupheadTime.Delta * Me.speed, 0F, 0F)
	End Sub

	' Token: 0x04004A80 RID: 19072
	Public Const SPEED As Single = 300F

	' Token: 0x04004A81 RID: 19073
	<Range(0F, 2F)>
	Public speed As Single = 1F
End Class
