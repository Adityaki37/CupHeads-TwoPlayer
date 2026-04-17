Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports UnityEngine

' Token: 0x02000626 RID: 1574
Public Class FlyingBirdLevelSmallBird
	Inherits LevelProperties.FlyingBird.Entity

	' Token: 0x1700037D RID: 893
	' (get) Token: 0x06002001 RID: 8193 RVA: 0x0012602C File Offset: 0x0012442C
	' (set) Token: 0x06002002 RID: 8194 RVA: 0x00126034 File Offset: 0x00124434
	Public Property state As FlyingBirdLevelSmallBird.State

	' Token: 0x1700037E RID: 894
	' (get) Token: 0x06002003 RID: 8195 RVA: 0x0012603D File Offset: 0x0012443D
	' (set) Token: 0x06002004 RID: 8196 RVA: 0x00126045 File Offset: 0x00124445
	Public Property direction As FlyingBirdLevelSmallBird.Direction

	' Token: 0x06002005 RID: 8197 RVA: 0x00126050 File Offset: 0x00124450
	Private Sub Start()
		Me.damageDealer = DamageDealer.NewEnemy()
		Me.damageReceiver = Me.sprite.GetComponent(Of DamageReceiver)()
		AddHandler Me.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
		Me.collisionChild = Me.sprite.GetComponent(Of CollisionChild)()
		AddHandler Me.collisionChild.OnPlayerCollision, AddressOf Me.OnPlayerCollision
		Me.aim = New GameObject("Aim").transform
		Me.aim.SetParent(Me.bulletRoot)
		Me.aim.ResetLocalTransforms()
		MyBase.gameObject.SetActive(False)
	End Sub

	' Token: 0x06002006 RID: 8198 RVA: 0x001260F5 File Offset: 0x001244F5
	Private Sub Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
		Me.PositionEggs()
	End Sub

	' Token: 0x06002007 RID: 8199 RVA: 0x00126113 File Offset: 0x00124513
	Public Overrides Sub LevelInit(properties As LevelProperties.FlyingBird)
		MyBase.LevelInit(properties)
		If Level.Current.mode = Level.Mode.Easy Then
			AddHandler properties.OnBossDeath, AddressOf Me.OnBossDeath
		End If
	End Sub

	' Token: 0x06002008 RID: 8200 RVA: 0x00126140 File Offset: 0x00124540
	Private Sub OnBossDeath()
		If Level.Current.mode = Level.Mode.Easy Then
			RemoveHandler MyBase.properties.OnBossDeath, AddressOf Me.OnBossDeath
		End If
		Me.sprite.GetComponent(Of Collider2D)().enabled = False
		RemoveHandler MyBase.properties.OnStateChange, AddressOf Me.OnBossDeath
		Me.StopAllCoroutines()
		Me.sprite.transform.ResetLocalTransforms()
		MyBase.animator.Play("Death")
		AudioManager.Play("level_flyingbird_small_bird_death_cry")
		Me.emitAudioFromObject.Add("level_flyingbird_small_bird_death_cry")
		AudioManager.[Stop]("level_flyingbird_small_bird_rotating_eggs_loop")
		For Each flyingBirdLevelSmallBirdEgg As FlyingBirdLevelSmallBirdEgg In Me.eggs
			flyingBirdLevelSmallBirdEgg.Explode()
		Next
		If Level.Current.mode <> Level.Mode.Easy Then
			Me.sprite.GetComponent(Of LevelBossDeathExploder)().StartExplosion()
			MyBase.StartCoroutine(Me.leave_cr())
		End If
	End Sub

	' Token: 0x06002009 RID: 8201 RVA: 0x00126260 File Offset: 0x00124660
	Private Sub OnPlayerCollision(hit As GameObject, phase As CollisionPhase)
		If Me.state = FlyingBirdLevelSmallBird.State.Dead Then
			Return
		End If
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x0600200A RID: 8202 RVA: 0x00126287 File Offset: 0x00124687
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		If Me.state = FlyingBirdLevelSmallBird.State.Dead Then
			Return
		End If
		MyBase.properties.DealDamage(info.damage)
	End Sub

	' Token: 0x0600200B RID: 8203 RVA: 0x001262A8 File Offset: 0x001246A8
	Public Sub StartPattern(pos As Vector2)
		AddHandler MyBase.properties.OnStateChange, AddressOf Me.OnBossDeath
		If Me.state <> FlyingBirdLevelSmallBird.State.Init Then
			Return
		End If
		Me.state = FlyingBirdLevelSmallBird.State.Starting
		MyBase.transform.position = pos
		MyBase.gameObject.SetActive(True)
		MyBase.StartCoroutine(Me.float_cr())
		MyBase.StartCoroutine(Me.start_cr())
	End Sub

	' Token: 0x0600200C RID: 8204 RVA: 0x00126316 File Offset: 0x00124716
	Private Sub TurnComplete()
	End Sub

	' Token: 0x0600200D RID: 8205 RVA: 0x00126318 File Offset: 0x00124718
	Private Sub PositionEggs()
		If Me.eggs Is Nothing OrElse Me.eggs.Count < 1 Then
			Return
		End If
		For Each flyingBirdLevelSmallBirdEgg As FlyingBirdLevelSmallBirdEgg In Me.eggs
			flyingBirdLevelSmallBirdEgg.transform.localPosition = Vector3.zero
		Next
	End Sub

	' Token: 0x0600200E RID: 8206 RVA: 0x0012639C File Offset: 0x0012479C
	Private Iterator Function start_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 1F)
		MyBase.StartCoroutine(Me.eggs_cr())
		Yield CupheadTime.WaitForSeconds(Me, 1F)
		MyBase.StartCoroutine(Me.moveX_cr())
		MyBase.StartCoroutine(Me.moveY_cr())
		MyBase.StartCoroutine(Me.shooting_cr())
		Return
	End Function

	' Token: 0x0600200F RID: 8207 RVA: 0x001263B8 File Offset: 0x001247B8
	Private Iterator Function float_cr() As IEnumerator
		Yield Me.sprite.TweenLocalPositionY(0F, 10F, 1F, EaseUtils.EaseType.easeOutSine)
		While True
			Yield Me.sprite.TweenLocalPositionY(10F, -10F, 1F, EaseUtils.EaseType.easeInOutSine)
			Yield Me.sprite.TweenLocalPositionY(-10F, 10F, 1F, EaseUtils.EaseType.easeInOutSine)
		End While
		Return
	End Function

	' Token: 0x06002010 RID: 8208 RVA: 0x001263D4 File Offset: 0x001247D4
	Private Iterator Function leave_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 1F)
		Dim renderer As SpriteRenderer = Me.sprite.GetComponent(Of SpriteRenderer)()
		Dim [end] As Single = CSng(If((MyBase.transform.position.x <= 0F), Level.Current.Left, Level.Current.Right))
		[end] += renderer.bounds.size.x / 2F * Mathf.Sign(MyBase.transform.position.x)
		MyBase.StartCoroutine(Me.tweenX_cr(MyBase.transform.position.x, [end], MyBase.properties.CurrentState.smallBird.leaveTime, EaseUtils.EaseType.easeInOutSine))
		Me.sprite.GetComponent(Of Collider2D)().enabled = False
		While Me.state <> FlyingBirdLevelSmallBird.State.Dead
			Yield Nothing
		End While
		Global.UnityEngine.[Object].Destroy(Me.sprite.GetComponent(Of LevelBossDeathExploder)())
		MyBase.gameObject.SetActive(False)
		Return
	End Function

	' Token: 0x06002011 RID: 8209 RVA: 0x001263F0 File Offset: 0x001247F0
	Private Sub ShootProjectile()
		Me.aim.LookAt2D(PlayerManager.Current.center)
		Me.bulletPrefab.Create(Me.bulletRoot.position, Me.aim.eulerAngles.z + 180F, -MyBase.properties.CurrentState.smallBird.shotSpeed).SetParryable(True)
	End Sub

	' Token: 0x06002012 RID: 8210 RVA: 0x00126464 File Offset: 0x00124864
	Private Iterator Function shooting_cr() As IEnumerator
		While True
			Yield CupheadTime.WaitForSeconds(Me, MyBase.properties.CurrentState.smallBird.shotDelay)
			Dim target As AbstractPlayerController = PlayerManager.GetNext()
			If Me.direction = FlyingBirdLevelSmallBird.Direction.Left Then
				If target.center.x > MyBase.transform.position.x Then
					Yield Me.Turn(FlyingBirdLevelSmallBird.Direction.Right)
				End If
			ElseIf target.center.x < MyBase.transform.position.x Then
				Yield Me.Turn(FlyingBirdLevelSmallBird.Direction.Left)
			End If
			Dim lastState As FlyingBirdLevelSmallBird.State = Me.state
			Me.state = FlyingBirdLevelSmallBird.State.Shooting
			MyBase.animator.SetTrigger("Shoot")
			MyBase.animator.WaitForAnimationToEnd(Me, "Shoot", False, True)
			Me.state = lastState
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06002013 RID: 8211 RVA: 0x0012647F File Offset: 0x0012487F
	Private Function Turn(d As FlyingBirdLevelSmallBird.Direction) As Coroutine
		Return MyBase.StartCoroutine(Me.turn_cr(d))
	End Function

	' Token: 0x06002014 RID: 8212 RVA: 0x00126490 File Offset: 0x00124890
	Private Iterator Function turn_cr(d As FlyingBirdLevelSmallBird.Direction) As IEnumerator
		If Me.direction <> d Then
			Me.sprite.transform.SetScale(New Single?(CSng(d)), Nothing, Nothing)
			Me.direction = d
			MyBase.animator.Play("Turn")
			Yield MyBase.animator.WaitForAnimationToEnd(Me, "Turn", False, True)
		End If
		Return
	End Function

	' Token: 0x06002015 RID: 8213 RVA: 0x001264B4 File Offset: 0x001248B4
	Private Iterator Function eggs_cr() As IEnumerator
		Dim count As Integer = MyBase.properties.CurrentState.smallBird.eggCount
		Me.eggs = New List(Of FlyingBirdLevelSmallBirdEgg)()
		Me.eggContainer = New GameObject("Eggs").transform
		Me.eggContainer.SetParent(MyBase.transform)
		Me.eggContainer.ResetLocalTransforms()
		Me.eggContainer.SetLocalPosition(Nothing, New Single?(-65F), Nothing)
		For i As Integer = 0 To count - 1
			Dim num As Single = CSng(i) / CSng(count) * 360F
			Dim flyingBirdLevelSmallBirdEgg As FlyingBirdLevelSmallBirdEgg = Me.eggPrefab.InstantiatePrefab(Of FlyingBirdLevelSmallBirdEgg)()
			flyingBirdLevelSmallBirdEgg.SetParent(Me.eggContainer, MyBase.properties)
			flyingBirdLevelSmallBirdEgg.container.SetEulerAngles(New Single?(0F), New Single?(0F), New Single?(num))
			Me.eggs.Add(flyingBirdLevelSmallBirdEgg)
		Next
		AudioManager.PlayLoop("level_flyingbird_small_bird_rotating_eggs_loop")
		Me.emitAudioFromObject.Add("level_flyingbird_small_bird_rotating_eggs_loop")
		While True
			Me.eggContainer.AddLocalEulerAngles(0F, 0F, MyBase.properties.CurrentState.smallBird.eggRotationSpeed * CupheadTime.Delta)
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06002016 RID: 8214 RVA: 0x001264D0 File Offset: 0x001248D0
	Private Iterator Function moveX_cr() As IEnumerator
		Me.direction = FlyingBirdLevelSmallBird.Direction.Left
		While True
			Dim minX As Single = MyBase.properties.CurrentState.smallBird.minX
			Me.state = FlyingBirdLevelSmallBird.State.Left
			Yield MyBase.StartCoroutine(Me.tweenX_cr(MyBase.transform.position.x, minX, MyBase.properties.CurrentState.smallBird.timeX, EaseUtils.EaseType.easeInOutSine))
			Yield Me.Turn(FlyingBirdLevelSmallBird.Direction.Right)
			Me.state = FlyingBirdLevelSmallBird.State.Right
			Yield MyBase.StartCoroutine(Me.tweenX_cr(minX, 520F, MyBase.properties.CurrentState.smallBird.timeX, EaseUtils.EaseType.easeInOutSine))
			Yield Me.Turn(FlyingBirdLevelSmallBird.Direction.Left)
		End While
		Return
	End Function

	' Token: 0x06002017 RID: 8215 RVA: 0x001264EC File Offset: 0x001248EC
	Private Iterator Function tweenX_cr(start As Single, [end] As Single, time As Single, ease As EaseUtils.EaseType) As IEnumerator
		MyBase.transform.SetPosition(New Single?(start), Nothing, Nothing)
		Dim t As Single = 0F
		While t < time
			Dim val As Single = t / time
			MyBase.transform.SetPosition(New Single?(EaseUtils.Ease(ease, start, [end], val)), Nothing, Nothing)
			t += CupheadTime.Delta
			Yield Nothing
		End While
		Me.state = FlyingBirdLevelSmallBird.State.Dead
		MyBase.transform.SetPosition(New Single?([end]), Nothing, Nothing)
		Yield Nothing
		Return
	End Function

	' Token: 0x06002018 RID: 8216 RVA: 0x00126524 File Offset: 0x00124924
	Private Iterator Function moveY_cr() As IEnumerator
		If Rand.Bool() Then
			Yield MyBase.StartCoroutine(Me.tweenY_cr(MyBase.transform.position.y, 260F, MyBase.properties.CurrentState.smallBird.timeY, EaseUtils.EaseType.easeInOutSine))
		Else
			Dim currentDist As Single = -230F - MyBase.transform.position.y
			Dim normalDist As Single = 490F
			Dim time As Single = MyBase.properties.CurrentState.smallBird.timeY - currentDist / normalDist
			Yield MyBase.StartCoroutine(Me.tweenY_cr(MyBase.transform.position.y, -230F, time, EaseUtils.EaseType.easeInOutSine))
			Yield MyBase.StartCoroutine(Me.tweenY_cr(-230F, 260F, MyBase.properties.CurrentState.smallBird.timeY, EaseUtils.EaseType.easeInOutSine))
		End If
		While True
			Yield MyBase.StartCoroutine(Me.tweenY_cr(260F, -230F, MyBase.properties.CurrentState.smallBird.timeY, EaseUtils.EaseType.easeInOutSine))
			Yield MyBase.StartCoroutine(Me.tweenY_cr(-230F, 260F, MyBase.properties.CurrentState.smallBird.timeY, EaseUtils.EaseType.easeInOutSine))
		End While
		Return
	End Function

	' Token: 0x06002019 RID: 8217 RVA: 0x00126540 File Offset: 0x00124940
	Private Iterator Function tweenY_cr(start As Single, [end] As Single, time As Single, ease As EaseUtils.EaseType) As IEnumerator
		MyBase.transform.SetPosition(Nothing, New Single?(start), Nothing)
		Dim t As Single = 0F
		While t < time
			Dim val As Single = t / time
			MyBase.transform.SetPosition(Nothing, New Single?(EaseUtils.Ease(ease, start, [end], val)), Nothing)
			t += CupheadTime.Delta
			Yield Nothing
		End While
		MyBase.transform.SetPosition(Nothing, New Single?([end]), Nothing)
		Yield Nothing
		Return
	End Function

	' Token: 0x0600201A RID: 8218 RVA: 0x00126578 File Offset: 0x00124978
	Private Sub SmallLaserShootSFX()
		AudioManager.Play("level_flyingbird_small_bird_shoot")
		Me.emitAudioFromObject.Add("level_flyingbird_small_bird_shoot")
	End Sub

	' Token: 0x0400288A RID: 10378
	<SerializeField()>
	Private sprite As FlyingBirdLevelSmallBirdSprite

	' Token: 0x0400288B RID: 10379
	Private collisionChild As CollisionChild

	' Token: 0x0400288C RID: 10380
	Private damageReceiver As DamageReceiver

	' Token: 0x0400288D RID: 10381
	Private damageDealer As DamageDealer

	' Token: 0x0400288E RID: 10382
	<Space(10F)>
	<SerializeField()>
	Private eggPrefab As FlyingBirdLevelSmallBirdEgg

	' Token: 0x0400288F RID: 10383
	<Space(10F)>
	<SerializeField()>
	Private bulletPrefab As BasicProjectile

	' Token: 0x04002890 RID: 10384
	<SerializeField()>
	Private bulletRoot As Transform

	' Token: 0x04002893 RID: 10387
	Private aim As Transform

	' Token: 0x04002894 RID: 10388
	Private eggContainer As Transform

	' Token: 0x04002895 RID: 10389
	Private eggs As List(Of FlyingBirdLevelSmallBirdEgg)

	' Token: 0x02000627 RID: 1575
	Public Enum State
		' Token: 0x04002897 RID: 10391
		Init
		' Token: 0x04002898 RID: 10392
		Starting
		' Token: 0x04002899 RID: 10393
		Right
		' Token: 0x0400289A RID: 10394
		Left
		' Token: 0x0400289B RID: 10395
		Shooting
		' Token: 0x0400289C RID: 10396
		Dead
	End Enum

	' Token: 0x02000628 RID: 1576
	Public Enum Direction
		' Token: 0x0400289E RID: 10398
		Right = -1
		' Token: 0x0400289F RID: 10399
		Left = 1
	End Enum
End Class
