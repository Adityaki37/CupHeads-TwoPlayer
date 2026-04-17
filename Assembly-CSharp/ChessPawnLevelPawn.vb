Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000545 RID: 1349
Public Class ChessPawnLevelPawn
	Inherits AbstractProjectile

	' Token: 0x17000338 RID: 824
	' (get) Token: 0x060018C6 RID: 6342 RVA: 0x000E078A File Offset: 0x000DEB8A
	Public Overrides ReadOnly Property ParryMeterMultiplier As Single
		Get
			Return 0F
		End Get
	End Property

	' Token: 0x17000339 RID: 825
	' (get) Token: 0x060018C7 RID: 6343 RVA: 0x000E0791 File Offset: 0x000DEB91
	' (set) Token: 0x060018C8 RID: 6344 RVA: 0x000E0799 File Offset: 0x000DEB99
	Public Property speed As Single

	' Token: 0x1700033A RID: 826
	' (get) Token: 0x060018C9 RID: 6345 RVA: 0x000E07A2 File Offset: 0x000DEBA2
	' (set) Token: 0x060018CA RID: 6346 RVA: 0x000E07AA File Offset: 0x000DEBAA
	Public Property inUse As Boolean

	' Token: 0x1700033B RID: 827
	' (get) Token: 0x060018CB RID: 6347 RVA: 0x000E07B3 File Offset: 0x000DEBB3
	' (set) Token: 0x060018CC RID: 6348 RVA: 0x000E07BB File Offset: 0x000DEBBB
	Public Property currentIndex As Integer

	' Token: 0x060018CD RID: 6349 RVA: 0x000E07C4 File Offset: 0x000DEBC4
	Public Function Init(level As ChessPawnLevel) As ChessPawnLevelPawn
		Dim chessPawnLevelPawn As ChessPawnLevelPawn = Global.UnityEngine.[Object].Instantiate(Of ChessPawnLevelPawn)(Me, Camera.main.transform.position + Vector3.up * 2000F, Quaternion.identity)
		chessPawnLevelPawn.level = level
		Return chessPawnLevelPawn
	End Function

	' Token: 0x060018CE RID: 6350 RVA: 0x000E0808 File Offset: 0x000DEC08
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me.level = Nothing
	End Sub

	' Token: 0x060018CF RID: 6351 RVA: 0x000E0817 File Offset: 0x000DEC17
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
		MyBase.OnCollisionPlayer(hit, phase)
	End Sub

	' Token: 0x060018D0 RID: 6352 RVA: 0x000E0835 File Offset: 0x000DEC35
	Protected Overrides Sub Die()
		MyBase.Die()
	End Sub

	' Token: 0x060018D1 RID: 6353 RVA: 0x000E083D File Offset: 0x000DEC3D
	Public Sub SetIndex(i As Integer)
		Me.currentIndex = i
		If i >= 0 Then
			Me.lastIndex = i
		End If
	End Sub

	' Token: 0x060018D2 RID: 6354 RVA: 0x000E0854 File Offset: 0x000DEC54
	Protected Overrides Sub OnDieDistance()
	End Sub

	' Token: 0x060018D3 RID: 6355 RVA: 0x000E0856 File Offset: 0x000DEC56
	Protected Overrides Sub OnDieLifetime()
	End Sub

	' Token: 0x060018D4 RID: 6356 RVA: 0x000E0858 File Offset: 0x000DEC58
	Public Overrides Sub OnLevelEnd()
	End Sub

	' Token: 0x060018D5 RID: 6357 RVA: 0x000E085C File Offset: 0x000DEC5C
	Public Overrides Sub OnParry(player As AbstractPlayerController)
		Me.parryCount += 1
		If PlayerManager.BothPlayersActive() AndAlso Me.parryCount < 2 Then
			Return
		End If
		Me.SetParryable(False)
		MyBase.StartCoroutine(Me.disable_collision_cr())
		If Me.state = ChessPawnLevelPawn.State.Run Then
			MyBase.animator.SetTrigger("Parry")
		End If
		Me.parriedHead.CreatePart(MyBase.transform.position + Vector3.up * 100F)
		Me.headRenderer.enabled = False
		MyBase.StartCoroutine(Me.SFX_KOG_PAWN_PawnParry_cr())
		Me.level.TakeDamage()
	End Sub

	' Token: 0x060018D6 RID: 6358 RVA: 0x000E090D File Offset: 0x000DED0D
	Public Sub StartIntro()
		MyBase.StartCoroutine(Me.intro_cr())
	End Sub

	' Token: 0x060018D7 RID: 6359 RVA: 0x000E091C File Offset: 0x000DED1C
	Private Iterator Function intro_cr() As IEnumerator
		Me.inUse = True
		Yield MyBase.StartCoroutine(Me.drop_cr(True))
		Me.inUse = False
		Return
	End Function

	' Token: 0x060018D8 RID: 6360 RVA: 0x000E0938 File Offset: 0x000DED38
	Private Iterator Function drop_cr(isIntro As Boolean) As IEnumerator
		Dim animationHelper As AnimationHelper = MyBase.GetComponent(Of AnimationHelper)()
		Dim targetPosition As Vector3 = Me.level.GetPosition(Me.currentIndex)
		MyBase.animator.Play("IntroStart")
		MyBase.animator.SetInteger("Intro", If((Not isIntro), 0, (Me.currentIndex Mod 2 + 1)))
		targetPosition.z = CSng((Me.currentIndex Mod 2)) * 0.0001F
		MyBase.animator.Update(0F)
		animationHelper.Speed = 0F
		Me.bodyRenderer.sortingOrder = 0
		Me.headRenderer.sortingOrder = 1
		Dim t As Single = 0F
		Dim dropPosition As Vector3 = targetPosition + ChessPawnLevelPawn.DropPositionOffset
		Dim wait As YieldInstruction = New WaitForFixedUpdate()
		While t < 1F
			MyBase.transform.position = Vector3.Lerp(dropPosition, targetPosition, t)
			t += CupheadTime.FixedDelta * 8F
			Yield wait
		End While
		animationHelper.Speed = 1F
		MyBase.transform.position = targetPosition
		Yield MyBase.animator.WaitForAnimationToStart(Me, "Idle", False)
		Return
	End Function

	' Token: 0x060018D9 RID: 6361 RVA: 0x000E095A File Offset: 0x000DED5A
	Public Sub Attack(warningTime As Single, horiztonalMovement As Single, dropSpeed As Single, runDelay As Single, runSpeed As Single, returnSpeed As Single)
		MyBase.StartCoroutine(Me.attack_cr(warningTime, horiztonalMovement, dropSpeed, runDelay, runSpeed, returnSpeed))
	End Sub

	' Token: 0x060018DA RID: 6362 RVA: 0x000E0974 File Offset: 0x000DED74
	Private Iterator Function attack_cr(warningTime As Single, horizontalMovement As Single, dropSpeed As Single, runDelay As Single, runSpeed As Single, returnDelay As Single) As IEnumerator
		Me.inUse = True
		Me.initialPosition = MyBase.transform.position
		Dim wait As YieldInstruction = New WaitForFixedUpdate()
		MyBase.animator.SetTrigger("JumpWarning")
		Yield CupheadTime.WaitForSeconds(Me, warningTime)
		Me.currentIndex = -1
		Me.state = ChessPawnLevelPawn.State.Jump
		Me.collider.enabled = True
		MyBase.animator.SetTrigger("Jump")
		Yield CupheadTime.WaitForSeconds(Me, 0.125F)
		If horizontalMovement <> 0F Then
			MyBase.transform.SetScale(New Single?(-Mathf.Sign(horizontalMovement)), Nothing, Nothing)
		End If
		Dim horizontalMovementCoroutine As Coroutine = MyBase.StartCoroutine(Me.horizontalMovement_cr(horizontalMovement, dropSpeed))
		While Not Me.beginFall
			Yield Nothing
		End While
		Me.beginFall = False
		Dim t As Single = 0F
		While t < 1F
			Dim position As Vector3 = MyBase.transform.position
			position.y = Me.initialPosition.y - 650F + 650F * Mathf.Sin(1.5707964F + t * 3.1415927F / 2F)
			If position.y < MyBase.transform.position.y Then
				Me.bodyRenderer.sortingOrder = 20
				Me.headRenderer.sortingOrder = 21
			End If
			MyBase.transform.position = position
			t += CupheadTime.FixedDelta * dropSpeed
			Yield wait
		End While
		MyBase.StopCoroutine(horizontalMovementCoroutine)
		MyBase.transform.position = New Vector3(MyBase.transform.position.x, Me.initialPosition.y - 650F)
		Dim testDir As Single = Mathf.Sign(PlayerManager.GetNext().transform.position.x - MyBase.transform.position.x)
		Dim quickLand As Boolean = Me.level.ClearToRun(testDir, MyBase.transform.position)
		If runDelay = 0F AndAlso quickLand Then
			MyBase.animator.SetInteger("Land", 1)
			MyBase.transform.SetScale(New Single?(testDir), Nothing, Nothing)
		Else
			MyBase.animator.SetInteger("Land", 2)
			Dim delay As Single = runDelay - 0.625F
			Yield CupheadTime.WaitForSeconds(Me, runDelay)
			While Not Me.level.ClearToRun(testDir, MyBase.transform.position)
				Yield wait
				testDir = Mathf.Sign(PlayerManager.GetNext().transform.position.x - MyBase.transform.position.x)
			End While
			MyBase.animator.SetInteger("Land", 3)
			MyBase.transform.SetScale(New Single?(testDir), Nothing, Nothing)
			Yield MyBase.animator.WaitForAnimationToStart(Me, "LandLongToRun", False)
		End If
		Me.state = ChessPawnLevelPawn.State.Run
		Me.speed = runSpeed * testDir
		While Mathf.Abs(MyBase.transform.position.x - Camera.main.transform.position.x) < 850F
			MyBase.transform.position += Me.speed * CupheadTime.FixedDelta * Vector3.right
			Yield wait
		End While
		Me.speed = 0F
		Me.state = ChessPawnLevelPawn.State.Idle
		MyBase.animator.SetInteger("Land", 0)
		Me.collider.enabled = False
		Me.currentIndex = Me.level.GetReturnIndex()
		Yield MyBase.StartCoroutine(Me.drop_cr(False))
		Me.inUse = False
		Return
	End Function

	' Token: 0x060018DB RID: 6363 RVA: 0x000E09B4 File Offset: 0x000DEDB4
	Private Iterator Function horizontalMovement_cr(horizontalMovement As Single, dropSpeed As Single) As IEnumerator
		Dim duration As Single = 1F / dropSpeed
		duration += 0.45833334F
		Dim horizontalSpeed As Single = horizontalMovement / duration
		Dim wait As YieldInstruction = New WaitForFixedUpdate()
		While True
			Dim position As Vector3 = MyBase.transform.position
			position.x += CupheadTime.FixedDelta * horizontalSpeed
			MyBase.transform.position = position
			Yield wait
		End While
		Return
	End Function

	' Token: 0x060018DC RID: 6364 RVA: 0x000E09E0 File Offset: 0x000DEDE0
	Public Sub Death()
		Me.StopAllCoroutines()
		MyBase.StartCoroutine(Me.death_cr())
		If Me.headRenderer.enabled Then
			Me.parriedHead.CreatePart(MyBase.transform.position + Vector3.up * 100F)
			Me.collider.enabled = False
		End If
	End Sub

	' Token: 0x060018DD RID: 6365 RVA: 0x000E0A48 File Offset: 0x000DEE48
	Private Iterator Function death_cr() As IEnumerator
		Me.collider.enabled = False
		MyBase.transform.SetScale(New Single?(1F), Nothing, Nothing)
		MyBase.transform.position = New Vector3(MyBase.transform.position.x, MyBase.transform.position.y, Me.initialPosition.x)
		MyBase.GetComponent(Of AnimationHelper)().Speed = 1F
		MyBase.animator.Play("DeathTwitch", 0, CSng(Me.lastIndex) * 0.125F)
		Dim smoke As Effect = Me.deathSmokeEffect.Create(MyBase.transform.position)
		MyBase.StartCoroutine(Me.move_smoke_cr(smoke))
		Dim delay As Single = CSng((Me.lastIndex Mod 4 * 2 + Me.lastIndex / 2)) * Me.deathTwitchDelayFixed + Global.UnityEngine.Random.Range(Me.deathTwitchDelayRange.minimum, Me.deathTwitchDelayRange.maximum)
		Yield CupheadTime.WaitForSeconds(Me, delay)
		MyBase.animator.Play("DeathAngel", 0, Global.UnityEngine.Random.Range(0F, 1F))
		smoke.animator.Play("Explode")
		Me.deathBody.CreatePart(MyBase.transform.position)
		While True
			Dim position As Vector3 = MyBase.transform.position
			position.y += Me.deathFloatUpSpeed * CupheadTime.Delta
			MyBase.transform.position = position
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x060018DE RID: 6366 RVA: 0x000E0A64 File Offset: 0x000DEE64
	Private Iterator Function move_smoke_cr(smoke As Effect) As IEnumerator
		Dim smokeRenderer As SpriteRenderer = smoke.GetComponent(Of SpriteRenderer)()
		While smoke IsNot Nothing
			smoke.transform.position = MyBase.transform.position + MathUtils.AngleToDirection(CSng(Global.UnityEngine.Random.Range(0, 360))) * 50F
			While smokeRenderer IsNot Nothing AndAlso smokeRenderer.sprite IsNot Nothing
				Yield Nothing
			End While
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x060018DF RID: 6367 RVA: 0x000E0A86 File Offset: 0x000DEE86
	Private Sub animationEvent_BeginFall()
		Me.beginFall = True
	End Sub

	' Token: 0x060018E0 RID: 6368 RVA: 0x000E0A90 File Offset: 0x000DEE90
	Private Iterator Function disable_collision_cr() As IEnumerator
		Me.collider.enabled = False
		Yield CupheadTime.WaitForSeconds(Me, 0.2F)
		Me.noHeadCollider.enabled = True
		Return
	End Function

	' Token: 0x060018E1 RID: 6369 RVA: 0x000E0AAB File Offset: 0x000DEEAB
	Private Sub AnimationEvent_SFX_KOG_PAWN_PawnLand()
		AudioManager.Play("sfx_dlc_kog_pawn_land")
		Me.emitAudioFromObject.Add("sfx_dlc_kog_pawn_land")
	End Sub

	' Token: 0x060018E2 RID: 6370 RVA: 0x000E0AC7 File Offset: 0x000DEEC7
	Private Sub AnimationEvent_SFX_KOG_PAWN_PawnJumpDown()
		AudioManager.Play("sfx_dlc_kog_pawn_jumpdown")
		Me.emitAudioFromObject.Add("sfx_dlc_kog_pawn_jumpdown")
	End Sub

	' Token: 0x060018E3 RID: 6371 RVA: 0x000E0AE3 File Offset: 0x000DEEE3
	Private Sub AnimationEvent_SFX_KOG_PAWN_PawnParryHit()
		AudioManager.Play(String.Empty)
		Me.emitAudioFromObject.Add("sfx_dlc_kog_pawn_parryhit")
	End Sub

	' Token: 0x060018E4 RID: 6372 RVA: 0x000E0B00 File Offset: 0x000DEF00
	Private Iterator Function SFX_KOG_PAWN_PawnParry_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 0.1F)
		AudioManager.Play("sfx_dlc_kog_pawn_parryhit")
		Me.emitAudioFromObject.Add("sfx_dlc_kog_pawn_parryhit")
		Yield CupheadTime.WaitForSeconds(Me, 0.15F)
		AudioManager.Play("sfx_dlc_kog_pawn_parrywoodbreak")
		Me.emitAudioFromObject.Add("sfx_dlc_kog_pawn_parrywoodbreak")
		Return
	End Function

	' Token: 0x040021D1 RID: 8657
	Private Const FALL_DISTANCE As Single = 650F

	' Token: 0x040021D2 RID: 8658
	Private Shared DropPositionOffset As Vector3 = New Vector3(0F, 100F)

	' Token: 0x040021D3 RID: 8659
	<SerializeField()>
	Private collider As Collider2D

	' Token: 0x040021D4 RID: 8660
	<SerializeField()>
	Private bodyRenderer As SpriteRenderer

	' Token: 0x040021D5 RID: 8661
	<SerializeField()>
	Private headRenderer As SpriteRenderer

	' Token: 0x040021D6 RID: 8662
	<SerializeField()>
	Private parriedHead As SpriteDeathParts

	' Token: 0x040021D7 RID: 8663
	<SerializeField()>
	Private deathTwitchDelayFixed As Single

	' Token: 0x040021D8 RID: 8664
	<SerializeField()>
	Private deathTwitchDelayRange As Rangef

	' Token: 0x040021D9 RID: 8665
	<SerializeField()>
	Private deathFloatUpSpeed As Single

	' Token: 0x040021DA RID: 8666
	<SerializeField()>
	Private deathSmokeEffect As Effect

	' Token: 0x040021DB RID: 8667
	<SerializeField()>
	Private deathBody As SpriteDeathParts

	' Token: 0x040021DC RID: 8668
	<SerializeField()>
	Private noHeadCollider As BoxCollider2D

	' Token: 0x040021DD RID: 8669
	Private level As ChessPawnLevel

	' Token: 0x040021DE RID: 8670
	Private state As ChessPawnLevelPawn.State

	' Token: 0x040021DF RID: 8671
	Private initialPosition As Vector3

	' Token: 0x040021E0 RID: 8672
	Private beginFall As Boolean

	' Token: 0x040021E1 RID: 8673
	Private parryCount As Integer

	' Token: 0x040021E2 RID: 8674
	Private lastIndex As Integer

	' Token: 0x02000546 RID: 1350
	Private Enum State
		' Token: 0x040021E7 RID: 8679
		Idle
		' Token: 0x040021E8 RID: 8680
		Jump
		' Token: 0x040021E9 RID: 8681
		Run
	End Enum
End Class
