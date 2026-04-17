Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020007F0 RID: 2032
Public Class SnowCultLevelPlatform
	Inherits LevelPlatform

	' Token: 0x06002EA4 RID: 11940 RVA: 0x001B81CE File Offset: 0x001B65CE
	Public Sub SetID(value As Integer)
		MyBase.animator.SetInteger("ID", value Mod 5)
	End Sub

	' Token: 0x06002EA5 RID: 11941 RVA: 0x001B81E4 File Offset: 0x001B65E4
	Public Sub StartRotate(angle As Single, pivotPoint As Vector3, loopSizeX As Single, loopSizeY As Single, speed As Single, pathOffset As Single, isClockwise As Boolean)
		Me.angle = angle
		Me.pivotPos = pivotPoint
		Me.speed = speed
		Me.loopSizeX = loopSizeX
		Me.loopSizeY = loopSizeY
		Me.isClockwise = isClockwise
		MyBase.StartCoroutine(Me.move_rotating_platforms_cr())
		MyBase.StartCoroutine(Me.sheen_cr())
	End Sub

	' Token: 0x06002EA6 RID: 11942 RVA: 0x001B8238 File Offset: 0x001B6638
	Private Iterator Function move_rotating_platforms_cr() As IEnumerator
		Dim handleRotationX As Vector3 = Vector3.zero
		If Me.angle = 0F OrElse Me.angle = 180F Then
			MyBase.transform.parent.position = Me.pivotPos + MathUtils.AngleToDirection(Me.angle) * Me.loopSizeX
		Else
			MyBase.transform.parent.position = Me.pivotPos + MathUtils.AngleToDirection(Me.angle) * Me.loopSizeY
		End If
		Me.angle *= 0.017453292F
		While True
			Me.angle += Me.speed * CupheadTime.FixedDelta
			If Me.isClockwise Then
				handleRotationX = New Vector3(Mathf.Sin(Me.angle) * Me.loopSizeX, 0F, 0F)
			Else
				handleRotationX = New Vector3(-Mathf.Sin(Me.angle) * Me.loopSizeX, 0F, 0F)
			End If
			Dim handleRotationY As Vector3 = New Vector3(0F, Mathf.Cos(Me.angle) * Me.loopSizeY, 0F)
			MyBase.transform.parent.position = Me.pivotPos
			MyBase.transform.parent.position += handleRotationX + handleRotationY
			Yield New WaitForFixedUpdate()
		End While
		Return
	End Function

	' Token: 0x06002EA7 RID: 11943 RVA: 0x001B8254 File Offset: 0x001B6654
	Private Iterator Function sheen_cr() As IEnumerator
		While True
			Yield CupheadTime.WaitForSeconds(Me, Global.UnityEngine.Random.Range(Me.sheenTimeMin, Me.sheenTimeMax))
			MyBase.animator.SetTrigger("Sheen")
			While Not MyBase.animator.GetCurrentAnimatorStateInfo(0).IsTag("Sheen")
				Yield Nothing
			End While
			While MyBase.animator.GetCurrentAnimatorStateInfo(0).IsTag("Sheen")
				Yield Nothing
			End While
		End While
		Return
	End Function

	' Token: 0x06002EA8 RID: 11944 RVA: 0x001B8270 File Offset: 0x001B6670
	Private Sub FixedUpdate()
		Me.player1 = PlayerManager.GetPlayer(PlayerId.PlayerOne)
		Me.player2 = PlayerManager.GetPlayer(PlayerId.PlayerTwo)
		If Me.player1 IsNot Nothing Then
			Me.p1IsColliding = Me.player1.transform.parent Is MyBase.transform
			Me.player1.transform.SetEulerAngles(Nothing, Nothing, New Single?(0F))
		Else
			Me.p1IsColliding = False
		End If
		If Me.player2 IsNot Nothing Then
			Me.p2IsColliding = Me.player2.transform.parent Is MyBase.transform
			Me.player2.transform.SetEulerAngles(Nothing, Nothing, New Single?(0F))
		Else
			Me.p2IsColliding = False
		End If
		MyBase.transform.localPosition = Vector3.Lerp(MyBase.transform.localPosition, New Vector3(0F, If((Not Me.p1IsColliding AndAlso Not Me.p2IsColliding), 0F, Me.downDist)), Me.bounceSpeed * CupheadTime.FixedDelta)
	End Sub

	' Token: 0x0400373E RID: 14142
	<SerializeField()>
	Private downDist As Single = -30F

	' Token: 0x0400373F RID: 14143
	<SerializeField()>
	Private bounceSpeed As Single = 20F

	' Token: 0x04003740 RID: 14144
	Private pivotPos As Vector3

	' Token: 0x04003741 RID: 14145
	Private isClockwise As Boolean

	' Token: 0x04003742 RID: 14146
	Private angle As Single

	' Token: 0x04003743 RID: 14147
	Private speed As Single

	' Token: 0x04003744 RID: 14148
	Private loopSizeX As Single

	' Token: 0x04003745 RID: 14149
	Private loopSizeY As Single

	' Token: 0x04003746 RID: 14150
	Private player1 As AbstractPlayerController

	' Token: 0x04003747 RID: 14151
	Private player2 As AbstractPlayerController

	' Token: 0x04003748 RID: 14152
	Private p1IsColliding As Boolean

	' Token: 0x04003749 RID: 14153
	Private p2IsColliding As Boolean

	' Token: 0x0400374A RID: 14154
	<SerializeField()>
	Private sheenTimeMin As Single = 1F

	' Token: 0x0400374B RID: 14155
	<SerializeField()>
	Private sheenTimeMax As Single = 5F
End Class
