Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000703 RID: 1795
Public Class OldManLevelGnomeProjectile
	Inherits AbstractProjectile

	' Token: 0x170003CB RID: 971
	' (get) Token: 0x06002692 RID: 9874 RVA: 0x0016928B File Offset: 0x0016768B
	' (set) Token: 0x06002693 RID: 9875 RVA: 0x00169293 File Offset: 0x00167693
	Public Property IsFlying As Boolean

	' Token: 0x06002694 RID: 9876 RVA: 0x0016929C File Offset: 0x0016769C
	Public Overridable Function Init(position As Vector3, speed As Vector3, gravity As Single, spawnParryable As Boolean, parryable As Boolean, target As OldManLevelStomachPlatform) As OldManLevelGnomeProjectile
		MyBase.ResetLifetime()
		MyBase.ResetDistance()
		MyBase.transform.position = position
		MyBase.transform.localScale = New Vector3(CSng(If((Not MathUtils.RandomBool()), 1, (-1))), 1F)
		Me.speed = speed
		Me.gravity = gravity
		Me.spawnParryable = spawnParryable
		Me.IsFlying = True
		Me.SetParryable(parryable)
		Me.target = target
		Me.animHelper = MyBase.GetComponent(Of AnimationHelper)()
		Me.animHelper.Speed = 1F
		MyBase.animator.Play(If((Not spawnParryable), If((Not parryable), "Chicken", "ChickenPink"), "Bone"))
		MyBase.animator.Update(0F)
		MyBase.GetComponent(Of Collider2D)().enabled = True
		Me.bouncingOffscreen = False
		Me.triedHit = False
		Me.underwaterSprite.color = Color.white
		Me.playedAnticipationSound = False
		Return Me
	End Function

	' Token: 0x06002695 RID: 9877 RVA: 0x001693A5 File Offset: 0x001677A5
	Public Overrides Sub OnParry(player As AbstractPlayerController)
		Me.target.CancelAnticipation()
		MyBase.OnParry(player)
	End Sub

	' Token: 0x06002696 RID: 9878 RVA: 0x001693B9 File Offset: 0x001677B9
	Public Overrides Sub OnLevelEnd()
	End Sub

	' Token: 0x06002697 RID: 9879 RVA: 0x001693BC File Offset: 0x001677BC
	Protected Overrides Sub FixedUpdate()
		MyBase.FixedUpdate()
		Me.speed += New Vector3(0F, Me.gravity * CupheadTime.FixedDelta)
		If Me.bouncingOffscreen Then
			Me.speed += New Vector3(0F, Me.gravity * CupheadTime.FixedDelta)
		End If
		MyBase.transform.Translate(Me.speed * CupheadTime.FixedDelta)
		If Me.bouncingOffscreen Then
			If Not Me.splashed AndAlso MyBase.transform.position.y < Me.target.main.splashHandler.transform.position.y Then
				Me.target.main.splashHandler.SplashIn(MyBase.transform.position.x)
				Me.speed *= 0.5F
				Me.gravity *= 0.5F
				Me.animHelper.Speed = 0.5F
				Me.splashed = True
			End If
			If MyBase.transform.position.y < -560F Then
				Me.Recycle()
			End If
			If Me.splashed Then
				Me.underwaterSprite.color = New Color(1F, 1F, 1F, (1F - Mathf.InverseLerp(Me.target.main.splashHandler.transform.position.y, Me.target.main.splashHandler.transform.position.y - 140F, MyBase.transform.position.y)) * 0.5F)
			End If
		End If
		If Me.spawnParryable AndAlso MyBase.transform.position.y < Me.target.transform.position.y + 200F AndAlso Not Me.playedAnticipationSound Then
			Me.SFX_PreBoneHit()
			Me.playedAnticipationSound = True
		End If
		If MyBase.transform.position.y < Me.target.transform.position.y + If((Not Me.spawnParryable), 200F, 50F) AndAlso Not Me.triedHit Then
			Me.HitTarget()
		End If
	End Sub

	' Token: 0x06002698 RID: 9880 RVA: 0x00169671 File Offset: 0x00167A71
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
		MyBase.OnCollisionPlayer(hit, phase)
	End Sub

	' Token: 0x06002699 RID: 9881 RVA: 0x00169690 File Offset: 0x00167A90
	Private Sub HitTarget()
		Me.triedHit = True
		If Me.target.isActivated Then
			For Each abstractPlayerController As AbstractPlayerController In Me.target.GetComponentsInChildren(Of AbstractPlayerController)()
				If Not(abstractPlayerController Is Nothing) Then
					abstractPlayerController.transform.parent = Nothing
				End If
			Next
			Me.target.DeactivatePlatform(Me.spawnParryable)
			Me.IsFlying = False
			If Me.spawnParryable Then
				Me.bouncingOffscreen = True
				Me.speed.y = -Me.speed.y * Me.bounceModifier
				Me.speed.x = Me.speed.x * 2F
				MyBase.transform.localScale = New Vector3(-MyBase.transform.localScale.x, 1F)
				MyBase.GetComponent(Of Collider2D)().enabled = False
			Else
				MyBase.StartCoroutine(Me.wait_for_eat())
			End If
		Else
			Me.bouncingOffscreen = True
		End If
	End Sub

	' Token: 0x0600269A RID: 9882 RVA: 0x001697A8 File Offset: 0x00167BA8
	Private Iterator Function wait_for_eat() As IEnumerator
		Dim anim As Animator = Me.target.GetComponent(Of Animator)()
		Yield anim.WaitForAnimationToStart(Me, "Eat", False)
		While anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.18965517F
			Yield Nothing
		End While
		MyBase.GetComponent(Of Collider2D)().enabled = False
		Me.Recycle()
		Return
	End Function

	' Token: 0x0600269B RID: 9883 RVA: 0x001697C3 File Offset: 0x00167BC3
	Private Sub SFX_PreBoneHit()
		AudioManager.Play("sfx_dlc_omm_p3_dinobells_prebonehit")
		Me.emitAudioFromObject.Add("sfx_dlc_omm_p3_dinobells_prebonehit")
	End Sub

	' Token: 0x04002F47 RID: 12103
	Private Const OFFSET_TO_HIT_BONE As Single = 50F

	' Token: 0x04002F48 RID: 12104
	Private Const OFFSET_TO_PLAY_ANTICIPATION_SOUND As Single = 200F

	' Token: 0x04002F49 RID: 12105
	Private Const OFFSET_TO_HIT_LEG As Single = 200F

	' Token: 0x04002F4B RID: 12107
	Private speed As Vector3

	' Token: 0x04002F4C RID: 12108
	Private gravity As Single

	' Token: 0x04002F4D RID: 12109
	Private spawnParryable As Boolean

	' Token: 0x04002F4E RID: 12110
	Private target As OldManLevelStomachPlatform

	' Token: 0x04002F4F RID: 12111
	Private bouncingOffscreen As Boolean

	' Token: 0x04002F50 RID: 12112
	<SerializeField()>
	Private bounceModifier As Single = 0.5F

	' Token: 0x04002F51 RID: 12113
	<SerializeField()>
	Private underwaterSprite As SpriteRenderer

	' Token: 0x04002F52 RID: 12114
	Private triedHit As Boolean

	' Token: 0x04002F53 RID: 12115
	Private splashed As Boolean

	' Token: 0x04002F54 RID: 12116
	Private animHelper As AnimationHelper

	' Token: 0x04002F55 RID: 12117
	Private playedAnticipationSound As Boolean
End Class
