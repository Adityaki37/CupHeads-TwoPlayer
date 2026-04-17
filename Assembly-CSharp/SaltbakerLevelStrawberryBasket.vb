Imports System
Imports UnityEngine

' Token: 0x020007D7 RID: 2007
Public Class SaltbakerLevelStrawberryBasket
	Inherits MonoBehaviour

	' Token: 0x06002DD1 RID: 11729 RVA: 0x001B0884 File Offset: 0x001AEC84
	Public Sub StartRunIn(sbOnLeft As Boolean)
		MyBase.transform.position = New Vector3(CSng(If((Not sbOnLeft), (Level.Current.Left - 300), (Level.Current.Right + 300))), MyBase.transform.position.y)
		MyBase.transform.localScale = New Vector3(CSng(If((Not sbOnLeft), (-1), 1)), 1F)
		Me.vel = (CSng((Level.Current.Left + Level.Current.Right)) / 2F + 80F * MyBase.transform.localScale.x - MyBase.transform.position.x) / 1.0416666F
		Me.anim.Play("RunIn")
		Me.moving = True
	End Sub

	' Token: 0x06002DD2 RID: 11730 RVA: 0x001B0970 File Offset: 0x001AED70
	Public Sub GetGrabbed()
		Me.moving = False
	End Sub

	' Token: 0x06002DD3 RID: 11731 RVA: 0x001B097C File Offset: 0x001AED7C
	Public Sub StartRunOut()
		Me.anim.Play("RunOut")
		Me.anim.Update(0F)
		Me.SFX_SALTBAKER_P1_StrawberryBag_CryingRunOff()
		Me.moving = True
		Me.vel *= 0.8F
	End Sub

	' Token: 0x06002DD4 RID: 11732 RVA: 0x001B09C8 File Offset: 0x001AEDC8
	Private Sub Update()
		If Me.moving Then
			MyBase.transform.position += Me.vel * Vector3.right * CupheadTime.Delta
			If Mathf.Abs(MyBase.transform.position.x) > 2000F Then
				Me.anim.StopPlayback()
				Me.moving = False
			End If
		End If
	End Sub

	' Token: 0x06002DD5 RID: 11733 RVA: 0x001B0A4C File Offset: 0x001AEE4C
	Private Sub LateUpdate()
		Me.rend.enabled = Me.saltbakerTopperRend.sprite Is Nothing OrElse Me.saltbaker.animator.GetCurrentAnimatorStateInfo(0).IsName("PhaseOneToTwo")
	End Sub

	' Token: 0x06002DD6 RID: 11734 RVA: 0x001B0A9B File Offset: 0x001AEE9B
	Private Sub SFX_SALTBAKER_P1_StrawberryBag_CryingRunOff()
		AudioManager.Play("sfx_dlc_saltbaker_p1_strawberrybag_cryingrunoff")
	End Sub

	' Token: 0x04003657 RID: 13911
	Private Const GRAB_OFFSET As Single = 80F

	' Token: 0x04003658 RID: 13912
	<SerializeField()>
	Private anim As Animator

	' Token: 0x04003659 RID: 13913
	<SerializeField()>
	Private rend As SpriteRenderer

	' Token: 0x0400365A RID: 13914
	<SerializeField()>
	Private saltbakerTopperRend As SpriteRenderer

	' Token: 0x0400365B RID: 13915
	<SerializeField()>
	Private saltbaker As SaltbakerLevelSaltbaker

	' Token: 0x0400365C RID: 13916
	Private moving As Boolean

	' Token: 0x0400365D RID: 13917
	Private vel As Single
End Class
