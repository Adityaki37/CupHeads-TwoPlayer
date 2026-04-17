Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x0200035C RID: 860
Public MustInherit Class AbstractMonoBehaviour
	Inherits MonoBehaviour

	' Token: 0x170001E6 RID: 486
	' (get) Token: 0x06000955 RID: 2389 RVA: 0x000066D1 File Offset: 0x00004AD1
	Public ReadOnly Property baseTransform As Transform
		Get
			Return MyBase.transform
		End Get
	End Property

	' Token: 0x170001E7 RID: 487
	' (get) Token: 0x06000956 RID: 2390 RVA: 0x000066D9 File Offset: 0x00004AD9
	Public ReadOnly Property transform As Transform
		Get
			If Not Me._transformCached Then
				Me._transform = Me.baseTransform
				Me._transformCached = True
			End If
			Return Me._transform
		End Get
	End Property

	' Token: 0x170001E8 RID: 488
	' (get) Token: 0x06000957 RID: 2391 RVA: 0x000066FF File Offset: 0x00004AFF
	Public ReadOnly Property baseRectTransform As RectTransform
		Get
			Return TryCast(MyBase.transform, RectTransform)
		End Get
	End Property

	' Token: 0x170001E9 RID: 489
	' (get) Token: 0x06000958 RID: 2392 RVA: 0x0000670C File Offset: 0x00004B0C
	Public ReadOnly Property rectTransform As RectTransform
		Get
			If Me._rectTransform Is Nothing Then
				Me._rectTransform = Me.baseRectTransform
			End If
			Return Me._rectTransform
		End Get
	End Property

	' Token: 0x170001EA RID: 490
	' (get) Token: 0x06000959 RID: 2393 RVA: 0x00006731 File Offset: 0x00004B31
	Public ReadOnly Property baseRigidbody As Rigidbody
		Get
			Return MyBase.GetComponent(Of Rigidbody)()
		End Get
	End Property

	' Token: 0x170001EB RID: 491
	' (get) Token: 0x0600095A RID: 2394 RVA: 0x00006739 File Offset: 0x00004B39
	Public ReadOnly Property rigidbody As Rigidbody
		Get
			If Me._rigidbody Is Nothing Then
				Me._rigidbody = Me.baseRigidbody
			End If
			Return Me._rigidbody
		End Get
	End Property

	' Token: 0x170001EC RID: 492
	' (get) Token: 0x0600095B RID: 2395 RVA: 0x0000675E File Offset: 0x00004B5E
	Public ReadOnly Property baseRigidbody2D As Rigidbody2D
		Get
			Return MyBase.GetComponent(Of Rigidbody2D)()
		End Get
	End Property

	' Token: 0x170001ED RID: 493
	' (get) Token: 0x0600095C RID: 2396 RVA: 0x00006766 File Offset: 0x00004B66
	Public ReadOnly Property rigidbody2D As Rigidbody2D
		Get
			If Me._rigidbody2D Is Nothing Then
				Me._rigidbody2D = Me.baseRigidbody2D
			End If
			Return Me._rigidbody2D
		End Get
	End Property

	' Token: 0x170001EE RID: 494
	' (get) Token: 0x0600095D RID: 2397 RVA: 0x0000678B File Offset: 0x00004B8B
	Public ReadOnly Property baseAnimator As Animator
		Get
			Return MyBase.GetComponent(Of Animator)()
		End Get
	End Property

	' Token: 0x170001EF RID: 495
	' (get) Token: 0x0600095E RID: 2398 RVA: 0x00006793 File Offset: 0x00004B93
	Public ReadOnly Property animator As Animator
		Get
			If Me._animator Is Nothing Then
				Me._animator = Me.baseAnimator
			End If
			Return Me._animator
		End Get
	End Property

	' Token: 0x0600095F RID: 2399 RVA: 0x000067B8 File Offset: 0x00004BB8
	Protected Overridable Sub Awake()
		MyBase.useGUILayout = False
	End Sub

	' Token: 0x06000960 RID: 2400 RVA: 0x000067C1 File Offset: 0x00004BC1
	Protected Overridable Sub Reset()
	End Sub

	' Token: 0x06000961 RID: 2401 RVA: 0x000067C3 File Offset: 0x00004BC3
	Protected Overridable Sub OnDrawGizmos()
	End Sub

	' Token: 0x06000962 RID: 2402 RVA: 0x000067C5 File Offset: 0x00004BC5
	Protected Overridable Sub OnDrawGizmosSelected()
	End Sub

	' Token: 0x06000963 RID: 2403 RVA: 0x000067C8 File Offset: 0x00004BC8
	Public Overridable Function InstantiatePrefab(Of T As MonoBehaviour)() As T
		Dim gameObject As GameObject = Global.UnityEngine.[Object].Instantiate(Of GameObject)(MyBase.gameObject)
		gameObject.name = gameObject.name.Replace("(Clone)", String.Empty)
		Return gameObject.GetComponent(Of T)()
	End Function

	' Token: 0x06000964 RID: 2404 RVA: 0x00006802 File Offset: 0x00004C02
	Public Function FrameDelayedCallback(callback As Action, frames As Integer) As Coroutine
		Return MyBase.StartCoroutine(Me.frameDelayedCallback_cr(callback, frames))
	End Function

	' Token: 0x06000965 RID: 2405 RVA: 0x00006814 File Offset: 0x00004C14
	Public Iterator Function frameDelayedCallback_cr(callback As Action, frames As Integer) As IEnumerator
		For i As Integer = 0 To frames - 1
			Yield Nothing
		Next
		If callback IsNot Nothing Then
			callback()
		End If
		Return
	End Function

	' Token: 0x170001F0 RID: 496
	' (get) Token: 0x06000966 RID: 2406 RVA: 0x00006836 File Offset: 0x00004C36
	Protected ReadOnly Property LocalDeltaTime As Single
		Get
			If Me.ignoreGlobalTime Then
				Return Time.deltaTime
			End If
			Return CupheadTime.Delta(Me.timeLayer)
		End Get
	End Property

	' Token: 0x06000967 RID: 2407 RVA: 0x00006859 File Offset: 0x00004C59
	Public Function TweenValue(start As Single, [end] As Single, time As Single, ease As EaseUtils.EaseType, updateCallback As AbstractMonoBehaviour.TweenUpdateHandler) As Coroutine
		Return MyBase.StartCoroutine(Me.tweenValue_cr(start, [end], time, ease, updateCallback))
	End Function

	' Token: 0x06000968 RID: 2408 RVA: 0x00006870 File Offset: 0x00004C70
	Protected Iterator Function tweenValue_cr(start As Single, [end] As Single, time As Single, ease As EaseUtils.EaseType, updateCallback As AbstractMonoBehaviour.TweenUpdateHandler) As IEnumerator
		Dim t As Single = 0F
		While t < time
			Dim val As Single = t / time
			If updateCallback IsNot Nothing Then
				updateCallback(EaseUtils.Ease(ease, start, [end], val))
			End If
			t += Me.LocalDeltaTime
			Yield Nothing
		End While
		If updateCallback IsNot Nothing Then
			updateCallback([end])
		End If
		Yield Nothing
		Return
	End Function

	' Token: 0x06000969 RID: 2409 RVA: 0x000068B0 File Offset: 0x00004CB0
	Public Function TweenScale(start As Vector2, [end] As Vector2, time As Single, ease As EaseUtils.EaseType) As Coroutine
		Return MyBase.StartCoroutine(Me.tweenScale_cr(start, [end], time, ease, Nothing))
	End Function

	' Token: 0x0600096A RID: 2410 RVA: 0x000068C4 File Offset: 0x00004CC4
	Public Function TweenScale(start As Vector2, [end] As Vector2, time As Single, ease As EaseUtils.EaseType, updateCallback As AbstractMonoBehaviour.TweenUpdateHandler) As Coroutine
		Return MyBase.StartCoroutine(Me.tweenScale_cr(start, [end], time, ease, updateCallback))
	End Function

	' Token: 0x0600096B RID: 2411 RVA: 0x000068DC File Offset: 0x00004CDC
	Private Iterator Function tweenScale_cr(start As Vector2, [end] As Vector2, time As Single, ease As EaseUtils.EaseType, Optional updateCallback As AbstractMonoBehaviour.TweenUpdateHandler = Nothing) As IEnumerator
		Me.transform.SetScale(New Single?(start.x), New Single?(start.y), Nothing)
		Dim t As Single = 0F
		While t < time
			Dim val As Single = t / time
			Dim x As Single = EaseUtils.Ease(ease, start.x, [end].x, val)
			Dim y As Single = EaseUtils.Ease(ease, start.y, [end].y, val)
			Me.transform.SetScale(New Single?(x), New Single?(y), Nothing)
			If updateCallback IsNot Nothing Then
				updateCallback(val)
			End If
			t += Me.LocalDeltaTime
			Yield Nothing
		End While
		Me.transform.SetScale(New Single?([end].x), New Single?([end].y), Nothing)
		If updateCallback IsNot Nothing Then
			updateCallback(1F)
		End If
		Yield Nothing
		Return
	End Function

	' Token: 0x0600096C RID: 2412 RVA: 0x0000691C File Offset: 0x00004D1C
	Public Function TweenPosition(start As Vector2, [end] As Vector2, time As Single, ease As EaseUtils.EaseType) As Coroutine
		Return MyBase.StartCoroutine(Me.tweenPosition_cr(start, [end], time, ease, Nothing))
	End Function

	' Token: 0x0600096D RID: 2413 RVA: 0x00006930 File Offset: 0x00004D30
	Public Function TweenPosition(start As Vector2, [end] As Vector2, time As Single, ease As EaseUtils.EaseType, updateCallback As AbstractMonoBehaviour.TweenUpdateHandler) As Coroutine
		Return MyBase.StartCoroutine(Me.tweenPosition_cr(start, [end], time, ease, updateCallback))
	End Function

	' Token: 0x0600096E RID: 2414 RVA: 0x00006948 File Offset: 0x00004D48
	Private Iterator Function tweenPosition_cr(start As Vector2, [end] As Vector2, time As Single, ease As EaseUtils.EaseType, Optional updateCallback As AbstractMonoBehaviour.TweenUpdateHandler = Nothing) As IEnumerator
		Me.transform.position = start
		Dim t As Single = 0F
		While t < time
			Dim val As Single = t / time
			Dim x As Single = EaseUtils.Ease(ease, start.x, [end].x, val)
			Dim y As Single = EaseUtils.Ease(ease, start.y, [end].y, val)
			Me.transform.SetPosition(New Single?(x), New Single?(y), New Single?(0F))
			If updateCallback IsNot Nothing Then
				updateCallback(val)
			End If
			t += Me.LocalDeltaTime
			Yield Nothing
		End While
		Me.transform.position = [end]
		If updateCallback IsNot Nothing Then
			updateCallback(1F)
		End If
		Yield Nothing
		Return
	End Function

	' Token: 0x0600096F RID: 2415 RVA: 0x00006988 File Offset: 0x00004D88
	Public Function TweenLocalPosition(start As Vector2, [end] As Vector2, time As Single, ease As EaseUtils.EaseType) As Coroutine
		Return MyBase.StartCoroutine(Me.tweenLocalPosition_cr(start, [end], time, ease, Nothing))
	End Function

	' Token: 0x06000970 RID: 2416 RVA: 0x0000699C File Offset: 0x00004D9C
	Public Function TweenLocalPosition(start As Vector2, [end] As Vector2, time As Single, ease As EaseUtils.EaseType, updateCallback As AbstractMonoBehaviour.TweenUpdateHandler) As Coroutine
		Return MyBase.StartCoroutine(Me.tweenLocalPosition_cr(start, [end], time, ease, updateCallback))
	End Function

	' Token: 0x06000971 RID: 2417 RVA: 0x000069B4 File Offset: 0x00004DB4
	Private Iterator Function tweenLocalPosition_cr(start As Vector2, [end] As Vector2, time As Single, ease As EaseUtils.EaseType, Optional updateCallback As AbstractMonoBehaviour.TweenUpdateHandler = Nothing) As IEnumerator
		Me.transform.localPosition = start
		Dim t As Single = 0F
		While t < time
			Dim val As Single = t / time
			Dim x As Single = EaseUtils.Ease(ease, start.x, [end].x, val)
			Dim y As Single = EaseUtils.Ease(ease, start.y, [end].y, val)
			Me.transform.SetLocalPosition(New Single?(x), New Single?(y), New Single?(0F))
			If updateCallback IsNot Nothing Then
				updateCallback(val)
			End If
			t += Me.LocalDeltaTime
			Yield Nothing
		End While
		Me.transform.localPosition = [end]
		If updateCallback IsNot Nothing Then
			updateCallback(1F)
		End If
		Yield Nothing
		Return
	End Function

	' Token: 0x06000972 RID: 2418 RVA: 0x000069F4 File Offset: 0x00004DF4
	Public Function TweenPositionX(start As Single, [end] As Single, time As Single, ease As EaseUtils.EaseType) As Coroutine
		Return MyBase.StartCoroutine(Me.tweenPositionX_cr(start, [end], time, ease, Nothing))
	End Function

	' Token: 0x06000973 RID: 2419 RVA: 0x00006A08 File Offset: 0x00004E08
	Public Function TweenPositionX(start As Single, [end] As Single, time As Single, ease As EaseUtils.EaseType, updateCallback As AbstractMonoBehaviour.TweenUpdateHandler) As Coroutine
		Return MyBase.StartCoroutine(Me.tweenPositionX_cr(start, [end], time, ease, updateCallback))
	End Function

	' Token: 0x06000974 RID: 2420 RVA: 0x00006A20 File Offset: 0x00004E20
	Private Iterator Function tweenPositionX_cr(start As Single, [end] As Single, time As Single, ease As EaseUtils.EaseType, Optional updateCallback As AbstractMonoBehaviour.TweenUpdateHandler = Nothing) As IEnumerator
		Me.transform.SetPosition(New Single?(start), Nothing, Nothing)
		Dim t As Single = 0F
		While t < time
			Dim val As Single = t / time
			Me.transform.SetPosition(New Single?(EaseUtils.Ease(ease, start, [end], val)), Nothing, Nothing)
			If updateCallback IsNot Nothing Then
				updateCallback(val)
			End If
			t += Me.LocalDeltaTime
			Yield Nothing
		End While
		Me.transform.SetPosition(New Single?([end]), Nothing, Nothing)
		If updateCallback IsNot Nothing Then
			updateCallback(1F)
		End If
		Yield Nothing
		Return
	End Function

	' Token: 0x06000975 RID: 2421 RVA: 0x00006A60 File Offset: 0x00004E60
	Public Function TweenLocalPositionX(start As Single, [end] As Single, time As Single, ease As EaseUtils.EaseType) As Coroutine
		Return MyBase.StartCoroutine(Me.tweenLocalPositionX_cr(start, [end], time, ease, Nothing))
	End Function

	' Token: 0x06000976 RID: 2422 RVA: 0x00006A74 File Offset: 0x00004E74
	Public Function TweenLocalPositionX(start As Single, [end] As Single, time As Single, ease As EaseUtils.EaseType, updateCallback As AbstractMonoBehaviour.TweenUpdateHandler) As Coroutine
		Return MyBase.StartCoroutine(Me.tweenLocalPositionX_cr(start, [end], time, ease, updateCallback))
	End Function

	' Token: 0x06000977 RID: 2423 RVA: 0x00006A8C File Offset: 0x00004E8C
	Private Iterator Function tweenLocalPositionX_cr(start As Single, [end] As Single, time As Single, ease As EaseUtils.EaseType, Optional updateCallback As AbstractMonoBehaviour.TweenUpdateHandler = Nothing) As IEnumerator
		Me.transform.SetLocalPosition(New Single?(start), Nothing, Nothing)
		Dim t As Single = 0F
		While t < time
			Dim val As Single = t / time
			Me.transform.SetLocalPosition(New Single?(EaseUtils.Ease(ease, start, [end], val)), Nothing, Nothing)
			If updateCallback IsNot Nothing Then
				updateCallback(val)
			End If
			t += Me.LocalDeltaTime
			Yield Nothing
		End While
		Me.transform.SetLocalPosition(New Single?([end]), Nothing, Nothing)
		If updateCallback IsNot Nothing Then
			updateCallback(1F)
		End If
		Yield Nothing
		Return
	End Function

	' Token: 0x06000978 RID: 2424 RVA: 0x00006ACC File Offset: 0x00004ECC
	Public Function TweenPositionY(start As Single, [end] As Single, time As Single, ease As EaseUtils.EaseType) As Coroutine
		Return MyBase.StartCoroutine(Me.tweenPositionY_cr(start, [end], time, ease, Nothing))
	End Function

	' Token: 0x06000979 RID: 2425 RVA: 0x00006AE0 File Offset: 0x00004EE0
	Public Function TweenPositionY(start As Single, [end] As Single, time As Single, ease As EaseUtils.EaseType, updateCallback As AbstractMonoBehaviour.TweenUpdateHandler) As Coroutine
		Return MyBase.StartCoroutine(Me.tweenPositionY_cr(start, [end], time, ease, updateCallback))
	End Function

	' Token: 0x0600097A RID: 2426 RVA: 0x00006AF8 File Offset: 0x00004EF8
	Private Iterator Function tweenPositionY_cr(start As Single, [end] As Single, time As Single, ease As EaseUtils.EaseType, Optional updateCallback As AbstractMonoBehaviour.TweenUpdateHandler = Nothing) As IEnumerator
		Me.transform.SetPosition(Nothing, New Single?(start), Nothing)
		Dim t As Single = 0F
		While t < time
			Dim val As Single = t / time
			Me.transform.SetPosition(Nothing, New Single?(EaseUtils.Ease(ease, start, [end], val)), Nothing)
			If updateCallback IsNot Nothing Then
				updateCallback(val)
			End If
			t += Me.LocalDeltaTime
			Yield Nothing
		End While
		Me.transform.SetPosition(Nothing, New Single?([end]), Nothing)
		If updateCallback IsNot Nothing Then
			updateCallback(1F)
		End If
		Yield Nothing
		Return
	End Function

	' Token: 0x0600097B RID: 2427 RVA: 0x00006B38 File Offset: 0x00004F38
	Public Function TweenLocalPositionY(start As Single, [end] As Single, time As Single, ease As EaseUtils.EaseType) As Coroutine
		Return MyBase.StartCoroutine(Me.tweenLocalPositionY_cr(start, [end], time, ease, Nothing))
	End Function

	' Token: 0x0600097C RID: 2428 RVA: 0x00006B4C File Offset: 0x00004F4C
	Public Function TweenLocalPositionY(start As Single, [end] As Single, time As Single, ease As EaseUtils.EaseType, updateCallback As AbstractMonoBehaviour.TweenUpdateHandler) As Coroutine
		Return MyBase.StartCoroutine(Me.tweenLocalPositionY_cr(start, [end], time, ease, updateCallback))
	End Function

	' Token: 0x0600097D RID: 2429 RVA: 0x00006B64 File Offset: 0x00004F64
	Private Iterator Function tweenLocalPositionY_cr(start As Single, [end] As Single, time As Single, ease As EaseUtils.EaseType, Optional updateCallback As AbstractMonoBehaviour.TweenUpdateHandler = Nothing) As IEnumerator
		Me.transform.SetLocalPosition(Nothing, New Single?(start), Nothing)
		Dim t As Single = 0F
		While t < time
			Dim val As Single = t / time
			Me.transform.SetLocalPosition(Nothing, New Single?(EaseUtils.Ease(ease, start, [end], val)), Nothing)
			If updateCallback IsNot Nothing Then
				updateCallback(val)
			End If
			t += Me.LocalDeltaTime
			Yield Nothing
		End While
		Me.transform.SetLocalPosition(Nothing, New Single?([end]), Nothing)
		If updateCallback IsNot Nothing Then
			updateCallback(1F)
		End If
		Yield Nothing
		Return
	End Function

	' Token: 0x0600097E RID: 2430 RVA: 0x00006BA4 File Offset: 0x00004FA4
	Public Function TweenPositionZ(start As Single, [end] As Single, time As Single, ease As EaseUtils.EaseType) As Coroutine
		Return MyBase.StartCoroutine(Me.tweenPositionZ_cr(start, [end], time, ease, Nothing))
	End Function

	' Token: 0x0600097F RID: 2431 RVA: 0x00006BB8 File Offset: 0x00004FB8
	Public Function TweenPositionZ(start As Single, [end] As Single, time As Single, ease As EaseUtils.EaseType, updateCallback As AbstractMonoBehaviour.TweenUpdateHandler) As Coroutine
		Return MyBase.StartCoroutine(Me.tweenPositionZ_cr(start, [end], time, ease, updateCallback))
	End Function

	' Token: 0x06000980 RID: 2432 RVA: 0x00006BD0 File Offset: 0x00004FD0
	Private Iterator Function tweenPositionZ_cr(start As Single, [end] As Single, time As Single, ease As EaseUtils.EaseType, Optional updateCallback As AbstractMonoBehaviour.TweenUpdateHandler = Nothing) As IEnumerator
		Me.transform.SetPosition(Nothing, Nothing, New Single?(start))
		Dim t As Single = 0F
		While t < time
			Dim val As Single = t / time
			Me.transform.SetPosition(Nothing, Nothing, New Single?(EaseUtils.Ease(ease, start, [end], val)))
			If updateCallback IsNot Nothing Then
				updateCallback(val)
			End If
			t += Me.LocalDeltaTime
			Yield Nothing
		End While
		Me.transform.SetPosition(Nothing, Nothing, New Single?([end]))
		If updateCallback IsNot Nothing Then
			updateCallback(1F)
		End If
		Yield Nothing
		Return
	End Function

	' Token: 0x06000981 RID: 2433 RVA: 0x00006C10 File Offset: 0x00005010
	Public Function TweenLocalPositionZ(start As Single, [end] As Single, time As Single, ease As EaseUtils.EaseType) As Coroutine
		Return MyBase.StartCoroutine(Me.tweenLocalPositionZ_cr(start, [end], time, ease, Nothing))
	End Function

	' Token: 0x06000982 RID: 2434 RVA: 0x00006C24 File Offset: 0x00005024
	Public Function TweenLocalPositionZ(start As Single, [end] As Single, time As Single, ease As EaseUtils.EaseType, updateCallback As AbstractMonoBehaviour.TweenUpdateHandler) As Coroutine
		Return MyBase.StartCoroutine(Me.tweenLocalPositionZ_cr(start, [end], time, ease, updateCallback))
	End Function

	' Token: 0x06000983 RID: 2435 RVA: 0x00006C3C File Offset: 0x0000503C
	Private Iterator Function tweenLocalPositionZ_cr(start As Single, [end] As Single, time As Single, ease As EaseUtils.EaseType, Optional updateCallback As AbstractMonoBehaviour.TweenUpdateHandler = Nothing) As IEnumerator
		Me.transform.SetLocalPosition(Nothing, Nothing, New Single?(start))
		Dim t As Single = 0F
		While t < time
			Dim val As Single = t / time
			Me.transform.SetLocalPosition(Nothing, Nothing, New Single?(EaseUtils.Ease(ease, start, [end], val)))
			If updateCallback IsNot Nothing Then
				updateCallback(val)
			End If
			t += Me.LocalDeltaTime
			Yield Nothing
		End While
		Me.transform.SetLocalPosition(Nothing, Nothing, New Single?([end]))
		If updateCallback IsNot Nothing Then
			updateCallback(1F)
		End If
		Yield Nothing
		Return
	End Function

	' Token: 0x06000984 RID: 2436 RVA: 0x00006C7C File Offset: 0x0000507C
	Public Function TweenRotation2D(start As Single, [end] As Single, time As Single, ease As EaseUtils.EaseType) As Coroutine
		Return MyBase.StartCoroutine(Me.tweenRotation2D_cr(start, [end], time, ease, Nothing))
	End Function

	' Token: 0x06000985 RID: 2437 RVA: 0x00006C90 File Offset: 0x00005090
	Public Function TweenRotation2D(start As Single, [end] As Single, time As Single, ease As EaseUtils.EaseType, updateCallback As AbstractMonoBehaviour.TweenUpdateHandler) As Coroutine
		Return MyBase.StartCoroutine(Me.tweenRotation2D_cr(start, [end], time, ease, updateCallback))
	End Function

	' Token: 0x06000986 RID: 2438 RVA: 0x00006CA8 File Offset: 0x000050A8
	Private Iterator Function tweenRotation2D_cr(start As Single, [end] As Single, time As Single, ease As EaseUtils.EaseType, Optional updateCallback As AbstractMonoBehaviour.TweenUpdateHandler = Nothing) As IEnumerator
		Me.transform.SetEulerAngles(Nothing, Nothing, New Single?(start))
		Dim t As Single = 0F
		While t < time
			Dim val As Single = t / time
			Me.transform.SetEulerAngles(Nothing, Nothing, New Single?(EaseUtils.Ease(ease, start, [end], val)))
			If updateCallback IsNot Nothing Then
				updateCallback(val)
			End If
			t += Me.LocalDeltaTime
			Yield Nothing
		End While
		Me.transform.SetEulerAngles(Nothing, Nothing, New Single?([end]))
		If updateCallback IsNot Nothing Then
			updateCallback(1F)
		End If
		Yield Nothing
		Return
	End Function

	' Token: 0x06000987 RID: 2439 RVA: 0x00006CE8 File Offset: 0x000050E8
	Public Function TweenLocalRotation2D(start As Single, [end] As Single, time As Single, ease As EaseUtils.EaseType) As Coroutine
		Return MyBase.StartCoroutine(Me.tweenLocalRotation2D_cr(start, [end], time, ease, Nothing))
	End Function

	' Token: 0x06000988 RID: 2440 RVA: 0x00006CFC File Offset: 0x000050FC
	Public Function TweenLocalRotation2D(start As Single, [end] As Single, time As Single, ease As EaseUtils.EaseType, updateCallback As AbstractMonoBehaviour.TweenUpdateHandler) As Coroutine
		Return MyBase.StartCoroutine(Me.tweenLocalRotation2D_cr(start, [end], time, ease, updateCallback))
	End Function

	' Token: 0x06000989 RID: 2441 RVA: 0x00006D14 File Offset: 0x00005114
	Private Iterator Function tweenLocalRotation2D_cr(start As Single, [end] As Single, time As Single, ease As EaseUtils.EaseType, Optional updateCallback As AbstractMonoBehaviour.TweenUpdateHandler = Nothing) As IEnumerator
		Me.transform.SetLocalEulerAngles(Nothing, Nothing, New Single?(start))
		Dim t As Single = 0F
		While t < time
			Dim val As Single = t / time
			Me.transform.SetLocalEulerAngles(Nothing, Nothing, New Single?(EaseUtils.Ease(ease, start, [end], val)))
			If updateCallback IsNot Nothing Then
				updateCallback(val)
			End If
			t += Me.LocalDeltaTime
			Yield Nothing
		End While
		Me.transform.SetLocalEulerAngles(Nothing, Nothing, New Single?([end]))
		If updateCallback IsNot Nothing Then
			updateCallback(1F)
		End If
		Yield Nothing
		Return
	End Function

	' Token: 0x0600098A RID: 2442 RVA: 0x00006D54 File Offset: 0x00005154
	Public Overridable Sub StopAllCoroutines()
		MyBase.StopAllCoroutines()
	End Sub

	' Token: 0x04001429 RID: 5161
	Private _transform As Transform

	' Token: 0x0400142A RID: 5162
	Private _transformCached As Boolean

	' Token: 0x0400142B RID: 5163
	Private _rectTransform As RectTransform

	' Token: 0x0400142C RID: 5164
	Private _rigidbody As Rigidbody

	' Token: 0x0400142D RID: 5165
	Private _rigidbody2D As Rigidbody2D

	' Token: 0x0400142E RID: 5166
	Private _animator As Animator

	' Token: 0x0400142F RID: 5167
	Protected ignoreGlobalTime As Boolean

	' Token: 0x04001430 RID: 5168
	Protected timeLayer As CupheadTime.Layer

	' Token: 0x0200035D RID: 861
	' (Invoke) Token: 0x0600098C RID: 2444
	Public Delegate Sub TweenUpdateHandler(value As Single)
End Class
