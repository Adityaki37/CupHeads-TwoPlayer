Imports System
Imports System.Collections
Imports System.Diagnostics
Imports UnityEngine

' Token: 0x0200081C RID: 2076
Public Class TrainLevelLollipopGhoul
	Inherits LevelProperties.Train.Entity

	' Token: 0x17000420 RID: 1056
	' (get) Token: 0x0600302A RID: 12330 RVA: 0x001C6E56 File Offset: 0x001C5256
	' (set) Token: 0x0600302B RID: 12331 RVA: 0x001C6E5E File Offset: 0x001C525E
	Public Property state As TrainLevelLollipopGhoul.State

	' Token: 0x14000053 RID: 83
	' (add) Token: 0x0600302C RID: 12332 RVA: 0x001C6E68 File Offset: 0x001C5268
	' (remove) Token: 0x0600302D RID: 12333 RVA: 0x001C6EA0 File Offset: 0x001C52A0
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnDamageTakenEvent As TrainLevelLollipopGhoul.OnDamageTakenHandler

	' Token: 0x14000054 RID: 84
	' (add) Token: 0x0600302E RID: 12334 RVA: 0x001C6ED8 File Offset: 0x001C52D8
	' (remove) Token: 0x0600302F RID: 12335 RVA: 0x001C6F10 File Offset: 0x001C5310
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnDeathEvent As Action

	' Token: 0x06003030 RID: 12336 RVA: 0x001C6F46 File Offset: 0x001C5346
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.damageReceiver = MyBase.GetComponent(Of DamageReceiver)()
		AddHandler Me.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
	End Sub

	' Token: 0x06003031 RID: 12337 RVA: 0x001C6F74 File Offset: 0x001C5374
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		If Me.health <= 0F Then
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

	' Token: 0x06003032 RID: 12338 RVA: 0x001C6FD7 File Offset: 0x001C53D7
	Public Overrides Sub LevelInit(properties As LevelProperties.Train)
		MyBase.LevelInit(properties)
		Me.health = properties.CurrentState.lollipopGhouls.health
	End Sub

	' Token: 0x06003033 RID: 12339 RVA: 0x001C6FF6 File Offset: 0x001C53F6
	Private Sub Die()
		If Me.OnDeathEvent IsNot Nothing Then
			Me.OnDeathEvent()
		End If
		Me.OnDeathEvent = Nothing
		Me.StopAllCoroutines()
		MyBase.StartCoroutine(Me.die_cr())
	End Sub

	' Token: 0x06003034 RID: 12340 RVA: 0x001C7028 File Offset: 0x001C5428
	Private Sub DeathAnimComplete()
		Me.StopAllCoroutines()
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub

	' Token: 0x06003035 RID: 12341 RVA: 0x001C703B File Offset: 0x001C543B
	Public Sub AnimateIn()
		MyBase.animator.Play("Intro")
		Me.state = TrainLevelLollipopGhoul.State.Ready
	End Sub

	' Token: 0x06003036 RID: 12342 RVA: 0x001C7054 File Offset: 0x001C5454
	Public Sub Attack()
		Me.state = TrainLevelLollipopGhoul.State.Attacking
		MyBase.StartCoroutine(Me.attack_cr())
	End Sub

	' Token: 0x06003037 RID: 12343 RVA: 0x001C706C File Offset: 0x001C546C
	Private Sub StartLightning()
		If Me.currentLightning IsNot Nothing Then
			Global.UnityEngine.[Object].Destroy(Me.currentLightning)
		End If
		Me.currentLightning = Global.UnityEngine.[Object].Instantiate(Of TrainLevelLollipopGhoulLightning)(Me.lightningPrefab)
		Me.currentLightning.transform.SetParent(Me.lightningRoot)
		Me.currentLightning.transform.ResetLocalTransforms()
	End Sub

	' Token: 0x06003038 RID: 12344 RVA: 0x001C70CC File Offset: 0x001C54CC
	Private Sub EndLightning()
		If Me.currentLightning Is Nothing Then
			Return
		End If
		Me.currentLightning.[End]()
		Me.currentLightning = Nothing
	End Sub

	' Token: 0x06003039 RID: 12345 RVA: 0x001C70F4 File Offset: 0x001C54F4
	Private Iterator Function attack_cr() As IEnumerator
		Yield Nothing
		MyBase.animator.ResetTrigger("Continue")
		MyBase.animator.SetTrigger("OnAttack")
		Yield MyBase.animator.WaitForAnimationToStart(Me, "Attack_Charge", False)
		AudioManager.Play("train_lollipop_ghoul_attack_start")
		Me.emitAudioFromObject.Add("train_lollipop_ghoul_attack_start")
		Yield CupheadTime.WaitForSeconds(Me, MyBase.properties.CurrentState.lollipopGhouls.warningTime)
		MyBase.animator.SetTrigger("Continue")
		Yield MyBase.animator.WaitForAnimationToStart(Me, "Attack_Loop", False)
		AudioManager.PlayLoop("train_lollipop_ghoul_attack_loop")
		Me.emitAudioFromObject.Add("train_lollipop_ghoul_attack_loop")
		Me.StartLightning()
		Yield MyBase.StartCoroutine(Me.head_cr())
		Me.EndLightning()
		AudioManager.[Stop]("train_lollipop_ghoul_attack_loop")
		AudioManager.Play("train_lollipop_ghoul_attack_end")
		Yield Nothing
		MyBase.animator.SetTrigger("Continue")
		Me.state = TrainLevelLollipopGhoul.State.Ready
		Return
	End Function

	' Token: 0x0600303A RID: 12346 RVA: 0x001C7110 File Offset: 0x001C5510
	Private Iterator Function head_cr() As IEnumerator
		Dim t As Single = 0F
		Dim time As Single = MyBase.properties.CurrentState.lollipopGhouls.moveTime
		Dim ease As EaseUtils.EaseType = EaseUtils.EaseType.easeInOutSine
		Dim start As Vector3 = Vector3.zero
		Dim [end] As Vector3 = New Vector3(MyBase.properties.CurrentState.lollipopGhouls.moveDistance, 0F, 0F)
		Me.head.localPosition = start
		While t < time
			Dim val As Single = EaseUtils.Ease(ease, 0F, 1F, t / time)
			Me.head.localPosition = Vector3.Lerp(start, [end], val)
			t += CupheadTime.Delta
			Yield Nothing
		End While
		Me.head.localPosition = [end]
		t = 0F
		While t < time
			Dim val2 As Single = EaseUtils.Ease(ease, 0F, 1F, t / time)
			Me.head.localPosition = Vector3.Lerp([end], start, val2)
			t += CupheadTime.Delta
			Yield Nothing
		End While
		Me.head.localPosition = start
		Return
	End Function

	' Token: 0x0600303B RID: 12347 RVA: 0x001C712C File Offset: 0x001C552C
	Private Iterator Function die_cr() As IEnumerator
		AudioManager.[Stop]("train_lollipop_ghoul_attack_loop")
		AudioManager.Play("train_lollipop_ghoul_die")
		Me.emitAudioFromObject.Add("train_lollipop_ghoul_die")
		Me.state = TrainLevelLollipopGhoul.State.Dead
		Yield CupheadTime.WaitForSeconds(Me, 0.3F)
		If Me.currentLightning IsNot Nothing Then
			Me.EndLightning()
		End If
		MyBase.animator.Play("Die")
		Return
	End Function

	' Token: 0x0600303C RID: 12348 RVA: 0x001C7147 File Offset: 0x001C5547
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me.lightningPrefab = Nothing
	End Sub

	' Token: 0x040038F3 RID: 14579
	<SerializeField()>
	Private head As Transform

	' Token: 0x040038F4 RID: 14580
	<SerializeField()>
	Private lightningRoot As Transform

	' Token: 0x040038F5 RID: 14581
	<Space(10F)>
	<SerializeField()>
	Private lightningPrefab As TrainLevelLollipopGhoulLightning

	' Token: 0x040038F7 RID: 14583
	Private health As Single

	' Token: 0x040038F8 RID: 14584
	Private damageReceiver As DamageReceiver

	' Token: 0x040038F9 RID: 14585
	Private currentLightning As TrainLevelLollipopGhoulLightning

	' Token: 0x0200081D RID: 2077
	Public Enum State
		' Token: 0x040038FD RID: 14589
		Init
		' Token: 0x040038FE RID: 14590
		Ready
		' Token: 0x040038FF RID: 14591
		Attacking
		' Token: 0x04003900 RID: 14592
		Dead
	End Enum

	' Token: 0x0200081E RID: 2078
	' (Invoke) Token: 0x0600303E RID: 12350
	Public Delegate Sub OnDamageTakenHandler(damage As Single)
End Class
