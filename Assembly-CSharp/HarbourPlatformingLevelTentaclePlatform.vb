Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020008D4 RID: 2260
Public Class HarbourPlatformingLevelTentaclePlatform
	Inherits LevelPlatform

	' Token: 0x060034E4 RID: 13540 RVA: 0x001EC37D File Offset: 0x001EA77D
	Public Overrides Sub AddChild(player As Transform)
		MyBase.AddChild(player)
		If Not Me.startedSinking Then
			MyBase.StartCoroutine(Me.sink_cr())
		End If
	End Sub

	' Token: 0x060034E5 RID: 13541 RVA: 0x001EC3A0 File Offset: 0x001EA7A0
	Private Iterator Function sink_cr() As IEnumerator
		Me.startedSinking = True
		Dim t As Single = 0F
		Dim start As Vector2 = Me.drag.transform.position
		Dim [end] As Vector2 = New Vector2(Me.drag.transform.position.x, Me.drag.transform.position.y - 500F)
		While t < Me.timeToSink
			Dim val As Single = EaseUtils.Ease(EaseUtils.EaseType.linear, 0F, 1F, t / Me.timeToSink)
			Me.drag.transform.position = Vector2.Lerp(start, [end], val)
			t += CupheadTime.Delta
			Yield Nothing
		End While
		Global.UnityEngine.[Object].Destroy(Me.drag.gameObject)
		Yield Nothing
		Return
	End Function

	' Token: 0x04003D15 RID: 15637
	<SerializeField()>
	Private drag As Transform

	' Token: 0x04003D16 RID: 15638
	Public timeToSink As Single = 5F

	' Token: 0x04003D17 RID: 15639
	Private startedSinking As Boolean
End Class
