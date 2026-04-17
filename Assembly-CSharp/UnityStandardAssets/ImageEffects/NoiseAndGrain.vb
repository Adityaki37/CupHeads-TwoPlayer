Imports System
Imports UnityEngine

Namespace UnityStandardAssets.ImageEffects
	' Token: 0x02000CDF RID: 3295
	<ExecuteInEditMode()>
	<RequireComponent(GetType(Camera))>
	<AddComponentMenu("Image Effects/Noise/Noise And Grain (Filmic)")>
	Public Class NoiseAndGrain
		Inherits PostEffectsBase

		' Token: 0x0600522D RID: 21037 RVA: 0x002A21C0 File Offset: 0x002A05C0
		Public Overrides Function CheckResources() As Boolean
			MyBase.CheckSupport(False)
			Me.noiseMaterial = MyBase.CheckShaderAndCreateMaterial(Me.noiseShader, Me.noiseMaterial)
			If Me.dx11Grain AndAlso Me.supportDX11 Then
				Me.dx11NoiseMaterial = MyBase.CheckShaderAndCreateMaterial(Me.dx11NoiseShader, Me.dx11NoiseMaterial)
			End If
			If Not Me.isSupported Then
				MyBase.ReportAutoDisable()
			End If
			Return Me.isSupported
		End Function

		' Token: 0x0600522E RID: 21038 RVA: 0x002A2234 File Offset: 0x002A0634
		Private Sub OnRenderImage(source As RenderTexture, destination As RenderTexture)
			If Not Me.CheckResources() OrElse Nothing Is Me.noiseTexture Then
				Graphics.Blit(source, destination)
				If Nothing Is Me.noiseTexture Then
				End If
				Return
			End If
			Me.softness = Mathf.Clamp(Me.softness, 0F, 0.99F)
			If Me.dx11Grain AndAlso Me.supportDX11 Then
				Me.dx11NoiseMaterial.SetFloat("_DX11NoiseTime", CSng(Time.frameCount))
				Me.dx11NoiseMaterial.SetTexture("_NoiseTex", Me.noiseTexture)
				Me.dx11NoiseMaterial.SetVector("_NoisePerChannel", If((Not Me.monochrome), Me.intensities, Vector3.one))
				Me.dx11NoiseMaterial.SetVector("_MidGrey", New Vector3(Me.midGrey, 1F / (1F - Me.midGrey), -1F / Me.midGrey))
				Me.dx11NoiseMaterial.SetVector("_NoiseAmount", New Vector3(Me.generalIntensity, Me.blackIntensity, Me.whiteIntensity) * Me.intensityMultiplier)
				If Me.softness > Mathf.Epsilon Then
					Dim temporary As RenderTexture = RenderTexture.GetTemporary(CInt((CSng(source.width) * (1F - Me.softness))), CInt((CSng(source.height) * (1F - Me.softness))))
					NoiseAndGrain.DrawNoiseQuadGrid(source, temporary, Me.dx11NoiseMaterial, Me.noiseTexture, If((Not Me.monochrome), 2, 3))
					Me.dx11NoiseMaterial.SetTexture("_NoiseTex", temporary)
					Graphics.Blit(source, destination, Me.dx11NoiseMaterial, 4)
					RenderTexture.ReleaseTemporary(temporary)
				Else
					NoiseAndGrain.DrawNoiseQuadGrid(source, destination, Me.dx11NoiseMaterial, Me.noiseTexture, If((Not Me.monochrome), 0, 1))
				End If
			Else
				If Me.noiseTexture Then
					Me.noiseTexture.wrapMode = TextureWrapMode.Repeat
					Me.noiseTexture.filterMode = Me.filterMode
				End If
				Me.noiseMaterial.SetTexture("_NoiseTex", Me.noiseTexture)
				Me.noiseMaterial.SetVector("_NoisePerChannel", If((Not Me.monochrome), Me.intensities, Vector3.one))
				Me.noiseMaterial.SetVector("_NoiseTilingPerChannel", If((Not Me.monochrome), Me.tiling, (Vector3.one * Me.monochromeTiling)))
				Me.noiseMaterial.SetVector("_MidGrey", New Vector3(Me.midGrey, 1F / (1F - Me.midGrey), -1F / Me.midGrey))
				Me.noiseMaterial.SetVector("_NoiseAmount", New Vector3(Me.generalIntensity, Me.blackIntensity, Me.whiteIntensity) * Me.intensityMultiplier)
				If Me.softness > Mathf.Epsilon Then
					Dim temporary2 As RenderTexture = RenderTexture.GetTemporary(CInt((CSng(source.width) * (1F - Me.softness))), CInt((CSng(source.height) * (1F - Me.softness))))
					NoiseAndGrain.DrawNoiseQuadGrid(source, temporary2, Me.noiseMaterial, Me.noiseTexture, 2)
					Me.noiseMaterial.SetTexture("_NoiseTex", temporary2)
					Graphics.Blit(source, destination, Me.noiseMaterial, 1)
					RenderTexture.ReleaseTemporary(temporary2)
				Else
					NoiseAndGrain.DrawNoiseQuadGrid(source, destination, Me.noiseMaterial, Me.noiseTexture, 0)
				End If
			End If
		End Sub

		' Token: 0x0600522F RID: 21039 RVA: 0x002A25E8 File Offset: 0x002A09E8
		Private Shared Sub DrawNoiseQuadGrid(source As RenderTexture, dest As RenderTexture, fxMaterial As Material, noise As Texture2D, passNr As Integer)
			RenderTexture.active = dest
			Dim num As Single = CSng(noise.width) * 1F
			Dim num2 As Single = 1F * CSng(source.width) / NoiseAndGrain.TILE_AMOUNT
			fxMaterial.SetTexture("_MainTex", source)
			GL.PushMatrix()
			GL.LoadOrtho()
			Dim num3 As Single = 1F * CSng(source.width) / (1F * CSng(source.height))
			Dim num4 As Single = 1F / num2
			Dim num5 As Single = num4 * num3
			Dim num6 As Single = num / (CSng(noise.width) * 1F)
			fxMaterial.SetPass(passNr)
			GL.Begin(7)
			Dim num7 As Single = 0F
			While num7 < 1F
				Dim num8 As Single = 0F
				While num8 < 1F
					Dim num9 As Single = Global.UnityEngine.Random.Range(0F, 1F)
					Dim num10 As Single = Global.UnityEngine.Random.Range(0F, 1F)
					num9 = Mathf.Floor(num9 * num) / num
					num10 = Mathf.Floor(num10 * num) / num
					Dim num11 As Single = 1F / num
					GL.MultiTexCoord2(0, num9, num10)
					GL.MultiTexCoord2(1, 0F, 0F)
					GL.Vertex3(num7, num8, 0.1F)
					GL.MultiTexCoord2(0, num9 + num6 * num11, num10)
					GL.MultiTexCoord2(1, 1F, 0F)
					GL.Vertex3(num7 + num4, num8, 0.1F)
					GL.MultiTexCoord2(0, num9 + num6 * num11, num10 + num6 * num11)
					GL.MultiTexCoord2(1, 1F, 1F)
					GL.Vertex3(num7 + num4, num8 + num5, 0.1F)
					GL.MultiTexCoord2(0, num9, num10 + num6 * num11)
					GL.MultiTexCoord2(1, 0F, 1F)
					GL.Vertex3(num7, num8 + num5, 0.1F)
					num8 += num5
				End While
				num7 += num4
			End While
			GL.[End]()
			GL.PopMatrix()
		End Sub

		' Token: 0x04005689 RID: 22153
		Public intensityMultiplier As Single = 0.25F

		' Token: 0x0400568A RID: 22154
		Public generalIntensity As Single = 0.5F

		' Token: 0x0400568B RID: 22155
		Public blackIntensity As Single = 1F

		' Token: 0x0400568C RID: 22156
		Public whiteIntensity As Single = 1F

		' Token: 0x0400568D RID: 22157
		Public midGrey As Single = 0.2F

		' Token: 0x0400568E RID: 22158
		Public dx11Grain As Boolean

		' Token: 0x0400568F RID: 22159
		Public softness As Single

		' Token: 0x04005690 RID: 22160
		Public monochrome As Boolean

		' Token: 0x04005691 RID: 22161
		Public intensities As Vector3 = New Vector3(1F, 1F, 1F)

		' Token: 0x04005692 RID: 22162
		Public tiling As Vector3 = New Vector3(64F, 64F, 64F)

		' Token: 0x04005693 RID: 22163
		Public monochromeTiling As Single = 64F

		' Token: 0x04005694 RID: 22164
		Public filterMode As FilterMode = FilterMode.Bilinear

		' Token: 0x04005695 RID: 22165
		Public noiseTexture As Texture2D

		' Token: 0x04005696 RID: 22166
		Public noiseShader As Shader

		' Token: 0x04005697 RID: 22167
		Private noiseMaterial As Material

		' Token: 0x04005698 RID: 22168
		Public dx11NoiseShader As Shader

		' Token: 0x04005699 RID: 22169
		Private dx11NoiseMaterial As Material

		' Token: 0x0400569A RID: 22170
		Private Shared TILE_AMOUNT As Single = 64F
	End Class
End Namespace
