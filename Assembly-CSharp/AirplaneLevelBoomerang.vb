Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020004B2 RID: 1202
Public Class AirplaneLevelBoomerang
	Inherits AbstractProjectile

	' Token: 0x0600139C RID: 5020 RVA: 0x000ADA08 File Offset: 0x000ABE08
	Public Function Create(pos As Vector2, speedF As Single, easeDF As Single, speedR As Single, easeDR As Single, delay As Single, onLeft As Boolean, id As Integer) As AirplaneLevelBoomerang
		Dim airplaneLevelBoomerang As AirplaneLevelBoomerang = TryCast(MyBase.Create(), AirplaneLevelBoomerang)
		airplaneLevelBoomerang.transform.position = pos
		airplaneLevelBoomerang.DamagesType.OnlyPlayer()
		airplaneLevelBoomerang.delay = delay
		airplaneLevelBoomerang.onLeft = onLeft
		airplaneLevelBoomerang.speedForward = speedF
		airplaneLevelBoomerang.easeDistanceForward = easeDF
		airplaneLevelBoomerang.speedReturn = speedR
		airplaneLevelBoomerang.easeDistanceReturn = easeDR
		airplaneLevelBoomerang.id = id
		Return airplaneLevelBoomerang
	End Function

	' Token: 0x0600139D RID: 5021 RVA: 0x000ADA75 File Offset: 0x000ABE75
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If phase = CollisionPhase.Enter Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x0600139E RID: 5022 RVA: 0x000ADA92 File Offset: 0x000ABE92
	Protected Overrides Sub Update()
		MyBase.Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x0600139F RID: 5023 RVA: 0x000ADAB0 File Offset: 0x000ABEB0
	Protected Overrides Sub Start()
		MyBase.Start()
		If Not MyBase.CanParry Then
			MyBase.animator.Play(If((Not Rand.Bool()), "B", "A"))
		End If
		MyBase.StartCoroutine(Me.move_cr())
		Me.SFX_DOGFIGHT_BoneShot_Loop()
	End Sub

	' Token: 0x060013A0 RID: 5024 RVA: 0x000ADB08 File Offset: 0x000ABF08
	Private Iterator Function move_cr() As IEnumerator
		Me.rend.enabled = True
		Dim [end] As Single = If((Not Me.onLeft), (-725F + Me.easeDistanceForward), (725F - Me.easeDistanceForward))
		Dim flipSprite As Boolean = Not Me.onLeft
		Dim wait As YieldInstruction = New WaitForFixedUpdate()
		MyBase.GetComponent(Of SpriteRenderer)().flipX = flipSprite
		While (Me.onLeft AndAlso Mathf.Sign(MyBase.transform.position.x - [end]) = -1F) OrElse (Not Me.onLeft AndAlso Mathf.Sign(MyBase.transform.position.x - [end]) = 1F)
			MyBase.transform.position += Vector3.right * Me.speedForward * CupheadTime.FixedDelta * CSng(If((Not Me.onLeft), (-1), 1))
			Yield wait
		End While
		Dim t As Single = 0F
		Dim tMax As Single = Me.easeDistanceForward / Me.speedForward * 2F
		Dim start As Single = [end]
		[end] = If((Not Me.onLeft), (-725F), 725F)
		While t < tMax
			t += CupheadTime.FixedDelta
			Yield wait
			MyBase.transform.position = New Vector3(Mathf.Lerp(start, [end], EaseUtils.EaseOutSine(0F, 1F, Mathf.InverseLerp(0F, tMax, t))), MyBase.transform.position.y)
		End While
		MyBase.transform.position = New Vector3([end], MyBase.transform.position.y)
		Yield CupheadTime.WaitForSeconds(Me, Me.delay)
		MyBase.GetComponent(Of SpriteRenderer)().flipX = Not flipSprite
		t = 0F
		tMax = Me.easeDistanceReturn / Me.speedReturn * 2F
		start = MyBase.transform.position.x
		[end] = If((Not Me.onLeft), (-725F + Me.easeDistanceReturn), (725F - Me.easeDistanceReturn))
		While t < tMax
			t += CupheadTime.FixedDelta
			Yield wait
			MyBase.transform.position = New Vector3(Mathf.Lerp(start, [end], EaseUtils.EaseInSine(0F, 1F, Mathf.InverseLerp(0F, tMax, t))), MyBase.transform.position.y)
		End While
		MyBase.transform.position = New Vector3([end], MyBase.transform.position.y)
		[end] = If((Not Me.onLeft), 1025F, (-1025F))
		While (Me.onLeft AndAlso Mathf.Sign(MyBase.transform.position.x - [end]) = 1F) OrElse (Not Me.onLeft AndAlso Mathf.Sign(MyBase.transform.position.x - [end]) = -1F)
			MyBase.transform.position += Vector3.right * Me.speedReturn * CupheadTime.FixedDelta * CSng(If((Not Me.onLeft), 1, (-1)))
			Yield wait
		End While
		Me.SFX_DOGFIGHT_BoneShot_StopLoop()
		Me.Die()
		Yield Nothing
		Return
	End Function

	' Token: 0x060013A1 RID: 5025 RVA: 0x000ADB23 File Offset: 0x000ABF23
	Public Overrides Sub OnParry(player As AbstractPlayerController)
		MyBase.OnParry(player)
		Me.SFX_DOGFIGHT_BoneShot_StopLoop()
	End Sub

	' Token: 0x060013A2 RID: 5026 RVA: 0x000ADB34 File Offset: 0x000ABF34
	Private Sub SFX_DOGFIGHT_BoneShot_Loop()
		AudioManager.FadeSFXVolume("sfx_dlc_dogfight_p1_bulldog_boneshot_0" + (Me.id + 1), 0.12F, 0.1F)
		AudioManager.PlayLoop("sfx_dlc_dogfight_p1_bulldog_boneshot_0" + (Me.id + 1))
		Me.emitAudioFromObject.Add("sfx_dlc_dogfight_p1_bulldog_boneshot_0" + (Me.id + 1))
	End Sub

	' Token: 0x060013A3 RID: 5027 RVA: 0x000ADBA5 File Offset: 0x000ABFA5
	Private Sub SFX_DOGFIGHT_BoneShot_StopLoop()
		AudioManager.[Stop]("sfx_dlc_dogfight_p1_bulldog_boneshot_0" + (Me.id + 1))
	End Sub

	' Token: 0x04001CBE RID: 7358
	Private Const xMax As Single = 725F

	' Token: 0x04001CBF RID: 7359
	Private delay As Single

	' Token: 0x04001CC0 RID: 7360
	Private onLeft As Boolean

	' Token: 0x04001CC1 RID: 7361
	Private speedForward As Single

	' Token: 0x04001CC2 RID: 7362
	Private easeDistanceForward As Single

	' Token: 0x04001CC3 RID: 7363
	Private speedReturn As Single

	' Token: 0x04001CC4 RID: 7364
	Private easeDistanceReturn As Single

	' Token: 0x04001CC5 RID: 7365
	<SerializeField()>
	Private rend As SpriteRenderer

	' Token: 0x04001CC6 RID: 7366
	Private id As Integer
End Class
