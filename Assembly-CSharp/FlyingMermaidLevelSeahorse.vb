Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000699 RID: 1689
Public Class FlyingMermaidLevelSeahorse
	Inherits AbstractCollidableObject

	' Token: 0x170003A5 RID: 933
	' (get) Token: 0x060023CF RID: 9167 RVA: 0x001506DE File Offset: 0x0014EADE
	' (set) Token: 0x060023D0 RID: 9168 RVA: 0x001506E6 File Offset: 0x0014EAE6
	Public Property state As FlyingMermaidLevelSeahorse.State

	' Token: 0x060023D1 RID: 9169 RVA: 0x001506F0 File Offset: 0x0014EAF0
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.spray.enabled = False
		MyBase.StartCoroutine(Me.intro_cr())
		Me.damageDealer = DamageDealer.NewEnemy()
		Me.damageReceiver = MyBase.GetComponent(Of DamageReceiver)()
		AddHandler Me.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
		Dim vector As Vector2 = MyBase.transform.position
		vector.y = Me.spawnY
		MyBase.transform.position = vector
	End Sub

	' Token: 0x060023D2 RID: 9170 RVA: 0x00150779 File Offset: 0x0014EB79
	Private Sub Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x060023D3 RID: 9171 RVA: 0x00150794 File Offset: 0x0014EB94
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		Me.hp -= info.damage
		If Me.hp < 0F AndAlso Me.state <> FlyingMermaidLevelSeahorse.State.Dying Then
			AudioManager.Play("level_mermaid_seahorse_death")
			Me.state = FlyingMermaidLevelSeahorse.State.Dying
			Me.StopAllCoroutines()
			MyBase.StartCoroutine(Me.die_cr())
		End If
	End Sub

	' Token: 0x060023D4 RID: 9172 RVA: 0x001507F4 File Offset: 0x0014EBF4
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x060023D5 RID: 9173 RVA: 0x00150814 File Offset: 0x0014EC14
	Public Sub Init(properties As LevelProperties.FlyingMermaid.Seahorse)
		Me.properties = properties
		Dim component As GroundHomingMovement = MyBase.GetComponent(Of GroundHomingMovement)()
		component.acceleration = properties.acceleration
		component.maxSpeed = properties.maxSpeed
		component.bounceRatio = properties.bounceRatio
		Me.hp = properties.hp
	End Sub

	' Token: 0x060023D6 RID: 9174 RVA: 0x00150860 File Offset: 0x0014EC60
	Private Iterator Function die_cr() As IEnumerator
		Me.state = FlyingMermaidLevelSeahorse.State.Dying
		Dim homer As GroundHomingMovement = MyBase.GetComponent(Of GroundHomingMovement)()
		Dim collider As Collider2D = MyBase.GetComponent(Of Collider2D)()
		homer.enabled = False
		collider.enabled = False
		MyBase.animator.SetTrigger("SprayDeath")
		Me.spray.[End]()
		AudioManager.Play("level_mermaid_seahorse_death")
		MyBase.animator.SetTrigger("OnDeath")
		Dim deathFx As Transform = Global.UnityEngine.[Object].Instantiate(Of Transform)(Me.deathFxPrefab)
		deathFx.SetParent(Me.deathFxRoot)
		deathFx.ResetLocalTransforms()
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

	' Token: 0x060023D7 RID: 9175 RVA: 0x0015087C File Offset: 0x0014EC7C
	Private Iterator Function intro_cr() As IEnumerator
		Dim homer As GroundHomingMovement = MyBase.GetComponent(Of GroundHomingMovement)()
		Dim collider As Collider2D = MyBase.GetComponent(Of Collider2D)()
		homer.enabled = False
		collider.enabled = False
		Dim t As Single = 0F
		While t < Me.riseTime
			t += CupheadTime.Delta
			Dim position As Vector2 = MyBase.transform.localPosition
			position.y += Me.riseDistance / Me.riseTime * CupheadTime.Delta
			MyBase.transform.localPosition = position
			Yield Nothing
		End While
		AudioManager.Play("level_mermaid_seahorse_intro")
		Dim animator As Animator = MyBase.GetComponent(Of Animator)()
		animator.SetTrigger("Continue")
		Yield animator.WaitForAnimationToStart(Me, "Spit_Start", False)
		Me.spray.enabled = True
		Me.spray.Init(Me.properties)
		Yield animator.WaitForAnimationToEnd(Me, "Spit_Start", False, True)
		Me.state = FlyingMermaidLevelSeahorse.State.Spit
		MyBase.StartCoroutine(Me.spit_cr())
		Return
	End Function

	' Token: 0x060023D8 RID: 9176 RVA: 0x00150898 File Offset: 0x0014EC98
	Private Iterator Function spit_cr() As IEnumerator
		AudioManager.Play("level_mermaid_seahorse_spit")
		Dim homer As GroundHomingMovement = MyBase.GetComponent(Of GroundHomingMovement)()
		Dim collider As Collider2D = MyBase.GetComponent(Of Collider2D)()
		homer.enabled = True
		collider.enabled = True
		Dim t As Single = 0F
		While True
			t += CupheadTime.Delta
			If t > Me.properties.homingDuration OrElse CType(Level.Current, FlyingMermaidLevel).MerdusaTransformStarted Then
				Exit For
			End If
			Yield Nothing
		End While
		homer.EnableHoming = False
		homer.bounceEnabled = False
		MyBase.animator.SetTrigger("SprayDeath")
		Me.spray.[End]()
		Return
	End Function

	' Token: 0x04002C92 RID: 11410
	<SerializeField()>
	Private spawnY As Single

	' Token: 0x04002C93 RID: 11411
	<SerializeField()>
	Private riseTime As Single

	' Token: 0x04002C94 RID: 11412
	<SerializeField()>
	Private riseDistance As Single

	' Token: 0x04002C95 RID: 11413
	<SerializeField()>
	Private deathStayTime As Single

	' Token: 0x04002C96 RID: 11414
	<SerializeField()>
	Private deathMoveTime As Single

	' Token: 0x04002C97 RID: 11415
	<SerializeField()>
	Private deathMoveDistance As Single

	' Token: 0x04002C98 RID: 11416
	<SerializeField()>
	Private spray As FlyingMermaidLevelSeahorseSpray

	' Token: 0x04002C99 RID: 11417
	<SerializeField()>
	Private deathFxRoot As Transform

	' Token: 0x04002C9A RID: 11418
	<SerializeField()>
	Private deathFxPrefab As Transform

	' Token: 0x04002C9B RID: 11419
	Private damageDealer As DamageDealer

	' Token: 0x04002C9C RID: 11420
	Private damageReceiver As DamageReceiver

	' Token: 0x04002C9D RID: 11421
	Private properties As LevelProperties.FlyingMermaid.Seahorse

	' Token: 0x04002C9E RID: 11422
	Private hp As Single

	' Token: 0x0200069A RID: 1690
	Public Enum State
		' Token: 0x04002CA0 RID: 11424
		Intro
		' Token: 0x04002CA1 RID: 11425
		Spit
		' Token: 0x04002CA2 RID: 11426
		Dying
	End Enum
End Class
