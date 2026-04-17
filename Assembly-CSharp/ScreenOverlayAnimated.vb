Imports System
Imports System.Collections
Imports UnityEngine
Imports UnityStandardAssets.ImageEffects

' Token: 0x020003E3 RID: 995
<ExecuteInEditMode()>
<RequireComponent(GetType(Camera))>
<AddComponentMenu("Image Effects/Other/Screen Overlay Animated")>
Public Class ScreenOverlayAnimated
	Inherits PostEffectsBase

	' Token: 0x06000D5C RID: 3420 RVA: 0x0008D94A File Offset: 0x0008BD4A
	Protected Overrides Sub Start()
		MyBase.StartCoroutine(Me.animate_cr())
	End Sub

	' Token: 0x06000D5D RID: 3421 RVA: 0x0008D95C File Offset: 0x0008BD5C
	Private Iterator Function animate_cr() As IEnumerator
		While True
			Yield New WaitForSeconds(0.025F)
			If Me.animated Then
				Me.currentTexture += 1
				If Me.currentTexture >= Me.textures.Length Then
					Me.currentTexture = 0
				End If
			End If
		End While
		Return
	End Function

	' Token: 0x06000D5E RID: 3422 RVA: 0x0008D977 File Offset: 0x0008BD77
	Public Overrides Function CheckResources() As Boolean
		MyBase.CheckSupport(False)
		Me.overlayMaterial = MyBase.CheckShaderAndCreateMaterial(Me.overlayShader, Me.overlayMaterial)
		If Not Me.isSupported Then
			MyBase.ReportAutoDisable()
		End If
		Return Me.isSupported
	End Function

	' Token: 0x06000D5F RID: 3423 RVA: 0x0008D9B0 File Offset: 0x0008BDB0
	Private Sub OnRenderImage(source As RenderTexture, destination As RenderTexture)
		If Not Me.CheckResources() Then
			Graphics.Blit(source, destination)
			Return
		End If
		Me.overlayMaterial.SetVector("_UV_Transform", Me.UV_Transform)
		Me.overlayMaterial.SetFloat("_Intensity", Me.intensity)
		If Me.textures IsNot Nothing AndAlso Me.textures.Length > Me.currentTexture AndAlso Me.textures(Me.currentTexture) IsNot Nothing Then
			Me.overlayMaterial.SetTexture("_Overlay", Me.textures(Me.currentTexture))
		End If
		Graphics.Blit(source, destination, Me.overlayMaterial, CInt(Me.blendMode))
	End Sub

	' Token: 0x040016DB RID: 5851
	Private Const FRAME_TIME As Single = 0.025F

	' Token: 0x040016DC RID: 5852
	Private UV_Transform As Vector4 = New Vector4(1F, 0F, 0F, 1F)

	' Token: 0x040016DD RID: 5853
	Public blendMode As ScreenOverlayAnimated.OverlayBlendMode = ScreenOverlayAnimated.OverlayBlendMode.Overlay

	' Token: 0x040016DE RID: 5854
	Public intensity As Single = 1F

	' Token: 0x040016DF RID: 5855
	Public animated As Boolean = True

	' Token: 0x040016E0 RID: 5856
	Public textures As Texture2D()

	' Token: 0x040016E1 RID: 5857
	Public overlayShader As Shader

	' Token: 0x040016E2 RID: 5858
	Private currentTexture As Integer

	' Token: 0x040016E3 RID: 5859
	Private overlayMaterial As Material

	' Token: 0x020003E4 RID: 996
	Public Enum OverlayBlendMode
		' Token: 0x040016E5 RID: 5861
		Additive
		' Token: 0x040016E6 RID: 5862
		ScreenBlend
		' Token: 0x040016E7 RID: 5863
		Multiply
		' Token: 0x040016E8 RID: 5864
		Overlay
		' Token: 0x040016E9 RID: 5865
		AlphaBlend
	End Enum
End Class
