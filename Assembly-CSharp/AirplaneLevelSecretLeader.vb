Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020004C2 RID: 1218
Public Class AirplaneLevelSecretLeader
	Inherits LevelProperties.Airplane.Entity

	' Token: 0x0600145F RID: 5215 RVA: 0x000B69B2 File Offset: 0x000B4DB2
	Private Sub Start()
		Me.damageDealer = DamageDealer.NewEnemy()
		Me.damageReceiver = MyBase.GetComponent(Of DamageReceiver)()
		AddHandler Me.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
		Me.effectSide = Rand.Bool()
	End Sub

	' Token: 0x06001460 RID: 5216 RVA: 0x000B69ED File Offset: 0x000B4DED
	Protected Overrides Sub OnDestroy()
		RemoveHandler Me.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
		MyBase.OnDestroy()
		Me.WORKAROUND_NullifyFields()
	End Sub

	' Token: 0x06001461 RID: 5217 RVA: 0x000B6A14 File Offset: 0x000B4E14
	Public Overrides Sub LevelInit(properties As LevelProperties.Airplane)
		MyBase.LevelInit(properties)
		Me.rocketPositionString = New PatternString(properties.CurrentState.secretLeader.rocketHomingSpawnLocation, True, True)
		Me.terrierProjectileParryableString = New PatternString(properties.CurrentState.secretTerriers.dogBulletParryString, True)
		MyBase.StartCoroutine(Me.attack_cr())
	End Sub

	' Token: 0x06001462 RID: 5218 RVA: 0x000B6A6E File Offset: 0x000B4E6E
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		MyBase.properties.DealDamage(info.damage)
		If MyBase.properties.CurrentHealth <= 0F AndAlso Not Me.isDead Then
			Me.Die()
		End If
	End Sub

	' Token: 0x06001463 RID: 5219 RVA: 0x000B6AA7 File Offset: 0x000B4EA7
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x06001464 RID: 5220 RVA: 0x000B6ABD File Offset: 0x000B4EBD
	Public Function TerrierProjectileParryable() As Boolean
		Return Me.terrierProjectileParryableString.PopLetter() = "P"c
	End Function

	' Token: 0x06001465 RID: 5221 RVA: 0x000B6ACE File Offset: 0x000B4ECE
	Public Sub DieMain()
		Me.StopAllCoroutines()
		MyBase.StartCoroutine(Me.die_main_cr())
	End Sub

	' Token: 0x06001466 RID: 5222 RVA: 0x000B6AE4 File Offset: 0x000B4EE4
	Private Iterator Function die_main_cr() As IEnumerator
		MyBase.GetComponent(Of Collider2D)().enabled = False
		Me.hiding = True
		Me.currentHole = 3
		MyBase.animator.Play("Death")
		Me.isDead = True
		MyBase.transform.localScale = New Vector3(Mathf.Sign(Me.level.GetHolePosition(Me.currentHole, True).x - Camera.main.transform.position.x), 1F)
		MyBase.transform.position = Me.level.GetLeaderDeathPosition(Me.currentHole)
		Yield MyBase.animator.WaitForAnimationToStart(Me, "DeathLoop", False)
		MyBase.animator.Play("Tears", 1)
		AudioManager.Play("sfx_dlc_dogfight_leadervocal_death")
		Return
	End Function

	' Token: 0x06001467 RID: 5223 RVA: 0x000B6B00 File Offset: 0x000B4F00
	Private Sub Die()
		Me.isDead = True
		Me.StopAllCoroutines()
		For i As Integer = 0 To Me.terriers.Length - 1
			Me.terriers(i).Die(i)
		Next
		Me.level.leader.animator.Play("Off")
		Me.level.leader.animator.Play("Copter_Death", Me.level.leader.animator.GetLayerIndex("Death"))
		Me.level.leader.animator.Play("Blades", MyBase.animator.GetLayerIndex("DeathBlades"))
		MyBase.animator.Play("DeathLoop")
		MyBase.animator.Play("Tears", 1)
		MyBase.transform.localScale = New Vector3(Mathf.Sign(Me.level.GetHolePosition(Me.currentHole, True).x - Camera.main.transform.position.x), 1F)
		MyBase.transform.position = Me.level.GetLeaderDeathPosition(Me.currentHole)
	End Sub

	' Token: 0x06001468 RID: 5224 RVA: 0x000B6C42 File Offset: 0x000B5042
	Private Sub HideAnimationComplete()
		Me.moved = True
	End Sub

	' Token: 0x06001469 RID: 5225 RVA: 0x000B6C4C File Offset: 0x000B504C
	Private Sub AttackAnimationStart()
		Dim secretLeader As LevelProperties.Airplane.SecretLeader = MyBase.properties.CurrentState.secretLeader
		Dim vector As Vector3 = New Vector3(CSng(If((Not Me.effectSide), 120, (-120))), 120F)
		Me.rocketBGPrefab.Create(Camera.main.transform.position + vector, MathUtils.DirectionToAngle(Vector3.up) + Global.UnityEngine.Random.Range(5F, 12F) * CSng(If((Not Me.effectSide), (-1), 1)), New Vector3(2F, 2F), 600F)
		Me.rocketBGEffect.Create(Camera.main.transform.position + vector)
		Me.effectSide = Not Me.effectSide
	End Sub

	' Token: 0x0600146A RID: 5226 RVA: 0x000B6D30 File Offset: 0x000B5130
	Private Sub AttackAnimationComplete()
		Dim secretLeader As LevelProperties.Airplane.SecretLeader = MyBase.properties.CurrentState.secretLeader
		Me.rocketPrefab.Create(PlayerManager.GetNext(), Camera.main.transform.position + Vector3.up * 800F + Me.rocketPositionString.PopFloat() * Vector3.right, secretLeader.rocketHomingSpeed, secretLeader.rocketHomingRotation, secretLeader.rocketHomingHP, secretLeader.rocketHomingTime)
	End Sub

	' Token: 0x0600146B RID: 5227 RVA: 0x000B6DBC File Offset: 0x000B51BC
	Private Iterator Function attack_cr() As IEnumerator
		Me.level.OccupyHole(Me.currentHole)
		While True
			MyBase.transform.localScale = New Vector3(Mathf.Sign(Me.level.GetHolePosition(Me.currentHole, True).x - Camera.main.transform.position.x), 1F)
			MyBase.transform.position = Me.level.GetHolePosition(Me.currentHole, True)
			Me.rend.sortingOrder = Me.currentHole Mod 3 + 50
			Me.backerRend.sortingOrder = Me.currentHole Mod 3 + 13
			Dim lookingStraight As Boolean = Me.currentHole = 2 OrElse Me.currentHole = 5
			MyBase.animator.SetBool("EyesDown", Not lookingStraight)
			Me.hiding = False
			If Not Me.first Then
				MyBase.animator.Play("Emerge")
			End If
			Me.first = False
			Me.boxCollider.enabled = True
			Yield CupheadTime.WaitForSeconds(Me, MyBase.properties.CurrentState.secretLeader.leaderPreAttackDelay)
			MyBase.animator.Play("AttackStart")
			Yield MyBase.animator.WaitForAnimationToStart(Me, "AttackPreHold", False)
			Yield CupheadTime.WaitForSeconds(Me, MyBase.properties.CurrentState.secretLeader.attackAnticipationHold)
			MyBase.animator.SetTrigger("ContinueAttack")
			Yield MyBase.animator.WaitForAnimationToStart(Me, "AttackPostHold", False)
			Yield CupheadTime.WaitForSeconds(Me, MyBase.properties.CurrentState.secretLeader.attackRecoveryHold)
			MyBase.animator.SetTrigger("ContinueAttack")
			Yield MyBase.animator.WaitForAnimationToEnd(Me, If((Not MyBase.animator.GetBool("EyesDown")), "AttackEnd", "AttackEndEyesDown"), False, True)
			Yield CupheadTime.WaitForSeconds(Me, MyBase.properties.CurrentState.secretLeader.leaderPostAttackDelay)
			MyBase.animator.Play(If((Me.currentHole Mod 3 <> 3), "Exit", "ExitLow"))
			While Not Me.moved
				Yield Nothing
			End While
			Me.boxCollider.enabled = False
			Me.hiding = True
			Me.moved = False
			Dim previousHole As Integer = Me.currentHole
			Me.currentHole = -1
			While Me.currentHole = -1
				Me.currentHole = Me.level.GetNextHole()
			End While
			Me.level.LeaveHole(previousHole)
			Yield CupheadTime.WaitForSeconds(Me, MyBase.properties.CurrentState.secretLeader.hideTime)
		End While
		Return
	End Function

	' Token: 0x0600146C RID: 5228 RVA: 0x000B6DD7 File Offset: 0x000B51D7
	Private Sub AnimationEvent_SFX_DOGFIGHT_PS_LeaderAttack()
		AudioManager.Play("sfx_dlc_dogfight_ps_leader_batonattack")
		Me.emitAudioFromObject.Add("sfx_dlc_dogfight_ps_leader_batonattack")
		AudioManager.Play("sfx_dlc_dogfight_leadervocal_command")
		Me.emitAudioFromObject.Add("sfx_dlc_dogfight_leadervocal_command")
	End Sub

	' Token: 0x0600146D RID: 5229 RVA: 0x000B6E10 File Offset: 0x000B5210
	Private Sub WORKAROUND_NullifyFields()
		Me.damageDealer = Nothing
		Me.rocketBGPrefab = Nothing
		Me.rocketPrefab = Nothing
		Me.rocketBGEffect = Nothing
		Me.level = Nothing
		Me.terriers = Nothing
		Me.rocketPositionString = Nothing
		Me.terrierProjectileParryableString = Nothing
		Me.boxCollider = Nothing
		Me.rend = Nothing
		Me.backerRend = Nothing
	End Sub

	' Token: 0x04001DAB RID: 7595
	Private isDead As Boolean

	' Token: 0x04001DAC RID: 7596
	Private damageDealer As DamageDealer

	' Token: 0x04001DAD RID: 7597
	Private damageReceiver As DamageReceiver

	' Token: 0x04001DAE RID: 7598
	<SerializeField()>
	Private rocketBGPrefab As BasicProjectile

	' Token: 0x04001DAF RID: 7599
	<SerializeField()>
	Private rocketPrefab As AirplaneLevelRocket

	' Token: 0x04001DB0 RID: 7600
	<SerializeField()>
	Private rocketBGEffect As Effect

	' Token: 0x04001DB1 RID: 7601
	<SerializeField()>
	Private level As AirplaneLevel

	' Token: 0x04001DB2 RID: 7602
	<SerializeField()>
	Private terriers As AirplaneLevelSecretTerrier()

	' Token: 0x04001DB3 RID: 7603
	Private rocketPositionString As PatternString

	' Token: 0x04001DB4 RID: 7604
	Private terrierProjectileParryableString As PatternString

	' Token: 0x04001DB5 RID: 7605
	Private attacked As Boolean

	' Token: 0x04001DB6 RID: 7606
	Private moved As Boolean

	' Token: 0x04001DB7 RID: 7607
	Private hiding As Boolean

	' Token: 0x04001DB8 RID: 7608
	Private first As Boolean = True

	' Token: 0x04001DB9 RID: 7609
	<SerializeField()>
	Private currentHole As Integer

	' Token: 0x04001DBA RID: 7610
	<SerializeField()>
	Private boxCollider As BoxCollider2D

	' Token: 0x04001DBB RID: 7611
	<SerializeField()>
	Private rend As SpriteRenderer

	' Token: 0x04001DBC RID: 7612
	<SerializeField()>
	Private backerRend As SpriteRenderer

	' Token: 0x04001DBD RID: 7613
	Private effectSide As Boolean
End Class
