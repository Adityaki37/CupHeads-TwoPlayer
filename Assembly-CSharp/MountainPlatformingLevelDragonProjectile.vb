Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020008DF RID: 2271
Public Class MountainPlatformingLevelDragonProjectile
	Inherits BasicProjectile

	' Token: 0x1700044F RID: 1103
	' (get) Token: 0x06003527 RID: 13607 RVA: 0x001EEF97 File Offset: 0x001ED397
	Protected Overrides ReadOnly Property DestroyLifetime As Single
		Get
			Return -1F
		End Get
	End Property

	' Token: 0x06003528 RID: 13608 RVA: 0x001EEFA0 File Offset: 0x001ED3A0
	Protected Overrides Sub Start()
		MyBase.Start()
		MountainPlatformingLevelDragonProjectile.numUntilPink -= 1
		Me.DestroyDistance = -1F
		If MountainPlatformingLevelDragonProjectile.numUntilPink <= 0 Then
			MountainPlatformingLevelDragonProjectile.numUntilPink = EnemyDatabase.GetProperties(EnemyID.dragon).MushroomPinkNumber.RandomInt()
			Me.SetParryable(True)
		Else
			Me.SetParryable(False)
		End If
		MyBase.StartCoroutine(Me.trail_cr())
	End Sub

	' Token: 0x06003529 RID: 13609 RVA: 0x001EF010 File Offset: 0x001ED410
	Private Iterator Function trail_cr() As IEnumerator
		While Not MyBase.dead
			Yield CupheadTime.WaitForSeconds(Me, Me.trailPeriod.RandomFloat())
			Dim effect As Effect = Me.trailPrefab.Create(Me.trailRoot.position + Me.trailMaxOffset * MathUtils.RandomPointInUnitCircle())
			effect.animator.Play("PuffA")
		End While
		Return
	End Function

	' Token: 0x0600352A RID: 13610 RVA: 0x001EF02B File Offset: 0x001ED42B
	Public Overrides Sub OnParryDie()
		MyBase.OnParryDie()
		Global.UnityEngine.[Object].Destroy(Me)
	End Sub

	' Token: 0x04003D4E RID: 15694
	Public Shared numUntilPink As Integer

	' Token: 0x04003D4F RID: 15695
	<SerializeField()>
	Private trailPrefab As Effect

	' Token: 0x04003D50 RID: 15696
	<SerializeField()>
	Private trailPeriod As MinMax

	' Token: 0x04003D51 RID: 15697
	<SerializeField()>
	Private trailMaxOffset As Single

	' Token: 0x04003D52 RID: 15698
	<SerializeField()>
	Private trailRoot As Transform
End Class
