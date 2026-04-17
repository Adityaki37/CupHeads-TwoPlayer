Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000827 RID: 2087
Public Class TrainLevelPumpkinProjectile
	Inherits AbstractProjectile

	' Token: 0x06003078 RID: 12408 RVA: 0x001C8DC2 File Offset: 0x001C71C2
	Protected Overrides Sub Start()
		MyBase.Start()
		Me.SetParryable(True)
		MyBase.StartCoroutine(Me.float_cr())
	End Sub

	' Token: 0x06003079 RID: 12409 RVA: 0x001C8DE0 File Offset: 0x001C71E0
	Protected Overrides Sub Update()
		MyBase.Update()
		If Not Me.hasDied AndAlso MyBase.transform.position.y < -325F Then
			Me.Die()
		End If
	End Sub

	' Token: 0x0600307A RID: 12410 RVA: 0x001C8E24 File Offset: 0x001C7224
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

	' Token: 0x0600307B RID: 12411 RVA: 0x001C8EB2 File Offset: 0x001C72B2
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If Me.damageDealer IsNot Nothing AndAlso phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x0600307C RID: 12412 RVA: 0x001C8EDB File Offset: 0x001C72DB
	Public Sub Drop()
		MyBase.transform.SetParent(Nothing)
		Me.StopAllCoroutines()
		MyBase.animator.Play("Fall")
		MyBase.StartCoroutine(Me.drop_cr())
	End Sub

	' Token: 0x0600307D RID: 12413 RVA: 0x001C8F0C File Offset: 0x001C730C
	Protected Overrides Sub Die()
		If Me.hasDied Then
			Return
		End If
		Me.hasDied = True
		Me.StopAllCoroutines()
		MyBase.Die()
	End Sub

	' Token: 0x0600307E RID: 12414 RVA: 0x001C8F30 File Offset: 0x001C7330
	Private Iterator Function float_cr() As IEnumerator
		Dim top As Single = MyBase.transform.localPosition.y
		Dim bottom As Single = top - 20F
		Dim time As Single = 0.4F
		While True
			Yield MyBase.TweenLocalPositionY(top, bottom, time, EaseUtils.EaseType.easeInOutSine)
			Yield MyBase.TweenLocalPositionY(bottom, top, time, EaseUtils.EaseType.easeInOutSine)
		End While
		Return
	End Function

	' Token: 0x0600307F RID: 12415 RVA: 0x001C8F4C File Offset: 0x001C734C
	Private Iterator Function drop_cr() As IEnumerator
		Dim top As Single = MyBase.transform.position.y
		Yield MyBase.TweenPositionY(top, -340F, Me.fallTime, EaseUtils.EaseType.easeInSine)
		Me.Die()
		Return
	End Function

	' Token: 0x04003925 RID: 14629
	Private Const DEATH_Y As Single = -325F

	' Token: 0x04003926 RID: 14630
	Public fallTime As Single

	' Token: 0x04003927 RID: 14631
	Private hasDied As Boolean
End Class
