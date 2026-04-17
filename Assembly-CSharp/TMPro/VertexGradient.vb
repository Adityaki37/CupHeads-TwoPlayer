Imports System
Imports UnityEngine

Namespace TMPro
	' Token: 0x02000CA6 RID: 3238
	<Serializable()>
	Public Structure VertexGradient
		' Token: 0x06005191 RID: 20881 RVA: 0x00299EAE File Offset: 0x002982AE
		Public Sub New(color As Color)
			Me.topLeft = color
			Me.topRight = color
			Me.bottomLeft = color
			Me.bottomRight = color
		End Sub

		' Token: 0x06005192 RID: 20882 RVA: 0x00299ECC File Offset: 0x002982CC
		Public Sub New(color0 As Color, color1 As Color, color2 As Color, color3 As Color)
			Me.topLeft = color0
			Me.topRight = color1
			Me.bottomLeft = color2
			Me.bottomRight = color3
		End Sub

		' Token: 0x04005476 RID: 21622
		Public topLeft As Color

		' Token: 0x04005477 RID: 21623
		Public topRight As Color

		' Token: 0x04005478 RID: 21624
		Public bottomLeft As Color

		' Token: 0x04005479 RID: 21625
		Public bottomRight As Color
	End Structure
End Namespace
