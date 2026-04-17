Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000717 RID: 1815
Public Class OldManLevelTurretProjectile
	Inherits BasicProjectile

	' Token: 0x0600277E RID: 10110 RVA: 0x0017287C File Offset: 0x00170C7C
	Protected Overrides Sub Start()
		MyBase.Start()
		Me.rend.flipX = Rand.Bool()
		MyBase.animator.Play("Projectile", 0, Global.UnityEngine.Random.Range(0F, 1F))
		MyBase.StartCoroutine(Me.spawn_sparkles_cr())
	End Sub

	' Token: 0x0600277F RID: 10111 RVA: 0x001728CC File Offset: 0x00170CCC
	Private Iterator Function spawn_sparkles_cr() As IEnumerator
		Me.sparkleAngle = CSng(Global.UnityEngine.Random.Range(0, 360))
		While True
			Yield CupheadTime.WaitForSeconds(Me, Me.sparkleSpawnDelay)
			CType(Level.Current, OldManLevel).CreateFX(MyBase.transform.position + MathUtils.AngleToDirection(Me.sparkleAngle) * Me.sparkleDistanceRange.RandomFloat(), True, MyBase.CanParry)
			Me.sparkleAngle = (Me.sparkleAngle + Me.sparkleAngleShiftRange.RandomFloat()) Mod 360F
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06002780 RID: 10112 RVA: 0x001728E8 File Offset: 0x00170CE8
	Protected Overrides Sub Move()
		If Me.Speed = 0F Then
		End If
		MyBase.transform.position += MyBase.transform.up * Me.Speed * CupheadTime.FixedDelta
	End Sub

	' Token: 0x0400303D RID: 12349
	<SerializeField()>
	Private rend As SpriteRenderer

	' Token: 0x0400303E RID: 12350
	<SerializeField()>
	Private sparkleSpawnDelay As Single

	' Token: 0x0400303F RID: 12351
	<SerializeField()>
	Private sparkleAngleShiftRange As MinMax

	' Token: 0x04003040 RID: 12352
	<SerializeField()>
	Private sparkleDistanceRange As MinMax

	' Token: 0x04003041 RID: 12353
	Private sparkleAngle As Single
End Class
