Imports System

Namespace UnityEngine.PostProcessing
	' Token: 0x02000BF9 RID: 3065
	Public MustInherit Class PostProcessingComponent(Of T As PostProcessingModel)
		Inherits PostProcessingComponentBase

		' Token: 0x1700068B RID: 1675
		' (get) Token: 0x06004937 RID: 18743 RVA: 0x0025DBC0 File Offset: 0x0025BFC0
		' (set) Token: 0x06004938 RID: 18744 RVA: 0x0025DBC8 File Offset: 0x0025BFC8
		Public Property model As T

		' Token: 0x06004939 RID: 18745 RVA: 0x0025DBD1 File Offset: 0x0025BFD1
		Public Overridable Sub Init(pcontext As PostProcessingContext, pmodel As T)
			Me.context = pcontext
			Me.model = pmodel
		End Sub

		' Token: 0x0600493A RID: 18746 RVA: 0x0025DBE1 File Offset: 0x0025BFE1
		Public Overrides Function GetModel() As PostProcessingModel
			Return Me.model
		End Function
	End Class
End Namespace
