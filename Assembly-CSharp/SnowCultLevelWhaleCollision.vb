Imports System
Imports UnityEngine

' Token: 0x020007FB RID: 2043
Public Class SnowCultLevelWhaleCollision
	Inherits AbstractCollidableObject

	' Token: 0x06002EF5 RID: 12021 RVA: 0x001BB3BF File Offset: 0x001B97BF
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		If phase <> CollisionPhase.[Exit] Then
			Me.wiz.PlayerHitByWhale(hit, phase)
		End If
	End Sub

	' Token: 0x040037AD RID: 14253
	<SerializeField()>
	Private wiz As SnowCultLevelWizard
End Class
