Imports System
Imports UnityEngine

' Token: 0x0200054A RID: 1354
Public Class ChessQueenLevelEgg
	Inherits AbstractProjectile

	' Token: 0x1700033D RID: 829
	' (get) Token: 0x060018FE RID: 6398 RVA: 0x000E29BE File Offset: 0x000E0DBE
	Public Overrides ReadOnly Property ParryMeterMultiplier As Single
		Get
			Return 0F
		End Get
	End Property

	' Token: 0x060018FF RID: 6399 RVA: 0x000E29C8 File Offset: 0x000E0DC8
	Public Function Create(position As Vector3, velocity As Vector3, gravity As Single, delay As Single) As ChessQueenLevelEgg
		MyBase.ResetLifetime()
		MyBase.ResetDistance()
		MyBase.transform.position = position
		Me.velocity = velocity
		Me.gravity = gravity
		Me.delay = delay
		Me.coll.enabled = False
		Me.rend.flipX = Rand.Bool()
		Me.anim.Play(Global.UnityEngine.Random.Range(0, 12).ToString())
		Return Me
	End Function

	' Token: 0x06001900 RID: 6400 RVA: 0x000E2A46 File Offset: 0x000E0E46
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x06001901 RID: 6401 RVA: 0x000E2A64 File Offset: 0x000E0E64
	Protected Overrides Sub FixedUpdate()
		MyBase.FixedUpdate()
		If Me.isDead Then
			Return
		End If
		If MyBase.lifetime > Me.delay Then
			Me.rend.sortingLayerName = "Projectiles"
			Me.rend.sortingOrder = 0
			Me.rend.color = Color.white
			Me.coll.enabled = True
		Else
			Me.rend.color = Color.Lerp(New Color(0.7F, 0.7F, 0.7F, 1F), Color.white, MyBase.lifetime / Me.delay)
		End If
		MyBase.transform.AddPosition(Me.velocity.x * CupheadTime.FixedDelta, Me.velocity.y * CupheadTime.FixedDelta, 0F)
		Me.velocity.y = Me.velocity.y - Me.gravity * CupheadTime.FixedDelta
		If MyBase.transform.position.y < CSng(Level.Current.Ground) + 15F Then
			Me.HitGround()
		End If
	End Sub

	' Token: 0x06001902 RID: 6402 RVA: 0x000E2B8C File Offset: 0x000E0F8C
	Protected Sub HitGround()
		Me.StopAllCoroutines()
		MyBase.transform.position = New Vector3(MyBase.transform.position.x, CSng(Level.Current.Ground) + 15F)
		Me.isDead = True
		Me.coll.enabled = False
		Me.explosionRend.flipX = Rand.Bool()
		Me.anim.Play(If((Not Rand.Bool()), "ExplodeB", "ExplodeA"), 1, 0F)
		Me.anim.Update(0F)
	End Sub

	' Token: 0x04002215 RID: 8725
	Private Const GROUND_OFFSET As Single = 15F

	' Token: 0x04002216 RID: 8726
	Private velocity As Vector2

	' Token: 0x04002217 RID: 8727
	Private gravity As Single

	' Token: 0x04002218 RID: 8728
	Private isDead As Boolean

	' Token: 0x04002219 RID: 8729
	Private delay As Single

	' Token: 0x0400221A RID: 8730
	<SerializeField()>
	Private anim As Animator

	' Token: 0x0400221B RID: 8731
	<SerializeField()>
	Private coll As Collider2D

	' Token: 0x0400221C RID: 8732
	<SerializeField()>
	Private rend As SpriteRenderer

	' Token: 0x0400221D RID: 8733
	<SerializeField()>
	Private explosionRend As SpriteRenderer
End Class
