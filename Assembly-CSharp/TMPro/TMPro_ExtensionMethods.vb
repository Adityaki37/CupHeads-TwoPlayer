Imports System
Imports System.Collections.Generic
Imports UnityEngine

Namespace TMPro
	' Token: 0x02000C98 RID: 3224
	Public Module TMPro_ExtensionMethods
		' Token: 0x0600516E RID: 20846 RVA: 0x002990FC File Offset: 0x002974FC
		<System.Runtime.CompilerServices.ExtensionAttribute()>
		Public Function ArrayToString(chars As Char()) As String
			Dim text As String = String.Empty
			Dim num As Integer = 0
			While num < chars.Length AndAlso chars(num) <> vbNullChar
				text += chars(num)
				num += 1
			End While
			Return text
		End Function

		' Token: 0x0600516F RID: 20847 RVA: 0x0029913C File Offset: 0x0029753C
		<System.Runtime.CompilerServices.ExtensionAttribute()>
		Public Function FindInstanceID(Of T As Global.UnityEngine.[Object])(list As List(Of T), target As T) As Integer
			Dim instanceID As Integer = target.GetInstanceID()
			For i As Integer = 0 To list.Count - 1
				Dim t As T = list(i)
				If t.GetInstanceID() = instanceID Then
					Return i
				End If
			Next
			Return -1
		End Function

		' Token: 0x06005170 RID: 20848 RVA: 0x0029918C File Offset: 0x0029758C
		<System.Runtime.CompilerServices.ExtensionAttribute()>
		Public Function Compare(a As Color32, b As Color32) As Boolean
			Return a.r = b.r AndAlso a.g = b.g AndAlso a.b = b.b AndAlso a.a = b.a
		End Function

		' Token: 0x06005171 RID: 20849 RVA: 0x002991E5 File Offset: 0x002975E5
		<System.Runtime.CompilerServices.ExtensionAttribute()>
		Public Function CompareRGB(a As Color32, b As Color32) As Boolean
			Return a.r = b.r AndAlso a.g = b.g AndAlso a.b = b.b
		End Function

		' Token: 0x06005172 RID: 20850 RVA: 0x00299220 File Offset: 0x00297620
		<System.Runtime.CompilerServices.ExtensionAttribute()>
		Public Function Compare(a As Color, b As Color) As Boolean
			Return a.r = b.r AndAlso a.g = b.g AndAlso a.b = b.b AndAlso a.a = b.a
		End Function

		' Token: 0x06005173 RID: 20851 RVA: 0x00299279 File Offset: 0x00297679
		<System.Runtime.CompilerServices.ExtensionAttribute()>
		Public Function CompareRGB(a As Color, b As Color) As Boolean
			Return a.r = b.r AndAlso a.g = b.g AndAlso a.b = b.b
		End Function

		' Token: 0x06005174 RID: 20852 RVA: 0x002992B4 File Offset: 0x002976B4
		<System.Runtime.CompilerServices.ExtensionAttribute()>
		Public Function Multiply(c1 As Color32, c2 As Color32) As Color32
			Dim b As Byte = CByte((CSng(c1.r) / 255F * (CSng(c2.r) / 255F) * 255F))
			Dim b2 As Byte = CByte((CSng(c1.g) / 255F * (CSng(c2.g) / 255F) * 255F))
			Dim b3 As Byte = CByte((CSng(c1.b) / 255F * (CSng(c2.b) / 255F) * 255F))
			Dim b4 As Byte = CByte((CSng(c1.a) / 255F * (CSng(c2.a) / 255F) * 255F))
			Return New Color32(b, b2, b3, b4)
		End Function

		' Token: 0x06005175 RID: 20853 RVA: 0x00299360 File Offset: 0x00297760
		<System.Runtime.CompilerServices.ExtensionAttribute()>
		Public Function Tint(c1 As Color32, c2 As Color32) As Color32
			Dim b As Byte = CByte((CSng(c1.r) / 255F * (CSng(c2.r) / 255F) * 255F))
			Dim b2 As Byte = CByte((CSng(c1.g) / 255F * (CSng(c2.g) / 255F) * 255F))
			Dim b3 As Byte = CByte((CSng(c1.b) / 255F * (CSng(c2.b) / 255F) * 255F))
			Dim b4 As Byte = CByte((CSng(c1.a) / 255F * (CSng(c2.a) / 255F) * 255F))
			Return New Color32(b, b2, b3, b4)
		End Function

		' Token: 0x06005176 RID: 20854 RVA: 0x0029940C File Offset: 0x0029780C
		<System.Runtime.CompilerServices.ExtensionAttribute()>
		Public Function Tint(c1 As Color32, tint As Single) As Color32
			Dim b As Byte = CByte(Mathf.Clamp(CSng(c1.r) / 255F * tint * 255F, 0F, 255F))
			Dim b2 As Byte = CByte(Mathf.Clamp(CSng(c1.g) / 255F * tint * 255F, 0F, 255F))
			Dim b3 As Byte = CByte(Mathf.Clamp(CSng(c1.b) / 255F * tint * 255F, 0F, 255F))
			Dim b4 As Byte = CByte(Mathf.Clamp(CSng(c1.a) / 255F * tint * 255F, 0F, 255F))
			Return New Color32(b, b2, b3, b4)
		End Function

		' Token: 0x06005177 RID: 20855 RVA: 0x002994C0 File Offset: 0x002978C0
		<System.Runtime.CompilerServices.ExtensionAttribute()>
		Public Function Compare(v1 As Vector3, v2 As Vector3, accuracy As Integer) As Boolean
			Dim flag As Boolean = CInt((v1.x * CSng(accuracy))) = CInt((v2.x * CSng(accuracy)))
			Dim flag2 As Boolean = CInt((v1.y * CSng(accuracy))) = CInt((v2.y * CSng(accuracy)))
			Dim flag3 As Boolean = CInt((v1.z * CSng(accuracy))) = CInt((v2.z * CSng(accuracy)))
			Return flag AndAlso flag2 AndAlso flag3
		End Function

		' Token: 0x06005178 RID: 20856 RVA: 0x00299528 File Offset: 0x00297928
		<System.Runtime.CompilerServices.ExtensionAttribute()>
		Public Function Compare(q1 As Quaternion, q2 As Quaternion, accuracy As Integer) As Boolean
			Dim flag As Boolean = CInt((q1.x * CSng(accuracy))) = CInt((q2.x * CSng(accuracy)))
			Dim flag2 As Boolean = CInt((q1.y * CSng(accuracy))) = CInt((q2.y * CSng(accuracy)))
			Dim flag3 As Boolean = CInt((q1.z * CSng(accuracy))) = CInt((q2.z * CSng(accuracy)))
			Dim flag4 As Boolean = CInt((q1.w * CSng(accuracy))) = CInt((q2.w * CSng(accuracy)))
			Return flag AndAlso flag2 AndAlso flag3 AndAlso flag4
		End Function
	End Module
End Namespace
