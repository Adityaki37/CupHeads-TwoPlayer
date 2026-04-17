Imports System

Namespace GCFreeUtils
	' Token: 0x02000B26 RID: 2854
	Public Class GCFreeActionList
		' Token: 0x06004528 RID: 17704 RVA: 0x0024768F File Offset: 0x00245A8F
		Public Sub New(size As Integer)
			Me.New(size, True)
		End Sub

		' Token: 0x06004529 RID: 17705 RVA: 0x00247699 File Offset: 0x00245A99
		Public Sub New(size As Integer, autoResizeable As Boolean)
			Me.actionList = New Action(size - 1) {}
			Me.autoResizeable = autoResizeable
			Me.Count = 0
		End Sub

		' Token: 0x1700062B RID: 1579
		' (get) Token: 0x0600452A RID: 17706 RVA: 0x002476BB File Offset: 0x00245ABB
		' (set) Token: 0x0600452B RID: 17707 RVA: 0x002476C3 File Offset: 0x00245AC3
		Public Property Count As Integer

		' Token: 0x0600452C RID: 17708 RVA: 0x002476CC File Offset: 0x00245ACC
		Public Sub Add(action As Action)
			If Me.Count = Me.actionList.Length Then
				If Not Me.autoResizeable Then
					Debug.LogError("[GCFreeActionList] Current buffer too small. Consider increasing the initial size or set as auto resizeable.", Nothing)
					Return
				End If
				Dim array As Action() = New Action(Me.actionList.Length * 2 - 1) {}
				Array.Copy(Me.actionList, array, Me.actionList.Length)
				Me.actionList = array
			End If
			Me.actionList(Me.Count) = action
			Me.Count += 1
		End Sub

		' Token: 0x0600452D RID: 17709 RVA: 0x00247750 File Offset: 0x00245B50
		Public Sub Remove(action As Action)
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

		' Token: 0x0600452E RID: 17710 RVA: 0x002477D8 File Offset: 0x00245BD8
		Public Sub [Call]()
			For i As Integer = 0 To Me.Count - 1
				Try
					If Me.actionList(i) IsNot Nothing Then
						Me.actionList(i)()
					End If
				Catch ex As Exception
					Debug.LogError(ex, Nothing)
				End Try
			Next
		End Sub

		' Token: 0x04004ADE RID: 19166
		Private Const ERR_BUFFER_TOO_SMALL As String = "[GCFreeActionList] Current buffer too small. Consider increasing the initial size or set as auto resizeable."

		' Token: 0x04004ADF RID: 19167
		Private Const LOG_RESIZING As String = "[GCFreeActionList] Resizing buffer. Maybe you want to increase the initial size."

		' Token: 0x04004AE1 RID: 19169
		Private actionList As Action()

		' Token: 0x04004AE2 RID: 19170
		Private autoResizeable As Boolean
	End Class
End Namespace
