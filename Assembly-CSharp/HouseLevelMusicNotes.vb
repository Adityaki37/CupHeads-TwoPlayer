Imports System
Imports UnityEngine

' Token: 0x020006CE RID: 1742
Public Class HouseLevelMusicNotes
	Inherits AbstractPausableComponent

	' Token: 0x0600251C RID: 9500 RVA: 0x0015C491 File Offset: 0x0015A891
	Private Sub ChangeAnimation()
		MyBase.animator.SetInteger("Type", Global.UnityEngine.Random.Range(0, 4))
	End Sub
End Class
