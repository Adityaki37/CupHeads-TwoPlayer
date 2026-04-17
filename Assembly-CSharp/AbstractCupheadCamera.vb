Imports System
Imports UnityEngine
Imports UnityEngine.UI

' Token: 0x020003D1 RID: 977
<RequireComponent(GetType(Camera))>
Public MustInherit Class AbstractCupheadCamera
	Inherits AbstractMonoBehaviour

	' Token: 0x17000225 RID: 549
	' (get) Token: 0x06000CB4 RID: 3252 RVA: 0x000891E7 File Offset: 0x000875E7
	Public ReadOnly Property camera As Camera
		Get
			If Me._camera Is Nothing Then
				Me._camera = MyBase.GetComponent(Of Camera)()
			End If
			Return Me._camera
		End Get
	End Property

	' Token: 0x06000CB5 RID: 3253 RVA: 0x0008920C File Offset: 0x0008760C
	Public Function ContainsPoint(point As Vector2) As Boolean
		Return Me.ContainsPoint(point, Vector2.zero)
	End Function

	' Token: 0x06000CB6 RID: 3254 RVA: 0x0008921C File Offset: 0x0008761C
	Public Function ContainsPoint(point As Vector2, padding As Vector2) As Boolean
		Return Me.CalculateContainsBounds(padding).Contains(point)
	End Function

	' Token: 0x06000CB7 RID: 3255 RVA: 0x0008923C File Offset: 0x0008763C
	Public Function CalculateContainsBounds(padding As Vector2) As Rect
		Dim orthographicSize As Single = Me.camera.orthographicSize
		Dim position As Vector3 = MyBase.transform.position
		Dim num As Single = orthographicSize * 1.7777778F * 2F + padding.x * 2F
		Dim num2 As Single = orthographicSize * 2F + padding.y * 2F
		Return RectUtils.NewFromCenter(position.x, position.y, num, num2)
	End Function

	' Token: 0x06000CB8 RID: 3256 RVA: 0x000892AE File Offset: 0x000876AE
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.camera.clearFlags = CameraClearFlags.[Nothing]
	End Sub

	' Token: 0x17000226 RID: 550
	' (get) Token: 0x06000CB9 RID: 3257 RVA: 0x000892C4 File Offset: 0x000876C4
	Public ReadOnly Property Bounds As Rect
		Get
			Dim num As Single = Me.camera.orthographicSize * 1.7777778F * 2F
			Dim num2 As Single = Me.camera.orthographicSize * 2F
			Return RectUtils.NewFromCenter(MyBase.transform.position.x, MyBase.transform.position.y, num, num2)
		End Get
	End Property

	' Token: 0x06000CBA RID: 3258 RVA: 0x0008932D File Offset: 0x0008772D
	Protected Overridable Sub LateUpdate()
		Me.UpdateRect()
	End Sub

	' Token: 0x06000CBB RID: 3259 RVA: 0x00089338 File Offset: 0x00087738
	Public Sub UpdateRect()
		Dim num As Single = CSng(Screen.width) / CSng(Screen.height)
		Dim num2 As Single = 1F - 0.1F * SettingsData.Data.overscan
		Dim rect As Rect
		If num > 1.7777778F Then
			rect = RectUtils.NewFromCenter(0.5F, 0.5F, num2 * 1.7777778F / num, num2 * 1F)
		Else
			rect = RectUtils.NewFromCenter(0.5F, 0.5F, num2 * 1F, num2 * num / 1.7777778F)
		End If
		If Me.camera.rect <> rect Then
			Me.camera.rect = rect
			Dim array As CanvasScaler() = Global.UnityEngine.[Object].FindObjectsOfType(Of CanvasScaler)()
			For Each canvasScaler As CanvasScaler In array
				canvasScaler.referenceResolution = New Vector2(1280F / rect.height, 720F / rect.height)
			Next
		End If
	End Sub

	' Token: 0x06000CBC RID: 3260 RVA: 0x0008942B File Offset: 0x0008782B
	Protected Overrides Sub OnDrawGizmos()
		MyBase.OnDrawGizmos()
		Me.DrawGizmos(0.1F)
	End Sub

	' Token: 0x06000CBD RID: 3261 RVA: 0x0008943E File Offset: 0x0008783E
	Protected Overrides Sub OnDrawGizmosSelected()
		MyBase.OnDrawGizmosSelected()
		Me.DrawGizmos(1F)
	End Sub

	' Token: 0x06000CBE RID: 3262 RVA: 0x00089454 File Offset: 0x00087854
	Private Sub DrawGizmos(a As Single)
		Gizmos.color = Color.white * New Color(1F, 1F, 1F, a)
		Gizmos.DrawWireCube(Me.camera.transform.position, New Vector3(Me.camera.orthographicSize * Me.camera.aspect * 2F, Me.camera.orthographicSize * 2F, 0F))
		Gizmos.DrawWireSphere(Me.camera.transform.position, 50F)
		Gizmos.color = New Color(0F, 1F, 0F, a)
		Gizmos.DrawWireSphere(Me.camera.transform.position, 10F)
	End Sub

	' Token: 0x04001642 RID: 5698
	Private _camera As Camera
End Class
