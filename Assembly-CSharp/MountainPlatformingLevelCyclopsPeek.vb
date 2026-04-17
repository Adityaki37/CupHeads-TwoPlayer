Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020008DA RID: 2266
Public Class MountainPlatformingLevelCyclopsPeek
	Inherits AbstractPausableComponent

	' Token: 0x06003510 RID: 13584 RVA: 0x001ED6D9 File Offset: 0x001EBAD9
	Private Sub Start()
		MyBase.StartCoroutine(Me.check_player_cr())
		Me.emitAudioFromObject.Add("castle_giant_head_peer")
	End Sub

	' Token: 0x06003511 RID: 13585 RVA: 0x001ED6F8 File Offset: 0x001EBAF8
	Private Iterator Function check_player_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 0.2F)
		Me.player = PlayerManager.GetNext()
		Dim t As Single = 0F
		Dim time As Single = Global.UnityEngine.Random.Range(3F, 6F)
		Dim laughTime As Single = Global.UnityEngine.Random.Range(6F, 10F)
		Dim t_laugh As Single = 0F
		While True
			If PlayerManager.GetPlayer(PlayerId.PlayerTwo) IsNot Nothing Then
				t += CupheadTime.Delta
				If t >= time Then
					Me.player = PlayerManager.GetNext()
					t = 0F
					time = Global.UnityEngine.Random.Range(3F, 6F)
				End If
			End If
			If Vector3.Distance(PlayerManager.GetPlayer(PlayerId.PlayerOne).transform.position, MyBase.transform.position) < 1000F Then
				t_laugh += CupheadTime.Delta
				If t_laugh >= laughTime Then
					Me.SoundGiantHeadPeer()
					t_laugh = 0F
					laughTime = Global.UnityEngine.Random.Range(6F, 10F)
				End If
			End If
			If Me.player.transform.position.x < MyBase.transform.position.x - Me.offset Then
				If Me.currentEyeState <> MountainPlatformingLevelCyclopsPeek.EyeState.left Then
					MyBase.animator.SetInteger("SideOn", 0)
					Me.currentEyeState = MountainPlatformingLevelCyclopsPeek.EyeState.left
				End If
			ElseIf Me.player.transform.position.x > MyBase.transform.position.x + Me.offset Then
				If Me.currentEyeState <> MountainPlatformingLevelCyclopsPeek.EyeState.right Then
					MyBase.animator.SetInteger("SideOn", 2)
					Me.currentEyeState = MountainPlatformingLevelCyclopsPeek.EyeState.right
				End If
			ElseIf Me.currentEyeState <> MountainPlatformingLevelCyclopsPeek.EyeState.middle Then
				MyBase.animator.SetInteger("SideOn", 1)
				Me.currentEyeState = MountainPlatformingLevelCyclopsPeek.EyeState.middle
			End If
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06003512 RID: 13586 RVA: 0x001ED713 File Offset: 0x001EBB13
	Private Sub SoundGiantHeadPeer()
		AudioManager.Play("castle_giant_head_peer")
		Me.emitAudioFromObject.Add("castle_giant_head_peer")
	End Sub

	' Token: 0x04003D33 RID: 15667
	Private currentEyeState As MountainPlatformingLevelCyclopsPeek.EyeState

	' Token: 0x04003D34 RID: 15668
	Private offset As Single = 300F

	' Token: 0x04003D35 RID: 15669
	Private player As AbstractPlayerController

	' Token: 0x020008DB RID: 2267
	Public Enum EyeState
		' Token: 0x04003D37 RID: 15671
		left
		' Token: 0x04003D38 RID: 15672
		middle
		' Token: 0x04003D39 RID: 15673
		right
	End Enum
End Class
