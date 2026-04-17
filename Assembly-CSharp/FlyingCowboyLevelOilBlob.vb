Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000656 RID: 1622
Public Class FlyingCowboyLevelOilBlob
	Inherits AbstractProjectile

	' Token: 0x060021CE RID: 8654 RVA: 0x0013B1FC File Offset: 0x001395FC
	Public Function Create(position As Vector3, finalYPosition As Single, snakeSpawnX As Single, properties As LevelProperties.FlyingCowboy.SnakeAttack, playSplatSFX As Boolean) As FlyingCowboyLevelOilBlob
		Dim flyingCowboyLevelOilBlob As FlyingCowboyLevelOilBlob = TryCast(MyBase.Create(position), FlyingCowboyLevelOilBlob)
		flyingCowboyLevelOilBlob.initialYPosition = position.y
		flyingCowboyLevelOilBlob.finalYPosition = finalYPosition
		Dim num As Single = flyingCowboyLevelOilBlob.finalYPosition - flyingCowboyLevelOilBlob.initialYPosition
		Dim num2 As Single = Mathf.Abs(num)
		If num2 >= FlyingCowboyLevelOilBlob.BlobCHeight Then
			flyingCowboyLevelOilBlob.animator.Play(If((Not Rand.Bool()), "F", "C"))
			flyingCowboyLevelOilBlob.finalYPosition -= Mathf.Sign(num) * FlyingCowboyLevelOilBlob.BlobCHeight
		ElseIf num2 >= FlyingCowboyLevelOilBlob.BlobBHeight Then
			flyingCowboyLevelOilBlob.animator.Play("B")
			flyingCowboyLevelOilBlob.finalYPosition -= Mathf.Sign(num) * FlyingCowboyLevelOilBlob.BlobBHeight
		Else
			flyingCowboyLevelOilBlob.animator.Play("A")
		End If
		If flyingCowboyLevelOilBlob.finalYPosition < flyingCowboyLevelOilBlob.initialYPosition Then
			Dim localScale As Vector3 = flyingCowboyLevelOilBlob.transform.localScale
			localScale.y *= -1F
			flyingCowboyLevelOilBlob.transform.localScale = localScale
		End If
		flyingCowboyLevelOilBlob.StartCoroutine(flyingCowboyLevelOilBlob.snakeSpawn_cr(snakeSpawnX, finalYPosition, properties, playSplatSFX))
		Return flyingCowboyLevelOilBlob
	End Function

	' Token: 0x060021CF RID: 8655 RVA: 0x0013B32A File Offset: 0x0013972A
	Protected Overrides Sub Awake()
		Me.spriteRenderer = MyBase.GetComponent(Of SpriteRenderer)()
	End Sub

	' Token: 0x060021D0 RID: 8656 RVA: 0x0013B338 File Offset: 0x00139738
	Private Sub LateUpdate()
		If Me.spriteRenderer.sprite IsNot Me.previousSprite Then
			Me.previousSprite = Me.spriteRenderer.sprite
			Me.frameCounter += 1
		End If
		Dim position As Vector3 = MyBase.transform.position
		position.y = Mathf.Lerp(Me.initialYPosition, Me.finalYPosition, CSng(Me.frameCounter) / 28F)
		MyBase.transform.position = position
	End Sub

	' Token: 0x060021D1 RID: 8657 RVA: 0x0013B3BC File Offset: 0x001397BC
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If phase = CollisionPhase.Enter Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x060021D2 RID: 8658 RVA: 0x0013B3DC File Offset: 0x001397DC
	Private Iterator Function snakeSpawn_cr(snakeSpawnX As Single, snakeSpawnY As Single, properties As LevelProperties.FlyingCowboy.SnakeAttack, playSplatSFX As Boolean) As IEnumerator
		Yield Nothing
		Yield Nothing
		Yield MyBase.animator.WaitForNormalizedTime(Me, 1F, Nothing, 0, False, False, True)
		Dim snake As BasicProjectile = Me.snakePrefab.Create(New Vector2(snakeSpawnX + FlyingCowboyLevelOilBlob.SnakeSpawnOffsetX, snakeSpawnY), 0F, -properties.snakeSpeed)
		snake.animator.Play(0, 0, Global.UnityEngine.Random.Range(0F, 1F))
		Me.splatEffect.Create(New Vector2(640F, snakeSpawnY))
		If playSplatSFX Then
			AudioManager.Play("sfx_DLC_Cowgirl_P1_LiquidSplat")
		End If
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
		Return
	End Function

	' Token: 0x04002A81 RID: 10881
	Private Shared BlobBHeight As Single = 79F

	' Token: 0x04002A82 RID: 10882
	Private Shared BlobCHeight As Single = 293F

	' Token: 0x04002A83 RID: 10883
	Private Shared SnakeSpawnOffsetX As Single = 130F

	' Token: 0x04002A84 RID: 10884
	<SerializeField()>
	Private snakePrefab As BasicProjectile

	' Token: 0x04002A85 RID: 10885
	<SerializeField()>
	Private splatEffect As Effect

	' Token: 0x04002A86 RID: 10886
	Private spriteRenderer As SpriteRenderer

	' Token: 0x04002A87 RID: 10887
	Private previousSprite As Sprite

	' Token: 0x04002A88 RID: 10888
	Private initialYPosition As Single

	' Token: 0x04002A89 RID: 10889
	Private finalYPosition As Single

	' Token: 0x04002A8A RID: 10890
	Private frameCounter As Integer
End Class
