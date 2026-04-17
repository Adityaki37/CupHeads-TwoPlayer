Imports System

Namespace UnityEngine.PostProcessing
	' Token: 0x02000BFF RID: 3071
	<Serializable()>
	Public NotInheritable Class ColorGradingCurve
		' Token: 0x06004951 RID: 18769 RVA: 0x00265338 File Offset: 0x00263738
		Public Sub New(curve As AnimationCurve, zeroValue As Single, [loop] As Boolean, bounds As Vector2)
			Me.curve = curve
			Me.m_ZeroValue = zeroValue
			Me.m_Loop = [loop]
			Me.m_Range = bounds.magnitude
		End Sub

		' Token: 0x06004952 RID: 18770 RVA: 0x00265364 File Offset: 0x00263764
		Public Sub Cache()
			If Not Me.m_Loop Then
				Return
			End If
			Dim length As Integer = Me.curve.length
			If length < 2 Then
				Return
			End If
			If Me.m_InternalLoopingCurve Is Nothing Then
				Me.m_InternalLoopingCurve = New AnimationCurve()
			End If
			Dim keyframe As Keyframe = Me.curve(length - 1)
			keyframe.time -= Me.m_Range
			Dim keyframe2 As Keyframe = Me.curve(0)
			keyframe2.time += Me.m_Range
			Me.m_InternalLoopingCurve.keys = Me.curve.keys
			Me.m_InternalLoopingCurve.AddKey(keyframe)
			Me.m_InternalLoopingCurve.AddKey(keyframe2)
		End Sub

		' Token: 0x06004953 RID: 18771 RVA: 0x0026541C File Offset: 0x0026381C
		Public Function Evaluate(t As Single) As Single
			If Me.curve.length = 0 Then
				Return Me.m_ZeroValue
			End If
			If Not Me.m_Loop OrElse Me.curve.length = 1 Then
				Return Me.curve.Evaluate(t)
			End If
			Return Me.m_InternalLoopingCurve.Evaluate(t)
		End Function

		' Token: 0x04004F74 RID: 20340
		Public curve As AnimationCurve

		' Token: 0x04004F75 RID: 20341
		<SerializeField()>
		Private m_Loop As Boolean

		' Token: 0x04004F76 RID: 20342
		<SerializeField()>
		Private m_ZeroValue As Single

		' Token: 0x04004F77 RID: 20343
		<SerializeField()>
		Private m_Range As Single

		' Token: 0x04004F78 RID: 20344
		Private m_InternalLoopingCurve As AnimationCurve
	End Class
End Namespace
