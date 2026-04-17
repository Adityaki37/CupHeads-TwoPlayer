Imports System
Imports UnityEngine

' Token: 0x02000799 RID: 1945
Public Class RumRunnersLevelPoliceBullet
	Inherits BasicProjectile

	' Token: 0x170003FA RID: 1018
	' (get) Token: 0x06002B35 RID: 11061 RVA: 0x00192C59 File Offset: 0x00191059
	' (set) Token: 0x06002B36 RID: 11062 RVA: 0x00192C61 File Offset: 0x00191061
	Public Property spiderDamage As Single

	' Token: 0x170003FB RID: 1019
	' (get) Token: 0x06002B37 RID: 11063 RVA: 0x00192C6A File Offset: 0x0019106A
	' (set) Token: 0x06002B38 RID: 11064 RVA: 0x00192C72 File Offset: 0x00191072
	Public Property direction As RumRunnersLevelPoliceman.Direction

	' Token: 0x06002B39 RID: 11065 RVA: 0x00192C7C File Offset: 0x0019107C
	Protected Overrides Sub Start()
		MyBase.Start()
		Me.spiderDamageDealer = New DamageDealer(Me)
		Me.spiderDamageDealer.SetDamage(Me.spiderDamage)
		AddHandler Me.spiderDamageDealer.OnDealDamage, AddressOf Me.OnDealDamage
		Me.spiderDamageDealer.SetStoneTime(MyBase.StoneTime)
		Me.spiderDamageDealer.PlayerId = Me.PlayerId
	End Sub

	' Token: 0x06002B3A RID: 11066 RVA: 0x00192CE8 File Offset: 0x001910E8
	Protected Overrides Sub OnCollisionEnemy(hit As GameObject, phase As CollisionPhase)
		If phase <> CollisionPhase.[Exit] Then
			Dim damageReceiver As DamageReceiver = hit.GetComponent(Of DamageReceiver)()
			If damageReceiver Is Nothing Then
				Dim component As DamageReceiverChild = hit.GetComponent(Of DamageReceiverChild)()
				If component IsNot Nothing Then
					damageReceiver = component.Receiver
				End If
			End If
			If damageReceiver IsNot Nothing AndAlso damageReceiver.GetComponent(Of RumRunnersLevelSpider)() IsNot Nothing Then
				Me.spiderDamageDealer.DealDamage(hit)
				MyBase.OnCollisionEnemy(hit, phase)
				Me.Die()
			End If
		End If
	End Sub

	' Token: 0x040033E8 RID: 13288
	Private spiderDamageDealer As DamageDealer
End Class
