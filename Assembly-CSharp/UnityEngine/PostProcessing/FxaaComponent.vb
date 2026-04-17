Imports System

Namespace UnityEngine.PostProcessing
	' Token: 0x02000BA9 RID: 2985
	Public NotInheritable Class FxaaComponent
		Inherits PostProcessingComponentRenderTexture(Of AntialiasingModel)

		' Token: 0x17000651 RID: 1617
		' (get) Token: 0x0600487A RID: 18554 RVA: 0x00260E9C File Offset: 0x0025F29C
		Public Overrides ReadOnly Property active As Boolean
			Get
				Return MyBase.model.enabled AndAlso MyBase.model.settings.method = AntialiasingModel.Method.Fxaa AndAlso Not Me.context.interrupted
			End Get
		End Property

		' Token: 0x0600487B RID: 18555 RVA: 0x00260EE4 File Offset: 0x0025F2E4
		Public Sub Render(source As RenderTexture, destination As RenderTexture)
			Dim fxaaSettings As AntialiasingModel.FxaaSettings = MyBase.model.settings.fxaaSettings
			Dim material As Material = Me.context.materialFactory.[Get]("Hidden/Post FX/FXAA")
			Dim fxaaQualitySettings As AntialiasingModel.FxaaQualitySettings = AntialiasingModel.FxaaQualitySettings.presets(CInt(fxaaSettings.preset))
			Dim fxaaConsoleSettings As AntialiasingModel.FxaaConsoleSettings = AntialiasingModel.FxaaConsoleSettings.presets(CInt(fxaaSettings.preset))
			material.SetVector(FxaaComponent.Uniforms._QualitySettings, New Vector3(fxaaQualitySettings.subpixelAliasingRemovalAmount, fxaaQualitySettings.edgeDetectionThreshold, fxaaQualitySettings.minimumRequiredLuminance))
			material.SetVector(FxaaComponent.Uniforms._ConsoleSettings, New Vector4(fxaaConsoleSettings.subpixelSpreadAmount, fxaaConsoleSettings.edgeSharpnessAmount, fxaaConsoleSettings.edgeDetectionThreshold, fxaaConsoleSettings.minimumRequiredLuminance))
			Graphics.Blit(source, destination, material, 0)
		End Sub

		' Token: 0x02000BAA RID: 2986
		Private NotInheritable Class Uniforms
			' Token: 0x04004DF5 RID: 19957
			Friend Shared _QualitySettings As Integer = Shader.PropertyToID("_QualitySettings")

			' Token: 0x04004DF6 RID: 19958
			Friend Shared _ConsoleSettings As Integer = Shader.PropertyToID("_ConsoleSettings")
		End Class
	End Class
End Namespace
