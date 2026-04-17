Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000797 RID: 1943
Public Class RumRunnersLevelMoth
	Inherits AbstractCollidableObject

	' Token: 0x06002B20 RID: 11040 RVA: 0x00192500 File Offset: 0x00190900
	Private Sub Start()
		Me.sparkWarning.SetActive(False)
		If MyBase.GetComponent(Of DamageReceiver)() Then
			Me.damageReceiver = MyBase.GetComponent(Of DamageReceiver)()
			AddHandler Me.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
		End If
	End Sub

	' Token: 0x06002B21 RID: 11041 RVA: 0x0019254C File Offset: 0x0019094C
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		Me.hp -= info.damage
		If Me.hp <= 0F Then
			Me.Die()
		End If
	End Sub

	' Token: 0x06002B22 RID: 11042 RVA: 0x00192578 File Offset: 0x00190978
	Public Sub Init(pos As Vector3, properties As LevelProperties.RumRunners.Moth, parent As RumRunnersLevelSpider)
		MyBase.transform.position = pos
		Me.properties = properties
		Me.hp = properties.hp
		Me.StartAttack()
		Me.parent = parent
		AddHandler Me.parent.OnDeathEvent, AddressOf Me.Die
	End Sub

	' Token: 0x06002B23 RID: 11043 RVA: 0x001925C8 File Offset: 0x001909C8
	Private Sub StartAttack()
		MyBase.StartCoroutine(Me.move_cr())
		MyBase.StartCoroutine(Me.shoot_cr())
		MyBase.StartCoroutine(Me.life_timer_cr())
	End Sub

	' Token: 0x06002B24 RID: 11044 RVA: 0x001925F4 File Offset: 0x001909F4
	Private Iterator Function move_cr() As IEnumerator
		Me.goingLeft = Rand.Bool()
		Dim dist As Single = If((Not Me.goingLeft), Mathf.Abs(540F - MyBase.transform.position.x), Mathf.Abs(-540F - MyBase.transform.position.x))
		Dim time As Single = dist / Me.properties.mothSpeed
		Dim t As Single = 0F
		Dim start As Single = MyBase.transform.position.x
		Dim [end] As Single = If((Not Me.goingLeft), 540F, (-540F))
		Dim wait As YieldInstruction = New WaitForFixedUpdate()
		While t < time
			t += CupheadTime.FixedDelta
			MyBase.transform.SetPosition(New Single?(Mathf.Lerp(start, [end], t / time)), Nothing, Nothing)
			Yield wait
		End While
		dist = Mathf.Abs(-1080F)
		time = dist / Me.properties.mothSpeed
		While Not Me.dead
			t = 0F
			Me.goingLeft = Not Me.goingLeft
			start = MyBase.transform.position.x
			[end] = If((Not Me.goingLeft), 540F, (-540F))
			While t < time
				t += CupheadTime.FixedDelta
				MyBase.transform.SetPosition(New Single?(Mathf.Lerp(start, [end], t / time)), Nothing, Nothing)
				Yield wait
			End While
			Yield wait
		End While
		Yield Nothing
		Return
	End Function

	' Token: 0x06002B25 RID: 11045 RVA: 0x00192610 File Offset: 0x00190A10
	Private Iterator Function shoot_cr() As IEnumerator
		While Not Me.dead
			Me.sparkWarning.SetActive(False)
			Yield CupheadTime.WaitForSeconds(Me, Me.properties.mothShootDelay)
			Me.sparkWarning.SetActive(True)
			Me.regularProjectile.Create(MyBase.transform.position, -90F, Me.properties.mothBulletSpeed)
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06002B26 RID: 11046 RVA: 0x0019262C File Offset: 0x00190A2C
	Private Iterator Function life_timer_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, Me.properties.mothLifetime)
		Me.Die()
		Return
	End Function

	' Token: 0x06002B27 RID: 11047 RVA: 0x00192647 File Offset: 0x00190A47
	Private Sub Die()
		Me.dead = True
		Me.StopAllCoroutines()
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub

	' Token: 0x06002B28 RID: 11048 RVA: 0x00192661 File Offset: 0x00190A61
	Protected Overrides Sub OnDestroy()
		RemoveHandler Me.parent.OnDeathEvent, AddressOf Me.Die
		MyBase.OnDestroy()
	End Sub

	' Token: 0x040033DC RID: 13276
	Private Const RIGHT_X As Single = 540F

	' Token: 0x040033DD RID: 13277
	Private Const LEFT_X As Single = -540F

	' Token: 0x040033DE RID: 13278
	<SerializeField()>
	Private sparkWarning As GameObject

	' Token: 0x040033DF RID: 13279
	<SerializeField()>
	Private regularProjectile As BasicProjectile

	' Token: 0x040033E0 RID: 13280
	Private parent As RumRunnersLevelSpider

	' Token: 0x040033E1 RID: 13281
	Private properties As LevelProperties.RumRunners.Moth

	' Token: 0x040033E2 RID: 13282
	Private damageReceiver As DamageReceiver

	' Token: 0x040033E3 RID: 13283
	Private hp As Single

	' Token: 0x040033E4 RID: 13284
	Private goingLeft As Boolean

	' Token: 0x040033E5 RID: 13285
	Private dead As Boolean
End Class
