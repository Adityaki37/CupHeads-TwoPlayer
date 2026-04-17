Imports System
Imports System.Collections.Generic

' Token: 0x02000A4B RID: 2635
Public Class MeterScoreTracker
	' Token: 0x06003EC3 RID: 16067 RVA: 0x0022668B File Offset: 0x00224A8B
	Public Sub New(type As MeterScoreTracker.Type)
		Me.type = type
	End Sub

	' Token: 0x06003EC4 RID: 16068 RVA: 0x0022669A File Offset: 0x00224A9A
	Public Sub Add(damageDealer As DamageDealer)
		AddHandler damageDealer.OnDealDamage, AddressOf Me.OnDealDamage
	End Sub

	' Token: 0x06003EC5 RID: 16069 RVA: 0x002266AE File Offset: 0x00224AAE
	Public Sub Add(projectile As AbstractProjectile)
		projectile.AddToMeterScoreTracker(Me)
	End Sub

	' Token: 0x06003EC6 RID: 16070 RVA: 0x002266B7 File Offset: 0x00224AB7
	Private Sub OnDealDamage(damage As Single, damageReceiver As DamageReceiver, damageDealer As DamageDealer)
		If Not Me.alreadyAddedScore Then
			Level.ScoringData.superMeterUsed += If((Me.type <> MeterScoreTracker.Type.Super), 1, 5)
			Me.alreadyAddedScore = True
		End If
	End Sub

	' Token: 0x040045CD RID: 17869
	Private type As MeterScoreTracker.Type

	' Token: 0x040045CE RID: 17870
	Private alreadyAddedScore As Boolean

	' Token: 0x040045CF RID: 17871
	Private projectilesToAdd As List(Of AbstractProjectile)

	' Token: 0x02000A4C RID: 2636
	Public Enum Type
		' Token: 0x040045D1 RID: 17873
		Super
		' Token: 0x040045D2 RID: 17874
		Ex
	End Enum
End Class
