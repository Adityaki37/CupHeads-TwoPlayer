Imports System
Imports UnityEngine

' Token: 0x020003AB RID: 939
Public Class DeferredAudioSource
	Inherits MonoBehaviour

	' Token: 0x06000B95 RID: 2965 RVA: 0x00084004 File Offset: 0x00082404
	Public Sub Initialize()
		Dim component As AudioSource = MyBase.GetComponent(Of AudioSource)()
		If Not DLCManager.DLCEnabled() AndAlso AssetLoader(Of AudioClip).IsDLCAsset(Me.audioClipName) Then
			component.clip = Nothing
		Else
			component.clip = AssetLoader(Of AudioClip).GetCachedAsset(Me.audioClipName)
		End If
		If Me.playOnInitialize Then
			component.Play()
		End If
	End Sub

	' Token: 0x04001525 RID: 5413
	<SerializeField()>
	Private audioClipName As String

	' Token: 0x04001526 RID: 5414
	<SerializeField()>
	Private playOnInitialize As Boolean
End Class
