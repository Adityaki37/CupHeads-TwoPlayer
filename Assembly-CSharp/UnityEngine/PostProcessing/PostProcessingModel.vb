Imports System

Namespace UnityEngine.PostProcessing
	' Token: 0x02000BFD RID: 3069
	<Serializable()>
	Public MustInherit Class PostProcessingModel
		' Token: 0x17000692 RID: 1682
		' (get) Token: 0x0600494C RID: 18764 RVA: 0x00263408 File Offset: 0x00261808
		' (set) Token: 0x0600494D RID: 18765 RVA: 0x00263410 File Offset: 0x00261810
		Public Property enabled As Boolean
			Get
				Return Me.m_Enabled
			End Get
			Set(value As Boolean)
				Me.m_Enabled = value
				If value Then
					Me.OnValidate()
				End If
			End Set
		End Property

		' Token: 0x0600494E RID: 18766
		Public MustOverride Sub Reset()

		' Token: 0x0600494F RID: 18767 RVA: 0x00263425 File Offset: 0x00261825
		Public Overridable Sub OnValidate()
		End Sub

		' Token: 0x04004F64 RID: 20324
		<SerializeField()>
		<GetSet("enabled")>
		Private m_Enabled As Boolean
	End Class
End Namespace
