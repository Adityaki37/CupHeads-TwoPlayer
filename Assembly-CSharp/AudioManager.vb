Imports System
Imports System.Collections.Generic
Imports System.Diagnostics
Imports GCFreeUtils
Imports UnityEngine
Imports UnityEngine.Audio

' Token: 0x020003B7 RID: 951
Public Module AudioManager
	' Token: 0x1400000D RID: 13
	' (add) Token: 0x06000BC1 RID: 3009 RVA: 0x00084C08 File Offset: 0x00083008
	' (remove) Token: 0x06000BC2 RID: 3010 RVA: 0x00084C3C File Offset: 0x0008303C
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnAttenuation As AudioManager.OnAttenuationHandler

	' Token: 0x1400000E RID: 14
	' (add) Token: 0x06000BC3 RID: 3011 RVA: 0x00084C70 File Offset: 0x00083070
	' (remove) Token: 0x06000BC4 RID: 3012 RVA: 0x00084CA4 File Offset: 0x000830A4
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnFollowObject As AudioManager.OnTransformHandler

	' Token: 0x1400000F RID: 15
	' (add) Token: 0x06000BC5 RID: 3013 RVA: 0x00084CD8 File Offset: 0x000830D8
	' (remove) Token: 0x06000BC6 RID: 3014 RVA: 0x00084D0C File Offset: 0x0008310C
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnPanEvent As AudioManager.OnPanEventHandler

	' Token: 0x14000010 RID: 16
	' (add) Token: 0x06000BC7 RID: 3015 RVA: 0x00084D40 File Offset: 0x00083140
	' (remove) Token: 0x06000BC8 RID: 3016 RVA: 0x00084D74 File Offset: 0x00083174
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnSnapshotEvent As AudioManager.OnSnapshotHandler

	' Token: 0x14000011 RID: 17
	' (add) Token: 0x06000BC9 RID: 3017 RVA: 0x00084DA8 File Offset: 0x000831A8
	' (remove) Token: 0x06000BCA RID: 3018 RVA: 0x00084DDC File Offset: 0x000831DC
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnBGMSlowdown As AudioManager.OnChangeBGMHandler

	' Token: 0x14000012 RID: 18
	' (add) Token: 0x06000BCB RID: 3019 RVA: 0x00084E10 File Offset: 0x00083210
	' (remove) Token: 0x06000BCC RID: 3020 RVA: 0x00084E44 File Offset: 0x00083244
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnBGMFadeVolume As AudioManager.OnChangeBGMVolumeHandler

	' Token: 0x14000013 RID: 19
	' (add) Token: 0x06000BCD RID: 3021 RVA: 0x00084E78 File Offset: 0x00083278
	' (remove) Token: 0x06000BCE RID: 3022 RVA: 0x00084EAC File Offset: 0x000832AC
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnStartBGMAlternate As AudioManager.OnStartBGMAlternateHandler

	' Token: 0x14000014 RID: 20
	' (add) Token: 0x06000BCF RID: 3023 RVA: 0x00084EE0 File Offset: 0x000832E0
	' (remove) Token: 0x06000BD0 RID: 3024 RVA: 0x00084F14 File Offset: 0x00083314
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnSFXSlowDown As AudioManager.OnChangeSFXHandler

	' Token: 0x14000015 RID: 21
	' (add) Token: 0x06000BD1 RID: 3025 RVA: 0x00084F48 File Offset: 0x00083348
	' (remove) Token: 0x06000BD2 RID: 3026 RVA: 0x00084F7C File Offset: 0x0008337C
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnSFXFadeVolume As AudioManager.OnChangeSFXStartEndHandler

	' Token: 0x14000017 RID: 23
	' (add) Token: 0x06000BD3 RID: 3027 RVA: 0x00084FB0 File Offset: 0x000833B0
	' (remove) Token: 0x06000BD4 RID: 3028 RVA: 0x00084FE4 File Offset: 0x000833E4
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnBGMPitchWarble As AudioManager.OnWarbleBGMPitchHandler

	' Token: 0x14000018 RID: 24
	' (add) Token: 0x06000BD5 RID: 3029 RVA: 0x00085018 File Offset: 0x00083418
	' (remove) Token: 0x06000BD6 RID: 3030 RVA: 0x0008504C File Offset: 0x0008344C
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnPlayManualBGM As AudioManager.OnBGMPlayListManualHandler

	' Token: 0x14000019 RID: 25
	' (add) Token: 0x06000BD7 RID: 3031 RVA: 0x00085080 File Offset: 0x00083480
	' (remove) Token: 0x06000BD8 RID: 3032 RVA: 0x000850B4 File Offset: 0x000834B4
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnPlayEvent As AudioManager.OnSfxHandler

	' Token: 0x1400001A RID: 26
	' (add) Token: 0x06000BD9 RID: 3033 RVA: 0x000850E8 File Offset: 0x000834E8
	' (remove) Token: 0x06000BDA RID: 3034 RVA: 0x0008511C File Offset: 0x0008351C
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnPlayLoopEvent As AudioManager.OnSfxHandler

	' Token: 0x1400001B RID: 27
	' (add) Token: 0x06000BDB RID: 3035 RVA: 0x00085150 File Offset: 0x00083550
	' (remove) Token: 0x06000BDC RID: 3036 RVA: 0x00085184 File Offset: 0x00083584
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnStopEvent As AudioManager.OnSfxHandler

	' Token: 0x1400001C RID: 28
	' (add) Token: 0x06000BDD RID: 3037 RVA: 0x000851B8 File Offset: 0x000835B8
	' (remove) Token: 0x06000BDE RID: 3038 RVA: 0x000851EC File Offset: 0x000835EC
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnPauseEvent As AudioManager.OnSfxHandler

	' Token: 0x1400001D RID: 29
	' (add) Token: 0x06000BDF RID: 3039 RVA: 0x00085220 File Offset: 0x00083620
	' (remove) Token: 0x06000BE0 RID: 3040 RVA: 0x00085254 File Offset: 0x00083654
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnUnpauseEvent As AudioManager.OnSfxHandler

	' Token: 0x1400001E RID: 30
	' (add) Token: 0x06000BE1 RID: 3041 RVA: 0x00085288 File Offset: 0x00083688
	' (remove) Token: 0x06000BE2 RID: 3042 RVA: 0x000852BC File Offset: 0x000836BC
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnStopAllEvent As Action

	' Token: 0x1400001F RID: 31
	' (add) Token: 0x06000BE3 RID: 3043 RVA: 0x000852F0 File Offset: 0x000836F0
	' (remove) Token: 0x06000BE4 RID: 3044 RVA: 0x00085324 File Offset: 0x00083724
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnStopBGMEvent As Action

	' Token: 0x14000020 RID: 32
	' (add) Token: 0x06000BE5 RID: 3045 RVA: 0x00085358 File Offset: 0x00083758
	' (remove) Token: 0x06000BE6 RID: 3046 RVA: 0x0008538C File Offset: 0x0008378C
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnPlayBGMEvent As Action

	' Token: 0x14000021 RID: 33
	' (add) Token: 0x06000BE7 RID: 3047 RVA: 0x000853C0 File Offset: 0x000837C0
	' (remove) Token: 0x06000BE8 RID: 3048 RVA: 0x000853F4 File Offset: 0x000837F4
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnPlayBGMPlaylistEvent As Action

	' Token: 0x14000022 RID: 34
	' (add) Token: 0x06000BE9 RID: 3049 RVA: 0x00085428 File Offset: 0x00083828
	' (remove) Token: 0x06000BEA RID: 3050 RVA: 0x0008545C File Offset: 0x0008385C
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnPauseAllSFXEvent As Action

	' Token: 0x14000023 RID: 35
	' (add) Token: 0x06000BEB RID: 3051 RVA: 0x00085490 File Offset: 0x00083890
	' (remove) Token: 0x06000BEC RID: 3052 RVA: 0x000854C4 File Offset: 0x000838C4
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnUnpauseAllSFXEvent As Action

	' Token: 0x14000024 RID: 36
	' (add) Token: 0x06000BED RID: 3053 RVA: 0x000854F8 File Offset: 0x000838F8
	' (remove) Token: 0x06000BEE RID: 3054 RVA: 0x0008552C File Offset: 0x0008392C
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnStopManualBGMTrackEvent As Action

	' Token: 0x1700020F RID: 527
	' (get) Token: 0x06000BEF RID: 3055 RVA: 0x00085560 File Offset: 0x00083960
	Private ReadOnly Property mixer As AudioMixer
		Get
			If AudioManager._mixer Is Nothing Then
				AudioManager._mixer = AudioManagerMixer.GetMixer()
			End If
			Return AudioManager._mixer
		End Get
	End Property

	' Token: 0x17000210 RID: 528
	' (get) Token: 0x06000BF0 RID: 3056 RVA: 0x00085584 File Offset: 0x00083984
	' (set) Token: 0x06000BF1 RID: 3057 RVA: 0x000855B0 File Offset: 0x000839B0
	Public Property sfxOptionsVolume As Single
		Get
			Dim num As Single
			AudioManager.mixer.GetFloat(AudioManager.[Property].Options_SFXVolume.ToString(), num)
			Return num
		End Get
		Set(value As Single)
			AudioManager.mixer.SetFloat(AudioManager.[Property].Options_SFXVolume.ToString(), value)
		End Set
	End Property

	' Token: 0x17000211 RID: 529
	' (get) Token: 0x06000BF2 RID: 3058 RVA: 0x000855D8 File Offset: 0x000839D8
	' (set) Token: 0x06000BF3 RID: 3059 RVA: 0x00085604 File Offset: 0x00083A04
	Public Property bgmOptionsVolume As Single
		Get
			Dim num As Single
			AudioManager.mixer.GetFloat(AudioManager.[Property].Options_BGMVolume.ToString(), num)
			Return num
		End Get
		Set(value As Single)
			AudioManager.mixer.SetFloat(AudioManager.[Property].Options_BGMVolume.ToString(), value)
		End Set
	End Property

	' Token: 0x17000212 RID: 530
	' (get) Token: 0x06000BF4 RID: 3060 RVA: 0x0008562C File Offset: 0x00083A2C
	' (set) Token: 0x06000BF5 RID: 3061 RVA: 0x00085658 File Offset: 0x00083A58
	Public Property masterVolume As Single
		Get
			Dim num As Single
			AudioManager.mixer.GetFloat(AudioManager.[Property].MasterVolume.ToString(), num)
			Return num
		End Get
		Set(value As Single)
			AudioManager.mixer.SetFloat(AudioManager.[Property].MasterVolume.ToString(), value)
		End Set
	End Property

	' Token: 0x06000BF6 RID: 3062 RVA: 0x00085680 File Offset: 0x00083A80
	Public Function CheckIfPlaying(key As String) As Boolean
		AudioManager.checkIfPlaying = False
		If AudioManager.OnCheckEvent IsNot Nothing Then
			key = key.ToLowerIfNecessary()
			AudioManager.checkIfPlaying = AudioManager.OnCheckEvent.CallAnyTrue(key)
			Return AudioManager.checkIfPlaying
		End If
		Return False
	End Function

	' Token: 0x06000BF7 RID: 3063 RVA: 0x000856B1 File Offset: 0x00083AB1
	Public Sub PlayBGMPlaylistManually(goThroughPlaylistAfter As Boolean)
		If AudioManager.OnPlayManualBGM IsNot Nothing Then
			AudioManager.OnPlayManualBGM(goThroughPlaylistAfter)
		End If
	End Sub

	' Token: 0x06000BF8 RID: 3064 RVA: 0x000856C8 File Offset: 0x00083AC8
	Public Sub StopBGMPlaylistManually()
		If AudioManager.OnStopManualBGMTrackEvent IsNot Nothing Then
			AudioManager.OnStopManualBGMTrackEvent()
		End If
	End Sub

	' Token: 0x06000BF9 RID: 3065 RVA: 0x000856DE File Offset: 0x00083ADE
	Public Sub ChangeSFXPitch(key As String, endPitch As Single, time As Single)
		If AudioManager.OnSFXSlowDown IsNot Nothing Then
			AudioManager.OnSFXSlowDown(key, endPitch, time)
		End If
	End Sub

	' Token: 0x06000BFA RID: 3066 RVA: 0x000856F7 File Offset: 0x00083AF7
	Public Sub ChangeBGMPitch(endPitch As Single, time As Single)
		If AudioManager.OnBGMSlowdown IsNot Nothing Then
			AudioManager.OnBGMSlowdown(endPitch, time)
		End If
	End Sub

	' Token: 0x06000BFB RID: 3067 RVA: 0x0008570F File Offset: 0x00083B0F
	Public Sub FadeBGMVolume(endVolume As Single, time As Single, fadeOut As Boolean)
		If AudioManager.OnBGMFadeVolume IsNot Nothing Then
			AudioManager.OnBGMFadeVolume(endVolume, time, fadeOut)
		End If
	End Sub

	' Token: 0x06000BFC RID: 3068 RVA: 0x00085728 File Offset: 0x00083B28
	Public Sub WarbleBGMPitch(warbles As Integer, minValue As Single(), maxValue As Single(), incrementTime As Single(), playTime As Single())
		If AudioManager.OnBGMPitchWarble IsNot Nothing Then
			AudioManager.OnBGMPitchWarble(warbles, minValue, maxValue, incrementTime, playTime)
		End If
	End Sub

	' Token: 0x06000BFD RID: 3069 RVA: 0x00085744 File Offset: 0x00083B44
	Public Sub StartBGMAlternate(index As Integer)
		If AudioManager.OnStartBGMAlternate IsNot Nothing Then
			AudioManager.OnStartBGMAlternate(index)
		End If
	End Sub

	' Token: 0x06000BFE RID: 3070 RVA: 0x0008575B File Offset: 0x00083B5B
	Public Sub Attenuation(key As String, attenuation As Boolean, endVolume As Single)
		If AudioManager.OnAttenuation IsNot Nothing Then
			AudioManager.OnAttenuation(key, attenuation, endVolume)
		End If
	End Sub

	' Token: 0x06000BFF RID: 3071 RVA: 0x00085774 File Offset: 0x00083B74
	Public Sub Play(key As String)
		key = key.ToLowerIfNecessary()
		If AudioManager.OnPlayEvent IsNot Nothing Then
			AudioManager.OnPlayEvent(key)
		End If
	End Sub

	' Token: 0x06000C00 RID: 3072 RVA: 0x00085793 File Offset: 0x00083B93
	Public Sub [Stop](key As String)
		key = key.ToLowerIfNecessary()
		If AudioManager.OnStopEvent IsNot Nothing Then
			AudioManager.OnStopEvent(key)
		End If
	End Sub

	' Token: 0x06000C01 RID: 3073 RVA: 0x000857B2 File Offset: 0x00083BB2
	Public Sub PlayLoop(key As String)
		key = key.ToLowerIfNecessary()
		If AudioManager.OnPlayLoopEvent IsNot Nothing Then
			AudioManager.OnPlayLoopEvent(key)
		End If
	End Sub

	' Token: 0x06000C02 RID: 3074 RVA: 0x000857D1 File Offset: 0x00083BD1
	Public Sub Pause(key As String)
		key = key.ToLowerIfNecessary()
		If AudioManager.OnPauseEvent IsNot Nothing Then
			AudioManager.OnPauseEvent(key)
		End If
	End Sub

	' Token: 0x06000C03 RID: 3075 RVA: 0x000857F0 File Offset: 0x00083BF0
	Public Sub Unpaused(key As String)
		key = key.ToLowerIfNecessary()
		If AudioManager.OnUnpauseEvent IsNot Nothing Then
			AudioManager.OnUnpauseEvent(key)
		End If
	End Sub

	' Token: 0x06000C04 RID: 3076 RVA: 0x0008580F File Offset: 0x00083C0F
	Public Sub Pan(key As String, value As Single)
		key = key.ToLowerIfNecessary()
		If AudioManager.OnPanEvent IsNot Nothing Then
			AudioManager.OnPanEvent(key, value)
		End If
	End Sub

	' Token: 0x06000C05 RID: 3077 RVA: 0x0008582F File Offset: 0x00083C2F
	Public Sub FadeSFXVolume(key As String, endVolume As Single, time As Single)
		AudioManager.FadeSFXVolume(key, -1F, endVolume, time)
	End Sub

	' Token: 0x06000C06 RID: 3078 RVA: 0x0008583E File Offset: 0x00083C3E
	Public Sub FadeSFXVolume(key As String, startVolume As Single, endVolume As Single, time As Single)
		If AudioManager.OnSFXFadeVolume IsNot Nothing Then
			AudioManager.OnSFXFadeVolume(key, startVolume, endVolume, time)
		End If
	End Sub

	' Token: 0x06000C07 RID: 3079 RVA: 0x00085858 File Offset: 0x00083C58
	Public Sub FadeSFXVolumeLinear(key As String, endVolume As Single, time As Single)
		AudioManager.FadeSFXVolumeLinear(key, -1F, endVolume, time)
	End Sub

	' Token: 0x06000C08 RID: 3080 RVA: 0x00085867 File Offset: 0x00083C67
	Public Sub FadeSFXVolumeLinear(key As String, startVolume As Single, endVolume As Single, time As Single)
		If AudioManager.OnSFXFadeVolumeLinear IsNot Nothing Then
			AudioManager.OnSFXFadeVolumeLinear(key, startVolume, endVolume, time)
		End If
	End Sub

	' Token: 0x06000C09 RID: 3081 RVA: 0x00085884 File Offset: 0x00083C84
	Public Sub FollowObject(keys As IEnumerable(Of String), transform As Transform)
		For Each text As String In keys
			AudioManager.FollowObject(keys, transform)
		Next
	End Sub

	' Token: 0x06000C0A RID: 3082 RVA: 0x000858D8 File Offset: 0x00083CD8
	Public Sub FollowObject(key As String, transform As Transform)
		key.ToLowerIfNecessary()
		If AudioManager.OnFollowObject IsNot Nothing Then
			AudioManager.OnFollowObject(key, transform)
		End If
	End Sub

	' Token: 0x06000C0B RID: 3083 RVA: 0x000858F7 File Offset: 0x00083CF7
	<Obsolete("Use Play(string key) instead")>
	Public Sub Play(sfx As Sfx)
		AudioManager.Play(sfx.ToString())
	End Sub

	' Token: 0x06000C0C RID: 3084 RVA: 0x0008590B File Offset: 0x00083D0B
	<Obsolete("Use Stop(string key) instead")>
	Public Sub [Stop](sfx As Sfx)
		AudioManager.[Stop](sfx.ToString())
	End Sub

	' Token: 0x06000C0D RID: 3085 RVA: 0x0008591F File Offset: 0x00083D1F
	Public Sub StopAll()
		If AudioManager.OnStopAllEvent IsNot Nothing Then
			AudioManager.OnStopAllEvent()
		End If
	End Sub

	' Token: 0x06000C0E RID: 3086 RVA: 0x00085935 File Offset: 0x00083D35
	Public Sub StopBGM()
		If AudioManager.OnStopBGMEvent IsNot Nothing Then
			AudioManager.OnStopBGMEvent()
		End If
	End Sub

	' Token: 0x06000C0F RID: 3087 RVA: 0x0008594B File Offset: 0x00083D4B
	Public Sub PlayBGM()
		If AudioManager.OnPlayBGMEvent IsNot Nothing Then
			AudioManager.OnPlayBGMEvent()
		End If
	End Sub

	' Token: 0x06000C10 RID: 3088 RVA: 0x00085961 File Offset: 0x00083D61
	Public Sub PlaylistBGM()
		If AudioManager.OnPlayBGMPlaylistEvent IsNot Nothing Then
			AudioManager.OnPlayBGMPlaylistEvent()
		End If
	End Sub

	' Token: 0x06000C11 RID: 3089 RVA: 0x00085977 File Offset: 0x00083D77
	Public Sub PauseAllSFX()
		If AudioManager.OnPauseAllSFXEvent IsNot Nothing Then
			AudioManager.OnPauseAllSFXEvent()
		End If
	End Sub

	' Token: 0x06000C12 RID: 3090 RVA: 0x0008598D File Offset: 0x00083D8D
	Public Sub UnpauseAllSFX()
		If AudioManager.OnUnpauseAllSFXEvent IsNot Nothing Then
			AudioManager.OnUnpauseAllSFXEvent()
		End If
	End Sub

	' Token: 0x06000C13 RID: 3091 RVA: 0x000859A3 File Offset: 0x00083DA3
	Public Sub SnapshotTransition(snapshotNames As String(), weights As Single(), time As Single)
		If AudioManager.OnSnapshotEvent IsNot Nothing Then
			AudioManager.OnSnapshotEvent(snapshotNames, weights, time)
		End If
	End Sub

	' Token: 0x06000C14 RID: 3092 RVA: 0x000859BC File Offset: 0x00083DBC
	Public Sub HandleSnapshot(snapshot As String, time As Single)
		Dim array As String() = New String() { AudioManager.Snapshots.Cutscene.ToString(), AudioManager.Snapshots.FrontEnd.ToString(), AudioManager.Snapshots.Unpaused.ToString(), AudioManager.Snapshots.Unpaused_Clean.ToString(), AudioManager.Snapshots.Unpaused_1920s.ToString(), AudioManager.Snapshots.Loadscreen.ToString(), AudioManager.Snapshots.Paused.ToString(), AudioManager.Snapshots.Super.ToString(), AudioManager.Snapshots.SuperStart.ToString(), AudioManager.Snapshots.Death.ToString(), AudioManager.Snapshots.EquipMenu.ToString(), AudioManager.Snapshots.RumRunners_RedBeam.ToString(), AudioManager.Snapshots.RumRunners_GreenBeam.ToString(), AudioManager.Snapshots.RumRunners_YellowBeam.ToString() }
		Dim array2 As Single() = New Single(array.Length - 1) {}
		For i As Integer = 0 To array.Length - 1
			array2(i) = If((Not(array(i) = snapshot)), 0F, 1F)
		Next
		AudioManager.SnapshotTransition(array, array2, time)
	End Sub

	' Token: 0x06000C15 RID: 3093 RVA: 0x00085B30 File Offset: 0x00083F30
	Public Sub SnapshotReset(sceneName As String, time As Single)
		Dim array As String() = New String() { AudioManager.Snapshots.Cutscene.ToString(), AudioManager.Snapshots.FrontEnd.ToString(), AudioManager.Snapshots.Unpaused.ToString(), AudioManager.Snapshots.Unpaused_Clean.ToString(), AudioManager.Snapshots.Unpaused_1920s.ToString(), AudioManager.Snapshots.Loadscreen.ToString(), AudioManager.Snapshots.Paused.ToString(), AudioManager.Snapshots.Super.ToString(), AudioManager.Snapshots.SuperStart.ToString(), AudioManager.Snapshots.Death.ToString(), AudioManager.Snapshots.EquipMenu.ToString(), AudioManager.Snapshots.RumRunners_RedBeam.ToString(), AudioManager.Snapshots.RumRunners_GreenBeam.ToString(), AudioManager.Snapshots.RumRunners_YellowBeam.ToString() }
		Dim num As Integer = 0
		For i As Integer = 0 To array.Length - 1
			If SettingsData.Data.vintageAudioEnabled Then
				If array(i) = AudioManager.Snapshots.Unpaused_1920s.ToString() Then
					num = i
				End If
			ElseIf sceneName = Scenes.scene_level_retro_arcade.ToString() Then
				If array(i) = AudioManager.Snapshots.Unpaused_Clean.ToString() Then
					num = i
				End If
			ElseIf array(i) = AudioManager.Snapshots.Unpaused.ToString() Then
				num = i
			End If
		Next
		Dim array2 As Single() = New Single(array.Length - 1) {}
		For j As Integer = 0 To array.Length - 1
			array2(j) = If((j <> num), 0F, 1F)
		Next
		AudioManager.SnapshotTransition(array, array2, time)
	End Sub

	' Token: 0x14000016 RID: 22
	' (add) Token: 0x06000C16 RID: 3094 RVA: 0x00085D54 File Offset: 0x00084154
	' (remove) Token: 0x06000C17 RID: 3095 RVA: 0x00085D88 File Offset: 0x00084188
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnSFXFadeVolumeLinear As AudioManager.OnChangeSFXStartEndHandler

	' Token: 0x04001572 RID: 5490
	Private Const VOLUME_MAX As Single = 0F

	' Token: 0x04001573 RID: 5491
	Private Const VOLUME_MIN As Single = -80F

	' Token: 0x04001574 RID: 5492
	Public OnCheckEvent As GCFreePredicateList(Of String) = New GCFreePredicateList(Of String)(10, True)

	' Token: 0x0400158D RID: 5517
	Private _mixer As AudioMixer

	' Token: 0x0400158E RID: 5518
	Private checkIfPlaying As Boolean

	' Token: 0x020003B8 RID: 952
	Public Enum Channel
		' Token: 0x04001590 RID: 5520
		[Default]
		' Token: 0x04001591 RID: 5521
		Level
	End Enum

	' Token: 0x020003B9 RID: 953
	Public Enum [Property]
		' Token: 0x04001593 RID: 5523
		MasterVolume
		' Token: 0x04001594 RID: 5524
		Options_BGMVolume
		' Token: 0x04001595 RID: 5525
		Options_SFXVolume
	End Enum

	' Token: 0x020003BA RID: 954
	Public Enum Snapshots
		' Token: 0x04001597 RID: 5527
		Cutscene
		' Token: 0x04001598 RID: 5528
		FrontEnd
		' Token: 0x04001599 RID: 5529
		Unpaused
		' Token: 0x0400159A RID: 5530
		Unpaused_Clean
		' Token: 0x0400159B RID: 5531
		Unpaused_1920s
		' Token: 0x0400159C RID: 5532
		Loadscreen
		' Token: 0x0400159D RID: 5533
		Paused
		' Token: 0x0400159E RID: 5534
		Super
		' Token: 0x0400159F RID: 5535
		SuperStart
		' Token: 0x040015A0 RID: 5536
		Death
		' Token: 0x040015A1 RID: 5537
		EquipMenu
		' Token: 0x040015A2 RID: 5538
		RumRunners_RedBeam
		' Token: 0x040015A3 RID: 5539
		RumRunners_GreenBeam
		' Token: 0x040015A4 RID: 5540
		RumRunners_YellowBeam
	End Enum

	' Token: 0x020003BB RID: 955
	' (Invoke) Token: 0x06000C1A RID: 3098
	Public Delegate Function OnCheckIfPlaying(key As String) As Boolean

	' Token: 0x020003BC RID: 956
	' (Invoke) Token: 0x06000C1E RID: 3102
	Public Delegate Sub OnSfxHandler(key As String)

	' Token: 0x020003BD RID: 957
	' (Invoke) Token: 0x06000C22 RID: 3106
	Public Delegate Sub OnTransformHandler(key As String, transform As Transform)

	' Token: 0x020003BE RID: 958
	' (Invoke) Token: 0x06000C26 RID: 3110
	Public Delegate Sub OnAttenuationHandler(key As String, attenuation As Boolean, endVolume As Single)

	' Token: 0x020003BF RID: 959
	' (Invoke) Token: 0x06000C2A RID: 3114
	Public Delegate Sub OnChangeBGMHandler([end] As Single, time As Single)

	' Token: 0x020003C0 RID: 960
	' (Invoke) Token: 0x06000C2E RID: 3118
	Public Delegate Sub OnChangeBGMVolumeHandler([end] As Single, time As Single, fadeOut As Boolean)

	' Token: 0x020003C1 RID: 961
	' (Invoke) Token: 0x06000C32 RID: 3122
	Public Delegate Sub OnStartBGMAlternateHandler(index As Integer)

	' Token: 0x020003C2 RID: 962
	' (Invoke) Token: 0x06000C36 RID: 3126
	Public Delegate Sub OnChangeSFXHandler(key As String, [end] As Single, time As Single)

	' Token: 0x020003C3 RID: 963
	' (Invoke) Token: 0x06000C3A RID: 3130
	Public Delegate Sub OnChangeSFXStartEndHandler(key As String, start As Single, [end] As Single, time As Single)

	' Token: 0x020003C4 RID: 964
	' (Invoke) Token: 0x06000C3E RID: 3134
	Public Delegate Sub OnWarbleBGMPitchHandler(warbles As Integer, minValue As Single(), maxValue As Single(), warbleTime As Single(), playTime As Single())

	' Token: 0x020003C5 RID: 965
	' (Invoke) Token: 0x06000C42 RID: 3138
	Public Delegate Sub OnSnapshotHandler(names As String(), weight As Single(), time As Single)

	' Token: 0x020003C6 RID: 966
	' (Invoke) Token: 0x06000C46 RID: 3142
	Public Delegate Sub OnPanEventHandler(key As String, value As Single)

	' Token: 0x020003C7 RID: 967
	' (Invoke) Token: 0x06000C4A RID: 3146
	Public Delegate Sub OnBGMPlayListManualHandler(loopPlayListAfter As Boolean)
End Module
