Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports System.Text.RegularExpressions

Namespace UnityEngine.UI
	' Token: 0x02000CB2 RID: 3250
	<AddComponentMenu("UI/Effects/Letter Spacing", 15)>
	Public Class LetterSpacing
		Inherits BaseMeshEffect

		' Token: 0x060051A2 RID: 20898 RVA: 0x0029B0CD File Offset: 0x002994CD
		Protected Sub New()
		End Sub

		' Token: 0x170008A2 RID: 2210
		' (get) Token: 0x060051A3 RID: 20899 RVA: 0x0029B0D5 File Offset: 0x002994D5
		' (set) Token: 0x060051A4 RID: 20900 RVA: 0x0029B0DD File Offset: 0x002994DD
		Public Property spacing As Single
			Get
				Return Me.m_spacing
			End Get
			Set(value As Single)
				If Me.m_spacing = value Then
					Return
				End If
				Me.m_spacing = value
				If MyBase.graphic IsNot Nothing Then
					MyBase.graphic.SetVerticesDirty()
				End If
			End Set
		End Property

		' Token: 0x060051A5 RID: 20901 RVA: 0x0029B110 File Offset: 0x00299510
		Public Overrides Sub ModifyMesh(vh As VertexHelper)
			If Not Me.IsActive() Then
				Return
			End If
			Dim list As List(Of UIVertex) = New List(Of UIVertex)()
			vh.GetUIVertexStream(list)
			Me.ModifyVertices(list)
			vh.Clear()
			vh.AddUIVertexTriangleStream(list)
		End Sub

		' Token: 0x060051A6 RID: 20902 RVA: 0x0029B14C File Offset: 0x0029954C
		Public Sub ModifyVertices(verts As List(Of UIVertex))
			If Not Me.IsActive() Then
				Return
			End If
			Dim component As Text = MyBase.GetComponent(Of Text)()
			Dim text As String = component.text
			Dim lines As IList(Of UILineInfo) = component.cachedTextGenerator.lines
			For i As Integer = lines.Count - 1 To 0 + 1 Step -1
				text = text.Insert(lines(i).startCharIdx, vbLf)
				text = text.Remove(lines(i).startCharIdx - 1, 1)
			Next
			Dim array As String() = text.Split(New Char() { vbLf })
			If component Is Nothing Then
				Debug.LogWarning("LetterSpacing: Missing Text component")
				Return
			End If
			Dim num As Single = Me.spacing * CSng(component.fontSize) / 100F
			Dim num2 As Single = 0F
			Dim num3 As Integer = 0
			Dim flag As Boolean = Me.useRichText AndAlso component.supportRichText
			Dim enumerator As IEnumerator = Nothing
			Dim match As Match = Nothing
			Select Case component.alignment
				Case TextAnchor.UpperLeft, TextAnchor.MiddleLeft, TextAnchor.LowerLeft
					num2 = 0F
				Case TextAnchor.UpperCenter, TextAnchor.MiddleCenter, TextAnchor.LowerCenter
					num2 = 0.5F
				Case TextAnchor.UpperRight, TextAnchor.MiddleRight, TextAnchor.LowerRight
					num2 = 1F
			End Select
			For Each text2 As String In array
				Dim length As Integer = text2.Length
				If flag Then
					enumerator = Me.GetRegexMatchedTagCollection(text2, length)
					match = Nothing
					If enumerator.MoveNext() Then
						match = CType(enumerator.Current, Match)
					End If
				End If
				Dim num4 As Single = CSng((length - 1)) * num * num2
				Dim k As Integer = 0
				Dim num5 As Integer = 0
				While k < text2.Length
					If flag AndAlso match IsNot Nothing AndAlso match.Index = k Then
						k += match.Length - 1
						num5 -= 1
						num3 += match.Length
						match = Nothing
						If enumerator.MoveNext() Then
							match = CType(enumerator.Current, Match)
						End If
					Else
						Dim num6 As Integer = num3 * 6
						Dim num7 As Integer = num3 * 6 + 1
						Dim num8 As Integer = num3 * 6 + 2
						Dim num9 As Integer = num3 * 6 + 3
						Dim num10 As Integer = num3 * 6 + 4
						Dim num11 As Integer = num3 * 6 + 5
						If num11 > verts.Count - 1 Then
							Return
						End If
						Dim uivertex As UIVertex = verts(num6)
						Dim uivertex2 As UIVertex = verts(num7)
						Dim uivertex3 As UIVertex = verts(num8)
						Dim uivertex4 As UIVertex = verts(num9)
						Dim uivertex5 As UIVertex = verts(num10)
						Dim uivertex6 As UIVertex = verts(num11)
						Dim vector As Vector3 = Vector3.right * (num * CSng(num5) - num4)
						uivertex.position += vector
						uivertex2.position += vector
						uivertex3.position += vector
						uivertex4.position += vector
						uivertex5.position += vector
						uivertex6.position += vector
						verts(num6) = uivertex
						verts(num7) = uivertex2
						verts(num8) = uivertex3
						verts(num9) = uivertex4
						verts(num10) = uivertex5
						verts(num11) = uivertex6
						num3 += 1
					End If
					k += 1
					num5 += 1
				End While
				num3 += 1
			Next
		End Sub

		' Token: 0x060051A7 RID: 20903 RVA: 0x0029B4C8 File Offset: 0x002998C8
		Private Function GetRegexMatchedTagCollection(line As String, <System.Runtime.InteropServices.OutAttribute()> ByRef lineLengthWithoutTags As Integer) As IEnumerator
			Dim matchCollection As MatchCollection = Regex.Matches(line, "<b>|</b>|<i>|</i>|<size=.*?>|</size>|<color=.*?>|</color>|<material=.*?>|</material>")
			lineLengthWithoutTags = 0
			Dim num As Integer = 0
			If matchCollection.Count > 0 Then
				Dim enumerator As IEnumerator = matchCollection.GetEnumerator()
				Try
					While enumerator.MoveNext()
						Dim obj As Object = enumerator.Current
						Dim match As Match = CType(obj, Match)
						num += match.Length
					End While
				Finally
					Dim disposable As IDisposable = TryCast(enumerator, IDisposable)
					Dim disposable2 As IDisposable = disposable
					If disposable IsNot Nothing Then
						disposable2.Dispose()
					End If
				End Try
			End If
			lineLengthWithoutTags = line.Length - num
			Return matchCollection.GetEnumerator()
		End Function

		' Token: 0x04005510 RID: 21776
		Private Const SupportedTagRegexPattersn As String = "<b>|</b>|<i>|</i>|<size=.*?>|</size>|<color=.*?>|</color>|<material=.*?>|</material>"

		' Token: 0x04005511 RID: 21777
		<SerializeField()>
		Private useRichText As Boolean

		' Token: 0x04005512 RID: 21778
		<SerializeField()>
		Private m_spacing As Single
	End Class
End Namespace
