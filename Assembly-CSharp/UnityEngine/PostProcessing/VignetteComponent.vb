Imports System

Namespace UnityEngine.PostProcessing
	' Token: 0x02000BBA RID: 3002
	Public NotInheritable Class VignetteComponent
		Inherits PostProcessingComponentRenderTexture(Of VignetteModel)

		' Token: 0x1700065A RID: 1626
		' (get) Token: 0x060048B9 RID: 18617 RVA: 0x00263264 File Offset: 0x00261664
		Public Overrides ReadOnly Property active As Boolean
			Get
				Return MyBase.model.enabled AndAlso Not Me.context.interrupted
			End Get
		End Property

		' Token: 0x060048BA RID: 18618 RVA: 0x00263288 File Offset: 0x00261688
		Public Overrides Sub Prepare(uberMaterial As Material)
			Dim settings As VignetteModel.Settings = MyBase.model.settings
			uberMaterial.SetColor(VignetteComponent.Uniforms._Vignette_Color, settings.color)
			If settings.mode = VignetteModel.Mode.Classic Then
				uberMaterial.SetVector(VignetteComponent.Uniforms._Vignette_Center, settings.center)
				uberMaterial.EnableKeyword("VIGNETTE_CLASSIC")
				Dim num As Single = (1F - settings.roundness) * 6F + settings.roundness
				uberMaterial.SetVector(VignetteComponent.Uniforms._Vignette_Settings, New Vector4(settings.intensity * 3F, settings.smoothness * 5F, num, If((Not settings.rounded), 0F, 1F)))
			ElseIf settings.mode = VignetteModel.Mode.Masked AndAlso settings.mask IsNot Nothing AndAlso settings.opacity > 0F Then
				uberMaterial.EnableKeyword("VIGNETTE_MASKED")
				uberMaterial.SetTexture(VignetteComponent.Uniforms._Vignette_Mask, settings.mask)
				uberMaterial.SetFloat(VignetteComponent.Uniforms._Vignette_Opacity, settings.opacity)
			End If
		End Sub

		' Token: 0x02000BBB RID: 3003
		Private NotInheritable Class Uniforms
			' Token: 0x04004E6D RID: 20077
			Friend Shared _Vignette_Color As Integer = Shader.PropertyToID("_Vignette_Color")

			' Token: 0x04004E6E RID: 20078
			Friend Shared _Vignette_Center As Integer = Shader.PropertyToID("_Vignette_Center")

			' Token: 0x04004E6F RID: 20079
			Friend Shared _Vignette_Settings As Integer = Shader.PropertyToID("_Vignette_Settings")

			' Token: 0x04004E70 RID: 20080
			Friend Shared _Vignette_Mask As Integer = Shader.PropertyToID("_Vignette_Mask")

			' Token: 0x04004E71 RID: 20081
			Friend Shared _Vignette_Opacity As Integer = Shader.PropertyToID("_Vignette_Opacity")
		End Class
	End Class
End Namespace
