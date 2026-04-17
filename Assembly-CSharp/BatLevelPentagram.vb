Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x0200050A RID: 1290
Public Class BatLevelPentagram
	Inherits AbstractCollidableObject

	' Token: 0x060016E1 RID: 5857 RVA: 0x000CDAE0 File Offset: 0x000CBEE0
	Public Sub Init(pos As Vector2, properties As LevelProperties.Bat.Pentagrams, player As AbstractPlayerController, onRight As Boolean)
		MyBase.transform.position = pos
		Me.properties = properties
		Me.player = player
		Me.onRight = onRight
		MyBase.GetComponent(Of Collider2D)().enabled = False
		MyBase.StartCoroutine(Me.move_cr())
		MyBase.transform.SetScale(New Single?(properties.pentagramSize), New Single?(properties.pentagramSize), New Single?(1F))
	End Sub

	' Token: 0x060016E2 RID: 5858 RVA: 0x000CDB58 File Offset: 0x000CBF58
	Private Iterator Function move_cr() As IEnumerator
		Dim pos As Vector3 = MyBase.transform.position
		Dim endPos As Single = 560F
		If Me.onRight Then
			While MyBase.transform.position.x > Me.player.transform.position.x
				pos.x = Mathf.MoveTowards(MyBase.transform.position.x, Me.player.transform.position.x, Me.properties.xSpeed * CupheadTime.Delta)
				MyBase.transform.position = pos
				Yield Nothing
			End While
		Else
			While MyBase.transform.position.x < Me.player.transform.position.x
				pos.x = Mathf.MoveTowards(MyBase.transform.position.x, Me.player.transform.position.x, Me.properties.xSpeed * CupheadTime.Delta)
				MyBase.transform.position = pos
				Yield Nothing
			End While
		End If
		MyBase.GetComponent(Of Collider2D)().enabled = True
		While MyBase.transform.position.y < endPos
			pos.y = Mathf.MoveTowards(MyBase.transform.position.y, endPos, Me.properties.ySpeed * CupheadTime.Delta)
			MyBase.transform.position = pos
			Yield Nothing
		End While
		Me.Die()
		Yield Nothing
		Return
	End Function

	' Token: 0x060016E3 RID: 5859 RVA: 0x000CDB73 File Offset: 0x000CBF73
	Private Sub Die()
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub

	' Token: 0x0400202A RID: 8234
	Private properties As LevelProperties.Bat.Pentagrams

	' Token: 0x0400202B RID: 8235
	Private player As AbstractPlayerController

	' Token: 0x0400202C RID: 8236
	Private onRight As Boolean
End Class
