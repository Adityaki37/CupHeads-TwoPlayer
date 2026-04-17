Imports System
Imports UnityEngine

Namespace Rewired.UI.ControlMapper
	' Token: 0x02000C38 RID: 3128
	<AddComponentMenu("")>
	Public Class ThemedElement
		Inherits MonoBehaviour

		' Token: 0x06004CE7 RID: 19687 RVA: 0x00274B78 File Offset: 0x00272F78
		Private Sub Start()
			Me.ApplyTheme()
			AddHandler ControlMapper.OnPlayerChange, AddressOf Me.ApplyTheme
		End Sub

		' Token: 0x06004CE8 RID: 19688 RVA: 0x00274B91 File Offset: 0x00272F91
		Private Sub OnDestroy()
			RemoveHandler ControlMapper.OnPlayerChange, AddressOf Me.ApplyTheme
		End Sub

		' Token: 0x06004CE9 RID: 19689 RVA: 0x00274BA4 File Offset: 0x00272FA4
		Private Sub OnEnable()
			ControlMapper.ApplyTheme(Me._elements)
		End Sub

		' Token: 0x06004CEA RID: 19690 RVA: 0x00274BB1 File Offset: 0x00272FB1
		Private Sub ApplyTheme()
			ControlMapper.ApplyTheme(Me._elements)
		End Sub

		' Token: 0x04005137 RID: 20791
		<SerializeField()>
		Private _elements As ThemedElement.ElementInfo()

		' Token: 0x02000C39 RID: 3129
		<Serializable()>
		Public Class ElementInfo
			' Token: 0x17000795 RID: 1941
			' (get) Token: 0x06004CEC RID: 19692 RVA: 0x00274BC6 File Offset: 0x00272FC6
			Public ReadOnly Property themeClass As String
				Get
					Return Me._themeClass
				End Get
			End Property

			' Token: 0x17000796 RID: 1942
			' (get) Token: 0x06004CED RID: 19693 RVA: 0x00274BCE File Offset: 0x00272FCE
			Public ReadOnly Property component As Component
				Get
					Return Me._component
				End Get
			End Property

			' Token: 0x04005138 RID: 20792
			<SerializeField()>
			Private _themeClass As String

			' Token: 0x04005139 RID: 20793
			<SerializeField()>
			Private _component As Component
		End Class
	End Class
End Namespace
