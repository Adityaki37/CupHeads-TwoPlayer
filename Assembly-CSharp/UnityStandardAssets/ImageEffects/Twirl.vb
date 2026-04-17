Imports System
Imports UnityEngine

Namespace UnityStandardAssets.ImageEffects
	' Token: 0x02000CF4 RID: 3316
	<ExecuteInEditMode()>
	<AddComponentMenu("Image Effects/Displacement/Twirl")>
	Public Class Twirl
		Inherits ImageEffectBase

		' Token: 0x06005274 RID: 21108 RVA: 0x002A4CFD File Offset: 0x002A30FD
		Private Sub OnRenderImage(source As RenderTexture, destination As RenderTexture)
			ImageEffects.RenderDistortion(MyBase.material, source, destination, Me.angle, Me.center, Me.radius)
		End Sub

		' Token: 0x04005716 RID: 22294
		Public radius As Vector2 = New Vector2(0.3F, 0.3F)

		' Token: 0x04005717 RID: 22295
		Public angle As Single = 50F

		' Token: 0x04005718 RID: 22296
		Public center As Vector2 = New Vector2(0.5F, 0.5F)
	End Class
End Namespace
