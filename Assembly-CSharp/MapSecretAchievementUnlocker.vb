Imports System
Imports UnityEngine

' Token: 0x0200095E RID: 2398
Public Class MapSecretAchievementUnlocker
	Inherits AbstractMonoBehaviour

	' Token: 0x060037FE RID: 14334 RVA: 0x00200F64 File Offset: 0x001FF364
	Private Sub OnTriggerEnter2D(collider As Collider2D)
		Dim component As MapPlayerController = collider.GetComponent(Of MapPlayerController)()
		OnlineManager.Instance.[Interface].UnlockAchievement(component.id, "FoundSecretPassage")
		If Me.updateDialogue Then
			Dialoguer.SetGlobalFloat(Me.dialoguerVariableID, 1F)
			PlayerData.SaveCurrentFile()
		End If
	End Sub

	' Token: 0x04003FE5 RID: 16357
	<SerializeField()>
	Private updateDialogue As Boolean = True

	' Token: 0x04003FE6 RID: 16358
	<SerializeField()>
	Private dialoguerVariableID As Integer = 7
End Class
