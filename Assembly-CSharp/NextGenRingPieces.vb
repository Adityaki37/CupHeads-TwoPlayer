Imports System
Imports UnityEngine

' Token: 0x02000B8D RID: 2957
<Serializable()>
Public Class NextGenRingPieces
	' Token: 0x0600480B RID: 18443 RVA: 0x0025D7DC File Offset: 0x0025BBDC
	Public Function getPieces() As Texture()
		If Me._pieces Is Nothing Then
			Me._pieces = New Texture(5) {}
			Me._pieces(0) = Me.topRight
			Me._pieces(1) = Me.middleRight
			Me._pieces(2) = Me.bottomRight
			Me._pieces(3) = Me.topLeft
			Me._pieces(4) = Me.middleLeft
			Me._pieces(5) = Me.bottomLeft
		End If
		Return Me._pieces
	End Function

	' Token: 0x04004D42 RID: 19778
	Public topLeft As Texture

	' Token: 0x04004D43 RID: 19779
	Public topRight As Texture

	' Token: 0x04004D44 RID: 19780
	Public middleLeft As Texture

	' Token: 0x04004D45 RID: 19781
	Public middleRight As Texture

	' Token: 0x04004D46 RID: 19782
	Public bottomLeft As Texture

	' Token: 0x04004D47 RID: 19783
	Public bottomRight As Texture

	' Token: 0x04004D48 RID: 19784
	Private _pieces As Texture()
End Class
