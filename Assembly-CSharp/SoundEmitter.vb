Imports System

' Token: 0x020009C9 RID: 2505
Public Class SoundEmitter
	' Token: 0x06003AE4 RID: 15076 RVA: 0x0021256D File Offset: 0x0021096D
	Public Sub New(parent As AbstractPausableComponent)
		Me.parent = parent
	End Sub

	' Token: 0x06003AE5 RID: 15077 RVA: 0x0021257C File Offset: 0x0021097C
	Public Sub Add(key As String)
		Me.parent.EmitSound(key)
	End Sub

	' Token: 0x040042A3 RID: 17059
	Private parent As AbstractPausableComponent
End Class
