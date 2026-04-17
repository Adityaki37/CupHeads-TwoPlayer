Imports System
Imports UnityEngine

' Token: 0x02000367 RID: 871
Public Module ArrayExtensions
	' Token: 0x060009BC RID: 2492 RVA: 0x0007CBD2 File Offset: 0x0007AFD2
	<System.Runtime.CompilerServices.ExtensionAttribute()>
	Public Function GetRandom(Of T)(array As T()) As T
		Return array(Global.UnityEngine.Random.Range(0, array.Length))
	End Function

	' Token: 0x060009BD RID: 2493 RVA: 0x0007CBE3 File Offset: 0x0007AFE3
	<System.Runtime.CompilerServices.ExtensionAttribute()>
	Public Function GetLast(Of T)(array As T()) As T
		Return array(array.Length - 1)
	End Function
End Module
