Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020007BC RID: 1980
Public Class SallyStagePlayLevelWindow
	Inherits AbstractCollidableObject

	' Token: 0x06002CC6 RID: 11462 RVA: 0x001A64C7 File Offset: 0x001A48C7
	Private Sub Start()
		Me.startPos = MyBase.transform.position
		MyBase.GetComponent(Of Collider2D)().enabled = False
		AddHandler MyBase.GetComponent(Of DamageReceiver)().OnDamageTaken, AddressOf Me.OnDamageTaken
	End Sub

	' Token: 0x06002CC7 RID: 11463 RVA: 0x001A64FD File Offset: 0x001A48FD
	Public Sub Init(pos As Vector2, parent As SallyStagePlayLevel)
		MyBase.transform.position = pos
		Me.parent = parent
	End Sub

	' Token: 0x06002CC8 RID: 11464 RVA: 0x001A6518 File Offset: 0x001A4918
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		Me.HP -= info.damage
		If Me.HP <= 0F AndAlso Not Me.isDying Then
			If Me.isBaby Then
				MyBase.StartCoroutine(Me.baby_slide_off())
			Else
				Me.NunDead()
			End If
		End If
	End Sub

	' Token: 0x06002CC9 RID: 11465 RVA: 0x001A6576 File Offset: 0x001A4976
	Public Sub WindowClosed()
		MyBase.animator.Play("Off")
	End Sub

	' Token: 0x06002CCA RID: 11466 RVA: 0x001A6588 File Offset: 0x001A4988
	Private Sub LeftWindow()
		MyBase.GetComponent(Of Collider2D)().enabled = False
	End Sub

	' Token: 0x06002CCB RID: 11467 RVA: 0x001A6598 File Offset: 0x001A4998
	Public Sub WindowOpenNun(properties As LevelProperties.SallyStagePlay, isPink As Boolean)
		Me.isBaby = False
		Me.speed = properties.CurrentState.nun.rulerSpeed
		Me.HP = CSng(properties.CurrentState.nun.HP)
		MyBase.GetComponent(Of SpriteRenderer)().enabled = True
		MyBase.GetComponent(Of Collider2D)().enabled = True
		Me.isPink = isPink
		For Each spriteRenderer As SpriteRenderer In Me.nunPink
			spriteRenderer.enabled = isPink
		Next
		MyBase.animator.Play("Window_Nun")
	End Sub

	' Token: 0x06002CCC RID: 11468 RVA: 0x001A6630 File Offset: 0x001A4A30
	Public Sub ShootRuler()
		Dim [next] As AbstractPlayerController = PlayerManager.GetNext()
		Dim vector As Vector3 = [next].transform.position - MyBase.transform.position
		Dim num As Single = MathUtils.DirectionToAngle(vector)
		Dim sallyStagePlayLevelWindowProjectile As SallyStagePlayLevelWindowProjectile = If((Not Me.isPink), Me.ruler, Me.rulerPink)
		sallyStagePlayLevelWindowProjectile.Create(MyBase.transform.position, num, Me.speed, Me.parent)
	End Sub

	' Token: 0x06002CCD RID: 11469 RVA: 0x001A66AC File Offset: 0x001A4AAC
	Public Sub WindowOpenBaby(properties As LevelProperties.SallyStagePlay)
		Me.isBaby = True
		Me.speed = properties.CurrentState.baby.bottleSpeed
		Me.HP = CSng(properties.CurrentState.baby.HP)
		MyBase.GetComponent(Of SpriteRenderer)().enabled = True
		MyBase.GetComponent(Of Collider2D)().enabled = True
		Dim text As String = If((Not Rand.Bool()), "_Boy", "_Girl")
		MyBase.animator.Play("Window_Baby" + text)
	End Sub

	' Token: 0x06002CCE RID: 11470 RVA: 0x001A6738 File Offset: 0x001A4B38
	Private Sub ShootBottle()
		Dim vector As Vector3 = New Vector3(MyBase.transform.position.x, -360F, 0F) - MyBase.transform.position
		Dim num As Single = MathUtils.DirectionToAngle(vector)
		Me.bottle.Create(New Vector2(MyBase.transform.position.x, MyBase.transform.position.y - 30F), num, Me.speed, Me.parent)
	End Sub

	' Token: 0x06002CCF RID: 11471 RVA: 0x001A67D0 File Offset: 0x001A4BD0
	Private Iterator Function baby_slide_off() As IEnumerator
		Me.isDying = True
		MyBase.animator.SetTrigger("Dead")
		MyBase.GetComponent(Of Collider2D)().enabled = False
		Yield CupheadTime.WaitForSeconds(Me, 1.5F)
		Dim start As Vector3 = MyBase.transform.position
		Dim [end] As Vector3 = New Vector3(MyBase.transform.position.x, MyBase.transform.position.y - 50F)
		Dim t As Single = 0F
		Dim time As Single = 0.1F
		While t < time
			t += CupheadTime.Delta
			MyBase.transform.position = Vector3.Lerp(start, [end], t / time)
			Yield Nothing
		End While
		Yield Nothing
		Me.isDying = False
		MyBase.GetComponent(Of SpriteRenderer)().enabled = False
		MyBase.animator.Play("Off")
		MyBase.transform.position = Me.startPos
		Yield Nothing
		Return
	End Function

	' Token: 0x06002CD0 RID: 11472 RVA: 0x001A67EB File Offset: 0x001A4BEB
	Private Sub NunDead()
		MyBase.animator.SetTrigger("Dead")
		MyBase.GetComponent(Of Collider2D)().enabled = False
	End Sub

	' Token: 0x06002CD1 RID: 11473 RVA: 0x001A6809 File Offset: 0x001A4C09
	Private Sub SoundBabyBoy()
		AudioManager.Play("sally_baby_boy")
		Me.emitAudioFromObject.Add("sally_baby_boy")
	End Sub

	' Token: 0x06002CD2 RID: 11474 RVA: 0x001A6825 File Offset: 0x001A4C25
	Private Sub SoundBabyGirl()
		AudioManager.Play("sally_baby_girl")
		Me.emitAudioFromObject.Add("sally_baby_girl")
	End Sub

	' Token: 0x06002CD3 RID: 11475 RVA: 0x001A6841 File Offset: 0x001A4C41
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me.rulerPink = Nothing
		Me.ruler = Nothing
		Me.bottle = Nothing
	End Sub

	' Token: 0x04003540 RID: 13632
	<SerializeField()>
	Private projectileRoot As Transform

	' Token: 0x04003541 RID: 13633
	<SerializeField()>
	Private rulerPink As SallyStagePlayLevelWindowProjectile

	' Token: 0x04003542 RID: 13634
	<SerializeField()>
	Private ruler As SallyStagePlayLevelWindowProjectile

	' Token: 0x04003543 RID: 13635
	<SerializeField()>
	Private bottle As SallyStagePlayLevelWindowProjectile

	' Token: 0x04003544 RID: 13636
	<SerializeField()>
	Private nunPink As SpriteRenderer()

	' Token: 0x04003545 RID: 13637
	Public windowNum As Integer

	' Token: 0x04003546 RID: 13638
	Private properties As LevelProperties.SallyStagePlay

	' Token: 0x04003547 RID: 13639
	Private parent As SallyStagePlayLevel

	' Token: 0x04003548 RID: 13640
	Private startPos As Vector3

	' Token: 0x04003549 RID: 13641
	Private speed As Single

	' Token: 0x0400354A RID: 13642
	Private HP As Single

	' Token: 0x0400354B RID: 13643
	Private isDying As Boolean

	' Token: 0x0400354C RID: 13644
	Private isBaby As Boolean = True

	' Token: 0x0400354D RID: 13645
	Private isPink As Boolean
End Class
