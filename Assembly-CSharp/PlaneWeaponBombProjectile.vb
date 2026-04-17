Imports System
Imports UnityEngine

' Token: 0x02000AB7 RID: 2743
Public Class PlaneWeaponBombProjectile
	Inherits AbstractProjectile

	' Token: 0x060041E7 RID: 16871 RVA: 0x00239F98 File Offset: 0x00238398
	Protected Overrides Sub Start()
		MyBase.Start()
		MyBase.transform.SetScale(New Single?(Me.bulletSize), New Single?(Me.bulletSize), Nothing)
		AudioManager.Play("plane_shmup_bomb_fire")
		Me.emitAudioFromObject.Add("plane_shmup_bomb_fire")
	End Sub

	' Token: 0x060041E8 RID: 16872 RVA: 0x00239FF0 File Offset: 0x002383F0
	Protected Overrides Sub FixedUpdate()
		MyBase.FixedUpdate()
		If MyBase.dead Then
			Return
		End If
		If Me.shootsUp Then
			Me.velocity.y = Me.velocity.y + Me.gravity * CupheadTime.FixedDelta
			MyBase.transform.position += Me.velocity * CupheadTime.FixedDelta
		Else
			Me.velocity.y = Me.velocity.y - Me.gravity * CupheadTime.FixedDelta
			MyBase.transform.position += Me.velocity * CupheadTime.FixedDelta
		End If
	End Sub

	' Token: 0x060041E9 RID: 16873 RVA: 0x0023A0B1 File Offset: 0x002384B1
	Private Sub DealDamage(hit As GameObject)
		Me.damageDealer.DealDamage(hit)
	End Sub

	' Token: 0x060041EA RID: 16874 RVA: 0x0023A0C0 File Offset: 0x002384C0
	Protected Overrides Sub OnCollisionEnemy(hit As GameObject, phase As CollisionPhase)
		Me.DealDamage(hit)
		MyBase.OnCollisionEnemy(hit, phase)
	End Sub

	' Token: 0x060041EB RID: 16875 RVA: 0x0023A0D1 File Offset: 0x002384D1
	Protected Overrides Sub OnCollisionOther(hit As GameObject, phase As CollisionPhase)
		If hit.tag <> "Parry" Then
			MyBase.OnCollisionOther(hit, phase)
		End If
	End Sub

	' Token: 0x060041EC RID: 16876 RVA: 0x0023A0F0 File Offset: 0x002384F0
	Protected Overrides Sub Die()
		MyBase.Die()
		MyBase.GetComponent(Of SpriteRenderer)().enabled = False
		AudioManager.Play("plane_shmup_bomb_explosion")
		Me.emitAudioFromObject.Add("plane_shmup_bomb_explosion")
		Me.explosion.Create(MyBase.transform.position, Me.Damage, MyBase.DamageMultiplier, Me.explosionSize)
	End Sub

	' Token: 0x060041ED RID: 16877 RVA: 0x0023A156 File Offset: 0x00238556
	Public Sub SetAnimation(player As PlayerId)
		MyBase.animator.Play(If(((player <> PlayerId.PlayerOne OrElse PlayerManager.player1IsMugman) AndAlso (player <> PlayerId.PlayerTwo OrElse Not PlayerManager.player1IsMugman)), "Bomb_MM", "Bomb_CH"))
	End Sub

	' Token: 0x0400484E RID: 18510
	<SerializeField()>
	Private explosion As PlaneWeaponBombExplosion

	' Token: 0x0400484F RID: 18511
	Public shootsUp As Boolean

	' Token: 0x04004850 RID: 18512
	Public explosionSize As Single

	' Token: 0x04004851 RID: 18513
	Public bulletSize As Single

	' Token: 0x04004852 RID: 18514
	Public gravity As Single

	' Token: 0x04004853 RID: 18515
	Public velocity As Vector2
End Class
