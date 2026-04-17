Imports System
Imports UnityEngine

' Token: 0x020006E5 RID: 1765
Public Class MouseLevelCartPlatformPusher
	Inherits AbstractCollidableObject

	' Token: 0x060025C0 RID: 9664 RVA: 0x001616C4 File Offset: 0x0015FAC4
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		Dim component As AbstractPlayerController = hit.GetComponent(Of AbstractPlayerController)()
		Dim component2 As Collider2D = MyBase.GetComponent(Of Collider2D)()
		Dim component3 As Collider2D = component.GetComponent(Of Collider2D)()
		If component.bottom < component2.bounds.max.y Then
			If component.center.x < component2.bounds.center.x Then
				Dim num As Single = component3.bounds.max.x - component2.bounds.min.x
				If num > 0F Then
					component.transform.AddPosition(-num, 0F, 0F)
				End If
			Else
				Dim num2 As Single = component2.bounds.max.x - component3.bounds.min.x
				If num2 > 0F Then
					component.transform.AddPosition(num2, 0F, 0F)
				End If
			End If
		End If
	End Sub
End Class
