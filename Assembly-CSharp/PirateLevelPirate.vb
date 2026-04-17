Imports System
Imports UnityEngine

' Token: 0x0200072F RID: 1839
Public Class PirateLevelPirate
	Inherits LevelProperties.Pirate.Entity

	' Token: 0x0600280A RID: 10250 RVA: 0x001763D0 File Offset: 0x001747D0
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Dim pirateLevel As PirateLevel = TryCast(Level.Current, PirateLevel)
		AddHandler pirateLevel.OnWhistleEvent, AddressOf Me.onWhistle
	End Sub

	' Token: 0x0600280B RID: 10251 RVA: 0x00176400 File Offset: 0x00174800
	Public Overrides Sub LevelInit(properties As LevelProperties.Pirate)
		MyBase.LevelInit(properties)
		AddHandler MyBase.GetComponent(Of DamageReceiver)().OnDamageTaken, AddressOf Me.OnDamageTaken
		AddHandler Level.Current.OnIntroEvent, AddressOf Me.OnIntroLaugh
		AddHandler properties.OnBossDeath, AddressOf Me.OnBossDeath
	End Sub

	' Token: 0x0600280C RID: 10252 RVA: 0x00176453 File Offset: 0x00174853
	Private Sub OnIntroLaugh()
		MyBase.animator.SetTrigger("OnLaugh")
	End Sub

	' Token: 0x0600280D RID: 10253 RVA: 0x00176465 File Offset: 0x00174865
	Private Sub onWhistle(creature As PirateLevel.Creature)
		Me.whistles = 0
		Me.creature = creature
		MyBase.animator.SetTrigger("OnWhistle")
		Me.loops = 1000
	End Sub

	' Token: 0x0600280E RID: 10254 RVA: 0x00176490 File Offset: 0x00174890
	Private Sub OnIdleEnd()
		If Me.loops >= Me.max Then
			Dim num As Integer = Global.UnityEngine.Random.Range(0, 100)
			Dim num2 As Integer = 0
			If num <= Me.bothChance Then
				num2 = 2
			ElseIf num <= Me.patchChance + Me.bothChance Then
				num2 = 1
			End If
			MyBase.animator.SetInteger("Blink", num2)
			MyBase.animator.SetTrigger("OnBlink")
			Return
		End If
		Me.loops += 1
	End Sub

	' Token: 0x0600280F RID: 10255 RVA: 0x00176511 File Offset: 0x00174911
	Private Sub OnBlink()
		Me.max = Global.UnityEngine.Random.Range(2, 5)
		Me.loops = 0
	End Sub

	' Token: 0x06002810 RID: 10256 RVA: 0x00176527 File Offset: 0x00174927
	Private Sub OnBossDeath()
		Me.StopAllCoroutines()
		MyBase.animator.SetTrigger("OnDeath")
		AudioManager.Play("level_pirate_fall_death")
	End Sub

	' Token: 0x06002811 RID: 10257 RVA: 0x00176549 File Offset: 0x00174949
	Public Sub FireGun(properties As LevelProperties.Pirate.Peashot)
		MyBase.animator.Play("Gun_Shoot")
	End Sub

	' Token: 0x06002812 RID: 10258 RVA: 0x0017655C File Offset: 0x0017495C
	Private Sub Whistle()
		Dim num As Integer = 1
		Dim creature As PirateLevel.Creature = Me.creature
		If creature <> PirateLevel.Creature.DogFish Then
			If creature = PirateLevel.Creature.Shark Then
				num = 3
			End If
		Else
			num = 2
		End If
		If Me.whistles >= num Then
			Return
		End If
		Me.whistleEffect.Create(Me.whistleRoot.position)
		Me.whistles += 1
	End Sub

	' Token: 0x06002813 RID: 10259 RVA: 0x001765C5 File Offset: 0x001749C5
	Private Sub WhistleSFX()
		AudioManager.Play("levels_pirate_whistle")
		Me.emitAudioFromObject.Add("levels_pirate_whistle")
	End Sub

	' Token: 0x06002814 RID: 10260 RVA: 0x001765E1 File Offset: 0x001749E1
	Public Sub EndGun()
		MyBase.animator.SetTrigger("OnGunEnd")
	End Sub

	' Token: 0x06002815 RID: 10261 RVA: 0x001765F3 File Offset: 0x001749F3
	Private Sub PlayLaughSound()
		AudioManager.Play("levels_pirate_laugh")
		Me.emitAudioFromObject.Add("levels_pirate_laugh")
	End Sub

	' Token: 0x06002816 RID: 10262 RVA: 0x00176610 File Offset: 0x00174A10
	Public Sub StartGun()
		MyBase.animator.SetTrigger("OnGunStart")
		Me.gunProperties = MyBase.properties.CurrentState.peashot
		Me.shotIndex = Global.UnityEngine.Random.Range(0, Me.gunProperties.shotType.Split(New Char() { ","c }).Length)
	End Sub

	' Token: 0x06002817 RID: 10263 RVA: 0x0017666C File Offset: 0x00174A6C
	Private Sub Shoot()
		If PlayerManager.Count <= 0 Then
			Me.gunRoot.LookAt2D(New Vector2(0F, 0F))
			Return
		End If
		Me.gunRoot.LookAt2D(PlayerManager.GetNext().center)
		AudioManager.Play("level_pirate_gun_shoot")
		Me.emitAudioFromObject.Add("level_pirate_gun_shoot")
		Me.muzzleFlash.Create(Me.gunRoot.position)
		Dim basicProjectile As BasicProjectile = Nothing
		If Me.gunProperties.shotType.Split(New Char() { ","c })(Me.shotIndex)(0) = "P"c Then
			basicProjectile = Me.gunProjectile.Create(Me.gunRoot.position, Me.gunRoot.eulerAngles.z, New Vector3(-1F, -1F, 1F), Me.gunProperties.speed)
			basicProjectile.SetParryable(True)
		ElseIf Me.gunProperties.shotType.Split(New Char() { ","c })(Me.shotIndex)(0) = "R"c Then
			basicProjectile = Me.gunProjectileRegular.Create(Me.gunRoot.position, Me.gunRoot.eulerAngles.z, New Vector3(-1F, -1F, 1F), Me.gunProperties.speed)
		End If
		basicProjectile.CollisionDeath.OnlyBounds()
		basicProjectile.CollisionDeath.Player = True
		Me.shotIndex = (Me.shotIndex + 1) Mod Me.gunProperties.shotType.Split(New Char() { ","c }).Length
	End Sub

	' Token: 0x06002818 RID: 10264 RVA: 0x0017683F File Offset: 0x00174C3F
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		If MyBase.properties.CurrentState.stateName = LevelProperties.Pirate.States.Boat Then
			Return
		End If
		MyBase.properties.DealDamage(info.damage)
	End Sub

	' Token: 0x06002819 RID: 10265 RVA: 0x00176869 File Offset: 0x00174C69
	Public Sub CleanUp()
		RemoveHandler MyBase.properties.OnBossDeath, AddressOf Me.OnBossDeath
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub

	' Token: 0x0600281A RID: 10266 RVA: 0x0017688D File Offset: 0x00174C8D
	Private Sub SoundGunStart()
		AudioManager.Play("level_pirate_gun_start")
		Me.emitAudioFromObject.Add("level_pirate_gun_start")
	End Sub

	' Token: 0x0600281B RID: 10267 RVA: 0x001768A9 File Offset: 0x00174CA9
	Private Sub SoundGunEnd()
		AudioManager.Play("level_pirate_gun_end")
		Me.emitAudioFromObject.Add("level_pirate_gun_end")
	End Sub

	' Token: 0x0600281C RID: 10268 RVA: 0x001768C5 File Offset: 0x00174CC5
	Private Sub SoundPirateFoot()
		AudioManager.Play("level_pirate_pirate_foot")
		Me.emitAudioFromObject.Add("level_pirate_pirate_foot")
	End Sub

	' Token: 0x0600281D RID: 10269 RVA: 0x001768E1 File Offset: 0x00174CE1
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me.gunProjectile = Nothing
		Me.gunProjectileRegular = Nothing
		Me.muzzleFlash = Nothing
		Me.whistleEffect = Nothing
	End Sub

	' Token: 0x040030CF RID: 12495
	Private Const MIN_IDLE_LOOPS As Integer = 2

	' Token: 0x040030D0 RID: 12496
	Private Const MAX_IDLE_LOOPS As Integer = 4

	' Token: 0x040030D1 RID: 12497
	<SerializeField()>
	Private gunRoot As Transform

	' Token: 0x040030D2 RID: 12498
	<SerializeField()>
	Private gunProjectile As BasicProjectile

	' Token: 0x040030D3 RID: 12499
	<SerializeField()>
	Private gunProjectileRegular As BasicProjectile

	' Token: 0x040030D4 RID: 12500
	<SerializeField()>
	Private muzzleFlash As Effect

	' Token: 0x040030D5 RID: 12501
	<SerializeField()>
	Private whistleRoot As Transform

	' Token: 0x040030D6 RID: 12502
	<SerializeField()>
	Private whistleEffect As Effect

	' Token: 0x040030D7 RID: 12503
	Private gunProperties As LevelProperties.Pirate.Peashot

	' Token: 0x040030D8 RID: 12504
	Private creature As PirateLevel.Creature

	' Token: 0x040030D9 RID: 12505
	Private whistles As Integer

	' Token: 0x040030DA RID: 12506
	Private patchChance As Integer = 25

	' Token: 0x040030DB RID: 12507
	Private bothChance As Integer = 15

	' Token: 0x040030DC RID: 12508
	Private loops As Integer

	' Token: 0x040030DD RID: 12509
	Private max As Integer = 2

	' Token: 0x040030DE RID: 12510
	Private shotIndex As Integer
End Class
