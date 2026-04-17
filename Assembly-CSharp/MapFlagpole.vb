Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000937 RID: 2359
Public Class MapFlagpole
	Inherits AbstractMapLevelDependentEntity

	' Token: 0x17000483 RID: 1155
	' (get) Token: 0x06003724 RID: 14116 RVA: 0x001FC331 File Offset: 0x001FA731
	Protected Overrides ReadOnly Property ReactToDifficultyChange As Boolean
		Get
			Return True
		End Get
	End Property

	' Token: 0x17000484 RID: 1156
	' (get) Token: 0x06003725 RID: 14117 RVA: 0x001FC334 File Offset: 0x001FA734
	Protected Overrides ReadOnly Property ReactToGradeChange As Boolean
		Get
			Return True
		End Get
	End Property

	' Token: 0x06003726 RID: 14118 RVA: 0x001FC337 File Offset: 0x001FA737
	Public Overrides Sub OnConditionMet()
		If Level.PreviouslyWon OrElse Me.forceNoAppearAnimation Then
			Me.Init(Me.difficulty, Me.grade)
		End If
	End Sub

	' Token: 0x06003727 RID: 14119 RVA: 0x001FC360 File Offset: 0x001FA760
	Public Overrides Sub DoTransition()
		If Level.PreviouslyWon Then
			MyBase.StartCoroutine(Me.shake_cr())
		Else
			MyBase.StartCoroutine(Me.raise_cr())
		End If
	End Sub

	' Token: 0x06003728 RID: 14120 RVA: 0x001FC38B File Offset: 0x001FA78B
	Public Overrides Sub OnConditionAlreadyMet()
		Me.Init(Me.difficulty, Me.grade)
	End Sub

	' Token: 0x06003729 RID: 14121 RVA: 0x001FC39F File Offset: 0x001FA79F
	Public Overrides Sub OnConditionNotMet()
		MyBase.gameObject.SetActive(False)
	End Sub

	' Token: 0x0600372A RID: 14122 RVA: 0x001FC3B0 File Offset: 0x001FA7B0
	Private Sub Init(difficulty As Level.Mode, grade As LevelScoringData.Grade)
		If Me._levels.Length = 0 Then
			Return
		End If
		Dim text As String = String.Empty
		Dim flag As Boolean = False
		For i As Integer = 0 To Level.platformingLevels.Length - 1
			If Level.platformingLevels(i) = Me._levels(0) Then
				flag = True
				Exit For
			End If
		Next
		If flag Then
			If grade < LevelScoringData.Grade.AMinus Then
				text = "IdleBelowA"
			ElseIf grade < LevelScoringData.Grade.P Then
				text = "IdleBelowP"
			Else
				text = "IdleP"
			End If
		ElseIf difficulty = Level.Mode.Easy Then
			text = "IdleEasy"
		ElseIf difficulty = Level.Mode.Normal AndAlso grade < LevelScoringData.Grade.AMinus Then
			text = "IdleNormalBelowA"
		ElseIf difficulty = Level.Mode.Normal AndAlso grade >= LevelScoringData.Grade.AMinus Then
			text = "IdleNormalA"
		ElseIf difficulty = Level.Mode.Hard AndAlso grade < LevelScoringData.Grade.S Then
			text = "IdleExpert"
		ElseIf difficulty = Level.Mode.Hard AndAlso grade >= LevelScoringData.Grade.S Then
			text = "IdleExpertS"
		End If
		MyBase.animator.Play(text)
		If Me.forceNoAppearAnimation Then
			MyBase.CurrentState = AbstractMapLevelDependentEntity.State.Complete
		End If
	End Sub

	' Token: 0x0600372B RID: 14123 RVA: 0x001FC4D0 File Offset: 0x001FA8D0
	Private Iterator Function raise_cr() As IEnumerator
		If Me._levels.Length = 0 Then
			Return
		End If
		Dim trigger As String = String.Empty
		Dim platformingLevel As Boolean = False
		For i As Integer = 0 To Level.platformingLevels.Length - 1
			If Level.platformingLevels(i) = Me._levels(0) Then
				platformingLevel = True
				Exit For
			End If
		Next
		If platformingLevel Then
			If Me.grade < LevelScoringData.Grade.AMinus Then
				trigger = "RaiseBelowA"
			ElseIf Me.grade < LevelScoringData.Grade.P Then
				trigger = "RaiseBelowP"
			Else
				trigger = "RaiseP"
			End If
		ElseIf Me.difficulty = Level.Mode.Easy Then
			trigger = "RaiseEasy"
		ElseIf Me.difficulty = Level.Mode.Normal AndAlso Me.grade < LevelScoringData.Grade.AMinus Then
			trigger = "RaiseNormalBelowA"
		ElseIf Me.difficulty = Level.Mode.Normal AndAlso Me.grade >= LevelScoringData.Grade.AMinus Then
			trigger = "RaiseNormalA"
		ElseIf Me.difficulty = Level.Mode.Hard AndAlso Me.grade < LevelScoringData.Grade.S Then
			trigger = "RaiseExpert"
		ElseIf Me.difficulty = Level.Mode.Hard AndAlso Me.grade >= LevelScoringData.Grade.S Then
			trigger = "RaiseExpertS"
		End If
		MyBase.animator.SetTrigger(trigger)
		If PlayerManager.playerWasChalice(0) Then
			AudioManager.Play("worldmap_level_raise_flag_chalice")
		ElseIf PlayerManager.player1IsMugman Then
			AudioManager.Play("worldmap_level_raise_flag_mugman")
		Else
			AudioManager.Play("world_map_flag_raise")
		End If
		Yield MyBase.animator.WaitForAnimationToEnd(Me, trigger, False, True)
		MyBase.CurrentState = AbstractMapLevelDependentEntity.State.Complete
		Return
	End Function

	' Token: 0x0600372C RID: 14124 RVA: 0x001FC4EC File Offset: 0x001FA8EC
	Private Iterator Function shake_cr() As IEnumerator
		MyBase.CurrentState = AbstractMapLevelDependentEntity.State.Complete
		Yield Nothing
		Return
	End Function

	' Token: 0x04003F51 RID: 16209
	<SerializeField()>
	Private forceNoAppearAnimation As Boolean
End Class
