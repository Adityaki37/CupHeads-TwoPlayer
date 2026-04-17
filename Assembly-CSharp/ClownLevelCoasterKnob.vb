Imports System
Imports UnityEngine

' Token: 0x02000563 RID: 1379
Public Class ClownLevelCoasterKnob
	Inherits ParrySwitch

	' Token: 0x060019F6 RID: 6646 RVA: 0x000ED87E File Offset: 0x000EBC7E
	Public Overrides Sub OnParryPostPause(player As AbstractPlayerController)
		MyBase.OnParryPostPause(player)
		player.stats.ParryOneQuarter()
		Me.sprite.GetComponent(Of SpriteRenderer)().enabled = False
		MyBase.GetComponent(Of Collider2D)().enabled = False
	End Sub

	' Token: 0x04002319 RID: 8985
	<SerializeField()>
	Private sprite As SpriteRenderer
End Class
