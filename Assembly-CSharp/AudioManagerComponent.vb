Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports UnityEngine
Imports UnityEngine.Audio

' Token: 0x020003C9 RID: 969
Public Class AudioManagerComponent
	Inherits AbstractMonoBehaviour

	' Token: 0x06000C4E RID: 3150 RVA: 0x00085DF8 File Offset: 0x000841F8
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.SetChannels()
		Me.dict = New Dictionary(Of String, AudioManagerComponent.SoundGroup)()
		For Each soundGroup As AudioManagerComponent.SoundGroup In Me.sounds
			soundGroup.Init(True)
			Me.dict(soundGroup.key.ToLowerIfNecessary()) = soundGroup
		Next
		For Each soundGroup2 As AudioManagerComponent.SoundGroup In Me.bgmPlaylist
			soundGroup2.Init(True)
			Me.dict(soundGroup2.key.ToLowerIfNecessary()) = soundGroup2
		Next
		For Each source As AudioManagerComponent.SoundGroup.Source In Me.bgmAlternates
			source.Init(True)
		Next
		For Each source2 As AudioManagerComponent.SoundGroup.Source In Me.bgmSources
			source2.Init(True)
		Next
		Me.AddEvents()
	End Sub

	' Token: 0x06000C4F RID: 3151 RVA: 0x00085F88 File Offset: 0x00084388
	Private Sub OnDestroy()
		Me.RemoveEvents()
	End Sub

	' Token: 0x06000C50 RID: 3152 RVA: 0x00085F90 File Offset: 0x00084390
	Private Sub OnValidate()
		For Each soundGroup As AudioManagerComponent.SoundGroup In Me.sounds
			If String.IsNullOrEmpty(soundGroup.key) Then
				soundGroup.key = soundGroup.trigger.ToString()
			End If
			soundGroup.key = soundGroup.key.ToLower()
		Next
		For Each soundGroup2 As AudioManagerComponent.SoundGroup In Me.bgmPlaylist
			If String.IsNullOrEmpty(soundGroup2.key) Then
				soundGroup2.key = soundGroup2.trigger.ToString()
			End If
			soundGroup2.key = soundGroup2.key.ToLower()
		Next
	End Sub

	' Token: 0x06000C51 RID: 3153 RVA: 0x0008609C File Offset: 0x0008449C
	Private Sub AddEvents()
		AddHandler AudioManager.OnPlayBGMEvent, AddressOf Me.StartBGM
		AddHandler AudioManager.OnPlayBGMPlaylistEvent, AddressOf Me.StartBGMPlaylist
		AddHandler AudioManager.OnSnapshotEvent, AddressOf Me.SnapshotTransition
		AudioManager.OnCheckEvent.Add(AddressOf Me.OnIsPlaying)
		AddHandler AudioManager.OnPlayEvent, AddressOf Me.OnPlay
		AddHandler AudioManager.OnPlayLoopEvent, AddressOf Me.OnPlayLoop
		AddHandler AudioManager.OnStopEvent, AddressOf Me.OnStop
		AddHandler AudioManager.OnPauseEvent, AddressOf Me.OnPause
		AddHandler AudioManager.OnUnpauseEvent, AddressOf Me.OnUnpause
		AddHandler AudioManager.OnFollowObject, AddressOf Me.OnFollowOject
		AddHandler AudioManager.OnPanEvent, AddressOf Me.OnPan
		AddHandler AudioManager.OnStopAllEvent, AddressOf Me.OnStopAll
		AddHandler AudioManager.OnStopBGMEvent, AddressOf Me.OnStopBGM
		AddHandler AudioManager.OnPauseAllSFXEvent, AddressOf Me.OnPauseAllSFX
		AddHandler AudioManager.OnUnpauseAllSFXEvent, AddressOf Me.OnUnpauseAllSFX
		AddHandler AudioManager.OnBGMSlowdown, AddressOf Me.OnBGMSlowdown
		AddHandler AudioManager.OnSFXSlowDown, AddressOf Me.OnSFXSlowDown
		AddHandler AudioManager.OnSFXFadeVolume, AddressOf Me.OnSFXVolume
		AddHandler AudioManager.OnSFXFadeVolumeLinear, AddressOf Me.OnSFXVolumeLinear
		AddHandler AudioManager.OnBGMPitchWarble, AddressOf Me.OnBGMWarblePitch
		AddHandler AudioManager.OnAttenuation, AddressOf Me.OnAttenuation
		AddHandler AudioManager.OnPlayManualBGM, AddressOf Me.PlayManualBGMTrack
		AddHandler AudioManager.OnStopManualBGMTrackEvent, AddressOf Me.StopManualBGMTrack
		AddHandler AudioManager.OnBGMFadeVolume, AddressOf Me.OnBGMVolumeFade
		AddHandler AudioManager.OnStartBGMAlternate, AddressOf Me.StartBGMAlternate
		If Me.autoplayBGM Then
			AddHandler SceneLoader.OnLoaderCompleteEvent, AddressOf Me.StartBGM
		End If
		If Me.autoplayBGMPlaylist Then
			AddHandler SceneLoader.OnLoaderCompleteEvent, AddressOf Me.StartBGMPlaylist
		End If
	End Sub

	' Token: 0x06000C52 RID: 3154 RVA: 0x00086290 File Offset: 0x00084690
	Private Sub RemoveEvents()
		RemoveHandler AudioManager.OnPlayBGMEvent, AddressOf Me.StartBGM
		RemoveHandler AudioManager.OnPlayBGMPlaylistEvent, AddressOf Me.StartBGMPlaylist
		RemoveHandler AudioManager.OnSnapshotEvent, AddressOf Me.SnapshotTransition
		AudioManager.OnCheckEvent.Remove(AddressOf Me.OnIsPlaying)
		RemoveHandler AudioManager.OnPlayEvent, AddressOf Me.OnPlay
		RemoveHandler AudioManager.OnPlayLoopEvent, AddressOf Me.OnPlayLoop
		RemoveHandler AudioManager.OnStopEvent, AddressOf Me.OnStop
		RemoveHandler AudioManager.OnPauseEvent, AddressOf Me.OnPause
		RemoveHandler AudioManager.OnUnpauseEvent, AddressOf Me.OnUnpause
		RemoveHandler AudioManager.OnFollowObject, AddressOf Me.OnFollowOject
		RemoveHandler AudioManager.OnPanEvent, AddressOf Me.OnPan
		RemoveHandler AudioManager.OnStopAllEvent, AddressOf Me.OnStopAll
		RemoveHandler AudioManager.OnStopBGMEvent, AddressOf Me.OnStopBGM
		RemoveHandler AudioManager.OnPauseAllSFXEvent, AddressOf Me.OnPauseAllSFX
		RemoveHandler AudioManager.OnUnpauseAllSFXEvent, AddressOf Me.OnUnpauseAllSFX
		RemoveHandler AudioManager.OnBGMSlowdown, AddressOf Me.OnBGMSlowdown
		RemoveHandler AudioManager.OnSFXSlowDown, AddressOf Me.OnSFXSlowDown
		RemoveHandler AudioManager.OnSFXFadeVolume, AddressOf Me.OnSFXVolume
		RemoveHandler AudioManager.OnSFXFadeVolumeLinear, AddressOf Me.OnSFXVolumeLinear
		RemoveHandler AudioManager.OnBGMPitchWarble, AddressOf Me.OnBGMWarblePitch
		RemoveHandler AudioManager.OnAttenuation, AddressOf Me.OnAttenuation
		RemoveHandler AudioManager.OnPlayManualBGM, AddressOf Me.PlayManualBGMTrack
		RemoveHandler AudioManager.OnStopManualBGMTrackEvent, AddressOf Me.StopManualBGMTrack
		RemoveHandler AudioManager.OnBGMFadeVolume, AddressOf Me.OnBGMVolumeFade
		RemoveHandler AudioManager.OnStartBGMAlternate, AddressOf Me.StartBGMAlternate
		If Me.autoplayBGM Then
			RemoveHandler SceneLoader.OnLoaderCompleteEvent, AddressOf Me.StartBGM
		End If
		If Me.autoplayBGMPlaylist Then
			RemoveHandler SceneLoader.OnLoaderCompleteEvent, AddressOf Me.StartBGMPlaylist
		End If
	End Sub

	' Token: 0x06000C53 RID: 3155 RVA: 0x00086484 File Offset: 0x00084884
	Private Sub Update()
		For i As Integer = 0 To Me.sounds.Count - 1
			Dim soundGroup As AudioManagerComponent.SoundGroup = Me.sounds(i)
			If soundGroup.emissionTransform IsNot Nothing Then
				soundGroup.FollowObject(soundGroup.emissionTransform.position)
			End If
		Next
	End Sub

	' Token: 0x06000C54 RID: 3156 RVA: 0x000864DC File Offset: 0x000848DC
	Private Sub StartBGM()
		Me.StopBGM()
		For Each source As AudioManagerComponent.SoundGroup.Source In Me.bgmSources
			If source.noLoop Then
				source.Play()
			Else
				source.PlayLooped()
			End If
		Next
	End Sub

	' Token: 0x06000C55 RID: 3157 RVA: 0x00086554 File Offset: 0x00084954
	Private Sub StartBGMAlternate(index As Integer)
		Me.StopBGM()
		If Me.bgmAlternates.Count > index AndAlso Me.bgmAlternates(index) IsNot Nothing Then
			If Me.bgmAlternates(index).noLoop Then
				Me.bgmAlternates(index).Play()
			Else
				Me.bgmAlternates(index).PlayLooped()
			End If
		End If
	End Sub

	' Token: 0x06000C56 RID: 3158 RVA: 0x000865C8 File Offset: 0x000849C8
	Private Sub StopBGM()
		For Each source As AudioManagerComponent.SoundGroup.Source In Me.bgmSources
			source.[Stop]()
		Next
		For Each source2 As AudioManagerComponent.SoundGroup.Source In Me.bgmAlternates
			source2.[Stop]()
		Next
	End Sub

	' Token: 0x06000C57 RID: 3159 RVA: 0x00086670 File Offset: 0x00084A70
	Private Sub OnLevelStart()
		Me.StartBGM()
	End Sub

	' Token: 0x06000C58 RID: 3160 RVA: 0x00086678 File Offset: 0x00084A78
	Private Sub OnStopBGM()
		Me.StopBGM()
	End Sub

	' Token: 0x06000C59 RID: 3161 RVA: 0x00086680 File Offset: 0x00084A80
	Private Sub OnBGMSlowdown([end] As Single, time As Single)
		For Each source As AudioManagerComponent.SoundGroup.Source In Me.bgmSources
			MyBase.StartCoroutine(source.change_pitch_cr([end], time))
		Next
		For Each source2 As AudioManagerComponent.SoundGroup.Source In Me.bgmAlternates
			MyBase.StartCoroutine(source2.change_pitch_cr([end], time))
		Next
		For i As Integer = 0 To Me.bgmPlaylist.Count - 1
			If Me.bgmPlaylist(i).CheckIfPlaying() Then
				MyBase.StartCoroutine(Me.bgmPlaylist(i).change_pitch_sfx([end], time))
			End If
		Next
	End Sub

	' Token: 0x06000C5A RID: 3162 RVA: 0x0008678C File Offset: 0x00084B8C
	Private Sub OnBGMVolumeFade([end] As Single, time As Single, onFadeout As Boolean)
		For Each source As AudioManagerComponent.SoundGroup.Source In Me.bgmSources
			If(source.isPlaying() AndAlso onFadeout) OrElse (Not onFadeout AndAlso source.isFadedOut) Then
				MyBase.StartCoroutine(source.change_volume_cr([end], time, onFadeout))
			End If
		Next
		For Each source2 As AudioManagerComponent.SoundGroup.Source In Me.bgmAlternates
			If(source2.isPlaying() AndAlso onFadeout) OrElse (Not onFadeout AndAlso source2.isFadedOut) Then
				MyBase.StartCoroutine(source2.change_volume_cr([end], time, onFadeout))
			End If
		Next
		For i As Integer = 0 To Me.bgmPlaylist.Count - 1
			If(Me.bgmPlaylist(i).CheckIfPlaying() AndAlso onFadeout) OrElse (Not onFadeout AndAlso Me.bgmPlaylist(i).isFadedOut) Then
				MyBase.StartCoroutine(Me.bgmPlaylist(i).change_volume_cr([end], time, onFadeout))
			End If
		Next
	End Sub

	' Token: 0x06000C5B RID: 3163 RVA: 0x00086900 File Offset: 0x00084D00
	Private Sub OnBGMWarblePitch(warbles As Integer, minValue As Single(), maxValue As Single(), warbleTime As Single(), playTime As Single())
		For Each source As AudioManagerComponent.SoundGroup.Source In Me.bgmSources
			MyBase.StartCoroutine(source.warble_pitch_cr(warbles, minValue, maxValue, warbleTime, playTime))
		Next
		For i As Integer = 0 To Me.bgmPlaylist.Count - 1
			If Me.bgmPlaylist(i).CheckIfPlaying() Then
				MyBase.StartCoroutine(Me.bgmPlaylist(i).warble_pitch_cr(warbles, minValue, maxValue, warbleTime, playTime))
			End If
		Next
	End Sub

	' Token: 0x06000C5C RID: 3164 RVA: 0x000869BC File Offset: 0x00084DBC
	Public Sub PlayManualBGMTrack(loopPlayListAfter As Boolean)
		For i As Integer = 0 To Me.bgmPlaylist.Count - 1
			If Me.bgmPlaylist(i).activatedManually Then
				If loopPlayListAfter Then
					Me.bgmPlaylist(i).Play()
					MyBase.StartCoroutine(Me.handle_cr(Me.bgmPlaylist(i).ClipLength()))
				Else
					Me.bgmPlaylist(i).PlayLoop()
				End If
			End If
		Next
	End Sub

	' Token: 0x06000C5D RID: 3165 RVA: 0x00086A48 File Offset: 0x00084E48
	Private Iterator Function handle_cr(clipLength As Single) As IEnumerator
		Yield New WaitForSeconds(clipLength)
		Me.StartBGMPlaylist()
		Yield Nothing
		Return
	End Function

	' Token: 0x06000C5E RID: 3166 RVA: 0x00086A6C File Offset: 0x00084E6C
	Public Sub StopManualBGMTrack()
		For i As Integer = 0 To Me.bgmPlaylist.Count - 1
			If Me.bgmPlaylist(i).activatedManually Then
				Me.bgmPlaylist(i).[Stop]()
			End If
		Next
	End Sub

	' Token: 0x06000C5F RID: 3167 RVA: 0x00086ABC File Offset: 0x00084EBC
	Private Sub StartBGMPlaylist()
		Dim flag As Boolean = True
		For i As Integer = 0 To Me.bgmPlaylist.Count - 1
			If Not Me.bgmPlaylist(i).activatedManually Then
				flag = False
			End If
		Next
		If flag Then
			Return
		End If
		Me.StopBGM()
		If Me.bgmPlaylist.Count > 0 Then
			Dim levelData As PlayerData.PlayerLevelDataObject = PlayerData.Data.GetLevelData(SceneLoader.CurrentLevel)
			levelData.bgmPlayListCurrent = (levelData.bgmPlayListCurrent + 1) Mod Me.bgmPlaylist.Count
			MyBase.StartCoroutine(Me.play_track_cr())
		End If
	End Sub

	' Token: 0x06000C60 RID: 3168 RVA: 0x00086B54 File Offset: 0x00084F54
	Private Iterator Function play_track_cr() As IEnumerator
		Dim levelData As PlayerData.PlayerLevelDataObject = PlayerData.Data.GetLevelData(SceneLoader.CurrentLevel)
		While True
			While Me.bgmPlaylist(levelData.bgmPlayListCurrent).activatedManually
				levelData.bgmPlayListCurrent = (levelData.bgmPlayListCurrent + 1) Mod Me.bgmPlaylist.Count
				Yield New WaitForFixedUpdate()
			End While
			Me.bgmPlaylist(levelData.bgmPlayListCurrent).Play()
			Yield New WaitForSeconds(Me.bgmPlaylist(levelData.bgmPlayListCurrent).ClipLength())
			levelData.bgmPlayListCurrent = (levelData.bgmPlayListCurrent + 1) Mod Me.bgmPlaylist.Count
			Yield New WaitForFixedUpdate()
		End While
		Return
	End Function

	' Token: 0x06000C61 RID: 3169 RVA: 0x00086B6F File Offset: 0x00084F6F
	Private Sub OnPlay(key As String)
		If Me.dict.ContainsKey(key) Then
			If AudioManagerComponent.ShowAudioVariations OrElse AudioManagerComponent.ShowAudioPlaying Then
			End If
			Me.dict(key).Play()
		End If
	End Sub

	' Token: 0x06000C62 RID: 3170 RVA: 0x00086BA7 File Offset: 0x00084FA7
	Private Sub OnPlayLoop(key As String)
		If Me.dict.ContainsKey(key) Then
			If AudioManagerComponent.ShowAudioVariations OrElse AudioManagerComponent.ShowAudioPlaying Then
			End If
			Me.dict(key).PlayLoop()
		End If
	End Sub

	' Token: 0x06000C63 RID: 3171 RVA: 0x00086BDF File Offset: 0x00084FDF
	Private Sub OnStop(key As String)
		If Me.dict.ContainsKey(key) Then
			Me.dict(key).[Stop]()
		End If
	End Sub

	' Token: 0x06000C64 RID: 3172 RVA: 0x00086C03 File Offset: 0x00085003
	Private Sub OnPause(key As String)
		If Me.dict.ContainsKey(key) Then
			Me.dict(key).Pause()
		End If
	End Sub

	' Token: 0x06000C65 RID: 3173 RVA: 0x00086C27 File Offset: 0x00085027
	Private Sub OnUnpause(key As String)
		If Me.dict.ContainsKey(key) Then
			Me.dict(key).Unpause()
		End If
	End Sub

	' Token: 0x06000C66 RID: 3174 RVA: 0x00086C4C File Offset: 0x0008504C
	Private Sub OnPauseAllSFX()
		For Each soundGroup As AudioManagerComponent.SoundGroup In Me.sounds
			soundGroup.Pause()
		Next
	End Sub

	' Token: 0x06000C67 RID: 3175 RVA: 0x00086CA8 File Offset: 0x000850A8
	Private Sub OnUnpauseAllSFX()
		For Each soundGroup As AudioManagerComponent.SoundGroup In Me.sounds
			soundGroup.Unpause()
		Next
	End Sub

	' Token: 0x06000C68 RID: 3176 RVA: 0x00086D04 File Offset: 0x00085104
	Private Sub OnFollowOject(key As String, transform As Transform)
		If Me.dict.ContainsKey(key) Then
			Me.dict(key).emissionTransform = transform
			Me.dict(key).FollowObject(Me.dict(key).emissionTransform.position)
		End If
	End Sub

	' Token: 0x06000C69 RID: 3177 RVA: 0x00086D5B File Offset: 0x0008515B
	Private Function OnIsPlaying(key As String) As Boolean
		Return Me.dict.ContainsKey(key) AndAlso Me.dict(key).CheckIfPlaying()
	End Function

	' Token: 0x06000C6A RID: 3178 RVA: 0x00086D81 File Offset: 0x00085181
	Private Sub OnAttenuation(key As String, attenuating As Boolean, endVolume As Single)
		If Me.dict.ContainsKey(key) Then
			Me.dict(key).OnAttenuate(attenuating, endVolume)
		End If
	End Sub

	' Token: 0x06000C6B RID: 3179 RVA: 0x00086DA7 File Offset: 0x000851A7
	Private Sub OnPan(key As String, value As Single)
		If Me.dict.ContainsKey(key) Then
			Me.dict(key).Pan(value)
		End If
	End Sub

	' Token: 0x06000C6C RID: 3180 RVA: 0x00086DCC File Offset: 0x000851CC
	Private Sub OnStopAll()
		For Each soundGroup As AudioManagerComponent.SoundGroup In Me.sounds
			soundGroup.[Stop]()
		Next
	End Sub

	' Token: 0x06000C6D RID: 3181 RVA: 0x00086E28 File Offset: 0x00085228
	Private Sub OnSFXSlowDown(key As String, [end] As Single, time As Single)
		If Me.dict.ContainsKey(key) Then
			MyBase.StartCoroutine(Me.dict(key).change_pitch_sfx([end], time))
		End If
	End Sub

	' Token: 0x06000C6E RID: 3182 RVA: 0x00086E55 File Offset: 0x00085255
	Private Sub OnSFXVolume(key As String, start As Single, [end] As Single, time As Single)
		If Me.dict.ContainsKey(key) Then
			MyBase.StartCoroutine(Me.dict(key).change_volume_sfx(start, [end], time, False))
		End If
	End Sub

	' Token: 0x06000C6F RID: 3183 RVA: 0x00086E85 File Offset: 0x00085285
	Private Sub OnSFXVolumeLinear(key As String, start As Single, [end] As Single, time As Single)
		If Me.dict.ContainsKey(key) Then
			MyBase.StartCoroutine(Me.dict(key).change_volume_sfx(start, [end], time, True))
		End If
	End Sub

	' Token: 0x06000C70 RID: 3184 RVA: 0x00086EB8 File Offset: 0x000852B8
	Private Sub SnapshotTransition(snapshotNames As String(), weights As Single(), time As Single)
		Dim groups As AudioManagerMixer.Groups = AudioManagerMixer.GetGroups()
		Dim list As List(Of AudioMixerGroup) = New List(Of AudioMixerGroup)()
		Dim list2 As List(Of AudioMixerSnapshot) = New List(Of AudioMixerSnapshot)()
		list.Add(groups.master)
		list.Add(groups.bgm_Options)
		list.Add(groups.sfx_Options)
		list.Add(groups.master_Options)
		list.Add(groups.sfx)
		list.Add(groups.levelSfx)
		list.Add(groups.ambience)
		list.Add(groups.creatures)
		list.Add(groups.announcer)
		list.Add(groups.super)
		list.Add(groups.bgm)
		list.Add(groups.levelBgm)
		list.Add(groups.musicSting)
		list.Add(groups.noise)
		list.Add(groups.noiseConstant)
		list.Add(groups.noiseShortterm)
		list.Add(groups.noise1920s)
		For i As Integer = 0 To weights.Length - 1
			If list(i).audioMixer.FindSnapshot(snapshotNames(i)) IsNot Nothing Then
				list2.Add(list(0).audioMixer.FindSnapshot(snapshotNames(i)))
			Else
				Global.Debug.LogError("Snapshot string is invalid", Nothing)
			End If
		Next
		For Each audioMixerGroup As AudioMixerGroup In list
			audioMixerGroup.audioMixer.TransitionToSnapshots(list2.ToArray(), weights, time)
		Next
	End Sub

	' Token: 0x06000C71 RID: 3185 RVA: 0x0008705C File Offset: 0x0008545C
	Private Sub SetChannels()
		Dim groups As AudioManagerMixer.Groups = AudioManagerMixer.GetGroups()
		Dim channel As AudioManager.Channel = Me.channel
		Dim audioMixerGroup As AudioMixerGroup
		Dim audioMixerGroup2 As AudioMixerGroup
		If channel = AudioManager.Channel.[Default] OrElse channel <> AudioManager.Channel.Level Then
			audioMixerGroup = groups.bgm
			audioMixerGroup2 = groups.sfx
			Dim noiseConstant As AudioMixerGroup = groups.noiseConstant
			Dim noiseShortterm As AudioMixerGroup = groups.noiseShortterm
		Else
			audioMixerGroup = groups.levelBgm
			audioMixerGroup2 = groups.levelSfx
		End If
		For Each source As AudioManagerComponent.SoundGroup.Source In Me.bgmSources
			If source.audio.outputAudioMixerGroup Is Nothing Then
				source.audio.outputAudioMixerGroup = audioMixerGroup
			End If
		Next
		For Each soundGroup As AudioManagerComponent.SoundGroup In Me.sounds
			soundGroup.SetMixerGroup(audioMixerGroup2)
		Next
		For Each soundGroup2 As AudioManagerComponent.SoundGroup In Me.bgmPlaylist
			soundGroup2.SetMixerGroup(audioMixerGroup)
		Next
	End Sub

	' Token: 0x0400160E RID: 5646
	<SerializeField()>
	Private channel As AudioManager.Channel

	' Token: 0x0400160F RID: 5647
	<SerializeField()>
	Private bgmSources As List(Of AudioManagerComponent.SoundGroup.Source)

	' Token: 0x04001610 RID: 5648
	<SerializeField()>
	Private bgmAlternates As List(Of AudioManagerComponent.SoundGroup.Source)

	' Token: 0x04001611 RID: 5649
	<SerializeField()>
	Private sounds As List(Of AudioManagerComponent.SoundGroup) = New List(Of AudioManagerComponent.SoundGroup)()

	' Token: 0x04001612 RID: 5650
	<SerializeField()>
	Private bgmPlaylist As List(Of AudioManagerComponent.SoundGroup) = New List(Of AudioManagerComponent.SoundGroup)()

	' Token: 0x04001613 RID: 5651
	<SerializeField()>
	Private autoplayBGM As Boolean = True

	' Token: 0x04001614 RID: 5652
	<SerializeField()>
	Private autoplayBGMPlaylist As Boolean = True

	' Token: 0x04001615 RID: 5653
	<SerializeField()>
	Private minValue As Single()

	' Token: 0x04001616 RID: 5654
	Private dict As Dictionary(Of String, AudioManagerComponent.SoundGroup)

	' Token: 0x04001617 RID: 5655
	Public Shared ShowAudioPlaying As Boolean

	' Token: 0x04001618 RID: 5656
	Public Shared ShowAudioVariations As Boolean

	' Token: 0x020003CA RID: 970
	<Serializable()>
	Public Class SoundGroup
		' Token: 0x06000C73 RID: 3187 RVA: 0x000871F8 File Offset: 0x000855F8
		Friend Sub Init(Optional initializeDeferrals As Boolean = False)
			Me.key = Me.key.ToLowerIfNecessary()
			For i As Integer = 0 To Me.sources.Count - 1
				If Me.sources(i).audio Is Nothing Then
					Me.sources.RemoveAt(i)
					i -= 1
				End If
			Next
			For Each source As AudioManagerComponent.SoundGroup.Source In Me.sources
				source.Init(initializeDeferrals)
			Next
		End Sub

		' Token: 0x06000C74 RID: 3188 RVA: 0x000872B0 File Offset: 0x000856B0
		Friend Sub SetMixerGroup(group As AudioMixerGroup)
			For Each source As AudioManagerComponent.SoundGroup.Source In Me.sources
				If source.audio IsNot Nothing AndAlso source.audio.outputAudioMixerGroup Is Nothing Then
					source.audio.outputAudioMixerGroup = group
				End If
			Next
		End Sub

		' Token: 0x06000C75 RID: 3189 RVA: 0x00087338 File Offset: 0x00085738
		Public Sub SetVolume(v As Single)
			For Each source As AudioManagerComponent.SoundGroup.Source In Me.sources
				source.SetVolume(v)
			Next
		End Sub

		' Token: 0x06000C76 RID: 3190 RVA: 0x00087394 File Offset: 0x00085794
		Public Sub Play()
			Dim source As AudioManagerComponent.SoundGroup.Source = Me.GetSource()
			If Me.sources.Count > 1 Then
				For Each source2 As AudioManagerComponent.SoundGroup.Source In Me.sources
					If Not source.wasJustPlayed Then
						Exit For
					End If
					source = Me.GetSource()
				Next
			End If
			source.wasJustPlayed = True
			source.Play()
			For Each source3 As AudioManagerComponent.SoundGroup.Source In Me.sources
				If source3 IsNot source Then
					source3.wasJustPlayed = False
				End If
			Next
		End Sub

		' Token: 0x06000C77 RID: 3191 RVA: 0x00087480 File Offset: 0x00085880
		Public Sub PlayLoop()
			Me.GetSource().PlayLooped()
		End Sub

		' Token: 0x06000C78 RID: 3192 RVA: 0x00087490 File Offset: 0x00085890
		Public Sub Pan(pan As Single)
			For Each source As AudioManagerComponent.SoundGroup.Source In Me.sources
				source.Pan(pan)
			Next
		End Sub

		' Token: 0x06000C79 RID: 3193 RVA: 0x000874EC File Offset: 0x000858EC
		Public Sub FollowObject(position As Vector3)
			For i As Integer = 0 To Me.sources.Count - 1
				Dim source As AudioManagerComponent.SoundGroup.Source = Me.sources(i)
				source.FollowObject(position)
			Next
		End Sub

		' Token: 0x06000C7A RID: 3194 RVA: 0x0008752C File Offset: 0x0008592C
		Public Function CheckIfPlaying() As Boolean
			Me.isPlaying = False
			For Each source As AudioManagerComponent.SoundGroup.Source In Me.sources
				source.isPlaying()
				If source.isPlaying() Then
					Me.isPlaying = True
				End If
			Next
			Return Me.isPlaying
		End Function

		' Token: 0x06000C7B RID: 3195 RVA: 0x000875A8 File Offset: 0x000859A8
		Public Function ClipLength() As Single
			Dim num As Single = 0F
			For Each source As AudioManagerComponent.SoundGroup.Source In Me.sources
				num = source.ClipLength()
			Next
			Return num
		End Function

		' Token: 0x06000C7C RID: 3196 RVA: 0x0008760C File Offset: 0x00085A0C
		Public Sub OnAttenuate(attentuating As Boolean, endVolume As Single)
			For Each source As AudioManagerComponent.SoundGroup.Source In Me.sources
				source.OnAttenuate(attentuating, endVolume)
			Next
		End Sub

		' Token: 0x06000C7D RID: 3197 RVA: 0x0008766C File Offset: 0x00085A6C
		Public Iterator Function warble_pitch_cr(warbles As Integer, minValue As Single(), maxValue As Single(), incrementAmount As Single(), playTime As Single()) As IEnumerator
			Dim isDecreasing As Boolean = Rand.Bool()
			Dim t As Single = 0F
			Dim startPitch As Single = 1F
			For Each s As AudioManagerComponent.SoundGroup.Source In Me.sources
				If s IsNot Nothing AndAlso s.audio.clip IsNot Nothing Then
					For i As Integer = 0 To warbles - 1
						While t < playTime(i)
							t += CupheadTime.Delta
							If isDecreasing Then
								If s.audio.pitch > minValue(i) Then
									s.audio.pitch -= incrementAmount(i)
								Else
									isDecreasing = False
								End If
							ElseIf s.audio.pitch < maxValue(i) Then
								s.audio.pitch += incrementAmount(i)
							Else
								isDecreasing = True
							End If
							Yield Nothing
						End While
						t = 0F
						Yield Nothing
					Next
					s.audio.pitch = startPitch
				End If
			Next
			Return
		End Function

		' Token: 0x06000C7E RID: 3198 RVA: 0x000876AC File Offset: 0x00085AAC
		Public Iterator Function change_pitch_sfx([end] As Single, time As Single) As IEnumerator
			For Each s As AudioManagerComponent.SoundGroup.Source In Me.sources
				Dim t As Single = 0F
				If s IsNot Nothing AndAlso s.audio.clip IsNot Nothing Then
					While t < time
						Dim val As Single = EaseUtils.Ease(EaseUtils.EaseType.linear, 0F, 1F, t / time)
						s.audio.pitch = Mathf.Lerp(s.audio.pitch, [end], val)
						t += Time.deltaTime
						Yield Nothing
					End While
					s.audio.pitch = [end]
				End If
				Yield Nothing
			Next
			Return
		End Function

		' Token: 0x06000C7F RID: 3199 RVA: 0x000876D8 File Offset: 0x00085AD8
		Public Iterator Function change_volume_sfx(start As Single, [end] As Single, time As Single, linear As Boolean) As IEnumerator
			For Each s As AudioManagerComponent.SoundGroup.Source In Me.sources
				Dim t As Single = 0F
				If s IsNot Nothing AndAlso s.audio.clip IsNot Nothing Then
					Dim initialVolume As Single = If((start < 0F), s.audio.volume, start)
					If Not linear AndAlso start >= 0F Then
						s.audio.volume = initialVolume
					End If
					While t < time
						Dim val As Single = EaseUtils.Ease(EaseUtils.EaseType.linear, 0F, 1F, t / time)
						If linear Then
							s.audio.volume = Mathf.Lerp(initialVolume, [end], val)
						Else
							s.audio.volume = Mathf.Lerp(s.audio.volume, [end], val)
						End If
						t += Time.deltaTime
						Yield Nothing
					End While
					s.audio.volume = [end]
					If [end] = 0F Then
						s.audio.[Stop]()
					End If
				End If
				Yield Nothing
			Next
			Return
		End Function

		' Token: 0x06000C80 RID: 3200 RVA: 0x00087710 File Offset: 0x00085B10
		Public Iterator Function change_volume_cr(endVolume As Single, time As Single, onFadeOut As Boolean) As IEnumerator
			For Each s As AudioManagerComponent.SoundGroup.Source In Me.sources
				Dim t As Single = 0F
				Dim startVol As Single = If((Not onFadeOut), 0F, s.audio.volume)
				Dim endVol As Single = If((Not onFadeOut), s.audio.volume, endVolume)
				If Not onFadeOut Then
					s.audio.Play()
					Me.isFadedOut = False
				Else
					Me.isFadedOut = True
				End If
				If s.audio IsNot Nothing AndAlso s.audio.clip IsNot Nothing Then
					While t < time
						Dim val As Single = EaseUtils.Ease(EaseUtils.EaseType.linear, 0F, 1F, t / time)
						s.audio.volume = Mathf.Lerp(startVol, endVol, val)
						t += Time.deltaTime
						Yield Nothing
					End While
					If onFadeOut Then
						s.audio.[Stop]()
						s.audio.volume = s.originalVolume
						Me.isFadedOut = True
					End If
				End If
				Yield Nothing
			Next
			Return
		End Function

		' Token: 0x06000C81 RID: 3201 RVA: 0x00087740 File Offset: 0x00085B40
		Public Sub [Stop]()
			For Each source As AudioManagerComponent.SoundGroup.Source In Me.sources
				source.[Stop]()
			Next
		End Sub

		' Token: 0x06000C82 RID: 3202 RVA: 0x0008779C File Offset: 0x00085B9C
		Public Sub Pause()
			For Each source As AudioManagerComponent.SoundGroup.Source In Me.sources
				source.Pause()
			Next
		End Sub

		' Token: 0x06000C83 RID: 3203 RVA: 0x000877F8 File Offset: 0x00085BF8
		Public Sub Unpause()
			For Each source As AudioManagerComponent.SoundGroup.Source In Me.sources
				source.UnPause()
			Next
		End Sub

		' Token: 0x06000C84 RID: 3204 RVA: 0x00087854 File Offset: 0x00085C54
		Private Function GetSource() As AudioManagerComponent.SoundGroup.Source
			Return Me.sources(Global.UnityEngine.Random.Range(0, Me.sources.Count))
		End Function

		' Token: 0x04001619 RID: 5657
		<SerializeField()>
		Private sources As List(Of AudioManagerComponent.SoundGroup.Source) = New List(Of AudioManagerComponent.SoundGroup.Source)() From { New AudioManagerComponent.SoundGroup.Source() }

		' Token: 0x0400161A RID: 5658
		Public trigger As Sfx

		' Token: 0x0400161B RID: 5659
		Public key As String

		' Token: 0x0400161C RID: 5660
		Private isPlaying As Boolean

		' Token: 0x0400161D RID: 5661
		Public emissionTransform As Transform

		' Token: 0x0400161E RID: 5662
		Public activatedManually As Boolean

		' Token: 0x0400161F RID: 5663
		Public isFadedOut As Boolean

		' Token: 0x04001620 RID: 5664
		Private volume As Single

		' Token: 0x020003CB RID: 971
		<Serializable()>
		Public Class Source
			' Token: 0x06000C86 RID: 3206 RVA: 0x0008787C File Offset: 0x00085C7C
			Friend Sub Init(initializeDeferrals As Boolean)
				If initializeDeferrals Then
					Dim component As DeferredAudioSource = Me.audio.GetComponent(Of DeferredAudioSource)()
					If component IsNot Nothing Then
						component.Initialize()
					End If
				End If
				If Me.audio IsNot Nothing AndAlso Me.audio.clip IsNot Nothing Then
					Me.audio.ignoreListenerPause = True
					Me.originalVolume = Me.audio.volume
				End If
			End Sub

			' Token: 0x06000C87 RID: 3207 RVA: 0x000878F1 File Offset: 0x00085CF1
			Public Sub SetVolume(v As Single)
				If Me.audio IsNot Nothing AndAlso Me.audio.clip IsNot Nothing Then
					Me.audio.volume = v * Me.originalVolume
				End If
			End Sub

			' Token: 0x06000C88 RID: 3208 RVA: 0x00087930 File Offset: 0x00085D30
			Public Sub Play()
				If Me.audio IsNot Nothing AndAlso Me.audio.clip IsNot Nothing Then
					Me.audio.PlayOneShot(Me.audio.clip)
					If Not AudioManagerComponent.ShowAudioPlaying OrElse AudioManagerComponent.ShowAudioVariations Then
					End If
				End If
			End Sub

			' Token: 0x06000C89 RID: 3209 RVA: 0x00087990 File Offset: 0x00085D90
			Public Sub PlayLooped()
				If Me.audio IsNot Nothing AndAlso Me.audio.clip IsNot Nothing Then
					Me.audio.[loop] = True
					Me.audio.Play()
					If Not AudioManagerComponent.ShowAudioPlaying OrElse AudioManagerComponent.ShowAudioVariations Then
					End If
				End If
			End Sub

			' Token: 0x06000C8A RID: 3210 RVA: 0x000879F0 File Offset: 0x00085DF0
			Public Iterator Function change_pitch_cr([end] As Single, time As Single) As IEnumerator
				Dim t As Single = 0F
				If Me.audio IsNot Nothing AndAlso Me.audio.clip IsNot Nothing Then
					While t < time
						Dim val As Single = EaseUtils.Ease(EaseUtils.EaseType.linear, 0F, 1F, t / time)
						Me.audio.pitch = Mathf.Lerp(Me.audio.pitch, [end], val)
						t += Time.deltaTime
						Yield Nothing
					End While
					Me.audio.pitch = [end]
				End If
				Return
			End Function

			' Token: 0x06000C8B RID: 3211 RVA: 0x00087A1C File Offset: 0x00085E1C
			Public Iterator Function change_volume_cr(endVolume As Single, time As Single, onFadeOut As Boolean) As IEnumerator
				Dim t As Single = 0F
				Dim startVol As Single = If((Not onFadeOut), 0F, Me.audio.volume)
				Dim endVol As Single = If((Not onFadeOut), Me.audio.volume, endVolume)
				If Not onFadeOut Then
					Me.audio.Play()
					Me.isFadedOut = False
				Else
					Me.isFadedOut = True
				End If
				If Me.audio IsNot Nothing AndAlso Me.audio.clip IsNot Nothing Then
					While t < time
						Dim val As Single = EaseUtils.Ease(EaseUtils.EaseType.linear, 0F, 1F, t / time)
						Me.audio.volume = Mathf.Lerp(startVol, endVol, val)
						t += Time.deltaTime
						Yield Nothing
					End While
					If onFadeOut Then
						Me.audio.[Stop]()
						Me.audio.volume = Me.originalVolume
						Me.isFadedOut = True
					End If
				End If
				Yield Nothing
				Return
			End Function

			' Token: 0x06000C8C RID: 3212 RVA: 0x00087A4C File Offset: 0x00085E4C
			Public Iterator Function warble_pitch_cr(warbles As Integer, minValue As Single(), maxValue As Single(), incrementAmount As Single(), playTime As Single()) As IEnumerator
				Dim isDecreasing As Boolean = Rand.Bool()
				Dim t As Single = 0F
				Dim startPitch As Single = 1F
				If Me.audio IsNot Nothing AndAlso Me.audio.clip IsNot Nothing Then
					For i As Integer = 0 To warbles - 1
						While t < playTime(i)
							t += CupheadTime.Delta
							If isDecreasing Then
								If Me.audio.pitch > minValue(i) Then
									Me.audio.pitch -= incrementAmount(i)
								Else
									isDecreasing = False
								End If
							ElseIf Me.audio.pitch < maxValue(i) Then
								Me.audio.pitch += incrementAmount(i)
							Else
								isDecreasing = True
							End If
							Yield Nothing
						End While
						t = 0F
						Yield Nothing
					Next
					Me.audio.pitch = startPitch
				End If
				Return
			End Function

			' Token: 0x06000C8D RID: 3213 RVA: 0x00087A8C File Offset: 0x00085E8C
			Public Sub [Stop]()
				If Me.audio IsNot Nothing AndAlso Me.audio.clip IsNot Nothing Then
					Me.audio.[loop] = False
					Me.audio.[Stop]()
				End If
			End Sub

			' Token: 0x06000C8E RID: 3214 RVA: 0x00087ACC File Offset: 0x00085ECC
			Public Sub Pause()
				If Me.audio IsNot Nothing AndAlso Me.audio.clip IsNot Nothing Then
					Me.audio.Pause()
				End If
			End Sub

			' Token: 0x06000C8F RID: 3215 RVA: 0x00087B00 File Offset: 0x00085F00
			Public Sub UnPause()
				If Me.audio IsNot Nothing AndAlso Me.audio.clip IsNot Nothing Then
					Me.audio.UnPause()
				End If
			End Sub

			' Token: 0x06000C90 RID: 3216 RVA: 0x00087B34 File Offset: 0x00085F34
			Public Sub Pan(pan As Single)
				If Me.audio IsNot Nothing AndAlso Me.audio.clip IsNot Nothing Then
					Me.audio.panStereo = pan
				End If
			End Sub

			' Token: 0x06000C91 RID: 3217 RVA: 0x00087B6C File Offset: 0x00085F6C
			Public Function ClipLength() As Single
				If Me.audio IsNot Nothing AndAlso Me.audio.clip IsNot Nothing Then
					Return Me.audio.clip.length
				End If
				Global.Debug.LogError("Clip is null", Nothing)
				Return 0F
			End Function

			' Token: 0x06000C92 RID: 3218 RVA: 0x00087BC1 File Offset: 0x00085FC1
			Public Sub FollowObject(position As Vector3)
				If Me.audio IsNot Nothing Then
					Me.audio.transform.position = position
				End If
			End Sub

			' Token: 0x06000C93 RID: 3219 RVA: 0x00087BE5 File Offset: 0x00085FE5
			Public Function isPlaying() As Boolean
				Return Me.audio IsNot Nothing AndAlso Me.audio.clip IsNot Nothing AndAlso Me.audio.isPlaying
			End Function

			' Token: 0x06000C94 RID: 3220 RVA: 0x00087C1C File Offset: 0x0008601C
			Public Sub OnAttenuate(attenuating As Boolean, volumeChange As Single)
				If Me.audio IsNot Nothing AndAlso Me.audio.clip IsNot Nothing Then
					If attenuating Then
						Me.audio.volume = volumeChange
					Else
						Me.audio.volume = Me.originalVolume
					End If
				End If
			End Sub

			' Token: 0x04001621 RID: 5665
			<SerializeField()>
			Friend audio As AudioSource

			' Token: 0x04001622 RID: 5666
			Public originalVolume As Single

			' Token: 0x04001623 RID: 5667
			Public wasJustPlayed As Boolean

			' Token: 0x04001624 RID: 5668
			Public isFadedOut As Boolean

			' Token: 0x04001625 RID: 5669
			Public noLoop As Boolean
		End Class
	End Class
End Class
