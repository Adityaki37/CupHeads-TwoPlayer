Imports System

Namespace TMPro
	' Token: 0x02000C9B RID: 3227
	<Serializable()>
	Public Class TMP_Glyph
		Inherits TMP_TextElement

		' Token: 0x0600517C RID: 20860 RVA: 0x002995DC File Offset: 0x002979DC
		Public Shared Function Clone(source As TMP_Glyph) As TMP_Glyph
			Return New TMP_Glyph() With { .id = source.id, .x = source.x, .y = source.y, .width = source.width, .height = source.height, .xOffset = source.xOffset, .yOffset = source.yOffset, .xAdvance = source.xAdvance }
		End Function
	End Class
End Namespace
