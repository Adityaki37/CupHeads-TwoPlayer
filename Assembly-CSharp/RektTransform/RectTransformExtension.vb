Imports System
Imports System.Diagnostics
Imports UnityEngine

Namespace RektTransform
	' Token: 0x0200036E RID: 878
	Public Module RectTransformExtension
		' Token: 0x060009CC RID: 2508 RVA: 0x0007D108 File Offset: 0x0007B508
		<Conditional("REKT_LOG_ACTIVE")>
		Private Sub Log(message As Object)
			Global.UnityEngine.Debug.Log(message)
		End Sub

		' Token: 0x060009CD RID: 2509 RVA: 0x0007D110 File Offset: 0x0007B510
		<System.Runtime.CompilerServices.ExtensionAttribute()>
		Public Sub DebugOutput(RT As RectTransform)
		End Sub

		' Token: 0x060009CE RID: 2510 RVA: 0x0007D114 File Offset: 0x0007B514
		<System.Runtime.CompilerServices.ExtensionAttribute()>
		Public Function GetWorldRect(RT As RectTransform) As Rect
			Dim array As Vector3() = New Vector3(3) {}
			RT.GetWorldCorners(array)
			Dim vector As Vector2 = New Vector2(array(2).x - array(1).x, array(1).y - array(0).y)
			Return New Rect(New Vector2(array(1).x, -array(1).y), vector)
		End Function

		' Token: 0x060009CF RID: 2511 RVA: 0x0007D18C File Offset: 0x0007B58C
		<System.Runtime.CompilerServices.ExtensionAttribute()>
		Public Function GetAnchors(RT As RectTransform) As MinMax
			Return New MinMax(RT.anchorMin, RT.anchorMax)
		End Function

		' Token: 0x060009D0 RID: 2512 RVA: 0x0007D19F File Offset: 0x0007B59F
		<System.Runtime.CompilerServices.ExtensionAttribute()>
		Public Sub SetAnchors(RT As RectTransform, anchors As MinMax)
			RT.anchorMin = anchors.min
			RT.anchorMax = anchors.max
		End Sub

		' Token: 0x060009D1 RID: 2513 RVA: 0x0007D1BB File Offset: 0x0007B5BB
		<System.Runtime.CompilerServices.ExtensionAttribute()>
		Public Function GetParent(RT As RectTransform) As RectTransform
			Return TryCast(RT.parent, RectTransform)
		End Function

		' Token: 0x060009D2 RID: 2514 RVA: 0x0007D1C8 File Offset: 0x0007B5C8
		<System.Runtime.CompilerServices.ExtensionAttribute()>
		Public Function GetWidth(RT As RectTransform) As Single
			Return RT.rect.width
		End Function

		' Token: 0x060009D3 RID: 2515 RVA: 0x0007D1E4 File Offset: 0x0007B5E4
		<System.Runtime.CompilerServices.ExtensionAttribute()>
		Public Function GetHeight(RT As RectTransform) As Single
			Return RT.rect.height
		End Function

		' Token: 0x060009D4 RID: 2516 RVA: 0x0007D1FF File Offset: 0x0007B5FF
		<System.Runtime.CompilerServices.ExtensionAttribute()>
		Public Function GetSize(RT As RectTransform) As Vector2
			Return New Vector2(RT.GetWidth(), RT.GetHeight())
		End Function

		' Token: 0x060009D5 RID: 2517 RVA: 0x0007D212 File Offset: 0x0007B612
		<System.Runtime.CompilerServices.ExtensionAttribute()>
		Public Sub SetWidth(RT As RectTransform, width As Single)
			RT.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width)
		End Sub

		' Token: 0x060009D6 RID: 2518 RVA: 0x0007D21C File Offset: 0x0007B61C
		<System.Runtime.CompilerServices.ExtensionAttribute()>
		Public Sub SetHeight(RT As RectTransform, height As Single)
			RT.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height)
		End Sub

		' Token: 0x060009D7 RID: 2519 RVA: 0x0007D226 File Offset: 0x0007B626
		<System.Runtime.CompilerServices.ExtensionAttribute()>
		Public Sub SetSize(RT As RectTransform, width As Single, height As Single)
			RT.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width)
			RT.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height)
		End Sub

		' Token: 0x060009D8 RID: 2520 RVA: 0x0007D238 File Offset: 0x0007B638
		<System.Runtime.CompilerServices.ExtensionAttribute()>
		Public Sub SetSize(RT As RectTransform, size As Vector2)
			RT.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, size.x)
			RT.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, size.y)
		End Sub

		' Token: 0x060009D9 RID: 2521 RVA: 0x0007D258 File Offset: 0x0007B658
		<System.Runtime.CompilerServices.ExtensionAttribute()>
		Public Function GetLeft(RT As RectTransform) As Vector2
			Return New Vector2(RT.offsetMin.x, RT.anchoredPosition.y)
		End Function

		' Token: 0x060009DA RID: 2522 RVA: 0x0007D288 File Offset: 0x0007B688
		<System.Runtime.CompilerServices.ExtensionAttribute()>
		Public Function GetRight(RT As RectTransform) As Vector2
			Return New Vector2(RT.offsetMax.x, RT.anchoredPosition.y)
		End Function

		' Token: 0x060009DB RID: 2523 RVA: 0x0007D2B8 File Offset: 0x0007B6B8
		<System.Runtime.CompilerServices.ExtensionAttribute()>
		Public Function GetTop(RT As RectTransform) As Vector2
			Return New Vector2(RT.anchoredPosition.x, RT.offsetMax.y)
		End Function

		' Token: 0x060009DC RID: 2524 RVA: 0x0007D2E8 File Offset: 0x0007B6E8
		<System.Runtime.CompilerServices.ExtensionAttribute()>
		Public Function GetBottom(RT As RectTransform) As Vector2
			Return New Vector2(RT.anchoredPosition.x, RT.offsetMin.y)
		End Function

		' Token: 0x060009DD RID: 2525 RVA: 0x0007D318 File Offset: 0x0007B718
		<System.Runtime.CompilerServices.ExtensionAttribute()>
		Public Sub SetLeft(RT As RectTransform, left As Single)
			Dim xMin As Single = RT.GetParent().rect.xMin
			Dim num As Single = RT.anchorMin.x * 2F - 1F
			RT.offsetMin = New Vector2(xMin + xMin * num + left, RT.offsetMin.y)
		End Sub

		' Token: 0x060009DE RID: 2526 RVA: 0x0007D378 File Offset: 0x0007B778
		<System.Runtime.CompilerServices.ExtensionAttribute()>
		Public Sub SetRight(RT As RectTransform, right As Single)
			Dim xMax As Single = RT.GetParent().rect.xMax
			Dim num As Single = RT.anchorMax.x * 2F - 1F
			RT.offsetMax = New Vector2(xMax - xMax * num + right, RT.offsetMax.y)
		End Sub

		' Token: 0x060009DF RID: 2527 RVA: 0x0007D3D8 File Offset: 0x0007B7D8
		<System.Runtime.CompilerServices.ExtensionAttribute()>
		Public Sub SetTop(RT As RectTransform, top As Single)
			Dim yMax As Single = RT.GetParent().rect.yMax
			Dim num As Single = RT.anchorMax.y * 2F - 1F
			RT.offsetMax = New Vector2(RT.offsetMax.x, yMax - yMax * num + top)
		End Sub

		' Token: 0x060009E0 RID: 2528 RVA: 0x0007D438 File Offset: 0x0007B838
		<System.Runtime.CompilerServices.ExtensionAttribute()>
		Public Sub SetBottom(RT As RectTransform, bottom As Single)
			Dim yMin As Single = RT.GetParent().rect.yMin
			Dim num As Single = RT.anchorMin.y * 2F - 1F
			RT.offsetMin = New Vector2(RT.offsetMin.x, yMin + yMin * num + bottom)
		End Sub

		' Token: 0x060009E1 RID: 2529 RVA: 0x0007D495 File Offset: 0x0007B895
		<System.Runtime.CompilerServices.ExtensionAttribute()>
		Public Sub Left(RT As RectTransform, left As Single)
			RT.SetLeft(left)
		End Sub

		' Token: 0x060009E2 RID: 2530 RVA: 0x0007D49E File Offset: 0x0007B89E
		<System.Runtime.CompilerServices.ExtensionAttribute()>
		Public Sub Right(RT As RectTransform, right As Single)
			RT.SetRight(-right)
		End Sub

		' Token: 0x060009E3 RID: 2531 RVA: 0x0007D4A8 File Offset: 0x0007B8A8
		<System.Runtime.CompilerServices.ExtensionAttribute()>
		Public Sub Top(RT As RectTransform, top As Single)
			RT.SetTop(-top)
		End Sub

		' Token: 0x060009E4 RID: 2532 RVA: 0x0007D4B2 File Offset: 0x0007B8B2
		<System.Runtime.CompilerServices.ExtensionAttribute()>
		Public Sub Bottom(RT As RectTransform, bottom As Single)
			RT.SetRight(bottom)
		End Sub

		' Token: 0x060009E5 RID: 2533 RVA: 0x0007D4BC File Offset: 0x0007B8BC
		<System.Runtime.CompilerServices.ExtensionAttribute()>
		Public Sub SetLeftFrom(RT As RectTransform, anchor As MinMax, left As Single)
			RT.offsetMin = New Vector2(RT.AnchorToParentSpace(anchor.min - RT.anchorMin).x + left, RT.offsetMin.y)
		End Sub

		' Token: 0x060009E6 RID: 2534 RVA: 0x0007D504 File Offset: 0x0007B904
		<System.Runtime.CompilerServices.ExtensionAttribute()>
		Public Sub SetRightFrom(RT As RectTransform, anchor As MinMax, right As Single)
			RT.offsetMax = New Vector2(RT.AnchorToParentSpace(anchor.max - RT.anchorMax).x + right, RT.offsetMax.y)
		End Sub

		' Token: 0x060009E7 RID: 2535 RVA: 0x0007D54C File Offset: 0x0007B94C
		<System.Runtime.CompilerServices.ExtensionAttribute()>
		Public Sub SetTopFrom(RT As RectTransform, anchor As MinMax, top As Single)
			Dim vector As Vector2 = RT.AnchorToParentSpace(anchor.max - RT.anchorMax)
			RT.offsetMax = New Vector2(RT.offsetMax.x, vector.y + top)
		End Sub

		' Token: 0x060009E8 RID: 2536 RVA: 0x0007D594 File Offset: 0x0007B994
		<System.Runtime.CompilerServices.ExtensionAttribute()>
		Public Sub SetBottomFrom(RT As RectTransform, anchor As MinMax, bottom As Single)
			Dim vector As Vector2 = RT.AnchorToParentSpace(anchor.min - RT.anchorMin)
			RT.offsetMin = New Vector2(RT.offsetMin.x, vector.y + bottom)
		End Sub

		' Token: 0x060009E9 RID: 2537 RVA: 0x0007D5DC File Offset: 0x0007B9DC
		<System.Runtime.CompilerServices.ExtensionAttribute()>
		Public Sub SetRelativeLeft(RT As RectTransform, left As Single)
			RT.offsetMin = New Vector2(RT.anchoredPosition.x + left, RT.offsetMin.y)
		End Sub

		' Token: 0x060009EA RID: 2538 RVA: 0x0007D614 File Offset: 0x0007BA14
		<System.Runtime.CompilerServices.ExtensionAttribute()>
		Public Sub SetRelativeRight(RT As RectTransform, right As Single)
			RT.offsetMax = New Vector2(RT.anchoredPosition.x + right, RT.offsetMax.y)
		End Sub

		' Token: 0x060009EB RID: 2539 RVA: 0x0007D64C File Offset: 0x0007BA4C
		<System.Runtime.CompilerServices.ExtensionAttribute()>
		Public Sub SetRelativeTop(RT As RectTransform, top As Single)
			RT.offsetMax = New Vector2(RT.offsetMax.x, RT.anchoredPosition.y + top)
		End Sub

		' Token: 0x060009EC RID: 2540 RVA: 0x0007D684 File Offset: 0x0007BA84
		<System.Runtime.CompilerServices.ExtensionAttribute()>
		Public Sub SetRelativeBottom(RT As RectTransform, bottom As Single)
			RT.offsetMin = New Vector2(RT.offsetMin.x, RT.anchoredPosition.y + bottom)
		End Sub

		' Token: 0x060009ED RID: 2541 RVA: 0x0007D6BC File Offset: 0x0007BABC
		<System.Runtime.CompilerServices.ExtensionAttribute()>
		Public Sub MoveLeft(RT As RectTransform, Optional left As Single = 0F)
			Dim xMin As Single = RT.GetParent().rect.xMin
			Dim num As Single = RT.anchorMax.x - RT.anchorMin.x
			Dim num2 As Single = RT.anchorMax.x * 2F - 1F
			RT.anchoredPosition = New Vector2(xMin + xMin * num2 + left - num * xMin, RT.anchoredPosition.y)
		End Sub

		' Token: 0x060009EE RID: 2542 RVA: 0x0007D740 File Offset: 0x0007BB40
		<System.Runtime.CompilerServices.ExtensionAttribute()>
		Public Sub MoveRight(RT As RectTransform, Optional right As Single = 0F)
			Dim xMax As Single = RT.GetParent().rect.xMax
			Dim num As Single = RT.anchorMax.x - RT.anchorMin.x
			Dim num2 As Single = RT.anchorMax.x * 2F - 1F
			RT.anchoredPosition = New Vector2(xMax - xMax * num2 - right + num * xMax, RT.anchoredPosition.y)
		End Sub

		' Token: 0x060009EF RID: 2543 RVA: 0x0007D7C4 File Offset: 0x0007BBC4
		<System.Runtime.CompilerServices.ExtensionAttribute()>
		Public Sub MoveTop(RT As RectTransform, Optional top As Single = 0F)
			Dim yMax As Single = RT.GetParent().rect.yMax
			Dim num As Single = RT.anchorMax.y - RT.anchorMin.y
			Dim num2 As Single = RT.anchorMax.y * 2F - 1F
			RT.anchoredPosition = New Vector2(RT.anchoredPosition.x, yMax - yMax * num2 - top + num * yMax)
		End Sub

		' Token: 0x060009F0 RID: 2544 RVA: 0x0007D848 File Offset: 0x0007BC48
		<System.Runtime.CompilerServices.ExtensionAttribute()>
		Public Sub MoveBottom(RT As RectTransform, Optional bottom As Single = 0F)
			Dim yMin As Single = RT.GetParent().rect.yMin
			Dim num As Single = RT.anchorMax.y - RT.anchorMin.y
			Dim num2 As Single = RT.anchorMax.y * 2F - 1F
			RT.anchoredPosition = New Vector2(RT.anchoredPosition.x, yMin + yMin * num2 + bottom - num * yMin)
		End Sub

		' Token: 0x060009F1 RID: 2545 RVA: 0x0007D8CB File Offset: 0x0007BCCB
		<System.Runtime.CompilerServices.ExtensionAttribute()>
		Public Sub MoveLeftInside(RT As RectTransform, Optional left As Single = 0F)
			RT.MoveLeft(left + RT.GetWidth() / 2F)
		End Sub

		' Token: 0x060009F2 RID: 2546 RVA: 0x0007D8E1 File Offset: 0x0007BCE1
		<System.Runtime.CompilerServices.ExtensionAttribute()>
		Public Sub MoveRightInside(RT As RectTransform, Optional right As Single = 0F)
			RT.MoveRight(right + RT.GetWidth() / 2F)
		End Sub

		' Token: 0x060009F3 RID: 2547 RVA: 0x0007D8F7 File Offset: 0x0007BCF7
		<System.Runtime.CompilerServices.ExtensionAttribute()>
		Public Sub MoveTopInside(RT As RectTransform, Optional top As Single = 0F)
			RT.MoveTop(top + RT.GetHeight() / 2F)
		End Sub

		' Token: 0x060009F4 RID: 2548 RVA: 0x0007D90D File Offset: 0x0007BD0D
		<System.Runtime.CompilerServices.ExtensionAttribute()>
		Public Sub MoveBottomInside(RT As RectTransform, Optional bottom As Single = 0F)
			RT.MoveBottom(bottom + RT.GetHeight() / 2F)
		End Sub

		' Token: 0x060009F5 RID: 2549 RVA: 0x0007D923 File Offset: 0x0007BD23
		<System.Runtime.CompilerServices.ExtensionAttribute()>
		Public Sub MoveLeftOutside(RT As RectTransform, Optional left As Single = 0F)
			RT.MoveLeft(left - RT.GetWidth() / 2F)
		End Sub

		' Token: 0x060009F6 RID: 2550 RVA: 0x0007D939 File Offset: 0x0007BD39
		<System.Runtime.CompilerServices.ExtensionAttribute()>
		Public Sub MoveRightOutside(RT As RectTransform, Optional right As Single = 0F)
			RT.MoveRight(right - RT.GetWidth() / 2F)
		End Sub

		' Token: 0x060009F7 RID: 2551 RVA: 0x0007D94F File Offset: 0x0007BD4F
		<System.Runtime.CompilerServices.ExtensionAttribute()>
		Public Sub MoveTopOutside(RT As RectTransform, Optional top As Single = 0F)
			RT.MoveTop(top - RT.GetHeight() / 2F)
		End Sub

		' Token: 0x060009F8 RID: 2552 RVA: 0x0007D965 File Offset: 0x0007BD65
		<System.Runtime.CompilerServices.ExtensionAttribute()>
		Public Sub MoveBottomOutside(RT As RectTransform, Optional bottom As Single = 0F)
			RT.MoveBottom(bottom - RT.GetHeight() / 2F)
		End Sub

		' Token: 0x060009F9 RID: 2553 RVA: 0x0007D97B File Offset: 0x0007BD7B
		<System.Runtime.CompilerServices.ExtensionAttribute()>
		Public Sub Move(RT As RectTransform, x As Single, y As Single)
			RT.MoveLeft(x)
			RT.MoveBottom(y)
		End Sub

		' Token: 0x060009FA RID: 2554 RVA: 0x0007D98B File Offset: 0x0007BD8B
		<System.Runtime.CompilerServices.ExtensionAttribute()>
		Public Sub Move(RT As RectTransform, point As Vector2)
			RT.MoveLeft(point.x)
			RT.MoveBottom(point.y)
		End Sub

		' Token: 0x060009FB RID: 2555 RVA: 0x0007D9A7 File Offset: 0x0007BDA7
		<System.Runtime.CompilerServices.ExtensionAttribute()>
		Public Sub MoveInside(RT As RectTransform, x As Single, y As Single)
			RT.MoveLeftInside(x)
			RT.MoveBottomInside(y)
		End Sub

		' Token: 0x060009FC RID: 2556 RVA: 0x0007D9B7 File Offset: 0x0007BDB7
		<System.Runtime.CompilerServices.ExtensionAttribute()>
		Public Sub MoveInside(RT As RectTransform, point As Vector2)
			RT.MoveLeftInside(point.x)
			RT.MoveBottomInside(point.y)
		End Sub

		' Token: 0x060009FD RID: 2557 RVA: 0x0007D9D3 File Offset: 0x0007BDD3
		<System.Runtime.CompilerServices.ExtensionAttribute()>
		Public Sub MoveOutside(RT As RectTransform, x As Single, y As Single)
			RT.MoveLeftOutside(x)
			RT.MoveBottomOutside(y)
		End Sub

		' Token: 0x060009FE RID: 2558 RVA: 0x0007D9E3 File Offset: 0x0007BDE3
		<System.Runtime.CompilerServices.ExtensionAttribute()>
		Public Sub MoveOutside(RT As RectTransform, point As Vector2)
			RT.MoveLeftOutside(point.x)
			RT.MoveBottomOutside(point.y)
		End Sub

		' Token: 0x060009FF RID: 2559 RVA: 0x0007D9FF File Offset: 0x0007BDFF
		<System.Runtime.CompilerServices.ExtensionAttribute()>
		Public Sub MoveFrom(RT As RectTransform, anchor As MinMax, point As Vector2)
			RT.MoveFrom(anchor, point.x, point.y)
		End Sub

		' Token: 0x06000A00 RID: 2560 RVA: 0x0007DA18 File Offset: 0x0007BE18
		<System.Runtime.CompilerServices.ExtensionAttribute()>
		Public Sub MoveFrom(RT As RectTransform, anchor As MinMax, x As Single, y As Single)
			Dim vector As Vector2 = RT.AnchorToParentSpace(RectTransformExtension.AnchorOrigin(anchor) - RT.AnchorOrigin())
			RT.anchoredPosition = New Vector2(vector.x + x, vector.y + y)
		End Sub

		' Token: 0x06000A01 RID: 2561 RVA: 0x0007DA5A File Offset: 0x0007BE5A
		<System.Runtime.CompilerServices.ExtensionAttribute()>
		Public Function ParentToChildSpace(RT As RectTransform, point As Vector2) As Vector2
			Return RT.ParentToChildSpace(point.x, point.y)
		End Function

		' Token: 0x06000A02 RID: 2562 RVA: 0x0007DA70 File Offset: 0x0007BE70
		<System.Runtime.CompilerServices.ExtensionAttribute()>
		Public Function ParentToChildSpace(RT As RectTransform, x As Single, y As Single) As Vector2
			Dim xMin As Single = RT.GetParent().rect.xMin
			Dim yMin As Single = RT.GetParent().rect.yMin
			Dim num As Single = RT.anchorMin.x * 2F - 1F
			Dim num2 As Single = RT.anchorMin.y * 2F - 1F
			Return New Vector2(xMin + xMin * num + x, yMin + yMin * num2 + y)
		End Function

		' Token: 0x06000A03 RID: 2563 RVA: 0x0007DAF4 File Offset: 0x0007BEF4
		<System.Runtime.CompilerServices.ExtensionAttribute()>
		Public Function ChildToParentSpace(RT As RectTransform, x As Single, y As Single) As Vector2
			Return RT.AnchorOriginParent() + New Vector2(x, y)
		End Function

		' Token: 0x06000A04 RID: 2564 RVA: 0x0007DB08 File Offset: 0x0007BF08
		<System.Runtime.CompilerServices.ExtensionAttribute()>
		Public Function ChildToParentSpace(RT As RectTransform, point As Vector2) As Vector2
			Return RT.AnchorOriginParent() + point
		End Function

		' Token: 0x06000A05 RID: 2565 RVA: 0x0007DB16 File Offset: 0x0007BF16
		<System.Runtime.CompilerServices.ExtensionAttribute()>
		Public Function ParentToAnchorSpace(RT As RectTransform, point As Vector2) As Vector2
			Return RT.ParentToAnchorSpace(point.x, point.y)
		End Function

		' Token: 0x06000A06 RID: 2566 RVA: 0x0007DB2C File Offset: 0x0007BF2C
		<System.Runtime.CompilerServices.ExtensionAttribute()>
		Public Function ParentToAnchorSpace(RT As RectTransform, x As Single, y As Single) As Vector2
			Dim rect As Rect = RT.GetParent().rect
			If rect.width <> 0F Then
				x /= rect.width
			Else
				x = 0F
			End If
			If rect.height <> 0F Then
				y /= rect.height
			Else
				y = 0F
			End If
			Return New Vector2(x, y)
		End Function

		' Token: 0x06000A07 RID: 2567 RVA: 0x0007DB9C File Offset: 0x0007BF9C
		<System.Runtime.CompilerServices.ExtensionAttribute()>
		Public Function AnchorToParentSpace(RT As RectTransform, x As Single, y As Single) As Vector2
			Return New Vector2(x * RT.GetParent().rect.width, y * RT.GetParent().rect.height)
		End Function

		' Token: 0x06000A08 RID: 2568 RVA: 0x0007DBD8 File Offset: 0x0007BFD8
		<System.Runtime.CompilerServices.ExtensionAttribute()>
		Public Function AnchorToParentSpace(RT As RectTransform, point As Vector2) As Vector2
			Return New Vector2(point.x * RT.GetParent().rect.width, point.y * RT.GetParent().rect.height)
		End Function

		' Token: 0x06000A09 RID: 2569 RVA: 0x0007DC20 File Offset: 0x0007C020
		<System.Runtime.CompilerServices.ExtensionAttribute()>
		Public Function AnchorOrigin(RT As RectTransform) As Vector2
			Return RectTransformExtension.AnchorOrigin(RT.GetAnchors())
		End Function

		' Token: 0x06000A0A RID: 2570 RVA: 0x0007DC30 File Offset: 0x0007C030
		Public Function AnchorOrigin(anchor As MinMax) As Vector2
			Dim num As Single = anchor.min.x + (anchor.max.x - anchor.min.x) / 2F
			Dim num2 As Single = anchor.min.y + (anchor.max.y - anchor.min.y) / 2F
			Return New Vector2(num, num2)
		End Function

		' Token: 0x06000A0B RID: 2571 RVA: 0x0007DCA0 File Offset: 0x0007C0A0
		<System.Runtime.CompilerServices.ExtensionAttribute()>
		Public Function AnchorOriginParent(RT As RectTransform) As Vector2
			Return Vector2.Scale(RT.AnchorOrigin(), New Vector2(RT.GetParent().rect.width, RT.GetParent().rect.height))
		End Function

		' Token: 0x06000A0C RID: 2572 RVA: 0x0007DCE4 File Offset: 0x0007C0E4
		<System.Runtime.CompilerServices.ExtensionAttribute()>
		Public Function GetRootCanvas(RT As RectTransform) As Canvas
			Dim canvas As Canvas = RT.GetComponentInParent(Of Canvas)()
			While Not canvas.isRootCanvas
				canvas = canvas.transform.parent.GetComponentInParent(Of Canvas)()
			End While
			Return canvas
		End Function
	End Module
End Namespace
