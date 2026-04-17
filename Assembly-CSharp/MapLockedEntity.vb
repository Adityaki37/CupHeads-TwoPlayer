Imports System

' Token: 0x02000942 RID: 2370
Public Class MapLockedEntity
	Inherits AbstractMapInteractiveEntity

	' Token: 0x0600376E RID: 14190 RVA: 0x001FDEE4 File Offset: 0x001FC2E4
	Protected Overrides Sub Reset()
		MyBase.Reset()
		Me.dialogueProperties = New AbstractUIInteractionDialogue.Properties("???")
	End Sub
End Class
