Imports System
Imports UnityEngine

' Token: 0x02000A53 RID: 2643
Public Class PlayerSuperChaliceIIIMinion
	Inherits BasicProjectileContinuesOnLevelEnd

	' Token: 0x06003EFE RID: 16126 RVA: 0x0022868B File Offset: 0x00226A8B
	Protected Overrides Sub OnDieLifetime()
	End Sub

	' Token: 0x06003EFF RID: 16127 RVA: 0x0022868D File Offset: 0x00226A8D
	Protected Overrides Sub OnDieDistance()
	End Sub

	' Token: 0x06003F00 RID: 16128 RVA: 0x00228690 File Offset: 0x00226A90
	Protected Overrides Sub Start()
		MyBase.Start()
		Me.startY = MyBase.transform.position.y
		Me.t = Global.UnityEngine.Random.Range(0F, 6.2831855F)
		Me.wavelength = Global.UnityEngine.Random.Range(150F, 300F)
		Me.damageDealer.SetDamageSource(DamageDealer.DamageSource.Super)
		Dim meterScoreTracker As MeterScoreTracker = New MeterScoreTracker(MeterScoreTracker.Type.Super)
		meterScoreTracker.Add(Me.damageDealer)
	End Sub

	' Token: 0x06003F01 RID: 16129 RVA: 0x00228708 File Offset: 0x00226B08
	Protected Overrides Sub OnDealDamage(damage As Single, receiver As DamageReceiver, damageDealer As DamageDealer)
		MyBase.OnDealDamage(damage, receiver, damageDealer)
		Me.impactFX.Create(Vector3.Lerp(MyBase.transform.position, receiver.transform.position, Global.UnityEngine.Random.Range(0F, 1F)) + Global.UnityEngine.Random.insideUnitSphere * 25F)
		AudioManager.Play("player_super_chalice_barrage_impact")
	End Sub

	' Token: 0x06003F02 RID: 16130 RVA: 0x00228774 File Offset: 0x00226B74
	Protected Overrides Sub Move()
		MyBase.transform.position += Me.Direction * Me.Speed * CupheadTime.FixedDelta
		If Me.wave Then
			Me.t += Me.Speed * CupheadTime.FixedDelta
			MyBase.transform.position = New Vector3(MyBase.transform.position.x, Me.startY + Mathf.Sin(Me.t / Me.wavelength) * Me.amplitude)
		End If
	End Sub

	' Token: 0x04004616 RID: 17942
	Public elementIndex As Integer

	' Token: 0x04004617 RID: 17943
	Private wavelength As Single = 180F

	' Token: 0x04004618 RID: 17944
	Private amplitude As Single = 20F

	' Token: 0x04004619 RID: 17945
	Private t As Single

	' Token: 0x0400461A RID: 17946
	Private startY As Single

	' Token: 0x0400461B RID: 17947
	Public wave As Boolean

	' Token: 0x0400461C RID: 17948
	<SerializeField()>
	Private impactFX As Effect
End Class
