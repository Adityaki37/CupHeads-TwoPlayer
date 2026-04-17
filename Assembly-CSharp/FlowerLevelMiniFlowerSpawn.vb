Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x0200060D RID: 1549
Public Class FlowerLevelMiniFlowerSpawn
	Inherits AbstractCollidableObject

	' Token: 0x06001F30 RID: 7984 RVA: 0x0011E604 File Offset: 0x0011CA04
	Public Sub OnMiniFlowerSpawn(parent As FlowerLevelFlower, properties As LevelProperties.Flower.EnemyPlants)
		Me.properties = properties
		Me.currentSpeed = Me.properties.miniFlowerMovmentSpeed
		Me.currentHP = CSng(Me.properties.miniFlowerPlantHP)
		Me.parent = parent
		AddHandler Me.parent.OnDeathEvent, AddressOf Me.HandleEnd
		MyBase.StartCoroutine(Me.move_cr())
	End Sub

	' Token: 0x06001F31 RID: 7985 RVA: 0x0011E668 File Offset: 0x0011CA68
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		Me.currentHP -= info.damage
		If Me.currentHP <= 0F Then
			If Me.isDead Then
				Return
			End If
			Me.isDead = True
			Me.parent.OnMiniFlowerDeath()
			MyBase.animator.SetTrigger("OnDeath")
			Me.explosion.GetComponent(Of Animator)().SetInteger("Variant", 1)
			Me.explosion.GetComponent(Of Animator)().SetTrigger("OnDeath")
			MyBase.GetComponent(Of Collider2D)().enabled = False
			MyBase.StartCoroutine(Me.die_cr())
			Me.currentSpeed = 0
			Me.explosion.Rotate(Vector3.forward, CSng(Global.UnityEngine.Random.Range(0, 360)))
		End If
	End Sub

	' Token: 0x06001F32 RID: 7986 RVA: 0x0011E730 File Offset: 0x0011CB30
	Public Sub SpawnPetals()
		MyBase.GetComponent(Of Collider2D)().enabled = False
		Dim vector As Vector3 = Me.spawnPoint.transform.position + Vector3.up * CSng(Global.UnityEngine.Random.Range(-10, 10))
		If Not Me.isFriendly Then
			Dim gameObject As GameObject = Global.UnityEngine.[Object].Instantiate(Of GameObject)(Me.petalA, vector, Quaternion.identity)
			gameObject.GetComponent(Of Animator)().Play("PetalA_Red", Global.UnityEngine.Random.Range(0, 1))
			MyBase.StartCoroutine(Me.fade_cr(gameObject, 0.8F, False))
			gameObject = Global.UnityEngine.[Object].Instantiate(Of GameObject)(Me.petalB, vector + Vector3.down * 50F, Quaternion.identity)
			gameObject.GetComponent(Of Animator)().Play("PetalB_Red_Loop")
			MyBase.StartCoroutine(Me.fade_cr(gameObject, 1F, False))
		Else
			Dim gameObject2 As GameObject = Global.UnityEngine.[Object].Instantiate(Of GameObject)(Me.petalA, vector, Quaternion.identity)
			gameObject2.GetComponent(Of Animator)().Play("PetalA_Blue", Global.UnityEngine.Random.Range(0, 1))
			MyBase.StartCoroutine(Me.fade_cr(gameObject2, 0.8F, False))
			gameObject2 = Global.UnityEngine.[Object].Instantiate(Of GameObject)(Me.petalB, vector + Vector3.down * 50F, Quaternion.identity)
			gameObject2.GetComponent(Of Animator)().Play("PetalB_Blue_Loop")
			MyBase.StartCoroutine(Me.fade_cr(gameObject2, 1F, True))
		End If
	End Sub

	' Token: 0x06001F33 RID: 7987 RVA: 0x0011E894 File Offset: 0x0011CC94
	Private Iterator Function move_cr() As IEnumerator
		Dim wait As YieldInstruction = New WaitForFixedUpdate()
		While True
			If Not Me.isAttacking AndAlso Me.isActive Then
				Dim num As Single = Mathf.Sin(Me.attackTime * CSng(Me.currentSpeed) / 3F)
				num = Mathf.Clamp(num, -2F, 2F)
				Me.attackTime += CupheadTime.FixedDelta
				Dim vector As Vector3 = Vector3.Lerp(MyBase.transform.position, Me.pivotPoint + Me.flightDirection * num * 4000F * CupheadTime.FixedDelta, 0.03F * CupheadTime.GlobalSpeed)
				MyBase.transform.position = vector
				Dim num2 As Single = 15F * Mathf.Sin(num) * -Mathf.Sign(Me.flightDirection.x)
				MyBase.transform.rotation = Quaternion.RotateTowards(MyBase.transform.rotation, Quaternion.Euler(0F, 0F, num2), 8F)
			Else
				MyBase.transform.rotation = Quaternion.RotateTowards(MyBase.transform.rotation, Quaternion.Euler(0F, 0F, 0F), 10F)
			End If
			Yield wait
		End While
		Return
	End Function

	' Token: 0x06001F34 RID: 7988 RVA: 0x0011E8B0 File Offset: 0x0011CCB0
	Private Iterator Function die_cr() As IEnumerator
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Death", 0, True, True)
		MyBase.GetComponent(Of SpriteRenderer)().enabled = False
		Return
	End Function

	' Token: 0x06001F35 RID: 7989 RVA: 0x0011E8CC File Offset: 0x0011CCCC
	Private Iterator Function fade_cr(petal As GameObject, duration As Single, Optional lastPetal As Boolean = False) As IEnumerator
		Dim wait As YieldInstruction = New WaitForFixedUpdate()
		Dim petalSprite As SpriteRenderer = petal.GetComponent(Of SpriteRenderer)()
		Dim currentTime As Single = duration
		Dim pct As Single = currentTime / duration
		While pct >= 0F
			Dim c As Color = petalSprite.material.color
			c.a = pct
			petalSprite.material.color = c
			petalSprite.transform.position += Vector3.down * 100F * CupheadTime.FixedDelta
			currentTime -= CupheadTime.FixedDelta
			pct = currentTime / duration
			Yield wait
		End While
		Global.UnityEngine.[Object].Destroy(petal)
		If lastPetal Then
			Me.Die()
		End If
		Return
	End Function

	' Token: 0x06001F36 RID: 7990 RVA: 0x0011E8FC File Offset: 0x0011CCFC
	Public Sub FriendlyFireDamage()
	End Sub

	' Token: 0x06001F37 RID: 7991 RVA: 0x0011E8FE File Offset: 0x0011CCFE
	Private Sub HandleEnd()
		If Me.isFriendly Then
			MyBase.GetComponent(Of Collider2D)().enabled = False
			Me.StopAllCoroutines()
		Else
			Me.Die()
		End If
	End Sub

	' Token: 0x06001F38 RID: 7992 RVA: 0x0011E928 File Offset: 0x0011CD28
	Private Sub Die()
		MyBase.GetComponent(Of Collider2D)().enabled = False
		Me.StopAllCoroutines()
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub

	' Token: 0x06001F39 RID: 7993 RVA: 0x0011E948 File Offset: 0x0011CD48
	Private Iterator Function initialFlight_cr() As IEnumerator
		Dim wait As YieldInstruction = New WaitForFixedUpdate()
		While MyBase.transform.position.y < Me.pivotPoint.y
			MyBase.transform.position += Me.flightDirection * CSng(Me.currentSpeed) * CupheadTime.GlobalSpeed
			Yield wait
		End While
		Me.isActive = True
		Me.attackTime = 0F
		If MyBase.transform.position.x < Me.pivotPoint.x Then
			Me.flightDirection = Vector3.right * CSng(Me.currentSpeed)
		Else
			Me.flightDirection = Vector3.left * CSng(Me.currentSpeed)
		End If
		MyBase.StartCoroutine(Me.attackDelay_cr())
		Yield wait
		Return
	End Function

	' Token: 0x06001F3A RID: 7994 RVA: 0x0011E964 File Offset: 0x0011CD64
	Private Iterator Function attackDelay_cr() As IEnumerator
		While True
			Yield CupheadTime.WaitForSeconds(Me, Global.UnityEngine.Random.Range(Me.properties.miniFlowerShootDelay.min, Me.properties.miniFlowerShootDelay.max))
			If Not Me.isAttacking Then
				MyBase.animator.SetTrigger("OnAttack")
				Me.isAttacking = True
			End If
		End While
		Return
	End Function

	' Token: 0x06001F3B RID: 7995 RVA: 0x0011E980 File Offset: 0x0011CD80
	Protected Overrides Sub Awake()
		Me.damageDealer = DamageDealer.NewEnemy()
		Me.damageReceiver = MyBase.GetComponent(Of DamageReceiver)()
		AddHandler Me.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
		Me.isFriendly = False
		Me.flightDirection = Vector3.up
		Me.pivotPoint = New Vector3(CSng(Level.Current.Left) + CSng(Level.Current.Width) / 2.5F, CSng((Level.Current.Ceiling - Level.Current.Height / 8)), 0F)
		MyBase.Awake()
	End Sub

	' Token: 0x06001F3C RID: 7996 RVA: 0x0011EA18 File Offset: 0x0011CE18
	Private Sub Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
		Me.damageReceiver.enabled = Me.isAttacking
	End Sub

	' Token: 0x06001F3D RID: 7997 RVA: 0x0011EA44 File Offset: 0x0011CE44
	Protected Overrides Sub OnDestroy()
		AudioManager.Play("flower_minion_simple_deathpop_low")
		Me.emitAudioFromObject.Add("flower_minion_simple_deathpop_low")
		Me.StopAllCoroutines()
		RemoveHandler Me.parent.OnDeathEvent, AddressOf Me.HandleEnd
		MyBase.OnDestroy()
		Me.bulletPrefab = Nothing
		Me.petalA = Nothing
		Me.petalB = Nothing
	End Sub

	' Token: 0x06001F3E RID: 7998 RVA: 0x0011EAA3 File Offset: 0x0011CEA3
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
		MyBase.OnCollisionPlayer(hit, phase)
	End Sub

	' Token: 0x06001F3F RID: 7999 RVA: 0x0011EAC1 File Offset: 0x0011CEC1
	Private Sub OnIntroEnd()
		MyBase.StartCoroutine(Me.initialFlight_cr())
	End Sub

	' Token: 0x06001F40 RID: 8000 RVA: 0x0011EAD0 File Offset: 0x0011CED0
	Private Sub StartedShooting()
		Dim gameObject As GameObject = Global.UnityEngine.[Object].Instantiate(Of GameObject)(Me.bulletPrefab, Me.spawnPoint.transform.position, Quaternion.identity)
		If Me.isFriendly Then
			gameObject.GetComponent(Of FlowerLevelMiniFlowerBullet)().OnBulletSpawned(Me.parent.attackPoint.transform.position, Me.properties.miniFlowerProjectileSpeed, CSng(Me.properties.miniFlowerProjectileDamage), True)
		Else
			gameObject.GetComponent(Of FlowerLevelMiniFlowerBullet)().OnBulletSpawned(PlayerManager.GetNext().center, Me.properties.miniFlowerProjectileSpeed, CSng(Me.properties.miniFlowerProjectileDamage), False)
		End If
	End Sub

	' Token: 0x06001F41 RID: 8001 RVA: 0x0011EB73 File Offset: 0x0011CF73
	Private Sub EndedShooting()
		Me.isAttacking = False
	End Sub

	' Token: 0x040027C5 RID: 10181
	Private Const easingValue As Single = 0.03F

	' Token: 0x040027C6 RID: 10182
	Private Const strength As Single = 4000F

	' Token: 0x040027C7 RID: 10183
	Private attackTime As Single

	' Token: 0x040027C8 RID: 10184
	Private currentHP As Single

	' Token: 0x040027C9 RID: 10185
	Private currentSpeed As Integer

	' Token: 0x040027CA RID: 10186
	Private isFriendly As Boolean

	' Token: 0x040027CB RID: 10187
	Private isAttacking As Boolean

	' Token: 0x040027CC RID: 10188
	Private isActive As Boolean

	' Token: 0x040027CD RID: 10189
	Private flightDirection As Vector3

	' Token: 0x040027CE RID: 10190
	Private pivotPoint As Vector3

	' Token: 0x040027CF RID: 10191
	Private parent As FlowerLevelFlower

	' Token: 0x040027D0 RID: 10192
	<SerializeField()>
	Private bulletPrefab As GameObject

	' Token: 0x040027D1 RID: 10193
	<SerializeField()>
	Private spawnPoint As GameObject

	' Token: 0x040027D2 RID: 10194
	<SerializeField()>
	Private explosion As Transform

	' Token: 0x040027D3 RID: 10195
	<SerializeField()>
	Private petalA As GameObject

	' Token: 0x040027D4 RID: 10196
	<SerializeField()>
	Private petalB As GameObject

	' Token: 0x040027D5 RID: 10197
	Private properties As LevelProperties.Flower.EnemyPlants

	' Token: 0x040027D6 RID: 10198
	Private damageDealer As DamageDealer

	' Token: 0x040027D7 RID: 10199
	Private damageReceiver As DamageReceiver

	' Token: 0x040027D8 RID: 10200
	Private isDead As Boolean
End Class
