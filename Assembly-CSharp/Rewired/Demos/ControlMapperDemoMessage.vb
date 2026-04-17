Imports System
Imports System.Collections
Imports Rewired.UI.ControlMapper
Imports UnityEngine
Imports UnityEngine.EventSystems
Imports UnityEngine.UI

Namespace Rewired.Demos
	' Token: 0x02000C05 RID: 3077
	<AddComponentMenu("")>
	Public Class ControlMapperDemoMessage
		Inherits MonoBehaviour

		' Token: 0x0600496A RID: 18794 RVA: 0x00265B64 File Offset: 0x00263F64
		Private Sub Awake()
			If Me.controlMapper IsNot Nothing Then
				AddHandler Me.controlMapper.ScreenClosedEvent, AddressOf Me.OnControlMapperClosed
				AddHandler Me.controlMapper.ScreenOpenedEvent, AddressOf Me.OnControlMapperOpened
			End If
		End Sub

		' Token: 0x0600496B RID: 18795 RVA: 0x00265BB0 File Offset: 0x00263FB0
		Private Sub Start()
			Me.SelectDefault()
		End Sub

		' Token: 0x0600496C RID: 18796 RVA: 0x00265BB8 File Offset: 0x00263FB8
		Private Sub OnControlMapperClosed()
			MyBase.gameObject.SetActive(True)
			MyBase.StartCoroutine(Me.SelectDefaultDeferred())
		End Sub

		' Token: 0x0600496D RID: 18797 RVA: 0x00265BD3 File Offset: 0x00263FD3
		Private Sub OnControlMapperOpened()
			MyBase.gameObject.SetActive(False)
		End Sub

		' Token: 0x0600496E RID: 18798 RVA: 0x00265BE1 File Offset: 0x00263FE1
		Private Sub SelectDefault()
			If EventSystem.current Is Nothing Then
				Return
			End If
			If Me.defaultSelectable IsNot Nothing Then
				EventSystem.current.SetSelectedGameObject(Me.defaultSelectable.gameObject)
			End If
		End Sub

		' Token: 0x0600496F RID: 18799 RVA: 0x00265C1C File Offset: 0x0026401C
		Private Iterator Function SelectDefaultDeferred() As IEnumerator
			Yield Nothing
			Me.SelectDefault()
			Return
		End Function

		' Token: 0x04004F81 RID: 20353
		Public controlMapper As ControlMapper

		' Token: 0x04004F82 RID: 20354
		Public defaultSelectable As Selectable
	End Class
End Namespace
