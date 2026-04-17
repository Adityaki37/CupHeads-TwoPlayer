Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020008D5 RID: 2261
Public Class HarbourPlatformingLevelUrchin
	Inherits PlatformingLevelGroundMovementEnemy

	' Token: 0x060034E7 RID: 13543 RVA: 0x001EC588 File Offset: 0x001EA988
	Protected Overrides Sub Start()
		MyBase.Start()
		MyBase.GetComponent(Of PlatformingLevelEnemyAnimationHandler)().SelectAnimation(Me.type.ToString())
		MyBase.StartCoroutine(Me.play_loop_SFX())
	End Sub

	' Token: 0x060034E8 RID: 13544 RVA: 0x001EC5BC File Offset: 0x001EA9BC
	Private Iterator Function play_loop_SFX() As IEnumerator
		Dim playerLeft As Boolean = False
		While True
			If CupheadLevelCamera.Current.ContainsPoint(MyBase.transform.position, New Vector2(100F, 1000F)) Then
				playerLeft = False
				If Not AudioManager.CheckIfPlaying("harbour_urchin_walk") Then
					AudioManager.PlayLoop("harbour_urchin_walk")
					Me.emitAudioFromObject.Add("harbour_urchin_walk")
				End If
			ElseIf Not playerLeft Then
				AudioManager.[Stop]("harbour_urchin_walk")
				playerLeft = True
			End If
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x060034E9 RID: 13545 RVA: 0x001EC5D8 File Offset: 0x001EA9D8
	Protected Overrides Function Turn() As Coroutine
		If CupheadLevelCamera.Current.ContainsPoint(MyBase.transform.position, New Vector2(100F, 1000F)) Then
			AudioManager.Play("harbour_urchin_turn")
			Me.emitAudioFromObject.Add("harbour_urchin_turn")
		End If
		Return MyBase.Turn()
	End Function

	' Token: 0x060034EA RID: 13546 RVA: 0x001EC633 File Offset: 0x001EAA33
	Protected Overrides Sub Die()
		MyBase.Die()
		AudioManager.[Stop]("harbour_urchin_walk")
		AudioManager.Play("harmour_urchin_death")
		Me.emitAudioFromObject.Add("harmour_urchin_death")
	End Sub

	' Token: 0x04003D18 RID: 15640
	Private Const ON_SCREEN_SOUND_PADDING As Single = 100F

	' Token: 0x04003D19 RID: 15641
	Public type As HarbourPlatformingLevelUrchin.Type

	' Token: 0x04003D1A RID: 15642
	Private isInSight As Boolean

	' Token: 0x020008D6 RID: 2262
	Public Enum Type
		' Token: 0x04003D1C RID: 15644
		A
		' Token: 0x04003D1D RID: 15645
		B
	End Enum
End Class
