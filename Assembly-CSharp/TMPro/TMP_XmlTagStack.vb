Imports System

Namespace TMPro
	' Token: 0x02000C94 RID: 3220
	Public Structure TMP_XmlTagStack(Of T)
		' Token: 0x06005159 RID: 20825 RVA: 0x00298ED3 File Offset: 0x002972D3
		Public Sub New(tagStack As T())
			Me.itemStack = tagStack
			Me.index = 0
		End Sub

		' Token: 0x0600515A RID: 20826 RVA: 0x00298EE3 File Offset: 0x002972E3
		Public Sub Clear()
			Me.index = 0
		End Sub

		' Token: 0x0600515B RID: 20827 RVA: 0x00298EEC File Offset: 0x002972EC
		Public Sub SetDefault(item As T)
			Me.itemStack(0) = item
			Me.index = 1
		End Sub

		' Token: 0x0600515C RID: 20828 RVA: 0x00298F02 File Offset: 0x00297302
		Public Sub Add(item As T)
			If Me.index < Me.itemStack.Length Then
				Me.itemStack(Me.index) = item
				Me.index += 1
			End If
		End Sub

		' Token: 0x0600515D RID: 20829 RVA: 0x00298F38 File Offset: 0x00297338
		Public Function Remove() As T
			Me.index -= 1
			If Me.index <= 0 Then
				Me.index = 0
				Return Me.itemStack(0)
			End If
			Return Me.itemStack(Me.index - 1)
		End Function

		' Token: 0x0600515E RID: 20830 RVA: 0x00298F86 File Offset: 0x00297386
		Public Function CurrentItem() As T
			Return Me.itemStack(Me.index - 1)
		End Function

		' Token: 0x040053F8 RID: 21496
		Public itemStack As T()

		' Token: 0x040053F9 RID: 21497
		Public index As Integer
	End Structure
End Namespace
