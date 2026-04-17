Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000661 RID: 1633
Public Class FlyingCowboyLevelUFO
	Inherits AbstractProjectile

	' Token: 0x17000391 RID: 913
	' (get) Token: 0x060021FD RID: 8701 RVA: 0x0013C92F File Offset: 0x0013AD2F
	Protected Overrides ReadOnly Property DestroyLifetime As Single
		Get
			Return 0F
		End Get
	End Property

	' Token: 0x060021FE RID: 8702 RVA: 0x0013C936 File Offset: 0x0013AD36
	Public Overridable Function Init(pos As Vector3, properties As LevelProperties.FlyingCowboy.UFOEnemy, health As Single) As FlyingCowboyLevelUFO
		MyBase.ResetLifetime()
		MyBase.ResetDistance()
		Me.startPos = pos
		MyBase.transform.position = pos
		Me.properties = properties
		Me.Health = health
		MyBase.StartCoroutine(Me.move_cr())
		Return Me
	End Function

	' Token: 0x060021FF RID: 8703 RVA: 0x0013C973 File Offset: 0x0013AD73
	Protected Overrides Sub Start()
		MyBase.Start()
		Me.damageReceiver = MyBase.GetComponent(Of DamageReceiver)()
		AddHandler Me.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
	End Sub

	' Token: 0x06002200 RID: 8704 RVA: 0x0013C99E File Offset: 0x0013AD9E
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		Me.Health -= info.damage
		If Me.Health < 0F AndAlso Not Me.isDead Then
			Level.Current.RegisterMinionKilled()
			Me.Respawn()
		End If
	End Sub

	' Token: 0x06002201 RID: 8705 RVA: 0x0013C9DE File Offset: 0x0013ADDE
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x06002202 RID: 8706 RVA: 0x0013C9FC File Offset: 0x0013ADFC
	Private Iterator Function move_cr() As IEnumerator
		Me.isDead = False
		Dim leftEdge As Single = -540F
		Dim initialLeftEdge As Single = leftEdge - 100F
		Dim rightEdge As Single = -640F + Me.properties.ufoPathLength
		Dim initialX As Single = MyBase.transform.position.x
		Dim travelDistance As Single = Mathf.Abs(initialX - initialLeftEdge)
		Dim travelTime As Single = travelDistance / Me.properties.introUFOSpeed
		Dim elapsedTime As Single = 0F
		While elapsedTime < travelTime
			elapsedTime += CupheadTime.FixedDelta
			Dim position As Vector3 = MyBase.transform.position
			position.x = EaseUtils.Ease(EaseUtils.EaseType.easeOutQuad, initialX, initialLeftEdge, elapsedTime / travelTime)
			MyBase.transform.position = position
			Yield New WaitForFixedUpdate()
		End While
		Me.movingLeft = False
		MyBase.StartCoroutine(Me.shoot_cr())
		Dim currentLeftEdge As Single = initialLeftEdge
		travelDistance = Mathf.Abs(rightEdge - leftEdge)
		travelTime = travelDistance / Me.properties.topUFOSpeed
		elapsedTime = 0F
		While True
			If elapsedTime < travelTime Then
				elapsedTime += CupheadTime.FixedDelta
				Dim position2 As Vector3 = MyBase.transform.position
				position2.x = EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, If((Not Me.movingLeft), currentLeftEdge, rightEdge), If((Not Me.movingLeft), rightEdge, currentLeftEdge), elapsedTime / travelTime)
				MyBase.transform.position = position2
			Else
				currentLeftEdge = leftEdge
				elapsedTime = 0F
				Me.movingLeft = Not Me.movingLeft
			End If
			Yield New WaitForFixedUpdate()
		End While
		Return
	End Function

	' Token: 0x06002203 RID: 8707 RVA: 0x0013CA18 File Offset: 0x0013AE18
	Private Iterator Function shoot_cr() As IEnumerator
		Dim shootString As PatternString = New PatternString(Me.properties.topUFOShootString, True, True)
		Dim parryString As PatternString = New PatternString(Me.properties.bulletParryString, True)
		While True
			Dim spreadAngle As MinMax = New MinMax(0F, Me.properties.spreadAngle)
			Yield CupheadTime.WaitForSeconds(Me, shootString.PopFloat())
			For i As Integer = 0 To Me.properties.bulletCount - 1
				Dim num As Single = spreadAngle.GetFloatAt(CSng(i) / (CSng(Me.properties.bulletCount) - 1F))
				Dim num2 As Single = spreadAngle.max / 2F
				num -= num2
				Dim num3 As Single = num + -90F
				Dim basicProjectile As BasicProjectile = Me.projectilePrefab.Create(MyBase.transform.position, num3, Me.properties.bulletSpeed)
				Dim flag As Boolean = parryString.PopLetter() = "P"c
				basicProjectile.SetParryable(flag)
				If flag Then
					basicProjectile.GetComponent(Of SpriteRenderer)().color = Color.magenta
				End If
			Next
		End While
		Return
	End Function

	' Token: 0x06002204 RID: 8708 RVA: 0x0013CA33 File Offset: 0x0013AE33
	Private Sub Respawn()
		Me.StopAllCoroutines()
		MyBase.StartCoroutine(Me.respawn_cr())
	End Sub

	' Token: 0x06002205 RID: 8709 RVA: 0x0013CA48 File Offset: 0x0013AE48
	Private Iterator Function respawn_cr() As IEnumerator
		Me.isDead = True
		MyBase.transform.position = New Vector3(1000F, 1000F)
		Dim waitTime As Single = Me.properties.topUFORespawnDelay
		Yield CupheadTime.WaitForSeconds(Me, waitTime)
		Me.Health = Me.properties.UFOHealth
		MyBase.transform.position = Me.startPos
		MyBase.StartCoroutine(Me.move_cr())
		Yield Nothing
		Return
	End Function

	' Token: 0x06002206 RID: 8710 RVA: 0x0013CA63 File Offset: 0x0013AE63
	Public Sub Dead()
		Me.StopAllCoroutines()
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub

	' Token: 0x04002AAD RID: 10925
	<SerializeField()>
	Private projectilePrefab As BasicProjectile

	' Token: 0x04002AAE RID: 10926
	Private Const LEFT_OFFSET As Single = 100F

	' Token: 0x04002AAF RID: 10927
	Private Const INITIAL_LEFT_OFFSET As Single = 100F

	' Token: 0x04002AB0 RID: 10928
	Private properties As LevelProperties.FlyingCowboy.UFOEnemy

	' Token: 0x04002AB1 RID: 10929
	Private damageReceiver As DamageReceiver

	' Token: 0x04002AB2 RID: 10930
	Private startPos As Vector3

	' Token: 0x04002AB3 RID: 10931
	Private isDead As Boolean

	' Token: 0x04002AB4 RID: 10932
	Private movingLeft As Boolean

	' Token: 0x04002AB5 RID: 10933
	Private Health As Single
End Class
