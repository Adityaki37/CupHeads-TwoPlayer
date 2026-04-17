Imports System
Imports System.Collections.Generic
Imports UnityEngine

' Token: 0x02000515 RID: 1301
Public Class BeeLevelPlatforms
	Inherits AbstractMonoBehaviour

	' Token: 0x06001730 RID: 5936 RVA: 0x000D0230 File Offset: 0x000CE630
	Public Sub Init()
		Dim list As List(Of Transform) = New List(Of Transform)(Me.rows)
		For i As Integer = 0 To Me.rows.Length - 1
			Dim transform As Transform = Global.UnityEngine.[Object].Instantiate(Of Transform)(Me.rows(i))
			list.Add(transform)
			transform.transform.SetParent(MyBase.transform)
			transform.transform.AddLocalPosition(0F, -230F, 0F)
		Next
		Me.rows = list.ToArray()
	End Sub

	' Token: 0x06001731 RID: 5937 RVA: 0x000D02B0 File Offset: 0x000CE6B0
	Public Sub Randomize(missingCount As Integer)
		For Each transform As Transform In Me.rows
			Dim list As List(Of Transform) = New List(Of Transform)(transform.GetChildTransforms())
			For Each transform2 As Transform In list
				transform2.gameObject.SetActive(True)
			Next
			For j As Integer = 0 To missingCount - 1
				If list.Count <= 1 Then
					Exit For
				End If
				Dim num As Integer = Global.UnityEngine.Random.Range(0, list.Count)
				If num = 0 AndAlso BeeLevelPlatforms.lastPlatform = 0 Then
					Exit For
				End If
				If num = 3 AndAlso BeeLevelPlatforms.lastPlatform = 2 Then
					Exit For
				End If
				list(num).gameObject.SetActive(False)
				BeeLevelPlatforms.lastPlatform = num
				list.RemoveAt(num)
			Next
		Next
	End Sub

	' Token: 0x0400206D RID: 8301
	Private Const OFFSET As Single = -230F

	' Token: 0x0400206E RID: 8302
	<SerializeField()>
	Private rows As Transform()

	' Token: 0x0400206F RID: 8303
	Private Shared lastPlatform As Integer
End Class
