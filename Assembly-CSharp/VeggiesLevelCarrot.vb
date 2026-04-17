Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports System.Diagnostics
Imports UnityEngine

' Token: 0x02000842 RID: 2114
Public Class VeggiesLevelCarrot
	Inherits LevelProperties.Veggies.Entity

	' Token: 0x17000423 RID: 1059
	' (get) Token: 0x060030ED RID: 12525 RVA: 0x001CBFE2 File Offset: 0x001CA3E2
	' (set) Token: 0x060030EE RID: 12526 RVA: 0x001CBFEA File Offset: 0x001CA3EA
	Public Property state As VeggiesLevelCarrot.State

	' Token: 0x1400005B RID: 91
	' (add) Token: 0x060030EF RID: 12527 RVA: 0x001CBFF4 File Offset: 0x001CA3F4
	' (remove) Token: 0x060030F0 RID: 12528 RVA: 0x001CC02C File Offset: 0x001CA42C
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnDeathEvent As Action

	' Token: 0x1400005C RID: 92
	' (add) Token: 0x060030F1 RID: 12529 RVA: 0x001CC064 File Offset: 0x001CA464
	' (remove) Token: 0x060030F2 RID: 12530 RVA: 0x001CC09C File Offset: 0x001CA49C
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnDamageTakenEvent As VeggiesLevelCarrot.OnDamageTakenHandler

	' Token: 0x060030F3 RID: 12531 RVA: 0x001CC0D4 File Offset: 0x001CA4D4
	Private Sub Start()
		MyBase.GetComponent(Of Collider2D)().enabled = False
		Me.mindLoop = Global.UnityEngine.[Object].Instantiate(Of AudioSource)(Me.mindLoopPrefab)
		Dim list As List(Of Transform) = New List(Of Transform)(Me.homingRoot.GetComponentsInChildren(Of Transform)())
		list.Remove(Me.homingRoot)
		Me.homingRoots = list.ToArray()
		Me.SfxGround()
	End Sub

	' Token: 0x060030F4 RID: 12532 RVA: 0x001CC12E File Offset: 0x001CA52E
	Private Sub Update()
		If PauseManager.state = PauseManager.State.Paused Then
			Me.mindLoop.Pause()
		Else
			Me.mindLoop.UnPause()
		End If
	End Sub

	' Token: 0x060030F5 RID: 12533 RVA: 0x001CC156 File Offset: 0x001CA556
	Public Overrides Sub OnLevelEnd()
		MyBase.OnLevelEnd()
		Me.mindLoop.[Stop]()
	End Sub

	' Token: 0x060030F6 RID: 12534 RVA: 0x001CC16C File Offset: 0x001CA56C
	Public Overrides Sub LevelInit(properties As LevelProperties.Veggies)
		MyBase.LevelInit(properties)
		Me.carrot = MyBase.properties.CurrentState.carrot
		Me.hp = CSng(Me.carrot.hp)
		AddHandler MyBase.GetComponent(Of DamageReceiver)().OnDamageTaken, AddressOf Me.OnDamageTaken
	End Sub

	' Token: 0x060030F7 RID: 12535 RVA: 0x001CC1C0 File Offset: 0x001CA5C0
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		If Me.dead Then
			Return
		End If
		If Me.OnDamageTakenEvent IsNot Nothing Then
			Me.OnDamageTakenEvent(info.damage)
		End If
		Me.hp -= info.damage
		If Me.hp <= 0F Then
			Me.Die()
		End If
	End Sub

	' Token: 0x060030F8 RID: 12536 RVA: 0x001CC21E File Offset: 0x001CA61E
	Private Sub OnInAnimComplete()
		MyBase.transform.GetComponent(Of Collider2D)().enabled = True
		MyBase.StartCoroutine(Me.rings_cr())
	End Sub

	' Token: 0x060030F9 RID: 12537 RVA: 0x001CC23E File Offset: 0x001CA63E
	Private Sub Die()
		Me.dead = True
		Me.StopAllCoroutines()
		MyBase.StartCoroutine(Me.die_cr())
	End Sub

	' Token: 0x060030FA RID: 12538 RVA: 0x001CC25A File Offset: 0x001CA65A
	Private Sub SfxGround()
		AudioManager.Play("level_veggies_carrot_rise")
	End Sub

	' Token: 0x060030FB RID: 12539 RVA: 0x001CC268 File Offset: 0x001CA668
	Private Sub ShootRegular()
		Me.spark.Create(Me.straightRoot.position)
		Me.straightRoot.LookAt2D(PlayerManager.GetNext().center)
		Me.straightPrefab.Create(Me, Me.straightRoot.position, Me.carrot.bulletSpeed, Me.straightRoot.eulerAngles.z)
	End Sub

	' Token: 0x060030FC RID: 12540 RVA: 0x001CC2DC File Offset: 0x001CA6DC
	Public Sub ShootHoming()
		Me.homingPrefab.Create(PlayerManager.GetNext(), Me, Me.GetHomingRoot(), Me.carrot.homingSpeed, Me.carrot.homingRotation, CSng(Me.carrot.homingHP))
	End Sub

	' Token: 0x060030FD RID: 12541 RVA: 0x001CC318 File Offset: 0x001CA718
	Private Function GetHomingRoot() As Vector2
		Dim position As Vector3 = Me.homingRoots(Global.UnityEngine.Random.Range(0, Me.homingRoots.Length)).position
		Me.homingRoot.SetScale(New Single?(Me.homingRoot.localScale.x * -1F), Nothing, Nothing)
		Return position
	End Function

	' Token: 0x060030FE RID: 12542 RVA: 0x001CC384 File Offset: 0x001CA784
	Private Iterator Function rings_cr() As IEnumerator
		While True
			MyBase.StartCoroutine(Me.carrot_cr())
			Yield CupheadTime.WaitForSeconds(Me, MyBase.properties.CurrentState.carrot.idleRange.RandomFloat())
			Dim count As Integer = 0
			MyBase.animator.SetTrigger("AttackStart")
			Yield MyBase.animator.WaitForAnimationToEnd(Me, "Attack_Start", False, True)
			While count < Me.carrot.bulletCount
				Yield CupheadTime.WaitForSeconds(Me, Me.carrot.bulletDelay * 0.5F)
				Me.ringEffectPrefab.Create(Me.straightRoot.position)
				Yield CupheadTime.WaitForSeconds(Me, Me.carrot.bulletDelay * 0.5F)
				Me.straightRoot.LookAt2D(PlayerManager.GetNext().center)
				For i As Integer = 0 To 5 - 1
					AudioManager.Play("level_veggies_carrot_beam")
					Me.ringPrefab.Create(Me.straightRoot.position, Me.straightRoot.eulerAngles.z, Me.carrot.bulletSpeed)
					Yield CupheadTime.WaitForSeconds(Me, 0.1F)
				Next
				count += 1
			End While
			MyBase.animator.SetTrigger("AttackEnd")
		End While
		Return
	End Function

	' Token: 0x060030FF RID: 12543 RVA: 0x001CC3A0 File Offset: 0x001CA7A0
	Private Iterator Function carrot_cr() As IEnumerator
		Dim bgCount As Integer = 0
		Dim p As LevelProperties.Veggies.Carrot = MyBase.properties.CurrentState.carrot
		AudioManager.Play("level_veggies_mindmeld_start")
		Me.mindLoop.Play()
		Yield CupheadTime.WaitForSeconds(Me, Me.carrot.startIdleTime)
		Dim side As Boolean = False
		bgCount = 0
		Dim numOfCarrots As Integer = p.homingNumOfCarrots.RandomInt()
		While bgCount < numOfCarrots
			Me.bgPrefab.Create(If((Not side), (-1), 1), Me.carrot.homingBgSpeed, Me)
			side = Not side
			bgCount += 1
			Yield CupheadTime.WaitForSeconds(Me, Me.carrot.homingDelay)
		End While
		Return
	End Function

	' Token: 0x06003100 RID: 12544 RVA: 0x001CC3BC File Offset: 0x001CA7BC
	Private Iterator Function die_cr() As IEnumerator
		Me.mindLoop.[Stop]()
		AudioManager.Play("level_veggies_carrot_die")
		If Me.OnDeathEvent IsNot Nothing Then
			Me.OnDeathEvent()
		End If
		MyBase.GetComponent(Of Collider2D)().enabled = False
		MyBase.animator.SetTrigger("Dead")
		Yield Nothing
		MyBase.properties.WinInstantly()
		Return
	End Function

	' Token: 0x04003997 RID: 14743
	<SerializeField()>
	Private mindLoopPrefab As AudioSource

	' Token: 0x04003998 RID: 14744
	Private mindLoop As AudioSource

	' Token: 0x04003999 RID: 14745
	<SerializeField()>
	Private homingRoot As Transform

	' Token: 0x0400399A RID: 14746
	<SerializeField()>
	Private straightRoot As Transform

	' Token: 0x0400399B RID: 14747
	<SerializeField()>
	Private homingPrefab As VeggiesLevelCarrotHomingProjectile

	' Token: 0x0400399C RID: 14748
	<SerializeField()>
	Private straightPrefab As VeggiesLevelCarrotRegularProjectile

	' Token: 0x0400399D RID: 14749
	<SerializeField()>
	Private ringPrefab As BasicProjectile

	' Token: 0x0400399E RID: 14750
	<SerializeField()>
	Private ringEffectPrefab As Effect

	' Token: 0x0400399F RID: 14751
	<SerializeField()>
	Private bgPrefab As VeggiesLevelCarrotBgCarrot

	' Token: 0x040039A0 RID: 14752
	<SerializeField()>
	Private spark As Effect

	' Token: 0x040039A1 RID: 14753
	Private carrot As LevelProperties.Veggies.Carrot

	' Token: 0x040039A2 RID: 14754
	Private homingRoots As Transform()

	' Token: 0x040039A3 RID: 14755
	Private dead As Boolean

	' Token: 0x040039A4 RID: 14756
	Private hp As Single

	' Token: 0x040039A5 RID: 14757
	Private floatingCoroutine As IEnumerator

	' Token: 0x02000843 RID: 2115
	Public Enum Direction
		' Token: 0x040039A9 RID: 14761
		Down
		' Token: 0x040039AA RID: 14762
		DownLeft
		' Token: 0x040039AB RID: 14763
		DownRight
	End Enum

	' Token: 0x02000844 RID: 2116
	Public Enum State
		' Token: 0x040039AD RID: 14765
		Start
		' Token: 0x040039AE RID: 14766
		Complete
	End Enum

	' Token: 0x02000845 RID: 2117
	' (Invoke) Token: 0x06003102 RID: 12546
	Public Delegate Sub OnAttackHandler(direction As VeggiesLevelCarrot.Direction)

	' Token: 0x02000846 RID: 2118
	' (Invoke) Token: 0x06003106 RID: 12550
	Public Delegate Sub OnDamageTakenHandler(damage As Single)
End Class
