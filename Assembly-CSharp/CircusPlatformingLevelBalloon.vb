Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x0200089A RID: 2202
Public Class CircusPlatformingLevelBalloon
	Inherits AbstractPlatformingLevelEnemy

	' Token: 0x0600333C RID: 13116 RVA: 0x001DD41A File Offset: 0x001DB81A
	Protected Overrides Sub OnStart()
	End Sub

	' Token: 0x0600333D RID: 13117 RVA: 0x001DD41C File Offset: 0x001DB81C
	Public Sub Init(pos As Vector2, rotation As Single, spreadCount As String, c As String)
		MyBase.transform.position = pos
		Me.rotation = rotation
		Me.spreadCount = spreadCount
		Me.SetColor(c)
	End Sub

	' Token: 0x0600333E RID: 13118 RVA: 0x001DD448 File Offset: 0x001DB848
	Protected Overrides Sub Start()
		MyBase.Start()
		MyBase.StartCoroutine(Me.movement_cr())
		MyBase.StartCoroutine(Me.idle_audio_delayer_cr(Me.idleSoundSelected, 2F, 4F))
		Me.emitAudioFromObject.Add(Me.idleSoundSelected)
	End Sub

	' Token: 0x0600333F RID: 13119 RVA: 0x001DD498 File Offset: 0x001DB898
	Private Sub SetColor(c As String)
		If c IsNot Nothing Then
			If Not(c = "B") Then
				If Not(c = "G") Then
					If c = "P" Then
						MyBase.animator.Play("PinkIdle")
						Me.idleSoundSelected = "circus_balloon_girl_idle"
						Me._canParry = True
					End If
				Else
					MyBase.animator.Play("GirlIdle")
					Me.idleSoundSelected = "circus_balloon_girl_idle"
				End If
			Else
				MyBase.animator.Play("BoyIdle")
				Me.idleSoundSelected = "circus_balloon_boy_idle"
			End If
		End If
	End Sub

	' Token: 0x06003340 RID: 13120 RVA: 0x001DD548 File Offset: 0x001DB948
	Private Iterator Function movement_cr() As IEnumerator
		Dim angle As Single = 0F
		Dim xVelocity As Vector3 = Vector3.zero
		While True
			angle += MyBase.Properties.flyingFishSinVelocity * CupheadTime.Delta
			xVelocity = If((Me.rotation <> 180F), MyBase.transform.right, (-MyBase.transform.right))
			Dim moveY As Vector3 = New Vector3(0F, Mathf.Sin(angle) * CupheadTime.Delta * 60F * MyBase.Properties.flyingFishSinSize)
			Dim moveX As Vector3 = xVelocity * MyBase.Properties.flyingFishVelocity * CupheadTime.Delta
			If CupheadTime.Delta IsNot 0F Then
				Dim vector As Vector3 = MyBase.transform.position + moveX + moveY
				vector.z = -vector.x
				MyBase.transform.position = vector
			End If
			If MyBase.transform.position.x < CSng((Level.Current.Left - 150)) Then
				Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
			End If
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06003341 RID: 13121 RVA: 0x001DD563 File Offset: 0x001DB963
	Protected Overrides Sub Die()
		If MyBase.Health <= 0F Then
			AudioManager.Play("circus_balloon_hit")
			Me.emitAudioFromObject.Add("circus_balloon_hit")
			MyBase.animator.SetTrigger("Death")
		End If
	End Sub

	' Token: 0x06003342 RID: 13122 RVA: 0x001DD5A0 File Offset: 0x001DB9A0
	Public Sub ExplodeBalloon()
		Dim array As String() = Me.spreadCount.Split(New Char() { ","c })
		Dim num As Single = 0F
		For i As Integer = 0 To array.Length - 1
			Parser.FloatTryParse(array(i), num)
			Me.SpawnBullet(num)
		Next
	End Sub

	' Token: 0x06003343 RID: 13123 RVA: 0x001DD5F0 File Offset: 0x001DB9F0
	Public Sub OnEndDeathAnim()
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub

	' Token: 0x06003344 RID: 13124 RVA: 0x001DD5FD File Offset: 0x001DB9FD
	Private Sub SpawnBullet(angle As Single)
		Me.projectile.Create(MyBase.transform.position, angle, Me.bulletSpeed)
	End Sub

	' Token: 0x06003345 RID: 13125 RVA: 0x001DD622 File Offset: 0x001DBA22
	Private Sub SoundBalloonDeathAnim()
		AudioManager.Play("circus_balloon_death")
		Me.emitAudioFromObject.Add("circus_balloon_death")
	End Sub

	' Token: 0x06003346 RID: 13126 RVA: 0x001DD63E File Offset: 0x001DBA3E
	Public Overrides Sub OnParry(player As AbstractPlayerController)
		MyBase.OnParry(player)
		Me._canParry = False
		MyBase.StartCoroutine(Me.parryCooldown_cr())
	End Sub

	' Token: 0x06003347 RID: 13127 RVA: 0x001DD65C File Offset: 0x001DBA5C
	Private Iterator Function parryCooldown_cr() As IEnumerator
		Dim t As Single = 0F
		While t < Me.coolDown
			t += CupheadTime.Delta
			Yield Nothing
		End While
		Me._canParry = True
		Yield Nothing
		Return
	End Function

	' Token: 0x04003B7C RID: 15228
	Private Const Blue As String = "B"

	' Token: 0x04003B7D RID: 15229
	Private Const Green As String = "G"

	' Token: 0x04003B7E RID: 15230
	Private Const Pink As String = "P"

	' Token: 0x04003B7F RID: 15231
	Private Const DeathParameterName As String = "Death"

	' Token: 0x04003B80 RID: 15232
	Private Const BoyIdle As String = "BoyIdle"

	' Token: 0x04003B81 RID: 15233
	Private Const GirlIdle As String = "GirlIdle"

	' Token: 0x04003B82 RID: 15234
	Private Const PinkIdle As String = "PinkIdle"

	' Token: 0x04003B83 RID: 15235
	Private Const BoyIdleSound As String = "circus_balloon_boy_idle"

	' Token: 0x04003B84 RID: 15236
	Private Const GirlIdleSound As String = "circus_balloon_girl_idle"

	' Token: 0x04003B85 RID: 15237
	<SerializeField()>
	Private bulletSpeed As Single

	' Token: 0x04003B86 RID: 15238
	<SerializeField()>
	Private projectile As BasicProjectile

	' Token: 0x04003B87 RID: 15239
	<SerializeField()>
	Private coolDown As Single = 0.4F

	' Token: 0x04003B88 RID: 15240
	Private rotation As Single

	' Token: 0x04003B89 RID: 15241
	Private spreadCount As String

	' Token: 0x04003B8A RID: 15242
	Private idleSoundSelected As String
End Class
