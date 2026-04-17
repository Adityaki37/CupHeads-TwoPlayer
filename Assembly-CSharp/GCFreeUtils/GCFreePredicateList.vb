Imports System

Namespace GCFreeUtils
	' Token: 0x02000B27 RID: 2855
	Public Class GCFreePredicateList(Of T)
		' Token: 0x0600452F RID: 17711 RVA: 0x00247838 File Offset: 0x00245C38
		Public Sub New(size As Integer)
			Me.New(size, True)
		End Sub

		' Token: 0x06004530 RID: 17712 RVA: 0x00247842 File Offset: 0x00245C42
		Public Sub New(size As Integer, autoResizeable As Boolean)
			Me.actionList = New Predicate(Of T)(size - 1) {}
			Me.autoResizeable = autoResizeable
			Me.Count = 0
		End Sub

		' Token: 0x1700062C RID: 1580
		' (get) Token: 0x06004531 RID: 17713 RVA: 0x00247864 File Offset: 0x00245C64
		' (set) Token: 0x06004532 RID: 17714 RVA: 0x0024786C File Offset: 0x00245C6C
		Public Property Count As Integer

		' Token: 0x06004533 RID: 17715 RVA: 0x00247878 File Offset: 0x00245C78
		Public Sub Add(action As Predicate(Of T))
			If Me.Count = Me.actionList.Length Then
				If Not Me.autoResizeable Then
					Debug.LogError("[GCFreeActionList] Current buffer too small. Consider increasing the initial size or set as auto resizeable.", Nothing)
					Return
				End If
				Dim array As Predicate(Of T)() = New Predicate(Of T)(Me.actionList.Length * 2 - 1) {}
				Array.Copy(Me.actionList, array, Me.actionList.Length)
				Me.actionList = array
			End If
			Me.actionList(Me.Count) = action
			Me.Count += 1
		End Sub

		' Token: 0x06004534 RID: 17716 RVA: 0x002478FC File Offset: 0x00245CFC
		Public Sub Remove(action As Predicate(Of T))
			If Me.Count > 0 Then
				For i As Integer = 0 To Me.Count - 1
					If Me.actionList(i) Is action Then
						If Me.Count > 1 Then
							Me.actionList(i) = Me.actionList(Me.Count - 1)
						Else
							Me.actionList(i) = Nothing
						End If
						Me.Count -= 1
						Exit For
					End If
				Next
			End If
		End Sub

		' Token: 0x06004535 RID: 17717 RVA: 0x00247984 File Offset: 0x00245D84
		Public Function CallAnyTrue(parameter As T) As Boolean
			For i As Integer = 0 To Me.Count - 1
				Try
					If Me.actionList(i) IsNot Nothing Then
						Dim flag As Boolean = Me.actionList(i)(parameter)
						If flag Then
							Return True
						End If
					End If
				Catch ex As Exception
					Debug.LogError(ex, Nothing)
				End Try
			Next
			Return False
		End Function

		' Token: 0x04004AE3 RID: 19171
		Private Const ERR_BUFFER_TOO_SMALL As String = "[GCFreeActionList] Current buffer too small. Consider increasing the initial size or set as auto resizeable."

		' Token: 0x04004AE4 RID: 19172
		Private Const LOG_RESIZING As String = "[GCFreeActionList] Resizing buffer. Maybe you want to increase the initial size."

		' Token: 0x04004AE6 RID: 19174
		Private actionList As Predicate(Of T)()

		' Token: 0x04004AE7 RID: 19175
		Private autoResizeable As Boolean
	End Class
End Namespace
