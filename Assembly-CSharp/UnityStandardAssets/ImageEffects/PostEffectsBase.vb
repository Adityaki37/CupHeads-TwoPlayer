Imports System
Imports UnityEngine

Namespace UnityStandardAssets.ImageEffects
	' Token: 0x02000CE1 RID: 3297
	<ExecuteInEditMode()>
	<RequireComponent(GetType(Camera))>
	Public Class PostEffectsBase
		Inherits MonoBehaviour

		' Token: 0x06005238 RID: 21048 RVA: 0x0008CA34 File Offset: 0x0008AE34
		Protected Function CheckShaderAndCreateMaterial(s As Shader, m2Create As Material) As Material
			If Not s Then
				MyBase.enabled = False
				Return Nothing
			End If
			If s.isSupported AndAlso m2Create AndAlso m2Create.shader Is s Then
				Return m2Create
			End If
			If Not s.isSupported Then
				Me.NotSupported()
				Return Nothing
			End If
			m2Create = New Material(s)
			m2Create.hideFlags = HideFlags.DontSave
			If m2Create Then
				Return m2Create
			End If
			Return Nothing
		End Function

		' Token: 0x06005239 RID: 21049 RVA: 0x0008CAB0 File Offset: 0x0008AEB0
		Protected Function CreateMaterial(s As Shader, m2Create As Material) As Material
			If Not s Then
				Return Nothing
			End If
			If m2Create AndAlso m2Create.shader Is s AndAlso s.isSupported Then
				Return m2Create
			End If
			If Not s.isSupported Then
				Return Nothing
			End If
			m2Create = New Material(s)
			m2Create.hideFlags = HideFlags.DontSave
			If m2Create Then
				Return m2Create
			End If
			Return Nothing
		End Function

		' Token: 0x0600523A RID: 21050 RVA: 0x0008CB1E File Offset: 0x0008AF1E
		Private Sub OnEnable()
			Me.isSupported = True
		End Sub

		' Token: 0x0600523B RID: 21051 RVA: 0x0008CB27 File Offset: 0x0008AF27
		Protected Function CheckSupport() As Boolean
			Return Me.CheckSupport(False)
		End Function

		' Token: 0x0600523C RID: 21052 RVA: 0x0008CB30 File Offset: 0x0008AF30
		Public Overridable Function CheckResources() As Boolean
			Return Me.isSupported
		End Function

		' Token: 0x0600523D RID: 21053 RVA: 0x0008CB38 File Offset: 0x0008AF38
		Protected Overridable Sub Start()
			Me.CheckResources()
		End Sub

		' Token: 0x0600523E RID: 21054 RVA: 0x0008CB44 File Offset: 0x0008AF44
		Protected Function CheckSupport(needDepth As Boolean) As Boolean
			Me.isSupported = True
			Me.supportHDRTextures = SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.ARGBHalf)
			Me.supportDX11 = SystemInfo.graphicsShaderLevel >= 50 AndAlso SystemInfo.supportsComputeShaders
			If Not SystemInfo.supportsImageEffects Then
				Me.NotSupported()
				Return False
			End If
			If needDepth AndAlso Not SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.Depth) Then
				Me.NotSupported()
				Return False
			End If
			If needDepth Then
				MyBase.GetComponent(Of Camera)().depthTextureMode = MyBase.GetComponent(Of Camera)().depthTextureMode Or DepthTextureMode.Depth
			End If
			Return True
		End Function

		' Token: 0x0600523F RID: 21055 RVA: 0x0008CBC3 File Offset: 0x0008AFC3
		Protected Function CheckSupport(needDepth As Boolean, needHdr As Boolean) As Boolean
			If Not Me.CheckSupport(needDepth) Then
				Return False
			End If
			If needHdr AndAlso Not Me.supportHDRTextures Then
				Me.NotSupported()
				Return False
			End If
			Return True
		End Function

		' Token: 0x06005240 RID: 21056 RVA: 0x0008CBED File Offset: 0x0008AFED
		Public Function Dx11Support() As Boolean
			Return Me.supportDX11
		End Function

		' Token: 0x06005241 RID: 21057 RVA: 0x0008CBF5 File Offset: 0x0008AFF5
		Protected Sub ReportAutoDisable()
		End Sub

		' Token: 0x06005242 RID: 21058 RVA: 0x0008CBF7 File Offset: 0x0008AFF7
		Private Function CheckShader(s As Shader) As Boolean
			If Not s.isSupported Then
				Me.NotSupported()
				Return False
			End If
			Return False
		End Function

		' Token: 0x06005243 RID: 21059 RVA: 0x0008CC0D File Offset: 0x0008B00D
		Protected Sub NotSupported()
			MyBase.enabled = False
			Me.isSupported = False
		End Sub

		' Token: 0x06005244 RID: 21060 RVA: 0x0008CC20 File Offset: 0x0008B020
		Protected Sub DrawBorder(dest As RenderTexture, material As Material)
			RenderTexture.active = dest
			Dim flag As Boolean = True
			GL.PushMatrix()
			GL.LoadOrtho()
			For i As Integer = 0 To material.passCount - 1
				material.SetPass(i)
				Dim num As Single
				Dim num2 As Single
				If flag Then
					num = 1F
					num2 = 0F
				Else
					num = 0F
					num2 = 1F
				End If
				Dim num3 As Single = 0F
				Dim num4 As Single = 1F / (CSng(dest.width) * 1F)
				Dim num5 As Single = 0F
				Dim num6 As Single = 1F
				GL.Begin(7)
				GL.TexCoord2(0F, num)
				GL.Vertex3(num3, num5, 0.1F)
				GL.TexCoord2(1F, num)
				GL.Vertex3(num4, num5, 0.1F)
				GL.TexCoord2(1F, num2)
				GL.Vertex3(num4, num6, 0.1F)
				GL.TexCoord2(0F, num2)
				GL.Vertex3(num3, num6, 0.1F)
				num3 = 1F - 1F / (CSng(dest.width) * 1F)
				num4 = 1F
				num5 = 0F
				num6 = 1F
				GL.TexCoord2(0F, num)
				GL.Vertex3(num3, num5, 0.1F)
				GL.TexCoord2(1F, num)
				GL.Vertex3(num4, num5, 0.1F)
				GL.TexCoord2(1F, num2)
				GL.Vertex3(num4, num6, 0.1F)
				GL.TexCoord2(0F, num2)
				GL.Vertex3(num3, num6, 0.1F)
				num3 = 0F
				num4 = 1F
				num5 = 0F
				num6 = 1F / (CSng(dest.height) * 1F)
				GL.TexCoord2(0F, num)
				GL.Vertex3(num3, num5, 0.1F)
				GL.TexCoord2(1F, num)
				GL.Vertex3(num4, num5, 0.1F)
				GL.TexCoord2(1F, num2)
				GL.Vertex3(num4, num6, 0.1F)
				GL.TexCoord2(0F, num2)
				GL.Vertex3(num3, num6, 0.1F)
				num3 = 0F
				num4 = 1F
				num5 = 1F - 1F / (CSng(dest.height) * 1F)
				num6 = 1F
				GL.TexCoord2(0F, num)
				GL.Vertex3(num3, num5, 0.1F)
				GL.TexCoord2(1F, num)
				GL.Vertex3(num4, num5, 0.1F)
				GL.TexCoord2(1F, num2)
				GL.Vertex3(num4, num6, 0.1F)
				GL.TexCoord2(0F, num2)
				GL.Vertex3(num3, num6, 0.1F)
				GL.[End]()
			Next
			GL.PopMatrix()
		End Sub

		' Token: 0x040056AD RID: 22189
		Protected supportHDRTextures As Boolean = True

		' Token: 0x040056AE RID: 22190
		Protected supportDX11 As Boolean

		' Token: 0x040056AF RID: 22191
		Protected isSupported As Boolean = True
	End Class
End Namespace
