Imports System
Imports System.Collections.Generic
Imports UnityEngine

' Token: 0x020003B0 RID: 944
Public Class RuntimeSceneAssetDatabase
	Inherits ScriptableObject

	' Token: 0x1700020C RID: 524
	' (get) Token: 0x06000BA7 RID: 2983 RVA: 0x0008465B File Offset: 0x00082A5B
	Public ReadOnly Property persistentAssets As HashSet(Of String)
		Get
			If Me._persistentAssets Is Nothing Then
				Me._persistentAssets = New HashSet(Of String)(Me.INTERNAL_persistentAssetNames)
			End If
			Return Me._persistentAssets
		End Get
	End Property

	' Token: 0x1700020D RID: 525
	' (get) Token: 0x06000BA8 RID: 2984 RVA: 0x0008467F File Offset: 0x00082A7F
	Public ReadOnly Property persistentAssetsDLC As HashSet(Of String)
		Get
			If Me._persistentAssetsDLC Is Nothing Then
				Me._persistentAssetsDLC = New HashSet(Of String)(Me.INTERNAL_persistentAssetNamesDLC)
			End If
			Return Me._persistentAssetsDLC
		End Get
	End Property

	' Token: 0x1700020E RID: 526
	' (get) Token: 0x06000BA9 RID: 2985 RVA: 0x000846A4 File Offset: 0x00082AA4
	Public ReadOnly Property sceneAssetMappings As Dictionary(Of String, String())
		Get
			If Me._sceneAssetMappings Is Nothing Then
				Me._sceneAssetMappings = New Dictionary(Of String, String())()
				For Each sceneAssetMapping As RuntimeSceneAssetDatabase.SceneAssetMapping In Me.INTERNAL_sceneAssetMappings
					Me._sceneAssetMappings.Add(sceneAssetMapping.sceneName, sceneAssetMapping.assetNames)
				Next
			End If
			Return Me._sceneAssetMappings
		End Get
	End Property

	' Token: 0x04001562 RID: 5474
	Public INTERNAL_persistentAssetNames As String()

	' Token: 0x04001563 RID: 5475
	Public INTERNAL_persistentAssetNamesDLC As String()

	' Token: 0x04001564 RID: 5476
	Public INTERNAL_sceneAssetMappings As RuntimeSceneAssetDatabase.SceneAssetMapping()

	' Token: 0x04001565 RID: 5477
	Private _persistentAssets As HashSet(Of String)

	' Token: 0x04001566 RID: 5478
	Private _persistentAssetsDLC As HashSet(Of String)

	' Token: 0x04001567 RID: 5479
	Private _sceneAssetMappings As Dictionary(Of String, String())

	' Token: 0x020003B1 RID: 945
	<Serializable()>
	Public Class SceneAssetMapping
		' Token: 0x04001568 RID: 5480
		Public sceneName As String

		' Token: 0x04001569 RID: 5481
		Public assetNames As String()
	End Class
End Class
