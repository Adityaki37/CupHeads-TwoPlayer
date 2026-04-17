Imports System
Imports UnityEngine

Namespace Rewired.UI.ControlMapper
	' Token: 0x02000C32 RID: 3122
	<AddComponentMenu("")>
	Public Class InputFieldInfo
		Inherits UIElementInfo

		' Token: 0x17000760 RID: 1888
		' (get) Token: 0x06004C8B RID: 19595 RVA: 0x00273B15 File Offset: 0x00271F15
		' (set) Token: 0x06004C8C RID: 19596 RVA: 0x00273B1D File Offset: 0x00271F1D
		Public Property actionId As Integer

		' Token: 0x17000761 RID: 1889
		' (get) Token: 0x06004C8D RID: 19597 RVA: 0x00273B26 File Offset: 0x00271F26
		' (set) Token: 0x06004C8E RID: 19598 RVA: 0x00273B2E File Offset: 0x00271F2E
		Public Property axisRange As AxisRange

		' Token: 0x17000762 RID: 1890
		' (get) Token: 0x06004C8F RID: 19599 RVA: 0x00273B37 File Offset: 0x00271F37
		' (set) Token: 0x06004C90 RID: 19600 RVA: 0x00273B3F File Offset: 0x00271F3F
		Public Property actionElementMapId As Integer

		' Token: 0x17000763 RID: 1891
		' (get) Token: 0x06004C91 RID: 19601 RVA: 0x00273B48 File Offset: 0x00271F48
		' (set) Token: 0x06004C92 RID: 19602 RVA: 0x00273B50 File Offset: 0x00271F50
		Public Property controllerType As ControllerType

		' Token: 0x17000764 RID: 1892
		' (get) Token: 0x06004C93 RID: 19603 RVA: 0x00273B59 File Offset: 0x00271F59
		' (set) Token: 0x06004C94 RID: 19604 RVA: 0x00273B61 File Offset: 0x00271F61
		Public Property controllerId As Integer
	End Class
End Namespace
