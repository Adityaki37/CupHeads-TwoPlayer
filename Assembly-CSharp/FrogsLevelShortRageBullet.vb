Imports System

' Token: 0x020006BD RID: 1725
Public Class FrogsLevelShortRageBullet
	Inherits BasicProjectile

	' Token: 0x0600249B RID: 9371 RVA: 0x001574C0 File Offset: 0x001558C0
	Protected Overrides Sub Die()
		If Not MyBase.CanParry Then
			Return
		End If
		MyBase.Die()
		Me.move = True
	End Sub
End Class
