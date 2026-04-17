Imports System
Imports UnityEngine

Namespace UnityStandardAssets.ImageEffects
	' Token: 0x02000CE2 RID: 3298
	<ExecuteInEditMode()>
	<RequireComponent(GetType(Camera))>
	Friend Class PostEffectsHelper
		Inherits MonoBehaviour

		' Token: 0x06005246 RID: 21062 RVA: 0x002A2BE9 File Offset: 0x002A0FE9
		Private Sub OnRenderImage(source As RenderTexture, destination As RenderTexture)
		End Sub

		' Token: 0x06005247 RID: 21063 RVA: 0x002A2BEC File Offset: 0x002A0FEC
		Private Shared Sub DrawLowLevelPlaneAlignedWithCamera(dist As Single, source As RenderTexture, dest As RenderTexture, material As Material, cameraForProjectionMatrix As Camera)
			RenderTexture.active = dest
			material.SetTexture("_MainTex", source)
			Dim flag As Boolean = True
			GL.PushMatrix()
			GL.LoadIdentity()
			GL.LoadProjectionMatrix(cameraForProjectionMatrix.projectionMatrix)
			Dim num As Single = cameraForProjectionMatrix.fieldOfView * 0.5F * 0.017453292F
			Dim num2 As Single = Mathf.Cos(num) / Mathf.Sin(num)
			Dim aspect As Single = cameraForProjectionMatrix.aspect
			Dim num3 As Single = aspect / -num2
			Dim num4 As Single = aspect / num2
			Dim num5 As Single = 1F / -num2
			Dim num6 As Single = 1F / num2
			Dim num7 As Single = 1F
			num3 *= dist * num7
			num4 *= dist * num7
			num5 *= dist * num7
			num6 *= dist * num7
			Dim num8 As Single = -dist
			For i As Integer = 0 To material.passCount - 1
				material.SetPass(i)
				GL.Begin(7)
				Dim num9 As Single
				Dim num10 As Single
				If flag Then
					num9 = 1F
					num10 = 0F
				Else
					num9 = 0F
					num10 = 1F
				End If
				GL.TexCoord2(0F, num9)
				GL.Vertex3(num3, num5, num8)
				GL.TexCoord2(1F, num9)
				GL.Vertex3(num4, num5, num8)
				GL.TexCoord2(1F, num10)
				GL.Vertex3(num4, num6, num8)
				GL.TexCoord2(0F, num10)
				GL.Vertex3(num3, num6, num8)
				GL.[End]()
			Next
			GL.PopMatrix()
		End Sub

		' Token: 0x06005248 RID: 21064 RVA: 0x002A2D54 File Offset: 0x002A1154
		Private Shared Sub DrawBorder(dest As RenderTexture, material As Material)
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

		' Token: 0x06005249 RID: 21065 RVA: 0x002A2FF4 File Offset: 0x002A13F4
		Private Shared Sub DrawLowLevelQuad(x1 As Single, x2 As Single, y1 As Single, y2 As Single, source As RenderTexture, dest As RenderTexture, material As Material)
			RenderTexture.active = dest
			material.SetTexture("_MainTex", source)
			Dim flag As Boolean = True
			GL.PushMatrix()
			GL.LoadOrtho()
			For i As Integer = 0 To material.passCount - 1
				material.SetPass(i)
				GL.Begin(7)
				Dim num As Single
				Dim num2 As Single
				If flag Then
					num = 1F
					num2 = 0F
				Else
					num = 0F
					num2 = 1F
				End If
				GL.TexCoord2(0F, num)
				GL.Vertex3(x1, y1, 0.1F)
				GL.TexCoord2(1F, num)
				GL.Vertex3(x2, y1, 0.1F)
				GL.TexCoord2(1F, num2)
				GL.Vertex3(x2, y2, 0.1F)
				GL.TexCoord2(0F, num2)
				GL.Vertex3(x1, y2, 0.1F)
				GL.[End]()
			Next
			GL.PopMatrix()
		End Sub
	End Class
End Namespace
