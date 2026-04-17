Imports System
Imports UnityEngine

' Token: 0x020007A9 RID: 1961
Public Class SallyStagePlayLevelBowingAnimation
	Inherits AbstractPausableComponent

	' Token: 0x06002C12 RID: 11282 RVA: 0x0019EAE2 File Offset: 0x0019CEE2
	Private Sub Start()
		AddHandler Level.Current.OnWinEvent, AddressOf Me.OnDeath
	End Sub

	' Token: 0x06002C13 RID: 11283 RVA: 0x0019EAFA File Offset: 0x0019CEFA
	Private Sub PickNumber()
		Me.maxCounter = Global.UnityEngine.Random.Range(12, 21)
	End Sub

	' Token: 0x06002C14 RID: 11284 RVA: 0x0019EB0C File Offset: 0x0019CF0C
	Private Sub Counter()
		If Me.counter < Me.maxCounter Then
			Me.counter += 1
		Else
			For Each animator As Animator In Me.animators
				animator.SetTrigger("OnBow")
				Me.counter = 0
			Next
		End If
	End Sub

	' Token: 0x06002C15 RID: 11285 RVA: 0x0019EB70 File Offset: 0x0019CF70
	Private Sub OnDeath()
		For Each animator As Animator In Me.animators
			animator.SetTrigger("OnDeath")
		Next
	End Sub

	' Token: 0x06002C16 RID: 11286 RVA: 0x0019EBA7 File Offset: 0x0019CFA7
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		RemoveHandler Level.Current.OnWinEvent, AddressOf Me.OnDeath
	End Sub

	' Token: 0x040034CA RID: 13514
	<SerializeField()>
	Private animators As Animator()

	' Token: 0x040034CB RID: 13515
	Private counter As Integer

	' Token: 0x040034CC RID: 13516
	Private maxCounter As Integer
End Class
