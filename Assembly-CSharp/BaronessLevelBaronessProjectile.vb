Imports System
Imports UnityEngine

' Token: 0x020004DD RID: 1245
Public Class BaronessLevelBaronessProjectile
	Inherits AbstractProjectile

	' Token: 0x0600155A RID: 5466 RVA: 0x000BF354 File Offset: 0x000BD754
	Protected Overrides Sub Start()
		MyBase.Start()
		Me.health = CSng(MyBase.GetComponentInParent(Of BaronessLevelBaronessProjectileBunch)().properties.projectileHP)
		Me.damageReceiver = MyBase.GetComponent(Of DamageReceiver)()
		AddHandler Me.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
	End Sub

	' Token: 0x0600155B RID: 5467 RVA: 0x000BF3A1 File Offset: 0x000BD7A1
	Protected Overrides Sub Update()
		MyBase.Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x0600155C RID: 5468 RVA: 0x000BF3BF File Offset: 0x000BD7BF
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x0600155D RID: 5469 RVA: 0x000BF3DD File Offset: 0x000BD7DD
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		Me.health -= info.damage
		If Me.health < 0F Then
			Me.Die()
		End If
	End Sub

	' Token: 0x0600155E RID: 5470 RVA: 0x000BF408 File Offset: 0x000BD808
	Protected Overrides Sub Die()
		Me.deathFX.Create(MyBase.transform.position)
		MyBase.GetComponent(Of Collider2D)().enabled = False
		MyBase.GetComponent(Of SpriteRenderer)().enabled = False
		Me.FX.SetActive(False)
		Me.StopAllCoroutines()
		MyBase.Die()
	End Sub

	' Token: 0x04001EB3 RID: 7859
	<SerializeField()>
	Private deathFX As Effect

	' Token: 0x04001EB4 RID: 7860
	<SerializeField()>
	Private FX As GameObject

	' Token: 0x04001EB5 RID: 7861
	Private damageReceiver As DamageReceiver

	' Token: 0x04001EB6 RID: 7862
	Private health As Single
End Class
