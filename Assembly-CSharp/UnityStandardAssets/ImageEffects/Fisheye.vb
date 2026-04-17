Imports System
Imports UnityEngine

Namespace UnityStandardAssets.ImageEffects
	' Token: 0x02000CD9 RID: 3289
	<ExecuteInEditMode()>
	<RequireComponent(GetType(Camera))>
	<AddComponentMenu("Image Effects/Displacement/Fisheye")>
	Friend Class Fisheye
		Inherits PostEffectsBase

		' Token: 0x06005219 RID: 21017 RVA: 0x002A18FA File Offset: 0x0029FCFA
		Public Overrides Function CheckResources() As Boolean
			MyBase.CheckSupport(False)
			Me.fisheyeMaterial = MyBase.CheckShaderAndCreateMaterial(Me.fishEyeShader, Me.fisheyeMaterial)
			If Not Me.isSupported Then
				MyBase.ReportAutoDisable()
			End If
			Return Me.isSupported
		End Function

		' Token: 0x0600521A RID: 21018 RVA: 0x002A1934 File Offset: 0x0029FD34
		Private Sub OnRenderImage(source As RenderTexture, destination As RenderTexture)
			If Not Me.CheckResources() Then
				Graphics.Blit(source, destination)
				Return
			End If
			Dim num As Single = 0.15625F
			Dim num2 As Single = CSng(source.width) * 1F / (CSng(source.height) * 1F)
			Me.fisheyeMaterial.SetVector("intensity", New Vector4(Me.strengthX * num2 * num, Me.strengthY * num, Me.strengthX * num2 * num, Me.strengthY * num))
			Graphics.Blit(source, destination, Me.fisheyeMaterial)
		End Sub

		' Token: 0x04005676 RID: 22134
		Public strengthX As Single = 0.05F

		' Token: 0x04005677 RID: 22135
		Public strengthY As Single = 0.05F

		' Token: 0x04005678 RID: 22136
		Public fishEyeShader As Shader

		' Token: 0x04005679 RID: 22137
		Private fisheyeMaterial As Material
	End Class
End Namespace
