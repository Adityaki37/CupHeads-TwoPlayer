Imports System
Imports UnityEngine

' Token: 0x020005EF RID: 1519
Public Class DragonLevelFire
	Inherits AbstractCollidableObject

	' Token: 0x06001E32 RID: 7730 RVA: 0x00115F24 File Offset: 0x00114324
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.damageDealer = DamageDealer.NewEnemy()
	End Sub

	' Token: 0x06001E33 RID: 7731 RVA: 0x00115F37 File Offset: 0x00114337
	Private Sub Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x06001E34 RID: 7732 RVA: 0x00115F4F File Offset: 0x0011434F
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If Me.damageDealer IsNot Nothing AndAlso phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x06001E35 RID: 7733 RVA: 0x00115F78 File Offset: 0x00114378
	Public Sub SetColliderEnabled(enabled As Boolean)
		For Each collider2D As Collider2D In MyBase.GetComponents(Of Collider2D)()
			collider2D.enabled = enabled
		Next
	End Sub

	' Token: 0x040026F7 RID: 9975
	Private damageDealer As DamageDealer

	' Token: 0x040026F8 RID: 9976
	Private localPosition As Vector3

	' Token: 0x040026F9 RID: 9977
	Private localScale As Vector3
End Class
