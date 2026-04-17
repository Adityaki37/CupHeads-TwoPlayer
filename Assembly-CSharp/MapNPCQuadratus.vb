Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x0200095A RID: 2394
Public Class MapNPCQuadratus
	Inherits AbstractMapInteractiveEntity

	' Token: 0x060037E3 RID: 14307 RVA: 0x002008E2 File Offset: 0x001FECE2
	Private Sub Start()
		Me.AddDialoguerEvents()
		If Me.entitySpriteRenderer Is Nothing Then
			Me.entitySpriteRenderer = MyBase.GetComponent(Of SpriteRenderer)()
		End If
	End Sub

	' Token: 0x060037E4 RID: 14308 RVA: 0x00200907 File Offset: 0x001FED07
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me.RemoveDialoguerEvents()
	End Sub

	' Token: 0x060037E5 RID: 14309 RVA: 0x00200918 File Offset: 0x001FED18
	Public Sub AddDialoguerEvents()
		AddHandler Dialoguer.events.onMessageEvent, AddressOf Me.OnDialoguerMessageEvent
		AddHandler Dialoguer.events.onEnded, AddressOf Me.OnDialogueEndedHandler
		AddHandler Dialoguer.events.onInstantlyEnded, AddressOf Me.OnDialogueEndedHandler
	End Sub

	' Token: 0x060037E6 RID: 14310 RVA: 0x00200968 File Offset: 0x001FED68
	Public Sub RemoveDialoguerEvents()
		RemoveHandler Dialoguer.events.onMessageEvent, AddressOf Me.OnDialoguerMessageEvent
		RemoveHandler Dialoguer.events.onEnded, AddressOf Me.OnDialogueEndedHandler
		RemoveHandler Dialoguer.events.onInstantlyEnded, AddressOf Me.OnDialogueEndedHandler
	End Sub

	' Token: 0x060037E7 RID: 14311 RVA: 0x002009B7 File Offset: 0x001FEDB7
	Private Sub OnDialoguerMessageEvent(message As String, metadata As String)
		If message = "QuadratusGift" Then
		End If
	End Sub

	' Token: 0x060037E8 RID: 14312 RVA: 0x002009C9 File Offset: 0x001FEDC9
	Protected Overrides Sub Activate()
	End Sub

	' Token: 0x060037E9 RID: 14313 RVA: 0x002009CC File Offset: 0x001FEDCC
	Protected Overrides Function Show(player As PlayerInput) As MapUIInteractionDialogue
		Dim num As Integer = PlayerData.Data.DeathCount(PlayerId.Any)
		Dim num2 As Single = Mathf.Sqrt(CSng(num))
		If num > 48 AndAlso num2 = Mathf.Round(num2) Then
			Dialoguer.SetGlobalFloat(15, 1F)
		Else
			Dialoguer.SetGlobalFloat(15, 0F)
		End If
		Dialoguer.SetGlobalFloat(Me.dialoguerScholarVariableID, 3F)
		PlayerData.SaveCurrentFile()
		MyBase.StartCoroutine(Me.tween_cr(Me.entitySpriteRenderer.color.a, 0.65F, EaseUtils.EaseType.easeInOutCubic, 0.5F))
		Return Nothing
	End Function

	' Token: 0x060037EA RID: 14314 RVA: 0x00200A64 File Offset: 0x001FEE64
	Public Overrides Sub Hide(dialogue As MapUIInteractionDialogue)
		If Map.Current.CurrentState <> Map.State.Ready Then
			Return
		End If
		If Map.Current.players(0).state <> MapPlayerController.State.Walking Then
			Return
		End If
		MyBase.StartCoroutine(Me.tween_cr(Me.entitySpriteRenderer.color.a, 0F, EaseUtils.EaseType.easeInOutCubic, 0.5F))
	End Sub

	' Token: 0x060037EB RID: 14315 RVA: 0x00200AC4 File Offset: 0x001FEEC4
	Private Sub OnDialogueEndedHandler()
	End Sub

	' Token: 0x060037EC RID: 14316 RVA: 0x00200AC8 File Offset: 0x001FEEC8
	Private Iterator Function tween_cr(start As Single, [end] As Single, ease As EaseUtils.EaseType, time As Single) As IEnumerator
		If start = [end] Then
			Return
		End If
		Dim t As Single = 0F
		Dim currentColor As Color = Color.white
		currentColor.a = start
		Me.entitySpriteRenderer.color = currentColor
		While t < time
			Dim val As Single = EaseUtils.Ease(ease, start, [end], t / time)
			currentColor.a = val
			Me.entitySpriteRenderer.color = currentColor
			t += CupheadTime.Delta
			Yield Nothing
		End While
		currentColor.a = [end]
		Me.entitySpriteRenderer.color = currentColor
		Yield Nothing
		Return
	End Function

	' Token: 0x04003FD8 RID: 16344
	Private Const QUADRATUS_STATE_ID As Integer = 15

	' Token: 0x04003FD9 RID: 16345
	<SerializeField()>
	Private entitySpriteRenderer As SpriteRenderer

	' Token: 0x04003FDA RID: 16346
	<SerializeField()>
	Private dialoguerScholarVariableID As Integer = 11
End Class
