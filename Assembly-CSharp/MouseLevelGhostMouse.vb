Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020006EF RID: 1775
Public Class MouseLevelGhostMouse
	Inherits AbstractCollidableObject

	' Token: 0x170003C7 RID: 967
	' (get) Token: 0x06002605 RID: 9733 RVA: 0x00163B73 File Offset: 0x00161F73
	' (set) Token: 0x06002606 RID: 9734 RVA: 0x00163B7B File Offset: 0x00161F7B
	Public Property state As MouseLevelGhostMouse.State

	' Token: 0x06002607 RID: 9735 RVA: 0x00163B84 File Offset: 0x00161F84
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.basePos = MyBase.transform.localPosition
		AddHandler MyBase.GetComponent(Of DamageReceiver)().OnDamageTaken, AddressOf Me.OnDamageTaken
	End Sub

	' Token: 0x06002608 RID: 9736 RVA: 0x00163BB9 File Offset: 0x00161FB9
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		Me.hp -= info.damage
		If Me.hp < 0F AndAlso Me.state <> MouseLevelGhostMouse.State.Dying Then
			Me.Die()
		End If
	End Sub

	' Token: 0x06002609 RID: 9737 RVA: 0x00163BF0 File Offset: 0x00161FF0
	Public Sub Spawn(properties As LevelProperties.Mouse)
		Me.properties = properties
		If Me.state = MouseLevelGhostMouse.State.Unspawned Then
			Me.StopAllCoroutines()
			Me.state = MouseLevelGhostMouse.State.Intro
			MyBase.animator.ResetTrigger("AttackBlue")
			MyBase.animator.ResetTrigger("AttackPink")
			MyBase.animator.ResetTrigger("Continue")
			MyBase.StartCoroutine(Me.spawn_cr())
		End If
	End Sub

	' Token: 0x0600260A RID: 9738 RVA: 0x00163C5C File Offset: 0x0016205C
	Private Iterator Function spawn_cr() As IEnumerator
		Dim spawnOffset As Single = 150F * MyBase.transform.localScale.x
		Dim yPos As Single = Me.basePos.y + Global.UnityEngine.Random.Range(-35F, 35F)
		Dim start As Vector2 = New Vector2(Me.basePos.x * 0.125F + spawnOffset, yPos)
		Me.hp = Me.properties.CurrentState.ghostMouse.hp
		MyBase.transform.SetEulerAngles(New Single?(0F), New Single?(0F), New Single?(0F))
		MyBase.animator.SetTrigger("Spawn")
		Dim t As Single = 0F
		While t < 1.083F
			MyBase.transform.SetLocalPosition(New Single?(EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, start.x, Me.basePos.x, t / 1.083F)), New Single?(yPos), Nothing)
			t += CupheadTime.Delta
			Yield Nothing
		End While
		MyBase.transform.SetLocalPosition(New Single?(Me.basePos.x), New Single?(yPos), Nothing)
		Yield MyBase.animator.WaitForAnimationToStart(Me, "Idle_A", False)
		Me.state = MouseLevelGhostMouse.State.Idle
		Return
	End Function

	' Token: 0x0600260B RID: 9739 RVA: 0x00163C77 File Offset: 0x00162077
	Public Sub Attack(pink As Boolean)
		Me.state = MouseLevelGhostMouse.State.Attack
		MyBase.StartCoroutine(Me.attack_cr(pink))
	End Sub

	' Token: 0x0600260C RID: 9740 RVA: 0x00163C90 File Offset: 0x00162090
	Private Iterator Function attack_cr(pink As Boolean) As IEnumerator
		MyBase.animator.SetTrigger(If((Not pink), "AttackBlue", "AttackPink"))
		Yield MyBase.animator.WaitForAnimationToStart(Me, If((Not pink), "Attack_Blue_Loop", "Attack_Pink_Loop"), False)
		Yield CupheadTime.WaitForSeconds(Me, Me.properties.CurrentState.ghostMouse.attackAnticipation)
		MyBase.animator.SetTrigger("Continue")
		Yield MyBase.animator.WaitForAnimationToStart(Me, "Idle_B", False)
		Me.state = MouseLevelGhostMouse.State.Idle
		Return
	End Function

	' Token: 0x0600260D RID: 9741 RVA: 0x00163CB4 File Offset: 0x001620B4
	Private Sub FireBlue()
		Me.blueBallPrefab.Create(Me.projectileRoot.position, Me.properties.CurrentState.ghostMouse.ballSpeed, Me.properties.CurrentState.ghostMouse.splitSpeed)
	End Sub

	' Token: 0x0600260E RID: 9742 RVA: 0x00163D08 File Offset: 0x00162108
	Private Sub FirePink()
		Me.pinkBallPrefab.Create(Me.projectileRoot.position, Me.properties.CurrentState.ghostMouse.ballSpeed, Me.properties.CurrentState.ghostMouse.splitSpeed)
	End Sub

	' Token: 0x0600260F RID: 9743 RVA: 0x00163D5B File Offset: 0x0016215B
	Public Sub Die()
		If Me.state = MouseLevelGhostMouse.State.Unspawned OrElse Me.state = MouseLevelGhostMouse.State.Dying Then
			Return
		End If
		Me.state = MouseLevelGhostMouse.State.Dying
		Me.StopAllCoroutines()
		MyBase.StartCoroutine(Me.death_cr())
	End Sub

	' Token: 0x06002610 RID: 9744 RVA: 0x00163D90 File Offset: 0x00162190
	Private Iterator Function death_cr() As IEnumerator
		While Me.state = MouseLevelGhostMouse.State.Intro
			Yield Nothing
		End While
		MyBase.animator.SetTrigger("Die")
		MyBase.transform.Rotate(0F, 0F, CSng(Global.UnityEngine.Random.Range(-16, 16)))
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Death", False, True)
		Me.state = MouseLevelGhostMouse.State.Unspawned
		Return
	End Function

	' Token: 0x06002611 RID: 9745 RVA: 0x00163DAB File Offset: 0x001621AB
	Private Sub SoundMouseGhostWail()
		AudioManager.Play("level_mouse_ghost_mouse_wail")
		Me.emitAudioFromObject.Add("level_mouse_ghost_mouse_wail")
	End Sub

	' Token: 0x06002612 RID: 9746 RVA: 0x00163DC7 File Offset: 0x001621C7
	Private Sub SoundMouseGhostLaugh()
		AudioManager.Play("level_mouse_ghost_mouse_laugh")
		Me.emitAudioFromObject.Add("level_mouse_ghost_mouse_laugh")
	End Sub

	' Token: 0x06002613 RID: 9747 RVA: 0x00163DE3 File Offset: 0x001621E3
	Private Sub SoundMouseGhostAttack()
		AudioManager.Play("level_mouse_ghost_attack")
		Me.emitAudioFromObject.Add("level_mouse_ghost_attack")
	End Sub

	' Token: 0x06002614 RID: 9748 RVA: 0x00163DFF File Offset: 0x001621FF
	Private Sub SoundMouseGhostDeath()
		AudioManager.Play("level_mouse_ghost_death")
		Me.emitAudioFromObject.Add("level_mouse_ghost_death")
	End Sub

	' Token: 0x06002615 RID: 9749 RVA: 0x00163E1B File Offset: 0x0016221B
	Private Sub SoundMouseGhostDeathStart()
		AudioManager.Play("level_mouse_ghost_death_start")
		Me.emitAudioFromObject.Add("level_mouse_ghost_death_start")
	End Sub

	' Token: 0x06002616 RID: 9750 RVA: 0x00163E37 File Offset: 0x00162237
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me.blueBallPrefab = Nothing
		Me.pinkBallPrefab = Nothing
	End Sub

	' Token: 0x04002E89 RID: 11913
	Private Const heightVariation As Single = 35F

	' Token: 0x04002E8A RID: 11914
	Private Const spawnXRatio As Single = 0.125F

	' Token: 0x04002E8C RID: 11916
	Private basePos As Vector2

	' Token: 0x04002E8D RID: 11917
	Private properties As LevelProperties.Mouse

	' Token: 0x04002E8E RID: 11918
	Private hp As Single

	' Token: 0x04002E8F RID: 11919
	<SerializeField()>
	Private blueBallPrefab As MouseLevelGhostMouseBall

	' Token: 0x04002E90 RID: 11920
	<SerializeField()>
	Private pinkBallPrefab As MouseLevelGhostMouseBall

	' Token: 0x04002E91 RID: 11921
	<SerializeField()>
	Private projectileRoot As Transform

	' Token: 0x020006F0 RID: 1776
	Public Enum State
		' Token: 0x04002E93 RID: 11923
		Unspawned
		' Token: 0x04002E94 RID: 11924
		Intro
		' Token: 0x04002E95 RID: 11925
		Idle
		' Token: 0x04002E96 RID: 11926
		Attack
		' Token: 0x04002E97 RID: 11927
		Dying
	End Enum
End Class
