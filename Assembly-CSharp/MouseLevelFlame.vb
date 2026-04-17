Imports System
Imports UnityEngine

' Token: 0x020006EE RID: 1774
Public Class MouseLevelFlame
	Inherits AbstractCollidableObject

	' Token: 0x060025FF RID: 9727 RVA: 0x00163A8B File Offset: 0x00161E8B
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.damageDealer = DamageDealer.NewEnemy()
		MyBase.transform.parent = Nothing
	End Sub

	' Token: 0x06002600 RID: 9728 RVA: 0x00163AAA File Offset: 0x00161EAA
	Private Sub Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x06002601 RID: 9729 RVA: 0x00163AC2 File Offset: 0x00161EC2
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If Me.damageDealer IsNot Nothing AndAlso phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x06002602 RID: 9730 RVA: 0x00163AEC File Offset: 0x00161EEC
	Public Sub SetColliderEnabled(enabled As Boolean)
		For Each collider2D As Collider2D In MyBase.GetComponents(Of Collider2D)()
			collider2D.enabled = enabled
		Next
	End Sub

	' Token: 0x06002603 RID: 9731 RVA: 0x00163B20 File Offset: 0x00161F20
	Public Sub UpdateParentTransform(mouseTransform As Transform)
		MyBase.transform.position = If((mouseTransform.localScale.x <> 1F), Me.flippedRoot.position, Me.root.position)
	End Sub

	' Token: 0x04002E86 RID: 11910
	<SerializeField()>
	Private root As Transform

	' Token: 0x04002E87 RID: 11911
	<SerializeField()>
	Private flippedRoot As Transform

	' Token: 0x04002E88 RID: 11912
	Private damageDealer As DamageDealer
End Class
