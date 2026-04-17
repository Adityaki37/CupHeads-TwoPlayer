Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020008D9 RID: 2265
Public Class MountainPlatformingLevelCyclopsBG
	Inherits AbstractPausableComponent

	' Token: 0x1700044E RID: 1102
	' (get) Token: 0x060034FE RID: 13566 RVA: 0x001ECF9B File Offset: 0x001EB39B
	' (set) Token: 0x060034FF RID: 13567 RVA: 0x001ECFA3 File Offset: 0x001EB3A3
	Public Property isWalking As Boolean

	' Token: 0x06003500 RID: 13568 RVA: 0x001ECFAC File Offset: 0x001EB3AC
	Private Sub Start()
		Me.sizeY = MyBase.GetComponent(Of SpriteRenderer)().bounds.size.y
		Me.blinkCounterMax = Global.UnityEngine.Random.Range(3, 6)
		Me.eye.enabled = False
	End Sub

	' Token: 0x06003501 RID: 13569 RVA: 0x001ECFF4 File Offset: 0x001EB3F4
	Private Sub OnTurn()
		MyBase.transform.SetScale(New Single?(-MyBase.transform.localScale.x), Nothing, Nothing)
		If MyBase.transform.localScale.x = 1F Then
			MyBase.transform.AddPosition(-47F, 0F, 0F)
		Else
			MyBase.transform.AddPosition(47F, 0F, 0F)
		End If
	End Sub

	' Token: 0x06003502 RID: 13570 RVA: 0x001ED08D File Offset: 0x001EB48D
	Private Sub DropRocks()
		MyBase.StartCoroutine(Me.drop_rocks_cr())
	End Sub

	' Token: 0x06003503 RID: 13571 RVA: 0x001ED09C File Offset: 0x001EB49C
	Private Sub StopWalking()
		Me.isWalking = False
	End Sub

	' Token: 0x06003504 RID: 13572 RVA: 0x001ED0A5 File Offset: 0x001EB4A5
	Private Sub StartWalking()
		Me.isWalking = True
	End Sub

	' Token: 0x06003505 RID: 13573 RVA: 0x001ED0AE File Offset: 0x001EB4AE
	Public Sub GetPlayer(player As AbstractPlayerController)
		Me.player = player
	End Sub

	' Token: 0x06003506 RID: 13574 RVA: 0x001ED0B8 File Offset: 0x001EB4B8
	Private Iterator Function drop_rocks_cr() As IEnumerator
		For i As Integer = 0 To Me.rockCount - 1
			Dim zero As Vector2 = Vector2.zero
			zero.y = CupheadLevelCamera.Current.Bounds.yMax + 200F
			zero.x = CupheadLevelCamera.Current.Bounds.xMin + Me.projectile.GetComponent(Of Renderer)().bounds.size.x / 2F + Me.projectile.GetComponent(Of Renderer)().bounds.size.x * CSng(i)
			Me.projectile.Create(MyBase.transform.position, zero, Me.rockSpeed, Me.rockDelay * CSng(i))
		Next
		Yield Nothing
		Return
	End Function

	' Token: 0x06003507 RID: 13575 RVA: 0x001ED0D3 File Offset: 0x001EB4D3
	Private Sub SlideUp()
		If Not Me.isDead Then
			MyBase.StartCoroutine(Me.slide_up_cr())
		End If
	End Sub

	' Token: 0x06003508 RID: 13576 RVA: 0x001ED0ED File Offset: 0x001EB4ED
	Private Sub SlideDown()
		MyBase.StartCoroutine(Me.slide_down_cr())
	End Sub

	' Token: 0x06003509 RID: 13577 RVA: 0x001ED0FC File Offset: 0x001EB4FC
	Private Iterator Function slide_up_cr() As IEnumerator
		Me.player = PlayerManager.GetNext()
		MyBase.transform.SetPosition(New Single?(Me.player.transform.position.x), Nothing, Nothing)
		Dim t As Single = 0F
		Dim time As Single = 0.4F
		Dim startPos As Single = MyBase.transform.position.y
		Dim endPos As Single = Me.start.y
		While t < time
			t += CupheadTime.Delta
			MyBase.transform.SetPosition(Nothing, New Single?(Mathf.Lerp(startPos, endPos, t / time)), Nothing)
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x0600350A RID: 13578 RVA: 0x001ED118 File Offset: 0x001EB518
	Private Iterator Function slide_down_cr() As IEnumerator
		Dim t As Single = 0F
		Dim time As Single = 0.8F
		Dim startPos As Single = MyBase.transform.position.y
		Dim endPos As Single = Me.start.y - Me.sizeY
		While t < time
			t += CupheadTime.Delta
			MyBase.transform.SetPosition(Nothing, New Single?(Mathf.Lerp(startPos, endPos, t / time)), Nothing)
			Yield Nothing
		End While
		If Me.isDead Then
			MyBase.gameObject.SetActive(False)
		Else
			Yield CupheadTime.WaitForSeconds(Me, 0.8F)
			MyBase.animator.SetTrigger("Continue")
		End If
		Yield Nothing
		Return
	End Function

	' Token: 0x0600350B RID: 13579 RVA: 0x001ED134 File Offset: 0x001EB534
	Private Sub BlinkMaybe()
		If Me.blinkCounter < Me.blinkCounterMax Then
			Me.eye.enabled = False
			Me.blinkCounter += 1
		Else
			Me.eye.enabled = True
			Me.blinkCounter = 0
			Me.blinkCounterMax = Global.UnityEngine.Random.Range(3, 6)
		End If
	End Sub

	' Token: 0x0600350C RID: 13580 RVA: 0x001ED191 File Offset: 0x001EB591
	Private Sub SoundGiantRockThrow()
		AudioManager.Play("castle_giant_rock_throw")
		Me.emitAudioFromObject.Add("castle_giant_rock_throw")
	End Sub

	' Token: 0x0600350D RID: 13581 RVA: 0x001ED1AD File Offset: 0x001EB5AD
	Private Sub SoundGiantRockThrowAppear()
		AudioManager.Play("castle_giant_rock_throw_appear")
		Me.emitAudioFromObject.Add("castle_giant_rock_throw_appear")
	End Sub

	' Token: 0x0600350E RID: 13582 RVA: 0x001ED1C9 File Offset: 0x001EB5C9
	Private Sub SoundGiantStartle()
		AudioManager.Play("castle_giant_startle")
		Me.emitAudioFromObject.Add("castle_giant_startle")
	End Sub

	' Token: 0x04003D26 RID: 15654
	<SerializeField()>
	Private eye As SpriteRenderer

	' Token: 0x04003D27 RID: 15655
	<SerializeField()>
	Private rockDelay As Single

	' Token: 0x04003D28 RID: 15656
	<SerializeField()>
	Private rockSpeed As Single

	' Token: 0x04003D29 RID: 15657
	<SerializeField()>
	Private rockCount As Integer

	' Token: 0x04003D2A RID: 15658
	<SerializeField()>
	Private projectile As MountainPlatformingLevelRock

	' Token: 0x04003D2B RID: 15659
	Private blinkCounter As Integer

	' Token: 0x04003D2C RID: 15660
	Private blinkCounterMax As Integer

	' Token: 0x04003D2D RID: 15661
	Private sizeY As Single

	' Token: 0x04003D2E RID: 15662
	Private isAltPattern As Boolean

	' Token: 0x04003D2F RID: 15663
	Private player As AbstractPlayerController

	' Token: 0x04003D30 RID: 15664
	Public start As Vector3

	' Token: 0x04003D32 RID: 15666
	Public isDead As Boolean
End Class
