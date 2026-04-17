Imports System
Imports UnityEngine

' Token: 0x020008A0 RID: 2208
Public Class CircusPlatformingLevelCannonProjectile
	Inherits BasicProjectile

	' Token: 0x06003364 RID: 13156 RVA: 0x001DE640 File Offset: 0x001DCA40
	Public Sub SetColor(color As String)
		Dim num As Integer = Global.UnityEngine.Random.Range(0, 2)
		If color IsNot Nothing Then
			If Not(color = "P") Then
				If Not(color = "G") Then
					If color = "O" Then
						MyBase.animator.SetInteger("Variation", num + 4)
					End If
				Else
					MyBase.animator.SetInteger("Variation", num + 2)
				End If
			Else
				Me.SetParryable(True)
				MyBase.animator.SetInteger("Variation", num)
			End If
		End If
	End Sub

	' Token: 0x04003BB0 RID: 15280
	Private Const VariationParameterName As String = "Variation"

	' Token: 0x04003BB1 RID: 15281
	Private Const Pink As String = "P"

	' Token: 0x04003BB2 RID: 15282
	Private Const Green As String = "G"

	' Token: 0x04003BB3 RID: 15283
	Private Const Orange As String = "O"
End Class
