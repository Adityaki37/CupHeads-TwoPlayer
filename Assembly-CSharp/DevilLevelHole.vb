Imports System
Imports System.Collections

' Token: 0x0200057A RID: 1402
Public Class DevilLevelHole
	Inherits AbstractCollidableObject

	' Token: 0x1700034F RID: 847
	' (get) Token: 0x06001AB0 RID: 6832 RVA: 0x000F4BCE File Offset: 0x000F2FCE
	' (set) Token: 0x06001AB1 RID: 6833 RVA: 0x000F4BD5 File Offset: 0x000F2FD5
	Public Shared Property PHASE_1_COMPLETE As Boolean

	' Token: 0x06001AB2 RID: 6834 RVA: 0x000F4BDD File Offset: 0x000F2FDD
	Private Sub Start()
		DevilLevelHole.PHASE_1_COMPLETE = False
		MyBase.StartCoroutine(Me.check_player_cr())
	End Sub

	' Token: 0x06001AB3 RID: 6835 RVA: 0x000F4BF4 File Offset: 0x000F2FF4
	Private Iterator Function check_player_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 0.2F)
		While True
			If PlayerManager.GetPlayer(PlayerId.PlayerTwo) IsNot Nothing AndAlso Not PlayerManager.GetPlayer(PlayerId.PlayerTwo).IsDead Then
				If PlayerManager.GetPlayer(PlayerId.PlayerOne).IsDead Then
					If PlayerManager.GetPlayer(PlayerId.PlayerTwo).transform.position.y < MyBase.transform.position.y Then
						Exit For
					End If
				ElseIf PlayerManager.GetPlayer(PlayerId.PlayerTwo).transform.position.y < MyBase.transform.position.y AndAlso PlayerManager.GetPlayer(PlayerId.PlayerOne).transform.position.y < MyBase.transform.position.y Then
					GoTo Block_6
				End If
			ElseIf PlayerManager.GetPlayer(PlayerId.PlayerOne).transform.position.y < MyBase.transform.position.y Then
				GoTo Block_7
			End If
			Yield Nothing
		End While
		DevilLevelHole.PHASE_1_COMPLETE = True
		GoTo IL_01A4
		Block_6:
		DevilLevelHole.PHASE_1_COMPLETE = True
		GoTo IL_01A4
		Block_7:
		DevilLevelHole.PHASE_1_COMPLETE = True
		IL_01A4:
		Return
	End Function
End Class
