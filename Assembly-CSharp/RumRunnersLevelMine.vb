Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000792 RID: 1938
Public Class RumRunnersLevelMine
	Inherits AbstractProjectile

	' Token: 0x170003F3 RID: 1011
	' (get) Token: 0x06002AF1 RID: 10993 RVA: 0x00190BF3 File Offset: 0x0018EFF3
	' (set) Token: 0x06002AF2 RID: 10994 RVA: 0x00190BFB File Offset: 0x0018EFFB
	Public Property xPos As Integer

	' Token: 0x170003F4 RID: 1012
	' (get) Token: 0x06002AF3 RID: 10995 RVA: 0x00190C04 File Offset: 0x0018F004
	' (set) Token: 0x06002AF4 RID: 10996 RVA: 0x00190C0C File Offset: 0x0018F00C
	Public Property yPos As Integer

	' Token: 0x170003F5 RID: 1013
	' (get) Token: 0x06002AF5 RID: 10997 RVA: 0x00190C15 File Offset: 0x0018F015
	' (set) Token: 0x06002AF6 RID: 10998 RVA: 0x00190C1D File Offset: 0x0018F01D
	Public Property endPhaseExplodePriority As Integer

	' Token: 0x170003F6 RID: 1014
	' (get) Token: 0x06002AF7 RID: 10999 RVA: 0x00190C26 File Offset: 0x0018F026
	Protected Overrides ReadOnly Property DestroyLifetime As Single
		Get
			Return 0F
		End Get
	End Property

	' Token: 0x06002AF8 RID: 11000 RVA: 0x00190C30 File Offset: 0x0018F030
	Public Function Init(targetPos As Vector3, properties As LevelProperties.RumRunners.Mine, parent As RumRunnersLevelSpider, xPos As Integer, yPos As Integer) As RumRunnersLevelMine
		MyBase.ResetLifetime()
		MyBase.ResetDistance()
		MyBase.transform.position = New Vector3(targetPos.x, 800F, -targetPos.y * 1E-05F)
		Me.targetPos = targetPos
		Me.targetPos.z = -targetPos.y * 1E-05F
		Me.xPos = xPos
		Me.yPos = yPos
		If Me.xPos = 3 AndAlso Me.yPos = 2 Then
			Me.endPhaseExplodePriority = 0
		ElseIf Me.xPos = 2 Then
			Me.endPhaseExplodePriority = 1
		Else
			Me.endPhaseExplodePriority = 2
		End If
		Me.properties = properties
		MyBase.animator.Play("Drop")
		MyBase.GetComponent(Of SpriteRenderer)().enabled = True
		Me.parent = parent
		MyBase.StartCoroutine(Me.lifetime_cr())
		Me.MoveDown()
		Me.webRenderer.transform.eulerAngles = New Vector3(0F, 0F, CSng(Global.UnityEngine.Random.Range(0, 360)))
		Me.explosionRenderer.transform.eulerAngles = New Vector3(0F, 0F, CSng(Global.UnityEngine.Random.Range(0, 360)))
		Me.smokeRenderer.transform.eulerAngles = New Vector3(0F, 0F, CSng(Global.UnityEngine.Random.Range(0, 360)))
		Me.webRenderer.transform.localScale = New Vector3(CSng(MathUtils.PlusOrMinus()), CSng(MathUtils.PlusOrMinus()), 1F)
		Me.explosionRenderer.transform.localScale = New Vector3(CSng(MathUtils.PlusOrMinus()), CSng(MathUtils.PlusOrMinus()), 1F)
		Me.smokeRenderer.transform.localScale = New Vector3(CSng(MathUtils.PlusOrMinus()), CSng(MathUtils.PlusOrMinus()), 1F)
		If yPos = 0 Then
			AudioManager.Play("sfx_dlc_rumrun_mine_drop_high")
			Me.emitAudioFromObject.Add("sfx_dlc_rumrun_mine_drop_high")
		ElseIf yPos = 1 Then
			AudioManager.Play("sfx_dlc_rumrun_mine_drop_mid")
			Me.emitAudioFromObject.Add("sfx_dlc_rumrun_mine_drop_mid")
		Else
			AudioManager.Play("sfx_dlc_rumrun_mine_drop_low")
			Me.emitAudioFromObject.Add("sfx_dlc_rumrun_mine_drop_low")
		End If
		Return Me
	End Function

	' Token: 0x06002AF9 RID: 11001 RVA: 0x00190E7E File Offset: 0x0018F27E
	Private Sub MoveDown()
		MyBase.StartCoroutine(Me.move_down_cr())
	End Sub

	' Token: 0x06002AFA RID: 11002 RVA: 0x00190E90 File Offset: 0x0018F290
	Private Iterator Function move_down_cr() As IEnumerator
		MyBase.transform.position = Me.targetPos
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Drop", False, True)
		MyBase.StartCoroutine(Me.check_distance_cr())
		Return
	End Function

	' Token: 0x06002AFB RID: 11003 RVA: 0x00190EAC File Offset: 0x0018F2AC
	Private Iterator Function check_distance_cr() As IEnumerator
		Me.damageDealer.SetDamage(Me.properties.mineBossDamage)
		Me.damageDealer.SetRate(0F)
		Me.checkingPlayers = True
		While Me.checkingPlayers
			If Me.parent AndAlso Me.parent.moving AndAlso Not Me.parent.isSummoning AndAlso Vector3.Distance(Me.parent.transform.position, MyBase.transform.position) < Me.properties.mineDistToExplode Then
				MyBase.animator.Play(If((Not Me.parent.goingLeft), "SwingRight", "SwingLeft"))
			End If
			Dim player As LevelPlayerController = TryCast(PlayerManager.GetPlayer(PlayerId.PlayerOne), LevelPlayerController)
			Dim player2 As LevelPlayerController = TryCast(PlayerManager.GetPlayer(PlayerId.PlayerTwo), LevelPlayerController)
			Dim player1Dist As Single = Vector3.Distance(player.center, MyBase.transform.position)
			If Not player.IsDead AndAlso player1Dist < Me.properties.mineDistToExplode Then
				Me.checkingPlayers = False
			End If
			If player2 IsNot Nothing Then
				Dim num As Single = Vector3.Distance(player2.center, MyBase.transform.position)
				If Not player2.IsDead AndAlso num < Me.properties.mineDistToExplode Then
					Me.checkingPlayers = False
				End If
			End If
			Yield Nothing
		End While
		If Not Me.exploding Then
			MyBase.StartCoroutine(Me.explosion_cr(False))
		End If
		Return
	End Function

	' Token: 0x06002AFC RID: 11004 RVA: 0x00190EC8 File Offset: 0x0018F2C8
	Private Iterator Function explosion_cr(timedOut As Boolean) As IEnumerator
		Me.exploding = True
		MyBase.animator.Play("PreExplode")
		AudioManager.Play("sfx_dlc_rumrun_mine_preexplode")
		Me.emitAudioFromObject.Add("sfx_dlc_rumrun_mine_preexplode")
		Yield CupheadTime.WaitForSeconds(Me, Me.properties.mineExplosionWarning * CSng(If((Not timedOut), 1, 2)))
		AudioManager.Play("sfx_dlc_rumrun_mine_explode")
		Me.emitAudioFromObject.Add("sfx_dlc_rumrun_mine_explode")
		MyBase.animator.Play("Explode")
		Return
	End Function

	' Token: 0x06002AFD RID: 11005 RVA: 0x00190EEA File Offset: 0x0018F2EA
	Protected Overrides Sub OnCollisionEnemy(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x06002AFE RID: 11006 RVA: 0x00190F08 File Offset: 0x0018F308
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x06002AFF RID: 11007 RVA: 0x00190F26 File Offset: 0x0018F326
	Public Sub SetTimer(t As Single)
		If Not Me.exploding Then
			Me.timer = t
		End If
	End Sub

	' Token: 0x06002B00 RID: 11008 RVA: 0x00190F3C File Offset: 0x0018F33C
	Private Iterator Function lifetime_cr() As IEnumerator
		Me.timer = Me.properties.mineTimer
		While Me.timer > 0F
			Me.timer -= CupheadTime.Delta
			Yield Nothing
		End While
		Me.checkingPlayers = False
		If Not Me.exploding Then
			MyBase.StartCoroutine(Me.explosion_cr(True))
		End If
		Return
	End Function

	' Token: 0x06002B01 RID: 11009 RVA: 0x00190F57 File Offset: 0x0018F357
	Private Sub Death()
		Me.StopAllCoroutines()
		Me.Recycle()
	End Sub

	' Token: 0x040033A6 RID: 13222
	Private Const START_HEIGHT As Single = 800F

	' Token: 0x040033AA RID: 13226
	Private properties As LevelProperties.RumRunners.Mine

	' Token: 0x040033AB RID: 13227
	Private parent As RumRunnersLevelSpider

	' Token: 0x040033AC RID: 13228
	Private targetPos As Vector3

	' Token: 0x040033AD RID: 13229
	Private checkingPlayers As Boolean

	' Token: 0x040033AE RID: 13230
	Private exploding As Boolean

	' Token: 0x040033AF RID: 13231
	Private timer As Single

	' Token: 0x040033B0 RID: 13232
	<SerializeField()>
	Private webRenderer As GameObject

	' Token: 0x040033B1 RID: 13233
	<SerializeField()>
	Private explosionRenderer As GameObject

	' Token: 0x040033B2 RID: 13234
	<SerializeField()>
	Private smokeRenderer As GameObject
End Class
