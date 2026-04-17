Imports System
Imports System.Collections.Generic

Namespace TMPro
	' Token: 0x02000C72 RID: 3186
	Friend Module TMP_ListPool(Of T)
		' Token: 0x06004FD8 RID: 20440 RVA: 0x00294ACB File Offset: 0x00292ECB
		Public Function [Get]() As List(Of T)
			Return TMP_ListPool(Of T).s_ListPool.[Get]()
		End Function

		' Token: 0x06004FD9 RID: 20441 RVA: 0x00294AD7 File Offset: 0x00292ED7
		Public Sub Release(toRelease As List(Of T))
			TMP_ListPool(Of T).s_ListPool.Release(toRelease)
		End Sub

		' Token: 0x04005292 RID: 21138
		Private s_ListPool As TMP_ObjectPool(Of List(Of T)) = New TMP_ObjectPool(Of List(Of T))(Nothing, Sub(l As List(Of T))
			l.Clear()
		End Sub)
	End Module
End Namespace
