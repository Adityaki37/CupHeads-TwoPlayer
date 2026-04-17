Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000787 RID: 1927
Public Class RumRunnersLevelBouncingBeetle
	Inherits AbstractProjectile

	' Token: 0x170003EA RID: 1002
	' (get) Token: 0x06002A7B RID: 10875 RVA: 0x0018D1DA File Offset: 0x0018B5DA
	' (set) Token: 0x06002A7C RID: 10876 RVA: 0x0018D1E2 File Offset: 0x0018B5E2
	Public Property leaveScreen As Boolean

	' Token: 0x170003EB RID: 1003
	' (get) Token: 0x06002A7D RID: 10877 RVA: 0x0018D1EB File Offset: 0x0018B5EB
	Protected Overrides ReadOnly Property DestroyLifetime As Single
		Get
			Return 0F
		End Get
	End Property

	' Token: 0x06002A7E RID: 10878 RVA: 0x0018D1F2 File Offset: 0x0018B5F2
	Protected Overrides Sub Start()
		MyBase.Start()
		Me.damageReceiver = MyBase.GetComponent(Of DamageReceiver)()
		AddHandler Me.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
		Me.initialScale = Me.visualTransform.localScale
	End Sub

	' Token: 0x06002A7F RID: 10879 RVA: 0x0018D22E File Offset: 0x0018B62E
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
		MyBase.OnCollisionPlayer(hit, phase)
	End Sub

	' Token: 0x06002A80 RID: 10880 RVA: 0x0018D24C File Offset: 0x0018B64C
	Public Overridable Function Init(pos As Vector2, velocity As Vector3, initialSpeed As Single, timeToSlowdown As Single, targetSpeed As Single, hp As Single) As RumRunnersLevelBouncingBeetle
		MyBase.ResetLifetime()
		MyBase.ResetDistance()
		Me.SetParryable(False)
		MyBase.transform.position = pos
		Me.velocity = velocity
		Me.currentSpeed = initialSpeed
		Me.initialSpeed = initialSpeed
		Me.targetSpeed = targetSpeed
		Me.currentSpeed = targetSpeed
		Me.slowdownDuration = timeToSlowdown
		Me.isMoving = True
		Me.hp = hp
		Me.offset = MyBase.GetComponent(Of Collider2D)().bounds.size.x / 2F
		Me.Move()
		Me.leaveScreen = False
		RumRunnersLevelBouncingBeetle.LastSortingIndex -= 1
		If RumRunnersLevelBouncingBeetle.LastSortingIndex < 10 Then
			RumRunnersLevelBouncingBeetle.LastSortingIndex = 15
		End If
		Me.visualTransform.GetComponent(Of SpriteRenderer)().sortingOrder = RumRunnersLevelBouncingBeetle.LastSortingIndex
		Return Me
	End Function

	' Token: 0x06002A81 RID: 10881 RVA: 0x0018D324 File Offset: 0x0018B724
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		Me.hp -= info.damage
		If Me.hp <= 0F Then
			Level.Current.RegisterMinionKilled()
			Me.Die()
		End If
	End Sub

	' Token: 0x06002A82 RID: 10882 RVA: 0x0018D35C File Offset: 0x0018B75C
	Protected Overrides Sub Die()
		Me.SFX_RUMRUN_CaterpillarBall_DeathExplosion()
		Me.explosionPrefab.Create(MyBase.transform.position)
		For i As Integer = 0 To Global.UnityEngine.Random.Range(3, 5) - 1
			Dim num As Single = Global.UnityEngine.Random.Range(0F, 360F)
			Dim vector As Vector3 = New Vector3(Mathf.Cos(num) * 50F, Mathf.Sin(num) * 50F)
			Dim spriteDeathParts As SpriteDeathParts = Me.shrapnelPrefab.CreatePart(MyBase.transform.position + vector)
			spriteDeathParts.animator.Update(0F)
			spriteDeathParts.animator.Play(0, 0, Global.UnityEngine.Random.Range(0F, 1F))
		Next
		For j As Integer = 0 To Global.UnityEngine.Random.Range(3, 5) - 1
			Dim num2 As Single = Global.UnityEngine.Random.Range(0F, 360F)
			Dim vector2 As Vector3 = New Vector3(Mathf.Cos(num2) * 50F, Mathf.Sin(num2) * 50F)
			Dim spriteDeathParts2 As SpriteDeathParts = Me.shrapnelPrefab.CreatePart(MyBase.transform.position + vector2)
			spriteDeathParts2.animator.Update(0F)
			spriteDeathParts2.animator.Play(0, 0, Global.UnityEngine.Random.Range(0F, 1F))
			spriteDeathParts2.transform.SetScale(New Single?(0.75F), New Single?(0.75F), Nothing)
			Dim component As SpriteRenderer = spriteDeathParts2.GetComponent(Of SpriteRenderer)()
			component.sortingLayerName = "Background"
			component.sortingOrder = 95
			component.color = New Color(0.7F, 0.7F, 0.7F, 1F)
		Next
		MyBase.Die()
		Me.Recycle()
	End Sub

	' Token: 0x06002A83 RID: 10883 RVA: 0x0018D525 File Offset: 0x0018B925
	Private Sub Move()
		MyBase.StartCoroutine(Me.move_cr())
	End Sub

	' Token: 0x06002A84 RID: 10884 RVA: 0x0018D534 File Offset: 0x0018B934
	Private Iterator Function move_cr() As IEnumerator
		Dim wait As YieldInstruction = New WaitForFixedUpdate()
		Dim elapsedTime As Single = 0F
		While True
			Yield wait
			If Me.isMoving Then
				If elapsedTime <= Me.slowdownDuration Then
					elapsedTime += CupheadTime.FixedDelta
					Me.currentSpeed = Mathf.Lerp(Me.initialSpeed, Me.targetSpeed, elapsedTime / Me.slowdownDuration)
				End If
				MyBase.transform.position += Me.velocity * Me.currentSpeed * CupheadTime.FixedDelta
				Me.CheckBounds()
			End If
		End While
		Return
	End Function

	' Token: 0x06002A85 RID: 10885 RVA: 0x0018D550 File Offset: 0x0018B950
	Private Sub CheckBounds()
		Dim flag As Boolean = False
		Dim vector As Vector3 = Vector3.zero
		Dim one As Vector3 = Vector3.one
		Dim num As Single = 0F
		If MyBase.transform.position.y > CupheadLevelCamera.Current.Bounds.yMax - Me.offset AndAlso Me.velocity.y > 0F Then
			flag = True
			Me.velocity.y = -Mathf.Abs(Me.velocity.y)
			vector = Vector2.up
			one.x = If((Me.velocity.x <= 0F), (-1F), 1F)
			num = 180F
		End If
		If MyBase.transform.position.y < CSng(Level.Current.Ground) + Me.offset AndAlso Me.velocity.y < 0F Then
			flag = True
			Me.velocity.y = Mathf.Abs(Me.velocity.y)
			vector = Vector2.down
			one.x = If((Me.velocity.x >= 0F), (-1F), 1F)
			one.y = 1F
			num = 0F
		End If
		If Not Me.leaveScreen Then
			If MyBase.transform.position.x > CupheadLevelCamera.Current.Bounds.xMax - Me.offset AndAlso Me.velocity.x > 0F Then
				flag = True
				Me.velocity.x = -Mathf.Abs(Me.velocity.x)
				vector = Vector2.right
				num = 90F
				one.x = If((Me.velocity.y >= 0F), (-1F), 1F)
			End If
			If MyBase.transform.position.x < CupheadLevelCamera.Current.Bounds.xMin + Me.offset AndAlso Me.velocity.x < 0F Then
				flag = True
				Me.velocity.x = Mathf.Abs(Me.velocity.x)
				vector = Vector2.left
				one.x = If((Me.velocity.y <= 0F), (-1F), 1F)
				num = 270F
			End If
		ElseIf MyBase.transform.position.x < CupheadLevelCamera.Current.Bounds.xMin - 100F OrElse MyBase.transform.position.x > CupheadLevelCamera.Current.Bounds.xMax + 100F Then
			MyBase.Die()
		End If
		If flag Then
			Dim effect As Effect = Me.wallPoofEffect.Create(MyBase.transform.position + vector * Me.offset)
			effect.transform.rotation = Quaternion.Euler(0F, 0F, num)
			Dim localScale As Vector3 = effect.transform.localScale
			localScale.x *= one.x
			effect.transform.localScale = localScale
			Me.SFX_RUMRUN_CaterpillarBall_Bounce()
			If Me.squashCoroutine IsNot Nothing Then
				MyBase.StopCoroutine(Me.squashCoroutine)
			End If
			Me.squashCoroutine = MyBase.StartCoroutine(Me.squash_cr(vector))
		End If
	End Sub

	' Token: 0x06002A86 RID: 10886 RVA: 0x0018D928 File Offset: 0x0018BD28
	Private Iterator Function squash_cr(normal As Vector2) As IEnumerator
		Dim scale As Vector3 = Me.initialScale
		Dim visualOffset As Vector3
		If normal.x <> 0F Then
			scale.x *= Me.squashAmount
			scale.y *= Me.squashAmountPerpendicular
			visualOffset = New Vector3(Me.offset * (1F - Me.squashAmount) * Mathf.Sign(normal.x), 0F)
		Else
			scale.y *= Me.squashAmount
			scale.x *= Me.squashAmountPerpendicular
			visualOffset = New Vector3(0F, Me.offset * (1F - Me.squashAmount) * Mathf.Sign(normal.y))
			Me.SFX_RUMRUN_CaterpillarBall_Bounce()
		End If
		Me.visualTransform.localScale = scale
		Me.visualTransform.localPosition = visualOffset
		Dim elapsedTime As Single = 0F
		While elapsedTime < 0.041666668F
			Yield Nothing
			elapsedTime += CupheadTime.Delta
		End While
		Me.visualTransform.localScale = Me.initialScale
		Me.visualTransform.localPosition = Vector3.zero
		Me.squashCoroutine = Nothing
		Return
	End Function

	' Token: 0x06002A87 RID: 10887 RVA: 0x0018D94A File Offset: 0x0018BD4A
	Private Sub SFX_RUMRUN_CaterpillarBall_Bounce()
		AudioManager.Play("sfx_dlc_rumrun_caterpillarball_bounce")
		Me.emitAudioFromObject.Add("sfx_dlc_rumrun_caterpillarball_bounce")
	End Sub

	' Token: 0x06002A88 RID: 10888 RVA: 0x0018D966 File Offset: 0x0018BD66
	Private Sub SFX_RUMRUN_CaterpillarBall_DeathExplosion()
		AudioManager.Play("sfx_dlc_rumrun_caterpillarball_deathexplosion")
		Me.emitAudioFromObject.Add("sfx_dlc_rumrun_caterpillarball_deathexplosion")
	End Sub

	' Token: 0x04003343 RID: 13123
	Private Const DESTROY_RANGE As Single = 100F

	' Token: 0x04003344 RID: 13124
	Private Shared LastSortingIndex As Integer

	' Token: 0x04003346 RID: 13126
	<SerializeField()>
	Private wallPoofEffect As Effect

	' Token: 0x04003347 RID: 13127
	<SerializeField()>
	Private visualTransform As Transform

	' Token: 0x04003348 RID: 13128
	<SerializeField()>
	Private squashAmount As Single

	' Token: 0x04003349 RID: 13129
	<SerializeField()>
	Private squashAmountPerpendicular As Single

	' Token: 0x0400334A RID: 13130
	Private isMoving As Boolean

	' Token: 0x0400334B RID: 13131
	Private initialSpeed As Single

	' Token: 0x0400334C RID: 13132
	Private targetSpeed As Single

	' Token: 0x0400334D RID: 13133
	Private currentSpeed As Single

	' Token: 0x0400334E RID: 13134
	Private slowdownDuration As Single

	' Token: 0x0400334F RID: 13135
	Private hp As Single

	' Token: 0x04003350 RID: 13136
	Private offset As Single

	' Token: 0x04003351 RID: 13137
	Private velocity As Vector3

	' Token: 0x04003352 RID: 13138
	Private initialScale As Vector3

	' Token: 0x04003353 RID: 13139
	Private squashCoroutine As Coroutine

	' Token: 0x04003354 RID: 13140
	Private damageReceiver As DamageReceiver

	' Token: 0x04003355 RID: 13141
	<SerializeField()>
	Private explosionPrefab As Effect

	' Token: 0x04003356 RID: 13142
	<SerializeField()>
	Private shrapnelPrefab As SpriteDeathPartsDLC
End Class
