Imports System
Imports UnityEngine

' Token: 0x02000685 RID: 1669
Public Class FlyingMermaidLevelEelBullet
	Inherits BasicProjectile

	' Token: 0x06002335 RID: 9013 RVA: 0x0014AD9C File Offset: 0x0014919C
	Private Sub RotateSpark()
		Me.spark.transform.SetEulerAngles(Nothing, Nothing, New Single?(CSng(Global.UnityEngine.Random.Range(0, 360))))
	End Sub

	' Token: 0x04002BE1 RID: 11233
	<SerializeField()>
	Private spark As Transform
End Class
