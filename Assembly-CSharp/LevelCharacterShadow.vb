Imports System
Imports UnityEngine

' Token: 0x0200049F RID: 1183
Public Class LevelCharacterShadow
	Inherits AbstractPausableComponent

	' Token: 0x06001345 RID: 4933 RVA: 0x000AA7C8 File Offset: 0x000A8BC8
	Private Sub Start()
		Me.shadow = New GameObject(MyBase.gameObject.name + "_Shadow").transform
		Me.spriteRenderer = Me.shadow.gameObject.AddComponent(Of SpriteRenderer)()
		Me.shadow.position = New Vector3(MyBase.transform.position.x, CSng(Level.Current.Ground), 0F)
		Me.spriteRenderer.sprite = Me.shadowSprites(0)
		If Me.isBGLayer Then
			Me.spriteRenderer.sortingLayerName = SpriteLayer.Background.ToString()
			Me.spriteRenderer.sortingOrder = 100
		End If
	End Sub

	' Token: 0x06001346 RID: 4934 RVA: 0x000AA888 File Offset: 0x000A8C88
	Private Sub Update()
		Dim position As Vector3 = Me.shadow.position
		position.x = MyBase.transform.position.x
		Dim component As Collider2D = Me.root.GetComponent(Of Collider2D)()
		Dim raycastHit2D As RaycastHit2D = Physics2D.BoxCast(Me.root.transform.position, New Vector2(component.bounds.size.x, 1F), 0F, Vector2.down, CSng(Me.maxDistance), Me.groundMask)
		If raycastHit2D.collider Is Nothing Then
			Me.spriteRenderer.enabled = False
			Return
		End If
		Dim component2 As LevelPlatform = raycastHit2D.collider.gameObject.GetComponent(Of LevelPlatform)()
		If component2 IsNot Nothing AndAlso Not component2.AllowShadows Then
			Me.spriteRenderer.enabled = False
			Return
		End If
		position.y = raycastHit2D.point.y
		Me.shadow.position = position
		Me.SetSprite()
	End Sub

	' Token: 0x06001347 RID: 4935 RVA: 0x000AA99C File Offset: 0x000A8D9C
	Private Sub SetSprite()
		Dim num As Integer = CInt((Mathf.Abs(MyBase.transform.position.y - Me.shadow.position.y) / CSng(Me.maxDistance) * CSng(Me.shadowSprites.Length)))
		If num < 0 OrElse num >= Me.shadowSprites.Length Then
			Me.spriteRenderer.enabled = False
			Return
		End If
		Me.spriteRenderer.enabled = True
		Me.spriteRenderer.sprite = Me.shadowSprites(num)
	End Sub

	' Token: 0x06001348 RID: 4936 RVA: 0x000AAA2A File Offset: 0x000A8E2A
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		If Me.shadow IsNot Nothing Then
			Global.UnityEngine.[Object].Destroy(Me.shadow.gameObject)
		End If
	End Sub

	' Token: 0x04001C69 RID: 7273
	<Range(1F, 1000F)>
	<SerializeField()>
	Private maxDistance As Integer = 250

	' Token: 0x04001C6A RID: 7274
	<SerializeField()>
	Private root As Transform

	' Token: 0x04001C6B RID: 7275
	<SerializeField()>
	Private shadowSprites As Sprite()

	' Token: 0x04001C6C RID: 7276
	<SerializeField()>
	Private isBGLayer As Boolean

	' Token: 0x04001C6D RID: 7277
	Private shadow As Transform

	' Token: 0x04001C6E RID: 7278
	Private spriteRenderer As SpriteRenderer

	' Token: 0x04001C6F RID: 7279
	Private groundMask As Integer = 1048576
End Class
