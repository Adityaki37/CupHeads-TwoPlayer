Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000836 RID: 2102
Public Class TutorialPlayerDeathEffect
	Inherits PlayerDeathEffect

	' Token: 0x060030BB RID: 12475 RVA: 0x001CB050 File Offset: 0x001C9450
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.tr = MyBase.transform
		Me.startPos = Me.tr.position
	End Sub

	' Token: 0x060030BC RID: 12476 RVA: 0x001CB075 File Offset: 0x001C9475
	Protected Overrides Sub Start()
		MyBase.Start()
		Me.Init()
	End Sub

	' Token: 0x060030BD RID: 12477 RVA: 0x001CB084 File Offset: 0x001C9484
	Private Sub Update()
		If Me.tr.localPosition.y >= 270F Then
			Me.tr.position = Me.startPos
		End If
	End Sub

	' Token: 0x060030BE RID: 12478 RVA: 0x001CB0BF File Offset: 0x001C94BF
	Protected Overrides Sub OnParrySwitch()
		MyBase.OnParrySwitch()
		If Me.parrySwitch.enabled Then
			MyBase.animator.SetTrigger("OnParryTutorial")
		End If
		Me.parrySwitch.enabled = False
	End Sub

	' Token: 0x060030BF RID: 12479 RVA: 0x001CB0F4 File Offset: 0x001C94F4
	Private Sub Init()
		Me.tr.position = Me.startPos
		Me.playerId = PlayerId.PlayerOne
		MyBase.animator.SetInteger("Mode", 0)
		MyBase.animator.SetBool("CanParry", True)
		Me.spriteRenderer = Me.cuphead
		Me.cuphead.gameObject.SetActive(True)
		Me.mugman.gameObject.SetActive(False)
		Me.parrySwitch.enabled = True
		Me.parrySwitch.gameObject.SetActive(True)
	End Sub

	' Token: 0x060030C0 RID: 12480 RVA: 0x001CB186 File Offset: 0x001C9586
	Protected Overrides Sub OnReviveParryAnimComplete()
		Me.StopAllCoroutines()
		MyBase.animator.Play("Level_Start")
		Me.exiting = False
		Me.Init()
		MyBase.StartCoroutine(MyBase.float_cr())
	End Sub

	' Token: 0x060030C1 RID: 12481 RVA: 0x001CB1B8 File Offset: 0x001C95B8
	Protected Overrides Iterator Function checkOutOfFrame_cr() As IEnumerator
		Yield Nothing
		Return
	End Function

	' Token: 0x060030C2 RID: 12482 RVA: 0x001CB1CC File Offset: 0x001C95CC
	Private Sub OnDestroy()
		Me.tr = Nothing
	End Sub

	' Token: 0x0400395E RID: 14686
	Protected startPos As Vector3

	' Token: 0x0400395F RID: 14687
	Private tr As Transform
End Class
