Imports System

' Token: 0x020009C7 RID: 2503
Public Class OnlineManager
	' Token: 0x170004D0 RID: 1232
	' (get) Token: 0x06003AD6 RID: 15062 RVA: 0x00212527 File Offset: 0x00210927
	Public Shared ReadOnly Property Instance As OnlineManager
		Get
			If OnlineManager.instance Is Nothing Then
				OnlineManager.instance = New OnlineManager()
			End If
			Return OnlineManager.instance
		End Get
	End Property

	' Token: 0x170004D1 RID: 1233
	' (get) Token: 0x06003AD7 RID: 15063 RVA: 0x00212542 File Offset: 0x00210942
	' (set) Token: 0x06003AD8 RID: 15064 RVA: 0x0021254A File Offset: 0x0021094A
	Public Property [Interface] As OnlineInterface

	' Token: 0x06003AD9 RID: 15065 RVA: 0x00212553 File Offset: 0x00210953
	Public Sub Init()
		Me.[Interface] = New OnlineInterfaceSteam()
		Me.[Interface].Init()
	End Sub

	' Token: 0x0400429F RID: 17055
	Private Shared instance As OnlineManager
End Class
