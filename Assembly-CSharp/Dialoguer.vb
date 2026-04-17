Imports System
Imports DialoguerCore

' Token: 0x02000B57 RID: 2903
Public Class Dialoguer
	' Token: 0x1700063D RID: 1597
	' (get) Token: 0x060045F5 RID: 17909 RVA: 0x0024D933 File Offset: 0x0024BD33
	' (set) Token: 0x060045F6 RID: 17910 RVA: 0x0024D93A File Offset: 0x0024BD3A
	Public Shared Property ready As Boolean

	' Token: 0x060045F7 RID: 17911 RVA: 0x0024D944 File Offset: 0x0024BD44
	Public Shared Sub Initialize()
		If Dialoguer.ready Then
			Return
		End If
		Dialoguer.events = New DialoguerEvents()
		DialoguerDataManager.Initialize()
		AddHandler DialoguerEventManager.onStarted, AddressOf Dialoguer.events.handler_onStarted
		AddHandler DialoguerEventManager.onEnded, AddressOf Dialoguer.events.handler_onEnded
		AddHandler DialoguerEventManager.onSuddenlyEnded, AddressOf Dialoguer.events.handler_SuddenlyEnded
		AddHandler DialoguerEventManager.onTextPhase, AddressOf Dialoguer.events.handler_TextPhase
		AddHandler DialoguerEventManager.onWindowClose, AddressOf Dialoguer.events.handler_WindowClose
		AddHandler DialoguerEventManager.onWaitStart, AddressOf Dialoguer.events.handler_WaitStart
		AddHandler DialoguerEventManager.onWaitComplete, AddressOf Dialoguer.events.handler_WaitComplete
		AddHandler DialoguerEventManager.onMessageEvent, AddressOf Dialoguer.events.handler_MessageEvent
		Dialoguer.ready = True
	End Sub

	' Token: 0x060045F8 RID: 17912 RVA: 0x0024DA19 File Offset: 0x0024BE19
	Public Shared Sub StartDialogue(dialogue As DialoguerDialogues)
		DialoguerDialogueManager.startDialogue(CInt(dialogue))
	End Sub

	' Token: 0x060045F9 RID: 17913 RVA: 0x0024DA21 File Offset: 0x0024BE21
	Public Shared Sub StartDialogue(dialogue As DialoguerDialogues, callback As DialoguerCallback)
		DialoguerDialogueManager.startDialogueWithCallback(CInt(dialogue), callback)
	End Sub

	' Token: 0x060045FA RID: 17914 RVA: 0x0024DA2A File Offset: 0x0024BE2A
	Public Shared Sub StartDialogue(dialogueId As Integer)
		DialoguerDialogueManager.startDialogue(dialogueId)
	End Sub

	' Token: 0x060045FB RID: 17915 RVA: 0x0024DA32 File Offset: 0x0024BE32
	Public Shared Sub StartDialogue(dialogueId As Integer, callback As DialoguerCallback)
		DialoguerDialogueManager.startDialogueWithCallback(dialogueId, callback)
	End Sub

	' Token: 0x060045FC RID: 17916 RVA: 0x0024DA3B File Offset: 0x0024BE3B
	Public Shared Sub ContinueDialogue(choice As Integer)
		DialoguerDialogueManager.continueDialogue(choice)
	End Sub

	' Token: 0x060045FD RID: 17917 RVA: 0x0024DA43 File Offset: 0x0024BE43
	Public Shared Sub ContinueDialogue()
		DialoguerDialogueManager.continueDialogue(0)
	End Sub

	' Token: 0x060045FE RID: 17918 RVA: 0x0024DA4B File Offset: 0x0024BE4B
	Public Shared Sub EndDialogue()
		DialoguerDialogueManager.endDialogue()
	End Sub

	' Token: 0x060045FF RID: 17919 RVA: 0x0024DA52 File Offset: 0x0024BE52
	Public Shared Sub SetGlobalBoolean(booleanId As Integer, booleanValue As Boolean)
		DialoguerDataManager.SetGlobalBoolean(booleanId, booleanValue)
	End Sub

	' Token: 0x06004600 RID: 17920 RVA: 0x0024DA5B File Offset: 0x0024BE5B
	Public Shared Function GetGlobalBoolean(booleanId As Integer) As Boolean
		Return DialoguerDataManager.GetGlobalBoolean(booleanId)
	End Function

	' Token: 0x06004601 RID: 17921 RVA: 0x0024DA63 File Offset: 0x0024BE63
	Public Shared Sub SetGlobalFloat(floatId As Integer, floatValue As Single)
		DialoguerDataManager.SetGlobalFloat(floatId, floatValue)
	End Sub

	' Token: 0x06004602 RID: 17922 RVA: 0x0024DA6C File Offset: 0x0024BE6C
	Public Shared Function GetGlobalFloat(floatId As Integer) As Single
		Return DialoguerDataManager.GetGlobalFloat(floatId)
	End Function

	' Token: 0x06004603 RID: 17923 RVA: 0x0024DA74 File Offset: 0x0024BE74
	Public Shared Sub SetGlobalString(stringId As Integer, stringValue As String)
		DialoguerDataManager.SetGlobalString(stringId, stringValue)
	End Sub

	' Token: 0x06004604 RID: 17924 RVA: 0x0024DA7D File Offset: 0x0024BE7D
	Public Shared Function GetGlobalString(stringId As Integer) As String
		Return DialoguerDataManager.GetGlobalString(stringId)
	End Function

	' Token: 0x06004605 RID: 17925 RVA: 0x0024DA85 File Offset: 0x0024BE85
	Public Shared Function GetGlobalVariablesState() As String
		Return DialoguerDataManager.GetGlobalVariablesState()
	End Function

	' Token: 0x06004606 RID: 17926 RVA: 0x0024DA8C File Offset: 0x0024BE8C
	Public Shared Sub SetGlobalVariablesState(globalVariablesXml As String)
		DialoguerDataManager.LoadGlobalVariablesState(globalVariablesXml)
	End Sub

	' Token: 0x1700063E RID: 1598
	' (get) Token: 0x06004607 RID: 17927 RVA: 0x0024DA94 File Offset: 0x0024BE94
	' (set) Token: 0x06004608 RID: 17928 RVA: 0x0024DA9B File Offset: 0x0024BE9B
	Public Shared Property events As DialoguerEvents
End Class
