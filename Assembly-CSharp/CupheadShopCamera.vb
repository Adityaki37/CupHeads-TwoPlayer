Imports System

' Token: 0x020003DB RID: 987
Public Class CupheadShopCamera
	Inherits AbstractCupheadGameCamera

	' Token: 0x1700023C RID: 572
	' (get) Token: 0x06000D3E RID: 3390 RVA: 0x0008C811 File Offset: 0x0008AC11
	' (set) Token: 0x06000D3F RID: 3391 RVA: 0x0008C818 File Offset: 0x0008AC18
	Public Shared Property Current As CupheadShopCamera

	' Token: 0x1700023D RID: 573
	' (get) Token: 0x06000D40 RID: 3392 RVA: 0x0008C820 File Offset: 0x0008AC20
	Public Overrides ReadOnly Property OrthographicSize As Single
		Get
			Return 360F
		End Get
	End Property

	' Token: 0x06000D41 RID: 3393 RVA: 0x0008C827 File Offset: 0x0008AC27
	Protected Overrides Sub Awake()
		MyBase.Awake()
		CupheadShopCamera.Current = Me
	End Sub

	' Token: 0x06000D42 RID: 3394 RVA: 0x0008C835 File Offset: 0x0008AC35
	Private Sub OnDestroy()
		If CupheadShopCamera.Current Is Me Then
			CupheadShopCamera.Current = Nothing
		End If
	End Sub
End Class
