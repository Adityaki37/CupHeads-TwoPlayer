Imports System
Imports UnityEngine

' Token: 0x02000438 RID: 1080
Public Class PlatformingLevelEnemyShadow
	Inherits AbstractCollidableObject

	' Token: 0x06000FDE RID: 4062 RVA: 0x0009D314 File Offset: 0x0009B714
	Private Sub Start()
		Me.shadow = New GameObject(MyBase.gameObject.name + "_Shadow").transform
		Me.spriteRenderer = Me.shadow.gameObject.AddComponent(Of SpriteRenderer)()
		Me.shadow.position = New Vector3(MyBase.transform.position.x, CSng(Level.Current.Ground), 0F)
		Me.spriteRenderer.sprite = Me.shadowSprites(0)
		Me.enemy = MyBase.GetComponent(Of PlatformingLevelGroundMovementEnemy)()
		Me.boxCollider = Me.enemy.GetComponent(Of BoxCollider2D)()
	End Sub

	' Token: 0x06000FDF RID: 4063 RVA: 0x0009D3C0 File Offset: 0x0009B7C0
	Private Sub Update()
		If Me.enemy.Grounded OrElse Me.enemy.Dead Then
			Me.spriteRenderer.enabled = False
			Return
		End If
		Me.spriteRenderer.enabled = True
		Dim position As Vector3 = Me.shadow.position
		position.x = MyBase.transform.position.x
		Dim raycastHit2D As RaycastHit2D = Physics2D.BoxCast(MyBase.transform.position, New Vector2(Me.boxCollider.size.x, 1F), 0F, Vector2.down, CSng(Me.maxDistance), Me.groundMask)
		If raycastHit2D.collider Is Nothing Then
			Me.spriteRenderer.enabled = False
			Return
		End If
		Dim component As LevelPlatform = raycastHit2D.collider.gameObject.GetComponent(Of LevelPlatform)()
		If component IsNot Nothing AndAlso Not component.AllowShadows Then
			Me.spriteRenderer.enabled = False
			Return
		End If
		position.y = raycastHit2D.point.y
		Me.shadow.position = position
		Me.SetSprite()
	End Sub

	' Token: 0x06000FE0 RID: 4064 RVA: 0x0009D4F8 File Offset: 0x0009B8F8
	Private Sub SetSprite()
		Dim num As Integer = CInt((Mathf.Abs(MyBase.transform.position.y - Me.shadow.position.y) / CSng(Me.maxDistance) * CSng(Me.shadowSprites.Length)))
		If num < 0 OrElse num >= Me.shadowSprites.Length Then
			Me.spriteRenderer.enabled = False
			Return
		End If
		Me.spriteRenderer.enabled = True
		Me.spriteRenderer.sprite = Me.shadowSprites(num)
	End Sub

	' Token: 0x06000FE1 RID: 4065 RVA: 0x0009D586 File Offset: 0x0009B986
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		If Me.shadow IsNot Nothing Then
			Global.UnityEngine.[Object].Destroy(Me.shadow.gameObject)
		End If
	End Sub

	' Token: 0x06000FE2 RID: 4066 RVA: 0x0009D5AF File Offset: 0x0009B9AF
	Public Function ShadowPosition() As Vector3
		Return Me.shadow.position
	End Function

	' Token: 0x04001975 RID: 6517
	<Range(1F, 1000F)>
	<SerializeField()>
	Private maxDistance As Integer = 250

	' Token: 0x04001976 RID: 6518
	<SerializeField()>
	Private shadowSprites As Sprite()

	' Token: 0x04001977 RID: 6519
	Private shadow As Transform

	' Token: 0x04001978 RID: 6520
	Private spriteRenderer As SpriteRenderer

	' Token: 0x04001979 RID: 6521
	Private enemy As PlatformingLevelGroundMovementEnemy

	' Token: 0x0400197A RID: 6522
	Private boxCollider As BoxCollider2D

	' Token: 0x0400197B RID: 6523
	Private groundMask As Integer = 1048576
End Class
