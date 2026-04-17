Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000A68 RID: 2664
Public Class WeaponArcProjectile
	Inherits AbstractProjectile

	' Token: 0x17000579 RID: 1401
	' (get) Token: 0x06003F8D RID: 16269 RVA: 0x0022B43C File Offset: 0x0022983C
	Protected Overrides ReadOnly Property DestroyLifetime As Single
		Get
			Return 1000F
		End Get
	End Property

	' Token: 0x06003F8E RID: 16270 RVA: 0x0022B443 File Offset: 0x00229843
	Protected Overrides Sub Awake()
		MyBase.Awake()
	End Sub

	' Token: 0x06003F8F RID: 16271 RVA: 0x0022B44C File Offset: 0x0022984C
	Protected Overrides Sub FixedUpdate()
		MyBase.FixedUpdate()
		If MyBase.dead Then
			Return
		End If
		Dim state As WeaponArcProjectile.State = Me._state
		If state <> WeaponArcProjectile.State.InAir Then
			If state = WeaponArcProjectile.State.OnGround Then
				Me.UpdateOnGround()
			End If
		Else
			Me.UpdateInAir()
		End If
		If Not Me.isEx Then
			Me.UpdateDamageState()
			If Not CupheadLevelCamera.Current.ContainsPoint(MyBase.transform.position, New Vector2(150F, 1000F)) Then
				Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
			End If
		End If
	End Sub

	' Token: 0x06003F90 RID: 16272 RVA: 0x0022B4E4 File Offset: 0x002298E4
	Private Sub UpdateInAir()
		Me.velocity.y = Me.velocity.y - Me.gravity * CupheadTime.FixedDelta
		MyBase.transform.position += Me.velocity * CupheadTime.FixedDelta
	End Sub

	' Token: 0x06003F91 RID: 16273 RVA: 0x0022B53A File Offset: 0x0022993A
	Private Sub UpdateOnGround()
	End Sub

	' Token: 0x06003F92 RID: 16274 RVA: 0x0022B53C File Offset: 0x0022993C
	Private Sub UpdateDamageState()
		If MyBase.lifetime < WeaponProperties.LevelWeaponArc.Basic.timeStateTwo Then
			Me.Damage = WeaponProperties.LevelWeaponArc.Basic.baseDamage
			MyBase.transform.SetScale(New Single?(1F), New Single?(1F), Nothing)
		ElseIf MyBase.lifetime < WeaponProperties.LevelWeaponArc.Basic.timeStateThree Then
			Me.Damage = WeaponProperties.LevelWeaponArc.Basic.damageStateTwo
			MyBase.transform.SetScale(New Single?(1.5F), New Single?(1.5F), Nothing)
		Else
			Me.Damage = WeaponProperties.LevelWeaponArc.Basic.damageStateThree
			MyBase.transform.SetScale(New Single?(2.5F), New Single?(2.5F), Nothing)
		End If
	End Sub

	' Token: 0x06003F93 RID: 16275 RVA: 0x0022B60C File Offset: 0x00229A0C
	Protected Overrides Sub OnCollisionGround(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionGround(hit, phase)
		Dim component As LevelPlatform = hit.GetComponent(Of LevelPlatform)()
		If Me._state = WeaponArcProjectile.State.InAir AndAlso (component Is Nothing OrElse (Not component.canFallThrough AndAlso Me.velocity.y < 0F)) Then
			Me.HitGround(hit)
		End If
	End Sub

	' Token: 0x06003F94 RID: 16276 RVA: 0x0022B668 File Offset: 0x00229A68
	Protected Overrides Sub OnCollisionOther(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionOther(hit, phase)
		Dim component As LevelPlatform = hit.GetComponent(Of LevelPlatform)()
		If Me._state = WeaponArcProjectile.State.InAir AndAlso component IsNot Nothing AndAlso Not component.canFallThrough AndAlso Me.velocity.y < 0F Then
			Me.HitGround(hit)
		End If
	End Sub

	' Token: 0x06003F95 RID: 16277 RVA: 0x0022B6C2 File Offset: 0x00229AC2
	Protected Overrides Sub OnCollisionEnemy(hit As GameObject, phase As CollisionPhase)
		If Not Me.isEx Then
			Me.damageDealer.SetDamage(Me.Damage)
			Me.damageDealer.DealDamage(hit)
		End If
		MyBase.OnCollisionEnemy(hit, phase)
	End Sub

	' Token: 0x06003F96 RID: 16278 RVA: 0x0022B6F8 File Offset: 0x00229AF8
	Private Sub HitGround(hit As GameObject)
		Me._state = WeaponArcProjectile.State.OnGround
		If Not Me.isEx Then
			Me.weapon.projectilesOnGround.Add(Me)
			If Me.weapon.projectilesOnGround.Count > WeaponProperties.LevelWeaponArc.Basic.maxNumMines Then
				Dim weaponArcProjectile As WeaponArcProjectile = Me.weapon.projectilesOnGround(0)
				Me.weapon.projectilesOnGround.RemoveAt(0)
				weaponArcProjectile.Die()
			End If
		Else
			MyBase.StartCoroutine(Me.timedExplode_cr())
		End If
		MyBase.transform.SetParent(hit.transform)
	End Sub

	' Token: 0x06003F97 RID: 16279 RVA: 0x0022B790 File Offset: 0x00229B90
	Private Iterator Function timedExplode_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, WeaponProperties.LevelWeaponArc.Ex.explodeDelay)
		Me.Die()
		Return
	End Function

	' Token: 0x06003F98 RID: 16280 RVA: 0x0022B7AC File Offset: 0x00229BAC
	Protected Overrides Sub Die()
		MyBase.Die()
		If Me.isEx Then
			Me.exExplosion.Create(MyBase.transform.position, Me.Damage, MyBase.DamageMultiplier, Me.PlayerId)
			AudioManager.Play("player_weapon_arc_ex_explosion")
			Me.emitAudioFromObject.Add("player_weapon_arc_ex_explosion")
			Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
		End If
	End Sub

	' Token: 0x06003F99 RID: 16281 RVA: 0x0022B81D File Offset: 0x00229C1D
	Protected Overrides Sub OnDestroy()
		If Me.weapon.projectilesOnGround.Contains(Me) Then
			Me.weapon.projectilesOnGround.Remove(Me)
		End If
		MyBase.OnDestroy()
	End Sub

	' Token: 0x04004678 RID: 18040
	<SerializeField()>
	Private isEx As Boolean

	' Token: 0x04004679 RID: 18041
	<SerializeField()>
	Private exExplosion As WeaponArcProjectileExplosion

	' Token: 0x0400467A RID: 18042
	Public chargeTime As Single

	' Token: 0x0400467B RID: 18043
	Public gravity As Single

	' Token: 0x0400467C RID: 18044
	Public velocity As Vector2

	' Token: 0x0400467D RID: 18045
	Public weapon As WeaponArc

	' Token: 0x0400467E RID: 18046
	Private _state As WeaponArcProjectile.State

	' Token: 0x02000A69 RID: 2665
	Public Enum State
		' Token: 0x04004680 RID: 18048
		InAir
		' Token: 0x04004681 RID: 18049
		OnGround
	End Enum
End Class
