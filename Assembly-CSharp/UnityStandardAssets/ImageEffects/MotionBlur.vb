Imports System
Imports UnityEngine

Namespace UnityStandardAssets.ImageEffects
	' Token: 0x02000CDE RID: 3294
	<ExecuteInEditMode()>
	<AddComponentMenu("Image Effects/Blur/Motion Blur (Color Accumulation)")>
	<RequireComponent(GetType(Camera))>
	Public Class MotionBlur
		Inherits ImageEffectBase

		' Token: 0x0600522A RID: 21034 RVA: 0x002A1FD5 File Offset: 0x002A03D5
		Protected Overrides Sub OnDisable()
			MyBase.OnDisable()
			Global.UnityEngine.[Object].DestroyImmediate(Me.accumTexture)
		End Sub

		' Token: 0x0600522B RID: 21035 RVA: 0x002A1FE8 File Offset: 0x002A03E8
		Private Sub OnRenderImage(source As RenderTexture, destination As RenderTexture)
			If Me.accumTexture Is Nothing OrElse Me.accumTexture.width <> source.width OrElse Me.accumTexture.height <> source.height Then
				Global.UnityEngine.[Object].DestroyImmediate(Me.accumTexture)
				Me.accumTexture = New RenderTexture(source.width, source.height, 0)
				Me.accumTexture.hideFlags = HideFlags.HideAndDontSave
				Graphics.Blit(source, Me.accumTexture)
			End If
			If Me.extraBlur Then
				Dim temporary As RenderTexture = RenderTexture.GetTemporary(source.width / 4, source.height / 4, 0)
				Me.accumTexture.MarkRestoreExpected()
				Graphics.Blit(Me.accumTexture, temporary)
				Graphics.Blit(temporary, Me.accumTexture)
				RenderTexture.ReleaseTemporary(temporary)
			End If
			Me.blurAmount = Mathf.Clamp(Me.blurAmount, 0F, 0.92F)
			MyBase.material.SetTexture("_MainTex", Me.accumTexture)
			MyBase.material.SetFloat("_AccumOrig", 1F - Me.blurAmount)
			Me.accumTexture.MarkRestoreExpected()
			Graphics.Blit(source, Me.accumTexture, MyBase.material)
			Graphics.Blit(Me.accumTexture, destination)
		End Sub

		' Token: 0x04005686 RID: 22150
		Public blurAmount As Single = 0.8F

		' Token: 0x04005687 RID: 22151
		Public extraBlur As Boolean

		' Token: 0x04005688 RID: 22152
		Private accumTexture As RenderTexture
	End Class
End Namespace
