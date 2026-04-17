Imports System
Imports UnityEngine

Namespace TMPro
	' Token: 0x02000C7C RID: 3196
	<ExecuteInEditMode()>
	<RequireComponent(GetType(MeshRenderer))>
	<RequireComponent(GetType(MeshFilter))>
	Public Class TMP_SubMesh
		Inherits MonoBehaviour

		' Token: 0x1700084C RID: 2124
		' (get) Token: 0x06005019 RID: 20505 RVA: 0x0029573A File Offset: 0x00293B3A
		' (set) Token: 0x0600501A RID: 20506 RVA: 0x00295742 File Offset: 0x00293B42
		Public Property fontAsset As TMP_FontAsset
			Get
				Return Me.m_fontAsset
			End Get
			Set(value As TMP_FontAsset)
				Me.m_fontAsset = value
			End Set
		End Property

		' Token: 0x1700084D RID: 2125
		' (get) Token: 0x0600501B RID: 20507 RVA: 0x0029574B File Offset: 0x00293B4B
		' (set) Token: 0x0600501C RID: 20508 RVA: 0x00295753 File Offset: 0x00293B53
		Public Property spriteAsset As TMP_SpriteAsset
			Get
				Return Me.m_spriteAsset
			End Get
			Set(value As TMP_SpriteAsset)
				Me.m_spriteAsset = value
			End Set
		End Property

		' Token: 0x1700084E RID: 2126
		' (get) Token: 0x0600501D RID: 20509 RVA: 0x0029575C File Offset: 0x00293B5C
		' (set) Token: 0x0600501E RID: 20510 RVA: 0x0029576A File Offset: 0x00293B6A
		Public Property material As Material
			Get
				Return Me.GetMaterial(Me.m_sharedMaterial)
			End Get
			Set(value As Material)
				If Me.m_sharedMaterial.GetInstanceID() = value.GetInstanceID() Then
					Return
				End If
				Me.m_sharedMaterial = value
				Me.m_padding = Me.GetPaddingForMaterial()
				Me.SetVerticesDirty()
				Me.SetMaterialDirty()
			End Set
		End Property

		' Token: 0x1700084F RID: 2127
		' (get) Token: 0x0600501F RID: 20511 RVA: 0x002957A2 File Offset: 0x00293BA2
		' (set) Token: 0x06005020 RID: 20512 RVA: 0x002957AA File Offset: 0x00293BAA
		Public Property sharedMaterial As Material
			Get
				Return Me.GetSharedMaterial()
			End Get
			Set(value As Material)
				Me.SetSharedMaterial(value)
			End Set
		End Property

		' Token: 0x17000850 RID: 2128
		' (get) Token: 0x06005021 RID: 20513 RVA: 0x002957B3 File Offset: 0x00293BB3
		' (set) Token: 0x06005022 RID: 20514 RVA: 0x002957BB File Offset: 0x00293BBB
		Public Property isDefaultMaterial As Boolean
			Get
				Return Me.m_isDefaultMaterial
			End Get
			Set(value As Boolean)
				Me.m_isDefaultMaterial = value
			End Set
		End Property

		' Token: 0x17000851 RID: 2129
		' (get) Token: 0x06005023 RID: 20515 RVA: 0x002957C4 File Offset: 0x00293BC4
		' (set) Token: 0x06005024 RID: 20516 RVA: 0x002957CC File Offset: 0x00293BCC
		Public Property padding As Single
			Get
				Return Me.m_padding
			End Get
			Set(value As Single)
				Me.m_padding = value
			End Set
		End Property

		' Token: 0x17000852 RID: 2130
		' (get) Token: 0x06005025 RID: 20517 RVA: 0x002957D5 File Offset: 0x00293BD5
		Public ReadOnly Property renderer As Renderer
			Get
				If Me.m_renderer Is Nothing Then
					Me.m_renderer = MyBase.GetComponent(Of Renderer)()
				End If
				Return Me.m_renderer
			End Get
		End Property

		' Token: 0x17000853 RID: 2131
		' (get) Token: 0x06005026 RID: 20518 RVA: 0x002957FA File Offset: 0x00293BFA
		Public ReadOnly Property meshFilter As MeshFilter
			Get
				If Me.m_meshFilter Is Nothing Then
					Me.m_meshFilter = MyBase.GetComponent(Of MeshFilter)()
				End If
				Return Me.m_meshFilter
			End Get
		End Property

		' Token: 0x17000854 RID: 2132
		' (get) Token: 0x06005027 RID: 20519 RVA: 0x00295820 File Offset: 0x00293C20
		' (set) Token: 0x06005028 RID: 20520 RVA: 0x0029586D File Offset: 0x00293C6D
		Public Property mesh As Mesh
			Get
				If Me.m_mesh Is Nothing Then
					Me.m_mesh = New Mesh()
					Me.m_mesh.hideFlags = HideFlags.HideAndDontSave
					Me.meshFilter.mesh = Me.m_mesh
				End If
				Return Me.m_mesh
			End Get
			Set(value As Mesh)
				Me.m_mesh = value
			End Set
		End Property

		' Token: 0x06005029 RID: 20521 RVA: 0x00295878 File Offset: 0x00293C78
		Private Sub OnEnable()
			If Not Me.m_isRegisteredForEvents Then
				Me.m_isRegisteredForEvents = True
			End If
			If Me.m_sharedMaterial IsNot Nothing Then
				Me.m_sharedMaterial.SetVector(ShaderUtilities.ID_ClipRect, New Vector4(-10000F, -10000F, 10000F, 10000F))
			End If
		End Sub

		' Token: 0x0600502A RID: 20522 RVA: 0x002958D1 File Offset: 0x00293CD1
		Private Sub OnDestroy()
			If Me.m_mesh IsNot Nothing Then
				Global.UnityEngine.[Object].DestroyImmediate(Me.m_mesh)
			End If
			Me.m_isRegisteredForEvents = False
		End Sub

		' Token: 0x0600502B RID: 20523 RVA: 0x002958F8 File Offset: 0x00293CF8
		Public Shared Function AddSubTextObject(textComponent As TextMeshPro, materialReference As MaterialReference) As TMP_SubMesh
			Dim gameObject As GameObject = New GameObject("TMP SubMesh [" + materialReference.material.name + "]")
			Dim tmp_SubMesh As TMP_SubMesh = gameObject.AddComponent(Of TMP_SubMesh)()
			gameObject.transform.SetParent(textComponent.transform, False)
			gameObject.transform.localPosition = Vector3.zero
			gameObject.transform.localRotation = Quaternion.identity
			gameObject.transform.localScale = Vector3.one
			gameObject.layer = textComponent.gameObject.layer
			tmp_SubMesh.m_meshFilter = gameObject.GetComponent(Of MeshFilter)()
			tmp_SubMesh.m_TextComponent = textComponent
			tmp_SubMesh.m_fontAsset = materialReference.fontAsset
			tmp_SubMesh.m_spriteAsset = materialReference.spriteAsset
			tmp_SubMesh.m_isDefaultMaterial = materialReference.isDefaultMaterial
			tmp_SubMesh.SetSharedMaterial(materialReference.material)
			tmp_SubMesh.renderer.sortingLayerID = textComponent.renderer.sortingLayerID
			tmp_SubMesh.renderer.sortingOrder = textComponent.renderer.sortingOrder
			Return tmp_SubMesh
		End Function

		' Token: 0x0600502C RID: 20524 RVA: 0x002959F4 File Offset: 0x00293DF4
		Public Sub DestroySelf()
			Global.UnityEngine.[Object].Destroy(MyBase.gameObject, 1F)
		End Sub

		' Token: 0x0600502D RID: 20525 RVA: 0x00295A08 File Offset: 0x00293E08
		Private Function GetMaterial(mat As Material) As Material
			If Me.m_renderer Is Nothing Then
				Me.m_renderer = MyBase.GetComponent(Of Renderer)()
			End If
			If Me.m_material Is Nothing OrElse Me.m_material.GetInstanceID() <> mat.GetInstanceID() Then
				Me.m_material = Me.CreateMaterialInstance(mat)
			End If
			Me.m_sharedMaterial = Me.m_material
			Me.m_padding = Me.GetPaddingForMaterial()
			Me.SetVerticesDirty()
			Me.SetMaterialDirty()
			Return Me.m_sharedMaterial
		End Function

		' Token: 0x0600502E RID: 20526 RVA: 0x00295A90 File Offset: 0x00293E90
		Private Function CreateMaterialInstance(source As Material) As Material
			Dim material As Material = New Material(source)
			material.shaderKeywords = source.shaderKeywords
			Dim material2 As Material = material
			material2.name += " (Instance)"
			Return material
		End Function

		' Token: 0x0600502F RID: 20527 RVA: 0x00295AC7 File Offset: 0x00293EC7
		Private Function GetSharedMaterial() As Material
			If Me.m_renderer Is Nothing Then
				Me.m_renderer = MyBase.GetComponent(Of Renderer)()
			End If
			Return Me.m_renderer.sharedMaterial
		End Function

		' Token: 0x06005030 RID: 20528 RVA: 0x00295AF1 File Offset: 0x00293EF1
		Private Sub SetSharedMaterial(mat As Material)
			Me.m_sharedMaterial = mat
			Me.m_padding = Me.GetPaddingForMaterial()
			Me.SetMaterialDirty()
		End Sub

		' Token: 0x06005031 RID: 20529 RVA: 0x00295B0C File Offset: 0x00293F0C
		Public Function GetPaddingForMaterial() As Single
			Return ShaderUtilities.GetPadding(Me.m_sharedMaterial, Me.m_TextComponent.extraPadding, Me.m_TextComponent.isUsingBold)
		End Function

		' Token: 0x06005032 RID: 20530 RVA: 0x00295B3C File Offset: 0x00293F3C
		Public Sub UpdateMeshPadding(isExtraPadding As Boolean, isUsingBold As Boolean)
			Me.m_padding = ShaderUtilities.GetPadding(Me.m_sharedMaterial, isExtraPadding, isUsingBold)
		End Sub

		' Token: 0x06005033 RID: 20531 RVA: 0x00295B51 File Offset: 0x00293F51
		Public Sub SetVerticesDirty()
			If Not MyBase.enabled Then
				Return
			End If
			If Me.m_TextComponent IsNot Nothing Then
				Me.m_TextComponent.havePropertiesChanged = True
				Me.m_TextComponent.SetVerticesDirty()
			End If
		End Sub

		' Token: 0x06005034 RID: 20532 RVA: 0x00295B87 File Offset: 0x00293F87
		Public Sub SetMaterialDirty()
			Me.UpdateMaterial()
		End Sub

		' Token: 0x06005035 RID: 20533 RVA: 0x00295B8F File Offset: 0x00293F8F
		Protected Sub UpdateMaterial()
			If Me.m_renderer Is Nothing Then
				Me.m_renderer = Me.renderer
			End If
			Me.m_renderer.sharedMaterial = Me.m_sharedMaterial
		End Sub

		' Token: 0x040052BD RID: 21181
		<SerializeField()>
		Private m_fontAsset As TMP_FontAsset

		' Token: 0x040052BE RID: 21182
		<SerializeField()>
		Private m_spriteAsset As TMP_SpriteAsset

		' Token: 0x040052BF RID: 21183
		<SerializeField()>
		Private m_material As Material

		' Token: 0x040052C0 RID: 21184
		<SerializeField()>
		Private m_sharedMaterial As Material

		' Token: 0x040052C1 RID: 21185
		<SerializeField()>
		Private m_isDefaultMaterial As Boolean

		' Token: 0x040052C2 RID: 21186
		<SerializeField()>
		Private m_padding As Single

		' Token: 0x040052C3 RID: 21187
		<SerializeField()>
		Private m_renderer As Renderer

		' Token: 0x040052C4 RID: 21188
		<SerializeField()>
		Private m_meshFilter As MeshFilter

		' Token: 0x040052C5 RID: 21189
		Private m_mesh As Mesh

		' Token: 0x040052C6 RID: 21190
		<SerializeField()>
		Private m_TextComponent As TextMeshPro

		' Token: 0x040052C7 RID: 21191
		<NonSerialized()>
		Private m_isRegisteredForEvents As Boolean
	End Class
End Namespace
