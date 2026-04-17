Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020006A0 RID: 1696
Public Class FlyingMermaidLevelTurtle
	Inherits AbstractCollidableObject

	' Token: 0x170003A7 RID: 935
	' (get) Token: 0x060023F3 RID: 9203 RVA: 0x001518EA File Offset: 0x0014FCEA
	' (set) Token: 0x060023F4 RID: 9204 RVA: 0x001518F2 File Offset: 0x0014FCF2
	Public Property state As FlyingMermaidLevelTurtle.State

	' Token: 0x060023F5 RID: 9205 RVA: 0x001518FC File Offset: 0x0014FCFC
	Protected Overrides Sub Awake()
		MyBase.Awake()
		MyBase.StartCoroutine(Me.intro_cr())
		Me.damageDealer = DamageDealer.NewEnemy()
		Me.damageReceiver = MyBase.GetComponent(Of DamageReceiver)()
		AddHandler Me.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
		Dim vector As Vector2 = MyBase.transform.position
		vector.y = Me.spawnY
		MyBase.transform.position = vector
	End Sub

	' Token: 0x060023F6 RID: 9206 RVA: 0x00151979 File Offset: 0x0014FD79
	Private Sub Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x060023F7 RID: 9207 RVA: 0x00151994 File Offset: 0x0014FD94
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		Me.hp -= info.damage
		If Me.hp < 0F AndAlso Me.state <> FlyingMermaidLevelTurtle.State.Dying Then
			Me.state = FlyingMermaidLevelTurtle.State.Dying
			Me.StopAllCoroutines()
			MyBase.StartCoroutine(Me.die_cr())
		End If
	End Sub

	' Token: 0x060023F8 RID: 9208 RVA: 0x001519EA File Offset: 0x0014FDEA
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x060023F9 RID: 9209 RVA: 0x00151A08 File Offset: 0x0014FE08
	Public Sub Init(properties As LevelProperties.FlyingMermaid.Turtle)
		Me.properties = properties
		Me.hp = properties.hp
	End Sub

	' Token: 0x060023FA RID: 9210 RVA: 0x00151A20 File Offset: 0x0014FE20
	Private Iterator Function die_cr() As IEnumerator
		AudioManager.Play("level_mermaid_turtle_flag")
		Me.state = FlyingMermaidLevelTurtle.State.Dying
		For Each collider2D As Collider2D In MyBase.GetComponents(Of Collider2D)()
			collider2D.enabled = False
		Next
		MyBase.animator.SetTrigger("OnDeath")
		Yield CupheadTime.WaitForSeconds(Me, Me.deathStayTime)
		Dim t As Single = 0F
		While t < Me.deathMoveTime
			t += CupheadTime.Delta
			Dim position As Vector2 = MyBase.transform.localPosition
			position.y -= Me.deathMoveDistance * CupheadTime.Delta / Me.deathMoveTime
			MyBase.transform.localPosition = position
			Yield Nothing
		End While
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
		Return
	End Function

	' Token: 0x060023FB RID: 9211 RVA: 0x00151A3C File Offset: 0x0014FE3C
	Private Iterator Function intro_cr() As IEnumerator
		AudioManager.Play("level_mermaid_turtle_enter")
		For Each collider2D As Collider2D In MyBase.GetComponents(Of Collider2D)()
			collider2D.enabled = False
		Next
		Dim t As Single = 0F
		While t < Me.riseTime
			t += CupheadTime.Delta
			Dim position As Vector2 = MyBase.transform.localPosition
			position.y += Me.riseDistance / Me.riseTime * CupheadTime.Delta
			MyBase.transform.localPosition = position
			Yield Nothing
		End While
		Dim animator As Animator = MyBase.GetComponent(Of Animator)()
		animator.SetTrigger("Continue")
		Yield animator.WaitForAnimationToEnd(Me, "Intro", False, True)
		Me.state = FlyingMermaidLevelTurtle.State.Idle
		MyBase.StartCoroutine(Me.pattern_cr())
		MyBase.StartCoroutine(Me.move_cr())
		For Each collider2D2 As Collider2D In MyBase.GetComponents(Of Collider2D)()
			collider2D2.enabled = True
		Next
		Return
	End Function

	' Token: 0x060023FC RID: 9212 RVA: 0x00151A58 File Offset: 0x0014FE58
	Private Iterator Function move_cr() As IEnumerator
		Dim sprite As SpriteRenderer = MyBase.GetComponent(Of SpriteRenderer)()
		While True
			If Me.moving Then
				Dim vector As Vector2 = MyBase.transform.localPosition
				vector.x -= Me.properties.speed * CupheadTime.Delta
				MyBase.transform.localPosition = vector
				If vector.x < CSng(Level.Current.Left) - sprite.bounds.size.x / 2F Then
					Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
				End If
			End If
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x060023FD RID: 9213 RVA: 0x00151A74 File Offset: 0x0014FE74
	Private Iterator Function pattern_cr() As IEnumerator
		Dim pattern As String() = Me.properties.explodeSpreadshotString.GetRandom().Split(New Char() { ","c })
		Yield CupheadTime.WaitForSeconds(Me, Me.properties.timeUntilShoot.RandomFloat())
		For i As Integer = 0 To pattern.Length - 1
			If pattern(i)(0) = "D"c Then
				Dim waitTime As Single = 0F
				Parser.FloatTryParse(pattern(i).Substring(1), waitTime)
				Yield CupheadTime.WaitForSeconds(Me, waitTime)
			ElseIf Not CType(Level.Current, FlyingMermaidLevel).MerdusaTransformStarted Then
				Me.currentExplodePattern = pattern(i)
				AudioManager.Play("level_mermaid_turtle_shell_pop")
				MyBase.animator.SetTrigger("Shoot")
				Yield MyBase.animator.WaitForAnimationToEnd(Me, "Idle", False, True)
				Me.moving = False
				Yield MyBase.animator.WaitForAnimationToEnd(Me, "Shoot_B", False, True)
				Me.moving = True
				AudioManager.Play("level_mermaid_turtle_post_cannon")
			End If
		Next
		Return
	End Function

	' Token: 0x060023FE RID: 9214 RVA: 0x00151A90 File Offset: 0x0014FE90
	Private Sub OnShootFX()
		Me.shootEffectPrefab.Create(Me.shootEffectRoot.position)
		Me.cannonBallPrefab.Create(Me.cannonBallRoot.transform.position, Me.currentExplodePattern, Me.properties)
	End Sub

	' Token: 0x04002CBC RID: 11452
	<SerializeField()>
	Private spawnY As Single

	' Token: 0x04002CBD RID: 11453
	<SerializeField()>
	Private riseTime As Single

	' Token: 0x04002CBE RID: 11454
	<SerializeField()>
	Private riseDistance As Single

	' Token: 0x04002CBF RID: 11455
	<SerializeField()>
	Private deathStayTime As Single

	' Token: 0x04002CC0 RID: 11456
	<SerializeField()>
	Private deathMoveTime As Single

	' Token: 0x04002CC1 RID: 11457
	<SerializeField()>
	Private deathMoveDistance As Single

	' Token: 0x04002CC2 RID: 11458
	<SerializeField()>
	Private cannonBallPrefab As FlyingMermaidLevelTurtleCannonBall

	' Token: 0x04002CC3 RID: 11459
	<SerializeField()>
	Private cannonBallRoot As Transform

	' Token: 0x04002CC4 RID: 11460
	<SerializeField()>
	Private shootEffectRoot As Transform

	' Token: 0x04002CC5 RID: 11461
	<SerializeField()>
	Private shootEffectPrefab As Effect

	' Token: 0x04002CC6 RID: 11462
	Private damageDealer As DamageDealer

	' Token: 0x04002CC7 RID: 11463
	Private damageReceiver As DamageReceiver

	' Token: 0x04002CC8 RID: 11464
	Private properties As LevelProperties.FlyingMermaid.Turtle

	' Token: 0x04002CC9 RID: 11465
	Private hp As Single

	' Token: 0x04002CCA RID: 11466
	Private moving As Boolean = True

	' Token: 0x04002CCB RID: 11467
	Private currentExplodePattern As String

	' Token: 0x020006A1 RID: 1697
	Public Enum State
		' Token: 0x04002CCD RID: 11469
		Intro
		' Token: 0x04002CCE RID: 11470
		Idle
		' Token: 0x04002CCF RID: 11471
		Dying
	End Enum
End Class
