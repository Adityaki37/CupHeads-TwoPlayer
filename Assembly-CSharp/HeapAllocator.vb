Imports System

' Token: 0x02000B28 RID: 2856
Public Module HeapAllocator
	' Token: 0x06004536 RID: 17718 RVA: 0x002479F8 File Offset: 0x00245DF8
	Public Sub Allocate(iterations As Integer)
		Dim array As Object() = New Object(iterations - 1) {}
		For i As Integer = 0 To iterations - 1
			Dim array2 As Object() = New Object(HeapAllocator.AllocationsPerIteration - 1) {}
			For j As Integer = 0 To HeapAllocator.AllocationsPerIteration - 1
				array2(j) = New Byte(HeapAllocator.BytesPerAllocation - 1) {}
			Next
			array(i) = array2
		Next
	End Sub

	' Token: 0x04004AE8 RID: 19176
	Private BytesPerAllocation As Integer = 1024

	' Token: 0x04004AE9 RID: 19177
	Private AllocationsPerIteration As Integer = 1024
End Module
