Imports System
Imports UnityEngine

' Token: 0x02000621 RID: 1569
Public Class FlyingBirdLevelLaser
	Inherits AbstractMonoBehaviour

	' Token: 0x06001FEC RID: 8172 RVA: 0x00125514 File Offset: 0x00123914
	Public Function Create(pos As Vector2, speed As Single) As FlyingBirdLevelLaser
		Dim flyingBirdLevelLaser As FlyingBirdLevelLaser = Me.InstantiatePrefab(Of FlyingBirdLevelLaser)()
		flyingBirdLevelLaser.transform.position = pos + New Vector2(flyingBirdLevelLaser.size.x, 0F)
		flyingBirdLevelLaser.speed = speed
		Return flyingBirdLevelLaser
	End Function

	' Token: 0x06001FED RID: 8173 RVA: 0x0012555C File Offset: 0x0012395C
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Dim component As SpriteRenderer = MyBase.transform.GetComponent(Of SpriteRenderer)()
		Me.size = component.sprite.bounds.size
	End Sub

	' Token: 0x06001FEE RID: 8174 RVA: 0x0012559C File Offset: 0x0012399C
	Private Sub Update()
		MyBase.transform.AddPosition(-Me.speed * CupheadTime.Delta, 0F, 0F)
		If MyBase.transform.position.x < -740F Then
			Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
		End If
	End Sub

	' Token: 0x04002869 RID: 10345
	Private size As Vector2

	' Token: 0x0400286A RID: 10346
	Private speed As Single
End Class
