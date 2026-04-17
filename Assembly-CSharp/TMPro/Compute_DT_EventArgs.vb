Imports System
Imports UnityEngine

Namespace TMPro
	' Token: 0x02000C97 RID: 3223
	Public Class Compute_DT_EventArgs
		' Token: 0x0600516C RID: 20844 RVA: 0x002990CF File Offset: 0x002974CF
		Public Sub New(type As Compute_DistanceTransform_EventTypes, progress As Single)
			Me.EventType = type
			Me.ProgressPercentage = progress
		End Sub

		' Token: 0x0600516D RID: 20845 RVA: 0x002990E5 File Offset: 0x002974E5
		Public Sub New(type As Compute_DistanceTransform_EventTypes, colors As Color())
			Me.EventType = type
			Me.Colors = colors
		End Sub

		' Token: 0x0400540A RID: 21514
		Public EventType As Compute_DistanceTransform_EventTypes

		' Token: 0x0400540B RID: 21515
		Public ProgressPercentage As Single

		' Token: 0x0400540C RID: 21516
		Public Colors As Color()
	End Class
End Namespace
