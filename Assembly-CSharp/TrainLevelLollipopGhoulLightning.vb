Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x0200081F RID: 2079
Public Class TrainLevelLollipopGhoulLightning
	Inherits AbstractCollidableObject

	' Token: 0x06003042 RID: 12354 RVA: 0x001C7704 File Offset: 0x001C5B04
	Private Sub Start()
		MyBase.StartCoroutine(Me.start_cr())
		Me.damageDealer = DamageDealer.NewEnemy(0.2F)
	End Sub

	' Token: 0x06003043 RID: 12355 RVA: 0x001C7723 File Offset: 0x001C5B23
	Private Sub Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x06003044 RID: 12356 RVA: 0x001C773B File Offset: 0x001C5B3B
	Public Sub [End]()
		MyBase.StartCoroutine(Me.end_cr())
	End Sub

	' Token: 0x06003045 RID: 12357 RVA: 0x001C774A File Offset: 0x001C5B4A
	Private Sub GoAway()
		Me.StopAllCoroutines()
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub

	' Token: 0x06003046 RID: 12358 RVA: 0x001C775D File Offset: 0x001C5B5D
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		If Me.damageDealer Is Nothing Then
			Return
		End If
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x06003047 RID: 12359 RVA: 0x001C7780 File Offset: 0x001C5B80
	Private Iterator Function start_cr() As IEnumerator
		MyBase.animator.SetTrigger("OnStart")
		Yield MyBase.animator.WaitForAnimationToStart(Me, "Loop", False)
		MyBase.animator.SetBool("isFX", True)
		MyBase.animator.SetTrigger("OnDustStart")
		Return
	End Function

	' Token: 0x06003048 RID: 12360 RVA: 0x001C779C File Offset: 0x001C5B9C
	Private Iterator Function end_cr() As IEnumerator
		MyBase.animator.SetTrigger("OnEnd")
		MyBase.animator.SetBool("isFX", False)
		Yield MyBase.animator.WaitForAnimationToStart(Me, "Init", False)
		MyBase.animator.SetTrigger("OnDustEnd")
		Yield MyBase.animator.WaitForAnimationToStart(Me, "Init", 2, False)
		Me.GoAway()
		Return
	End Function

	' Token: 0x04003901 RID: 14593
	<SerializeField()>
	Private spark1 As Transform

	' Token: 0x04003902 RID: 14594
	<SerializeField()>
	Private spark2 As Transform

	' Token: 0x04003903 RID: 14595
	Private damageDealer As DamageDealer
End Class
