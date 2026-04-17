Imports System
Imports System.Diagnostics
Imports UnityEngine

' Token: 0x02000479 RID: 1145
Public MustInherit Class AbstractLevelProperties(Of STATE As AbstractLevelState(Of PATTERN, STATE_NAMES), PATTERN, STATE_NAMES)
	' Token: 0x0600119A RID: 4506 RVA: 0x000037CE File Offset: 0x00001BCE
	Public Sub New(hp As Single, goalTimes As Level.GoalTimes, states As STATE())
		Me.TotalHealth = hp
		Me.CurrentHealth = Me.TotalHealth
		Me.goalTimes = goalTimes
		Me.states = states
		Me.stateIndex = 0
	End Sub

	' Token: 0x170002BE RID: 702
	' (get) Token: 0x0600119B RID: 4507 RVA: 0x000037FE File Offset: 0x00001BFE
	' (set) Token: 0x0600119C RID: 4508 RVA: 0x00003806 File Offset: 0x00001C06
	Public Property CurrentHealth As Single

	' Token: 0x1400002A RID: 42
	' (add) Token: 0x0600119D RID: 4509 RVA: 0x00003810 File Offset: 0x00001C10
	' (remove) Token: 0x0600119E RID: 4510 RVA: 0x00003848 File Offset: 0x00001C48
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnBossDamaged As AbstractLevelProperties(Of STATE, PATTERN, STATE_NAMES).OnBossDamagedHandler

	' Token: 0x1400002B RID: 43
	' (add) Token: 0x0600119F RID: 4511 RVA: 0x00003880 File Offset: 0x00001C80
	' (remove) Token: 0x060011A0 RID: 4512 RVA: 0x000038B8 File Offset: 0x00001CB8
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnBossDeath As Action

	' Token: 0x1400002C RID: 44
	' (add) Token: 0x060011A1 RID: 4513 RVA: 0x000038F0 File Offset: 0x00001CF0
	' (remove) Token: 0x060011A2 RID: 4514 RVA: 0x00003928 File Offset: 0x00001D28
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnStateChange As Action

	' Token: 0x170002BF RID: 703
	' (get) Token: 0x060011A3 RID: 4515 RVA: 0x0000395E File Offset: 0x00001D5E
	Public ReadOnly Property CurrentState As STATE
		Get
			Me.stateIndex = Mathf.Clamp(Me.stateIndex, 0, Me.states.Length - 1)
			Return Me.states(Me.stateIndex)
		End Get
	End Property

	' Token: 0x060011A4 RID: 4516 RVA: 0x00003990 File Offset: 0x00001D90
	Public Sub DealDamage(damage As Single)
		Me.CurrentHealth -= damage
		If Me.OnBossDamaged IsNot Nothing Then
			Me.OnBossDamaged(damage)
		End If
		If Me.CurrentHealth <= 0F Then
			Me.WinInstantly()
			Return
		End If
		Dim num As Integer = 0
		For i As Integer = 0 To Me.states.Length - 1
			Dim num2 As Single = Me.CurrentHealth / Me.TotalHealth
			If num2 < Me.states(i).healthTrigger Then
				num = i
			End If
		Next
		If Me.stateIndex <> num Then
			Me.stateIndex = num
			If Me.OnStateChange IsNot Nothing Then
				Me.OnStateChange()
			End If
		End If
	End Sub

	' Token: 0x060011A5 RID: 4517 RVA: 0x00003A4C File Offset: 0x00001E4C
	Public Sub DealDamageToNextNamedState()
		Dim stateName As STATE_NAMES = Me.CurrentState.stateName
		Dim text As String = stateName.ToString()
		Dim num As Integer = 0
		While CSng(num) < Me.TotalHealth
			Me.DealDamage(1F)
			Dim stateName2 As STATE_NAMES = Me.CurrentState.stateName
			If stateName2.ToString() <> "Generic" Then
				Dim text2 As String = text
				Dim stateName3 As STATE_NAMES = Me.CurrentState.stateName
				If text2 <> stateName3.ToString() Then
					Return
				End If
			End If
			num += 1
		End While
	End Sub

	' Token: 0x060011A6 RID: 4518 RVA: 0x00003AF3 File Offset: 0x00001EF3
	Public Function GetNextStateHealthTrigger() As Single
		If Me.stateIndex < Me.states.Length - 1 Then
			Return Me.states(Me.stateIndex + 1).healthTrigger
		End If
		Return 0F
	End Function

	' Token: 0x060011A7 RID: 4519 RVA: 0x00003B2D File Offset: 0x00001F2D
	Public Sub WinInstantly()
		If Me.OnBossDeath IsNot Nothing Then
			Me.OnBossDeath()
		End If
		Me.OnBossDeath = Nothing
	End Sub

	' Token: 0x04001B28 RID: 6952
	Public TotalHealth As Single

	' Token: 0x04001B2A RID: 6954
	Public goalTimes As Level.GoalTimes

	' Token: 0x04001B2B RID: 6955
	Private states As STATE()

	' Token: 0x04001B2C RID: 6956
	Private stateIndex As Integer

	' Token: 0x0200047A RID: 1146
	' (Invoke) Token: 0x060011A9 RID: 4521
	Public Delegate Sub OnBossDamagedHandler(damage As Single)
End Class
