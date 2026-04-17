Imports System
Imports UnityEngine

' Token: 0x02000990 RID: 2448
Public Class MapEquipUICardBackSelectIcon
	Inherits AbstractMapCardIcon

	' Token: 0x170004A6 RID: 1190
	' (get) Token: 0x06003946 RID: 14662 RVA: 0x00208512 File Offset: 0x00206912
	' (set) Token: 0x06003947 RID: 14663 RVA: 0x0020851A File Offset: 0x0020691A
	Public Property Index As Integer

	' Token: 0x06003948 RID: 14664 RVA: 0x00208524 File Offset: 0x00206924
	Public Function GetIndexOfNeighbor(direction As Trilean2) As Integer
		Dim mapEquipUICardBackSelectIcon As MapEquipUICardBackSelectIcon = Nothing
		If direction.x < 0 Then
			mapEquipUICardBackSelectIcon = Me.left
		End If
		If direction.x > 0 Then
			mapEquipUICardBackSelectIcon = Me.right
		End If
		If direction.y > 0 Then
			mapEquipUICardBackSelectIcon = Me.up
		End If
		If direction.y < 0 Then
			mapEquipUICardBackSelectIcon = Me.down
		End If
		If mapEquipUICardBackSelectIcon Is Nothing Then
			Return Me.Index
		End If
		Return mapEquipUICardBackSelectIcon.Index
	End Function

	' Token: 0x040040D7 RID: 16599
	<Header("Directions")>
	<SerializeField()>
	Private up As MapEquipUICardBackSelectIcon

	' Token: 0x040040D8 RID: 16600
	<SerializeField()>
	Private down As MapEquipUICardBackSelectIcon

	' Token: 0x040040D9 RID: 16601
	<SerializeField()>
	Private left As MapEquipUICardBackSelectIcon

	' Token: 0x040040DA RID: 16602
	<SerializeField()>
	Private right As MapEquipUICardBackSelectIcon
End Class
