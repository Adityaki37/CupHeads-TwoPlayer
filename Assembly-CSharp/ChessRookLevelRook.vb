Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000554 RID: 1364
Public Class ChessRookLevelRook
	Inherits LevelProperties.ChessRook.Entity

	' Token: 0x06001964 RID: 6500 RVA: 0x000E62D4 File Offset: 0x000E46D4
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.damageDealer = DamageDealer.NewEnemy()
	End Sub

	' Token: 0x06001965 RID: 6501 RVA: 0x000E62E7 File Offset: 0x000E46E7
	Public Overrides Sub LevelInit(properties As LevelProperties.ChessRook)
		MyBase.LevelInit(properties)
	End Sub

	' Token: 0x06001966 RID: 6502 RVA: 0x000E62F0 File Offset: 0x000E46F0
	Protected Overrides Sub OnDrawGizmos()
		MyBase.OnDrawGizmosSelected()
		Gizmos.color = Color.cyan
		For Each transform As Transform In Me.straightShotSpawnPoints
			Gizmos.DrawLine(transform.transform.position, transform.transform.position - Vector3.right * 1000F)
		Next
	End Sub

	' Token: 0x06001967 RID: 6503 RVA: 0x000E635B File Offset: 0x000E475B
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x06001968 RID: 6504 RVA: 0x000E637C File Offset: 0x000E477C
	Protected Overrides Sub OnCollisionEnemyProjectile(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionEnemyProjectile(hit, phase)
		If phase = CollisionPhase.Enter Then
			Dim component As ChessRookLevelPinkCannonBall = hit.GetComponent(Of ChessRookLevelPinkCannonBall)()
			If component AndAlso component.finishedOriginalArc Then
				Me.damaged()
				component.Explosion()
			End If
		End If
	End Sub

	' Token: 0x06001969 RID: 6505 RVA: 0x000E63C0 File Offset: 0x000E47C0
	Private Sub damaged()
		If Me.dead Then
			Return
		End If
		AudioManager.Play("sfx_dlc_kog_rook_hurt")
		Me.emitAudioFromObject.Add("sfx_dlc_kog_rook_hurt")
		Me.hitFlash.Flash(0.7F)
		Dim stateName As LevelProperties.ChessRook.States = MyBase.properties.CurrentState.stateName
		MyBase.properties.DealDamage(If((Not PlayerManager.BothPlayersActive()), 10F, ChessKingLevelKing.multiplayerDamageNerf))
		If MyBase.properties.CurrentHealth <= 0F AndAlso Not Me.dead Then
			Me.die()
		ElseIf stateName = LevelProperties.ChessRook.States.PhaseThree OrElse stateName = LevelProperties.ChessRook.States.PhaseFour Then
			If Me.transitionCoroutine IsNot Nothing Then
				MyBase.StopCoroutine(Me.transitionCoroutine)
				MyBase.animator.ResetTrigger("Transition")
				Me.transitionCoroutine = Nothing
				MyBase.animator.Play("LateIntro", ChessRookLevelRook.SparkLayerStateIndex)
			End If
			Me.hitSparkEffect.Create(MyBase.transform.position)
			MyBase.animator.Play("Hit2", 0, 0F)
		Else
			MyBase.animator.Play("Hit1", 0, 0F)
			MyBase.animator.Play("HitSmoke", 3, 0F)
		End If
	End Sub

	' Token: 0x0600196A RID: 6506 RVA: 0x000E6514 File Offset: 0x000E4914
	Public Sub OnPhaseChange()
		Me.StopAllCoroutines()
		Me.StartAttacks()
		MyBase.animator.ResetTrigger("SparkAttack")
		If MyBase.properties.CurrentState.stateName = LevelProperties.ChessRook.States.PhaseThree Then
			Me.transitionCoroutine = MyBase.StartCoroutine(Me.transition_cr())
		End If
	End Sub

	' Token: 0x0600196B RID: 6507 RVA: 0x000E6565 File Offset: 0x000E4965
	Private Sub Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x0600196C RID: 6508 RVA: 0x000E6580 File Offset: 0x000E4980
	Private Sub animationEvent_IntroFinished()
		Me.StartAttacks()
		MyBase.animator.Play("Intro", ChessRookLevelRook.SparkLayerStateIndex)
		AudioManager.PlayLoop("sfx_dlc_kog_rook_grindingwheel_lowspeed")
		Me.emitAudioFromObject.Add("sfx_dlc_kog_rook_grindingwheel_lowspeed")
		AudioManager.PlayLoop("sfx_dlc_kog_rook_grindingwheel_lowspeed_axeonwheel")
		AudioManager.FadeSFXVolume("sfx_dlc_kog_rook_grindingwheel_lowspeed_axeonwheel", 0.0001F, 0.0001F)
		Me.emitAudioFromObject.Add("sfx_dlc_kog_rook_grindingwheel_lowspeed_axeonwheel")
		AudioManager.PlayLoop("sfx_dlc_kog_rook_sparks_loop")
	End Sub

	' Token: 0x0600196D RID: 6509 RVA: 0x000E65FC File Offset: 0x000E49FC
	Private Iterator Function transition_cr() As IEnumerator
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Hit1", False, True)
		MyBase.animator.SetTrigger("Transition")
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Transition", False, True)
		AudioManager.[Stop]("sfx_dlc_kog_rook_grindingwheel_lowspeed")
		AudioManager.PlayLoop("sfx_dlc_kog_rook_grindingwheel_highspeed")
		Me.emitAudioFromObject.Add("sfx_dlc_kog_rook_grindingwheel_highspeed")
		Me.transitionCoroutine = Nothing
		Return
	End Function

	' Token: 0x0600196E RID: 6510 RVA: 0x000E6617 File Offset: 0x000E4A17
	Private Sub animationEvent_EndEarlyPhaseSparks()
		MyBase.animator.Play("EarlyOutro", ChessRookLevelRook.SparkLayerStateIndex)
	End Sub

	' Token: 0x0600196F RID: 6511 RVA: 0x000E662E File Offset: 0x000E4A2E
	Private Sub animationEvent_StartLatePhaseSparks()
		MyBase.animator.Play("LateIntro", ChessRookLevelRook.SparkLayerStateIndex)
	End Sub

	' Token: 0x06001970 RID: 6512 RVA: 0x000E6648 File Offset: 0x000E4A48
	Private Sub StartAttacks()
		MyBase.StartCoroutine(Me.pink_cannonballs_cr())
		MyBase.StartCoroutine(Me.regular_cannonballs_cr())
		If MyBase.properties.CurrentState.straightShooters.straightShotOn Then
			MyBase.StartCoroutine(Me.straight_shot_cr())
		End If
	End Sub

	' Token: 0x06001971 RID: 6513 RVA: 0x000E6698 File Offset: 0x000E4A98
	Private Iterator Function pink_cannonballs_cr() As IEnumerator
		Dim p As LevelProperties.ChessRook.PinkCannonBall = MyBase.properties.CurrentState.pinkCannonBall
		Dim delayPattern As PatternString = New PatternString(p.pinkShotDelayString, True, True)
		Dim apexHeightPattern As PatternString = New PatternString(p.pinkShotApexHeightString, True, True)
		Dim targetPattern As PatternString = New PatternString(p.pinkShotTargetString, True, True)
		While True
			Dim delay As Single = delayPattern.PopFloat()
			Dim apexHeight As Single = apexHeightPattern.PopFloat()
			Dim targetDistance As Single = targetPattern.PopFloat()
			Yield CupheadTime.WaitForSeconds(Me, delay - 0.16666667F)
			Me.spawnEffect.Play("Spawn" + If((Not Rand.Bool()), "B", "A") + "Head" + If((Not Me.headTypeB), "A", "B"), 0, 0F)
			Me.spawnEffect.Update(0F)
			Yield CupheadTime.WaitForSeconds(Me, 0.16666667F)
			Dim cannonBall As ChessRookLevelPinkCannonBall = Me.cannonballPink.Spawn()
			cannonBall.Create(Me.cannonballSpawnRoot.position + MyBase.transform.forward * 1E-05F * CSng(Me.headZOffset), apexHeight, targetDistance, p)
			cannonBall.animator.Play(If((Not Me.headTypeB), "A", "B"))
			Me.headTypeB = Not Me.headTypeB
			Me.headZOffset = (Me.headZOffset + 1) Mod 10
		End While
		Return
	End Function

	' Token: 0x06001972 RID: 6514 RVA: 0x000E66B4 File Offset: 0x000E4AB4
	Private Iterator Function regular_cannonballs_cr() As IEnumerator
		Dim p As LevelProperties.ChessRook.RegularCannonBall = MyBase.properties.CurrentState.regularCannonBall
		Dim delayPattern As PatternString = New PatternString(p.cannonDelayString, True, True)
		Dim apexHeightPattern As PatternString = New PatternString(p.cannonApexHeightString, True, True)
		Dim targetPattern As PatternString = New PatternString(p.cannonTargetString, True, True)
		While True
			Dim delay As Single = delayPattern.PopFloat()
			Dim apexHeight As Single = apexHeightPattern.PopFloat()
			Dim targetDistance As Single = targetPattern.PopFloat()
			Yield CupheadTime.WaitForSeconds(Me, delay - 0.16666667F)
			Me.spawnEffect.Play("Spawn" + If((Not Rand.Bool()), "B", "A") + "Skull", 0, 0F)
			Me.spawnEffect.Update(0F)
			Yield CupheadTime.WaitForSeconds(Me, 0.16666667F)
			Dim cannonBall As ChessRookLevelRegularCannonball = Me.cannonballRegular.Spawn()
			cannonBall.Create(Me.cannonballSpawnRoot.position + MyBase.transform.forward * 1E-05F * CSng(Me.headZOffset), apexHeight, targetDistance, p)
			Me.headZOffset = (Me.headZOffset + 1) Mod 10
		End While
		Return
	End Function

	' Token: 0x06001973 RID: 6515 RVA: 0x000E66D0 File Offset: 0x000E4AD0
	Private Iterator Function straight_shot_cr() As IEnumerator
		Dim p As LevelProperties.ChessRook.StraightShooters = MyBase.properties.CurrentState.straightShooters
		Dim sequencePattern As PatternString = New PatternString(p.straightShotSeqString, True, True)
		Dim delayPattern As PatternString = New PatternString(p.straightShotDelayString, True, True)
		Dim EarlyPhaseTransitionOffset As Single = 0.14583333F
		Dim EarlyPhaseShootOffsetRange As Rangef = New Rangef(0.16666667F, 0.33333334F)
		While True
			Dim delay As Single = delayPattern.PopFloat()
			Dim isEarlyPhase As Boolean = MyBase.properties.CurrentState.stateName = LevelProperties.ChessRook.States.Main OrElse MyBase.properties.CurrentState.stateName = LevelProperties.ChessRook.States.PhaseTwo
			Dim shootDelay As Single = 0F
			If isEarlyPhase Then
				shootDelay = Global.UnityEngine.Random.Range(EarlyPhaseShootOffsetRange.minimum, EarlyPhaseShootOffsetRange.maximum)
				delay -= EarlyPhaseTransitionOffset
				delay -= shootDelay
			End If
			Yield CupheadTime.WaitForSeconds(Me, delay)
			If isEarlyPhase Then
				MyBase.animator.SetTrigger("SparkAttack")
				Yield MyBase.animator.WaitForAnimationToEnd(Me, "Idle1.Main", False, True)
				MyBase.animator.Play("EarlyActiveA", ChessRookLevelRook.SparkLayerStateIndex)
				Yield CupheadTime.WaitForSeconds(Me, shootDelay)
			End If
			Dim sequence As Char = sequencePattern.PopLetter()
			Dim spawnPosIndex As Integer = 0
			If sequence = "T"c Then
				spawnPosIndex = 0
			ElseIf sequence = "M"c Then
				spawnPosIndex = 1
			ElseIf sequence = "B"c Then
				spawnPosIndex = 2
			End If
			Dim position As Vector3 = Me.straightShotSpawnPoints(spawnPosIndex).position
			Me.straightShot.Create(position, 180F, p.straightShotBulletSpeed)
			Me.smokeEffect.Create(position)
			Me.straightShotSparkEffect.Create(position)
			AudioManager.Play("sfx_dlc_kog_rook_sparks_singles")
		End While
		Return
	End Function

	' Token: 0x06001974 RID: 6516 RVA: 0x000E66EC File Offset: 0x000E4AEC
	Private Sub die()
		Me.dead = True
		Me.StopAllCoroutines()
		AudioManager.Play("sfx_dlc_kog_rook_death")
		AudioManager.[Stop]("sfx_dlc_kog_rook_sparks_loop")
		AudioManager.[Stop]("sfx_dlc_kog_rook_grindingwheel_lowspeed")
		AudioManager.[Stop]("sfx_dlc_kog_rook_grindingwheel_highspeed")
		AudioManager.[Stop]("sfx_dlc_kog_rook_grindingwheel_lowspeed_axeonwheel")
		MyBase.animator.Play("Death", ChessRookLevelRook.BaseLayerStateIndex)
		MyBase.animator.Play("Off", ChessRookLevelRook.SparkLayerStateIndex)
		MyBase.animator.Play("Off", ChessRookLevelRook.WheelLayerStateIndex)
		Me.wheelRenderer.sortingOrder = 1000
		Me.wheelRenderer.sortingLayerName = "Foreground"
	End Sub

	' Token: 0x06001975 RID: 6517 RVA: 0x000E6797 File Offset: 0x000E4B97
	Private Sub SFX_GrindAxe()
		MyBase.StartCoroutine(Me.sfx_grind_axe_cr())
	End Sub

	' Token: 0x06001976 RID: 6518 RVA: 0x000E67A8 File Offset: 0x000E4BA8
	Private Iterator Function sfx_grind_axe_cr() As IEnumerator
		AudioManager.FadeSFXVolume("sfx_dlc_kog_rook_grindingwheel_lowspeed_axeonwheel", 0.7F, 0.1F)
		Yield CupheadTime.WaitForSeconds(Me, 0.3F)
		AudioManager.FadeSFXVolume("sfx_dlc_kog_rook_grindingwheel_lowspeed_axeonwheel", 0.0001F, 0.1F)
		Return
	End Function

	' Token: 0x0400227F RID: 8831
	Private Shared BaseLayerStateIndex As Integer

	' Token: 0x04002280 RID: 8832
	Private Shared SparkLayerStateIndex As Integer = 1

	' Token: 0x04002281 RID: 8833
	Private Shared WheelLayerStateIndex As Integer = 2

	' Token: 0x04002282 RID: 8834
	<SerializeField()>
	Private wheelRenderer As SpriteRenderer

	' Token: 0x04002283 RID: 8835
	<SerializeField()>
	Private cannonballSpawnRoot As Transform

	' Token: 0x04002284 RID: 8836
	<SerializeField()>
	Private cannonballPink As ChessRookLevelPinkCannonBall

	' Token: 0x04002285 RID: 8837
	<SerializeField()>
	Private cannonballRegular As ChessRookLevelRegularCannonball

	' Token: 0x04002286 RID: 8838
	<SerializeField()>
	Private straightShot As BasicProjectile

	' Token: 0x04002287 RID: 8839
	<SerializeField()>
	Private straightShotSpawnPoints As Transform()

	' Token: 0x04002288 RID: 8840
	<SerializeField()>
	Private hitSparkEffect As Effect

	' Token: 0x04002289 RID: 8841
	<SerializeField()>
	Private straightShotSparkEffect As Effect

	' Token: 0x0400228A RID: 8842
	<SerializeField()>
	Private smokeEffect As Effect

	' Token: 0x0400228B RID: 8843
	<SerializeField()>
	Private spawnEffect As Animator

	' Token: 0x0400228C RID: 8844
	<SerializeField()>
	Private hitFlash As HitFlash

	' Token: 0x0400228D RID: 8845
	Private damageDealer As DamageDealer

	' Token: 0x0400228E RID: 8846
	Private transitionCoroutine As Coroutine

	' Token: 0x0400228F RID: 8847
	Private dead As Boolean

	' Token: 0x04002290 RID: 8848
	Private headTypeB As Boolean

	' Token: 0x04002291 RID: 8849
	Private headZOffset As Integer
End Class
