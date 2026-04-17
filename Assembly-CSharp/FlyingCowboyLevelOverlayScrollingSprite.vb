Imports System
Imports UnityEngine

' Token: 0x02000657 RID: 1623
Public Class FlyingCowboyLevelOverlayScrollingSprite
	Inherits ScrollingSprite

	' Token: 0x060021D5 RID: 8661 RVA: 0x0013B5C8 File Offset: 0x001399C8
	Protected Overrides Sub Start()
		MyBase.Start()
		Me.leftRenderer = MyBase.GetComponent(Of SpriteRenderer)()
		Me.rightRenderer = MyBase.copyRenderers.Find(Function(renderer As SpriteRenderer) renderer.transform.position.x > Me.leftRenderer.transform.position.x)
		Me.rightOverlayRenderers = New SpriteRenderer(Me.overlayRenderers.Length - 1) {}
		For i As Integer = 0 To Me.overlayRenderers.Length - 1
			Dim spriteRenderer As SpriteRenderer = Me.overlayRenderers(i)
			Dim gameObject As GameObject = New GameObject(spriteRenderer.gameObject.name)
			Dim spriteRenderer2 As SpriteRenderer = gameObject.AddComponent(Of SpriteRenderer)()
			spriteRenderer2.sprite = spriteRenderer.sprite
			spriteRenderer2.sortingLayerID = spriteRenderer.sortingLayerID
			spriteRenderer2.sortingOrder = spriteRenderer.sortingOrder
			spriteRenderer2.enabled = False
			gameObject.transform.SetParent(Me.rightRenderer.transform, False)
			Me.rightOverlayRenderers(i) = spriteRenderer2
		Next
		Me.leftOverlaysEnabled = New Boolean(Me.overlayRenderers.Length - 1) {}
		Me.rightOverlaysEnabled = New Boolean(Me.overlayRenderers.Length - 1) {}
	End Sub

	' Token: 0x060021D6 RID: 8662 RVA: 0x0013B6C4 File Offset: 0x00139AC4
	Protected Overrides Sub onLoop()
		MyBase.onLoop()
		Dim array As Boolean() = Me.leftOverlaysEnabled
		Me.leftOverlaysEnabled = Me.rightOverlaysEnabled
		Me.rightOverlaysEnabled = array
		For i As Integer = 0 To Me.rightOverlaysEnabled.Length - 1
			Me.rightOverlaysEnabled(i) = Global.UnityEngine.Random.value < Me.overlayProbability
		Next
		FlyingCowboyLevelOverlayScrollingSprite.toggleOverlays(Me.overlayRenderers, Me.leftOverlaysEnabled)
		FlyingCowboyLevelOverlayScrollingSprite.toggleOverlays(Me.rightOverlayRenderers, Me.rightOverlaysEnabled)
	End Sub

	' Token: 0x060021D7 RID: 8663 RVA: 0x0013B744 File Offset: 0x00139B44
	Private Shared Sub toggleOverlays(overlayRenderers As SpriteRenderer(), activeStatus As Boolean())
		For i As Integer = 0 To overlayRenderers.Length - 1
			overlayRenderers(i).enabled = activeStatus(i)
		Next
	End Sub

	' Token: 0x04002A8B RID: 10891
	<SerializeField()>
	<Range(0F, 1F)>
	Private overlayProbability As Single

	' Token: 0x04002A8C RID: 10892
	<SerializeField()>
	Private overlayRenderers As SpriteRenderer()

	' Token: 0x04002A8D RID: 10893
	Private leftRenderer As SpriteRenderer

	' Token: 0x04002A8E RID: 10894
	Private rightRenderer As SpriteRenderer

	' Token: 0x04002A8F RID: 10895
	Private rightOverlayRenderers As SpriteRenderer()

	' Token: 0x04002A90 RID: 10896
	Private leftOverlaysEnabled As Boolean()

	' Token: 0x04002A91 RID: 10897
	Private rightOverlaysEnabled As Boolean()
End Class
