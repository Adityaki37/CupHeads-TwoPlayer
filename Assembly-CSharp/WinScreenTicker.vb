Imports System
Imports System.Collections
Imports UnityEngine
Imports UnityEngine.UI

' Token: 0x02000B35 RID: 2869
Public Class WinScreenTicker
	Inherits AbstractMonoBehaviour

	' Token: 0x17000635 RID: 1589
	' (get) Token: 0x0600458D RID: 17805 RVA: 0x0024AA8C File Offset: 0x00248E8C
	' (set) Token: 0x0600458E RID: 17806 RVA: 0x0024AA94 File Offset: 0x00248E94
	Public Property TargetValue As Integer

	' Token: 0x17000636 RID: 1590
	' (get) Token: 0x0600458F RID: 17807 RVA: 0x0024AA9D File Offset: 0x00248E9D
	' (set) Token: 0x06004590 RID: 17808 RVA: 0x0024AAA5 File Offset: 0x00248EA5
	Public Property MaxValue As Integer

	' Token: 0x17000637 RID: 1591
	' (get) Token: 0x06004591 RID: 17809 RVA: 0x0024AAAE File Offset: 0x00248EAE
	' (set) Token: 0x06004592 RID: 17810 RVA: 0x0024AAB6 File Offset: 0x00248EB6
	Public Property FinishedCounting As Boolean

	' Token: 0x06004593 RID: 17811 RVA: 0x0024AABF File Offset: 0x00248EBF
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.input = New CupheadInput.AnyPlayerInput(False)
	End Sub

	' Token: 0x06004594 RID: 17812 RVA: 0x0024AAD3 File Offset: 0x00248ED3
	Private Sub Start()
		MyBase.StartCoroutine(Me.select_type_cr())
	End Sub

	' Token: 0x06004595 RID: 17813 RVA: 0x0024AAE4 File Offset: 0x00248EE4
	Private Iterator Function select_type_cr() As IEnumerator
		Select Case Me.tickerType
			Case WinScreenTicker.TickerType.Time
				MyBase.StartCoroutine(Me.time_tally_up_cr())
			Case WinScreenTicker.TickerType.Health
				MyBase.StartCoroutine(Me.health_tally_up_cr())
			Case WinScreenTicker.TickerType.Score
				MyBase.StartCoroutine(Me.score_tally_up_cr())
			Case WinScreenTicker.TickerType.Stars
				MyBase.StartCoroutine(Me.stars_tally_up_cr())
		End Select
		Yield Nothing
		Return
	End Function

	' Token: 0x06004596 RID: 17814 RVA: 0x0024AB00 File Offset: 0x00248F00
	Private Iterator Function health_tally_up_cr() As IEnumerator
		Dim isTallying As Boolean = True
		Dim t As Single = 0F
		Dim counter As Integer = 0
		Me.valueText.text = counter + " " + Me.MaxValue
		While Not Me.startedCounting
			Yield Nothing
		End While
		While counter < Me.TargetValue AndAlso isTallying
			If counter >= Me.TargetValue Then
				Exit While
			End If
			While t < 0.03F
				If Me.input.GetButtonDown(CupheadButton.Jump) Then
					isTallying = False
					Exit While
				End If
				t += CupheadTime.Delta
				Yield Nothing
			End While
			t = 0F
			If isTallying Then
				AudioManager.Play("win_score_tick")
				counter += 1
				Me.valueText.text = counter + " " + Me.MaxValue
			End If
		End While
		Me.valueText.text = Me.TargetValue + " " + Me.MaxValue
		Me.valueText.GetComponent(Of Animator)().SetTrigger("MakeBigTally")
		If Me.TargetValue = Me.MaxValue Then
			AudioManager.Play("win_score_tick")
			Me.valueText.color = ColorUtils.HexToColor("FCC93D")
		End If
		Yield Nothing
		Me.FinishedCounting = True
		Return
	End Function

	' Token: 0x06004597 RID: 17815 RVA: 0x0024AB1C File Offset: 0x00248F1C
	Private Iterator Function score_tally_up_cr() As IEnumerator
		Dim isTallying As Boolean = True
		Dim t As Single = 0F
		Dim counter As Integer = 0
		Me.valueText.text = counter + " " + Me.MaxValue
		If Me.leaderDots.Length > 0 Then
			Me.leaderDots(0).enabled = True
			Me.leaderDots(1).enabled = False
		End If
		While Not Me.startedCounting
			Yield Nothing
		End While
		While counter <= Me.TargetValue AndAlso isTallying
			If counter >= Me.TargetValue Then
				Exit While
			End If
			While t < 0.03F
				If Me.input.GetButtonDown(CupheadButton.Jump) Then
					isTallying = False
					Exit While
				End If
				t += CupheadTime.Delta
				Yield Nothing
			End While
			t = 0F
			If isTallying Then
				AudioManager.Play("win_score_tick")
				counter += 1
				If Me.leaderDots.Length > 0 AndAlso counter > 9 Then
					Me.leaderDots(0).enabled = False
					Me.leaderDots(1).enabled = True
				End If
				Me.valueText.text = counter + " " + Me.MaxValue
			End If
			Yield Nothing
		End While
		Me.valueText.text = Me.TargetValue + " " + Me.MaxValue
		Me.valueText.GetComponent(Of Animator)().SetTrigger("MakeBigTally")
		If Me.TargetValue = Me.MaxValue Then
			AudioManager.Play("win_score_tick")
			Me.valueText.color = ColorUtils.HexToColor("FCC93D")
		End If
		Yield Nothing
		Me.FinishedCounting = True
		Return
	End Function

	' Token: 0x06004598 RID: 17816 RVA: 0x0024AB38 File Offset: 0x00248F38
	Private Iterator Function time_tally_up_cr() As IEnumerator
		Dim isTallying As Boolean = True
		Dim t As Single = 0F
		Dim minutesMax As Integer = Me.MaxValue / 60
		Dim secondsMax As Integer = Me.MaxValue Mod 60
		Dim minutesTarget As Integer = Me.TargetValue / 60
		Dim secondsTarget As Integer = Me.TargetValue Mod 60
		Dim secondCounter As Integer = 0
		Dim minuteCounter As Integer = 0
		Me.valueText.text = "00 00"
		While Not Me.startedCounting
			Yield Nothing
		End While
		AudioManager.PlayLoop("win_time_ticker_loop")
		While isTallying
			If secondCounter < 60 Then
				secondCounter += 1
			Else
				minuteCounter += 1
				secondCounter = 0
			End If
			Dim displayedMinutes As String = If((minuteCounter > 9), minuteCounter.ToString(), ("0" + minuteCounter.ToString()))
			Dim displayedSeconds As String = If((secondCounter > 9), secondCounter.ToString(), ("0" + secondCounter.ToString()))
			Me.valueText.text = displayedMinutes + " " + displayedSeconds
			If minuteCounter >= minutesTarget AndAlso secondCounter >= secondsTarget Then
				isTallying = False
				Exit While
			End If
			While t < 0.03F
				If Me.input.GetButtonDown(CupheadButton.Jump) Then
					isTallying = False
					Exit While
				End If
				t += CupheadTime.Delta
				Yield Nothing
			End While
			t = 0F
		End While
		AudioManager.[Stop]("win_time_ticker_loop")
		AudioManager.Play("win_time_ticker_loop_end")
		Dim minutes As String = If((minutesTarget > 9), minutesTarget.ToString(), ("0" + minutesTarget.ToString()))
		Dim seconds As String = If((secondsTarget > 9), secondsTarget.ToString(), ("0" + secondsTarget.ToString()))
		Me.valueText.text = minutes + " " + seconds
		If minutesTarget = minutesMax Then
			If secondsTarget <= secondsMax Then
				AudioManager.Play("win_score_tick")
				Me.valueText.color = ColorUtils.HexToColor("FCC93D")
			End If
		ElseIf minutesTarget < minutesMax Then
			AudioManager.Play("win_score_tick")
			Me.valueText.color = ColorUtils.HexToColor("FCC93D")
		End If
		Me.valueText.GetComponent(Of Animator)().SetTrigger("MakeBigTally")
		Me.FinishedCounting = True
		Yield Nothing
		Return
	End Function

	' Token: 0x06004599 RID: 17817 RVA: 0x0024AB54 File Offset: 0x00248F54
	Private Iterator Function stars_tally_up_cr() As IEnumerator
		Dim startVal As Integer = 0
		If Me.TargetValue = 2 Then
			Me.leaderDots(0).enabled = False
			Me.leaderDots(1).enabled = True
			Me.stars(0).gameObject.SetActive(True)
		Else
			Me.leaderDots(0).enabled = True
			Me.leaderDots(1).enabled = False
			Me.stars(0).gameObject.SetActive(False)
			startVal = 1
		End If
		Dim time As YieldInstruction = New WaitForSeconds(0.5F)
		While Not Me.startedCounting
			Yield Nothing
		End While
		For i As Integer = startVal To Me.TargetValue + 1 + startVal - 1
			Me.stars(i).SetTrigger("OnAppear")
			AudioManager.Play("win_skill_lvl")
			If Not Me.input.GetButtonDown(CupheadButton.Accept) Then
				Yield time
			End If
		Next
		Me.FinishedCounting = True
		Yield Nothing
		Return
	End Function

	' Token: 0x0600459A RID: 17818 RVA: 0x0024AB6F File Offset: 0x00248F6F
	Public Sub StartCounting()
		Me.startedCounting = True
	End Sub

	' Token: 0x04004BB9 RID: 19385
	Public tickerType As WinScreenTicker.TickerType

	' Token: 0x04004BBA RID: 19386
	<SerializeField()>
	Private stars As Animator()

	' Token: 0x04004BBB RID: 19387
	<SerializeField()>
	Private leaderDots As Text()

	' Token: 0x04004BBC RID: 19388
	<SerializeField()>
	Private label As Text

	' Token: 0x04004BBD RID: 19389
	<SerializeField()>
	Private valueText As Text

	' Token: 0x04004BC0 RID: 19392
	Private startedCounting As Boolean

	' Token: 0x04004BC2 RID: 19394
	Private Const TIME_COUNTER_TIME As Single = 0.03F

	' Token: 0x04004BC3 RID: 19395
	Private Const USUAL_COUNTER_TIME As Single = 0.07F

	' Token: 0x04004BC4 RID: 19396
	Private Const STAR_COUNTER_TIME As Single = 0.5F

	' Token: 0x04004BC5 RID: 19397
	Private input As CupheadInput.AnyPlayerInput

	' Token: 0x02000B36 RID: 2870
	Public Enum TickerType
		' Token: 0x04004BC7 RID: 19399
		Time
		' Token: 0x04004BC8 RID: 19400
		Health
		' Token: 0x04004BC9 RID: 19401
		Score
		' Token: 0x04004BCA RID: 19402
		Stars
	End Enum
End Class
