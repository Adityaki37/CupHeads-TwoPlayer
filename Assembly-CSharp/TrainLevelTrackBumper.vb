Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x0200082D RID: 2093
Public Class TrainLevelTrackBumper
	Inherits AbstractPausableComponent

	' Token: 0x06003099 RID: 12441 RVA: 0x001C98FA File Offset: 0x001C7CFA
	Protected Overrides Sub Awake()
		MyBase.Awake()
		MyBase.StartCoroutine(Me.main_cr())
	End Sub

	' Token: 0x0600309A RID: 12442 RVA: 0x001C9910 File Offset: 0x001C7D10
	Private Iterator Function main_cr() As IEnumerator
		Dim startingY As Single = MyBase.transform.position.y
		Dim t As Single = 0F
		While True
			t += CupheadTime.Delta
			Dim d As Single = (MyBase.transform.position.x + 6000F * t) Mod 4500F
			Dim y As Single = startingY
			If d < 600F Then
				Dim num As Single = d / 600F
				y += Mathf.Sin(num * 3.1415927F) * 3F
			End If
			MyBase.transform.SetPosition(Nothing, New Single?(y), Nothing)
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x04003937 RID: 14647
	Private Const bumpSpeed As Single = 6000F

	' Token: 0x04003938 RID: 14648
	Private Const bumpHeight As Single = 3F

	' Token: 0x04003939 RID: 14649
	Private Const bumpDuration As Single = 0.1F

	' Token: 0x0400393A RID: 14650
	Private Const bumpPeriod As Single = 0.75F
End Class
