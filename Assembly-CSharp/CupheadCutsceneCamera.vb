Imports System
Imports UnityEngine

' Token: 0x020003D6 RID: 982
Public Class CupheadCutsceneCamera
	Inherits AbstractCupheadGameCamera

	' Token: 0x1700022D RID: 557
	' (get) Token: 0x06000CF1 RID: 3313 RVA: 0x0008A519 File Offset: 0x00088919
	' (set) Token: 0x06000CF2 RID: 3314 RVA: 0x0008A520 File Offset: 0x00088920
	Public Shared Property Current As CupheadCutsceneCamera

	' Token: 0x1700022E RID: 558
	' (get) Token: 0x06000CF3 RID: 3315 RVA: 0x0008A528 File Offset: 0x00088928
	Public Overrides ReadOnly Property OrthographicSize As Single
		Get
			Return 360F
		End Get
	End Property

	' Token: 0x06000CF4 RID: 3316 RVA: 0x0008A52F File Offset: 0x0008892F
	Protected Overrides Sub Awake()
		MyBase.Awake()
		CupheadCutsceneCamera.Current = Me
	End Sub

	' Token: 0x06000CF5 RID: 3317 RVA: 0x0008A53D File Offset: 0x0008893D
	Private Sub OnDestroy()
		If CupheadCutsceneCamera.Current Is Me Then
			CupheadCutsceneCamera.Current = Nothing
		End If
	End Sub

	' Token: 0x06000CF6 RID: 3318 RVA: 0x0008A555 File Offset: 0x00088955
	Protected Overrides Sub LateUpdate()
		MyBase.LateUpdate()
		MyBase.Move()
	End Sub

	' Token: 0x06000CF7 RID: 3319 RVA: 0x0008A563 File Offset: 0x00088963
	Public Sub SetPosition(newPos As Vector3)
		Me._position = newPos
	End Sub

	' Token: 0x06000CF8 RID: 3320 RVA: 0x0008A56C File Offset: 0x0008896C
	Public Sub Init()
		MyBase.enabled = True
		Dim texture2D As Texture2D = Me.CreateBorderTexture()
		If Not Me.noBars Then
			Dim transform As Transform = New GameObject("Border").transform
			Me.CreateBorderRenderer(texture2D, transform, "Left")
			Me.CreateBorderRenderer(texture2D, transform, "Right")
			Me.CreateBorderRenderer(texture2D, transform, "Top")
			Me.CreateBorderRenderer(texture2D, transform, "Bottom")
		End If
		If Not Me.noShake Then
			If Me.minimalShake Then
				MyBase.StartSmoothShake(3F, 1.5F, 6)
			Else
				MyBase.StartSmoothShake(4F, 0.75F, 8)
			End If
		End If
	End Sub

	' Token: 0x06000CF9 RID: 3321 RVA: 0x0008A618 File Offset: 0x00088A18
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
							zero = New Vector2(3280F, 1000F)
							zero2 = New Vector2(0F, -360F)
							zero3 = New Vector2(0.5F, 1F)
						End If
					Else
						zero = New Vector2(3280F, 1000F)
						zero2 = New Vector2(0F, 360F)
						zero3 = New Vector2(0.5F, 0F)
					End If
				Else
					zero = New Vector2(1000F, 2720F)
					zero2 = New Vector2(640F, 0F)
					zero3 = New Vector2(0F, 0.5F)
				End If
			Else
				zero = New Vector2(1000F, 2720F)
				zero2 = New Vector2(-640F, 0F)
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

	' Token: 0x06000CFA RID: 3322 RVA: 0x0008A804 File Offset: 0x00088C04
	Private Function CreateBorderTexture() As Texture2D
		Dim texture2D As Texture2D = New Texture2D(1, 1)
		texture2D.filterMode = FilterMode.Point
		texture2D.SetPixel(0, 0, Color.black)
		texture2D.Apply()
		Return texture2D
	End Function

	' Token: 0x0400165E RID: 5726
	Public Const EDITOR_PATH As String = "Assets/_CUPHEAD/Prefabs/Camera/CutsceneCamera.prefab"

	' Token: 0x0400165F RID: 5727
	Private Const BOUND_COLLIDER_SIZE As Single = 400F

	' Token: 0x04001660 RID: 5728
	Private Const BORDER_THICKNESS As Single = 1000F

	' Token: 0x04001661 RID: 5729
	Public Const LEFT As Single = -640F

	' Token: 0x04001662 RID: 5730
	Public Const RIGHT As Single = 640F

	' Token: 0x04001663 RID: 5731
	Public Const BOTTOM As Single = -360F

	' Token: 0x04001664 RID: 5732
	Public Const TOP As Single = 360F

	' Token: 0x04001665 RID: 5733
	Public noShake As Boolean

	' Token: 0x04001666 RID: 5734
	Public minimalShake As Boolean

	' Token: 0x04001667 RID: 5735
	Public noBars As Boolean

	' Token: 0x020003D7 RID: 983
	Public Enum Mode
		' Token: 0x04001669 RID: 5737
		Lerp
		' Token: 0x0400166A RID: 5738
		TrapBox
		' Token: 0x0400166B RID: 5739
		Relative
		' Token: 0x0400166C RID: 5740
		Platforming
		' Token: 0x0400166D RID: 5741
		[Static] = 10000
	End Enum
End Class
