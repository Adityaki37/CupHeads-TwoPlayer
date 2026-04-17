Imports System
Imports UnityEngine

' Token: 0x020007F8 RID: 2040
Public Class SnowCultLevelSplitShotBullet
	Inherits AbstractProjectile

	' Token: 0x06002EDF RID: 11999 RVA: 0x001BAA84 File Offset: 0x001B8E84
	Public Overridable Function Init(pos As Vector3, rotation As Single, speed As Single, numOfBullets As Integer, spreadAngle As Single, properties As LevelProperties.SnowCult.SplitShot) As SnowCultLevelSplitShotBullet
		MyBase.ResetLifetime()
		MyBase.ResetDistance()
		MyBase.transform.position = pos
		Me.basePos = pos
		Me.rotation = rotation
		Me.speed = speed
		Me.moving = False
		Me.numOfBullets = numOfBullets
		Me.spreadAngle = spreadAngle
		Me.coll = MyBase.GetComponent(Of Collider2D)()
		Me.wobbleTimer = Global.UnityEngine.Random.Range(0F, 6.2831855F)
		Me.coll.enabled = False
		Me.spawnFX.Play("Spawn" + If((Not MyBase.CanParry), String.Empty, "Pink"))
		Return Me
	End Function

	' Token: 0x06002EE0 RID: 12000 RVA: 0x001BAB34 File Offset: 0x001B8F34
	Protected Overrides Sub FixedUpdate()
		MyBase.FixedUpdate()
		If Me.moving Then
			MyBase.transform.position += MathUtils.AngleToDirection(Me.rotation) * Me.speed * CupheadTime.FixedDelta
			If Me.coll.enabled AndAlso Mathf.Abs(MyBase.transform.position.x) > CSng(Level.Current.Right) Then
				Me.middleAngle = If((MyBase.transform.position.x >= 0F), 180F, 0F)
				MyBase.transform.localScale = New Vector3(-Mathf.Sign(MyBase.transform.position.x), 1F)
				MyBase.transform.position = New Vector3(CSng((Level.Current.Left - 65)) * -Mathf.Sign(MyBase.transform.position.x), MyBase.transform.position.y)
				MyBase.animator.Play("BucketExplode")
				Me.SFX_SNOWCULT_JackFrostSplitshotBucketImpact()
				Me.SFX_SNOWCULT_JackFrostSplitshotBucketTravelLoopStop()
				Me.coll.enabled = False
				Me.SpawnProjectiles()
				Me.speed = 0F
			End If
		Else
			Me.wobbleTimer += CupheadTime.FixedDelta * Me.wobbleSpeed
			MyBase.transform.position = Me.basePos + New Vector3(Mathf.Sin(Me.wobbleTimer * 3F) * Me.wobbleX, Mathf.Cos(Me.wobbleTimer * 2F) * Me.wobbleY)
		End If
	End Sub

	' Token: 0x06002EE1 RID: 12001 RVA: 0x001BAD14 File Offset: 0x001B9114
	Public Sub Grow()
		Me.coll.enabled = True
		Me.startedGrowing = True
		MyBase.animator.Play("BucketStart" + If((Not MyBase.CanParry), String.Empty, "Pink"))
	End Sub

	' Token: 0x06002EE2 RID: 12002 RVA: 0x001BAD64 File Offset: 0x001B9164
	Public Sub Fire()
		Me.moving = True
		MyBase.animator.Play("BucketLoop" + If((Not MyBase.CanParry), String.Empty, "Pink"))
		Me.shootFX.Create(MyBase.transform.position, New Vector3(-Mathf.Sign(MyBase.transform.position.x), 1F))
		Me.spawnFX.Play("None")
		Me.SFX_SNOWCULT_JackFrostSplitshotBucketTravelLoop()
	End Sub

	' Token: 0x06002EE3 RID: 12003 RVA: 0x001BADF7 File Offset: 0x001B91F7
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		Me.SFX_SNOWCULT_JackFrostSplitshotBucketImpact()
		MyBase.OnCollisionPlayer(hit, phase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x06002EE4 RID: 12004 RVA: 0x001BAE1B File Offset: 0x001B921B
	Private Sub AniEvent_EndExplode()
		Me.Recycle()
	End Sub

	' Token: 0x06002EE5 RID: 12005 RVA: 0x001BAE24 File Offset: 0x001B9224
	Private Sub SpawnProjectiles()
		If Me.bulletsSpawned Then
			Return
		End If
		Me.bulletsSpawned = True
		Dim num As Single = Me.spreadAngle / Mathf.Round(CSng((Me.numOfBullets / 2)))
		Dim num2 As Single = Me.middleAngle - Me.spreadAngle
		For i As Integer = 0 To Me.numOfBullets - 1
			Me.shatteredBullet.Create(MyBase.transform.position, num2 + num * CSng(i), Me.speed)
		Next
	End Sub

	' Token: 0x06002EE6 RID: 12006 RVA: 0x001BAEA8 File Offset: 0x001B92A8
	Protected Overrides Sub Update()
		MyBase.Update()
		If Me.main.dead <> Me.dead Then
			Me.dead = True
			Me.SFX_SNOWCULT_JackFrostSplitshotBucketTravelLoopStop()
			If Not Me.startedGrowing Then
				Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
			ElseIf MyBase.animator.GetCurrentAnimatorStateInfo(0).IsName("BucketStart" + If((Not MyBase.CanParry), String.Empty, "Pink")) Then
				Me.spawnFX.Play("None")
				MyBase.animator.Play("BucketStartReverse" + If((Not MyBase.CanParry), String.Empty, "Pink"), 0, 1F - MyBase.animator.GetCurrentAnimatorStateInfo(0).normalizedTime)
			End If
		End If
	End Sub

	' Token: 0x06002EE7 RID: 12007 RVA: 0x001BAF90 File Offset: 0x001B9390
	Private Sub SFX_SNOWCULT_JackFrostSplitshotBucketImpact()
		AudioManager.Play("sfx_dlc_snowcult_p3_snowflake_splitshot_attack_bucket_impact")
		Me.emitAudioFromObject.Add("sfx_dlc_snowcult_p3_snowflake_splitshot_attack_bucket_impact")
	End Sub

	' Token: 0x06002EE8 RID: 12008 RVA: 0x001BAFAC File Offset: 0x001B93AC
	Private Sub SFX_SNOWCULT_JackFrostSplitshotBucketTravelLoop()
		AudioManager.PlayLoop("sfx_dlc_snowcult_p3_snowflake_splitshot_handwaving_attack_bucket_travel_loop")
		Me.emitAudioFromObject.Add("sfx_dlc_snowcult_p3_snowflake_splitshot_handwaving_attack_bucket_travel_loop")
	End Sub

	' Token: 0x06002EE9 RID: 12009 RVA: 0x001BAFC8 File Offset: 0x001B93C8
	Private Sub SFX_SNOWCULT_JackFrostSplitshotBucketTravelLoopStop()
		AudioManager.[Stop]("sfx_dlc_snowcult_p3_snowflake_splitshot_handwaving_attack_bucket_travel_loop")
	End Sub

	' Token: 0x0400378D RID: 14221
	<SerializeField()>
	Private shatteredBullet As SnowCultLevelSplitShotBulletShattered

	' Token: 0x0400378E RID: 14222
	<SerializeField()>
	Private shootFX As Effect

	' Token: 0x0400378F RID: 14223
	<SerializeField()>
	Private spawnFX As Animator

	' Token: 0x04003790 RID: 14224
	Private middleAngle As Single

	' Token: 0x04003791 RID: 14225
	Private spreadAngle As Single

	' Token: 0x04003792 RID: 14226
	Private rotation As Single

	' Token: 0x04003793 RID: 14227
	Private speed As Single

	' Token: 0x04003794 RID: 14228
	Private moving As Boolean

	' Token: 0x04003795 RID: 14229
	Private numOfBullets As Integer

	' Token: 0x04003796 RID: 14230
	Private bulletsSpawned As Boolean

	' Token: 0x04003797 RID: 14231
	Private coll As Collider2D

	' Token: 0x04003798 RID: 14232
	Private basePos As Vector3

	' Token: 0x04003799 RID: 14233
	Private wobbleTimer As Single

	' Token: 0x0400379A RID: 14234
	<SerializeField()>
	Private wobbleX As Single = 10F

	' Token: 0x0400379B RID: 14235
	<SerializeField()>
	Private wobbleY As Single = 10F

	' Token: 0x0400379C RID: 14236
	<SerializeField()>
	Private wobbleSpeed As Single = 1F

	' Token: 0x0400379D RID: 14237
	Public main As SnowCultLevelJackFrost

	' Token: 0x0400379E RID: 14238
	Private dead As Boolean

	' Token: 0x0400379F RID: 14239
	Private startedGrowing As Boolean
End Class
