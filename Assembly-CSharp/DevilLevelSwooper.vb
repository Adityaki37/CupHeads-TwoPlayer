Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000587 RID: 1415
Public Class DevilLevelSwooper
	Inherits AbstractCollidableObject

	' Token: 0x06001AFF RID: 6911 RVA: 0x000F8010 File Offset: 0x000F6410
	Public Function Create(parent As DevilLevelGiantHead, properties As LevelProperties.Devil.Swoopers, spawnPos As Vector3, xPos As Single) As DevilLevelSwooper
		Dim devilLevelSwooper As DevilLevelSwooper = Me.InstantiatePrefab(Of DevilLevelSwooper)()
		devilLevelSwooper.parent = parent
		devilLevelSwooper.properties = properties
		devilLevelSwooper.state = DevilLevelSwooper.State.Intro
		devilLevelSwooper.transform.position = spawnPos
		devilLevelSwooper.yPos = properties.yIdlePos.RandomFloat()
		devilLevelSwooper.StartCoroutine(devilLevelSwooper.spawn_cr(xPos))
		Return devilLevelSwooper
	End Function

	' Token: 0x06001B00 RID: 6912 RVA: 0x000F8066 File Offset: 0x000F6466
	Protected Overrides Sub Awake()
		MyBase.Awake()
		AddHandler MyBase.GetComponent(Of DamageReceiver)().OnDamageTaken, AddressOf Me.OnDamageTaken
		Me.damageDealer = DamageDealer.NewEnemy()
	End Sub

	' Token: 0x06001B01 RID: 6913 RVA: 0x000F8090 File Offset: 0x000F6490
	Private Sub Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x06001B02 RID: 6914 RVA: 0x000F80A8 File Offset: 0x000F64A8
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If Me.damageDealer IsNot Nothing AndAlso phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x06001B03 RID: 6915 RVA: 0x000F80D1 File Offset: 0x000F64D1
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		Me.hp -= info.damage
		If Me.hp < 0F AndAlso Me.state <> DevilLevelSwooper.State.Dying Then
			Me.Die()
		End If
	End Sub

	' Token: 0x06001B04 RID: 6916 RVA: 0x000F8108 File Offset: 0x000F6508
	Private Iterator Function spawn_cr(xPos As Single) As IEnumerator
		Me.hp = Me.properties.hp
		MyBase.transform.SetEulerAngles(New Single?(0F), New Single?(0F), New Single?(0F))
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Spawn", False, True)
		While MyBase.transform.position.y < CupheadLevelCamera.Current.Bounds.yMax + 50F
			MyBase.transform.position += Vector3.up * 200F * CupheadTime.Delta
			Yield Nothing
		End While
		MyBase.animator.SetTrigger("Continue")
		Dim t As Single = 0F
		Dim start As Vector2 = MyBase.transform.position
		Dim [end] As Vector2 = New Vector3(xPos, Me.yPos)
		While t < 2F
			Dim val As Single = EaseUtils.Ease(EaseUtils.EaseType.linear, 0F, 1F, t / 2F)
			MyBase.transform.position = Vector3.Lerp(start, [end], val)
			t += CupheadTime.Delta
			Yield Nothing
		End While
		MyBase.transform.SetPosition(New Single?(xPos), New Single?(Me.yPos), Nothing)
		Me.state = DevilLevelSwooper.State.Idle
		Return
	End Function

	' Token: 0x06001B05 RID: 6917 RVA: 0x000F812A File Offset: 0x000F652A
	Public Sub Swoop()
		Me.state = DevilLevelSwooper.State.Swooping
		MyBase.StartCoroutine(Me.swoop_cr())
		AudioManager.Play("mini_devil_attack")
	End Sub

	' Token: 0x06001B06 RID: 6918 RVA: 0x000F814C File Offset: 0x000F654C
	Private Iterator Function swoop_cr() As IEnumerator
		Dim bestDistance As Single = Single.MaxValue
		Dim bestVelocity As Vector2 = Vector2.zero
		Dim target As Vector3 = PlayerManager.GetNext().center
		Dim relativeTargetPos As Vector2 = target - MyBase.transform.position
		relativeTargetPos.x = Mathf.Abs(relativeTargetPos.x)
		If target.x > MyBase.transform.position.x Then
			MyBase.animator.SetTrigger("OnTurn")
			Yield MyBase.animator.WaitForAnimationToEnd(Me, "Turn", False, True)
		End If
		MyBase.animator.SetBool("Spinning", True)
		Me.AttackSFX()
		Dim num As Single = 0F
		While num < 1F
			Dim num2 As Single = -Me.properties.launchAngle.GetFloatAt(num)
			Dim floatAt As Single = Me.properties.launchSpeed.GetFloatAt(num)
			Dim vector As Vector2 = MathUtils.AngleToDirection(num2) * floatAt
			Dim num3 As Single = relativeTargetPos.x / vector.x
			Dim num4 As Single = vector.y * num3 + 0.5F * Me.properties.gravity * num3 * num3
			Dim num5 As Single = Mathf.Abs(relativeTargetPos.y - num4)
			Dim num6 As Single = vector.y + Me.properties.gravity * num3
			If num6 >= 0F Then
				If num5 < bestDistance Then
					bestDistance = num5
					bestVelocity = vector
				End If
			End If
			num += 0.01F
		End While
		If target.x < MyBase.transform.position.x Then
			bestVelocity.x *= -1F
		End If
		Dim velocity As Vector2 = bestVelocity
		While MyBase.transform.position.y < CSng((Level.Current.Ceiling + 150))
			velocity.y += Me.properties.gravity * CupheadTime.FixedDelta
			MyBase.transform.AddPosition(velocity.x * CupheadTime.FixedDelta, velocity.y * CupheadTime.FixedDelta, 0F)
			Yield New WaitForFixedUpdate()
		End While
		Me.state = DevilLevelSwooper.State.Returning
		Dim xPos As Single = Me.parent.PutSwooperInSlot(Me)
		MyBase.transform.SetPosition(New Single?(xPos), Nothing, Nothing)
		Yield CupheadTime.WaitForSeconds(Me, 0.5F)
		Dim moveTime As Single = 1.5F
		Dim t As Single = 0F
		While t < moveTime
			MyBase.transform.SetPosition(Nothing, New Single?(EaseUtils.Ease(EaseUtils.EaseType.easeOutSine, CSng((Level.Current.Ceiling + 150)), Me.yPos, t / moveTime)), Nothing)
			t += CupheadTime.FixedDelta
			Yield New WaitForFixedUpdate()
		End While
		Me.state = DevilLevelSwooper.State.Idle
		MyBase.transform.SetPosition(Nothing, New Single?(Me.yPos), Nothing)
		MyBase.animator.SetBool("Spinning", False)
		Me.AttackSFXEnd()
		Return
	End Function

	' Token: 0x06001B07 RID: 6919 RVA: 0x000F8168 File Offset: 0x000F6568
	Public Sub Die()
		If Me.finalSwooping Then
			AudioManager.[Stop]("swooper_spin")
		End If
		If Me.state = DevilLevelSwooper.State.Dying Then
			Return
		End If
		Me.state = DevilLevelSwooper.State.Dying
		Me.StopAllCoroutines()
		Me.parent.OnSwooperDeath(Me)
		MyBase.StartCoroutine(Me.death_cr())
		AudioManager.Play("mini_devil_die")
		Me.emitAudioFromObject.Add("mini_devil_die")
		AudioManager.[Stop]("swooper_spin")
	End Sub

	' Token: 0x06001B08 RID: 6920 RVA: 0x000F81E4 File Offset: 0x000F65E4
	Private Iterator Function death_cr() As IEnumerator
		While Me.state = DevilLevelSwooper.State.Intro
			Yield Nothing
		End While
		For Each effect As Effect In Me.explosions
			effect.Create(MyBase.transform.position)
		Next
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
		Return
	End Function

	' Token: 0x06001B09 RID: 6921 RVA: 0x000F8200 File Offset: 0x000F6600
	Private Sub OnTurn()
		MyBase.transform.SetScale(New Single?(-MyBase.transform.localScale.x), Nothing, Nothing)
	End Sub

	' Token: 0x06001B0A RID: 6922 RVA: 0x000F8243 File Offset: 0x000F6643
	Private Sub AttackSFX()
		AudioManager.PlayLoop("swooper_spin")
		Me.emitAudioFromObject.Add("swooper_spin_end")
	End Sub

	' Token: 0x06001B0B RID: 6923 RVA: 0x000F825F File Offset: 0x000F665F
	Private Sub AttackSFXEnd()
		If Me.finalSwooping Then
			AudioManager.[Stop]("swooper_spin")
		End If
		AudioManager.Play("swooper_spin_end")
		Me.emitAudioFromObject.Add("swooper_spin_end")
		Me.finalSwooping = False
	End Sub

	' Token: 0x0400243C RID: 9276
	<SerializeField()>
	Private explosions As Effect()

	' Token: 0x0400243D RID: 9277
	Private Const SPAWN_X_RATIO As Single = 0.5F

	' Token: 0x0400243E RID: 9278
	Public state As DevilLevelSwooper.State

	' Token: 0x0400243F RID: 9279
	Private parent As DevilLevelGiantHead

	' Token: 0x04002440 RID: 9280
	Private properties As LevelProperties.Devil.Swoopers

	' Token: 0x04002441 RID: 9281
	Private damageDealer As DamageDealer

	' Token: 0x04002442 RID: 9282
	Private hp As Single

	' Token: 0x04002443 RID: 9283
	Public finalSwooping As Boolean

	' Token: 0x04002444 RID: 9284
	Private yPos As Single

	' Token: 0x02000588 RID: 1416
	Public Enum State
		' Token: 0x04002446 RID: 9286
		Intro
		' Token: 0x04002447 RID: 9287
		Idle
		' Token: 0x04002448 RID: 9288
		Swooping
		' Token: 0x04002449 RID: 9289
		Returning
		' Token: 0x0400244A RID: 9290
		Dying
	End Enum
End Class
