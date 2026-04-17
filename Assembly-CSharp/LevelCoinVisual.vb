Imports System
Imports UnityEngine

' Token: 0x020004A2 RID: 1186
Public Class LevelCoinVisual
	Inherits AbstractPausableComponent

	' Token: 0x06001353 RID: 4947 RVA: 0x000AAC7F File Offset: 0x000A907F
	Private Sub OnDeathAnimComplete()
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub
End Class
