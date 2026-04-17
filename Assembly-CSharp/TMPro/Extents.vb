Imports System
Imports UnityEngine

Namespace TMPro
	' Token: 0x02000CAC RID: 3244
	Public Structure Extents
		' Token: 0x06005197 RID: 20887 RVA: 0x0029A01F File Offset: 0x0029841F
		Public Sub New(min As Vector2, max As Vector2)
			Me.min = min
			Me.max = max
		End Sub

		' Token: 0x06005198 RID: 20888 RVA: 0x0029A030 File Offset: 0x00298430
		Public Overrides Function ToString() As String
			Return String.Concat(New String() { "Min (", Me.min.x.ToString("f2"), ", ", Me.min.y.ToString("f2"), ")   Max (", Me.max.x.ToString("f2"), ", ", Me.max.y.ToString("f2"), ")" })
		End Function

		' Token: 0x040054A0 RID: 21664
		Public min As Vector2

		' Token: 0x040054A1 RID: 21665
		Public max As Vector2
	End Structure
End Namespace
