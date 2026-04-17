Imports System
Imports UnityEngine

Namespace TMPro
	' Token: 0x02000CAD RID: 3245
	<Serializable()>
	Public Structure Mesh_Extents
		' Token: 0x06005199 RID: 20889 RVA: 0x0029A0D3 File Offset: 0x002984D3
		Public Sub New(min As Vector2, max As Vector2)
			Me.min = min
			Me.max = max
		End Sub

		' Token: 0x0600519A RID: 20890 RVA: 0x0029A0E4 File Offset: 0x002984E4
		Public Overrides Function ToString() As String
			Return String.Concat(New String() { "Min (", Me.min.x.ToString("f2"), ", ", Me.min.y.ToString("f2"), ")   Max (", Me.max.x.ToString("f2"), ", ", Me.max.y.ToString("f2"), ")" })
		End Function

		' Token: 0x040054A2 RID: 21666
		Public min As Vector2

		' Token: 0x040054A3 RID: 21667
		Public max As Vector2
	End Structure
End Namespace
