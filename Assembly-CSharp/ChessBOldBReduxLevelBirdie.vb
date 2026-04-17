Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x0200052B RID: 1323
Public Class ChessBOldBReduxLevelBirdie
	Inherits AbstractProjectile

	' Token: 0x1700032F RID: 815
	' (get) Token: 0x060017E2 RID: 6114 RVA: 0x000D7CBB File Offset: 0x000D60BB
	Protected Overrides ReadOnly Property DestroyLifetime As Single
		Get
			Return 0F
		End Get
	End Property

	' Token: 0x060017E3 RID: 6115 RVA: 0x000D7CC4 File Offset: 0x000D60C4
	Public Sub Setup(pivotPoint As Transform, angle As Single, properties As LevelProperties.ChessBOldB.Birdie, loopSize As Single, chosenBall As Boolean)
		Me.angle = angle
		Me.pivotPoint = pivotPoint
		Me.properties = properties
		Me.loopSize = loopSize
		Me.isMoving = False
		Me.chosenBall = chosenBall
		Me.RepositionBall()
		Me.sprite.color = Me.defaultColor
		Me.SetParryable(False)
	End Sub

	' Token: 0x060017E4 RID: 6116 RVA: 0x000D7D1B File Offset: 0x000D611B
	Protected Overrides Sub Awake()
		Me.sprite = MyBase.GetComponent(Of SpriteRenderer)()
		MyBase.Awake()
	End Sub

	' Token: 0x060017E5 RID: 6117 RVA: 0x000D7D2F File Offset: 0x000D612F
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
		MyBase.OnCollisionPlayer(hit, phase)
	End Sub

	' Token: 0x060017E6 RID: 6118 RVA: 0x000D7D4D File Offset: 0x000D614D
	Protected Overrides Sub FixedUpdate()
		MyBase.FixedUpdate()
		If Me.isMoving Then
			Me.MoveBirdie()
		End If
	End Sub

	' Token: 0x060017E7 RID: 6119 RVA: 0x000D7D66 File Offset: 0x000D6166
	Public Sub StopMoving()
		Me.isMoving = False
	End Sub

	' Token: 0x060017E8 RID: 6120 RVA: 0x000D7D6F File Offset: 0x000D616F
	Public Sub HandleMovement(rotationTime As Single, goingClockwise As Boolean)
		Me.rotationTime = If((Not goingClockwise), (-rotationTime), rotationTime)
		Me.isMoving = True
	End Sub

	' Token: 0x060017E9 RID: 6121 RVA: 0x000D7D8C File Offset: 0x000D618C
	Public Sub RepositionBall()
		Dim zero As Vector3 = Vector3.zero
		Dim zero2 As Vector3 = Vector3.zero
		Me.angle *= 0.017453292F
		zero = New Vector3(Mathf.Sin(Me.angle) * Me.loopSize, 0F, 0F)
		zero2 = New Vector3(0F, Mathf.Cos(Me.angle) * Me.loopSize, 0F)
		MyBase.transform.position = Me.pivotPoint.position
		MyBase.transform.position += zero + zero2
		Me.offScreen = False
		Me.damageDealer.SetDamageFlags(False, False, False)
	End Sub

	' Token: 0x060017EA RID: 6122 RVA: 0x000D7E50 File Offset: 0x000D6250
	Private Sub MoveBirdie()
		Dim zero As Vector3 = Vector3.zero
		Dim zero2 As Vector3 = Vector3.zero
		Me.angle += Me.rotationTime * CupheadTime.FixedDelta
		zero = New Vector3(Mathf.Sin(Me.angle) * Me.loopSize, 0F, 0F)
		zero2 = New Vector3(0F, Mathf.Cos(Me.angle) * Me.loopSize, 0F)
		MyBase.transform.position = Me.pivotPoint.position
		MyBase.transform.position += zero + zero2
	End Sub

	' Token: 0x060017EB RID: 6123 RVA: 0x000D7F08 File Offset: 0x000D6308
	Public Overrides Sub OnParry(player As AbstractPlayerController)
		If Me.ParryBirdie IsNot Nothing Then
			Me.isMoving = False
			Me.ParryBirdie(Me.chosenBall)
			MyBase.StartCoroutine(Me.turn_off_collider_cr())
			If Not Me.chosenBall Then
				MyBase.StartCoroutine(Me.attack_cr())
			End If
		End If
	End Sub

	' Token: 0x060017EC RID: 6124 RVA: 0x000D7F5D File Offset: 0x000D635D
	Public Sub TurnPink()
		Me.SetParryable(True)
		Me.sprite.color = Me.pinkColor
	End Sub

	' Token: 0x060017ED RID: 6125 RVA: 0x000D7F78 File Offset: 0x000D6378
	Private Iterator Function turn_off_collider_cr() As IEnumerator
		MyBase.GetComponent(Of Collider2D)().enabled = False
		Yield CupheadTime.WaitForSeconds(Me, Me.properties.colliderOffTime)
		MyBase.GetComponent(Of Collider2D)().enabled = True
		Me.offScreen = False
		Me.damageDealer.SetDamageFlags(True, False, False)
		Return
	End Function

	' Token: 0x060017EE RID: 6126 RVA: 0x000D7F94 File Offset: 0x000D6394
	Private Iterator Function attack_cr() As IEnumerator
		Dim wait As YieldInstruction = New WaitForFixedUpdate()
		Dim player As AbstractPlayerController = PlayerManager.GetNext()
		Dim dir As Vector3 = player.transform.position - MyBase.transform.position
		Dim angle As Single = MathUtils.DirectionToAngle(dir)
		Dim changedDirection As Boolean = False
		Dim straightTime As Single = 0F
		Dim timeToStraight As Single = Me.properties.timeToStraight
		Dim arcTime As Single = 0F
		Dim timeToArc As Single = Me.properties.timeToMaxSpeed
		Dim xSpeedMinMax As MinMax = Me.properties.xSpeed
		Dim ySpeedMinMax As MinMax = Me.properties.ySpeed
		Dim xSpeed As Single = 0F
		Dim ySpeed As Single = 0F
		MyBase.StartCoroutine(Me.check_bounds_cr())
		While Not Me.offScreen
			If arcTime < timeToArc Then
				arcTime += CupheadTime.FixedDelta
				xSpeed = xSpeedMinMax.GetFloatAt(arcTime / timeToArc)
				ySpeed = ySpeedMinMax.GetFloatAt(1F - arcTime / timeToArc)
			End If
			If xSpeed > 0F AndAlso Not changedDirection Then
				If straightTime < timeToStraight Then
					straightTime += CupheadTime.FixedDelta
					dir = player.transform.position - MyBase.transform.position
					angle = MathUtils.DirectionToAngle(dir)
				Else
					changedDirection = True
				End If
			End If
			Dim speed As Vector3 = New Vector3(xSpeed, ySpeed)
			Dim rot As Quaternion = Quaternion.Euler(0F, 0F, angle)
			speed = rot * speed
			MyBase.transform.position += speed * CupheadTime.FixedDelta
			Yield wait
		End While
		Return
	End Function

	' Token: 0x060017EF RID: 6127 RVA: 0x000D7FB0 File Offset: 0x000D63B0
	Private Iterator Function check_bounds_cr() As IEnumerator
		Dim offset As Single = 200F
		While MyBase.transform.position.x < CSng(Level.Current.Right) + offset AndAlso MyBase.transform.position.x > CSng(Level.Current.Left) - offset AndAlso MyBase.transform.position.y < CSng(Level.Current.Ceiling) + offset AndAlso MyBase.transform.position.y > CSng(Level.Current.Ground) - offset
			Yield Nothing
		End While
		Me.offScreen = True
		Yield Nothing
		Return
	End Function

	' Token: 0x0400210E RID: 8462
	Private Const ONE As Single = 1F

	' Token: 0x0400210F RID: 8463
	Public ParryBirdie As ChessBOldBReduxLevelBirdie.OnParryBirdie

	' Token: 0x04002110 RID: 8464
	<SerializeField()>
	Private defaultColor As Color

	' Token: 0x04002111 RID: 8465
	<SerializeField()>
	Private pinkColor As Color

	' Token: 0x04002112 RID: 8466
	Private properties As LevelProperties.ChessBOldB.Birdie

	' Token: 0x04002113 RID: 8467
	Private sprite As SpriteRenderer

	' Token: 0x04002114 RID: 8468
	Private pivotPoint As Transform

	' Token: 0x04002115 RID: 8469
	Private timesToChangeDir As Integer

	' Token: 0x04002116 RID: 8470
	Private angle As Single

	' Token: 0x04002117 RID: 8471
	Private loopSize As Single

	' Token: 0x04002118 RID: 8472
	Private rotationTime As Single

	' Token: 0x04002119 RID: 8473
	Private isMoving As Boolean

	' Token: 0x0400211A RID: 8474
	Private chosenBall As Boolean

	' Token: 0x0400211B RID: 8475
	Private offScreen As Boolean

	' Token: 0x0200052C RID: 1324
	' (Invoke) Token: 0x060017F1 RID: 6129
	Public Delegate Sub OnParryBirdie(correctBall As Boolean)
End Class
