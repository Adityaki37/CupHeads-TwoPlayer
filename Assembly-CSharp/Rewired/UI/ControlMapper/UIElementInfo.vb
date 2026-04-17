Imports System
Imports System.Diagnostics
Imports UnityEngine
Imports UnityEngine.EventSystems
Imports UnityEngine.UI

Namespace Rewired.UI.ControlMapper
	' Token: 0x02000C48 RID: 3144
	<AddComponentMenu("")>
	Public MustInherit Class UIElementInfo
		Inherits MonoBehaviour
		Implements ISelectHandler, IEventSystemHandler

		' Token: 0x140000F8 RID: 248
		' (add) Token: 0x06004D4E RID: 19790 RVA: 0x00265CD0 File Offset: 0x002640D0
		' (remove) Token: 0x06004D4F RID: 19791 RVA: 0x00265D08 File Offset: 0x00264108
		<DebuggerBrowsable(DebuggerBrowsableState.Never)>
		Public Event OnSelectedEvent As Action(Of GameObject)

		' Token: 0x06004D50 RID: 19792 RVA: 0x00265D3E File Offset: 0x0026413E
		Public Sub OnSelect(eventData As BaseEventData) Implements UnityEngine.EventSystems.ISelectHandler.OnSelect
			If Me.OnSelectedEvent IsNot Nothing Then
				Me.OnSelectedEvent(MyBase.gameObject)
			End If
		End Sub

		' Token: 0x0400518B RID: 20875
		Public identifier As String

		' Token: 0x0400518C RID: 20876
		Public intData As Integer

		' Token: 0x0400518D RID: 20877
		Public text As Text
	End Class
End Namespace
