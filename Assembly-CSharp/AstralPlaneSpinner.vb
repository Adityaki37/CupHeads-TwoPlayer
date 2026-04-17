Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020006C3 RID: 1731
Public Class AstralPlaneSpinner
	Inherits MonoBehaviour

	' Token: 0x060024CD RID: 9421 RVA: 0x0015902F File Offset: 0x0015742F
	Private Sub Start()
		MyBase.StartCoroutine(Me.rotate_space_bg_cr())
	End Sub

	' Token: 0x060024CE RID: 9422 RVA: 0x00159040 File Offset: 0x00157440
	Private Iterator Function rotate_space_bg_cr() As IEnumerator
		While True
			Yield CupheadTime.WaitForSeconds(Me, 0.041666668F)
			Me.spaceLayers(0).Rotate(New Vector3(0F, 0F, 0.3F))
			Me.spaceLayers(1).Rotate(New Vector3(0F, 0F, 0.5F))
			Me.spaceLayers(2).Rotate(New Vector3(0F, 0F, 1F))
		End While
		Return
	End Function

	' Token: 0x04002D6E RID: 11630
	<SerializeField()>
	Private spaceLayers As Transform()
End Class
