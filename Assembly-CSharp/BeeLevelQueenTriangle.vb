Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000520 RID: 1312
Public Class BeeLevelQueenTriangle
	Inherits AbstractProjectile

	' Token: 0x06001785 RID: 6021 RVA: 0x000D3DF8 File Offset: 0x000D21F8
	Public Function Create(properties As BeeLevelQueenTriangle.Properties) As BeeLevelQueenTriangle
		Dim beeLevelQueenTriangle As BeeLevelQueenTriangle = TryCast(MyBase.Create(), BeeLevelQueenTriangle)
		beeLevelQueenTriangle.transform.position = properties.player.center
		beeLevelQueenTriangle.transform.SetEulerAngles(New Single?(0F), New Single?(0F), New Single?(CSng(Global.UnityEngine.Random.Range(0, 360))))
		beeLevelQueenTriangle.properties = properties
		Return beeLevelQueenTriangle
	End Function

	' Token: 0x1700032B RID: 811
	' (get) Token: 0x06001786 RID: 6022 RVA: 0x000D3E5F File Offset: 0x000D225F
	Protected Overrides ReadOnly Property DestroyLifetime As Single
		Get
			Return 100F
		End Get
	End Property

	' Token: 0x06001787 RID: 6023 RVA: 0x000D3E68 File Offset: 0x000D2268
	Protected Overrides Sub Awake()
		MyBase.Awake()
		If Not Me.isInvincible Then
			Me.damageReceiver = MyBase.GetComponent(Of DamageReceiver)()
			AddHandler Me.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
		End If
		AudioManager.Play("bee_queen_triangle_spawn")
		Me.emitAudioFromObject.Add("bee_queen_triangle_spawn")
		AudioManager.PlayLoop("bee_queen_triangle_loop")
		Me.emitAudioFromObject.Add("bee_queen_triangle_loop")
	End Sub

	' Token: 0x06001788 RID: 6024 RVA: 0x000D3EDD File Offset: 0x000D22DD
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x06001789 RID: 6025 RVA: 0x000D3EFB File Offset: 0x000D22FB
	Protected Overrides Sub Start()
		MyBase.Start()
		MyBase.StartCoroutine(Me.go_cr())
	End Sub

	' Token: 0x0600178A RID: 6026 RVA: 0x000D3F10 File Offset: 0x000D2310
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		If Not Me.isInvincible Then
			RemoveHandler Me.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
		End If
		Me.childPrefab = Nothing
		Me.childPrefabInvincible = Nothing
	End Sub

	' Token: 0x0600178B RID: 6027 RVA: 0x000D3F48 File Offset: 0x000D2348
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		Me.properties.health -= info.damage
		If Me.properties.health <= 0F Then
			Me.Die()
		End If
	End Sub

	' Token: 0x0600178C RID: 6028 RVA: 0x000D3F7D File Offset: 0x000D237D
	Protected Overrides Sub Die()
		MyBase.Die()
		AudioManager.[Stop]("bee_queen_triangle_loop")
		MyBase.GetComponent(Of Collider2D)().enabled = False
		Me.StopAllCoroutines()
	End Sub

	' Token: 0x0600178D RID: 6029 RVA: 0x000D3FA4 File Offset: 0x000D23A4
	Protected Overrides Sub Update()
		MyBase.Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
		If Me.properties IsNot Nothing Then
			MyBase.transform.AddPosition(Me.forward.x * CupheadTime.Delta * Me.properties.speed, Me.forward.y * CupheadTime.Delta * Me.properties.speed, 0F)
			MyBase.transform.AddEulerAngles(0F, 0F, Me.properties.rotationSpeed * CSng(Me.properties.direction) * CupheadTime.Delta)
		End If
	End Sub

	' Token: 0x0600178E RID: 6030 RVA: 0x000D4064 File Offset: 0x000D2464
	Private Iterator Function go_cr() As IEnumerator
		MyBase.transform.GetComponent(Of Collider2D)().enabled = False
		Dim aim As Transform = New GameObject("Aim").transform
		aim.SetParent(MyBase.transform)
		aim.ResetLocalTransforms()
		Yield MyBase.StartCoroutine(Me.tweenColor_cr(New Color(0F, 0F, 0F, 0F), New Color(0F, 0F, 0F, 1F), Me.properties.introTime / 2F))
		Yield MyBase.StartCoroutine(Me.tweenColor_cr(New Color(0F, 0F, 0F, 1F), New Color(1F, 1F, 1F, 1F), Me.properties.introTime / 2F))
		aim.LookAt2D(Me.properties.player.center)
		Me.forward = aim.transform.right
		MyBase.transform.GetComponent(Of Collider2D)().enabled = True
		MyBase.StartCoroutine(Me.shoot_cr())
		Dim t As Single = 0F
		While t < 1F
			Dim val As Single = t / 1F
			Me.properties.speed = Mathf.Lerp(0F, Me.properties.speedMax, val)
			Me.properties.rotationSpeed = Mathf.Lerp(0F, Me.properties.rotationSpeedMax, val)
			t += CupheadTime.Delta
			Yield Nothing
		End While
		Me.properties.speed = Me.properties.speedMax
		Me.properties.rotationSpeed = Me.properties.rotationSpeedMax
		Return
	End Function

	' Token: 0x0600178F RID: 6031 RVA: 0x000D4080 File Offset: 0x000D2480
	Private Iterator Function shoot_cr() As IEnumerator
		Dim count As Integer = 0
		While count < Me.properties.childCount
			AudioManager.Play("bee_queen_triangle_shoot")
			Me.emitAudioFromObject.Add("bee_queen_triangle_shoot")
			For Each transform As Transform In Me.roots
				If Me.properties.damageable Then
					Me.childPrefab.Create(transform.position, transform.eulerAngles.z, Me.properties.childSpeed, Me.properties.childHealth).SetParryable(True)
				Else
					Me.childPrefabInvincible.Create(transform.position, transform.eulerAngles.z, Me.properties.childSpeed).SetParryable(True)
				End If
			Next
			MyBase.animator.Play("Attack")
			count += 1
			Yield CupheadTime.WaitForSeconds(Me, Me.properties.childDelay)
		End While
		MyBase.animator.Play("Idle")
		Return
	End Function

	' Token: 0x06001790 RID: 6032 RVA: 0x000D409C File Offset: 0x000D249C
	Private Iterator Function tweenColor_cr(start As Color, [end] As Color, time As Single) As IEnumerator
		Dim r As SpriteRenderer = MyBase.GetComponent(Of SpriteRenderer)()
		r.color = start
		Yield Nothing
		Dim t As Single = 0F
		While t < time
			Dim val As Single = t / time
			r.color = Color.Lerp(start, [end], val)
			t += CupheadTime.Delta
			Yield Nothing
		End While
		r.color = [end]
		Yield Nothing
		Return
	End Function

	' Token: 0x040020B6 RID: 8374
	<SerializeField()>
	Private isInvincible As Boolean

	' Token: 0x040020B7 RID: 8375
	<SerializeField()>
	Private roots As Transform()

	' Token: 0x040020B8 RID: 8376
	<SerializeField()>
	Private childPrefab As BasicDamagableProjectile

	' Token: 0x040020B9 RID: 8377
	<SerializeField()>
	Private childPrefabInvincible As BasicProjectile

	' Token: 0x040020BA RID: 8378
	Private properties As BeeLevelQueenTriangle.Properties

	' Token: 0x040020BB RID: 8379
	Private forward As Vector2

	' Token: 0x040020BC RID: 8380
	Private damageReceiver As DamageReceiver

	' Token: 0x02000521 RID: 1313
	Public Class Properties
		' Token: 0x06001791 RID: 6033 RVA: 0x000D40CC File Offset: 0x000D24CC
		Public Sub New(player As AbstractPlayerController, introTime As Single, speed As Single, rotationSpeed As Single, health As Single, childSpeed As Single, childDelay As Single, childHealth As Single, childCount As Integer, damageable As Boolean)
			Me.player = player
			Me.damageable = damageable
			Me.introTime = introTime
			Me.speedMax = speed
			Me.rotationSpeedMax = rotationSpeed
			Me.healthMax = health
			Me.childSpeed = childSpeed
			Me.childDelay = childDelay
			Me.childHealth = childHealth
			Me.childCount = childCount
			Me.direction = MathUtils.PlusOrMinus()
			Me.speed = 0F
			Me.rotationSpeed = 0F
			Me.health = health
		End Sub

		' Token: 0x040020BD RID: 8381
		Public player As AbstractPlayerController

		' Token: 0x040020BE RID: 8382
		Public damageable As Boolean

		' Token: 0x040020BF RID: 8383
		Public introTime As Single

		' Token: 0x040020C0 RID: 8384
		Public speedMax As Single

		' Token: 0x040020C1 RID: 8385
		Public rotationSpeedMax As Single

		' Token: 0x040020C2 RID: 8386
		Public healthMax As Single

		' Token: 0x040020C3 RID: 8387
		Public childSpeed As Single

		' Token: 0x040020C4 RID: 8388
		Public childDelay As Single

		' Token: 0x040020C5 RID: 8389
		Public childHealth As Single

		' Token: 0x040020C6 RID: 8390
		Public childCount As Integer

		' Token: 0x040020C7 RID: 8391
		Public direction As Integer

		' Token: 0x040020C8 RID: 8392
		Public speed As Single

		' Token: 0x040020C9 RID: 8393
		Public rotationSpeed As Single

		' Token: 0x040020CA RID: 8394
		Public health As Single
	End Class
End Class
