Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000662 RID: 1634
Public Class FlyingGenieLevelBackgroundShade
	Inherits AbstractPausableComponent

	' Token: 0x06002208 RID: 8712 RVA: 0x0013D0AA File Offset: 0x0013B4AA
	Private Sub Start()
		MyBase.FrameDelayedCallback(AddressOf Me.GetSprites, 1)
	End Sub

	' Token: 0x06002209 RID: 8713 RVA: 0x0013D0C0 File Offset: 0x0013B4C0
	Private Sub GetSprites()
		Me.darkClones = Me.darkSprite.gameObject.transform.GetComponentsInChildren(Of SpriteRenderer)()
		MyBase.StartCoroutine(Me.fade_sprite_cr())
	End Sub

	' Token: 0x0600220A RID: 8714 RVA: 0x0013D0EC File Offset: 0x0013B4EC
	Private Iterator Function fade_sprite_cr() As IEnumerator
		While True
			Dim t As Single = FlyingGenieLevel.mainTimer
			Dim period As Single = 12F
			Dim shade As Single = (Mathf.Sin(t * 3.1415927F * 2F / period) + 1F) / 2F
			shade = Mathf.Lerp(Me.fullSunOpacity, Me.fullShadeOpactity, shade)
			For Each spriteRenderer As SpriteRenderer In Me.darkClones
				spriteRenderer.color = New Color(spriteRenderer.GetComponent(Of SpriteRenderer)().color.r, spriteRenderer.GetComponent(Of SpriteRenderer)().color.g, spriteRenderer.GetComponent(Of SpriteRenderer)().color.b, shade)
			Next
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x04002AB6 RID: 10934
	<SerializeField()>
	Private darkSprite As SpriteRenderer

	' Token: 0x04002AB7 RID: 10935
	Private darkClones As SpriteRenderer()

	' Token: 0x04002AB8 RID: 10936
	<SerializeField()>
	Private fullSunOpacity As Single

	' Token: 0x04002AB9 RID: 10937
	<SerializeField()>
	Private fullShadeOpactity As Single = 1F
End Class
