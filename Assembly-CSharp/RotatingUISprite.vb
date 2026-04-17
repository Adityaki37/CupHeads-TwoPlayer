Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000B16 RID: 2838
Public Class RotatingUISprite
	Inherits MonoBehaviour

	' Token: 0x060044C9 RID: 17609 RVA: 0x0024699C File Offset: 0x00244D9C
	Private Sub Start()
		MyBase.StartCoroutine(Me.rotate_cr())
	End Sub

	' Token: 0x060044CA RID: 17610 RVA: 0x002469AC File Offset: 0x00244DAC
	Private Iterator Function rotate_cr() As IEnumerator
		While True
			MyBase.transform.Rotate(New Vector3(0F, 0F, Me.speed / CSng(Me.frameRate)))
			Yield CupheadTime.WaitForSeconds(Me, 1F / CSng(Me.frameRate))
		End While
		Return
	End Function

	' Token: 0x04004A83 RID: 19075
	<SerializeField()>
	Private speed As Single = 1F

	' Token: 0x04004A84 RID: 19076
	<SerializeField()>
	Private frameRate As Integer = 12
End Class
