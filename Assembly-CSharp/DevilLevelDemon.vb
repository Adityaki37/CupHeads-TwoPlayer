Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x0200056E RID: 1390
Public Class DevilLevelDemon
	Inherits AbstractCollidableObject

	' Token: 0x1700034B RID: 843
	' (get) Token: 0x06001A48 RID: 6728 RVA: 0x000F0EF3 File Offset: 0x000EF2F3
	' (set) Token: 0x06001A49 RID: 6729 RVA: 0x000F0EFB File Offset: 0x000EF2FB
	Public Property JumpRoot As Vector3

	' Token: 0x1700034C RID: 844
	' (get) Token: 0x06001A4A RID: 6730 RVA: 0x000F0F04 File Offset: 0x000EF304
	' (set) Token: 0x06001A4B RID: 6731 RVA: 0x000F0F0C File Offset: 0x000EF30C
	Public Property RunRoot As Vector3

	' Token: 0x1700034D RID: 845
	' (get) Token: 0x06001A4C RID: 6732 RVA: 0x000F0F15 File Offset: 0x000EF315
	' (set) Token: 0x06001A4D RID: 6733 RVA: 0x000F0F1D File Offset: 0x000EF31D
	Public Property PillarDestination As Vector3

	' Token: 0x1700034E RID: 846
	' (get) Token: 0x06001A4E RID: 6734 RVA: 0x000F0F26 File Offset: 0x000EF326
	' (set) Token: 0x06001A4F RID: 6735 RVA: 0x000F0F2E File Offset: 0x000EF32E
	Public Property FrontSpawn As Vector3

	' Token: 0x06001A50 RID: 6736 RVA: 0x000F0F37 File Offset: 0x000EF337
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.damageDealer = DamageDealer.NewEnemy()
		Me.damageReceiver = MyBase.GetComponent(Of DamageReceiver)()
		AddHandler Me.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
	End Sub

	' Token: 0x06001A51 RID: 6737 RVA: 0x000F0F6D File Offset: 0x000EF36D
	Private Sub Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x06001A52 RID: 6738 RVA: 0x000F0F85 File Offset: 0x000EF385
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If Me.damageDealer IsNot Nothing AndAlso phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x06001A53 RID: 6739 RVA: 0x000F0FB0 File Offset: 0x000EF3B0
	Public Function Create(position As Vector2, direction As Single, speed As Single, hp As Single, parent As DevilLevelSittingDevil) As DevilLevelDemon
		Dim devilLevelDemon As DevilLevelDemon = Me.InstantiatePrefab(Of DevilLevelDemon)()
		devilLevelDemon.transform.position = position
		devilLevelDemon.frontDirection = direction
		devilLevelDemon.speed = speed
		devilLevelDemon.hp = hp
		devilLevelDemon.parent = parent
		devilLevelDemon.transform.localScale = New Vector3(direction * 0.9F, 0.9F, 0.9F)
		devilLevelDemon.sprite.color = Me.backgroundTint
		Dim num As Integer = Global.UnityEngine.Random.Range(0, 3)
		If num = 0 Then
			devilLevelDemon.animator.SetFloat("PeekVariation", CSng(Global.UnityEngine.Random.Range(0, 3)) / 2F)
			devilLevelDemon.animator.SetTrigger("JumpOut")
		ElseIf num = 1 Then
			devilLevelDemon.animator.SetFloat("PeekVariation", CSng(Global.UnityEngine.Random.Range(0, 3)) / 2F)
			devilLevelDemon.animator.SetTrigger("RunOut")
		Else
			devilLevelDemon.animator.Play("JumpOut")
		End If
		Return devilLevelDemon
	End Function

	' Token: 0x06001A54 RID: 6740 RVA: 0x000F10B1 File Offset: 0x000EF4B1
	Private Sub Start()
		Dim devilLevelSittingDevil As DevilLevelSittingDevil = Me.parent
		devilLevelSittingDevil.OnPhase1Death = CType([Delegate].Combine(devilLevelSittingDevil.OnPhase1Death, AddressOf Me.Die), Action)
	End Sub

	' Token: 0x06001A55 RID: 6741 RVA: 0x000F10DA File Offset: 0x000EF4DA
	Public Sub PlaceForJump()
		MyBase.transform.position = Me.JumpRoot
		Me.hasJumped = True
	End Sub

	' Token: 0x06001A56 RID: 6742 RVA: 0x000F10F4 File Offset: 0x000EF4F4
	Public Sub StartMoving()
		If Not Me.moving Then
			MyBase.StartCoroutine(Me.demonMovement_cr())
			AudioManager.Play("devil_imp_spawn")
			Me.emitAudioFromObject.Add("devil_imp_spawn")
		End If
	End Sub

	' Token: 0x06001A57 RID: 6743 RVA: 0x000F1128 File Offset: 0x000EF528
	Protected Iterator Function demonMovement_cr() As IEnumerator
		Me.moving = True
		If Not Me.hasJumped Then
			MyBase.transform.position = Me.RunRoot
		End If
		Dim backDirection As Vector3 = (Me.PillarDestination - MyBase.transform.position).normalized
		While CupheadLevelCamera.Current.ContainsPoint(MyBase.transform.position, New Vector2(150F, 150F))
			MyBase.transform.position += backDirection * Me.speed * CupheadTime.Delta
			Dim scaleDelta As Single = 0.099999964F * CupheadTime.Delta
			MyBase.transform.localScale -= New Vector3(Me.frontDirection * scaleDelta, scaleDelta, scaleDelta)
			Yield Nothing
		End While
		Yield CupheadTime.WaitForSeconds(Me, Me.frontWaitTime)
		MyBase.transform.localScale = New Vector3(-Me.frontDirection, 1F, 1F)
		MyBase.transform.position = Me.FrontSpawn
		Me.collider2d.enabled = True
		Me.sprite.sortingLayerName = "Enemies"
		Me.sprite.sortingOrder = 0
		Me.sprite.color = Color.black
		While True
			MyBase.transform.AddPosition(Me.frontDirection * Me.speed * CupheadTime.Delta, 0F, 0F)
			If CupheadLevelCamera.Current.ContainsPoint(MyBase.transform.position, New Vector2(150F, 150F)) Then
				Me.enteredScreen = True
			ElseIf Me.enteredScreen Then
				Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
			End If
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06001A58 RID: 6744 RVA: 0x000F1143 File Offset: 0x000EF543
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		Me.hp -= info.damage
		If Me.hp < 0F Then
			Me.Die()
		End If
	End Sub

	' Token: 0x06001A59 RID: 6745 RVA: 0x000F116E File Offset: 0x000EF56E
	Private Sub RemoveEvent()
		Dim devilLevelSittingDevil As DevilLevelSittingDevil = Me.parent
		devilLevelSittingDevil.OnPhase1Death = CType([Delegate].Remove(devilLevelSittingDevil.OnPhase1Death, AddressOf Me.Die), Action)
	End Sub

	' Token: 0x06001A5A RID: 6746 RVA: 0x000F1197 File Offset: 0x000EF597
	Protected Overrides Sub OnDestroy()
		Me.RemoveEvent()
		MyBase.OnDestroy()
	End Sub

	' Token: 0x06001A5B RID: 6747 RVA: 0x000F11A8 File Offset: 0x000EF5A8
	Private Sub Die()
		AudioManager.Play("devil_imp_death")
		Me.emitAudioFromObject.Add("devil_imp_death")
		Me.explosion.Create(Me.collider2d.bounds.center)
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub

	' Token: 0x06001A5C RID: 6748 RVA: 0x000F11F9 File Offset: 0x000EF5F9
	Private Sub ImpStepSFX()
		AudioManager.Play("devil_imp_step")
		Me.emitAudioFromObject.Add("devil_imp_step")
	End Sub

	' Token: 0x0400236C RID: 9068
	Private Const PeekVariationParameterName As String = "PeekVariation"

	' Token: 0x0400236D RID: 9069
	Private Const JumpOutParameterName As String = "JumpOut"

	' Token: 0x0400236E RID: 9070
	Private Const RunOutParameterName As String = "RunOut"

	' Token: 0x0400236F RID: 9071
	Private Const JumpOutStateName As String = "JumpOut"

	' Token: 0x04002370 RID: 9072
	Private Const EnemyLayerName As String = "Enemies"

	' Token: 0x04002371 RID: 9073
	Private Const PeekVariations As Integer = 3

	' Token: 0x04002372 RID: 9074
	Private Const StartScale As Single = 0.9F

	' Token: 0x04002373 RID: 9075
	Private Const PillarScale As Single = 0.8F

	' Token: 0x04002374 RID: 9076
	<SerializeField()>
	Private collider2d As Collider2D

	' Token: 0x04002375 RID: 9077
	<SerializeField()>
	Private frontWaitTime As Single

	' Token: 0x04002376 RID: 9078
	<SerializeField()>
	Private sprite As SpriteRenderer

	' Token: 0x04002377 RID: 9079
	<SerializeField()>
	Private backgroundTint As Color

	' Token: 0x04002378 RID: 9080
	<SerializeField()>
	Private explosion As PlatformingLevelGenericExplosion

	' Token: 0x04002379 RID: 9081
	Private damageDealer As DamageDealer

	' Token: 0x0400237A RID: 9082
	Private enteredScreen As Boolean

	' Token: 0x0400237B RID: 9083
	Private frontDirection As Single

	' Token: 0x0400237C RID: 9084
	Private speed As Single

	' Token: 0x0400237D RID: 9085
	Private hp As Single

	' Token: 0x0400237E RID: 9086
	Private moving As Boolean

	' Token: 0x0400237F RID: 9087
	Private hasJumped As Boolean

	' Token: 0x04002380 RID: 9088
	Private damageReceiver As DamageReceiver

	' Token: 0x04002381 RID: 9089
	Private parent As DevilLevelSittingDevil
End Class
