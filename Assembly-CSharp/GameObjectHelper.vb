Imports System
Imports UnityEngine

' Token: 0x02000372 RID: 882
Public Class GameObjectHelper
	' Token: 0x06000A29 RID: 2601 RVA: 0x0007E3D4 File Offset: 0x0007C7D4
	Public Sub New(name As String)
		Me._gameObject = New GameObject("[Helper] " + name)
		Me.events = Me._gameObject.AddComponent(Of GameObjectHelperGO)()
	End Sub

	' Token: 0x170001F3 RID: 499
	' (get) Token: 0x06000A2A RID: 2602 RVA: 0x0007E403 File Offset: 0x0007C803
	' (set) Token: 0x06000A2B RID: 2603 RVA: 0x0007E40B File Offset: 0x0007C80B
	Public Property events As GameObjectHelperGO

	' Token: 0x06000A2C RID: 2604 RVA: 0x0007E414 File Offset: 0x0007C814
	Public Sub Destroy()
		Global.UnityEngine.[Object].Destroy(Me._gameObject)
	End Sub

	' Token: 0x06000A2D RID: 2605 RVA: 0x0007E421 File Offset: 0x0007C821
	Public Sub DontDestroyOnLoad()
		Global.UnityEngine.[Object].DontDestroyOnLoad(Me._gameObject)
	End Sub

	' Token: 0x0400145E RID: 5214
	Private _gameObject As GameObject
End Class
