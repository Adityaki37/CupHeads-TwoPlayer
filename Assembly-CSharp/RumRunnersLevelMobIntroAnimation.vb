Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000796 RID: 1942
Public Class RumRunnersLevelMobIntroAnimation
	Inherits MonoBehaviour

	' Token: 0x170003F7 RID: 1015
	' (get) Token: 0x06002B16 RID: 11030 RVA: 0x0019228B File Offset: 0x0019068B
	' (set) Token: 0x06002B17 RID: 11031 RVA: 0x00192293 File Offset: 0x00190693
	Public Property bugGirlDamage As Single

	' Token: 0x06002B18 RID: 11032 RVA: 0x0019229C File Offset: 0x0019069C
	Private Sub Start()
		If Level.Current.mode = Level.Mode.Easy Then
			Me.grub.SetActive(False)
		End If
	End Sub

	' Token: 0x06002B19 RID: 11033 RVA: 0x001922BC File Offset: 0x001906BC
	Private Iterator Function bugWalk() As IEnumerator
		Dim walkSpeed As Single = Me.bugGirlWalkDistance / Me.bugGirlWalkDuration
		While True
			Yield Nothing
			Me.bugGirlTransform.position = Me.bugGirlTransform.position + New Vector3(walkSpeed * CupheadTime.Delta, 0F)
		End While
		Return
	End Function

	' Token: 0x06002B1A RID: 11034 RVA: 0x001922D8 File Offset: 0x001906D8
	Private Iterator Function timeout_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, RumRunnersLevelMobIntroAnimation.IntroTimeoutDuration)
		MyBase.gameObject.SetActive(False)
		Return
	End Function

	' Token: 0x06002B1B RID: 11035 RVA: 0x001922F3 File Offset: 0x001906F3
	Public Sub StartBugWalk()
		Me.bugWalkCoroutine = MyBase.StartCoroutine(Me.bugWalk())
	End Sub

	' Token: 0x06002B1C RID: 11036 RVA: 0x00192307 File Offset: 0x00190707
	Public Sub StopBugWalk()
		MyBase.StopCoroutine(Me.bugWalkCoroutine)
	End Sub

	' Token: 0x06002B1D RID: 11037 RVA: 0x00192318 File Offset: 0x00190718
	Public Sub BarrelExit()
		Me.barrelAnimator.SetTrigger("Exit")
		Dim component As SpriteRenderer = Me.barrelAnimator.GetComponent(Of SpriteRenderer)()
		component.sortingLayerName = "Foreground"
		component.sortingOrder = 100
		MyBase.StartCoroutine(Me.timeout_cr())
	End Sub

	' Token: 0x040033D4 RID: 13268
	Private Shared IntroTimeoutDuration As Single = 2F

	' Token: 0x040033D5 RID: 13269
	<SerializeField()>
	Private bugGirlTransform As Transform

	' Token: 0x040033D6 RID: 13270
	<SerializeField()>
	Private bugGirlWalkDistance As Single

	' Token: 0x040033D7 RID: 13271
	<SerializeField()>
	Private bugGirlWalkDuration As Single

	' Token: 0x040033D8 RID: 13272
	<SerializeField()>
	Private barrelAnimator As Animator

	' Token: 0x040033D9 RID: 13273
	<SerializeField()>
	Private grub As GameObject

	' Token: 0x040033DB RID: 13275
	Private bugWalkCoroutine As Coroutine
End Class
