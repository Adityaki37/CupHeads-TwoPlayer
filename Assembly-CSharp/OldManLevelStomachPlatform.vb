Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000716 RID: 1814
Public Class OldManLevelStomachPlatform
	Inherits LevelPlatform

	' Token: 0x170003D0 RID: 976
	' (get) Token: 0x0600276D RID: 10093 RVA: 0x00172256 File Offset: 0x00170656
	' (set) Token: 0x0600276E RID: 10094 RVA: 0x0017225E File Offset: 0x0017065E
	Public Property isActivated As Boolean

	' Token: 0x0600276F RID: 10095 RVA: 0x00172267 File Offset: 0x00170667
	Private Sub Start()
		Me.isActivated = True
		MyBase.animator.Play("Idle", 0, Global.UnityEngine.Random.Range(0F, 1F))
	End Sub

	' Token: 0x06002770 RID: 10096 RVA: 0x00172290 File Offset: 0x00170690
	Private Sub aniEvent_SpawnParryable()
		Me.main.SpawnParryable(MyBase.transform.position + Me.tongueOffset)
		Me.splashAnimator.Play("OpenSplash")
		Me.splashAnimator.Update(0F)
		Me.SFX_BellLoop()
	End Sub

	' Token: 0x06002771 RID: 10097 RVA: 0x001722E4 File Offset: 0x001706E4
	Public Sub FlipX()
		For Each spriteRenderer As SpriteRenderer In Me.rend
			spriteRenderer.flipX = True
		Next
		Me.boxColl.offset = New Vector2(-Me.boxColl.offset.x, Me.boxColl.offset.y)
		Me.tongueOffset.x = -Me.tongueOffset.x
	End Sub

	' Token: 0x06002772 RID: 10098 RVA: 0x00172366 File Offset: 0x00170766
	Public Sub Anticipation()
		If Me.isActivated Then
			Me.isTargeted = True
			MyBase.animator.SetTrigger("Anticipation")
		End If
	End Sub

	' Token: 0x06002773 RID: 10099 RVA: 0x0017238C File Offset: 0x0017078C
	Public Sub CancelAnticipation()
		Me.isTargeted = False
		If Me.isActivated Then
			MyBase.animator.Play("Idle")
		Else
			Me.isActivated = True
			MyBase.animator.SetBool("IsEating", False)
			MyBase.animator.Play("ReverseEat", 0, 1F - MyBase.animator.GetCurrentAnimatorStateInfo(0).normalizedTime)
			MyBase.animator.Play("Eat_Ripple_End", 1, 0F)
			If Me.bubbleCoroutine IsNot Nothing Then
				MyBase.StopCoroutine(Me.bubbleCoroutine)
			End If
		End If
	End Sub

	' Token: 0x06002774 RID: 10100 RVA: 0x00172430 File Offset: 0x00170830
	Public Function GetTonguePos() As Vector3
		Return MyBase.transform.position + Me.tongueOffset
	End Function

	' Token: 0x06002775 RID: 10101 RVA: 0x00172448 File Offset: 0x00170848
	Public Sub DeactivatePlatform(spawnsParryable As Boolean)
		Me.spawnsParryable = spawnsParryable
		Dim text As String = If((Not spawnsParryable), "IsEating", "IsBell")
		If spawnsParryable Then
			Me.sparkAnimator.Play("Spark")
			Me.SFX_BonkHead()
		Else
			Me.SFX_Chomp()
		End If
		MyBase.animator.SetBool(text, True)
		Me.isActivated = False
		Me.isTargeted = False
		If Not spawnsParryable Then
			Me.bubbleCoroutine = MyBase.StartCoroutine(Me.bubble_cr())
		End If
	End Sub

	' Token: 0x06002776 RID: 10102 RVA: 0x001724CC File Offset: 0x001708CC
	Private Iterator Function bubble_cr() As IEnumerator
		Dim noseTimer As Single = Me.noseBubbleRange.RandomFloat()
		Dim mouthTimer As Single = Me.mouthBubbleRange.RandomFloat()
		While True
			noseTimer -= CupheadTime.Delta
			mouthTimer -= CupheadTime.Delta
			If noseTimer <= 0F Then
				Me.noseBubble.Play("Bubble", 0, 0F)
				Me.noseBubbleRend.flipX = Rand.Bool()
				noseTimer += Me.noseBubbleRange.RandomFloat()
			End If
			If mouthTimer <= 0F Then
				Me.mouthBubble.Play("Bubble", 0, 0F)
				Me.mouthBubbleRend.flipX = Rand.Bool()
				mouthTimer += Me.mouthBubbleRange.RandomFloat()
			End If
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06002777 RID: 10103 RVA: 0x001724E7 File Offset: 0x001708E7
	Public Sub ActivatePlatform()
		MyBase.StartCoroutine(Me.activate_cr())
	End Sub

	' Token: 0x06002778 RID: 10104 RVA: 0x001724F8 File Offset: 0x001708F8
	Private Iterator Function activate_cr() As IEnumerator
		If Me.bubbleCoroutine IsNot Nothing Then
			MyBase.StopCoroutine(Me.bubbleCoroutine)
		End If
		Me.mouthBubble.Play("None")
		Me.noseBubble.Play("None")
		If MyBase.animator.GetBool("IsBell") Then
			MyBase.animator.SetBool("IsBell", False)
			MyBase.animator.Play("Bell_End")
		Else
			Yield CupheadTime.WaitForSeconds(Me, Global.UnityEngine.Random.Range(0F, 0.2F))
			MyBase.animator.SetBool("IsEating", False)
		End If
		Me.isActivated = True
		Me.spawnsParryable = False
		Return
	End Function

	' Token: 0x06002779 RID: 10105 RVA: 0x00172513 File Offset: 0x00170913
	Private Sub SFX_Chomp()
		AudioManager.Play("sfx_dlc_omm_p3_dino_chomp")
		Me.emitAudioFromObject.Add("sfx_dlc_omm_p3_dino_chomp")
	End Sub

	' Token: 0x0600277A RID: 10106 RVA: 0x0017252F File Offset: 0x0017092F
	Private Sub SFX_BonkHead()
		AudioManager.Play("sfx_dlc_omm_p3_dinobells_bonkhead")
		Me.emitAudioFromObject.Add("sfx_dlc_omm_p3_dinobells_bonkhead")
	End Sub

	' Token: 0x0600277B RID: 10107 RVA: 0x0017254B File Offset: 0x0017094B
	Private Sub SFX_BellLoop()
		AudioManager.PlayLoop("sfx_dlc_omm_p3_dinobells_loop")
		Me.emitAudioFromObject.Add("sfx_dlc_omm_p3_dinobells_loop")
	End Sub

	' Token: 0x0600277C RID: 10108 RVA: 0x00172567 File Offset: 0x00170967
	Private Sub AniEvent_SFX_BellLoopEnd()
		AudioManager.[Stop]("sfx_dlc_omm_p3_dinobells_loop")
	End Sub

	' Token: 0x0400302D RID: 12333
	<SerializeField()>
	Private rend As SpriteRenderer()

	' Token: 0x0400302E RID: 12334
	<SerializeField()>
	Private boxColl As BoxCollider2D

	' Token: 0x04003030 RID: 12336
	Public isTargeted As Boolean

	' Token: 0x04003031 RID: 12337
	Private spawnsParryable As Boolean

	' Token: 0x04003032 RID: 12338
	Public sparkAnimator As Animator

	' Token: 0x04003033 RID: 12339
	Private tongueOffset As Vector3 = New Vector3(20F, 30F)

	' Token: 0x04003034 RID: 12340
	Public main As OldManLevelGnomeLeader

	' Token: 0x04003035 RID: 12341
	<SerializeField()>
	Private splashAnimator As Animator

	' Token: 0x04003036 RID: 12342
	<SerializeField()>
	Private mouthBubble As Animator

	' Token: 0x04003037 RID: 12343
	<SerializeField()>
	Private noseBubble As Animator

	' Token: 0x04003038 RID: 12344
	<SerializeField()>
	Private mouthBubbleRend As SpriteRenderer

	' Token: 0x04003039 RID: 12345
	<SerializeField()>
	Private noseBubbleRend As SpriteRenderer

	' Token: 0x0400303A RID: 12346
	<SerializeField()>
	Private noseBubbleRange As MinMax = New MinMax(0.5833333F, 1F)

	' Token: 0x0400303B RID: 12347
	<SerializeField()>
	Private mouthBubbleRange As MinMax = New MinMax(1F, 1.5833334F)

	' Token: 0x0400303C RID: 12348
	Private bubbleCoroutine As Coroutine
End Class
