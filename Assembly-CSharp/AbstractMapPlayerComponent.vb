Imports System
Imports UnityEngine

' Token: 0x02000972 RID: 2418
Public MustInherit Class AbstractMapPlayerComponent
	Inherits AbstractPausableComponent

	' Token: 0x1700048C RID: 1164
	' (get) Token: 0x0600384E RID: 14414 RVA: 0x002033E5 File Offset: 0x002017E5
	' (set) Token: 0x0600384F RID: 14415 RVA: 0x002033ED File Offset: 0x002017ED
	Public Property player As MapPlayerController

	' Token: 0x1700048D RID: 1165
	' (get) Token: 0x06003850 RID: 14416 RVA: 0x002033F6 File Offset: 0x002017F6
	' (set) Token: 0x06003851 RID: 14417 RVA: 0x002033FE File Offset: 0x002017FE
	Public Property input As PlayerInput

	' Token: 0x06003852 RID: 14418 RVA: 0x00203407 File Offset: 0x00201807
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.player = MyBase.GetComponent(Of MapPlayerController)()
		Me.input = MyBase.GetComponent(Of PlayerInput)()
		Me.RegisterEvents()
	End Sub

	' Token: 0x06003853 RID: 14419 RVA: 0x0020342D File Offset: 0x0020182D
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me.UnregisterEvents()
	End Sub

	' Token: 0x06003854 RID: 14420 RVA: 0x0020343C File Offset: 0x0020183C
	Private Sub RegisterEvents()
		AddHandler Me.player.LadderEnterEvent, AddressOf Me.OnLadderEnter
		AddHandler Me.player.LadderEnterCompleteEvent, AddressOf Me.OnLadderEnterComplete
		AddHandler Me.player.LadderExitEvent, AddressOf Me.OnLadderExit
		AddHandler Me.player.LadderExitCompleteEvent, AddressOf Me.OnLadderExitComplete
	End Sub

	' Token: 0x06003855 RID: 14421 RVA: 0x002034AC File Offset: 0x002018AC
	Private Sub UnregisterEvents()
		RemoveHandler Me.player.LadderEnterEvent, AddressOf Me.OnLadderEnter
		RemoveHandler Me.player.LadderEnterCompleteEvent, AddressOf Me.OnLadderEnterComplete
		RemoveHandler Me.player.LadderExitEvent, AddressOf Me.OnLadderExit
		RemoveHandler Me.player.LadderExitCompleteEvent, AddressOf Me.OnLadderExitComplete
	End Sub

	' Token: 0x06003856 RID: 14422 RVA: 0x00203519 File Offset: 0x00201919
	Protected Overridable Sub OnLadderEnter(point As Vector2, ladder As MapPlayerLadderObject, location As MapLadder.Location)
	End Sub

	' Token: 0x06003857 RID: 14423 RVA: 0x0020351B File Offset: 0x0020191B
	Protected Overridable Sub OnLadderExit(point As Vector2, [exit] As Vector2, location As MapLadder.Location)
	End Sub

	' Token: 0x06003858 RID: 14424 RVA: 0x0020351D File Offset: 0x0020191D
	Protected Overridable Sub OnLadderEnterComplete()
	End Sub

	' Token: 0x06003859 RID: 14425 RVA: 0x0020351F File Offset: 0x0020191F
	Protected Overridable Sub OnLadderExitComplete()
	End Sub
End Class
