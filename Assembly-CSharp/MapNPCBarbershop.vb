Imports System
Imports UnityEngine

' Token: 0x02000948 RID: 2376
Public Class MapNPCBarbershop
	Inherits AbstractMonoBehaviour

	' Token: 0x0600377D RID: 14205 RVA: 0x001FE3EE File Offset: 0x001FC7EE
	Private Sub Start()
		If Dialoguer.GetGlobalFloat(Me.dialoguerVariableID) > 0F Then
			Me.NowFour()
			Me.CleanUp()
		End If
	End Sub

	' Token: 0x0600377E RID: 14206 RVA: 0x001FE411 File Offset: 0x001FC811
	Protected Overrides Sub OnDrawGizmosSelected()
		MyBase.OnDrawGizmosSelected()
		Gizmos.DrawWireSphere(Me.fourPosition + MyBase.transform.parent.position, 1F)
	End Sub

	' Token: 0x0600377F RID: 14207 RVA: 0x001FE43E File Offset: 0x001FC83E
	Public Sub NowFour()
		MyBase.animator.runtimeAnimatorController = Me.fourAnimatorController
		MyBase.transform.localPosition = Me.fourPosition
	End Sub

	' Token: 0x06003780 RID: 14208 RVA: 0x001FE462 File Offset: 0x001FC862
	Public Sub CleanUp()
		If Me.mapDialogueInteraction Then
			Global.UnityEngine.[Object].Destroy(Me.mapDialogueInteraction)
		End If
		If Me.mapNPCDistanceAnimator Then
			Global.UnityEngine.[Object].Destroy(Me.mapNPCDistanceAnimator)
		End If
	End Sub

	' Token: 0x06003781 RID: 14209 RVA: 0x001FE49A File Offset: 0x001FC89A
	Private Sub SongLooped()
	End Sub

	' Token: 0x06003782 RID: 14210 RVA: 0x001FE49C File Offset: 0x001FC89C
	Private Sub Show()
		MyBase.animator.SetTrigger("show")
	End Sub

	' Token: 0x04003F91 RID: 16273
	<SerializeField()>
	Private fourAnimatorController As RuntimeAnimatorController

	' Token: 0x04003F92 RID: 16274
	<SerializeField()>
	Private fourPosition As Vector3

	' Token: 0x04003F93 RID: 16275
	<SerializeField()>
	Protected mapNPCDistanceAnimator As MapNPCLostBarbershop

	' Token: 0x04003F94 RID: 16276
	Public mapDialogueInteraction As MapDialogueInteraction

	' Token: 0x04003F95 RID: 16277
	<SerializeField()>
	Private dialoguerVariableID As Integer = 10
End Class
