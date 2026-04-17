Imports System
Imports UnityEngine

' Token: 0x020005A9 RID: 1449
Public Class DicePalaceCardLevelGridBlock
	Inherits AbstractCollidableObject

	' Token: 0x06001BEB RID: 7147 RVA: 0x000FFE83 File Offset: 0x000FE283
	Protected Overrides Sub OnCollisionOther(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionOther(hit, phase)
	End Sub

	' Token: 0x040024FF RID: 9471
	Public Xcoordinate As Single

	' Token: 0x04002500 RID: 9472
	Public Ycoordinate As Single

	' Token: 0x04002501 RID: 9473
	Public hasBlock As Boolean

	' Token: 0x04002502 RID: 9474
	Public size As Single

	' Token: 0x04002503 RID: 9475
	Public blockHeld As DicePalaceCardLevelBlock
End Class
