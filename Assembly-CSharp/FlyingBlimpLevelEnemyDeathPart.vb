Imports System
Imports UnityEngine

' Token: 0x02000635 RID: 1589
Public Class FlyingBlimpLevelEnemyDeathPart
	Inherits AbstractProjectile

	' Token: 0x17000385 RID: 901
	' (get) Token: 0x06002091 RID: 8337 RVA: 0x0012C69D File Offset: 0x0012AA9D
	Public Overrides ReadOnly Property ParryMeterMultiplier As Single
		Get
			Return 0.25F
		End Get
	End Property

	' Token: 0x06002092 RID: 8338 RVA: 0x0012C6A4 File Offset: 0x0012AAA4
	Public Function CreatePart(position As Vector3, properties As LevelProperties.FlyingBlimp.Gear) As FlyingBlimpLevelEnemyDeathPart
		Dim flyingBlimpLevelEnemyDeathPart As FlyingBlimpLevelEnemyDeathPart = Me.InstantiatePrefab(Of FlyingBlimpLevelEnemyDeathPart)()
		flyingBlimpLevelEnemyDeathPart.transform.position = position
		flyingBlimpLevelEnemyDeathPart.properties = properties
		Return flyingBlimpLevelEnemyDeathPart
	End Function

	' Token: 0x06002093 RID: 8339 RVA: 0x0012C6CC File Offset: 0x0012AACC
	Protected Overrides Sub Start()
		MyBase.Start()
		If Not Me.gear Then
			Me.velocity = New Vector2(Global.UnityEngine.Random.Range(-500F, 500F), Global.UnityEngine.Random.Range(500F, 1200F))
		Else
			Me.velocity = New Vector2(-500F, Me.properties.bounceHeight)
		End If
	End Sub

	' Token: 0x06002094 RID: 8340 RVA: 0x0012C734 File Offset: 0x0012AB34
	Protected Overrides Sub FixedUpdate()
		MyBase.Update()
		MyBase.transform.position += (Me.velocity + New Vector2(-Me.properties.bounceSpeed, Me.accumulatedGravity)) * Time.fixedDeltaTime
		Me.accumulatedGravity += -100F
		If MyBase.transform.position.y < -360F Then
			Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
		End If
	End Sub

	' Token: 0x06002095 RID: 8341 RVA: 0x0012C7C8 File Offset: 0x0012ABC8
	Public Overrides Sub OnParry(player As AbstractPlayerController)
		If Not Me.getNewWeapon Then
			If Me.parryCounter < CSng(Me.properties.parryCount) Then
				Me.parryCounter += 1F
				Me.accumulatedGravity = 0F
			Else
				MyBase.GetComponent(Of SpriteRenderer)().color = ColorUtils.HexToColor("FF00EDFF")
				MyBase.FrameDelayedCallback(AddressOf Me.SetWeapon, 5)
				Me.accumulatedGravity = 0F
			End If
		Else
			Me.parriedIt = True
			Me.Die()
		End If
	End Sub

	' Token: 0x06002096 RID: 8342 RVA: 0x0012C860 File Offset: 0x0012AC60
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If Me.getNewWeapon AndAlso Not Me.parriedIt Then
			Dim [next] As AbstractPlayerController = PlayerManager.GetNext()
			Dim planePlayerController As PlanePlayerController = TryCast([next], PlanePlayerController)
			planePlayerController.weaponManager.SwitchToWeapon(Weapon.plane_weapon_laser)
			Me.Die()
		End If
	End Sub

	' Token: 0x06002097 RID: 8343 RVA: 0x0012C8AE File Offset: 0x0012ACAE
	Private Sub SetWeapon()
		Me.getNewWeapon = True
	End Sub

	' Token: 0x06002098 RID: 8344 RVA: 0x0012C8B7 File Offset: 0x0012ACB7
	Protected Overrides Sub Die()
		MyBase.GetComponent(Of SpriteRenderer)().enabled = False
		MyBase.Die()
	End Sub

	' Token: 0x04002913 RID: 10515
	<SerializeField()>
	Private gear As Boolean

	' Token: 0x04002914 RID: 10516
	Private properties As LevelProperties.FlyingBlimp.Gear

	' Token: 0x04002915 RID: 10517
	Private Const VELOCITY_X_MIN As Single = -500F

	' Token: 0x04002916 RID: 10518
	Private Const VELOCITY_X_MAX As Single = 500F

	' Token: 0x04002917 RID: 10519
	Private Const VELOCITY_Y_MIN As Single = 500F

	' Token: 0x04002918 RID: 10520
	Private Const VELOCITY_Y_MAX As Single = 1200F

	' Token: 0x04002919 RID: 10521
	Private Const GRAVITY As Single = -100F

	' Token: 0x0400291A RID: 10522
	Private velocity As Vector2

	' Token: 0x0400291B RID: 10523
	Private accumulatedGravity As Single

	' Token: 0x0400291C RID: 10524
	Private parryCounter As Single

	' Token: 0x0400291D RID: 10525
	Private getNewWeapon As Boolean

	' Token: 0x0400291E RID: 10526
	Private parriedIt As Boolean
End Class
