Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020008B7 RID: 2231
Public Class FunhousePlatformingLevelJackinBox
	Inherits PlatformingLevelShootingEnemy

	' Token: 0x06003409 RID: 13321 RVA: 0x001E3248 File Offset: 0x001E1648
	Protected Overrides Sub Start()
		MyBase.Start()
		Me.directionIndex = Global.UnityEngine.Random.Range(0, MyBase.Properties.jackinDirectionString.Split(New Char() { ","c }).Length)
		AddHandler Me.jack.GetComponent(Of DamageReceiver)().OnDamageTaken, AddressOf Me.OnDamageTaken
		AudioManager.PlayLoop("funhouse_jackbox_eye_spin_loop")
		Me.emitAudioFromObject.Add("funhouse_jackbox_eye_spin_loop")
		MyBase.StartCoroutine(Me.check_to_start_cr())
	End Sub

	' Token: 0x0600340A RID: 13322 RVA: 0x001E32C8 File Offset: 0x001E16C8
	Protected Overrides Sub OnStart()
		MyBase.OnStart()
		MyBase.StartCoroutine(Me.pop_up_cr())
	End Sub

	' Token: 0x0600340B RID: 13323 RVA: 0x001E32E0 File Offset: 0x001E16E0
	Private Iterator Function check_to_start_cr() As IEnumerator
		While MyBase.transform.position.x > CupheadLevelCamera.Current.Bounds.xMax + Me.offset
			Yield Nothing
		End While
		Me.OnStart()
		Yield Nothing
		Return
	End Function

	' Token: 0x0600340C RID: 13324 RVA: 0x001E32FB File Offset: 0x001E16FB
	Protected Overrides Sub Shoot()
		If Me.shootTime Then
			MyBase.Shoot()
		End If
	End Sub

	' Token: 0x0600340D RID: 13325 RVA: 0x001E3310 File Offset: 0x001E1710
	Private Iterator Function pop_up_cr() As IEnumerator
		Dim dir As String = String.Empty
		While True
			While MyBase.transform.position.x > CupheadLevelCamera.Current.Bounds.xMax + Me.offset OrElse MyBase.transform.position.x < CupheadLevelCamera.Current.Bounds.xMin - Me.offset
				Yield Nothing
			End While
			Me.justDied = False
			If MyBase.Properties.jackinDirectionString.Split(New Char() { ","c })(Me.directionIndex)(0) = "U"c Then
				MyBase.animator.SetInteger("Direction", 1)
				Me.jack.transform.position = Me.topRoot.transform.position
				Me.jack.transform.SetEulerAngles(Nothing, Nothing, New Single?(270F))
				Me.jack.GetComponent(Of SpriteRenderer)().sortingOrder = -5
				dir = "Up"
			ElseIf MyBase.Properties.jackinDirectionString.Split(New Char() { ","c })(Me.directionIndex)(0) = "L"c Then
				MyBase.animator.SetInteger("Direction", 2)
				Me.jack.transform.position = Me.leftRoot.transform.position
				Me.jack.transform.SetEulerAngles(Nothing, Nothing, New Single?(0F))
				Me.jack.GetComponent(Of SpriteRenderer)().sortingOrder = 5
				dir = "Left"
			ElseIf MyBase.Properties.jackinDirectionString.Split(New Char() { ","c })(Me.directionIndex)(0) = "D"c Then
				MyBase.animator.SetInteger("Direction", 3)
				Me.jack.transform.position = Me.bottomRoot.transform.position
				Me.jack.transform.SetEulerAngles(Nothing, Nothing, New Single?(90F))
				Me.jack.GetComponent(Of SpriteRenderer)().sortingOrder = -5
				dir = "Down"
			ElseIf MyBase.Properties.jackinDirectionString.Split(New Char() { ","c })(Me.directionIndex)(0) = "R"c Then
				MyBase.animator.SetInteger("Direction", 4)
				Me.jack.transform.position = Me.rightRoot.transform.position
				Me.jack.transform.SetEulerAngles(Nothing, Nothing, New Single?(180F))
				Me.jack.GetComponent(Of SpriteRenderer)().sortingOrder = -5
				dir = "Right"
			End If
			MyBase.animator.SetTrigger("OnDirection")
			Yield MyBase.animator.WaitForAnimationToStart(Me, "Eye_" + dir, 1, False)
			AudioManager.[Stop]("funhouse_jackbox_eye_spin_loop")
			Yield CupheadTime.WaitForSeconds(Me, 0.3F)
			AudioManager.PlayLoop("funhouse_jackbox_eye_spin_loop")
			Me.emitAudioFromObject.Add("funhouse_jackbox_eye_spin_loop")
			MyBase.animator.SetTrigger("OnHead")
			Yield MyBase.animator.WaitForAnimationToEnd(Me, "Jack_Head", 3, False, True)
			If Not Me.justDied Then
				Me.shootTime = True
				Me.shootTime = False
			End If
			Yield CupheadTime.WaitForSeconds(Me, Me.DieTime())
			Me.directionIndex = (Me.directionIndex + 1) Mod MyBase.Properties.jackinDirectionString.Split(New Char() { ","c }).Length
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x0600340E RID: 13326 RVA: 0x001E332C File Offset: 0x001E172C
	Private Sub HideSprite()
		Select Case MyBase.animator.GetInteger("Direction")
			Case 1
				Me.top.GetComponent(Of SpriteRenderer)().enabled = False
			Case 2
				Me.left.GetComponent(Of SpriteRenderer)().enabled = False
			Case 3
				Me.bottom.GetComponent(Of SpriteRenderer)().enabled = False
			Case 4
				Me.right.GetComponent(Of SpriteRenderer)().enabled = False
		End Select
	End Sub

	' Token: 0x0600340F RID: 13327 RVA: 0x001E33C0 File Offset: 0x001E17C0
	Private Sub SlideSprite()
		Select Case MyBase.animator.GetInteger("Direction")
			Case 1
				MyBase.StartCoroutine(Me.slide_in(Me.top, Vector3.up))
			Case 2
				MyBase.StartCoroutine(Me.slide_in(Me.left, Vector3.left))
			Case 3
				MyBase.StartCoroutine(Me.slide_in(Me.bottom, Vector3.down))
			Case 4
				MyBase.StartCoroutine(Me.slide_in(Me.right, Vector3.right))
		End Select
	End Sub

	' Token: 0x06003410 RID: 13328 RVA: 0x001E3470 File Offset: 0x001E1870
	Private Sub ShootProjectile()
		If Not Me.justDied Then
			Me.player = PlayerManager.GetNext()
			Me.projectile.Create(Me.jackRoot.transform.position, MyBase.Properties.ProjectileSpeed, MyBase.Properties.jackinShootDelay, Me.player, MyBase.animator.GetInteger("Direction"))
			AudioManager.Play("funhouse_jackbox_shoot")
			Me.emitAudioFromObject.Add("funhouse_jackbox_shoot")
		End If
	End Sub

	' Token: 0x06003411 RID: 13329 RVA: 0x001E34F8 File Offset: 0x001E18F8
	Private Iterator Function slide_in(sprite As Transform, direction As Vector3) As IEnumerator
		Dim startPos As Vector3 = sprite.transform.position + -direction * 100F
		Dim endPos As Vector3 = sprite.transform.position
		sprite.transform.position = startPos
		Dim t As Single = 0F
		Dim time As Single = 1F
		sprite.GetComponent(Of SpriteRenderer)().enabled = True
		While t < time
			t += CupheadTime.Delta
			Dim val As Single = EaseUtils.Ease(EaseUtils.EaseType.easeInOutBounce, 0F, 1F, t / time)
			sprite.transform.position = Vector3.Lerp(startPos, endPos, val)
			Yield Nothing
		End While
		AudioManager.Play("funhouse_jackbox_shoot_launch")
		Me.emitAudioFromObject.Add("funhouse_jackbox_shoot_launch")
		Yield Nothing
		Return
	End Function

	' Token: 0x06003412 RID: 13330 RVA: 0x001E3524 File Offset: 0x001E1924
	Private Function DieTime() As Single
		Return If((Not Me.justDied), MyBase.Properties.jackinAppearDelay, MyBase.Properties.jackinDeathAppearDelay)
	End Function

	' Token: 0x06003413 RID: 13331 RVA: 0x001E355E File Offset: 0x001E195E
	Protected Overrides Sub Die()
		Me.justDied = True
	End Sub

	' Token: 0x06003414 RID: 13332 RVA: 0x001E3567 File Offset: 0x001E1967
	Private Sub SoundJackInBoxHeadPop()
		AudioManager.Play("funhouse_jackbox_jack_head")
		Me.emitAudioFromObject.Add("funhouse_jackbox_jack_head")
	End Sub

	' Token: 0x06003415 RID: 13333 RVA: 0x001E3583 File Offset: 0x001E1983
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me.projectile = Nothing
	End Sub

	' Token: 0x04003C4D RID: 15437
	<SerializeField()>
	Private projectile As FunhousePlatformingLevelJackinBoxProjectile

	' Token: 0x04003C4E RID: 15438
	<SerializeField()>
	Private jack As GameObject

	' Token: 0x04003C4F RID: 15439
	<SerializeField()>
	Private jackRoot As Transform

	' Token: 0x04003C50 RID: 15440
	<SerializeField()>
	Private topRoot As Transform

	' Token: 0x04003C51 RID: 15441
	<SerializeField()>
	Private bottomRoot As Transform

	' Token: 0x04003C52 RID: 15442
	<SerializeField()>
	Private rightRoot As Transform

	' Token: 0x04003C53 RID: 15443
	<SerializeField()>
	Private leftRoot As Transform

	' Token: 0x04003C54 RID: 15444
	<SerializeField()>
	Private top As Transform

	' Token: 0x04003C55 RID: 15445
	<SerializeField()>
	Private bottom As Transform

	' Token: 0x04003C56 RID: 15446
	<SerializeField()>
	Private right As Transform

	' Token: 0x04003C57 RID: 15447
	<SerializeField()>
	Private left As Transform

	' Token: 0x04003C58 RID: 15448
	Private directionIndex As Integer

	' Token: 0x04003C59 RID: 15449
	Private justDied As Boolean

	' Token: 0x04003C5A RID: 15450
	Private shootTime As Boolean

	' Token: 0x04003C5B RID: 15451
	Private offset As Single = 50F

	' Token: 0x04003C5C RID: 15452
	Private player As AbstractPlayerController
End Class
