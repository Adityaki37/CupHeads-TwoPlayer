Imports System
Imports UnityEngine

' Token: 0x020005DC RID: 1500
Public Class DicePalacePachinkoLevelPlatforms
	Inherits AbstractCollidableObject

	' Token: 0x06001DA5 RID: 7589 RVA: 0x00110824 File Offset: 0x0010EC24
	Public Sub InitPlatforms(properties As LevelProperties.DicePalacePachinko)
		Dim vector As Vector3
		vector.y = CSng(Level.Current.Ground)
		vector.x = CSng(Level.Current.Left)
		vector.z = 0F
		For i As Integer = 0 To 3 - 1
			Dim num As Integer
			If i <> 0 Then
				num = If((i Mod 2 <> 0), 4, 3)
			Else
				num = 3
			End If
			For j As Integer = 0 To num - 1
				Dim gameObject As GameObject = Me.platformSprite.gameObject
				Dim vector2 As Vector2
				If num = 4 Then
					vector2.x = properties.CurrentState.pachinko.platformWidthFour
				Else
					vector2.x = properties.CurrentState.pachinko.platformWidthThree
				End If
				vector2.y = 1F
				gameObject.transform.localScale = vector2
				Dim vector3 As Vector3 = vector
				If num = 3 Then
					Dim num2 As Single = CSng(Level.Current.Width) - gameObject.GetComponent(Of SpriteRenderer)().sprite.bounds.size.x * 3.6F
					vector3.x = vector3.x + num2 / CSng((num - 1)) * CSng(j) + gameObject.GetComponent(Of SpriteRenderer)().sprite.bounds.size.x * 1.8F
				Else
					vector3.x += CSng((Level.Current.Width / (num - 1) * j))
				End If
				vector3.y = CSng(Level.Current.Ground) + Parser.FloatParse(properties.CurrentState.pachinko.platformHeights.Split(New Char() { ","c })(i)) + gameObject.GetComponent(Of SpriteRenderer)().sprite.bounds.size.y / 2F
				If j = 0 Then
					vector3.x += gameObject.GetComponent(Of SpriteRenderer)().sprite.bounds.size.x / 2F
				ElseIf j = num - 1 Then
					vector3.x -= gameObject.GetComponent(Of SpriteRenderer)().sprite.bounds.size.x / 2F
				End If
				Dim gameObject2 As GameObject = New GameObject()
				gameObject2.AddComponent(Of LevelPlatform)()
				gameObject2.GetComponent(Of BoxCollider2D)().size = New Vector2(gameObject.GetComponent(Of SpriteRenderer)().sprite.bounds.size.x * vector2.x, gameObject.GetComponent(Of SpriteRenderer)().sprite.bounds.size.y)
				gameObject2.transform.position = vector3
				Global.UnityEngine.[Object].Instantiate(Of GameObject)(gameObject, vector3, Quaternion.identity)
			Next
		Next
	End Sub

	' Token: 0x04002682 RID: 9858
	Private Const NUMBER_OF_ROWS As Integer = 3

	' Token: 0x04002683 RID: 9859
	Private Const MAX_NUMBER_OF_COLUMNS As Integer = 4

	' Token: 0x04002684 RID: 9860
	<SerializeField()>
	Private platformSprite As SpriteRenderer
End Class
