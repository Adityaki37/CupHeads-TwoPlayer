Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000819 RID: 2073
Public Class TrainLevelGhostCannonGhost
	Inherits HomingProjectile

	' Token: 0x1700041E RID: 1054
	' (get) Token: 0x0600300F RID: 12303 RVA: 0x001C6569 File Offset: 0x001C4969
	Protected Overrides ReadOnly Property DestroyLifetime As Single
		Get
			Return 1000F
		End Get
	End Property

	' Token: 0x06003010 RID: 12304 RVA: 0x001C6570 File Offset: 0x001C4970
	Public Function Create(pos As Vector3, delay As Single, speed As Single, aimSpeed As Single, health As Single, skullSpeed As Single) As TrainLevelGhostCannonGhost
		Dim trainLevelGhostCannonGhost As TrainLevelGhostCannonGhost = TryCast(MyBase.Create(pos, -90F, speed, speed, aimSpeed, Single.MaxValue, 2F, PlayerManager.GetNext()), TrainLevelGhostCannonGhost)
		trainLevelGhostCannonGhost.HomingEnabled = False
		trainLevelGhostCannonGhost.transform.position = pos
		trainLevelGhostCannonGhost.delay = delay
		trainLevelGhostCannonGhost.health = health
		trainLevelGhostCannonGhost.skullSpeed = skullSpeed
		trainLevelGhostCannonGhost.GetComponent(Of Collider2D)().enabled = False
		Return trainLevelGhostCannonGhost
	End Function

	' Token: 0x06003011 RID: 12305 RVA: 0x001C65DE File Offset: 0x001C49DE
	Protected Overrides Sub Start()
		MyBase.Start()
		Me.damageable = False
		Me.damageReceiver = MyBase.GetComponent(Of DamageReceiver)()
		AddHandler Me.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
		MyBase.StartCoroutine(Me.start_cr())
	End Sub

	' Token: 0x06003012 RID: 12306 RVA: 0x001C661D File Offset: 0x001C4A1D
	Protected Overrides Sub Update()
		MyBase.Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x06003013 RID: 12307 RVA: 0x001C663B File Offset: 0x001C4A3B
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If phase <> CollisionPhase.[Exit] Then
			Me.Die()
		End If
	End Sub

	' Token: 0x06003014 RID: 12308 RVA: 0x001C6654 File Offset: 0x001C4A54
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		If Not Me.damageable OrElse Me.health <= 0F Then
			Return
		End If
		Me.health -= info.damage
		If Me.health <= 0F Then
			Me.Die()
		End If
	End Sub

	' Token: 0x06003015 RID: 12309 RVA: 0x001C66A8 File Offset: 0x001C4AA8
	Protected Overrides Sub Die()
		AudioManager.Play("train_lollipop_cannon_ghost_death")
		Me.emitAudioFromObject.Add("train_lollipop_cannon_ghost_death")
		Me.StopAllCoroutines()
		Me.health = -1F
		Me.damageable = False
		MyBase.animator.Play("Die")
	End Sub

	' Token: 0x06003016 RID: 12310 RVA: 0x001C66F7 File Offset: 0x001C4AF7
	Private Sub DropSkull()
		Me.skullPrefab.Create(MyBase.transform.position, Me.skullSpeed)
	End Sub

	' Token: 0x06003017 RID: 12311 RVA: 0x001C6716 File Offset: 0x001C4B16
	Private Sub OnDieAnimComplete()
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub

	' Token: 0x06003018 RID: 12312 RVA: 0x001C6724 File Offset: 0x001C4B24
	Private Iterator Function start_cr() As IEnumerator
		Yield MyBase.StartCoroutine(Me.up_cr())
		Yield CupheadTime.WaitForSeconds(Me, Me.delay)
		MyBase.animator.Play("Attack")
		Me.damageable = True
		MyBase.HomingEnabled = True
		MyBase.GetComponent(Of Collider2D)().enabled = True
		Return
	End Function

	' Token: 0x06003019 RID: 12313 RVA: 0x001C6740 File Offset: 0x001C4B40
	Private Iterator Function up_cr() As IEnumerator
		Yield MyBase.TweenPositionY(MyBase.transform.position.y, 500F, 0.4F, EaseUtils.EaseType.linear)
		Return
	End Function

	' Token: 0x0600301A RID: 12314 RVA: 0x001C675B File Offset: 0x001C4B5B
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me.skullPrefab = Nothing
	End Sub

	' Token: 0x040038E5 RID: 14565
	<SerializeField()>
	Private skullPrefab As TrainLevelGhostCannonGhostSkull

	' Token: 0x040038E6 RID: 14566
	Private delay As Single

	' Token: 0x040038E7 RID: 14567
	Private health As Single

	' Token: 0x040038E8 RID: 14568
	Private skullSpeed As Single

	' Token: 0x040038E9 RID: 14569
	Private damageable As Boolean

	' Token: 0x040038EA RID: 14570
	Private damageReceiver As DamageReceiver
End Class
