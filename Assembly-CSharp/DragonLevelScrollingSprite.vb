Imports System
Imports UnityEngine

' Token: 0x020005FF RID: 1535
Public Class DragonLevelScrollingSprite
	Inherits ScrollingSprite

	' Token: 0x06001E87 RID: 7815 RVA: 0x00114771 File Offset: 0x00112B71
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.playbackSpeed = 0F
	End Sub

	' Token: 0x06001E88 RID: 7816 RVA: 0x00114784 File Offset: 0x00112B84
	Protected Overrides Sub Update()
		Me.playbackSpeed = Mathf.Lerp(0.1F, 1F, DragonLevel.SPEED)
		MyBase.Update()
	End Sub

	' Token: 0x04002762 RID: 10082
	Private Const MIN_SPEED As Single = 0.1F
End Class
