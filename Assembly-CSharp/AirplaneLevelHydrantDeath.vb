Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020004BC RID: 1212
Public Class AirplaneLevelHydrantDeath
	Inherits MonoBehaviour

	' Token: 0x06001412 RID: 5138 RVA: 0x000B29D5 File Offset: 0x000B0DD5
	Private Sub Start()
		If Me.pieces Then
			MyBase.StartCoroutine(Me.recede_cr())
		End If
	End Sub

	' Token: 0x06001413 RID: 5139 RVA: 0x000B29F4 File Offset: 0x000B0DF4
	Private Iterator Function recede_cr() As IEnumerator
		Dim startPos As Vector3 = MyBase.transform.position
		Dim endPos As Vector3 = New Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y - 100F, MyBase.transform.position.z)
		endPos = Vector3.Lerp(startPos, endPos, 0.35F)
		While True
			Dim t As Single = Me.anim.GetCurrentAnimatorStateInfo(0).normalizedTime
			Me.pieces.transform.position = Vector3.Lerp(startPos, endPos, EaseUtils.EaseInSine(0F, 1F, t))
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x04001D3E RID: 7486
	<SerializeField()>
	Private anim As Animator

	' Token: 0x04001D3F RID: 7487
	<SerializeField()>
	Private pieces As GameObject
End Class
