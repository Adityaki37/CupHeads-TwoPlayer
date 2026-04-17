Imports System
Imports UnityEngine

' Token: 0x0200078F RID: 1935
Public Class RumRunnersLevelGrubPath
	Inherits MonoBehaviour

	' Token: 0x06002ADC RID: 10972 RVA: 0x0018FFE9 File Offset: 0x0018E3E9
	Public Function GetPoint(t As Single) As Vector2
		Return Vector2.LerpUnclamped(Vector2.LerpUnclamped(Me.start, Me.controlPoint, t), Vector2.LerpUnclamped(Me.controlPoint, MyBase.transform.position, t), t)
	End Function

	' Token: 0x0400339A RID: 13210
	Public start As Vector2

	' Token: 0x0400339B RID: 13211
	Public controlPoint As Vector2

	' Token: 0x0400339C RID: 13212
	Public forceFGSet As Single = 2F
End Class
