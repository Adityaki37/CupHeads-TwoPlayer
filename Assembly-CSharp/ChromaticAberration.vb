Imports System
Imports UnityEngine

' Token: 0x020003E1 RID: 993
<ExecuteInEditMode()>
Public Class ChromaticAberration
	Inherits MonoBehaviour

	' Token: 0x1700023F RID: 575
	' (get) Token: 0x06000D4F RID: 3407 RVA: 0x0008D085 File Offset: 0x0008B485
	Private ReadOnly Property material As Material
		Get
			If Me.curMaterial Is Nothing Then
				Me.curMaterial = New Material(Me.shader)
				Me.curMaterial.hideFlags = HideFlags.HideAndDontSave
			End If
			Return Me.curMaterial
		End Get
	End Property

	' Token: 0x06000D50 RID: 3408 RVA: 0x0008D0BC File Offset: 0x0008B4BC
	Protected Overridable Sub Start()
		If Not SystemInfo.supportsImageEffects Then
			MyBase.enabled = False
			Return
		End If
	End Sub

	' Token: 0x06000D51 RID: 3409 RVA: 0x0008D0D0 File Offset: 0x0008B4D0
	Protected Overridable Sub OnRenderImage(sourceTexture As RenderTexture, destTexture As RenderTexture)
		If Me.shader IsNot Nothing Then
			Dim num As Single = CSng(destTexture.width) / CSng(destTexture.height)
			Dim num2 As Single = If((num >= 1.7777778F), 1F, (num / 1.7777778F))
			num2 *= 1F - 0.1F * SettingsData.Data.overscan
			Dim num3 As Single = num2 * CSng(destTexture.height) / 1080F
			Me.material.SetVector("_Screen", New Vector2(CSng(destTexture.width), CSng(destTexture.height)))
			Me.material.SetVector("_Red", Me.r * num3)
			Me.material.SetVector("_Green", Me.g * num3)
			Me.material.SetVector("_Blue", Me.b * num3)
			Graphics.Blit(sourceTexture, destTexture, Me.material)
		Else
			Graphics.Blit(sourceTexture, destTexture)
		End If
	End Sub

	' Token: 0x06000D52 RID: 3410 RVA: 0x0008D1E8 File Offset: 0x0008B5E8
	Protected Overridable Sub OnDisable()
		If Me.curMaterial Then
			Global.UnityEngine.[Object].DestroyImmediate(Me.curMaterial)
		End If
	End Sub

	' Token: 0x040016C7 RID: 5831
	Public shader As Shader

	' Token: 0x040016C8 RID: 5832
	Public r As Vector2

	' Token: 0x040016C9 RID: 5833
	Public g As Vector2

	' Token: 0x040016CA RID: 5834
	Public b As Vector2

	' Token: 0x040016CB RID: 5835
	Private curMaterial As Material
End Class
