Imports System
Imports UnityEngine

Namespace UnityStandardAssets.ImageEffects
	' Token: 0x02000CF7 RID: 3319
	<ExecuteInEditMode()>
	<AddComponentMenu("Image Effects/Displacement/Vortex")>
	Public Class Vortex
		Inherits ImageEffectBase

		' Token: 0x06005279 RID: 21113 RVA: 0x002A50CF File Offset: 0x002A34CF
		Private Sub OnRenderImage(source As RenderTexture, destination As RenderTexture)
			ImageEffects.RenderDistortion(MyBase.material, source, destination, Me.angle, Me.center, Me.radius)
		End Sub

		' Token: 0x0400572A RID: 22314
		Public radius As Vector2 = New Vector2(0.4F, 0.4F)

		' Token: 0x0400572B RID: 22315
		Public angle As Single = 50F

		' Token: 0x0400572C RID: 22316
		Public center As Vector2 = New Vector2(0.5F, 0.5F)
	End Class
End Namespace
