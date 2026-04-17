Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000591 RID: 1425
Public Class DevilLevelPitchforkRingProjectile
	Inherits AbstractProjectile

	' Token: 0x17000355 RID: 853
	' (get) Token: 0x06001B45 RID: 6981 RVA: 0x000FA1A9 File Offset: 0x000F85A9
	Protected Overrides ReadOnly Property DestroyedAfterLeavingScreen As Boolean
		Get
			Return True
		End Get
	End Property

	' Token: 0x17000356 RID: 854
	' (get) Token: 0x06001B46 RID: 6982 RVA: 0x000FA1AC File Offset: 0x000F85AC
	Protected Overrides ReadOnly Property DestroyLifetime As Single
		Get
			Return -1F
		End Get
	End Property

	' Token: 0x06001B47 RID: 6983 RVA: 0x000FA1B4 File Offset: 0x000F85B4
	Public Function Create(pos As Vector2, speed As Single, groundDuration As Single, parent As DevilLevelSittingDevil, waitTime As Single) As DevilLevelPitchforkRingProjectile
		Dim devilLevelPitchforkRingProjectile As DevilLevelPitchforkRingProjectile = Me.InstantiatePrefab(Of DevilLevelPitchforkRingProjectile)()
		devilLevelPitchforkRingProjectile.transform.position = pos
		devilLevelPitchforkRingProjectile.speed = speed
		devilLevelPitchforkRingProjectile.state = DevilLevelPitchforkRingProjectile.State.Idle
		devilLevelPitchforkRingProjectile.groundDuration = groundDuration
		devilLevelPitchforkRingProjectile.parent = parent
		devilLevelPitchforkRingProjectile.waitTime = waitTime
		devilLevelPitchforkRingProjectile.StartCoroutine(devilLevelPitchforkRingProjectile.wait_cr())
		Return devilLevelPitchforkRingProjectile
	End Function

	' Token: 0x06001B48 RID: 6984 RVA: 0x000FA20C File Offset: 0x000F860C
	Protected Overrides Sub Update()
		MyBase.Update()
		If Me.parent Is Nothing Then
			Me.Die()
		End If
	End Sub

	' Token: 0x06001B49 RID: 6985 RVA: 0x000FA22B File Offset: 0x000F862B
	Protected Overrides Sub Start()
		MyBase.Start()
		MyBase.GetComponent(Of Collider2D)().enabled = False
	End Sub

	' Token: 0x06001B4A RID: 6986 RVA: 0x000FA23F File Offset: 0x000F863F
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
		MyBase.OnCollisionPlayer(hit, phase)
	End Sub

	' Token: 0x06001B4B RID: 6987 RVA: 0x000FA260 File Offset: 0x000F8660
	Protected Overrides Sub FixedUpdate()
		MyBase.FixedUpdate()
		If Not Me.waitTimeUp Then
			Return
		End If
		If Not MyBase.dead AndAlso Me.state = DevilLevelPitchforkRingProjectile.State.Attacking Then
			If Not Me.soundPlayed Then
				Me.AttackSFX()
				Me.soundPlayed = True
			End If
			MyBase.transform.AddPosition(Me.velocity.x * CupheadTime.FixedDelta, Me.velocity.y * CupheadTime.FixedDelta, 0F)
			Dim radius As Single = MyBase.GetComponent(Of CircleCollider2D)().radius
		End If
	End Sub

	' Token: 0x06001B4C RID: 6988 RVA: 0x000FA2EC File Offset: 0x000F86EC
	Private Iterator Function wait_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, Me.waitTime)
		Me.waitTimeUp = True
		MyBase.GetComponent(Of Collider2D)().enabled = True
		MyBase.animator.SetTrigger("Continue")
		Return
	End Function

	' Token: 0x06001B4D RID: 6989 RVA: 0x000FA308 File Offset: 0x000F8708
	Public Sub Attack()
		If Not MyBase.dead Then
			Me.state = DevilLevelPitchforkRingProjectile.State.Attacking
			Me.velocity = Me.speed * (PlayerManager.GetNext().center - MyBase.transform.position).normalized
			MyBase.StartCoroutine(Me.main_cr())
		End If
	End Sub

	' Token: 0x06001B4E RID: 6990 RVA: 0x000FA36C File Offset: 0x000F876C
	Private Iterator Function main_cr() As IEnumerator
		While Me.state = DevilLevelPitchforkRingProjectile.State.Attacking
			Yield Nothing
		End While
		Yield CupheadTime.WaitForSeconds(Me, Me.groundDuration)
		Me.Die()
		Return
	End Function

	' Token: 0x06001B4F RID: 6991 RVA: 0x000FA387 File Offset: 0x000F8787
	Protected Overrides Sub Die()
		MyBase.Die()
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub

	' Token: 0x06001B50 RID: 6992 RVA: 0x000FA39A File Offset: 0x000F879A
	Public Overrides Sub SetParryable(parryable As Boolean)
		MyBase.SetParryable(parryable)
		MyBase.animator.SetBool("IsPink", parryable)
	End Sub

	' Token: 0x06001B51 RID: 6993 RVA: 0x000FA3B4 File Offset: 0x000F87B4
	Private Sub AttackSFX()
		AudioManager.Play("devil_ring_projectile")
		Me.emitAudioFromObject.Add("devil_ring_projectile")
	End Sub

	' Token: 0x04002481 RID: 9345
	Public state As DevilLevelPitchforkRingProjectile.State

	' Token: 0x04002482 RID: 9346
	Private velocity As Vector2

	' Token: 0x04002483 RID: 9347
	Private speed As Single

	' Token: 0x04002484 RID: 9348
	Private groundDuration As Single

	' Token: 0x04002485 RID: 9349
	Private parent As DevilLevelSittingDevil

	' Token: 0x04002486 RID: 9350
	Private waitTime As Single

	' Token: 0x04002487 RID: 9351
	Private waitTimeUp As Boolean

	' Token: 0x04002488 RID: 9352
	Private soundPlayed As Boolean

	' Token: 0x02000592 RID: 1426
	Public Enum State
		' Token: 0x0400248A RID: 9354
		Idle
		' Token: 0x0400248B RID: 9355
		Attacking
		' Token: 0x0400248C RID: 9356
		OnGround
	End Enum
End Class
