Imports System
Imports System.Collections
Imports TMPro
Imports UnityEngine

' Token: 0x0200038A RID: 906
Public Class UITMPAnimator
	Inherits AbstractMonoBehaviour

	' Token: 0x06000ABF RID: 2751 RVA: 0x000803E0 File Offset: 0x0007E7E0
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.text = MyBase.GetComponent(Of TextMeshProUGUI)()
		MyBase.StartCoroutine(Me.animateCharacters_cr())
		Me.ignoreGlobalTime = True
	End Sub

	' Token: 0x06000AC0 RID: 2752 RVA: 0x00080408 File Offset: 0x0007E808
	Private Iterator Function animateCharacters_cr() As IEnumerator
		Me.text.havePropertiesChanged = True
		While True
			Me.text.ForceMeshUpdate()
			Dim textInfo As TMP_TextInfo = Me.text.textInfo
			Dim characterCount As Integer = textInfo.characterCount
			If characterCount <> 0 Then
				For i As Integer = 0 To characterCount - 1
					If textInfo.characterInfo(i).isVisible Then
						Dim vector As Vector3 = New Vector3(Global.UnityEngine.Random.Range(-0.25F, 0.25F), Global.UnityEngine.Random.Range(-0.25F, 0.25F), 0F)
						Dim vertexIndex As Integer = CInt(textInfo.characterInfo(i).vertexIndex)
						Dim materialReferenceIndex As Integer = textInfo.characterInfo(i).materialReferenceIndex
						Dim vertices As Vector3() = textInfo.meshInfo(materialReferenceIndex).vertices
						vertices(vertexIndex) += vector
						vertices(vertexIndex + 1) += vector
						vertices(vertexIndex + 2) += vector
						vertices(vertexIndex + 3) += vector
					End If
				Next
				Me.text.UpdateVertexData()
				Yield New WaitForSeconds(0.07F)
			End If
		End While
		Return
	End Function

	' Token: 0x06000AC1 RID: 2753 RVA: 0x00080424 File Offset: 0x0007E824
	Private Iterator Function updateTextLikeAMoron_cr() As IEnumerator
		Me.text.transform.SetScale(New Single?(1F), New Single?(1F), New Single?(0.99F))
		Yield Nothing
		Me.text.transform.SetScale(New Single?(1F), New Single?(1F), New Single?(1F))
		Return
	End Function

	' Token: 0x04001491 RID: 5265
	Private text As TextMeshProUGUI
End Class
