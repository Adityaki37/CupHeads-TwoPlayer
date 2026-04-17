Imports System
Imports UnityEngine

' Token: 0x02000780 RID: 1920
Public Class RobotLevelScrollingSprite
	Inherits ScrollingSpriteSpawner

	' Token: 0x06002A35 RID: 10805 RVA: 0x0018B2A4 File Offset: 0x001896A4
	Protected Overrides Sub OnSpawn(obj As GameObject)
		MyBase.OnSpawn(obj)
		Dim component As SpriteRenderer = obj.GetComponent(Of SpriteRenderer)()
		component.sortingLayerName = Me.layer.ToString()
		component.sprite = Me.sprites(Global.UnityEngine.Random.Range(0, Me.sprites.Length))
		Dim vector As Vector3 = Vector3.up * Global.UnityEngine.Random.Range(Me.yOffset.min, Me.yOffset.max)
		obj.transform.position += vector
		obj.transform.localScale = New Vector3(MyBase.transform.localScale.x * CSng(MathUtils.PlusOrMinus()), MyBase.transform.localScale.y, MyBase.transform.localScale.z)
	End Sub

	' Token: 0x0400330E RID: 13070
	<SerializeField()>
	Private layer As SpriteLayer = SpriteLayer.[Default]

	' Token: 0x0400330F RID: 13071
	<SerializeField()>
	Private yOffset As MinMax

	' Token: 0x04003310 RID: 13072
	<SerializeField()>
	Private sprites As Sprite()
End Class
