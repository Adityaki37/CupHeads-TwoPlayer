Imports System
Imports UnityEngine

' Token: 0x020008FA RID: 2298
Public Class PlatformingLevelParallaxChild
	Inherits AbstractMonoBehaviour

	' Token: 0x1700045B RID: 1115
	' (get) Token: 0x060035E5 RID: 13797 RVA: 0x001F6444 File Offset: 0x001F4844
	Public ReadOnly Property SortingOrderOffset As Integer
		Get
			Return Me._sortingOrderOffset
		End Get
	End Property

	' Token: 0x04003DF9 RID: 15865
	<SerializeField()>
	Private _sortingOrderOffset As Integer
End Class
