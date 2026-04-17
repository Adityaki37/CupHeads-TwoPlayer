Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports UnityEngine

' Token: 0x020008A4 RID: 2212
Public Class CircusPlatformingLevelHotdog
	Inherits AbstractPlatformingLevelEnemy

	' Token: 0x17000444 RID: 1092
	' (get) Token: 0x06003377 RID: 13175 RVA: 0x001DEF62 File Offset: 0x001DD362
	' (set) Token: 0x06003378 RID: 13176 RVA: 0x001DEF6C File Offset: 0x001DD36C
	Public Property ProjectilesCanHit As Boolean
		Get
			Return Me.projectilesCanHit
		End Get
		Set(value As Boolean)
			Me.projectilesCanHit = value
			For i As Integer = 0 To Me.projectileList.Count - 1
				Me.projectileList(i).EnableCollider(Me.projectilesCanHit)
			Next
			MyBase.animator.Play("Dance")
		End Set
	End Property

	' Token: 0x06003379 RID: 13177 RVA: 0x001DEFC3 File Offset: 0x001DD3C3
	Protected Overrides Sub OnStart()
	End Sub

	' Token: 0x0600337A RID: 13178 RVA: 0x001DEFC8 File Offset: 0x001DD3C8
	Protected Overrides Sub Start()
		MyBase.Start()
		Me.spawnPattern = Me.spawnPatternString.Split(New Char() { ","c })
		Me.condimentPattern = Me.condimentPatternString.Split(New Char() { ","c })
		Me.sidePattern = Me.sidePatternString.Split(New Char() { ","c })
		Me.shotDelayPattern = Me.shotDelayPatternString.Split(New Char() { ","c })
		Me.spawnIndex = Global.UnityEngine.Random.Range(0, Me.spawnPattern.Length)
		Me.condimentIndex = Global.UnityEngine.Random.Range(0, Me.condimentPattern.Length)
		Me.sideIndex = Global.UnityEngine.Random.Range(0, Me.sidePattern.Length)
		Me.shotDelayIndex = Global.UnityEngine.Random.Range(0, Me.shotDelayPattern.Length)
		Me.currentDelay = Parser.IntParse(Me.shotDelayPattern(Me.shotDelayIndex))
	End Sub

	' Token: 0x0600337B RID: 13179 RVA: 0x001DF0B4 File Offset: 0x001DD4B4
	Public Sub ShootProjectile()
		Me.currentDelay -= 1
		If Me.currentDelay <= 0 Then
			Me.shotDelayIndex = (Me.shotDelayIndex + 1) Mod Me.shotDelayPattern.Length
			Me.currentDelay = Parser.IntParse(Me.shotDelayPattern(Me.shotDelayIndex))
			Dim text As String = Me.sidePattern(Me.sideIndex)
			Dim flag As Boolean = text = "R"
			Dim num As Integer = Parser.IntParse(Me.spawnPattern(Me.spawnIndex))
			If flag Then
				num += Me.projectilesSpawnPoints.Length / 2
			End If
			AudioManager.Play("circus_hotdog_projectile_shoot")
			Me.emitAudioFromObject.Add("circus_hotdog_projectile_shoot")
			Dim circusPlatformingLevelHotdogProjectile As CircusPlatformingLevelHotdogProjectile = TryCast(Me.projectilePrefab.Create(Me.projectilesSpawnPoints(num).position), CircusPlatformingLevelHotdogProjectile)
			circusPlatformingLevelHotdogProjectile.Speed = -MyBase.Properties.ProjectileSpeed
			circusPlatformingLevelHotdogProjectile.SetCondiment(Me.condimentPattern(Me.condimentIndex))
			circusPlatformingLevelHotdogProjectile.Side(flag)
			circusPlatformingLevelHotdogProjectile.DestroyDistance = Me.projectileDistance
			Me.projectileList.Add(circusPlatformingLevelHotdogProjectile)
			AddHandler circusPlatformingLevelHotdogProjectile.OnDestroyCallback, AddressOf Me.HotDogProjectileDie
			circusPlatformingLevelHotdogProjectile.EnableCollider(Me.projectilesCanHit)
			Me.spawnIndex = (Me.spawnIndex + 1) Mod Me.spawnPattern.Length
			Me.condimentIndex = (Me.condimentIndex + 1) Mod Me.condimentPattern.Length
			Me.sideIndex = (Me.sideIndex + 1) Mod Me.sidePattern.Length
		End If
	End Sub

	' Token: 0x0600337C RID: 13180 RVA: 0x001DF22E File Offset: 0x001DD62E
	Private Sub HotDogProjectileDie(obj As CircusPlatformingLevelHotdogProjectile)
		RemoveHandler obj.OnDestroyCallback, AddressOf Me.HotDogProjectileDie
		Me.projectileList.Remove(obj)
	End Sub

	' Token: 0x0600337D RID: 13181 RVA: 0x001DF24F File Offset: 0x001DD64F
	Protected Overrides Sub Die()
		MyBase.animator.SetTrigger("Death")
		MyBase.StartCoroutine(Me.Explosion_cr())
		MyBase.GetComponent(Of BoxCollider2D)().enabled = False
	End Sub

	' Token: 0x0600337E RID: 13182 RVA: 0x001DF27C File Offset: 0x001DD67C
	Private Iterator Function Explosion_cr() As IEnumerator
		Me.exploder.StartExplosion()
		Yield New WaitForSeconds(2.5F)
		Me.exploder.StopExplosions()
		Return
	End Function

	' Token: 0x0600337F RID: 13183 RVA: 0x001DF297 File Offset: 0x001DD697
	Public Sub DeathAnimationEnd()
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub

	' Token: 0x06003380 RID: 13184 RVA: 0x001DF2A4 File Offset: 0x001DD6A4
	Private Sub HotDogDanceSFX()
		AudioManager.Play("circus_hotdog_dance")
		Me.emitAudioFromObject.Add("circus_hotdog_dance")
	End Sub

	' Token: 0x06003381 RID: 13185 RVA: 0x001DF2C0 File Offset: 0x001DD6C0
	Private Sub HotDogDeathSFX()
		AudioManager.[Stop]("circus_hotdog_dance")
		AudioManager.Play("circus_hotdog_death")
		Me.emitAudioFromObject.Add("circus_hotdog_death")
	End Sub

	' Token: 0x06003382 RID: 13186 RVA: 0x001DF2E6 File Offset: 0x001DD6E6
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me.projectilePrefab = Nothing
	End Sub

	' Token: 0x04003BC4 RID: 15300
	Private Const DeathParameterName As String = "Death"

	' Token: 0x04003BC5 RID: 15301
	Private Const Right As String = "R"

	' Token: 0x04003BC6 RID: 15302
	<SerializeField()>
	Private projectilesSpawnPoints As Transform()

	' Token: 0x04003BC7 RID: 15303
	<SerializeField()>
	Private spawnPatternString As String

	' Token: 0x04003BC8 RID: 15304
	<SerializeField()>
	Private condimentPatternString As String

	' Token: 0x04003BC9 RID: 15305
	<SerializeField()>
	Private sidePatternString As String

	' Token: 0x04003BCA RID: 15306
	<SerializeField()>
	Private shotDelayPatternString As String

	' Token: 0x04003BCB RID: 15307
	<SerializeField()>
	Private projectileDistance As Single

	' Token: 0x04003BCC RID: 15308
	<SerializeField()>
	Private projectilePrefab As BasicProjectile

	' Token: 0x04003BCD RID: 15309
	<SerializeField()>
	Private exploder As LevelBossDeathExploder

	' Token: 0x04003BCE RID: 15310
	Private spawnPattern As String()

	' Token: 0x04003BCF RID: 15311
	Private condimentPattern As String()

	' Token: 0x04003BD0 RID: 15312
	Private sidePattern As String()

	' Token: 0x04003BD1 RID: 15313
	Private shotDelayPattern As String()

	' Token: 0x04003BD2 RID: 15314
	Private spawnIndex As Integer

	' Token: 0x04003BD3 RID: 15315
	Private condimentIndex As Integer

	' Token: 0x04003BD4 RID: 15316
	Private sideIndex As Integer

	' Token: 0x04003BD5 RID: 15317
	Private shotDelayIndex As Integer

	' Token: 0x04003BD6 RID: 15318
	Private currentDelay As Integer

	' Token: 0x04003BD7 RID: 15319
	Private projectileList As List(Of CircusPlatformingLevelHotdogProjectile) = New List(Of CircusPlatformingLevelHotdogProjectile)()

	' Token: 0x04003BD8 RID: 15320
	Private projectilesCanHit As Boolean
End Class
