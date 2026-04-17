Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020008DC RID: 2268
Public Class MountainPlatformingLevelCyclopsRocks
	Inherits AbstractPausableComponent

	' Token: 0x06003514 RID: 13588 RVA: 0x001EDA57 File Offset: 0x001EBE57
	Private Sub Start()
		Me.cyclopsState = MountainPlatformingLevelCyclopsRocks.CyclopsState.UnSpawned
		MyBase.StartCoroutine(Me.start_trigger_cr())
		Me.cyclopsAnimator = Me.cyclopsBG.GetComponent(Of Animator)()
	End Sub

	' Token: 0x06003515 RID: 13589 RVA: 0x001EDA80 File Offset: 0x001EBE80
	Private Iterator Function start_trigger_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 0.2F)
		Me.player = PlayerManager.GetNext()
		While Me.player.transform.position.x < Me.onTrigger.transform.position.x
			Yield Nothing
			If Me.player Is Nothing OrElse Me.player.IsDead Then
				Me.player = PlayerManager.GetNext()
			End If
		End While
		Me.StartCyclops()
		While Me.cyclopsState <> MountainPlatformingLevelCyclopsRocks.CyclopsState.Dead
			If Me.player Is Nothing OrElse Me.player.IsDead Then
				Me.player = PlayerManager.GetNext()
			End If
			Me.playerInTrigger = Me.player.transform.position.x > Me.onTrigger.transform.position.x
			If Me.player.transform.position.x > Me.offTrigger.transform.position.x Then
				Me.cyclopsBG.isDead = True
				Me.cyclopsState = MountainPlatformingLevelCyclopsRocks.CyclopsState.Dead
				Exit While
			End If
			Yield Nothing
		End While
		Yield Nothing
		Return
	End Function

	' Token: 0x06003516 RID: 13590 RVA: 0x001EDA9C File Offset: 0x001EBE9C
	Private Sub StartCyclops()
		Me.IsIdle = True
		Me.playerInTrigger = True
		Me.cyclopsBG.start = Me.cyclopsBG.transform.position
		Me.cyclopsAnimator.SetTrigger("StartCyclops")
		Me.cyclopsAnimator.SetBool("isIdle", Me.IsIdle)
		Me.cyclopsState = MountainPlatformingLevelCyclopsRocks.CyclopsState.Spawned
		MyBase.StartCoroutine(Me.walk_and_idle_cr())
		MyBase.StartCoroutine(Me.attack_cr())
		MyBase.StartCoroutine(Me.move_cr())
	End Sub

	' Token: 0x06003517 RID: 13591 RVA: 0x001EDB28 File Offset: 0x001EBF28
	Private Iterator Function turn_cyclops_cr() As IEnumerator
		If Me.cyclopsState <> MountainPlatformingLevelCyclopsRocks.CyclopsState.Turning Then
			Dim ani As String = If(Me.IsIdle, "Turn", "Turn_To_Walk")
			Me.cyclopsAnimator.SetTrigger("OnTurn")
			Me.cyclopsState = MountainPlatformingLevelCyclopsRocks.CyclopsState.Turning
			Yield Me.cyclopsAnimator.WaitForAnimationToEnd(Me, ani, False, True)
			Me.facingLeft = Me.cyclopsBG.transform.localScale.x = 1F
			Me.cyclopsState = MountainPlatformingLevelCyclopsRocks.CyclopsState.Spawned
		End If
		Return
	End Function

	' Token: 0x06003518 RID: 13592 RVA: 0x001EDB44 File Offset: 0x001EBF44
	Private Iterator Function walk_and_idle_cr() As IEnumerator
		Me.facingLeft = True
		Dim t As Single = 0F
		Dim timer As Single = 1F
		While Me.cyclopsState <> MountainPlatformingLevelCyclopsRocks.CyclopsState.Dead
			If Me.cyclopsState = MountainPlatformingLevelCyclopsRocks.CyclopsState.Spawned Then
				If Me.IsIdle Then
					If Me.player.transform.position.x < Me.cyclopsBG.transform.position.x + Me.cyclopsStopOffset AndAlso Me.player.transform.position.x > Me.cyclopsBG.transform.position.x - Me.cyclopsStopOffset Then
						If Me.player.transform.position.x < Me.cyclopsBG.transform.position.x AndAlso Not Me.facingLeft Then
							Yield MyBase.StartCoroutine(Me.turn_cyclops_cr())
						ElseIf Me.player.transform.position.x > Me.cyclopsBG.transform.position.x AndAlso Me.facingLeft Then
							Yield MyBase.StartCoroutine(Me.turn_cyclops_cr())
						End If
					Else
						Me.IsIdle = False
						Me.cyclopsAnimator.SetBool("isIdle", Me.IsIdle)
						Yield Me.cyclopsAnimator.WaitForAnimationToEnd(Me, "Idle_To_Walk", False, True)
					End If
				ElseIf Me.player.transform.position.x < Me.cyclopsBG.transform.position.x - Me.cyclopsStopOffset AndAlso Not Me.facingLeft Then
					Yield MyBase.StartCoroutine(Me.turn_cyclops_cr())
				ElseIf Me.player.transform.position.x > Me.cyclopsBG.transform.position.x + Me.cyclopsStopOffset AndAlso Me.facingLeft Then
					Yield MyBase.StartCoroutine(Me.turn_cyclops_cr())
				ElseIf t < timer Then
					t += CupheadTime.Delta
				Else
					Me.IsIdle = True
					Me.cyclopsAnimator.SetBool("isIdle", Me.IsIdle)
					Yield Me.cyclopsAnimator.WaitForAnimationToEnd(Me, "Walk_To_Idle", False, True)
					t = 0F
				End If
			End If
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06003519 RID: 13593 RVA: 0x001EDB60 File Offset: 0x001EBF60
	Private Iterator Function move_cr() As IEnumerator
		While Me.cyclopsBG IsNot Nothing
			If Me.cyclopsBG.isWalking Then
				Me.cyclopsBG.transform.AddPosition(If((Not Me.facingLeft), Me.walkSpeed, (-Me.walkSpeed)) * CupheadTime.Delta, 0F, 0F)
			End If
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x0600351A RID: 13594 RVA: 0x001EDB7C File Offset: 0x001EBF7C
	Private Iterator Function attack_cr() As IEnumerator
		While Me.cyclopsState <> MountainPlatformingLevelCyclopsRocks.CyclopsState.Dead
			Yield CupheadTime.WaitForSeconds(Me, Me.attackDelayRange.RandomFloat())
			While Not Me.playerInTrigger OrElse Me.cyclopsState = MountainPlatformingLevelCyclopsRocks.CyclopsState.Turning
				Yield Nothing
			End While
			Me.cyclopsBG.GetPlayer(Me.player)
			Me.cyclopsAnimator.SetTrigger("OnAttack")
			Yield Me.cyclopsAnimator.WaitForAnimationToStart(Me, "Attack_Start", False)
			Me.cyclopsState = MountainPlatformingLevelCyclopsRocks.CyclopsState.Attacking
			If Me.IsIdle Then
				Yield Me.cyclopsAnimator.WaitForAnimationToEnd(Me, "Attack_To_Idle", False, True)
			Else
				Yield Me.cyclopsAnimator.WaitForAnimationToEnd(Me, "Attack_To_Walk", False, True)
			End If
			If Me.player.transform.position.x < Me.cyclopsBG.transform.position.x AndAlso Not Me.facingLeft Then
				Yield MyBase.StartCoroutine(Me.turn_cyclops_cr())
			ElseIf Me.player.transform.position.x > Me.cyclopsBG.transform.position.x AndAlso Me.facingLeft Then
				Yield MyBase.StartCoroutine(Me.turn_cyclops_cr())
			Else
				Me.cyclopsState = MountainPlatformingLevelCyclopsRocks.CyclopsState.Spawned
			End If
			Yield CupheadTime.WaitForSeconds(Me, 0.1F)
		End While
		Me.cyclopsAnimator.SetTrigger("OnAttack")
		Yield Nothing
		Return
	End Function

	' Token: 0x0600351B RID: 13595 RVA: 0x001EDB98 File Offset: 0x001EBF98
	Protected Overrides Sub OnDrawGizmos()
		MyBase.OnDrawGizmos()
		Gizmos.color = New Color(0F, 0F, 1F, 1F)
		Gizmos.DrawLine(Me.offTrigger.transform.position, New Vector3(Me.offTrigger.transform.position.x, 5000F, 0F))
		Gizmos.DrawLine(Me.onTrigger.transform.position, New Vector3(Me.onTrigger.transform.position.x, 5000F, 0F))
		Gizmos.color = New Color(1F, 0F, 1F, 1F)
		If Me.cyclopsBG Then
			Gizmos.DrawLine(New Vector3(Me.cyclopsBG.transform.position.x + Me.cyclopsStopOffset, Me.cyclopsBG.transform.position.y), New Vector3(Me.cyclopsBG.transform.position.x - Me.cyclopsStopOffset, Me.cyclopsBG.transform.position.y))
		End If
	End Sub

	' Token: 0x04003D3A RID: 15674
	<SerializeField()>
	Private walkSpeed As Single

	' Token: 0x04003D3B RID: 15675
	<SerializeField()>
	Private attackDelayRange As MinMax

	' Token: 0x04003D3C RID: 15676
	<SerializeField()>
	Private onTrigger As Transform

	' Token: 0x04003D3D RID: 15677
	<SerializeField()>
	Private offTrigger As Transform

	' Token: 0x04003D3E RID: 15678
	<SerializeField()>
	Private cyclopsBG As MountainPlatformingLevelCyclopsBG

	' Token: 0x04003D3F RID: 15679
	<SerializeField()>
	Private cyclopsStopOffset As Single

	' Token: 0x04003D40 RID: 15680
	Private player As AbstractPlayerController

	' Token: 0x04003D41 RID: 15681
	Private cyclopsState As MountainPlatformingLevelCyclopsRocks.CyclopsState

	' Token: 0x04003D42 RID: 15682
	Private IsIdle As Boolean

	' Token: 0x04003D43 RID: 15683
	Private playerInTrigger As Boolean

	' Token: 0x04003D44 RID: 15684
	Private facingLeft As Boolean

	' Token: 0x04003D45 RID: 15685
	Private cyclopsAnimator As Animator

	' Token: 0x020008DD RID: 2269
	Private Enum CyclopsState
		' Token: 0x04003D47 RID: 15687
		UnSpawned
		' Token: 0x04003D48 RID: 15688
		Spawned
		' Token: 0x04003D49 RID: 15689
		Turning
		' Token: 0x04003D4A RID: 15690
		Attacking
		' Token: 0x04003D4B RID: 15691
		Dead
	End Enum
End Class
