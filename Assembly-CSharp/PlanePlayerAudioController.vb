Imports System
Imports System.Collections

' Token: 0x02000A94 RID: 2708
Public Class PlanePlayerAudioController
	Inherits AbstractPlanePlayerComponent

	' Token: 0x060040E6 RID: 16614 RVA: 0x00234FB5 File Offset: 0x002333B5
	Protected Overrides Sub OnAwake()
		MyBase.OnAwake()
	End Sub

	' Token: 0x060040E7 RID: 16615 RVA: 0x00234FBD File Offset: 0x002333BD
	Private Sub Start()
		AddHandler MyBase.player.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
		AudioManager.PlayLoop("player_plane_engine")
	End Sub

	' Token: 0x060040E8 RID: 16616 RVA: 0x00234FE5 File Offset: 0x002333E5
	Public Sub LevelInit()
	End Sub

	' Token: 0x060040E9 RID: 16617 RVA: 0x00234FE7 File Offset: 0x002333E7
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		If info.damage > 0F Then
			AudioManager.Play("player_plane_hit")
			If MyBase.player.stats.Health > 0 Then
				MyBase.StartCoroutine(Me.change_pitch_cr())
			End If
		End If
	End Sub

	' Token: 0x060040EA RID: 16618 RVA: 0x00235028 File Offset: 0x00233428
	Private Iterator Function change_pitch_cr() As IEnumerator
		If MyBase.player.stats.Health = 1 Then
			AudioManager.Play("player_damage_crack_level4")
			Me.emitAudioFromObject.Add("player_damage_crack_level4")
			AudioManager.ChangeSFXPitch("player_plane_engine", 0.3F, 0.4F)
		ElseIf MyBase.player.stats.Health = 2 Then
			AudioManager.Play("player_damage_crack_level3")
			Me.emitAudioFromObject.Add("player_damage_crack_level3")
			AudioManager.ChangeSFXPitch("player_plane_engine", 0.35F, 0.4F)
		ElseIf MyBase.player.stats.Health = 3 Then
			AudioManager.Play("player_damage_crack_level2")
			Me.emitAudioFromObject.Add("player_damage_crack_level2")
			AudioManager.ChangeSFXPitch("player_plane_engine", 0.4F, 0.4F)
		Else
			AudioManager.Play("player_damage_crack_level1")
			Me.emitAudioFromObject.Add("player_damage_crack_level1")
			AudioManager.ChangeSFXPitch("player_plane_engine", 0.5F, 0.4F)
		End If
		Yield CupheadTime.WaitForSeconds(Me, 1F)
		AudioManager.ChangeSFXPitch("player_plane_engine", 1F, 1F)
		Yield Nothing
		Return
	End Function
End Class
