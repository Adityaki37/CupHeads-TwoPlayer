Imports System

Namespace UnityEngine.PostProcessing
	' Token: 0x02000BFE RID: 3070
	Public Class PostProcessingProfile
		Inherits ScriptableObject

		' Token: 0x04004F65 RID: 20325
		Public debugViews As BuiltinDebugViewsModel = New BuiltinDebugViewsModel()

		' Token: 0x04004F66 RID: 20326
		Public fog As FogModel = New FogModel()

		' Token: 0x04004F67 RID: 20327
		Public antialiasing As AntialiasingModel = New AntialiasingModel()

		' Token: 0x04004F68 RID: 20328
		Public ambientOcclusion As AmbientOcclusionModel = New AmbientOcclusionModel()

		' Token: 0x04004F69 RID: 20329
		Public screenSpaceReflection As ScreenSpaceReflectionModel = New ScreenSpaceReflectionModel()

		' Token: 0x04004F6A RID: 20330
		Public depthOfField As DepthOfFieldModel = New DepthOfFieldModel()

		' Token: 0x04004F6B RID: 20331
		Public motionBlur As MotionBlurModel = New MotionBlurModel()

		' Token: 0x04004F6C RID: 20332
		Public eyeAdaptation As EyeAdaptationModel = New EyeAdaptationModel()

		' Token: 0x04004F6D RID: 20333
		Public bloom As BloomModel = New BloomModel()

		' Token: 0x04004F6E RID: 20334
		Public colorGrading As ColorGradingModel = New ColorGradingModel()

		' Token: 0x04004F6F RID: 20335
		Public userLut As UserLutModel = New UserLutModel()

		' Token: 0x04004F70 RID: 20336
		Public chromaticAberration As ChromaticAberrationModel = New ChromaticAberrationModel()

		' Token: 0x04004F71 RID: 20337
		Public grain As GrainModel = New GrainModel()

		' Token: 0x04004F72 RID: 20338
		Public vignette As VignetteModel = New VignetteModel()

		' Token: 0x04004F73 RID: 20339
		Public dithering As DitheringModel = New DitheringModel()
	End Class
End Namespace
