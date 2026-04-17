Imports System
Imports System.Collections.Generic
Imports UnityEngine

' Token: 0x0200092A RID: 2346
Public MustInherit Class AbstractMapLevelDependentEntity
	Inherits AbstractMonoBehaviour

	' Token: 0x1700047E RID: 1150
	' (get) Token: 0x060036DE RID: 14046 RVA: 0x001FA587 File Offset: 0x001F8987
	' (set) Token: 0x060036DF RID: 14047 RVA: 0x001FA58E File Offset: 0x001F898E
	Public Shared Property RegisteredEntities As List(Of AbstractMapLevelDependentEntity)

	' Token: 0x1700047F RID: 1151
	' (get) Token: 0x060036E0 RID: 14048 RVA: 0x001FA596 File Offset: 0x001F8996
	Protected Overridable ReadOnly Property ReactToGradeChange As Boolean
		Get
			Return False
		End Get
	End Property

	' Token: 0x17000480 RID: 1152
	' (get) Token: 0x060036E1 RID: 14049 RVA: 0x001FA599 File Offset: 0x001F8999
	Protected Overridable ReadOnly Property ReactToDifficultyChange As Boolean
		Get
			Return False
		End Get
	End Property

	' Token: 0x17000481 RID: 1153
	' (get) Token: 0x060036E2 RID: 14050 RVA: 0x001FA59C File Offset: 0x001F899C
	Public ReadOnly Property CameraPosition As Vector2
		Get
			Return MyBase.baseTransform.position + Me._cameraPosition
		End Get
	End Property

	' Token: 0x17000482 RID: 1154
	' (get) Token: 0x060036E3 RID: 14051 RVA: 0x001FA5B9 File Offset: 0x001F89B9
	' (set) Token: 0x060036E4 RID: 14052 RVA: 0x001FA5C1 File Offset: 0x001F89C1
	Public Property CurrentState As AbstractMapLevelDependentEntity.State

	' Token: 0x060036E5 RID: 14053
	Public MustOverride Sub OnConditionNotMet()

	' Token: 0x060036E6 RID: 14054
	Public MustOverride Sub OnConditionMet()

	' Token: 0x060036E7 RID: 14055
	Public MustOverride Sub OnConditionAlreadyMet()

	' Token: 0x060036E8 RID: 14056
	Public MustOverride Sub DoTransition()

	' Token: 0x060036E9 RID: 14057 RVA: 0x001FA5CA File Offset: 0x001F89CA
	Protected Overrides Sub Awake()
		MyBase.Awake()
		If AbstractMapLevelDependentEntity.RegisteredEntities Is Nothing Then
			AbstractMapLevelDependentEntity.RegisteredEntities = New List(Of AbstractMapLevelDependentEntity)()
		End If
	End Sub

	' Token: 0x060036EA RID: 14058 RVA: 0x001FA5E6 File Offset: 0x001F89E6
	Protected Overridable Sub Start()
		Me.Check()
	End Sub

	' Token: 0x060036EB RID: 14059 RVA: 0x001FA5EE File Offset: 0x001F89EE
	Private Sub OnDestroy()
		AbstractMapLevelDependentEntity.RegisteredEntities = Nothing
	End Sub

	' Token: 0x060036EC RID: 14060 RVA: 0x001FA5F8 File Offset: 0x001F89F8
	Private Sub Check()
		Dim flag As Boolean = Me.ValidateSucess()
		If flag Then
			Dim flag2 As Boolean = False
			For Each levels2 As Levels In Me._levels
				If Me.anyLevelPassesCheck Then
					If(Level.PreviousLevel <> levels2 OrElse Level.PreviouslyWon) AndAlso PlayerData.Data.GetLevelData(levels2).completed Then
						flag2 = False
						Exit For
					End If
					If Level.PreviousLevel = levels2 AndAlso Level.Won AndAlso Not Level.PreviouslyWon Then
						flag2 = True
					End If
				Else
					flag2 = Me.ValidateCondition(levels2)
				End If
			Next
			If flag2 Then
				Me.CallOnConditionMet()
			Else
				Me.CallOnConditionAlreadyMet()
			End If
			Return
		End If
		Me.CallOnConditionNotMet()
	End Sub

	' Token: 0x060036ED RID: 14061 RVA: 0x001FA6C0 File Offset: 0x001F8AC0
	Protected Overridable Function ValidateCondition(level As Levels) As Boolean
		If Not Level.Won Then
			Return False
		End If
		If Level.PreviousLevel <> level Then
			Return False
		End If
		Dim flag As Boolean = False
		If Not Level.PreviouslyWon AndAlso Level.Won Then
			Me.firstTimeWon = True
			flag = True
		End If
		If Me.ReactToGradeChange AndAlso Level.Grade > Level.PreviousGrade Then
			Me.gradeChanged = True
			flag = True
		End If
		If Me.ReactToDifficultyChange AndAlso Level.Difficulty > Level.PreviousDifficulty Then
			Me.difficultyChanged = True
			flag = True
		End If
		Return flag
	End Function

	' Token: 0x060036EE RID: 14062 RVA: 0x001FA74C File Offset: 0x001F8B4C
	Protected Overridable Function ValidateSucess() As Boolean
		Dim flag As Boolean = True
		For Each levels2 As Levels In Me._levels
			Dim levelData As PlayerData.PlayerLevelDataObject = PlayerData.Data.GetLevelData(levels2)
			If Not levelData.completed Then
				flag = False
				If Not Me.anyLevelPassesCheck Then
					Exit For
				End If
			Else
				Me.difficulty = levelData.difficultyBeaten
				Me.grade = levelData.grade
				If Me.anyLevelPassesCheck Then
					flag = True
					Exit For
				End If
			End If
		Next
		Return flag
	End Function

	' Token: 0x060036EF RID: 14063 RVA: 0x001FA7D7 File Offset: 0x001F8BD7
	Private Sub CallOnConditionNotMet()
		Me.CurrentState = AbstractMapLevelDependentEntity.State.Incomplete
		Me.OnConditionNotMet()
	End Sub

	' Token: 0x060036F0 RID: 14064 RVA: 0x001FA7E6 File Offset: 0x001F8BE6
	Private Sub CallOnConditionAlreadyMet()
		Me.CurrentState = AbstractMapLevelDependentEntity.State.Complete
		Me.OnConditionAlreadyMet()
	End Sub

	' Token: 0x060036F1 RID: 14065 RVA: 0x001FA7F5 File Offset: 0x001F8BF5
	Private Sub CallOnConditionMet()
		Me.CurrentState = AbstractMapLevelDependentEntity.State.Incomplete
		Me.OnConditionMet()
		AbstractMapLevelDependentEntity.RegisteredEntities.Add(Me)
	End Sub

	' Token: 0x060036F2 RID: 14066 RVA: 0x001FA80F File Offset: 0x001F8C0F
	Public Sub MapMeetCondition()
		Me.DoTransition()
	End Sub

	' Token: 0x060036F3 RID: 14067 RVA: 0x001FA817 File Offset: 0x001F8C17
	Private Sub OnValidate()
		If Me._levels Is Nothing Then
			Me._levels = New Levels(0) {}
		End If
		If Me._levels.Length < 1 Then
			Array.Resize(Of Levels)(Me._levels, 1)
		End If
	End Sub

	' Token: 0x060036F4 RID: 14068 RVA: 0x001FA84C File Offset: 0x001F8C4C
	Protected Overrides Sub OnDrawGizmosSelected()
		MyBase.OnDrawGizmosSelected()
		Dim vector As Vector2 = MyBase.baseTransform.position + Me._cameraPosition
		Gizmos.color = Color.black
		Gizmos.DrawWireSphere(vector, 0.19F)
		Gizmos.color = Color.red
		Gizmos.DrawWireSphere(vector, 0.2F)
	End Sub

	' Token: 0x04003F1B RID: 16155
	<SerializeField()>
	Protected anyLevelPassesCheck As Boolean

	' Token: 0x04003F1C RID: 16156
	<SerializeField()>
	Protected _levels As Levels()

	' Token: 0x04003F1D RID: 16157
	<SerializeField()>
	Private _cameraPosition As Vector2 = Vector2.zero

	' Token: 0x04003F1E RID: 16158
	Public panCamera As Boolean

	' Token: 0x04003F20 RID: 16160
	Protected firstTimeWon As Boolean

	' Token: 0x04003F21 RID: 16161
	Protected gradeChanged As Boolean

	' Token: 0x04003F22 RID: 16162
	Protected difficultyChanged As Boolean

	' Token: 0x04003F23 RID: 16163
	Protected difficulty As Level.Mode

	' Token: 0x04003F24 RID: 16164
	Protected grade As LevelScoringData.Grade

	' Token: 0x0200092B RID: 2347
	Public Enum State
		' Token: 0x04003F26 RID: 16166
		Incomplete
		' Token: 0x04003F27 RID: 16167
		Transitioning
		' Token: 0x04003F28 RID: 16168
		Complete
	End Enum
End Class
