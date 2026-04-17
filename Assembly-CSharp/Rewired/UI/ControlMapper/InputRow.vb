Imports System
Imports UnityEngine
Imports UnityEngine.UI

Namespace Rewired.UI.ControlMapper
	' Token: 0x02000C33 RID: 3123
	<AddComponentMenu("")>
	Public Class InputRow
		Inherits MonoBehaviour

		' Token: 0x17000765 RID: 1893
		' (get) Token: 0x06004C96 RID: 19606 RVA: 0x00273B72 File Offset: 0x00271F72
		' (set) Token: 0x06004C97 RID: 19607 RVA: 0x00273B7A File Offset: 0x00271F7A
		Public Property buttons As ButtonInfo()

		' Token: 0x06004C98 RID: 19608 RVA: 0x00273B83 File Offset: 0x00271F83
		Public Sub Initialize(rowIndex As Integer, label As String, inputFieldActivatedCallback As Action(Of Integer, ButtonInfo))
			Me.rowIndex = rowIndex
			Me.label.text = label
			Me.inputFieldActivatedCallback = inputFieldActivatedCallback
			Me.buttons = MyBase.transform.GetComponentsInChildren(Of ButtonInfo)(True)
		End Sub

		' Token: 0x06004C99 RID: 19609 RVA: 0x00273BB1 File Offset: 0x00271FB1
		Public Sub OnButtonActivated(buttonInfo As ButtonInfo)
			If Me.inputFieldActivatedCallback Is Nothing Then
				Return
			End If
			Me.inputFieldActivatedCallback(Me.rowIndex, buttonInfo)
		End Sub

		' Token: 0x040050F0 RID: 20720
		Public label As Text

		' Token: 0x040050F2 RID: 20722
		Private rowIndex As Integer

		' Token: 0x040050F3 RID: 20723
		Private inputFieldActivatedCallback As Action(Of Integer, ButtonInfo)
	End Class
End Namespace
