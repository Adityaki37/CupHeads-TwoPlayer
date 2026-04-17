Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020004C3 RID: 1219
Public Class AirplaneLevelSecretTerrier
	Inherits LevelProperties.Airplane.Entity

	' Token: 0x0600146F RID: 5231 RVA: 0x000B7550 File Offset: 0x000B5950
	Private Sub Start()
		Me.damageDealer = DamageDealer.NewEnemy()
		Me.damageReceiver = MyBase.GetComponent(Of DamageReceiver)()
		AddHandler Me.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
		Me.MoveToHolePosition()
		MyBase.animator.Play("Intro_" + Me.introNum(Me.currentHole))
		MyBase.animator.Update(0F)
		Me.firstAttack = True
	End Sub

	' Token: 0x06001470 RID: 5232 RVA: 0x000B75CF File Offset: 0x000B59CF
	Protected Overrides Sub OnDestroy()
		RemoveHandler Me.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
		MyBase.OnDestroy()
		Me.WORKAROUND_NullifyFields()
	End Sub

	' Token: 0x06001471 RID: 5233 RVA: 0x000B75F4 File Offset: 0x000B59F4
	Public Overrides Sub LevelInit(properties As LevelProperties.Airplane)
		MyBase.LevelInit(properties)
		Me.hp = properties.CurrentState.secretTerriers.dogRetreatDamage
		Me.level.OccupyHole(Me.currentHole)
		MyBase.transform.localScale = New Vector3(-Mathf.Sign(Me.level.GetHolePosition(Me.currentHole, False).x - Camera.main.transform.position.x), 1F)
		MyBase.transform.position = Me.level.GetHolePosition(Me.currentHole, False)
	End Sub

	' Token: 0x06001472 RID: 5234 RVA: 0x000B7699 File Offset: 0x000B5A99
	Private Sub AniEvent_StartTerriers()
		MyBase.StartCoroutine(Me.attack_cr())
	End Sub

	' Token: 0x06001473 RID: 5235 RVA: 0x000B76A8 File Offset: 0x000B5AA8
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		If Me.hp > 0F Then
			Me.hp -= info.damage
			If Me.hp <= 0F Then
				Level.Current.RegisterMinionKilled()
				Me.StopAllCoroutines()
				Me.coll.enabled = False
				MyBase.StartCoroutine(Me.timeout_cr())
			End If
		End If
	End Sub

	' Token: 0x06001474 RID: 5236 RVA: 0x000B7711 File Offset: 0x000B5B11
	Public Function CurrentHole() As Integer
		Return Me.currentHole
	End Function

	' Token: 0x06001475 RID: 5237 RVA: 0x000B7719 File Offset: 0x000B5B19
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x06001476 RID: 5238 RVA: 0x000B7730 File Offset: 0x000B5B30
	Protected Sub MoveToHolePosition()
		Me.rend.sortingOrder = Me.currentHole Mod 3 + 50
		Me.backerRend.sortingOrder = Me.currentHole Mod 3 + 13
		MyBase.transform.localScale = New Vector3(-Mathf.Sign(Me.level.GetHolePosition(Me.currentHole, False).x - Camera.main.transform.position.x), 1F)
		MyBase.transform.position = Me.level.GetHolePosition(Me.currentHole, False)
	End Sub

	' Token: 0x06001477 RID: 5239 RVA: 0x000B77D4 File Offset: 0x000B5BD4
	Public Sub Die(index As Integer)
		Me.StopAllCoroutines()
		While Me.currentHole = -1
			Me.currentHole = Me.level.GetNextHole()
			Me.MoveToHolePosition()
		End While
		Dim text As String = "Death_" + (index + 1).ToString()
		MyBase.animator.Play(text)
		If index + 1 = 1 OrElse index + 1 = 4 Then
			MyBase.animator.Play("Stars", 1)
		End If
	End Sub

	' Token: 0x06001478 RID: 5240 RVA: 0x000B785A File Offset: 0x000B5C5A
	Private Sub HideAnimationComplete()
		Me.moved = True
		Me.coll.enabled = False
	End Sub

	' Token: 0x06001479 RID: 5241 RVA: 0x000B7870 File Offset: 0x000B5C70
	Private Sub AttackAnimationComplete()
		Dim secretTerriers As LevelProperties.Airplane.SecretTerriers = MyBase.properties.CurrentState.secretTerriers
		If Me.nextAttackPink Then
			Me.bulletPrefabPink.Create(Me.bulletRoot.position, PlayerManager.GetNext().transform.position, secretTerriers, MyBase.transform.localScale)
		Else
			Me.bulletPrefab.Create(Me.bulletRoot.position, PlayerManager.GetNext().transform.position, secretTerriers, MyBase.transform.localScale)
		End If
		Me.attacked = True
		AudioManager.Play("sfx_dlc_dogfight_ps_terrier_pineapplethrow")
	End Sub

	' Token: 0x0600147A RID: 5242 RVA: 0x000B7914 File Offset: 0x000B5D14
	Public Sub TryStartAttack()
		Me.nextAttackPink = Me.leader.TerrierProjectileParryable()
		If Me.canAttack Then
			MyBase.animator.SetTrigger(If((Not Me.nextAttackPink), "Attack", "AttackPink"))
		End If
	End Sub

	' Token: 0x0600147B RID: 5243 RVA: 0x000B7964 File Offset: 0x000B5D64
	Private Iterator Function attack_cr() As IEnumerator
		Me.level.OccupyHole(Me.currentHole)
		While True
			Me.MoveToHolePosition()
			If Not Me.firstAttack Then
				MyBase.animator.Play("Emerge")
			End If
			Me.firstAttack = False
			Me.canAttack = True
			Me.coll.enabled = True
			Me.attacked = False
			Me.moved = False
			While Not Me.attacked
				Yield Nothing
			End While
			Me.canAttack = False
			Me.attacked = False
			Yield CupheadTime.WaitForSeconds(Me, MyBase.properties.CurrentState.secretTerriers.dogPostAttackDelay)
			MyBase.animator.SetTrigger("OnMove")
			While Not Me.moved
				Yield Nothing
			End While
			Me.moved = False
			Dim previousHole As Integer = Me.currentHole
			Me.currentHole = -1
			While Me.currentHole = -1
				Me.currentHole = Me.level.GetNextHole()
			End While
			Me.level.LeaveHole(previousHole)
		End While
		Return
	End Function

	' Token: 0x0600147C RID: 5244 RVA: 0x000B7980 File Offset: 0x000B5D80
	Private Iterator Function timeout_cr() As IEnumerator
		MyBase.animator.ResetTrigger("Attack")
		MyBase.animator.ResetTrigger("OnMove")
		MyBase.animator.Play("Move")
		Me.canAttack = False
		Me.level.LeaveHole(Me.currentHole)
		Me.currentHole = -1
		Yield CupheadTime.WaitForSeconds(Me, MyBase.properties.CurrentState.secretTerriers.dogTimeOut)
		Me.hp = MyBase.properties.CurrentState.secretTerriers.dogRetreatDamage
		While Me.currentHole = -1
			Me.currentHole = Me.level.GetNextHole()
			Yield Nothing
		End While
		MyBase.StartCoroutine(Me.attack_cr())
		Return
	End Function

	' Token: 0x0600147D RID: 5245 RVA: 0x000B799B File Offset: 0x000B5D9B
	Private Sub AniEvent_PullGrenadePin()
		AudioManager.Play("sfx_dlc_dogfight_ps_terrier_pineapplepinclink")
	End Sub

	' Token: 0x0600147E RID: 5246 RVA: 0x000B79A8 File Offset: 0x000B5DA8
	Private Sub WORKAROUND_NullifyFields()
		Me.damageDealer = Nothing
		Me.bulletRoot = Nothing
		Me.bulletPrefab = Nothing
		Me.bulletPrefabPink = Nothing
		Me.level = Nothing
		Me.introNum = Nothing
		Me.coll = Nothing
		Me.leader = Nothing
		Me.rend = Nothing
		Me.backerRend = Nothing
	End Sub

	' Token: 0x04001DBE RID: 7614
	Private isDead As Boolean

	' Token: 0x04001DBF RID: 7615
	Private damageDealer As DamageDealer

	' Token: 0x04001DC0 RID: 7616
	Private damageReceiver As DamageReceiver

	' Token: 0x04001DC1 RID: 7617
	<SerializeField()>
	Private bulletRoot As Transform

	' Token: 0x04001DC2 RID: 7618
	<SerializeField()>
	Private bulletPrefab As AirplaneLevelSecretTerrierBullet

	' Token: 0x04001DC3 RID: 7619
	<SerializeField()>
	Private bulletPrefabPink As AirplaneLevelSecretTerrierBullet

	' Token: 0x04001DC4 RID: 7620
	<SerializeField()>
	Private level As AirplaneLevel

	' Token: 0x04001DC5 RID: 7621
	Private attacked As Boolean

	' Token: 0x04001DC6 RID: 7622
	Private moved As Boolean

	' Token: 0x04001DC7 RID: 7623
	Private canAttack As Boolean

	' Token: 0x04001DC8 RID: 7624
	Private hp As Single

	' Token: 0x04001DC9 RID: 7625
	<SerializeField()>
	Private currentHole As Integer

	' Token: 0x04001DCA RID: 7626
	Private introNum As Integer() = New Integer() { 1, 3, 2, 0, 4 }

	' Token: 0x04001DCB RID: 7627
	<SerializeField()>
	Private coll As Collider2D

	' Token: 0x04001DCC RID: 7628
	<SerializeField()>
	Private leader As AirplaneLevelSecretLeader

	' Token: 0x04001DCD RID: 7629
	Private firstAttack As Boolean

	' Token: 0x04001DCE RID: 7630
	Private nextAttackPink As Boolean

	' Token: 0x04001DCF RID: 7631
	<SerializeField()>
	Private rend As SpriteRenderer

	' Token: 0x04001DD0 RID: 7632
	<SerializeField()>
	Private backerRend As SpriteRenderer
End Class
