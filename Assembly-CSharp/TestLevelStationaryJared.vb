Imports System
Imports UnityEngine

' Token: 0x020004AF RID: 1199
Public Class TestLevelStationaryJared
	Inherits LevelProperties.Test.Entity

	' Token: 0x06001390 RID: 5008 RVA: 0x000AC2AA File Offset: 0x000AA6AA
	Public Overrides Sub LevelInit(properties As LevelProperties.Test)
		MyBase.LevelInit(properties)
	End Sub

	' Token: 0x06001391 RID: 5009 RVA: 0x000AC2B3 File Offset: 0x000AA6B3
	Private Sub Start()
		Me.damageReceiver = MyBase.GetComponent(Of DamageReceiver)()
		AddHandler Me.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
	End Sub

	' Token: 0x06001392 RID: 5010 RVA: 0x000AC2D8 File Offset: 0x000AA6D8
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		AudioManager.Play("test_sound_2")
	End Sub

	' Token: 0x06001393 RID: 5011 RVA: 0x000AC2E4 File Offset: 0x000AA6E4
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
	End Sub

	' Token: 0x06001394 RID: 5012 RVA: 0x000AC2EE File Offset: 0x000AA6EE
	Public Overrides Sub OnParry(player As AbstractPlayerController)
		MyBase.OnParry(player)
		player.stats.OnParry(1F, True)
	End Sub

	' Token: 0x04001C9F RID: 7327
	<SerializeField()>
	Private childSprite As Transform

	' Token: 0x04001CA0 RID: 7328
	Private damageReceiver As DamageReceiver
End Class
