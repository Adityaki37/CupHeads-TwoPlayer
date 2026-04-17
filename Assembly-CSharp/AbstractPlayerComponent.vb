Imports System

' Token: 0x020009D1 RID: 2513
Public Class AbstractPlayerComponent
	Inherits AbstractCollidableObject

	' Token: 0x170004D4 RID: 1236
	' (get) Token: 0x06003B0A RID: 15114 RVA: 0x00213827 File Offset: 0x00211C27
	Public ReadOnly Property basePlayer As AbstractPlayerController
		Get
			If Me._basePlayer Is Nothing Then
				Me._basePlayer = MyBase.GetComponent(Of AbstractPlayerController)()
			End If
			Return Me._basePlayer
		End Get
	End Property

	' Token: 0x06003B0B RID: 15115 RVA: 0x0021384C File Offset: 0x00211C4C
	Protected NotInheritable Overrides Sub Awake()
		MyBase.Awake()
		Me.OnAwake()
	End Sub

	' Token: 0x06003B0C RID: 15116 RVA: 0x0021385A File Offset: 0x00211C5A
	Protected Overridable Sub OnAwake()
	End Sub

	' Token: 0x06003B0D RID: 15117 RVA: 0x0021385C File Offset: 0x00211C5C
	Public Overridable Sub OnLevelStart()
	End Sub

	' Token: 0x040042C6 RID: 17094
	Private _basePlayer As AbstractPlayerController
End Class
