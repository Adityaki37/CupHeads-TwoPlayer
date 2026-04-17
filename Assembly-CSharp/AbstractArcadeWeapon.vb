Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000A00 RID: 2560
Public MustInherit Class AbstractArcadeWeapon
	Inherits AbstractPausableComponent

	' Token: 0x1700051D RID: 1309
	' (get) Token: 0x06003C7E RID: 15486
	Protected MustOverride ReadOnly Property rapidFire As Boolean

	' Token: 0x1700051E RID: 1310
	' (get) Token: 0x06003C7F RID: 15487
	Protected MustOverride ReadOnly Property rapidFireRate As Single

	' Token: 0x1700051F RID: 1311
	' (get) Token: 0x06003C80 RID: 15488 RVA: 0x002199D1 File Offset: 0x00217DD1
	' (set) Token: 0x06003C81 RID: 15489 RVA: 0x002199D9 File Offset: 0x00217DD9
	Public Property id As Weapon

	' Token: 0x06003C82 RID: 15490 RVA: 0x002199E4 File Offset: 0x00217DE4
	Public Overridable Sub Initialize(weaponManager As ArcadePlayerWeaponManager, id As Weapon)
		Me.weaponManager = weaponManager
		Me.player = weaponManager.GetComponent(Of ArcadePlayerController)()
		Me.id = id
		Me.firing = New AbstractArcadeWeapon.FiringSwitches()
		Me.StartCoroutines()
		AddHandler Me.player.OnReviveEvent, AddressOf Me.OnRevive
	End Sub

	' Token: 0x06003C83 RID: 15491 RVA: 0x00219A34 File Offset: 0x00217E34
	Private Sub OnDealDamage(damage As Single, receiver As DamageReceiver, dealer As DamageDealer)
		If Me.player Is Nothing OrElse Me.player.IsDead OrElse Me.player.stats Is Nothing OrElse Not receiver.enabled Then
			Return
		End If
		Me.player.stats.OnDealDamage(damage, dealer)
	End Sub

	' Token: 0x06003C84 RID: 15492 RVA: 0x00219A96 File Offset: 0x00217E96
	Private Sub OnRevive(pos As Vector3)
		Me.StartCoroutines()
	End Sub

	' Token: 0x06003C85 RID: 15493 RVA: 0x00219A9E File Offset: 0x00217E9E
	Private Sub StartCoroutines()
		Me.StopAllCoroutines()
		MyBase.StartCoroutine(Me.fireWeapon_cr(AbstractArcadeWeapon.Mode.Basic))
		MyBase.StartCoroutine(Me.fireWeapon_cr(AbstractArcadeWeapon.Mode.Ex))
	End Sub

	' Token: 0x06003C86 RID: 15494 RVA: 0x00219AC2 File Offset: 0x00217EC2
	Private Sub OnEnable()
		Me.StartCoroutines()
	End Sub

	' Token: 0x06003C87 RID: 15495 RVA: 0x00219ACA File Offset: 0x00217ECA
	Public Overridable Sub BeginBasic()
		Me.beginFiring(AbstractArcadeWeapon.Mode.Basic)
	End Sub

	' Token: 0x06003C88 RID: 15496 RVA: 0x00219AD3 File Offset: 0x00217ED3
	Public Overridable Sub EndBasic()
		Me.endFiring(AbstractArcadeWeapon.Mode.Basic)
	End Sub

	' Token: 0x06003C89 RID: 15497 RVA: 0x00219ADC File Offset: 0x00217EDC
	Protected Overridable Function fireBasic() As AbstractProjectile
		Dim abstractProjectile As AbstractProjectile = Me.fireProjectile(AbstractArcadeWeapon.Mode.Basic)
		AddHandler abstractProjectile.OnDealDamageEvent, AddressOf Me.OnDealDamage
		Return abstractProjectile
	End Function

	' Token: 0x06003C8A RID: 15498 RVA: 0x00219B04 File Offset: 0x00217F04
	Public Overridable Sub BeginEx()
		Me.beginFiring(AbstractArcadeWeapon.Mode.Ex)
	End Sub

	' Token: 0x06003C8B RID: 15499 RVA: 0x00219B0D File Offset: 0x00217F0D
	Public Overridable Sub EndEx()
		Me.endFiring(AbstractArcadeWeapon.Mode.Ex)
	End Sub

	' Token: 0x06003C8C RID: 15500 RVA: 0x00219B18 File Offset: 0x00217F18
	Protected Overridable Function fireEx() As AbstractProjectile
		Return Me.fireProjectile(AbstractArcadeWeapon.Mode.Ex)
	End Function

	' Token: 0x06003C8D RID: 15501 RVA: 0x00219B2E File Offset: 0x00217F2E
	Protected Overridable Sub beginFiring(mode As AbstractArcadeWeapon.Mode)
		Me.weaponManager.IsShooting = True
		Me.firing.[Set](mode, True)
	End Sub

	' Token: 0x06003C8E RID: 15502 RVA: 0x00219B4C File Offset: 0x00217F4C
	Protected Overridable Function fireProjectile(mode As AbstractArcadeWeapon.Mode) As AbstractProjectile
		Dim vector As Vector2 = Me.weaponManager.GetBulletPosition()
		If mode = AbstractArcadeWeapon.Mode.Ex Then
			vector = Me.weaponManager.ExPosition
		End If
		If mode = AbstractArcadeWeapon.Mode.Basic Then
			Me.weaponManager.UpdateAim()
		End If
		If Me.GetProjectile(mode) Is Nothing Then
			Return Nothing
		End If
		If Me.GetEffect(mode) IsNot Nothing Then
			If mode <> AbstractArcadeWeapon.Mode.Basic AndAlso mode = AbstractArcadeWeapon.Mode.Ex Then
				Me.weaponManager.CreateExDust(Me.GetEffect(mode))
			End If
		End If
		Me.weaponManager.UpdateAim()
		Return Me.GetProjectile(mode).Create(vector, Me.weaponManager.GetBulletRotation(), Me.weaponManager.GetBulletScale())
	End Function

	' Token: 0x06003C8F RID: 15503 RVA: 0x00219C12 File Offset: 0x00218012
	Protected Overridable Sub endFiring(mode As AbstractArcadeWeapon.Mode)
		Me.weaponManager.IsShooting = False
		Me.firing.[Set](mode, False)
	End Sub

	' Token: 0x06003C90 RID: 15504 RVA: 0x00219C2D File Offset: 0x0021802D
	Private Function GetProjectile(mode As AbstractArcadeWeapon.Mode) As AbstractProjectile
		If mode = AbstractArcadeWeapon.Mode.Basic OrElse mode <> AbstractArcadeWeapon.Mode.Ex Then
			Return Me.basicPrefab
		End If
		Return Me.exPrefab
	End Function

	' Token: 0x06003C91 RID: 15505 RVA: 0x00219C4E File Offset: 0x0021804E
	Private Function GetEffect(mode As AbstractArcadeWeapon.Mode) As Effect
		If mode = AbstractArcadeWeapon.Mode.Basic OrElse mode <> AbstractArcadeWeapon.Mode.Ex Then
			Return Me.basicEffectPrefab
		End If
		Return Me.exEffectPrefab
	End Function

	' Token: 0x06003C92 RID: 15506 RVA: 0x00219C6F File Offset: 0x0021806F
	Private Function getFiringMethod(mode As AbstractArcadeWeapon.Mode) As AbstractArcadeWeapon.FireProjectileDelegate
		If mode <> AbstractArcadeWeapon.Mode.Ex Then
			If mode <> AbstractArcadeWeapon.Mode.Basic Then
			End If
			Return AddressOf Me.fireBasic
		End If
		Return AddressOf Me.fireEx
	End Function

	' Token: 0x06003C93 RID: 15507 RVA: 0x00219CA0 File Offset: 0x002180A0
	Protected Overridable Iterator Function fireWeapon_cr(mode As AbstractArcadeWeapon.Mode) As IEnumerator
		Dim t As Single = 0F
		Dim waitInstruction As WaitForFixedUpdate = New WaitForFixedUpdate()
		While True
			Yield waitInstruction
			If Not Me.player.motor.Dashing Then
				If t < Me.rapidFireRate Then
					t += CupheadTime.FixedDelta
				ElseIf Me.firing.[Get](mode) AndAlso Me.weaponManager.IsShooting Then
					Me.weaponManager.TriggerWeaponFire()
					Me.getFiringMethod(mode)()
					If mode = AbstractArcadeWeapon.Mode.Ex OrElse Not Me.rapidFire Then
						Me.firing.[Set](mode, False)
						Me.weaponManager.IsShooting = False
					End If
					t = 0F
				End If
			End If
		End While
		Return
	End Function

	' Token: 0x040043E1 RID: 17377
	<Header("Ex")>
	<SerializeField()>
	Protected exPrefab As AbstractProjectile

	' Token: 0x040043E2 RID: 17378
	<SerializeField()>
	Protected exEffectPrefab As Effect

	' Token: 0x040043E3 RID: 17379
	<Header("Basic")>
	<SerializeField()>
	Protected basicPrefab As AbstractProjectile

	' Token: 0x040043E4 RID: 17380
	<SerializeField()>
	Protected basicEffectPrefab As Effect

	' Token: 0x040043E6 RID: 17382
	Protected firing As AbstractArcadeWeapon.FiringSwitches

	' Token: 0x040043E7 RID: 17383
	Protected player As ArcadePlayerController

	' Token: 0x040043E8 RID: 17384
	Protected weaponManager As ArcadePlayerWeaponManager

	' Token: 0x02000A01 RID: 2561
	Public Enum Mode
		' Token: 0x040043EA RID: 17386
		Basic
		' Token: 0x040043EB RID: 17387
		Ex
	End Enum

	' Token: 0x02000A02 RID: 2562
	' (Invoke) Token: 0x06003C95 RID: 15509
	Private Delegate Function FireProjectileDelegate() As AbstractProjectile

	' Token: 0x02000A03 RID: 2563
	<Serializable()>
	Public Class Prefabs
		' Token: 0x06003C99 RID: 15513 RVA: 0x00219CCA File Offset: 0x002180CA
		Public Function [Get](mode As AbstractArcadeWeapon.Mode) As AbstractProjectile
			If mode <> AbstractArcadeWeapon.Mode.Ex Then
				If mode <> AbstractArcadeWeapon.Mode.Basic Then
				End If
				Return Me.basic
			End If
			Return Me.ex
		End Function

		' Token: 0x040043EC RID: 17388
		Public basic As AbstractProjectile

		' Token: 0x040043ED RID: 17389
		Public ex As AbstractProjectile
	End Class

	' Token: 0x02000A04 RID: 2564
	<Serializable()>
	Public Class MuzzleEffects
		' Token: 0x06003C9B RID: 15515 RVA: 0x00219CF3 File Offset: 0x002180F3
		Public Function [Get](mode As AbstractArcadeWeapon.Mode) As Effect
			If mode <> AbstractArcadeWeapon.Mode.Ex Then
				If mode <> AbstractArcadeWeapon.Mode.Basic Then
				End If
				Return Me.basic
			End If
			Return Me.ex
		End Function

		' Token: 0x040043EE RID: 17390
		Public basic As Effect

		' Token: 0x040043EF RID: 17391
		Public ex As Effect
	End Class

	' Token: 0x02000A05 RID: 2565
	Public Class FiringSwitches
		' Token: 0x06003C9D RID: 15517 RVA: 0x00219D1C File Offset: 0x0021811C
		Public Function [Get](mode As AbstractArcadeWeapon.Mode) As Boolean
			If mode <> AbstractArcadeWeapon.Mode.Ex Then
				If mode <> AbstractArcadeWeapon.Mode.Basic Then
				End If
				Return Me.basic
			End If
			Return Me.ex
		End Function

		' Token: 0x06003C9E RID: 15518 RVA: 0x00219D3D File Offset: 0x0021813D
		Public Sub [Set](mode As AbstractArcadeWeapon.Mode, val As Boolean)
			If mode <> AbstractArcadeWeapon.Mode.Ex Then
				If mode <> AbstractArcadeWeapon.Mode.Basic Then
				End If
				Me.basic = val
			Else
				Me.ex = val
			End If
		End Sub

		' Token: 0x040043F0 RID: 17392
		Public basic As Boolean

		' Token: 0x040043F1 RID: 17393
		Public ex As Boolean
	End Class
End Class
