Imports System
Imports UnityEngine

' Token: 0x020008FF RID: 2303
Public Class PlatformingLevelEditorPlatform
	Inherits AbstractMonoBehaviour

	' Token: 0x0600360B RID: 13835 RVA: 0x001F6514 File Offset: 0x001F4914
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Dim type As PlatformingLevelEditorPlatform.Type = Me._type
		If type <> PlatformingLevelEditorPlatform.Type.Platform Then
			If type = PlatformingLevelEditorPlatform.Type.Solid Then
				Dim gameObject As GameObject = New GameObject("ground")
				Dim gameObject2 As GameObject = New GameObject("walls")
				Dim gameObject3 As GameObject = New GameObject("ceiling")
				gameObject.layer = 20
				gameObject.tag = "Ground"
				gameObject2.layer = 18
				gameObject2.tag = "Wall"
				gameObject3.layer = 19
				gameObject3.tag = "Ceiling"
				gameObject.transform.SetParent(MyBase.transform)
				gameObject2.transform.SetParent(MyBase.transform)
				gameObject3.transform.SetParent(MyBase.transform)
				gameObject.transform.ResetLocalTransforms()
				gameObject2.transform.ResetLocalTransforms()
				gameObject3.transform.ResetLocalTransforms()
				Me._topCollider = gameObject.AddComponent(Of BoxCollider2D)()
				Me._middleCollider = gameObject2.AddComponent(Of BoxCollider2D)()
				Me._bottomCollider = gameObject3.AddComponent(Of BoxCollider2D)()
				Me._topCollider.isTrigger = True
				Me._middleCollider.isTrigger = True
				Me._bottomCollider.isTrigger = True
				Me._topCollider.size = New Vector2(Me._size.x, 20F)
				Me._middleCollider.size = Me._size - New Vector2(0F, 40F)
				Me._bottomCollider.size = New Vector2(Me._size.x, 20F)
				Me._topCollider.offset = New Vector2(0F, Me._size.y / 2F - Me._topCollider.size.y / 2F) + Me._offset
				Me._middleCollider.offset = Vector2.zero + Me._offset
				Me._bottomCollider.offset = New Vector2(0F, -(Me._size.y / 2F - Me._bottomCollider.size.y / 2F)) + Me._offset
			End If
		Else
			Me._collider = MyBase.gameObject.AddComponent(Of BoxCollider2D)()
			Me._collider.size = Me._size
			Me._collider.offset = Me._offset
			Me._collider.isTrigger = True
			Me._platform = MyBase.gameObject.AddComponent(Of LevelPlatform)()
			Me._platform.canFallThrough = Me._canFallThrough
		End If
	End Sub

	' Token: 0x0600360C RID: 13836 RVA: 0x001F67BA File Offset: 0x001F4BBA
	Protected Overrides Sub OnDrawGizmos()
		MyBase.OnDrawGizmos()
		Me.DrawGizmos(0.5F)
	End Sub

	' Token: 0x0600360D RID: 13837 RVA: 0x001F67CD File Offset: 0x001F4BCD
	Protected Overrides Sub OnDrawGizmosSelected()
		MyBase.OnDrawGizmosSelected()
		Me.DrawGizmos(1F)
	End Sub

	' Token: 0x0600360E RID: 13838 RVA: 0x001F67E0 File Offset: 0x001F4BE0
	Private Sub DrawGizmos(a As Single)
		Dim matrix As Matrix4x4 = Gizmos.matrix
		Gizmos.matrix = MyBase.transform.localToWorldMatrix
		Dim vector As Vector2 = Vector2.zero + Me._offset
		Gizmos.color = New Color(0F, 0F, 0F, 0.4F * a)
		Gizmos.DrawCube(vector, Me._size)
		Gizmos.color = Color.cyan * New Color(1F, 1F, 1F, a)
		Dim type As PlatformingLevelEditorPlatform.Type = Me._type
		If type <> PlatformingLevelEditorPlatform.Type.Platform Then
			If type = PlatformingLevelEditorPlatform.Type.Solid Then
				Gizmos.DrawWireCube(vector, Me._size)
			End If
		Else
			Dim num As Single = vector.y + Me._size.y / 2F
			Dim num2 As Single = vector.y - Me._size.y / 2F
			Dim num3 As Single = num - 10F
			Dim num4 As Single = vector.x - Me._size.x / 2F
			Dim num5 As Single = vector.x + Me._size.x / 2F
			Gizmos.DrawLine(New Vector2(num4, num), New Vector2(num5, num))
			If Not Me._canFallThrough Then
				Gizmos.DrawLine(New Vector2(num4, num3), New Vector2(num5, num3))
			Else
				Gizmos.DrawLine(New Vector2(vector.x, num2 + 50F), New Vector2(vector.x, num2))
				Gizmos.DrawLine(New Vector2(vector.x - 20F, num2 + 20F), New Vector2(vector.x, num2))
				Gizmos.DrawLine(New Vector2(vector.x + 20F, num2 + 20F), New Vector2(vector.x, num2))
			End If
		End If
		Gizmos.matrix = matrix
	End Sub

	' Token: 0x04003E0F RID: 15887
	Private Const THICKNESS As Integer = 20

	' Token: 0x04003E10 RID: 15888
	<SerializeField()>
	Private _type As PlatformingLevelEditorPlatform.Type

	' Token: 0x04003E11 RID: 15889
	<SerializeField()>
	Private _canFallThrough As Boolean

	' Token: 0x04003E12 RID: 15890
	<SerializeField()>
	Private _size As Vector2 = New Vector2(100F, 10F)

	' Token: 0x04003E13 RID: 15891
	<SerializeField()>
	Private _offset As Vector2 = New Vector2(0F, 0F)

	' Token: 0x04003E14 RID: 15892
	Private _platform As LevelPlatform

	' Token: 0x04003E15 RID: 15893
	Private _collider As BoxCollider2D

	' Token: 0x04003E16 RID: 15894
	Private _topCollider As BoxCollider2D

	' Token: 0x04003E17 RID: 15895
	Private _middleCollider As BoxCollider2D

	' Token: 0x04003E18 RID: 15896
	Private _bottomCollider As BoxCollider2D

	' Token: 0x02000900 RID: 2304
	Public Enum Type
		' Token: 0x04003E1A RID: 15898
		Platform
		' Token: 0x04003E1B RID: 15899
		Solid
	End Enum
End Class
