Imports System
Imports System.Collections.Generic
Imports UnityEngine

Namespace TMPro
	' Token: 0x02000C7B RID: 3195
	<Serializable()>
	Public Class TMP_StyleSheet
		Inherits ScriptableObject

		' Token: 0x1700084B RID: 2123
		' (get) Token: 0x06005011 RID: 20497 RVA: 0x002955AC File Offset: 0x002939AC
		Public Shared ReadOnly Property instance As TMP_StyleSheet
			Get
				If TMP_StyleSheet.s_Instance Is Nothing Then
					TMP_StyleSheet.s_Instance = TMP_Settings.defaultStyleSheet
					If TMP_StyleSheet.s_Instance Is Nothing Then
						TMP_StyleSheet.s_Instance = TryCast(Resources.Load("Style Sheets/TMP Default Style Sheet"), TMP_StyleSheet)
					End If
					If TMP_StyleSheet.s_Instance Is Nothing Then
						Return Nothing
					End If
					TMP_StyleSheet.s_Instance.LoadStyleDictionaryInternal()
				End If
				Return TMP_StyleSheet.s_Instance
			End Get
		End Property

		' Token: 0x06005012 RID: 20498 RVA: 0x00295618 File Offset: 0x00293A18
		Public Shared Function LoadDefaultStyleSheet() As TMP_StyleSheet
			Return TMP_StyleSheet.instance
		End Function

		' Token: 0x06005013 RID: 20499 RVA: 0x0029561F File Offset: 0x00293A1F
		Public Shared Function GetStyle(hashCode As Integer) As TMP_Style
			Return TMP_StyleSheet.instance.GetStyleInternal(hashCode)
		End Function

		' Token: 0x06005014 RID: 20500 RVA: 0x0029562C File Offset: 0x00293A2C
		Private Function GetStyleInternal(hashCode As Integer) As TMP_Style
			Dim tmp_Style As TMP_Style
			If Me.m_StyleDictionary.TryGetValue(hashCode, tmp_Style) Then
				Return tmp_Style
			End If
			Return Nothing
		End Function

		' Token: 0x06005015 RID: 20501 RVA: 0x00295650 File Offset: 0x00293A50
		Public Sub UpdateStyleDictionaryKey(old_key As Integer, new_key As Integer)
			If Me.m_StyleDictionary.ContainsKey(old_key) Then
				Dim tmp_Style As TMP_Style = Me.m_StyleDictionary(old_key)
				Me.m_StyleDictionary.Add(new_key, tmp_Style)
				Me.m_StyleDictionary.Remove(old_key)
			End If
		End Sub

		' Token: 0x06005016 RID: 20502 RVA: 0x00295695 File Offset: 0x00293A95
		Public Shared Sub RefreshStyles()
			TMP_StyleSheet.s_Instance.LoadStyleDictionaryInternal()
		End Sub

		' Token: 0x06005017 RID: 20503 RVA: 0x002956A4 File Offset: 0x00293AA4
		Private Sub LoadStyleDictionaryInternal()
			Me.m_StyleDictionary.Clear()
			For i As Integer = 0 To Me.m_StyleList.Count - 1
				Me.m_StyleList(i).RefreshStyle()
				If Not Me.m_StyleDictionary.ContainsKey(Me.m_StyleList(i).hashCode) Then
					Me.m_StyleDictionary.Add(Me.m_StyleList(i).hashCode, Me.m_StyleList(i))
				End If
			Next
		End Sub

		' Token: 0x040052BA RID: 21178
		Private Shared s_Instance As TMP_StyleSheet

		' Token: 0x040052BB RID: 21179
		<SerializeField()>
		Private m_StyleList As List(Of TMP_Style) = New List(Of TMP_Style)(1)

		' Token: 0x040052BC RID: 21180
		Private m_StyleDictionary As Dictionary(Of Integer, TMP_Style) = New Dictionary(Of Integer, TMP_Style)()
	End Class
End Namespace
