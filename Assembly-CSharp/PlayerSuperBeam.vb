Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports UnityEngine

' Token: 0x02000A4F RID: 2639
Public Class PlayerSuperBeam
	Inherits AbstractPlayerSuper

	' Token: 0x06003EDC RID: 16092 RVA: 0x00226BB3 File Offset: 0x00224FB3
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.damageReceivers = New List(Of DamageReceiver)()
	End Sub

	' Token: 0x06003EDD RID: 16093 RVA: 0x00226BC6 File Offset: 0x00224FC6
	Protected Overrides Sub StartSuper()
		MyBase.StartSuper()
		AudioManager.Play("player_super_beam_start")
		MyBase.StartCoroutine(Me.super_cr())
	End Sub

	' Token: 0x06003EDE RID: 16094 RVA: 0x00226BE8 File Offset: 0x00224FE8
	Protected Overrides Sub OnCollision(hit As GameObject, phase As CollisionPhase)
		Dim component As DamageReceiver = hit.GetComponent(Of DamageReceiver)()
		If component IsNot Nothing Then
			If Me.damageReceivers.Contains(component) Then
				Return
			End If
			Me.damageReceivers.Add(component)
		End If
	End Sub

	' Token: 0x06003EDF RID: 16095 RVA: 0x00226C28 File Offset: 0x00225028
	Private Sub OnDealDamage(damage As Single, receiver As DamageReceiver, dealer As DamageDealer)
		Dim componentInChildren As Collider2D = receiver.GetComponentInChildren(Of Collider2D)()
		Dim vector As Vector2 = Vector2.zero
		Dim zero As Vector2 = Vector2.zero
		If componentInChildren.[GetType]() Is GetType(BoxCollider2D) Then
			vector = TryCast(componentInChildren, BoxCollider2D).size
		Else
			If componentInChildren.[GetType]() IsNot GetType(CircleCollider2D) Then
				Return
			End If
			vector = Vector2.one * TryCast(componentInChildren, CircleCollider2D).radius
		End If
		Dim num As Single = receiver.transform.position.x + Global.UnityEngine.Random.Range(-vector.x / 2F, vector.x / 2F)
		zero = New Vector2(num, MyBase.transform.position.y + CSng(Global.UnityEngine.Random.Range(-100, 100)))
		Me.hitPrefab.Create(zero)
	End Sub

	' Token: 0x06003EE0 RID: 16096 RVA: 0x00226D10 File Offset: 0x00225110
	Private Iterator Function super_cr() As IEnumerator
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Start", False, True)
		Me.Fire()
		Yield CupheadTime.WaitForSeconds(Me, WeaponProperties.LevelSuperBeam.time)
		MyBase.animator.SetTrigger("OnEnd")
		AudioManager.Play("player_super_beam_end_ground")
		AudioManager.[Stop]("player_superbeam_firing_loop")
		Me.EndSuper(True)
		Return
	End Function

	' Token: 0x06003EE1 RID: 16097 RVA: 0x00226D2C File Offset: 0x0022512C
	Protected Overrides Sub Fire()
		MyBase.Fire()
		AudioManager.Play("player_superbeam_firing_loop")
		AudioManager.Play("player_superbeam_milk_explosion")
		Me.damageDealer = New DamageDealer(WeaponProperties.LevelSuperBeam.damage, WeaponProperties.LevelSuperBeam.damageRate, DamageDealer.DamageSource.Super, False, True, True)
		AddHandler Me.damageDealer.OnDealDamage, AddressOf Me.OnDealDamage
		Me.damageDealer.DamageMultiplier *= PlayerManager.DamageMultiplier
		Me.damageDealer.PlayerId = Me.player.id
		Dim meterScoreTracker As MeterScoreTracker = New MeterScoreTracker(MeterScoreTracker.Type.Super)
		meterScoreTracker.Add(Me.damageDealer)
	End Sub

	' Token: 0x06003EE2 RID: 16098 RVA: 0x00226DC3 File Offset: 0x002251C3
	Private Sub OnEndAnimComplete()
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub

	' Token: 0x06003EE3 RID: 16099 RVA: 0x00226DD0 File Offset: 0x002251D0
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me.hitPrefab = Nothing
		Me.damageReceivers.Clear()
		Me.damageReceivers = Nothing
	End Sub

	' Token: 0x040045DD RID: 17885
	<Header("Effects")>
	<SerializeField()>
	Private hitPrefab As Effect

	' Token: 0x040045DE RID: 17886
	Private damageReceivers As List(Of DamageReceiver)
End Class
