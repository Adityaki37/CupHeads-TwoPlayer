Imports System
Imports UnityEngine

' Token: 0x02000AE6 RID: 2790
<RequireComponent(GetType(Rigidbody2D))>
<RequireComponent(GetType(DamageReceiver))>
Public Class BasicDamagableProjectile
	Inherits BasicProjectile

	' Token: 0x06004391 RID: 17297 RVA: 0x0023FF08 File Offset: 0x0023E308
	Public Overridable Function Create(position As Vector2, rotation As Single, speed As Single, health As Single) As BasicDamagableProjectile
		Dim basicDamagableProjectile As BasicDamagableProjectile = TryCast(Me.Create(position, rotation, speed), BasicDamagableProjectile)
		basicDamagableProjectile.health = health
		Return basicDamagableProjectile
	End Function

	' Token: 0x06004392 RID: 17298 RVA: 0x0023FF30 File Offset: 0x0023E330
	Public Overridable Function Create(position As Vector2, rotation As Single, scale As Vector2, speed As Single, health As Single) As BasicDamagableProjectile
		Dim basicDamagableProjectile As BasicDamagableProjectile = TryCast(Me.Create(position, rotation, scale, speed), BasicDamagableProjectile)
		basicDamagableProjectile.health = health
		Return basicDamagableProjectile
	End Function

	' Token: 0x06004393 RID: 17299 RVA: 0x0023FF57 File Offset: 0x0023E357
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.damageReceiver = MyBase.GetComponent(Of DamageReceiver)()
		AddHandler Me.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
	End Sub

	' Token: 0x06004394 RID: 17300 RVA: 0x0023FF82 File Offset: 0x0023E382
	Protected Overrides Sub OnDestroy()
		RemoveHandler Me.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
		MyBase.OnDestroy()
	End Sub

	' Token: 0x06004395 RID: 17301 RVA: 0x0023FFA1 File Offset: 0x0023E3A1
	Protected Overrides Sub Update()
		MyBase.Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x06004396 RID: 17302 RVA: 0x0023FFBF File Offset: 0x0023E3BF
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		Me.health -= info.damage
		If Me.health <= 0F Then
			Me.Die()
		End If
	End Sub

	' Token: 0x04004959 RID: 18777
	Public health As Single = 10F

	' Token: 0x0400495A RID: 18778
	Private damageReceiver As DamageReceiver
End Class
