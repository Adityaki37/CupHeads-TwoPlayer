Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports UnityEngine

' Token: 0x020009D0 RID: 2512
Public MustInherit Class AbstractParryEffect
	Inherits Effect

	' Token: 0x06003AF9 RID: 15097 RVA: 0x00212BE4 File Offset: 0x00210FE4
	Public Function Create(player As AbstractPlayerController) As AbstractParryEffect
		Dim abstractParryEffect As AbstractParryEffect = TryCast(MyBase.Create(player.center, player.transform.localScale), AbstractParryEffect)
		abstractParryEffect.SetPlayer(player)
		Return abstractParryEffect
	End Function

	' Token: 0x170004D3 RID: 1235
	' (get) Token: 0x06003AFA RID: 15098
	Protected MustOverride ReadOnly Property IsHit As Boolean

	' Token: 0x06003AFB RID: 15099 RVA: 0x00212C18 File Offset: 0x00211018
	Public Overrides Sub Initialize(position As Vector3, scale As Vector3, randomR As Boolean)
		MyBase.Initialize(position, scale, randomR)
		MyBase.animator.enabled = False
		Me.sprites.SetActive(False)
		Me.projectiles = New List(Of AbstractProjectile)()
		Me.sparks = New List(Of Effect)()
		Me.switches = New List(Of ParrySwitch)()
		Me.entities = New List(Of AbstractLevelEntity)()
		MyBase.tag = "Parry"
	End Sub

	' Token: 0x06003AFC RID: 15100 RVA: 0x00212C80 File Offset: 0x00211080
	Protected Overrides Sub OnCollision(hit As GameObject, phase As CollisionPhase)
		If Me.cancel Then
			Return
		End If
		MyBase.OnCollision(hit, phase)
		If Not Me.player.IsDead AndAlso phase = CollisionPhase.Enter Then
			Dim abstractProjectile As AbstractProjectile = hit.GetComponent(Of AbstractProjectile)()
			If abstractProjectile Is Nothing Then
				Dim component As CollisionChild = hit.GetComponent(Of CollisionChild)()
				Dim abstractCollidableObject As AbstractCollidableObject
				If component IsNot Nothing AndAlso component.ForwardParry(abstractCollidableObject) Then
					abstractProjectile = abstractCollidableObject.GetComponent(Of AbstractProjectile)()
				End If
			End If
			If abstractProjectile IsNot Nothing AndAlso abstractProjectile.CanParry Then
				Me.projectiles.Add(abstractProjectile)
				If Not Me.player.stats.NextParryActivatesHealerCharm() Then
					Me.sparks.Add(Me.spark.Create(abstractProjectile.transform.position))
				End If
				If Not Me.didHitSomething Then
					MyBase.StartCoroutine(Me.hit_cr(False))
				End If
			End If
			Dim component2 As ParrySwitch = hit.GetComponent(Of ParrySwitch)()
			If component2 IsNot Nothing AndAlso component2.enabled AndAlso component2.IsParryable Then
				Me.switches.Add(component2)
				If Not Me.didHitSomething Then
					MyBase.StartCoroutine(Me.hit_cr(False))
				End If
			End If
			Dim component3 As AbstractLevelEntity = hit.GetComponent(Of AbstractLevelEntity)()
			If component3 IsNot Nothing AndAlso component3.enabled AndAlso component3.canParry Then
				Me.entities.Add(component3)
				If Not Me.didHitSomething Then
					MyBase.StartCoroutine(Me.hit_cr(False))
				End If
			End If
			If(Me.player.stats.Loadout.charm = Charm.charm_parry_attack OrElse Me.player.stats.CurseWhetsone) AndAlso Not Me.didHitSomething AndAlso Not Level.IsChessBoss Then
				Dim component4 As IParryAttack = Me.player.GetComponent(Of IParryAttack)()
				If component4 IsNot Nothing AndAlso Not component4.AttackParryUsed Then
					Dim damageReceiver As DamageReceiver = hit.GetComponent(Of DamageReceiver)()
					If damageReceiver Is Nothing Then
						Dim component5 As DamageReceiverChild = hit.GetComponent(Of DamageReceiverChild)()
						If component5 IsNot Nothing Then
							damageReceiver = component5.Receiver
						End If
					End If
					If damageReceiver IsNot Nothing AndAlso damageReceiver.type = DamageReceiver.Type.Enemy Then
						component4.HasHitEnemy = True
						Dim damageDealer As DamageDealer = New DamageDealer(WeaponProperties.CharmParryAttack.damage, 0F, False, True, False)
						damageDealer.DealDamage(hit)
						Me.ShowParryAttackEffect(hit)
						MyBase.StartCoroutine(Me.hit_cr(True))
					End If
				End If
			End If
		End If
	End Sub

	' Token: 0x06003AFD RID: 15101 RVA: 0x00212EF4 File Offset: 0x002112F4
	Private Sub ShowParryAttackEffect(hit As GameObject)
		Dim num As Integer = Physics2D.RaycastNonAlloc(hit.transform.position, MyBase.transform.position - hit.transform.position, Me.contactsBuffer, (MyBase.transform.position - hit.transform.position).magnitude)
		If num = 0 Then
			Return
		End If
		Dim vector As Vector3 = Me.contactsBuffer(0).point
		For i As Integer = 1 To num - 1
			If Me.contactsBuffer(i).collider.tag = "Parry" Then
				vector = Me.contactsBuffer(i).point
			End If
		Next
		Dim parryAttackSpark As ParryAttackSpark = TryCast(Me.parryAttack.Create(vector), ParryAttackSpark)
		parryAttackSpark.IsCuphead = Me.player.id = PlayerId.PlayerOne
		Me.sparks.Add(parryAttackSpark)
		parryAttackSpark.Play()
	End Sub

	' Token: 0x06003AFE RID: 15102 RVA: 0x00213008 File Offset: 0x00211408
	Protected Overridable Sub SetPlayer(player As AbstractPlayerController)
		Me.player = player
		MyBase.transform.SetParent(player.transform)
		MyBase.StartCoroutine(Me.lifetime_cr())
	End Sub

	' Token: 0x06003AFF RID: 15103 RVA: 0x0021302F File Offset: 0x0021142F
	Protected Overridable Sub OnHitCancel()
		If Me Is Nothing Then
			Return
		End If
		Me.Cancel()
		AudioManager.[Stop]("player_parry")
	End Sub

	' Token: 0x06003B00 RID: 15104 RVA: 0x00213050 File Offset: 0x00211450
	Protected Overridable Sub Cancel()
		For Each effect As Effect In Me.sparks
			Global.UnityEngine.[Object].Destroy(effect.gameObject)
		Next
		Me.cancel = True
		Me.CancelSwitch()
		Me.StopAllCoroutines()
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub

	' Token: 0x06003B01 RID: 15105 RVA: 0x002130D0 File Offset: 0x002114D0
	Protected Overridable Sub CancelSwitch()
	End Sub

	' Token: 0x06003B02 RID: 15106 RVA: 0x002130D2 File Offset: 0x002114D2
	Protected Overridable Sub OnPaused()
	End Sub

	' Token: 0x06003B03 RID: 15107 RVA: 0x002130D4 File Offset: 0x002114D4
	Protected Overridable Sub OnUnpaused()
	End Sub

	' Token: 0x06003B04 RID: 15108 RVA: 0x002130D6 File Offset: 0x002114D6
	Protected Overridable Sub OnSuccess()
	End Sub

	' Token: 0x06003B05 RID: 15109 RVA: 0x002130D8 File Offset: 0x002114D8
	Protected Overridable Sub OnEnd()
	End Sub

	' Token: 0x06003B06 RID: 15110 RVA: 0x002130DC File Offset: 0x002114DC
	Private Iterator Function lifetime_cr() As IEnumerator
		If Me.player IsNot Nothing AndAlso (Me.player.stats.Loadout.charm <> Charm.charm_parry_plus OrElse Level.IsChessBoss) Then
			If Me.player.stats.isChalice Then
				Yield CupheadTime.WaitForSeconds(Me, If((Level.Current.playerMode <> PlayerMode.Plane), 0.3F, 0.4F))
			Else
				Yield CupheadTime.WaitForSeconds(Me, 0.2F)
			End If
			MyBase.GetComponent(Of Collider2D)().enabled = False
			Me.CancelSwitch()
			Yield CupheadTime.WaitForSeconds(Me, 1F)
		End If
		Yield Nothing
		Return
	End Function

	' Token: 0x06003B07 RID: 15111 RVA: 0x002130F8 File Offset: 0x002114F8
	Private Iterator Function hit_cr(Optional hitEnemy As Boolean = False) As IEnumerator
		If Me.player.IsDead OrElse Not Me.player.gameObject.activeInHierarchy OrElse Not MyBase.gameObject.activeInHierarchy Then
			Return
		End If
		Dim hit As Boolean = False
		Me.didHitSomething = True
		Dim parryController As IParryAttack = Me.player.GetComponent(Of IParryAttack)()
		If parryController IsNot Nothing Then
			parryController.AttackParryUsed = True
		End If
		MyBase.animator.enabled = True
		Me.sprites.SetActive(True)
		If Not hitEnemy Then
			For Each parrySwitch As ParrySwitch In Me.switches
				parrySwitch.OnParryPrePause(Me.player)
			Next
			For Each abstractLevelEntity As AbstractLevelEntity In Me.entities
				abstractLevelEntity.OnParry(Me.player)
			Next
			For Each abstractProjectile As AbstractProjectile In Me.projectiles
				abstractProjectile.OnParry(Me.player)
				Me.player.stats.OnParry(abstractProjectile.ParryMeterMultiplier, abstractProjectile.CountParryTowardsScore)
			Next
		End If
		If Me.player.IsDead OrElse Not Me.player.gameObject.activeInHierarchy OrElse Not MyBase.gameObject.activeInHierarchy Then
			Return
		End If
		If Level.Current Is Nothing OrElse Not Level.IsChessBoss OrElse Not Level.Current.Ending Then
			PauseManager.Pause()
		End If
		AudioManager.Play("player_parry")
		Me.OnPaused()
		Dim pauseTime As Single = If((Not hitEnemy), 0.185F, 0.13875F)
		Dim t As Single = 0F
		While t < pauseTime
			hit = Me.IsHit
			If hit Then
				t = pauseTime
			End If
			t += Time.fixedDeltaTime
			For i As Integer = 0 To 2 - 1
				Dim playerId As PlayerId = If((i <> 0), PlayerId.PlayerTwo, PlayerId.PlayerOne)
				If Me.player IsNot Nothing AndAlso Me.player.id = playerId Then
					If pauseTime - t < 0.134F Then
						Me.player.BufferInputs()
					End If
				Else
					Dim abstractPlayerController As AbstractPlayerController = PlayerManager.GetPlayer(playerId)
					If abstractPlayerController IsNot Nothing Then
						abstractPlayerController.BufferInputs()
					End If
				End If
			Next
			Yield New WaitForFixedUpdate()
		End While
		While LevelNewPlayerGUI.Current IsNot Nothing AndAlso LevelNewPlayerGUI.Current.gameObject.activeInHierarchy
			Yield Nothing
		End While
		If Not hit Then
			Me.OnSuccess()
			If Level.Current Is Nothing OrElse Not Level.IsChessBoss OrElse Not Level.Current.Ending Then
				PauseManager.Unpause()
			End If
			Me.OnUnpaused()
			Me.OnEnd()
			MyBase.transform.parent = Nothing
			MyBase.GetComponent(Of Collider2D)().enabled = False
			If Not hitEnemy Then
				For Each parrySwitch2 As ParrySwitch In Me.switches
					parrySwitch2.OnParryPostPause(Me.player)
				Next
			End If
		End If
		Return
	End Function

	' Token: 0x06003B08 RID: 15112 RVA: 0x0021311A File Offset: 0x0021151A
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me.spark = Nothing
		Me.parryAttack = Nothing
	End Sub

	' Token: 0x040042B5 RID: 17077
	Public Const TAG As String = "Parry"

	' Token: 0x040042B6 RID: 17078
	Private Const PAUSE_TIME As Single = 0.185F

	' Token: 0x040042B7 RID: 17079
	Private Const COLLIDER_LIFETIME As Single = 0.2F

	' Token: 0x040042B8 RID: 17080
	Private Const CHALICE_COLLIDER_LIFETIME As Single = 0.3F

	' Token: 0x040042B9 RID: 17081
	Private Const CHALICE_PLANE_COLLIDER_LIFETIME As Single = 0.4F

	' Token: 0x040042BA RID: 17082
	Private Const SPRITE_LIFETIME As Single = 1F

	' Token: 0x040042BB RID: 17083
	<SerializeField()>
	Private sprites As GameObject

	' Token: 0x040042BC RID: 17084
	<SerializeField()>
	Private spark As Effect

	' Token: 0x040042BD RID: 17085
	<SerializeField()>
	Private parryAttack As ParryAttackSpark

	' Token: 0x040042BE RID: 17086
	Protected player As AbstractPlayerController

	' Token: 0x040042BF RID: 17087
	Protected didHitSomething As Boolean

	' Token: 0x040042C0 RID: 17088
	Protected cancel As Boolean

	' Token: 0x040042C1 RID: 17089
	Private projectiles As List(Of AbstractProjectile)

	' Token: 0x040042C2 RID: 17090
	Private sparks As List(Of Effect)

	' Token: 0x040042C3 RID: 17091
	Private switches As List(Of ParrySwitch)

	' Token: 0x040042C4 RID: 17092
	Private entities As List(Of AbstractLevelEntity)

	' Token: 0x040042C5 RID: 17093
	Private contactsBuffer As RaycastHit2D() = New RaycastHit2D(9) {}
End Class
