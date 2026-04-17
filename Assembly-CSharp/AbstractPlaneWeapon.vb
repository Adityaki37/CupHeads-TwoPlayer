Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000AAE RID: 2734
Public MustInherit Class AbstractPlaneWeapon
	Inherits AbstractPausableComponent

	' Token: 0x170005BE RID: 1470
	' (get) Token: 0x060041AE RID: 16814
	Protected MustOverride ReadOnly Property rapidFire As Boolean

	' Token: 0x170005BF RID: 1471
	' (get) Token: 0x060041AF RID: 16815
	Protected MustOverride ReadOnly Property rapidFireRate As Single

	' Token: 0x170005C0 RID: 1472
	' (get) Token: 0x060041B0 RID: 16816 RVA: 0x00238E1F File Offset: 0x0023721F
	' (set) Token: 0x060041B1 RID: 16817 RVA: 0x00238E27 File Offset: 0x00237227
	Public Property index As Integer

	' Token: 0x060041B2 RID: 16818 RVA: 0x00238E30 File Offset: 0x00237230
	Public Overridable Sub Initialize(weaponManager As PlanePlayerWeaponManager, index As Integer)
		Me.weaponManager = weaponManager
		Me.player = weaponManager.GetComponent(Of PlanePlayerController)()
		Me.index = index
		Me.firing = New AbstractPlaneWeapon.FiringSwitches()
		MyBase.StartCoroutine(Me.fireWeapon_cr(AbstractPlaneWeapon.Mode.Basic))
		MyBase.StartCoroutine(Me.fireWeapon_cr(AbstractPlaneWeapon.Mode.Ex))
		MyBase.StartCoroutine(Me.endFiringAnimation_cr())
		AddHandler Me.player.OnReviveEvent, AddressOf Me.OnRevive
	End Sub

	' Token: 0x060041B3 RID: 16819 RVA: 0x00238EA2 File Offset: 0x002372A2
	Private Sub OnRevive(pos As Vector3)
		MyBase.StartCoroutine(Me.fireWeapon_cr(AbstractPlaneWeapon.Mode.Basic))
		MyBase.StartCoroutine(Me.fireWeapon_cr(AbstractPlaneWeapon.Mode.Ex))
		MyBase.StartCoroutine(Me.endFiringAnimation_cr())
	End Sub

	' Token: 0x060041B4 RID: 16820 RVA: 0x00238ED0 File Offset: 0x002372D0
	Private Sub OnDealDamage(damage As Single, receiver As DamageReceiver, dealer As DamageDealer)
		If Me.player Is Nothing OrElse Me.player.IsDead OrElse Me.player.stats Is Nothing OrElse Not receiver.enabled Then
			Return
		End If
		Me.player.stats.OnDealDamage(damage, dealer)
	End Sub

	' Token: 0x060041B5 RID: 16821 RVA: 0x00238F32 File Offset: 0x00237332
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me.exPrefab = Nothing
		Me.exEffectPrefab = Nothing
		Me.basicPrefab = Nothing
		Me.basicEffectPrefab = Nothing
		Me.shrunkPrefab = Nothing
	End Sub

	' Token: 0x060041B6 RID: 16822 RVA: 0x00238F5D File Offset: 0x0023735D
	Public Overridable Sub BeginBasic()
		Me.beginFiring(AbstractPlaneWeapon.Mode.Basic)
	End Sub

	' Token: 0x060041B7 RID: 16823 RVA: 0x00238F66 File Offset: 0x00237366
	Public Overridable Sub EndBasic()
		Me.endFiring(AbstractPlaneWeapon.Mode.Basic)
	End Sub

	' Token: 0x060041B8 RID: 16824 RVA: 0x00238F70 File Offset: 0x00237370
	Protected Overridable Function fireBasic() As AbstractProjectile
		Dim abstractProjectile As AbstractProjectile = Me.fireProjectile(AbstractPlaneWeapon.Mode.Basic)
		abstractProjectile.PlayerId = Me.player.id
		AddHandler abstractProjectile.OnDealDamageEvent, AddressOf Me.OnDealDamage
		Return abstractProjectile
	End Function

	' Token: 0x060041B9 RID: 16825 RVA: 0x00238FA9 File Offset: 0x002373A9
	Public Overridable Sub BeginEx()
		Me.beginFiring(AbstractPlaneWeapon.Mode.Ex)
	End Sub

	' Token: 0x060041BA RID: 16826 RVA: 0x00238FB2 File Offset: 0x002373B2
	Public Overridable Sub EndEx()
		Me.endFiring(AbstractPlaneWeapon.Mode.Ex)
	End Sub

	' Token: 0x060041BB RID: 16827 RVA: 0x00238FBC File Offset: 0x002373BC
	Protected Overridable Function fireEx() As AbstractProjectile
		Return Me.fireProjectile(AbstractPlaneWeapon.Mode.Ex)
	End Function

	' Token: 0x060041BC RID: 16828 RVA: 0x00238FD2 File Offset: 0x002373D2
	Protected Overridable Sub beginFiring(mode As AbstractPlaneWeapon.Mode)
		MyBase.StopCoroutine("endFiringAnimation_cr")
		Me.weaponManager.IsShooting = True
		Me.firing.[Set](mode, True)
	End Sub

	' Token: 0x060041BD RID: 16829 RVA: 0x00238FF8 File Offset: 0x002373F8
	Protected Overridable Function fireProjectile(mode As AbstractPlaneWeapon.Mode) As AbstractProjectile
		Dim vector As Vector2 = Me.weaponManager.GetBulletPosition() + New Vector2(-10F, 0F) + New Vector2(Global.UnityEngine.Random.Range(-5F, 5F), Global.UnityEngine.Random.Range(-5F, 5F))
		If Me.GetProjectile(mode) Is Nothing Then
			Return Nothing
		End If
		If Me.GetEffect(mode) IsNot Nothing Then
			If mode = AbstractPlaneWeapon.Mode.Basic OrElse mode <> AbstractPlaneWeapon.Mode.Ex Then
				Me.basicEffectPrefab.Create(vector, MyBase.transform.localScale).transform.SetParent(MyBase.transform)
			End If
		End If
		Dim abstractProjectile As AbstractProjectile = Me.GetProjectile(mode).Create(vector)
		If mode = AbstractPlaneWeapon.Mode.Ex Then
			abstractProjectile.DamageSource = DamageDealer.DamageSource.Ex
			CupheadLevelCamera.Current.Shake(5F, 0.5F, False)
		End If
		abstractProjectile.PlayerId = Me.player.id
		Return abstractProjectile
	End Function

	' Token: 0x060041BE RID: 16830 RVA: 0x002390FE File Offset: 0x002374FE
	Protected Overridable Sub endFiring(mode As AbstractPlaneWeapon.Mode)
		Me.weaponManager.IsShooting = False
		Me.firing.[Set](mode, False)
	End Sub

	' Token: 0x060041BF RID: 16831 RVA: 0x00239119 File Offset: 0x00237519
	Private Function GetProjectile(mode As AbstractPlaneWeapon.Mode) As AbstractProjectile
		If mode <> AbstractPlaneWeapon.Mode.Basic AndAlso mode = AbstractPlaneWeapon.Mode.Ex Then
			Return Me.exPrefab
		End If
		If Me.player.Shrunk Then
			Return Me.shrunkPrefab
		End If
		Return Me.basicPrefab
	End Function

	' Token: 0x060041C0 RID: 16832 RVA: 0x00239151 File Offset: 0x00237551
	Protected Overridable Function GetEffect(mode As AbstractPlaneWeapon.Mode) As Effect
		If mode = AbstractPlaneWeapon.Mode.Basic OrElse mode <> AbstractPlaneWeapon.Mode.Ex Then
			Return Me.basicEffectPrefab
		End If
		Return Me.exEffectPrefab
	End Function

	' Token: 0x060041C1 RID: 16833 RVA: 0x00239172 File Offset: 0x00237572
	Private Function getFiringMethod(mode As AbstractPlaneWeapon.Mode) As AbstractPlaneWeapon.FireProjectileDelegate
		If mode <> AbstractPlaneWeapon.Mode.Ex Then
			If mode <> AbstractPlaneWeapon.Mode.Basic Then
			End If
			Return AddressOf Me.fireBasic
		End If
		Return AddressOf Me.fireEx
	End Function

	' Token: 0x060041C2 RID: 16834 RVA: 0x002391A4 File Offset: 0x002375A4
	Private Iterator Function fireWeapon_cr(mode As AbstractPlaneWeapon.Mode) As IEnumerator
		Dim time As Single = Me.rapidFireRate
		Dim waitInstruction As WaitForFixedUpdate = New WaitForFixedUpdate()
		While True
			Yield waitInstruction
			If mode = AbstractPlaneWeapon.Mode.Basic AndAlso Me.t < time Then
				If Me.weaponManager.CurrentWeapon Is Me Then
					Me.t += CupheadTime.FixedDelta
				End If
			ElseIf Me.firing.[Get](mode) Then
				Me.getFiringMethod(mode)()
				If mode = AbstractPlaneWeapon.Mode.Ex OrElse Not Me.rapidFire Then
					Me.firing.[Set](mode, False)
					MyBase.StartCoroutine(Me.endFiringAnimation_cr())
				End If
				Me.t = 0F
			End If
		End While
		Return
	End Function

	' Token: 0x060041C3 RID: 16835 RVA: 0x002391C8 File Offset: 0x002375C8
	Private Iterator Function endFiringAnimation_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 0.16666667F)
		Me.weaponManager.IsShooting = False
		Return
	End Function

	' Token: 0x04004826 RID: 18470
	Private Const ANIMATION_FRAMES As Integer = 10

	' Token: 0x04004827 RID: 18471
	<Header("Ex")>
	<SerializeField()>
	Protected exPrefab As AbstractProjectile

	' Token: 0x04004828 RID: 18472
	<SerializeField()>
	Protected exEffectPrefab As Effect

	' Token: 0x04004829 RID: 18473
	<Header("Basic")>
	<SerializeField()>
	Protected basicPrefab As AbstractProjectile

	' Token: 0x0400482A RID: 18474
	<SerializeField()>
	Protected basicEffectPrefab As Effect

	' Token: 0x0400482B RID: 18475
	<Header("Shrunk")>
	<SerializeField()>
	Protected shrunkPrefab As AbstractProjectile

	' Token: 0x0400482C RID: 18476
	<SerializeField()>
	Protected shrunkDamageMultiplier As Single = 0.5F

	' Token: 0x0400482E RID: 18478
	Protected firing As AbstractPlaneWeapon.FiringSwitches

	' Token: 0x0400482F RID: 18479
	Protected player As PlanePlayerController

	' Token: 0x04004830 RID: 18480
	Protected weaponManager As PlanePlayerWeaponManager

	' Token: 0x04004831 RID: 18481
	Private t As Single = 1000F

	' Token: 0x02000AAF RID: 2735
	Public Enum Mode
		' Token: 0x04004833 RID: 18483
		Basic
		' Token: 0x04004834 RID: 18484
		Ex
	End Enum

	' Token: 0x02000AB0 RID: 2736
	' (Invoke) Token: 0x060041C5 RID: 16837
	Private Delegate Function FireProjectileDelegate() As AbstractProjectile

	' Token: 0x02000AB1 RID: 2737
	<Serializable()>
	Public Class Prefabs
		' Token: 0x060041C9 RID: 16841 RVA: 0x002391EB File Offset: 0x002375EB
		Public Function [Get](mode As AbstractPlaneWeapon.Mode) As AbstractProjectile
			If mode <> AbstractPlaneWeapon.Mode.Ex Then
				If mode <> AbstractPlaneWeapon.Mode.Basic Then
				End If
				Return Me.basic
			End If
			Return Me.ex
		End Function

		' Token: 0x04004835 RID: 18485
		Public basic As AbstractProjectile

		' Token: 0x04004836 RID: 18486
		Public ex As AbstractProjectile
	End Class

	' Token: 0x02000AB2 RID: 2738
	<Serializable()>
	Public Class MuzzleEffects
		' Token: 0x060041CB RID: 16843 RVA: 0x00239214 File Offset: 0x00237614
		Public Function [Get](mode As AbstractPlaneWeapon.Mode) As Effect
			If mode <> AbstractPlaneWeapon.Mode.Ex Then
				If mode <> AbstractPlaneWeapon.Mode.Basic Then
				End If
				Return Me.basic
			End If
			Return Me.ex
		End Function

		' Token: 0x04004837 RID: 18487
		Public basic As Effect

		' Token: 0x04004838 RID: 18488
		Public ex As Effect
	End Class

	' Token: 0x02000AB3 RID: 2739
	Public Class FiringSwitches
		' Token: 0x060041CD RID: 16845 RVA: 0x0023923D File Offset: 0x0023763D
		Public Function [Get](mode As AbstractPlaneWeapon.Mode) As Boolean
			If mode <> AbstractPlaneWeapon.Mode.Ex Then
				If mode <> AbstractPlaneWeapon.Mode.Basic Then
				End If
				Return Me.basic
			End If
			Return Me.ex
		End Function

		' Token: 0x060041CE RID: 16846 RVA: 0x0023925E File Offset: 0x0023765E
		Public Sub [Set](mode As AbstractPlaneWeapon.Mode, val As Boolean)
			If mode <> AbstractPlaneWeapon.Mode.Ex Then
				If mode <> AbstractPlaneWeapon.Mode.Basic Then
				End If
				Me.basic = val
			Else
				Me.ex = val
			End If
		End Sub

		' Token: 0x04004839 RID: 18489
		Public basic As Boolean

		' Token: 0x0400483A RID: 18490
		Public ex As Boolean
	End Class
End Class
