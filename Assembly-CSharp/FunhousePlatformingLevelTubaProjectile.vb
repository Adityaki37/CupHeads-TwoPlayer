Imports System

' Token: 0x020008C0 RID: 2240
Public Class FunhousePlatformingLevelTubaProjectile
	Inherits BasicProjectile

	' Token: 0x1700044B RID: 1099
	' (get) Token: 0x06003451 RID: 13393 RVA: 0x001E5ECC File Offset: 0x001E42CC
	Protected Overrides ReadOnly Property DestroyedAfterLeavingScreen As Boolean
		Get
			Return True
		End Get
	End Property

	' Token: 0x1700044C RID: 1100
	' (get) Token: 0x06003452 RID: 13394 RVA: 0x001E5ECF File Offset: 0x001E42CF
	Protected Overrides ReadOnly Property DestroyLifetime As Single
		Get
			Return 0F
		End Get
	End Property

	' Token: 0x06003453 RID: 13395 RVA: 0x001E5ED6 File Offset: 0x001E42D6
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.DestroyDistance = 0F
	End Sub
End Class
