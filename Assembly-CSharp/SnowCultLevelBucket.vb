Imports System
Imports UnityEngine

' Token: 0x020007E9 RID: 2025
Public Class SnowCultLevelBucket
	Inherits MonoBehaviour

	' Token: 0x06002E59 RID: 11865 RVA: 0x001B509C File Offset: 0x001B349C
	Private Sub FixedUpdate()
		MyBase.transform.position += Me.fallSpeed * Vector3.down * CupheadTime.FixedDelta
		Me.fallSpeed += CupheadTime.FixedDelta * Me.accel
	End Sub

	' Token: 0x040036ED RID: 14061
	<SerializeField()>
	Private fallSpeed As Single = 10F

	' Token: 0x040036EE RID: 14062
	<SerializeField()>
	Private accel As Single = 1F
End Class
