Imports System
Imports UnityEngine

' Token: 0x02000852 RID: 2130
Public Class VeggiesLevelOnionTearsStream
	Inherits AbstractMonoBehaviour

	' Token: 0x06003164 RID: 12644 RVA: 0x001CE484 File Offset: 0x001CC884
	Public Function Create(pos As Vector2, scale As Integer) As VeggiesLevelOnionTearsStream
		Dim veggiesLevelOnionTearsStream As VeggiesLevelOnionTearsStream = Me.InstantiatePrefab(Of VeggiesLevelOnionTearsStream)()
		veggiesLevelOnionTearsStream.transform.SetScale(New Single?(CSng(scale)), New Single?(1F), New Single?(1F))
		veggiesLevelOnionTearsStream.transform.position = pos
		Return veggiesLevelOnionTearsStream
	End Function

	' Token: 0x06003165 RID: 12645 RVA: 0x001CE4D0 File Offset: 0x001CC8D0
	Public Sub [End]()
		If Me.ending Then
			Return
		End If
		Me.ending = True
		MyBase.animator.Play("Out")
	End Sub

	' Token: 0x06003166 RID: 12646 RVA: 0x001CE4F5 File Offset: 0x001CC8F5
	Private Sub OnAnimEnd()
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub

	' Token: 0x040039EE RID: 14830
	Private ending As Boolean
End Class
