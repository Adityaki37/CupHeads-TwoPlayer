Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000949 RID: 2377
Public Class MapNPCBarbershopSong
	Inherits MonoBehaviour

	' Token: 0x06003784 RID: 14212 RVA: 0x001FE4C2 File Offset: 0x001FC8C2
	Private Sub Start()
		Me.input = New CupheadInput.AnyPlayerInput(False)
		Me.AddDialoguerEvents()
	End Sub

	' Token: 0x06003785 RID: 14213 RVA: 0x001FE4D8 File Offset: 0x001FC8D8
	Private Sub Update()
		If Me.songCoroutine IsNot Nothing AndAlso Me.delay AndAlso (Me.input.GetAnyButtonDown() OrElse Me.songEndedOrPlayerStop) Then
			MyBase.StopCoroutine(Me.songCoroutine)
			Me.songCoroutine = Nothing
			Me.delay = False
			Me.songEndedOrPlayerStop = True
			AudioManager.[Stop]("mus_barbershop")
			AudioManager.FadeBGMVolume(1F, 0.5F, False)
			AudioManager.FadeSFXVolume("worldmap_hint_djimmithegreat", 1F, 0.5F)
			For i As Integer = 0 To Me.barbershopAnimators.Length - 1
				Me.barbershopAnimators(i).SetTrigger("endsong")
			Next
			Me.songEndedOrPlayerStop = False
			If Map.Current IsNot Nothing Then
				Map.Current.CurrentState = Map.State.Ready
			End If
			For j As Integer = 0 To Map.Current.players.Length - 1
				If Not(Map.Current.players(j) Is Nothing) Then
					Map.Current.players(j).Enable()
				End If
			Next
		End If
	End Sub

	' Token: 0x06003786 RID: 14214 RVA: 0x001FE5FC File Offset: 0x001FC9FC
	Private Sub OnDestroy()
		Me.RemoveDialoguerEvents()
	End Sub

	' Token: 0x06003787 RID: 14215 RVA: 0x001FE604 File Offset: 0x001FCA04
	Private Iterator Function sing_cr() As IEnumerator
		AudioManager.FadeBGMVolume(0F, 0.5F, True)
		AudioManager.FadeSFXVolume("worldmap_hint_djimmithegreat", 0.01F, 0.5F)
		AudioManager.Play("mus_barbershop")
		Yield Nothing
		For i As Integer = 0 To Map.Current.players.Length - 1
			If Not(Map.Current.players(i) Is Nothing) Then
				Map.Current.players(i).Disable()
			End If
		Next
		If Map.Current IsNot Nothing Then
			Map.Current.CurrentState = Map.State.[Event]
		End If
		For j As Integer = 0 To Me.barbershopAnimators.Length - 1
			Me.barbershopAnimators(j).SetTrigger("sing")
		Next
		Yield CupheadTime.WaitForSeconds(Me, 0.5F)
		Me.delay = True
		Yield Me.barbershopAnimators(3).WaitForAnimationToStart(Me, "anim_map_barbershop_sing_hold", False)
		Me.barbershopAnimators(0).SetTrigger("trans")
		Yield New WaitForSeconds(0.083333336F)
		Me.barbershopAnimators(1).SetTrigger("trans")
		Yield New WaitForSeconds(0.083333336F)
		Me.barbershopAnimators(2).SetTrigger("trans")
		Yield New WaitForSeconds(0.083333336F)
		Me.barbershopAnimators(3).SetTrigger("trans")
		Yield Me.barbershopAnimators(3).WaitForAnimationToStart(Me, "anim_map_barbershop_sing_idle_boil", True)
		Me.barbershopAnimators(0).SetTrigger("blink")
		Yield New WaitForSeconds(0.083333336F)
		Me.barbershopAnimators(1).SetTrigger("blink")
		Yield New WaitForSeconds(0.083333336F)
		Me.barbershopAnimators(2).SetTrigger("blink")
		Yield New WaitForSeconds(0.083333336F)
		Me.barbershopAnimators(3).SetTrigger("blink")
		Yield Me.barbershopAnimators(3).WaitForAnimationToStart(Me, "anim_map_barbershop_sing_main_loop", True)
		While Not Me.songEndedOrPlayerStop
			Yield Nothing
			If Not AudioManager.CheckIfPlaying("mus_barbershop") Then
				Me.songEndedOrPlayerStop = True
			End If
		End While
		For k As Integer = 0 To Me.barbershopAnimators.Length - 1
			Me.barbershopAnimators(k).SetTrigger("endsong")
		Next
		Yield Nothing
		Me.songCoroutine = Nothing
		Me.songEndedOrPlayerStop = False
		If Map.Current IsNot Nothing Then
			Map.Current.CurrentState = Map.State.Ready
		End If
		For l As Integer = 0 To Map.Current.players.Length - 1
			If Not(Map.Current.players(l) Is Nothing) Then
				Map.Current.players(l).Enable()
			End If
		Next
		AudioManager.FadeBGMVolume(1F, 0.5F, False)
		AudioManager.FadeSFXVolume("worldmap_hint_djimmithegreat", 1F, 0.5F)
		Return
	End Function

	' Token: 0x06003788 RID: 14216 RVA: 0x001FE61F File Offset: 0x001FCA1F
	Public Sub AddDialoguerEvents()
		AddHandler Dialoguer.events.onMessageEvent, AddressOf Me.OnDialoguerMessageEvent
	End Sub

	' Token: 0x06003789 RID: 14217 RVA: 0x001FE637 File Offset: 0x001FCA37
	Public Sub RemoveDialoguerEvents()
		RemoveHandler Dialoguer.events.onMessageEvent, AddressOf Me.OnDialoguerMessageEvent
	End Sub

	' Token: 0x0600378A RID: 14218 RVA: 0x001FE650 File Offset: 0x001FCA50
	Private Sub OnDialoguerMessageEvent(message As String, metadata As String)
		If Me.SkipDialogueEvent Then
			Return
		End If
		If message = "QuartetSing" Then
			If Me.songCoroutine IsNot Nothing Then
				MyBase.StopCoroutine(Me.songCoroutine)
			End If
			Me.songCoroutine = MyBase.StartCoroutine(Me.sing_cr())
		End If
	End Sub

	' Token: 0x04003F96 RID: 16278
	Private input As CupheadInput.AnyPlayerInput

	' Token: 0x04003F97 RID: 16279
	<SerializeField()>
	Private barbershopAnimators As Animator() = New Animator(3) {}

	' Token: 0x04003F98 RID: 16280
	Private songCoroutine As Coroutine

	' Token: 0x04003F99 RID: 16281
	Public songEndedOrPlayerStop As Boolean

	' Token: 0x04003F9A RID: 16282
	Private delay As Boolean

	' Token: 0x04003F9B RID: 16283
	<HideInInspector()>
	Public SkipDialogueEvent As Boolean
End Class
