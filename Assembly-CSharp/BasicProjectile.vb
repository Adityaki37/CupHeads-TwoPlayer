Imports System
Imports UnityEngine

' Token: 0x02000AE7 RID: 2791
Public Class BasicProjectile
	Inherits AbstractProjectile

	' Token: 0x17000604 RID: 1540
	' (get) Token: 0x06004398 RID: 17304 RVA: 0x000B8149 File Offset: 0x000B6549
	Protected Overridable ReadOnly Property Direction As Vector3
		Get
			Return MyBase.transform.right
		End Get
	End Property

	' Token: 0x17000605 RID: 1541
	' (get) Token: 0x06004399 RID: 17305 RVA: 0x000B8156 File Offset: 0x000B6556
	Protected Overrides ReadOnly Property DestroyLifetime As Single
		Get
			Return 10F
		End Get
	End Property

	' Token: 0x17000606 RID: 1542
	' (get) Token: 0x0600439A RID: 17306 RVA: 0x000B815D File Offset: 0x000B655D
	Protected Overrides ReadOnly Property DestroyedAfterLeavingScreen As Boolean
		Get
			Return True
		End Get
	End Property

	' Token: 0x17000607 RID: 1543
	' (get) Token: 0x0600439B RID: 17307 RVA: 0x000B8160 File Offset: 0x000B6560
	Protected Overridable ReadOnly Property projectileMissImpactSFX As String
		Get
			Return "player_weapon_peashot_miss"
		End Get
	End Property

	' Token: 0x0600439C RID: 17308 RVA: 0x000B8168 File Offset: 0x000B6568
	Public Overridable Function Create(position As Vector2, rotation As Single, speed As Single) As BasicProjectile
		Dim basicProjectile As BasicProjectile = TryCast(Me.Create(position, rotation), BasicProjectile)
		basicProjectile.Speed = speed
		Return basicProjectile
	End Function

	' Token: 0x0600439D RID: 17309 RVA: 0x000B818C File Offset: 0x000B658C
	Public Overridable Function Create(position As Vector2, rotation As Single, scale As Vector2, speed As Single) As BasicProjectile
		Dim basicProjectile As BasicProjectile = TryCast(Me.Create(position, rotation, scale), BasicProjectile)
		basicProjectile.Speed = speed
		Return basicProjectile
	End Function

	' Token: 0x0600439E RID: 17310 RVA: 0x000B81B1 File Offset: 0x000B65B1
	Protected Overrides Sub Awake()
		MyBase.Awake()
		If MyBase.CompareTag("EnemyProjectile") Then
			Me.DamagesType.Player = True
		End If
	End Sub

	' Token: 0x0600439F RID: 17311 RVA: 0x000B81D5 File Offset: 0x000B65D5
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
	End Sub

	' Token: 0x060043A0 RID: 17312 RVA: 0x000B81DD File Offset: 0x000B65DD
	Protected Overrides Sub OnCollision(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollision(hit, phase)
	End Sub

	' Token: 0x060043A1 RID: 17313 RVA: 0x000B81E7 File Offset: 0x000B65E7
	Protected Overrides Sub OnCollisionOther(hit As GameObject, phase As CollisionPhase)
		If hit.tag = "Parry" Then
			Return
		End If
		MyBase.OnCollisionOther(hit, phase)
	End Sub

	' Token: 0x060043A2 RID: 17314 RVA: 0x000B8207 File Offset: 0x000B6607
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		If phase <> CollisionPhase.[Exit] Then
			Me.DealDamage(hit)
		End If
		MyBase.OnCollisionPlayer(hit, phase)
	End Sub

	' Token: 0x060043A3 RID: 17315 RVA: 0x000B821F File Offset: 0x000B661F
	Protected Overrides Sub OnCollisionEnemy(hit As GameObject, phase As CollisionPhase)
		If phase <> CollisionPhase.[Exit] Then
			Me.DealDamage(hit)
		End If
		MyBase.OnCollisionEnemy(hit, phase)
	End Sub

	' Token: 0x060043A4 RID: 17316 RVA: 0x000B8238 File Offset: 0x000B6638
	Protected Overrides Sub OnCollisionDie(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionDie(hit, phase)
		If MyBase.tag = "PlayerProjectile" AndAlso phase = CollisionPhase.Enter Then
			If(hit.GetComponent(Of DamageReceiver)() AndAlso hit.GetComponent(Of DamageReceiver)().enabled) OrElse (hit.GetComponent(Of DamageReceiverChild)() AndAlso hit.GetComponent(Of DamageReceiverChild)().enabled) Then
				AudioManager.Play("player_shoot_hit_cuphead")
			Else
				AudioManager.Play(Me.projectileMissImpactSFX)
			End If
		End If
	End Sub

	' Token: 0x060043A5 RID: 17317 RVA: 0x000B82C2 File Offset: 0x000B66C2
	Private Sub DealDamage(hit As GameObject)
		Me.damageDealer.DealDamage(hit)
	End Sub

	' Token: 0x060043A6 RID: 17318 RVA: 0x000B82D4 File Offset: 0x000B66D4
	Protected Overrides Sub Die()
		Me.move = False
		Dim component As EffectSpawner = MyBase.GetComponent(Of EffectSpawner)()
		If component IsNot Nothing Then
			Global.UnityEngine.[Object].Destroy(component)
		End If
		MyBase.Die()
	End Sub

	' Token: 0x060043A7 RID: 17319 RVA: 0x000B8307 File Offset: 0x000B6707
	Protected Overrides Sub FixedUpdate()
		MyBase.FixedUpdate()
		If Me.move Then
			Me.Move()
		End If
	End Sub

	' Token: 0x060043A8 RID: 17320 RVA: 0x000B8320 File Offset: 0x000B6720
	Protected Overridable Sub Move()
		MyBase.transform.position += Me.Direction * Me.Speed * CupheadTime.FixedDelta - New Vector3(0F, Me._accumulativeGravity * CupheadTime.FixedDelta, 0F)
		Me._accumulativeGravity += Me.Gravity * CupheadTime.FixedDelta
	End Sub

	' Token: 0x0400495B RID: 18779
	<Space(10F)>
	Public Speed As Single

	' Token: 0x0400495C RID: 18780
	Public Gravity As Single

	' Token: 0x0400495D RID: 18781
	<Space(10F)>
	Public SfxOnDeath As Sfx

	' Token: 0x0400495E RID: 18782
	Protected move As Boolean = True

	' Token: 0x0400495F RID: 18783
	Protected _accumulativeGravity As Single
End Class
