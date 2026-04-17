Imports System
Imports UnityEngine

Namespace UnityStandardAssets.ImageEffects
	' Token: 0x02000CCF RID: 3279
	<ExecuteInEditMode()>
	<RequireComponent(GetType(Camera))>
	<AddComponentMenu("Image Effects/Camera/Depth of Field (Lens Blur, Scatter, DX11)")>
	Public Class DepthOfField
		Inherits PostEffectsBase

		' Token: 0x060051F9 RID: 20985 RVA: 0x0029F75C File Offset: 0x0029DB5C
		Public Overrides Function CheckResources() As Boolean
			MyBase.CheckSupport(True)
			Me.dofHdrMaterial = MyBase.CheckShaderAndCreateMaterial(Me.dofHdrShader, Me.dofHdrMaterial)
			If Me.supportDX11 AndAlso Me.blurType = DepthOfField.BlurType.DX11 Then
				Me.dx11bokehMaterial = MyBase.CheckShaderAndCreateMaterial(Me.dx11BokehShader, Me.dx11bokehMaterial)
				Me.CreateComputeResources()
			End If
			If Not Me.isSupported Then
				MyBase.ReportAutoDisable()
			End If
			Return Me.isSupported
		End Function

		' Token: 0x060051FA RID: 20986 RVA: 0x0029F7D5 File Offset: 0x0029DBD5
		Private Sub OnEnable()
			MyBase.GetComponent(Of Camera)().depthTextureMode = MyBase.GetComponent(Of Camera)().depthTextureMode Or DepthTextureMode.Depth
		End Sub

		' Token: 0x060051FB RID: 20987 RVA: 0x0029F7EC File Offset: 0x0029DBEC
		Private Sub OnDisable()
			Me.ReleaseComputeResources()
			If Me.dofHdrMaterial Then
				Global.UnityEngine.[Object].DestroyImmediate(Me.dofHdrMaterial)
			End If
			Me.dofHdrMaterial = Nothing
			If Me.dx11bokehMaterial Then
				Global.UnityEngine.[Object].DestroyImmediate(Me.dx11bokehMaterial)
			End If
			Me.dx11bokehMaterial = Nothing
		End Sub

		' Token: 0x060051FC RID: 20988 RVA: 0x0029F843 File Offset: 0x0029DC43
		Private Sub ReleaseComputeResources()
			If Me.cbDrawArgs IsNot Nothing Then
				Me.cbDrawArgs.Release()
			End If
			Me.cbDrawArgs = Nothing
			If Me.cbPoints IsNot Nothing Then
				Me.cbPoints.Release()
			End If
			Me.cbPoints = Nothing
		End Sub

		' Token: 0x060051FD RID: 20989 RVA: 0x0029F880 File Offset: 0x0029DC80
		Private Sub CreateComputeResources()
			If Me.cbDrawArgs Is Nothing Then
				Me.cbDrawArgs = New ComputeBuffer(1, 16, ComputeBufferType.DrawIndirect)
				Dim array As Integer() = New Integer() { 0, 1, 0, 0 }
				Me.cbDrawArgs.SetData(array)
			End If
			If Me.cbPoints Is Nothing Then
				Me.cbPoints = New ComputeBuffer(90000, 28, ComputeBufferType.Append)
			End If
		End Sub

		' Token: 0x060051FE RID: 20990 RVA: 0x0029F8EC File Offset: 0x0029DCEC
		Private Function FocalDistance01(worldDist As Single) As Single
			Return MyBase.GetComponent(Of Camera)().WorldToViewportPoint((worldDist - MyBase.GetComponent(Of Camera)().nearClipPlane) * MyBase.GetComponent(Of Camera)().transform.forward + MyBase.GetComponent(Of Camera)().transform.position).z / (MyBase.GetComponent(Of Camera)().farClipPlane - MyBase.GetComponent(Of Camera)().nearClipPlane)
		End Function

		' Token: 0x060051FF RID: 20991 RVA: 0x0029F95C File Offset: 0x0029DD5C
		Private Sub WriteCoc(fromTo As RenderTexture, fgDilate As Boolean)
			Me.dofHdrMaterial.SetTexture("_FgOverlap", Nothing)
			If Me.nearBlur AndAlso fgDilate Then
				Dim num As Integer = fromTo.width / 2
				Dim num2 As Integer = fromTo.height / 2
				Dim renderTexture As RenderTexture = RenderTexture.GetTemporary(num, num2, 0, fromTo.format)
				Graphics.Blit(fromTo, renderTexture, Me.dofHdrMaterial, 4)
				Dim num3 As Single = Me.internalBlurWidth * Me.foregroundOverlap
				Me.dofHdrMaterial.SetVector("_Offsets", New Vector4(0F, num3, 0F, num3))
				Dim temporary As RenderTexture = RenderTexture.GetTemporary(num, num2, 0, fromTo.format)
				Graphics.Blit(renderTexture, temporary, Me.dofHdrMaterial, 2)
				RenderTexture.ReleaseTemporary(renderTexture)
				Me.dofHdrMaterial.SetVector("_Offsets", New Vector4(num3, 0F, 0F, num3))
				renderTexture = RenderTexture.GetTemporary(num, num2, 0, fromTo.format)
				Graphics.Blit(temporary, renderTexture, Me.dofHdrMaterial, 2)
				RenderTexture.ReleaseTemporary(temporary)
				Me.dofHdrMaterial.SetTexture("_FgOverlap", renderTexture)
				fromTo.MarkRestoreExpected()
				Graphics.Blit(fromTo, fromTo, Me.dofHdrMaterial, 13)
				RenderTexture.ReleaseTemporary(renderTexture)
			Else
				fromTo.MarkRestoreExpected()
				Graphics.Blit(fromTo, fromTo, Me.dofHdrMaterial, 0)
			End If
		End Sub

		' Token: 0x06005200 RID: 20992 RVA: 0x0029FA9C File Offset: 0x0029DE9C
		Private Sub OnRenderImage(source As RenderTexture, destination As RenderTexture)
			If Not Me.CheckResources() Then
				Graphics.Blit(source, destination)
				Return
			End If
			If Me.aperture < 0F Then
				Me.aperture = 0F
			End If
			If Me.maxBlurSize < 0.1F Then
				Me.maxBlurSize = 0.1F
			End If
			Me.focalSize = Mathf.Clamp(Me.focalSize, 0F, 2F)
			Me.internalBlurWidth = Mathf.Max(Me.maxBlurSize, 0F)
			Me.focalDistance01 = If((Not Me.focalTransform), Me.FocalDistance01(Me.focalLength), (MyBase.GetComponent(Of Camera)().WorldToViewportPoint(Me.focalTransform.position).z / MyBase.GetComponent(Of Camera)().farClipPlane))
			Me.dofHdrMaterial.SetVector("_CurveParams", New Vector4(1F, Me.focalSize, Me.aperture / 10F, Me.focalDistance01))
			Dim renderTexture As RenderTexture = Nothing
			Dim renderTexture2 As RenderTexture = Nothing
			Dim num As Single = Me.internalBlurWidth * Me.foregroundOverlap
			If Me.visualizeFocus Then
				Me.WriteCoc(source, True)
				Graphics.Blit(source, destination, Me.dofHdrMaterial, 16)
			ElseIf Me.blurType = DepthOfField.BlurType.DX11 AndAlso Me.dx11bokehMaterial Then
				If Me.highResolution Then
					Me.internalBlurWidth = If((Me.internalBlurWidth >= 0.1F), Me.internalBlurWidth, 0.1F)
					num = Me.internalBlurWidth * Me.foregroundOverlap
					renderTexture = RenderTexture.GetTemporary(source.width, source.height, 0, source.format)
					Dim temporary As RenderTexture = RenderTexture.GetTemporary(source.width, source.height, 0, source.format)
					Me.WriteCoc(source, False)
					Dim renderTexture3 As RenderTexture = RenderTexture.GetTemporary(source.width >> 1, source.height >> 1, 0, source.format)
					Dim renderTexture4 As RenderTexture = RenderTexture.GetTemporary(source.width >> 1, source.height >> 1, 0, source.format)
					Graphics.Blit(source, renderTexture3, Me.dofHdrMaterial, 15)
					Me.dofHdrMaterial.SetVector("_Offsets", New Vector4(0F, 1.5F, 0F, 1.5F))
					Graphics.Blit(renderTexture3, renderTexture4, Me.dofHdrMaterial, 19)
					Me.dofHdrMaterial.SetVector("_Offsets", New Vector4(1.5F, 0F, 0F, 1.5F))
					Graphics.Blit(renderTexture4, renderTexture3, Me.dofHdrMaterial, 19)
					If Me.nearBlur Then
						Graphics.Blit(source, renderTexture4, Me.dofHdrMaterial, 4)
					End If
					Me.dx11bokehMaterial.SetTexture("_BlurredColor", renderTexture3)
					Me.dx11bokehMaterial.SetFloat("_SpawnHeuristic", Me.dx11SpawnHeuristic)
					Me.dx11bokehMaterial.SetVector("_BokehParams", New Vector4(Me.dx11BokehScale, Me.dx11BokehIntensity, Mathf.Clamp(Me.dx11BokehThreshold, 0.005F, 4F), Me.internalBlurWidth))
					Me.dx11bokehMaterial.SetTexture("_FgCocMask", If((Not Me.nearBlur), Nothing, renderTexture4))
					Graphics.SetRandomWriteTarget(1, Me.cbPoints)
					Graphics.Blit(source, renderTexture, Me.dx11bokehMaterial, 0)
					Graphics.ClearRandomWriteTargets()
					If Me.nearBlur Then
						Me.dofHdrMaterial.SetVector("_Offsets", New Vector4(0F, num, 0F, num))
						Graphics.Blit(renderTexture4, renderTexture3, Me.dofHdrMaterial, 2)
						Me.dofHdrMaterial.SetVector("_Offsets", New Vector4(num, 0F, 0F, num))
						Graphics.Blit(renderTexture3, renderTexture4, Me.dofHdrMaterial, 2)
						Graphics.Blit(renderTexture4, renderTexture, Me.dofHdrMaterial, 3)
					End If
					Graphics.Blit(renderTexture, temporary, Me.dofHdrMaterial, 20)
					Me.dofHdrMaterial.SetVector("_Offsets", New Vector4(Me.internalBlurWidth, 0F, 0F, Me.internalBlurWidth))
					Graphics.Blit(renderTexture, source, Me.dofHdrMaterial, 5)
					Me.dofHdrMaterial.SetVector("_Offsets", New Vector4(0F, Me.internalBlurWidth, 0F, Me.internalBlurWidth))
					Graphics.Blit(source, temporary, Me.dofHdrMaterial, 21)
					Graphics.SetRenderTarget(temporary)
					ComputeBuffer.CopyCount(Me.cbPoints, Me.cbDrawArgs, 0)
					Me.dx11bokehMaterial.SetBuffer("pointBuffer", Me.cbPoints)
					Me.dx11bokehMaterial.SetTexture("_MainTex", Me.dx11BokehTexture)
					Me.dx11bokehMaterial.SetVector("_Screen", New Vector3(1F / (1F * CSng(source.width)), 1F / (1F * CSng(source.height)), Me.internalBlurWidth))
					Me.dx11bokehMaterial.SetPass(2)
					Graphics.DrawProceduralIndirect(MeshTopology.Points, Me.cbDrawArgs, 0)
					Graphics.Blit(temporary, destination)
					RenderTexture.ReleaseTemporary(temporary)
					RenderTexture.ReleaseTemporary(renderTexture3)
					RenderTexture.ReleaseTemporary(renderTexture4)
				Else
					renderTexture = RenderTexture.GetTemporary(source.width >> 1, source.height >> 1, 0, source.format)
					renderTexture2 = RenderTexture.GetTemporary(source.width >> 1, source.height >> 1, 0, source.format)
					num = Me.internalBlurWidth * Me.foregroundOverlap
					Me.WriteCoc(source, False)
					source.filterMode = FilterMode.Bilinear
					Graphics.Blit(source, renderTexture, Me.dofHdrMaterial, 6)
					Dim renderTexture3 As RenderTexture = RenderTexture.GetTemporary(renderTexture.width >> 1, renderTexture.height >> 1, 0, renderTexture.format)
					Dim renderTexture4 As RenderTexture = RenderTexture.GetTemporary(renderTexture.width >> 1, renderTexture.height >> 1, 0, renderTexture.format)
					Graphics.Blit(renderTexture, renderTexture3, Me.dofHdrMaterial, 15)
					Me.dofHdrMaterial.SetVector("_Offsets", New Vector4(0F, 1.5F, 0F, 1.5F))
					Graphics.Blit(renderTexture3, renderTexture4, Me.dofHdrMaterial, 19)
					Me.dofHdrMaterial.SetVector("_Offsets", New Vector4(1.5F, 0F, 0F, 1.5F))
					Graphics.Blit(renderTexture4, renderTexture3, Me.dofHdrMaterial, 19)
					Dim renderTexture5 As RenderTexture = Nothing
					If Me.nearBlur Then
						renderTexture5 = RenderTexture.GetTemporary(source.width >> 1, source.height >> 1, 0, source.format)
						Graphics.Blit(source, renderTexture5, Me.dofHdrMaterial, 4)
					End If
					Me.dx11bokehMaterial.SetTexture("_BlurredColor", renderTexture3)
					Me.dx11bokehMaterial.SetFloat("_SpawnHeuristic", Me.dx11SpawnHeuristic)
					Me.dx11bokehMaterial.SetVector("_BokehParams", New Vector4(Me.dx11BokehScale, Me.dx11BokehIntensity, Mathf.Clamp(Me.dx11BokehThreshold, 0.005F, 4F), Me.internalBlurWidth))
					Me.dx11bokehMaterial.SetTexture("_FgCocMask", renderTexture5)
					Graphics.SetRandomWriteTarget(1, Me.cbPoints)
					Graphics.Blit(renderTexture, renderTexture2, Me.dx11bokehMaterial, 0)
					Graphics.ClearRandomWriteTargets()
					RenderTexture.ReleaseTemporary(renderTexture3)
					RenderTexture.ReleaseTemporary(renderTexture4)
					If Me.nearBlur Then
						Me.dofHdrMaterial.SetVector("_Offsets", New Vector4(0F, num, 0F, num))
						Graphics.Blit(renderTexture5, renderTexture, Me.dofHdrMaterial, 2)
						Me.dofHdrMaterial.SetVector("_Offsets", New Vector4(num, 0F, 0F, num))
						Graphics.Blit(renderTexture, renderTexture5, Me.dofHdrMaterial, 2)
						Graphics.Blit(renderTexture5, renderTexture2, Me.dofHdrMaterial, 3)
					End If
					Me.dofHdrMaterial.SetVector("_Offsets", New Vector4(Me.internalBlurWidth, 0F, 0F, Me.internalBlurWidth))
					Graphics.Blit(renderTexture2, renderTexture, Me.dofHdrMaterial, 5)
					Me.dofHdrMaterial.SetVector("_Offsets", New Vector4(0F, Me.internalBlurWidth, 0F, Me.internalBlurWidth))
					Graphics.Blit(renderTexture, renderTexture2, Me.dofHdrMaterial, 5)
					Graphics.SetRenderTarget(renderTexture2)
					ComputeBuffer.CopyCount(Me.cbPoints, Me.cbDrawArgs, 0)
					Me.dx11bokehMaterial.SetBuffer("pointBuffer", Me.cbPoints)
					Me.dx11bokehMaterial.SetTexture("_MainTex", Me.dx11BokehTexture)
					Me.dx11bokehMaterial.SetVector("_Screen", New Vector3(1F / (1F * CSng(renderTexture2.width)), 1F / (1F * CSng(renderTexture2.height)), Me.internalBlurWidth))
					Me.dx11bokehMaterial.SetPass(1)
					Graphics.DrawProceduralIndirect(MeshTopology.Points, Me.cbDrawArgs, 0)
					Me.dofHdrMaterial.SetTexture("_LowRez", renderTexture2)
					Me.dofHdrMaterial.SetTexture("_FgOverlap", renderTexture5)
					Me.dofHdrMaterial.SetVector("_Offsets", 1F * CSng(source.width) / (1F * CSng(renderTexture2.width)) * Me.internalBlurWidth * Vector4.one)
					Graphics.Blit(source, destination, Me.dofHdrMaterial, 9)
					If renderTexture5 Then
						RenderTexture.ReleaseTemporary(renderTexture5)
					End If
				End If
			Else
				source.filterMode = FilterMode.Bilinear
				If Me.highResolution Then
					Me.internalBlurWidth *= 2F
				End If
				Me.WriteCoc(source, True)
				renderTexture = RenderTexture.GetTemporary(source.width >> 1, source.height >> 1, 0, source.format)
				renderTexture2 = RenderTexture.GetTemporary(source.width >> 1, source.height >> 1, 0, source.format)
				Dim num2 As Integer = If((Me.blurSampleCount <> DepthOfField.BlurSampleCount.High AndAlso Me.blurSampleCount <> DepthOfField.BlurSampleCount.Medium), 11, 17)
				If Me.highResolution Then
					Me.dofHdrMaterial.SetVector("_Offsets", New Vector4(0F, Me.internalBlurWidth, 0.025F, Me.internalBlurWidth))
					Graphics.Blit(source, destination, Me.dofHdrMaterial, num2)
				Else
					Me.dofHdrMaterial.SetVector("_Offsets", New Vector4(0F, Me.internalBlurWidth, 0.1F, Me.internalBlurWidth))
					Graphics.Blit(source, renderTexture, Me.dofHdrMaterial, 6)
					Graphics.Blit(renderTexture, renderTexture2, Me.dofHdrMaterial, num2)
					Me.dofHdrMaterial.SetTexture("_LowRez", renderTexture2)
					Me.dofHdrMaterial.SetTexture("_FgOverlap", Nothing)
					Me.dofHdrMaterial.SetVector("_Offsets", Vector4.one * (1F * CSng(source.width) / (1F * CSng(renderTexture2.width))) * Me.internalBlurWidth)
					Graphics.Blit(source, destination, Me.dofHdrMaterial, If((Me.blurSampleCount <> DepthOfField.BlurSampleCount.High), 12, 18))
				End If
			End If
			If renderTexture Then
				RenderTexture.ReleaseTemporary(renderTexture)
			End If
			If renderTexture2 Then
				RenderTexture.ReleaseTemporary(renderTexture2)
			End If
		End Sub

		' Token: 0x0400560C RID: 22028
		Public visualizeFocus As Boolean

		' Token: 0x0400560D RID: 22029
		Public focalLength As Single = 10F

		' Token: 0x0400560E RID: 22030
		Public focalSize As Single = 0.05F

		' Token: 0x0400560F RID: 22031
		Public aperture As Single = 11.5F

		' Token: 0x04005610 RID: 22032
		Public focalTransform As Transform

		' Token: 0x04005611 RID: 22033
		Public maxBlurSize As Single = 2F

		' Token: 0x04005612 RID: 22034
		Public highResolution As Boolean

		' Token: 0x04005613 RID: 22035
		Public blurType As DepthOfField.BlurType

		' Token: 0x04005614 RID: 22036
		Public blurSampleCount As DepthOfField.BlurSampleCount = DepthOfField.BlurSampleCount.High

		' Token: 0x04005615 RID: 22037
		Public nearBlur As Boolean

		' Token: 0x04005616 RID: 22038
		Public foregroundOverlap As Single = 1F

		' Token: 0x04005617 RID: 22039
		Public dofHdrShader As Shader

		' Token: 0x04005618 RID: 22040
		Private dofHdrMaterial As Material

		' Token: 0x04005619 RID: 22041
		Public dx11BokehShader As Shader

		' Token: 0x0400561A RID: 22042
		Private dx11bokehMaterial As Material

		' Token: 0x0400561B RID: 22043
		Public dx11BokehThreshold As Single = 0.5F

		' Token: 0x0400561C RID: 22044
		Public dx11SpawnHeuristic As Single = 0.0875F

		' Token: 0x0400561D RID: 22045
		Public dx11BokehTexture As Texture2D

		' Token: 0x0400561E RID: 22046
		Public dx11BokehScale As Single = 1.2F

		' Token: 0x0400561F RID: 22047
		Public dx11BokehIntensity As Single = 2.5F

		' Token: 0x04005620 RID: 22048
		Private focalDistance01 As Single = 10F

		' Token: 0x04005621 RID: 22049
		Private cbDrawArgs As ComputeBuffer

		' Token: 0x04005622 RID: 22050
		Private cbPoints As ComputeBuffer

		' Token: 0x04005623 RID: 22051
		Private internalBlurWidth As Single = 1F

		' Token: 0x02000CD0 RID: 3280
		Public Enum BlurType
			' Token: 0x04005625 RID: 22053
			DiscBlur
			' Token: 0x04005626 RID: 22054
			DX11
		End Enum

		' Token: 0x02000CD1 RID: 3281
		Public Enum BlurSampleCount
			' Token: 0x04005628 RID: 22056
			Low
			' Token: 0x04005629 RID: 22057
			Medium
			' Token: 0x0400562A RID: 22058
			High
		End Enum
	End Class
End Namespace
