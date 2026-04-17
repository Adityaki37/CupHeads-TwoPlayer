Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020004C1 RID: 1217
Public Class AirplaneLevelRocket
	Inherits HomingProjectile

	' Token: 0x06001454 RID: 5204 RVA: 0x000B64A0 File Offset: 0x000B48A0
	Public Function Create(player As AbstractPlayerController, pos As Vector2, speed As Single, rotationSpeed As Single, health As Single, homingTime As Single) As AirplaneLevelRocket
		Dim airplaneLevelRocket As AirplaneLevelRocket = TryCast(MyBase.Create(pos, -90F, speed, speed, rotationSpeed, Me.DestroyLifetime, 0F, player), AirplaneLevelRocket)
		airplaneLevelRocket.DamagesType.OnlyPlayer()
		airplaneLevelRocket.Init(health)
		airplaneLevelRocket.homingTimer = homingTime
		Return airplaneLevelRocket
	End Function

	' Token: 0x06001455 RID: 5205 RVA: 0x000B64EC File Offset: 0x000B48EC
	Protected Overrides Sub Start()
		MyBase.Start()
		MyBase.StartCoroutine(Me.sfx_rocket_spawn_cr())
		MyBase.StartCoroutine(Me.spawn_effect_cr())
	End Sub

	' Token: 0x06001456 RID: 5206 RVA: 0x000B650E File Offset: 0x000B490E
	Private Sub Init(health As Single)
		Me.health = health
		AddHandler MyBase.GetComponent(Of DamageReceiver)().OnDamageTaken, AddressOf Me.OnDamageTaken
	End Sub

	' Token: 0x06001457 RID: 5207 RVA: 0x000B652E File Offset: 0x000B492E
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		AudioManager.Play("sfx_DLC_Dogfight_P1_HydrantMissile_Impact")
	End Sub

	' Token: 0x06001458 RID: 5208 RVA: 0x000B6544 File Offset: 0x000B4944
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		If Me.health <= 0F Then
			Return
		End If
		Me.health -= info.damage
		If Me.health <= 0F Then
			Level.Current.RegisterMinionKilled()
			Me.Die()
		End If
	End Sub

	' Token: 0x06001459 RID: 5209 RVA: 0x000B6598 File Offset: 0x000B4998
	Private Iterator Function continue_without_homing_cr() As IEnumerator
		Dim wait As YieldInstruction = New WaitForFixedUpdate()
		While True
			MyBase.transform.AddPosition(Me.velocity.x * CupheadTime.FixedDelta, Me.velocity.y * CupheadTime.FixedDelta, 0F)
			Yield wait
		End While
		Return
	End Function

	' Token: 0x0600145A RID: 5210 RVA: 0x000B65B4 File Offset: 0x000B49B4
	Protected Overrides Sub Die()
		MyBase.Die()
		Me.StopAllCoroutines()
		MyBase.GetComponent(Of Collider2D)().enabled = False
		Me.sprite.GetComponent(Of SpriteRenderer)().enabled = False
		Dim gameObject As GameObject = GameObject.Find("BullDogPlane")
		If gameObject AndAlso Mathf.Abs(gameObject.transform.position.x - MyBase.transform.position.x) < 800F AndAlso Mathf.Abs(gameObject.transform.position.y - MyBase.transform.position.y) < 175F Then
			Me.deathOnPlaneFX.Create(MyBase.transform.position)
		Else
			Me.deathFX.Create(MyBase.transform.position)
		End If
		AudioManager.Play("sfx_DLC_Dogfight_P1_HydrantMissile_DeathExplode")
	End Sub

	' Token: 0x0600145B RID: 5211 RVA: 0x000B66AC File Offset: 0x000B4AAC
	Private Iterator Function spawn_effect_cr() As IEnumerator
		While True
			Yield CupheadTime.WaitForSeconds(Me, Me.fxSpawnRate.RandomFloat())
			Me.effectFX.Create(Me.effectRoot.position)
			AudioManager.Play("sfx_DLC_Dogfight_P1_HydrantMissile_Chuff")
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x0600145C RID: 5212 RVA: 0x000B66C8 File Offset: 0x000B4AC8
	Protected Overrides Sub Update()
		MyBase.Update()
		If Me.homingTimer > 0F Then
			Me.homingTimer -= CupheadTime.Delta
			If Me.homingTimer <= 0F Then
				Me.StopAllCoroutines()
				MyBase.StartCoroutine(Me.continue_without_homing_cr())
			End If
		End If
	End Sub

	' Token: 0x0600145D RID: 5213 RVA: 0x000B6728 File Offset: 0x000B4B28
	Private Iterator Function sfx_rocket_spawn_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 0.5F)
		AudioManager.Play("sfx_DLC_Dogfight_P1_HydrantMissile_Entrance")
		Return
	End Function

	' Token: 0x04001DA3 RID: 7587
	Private homingTimer As Single

	' Token: 0x04001DA4 RID: 7588
	Private health As Single

	' Token: 0x04001DA5 RID: 7589
	<SerializeField()>
	Private effectRoot As Transform

	' Token: 0x04001DA6 RID: 7590
	<SerializeField()>
	Private effectFX As Effect

	' Token: 0x04001DA7 RID: 7591
	<SerializeField()>
	Private deathFX As Effect

	' Token: 0x04001DA8 RID: 7592
	<SerializeField()>
	Private deathOnPlaneFX As Effect

	' Token: 0x04001DA9 RID: 7593
	<SerializeField()>
	Private sprite As SpriteRenderer

	' Token: 0x04001DAA RID: 7594
	<SerializeField()>
	Private fxSpawnRate As MinMax
End Class
