Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020006F8 RID: 1784
Public Class MouseLevelSawBladeSide
	Inherits AbstractPausableComponent

	' Token: 0x170003C9 RID: 969
	' (get) Token: 0x06002635 RID: 9781 RVA: 0x00165352 File Offset: 0x00163752
	' (set) Token: 0x06002636 RID: 9782 RVA: 0x0016535A File Offset: 0x0016375A
	Public Property state As MouseLevelSawBladeSide.State

	' Token: 0x06002637 RID: 9783 RVA: 0x00165364 File Offset: 0x00163764
	Public Sub Begin(properties As LevelProperties.Mouse)
		Me.properties = properties
		For Each mouseLevelSawBlade As MouseLevelSawBlade In Me.sawBlades
			mouseLevelSawBlade.Begin(properties)
		Next
		MyBase.StartCoroutine(Me.intro_cr())
	End Sub

	' Token: 0x06002638 RID: 9784 RVA: 0x001653AC File Offset: 0x001637AC
	Public Sub Leave()
		Me.StopAllCoroutines()
		For Each mouseLevelSawBlade As MouseLevelSawBlade In Me.sawBlades
			mouseLevelSawBlade.Leave()
		Next
	End Sub

	' Token: 0x06002639 RID: 9785 RVA: 0x001653E4 File Offset: 0x001637E4
	Public Sub SetPattern(pattern As String)
		Me.pattern = pattern.Split(New Char() { ","c })
		Me.patternIndex = Global.UnityEngine.Random.Range(0, Me.pattern.Length)
	End Sub

	' Token: 0x0600263A RID: 9786 RVA: 0x00165411 File Offset: 0x00163811
	Public Sub FullAttack()
		Me.StopAllCoroutines()
		MyBase.StartCoroutine(Me.fullAttack_cr())
	End Sub

	' Token: 0x0600263B RID: 9787 RVA: 0x00165428 File Offset: 0x00163828
	Private Iterator Function intro_cr() As IEnumerator
		While Me.sawBlades(0).state <> MouseLevelSawBlade.State.Idle
			Yield Nothing
		End While
		Me.state = MouseLevelSawBladeSide.State.Pattern
		MyBase.StartCoroutine(Me.pattern_cr())
		Return
	End Function

	' Token: 0x0600263C RID: 9788 RVA: 0x00165444 File Offset: 0x00163844
	Private Iterator Function pattern_cr() As IEnumerator
		If Me.pattern Is Nothing Then
			Return
		End If
		While True
			Yield CupheadTime.WaitForSeconds(Me, Me.properties.CurrentState.brokenCanSawBlades.delayBeforeNextSaw)
			Dim sawBladeIndex As Integer = 0
			Parser.IntTryParse(Me.pattern(Me.patternIndex), sawBladeIndex)
			Me.sawBlades(sawBladeIndex - 1).Attack()
			Me.patternIndex = (Me.patternIndex + 1) Mod Me.pattern.Length
		End While
		Return
	End Function

	' Token: 0x0600263D RID: 9789 RVA: 0x00165460 File Offset: 0x00163860
	Private Iterator Function fullAttack_cr() As IEnumerator
		Me.state = MouseLevelSawBladeSide.State.FullAttack
		Dim canFullAttack As Boolean = False
		While Not canFullAttack
			canFullAttack = True
			For Each mouseLevelSawBlade As MouseLevelSawBlade In Me.sawBlades
				If mouseLevelSawBlade.state <> MouseLevelSawBlade.State.Idle Then
					canFullAttack = False
				End If
			Next
			Yield Nothing
		End While
		AudioManager.Play("level_mouse_buzzsaw_wall")
		For Each mouseLevelSawBlade2 As MouseLevelSawBlade In Me.sawBlades
			mouseLevelSawBlade2.FullAttack()
		Next
		While Me.sawBlades(0).state <> MouseLevelSawBlade.State.Idle
			Yield Nothing
		End While
		Me.state = MouseLevelSawBladeSide.State.Pattern
		MyBase.StartCoroutine(Me.pattern_cr())
		Return
	End Function

	' Token: 0x04002EC2 RID: 11970
	<SerializeField()>
	Private sawBlades As MouseLevelSawBlade()

	' Token: 0x04002EC3 RID: 11971
	Private properties As LevelProperties.Mouse

	' Token: 0x04002EC4 RID: 11972
	Private pattern As String()

	' Token: 0x04002EC5 RID: 11973
	Private patternIndex As Integer

	' Token: 0x020006F9 RID: 1785
	Public Enum State
		' Token: 0x04002EC8 RID: 11976
		Init
		' Token: 0x04002EC9 RID: 11977
		Pattern
		' Token: 0x04002ECA RID: 11978
		FullAttack
	End Enum
End Class
