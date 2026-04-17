Imports System
Imports System.Collections.Generic
Imports UnityEngine

' Token: 0x02000B04 RID: 2820
Public Class ShopSceneBuyAnimation
	Inherits MonoBehaviour

	' Token: 0x06004469 RID: 17513 RVA: 0x00243364 File Offset: 0x00241764
	Private Sub Start()
		Me.rightIndex = New List(Of Integer)()
		Me.indexes = New List(Of Integer)() From { 0, 1, 2, 3, 4, 5 }
		Dim num As Integer = Global.UnityEngine.Random.Range(0, 6)
		Me.rightIndex.Add(Me.indexes(num))
		Me.indexes.RemoveAt(num)
		Dim num2 As Integer = Global.UnityEngine.Random.Range(0, 5)
		Me.rightIndex.Add(Me.indexes(num2))
		Me.indexes.RemoveAt(num2)
		Dim num3 As Integer = Global.UnityEngine.Random.Range(0, 4)
		Me.rightIndex.Add(Me.indexes(num3))
		Me.indexes.RemoveAt(num3)
		For i As Integer = 0 To Me.rightIndex.Count - 1
			Me.coins(Me.rightIndex(i)).gameObject.SetActive(True)
		Next
	End Sub

	' Token: 0x0600446A RID: 17514 RVA: 0x00243474 File Offset: 0x00241874
	Private Sub OnDestroy()
		For i As Integer = 0 To Me.coins.Length - 1
			Me.coins(i) = Nothing
		Next
	End Sub

	' Token: 0x04004A03 RID: 18947
	Public coins As GameObject()

	' Token: 0x04004A04 RID: 18948
	Private indexes As List(Of Integer)

	' Token: 0x04004A05 RID: 18949
	Private rightIndex As List(Of Integer)
End Class
