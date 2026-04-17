Imports System
Imports UnityEngine

Namespace TMPro
	' Token: 0x02000CAE RID: 3246
	Public Structure WordWrapState
		' Token: 0x040054A4 RID: 21668
		Public previous_WordBreak As Integer

		' Token: 0x040054A5 RID: 21669
		Public total_CharacterCount As Integer

		' Token: 0x040054A6 RID: 21670
		Public visible_CharacterCount As Integer

		' Token: 0x040054A7 RID: 21671
		Public visible_SpriteCount As Integer

		' Token: 0x040054A8 RID: 21672
		Public visible_LinkCount As Integer

		' Token: 0x040054A9 RID: 21673
		Public firstCharacterIndex As Integer

		' Token: 0x040054AA RID: 21674
		Public firstVisibleCharacterIndex As Integer

		' Token: 0x040054AB RID: 21675
		Public lastCharacterIndex As Integer

		' Token: 0x040054AC RID: 21676
		Public lastVisibleCharIndex As Integer

		' Token: 0x040054AD RID: 21677
		Public lineNumber As Integer

		' Token: 0x040054AE RID: 21678
		Public maxAscender As Single

		' Token: 0x040054AF RID: 21679
		Public maxDescender As Single

		' Token: 0x040054B0 RID: 21680
		Public maxLineAscender As Single

		' Token: 0x040054B1 RID: 21681
		Public maxLineDescender As Single

		' Token: 0x040054B2 RID: 21682
		Public previousLineAscender As Single

		' Token: 0x040054B3 RID: 21683
		Public xAdvance As Single

		' Token: 0x040054B4 RID: 21684
		Public preferredWidth As Single

		' Token: 0x040054B5 RID: 21685
		Public preferredHeight As Single

		' Token: 0x040054B6 RID: 21686
		Public previousLineScale As Single

		' Token: 0x040054B7 RID: 21687
		Public wordCount As Integer

		' Token: 0x040054B8 RID: 21688
		Public fontStyle As FontStyles

		' Token: 0x040054B9 RID: 21689
		Public fontScale As Single

		' Token: 0x040054BA RID: 21690
		Public fontScaleMultiplier As Single

		' Token: 0x040054BB RID: 21691
		Public currentFontSize As Single

		' Token: 0x040054BC RID: 21692
		Public baselineOffset As Single

		' Token: 0x040054BD RID: 21693
		Public lineOffset As Single

		' Token: 0x040054BE RID: 21694
		Public textInfo As TMP_TextInfo

		' Token: 0x040054BF RID: 21695
		Public lineInfo As TMP_LineInfo

		' Token: 0x040054C0 RID: 21696
		Public vertexColor As Color32

		' Token: 0x040054C1 RID: 21697
		Public colorStack As TMP_XmlTagStack(Of Color32)

		' Token: 0x040054C2 RID: 21698
		Public sizeStack As TMP_XmlTagStack(Of Single)

		' Token: 0x040054C3 RID: 21699
		Public fontWeightStack As TMP_XmlTagStack(Of Integer)

		' Token: 0x040054C4 RID: 21700
		Public styleStack As TMP_XmlTagStack(Of Integer)

		' Token: 0x040054C5 RID: 21701
		Public actionStack As TMP_XmlTagStack(Of Integer)

		' Token: 0x040054C6 RID: 21702
		Public materialReferenceStack As TMP_XmlTagStack(Of MaterialReference)

		' Token: 0x040054C7 RID: 21703
		Public currentFontAsset As TMP_FontAsset

		' Token: 0x040054C8 RID: 21704
		Public currentSpriteAsset As TMP_SpriteAsset

		' Token: 0x040054C9 RID: 21705
		Public currentMaterial As Material

		' Token: 0x040054CA RID: 21706
		Public currentMaterialIndex As Integer

		' Token: 0x040054CB RID: 21707
		Public meshExtents As Extents

		' Token: 0x040054CC RID: 21708
		Public tagNoParsing As Boolean
	End Structure
End Namespace
