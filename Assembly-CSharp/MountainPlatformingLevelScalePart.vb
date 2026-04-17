Imports System
Imports UnityEngine

' Token: 0x020008F1 RID: 2289
Public Class MountainPlatformingLevelScalePart
	Inherits AbstractCollidableObject

	' Token: 0x060035AA RID: 13738 RVA: 0x001F4998 File Offset: 0x001F2D98
	Protected Overrides Sub OnCollision(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollision(hit, phase)
		If hit.GetComponent(Of AbstractPlayerController)() IsNot Nothing Then
			If phase = CollisionPhase.[Exit] Then
				Me.steppedOn = False
			Else
				Me.steppedOn = True
			End If
		End If
	End Sub

	' Token: 0x04003DC3 RID: 15811
	Public steppedOn As Boolean
End Class
