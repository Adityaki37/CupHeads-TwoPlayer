Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x0200094C RID: 2380
Public Class MapNPCChaliceFanB
	Inherits AbstractMonoBehaviour

	' Token: 0x0600379D RID: 14237 RVA: 0x001FF190 File Offset: 0x001FD590
	Private Sub Start()
		If PlayerData.Data.hasTalkedToChaliceFan Then
			Dialoguer.SetGlobalFloat(25, 1F)
		End If
		Me.AddDialoguerEvents()
		Dim num As Integer = 0
		For i As Integer = 0 To Level.chaliceLevels.Length - 1
			If PlayerData.Data.GetLevelData(Level.chaliceLevels(i)).completedAsChaliceP1 Then
				Me.lineSprites(i).enabled = True
			End If
		Next
		For j As Integer = 0 To Level.chaliceLevels.Length - 1
			If PlayerData.Data.GetLevelData(Level.chaliceLevels(j)).completedAsChaliceP1 Then
				num += 1
			ElseIf Me.undefeatedBoss = Levels.Test Then
				Me.undefeatedBoss = Level.chaliceLevels(j)
			End If
		Next
		If num = Level.chaliceLevels.Length Then
			Dialoguer.SetGlobalFloat(25, 2F)
			Me.campfire.gameObject.SetActive(True)
			MyBase.StartCoroutine(Me.campfire_smoke_cr())
			Me.questComplete = True
		Else
			Me.SetBossRefText(Me.undefeatedBoss)
		End If
		MyBase.StartCoroutine(Me.blink_cr())
	End Sub

	' Token: 0x0600379E RID: 14238 RVA: 0x001FF2B4 File Offset: 0x001FD6B4
	Private Sub SetBossRefText(level As Levels)
		Dim translationElement As TranslationElement = Localization.Find(level.ToString() + "Reference")
		SpeechBubble.Instance.setBossRefText = translationElement.translation.text
	End Sub

	' Token: 0x0600379F RID: 14239 RVA: 0x001FF2F6 File Offset: 0x001FD6F6
	Private Sub UpdateBossRef()
		Me.SetBossRefText(Me.undefeatedBoss)
	End Sub

	' Token: 0x060037A0 RID: 14240 RVA: 0x001FF304 File Offset: 0x001FD704
	Private Sub OnDestroy()
		Me.RemoveDialoguerEvents()
	End Sub

	' Token: 0x060037A1 RID: 14241 RVA: 0x001FF30C File Offset: 0x001FD70C
	Public Sub AddDialoguerEvents()
		AddHandler Localization.OnLanguageChangedEvent, AddressOf Me.UpdateBossRef
		AddHandler Dialoguer.events.onMessageEvent, AddressOf Me.OnDialoguerMessageEvent
		AddHandler Dialoguer.events.onEnded, AddressOf Me.OnDialogueEnded
	End Sub

	' Token: 0x060037A2 RID: 14242 RVA: 0x001FF34B File Offset: 0x001FD74B
	Public Sub RemoveDialoguerEvents()
		RemoveHandler Localization.OnLanguageChangedEvent, AddressOf Me.UpdateBossRef
		RemoveHandler Dialoguer.events.onMessageEvent, AddressOf Me.OnDialoguerMessageEvent
		RemoveHandler Dialoguer.events.onEnded, AddressOf Me.OnDialogueEnded
	End Sub

	' Token: 0x060037A3 RID: 14243 RVA: 0x001FF38A File Offset: 0x001FD78A
	Private Sub OnDialoguerMessageEvent(message As String, metadata As String)
		If message = "MetChaliceFan" Then
			PlayerData.Data.hasTalkedToChaliceFan = True
			PlayerData.SaveCurrentFile()
			Dialoguer.SetGlobalFloat(25, 1F)
		End If
	End Sub

	' Token: 0x060037A4 RID: 14244 RVA: 0x001FF3B8 File Offset: 0x001FD7B8
	Private Iterator Function blink_cr() As IEnumerator
		While True
			Yield CupheadTime.WaitForSeconds(Me, Me.blinkRange.RandomFloat())
			While MyBase.animator.GetCurrentAnimatorStateInfo(0).normalizedTime Mod 1F > 0.1F
				Yield Nothing
			End While
			Me.blinkRend.enabled = True
			While MyBase.animator.GetCurrentAnimatorStateInfo(0).normalizedTime Mod 1F < 0.9F
				Yield Nothing
			End While
			Me.blinkRend.enabled = False
		End While
		Return
	End Function

	' Token: 0x060037A5 RID: 14245 RVA: 0x001FF3D4 File Offset: 0x001FD7D4
	Private Iterator Function campfire_smoke_cr() As IEnumerator
		Me.campfire.SetBool("SmokeL", True)
		Yield CupheadTime.WaitForSeconds(Me, Global.UnityEngine.Random.Range(0.5F, 1F))
		Me.campfire.SetBool("SmokeR", True)
		While True
			If Not Me.campfire.GetCurrentAnimatorStateInfo(1).IsName("None") OrElse Not Me.campfire.GetCurrentAnimatorStateInfo(2).IsName("None") Then
				Yield CupheadTime.WaitForSeconds(Me, Global.UnityEngine.Random.Range(1.5F, 3F))
			End If
			Me.campfire.SetBool("SmokeL", Rand.Bool())
			Me.campfire.SetBool("SmokeR", Rand.Bool())
			If Me.campfire.GetCurrentAnimatorStateInfo(1).IsName("None") Then
				Me.campfire.SetBool("SmokeL", True)
			End If
			If Me.campfire.GetCurrentAnimatorStateInfo(2).IsName("None") Then
				Me.campfire.SetBool("SmokeR", True)
			End If
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x060037A6 RID: 14246 RVA: 0x001FF3F0 File Offset: 0x001FD7F0
	Private Sub OnDialogueEnded()
		If Me.questComplete AndAlso Not PlayerData.Data.unlockedChaliceRecolor Then
			MapEventNotification.Current.ShowTooltipEvent(TooltipEvent.ChaliceFan)
			PlayerData.Data.unlockedChaliceRecolor = True
			PlayerData.SaveCurrentFile()
			MapUI.Current.Refresh()
		End If
	End Sub

	' Token: 0x04003FA6 RID: 16294
	Private Const CHALICEFANBSTATE_INDEX As Integer = 25

	' Token: 0x04003FA7 RID: 16295
	<SerializeField()>
	Private lineSprites As SpriteRenderer()

	' Token: 0x04003FA8 RID: 16296
	<SerializeField()>
	Private blinkRend As SpriteRenderer

	' Token: 0x04003FA9 RID: 16297
	<SerializeField()>
	Private blinkRange As MinMax = New MinMax(3F, 5F)

	' Token: 0x04003FAA RID: 16298
	<SerializeField()>
	Private campfire As Animator

	' Token: 0x04003FAB RID: 16299
	Private undefeatedBoss As Levels = Levels.Test

	' Token: 0x04003FAC RID: 16300
	Private questComplete As Boolean
End Class
