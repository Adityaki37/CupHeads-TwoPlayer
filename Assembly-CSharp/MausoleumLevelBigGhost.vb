Imports System
Imports UnityEngine

' Token: 0x020006D4 RID: 1748
Public Class MausoleumLevelBigGhost
	Inherits MausoleumLevelGhostBase

	' Token: 0x06002539 RID: 9529 RVA: 0x0015D0CC File Offset: 0x0015B4CC
	Public Function Create(position As Vector2, rotation As Single, speed As Single, properties As LevelProperties.Mausoleum.BigGhost, urn As GameObject) As MausoleumLevelBigGhost
		Dim mausoleumLevelBigGhost As MausoleumLevelBigGhost = TryCast(MyBase.Create(position, rotation, speed), MausoleumLevelBigGhost)
		mausoleumLevelBigGhost.properties = properties
		mausoleumLevelBigGhost.urn = urn
		Return mausoleumLevelBigGhost
	End Function

	' Token: 0x0600253A RID: 9530 RVA: 0x0015D0FC File Offset: 0x0015B4FC
	Public Overrides Sub OnParry(player As AbstractPlayerController)
		Dim vector As Vector2 = Me.smallRoot1.transform.position
		Dim vector2 As Vector2 = vector
		Dim vector3 As Vector2 = New Vector2(Global.UnityEngine.Random.value * CSng(If((Not Rand.Bool()), (-1), 1)), Global.UnityEngine.Random.value * CSng(If((Not Rand.Bool()), (-1), 1)))
		vector = vector2 + vector3.normalized * Me.smallRoot1.radius * Global.UnityEngine.Random.value
		Dim vector4 As Vector2 = Me.smallRoot2.transform.position
		Dim vector5 As Vector2 = vector4
		Dim vector6 As Vector2 = New Vector2(Global.UnityEngine.Random.value * CSng(If((Not Rand.Bool()), (-1), 1)), Global.UnityEngine.Random.value * CSng(If((Not Rand.Bool()), (-1), 1)))
		vector4 = vector5 + vector6.normalized * Me.smallRoot2.radius * Global.UnityEngine.Random.value
		Dim vector7 As Vector3 = Me.urn.transform.position - vector
		Dim vector8 As Vector3 = Me.urn.transform.position - vector4
		Dim mausoleumLevelRegularGhost As MausoleumLevelRegularGhost = TryCast(Me.regGhost.Create(vector, MathUtils.DirectionToAngle(vector7), Me.properties.littleGhostSpeed), MausoleumLevelRegularGhost)
		mausoleumLevelRegularGhost.GetParent(Me.parent)
		Dim mausoleumLevelRegularGhost2 As MausoleumLevelRegularGhost = TryCast(Me.regGhost.Create(vector4, MathUtils.DirectionToAngle(vector8), Me.properties.littleGhostSpeed), MausoleumLevelRegularGhost)
		mausoleumLevelRegularGhost2.GetParent(Me.parent)
		mausoleumLevelRegularGhost.transform.SetScale(New Single?(0.7F), New Single?(0.7F), New Single?(0.7F))
		mausoleumLevelRegularGhost2.transform.SetScale(New Single?(0.7F), New Single?(0.7F), New Single?(0.7F))
		MyBase.OnParry(player)
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub

	' Token: 0x04002DD9 RID: 11737
	<SerializeField()>
	Private regGhost As MausoleumLevelRegularGhost

	' Token: 0x04002DDA RID: 11738
	<SerializeField()>
	Private smallRoot1 As FlyingBlimpLevelSpawnRadius

	' Token: 0x04002DDB RID: 11739
	<SerializeField()>
	Private smallRoot2 As FlyingBlimpLevelSpawnRadius

	' Token: 0x04002DDC RID: 11740
	Private properties As LevelProperties.Mausoleum.BigGhost

	' Token: 0x04002DDD RID: 11741
	Private urn As GameObject
End Class
