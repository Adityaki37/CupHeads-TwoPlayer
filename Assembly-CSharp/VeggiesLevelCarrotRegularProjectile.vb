Imports System
Imports UnityEngine

' Token: 0x0200084A RID: 2122
Public Class VeggiesLevelCarrotRegularProjectile
	Inherits AbstractProjectile

	' Token: 0x17000426 RID: 1062
	' (get) Token: 0x06003120 RID: 12576 RVA: 0x001CCF07 File Offset: 0x001CB307
	Protected Overrides ReadOnly Property DestroyLifetime As Single
		Get
			Return 1000F
		End Get
	End Property

	' Token: 0x06003121 RID: 12577 RVA: 0x001CCF10 File Offset: 0x001CB310
	Public Function Create(parent As VeggiesLevelCarrot, pos As Vector2, speed As Single, rotation As Single) As VeggiesLevelCarrotRegularProjectile
		Dim veggiesLevelCarrotRegularProjectile As VeggiesLevelCarrotRegularProjectile = TryCast(Me.Create(), VeggiesLevelCarrotRegularProjectile)
		veggiesLevelCarrotRegularProjectile.CollisionDeath.None()
		veggiesLevelCarrotRegularProjectile.DamagesType.OnlyPlayer()
		veggiesLevelCarrotRegularProjectile.Init(parent, pos, speed, rotation)
		Return veggiesLevelCarrotRegularProjectile
	End Function

	' Token: 0x06003122 RID: 12578 RVA: 0x001CCF4C File Offset: 0x001CB34C
	Protected Overrides Sub FixedUpdate()
		MyBase.FixedUpdate()
		If MyBase.dead Then
			Return
		End If
		MyBase.transform.position += MyBase.transform.right * (Me.speed * CupheadTime.FixedDelta)
	End Sub

	' Token: 0x06003123 RID: 12579 RVA: 0x001CCF9D File Offset: 0x001CB39D
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		RemoveHandler Me.parent.OnDeathEvent, AddressOf Me.Die
	End Sub

	' Token: 0x06003124 RID: 12580 RVA: 0x001CCFBD File Offset: 0x001CB3BD
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x06003125 RID: 12581 RVA: 0x001CCFDC File Offset: 0x001CB3DC
	Private Sub Init(parent As VeggiesLevelCarrot, pos As Vector2, speed As Single, rotation As Single)
		Me.parent = parent
		Me.speed = speed
		AddHandler parent.OnDeathEvent, AddressOf Me.Die
		MyBase.transform.position = pos
		MyBase.transform.SetLocalEulerAngles(New Single?(0F), New Single?(0F), New Single?(rotation))
	End Sub

	' Token: 0x06003126 RID: 12582 RVA: 0x001CD041 File Offset: 0x001CB441
	Protected Overrides Sub Die()
		AudioManager.Play("level_veggies_carrot_projectile_death")
		MyBase.Die()
		MyBase.GetComponent(Of Collider2D)().enabled = False
		Me.StopAllCoroutines()
	End Sub

	' Token: 0x040039BC RID: 14780
	Private speed As Single

	' Token: 0x040039BD RID: 14781
	Private parent As VeggiesLevelCarrot
End Class
