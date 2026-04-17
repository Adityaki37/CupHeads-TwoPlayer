Imports System
Imports UnityEngine

' Token: 0x020004C4 RID: 1220
Public Class AirplaneLevelSecretTerrierBullet
	Inherits AbstractProjectile

	' Token: 0x06001480 RID: 5248 RVA: 0x000B7DE4 File Offset: 0x000B61E4
	Public Function Create(pos As Vector3, targetPos As Vector3, props As LevelProperties.Airplane.SecretTerriers, scale As Vector3) As AirplaneLevelSecretTerrierBullet
		Dim airplaneLevelSecretTerrierBullet As AirplaneLevelSecretTerrierBullet = TryCast(MyBase.Create(), AirplaneLevelSecretTerrierBullet)
		airplaneLevelSecretTerrierBullet.speed = props.dogBulletArcSpeed
		airplaneLevelSecretTerrierBullet.arcHeight = props.dogBulletArcHeight
		airplaneLevelSecretTerrierBullet.splitAngle = props.dogBulletSplitAngle
		airplaneLevelSecretTerrierBullet.splitSpeed = props.dogBulletSplitSpeed
		airplaneLevelSecretTerrierBullet.willSplit = props.dogBulletWillSplit
		airplaneLevelSecretTerrierBullet.hp = props.dogBulletHealth
		airplaneLevelSecretTerrierBullet.transform.position = pos
		airplaneLevelSecretTerrierBullet.posTimer = 0F
		airplaneLevelSecretTerrierBullet.startPos = pos
		airplaneLevelSecretTerrierBullet.destPos = targetPos
		airplaneLevelSecretTerrierBullet.lastPos = airplaneLevelSecretTerrierBullet.startPos
		airplaneLevelSecretTerrierBullet.transform.localScale = scale
		airplaneLevelSecretTerrierBullet.damageReceiver = airplaneLevelSecretTerrierBullet.GetComponent(Of DamageReceiver)()
		AddHandler airplaneLevelSecretTerrierBullet.damageReceiver.OnDamageTaken, AddressOf airplaneLevelSecretTerrierBullet.OnDamageTaken
		Return airplaneLevelSecretTerrierBullet
	End Function

	' Token: 0x06001481 RID: 5249 RVA: 0x000B7EA7 File Offset: 0x000B62A7
	Public Overrides Sub SetParryable(parryable As Boolean)
		MyBase.SetParryable(parryable)
	End Sub

	' Token: 0x06001482 RID: 5250 RVA: 0x000B7EB0 File Offset: 0x000B62B0
	Protected Overrides Sub FixedUpdate()
		MyBase.FixedUpdate()
		If Me.exploded Then
			Return
		End If
		If Me.posTimer < 1F Then
			Me.lastPos = MyBase.transform.position
			MyBase.transform.position = Vector3.Lerp(Me.startPos, Me.destPos, Me.posTimer) + Vector3.up * Mathf.Sin(Me.posTimer * 3.1415927F) * Me.arcHeight
			Me.posTimer += Me.speed * CupheadTime.FixedDelta
		Else
			MyBase.transform.position += Me.destPos - Me.lastPos
			Me.lastPos += Vector3.up * Me.arcHeight * CupheadTime.FixedDelta * 0.25F
		End If
	End Sub

	' Token: 0x06001483 RID: 5251 RVA: 0x000B7FB7 File Offset: 0x000B63B7
	Protected Overrides Sub Update()
		MyBase.Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x06001484 RID: 5252 RVA: 0x000B7FD8 File Offset: 0x000B63D8
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		If Me.exploded Then
			Return
		End If
		Me.hp -= info.damage
		If Me.hp <= 0F Then
			Me.exploded = True
			Me.coll.enabled = False
			MyBase.animator.Play(If((Not Rand.Bool()), "ExplodeB", "ExplodeA"))
			AudioManager.Play("sfx_dlc_dogfight_ps_terrier_pineappleexplode")
		End If
	End Sub

	' Token: 0x06001485 RID: 5253 RVA: 0x000B8058 File Offset: 0x000B6458
	Private Sub AniEvent_SpawnShrapnel()
		Me.splitBulletPrefab.Create(MyBase.transform.position, MathUtils.DirectionToAngle(Vector3.right), Me.splitSpeed)
		Me.splitBulletPrefab.Create(MyBase.transform.position, MathUtils.DirectionToAngle(Vector3.right) - Me.splitAngle, Me.splitSpeed)
		Me.splitBulletPrefab.Create(MyBase.transform.position, MathUtils.DirectionToAngle(Vector3.right) + Me.splitAngle, Me.splitSpeed)
	End Sub

	' Token: 0x06001486 RID: 5254 RVA: 0x000B8106 File Offset: 0x000B6506
	Private Sub AniEvent_EndExplosion()
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub

	' Token: 0x06001487 RID: 5255 RVA: 0x000B8113 File Offset: 0x000B6513
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If phase = CollisionPhase.Enter Then
			Me.damageDealer.DealDamage(hit)
		End If
		AudioManager.Play("sfx_dlc_dogfight_ps_terrier_pineapplehitplayer")
	End Sub

	' Token: 0x04001DD1 RID: 7633
	Private speed As Single

	' Token: 0x04001DD2 RID: 7634
	Private arcHeight As Single

	' Token: 0x04001DD3 RID: 7635
	Private splitAngle As Single

	' Token: 0x04001DD4 RID: 7636
	Private splitSpeed As Single

	' Token: 0x04001DD5 RID: 7637
	Private hp As Single

	' Token: 0x04001DD6 RID: 7638
	Private willSplit As Boolean

	' Token: 0x04001DD7 RID: 7639
	Private startPos As Vector3

	' Token: 0x04001DD8 RID: 7640
	Private destPos As Vector3

	' Token: 0x04001DD9 RID: 7641
	Private lastPos As Vector3

	' Token: 0x04001DDA RID: 7642
	Private posTimer As Single

	' Token: 0x04001DDB RID: 7643
	Private exploded As Boolean

	' Token: 0x04001DDC RID: 7644
	<SerializeField()>
	Private coll As CircleCollider2D

	' Token: 0x04001DDD RID: 7645
	<SerializeField()>
	Private splitBulletPrefab As BasicProjectile

	' Token: 0x04001DDE RID: 7646
	Private damageReceiver As DamageReceiver
End Class
