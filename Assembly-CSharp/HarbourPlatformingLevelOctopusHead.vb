Imports System
Imports UnityEngine

' Token: 0x020008D0 RID: 2256
Public Class HarbourPlatformingLevelOctopusHead
	Inherits LevelPlatform

	' Token: 0x060034D1 RID: 13521 RVA: 0x001EB727 File Offset: 0x001E9B27
	Public Overrides Sub AddChild(player As Transform)
		MyBase.AddChild(player)
		Me.octopus.animator.SetBool("playerOn", True)
	End Sub

	' Token: 0x060034D2 RID: 13522 RVA: 0x001EB746 File Offset: 0x001E9B46
	Public Overrides Sub OnPlayerExit(player As Transform)
		MyBase.OnPlayerExit(player)
		If MyBase.transform.childCount <= 1 Then
			Me.octopus.animator.SetBool("playerOn", False)
		End If
	End Sub

	' Token: 0x04003CFB RID: 15611
	<SerializeField()>
	Private octopus As HarbourPlatformingLevelOctopus
End Class
