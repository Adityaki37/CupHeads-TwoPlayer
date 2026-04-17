Imports System
Imports UnityEngine

Namespace TMPro
	' Token: 0x02000C7A RID: 3194
	<Serializable()>
	Public Class TMP_Style
		' Token: 0x17000845 RID: 2117
		' (get) Token: 0x06005007 RID: 20487 RVA: 0x0029547B File Offset: 0x0029387B
		' (set) Token: 0x06005008 RID: 20488 RVA: 0x00295483 File Offset: 0x00293883
		Public Property name As String
			Get
				Return Me.m_Name
			End Get
			Set(value As String)
				If value <> Me.m_Name Then
					Me.m_Name = value
				End If
			End Set
		End Property

		' Token: 0x17000846 RID: 2118
		' (get) Token: 0x06005009 RID: 20489 RVA: 0x0029549D File Offset: 0x0029389D
		' (set) Token: 0x0600500A RID: 20490 RVA: 0x002954A5 File Offset: 0x002938A5
		Public Property hashCode As Integer
			Get
				Return Me.m_HashCode
			End Get
			Set(value As Integer)
				If value <> Me.m_HashCode Then
					Me.m_HashCode = value
				End If
			End Set
		End Property

		' Token: 0x17000847 RID: 2119
		' (get) Token: 0x0600500B RID: 20491 RVA: 0x002954BA File Offset: 0x002938BA
		Public ReadOnly Property styleOpeningDefinition As String
			Get
				Return Me.m_OpeningDefinition
			End Get
		End Property

		' Token: 0x17000848 RID: 2120
		' (get) Token: 0x0600500C RID: 20492 RVA: 0x002954C2 File Offset: 0x002938C2
		Public ReadOnly Property styleClosingDefinition As String
			Get
				Return Me.m_ClosingDefinition
			End Get
		End Property

		' Token: 0x17000849 RID: 2121
		' (get) Token: 0x0600500D RID: 20493 RVA: 0x002954CA File Offset: 0x002938CA
		Public ReadOnly Property styleOpeningTagArray As Integer()
			Get
				Return Me.m_OpeningTagArray
			End Get
		End Property

		' Token: 0x1700084A RID: 2122
		' (get) Token: 0x0600500E RID: 20494 RVA: 0x002954D2 File Offset: 0x002938D2
		Public ReadOnly Property styleClosingTagArray As Integer()
			Get
				Return Me.m_ClosingTagArray
			End Get
		End Property

		' Token: 0x0600500F RID: 20495 RVA: 0x002954DC File Offset: 0x002938DC
		Public Sub RefreshStyle()
			Me.m_HashCode = TMP_TextUtilities.GetSimpleHashCode(Me.m_Name)
			Me.m_OpeningTagArray = New Integer(Me.m_OpeningDefinition.Length - 1) {}
			For i As Integer = 0 To Me.m_OpeningDefinition.Length - 1
				Me.m_OpeningTagArray(i) = CInt(Me.m_OpeningDefinition(i))
			Next
			Me.m_ClosingTagArray = New Integer(Me.m_ClosingDefinition.Length - 1) {}
			For j As Integer = 0 To Me.m_ClosingDefinition.Length - 1
				Me.m_ClosingTagArray(j) = CInt(Me.m_ClosingDefinition(j))
			Next
			TMPro_EventManager.ON_TEXT_STYLE_PROPERTY_CHANGED(True)
		End Sub

		' Token: 0x040052B4 RID: 21172
		<SerializeField()>
		Private m_Name As String

		' Token: 0x040052B5 RID: 21173
		<SerializeField()>
		Private m_HashCode As Integer

		' Token: 0x040052B6 RID: 21174
		<SerializeField()>
		Private m_OpeningDefinition As String

		' Token: 0x040052B7 RID: 21175
		<SerializeField()>
		Private m_ClosingDefinition As String

		' Token: 0x040052B8 RID: 21176
		<SerializeField()>
		Private m_OpeningTagArray As Integer()

		' Token: 0x040052B9 RID: 21177
		<SerializeField()>
		Private m_ClosingTagArray As Integer()
	End Class
End Namespace
