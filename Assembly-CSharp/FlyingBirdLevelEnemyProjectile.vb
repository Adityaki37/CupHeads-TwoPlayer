Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x0200061D RID: 1565
Public Class FlyingBirdLevelEnemyProjectile
	Inherits AbstractProjectile

	' Token: 0x06001FD7 RID: 8151 RVA: 0x00124380 File Offset: 0x00122780
	Public Overridable Function Create(time As Single, height As Single, pos As Vector2) As AbstractProjectile
		Dim flyingBirdLevelEnemyProjectile As FlyingBirdLevelEnemyProjectile = TryCast(Me.Create(pos, 0F), FlyingBirdLevelEnemyProjectile)
		flyingBirdLevelEnemyProjectile.time = time
		flyingBirdLevelEnemyProjectile.height = height
		flyingBirdLevelEnemyProjectile.DamagesType.OnlyPlayer()
		flyingBirdLevelEnemyProjectile.CollisionDeath.OnlyPlayer()
		flyingBirdLevelEnemyProjectile.Init()
		Return flyingBirdLevelEnemyProjectile
	End Function

	' Token: 0x06001FD8 RID: 8152 RVA: 0x001243CB File Offset: 0x001227CB
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x06001FD9 RID: 8153 RVA: 0x001243E9 File Offset: 0x001227E9
	Private Sub Init()
		MyBase.StartCoroutine(Me.go_cr())
	End Sub

	' Token: 0x06001FDA RID: 8154 RVA: 0x001243F8 File Offset: 0x001227F8
	Private Sub Check()
		If MyBase.transform.position.y < -460F Then
			Me.StopAllCoroutines()
			Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
		End If
	End Sub

	' Token: 0x06001FDB RID: 8155 RVA: 0x00124434 File Offset: 0x00122834
	Private Iterator Function go_cr() As IEnumerator
		Dim start As Single = MyBase.transform.position.y
		Dim [end] As Single = start + Me.height
		Dim t As Single = 0F
		Dim speed As Single = 0F
		t = 0F
		While t < Me.time
			Dim val As Single = t / Me.time
			MyBase.transform.SetPosition(Nothing, New Single?(EaseUtils.Ease(EaseUtils.EaseType.easeOutSine, start, [end], val)), Nothing)
			t += CupheadTime.Delta
			Yield Nothing
		End While
		t = 0F
		While t < Me.time
			Dim val2 As Single = t / Me.time
			Dim last As Single = MyBase.transform.position.y
			MyBase.transform.SetPosition(Nothing, New Single?(EaseUtils.Ease(EaseUtils.EaseType.easeInSine, [end], start, val2)), Nothing)
			speed = MyBase.transform.position.y - last
			t += CupheadTime.Delta
			Yield Nothing
		End While
		While True
			Me.Check()
			MyBase.transform.AddPosition(0F, speed * CupheadTime.GlobalSpeed, 0F)
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x04002855 RID: 10325
	Private time As Single

	' Token: 0x04002856 RID: 10326
	Private height As Single
End Class
