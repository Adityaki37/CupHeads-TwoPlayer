Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000884 RID: 2180
Public Class ForestPlatformingLevelMushroomProjectile
	Inherits BasicProjectile

	' Token: 0x1700043C RID: 1084
	' (get) Token: 0x0600329C RID: 12956 RVA: 0x001D6987 File Offset: 0x001D4D87
	Protected Overrides ReadOnly Property DestroyLifetime As Single
		Get
			Return -1F
		End Get
	End Property

	' Token: 0x0600329D RID: 12957 RVA: 0x001D6990 File Offset: 0x001D4D90
	Protected Overrides Sub Start()
		MyBase.Start()
		ForestPlatformingLevelMushroomProjectile.numUntilPink -= 1
		Me.DestroyDistance = -1F
		If ForestPlatformingLevelMushroomProjectile.numUntilPink = 0 Then
			ForestPlatformingLevelMushroomProjectile.numUntilPink = EnemyDatabase.GetProperties(EnemyID.mushroom).MushroomPinkNumber.RandomInt()
			Me.SetInt(AbstractProjectile.[Variant], 1)
			Me.SetParryable(True)
		Else
			Me.SetInt(AbstractProjectile.[Variant], 0)
			Me.SetParryable(False)
		End If
		MyBase.StartCoroutine(Me.trail_cr())
	End Sub

	' Token: 0x0600329E RID: 12958 RVA: 0x001D6A18 File Offset: 0x001D4E18
	Private Iterator Function trail_cr() As IEnumerator
		While Not MyBase.dead
			Yield CupheadTime.WaitForSeconds(Me, Me.trailPeriod.RandomFloat())
			Me.trailPrefab.Create(Me.trailRoot.position + Me.trailMaxOffset * MathUtils.RandomPointInUnitCircle())
		End While
		Return
	End Function

	' Token: 0x0600329F RID: 12959 RVA: 0x001D6A33 File Offset: 0x001D4E33
	Public Overrides Sub OnParryDie()
		MyBase.OnParryDie()
		Global.UnityEngine.[Object].Destroy(Me)
	End Sub

	' Token: 0x04003AE0 RID: 15072
	Public Shared numUntilPink As Integer

	' Token: 0x04003AE1 RID: 15073
	<SerializeField()>
	Private trailPrefab As Effect

	' Token: 0x04003AE2 RID: 15074
	<SerializeField()>
	Private trailPeriod As MinMax

	' Token: 0x04003AE3 RID: 15075
	<SerializeField()>
	Private trailMaxOffset As Single

	' Token: 0x04003AE4 RID: 15076
	<SerializeField()>
	Private trailRoot As Transform
End Class
