Imports System
Imports UnityEngine

Namespace UnityStandardAssets.ImageEffects
	' Token: 0x02000CC3 RID: 3267
	<ExecuteInEditMode()>
	<AddComponentMenu("Image Effects/Blur/Blur")>
	Public Class Blur
		Inherits MonoBehaviour

		' Token: 0x170008A3 RID: 2211
		' (get) Token: 0x060051C0 RID: 20928 RVA: 0x0029D1BC File Offset: 0x0029B5BC
		Protected ReadOnly Property material As Material
			Get
				If Blur.m_Material Is Nothing Then
					Blur.m_Material = New Material(Me.blurShader)
					Blur.m_Material.hideFlags = HideFlags.DontSave
				End If
				Return Blur.m_Material
			End Get
		End Property

		' Token: 0x060051C1 RID: 20929 RVA: 0x0029D1EF File Offset: 0x0029B5EF
		Protected Sub OnDisable()
			If Blur.m_Material Then
				Global.UnityEngine.[Object].DestroyImmediate(Blur.m_Material)
			End If
		End Sub

		' Token: 0x060051C2 RID: 20930 RVA: 0x0029D20C File Offset: 0x0029B60C
		Protected Sub Start()
			If Not SystemInfo.supportsImageEffects Then
				MyBase.enabled = False
				Return
			End If
			If Not Me.blurShader OrElse Not Me.material.shader.isSupported Then
				MyBase.enabled = False
				Return
			End If
		End Sub

		' Token: 0x060051C3 RID: 20931 RVA: 0x0029D258 File Offset: 0x0029B658
		Public Sub FourTapCone(source As RenderTexture, dest As RenderTexture, iteration As Integer)
			Dim num As Single = 0.5F + CSng(iteration) * Me.blurSpread
			Graphics.BlitMultiTap(source, dest, Me.material, New Vector2() { New Vector2(-num, -num), New Vector2(-num, num), New Vector2(num, num), New Vector2(num, -num) })
		End Sub

		' Token: 0x060051C4 RID: 20932 RVA: 0x0029D2D8 File Offset: 0x0029B6D8
		Private Sub DownSample4x(source As RenderTexture, dest As RenderTexture)
			Dim num As Single = 1F
			Graphics.BlitMultiTap(source, dest, Me.material, New Vector2() { New Vector2(-num, -num), New Vector2(-num, num), New Vector2(num, num), New Vector2(num, -num) })
		End Sub

		' Token: 0x060051C5 RID: 20933 RVA: 0x0029D350 File Offset: 0x0029B750
		Private Sub OnRenderImage(source As RenderTexture, destination As RenderTexture)
			Dim num As Integer = source.width / 4
			Dim num2 As Integer = source.height / 4
			Dim renderTexture As RenderTexture = RenderTexture.GetTemporary(num, num2, 0)
			Me.DownSample4x(source, renderTexture)
			For i As Integer = 0 To Me.iterations - 1
				Dim temporary As RenderTexture = RenderTexture.GetTemporary(num, num2, 0)
				Me.FourTapCone(renderTexture, temporary, i)
				RenderTexture.ReleaseTemporary(renderTexture)
				renderTexture = temporary
			Next
			Graphics.Blit(renderTexture, destination)
			RenderTexture.ReleaseTemporary(renderTexture)
		End Sub

		' Token: 0x0400559E RID: 21918
		Public iterations As Integer = 3

		' Token: 0x0400559F RID: 21919
		Public blurSpread As Single = 0.6F

		' Token: 0x040055A0 RID: 21920
		Public blurShader As Shader

		' Token: 0x040055A1 RID: 21921
		Private Shared m_Material As Material
	End Class
End Namespace
