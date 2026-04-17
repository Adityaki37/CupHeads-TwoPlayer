Imports System

' Token: 0x020005F8 RID: 1528
Public Class DragonLevelPeashot
	Inherits BasicProjectile

	' Token: 0x17000376 RID: 886
	' (get) Token: 0x06001E6B RID: 7787 RVA: 0x00118AE8 File Offset: 0x00116EE8
	' (set) Token: 0x06001E6C RID: 7788 RVA: 0x00118AF0 File Offset: 0x00116EF0
	Public Property color As Integer
		Get
			Return Me._color
		End Get
		Set(value As Integer)
			Me._color = value
			Me.SetColor()
		End Set
	End Property

	' Token: 0x06001E6D RID: 7789 RVA: 0x00118AFF File Offset: 0x00116EFF
	Private Sub SetColor()
		MyBase.animator.SetInteger("Color", Me._color)
		If Me.color = 2 Then
			Me.SetParryable(True)
		End If
	End Sub

	' Token: 0x04002748 RID: 10056
	Private _color As Integer
End Class
