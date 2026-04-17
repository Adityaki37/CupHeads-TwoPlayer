Imports System
Imports UnityEngine

' Token: 0x02000673 RID: 1651
Public Class FlyingGenieLevelMummyDeathEffect
	Inherits Effect

	' Token: 0x060022BD RID: 8893 RVA: 0x001463E8 File Offset: 0x001447E8
	Public Function Create(pos As Vector3, purpleColor As Color) As FlyingGenieLevelMummyDeathEffect
		Dim flyingGenieLevelMummyDeathEffect As FlyingGenieLevelMummyDeathEffect = TryCast(MyBase.Create(pos), FlyingGenieLevelMummyDeathEffect)
		flyingGenieLevelMummyDeathEffect.transform.position = pos
		flyingGenieLevelMummyDeathEffect.purpleColor = purpleColor
		Return flyingGenieLevelMummyDeathEffect
	End Function

	' Token: 0x060022BE RID: 8894 RVA: 0x00146418 File Offset: 0x00144818
	Private Sub CreateConfetti()
		For Each flyingGenieLevelConfettiParts As FlyingGenieLevelConfettiParts In Me.parts
			flyingGenieLevelConfettiParts.CreatePart(MyBase.transform.position, Me.purpleColor)
		Next
	End Sub

	' Token: 0x04002B5B RID: 11099
	<SerializeField()>
	Private parts As FlyingGenieLevelConfettiParts()

	' Token: 0x04002B5C RID: 11100
	Private purpleColor As Color
End Class
