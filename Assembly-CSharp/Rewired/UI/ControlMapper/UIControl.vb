Imports System
Imports UnityEngine
Imports UnityEngine.UI

Namespace Rewired.UI.ControlMapper
	' Token: 0x02000C46 RID: 3142
	<AddComponentMenu("")>
	Public Class UIControl
		Inherits MonoBehaviour

		' Token: 0x170007C1 RID: 1985
		' (get) Token: 0x06004D42 RID: 19778 RVA: 0x00275905 File Offset: 0x00273D05
		Public ReadOnly Property id As Integer
			Get
				Return Me._id
			End Get
		End Property

		' Token: 0x06004D43 RID: 19779 RVA: 0x0027590D File Offset: 0x00273D0D
		Private Sub Awake()
			Me._id = UIControl.GetNextUid()
		End Sub

		' Token: 0x170007C2 RID: 1986
		' (get) Token: 0x06004D44 RID: 19780 RVA: 0x0027591A File Offset: 0x00273D1A
		' (set) Token: 0x06004D45 RID: 19781 RVA: 0x00275922 File Offset: 0x00273D22
		Public Property showTitle As Boolean
			Get
				Return Me._showTitle
			End Get
			Set(value As Boolean)
				If Me.title Is Nothing Then
					Return
				End If
				Me.title.gameObject.SetActive(value)
				Me._showTitle = value
			End Set
		End Property

		' Token: 0x06004D46 RID: 19782 RVA: 0x0027594E File Offset: 0x00273D4E
		Public Overridable Sub SetCancelCallback(cancelCallback As Action)
		End Sub

		' Token: 0x06004D47 RID: 19783 RVA: 0x00275950 File Offset: 0x00273D50
		Private Shared Function GetNextUid() As Integer
			If UIControl._uidCounter = 2147483647 Then
				UIControl._uidCounter = 0
			End If
			Dim uidCounter As Integer = UIControl._uidCounter
			UIControl._uidCounter += 1
			Return uidCounter
		End Function

		' Token: 0x04005185 RID: 20869
		Public title As Text

		' Token: 0x04005186 RID: 20870
		Private _id As Integer

		' Token: 0x04005187 RID: 20871
		Private _showTitle As Boolean

		' Token: 0x04005188 RID: 20872
		Private Shared _uidCounter As Integer
	End Class
End Namespace
