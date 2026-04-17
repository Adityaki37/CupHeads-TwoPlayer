Imports System
Imports System.Diagnostics

Namespace DialoguerCore
	' Token: 0x02000B62 RID: 2914
	Public Class DialoguerEventManager
		' Token: 0x140000DB RID: 219
		' (add) Token: 0x0600464D RID: 17997 RVA: 0x0024E0B0 File Offset: 0x0024C4B0
		' (remove) Token: 0x0600464E RID: 17998 RVA: 0x0024E0E4 File Offset: 0x0024C4E4
		<DebuggerBrowsable(DebuggerBrowsableState.Never)>
		Public Shared Event onStarted As DialoguerEventManager.StartedHandler

		' Token: 0x0600464F RID: 17999 RVA: 0x0024E118 File Offset: 0x0024C518
		Public Shared Sub dispatchOnStarted()
			If DialoguerEventManager.onStarted IsNot Nothing Then
				DialoguerEventManager.onStarted()
			End If
		End Sub

		' Token: 0x140000DC RID: 220
		' (add) Token: 0x06004650 RID: 18000 RVA: 0x0024E130 File Offset: 0x0024C530
		' (remove) Token: 0x06004651 RID: 18001 RVA: 0x0024E164 File Offset: 0x0024C564
		<DebuggerBrowsable(DebuggerBrowsableState.Never)>
		Public Shared Event onEnded As DialoguerEventManager.EndedHandler

		' Token: 0x06004652 RID: 18002 RVA: 0x0024E198 File Offset: 0x0024C598
		Public Shared Sub dispatchOnEnded()
			If DialoguerEventManager.onEnded IsNot Nothing Then
				DialoguerEventManager.onEnded()
			End If
		End Sub

		' Token: 0x140000DD RID: 221
		' (add) Token: 0x06004653 RID: 18003 RVA: 0x0024E1B0 File Offset: 0x0024C5B0
		' (remove) Token: 0x06004654 RID: 18004 RVA: 0x0024E1E4 File Offset: 0x0024C5E4
		<DebuggerBrowsable(DebuggerBrowsableState.Never)>
		Public Shared Event onSuddenlyEnded As DialoguerEventManager.SuddenlyEndedHandler

		' Token: 0x06004655 RID: 18005 RVA: 0x0024E218 File Offset: 0x0024C618
		Public Shared Sub dispatchOnSuddenlyEnded()
			If DialoguerEventManager.onSuddenlyEnded IsNot Nothing Then
				DialoguerEventManager.onSuddenlyEnded()
			End If
		End Sub

		' Token: 0x140000DE RID: 222
		' (add) Token: 0x06004656 RID: 18006 RVA: 0x0024E230 File Offset: 0x0024C630
		' (remove) Token: 0x06004657 RID: 18007 RVA: 0x0024E264 File Offset: 0x0024C664
		<DebuggerBrowsable(DebuggerBrowsableState.Never)>
		Public Shared Event onTextPhase As DialoguerEventManager.TextPhaseHandler

		' Token: 0x06004658 RID: 18008 RVA: 0x0024E298 File Offset: 0x0024C698
		Public Shared Sub dispatchOnTextPhase(data As DialoguerTextData)
			If DialoguerEventManager.onTextPhase IsNot Nothing Then
				DialoguerEventManager.onTextPhase(data)
			End If
		End Sub

		' Token: 0x140000DF RID: 223
		' (add) Token: 0x06004659 RID: 18009 RVA: 0x0024E2B0 File Offset: 0x0024C6B0
		' (remove) Token: 0x0600465A RID: 18010 RVA: 0x0024E2E4 File Offset: 0x0024C6E4
		<DebuggerBrowsable(DebuggerBrowsableState.Never)>
		Public Shared Event onWindowClose As DialoguerEventManager.WindowCloseHandler

		' Token: 0x0600465B RID: 18011 RVA: 0x0024E318 File Offset: 0x0024C718
		Public Shared Sub dispatchOnWindowClose()
			If DialoguerEventManager.onWindowClose IsNot Nothing Then
				DialoguerEventManager.onWindowClose()
			End If
		End Sub

		' Token: 0x140000E0 RID: 224
		' (add) Token: 0x0600465C RID: 18012 RVA: 0x0024E330 File Offset: 0x0024C730
		' (remove) Token: 0x0600465D RID: 18013 RVA: 0x0024E364 File Offset: 0x0024C764
		<DebuggerBrowsable(DebuggerBrowsableState.Never)>
		Public Shared Event onWaitStart As DialoguerEventManager.WaitStartHandler

		' Token: 0x0600465E RID: 18014 RVA: 0x0024E398 File Offset: 0x0024C798
		Public Shared Sub dispatchOnWaitStart()
			If DialoguerEventManager.onWaitStart IsNot Nothing Then
				DialoguerEventManager.onWaitStart()
			End If
		End Sub

		' Token: 0x140000E1 RID: 225
		' (add) Token: 0x0600465F RID: 18015 RVA: 0x0024E3B0 File Offset: 0x0024C7B0
		' (remove) Token: 0x06004660 RID: 18016 RVA: 0x0024E3E4 File Offset: 0x0024C7E4
		<DebuggerBrowsable(DebuggerBrowsableState.Never)>
		Public Shared Event onWaitComplete As DialoguerEventManager.WaitCompleteHandler

		' Token: 0x06004661 RID: 18017 RVA: 0x0024E418 File Offset: 0x0024C818
		Public Shared Sub dispatchOnWaitComplete()
			If DialoguerEventManager.onWaitComplete IsNot Nothing Then
				DialoguerEventManager.onWaitComplete()
			End If
		End Sub

		' Token: 0x140000E2 RID: 226
		' (add) Token: 0x06004662 RID: 18018 RVA: 0x0024E430 File Offset: 0x0024C830
		' (remove) Token: 0x06004663 RID: 18019 RVA: 0x0024E464 File Offset: 0x0024C864
		<DebuggerBrowsable(DebuggerBrowsableState.Never)>
		Public Shared Event onMessageEvent As DialoguerEventManager.MessageEventHandler

		' Token: 0x06004664 RID: 18020 RVA: 0x0024E498 File Offset: 0x0024C898
		Public Shared Sub dispatchOnMessageEvent(message As String, metadata As String)
			If DialoguerEventManager.onMessageEvent IsNot Nothing Then
				DialoguerEventManager.onMessageEvent(message, metadata)
			End If
		End Sub

		' Token: 0x02000B63 RID: 2915
		' (Invoke) Token: 0x06004666 RID: 18022
		Public Delegate Sub StartedHandler()

		' Token: 0x02000B64 RID: 2916
		' (Invoke) Token: 0x0600466A RID: 18026
		Public Delegate Sub EndedHandler()

		' Token: 0x02000B65 RID: 2917
		' (Invoke) Token: 0x0600466E RID: 18030
		Public Delegate Sub SuddenlyEndedHandler()

		' Token: 0x02000B66 RID: 2918
		' (Invoke) Token: 0x06004672 RID: 18034
		Public Delegate Sub TextPhaseHandler(data As DialoguerTextData)

		' Token: 0x02000B67 RID: 2919
		' (Invoke) Token: 0x06004676 RID: 18038
		Public Delegate Sub WindowCloseHandler()

		' Token: 0x02000B68 RID: 2920
		' (Invoke) Token: 0x0600467A RID: 18042
		Public Delegate Sub WaitStartHandler()

		' Token: 0x02000B69 RID: 2921
		' (Invoke) Token: 0x0600467E RID: 18046
		Public Delegate Sub WaitCompleteHandler()

		' Token: 0x02000B6A RID: 2922
		' (Invoke) Token: 0x06004682 RID: 18050
		Public Delegate Sub MessageEventHandler(message As String, metadata As String)
	End Class
End Namespace
