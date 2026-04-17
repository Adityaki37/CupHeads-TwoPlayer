Imports System
Imports UnityEngine

' Token: 0x02000880 RID: 2176
Public Class ForestPlatformingLevelLobber
	Inherits PlatformingLevelShootingEnemy

	' Token: 0x06003283 RID: 12931 RVA: 0x001D663E File Offset: 0x001D4A3E
	Protected Overrides Sub Awake()
		MyBase.Awake()
		MyBase.animator.Play("Idle", 0, Global.UnityEngine.Random.Range(0F, 1F))
	End Sub

	' Token: 0x06003284 RID: 12932 RVA: 0x001D6666 File Offset: 0x001D4A66
	Protected Overrides Sub Shoot()
		MyBase.Shoot()
	End Sub

	' Token: 0x06003285 RID: 12933 RVA: 0x001D666E File Offset: 0x001D4A6E
	Private Sub PlayLobberSound()
		AudioManager.Play("level_forestlobber_shoot")
		Me.emitAudioFromObject.Add("level_forestlobber_shoot")
	End Sub

	' Token: 0x06003286 RID: 12934 RVA: 0x001D668A File Offset: 0x001D4A8A
	Protected Overrides Sub Die()
		AudioManager.Play("level_mermaid_turtle_shell_pop")
		Me.emitAudioFromObject.Add("level_mermaid_turtle_shell_pop")
		MyBase.FrameDelayedCallback(AddressOf Me.Kill, 1)
	End Sub

	' Token: 0x06003287 RID: 12935 RVA: 0x001D66BA File Offset: 0x001D4ABA
	Private Sub Kill()
		MyBase.Die()
	End Sub

	' Token: 0x06003288 RID: 12936 RVA: 0x001D66C2 File Offset: 0x001D4AC2
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me._shootEffect = Nothing
	End Sub
End Class
