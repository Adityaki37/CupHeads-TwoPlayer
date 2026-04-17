Imports System
Imports UnityEngine

' Token: 0x02000955 RID: 2389
Public Class MapNPCMusic
	Inherits MonoBehaviour

	' Token: 0x060037D1 RID: 14289 RVA: 0x00200417 File Offset: 0x001FE817
	Private Sub Start()
		AddHandler Dialoguer.events.onMessageEvent, AddressOf Me.OnDialoguerMessageEvent
	End Sub

	' Token: 0x060037D2 RID: 14290 RVA: 0x0020042F File Offset: 0x001FE82F
	Private Sub OnDestroy()
		RemoveHandler Dialoguer.events.onMessageEvent, AddressOf Me.OnDialoguerMessageEvent
	End Sub

	' Token: 0x060037D3 RID: 14291 RVA: 0x00200448 File Offset: 0x001FE848
	Private Sub OnDialoguerMessageEvent(message As String, metadata As String)
		If message = "MinimalistMusic" AndAlso Me.musicType = MapNPCMusic.MusicType.Minimalist Then
			PlayerData.Data.pianoAudioEnabled = True
			PlayerData.SaveCurrentFile()
			Map.Current.OnNPCChangeMusic()
		ElseIf message = "RegularMusic" AndAlso Me.musicType = MapNPCMusic.MusicType.Regular Then
			PlayerData.Data.pianoAudioEnabled = False
			PlayerData.SaveCurrentFile()
			Map.Current.OnNPCChangeMusic()
		End If
	End Sub

	' Token: 0x04003FCC RID: 16332
	<SerializeField()>
	Private musicType As MapNPCMusic.MusicType

	' Token: 0x02000956 RID: 2390
	Public Enum MusicType
		' Token: 0x04003FCE RID: 16334
		Regular
		' Token: 0x04003FCF RID: 16335
		Minimalist
	End Enum
End Class
