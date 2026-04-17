Imports System
Imports UnityEngine

' Token: 0x020006C5 RID: 1733
Public Class GraveyardLevelPlatform
	Inherits AbstractMonoBehaviour

	' Token: 0x060024D5 RID: 9429 RVA: 0x00159371 File Offset: 0x00157771
	Private Sub Start()
		Me.center = MyBase.transform.position
		Me.t = -0.8F
	End Sub

	' Token: 0x060024D6 RID: 9430 RVA: 0x00159390 File Offset: 0x00157790
	Private Sub Update()
		Me.t += CupheadTime.Delta * Me.speed
		MyBase.transform.position = Me.center + MathUtils.AngleToDirection(-90F + Mathf.Sin(Me.t) * Me.maxAngle) * Me.radius
	End Sub

	' Token: 0x04002D73 RID: 11635
	Private t As Single

	' Token: 0x04002D74 RID: 11636
	Private center As Vector3

	' Token: 0x04002D75 RID: 11637
	<SerializeField()>
	Private radius As Single = 700F

	' Token: 0x04002D76 RID: 11638
	<SerializeField()>
	Private speed As Single = 315F

	' Token: 0x04002D77 RID: 11639
	<SerializeField()>
	Private maxAngle As Single = 30F
End Class
