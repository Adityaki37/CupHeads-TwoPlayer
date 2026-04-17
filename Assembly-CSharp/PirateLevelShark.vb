Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000725 RID: 1829
Public Class PirateLevelShark
	Inherits LevelProperties.Pirate.Entity

	' Token: 0x170003D1 RID: 977
	' (get) Token: 0x060027D2 RID: 10194 RVA: 0x00174B29 File Offset: 0x00172F29
	' (set) Token: 0x060027D3 RID: 10195 RVA: 0x00174B31 File Offset: 0x00172F31
	Public Property state As PirateLevelShark.State

	' Token: 0x060027D4 RID: 10196 RVA: 0x00174B3C File Offset: 0x00172F3C
	Protected Overrides Sub Awake()
		MyBase.Awake()
		AddHandler MyBase.GetComponent(Of DamageReceiver)().OnDamageTaken, AddressOf Me.OnDamageTaken
		Me.shark.SetActive(False)
		Me.shark.transform.SetLocalPosition(New Single?(-950F), Nothing, Nothing)
		Me.splash.SetActive(False)
	End Sub

	' Token: 0x060027D5 RID: 10197 RVA: 0x00174BAC File Offset: 0x00172FAC
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		If Me.state <> PirateLevelShark.State.[Exit] AndAlso Me.state <> PirateLevelShark.State.Exit_Shot Then
			Return
		End If
		If Me.shotCoroutine IsNot Nothing Then
			MyBase.StopCoroutine(Me.shotCoroutine)
		End If
		Me.shotCoroutine = Me.shot_cr()
		MyBase.StartCoroutine(Me.shotCoroutine)
	End Sub

	' Token: 0x060027D6 RID: 10198 RVA: 0x00174C02 File Offset: 0x00173002
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x060027D7 RID: 10199 RVA: 0x00174C20 File Offset: 0x00173020
	Public Overrides Sub LevelInitWithGroup(propertyGroup As AbstractLevelPropertyGroup)
		MyBase.LevelInitWithGroup(propertyGroup)
		Me.sharkProperties = TryCast(propertyGroup, LevelProperties.Pirate.Shark)
		MyBase.StartCoroutine(Me.shark_cr())
		MyBase.StartCoroutine(Me.collider_cr())
		Me.state = PirateLevelShark.State.Swim
		Me.damageDealer = New DamageDealer(1F, 1F)
		Me.damageDealer.SetDirection(DamageDealer.Direction.Right, MyBase.transform)
		Dim position As Vector3 = MyBase.transform.position
		position.x = Me.sharkProperties.x
		MyBase.transform.position = position
	End Sub

	' Token: 0x060027D8 RID: 10200 RVA: 0x00174CB2 File Offset: 0x001730B2
	Private Sub OnBiteAnimComplete()
		Me.state = PirateLevelShark.State.[Exit]
		MyBase.StartCoroutine(Me.exit_cr())
	End Sub

	' Token: 0x060027D9 RID: 10201 RVA: 0x00174CC8 File Offset: 0x001730C8
	Private Sub OnBiteAudio()
	End Sub

	' Token: 0x060027DA RID: 10202 RVA: 0x00174CCA File Offset: 0x001730CA
	Private Sub OnBiteShake()
		CupheadLevelCamera.Current.Shake(12F, 0.5F, False)
	End Sub

	' Token: 0x060027DB RID: 10203 RVA: 0x00174CE1 File Offset: 0x001730E1
	Private Sub Splash()
		Me.splash.SetActive(True)
		MyBase.animator.Play("Splash", 3)
	End Sub

	' Token: 0x060027DC RID: 10204 RVA: 0x00174D00 File Offset: 0x00173100
	Private Sub [End]()
		AudioManager.[Stop]("level_pirate_shark_exit_normal_loop")
		Me.state = PirateLevelShark.State.Complete
		Me.StopAllCoroutines()
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub

	' Token: 0x060027DD RID: 10205 RVA: 0x00174D24 File Offset: 0x00173124
	Private Iterator Function shot_cr() As IEnumerator
		Me.state = PirateLevelShark.State.Exit_Shot
		MyBase.animator.SetLayerWeight(1, 1F)
		Dim t As Single = 0F
		While t < 1F
			t += CupheadTime.Delta
			Yield Nothing
		End While
		MyBase.animator.SetLayerWeight(1, 0F)
		Me.state = PirateLevelShark.State.[Exit]
		Return
	End Function

	' Token: 0x060027DE RID: 10206 RVA: 0x00174D40 File Offset: 0x00173140
	Private Iterator Function shark_cr() As IEnumerator
		AudioManager.Play("level_pirate_shark_warning")
		Yield MyBase.StartCoroutine(Me.fin_cr())
		Me.shark.SetActive(True)
		Me.state = PirateLevelShark.State.Attack
		MyBase.animator.Play("Attack")
		AudioManager.Play("levels_pirate_shark_attack")
		Me.emitAudioFromObject.Add("levels_pirate_shark_attack")
		Return
	End Function

	' Token: 0x060027DF RID: 10207 RVA: 0x00174D5C File Offset: 0x0017315C
	Private Iterator Function exit_cr() As IEnumerator
		MyBase.animator.Play("Exit")
		AudioManager.PlayLoop("level_pirate_shark_exit_normal_loop")
		Me.emitAudioFromObject.Add("level_pirate_shark_exit_normal_loop")
		MyBase.animator.Play("Exit", 1)
		While True
			If Me.shark.transform.position.x < -950F Then
				Me.[End]()
			End If
			Dim speed As Single = If((Me.state <> PirateLevelShark.State.[Exit]), Me.sharkProperties.shotExitSpeed, Me.sharkProperties.exitSpeed) * CupheadTime.Delta
			MyBase.transform.AddPosition(-speed, 0F, 0F)
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x060027E0 RID: 10208 RVA: 0x00174D78 File Offset: 0x00173178
	Private Iterator Function fin_cr() As IEnumerator
		Dim t As Single = 0F
		Dim time As Single = Me.sharkProperties.finTime
		Dim startX As Integer = 640
		Dim endX As Integer = -740
		While t < time
			Dim val As Single = t / time
			Dim x As Single = Mathf.Lerp(CSng(startX), CSng(endX), val)
			Me.fin.transform.SetPosition(New Single?(x), Nothing, Nothing)
			t += CupheadTime.Delta
			Yield Nothing
		End While
		Me.fin.transform.SetPosition(New Single?(CSng(endX)), Nothing, Nothing)
		Yield CupheadTime.WaitForSeconds(Me, Me.sharkProperties.attackDelay)
		Return
	End Function

	' Token: 0x060027E1 RID: 10209 RVA: 0x00174D94 File Offset: 0x00173194
	Private Iterator Function collider_cr() As IEnumerator
		Dim collider As BoxCollider2D = TryCast(MyBase.GetComponent(Of Collider2D)(), BoxCollider2D)
		Dim childCollider As BoxCollider2D = TryCast(Me.shark.GetComponent(Of Collider2D)(), BoxCollider2D)
		While True
			collider.offset = Me.shark.transform.localPosition + childCollider.offset
			collider.size = childCollider.size
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x04003092 RID: 12434
	Public Const SHOT_DELAY As Single = 1F

	' Token: 0x04003093 RID: 12435
	Public Const START_X As Single = -950F

	' Token: 0x04003095 RID: 12437
	<SerializeField()>
	Private fin As GameObject

	' Token: 0x04003096 RID: 12438
	<SerializeField()>
	Private shark As GameObject

	' Token: 0x04003097 RID: 12439
	<SerializeField()>
	Private splash As GameObject

	' Token: 0x04003098 RID: 12440
	Private sharkProperties As LevelProperties.Pirate.Shark

	' Token: 0x04003099 RID: 12441
	Private damageDealer As DamageDealer

	' Token: 0x0400309A RID: 12442
	Private shotCoroutine As IEnumerator

	' Token: 0x02000726 RID: 1830
	Public Enum State
		' Token: 0x0400309C RID: 12444
		Init
		' Token: 0x0400309D RID: 12445
		Swim
		' Token: 0x0400309E RID: 12446
		Attack
		' Token: 0x0400309F RID: 12447
		[Exit]
		' Token: 0x040030A0 RID: 12448
		Exit_Shot
		' Token: 0x040030A1 RID: 12449
		Complete
	End Enum
End Class
