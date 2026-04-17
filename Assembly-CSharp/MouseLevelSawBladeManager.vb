Imports System
Imports System.Collections
Imports System.Linq
Imports UnityEngine

' Token: 0x020006F6 RID: 1782
Public Class MouseLevelSawBladeManager
	Inherits AbstractPausableComponent

	' Token: 0x06002630 RID: 9776 RVA: 0x00164FAD File Offset: 0x001633AD
	Public Sub Begin(properties As LevelProperties.Mouse)
		Me.properties = properties
		Me.leftSawBlades.Begin(properties)
		Me.rightSawBlades.Begin(properties)
		MyBase.StartCoroutine(Me.pattern_cr())
		MyBase.StartCoroutine(Me.fullAttack_cr())
	End Sub

	' Token: 0x06002631 RID: 9777 RVA: 0x00164FE8 File Offset: 0x001633E8
	Public Sub Leave()
		Me.StopAllCoroutines()
		Me.leftSawBlades.Leave()
		Me.rightSawBlades.Leave()
	End Sub

	' Token: 0x06002632 RID: 9778 RVA: 0x00165008 File Offset: 0x00163408
	Private Iterator Function pattern_cr() As IEnumerator
		Dim patternState As LevelProperties.Mouse.State = Me.properties.CurrentState
		If patternState.brokenCanSawBlades.patternString.Length = 0 Then
			Return
		End If
		Dim patternString As String = patternState.brokenCanSawBlades.patternString.RandomChoice()
		Me.leftSawBlades.SetPattern(patternString)
		Me.rightSawBlades.SetPattern(patternString)
		While True
			If Me.properties.CurrentState IsNot patternState Then
				If Not patternState.brokenCanSawBlades.patternString.SequenceEqual(Me.properties.CurrentState.brokenCanSawBlades.patternString) Then
					patternString = Me.properties.CurrentState.brokenCanSawBlades.patternString.RandomChoice()
					Me.leftSawBlades.SetPattern(patternString)
					Me.rightSawBlades.SetPattern(patternString)
				End If
				patternState = Me.properties.CurrentState
			End If
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06002633 RID: 9779 RVA: 0x00165024 File Offset: 0x00163424
	Private Iterator Function fullAttack_cr() As IEnumerator
		While True
			While Me.leftSawBlades.state <> MouseLevelSawBladeSide.State.Pattern OrElse Me.rightSawBlades.state <> MouseLevelSawBladeSide.State.Pattern
				Yield Nothing
			End While
			Yield CupheadTime.WaitForSeconds(Me, Me.properties.CurrentState.brokenCanSawBlades.fullAttackTime.RandomFloat())
			Dim player As AbstractPlayerController = PlayerManager.GetNext()
			If player.transform.position.x > 0F Then
				Me.rightSawBlades.FullAttack()
			Else
				Me.leftSawBlades.FullAttack()
			End If
		End While
		Return
	End Function

	' Token: 0x04002EBA RID: 11962
	<SerializeField()>
	Private leftSawBlades As MouseLevelSawBladeSide

	' Token: 0x04002EBB RID: 11963
	<SerializeField()>
	Private rightSawBlades As MouseLevelSawBladeSide

	' Token: 0x04002EBC RID: 11964
	Private properties As LevelProperties.Mouse

	' Token: 0x020006F7 RID: 1783
	Public Enum State
		' Token: 0x04002EBE RID: 11966
		Init
		' Token: 0x04002EBF RID: 11967
		Idle
		' Token: 0x04002EC0 RID: 11968
		Warning
		' Token: 0x04002EC1 RID: 11969
		Attack
	End Enum
End Class
