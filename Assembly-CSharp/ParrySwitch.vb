Imports System
Imports System.Collections
Imports System.Diagnostics
Imports UnityEngine

' Token: 0x02000B25 RID: 2853
Public Class ParrySwitch
	Inherits AbstractSwitch

	' Token: 0x140000D2 RID: 210
	' (add) Token: 0x0600451C RID: 17692 RVA: 0x000B2C20 File Offset: 0x000B1020
	' (remove) Token: 0x0600451D RID: 17693 RVA: 0x000B2C58 File Offset: 0x000B1058
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnPrePauseActivate As Action

	' Token: 0x1700062A RID: 1578
	' (get) Token: 0x0600451E RID: 17694 RVA: 0x000B2C8E File Offset: 0x000B108E
	' (set) Token: 0x0600451F RID: 17695 RVA: 0x000B2C96 File Offset: 0x000B1096
	Public Property IsParryable As Boolean

	' Token: 0x06004520 RID: 17696 RVA: 0x000B2C9F File Offset: 0x000B109F
	Protected Sub FirePrePauseEvent()
		If Me.OnPrePauseActivate IsNot Nothing Then
			Me.OnPrePauseActivate()
		End If
	End Sub

	' Token: 0x06004521 RID: 17697 RVA: 0x000B2CB7 File Offset: 0x000B10B7
	Protected Overrides Sub Awake()
		MyBase.Awake()
		MyBase.tag = "ParrySwitch"
		Me.IsParryable = True
		If MyBase.GetComponent(Of Collider2D)() Is Nothing Then
		End If
	End Sub

	' Token: 0x06004522 RID: 17698 RVA: 0x000B2CE2 File Offset: 0x000B10E2
	Public Overridable Sub OnParryPrePause(player As AbstractPlayerController)
		If Me.parrySpark Then
			Me.parrySpark.Create(MyBase.transform.position)
		End If
		Me.FirePrePauseEvent()
	End Sub

	' Token: 0x06004523 RID: 17699 RVA: 0x000B2D11 File Offset: 0x000B1111
	Public Overridable Sub OnParryPostPause(player As AbstractPlayerController)
		MyBase.DispatchEvent()
	End Sub

	' Token: 0x06004524 RID: 17700 RVA: 0x000B2D19 File Offset: 0x000B1119
	Public Sub ActivateFromOtherSource()
		If Me.parrySpark Then
			Me.parrySpark.Create(MyBase.transform.position)
		End If
		MyBase.DispatchEvent()
	End Sub

	' Token: 0x06004525 RID: 17701 RVA: 0x000B2D48 File Offset: 0x000B1148
	Public Sub StartParryCooldown()
		MyBase.GetComponent(Of Collider2D)().enabled = False
		MyBase.StartCoroutine(Me.parryCooldown_cr())
	End Sub

	' Token: 0x06004526 RID: 17702 RVA: 0x000B2D64 File Offset: 0x000B1164
	Private Iterator Function parryCooldown_cr() As IEnumerator
		Dim t As Single = 0F
		While t < Me.coolDown
			t += CupheadTime.Delta
			Yield Nothing
		End While
		Dim collider As Collider2D = MyBase.GetComponent(Of Collider2D)()
		collider.enabled = True
		Yield Nothing
		Return
	End Function

	' Token: 0x06004527 RID: 17703 RVA: 0x000B2D7F File Offset: 0x000B117F
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me.parrySpark = Nothing
	End Sub

	' Token: 0x04004ADA RID: 19162
	<SerializeField()>
	Protected parrySpark As Effect

	' Token: 0x04004ADB RID: 19163
	<SerializeField()>
	Protected coolDown As Single = 0.4F
End Class
