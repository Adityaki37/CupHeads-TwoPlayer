Imports System
Imports System.Collections.Generic
Imports UnityEngine

' Token: 0x02000A75 RID: 2677
Public Class WeaponCrackshotProjectile
	Inherits BasicProjectile

	' Token: 0x06003FF6 RID: 16374 RVA: 0x0022E52C File Offset: 0x0022C92C
	Protected Overrides Sub Start()
		MyBase.Start()
		MyBase.animator.Play(Me.[variant].ToString(), 0, Global.UnityEngine.Random.Range(0F, 1F))
		Me.damageDealer.isDLCWeapon = True
		AudioManager.Play("player_weapon_crackshot_shoot")
		Me.emitAudioFromObject.Add("player_weapon_crackshot_shoot")
	End Sub

	' Token: 0x06003FF7 RID: 16375 RVA: 0x0022E591 File Offset: 0x0022C991
	Protected Overrides Sub OnDieDistance()
		If MyBase.dead Then
			Return
		End If
		Me.Die()
		MyBase.animator.SetTrigger("OnDistanceDie")
	End Sub

	' Token: 0x06003FF8 RID: 16376 RVA: 0x0022E5B8 File Offset: 0x0022C9B8
	Protected Overrides Sub Die()
		Me.move = False
		MyBase.Die()
		Me.coll.enabled = False
		If MyBase.animator.GetCurrentAnimatorStateInfo(0).IsTag("Comet") Then
			MyBase.animator.Play(If((Not Rand.Bool()), "ImpactCometB", "ImpactCometA"))
		Else
			MyBase.animator.Play(If((Not Rand.Bool()), "ImpactSmallB", "ImpactSmallA"))
		End If
	End Sub

	' Token: 0x06003FF9 RID: 16377 RVA: 0x0022E649 File Offset: 0x0022CA49
	Private Sub _OnDieAnimComplete()
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub

	' Token: 0x06003FFA RID: 16378 RVA: 0x0022E658 File Offset: 0x0022CA58
	Protected Overrides Sub FixedUpdate()
		MyBase.FixedUpdate()
		If Not Me.cracked AndAlso MyBase.distance > WeaponProperties.LevelWeaponCrackshot.Basic.crackDistance AndAlso Not MyBase.dead Then
			Me.cracked = True
			Me.crackFX.Create(MyBase.transform.position)
			MyBase.animator.SetBool("IsB", Me.useBComet)
			MyBase.animator.Play(If((Not Rand.Bool()), "CometStartA", "CometStartB"))
			AudioManager.Play("player_weapon_crackshot_shootfast")
			Me.emitAudioFromObject.Add("player_weapon_crackshot_shootfast")
			Me.damageDealer.SetDamage(WeaponProperties.LevelWeaponCrackshot.Basic.crackedDamage)
			Me.Speed = WeaponProperties.LevelWeaponCrackshot.Basic.crackedSpeed
			Me.FindTarget()
			If Me.target IsNot Nothing Then
				If Vector3.Angle(MyBase.transform.right, Me.target.bounds.center - MyBase.transform.position) > Me.maxAngleRange Then
					If Mathf.Abs(MyBase.transform.right.y) < 0.05F Then
						MyBase.transform.eulerAngles = New Vector3(0F, 0F, If((MyBase.transform.eulerAngles.z <= 90F), (MathUtils.DirectionToAngle(MyBase.transform.right) + Me.maxAngleRange), (MathUtils.DirectionToAngle(MyBase.transform.right) - Me.maxAngleRange)))
					Else
						MyBase.transform.eulerAngles = New Vector3(0F, 0F, MathUtils.DirectionToAngle(MyBase.transform.right) + Me.maxAngleRange)
					End If
				Else
					MyBase.transform.eulerAngles = New Vector3(0F, 0F, MathUtils.DirectionToAngle(Me.target.bounds.center - MyBase.transform.position))
				End If
			End If
		End If
	End Sub

	' Token: 0x06003FFB RID: 16379 RVA: 0x0022E890 File Offset: 0x0022CC90
	Public Sub FindTarget()
		Me.target = Me.findBestTarget(AbstractProjectile.FindOverlapScreenDamageReceivers())
	End Sub

	' Token: 0x06003FFC RID: 16380 RVA: 0x0022E8A4 File Offset: 0x0022CCA4
	Private Function findBestTarget(damageReceivers As IEnumerable(Of DamageReceiver)) As Collider2D
		Dim num As Single = Single.MaxValue
		Dim num2 As Single = Single.MaxValue
		Dim collider2D As Collider2D = Nothing
		Dim collider2D2 As Collider2D = Nothing
		Dim vector As Vector2 = MyBase.transform.position
		For Each damageReceiver As DamageReceiver In damageReceivers
			If damageReceiver.gameObject.activeInHierarchy AndAlso damageReceiver.enabled AndAlso damageReceiver.type = DamageReceiver.Type.Enemy Then
				For Each collider2D3 As Collider2D In damageReceiver.GetComponents(Of Collider2D)()
					If collider2D3.isActiveAndEnabled AndAlso CupheadLevelCamera.Current.ContainsPoint(collider2D3.bounds.center, collider2D3.bounds.size / 2F) Then
						Dim sqrMagnitude As Single = (vector - collider2D3.bounds.center).sqrMagnitude
						If sqrMagnitude < num2 Then
							num2 = sqrMagnitude
							collider2D2 = collider2D3
						End If
						If sqrMagnitude < num AndAlso Vector3.Angle(MyBase.transform.right, collider2D3.bounds.center - vector) < Me.maxAngleRange Then
							num = sqrMagnitude
							collider2D = collider2D3
						End If
					End If
				Next
				For Each damageReceiverChild As DamageReceiverChild In damageReceiver.GetComponentsInChildren(Of DamageReceiverChild)()
					For Each collider2D4 As Collider2D In damageReceiverChild.GetComponents(Of Collider2D)()
						If collider2D4.isActiveAndEnabled AndAlso CupheadLevelCamera.Current.ContainsPoint(collider2D4.bounds.center, collider2D4.bounds.size / 2F) Then
							Dim sqrMagnitude2 As Single = (vector - collider2D4.bounds.center).sqrMagnitude
							If sqrMagnitude2 < num2 Then
								num2 = sqrMagnitude2
								collider2D2 = collider2D4
							End If
							If sqrMagnitude2 < num AndAlso Vector3.Angle(MyBase.transform.right, collider2D4.bounds.center - vector) < Me.maxAngleRange Then
								num = sqrMagnitude2
								collider2D = collider2D4
							End If
						End If
					Next
				Next
			End If
		Next
		Return If((Not(collider2D Is Nothing)), collider2D, collider2D2)
	End Function

	' Token: 0x040046C6 RID: 18118
	Private target As Collider2D

	' Token: 0x040046C7 RID: 18119
	Private cracked As Boolean

	' Token: 0x040046C8 RID: 18120
	Public maxAngleRange As Single

	' Token: 0x040046C9 RID: 18121
	Public [variant] As Integer

	' Token: 0x040046CA RID: 18122
	Public useBComet As Boolean

	' Token: 0x040046CB RID: 18123
	<SerializeField()>
	Private coll As Collider2D

	' Token: 0x040046CC RID: 18124
	<SerializeField()>
	Private crackFX As Effect
End Class
