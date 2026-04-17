Imports System

Namespace UnityEngine.PostProcessing
	' Token: 0x02000BF8 RID: 3064
	Public MustInherit Class PostProcessingComponentBase
		' Token: 0x06004931 RID: 18737 RVA: 0x0025DBB1 File Offset: 0x0025BFB1
		Public Overridable Function GetCameraFlags() As DepthTextureMode
			Return DepthTextureMode.None
		End Function

		' Token: 0x1700068A RID: 1674
		' (get) Token: 0x06004932 RID: 18738
		Public MustOverride ReadOnly Property active As Boolean

		' Token: 0x06004933 RID: 18739 RVA: 0x0025DBB4 File Offset: 0x0025BFB4
		Public Overridable Sub OnEnable()
		End Sub

		' Token: 0x06004934 RID: 18740 RVA: 0x0025DBB6 File Offset: 0x0025BFB6
		Public Overridable Sub OnDisable()
		End Sub

		' Token: 0x06004935 RID: 18741
		Public MustOverride Function GetModel() As PostProcessingModel

		' Token: 0x04004F5D RID: 20317
		Public context As PostProcessingContext
	End Class
End Namespace
