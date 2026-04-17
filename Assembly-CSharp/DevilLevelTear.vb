Imports System
Imports UnityEngine

' Token: 0x02000599 RID: 1433
Public Class DevilLevelTear
	Inherits AbstractProjectile

	' Token: 0x06001B78 RID: 7032 RVA: 0x000FB1D8 File Offset: 0x000F95D8
	Public Function CreateTear(position As Vector2, speed As Single) As DevilLevelTear
		Dim devilLevelTear As DevilLevelTear = Me.InstantiatePrefab(Of DevilLevelTear)()
		devilLevelTear.transform.position = position
		devilLevelTear.speed = speed
		devilLevelTear.animator.Play("Drop_" + Global.UnityEngine.Random.Range(1, 7).ToStringInvariant())
		Return devilLevelTear
	End Function

	' Token: 0x06001B79 RID: 7033 RVA: 0x000FB226 File Offset: 0x000F9626
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
		MyBase.OnCollisionPlayer(hit, phase)
	End Sub

	' Token: 0x06001B7A RID: 7034 RVA: 0x000FB244 File Offset: 0x000F9644
	Protected Overrides Sub FixedUpdate()
		MyBase.FixedUpdate()
		If MyBase.dead Then
			Return
		End If
		MyBase.transform.AddPosition(0F, -Me.speed * CupheadTime.FixedDelta, 0F)
	End Sub

	' Token: 0x040024A7 RID: 9383
	Private speed As Single
End Class
