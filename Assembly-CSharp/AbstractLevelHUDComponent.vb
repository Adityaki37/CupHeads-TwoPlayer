Imports System

' Token: 0x0200048A RID: 1162
Public Class AbstractLevelHUDComponent
	Inherits AbstractMonoBehaviour

	' Token: 0x170002CE RID: 718
	' (get) Token: 0x06001230 RID: 4656 RVA: 0x000A9025 File Offset: 0x000A7425
	' (set) Token: 0x06001231 RID: 4657 RVA: 0x000A902D File Offset: 0x000A742D
	Protected Private Property _hud As LevelHUDPlayer

	' Token: 0x170002CF RID: 719
	' (get) Token: 0x06001232 RID: 4658 RVA: 0x000A9036 File Offset: 0x000A7436
	Protected ReadOnly Property _player As AbstractPlayerController
		Get
			Return Me._hud.player
		End Get
	End Property

	' Token: 0x06001233 RID: 4659 RVA: 0x000A9043 File Offset: 0x000A7443
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.ignoreGlobalTime = True
		Me.timeLayer = CupheadTime.Layer.UI
	End Sub

	' Token: 0x06001234 RID: 4660 RVA: 0x000A9059 File Offset: 0x000A7459
	Private Sub Start()
		If Me._parentToHudCanvas Then
			MyBase.transform.SetParent(LevelHUD.Current.Canvas.transform, False)
		End If
	End Sub

	' Token: 0x06001235 RID: 4661 RVA: 0x000A9081 File Offset: 0x000A7481
	Public Overridable Sub Init(hud As LevelHUDPlayer)
		Me._hud = hud
	End Sub

	' Token: 0x04001BAB RID: 7083
	Protected _parentToHudCanvas As Boolean
End Class
