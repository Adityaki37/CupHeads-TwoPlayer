Imports System
Imports UnityEngine

Namespace UnityStandardAssets.ImageEffects
	' Token: 0x02000CCD RID: 3277
	<ExecuteInEditMode()>
	<AddComponentMenu("Image Effects/Color Adjustments/Contrast Stretch")>
	Public Class ContrastStretch
		Inherits MonoBehaviour

		' Token: 0x170008A4 RID: 2212
		' (get) Token: 0x060051EC RID: 20972 RVA: 0x0029F0D1 File Offset: 0x0029D4D1
		Protected ReadOnly Property materialLum As Material
			Get
				If Me.m_materialLum Is Nothing Then
					Me.m_materialLum = New Material(Me.shaderLum)
					Me.m_materialLum.hideFlags = HideFlags.HideAndDontSave
				End If
				Return Me.m_materialLum
			End Get
		End Property

		' Token: 0x170008A5 RID: 2213
		' (get) Token: 0x060051ED RID: 20973 RVA: 0x0029F108 File Offset: 0x0029D508
		Protected ReadOnly Property materialReduce As Material
			Get
				If Me.m_materialReduce Is Nothing Then
					Me.m_materialReduce = New Material(Me.shaderReduce)
					Me.m_materialReduce.hideFlags = HideFlags.HideAndDontSave
				End If
				Return Me.m_materialReduce
			End Get
		End Property

		' Token: 0x170008A6 RID: 2214
		' (get) Token: 0x060051EE RID: 20974 RVA: 0x0029F13F File Offset: 0x0029D53F
		Protected ReadOnly Property materialAdapt As Material
			Get
				If Me.m_materialAdapt Is Nothing Then
					Me.m_materialAdapt = New Material(Me.shaderAdapt)
					Me.m_materialAdapt.hideFlags = HideFlags.HideAndDontSave
				End If
				Return Me.m_materialAdapt
			End Get
		End Property

		' Token: 0x170008A7 RID: 2215
		' (get) Token: 0x060051EF RID: 20975 RVA: 0x0029F176 File Offset: 0x0029D576
		Protected ReadOnly Property materialApply As Material
			Get
				If Me.m_materialApply Is Nothing Then
					Me.m_materialApply = New Material(Me.shaderApply)
					Me.m_materialApply.hideFlags = HideFlags.HideAndDontSave
				End If
				Return Me.m_materialApply
			End Get
		End Property

		' Token: 0x060051F0 RID: 20976 RVA: 0x0029F1B0 File Offset: 0x0029D5B0
		Private Sub Start()
			If Not SystemInfo.supportsImageEffects Then
				MyBase.enabled = False
				Return
			End If
			If Not Me.shaderAdapt.isSupported OrElse Not Me.shaderApply.isSupported OrElse Not Me.shaderLum.isSupported OrElse Not Me.shaderReduce.isSupported Then
				MyBase.enabled = False
				Return
			End If
		End Sub

		' Token: 0x060051F1 RID: 20977 RVA: 0x0029F218 File Offset: 0x0029D618
		Private Sub OnEnable()
			For i As Integer = 0 To 2 - 1
				If Not Me.adaptRenderTex(i) Then
					Me.adaptRenderTex(i) = New RenderTexture(1, 1, 0)
					Me.adaptRenderTex(i).hideFlags = HideFlags.HideAndDontSave
				End If
			Next
		End Sub

		' Token: 0x060051F2 RID: 20978 RVA: 0x0029F268 File Offset: 0x0029D668
		Private Sub OnDisable()
			For i As Integer = 0 To 2 - 1
				Global.UnityEngine.[Object].DestroyImmediate(Me.adaptRenderTex(i))
				Me.adaptRenderTex(i) = Nothing
			Next
			If Me.m_materialLum Then
				Global.UnityEngine.[Object].DestroyImmediate(Me.m_materialLum)
			End If
			If Me.m_materialReduce Then
				Global.UnityEngine.[Object].DestroyImmediate(Me.m_materialReduce)
			End If
			If Me.m_materialAdapt Then
				Global.UnityEngine.[Object].DestroyImmediate(Me.m_materialAdapt)
			End If
			If Me.m_materialApply Then
				Global.UnityEngine.[Object].DestroyImmediate(Me.m_materialApply)
			End If
		End Sub

		' Token: 0x060051F3 RID: 20979 RVA: 0x0029F30C File Offset: 0x0029D70C
		Private Sub OnRenderImage(source As RenderTexture, destination As RenderTexture)
			Dim renderTexture As RenderTexture = RenderTexture.GetTemporary(source.width, source.height)
			Graphics.Blit(source, renderTexture, Me.materialLum)
			While renderTexture.width > 1 OrElse renderTexture.height > 1
				Dim num As Integer = renderTexture.width / 2
				If num < 1 Then
					num = 1
				End If
				Dim num2 As Integer = renderTexture.height / 2
				If num2 < 1 Then
					num2 = 1
				End If
				Dim temporary As RenderTexture = RenderTexture.GetTemporary(num, num2)
				Graphics.Blit(renderTexture, temporary, Me.materialReduce)
				RenderTexture.ReleaseTemporary(renderTexture)
				renderTexture = temporary
			End While
			Me.CalculateAdaptation(renderTexture)
			Me.materialApply.SetTexture("_AdaptTex", Me.adaptRenderTex(Me.curAdaptIndex))
			Graphics.Blit(source, destination, Me.materialApply)
			RenderTexture.ReleaseTemporary(renderTexture)
		End Sub

		' Token: 0x060051F4 RID: 20980 RVA: 0x0029F3D0 File Offset: 0x0029D7D0
		Private Sub CalculateAdaptation(curTexture As Texture)
			Dim num As Integer = Me.curAdaptIndex
			Me.curAdaptIndex = (Me.curAdaptIndex + 1) Mod 2
			Dim num2 As Single = 1F - Mathf.Pow(1F - Me.adaptationSpeed, 30F * Time.deltaTime)
			num2 = Mathf.Clamp(num2, 0.01F, 1F)
			Me.materialAdapt.SetTexture("_CurTex", curTexture)
			Me.materialAdapt.SetVector("_AdaptParams", New Vector4(num2, Me.limitMinimum, Me.limitMaximum, 0F))
			Graphics.SetRenderTarget(Me.adaptRenderTex(Me.curAdaptIndex))
			GL.Clear(False, True, Color.black)
			Graphics.Blit(Me.adaptRenderTex(num), Me.adaptRenderTex(Me.curAdaptIndex), Me.materialAdapt)
		End Sub

		' Token: 0x040055F6 RID: 22006
		Public adaptationSpeed As Single = 0.02F

		' Token: 0x040055F7 RID: 22007
		Public limitMinimum As Single = 0.2F

		' Token: 0x040055F8 RID: 22008
		Public limitMaximum As Single = 0.6F

		' Token: 0x040055F9 RID: 22009
		Private adaptRenderTex As RenderTexture() = New RenderTexture(1) {}

		' Token: 0x040055FA RID: 22010
		Private curAdaptIndex As Integer

		' Token: 0x040055FB RID: 22011
		Public shaderLum As Shader

		' Token: 0x040055FC RID: 22012
		Private m_materialLum As Material

		' Token: 0x040055FD RID: 22013
		Public shaderReduce As Shader

		' Token: 0x040055FE RID: 22014
		Private m_materialReduce As Material

		' Token: 0x040055FF RID: 22015
		Public shaderAdapt As Shader

		' Token: 0x04005600 RID: 22016
		Private m_materialAdapt As Material

		' Token: 0x04005601 RID: 22017
		Public shaderApply As Shader

		' Token: 0x04005602 RID: 22018
		Private m_materialApply As Material
	End Class
End Namespace
