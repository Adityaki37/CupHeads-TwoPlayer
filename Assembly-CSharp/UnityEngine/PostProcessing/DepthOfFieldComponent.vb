Imports System

Namespace UnityEngine.PostProcessing
	' Token: 0x02000BA1 RID: 2977
	Public NotInheritable Class DepthOfFieldComponent
		Inherits PostProcessingComponentRenderTexture(Of DepthOfFieldModel)

		' Token: 0x1700064D RID: 1613
		' (get) Token: 0x0600485A RID: 18522 RVA: 0x0025FFD5 File Offset: 0x0025E3D5
		Public Overrides ReadOnly Property active As Boolean
			Get
				Return MyBase.model.enabled AndAlso Not Me.context.interrupted
			End Get
		End Property

		' Token: 0x0600485B RID: 18523 RVA: 0x0025FFF8 File Offset: 0x0025E3F8
		Public Overrides Function GetCameraFlags() As DepthTextureMode
			Return DepthTextureMode.Depth
		End Function

		' Token: 0x0600485C RID: 18524 RVA: 0x0025FFFC File Offset: 0x0025E3FC
		Private Function CalculateFocalLength() As Single
			Dim settings As DepthOfFieldModel.Settings = MyBase.model.settings
			If Not settings.useCameraFov Then
				Return settings.focalLength / 1000F
			End If
			Dim num As Single = Me.context.camera.fieldOfView * 0.017453292F
			Return 0.012F / Mathf.Tan(0.5F * num)
		End Function

		' Token: 0x0600485D RID: 18525 RVA: 0x00260058 File Offset: 0x0025E458
		Private Function CalculateMaxCoCRadius(screenHeight As Integer) As Single
			Dim num As Single = CSng(MyBase.model.settings.kernelSize) * 4F + 6F
			Return Mathf.Min(0.05F, num / CSng(screenHeight))
		End Function

		' Token: 0x0600485E RID: 18526 RVA: 0x00260094 File Offset: 0x0025E494
		Private Function CheckHistory(width As Integer, height As Integer) As Boolean
			Return Me.m_CoCHistory IsNot Nothing AndAlso Me.m_CoCHistory.IsCreated() AndAlso Me.m_CoCHistory.width = width AndAlso Me.m_CoCHistory.height = height
		End Function

		' Token: 0x0600485F RID: 18527 RVA: 0x002600E4 File Offset: 0x0025E4E4
		Private Function SelectFormat(primary As RenderTextureFormat, secondary As RenderTextureFormat) As RenderTextureFormat
			If SystemInfo.SupportsRenderTextureFormat(primary) Then
				Return primary
			End If
			If SystemInfo.SupportsRenderTextureFormat(secondary) Then
				Return secondary
			End If
			Return RenderTextureFormat.[Default]
		End Function

		' Token: 0x06004860 RID: 18528 RVA: 0x00260104 File Offset: 0x0025E504
		Public Sub Prepare(source As RenderTexture, uberMaterial As Material, antialiasCoC As Boolean, taaJitter As Vector2, taaBlending As Single)
			Dim settings As DepthOfFieldModel.Settings = MyBase.model.settings
			Dim renderTextureFormat As RenderTextureFormat = RenderTextureFormat.DefaultHDR
			Dim renderTextureFormat2 As RenderTextureFormat = Me.SelectFormat(RenderTextureFormat.R8, RenderTextureFormat.RHalf)
			Dim num As Single = Me.CalculateFocalLength()
			Dim num2 As Single = Mathf.Max(settings.focusDistance, num)
			Dim num3 As Single = CSng(source.width) / CSng(source.height)
			Dim num4 As Single = num * num / (settings.aperture * (num2 - num) * 0.024F * 2F)
			Dim num5 As Single = Me.CalculateMaxCoCRadius(source.height)
			Dim material As Material = Me.context.materialFactory.[Get]("Hidden/Post FX/Depth Of Field")
			material.SetFloat(DepthOfFieldComponent.Uniforms._Distance, num2)
			material.SetFloat(DepthOfFieldComponent.Uniforms._LensCoeff, num4)
			material.SetFloat(DepthOfFieldComponent.Uniforms._MaxCoC, num5)
			material.SetFloat(DepthOfFieldComponent.Uniforms._RcpMaxCoC, 1F / num5)
			material.SetFloat(DepthOfFieldComponent.Uniforms._RcpAspect, 1F / num3)
			Dim renderTexture As RenderTexture = Me.context.renderTextureFactory.[Get](Me.context.width, Me.context.height, 0, renderTextureFormat2, RenderTextureReadWrite.Linear, FilterMode.Bilinear, TextureWrapMode.Clamp, "FactoryTempTexture")
			Graphics.Blit(Nothing, renderTexture, material, 0)
			If antialiasCoC Then
				material.SetTexture(DepthOfFieldComponent.Uniforms._CoCTex, renderTexture)
				Dim num6 As Single = If((Not Me.CheckHistory(Me.context.width, Me.context.height)), 0F, taaBlending)
				material.SetVector(DepthOfFieldComponent.Uniforms._TaaParams, New Vector3(taaJitter.x, taaJitter.y, num6))
				Dim temporary As RenderTexture = RenderTexture.GetTemporary(Me.context.width, Me.context.height, 0, renderTextureFormat2)
				Graphics.Blit(Me.m_CoCHistory, temporary, material, 1)
				Me.context.renderTextureFactory.Release(renderTexture)
				If Me.m_CoCHistory IsNot Nothing Then
					RenderTexture.ReleaseTemporary(Me.m_CoCHistory)
				End If
				Dim renderTexture2 As RenderTexture = temporary
				renderTexture = renderTexture2
				Me.m_CoCHistory = renderTexture2
			End If
			Dim renderTexture3 As RenderTexture = Me.context.renderTextureFactory.[Get](Me.context.width / 2, Me.context.height / 2, 0, renderTextureFormat, RenderTextureReadWrite.[Default], FilterMode.Bilinear, TextureWrapMode.Clamp, "FactoryTempTexture")
			material.SetTexture(DepthOfFieldComponent.Uniforms._CoCTex, renderTexture)
			Graphics.Blit(source, renderTexture3, material, 2)
			Dim renderTexture4 As RenderTexture = Me.context.renderTextureFactory.[Get](Me.context.width / 2, Me.context.height / 2, 0, renderTextureFormat, RenderTextureReadWrite.[Default], FilterMode.Bilinear, TextureWrapMode.Clamp, "FactoryTempTexture")
			Graphics.Blit(renderTexture3, renderTexture4, material, CInt((3 + settings.kernelSize)))
			Graphics.Blit(renderTexture4, renderTexture3, material, 7)
			uberMaterial.SetVector(DepthOfFieldComponent.Uniforms._DepthOfFieldParams, New Vector3(num2, num4, num5))
			If Me.context.profile.debugViews.IsModeActive(BuiltinDebugViewsModel.Mode.FocusPlane) Then
				uberMaterial.EnableKeyword("DEPTH_OF_FIELD_COC_VIEW")
				Me.context.Interrupt()
			Else
				uberMaterial.SetTexture(DepthOfFieldComponent.Uniforms._DepthOfFieldTex, renderTexture3)
				uberMaterial.SetTexture(DepthOfFieldComponent.Uniforms._DepthOfFieldCoCTex, renderTexture)
				uberMaterial.EnableKeyword("DEPTH_OF_FIELD")
			End If
			Me.context.renderTextureFactory.Release(renderTexture4)
		End Sub

		' Token: 0x06004861 RID: 18529 RVA: 0x00260422 File Offset: 0x0025E822
		Public Overrides Sub OnDisable()
			If Me.m_CoCHistory IsNot Nothing Then
				RenderTexture.ReleaseTemporary(Me.m_CoCHistory)
			End If
			Me.m_CoCHistory = Nothing
		End Sub

		' Token: 0x04004DCB RID: 19915
		Private Const k_ShaderString As String = "Hidden/Post FX/Depth Of Field"

		' Token: 0x04004DCC RID: 19916
		Private m_CoCHistory As RenderTexture

		' Token: 0x04004DCD RID: 19917
		Private Const k_FilmHeight As Single = 0.024F

		' Token: 0x02000BA2 RID: 2978
		Private NotInheritable Class Uniforms
			' Token: 0x04004DCE RID: 19918
			Friend Shared _DepthOfFieldTex As Integer = Shader.PropertyToID("_DepthOfFieldTex")

			' Token: 0x04004DCF RID: 19919
			Friend Shared _DepthOfFieldCoCTex As Integer = Shader.PropertyToID("_DepthOfFieldCoCTex")

			' Token: 0x04004DD0 RID: 19920
			Friend Shared _Distance As Integer = Shader.PropertyToID("_Distance")

			' Token: 0x04004DD1 RID: 19921
			Friend Shared _LensCoeff As Integer = Shader.PropertyToID("_LensCoeff")

			' Token: 0x04004DD2 RID: 19922
			Friend Shared _MaxCoC As Integer = Shader.PropertyToID("_MaxCoC")

			' Token: 0x04004DD3 RID: 19923
			Friend Shared _RcpMaxCoC As Integer = Shader.PropertyToID("_RcpMaxCoC")

			' Token: 0x04004DD4 RID: 19924
			Friend Shared _RcpAspect As Integer = Shader.PropertyToID("_RcpAspect")

			' Token: 0x04004DD5 RID: 19925
			Friend Shared _MainTex As Integer = Shader.PropertyToID("_MainTex")

			' Token: 0x04004DD6 RID: 19926
			Friend Shared _CoCTex As Integer = Shader.PropertyToID("_CoCTex")

			' Token: 0x04004DD7 RID: 19927
			Friend Shared _TaaParams As Integer = Shader.PropertyToID("_TaaParams")

			' Token: 0x04004DD8 RID: 19928
			Friend Shared _DepthOfFieldParams As Integer = Shader.PropertyToID("_DepthOfFieldParams")
		End Class
	End Class
End Namespace
