Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020003DA RID: 986
Public Class CupheadMapCamera
	Inherits AbstractCupheadGameCamera

	' Token: 0x17000239 RID: 569
	' (get) Token: 0x06000D2F RID: 3375 RVA: 0x0008C0C0 File Offset: 0x0008A4C0
	' (set) Token: 0x06000D30 RID: 3376 RVA: 0x0008C0C7 File Offset: 0x0008A4C7
	Public Shared Property Current As CupheadMapCamera

	' Token: 0x1700023A RID: 570
	' (get) Token: 0x06000D31 RID: 3377 RVA: 0x0008C0CF File Offset: 0x0008A4CF
	Public Overrides ReadOnly Property OrthographicSize As Single
		Get
			Return 3.6F
		End Get
	End Property

	' Token: 0x06000D32 RID: 3378 RVA: 0x0008C0D6 File Offset: 0x0008A4D6
	Protected Overrides Sub Awake()
		MyBase.Awake()
		CupheadMapCamera.Current = Me
		Me.SetupColliders()
	End Sub

	' Token: 0x06000D33 RID: 3379 RVA: 0x0008C0EA File Offset: 0x0008A4EA
	Private Sub OnDestroy()
		If CupheadMapCamera.Current Is Me Then
			CupheadMapCamera.Current = Nothing
		End If
	End Sub

	' Token: 0x1700023B RID: 571
	' (get) Token: 0x06000D34 RID: 3380 RVA: 0x0008C104 File Offset: 0x0008A504
	Private ReadOnly Property playerCenter As Vector2
		Get
			If PlayerManager.Multiplayer Then
				Return(Map.Current.players(0).transform.position + Map.Current.players(1).transform.position) / 2F
			End If
			Return Map.Current.players(0).transform.position
		End Get
	End Property

	' Token: 0x06000D35 RID: 3381 RVA: 0x0008C17C File Offset: 0x0008A57C
	Private Sub Update()
		If Map.Current.CurrentState = Map.State.[Event] Then
			Return
		End If
		Dim position As Vector3 = MyBase.transform.position
		Dim vector As Vector3 = Me.playerCenter
		If Me.properties.moveX Then
			position.x = vector.x
		End If
		If Me.properties.moveY Then
			position.y = vector.y
		End If
		position.x = Mathf.Clamp(position.x, Me.properties.bounds.left + Me.offset.x, Me.properties.bounds.right - Me.offset.x)
		position.y = Mathf.Clamp(position.y, Me.properties.bounds.bottom + Me.offset.y, Me.properties.bounds.top - Me.offset.y)
		If Me.centerOnPlayer Then
			MyBase.transform.position = Vector3.Lerp(MyBase.transform.position, position, Time.deltaTime * 6F)
		End If
		Me.UpdateColliders()
	End Sub

	' Token: 0x06000D36 RID: 3382 RVA: 0x0008C2BC File Offset: 0x0008A6BC
	Public Sub Init(properties As Map.Camera)
		MyBase.camera.orthographicSize = 3.6F
		Me.properties = properties
		Me.offset = New Vector2(MyBase.Bounds.width / 2F, MyBase.Bounds.height / 2F)
		MyBase.transform.position = Me.playerCenter
	End Sub

	' Token: 0x06000D37 RID: 3383 RVA: 0x0008C32C File Offset: 0x0008A72C
	Public Function IsCameraFarFromPlayer() As Boolean
		Dim position As Vector3 = MyBase.transform.position
		Dim vector As Vector3 = Me.playerCenter
		vector.x = Mathf.Clamp(vector.x, Me.properties.bounds.left + Me.offset.x, Me.properties.bounds.right - Me.offset.x)
		vector.y = Mathf.Clamp(vector.y, Me.properties.bounds.bottom + Me.offset.y, Me.properties.bounds.top - Me.offset.y)
		Return CDbl((position - vector).sqrMagnitude) > 0.01
	End Function

	' Token: 0x06000D38 RID: 3384 RVA: 0x0008C402 File Offset: 0x0008A802
	Public Function MoveToPosition(position As Vector2, time As Single, zoom As Single) As Coroutine
		MyBase.Zoom(zoom, time, EaseUtils.EaseType.easeInOutSine)
		Return MyBase.StartCoroutine(Me.moveToPosition_cr(position, time))
	End Function

	' Token: 0x06000D39 RID: 3385 RVA: 0x0008C41C File Offset: 0x0008A81C
	Private Iterator Function moveToPosition_cr(position As Vector2, time As Single) As IEnumerator
		Dim start As Vector2 = MyBase.transform.position
		Dim t As Single = 0F
		While t < time
			Dim val As Single = t / time
			Dim x As Single = EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, start.x, position.x, val)
			Dim y As Single = EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, start.y, position.y, val)
			MyBase.transform.SetPosition(New Single?(x), New Single?(y), New Single?(0F))
			t += MyBase.LocalDeltaTime
			Yield Nothing
		End While
		MyBase.transform.position = position
		Yield Nothing
		Return
	End Function

	' Token: 0x06000D3A RID: 3386 RVA: 0x0008C448 File Offset: 0x0008A848
	Private Sub SetupColliders()
		Me.edgeCollider = MyBase.gameObject.AddComponent(Of EdgeCollider2D)()
		Me.edgeCollider.points = New Vector2(1) {}
		Me.secretPathEdgeCollider = New GameObject() With { .transform = { .parent = MyBase.transform }, .layer = 25 }.AddComponent(Of EdgeCollider2D)()
		Me.secretPathEdgeCollider.points = Me.edgeCollider.points
		Me.UpdateColliders()
	End Sub

	' Token: 0x06000D3B RID: 3387 RVA: 0x0008C4C0 File Offset: 0x0008A8C0
	Private Sub UpdateColliders()
		Dim array As Vector2() = New Vector2() { New Vector3(-MyBase.Bounds.width / 2F, -MyBase.Bounds.height / 2F, 0F), New Vector3(-MyBase.Bounds.width / 2F, MyBase.Bounds.height / 2F, 0F), New Vector3(MyBase.Bounds.width / 2F, MyBase.Bounds.height / 2F, 0F), New Vector3(MyBase.Bounds.width / 2F, -MyBase.Bounds.height / 2F, 0F), New Vector3(-MyBase.Bounds.width / 2F, -MyBase.Bounds.height / 2F, 0F) }
		Me.edgeCollider.points = array
		Me.secretPathEdgeCollider.points = array
	End Sub

	' Token: 0x06000D3C RID: 3388 RVA: 0x0008C648 File Offset: 0x0008AA48
	Public Sub SetActiveCollider(active As Boolean)
		Me.edgeCollider.enabled = active
		Me.secretPathEdgeCollider.enabled = active
	End Sub

	' Token: 0x040016A6 RID: 5798
	Public centerOnPlayer As Boolean = True

	' Token: 0x040016A7 RID: 5799
	Private Const SPEED As Single = 6F

	' Token: 0x040016A8 RID: 5800
	Private Const ORTHO_SIZE As Single = 3.6F

	' Token: 0x040016AA RID: 5802
	Private properties As Map.Camera

	' Token: 0x040016AB RID: 5803
	Private offset As Vector2

	' Token: 0x040016AC RID: 5804
	Private edgeCollider As EdgeCollider2D

	' Token: 0x040016AD RID: 5805
	Private secretPathEdgeCollider As EdgeCollider2D
End Class
