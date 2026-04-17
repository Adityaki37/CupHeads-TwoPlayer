Imports System
Imports UnityEngine

Namespace UnityStandardAssets.ImageEffects
	' Token: 0x02000CE9 RID: 3305
	<ExecuteInEditMode()>
	<AddComponentMenu("Image Effects/Color Adjustments/Sepia Tone")>
	Public Class SepiaTone
		Inherits ImageEffectBase

		' Token: 0x06005260 RID: 21088 RVA: 0x002A3CB2 File Offset: 0x002A20B2
		Private Sub OnRenderImage(source As RenderTexture, destination As RenderTexture)
			Graphics.Blit(source, destination, MyBase.material)
		End Sub
	End Class
End Namespace
