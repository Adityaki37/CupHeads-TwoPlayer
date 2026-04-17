Imports System
Imports System.Collections.Generic
Imports UnityEngine.Events

Namespace TMPro
	' Token: 0x02000C76 RID: 3190
	Friend Class TMP_ObjectPool(Of T As New)
		' Token: 0x06004FE9 RID: 20457 RVA: 0x0029513F File Offset: 0x0029353F
		Public Sub New(actionOnGet As UnityAction(Of T), actionOnRelease As UnityAction(Of T))
			Me.m_ActionOnGet = actionOnGet
			Me.m_ActionOnRelease = actionOnRelease
		End Sub

		' Token: 0x17000837 RID: 2103
		' (get) Token: 0x06004FEA RID: 20458 RVA: 0x00295160 File Offset: 0x00293560
		' (set) Token: 0x06004FEB RID: 20459 RVA: 0x00295168 File Offset: 0x00293568
		Public Property countAll As Integer

		' Token: 0x17000838 RID: 2104
		' (get) Token: 0x06004FEC RID: 20460 RVA: 0x00295171 File Offset: 0x00293571
		Public ReadOnly Property countActive As Integer
			Get
				Return Me.countAll - Me.countInactive
			End Get
		End Property

		' Token: 0x17000839 RID: 2105
		' (get) Token: 0x06004FED RID: 20461 RVA: 0x00295180 File Offset: 0x00293580
		Public ReadOnly Property countInactive As Integer
			Get
				Return Me.m_Stack.Count
			End Get
		End Property

		' Token: 0x06004FEE RID: 20462 RVA: 0x00295190 File Offset: 0x00293590
		Public Function [Get]() As T
			Dim t As T
			If Me.m_Stack.Count = 0 Then
				t = New T()
				Me.countAll += 1
			Else
				t = Me.m_Stack.Pop()
			End If
			If Me.m_ActionOnGet IsNot Nothing Then
				Me.m_ActionOnGet(t)
			End If
			Return t
		End Function

		' Token: 0x06004FEF RID: 20463 RVA: 0x002951EC File Offset: 0x002935EC
		Public Sub Release(element As T)
			If Me.m_Stack.Count > 0 AndAlso Object.ReferenceEquals(Me.m_Stack.Peek(), element) Then
				Debug.LogError("Internal error. Trying to destroy object that is already released to pool.", Nothing)
			End If
			If Me.m_ActionOnRelease IsNot Nothing Then
				Me.m_ActionOnRelease(element)
			End If
			Me.m_Stack.Push(element)
		End Sub

		' Token: 0x0400529D RID: 21149
		Private m_Stack As Stack(Of T) = New Stack(Of T)()

		' Token: 0x0400529E RID: 21150
		Private m_ActionOnGet As UnityAction(Of T)

		' Token: 0x0400529F RID: 21151
		Private m_ActionOnRelease As UnityAction(Of T)
	End Class
End Namespace
