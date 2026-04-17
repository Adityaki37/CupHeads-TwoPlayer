Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020005C6 RID: 1478
Public Class DicePalaceFlyingMemoryLevelBot
	Inherits AbstractProjectile

	' Token: 0x06001CDC RID: 7388 RVA: 0x0010869A File Offset: 0x00106A9A
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.damageReceiver = MyBase.GetComponent(Of DamageReceiver)()
		AddHandler Me.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
	End Sub

	' Token: 0x06001CDD RID: 7389 RVA: 0x001086C8 File Offset: 0x00106AC8
	Public Sub Init(properties As LevelProperties.DicePalaceFlyingMemory.Bots, startingPoint As DicePalaceFlyingMemoryLevelContactPoint, moveOnY As Boolean, health As Single, player As AbstractPlayerController)
		MyBase.transform.position = startingPoint.transform.position
		Me.currentPoint = startingPoint
		Me.health = health
		Me.player = player
		Me.moveOnY = moveOnY
		Me.properties = properties
		MyBase.transform.SetScale(New Single?(properties.botsScale), New Single?(properties.botsScale), Nothing)
		Me.gameManager = DicePalaceFlyingMemoryLevelGameManager.Instance
		Me.targetPoint = Me.gameManager.contactPoints(1, 1)
		Me.movementString = properties.movementString.GetRandom().Split(New Char() { ","c })
		Me.directionString = properties.directionString.GetRandom().Split(New Char() { ","c })
		Me.movementIndex = Global.UnityEngine.Random.Range(0, Me.movementString.Length)
		MyBase.StartCoroutine(Me.move_cr())
	End Sub

	' Token: 0x06001CDE RID: 7390 RVA: 0x001087C0 File Offset: 0x00106BC0
	Protected Overrides Sub Update()
		MyBase.Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x06001CDF RID: 7391 RVA: 0x001087DE File Offset: 0x00106BDE
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x06001CE0 RID: 7392 RVA: 0x001087FC File Offset: 0x00106BFC
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		Me.health -= info.damage
		If Me.health < 0F Then
			Me.Die()
		End If
	End Sub

	' Token: 0x06001CE1 RID: 7393 RVA: 0x00108828 File Offset: 0x00106C28
	Private Sub CalculateYTarget(followPlayer As Boolean)
		Me.reachedEnd = False
		If followPlayer Then
			If Me.player.transform.position.y <= MyBase.transform.position.y Then
				Me.MoveUp()
			Else
				Me.MoveDown()
			End If
		ElseIf Me.currentPoint.Ycoord <= (Me.gameManager.contactDimY - 1) / 2 Then
			Me.MoveUp()
		Else
			Me.MoveDown()
		End If
		If Me.reachedEnd Then
			Me.targetPos.y = Me.gameManager.contactPoints(Me.currentPoint.Xcoord, Me.setPosition).transform.position.y + Me.offsetEnd
		Else
			Me.targetPoint = Me.gameManager.contactPoints(Me.currentPoint.Xcoord, Me.setPosition)
			Me.targetPos = Me.targetPoint.transform.position
		End If
	End Sub

	' Token: 0x06001CE2 RID: 7394 RVA: 0x0010894C File Offset: 0x00106D4C
	Private Sub CalculateXTarget(followPlayer As Boolean)
		Me.reachedEnd = False
		If followPlayer Then
			If Me.player.transform.position.x <= MyBase.transform.position.x Then
				Me.MoveRight()
			Else
				Me.MoveLeft()
			End If
		ElseIf Me.currentPoint.Xcoord <= (Me.gameManager.contactDimX - 1) / 2 Then
			Me.MoveRight()
		Else
			Me.MoveLeft()
		End If
		If Me.reachedEnd Then
			Me.targetPos.x = Me.gameManager.contactPoints(Me.setPosition, Me.currentPoint.Ycoord).transform.position.x + Me.offsetEnd
		Else
			Me.targetPoint = Me.gameManager.contactPoints(Me.setPosition, Me.currentPoint.Ycoord)
			Me.targetPos = Me.targetPoint.transform.position
		End If
	End Sub

	' Token: 0x06001CE3 RID: 7395 RVA: 0x00108A70 File Offset: 0x00106E70
	Private Iterator Function move_cr() As IEnumerator
		Dim followPlayer As Boolean = False
		Dim pos As Vector3 = MyBase.transform.position
		While True
			Parser.IntTryParse(Me.movementString(Me.movementIndex), Me.movement)
			If Me.player Is Nothing OrElse Me.player.IsDead Then
				Me.player = PlayerManager.GetNext()
			End If
			Me.GetMovement(followPlayer)
			If Me.moveOnY Then
				Me.CalculateYTarget(followPlayer)
			Else
				Me.CalculateXTarget(followPlayer)
			End If
			If Me.moveOnY Then
				While MyBase.transform.position.y <> Me.targetPos.y
					pos.y = Mathf.MoveTowards(MyBase.transform.position.y, Me.targetPos.y, Me.properties.botsSpeed * CupheadTime.Delta)
					MyBase.transform.position = pos
					Yield Nothing
				End While
			Else
				While MyBase.transform.position.x <> Me.targetPos.x
					pos.x = Mathf.MoveTowards(MyBase.transform.position.x, Me.targetPos.x, Me.properties.botsSpeed * CupheadTime.Delta)
					MyBase.transform.position = pos
					Yield Nothing
				End While
			End If
			If Me.reachedEnd Then
				Me.OnDestroy()
			Else
				Me.currentPoint = Me.targetPoint
				Me.moveOnY = Not Me.moveOnY
				Me.movementIndex = (Me.movementIndex + 1) Mod Me.movementString.Length
				Me.directionIndex = (Me.directionIndex + 1) Mod Me.directionString.Length
			End If
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06001CE4 RID: 7396 RVA: 0x00108A8C File Offset: 0x00106E8C
	Private Function GetMovement(followPlayer As Boolean) As Boolean
		If Me.directionString(Me.directionIndex)(0) = "N"c Then
			followPlayer = False
		ElseIf Me.directionString(Me.directionIndex)(0) = "P"c Then
			followPlayer = True
		End If
		Return followPlayer
	End Function

	' Token: 0x06001CE5 RID: 7397 RVA: 0x00108ADC File Offset: 0x00106EDC
	Private Sub MoveUp()
		Dim num As Integer = Me.currentPoint.Ycoord + Me.movement
		If num > Me.gameManager.contactDimY - 1 Then
			Me.setPosition = Me.gameManager.contactDimY - 1
			Me.reachedEnd = True
			Me.offsetEnd = -200F
		Else
			Me.setPosition = num
			Me.reachedEnd = False
		End If
	End Sub

	' Token: 0x06001CE6 RID: 7398 RVA: 0x00108B48 File Offset: 0x00106F48
	Private Sub MoveDown()
		Dim num As Integer = Me.currentPoint.Ycoord - Me.movement
		If num < 0 Then
			Me.setPosition = 0
			Me.reachedEnd = True
			Me.offsetEnd = 200F
		Else
			Me.setPosition = num
			Me.reachedEnd = False
		End If
	End Sub

	' Token: 0x06001CE7 RID: 7399 RVA: 0x00108B9C File Offset: 0x00106F9C
	Private Sub MoveLeft()
		Dim num As Integer = Me.currentPoint.Xcoord - Me.movement
		If num < 0 Then
			Me.setPosition = 0
			Me.reachedEnd = True
			Me.offsetEnd = -200F
		Else
			Me.setPosition = num
			Me.reachedEnd = False
		End If
	End Sub

	' Token: 0x06001CE8 RID: 7400 RVA: 0x00108BF0 File Offset: 0x00106FF0
	Private Sub MoveRight()
		Dim num As Integer = Me.currentPoint.Xcoord + Me.movement
		If num > Me.gameManager.contactDimX - 1 Then
			Me.setPosition = Me.gameManager.contactDimX - 1
			Me.reachedEnd = True
			Me.offsetEnd = 200F
		Else
			Me.setPosition = num
			Me.reachedEnd = False
		End If
	End Sub

	' Token: 0x06001CE9 RID: 7401 RVA: 0x00108C5C File Offset: 0x0010705C
	Private Iterator Function shoot_bullets_cr() As IEnumerator
		While True
			MyBase.animator.Play("Bot_Warning")
			Yield CupheadTime.WaitForSeconds(Me, Me.properties.bulletWarningDuration)
			Me.FireProjectile()
			Me.player = PlayerManager.GetNext()
			MyBase.animator.Play("Off")
			Yield Nothing
			Yield CupheadTime.WaitForSeconds(Me, Me.properties.bulletDelay)
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06001CEA RID: 7402 RVA: 0x00108C78 File Offset: 0x00107078
	Private Sub FireProjectile()
		If Me.player Is Nothing OrElse Me.player.IsDead Then
			Me.player = PlayerManager.GetNext()
		End If
		Dim vector As Vector3 = Me.player.transform.position - MyBase.transform.position
		Dim num As Single = MathUtils.DirectionToAngle(vector)
		Me.projectile.Create(MyBase.transform.position, num, Me.properties.bulletSpeed)
	End Sub

	' Token: 0x06001CEB RID: 7403 RVA: 0x00108D06 File Offset: 0x00107106
	Protected Overrides Sub Die()
		Me.StopAllCoroutines()
		MyBase.GetComponent(Of SpriteRenderer)().enabled = False
		MyBase.OnDestroy()
		MyBase.Die()
	End Sub

	' Token: 0x040025C4 RID: 9668
	<SerializeField()>
	Private projectile As BasicProjectile

	' Token: 0x040025C5 RID: 9669
	Private gameManager As DicePalaceFlyingMemoryLevelGameManager

	' Token: 0x040025C6 RID: 9670
	Private properties As LevelProperties.DicePalaceFlyingMemory.Bots

	' Token: 0x040025C7 RID: 9671
	Private currentPoint As DicePalaceFlyingMemoryLevelContactPoint

	' Token: 0x040025C8 RID: 9672
	Private targetPoint As DicePalaceFlyingMemoryLevelContactPoint

	' Token: 0x040025C9 RID: 9673
	Private player As AbstractPlayerController

	' Token: 0x040025CA RID: 9674
	Private damageReceiver As DamageReceiver

	' Token: 0x040025CB RID: 9675
	Private moveOnY As Boolean

	' Token: 0x040025CC RID: 9676
	Private reachedEnd As Boolean

	' Token: 0x040025CD RID: 9677
	Private health As Single

	' Token: 0x040025CE RID: 9678
	Private movement As Integer

	' Token: 0x040025CF RID: 9679
	Private movementIndex As Integer

	' Token: 0x040025D0 RID: 9680
	Private directionIndex As Integer

	' Token: 0x040025D1 RID: 9681
	Private setPosition As Integer

	' Token: 0x040025D2 RID: 9682
	Private offsetEnd As Single

	' Token: 0x040025D3 RID: 9683
	Private targetPos As Vector3

	' Token: 0x040025D4 RID: 9684
	Private movementString As String()

	' Token: 0x040025D5 RID: 9685
	Private directionString As String()
End Class
