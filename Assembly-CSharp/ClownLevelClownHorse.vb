Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports UnityEngine

' Token: 0x0200055D RID: 1373
Public Class ClownLevelClownHorse
	Inherits LevelProperties.Clown.Entity

	' Token: 0x17000347 RID: 839
	' (get) Token: 0x060019B8 RID: 6584 RVA: 0x000E9FAC File Offset: 0x000E83AC
	' (set) Token: 0x060019B9 RID: 6585 RVA: 0x000E9FB4 File Offset: 0x000E83B4
	Public Property horseType As ClownLevelClownHorse.HorseType

	' Token: 0x060019BA RID: 6586 RVA: 0x000E9FC0 File Offset: 0x000E83C0
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.damageDealer = DamageDealer.NewEnemy()
		Me.damageReceiver = Me.clownHorseHead.GetComponent(Of DamageReceiver)()
		AddHandler Me.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
		AddHandler Me.clownHorseHead.GetComponent(Of CollisionChild)().OnPlayerCollision, AddressOf Me.OnCollisionPlayer
		MyBase.gameObject.SetActive(False)
	End Sub

	' Token: 0x060019BB RID: 6587 RVA: 0x000EA02F File Offset: 0x000E842F
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		MyBase.properties.DealDamage(info.damage)
		If Level.Current.mode = Level.Mode.Easy Then
			AddHandler Level.Current.OnLevelEndEvent, AddressOf Me.Dead
		End If
	End Sub

	' Token: 0x060019BC RID: 6588 RVA: 0x000EA067 File Offset: 0x000E8467
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x060019BD RID: 6589 RVA: 0x000EA085 File Offset: 0x000E8485
	Private Sub Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x060019BE RID: 6590 RVA: 0x000EA0A0 File Offset: 0x000E84A0
	Public Sub StartCarouselHorse()
		MyBase.gameObject.SetActive(True)
		MyBase.animator.SetTrigger("Start")
		Dim horse As LevelProperties.Clown.Horse = MyBase.properties.CurrentState.horse
		Me.dropMainIndex = Global.UnityEngine.Random.Range(0, horse.DropBulletPositionString.Length)
		Me.pinkMainIndex = Global.UnityEngine.Random.Range(0, horse.WavePinkString.Length)
		Me.horseTypePattern = horse.HorseString.GetRandom().Split(New Char() { ","c })
		Me.horseTypeIndex = Global.UnityEngine.Random.Range(0, Me.horseTypePattern.Length)
		Me.wavePositionPattern = horse.WavePosString.GetRandom().Split(New Char() { ","c })
		Me.wavePinkPattern = horse.WavePinkString(Me.pinkMainIndex).Split(New Char() { ","c })
		Me.dropPositionPattern = horse.DropHorsePositionString.GetRandom().Split(New Char() { ","c })
		Me.dropBulletPositionPattern = horse.DropBulletPositionString(Me.dropMainIndex).Split(New Char() { ","c })
		Me.dropBulletIndex = Global.UnityEngine.Random.Range(0, Me.dropBulletPositionPattern.Length)
		Me.StopAllCoroutines()
		MyBase.StartCoroutine(Me.select_horse_cr())
	End Sub

	' Token: 0x060019BF RID: 6591 RVA: 0x000EA1E6 File Offset: 0x000E85E6
	Private Sub BounceSFX()
		AudioManager.Play("clown_horse_clown")
		Me.emitAudioFromObject.Add("clown_horse_clown")
	End Sub

	' Token: 0x060019C0 RID: 6592 RVA: 0x000EA202 File Offset: 0x000E8602
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me.regularHorseshoe = Nothing
		Me.pinkHorseshoe = Nothing
		Me.spitFxPrefabA = Nothing
		Me.spitFxPrefabB = Nothing
	End Sub

	' Token: 0x060019C1 RID: 6593 RVA: 0x000EA228 File Offset: 0x000E8628
	Private Iterator Function select_horse_cr() As IEnumerator
		Dim p As LevelProperties.Clown.Horse = MyBase.properties.CurrentState.horse
		If Me.horseTypePattern(Me.horseTypeIndex)(0) = "W"c Then
			MyBase.animator.SetBool("IsWave", True)
			MyBase.StartCoroutine(Me.horse_cr(ClownLevelClownHorse.HorseType.Wave, Me.wavePositionPattern, p.WaveATKRepeat))
		ElseIf Me.horseTypePattern(Me.horseTypeIndex)(0) = "D"c Then
			MyBase.animator.SetBool("IsWave", False)
			MyBase.StartCoroutine(Me.horse_cr(ClownLevelClownHorse.HorseType.Drop, Me.dropPositionPattern, p.DropATKRepeat))
		Else
			Global.Debug.LogError("Horse Type Pattern is messed up!", Nothing)
		End If
		If Me.horseTypeIndex < Me.horseTypePattern.Length - 1 Then
			Me.horseTypeIndex += 1
		Else
			Me.horseTypeIndex = 0
		End If
		Yield Nothing
		Return
	End Function

	' Token: 0x060019C2 RID: 6594 RVA: 0x000EA244 File Offset: 0x000E8644
	Private Iterator Function horse_cr(horseType As ClownLevelClownHorse.HorseType, positionPattern As String(), ATKAmount As Single) As IEnumerator
		Dim isPink As Boolean = False
		Dim hesitate As Single = 0F
		Dim posOffset As Single = 0F
		Dim ATKcounter As Single = 0F
		Dim YSpeed As Single = 0F
		Dim p As LevelProperties.Clown.Horse = MyBase.properties.CurrentState.horse
		Me.SelectStartPos()
		While ATKcounter < ATKAmount
			Parser.FloatTryParse(positionPattern(Me.positionIndex), posOffset)
			Dim getPos As Single = 360F - posOffset
			While MyBase.transform.position.y <> getPos
				Me.pos = MyBase.transform.position
				Me.pos.y = Mathf.MoveTowards(MyBase.transform.position.y, getPos, p.HorseSpeed * CupheadTime.Delta)
				MyBase.transform.position = Me.pos
				Yield Nothing
			End While
			MyBase.StartCoroutine(Me.spit_fx_cr())
			If horseType <> ClownLevelClownHorse.HorseType.Wave Then
				If horseType = ClownLevelClownHorse.HorseType.Drop Then
					hesitate = p.DropHesitate
					Dim spawnY As Single = 0F
					Dim nextSpawnY As Single = 0F
					Dim i As Integer = 0
					Me.dropBulletPositionPattern = p.DropBulletPositionString(Me.dropMainIndex).Split(New Char() { ","c })
					Dim droppattern As String() = Me.dropBulletPositionPattern(Me.dropBulletIndex).Split(New Char() { "-"c })
					Me.SpitSFX()
					MyBase.animator.SetBool("Spit", True)
					Dim durationBeforeDrops As Single() = New Single(droppattern.Length - 1) {}
					Dim indexPatterns As List(Of Integer) = New List(Of Integer)(droppattern.Length)
					For l As Integer = 0 To droppattern.Length - 1
						indexPatterns.Add(l)
					Next
					Dim currentDuration As Single = MyBase.properties.CurrentState.horse.DropBulletDelay
					Dim dropTwo As Boolean = True
					While indexPatterns.Count > 0
						If indexPatterns.Count > 1 AndAlso dropTwo Then
							currentDuration += MyBase.properties.CurrentState.horse.DropBulletTwoDelay.RandomFloat()
							Dim num As Integer = Global.UnityEngine.Random.Range(0, indexPatterns.Count)
							durationBeforeDrops(indexPatterns(num)) = currentDuration
							indexPatterns.RemoveAt(num)
							num = Global.UnityEngine.Random.Range(0, indexPatterns.Count)
							durationBeforeDrops(indexPatterns(num)) = currentDuration
							indexPatterns.RemoveAt(num)
							dropTwo = False
						Else
							currentDuration += MyBase.properties.CurrentState.horse.DropBulletOneDelay.RandomFloat()
							Dim num2 As Integer = Global.UnityEngine.Random.Range(0, indexPatterns.Count)
							durationBeforeDrops(indexPatterns(num2)) = currentDuration
							indexPatterns.RemoveAt(num2)
							dropTwo = True
						End If
					End While
					For j As Integer = 0 To droppattern.Length - 1
						If j < droppattern.Length - 1 Then
							i = j + 1
						Else
							i = 0
						End If
						Parser.FloatTryParse(droppattern(j), spawnY)
						Parser.FloatTryParse(droppattern(i), nextSpawnY)
						Dim dist As Single = nextSpawnY - spawnY
						Me.FireDropBullets(spawnY, durationBeforeDrops(j))
						Dim halfSpeed As Single = p.DropBulletInitalSpeed / 2F
						Yield CupheadTime.WaitForSeconds(Me, dist / halfSpeed / 2F)
					Next
					MyBase.animator.SetBool("Spit", False)
					If Me.dropBulletIndex < Me.dropBulletPositionPattern.Length - 1 Then
						Me.dropBulletIndex += 1
					Else
						Me.dropMainIndex = (Me.dropMainIndex + 1) Mod p.DropBulletPositionString.Length
						Me.dropBulletIndex = 0
					End If
					Yield CupheadTime.WaitForSeconds(Me, p.DropATKDelay)
				End If
			Else
				hesitate = p.WaveHesitate
				Dim pos As Vector3 = Me.projectileRoot.transform.position
				If Rand.Bool() Then
					YSpeed = -p.WaveBulletWaveSpeed
				Else
					YSpeed = p.WaveBulletWaveSpeed
				End If
				Me.SpitSFX()
				MyBase.animator.SetBool("Spit", True)
				For k As Integer = 0 To p.WaveBulletCount - 1
					Me.wavePinkPattern = p.WavePinkString(Me.pinkMainIndex).Split(New Char() { ","c })
					If Me.wavePinkPattern(Me.pinkIndex)(0) = "R"c Then
						isPink = False
					ElseIf Me.wavePinkPattern(Me.pinkIndex)(0) = "P"c Then
						isPink = True
					End If
					Me.FireWaveBullets(k, isPink, YSpeed, pos)
					If Me.pinkIndex < Me.wavePinkPattern.Length - 1 Then
						Me.pinkIndex += 1
					Else
						Me.pinkMainIndex = (Me.pinkMainIndex + 1) Mod p.WavePinkString.Length
						Me.pinkIndex = 0
					End If
					Yield CupheadTime.WaitForSeconds(Me, p.WaveBulletDelay)
				Next
				MyBase.animator.SetBool("Spit", False)
				Yield CupheadTime.WaitForSeconds(Me, p.WaveATKDelay)
			End If
			Me.positionIndex = Me.positionIndex Mod positionPattern.Length
			ATKcounter += 1F
		End While
		Yield CupheadTime.WaitForSeconds(Me, hesitate)
		While MyBase.transform.position.y <> Me.startPos.y
			Me.pos = MyBase.transform.position
			Me.pos.y = Mathf.MoveTowards(MyBase.transform.position.y, Me.startPos.y, p.HorseSpeed * CupheadTime.Delta)
			MyBase.transform.position = Me.pos
			Yield Nothing
		End While
		MyBase.StartCoroutine(Me.select_horse_cr())
		Yield Nothing
		Return
	End Function

	' Token: 0x060019C3 RID: 6595 RVA: 0x000EA274 File Offset: 0x000E8674
	Private Iterator Function spit_fx_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 0.167F)
		Do
			Me.spitFxPrefabA.Create(Me.spitFxRoot.position, MyBase.transform.localScale)
			Yield CupheadTime.WaitForSeconds(Me, Global.UnityEngine.Random.Range(0.125F, 0.21F))
			If Not MyBase.animator.GetBool("Spit") Then
				Exit Do
			End If
			Me.spitFxPrefabB.Create(Me.spitFxRoot.position, MyBase.transform.localScale)
			Yield CupheadTime.WaitForSeconds(Me, Global.UnityEngine.Random.Range(0.125F, 0.21F))
		Loop While MyBase.animator.GetBool("Spit")
		Return
	End Function

	' Token: 0x060019C4 RID: 6596 RVA: 0x000EA290 File Offset: 0x000E8690
	Private Sub SelectStartPos()
		Me.startPos.y = 860F
		If Rand.Bool() Then
			Me.startPos.x = -640F + MyBase.properties.CurrentState.horse.HorseXPosOffset
			MyBase.transform.position = Me.startPos
			MyBase.transform.SetScale(New Single?(-1F), New Single?(1F), New Single?(1F))
		Else
			Me.startPos.x = 640F - MyBase.properties.CurrentState.horse.HorseXPosOffset
			MyBase.transform.position = Me.startPos
			MyBase.transform.SetScale(New Single?(1F), New Single?(1F), New Single?(1F))
		End If
	End Sub

	' Token: 0x060019C5 RID: 6597 RVA: 0x000EA37C File Offset: 0x000E877C
	Private Sub SpitSFX()
		AudioManager.Play("clown_horse_head_spit")
		Me.emitAudioFromObject.Add("clown_horse_head_spit")
	End Sub

	' Token: 0x060019C6 RID: 6598 RVA: 0x000EA398 File Offset: 0x000E8798
	Private Sub FireWaveBullets(index As Integer, isPink As Boolean, YSpeed As Single, pos As Vector3)
		Dim horse As LevelProperties.Clown.Horse = MyBase.properties.CurrentState.horse
		Dim flag As Boolean = MyBase.transform.position.x > 0F
		If isPink Then
			Dim clownLevelHorseshoe As ClownLevelHorseshoe = Global.UnityEngine.[Object].Instantiate(Of ClownLevelHorseshoe)(Me.pinkHorseshoe)
			clownLevelHorseshoe.Init(pos, horse.WaveBulletSpeed, YSpeed, flag, 0F, horse, ClownLevelClownHorse.HorseType.Wave)
		Else
			Dim clownLevelHorseshoe2 As ClownLevelHorseshoe = Global.UnityEngine.[Object].Instantiate(Of ClownLevelHorseshoe)(Me.regularHorseshoe)
			clownLevelHorseshoe2.Init(pos, horse.WaveBulletSpeed, YSpeed, flag, 0F, horse, ClownLevelClownHorse.HorseType.Wave)
		End If
	End Sub

	' Token: 0x060019C7 RID: 6599 RVA: 0x000EA438 File Offset: 0x000E8838
	Private Sub FireDropBullets(spawnY As Single, durationBeforeDrop As Single)
		Dim horse As LevelProperties.Clown.Horse = MyBase.properties.CurrentState.horse
		Dim flag As Boolean = Me.projectileRoot.transform.position.x > 0F
		Dim clownLevelHorseshoe As ClownLevelHorseshoe = Global.UnityEngine.[Object].Instantiate(Of ClownLevelHorseshoe)(Me.regularHorseshoe)
		clownLevelHorseshoe.Init(Me.projectileRoot.transform.position, horse.DropBulletInitalSpeed, spawnY, flag, durationBeforeDrop, horse, ClownLevelClownHorse.HorseType.Drop)
	End Sub

	' Token: 0x060019C8 RID: 6600 RVA: 0x000EA4B2 File Offset: 0x000E88B2
	Public Sub StartDeath()
		Me.StopAllCoroutines()
		MyBase.StartCoroutine(Me.horse_death_cr())
	End Sub

	' Token: 0x060019C9 RID: 6601 RVA: 0x000EA4C8 File Offset: 0x000E88C8
	Private Iterator Function horse_death_cr() As IEnumerator
		Dim t As Single = 0F
		Dim time As Single = 3F
		Dim start As Vector2 = MyBase.transform.position
		Me.startPos.x = MyBase.transform.position.x
		MyBase.GetComponent(Of SpriteRenderer)().color = ColorUtils.HexToColor("FFFFFFFF")
		Me.clownHorseHead.GetComponent(Of Collider2D)().enabled = False
		Me.clownHorseBody.GetComponent(Of Collider2D)().enabled = False
		Me.StartExplosions()
		MyBase.animator.Play("Off")
		MyBase.animator.SetTrigger("Dead")
		Me.FallHorseScreamSFX()
		Yield CupheadTime.WaitForSeconds(Me, 1F)
		While t < time
			Dim val As Single = EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, 0F, 1F, t / time)
			MyBase.transform.position = Vector2.Lerp(start, Me.startPos, val)
			t += CupheadTime.Delta
			Yield Nothing
		End While
		t = 0F
		Me.pos.x = 0F
		Me.pos.y = MyBase.transform.position.y
		MyBase.transform.position = Me.pos
		Yield CupheadTime.WaitForSeconds(Me, 0.75F)
		While t < time
			Dim val2 As Single = EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, 0F, 1F, t / time)
			MyBase.transform.position = Vector2.Lerp(Me.pos, New Vector3(0F, 250F, 0F), val2)
			t += CupheadTime.Delta
			Yield Nothing
		End While
		Me.startPos.x = MyBase.transform.position.x
		Me.EndExplosions()
		While Me.clownHorseHead.GetComponent(Of HitFlash)().flashing
			Yield Nothing
		End While
		Global.UnityEngine.[Object].Destroy(Me.clownHorseHead.GetComponent(Of HitFlash)())
		Yield CupheadTime.WaitForSeconds(Me, 1F)
		MyBase.GetComponent(Of SpriteRenderer)().sortingLayerName = "Player"
		MyBase.GetComponent(Of SpriteRenderer)().sortingOrder = 200
		MyBase.animator.SetTrigger("Fall")
		Me.moveObject = MyBase.transform
		Yield CupheadTime.WaitForSeconds(Me, 0.3F)
		While Not Me.droppedClown
			Yield Nothing
		End While
		t = 0F
		time = 3F
		While t < time
			If CupheadTime.Delta IsNot 0F Then
				Dim num As Single = EaseUtils.Ease(EaseUtils.EaseType.linear, 0F, 1F, t / time)
				Me.moveObject.transform.position = Vector2.Lerp(Me.moveObject.transform.position, Me.startPos, num)
				t += CupheadTime.Delta
			End If
			Yield Nothing
		End While
		Me.moveObject.transform.position = Me.startPos
		Global.UnityEngine.[Object].Destroy(Me.moveObject.gameObject)
		Yield Nothing
		Return
	End Function

	' Token: 0x060019CA RID: 6602 RVA: 0x000EA4E3 File Offset: 0x000E88E3
	Private Sub Separate()
		Me.clownHorseBody.transform.parent = Nothing
		Me.moveObject = Me.clownHorseBody.transform
	End Sub

	' Token: 0x060019CB RID: 6603 RVA: 0x000EA508 File Offset: 0x000E8908
	Private Iterator Function clown_fall_cr() As IEnumerator
		Dim fallGravity As Single = -100F
		Dim fallAccumulatedGravity As Single = 0F
		Dim fallVelocity As Vector2 = Vector3.zero
		Me.FallHorseSFXOff()
		Me.droppedClown = True
		While MyBase.transform.position.y > -660F
			If CupheadTime.Delta IsNot 0F Then
				MyBase.transform.position += (fallVelocity + New Vector2(-300F, fallAccumulatedGravity)) * CupheadTime.FixedDelta
				fallAccumulatedGravity += fallGravity
			End If
			Yield Nothing
		End While
		Yield CupheadTime.WaitForSeconds(Me, 0.1F)
		While Me.moveObject IsNot Nothing
			Yield Nothing
		End While
		Me.clownSwing.StartSwing()
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
		Yield Nothing
		Return
	End Function

	' Token: 0x060019CC RID: 6604 RVA: 0x000EA523 File Offset: 0x000E8923
	Private Sub StartExplosions()
		Me.clownHorseHead.GetComponent(Of LevelBossDeathExploder)().StartExplosion()
	End Sub

	' Token: 0x060019CD RID: 6605 RVA: 0x000EA535 File Offset: 0x000E8935
	Private Sub EndExplosions()
		Me.clownHorseHead.GetComponent(Of LevelBossDeathExploder)().StopExplosions()
	End Sub

	' Token: 0x060019CE RID: 6606 RVA: 0x000EA548 File Offset: 0x000E8948
	Private Sub Dead()
		Me.StopAllCoroutines()
		MyBase.animator.Play("Off")
		MyBase.animator.SetTrigger("Dead")
		RemoveHandler Level.Current.OnLevelEndEvent, AddressOf Me.Dead
		MyBase.StartCoroutine(Me.move_to_death_spot_cr())
	End Sub

	' Token: 0x060019CF RID: 6607 RVA: 0x000EA5A0 File Offset: 0x000E89A0
	Private Iterator Function move_to_death_spot_cr() As IEnumerator
		Dim t As Single = 0F
		Dim time As Single = 1F
		Yield CupheadTime.WaitForSeconds(Me, 0.2F)
		While t < time
			Dim val As Single = EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, 0F, 1F, t / time)
			MyBase.transform.position = Vector2.Lerp(MyBase.transform.position, New Vector3(Me.pos.x, 250F, 0F), val)
			t += CupheadTime.Delta
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x060019D0 RID: 6608 RVA: 0x000EA5BB File Offset: 0x000E89BB
	Private Sub FallHorseSFX()
		AudioManager.Play("clown_horse_death_slide")
	End Sub

	' Token: 0x060019D1 RID: 6609 RVA: 0x000EA5C7 File Offset: 0x000E89C7
	Private Sub FallHorseScreamSFX()
		If Not Me.ScreamSFXPlaying Then
			Me.ScreamSFXPlaying = True
			AudioManager.PlayLoop("clown_horse_death")
		End If
	End Sub

	' Token: 0x060019D2 RID: 6610 RVA: 0x000EA5E5 File Offset: 0x000E89E5
	Private Sub FallHorseSFXOff()
		AudioManager.FadeSFXVolume("clown_horse_death", 0F, 1F)
		AudioManager.Play("clown_horse_death_end")
	End Sub

	' Token: 0x040022CF RID: 8911
	<SerializeField()>
	Private clownHorseBody As GameObject

	' Token: 0x040022D0 RID: 8912
	<SerializeField()>
	Private clownHorseHead As GameObject

	' Token: 0x040022D1 RID: 8913
	<SerializeField()>
	Private clownSwing As ClownLevelClownSwing

	' Token: 0x040022D2 RID: 8914
	<SerializeField()>
	Private projectileRoot As Transform

	' Token: 0x040022D3 RID: 8915
	<SerializeField()>
	Private regularHorseshoe As ClownLevelHorseshoe

	' Token: 0x040022D4 RID: 8916
	<SerializeField()>
	Private pinkHorseshoe As ClownLevelHorseshoe

	' Token: 0x040022D5 RID: 8917
	<SerializeField()>
	Private spitFxPrefabA As Effect

	' Token: 0x040022D6 RID: 8918
	<SerializeField()>
	Private spitFxPrefabB As Effect

	' Token: 0x040022D7 RID: 8919
	<SerializeField()>
	Private spitFxRoot As Transform

	' Token: 0x040022D8 RID: 8920
	Private pos As Vector3

	' Token: 0x040022D9 RID: 8921
	Private startPos As Vector3

	' Token: 0x040022DA RID: 8922
	Private moveObject As Transform

	' Token: 0x040022DB RID: 8923
	Private damageDealer As DamageDealer

	' Token: 0x040022DC RID: 8924
	Private damageReceiver As DamageReceiver

	' Token: 0x040022DD RID: 8925
	Private horseTypePattern As String()

	' Token: 0x040022DE RID: 8926
	Private wavePositionPattern As String()

	' Token: 0x040022DF RID: 8927
	Private wavePinkPattern As String()

	' Token: 0x040022E0 RID: 8928
	Private dropPositionPattern As String()

	' Token: 0x040022E1 RID: 8929
	Private dropBulletPositionPattern As String()

	' Token: 0x040022E2 RID: 8930
	Private positionIndex As Integer

	' Token: 0x040022E3 RID: 8931
	Private pinkIndex As Integer

	' Token: 0x040022E4 RID: 8932
	Private dropBulletIndex As Integer

	' Token: 0x040022E5 RID: 8933
	Private horseTypeIndex As Integer

	' Token: 0x040022E6 RID: 8934
	Private dropMainIndex As Integer

	' Token: 0x040022E7 RID: 8935
	Private pinkMainIndex As Integer

	' Token: 0x040022E8 RID: 8936
	Private droppedClown As Boolean

	' Token: 0x040022E9 RID: 8937
	Private ScreamSFXPlaying As Boolean

	' Token: 0x0200055E RID: 1374
	Public Enum HorseType
		' Token: 0x040022EB RID: 8939
		Wave
		' Token: 0x040022EC RID: 8940
		Drop
		' Token: 0x040022ED RID: 8941
		Simple
	End Enum
End Class
