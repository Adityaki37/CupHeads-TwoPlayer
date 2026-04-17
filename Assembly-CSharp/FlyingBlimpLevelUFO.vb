Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000646 RID: 1606
Public Class FlyingBlimpLevelUFO
	Inherits AbstractCollidableObject

	' Token: 0x060020F4 RID: 8436 RVA: 0x001305DC File Offset: 0x0012E9DC
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.beamTriggered = False
		Me.damageReceiver = MyBase.GetComponent(Of DamageReceiver)()
		AddHandler Me.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
		Me.damageDealer = DamageDealer.NewEnemy()
		Me.collisionChild = Me.beamPrefab.GetComponent(Of CollisionChild)()
		AddHandler Me.collisionChild.OnPlayerCollision, AddressOf Me.OnCollisionPlayer
		Dim num As Single = CSng(Global.UnityEngine.Random.Range(0, 2))
		If num = 0F Then
			Me.beamPrefab.SetScale(New Single?(-MyBase.transform.localScale.x), New Single?(MyBase.transform.localScale.y), New Single?(MyBase.transform.localScale.z))
		End If
	End Sub

	' Token: 0x060020F5 RID: 8437 RVA: 0x001306B5 File Offset: 0x0012EAB5
	Private Sub Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x060020F6 RID: 8438 RVA: 0x001306CD File Offset: 0x0012EACD
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		Me.health -= info.damage
		If Me.health < 0F Then
			Me.Die()
		End If
	End Sub

	' Token: 0x060020F7 RID: 8439 RVA: 0x001306F8 File Offset: 0x0012EAF8
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x060020F8 RID: 8440 RVA: 0x00130718 File Offset: 0x0012EB18
	Public Sub Init(startPos As Vector2, midPos As Vector2, endPos As Vector2, speed As Single, health As Single, properties As LevelProperties.FlyingBlimp.UFO)
		MyBase.transform.position = startPos
		Me.ufoMidPoint = midPos
		Me.ufoStopPoint = endPos
		Me.speed = speed
		Me.properties = properties
		Me.health = health
		MyBase.StartCoroutine(Me.to_position_cr())
	End Sub

	' Token: 0x060020F9 RID: 8441 RVA: 0x00130774 File Offset: 0x0012EB74
	Private Iterator Function to_position_cr() As IEnumerator
		Dim wait As YieldInstruction = New WaitForFixedUpdate()
		While MyBase.transform.position <> Me.ufoMidPoint
			MyBase.transform.position = Vector3.MoveTowards(MyBase.transform.position, Me.ufoMidPoint, Me.speed * CupheadTime.FixedDelta)
			Yield wait
		End While
		MyBase.transform.GetComponent(Of SpriteRenderer)().sortingOrder = 3
		While MyBase.transform.position <> Me.ufoStopPoint
			MyBase.transform.position = Vector3.MoveTowards(MyBase.transform.position, Me.ufoStopPoint, Me.speed * CupheadTime.FixedDelta)
			Yield wait
		End While
		MyBase.StartCoroutine(Me.move_cr())
		Return
	End Function

	' Token: 0x060020FA RID: 8442 RVA: 0x00130790 File Offset: 0x0012EB90
	Private Iterator Function move_cr() As IEnumerator
		Dim wait As YieldInstruction = New WaitForFixedUpdate()
		Dim offset As Single = 50F
		While MyBase.transform.position.x > -640F - offset
			Me.player = PlayerManager.GetNext()
			Dim dist As Single = Me.player.transform.position.x - MyBase.transform.position.x
			Dim pos As Vector3 = MyBase.transform.position
			pos.x += -Me.speed * CupheadTime.FixedDelta
			MyBase.transform.position = pos
			Me.proximity = If((Not Me.typeB), Me.properties.UFOProximityA, Me.properties.UFOProximityB)
			If dist > -Me.proximity AndAlso dist < Me.proximity AndAlso Not Me.beamTriggered Then
				Me.beamTriggered = True
				MyBase.StartCoroutine(Me.ActivateBeam())
			End If
			Yield wait
		End While
		Me.Die()
		Return
	End Function

	' Token: 0x060020FB RID: 8443 RVA: 0x001307AC File Offset: 0x0012EBAC
	Private Iterator Function ActivateBeam() As IEnumerator
		MyBase.animator.SetTrigger("StartBeam")
		Yield CupheadTime.WaitForSeconds(Me, Me.properties.UFOWarningBeamDuration)
		AudioManager.Play("level_flying_blimp_moon_UFO_fire_laser")
		MyBase.animator.SetTrigger("Continue")
		Yield CupheadTime.WaitForSeconds(Me, Me.properties.beamDuration)
		MyBase.animator.SetTrigger("End")
		Return
	End Function

	' Token: 0x060020FC RID: 8444 RVA: 0x001307C7 File Offset: 0x0012EBC7
	Private Sub Die()
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub

	' Token: 0x0400298B RID: 10635
	Public typeB As Boolean

	' Token: 0x0400298C RID: 10636
	<SerializeField()>
	Private beamPrefab As Transform

	' Token: 0x0400298D RID: 10637
	Private damageDealer As DamageDealer

	' Token: 0x0400298E RID: 10638
	Private damageReceiver As DamageReceiver

	' Token: 0x0400298F RID: 10639
	Private collisionChild As CollisionChild

	' Token: 0x04002990 RID: 10640
	Private ufoMidPoint As Vector3

	' Token: 0x04002991 RID: 10641
	Private ufoStopPoint As Vector3

	' Token: 0x04002992 RID: 10642
	Private player As AbstractPlayerController

	' Token: 0x04002993 RID: 10643
	Private properties As LevelProperties.FlyingBlimp.UFO

	' Token: 0x04002994 RID: 10644
	Private speed As Single

	' Token: 0x04002995 RID: 10645
	Private health As Single

	' Token: 0x04002996 RID: 10646
	Private proximity As Single

	' Token: 0x04002997 RID: 10647
	Private beamTriggered As Boolean
End Class
