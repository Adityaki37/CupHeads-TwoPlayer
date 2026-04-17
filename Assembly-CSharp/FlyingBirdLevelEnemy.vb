Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x0200061B RID: 1563
Public Class FlyingBirdLevelEnemy
	Inherits AbstractProjectile

	' Token: 0x06001FCA RID: 8138 RVA: 0x00123D58 File Offset: 0x00122158
	Public Function Create(pos As Vector2, properties As FlyingBirdLevelEnemy.Properties) As FlyingBirdLevelEnemy
		Dim gameObject As GameObject = Global.UnityEngine.[Object].Instantiate(Of GameObject)(MyBase.gameObject)
		Dim component As FlyingBirdLevelEnemy = gameObject.GetComponent(Of FlyingBirdLevelEnemy)()
		component.transform.position = pos
		component.properties = properties
		component.Init()
		Return component
	End Function

	' Token: 0x06001FCB RID: 8139 RVA: 0x00123D97 File Offset: 0x00122197
	Protected Overrides Sub Awake()
		MyBase.Awake()
		AddHandler MyBase.GetComponent(Of DamageReceiver)().OnDamageTaken, AddressOf Me.OnDamageTaken
	End Sub

	' Token: 0x06001FCC RID: 8140 RVA: 0x00123DB6 File Offset: 0x001221B6
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If Me.damageDealer IsNot Nothing AndAlso phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x06001FCD RID: 8141 RVA: 0x00123DE0 File Offset: 0x001221E0
	Private Sub Init()
		Me.startPos = MyBase.transform.position
		Me.health = CSng(Me.properties.health)
		MyBase.StartCoroutine(Me.x_cr())
		MyBase.StartCoroutine(Me.shoot_cr())
	End Sub

	' Token: 0x06001FCE RID: 8142 RVA: 0x00123E2F File Offset: 0x0012222F
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		Me.health -= info.damage
		If Me.health <= 0F Then
			Me.Die()
		End If
	End Sub

	' Token: 0x06001FCF RID: 8143 RVA: 0x00123E5A File Offset: 0x0012225A
	Private Sub Shoot()
	End Sub

	' Token: 0x06001FD0 RID: 8144 RVA: 0x00123E5C File Offset: 0x0012225C
	Public Overrides Sub OnParryDie()
		Me.Die()
	End Sub

	' Token: 0x06001FD1 RID: 8145 RVA: 0x00123E64 File Offset: 0x00122264
	Protected Overrides Sub Die()
		MyBase.Die()
		Me.StopAllCoroutines()
		AudioManager.Play("level_flying_bird_smallbird_death")
		Me.emitAudioFromObject.Add("level_flying_bird_smallbird_death")
		MyBase.GetComponent(Of Collider2D)().enabled = False
	End Sub

	' Token: 0x06001FD2 RID: 8146 RVA: 0x00123E98 File Offset: 0x00122298
	Private Iterator Function y_cr() As IEnumerator
		Dim start As Single = Me.startPos.y + Me.properties.floatRange / 2F
		Dim [end] As Single = Me.startPos.y - Me.properties.floatRange / 2F
		MyBase.transform.SetPosition(Nothing, New Single?(start), Nothing)
		Dim t As Single = 0F
		While True
			t = 0F
			While t < Me.properties.floatTime
				Dim val As Single = t / Me.properties.floatTime
				MyBase.transform.SetPosition(Nothing, New Single?(EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, start, [end], val)), Nothing)
				t += CupheadTime.Delta
				Yield Nothing
			End While
			t = 0F
			While t < Me.properties.floatTime
				Dim val2 As Single = t / Me.properties.floatTime
				MyBase.transform.SetPosition(Nothing, New Single?(EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, [end], start, val2)), Nothing)
				t += CupheadTime.Delta
				Yield Nothing
			End While
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06001FD3 RID: 8147 RVA: 0x00123EB4 File Offset: 0x001222B4
	Private Iterator Function x_cr() As IEnumerator
		While True
			MyBase.transform.AddPosition(-Me.properties.speed * CupheadTime.Delta, 0F, 0F)
			If MyBase.transform.position.x < -740F Then
				Me.Die()
			End If
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06001FD4 RID: 8148 RVA: 0x00123ED0 File Offset: 0x001222D0
	Private Iterator Function shoot_cr() As IEnumerator
		While True
			Yield CupheadTime.WaitForSeconds(Me, Me.properties.projectileDelay)
			Me.Shoot()
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x0400284A RID: 10314
	<SerializeField()>
	Private projectilePrefab As FlyingBirdLevelEnemyProjectile

	' Token: 0x0400284B RID: 10315
	Private properties As FlyingBirdLevelEnemy.Properties

	' Token: 0x0400284C RID: 10316
	Private startPos As Vector2

	' Token: 0x0400284D RID: 10317
	Private health As Single

	' Token: 0x0200061C RID: 1564
	Public Class Properties
		' Token: 0x06001FD5 RID: 8149 RVA: 0x00123EEB File Offset: 0x001222EB
		Public Sub New(health As Integer, speed As Single, floatRange As Single, floatTime As Single, projectileHeight As Single, projectileFallTime As Single, projectileDelay As Single)
			Me.health = health
			Me.speed = speed
			Me.floatRange = floatRange
			Me.floatTime = floatTime
			Me.projectileHeight = projectileHeight
			Me.projectileFallTime = projectileFallTime
			Me.projectileDelay = projectileDelay
		End Sub

		' Token: 0x0400284E RID: 10318
		Public health As Integer

		' Token: 0x0400284F RID: 10319
		Public speed As Single

		' Token: 0x04002850 RID: 10320
		Public floatRange As Single

		' Token: 0x04002851 RID: 10321
		Public floatTime As Single

		' Token: 0x04002852 RID: 10322
		Public projectileHeight As Single

		' Token: 0x04002853 RID: 10323
		Public projectileFallTime As Single

		' Token: 0x04002854 RID: 10324
		Public projectileDelay As Single
	End Class
End Class
