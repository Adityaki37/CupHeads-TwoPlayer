Imports System

' Token: 0x02000AC6 RID: 2758
Public Class PlayerDebug
	' Token: 0x06004239 RID: 16953 RVA: 0x0023C2F6 File Offset: 0x0023A6F6
	Public Shared Sub Enable()
		PlayerDebug.Enabled = True
	End Sub

	' Token: 0x0600423A RID: 16954 RVA: 0x0023C2FE File Offset: 0x0023A6FE
	Public Shared Sub Disable()
		PlayerDebug.Enabled = False
	End Sub

	' Token: 0x0600423B RID: 16955 RVA: 0x0023C306 File Offset: 0x0023A706
	Public Shared Sub Toggle()
		PlayerDebug.Enabled = Not PlayerDebug.Enabled
	End Sub

	' Token: 0x040048AF RID: 18607
	Public Shared Enabled As Boolean = True
End Class
