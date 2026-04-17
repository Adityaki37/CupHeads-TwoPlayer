Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020008C7 RID: 2247
Public Class HarbourPlatformingLevelFish
	Inherits AbstractPlatformingLevelEnemy

	' Token: 0x06003485 RID: 13445 RVA: 0x001E7F1D File Offset: 0x001E631D
	Public Sub Init(pos As Vector2, rotation As Single, type As String)
		MyBase.transform.position = pos
		Me.type = type
		Me.rotation = rotation
	End Sub

	' Token: 0x06003486 RID: 13446 RVA: 0x001E7F3E File Offset: 0x001E633E
	Protected Overrides Sub OnStart()
	End Sub

	' Token: 0x06003487 RID: 13447 RVA: 0x001E7F40 File Offset: 0x001E6340
	Protected Overrides Sub Start()
		Me.blinkLayer.enabled = False
		Me.blinkCounterMax = Global.UnityEngine.Random.Range(10, 20)
		MyBase.transform.SetScale(New Single?(If((Me.rotation <> 180F), (-MyBase.transform.localScale.x), MyBase.transform.localScale.x)), Nothing, Nothing)
		For i As Integer = 0 To 5 - 1
			If Me.type.Substring(0, 1) = Me.letters.Split(New Char() { ","c })(i) Then
				Me.num = i + 1
			End If
		Next
		Me.isA = Me.type.Substring(1, 1) = "A"
		MyBase.animator.SetInteger("Type", Me.num)
		MyBase.animator.SetBool("Is1", Me.isA)
		If Me.num = 4 Then
			Me._canParry = True
		End If
		MyBase.StartCoroutine(Me.movement_cr())
		MyBase.Start()
	End Sub

	' Token: 0x06003488 RID: 13448 RVA: 0x001E808B File Offset: 0x001E648B
	Private Sub FlyingFishSFX()
		AudioManager.Play("harbour_flying_fish_idle")
		Me.emitAudioFromObject.Add("harbour_flying_fish_idle")
	End Sub

	' Token: 0x06003489 RID: 13449 RVA: 0x001E80A8 File Offset: 0x001E64A8
	Private Iterator Function movement_cr() As IEnumerator
		Dim angle As Single = 0F
		Dim xVelocity As Vector3 = Vector3.zero
		While True
			angle += MyBase.Properties.flyingFishSinVelocity * CupheadTime.Delta
			xVelocity = If((Me.rotation <> 180F), MyBase.transform.right, (-MyBase.transform.right))
			Dim moveY As Vector3 = New Vector3(0F, Mathf.Sin(angle) * CupheadTime.Delta * 60F * MyBase.Properties.flyingFishSinSize)
			Dim moveX As Vector3 = xVelocity * MyBase.Properties.flyingFishVelocity * CupheadTime.Delta
			If CupheadTime.Delta IsNot 0F Then
				MyBase.transform.position += moveX + moveY
			End If
			If Not CupheadLevelCamera.Current.ContainsPoint(MyBase.transform.position, AbstractPlatformingLevelEnemy.CAMERA_DEATH_PADDING) Then
				AudioManager.[Stop]("harbour_flying_fish_idle")
				Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
			End If
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x0600348A RID: 13450 RVA: 0x001E80C4 File Offset: 0x001E64C4
	Private Sub IncrementBlinkCounter()
		Me.FlyingFishSFX()
		If Me.blinkCounter < Me.blinkCounterMax Then
			Me.blinkLayer.enabled = False
			Me.blinkCounter += 1
		Else
			Me.blinkLayer.enabled = True
			Me.blinkCounter = 0
			Me.blinkCounterMax = Global.UnityEngine.Random.Range(5, 10)
		End If
	End Sub

	' Token: 0x0600348B RID: 13451 RVA: 0x001E8128 File Offset: 0x001E6528
	Protected Overrides Sub Die()
		AudioManager.[Stop]("harbour_flying_fish_idle")
		AudioManager.Play("harbour_flying_fish_death")
		Me.emitAudioFromObject.Add("harbour_flying_fish_death")
		MyBase.Die()
	End Sub

	' Token: 0x04003CA8 RID: 15528
	<SerializeField()>
	Private blinkLayer As SpriteRenderer

	' Token: 0x04003CA9 RID: 15529
	Private letters As String = "A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P"

	' Token: 0x04003CAA RID: 15530
	Private type As String

	' Token: 0x04003CAB RID: 15531
	Private rotation As Single

	' Token: 0x04003CAC RID: 15532
	Private num As Integer

	' Token: 0x04003CAD RID: 15533
	Private isA As Boolean

	' Token: 0x04003CAE RID: 15534
	Private blinkCounter As Integer

	' Token: 0x04003CAF RID: 15535
	Private blinkCounterMax As Integer
End Class
