Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000B2C RID: 2860
Public Class WaitForFrameTimePersistent
	Implements IEnumerator

	' Token: 0x06004559 RID: 17753 RVA: 0x00247EF9 File Offset: 0x002462F9
	Public Sub New(frameTime As Single, Optional useUnalteredTime As Boolean = False)
		Me.frameTime = frameTime
		Me.useUnalteredTime = useUnalteredTime
	End Sub

	' Token: 0x1700062D RID: 1581
	' (get) Token: 0x0600455A RID: 17754 RVA: 0x00247F0F File Offset: 0x0024630F
	' (set) Token: 0x0600455B RID: 17755 RVA: 0x00247F17 File Offset: 0x00246317
	Public Property accumulator As Single

	' Token: 0x1700062E RID: 1582
	' (get) Token: 0x0600455C RID: 17756 RVA: 0x00247F20 File Offset: 0x00246320
	' (set) Token: 0x0600455D RID: 17757 RVA: 0x00247F28 File Offset: 0x00246328
	Public Property frameTime As Single

	' Token: 0x1700062F RID: 1583
	' (get) Token: 0x0600455E RID: 17758 RVA: 0x00247F31 File Offset: 0x00246331
	Public ReadOnly Property totalDelta As Single
		Get
			Return Me.frameTime + Me.accumulator
		End Get
	End Property

	' Token: 0x17000630 RID: 1584
	' (get) Token: 0x0600455F RID: 17759 RVA: 0x00247F40 File Offset: 0x00246340
	Public Overridable ReadOnly Property Current As Object Implements System.Collections.IEnumerator.Current
		Get
			Return Nothing
		End Get
	End Property

	' Token: 0x06004560 RID: 17760 RVA: 0x00247F44 File Offset: 0x00246344
	Public Function MoveNext() As Boolean Implements System.Collections.IEnumerator.MoveNext
		Me.accumulator += Me.deltaTime()
		Dim flag As Boolean = Me.accumulator >= Me.frameTime
		If flag Then
			Me.accumulator -= Mathf.Floor(Me.accumulator / Me.frameTime) * Me.frameTime
		End If
		Return Not flag
	End Function

	' Token: 0x06004561 RID: 17761 RVA: 0x00247FA6 File Offset: 0x002463A6
	Protected Overridable Function deltaTime() As Single
		Return If((Not Me.useUnalteredTime), CupheadTime.Delta, Time.deltaTime)
	End Function

	' Token: 0x06004562 RID: 17762 RVA: 0x00247FC7 File Offset: 0x002463C7
	Public Sub Reset() Implements System.Collections.IEnumerator.Reset
		Me.accumulator = 0F
	End Sub

	' Token: 0x04004AF5 RID: 19189
	Private useUnalteredTime As Boolean
End Class
