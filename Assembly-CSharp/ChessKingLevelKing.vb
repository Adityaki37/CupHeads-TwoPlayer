Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports UnityEngine

' Token: 0x0200053C RID: 1340
Public Class ChessKingLevelKing
	Inherits LevelProperties.ChessKing.Entity

	' Token: 0x17000334 RID: 820
	' (get) Token: 0x0600185C RID: 6236 RVA: 0x000DC856 File Offset: 0x000DAC56
	' (set) Token: 0x0600185D RID: 6237 RVA: 0x000DC85E File Offset: 0x000DAC5E
	Public Property GOT_PARRIED As Boolean

	' Token: 0x0600185E RID: 6238 RVA: 0x000DC868 File Offset: 0x000DAC68
	Public Sub StartGame()
		Me.rats = New List(Of ChessKingLevelRat)()
		MyBase.StartCoroutine(Me.timer_cr())
		Dim king As LevelProperties.ChessKing.King = MyBase.properties.CurrentState.king
		Me.trialPoolMainIndex = Global.UnityEngine.Random.Range(0, MyBase.properties.CurrentState.king.trialPool.Length)
		Me.kingAttackStringMainIndex = Global.UnityEngine.Random.Range(0, king.kingAttackString.Length)
		Dim array As String() = king.kingAttackString(Me.kingAttackStringMainIndex).Split(New Char() { ","c })
		Me.kingAttackStringIndex = Global.UnityEngine.Random.Range(0, array.Length)
		MyBase.StartCoroutine(Me.create_trial_cr())
	End Sub

	' Token: 0x0600185F RID: 6239 RVA: 0x000DC90F File Offset: 0x000DAD0F
	Private Sub Update()
	End Sub

	' Token: 0x06001860 RID: 6240 RVA: 0x000DC911 File Offset: 0x000DAD11
	Public Overrides Sub LevelInit(properties As LevelProperties.ChessKing)
		MyBase.LevelInit(properties)
	End Sub

	' Token: 0x06001861 RID: 6241 RVA: 0x000DC91C File Offset: 0x000DAD1C
	Public Sub StateChange()
		Dim king As LevelProperties.ChessKing.King = MyBase.properties.CurrentState.king
		Me.trialPoolMainIndex = Global.UnityEngine.Random.Range(0, MyBase.properties.CurrentState.king.trialPool.Length)
		Me.kingAttackStringMainIndex = Global.UnityEngine.Random.Range(0, king.kingAttackString.Length)
		Dim array As String() = king.kingAttackString(Me.kingAttackStringMainIndex).Split(New Char() { ","c })
		Me.kingAttackStringIndex = Global.UnityEngine.Random.Range(0, array.Length)
	End Sub

	' Token: 0x06001862 RID: 6242 RVA: 0x000DC99E File Offset: 0x000DAD9E
	Public Overrides Sub OnParry(player As AbstractPlayerController)
		MyBase.OnParry(player)
		Me.GOT_PARRIED = True
		MyBase.properties.DealDamage(If((Not PlayerManager.BothPlayersActive()), 10F, ChessKingLevelKing.multiplayerDamageNerf))
	End Sub

	' Token: 0x06001863 RID: 6243 RVA: 0x000DC9D2 File Offset: 0x000DADD2
	Public Sub BecomeParryable()
		Me.GOT_PARRIED = False
		MyBase.animator.Play("Parryable")
	End Sub

	' Token: 0x06001864 RID: 6244 RVA: 0x000DC9EC File Offset: 0x000DADEC
	Private Iterator Function create_trial_cr() As IEnumerator
		Me.parryPoints = New List(Of ChessKingLevelParryPoint)()
		Dim p As LevelProperties.ChessKing.King = MyBase.properties.CurrentState.king
		Dim trial As String() = p.trialPool(Me.trialPoolMainIndex).Split(New Char() { ","c })
		Me.GOT_PARRIED = False
		For j As Integer = 0 To trial.Length - 1
			Dim array As String() = trial(j).Split(New Char() { ":"c })
			Dim vector As Vector3 = Vector3.zero
			Dim num As Single = 0F
			Dim num2 As Single = 0F
			Dim num3 As Single = 0F
			Dim flag As Boolean = False
			For k As Integer = 0 To array.Length - 1
				Select Case k
					Case 0
						Parser.FloatTryParse(array(k), num)
					Case 1
						Parser.FloatTryParse(array(k), num2)
					Case 2
						flag = True
						vector = Me.GetDir(array(k))
					Case 3
						Parser.FloatTryParse(array(k), num3)
				End Select
			Next
			Dim chessKingLevelParryPoint As ChessKingLevelParryPoint = Global.UnityEngine.[Object].Instantiate(Of ChessKingLevelParryPoint)(Me.parryPoint)
			Dim vector2 As Vector3 = New Vector3(CSng(Level.Current.Left), CSng(Level.Current.Ground)) + New Vector3(num, num2)
			If flag Then
				chessKingLevelParryPoint.Init(vector2, vector, p.bluePointSpeed, num3)
			Else
				chessKingLevelParryPoint.Init(vector2)
			End If
			Me.parryPoints.Add(chessKingLevelParryPoint)
		Next
		For i As Integer = 0 To Me.parryPoints.Count - 1
			Me.parryPoints(i).Activate()
			While Not Me.parryPoints(i).GOT_PARRIED
				If Me.groundTrigger.PLAYER_FALLEN Then
					Exit While
				End If
				Yield Nothing
			End While
			If Not Me.challengeActivated Then
				Me.groundTrigger.CheckPlayer(True)
				Me.MoveBluePoints()
				Me.challengeActivated = True
			End If
		Next
		Me.EndChallenge()
		Yield Nothing
		Return
	End Function

	' Token: 0x06001865 RID: 6245 RVA: 0x000DCA07 File Offset: 0x000DAE07
	Private Sub EndChallenge()
		MyBase.StartCoroutine(Me.end_challenge())
	End Sub

	' Token: 0x06001866 RID: 6246 RVA: 0x000DCA18 File Offset: 0x000DAE18
	Private Iterator Function end_challenge() As IEnumerator
		If Not Me.groundTrigger.PLAYER_FALLEN Then
			Me.BecomeParryable()
			While Not Me.GOT_PARRIED
				If Me.groundTrigger.PLAYER_FALLEN Then
					Exit While
				End If
				Yield Nothing
			End While
		End If
		Me.challengeActivated = False
		MyBase.animator.Play("Idle")
		Me.groundTrigger.CheckPlayer(False)
		Me.ClearPoints()
		If Not Me.GOT_PARRIED Then
			Me.Attack()
		End If
		While Me.isAttacking
			Yield Nothing
		End While
		Dim player As LevelPlayerController = TryCast(PlayerManager.GetPlayer(PlayerId.PlayerOne), LevelPlayerController)
		While Not player.motor.Grounded
			Yield Nothing
		End While
		Me.trialPoolMainIndex = (Me.trialPoolMainIndex + 1) Mod MyBase.properties.CurrentState.king.trialPool.Length
		MyBase.StartCoroutine(Me.create_trial_cr())
		Return
	End Function

	' Token: 0x06001867 RID: 6247 RVA: 0x000DCA34 File Offset: 0x000DAE34
	Private Sub ClearPoints()
		For i As Integer = 0 To Me.parryPoints.Count - 1
			Global.UnityEngine.[Object].Destroy(Me.parryPoints(i).gameObject)
		Next
		Me.parryPoints.Clear()
	End Sub

	' Token: 0x06001868 RID: 6248 RVA: 0x000DCA80 File Offset: 0x000DAE80
	Private Sub MoveBluePoints()
		For i As Integer = 0 To Me.parryPoints.Count - 1
			Me.parryPoints(i).MovePoint()
		Next
	End Sub

	' Token: 0x06001869 RID: 6249 RVA: 0x000DCABC File Offset: 0x000DAEBC
	Private Iterator Function timer_cr() As IEnumerator
		Dim t As Single = 0F
		Dim time As Single = MyBase.properties.CurrentState.king.kingAttackTimer
		While True
			If Not Me.challengeActivated AndAlso Not Me.isAttacking Then
				If t < time Then
					t += CupheadTime.Delta
				Else
					Me.Attack()
					t = 0F
				End If
			Else
				t = 0F
			End If
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x0600186A RID: 6250 RVA: 0x000DCAD8 File Offset: 0x000DAED8
	Private Function GetDir(part As String) As Vector3
		If part(0) = "R"c Then
			Return Vector3.right
		End If
		If part(0) = "L"c Then
			Return Vector3.left
		End If
		If part(0) = "U"c Then
			Return Vector3.up
		End If
		Return Vector3.down
	End Function

	' Token: 0x0600186B RID: 6251 RVA: 0x000DCB28 File Offset: 0x000DAF28
	Private Sub Attack()
		Me.isAttacking = True
		Dim king As LevelProperties.ChessKing.King = MyBase.properties.CurrentState.king
		Dim array As String() = king.kingAttackString(Me.kingAttackStringMainIndex).Split(New Char() { ","c })
		Dim text As String = array(Me.kingAttackStringIndex)
		If text IsNot Nothing Then
			If Not(text = "F") Then
				If Not(text = "B") Then
					If text = "R" Then
						MyBase.StartCoroutine(Me.rat_attack_cr())
					End If
				Else
					MyBase.StartCoroutine(Me.beam_attack_cr())
				End If
			Else
				MyBase.StartCoroutine(Me.full_screen_attack_cr())
			End If
		End If
		If Me.kingAttackStringIndex < array.Length - 1 Then
			Me.kingAttackStringIndex += 1
		Else
			Me.kingAttackStringMainIndex = (Me.kingAttackStringMainIndex + 1) Mod king.kingAttackString.Length
			Me.kingAttackStringIndex = 0
		End If
	End Sub

	' Token: 0x0600186C RID: 6252 RVA: 0x000DCC28 File Offset: 0x000DB028
	Private Iterator Function full_screen_attack_cr() As IEnumerator
		Dim p As LevelProperties.ChessKing.Full = MyBase.properties.CurrentState.full
		MyBase.animator.SetBool("isAnti", True)
		Yield CupheadTime.WaitForSeconds(Me, p.fullAttackAnti)
		Me.fullAttack.SetActive(True)
		Yield CupheadTime.WaitForSeconds(Me, p.fullAttackDuration)
		Me.fullAttack.SetActive(False)
		MyBase.animator.SetBool("isAnti", False)
		Yield CupheadTime.WaitForSeconds(Me, p.fullAttackRecovery)
		Me.isAttacking = False
		Yield Nothing
		Return
	End Function

	' Token: 0x0600186D RID: 6253 RVA: 0x000DCC44 File Offset: 0x000DB044
	Private Iterator Function beam_attack_cr() As IEnumerator
		Dim p As LevelProperties.ChessKing.Beam = MyBase.properties.CurrentState.beam
		MyBase.animator.SetBool("isAnti", True)
		Yield CupheadTime.WaitForSeconds(Me, p.beamAttackAnti)
		Me.beamAttack.SetActive(True)
		Yield CupheadTime.WaitForSeconds(Me, p.beamAttackDuration)
		Me.beamAttack.SetActive(False)
		MyBase.animator.SetBool("isAnti", False)
		Yield CupheadTime.WaitForSeconds(Me, p.beamAttackRecovery)
		Me.isAttacking = False
		Yield Nothing
		Return
	End Function

	' Token: 0x0600186E RID: 6254 RVA: 0x000DCC60 File Offset: 0x000DB060
	Private Iterator Function rat_attack_cr() As IEnumerator
		Dim p As LevelProperties.ChessKing.Rat = MyBase.properties.CurrentState.rat
		MyBase.animator.SetBool("isAnti", True)
		Yield CupheadTime.WaitForSeconds(Me, p.ratSummonAnti)
		If Me.rats.Count < p.maxRatNumber Then
			Dim chessKingLevelRat As ChessKingLevelRat = Global.UnityEngine.[Object].Instantiate(Of ChessKingLevelRat)(Me.ratPrefab)
			chessKingLevelRat.Init(Me.ratSpawn.position, p.ratSpeed)
			Me.rats.Add(chessKingLevelRat)
		End If
		Yield CupheadTime.WaitForSeconds(Me, p.ratSummonDuration)
		MyBase.animator.SetBool("isAnti", False)
		Yield CupheadTime.WaitForSeconds(Me, p.ratSummonRecovery)
		Me.isAttacking = False
		Yield Nothing
		Return
	End Function

	' Token: 0x0400218E RID: 8590
	Public Shared multiplayerDamageNerf As Single = 8F

	' Token: 0x0400218F RID: 8591
	Private Const Y_SPAWN As Single = -300F

	' Token: 0x04002190 RID: 8592
	<SerializeField()>
	Private ratPrefab As ChessKingLevelRat

	' Token: 0x04002191 RID: 8593
	Private rats As List(Of ChessKingLevelRat)

	' Token: 0x04002192 RID: 8594
	<SerializeField()>
	Private ratSpawn As Transform

	' Token: 0x04002193 RID: 8595
	<SerializeField()>
	Private beamAttack As GameObject

	' Token: 0x04002194 RID: 8596
	<SerializeField()>
	Private fullAttack As GameObject

	' Token: 0x04002195 RID: 8597
	<SerializeField()>
	Private groundTrigger As ChessKingLevelGroundTrigger

	' Token: 0x04002196 RID: 8598
	<SerializeField()>
	Private parryPoint As ChessKingLevelParryPoint

	' Token: 0x04002197 RID: 8599
	Private parryPoints As List(Of ChessKingLevelParryPoint)

	' Token: 0x04002199 RID: 8601
	Private kingAttackStringMainIndex As Integer

	' Token: 0x0400219A RID: 8602
	Private kingAttackStringIndex As Integer

	' Token: 0x0400219B RID: 8603
	Private trialPoolMainIndex As Integer

	' Token: 0x0400219C RID: 8604
	Private challengeActivated As Boolean

	' Token: 0x0400219D RID: 8605
	Private isAttacking As Boolean
End Class
