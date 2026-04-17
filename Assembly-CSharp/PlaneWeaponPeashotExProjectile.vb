Imports System
Imports UnityEngine

' Token: 0x02000AC1 RID: 2753
Public Class PlaneWeaponPeashotExProjectile
	Inherits AbstractProjectile

	' Token: 0x06004220 RID: 16928 RVA: 0x0023B8BC File Offset: 0x00239CBC
	Public Sub Init()
		Me.Cuphead.enabled = (Me.PlayerId = PlayerId.PlayerOne AndAlso Not PlayerManager.player1IsMugman) OrElse (Me.PlayerId = PlayerId.PlayerTwo AndAlso PlayerManager.player1IsMugman)
		Me.Mugman.enabled = (Me.PlayerId = PlayerId.PlayerOne AndAlso PlayerManager.player1IsMugman) OrElse (Me.PlayerId = PlayerId.PlayerTwo AndAlso Not PlayerManager.player1IsMugman)
	End Sub

	' Token: 0x06004221 RID: 16929 RVA: 0x0023B93C File Offset: 0x00239D3C
	Protected Overrides Sub OnDealDamage(damage As Single, receiver As DamageReceiver, damageDealer As DamageDealer)
		MyBase.OnDealDamage(damage, receiver, damageDealer)
		Me.chompFxPrefab.Create(Me.chompFxRoot.position)
		Me.state = PlaneWeaponPeashotExProjectile.State.Frozen
		Me.speed = 0F
		Me.timeSinceFrozen = 0F
		AudioManager.Play("player_plane_weapon_ex_chomp")
		Me.emitAudioFromObject.Add("player_plane_weapon_ex_chomp")
	End Sub

	' Token: 0x06004222 RID: 16930 RVA: 0x0023B9A0 File Offset: 0x00239DA0
	Protected Overrides Sub FixedUpdate()
		MyBase.FixedUpdate()
		If MyBase.dead Then
			Return
		End If
		Dim state As PlaneWeaponPeashotExProjectile.State = Me.state
		If state <> PlaneWeaponPeashotExProjectile.State.Idle Then
			If state = PlaneWeaponPeashotExProjectile.State.Frozen Then
				Me.timeSinceFrozen += CupheadTime.FixedDelta
				If Me.timeSinceFrozen > Me.FreezeTime Then
					Me.state = PlaneWeaponPeashotExProjectile.State.Idle
					Me.speed = Me.MaxSpeed
				End If
			End If
		Else
			Me.speed = Mathf.Min(Me.MaxSpeed, Me.speed + Me.Acceleration * CupheadTime.FixedDelta)
		End If
		MyBase.transform.AddPosition(Me.speed * CupheadTime.FixedDelta, 0F, 0F)
	End Sub

	' Token: 0x06004223 RID: 16931 RVA: 0x0023BA5D File Offset: 0x00239E5D
	Protected Overrides Sub OnCollisionEnemy(hit As GameObject, phase As CollisionPhase)
		Me.DealDamage(hit)
		MyBase.OnCollisionEnemy(hit, phase)
	End Sub

	' Token: 0x06004224 RID: 16932 RVA: 0x0023BA6E File Offset: 0x00239E6E
	Protected Overrides Sub OnCollisionOther(hit As GameObject, phase As CollisionPhase)
		If hit.tag = "Parry" Then
			Return
		End If
		MyBase.OnCollisionOther(hit, phase)
	End Sub

	' Token: 0x06004225 RID: 16933 RVA: 0x0023BA8E File Offset: 0x00239E8E
	Private Sub DealDamage(hit As GameObject)
		Me.damageDealer.DealDamage(hit)
	End Sub

	' Token: 0x06004226 RID: 16934 RVA: 0x0023BA9D File Offset: 0x00239E9D
	Public Overrides Sub OnLevelEnd()
	End Sub

	' Token: 0x04004896 RID: 18582
	Public MaxSpeed As Single

	' Token: 0x04004897 RID: 18583
	Public Acceleration As Single

	' Token: 0x04004898 RID: 18584
	Public FreezeTime As Single

	' Token: 0x04004899 RID: 18585
	<SerializeField()>
	Private chompFxPrefab As Effect

	' Token: 0x0400489A RID: 18586
	<SerializeField()>
	Private chompFxRoot As Transform

	' Token: 0x0400489B RID: 18587
	<SerializeField()>
	Private Cuphead As SpriteRenderer

	' Token: 0x0400489C RID: 18588
	<SerializeField()>
	Private Mugman As SpriteRenderer

	' Token: 0x0400489D RID: 18589
	Private state As PlaneWeaponPeashotExProjectile.State

	' Token: 0x0400489E RID: 18590
	Private timeSinceFrozen As Single

	' Token: 0x0400489F RID: 18591
	Public speed As Single

	' Token: 0x02000AC2 RID: 2754
	Public Enum State
		' Token: 0x040048A1 RID: 18593
		Idle
		' Token: 0x040048A2 RID: 18594
		Frozen
	End Enum
End Class
