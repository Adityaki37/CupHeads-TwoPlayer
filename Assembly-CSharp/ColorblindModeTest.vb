Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x0200047D RID: 1149
<RequireComponent(GetType(SpriteRenderer))>
Public Class ColorblindModeTest
	Inherits AbstractMonoBehaviour

	' Token: 0x060011B2 RID: 4530 RVA: 0x000A5EB0 File Offset: 0x000A42B0
	Private Sub Start()
		Me.mat = MyBase.GetComponent(Of SpriteRenderer)().material
		MyBase.StartCoroutine(Me.flash_cr())
	End Sub

	' Token: 0x060011B3 RID: 4531 RVA: 0x000A5ED0 File Offset: 0x000A42D0
	Private Iterator Function flash_cr() As IEnumerator
		Dim goingUp As Boolean = True
		Dim valMin As Single = 0F
		Dim valMax As Single = 2F
		Dim start As Single = valMin
		Dim [end] As Single = valMax
		Dim t As Single = 0F
		Dim time As Single = 0.2F
		While True
			While t < time
				t += CupheadTime.Delta
				Me.mat.SetFloat("_Intensity", Mathf.Lerp(start, [end], t / time))
				Yield Nothing
			End While
			Me.mat.SetFloat("_Intensity", [end])
			goingUp = Not goingUp
			start = If((Not goingUp), valMax, valMin)
			[end] = If((Not goingUp), valMin, valMax)
			t = 0F
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x04001B34 RID: 6964
	Private mat As Material
End Class
