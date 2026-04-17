Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020007C4 RID: 1988
Public Class SaltbakerLevelCutter
	Inherits AbstractProjectile

	' Token: 0x1700040C RID: 1036
	' (get) Token: 0x06002CFA RID: 11514 RVA: 0x001A814B File Offset: 0x001A654B
	Protected Overrides ReadOnly Property DestroyLifetime As Single
		Get
			Return 0F
		End Get
	End Property

	' Token: 0x06002CFB RID: 11515 RVA: 0x001A8154 File Offset: 0x001A6554
	Public Function Create(position As Vector3, speed As Single, goingLeft As Boolean, id As Integer) As SaltbakerLevelCutter
		Dim saltbakerLevelCutter As SaltbakerLevelCutter = Me.InstantiatePrefab(Of SaltbakerLevelCutter)()
		saltbakerLevelCutter.transform.position = position
		saltbakerLevelCutter.speed = speed
		saltbakerLevelCutter.goingLeft = goingLeft
		saltbakerLevelCutter.transform.localScale = New Vector3(CSng(If((Not goingLeft), 1, (-1))), 1F)
		If id = 1 Then
			saltbakerLevelCutter.transform.position += Vector3.down * 5F
		End If
		saltbakerLevelCutter.sfxID = id
		saltbakerLevelCutter.SFX_SALTBAKER_P3_PizzaWheel_Loop(id)
		saltbakerLevelCutter.rend.sortingOrder = id
		Return saltbakerLevelCutter
	End Function

	' Token: 0x06002CFC RID: 11516 RVA: 0x001A81F0 File Offset: 0x001A65F0
	Private Sub AniEvent_StartMove()
		MyBase.StartCoroutine(Me.move_cr())
		Me.dustFX.transform.parent = Nothing
		Me.dustFX.SetActive(True)
	End Sub

	' Token: 0x06002CFD RID: 11517 RVA: 0x001A821C File Offset: 0x001A661C
	Private Sub AniEvent_Variation()
		If((Me.goingLeft AndAlso MyBase.transform.position.x > CSng(Level.Current.Left) + 50F + 100F) OrElse (Not Me.goingLeft AndAlso MyBase.transform.position.x < CSng(Level.Current.Right) - 50F - 100F)) AndAlso Global.UnityEngine.Random.Range(0, 4) = 0 Then
			MyBase.animator.SetTrigger("Variation")
		End If
	End Sub

	' Token: 0x06002CFE RID: 11518 RVA: 0x001A82B9 File Offset: 0x001A66B9
	Private Sub AniEvent_ChangeDirection()
		Me.goingLeft = Not Me.goingLeft
	End Sub

	' Token: 0x06002CFF RID: 11519 RVA: 0x001A82CC File Offset: 0x001A66CC
	Private Sub AniEvent_CompleteTurn()
		MyBase.transform.localScale = New Vector3(-MyBase.transform.localScale.x, 1F)
		Me.turning = False
	End Sub

	' Token: 0x06002D00 RID: 11520 RVA: 0x001A8309 File Offset: 0x001A6709
	Private Sub AniEvent_CompleteSink()
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub

	' Token: 0x06002D01 RID: 11521 RVA: 0x001A8316 File Offset: 0x001A6716
	Public Sub Sink()
		MyBase.animator.SetBool("Sink", True)
		Me.SFX_SALTBAKER_P3_PizzaWheel_Dive(Me.sfxID)
	End Sub

	' Token: 0x06002D02 RID: 11522 RVA: 0x001A8338 File Offset: 0x001A6738
	Private Iterator Function move_cr() As IEnumerator
		Dim left As Single = CSng(Level.Current.Left) + 50F
		Dim right As Single = CSng(Level.Current.Right) - 50F
		Dim wait As YieldInstruction = New WaitForFixedUpdate()
		While True
			Dim dir As Vector3 = If((Not Me.goingLeft), Vector3.right, Vector3.left)
			MyBase.transform.position += dir * Me.speed * CupheadTime.FixedDelta
			If Not Me.turning AndAlso ((Me.goingLeft AndAlso MyBase.transform.position.x < left) OrElse (Not Me.goingLeft AndAlso MyBase.transform.position.x > right)) Then
				Me.turning = True
				MyBase.animator.Play("Turn")
			End If
			Yield wait
		End While
		Return
	End Function

	' Token: 0x06002D03 RID: 11523 RVA: 0x001A8353 File Offset: 0x001A6753
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
		MyBase.OnCollisionPlayer(hit, phase)
	End Sub

	' Token: 0x06002D04 RID: 11524 RVA: 0x001A8374 File Offset: 0x001A6774
	Private Sub SFX_SALTBAKER_P3_PizzaWheel_Loop(loopNumber As Integer)
		Dim text As String = "sfx_dlc_saltbaker_p3_pizzawheel_movement_loop_" + (loopNumber + 1)
		AudioManager.PlayLoop(text)
		Me.emitAudioFromObject.Add(text)
	End Sub

	' Token: 0x06002D05 RID: 11525 RVA: 0x001A83A6 File Offset: 0x001A67A6
	Private Sub SFX_SALTBAKER_P3_PizzaWheel_Dive(loopNumber As Integer)
		AudioManager.[Stop]("sfx_dlc_saltbaker_p3_pizzawheel_movement_loop_" + (loopNumber + 1))
	End Sub

	' Token: 0x06002D06 RID: 11526 RVA: 0x001A83BF File Offset: 0x001A67BF
	Private Sub AnimationEvent_SFX_SALTBAKER_P3_RunnerWheel_Spawn()
		AudioManager.Play("sfx_dlc_saltbaker_p3_pizzawheel_spawn")
		Me.emitAudioFromObject.Add("sfx_dlc_saltbaker_p3_pizzawheel_spawn")
	End Sub

	' Token: 0x0400357F RID: 13695
	Private Const SCREEN_EDGE_OFFSET As Single = 50F

	' Token: 0x04003580 RID: 13696
	Private speed As Single

	' Token: 0x04003581 RID: 13697
	Private goingLeft As Boolean

	' Token: 0x04003582 RID: 13698
	Private turning As Boolean

	' Token: 0x04003583 RID: 13699
	Private sfxID As Integer

	' Token: 0x04003584 RID: 13700
	Private properties As LevelProperties.Saltbaker.Cutter

	' Token: 0x04003585 RID: 13701
	<SerializeField()>
	Private dustFX As GameObject

	' Token: 0x04003586 RID: 13702
	<SerializeField()>
	Private rend As SpriteRenderer
End Class
