Imports System
Imports UnityEngine

' Token: 0x020008B5 RID: 2229
Public Class FunhousePlatformingLevelExplosionFX
	Inherits Effect

	' Token: 0x060033F8 RID: 13304 RVA: 0x001E2B7E File Offset: 0x001E0F7E
	Private Sub SpawnSmoke()
		Me.smoke.Create(MyBase.transform.position)
	End Sub

	' Token: 0x060033F9 RID: 13305 RVA: 0x001E2B97 File Offset: 0x001E0F97
	Private Sub FirecrackerLines()
		Me.firecracker.Create(MyBase.transform.position)
	End Sub

	' Token: 0x060033FA RID: 13306 RVA: 0x001E2BB0 File Offset: 0x001E0FB0
	Private Sub MiniExplosion()
		MyBase.GetComponent(Of EffectRadius)().CreateInRadius()
	End Sub

	' Token: 0x04003C45 RID: 15429
	<SerializeField()>
	Private smoke As Effect

	' Token: 0x04003C46 RID: 15430
	<SerializeField()>
	Private firecracker As Effect
End Class
