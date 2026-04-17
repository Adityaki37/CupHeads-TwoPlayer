Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020005AE RID: 1454
Public Class DicePalaceCigarLevelCigar
	Inherits LevelProperties.DicePalaceCigar.Entity

	' Token: 0x06001C0B RID: 7179 RVA: 0x001016C4 File Offset: 0x000FFAC4
	Protected Overrides Sub Awake()
		Me.damageDealer = DamageDealer.NewEnemy()
		Me.damageReceiver = MyBase.GetComponent(Of DamageReceiver)()
		AddHandler Me.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
		AddHandler Me.collisionChild.OnPlayerCollision, AddressOf Me.OnCollisionPlayer
		MyBase.Awake()
	End Sub

	' Token: 0x06001C0C RID: 7180 RVA: 0x0010171D File Offset: 0x000FFB1D
	Private Sub Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x06001C0D RID: 7181 RVA: 0x00101735 File Offset: 0x000FFB35
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x06001C0E RID: 7182 RVA: 0x00101753 File Offset: 0x000FFB53
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		MyBase.properties.DealDamage(info.damage)
	End Sub

	' Token: 0x06001C0F RID: 7183 RVA: 0x00101768 File Offset: 0x000FFB68
	Public Overrides Sub LevelInit(properties As LevelProperties.DicePalaceCigar)
		Me.onRightSpawn = True
		Me.isFiring = False
		Me.rightAsh.SetActive(False)
		Me.spitAttackCountIndex = Global.UnityEngine.Random.Range(0, properties.CurrentState.spiralSmoke.attackCount.Split(New Char() { ","c }).Length)
		Me.spitAttackDirectionIndex = Global.UnityEngine.Random.Range(0, properties.CurrentState.spiralSmoke.rotationDirectionString.Split(New Char() { ","c }).Length)
		Me.ghostAttackDelayIndex = Global.UnityEngine.Random.Range(0, properties.CurrentState.cigaretteGhost.attackDelayString.Split(New Char() { ","c }).Length)
		Me.ghostSpawnPositionIndex = Global.UnityEngine.Random.Range(0, properties.CurrentState.cigaretteGhost.spawnPositionString.Split(New Char() { ","c }).Length)
		MyBase.LevelInit(properties)
		AddHandler Level.Current.OnIntroEvent, AddressOf Me.OnIntroEnd
		AddHandler Level.Current.OnWinEvent, AddressOf Me.OnDeath
		MyBase.StartCoroutine(Me.intro_cr())
	End Sub

	' Token: 0x06001C10 RID: 7184 RVA: 0x00101888 File Offset: 0x000FFC88
	Private Iterator Function intro_cr() As IEnumerator
		AudioManager.PlayLoop("dice_palace_cigar_intro_start_loop")
		Me.emitAudioFromObject.Add("dice_palace_cigar_intro_start_loop")
		Yield CupheadTime.WaitForSeconds(Me, 2F)
		MyBase.animator.SetTrigger("Continue")
		Yield Nothing
		Return
	End Function

	' Token: 0x06001C11 RID: 7185 RVA: 0x001018A3 File Offset: 0x000FFCA3
	Private Sub StopIntroLoop()
		AudioManager.[Stop]("dice_palace_cigar_intro_start_loop")
	End Sub

	' Token: 0x06001C12 RID: 7186 RVA: 0x001018AF File Offset: 0x000FFCAF
	Private Sub OnIntroEnd()
		MyBase.StartCoroutine(Me.attack_cr())
		MyBase.StartCoroutine(Me.ghostAttack_cr())
	End Sub

	' Token: 0x06001C13 RID: 7187 RVA: 0x001018CC File Offset: 0x000FFCCC
	Private Iterator Function attack_cr() As IEnumerator
		While True
			MyBase.GetComponent(Of BoxCollider2D)().enabled = True
			Me.maxCounter = Parser.IntParse(MyBase.properties.CurrentState.spiralSmoke.attackCount.Split(New Char() { ","c })(Me.spitAttackCountIndex))
			Me.isFiring = True
			While Me.isFiring
				If Me.counter > Me.maxCounter Then
					Me.isFiring = False
					Me.counter = 0
					Exit While
				End If
				Yield CupheadTime.WaitForSeconds(Me, MyBase.properties.CurrentState.spiralSmoke.hesitateBeforeAttackDelay)
				Me.counter += 1
				MyBase.animator.SetTrigger("IsAttacking")
				Yield MyBase.animator.WaitForAnimationToEnd(Me, "Attack", False, True)
				Yield Nothing
			End While
			Me.spitAttackCountIndex += 1
			If Me.spitAttackCountIndex >= MyBase.properties.CurrentState.spiralSmoke.attackCount.Split(New Char() { ","c }).Length Then
				Me.spitAttackCountIndex = 0
			End If
			Me.spitAttackDirectionIndex += 1
			If Me.spitAttackDirectionIndex >= MyBase.properties.CurrentState.spiralSmoke.rotationDirectionString.Split(New Char() { ","c }).Length Then
				Me.spitAttackDirectionIndex = 0
			End If
			Yield CupheadTime.WaitForSeconds(Me, MyBase.properties.CurrentState.cigar.warningDelay)
			MyBase.animator.SetTrigger("OnStateChange")
			Yield MyBase.animator.WaitForAnimationToEnd(Me, "Teleport_End", False, True)
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06001C14 RID: 7188 RVA: 0x001018E7 File Offset: 0x000FFCE7
	Private Sub TeleportSFX()
		AudioManager.Play("dice_palace_cigar_teleport")
		Me.emitAudioFromObject.Add("dice_palace_cigar_teleport")
	End Sub

	' Token: 0x06001C15 RID: 7189 RVA: 0x00101903 File Offset: 0x000FFD03
	Private Sub AttackSFX()
		AudioManager.Play("dice_palace_cigar_attack")
		Me.emitAudioFromObject.Add("dice_palace_cigar_attack")
	End Sub

	' Token: 0x06001C16 RID: 7190 RVA: 0x00101920 File Offset: 0x000FFD20
	Private Sub SwitchSides()
		Me.leftAsh.SetActive(Not Me.onRightSpawn)
		Me.rightAsh.SetActive(Me.onRightSpawn)
		Me.onRightSpawn = Not Me.onRightSpawn
		If Not Me.facingBack Then
			MyBase.transform.Rotate(Vector3.up, 180F)
		End If
		If Me.onRightSpawn Then
			MyBase.transform.position = Me.rightSpawnPointFacingRight.position
		Else
			MyBase.transform.position = Me.leftSpawnPointFacingRight.position
		End If
		MyBase.StartCoroutine(Me.finish_teleport_cr())
	End Sub

	' Token: 0x06001C17 RID: 7191 RVA: 0x001019CC File Offset: 0x000FFDCC
	Private Sub Rotate()
		Dim [next] As AbstractPlayerController = PlayerManager.GetNext()
		If [next].transform.position.x < MyBase.transform.position.x AndAlso Not Me.onRightSpawn Then
			MyBase.transform.position = Me.leftSpawnPointFacingLeft.position
			MyBase.transform.Rotate(Vector3.up, 180F)
			Me.facingBack = True
		ElseIf [next].transform.position.x > MyBase.transform.position.x AndAlso Me.onRightSpawn Then
			MyBase.transform.position = Me.rightSpawnPointFacingLeft.position
			MyBase.transform.Rotate(Vector3.up, 180F)
			Me.facingBack = True
		Else
			MyBase.transform.Rotate(Vector3.up, 0F)
			Me.facingBack = False
		End If
	End Sub

	' Token: 0x06001C18 RID: 7192 RVA: 0x00101AD8 File Offset: 0x000FFED8
	Private Sub CheckIfBackward()
		If Me.facingBack Then
			MyBase.transform.Rotate(Vector3.up, 180F)
			If Me.onRightSpawn Then
				MyBase.transform.position = Me.rightSpawnPointFacingRight.position
			Else
				MyBase.transform.position = Me.leftSpawnPointFacingRight.position
			End If
			Me.facingBack = False
		End If
	End Sub

	' Token: 0x06001C19 RID: 7193 RVA: 0x00101B48 File Offset: 0x000FFF48
	Private Iterator Function finish_teleport_cr() As IEnumerator
		AudioManager.PlayLoop("dice_palace_cigar_teleport_warning_loop")
		Me.emitAudioFromObject.Add("dice_palace_cigar_teleport_warning_loop")
		Yield CupheadTime.WaitForSeconds(Me, MyBase.properties.CurrentState.cigar.warningDelay)
		Me.VOXTeleportWarning()
		AudioManager.[Stop]("dice_palace_cigar_teleport_warning_loop")
		AudioManager.Play("dice_palace_cigar_teleport_end")
		Me.emitAudioFromObject.Add("dice_palace_cigar_teleport_end")
		MyBase.animator.SetTrigger("Continue")
		Yield Nothing
		Return
	End Function

	' Token: 0x06001C1A RID: 7194 RVA: 0x00101B64 File Offset: 0x000FFF64
	Private Sub SpitAttack()
		Dim flag As Boolean
		If Me.facingBack Then
			flag = Not Me.onRightSpawn
		Else
			flag = Me.onRightSpawn
		End If
		Dim abstractProjectile As AbstractProjectile = Me.spitPrefab.Create(Me.spitSpawnPoint.position, CSng(CInt(MyBase.transform.eulerAngles.y)))
		If MyBase.properties.CurrentState.spiralSmoke.rotationDirectionString.Split(New Char() { ","c })(Me.spitAttackDirectionIndex)(0) = "1"c Then
			abstractProjectile.GetComponent(Of DicePalaceCigarLevelCigarSpit)().InitProjectile(MyBase.properties, True, flag)
		Else
			abstractProjectile.GetComponent(Of DicePalaceCigarLevelCigarSpit)().InitProjectile(MyBase.properties, False, flag)
		End If
	End Sub

	' Token: 0x06001C1B RID: 7195 RVA: 0x00101C2C File Offset: 0x0010002C
	Private Iterator Function ghostAttack_cr() As IEnumerator
		While True
			Dim spawnPosx As Single = Global.UnityEngine.Random.Range(Me.ghostSpawnPoint.transform.position.x - Me.ghostOffset, Me.ghostSpawnPoint.transform.position.x + Me.ghostOffset)
			Yield CupheadTime.WaitForSeconds(Me, Parser.FloatParse(MyBase.properties.CurrentState.cigaretteGhost.attackDelayString.Split(New Char() { ","c })(Me.ghostAttackDelayIndex)))
			Dim proj As AbstractProjectile = Me.ghostPrefab.Create(New Vector2(spawnPosx, Me.ghostSpawnPoint.transform.position.y))
			proj.GetComponent(Of DicePalaceCigarLevelCigaretteGhost)().InitGhost(MyBase.properties)
			Me.ghostAttackDelayIndex += 1
			If Me.ghostAttackDelayIndex >= MyBase.properties.CurrentState.cigaretteGhost.attackDelayString.Split(New Char() { ","c }).Length Then
				Me.ghostAttackDelayIndex = 0
			End If
			Me.ghostSpawnPositionIndex += 1
			If Me.ghostSpawnPositionIndex >= MyBase.properties.CurrentState.cigaretteGhost.spawnPositionString.Split(New Char() { ","c }).Length Then
				Me.ghostSpawnPositionIndex = 0
			End If
		End While
		Return
	End Function

	' Token: 0x06001C1C RID: 7196 RVA: 0x00101C47 File Offset: 0x00100047
	Private Sub SmokeAB()
		Me.smokeA.Create(Me.smokeSpawnPoint.transform.position)
		Me.smokeB.Create(Me.smokeSpawnPoint.transform.position)
	End Sub

	' Token: 0x06001C1D RID: 7197 RVA: 0x00101C81 File Offset: 0x00100081
	Private Sub SmokeB()
		Me.smokeB.Create(Me.smokeSpawnPoint.transform.position)
	End Sub

	' Token: 0x06001C1E RID: 7198 RVA: 0x00101CA0 File Offset: 0x001000A0
	Private Sub SwitchLayer()
		MyBase.GetComponent(Of SpriteRenderer)().sortingLayerName = SpriteLayer.Map.ToString()
		MyBase.GetComponent(Of SpriteRenderer)().sortingOrder = 200
	End Sub

	' Token: 0x06001C1F RID: 7199 RVA: 0x00101CD7 File Offset: 0x001000D7
	Protected Overrides Sub OnDestroy()
		Me.StopAllCoroutines()
		MyBase.OnDestroy()
		Me.smokeA = Nothing
		Me.smokeB = Nothing
		Me.spitPrefab = Nothing
		Me.ghostPrefab = Nothing
	End Sub

	' Token: 0x06001C20 RID: 7200 RVA: 0x00101D04 File Offset: 0x00100104
	Private Sub OnDeath()
		AudioManager.Play("dice_palace_cigar_death")
		Me.emitAudioFromObject.Add("dice_palace_cigar_death")
		Me.VOXDeath()
		Me.StopAllCoroutines()
		MyBase.GetComponent(Of Collider2D)().enabled = False
		MyBase.animator.SetTrigger("OnDeath")
	End Sub

	' Token: 0x06001C21 RID: 7201 RVA: 0x00101D53 File Offset: 0x00100153
	Private Sub DeathSFX()
		AudioManager.Play("dice_palace_cigar_death_end")
		Me.emitAudioFromObject.Add("dice_palace_cigar_death_end")
	End Sub

	' Token: 0x06001C22 RID: 7202 RVA: 0x00101D6F File Offset: 0x0010016F
	Private Sub VOXIntro()
		AudioManager.Play("cigar_vox_intro")
		Me.emitAudioFromObject.Add("cigar_vox_intro")
	End Sub

	' Token: 0x06001C23 RID: 7203 RVA: 0x00101D8B File Offset: 0x0010018B
	Private Sub VOXDeath()
		AudioManager.Play("cigar_vox_death")
		Me.emitAudioFromObject.Add("cigar_vox_death")
	End Sub

	' Token: 0x06001C24 RID: 7204 RVA: 0x00101DA7 File Offset: 0x001001A7
	Private Sub VOXTeleport()
		AudioManager.Play("cigar_vox_pre_teleport")
		Me.emitAudioFromObject.Add("cigar_vox_pre_teleport")
	End Sub

	' Token: 0x06001C25 RID: 7205 RVA: 0x00101DC3 File Offset: 0x001001C3
	Private Sub VOXTeleportWarning()
		AudioManager.Play("cigar_vox_warning")
		Me.emitAudioFromObject.Add("cigar_vox_warning")
	End Sub

	' Token: 0x06001C26 RID: 7206 RVA: 0x00101DE0 File Offset: 0x001001E0
	Protected Overrides Sub OnDrawGizmos()
		MyBase.OnDrawGizmos()
		Gizmos.DrawLine(New Vector2(Me.ghostSpawnPoint.transform.position.x - Me.ghostOffset, Me.ghostSpawnPoint.transform.position.y), New Vector2(Me.ghostSpawnPoint.transform.position.x + Me.ghostOffset, Me.ghostSpawnPoint.transform.position.y))
	End Sub

	' Token: 0x0400251A RID: 9498
	<Space(5F)>
	<SerializeField()>
	Private leftAshTray As GameObject

	' Token: 0x0400251B RID: 9499
	<SerializeField()>
	Private rightAshTray As GameObject

	' Token: 0x0400251C RID: 9500
	<Space(5F)>
	<SerializeField()>
	Private leftAsh As GameObject

	' Token: 0x0400251D RID: 9501
	<SerializeField()>
	Private rightAsh As GameObject

	' Token: 0x0400251E RID: 9502
	<Space(5F)>
	<SerializeField()>
	Private leftSpawnPointFacingRight As Transform

	' Token: 0x0400251F RID: 9503
	<SerializeField()>
	Private leftSpawnPointFacingLeft As Transform

	' Token: 0x04002520 RID: 9504
	<SerializeField()>
	Private rightSpawnPointFacingLeft As Transform

	' Token: 0x04002521 RID: 9505
	<SerializeField()>
	Private rightSpawnPointFacingRight As Transform

	' Token: 0x04002522 RID: 9506
	<Space(5F)>
	<SerializeField()>
	Private smokeSpawnPoint As Transform

	' Token: 0x04002523 RID: 9507
	<SerializeField()>
	Private smokeA As Effect

	' Token: 0x04002524 RID: 9508
	<SerializeField()>
	Private smokeB As Effect

	' Token: 0x04002525 RID: 9509
	<SerializeField()>
	Private collisionChild As CollisionChild

	' Token: 0x04002526 RID: 9510
	<Space(10F)>
	<SerializeField()>
	Private spitPrefab As DicePalaceCigarLevelCigarSpit

	' Token: 0x04002527 RID: 9511
	<SerializeField()>
	Private spitSpawnPoint As Transform

	' Token: 0x04002528 RID: 9512
	<SerializeField()>
	Private ghostPrefab As DicePalaceCigarLevelCigaretteGhost

	' Token: 0x04002529 RID: 9513
	<SerializeField()>
	Private ghostSpawnPoint As Transform

	' Token: 0x0400252A RID: 9514
	<SerializeField()>
	Private ghostOffset As Single

	' Token: 0x0400252B RID: 9515
	Private isVisible As Boolean

	' Token: 0x0400252C RID: 9516
	Private onRightSpawn As Boolean

	' Token: 0x0400252D RID: 9517
	Private isFiring As Boolean

	' Token: 0x0400252E RID: 9518
	Private facingBack As Boolean

	' Token: 0x0400252F RID: 9519
	Private spitAttackCountIndex As Integer

	' Token: 0x04002530 RID: 9520
	Private spitAttackDirectionIndex As Integer

	' Token: 0x04002531 RID: 9521
	Private ghostAttackDelayIndex As Integer

	' Token: 0x04002532 RID: 9522
	Private ghostSpawnPositionIndex As Integer

	' Token: 0x04002533 RID: 9523
	Private counter As Integer

	' Token: 0x04002534 RID: 9524
	Private maxCounter As Integer

	' Token: 0x04002535 RID: 9525
	Private damageReceiver As DamageReceiver

	' Token: 0x04002536 RID: 9526
	Private damageDealer As DamageDealer
End Class
