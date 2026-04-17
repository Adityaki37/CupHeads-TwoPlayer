Imports System
Imports UnityEngine

' Token: 0x020006F3 RID: 1779
Public Class MouseLevelRomanCandleProjectile
	Inherits HomingProjectile

	' Token: 0x0600261F RID: 9759 RVA: 0x0016456D File Offset: 0x0016296D
	Protected Overrides Sub Die()
		MyBase.Die()
		Me.StopAllCoroutines()
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub
End Class
