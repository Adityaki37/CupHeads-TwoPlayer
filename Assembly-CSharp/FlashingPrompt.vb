Imports System
Imports UnityEngine

' Token: 0x0200045C RID: 1116
Public Class FlashingPrompt
	Inherits AbstractMonoBehaviour

	' Token: 0x170002A9 RID: 681
	' (get) Token: 0x060010ED RID: 4333 RVA: 0x000A2575 File Offset: 0x000A0975
	Protected Overridable ReadOnly Property ShouldShow As Boolean
		Get
			Return True
		End Get
	End Property

	' Token: 0x060010EE RID: 4334 RVA: 0x000A2578 File Offset: 0x000A0978
	Private Sub Update()
		If Me.ShouldShow Then
			Me.flashTimer = (Me.flashTimer + CupheadTime.Delta) Mod 1.5F
			If Me.child IsNot Nothing Then
				Me.child.SetActive(Me.flashTimer < 0.75F)
			Else
				Me.childGroup.alpha = If((Me.flashTimer >= 0.75F), 0F, 1F)
			End If
		Else
			If Me.child IsNot Nothing Then
				Me.child.SetActive(False)
			Else
				Me.childGroup.alpha = 0F
			End If
			Me.flashTimer = 0F
		End If
	End Sub

	' Token: 0x04001A58 RID: 6744
	Private Const FLASH_TIME As Single = 0.75F

	' Token: 0x04001A59 RID: 6745
	Private flashTimer As Single

	' Token: 0x04001A5A RID: 6746
	<SerializeField()>
	Private child As GameObject

	' Token: 0x04001A5B RID: 6747
	<SerializeField()>
	Private childGroup As CanvasGroup
End Class
