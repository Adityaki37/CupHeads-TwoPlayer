Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x0200070B RID: 1803
Public Class OldManLevelPuppetBall
	Inherits AbstractProjectile

	' Token: 0x170003CE RID: 974
	' (get) Token: 0x060026E3 RID: 9955 RVA: 0x0016C659 File Offset: 0x0016AA59
	' (set) Token: 0x060026E4 RID: 9956 RVA: 0x0016C661 File Offset: 0x0016AA61
	Public Property readyToCatch As Boolean

	' Token: 0x170003CF RID: 975
	' (get) Token: 0x060026E5 RID: 9957 RVA: 0x0016C66A File Offset: 0x0016AA6A
	' (set) Token: 0x060026E6 RID: 9958 RVA: 0x0016C672 File Offset: 0x0016AA72
	Public Property isMoving As Boolean

	' Token: 0x060026E7 RID: 9959 RVA: 0x0016C67C File Offset: 0x0016AA7C
	Public Overridable Function Init(startPos As Vector3, platformPos As Vector3, endPos As Vector3, properties As LevelProperties.OldMan.Hands) As OldManLevelPuppetBall
		MyBase.ResetLifetime()
		MyBase.ResetDistance()
		MyBase.transform.position = startPos
		MyBase.transform.localScale = New Vector3(Mathf.Sign(endPos.x - startPos.x), 1F)
		Me.startPos = startPos
		Me.endPos = endPos
		Me.platformPos = platformPos + Vector3.up * -10F
		Me.properties = properties
		Me.Move()
		MyBase.animator.Play("Idle", 0, 0.7647059F)
		Return Me
	End Function

	' Token: 0x060026E8 RID: 9960 RVA: 0x0016C718 File Offset: 0x0016AB18
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me.WORKAROUND_NullifyFields()
	End Sub

	' Token: 0x060026E9 RID: 9961 RVA: 0x0016C726 File Offset: 0x0016AB26
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
		Me.SFX_OMM_P2_DamagePlayerCheer()
	End Sub

	' Token: 0x060026EA RID: 9962 RVA: 0x0016C74A File Offset: 0x0016AB4A
	Private Sub Move()
		Me.isMoving = True
		MyBase.StartCoroutine(Me.move_cr())
	End Sub

	' Token: 0x060026EB RID: 9963 RVA: 0x0016C760 File Offset: 0x0016AB60
	Private Iterator Function move_cr() As IEnumerator
		Me.readyToCatch = False
		Dim STRAIGHT_BOUNCE_CUTOFF As Single = 0.66F
		Dim wait As YieldInstruction = New WaitForFixedUpdate()
		Dim percentage As Single = Mathf.Abs(Me.startPos.x - Me.platformPos.x) / Mathf.Abs(Me.startPos.x - Me.endPos.x)
		Dim newX As Single = MyBase.transform.position.x
		Dim newY As Single = MyBase.transform.position.y
		Dim direction As Single = Mathf.Sign(Me.endPos.x - Me.startPos.x)
		Dim xTotalDist As Single = Mathf.Abs(Me.startPos.x - Me.platformPos.x)
		Dim yRad As Single = MyBase.transform.position.y - (Me.platformPos.y + Me.size)
		While MyBase.transform.position.y > Me.platformPos.y + Me.size + 20F
			newX += direction * CupheadTime.FixedDelta * Me.properties.ballSpeed * If((Not Me.puppetDead), 1F, 0F)
			Dim xDist As Single = Mathf.Abs(newX - Me.startPos.x)
			newY = If((percentage >= 1F - STRAIGHT_BOUNCE_CUTOFF), (Me.platformPos.y + Me.size + yRad * Mathf.Cos(xDist / xTotalDist * 1.5707964F)), Mathf.Lerp(Me.startPos.y, Me.platformPos.y + Me.size, xDist / xTotalDist))
			MyBase.transform.SetPosition(New Single?(newX), New Single?(newY), Nothing)
			Yield wait
		End While
		MyBase.transform.SetPosition(New Single?(Me.platformPos.x), New Single?(Me.platformPos.y + Me.size), Nothing)
		newX = MyBase.transform.position.x
		xTotalDist = Mathf.Abs(Me.platformPos.x - Me.endPos.x)
		yRad = Me.endPos.y - MyBase.transform.position.y
		MyBase.animator.SetTrigger("OnBounce")
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Bounce", False, True)
		While Mathf.Sign(Me.endPos.x - MyBase.transform.position.x) = direction OrElse Me.puppetDead
			newX += direction * CupheadTime.FixedDelta * Me.properties.ballSpeed * If((Not Me.puppetDead), 1F, 0F)
			Dim xDist As Single = Mathf.Abs(newX - Me.platformPos.x)
			newY = If((percentage <= STRAIGHT_BOUNCE_CUTOFF), (Me.platformPos.y + Me.size + yRad * Mathf.Sin(xDist / xTotalDist * 1.5707964F)), Mathf.Lerp(Me.platformPos.y + Me.size, Me.endPos.y, xDist / xTotalDist))
			MyBase.transform.SetPosition(New Single?(newX), New Single?(newY), Nothing)
			If Not Me.readyToCatch AndAlso xDist / xTotalDist >= 0.9F AndAlso Not Me.puppetDead Then
				Me.readyToCatch = True
			End If
			If Me.puppetDead AndAlso (MyBase.transform.position.x > 1140F OrElse MyBase.transform.position.x < -1140F) Then
				Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
			End If
			Yield wait
		End While
		Return
	End Function

	' Token: 0x060026EC RID: 9964 RVA: 0x0016C77C File Offset: 0x0016AB7C
	Private Sub LateUpdate()
		Me.shadowRend.transform.position = New Vector3(MyBase.transform.position.x, Me.platformPos.y + Me.size)
		If MyBase.transform.position.y < Me.platformPos.y + Me.size + Me.shadowRange Then
			Dim num As Single = Mathf.Lerp(CSng((Me.shadowSprites.Length - 1)), 0F, Mathf.InverseLerp(Me.platformPos.y + Me.size, Me.platformPos.y + Me.size + Me.shadowRange, MyBase.transform.position.y))
			Me.shadowRend.sprite = Me.shadowSprites(CInt(num))
		End If
	End Sub

	' Token: 0x060026ED RID: 9965 RVA: 0x0016C861 File Offset: 0x0016AC61
	Public Sub GetCaught()
		Me.isMoving = False
		Me.Recycle()
	End Sub

	' Token: 0x060026EE RID: 9966 RVA: 0x0016C870 File Offset: 0x0016AC70
	Public Sub Explode()
		Me.puppetDead = True
		Me.shadowRend.enabled = False
		Dim component As SpriteRenderer = MyBase.GetComponent(Of SpriteRenderer)()
		component.sortingLayerName = "Effects"
		component.sortingOrder = 100
		MyBase.animator.Play("Explode")
		For i As Integer = 0 To 12 - 1
			Me.coinPrefab.Create(MyBase.transform.position + MathUtils.AngleToDirection(CSng((Global.UnityEngine.Random.Range(0, 360) * Global.UnityEngine.Random.Range(0, 50)))))
		Next
		For j As Integer = 0 To 15 - 1
			Me.featherPrefab.Create(MyBase.transform.position + MathUtils.AngleToDirection(CSng((Global.UnityEngine.Random.Range(0, 360) * Global.UnityEngine.Random.Range(0, 50)))))
		Next
	End Sub

	' Token: 0x060026EF RID: 9967 RVA: 0x0016C956 File Offset: 0x0016AD56
	Private Sub SFX_OMM_P2_DamagePlayerCheer()
		AudioManager.Play("sfx_dlc_omm_p2_puppet_ball_damageplayercheer")
		Me.emitAudioFromObject.Add("sfx_dlc_omm_p2_puppet_ball_damageplayercheer")
	End Sub

	' Token: 0x060026F0 RID: 9968 RVA: 0x0016C972 File Offset: 0x0016AD72
	Private Sub AnimationEvent_SFX_OMM_P2_PuppetBallBounce()
		AudioManager.Play("sfx_dlc_omm_p2_puppet_ball_bounce")
		Me.emitAudioFromObject.Add("sfx_dlc_omm_p2_puppet_ball_bounce")
	End Sub

	' Token: 0x060026F1 RID: 9969 RVA: 0x0016C98E File Offset: 0x0016AD8E
	Private Sub AnimationEvent_ExplodeEnd()
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub

	' Token: 0x060026F2 RID: 9970 RVA: 0x0016C99B File Offset: 0x0016AD9B
	Private Sub WORKAROUND_NullifyFields()
		Me.sprite = Nothing
		Me.shadowRend = Nothing
		Me.shadowSprites = Nothing
		Me.coinPrefab = Nothing
		Me.featherPrefab = Nothing
	End Sub

	' Token: 0x04002F90 RID: 12176
	Private Const GROUND_Y_OFFSET As Single = -10F

	' Token: 0x04002F91 RID: 12177
	Private Const HIT_GROUND_OFFSET As Single = 20F

	' Token: 0x04002F94 RID: 12180
	Private properties As LevelProperties.OldMan.Hands

	' Token: 0x04002F95 RID: 12181
	Private startPos As Vector3

	' Token: 0x04002F96 RID: 12182
	Private endPos As Vector3

	' Token: 0x04002F97 RID: 12183
	Private platformPos As Vector3

	' Token: 0x04002F98 RID: 12184
	Private size As Single = 50F

	' Token: 0x04002F99 RID: 12185
	<SerializeField()>
	Private shadowRange As Single = 100F

	' Token: 0x04002F9A RID: 12186
	<SerializeField()>
	Private sprite As SpriteRenderer

	' Token: 0x04002F9B RID: 12187
	<SerializeField()>
	Private shadowRend As SpriteRenderer

	' Token: 0x04002F9C RID: 12188
	<SerializeField()>
	Private shadowSprites As Sprite()

	' Token: 0x04002F9D RID: 12189
	<SerializeField()>
	Private coinPrefab As Effect

	' Token: 0x04002F9E RID: 12190
	<SerializeField()>
	Private featherPrefab As Effect

	' Token: 0x04002F9F RID: 12191
	Private puppetDead As Boolean
End Class
