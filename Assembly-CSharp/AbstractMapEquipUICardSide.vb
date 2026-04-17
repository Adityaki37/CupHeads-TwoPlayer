Imports System
Imports UnityEngine

' Token: 0x02000986 RID: 2438
Public Class AbstractMapEquipUICardSide
	Inherits AbstractMonoBehaviour

	' Token: 0x170004A3 RID: 1187
	' (get) Token: 0x06003900 RID: 14592 RVA: 0x00205EE8 File Offset: 0x002042E8
	' (set) Token: 0x06003901 RID: 14593 RVA: 0x00205EF0 File Offset: 0x002042F0
	Protected Private Property playerID As PlayerId

	' Token: 0x06003902 RID: 14594 RVA: 0x00205EF9 File Offset: 0x002042F9
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.canvasGroup = MyBase.GetComponent(Of CanvasGroup)()
	End Sub

	' Token: 0x06003903 RID: 14595 RVA: 0x00205F0D File Offset: 0x0020430D
	Public Overridable Sub Init(playerID As PlayerId)
		Me.playerID = playerID
	End Sub

	' Token: 0x06003904 RID: 14596 RVA: 0x00205F16 File Offset: 0x00204316
	Public Sub SetActive(active As Boolean)
		Me.canvasGroup.alpha = CSng(If((Not active), 0, 1))
	End Sub

	' Token: 0x04004091 RID: 16529
	Protected canvasGroup As CanvasGroup
End Class
