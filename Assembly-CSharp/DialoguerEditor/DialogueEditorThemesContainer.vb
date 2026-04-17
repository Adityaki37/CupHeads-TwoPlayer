Imports System
Imports System.Collections.Generic

Namespace DialoguerEditor
	' Token: 0x02000B4C RID: 2892
	<Serializable()>
	Public Class DialogueEditorThemesContainer
		' Token: 0x060045E6 RID: 17894 RVA: 0x0024D728 File Offset: 0x0024BB28
		Public Sub New()
			Me.themes = New List(Of DialogueEditorThemeObject)()
		End Sub

		' Token: 0x060045E7 RID: 17895 RVA: 0x0024D73C File Offset: 0x0024BB3C
		Public Sub addTheme()
			Dim count As Integer = Me.themes.Count
			Me.themes.Add(New DialogueEditorThemeObject())
			Me.themes(count).id = count
		End Sub

		' Token: 0x060045E8 RID: 17896 RVA: 0x0024D778 File Offset: 0x0024BB78
		Public Sub removeTheme()
			Me.themes.RemoveAt(Me.themes.Count - 1)
			If Me.selection >= Me.themes.Count Then
				Me.selection = Me.themes.Count - 1
			End If
		End Sub

		' Token: 0x04004C27 RID: 19495
		Public themes As List(Of DialogueEditorThemeObject)

		' Token: 0x04004C28 RID: 19496
		Public selection As Integer
	End Class
End Namespace
