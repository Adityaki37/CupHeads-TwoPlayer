Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020008CE RID: 2254
Public Class HarbourPlatformingLevelOctoProjectile
	Inherits AbstractProjectile

	' Token: 0x060034B6 RID: 13494 RVA: 0x001EA34E File Offset: 0x001E874E
	Protected Overrides Sub Start()
		MyBase.Start()
		Me.velocity.y = Me.speedY
		Me.velocity.x = Me.speedX
		MyBase.StartCoroutine(Me.move_cr())
	End Sub

	' Token: 0x060034B7 RID: 13495 RVA: 0x001EA388 File Offset: 0x001E8788
	Private Iterator Function move_cr() As IEnumerator
		While True
			MyBase.transform.AddPosition(Me.velocity.x * CupheadTime.Delta, Me.velocity.y * CupheadTime.Delta, 0F)
			Me.velocity.y = Me.velocity.y - Me.gravity * CupheadTime.Delta
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x04003CE1 RID: 15585
	<SerializeField()>
	Private speedX As Single

	' Token: 0x04003CE2 RID: 15586
	<SerializeField()>
	Private speedY As Single

	' Token: 0x04003CE3 RID: 15587
	<SerializeField()>
	Private gravity As Single

	' Token: 0x04003CE4 RID: 15588
	Private velocity As Vector2
End Class
