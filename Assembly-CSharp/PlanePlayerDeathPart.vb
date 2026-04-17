Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000A9A RID: 2714
Public Class PlanePlayerDeathPart
	Inherits AbstractMonoBehaviour

	' Token: 0x0600410D RID: 16653 RVA: 0x002357E8 File Offset: 0x00233BE8
	Public Function CreatePart(player As PlayerId, position As Vector3) As PlanePlayerDeathPart
		Dim planePlayerDeathPart As PlanePlayerDeathPart = Me.InstantiatePrefab(Of PlanePlayerDeathPart)()
		planePlayerDeathPart.transform.position = position
		planePlayerDeathPart.animator.SetInteger("Player", CInt(player))
		Return planePlayerDeathPart
	End Function

	' Token: 0x0600410E RID: 16654 RVA: 0x0023581A File Offset: 0x00233C1A
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.velocity = New Vector2(Global.UnityEngine.Random.Range(-500F, 500F), Global.UnityEngine.Random.Range(500F, 1000F))
		MyBase.StartCoroutine(Me.move_cr())
	End Sub

	' Token: 0x0600410F RID: 16655 RVA: 0x00235858 File Offset: 0x00233C58
	Private Iterator Function move_cr() As IEnumerator
		While True
			MyBase.transform.position += (Me.velocity + New Vector2(-300F, Me.accumulatedGravity)) * MyBase.LocalDeltaTime
			Me.accumulatedGravity += -6000F * MyBase.LocalDeltaTime
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06004110 RID: 16656 RVA: 0x00235874 File Offset: 0x00233C74
	Public Sub GameOverUnpause()
		MyBase.animator.enabled = True
		Dim component As AnimationHelper = MyBase.GetComponent(Of AnimationHelper)()
		component.IgnoreGlobal = True
		Me.ignoreGlobalTime = True
		MyBase.enabled = True
	End Sub

	' Token: 0x040047A5 RID: 18341
	Private Const VELOCITY_X_MIN As Single = -500F

	' Token: 0x040047A6 RID: 18342
	Private Const VELOCITY_X_MAX As Single = 500F

	' Token: 0x040047A7 RID: 18343
	Private Const VELOCITY_Y_MIN As Single = 500F

	' Token: 0x040047A8 RID: 18344
	Private Const VELOCITY_Y_MAX As Single = 1000F

	' Token: 0x040047A9 RID: 18345
	Private Const GRAVITY As Single = -6000F

	' Token: 0x040047AA RID: 18346
	Private velocity As Vector2

	' Token: 0x040047AB RID: 18347
	Private accumulatedGravity As Single
End Class
