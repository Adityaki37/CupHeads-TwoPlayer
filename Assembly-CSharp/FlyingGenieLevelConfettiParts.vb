Imports System
Imports UnityEngine

' Token: 0x02000666 RID: 1638
Public Class FlyingGenieLevelConfettiParts
	Inherits SpriteDeathParts

	' Token: 0x06002222 RID: 8738 RVA: 0x0013DF0C File Offset: 0x0013C30C
	Public Function CreatePart(position As Vector3, purpleColor As Color) As FlyingGenieLevelConfettiParts
		Dim flyingGenieLevelConfettiParts As FlyingGenieLevelConfettiParts = TryCast(MyBase.CreatePart(position), FlyingGenieLevelConfettiParts)
		flyingGenieLevelConfettiParts.purpleVersion.color = purpleColor
		Return flyingGenieLevelConfettiParts
	End Function

	' Token: 0x04002AC9 RID: 10953
	<SerializeField()>
	Private purpleVersion As SpriteRenderer
End Class
