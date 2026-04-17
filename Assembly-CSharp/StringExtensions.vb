Imports System

' Token: 0x0200036F RID: 879
Public Module StringExtensions
	' Token: 0x06000A0D RID: 2573 RVA: 0x0007DD1C File Offset: 0x0007C11C
	<System.Runtime.CompilerServices.ExtensionAttribute()>
	Public Function UpperFirst(str As String) As String
		If String.IsNullOrEmpty(str) Then
			Return String.Empty
		End If
		Dim array As Char() = str.ToCharArray()
		array(0) = Char.ToUpper(array(0))
		Return New String(array)
	End Function

	' Token: 0x06000A0E RID: 2574 RVA: 0x0007DD54 File Offset: 0x0007C154
	<System.Runtime.CompilerServices.ExtensionAttribute()>
	Public Function LowerFirst(str As String) As String
		If String.IsNullOrEmpty(str) Then
			Return String.Empty
		End If
		Dim array As Char() = str.ToCharArray()
		array(0) = Char.ToLower(array(0))
		Return New String(array)
	End Function

	' Token: 0x06000A0F RID: 2575 RVA: 0x0007DD8C File Offset: 0x0007C18C
	<System.Runtime.CompilerServices.ExtensionAttribute()>
	Public Function UppercaseWords(str As String) As String
		Dim array As Char() = str.ToCharArray()
		If array.Length >= 1 AndAlso Char.IsLower(array(0)) Then
			array(0) = Char.ToUpper(array(0))
		End If
		For i As Integer = 1 To array.Length - 1
			If(array(i - 1) = " "c OrElse array(i - 1) = "_"c OrElse array(i - 1) = "/"c) AndAlso Char.IsLower(array(i)) Then
				array(i) = Char.ToUpper(array(i))
			End If
		Next
		Return New String(array)
	End Function

	' Token: 0x06000A10 RID: 2576 RVA: 0x0007DE18 File Offset: 0x0007C218
	<System.Runtime.CompilerServices.ExtensionAttribute()>
	Public Function ToLowerIfNecessary(str As String) As String
		If str Is Nothing Then
			Throw New NullReferenceException()
		End If
		Dim flag As Boolean = False
		Dim length As Integer = str.Length
		For i As Integer = 0 To length - 1
			If Char.IsUpper(str(i)) Then
				flag = True
				Exit For
			End If
		Next
		If flag Then
			Return str.ToLower()
		End If
		Return str
	End Function
End Module
