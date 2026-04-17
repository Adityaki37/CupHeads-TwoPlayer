Imports System
Imports UnityEngine

' Token: 0x02000791 RID: 1937
Public Class RumRunnersLevelLaserMask
	Inherits MonoBehaviour

	' Token: 0x06002AEF RID: 10991 RVA: 0x00190B78 File Offset: 0x0018EF78
	Public Sub Setup(layerID As Integer, lowestLayerOrder As Integer)
		For Each spriteRenderer As SpriteRenderer In Me.maskRenderers
			spriteRenderer.sortingLayerID = layerID
			spriteRenderer.sortingOrder = lowestLayerOrder - 1
		Next
		For Each spriteRenderer2 As SpriteRenderer In Me.clearRenderers
			spriteRenderer2.sortingLayerID = layerID
			spriteRenderer2.sortingOrder = lowestLayerOrder + 4
		Next
	End Sub

	' Token: 0x040033A4 RID: 13220
	<SerializeField()>
	Private maskRenderers As SpriteRenderer()

	' Token: 0x040033A5 RID: 13221
	<SerializeField()>
	Private clearRenderers As SpriteRenderer()
End Class
