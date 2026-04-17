Imports System
Imports System.Collections.Generic
Imports UnityEngine

' Token: 0x0200042E RID: 1070
Public Class StringVariantGenerator
	' Token: 0x06000F9E RID: 3998 RVA: 0x0009C004 File Offset: 0x0009A404
	Private Sub New()
		Me.characterGenerators = New Dictionary(Of Char, StringVariantGenerator.CharacterGenerator)() From { { "a"c, New StringVariantGenerator.CharacterGenerator("aA*", Nothing, Nothing) }, { "b"c, New StringVariantGenerator.CharacterGenerator("bB(", Nothing, Nothing) }, { "c"c, New StringVariantGenerator.CharacterGenerator("cC)", Nothing, Nothing) }, { "d"c, New StringVariantGenerator.CharacterGenerator("dD", Nothing, Nothing) }, { "e"c, New StringVariantGenerator.CharacterGenerator("eE&", Nothing, Nothing) }, { "f"c, New StringVariantGenerator.CharacterGenerator("fF", Nothing, Nothing) }, { "g"c, New StringVariantGenerator.CharacterGenerator("gG", Nothing, Nothing) }, { "h"c, New StringVariantGenerator.CharacterGenerator("hH-", Nothing, Nothing) }, { "i"c, New StringVariantGenerator.CharacterGenerator("iI", Nothing, Nothing) }, { "j"c, New StringVariantGenerator.CharacterGenerator("jJ", Nothing, Nothing) }, { "k"c, New StringVariantGenerator.CharacterGenerator("kK", Nothing, Nothing) }, { "l"c, New StringVariantGenerator.CharacterGenerator("lL%", Nothing, Nothing) }, { "m"c, New StringVariantGenerator.CharacterGenerator("mM", Nothing, Nothing) }, { "n"c, New StringVariantGenerator.CharacterGenerator("nN^", Nothing, Nothing) }, { "o"c, New StringVariantGenerator.CharacterGenerator("oO+", Nothing, Nothing) }, { "p"c, New StringVariantGenerator.CharacterGenerator("pP", Nothing, Nothing) }, { "q"c, New StringVariantGenerator.CharacterGenerator("qQ", Nothing, Nothing) }, { "r"c, New StringVariantGenerator.CharacterGenerator("rR@", Nothing, Nothing) }, { "s"c, New StringVariantGenerator.CharacterGenerator("sS#", Nothing, Nothing) }, { "t"c, New StringVariantGenerator.CharacterGenerator("tT$", Nothing, Nothing) }, { "u"c, New StringVariantGenerator.CharacterGenerator("uU", Nothing, Nothing) }, { "v"c, New StringVariantGenerator.CharacterGenerator("vV", Nothing, Nothing) }, { "w"c, New StringVariantGenerator.CharacterGenerator("wW", Nothing, Nothing) }, { "x"c, New StringVariantGenerator.CharacterGenerator("xX", Nothing, Nothing) }, { "y"c, New StringVariantGenerator.CharacterGenerator("yY", Nothing, Nothing) }, { "z"c, New StringVariantGenerator.CharacterGenerator("zZ", Nothing, Nothing) }, { "-"c, New StringVariantGenerator.CharacterGenerator(":;", New List(Of String)() From { "[" }, New List(Of String)() From { "[" }) }, { "!"c, New StringVariantGenerator.CharacterGenerator("!1", New List(Of String)() From { "[", "{", "[[", "{{" }, Nothing) }, { "~"c, New StringVariantGenerator.CharacterGenerator("~`", New List(Of String)() From { "[", "{", "[[", "{{" }, Nothing) }, { "'"c, New StringVariantGenerator.CharacterGenerator("'""", New List(Of String)() From { "[", "{", String.Empty }, New List(Of String)() From { "[", "{", String.Empty }) }, { "."c, New StringVariantGenerator.CharacterGenerator(".>", New List(Of String)() From { "[", "{", "[[", "{{" }, Nothing) }, { ","c, New StringVariantGenerator.CharacterGenerator(",<", New List(Of String)() From { "[", "{", String.Empty }, New List(Of String)() From { "[", "{", String.Empty }) }, { "?"c, New StringVariantGenerator.CharacterGenerator("?/", New List(Of String)() From { "[", "{", "[[", "{{" }, Nothing) }, { " "c, New StringVariantGenerator.CharacterGenerator("   ]  }", Nothing, Nothing) } }
	End Sub

	' Token: 0x1700026B RID: 619
	' (get) Token: 0x06000F9F RID: 3999 RVA: 0x0009C452 File Offset: 0x0009A852
	Public Shared ReadOnly Property Instance As StringVariantGenerator
		Get
			If StringVariantGenerator._instance Is Nothing Then
				StringVariantGenerator._instance = New StringVariantGenerator()
			End If
			Return StringVariantGenerator._instance
		End Get
	End Property

	' Token: 0x06000FA0 RID: 4000 RVA: 0x0009C470 File Offset: 0x0009A870
	Public Function Generate(input As String) As String
		For Each characterGenerator As StringVariantGenerator.CharacterGenerator In Me.characterGenerators.Values
			characterGenerator.Init()
		Next
		Dim text As String = String.Empty
		Dim flag As Boolean = False
		input = input.Replace(" -", "-")
		input = input.Replace("- ", "-")
		For Each c As Char In input
			If c = "<"c Then
				flag = True
			End If
			If c = ">"c Then
				flag = False
			End If
			If Not flag AndAlso Me.characterGenerators.ContainsKey(c) Then
				text += Me.characterGenerators(c).generate()
			Else
				text += c
			End If
		Next
		Return text
	End Function

	' Token: 0x040018D2 RID: 6354
	Private Shared _instance As StringVariantGenerator

	' Token: 0x040018D3 RID: 6355
	Private characterGenerators As Dictionary(Of Char, StringVariantGenerator.CharacterGenerator)

	' Token: 0x0200042F RID: 1071
	Private Class CharacterGenerator
		' Token: 0x06000FA1 RID: 4001 RVA: 0x0009C584 File Offset: 0x0009A984
		Public Sub New(variants As String, Optional randomPrefixes As List(Of String) = Nothing, Optional randomSuffixes As List(Of String) = Nothing)
			Me.variants = variants
			Me.randomPrefixes = randomPrefixes
			Me.randomSuffixes = randomSuffixes
		End Sub

		' Token: 0x06000FA2 RID: 4002 RVA: 0x0009C5A1 File Offset: 0x0009A9A1
		Public Sub Init()
			Me.currentIndex = Global.UnityEngine.Random.Range(0, Me.variants.Length)
		End Sub

		' Token: 0x06000FA3 RID: 4003 RVA: 0x0009C5BC File Offset: 0x0009A9BC
		Public Function generate() As String
			Me.currentIndex = (Me.currentIndex + 1) Mod Me.variants.Length
			Dim text As String = Me.variants(Me.currentIndex).ToString()
			If Me.randomPrefixes IsNot Nothing Then
				text = Me.randomPrefixes.RandomChoice() + text
			End If
			If Me.randomSuffixes IsNot Nothing Then
				text += Me.randomSuffixes.RandomChoice()
			End If
			Return text
		End Function

		' Token: 0x040018D4 RID: 6356
		Private currentIndex As Integer

		' Token: 0x040018D5 RID: 6357
		Private variants As String

		' Token: 0x040018D6 RID: 6358
		Private randomPrefixes As List(Of String)

		' Token: 0x040018D7 RID: 6359
		Private randomSuffixes As List(Of String)
	End Class
End Class
