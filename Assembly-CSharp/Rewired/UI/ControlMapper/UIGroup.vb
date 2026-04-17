Imports System
Imports UnityEngine
Imports UnityEngine.UI

Namespace Rewired.UI.ControlMapper
	' Token: 0x02000C49 RID: 3145
	<AddComponentMenu("")>
	Public Class UIGroup
		Inherits MonoBehaviour

		' Token: 0x170007C4 RID: 1988
		' (get) Token: 0x06004D52 RID: 19794 RVA: 0x00275B7C File Offset: 0x00273F7C
		' (set) Token: 0x06004D53 RID: 19795 RVA: 0x00275BA4 File Offset: 0x00273FA4
		Public Property labelText As String
			Get
				Return If((Not(Me._label IsNot Nothing)), String.Empty, Me._label.text)
			End Get
			Set(value As String)
				If Me._label Is Nothing Then
					Return
				End If
				Me._label.text = value
			End Set
		End Property

		' Token: 0x170007C5 RID: 1989
		' (get) Token: 0x06004D54 RID: 19796 RVA: 0x00275BC4 File Offset: 0x00273FC4
		Public ReadOnly Property content As Transform
			Get
				Return Me._content
			End Get
		End Property

		' Token: 0x06004D55 RID: 19797 RVA: 0x00275BCC File Offset: 0x00273FCC
		Public Sub SetLabelActive(state As Boolean)
			If Me._label Is Nothing Then
				Return
			End If
			Me._label.gameObject.SetActive(state)
		End Sub

		' Token: 0x0400518F RID: 20879
		<SerializeField()>
		Private _label As Text

		' Token: 0x04005190 RID: 20880
		<SerializeField()>
		Private _content As Transform
	End Class
End Namespace
