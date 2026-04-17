Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000885 RID: 2181
Public Class TreePlatformingLevelAudioArea
	Inherits AbstractPausableComponent

	' Token: 0x060032A1 RID: 12961 RVA: 0x001D6B42 File Offset: 0x001D4F42
	Private Sub Start()
		MyBase.StartCoroutine(Me.check_sound_cr())
	End Sub

	' Token: 0x060032A2 RID: 12962 RVA: 0x001D6B51 File Offset: 0x001D4F51
	Private Sub PlaySound()
		AudioManager.PlayLoop("amb_treecave")
		MyBase.StartCoroutine(Me.fade_volume_cr(True))
	End Sub

	' Token: 0x060032A3 RID: 12963 RVA: 0x001D6B6B File Offset: 0x001D4F6B
	Private Sub StopSound()
		MyBase.StartCoroutine(Me.fade_volume_cr(False))
	End Sub

	' Token: 0x060032A4 RID: 12964 RVA: 0x001D6B7C File Offset: 0x001D4F7C
	Private Iterator Function fade_volume_cr(fadeIn As Boolean) As IEnumerator
		Me.isFading = True
		Dim time As Single = 1F
		Dim t As Single = 0F
		While t < time
			t += CupheadTime.Delta
			AudioManager.Attenuation("amb_treecave", True, If((Not fadeIn), (1F - t / time), (t / time)))
			Yield Nothing
		End While
		If Not fadeIn Then
			AudioManager.[Stop]("amb_treecave")
		End If
		Me.isFading = False
		Yield Nothing
		Return
	End Function

	' Token: 0x060032A5 RID: 12965 RVA: 0x001D6BA0 File Offset: 0x001D4FA0
	Private Iterator Function check_sound_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 0.2F)
		Dim player As AbstractPlayerController = PlayerManager.GetNext()
		While True
			If player.transform.position.x > Me.startPoint.transform.position.x AndAlso player.transform.position.x < Me.endPoint.transform.position.x Then
				If Not AudioManager.CheckIfPlaying("amb_treecave") Then
					Me.PlaySound()
				End If
			ElseIf AudioManager.CheckIfPlaying("amb_treecave") AndAlso Not Me.isFading Then
				Me.StopSound()
			End If
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x060032A6 RID: 12966 RVA: 0x001D6BBC File Offset: 0x001D4FBC
	Private Iterator Function play_one_shots_cr() As IEnumerator
		Dim delay As MinMax = New MinMax(4F, 8F)
		While True
			If AudioManager.CheckIfPlaying("amb_treecave") Then
				Yield CupheadTime.WaitForSeconds(Me, delay)
				AudioManager.Play("NAME")
			End If
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x060032A7 RID: 12967 RVA: 0x001D6BD8 File Offset: 0x001D4FD8
	Protected Overrides Sub OnDrawGizmos()
		MyBase.OnDrawGizmos()
		Gizmos.color = Color.red
		Gizmos.DrawLine(New Vector2(Me.startPoint.position.x, Me.startPoint.position.y + 1000F), New Vector2(Me.startPoint.position.x, Me.startPoint.position.y - 1000F))
		Gizmos.color = Color.blue
		Gizmos.DrawLine(New Vector2(Me.endPoint.position.x, Me.endPoint.position.y + 1000F), New Vector2(Me.endPoint.position.x, Me.endPoint.position.y - 1000F))
	End Sub

	' Token: 0x04003AE5 RID: 15077
	<SerializeField()>
	Private startPoint As Transform

	' Token: 0x04003AE6 RID: 15078
	<SerializeField()>
	Private endPoint As Transform

	' Token: 0x04003AE7 RID: 15079
	Private isFading As Boolean
End Class
