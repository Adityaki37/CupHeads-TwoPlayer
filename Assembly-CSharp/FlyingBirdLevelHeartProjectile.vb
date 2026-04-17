Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000620 RID: 1568
Public Class FlyingBirdLevelHeartProjectile
	Inherits BasicProjectile

	' Token: 0x06001FE9 RID: 8169 RVA: 0x001253BB File Offset: 0x001237BB
	Protected Overrides Sub Start()
		MyBase.Start()
		MyBase.StartCoroutine(Me.spawn_fx_cr())
	End Sub

	' Token: 0x06001FEA RID: 8170 RVA: 0x001253D0 File Offset: 0x001237D0
	Private Iterator Function spawn_fx_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 0.17F)
		While True
			Me.FX.Create(MyBase.transform.position).transform.SetEulerAngles(Nothing, Nothing, New Single?(MyBase.transform.eulerAngles.z))
			Yield CupheadTime.WaitForSeconds(Me, 0.2F)
		End While
		Return
	End Function

	' Token: 0x04002868 RID: 10344
	<SerializeField()>
	Private FX As Effect
End Class
