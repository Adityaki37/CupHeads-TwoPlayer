Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports UnityEngine

' Token: 0x020005A1 RID: 1441
Public Class DicePalaceBoozeLevelMartini
	Inherits DicePalaceBoozeLevelBossBase

	' Token: 0x06001BAA RID: 7082 RVA: 0x000FC284 File Offset: 0x000FA684
	Protected Overrides Sub Awake()
		Me.olives = New List(Of DicePalaceBoozeLevelOlive)()
		Me.damageDealer = DamageDealer.NewEnemy()
		Me.damageReceiver = MyBase.GetComponent(Of DamageReceiver)()
		AddHandler Me.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
		MyBase.Awake()
	End Sub

	' Token: 0x06001BAB RID: 7083 RVA: 0x000FC2D0 File Offset: 0x000FA6D0
	Private Sub Update()
		Me.damageDealer.Update()
	End Sub

	' Token: 0x06001BAC RID: 7084 RVA: 0x000FC2E0 File Offset: 0x000FA6E0
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		Dim health As Single = Me.health
		Me.health -= info.damage
		If health > 0F Then
			Level.Current.timeline.DealDamage(Mathf.Clamp(health - Me.health, 0F, health))
		End If
		If Me.health < 0F AndAlso Not MyBase.isDead Then
			Me.StartDying()
			Me.MartiniDeathSFX()
		End If
	End Sub

	' Token: 0x06001BAD RID: 7085 RVA: 0x000FC35C File Offset: 0x000FA75C
	Public Overrides Sub LevelInit(properties As LevelProperties.DicePalaceBooze)
		Me.activeOlives = 0
		Me.pinkShotIndex = Global.UnityEngine.Random.Range(0, properties.CurrentState.martini.pinkString.Split(New Char() { ","c }).Length)
		Dim num As Integer = properties.CurrentState.martini.olivePositionStringX.Length
		Dim num2 As Integer = Global.UnityEngine.Random.Range(0, num)
		Dim num3 As Integer = Global.UnityEngine.Random.Range(0, num)
		For i As Integer = 0 To num - 1
			Dim gameObject As GameObject = Global.UnityEngine.[Object].Instantiate(Of GameObject)(Me.olive.gameObject, Me.spawnPoint.position, Quaternion.identity)
			gameObject.GetComponent(Of DicePalaceBoozeLevelOlive)().InitOlive(properties, Parser.IntParse(properties.CurrentState.martini.pinkString.Split(New Char() { ","c })(Me.pinkShotIndex)), properties.CurrentState.martini.olivePositionStringY(num3), properties.CurrentState.martini.olivePositionStringX(num2))
			Me.pinkShotIndex += 1
			If Me.pinkShotIndex >= properties.CurrentState.martini.pinkString.Split(New Char() { ","c }).Length Then
				Me.pinkShotIndex = 0
			End If
			num3 += 1
			If num3 >= num Then
				num3 = 0
			End If
			num2 += 1
			If num2 >= num Then
				num2 = 0
			End If
			gameObject.SetActive(False)
			Me.olives.Add(gameObject.GetComponent(Of DicePalaceBoozeLevelOlive)())
		Next
		AddHandler Level.Current.OnIntroEvent, AddressOf Me.OnIntroEnd
		AddHandler Level.Current.OnWinEvent, AddressOf Me.HandleDead
		AudioManager.Play("booze_martini_intro")
		Me.emitAudioFromObject.Add("booze_martini_intro")
		MyBase.LevelInit(properties)
		Me.health = properties.CurrentState.martini.martiniHP
	End Sub

	' Token: 0x06001BAE RID: 7086 RVA: 0x000FC52C File Offset: 0x000FA92C
	Private Sub OnIntroEnd()
		MyBase.StartCoroutine(Me.attack_cr())
	End Sub

	' Token: 0x06001BAF RID: 7087 RVA: 0x000FC53C File Offset: 0x000FA93C
	Private Iterator Function attack_cr() As IEnumerator
		Me.oliveIndex = 0
		Dim counter As Integer = 0
		While True
			counter = 0
			Yield CupheadTime.WaitForSeconds(Me, MyBase.properties.CurrentState.martini.oliveSpawnDelay - DicePalaceBoozeLevelBossBase.ATTACK_DELAY)
			Yield Nothing
			While Me.olives(Me.oliveIndex).gameObject.activeSelf
				Me.oliveIndex = (Me.oliveIndex + 1) Mod Me.olives.Count
				counter += 1
				If counter >= Me.olives.Count Then
					Me.allActive = True
					Exit While
				End If
				Yield Nothing
			End While
			If counter < Me.olives.Count Then
				Me.allActive = False
			End If
			If Not Me.allActive Then
				MyBase.animator.SetTrigger("OnAttack")
				Yield MyBase.animator.WaitForAnimationToStart(Me, "Attack", False)
				AudioManager.Play("booze_martini_attack")
				Me.emitAudioFromObject.Add("booze_martini_attack")
				Yield MyBase.animator.WaitForAnimationToEnd(Me, "Attack", False, True)
			End If
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06001BB0 RID: 7088 RVA: 0x000FC558 File Offset: 0x000FA958
	Private Sub ShootOlive()
		Me.olives(Me.oliveIndex).transform.position = Me.spawnPoint.position
		Me.olives(Me.oliveIndex).gameObject.SetActive(True)
		Me.olives(Me.oliveIndex).ResetOlive(Parser.IntParse(MyBase.properties.CurrentState.martini.pinkString.Split(New Char() { ","c })(Me.pinkShotIndex)))
		Me.pinkShotIndex += 1
		If Me.pinkShotIndex >= MyBase.properties.CurrentState.martini.pinkString.Split(New Char() { ","c }).Length Then
			Me.pinkShotIndex = 0
		End If
		Me.oliveIndex = (Me.oliveIndex + 1) Mod Me.olives.Count
	End Sub

	' Token: 0x06001BB1 RID: 7089 RVA: 0x000FC64F File Offset: 0x000FAA4F
	Private Sub OnOliveDeath()
		Me.activeOlives -= 1
	End Sub

	' Token: 0x06001BB2 RID: 7090 RVA: 0x000FC65F File Offset: 0x000FAA5F
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
		MyBase.OnCollisionPlayer(hit, phase)
	End Sub

	' Token: 0x06001BB3 RID: 7091 RVA: 0x000FC67D File Offset: 0x000FAA7D
	Protected Overrides Sub OnDestroy()
		Me.StopAllCoroutines()
		MyBase.OnDestroy()
		Me.olive = Nothing
	End Sub

	' Token: 0x06001BB4 RID: 7092 RVA: 0x000FC692 File Offset: 0x000FAA92
	Private Sub MartiniDeathSFX()
		AudioManager.Play("martini_death_vox")
		Me.emitAudioFromObject.Add("martini_death_vox")
	End Sub

	' Token: 0x040024B9 RID: 9401
	<SerializeField()>
	Private olive As DicePalaceBoozeLevelOlive

	' Token: 0x040024BA RID: 9402
	<SerializeField()>
	Private spawnPoint As Transform

	' Token: 0x040024BB RID: 9403
	Private allActive As Boolean

	' Token: 0x040024BC RID: 9404
	Private oliveIndex As Integer

	' Token: 0x040024BD RID: 9405
	Private pinkShotIndex As Integer

	' Token: 0x040024BE RID: 9406
	Private activeOlives As Integer

	' Token: 0x040024BF RID: 9407
	Private olives As List(Of DicePalaceBoozeLevelOlive)

	' Token: 0x040024C0 RID: 9408
	Private damageDealer As DamageDealer

	' Token: 0x040024C1 RID: 9409
	Private damageReceiver As DamageReceiver
End Class
