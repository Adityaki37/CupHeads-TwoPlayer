Imports System

' Token: 0x02000708 RID: 1800
Public Class OldManLevelParryThermometer
	Inherits ParrySwitch

	' Token: 0x170003CD RID: 973
	' (get) Token: 0x060026D0 RID: 9936 RVA: 0x0016B519 File Offset: 0x00169919
	' (set) Token: 0x060026D1 RID: 9937 RVA: 0x0016B521 File Offset: 0x00169921
	Public Property isActivated As Boolean

	' Token: 0x060026D2 RID: 9938 RVA: 0x0016B52A File Offset: 0x0016992A
	Public Overrides Sub OnParryPrePause(player As AbstractPlayerController)
		Me.isActivated = True
		MyBase.OnParryPrePause(player)
	End Sub

	' Token: 0x060026D3 RID: 9939 RVA: 0x0016B53A File Offset: 0x0016993A
	Public Overrides Sub OnParryPostPause(player As AbstractPlayerController)
		MyBase.OnParryPostPause(player)
		Me.isActivated = False
		MyBase.gameObject.SetActive(False)
	End Sub
End Class
