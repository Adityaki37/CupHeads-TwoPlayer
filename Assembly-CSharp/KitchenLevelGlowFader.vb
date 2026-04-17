Imports System
Imports UnityEngine

' Token: 0x020006D1 RID: 1745
Public Class KitchenLevelGlowFader
	Inherits MonoBehaviour

	' Token: 0x06002528 RID: 9512 RVA: 0x0015C804 File Offset: 0x0015AC04
	Private Sub Update()
		Me.t += CupheadTime.Delta
		Me.rend.color = New Color(1F, 1F, 1F, Mathf.Lerp(Me.alphaMin, 1F, (Mathf.Sin(Me.t * Me.speedModifier) + 1F) / 2F))
	End Sub

	' Token: 0x04002DD0 RID: 11728
	<SerializeField()>
	Private rend As SpriteRenderer

	' Token: 0x04002DD1 RID: 11729
	<SerializeField()>
	<Range(0F, 6.2831855F)>
	Private startOffset As Single

	' Token: 0x04002DD2 RID: 11730
	Private t As Single

	' Token: 0x04002DD3 RID: 11731
	<SerializeField()>
	Private speedModifier As Single = 1F

	' Token: 0x04002DD4 RID: 11732
	<SerializeField()>
	Private alphaMin As Single = 0.8F
End Class
