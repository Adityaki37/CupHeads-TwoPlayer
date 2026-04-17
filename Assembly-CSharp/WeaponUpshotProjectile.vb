Imports System
Imports UnityEngine

' Token: 0x02000A8B RID: 2699
Public Class WeaponUpshotProjectile
	Inherits AbstractProjectile

	' Token: 0x1700059A RID: 1434
	' (get) Token: 0x06004086 RID: 16518 RVA: 0x00232043 File Offset: 0x00230443
	Protected Overrides ReadOnly Property DestroyedAfterLeavingScreen As Boolean
		Get
			Return True
		End Get
	End Property

	' Token: 0x06004087 RID: 16519 RVA: 0x00232048 File Offset: 0x00230448
	Protected Overrides Sub Start()
		MyBase.Start()
		Me.damageDealer.isDLCWeapon = True
		Dim player As AbstractPlayerController = PlayerManager.GetPlayer(Me.PlayerId)
		Me.onLeft = player.transform.localScale.x < 0F
		Me.startAngle = MyBase.transform.eulerAngles.z
		Me.SetAngle()
	End Sub

	' Token: 0x06004088 RID: 16520 RVA: 0x002320B2 File Offset: 0x002304B2
	Protected Overrides Sub FixedUpdate()
		MyBase.FixedUpdate()
		Me.UpdateSpeed()
		Me.Move()
	End Sub

	' Token: 0x06004089 RID: 16521 RVA: 0x002320C8 File Offset: 0x002304C8
	Private Sub UpdateSpeed()
		If Me.time < Me.timeToArc Then
			Me.time += CupheadTime.FixedDelta
			Me.ySpeed = Me.ySpeedMinMax.GetFloatAt(Me.time / Me.timeToArc)
			Me.ySpeed = If((Not Me.onLeft), Me.ySpeed, (-Me.ySpeed))
			Me.SetAngle()
		End If
	End Sub

	' Token: 0x0600408A RID: 16522 RVA: 0x00232140 File Offset: 0x00230540
	Private Sub Move()
		If MyBase.dead Then
			Return
		End If
		Dim vector As Vector3 = New Vector3(Me.xSpeed, Me.ySpeed)
		Dim quaternion As Quaternion = Quaternion.Euler(0F, 0F, Me.startAngle)
		vector = quaternion * vector
		MyBase.transform.position += vector * CupheadTime.FixedDelta
	End Sub

	' Token: 0x0600408B RID: 16523 RVA: 0x002321AC File Offset: 0x002305AC
	Private Sub SetAngle()
		Dim num As Integer = Mathf.RoundToInt(Me.startAngle)
		If num <> 0 Then
			If num = 45 Then
				MyBase.transform.SetEulerAngles(Nothing, Nothing, New Single?(Me.time * 2F / Me.timeToArc * 45F))
				Return
			End If
			If num = 90 Then
				MyBase.transform.SetEulerAngles(Nothing, Nothing, New Single?(CSng(If((Not Me.onLeft), 45, (-225))) + Me.time * 2F / Me.timeToArc * CSng(If((Not Me.onLeft), 45, (-45)))))
				Return
			End If
			If num = 135 Then
				MyBase.transform.SetEulerAngles(Nothing, Nothing, New Single?(180F + Me.time * 2F / Me.timeToArc * -45F))
				Return
			End If
			If num <> 180 Then
				If num = 225 Then
					MyBase.transform.SetEulerAngles(Nothing, Nothing, New Single?(270F + Me.time * 2F / Me.timeToArc * -45F))
					Return
				End If
				If num = 270 Then
					MyBase.transform.SetEulerAngles(Nothing, Nothing, New Single?(CSng(If((Not Me.onLeft), 225, (-45))) + Me.time * 2F / Me.timeToArc * CSng(If((Not Me.onLeft), 45, (-45)))))
					Return
				End If
				If num <> 315 Then
					Return
				End If
				MyBase.transform.SetEulerAngles(Nothing, Nothing, New Single?(270F + Me.time * 2F / Me.timeToArc * 45F))
				Return
			End If
		End If
		MyBase.transform.SetEulerAngles(Nothing, Nothing, New Single?(CSng(If((Not Me.onLeft), (-45), 225)) + Me.time * 2F / Me.timeToArc * CSng(If((Not Me.onLeft), 45, (-45)))))
	End Sub

	' Token: 0x0600408C RID: 16524 RVA: 0x00232464 File Offset: 0x00230864
	Protected Overrides Sub OnCollisionEnemy(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionEnemy(hit, phase)
		If phase = CollisionPhase.Enter Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x0600408D RID: 16525 RVA: 0x00232484 File Offset: 0x00230884
	Protected Overrides Sub OnCollisionDie(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionDie(hit, phase)
		If MyBase.tag = "PlayerProjectile" AndAlso phase = CollisionPhase.Enter Then
			If hit.GetComponent(Of DamageReceiver)() AndAlso hit.GetComponent(Of DamageReceiver)().enabled Then
				AudioManager.Play("player_shoot_hit_cuphead")
			Else
				AudioManager.Play("player_weapon_peashot_miss")
			End If
		End If
	End Sub

	' Token: 0x0600408E RID: 16526 RVA: 0x002324ED File Offset: 0x002308ED
	Protected Overrides Sub Die()
		MyBase.Die()
		Me.StopAllCoroutines()
	End Sub

	' Token: 0x0400474A RID: 18250
	Public ySpeedMinMax As MinMax

	' Token: 0x0400474B RID: 18251
	Public timeToArc As Single

	' Token: 0x0400474C RID: 18252
	Public xSpeed As Single

	' Token: 0x0400474D RID: 18253
	Private ySpeed As Single

	' Token: 0x0400474E RID: 18254
	Private time As Single

	' Token: 0x0400474F RID: 18255
	Private onLeft As Boolean

	' Token: 0x04004750 RID: 18256
	Private startAngle As Single
End Class
