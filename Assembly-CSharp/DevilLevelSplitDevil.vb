Imports System
Imports UnityEngine

' Token: 0x02000583 RID: 1411
Public Class DevilLevelSplitDevil
	Inherits LevelProperties.Devil.Entity

	' Token: 0x06001AF2 RID: 6898 RVA: 0x000F7D70 File Offset: 0x000F6170
	Protected Overrides Sub Awake()
		MyBase.Awake()
		MyBase.animator.Play("Idle")
		Me.state = DevilLevelSplitDevil.State.Idle
	End Sub

	' Token: 0x06001AF3 RID: 6899 RVA: 0x000F7D90 File Offset: 0x000F6190
	Private Sub LateUpdate()
		Dim levelPlayerController As LevelPlayerController = TryCast(PlayerManager.GetPlayer(PlayerId.PlayerOne), LevelPlayerController)
		Dim levelPlayerController2 As LevelPlayerController = TryCast(PlayerManager.GetPlayer(PlayerId.PlayerTwo), LevelPlayerController)
		Dim flag As Boolean = levelPlayerController Is Nothing OrElse levelPlayerController.transform.localScale.x > 0F
		Me.headsControler.SetBool("LookRight", flag)
		MyBase.animator.SetBool("LookRight", flag)
		Me.headsControler.SetBool("DevilLeft", Me.DevilLeft)
	End Sub

	' Token: 0x06001AF4 RID: 6900 RVA: 0x000F7E18 File Offset: 0x000F6218
	Public Sub OnIdleLeftEnd()
		Dim bool As Boolean = MyBase.animator.GetBool("Shoot")
		Dim bool2 As Boolean = MyBase.animator.GetBool("LookRight")
		Dim text As String = "DevilLeftShootTransition_2_3_4"
		Dim text2 As String = "DevilLeftIdleBody_2_3_4"
		If bool Then
			If bool2 Then
				text = "DevilToAngel_Transition_2_3_4"
				text2 = "DevilToAngelIdleBody_2_3_4"
			End If
			Me.headsControler.enabled = True
			Me.headsControler.SetBool("Shoot", True)
			Me.headsControler.Play(text, -1, 1F)
			MyBase.animator.Play(text2, -1, 1F)
		End If
	End Sub

	' Token: 0x06001AF5 RID: 6901 RVA: 0x000F7EAC File Offset: 0x000F62AC
	Public Sub OnIdleRightEnd()
		Dim bool As Boolean = MyBase.animator.GetBool("Shoot")
		Dim bool2 As Boolean = MyBase.animator.GetBool("LookRight")
		Dim text As String = "DevilRightShootTransition_2_3_4"
		Dim text2 As String = "DevilRightIdleBody_2_3_4"
		If bool Then
			If Not bool2 Then
				text = "AngelToDevil_transition_2_3_4"
				text2 = "AngelToDevilIdleBody_2_3_4"
			End If
			Me.headsControler.enabled = True
			Me.headsControler.SetBool("Shoot", True)
			Me.headsControler.Play(text, -1, 1F)
			MyBase.animator.Play(text2, -1, 1F)
		End If
	End Sub

	' Token: 0x06001AF6 RID: 6902 RVA: 0x000F7F40 File Offset: 0x000F6340
	Public Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		MyBase.properties.DealDamage(info.damage)
	End Sub

	' Token: 0x06001AF7 RID: 6903 RVA: 0x000F7F53 File Offset: 0x000F6353
	Public Sub StartTransform()
		MyBase.animator.SetTrigger("IsDead")
	End Sub

	' Token: 0x06001AF8 RID: 6904 RVA: 0x000F7F65 File Offset: 0x000F6365
	Public Sub OnDeadAnimationDone()
		Me.SplitDevilAnimationDone = True
		MyBase.gameObject.SetActive(False)
	End Sub

	' Token: 0x0400242C RID: 9260
	Public state As DevilLevelSplitDevil.State

	' Token: 0x0400242D RID: 9261
	<SerializeField()>
	Private headsControler As Animator

	' Token: 0x0400242E RID: 9262
	Public DevilLeft As Boolean = True

	' Token: 0x0400242F RID: 9263
	Public SplitDevilAnimationDone As Boolean

	' Token: 0x04002430 RID: 9264
	<SerializeField()>
	Private projectilePrefab As DevilLevelSplitDevilProjectile

	' Token: 0x04002431 RID: 9265
	Private AngelprojectilePrefab As DevilLevelSplitDevilProjectile

	' Token: 0x04002432 RID: 9266
	<SerializeField()>
	Private projectileRootLeft As Transform

	' Token: 0x04002433 RID: 9267
	<SerializeField()>
	Private projectileRootRight As Transform

	' Token: 0x04002434 RID: 9268
	Private patternIndex As Integer

	' Token: 0x04002435 RID: 9269
	Private pattern As LevelProperties.Devil.Pattern

	' Token: 0x02000584 RID: 1412
	Public Enum State
		' Token: 0x04002437 RID: 9271
		Idle
		' Token: 0x04002438 RID: 9272
		Shoot
		' Token: 0x04002439 RID: 9273
		summon
	End Enum
End Class
