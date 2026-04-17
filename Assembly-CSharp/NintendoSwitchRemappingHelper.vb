Imports System
Imports System.Collections.Generic

' Token: 0x020009CC RID: 2508
Public Module NintendoSwitchRemappingHelper
	' Token: 0x06003AEE RID: 15086 RVA: 0x00212768 File Offset: 0x00210B68
	Public Sub DuplicateXML(updatedMappings As Dictionary(Of String, String), existingMappings As Dictionary(Of String, String))
		If updatedMappings.Count > 1 Then
			Throw New Exception("More than one mapping found!")
		End If
		For Each keyValuePair As KeyValuePair(Of String, String) In updatedMappings
			Dim text As String = Nothing
			If keyValuePair.Key.Contains(NintendoSwitchRemappingHelper.P1Identifier) Then
				text = NintendoSwitchRemappingHelper.P1Identifier
			ElseIf keyValuePair.Key.Contains(NintendoSwitchRemappingHelper.P2Identifier) Then
				text = NintendoSwitchRemappingHelper.P2Identifier
			End If
			If text Is Nothing Then
				Throw New Exception("Unable to determine controller mapping origin")
			End If
			Dim dictionary As Dictionary(Of String, String) = Nothing
			Dim text2 As String = Nothing
			Dim text3 As String = Nothing
			For Each keyValuePair2 As KeyValuePair(Of String, String) In NintendoSwitchRemappingHelper.DualControllers
				If keyValuePair.Key.Contains(keyValuePair2.Key) Then
					text2 = keyValuePair2.Key
					text3 = keyValuePair2.Value
					dictionary = NintendoSwitchRemappingHelper.DualControllers
					Exit For
				End If
			Next
			For Each keyValuePair3 As KeyValuePair(Of String, String) In NintendoSwitchRemappingHelper.SingleControllers
				If keyValuePair.Key.Contains(keyValuePair3.Key) Then
					text2 = keyValuePair3.Key
					text3 = keyValuePair3.Value
					dictionary = NintendoSwitchRemappingHelper.SingleControllers
					Exit For
				End If
			Next
			If dictionary Is Nothing Then
				Throw New Exception("Unable to determine controller search values")
			End If
			Dim value As String = keyValuePair.Value
			For Each keyValuePair4 As KeyValuePair(Of String, String) In dictionary
				Dim key As String = keyValuePair4.Key
				Dim value2 As String = keyValuePair4.Value
				Dim text4 As String = keyValuePair.Key.Replace(text2, key)
				Dim text5 As String = value.Replace(text2, key)
				text5 = text5.Replace(text3, value2)
				existingMappings(text4) = text5
			Next
		Next
	End Sub

	' Token: 0x040042AA RID: 17066
	Private P1Identifier As String = "r2|0"

	' Token: 0x040042AB RID: 17067
	Private P2Identifier As String = "r2|1"

	' Token: 0x040042AC RID: 17068
	Private DualControllers As Dictionary(Of String, String) = New Dictionary(Of String, String)() From { { "521b808c-0248-4526-bc10-f1d16ee76bf1", "Joy-Con (Dual)" }, { "7bf3154b-9db8-4d52-950f-cd0eed8a5819", "Pro Controller" }, { "1fbdd13b-0795-4173-8a95-a2a75de9d204", "Handheld" } }

	' Token: 0x040042AD RID: 17069
	Private SingleControllers As Dictionary(Of String, String) = New Dictionary(Of String, String)() From { { "3eb01142-da0e-4a86-8ae8-a15c2b1f2a04", "Joy-Con (L)" }, { "605dc720-1b38-473d-a459-67d5857aa6ea", "Joy-Con (R)" } }
End Module
