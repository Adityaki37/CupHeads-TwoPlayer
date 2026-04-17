Imports System
Imports UnityEngine

' Token: 0x02000ABB RID: 2747
Public Class PlaneWeaponChaliceBombProjectile
	Inherits AbstractProjectile

	' Token: 0x060041FB RID: 16891 RVA: 0x0023A4BC File Offset: 0x002388BC
	Protected Overrides Sub Start()
		MyBase.Start()
		MyBase.transform.SetScale(New Single?(Me.size), New Single?(Me.size), Nothing)
		AudioManager.Play("plane_shmup_bomb_fire")
		Me.emitAudioFromObject.Add("plane_shmup_bomb_fire")
	End Sub

	' Token: 0x060041FC RID: 16892 RVA: 0x0023A514 File Offset: 0x00238914
	Protected Overrides Sub FixedUpdate()
		MyBase.FixedUpdate()
		If MyBase.dead Then
			Return
		End If
		Me.velocity.y = Me.velocity.y - Me.gravity * CupheadTime.FixedDelta
		MyBase.transform.position += Me.velocity * CupheadTime.FixedDelta
	End Sub

	' Token: 0x060041FD RID: 16893 RVA: 0x0023A57C File Offset: 0x0023897C
	Private Sub DealDamage(hit As GameObject)
		Me.damageDealer.DealDamage(hit)
	End Sub

	' Token: 0x060041FE RID: 16894 RVA: 0x0023A58B File Offset: 0x0023898B
	Protected Overrides Sub OnCollisionEnemy(hit As GameObject, phase As CollisionPhase)
		Me.DealDamage(hit)
		MyBase.OnCollisionEnemy(hit, phase)
	End Sub

	' Token: 0x060041FF RID: 16895 RVA: 0x0023A59C File Offset: 0x0023899C
	Protected Overrides Sub OnCollisionOther(hit As GameObject, phase As CollisionPhase)
		If hit.tag <> "Parry" Then
			MyBase.OnCollisionOther(hit, phase)
		End If
	End Sub

	' Token: 0x06004200 RID: 16896 RVA: 0x0023A5BC File Offset: 0x002389BC
	Protected Overrides Sub Die()
		MyBase.Die()
		MyBase.GetComponent(Of SpriteRenderer)().enabled = False
		AudioManager.Play("plane_shmup_bomb_explosion")
		Me.emitAudioFromObject.Add("plane_shmup_bomb_explosion")
		Me.explosion.Create(MyBase.transform.position, Me.damageExplosion, MyBase.DamageMultiplier, Me.explosionSize)
		Me.explosion.animator.Play(If((Not Me.isA), "B", "A"))
	End Sub

	' Token: 0x06004201 RID: 16897 RVA: 0x0023A64C File Offset: 0x00238A4C
	Public Sub SetAnimation(isA As Boolean)
		Me.isA = isA
		MyBase.animator.Play(If((Not isA), "B", "A"))
	End Sub

	' Token: 0x04004863 RID: 18531
	<SerializeField()>
	Private explosion As PlaneWeaponBombExplosion

	' Token: 0x04004864 RID: 18532
	Public explosionSize As Single

	' Token: 0x04004865 RID: 18533
	Public gravity As Single

	' Token: 0x04004866 RID: 18534
	Public damageExplosion As Single

	' Token: 0x04004867 RID: 18535
	Public size As Single

	' Token: 0x04004868 RID: 18536
	Private isA As Boolean

	' Token: 0x04004869 RID: 18537
	Public velocity As Vector2
End Class
