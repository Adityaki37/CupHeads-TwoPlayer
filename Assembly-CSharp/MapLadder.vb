Imports System
Imports UnityEngine

' Token: 0x0200096D RID: 2413
Public Class MapLadder
	Inherits AbstractMonoBehaviour

	' Token: 0x0600383E RID: 14398 RVA: 0x00203078 File Offset: 0x00201478
	Protected Overrides Sub OnDrawGizmos()
		Dim num As Single = 0.1F
		MyBase.OnDrawGizmos()
		Dim position As Vector3 = MyBase.baseTransform.position
		Dim vector As Vector3 = position + New Vector3(0F, Me.height, 0F)
		Me.DrawPointGizmos(position, Me.bottom)
		Me.DrawPointGizmos(vector, Me.top)
		Gizmos.color = Color.black
		Gizmos.DrawLine(position, vector)
		Gizmos.DrawLine(New Vector2(position.x - num, position.y), New Vector2(position.x + num, position.y))
	End Sub

	' Token: 0x0600383F RID: 14399 RVA: 0x00203128 File Offset: 0x00201528
	Private Sub DrawPointGizmos(point As Vector2, properties As MapLadder.PointProperties)
		Gizmos.color = Color.red
		Gizmos.DrawWireSphere(point + properties.dialogueOffset, 0.05F)
		Gizmos.color = Color.white
		Gizmos.DrawWireSphere(point + properties.dialogueOffset, 0.07F)
		Gizmos.color = Color.green
		Gizmos.DrawWireSphere(point + properties.interactionPoint, 0.05F)
		Gizmos.color = Color.white
		Gizmos.DrawWireSphere(point + properties.interactionPoint, 0.07F)
		Gizmos.color = Color.green
		Gizmos.DrawWireSphere(point + properties.interactionPoint, properties.interactionDistance)
		Gizmos.color = Color.white
		Gizmos.DrawWireSphere(point + properties.interactionPoint, properties.interactionDistance + 0.02F)
		Gizmos.color = Color.blue
		Gizmos.DrawWireCube(point + properties.[exit], Vector3.one * 0.05F)
		Gizmos.color = Color.white
		Gizmos.DrawWireCube(point + properties.[exit], Vector3.one * 0.07F)
	End Sub

	' Token: 0x06003840 RID: 14400 RVA: 0x00203279 File Offset: 0x00201679
	Private Sub SetLayer(renderer As SpriteRenderer)
		If renderer Is Nothing Then
			Return
		End If
		renderer.sortingLayerName = "Background"
		renderer.sortingOrder = 100
	End Sub

	' Token: 0x04004016 RID: 16406
	Public Shared DIALOGUE_OFFSET As Vector2 = New Vector2(0F, 0.5F)

	' Token: 0x04004017 RID: 16407
	Public Shared INTERACTION_POINT_TOP As Vector2 = New Vector2(0F, 0.1F)

	' Token: 0x04004018 RID: 16408
	Public Shared INTERACTION_POINT_BOTTOM As Vector2 = New Vector2(0F, -0.1F)

	' Token: 0x04004019 RID: 16409
	Public Const INTERACTION_DISTANCE As Single = 0.2F

	' Token: 0x0400401A RID: 16410
	Public Shared DIALOGUE_ENTER As AbstractUIInteractionDialogue.Properties = New AbstractUIInteractionDialogue.Properties("CLIMB")

	' Token: 0x0400401B RID: 16411
	Public Shared DIALOGUE_EXIT As AbstractUIInteractionDialogue.Properties = New AbstractUIInteractionDialogue.Properties("EXIT")

	' Token: 0x0400401C RID: 16412
	Public Shared EXIT_TOP As Vector2 = New Vector2(0F, 0.2F)

	' Token: 0x0400401D RID: 16413
	Public Shared EXIT_BOTTOM As Vector2 = New Vector2(0F, -0.2F)

	' Token: 0x0400401E RID: 16414
	Public height As Single = 1F

	' Token: 0x0400401F RID: 16415
	<SerializeField()>
	Private top As MapLadder.PointProperties = MapLadder.PointProperties.TopDefault()

	' Token: 0x04004020 RID: 16416
	<SerializeField()>
	Private bottom As MapLadder.PointProperties = MapLadder.PointProperties.BottomDefault()

	' Token: 0x0200096E RID: 2414
	Public Enum Location
		' Token: 0x04004022 RID: 16418
		Top
		' Token: 0x04004023 RID: 16419
		Bottom
	End Enum

	' Token: 0x0200096F RID: 2415
	<Serializable()>
	Public Class PointProperties
		' Token: 0x1700048A RID: 1162
		' (get) Token: 0x06003843 RID: 14403 RVA: 0x0020335F File Offset: 0x0020175F
		' (set) Token: 0x06003844 RID: 14404 RVA: 0x00203367 File Offset: 0x00201767
		Public Property location As MapLadder.Location

		' Token: 0x06003845 RID: 14405 RVA: 0x00203370 File Offset: 0x00201770
		Public Shared Function TopDefault() As MapLadder.PointProperties
			Return New MapLadder.PointProperties() With { .interactionPoint = MapLadder.INTERACTION_POINT_TOP, .[exit] = MapLadder.EXIT_TOP, .location = MapLadder.Location.Top }
		End Function

		' Token: 0x06003846 RID: 14406 RVA: 0x002033A4 File Offset: 0x002017A4
		Public Shared Function BottomDefault() As MapLadder.PointProperties
			Return New MapLadder.PointProperties() With { .interactionPoint = MapLadder.INTERACTION_POINT_BOTTOM, .[exit] = MapLadder.EXIT_BOTTOM, .location = MapLadder.Location.Bottom }
		End Function

		' Token: 0x04004024 RID: 16420
		Public interactionPoint As Vector2 = Vector2.zero

		' Token: 0x04004025 RID: 16421
		Public interactionDistance As Single = 0.2F

		' Token: 0x04004026 RID: 16422
		Public dialogueOffset As Vector2 = MapLadder.DIALOGUE_OFFSET

		' Token: 0x04004027 RID: 16423
		Public [exit] As Vector2 = Vector2.zero
	End Class
End Class
