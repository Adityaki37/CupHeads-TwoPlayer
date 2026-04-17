Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x0200081A RID: 2074
Public Class TrainLevelGhostCannonGhostSkull
	Inherits AbstractProjectile

	' Token: 0x0600301C RID: 12316 RVA: 0x001C6930 File Offset: 0x001C4D30
	Public Function Create(pos As Vector3, speed As Single) As TrainLevelGhostCannonGhostSkull
		Dim trainLevelGhostCannonGhostSkull As TrainLevelGhostCannonGhostSkull = Global.UnityEngine.[Object].Instantiate(Of TrainLevelGhostCannonGhostSkull)(Me)
		trainLevelGhostCannonGhostSkull.transform.position = pos
		trainLevelGhostCannonGhostSkull.maxSpeed = speed
		Return trainLevelGhostCannonGhostSkull
	End Function

	' Token: 0x1700041F RID: 1055
	' (get) Token: 0x0600301D RID: 12317 RVA: 0x001C6958 File Offset: 0x001C4D58
	Protected Overrides ReadOnly Property DestroyLifetime As Single
		Get
			Return 1000F
		End Get
	End Property

	' Token: 0x0600301E RID: 12318 RVA: 0x001C695F File Offset: 0x001C4D5F
	Protected Overrides Sub Start()
		MyBase.Start()
		Me.SetParryable(True)
		MyBase.StartCoroutine(Me.speed_cr())
	End Sub

	' Token: 0x0600301F RID: 12319 RVA: 0x001C697C File Offset: 0x001C4D7C
	Protected Overrides Sub Update()
		MyBase.Update()
		If Not MyBase.dead AndAlso MyBase.transform.position.y < -325F Then
			Me.Die()
		End If
		MyBase.transform.AddPosition(0F, -Me.speed * CupheadTime.Delta, 0F)
	End Sub

	' Token: 0x06003020 RID: 12320 RVA: 0x001C69E4 File Offset: 0x001C4DE4
	Protected Overrides Sub OnCollision(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollision(hit, phase)
		If phase = CollisionPhase.Enter Then
			If hit.tag = "ParrySwitch" Then
				Dim component As ParrySwitch = hit.GetComponent(Of ParrySwitch)()
				If component.name <> "Right" AndAlso component.name <> "Left" Then
					Return
				End If
				component.ActivateFromOtherSource()
				Me.Die()
			ElseIf hit.name = "HandCar" Then
				Me.Die()
			End If
		End If
	End Sub

	' Token: 0x06003021 RID: 12321 RVA: 0x001C6A72 File Offset: 0x001C4E72
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If Me.damageDealer IsNot Nothing AndAlso phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x06003022 RID: 12322 RVA: 0x001C6A9C File Offset: 0x001C4E9C
	Private Iterator Function speed_cr() As IEnumerator
		Yield MyBase.TweenPositionY(MyBase.transform.position.y, MyBase.transform.position.y + 100F, 0.4F, EaseUtils.EaseType.easeOutCubic)
		Yield MyBase.StartCoroutine(Me.tweenSpeed_cr(0F, Me.maxSpeed, 0.4F, EaseUtils.EaseType.linear))
		Return
	End Function

	' Token: 0x06003023 RID: 12323 RVA: 0x001C6AB8 File Offset: 0x001C4EB8
	Private Iterator Function tweenSpeed_cr(start As Single, [end] As Single, time As Single, ease As EaseUtils.EaseType) As IEnumerator
		Dim t As Single = 0F
		While t < time
			Dim val As Single = t / time
			Me.speed = EaseUtils.Ease(ease, start, [end], val)
			t += CupheadTime.Delta
			Yield Nothing
		End While
		Me.speed = Me.maxSpeed
		Return
	End Function

	' Token: 0x040038EB RID: 14571
	Private Const DEATH_Y As Single = -325F

	' Token: 0x040038EC RID: 14572
	Private maxSpeed As Single

	' Token: 0x040038ED RID: 14573
	Private speed As Single
End Class
