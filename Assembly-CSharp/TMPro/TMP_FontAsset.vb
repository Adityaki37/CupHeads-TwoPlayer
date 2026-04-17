Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports UnityEngine

Namespace TMPro
	' Token: 0x02000C70 RID: 3184
	<Serializable()>
	Public Class TMP_FontAsset
		Inherits TMP_Asset

		' Token: 0x17000831 RID: 2097
		' (get) Token: 0x06004FC7 RID: 20423 RVA: 0x002942B3 File Offset: 0x002926B3
		Public Shared ReadOnly Property defaultFontAsset As TMP_FontAsset
			Get
				If TMP_FontAsset.s_defaultFontAsset Is Nothing Then
					TMP_FontAsset.s_defaultFontAsset = Resources.Load(Of TMP_FontAsset)("Fonts & Materials/ARIAL SDF")
				End If
				Return TMP_FontAsset.s_defaultFontAsset
			End Get
		End Property

		' Token: 0x17000832 RID: 2098
		' (get) Token: 0x06004FC8 RID: 20424 RVA: 0x002942D9 File Offset: 0x002926D9
		Public ReadOnly Property fontInfo As FaceInfo
			Get
				Return Me.m_fontInfo
			End Get
		End Property

		' Token: 0x17000833 RID: 2099
		' (get) Token: 0x06004FC9 RID: 20425 RVA: 0x002942E1 File Offset: 0x002926E1
		Public ReadOnly Property characterDictionary As Dictionary(Of Integer, TMP_Glyph)
			Get
				Return Me.m_characterDictionary
			End Get
		End Property

		' Token: 0x17000834 RID: 2100
		' (get) Token: 0x06004FCA RID: 20426 RVA: 0x002942E9 File Offset: 0x002926E9
		Public ReadOnly Property kerningDictionary As Dictionary(Of Integer, KerningPair)
			Get
				Return Me.m_kerningDictionary
			End Get
		End Property

		' Token: 0x17000835 RID: 2101
		' (get) Token: 0x06004FCB RID: 20427 RVA: 0x002942F1 File Offset: 0x002926F1
		Public ReadOnly Property kerningInfo As KerningTable
			Get
				Return Me.m_kerningInfo
			End Get
		End Property

		' Token: 0x17000836 RID: 2102
		' (get) Token: 0x06004FCC RID: 20428 RVA: 0x002942F9 File Offset: 0x002926F9
		Public ReadOnly Property lineBreakingInfo As LineBreakingTable
			Get
				Return Me.m_lineBreakingInfo
			End Get
		End Property

		' Token: 0x06004FCD RID: 20429 RVA: 0x00294301 File Offset: 0x00292701
		Private Sub OnEnable()
			If Me.m_characterDictionary Is Nothing Then
				Me.ReadFontDefinition()
			End If
		End Sub

		' Token: 0x06004FCE RID: 20430 RVA: 0x00294319 File Offset: 0x00292719
		Private Sub OnDisable()
		End Sub

		' Token: 0x06004FCF RID: 20431 RVA: 0x0029431B File Offset: 0x0029271B
		Public Sub AddFaceInfo(faceInfo As FaceInfo)
			Me.m_fontInfo = faceInfo
		End Sub

		' Token: 0x06004FD0 RID: 20432 RVA: 0x00294324 File Offset: 0x00292724
		Public Sub AddGlyphInfo(glyphInfo As TMP_Glyph())
			Me.m_glyphInfoList = New List(Of TMP_Glyph)()
			Dim num As Integer = glyphInfo.Length
			Me.m_fontInfo.CharacterCount = num
			Me.m_characterSet = New Integer(num - 1) {}
			For i As Integer = 0 To num - 1
				Dim tmp_Glyph As TMP_Glyph = New TMP_Glyph()
				tmp_Glyph.id = glyphInfo(i).id
				tmp_Glyph.x = glyphInfo(i).x
				tmp_Glyph.y = glyphInfo(i).y
				tmp_Glyph.width = glyphInfo(i).width
				tmp_Glyph.height = glyphInfo(i).height
				tmp_Glyph.xOffset = glyphInfo(i).xOffset
				tmp_Glyph.yOffset = glyphInfo(i).yOffset + Me.m_fontInfo.Padding
				tmp_Glyph.xAdvance = glyphInfo(i).xAdvance
				Me.m_glyphInfoList.Add(tmp_Glyph)
				Me.m_characterSet(i) = tmp_Glyph.id
			Next
			Me.m_glyphInfoList = Me.m_glyphInfoList.OrderBy(Function(s As TMP_Glyph) s.id).ToList()
		End Sub

		' Token: 0x06004FD1 RID: 20433 RVA: 0x00294439 File Offset: 0x00292839
		Public Sub AddKerningInfo(kerningTable As KerningTable)
			Me.m_kerningInfo = kerningTable
		End Sub

		' Token: 0x06004FD2 RID: 20434 RVA: 0x00294444 File Offset: 0x00292844
		Public Sub ReadFontDefinition()
			If Me.m_fontInfo Is Nothing Then
				Return
			End If
			Me.m_characterDictionary = New Dictionary(Of Integer, TMP_Glyph)()
			For Each tmp_Glyph As TMP_Glyph In Me.m_glyphInfoList
				If Not Me.m_characterDictionary.ContainsKey(tmp_Glyph.id) Then
					Me.m_characterDictionary.Add(tmp_Glyph.id, tmp_Glyph)
				End If
			Next
			Dim tmp_Glyph2 As TMP_Glyph = New TMP_Glyph()
			If Me.m_characterDictionary.ContainsKey(32) Then
				Me.m_characterDictionary(32).width = Me.m_characterDictionary(32).xAdvance
				Me.m_characterDictionary(32).height = Me.m_fontInfo.Ascender - Me.m_fontInfo.Descender
				Me.m_characterDictionary(32).yOffset = Me.m_fontInfo.Ascender
			Else
				tmp_Glyph2 = New TMP_Glyph()
				tmp_Glyph2.id = 32
				tmp_Glyph2.x = 0F
				tmp_Glyph2.y = 0F
				tmp_Glyph2.width = Me.m_fontInfo.Ascender / 5F
				tmp_Glyph2.height = Me.m_fontInfo.Ascender - Me.m_fontInfo.Descender
				tmp_Glyph2.xOffset = 0F
				tmp_Glyph2.yOffset = Me.m_fontInfo.Ascender
				tmp_Glyph2.xAdvance = Me.m_fontInfo.PointSize / 4F
				Me.m_characterDictionary.Add(32, tmp_Glyph2)
			End If
			If Not Me.m_characterDictionary.ContainsKey(160) Then
				tmp_Glyph2 = TMP_Glyph.Clone(Me.m_characterDictionary(32))
				Me.m_characterDictionary.Add(160, tmp_Glyph2)
			End If
			If Not Me.m_characterDictionary.ContainsKey(8203) Then
				tmp_Glyph2 = TMP_Glyph.Clone(Me.m_characterDictionary(32))
				tmp_Glyph2.width = 0F
				tmp_Glyph2.xAdvance = 0F
				Me.m_characterDictionary.Add(8203, tmp_Glyph2)
			End If
			If Not Me.m_characterDictionary.ContainsKey(10) Then
				tmp_Glyph2 = New TMP_Glyph()
				tmp_Glyph2.id = 10
				tmp_Glyph2.x = 0F
				tmp_Glyph2.y = 0F
				tmp_Glyph2.width = 10F
				tmp_Glyph2.height = Me.m_characterDictionary(32).height
				tmp_Glyph2.xOffset = 0F
				tmp_Glyph2.yOffset = Me.m_characterDictionary(32).yOffset
				tmp_Glyph2.xAdvance = 0F
				Me.m_characterDictionary.Add(10, tmp_Glyph2)
				If Not Me.m_characterDictionary.ContainsKey(13) Then
					Me.m_characterDictionary.Add(13, tmp_Glyph2)
				End If
			End If
			If Not Me.m_characterDictionary.ContainsKey(9) Then
				tmp_Glyph2 = New TMP_Glyph()
				tmp_Glyph2.id = 9
				tmp_Glyph2.x = Me.m_characterDictionary(32).x
				tmp_Glyph2.y = Me.m_characterDictionary(32).y
				tmp_Glyph2.width = Me.m_characterDictionary(32).width * CSng(Me.tabSize) + (Me.m_characterDictionary(32).xAdvance - Me.m_characterDictionary(32).width) * CSng((Me.tabSize - 1))
				tmp_Glyph2.height = Me.m_characterDictionary(32).height
				tmp_Glyph2.xOffset = Me.m_characterDictionary(32).xOffset
				tmp_Glyph2.yOffset = Me.m_characterDictionary(32).yOffset
				tmp_Glyph2.xAdvance = Me.m_characterDictionary(32).xAdvance * CSng(Me.tabSize)
				Me.m_characterDictionary.Add(9, tmp_Glyph2)
			End If
			Me.m_fontInfo.TabWidth = Me.m_characterDictionary(9).xAdvance
			If Me.m_fontInfo.Scale = 0F Then
				Me.m_fontInfo.Scale = 1F
			End If
			Me.m_kerningDictionary = New Dictionary(Of Integer, KerningPair)()
			Dim kerningPairs As List(Of KerningPair) = Me.m_kerningInfo.kerningPairs
			For i As Integer = 0 To kerningPairs.Count - 1
				Dim kerningPair As KerningPair = kerningPairs(i)
				Dim kerningPairKey As KerningPairKey = New KerningPairKey(kerningPair.AscII_Left, kerningPair.AscII_Right)
				If Not Me.m_kerningDictionary.ContainsKey(kerningPairKey.key) Then
					Me.m_kerningDictionary.Add(kerningPairKey.key, kerningPair)
				ElseIf Not TMP_Settings.warningsDisabled Then
				End If
			Next
			Me.m_lineBreakingInfo = New LineBreakingTable()
			Dim textAsset As TextAsset = TryCast(Resources.Load("LineBreaking Leading Characters", GetType(TextAsset)), TextAsset)
			If textAsset IsNot Nothing Then
				Me.m_lineBreakingInfo.leadingCharacters = Me.GetCharacters(textAsset)
			End If
			Dim textAsset2 As TextAsset = TryCast(Resources.Load("LineBreaking Following Characters", GetType(TextAsset)), TextAsset)
			If textAsset2 IsNot Nothing Then
				Me.m_lineBreakingInfo.followingCharacters = Me.GetCharacters(textAsset2)
			End If
			Me.hashCode = TMP_TextUtilities.GetSimpleHashCode(MyBase.name)
			Me.materialHashCode = TMP_TextUtilities.GetSimpleHashCode(Me.material.name)
		End Sub

		' Token: 0x06004FD3 RID: 20435 RVA: 0x002949C0 File Offset: 0x00292DC0
		Private Function GetCharacters(file As TextAsset) As Dictionary(Of Integer, Char)
			Dim dictionary As Dictionary(Of Integer, Char) = New Dictionary(Of Integer, Char)()
			For Each c As Char In file.text
				If Not dictionary.ContainsKey(CInt(c)) Then
					dictionary.Add(CInt(c), c)
				End If
			Next
			Return dictionary
		End Function

		' Token: 0x06004FD4 RID: 20436 RVA: 0x00294A0E File Offset: 0x00292E0E
		Public Function HasCharacter(character As Integer) As Boolean
			Return Me.m_characterDictionary IsNot Nothing AndAlso Me.m_characterDictionary.ContainsKey(character)
		End Function

		' Token: 0x06004FD5 RID: 20437 RVA: 0x00294A31 File Offset: 0x00292E31
		Public Function HasCharacter(character As Char) As Boolean
			Return Me.m_characterDictionary IsNot Nothing AndAlso Me.m_characterDictionary.ContainsKey(CInt(character))
		End Function

		' Token: 0x06004FD6 RID: 20438 RVA: 0x00294A54 File Offset: 0x00292E54
		Public Function HasCharacters(text As String, <System.Runtime.InteropServices.OutAttribute()> ByRef missingCharacters As List(Of Char)) As Boolean
			If Me.m_characterDictionary Is Nothing Then
				missingCharacters = Nothing
				Return False
			End If
			missingCharacters = New List(Of Char)()
			For i As Integer = 0 To text.Length - 1
				If Not Me.m_characterDictionary.ContainsKey(CInt(text(i))) Then
					missingCharacters.Add(text(i))
				End If
			Next
			Return missingCharacters.Count = 0
		End Function

		' Token: 0x04005278 RID: 21112
		Private Shared s_defaultFontAsset As TMP_FontAsset

		' Token: 0x04005279 RID: 21113
		Public fontAssetType As TMP_FontAsset.FontAssetTypes

		' Token: 0x0400527A RID: 21114
		<SerializeField()>
		Private m_fontInfo As FaceInfo

		' Token: 0x0400527B RID: 21115
		<SerializeField()>
		Public atlas As Texture2D

		' Token: 0x0400527C RID: 21116
		<SerializeField()>
		Private m_glyphInfoList As List(Of TMP_Glyph)

		' Token: 0x0400527D RID: 21117
		Private m_characterDictionary As Dictionary(Of Integer, TMP_Glyph)

		' Token: 0x0400527E RID: 21118
		Private m_kerningDictionary As Dictionary(Of Integer, KerningPair)

		' Token: 0x0400527F RID: 21119
		<SerializeField()>
		Private m_kerningInfo As KerningTable

		' Token: 0x04005280 RID: 21120
		<SerializeField()>
		Private m_kerningPair As KerningPair

		' Token: 0x04005281 RID: 21121
		<SerializeField()>
		Private m_lineBreakingInfo As LineBreakingTable

		' Token: 0x04005282 RID: 21122
		<SerializeField()>
		Public fallbackFontAssets As List(Of TMP_FontAsset)

		' Token: 0x04005283 RID: 21123
		<SerializeField()>
		Public fontCreationSettings As FontCreationSetting

		' Token: 0x04005284 RID: 21124
		Public fontWeights As TMP_FontWeights() = New TMP_FontWeights(9) {}

		' Token: 0x04005285 RID: 21125
		Private m_characterSet As Integer()

		' Token: 0x04005286 RID: 21126
		Public normalStyle As Single

		' Token: 0x04005287 RID: 21127
		Public normalSpacingOffset As Single

		' Token: 0x04005288 RID: 21128
		Public boldStyle As Single = 0.75F

		' Token: 0x04005289 RID: 21129
		Public boldSpacing As Single = 7F

		' Token: 0x0400528A RID: 21130
		Public italicStyle As Byte = 35

		' Token: 0x0400528B RID: 21131
		Public tabSize As Byte = 10

		' Token: 0x0400528C RID: 21132
		Private m_oldTabSize As Byte

		' Token: 0x02000C71 RID: 3185
		Public Enum FontAssetTypes
			' Token: 0x0400528F RID: 21135
			None
			' Token: 0x04005290 RID: 21136
			SDF
			' Token: 0x04005291 RID: 21137
			Bitmap
		End Enum
	End Class
End Namespace
