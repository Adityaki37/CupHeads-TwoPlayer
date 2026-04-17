Imports System
Imports UnityEngine

' Token: 0x02000705 RID: 1797
Public Class OldManLevelLobberProjectile
	Inherits BasicProjectile

	' Token: 0x060026A3 RID: 9891 RVA: 0x00169B5C File Offset: 0x00167F5C
	Protected Overrides Sub OnCollision(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollision(hit, phase)
		If hit.GetComponent(Of LevelPlatform)() Then
			Me.Die()
			For Each abstractPlayerController As AbstractPlayerController In hit.GetComponentsInChildren(Of AbstractPlayerController)()
				If Not(abstractPlayerController Is Nothing) Then
					abstractPlayerController.transform.parent = Nothing
				End If
			Next
			hit.SetActive(False)
		End If
	End Sub

	' Token: 0x060026A4 RID: 9892 RVA: 0x00169BCA File Offset: 0x00167FCA
	Protected Overrides Sub Die()
		MyBase.Die()
		MyBase.GetComponent(Of SpriteRenderer)().enabled = False
	End Sub
End Class
