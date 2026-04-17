Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000A5D RID: 2653
Public MustInherit Class AbstractLevelWeapon
	Inherits AbstractPausableComponent

	' Token: 0x1700056F RID: 1391
	' (get) Token: 0x06003F3D RID: 16189 RVA: 0x0022A0E3 File Offset: 0x002284E3
	' (set) Token: 0x06003F3E RID: 16190 RVA: 0x0022A0EA File Offset: 0x002284EA
	Public Shared Property ONE_PLAYER_FIRING As Boolean

	' Token: 0x17000570 RID: 1392
	' (get) Token: 0x06003F3F RID: 16191
	Protected MustOverride ReadOnly Property rapidFire As Boolean

	' Token: 0x17000571 RID: 1393
	' (get) Token: 0x06003F40 RID: 16192
	Protected MustOverride ReadOnly Property rapidFireRate As Single

	' Token: 0x17000572 RID: 1394
	' (get) Token: 0x06003F41 RID: 16193 RVA: 0x0022A0F2 File Offset: 0x002284F2
	' (set) Token: 0x06003F42 RID: 16194 RVA: 0x0022A0FA File Offset: 0x002284FA
	Public Property id As Weapon

	' Token: 0x17000573 RID: 1395
	' (get) Token: 0x06003F43 RID: 16195 RVA: 0x0022A103 File Offset: 0x00228503
	Protected Overridable ReadOnly Property isChargeWeapon As Boolean
		Get
			Return False
		End Get
	End Property

	' Token: 0x17000574 RID: 1396
	' (get) Token: 0x06003F44 RID: 16196 RVA: 0x0022A106 File Offset: 0x00228506
	Protected Overrides ReadOnly Property emitTransform As Transform
		Get
			Return Me.player.transform
		End Get
	End Property

	' Token: 0x06003F45 RID: 16197 RVA: 0x0022A114 File Offset: 0x00228514
	Public Overridable Sub Initialize(weaponManager As LevelPlayerWeaponManager, id As Weapon)
		Me.weaponManager = weaponManager
		Me.player = weaponManager.GetComponent(Of LevelPlayerController)()
		Me.id = id
		Me.firing = New AbstractLevelWeapon.FiringSwitches()
		Me.StartCoroutines()
		AddHandler Me.player.OnReviveEvent, AddressOf Me.OnRevive
	End Sub

	' Token: 0x06003F46 RID: 16198 RVA: 0x0022A164 File Offset: 0x00228564
	Public Sub OnDealDamage(damage As Single, receiver As DamageReceiver, dealer As DamageDealer)
		If Me.player Is Nothing OrElse Me.player.IsDead OrElse Me.player.stats Is Nothing OrElse Not receiver.enabled Then
			Return
		End If
		Me.player.stats.OnDealDamage(damage, dealer)
	End Sub

	' Token: 0x06003F47 RID: 16199 RVA: 0x0022A1C6 File Offset: 0x002285C6
	Private Sub OnRevive(pos As Vector3)
		Me.StartCoroutines()
	End Sub

	' Token: 0x06003F48 RID: 16200 RVA: 0x0022A1D0 File Offset: 0x002285D0
	Private Sub StartCoroutines()
		Me.StopAllCoroutines()
		If Me.isChargeWeapon Then
			MyBase.StartCoroutine(Me.chargeFireWeapon_cr(AbstractLevelWeapon.Mode.Basic))
		Else
			MyBase.StartCoroutine(Me.fireWeapon_cr(AbstractLevelWeapon.Mode.Basic))
		End If
		MyBase.StartCoroutine(Me.fireWeapon_cr(AbstractLevelWeapon.Mode.Ex))
	End Sub

	' Token: 0x06003F49 RID: 16201 RVA: 0x0022A21D File Offset: 0x0022861D
	Protected Overridable Sub OnEnable()
		Me.StartCoroutines()
	End Sub

	' Token: 0x06003F4A RID: 16202 RVA: 0x0022A228 File Offset: 0x00228628
	Private Sub Update()
		If Me.firing.[Get](AbstractLevelWeapon.Mode.Basic) OrElse Me.firing.[Get](AbstractLevelWeapon.Mode.Ex) Then
			AbstractLevelWeapon.ONE_PLAYER_FIRING = True
		End If
		If Me.isUsingLoop AndAlso AudioManager.CheckIfPlaying(Me.WeaponSound) Then
			Me.emitAudioFromObject.Add(Me.WeaponSound)
		End If
	End Sub

	' Token: 0x06003F4B RID: 16203 RVA: 0x0022A289 File Offset: 0x00228689
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me.exPrefab = Nothing
		Me.exEffectPrefab = Nothing
		Me.exFiringHitboxPrefab = Nothing
		Me.basicPrefab = Nothing
		Me.basicEffectPrefab = Nothing
		Me.basicFiringHitboxPrefab = Nothing
	End Sub

	' Token: 0x06003F4C RID: 16204 RVA: 0x0022A2BB File Offset: 0x002286BB
	Protected Overridable Sub BasicSoundOneShot(soundP1 As String, soundP2 As String)
		If Me.player.id = PlayerId.PlayerOne Then
			AudioManager.Play(soundP1)
			Me.emitAudioFromObject.Add(soundP1)
		Else
			AudioManager.Play(soundP2)
			Me.emitAudioFromObject.Add(soundP2)
		End If
	End Sub

	' Token: 0x06003F4D RID: 16205 RVA: 0x0022A2F6 File Offset: 0x002286F6
	Protected Overridable Sub OneShotCooldown(sound As String)
		If Me.coolingDown Then
			Return
		End If
		AudioManager.Play(sound)
		Me.emitAudioFromObject.Add(sound)
	End Sub

	' Token: 0x06003F4E RID: 16206 RVA: 0x0022A316 File Offset: 0x00228716
	Protected Overridable Sub ActivateCooldown()
		If Me.coolingDown Then
			Return
		End If
		Me.coolingDown = True
		If MyBase.gameObject.activeInHierarchy Then
			MyBase.StartCoroutine(Me.shot_cooldown_cr())
		End If
	End Sub

	' Token: 0x06003F4F RID: 16207 RVA: 0x0022A348 File Offset: 0x00228748
	Private Iterator Function shot_cooldown_cr() As IEnumerator
		Dim t As Single = 0F
		Dim cooldownTime As Single = Global.UnityEngine.Random.Range(4F, 7F)
		While t < cooldownTime
			t += CupheadTime.Delta
			Yield Nothing
		End While
		Me.coolingDown = False
		Return
	End Function

	' Token: 0x06003F50 RID: 16208 RVA: 0x0022A364 File Offset: 0x00228764
	Protected Overridable Sub BeginBasicCheckAttenuation(soundP1 As String, soundP2 As String)
		If PlayerManager.GetPlayer(PlayerId.PlayerTwo) IsNot Nothing Then
			If Me.player.id = PlayerId.PlayerOne Then
				AudioManager.Attenuation(soundP1, AbstractLevelWeapon.ONE_PLAYER_FIRING, 0.1F)
			Else
				AudioManager.Attenuation(soundP2, AbstractLevelWeapon.ONE_PLAYER_FIRING, 0.1F)
			End If
		End If
	End Sub

	' Token: 0x06003F51 RID: 16209 RVA: 0x0022A3B8 File Offset: 0x002287B8
	Protected Overridable Sub EndBasicCheckAttenuation(soundP1 As String, soundP2 As String)
		If PlayerManager.GetPlayer(PlayerId.PlayerTwo) IsNot Nothing Then
			If Me.player.id = PlayerId.PlayerOne Then
				If PlayerManager.GetPlayer(PlayerId.PlayerTwo) IsNot Nothing Then
					AudioManager.Attenuation(soundP2, AbstractLevelWeapon.ONE_PLAYER_FIRING, 0.1F)
					AudioManager.Attenuation(soundP1, False, 0.1F)
				End If
			Else
				AudioManager.Attenuation(soundP1, AbstractLevelWeapon.ONE_PLAYER_FIRING, 0.1F)
				AudioManager.Attenuation(soundP2, False, 0.1F)
			End If
		End If
	End Sub

	' Token: 0x06003F52 RID: 16210 RVA: 0x0022A434 File Offset: 0x00228834
	Protected Overridable Sub BasicSoundLoop(loopP1 As String, loopP2 As String)
		If Me.player.id = PlayerId.PlayerOne Then
			Me.WeaponSound = loopP1
			AudioManager.PlayLoop(loopP1)
			AudioManager.Attenuation(loopP1, AbstractLevelWeapon.ONE_PLAYER_FIRING, 0.1F)
		Else
			Me.WeaponSound = loopP2
			AudioManager.PlayLoop(loopP2)
			AudioManager.Attenuation(loopP2, AbstractLevelWeapon.ONE_PLAYER_FIRING, 0.1F)
		End If
		Me.isUsingLoop = True
	End Sub

	' Token: 0x06003F53 RID: 16211 RVA: 0x0022A498 File Offset: 0x00228898
	Protected Overridable Sub StopLoopSound(loopP1 As String, loopP2 As String)
		If Me.player.id = PlayerId.PlayerOne Then
			AudioManager.[Stop](loopP1)
			If PlayerManager.GetPlayer(PlayerId.PlayerTwo) IsNot Nothing Then
				AudioManager.Attenuation(loopP2, AbstractLevelWeapon.ONE_PLAYER_FIRING, 0.1F)
			End If
		Else
			AudioManager.[Stop](loopP2)
			AudioManager.Attenuation(loopP1, AbstractLevelWeapon.ONE_PLAYER_FIRING, 0.1F)
		End If
	End Sub

	' Token: 0x06003F54 RID: 16212 RVA: 0x0022A4F7 File Offset: 0x002288F7
	Public Overridable Sub BeginBasic()
		Me.beginFiring(AbstractLevelWeapon.Mode.Basic)
	End Sub

	' Token: 0x06003F55 RID: 16213 RVA: 0x0022A500 File Offset: 0x00228900
	Public Overridable Sub EndBasic()
		Me.endFiring(AbstractLevelWeapon.Mode.Basic)
		AbstractLevelWeapon.ONE_PLAYER_FIRING = False
	End Sub

	' Token: 0x06003F56 RID: 16214 RVA: 0x0022A510 File Offset: 0x00228910
	Protected Overridable Function fireBasic() As AbstractProjectile
		Dim abstractProjectile As AbstractProjectile = Me.fireProjectile(AbstractLevelWeapon.Mode.Basic, True)
		AddHandler abstractProjectile.OnDealDamageEvent, AddressOf Me.OnDealDamage
		Return abstractProjectile
	End Function

	' Token: 0x06003F57 RID: 16215 RVA: 0x0022A53C File Offset: 0x0022893C
	Protected Function fireBasicNoEffect() As AbstractProjectile
		Dim abstractProjectile As AbstractProjectile = Me.fireProjectile(AbstractLevelWeapon.Mode.Basic, False)
		AddHandler abstractProjectile.OnDealDamageEvent, AddressOf Me.OnDealDamage
		Return abstractProjectile
	End Function

	' Token: 0x06003F58 RID: 16216 RVA: 0x0022A565 File Offset: 0x00228965
	Public Overridable Sub BeginEx()
		Me.beginFiring(AbstractLevelWeapon.Mode.Ex)
	End Sub

	' Token: 0x06003F59 RID: 16217 RVA: 0x0022A56E File Offset: 0x0022896E
	Public Overridable Sub EndEx()
		Me.endFiring(AbstractLevelWeapon.Mode.Ex)
	End Sub

	' Token: 0x06003F5A RID: 16218 RVA: 0x0022A578 File Offset: 0x00228978
	Protected Overridable Function fireEx() As AbstractProjectile
		Return Me.fireProjectile(AbstractLevelWeapon.Mode.Ex, True)
	End Function

	' Token: 0x06003F5B RID: 16219 RVA: 0x0022A58F File Offset: 0x0022898F
	Protected Overridable Sub beginFiring(mode As AbstractLevelWeapon.Mode)
		Me.weaponManager.IsShooting = True
		Me.firing.[Set](mode, True)
	End Sub

	' Token: 0x06003F5C RID: 16220 RVA: 0x0022A5AC File Offset: 0x002289AC
	Protected Overridable Function fireProjectile(mode As AbstractLevelWeapon.Mode, Optional createEffect As Boolean = True) As AbstractProjectile
		Dim vector As Vector2 = Me.weaponManager.GetBulletPosition()
		If mode = AbstractLevelWeapon.Mode.Ex Then
			vector = Me.weaponManager.ExPosition
		End If
		If mode = AbstractLevelWeapon.Mode.Basic Then
			Me.weaponManager.UpdateAim()
		End If
		If Me.GetProjectile(mode) Is Nothing Then
			Return Nothing
		End If
		If Me.GetEffect(mode) IsNot Nothing AndAlso createEffect Then
			If mode = AbstractLevelWeapon.Mode.Basic OrElse mode <> AbstractLevelWeapon.Mode.Ex Then
				Dim effect As Effect = Me.basicEffectPrefab.Create(vector, MyBase.transform.localScale)
				Dim weaponSparkEffect As WeaponSparkEffect = TryCast(effect, WeaponSparkEffect)
				If weaponSparkEffect IsNot Nothing Then
					Dim directionPose As LevelPlayerWeaponManager.Pose = Me.weaponManager.GetDirectionPose()
					If directionPose = LevelPlayerWeaponManager.Pose.Forward OrElse directionPose = LevelPlayerWeaponManager.Pose.Forward_R OrElse directionPose = LevelPlayerWeaponManager.Pose.Up_D OrElse directionPose = LevelPlayerWeaponManager.Pose.Up_D_R Then
						weaponSparkEffect.SetPlayer(Me.player)
					End If
					If directionPose = LevelPlayerWeaponManager.Pose.Down Then
						weaponSparkEffect.BringToFrontOfPlayer()
					End If
				End If
			Else
				Me.weaponManager.CreateExDust(Me.GetEffect(mode))
			End If
		End If
		Dim abstractProjectile As AbstractProjectile = Me.GetProjectile(mode).Create(vector, Me.weaponManager.GetBulletRotation(), Me.weaponManager.GetBulletScale())
		If mode = AbstractLevelWeapon.Mode.Ex Then
			abstractProjectile.DamageSource = DamageDealer.DamageSource.Ex
			CupheadLevelCamera.Current.Shake(5F, 0.5F, False)
		End If
		If Me.GetFiringHitbox(mode) IsNot Nothing Then
			abstractProjectile.AddFiringHitbox(Me.GetFiringHitbox(mode).Create(vector, Me.weaponManager.GetBulletRotation()))
		End If
		abstractProjectile.PlayerId = Me.player.id
		Return abstractProjectile
	End Function

	' Token: 0x06003F5D RID: 16221 RVA: 0x0022A746 File Offset: 0x00228B46
	Protected Overridable Sub endFiring(mode As AbstractLevelWeapon.Mode)
		Me.weaponManager.IsShooting = False
		Me.firing.[Set](mode, False)
	End Sub

	' Token: 0x06003F5E RID: 16222 RVA: 0x0022A761 File Offset: 0x00228B61
	Private Function GetProjectile(mode As AbstractLevelWeapon.Mode) As AbstractProjectile
		If mode = AbstractLevelWeapon.Mode.Basic OrElse mode <> AbstractLevelWeapon.Mode.Ex Then
			Return Me.basicPrefab
		End If
		Return Me.exPrefab
	End Function

	' Token: 0x06003F5F RID: 16223 RVA: 0x0022A782 File Offset: 0x00228B82
	Private Function GetEffect(mode As AbstractLevelWeapon.Mode) As Effect
		If mode = AbstractLevelWeapon.Mode.Basic OrElse mode <> AbstractLevelWeapon.Mode.Ex Then
			Return Me.basicEffectPrefab
		End If
		Return Me.exEffectPrefab
	End Function

	' Token: 0x06003F60 RID: 16224 RVA: 0x0022A7A3 File Offset: 0x00228BA3
	Private Function GetFiringHitbox(mode As AbstractLevelWeapon.Mode) As LevelPlayerWeaponFiringHitbox
		If mode = AbstractLevelWeapon.Mode.Basic OrElse mode <> AbstractLevelWeapon.Mode.Ex Then
			Return Me.basicFiringHitboxPrefab
		End If
		Return Me.exFiringHitboxPrefab
	End Function

	' Token: 0x06003F61 RID: 16225 RVA: 0x0022A7C4 File Offset: 0x00228BC4
	Private Function getFiringMethod(mode As AbstractLevelWeapon.Mode) As AbstractLevelWeapon.FireProjectileDelegate
		If mode <> AbstractLevelWeapon.Mode.Ex Then
			If mode <> AbstractLevelWeapon.Mode.Basic Then
			End If
			Return AddressOf Me.fireBasic
		End If
		Return AddressOf Me.fireEx
	End Function

	' Token: 0x06003F62 RID: 16226 RVA: 0x0022A7F4 File Offset: 0x00228BF4
	Protected Overridable Iterator Function fireWeapon_cr(mode As AbstractLevelWeapon.Mode) As IEnumerator
		Dim waitInstruction As WaitForFixedUpdate = New WaitForFixedUpdate()
		While True
			Yield waitInstruction
			If Not Me.player.motor.Dashing Then
				If mode = AbstractLevelWeapon.Mode.Basic AndAlso Me.t < Me.rapidFireRate Then
					If Me.weaponManager.CurrentWeapon Is Me Then
						Me.t += CupheadTime.FixedDelta
					End If
				ElseIf Me.firing.[Get](mode) AndAlso Me.weaponManager.IsShooting Then
					Me.weaponManager.TriggerWeaponFire()
					Me.getFiringMethod(mode)()
					If mode = AbstractLevelWeapon.Mode.Ex OrElse Not Me.rapidFire Then
						Me.firing.[Set](mode, False)
						Me.weaponManager.IsShooting = False
					End If
					Me.t = 0F
				End If
			End If
		End While
		Return
	End Function

	' Token: 0x06003F63 RID: 16227 RVA: 0x0022A818 File Offset: 0x00228C18
	Protected Overridable Iterator Function chargeFireWeapon_cr(mode As AbstractLevelWeapon.Mode) As IEnumerator
		Dim waitInstruction As WaitForFixedUpdate = New WaitForFixedUpdate()
		While True
			Yield waitInstruction
			If mode = AbstractLevelWeapon.Mode.Basic AndAlso Me.firing.[Get](mode) AndAlso Me.weaponManager.IsShooting Then
				Me.alreadyHeld = True
			ElseIf mode = AbstractLevelWeapon.Mode.Basic AndAlso Me.alreadyHeld Then
				Me.alreadyReleased = True
			End If
			If mode = AbstractLevelWeapon.Mode.Basic AndAlso Me.t < Me.rapidFireRate Then
				If Me.weaponManager.CurrentWeapon Is Me Then
					Me.t += CupheadTime.FixedDelta
					Me.charging = False
				End If
			ElseIf Me.firing.[Get](mode) AndAlso Me.weaponManager.IsShooting AndAlso Not Me.player.motor.Dashing AndAlso Not Me.player.motor.IsHit AndAlso Not Me.player.motor.IsUsingSuperOrEx AndAlso Not Me.alreadyReleased Then
				If Not Me.charging Then
					Me.StartCharging()
				End If
				Me.charging = True
			ElseIf Me.charging OrElse Me.alreadyReleased Then
				Me.charging = False
				Me.alreadyReleased = False
				Me.alreadyHeld = False
				Me.weaponManager.TriggerWeaponFire()
				Me.getFiringMethod(mode)()
				If Not Me.weaponManager.IsShooting Then
					Me.firing.[Set](mode, False)
				End If
				Me.t = 0F
			ElseIf Not Me.charging Then
				Me.StopCharging()
			End If
		End While
		Return
	End Function

	' Token: 0x06003F64 RID: 16228 RVA: 0x0022A83A File Offset: 0x00228C3A
	Protected Overridable Sub StartCharging()
	End Sub

	' Token: 0x06003F65 RID: 16229 RVA: 0x0022A83C File Offset: 0x00228C3C
	Protected Overridable Sub StopCharging()
	End Sub

	' Token: 0x0400464F RID: 17999
	<Header("Ex")>
	<SerializeField()>
	Protected exPrefab As AbstractProjectile

	' Token: 0x04004650 RID: 18000
	<SerializeField()>
	Protected exEffectPrefab As Effect

	' Token: 0x04004651 RID: 18001
	<SerializeField()>
	Protected exFiringHitboxPrefab As LevelPlayerWeaponFiringHitbox

	' Token: 0x04004652 RID: 18002
	<Header("Basic")>
	<SerializeField()>
	Protected basicPrefab As AbstractProjectile

	' Token: 0x04004653 RID: 18003
	<SerializeField()>
	Protected basicEffectPrefab As Effect

	' Token: 0x04004654 RID: 18004
	<SerializeField()>
	Protected basicFiringHitboxPrefab As LevelPlayerWeaponFiringHitbox

	' Token: 0x04004656 RID: 18006
	Protected firing As AbstractLevelWeapon.FiringSwitches

	' Token: 0x04004657 RID: 18007
	Protected player As LevelPlayerController

	' Token: 0x04004658 RID: 18008
	Protected weaponManager As LevelPlayerWeaponManager

	' Token: 0x04004659 RID: 18009
	Private WeaponSound As String

	' Token: 0x0400465A RID: 18010
	Private isUsingLoop As Boolean

	' Token: 0x0400465B RID: 18011
	Private coolingDown As Boolean

	' Token: 0x0400465C RID: 18012
	Private t As Single = 1000F

	' Token: 0x0400465D RID: 18013
	Private charging As Boolean

	' Token: 0x0400465E RID: 18014
	Private alreadyHeld As Boolean

	' Token: 0x0400465F RID: 18015
	Private alreadyReleased As Boolean

	' Token: 0x02000A5E RID: 2654
	Public Enum Mode
		' Token: 0x04004661 RID: 18017
		Basic
		' Token: 0x04004662 RID: 18018
		Ex
	End Enum

	' Token: 0x02000A5F RID: 2655
	' (Invoke) Token: 0x06003F67 RID: 16231
	Private Delegate Function FireProjectileDelegate() As AbstractProjectile

	' Token: 0x02000A60 RID: 2656
	<Serializable()>
	Public Class Prefabs
		' Token: 0x06003F6B RID: 16235 RVA: 0x0022A846 File Offset: 0x00228C46
		Public Function [Get](mode As AbstractLevelWeapon.Mode) As AbstractProjectile
			If mode <> AbstractLevelWeapon.Mode.Ex Then
				If mode <> AbstractLevelWeapon.Mode.Basic Then
				End If
				Return Me.basic
			End If
			Return Me.ex
		End Function

		' Token: 0x04004663 RID: 18019
		Public basic As AbstractProjectile

		' Token: 0x04004664 RID: 18020
		Public ex As AbstractProjectile
	End Class

	' Token: 0x02000A61 RID: 2657
	<Serializable()>
	Public Class MuzzleEffects
		' Token: 0x06003F6D RID: 16237 RVA: 0x0022A86F File Offset: 0x00228C6F
		Public Function [Get](mode As AbstractLevelWeapon.Mode) As Effect
			If mode <> AbstractLevelWeapon.Mode.Ex Then
				If mode <> AbstractLevelWeapon.Mode.Basic Then
				End If
				Return Me.basic
			End If
			Return Me.ex
		End Function

		' Token: 0x04004665 RID: 18021
		Public basic As Effect

		' Token: 0x04004666 RID: 18022
		Public ex As Effect
	End Class

	' Token: 0x02000A62 RID: 2658
	Public Class FiringSwitches
		' Token: 0x06003F6F RID: 16239 RVA: 0x0022A898 File Offset: 0x00228C98
		Public Function [Get](mode As AbstractLevelWeapon.Mode) As Boolean
			If mode <> AbstractLevelWeapon.Mode.Ex Then
				If mode <> AbstractLevelWeapon.Mode.Basic Then
				End If
				Return Me.basic
			End If
			Return Me.ex
		End Function

		' Token: 0x06003F70 RID: 16240 RVA: 0x0022A8B9 File Offset: 0x00228CB9
		Public Sub [Set](mode As AbstractLevelWeapon.Mode, val As Boolean)
			If mode <> AbstractLevelWeapon.Mode.Ex Then
				If mode <> AbstractLevelWeapon.Mode.Basic Then
				End If
				Me.basic = val
			Else
				Me.ex = val
			End If
		End Sub

		' Token: 0x04004667 RID: 18023
		Public basic As Boolean

		' Token: 0x04004668 RID: 18024
		Public ex As Boolean
	End Class
End Class
