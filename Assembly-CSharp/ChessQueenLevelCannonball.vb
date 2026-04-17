Imports System
Imports UnityEngine

' Token: 0x02000549 RID: 1353
Public Class ChessQueenLevelCannonball
	Inherits BasicProjectile

	' Token: 0x060018F9 RID: 6393 RVA: 0x000E281F File Offset: 0x000E0C1F
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.DamagesType.Player = False
	End Sub

	' Token: 0x060018FA RID: 6394 RVA: 0x000E2834 File Offset: 0x000E0C34
	Public Overrides Function Create(position As Vector2, rotation As Single) As AbstractProjectile
		Dim chessQueenLevelCannonball As ChessQueenLevelCannonball = TryCast(Me.Create(position), ChessQueenLevelCannonball)
		chessQueenLevelCannonball.vel = MathUtils.AngleToDirection(rotation)
		Return chessQueenLevelCannonball
	End Function

	' Token: 0x060018FB RID: 6395 RVA: 0x000E2860 File Offset: 0x000E0C60
	Protected Overrides Sub Move()
		If Me.hit Then
			Return
		End If
		MyBase.transform.position += Me.vel * Me.Speed * CupheadTime.FixedDelta
		Me.frameTimer += CupheadTime.FixedDelta
		If Me.frameTimer >= 0.041666668F Then
			Dim num As Single = Mathf.Lerp(1F, Me.minScale, Mathf.InverseLerp(-150F, 440F, MyBase.transform.position.y))
			Me.sprite.transform.localScale = New Vector3(num, num)
			Me.frameTimer -= 0.041666668F
		End If
	End Sub

	' Token: 0x060018FC RID: 6396 RVA: 0x000E2928 File Offset: 0x000E0D28
	Public Sub HitQueen()
		Me.hit = True
		Me.sprite.transform.localScale = New Vector3(1F, 1F)
		Me.sprite.flipX = Rand.Bool()
		Me.sprite.transform.eulerAngles = New Vector3(0F, 0F, CSng(Global.UnityEngine.Random.Range(0, 360)))
		MyBase.animator.Play("Explode")
		MyBase.animator.Update(0F)
	End Sub

	' Token: 0x0400220F RID: 8719
	Private Const FRAME_TIME As Single = 0.041666668F

	' Token: 0x04002210 RID: 8720
	Private vel As Vector3

	' Token: 0x04002211 RID: 8721
	<SerializeField()>
	Private minScale As Single = 0.75F

	' Token: 0x04002212 RID: 8722
	<SerializeField()>
	Private sprite As SpriteRenderer

	' Token: 0x04002213 RID: 8723
	Private frameTimer As Single

	' Token: 0x04002214 RID: 8724
	Private hit As Boolean
End Class
