Imports System
Imports UnityEngine

Namespace TMPro
	' Token: 0x02000C8D RID: 3213
	<Serializable()>
	Public Class TMP_TextInfo
		' Token: 0x0600511F RID: 20767 RVA: 0x002961AC File Offset: 0x002945AC
		Public Sub New()
			Me.characterInfo = New TMP_CharacterInfo(7) {}
			Me.wordInfo = New TMP_WordInfo(15) {}
			Me.linkInfo = New TMP_LinkInfo(-1) {}
			Me.lineInfo = New TMP_LineInfo(1) {}
			Me.pageInfo = New TMP_PageInfo(15) {}
			Me.meshInfo = New TMP_MeshInfo(0) {}
		End Sub

		' Token: 0x06005120 RID: 20768 RVA: 0x0029620C File Offset: 0x0029460C
		Public Sub New(textComponent As TMP_Text)
			Me.textComponent = textComponent
			Me.characterInfo = New TMP_CharacterInfo(7) {}
			Me.wordInfo = New TMP_WordInfo(3) {}
			Me.linkInfo = New TMP_LinkInfo(-1) {}
			Me.lineInfo = New TMP_LineInfo(1) {}
			Me.pageInfo = New TMP_PageInfo(15) {}
			Me.meshInfo = New TMP_MeshInfo(0) {}
			Me.meshInfo(0).mesh = textComponent.mesh
			Me.materialCount = 1
		End Sub

		' Token: 0x06005121 RID: 20769 RVA: 0x00296290 File Offset: 0x00294690
		Public Sub Clear()
			Me.characterCount = 0
			Me.spaceCount = 0
			Me.wordCount = 0
			Me.linkCount = 0
			Me.lineCount = 0
			Me.pageCount = 0
			Me.spriteCount = 0
			For i As Integer = 0 To Me.meshInfo.Length - 1
				Me.meshInfo(i).vertexCount = 0
			Next
		End Sub

		' Token: 0x06005122 RID: 20770 RVA: 0x002962FC File Offset: 0x002946FC
		Public Sub ClearMeshInfo(updateMesh As Boolean)
			For i As Integer = 0 To Me.meshInfo.Length - 1
				Me.meshInfo(i).Clear(updateMesh)
			Next
		End Sub

		' Token: 0x06005123 RID: 20771 RVA: 0x00296334 File Offset: 0x00294734
		Public Sub ClearAllMeshInfo()
			For i As Integer = 0 To Me.meshInfo.Length - 1
				Me.meshInfo(i).Clear(True)
			Next
		End Sub

		' Token: 0x06005124 RID: 20772 RVA: 0x0029636C File Offset: 0x0029476C
		Public Sub ClearUnusedVertices(materials As MaterialReference())
			For i As Integer = 0 To Me.meshInfo.Length - 1
				Dim num As Integer = 0
				Me.meshInfo(i).ClearUnusedVertices(num)
			Next
		End Sub

		' Token: 0x06005125 RID: 20773 RVA: 0x002963A8 File Offset: 0x002947A8
		Public Sub ClearLineInfo()
			If Me.lineInfo Is Nothing Then
				Me.lineInfo = New TMP_LineInfo(1) {}
			End If
			For i As Integer = 0 To Me.lineInfo.Length - 1
				Me.lineInfo(i).characterCount = 0
				Me.lineInfo(i).spaceCount = 0
				Me.lineInfo(i).width = 0F
				Me.lineInfo(i).ascender = TMP_TextInfo.k_InfinityVectorNegative.x
				Me.lineInfo(i).descender = TMP_TextInfo.k_InfinityVectorPositive.x
				Me.lineInfo(i).lineExtents.min = TMP_TextInfo.k_InfinityVectorPositive
				Me.lineInfo(i).lineExtents.max = TMP_TextInfo.k_InfinityVectorNegative
				Me.lineInfo(i).maxAdvance = 0F
			Next
		End Sub

		' Token: 0x06005126 RID: 20774 RVA: 0x002964A4 File Offset: 0x002948A4
		Public Shared Sub Resize(Of T)(ByRef array As T(), size As Integer)
			Dim num As Integer = If((size <= 1024), Mathf.NextPowerOfTwo(size), (size + 256))
			Array.Resize(Of T)(array, num)
		End Sub

		' Token: 0x06005127 RID: 20775 RVA: 0x002964D6 File Offset: 0x002948D6
		Public Shared Sub Resize(Of T)(ByRef array As T(), size As Integer, isBlockAllocated As Boolean)
			If size <= array.Length Then
				Return
			End If
			If isBlockAllocated Then
				size = If((size <= 1024), Mathf.NextPowerOfTwo(size), (size + 256))
			End If
			Array.Resize(Of T)(array, size)
		End Sub

		' Token: 0x040053D2 RID: 21458
		Private Shared k_InfinityVectorPositive As Vector2 = New Vector2(1000000F, 1000000F)

		' Token: 0x040053D3 RID: 21459
		Private Shared k_InfinityVectorNegative As Vector2 = New Vector2(-1000000F, -1000000F)

		' Token: 0x040053D4 RID: 21460
		Public textComponent As TMP_Text

		' Token: 0x040053D5 RID: 21461
		Public characterCount As Integer

		' Token: 0x040053D6 RID: 21462
		Public spriteCount As Integer

		' Token: 0x040053D7 RID: 21463
		Public spaceCount As Integer

		' Token: 0x040053D8 RID: 21464
		Public wordCount As Integer

		' Token: 0x040053D9 RID: 21465
		Public linkCount As Integer

		' Token: 0x040053DA RID: 21466
		Public lineCount As Integer

		' Token: 0x040053DB RID: 21467
		Public pageCount As Integer

		' Token: 0x040053DC RID: 21468
		Public materialCount As Integer

		' Token: 0x040053DD RID: 21469
		Public characterInfo As TMP_CharacterInfo()

		' Token: 0x040053DE RID: 21470
		Public wordInfo As TMP_WordInfo()

		' Token: 0x040053DF RID: 21471
		Public linkInfo As TMP_LinkInfo()

		' Token: 0x040053E0 RID: 21472
		Public lineInfo As TMP_LineInfo()

		' Token: 0x040053E1 RID: 21473
		Public pageInfo As TMP_PageInfo()

		' Token: 0x040053E2 RID: 21474
		Public meshInfo As TMP_MeshInfo()
	End Class
End Namespace
