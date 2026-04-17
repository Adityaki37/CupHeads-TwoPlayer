Imports System
Imports UnityEngine

' Token: 0x020004E9 RID: 1257
Public Class BaronessLevelCandyCornMini
	Inherits AbstractProjectile

	' Token: 0x1700031F RID: 799
	' (get) Token: 0x060015D3 RID: 5587 RVA: 0x000C42FE File Offset: 0x000C26FE
	' (set) Token: 0x060015D4 RID: 5588 RVA: 0x000C4306 File Offset: 0x000C2706
	Public Property state As BaronessLevelCandyCornMini.State

	' Token: 0x060015D5 RID: 5589 RVA: 0x000C430F File Offset: 0x000C270F
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.damageReceiver = MyBase.GetComponent(Of DamageReceiver)()
		AddHandler Me.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
	End Sub

	' Token: 0x060015D6 RID: 5590 RVA: 0x000C433A File Offset: 0x000C273A
	Protected Overrides Sub Start()
		MyBase.Start()
		MyBase.GetComponent(Of SpriteRenderer)().flipX = Rand.Bool()
	End Sub

	' Token: 0x060015D7 RID: 5591 RVA: 0x000C4352 File Offset: 0x000C2752
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x060015D8 RID: 5592 RVA: 0x000C4370 File Offset: 0x000C2770
	Public Sub Init(pos As Vector2, speed As Single, health As Single)
		Me.speed = speed
		MyBase.transform.position = pos
		Me.health = health
		Me.state = BaronessLevelCandyCornMini.State.Spawned
	End Sub

	' Token: 0x060015D9 RID: 5593 RVA: 0x000C4398 File Offset: 0x000C2798
	Protected Overrides Sub Update()
		MyBase.Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x060015DA RID: 5594 RVA: 0x000C43B8 File Offset: 0x000C27B8
	Protected Overrides Sub FixedUpdate()
		MyBase.FixedUpdate()
		Dim num As Single = 2F
		Dim position As Vector3 = MyBase.transform.position
		position.y = Mathf.MoveTowards(MyBase.transform.position.y, 720F + num, Me.speed * CupheadTime.FixedDelta * Me.hitPauseCoefficient())
		MyBase.transform.position = position
		If MyBase.transform.position.y = 720F + num Then
			Me.Die()
		End If
	End Sub

	' Token: 0x060015DB RID: 5595 RVA: 0x000C4447 File Offset: 0x000C2847
	Private Function hitPauseCoefficient() As Single
		Return If((Not MyBase.GetComponent(Of DamageReceiver)().IsHitPaused), 1F, 0F)
	End Function

	' Token: 0x060015DC RID: 5596 RVA: 0x000C4468 File Offset: 0x000C2868
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		Me.health -= info.damage
		If Me.health < 0F Then
			Me.deathEffect.Create(MyBase.transform.position)
			Me.Die()
		End If
	End Sub

	' Token: 0x060015DD RID: 5597 RVA: 0x000C44B5 File Offset: 0x000C28B5
	Protected Overrides Sub Die()
		MyBase.Die()
		Me.state = BaronessLevelCandyCornMini.State.Unspawned
		Me.StopAllCoroutines()
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub

	' Token: 0x060015DE RID: 5598 RVA: 0x000C44D5 File Offset: 0x000C28D5
	Private Sub SoundCandyCornMiniBite()
		AudioManager.Play("level_baroness_candycorn_mini_bite")
		Me.emitAudioFromObject.Add("level_baroness_candycorn_mini_bite")
	End Sub

	' Token: 0x060015DF RID: 5599 RVA: 0x000C44F1 File Offset: 0x000C28F1
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me.deathEffect = Nothing
	End Sub

	' Token: 0x04001F27 RID: 7975
	<SerializeField()>
	Private deathEffect As Effect

	' Token: 0x04001F29 RID: 7977
	Private speed As Single

	' Token: 0x04001F2A RID: 7978
	Private health As Single

	' Token: 0x04001F2B RID: 7979
	Private lastPos As Vector3

	' Token: 0x04001F2C RID: 7980
	Private distFromLeaderX As Vector3

	' Token: 0x04001F2D RID: 7981
	Private damageReceiver As DamageReceiver

	' Token: 0x020004EA RID: 1258
	Public Enum State
		' Token: 0x04001F2F RID: 7983
		Unspawned
		' Token: 0x04001F30 RID: 7984
		Spawned
		' Token: 0x04001F31 RID: 7985
		Dying
	End Enum
End Class
