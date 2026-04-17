Imports System

' Token: 0x02000AA2 RID: 2722
Public Class PlanePlayerParryEffect
	Inherits AbstractParryEffect

	' Token: 0x170005B5 RID: 1461
	' (get) Token: 0x06004148 RID: 16712 RVA: 0x00236954 File Offset: 0x00234D54
	Protected Overrides ReadOnly Property IsHit As Boolean
		Get
			Return False
		End Get
	End Property

	' Token: 0x06004149 RID: 16713 RVA: 0x00236957 File Offset: 0x00234D57
	Protected Overrides Sub SetPlayer(player As AbstractPlayerController)
		MyBase.SetPlayer(player)
		Me.planePlayer = TryCast(player, PlanePlayerController)
	End Sub

	' Token: 0x0600414A RID: 16714 RVA: 0x0023696C File Offset: 0x00234D6C
	Protected Overrides Sub OnSuccess()
		MyBase.OnSuccess()
		Me.planePlayer.parryController.OnParrySuccess()
	End Sub

	' Token: 0x040047DF RID: 18399
	Private planePlayer As PlanePlayerController
End Class
