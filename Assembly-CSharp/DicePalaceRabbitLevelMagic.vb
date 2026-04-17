Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020005DE RID: 1502
Public Class DicePalaceRabbitLevelMagic
	Inherits AbstractProjectile

	' Token: 0x1700036F RID: 879
	' (get) Token: 0x06001DA9 RID: 7593 RVA: 0x00110D28 File Offset: 0x0010F128
	' (set) Token: 0x06001DAA RID: 7594 RVA: 0x00110D30 File Offset: 0x0010F130
	Public Property AppearTime As Single

	' Token: 0x06001DAB RID: 7595 RVA: 0x00110D39 File Offset: 0x0010F139
	Protected Overrides Sub Start()
		MyBase.Start()
		Me.initialPosition = MyBase.transform.position
		Me.idleRoutine = Me.wait_activation_cr()
		MyBase.StartCoroutine(Me.idleRoutine)
		MyBase.StartCoroutine(Me.FadeIn())
	End Sub

	' Token: 0x06001DAC RID: 7596 RVA: 0x00110D78 File Offset: 0x0010F178
	Protected Overrides Sub Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
		MyBase.Update()
	End Sub

	' Token: 0x06001DAD RID: 7597 RVA: 0x00110D96 File Offset: 0x0010F196
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
		MyBase.OnCollisionPlayer(hit, phase)
	End Sub

	' Token: 0x06001DAE RID: 7598 RVA: 0x00110DB4 File Offset: 0x0010F1B4
	Public Overrides Sub SetParryable(parryable As Boolean)
		MyBase.SetParryable(parryable)
		MyBase.animator.SetBool("CanParry", parryable)
	End Sub

	' Token: 0x06001DAF RID: 7599 RVA: 0x00110DD0 File Offset: 0x0010F1D0
	Public Sub ActivateOrb()
		Me.circleCollider.enabled = True
		Dim color As Color = Me.spriteRenderer.color
		color.a = 1F
		Me.spriteRenderer.color = color
		MyBase.StopCoroutine(Me.idleRoutine)
		MyBase.transform.position = Me.initialPosition
		MyBase.animator.SetTrigger("Attack")
		If Not Me.StartMagicLaserSFX Then
			AudioManager.Play("projectile_laser")
			Me.emitAudioFromObject.Add("projectile_laser")
			Me.StartMagicLaserSFX = True
		End If
	End Sub

	' Token: 0x06001DB0 RID: 7600 RVA: 0x00110E66 File Offset: 0x0010F266
	Public Sub Move(startY As Single, down As Boolean, speed As Single)
		MyBase.StartCoroutine(Me.move_cr(startY, down, speed))
	End Sub

	' Token: 0x06001DB1 RID: 7601 RVA: 0x00110E78 File Offset: 0x0010F278
	Private Iterator Function wait_activation_cr() As IEnumerator
		While True
			Dim vector As Vector3 = New Vector3(CSng(Global.UnityEngine.Random.Range(-1, 1)), CSng(Global.UnityEngine.Random.Range(-1, 1)), 0F)
			Dim newDirection As Vector3 = vector.normalized * 10F
			Dim progress As Single = 0F
			While progress < 0.1F
				MyBase.transform.position += newDirection * CupheadTime.Delta
				progress += CupheadTime.Delta
				Yield Nothing
			End While
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06001DB2 RID: 7602 RVA: 0x00110E94 File Offset: 0x0010F294
	Private Iterator Function move_cr(startY As Single, down As Boolean, speed As Single) As IEnumerator
		Me.StartMagicSFX = False
		Me.StartMagicLaserSFX = False
		Dim velocity As Vector3 = speed * If((Not down), Vector3.up, Vector3.down)
		Dim progress As Single = 0F
		While (Not down AndAlso startY + progress < 360F) OrElse (down AndAlso startY + progress > -360F)
			MyBase.transform.position += velocity * CupheadTime.Delta
			progress += velocity.y * CupheadTime.Delta
			Yield Nothing
		End While
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
		Return
	End Function

	' Token: 0x06001DB3 RID: 7603 RVA: 0x00110EC4 File Offset: 0x0010F2C4
	Private Iterator Function FadeIn() As IEnumerator
		If Not Me.StartMagicSFX Then
			AudioManager.Play("projectile_magic_start")
			Me.emitAudioFromObject.Add("projectile_magic_start")
			Me.StartMagicSFX = True
		End If
		While Me.spriteRenderer.color.a < 1F
			Dim c As Color = Me.spriteRenderer.color
			c.a += CupheadTime.Delta / Me.AppearTime
			Me.spriteRenderer.color = c
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06001DB4 RID: 7604 RVA: 0x00110EDF File Offset: 0x0010F2DF
	Public Sub SetSuit(suit As Integer)
		MyBase.animator.SetInteger("Suit", suit)
	End Sub

	' Token: 0x06001DB5 RID: 7605 RVA: 0x00110EF2 File Offset: 0x0010F2F2
	Public Sub IsOffset(offset As Boolean)
		MyBase.animator.SetFloat("CycleOffset", If((Not offset), 0F, 0.5F))
	End Sub

	' Token: 0x0400268C RID: 9868
	Private Const IdleSpeed As Single = 10F

	' Token: 0x0400268D RID: 9869
	<SerializeField()>
	Private spriteRenderer As SpriteRenderer

	' Token: 0x0400268E RID: 9870
	<SerializeField()>
	Private circleCollider As CircleCollider2D

	' Token: 0x0400268F RID: 9871
	Private idleRoutine As IEnumerator

	' Token: 0x04002690 RID: 9872
	Private initialPosition As Vector3

	' Token: 0x04002692 RID: 9874
	Private StartMagicSFX As Boolean

	' Token: 0x04002693 RID: 9875
	Private StartMagicLaserSFX As Boolean
End Class
