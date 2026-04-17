Imports System
Imports System.Collections
Imports System.Diagnostics
Imports UnityEngine

' Token: 0x020009F4 RID: 2548
Public Class ArcadePlayerParryController
	Inherits AbstractArcadePlayerComponent

	' Token: 0x17000515 RID: 1301
	' (get) Token: 0x06003C21 RID: 15393 RVA: 0x00218717 File Offset: 0x00216B17
	Public ReadOnly Property State As ArcadePlayerParryController.ParryState
		Get
			Return Me.state
		End Get
	End Property

	' Token: 0x1400007E RID: 126
	' (add) Token: 0x06003C22 RID: 15394 RVA: 0x00218720 File Offset: 0x00216B20
	' (remove) Token: 0x06003C23 RID: 15395 RVA: 0x00218758 File Offset: 0x00216B58
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnParryStartEvent As Action

	' Token: 0x1400007F RID: 127
	' (add) Token: 0x06003C24 RID: 15396 RVA: 0x00218790 File Offset: 0x00216B90
	' (remove) Token: 0x06003C25 RID: 15397 RVA: 0x002187C8 File Offset: 0x00216BC8
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnParryEndEvent As Action

	' Token: 0x06003C26 RID: 15398 RVA: 0x002187FE File Offset: 0x00216BFE
	Private Sub Start()
		AddHandler MyBase.player.motor.OnParryEvent, AddressOf Me.StartParry
	End Sub

	' Token: 0x06003C27 RID: 15399 RVA: 0x0021881C File Offset: 0x00216C1C
	Public Overrides Sub OnLevelStart()
		MyBase.OnLevelStart()
		Me.state = ArcadePlayerParryController.ParryState.Ready
	End Sub

	' Token: 0x06003C28 RID: 15400 RVA: 0x0021882B File Offset: 0x00216C2B
	Private Sub StartParry()
		Me.state = ArcadePlayerParryController.ParryState.Parrying
		If Me.OnParryStartEvent IsNot Nothing Then
			Me.OnParryStartEvent()
		End If
		MyBase.StartCoroutine(Me.parry_cr())
	End Sub

	' Token: 0x06003C29 RID: 15401 RVA: 0x00218858 File Offset: 0x00216C58
	Private Iterator Function parry_cr() As IEnumerator
		Me.effect.Create(MyBase.player)
		Yield CupheadTime.WaitForSeconds(Me, 0.2F)
		Me.state = ArcadePlayerParryController.ParryState.Ready
		If Me.OnParryEndEvent IsNot Nothing Then
			Me.OnParryEndEvent()
		End If
		Return
	End Function

	' Token: 0x040043A8 RID: 17320
	Public Const DURATION As Single = 0.2F

	' Token: 0x040043A9 RID: 17321
	Private state As ArcadePlayerParryController.ParryState

	' Token: 0x040043AA RID: 17322
	<SerializeField()>
	Private effect As ArcadePlayerParryEffect

	' Token: 0x020009F5 RID: 2549
	Public Enum ParryState
		' Token: 0x040043AE RID: 17326
		Init
		' Token: 0x040043AF RID: 17327
		Ready
		' Token: 0x040043B0 RID: 17328
		Parrying
	End Enum
End Class
