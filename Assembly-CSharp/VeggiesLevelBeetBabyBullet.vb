Imports System
Imports UnityEngine

' Token: 0x02000840 RID: 2112
Public Class VeggiesLevelBeetBabyBullet
	Inherits AbstractProjectile

	' Token: 0x060030E7 RID: 12519 RVA: 0x001CBED0 File Offset: 0x001CA2D0
	Public Function Create(speed As Single, pos As Vector2, rot As Single) As VeggiesLevelBeetBabyBullet
		Dim veggiesLevelBeetBabyBullet As VeggiesLevelBeetBabyBullet = TryCast(Me.Create(pos, rot), VeggiesLevelBeetBabyBullet)
		veggiesLevelBeetBabyBullet.CollisionDeath.OnlyPlayer()
		veggiesLevelBeetBabyBullet.DamagesType.OnlyPlayer()
		veggiesLevelBeetBabyBullet.speed = speed
		Return veggiesLevelBeetBabyBullet
	End Function

	' Token: 0x060030E8 RID: 12520 RVA: 0x001CBF0A File Offset: 0x001CA30A
	Protected Overrides Sub Awake()
		MyBase.Awake()
	End Sub

	' Token: 0x060030E9 RID: 12521 RVA: 0x001CBF14 File Offset: 0x001CA314
	Protected Overrides Sub Update()
		MyBase.Update()
		If Me.state = VeggiesLevelBeetBabyBullet.State.Dead Then
			Return
		End If
		MyBase.transform.position += MyBase.transform.right * Me.speed * CupheadTime.Delta
		If MyBase.transform.position.y < CSng(Level.Current.Ground) Then
			Me.Die()
		End If
	End Sub

	' Token: 0x060030EA RID: 12522 RVA: 0x001CBF98 File Offset: 0x001CA398
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		Me.damageDealer.DealDamage(hit)
		MyBase.OnCollisionPlayer(hit, phase)
	End Sub

	' Token: 0x060030EB RID: 12523 RVA: 0x001CBFAF File Offset: 0x001CA3AF
	Protected Overrides Sub Die()
		MyBase.Die()
		Me.state = VeggiesLevelBeetBabyBullet.State.Dead
		MyBase.animator.SetTrigger("Death")
		MyBase.GetComponent(Of Collider2D)().enabled = False
	End Sub

	' Token: 0x04003991 RID: 14737
	Private state As VeggiesLevelBeetBabyBullet.State

	' Token: 0x04003992 RID: 14738
	Private speed As Single

	' Token: 0x02000841 RID: 2113
	Public Enum State
		' Token: 0x04003994 RID: 14740
		Go
		' Token: 0x04003995 RID: 14741
		Dead
	End Enum
End Class
