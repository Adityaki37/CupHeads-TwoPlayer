Imports System
Imports UnityEngine

' Token: 0x02000658 RID: 1624
Public Class FlyingCowboyLevelOverlayToggle
	Inherits MonoBehaviour

	' Token: 0x060021DA RID: 8666 RVA: 0x0013B7B4 File Offset: 0x00139BB4
	Private Sub Start()
		Dim component As SpriteRenderer = MyBase.GetComponent(Of SpriteRenderer)()
		For Each spriteRenderer As SpriteRenderer In Me.overlayRenderers
			If Global.UnityEngine.Random.value < Me.overlayProbability Then
				spriteRenderer.enabled = True
				spriteRenderer.sortingOrder = component.sortingOrder + 1
			Else
				spriteRenderer.enabled = False
			End If
		Next
	End Sub

	' Token: 0x04002A92 RID: 10898
	<SerializeField()>
	<Range(0F, 1F)>
	Private overlayProbability As Single

	' Token: 0x04002A93 RID: 10899
	<SerializeField()>
	Private overlayRenderers As SpriteRenderer()
End Class
