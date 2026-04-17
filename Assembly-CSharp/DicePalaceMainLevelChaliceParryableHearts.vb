Imports System
Imports System.Collections
Imports System.Diagnostics
Imports UnityEngine

' Token: 0x020005D1 RID: 1489
Public Class DicePalaceMainLevelChaliceParryableHearts
	Inherits AbstractProjectile

	' Token: 0x14000041 RID: 65
	' (add) Token: 0x06001D45 RID: 7493 RVA: 0x0010CA94 File Offset: 0x0010AE94
	' (remove) Token: 0x06001D46 RID: 7494 RVA: 0x0010CACC File Offset: 0x0010AECC
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnPrePauseActivate As Action

	' Token: 0x1700036B RID: 875
	' (get) Token: 0x06001D47 RID: 7495 RVA: 0x0010CB02 File Offset: 0x0010AF02
	' (set) Token: 0x06001D48 RID: 7496 RVA: 0x0010CB0A File Offset: 0x0010AF0A
	Public Property IsParryable As Boolean

	' Token: 0x1700036C RID: 876
	' (get) Token: 0x06001D49 RID: 7497 RVA: 0x0010CB13 File Offset: 0x0010AF13
	Public Overrides ReadOnly Property ParryMeterMultiplier As Single
		Get
			Return 0F
		End Get
	End Property

	' Token: 0x06001D4A RID: 7498 RVA: 0x0010CB1A File Offset: 0x0010AF1A
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.SetParryable(True)
		If MyBase.GetComponent(Of Collider2D)() Is Nothing Then
		End If
	End Sub

	' Token: 0x06001D4B RID: 7499 RVA: 0x0010CB3A File Offset: 0x0010AF3A
	Public Overridable Sub OnParryPrePause(player As AbstractPlayerController)
		If Me.parrySpark Then
			Me.parrySpark.Create(MyBase.transform.position)
		End If
	End Sub

	' Token: 0x06001D4C RID: 7500 RVA: 0x0010CB63 File Offset: 0x0010AF63
	Public Overrides Sub OnParry(player As AbstractPlayerController)
		Me.SetParryable(False)
		MyBase.StartCoroutine(Me.parryCooldown_cr())
	End Sub

	' Token: 0x06001D4D RID: 7501 RVA: 0x0010CB7C File Offset: 0x0010AF7C
	Private Iterator Function parryCooldown_cr() As IEnumerator
		Dim t As Single = 0F
		While t < Me.coolDown
			t += CupheadTime.Delta
			Yield Nothing
		End While
		Dim collider As Collider2D = MyBase.GetComponent(Of Collider2D)()
		collider.enabled = True
		Me.SetParryable(True)
		Yield Nothing
		Return
	End Function

	' Token: 0x06001D4E RID: 7502 RVA: 0x0010CB97 File Offset: 0x0010AF97
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me.parrySpark = Nothing
	End Sub

	' Token: 0x0400262D RID: 9773
	<SerializeField()>
	Private parrySpark As Effect

	' Token: 0x0400262E RID: 9774
	<SerializeField()>
	Protected coolDown As Single = 0.4F
End Class
