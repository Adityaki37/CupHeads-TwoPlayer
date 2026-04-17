Imports System
Imports UnityEngine

' Token: 0x02000A80 RID: 2688
Public Class WeaponPeashotExProjectile
	Inherits AbstractProjectile

	' Token: 0x06004046 RID: 16454 RVA: 0x00230B14 File Offset: 0x0022EF14
	Protected Overrides Sub FixedUpdate()
		MyBase.FixedUpdate()
		If MyBase.dead Then
			Return
		End If
		If Me.timeUntilUnfreeze > 0F Then
			Me.timeUntilUnfreeze -= CupheadTime.FixedDelta
			Me.currentSpeed = 0F
		Else
			Me.currentSpeed = Me.moveSpeed
		End If
		Dim vector As Vector2 = MathUtils.AngleToDirection(MyBase.transform.eulerAngles.z) * Me.currentSpeed
		MyBase.transform.AddPosition(vector.x * CupheadTime.FixedDelta, vector.y * CupheadTime.FixedDelta, 0F)
	End Sub

	' Token: 0x06004047 RID: 16455 RVA: 0x00230BC0 File Offset: 0x0022EFC0
	Protected Overrides Sub OnCollisionEnemy(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionEnemy(hit, phase)
		Dim num As Single = Me.damageDealer.DealDamage(hit)
		Me.totalDamage += num
		If Me.totalDamage > Me.maxDamage Then
			Me.Die()
		End If
		If num > 0F Then
			Me.hitFXPrefab.Create(Me.hitFxRoot.position)
			AudioManager.Play("player_ex_impact_hit")
			Me.emitAudioFromObject.Add("player_ex_impact_hit")
			Me.timeUntilUnfreeze = Me.hitFreezeTime
		End If
	End Sub

	' Token: 0x06004048 RID: 16456 RVA: 0x00230C4F File Offset: 0x0022F04F
	Protected Overrides Sub OnCollisionOther(hit As GameObject, phase As CollisionPhase)
		If hit.tag = "Parry" Then
			Return
		End If
		MyBase.OnCollisionOther(hit, phase)
	End Sub

	' Token: 0x04004715 RID: 18197
	<SerializeField()>
	Private hitFXPrefab As Effect

	' Token: 0x04004716 RID: 18198
	<SerializeField()>
	Private hitFxRoot As Transform

	' Token: 0x04004717 RID: 18199
	Private timeUntilUnfreeze As Single

	' Token: 0x04004718 RID: 18200
	Public moveSpeed As Single

	' Token: 0x04004719 RID: 18201
	Public hitFreezeTime As Single

	' Token: 0x0400471A RID: 18202
	Private totalDamage As Single

	' Token: 0x0400471B RID: 18203
	Private currentSpeed As Single

	' Token: 0x0400471C RID: 18204
	Public maxDamage As Single
End Class
