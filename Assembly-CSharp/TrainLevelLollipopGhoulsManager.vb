Imports System
Imports System.Collections
Imports System.Diagnostics
Imports UnityEngine

' Token: 0x02000820 RID: 2080
Public Class TrainLevelLollipopGhoulsManager
	Inherits LevelProperties.Train.Entity

	' Token: 0x14000055 RID: 85
	' (add) Token: 0x0600304A RID: 12362 RVA: 0x001C79C4 File Offset: 0x001C5DC4
	' (remove) Token: 0x0600304B RID: 12363 RVA: 0x001C79FC File Offset: 0x001C5DFC
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnDamageTakenEvent As TrainLevelLollipopGhoulsManager.OnDamageTakenHandler

	' Token: 0x14000056 RID: 86
	' (add) Token: 0x0600304C RID: 12364 RVA: 0x001C7A34 File Offset: 0x001C5E34
	' (remove) Token: 0x0600304D RID: 12365 RVA: 0x001C7A6C File Offset: 0x001C5E6C
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnDeathEvent As Action

	' Token: 0x0600304E RID: 12366 RVA: 0x001C7AA2 File Offset: 0x001C5EA2
	Public Sub Setup()
		Me.cars(1).Explode(2)
	End Sub

	' Token: 0x0600304F RID: 12367 RVA: 0x001C7AB4 File Offset: 0x001C5EB4
	Public Overrides Sub LevelInit(properties As LevelProperties.Train)
		MyBase.LevelInit(properties)
		Me.ghoulLeft.LevelInit(properties)
		Me.ghoulRight.LevelInit(properties)
		Me.cannons.LevelInit(properties)
		AddHandler Me.ghoulLeft.OnDamageTakenEvent, AddressOf Me.OnDamageTaken
		AddHandler Me.ghoulLeft.OnDeathEvent, AddressOf Me.OnDeath
		AddHandler Me.ghoulRight.OnDamageTakenEvent, AddressOf Me.OnDamageTaken
		AddHandler Me.ghoulRight.OnDeathEvent, AddressOf Me.OnDeath
	End Sub

	' Token: 0x06003050 RID: 12368 RVA: 0x001C7B48 File Offset: 0x001C5F48
	Private Sub OnDeath()
		Me.deadCount += 1
		If Me.deadCount > 1 Then
			Me.EndGhouls()
		End If
	End Sub

	' Token: 0x06003051 RID: 12369 RVA: 0x001C7B6A File Offset: 0x001C5F6A
	Private Sub OnDamageTaken(damage As Single)
		If Me.OnDamageTakenEvent IsNot Nothing Then
			Me.OnDamageTakenEvent(damage)
		End If
	End Sub

	' Token: 0x06003052 RID: 12370 RVA: 0x001C7B84 File Offset: 0x001C5F84
	Private Iterator Function start_cr() As IEnumerator
		AudioManager.Play("level_train_top_explode")
		Me.cars(0).Explode(0)
		Me.cars(2).Explode(1)
		Yield Nothing
		Me.ghoulLeft.AnimateIn()
		Me.ghoulRight.AnimateIn()
		AudioManager.Play("train_lollipop_ghoul_intro")
		Me.emitAudioFromObject.Add("train_lollipop_ghoul_intro")
		Yield CupheadTime.WaitForSeconds(Me, MyBase.properties.CurrentState.lollipopGhouls.initDelay)
		MyBase.StartCoroutine(Me.ghouls_cr())
		MyBase.StartCoroutine(Me.cannons_cr())
		Return
	End Function

	' Token: 0x06003053 RID: 12371 RVA: 0x001C7B9F File Offset: 0x001C5F9F
	Public Sub StartGhouls()
		MyBase.StartCoroutine(Me.start_cr())
	End Sub

	' Token: 0x06003054 RID: 12372 RVA: 0x001C7BAE File Offset: 0x001C5FAE
	Private Sub EndGhouls()
		Me.StopAllCoroutines()
		Me.cannons.[End]()
		If Me.OnDeathEvent IsNot Nothing Then
			Me.OnDeathEvent()
		End If
	End Sub

	' Token: 0x06003055 RID: 12373 RVA: 0x001C7BD8 File Offset: 0x001C5FD8
	Private Function NextGhoul() As TrainLevelLollipopGhoul
		If Me.deadCount > 1 Then
			Return Nothing
		End If
		If Me.ghoulRight.state = TrainLevelLollipopGhoul.State.Dead OrElse Me.ghoulRight.transform Is Nothing Then
			Return Me.ghoulLeft
		End If
		If Me.ghoulLeft.state = TrainLevelLollipopGhoul.State.Dead OrElse Me.ghoulLeft.transform Is Nothing Then
			Return Me.ghoulRight
		End If
		Me.current = CInt(Mathf.Repeat(CSng((Me.current + 1)), 2F))
		Dim num As Integer = Me.current
		If num = 0 OrElse num <> 1 Then
			Return Me.ghoulLeft
		End If
		Return Me.ghoulRight
	End Function

	' Token: 0x06003056 RID: 12374 RVA: 0x001C7C90 File Offset: 0x001C6090
	Private Iterator Function ghouls_cr() As IEnumerator
		Me.current = Global.UnityEngine.Random.Range(0, 2)
		While True
			Dim ghoul As TrainLevelLollipopGhoul = Me.NextGhoul()
			Yield Nothing
			If ghoul IsNot Nothing Then
				ghoul.Attack()
				While ghoul.state = TrainLevelLollipopGhoul.State.Attacking
					Yield Nothing
				End While
				Yield CupheadTime.WaitForSeconds(Me, MyBase.properties.CurrentState.lollipopGhouls.mainDelay)
			End If
		End While
		Return
	End Function

	' Token: 0x06003057 RID: 12375 RVA: 0x001C7CAC File Offset: 0x001C60AC
	Private Iterator Function cannons_cr() As IEnumerator
		Dim cannon As Integer = Global.UnityEngine.Random.Range(0, 3)
		Dim direction As Integer = 1
		While True
			Yield CupheadTime.WaitForSeconds(Me, MyBase.properties.CurrentState.lollipopGhouls.cannonDelay)
			Me.cannons.Shoot(cannon)
			Yield CupheadTime.WaitForSeconds(Me, 1F)
			cannon += direction
			If cannon >= 3 Then
				direction = -1
				cannon = 1
			ElseIf cannon < 0 Then
				cannon = 1
				direction = 1
			End If
		End While
		Return
	End Function

	' Token: 0x04003904 RID: 14596
	<SerializeField()>
	Private ghoulLeft As TrainLevelLollipopGhoul

	' Token: 0x04003905 RID: 14597
	<SerializeField()>
	Private ghoulRight As TrainLevelLollipopGhoul

	' Token: 0x04003906 RID: 14598
	<Space(10F)>
	<SerializeField()>
	Private cannons As TrainLevelGhostCannons

	' Token: 0x04003907 RID: 14599
	<Space(10F)>
	<SerializeField()>
	Private cars As TrainLevelPassengerCar()

	' Token: 0x0400390A RID: 14602
	Private deadCount As Integer

	' Token: 0x0400390B RID: 14603
	Private current As Integer

	' Token: 0x02000821 RID: 2081
	' (Invoke) Token: 0x06003059 RID: 12377
	Public Delegate Sub OnDamageTakenHandler(damage As Single)
End Class
