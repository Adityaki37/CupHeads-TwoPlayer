Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x0200060C RID: 1548
Public Class FlowerLevelMiniFlowerBullet
	Inherits AbstractProjectile

	' Token: 0x06001F25 RID: 7973 RVA: 0x0011E2D0 File Offset: 0x0011C6D0
	Public Sub OnBulletSpawned(target As Vector3, speed As Integer, damage As Single, Optional friendlyFireDamage As Boolean = False)
		Me.friendlyFire = friendlyFireDamage
		Me.targetDirection = (target - MyBase.transform.position).normalized
		Me.bulletSpeed = speed
		Me.damage = damage
		MyBase.StartCoroutine(Me.spawn_fx_cr())
	End Sub

	' Token: 0x06001F26 RID: 7974 RVA: 0x0011E31F File Offset: 0x0011C71F
	Protected Overrides Sub Awake()
		Me.initDamage = False
		MyBase.Awake()
	End Sub

	' Token: 0x06001F27 RID: 7975 RVA: 0x0011E32E File Offset: 0x0011C72E
	Protected Overrides Sub Start()
		MyBase.Start()
	End Sub

	' Token: 0x06001F28 RID: 7976 RVA: 0x0011E338 File Offset: 0x0011C738
	Protected Overrides Sub Update()
		If Not Me.initDamage Then
			Me.damageDealer.SetDamage(Me.damage)
			Me.damageDealer.SetDamageFlags(True, Me.friendlyFire, False)
			Me.initDamage = True
		End If
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
		MyBase.Update()
	End Sub

	' Token: 0x06001F29 RID: 7977 RVA: 0x0011E398 File Offset: 0x0011C798
	Protected Overrides Sub FixedUpdate()
		MyBase.FixedUpdate()
		MyBase.transform.position += Me.targetDirection * CSng(Me.bulletSpeed) * CupheadTime.FixedDelta
		MyBase.transform.up = -Me.targetDirection
	End Sub

	' Token: 0x06001F2A RID: 7978 RVA: 0x0011E3F3 File Offset: 0x0011C7F3
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
			Me.Die()
		End If
	End Sub

	' Token: 0x06001F2B RID: 7979 RVA: 0x0011E418 File Offset: 0x0011C818
	Protected Overrides Sub OnCollisionEnemy(hit As GameObject, phase As CollisionPhase)
		If Me.friendlyFire AndAlso hit.GetComponent(Of FlowerLevelFlower)() IsNot Nothing Then
			MyBase.OnCollisionEnemy(hit, phase)
			Me.damageDealer.DealDamage(hit)
			Me.Die()
		End If
		MyBase.OnCollisionEnemy(hit, phase)
	End Sub

	' Token: 0x06001F2C RID: 7980 RVA: 0x0011E464 File Offset: 0x0011C864
	Protected Overrides Sub OnCollisionGround(hit As GameObject, phase As CollisionPhase)
		Me.Die()
		MyBase.OnCollisionGround(hit, phase)
	End Sub

	' Token: 0x06001F2D RID: 7981 RVA: 0x0011E474 File Offset: 0x0011C874
	Protected Overrides Sub Die()
		Me.bulletSpeed = 0
		MyBase.transform.Rotate(Vector3.forward, 360F)
		Me.StopAllCoroutines()
		AudioManager.Play("flower_minion_simple_deathpop_high")
		Me.emitAudioFromObject.Add("flower_minion_simple_deathpop_high")
		MyBase.Die()
	End Sub

	' Token: 0x06001F2E RID: 7982 RVA: 0x0011E4C4 File Offset: 0x0011C8C4
	Private Iterator Function spawn_fx_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 0.17F)
		While True
			Me.puff.Create(MyBase.transform.position).transform.SetEulerAngles(Nothing, Nothing, New Single?(MathUtils.DirectionToAngle(Me.targetDirection)))
			Yield CupheadTime.WaitForSeconds(Me, 0.2F)
		End While
		Return
	End Function

	' Token: 0x040027BF RID: 10175
	<SerializeField()>
	Private puff As Effect

	' Token: 0x040027C0 RID: 10176
	Private friendlyFire As Boolean

	' Token: 0x040027C1 RID: 10177
	Private initDamage As Boolean

	' Token: 0x040027C2 RID: 10178
	Private damage As Single

	' Token: 0x040027C3 RID: 10179
	Private bulletSpeed As Integer

	' Token: 0x040027C4 RID: 10180
	Private targetDirection As Vector3
End Class
