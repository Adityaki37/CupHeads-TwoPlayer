Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020004E7 RID: 1255
Public Class BaronessLevelCandyCorn
	Inherits BaronessLevelMiniBossBase

	' Token: 0x1700031E RID: 798
	' (get) Token: 0x060015C0 RID: 5568 RVA: 0x000C37B4 File Offset: 0x000C1BB4
	' (set) Token: 0x060015C1 RID: 5569 RVA: 0x000C37BC File Offset: 0x000C1BBC
	Public Property state As BaronessLevelCandyCorn.State

	' Token: 0x060015C2 RID: 5570 RVA: 0x000C37C8 File Offset: 0x000C1BC8
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.firstTime = True
		Me.isDying = False
		Me.damageDealer = DamageDealer.NewEnemy()
		Me.damageReceiver = MyBase.GetComponent(Of DamageReceiver)()
		AddHandler Me.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
		Me.state = BaronessLevelCandyCorn.State.Move
	End Sub

	' Token: 0x060015C3 RID: 5571 RVA: 0x000C381F File Offset: 0x000C1C1F
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x060015C4 RID: 5572 RVA: 0x000C3840 File Offset: 0x000C1C40
	Public Sub Init(properties As LevelProperties.Baroness.CandyCorn, pos As Vector2, speed As Single, health As Single)
		Me.properties = properties
		Me.speed = speed
		Me.health = health
		MyBase.transform.position = pos
		Me.bottomPoint = pos.y
		Me.isTop = False
		Me.movingLeft = True
		If Me.properties.spawnMinis Then
			MyBase.StartCoroutine(Me.spawnMinis_cr())
		End If
		MyBase.StartCoroutine(Me.switchLayer_cr())
	End Sub

	' Token: 0x060015C5 RID: 5573 RVA: 0x000C38B9 File Offset: 0x000C1CB9
	Private Sub Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x060015C6 RID: 5574 RVA: 0x000C38D1 File Offset: 0x000C1CD1
	Private Sub FixedUpdate()
		If Me.state = BaronessLevelCandyCorn.State.Move Then
			If Me.moveY Then
				Me.MoveAlongY()
			Else
				Me.MoveAlongX()
			End If
		End If
	End Sub

	' Token: 0x060015C7 RID: 5575 RVA: 0x000C38FC File Offset: 0x000C1CFC
	Protected Overrides Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		If Me.health > 0F Then
			MyBase.OnDamageTaken(info)
		End If
		Me.health -= info.damage
		If Me.health <= 0F AndAlso Me.state <> BaronessLevelCandyCorn.State.Dying Then
			Dim damageInfo As DamageDealer.DamageInfo = New DamageDealer.DamageInfo(Me.health, info.direction, info.origin, info.damageSource)
			MyBase.OnDamageTaken(damageInfo)
			Me.state = BaronessLevelCandyCorn.State.Dying
			Me.StartDeath()
		End If
	End Sub

	' Token: 0x060015C8 RID: 5576 RVA: 0x000C3984 File Offset: 0x000C1D84
	Private Iterator Function switchLayer_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 3F)
		MyBase.gameObject.GetComponent(Of SpriteRenderer)().sortingLayerName = SpriteLayer.Enemies.ToString()
		MyBase.gameObject.GetComponent(Of SpriteRenderer)().sortingOrder = 2
		Return
	End Function

	' Token: 0x060015C9 RID: 5577 RVA: 0x000C399F File Offset: 0x000C1D9F
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me.miniCandyPrefab = Nothing
	End Sub

	' Token: 0x060015CA RID: 5578 RVA: 0x000C39B0 File Offset: 0x000C1DB0
	Private Iterator Function spawnMinis_cr() As IEnumerator
		Me.targetPos = MyBase.transform
		Dim targetPos2 As Transform = Me.targetPos
		Dim thisRenderer As SpriteRenderer = MyBase.gameObject.GetComponent(Of SpriteRenderer)()
		While True
			If MyBase.animator.GetCurrentAnimatorStateInfo(0).IsName("Turn_A") OrElse MyBase.animator.GetCurrentAnimatorStateInfo(0).IsName("Turn_B") Then
				Dim miniCandyCorn As BaronessLevelCandyCornMini = Global.UnityEngine.[Object].Instantiate(Of BaronessLevelCandyCornMini)(Me.miniCandyPrefab)
				miniCandyCorn.Init(MyBase.transform.position, Me.properties.miniCornMovementSpeed, CSng(Me.properties.miniCornHP))
				targetPos2 = miniCandyCorn.transform
				Dim r As SpriteRenderer = miniCandyCorn.GetComponent(Of SpriteRenderer)()
				r.sortingLayerName = thisRenderer.sortingLayerName
				r.sortingOrder = thisRenderer.sortingOrder - 1
				Yield CupheadTime.WaitForSeconds(Me, Me.properties.miniCornSpawnDelay)
			Else
				Yield Nothing
			End If
		End While
		Return
	End Function

	' Token: 0x060015CB RID: 5579 RVA: 0x000C39CC File Offset: 0x000C1DCC
	Private Sub MoveAlongX()
		Dim num As Single = 50F
		Dim num2 As Single = 10F
		Dim vector As Vector3 = New Vector3(Me.properties.centerPosition, 0F, 0F)
		Dim vector2 As Vector3 = vector - MyBase.transform.position
		If Me.movingLeft Then
			If MyBase.transform.position.x > -640F + num Then
				MyBase.transform.position -= MyBase.transform.right * (Me.speed * CupheadTime.FixedDelta * Me.hitPauseCoefficient())
				If vector2.x < num2 AndAlso vector2.x > -num2 AndAlso Not Me.justSwitchedMiddle Then
					Me.checkIfSwitch()
				End If
			Else
				Me.moveY = True
				Me.justSwitchedMiddle = False
				Me.movingLeft = False
			End If
		ElseIf Not Me.movingLeft Then
			If MyBase.transform.position.x < CSng(Level.Current.Right) - num Then
				MyBase.transform.position += MyBase.transform.right * (Me.speed * CupheadTime.FixedDelta * Me.hitPauseCoefficient())
				If vector2.x < num2 AndAlso vector2.x > -num2 AndAlso Not Me.justSwitchedMiddle Then
					Me.checkIfSwitch()
				End If
			Else
				Me.moveY = True
				Me.justSwitchedMiddle = False
				Me.movingLeft = True
			End If
		End If
	End Sub

	' Token: 0x060015CC RID: 5580 RVA: 0x000C3B74 File Offset: 0x000C1F74
	Private Sub MoveAlongY()
		Dim num As Single = 125F
		If Not Me.isTop Then
			If MyBase.transform.position.y < 360F - num Then
				MyBase.transform.position += MyBase.transform.up * (Me.speed * CupheadTime.FixedDelta * Me.hitPauseCoefficient())
			Else
				Me.isTop = True
				Me.moveY = False
			End If
		ElseIf MyBase.transform.position.y > Me.bottomPoint Then
			MyBase.transform.position -= MyBase.transform.up * (Me.speed * CupheadTime.FixedDelta * Me.hitPauseCoefficient())
		Else
			Me.isTop = False
			Me.moveY = False
		End If
	End Sub

	' Token: 0x060015CD RID: 5581 RVA: 0x000C3C6C File Offset: 0x000C206C
	Private Sub checkIfSwitch()
		MyBase.StartCoroutine(Me.switch_cr())
	End Sub

	' Token: 0x060015CE RID: 5582 RVA: 0x000C3C7C File Offset: 0x000C207C
	Private Iterator Function switch_cr() As IEnumerator
		Dim pattern As String() = Me.properties.changeLevelString.GetRandom().Split(New Char() { ","c })
		If Me.firstTime Then
			Me.firstIndex = Global.UnityEngine.Random.Range(0, pattern.Length)
			Me.firstTime = False
		End If
		If pattern(Me.firstIndex)(0) = "Y"c Then
			Me.moveY = True
			Me.justSwitchedMiddle = True
		ElseIf pattern(Me.firstIndex)(0) = "N"c Then
			Me.moveY = False
			Me.justSwitchedMiddle = True
		End If
		If Me.firstIndex < pattern.Length - 1 Then
			Me.firstIndex += 1
		Else
			Me.firstIndex = 0
		End If
		Yield Nothing
		Return
	End Function

	' Token: 0x060015CF RID: 5583 RVA: 0x000C3C97 File Offset: 0x000C2097
	Private Sub StartDeath()
		Me.state = BaronessLevelCandyCorn.State.Dying
		Me.StopAllCoroutines()
		MyBase.StartCoroutine(Me.death_cr())
	End Sub

	' Token: 0x060015D0 RID: 5584 RVA: 0x000C3CB4 File Offset: 0x000C20B4
	Private Iterator Function death_cr() As IEnumerator
		Dim wait As YieldInstruction = New WaitForFixedUpdate()
		Dim speed As Single = Me.properties.deathMoveSpeed
		Me.StartExplosions()
		Me.isDying = True
		MyBase.animator.SetTrigger("Death")
		MyBase.GetComponent(Of Collider2D)().enabled = False
		Yield CupheadTime.WaitForSeconds(Me, 0.5F)
		While MyBase.transform.position.y < 560F
			MyBase.transform.position += Vector3.up * speed * CupheadTime.FixedDelta
			speed += Me.properties.deathAcceleration
			Yield wait
		End While
		Me.Die()
		Yield Nothing
		Return
	End Function

	' Token: 0x060015D1 RID: 5585 RVA: 0x000C3CCF File Offset: 0x000C20CF
	Private Sub SoundCandyCornBite()
		AudioManager.Play("level_baroness_candycorn_bite")
		Me.emitAudioFromObject.Add("level_baroness_candycorn_bite")
	End Sub

	' Token: 0x04001F16 RID: 7958
	<SerializeField()>
	Private miniCandyPrefab As BaronessLevelCandyCornMini

	' Token: 0x04001F17 RID: 7959
	Private properties As LevelProperties.Baroness.CandyCorn

	' Token: 0x04001F18 RID: 7960
	Private targetPos As Transform

	' Token: 0x04001F19 RID: 7961
	Private damageDealer As DamageDealer

	' Token: 0x04001F1A RID: 7962
	Private damageReceiver As DamageReceiver

	' Token: 0x04001F1B RID: 7963
	Private health As Single

	' Token: 0x04001F1C RID: 7964
	Private speed As Single

	' Token: 0x04001F1D RID: 7965
	Private bottomPoint As Single

	' Token: 0x04001F1E RID: 7966
	Private firstIndex As Integer

	' Token: 0x04001F1F RID: 7967
	Private isTop As Boolean

	' Token: 0x04001F20 RID: 7968
	Private moveY As Boolean

	' Token: 0x04001F21 RID: 7969
	Private firstTime As Boolean

	' Token: 0x04001F22 RID: 7970
	Private justSwitchedMiddle As Boolean

	' Token: 0x04001F23 RID: 7971
	Private movingLeft As Boolean

	' Token: 0x020004E8 RID: 1256
	Public Enum State
		' Token: 0x04001F25 RID: 7973
		Move
		' Token: 0x04001F26 RID: 7974
		Dying
	End Enum
End Class
