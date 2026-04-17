Imports System
Imports System.Collections.Generic
Imports System.Linq

Namespace TMPro
	' Token: 0x02000CA0 RID: 3232
	<Serializable()>
	Public Class KerningTable
		' Token: 0x06005180 RID: 20864 RVA: 0x00299691 File Offset: 0x00297A91
		Public Sub New()
			Me.kerningPairs = New List(Of KerningPair)()
		End Sub

		' Token: 0x06005181 RID: 20865 RVA: 0x002996A4 File Offset: 0x00297AA4
		Public Sub AddKerningPair()
			If Me.kerningPairs.Count = 0 Then
				Me.kerningPairs.Add(New KerningPair(0, 0, 0F))
			Else
				Dim ascII_Left As Integer = Me.kerningPairs.Last().AscII_Left
				Dim ascII_Right As Integer = Me.kerningPairs.Last().AscII_Right
				Dim xadvanceOffset As Single = Me.kerningPairs.Last().XadvanceOffset
				Me.kerningPairs.Add(New KerningPair(ascII_Left, ascII_Right, xadvanceOffset))
			End If
		End Sub

		' Token: 0x06005182 RID: 20866 RVA: 0x00299724 File Offset: 0x00297B24
		Public Function AddKerningPair(left As Integer, right As Integer, offset As Single) As Integer
			Dim num As Integer = Me.kerningPairs.FindIndex(Function(item As KerningPair) item.AscII_Left = left AndAlso item.AscII_Right = right)
			If num = -1 Then
				Me.kerningPairs.Add(New KerningPair(left, right, offset))
				Return 0
			End If
			Return -1
		End Function

		' Token: 0x06005183 RID: 20867 RVA: 0x00299784 File Offset: 0x00297B84
		Public Sub RemoveKerningPair(left As Integer, right As Integer)
			Dim num As Integer = Me.kerningPairs.FindIndex(Function(item As KerningPair) item.AscII_Left = left AndAlso item.AscII_Right = right)
			If num <> -1 Then
				Me.kerningPairs.RemoveAt(num)
			End If
		End Sub

		' Token: 0x06005184 RID: 20868 RVA: 0x002997D0 File Offset: 0x00297BD0
		Public Sub RemoveKerningPair(index As Integer)
			Me.kerningPairs.RemoveAt(index)
		End Sub

		' Token: 0x06005185 RID: 20869 RVA: 0x002997E0 File Offset: 0x00297BE0
		Public Sub SortKerningPairs()
			If Me.kerningPairs.Count > 0 Then
				Me.kerningPairs = Me.kerningPairs.OrderBy(Function(s As KerningPair) s.AscII_Left).ThenBy(Function(s As KerningPair) s.AscII_Right).ToList()
			End If
		End Sub

		' Token: 0x04005439 RID: 21561
		Public kerningPairs As List(Of KerningPair)
	End Class
End Namespace
