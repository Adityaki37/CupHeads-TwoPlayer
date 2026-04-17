Imports System
Imports System.Collections.Generic
Imports UnityEngine

Namespace TMPro
	' Token: 0x02000C77 RID: 3191
	<ExecuteInEditMode()>
	<Serializable()>
	Public Class TMP_Settings
		Inherits ScriptableObject

		' Token: 0x1700083A RID: 2106
		' (get) Token: 0x06004FF1 RID: 20465 RVA: 0x00295260 File Offset: 0x00293660
		Public Shared ReadOnly Property enableWordWrapping As Boolean
			Get
				Return TMP_Settings.instance.m_enableWordWrapping
			End Get
		End Property

		' Token: 0x1700083B RID: 2107
		' (get) Token: 0x06004FF2 RID: 20466 RVA: 0x0029526C File Offset: 0x0029366C
		Public Shared ReadOnly Property enableKerning As Boolean
			Get
				Return TMP_Settings.instance.m_enableKerning
			End Get
		End Property

		' Token: 0x1700083C RID: 2108
		' (get) Token: 0x06004FF3 RID: 20467 RVA: 0x00295278 File Offset: 0x00293678
		Public Shared ReadOnly Property enableExtraPadding As Boolean
			Get
				Return TMP_Settings.instance.m_enableExtraPadding
			End Get
		End Property

		' Token: 0x1700083D RID: 2109
		' (get) Token: 0x06004FF4 RID: 20468 RVA: 0x00295284 File Offset: 0x00293684
		Public Shared ReadOnly Property enableTintAllSprites As Boolean
			Get
				Return TMP_Settings.instance.m_enableTintAllSprites
			End Get
		End Property

		' Token: 0x1700083E RID: 2110
		' (get) Token: 0x06004FF5 RID: 20469 RVA: 0x00295290 File Offset: 0x00293690
		Public Shared ReadOnly Property warningsDisabled As Boolean
			Get
				Return TMP_Settings.instance.m_warningsDisabled
			End Get
		End Property

		' Token: 0x1700083F RID: 2111
		' (get) Token: 0x06004FF6 RID: 20470 RVA: 0x0029529C File Offset: 0x0029369C
		Public Shared ReadOnly Property defaultFontAsset As TMP_FontAsset
			Get
				Return TMP_Settings.instance.m_defaultFontAsset
			End Get
		End Property

		' Token: 0x17000840 RID: 2112
		' (get) Token: 0x06004FF7 RID: 20471 RVA: 0x002952A8 File Offset: 0x002936A8
		Public Shared ReadOnly Property fallbackFontAssets As List(Of TMP_FontAsset)
			Get
				Return TMP_Settings.instance.m_fallbackFontAssets
			End Get
		End Property

		' Token: 0x17000841 RID: 2113
		' (get) Token: 0x06004FF8 RID: 20472 RVA: 0x002952B4 File Offset: 0x002936B4
		Public Shared ReadOnly Property defaultSpriteAsset As TMP_SpriteAsset
			Get
				Return TMP_Settings.instance.m_defaultSpriteAsset
			End Get
		End Property

		' Token: 0x17000842 RID: 2114
		' (get) Token: 0x06004FF9 RID: 20473 RVA: 0x002952C0 File Offset: 0x002936C0
		Public Shared ReadOnly Property defaultStyleSheet As TMP_StyleSheet
			Get
				Return TMP_Settings.instance.m_defaultStyleSheet
			End Get
		End Property

		' Token: 0x17000843 RID: 2115
		' (get) Token: 0x06004FFA RID: 20474 RVA: 0x002952CC File Offset: 0x002936CC
		Public Shared ReadOnly Property instance As TMP_Settings
			Get
				If TMP_Settings.s_Instance Is Nothing Then
					TMP_Settings.s_Instance = TryCast(Resources.Load("TMP Settings"), TMP_Settings)
				End If
				Return TMP_Settings.s_Instance
			End Get
		End Property

		' Token: 0x06004FFB RID: 20475 RVA: 0x002952F8 File Offset: 0x002936F8
		Public Shared Function LoadDefaultSettings() As TMP_Settings
			If TMP_Settings.s_Instance Is Nothing Then
				Dim tmp_Settings As TMP_Settings = TryCast(Resources.Load("TMP Settings"), TMP_Settings)
				If tmp_Settings IsNot Nothing Then
					TMP_Settings.s_Instance = tmp_Settings
				End If
			End If
			Return TMP_Settings.s_Instance
		End Function

		' Token: 0x06004FFC RID: 20476 RVA: 0x0029533C File Offset: 0x0029373C
		Public Shared Function GetSettings() As TMP_Settings
			If TMP_Settings.instance Is Nothing Then
				Return Nothing
			End If
			Return TMP_Settings.instance
		End Function

		' Token: 0x06004FFD RID: 20477 RVA: 0x00295355 File Offset: 0x00293755
		Public Shared Function GetFontAsset() As TMP_FontAsset
			If TMP_Settings.instance Is Nothing Then
				Return Nothing
			End If
			Return TMP_Settings.instance.m_defaultFontAsset
		End Function

		' Token: 0x06004FFE RID: 20478 RVA: 0x00295373 File Offset: 0x00293773
		Public Shared Function GetSpriteAsset() As TMP_SpriteAsset
			If TMP_Settings.instance Is Nothing Then
				Return Nothing
			End If
			Return TMP_Settings.instance.m_defaultSpriteAsset
		End Function

		' Token: 0x06004FFF RID: 20479 RVA: 0x00295391 File Offset: 0x00293791
		Public Shared Function GetStyleSheet() As TMP_StyleSheet
			If TMP_Settings.instance Is Nothing Then
				Return Nothing
			End If
			Return TMP_Settings.instance.m_defaultStyleSheet
		End Function

		' Token: 0x040052A1 RID: 21153
		Private Shared s_Instance As TMP_Settings

		' Token: 0x040052A2 RID: 21154
		<SerializeField()>
		Private m_enableWordWrapping As Boolean

		' Token: 0x040052A3 RID: 21155
		<SerializeField()>
		Private m_enableKerning As Boolean

		' Token: 0x040052A4 RID: 21156
		<SerializeField()>
		Private m_enableExtraPadding As Boolean

		' Token: 0x040052A5 RID: 21157
		<SerializeField()>
		Private m_enableTintAllSprites As Boolean

		' Token: 0x040052A6 RID: 21158
		<SerializeField()>
		Private m_warningsDisabled As Boolean

		' Token: 0x040052A7 RID: 21159
		<SerializeField()>
		Private m_defaultFontAsset As TMP_FontAsset

		' Token: 0x040052A8 RID: 21160
		<SerializeField()>
		Private m_fallbackFontAssets As List(Of TMP_FontAsset)

		' Token: 0x040052A9 RID: 21161
		<SerializeField()>
		Private m_defaultSpriteAsset As TMP_SpriteAsset

		' Token: 0x040052AA RID: 21162
		<SerializeField()>
		Private m_defaultStyleSheet As TMP_StyleSheet
	End Class
End Namespace
