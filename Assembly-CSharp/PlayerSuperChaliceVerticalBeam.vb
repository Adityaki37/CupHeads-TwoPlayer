Imports System
Imports System.Collections.Generic
Imports UnityEngine

' Token: 0x02000A58 RID: 2648
Public Class PlayerSuperChaliceVerticalBeam
	Inherits AbstractPlayerSuper

	' Token: 0x06003F1A RID: 16154 RVA: 0x00228E63 File Offset: 0x00227263
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.damageReceivers = New List(Of DamageReceiver)()
	End Sub

	' Token: 0x06003F1B RID: 16155 RVA: 0x00228E78 File Offset: 0x00227278
	Protected Overrides Sub Update()
		MyBase.Update()
		If Me.updateStraw Then
			Me.UpdateStraw()
		End If
		If Me.player Is Nothing Then
			Me.Interrupt()
		Else
			Me.player.transform.position = MyBase.transform.position
		End If
	End Sub

	' Token: 0x06003F1C RID: 16156 RVA: 0x00228ED4 File Offset: 0x002272D4
	Protected Overrides Sub OnCollision(hit As GameObject, phase As CollisionPhase)
		Dim component As DamageReceiver = hit.GetComponent(Of DamageReceiver)()
		If component IsNot Nothing Then
			If Me.damageReceivers.Contains(component) Then
				Return
			End If
			Me.damageReceivers.Add(component)
		End If
	End Sub

	' Token: 0x06003F1D RID: 16157 RVA: 0x00228F14 File Offset: 0x00227314
	Private Sub OnDealDamage(damage As Single, receiver As DamageReceiver, dealer As DamageDealer)
		Dim componentInChildren As Collider2D = receiver.GetComponentInChildren(Of Collider2D)()
		Dim vector As Vector2 = Vector2.zero
		Dim vector2 As Vector2 = Vector2.zero
		If componentInChildren.[GetType]() Is GetType(BoxCollider2D) Then
			vector = TryCast(componentInChildren, BoxCollider2D).size
		Else
			If componentInChildren.[GetType]() IsNot GetType(CircleCollider2D) Then
				Return
			End If
			vector = Vector2.one * TryCast(componentInChildren, CircleCollider2D).radius
		End If
		vector2 = New Vector2(componentInChildren.transform.position.x + Global.UnityEngine.Random.Range(-vector.x / 2F, vector.x / 2F), componentInChildren.transform.position.y + Global.UnityEngine.Random.Range(-vector.y / 2F, vector.y / 2F))
		vector2 += componentInChildren.offset
		Me.hitPrefab.Create(vector2)
	End Sub

	' Token: 0x06003F1E RID: 16158 RVA: 0x0022901C File Offset: 0x0022741C
	Protected Overrides Sub Fire()
		AudioManager.Play("player_super_chalice_superbeam")
		MyBase.Fire()
		Me.damageDealer = New DamageDealer(WeaponProperties.LevelSuperChaliceVertBeam.damage, WeaponProperties.LevelSuperChaliceVertBeam.damageRate, DamageDealer.DamageSource.Super, False, True, True)
		AddHandler Me.damageDealer.OnDealDamage, AddressOf Me.OnDealDamage
		Me.damageDealer.DamageMultiplier *= PlayerManager.DamageMultiplier
		Me.damageDealer.PlayerId = Me.player.id
		Dim meterScoreTracker As MeterScoreTracker = New MeterScoreTracker(MeterScoreTracker.Type.Super)
		meterScoreTracker.Add(Me.damageDealer)
	End Sub

	' Token: 0x06003F1F RID: 16159 RVA: 0x002290AC File Offset: 0x002274AC
	Protected Overrides Sub StartSuper()
		If Me.player Is Nothing Then
			Return
		End If
		RemoveHandler Me.player.weaponManager.OnSuperStart, AddressOf Me.player.motor.StartSuper
		RemoveHandler Me.player.weaponManager.OnSuperEnd, AddressOf Me.player.motor.OnSuperEnd
		MyBase.StartSuper()
		Dim text As String = If((Not Me.player.motor.Grounded), "_Air", String.Empty)
		MyBase.animator.Play("Vert_Beam_Loop" + text)
		AudioManager.Play("player_super_chalice_superbeam_start")
	End Sub

	' Token: 0x06003F20 RID: 16160 RVA: 0x00229162 File Offset: 0x00227562
	Private Sub AnimationDone()
		If Me.player IsNot Nothing Then
			Me.player.motor.CheckForPostSuperHop()
		End If
		Me.EndSuper(True)
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub

	' Token: 0x06003F21 RID: 16161 RVA: 0x00229198 File Offset: 0x00227598
	Private Sub UpdateStraw()
		Dim main As Camera = Camera.main
		Dim transform As Transform = main.transform
		Me.StrawFX.transform.SetPosition(Nothing, New Single?(transform.position.y + -200F * MyBase.transform.localScale.y), Nothing)
	End Sub

	' Token: 0x06003F22 RID: 16162 RVA: 0x00229202 File Offset: 0x00227602
	Private Sub Ani_LockStraw()
		Me.updateStraw = True
	End Sub

	' Token: 0x06003F23 RID: 16163 RVA: 0x0022920B File Offset: 0x0022760B
	Private Sub Ani_UnLockStraw()
		Me.updateStraw = False
		AudioManager.Play("player_super_chalice_superbeam_end")
	End Sub

	' Token: 0x06003F24 RID: 16164 RVA: 0x0022921E File Offset: 0x0022761E
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me.hitPrefab = Nothing
		Me.damageReceivers.Clear()
		Me.damageReceivers = Nothing
	End Sub

	' Token: 0x04004632 RID: 17970
	Private Const STRAW_Y_OFFSET As Single = -200F

	' Token: 0x04004633 RID: 17971
	<Header("Effects")>
	<SerializeField()>
	Private hitPrefab As Effect

	' Token: 0x04004634 RID: 17972
	<SerializeField()>
	Private StrawFX As GameObject

	' Token: 0x04004635 RID: 17973
	Private updateStraw As Boolean

	' Token: 0x04004636 RID: 17974
	Private damageReceivers As List(Of DamageReceiver)
End Class
