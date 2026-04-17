Imports System
Imports UnityEngine

Namespace UnityStandardAssets.ImageEffects
	' Token: 0x02000CDC RID: 3292
	<RequireComponent(GetType(Camera))>
	<AddComponentMenu("")>
	Public Class ImageEffectBase
		Inherits MonoBehaviour

		' Token: 0x06005222 RID: 21026 RVA: 0x0029EE05 File Offset: 0x0029D205
		Protected Overridable Sub Start()
			If Not SystemInfo.supportsImageEffects Then
				MyBase.enabled = False
				Return
			End If
			If Not Me.shader OrElse Not Me.shader.isSupported Then
				MyBase.enabled = False
			End If
		End Sub

		' Token: 0x170008A8 RID: 2216
		' (get) Token: 0x06005223 RID: 21027 RVA: 0x0029EE40 File Offset: 0x0029D240
		Protected ReadOnly Property material As Material
			Get
				If Me.m_Material Is Nothing Then
					Me.m_Material = New Material(Me.shader)
					Me.m_Material.hideFlags = HideFlags.HideAndDontSave
				End If
				Return Me.m_Material
			End Get
		End Property

		' Token: 0x06005224 RID: 21028 RVA: 0x0029EE77 File Offset: 0x0029D277
		Protected Overridable Sub OnDisable()
			If Me.m_Material Then
				Global.UnityEngine.[Object].DestroyImmediate(Me.m_Material)
			End If
		End Sub

		' Token: 0x04005684 RID: 22148
		Public shader As Shader

		' Token: 0x04005685 RID: 22149
		Private m_Material As Material
	End Class
End Namespace
