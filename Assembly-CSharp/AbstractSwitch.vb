Imports System
Imports System.Diagnostics

' Token: 0x02000B22 RID: 2850
Public MustInherit Class AbstractSwitch
	Inherits AbstractCollidableObject

	' Token: 0x140000C8 RID: 200
	' (add) Token: 0x060044F6 RID: 17654 RVA: 0x000B2B84 File Offset: 0x000B0F84
	' (remove) Token: 0x060044F7 RID: 17655 RVA: 0x000B2BBC File Offset: 0x000B0FBC
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnActivate As Action

	' Token: 0x060044F8 RID: 17656 RVA: 0x000B2BF2 File Offset: 0x000B0FF2
	Protected Sub DispatchEvent()
		If Me.OnActivate IsNot Nothing Then
			Me.OnActivate()
		End If
	End Sub
End Class
