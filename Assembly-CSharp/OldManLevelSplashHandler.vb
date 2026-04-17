Imports System
Imports System.Collections.Generic
Imports UnityEngine

' Token: 0x02000714 RID: 1812
Public Class OldManLevelSplashHandler
	Inherits AbstractPausableComponent

	' Token: 0x06002766 RID: 10086 RVA: 0x00171F90 File Offset: 0x00170390
	Public Sub SplashOut(posX As Single)
		Me.splashOut.Create(New Vector3(posX, MyBase.transform.position.y))
	End Sub

	' Token: 0x06002767 RID: 10087 RVA: 0x00171FC4 File Offset: 0x001703C4
	Public Sub SplashIn(posX As Single)
		Me.splashIn.Create(New Vector3(posX, MyBase.transform.position.y))
	End Sub

	' Token: 0x06002768 RID: 10088 RVA: 0x00171FF8 File Offset: 0x001703F8
	Private Sub Update()
		Dim allPlayers As Dictionary(Of Integer, AbstractPlayerController).ValueCollection = PlayerManager.GetAllPlayers()
		For i As Integer = 0 To 2 - 1
			Dim player As AbstractPlayerController = PlayerManager.GetPlayer(CType(i, PlayerId))
			If player Is Nothing OrElse player.IsDead Then
				Me.lastKnownPlayerPos(i) = Vector3.zero
			Else
				If Me.lastKnownPlayerPos(i).y < MyBase.transform.position.y Then
					If player.transform.position.y > MyBase.transform.position.y Then
					End If
				ElseIf player.transform.position.y <= MyBase.transform.position.y Then
					Me.SplashIn(player.transform.position.x)
					Me.SFX_PlayerSplashIn()
				End If
				Me.lastKnownPlayerPos(i) = player.transform.position
			End If
		Next
	End Sub

	' Token: 0x06002769 RID: 10089 RVA: 0x0017211A File Offset: 0x0017051A
	Private Sub SFX_PlayerSplashIn()
		AudioManager.Play("sfx_dlc_omm_p3_stomachacid_splash")
		Me.emitAudioFromObject.Add("sfx_dlc_omm_p3_stomachacid_splash")
	End Sub

	' Token: 0x04003022 RID: 12322
	<SerializeField()>
	Private splashIn As Effect

	' Token: 0x04003023 RID: 12323
	<SerializeField()>
	Private splashOut As Effect

	' Token: 0x04003024 RID: 12324
	Private lastKnownPlayerPos As Vector3() = New Vector3() { Vector3.zero, Vector3.zero }
End Class
