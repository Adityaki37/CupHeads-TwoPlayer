Imports System
Imports UnityEngine

' Token: 0x020006FD RID: 1789
Public Class OldManLevelBeardController
	Inherits MonoBehaviour

	' Token: 0x0600264E RID: 9806 RVA: 0x00165E60 File Offset: 0x00164260
	Public Sub CueRuffle(which As Integer)
		If Me.ruffles(which) IsNot Nothing AndAlso Me.ruffles(which).GetCurrentAnimatorStateInfo(0).IsName("None") Then
			Me.ruffleCued(which) = True
		End If
	End Sub

	' Token: 0x0600264F RID: 9807 RVA: 0x00165EA9 File Offset: 0x001642A9
	Private Sub FixedUpdate()
		Me.frameTimer += CupheadTime.FixedDelta
		If Me.frameTimer > 0.041666668F Then
			Me.frameTimer -= 0.041666668F
			Me.[Step]()
		End If
	End Sub

	' Token: 0x06002650 RID: 9808 RVA: 0x00165EE5 File Offset: 0x001642E5
	Private Sub [Step]()
		Me.frameNum = (Me.frameNum + 1) Mod 6
		Me.rend.sprite = Me.sprites(Me.frameNum / 2)
	End Sub

	' Token: 0x06002651 RID: 9809 RVA: 0x00165F14 File Offset: 0x00164314
	Private Sub LateUpdate()
		For i As Integer = 0 To Me.ruffleCued.Length - 1
			If Me.ruffleCued(i) Then
				Dim num As Integer = Me.ruffleStartFrames(i) - Me.frameNum
				If num = 0 OrElse num = 1 Then
					Dim num2 As Single = Me.frameTimer * 24F
					Dim num3 As Single = (CSng(num) + num2) * 0.071428575F
					Me.ruffles(i).Play("Ruffle", 0, num3)
					Me.ruffles(i).Update(0F)
					Me.ruffleCued(i) = False
				End If
			End If
		Next
	End Sub

	' Token: 0x04002ED4 RID: 11988
	Private Const RUFFLE_FRAME_TIME_NORMALIZED As Single = 0.071428575F

	' Token: 0x04002ED5 RID: 11989
	<SerializeField()>
	Private rend As SpriteRenderer

	' Token: 0x04002ED6 RID: 11990
	<SerializeField()>
	Private sprites As Sprite()

	' Token: 0x04002ED7 RID: 11991
	<SerializeField()>
	Private ruffles As Animator()

	' Token: 0x04002ED8 RID: 11992
	<SerializeField()>
	Private ruffleStartFrames As Integer()

	' Token: 0x04002ED9 RID: 11993
	Private ruffleCued As Boolean() = New Boolean(9) {}

	' Token: 0x04002EDA RID: 11994
	Private frameTimer As Single

	' Token: 0x04002EDB RID: 11995
	Private frameNum As Integer
End Class
