Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports UnityEngine

' Token: 0x02000427 RID: 1063
Public Class MapDialogueInteraction
	Inherits AbstractMapInteractiveEntity

	' Token: 0x06000F6A RID: 3946 RVA: 0x00099930 File Offset: 0x00097D30
	Protected Overridable Sub Start()
		If Me.speechBubble Is Nothing Then
			Dim vector As Vector3 = MyBase.transform.position
			If Me.speechBubblePositions IsNot Nothing Then
				vector = Me.ApplyCustonMargin(vector)
			Else
				vector.x += Me.speechBubblePosition.x
				vector.y += Me.speechBubblePosition.y
			End If
			Me.speechBubble = SpeechBubble.Instance
			If Me.speechBubble Is Nothing Then
				Me.speechBubble = Global.UnityEngine.[Object].Instantiate(Of GameObject)(Me.speechBubblePrefab.gameObject, vector, Quaternion.identity, MapUI.Current.sceneCanvas.transform).GetComponent(Of SpeechBubble)()
			End If
		End If
		AddHandler Dialoguer.events.onEnded, AddressOf Me.OnDialogueEndedHandler
		AddHandler Dialoguer.events.onInstantlyEnded, AddressOf Me.OnDialogueEndedHandler
	End Sub

	' Token: 0x06000F6B RID: 3947 RVA: 0x00099A1C File Offset: 0x00097E1C
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		RemoveHandler Dialoguer.events.onEnded, AddressOf Me.OnDialogueEndedHandler
		RemoveHandler Dialoguer.events.onInstantlyEnded, AddressOf Me.OnDialogueEndedHandler
	End Sub

	' Token: 0x06000F6C RID: 3948 RVA: 0x00099A50 File Offset: 0x00097E50
	Private Sub OnDialogueEndedHandler()
		Me.IsDialogueEnded = True
		Me.Check()
	End Sub

	' Token: 0x06000F6D RID: 3949 RVA: 0x00099A60 File Offset: 0x00097E60
	Protected Overrides Sub OnDrawGizmosSelected()
		MyBase.OnDrawGizmosSelected()
		Dim vector As Vector3 = MyBase.transform.position
		If Me.speechBubblePositions IsNot Nothing Then
			vector = Me.ApplyCustonMargin(vector)
		Else
			vector.x += Me.speechBubblePosition.x
			vector.y += Me.speechBubblePosition.y
		End If
		Gizmos.DrawWireSphere(vector, Me.interactionDistance * 0.5F)
		vector = MyBase.transform.position
		vector.x += Me.panCameraToPosition.x
		vector.y += Me.panCameraToPosition.y
		Gizmos.DrawWireSphere(vector, Me.interactionDistance * 0.5F)
	End Sub

	' Token: 0x06000F6E RID: 3950 RVA: 0x00099B2C File Offset: 0x00097F2C
	Protected Overrides Sub Activate(player As MapPlayerController)
		If Me.dialogues(CInt(player.id)) IsNot Nothing AndAlso Me.dialogues(CInt(player.id)).transform.localScale.x = 1F AndAlso MapBasicStartUI.Current.CurrentState = AbstractMapSceneStartUI.State.Inactive AndAlso MapDifficultySelectStartUI.Current.CurrentState = AbstractMapSceneStartUI.State.Inactive AndAlso MapConfirmStartUI.Current.CurrentState = AbstractMapSceneStartUI.State.Inactive AndAlso (Map.Current Is Nothing OrElse Map.Current.CurrentState <> Map.State.Graveyard) Then
			MyBase.Activate(player)
			Me.StartSpeechBubble()
		End If
	End Sub

	' Token: 0x06000F6F RID: 3951 RVA: 0x00099BD4 File Offset: 0x00097FD4
	Protected Overridable Sub StartSpeechBubble()
		If Me.speechBubble.displayState = SpeechBubble.DisplayState.Hidden Then
			Dim vector As Vector3 = MyBase.transform.position
			If Me.speechBubblePositions IsNot Nothing Then
				vector = Me.ApplyCustonMargin(vector)
			Else
				vector.x += Me.speechBubblePosition.x
				vector.y += Me.speechBubblePosition.y
			End If
			Me.speechBubble.basePosition = vector
			vector = MyBase.transform.position
			vector.x += Me.panCameraToPosition.x
			vector.y += Me.panCameraToPosition.y
			Me.speechBubble.panPosition = vector
			Me.speechBubble.maxLines = Me.maxLines
			Me.speechBubble.tailOnTheLeft = Me.tailOnTheLeft
			Me.speechBubble.expandOnTheRight = Me.expandOnTheRight
			Me.speechBubble.hideTail = Me.hideTail
			If Me.cutsceneCoroutine IsNot Nothing Then
				MyBase.StopCoroutine(Me.cutsceneCoroutine)
			End If
			Me.cutsceneCoroutine = MyBase.StartCoroutine(Me.CutScene_cr())
		End If
	End Sub

	' Token: 0x06000F70 RID: 3952 RVA: 0x00099D14 File Offset: 0x00098114
	Protected Overrides Sub Check()
		If Me.disabledActivations Then
			Return
		End If
		MyBase.Check()
	End Sub

	' Token: 0x06000F71 RID: 3953 RVA: 0x00099D28 File Offset: 0x00098128
	Private Iterator Function CutScene_cr() As IEnumerator
		If Me.speechBubble.displayState <> SpeechBubble.DisplayState.Hidden Then
			Return
		End If
		For i As Integer = 0 To Map.Current.players.Length - 1
			If Not(Map.Current.players(i) Is Nothing) Then
				Map.Current.players(i).Disable()
			End If
		Next
		Yield Nothing
		Me.currentlySpeaking = True
		Dialoguer.StartDialogue(Me.dialogueInteraction)
		Dim afterDialogue As DialoguerEvents.EndedHandler = Nothing
		afterDialogue = Sub()
			RemoveHandler Dialoguer.events.onEnded, afterDialogue
			Me.StartCoroutine(Me.reactivate_input_cr())
		End Sub
		AddHandler Dialoguer.events.onEnded, afterDialogue
		Return
	End Function

	' Token: 0x06000F72 RID: 3954 RVA: 0x00099D44 File Offset: 0x00098144
	Private Iterator Function reactivate_input_cr() As IEnumerator
		If CupheadMapCamera.Current IsNot Nothing Then
			CupheadMapCamera.Current.SetActiveCollider(False)
		End If
		While CupheadMapCamera.Current IsNot Nothing AndAlso CupheadMapCamera.Current.IsCameraFarFromPlayer()
			Yield Nothing
		End While
		If CupheadMapCamera.Current IsNot Nothing Then
			CupheadMapCamera.Current.SetActiveCollider(True)
		End If
		For i As Integer = 0 To Map.Current.players.Length - 1
			If Not(Map.Current.players(i) Is Nothing) Then
				Map.Current.players(i).Enable()
			End If
		Next
		Me.currentlySpeaking = False
		Return
	End Function

	' Token: 0x06000F73 RID: 3955 RVA: 0x00099D60 File Offset: 0x00098160
	Private Function ApplyCustonMargin(pos As Vector3) As Vector3
		Dim num As Integer = 0
		Dim flag As Boolean = False
		While Not flag AndAlso num < Me.speechBubblePositions.Length
			flag = Me.speechBubblePositions(num).languageApplied = Localization.language
			num = If((Not flag), (num + 1), num)
		End While
		If flag Then
			Dim speechBubblePositionLanguage As MapDialogueInteraction.speechBubblePositionLanguage = Me.speechBubblePositions(num)
			pos.x += speechBubblePositionLanguage.speechBubblePosition.x
			pos.y += speechBubblePositionLanguage.speechBubblePosition.y
		Else
			pos.x += Me.speechBubblePosition.x
			pos.y += Me.speechBubblePosition.y
		End If
		Return pos
	End Function

	' Token: 0x06000F74 RID: 3956 RVA: 0x00099E38 File Offset: 0x00098238
	Public Iterator Function DebugStartDialogue() As IEnumerator
		Yield MyBase.StartCoroutine(Me.debugActivate_input_cr())
		Return
	End Function

	' Token: 0x06000F75 RID: 3957 RVA: 0x00099E54 File Offset: 0x00098254
	Private Iterator Function debugActivate_input_cr() As IEnumerator
		Dim DoneLanguage As Boolean = False
		Dim index As Integer = 0
		Dim Wait As WaitForSeconds = New WaitForSeconds(0.3F)
		Dim startedLanguage As Localization.Languages = Localization.language
		Dim ConditionIndex As Integer = 0
		Dim DoneVariables As Boolean = False
		Yield Wait
		Yield Wait
		While Not DoneLanguage
			DoneVariables = False
			ConditionIndex = 0
			While Not DoneVariables
				If Me.DebugDialogerCondition.Count > 0 Then
					Dialoguer.SetGlobalFloat(Me.DebugDialogerCondition(ConditionIndex).ConditionId, Me.DebugDialogerCondition(ConditionIndex).Values)
				End If
				Me.IsDialogueEnded = False
				index = 0
				Yield Wait
				Yield Wait
				Me.Activate(Map.Current.players(0))
				Yield Wait
				While Not Me.IsDialogueEnded
					Dim ConditionKey As String = String.Empty
					If Me.DebugDialogerCondition.Count > 0 Then
						ConditionKey = String.Concat(New Object() { "_", Me.DebugDialogerCondition(ConditionIndex).ConditionId, "_", Me.DebugDialogerCondition(ConditionIndex).Values })
					End If
					Dim cameraType As ScreenshotHandler.cameraType = ScreenshotHandler.cameraType.Map
					Dim text As String = "LOC_Screenshot"
					Dim array As Object() = New Object(5) {}
					array(0) = Me.dialogueInteraction.ToString()
					array(1) = ConditionKey
					array(2) = "_"
					array(3) = Localization.language
					array(4) = "_"
					Dim num As Integer = 5
					Dim num2 As Integer = index
					Dim num3 As Integer = num2
					index = num2 + 1
					array(num) = num3
					ScreenshotHandler.TakeScreenshot_Static(cameraType, text, String.Concat(array))
					Yield Wait
					Dialoguer.ContinueDialogue()
					Yield Wait
				End While
				ConditionIndex += 1
				If ConditionIndex >= Me.DebugDialogerCondition.Count Then
					DoneVariables = True
				End If
			End While
			Yield Nothing
			Yield Nothing
			Yield Nothing
			If Localization.language = startedLanguage Then
				DoneLanguage = True
			End If
		End While
		Return
	End Function

	' Token: 0x0400187F RID: 6271
	<SerializeField()>
	Private speechBubblePrefab As SpeechBubble

	' Token: 0x04001880 RID: 6272
	<SerializeField()>
	Private speechBubblePosition As Vector2

	' Token: 0x04001881 RID: 6273
	<SerializeField()>
	Private speechBubblePositions As MapDialogueInteraction.speechBubblePositionLanguage()

	' Token: 0x04001882 RID: 6274
	<SerializeField()>
	Private panCameraToPosition As Vector2

	' Token: 0x04001883 RID: 6275
	<SerializeField()>
	Private maxLines As Integer = -1

	' Token: 0x04001884 RID: 6276
	<SerializeField()>
	Private tailOnTheLeft As Boolean

	' Token: 0x04001885 RID: 6277
	<SerializeField()>
	Private hideTail As Boolean

	' Token: 0x04001886 RID: 6278
	<SerializeField()>
	Private expandOnTheRight As Boolean

	' Token: 0x04001887 RID: 6279
	Protected speechBubble As SpeechBubble

	' Token: 0x04001888 RID: 6280
	Public dialogueInteraction As DialoguerDialogues

	' Token: 0x04001889 RID: 6281
	Private cutsceneCoroutine As Coroutine

	' Token: 0x0400188A RID: 6282
	<HideInInspector()>
	Public disabledActivations As Boolean

	' Token: 0x0400188B RID: 6283
	<Header("DEBUG")>
	Public DebugDialogerCondition As List(Of MapDialogueInteraction.DEBUG_DialoguerCondition) = New List(Of MapDialogueInteraction.DEBUG_DialoguerCondition)()

	' Token: 0x0400188C RID: 6284
	Private IsDialogueEnded As Boolean

	' Token: 0x0400188D RID: 6285
	Public currentlySpeaking As Boolean

	' Token: 0x02000428 RID: 1064
	<Serializable()>
	Public Structure speechBubblePositionLanguage
		' Token: 0x0400188E RID: 6286
		Public languageApplied As Localization.Languages

		' Token: 0x0400188F RID: 6287
		Public speechBubblePosition As Vector2
	End Structure

	' Token: 0x02000429 RID: 1065
	<Serializable()>
	Public Structure DEBUG_DialoguerCondition
		' Token: 0x04001890 RID: 6288
		Public ConditionId As Integer

		' Token: 0x04001891 RID: 6289
		Public Values As Single
	End Structure
End Class
