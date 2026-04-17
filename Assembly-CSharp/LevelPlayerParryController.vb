Imports System
Imports System.Collections
Imports System.Diagnostics
Imports UnityEngine

' Token: 0x02000A35 RID: 2613
Public Class LevelPlayerParryController
	Inherits AbstractLevelPlayerComponent
	Implements IParryAttack

	' Token: 0x1700055F RID: 1375
	' (get) Token: 0x06003E24 RID: 15908 RVA: 0x002236C6 File Offset: 0x00221AC6
	Public ReadOnly Property State As LevelPlayerParryController.ParryState
		Get
			Return Me.state
		End Get
	End Property

	' Token: 0x14000093 RID: 147
	' (add) Token: 0x06003E25 RID: 15909 RVA: 0x002236D0 File Offset: 0x00221AD0
	' (remove) Token: 0x06003E26 RID: 15910 RVA: 0x00223708 File Offset: 0x00221B08
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnParryStartEvent As Action

	' Token: 0x14000094 RID: 148
	' (add) Token: 0x06003E27 RID: 15911 RVA: 0x00223740 File Offset: 0x00221B40
	' (remove) Token: 0x06003E28 RID: 15912 RVA: 0x00223778 File Offset: 0x00221B78
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnParryEndEvent As Action

	' Token: 0x17000560 RID: 1376
	' (get) Token: 0x06003E29 RID: 15913 RVA: 0x002237AE File Offset: 0x00221BAE
	' (set) Token: 0x06003E2A RID: 15914 RVA: 0x002237B6 File Offset: 0x00221BB6
	Public Property AttackParryUsed As Boolean Implements IParryAttack.AttackParryUsed

	' Token: 0x17000561 RID: 1377
	' (get) Token: 0x06003E2B RID: 15915 RVA: 0x002237BF File Offset: 0x00221BBF
	' (set) Token: 0x06003E2C RID: 15916 RVA: 0x002237C7 File Offset: 0x00221BC7
	Public Property HasHitEnemy As Boolean Implements IParryAttack.HasHitEnemy

	' Token: 0x06003E2D RID: 15917 RVA: 0x002237D0 File Offset: 0x00221BD0
	Private Sub Start()
		AddHandler MyBase.player.motor.OnParryEvent, AddressOf Me.StartParry
		AddHandler MyBase.player.motor.OnGroundedEvent, AddressOf Me.OnGround
	End Sub

	' Token: 0x06003E2E RID: 15918 RVA: 0x0022380A File Offset: 0x00221C0A
	Private Sub OnGround()
		Me.AttackParryUsed = False
	End Sub

	' Token: 0x06003E2F RID: 15919 RVA: 0x00223813 File Offset: 0x00221C13
	Public Overrides Sub OnLevelStart()
		MyBase.OnLevelStart()
		Me.state = LevelPlayerParryController.ParryState.Ready
	End Sub

	' Token: 0x06003E30 RID: 15920 RVA: 0x00223822 File Offset: 0x00221C22
	Private Sub StartParry()
		Me.state = LevelPlayerParryController.ParryState.Parrying
		If Me.OnParryStartEvent IsNot Nothing Then
			Me.OnParryStartEvent()
		End If
		MyBase.StartCoroutine(Me.parry_cr())
	End Sub

	' Token: 0x06003E31 RID: 15921 RVA: 0x00223850 File Offset: 0x00221C50
	Private Iterator Function parry_cr() As IEnumerator
		Me.effect.Create(MyBase.player)
		Yield CupheadTime.WaitForSeconds(Me, 0.2F)
		Me.state = LevelPlayerParryController.ParryState.Ready
		If Me.OnParryEndEvent IsNot Nothing Then
			Me.OnParryEndEvent()
		End If
		Me.HasHitEnemy = False
		Return
	End Function

	' Token: 0x06003E32 RID: 15922 RVA: 0x0022386B File Offset: 0x00221C6B
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me.effect = Nothing
	End Sub

	' Token: 0x04004559 RID: 17753
	Public Const DURATION As Single = 0.2F

	' Token: 0x0400455A RID: 17754
	Private state As LevelPlayerParryController.ParryState

	' Token: 0x0400455B RID: 17755
	<SerializeField()>
	Private effect As LevelPlayerParryEffect

	' Token: 0x02000A36 RID: 2614
	Public Enum ParryState
		' Token: 0x04004561 RID: 17761
		Init
		' Token: 0x04004562 RID: 17762
		Ready
		' Token: 0x04004563 RID: 17763
		Parrying
	End Enum
End Class
