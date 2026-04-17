Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000954 RID: 2388
Public Class MapNPCLostBarbershop
	Inherits AbstractMapInteractiveEntity

	' Token: 0x060037C6 RID: 14278 RVA: 0x00200066 File Offset: 0x001FE466
	Private Sub Start()
		Me.AddDialoguerEvents()
	End Sub

	' Token: 0x060037C7 RID: 14279 RVA: 0x0020006E File Offset: 0x001FE46E
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me.RemoveDialoguerEvents()
	End Sub

	' Token: 0x060037C8 RID: 14280 RVA: 0x0020007C File Offset: 0x001FE47C
	Public Sub AddDialoguerEvents()
		AddHandler Dialoguer.events.onMessageEvent, AddressOf Me.OnDialoguerMessageEvent
		AddHandler Dialoguer.events.onEnded, AddressOf Me.OnDialogueEndedHandler
		AddHandler Dialoguer.events.onInstantlyEnded, AddressOf Me.OnDialogueEndedHandler
	End Sub

	' Token: 0x060037C9 RID: 14281 RVA: 0x002000CC File Offset: 0x001FE4CC
	Public Sub RemoveDialoguerEvents()
		RemoveHandler Dialoguer.events.onMessageEvent, AddressOf Me.OnDialoguerMessageEvent
		RemoveHandler Dialoguer.events.onEnded, AddressOf Me.OnDialogueEndedHandler
		RemoveHandler Dialoguer.events.onInstantlyEnded, AddressOf Me.OnDialogueEndedHandler
	End Sub

	' Token: 0x060037CA RID: 14282 RVA: 0x0020011B File Offset: 0x001FE51B
	Private Sub OnDialoguerMessageEvent(message As String, metadata As String)
		If Me.SkipDialogueEvent Then
			Return
		End If
		If message = "LostBarberFound" Then
			MyBase.GetComponent(Of MapDialogueInteraction)().disabledActivations = True
			Me.reunited = True
		End If
	End Sub

	' Token: 0x060037CB RID: 14283 RVA: 0x0020014C File Offset: 0x001FE54C
	Protected Overrides Sub Activate()
	End Sub

	' Token: 0x060037CC RID: 14284 RVA: 0x0020014E File Offset: 0x001FE54E
	Protected Overrides Function Show(player As PlayerInput) As MapUIInteractionDialogue
		Me.FoundBarbershopSFX()
		MyBase.animator.SetTrigger(Me.triggerShow)
		Return Nothing
	End Function

	' Token: 0x060037CD RID: 14285 RVA: 0x00200168 File Offset: 0x001FE568
	Private Sub OnDialogueEndedHandler()
		If Me.SkipDialogueEvent Then
			Return
		End If
		If Me.reunited Then
			Dialoguer.SetGlobalFloat(Me.dialoguerVariableID, 1F)
			PlayerData.SaveCurrentFile()
			MyBase.StartCoroutine(Me.found_cr())
		End If
	End Sub

	' Token: 0x060037CE RID: 14286 RVA: 0x002001A4 File Offset: 0x001FE5A4
	Private Iterator Function found_cr() As IEnumerator
		MyBase.animator.SetTrigger(Me.triggerHide)
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "anim_map_barbershop_outtro_d", False, True)
		Me.playerCanWalkBehind = True
		MyBase.SetLayer(MyBase.GetComponent(Of SpriteRenderer)())
		For i As Integer = 0 To Me.mapNPCBarbershops.Length - 1
			Me.mapNPCBarbershops(i).NowFour()
			If Not(Me.mapNPCBarbershops(i).mapDialogueInteraction Is Nothing) Then
				For j As Integer = 0 To Me.mapNPCBarbershops(i).mapDialogueInteraction.dialogues.Length - 1
					If Not(Me.mapNPCBarbershops(i).mapDialogueInteraction.dialogues(j) Is Nothing) Then
						Me.mapNPCBarbershops(i).mapDialogueInteraction.Hide(Me.mapNPCBarbershops(i).mapDialogueInteraction.dialogues(j))
					End If
				Next
			End If
		Next
		Me.Hide(Nothing)
		While CupheadMapCamera.Current IsNot Nothing AndAlso CupheadMapCamera.Current.IsCameraFarFromPlayer()
			Yield Nothing
		End While
		For k As Integer = 0 To Me.mapNPCBarbershops.Length - 1
			Me.mapNPCBarbershops(k).CleanUp()
		Next
		Return
	End Function

	' Token: 0x060037CF RID: 14287 RVA: 0x002001BF File Offset: 0x001FE5BF
	Private Sub FoundBarbershopSFX()
		If Not Me.FirstTimeFoundSFX Then
			AudioManager.Play("find_barbershop_member")
			Me.FirstTimeFoundSFX = True
		End If
	End Sub

	' Token: 0x04003FC4 RID: 16324
	<SerializeField()>
	Private triggerShow As String

	' Token: 0x04003FC5 RID: 16325
	<SerializeField()>
	Private triggerHide As String

	' Token: 0x04003FC6 RID: 16326
	<SerializeField()>
	Private mapNPCBarbershops As MapNPCBarbershop()

	' Token: 0x04003FC7 RID: 16327
	<SerializeField()>
	Private dialoguerVariableID As Integer = 10

	' Token: 0x04003FC8 RID: 16328
	Private reunited As Boolean

	' Token: 0x04003FC9 RID: 16329
	Private FirstTimeFoundSFX As Boolean

	' Token: 0x04003FCA RID: 16330
	Private spriteRenderer As SpriteRenderer

	' Token: 0x04003FCB RID: 16331
	<HideInInspector()>
	Public SkipDialogueEvent As Boolean
End Class
