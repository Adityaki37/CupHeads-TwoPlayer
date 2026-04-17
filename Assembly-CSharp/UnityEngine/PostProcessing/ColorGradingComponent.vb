Imports System

Namespace UnityEngine.PostProcessing
	' Token: 0x02000B9F RID: 2975
	Public NotInheritable Class ColorGradingComponent
		Inherits PostProcessingComponentRenderTexture(Of ColorGradingModel)

		' Token: 0x1700064C RID: 1612
		' (get) Token: 0x06004842 RID: 18498 RVA: 0x0025EFF3 File Offset: 0x0025D3F3
		Public Overrides ReadOnly Property active As Boolean
			Get
				Return MyBase.model.enabled AndAlso Not Me.context.interrupted
			End Get
		End Property

		' Token: 0x06004843 RID: 18499 RVA: 0x0025F016 File Offset: 0x0025D416
		Private Function StandardIlluminantY(x As Single) As Single
			Return 2.87F * x - 3F * x * x - 0.27509508F
		End Function

		' Token: 0x06004844 RID: 18500 RVA: 0x0025F030 File Offset: 0x0025D430
		Private Function CIExyToLMS(x As Single, y As Single) As Vector3
			Dim num As Single = 1F
			Dim num2 As Single = num * x / y
			Dim num3 As Single = num * (1F - x - y) / y
			Dim num4 As Single = 0.7328F * num2 + 0.4296F * num - 0.1624F * num3
			Dim num5 As Single = -0.7036F * num2 + 1.6975F * num + 0.0061F * num3
			Dim num6 As Single = 0.003F * num2 + 0.0136F * num + 0.9834F * num3
			Return New Vector3(num4, num5, num6)
		End Function

		' Token: 0x06004845 RID: 18501 RVA: 0x0025F0AC File Offset: 0x0025D4AC
		Private Function CalculateColorBalance(temperature As Single, tint As Single) As Vector3
			Dim num As Single = temperature / 55F
			Dim num2 As Single = tint / 55F
			Dim num3 As Single = 0.31271F - num * If((num >= 0F), 0.05F, 0.1F)
			Dim num4 As Single = Me.StandardIlluminantY(num3) + num2 * 0.05F
			Dim vector As Vector3 = New Vector3(0.949237F, 1.03542F, 1.08728F)
			Dim vector2 As Vector3 = Me.CIExyToLMS(num3, num4)
			Return New Vector3(vector.x / vector2.x, vector.y / vector2.y, vector.z / vector2.z)
		End Function

		' Token: 0x06004846 RID: 18502 RVA: 0x0025F150 File Offset: 0x0025D550
		Private Shared Function NormalizeColor(c As Color) As Color
			Dim num As Single = (c.r + c.g + c.b) / 3F
			If Mathf.Approximately(num, 0F) Then
				Return New Color(1F, 1F, 1F, c.a)
			End If
			Return New Color() With { .r = c.r / num, .g = c.g / num, .b = c.b / num, .a = c.a }
		End Function

		' Token: 0x06004847 RID: 18503 RVA: 0x0025F1EE File Offset: 0x0025D5EE
		Private Shared Function ClampVector(v As Vector3, min As Single, max As Single) As Vector3
			Return New Vector3(Mathf.Clamp(v.x, min, max), Mathf.Clamp(v.y, min, max), Mathf.Clamp(v.z, min, max))
		End Function

		' Token: 0x06004848 RID: 18504 RVA: 0x0025F220 File Offset: 0x0025D620
		Public Shared Function GetLiftValue(lift As Color) As Vector3
			Dim color As Color = ColorGradingComponent.NormalizeColor(lift)
			Dim num As Single = (color.r + color.g + color.b) / 3F
			Dim num2 As Single = (color.r - num) * 0.1F + lift.a
			Dim num3 As Single = (color.g - num) * 0.1F + lift.a
			Dim num4 As Single = (color.b - num) * 0.1F + lift.a
			Return ColorGradingComponent.ClampVector(New Vector3(num2, num3, num4), -1F, 1F)
		End Function

		' Token: 0x06004849 RID: 18505 RVA: 0x0025F2B4 File Offset: 0x0025D6B4
		Public Shared Function GetGammaValue(gamma As Color) As Vector3
			Dim color As Color = ColorGradingComponent.NormalizeColor(gamma)
			Dim num As Single = (color.r + color.g + color.b) / 3F
			gamma.a *= If((gamma.a >= 0F), 5F, 0.8F)
			Dim num2 As Single = Mathf.Pow(2F, (color.r - num) * 0.5F) + gamma.a
			Dim num3 As Single = Mathf.Pow(2F, (color.g - num) * 0.5F) + gamma.a
			Dim num4 As Single = Mathf.Pow(2F, (color.b - num) * 0.5F) + gamma.a
			Dim num5 As Single = 1F / Mathf.Max(0.01F, num2)
			Dim num6 As Single = 1F / Mathf.Max(0.01F, num3)
			Dim num7 As Single = 1F / Mathf.Max(0.01F, num4)
			Return ColorGradingComponent.ClampVector(New Vector3(num5, num6, num7), 0F, 5F)
		End Function

		' Token: 0x0600484A RID: 18506 RVA: 0x0025F3D0 File Offset: 0x0025D7D0
		Public Shared Function GetGainValue(gain As Color) As Vector3
			Dim color As Color = ColorGradingComponent.NormalizeColor(gain)
			Dim num As Single = (color.r + color.g + color.b) / 3F
			gain.a *= If((gain.a <= 0F), 1F, 3F)
			Dim num2 As Single = Mathf.Pow(2F, (color.r - num) * 0.5F) + gain.a
			Dim num3 As Single = Mathf.Pow(2F, (color.g - num) * 0.5F) + gain.a
			Dim num4 As Single = Mathf.Pow(2F, (color.b - num) * 0.5F) + gain.a
			Return ColorGradingComponent.ClampVector(New Vector3(num2, num3, num4), 0F, 4F)
		End Function

		' Token: 0x0600484B RID: 18507 RVA: 0x0025F4AF File Offset: 0x0025D8AF
		Public Shared Sub CalculateLiftGammaGain(lift As Color, gamma As Color, gain As Color, <System.Runtime.InteropServices.OutAttribute()> ByRef outLift As Vector3, <System.Runtime.InteropServices.OutAttribute()> ByRef outGamma As Vector3, <System.Runtime.InteropServices.OutAttribute()> ByRef outGain As Vector3)
			outLift = ColorGradingComponent.GetLiftValue(lift)
			outGamma = ColorGradingComponent.GetGammaValue(gamma)
			outGain = ColorGradingComponent.GetGainValue(gain)
		End Sub

		' Token: 0x0600484C RID: 18508 RVA: 0x0025F4D8 File Offset: 0x0025D8D8
		Public Shared Function GetSlopeValue(slope As Color) As Vector3
			Dim color As Color = ColorGradingComponent.NormalizeColor(slope)
			Dim num As Single = (color.r + color.g + color.b) / 3F
			slope.a *= 0.5F
			Dim num2 As Single = (color.r - num) * 0.1F + slope.a + 1F
			Dim num3 As Single = (color.g - num) * 0.1F + slope.a + 1F
			Dim num4 As Single = (color.b - num) * 0.1F + slope.a + 1F
			Return ColorGradingComponent.ClampVector(New Vector3(num2, num3, num4), 0F, 2F)
		End Function

		' Token: 0x0600484D RID: 18509 RVA: 0x0025F590 File Offset: 0x0025D990
		Public Shared Function GetPowerValue(power As Color) As Vector3
			Dim color As Color = ColorGradingComponent.NormalizeColor(power)
			Dim num As Single = (color.r + color.g + color.b) / 3F
			power.a *= 0.5F
			Dim num2 As Single = (color.r - num) * 0.1F + power.a + 1F
			Dim num3 As Single = (color.g - num) * 0.1F + power.a + 1F
			Dim num4 As Single = (color.b - num) * 0.1F + power.a + 1F
			Dim num5 As Single = 1F / Mathf.Max(0.01F, num2)
			Dim num6 As Single = 1F / Mathf.Max(0.01F, num3)
			Dim num7 As Single = 1F / Mathf.Max(0.01F, num4)
			Return ColorGradingComponent.ClampVector(New Vector3(num5, num6, num7), 0.5F, 2.5F)
		End Function

		' Token: 0x0600484E RID: 18510 RVA: 0x0025F684 File Offset: 0x0025DA84
		Public Shared Function GetOffsetValue(offset As Color) As Vector3
			Dim color As Color = ColorGradingComponent.NormalizeColor(offset)
			Dim num As Single = (color.r + color.g + color.b) / 3F
			offset.a *= 0.5F
			Dim num2 As Single = (color.r - num) * 0.05F + offset.a
			Dim num3 As Single = (color.g - num) * 0.05F + offset.a
			Dim num4 As Single = (color.b - num) * 0.05F + offset.a
			Return ColorGradingComponent.ClampVector(New Vector3(num2, num3, num4), -0.8F, 0.8F)
		End Function

		' Token: 0x0600484F RID: 18511 RVA: 0x0025F72A File Offset: 0x0025DB2A
		Public Shared Sub CalculateSlopePowerOffset(slope As Color, power As Color, offset As Color, <System.Runtime.InteropServices.OutAttribute()> ByRef outSlope As Vector3, <System.Runtime.InteropServices.OutAttribute()> ByRef outPower As Vector3, <System.Runtime.InteropServices.OutAttribute()> ByRef outOffset As Vector3)
			outSlope = ColorGradingComponent.GetSlopeValue(slope)
			outPower = ColorGradingComponent.GetPowerValue(power)
			outOffset = ColorGradingComponent.GetOffsetValue(offset)
		End Sub

		' Token: 0x06004850 RID: 18512 RVA: 0x0025F752 File Offset: 0x0025DB52
		Private Function GetCurveFormat() As TextureFormat
			If SystemInfo.SupportsTextureFormat(TextureFormat.RGBAHalf) Then
				Return TextureFormat.RGBAHalf
			End If
			Return TextureFormat.RGBA32
		End Function

		' Token: 0x06004851 RID: 18513 RVA: 0x0025F764 File Offset: 0x0025DB64
		Private Function GetCurveTexture() As Texture2D
			If Me.m_GradingCurves Is Nothing Then
				Me.m_GradingCurves = New Texture2D(128, 2, Me.GetCurveFormat(), False, True) With { .name = "Internal Curves Texture", .hideFlags = HideFlags.DontSave, .anisoLevel = 0, .wrapMode = TextureWrapMode.Clamp, .filterMode = FilterMode.Bilinear }
			End If
			Dim curves As ColorGradingModel.CurvesSettings = MyBase.model.settings.curves
			curves.hueVShue.Cache()
			curves.hueVSsat.Cache()
			For i As Integer = 0 To 128 - 1
				Dim num As Single = CSng(i) * 0.0078125F
				Dim num2 As Single = curves.hueVShue.Evaluate(num)
				Dim num3 As Single = curves.hueVSsat.Evaluate(num)
				Dim num4 As Single = curves.satVSsat.Evaluate(num)
				Dim num5 As Single = curves.lumVSsat.Evaluate(num)
				Me.m_pixels(i) = New Color(num2, num3, num4, num5)
				Dim num6 As Single = curves.master.Evaluate(num)
				Dim num7 As Single = curves.red.Evaluate(num)
				Dim num8 As Single = curves.green.Evaluate(num)
				Dim num9 As Single = curves.blue.Evaluate(num)
				Me.m_pixels(i + 128) = New Color(num7, num8, num9, num6)
			Next
			Me.m_GradingCurves.SetPixels(Me.m_pixels)
			Me.m_GradingCurves.Apply(False, False)
			Return Me.m_GradingCurves
		End Function

		' Token: 0x06004852 RID: 18514 RVA: 0x0025F8F7 File Offset: 0x0025DCF7
		Private Function IsLogLutValid(lut As RenderTexture) As Boolean
			Return lut IsNot Nothing AndAlso lut.IsCreated() AndAlso lut.height = 32
		End Function

		' Token: 0x06004853 RID: 18515 RVA: 0x0025F91D File Offset: 0x0025DD1D
		Private Function GetLutFormat() As RenderTextureFormat
			If SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.ARGBHalf) Then
				Return RenderTextureFormat.ARGBHalf
			End If
			Return RenderTextureFormat.ARGB32
		End Function

		' Token: 0x06004854 RID: 18516 RVA: 0x0025F930 File Offset: 0x0025DD30
		Private Sub GenerateLut()
			Dim settings As ColorGradingModel.Settings = MyBase.model.settings
			If Not Me.IsLogLutValid(MyBase.model.bakedLut) Then
				GraphicsUtils.Destroy(MyBase.model.bakedLut)
				MyBase.model.bakedLut = New RenderTexture(1024, 32, 0, Me.GetLutFormat()) With { .name = "Color Grading Log LUT", .hideFlags = HideFlags.DontSave, .filterMode = FilterMode.Bilinear, .wrapMode = TextureWrapMode.Clamp, .anisoLevel = 0 }
			End If
			Dim material As Material = Me.context.materialFactory.[Get]("Hidden/Post FX/Lut Generator")
			material.SetVector(ColorGradingComponent.Uniforms._LutParams, New Vector4(32F, 0.00048828125F, 0.015625F, 1.032258F))
			material.shaderKeywords = Nothing
			Dim tonemapping As ColorGradingModel.TonemappingSettings = settings.tonemapping
			Dim tonemapper As ColorGradingModel.Tonemapper = tonemapping.tonemapper
			If tonemapper <> ColorGradingModel.Tonemapper.Neutral Then
				If tonemapper = ColorGradingModel.Tonemapper.ACES Then
					material.EnableKeyword("TONEMAPPING_FILMIC")
				End If
			Else
				material.EnableKeyword("TONEMAPPING_NEUTRAL")
				Dim num As Single = tonemapping.neutralBlackIn * 20F + 1F
				Dim num2 As Single = tonemapping.neutralBlackOut * 10F + 1F
				Dim num3 As Single = tonemapping.neutralWhiteIn / 20F
				Dim num4 As Single = 1F - tonemapping.neutralWhiteOut / 20F
				Dim num5 As Single = num / num2
				Dim num6 As Single = num3 / num4
				Dim num7 As Single = Mathf.Max(0F, Mathf.LerpUnclamped(0.57F, 0.37F, num5))
				Dim num8 As Single = Mathf.LerpUnclamped(0.01F, 0.24F, num6)
				Dim num9 As Single = Mathf.Max(0F, Mathf.LerpUnclamped(0.02F, 0.2F, num5))
				material.SetVector(ColorGradingComponent.Uniforms._NeutralTonemapperParams1, New Vector4(0.2F, num7, num8, num9))
				material.SetVector(ColorGradingComponent.Uniforms._NeutralTonemapperParams2, New Vector4(0.02F, 0.3F, tonemapping.neutralWhiteLevel, tonemapping.neutralWhiteClip / 10F))
			End If
			material.SetFloat(ColorGradingComponent.Uniforms._HueShift, settings.basic.hueShift / 360F)
			material.SetFloat(ColorGradingComponent.Uniforms._Saturation, settings.basic.saturation)
			material.SetFloat(ColorGradingComponent.Uniforms._Contrast, settings.basic.contrast)
			material.SetVector(ColorGradingComponent.Uniforms._Balance, Me.CalculateColorBalance(settings.basic.temperature, settings.basic.tint))
			Dim vector As Vector3
			Dim vector2 As Vector3
			Dim vector3 As Vector3
			ColorGradingComponent.CalculateLiftGammaGain(settings.colorWheels.linear.lift, settings.colorWheels.linear.gamma, settings.colorWheels.linear.gain, vector, vector2, vector3)
			material.SetVector(ColorGradingComponent.Uniforms._Lift, vector)
			material.SetVector(ColorGradingComponent.Uniforms._InvGamma, vector2)
			material.SetVector(ColorGradingComponent.Uniforms._Gain, vector3)
			Dim vector4 As Vector3
			Dim vector5 As Vector3
			Dim vector6 As Vector3
			ColorGradingComponent.CalculateSlopePowerOffset(settings.colorWheels.log.slope, settings.colorWheels.log.power, settings.colorWheels.log.offset, vector4, vector5, vector6)
			material.SetVector(ColorGradingComponent.Uniforms._Slope, vector4)
			material.SetVector(ColorGradingComponent.Uniforms._Power, vector5)
			material.SetVector(ColorGradingComponent.Uniforms._Offset, vector6)
			material.SetVector(ColorGradingComponent.Uniforms._ChannelMixerRed, settings.channelMixer.red)
			material.SetVector(ColorGradingComponent.Uniforms._ChannelMixerGreen, settings.channelMixer.green)
			material.SetVector(ColorGradingComponent.Uniforms._ChannelMixerBlue, settings.channelMixer.blue)
			material.SetTexture(ColorGradingComponent.Uniforms._Curves, Me.GetCurveTexture())
			Graphics.Blit(Nothing, MyBase.model.bakedLut, material, 0)
		End Sub

		' Token: 0x06004855 RID: 18517 RVA: 0x0025FD0C File Offset: 0x0025E10C
		Public Overrides Sub Prepare(uberMaterial As Material)
			If MyBase.model.isDirty OrElse Not Me.IsLogLutValid(MyBase.model.bakedLut) Then
				Me.GenerateLut()
				MyBase.model.isDirty = False
			End If
			uberMaterial.EnableKeyword(If((Not Me.context.profile.debugViews.IsModeActive(BuiltinDebugViewsModel.Mode.PreGradingLog)), "COLOR_GRADING", "COLOR_GRADING_LOG_VIEW"))
			Dim bakedLut As RenderTexture = MyBase.model.bakedLut
			uberMaterial.SetTexture(ColorGradingComponent.Uniforms._LogLut, bakedLut)
			uberMaterial.SetVector(ColorGradingComponent.Uniforms._LogLut_Params, New Vector3(1F / CSng(bakedLut.width), 1F / CSng(bakedLut.height), CSng(bakedLut.height) - 1F))
			Dim num As Single = Mathf.Exp(MyBase.model.settings.basic.postExposure * 0.6931472F)
			uberMaterial.SetFloat(ColorGradingComponent.Uniforms._ExposureEV, num)
		End Sub

		' Token: 0x06004856 RID: 18518 RVA: 0x0025FE08 File Offset: 0x0025E208
		Public Sub OnGUI()
			Dim bakedLut As RenderTexture = MyBase.model.bakedLut
			Dim rect As Rect = New Rect(Me.context.viewport.x * CSng(Screen.width) + 8F, 8F, CSng(bakedLut.width), CSng(bakedLut.height))
			GUI.DrawTexture(rect, bakedLut)
		End Sub

		' Token: 0x06004857 RID: 18519 RVA: 0x0025FE62 File Offset: 0x0025E262
		Public Overrides Sub OnDisable()
			GraphicsUtils.Destroy(Me.m_GradingCurves)
			GraphicsUtils.Destroy(MyBase.model.bakedLut)
			Me.m_GradingCurves = Nothing
			MyBase.model.bakedLut = Nothing
		End Sub

		' Token: 0x04004DB2 RID: 19890
		Private Const k_InternalLogLutSize As Integer = 32

		' Token: 0x04004DB3 RID: 19891
		Private Const k_CurvePrecision As Integer = 128

		' Token: 0x04004DB4 RID: 19892
		Private Const k_CurveStep As Single = 0.0078125F

		' Token: 0x04004DB5 RID: 19893
		Private m_GradingCurves As Texture2D

		' Token: 0x04004DB6 RID: 19894
		Private m_pixels As Color() = New Color(255) {}

		' Token: 0x02000BA0 RID: 2976
		Private NotInheritable Class Uniforms
			' Token: 0x04004DB7 RID: 19895
			Friend Shared _LutParams As Integer = Shader.PropertyToID("_LutParams")

			' Token: 0x04004DB8 RID: 19896
			Friend Shared _NeutralTonemapperParams1 As Integer = Shader.PropertyToID("_NeutralTonemapperParams1")

			' Token: 0x04004DB9 RID: 19897
			Friend Shared _NeutralTonemapperParams2 As Integer = Shader.PropertyToID("_NeutralTonemapperParams2")

			' Token: 0x04004DBA RID: 19898
			Friend Shared _HueShift As Integer = Shader.PropertyToID("_HueShift")

			' Token: 0x04004DBB RID: 19899
			Friend Shared _Saturation As Integer = Shader.PropertyToID("_Saturation")

			' Token: 0x04004DBC RID: 19900
			Friend Shared _Contrast As Integer = Shader.PropertyToID("_Contrast")

			' Token: 0x04004DBD RID: 19901
			Friend Shared _Balance As Integer = Shader.PropertyToID("_Balance")

			' Token: 0x04004DBE RID: 19902
			Friend Shared _Lift As Integer = Shader.PropertyToID("_Lift")

			' Token: 0x04004DBF RID: 19903
			Friend Shared _InvGamma As Integer = Shader.PropertyToID("_InvGamma")

			' Token: 0x04004DC0 RID: 19904
			Friend Shared _Gain As Integer = Shader.PropertyToID("_Gain")

			' Token: 0x04004DC1 RID: 19905
			Friend Shared _Slope As Integer = Shader.PropertyToID("_Slope")

			' Token: 0x04004DC2 RID: 19906
			Friend Shared _Power As Integer = Shader.PropertyToID("_Power")

			' Token: 0x04004DC3 RID: 19907
			Friend Shared _Offset As Integer = Shader.PropertyToID("_Offset")

			' Token: 0x04004DC4 RID: 19908
			Friend Shared _ChannelMixerRed As Integer = Shader.PropertyToID("_ChannelMixerRed")

			' Token: 0x04004DC5 RID: 19909
			Friend Shared _ChannelMixerGreen As Integer = Shader.PropertyToID("_ChannelMixerGreen")

			' Token: 0x04004DC6 RID: 19910
			Friend Shared _ChannelMixerBlue As Integer = Shader.PropertyToID("_ChannelMixerBlue")

			' Token: 0x04004DC7 RID: 19911
			Friend Shared _Curves As Integer = Shader.PropertyToID("_Curves")

			' Token: 0x04004DC8 RID: 19912
			Friend Shared _LogLut As Integer = Shader.PropertyToID("_LogLut")

			' Token: 0x04004DC9 RID: 19913
			Friend Shared _LogLut_Params As Integer = Shader.PropertyToID("_LogLut_Params")

			' Token: 0x04004DCA RID: 19914
			Friend Shared _ExposureEV As Integer = Shader.PropertyToID("_ExposureEV")
		End Class
	End Class
End Namespace
