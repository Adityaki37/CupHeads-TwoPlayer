Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x0200056B RID: 1387
Public Class ClownLevelPenguinBullet
	Inherits BasicProjectile

	' Token: 0x06001A37 RID: 6711 RVA: 0x000EFF46 File Offset: 0x000EE346
	Protected Overrides Sub Start()
		MyBase.Start()
		Me.move = False
		MyBase.StartCoroutine(Me.timer_cr())
	End Sub

	' Token: 0x06001A38 RID: 6712 RVA: 0x000EFF64 File Offset: 0x000EE364
	Private Iterator Function timer_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 0.3F)
		Me.move = True
		Me.bulletFX.Create(Me.root.transform.position).transform.SetEulerAngles(Nothing, Nothing, New Single?(MyBase.transform.eulerAngles.z - 90F))
		Yield Nothing
		Return
	End Function

	' Token: 0x04002354 RID: 9044
	<SerializeField()>
	Private bulletFX As Effect

	' Token: 0x04002355 RID: 9045
	<SerializeField()>
	Private root As Transform
End Class
