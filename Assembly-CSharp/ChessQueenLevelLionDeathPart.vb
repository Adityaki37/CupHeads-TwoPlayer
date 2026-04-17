Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x0200054C RID: 1356
Public Class ChessQueenLevelLionDeathPart
	Inherits AbstractPausableComponent

	' Token: 0x06001913 RID: 6419 RVA: 0x000E3512 File Offset: 0x000E1912
	Private Sub Start()
		MyBase.animator.Play("Loop", 0, Global.UnityEngine.Random.Range(0F, 1F))
		MyBase.StartCoroutine(Me.grow_cr())
	End Sub

	' Token: 0x06001914 RID: 6420 RVA: 0x000E3544 File Offset: 0x000E1944
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

	' Token: 0x0400222F RID: 8751
	<SerializeField()>
	Private growthSpeed As Single
End Class
