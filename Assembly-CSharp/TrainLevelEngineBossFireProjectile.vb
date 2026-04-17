Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000814 RID: 2068
Public Class TrainLevelEngineBossFireProjectile
	Inherits AbstractProjectile

	' Token: 0x06002FF3 RID: 12275 RVA: 0x001C5E53 File Offset: 0x001C4253
	Public Sub Create(pos As Vector2, velocity As Vector2, gravity As Single)
		Me.InstantiatePrefab(Of TrainLevelEngineBossFireProjectile)().Init(pos, velocity, gravity)
	End Sub

	' Token: 0x06002FF4 RID: 12276 RVA: 0x001C5E64 File Offset: 0x001C4264
	Protected Overrides Sub Start()
		MyBase.Start()
		MyBase.animator.SetFloat("Direction", CSng(If((Me.velocity.x <= 0F), 1, (-1))))
		MyBase.animator.Play("Idle", 0, Global.UnityEngine.Random.value)
		MyBase.transform.eulerAngles = New Vector3(0F, 0F, CSng(Global.UnityEngine.Random.Range(0, 360)))
		Me.collider2d = MyBase.GetComponent(Of CircleCollider2D)()
		Me.spriteRenderer = MyBase.GetComponent(Of SpriteRenderer)()
		MyBase.StartCoroutine(Me.spawnTrail_cr())
	End Sub

	' Token: 0x06002FF5 RID: 12277 RVA: 0x001C5F08 File Offset: 0x001C4308
	Protected Overrides Sub Update()
		MyBase.Update()
		If Me.state = TrainLevelEngineBossFireProjectile.State.Moving Then
			MyBase.transform.AddPosition(Me.velocity.x * CupheadTime.Delta, Me.velocity.y * CupheadTime.Delta, 0F)
			Me.velocity.y = Me.velocity.y - Me.gravity * CupheadTime.Delta
			If MyBase.transform.position.y < -300F Then
				Me.Die()
			End If
		End If
	End Sub

	' Token: 0x06002FF6 RID: 12278 RVA: 0x001C5FAC File Offset: 0x001C43AC
	Protected Overrides Sub FixedUpdate()
		MyBase.FixedUpdate()
		Dim num As Integer = Me.collider2d.OverlapCollider(TrainLevelEngineBossFireProjectile.filter, TrainLevelEngineBossFireProjectile.buffer)
		For i As Integer = 0 To num - 1
			If TrainLevelEngineBossFireProjectile.buffer(i).GetComponent(Of TrainLevelPlatform)() Then
				Me.Die()
				Exit For
			End If
		Next
	End Sub

	' Token: 0x06002FF7 RID: 12279 RVA: 0x001C6008 File Offset: 0x001C4408
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
			Me.Die()
		End If
	End Sub

	' Token: 0x06002FF8 RID: 12280 RVA: 0x001C602C File Offset: 0x001C442C
	Protected Overrides Sub Die()
		MyBase.Die()
		MyBase.transform.rotation = Quaternion.identity
		Me.spriteRenderer.flipX = Rand.Bool()
		Me.state = TrainLevelEngineBossFireProjectile.State.Dead
	End Sub

	' Token: 0x06002FF9 RID: 12281 RVA: 0x001C605B File Offset: 0x001C445B
	Private Sub Init(pos As Vector2, velocity As Vector2, gravity As Single)
		MyBase.transform.position = pos
		Me.velocity = velocity
		Me.gravity = gravity
		Me.state = TrainLevelEngineBossFireProjectile.State.Moving
	End Sub

	' Token: 0x06002FFA RID: 12282 RVA: 0x001C6084 File Offset: 0x001C4484
	Public Iterator Function spawnTrail_cr() As IEnumerator
		While True
			Yield CupheadTime.WaitForSeconds(Me, 0.1F)
			Me.trailPrefab.Create(MyBase.transform.position + TrainLevelEngineBossFireProjectile.TrailOffset).Play()
		End While
		Return
	End Function

	' Token: 0x06002FFB RID: 12283 RVA: 0x001C609F File Offset: 0x001C449F
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me.trailPrefab = Nothing
	End Sub

	' Token: 0x040038CB RID: 14539
	Private Const IdleStateName As String = "Idle"

	' Token: 0x040038CC RID: 14540
	Private Const DirectionParameterName As String = "Direction"

	' Token: 0x040038CD RID: 14541
	Private Shared TrailOffset As Vector3 = New Vector3(0F, 10F, 0F)

	' Token: 0x040038CE RID: 14542
	Private Shared filter As ContactFilter2D = Nothing.NoFilter()

	' Token: 0x040038CF RID: 14543
	Private Shared buffer As Collider2D() = New Collider2D(9) {}

	' Token: 0x040038D0 RID: 14544
	<SerializeField()>
	Private trailPrefab As Effect

	' Token: 0x040038D1 RID: 14545
	Public Const GROUND_Y As Single = -300F

	' Token: 0x040038D2 RID: 14546
	Private state As TrainLevelEngineBossFireProjectile.State

	' Token: 0x040038D3 RID: 14547
	Private velocity As Vector2

	' Token: 0x040038D4 RID: 14548
	Private gravity As Single

	' Token: 0x040038D5 RID: 14549
	Private collider2d As CircleCollider2D

	' Token: 0x040038D6 RID: 14550
	Private spriteRenderer As SpriteRenderer

	' Token: 0x02000815 RID: 2069
	Public Enum State
		' Token: 0x040038D8 RID: 14552
		Init
		' Token: 0x040038D9 RID: 14553
		Moving
		' Token: 0x040038DA RID: 14554
		Dead
	End Enum
End Class
