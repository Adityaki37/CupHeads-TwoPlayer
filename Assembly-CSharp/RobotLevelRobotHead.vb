Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000779 RID: 1913
Public Class RobotLevelRobotHead
	Inherits RobotLevelRobotBodyPart

	' Token: 0x060029E1 RID: 10721 RVA: 0x00187A44 File Offset: 0x00185E44
	Public Overrides Sub InitBodyPart(parent As RobotLevelRobot, properties As LevelProperties.Robot, Optional primaryHP As Integer = 1, Optional secondaryHP As Integer = 1, Optional attackDelayMinus As Single = 0F)
		MyBase.GetComponent(Of BoxCollider2D)().enabled = True
		Me.currentPlayer = PlayerManager.GetNext()
		Me.primaryAttackDelay = properties.CurrentState.hose.attackDelayRange.RandomFloat()
		Me.secondaryAttackDelay = properties.CurrentState.cannon.attackDelay
		Me.attackStringGroup = Global.UnityEngine.Random.Range(0, properties.CurrentState.cannon.shootString.Length)
		Me.attackStringIndex = Global.UnityEngine.Random.Range(0, properties.CurrentState.cannon.shootString(Me.attackStringGroup).Split(New Char() { ","c }).Length)
		AddHandler parent.OnDeathEvent, AddressOf Me.OnPrimaryDeath
		primaryHP = properties.CurrentState.hose.health
		attackDelayMinus = properties.CurrentState.hose.delayMinus
		MyBase.InitBodyPart(parent, properties, primaryHP, secondaryHP, attackDelayMinus)
		Me.StartPrimary()
	End Sub

	' Token: 0x060029E2 RID: 10722 RVA: 0x00187B38 File Offset: 0x00185F38
	Protected Overrides Sub OnPrimaryAttack()
		If Me.currentPlayer Is Nothing OrElse Me.currentPlayer.IsDead Then
			Me.currentPlayer = PlayerManager.GetNext()
		End If
		If Me.current = RobotLevelRobotBodyPart.state.primary Then
			If Me.currentPlayer.id = PlayerId.PlayerOne Then
				If PlayerManager.GetPlayer(PlayerId.PlayerTwo) IsNot Nothing Then
					Me.currentPlayer = PlayerManager.GetPlayer(PlayerId.PlayerTwo)
				End If
			Else
				Me.currentPlayer = PlayerManager.GetPlayer(PlayerId.PlayerOne)
			End If
			MyBase.StartCoroutine(Me.warningLaser_cr())
			MyBase.OnPrimaryAttack()
		End If
	End Sub

	' Token: 0x060029E3 RID: 10723 RVA: 0x00187BD0 File Offset: 0x00185FD0
	Private Iterator Function warningLaser_cr() As IEnumerator
		If Me.current = RobotLevelRobotBodyPart.state.primary Then
			Yield CupheadTime.WaitForSeconds(Me, Me.properties.CurrentState.hose.warningDuration)
			If Me.current = RobotLevelRobotBodyPart.state.primary Then
				If Me.currentPlayer Is Nothing OrElse Me.currentPlayer.IsDead Then
					Me.currentPlayer = PlayerManager.GetNext()
				End If
				Dim dir As Vector3 = (Me.currentPlayer.center - MyBase.transform.position).normalized
				Me.angle = Vector3.Angle(Vector3.up, dir)
				If Me.angle < 0F Then
					Me.angle *= -1F
				End If
				Me.angle = Mathf.Clamp(Me.angle, Me.properties.CurrentState.hose.aimAngleParameter.min, Me.properties.CurrentState.hose.aimAngleParameter.max)
				Yield Nothing
				Me.laser = Me.primary.GetComponent(Of RobotLevelHoseLaser)().Create(MyBase.transform.position, Me.angle - 90F, Me)
				Me.laser.animator.SetTrigger("OnWarning")
				AudioManager.Play("robot_raygun_charge")
				Me.emitAudioFromObject.Add("robot_raygun_charge")
			End If
			Yield CupheadTime.WaitForSeconds(Me, Me.properties.CurrentState.hose.warningDuration)
			If Me.current = RobotLevelRobotBodyPart.state.primary Then
				Me.laser.animator.SetTrigger("OnAttack")
				AudioManager.Play("robot_raygun_shoot")
				Me.emitAudioFromObject.Add("robot_raygun_shoot")
				Yield Nothing
			Else
				AudioManager.[Stop]("robot_raygun_charge")
			End If
			Yield Nothing
		End If
		If Me.current = RobotLevelRobotBodyPart.state.primary Then
			MyBase.StartCoroutine(Me.attackLaser_cr())
		End If
		Return
	End Function

	' Token: 0x060029E4 RID: 10724 RVA: 0x00187BEC File Offset: 0x00185FEC
	Private Iterator Function attackLaser_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, CSng(Me.properties.CurrentState.hose.beamDuration))
		AudioManager.[Stop]("robot_raygun_charge")
		If Me.laser IsNot Nothing Then
			Global.UnityEngine.[Object].Destroy(Me.laser.gameObject)
			Me.isAttacking = False
		End If
		If CSng(Global.UnityEngine.Random.Range(0, 100)) <= 25F AndAlso Not AudioManager.CheckIfPlaying("robot_vocals_laugh") Then
			AudioManager.Play("robot_vocals_laugh")
			Me.emitAudioFromObject.Add("robot_vocals_laugh")
		End If
		Return
	End Function

	' Token: 0x060029E5 RID: 10725 RVA: 0x00187C08 File Offset: 0x00186008
	Protected Overrides Sub OnPrimaryDeath()
		If Me.current <> RobotLevelRobotBodyPart.state.secondary AndAlso Me.currentHealth(0) <= 0F Then
			Me.parent.animator.SetBool("HeadStageTwoTransition", True)
			MyBase.StartCoroutine(Me.endLasers_cr())
		End If
		MyBase.OnPrimaryDeath()
	End Sub

	' Token: 0x060029E6 RID: 10726 RVA: 0x00187C5C File Offset: 0x0018605C
	Private Iterator Function endLasers_cr() As IEnumerator
		Yield Me.parent.animator.WaitForAnimationToEnd(Me.parent, "Idle", 2, True, True)
		AudioManager.Play("robot_head_antennae_destroyed")
		MyBase.GetComponent(Of BoxCollider2D)().enabled = False
		MyBase.StopCoroutine(Me.warningLaser_cr())
		MyBase.StopCoroutine(Me.attackLaser_cr())
		Me.ExitCurrentAttacks()
		Me.StartSecondary()
		Me.deathEffect.Create(MyBase.transform.position)
		AudioManager.Play("robot_upper_chest_port_destroyed")
		Me.emitAudioFromObject.Add("robot_upper_chest_port_destroyed")
		Return
	End Function

	' Token: 0x060029E7 RID: 10727 RVA: 0x00187C78 File Offset: 0x00186078
	Private Iterator Function startSecondary_cr() As IEnumerator
		Me.StartSecondary()
		Yield Nothing
		Return
	End Function

	' Token: 0x060029E8 RID: 10728 RVA: 0x00187C94 File Offset: 0x00186094
	Protected Overrides Sub OnSecondaryAttack()
		Me.secondaryAttackDelay = Me.properties.CurrentState.cannon.attackDelay
		Dim text As String = Me.properties.CurrentState.cannon.shootString(Me.attackStringGroup).Split(New Char() { ","c })(Me.attackStringIndex)
		Me.attackStringIndex += 1
		If Me.attackStringIndex >= Me.properties.CurrentState.cannon.shootString(Me.attackStringGroup).Split(New Char() { ","c }).Length - 1 Then
			Me.secondaryAttackDelay = Me.properties.CurrentState.cannon.attackDelay
			Me.attackStringIndex = 0
			Me.attackStringGroup += 1
			If Me.attackStringGroup >= Me.properties.CurrentState.cannon.shootString.Length - 1 Then
				Me.attackStringGroup = 0
				Me.secondaryAttackDelay = Me.properties.CurrentState.cannon.attackDelayRange.RandomFloat()
			End If
		End If
		Me.parent.animator.SetTrigger("HeadAttack")
		MyBase.StartCoroutine(Me.spreadShot_cr(text))
		MyBase.OnSecondaryAttack()
	End Sub

	' Token: 0x060029E9 RID: 10729 RVA: 0x00187DE0 File Offset: 0x001861E0
	Private Iterator Function spreadShot_cr(attackString As String) As IEnumerator
		Yield Me.parent.animator.WaitForAnimationToEnd(Me, "Stage Two Idle", 2, True, True)
		Me.cannonSpreadShot(attackString)
		Return
	End Function

	' Token: 0x060029EA RID: 10730 RVA: 0x00187E04 File Offset: 0x00186204
	Private Sub cannonSpreadShot(attackString As String)
		Dim num As Integer = 0
		Parser.IntTryParse(attackString.Substring(1), num)
		num -= 1
		Dim array As String() = Me.properties.CurrentState.cannon.spreadVariableGroups(num).Split(New Char() { ","c })
		Dim num2 As Single = 0F
		Dim num3 As Integer = 0
		Dim minMax As MinMax = New MinMax(0F, 0F)
		For Each text As String In array
			If text(0) = "S"c Then
				Parser.FloatTryParse(text.Substring(1), num2)
			ElseIf text(0) = "N"c Then
				Parser.IntTryParse(text.Substring(1), num3)
			Else
				Dim array3 As String() = text.Split(New Char() { "-"c })
				Parser.FloatTryParse(array3(0), minMax.min)
				Parser.FloatTryParse(array3(1), minMax.max)
			End If
		Next
		AudioManager.Play("robot_head_shoot")
		Me.emitAudioFromObject.Add("robot_head_shoot")
		For j As Integer = 0 To num3 - 1
			Dim floatAt As Single = minMax.GetFloatAt(CSng(j) / (CSng(num3) - 1F))
			If j Mod 2 = 0 Then
				Dim component As BasicProjectile = Me.secondary.GetComponent(Of BasicProjectile)()
				component.Create(MyBase.transform.position, floatAt, num2)
			Else
				Me.nutProjectile.Create(MyBase.transform.position, floatAt, num2)
			End If
		Next
	End Sub

	' Token: 0x060029EB RID: 10731 RVA: 0x00187FA2 File Offset: 0x001863A2
	Protected Overrides Sub ExitCurrentAttacks()
		If Me.laser IsNot Nothing Then
			Global.UnityEngine.[Object].Destroy(Me.laser.gameObject)
		End If
		MyBase.ExitCurrentAttacks()
	End Sub

	' Token: 0x040032C9 RID: 13001
	Private laser As RobotLevelHoseLaser

	' Token: 0x040032CA RID: 13002
	Private currentPlayer As AbstractPlayerController

	' Token: 0x040032CB RID: 13003
	Private angle As Single

	' Token: 0x040032CC RID: 13004
	Private attackStringGroup As Integer

	' Token: 0x040032CD RID: 13005
	Private attackStringIndex As Integer

	' Token: 0x040032CE RID: 13006
	<SerializeField()>
	Private nutProjectile As BasicProjectile
End Class
