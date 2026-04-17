Imports System
Imports System.Collections
Imports System.Diagnostics
Imports UnityEngine

' Token: 0x0200080A RID: 2058
Public Class TrainLevelBlindSpecter
	Inherits LevelProperties.Train.Entity

	' Token: 0x1400004F RID: 79
	' (add) Token: 0x06002F9B RID: 12187 RVA: 0x001C3B6C File Offset: 0x001C1F6C
	' (remove) Token: 0x06002F9C RID: 12188 RVA: 0x001C3BA4 File Offset: 0x001C1FA4
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnDamageTakenEvent As TrainLevelBlindSpecter.OnDamageTakenHandler

	' Token: 0x14000050 RID: 80
	' (add) Token: 0x06002F9D RID: 12189 RVA: 0x001C3BDC File Offset: 0x001C1FDC
	' (remove) Token: 0x06002F9E RID: 12190 RVA: 0x001C3C14 File Offset: 0x001C2014
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnDeathEvent As Action

	' Token: 0x06002F9F RID: 12191 RVA: 0x001C3C4C File Offset: 0x001C204C
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.spriteRenderer = MyBase.GetComponent(Of SpriteRenderer)()
		Me.damageReceiver = MyBase.GetComponent(Of DamageReceiver)()
		AddHandler Me.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
		Me.damageDealer = DamageDealer.NewEnemy()
		MyBase.animator.enabled = False
		Me.spriteRenderer.enabled = False
	End Sub

	' Token: 0x06002FA0 RID: 12192 RVA: 0x001C3CB1 File Offset: 0x001C20B1
	Private Sub Start()
		AddHandler Level.Current.OnIntroEvent, AddressOf Me.OnIntro
	End Sub

	' Token: 0x06002FA1 RID: 12193 RVA: 0x001C3CC9 File Offset: 0x001C20C9
	Private Sub Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x06002FA2 RID: 12194 RVA: 0x001C3CE1 File Offset: 0x001C20E1
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If Me.damageDealer IsNot Nothing AndAlso phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x06002FA3 RID: 12195 RVA: 0x001C3D0C File Offset: 0x001C210C
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		If Me.dead Then
			Return
		End If
		If Me.OnDamageTakenEvent IsNot Nothing Then
			Me.OnDamageTakenEvent(info.damage)
		End If
		Me.health -= info.damage
		If Me.health <= 0F Then
			Me.Die()
		End If
	End Sub

	' Token: 0x06002FA4 RID: 12196 RVA: 0x001C3D6A File Offset: 0x001C216A
	Public Overrides Sub LevelInit(properties As LevelProperties.Train)
		MyBase.LevelInit(properties)
		Me.health = CSng(properties.CurrentState.blindSpecter.health)
	End Sub

	' Token: 0x06002FA5 RID: 12197 RVA: 0x001C3D8A File Offset: 0x001C218A
	Private Sub OnIntro()
		MyBase.StartCoroutine(Me.loop_cr())
	End Sub

	' Token: 0x06002FA6 RID: 12198 RVA: 0x001C3D9C File Offset: 0x001C219C
	Private Sub Die()
		If Me.dead Then
			Return
		End If
		MyBase.GetComponent(Of LevelBossDeathExploder)().StartExplosion()
		Me.dead = True
		Me.damageReceiver.enabled = False
		Me.StopAllCoroutines()
		MyBase.animator.Play("Death")
		AudioManager.Play("train_blindspector_death")
		Me.emitAudioFromObject.Add("train_blindspector_death")
	End Sub

	' Token: 0x06002FA7 RID: 12199 RVA: 0x001C3E03 File Offset: 0x001C2203
	Private Sub OnDeathAnimComplete()
		If Me.OnDeathEvent IsNot Nothing Then
			Me.OnDeathEvent()
		End If
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub

	' Token: 0x06002FA8 RID: 12200 RVA: 0x001C3E26 File Offset: 0x001C2226
	Private Sub SfxIntro()
		AudioManager.Play("level_train_blindspecter_intro")
	End Sub

	' Token: 0x06002FA9 RID: 12201 RVA: 0x001C3E34 File Offset: 0x001C2234
	Private Sub FireEyeball()
		AudioManager.Play("train_blindspector_attack")
		Me.emitAudioFromObject.Add("train_blindspector_attack")
		Dim value As Single = Global.UnityEngine.Random.value
		Dim vector As Vector2 = New Vector2(Me.blindSpecterProperties.timeX.RandomFloat(), Me.blindSpecterProperties.timeY.GetFloatAt(value))
		Me.eyePrefab.Create(Me.eyeRoot.position, vector, Me.blindSpecterProperties.heightMax.GetFloatAt(value), Me.shots Mod 2 > 0, Me.blindSpecterProperties.eyeHealth)
		Me.shots += 1
	End Sub

	' Token: 0x06002FAA RID: 12202 RVA: 0x001C3EDC File Offset: 0x001C22DC
	Private Iterator Function loop_cr() As IEnumerator
		MyBase.animator.enabled = True
		Me.spriteRenderer.enabled = True
		MyBase.animator.Play("Intro")
		Me.blindSpecterProperties = MyBase.properties.CurrentState.blindSpecter
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Intro", False, True)
		Yield CupheadTime.WaitForSeconds(Me, 2F)
		While True
			Me.shots = 0
			MyBase.animator.Play("Attack_Start")
			While Me.shots < Me.blindSpecterProperties.attackLoops * 2
				Yield Nothing
			End While
			MyBase.animator.SetTrigger("Continue")
			Yield MyBase.animator.WaitForAnimationToEnd(Me, "Attack_End", False, True)
			Yield CupheadTime.WaitForSeconds(Me, Me.blindSpecterProperties.hesitate)
		End While
		Return
	End Function

	' Token: 0x06002FAB RID: 12203 RVA: 0x001C3EF7 File Offset: 0x001C22F7
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me.eyePrefab = Nothing
	End Sub

	' Token: 0x04003881 RID: 14465
	<SerializeField()>
	Private eyeRoot As Transform

	' Token: 0x04003882 RID: 14466
	<SerializeField()>
	Private eyePrefab As TrainLevelBlindSpecterEyeProjectile

	' Token: 0x04003883 RID: 14467
	Private spriteRenderer As SpriteRenderer

	' Token: 0x04003884 RID: 14468
	Private blindSpecterProperties As LevelProperties.Train.BlindSpecter

	' Token: 0x04003885 RID: 14469
	Private shots As Integer

	' Token: 0x04003886 RID: 14470
	Private damageDealer As DamageDealer

	' Token: 0x04003887 RID: 14471
	Private damageReceiver As DamageReceiver

	' Token: 0x04003888 RID: 14472
	Private health As Single

	' Token: 0x04003889 RID: 14473
	Private dead As Boolean

	' Token: 0x0200080B RID: 2059
	' (Invoke) Token: 0x06002FAD RID: 12205
	Public Delegate Sub OnDamageTakenHandler(damage As Single)
End Class
