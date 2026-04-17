Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020005A0 RID: 1440
Public Class DicePalaceBoozeLevelDecanter
	Inherits DicePalaceBoozeLevelBossBase

	' Token: 0x06001BA0 RID: 7072 RVA: 0x000FBC87 File Offset: 0x000FA087
	Protected Overrides Sub Awake()
		Me.damageDealer = DamageDealer.NewEnemy()
		Me.damageReceiver = MyBase.GetComponent(Of DamageReceiver)()
		AddHandler Me.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
		MyBase.Awake()
	End Sub

	' Token: 0x06001BA1 RID: 7073 RVA: 0x000FBCBD File Offset: 0x000FA0BD
	Private Sub Update()
		Me.damageDealer.Update()
	End Sub

	' Token: 0x06001BA2 RID: 7074 RVA: 0x000FBCCC File Offset: 0x000FA0CC
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		Dim health As Single = Me.health
		Me.health -= info.damage
		If health > 0F Then
			Level.Current.timeline.DealDamage(Mathf.Clamp(health - Me.health, 0F, health))
		End If
		If Me.health < 0F AndAlso Not MyBase.isDead Then
			Me.StartDying()
		End If
	End Sub

	' Token: 0x06001BA3 RID: 7075 RVA: 0x000FBD44 File Offset: 0x000FA144
	Public Overrides Sub LevelInit(properties As LevelProperties.DicePalaceBooze)
		Me.dropPosition.z = 0F
		Me.dropPosition.y = Me.sprayYRoot.position.y
		Me.attackDelayIndex = Global.UnityEngine.Random.Range(0, properties.CurrentState.decanter.attackDelayString.Split(New Char() { ","c }).Length)
		Me.attacking = False
		Me.nextPlayerTarget = PlayerId.PlayerOne
		AddHandler Level.Current.OnIntroEvent, AddressOf Me.OnIntroEnd
		AddHandler Level.Current.OnWinEvent, AddressOf Me.HandleDead
		Me.health = properties.CurrentState.decanter.decanterHP
		AudioManager.Play("booze_decanter_intro")
		Me.emitAudioFromObject.Add("booze_decanter_intro")
		MyBase.LevelInit(properties)
	End Sub

	' Token: 0x06001BA4 RID: 7076 RVA: 0x000FBE1F File Offset: 0x000FA21F
	Private Sub OnIntroEnd()
		MyBase.StartCoroutine(Me.attack_cr())
	End Sub

	' Token: 0x06001BA5 RID: 7077 RVA: 0x000FBE30 File Offset: 0x000FA230
	Private Iterator Function attack_cr() As IEnumerator
		While True
			Yield CupheadTime.WaitForSeconds(Me, Parser.FloatParse(MyBase.properties.CurrentState.decanter.attackDelayString.Split(New Char() { ","c })(Me.attackDelayIndex)) - DicePalaceBoozeLevelBossBase.ATTACK_DELAY)
			MyBase.animator.SetTrigger("OnAttack")
			Yield MyBase.animator.WaitForAnimationToStart(Me, "Attack", False)
			AudioManager.Play("booze_decanter_attack")
			Me.emitAudioFromObject.Add("booze_decanter_attack")
			MyBase.StartCoroutine(Me.spray_cr())
			Me.attackDelayIndex += 1
			If Me.attackDelayIndex >= MyBase.properties.CurrentState.decanter.attackDelayString.Split(New Char() { ","c }).Length Then
				Me.attackDelayIndex = 0
			End If
			If Me.nextPlayerTarget = PlayerId.PlayerOne Then
				If PlayerManager.GetPlayer(PlayerId.PlayerTwo) IsNot Nothing Then
					Me.nextPlayerTarget = PlayerId.PlayerTwo
				End If
			Else
				Me.nextPlayerTarget = PlayerId.PlayerOne
			End If
			While Me.attacking
				Yield Nothing
			End While
		End While
		Return
	End Function

	' Token: 0x06001BA6 RID: 7078 RVA: 0x000FBE4C File Offset: 0x000FA24C
	Private Iterator Function spray_cr() As IEnumerator
		Me.attacking = True
		Yield CupheadTime.WaitForSeconds(Me, MyBase.properties.CurrentState.decanter.beamAppearDelayRange.RandomFloat())
		AudioManager.Play("booze_decanter_spray_down")
		Me.emitAudioFromObject.Add("booze_decanter_spray_down")
		Dim spray As GameObject = Global.UnityEngine.[Object].Instantiate(Of GameObject)(Me.sprayPrefab, Me.dropPosition, Quaternion.identity)
		Me.attacking = False
		Me.dropPosition.x = PlayerManager.GetPlayer(Me.nextPlayerTarget).center.x
		Dim pos As Vector3 = spray.transform.position
		pos.x = Me.dropPosition.x
		spray.transform.position = pos
		Yield spray.GetComponent(Of Animator)().WaitForAnimationToEnd(Me, "Spray", False, True)
		Global.UnityEngine.[Object].Destroy(spray)
		Return
	End Function

	' Token: 0x06001BA7 RID: 7079 RVA: 0x000FBE67 File Offset: 0x000FA267
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
		MyBase.OnCollisionPlayer(hit, phase)
	End Sub

	' Token: 0x06001BA8 RID: 7080 RVA: 0x000FBE85 File Offset: 0x000FA285
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me.sprayPrefab = Nothing
	End Sub

	' Token: 0x040024B1 RID: 9393
	<SerializeField()>
	Private sprayYRoot As Transform

	' Token: 0x040024B2 RID: 9394
	<SerializeField()>
	Private sprayPrefab As GameObject

	' Token: 0x040024B3 RID: 9395
	Private attacking As Boolean

	' Token: 0x040024B4 RID: 9396
	Private attackDelayIndex As Integer

	' Token: 0x040024B5 RID: 9397
	Private nextPlayerTarget As PlayerId

	' Token: 0x040024B6 RID: 9398
	Private dropPosition As Vector3

	' Token: 0x040024B7 RID: 9399
	Private damageDealer As DamageDealer

	' Token: 0x040024B8 RID: 9400
	Private damageReceiver As DamageReceiver
End Class
