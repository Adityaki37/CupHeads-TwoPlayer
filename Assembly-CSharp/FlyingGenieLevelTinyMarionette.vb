Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x0200067E RID: 1662
Public Class FlyingGenieLevelTinyMarionette
	Inherits AbstractCollidableObject

	' Token: 0x06002311 RID: 8977 RVA: 0x00149338 File Offset: 0x00147738
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.damageDealer = DamageDealer.NewEnemy()
		Me.damageReceiver = MyBase.GetComponent(Of DamageReceiver)()
		AddHandler Me.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
		Me.damageReceiver.enabled = False
	End Sub

	' Token: 0x06002312 RID: 8978 RVA: 0x00149385 File Offset: 0x00147785
	Private Sub FixedUpdate()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x06002313 RID: 8979 RVA: 0x0014939D File Offset: 0x0014779D
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		Me.hp -= info.damage
		If Me.hp < 0F AndAlso Not Me.isDead Then
			Me.isDead = True
			Me.Die()
		End If
	End Sub

	' Token: 0x06002314 RID: 8980 RVA: 0x001493DA File Offset: 0x001477DA
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x06002315 RID: 8981 RVA: 0x001493F8 File Offset: 0x001477F8
	Public Sub Activate(endPos As Vector3, properties As LevelProperties.FlyingGenie.Scan, movingClockwise As Boolean)
		Me.properties = properties
		Me.hp = properties.miniHP
		Me.isClockwise = movingClockwise
		MyBase.StartCoroutine(Me.tiny_marionette(endPos))
	End Sub

	' Token: 0x06002316 RID: 8982 RVA: 0x00149424 File Offset: 0x00147824
	Private Iterator Function bounce_marionette_cr() As IEnumerator
		Dim t As Single = 0F
		Dim time As Single = 0.5F
		Dim start As Single = MyBase.transform.position.y
		Dim [end] As Single = MyBase.transform.position.y + 100F
		While t < time
			Dim val As Single = EaseUtils.Ease(EaseUtils.EaseType.easeOutBounce, 0F, 1F, t / time)
			MyBase.transform.SetPosition(Nothing, New Single?(Mathf.Lerp(start, [end], val)), Nothing)
			t += CupheadTime.Delta
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06002317 RID: 8983 RVA: 0x00149440 File Offset: 0x00147840
	Private Iterator Function tiny_marionette(endPos As Vector3) As IEnumerator
		MyBase.StartCoroutine(Me.bounce_marionette_cr())
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Puppet_Intro", False, True)
		Me.damageReceiver.enabled = True
		Dim t As Single = 0F
		Dim time As Single = Me.properties.movementSpeed
		Dim start As Vector3 = MyBase.transform.position
		Yield CupheadTime.WaitForSeconds(Me, 0.5F)
		While t < time
			Dim val As Single = EaseUtils.Ease(EaseUtils.EaseType.easeInSine, 0F, 1F, t / time)
			MyBase.transform.position = Vector3.Lerp(start, endPos, val)
			t += CupheadTime.Delta
			Yield New WaitForFixedUpdate()
		End While
		Yield CupheadTime.WaitForSeconds(Me, 1F)
		Me.startedDown = Rand.Bool()
		Me.turningDown = Not Me.startedDown
		MyBase.animator.SetBool("OnTurningDown", Me.turningDown)
		Dim dirString As String = If((Not Me.startedDown), "Up_", "Down_")
		MyBase.animator.SetTrigger("OnStartCycle")
		MyBase.animator.SetBool("IsDown", Me.startedDown)
		Me.bulletMainIndex = Global.UnityEngine.Random.Range(0, Me.properties.bulletString.Length)
		Dim bulletString As String() = Me.properties.bulletString(Me.bulletMainIndex).Split(New Char() { ","c })
		Me.bulletIndex = Global.UnityEngine.Random.Range(0, bulletString.Length)
		Yield MyBase.animator.WaitForAnimationToEnd(Me, dirString + "Warning_Shoot", False, True)
		While True
			bulletString = Me.properties.bulletString(Me.bulletMainIndex).Split(New Char() { ","c })
			Yield CupheadTime.WaitForSeconds(Me, Me.properties.shootDelay)
			MyBase.animator.SetTrigger("OnShoot")
			Yield MyBase.animator.WaitForAnimationToEnd(Me, False)
			If Me.bulletIndex < bulletString.Length - 1 Then
				Me.bulletIndex += 1
			Else
				Me.bulletMainIndex = (Me.bulletMainIndex + 1) Mod Me.properties.bulletString.Length
				Me.bulletIndex = 0
			End If
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06002318 RID: 8984 RVA: 0x00149464 File Offset: 0x00147864
	Private Sub ShootBullet(pos As Vector3, rotation As Single)
		Dim array As String() = Me.properties.bulletString(Me.bulletMainIndex).Split(New Char() { ","c })
		If array(Me.bulletIndex)(0) = "P"c Then
			Me.pinkProjectile.Create(pos, rotation + 90F, Me.properties.bulletSpeed)
		Else
			Me.projectile.Create(pos, rotation + 90F, Me.properties.bulletSpeed)
		End If
	End Sub

	' Token: 0x06002319 RID: 8985 RVA: 0x001494F8 File Offset: 0x001478F8
	Private Sub AniEventCheckFlip()
		If Me.hasStarted Then
			MyBase.transform.SetScale(New Single?(MyBase.transform.localScale.x * -1F), Nothing, Nothing)
			Me.turningDown = Not Me.turningDown
			MyBase.animator.SetBool("OnTurningDown", Me.turningDown)
		Else
			Me.hasStarted = True
			If Not Me.isClockwise AndAlso Not Me.startedDown Then
				MyBase.transform.SetScale(New Single?(MyBase.transform.localScale.x * -1F), Nothing, Nothing)
			ElseIf Me.isClockwise AndAlso Me.startedDown Then
				MyBase.transform.SetScale(New Single?(MyBase.transform.localScale.x * -1F), Nothing, Nothing)
			End If
		End If
	End Sub

	' Token: 0x0600231A RID: 8986 RVA: 0x00149624 File Offset: 0x00147A24
	Private Sub AniEventShoot()
		Dim effect As Effect = Me.shootFX.Create(Me.shootRoot.transform.position)
		AudioManager.Play("genie_puppetsmall_shoot")
		Me.emitAudioFromObject.Add("genie_puppetsmall_shoot")
		effect.transform.SetEulerAngles(Nothing, Nothing, New Single?(Me.shootRoot.transform.eulerAngles.z))
		Me.ShootBullet(Me.shootRoot.transform.position, Me.shootRoot.transform.eulerAngles.z)
	End Sub

	' Token: 0x0600231B RID: 8987 RVA: 0x001496CF File Offset: 0x00147ACF
	Public Sub Die()
		MyBase.animator.SetTrigger("OnDeath")
		AudioManager.Play("genie_puppetsmall_death")
		Me.emitAudioFromObject.Add("genie_puppetsmall_death")
		Me.StopAllCoroutines()
		MyBase.StartCoroutine(Me.dead_move_cr())
	End Sub

	' Token: 0x0600231C RID: 8988 RVA: 0x00149710 File Offset: 0x00147B10
	Private Iterator Function dead_move_cr() As IEnumerator
		Dim t As Single = 0F
		Dim timer As Single = 0.5F
		Dim downTimer As Single = 0.5F
		Dim start As Single = MyBase.transform.position.y
		Dim [end] As Single = 660F
		Dim downEnd As Single = MyBase.transform.position.y - 50F
		MyBase.GetComponent(Of LevelBossDeathExploder)().StartExplosion()
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Death_Start", False, True)
		While t < downTimer
			Dim val As Single = EaseUtils.Ease(EaseUtils.EaseType.easeInSine, 0F, 1F, t / downTimer)
			MyBase.transform.SetPosition(Nothing, New Single?(Mathf.Lerp(start, downEnd, val)), Nothing)
			t += CupheadTime.Delta
			Yield Nothing
		End While
		t = 0F
		start = MyBase.transform.position.y
		While t < timer
			Dim val2 As Single = EaseUtils.Ease(EaseUtils.EaseType.easeInSine, 0F, 1F, t / timer)
			MyBase.transform.SetPosition(Nothing, New Single?(Mathf.Lerp(start, [end], val2)), Nothing)
			t += CupheadTime.Delta
			Yield Nothing
		End While
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
		Yield Nothing
		Return
	End Function

	' Token: 0x0600231D RID: 8989 RVA: 0x0014972B File Offset: 0x00147B2B
	Private Sub SoundPuppetSmallEnterPuppet()
		AudioManager.Play("genie_puppetsmall_enter_puppetsmall")
		Me.emitAudioFromObject.Add("genie_puppetsmall_enter_puppetsmall")
	End Sub

	' Token: 0x0600231E RID: 8990 RVA: 0x00149747 File Offset: 0x00147B47
	Private Sub SoundPuppetSmallDance()
		AudioManager.Play("genie_puppetsmall_move")
		Me.emitAudioFromObject.Add("genie_puppetsmall_move")
	End Sub

	' Token: 0x0600231F RID: 8991 RVA: 0x00149763 File Offset: 0x00147B63
	Private Sub SoundPuppetShootWarning()
		AudioManager.Play("genie_puppetsmall_shootwarning")
		Me.emitAudioFromObject.Add("genie_puppetsmall_shootwarning")
	End Sub

	' Token: 0x06002320 RID: 8992 RVA: 0x0014977F File Offset: 0x00147B7F
	Private Sub SoundPuppetWarningShot()
		AudioManager.Play("genie_puppetsmall_warningshot")
		Me.emitAudioFromObject.Add("genie_puppetsmall_warningshot")
	End Sub

	' Token: 0x04002BB1 RID: 11185
	<SerializeField()>
	Private projectile As BasicProjectile

	' Token: 0x04002BB2 RID: 11186
	<SerializeField()>
	Private pinkProjectile As BasicProjectile

	' Token: 0x04002BB3 RID: 11187
	<SerializeField()>
	Private shootFX As Effect

	' Token: 0x04002BB4 RID: 11188
	<SerializeField()>
	Private shootRoot As Transform

	' Token: 0x04002BB5 RID: 11189
	Private damageDealer As DamageDealer

	' Token: 0x04002BB6 RID: 11190
	Private damageReceiver As DamageReceiver

	' Token: 0x04002BB7 RID: 11191
	Private hp As Single

	' Token: 0x04002BB8 RID: 11192
	Private turningDown As Boolean

	' Token: 0x04002BB9 RID: 11193
	Private isClockwise As Boolean

	' Token: 0x04002BBA RID: 11194
	Private startedDown As Boolean

	' Token: 0x04002BBB RID: 11195
	Private hasStarted As Boolean

	' Token: 0x04002BBC RID: 11196
	Private isDead As Boolean

	' Token: 0x04002BBD RID: 11197
	Private bulletMainIndex As Integer

	' Token: 0x04002BBE RID: 11198
	Private bulletIndex As Integer

	' Token: 0x04002BBF RID: 11199
	Private properties As LevelProperties.FlyingGenie.Scan
End Class
