Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020003D8 RID: 984
Public Class CupheadLevelCamera
	Inherits AbstractCupheadGameCamera

	' Token: 0x1700022F RID: 559
	' (get) Token: 0x06000CFC RID: 3324 RVA: 0x0008A852 File Offset: 0x00088C52
	' (set) Token: 0x06000CFD RID: 3325 RVA: 0x0008A859 File Offset: 0x00088C59
	Public Shared Property Current As CupheadLevelCamera

	' Token: 0x17000230 RID: 560
	' (get) Token: 0x06000CFE RID: 3326 RVA: 0x0008A861 File Offset: 0x00088C61
	' (set) Token: 0x06000CFF RID: 3327 RVA: 0x0008A869 File Offset: 0x00088C69
	Public Property cameraLocked As Boolean

	' Token: 0x17000231 RID: 561
	' (get) Token: 0x06000D00 RID: 3328 RVA: 0x0008A872 File Offset: 0x00088C72
	' (set) Token: 0x06000D01 RID: 3329 RVA: 0x0008A87A File Offset: 0x00088C7A
	Public Property cameraOffset As Boolean

	' Token: 0x17000232 RID: 562
	' (get) Token: 0x06000D02 RID: 3330 RVA: 0x0008A883 File Offset: 0x00088C83
	' (set) Token: 0x06000D03 RID: 3331 RVA: 0x0008A88B File Offset: 0x00088C8B
	Public Property autoScrolling As Boolean

	' Token: 0x17000233 RID: 563
	' (get) Token: 0x06000D04 RID: 3332 RVA: 0x0008A894 File Offset: 0x00088C94
	' (set) Token: 0x06000D05 RID: 3333 RVA: 0x0008A89C File Offset: 0x00088C9C
	Public Property autoScrollSpeedMultiplier As Single

	' Token: 0x17000234 RID: 564
	' (get) Token: 0x06000D06 RID: 3334 RVA: 0x0008A8A5 File Offset: 0x00088CA5
	' (set) Token: 0x06000D07 RID: 3335 RVA: 0x0008A8AD File Offset: 0x00088CAD
	Public Property Left As Single

	' Token: 0x17000235 RID: 565
	' (get) Token: 0x06000D08 RID: 3336 RVA: 0x0008A8B6 File Offset: 0x00088CB6
	' (set) Token: 0x06000D09 RID: 3337 RVA: 0x0008A8BE File Offset: 0x00088CBE
	Public Property Right As Single

	' Token: 0x17000236 RID: 566
	' (get) Token: 0x06000D0A RID: 3338 RVA: 0x0008A8C7 File Offset: 0x00088CC7
	' (set) Token: 0x06000D0B RID: 3339 RVA: 0x0008A8CF File Offset: 0x00088CCF
	Public Property Bottom As Single

	' Token: 0x17000237 RID: 567
	' (get) Token: 0x06000D0C RID: 3340 RVA: 0x0008A8D8 File Offset: 0x00088CD8
	' (set) Token: 0x06000D0D RID: 3341 RVA: 0x0008A8E0 File Offset: 0x00088CE0
	Public Property Top As Single

	' Token: 0x17000238 RID: 568
	' (get) Token: 0x06000D0E RID: 3342 RVA: 0x0008A8E9 File Offset: 0x00088CE9
	Public Overrides ReadOnly Property OrthographicSize As Single
		Get
			Return 360F
		End Get
	End Property

	' Token: 0x06000D0F RID: 3343 RVA: 0x0008A8F0 File Offset: 0x00088CF0
	Protected Overrides Sub Awake()
		MyBase.Awake()
		CupheadLevelCamera.Current = Me
	End Sub

	' Token: 0x06000D10 RID: 3344 RVA: 0x0008A8FE File Offset: 0x00088CFE
	Private Sub Start()
		Me.autoScrolling = False
		Me.cameraLocked = False
		Me.cameraOffset = False
		Me.autoScrollSpeedMultiplier = 1F
		Me._position = MyBase.transform.position
	End Sub

	' Token: 0x06000D11 RID: 3345 RVA: 0x0008A931 File Offset: 0x00088D31
	Private Sub OnDestroy()
		If CupheadLevelCamera.Current Is Me Then
			CupheadLevelCamera.Current = Nothing
		End If
	End Sub

	' Token: 0x06000D12 RID: 3346 RVA: 0x0008A94C File Offset: 0x00088D4C
	Private Sub Update()
		If PlayerManager.Count <= 0 Then
			Return
		End If
		Me.UpdateBounds()
		Dim position As Vector3 = Me._position
		Dim mode As CupheadLevelCamera.Mode = Me.mode
		Select Case mode
			Case CupheadLevelCamera.Mode.Lerp
			Case CupheadLevelCamera.Mode.TrapBox
				Me.UpdateModeTrapBox()
				GoTo IL_00A4
			Case CupheadLevelCamera.Mode.Relative
				Me.UpdateModeRelative()
				GoTo IL_00A4
			Case CupheadLevelCamera.Mode.Platforming
				Me.UpdatePlatforming()
				GoTo IL_00A4
			Case CupheadLevelCamera.Mode.Path
				Me.UpdatePath()
				GoTo IL_00A4
			Case CupheadLevelCamera.Mode.RelativeRook
				Me.UpdateModeRelativeRook()
				GoTo IL_00A4
			Case CupheadLevelCamera.Mode.RelativeRumRunners
				Me.UpdateModeRelativeRumRunners()
				GoTo IL_00A4
			Case Else
				If mode = CupheadLevelCamera.Mode.[Static] Then
					GoTo IL_00A4
				End If
		End Select
		Me.UpdateModeLerp()
		IL_00A4:
		Dim position2 As Vector3 = Me._position
		If MyBase.Width * 2F > CSng(Me.bounds.Width) Then
			position2.x = Mathf.Lerp(position.x, 0F, CupheadTime.Delta * 10F)
		End If
		If MyBase.Height * 2F > CSng(Me.bounds.Height) Then
			position2.y = Mathf.Lerp(position.y, 0F, CupheadTime.Delta * 10F)
		End If
		Me._position = position2
		MyBase.Move()
	End Sub

	' Token: 0x06000D13 RID: 3347 RVA: 0x0008AA9B File Offset: 0x00088E9B
	Protected Overrides Sub LateUpdate()
		MyBase.LateUpdate()
		MyBase.Move()
	End Sub

	' Token: 0x06000D14 RID: 3348 RVA: 0x0008AAAC File Offset: 0x00088EAC
	Private Sub UpdateBounds()
		Me.Left = If((Not Me.bounds.leftEnabled), Single.MinValue, (CSng((-CSng(Me.bounds.left))) + MyBase.Width))
		Me.Right = If((Not Me.bounds.rightEnabled), Single.MaxValue, (CSng(Me.bounds.right) - MyBase.Width))
		Me.Bottom = If((Not Me.bounds.bottomEnabled), Single.MinValue, (CSng((-CSng(Me.bounds.bottom))) + MyBase.Height))
		Me.Top = If((Not Me.bounds.topEnabled), Single.MaxValue, (CSng(Me.bounds.top) - MyBase.Height))
	End Sub

	' Token: 0x06000D15 RID: 3349 RVA: 0x0008AB87 File Offset: 0x00088F87
	Public Sub DisableRightCollider()
		Me.rightCollider.gameObject.SetActive(False)
	End Sub

	' Token: 0x06000D16 RID: 3350 RVA: 0x0008AB9A File Offset: 0x00088F9A
	Public Sub MoveRightCollider()
		Me.rightCollider.transform.AddPosition(100F, 0F, 0F)
	End Sub

	' Token: 0x06000D17 RID: 3351 RVA: 0x0008ABBC File Offset: 0x00088FBC
	Public Sub Init(properties As Level.Camera)
		MyBase.enabled = True
		Me.mode = properties.mode
		MyBase.zoom = properties.zoom
		Me.moveX = properties.moveX
		Me.moveY = properties.moveY
		Me.stabilizeY = properties.stabilizeY
		Me.stabilizePaddingTop = properties.stabilizePaddingTop
		Me.stabilizePaddingBottom = properties.stabilizePaddingBottom
		Me.bounds = properties.bounds
		Me.path = properties.path
		Me.pathMovesOnlyForward = properties.pathMovesOnlyForward
		If properties.mode = CupheadLevelCamera.Mode.Path Then
			MyBase.transform.position = Me.path.Lerp(0F)
		End If
		Me.UpdateBounds()
		If properties.colliders Then
			Me.collidersRoot = New GameObject("Colliders").transform
			Me.collidersRoot.parent = MyBase.transform
			Me.collidersRoot.ResetLocalTransforms()
			Me.SetupCollider(Level.Bounds.Side.Left)
			Me.rightCollider = Me.SetupCollider(Level.Bounds.Side.Right)
		End If
	End Sub

	' Token: 0x06000D18 RID: 3352 RVA: 0x0008ACC8 File Offset: 0x000890C8
	Private Function CreateBorderRenderer(texture As Texture2D, parent As Transform, name As String) As SpriteRenderer
		Dim zero As Vector2 = Vector2.zero
		Dim zero2 As Vector2 = Vector2.zero
		Dim zero3 As Vector2 = Vector2.zero
		Dim text As String = name.ToLower()
		If text IsNot Nothing Then
			If Not(text = "left") Then
				If Not(text = "right") Then
					If Not(text = "top") Then
						If text = "bottom" Then
							zero = New Vector2(CSng(Me.bounds.Width) + 2000F, 1000F)
							zero2 = New Vector2(Me.bounds.Center.x, CSng((-CSng(Me.bounds.bottom))))
							zero3 = New Vector2(0.5F, 1F)
						End If
					Else
						zero = New Vector2(CSng(Me.bounds.Width) + 2000F, 1000F)
						zero2 = New Vector2(Me.bounds.Center.x, CSng(Me.bounds.top))
						zero3 = New Vector2(0.5F, 0F)
					End If
				Else
					zero = New Vector2(1000F, CSng(Me.bounds.Height) + 2000F)
					zero2 = New Vector2(CSng(Me.bounds.right), Me.bounds.Center.y)
					zero3 = New Vector2(0F, 0.5F)
				End If
			Else
				zero = New Vector2(1000F, CSng(Me.bounds.Height) + 2000F)
				zero2 = New Vector2(CSng((-CSng(Me.bounds.left))), Me.bounds.Center.y)
				zero3 = New Vector2(1F, 0.5F)
			End If
		End If
		Dim spriteRenderer As SpriteRenderer = New GameObject(name).AddComponent(Of SpriteRenderer)()
		spriteRenderer.transform.SetParent(parent)
		Dim sprite As Sprite = Sprite.Create(texture, New Rect(0F, 0F, 1F, 1F), zero3, 1F)
		spriteRenderer.sprite = sprite
		spriteRenderer.transform.localScale = zero
		spriteRenderer.transform.position = zero2
		spriteRenderer.sortingLayerName = SpriteLayer.Foreground.ToString()
		spriteRenderer.sortingOrder = 10000
		Return spriteRenderer
	End Function

	' Token: 0x06000D19 RID: 3353 RVA: 0x0008AF40 File Offset: 0x00089340
	Private Function CreateBorderTexture() As Texture2D
		Dim texture2D As Texture2D = New Texture2D(1, 1)
		texture2D.filterMode = FilterMode.Point
		texture2D.SetPixel(0, 0, Color.black)
		texture2D.Apply()
		Return texture2D
	End Function

	' Token: 0x06000D1A RID: 3354 RVA: 0x0008AF70 File Offset: 0x00089370
	Private Function SetupCollider(side As Level.Bounds.Side) As Transform
		Dim text As String = String.Empty
		Dim text2 As String = String.Empty
		Dim num As Integer = 0
		Dim num2 As Integer = 0
		Dim zero As Vector2 = Vector2.zero
		Dim vector As Vector2 = New Vector2(MyBase.Bounds.xMin, MyBase.Bounds.yMax)
		Dim vector2 As Vector2 = New Vector2(MyBase.Bounds.xMax, MyBase.Bounds.yMin)
		Dim x As Single = vector.x
		Dim x2 As Single = vector2.x
		Dim y As Single = vector2.y
		Dim y2 As Single = vector.y
		Select Case side
			Case Level.Bounds.Side.Left
				text = "Level_Wall_Left"
				text2 = "Wall"
				num = LayerMask.NameToLayer(Layers.Bounds_Walls.ToString())
				num2 = 90
				zero = New Vector2(x - 200F, 0F)
			Case Level.Bounds.Side.Right
				text = "Level_Wall_Right"
				text2 = "Wall"
				num = LayerMask.NameToLayer(Layers.Bounds_Walls.ToString())
				num2 = -90
				zero = New Vector2(x2 + 200F, 0F)
			Case Level.Bounds.Side.Top
				text = "Level_Ceiling"
				text2 = "Ceiling"
				num = LayerMask.NameToLayer(Layers.Bounds_Ceiling.ToString())
				zero = New Vector2(0F, y + 200F)
			Case Level.Bounds.Side.Bottom
				text = "Level_Ground"
				text2 = "Ground"
				num = LayerMask.NameToLayer(Layers.Bounds_Ground.ToString())
				num2 = 180
				zero = New Vector2(0F, y2 - 200F)
		End Select
		Dim gameObject As GameObject = New GameObject(text)
		gameObject.tag = text2
		gameObject.layer = num
		gameObject.transform.ResetLocalTransforms()
		gameObject.transform.SetPosition(New Single?(zero.x), New Single?(zero.y), Nothing)
		gameObject.transform.SetEulerAngles(Nothing, Nothing, New Single?(CSng(num2)))
		gameObject.transform.parent = Me.collidersRoot
		Dim boxCollider2D As BoxCollider2D = gameObject.AddComponent(Of BoxCollider2D)()
		boxCollider2D.isTrigger = True
		boxCollider2D.size = New Vector2(2000F, 400F)
		Return gameObject.transform
	End Function

	' Token: 0x06000D1B RID: 3355 RVA: 0x0008B1D4 File Offset: 0x000895D4
	Private Sub UpdateModeLerp()
		Dim position As Vector3 = Me._position
		Dim vector As Vector3 = PlayerManager.Center
		If Me.moveX Then
			position.x = vector.x
		End If
		If Me.moveY Then
			position.y = vector.y
		End If
		position.x = Mathf.Clamp(position.x, Me.Left, Me.Right)
		position.y = Mathf.Clamp(position.y, Me.Bottom, Me.Top)
		Me._position = Vector3.Lerp(Me._position, position, CupheadTime.Delta * Me.LERP_SPEED)
	End Sub

	' Token: 0x06000D1C RID: 3356 RVA: 0x0008B288 File Offset: 0x00089688
	Private Sub UpdateModeTrapBox()
		Dim position As Vector3 = Me._position
		Dim vector As Vector3 = PlayerManager.CameraCenter
		If Me.moveX Then
			position.x = vector.x
		End If
		If Me.moveY Then
			position.y = vector.y
		End If
		position.x = Mathf.Clamp(position.x, Me.Left, Me.Right)
		position.y = Mathf.Clamp(position.y, Me.Bottom, Me.Top)
		Me._position = Vector3.Lerp(Me._position, position, Time.deltaTime * Me.LERP_SPEED)
	End Sub

	' Token: 0x06000D1D RID: 3357 RVA: 0x0008B338 File Offset: 0x00089738
	Private Sub UpdateModeRelative()
		Dim vector As Vector2 = Me._position
		Dim vector2 As Vector2 = New Vector2(0F, 0F)
		vector2.x = MathUtils.GetPercentage(CSng(Level.Current.Left), CSng(Level.Current.Right), PlayerManager.Center.x)
		vector2.y = MathUtils.GetPercentage(CSng(Level.Current.Ground), CSng(Level.Current.Ceiling), PlayerManager.Center.y)
		If Me.moveX Then
			vector.x = Mathf.Lerp(Me.Left, Me.Right, vector2.x)
		End If
		If Me.moveY Then
			vector.y = Mathf.Lerp(Me.Bottom, Me.Top, vector2.y)
		End If
		vector.x = Mathf.Clamp(vector.x, Me.Left, Me.Right)
		vector.y = Mathf.Clamp(vector.y, Me.Bottom, Me.Top)
		Me._position = Vector3.Lerp(Me._position, vector, CupheadTime.Delta * 5F)
	End Sub

	' Token: 0x06000D1E RID: 3358 RVA: 0x0008B47C File Offset: 0x0008987C
	Private Sub UpdateModeRelativeRook()
		Dim vector As Vector2 = Me._position
		Dim vector2 As Vector2 = New Vector2(0F, 0F)
		vector2.x = MathUtils.GetPercentage(CSng(Level.Current.Left), CSng(Level.Current.Right), PlayerManager.TopPlayerPosition.x)
		vector2.y = PlayerManager.TopPlayerPosition.y
		If Me.moveX Then
			vector.x = Mathf.Lerp(Me.Left, Me.Right, vector2.x)
		End If
		vector.y = Mathf.Lerp(Me.Bottom, Me.Top, Mathf.InverseLerp(200F, 400F, vector2.y))
		vector.x = Mathf.Clamp(vector.x, Me.Left, Me.Right)
		vector.y = Mathf.Clamp(vector.y, Me.Bottom, Me.Top)
		Me._position = New Vector3(Mathf.Lerp(Me._position.x, vector.x, CupheadTime.Delta * 5F), Mathf.Lerp(Me._position.y, vector.y, CupheadTime.Delta * 2.5F))
	End Sub

	' Token: 0x06000D1F RID: 3359 RVA: 0x0008B5D8 File Offset: 0x000899D8
	Private Sub UpdateModeRelativeRumRunners()
		Dim vector As Vector2 = Me._position
		Dim vector2 As Vector2 = New Vector2(0F, 0F)
		vector2.x = MathUtils.GetPercentage(CSng(Level.Current.Left), CSng(Level.Current.Right), PlayerManager.TopPlayerPosition.x)
		vector2.y = PlayerManager.TopPlayerPosition.y
		If Me.moveX Then
			vector.x = Mathf.Lerp(Me.Left, Me.Right, vector2.x)
		End If
		vector.y = If((vector2.y >= 200F), Me.Top, Me.Bottom)
		vector.x = Mathf.Clamp(vector.x, Me.Left, Me.Right)
		vector.y = Mathf.Clamp(vector.y, Me.Bottom, Me.Top)
		Me._position = New Vector3(Mathf.Lerp(Me._position.x, vector.x, CupheadTime.Delta * 5F), Mathf.Lerp(Me._position.y, vector.y, CupheadTime.Delta * 2.5F))
	End Sub

	' Token: 0x06000D20 RID: 3360 RVA: 0x0008B730 File Offset: 0x00089B30
	Private Sub UpdatePlatforming()
		Dim position As Vector3 = Me._position
		Dim vector As Vector3 = PlayerManager.Center
		If Me.moveX AndAlso position.x < vector.x Then
			position.x = vector.x
		End If
		If Me.moveY Then
			position.y = vector.y
		End If
		position.x = Mathf.Clamp(position.x, Me.Left, Me.Right)
		position.y = Mathf.Clamp(position.y, Me.Bottom, Me.Top)
		Me._position = Vector3.Lerp(Me._position, position, CupheadTime.Delta * 5F)
	End Sub

	' Token: 0x06000D21 RID: 3361 RVA: 0x0008B7F4 File Offset: 0x00089BF4
	Public Sub LockCamera(lockCamera As Boolean)
		Me.cameraLocked = lockCamera
	End Sub

	' Token: 0x06000D22 RID: 3362 RVA: 0x0008B7FD File Offset: 0x00089BFD
	Public Sub SetAutoScroll(isScrolling As Boolean)
		Me.autoScrolling = isScrolling
	End Sub

	' Token: 0x06000D23 RID: 3363 RVA: 0x0008B806 File Offset: 0x00089C06
	Public Sub OffsetCamera(cameraOffset As Boolean, leftOffset As Boolean)
		Me.cameraOffset = cameraOffset
		Me.leftOffset = leftOffset
	End Sub

	' Token: 0x06000D24 RID: 3364 RVA: 0x0008B816 File Offset: 0x00089C16
	Public Sub SetAutoscrollSpeedMultiplier(multiplier As Single)
		Me.autoScrollSpeedMultiplier = multiplier
	End Sub

	' Token: 0x06000D25 RID: 3365 RVA: 0x0008B820 File Offset: 0x00089C20
	Private Sub UpdatePath()
		Dim position As Vector3 = Me._position
		Dim vector As Vector2 = PlayerManager.Center
		If Me.stabilizeY Then
			Dim player As AbstractPlayerController = PlayerManager.GetPlayer(PlayerId.PlayerOne)
			Dim player2 As AbstractPlayerController = PlayerManager.GetPlayer(PlayerId.PlayerTwo)
			Dim vector2 As Vector2 = If((Not(player Is Nothing)), player.center, Vector2.zero)
			Dim vector3 As Vector2 = If((Not(player2 Is Nothing)), player2.center, Vector2.zero)
			If vector2.y > position.y + Me.stabilizePaddingTop Then
				vector2.y -= Me.stabilizePaddingTop
			ElseIf vector2.y < position.y - Me.stabilizePaddingBottom Then
				vector2.y += Me.stabilizePaddingBottom
			Else
				vector2.y = position.y
			End If
			If vector3.y > position.y + Me.stabilizePaddingTop Then
				vector3.y -= Me.stabilizePaddingTop
			ElseIf vector3.y < position.y - Me.stabilizePaddingBottom Then
				vector3.y += Me.stabilizePaddingBottom
			Else
				vector3.y = position.y
			End If
			If player IsNot Nothing AndAlso Not player.IsDead AndAlso player2 IsNot Nothing AndAlso Not player2.IsDead Then
				vector = (vector2 + vector3) / 2F
			ElseIf player IsNot Nothing AndAlso Not player.IsDead Then
				vector = vector2
			ElseIf player2 IsNot Nothing AndAlso Not player2.IsDead Then
				vector = vector3
			End If
		End If
		If Me.cameraOffset Then
			Dim num As Single = If((Not Me.leftOffset), (-500F), 500F)
			Me.targetPos = New Vector3(vector.x + num, vector.y)
		Else
			Me.targetPos = vector
		End If
		Dim vector4 As Vector3 = Me.path.GetClosestPoint(Me._position, Me.targetPos, Me.moveX, Me.moveY)
		Dim num2 As Single = (vector4 - position).magnitude / CupheadTime.Delta
		Dim num3 As Single = Mathf.Max(Me._speedLastFrame + 5000F * CupheadTime.Delta, 1000F)
		If num2 > num3 Then
			vector4 = position + (vector4 - position).normalized * num3 * CupheadTime.Delta
		End If
		Me._speedLastFrame = Mathf.Min(num2, num3)
		If Me.pathMovesOnlyForward Then
			Dim closestNormalizedPoint As Single = Me.path.GetClosestNormalizedPoint(Me._position, vector4, Me.moveX, Me.moveY)
			If closestNormalizedPoint < Me._minPathValue Then
				Return
			End If
		End If
		position.x = vector4.x
		position.y = vector4.y
		If Not Me.cameraLocked Then
			If Not Me.autoScrolling Then
				Me._position = Vector3.Lerp(Me._position, position, CupheadTime.Delta * 15F)
			Else
				Dim vector5 As Vector3 = New Vector3(MyBase.transform.position.x + 500F, MyBase.transform.position.y)
				Dim vector6 As Vector3 = Me.path.GetClosestPoint(Me._position, vector5, Me.moveX, Me.moveY)
				Dim num4 As Single = 200F * Me.autoScrollSpeedMultiplier
				Me._position = Vector3.MoveTowards(Me._position, vector6, CupheadTime.Delta * num4)
			End If
		End If
		If Me.pathMovesOnlyForward Then
			Me._minPathValue = Me.path.GetClosestNormalizedPoint(Me._position, Me._position, Me.moveX, Me.moveY)
		End If
	End Sub

	' Token: 0x06000D26 RID: 3366 RVA: 0x0008BC80 File Offset: 0x0008A080
	Public Iterator Function rotate_camera() As IEnumerator
		Dim time As Single = 2F
		Dim t As Single = 0F
		While True
			t += CupheadTime.Delta
			Dim phase As Single = Mathf.Sin(t / time)
			MyBase.transform.localRotation = Quaternion.Euler(New Vector3(0F, 0F, phase * 1F))
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06000D27 RID: 3367 RVA: 0x0008BC9C File Offset: 0x0008A09C
	Public Sub SetRotation(amount As Single)
		MyBase.transform.SetEulerAngles(Nothing, Nothing, New Single?(amount))
	End Sub

	' Token: 0x06000D28 RID: 3368 RVA: 0x0008BCCC File Offset: 0x0008A0CC
	Public Iterator Function change_zoom_cr(newSize As Single, time As Single) As IEnumerator
		Dim t As Single = 0F
		While t < time
			Dim val As Single = t / time
			MyBase.zoom = Mathf.Lerp(MyBase.zoom, newSize, val)
			t += CupheadTime.Delta
			Yield Nothing
		End While
		MyBase.zoom = newSize
		Yield Nothing
		Return
	End Function

	' Token: 0x06000D29 RID: 3369 RVA: 0x0008BCF8 File Offset: 0x0008A0F8
	Public Iterator Function slide_camera_cr(slideAmount As Vector3, time As Single) As IEnumerator
		Dim t As Single = 0F
		Dim start As Vector3 = Me._position
		While t < time
			t += CupheadTime.Delta
			Dim val As Single = EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, 0F, 1F, t / time)
			Me._position = Vector3.Lerp(start, slideAmount, val)
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06000D2A RID: 3370 RVA: 0x0008BD21 File Offset: 0x0008A121
	Public Sub ChangeHorizontalBounds(left As Integer, right As Integer)
		Me.bounds.left = left
		Me.bounds.right = right
	End Sub

	' Token: 0x06000D2B RID: 3371 RVA: 0x0008BD3B File Offset: 0x0008A13B
	Public Sub ChangeVerticalBounds(top As Integer, bottom As Integer)
		Me.bounds.top = top
		Me.bounds.bottom = bottom
	End Sub

	' Token: 0x06000D2C RID: 3372 RVA: 0x0008BD55 File Offset: 0x0008A155
	Public Sub ChangeCameraMode(mode As CupheadLevelCamera.Mode)
		Me.mode = mode
	End Sub

	' Token: 0x06000D2D RID: 3373 RVA: 0x0008BD5E File Offset: 0x0008A15E
	Public Sub SetPosition(pos As Vector3)
		Me._position = pos
		MyBase.transform.position = pos
	End Sub

	' Token: 0x0400166F RID: 5743
	Public Const EDITOR_PATH As String = "Assets/_CUPHEAD/Prefabs/Camera/LevelCamera.prefab"

	' Token: 0x04001670 RID: 5744
	Public Const AUTOSCROLL_SPEED As Single = 200F

	' Token: 0x04001675 RID: 5749
	Private leftOffset As Boolean

	' Token: 0x04001676 RID: 5750
	Private Const BOUND_COLLIDER_SIZE As Single = 400F

	' Token: 0x04001677 RID: 5751
	Private Const BORDER_THICKNESS As Single = 1000F

	' Token: 0x04001678 RID: 5752
	Private Const CENTER_SPEED As Single = 10F

	' Token: 0x04001679 RID: 5753
	Private Const AUTOSCROLL_CHECK As Single = 500F

	' Token: 0x0400167A RID: 5754
	Private Const OFFSET_AMOUNT As Single = 500F

	' Token: 0x0400167B RID: 5755
	Private Const THREE_SIXTY As Single = 360F

	' Token: 0x0400167C RID: 5756
	Private moveX As Boolean

	' Token: 0x0400167D RID: 5757
	Private moveY As Boolean

	' Token: 0x0400167E RID: 5758
	Private stabilizeY As Boolean

	' Token: 0x0400167F RID: 5759
	Private stabilizePaddingTop As Single

	' Token: 0x04001680 RID: 5760
	Private stabilizePaddingBottom As Single

	' Token: 0x04001681 RID: 5761
	Private targetPos As Vector3

	' Token: 0x04001682 RID: 5762
	Private mode As CupheadLevelCamera.Mode

	' Token: 0x04001683 RID: 5763
	Private bounds As Level.Bounds

	' Token: 0x04001684 RID: 5764
	Private collidersRoot As Transform

	' Token: 0x04001685 RID: 5765
	Private path As VectorPath

	' Token: 0x04001686 RID: 5766
	Private pathMovesOnlyForward As Boolean

	' Token: 0x04001687 RID: 5767
	Public enablePathScrubbing As Boolean

	' Token: 0x04001688 RID: 5768
	<Range(0F, 1F)>
	Public scrub As Single

	' Token: 0x0400168D RID: 5773
	Private leftCollider As Transform

	' Token: 0x0400168E RID: 5774
	Private rightCollider As Transform

	' Token: 0x0400168F RID: 5775
	Private topCollider As Transform

	' Token: 0x04001690 RID: 5776
	Private bottomCollider As Transform

	' Token: 0x04001691 RID: 5777
	<HideInInspector()>
	Public LERP_SPEED As Single = 2F

	' Token: 0x04001692 RID: 5778
	Private Const RELATIVE_LERP_SPEED As Single = 5F

	' Token: 0x04001693 RID: 5779
	Private Const ROOK_SCROLL_UP_MIN As Single = 200F

	' Token: 0x04001694 RID: 5780
	Private Const ROOK_SCROLL_UP_MAX As Single = 400F

	' Token: 0x04001695 RID: 5781
	Private Const V_LERP_SLOW_SPEED As Single = 2.5F

	' Token: 0x04001696 RID: 5782
	Private Const RUMRUNNERS_SCROLL_UP_THRESHOLD As Single = 200F

	' Token: 0x04001697 RID: 5783
	Private Const PLATFORMING_LERP_SPEED As Single = 5F

	' Token: 0x04001698 RID: 5784
	Private Const PATH_LERP_SPEED As Single = 15F

	' Token: 0x04001699 RID: 5785
	Private Const PATH_MAX_SPEED_BEFORE_ACCELERATION As Single = 1000F

	' Token: 0x0400169A RID: 5786
	Private Const PATH_ACCELERATION As Single = 5000F

	' Token: 0x0400169B RID: 5787
	Private _minPathValue As Single = Single.MinValue

	' Token: 0x0400169C RID: 5788
	Private _speedLastFrame As Single

	' Token: 0x020003D9 RID: 985
	Public Enum Mode
		' Token: 0x0400169E RID: 5790
		Lerp
		' Token: 0x0400169F RID: 5791
		TrapBox
		' Token: 0x040016A0 RID: 5792
		Relative
		' Token: 0x040016A1 RID: 5793
		Platforming
		' Token: 0x040016A2 RID: 5794
		Path
		' Token: 0x040016A3 RID: 5795
		RelativeRook
		' Token: 0x040016A4 RID: 5796
		RelativeRumRunners
		' Token: 0x040016A5 RID: 5797
		[Static] = 10000
	End Enum
End Class
