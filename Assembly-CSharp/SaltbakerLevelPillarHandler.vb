Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports UnityEngine

' Token: 0x020007D3 RID: 2003
Public Class SaltbakerLevelPillarHandler
	Inherits LevelProperties.Saltbaker.Entity

	' Token: 0x06002D78 RID: 11640 RVA: 0x001ACC9E File Offset: 0x001AB09E
	Public Sub TakeDamage(info As DamageDealer.DamageInfo)
		MyBase.properties.DealDamage(info.damage)
	End Sub

	' Token: 0x06002D79 RID: 11641 RVA: 0x001ACCB4 File Offset: 0x001AB0B4
	Public Sub StartPlatforms()
		Me.leftPillar.transform.position = New Vector3(MyBase.transform.position.x - MyBase.properties.CurrentState.doomPillar.pillarPosition.min, MyBase.transform.position.y)
		Me.rightPillar.transform.position = New Vector3(MyBase.transform.position.x + MyBase.properties.CurrentState.doomPillar.pillarPosition.min, MyBase.transform.position.y)
		MyBase.StartCoroutine(Me.create_platforms_cr())
		MyBase.StartCoroutine(Me.create_glass_cr())
	End Sub

	' Token: 0x06002D7A RID: 11642 RVA: 0x001ACD88 File Offset: 0x001AB188
	Public Sub StartPillarOfDoom()
		Dim doomPillar As LevelProperties.Saltbaker.DoomPillar = MyBase.properties.CurrentState.doomPillar
		AddHandler MyBase.properties.OnBossDeath, AddressOf Me.Die
		Me.leftAnimator.Play("IntroA", 0, 0F)
		Me.rightAnimator.Play("IntroB", 0, 0F)
		Me.SFX_SALTB_P4_TornadoPillars_Loop()
		MyBase.StartCoroutine(Me.move_horizontal_cr())
	End Sub

	' Token: 0x06002D7B RID: 11643 RVA: 0x001ACDFC File Offset: 0x001AB1FC
	Public Sub StartHeart()
		Me.darkHeart.gameObject.SetActive(True)
		Me.darkHeart.Init(Vector3.up * 500F, Me.leftPillar, Me.rightPillar, MyBase.properties.CurrentState.darkHeart, Me)
	End Sub

	' Token: 0x06002D7C RID: 11644 RVA: 0x001ACE54 File Offset: 0x001AB254
	Public Function GetPlatformFallSpeed() As Single
		Return Mathf.Lerp(MyBase.properties.CurrentState.doomPillar.platformFallSpeed.min, MyBase.properties.CurrentState.doomPillar.platformFallSpeed.max, Mathf.InverseLerp(0F, MyBase.properties.CurrentState.doomPillar.phaseTime, Me.phaseTimer))
	End Function

	' Token: 0x06002D7D RID: 11645 RVA: 0x001ACEC0 File Offset: 0x001AB2C0
	Private Iterator Function create_glass_cr() As IEnumerator
		Me.chunkOrderString = New PatternString(Me.chunkOrder, True)
		Me.chunkPositionString = New PatternString(Me.chunkPosition, True)
		Me.chunkSpawnTimeString = New PatternString(Me.chunkSpawnTime, True)
		While True
			Dim t As Single = Me.chunkSpawnTimeString.PopFloat()
			While t > 0F
				t -= CupheadTime.Delta * Mathf.Lerp(0.5F, 1F, Mathf.InverseLerp(0F, MyBase.properties.CurrentState.doomPillar.phaseTime, Me.phaseTimer))
				Yield Nothing
			End While
			Dim usableChunk As Integer = -1
			For i As Integer = 0 To Me.chunks.Count - 1
				If Me.chunks(i).transform.position.y < -520F Then
					usableChunk = i
					Exit For
				End If
			Next
			If usableChunk = -1 Then
				Me.chunks.Add(Global.UnityEngine.[Object].Instantiate(Of SaltbakerLevelGlassChunk)(Me.chunkPrefab))
				usableChunk = Me.chunks.Count - 1
			End If
			Dim xPos As Single = Mathf.Lerp(CSng(Level.Current.Left), CSng(Level.Current.Right), Me.chunkPositionString.PopFloat())
			Dim id As Integer = Me.chunkOrderString.PopInt()
			Dim inBack As Boolean = Rand.Bool()
			Dim fallSpeed As Single = Me.GetPlatformFallSpeed() + CSng(If((Not inBack), Global.UnityEngine.Random.Range(100, 200), Global.UnityEngine.Random.Range(-100, -75)))
			Me.chunks(usableChunk).Reset(New Vector3(xPos, 520F), fallSpeed, id < 4, Me.chunkFlip(id), Rand.Bool(), inBack, id Mod 4)
			Me.chunkFlip(id) = Not Me.chunkFlip(id)
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06002D7E RID: 11646 RVA: 0x001ACEDC File Offset: 0x001AB2DC
	Private Iterator Function create_platforms_cr() As IEnumerator
		Dim bigCounter As Integer = 0
		Dim mediumCounter As Integer = 0
		Dim smallCounter As Integer = 0
		Dim pillarSpawnOffset As Single = 100F
		Dim p As LevelProperties.Saltbaker.DoomPillar = MyBase.properties.CurrentState.doomPillar
		Dim pillarXBuffer As Single = 146F + Me.leftPillar.GetComponent(Of BoxCollider2D)().size.x / 2F
		Dim wait As YieldInstruction = New WaitForFixedUpdate()
		Dim platformSize As PatternString = New PatternString(p.platformSizeString, True, True)
		Dim platformPosX As PatternString = New PatternString(p.platformXSpawnString, True, True)
		Dim platformPosY As PatternString = New PatternString(p.platformYSpawnString, True, True)
		If p.platformXYUnified Then
			platformPosY.SetMainStringIndex(platformPosX.GetMainStringIndex())
			platformPosY.SetSubStringIndex(platformPosX.GetSubStringIndex())
			If platformPosX.SubStringLength() <> platformPosY.SubStringLength() Then
				Global.Debug.Break()
			End If
			platformPosY.PopFloat()
		End If
		Dim spawnDistance As Single = 0F
		While True
			Dim spawnPos As Vector3 = New Vector3(Mathf.Lerp(Me.leftPillar.transform.position.x + pillarXBuffer, Me.rightPillar.transform.position.x - pillarXBuffer, (platformPosX.PopFloat() + 1F) / 2F), CSng(Level.Current.Ceiling) + pillarSpawnOffset + spawnDistance)
			If Me.suppressCenterPlatforms Then
				spawnPos = New Vector3(If((Not Rand.Bool()), (Me.rightPillar.transform.position.x - pillarXBuffer), (Me.leftPillar.transform.position.x + pillarXBuffer)), spawnPos.y)
			End If
			spawnDistance = platformPosY.PopFloat()
			Dim platform As GameObject = Nothing
			Dim c As Char = platformSize.PopLetter()
			If c <> "L"c Then
				If c <> "M"c Then
					If c <> "S"c Then
						Global.Debug.LogError("Pattern string is incorrect.", Nothing)
						Global.Debug.Break()
					Else
						platform = Global.UnityEngine.[Object].Instantiate(Of GameObject)(Me.smallPlatform(smallCounter Mod 2))
						platform.transform.GetChild(0).localScale = New Vector3(CSng(If((smallCounter Mod 4 >= 2), (-1), 1)), 1F)
						smallCounter += 1
					End If
				Else
					platform = Global.UnityEngine.[Object].Instantiate(Of GameObject)(Me.medPlatform(mediumCounter Mod 2))
					platform.transform.GetChild(0).localScale = New Vector3(CSng(If((mediumCounter Mod 4 >= 2), (-1), 1)), 1F)
					mediumCounter += 1
				End If
			Else
				platform = Global.UnityEngine.[Object].Instantiate(Of GameObject)(Me.bigPlatform(bigCounter Mod 2))
				platform.transform.GetChild(0).localScale = New Vector3(CSng(If((bigCounter Mod 4 >= 2), (-1), 1)), 1F)
				bigCounter += 1
			End If
			platform.transform.position = spawnPos
			Me.platforms.Add(platform.gameObject)
			While spawnDistance > 0F
				spawnDistance -= CupheadTime.FixedDelta * Me.GetPlatformFallSpeed()
				Yield wait
			End While
		End While
		Return
	End Function

	' Token: 0x06002D7F RID: 11647 RVA: 0x001ACEF8 File Offset: 0x001AB2F8
	Private Iterator Function move_horizontal_cr() As IEnumerator
		Dim p As LevelProperties.Saltbaker.DoomPillar = MyBase.properties.CurrentState.doomPillar
		Dim wait As YieldInstruction = New WaitForFixedUpdate()
		While True
			Dim t As Single = Mathf.InverseLerp(0F, p.phaseTime, Me.phaseTimer)
			Me.leftPillar.transform.position = Vector3.Lerp(New Vector3(MyBase.transform.position.x - p.pillarPosition.min, MyBase.transform.position.y), New Vector3(MyBase.transform.position.x - p.pillarPosition.max, MyBase.transform.position.y), t)
			Me.rightPillar.transform.position = Vector3.Lerp(New Vector3(MyBase.transform.position.x + p.pillarPosition.min, MyBase.transform.position.y), New Vector3(MyBase.transform.position.x + p.pillarPosition.max, MyBase.transform.position.y), t)
			Me.phaseTimer += CupheadTime.FixedDelta
			Yield wait
		End While
		Return
	End Function

	' Token: 0x06002D80 RID: 11648 RVA: 0x001ACF14 File Offset: 0x001AB314
	Private Sub Update()
		Me.platforms.RemoveAll(Function(g As GameObject) g Is Nothing)
		For Each gameObject As GameObject In Me.platforms
			If gameObject.transform.position.y < CSng(Level.Current.Ground) - 400F Then
				Global.UnityEngine.[Object].Destroy(gameObject.gameObject)
			Else
				gameObject.transform.position += Vector3.down * Me.GetPlatformFallSpeed() * CupheadTime.Delta
			End If
		Next
		For Each abstractPlayerController As AbstractPlayerController In PlayerManager.GetAllPlayers()
			Dim levelPlayerController As LevelPlayerController = CType(abstractPlayerController, LevelPlayerController)
			If levelPlayerController Then
				levelPlayerController.animationController.spriteRenderer.sortingOrder = If((levelPlayerController.motor.MoveDirection.y <= 0), 10, 510)
			End If
		Next
	End Sub

	' Token: 0x06002D81 RID: 11649 RVA: 0x001AD090 File Offset: 0x001AB490
	Private Sub Die()
		Me.leftAnimator.Play("Death", -1, 0F)
		Me.rightAnimator.Play("Death", -1, 0.5F)
		Me.SFX_SALTB_P4_TornadoPillar_LoopStop()
		Me.darkHeart.Die()
	End Sub

	' Token: 0x06002D82 RID: 11650 RVA: 0x001AD0CF File Offset: 0x001AB4CF
	Private Sub SFX_SALTB_P4_TornadoPillars_Loop()
		AudioManager.PlayLoop("sfx_DLC_Saltbaker_P4_Tornado_Left_Loop")
		AudioManager.PlayLoop("sfx_DLC_Saltbaker_P4_Tornado_Right_Loop")
	End Sub

	' Token: 0x06002D83 RID: 11651 RVA: 0x001AD0E5 File Offset: 0x001AB4E5
	Private Sub SFX_SALTB_P4_TornadoPillar_LoopStop()
		AudioManager.[Stop]("sfx_DLC_Saltbaker_P4_Tornado_Left_Loop")
		AudioManager.[Stop]("sfx_DLC_Saltbaker_P4_Tornado_Right_Loop")
	End Sub

	' Token: 0x040035F9 RID: 13817
	<Header("Other")>
	<SerializeField()>
	Private leftPillar As GameObject

	' Token: 0x040035FA RID: 13818
	<SerializeField()>
	Private rightPillar As GameObject

	' Token: 0x040035FB RID: 13819
	<SerializeField()>
	Private leftAnimator As Animator

	' Token: 0x040035FC RID: 13820
	<SerializeField()>
	Private rightAnimator As Animator

	' Token: 0x040035FD RID: 13821
	<SerializeField()>
	Private darkHeart As SaltbakerLevelHeart

	' Token: 0x040035FE RID: 13822
	<SerializeField()>
	Private smallPlatform As GameObject()

	' Token: 0x040035FF RID: 13823
	<SerializeField()>
	Private medPlatform As GameObject()

	' Token: 0x04003600 RID: 13824
	<SerializeField()>
	Private bigPlatform As GameObject()

	' Token: 0x04003601 RID: 13825
	Private platforms As List(Of GameObject) = New List(Of GameObject)()

	' Token: 0x04003602 RID: 13826
	<SerializeField()>
	Private chunkPrefab As SaltbakerLevelGlassChunk

	' Token: 0x04003603 RID: 13827
	Private chunks As List(Of SaltbakerLevelGlassChunk) = New List(Of SaltbakerLevelGlassChunk)()

	' Token: 0x04003604 RID: 13828
	<SerializeField()>
	Private chunkOrder As String

	' Token: 0x04003605 RID: 13829
	Private chunkOrderString As PatternString

	' Token: 0x04003606 RID: 13830
	<SerializeField()>
	Private chunkPosition As String

	' Token: 0x04003607 RID: 13831
	Private chunkPositionString As PatternString

	' Token: 0x04003608 RID: 13832
	<SerializeField()>
	Private chunkSpawnTime As String

	' Token: 0x04003609 RID: 13833
	Private chunkSpawnTimeString As PatternString

	' Token: 0x0400360A RID: 13834
	Private chunkFlip As Boolean() = New Boolean(7) {}

	' Token: 0x0400360B RID: 13835
	Private phaseTimer As Single

	' Token: 0x0400360C RID: 13836
	Public suppressCenterPlatforms As Boolean
End Class
