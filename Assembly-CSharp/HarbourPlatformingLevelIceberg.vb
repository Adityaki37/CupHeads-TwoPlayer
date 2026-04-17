Imports System
Imports UnityEngine

' Token: 0x020008C9 RID: 2249
Public Class HarbourPlatformingLevelIceberg
	Inherits AbstractCollidableObject

	' Token: 0x06003490 RID: 13456 RVA: 0x001E8647 File Offset: 0x001E6A47
	Private Sub Start()
		Me.damageDealer = DamageDealer.NewEnemy()
	End Sub

	' Token: 0x06003491 RID: 13457 RVA: 0x001E8654 File Offset: 0x001E6A54
	Private Sub Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x06003492 RID: 13458 RVA: 0x001E866C File Offset: 0x001E6A6C
	Protected Overrides Sub OnCollision(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollision(hit, phase)
		If hit.GetComponent(Of HarbourPlatformingLevelOctoProjectile)() Then
			Me.SmashSFX()
			Global.UnityEngine.[Object].Destroy(hit.gameObject)
			Me.DeathParts()
			Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
		End If
	End Sub

	' Token: 0x06003493 RID: 13459 RVA: 0x001E86A8 File Offset: 0x001E6AA8
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x06003494 RID: 13460 RVA: 0x001E86C8 File Offset: 0x001E6AC8
	Public Sub DeathParts()
		Me.explosion.Create(MyBase.transform.position)
		For Each spriteDeathParts As SpriteDeathParts In Me.deathParts
			spriteDeathParts.CreatePart(MyBase.transform.position)
		Next
	End Sub

	' Token: 0x06003495 RID: 13461 RVA: 0x001E871D File Offset: 0x001E6B1D
	Private Sub SmashSFX()
		AudioManager.Play("harbour_iceberg_smash")
		Me.emitAudioFromObject.Add("harbour_iceberg_smash")
	End Sub

	' Token: 0x04003CBE RID: 15550
	<SerializeField()>
	Private explosion As Effect

	' Token: 0x04003CBF RID: 15551
	<SerializeField()>
	Private deathParts As SpriteDeathParts()

	' Token: 0x04003CC0 RID: 15552
	Private damageDealer As DamageDealer
End Class
