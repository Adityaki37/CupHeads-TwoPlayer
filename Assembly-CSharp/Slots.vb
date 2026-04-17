Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020006AC RID: 1708
<Serializable()>
Public Class Slots
	' Token: 0x06002446 RID: 9286 RVA: 0x00154EAD File Offset: 0x001532AD
	Public Sub Init(parent As MonoBehaviour)
		Me.parent = parent
	End Sub

	' Token: 0x06002447 RID: 9287 RVA: 0x00154EB6 File Offset: 0x001532B6
	Public Sub Spin()
		Me.parent.StartCoroutine(Me.spin_cr())
	End Sub

	' Token: 0x06002448 RID: 9288 RVA: 0x00154ECC File Offset: 0x001532CC
	Private Iterator Function spin_cr() As IEnumerator
		Me.left.StartSpin()
		Yield CupheadTime.WaitForSeconds(Me.parent, 0.2F)
		Me.mid.StartSpin()
		Yield CupheadTime.WaitForSeconds(Me.parent, 0.2F)
		Me.right.StartSpin()
		Return
	End Function

	' Token: 0x06002449 RID: 9289 RVA: 0x00154EE7 File Offset: 0x001532E7
	Public Sub [Stop](mode As Slots.Mode)
		Me.parent.StartCoroutine(Me.stop_cr(mode))
	End Sub

	' Token: 0x0600244A RID: 9290 RVA: 0x00154EFC File Offset: 0x001532FC
	Private Iterator Function stop_cr(mode As Slots.Mode) As IEnumerator
		Me.left.StopSpin(mode)
		Yield CupheadTime.WaitForSeconds(Me.parent, 0.2F)
		Me.mid.StopSpin(mode)
		Yield CupheadTime.WaitForSeconds(Me.parent, 0.2F)
		Me.right.StopSpin(mode)
		Return
	End Function

	' Token: 0x0600244B RID: 9291 RVA: 0x00154F1E File Offset: 0x0015331E
	Public Sub StartFlash()
		Me.parent.StartCoroutine(Me.startFlash_cr())
	End Sub

	' Token: 0x0600244C RID: 9292 RVA: 0x00154F34 File Offset: 0x00153334
	Private Iterator Function startFlash_cr() As IEnumerator
		Me.left.Flash()
		Yield CupheadTime.WaitForSeconds(Me.parent, 0.2F)
		Me.mid.Flash()
		Yield CupheadTime.WaitForSeconds(Me.parent, 0.2F)
		Me.right.Flash()
		Return
	End Function

	' Token: 0x0600244D RID: 9293 RVA: 0x00154F4F File Offset: 0x0015334F
	Public Sub OnDestroy()
		Me.left = Nothing
		Me.mid = Nothing
		Me.right = Nothing
	End Sub

	' Token: 0x04002D02 RID: 11522
	Private Const DELAY As Single = 0.2F

	' Token: 0x04002D03 RID: 11523
	<SerializeField()>
	Private left As FrogsLevelMorphedSlot

	' Token: 0x04002D04 RID: 11524
	<SerializeField()>
	Private mid As FrogsLevelMorphedSlot

	' Token: 0x04002D05 RID: 11525
	<SerializeField()>
	Private right As FrogsLevelMorphedSlot

	' Token: 0x04002D06 RID: 11526
	Private parent As MonoBehaviour

	' Token: 0x020006AD RID: 1709
	Public Enum Mode
		' Token: 0x04002D08 RID: 11528
		Snake
		' Token: 0x04002D09 RID: 11529
		Tiger
		' Token: 0x04002D0A RID: 11530
		Bison
		' Token: 0x04002D0B RID: 11531
		Oni
	End Enum
End Class
