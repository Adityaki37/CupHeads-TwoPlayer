Imports System
Imports UnityEngine

' Token: 0x020007E8 RID: 2024
Public Class SnowCultLevelBatEffect
	Inherits Effect

	' Token: 0x06002E57 RID: 11863 RVA: 0x001B4F7B File Offset: 0x001B337B
	Public Sub SetColor(s As String)
		Me.colorString = s
		MyBase.animator.Play(Me.baseAnimName + s)
		If Me.secondaryRenderer Then
			Me.secondaryRenderer.flipX = Rand.Bool()
		End If
	End Sub

	' Token: 0x040036EA RID: 14058
	<SerializeField()>
	Private secondaryRenderer As SpriteRenderer

	' Token: 0x040036EB RID: 14059
	<SerializeField()>
	Private baseAnimName As String

	' Token: 0x040036EC RID: 14060
	Protected colorString As String
End Class
