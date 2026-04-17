Imports System
Imports System.Collections.Generic

' Token: 0x02000363 RID: 867
Public Class EventManager
	' Token: 0x170001F2 RID: 498
	' (get) Token: 0x0600099D RID: 2461 RVA: 0x0007C19F File Offset: 0x0007A59F
	Public Shared ReadOnly Property Instance As EventManager
		Get
			If EventManager._instance Is Nothing Then
				EventManager._instance = New EventManager()
			End If
			Return EventManager._instance
		End Get
	End Property

	' Token: 0x0600099E RID: 2462 RVA: 0x0007C1BC File Offset: 0x0007A5BC
	Public Sub AddListener(Of T As GameEvent)(del As EventManager.EventDelegate(Of T))
		If Me.delegateLookup.ContainsKey(del) Then
			Return
		End If
		Dim eventDelegate As EventManager.EventDelegate = Sub(e As GameEvent)
			del(CType(CObj(e), T))
		End Sub
		Me.delegateLookup(del) = eventDelegate
		Dim eventDelegate2 As EventManager.EventDelegate
		If Me.delegates.TryGetValue(GetType(T), eventDelegate2) Then
			Dim dictionary As Dictionary(Of Type, EventManager.EventDelegate) = Me.delegates
			Dim typeFromHandle As Type = GetType(T)
			Dim eventDelegate3 As EventManager.EventDelegate = CType([Delegate].Combine(eventDelegate2, eventDelegate), EventManager.EventDelegate)
			eventDelegate2 = eventDelegate3
			dictionary(typeFromHandle) = eventDelegate3
		Else
			Me.delegates(GetType(T)) = eventDelegate
		End If
	End Sub

	' Token: 0x0600099F RID: 2463 RVA: 0x0007C268 File Offset: 0x0007A668
	Public Sub RemoveListener(Of T As GameEvent)(del As EventManager.EventDelegate(Of T))
		Dim eventDelegate As EventManager.EventDelegate
		If Me.delegateLookup.TryGetValue(del, eventDelegate) Then
			Dim eventDelegate2 As EventManager.EventDelegate
			If Me.delegates.TryGetValue(GetType(T), eventDelegate2) Then
				eventDelegate2 = CType([Delegate].Remove(eventDelegate2, eventDelegate), EventManager.EventDelegate)
				If eventDelegate2 Is Nothing Then
					Me.delegates.Remove(GetType(T))
				Else
					Me.delegates(GetType(T)) = eventDelegate2
				End If
			End If
			Me.delegateLookup.Remove(del)
		End If
	End Sub

	' Token: 0x060009A0 RID: 2464 RVA: 0x0007C2F8 File Offset: 0x0007A6F8
	Public Sub Raise(e As GameEvent)
		Dim eventDelegate As EventManager.EventDelegate
		If Me.delegates.TryGetValue(e.[GetType](), eventDelegate) Then
			eventDelegate(e)
		End If
	End Sub

	' Token: 0x04001444 RID: 5188
	Private Shared _instance As EventManager

	' Token: 0x04001445 RID: 5189
	Private delegates As Dictionary(Of Type, EventManager.EventDelegate) = New Dictionary(Of Type, EventManager.EventDelegate)()

	' Token: 0x04001446 RID: 5190
	Private delegateLookup As Dictionary(Of [Delegate], EventManager.EventDelegate) = New Dictionary(Of [Delegate], EventManager.EventDelegate)()

	' Token: 0x02000364 RID: 868
	' (Invoke) Token: 0x060009A3 RID: 2467
	Public Delegate Sub EventDelegate(Of T As GameEvent)(e As T)

	' Token: 0x02000365 RID: 869
	' (Invoke) Token: 0x060009A7 RID: 2471
	Private Delegate Sub EventDelegate(e As GameEvent)
End Class
