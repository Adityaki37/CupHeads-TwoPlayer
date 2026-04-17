Imports System
Imports UnityEngine

' Token: 0x020003E5 RID: 997
Public Class KitchenParallaxLayer
	Inherits ParallaxLayer

	' Token: 0x06000D61 RID: 3425 RVA: 0x0008DF11 File Offset: 0x0008C311
	Protected Overrides Sub Start()
		MyBase.Start()
		Me.level = TryCast(Level.Current, SaltbakerLevel)
	End Sub

	' Token: 0x06000D62 RID: 3426 RVA: 0x0008DF2C File Offset: 0x0008C32C
	Protected Overrides Sub UpdateMinMax()
		Dim position As Vector3 = MyBase.transform.position
		If Not Me.ignoreX Then
			Dim vector As Vector2 = Me._camera.transform.position
			Dim zero As Vector2 = Vector2.zero
			Dim num As Single = vector.x + Mathf.Abs(Me._camera.Left)
			Dim num2 As Single = Me._camera.Right + Mathf.Abs(Me._camera.Left)
			zero.x = num / num2
			If Single.IsNaN(zero.x) Then
				zero.x = 0.5F
			End If
			position.x = Mathf.Lerp(Me.bottomLeft.x, Me.topRight.x, zero.x) + Me._camera.transform.position.x
		End If
		position.y = Mathf.Lerp(Me.startY, Me.endY, Me.level.yScrollPos)
		MyBase.transform.position = position
	End Sub

	' Token: 0x040016EA RID: 5866
	<SerializeField()>
	Private startY As Single

	' Token: 0x040016EB RID: 5867
	<SerializeField()>
	Private endY As Single

	' Token: 0x040016EC RID: 5868
	<SerializeField()>
	Private ignoreX As Boolean

	' Token: 0x040016ED RID: 5869
	Private level As SaltbakerLevel
End Class
