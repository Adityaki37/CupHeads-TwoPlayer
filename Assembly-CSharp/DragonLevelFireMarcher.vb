Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020005F0 RID: 1520
Public Class DragonLevelFireMarcher
	Inherits AbstractCollidableObject

	' Token: 0x06001E37 RID: 7735 RVA: 0x00115FB3 File Offset: 0x001143B3
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.damageDealer = DamageDealer.NewEnemy()
		If Me.canJump Then
			MyBase.animator.Play("Idle", 0, Global.UnityEngine.Random.Range(0F, 1F))
		End If
	End Sub

	' Token: 0x06001E38 RID: 7736 RVA: 0x00115FF1 File Offset: 0x001143F1
	Private Sub Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x06001E39 RID: 7737 RVA: 0x00116009 File Offset: 0x00114409
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If Me.damageDealer IsNot Nothing AndAlso phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x06001E3A RID: 7738 RVA: 0x00116034 File Offset: 0x00114434
	Public Function Create(root As Transform, properties As LevelProperties.Dragon.FireMarchers) As DragonLevelFireMarcher
		Dim dragonLevelFireMarcher As DragonLevelFireMarcher = Me.InstantiatePrefab(Of DragonLevelFireMarcher)()
		dragonLevelFireMarcher.transform.parent = root
		dragonLevelFireMarcher.transform.ResetLocalPosition()
		dragonLevelFireMarcher.properties = properties
		dragonLevelFireMarcher.StartCoroutine(dragonLevelFireMarcher.move_cr())
		Return dragonLevelFireMarcher
	End Function

	' Token: 0x06001E3B RID: 7739 RVA: 0x00116074 File Offset: 0x00114474
	Private Iterator Function move_cr() As IEnumerator
		Dim initialYOffset As Single = Mathf.Sin(0.9773844F) * 5F
		Dim timeSlowed As Single = 0F
		While MyBase.transform.position.x < CSng((Level.Current.Right + 100))
			Dim speed As Single = Me.properties.moveSpeed
			If Me.slowing Then
				timeSlowed += CupheadTime.Delta
				If timeSlowed > 0.25F Then
					Return
				End If
				speed = EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, speed, 0F, timeSlowed / 0.25F)
			End If
			Dim x As Single = MyBase.transform.localPosition.x + speed * CupheadTime.Delta
			Dim y As Single = Mathf.Sin((70F + MyBase.transform.localPosition.x) * 2F * 3.1415927F / 450F) * 5F - initialYOffset
			y += Mathf.Min(1F, x / 300F) * -23F
			If x < Me.squeezeDistance Then
				MyBase.transform.SetScale(New Single?(x / Me.squeezeDistance), Nothing, Nothing)
			Else
				MyBase.transform.SetScale(New Single?(1F), Nothing, Nothing)
			End If
			MyBase.transform.SetLocalPosition(New Single?(x), New Single?(y), Nothing)
			Yield Nothing
		End While
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
		Return
	End Function

	' Token: 0x06001E3C RID: 7740 RVA: 0x00116090 File Offset: 0x00114490
	Public Function CanJump() As Boolean
		If Not Me.canJump OrElse Me.wantsToJump Then
			Return False
		End If
		Dim currentAnimatorStateInfo As AnimatorStateInfo = MyBase.animator.GetCurrentAnimatorStateInfo(0)
		Dim num As Single = MyBase.transform.localPosition.x + (1F - currentAnimatorStateInfo.normalizedTime Mod 1F) * currentAnimatorStateInfo.length * Me.properties.moveSpeed
		Return num > Me.properties.jumpX.min AndAlso num < Me.properties.jumpX.max
	End Function

	' Token: 0x06001E3D RID: 7741 RVA: 0x0011612A File Offset: 0x0011452A
	Public Sub StartJump(targetPlayer As AbstractPlayerController)
		Me.targetPlayer = targetPlayer
		Me.wantsToJump = True
		MyBase.StartCoroutine(Me.jump_cr())
	End Sub

	' Token: 0x06001E3E RID: 7742 RVA: 0x00116148 File Offset: 0x00114548
	Private Iterator Function jump_cr() As IEnumerator
		MyBase.animator.SetTrigger("StartJump")
		Yield MyBase.animator.WaitForAnimationToStart(Me, "Crouch_Start", False)
		AudioManager.Play("level_dragon_fire_marcher_b_couch_start")
		Me.emitAudioFromObject.Add("level_dragon_fire_marcher_b_couch_start")
		Me.slowing = True
		Yield MyBase.animator.WaitForAnimationToStart(Me, "Crouch_Loop", False)
		Dim targetPos As Vector2 = Me.targetPlayer.center
		If targetPos.x < MyBase.transform.position.x Then
			MyBase.transform.SetScale(New Single?(-1F), Nothing, Nothing)
		End If
		Dim bestDistance As Single = Single.MaxValue
		Dim bestLaunchVelocity As Vector2 = Vector2.zero
		Dim relativeTargetPos As Vector2 = targetPos - MyBase.transform.position
		relativeTargetPos.x = Mathf.Abs(relativeTargetPos.x)
		Dim num As Single = 0F
		While num < 1F
			Dim floatAt As Single = Me.properties.jumpAngle.GetFloatAt(num)
			Dim floatAt2 As Single = Me.properties.jumpSpeed.GetFloatAt(num)
			Dim vector As Vector2 = MathUtils.AngleToDirection(floatAt) * floatAt2
			Dim num2 As Single = relativeTargetPos.x / vector.x
			Dim num3 As Single = vector.y * num2 - 0.5F * Me.properties.gravity * num2 * num2
			Dim num4 As Single = Mathf.Abs(relativeTargetPos.y - num3)
			If num4 < bestDistance Then
				bestDistance = num4
				bestLaunchVelocity = vector
			End If
			num += 0.01F
		End While
		Yield CupheadTime.WaitForSeconds(Me, Me.properties.crouchTime)
		MyBase.animator.SetTrigger("Continue")
		Yield MyBase.animator.WaitForAnimationToStart(Me, "Jump_Start", False)
		AudioManager.Play("level_dragon_fire_marcher_b_jump_start")
		Me.emitAudioFromObject.Add("level_dragon_fire_marcher_b_jump_start")
		Dim velocity As Vector2 = bestLaunchVelocity
		velocity.x *= MyBase.transform.localScale.x
		Dim t As Single = 0F
		Dim initialPos As Vector2 = MyBase.transform.localPosition
		While MyBase.transform.position.y > -400F
			t += CupheadTime.FixedDelta
			MyBase.transform.SetLocalPosition(New Single?(initialPos.x + t * velocity.x), New Single?(initialPos.y + t * velocity.y - 0.5F * Me.properties.gravity * t * t), Nothing)
			Yield New WaitForFixedUpdate()
		End While
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
		Return
	End Function

	' Token: 0x040026FA RID: 9978
	Private Const sinOffset As Single = 70F

	' Token: 0x040026FB RID: 9979
	Private Const sinPeriod As Single = 450F

	' Token: 0x040026FC RID: 9980
	Private Const sinHeight As Single = 5F

	' Token: 0x040026FD RID: 9981
	Private Const linearOffset As Single = -23F

	' Token: 0x040026FE RID: 9982
	Private Const linearOffsetDistance As Single = 300F

	' Token: 0x040026FF RID: 9983
	Private Const minJumpX As Single = 50F

	' Token: 0x04002700 RID: 9984
	Private Const maxJumpX As Single = 590F

	' Token: 0x04002701 RID: 9985
	Private damageDealer As DamageDealer

	' Token: 0x04002702 RID: 9986
	Private properties As LevelProperties.Dragon.FireMarchers

	' Token: 0x04002703 RID: 9987
	<SerializeField()>
	Private squeezeDistance As Single

	' Token: 0x04002704 RID: 9988
	<SerializeField()>
	Private canJump As Boolean

	' Token: 0x04002705 RID: 9989
	Private wantsToJump As Boolean

	' Token: 0x04002706 RID: 9990
	Private slowing As Boolean

	' Token: 0x04002707 RID: 9991
	Private targetPlayer As AbstractPlayerController
End Class
