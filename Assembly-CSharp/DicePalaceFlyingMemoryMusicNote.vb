Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020005CE RID: 1486
Public Class DicePalaceFlyingMemoryMusicNote
	Inherits BasicProjectile

	' Token: 0x06001D2F RID: 7471 RVA: 0x0010C2B4 File Offset: 0x0010A6B4
	Public Function Create(pos As Vector3, rotation As Single, speed As Single, deathTimer As Single) As DicePalaceFlyingMemoryMusicNote
		Dim dicePalaceFlyingMemoryMusicNote As DicePalaceFlyingMemoryMusicNote = TryCast(MyBase.Create(pos, rotation, speed), DicePalaceFlyingMemoryMusicNote)
		dicePalaceFlyingMemoryMusicNote.deathTimer = deathTimer
		Return dicePalaceFlyingMemoryMusicNote
	End Function

	' Token: 0x06001D30 RID: 7472 RVA: 0x0010C2DE File Offset: 0x0010A6DE
	Protected Overrides Sub Start()
		MyBase.Start()
		MyBase.StartCoroutine(Me.death_timer_cr())
	End Sub

	' Token: 0x06001D31 RID: 7473 RVA: 0x0010C2F4 File Offset: 0x0010A6F4
	Protected Overrides Sub FixedUpdate()
		MyBase.FixedUpdate()
		Me.sprite.transform.SetEulerAngles(Nothing, Nothing, New Single?(0F))
	End Sub

	' Token: 0x06001D32 RID: 7474 RVA: 0x0010C334 File Offset: 0x0010A734
	Private Iterator Function death_timer_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, Me.deathTimer)
		MyBase.animator.SetTrigger("OnDeath")
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Note_Death", False, True)
		Me.move = False
		Yield Nothing
		Return
	End Function

	' Token: 0x06001D33 RID: 7475 RVA: 0x0010C34F File Offset: 0x0010A74F
	Private Sub Kill()
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub

	' Token: 0x0400261D RID: 9757
	<SerializeField()>
	Private sprite As Transform

	' Token: 0x0400261E RID: 9758
	Private deathTimer As Single
End Class
