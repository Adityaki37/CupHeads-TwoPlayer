Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x0200055F RID: 1375
Public Class ClownLevelClownSwing
	Inherits LevelProperties.Clown.Entity

	' Token: 0x17000348 RID: 840
	' (get) Token: 0x060019D4 RID: 6612 RVA: 0x000EBCE0 File Offset: 0x000EA0E0
	' (set) Token: 0x060019D5 RID: 6613 RVA: 0x000EBCE8 File Offset: 0x000EA0E8
	Public Property state As ClownLevelClownSwing.State

	' Token: 0x060019D6 RID: 6614 RVA: 0x000EBCF1 File Offset: 0x000EA0F1
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.damageReceiver = MyBase.GetComponent(Of DamageReceiver)()
		AddHandler Me.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
		Me.state = ClownLevelClownSwing.State.Intro
	End Sub

	' Token: 0x060019D7 RID: 6615 RVA: 0x000EBD24 File Offset: 0x000EA124
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		MyBase.properties.DealDamage(info.damage)
		If MyBase.properties.CurrentHealth <= 0F AndAlso Me.state <> ClownLevelClownSwing.State.Death Then
			Me.state = ClownLevelClownSwing.State.Death
			Me.StartDeath()
		End If
	End Sub

	' Token: 0x060019D8 RID: 6616 RVA: 0x000EBD70 File Offset: 0x000EA170
	Public Overrides Sub LevelInit(properties As LevelProperties.Clown)
		MyBase.LevelInit(properties)
		Me.eyeMainIndex = Global.UnityEngine.Random.Range(0, properties.CurrentState.swing.positionString.Length)
	End Sub

	' Token: 0x060019D9 RID: 6617 RVA: 0x000EBD97 File Offset: 0x000EA197
	Public Sub StartSwing()
		AudioManager.Play("clown_swing_face_intro")
		Me.emitAudioFromObject.Add("clown_swing_face_intro")
		MyBase.StartCoroutine(Me.swing_intro_cr())
	End Sub

	' Token: 0x060019DA RID: 6618 RVA: 0x000EBDC0 File Offset: 0x000EA1C0
	Private Iterator Function swing_intro_cr() As IEnumerator
		Dim t As Single = 0F
		Dim time As Single = 5F
		Dim start As Vector2 = MyBase.transform.position
		While t < time
			Dim val As Single = EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, 0F, 1F, t / time)
			MyBase.transform.position = Vector2.Lerp(start, Me.swingStopPosition.position, val)
			t += CupheadTime.Delta
			Yield Nothing
		End While
		MyBase.transform.position = Me.swingStopPosition.position
		t = 0F
		time = 0.5F
		start = Me.umbrella.transform.position
		Dim [end] As Vector2 = New Vector3(Me.umbrella.transform.position.x, Me.umbrella.transform.position.y - 30F)
		While t < time
			Dim val2 As Single = EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, 0F, 1F, t / time)
			Me.umbrella.transform.position = Vector2.Lerp(start, [end], val2)
			t += CupheadTime.Delta
			Yield Nothing
		End While
		Me.umbrella.transform.position = start
		Me.umbrella.GetComponent(Of SpriteRenderer)().sortingLayerName = "Player"
		Me.umbrella.GetComponent(Of SpriteRenderer)().sortingOrder = 200
		Me.state = ClownLevelClownSwing.State.Idle
		MyBase.StartCoroutine(Me.swing_cr())
		AddHandler Me.coasterHandler.OnCoasterLeave, AddressOf Me.StartEnemies
		Yield Nothing
		Return
	End Function

	' Token: 0x060019DB RID: 6619 RVA: 0x000EBDDC File Offset: 0x000EA1DC
	Private Iterator Function swing_cr() As IEnumerator
		Dim p As LevelProperties.Clown.Swing = MyBase.properties.CurrentState.swing
		Dim spacingFront As Single = Me.swingFrontPrefab.GetComponent(Of Renderer)().bounds.size.x + p.swingSpacing
		Dim spacingBack As Single = Me.swingBackPrefab.GetComponent(Of Renderer)().bounds.size.x + p.swingSpacing
		Dim numOfSwings As Integer = 6
		AudioManager.Play("clown_swing_open")
		Me.emitAudioFromObject.Add("clown_swing_open")
		MyBase.animator.SetTrigger("Continue")
		For i As Integer = 0 To numOfSwings - 1
			Dim vector As Vector3 = New Vector3(-640F - spacingFront + spacingFront * CSng(i), 360F, 0F)
			Dim clownLevelSwings As ClownLevelSwings = Global.UnityEngine.[Object].Instantiate(Of ClownLevelSwings)(Me.swingFrontPrefab)
			clownLevelSwings.Init(vector, MyBase.properties.CurrentState.swing, spacingFront, CSng(i))
		Next
		For j As Integer = 0 To numOfSwings - 1
			Dim vector2 As Vector3 = New Vector3(640F + spacingBack - spacingBack * CSng(j), 360F, 0F)
			Dim clownLevelSwings2 As ClownLevelSwings = Global.UnityEngine.[Object].Instantiate(Of ClownLevelSwings)(Me.swingBackPrefab)
			clownLevelSwings2.Init(vector2, MyBase.properties.CurrentState.swing, spacingBack, CSng(j))
		Next
		Yield Nothing
		Return
	End Function

	' Token: 0x060019DC RID: 6620 RVA: 0x000EBDF7 File Offset: 0x000EA1F7
	Private Sub StartBottom()
		MyBase.animator.Play("Swing_Bottom_Idle")
	End Sub

	' Token: 0x060019DD RID: 6621 RVA: 0x000EBE09 File Offset: 0x000EA209
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me.enemy = Nothing
		Me.swingFrontPrefab = Nothing
		Me.swingBackPrefab = Nothing
	End Sub

	' Token: 0x060019DE RID: 6622 RVA: 0x000EBE28 File Offset: 0x000EA228
	Private Sub StartDeath()
		If Me.OnDeath IsNot Nothing Then
			Me.OnDeath()
		End If
		Me.StopAllCoroutines()
		AudioManager.Play("clown_swing_death")
		Me.emitAudioFromObject.Add("clown_swing_death")
		MyBase.animator.Play("Swing_Death")
		MyBase.animator.Play("Swing_Bottom_Death")
		MyBase.animator.Play("Face_Death")
		ClownLevelSwings.moveSpeed = MyBase.properties.CurrentState.swing.swingSpeed * 2F
		MyBase.GetComponent(Of Collider2D)().enabled = False
	End Sub

	' Token: 0x060019DF RID: 6623 RVA: 0x000EBEC7 File Offset: 0x000EA2C7
	Private Sub SetMoveDirection([set] As Integer)
		If [set] = 1 Then
			Me.moveUp = True
		Else
			Me.moveUp = False
		End If
	End Sub

	' Token: 0x060019E0 RID: 6624 RVA: 0x000EBEE4 File Offset: 0x000EA2E4
	Private Iterator Function move_topper_cr() As IEnumerator
		Dim speed As Single = 60F
		While True
			If Me.moveUp Then
				Me.topper.transform.position += Vector3.up * speed * CupheadTime.Delta
			Else
				Me.topper.transform.position -= Vector3.up * speed * CupheadTime.Delta
			End If
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x060019E1 RID: 6625 RVA: 0x000EBEFF File Offset: 0x000EA2FF
	Private Sub StartEnemies()
		MyBase.animator.SetBool("IsAttacking", True)
		MyBase.StartCoroutine(Me.enemies_cr())
	End Sub

	' Token: 0x060019E2 RID: 6626 RVA: 0x000EBF20 File Offset: 0x000EA320
	Private Iterator Function enemies_cr() As IEnumerator
		Dim p As LevelProperties.Clown.Swing = MyBase.properties.CurrentState.swing
		Dim enemyPosString As String() = p.positionString(Me.eyeMainIndex).Split(New Char() { ","c })
		Me.state = ClownLevelClownSwing.State.Enemies
		AudioManager.Play("clown_swing_face_attack_intro")
		Me.emitAudioFromObject.Add("clown_swing_face_attack_intro")
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Face_Attack_Intro", 1, False, True)
		For i As Integer = 0 To enemyPosString.Length - 1
			Dim enemyPos As String() = enemyPosString(i).Split(New Char() { "-"c })
			For Each pos As String In enemyPos
				Dim targetX As Single = 0F
				Parser.FloatTryParse(pos, targetX)
				Me.enemy.Create(MyBase.transform.position, targetX, p.HP, p, Me)
				Yield CupheadTime.WaitForSeconds(Me, p.spawnDelay)
			Next
		Next
		Me.eyeMainIndex = (Me.eyeMainIndex + 1) Mod p.positionString.Length
		MyBase.animator.SetBool("IsAttacking", False)
		AudioManager.Play("clown_swing_face_attack_outro")
		Me.emitAudioFromObject.Add("clown_swing_face_attack_outro")
		Yield Nothing
		Return
	End Function

	' Token: 0x040022EE RID: 8942
	Public Const NumOfSwings As Integer = 6

	' Token: 0x040022EF RID: 8943
	<SerializeField()>
	Private coasterHandler As ClownLevelCoasterHandler

	' Token: 0x040022F0 RID: 8944
	<SerializeField()>
	Private umbrella As GameObject

	' Token: 0x040022F1 RID: 8945
	<SerializeField()>
	Private topper As GameObject

	' Token: 0x040022F2 RID: 8946
	<SerializeField()>
	Private enemy As ClownLevelEnemy

	' Token: 0x040022F3 RID: 8947
	<SerializeField()>
	Private swingFrontPrefab As ClownLevelSwings

	' Token: 0x040022F4 RID: 8948
	<SerializeField()>
	Private swingBackPrefab As ClownLevelSwings

	' Token: 0x040022F5 RID: 8949
	<SerializeField()>
	Private clownBullet As BasicProjectile

	' Token: 0x040022F6 RID: 8950
	<SerializeField()>
	Private swingStopPosition As Transform

	' Token: 0x040022F8 RID: 8952
	Private damageReceiver As DamageReceiver

	' Token: 0x040022F9 RID: 8953
	Private moveUp As Boolean

	' Token: 0x040022FA RID: 8954
	Private eyeMainIndex As Integer

	' Token: 0x040022FB RID: 8955
	Public OnDeath As Action

	' Token: 0x02000560 RID: 1376
	Public Enum State
		' Token: 0x040022FD RID: 8957
		Intro
		' Token: 0x040022FE RID: 8958
		Idle
		' Token: 0x040022FF RID: 8959
		Enemies
		' Token: 0x04002300 RID: 8960
		Death
	End Enum
End Class
