Imports System
Imports UnityEngine

' Token: 0x02000586 RID: 1414
<RequireComponent(GetType(Collider2D))>
Public Class DevilLevelSplitDevilVisual
	Inherits LevelProperties.Devil.Entity

	' Token: 0x06001AFB RID: 6907 RVA: 0x000F7F8A File Offset: 0x000F638A
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.damageDealer = DamageDealer.NewEnemy()
		Me.damageReceiver = MyBase.GetComponent(Of DamageReceiver)()
		AddHandler Me.damageReceiver.OnDamageTaken, AddressOf MyBase.GetComponentInParent(Of DevilLevelSplitDevil)().OnDamageTaken
	End Sub

	' Token: 0x06001AFC RID: 6908 RVA: 0x000F7FC5 File Offset: 0x000F63C5
	Private Sub Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x06001AFD RID: 6909 RVA: 0x000F7FDD File Offset: 0x000F63DD
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If Me.damageDealer IsNot Nothing AndAlso phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x0400243A RID: 9274
	Private damageDealer As DamageDealer

	' Token: 0x0400243B RID: 9275
	Private damageReceiver As DamageReceiver
End Class
