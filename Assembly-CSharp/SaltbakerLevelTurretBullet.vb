Imports System
Imports UnityEngine

' Token: 0x020007DA RID: 2010
Public Class SaltbakerLevelTurretBullet
	Inherits BasicProjectile

	' Token: 0x06002DE3 RID: 11747 RVA: 0x001B0F34 File Offset: 0x001AF334
	Public Function Create(pos As Vector3, rotation As Single, speed As Single, parent As SaltbakerLevelSaltbaker) As SaltbakerLevelTurretBullet
		Dim saltbakerLevelTurretBullet As SaltbakerLevelTurretBullet = TryCast(MyBase.Create(pos, rotation, speed), SaltbakerLevelTurretBullet)
		saltbakerLevelTurretBullet.parent = parent
		saltbakerLevelTurretBullet.animator.Play(If((Not Rand.Bool()), "B", "A"))
		saltbakerLevelTurretBullet.animator.Update(0F)
		Return saltbakerLevelTurretBullet
	End Function

	' Token: 0x06002DE4 RID: 11748 RVA: 0x001B0F92 File Offset: 0x001AF392
	Protected Overrides Sub Start()
		MyBase.Start()
		AddHandler Me.parent.OnDeathEvent, AddressOf Me.Die
	End Sub

	' Token: 0x06002DE5 RID: 11749 RVA: 0x001B0FB2 File Offset: 0x001AF3B2
	Protected Overrides Sub OnDestroy()
		RemoveHandler Me.parent.OnDeathEvent, AddressOf Me.Die
		MyBase.OnDestroy()
	End Sub

	' Token: 0x04003667 RID: 13927
	Private parent As SaltbakerLevelSaltbaker
End Class
