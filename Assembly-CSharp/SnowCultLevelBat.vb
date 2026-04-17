Imports System
Imports UnityEngine

' Token: 0x020007E6 RID: 2022
Public Class SnowCultLevelBat
	Inherits AbstractProjectile

	' Token: 0x06002E47 RID: 11847 RVA: 0x001B48FC File Offset: 0x001B2CFC
	Public Overridable Function Init(startPos As Vector3, launchVel As Vector3, properties As LevelProperties.SnowCult.Snowball, parent As SnowCultLevelYeti, parryable As Boolean, suffix As String) As SnowCultLevelBat
		MyBase.ResetLifetime()
		MyBase.ResetDistance()
		Me.parent = parent
		AddHandler Me.parent.OnDeathEvent, AddressOf Me.Dead
		MyBase.transform.position = startPos
		Me.speed = properties.batAttackSpeed
		Me.readdOnEscape = properties.batsReaddedOnEscape
		Me.moving = False
		Me.launchVelocity = launchVel
		MyBase.transform.localScale = New Vector3(Mathf.Sign(-launchVel.x), 1F)
		Me.Health = properties.batHP
		Me.shotSpeed = properties.batShotSpeed
		Me.animatorSuffix = suffix
		Me.SetParryable(parryable)
		MyBase.animator.Play("Slowdown" + Me.animatorSuffix, 0, Global.UnityEngine.Random.Range(0F, 0.33F))
		Return Me
	End Function

	' Token: 0x06002E48 RID: 11848 RVA: 0x001B49DC File Offset: 0x001B2DDC
	Protected Overrides Sub Start()
		MyBase.Start()
		Me.damageReceiver = MyBase.GetComponent(Of DamageReceiver)()
		AddHandler Me.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
	End Sub

	' Token: 0x06002E49 RID: 11849 RVA: 0x001B4A07 File Offset: 0x001B2E07
	Protected Overrides Sub OnDieLifetime()
	End Sub

	' Token: 0x06002E4A RID: 11850 RVA: 0x001B4A09 File Offset: 0x001B2E09
	Protected Overrides Sub OnDieDistance()
	End Sub

	' Token: 0x06002E4B RID: 11851 RVA: 0x001B4A0B File Offset: 0x001B2E0B
	Public Overrides Sub OnParryDie()
		If Level.Current.mode = Level.Mode.Easy Then
			Me.EasyModeDie()
		Else
			MyBase.OnParryDie()
		End If
	End Sub

	' Token: 0x06002E4C RID: 11852 RVA: 0x001B4A2D File Offset: 0x001B2E2D
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x06002E4D RID: 11853 RVA: 0x001B4A4B File Offset: 0x001B2E4B
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		Me.Health -= info.damage
		If Me.Health < 0F Then
			Level.Current.RegisterMinionKilled()
			Me.Dead()
		End If
	End Sub

	' Token: 0x06002E4E RID: 11854 RVA: 0x001B4A80 File Offset: 0x001B2E80
	Public Sub AttackPlayer(startPos As Vector3, height As Single, width As Single, arc As Single)
		Me.moving = True
		Me.attackStart = startPos
		Me.attackHeight = startPos.y - (CupheadLevelCamera.Current.Bounds.y + 100F) - height
		Me.attackWidth = width
		MyBase.transform.localScale = New Vector3(Mathf.Sign(-Me.attackWidth), 1F)
		Me.attackTime = 0F
		Me.arcModifier = arc
		MyBase.animator.SetFloat("YSpeed", -10F)
		MyBase.animator.Play("Enter" + Me.animatorSuffix)
		Me.spriteRenderer.sortingOrder = 30
		Me.collider.enabled = True
		Me.reachedCircle = True
	End Sub

	' Token: 0x06002E4F RID: 11855 RVA: 0x001B4B50 File Offset: 0x001B2F50
	Protected Overrides Sub FixedUpdate()
		MyBase.FixedUpdate()
		If Not Me.reachedCircle Then
			If MyBase.transform.position.y >= 460F Then
				Me.reachedCircle = True
				Me.collider.enabled = False
			Else
				MyBase.transform.position += Me.launchVelocity * CupheadTime.FixedDelta
				Me.launchVelocity += Vector3.up * 500F * CupheadTime.FixedDelta
			End If
		End If
		If Me.moving Then
			Me.attackTime += CupheadTime.FixedDelta * Me.speed
			If Me.attackTime < 0.9F Then
				Me.lastPos = MyBase.transform.position
				MyBase.transform.position = New Vector3(Mathf.Lerp(Me.attackStart.x, Me.attackStart.x + Me.attackWidth, Me.attackTime), Me.attackStart.y + Mathf.Pow(Mathf.Sin(Me.attackTime * 3.1415927F), Me.arcModifier) * -Me.attackHeight)
				MyBase.animator.SetFloat("YSpeed", MyBase.transform.position.y - Me.lastPos.y)
			Else
				Dim vector As Vector3 = New Vector3(Me.attackStart.x + Me.attackWidth, Me.attackStart.y) - Me.lastPos
				If vector.magnitude > 15F Then
					vector = vector.normalized * 15F
				End If
				MyBase.transform.position += vector
				MyBase.animator.SetFloat("YSpeed", vector.y)
			End If
			If MyBase.transform.position.y - Me.lastPos.y > -6F Then
				Me.dripTimer -= CupheadTime.FixedDelta
				If Me.dripTimer <= 0F Then
					Dim snowCultLevelBatDrip As SnowCultLevelBatDrip = TryCast(Me.dripPrefab.Create(MyBase.transform.position + Vector3.down * 50F), SnowCultLevelBatDrip)
					snowCultLevelBatDrip.SetColor(Me.animatorSuffix)
					snowCultLevelBatDrip.vel.x = (MyBase.transform.position.x - Me.lastPos.x) / 2F
					Me.dripTimer = Global.UnityEngine.Random.Range(0.3F, 0.7F)
				End If
			End If
			If Me.attackTime > 1.2F Then
				If Me.readdOnEscape Then
					Me.moving = False
					Me.collider.enabled = False
					Me.parent.ReturnBatToList(Me)
				Else
					Me.Dead()
				End If
			End If
		End If
	End Sub

	' Token: 0x06002E50 RID: 11856 RVA: 0x001B4E68 File Offset: 0x001B3268
	Public Sub Dead()
		If MyBase.transform.position.y < 360F Then
			CType(Me.explosionPrefab.Create(MyBase.transform.position), SnowCultLevelBatEffect).SetColor(Me.animatorSuffix)
			Me.SFX_SNOWCULT_BatDie()
		End If
		If Level.Current.mode = Level.Mode.Easy Then
			Me.EasyModeDie()
		Else
			Me.StopAllCoroutines()
			Me.Recycle()
		End If
	End Sub

	' Token: 0x06002E51 RID: 11857 RVA: 0x001B4EE4 File Offset: 0x001B32E4
	Private Sub EasyModeDie()
		Me.moving = False
		Me.collider.enabled = False
		Me.parent.ReturnBatToList(Me)
		MyBase.transform.position = New Vector3(MyBase.transform.position.x, 460F)
	End Sub

	' Token: 0x06002E52 RID: 11858 RVA: 0x001B4F38 File Offset: 0x001B3338
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		RemoveHandler Me.parent.OnDeathEvent, AddressOf Me.Dead
	End Sub

	' Token: 0x06002E53 RID: 11859 RVA: 0x001B4F57 File Offset: 0x001B3357
	Private Sub SFX_SNOWCULT_BatDie()
		AudioManager.Play("sfx_dlc_snowcult_p2_popsicle_bat_death")
		Me.emitAudioFromObject.Add("sfx_dlc_snowcult_p2_popsicle_bat_death")
	End Sub

	' Token: 0x040036CE RID: 14030
	Private Const PADDING_FLOOR As Single = 100F

	' Token: 0x040036CF RID: 14031
	Private Const PADDING_CEILING As Single = 100F

	' Token: 0x040036D0 RID: 14032
	Private Const DRIP_TIME_MIN As Single = 0.3F

	' Token: 0x040036D1 RID: 14033
	Private Const DRIP_TIME_MAX As Single = 0.7F

	' Token: 0x040036D2 RID: 14034
	Private damageReceiver As DamageReceiver

	' Token: 0x040036D3 RID: 14035
	Private parent As SnowCultLevelYeti

	' Token: 0x040036D4 RID: 14036
	Private speed As Single

	' Token: 0x040036D5 RID: 14037
	Private Health As Single

	' Token: 0x040036D6 RID: 14038
	Public reachedCircle As Boolean

	' Token: 0x040036D7 RID: 14039
	Public moving As Boolean

	' Token: 0x040036D8 RID: 14040
	Private launchVelocity As Vector3

	' Token: 0x040036D9 RID: 14041
	Private attackHeight As Single

	' Token: 0x040036DA RID: 14042
	Private attackWidth As Single

	' Token: 0x040036DB RID: 14043
	Private attackStart As Vector3

	' Token: 0x040036DC RID: 14044
	Private attackTime As Single

	' Token: 0x040036DD RID: 14045
	Private arcModifier As Single = 1F

	' Token: 0x040036DE RID: 14046
	Private dripTimer As Single

	' Token: 0x040036DF RID: 14047
	Private shotSpeed As Single

	' Token: 0x040036E0 RID: 14048
	Private readdOnEscape As Boolean

	' Token: 0x040036E1 RID: 14049
	Private lastPos As Vector3

	' Token: 0x040036E2 RID: 14050
	<SerializeField()>
	Private explosionPrefab As SnowCultLevelBatEffect

	' Token: 0x040036E3 RID: 14051
	<SerializeField()>
	Private dripPrefab As SnowCultLevelBatEffect

	' Token: 0x040036E4 RID: 14052
	<SerializeField()>
	Private collider As Collider2D

	' Token: 0x040036E5 RID: 14053
	<SerializeField()>
	Private spriteRenderer As SpriteRenderer

	' Token: 0x040036E6 RID: 14054
	Private animatorSuffix As String
End Class
