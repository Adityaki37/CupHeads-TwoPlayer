Imports System
Imports UnityEngine

' Token: 0x02000B2B RID: 2859
<ExecuteInEditMode()>
Public NotInheritable Class SortingLayerExposed
	Inherits MonoBehaviour

	' Token: 0x06004556 RID: 17750 RVA: 0x00247EB7 File Offset: 0x002462B7
	Public Sub OnValidate()
		Me.Apply()
	End Sub

	' Token: 0x06004557 RID: 17751 RVA: 0x00247EBF File Offset: 0x002462BF
	Public Sub OnEnable()
		Me.Apply()
	End Sub

	' Token: 0x06004558 RID: 17752 RVA: 0x00247EC8 File Offset: 0x002462C8
	Private Sub Apply()
		Dim component As MeshRenderer = MyBase.gameObject.GetComponent(Of MeshRenderer)()
		component.sortingLayerName = Me.sortingLayerName
		component.sortingOrder = Me.sortingOrder
	End Sub

	' Token: 0x04004AF1 RID: 19185
	<SerializeField()>
	Private sortingLayerName As String = "Default"

	' Token: 0x04004AF2 RID: 19186
	<SerializeField()>
	Private sortingOrder As Integer
End Class
