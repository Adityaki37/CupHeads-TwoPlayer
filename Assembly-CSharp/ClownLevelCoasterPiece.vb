Imports System
Imports System.Collections.Generic
Imports UnityEngine

' Token: 0x02000564 RID: 1380
Public Class ClownLevelCoasterPiece
	Inherits AbstractCollidableObject

	' Token: 0x060019F8 RID: 6648 RVA: 0x000ED8B7 File Offset: 0x000EBCB7
	Public Sub Init(startPos As Vector3)
		MyBase.transform.position = startPos
	End Sub

	' Token: 0x0400231A RID: 8986
	Public riders As List(Of ClownLevelRiders)

	' Token: 0x0400231B RID: 8987
	Public newPieceRoot As Transform

	' Token: 0x0400231C RID: 8988
	Public tailRoot As Transform

	' Token: 0x0400231D RID: 8989
	Public ridersFrontRoot As Transform

	' Token: 0x0400231E RID: 8990
	Public ridersBackRoot As Transform
End Class
