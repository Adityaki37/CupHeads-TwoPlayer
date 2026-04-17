Imports System
Imports UnityEngine

Namespace UnityStandardAssets.ImageEffects
	' Token: 0x02000CE0 RID: 3296
	<ExecuteInEditMode()>
	<RequireComponent(GetType(Camera))>
	<AddComponentMenu("Image Effects/Noise/Noise and Scratches")>
	Public Class NoiseAndScratches
		Inherits MonoBehaviour

		' Token: 0x06005232 RID: 21042 RVA: 0x002A2840 File Offset: 0x002A0C40
		Protected Sub Start()
			If Not SystemInfo.supportsImageEffects Then
				MyBase.enabled = False
				Return
			End If
			If Me.shaderRGB Is Nothing OrElse Me.shaderYUV Is Nothing Then
				MyBase.enabled = False
			ElseIf Not Me.shaderRGB.isSupported Then
				MyBase.enabled = False
			ElseIf Not Me.shaderYUV.isSupported Then
				Me.rgbFallback = True
			End If
		End Sub

		' Token: 0x170008A9 RID: 2217
		' (get) Token: 0x06005233 RID: 21043 RVA: 0x002A28C0 File Offset: 0x002A0CC0
		Protected ReadOnly Property material As Material
			Get
				If Me.m_MaterialRGB Is Nothing Then
					Me.m_MaterialRGB = New Material(Me.shaderRGB)
					Me.m_MaterialRGB.hideFlags = HideFlags.HideAndDontSave
				End If
				If Me.m_MaterialYUV Is Nothing AndAlso Not Me.rgbFallback Then
					Me.m_MaterialYUV = New Material(Me.shaderYUV)
					Me.m_MaterialYUV.hideFlags = HideFlags.HideAndDontSave
				End If
				Return If((Me.rgbFallback OrElse Me.monochrome), Me.m_MaterialRGB, Me.m_MaterialYUV)
			End Get
		End Property

		' Token: 0x06005234 RID: 21044 RVA: 0x002A295D File Offset: 0x002A0D5D
		Protected Sub OnDisable()
			If Me.m_MaterialRGB Then
				Global.UnityEngine.[Object].DestroyImmediate(Me.m_MaterialRGB)
			End If
			If Me.m_MaterialYUV Then
				Global.UnityEngine.[Object].DestroyImmediate(Me.m_MaterialYUV)
			End If
		End Sub

		' Token: 0x06005235 RID: 21045 RVA: 0x002A2998 File Offset: 0x002A0D98
		Private Sub SanitizeParameters()
			Me.grainIntensityMin = Mathf.Clamp(Me.grainIntensityMin, 0F, 5F)
			Me.grainIntensityMax = Mathf.Clamp(Me.grainIntensityMax, 0F, 5F)
			Me.scratchIntensityMin = Mathf.Clamp(Me.scratchIntensityMin, 0F, 5F)
			Me.scratchIntensityMax = Mathf.Clamp(Me.scratchIntensityMax, 0F, 5F)
			Me.scratchFPS = Mathf.Clamp(Me.scratchFPS, 1F, 30F)
			Me.scratchJitter = Mathf.Clamp(Me.scratchJitter, 0F, 1F)
			Me.grainSize = Mathf.Clamp(Me.grainSize, 0.1F, 50F)
		End Sub

		' Token: 0x06005236 RID: 21046 RVA: 0x002A2A64 File Offset: 0x002A0E64
		Private Sub OnRenderImage(source As RenderTexture, destination As RenderTexture)
			Me.SanitizeParameters()
			If Me.scratchTimeLeft <= 0F Then
				Me.scratchTimeLeft = Global.UnityEngine.Random.value * 2F / Me.scratchFPS
				Me.scratchX = Global.UnityEngine.Random.value
				Me.scratchY = Global.UnityEngine.Random.value
			End If
			Me.scratchTimeLeft -= Time.deltaTime
			Dim material As Material = Me.material
			material.SetTexture("_GrainTex", Me.grainTexture)
			material.SetTexture("_ScratchTex", Me.scratchTexture)
			Dim num As Single = 1F / Me.grainSize
			material.SetVector("_GrainOffsetScale", New Vector4(Global.UnityEngine.Random.value, Global.UnityEngine.Random.value, CSng(Screen.width) / CSng(Me.grainTexture.width) * num, CSng(Screen.height) / CSng(Me.grainTexture.height) * num))
			material.SetVector("_ScratchOffsetScale", New Vector4(Me.scratchX + Global.UnityEngine.Random.value * Me.scratchJitter, Me.scratchY + Global.UnityEngine.Random.value * Me.scratchJitter, CSng(Screen.width) / CSng(Me.scratchTexture.width), CSng(Screen.height) / CSng(Me.scratchTexture.height)))
			material.SetVector("_Intensity", New Vector4(Global.UnityEngine.Random.Range(Me.grainIntensityMin, Me.grainIntensityMax), Global.UnityEngine.Random.Range(Me.scratchIntensityMin, Me.scratchIntensityMax), 0F, 0F))
			Graphics.Blit(source, destination, material)
		End Sub

		' Token: 0x0400569B RID: 22171
		Public monochrome As Boolean = True

		' Token: 0x0400569C RID: 22172
		Private rgbFallback As Boolean

		' Token: 0x0400569D RID: 22173
		Public grainIntensityMin As Single = 0.1F

		' Token: 0x0400569E RID: 22174
		Public grainIntensityMax As Single = 0.2F

		' Token: 0x0400569F RID: 22175
		Public grainSize As Single = 2F

		' Token: 0x040056A0 RID: 22176
		Public scratchIntensityMin As Single = 0.05F

		' Token: 0x040056A1 RID: 22177
		Public scratchIntensityMax As Single = 0.25F

		' Token: 0x040056A2 RID: 22178
		Public scratchFPS As Single = 10F

		' Token: 0x040056A3 RID: 22179
		Public scratchJitter As Single = 0.01F

		' Token: 0x040056A4 RID: 22180
		Public grainTexture As Texture

		' Token: 0x040056A5 RID: 22181
		Public scratchTexture As Texture

		' Token: 0x040056A6 RID: 22182
		Public shaderRGB As Shader

		' Token: 0x040056A7 RID: 22183
		Public shaderYUV As Shader

		' Token: 0x040056A8 RID: 22184
		Private m_MaterialRGB As Material

		' Token: 0x040056A9 RID: 22185
		Private m_MaterialYUV As Material

		' Token: 0x040056AA RID: 22186
		Private scratchTimeLeft As Single

		' Token: 0x040056AB RID: 22187
		Private scratchX As Single

		' Token: 0x040056AC RID: 22188
		Private scratchY As Single
	End Class
End Namespace
