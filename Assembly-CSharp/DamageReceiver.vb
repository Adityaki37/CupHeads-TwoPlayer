Imports System
Imports System.Collections
Imports System.Diagnostics
Imports UnityEngine

' Token: 0x02000AF2 RID: 2802
Public Class DamageReceiver
	Inherits AbstractPausableComponent

	' Token: 0x17000613 RID: 1555
	' (get) Token: 0x060043F2 RID: 17394 RVA: 0x0023BCDD File Offset: 0x0023A0DD
	' (set) Token: 0x060043F3 RID: 17395 RVA: 0x0023BCE5 File Offset: 0x0023A0E5
	Public Property IsHitPaused As Boolean

	' Token: 0x140000BF RID: 191
	' (add) Token: 0x060043F4 RID: 17396 RVA: 0x0023BCF0 File Offset: 0x0023A0F0
	' (remove) Token: 0x060043F5 RID: 17397 RVA: 0x0023BD28 File Offset: 0x0023A128
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnDamageTaken As DamageReceiver.OnDamageTakenHandler

	' Token: 0x060043F6 RID: 17398 RVA: 0x0023BD60 File Offset: 0x0023A160
	Protected Overrides Sub Awake()
		MyBase.Awake()
		If MyBase.animator IsNot Nothing Then
			Me.animHelper = MyBase.animator.GetComponent(Of AnimationHelper)()
		End If
		If Me.type = DamageReceiver.Type.Other Then
			Return
		End If
		MyBase.tag = Me.type.ToString()
		Me.IsHitPaused = False
	End Sub

	' Token: 0x060043F7 RID: 17399 RVA: 0x0023BDC0 File Offset: 0x0023A1C0
	Public Overridable Sub TakeDamage(info As DamageDealer.DamageInfo)
		If Not MyBase.enabled Then
			Return
		End If
		If Me.OnDamageTaken IsNot Nothing Then
			If DamageReceiver.DEBUG_DO_MEGA_DAMAGE AndAlso (Me.type = DamageReceiver.Type.Enemy OrElse Me.type = DamageReceiver.Type.Other) Then
				info.SetEditorPlayer()
			End If
			Me.OnDamageTaken(info)
			If(Me.type = DamageReceiver.Type.Enemy OrElse Me.type = DamageReceiver.Type.Other) AndAlso info.damageSource = DamageDealer.DamageSource.Super Then
				MyBase.StartCoroutine(Me.pauseAnim_cr())
			End If
		End If
	End Sub

	' Token: 0x060043F8 RID: 17400 RVA: 0x0023BE47 File Offset: 0x0023A247
	Public Overridable Sub TakeDamageBruteForce(info As DamageDealer.DamageInfo)
		If Me.OnDamageTaken IsNot Nothing Then
			Me.OnDamageTaken(info)
		End If
	End Sub

	' Token: 0x060043F9 RID: 17401 RVA: 0x0023BE60 File Offset: 0x0023A260
	Private Iterator Function pauseAnim_cr() As IEnumerator
		If Me.animHelper IsNot Nothing Then
			Me.animHelper.Speed = 0F
		End If
		If MyBase.animator IsNot Nothing Then
			MyBase.animator.enabled = False
		End If
		For i As Integer = 0 To Me.animatorsEffectedByPause.Length - 1
			Me.animatorsEffectedByPause(i).GetComponent(Of Animator)().enabled = False
			Me.animatorsEffectedByPause(i).Speed = 0F
		Next
		Me.IsHitPaused = True
		CupheadLevelCamera.Current.Shake(10F, 0.6F, False)
		Yield CupheadTime.WaitForSeconds(Me, 0.15F)
		Me.IsHitPaused = False
		If MyBase.animator IsNot Nothing Then
			MyBase.animator.enabled = True
		End If
		If Me.animHelper IsNot Nothing Then
			Me.animHelper.Speed = 1F
		End If
		For j As Integer = 0 To Me.animatorsEffectedByPause.Length - 1
			Me.animatorsEffectedByPause(j).GetComponent(Of Animator)().enabled = True
			Me.animatorsEffectedByPause(j).Speed = 1F
		Next
		Return
	End Function

	' Token: 0x060043FA RID: 17402 RVA: 0x0023BE7C File Offset: 0x0023A27C
	Public Shared Sub Debug_ToggleMegaDamage()
		DamageReceiver.DEBUG_DO_MEGA_DAMAGE = Not DamageReceiver.DEBUG_DO_MEGA_DAMAGE
		Dim text As String = If((Not DamageReceiver.DEBUG_DO_MEGA_DAMAGE), "red", "green")
	End Sub

	' Token: 0x04004991 RID: 18833
	Public Const ENEMY_HIT_PAUSE_TIME As Single = 0.15F

	' Token: 0x04004992 RID: 18834
	Public type As DamageReceiver.Type

	' Token: 0x04004993 RID: 18835
	Public animatorsEffectedByPause As AnimationHelper()

	' Token: 0x04004994 RID: 18836
	<NonSerialized()>
	Public OffScreenPadding As Vector2 = New Vector2(50F, 50F)

	' Token: 0x04004995 RID: 18837
	Private animHelper As AnimationHelper

	' Token: 0x04004998 RID: 18840
	Public Shared DEBUG_DO_MEGA_DAMAGE As Boolean

	' Token: 0x02000AF3 RID: 2803
	Public Enum Type
		' Token: 0x0400499A RID: 18842
		Enemy
		' Token: 0x0400499B RID: 18843
		Player
		' Token: 0x0400499C RID: 18844
		Other
	End Enum

	' Token: 0x02000AF4 RID: 2804
	' (Invoke) Token: 0x060043FD RID: 17405
	Public Delegate Sub OnDamageTakenHandler(info As DamageDealer.DamageInfo)
End Class
