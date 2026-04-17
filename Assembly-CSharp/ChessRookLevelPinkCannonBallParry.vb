Imports System
Imports UnityEngine

' Token: 0x02000552 RID: 1362
Public Class ChessRookLevelPinkCannonBallParry
	Inherits AbstractProjectile

	' Token: 0x17000344 RID: 836
	' (get) Token: 0x06001957 RID: 6487 RVA: 0x000E5E4C File Offset: 0x000E424C
	Public Overrides ReadOnly Property ParryMeterMultiplier As Single
		Get
			Return 0F
		End Get
	End Property

	' Token: 0x06001958 RID: 6488 RVA: 0x000E5E53 File Offset: 0x000E4253
	Protected Overrides Sub RandomizeVariant()
	End Sub

	' Token: 0x06001959 RID: 6489 RVA: 0x000E5E55 File Offset: 0x000E4255
	Protected Overrides Sub SetTrigger(trigger As String)
	End Sub

	' Token: 0x0600195A RID: 6490 RVA: 0x000E5E57 File Offset: 0x000E4257
	Public Overrides Sub OnParry(player As AbstractPlayerController)
		Me.main.GotParried(player)
	End Sub

	' Token: 0x04002275 RID: 8821
	<SerializeField()>
	Private main As ChessRookLevelPinkCannonBall
End Class
