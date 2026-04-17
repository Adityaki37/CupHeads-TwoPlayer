Imports System
Imports UnityEngine

' Token: 0x02000A74 RID: 2676
Public Class WeaponCrackshotExProjectileChild
	Inherits BasicProjectile

	' Token: 0x06003FF2 RID: 16370 RVA: 0x0022E440 File Offset: 0x0022C840
	Protected Overrides Sub Start()
		MyBase.Start()
		MyBase.animator.SetBool("IsB", Rand.Bool())
		MyBase.animator.Play(If((Not Rand.Bool()), "CometStartA", "CometStartB"))
		Me.damageDealer.isDLCWeapon = True
	End Sub

	' Token: 0x06003FF3 RID: 16371 RVA: 0x0022E498 File Offset: 0x0022C898
	Protected Overrides Sub Die()
		MyBase.Die()
		If MyBase.animator.GetCurrentAnimatorStateInfo(0).IsTag("Comet") Then
			MyBase.animator.Play(If((Not Rand.Bool()), "ImpactCometB", "ImpactCometA"))
		Else
			MyBase.animator.Play(If((Not Rand.Bool()), "ImpactSmallB", "ImpactSmallA"))
		End If
	End Sub

	' Token: 0x06003FF4 RID: 16372 RVA: 0x0022E516 File Offset: 0x0022C916
	Private Sub OnEffectComplete()
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub
End Class
