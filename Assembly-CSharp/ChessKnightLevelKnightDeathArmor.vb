Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000541 RID: 1345
Public Class ChessKnightLevelKnightDeathArmor
	Inherits AbstractPausableComponent

	' Token: 0x060018BA RID: 6330 RVA: 0x000E036A File Offset: 0x000DE76A
	Private Sub Start()
		MyBase.animator.Play(Me.type.ToString())
		MyBase.StartCoroutine(Me.grow_cr())
	End Sub

	' Token: 0x060018BB RID: 6331 RVA: 0x000E0398 File Offset: 0x000DE798
	Private Iterator Function grow_cr() As IEnumerator
		Dim elapsed As Single = 0F
		Dim wait As WaitForFrameTimePersistent = New WaitForFrameTimePersistent(0.041666668F, False)
		While True
			Yield wait
			elapsed += wait.frameTime + wait.accumulator
			Dim scale As Vector3 = MyBase.transform.localScale
			scale.x = (1F + elapsed * Me.growthSpeed) * Mathf.Sign(scale.x)
			scale.y = 1F + elapsed * Me.growthSpeed
			MyBase.transform.localScale = scale
		End While
		Return
	End Function

	' Token: 0x040021C5 RID: 8645
	<SerializeField()>
	Private type As ChessKnightLevelKnightDeathArmor.Type

	' Token: 0x040021C6 RID: 8646
	<SerializeField()>
	Private growthSpeed As Single

	' Token: 0x02000542 RID: 1346
	Public Enum Type
		' Token: 0x040021C8 RID: 8648
		Helmet
		' Token: 0x040021C9 RID: 8649
		Shield
		' Token: 0x040021CA RID: 8650
		Sword
	End Enum
End Class
