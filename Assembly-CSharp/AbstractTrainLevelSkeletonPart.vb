Imports System
Imports UnityEngine

' Token: 0x02000807 RID: 2055
Public Class AbstractTrainLevelSkeletonPart
	Inherits AbstractCollidableObject

	' Token: 0x06002F90 RID: 12176 RVA: 0x001C3A03 File Offset: 0x001C1E03
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.exploder = MyBase.GetComponent(Of LevelBossDeathExploder)()
		Me.damageDealer = DamageDealer.NewEnemy()
	End Sub

	' Token: 0x06002F91 RID: 12177 RVA: 0x001C3A24 File Offset: 0x001C1E24
	Public Sub SetPosition(position As TrainLevelSkeleton.Position)
		Select Case position
			Case TrainLevelSkeleton.Position.Right
				MyBase.transform.SetPosition(New Single?(470F), Nothing, Nothing)
			Case Else
				MyBase.transform.SetPosition(New Single?(0F), Nothing, Nothing)
			Case TrainLevelSkeleton.Position.Left
				MyBase.transform.SetPosition(New Single?(-470F), Nothing, Nothing)
		End Select
	End Sub

	' Token: 0x06002F92 RID: 12178 RVA: 0x001C3ACC File Offset: 0x001C1ECC
	Public Sub [In]()
		MyBase.animator.Play("In")
	End Sub

	' Token: 0x06002F93 RID: 12179 RVA: 0x001C3ADE File Offset: 0x001C1EDE
	Public Sub Out()
		MyBase.animator.SetTrigger("Out")
	End Sub

	' Token: 0x06002F94 RID: 12180 RVA: 0x001C3AF0 File Offset: 0x001C1EF0
	Public Sub Die()
		Me.exploder.StartExplosion()
		MyBase.animator.Play("Death")
	End Sub

	' Token: 0x06002F95 RID: 12181 RVA: 0x001C3B0D File Offset: 0x001C1F0D
	Public Sub EndDeath()
		Me.exploder.StopExplosions()
	End Sub

	' Token: 0x06002F96 RID: 12182 RVA: 0x001C3B1A File Offset: 0x001C1F1A
	Private Sub Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x06002F97 RID: 12183 RVA: 0x001C3B32 File Offset: 0x001C1F32
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If Me.damageDealer IsNot Nothing AndAlso phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x0400387E RID: 14462
	Public Const X As Single = 470F

	' Token: 0x0400387F RID: 14463
	Private exploder As LevelBossDeathExploder

	' Token: 0x04003880 RID: 14464
	Private damageDealer As DamageDealer
End Class
