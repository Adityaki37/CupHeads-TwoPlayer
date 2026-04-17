Imports System
Imports UnityEngine

' Token: 0x020005A5 RID: 1445
Public Class DicePalaceCardLevelBlock
	Inherits LevelPlatform

	' Token: 0x06001BE5 RID: 7141 RVA: 0x000FFE53 File Offset: 0x000FE253
	Public Overrides Sub AddChild(player As Transform)
	End Sub

	' Token: 0x06001BE6 RID: 7142 RVA: 0x000FFE55 File Offset: 0x000FE255
	Public Sub DestroyBlock()
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub

	' Token: 0x040024F1 RID: 9457
	Public suit As DicePalaceCardLevelBlock.Suit

	' Token: 0x040024F2 RID: 9458
	Public stopOffsetX As Integer

	' Token: 0x040024F3 RID: 9459
	Public gridBlock As DicePalaceCardLevelGridBlock(,)

	' Token: 0x040024F4 RID: 9460
	Private YCheck As Single

	' Token: 0x040024F5 RID: 9461
	Private damageDealer As DamageDealer

	' Token: 0x020005A6 RID: 1446
	Public Enum Suit
		' Token: 0x040024F7 RID: 9463
		Hearts = 1
		' Token: 0x040024F8 RID: 9464
		Spades
		' Token: 0x040024F9 RID: 9465
		Clubs
		' Token: 0x040024FA RID: 9466
		Diamonds
	End Enum
End Class
