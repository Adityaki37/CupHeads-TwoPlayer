Imports System
Imports TMPro
Imports UnityEngine

' Token: 0x0200046D RID: 1133
Public Class TextMeshRandomAngle
	Inherits MonoBehaviour

	' Token: 0x0600115F RID: 4447 RVA: 0x000A4F68 File Offset: 0x000A3368
	Private Sub Start()
		Me.initialAngles = New Single(Me.m_TextComponent.text.Length - 1) {}
		Me.jitterAngles = New Single(Me.m_TextComponent.text.Length - 1) {}
		For i As Integer = 0 To Me.initialAngles.Length - 1
			Me.initialAngles(i) = Global.UnityEngine.Random.Range(-Me.m_AngleAmplitude, Me.m_AngleAmplitude)
		Next
		Me.jitterDelay = 0.083333336F
		Me.ApplyRotation()
	End Sub

	' Token: 0x06001160 RID: 4448 RVA: 0x000A4FF0 File Offset: 0x000A33F0
	Private Sub Update()
		Me.currentJitterDelay -= CupheadTime.Delta
		If Me.currentJitterDelay > 0F Then
			Return
		End If
		Me.currentJitterDelay = Me.jitterDelay
		For i As Integer = 0 To Me.initialAngles.Length - 1
			Me.jitterAngles(i) = Global.UnityEngine.Random.Range(-Me.m_JitterAngleAmplitude, Me.m_JitterAngleAmplitude)
		Next
		Me.ApplyRotation()
	End Sub

	' Token: 0x06001161 RID: 4449 RVA: 0x000A506C File Offset: 0x000A346C
	Private Sub ApplyRotation()
		Me.m_TextComponent.havePropertiesChanged = True
		Me.m_ShadowTextComponent.havePropertiesChanged = True
		Me.m_TextComponent.ForceMeshUpdate()
		Me.m_ShadowTextComponent.ForceMeshUpdate()
		Dim textInfo As TMP_TextInfo = Me.m_TextComponent.textInfo
		Dim characterCount As Integer = textInfo.characterCount
		If characterCount = 0 OrElse Me.m_TextComponent.text.Length = 0 Then
			Return
		End If
		For i As Integer = 0 To characterCount - 1
			If textInfo.characterInfo(i).isVisible Then
				Dim vertexIndex As Integer = CInt(textInfo.characterInfo(i).vertexIndex)
				Dim materialReferenceIndex As Integer = textInfo.characterInfo(i).materialReferenceIndex
				Dim vertices As Vector3() = textInfo.meshInfo(materialReferenceIndex).vertices
				Dim vector As Vector3 = New Vector2((vertices(vertexIndex).x + vertices(vertexIndex + 2).x) / 2F, (vertices(vertexIndex).y + vertices(vertexIndex + 2).y) / 2F)
				vertices(vertexIndex) += -vector
				vertices(vertexIndex + 1) += -vector
				vertices(vertexIndex + 2) += -vector
				vertices(vertexIndex + 3) += -vector
				Dim num As Single = Me.initialAngles(i) + Me.jitterAngles(i)
				Dim matrix4x As Matrix4x4 = Matrix4x4.TRS(New Vector3(0F, 0F, 0F), Quaternion.Euler(0F, 0F, num), Vector3.one)
				vertices(vertexIndex) = matrix4x.MultiplyPoint3x4(vertices(vertexIndex))
				vertices(vertexIndex + 1) = matrix4x.MultiplyPoint3x4(vertices(vertexIndex + 1))
				vertices(vertexIndex + 2) = matrix4x.MultiplyPoint3x4(vertices(vertexIndex + 2))
				vertices(vertexIndex + 3) = matrix4x.MultiplyPoint3x4(vertices(vertexIndex + 3))
				vector += New Vector3(Global.UnityEngine.Random.Range(-Me.m_JitterOffsetAmplitude, Me.m_JitterOffsetAmplitude), Global.UnityEngine.Random.Range(-Me.m_JitterOffsetAmplitude, Me.m_JitterOffsetAmplitude), 0F)
				vertices(vertexIndex) += vector
				vertices(vertexIndex + 1) += vector
				vertices(vertexIndex + 2) += vector
				vertices(vertexIndex + 3) += vector
				Me.m_ShadowTextComponent.textInfo.meshInfo(materialReferenceIndex).vertices = vertices
			End If
		Next
		Me.m_TextComponent.UpdateVertexData()
		Me.m_ShadowTextComponent.UpdateVertexData()
	End Sub

	' Token: 0x04001AD7 RID: 6871
	<SerializeField()>
	Private m_TextComponent As TMP_Text

	' Token: 0x04001AD8 RID: 6872
	<SerializeField()>
	Private m_ShadowTextComponent As TMP_Text

	' Token: 0x04001AD9 RID: 6873
	Public m_AngleAmplitude As Single = 5F

	' Token: 0x04001ADA RID: 6874
	Public m_JitterAngleAmplitude As Single = 0.7F

	' Token: 0x04001ADB RID: 6875
	Public m_JitterOffsetAmplitude As Single = 0.1F

	' Token: 0x04001ADC RID: 6876
	Private initialAngles As Single()

	' Token: 0x04001ADD RID: 6877
	Private jitterAngles As Single()

	' Token: 0x04001ADE RID: 6878
	Private jitterDelay As Single = 0.1F

	' Token: 0x04001ADF RID: 6879
	Private currentJitterDelay As Single
End Class
