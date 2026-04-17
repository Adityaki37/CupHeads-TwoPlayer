Imports System
Imports UnityEngine
Imports UnityEngine.UI

' Token: 0x02000451 RID: 1105
Public Class AchievementIcon
	Inherits MonoBehaviour

	' Token: 0x0600109E RID: 4254 RVA: 0x0009F9C9 File Offset: 0x0009DDC9
	Private Sub Awake()
		Me.image = MyBase.GetComponent(Of Image)()
	End Sub

	' Token: 0x0600109F RID: 4255 RVA: 0x0009F9D7 File Offset: 0x0009DDD7
	Public Sub SetIcon(sprite As Sprite)
		Me.image.sprite = sprite
	End Sub

	' Token: 0x040019D3 RID: 6611
	Private image As Image
End Class
