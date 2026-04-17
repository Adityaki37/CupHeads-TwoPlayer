Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020005C3 RID: 1475
Public Class DicePalaceFlyingHorseLevelLights
	Inherits AbstractPausableComponent

	' Token: 0x06001CC3 RID: 7363 RVA: 0x00107803 File Offset: 0x00105C03
	Private Sub Start()
		MyBase.FrameDelayedCallback(AddressOf Me.GetSprites, 1)
	End Sub

	' Token: 0x06001CC4 RID: 7364 RVA: 0x00107819 File Offset: 0x00105C19
	Private Sub GetSprites()
		MyBase.StartCoroutine(Me.move_cr())
	End Sub

	' Token: 0x06001CC5 RID: 7365 RVA: 0x00107828 File Offset: 0x00105C28
	Private Iterator Function move_cr() As IEnumerator
		While True
			If MyBase.transform.position.x > -640F - Me.size Then
				MyBase.transform.position += Vector3.left * Me.speed * CupheadTime.Delta
			Else
				MyBase.transform.position = New Vector3(640F + Me.size, MyBase.transform.position.y, 0F)
			End If
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x040025B1 RID: 9649
	<SerializeField()>
	Private size As Single = 500F

	' Token: 0x040025B2 RID: 9650
	<SerializeField()>
	Private speed As Single = 30F
End Class
