Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020008E6 RID: 2278
Public Class MountainPlatformingLevelMinerRope
	Inherits AbstractPausableComponent

	' Token: 0x06003563 RID: 13667 RVA: 0x001F1F3C File Offset: 0x001F033C
	Public Sub PullRope(ascendTime As Single, startPos As Vector2)
		MyBase.StartCoroutine(Me.pull_up_rope_cr(ascendTime, startPos))
	End Sub

	' Token: 0x06003564 RID: 13668 RVA: 0x001F1F50 File Offset: 0x001F0350
	Public Iterator Function pull_up_rope_cr(ascendTime As Single, startPos As Vector2) As IEnumerator
		Dim t As Single = 0F
		Dim [end] As Vector3 = New Vector3(MyBase.transform.position.x, startPos.y + 400F)
		Dim start As Vector3 = MyBase.transform.position
		While t < ascendTime
			t += CupheadTime.Delta
			Dim val As Single = EaseUtils.Ease(EaseUtils.EaseType.linear, 0F, 1F, t / ascendTime)
			MyBase.transform.position = Vector2.Lerp(start, [end], val)
			Yield Nothing
		End While
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
		Yield Nothing
		Return
	End Function
End Class
