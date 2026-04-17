Imports System
Imports UnityEngine
Imports UnityEngine.UI

' Token: 0x02000837 RID: 2103
Public Class TutorialShmupLevelParryNext
	Inherits AbstractCollidableObject

	' Token: 0x060030C4 RID: 12484 RVA: 0x001CB264 File Offset: 0x001C9664
	Private Sub Start()
		AddHandler Me.parrySwitch.OnActivate, AddressOf Me.SetNextParry
		If Me.startAsParry Then
			Me.image.enabled = True
			Me.parrySwitch.enabled = True
		Else
			Me.image.enabled = False
			Me.parrySwitch.enabled = False
		End If
	End Sub

	' Token: 0x060030C5 RID: 12485 RVA: 0x001CB2C8 File Offset: 0x001C96C8
	Private Sub SetNextParry()
		Me.nextSphere.parrySwitch.enabled = True
		Me.parrySwitch.enabled = False
		If Me.lastPlayerController IsNot Nothing Then
			Me.lastPlayerController.stats.OnParry(1F, True)
			Me.lastPlayerController = Nothing
		End If
		Me.image.enabled = False
		Me.nextSphere.image.enabled = True
	End Sub

	' Token: 0x060030C6 RID: 12486 RVA: 0x001CB340 File Offset: 0x001C9740
	Protected Overrides Sub OnCollisionOther(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionOther(hit, phase)
		If hit.transform AndAlso hit.transform.parent Then
			Me.lastPlayerController = hit.transform.parent.GetComponent(Of AbstractPlayerController)()
		End If
	End Sub

	' Token: 0x04003960 RID: 14688
	<SerializeField()>
	Private nextSphere As TutorialShmupLevelParryNext

	' Token: 0x04003961 RID: 14689
	<SerializeField()>
	Private image As Image

	' Token: 0x04003962 RID: 14690
	<SerializeField()>
	Private startAsParry As Boolean

	' Token: 0x04003963 RID: 14691
	<SerializeField()>
	Private parrySwitch As ParrySwitch

	' Token: 0x04003964 RID: 14692
	Private lastPlayerController As AbstractPlayerController
End Class
