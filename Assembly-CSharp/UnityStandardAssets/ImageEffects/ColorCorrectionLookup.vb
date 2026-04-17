Imports System
Imports UnityEngine

Namespace UnityStandardAssets.ImageEffects
	' Token: 0x02000CCA RID: 3274
	<ExecuteInEditMode()>
	<AddComponentMenu("Image Effects/Color Adjustments/Color Correction (3D Lookup Texture)")>
	Public Class ColorCorrectionLookup
		Inherits PostEffectsBase

		' Token: 0x060051DF RID: 20959 RVA: 0x0029EA3C File Offset: 0x0029CE3C
		Public Overrides Function CheckResources() As Boolean
			MyBase.CheckSupport(False)
			Me.material = MyBase.CheckShaderAndCreateMaterial(Me.shader, Me.material)
			If Not Me.isSupported OrElse Not SystemInfo.supports3DTextures Then
				MyBase.ReportAutoDisable()
			End If
			Return Me.isSupported
		End Function

		' Token: 0x060051E0 RID: 20960 RVA: 0x0029EA8A File Offset: 0x0029CE8A
		Private Sub OnDisable()
			If Me.material Then
				Global.UnityEngine.[Object].DestroyImmediate(Me.material)
				Me.material = Nothing
			End If
		End Sub

		' Token: 0x060051E1 RID: 20961 RVA: 0x0029EAAE File Offset: 0x0029CEAE
		Private Sub OnDestroy()
			If Me.converted3DLut Then
				Global.UnityEngine.[Object].DestroyImmediate(Me.converted3DLut)
			End If
			Me.converted3DLut = Nothing
		End Sub

		' Token: 0x060051E2 RID: 20962 RVA: 0x0029EAD4 File Offset: 0x0029CED4
		Public Sub SetIdentityLut()
			Dim num As Integer = 16
			Dim array As Color() = New Color(num * num * num - 1) {}
			Dim num2 As Single = 1F / (1F * CSng(num) - 1F)
			For i As Integer = 0 To num - 1
				For j As Integer = 0 To num - 1
					For k As Integer = 0 To num - 1
						array(i + j * num + k * num * num) = New Color(CSng(i) * 1F * num2, CSng(j) * 1F * num2, CSng(k) * 1F * num2, 1F)
					Next
				Next
			Next
			If Me.converted3DLut Then
				Global.UnityEngine.[Object].DestroyImmediate(Me.converted3DLut)
			End If
			Me.converted3DLut = New Texture3D(num, num, num, TextureFormat.ARGB32, False)
			Me.converted3DLut.SetPixels(array)
			Me.converted3DLut.Apply()
			Me.basedOnTempTex = String.Empty
		End Sub

		' Token: 0x060051E3 RID: 20963 RVA: 0x0029EBD4 File Offset: 0x0029CFD4
		Public Function ValidDimensions(tex2d As Texture2D) As Boolean
			If Not tex2d Then
				Return False
			End If
			Dim height As Integer = tex2d.height
			Return height = Mathf.FloorToInt(Mathf.Sqrt(CSng(tex2d.width)))
		End Function

		' Token: 0x060051E4 RID: 20964 RVA: 0x0029EC10 File Offset: 0x0029D010
		Public Sub Convert(temp2DTex As Texture2D, path As String)
			If temp2DTex Then
				Dim num As Integer = temp2DTex.width * temp2DTex.height
				num = temp2DTex.height
				If Not Me.ValidDimensions(temp2DTex) Then
					Me.basedOnTempTex = String.Empty
					Return
				End If
				Dim pixels As Color() = temp2DTex.GetPixels()
				Dim array As Color() = New Color(pixels.Length - 1) {}
				For i As Integer = 0 To num - 1
					For j As Integer = 0 To num - 1
						For k As Integer = 0 To num - 1
							Dim num2 As Integer = num - j - 1
							array(i + j * num + k * num * num) = pixels(k * num + i + num2 * num * num)
						Next
					Next
				Next
				If Me.converted3DLut Then
					Global.UnityEngine.[Object].DestroyImmediate(Me.converted3DLut)
				End If
				Me.converted3DLut = New Texture3D(num, num, num, TextureFormat.ARGB32, False)
				Me.converted3DLut.SetPixels(array)
				Me.converted3DLut.Apply()
				Me.basedOnTempTex = path
			Else
				Global.Debug.LogError("Couldn't color correct with 3D LUT texture. Image Effect will be disabled.", Nothing)
			End If
		End Sub

		' Token: 0x060051E5 RID: 20965 RVA: 0x0029ED34 File Offset: 0x0029D134
		Private Sub OnRenderImage(source As RenderTexture, destination As RenderTexture)
			If Not Me.CheckResources() OrElse Not SystemInfo.supports3DTextures Then
				Graphics.Blit(source, destination)
				Return
			End If
			If Me.converted3DLut Is Nothing Then
				Me.SetIdentityLut()
			End If
			Dim width As Integer = Me.converted3DLut.width
			Me.converted3DLut.wrapMode = TextureWrapMode.Clamp
			Me.material.SetFloat("_Scale", CSng((width - 1)) / (1F * CSng(width)))
			Me.material.SetFloat("_Offset", 1F / (2F * CSng(width)))
			Me.material.SetTexture("_ClutTex", Me.converted3DLut)
			Graphics.Blit(source, destination, Me.material, If((QualitySettings.activeColorSpace <> ColorSpace.Linear), 0, 1))
		End Sub

		' Token: 0x040055EA RID: 21994
		Public shader As Shader

		' Token: 0x040055EB RID: 21995
		Private material As Material

		' Token: 0x040055EC RID: 21996
		Public converted3DLut As Texture3D

		' Token: 0x040055ED RID: 21997
		Public basedOnTempTex As String = String.Empty
	End Class
End Namespace
