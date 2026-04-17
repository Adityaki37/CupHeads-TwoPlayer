Imports System
Imports UnityEngine

' Token: 0x02000AB9 RID: 2745
Public Class PlaneWeaponChaliceBombExProjectile
	Inherits AbstractProjectile

	' Token: 0x060041F4 RID: 16884 RVA: 0x0023A314 File Offset: 0x00238714
	Protected Overrides Sub OnDealDamage(damage As Single, receiver As DamageReceiver, damageDealer As DamageDealer)
		MyBase.OnDealDamage(damage, receiver, damageDealer)
		Me.DamageRate += Me.DamageRateIncrease
		damageDealer.SetRate(Me.DamageRate)
		Me.chompFxPrefab.Create(Me.chompFxRoot.position)
		Me.state = PlaneWeaponChaliceBombExProjectile.State.Frozen
		Me.speed = 0F
		Me.timeSinceFrozen = 0F
		AudioManager.Play("player_plane_weapon_ex_chomp")
		Me.emitAudioFromObject.Add("player_plane_weapon_ex_chomp")
	End Sub

	' Token: 0x060041F5 RID: 16885 RVA: 0x0023A398 File Offset: 0x00238798
	Protected Overrides Sub FixedUpdate()
		MyBase.FixedUpdate()
		If MyBase.dead Then
			Return
		End If
		Dim state As PlaneWeaponChaliceBombExProjectile.State = Me.state
		If state <> PlaneWeaponChaliceBombExProjectile.State.Idle Then
			If state = PlaneWeaponChaliceBombExProjectile.State.Frozen Then
				Me.timeSinceFrozen += CupheadTime.FixedDelta
				If Me.timeSinceFrozen > Me.FreezeTime Then
					Me.state = PlaneWeaponChaliceBombExProjectile.State.Idle
				End If
			End If
		Else
			Me.Velocity.y = Me.Velocity.y - Me.Gravity * CupheadTime.FixedDelta
			MyBase.transform.position += Me.Velocity * CupheadTime.FixedDelta
			MyBase.transform.rotation = Quaternion.Euler(0F, 0F, MathUtils.DirectionToAngle(Me.Velocity))
		End If
	End Sub

	' Token: 0x060041F6 RID: 16886 RVA: 0x0023A472 File Offset: 0x00238872
	Protected Overrides Sub OnCollisionEnemy(hit As GameObject, phase As CollisionPhase)
		Me.DealDamage(hit)
		MyBase.OnCollisionEnemy(hit, phase)
	End Sub

	' Token: 0x060041F7 RID: 16887 RVA: 0x0023A483 File Offset: 0x00238883
	Protected Overrides Sub OnCollisionOther(hit As GameObject, phase As CollisionPhase)
		If hit.tag = "Parry" Then
			Return
		End If
		MyBase.OnCollisionOther(hit, phase)
	End Sub

	' Token: 0x060041F8 RID: 16888 RVA: 0x0023A4A3 File Offset: 0x002388A3
	Private Sub DealDamage(hit As GameObject)
		Me.damageDealer.DealDamage(hit)
	End Sub

	' Token: 0x060041F9 RID: 16889 RVA: 0x0023A4B2 File Offset: 0x002388B2
	Public Overrides Sub OnLevelEnd()
	End Sub

	' Token: 0x04004855 RID: 18517
	Public MaxSpeed As Single

	' Token: 0x04004856 RID: 18518
	Public Acceleration As Single

	' Token: 0x04004857 RID: 18519
	Public FreezeTime As Single

	' Token: 0x04004858 RID: 18520
	<SerializeField()>
	Private chompFxPrefab As Effect

	' Token: 0x04004859 RID: 18521
	<SerializeField()>
	Private chompFxRoot As Transform

	' Token: 0x0400485A RID: 18522
	Public Velocity As Vector3

	' Token: 0x0400485B RID: 18523
	Public Gravity As Single

	' Token: 0x0400485C RID: 18524
	Public DamageRateIncrease As Single

	' Token: 0x0400485D RID: 18525
	Private state As PlaneWeaponChaliceBombExProjectile.State

	' Token: 0x0400485E RID: 18526
	Private timeSinceFrozen As Single

	' Token: 0x0400485F RID: 18527
	Public speed As Single

	' Token: 0x02000ABA RID: 2746
	Public Enum State
		' Token: 0x04004861 RID: 18529
		Idle
		' Token: 0x04004862 RID: 18530
		Frozen
	End Enum
End Class
