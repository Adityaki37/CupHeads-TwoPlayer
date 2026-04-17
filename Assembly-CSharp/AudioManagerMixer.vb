Imports System
Imports UnityEngine
Imports UnityEngine.Audio

' Token: 0x020003CC RID: 972
Public Class AudioManagerMixer
	Inherits MonoBehaviour

	' Token: 0x06000C96 RID: 3222 RVA: 0x00088FB6 File Offset: 0x000873B6
	Private Shared Sub Init()
		If AudioManagerMixer.Manager Is Nothing Then
			AudioManagerMixer.Manager = Resources.Load(Of AudioManagerMixer)("Audio/AudioMixer")
		End If
	End Sub

	' Token: 0x06000C97 RID: 3223 RVA: 0x00088FD7 File Offset: 0x000873D7
	Public Shared Function GetMixer() As AudioMixer
		AudioManagerMixer.Init()
		Return AudioManagerMixer.Manager.mixer
	End Function

	' Token: 0x06000C98 RID: 3224 RVA: 0x00088FE8 File Offset: 0x000873E8
	Public Shared Function GetGroups() As AudioManagerMixer.Groups
		AudioManagerMixer.Init()
		Return AudioManagerMixer.Manager.audioGroups
	End Function

	' Token: 0x04001626 RID: 5670
	Private Const PATH As String = "Audio/AudioMixer"

	' Token: 0x04001627 RID: 5671
	Private Shared Manager As AudioManagerMixer

	' Token: 0x04001628 RID: 5672
	<SerializeField()>
	Private mixer As AudioMixer

	' Token: 0x04001629 RID: 5673
	<SerializeField()>
	Private audioGroups As AudioManagerMixer.Groups

	' Token: 0x020003CD RID: 973
	<Serializable()>
	Public Class Groups
		' Token: 0x17000213 RID: 531
		' (get) Token: 0x06000C9A RID: 3226 RVA: 0x00089001 File Offset: 0x00087401
		Public ReadOnly Property master As AudioMixerGroup
			Get
				Return Me._master
			End Get
		End Property

		' Token: 0x17000214 RID: 532
		' (get) Token: 0x06000C9B RID: 3227 RVA: 0x00089009 File Offset: 0x00087409
		Public ReadOnly Property master_Options As AudioMixerGroup
			Get
				Return Me._master_Options
			End Get
		End Property

		' Token: 0x17000215 RID: 533
		' (get) Token: 0x06000C9C RID: 3228 RVA: 0x00089011 File Offset: 0x00087411
		Public ReadOnly Property bgm_Options As AudioMixerGroup
			Get
				Return Me._bgm_Options
			End Get
		End Property

		' Token: 0x17000216 RID: 534
		' (get) Token: 0x06000C9D RID: 3229 RVA: 0x00089019 File Offset: 0x00087419
		Public ReadOnly Property sfx_Options As AudioMixerGroup
			Get
				Return Me._sfx_Options
			End Get
		End Property

		' Token: 0x17000217 RID: 535
		' (get) Token: 0x06000C9E RID: 3230 RVA: 0x00089021 File Offset: 0x00087421
		Public ReadOnly Property bgm As AudioMixerGroup
			Get
				Return Me._bgm
			End Get
		End Property

		' Token: 0x17000218 RID: 536
		' (get) Token: 0x06000C9F RID: 3231 RVA: 0x00089029 File Offset: 0x00087429
		Public ReadOnly Property levelBgm As AudioMixerGroup
			Get
				Return Me._levelBgm
			End Get
		End Property

		' Token: 0x17000219 RID: 537
		' (get) Token: 0x06000CA0 RID: 3232 RVA: 0x00089031 File Offset: 0x00087431
		Public ReadOnly Property musicSting As AudioMixerGroup
			Get
				Return Me._musicSting
			End Get
		End Property

		' Token: 0x1700021A RID: 538
		' (get) Token: 0x06000CA1 RID: 3233 RVA: 0x00089039 File Offset: 0x00087439
		Public ReadOnly Property sfx As AudioMixerGroup
			Get
				Return Me._sfx
			End Get
		End Property

		' Token: 0x1700021B RID: 539
		' (get) Token: 0x06000CA2 RID: 3234 RVA: 0x00089041 File Offset: 0x00087441
		Public ReadOnly Property levelSfx As AudioMixerGroup
			Get
				Return Me._levelSfx
			End Get
		End Property

		' Token: 0x1700021C RID: 540
		' (get) Token: 0x06000CA3 RID: 3235 RVA: 0x00089049 File Offset: 0x00087449
		Public ReadOnly Property ambience As AudioMixerGroup
			Get
				Return Me._ambience
			End Get
		End Property

		' Token: 0x1700021D RID: 541
		' (get) Token: 0x06000CA4 RID: 3236 RVA: 0x00089051 File Offset: 0x00087451
		Public ReadOnly Property creatures As AudioMixerGroup
			Get
				Return Me._creatures
			End Get
		End Property

		' Token: 0x1700021E RID: 542
		' (get) Token: 0x06000CA5 RID: 3237 RVA: 0x00089059 File Offset: 0x00087459
		Public ReadOnly Property announcer As AudioMixerGroup
			Get
				Return Me._announcer
			End Get
		End Property

		' Token: 0x1700021F RID: 543
		' (get) Token: 0x06000CA6 RID: 3238 RVA: 0x00089061 File Offset: 0x00087461
		Public ReadOnly Property super As AudioMixerGroup
			Get
				Return Me._super
			End Get
		End Property

		' Token: 0x17000220 RID: 544
		' (get) Token: 0x06000CA7 RID: 3239 RVA: 0x00089069 File Offset: 0x00087469
		Public ReadOnly Property noise As AudioMixerGroup
			Get
				Return Me._noise
			End Get
		End Property

		' Token: 0x17000221 RID: 545
		' (get) Token: 0x06000CA8 RID: 3240 RVA: 0x00089071 File Offset: 0x00087471
		Public ReadOnly Property noiseConstant As AudioMixerGroup
			Get
				Return Me._noiseConstant
			End Get
		End Property

		' Token: 0x17000222 RID: 546
		' (get) Token: 0x06000CA9 RID: 3241 RVA: 0x00089079 File Offset: 0x00087479
		Public ReadOnly Property noiseShortterm As AudioMixerGroup
			Get
				Return Me._noiseShortterm
			End Get
		End Property

		' Token: 0x17000223 RID: 547
		' (get) Token: 0x06000CAA RID: 3242 RVA: 0x00089081 File Offset: 0x00087481
		Public ReadOnly Property noise1920s As AudioMixerGroup
			Get
				Return Me._noise1920s
			End Get
		End Property

		' Token: 0x0400162A RID: 5674
		<SerializeField()>
		Private _master As AudioMixerGroup

		' Token: 0x0400162B RID: 5675
		<SerializeField()>
		Private _master_Options As AudioMixerGroup

		' Token: 0x0400162C RID: 5676
		<SerializeField()>
		Private _bgm_Options As AudioMixerGroup

		' Token: 0x0400162D RID: 5677
		<SerializeField()>
		Private _sfx_Options As AudioMixerGroup

		' Token: 0x0400162E RID: 5678
		<Space(10F)>
		<Header("BGM")>
		<SerializeField()>
		Private _bgm As AudioMixerGroup

		' Token: 0x0400162F RID: 5679
		<SerializeField()>
		Private _levelBgm As AudioMixerGroup

		' Token: 0x04001630 RID: 5680
		<SerializeField()>
		Private _musicSting As AudioMixerGroup

		' Token: 0x04001631 RID: 5681
		<Space(10F)>
		<Header("SFX")>
		<SerializeField()>
		Private _sfx As AudioMixerGroup

		' Token: 0x04001632 RID: 5682
		<SerializeField()>
		Private _levelSfx As AudioMixerGroup

		' Token: 0x04001633 RID: 5683
		<SerializeField()>
		Private _ambience As AudioMixerGroup

		' Token: 0x04001634 RID: 5684
		<SerializeField()>
		Private _creatures As AudioMixerGroup

		' Token: 0x04001635 RID: 5685
		<SerializeField()>
		Private _announcer As AudioMixerGroup

		' Token: 0x04001636 RID: 5686
		<SerializeField()>
		Private _super As AudioMixerGroup

		' Token: 0x04001637 RID: 5687
		<Space(10F)>
		<Header("Noise")>
		<SerializeField()>
		Private _noise As AudioMixerGroup

		' Token: 0x04001638 RID: 5688
		<SerializeField()>
		Private _noiseConstant As AudioMixerGroup

		' Token: 0x04001639 RID: 5689
		<SerializeField()>
		Private _noiseShortterm As AudioMixerGroup

		' Token: 0x0400163A RID: 5690
		<SerializeField()>
		Private _noise1920s As AudioMixerGroup
	End Class
End Class
