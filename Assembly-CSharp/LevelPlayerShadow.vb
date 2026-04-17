Imports System
Imports UnityEngine

' Token: 0x02000A38 RID: 2616
Public Class LevelPlayerShadow
	Inherits AbstractLevelPlayerComponent

	' Token: 0x06003E43 RID: 15939 RVA: 0x00223CA0 File Offset: 0x002220A0
	Private Sub Start()
		Me.shadow = New GameObject(MyBase.gameObject.name + "_Shadow").transform
		Me.spriteRenderer = Me.shadow.gameObject.AddComponent(Of SpriteRenderer)()
		Me.shadow.position = New Vector3(MyBase.transform.position.x, CSng(Level.Current.Ground), 0F)
		Me.spriteRenderer.sprite = Me.shadowSprites(0)
		If Level.Current IsNot Nothing Then
			Me.spriteRenderer.sortingOrder = Level.Current.playerShadowSortingOrder
		End If
		If SceneLoader.CurrentLevel = Levels.ChaliceTutorial Then
			Me.spriteRenderer.gameObject.layer = 31
		End If
	End Sub

	' Token: 0x06003E44 RID: 15940 RVA: 0x00223D74 File Offset: 0x00222174
	Private Sub Update()
		If(MyBase.player.motor.Grounded AndAlso Not MyBase.player.motor.Dashing) OrElse MyBase.player.IsDead OrElse ((MyBase.player.stats.Loadout.charm = Charm.charm_smoke_dash OrElse MyBase.player.stats.CurseSmokeDash) AndAlso Not Level.IsChessBoss AndAlso MyBase.player.motor.Dashing) Then
			Me.spriteRenderer.enabled = False
			Return
		End If
		Me.spriteRenderer.enabled = True
		Dim position As Vector3 = Me.shadow.position
		position.x = MyBase.transform.position.x
		Dim collider As BoxCollider2D = MyBase.player.collider
		Dim raycastHit2D As RaycastHit2D = Physics2D.BoxCast(MyBase.player.transform.position, New Vector2(MyBase.player.collider.size.x, 1F), 0F, If((Not MyBase.player.motor.GravityReversed), Vector2.down, Vector2.up), CSng(Me.maxDistance), If((Not MyBase.player.motor.GravityReversed), Me.groundMask, Me.ceilingMask))
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

	' Token: 0x06003E45 RID: 15941 RVA: 0x00223F70 File Offset: 0x00222370
	Private Sub SetSprite()
		Dim num As Integer = CInt((Mathf.Abs(MyBase.transform.position.y - Me.shadow.position.y) / CSng(Me.maxDistance) * CSng(Me.shadowSprites.Length)))
		If num < 0 OrElse num >= Me.shadowSprites.Length Then
			Me.spriteRenderer.enabled = False
			Return
		End If
		Me.spriteRenderer.enabled = True
		Me.spriteRenderer.sprite = Me.shadowSprites(num)
	End Sub

	' Token: 0x06003E46 RID: 15942 RVA: 0x00223FFE File Offset: 0x002223FE
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		If Me.shadow IsNot Nothing Then
			Global.UnityEngine.[Object].Destroy(Me.shadow.gameObject)
		End If
	End Sub

	' Token: 0x06003E47 RID: 15943 RVA: 0x00224027 File Offset: 0x00222427
	Public Function ShadowPosition() As Vector3
		Return Me.shadow.position
	End Function

	' Token: 0x04004567 RID: 17767
	<Range(1F, 1000F)>
	<SerializeField()>
	Private maxDistance As Integer = 250

	' Token: 0x04004568 RID: 17768
	<SerializeField()>
	Private shadowSprites As Sprite()

	' Token: 0x04004569 RID: 17769
	Private shadow As Transform

	' Token: 0x0400456A RID: 17770
	Private spriteRenderer As SpriteRenderer

	' Token: 0x0400456B RID: 17771
	Private groundMask As Integer = 1048576

	' Token: 0x0400456C RID: 17772
	Private ceilingMask As Integer = 524288
End Class
