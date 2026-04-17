Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000633 RID: 1587
Public Class FlyingBlimpLevelEnemy
	Inherits AbstractCollidableObject

	' Token: 0x17000384 RID: 900
	' (get) Token: 0x06002080 RID: 8320 RVA: 0x0012BB8F File Offset: 0x00129F8F
	' (set) Token: 0x06002081 RID: 8321 RVA: 0x0012BB97 File Offset: 0x00129F97
	Public Property state As FlyingBlimpLevelEnemy.State

	' Token: 0x06002082 RID: 8322 RVA: 0x0012BBA0 File Offset: 0x00129FA0
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.damageDealer = DamageDealer.NewEnemy()
		Me.damageReceiver = MyBase.GetComponent(Of DamageReceiver)()
		AddHandler Me.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
	End Sub

	' Token: 0x06002083 RID: 8323 RVA: 0x0012BBD6 File Offset: 0x00129FD6
	Private Sub Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x06002084 RID: 8324 RVA: 0x0012BBF0 File Offset: 0x00129FF0
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		Me.hp -= info.damage
		If Me.hp < 0F AndAlso Me.state = FlyingBlimpLevelEnemy.State.Spawned Then
			Me.StopAllCoroutines()
			MyBase.StartCoroutine(Me.dying_cr())
		End If
	End Sub

	' Token: 0x06002085 RID: 8325 RVA: 0x0012BC3F File Offset: 0x0012A03F
	Private Sub Start()
		Me.startPoint = MyBase.transform.position
	End Sub

	' Token: 0x06002086 RID: 8326 RVA: 0x0012BC54 File Offset: 0x0012A054
	Public Sub Init(properties As LevelProperties.FlyingBlimp, startPoint As Vector3, stopPoint As Single, Aparryable As Boolean, parent As FlyingBlimpLevelBlimpLady)
		Me.enemyProperties = properties.CurrentState.enemy
		Me.properties = properties
		Me.parent = parent
		Me.startPoint = startPoint
		Me.stopPoint = stopPoint
		AddHandler Me.parent.OnDeathEvent, AddressOf Me.Die
		Me.parryable = Aparryable
		MyBase.StartCoroutine(Me.emerge_cr())
	End Sub

	' Token: 0x06002087 RID: 8327 RVA: 0x0012BCBC File Offset: 0x0012A0BC
	Private Sub CreatePieces()
		For Each flyingBlimpLevelEnemyDeathPart As FlyingBlimpLevelEnemyDeathPart In Me.deathPieces
			flyingBlimpLevelEnemyDeathPart.CreatePart(MyBase.transform.position, Me.properties.CurrentState.gear)
		Next
	End Sub

	' Token: 0x06002088 RID: 8328 RVA: 0x0012BD0A File Offset: 0x0012A10A
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		RemoveHandler Me.parent.OnDeathEvent, AddressOf Me.Die
		Me.projectilePrefab = Nothing
		Me.parryablePrefab = Nothing
		Me.deathPieces = Nothing
	End Sub

	' Token: 0x06002089 RID: 8329 RVA: 0x0012BD3E File Offset: 0x0012A13E
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x0600208A RID: 8330 RVA: 0x0012BD5C File Offset: 0x0012A15C
	Public Iterator Function emerge_cr() As IEnumerator
		Dim wait As YieldInstruction = New WaitForFixedUpdate()
		Me.state = FlyingBlimpLevelEnemy.State.Spawned
		Me.hp = CSng(Me.enemyProperties.hp)
		Dim collider As Collider2D = MyBase.GetComponent(Of Collider2D)()
		collider.enabled = True
		While MyBase.transform.position.x > Me.stopPoint
			MyBase.transform.position += MyBase.transform.right * -Me.enemyProperties.speed * CupheadTime.FixedDelta
			Yield wait
		End While
		Yield CupheadTime.WaitForSeconds(Me, Me.enemyProperties.shotDelay)
		MyBase.animator.Play("Enemy_Attack")
		AudioManager.Play("level_flying_blimp_cannon_ship_fire")
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Enemy_Attack", False, True)
		While MyBase.transform.position.x <= Me.startPoint.x
			MyBase.transform.position += MyBase.transform.right * Me.enemyProperties.speed * CupheadTime.FixedDelta
			Yield wait
			If MyBase.transform.position.x > Me.startPoint.x Then
				Me.Die()
			End If
		End While
		Return
	End Function

	' Token: 0x0600208B RID: 8331 RVA: 0x0012BD78 File Offset: 0x0012A178
	Private Sub FireSpreadshot()
		Dim [next] As AbstractPlayerController = PlayerManager.GetNext()
		Dim num As Single = [next].transform.position.x - MyBase.transform.position.x
		Dim num2 As Single = [next].transform.position.y - MyBase.transform.position.y
		Dim effect As Effect = Global.UnityEngine.[Object].Instantiate(Of Effect)(Me.bulletEffect)
		effect.transform.position = Me.projectileRoot.transform.position
		effect.GetComponent(Of Animator)().SetInteger("PickAni", Global.UnityEngine.Random.Range(0, 3))
		For i As Integer = 0 To Me.enemyProperties.numBullets - 1
			Dim num3 As Single = Me.enemyProperties.spreadAngle.GetFloatAt(CSng(i) / (CSng(Me.enemyProperties.numBullets) - 1F))
			Dim num4 As Single = Me.enemyProperties.spreadAngle.max / 2F
			num3 -= num4
			Dim num5 As Single = Mathf.Atan2(num2, num) * 57.29578F
			Me.animationPicker = Global.UnityEngine.Random.Range(0, 3)
			If [next].transform.position.x > MyBase.transform.position.x Then
				num5 = CSng(If(([next].transform.position.y <= MyBase.transform.position.y), (-90), 90))
			End If
			Dim num6 As Integer = Me.animationPicker
			If num6 <> 0 Then
				If num6 <> 1 Then
					Me.projectilePrefab.Create(Me.projectileRoot.position, num5 + num3, Me.enemyProperties.BSpeed).GetComponent(Of Animator)().Play("Bullet_3")
				Else
					Me.projectilePrefab.Create(Me.projectileRoot.position, num5 + num3, Me.enemyProperties.BSpeed).GetComponent(Of Animator)().Play("Bullet_2")
				End If
			Else
				Me.projectilePrefab.Create(Me.projectileRoot.position, num5 + num3, Me.enemyProperties.BSpeed).GetComponent(Of Animator)().Play("Bullet_1")
			End If
		Next
	End Sub

	' Token: 0x0600208C RID: 8332 RVA: 0x0012BFE4 File Offset: 0x0012A3E4
	Private Sub FireSingle()
		Dim [next] As AbstractPlayerController = PlayerManager.GetNext()
		Dim num As Single = [next].transform.position.x - MyBase.transform.position.x
		Dim num2 As Single = [next].transform.position.y - MyBase.transform.position.y
		Dim num3 As Single = -3F
		Dim num4 As Single = Mathf.Atan2(num2, num) * 57.29578F
		Dim effect As Effect = Global.UnityEngine.[Object].Instantiate(Of Effect)(Me.bulletEffect)
		effect.transform.position = Me.projectileRoot.transform.position
		effect.GetComponent(Of Animator)().SetInteger("PickAni", Global.UnityEngine.Random.Range(0, 3))
		If [next].transform.position.x > MyBase.transform.position.x Then
			num4 = CSng(If(([next].transform.position.y <= MyBase.transform.position.y), (-90), 90))
		End If
		If Not Me.parryable Then
			Me.animationPicker = Global.UnityEngine.Random.Range(0, 3)
		Else
			Me.animationPicker = Global.UnityEngine.Random.Range(0, 2)
		End If
		If Not Me.parryable Then
			Dim num5 As Integer = Me.animationPicker
			If num5 <> 0 Then
				If num5 <> 1 Then
					Me.projectilePrefab.Create(Me.projectileRoot.position, num4 + num3, Me.enemyProperties.ASpeed).GetComponent(Of Animator)().Play("Bullet_3")
				Else
					Me.projectilePrefab.Create(Me.projectileRoot.position, num4 + num3, Me.enemyProperties.ASpeed).GetComponent(Of Animator)().Play("Bullet_2")
				End If
			Else
				Me.projectilePrefab.Create(Me.projectileRoot.position, num4 + num3, Me.enemyProperties.ASpeed).GetComponent(Of Animator)().Play("Bullet_1")
			End If
		Else
			Dim num6 As Integer = Me.animationPicker
			If num6 <> 0 Then
				Me.parryablePrefab.Create(Me.projectileRoot.position, num4 + num3, Me.enemyProperties.ASpeed).GetComponent(Of Animator)().Play("Bullet_2")
			Else
				Me.parryablePrefab.Create(Me.projectileRoot.position, num4 + num3, Me.enemyProperties.ASpeed).GetComponent(Of Animator)().Play("Bullet_1")
			End If
		End If
	End Sub

	' Token: 0x0600208D RID: 8333 RVA: 0x0012C2AD File Offset: 0x0012A6AD
	Private Sub FlipEnemy()
		MyBase.GetComponent(Of SpriteRenderer)().flipX = Not MyBase.GetComponent(Of SpriteRenderer)().flipX
	End Sub

	' Token: 0x0600208E RID: 8334 RVA: 0x0012C2C8 File Offset: 0x0012A6C8
	Private Iterator Function dying_cr() As IEnumerator
		MyBase.GetComponent(Of Collider2D)().enabled = False
		AudioManager.Play("level_flying_blimp_cannon_ship_death")
		MyBase.animator.Play("Enemy_Explode")
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Enemy_Explode", False, True)
		Me.Die()
		Return
	End Function

	' Token: 0x0600208F RID: 8335 RVA: 0x0012C2E3 File Offset: 0x0012A6E3
	Private Sub Die()
		Me.state = FlyingBlimpLevelEnemy.State.Unspawned
		Me.StopAllCoroutines()
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub

	' Token: 0x04002900 RID: 10496
	Private enemyProperties As LevelProperties.FlyingBlimp.Enemy

	' Token: 0x04002901 RID: 10497
	Private properties As LevelProperties.FlyingBlimp

	' Token: 0x04002902 RID: 10498
	Private startPoint As Vector3

	' Token: 0x04002903 RID: 10499
	<SerializeField()>
	Private bulletEffect As Effect

	' Token: 0x04002904 RID: 10500
	<SerializeField()>
	Private deathPieces As FlyingBlimpLevelEnemyDeathPart()

	' Token: 0x04002905 RID: 10501
	<SerializeField()>
	Private projectilePrefab As FlyingBlimpLevelEnemyProjectile

	' Token: 0x04002906 RID: 10502
	<SerializeField()>
	Private parryablePrefab As FlyingBlimpLevelEnemyProjectile

	' Token: 0x04002907 RID: 10503
	<SerializeField()>
	Private projectileRoot As Transform

	' Token: 0x04002908 RID: 10504
	Private player As AbstractPlayerController

	' Token: 0x04002909 RID: 10505
	Private parent As FlyingBlimpLevelBlimpLady

	' Token: 0x0400290A RID: 10506
	Private damageDealer As DamageDealer

	' Token: 0x0400290B RID: 10507
	Private damageReceiver As DamageReceiver

	' Token: 0x0400290C RID: 10508
	Private hp As Single

	' Token: 0x0400290D RID: 10509
	Private stopPoint As Single

	' Token: 0x0400290E RID: 10510
	Private parryable As Boolean

	' Token: 0x0400290F RID: 10511
	Private animationPicker As Integer

	' Token: 0x02000634 RID: 1588
	Public Enum State
		' Token: 0x04002911 RID: 10513
		Unspawned
		' Token: 0x04002912 RID: 10514
		Spawned
	End Enum
End Class
