Imports System
Imports System.Collections.Generic

' Token: 0x020005A8 RID: 1448
Public Class DicePalaceCardLevelColumn
	Inherits AbstractCollidableObject

	' Token: 0x040024FB RID: 9467
	Public blockPieces As List(Of DicePalaceCardLevelBlock)

	' Token: 0x040024FC RID: 9468
	Public blockXPos As Integer()

	' Token: 0x040024FD RID: 9469
	Public columnStopYPos As Integer()

	' Token: 0x040024FE RID: 9470
	Public blockCounter As Integer
End Class
