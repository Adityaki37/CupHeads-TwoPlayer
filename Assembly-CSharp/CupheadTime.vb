Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports GCFreeUtils
Imports UnityEngine

' Token: 0x020003F0 RID: 1008
Public Module CupheadTime
	' Token: 0x06000D9F RID: 3487 RVA: 0x0008EC0C File Offset: 0x0008D00C
	Shared Sub New()
		Dim values As CupheadTime.Layer() = EnumUtils.GetValues(Of CupheadTime.Layer)()
		For Each layer As CupheadTime.Layer In values
			CupheadTime.layers.Add(CInt(layer), 1F)
		Next
	End Sub

	' Token: 0x17000247 RID: 583
	' (get) Token: 0x06000DA0 RID: 3488 RVA: 0x0008EC77 File Offset: 0x0008D077
	Public ReadOnly Property Delta As CupheadTime.DeltaObject
		Get
			Return CupheadTime.delta
		End Get
	End Property

	' Token: 0x17000248 RID: 584
	' (get) Token: 0x06000DA1 RID: 3489 RVA: 0x0008EC7E File Offset: 0x0008D07E
	Public ReadOnly Property GlobalDelta As Single
		Get
			Return Time.deltaTime
		End Get
	End Property

	' Token: 0x17000249 RID: 585
	' (get) Token: 0x06000DA2 RID: 3490 RVA: 0x0008EC85 File Offset: 0x0008D085
	Public ReadOnly Property FixedDelta As Single
		Get
			Return Time.fixedDeltaTime * CupheadTime.GlobalSpeed
		End Get
	End Property

	' Token: 0x1700024A RID: 586
	' (get) Token: 0x06000DA3 RID: 3491 RVA: 0x0008EC92 File Offset: 0x0008D092
	' (set) Token: 0x06000DA4 RID: 3492 RVA: 0x0008EC99 File Offset: 0x0008D099
	Public Property GlobalSpeed As Single
		Get
			Return CupheadTime.globalSpeed
		End Get
		Set(value As Single)
			CupheadTime.globalSpeed = Mathf.Clamp(value, 0F, 1F)
			CupheadTime.OnChanged()
		End Set
	End Property

	' Token: 0x06000DA5 RID: 3493 RVA: 0x0008ECB5 File Offset: 0x0008D0B5
	Public Function GetLayerSpeed(layer As CupheadTime.Layer) As Single
		Return CupheadTime.layers(CInt(layer))
	End Function

	' Token: 0x06000DA6 RID: 3494 RVA: 0x0008ECC2 File Offset: 0x0008D0C2
	Public Sub SetLayerSpeed(layer As CupheadTime.Layer, value As Single)
		CupheadTime.layers(CInt(layer)) = value
		CupheadTime.OnChanged()
	End Sub

	' Token: 0x06000DA7 RID: 3495 RVA: 0x0008ECD5 File Offset: 0x0008D0D5
	Public Sub Reset()
		CupheadTime.SetAll(1F)
	End Sub

	' Token: 0x06000DA8 RID: 3496 RVA: 0x0008ECE4 File Offset: 0x0008D0E4
	Public Sub SetAll(value As Single)
		CupheadTime.GlobalSpeed = value
		For Each layer As CupheadTime.Layer In EnumUtils.GetValues(Of CupheadTime.Layer)()
			CupheadTime.layers(CInt(layer)) = value
		Next
		CupheadTime.OnChanged()
	End Sub

	' Token: 0x06000DA9 RID: 3497 RVA: 0x0008ED26 File Offset: 0x0008D126
	Private Sub OnChanged()
		If CupheadTime.OnChangedEvent IsNot Nothing Then
			CupheadTime.OnChangedEvent.[Call]()
		End If
	End Sub

	' Token: 0x06000DAA RID: 3498 RVA: 0x0008ED3C File Offset: 0x0008D13C
	Public Function IsPaused() As Boolean
		Return CupheadTime.GlobalSpeed <= 1E-05F OrElse PauseManager.state = PauseManager.State.Paused
	End Function

	' Token: 0x06000DAB RID: 3499 RVA: 0x0008ED58 File Offset: 0x0008D158
	Public Function WaitForSeconds(m As MonoBehaviour, time As Single) As Coroutine
		Return m.StartCoroutine(CupheadTime.waitForSeconds_cr(time, CupheadTime.Layer.[Default]))
	End Function

	' Token: 0x06000DAC RID: 3500 RVA: 0x0008ED67 File Offset: 0x0008D167
	Public Function WaitForSeconds(m As MonoBehaviour, time As Single, layer As CupheadTime.Layer) As Coroutine
		Return m.StartCoroutine(CupheadTime.waitForSeconds_cr(time, layer))
	End Function

	' Token: 0x06000DAD RID: 3501 RVA: 0x0008ED78 File Offset: 0x0008D178
	Private Iterator Function waitForSeconds_cr(time As Single, layer As CupheadTime.Layer) As IEnumerator
		Dim t As Single = 0F
		While t < time
			t += CupheadTime.Delta(layer)
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06000DAE RID: 3502 RVA: 0x0008ED9A File Offset: 0x0008D19A
	Public Function WaitForUnpause(m As MonoBehaviour) As Coroutine
		Return m.StartCoroutine(CupheadTime.waitForUnpause_cr())
	End Function

	' Token: 0x06000DAF RID: 3503 RVA: 0x0008EDA8 File Offset: 0x0008D1A8
	Private Iterator Function waitForUnpause_cr() As IEnumerator
		While CupheadTime.GlobalSpeed = 0F
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x0400171B RID: 5915
	Private delta As CupheadTime.DeltaObject = New CupheadTime.DeltaObject()

	' Token: 0x0400171C RID: 5916
	Private globalSpeed As Single = 1F

	' Token: 0x0400171D RID: 5917
	Private layers As Dictionary(Of Integer, Single) = New Dictionary(Of Integer, Single)()

	' Token: 0x0400171E RID: 5918
	Public OnChangedEvent As GCFreeActionList = New GCFreeActionList(200, True)

	' Token: 0x020003F1 RID: 1009
	Public Enum Layer
		' Token: 0x04001720 RID: 5920
		[Default]
		' Token: 0x04001721 RID: 5921
		Player
		' Token: 0x04001722 RID: 5922
		Enemy
		' Token: 0x04001723 RID: 5923
		UI
	End Enum

	' Token: 0x020003F2 RID: 1010
	Public Class DeltaObject
		' Token: 0x1700024B RID: 587
		Public ReadOnly Default Property Item(layer As CupheadTime.Layer) As Single
			Get
				Return Time.deltaTime * CupheadTime.GetLayerSpeed(layer) * CupheadTime.GlobalSpeed
			End Get
		End Property

		' Token: 0x06000DB2 RID: 3506 RVA: 0x0008EDD8 File Offset: 0x0008D1D8
		Public Shared Widening Operator CType(d As CupheadTime.DeltaObject) As Single
			Return d(CupheadTime.Layer.[Default]) * CupheadTime.GlobalSpeed
		End Operator
	End Class
End Module
