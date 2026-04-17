Imports System
Imports UnityEngine

' Token: 0x020008AA RID: 2218
Public Class CircusPlatformingLevelPretzel
	Inherits AbstractPlatformingLevelEnemy

	' Token: 0x060033B3 RID: 13235 RVA: 0x001E07AA File Offset: 0x001DEBAA
	Protected Overrides Sub OnStart()
	End Sub

	' Token: 0x060033B4 RID: 13236 RVA: 0x001E07AC File Offset: 0x001DEBAC
	Public Sub SetPath(path As Transform())
		Me.path = path
	End Sub

	' Token: 0x060033B5 RID: 13237 RVA: 0x001E07B5 File Offset: 0x001DEBB5
	Public Sub SetStartPosition(index As Integer)
		Me.nextPoint = index
		MyBase.transform.position = Me.path(Me.nextPoint).position
	End Sub

	' Token: 0x060033B6 RID: 13238 RVA: 0x001E07DC File Offset: 0x001DEBDC
	Public Sub Jump()
		If Me.goingLeft Then
			Me.nextPoint -= 1
		Else
			Me.nextPoint += 1
		End If
		If Me.nextPoint < 0 OrElse (Me.path IsNot Nothing AndAlso Me.nextPoint >= Me.path.Length) Then
			Me.Die()
		End If
	End Sub

	' Token: 0x060033B7 RID: 13239 RVA: 0x001E0848 File Offset: 0x001DEC48
	Public Sub Land()
		MyBase.animator.SetTrigger("Salt")
		If Me.nextPoint < Me.path.Length Then
			MyBase.transform.position = Me.path(Me.nextPoint).position
		End If
	End Sub

	' Token: 0x060033B8 RID: 13240 RVA: 0x001E0895 File Offset: 0x001DEC95
	Public Sub JumpSFX()
		AudioManager.Play("circus_pretzel_jump")
		Me.emitAudioFromObject.Add("circus_pretzel_jump")
	End Sub

	' Token: 0x060033B9 RID: 13241 RVA: 0x001E08B4 File Offset: 0x001DECB4
	Protected Overrides Sub Die()
		AudioManager.[Stop]("circus_pretzel_jump")
		AudioManager.Play("circus_generic_death_big")
		Me.emitAudioFromObject.Add("circus_generic_death_big")
		MyBase.Die()
		Global.UnityEngine.[Object].Destroy(MyBase.transform.parent.gameObject)
	End Sub

	' Token: 0x04003C00 RID: 15360
	Private Const SaltParameterName As String = "Salt"

	' Token: 0x04003C01 RID: 15361
	Public goingLeft As Boolean

	' Token: 0x04003C02 RID: 15362
	<SerializeField()>
	Private jumpMultiplierX As Single

	' Token: 0x04003C03 RID: 15363
	<SerializeField()>
	Private jumpMultiplierY As Single

	' Token: 0x04003C04 RID: 15364
	<SerializeField()>
	Private inverseJumpMultiplierY As Single

	' Token: 0x04003C05 RID: 15365
	<SerializeField()>
	Private transformDustA As Transform

	' Token: 0x04003C06 RID: 15366
	<SerializeField()>
	Private transformDustB As Transform

	' Token: 0x04003C07 RID: 15367
	Private path As Transform()

	' Token: 0x04003C08 RID: 15368
	Private nextPoint As Integer
End Class
