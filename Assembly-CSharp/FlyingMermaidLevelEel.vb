Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000683 RID: 1667
Public Class FlyingMermaidLevelEel
	Inherits AbstractCollidableObject

	' Token: 0x1700039F RID: 927
	' (get) Token: 0x06002329 RID: 9001 RVA: 0x0014A37F File Offset: 0x0014877F
	' (set) Token: 0x0600232A RID: 9002 RVA: 0x0014A387 File Offset: 0x00148787
	Public Property state As FlyingMermaidLevelEel.State

	' Token: 0x0600232B RID: 9003 RVA: 0x0014A390 File Offset: 0x00148790
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.damageDealer = DamageDealer.NewEnemy()
		Me.damageReceiver = MyBase.GetComponent(Of DamageReceiver)()
		AddHandler Me.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
	End Sub

	' Token: 0x0600232C RID: 9004 RVA: 0x0014A3C6 File Offset: 0x001487C6
	Private Sub Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x0600232D RID: 9005 RVA: 0x0014A3DE File Offset: 0x001487DE
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		Me.hp -= info.damage
		If Me.hp < 0F AndAlso Me.state = FlyingMermaidLevelEel.State.Spawned Then
			Me.Die(True, False)
		End If
	End Sub

	' Token: 0x0600232E RID: 9006 RVA: 0x0014A418 File Offset: 0x00148818
	Public Sub Die(explode As Boolean, permanent As Boolean)
		Dim component As Collider2D = MyBase.GetComponent(Of Collider2D)()
		If Not component.enabled Then
			Return
		End If
		Me.StopAllCoroutines()
		component.enabled = False
		MyBase.animator.SetTrigger("Despawn")
		MyBase.animator.ResetTrigger("Attack")
		MyBase.animator.ResetTrigger("Continue")
		MyBase.animator.ResetTrigger("Leave")
		Dim num As Single = CSng(If((Not(MyBase.GetComponent(Of SpriteRenderer)().sortingLayerName = "Foreground")), (-270), (-380)))
		If explode AndAlso Me.state = FlyingMermaidLevelEel.State.Spawned Then
			Dim component2 As SpriteRenderer = MyBase.GetComponent(Of SpriteRenderer)()
			For i As Integer = 0 To Me.numSegments - 1
				Dim floatAt As Single = Me.segmentY.GetFloatAt(CSng(i) / (CSng(Me.numSegments) - 1F))
				Dim vector As Vector2 = MyBase.transform.position
				vector.y += floatAt
				If vector.y >= num + 30F Then
					Dim flyingMermaidLevelEelSegment As FlyingMermaidLevelEelSegment
					If i = Me.numSegments - 1 Then
						flyingMermaidLevelEelSegment = Me.headSegmentPrefab
					Else
						flyingMermaidLevelEelSegment = Me.bodySegmentPrefabs.RandomChoice()
					End If
					Dim text As String = component2.sortingLayerName
					Dim num2 As Integer = component2.sortingOrder
					Dim num3 As Integer = Global.UnityEngine.Random.Range(-1, 2)
					If text = "Foreground" Then
						If num3 = -1 Then
							text = "Enemies"
							num2 = 1000
						ElseIf num3 = 1 Then
							num2 = 21
						End If
					ElseIf num3 = -1 Then
						text = "Background"
						num2 = 75
					ElseIf num3 = 1 Then
						text = "Foreground"
						num2 = 1
					End If
					flyingMermaidLevelEelSegment.Create(vector, text, num2)
				End If
			Next
		End If
		If Not permanent Then
			MyBase.StartCoroutine(Me.main_cr())
		End If
		Me.state = FlyingMermaidLevelEel.State.Unspawned
	End Sub

	' Token: 0x0600232F RID: 9007 RVA: 0x0014A60A File Offset: 0x00148A0A
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x06002330 RID: 9008 RVA: 0x0014A628 File Offset: 0x00148A28
	Public Sub Init(properties As LevelProperties.FlyingMermaid.Eel)
		Me.properties = properties
		Me.initialY = MyBase.transform.localPosition.y
		Dim component As Collider2D = MyBase.GetComponent(Of Collider2D)()
		component.enabled = False
		Me.bulletPinkPattern = properties.bulletPinkString.Split(New Char() { ","c })
		Me.bulletPinkIndex = Global.UnityEngine.Random.Range(0, Me.bulletPinkPattern.Length)
	End Sub

	' Token: 0x06002331 RID: 9009 RVA: 0x0014A693 File Offset: 0x00148A93
	Public Sub StartPattern()
		MyBase.StartCoroutine(Me.main_cr())
	End Sub

	' Token: 0x06002332 RID: 9010 RVA: 0x0014A6A4 File Offset: 0x00148AA4
	Public Iterator Function main_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, Me.properties.appearDelay.RandomFloat())
		Me.state = FlyingMermaidLevelEel.State.Spawned
		MyBase.animator.SetTrigger("Spawn")
		AudioManager.Play("level_mermaid_eel_intro")
		Dim t As Single = 0F
		Me.hp = Me.properties.hp
		Dim collider As Collider2D = MyBase.GetComponent(Of Collider2D)()
		collider.enabled = True
		While t < Me.riseTime - 0.25F
			t += CupheadTime.Delta
			MyBase.transform.SetLocalPosition(Nothing, New Single?(Mathf.Lerp(Me.initialY - Me.riseDistance, Me.initialY, t / Me.riseTime)), Nothing)
			Yield Nothing
		End While
		MyBase.animator.SetTrigger("Continue")
		While t < Me.riseTime
			t += CupheadTime.Delta
			MyBase.transform.SetLocalPosition(Nothing, New Single?(Mathf.Lerp(Me.initialY - Me.riseDistance, Me.initialY, t / Me.riseTime)), Nothing)
			Yield Nothing
		End While
		MyBase.transform.SetLocalPosition(Nothing, New Single?(Me.initialY), Nothing)
		Yield MyBase.animator.WaitForAnimationToStart(Me, "Idle", False)
		For numAttacks As Integer = Me.properties.attackAmount.RandomInt()To 0 Step -1
			Yield CupheadTime.WaitForSeconds(Me, Me.properties.idleTime.RandomFloat())
			AudioManager.Play("level_mermaid_eel_attack_start")
			MyBase.animator.SetTrigger("Attack")
			Yield MyBase.animator.WaitForAnimationToEnd(Me, "Attack_Start", False, True)
			Me.FireProjectiles()
			Yield MyBase.animator.WaitForAnimationToEnd(Me, "Attack_End", False, True)
			AudioManager.Play("level_mermaid_eel_attack_end")
		Next
		Yield CupheadTime.WaitForSeconds(Me, Me.properties.idleTime.RandomFloat())
		MyBase.animator.SetTrigger("Leave")
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Leave_Start", False, True)
		AudioManager.Play("level_mermaid_eel_attack_leave")
		t = 0F
		Dim spawnedSplash As Boolean = False
		Dim sprite As SpriteRenderer = MyBase.GetComponent(Of SpriteRenderer)()
		Dim waterY As Single = CSng(If((Not(sprite.sortingLayerName = "Foreground")), (-270), (-380)))
		While t < Me.leaveTime
			t += CupheadTime.Delta
			MyBase.transform.SetLocalPosition(Nothing, New Single?(Mathf.Lerp(Me.initialY, Me.initialY - Me.riseDistance, t / Me.leaveTime)), Nothing)
			If Not spawnedSplash AndAlso MyBase.transform.position.y < waterY - 80F Then
				FlyingMermaidLevelSplashManager.Instance.SpawnSplashMedium(MyBase.gameObject, 35F, True, waterY + 80F)
				spawnedSplash = True
			End If
			Yield Nothing
		End While
		Me.Die(False, False)
		Return
	End Function

	' Token: 0x06002333 RID: 9011 RVA: 0x0014A6C0 File Offset: 0x00148AC0
	Private Sub FireProjectiles()
		Dim num As Integer = 0
		While CSng(num) < Me.properties.numBullets
			Dim floatAt As Single = Me.properties.spreadAngle.GetFloatAt(CSng(num) / (Me.properties.numBullets - 1F))
			Dim basicProjectile As BasicProjectile = Me.projectilePrefab.Create(Me.projectileRoot.position, floatAt, Me.properties.bulletSpeed)
			basicProjectile.SetParryable(Me.bulletPinkPattern(Me.bulletPinkIndex)(0) = "P"c)
			Me.bulletPinkIndex = (Me.bulletPinkIndex + 1) Mod Me.bulletPinkPattern.Length
			num += 1
		End While
	End Sub

	' Token: 0x04002BCE RID: 11214
	<SerializeField()>
	Private riseTime As Single

	' Token: 0x04002BCF RID: 11215
	<SerializeField()>
	Private riseDistance As Single

	' Token: 0x04002BD0 RID: 11216
	<SerializeField()>
	Private leaveTime As Single

	' Token: 0x04002BD1 RID: 11217
	<SerializeField()>
	Private segmentY As MinMax

	' Token: 0x04002BD2 RID: 11218
	<SerializeField()>
	Private numSegments As Integer

	' Token: 0x04002BD3 RID: 11219
	<SerializeField()>
	Private projectilePrefab As BasicProjectile

	' Token: 0x04002BD4 RID: 11220
	<SerializeField()>
	Private projectileRoot As Transform

	' Token: 0x04002BD5 RID: 11221
	<SerializeField()>
	Private headSegmentPrefab As FlyingMermaidLevelEelSegment

	' Token: 0x04002BD6 RID: 11222
	<SerializeField()>
	Private bodySegmentPrefabs As FlyingMermaidLevelEelSegment()

	' Token: 0x04002BD7 RID: 11223
	Private properties As LevelProperties.FlyingMermaid.Eel

	' Token: 0x04002BD8 RID: 11224
	Private damageDealer As DamageDealer

	' Token: 0x04002BD9 RID: 11225
	Private damageReceiver As DamageReceiver

	' Token: 0x04002BDA RID: 11226
	Private bulletPinkPattern As String()

	' Token: 0x04002BDB RID: 11227
	Private bulletPinkIndex As Integer

	' Token: 0x04002BDC RID: 11228
	Private initialY As Single

	' Token: 0x04002BDD RID: 11229
	Private hp As Single

	' Token: 0x02000684 RID: 1668
	Public Enum State
		' Token: 0x04002BDF RID: 11231
		Unspawned
		' Token: 0x04002BE0 RID: 11232
		Spawned
	End Enum
End Class
