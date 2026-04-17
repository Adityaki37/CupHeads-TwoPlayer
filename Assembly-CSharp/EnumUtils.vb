Imports System
Imports UnityEngine

' Token: 0x02000391 RID: 913
Public Module EnumUtils
	' Token: 0x06000AFF RID: 2815 RVA: 0x00081A52 File Offset: 0x0007FE52
	Public Function GetValues(Of T)() As T()
		If Not GetType(T).IsEnum Then
			Throw New ArgumentException("T must be an enum type")
		End If
		Return CType([Enum].GetValues(GetType(T)), T())
	End Function

	' Token: 0x06000B00 RID: 2816 RVA: 0x00081A88 File Offset: 0x0007FE88
	Public Function GetValuesAsStrings(Of T)() As String()
		Dim values As T() = EnumUtils.GetValues(Of T)()
		Dim array As String() = New String(values.Length - 1) {}
		For i As Integer = 0 To values.Length - 1
			array(i) = values(i).ToString()
		Next
		Return array
	End Function

	' Token: 0x06000B01 RID: 2817 RVA: 0x00081AD0 File Offset: 0x0007FED0
	Public Function GetCount(Of T)() As Integer
		Return EnumUtils.GetValues(Of T)().Length
	End Function

	' Token: 0x06000B02 RID: 2818 RVA: 0x00081ADC File Offset: 0x0007FEDC
	Public Function Random(Of T)() As T
		Dim values As T() = EnumUtils.GetValues(Of T)()
		Return values(Global.UnityEngine.Random.Range(0, values.Length))
	End Function

	' Token: 0x06000B03 RID: 2819 RVA: 0x00081B00 File Offset: 0x0007FF00
	Public Function Parse(Of T)(name As String) As T
		Dim values As T() = EnumUtils.GetValues(Of T)()
		For i As Integer = 0 To values.Length - 1
			If name = values(i).ToString() Then
				Return values(i)
			End If
		Next
		Return values(0)
	End Function

	' Token: 0x06000B04 RID: 2820 RVA: 0x00081B58 File Offset: 0x0007FF58
	Public Function TryParse(Of T)(name As String, <System.Runtime.InteropServices.OutAttribute()> ByRef result As T) As Boolean
		Dim values As T() = EnumUtils.GetValues(Of T)()
		For i As Integer = 0 To values.Length - 1
			If name = values(i).ToString() Then
				result = values(i)
				Return True
			End If
		Next
		result = values(0)
		Return False
	End Function
End Module
