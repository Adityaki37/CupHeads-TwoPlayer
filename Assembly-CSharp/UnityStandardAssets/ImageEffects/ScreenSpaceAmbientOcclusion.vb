Imports System
Imports UnityEngine

Namespace UnityStandardAssets.ImageEffects
	' Token: 0x02000CE7 RID: 3303
	<ExecuteInEditMode()>
	<RequireComponent(GetType(Camera))>
	<AddComponentMenu("Image Effects/Rendering/Screen Space Ambient Occlusion")>
	Public Class ScreenSpaceAmbientOcclusion
		Inherits MonoBehaviour

		' Token: 0x06005258 RID: 21080 RVA: 0x002A386C File Offset: 0x002A1C6C
		Private Shared Function CreateMaterial(shader As Shader) As Material
			If Not shader Then
				Return Nothing
			End If
			Return New Material(shader) With { .hideFlags = HideFlags.HideAndDontSave }
		End Function

		' Token: 0x06005259 RID: 21081 RVA: 0x002A3896 File Offset: 0x002A1C96
		Private Shared Sub DestroyMaterial(mat As Material)
			If mat Then
				Global.UnityEngine.[Object].DestroyImmediate(mat)
				mat = Nothing
			End If
		End Sub

		' Token: 0x0600525A RID: 21082 RVA: 0x002A38AC File Offset: 0x002A1CAC
		Private Sub OnDisable()
			ScreenSpaceAmbientOcclusion.DestroyMaterial(Me.m_SSAOMaterial)
		End Sub

		' Token: 0x0600525B RID: 21083 RVA: 0x002A38BC File Offset: 0x002A1CBC
		Private Sub Start()
			If Not SystemInfo.supportsImageEffects OrElse Not SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.Depth) Then
				Me.m_Supported = False
				MyBase.enabled = False
				Return
			End If
			Me.CreateMaterials()
			If Not Me.m_SSAOMaterial OrElse Me.m_SSAOMaterial.passCount <> 5 Then
				Me.m_Supported = False
				MyBase.enabled = False
				Return
			End If
			Me.m_Supported = True
		End Sub

		' Token: 0x0600525C RID: 21084 RVA: 0x002A392A File Offset: 0x002A1D2A
		Private Sub OnEnable()
			MyBase.GetComponent(Of Camera)().depthTextureMode = MyBase.GetComponent(Of Camera)().depthTextureMode Or DepthTextureMode.DepthNormals
		End Sub

		' Token: 0x0600525D RID: 21085 RVA: 0x002A3940 File Offset: 0x002A1D40
		Private Sub CreateMaterials()
			If Not Me.m_SSAOMaterial AndAlso Me.m_SSAOShader.isSupported Then
				Me.m_SSAOMaterial = ScreenSpaceAmbientOcclusion.CreateMaterial(Me.m_SSAOShader)
				Me.m_SSAOMaterial.SetTexture("_RandomTexture", Me.m_RandomTexture)
			End If
		End Sub

		' Token: 0x0600525E RID: 21086 RVA: 0x002A3994 File Offset: 0x002A1D94
		<ImageEffectOpaque()>
		Private Sub OnRenderImage(source As RenderTexture, destination As RenderTexture)
			If Not Me.m_Supported OrElse Not Me.m_SSAOShader.isSupported Then
				MyBase.enabled = False
				Return
			End If
			Me.CreateMaterials()
			Me.m_Downsampling = Mathf.Clamp(Me.m_Downsampling, 1, 6)
			Me.m_Radius = Mathf.Clamp(Me.m_Radius, 0.05F, 1F)
			Me.m_MinZ = Mathf.Clamp(Me.m_MinZ, 1E-05F, 0.5F)
			Me.m_OcclusionIntensity = Mathf.Clamp(Me.m_OcclusionIntensity, 0.5F, 4F)
			Me.m_OcclusionAttenuation = Mathf.Clamp(Me.m_OcclusionAttenuation, 0.2F, 2F)
			Me.m_Blur = Mathf.Clamp(Me.m_Blur, 0, 4)
			Dim renderTexture As RenderTexture = RenderTexture.GetTemporary(source.width / Me.m_Downsampling, source.height / Me.m_Downsampling, 0)
			Dim fieldOfView As Single = MyBase.GetComponent(Of Camera)().fieldOfView
			Dim farClipPlane As Single = MyBase.GetComponent(Of Camera)().farClipPlane
			Dim num As Single = Mathf.Tan(fieldOfView * 0.017453292F * 0.5F) * farClipPlane
			Dim num2 As Single = num * MyBase.GetComponent(Of Camera)().aspect
			Me.m_SSAOMaterial.SetVector("_FarCorner", New Vector3(num2, num, farClipPlane))
			Dim num3 As Integer
			Dim num4 As Integer
			If Me.m_RandomTexture Then
				num3 = Me.m_RandomTexture.width
				num4 = Me.m_RandomTexture.height
			Else
				num3 = 1
				num4 = 1
			End If
			Me.m_SSAOMaterial.SetVector("_NoiseScale", New Vector3(CSng(renderTexture.width) / CSng(num3), CSng(renderTexture.height) / CSng(num4), 0F))
			Me.m_SSAOMaterial.SetVector("_Params", New Vector4(Me.m_Radius, Me.m_MinZ, 1F / Me.m_OcclusionAttenuation, Me.m_OcclusionIntensity))
			Dim flag As Boolean = Me.m_Blur > 0
			Graphics.Blit(If((Not flag), source, Nothing), renderTexture, Me.m_SSAOMaterial, CInt(Me.m_SampleCount))
			If flag Then
				Dim temporary As RenderTexture = RenderTexture.GetTemporary(source.width, source.height, 0)
				Me.m_SSAOMaterial.SetVector("_TexelOffsetScale", New Vector4(CSng(Me.m_Blur) / CSng(source.width), 0F, 0F, 0F))
				Me.m_SSAOMaterial.SetTexture("_SSAO", renderTexture)
				Graphics.Blit(Nothing, temporary, Me.m_SSAOMaterial, 3)
				RenderTexture.ReleaseTemporary(renderTexture)
				Dim temporary2 As RenderTexture = RenderTexture.GetTemporary(source.width, source.height, 0)
				Me.m_SSAOMaterial.SetVector("_TexelOffsetScale", New Vector4(0F, CSng(Me.m_Blur) / CSng(source.height), 0F, 0F))
				Me.m_SSAOMaterial.SetTexture("_SSAO", temporary)
				Graphics.Blit(source, temporary2, Me.m_SSAOMaterial, 3)
				RenderTexture.ReleaseTemporary(temporary)
				renderTexture = temporary2
			End If
			Me.m_SSAOMaterial.SetTexture("_SSAO", renderTexture)
			Graphics.Blit(source, destination, Me.m_SSAOMaterial, 4)
			RenderTexture.ReleaseTemporary(renderTexture)
		End Sub

		' Token: 0x040056C5 RID: 22213
		Public m_Radius As Single = 0.4F

		' Token: 0x040056C6 RID: 22214
		Public m_SampleCount As ScreenSpaceAmbientOcclusion.SSAOSamples = ScreenSpaceAmbientOcclusion.SSAOSamples.Medium

		' Token: 0x040056C7 RID: 22215
		Public m_OcclusionIntensity As Single = 1.5F

		' Token: 0x040056C8 RID: 22216
		Public m_Blur As Integer = 2

		' Token: 0x040056C9 RID: 22217
		Public m_Downsampling As Integer = 2

		' Token: 0x040056CA RID: 22218
		Public m_OcclusionAttenuation As Single = 1F

		' Token: 0x040056CB RID: 22219
		Public m_MinZ As Single = 0.01F

		' Token: 0x040056CC RID: 22220
		Public m_SSAOShader As Shader

		' Token: 0x040056CD RID: 22221
		Private m_SSAOMaterial As Material

		' Token: 0x040056CE RID: 22222
		Public m_RandomTexture As Texture2D

		' Token: 0x040056CF RID: 22223
		Private m_Supported As Boolean

		' Token: 0x02000CE8 RID: 3304
		Public Enum SSAOSamples
			' Token: 0x040056D1 RID: 22225
			Low
			' Token: 0x040056D2 RID: 22226
			Medium
			' Token: 0x040056D3 RID: 22227
			High
		End Enum
	End Class
End Namespace
