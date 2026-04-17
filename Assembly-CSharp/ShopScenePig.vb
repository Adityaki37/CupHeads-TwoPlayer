Imports System
Imports UnityEngine

' Token: 0x02000B09 RID: 2825
Public Class ShopScenePig
	Inherits AbstractMonoBehaviour

	' Token: 0x06004486 RID: 17542 RVA: 0x00244010 File Offset: 0x00242410
	Private Sub OnIdleLoop()
		Me.idleLoops += 1
		If Me.idleLoops >= Me.idleLoopsMax Then
			MyBase.animator.SetTrigger("OnClock")
			Me.idleLoopsMax = Global.UnityEngine.Random.Range(20, 35)
			Me.idleLoops = 0
		End If
	End Sub

	' Token: 0x06004487 RID: 17543 RVA: 0x00244062 File Offset: 0x00242462
	Public Sub OnStart()
		AudioManager.Play("shop_pig_welcome")
		MyBase.animator.Play("Welcome")
	End Sub

	' Token: 0x06004488 RID: 17544 RVA: 0x0024407E File Offset: 0x0024247E
	Public Sub OnPurchase()
		AudioManager.Play("shop_pig_nod")
		MyBase.animator.Play("Nod")
	End Sub

	' Token: 0x06004489 RID: 17545 RVA: 0x0024409A File Offset: 0x0024249A
	Public Sub OnExit()
		AudioManager.Play("shop_pig_bye")
		MyBase.animator.Play("Bye")
	End Sub

	' Token: 0x04004A2C RID: 18988
	Private Const CLOCK_LOOPS_MIN As Integer = 20

	' Token: 0x04004A2D RID: 18989
	Private Const CLOCK_LOOPS_MAX As Integer = 35

	' Token: 0x04004A2E RID: 18990
	Private idleLoopsMax As Integer = 35

	' Token: 0x04004A2F RID: 18991
	Private idleLoops As Integer
End Class
