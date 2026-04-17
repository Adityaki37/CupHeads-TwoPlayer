Imports System
Imports UnityEngine

' Token: 0x02000817 RID: 2071
Public Class TrainLevelEngineCar
	Inherits AbstractPausableComponent

	' Token: 0x06003006 RID: 12294 RVA: 0x001C62B0 File Offset: 0x001C46B0
	Public Sub PlayRage()
		AudioManager.Play("train_engine_car_rage_loop")
		Me.emitAudioFromObject.Add("train_engine_car_rage_loop")
		MyBase.animator.Play("Rage")
	End Sub

	' Token: 0x06003007 RID: 12295 RVA: 0x001C62DC File Offset: 0x001C46DC
	Public Sub [End]()
		MyBase.animator.Play("Idle")
	End Sub

	' Token: 0x06003008 RID: 12296 RVA: 0x001C62EE File Offset: 0x001C46EE
	Private Sub SteamEffect()
		Me.steamEffect.Create(Me.steamRoot.position)
	End Sub

	' Token: 0x06003009 RID: 12297 RVA: 0x001C6307 File Offset: 0x001C4707
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me.steamEffect = Nothing
	End Sub

	' Token: 0x040038DD RID: 14557
	<SerializeField()>
	Private steamRoot As Transform

	' Token: 0x040038DE RID: 14558
	<SerializeField()>
	Private steamEffect As Effect
End Class
