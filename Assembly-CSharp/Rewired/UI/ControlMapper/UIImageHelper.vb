Imports System
Imports UnityEngine
Imports UnityEngine.UI

Namespace Rewired.UI.ControlMapper
	' Token: 0x02000C4A RID: 3146
	<AddComponentMenu("")>
	<RequireComponent(GetType(Image))>
	Public Class UIImageHelper
		Inherits MonoBehaviour

		' Token: 0x06004D57 RID: 19799 RVA: 0x00275BFC File Offset: 0x00273FFC
		Public Sub SetEnabledState(newState As Boolean)
			Me.currentState = newState
			Dim state As UIImageHelper.State = If((Not newState), Me.disabledState, Me.enabledState)
			If state Is Nothing Then
				Return
			End If
			Dim component As Image = MyBase.gameObject.GetComponent(Of Image)()
			If component Is Nothing Then
				Global.UnityEngine.Debug.LogError("Image is missing!")
				Return
			End If
			state.[Set](component)
		End Sub

		' Token: 0x06004D58 RID: 19800 RVA: 0x00275C59 File Offset: 0x00274059
		Public Sub SetEnabledStateColor(color As Color)
			Me.enabledState.color = color
		End Sub

		' Token: 0x06004D59 RID: 19801 RVA: 0x00275C67 File Offset: 0x00274067
		Public Sub SetDisabledStateColor(color As Color)
			Me.disabledState.color = color
		End Sub

		' Token: 0x06004D5A RID: 19802 RVA: 0x00275C78 File Offset: 0x00274078
		Public Sub Refresh()
			Dim state As UIImageHelper.State = If((Not Me.currentState), Me.disabledState, Me.enabledState)
			Dim component As Image = MyBase.gameObject.GetComponent(Of Image)()
			If component Is Nothing Then
				Return
			End If
			state.[Set](component)
		End Sub

		' Token: 0x04005191 RID: 20881
		<SerializeField()>
		Private enabledState As UIImageHelper.State

		' Token: 0x04005192 RID: 20882
		<SerializeField()>
		Private disabledState As UIImageHelper.State

		' Token: 0x04005193 RID: 20883
		Private currentState As Boolean

		' Token: 0x02000C4B RID: 3147
		<Serializable()>
		Private Class State
			' Token: 0x06004D5C RID: 19804 RVA: 0x00275CCA File Offset: 0x002740CA
			Public Sub [Set](image As Image)
				If image Is Nothing Then
					Return
				End If
				image.color = Me.color
			End Sub

			' Token: 0x04005194 RID: 20884
			<SerializeField()>
			Public color As Color
		End Class
	End Class
End Namespace
