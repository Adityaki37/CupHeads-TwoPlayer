Imports System
Imports UnityEngine.Rendering

Namespace UnityEngine.PostProcessing
	' Token: 0x02000BFA RID: 3066
	Public MustInherit Class PostProcessingComponentCommandBuffer(Of T As PostProcessingModel)
		Inherits PostProcessingComponent(Of T)

		' Token: 0x0600493C RID: 18748
		Public MustOverride Function GetCameraEvent() As CameraEvent

		' Token: 0x0600493D RID: 18749
		Public MustOverride Function GetName() As String

		' Token: 0x0600493E RID: 18750
		Public MustOverride Sub PopulateCommandBuffer(cb As CommandBuffer)
	End Class
End Namespace
