Imports System
Imports UnityEngine

' Token: 0x020004C7 RID: 1223
Public Class AirplaneLevelTerrierSmokeFX
	Inherits Effect

	' Token: 0x060014B4 RID: 5300 RVA: 0x000BA0B6 File Offset: 0x000B84B6
	Public Sub [Step](t As Single)
		Me.myTransform.position += Me.vel * t
	End Sub

	' Token: 0x060014B5 RID: 5301 RVA: 0x000BA0DA File Offset: 0x000B84DA
	Protected Overrides Sub OnEffectComplete()
		If Me.dead Then
			Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
		Else
			Me.inUse = False
		End If
	End Sub

	' Token: 0x04001E22 RID: 7714
	Public rend As SpriteRenderer

	' Token: 0x04001E23 RID: 7715
	Public vel As Vector3

	' Token: 0x04001E24 RID: 7716
	Public dead As Boolean

	' Token: 0x04001E25 RID: 7717
	Public inUse As Boolean = True

	' Token: 0x04001E26 RID: 7718
	Public myTransform As Transform
End Class
