Imports System
Imports UnityEngine

Namespace RektTransform
	' Token: 0x0200036C RID: 876
	Public Structure MinMax
		' Token: 0x060009C6 RID: 2502 RVA: 0x0007D020 File Offset: 0x0007B420
		Public Sub New(min As Vector2, max As Vector2)
			Me.min = New Vector2(Mathf.Clamp01(min.x), Mathf.Clamp01(min.y))
			Me.max = New Vector2(Mathf.Clamp01(max.x), Mathf.Clamp01(max.y))
		End Sub

		' Token: 0x060009C7 RID: 2503 RVA: 0x0007D073 File Offset: 0x0007B473
		Public Sub New(minx As Single, miny As Single, maxx As Single, maxy As Single)
			Me.min = New Vector2(Mathf.Clamp01(minx), Mathf.Clamp01(miny))
			Me.max = New Vector2(Mathf.Clamp01(maxx), Mathf.Clamp01(maxy))
		End Sub

		' Token: 0x0400145B RID: 5211
		Public min As Vector2

		' Token: 0x0400145C RID: 5212
		Public max As Vector2
	End Structure
End Namespace
