Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000567 RID: 1383
Public Class ClownLevelDucks
	Inherits AbstractProjectile

	' Token: 0x06001A08 RID: 6664 RVA: 0x000EDF3C File Offset: 0x000EC33C
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.bombDropped = False
		Me.damageReceiver = MyBase.GetComponent(Of DamageReceiver)()
		AddHandler Me.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
		AddHandler Me.body.GetComponent(Of DamageReceiver)().OnDamageTaken, AddressOf Me.OnDamageTaken
		Me.collisionChild = Me.body.GetComponent(Of CollisionChild)()
		AddHandler Me.collisionChild.OnPlayerCollision, AddressOf Me.OnCollisionPlayer
		If Me.isBombDuck Then
			Me.bomb.GetComponent(Of Transform)()
			AddHandler Me.bomb.GetComponent(Of DamageReceiver)().OnDamageTaken, AddressOf Me.OnDamageTaken
		End If
	End Sub

	' Token: 0x06001A09 RID: 6665 RVA: 0x000EDFF1 File Offset: 0x000EC3F1
	Public Function Init(pos As Vector2, properties As LevelProperties.Clown.Duck, maxYPos As Single, speedY As Single) As ClownLevelDucks
		Me.properties = properties
		MyBase.transform.position = pos
		Me.maxYPos = maxYPos
		Me.speedY = speedY
		Me.originalSpeed = Me.speedY
		Return Me
	End Function

	' Token: 0x06001A0A RID: 6666 RVA: 0x000EE027 File Offset: 0x000EC427
	Protected Overrides Sub Start()
		MyBase.Start()
		MyBase.StartCoroutine(Me.move_cr())
	End Sub

	' Token: 0x06001A0B RID: 6667 RVA: 0x000EE03C File Offset: 0x000EC43C
	Protected Overrides Sub Update()
		MyBase.Update()
		Me.VaryingSpeed()
		If Me.isBombDuck Then
			Me.BombCheck()
		End If
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x06001A0C RID: 6668 RVA: 0x000EE071 File Offset: 0x000EC471
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		MyBase.StartCoroutine(Me.spin_cr())
		If Me.isBombDuck AndAlso Not Me.bombDropped Then
			MyBase.StartCoroutine(Me.drop_bomb_cr())
		End If
	End Sub

	' Token: 0x06001A0D RID: 6669 RVA: 0x000EE0A3 File Offset: 0x000EC4A3
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x06001A0E RID: 6670 RVA: 0x000EE0C1 File Offset: 0x000EC4C1
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me.sparkPrefab = Nothing
		Me.explosionPrefab = Nothing
		Me.smokePrefab = Nothing
	End Sub

	' Token: 0x06001A0F RID: 6671 RVA: 0x000EE0E0 File Offset: 0x000EC4E0
	Private Iterator Function move_cr() As IEnumerator
		Dim goingUp As Boolean = False
		Dim stopDist As Single = 100F
		Dim endPos As Single = 360F - Me.maxYPos
		Me.slowDown = True
		While MyBase.transform.position.x > -840F
			Dim pos As Vector3 = MyBase.transform.position
			While MyBase.transform.position.y <> endPos
				Dim dist As Single = MyBase.transform.position.y - endPos
				dist = Mathf.Abs(dist)
				If dist < stopDist Then
					Me.slowDown = True
				End If
				pos.x -= Me.properties.duckXMovementSpeed * CupheadTime.Delta
				pos.y = Mathf.MoveTowards(MyBase.transform.position.y, endPos, Me.speedY * CupheadTime.Delta)
				MyBase.transform.position = pos
				Yield Nothing
			End While
			goingUp = Not goingUp
			endPos = If((Not goingUp), (360F - Me.maxYPos), 360F)
			Yield Nothing
		End While
		Me.Die()
		Yield Nothing
		Return
	End Function

	' Token: 0x06001A10 RID: 6672 RVA: 0x000EE0FC File Offset: 0x000EC4FC
	Private Sub VaryingSpeed()
		Dim num As Single = 4F
		If Me.slowDown Then
			If Me.speedY <= Me.originalSpeed / 3F Then
				Me.slowDown = False
			Else
				Me.speedY -= num
			End If
		ElseIf Me.speedY < Me.originalSpeed Then
			Me.speedY += num
		Else
			Me.speedY = Me.originalSpeed
		End If
	End Sub

	' Token: 0x06001A11 RID: 6673 RVA: 0x000EE180 File Offset: 0x000EC580
	Private Iterator Function spin_cr() As IEnumerator
		AudioManager.Play("clown_regular_duck_spin")
		Me.emitAudioFromObject.Add("clown_regular_duck_spin")
		Dim spark As Effect = Global.UnityEngine.[Object].Instantiate(Of Effect)(Me.sparkPrefab)
		spark.transform.position = MyBase.transform.position
		MyBase.animator.SetBool("Spin", True)
		MyBase.transform.GetComponent(Of Collider2D)().enabled = False
		Me.body.transform.GetComponent(Of Collider2D)().enabled = False
		Yield CupheadTime.WaitForSeconds(Me, Me.properties.spinDuration)
		MyBase.animator.SetBool("Spin", False)
		MyBase.transform.GetComponent(Of Collider2D)().enabled = True
		Me.body.transform.GetComponent(Of Collider2D)().enabled = True
		Yield Nothing
		Return
	End Function

	' Token: 0x06001A12 RID: 6674 RVA: 0x000EE19C File Offset: 0x000EC59C
	Private Sub BombCheck()
		Me.player = PlayerManager.GetNext()
		Dim num As Single = 10F
		Dim num2 As Single = Me.player.transform.position.x - MyBase.transform.position.x
		If num2 > -num AndAlso num2 < num AndAlso Not Me.bombDropped Then
			MyBase.StartCoroutine(Me.drop_bomb_cr())
		End If
	End Sub

	' Token: 0x06001A13 RID: 6675 RVA: 0x000EE210 File Offset: 0x000EC610
	Private Iterator Function drop_bomb_cr() As IEnumerator
		Dim vel As Single = Me.properties.bombSpeed
		Dim acceleration As Single = 5F
		Me.bombDropped = True
		Me.bomb.transform.parent = Nothing
		Me.bomb.GetComponent(Of Animator)().SetBool("Fall", True)
		While Me.bomb.transform.position.y > CSng(Level.Current.Ground)
			Me.bomb.transform.AddPosition(0F, -vel * CupheadTime.Delta, 0F)
			vel += acceleration
			Yield Nothing
		End While
		Me.bomb.GetComponent(Of Animator)().SetBool("Fall", False)
		Dim explosion As Effect = Global.UnityEngine.[Object].Instantiate(Of Effect)(Me.explosionPrefab)
		explosion.transform.position = Me.bomb.transform.position
		Dim num As Integer = Global.UnityEngine.Random.Range(0, 3)
		If num = 3 Then
			Dim effect As Effect = Global.UnityEngine.[Object].Instantiate(Of Effect)(Me.smokePrefab)
			effect.transform.position = Me.bomb.transform.position
		End If
		AudioManager.Play("clown_bulb_explosion")
		Me.emitAudioFromObject.Add("clown_bulb_explosion")
		Me.CreatePieces()
		Global.UnityEngine.[Object].Destroy(Me.bomb.gameObject)
		Return
	End Function

	' Token: 0x06001A14 RID: 6676 RVA: 0x000EE22C File Offset: 0x000EC62C
	Private Sub CreatePieces()
		For Each spriteDeathParts As SpriteDeathParts In Me.deathParts
			spriteDeathParts.CreatePart(Me.bomb.transform.position)
		Next
	End Sub

	' Token: 0x06001A15 RID: 6677 RVA: 0x000EE26F File Offset: 0x000EC66F
	Public Overrides Sub OnParry(player As AbstractPlayerController)
		MyBase.animator.SetBool("Spin", True)
		MyBase.transform.GetComponent(Of Collider2D)().enabled = False
		Me.body.transform.GetComponent(Of Collider2D)().enabled = False
	End Sub

	' Token: 0x06001A16 RID: 6678 RVA: 0x000EE2A9 File Offset: 0x000EC6A9
	Protected Overrides Sub Die()
		Me.StopAllCoroutines()
		MyBase.Die()
	End Sub

	' Token: 0x0400232C RID: 9004
	Public isBombDuck As Boolean

	' Token: 0x0400232D RID: 9005
	<SerializeField()>
	Private explosionPrefab As Effect

	' Token: 0x0400232E RID: 9006
	<SerializeField()>
	Private smokePrefab As Effect

	' Token: 0x0400232F RID: 9007
	<SerializeField()>
	Private sparkPrefab As Effect

	' Token: 0x04002330 RID: 9008
	<SerializeField()>
	Private deathParts As SpriteDeathParts()

	' Token: 0x04002331 RID: 9009
	<SerializeField()>
	Private bomb As Transform

	' Token: 0x04002332 RID: 9010
	<SerializeField()>
	Private body As GameObject

	' Token: 0x04002333 RID: 9011
	Private properties As LevelProperties.Clown.Duck

	' Token: 0x04002334 RID: 9012
	Private player As AbstractPlayerController

	' Token: 0x04002335 RID: 9013
	Private damageReceiver As DamageReceiver

	' Token: 0x04002336 RID: 9014
	Private collisionChild As CollisionChild

	' Token: 0x04002337 RID: 9015
	Private maxYPos As Single

	' Token: 0x04002338 RID: 9016
	Private speedY As Single

	' Token: 0x04002339 RID: 9017
	Private originalSpeed As Single

	' Token: 0x0400233A RID: 9018
	Private slowDown As Boolean

	' Token: 0x0400233B RID: 9019
	Private bombDropped As Boolean
End Class
