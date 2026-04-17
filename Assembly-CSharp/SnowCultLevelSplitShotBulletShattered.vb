Imports System
Imports UnityEngine

' Token: 0x020007F9 RID: 2041
Public Class SnowCultLevelSplitShotBulletShattered
	Inherits BasicProjectile

	' Token: 0x06002EEB RID: 12011 RVA: 0x001BAFE8 File Offset: 0x001B93E8
	Public Overrides Function Create(position As Vector2, rotation As Single, speed As Single) As BasicProjectile
		Dim snowCultLevelSplitShotBulletShattered As SnowCultLevelSplitShotBulletShattered = TryCast(MyBase.Create(position, rotation, speed), SnowCultLevelSplitShotBulletShattered)
		snowCultLevelSplitShotBulletShattered.animator.Play(If((Not Rand.Bool()), "MoonB", "MoonA"))
		snowCultLevelSplitShotBulletShattered.fxTimer = 0F
		Return snowCultLevelSplitShotBulletShattered
	End Function

	' Token: 0x06002EEC RID: 12012 RVA: 0x001BB034 File Offset: 0x001B9434
	Protected Overrides Sub FixedUpdate()
		MyBase.FixedUpdate()
		Me.fxTimer += CupheadTime.FixedDelta
		If Me.fxTimer > Me.fxDelay Then
			Me.fxTimer -= Me.fxDelay
			Me.trailFX.Create(MyBase.transform.position)
		End If
	End Sub

	' Token: 0x040037A0 RID: 14240
	<SerializeField()>
	Private trailFX As Effect

	' Token: 0x040037A1 RID: 14241
	<SerializeField()>
	Private fxDelay As Single = 0.3F

	' Token: 0x040037A2 RID: 14242
	Private fxTimer As Single
End Class
