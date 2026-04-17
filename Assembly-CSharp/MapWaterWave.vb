Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000966 RID: 2406
Public Class MapWaterWave
	Inherits AbstractPausableComponent

	' Token: 0x06003815 RID: 14357 RVA: 0x002016C3 File Offset: 0x001FFAC3
	Private Sub Start()
		MyBase.StartCoroutine(Me.wave_cr())
	End Sub

	' Token: 0x06003816 RID: 14358 RVA: 0x002016D4 File Offset: 0x001FFAD4
	Private Iterator Function wave_cr() As IEnumerator
		While True
			MyBase.animator.Play("Wave", 0, Me.offsetRange.RandomFloat())
			Yield MyBase.animator.WaitForAnimationToEnd(Me, "Wave", False, True)
			Yield CupheadTime.WaitForSeconds(Me, Me.delayRange.RandomFloat())
		End While
		Return
	End Function

	' Token: 0x04003FF1 RID: 16369
	<SerializeField()>
	Public offsetRange As MinMax

	' Token: 0x04003FF2 RID: 16370
	<SerializeField()>
	Public delayRange As MinMax
End Class
