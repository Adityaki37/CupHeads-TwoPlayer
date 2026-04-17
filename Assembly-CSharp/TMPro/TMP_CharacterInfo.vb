Imports System
Imports UnityEngine

Namespace TMPro
	' Token: 0x02000CA4 RID: 3236
	Public Structure TMP_CharacterInfo
		' Token: 0x04005452 RID: 21586
		Public character As Char

		' Token: 0x04005453 RID: 21587
		Public index As Short

		' Token: 0x04005454 RID: 21588
		Public elementType As TMP_TextElementType

		' Token: 0x04005455 RID: 21589
		Public textElement As TMP_TextElement

		' Token: 0x04005456 RID: 21590
		Public fontAsset As TMP_FontAsset

		' Token: 0x04005457 RID: 21591
		Public spriteAsset As TMP_SpriteAsset

		' Token: 0x04005458 RID: 21592
		Public spriteIndex As Integer

		' Token: 0x04005459 RID: 21593
		Public material As Material

		' Token: 0x0400545A RID: 21594
		Public materialReferenceIndex As Integer

		' Token: 0x0400545B RID: 21595
		Public pointSize As Single

		' Token: 0x0400545C RID: 21596
		Public lineNumber As Short

		' Token: 0x0400545D RID: 21597
		Public pageNumber As Short

		' Token: 0x0400545E RID: 21598
		Public vertexIndex As Short

		' Token: 0x0400545F RID: 21599
		Public vertex_TL As TMP_Vertex

		' Token: 0x04005460 RID: 21600
		Public vertex_BL As TMP_Vertex

		' Token: 0x04005461 RID: 21601
		Public vertex_TR As TMP_Vertex

		' Token: 0x04005462 RID: 21602
		Public vertex_BR As TMP_Vertex

		' Token: 0x04005463 RID: 21603
		Public topLeft As Vector3

		' Token: 0x04005464 RID: 21604
		Public bottomLeft As Vector3

		' Token: 0x04005465 RID: 21605
		Public topRight As Vector3

		' Token: 0x04005466 RID: 21606
		Public bottomRight As Vector3

		' Token: 0x04005467 RID: 21607
		Public origin As Single

		' Token: 0x04005468 RID: 21608
		Public ascender As Single

		' Token: 0x04005469 RID: 21609
		Public baseLine As Single

		' Token: 0x0400546A RID: 21610
		Public descender As Single

		' Token: 0x0400546B RID: 21611
		Public xAdvance As Single

		' Token: 0x0400546C RID: 21612
		Public aspectRatio As Single

		' Token: 0x0400546D RID: 21613
		Public scale As Single

		' Token: 0x0400546E RID: 21614
		Public color As Color32

		' Token: 0x0400546F RID: 21615
		Public style As FontStyles

		' Token: 0x04005470 RID: 21616
		Public isVisible As Boolean
	End Structure
End Namespace
