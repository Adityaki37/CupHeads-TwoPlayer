Imports System
Imports System.Collections.Generic
Imports UnityEngine

' Token: 0x02000924 RID: 2340
<Serializable()>
Public Class TranslationElement
	Implements ISerializationCallbackReceiver

	' Token: 0x060036AC RID: 13996 RVA: 0x001FA137 File Offset: 0x001F8537
	Public Sub New()
	End Sub

	' Token: 0x060036AD RID: 13997 RVA: 0x001FA162 File Offset: 0x001F8562
	Public Sub New(key As String, depth As Integer, id As Integer)
		Me.key = key
		Me.m_ID = id
		Me.m_Depth = depth
	End Sub

	' Token: 0x060036AE RID: 13998 RVA: 0x001FA1A4 File Offset: 0x001F85A4
	Public Sub New(key As String, category As Localization.Categories, description As String, translation1 As String, translation2 As String, depth As Integer, id As Integer)
		Me.m_ID = id
		Me.m_Depth = depth
		Me.key = key
		Me.category = category
		Me.description = description
		Me.translations(CInt(Localization.language1)).text = translation1
		Me.translations(CInt(Localization.language2)).text = translation2
	End Sub

	' Token: 0x17000472 RID: 1138
	' (get) Token: 0x060036AF RID: 13999 RVA: 0x001FA230 File Offset: 0x001F8630
	' (set) Token: 0x060036B0 RID: 14000 RVA: 0x001FA2CC File Offset: 0x001F86CC
	Public Property translation As Localization.Translation
		Get
			If Not PlayerManager.Multiplayer AndAlso Me.translationsCuphead IsNot Nothing AndAlso CType(Me.translationsCuphead.Length, Localization.Languages) > Localization.language Then
				Return Me.translationsCuphead(CInt(Localization.language))
			End If
			If Not PlayerManager.Multiplayer AndAlso Me.translationsMugman IsNot Nothing AndAlso CType(Me.translationsMugman.Length, Localization.Languages) > Localization.language Then
				Return Me.translationsMugman(CInt(Localization.language))
			End If
			Return Me.translations(CInt(Localization.language))
		End Get
		Set(value As Localization.Translation)
			If Not PlayerManager.Multiplayer AndAlso Me.translationsCuphead IsNot Nothing AndAlso CType(Me.translationsCuphead.Length, Localization.Languages) > Localization.language Then
				Me.translationsCuphead(CInt(Localization.language)) = value
			End If
			If Not PlayerManager.Multiplayer AndAlso Me.translationsMugman IsNot Nothing AndAlso CType(Me.translationsMugman.Length, Localization.Languages) > Localization.language Then
				Me.translationsMugman(CInt(Localization.language)) = value
			End If
			Me.translations(CInt(Localization.language)) = value
		End Set
	End Property

	' Token: 0x17000473 RID: 1139
	' (get) Token: 0x060036B1 RID: 14001 RVA: 0x001FA369 File Offset: 0x001F8769
	' (set) Token: 0x060036B2 RID: 14002 RVA: 0x001FA371 File Offset: 0x001F8771
	Public Property depth As Integer
		Get
			Return Me.m_Depth
		End Get
		Set(value As Integer)
			Me.m_Depth = value
		End Set
	End Property

	' Token: 0x17000474 RID: 1140
	' (get) Token: 0x060036B3 RID: 14003 RVA: 0x001FA37A File Offset: 0x001F877A
	' (set) Token: 0x060036B4 RID: 14004 RVA: 0x001FA382 File Offset: 0x001F8782
	Public Property parent As TranslationElement
		Get
			Return Me.m_Parent
		End Get
		Set(value As TranslationElement)
			Me.m_Parent = value
		End Set
	End Property

	' Token: 0x17000475 RID: 1141
	' (get) Token: 0x060036B5 RID: 14005 RVA: 0x001FA38B File Offset: 0x001F878B
	' (set) Token: 0x060036B6 RID: 14006 RVA: 0x001FA393 File Offset: 0x001F8793
	Public Property children As List(Of TranslationElement)
		Get
			Return Me.m_Children
		End Get
		Set(value As List(Of TranslationElement))
			Me.m_Children = value
		End Set
	End Property

	' Token: 0x17000476 RID: 1142
	' (get) Token: 0x060036B7 RID: 14007 RVA: 0x001FA39C File Offset: 0x001F879C
	Public ReadOnly Property hasChildren As Boolean
		Get
			Return Me.children IsNot Nothing AndAlso Me.children.Count > 0
		End Get
	End Property

	' Token: 0x17000477 RID: 1143
	' (get) Token: 0x060036B8 RID: 14008 RVA: 0x001FA3BA File Offset: 0x001F87BA
	' (set) Token: 0x060036B9 RID: 14009 RVA: 0x001FA3C2 File Offset: 0x001F87C2
	Public Property id As Integer
		Get
			Return Me.m_ID
		End Get
		Set(value As Integer)
			Me.m_ID = value
		End Set
	End Property

	' Token: 0x060036BA RID: 14010 RVA: 0x001FA3CC File Offset: 0x001F87CC
	Private Function Grow(oldTranslations As Localization.Translation(), newLength As Integer) As Localization.Translation()
		Dim array As Localization.Translation() = New Localization.Translation(newLength - 1) {}
		For i As Integer = 0 To oldTranslations.Length - 1
			array(i).fonts = oldTranslations(i).fonts
			array(i).image = oldTranslations(i).image
			array(i).spriteAtlasName = oldTranslations(i).spriteAtlasName
			array(i).spriteAtlasImageName = oldTranslations(i).spriteAtlasImageName
			array(i).hasImage = oldTranslations(i).hasImage
			array(i).text = oldTranslations(i).text
		Next
		For j As Integer = oldTranslations.Length To array.Length - 1
			array(j).fonts = Nothing
			array(j).image = Nothing
			array(j).hasImage = False
			array(j).text = String.Empty
			array(j).spriteAtlasName = String.Empty
			array(j).spriteAtlasImageName = String.Empty
		Next
		Return array
	End Function

	' Token: 0x060036BB RID: 14011 RVA: 0x001FA4F5 File Offset: 0x001F88F5
	Public Sub OnBeforeSerialize() Implements UnityEngine.ISerializationCallbackReceiver.OnBeforeSerialize
	End Sub

	' Token: 0x060036BC RID: 14012 RVA: 0x001FA4F8 File Offset: 0x001F88F8
	Public Sub OnAfterDeserialize() Implements UnityEngine.ISerializationCallbackReceiver.OnAfterDeserialize
		Dim num As Integer = [Enum].GetNames(GetType(Localization.Languages)).Length
		If Me.translations.Length < num Then
			Me.translations = Me.Grow(Me.translations, num)
			If Me.translationsCuphead IsNot Nothing Then
				Me.translationsCuphead = Me.Grow(Me.translationsCuphead, num)
			End If
			If Me.translationsMugman IsNot Nothing Then
				Me.translationsMugman = Me.Grow(Me.translationsMugman, num)
			End If
		End If
	End Sub

	' Token: 0x04003EDC RID: 16092
	<SerializeField()>
	Private m_ID As Integer

	' Token: 0x04003EDD RID: 16093
	<SerializeField()>
	Private m_Depth As Integer

	' Token: 0x04003EDE RID: 16094
	<NonSerialized()>
	Private m_Parent As TranslationElement

	' Token: 0x04003EDF RID: 16095
	<NonSerialized()>
	Private m_Children As List(Of TranslationElement)

	' Token: 0x04003EE0 RID: 16096
	<SerializeField()>
	Public key As String = String.Empty

	' Token: 0x04003EE1 RID: 16097
	<SerializeField()>
	Public category As Localization.Categories

	' Token: 0x04003EE2 RID: 16098
	<SerializeField()>
	Public description As String = String.Empty

	' Token: 0x04003EE3 RID: 16099
	<SerializeField()>
	Public translations As Localization.Translation() = New Localization.Translation(11) {}

	' Token: 0x04003EE4 RID: 16100
	<SerializeField()>
	Public translationsCuphead As Localization.Translation()

	' Token: 0x04003EE5 RID: 16101
	<SerializeField()>
	Public translationsMugman As Localization.Translation()

	' Token: 0x04003EE6 RID: 16102
	Public enabled As Boolean

	' Token: 0x04003EE7 RID: 16103
	<NonSerialized()>
	Public multiplayerLock As Boolean
End Class
