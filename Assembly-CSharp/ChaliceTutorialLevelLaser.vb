Imports System
Imports UnityEngine

' Token: 0x02000526 RID: 1318
Public Class ChaliceTutorialLevelLaser
	Inherits AbstractCollidableObject

	' Token: 0x060017B9 RID: 6073 RVA: 0x000D5CC8 File Offset: 0x000D40C8
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		Me.level.resetParryables = True
		AudioManager.Play("sfx_rip_fail")
		Me.hitAnimator.transform.position = New Vector3(MyBase.transform.position.x + Me.coll.bounds.size.x / 2F, hit.transform.position.y + 100F)
		Me.hitAnimator.transform.eulerAngles = New Vector3(0F, 0F, CSng(Global.UnityEngine.Random.Range(0, 360)))
		Me.hitAnimator.Play("Hit")
	End Sub

	' Token: 0x060017BA RID: 6074 RVA: 0x000D5D91 File Offset: 0x000D4191
	Private Sub Enabled()
		MyBase.animator.SetBool("On", True)
		Me.coll.enabled = True
	End Sub

	' Token: 0x060017BB RID: 6075 RVA: 0x000D5DB0 File Offset: 0x000D41B0
	Private Sub Disabled()
		MyBase.animator.SetBool("On", False)
		Me.coll.enabled = False
	End Sub

	' Token: 0x060017BC RID: 6076 RVA: 0x000D5DCF File Offset: 0x000D41CF
	Private Sub Update()
		If Not Me.parryable.isDeactivated Then
			Me.Enabled()
		Else
			Me.Disabled()
		End If
	End Sub

	' Token: 0x060017BD RID: 6077 RVA: 0x000D5DF2 File Offset: 0x000D41F2
	Private Sub AniEvent_SFX_Open()
		AudioManager.Play("sfx_rip_open")
	End Sub

	' Token: 0x040020E7 RID: 8423
	<SerializeField()>
	Private level As ChaliceTutorialLevel

	' Token: 0x040020E8 RID: 8424
	<SerializeField()>
	Private parryable As ChaliceTutorialLevelParryable

	' Token: 0x040020E9 RID: 8425
	<SerializeField()>
	Private coll As Collider2D

	' Token: 0x040020EA RID: 8426
	<SerializeField()>
	Private hitAnimator As Animator
End Class
