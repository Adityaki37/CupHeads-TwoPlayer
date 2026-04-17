Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020004E6 RID: 1254
Public Class BaronessLevelPlatform
	Inherits PlatformingLevelPlatformSag

	' Token: 0x060015BC RID: 5564 RVA: 0x000C3058 File Offset: 0x000C1458
	Public Sub getProperties(properties As LevelProperties.Baroness.Platform)
		Me.properties = properties
		Dim position As Vector3 = MyBase.transform.position
		position.y = properties.YPosition
		MyBase.transform.position = position
		Me.maxCounter = Global.UnityEngine.Random.Range(4, 9)
		MyBase.StartCoroutine(Me.move_cr())
	End Sub

	' Token: 0x060015BD RID: 5565 RVA: 0x000C30AC File Offset: 0x000C14AC
	Private Iterator Function move_cr() As IEnumerator
		Dim movingLeft As Boolean = True
		While True
			Dim pos As Vector3 = MyBase.transform.position
			If movingLeft Then
				If Me.castle.state = BaronessLevelCastle.State.Chase Then
					MyBase.animator.Play("Fast")
				End If
				pos.x = Mathf.MoveTowards(MyBase.transform.position.x, -640F + Me.properties.LeftBoundaryOffset, Me.speed * CupheadTime.Delta)
				movingLeft = MyBase.transform.position.x <> -640F + Me.properties.LeftBoundaryOffset
			Else
				If Me.castle.state = BaronessLevelCastle.State.Chase Then
					MyBase.animator.Play("Slow")
				End If
				pos.x = Mathf.MoveTowards(MyBase.transform.position.x, CSng(Level.Current.Right) - Me.properties.RightBoundaryOffset, Me.speed * CupheadTime.Delta)
				movingLeft = MyBase.transform.position.x = CSng(Level.Current.Right) - Me.properties.RightBoundaryOffset
			End If
			MyBase.transform.position = pos
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x060015BE RID: 5566 RVA: 0x000C30C8 File Offset: 0x000C14C8
	Private Sub SweatCounter()
		If Me.counter < Me.maxCounter Then
			Me.counter += 1
		Else
			MyBase.animator.Play("Sweat")
			Me.counter = 0
			Me.maxCounter = Global.UnityEngine.Random.Range(4, 9)
		End If
	End Sub

	' Token: 0x04001F10 RID: 7952
	<SerializeField()>
	Private castle As BaronessLevelCastle

	' Token: 0x04001F11 RID: 7953
	Private speed As Single = 200F

	' Token: 0x04001F12 RID: 7954
	Private counter As Integer

	' Token: 0x04001F13 RID: 7955
	Private maxCounter As Integer

	' Token: 0x04001F14 RID: 7956
	Private properties As LevelProperties.Baroness.Platform
End Class
