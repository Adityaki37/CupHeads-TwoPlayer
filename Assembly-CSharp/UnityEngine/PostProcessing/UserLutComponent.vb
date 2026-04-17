Imports System

Namespace UnityEngine.PostProcessing
	' Token: 0x02000BB8 RID: 3000
	Public NotInheritable Class UserLutComponent
		Inherits PostProcessingComponentRenderTexture(Of UserLutModel)

		' Token: 0x17000659 RID: 1625
		' (get) Token: 0x060048B4 RID: 18612 RVA: 0x002630C4 File Offset: 0x002614C4
		Public Overrides ReadOnly Property active As Boolean
			Get
				Dim settings As UserLutModel.Settings = MyBase.model.settings
				Return MyBase.model.enabled AndAlso settings.lut IsNot Nothing AndAlso settings.contribution > 0F AndAlso settings.lut.height = CInt(Mathf.Sqrt(CSng(settings.lut.width))) AndAlso Not Me.context.interrupted
			End Get
		End Property

		' Token: 0x060048B5 RID: 18613 RVA: 0x00263148 File Offset: 0x00261548
		Public Overrides Sub Prepare(uberMaterial As Material)
			Dim settings As UserLutModel.Settings = MyBase.model.settings
			uberMaterial.EnableKeyword("USER_LUT")
			uberMaterial.SetTexture(UserLutComponent.Uniforms._UserLut, settings.lut)
			uberMaterial.SetVector(UserLutComponent.Uniforms._UserLut_Params, New Vector4(1F / CSng(settings.lut.width), 1F / CSng(settings.lut.height), CSng(settings.lut.height) - 1F, settings.contribution))
		End Sub

		' Token: 0x060048B6 RID: 18614 RVA: 0x002631D0 File Offset: 0x002615D0
		Public Sub OnGUI()
			Dim settings As UserLutModel.Settings = MyBase.model.settings
			Dim rect As Rect = New Rect(Me.context.viewport.x * CSng(Screen.width) + 8F, 8F, CSng(settings.lut.width), CSng(settings.lut.height))
			GUI.DrawTexture(rect, settings.lut)
		End Sub

		' Token: 0x02000BB9 RID: 3001
		Private NotInheritable Class Uniforms
			' Token: 0x04004E6B RID: 20075
			Friend Shared _UserLut As Integer = Shader.PropertyToID("_UserLut")

			' Token: 0x04004E6C RID: 20076
			Friend Shared _UserLut_Params As Integer = Shader.PropertyToID("_UserLut_Params")
		End Class
	End Class
End Namespace
