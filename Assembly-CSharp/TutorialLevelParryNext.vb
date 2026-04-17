Imports System
Imports UnityEngine

' Token: 0x02000834 RID: 2100
Public Class TutorialLevelParryNext
	Inherits AbstractCollidableObject

	' Token: 0x060030B3 RID: 12467 RVA: 0x001CA4C0 File Offset: 0x001C88C0
	Private Sub Start()
		AddHandler Me.parrySwitch.OnActivate, AddressOf Me.SetNextParry
		If Me.startAsParry Then
			Me.spriteRenderer.sprite = Me.parrySprite
			Me.spriteRenderer.sharedMaterial = Me.parryMaterial
			Me.parrySwitch.enabled = True
		Else
			Me.spriteRenderer.sprite = Me.normalSprite
			Me.spriteRenderer.sharedMaterial = Me.normalMaterial
			Me.parrySwitch.enabled = False
		End If
	End Sub

	' Token: 0x060030B4 RID: 12468 RVA: 0x001CA550 File Offset: 0x001C8950
	Private Sub SetNextParry()
		Me.nextSphere.parrySwitch.enabled = True
		Me.parrySwitch.enabled = False
		If Me.lastPlayerController IsNot Nothing Then
			Me.lastPlayerController.stats.OnParry(1F, True)
			Me.lastPlayerController = Nothing
		End If
		Me.spriteRenderer.sprite = Me.normalSprite
		Me.spriteRenderer.sharedMaterial = Me.normalMaterial
		Me.nextSphere.spriteRenderer.sprite = Me.nextSphere.parrySprite
		Me.nextSphere.spriteRenderer.sharedMaterial = Me.parryMaterial
	End Sub

	' Token: 0x060030B5 RID: 12469 RVA: 0x001CA5FC File Offset: 0x001C89FC
	Protected Overrides Sub OnCollisionOther(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionOther(hit, phase)
		If hit.transform AndAlso hit.transform.parent Then
			Me.lastPlayerController = hit.transform.parent.GetComponent(Of AbstractPlayerController)()
		End If
	End Sub

	' Token: 0x04003954 RID: 14676
	<SerializeField()>
	Private nextSphere As TutorialLevelParryNext

	' Token: 0x04003955 RID: 14677
	<SerializeField()>
	Private spriteRenderer As SpriteRenderer

	' Token: 0x04003956 RID: 14678
	<SerializeField()>
	Private normalSprite As Sprite

	' Token: 0x04003957 RID: 14679
	<SerializeField()>
	Private parrySprite As Sprite

	' Token: 0x04003958 RID: 14680
	<SerializeField()>
	Private normalMaterial As Material

	' Token: 0x04003959 RID: 14681
	<SerializeField()>
	Private parryMaterial As Material

	' Token: 0x0400395A RID: 14682
	<SerializeField()>
	Private startAsParry As Boolean

	' Token: 0x0400395B RID: 14683
	<SerializeField()>
	Private parrySwitch As ParrySwitch

	' Token: 0x0400395C RID: 14684
	Private lastPlayerController As AbstractPlayerController
End Class
