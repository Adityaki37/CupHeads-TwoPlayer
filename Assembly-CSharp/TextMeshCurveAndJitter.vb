Imports System
Imports System.Collections
Imports TMPro
Imports UnityEngine

' Token: 0x0200046C RID: 1132
Public Class TextMeshCurveAndJitter
	Inherits MonoBehaviour

	' Token: 0x170002B9 RID: 697
	' (set) Token: 0x06001156 RID: 4438 RVA: 0x000A47F2 File Offset: 0x000A2BF2
	Public WriteOnly Property AlphaValue As Byte
		Set(value As Byte)
			Me.applyAlpha = True
			Me.alphaValue = value
		End Set
	End Property

	' Token: 0x06001157 RID: 4439 RVA: 0x000A4802 File Offset: 0x000A2C02
	Private Sub Awake()
		Me.jitterDelay = 0.083333336F
		Me.AlphaValue = Byte.MaxValue
		Me.m_TextComponent = MyBase.gameObject.GetComponent(Of TMP_Text)()
	End Sub

	' Token: 0x06001158 RID: 4440 RVA: 0x000A482B File Offset: 0x000A2C2B
	Private Sub Start()
		MyBase.StartCoroutine(Me.WarpText())
	End Sub

	' Token: 0x06001159 RID: 4441 RVA: 0x000A483C File Offset: 0x000A2C3C
	Private Function CopyAnimationCurve(curve As AnimationCurve) As AnimationCurve
		Return New AnimationCurve() With { .keys = curve.keys }
	End Function

	' Token: 0x0600115A RID: 4442 RVA: 0x000A485C File Offset: 0x000A2C5C
	Private Iterator Function WarpText() As IEnumerator
		Me.VertexCurve.preWrapMode = WrapMode.Once
		Me.VertexCurve.postWrapMode = WrapMode.Once
		Me.m_TextComponent.havePropertiesChanged = True
		While True
			Me.currentJitterDelay -= CupheadTime.Delta
			If Me.currentJitterDelay <= 0F Then
				Me.currentJitterDelay = Me.jitterDelay
			End If
			Me.ApplyChanges(True)
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x0600115B RID: 4443 RVA: 0x000A4878 File Offset: 0x000A2C78
	Public Sub ApplyChanges(jitter As Boolean)
		Me.m_TextComponent.ForceMeshUpdate()
		Dim textInfo As TMP_TextInfo = Me.m_TextComponent.textInfo
		Dim characterCount As Integer = textInfo.characterCount
		If characterCount = 0 OrElse Me.m_TextComponent.text.Length = 0 Then
			Return
		End If
		Dim x As Single = Me.m_TextComponent.bounds.min.x
		Dim x2 As Single = Me.m_TextComponent.bounds.max.x
		For i As Integer = 0 To characterCount - 1
			If textInfo.characterInfo(i).isVisible Then
				Dim vertexIndex As Integer = CInt(textInfo.characterInfo(i).vertexIndex)
				Dim materialReferenceIndex As Integer = textInfo.characterInfo(i).materialReferenceIndex
				Dim vertices As Vector3() = textInfo.meshInfo(materialReferenceIndex).vertices
				Dim colors As Color32() = textInfo.meshInfo(materialReferenceIndex).colors32
				If jitter Then
					Me.ApplyCurveAndJitter(jitter, vertices, vertexIndex, i, textInfo, x, x2)
				End If
				If Me.applyAlpha Then
					Me.ApplyAlpha(colors, vertexIndex)
				End If
			End If
		Next
		jitter = False
		Me.m_TextComponent.UpdateVertexData()
	End Sub

	' Token: 0x0600115C RID: 4444 RVA: 0x000A49B8 File Offset: 0x000A2DB8
	Private Sub ApplyCurveAndJitter(jitter As Boolean, vertices As Vector3(), vertexIndex As Integer, i As Integer, textInfo As TMP_TextInfo, boundsMinX As Single, boundsMaxX As Single)
		Dim vector As Vector3 = New Vector2((vertices(vertexIndex).x + vertices(vertexIndex + 2).x) / 2F, textInfo.characterInfo(i).baseLine)
		Dim num As Single = (vector.x - Mathf.Min(boundsMinX, -229.3845F)) / (Mathf.Max(boundsMaxX, 226.2879F) - Mathf.Min(boundsMinX, -229.3845F))
		Dim num2 As Single = Me.VertexSpacing.Evaluate(num) * Me.SpacingScale
		vertices(vertexIndex).x = vertices(vertexIndex).x + num2
		Dim num3 As Integer = vertexIndex + 1
		vertices(num3).x = vertices(num3).x + num2
		Dim num4 As Integer = vertexIndex + 2
		vertices(num4).x = vertices(num4).x + num2
		Dim num5 As Integer = vertexIndex + 3
		vertices(num5).x = vertices(num5).x + num2
		vertices(vertexIndex) += -vector
		vertices(vertexIndex + 1) += -vector
		vertices(vertexIndex + 2) += -vector
		vertices(vertexIndex + 3) += -vector
		Dim num6 As Single = (vector.x - boundsMinX) / (boundsMaxX - boundsMinX)
		Dim num7 As Single = num6 + 0.0001F
		Dim num8 As Single = Me.VertexCurve.Evaluate(num6) * Me.CurveScale
		Dim num9 As Single = Me.VertexCurve.Evaluate(num7) * Me.CurveScale
		Dim vector2 As Vector3 = New Vector3(1F, 0F, 0F)
		Dim vector3 As Vector3 = New Vector3(num7 * (boundsMaxX - boundsMinX) + boundsMinX, num9) - New Vector3(vector.x, num8)
		Dim num10 As Single = Mathf.Acos(Vector3.Dot(vector2, vector3.normalized)) * 57.29578F
		Dim num11 As Single = If((Vector3.Cross(vector2, vector3).z <= 0F), (360F - num10), num10)
		Dim num12 As Single = 0F
		If jitter Then
			num12 = Global.UnityEngine.Random.Range(-Me.jitterAngleAmplitude, Me.jitterAngleAmplitude)
		End If
		Dim matrix4x As Matrix4x4 = Matrix4x4.TRS(New Vector3(0F, num8, 0F), Quaternion.Euler(0F, 0F, num11 + num12), Vector3.one)
		vertices(vertexIndex) = matrix4x.MultiplyPoint3x4(vertices(vertexIndex))
		vertices(vertexIndex + 1) = matrix4x.MultiplyPoint3x4(vertices(vertexIndex + 1))
		vertices(vertexIndex + 2) = matrix4x.MultiplyPoint3x4(vertices(vertexIndex + 2))
		vertices(vertexIndex + 3) = matrix4x.MultiplyPoint3x4(vertices(vertexIndex + 3))
		vertices(vertexIndex) += vector
		vertices(vertexIndex + 1) += vector
		vertices(vertexIndex + 2) += vector
		vertices(vertexIndex + 3) += vector
		Dim zero As Vector3 = Vector3.zero
		If jitter Then
			zero = New Vector3(Global.UnityEngine.Random.Range(-Me.jitterAmplitude, Me.jitterAmplitude), Global.UnityEngine.Random.Range(-Me.jitterAmplitude, Me.jitterAmplitude), 0F)
		End If
		vertices(vertexIndex) += zero
		vertices(vertexIndex + 1) += zero
		vertices(vertexIndex + 2) += zero
		vertices(vertexIndex + 3) += zero
	End Sub

	' Token: 0x0600115D RID: 4445 RVA: 0x000A4DC8 File Offset: 0x000A31C8
	Private Sub ApplyAlpha(vertices As Color32(), vertexIndex As Integer)
		vertices(vertexIndex).a = Me.alphaValue
		vertices(vertexIndex + 1).a = Me.alphaValue
		vertices(vertexIndex + 2).a = Me.alphaValue
		vertices(vertexIndex + 3).a = Me.alphaValue
	End Sub

	' Token: 0x04001AC8 RID: 6856
	Private Const MAX_BOUNDS_TEXT_COMPONENT As Single = 226.2879F

	' Token: 0x04001AC9 RID: 6857
	Private Const MIN_BOUNDS_TEXT_COMPONENT As Single = -229.3845F

	' Token: 0x04001ACA RID: 6858
	<SerializeField()>
	Private m_TextComponent As TMP_Text

	' Token: 0x04001ACB RID: 6859
	Public VertexCurve As AnimationCurve = New AnimationCurve(New Keyframe() { New Keyframe(0F, 0F), New Keyframe(0.25F, 2F), New Keyframe(0.5F, 0F), New Keyframe(0.75F, 2F), New Keyframe(1F, 0F) })

	' Token: 0x04001ACC RID: 6860
	Public VertexSpacing As AnimationCurve = New AnimationCurve(New Keyframe() { New Keyframe(0F, 1.5F), New Keyframe(0.5F, 0F), New Keyframe(1F, -1.5F) })

	' Token: 0x04001ACD RID: 6861
	Public AngleMultiplier As Single = 1F

	' Token: 0x04001ACE RID: 6862
	Public SpeedMultiplier As Single = 1F

	' Token: 0x04001ACF RID: 6863
	Public CurveScale As Single = 1F

	' Token: 0x04001AD0 RID: 6864
	Public SpacingScale As Single = 1F

	' Token: 0x04001AD1 RID: 6865
	Public jitterAmplitude As Single = 0.1F

	' Token: 0x04001AD2 RID: 6866
	Public jitterAngleAmplitude As Single = 0.1F

	' Token: 0x04001AD3 RID: 6867
	Private jitterDelay As Single = 0.1F

	' Token: 0x04001AD4 RID: 6868
	Private currentJitterDelay As Single

	' Token: 0x04001AD5 RID: 6869
	Private applyAlpha As Boolean

	' Token: 0x04001AD6 RID: 6870
	Private alphaValue As Byte
End Class
