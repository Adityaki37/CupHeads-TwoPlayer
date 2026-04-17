Imports System

' Token: 0x020008C2 RID: 2242
Public Class HarbourPlatformingLevelBarnacle
	Inherits PlatformingLevelShootingEnemy

	' Token: 0x06003466 RID: 13414 RVA: 0x001E6FC5 File Offset: 0x001E53C5
	Private Sub AttackSFX()
		AudioManager.Play("harbour_barnacle_attack")
		Me.emitAudioFromObject.Add("harbour_barnacle_attack")
	End Sub

	' Token: 0x06003467 RID: 13415 RVA: 0x001E6FE1 File Offset: 0x001E53E1
	Protected Overrides Sub Die()
		AudioManager.Play("harbour_barnacle_death")
		Me.emitAudioFromObject.Add("harbour_barnacle_death")
		MyBase.Die()
	End Sub
End Class
