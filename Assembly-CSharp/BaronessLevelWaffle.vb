Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020004F7 RID: 1271
Public Class BaronessLevelWaffle
	Inherits BaronessLevelMiniBossBase

	' Token: 0x17000324 RID: 804
	' (get) Token: 0x06001655 RID: 5717 RVA: 0x000C7F94 File Offset: 0x000C6394
	' (set) Token: 0x06001656 RID: 5718 RVA: 0x000C7F9C File Offset: 0x000C639C
	Public Property state As BaronessLevelWaffle.State

	' Token: 0x06001657 RID: 5719 RVA: 0x000C7FA8 File Offset: 0x000C63A8
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Dim num As Single = CSng(Global.UnityEngine.Random.Range(0, 2))
		Me.pathA = num = 0F
		Me.check = True
		Me.isDead = False
		Me.isDying = False
		Me.damageDealer = DamageDealer.NewEnemy()
		Me.damageReceiver = MyBase.GetComponent(Of DamageReceiver)()
		AddHandler Me.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
		AddHandler Me.mouth.GetComponent(Of DamageReceiver)().OnDamageTaken, AddressOf Me.OnDamageTaken
		MyBase.GetComponent(Of Collider2D)().enabled = True
		For i As Integer = 0 To Me.diagonalPieces.Length - 1
			AddHandler Me.diagonalPieces(i).wafflepiece.GetComponent(Of DamageReceiver)().OnDamageTaken, AddressOf Me.OnDamageTaken
			Me.diagonalPieces(i).wafflepiece.GetComponent(Of Collider2D)().enabled = False
			AddHandler Me.diagonalPieces(i).wafflepiece.GetComponent(Of CollisionChild)().OnPlayerCollision, AddressOf Me.OnCollisionPlayer
		Next
		For j As Integer = 0 To Me.straightPieces.Length - 1
			AddHandler Me.straightPieces(j).wafflepiece.GetComponent(Of DamageReceiver)().OnDamageTaken, AddressOf Me.OnDamageTaken
			Me.straightPieces(j).wafflepiece.GetComponent(Of Collider2D)().enabled = False
			AddHandler Me.straightPieces(j).wafflepiece.GetComponent(Of CollisionChild)().OnPlayerCollision, AddressOf Me.OnCollisionPlayer
		Next
	End Sub

	' Token: 0x06001658 RID: 5720 RVA: 0x000C813B File Offset: 0x000C653B
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x06001659 RID: 5721 RVA: 0x000C815C File Offset: 0x000C655C
	Public Sub Init(properties As LevelProperties.Baroness.Waffle, pos As Vector2, pivot As Transform, speed As Single, health As Single)
		Me.properties = properties
		Me.speed = speed
		Me.health = health
		MyBase.transform.position = pos
		Me.pivotPoint = pivot
		Me.state = BaronessLevelWaffle.State.Enter
		MyBase.StartCoroutine(Me.enter_cr())
		MyBase.StartCoroutine(Me.switchLayer_cr())
	End Sub

	' Token: 0x0600165A RID: 5722 RVA: 0x000C81B9 File Offset: 0x000C65B9
	Private Sub Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x0600165B RID: 5723 RVA: 0x000C81D4 File Offset: 0x000C65D4
	Private Iterator Function switchLayer_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 3F)
		MyBase.gameObject.GetComponent(Of SpriteRenderer)().sortingLayerName = SpriteLayer.Enemies.ToString()
		MyBase.gameObject.GetComponent(Of SpriteRenderer)().sortingOrder = 2
		Return
	End Function

	' Token: 0x0600165C RID: 5724 RVA: 0x000C81F0 File Offset: 0x000C65F0
	Protected Overrides Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		If Me.health > 0F Then
			MyBase.OnDamageTaken(info)
		End If
		Me.health -= info.damage
		If Me.health < 0F AndAlso Me.state = BaronessLevelWaffle.State.Move Then
			Dim damageInfo As DamageDealer.DamageInfo = New DamageDealer.DamageInfo(Me.health, info.direction, info.origin, info.damageSource)
			MyBase.OnDamageTaken(damageInfo)
			Me.isDead = True
			MyBase.StartCoroutine(Me.death_cr())
		End If
	End Sub

	' Token: 0x0600165D RID: 5725 RVA: 0x000C827C File Offset: 0x000C667C
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me.explosion = Nothing
		Me.explosionReverse = Nothing
		Me.straightPieces = Nothing
		Me.diagonalPieces = Nothing
	End Sub

	' Token: 0x0600165E RID: 5726 RVA: 0x000C82A0 File Offset: 0x000C66A0
	Private Iterator Function enter_cr() As IEnumerator
		Dim wait As YieldInstruction = New WaitForFixedUpdate()
		Me.originalPivotPos = Me.pivotPoint.transform.position
		If Me.pathA Then
			Me.startPos = Me.pivotPoint.position + Vector3.right * Me.loopSize
			Me.angle = -1.5707964F
		Else
			Me.startPos = Me.pivotPoint.position + Vector3.down * Me.loopSize
			Me.angle = 3.1415927F
			Me.speed = -Me.speed
		End If
		While MyBase.transform.position <> Me.startPos
			MyBase.transform.position = Vector3.MoveTowards(MyBase.transform.position, Me.startPos, Me.properties.movementSpeed * 300F * CupheadTime.FixedDelta)
			Yield wait
		End While
		Me.StartCircle()
		Yield Nothing
		Return
	End Function

	' Token: 0x0600165F RID: 5727 RVA: 0x000C82BB File Offset: 0x000C66BB
	Private Sub StartCircle()
		Me.state = BaronessLevelWaffle.State.Move
		MyBase.StartCoroutine(Me.circle_cr())
		MyBase.StartCoroutine(Me.check_attack_cr())
	End Sub

	' Token: 0x06001660 RID: 5728 RVA: 0x000C82E0 File Offset: 0x000C66E0
	Private Iterator Function circle_cr() As IEnumerator
		Dim wait As YieldInstruction = New WaitForFixedUpdate()
		While Not Me.isDead
			If Me.state = BaronessLevelWaffle.State.Move Then
				Me.MovePivot()
				Me.PathMovement()
				Me.CheckIfTurn()
			End If
			Yield wait
		End While
		Return
	End Function

	' Token: 0x06001661 RID: 5729 RVA: 0x000C82FC File Offset: 0x000C66FC
	Private Sub MovePivot()
		Dim position As Vector3 = Me.pivotPoint.transform.position
		Dim pivotPointMoveAmount As Single = Me.properties.pivotPointMoveAmount
		If Me.pivotMovingLeft Then
			position.x = Mathf.MoveTowards(Me.pivotPoint.transform.position.x, Me.originalPivotPos.x - pivotPointMoveAmount, Me.properties.XAxisSpeed * CupheadTime.FixedDelta * Me.hitPauseCoefficient())
			Me.pivotMovingLeft = Me.pivotPoint.transform.position.x <> Me.originalPivotPos.x - pivotPointMoveAmount
		Else
			position.x = Mathf.MoveTowards(Me.pivotPoint.transform.position.x, Me.originalPivotPos.x + pivotPointMoveAmount, Me.properties.XAxisSpeed * CupheadTime.FixedDelta * Me.hitPauseCoefficient())
			Me.pivotMovingLeft = Me.pivotPoint.transform.position.x = Me.originalPivotPos.x + pivotPointMoveAmount
		End If
		Me.pivotPoint.transform.position = position
	End Sub

	' Token: 0x06001662 RID: 5730 RVA: 0x000C844C File Offset: 0x000C684C
	Private Sub PathMovement()
		Me.angle += Me.speed * CupheadTime.FixedDelta * Me.hitPauseCoefficient()
		Dim vector As Vector3 = New Vector3(-Mathf.Sin(Me.angle) * Me.loopSize, 0F, 0F)
		Dim vector2 As Vector3 = New Vector3(0F, Mathf.Cos(Me.angle) * Me.loopSize, 0F)
		MyBase.transform.position = Me.pivotPoint.position
		MyBase.transform.position += vector + vector2
	End Sub

	' Token: 0x06001663 RID: 5731 RVA: 0x000C84F4 File Offset: 0x000C68F4
	Private Sub CheckIfTurn()
		If Me.check Then
			If MyBase.transform.position.y < Me.pivotPoint.position.y Then
				If Not Me.onBottom Then
					MyBase.StartCoroutine(Me.turn_cr())
					Me.check = False
				End If
				Me.onBottom = True
			Else
				If Me.onBottom Then
					MyBase.StartCoroutine(Me.turn_cr())
					Me.check = False
				End If
				Me.onBottom = False
			End If
		End If
	End Sub

	' Token: 0x06001664 RID: 5732 RVA: 0x000C8588 File Offset: 0x000C6988
	Private Iterator Function turn_cr() As IEnumerator
		MyBase.animator.SetBool("Turn", True)
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Waffle_Turn", False, True)
		MyBase.animator.SetBool("Turn", False)
		Me.check = True
		Yield Nothing
		Return
	End Function

	' Token: 0x06001665 RID: 5733 RVA: 0x000C85A4 File Offset: 0x000C69A4
	Private Sub Turn()
		MyBase.transform.SetScale(New Single?(-MyBase.transform.localScale.x), New Single?(1F), New Single?(1F))
	End Sub

	' Token: 0x06001666 RID: 5734 RVA: 0x000C85EC File Offset: 0x000C69EC
	Private Iterator Function check_attack_cr() As IEnumerator
		If Not Me.isDead Then
			Yield CupheadTime.WaitForSeconds(Me, Me.properties.attackDelayRange.RandomFloat())
			MyBase.StartCoroutine(Me.attack_cr())
			Me.state = BaronessLevelWaffle.State.Attack
			While Me.state = BaronessLevelWaffle.State.Attack
				Yield Nothing
			End While
		End If
		Return
	End Function

	' Token: 0x06001667 RID: 5735 RVA: 0x000C8608 File Offset: 0x000C6A08
	Private Iterator Function attack_cr() As IEnumerator
		If Not Me.isDead Then
			MyBase.animator.Play("Waffle_Tuck_Start")
			MyBase.GetComponent(Of Collider2D)().enabled = False
			Dim randomValue As Single = CSng(Global.UnityEngine.Random.Range(0, 2))
			Me.diagFirst = randomValue = 0F
			Yield CupheadTime.WaitForSeconds(Me, Me.properties.anticipation)
			MyBase.animator.SetTrigger("Continue")
			MyBase.StartCoroutine(Me.waffle_pieces(If((Not Me.diagFirst), Me.straightPieces, Me.diagonalPieces), True))
			Yield CupheadTime.WaitForSeconds(Me, Me.properties.explodeTwoDuration)
			MyBase.StartCoroutine(Me.waffle_pieces(If((Not Me.diagFirst), Me.diagonalPieces, Me.straightPieces), False))
		End If
		Return
	End Function

	' Token: 0x06001668 RID: 5736 RVA: 0x000C8624 File Offset: 0x000C6A24
	Private Sub hitPause(i As Integer)
		If Me.diagonalPieces(i).wafflepiece.GetComponent(Of DamageReceiver)().IsHitPaused OrElse Me.straightPieces(i).wafflepiece.GetComponent(Of DamageReceiver)().IsHitPaused Then
			BaronessLevelWaffle.pauseValue = 0F
		Else
			BaronessLevelWaffle.pauseValue = 1F
		End If
	End Sub

	' Token: 0x06001669 RID: 5737 RVA: 0x000C8684 File Offset: 0x000C6A84
	Private Iterator Function waffle_pieces(pieces As BaronessLevelWaffle.WafflePieces(), isFirst As Boolean) As IEnumerator
		Dim t As Single = 0F
		Dim explodeTime As Single = Me.properties.explodeSpeed
		Dim returnTime As Single = Me.properties.explodeReturnSpeed
		Dim wait As YieldInstruction = New WaitForFixedUpdate()
		While Not Me.switchedOn
			Yield Nothing
		End While
		For Each wafflePieces As BaronessLevelWaffle.WafflePieces In pieces
			wafflePieces.wafflepiece.GetComponent(Of Collider2D)().enabled = True
			wafflePieces.waffleFX.Play("Trail", 0, Global.UnityEngine.Random.Range(0F, 0.6F))
		Next
		If isFirst Then
			Me.explosion.Create(New Vector2(Me.mouth.position.x, Me.mouth.position.y - 20F))
		End If
		While t < explodeTime
			For j As Integer = 0 To pieces.Length - 1
				t += CupheadTime.FixedDelta
				Dim num As Single = EaseUtils.Ease(EaseUtils.EaseType.easeOutSine, 0F, 1F, t / explodeTime)
				pieces(j).wafflepiece.transform.localPosition = Vector3.Lerp(pieces(j).wafflepiece.transform.localPosition.normalized, pieces(j).direction * Me.properties.explodeDistance, num * BaronessLevelWaffle.pauseValue)
				Me.hitPause(j)
			Next
			Yield wait
		End While
		t = 0F
		For Each wafflePieces2 As BaronessLevelWaffle.WafflePieces In pieces
			wafflePieces2.wafflepiece.GetComponent(Of Collider2D)().enabled = True
			wafflePieces2.waffleFX.SetTrigger("Death")
		Next
		If isFirst Then
			Me.explosionReverse.Create(New Vector2(Me.mouth.position.x, Me.mouth.position.y - 20F))
		End If
		While t < returnTime / 2F
			For l As Integer = 0 To pieces.Length - 1
				t += CupheadTime.FixedDelta
				Dim num2 As Single = EaseUtils.Ease(EaseUtils.EaseType.easeInSine, 0F, 1F, t / returnTime)
				pieces(l).wafflepiece.transform.localPosition = Vector3.Lerp(pieces(l).wafflepiece.transform.localPosition, Vector3.zero, num2 * BaronessLevelWaffle.pauseValue)
				Me.hitPause(l)
			Next
			Yield wait
		End While
		For m As Integer = 0 To pieces.Length - 1
			pieces(m).wafflepiece.GetComponent(Of Collider2D)().enabled = False
			pieces(m).wafflepiece.localPosition = Vector3.zero
		Next
		Yield Nothing
		If Not isFirst Then
			MyBase.animator.SetBool("Split", False)
			MyBase.GetComponent(Of Collider2D)().enabled = True
			Yield MyBase.animator.WaitForAnimationToEnd(Me, "Waffle_Return", False, True)
			Me.state = BaronessLevelWaffle.State.Move
			Me.switchedOn = False
			Yield MyBase.StartCoroutine(Me.check_attack_cr())
		End If
		Return
	End Function

	' Token: 0x0600166A RID: 5738 RVA: 0x000C86AD File Offset: 0x000C6AAD
	Private Sub switchAnimation()
		Me.switchedOn = True
		MyBase.animator.SetBool("Split", True)
	End Sub

	' Token: 0x0600166B RID: 5739 RVA: 0x000C86C8 File Offset: 0x000C6AC8
	Private Iterator Function destroyMouth_cr() As IEnumerator
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Waffle_Explode_Death", False, True)
		Me.mouth.GetComponent(Of Collider2D)().enabled = False
		Return
	End Function

	' Token: 0x0600166C RID: 5740 RVA: 0x000C86E4 File Offset: 0x000C6AE4
	Private Iterator Function death_cr() As IEnumerator
		Me.pivotPoint.transform.position = Me.originalPivotPos
		Dim wait As YieldInstruction = New WaitForFixedUpdate()
		Me.StartExplosions()
		Dim collider As Collider2D = MyBase.GetComponent(Of Collider2D)()
		Me.isDead = True
		Me.state = BaronessLevelWaffle.State.Dying
		Me.isDying = True
		MyBase.animator.SetTrigger("Death")
		MyBase.GetComponent(Of Collider2D)().enabled = False
		Yield CupheadTime.WaitForSeconds(Me, 1F)
		MyBase.animator.SetBool("DeathExplode", True)
		Dim explodeDeath As Boolean = True
		Dim untilDestroy As Single = 1500F
		MyBase.StartCoroutine(Me.destroyMouth_cr())
		While explodeDeath
			collider.enabled = False
			For i As Integer = 0 To Me.diagonalPieces.Length - 1
				Me.diagonalPieces(i).distanceFromCenter = Vector3.Distance(Me.diagonalPieces(i).wafflepiece.transform.localPosition, Me.mouth.transform.localPosition)
				Me.diagonalPieces(i).wafflepiece.GetComponent(Of Collider2D)().enabled = False
				Me.diagonalPieces(i).wafflepiece.transform.localPosition += Me.diagonalPieces(i).direction * 700F * CupheadTime.FixedDelta
				If Me.diagonalPieces(i).distanceFromCenter >= untilDestroy Then
					explodeDeath = False
					Exit For
				End If
			Next
			For j As Integer = 0 To Me.straightPieces.Length - 1
				Me.straightPieces(j).distanceFromCenter = Vector3.Distance(Me.straightPieces(j).wafflepiece.transform.localPosition, Me.mouth.transform.localPosition)
				Me.straightPieces(j).wafflepiece.GetComponent(Of Collider2D)().enabled = False
				Me.straightPieces(j).wafflepiece.transform.localPosition += Me.straightPieces(j).direction * 700F * CupheadTime.FixedDelta
				If Me.straightPieces(j).distanceFromCenter >= untilDestroy Then
					explodeDeath = False
					Exit For
				End If
			Next
			Yield wait
		End While
		Me.Die()
		Yield Nothing
		Return
	End Function

	' Token: 0x0600166D RID: 5741 RVA: 0x000C86FF File Offset: 0x000C6AFF
	Private Sub SoundWaffleExplode()
		AudioManager.Play("level_baroness_waffle_explode")
		Me.emitAudioFromObject.Add("level_baroness_waffle_explode")
	End Sub

	' Token: 0x0600166E RID: 5742 RVA: 0x000C871B File Offset: 0x000C6B1B
	Private Sub SoundWaffleWingflap()
		AudioManager.Play("level_baroness_waffle_wingflap")
		Me.emitAudioFromObject.Add("level_baroness_waffle_wingflap")
	End Sub

	' Token: 0x0600166F RID: 5743 RVA: 0x000C8737 File Offset: 0x000C6B37
	Private Sub SoundWaffleReform()
		AudioManager.Play("level_baroness_waffle_reform")
		Me.emitAudioFromObject.Add("level_baroness_waffle_reform")
	End Sub

	' Token: 0x04001F99 RID: 8089
	Private Shared pauseValue As Single

	' Token: 0x04001F9B RID: 8091
	<SerializeField()>
	Private explosion As Effect

	' Token: 0x04001F9C RID: 8092
	<SerializeField()>
	Private explosionReverse As Effect

	' Token: 0x04001F9D RID: 8093
	<SerializeField()>
	Private diagonalPieces As BaronessLevelWaffle.WafflePieces()

	' Token: 0x04001F9E RID: 8094
	<SerializeField()>
	Private straightPieces As BaronessLevelWaffle.WafflePieces()

	' Token: 0x04001F9F RID: 8095
	<SerializeField()>
	Private mouth As Transform

	' Token: 0x04001FA0 RID: 8096
	Private properties As LevelProperties.Baroness.Waffle

	' Token: 0x04001FA1 RID: 8097
	Private pivotPoint As Transform

	' Token: 0x04001FA2 RID: 8098
	Private damageDealer As DamageDealer

	' Token: 0x04001FA3 RID: 8099
	Private damageReceiver As DamageReceiver

	' Token: 0x04001FA4 RID: 8100
	Private health As Single

	' Token: 0x04001FA5 RID: 8101
	Private speed As Single

	' Token: 0x04001FA6 RID: 8102
	Private angle As Single

	' Token: 0x04001FA7 RID: 8103
	Private loopSize As Single = 200F

	' Token: 0x04001FA8 RID: 8104
	Private switchedOn As Boolean

	' Token: 0x04001FA9 RID: 8105
	Private pathA As Boolean

	' Token: 0x04001FAA RID: 8106
	Private check As Boolean

	' Token: 0x04001FAB RID: 8107
	Private onBottom As Boolean

	' Token: 0x04001FAC RID: 8108
	Private diagFirst As Boolean

	' Token: 0x04001FAD RID: 8109
	Private isDead As Boolean

	' Token: 0x04001FAE RID: 8110
	Private pivotMovingLeft As Boolean

	' Token: 0x04001FAF RID: 8111
	Private isHitPaused As Boolean

	' Token: 0x04001FB0 RID: 8112
	Private startPos As Vector3

	' Token: 0x04001FB1 RID: 8113
	Private originalPivotPos As Vector3

	' Token: 0x020004F8 RID: 1272
	Public Enum State
		' Token: 0x04001FB3 RID: 8115
		Enter
		' Token: 0x04001FB4 RID: 8116
		Move
		' Token: 0x04001FB5 RID: 8117
		Attack
		' Token: 0x04001FB6 RID: 8118
		Dying
	End Enum

	' Token: 0x020004F9 RID: 1273
	<Serializable()>
	Public Class WafflePieces
		' Token: 0x04001FB7 RID: 8119
		Public wafflepiece As Transform

		' Token: 0x04001FB8 RID: 8120
		Public waffleFX As Animator

		' Token: 0x04001FB9 RID: 8121
		Public direction As Vector3

		' Token: 0x04001FBA RID: 8122
		Public distanceFromCenter As Single

		' Token: 0x04001FBB RID: 8123
		Public damageDealer As DamageDealer
	End Class
End Class
