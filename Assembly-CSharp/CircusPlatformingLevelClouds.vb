Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020008A1 RID: 2209
Public Class CircusPlatformingLevelClouds
	Inherits AbstractPausableComponent

	' Token: 0x06003366 RID: 13158 RVA: 0x001DE6F0 File Offset: 0x001DCAF0
	Private Sub Start()
		MyBase.StartCoroutine(Me.change_y_axis())
	End Sub

	' Token: 0x06003367 RID: 13159 RVA: 0x001DE700 File Offset: 0x001DCB00
	Private Iterator Function change_y_axis() As IEnumerator
		Dim cloudStartPositionsX As Single() = New Single(Me.cloudPieces.Length - 1) {}
		Dim cloudStartSpeedX As Single() = New Single(Me.cloudPieces.Length - 1) {}
		For j As Integer = 0 To Me.cloudPieces.Length - 1
			cloudStartPositionsX(j) = Me.cloudPieces(j).cloud.position.y
			For Each scrollingSprite As ScrollingSprite In Me.cloudPieces(j).cloud.GetComponentsInChildren(Of ScrollingSprite)()
				cloudStartSpeedX(j) = scrollingSprite.speed
			Next
		Next
		While True
			For i As Integer = 0 To Me.cloudPieces.Length - 1
				Me.cloudPieces(i).cloud.SetPosition(Nothing, New Single?(Mathf.Lerp(cloudStartPositionsX(i), Me.cloudPieces(i).cloudEndY, Me.RelativePosition(Me.cloudPieces(i).cameraRelativePosX))), Nothing)
				If CupheadLevelCamera.Current.transform.position <> Me.lastPosition Then
					If Me.cloudPieces(i).cloud.GetComponent(Of PlatformingLevelParallax)() Then
						Me.cloudPieces(i).cloud.GetComponent(Of PlatformingLevelParallax)().enabled = False
					End If
					For Each scrollingSprite2 As ScrollingSprite In Me.cloudPieces(i).cloud.GetComponentsInChildren(Of ScrollingSprite)()
						If CupheadLevelCamera.Current.transform.position.x < Me.lastPosition.x Then
							If scrollingSprite2.speed > cloudStartSpeedX(i) Then
								scrollingSprite2.speed -= Me.incrementAmount
							End If
						ElseIf scrollingSprite2.speed < cloudStartSpeedX(i) * Me.cloudPieces(i).speedMultiplyAmount Then
							scrollingSprite2.speed += Me.incrementAmount
						End If
					Next
					Me.cloudPieces(i).UpdateCurrentRelativePos(Me.RelativePosition(Me.cloudPieces(i).cameraRelativePosX))
					Me.lastPosition = CupheadLevelCamera.Current.transform.position
					Yield Nothing
				Else
					If Me.cloudPieces(i).cloud.GetComponent(Of PlatformingLevelParallax)() Then
						Me.cloudPieces(i).cloud.GetComponent(Of PlatformingLevelParallax)().enabled = True
						Me.cloudPieces(i).cloud.GetComponent(Of PlatformingLevelParallax)().UpdateBasePosition()
					End If
					For Each scrollingSprite3 As ScrollingSprite In Me.cloudPieces(i).cloud.GetComponentsInChildren(Of ScrollingSprite)()
						If scrollingSprite3.speed > cloudStartSpeedX(i) Then
							scrollingSprite3.speed -= Me.incrementAmount
						Else
							scrollingSprite3.speed = cloudStartSpeedX(i)
						End If
					Next
					Yield Nothing
				End If
			Next
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06003368 RID: 13160 RVA: 0x001DE71C File Offset: 0x001DCB1C
	Private Function RelativePosition(relativePosX As Single) As Single
		Dim num As Single = relativePosX - CSng(Level.Current.Left)
		Dim num2 As Single = CupheadLevelCamera.Current.transform.position.x - CSng(Level.Current.Left)
		Return num2 / num
	End Function

	' Token: 0x04003BB4 RID: 15284
	<SerializeField()>
	Private cloudPieces As CircusPlatformingLevelClouds.CloudPiece()

	' Token: 0x04003BB5 RID: 15285
	Private lastPosition As Vector3

	' Token: 0x04003BB6 RID: 15286
	<SerializeField()>
	Private incrementAmount As Single = 2F

	' Token: 0x020008A2 RID: 2210
	<Serializable()>
	Public Class CloudPiece
		' Token: 0x0600336A RID: 13162 RVA: 0x001DE768 File Offset: 0x001DCB68
		Public Sub UpdateCurrentRelativePos(pos As Single)
			Me.currentRelativePosX = pos
		End Sub

		' Token: 0x0600336B RID: 13163 RVA: 0x001DE771 File Offset: 0x001DCB71
		Public Function CurrentRelativePosX() As Single
			Return Me.currentRelativePosX
		End Function

		' Token: 0x04003BB7 RID: 15287
		Public cloud As Transform

		' Token: 0x04003BB8 RID: 15288
		Public cloudEndY As Single

		' Token: 0x04003BB9 RID: 15289
		Public cameraRelativePosX As Single

		' Token: 0x04003BBA RID: 15290
		Public speedMultiplyAmount As Single

		' Token: 0x04003BBB RID: 15291
		Private currentRelativePosX As Single
	End Class
End Class
