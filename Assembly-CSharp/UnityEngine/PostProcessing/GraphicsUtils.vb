Imports System

Namespace UnityEngine.PostProcessing
	' Token: 0x02000C00 RID: 3072
	Public Module GraphicsUtils
		' Token: 0x17000693 RID: 1683
		' (get) Token: 0x06004954 RID: 18772 RVA: 0x00265475 File Offset: 0x00263875
		Public ReadOnly Property isLinearColorSpace As Boolean
			Get
				Return QualitySettings.activeColorSpace = ColorSpace.Linear
			End Get
		End Property

		' Token: 0x17000694 RID: 1684
		' (get) Token: 0x06004955 RID: 18773 RVA: 0x0026547F File Offset: 0x0026387F
		Public ReadOnly Property supportsDX11 As Boolean
			Get
				Return SystemInfo.graphicsShaderLevel >= 50 AndAlso SystemInfo.supportsComputeShaders
			End Get
		End Property

		' Token: 0x17000695 RID: 1685
		' (get) Token: 0x06004956 RID: 18774 RVA: 0x00265498 File Offset: 0x00263898
		Public ReadOnly Property whiteTexture As Texture2D
			Get
				If GraphicsUtils.s_WhiteTexture IsNot Nothing Then
					Return GraphicsUtils.s_WhiteTexture
				End If
				GraphicsUtils.s_WhiteTexture = New Texture2D(1, 1, TextureFormat.ARGB32, False)
				GraphicsUtils.s_WhiteTexture.SetPixel(0, 0, New Color(1F, 1F, 1F, 1F))
				GraphicsUtils.s_WhiteTexture.Apply()
				Return GraphicsUtils.s_WhiteTexture
			End Get
		End Property

		' Token: 0x17000696 RID: 1686
		' (get) Token: 0x06004957 RID: 18775 RVA: 0x00265500 File Offset: 0x00263900
		Public ReadOnly Property quad As Mesh
			Get
				If GraphicsUtils.s_Quad IsNot Nothing Then
					Return GraphicsUtils.s_Quad
				End If
				Dim array As Vector3() = New Vector3() { New Vector3(-1F, -1F, 0F), New Vector3(1F, 1F, 0F), New Vector3(1F, -1F, 0F), New Vector3(-1F, 1F, 0F) }
				Dim array2 As Vector2() = New Vector2() { New Vector2(0F, 0F), New Vector2(1F, 1F), New Vector2(1F, 0F), New Vector2(0F, 1F) }
				Dim array3 As Integer() = New Integer() { 0, 1, 2, 1, 0, 3 }
				GraphicsUtils.s_Quad = New Mesh() With { .vertices = array, .uv = array2, .triangles = array3 }
				GraphicsUtils.s_Quad.RecalculateNormals()
				GraphicsUtils.s_Quad.RecalculateBounds()
				Return GraphicsUtils.s_Quad
			End Get
		End Property

		' Token: 0x06004958 RID: 18776 RVA: 0x0026566C File Offset: 0x00263A6C
		Public Sub Blit(material As Material, pass As Integer)
			GL.PushMatrix()
			GL.LoadOrtho()
			material.SetPass(pass)
			GL.Begin(5)
			GL.TexCoord2(0F, 0F)
			GL.Vertex3(0F, 0F, 0.1F)
			GL.TexCoord2(1F, 0F)
			GL.Vertex3(1F, 0F, 0.1F)
			GL.TexCoord2(0F, 1F)
			GL.Vertex3(0F, 1F, 0.1F)
			GL.TexCoord2(1F, 1F)
			GL.Vertex3(1F, 1F, 0.1F)
			GL.[End]()
			GL.PopMatrix()
		End Sub

		' Token: 0x06004959 RID: 18777 RVA: 0x00265728 File Offset: 0x00263B28
		Public Sub ClearAndBlit(source As Texture, destination As RenderTexture, material As Material, pass As Integer, Optional clearColor As Boolean = True, Optional clearDepth As Boolean = False)
			Dim active As RenderTexture = RenderTexture.active
			RenderTexture.active = destination
			GL.Clear(False, clearColor, Color.clear)
			GL.PushMatrix()
			GL.LoadOrtho()
			material.SetTexture("_MainTex", source)
			material.SetPass(pass)
			GL.Begin(5)
			GL.TexCoord2(0F, 0F)
			GL.Vertex3(0F, 0F, 0.1F)
			GL.TexCoord2(1F, 0F)
			GL.Vertex3(1F, 0F, 0.1F)
			GL.TexCoord2(0F, 1F)
			GL.Vertex3(0F, 1F, 0.1F)
			GL.TexCoord2(1F, 1F)
			GL.Vertex3(1F, 1F, 0.1F)
			GL.[End]()
			GL.PopMatrix()
			RenderTexture.active = active
		End Sub

		' Token: 0x0600495A RID: 18778 RVA: 0x0026580E File Offset: 0x00263C0E
		Public Sub Destroy(obj As [Object])
			If obj IsNot Nothing Then
				[Object].Destroy(obj)
			End If
		End Sub

		' Token: 0x0600495B RID: 18779 RVA: 0x00265822 File Offset: 0x00263C22
		Public Sub Dispose()
			GraphicsUtils.Destroy(GraphicsUtils.s_Quad)
		End Sub

		' Token: 0x04004F79 RID: 20345
		Private s_WhiteTexture As Texture2D

		' Token: 0x04004F7A RID: 20346
		Private s_Quad As Mesh
	End Module
End Namespace
