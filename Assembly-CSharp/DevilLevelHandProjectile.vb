Imports System
Imports System.Collections

' Token: 0x02000579 RID: 1401
Public Class DevilLevelHandProjectile
	Inherits BasicProjectile

	' Token: 0x06001AAD RID: 6829 RVA: 0x000F4ADF File Offset: 0x000F2EDF
	Protected Overrides Sub Start()
		MyBase.Start()
		MyBase.StartCoroutine(Me.animation_cr())
	End Sub

	' Token: 0x06001AAE RID: 6830 RVA: 0x000F4AF4 File Offset: 0x000F2EF4
	Private Iterator Function animation_cr() As IEnumerator
		Me.move = False
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Projectile", False, True)
		Me.move = True
		Return
	End Function
End Class
