Imports System
Imports System.Collections
Imports System.Diagnostics
Imports UnityEngine
Imports UnityStandardAssets.ImageEffects

' Token: 0x020003D2 RID: 978
Public MustInherit Class AbstractCupheadGameCamera
	Inherits AbstractCupheadCamera

	' Token: 0x17000227 RID: 551
	' (get) Token: 0x06000CC0 RID: 3264 RVA: 0x0008953E File Offset: 0x0008793E
	' (set) Token: 0x06000CC1 RID: 3265 RVA: 0x00089546 File Offset: 0x00087946
	Public Property zoom As Single
		Get
			Return Me._zoom
		End Get
		Protected Set(value As Single)
			Me._zoom = value
			MyBase.camera.orthographicSize = Me.OrthographicSize / Me._zoom
		End Set
	End Property

	' Token: 0x17000228 RID: 552
	' (get) Token: 0x06000CC2 RID: 3266 RVA: 0x00089567 File Offset: 0x00087967
	' (set) Token: 0x06000CC3 RID: 3267 RVA: 0x0008956F File Offset: 0x0008796F
	Public Property isShaking As Boolean

	' Token: 0x17000229 RID: 553
	' (get) Token: 0x06000CC4 RID: 3268
	Public MustOverride ReadOnly Property OrthographicSize As Single

	' Token: 0x1700022A RID: 554
	' (get) Token: 0x06000CC5 RID: 3269 RVA: 0x00089578 File Offset: 0x00087978
	Public ReadOnly Property Width As Single
		Get
			Return 1.7777778F * Me.OrthographicSize * Me.zoom
		End Get
	End Property

	' Token: 0x1700022B RID: 555
	' (get) Token: 0x06000CC6 RID: 3270 RVA: 0x0008958D File Offset: 0x0008798D
	Public ReadOnly Property Height As Single
		Get
			Return Me.OrthographicSize * Me.zoom
		End Get
	End Property

	' Token: 0x1700022C RID: 556
	' (get) Token: 0x06000CC7 RID: 3271 RVA: 0x0008959C File Offset: 0x0008799C
	Public ReadOnly Property Top As Single
		Get
			Return MyBase.camera.ScreenToWorldPoint(New Vector3(0F, CSng(Screen.height), 0F)).y
		End Get
	End Property

	' Token: 0x06000CC8 RID: 3272 RVA: 0x000895D1 File Offset: 0x000879D1
	Protected Overrides Sub Awake()
		MyBase.Awake()
		MyBase.camera.orthographicSize = Me.OrthographicSize
		Me._blurEffect = MyBase.GetComponent(Of BlurOptimized)()
		Me._blurEffect.enabled = False
		MyBase.camera.clearFlags = CameraClearFlags.Color
	End Sub

	' Token: 0x06000CC9 RID: 3273 RVA: 0x0008960E File Offset: 0x00087A0E
	Protected Sub Move()
		MyBase.transform.position = Me._position + Me._shakeAdd + Me._floatAdd
	End Sub

	' Token: 0x14000025 RID: 37
	' (add) Token: 0x06000CCA RID: 3274 RVA: 0x00089638 File Offset: 0x00087A38
	' (remove) Token: 0x06000CCB RID: 3275 RVA: 0x00089670 File Offset: 0x00087A70
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnShakeEvent As AbstractCupheadGameCamera.OnShakeHandler

	' Token: 0x06000CCC RID: 3276 RVA: 0x000896A8 File Offset: 0x00087AA8
	Public Sub Shake(amount As Single, time As Single, Optional bypassEvent As Boolean = False)
		Me.isShaking = True
		Me.ResetShake()
		Me.shakeAmount = amount
		If Not bypassEvent AndAlso Me.OnShakeEvent IsNot Nothing Then
			Me.OnShakeEvent(amount, time)
		End If
		Me.shakeCoroutine = Me.falloffShake_cr(amount, time)
		MyBase.StartCoroutine(Me.shakeCoroutine)
	End Sub

	' Token: 0x06000CCD RID: 3277 RVA: 0x00089702 File Offset: 0x00087B02
	Public Sub StartShake(amount As Single)
		Me.ResetShake()
		Me.shakeAmount = amount
		Me.shakeCoroutine = Me.endlessShake_cr(amount)
		MyBase.StartCoroutine(Me.shakeCoroutine)
	End Sub

	' Token: 0x06000CCE RID: 3278 RVA: 0x0008972B File Offset: 0x00087B2B
	Public Sub EndShake(time As Single)
		Me.ResetShake()
		If time > 0F Then
			Me.shakeCoroutine = Me.falloffShake_cr(Me.shakeAmount, time)
			MyBase.StartCoroutine(Me.shakeCoroutine)
		End If
	End Sub

	' Token: 0x06000CCF RID: 3279 RVA: 0x0008975E File Offset: 0x00087B5E
	Public Sub StartSmoothShake(amount As Single, period As Single, octaves As Integer)
		Me.ResetShake()
		Me.shakeCoroutine = Me.endlessSmoothShake_cr(amount, period, octaves)
		MyBase.StartCoroutine(Me.shakeCoroutine)
	End Sub

	' Token: 0x06000CD0 RID: 3280 RVA: 0x00089782 File Offset: 0x00087B82
	Public Sub ResetShake()
		If Me.shakeCoroutine IsNot Nothing Then
			MyBase.StopCoroutine(Me.shakeCoroutine)
		End If
		Me.shakeCoroutine = Nothing
		Me._shakeAdd = Vector3.zero
	End Sub

	' Token: 0x06000CD1 RID: 3281 RVA: 0x000897B0 File Offset: 0x00087BB0
	Private Iterator Function falloffShake_cr(amount As Single, time As Single) As IEnumerator
		Dim t As Single = 0F
		Dim halfAmount As Single = amount / 2F
		While t < time
			Dim val As Single = 1F - t / time
			Me.shakeAmount = amount * val
			Dim x As Single = Global.UnityEngine.Random.Range(-halfAmount, halfAmount)
			Dim y As Single = Global.UnityEngine.Random.Range(-halfAmount, halfAmount)
			Me._shakeAdd = New Vector3(x * val, y * val, 0F)
			t += CupheadTime.Delta
			Yield Nothing
			If PauseManager.state = PauseManager.State.Paused Then
				While PauseManager.state = PauseManager.State.Paused
					Yield Nothing
				End While
			End If
		End While
		Me.ResetShake()
		Me.isShaking = False
		Return
	End Function

	' Token: 0x06000CD2 RID: 3282 RVA: 0x000897DC File Offset: 0x00087BDC
	Private Iterator Function endlessShake_cr(amount As Single) As IEnumerator
		Dim halfAmount As Single = amount / 2F
		While True
			Dim x As Single = Global.UnityEngine.Random.Range(-halfAmount, halfAmount)
			Dim y As Single = Global.UnityEngine.Random.Range(-halfAmount, halfAmount)
			Me._shakeAdd = New Vector3(x, y, 0F)
			Yield Nothing
			If PauseManager.state = PauseManager.State.Paused Then
				While PauseManager.state = PauseManager.State.Paused
					Yield Nothing
				End While
			End If
		End While
		Return
	End Function

	' Token: 0x06000CD3 RID: 3283 RVA: 0x00089800 File Offset: 0x00087C00
	Private Iterator Function endlessSmoothShake_cr(amount As Single, period As Single, octaves As Integer) As IEnumerator
		Dim t As Single = 0F
		While True
			t += CupheadTime.Delta
			Dim x As Single = 0F
			Dim y As Single = 0F
			Dim scale As Single = 1F
			For i As Integer = 0 To octaves - 1
				scale = Mathf.Pow(2F, CSng(i))
				x += Mathf.PerlinNoise((t + 1000F) * scale / period, 0F) * amount / scale
				y += Mathf.PerlinNoise((t + 2000F) * scale / period, 0F) * amount / scale
			Next
			Me._shakeAdd.x = x
			Me._shakeAdd.y = y
			Me._shakeAdd.z = 0F
			Yield Nothing
			If PauseManager.state = PauseManager.State.Paused Then
				While PauseManager.state = PauseManager.State.Paused
					Yield Nothing
				End While
			End If
		End While
		Return
	End Function

	' Token: 0x06000CD4 RID: 3284 RVA: 0x00089830 File Offset: 0x00087C30
	Public Sub StartFloat(amount As Single, time As Single)
		Me.ResetFloat()
		Me.floatCoroutine = Me.float_cr(amount, time)
		MyBase.StartCoroutine(Me.floatCoroutine)
	End Sub

	' Token: 0x06000CD5 RID: 3285 RVA: 0x00089853 File Offset: 0x00087C53
	Public Sub EndFloat()
		Me.ResetFloat()
	End Sub

	' Token: 0x06000CD6 RID: 3286 RVA: 0x0008985B File Offset: 0x00087C5B
	Public Sub ResetFloat()
		If Me.floatCoroutine IsNot Nothing Then
			MyBase.StopCoroutine(Me.floatCoroutine)
		End If
		Me.floatCoroutine = Nothing
		Me._floatAdd = Vector3.zero
	End Sub

	' Token: 0x06000CD7 RID: 3287 RVA: 0x00089886 File Offset: 0x00087C86
	Public Sub SetManualFloat(value As Vector3)
		Me._floatAdd = value
	End Sub

	' Token: 0x06000CD8 RID: 3288 RVA: 0x00089890 File Offset: 0x00087C90
	Private Iterator Function float_cr(amount As Single, time As Single) As IEnumerator
		Me.floatState = AbstractCupheadGameCamera.FloatState.Float
		Dim t As Single = 0F
		Dim bottom As Single = 0F
		While True
			t = 0F
			While t < time
				Dim val As Single = t / time
				Dim y As Single = EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, bottom, amount, val)
				Me._floatAdd = New Vector3(0F, y, 0F)
				t += CupheadTime.Delta
				Yield Nothing
			End While
			t = 0F
			While t < time
				Dim val2 As Single = t / time
				Dim y2 As Single = EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, amount, bottom, val2)
				Me._floatAdd = New Vector3(0F, y2, 0F)
				t += CupheadTime.Delta
				Yield Nothing
			End While
			If Me.floatState = AbstractCupheadGameCamera.FloatState.[Stop] Then
				Me.ResetFloat()
			End If
		End While
		Return
	End Function

	' Token: 0x06000CD9 RID: 3289 RVA: 0x000898B9 File Offset: 0x00087CB9
	Public Sub Zoom(newZoom As Single, time As Single, ease As EaseUtils.EaseType)
		Me.StopZoom()
		Me.zoomCoroutine = Me.zoom_cr(newZoom, time, ease)
		MyBase.StartCoroutine(Me.zoomCoroutine)
	End Sub

	' Token: 0x06000CDA RID: 3290 RVA: 0x000898DD File Offset: 0x00087CDD
	Private Sub StopZoom()
		If Me.zoomCoroutine IsNot Nothing Then
			MyBase.StopCoroutine(Me.zoomCoroutine)
		End If
		Me.zoomCoroutine = Nothing
	End Sub

	' Token: 0x06000CDB RID: 3291 RVA: 0x00089900 File Offset: 0x00087D00
	Private Iterator Function zoom_cr(newZoom As Single, time As Single, ease As EaseUtils.EaseType) As IEnumerator
		Dim oldZoom As Single = Me.zoom
		Dim t As Single = 0F
		While t < time
			Dim val As Single = t / time
			Me.zoom = EaseUtils.Ease(ease, oldZoom, newZoom, val)
			t += CupheadTime.Delta
			Yield Nothing
			If PauseManager.state = PauseManager.State.Paused Then
				While PauseManager.state = PauseManager.State.Paused
					Yield Nothing
				End While
			End If
		End While
		Me.zoom = newZoom
		Yield Nothing
		Return
	End Function

	' Token: 0x06000CDC RID: 3292 RVA: 0x00089930 File Offset: 0x00087D30
	Public Sub StartBlur()
		Me.maxBlur = 1.2F
		Me.StartBlurCoroutine(2F, 0.15F, False)
	End Sub

	' Token: 0x06000CDD RID: 3293 RVA: 0x0008994E File Offset: 0x00087D4E
	Public Sub StartBlur(time As Single)
		Me.maxBlur = 1.2F
		Me.StartBlurCoroutine(2F, time, False)
	End Sub

	' Token: 0x06000CDE RID: 3294 RVA: 0x00089968 File Offset: 0x00087D68
	Public Sub StartBlur(time As Single, amount As Single)
		Me.maxBlur = amount
		Me.StartBlurCoroutine(2F, time, False)
	End Sub

	' Token: 0x06000CDF RID: 3295 RVA: 0x0008997E File Offset: 0x00087D7E
	Public Sub EndBlur()
		Me.maxBlur = 1.2F
		Me.StartBlurCoroutine(0F, 0.15F, True)
	End Sub

	' Token: 0x06000CE0 RID: 3296 RVA: 0x0008999C File Offset: 0x00087D9C
	Public Sub EndBlur(time As Single)
		Me.maxBlur = 1.2F
		Me.StartBlurCoroutine(0F, time, True)
	End Sub

	' Token: 0x06000CE1 RID: 3297 RVA: 0x000899B6 File Offset: 0x00087DB6
	Public Sub EndBlur(time As Single, amount As Single)
		Me.maxBlur = amount
		Me.StartBlurCoroutine(0F, time, True)
	End Sub

	' Token: 0x06000CE2 RID: 3298 RVA: 0x000899CC File Offset: 0x00087DCC
	Private Sub StartBlurCoroutine(amount As Single, time As Single, disableBlurWhenComplete As Boolean)
		Me.StopBlurCoroutine()
		Me._blurCoroutine = Me.blur_cr(amount, time, disableBlurWhenComplete)
		MyBase.StartCoroutine(Me._blurCoroutine)
	End Sub

	' Token: 0x06000CE3 RID: 3299 RVA: 0x000899F0 File Offset: 0x00087DF0
	Private Sub StopBlurCoroutine()
		If Me._blurCoroutine IsNot Nothing Then
			MyBase.StopCoroutine(Me._blurCoroutine)
		End If
		Me._blurCoroutine = Nothing
	End Sub

	' Token: 0x06000CE4 RID: 3300 RVA: 0x00089A10 File Offset: 0x00087E10
	Private Sub UpdateBlur()
		Me._blurEffect.blurSize = Mathf.Lerp(0F, Me.maxBlur, Me._currentBlurAmount)
	End Sub

	' Token: 0x06000CE5 RID: 3301 RVA: 0x00089A34 File Offset: 0x00087E34
	Protected Iterator Function blur_cr([end] As Single, time As Single, disableBlurWhenComplete As Boolean) As IEnumerator
		Dim start As Single = Me._currentBlurAmount
		Me._blurEffect.enabled = True
		Dim t As Single = 0F
		While t < time
			Dim val As Single = t / time
			Me._currentBlurAmount = Mathf.Lerp(start, [end], val)
			Me.UpdateBlur()
			t += Time.deltaTime
			Yield Nothing
		End While
		Me._currentBlurAmount = [end]
		Me.UpdateBlur()
		Yield Nothing
		If disableBlurWhenComplete Then
			Me._blurEffect.enabled = False
		End If
		Return
	End Function

	' Token: 0x04001643 RID: 5699
	Private _shakeAdd As Vector3

	' Token: 0x04001644 RID: 5700
	Private _floatAdd As Vector3

	' Token: 0x04001645 RID: 5701
	Protected _position As Vector3

	' Token: 0x04001646 RID: 5702
	Protected _blurEffect As BlurOptimized

	' Token: 0x04001647 RID: 5703
	Private _zoom As Single = 1F

	' Token: 0x04001649 RID: 5705
	Private shakeCoroutine As IEnumerator

	' Token: 0x0400164A RID: 5706
	Private shakeAmount As Single

	' Token: 0x0400164C RID: 5708
	Private floatState As AbstractCupheadGameCamera.FloatState

	' Token: 0x0400164D RID: 5709
	Private floatCoroutine As IEnumerator

	' Token: 0x0400164E RID: 5710
	Private zoomCoroutine As IEnumerator

	' Token: 0x0400164F RID: 5711
	Private Const BLUR_TIME_START As Single = 0.15F

	' Token: 0x04001650 RID: 5712
	Private Const BLUR_TIME_END As Single = 0.15F

	' Token: 0x04001651 RID: 5713
	Private _blurCoroutine As IEnumerator

	' Token: 0x04001652 RID: 5714
	Private _currentBlurAmount As Single

	' Token: 0x04001653 RID: 5715
	Private maxBlur As Single = 1.2F

	' Token: 0x020003D3 RID: 979
	' (Invoke) Token: 0x06000CE7 RID: 3303
	Public Delegate Sub OnShakeHandler(amount As Single, time As Single)

	' Token: 0x020003D4 RID: 980
	Private Enum FloatState
		' Token: 0x04001655 RID: 5717
		[Stop]
		' Token: 0x04001656 RID: 5718
		Float
	End Enum
End Class
