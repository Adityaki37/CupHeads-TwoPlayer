Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020008A3 RID: 2211
Public Class CircusPlatformingLevelDunkPlatform
	Inherits AbstractCollidableObject

	' Token: 0x0600336D RID: 13165 RVA: 0x001DEC98 File Offset: 0x001DD098
	Private Sub Start()
		Me.collider2d = MyBase.GetComponent(Of Collider2D)()
	End Sub

	' Token: 0x0600336E RID: 13166 RVA: 0x001DECA8 File Offset: 0x001DD0A8
	Protected Overrides Sub OnCollision(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollision(hit, phase)
		If hit.GetComponent(Of CircusPlatformingLevelCannonProjectile)() Then
			Me.collider2d.enabled = False
			MyBase.animator.SetTrigger("Hit")
			MyBase.StartCoroutine(Me.waitSpin_cr())
		End If
	End Sub

	' Token: 0x0600336F RID: 13167 RVA: 0x001DECF6 File Offset: 0x001DD0F6
	Public Sub Drop()
		MyBase.StartCoroutine(Me.deactivate_cr())
	End Sub

	' Token: 0x06003370 RID: 13168 RVA: 0x001DED08 File Offset: 0x001DD108
	Private Iterator Function deactivate_cr() As IEnumerator
		MyBase.animator.SetTrigger("Drop")
		Me.platform.enabled = False
		Yield CupheadTime.WaitForSeconds(Me, Me.platformDown)
		MyBase.animator.SetTrigger("Raise")
		Return
	End Function

	' Token: 0x06003371 RID: 13169 RVA: 0x001DED23 File Offset: 0x001DD123
	Public Sub ActivatePlatform()
		Me.collider2d.enabled = True
		Me.platform.enabled = True
	End Sub

	' Token: 0x06003372 RID: 13170 RVA: 0x001DED40 File Offset: 0x001DD140
	Private Iterator Function waitSpin_cr() As IEnumerator
		AudioManager.Play("circus_platform_plank_target")
		Me.emitAudioFromObject.Add("circus_platform_plank_target")
		Yield CupheadTime.WaitForSeconds(Me, Me.targetSpin)
		MyBase.animator.SetTrigger("SpinStop")
		Return
	End Function

	' Token: 0x06003373 RID: 13171 RVA: 0x001DED5B File Offset: 0x001DD15B
	Private Sub DropSFX()
		AudioManager.Play("circus_platform_plank_drop")
		Me.emitAudioFromObject.Add("circus_platform_plank_drop")
	End Sub

	' Token: 0x06003374 RID: 13172 RVA: 0x001DED77 File Offset: 0x001DD177
	Private Sub RaiseSFX()
		AudioManager.Play("circus_platform_plank_raise")
		Me.emitAudioFromObject.Add("circus_platform_plank_raise")
	End Sub

	' Token: 0x06003375 RID: 13173 RVA: 0x001DED93 File Offset: 0x001DD193
	Private Sub PlankSFX()
		AudioManager.Play("circus_platform_plank_target")
		Me.emitAudioFromObject.Add("circus_platform_plank_target")
	End Sub

	' Token: 0x04003BBC RID: 15292
	Private Const HitParameterName As String = "Hit"

	' Token: 0x04003BBD RID: 15293
	Private Const DropParameterName As String = "Drop"

	' Token: 0x04003BBE RID: 15294
	Private Const RaiseParameterName As String = "Raise"

	' Token: 0x04003BBF RID: 15295
	Private Const StopSpinParameterName As String = "SpinStop"

	' Token: 0x04003BC0 RID: 15296
	<SerializeField()>
	Private platform As Collider2D

	' Token: 0x04003BC1 RID: 15297
	<SerializeField()>
	Private platformDown As Single

	' Token: 0x04003BC2 RID: 15298
	<SerializeField()>
	Private targetSpin As Single

	' Token: 0x04003BC3 RID: 15299
	Private collider2d As Collider2D
End Class
