Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000607 RID: 1543
Public Class FlowerLevelEnemySeed
	Inherits AbstractProjectile

	' Token: 0x06001EBB RID: 7867 RVA: 0x0011AD34 File Offset: 0x00119134
	Public Sub OnSeedSpawn(properties As LevelProperties.Flower, parent As FlowerLevelFlower, type As Char, isActive As Boolean)
		Me.isActive = isActive
		Me.properties = properties
		Select Case type
			Case "A"c
				MyBase.animator.SetInteger("Type", 1)
			Case "B"c
				MyBase.animator.SetInteger("Type", 0)
			Case "C"c
				MyBase.animator.SetInteger("Type", 2)
				Me.SetParryable(True)
		End Select
		Me.fallingSpeed = properties.CurrentState.enemyPlants.fallingSeedSpeed
		Me.type = type
		Me.parent = parent
		AddHandler Me.parent.OnDeathEvent, AddressOf Me.KillSeed
	End Sub

	' Token: 0x06001EBC RID: 7868 RVA: 0x0011ADF3 File Offset: 0x001191F3
	Private Sub OnSeedLand()
		MyBase.StartCoroutine(Me.onSeedLand_cr())
	End Sub

	' Token: 0x06001EBD RID: 7869 RVA: 0x0011AE04 File Offset: 0x00119204
	Private Iterator Function onSeedLand_cr() As IEnumerator
		If Me.type = "B"c Then
			If Not Me.plantSpawned Then
				MyBase.animator.Play("Chomper_Landing")
				Yield MyBase.animator.WaitForAnimationToEnd(Me, "Chomper_Landing", False, True)
				If Me.isActive Then
					Dim gameObject As GameObject = Global.UnityEngine.[Object].Instantiate(Of GameObject)(Me.chomperSpawn)
					gameObject.transform.position = MyBase.transform.position
					gameObject.GetComponent(Of FlowerLevelChomperSeed)().OnChomperStart(Me.parent, Me.properties.CurrentState.enemyPlants)
					Me.plantSpawned = True
					MyBase.gameObject.SetActive(False)
				End If
			End If
		Else
			MyBase.animator.SetTrigger("Landed")
			Yield New WaitForEndOfFrame()
			Yield MyBase.animator.WaitForAnimationToEnd(Me, True)
			MyBase.animator.Play("Ground_Burst_Start")
		End If
		Return
	End Function

	' Token: 0x06001EBE RID: 7870 RVA: 0x0011AE1F File Offset: 0x0011921F
	Private Sub KillSeed()
		Me.isActive = False
	End Sub

	' Token: 0x06001EBF RID: 7871 RVA: 0x0011AE28 File Offset: 0x00119228
	Protected Overrides Sub Awake()
		Me.isActive = True
		MyBase.transform.localScale = New Vector3(MyBase.transform.localScale.x * CSng(MathUtils.PlusOrMinus()), MyBase.transform.localScale.y, MyBase.transform.localScale.z)
		MyBase.Awake()
	End Sub

	' Token: 0x06001EC0 RID: 7872 RVA: 0x0011AE92 File Offset: 0x00119292
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x06001EC1 RID: 7873 RVA: 0x0011AEB0 File Offset: 0x001192B0
	Protected Overrides Sub Update()
		MyBase.Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x06001EC2 RID: 7874 RVA: 0x0011AECE File Offset: 0x001192CE
	Protected Overrides Sub FixedUpdate()
		MyBase.transform.position += -Vector3.up * (CSng(Me.fallingSpeed) * CupheadTime.FixedDelta)
		MyBase.FixedUpdate()
	End Sub

	' Token: 0x06001EC3 RID: 7875 RVA: 0x0011AF08 File Offset: 0x00119308
	Protected Overrides Sub OnCollisionGround(hit As GameObject, phase As CollisionPhase)
		If Not Me.plantSpawned Then
			If Me.isActive Then
				Me.OnSeedLand()
			ElseIf Me.type = "C"c Then
				Me.type = "A"c
				Me.OnSeedLand()
			Else
				Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
			End If
			Me.fallingSpeed = 0
			If MyBase.CanParry Then
				Me.SetParryable(False)
			End If
			Me.plantSpawned = True
		End If
		MyBase.OnCollisionGround(hit, phase)
	End Sub

	' Token: 0x06001EC4 RID: 7876 RVA: 0x0011AF8C File Offset: 0x0011938C
	Protected Overrides Sub OnCollisionEnemy(hit As GameObject, phase As CollisionPhase)
		If hit.GetComponent(Of FlowerLevelFlowerDamageRegion)() IsNot Nothing Then
			Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
		Else
			Select Case Me.type
				Case "A"c
					If hit.GetComponent(Of FlowerLevelVenusSpawn)() IsNot Nothing Then
						Me.isActive = False
					End If
				Case "B"c
					If hit.GetComponent(Of FlowerLevelChomperSeed)() IsNot Nothing Then
						Me.isActive = False
					End If
				Case "C"c
					If hit.GetComponent(Of FlowerLevelMiniFlowerSpawn)() IsNot Nothing Then
						Me.isActive = False
					End If
			End Select
		End If
		MyBase.OnCollisionEnemyProjectile(hit, phase)
	End Sub

	' Token: 0x06001EC5 RID: 7877 RVA: 0x0011B03F File Offset: 0x0011943F
	Protected Overrides Sub Die()
		MyBase.Die()
		MyBase.GetComponent(Of Collider2D)().enabled = False
		Me.StopAllCoroutines()
		Me.parent.OnMiniFlowerDeath()
	End Sub

	' Token: 0x06001EC6 RID: 7878 RVA: 0x0011B064 File Offset: 0x00119464
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		RemoveHandler Me.parent.OnDeathEvent, AddressOf Me.KillSeed
		Me.homingVenusFlyTrapSpawn = Nothing
		Me.chomperSpawn = Nothing
		Me.miniFlowerSpawn = Nothing
	End Sub

	' Token: 0x06001EC7 RID: 7879 RVA: 0x0011B098 File Offset: 0x00119498
	Private Sub OnSpawnPlant()
		Dim c As Char = Me.type
		If c <> "A"c Then
			If c = "C"c Then
				Dim gameObject As GameObject = Global.UnityEngine.[Object].Instantiate(Of GameObject)(Me.miniFlowerSpawn)
				gameObject.transform.position = Me.spawnPoint.transform.position
				gameObject.GetComponent(Of FlowerLevelMiniFlowerSpawn)().OnMiniFlowerSpawn(Me.parent, Me.properties.CurrentState.enemyPlants)
				gameObject.transform.localScale = MyBase.transform.localScale
			End If
		Else
			Dim gameObject As GameObject = Global.UnityEngine.[Object].Instantiate(Of GameObject)(Me.homingVenusFlyTrapSpawn)
			gameObject.transform.position = Me.spawnPoint.transform.position
			gameObject.GetComponent(Of FlowerLevelVenusSpawn)().OnVenusSpawn(Me.parent, Me.properties.CurrentState.enemyPlants.venusPlantHP, CSng(Me.properties.CurrentState.enemyPlants.venusTurningSpeed), Me.properties.CurrentState.enemyPlants.venusMovmentSpeed, Me.properties.CurrentState.enemyPlants.venusTurningDelay)
			gameObject.transform.localScale = MyBase.transform.localScale
		End If
		Me.plantSpawned = True
	End Sub

	' Token: 0x06001EC8 RID: 7880 RVA: 0x0011B1D8 File Offset: 0x001195D8
	Private Sub TriggerVine()
		MyBase.StartCoroutine(Me.triggerVine_cr())
	End Sub

	' Token: 0x06001EC9 RID: 7881 RVA: 0x0011B1E8 File Offset: 0x001195E8
	Private Iterator Function triggerVine_cr() As IEnumerator
		Yield New WaitForEndOfFrame()
		Yield New WaitForEndOfFrame()
		MyBase.animator.Play("Trigger_Vine", 1)
		Return
	End Function

	' Token: 0x06001ECA RID: 7882 RVA: 0x0011B203 File Offset: 0x00119603
	Private Sub OnDeath()
		If Me.plantSpawned Then
			Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
		End If
	End Sub

	' Token: 0x06001ECB RID: 7883 RVA: 0x0011B21B File Offset: 0x0011961B
	Private Sub GroundPopAudio()
		AudioManager.Play("flower_vine_groundburst_start")
		Me.emitAudioFromObject.Add("flower_vine_groundburst_start")
	End Sub

	' Token: 0x06001ECC RID: 7884 RVA: 0x0011B237 File Offset: 0x00119637
	Private Sub VineGrowLargeAudio()
		AudioManager.Play("flower_venus_vine_grow_large")
		Me.emitAudioFromObject.Add("flower_venus_vine_grow_large")
	End Sub

	' Token: 0x06001ECD RID: 7885 RVA: 0x0011B253 File Offset: 0x00119653
	Private Sub VineGrowMediumAudio()
		AudioManager.Play("flower_venus_vine_grow_medium")
		Me.emitAudioFromObject.Add("flower_venus_vine_grow_medium")
	End Sub

	' Token: 0x06001ECE RID: 7886 RVA: 0x0011B26F File Offset: 0x0011966F
	Private Sub VineGrowSmallAudio()
		AudioManager.Play("flower_venus_vine_grow_small")
		Me.emitAudioFromObject.Add("flower_venus_vine_grow_small")
	End Sub

	' Token: 0x04002782 RID: 10114
	Private fallingSpeed As Integer

	' Token: 0x04002783 RID: 10115
	Private type As Char

	' Token: 0x04002784 RID: 10116
	Private isActive As Boolean

	' Token: 0x04002785 RID: 10117
	Private plantSpawned As Boolean

	' Token: 0x04002786 RID: 10118
	Private properties As LevelProperties.Flower

	' Token: 0x04002787 RID: 10119
	Private parent As FlowerLevelFlower

	' Token: 0x04002788 RID: 10120
	<SerializeField()>
	Private spawnPoint As GameObject

	' Token: 0x04002789 RID: 10121
	<Space(10F)>
	<Header("Venus Fly Trap")>
	<SerializeField()>
	Private venusSeedTex As Sprite

	' Token: 0x0400278A RID: 10122
	<SerializeField()>
	Private homingVenusFlyTrapSpawn As GameObject

	' Token: 0x0400278B RID: 10123
	<Space(10F)>
	<Header("Chomper")>
	<SerializeField()>
	Private chomperSeedTex As Sprite

	' Token: 0x0400278C RID: 10124
	<SerializeField()>
	Private chomperSpawn As GameObject

	' Token: 0x0400278D RID: 10125
	<Space(10F)>
	<Header("Mini Flower")>
	<SerializeField()>
	Private miniFlowerSeedTex As Sprite

	' Token: 0x0400278E RID: 10126
	<SerializeField()>
	Private miniFlowerSpawn As GameObject
End Class
