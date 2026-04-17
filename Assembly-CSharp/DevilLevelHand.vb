Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000577 RID: 1399
Public Class DevilLevelHand
	Inherits AbstractCollidableObject

	' Token: 0x06001A9B RID: 6811 RVA: 0x000F3DF0 File Offset: 0x000F21F0
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.isDead = False
		Me.damageDealer = DamageDealer.NewEnemy()
		AddHandler Me.demonSprite.GetComponent(Of DamageReceiver)().OnDamageTaken, AddressOf Me.OnDamageTaken
		AddHandler Me.demonSprite.GetComponent(Of CollisionChild)().OnPlayerCollision, AddressOf Me.OnCollisionPlayer
	End Sub

	' Token: 0x06001A9C RID: 6812 RVA: 0x000F3E4E File Offset: 0x000F224E
	Private Sub Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x06001A9D RID: 6813 RVA: 0x000F3E66 File Offset: 0x000F2266
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		If Me.isInvincible Then
			Return
		End If
		Me.hp -= info.damage
		If Me.hp < 0F Then
			Me.Die()
		End If
	End Sub

	' Token: 0x06001A9E RID: 6814 RVA: 0x000F3E9D File Offset: 0x000F229D
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If Me.damageDealer IsNot Nothing AndAlso phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x06001A9F RID: 6815 RVA: 0x000F3EC8 File Offset: 0x000F22C8
	Public Sub StartPattern(properties As LevelProperties.Devil.Hands)
		Me.properties = properties
		Me.pinkStringIndex = Global.UnityEngine.Random.Range(0, properties.pinkString.Length)
		Me.maxHp = properties.HP
		Me.hp = Me.maxHp
		Me.state = DevilLevelHand.State.Idle
		Me.startPos = New Vector2(MyBase.transform.position.x, properties.yRange.max)
		MyBase.transform.position = Me.startPos
		Me.handLocalStartPos = Me.handSprite.transform.localPosition
		Me.demonLocalStartPos = Me.demonSprite.transform.localPosition
		MyBase.gameObject.SetActive(True)
		MyBase.StartCoroutine(Me.move_cr())
	End Sub

	' Token: 0x06001AA0 RID: 6816 RVA: 0x000F3F96 File Offset: 0x000F2396
	Public Sub SpawnIn()
		MyBase.StartCoroutine(Me.move_in_cr())
	End Sub

	' Token: 0x06001AA1 RID: 6817 RVA: 0x000F3FA8 File Offset: 0x000F23A8
	Private Iterator Function move_in_cr() As IEnumerator
		Me.startAtTop = True
		Me.despawned = False
		MyBase.transform.position = Me.startPos
		Me.handSprite.transform.localPosition = Me.handLocalStartPos
		Me.demonSprite.transform.localPosition = Me.demonLocalStartPos
		MyBase.animator.Play("Off", 1)
		MyBase.animator.Play("Hand_Loop")
		Dim xPos As Single = 547F
		Dim start As Vector3 = New Vector3(Me.startPos.x, Me.properties.yRange.max)
		Dim [end] As Vector3 = New Vector3(If((Not Me.onLeft), xPos, (-xPos)), Me.properties.yRange.max)
		Dim t As Single = 0F
		Dim time As Single = 1F
		Dim wait As YieldInstruction = New WaitForFixedUpdate()
		While t < time
			t += CupheadTime.FixedDelta
			MyBase.transform.position = Vector3.Lerp(start, [end], t / time)
			Yield wait
		End While
		Me.isInvincible = False
		MyBase.StartCoroutine(Me.hand_move_up_cr())
		Return
	End Function

	' Token: 0x06001AA2 RID: 6818 RVA: 0x000F3FC4 File Offset: 0x000F23C4
	Private Iterator Function move_cr() As IEnumerator
		Dim moveTime As Single = (Me.properties.yRange.max - Me.properties.yRange.min) / Me.properties.speed
		Dim t As Single = 0F
		Dim startY As Single = Me.demonSprite.transform.position.y
		Dim endY As Single = Me.properties.yRange.min
		While True
			While Not Me.isSliding
				Yield Nothing
			End While
			startY = Me.demonSprite.transform.position.y
			endY = Me.properties.yRange.min
			Me.startAtTop = False
			While Me.isSliding
				t = 0F
				While t < moveTime AndAlso Me.isSliding
					Me.demonSprite.transform.SetPosition(Nothing, New Single?(Mathf.Lerp(startY, endY, t / moveTime)), Nothing)
					t += CupheadTime.FixedDelta
					Yield New WaitForFixedUpdate()
				End While
				startY = If((Not Me.startAtTop), Me.properties.yRange.min, Me.properties.yRange.max)
				endY = If((Not Me.startAtTop), Me.properties.yRange.max, Me.properties.yRange.min)
				Me.startAtTop = Not Me.startAtTop
				Yield New WaitForFixedUpdate()
			End While
			Yield New WaitForFixedUpdate()
		End While
		Return
	End Function

	' Token: 0x06001AA3 RID: 6819 RVA: 0x000F3FE0 File Offset: 0x000F23E0
	Public Sub Fire()
		If Me.properties.pinkString(Me.pinkStringIndex) = "P"c Then
			Dim basicProjectile As BasicProjectile = Me.bulletPinkPrefab.Create(Me.bulletRoot.position, Me.shootAngle, Me.properties.bulletSpeed)
			basicProjectile.transform.SetScale(New Single?(CSng(If((Not Me.onLeft), 1, 1))), New Single?(CSng(If((Not Me.onLeft), (-1), 1))), Nothing)
		Else
			Dim basicProjectile2 As BasicProjectile = Me.bulletPrefab.Create(Me.bulletRoot.position, Me.shootAngle, Me.properties.bulletSpeed)
			basicProjectile2.transform.SetScale(New Single?(CSng(If((Not Me.onLeft), 1, 1))), New Single?(CSng(If((Not Me.onLeft), (-1), 1))), Nothing)
		End If
		Me.pinkStringIndex = (Me.pinkStringIndex + 1) Mod Me.properties.pinkString.Length
	End Sub

	' Token: 0x06001AA4 RID: 6820 RVA: 0x000F4110 File Offset: 0x000F2510
	Public Sub Die()
		Me.isSliding = False
		Me.isInvincible = True
		MyBase.StartCoroutine(Me.demon_move_down_cr())
	End Sub

	' Token: 0x06001AA5 RID: 6821 RVA: 0x000F4130 File Offset: 0x000F2530
	Private Iterator Function hand_move_up_cr() As IEnumerator
		MyBase.animator.SetTrigger("OnRelease")
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Hand_Release_Start", False, True)
		Me.isSliding = True
		Dim t As Single = 0F
		Dim time As Single = 0.5F
		Dim start As Single = Me.handSprite.transform.position.y
		Dim [end] As Single = 860F
		Dim wait As YieldInstruction = New WaitForFixedUpdate()
		While t < time
			t += CupheadTime.FixedDelta
			Me.handSprite.transform.SetPosition(Nothing, New Single?(Mathf.Lerp(start, [end], t / time)), Nothing)
			Yield wait
		End While
		Yield wait
		Return
	End Function

	' Token: 0x06001AA6 RID: 6822 RVA: 0x000F414C File Offset: 0x000F254C
	Private Iterator Function demon_move_down_cr() As IEnumerator
		MyBase.animator.SetTrigger("OnDeath")
		Dim t As Single = 0F
		Dim time As Single = 1F
		Dim start As Single = Me.demonSprite.transform.position.y
		Dim [end] As Single = -860F
		Dim wait As YieldInstruction = New WaitForFixedUpdate()
		While t < time
			t += CupheadTime.FixedDelta
			Me.demonSprite.transform.SetPosition(Nothing, New Single?(Mathf.Lerp(start, [end], t / time)), Nothing)
			Yield wait
		End While
		Yield wait
		If Me.isDead Then
			Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
		End If
		Me.despawned = True
		Me.hp = Me.maxHp
		Return
	End Function

	' Token: 0x06001AA7 RID: 6823 RVA: 0x000F4167 File Offset: 0x000F2567
	Private Sub SFXAttack()
		AudioManager.Play("fat_bat_attack")
		Me.emitAudioFromObject.Add("fat_bat_attack")
	End Sub

	' Token: 0x06001AA8 RID: 6824 RVA: 0x000F4183 File Offset: 0x000F2583
	Private Sub SFXDeath()
		AudioManager.Play("fat_bat_die")
		Me.emitAudioFromObject.Add("fat_bat_die")
	End Sub

	' Token: 0x06001AA9 RID: 6825 RVA: 0x000F419F File Offset: 0x000F259F
	Private Sub SFXHandRelease()
		AudioManager.Play("p3_hand_release_start")
		Me.emitAudioFromObject.Add("p3_hand_release_start")
	End Sub

	' Token: 0x06001AAA RID: 6826 RVA: 0x000F41BB File Offset: 0x000F25BB
	Private Sub SFXFatSpawn()
		AudioManager.Play("fat_bat_spawn")
		Me.emitAudioFromObject.Add("fat_bat_spawn")
	End Sub

	' Token: 0x06001AAB RID: 6827 RVA: 0x000F41D7 File Offset: 0x000F25D7
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me.bulletPrefab = Nothing
		Me.bulletPinkPrefab = Nothing
	End Sub

	' Token: 0x040023C7 RID: 9159
	Public state As DevilLevelHand.State

	' Token: 0x040023C8 RID: 9160
	Public despawned As Boolean

	' Token: 0x040023C9 RID: 9161
	Public isDead As Boolean

	' Token: 0x040023CA RID: 9162
	<SerializeField()>
	Private onLeft As Boolean

	' Token: 0x040023CB RID: 9163
	<SerializeField()>
	Private shootAngle As Single

	' Token: 0x040023CC RID: 9164
	<SerializeField()>
	Private bulletRoot As Transform

	' Token: 0x040023CD RID: 9165
	<SerializeField()>
	Private bulletPrefab As BasicProjectile

	' Token: 0x040023CE RID: 9166
	<SerializeField()>
	Private bulletPinkPrefab As BasicProjectile

	' Token: 0x040023CF RID: 9167
	<Header("Sprites")>
	<SerializeField()>
	Private demonSprite As SpriteRenderer

	' Token: 0x040023D0 RID: 9168
	Private demonLocalStartPos As Vector3

	' Token: 0x040023D1 RID: 9169
	<SerializeField()>
	Private handSprite As SpriteRenderer

	' Token: 0x040023D2 RID: 9170
	Private handLocalStartPos As Vector3

	' Token: 0x040023D3 RID: 9171
	Private properties As LevelProperties.Devil.Hands

	' Token: 0x040023D4 RID: 9172
	Private damageReceiver As DamageReceiver

	' Token: 0x040023D5 RID: 9173
	Private damageDealer As DamageDealer

	' Token: 0x040023D6 RID: 9174
	Private startPos As Vector3

	' Token: 0x040023D7 RID: 9175
	Private hp As Single

	' Token: 0x040023D8 RID: 9176
	Private maxHp As Single

	' Token: 0x040023D9 RID: 9177
	Private isInvincible As Boolean = True

	' Token: 0x040023DA RID: 9178
	Private isSliding As Boolean

	' Token: 0x040023DB RID: 9179
	Private startAtTop As Boolean = True

	' Token: 0x040023DC RID: 9180
	Private pinkStringIndex As Integer

	' Token: 0x02000578 RID: 1400
	Public Enum State
		' Token: 0x040023DE RID: 9182
		Uninitialized
		' Token: 0x040023DF RID: 9183
		Idle
	End Enum
End Class
