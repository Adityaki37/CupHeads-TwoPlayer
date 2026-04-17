Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000697 RID: 1687
Public Class FlyingMermaidLevelPufferfish
	Inherits AbstractProjectile

	' Token: 0x170003A4 RID: 932
	' (get) Token: 0x060023C2 RID: 9154 RVA: 0x00150282 File Offset: 0x0014E682
	' (set) Token: 0x060023C3 RID: 9155 RVA: 0x0015028A File Offset: 0x0014E68A
	Public Property state As FlyingMermaidLevelPufferfish.State

	' Token: 0x060023C4 RID: 9156 RVA: 0x00150294 File Offset: 0x0014E694
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.damageReceiver = MyBase.GetComponent(Of DamageReceiver)()
		AddHandler Me.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
		Dim vector As Vector2 = MyBase.transform.position
		vector.y = Me.spawnY
		MyBase.transform.position = vector
		Me.SetParryable(Me.parryable)
		MyBase.animator.Play("Idle", 0, Global.UnityEngine.Random.Range(0F, 1F))
	End Sub

	' Token: 0x060023C5 RID: 9157 RVA: 0x00150325 File Offset: 0x0014E725
	Protected Overrides Sub Update()
		MyBase.Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x060023C6 RID: 9158 RVA: 0x00150344 File Offset: 0x0014E744
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		AudioManager.Play("level_mermaid_merdusa_puffer_fish_hit")
		Me.hp -= info.damage
		If Me.hp < 0F AndAlso Me.state <> FlyingMermaidLevelPufferfish.State.Dying Then
			Me.state = FlyingMermaidLevelPufferfish.State.Dying
			Me.StartDeath()
		End If
	End Sub

	' Token: 0x060023C7 RID: 9159 RVA: 0x00150397 File Offset: 0x0014E797
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If Me.state <> FlyingMermaidLevelPufferfish.State.Dying AndAlso phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x060023C8 RID: 9160 RVA: 0x001503C1 File Offset: 0x0014E7C1
	Public Sub Init(properties As LevelProperties.FlyingMermaid.Pufferfish)
		Me.properties = properties
		Me.hp = properties.hp
		MyBase.StartCoroutine(Me.loop_cr())
	End Sub

	' Token: 0x060023C9 RID: 9161 RVA: 0x001503E4 File Offset: 0x0014E7E4
	Private Iterator Function loop_cr() As IEnumerator
		Dim speed As Single = Me.properties.floatSpeed * Global.UnityEngine.Random.Range(0.9F, 1.1F)
		While True
			Dim position As Vector2 = MyBase.transform.position
			position.y += speed * CupheadTime.Delta
			MyBase.transform.position = position
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x060023CA RID: 9162 RVA: 0x001503FF File Offset: 0x0014E7FF
	Private Sub StartDeath()
		Me.StopAllCoroutines()
		MyBase.StartCoroutine(Me.dying_cr())
	End Sub

	' Token: 0x060023CB RID: 9163 RVA: 0x00150414 File Offset: 0x0014E814
	Private Iterator Function dying_cr() As IEnumerator
		MyBase.gameObject.tag = "EnemyProjectile"
		Me.deathFX.Create(MyBase.transform.position)
		MyBase.animator.Play("Death")
		Dim velocity As Single = 100F
		While MyBase.transform.position.y > -660F
			velocity += CupheadTime.Delta * 300F
			MyBase.transform.AddPosition(0F, (-velocity + Me.accumulatedGravity) * CupheadTime.Delta, 0F)
			Me.accumulatedGravity += -100F
			Yield Nothing
		End While
		Me.Die()
		Return
	End Function

	' Token: 0x060023CC RID: 9164 RVA: 0x0015042F File Offset: 0x0014E82F
	Protected Overrides Sub Die()
		MyBase.transform.GetComponent(Of SpriteRenderer)().enabled = False
		MyBase.Die()
	End Sub

	' Token: 0x060023CD RID: 9165 RVA: 0x00150448 File Offset: 0x0014E848
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me.deathFX = Nothing
	End Sub

	' Token: 0x04002C85 RID: 11397
	Private Const GRAVITY As Single = -100F

	' Token: 0x04002C87 RID: 11399
	<SerializeField()>
	Private deathFX As Effect

	' Token: 0x04002C88 RID: 11400
	<SerializeField()>
	Private spawnY As Single

	' Token: 0x04002C89 RID: 11401
	<SerializeField()>
	Private parryable As Boolean

	' Token: 0x04002C8A RID: 11402
	Private damageReceiver As DamageReceiver

	' Token: 0x04002C8B RID: 11403
	Private properties As LevelProperties.FlyingMermaid.Pufferfish

	' Token: 0x04002C8C RID: 11404
	Private hp As Single

	' Token: 0x04002C8D RID: 11405
	Private accumulatedGravity As Single

	' Token: 0x02000698 RID: 1688
	Public Enum State
		' Token: 0x04002C8F RID: 11407
		Idle
		' Token: 0x04002C90 RID: 11408
		Dying
	End Enum
End Class
