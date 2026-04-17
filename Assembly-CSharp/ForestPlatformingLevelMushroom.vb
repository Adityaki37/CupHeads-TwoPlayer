Imports System

' Token: 0x02000883 RID: 2179
Public Class ForestPlatformingLevelMushroom
	Inherits PlatformingLevelShootingEnemy

	' Token: 0x06003296 RID: 12950 RVA: 0x001D68D5 File Offset: 0x001D4CD5
	Protected Overrides Sub Awake()
		MyBase.Awake()
		ForestPlatformingLevelMushroomProjectile.numUntilPink = MyBase.Properties.MushroomPinkNumber.RandomInt()
	End Sub

	' Token: 0x06003297 RID: 12951 RVA: 0x001D68F2 File Offset: 0x001D4CF2
	Protected Overrides Sub Shoot()
		MyBase.Shoot()
	End Sub

	' Token: 0x06003298 RID: 12952 RVA: 0x001D68FC File Offset: 0x001D4CFC
	Private Sub EmergeFromGround()
		MyBase.setDirection(If((Me._target.center.x <= MyBase.transform.position.x), PlatformingLevelShootingEnemy.Direction.Left, PlatformingLevelShootingEnemy.Direction.Right))
	End Sub

	' Token: 0x06003299 RID: 12953 RVA: 0x001D6941 File Offset: 0x001D4D41
	Private Sub PlayMushroomSound()
		AudioManager.Play("level_mushroom_shoot")
		Me.emitAudioFromObject.Add("level_mushroom_shoot")
	End Sub

	' Token: 0x0600329A RID: 12954 RVA: 0x001D695D File Offset: 0x001D4D5D
	Protected Overrides Sub Die()
		AudioManager.Play("level_mermaid_turtle_shell_pop")
		Me.emitAudioFromObject.Add("level_mermaid_turtle_shell_pop")
		MyBase.Die()
	End Sub
End Class
