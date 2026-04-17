Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x0200079A RID: 1946
Public Class RumRunnersLevelPoliceman
	Inherits AbstractCollidableObject

	' Token: 0x170003FC RID: 1020
	' (get) Token: 0x06002B3C RID: 11068 RVA: 0x00192D69 File Offset: 0x00191169
	' (set) Token: 0x06002B3D RID: 11069 RVA: 0x00192D71 File Offset: 0x00191171
	Public Property isActive As Boolean

	' Token: 0x06002B3E RID: 11070 RVA: 0x00192D7C File Offset: 0x0019117C
	Private Sub Start()
		Me.damageDealer = DamageDealer.NewEnemy()
		Me.damageReceiver = MyBase.GetComponent(Of DamageReceiver)()
		AddHandler Me.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
		Me.scaleX = MyBase.transform.localScale.x
		Me.collider = MyBase.GetComponent(Of Collider2D)()
		Me.collider.enabled = False
	End Sub

	' Token: 0x06002B3F RID: 11071 RVA: 0x00192DE8 File Offset: 0x001911E8
	Private Sub Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x06002B40 RID: 11072 RVA: 0x00192E00 File Offset: 0x00191200
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x06002B41 RID: 11073 RVA: 0x00192E20 File Offset: 0x00191220
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		Me.hp -= info.damage
		If Me.hp <= 0F AndAlso Me.deathCoroutine Is Nothing Then
			Level.Current.RegisterMinionKilled()
			Me.StopAllCoroutines()
			Me.deathCoroutine = MyBase.StartCoroutine(Me.death_cr())
		End If
	End Sub

	' Token: 0x06002B42 RID: 11074 RVA: 0x00192E7D File Offset: 0x0019127D
	Public Sub SetProperties(properties As LevelProperties.RumRunners.Spider, spider As RumRunnersLevelSpider)
		Me.properties = properties
		Me.spider = spider
	End Sub

	' Token: 0x06002B43 RID: 11075 RVA: 0x00192E90 File Offset: 0x00191290
	Public Sub CopAppear(appearPos As Vector3, isPink As Boolean, goingLeft As Boolean)
		If Me.deathCoroutine IsNot Nothing Then
			Return
		End If
		Dim vector As Vector3 = Me.spawnPositionOffset
		vector.x *= CSng(If((Not goingLeft), 1, (-1)))
		MyBase.transform.position = appearPos + vector
		Me.isPink = isPink
		Me.hp = Me.properties.copHealth
		MyBase.StartCoroutine(Me.shooting_cr())
		MyBase.transform.SetScale(New Single?(If((Not goingLeft), Me.scaleX, (-Me.scaleX))), Nothing, Nothing)
		Me.isActive = True
	End Sub

	' Token: 0x06002B44 RID: 11076 RVA: 0x00192F4C File Offset: 0x0019134C
	Private Iterator Function shooting_cr() As IEnumerator
		Me.collider.enabled = True
		Me.gunSmokeRenderer.enabled = Not Me.isPink
		Me.gunSmokeParryRenderer.enabled = Me.isPink
		Me.lastShootDirection = Me.calculateDirection()
		Dim animatorParameter As String
		Dim stateBaseName As String
		If Me.lastShootDirection = RumRunnersLevelPoliceman.Direction.Down Then
			animatorParameter = "ShootingDown"
			stateBaseName = "ShootDown"
			Me.currentBulletOrigin = Me.bulletOriginDown
		ElseIf Me.lastShootDirection = RumRunnersLevelPoliceman.Direction.Up Then
			animatorParameter = "ShootingUp"
			stateBaseName = "ShootUp"
			Me.currentBulletOrigin = Me.bulletOriginUp
		Else
			animatorParameter = "Shooting"
			stateBaseName = "ShootStraight"
			Me.currentBulletOrigin = Me.bulletOriginStraight
		End If
		Dim alignmentCoroutine As Coroutine = MyBase.StartCoroutine(Me.align_cr())
		MyBase.animator.SetBool(animatorParameter, True)
		Yield MyBase.animator.WaitForAnimationToStart(Me, stateBaseName + "Hold", False)
		Yield CupheadTime.WaitForSeconds(Me, Me.properties.copAttackWarning)
		MyBase.animator.SetTrigger("Shoot")
		Yield MyBase.animator.WaitForAnimationToStart(Me, stateBaseName + "ExitHold", False)
		Yield CupheadTime.WaitForSeconds(Me, Me.properties.copExitDelay)
		MyBase.animator.SetTrigger("ShootExit")
		Yield MyBase.animator.WaitForAnimationToStart(Me, "ShootExit", False)
		Yield Nothing
		While MyBase.animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1F
			Yield Nothing
		End While
		Me.collider.enabled = False
		MyBase.transform.position = New Vector3(0F, 1000F)
		MyBase.animator.SetBool(animatorParameter, False)
		Me.isActive = False
		MyBase.StopCoroutine(alignmentCoroutine)
		Return
	End Function

	' Token: 0x06002B45 RID: 11077 RVA: 0x00192F68 File Offset: 0x00191368
	Private Iterator Function align_cr() As IEnumerator
		Dim waitInstruction As YieldInstruction = New WaitForFixedUpdate()
		While True
			Yield waitInstruction
			MyBase.transform.SetPosition(Nothing, New Single?(RumRunnersLevel.GroundWalkingPosY(MyBase.transform.position, Me.collider, 0F, 200F)), Nothing)
		End While
		Return
	End Function

	' Token: 0x06002B46 RID: 11078 RVA: 0x00192F84 File Offset: 0x00191384
	Private Iterator Function death_cr() As IEnumerator
		Me.SFX_RUMRUN_Police_DiePoof()
		Dim puffPosition As Vector3 = Me.collider.bounds.center
		MyBase.animator.SetBool("Shooting", False)
		MyBase.animator.SetBool("ShootingUp", False)
		MyBase.animator.SetBool("ShootingDown", False)
		Me.isActive = False
		Me.collider.enabled = False
		MyBase.transform.position = puffPosition
		MyBase.animator.SetBool("Die", True)
		Yield MyBase.animator.WaitForAnimationToStart(Me, "Death", False)
		While MyBase.animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1F
			Yield Nothing
		End While
		MyBase.transform.position = New Vector3(0F, 1000F)
		MyBase.animator.SetBool("Die", False)
		Me.deathCoroutine = Nothing
		Return
	End Function

	' Token: 0x06002B47 RID: 11079 RVA: 0x00192FA0 File Offset: 0x001913A0
	Private Sub animationEvent_SpawnBullet()
		Dim vector As Vector3 = Me.spider.transform.position - MyBase.transform.position
		Dim rumRunnersLevelPoliceBullet As RumRunnersLevelPoliceBullet = CType(Me.regularBullet.Create(Me.currentBulletOrigin.transform.position, MathUtils.DirectionToAngle(vector), Me.properties.copBulletSpeed), RumRunnersLevelPoliceBullet)
		rumRunnersLevelPoliceBullet.spiderDamage = Me.properties.copBulletBossDamage
		rumRunnersLevelPoliceBullet.direction = Me.lastShootDirection
		rumRunnersLevelPoliceBullet.SetParryable(Me.isPink)
		rumRunnersLevelPoliceBullet.GetComponent(Of SpriteRenderer)().flipY = Mathf.Sign(vector.x) < 0F
	End Sub

	' Token: 0x06002B48 RID: 11080 RVA: 0x00193051 File Offset: 0x00191451
	Private Sub animationEvent_ExitDisappeared()
		Me.collider.enabled = False
	End Sub

	' Token: 0x06002B49 RID: 11081 RVA: 0x00193060 File Offset: 0x00191460
	Private Function calculateDirection() As RumRunnersLevelPoliceman.Direction
		If MyBase.transform.position.y - Me.spider.transform.position.y > RumRunnersLevelPoliceman.SpiderYDistanceThreshold Then
			Return RumRunnersLevelPoliceman.Direction.Down
		End If
		If Me.spider.transform.position.y - MyBase.transform.position.y > RumRunnersLevelPoliceman.SpiderYDistanceThreshold Then
			Return RumRunnersLevelPoliceman.Direction.Up
		End If
		Return RumRunnersLevelPoliceman.Direction.Straight
	End Function

	' Token: 0x06002B4A RID: 11082 RVA: 0x001930DE File Offset: 0x001914DE
	Private Sub AnimationEvent_SFX_RUMRUN_Police_GunShoot()
		AudioManager.Play("sfx_dlc_rumrun_policegun_shoot")
		Me.emitAudioFromObject.Add("sfx_dlc_rumrun_policegun_shoot")
	End Sub

	' Token: 0x06002B4B RID: 11083 RVA: 0x001930FA File Offset: 0x001914FA
	Private Sub SFX_RUMRUN_Police_DiePoof()
		AudioManager.Play("sfx_dlc_rumrun_lackey_poof")
		Me.emitAudioFromObject.Add("sfx_dlc_rumrun_lackey_poof")
		AudioManager.[Stop]("sfx_dlc_rumrun_policegun_shoot")
	End Sub

	' Token: 0x040033EB RID: 13291
	Private Shared SpiderYDistanceThreshold As Single = 100F

	' Token: 0x040033EC RID: 13292
	<SerializeField()>
	Private regularBullet As RumRunnersLevelPoliceBullet

	' Token: 0x040033ED RID: 13293
	<SerializeField()>
	Private bulletOriginStraight As Transform

	' Token: 0x040033EE RID: 13294
	<SerializeField()>
	Private bulletOriginUp As Transform

	' Token: 0x040033EF RID: 13295
	<SerializeField()>
	Private bulletOriginDown As Transform

	' Token: 0x040033F0 RID: 13296
	<SerializeField()>
	Private spawnPositionOffset As Vector2

	' Token: 0x040033F1 RID: 13297
	<SerializeField()>
	Private gunSmokeRenderer As SpriteRenderer

	' Token: 0x040033F2 RID: 13298
	<SerializeField()>
	Private gunSmokeParryRenderer As SpriteRenderer

	' Token: 0x040033F3 RID: 13299
	Private properties As LevelProperties.RumRunners.Spider

	' Token: 0x040033F4 RID: 13300
	Private spider As RumRunnersLevelSpider

	' Token: 0x040033F5 RID: 13301
	Private damageDealer As DamageDealer

	' Token: 0x040033F6 RID: 13302
	Private damageReceiver As DamageReceiver

	' Token: 0x040033F7 RID: 13303
	Private isPink As Boolean

	' Token: 0x040033F8 RID: 13304
	Private hp As Single

	' Token: 0x040033F9 RID: 13305
	Private scaleX As Single

	' Token: 0x040033FA RID: 13306
	Private collider As Collider2D

	' Token: 0x040033FB RID: 13307
	Private currentBulletOrigin As Transform

	' Token: 0x040033FC RID: 13308
	Private deathCoroutine As Coroutine

	' Token: 0x040033FD RID: 13309
	Private lastShootDirection As RumRunnersLevelPoliceman.Direction

	' Token: 0x0200079B RID: 1947
	Public Enum Direction
		' Token: 0x04003400 RID: 13312
		Straight
		' Token: 0x04003401 RID: 13313
		Up
		' Token: 0x04003402 RID: 13314
		Down
	End Enum
End Class
