Imports System
Imports System.Collections.Generic
Imports System.Diagnostics
Imports UnityEngine

' Token: 0x0200041D RID: 1053
<Serializable()>
Public Class SettingsData
	' Token: 0x06000F18 RID: 3864 RVA: 0x00096C0C File Offset: 0x0009500C
	Public Sub New()
		Me.overscan = 0F
		Me.chromaticAberration = 1F
		Me.screenWidth = Screen.currentResolution.width
		Me.screenHeight = Screen.currentResolution.height
		Me.fullScreen = Screen.fullScreen
		Me.vSyncCount = QualitySettings.vSyncCount
		Me.masterVolume = SettingsData.originalMasterVolume
		Me.sFXVolume = SettingsData.originalsFXVolume
		Me.musicVolume = SettingsData.originalMusicVolume
		Me.hasBootedUpGame = False
		Me.SetCameraEffectDefaults()
	End Sub

	' Token: 0x17000262 RID: 610
	' (get) Token: 0x06000F19 RID: 3865 RVA: 0x00096CB0 File Offset: 0x000950B0
	Public Shared ReadOnly Property Data As SettingsData
		Get
			If SettingsData._data Is Nothing Then
				If Not SettingsData.originalAudioValuesInitialized Then
					SettingsData.originalAudioValuesInitialized = True
					SettingsData.originalMasterVolume = AudioManager.masterVolume
					SettingsData.originalsFXVolume = AudioManager.sfxOptionsVolume
					SettingsData.originalMusicVolume = AudioManager.bgmOptionsVolume
				End If
				If SettingsData.hasKey() Then
					Try
						SettingsData._data = JsonUtility.FromJson(Of SettingsData)(PlayerPrefs.GetString("cuphead_settings_data_v1"))
					Catch ex As ArgumentException
						SettingsData._data = New SettingsData()
						SettingsData.Save()
					End Try
				Else
					SettingsData._data = New SettingsData()
					SettingsData.Save()
				End If
				If SettingsData._data Is Nothing Then
					Return Nothing
				End If
				SettingsData.ApplySettings()
			End If
			Return SettingsData._data
		End Get
	End Property

	' Token: 0x06000F1A RID: 3866 RVA: 0x00096D6C File Offset: 0x0009516C
	Public Shared Sub Save()
		Dim text As String = JsonUtility.ToJson(SettingsData._data)
		PlayerPrefs.SetString("cuphead_settings_data_v1", text)
		PlayerPrefs.Save()
	End Sub

	' Token: 0x06000F1B RID: 3867 RVA: 0x00096D94 File Offset: 0x00095194
	Public Shared Sub LoadFromCloud(handler As SettingsData.SettingsDataLoadFromCloudHandler)
		SettingsData._loadFromCloudHandler = handler
		If OnlineManager.Instance.[Interface].CloudStorageInitialized Then
			OnlineManager.Instance.[Interface].LoadCloudData(New String() { "cuphead_settings_data_v1" }, AddressOf SettingsData.OnLoadedCloudData)
		End If
	End Sub

	' Token: 0x06000F1C RID: 3868 RVA: 0x00096DF8 File Offset: 0x000951F8
	Public Shared Sub SaveToCloud()
		If OnlineManager.Instance.[Interface].CloudStorageInitialized Then
			Dim text As String = JsonUtility.ToJson(SettingsData._data)
			Dim dictionary As Dictionary(Of String, String) = New Dictionary(Of String, String)()
			dictionary("cuphead_settings_data_v1") = text
			OnlineManager.Instance.[Interface].SaveCloudData(dictionary, AddressOf SettingsData.OnSavedCloudData)
		End If
	End Sub

	' Token: 0x06000F1D RID: 3869 RVA: 0x00096E63 File Offset: 0x00095263
	Private Shared Sub OnSavedCloudData(success As Boolean)
	End Sub

	' Token: 0x06000F1E RID: 3870 RVA: 0x00096E68 File Offset: 0x00095268
	Private Shared Sub OnLoadedCloudData(data As String(), result As CloudLoadResult)
		If result = CloudLoadResult.Failed Then
			SettingsData.LoadFromCloud(SettingsData._loadFromCloudHandler)
			Return
		End If
		Try
			If result = CloudLoadResult.NoData Then
				If SettingsData.hasKey() Then
					Try
						SettingsData._data = JsonUtility.FromJson(Of SettingsData)(PlayerPrefs.GetString("cuphead_settings_data_v1"))
					Catch ex As ArgumentException
						SettingsData._data = New SettingsData()
					End Try
				Else
					SettingsData._data = New SettingsData()
				End If
				SettingsData.SaveToCloud()
			Else
				SettingsData._data = JsonUtility.FromJson(Of SettingsData)(data(0))
			End If
		Catch ex2 As ArgumentException
		End Try
		If SettingsData._loadFromCloudHandler IsNot Nothing Then
			SettingsData._loadFromCloudHandler(True)
			SettingsData._loadFromCloudHandler = Nothing
		End If
	End Sub

	' Token: 0x06000F1F RID: 3871 RVA: 0x00096F2C File Offset: 0x0009532C
	Public Shared Sub Reset()
		SettingsData._data = New SettingsData()
		SettingsData.Save()
	End Sub

	' Token: 0x06000F20 RID: 3872 RVA: 0x00096F3D File Offset: 0x0009533D
	Public Shared Sub ApplySettings()
		If SettingsData.OnSettingsAppliedEvent IsNot Nothing Then
			SettingsData.OnSettingsAppliedEvent()
		End If
		SettingsData.Save()
	End Sub

	' Token: 0x06000F21 RID: 3873 RVA: 0x00096F58 File Offset: 0x00095358
	Public Shared Sub ApplySettingsOnStartup()
		If Screen.width < 320 OrElse Screen.height < 240 Then
			SettingsData.Data.screenWidth = 640
			SettingsData.Data.screenHeight = 480
			SettingsData.Data.fullScreen = False
			Screen.SetResolution(SettingsData.Data.screenWidth, SettingsData.Data.screenHeight, SettingsData.Data.fullScreen)
		End If
		QualitySettings.vSyncCount = SettingsData.Data.vSyncCount
		AudioManager.masterVolume = SettingsData.Data.masterVolume
		AudioManager.sfxOptionsVolume = SettingsData.Data.sFXVolume
		AudioManager.bgmOptionsVolume = SettingsData.Data.musicVolume
	End Sub

	' Token: 0x06000F22 RID: 3874 RVA: 0x0009700B File Offset: 0x0009540B
	Private Shared Function hasKey() As Boolean
		Return PlayerPrefs.HasKey("cuphead_settings_data_v1")
	End Function

	' Token: 0x14000028 RID: 40
	' (add) Token: 0x06000F23 RID: 3875 RVA: 0x00097018 File Offset: 0x00095418
	' (remove) Token: 0x06000F24 RID: 3876 RVA: 0x0009704C File Offset: 0x0009544C
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Shared Event OnSettingsAppliedEvent As Action

	' Token: 0x17000263 RID: 611
	' (get) Token: 0x06000F25 RID: 3877 RVA: 0x00097080 File Offset: 0x00095480
	Public ReadOnly Property vintageAudioEnabled As Boolean
		Get
			Return PlayerData.inGame AndAlso PlayerData.Data.vintageAudioEnabled
		End Get
	End Property

	' Token: 0x17000264 RID: 612
	' (get) Token: 0x06000F26 RID: 3878 RVA: 0x00097098 File Offset: 0x00095498
	Public ReadOnly Property filter As BlurGamma.Filter
		Get
			If Not PlayerData.inGame Then
				Return BlurGamma.Filter.None
			End If
			Return PlayerData.Data.filter
		End Get
	End Property

	' Token: 0x17000265 RID: 613
	' (get) Token: 0x06000F27 RID: 3879 RVA: 0x000970B0 File Offset: 0x000954B0
	' (set) Token: 0x06000F28 RID: 3880 RVA: 0x000970BE File Offset: 0x000954BE
	Public Property Brightness As Single
		Get
			Me.ClampBrightness()
			Return Me.brightness
		End Get
		Set(value As Single)
			Me.brightness = value
			Me.ClampBrightness()
		End Set
	End Property

	' Token: 0x06000F29 RID: 3881 RVA: 0x000970CD File Offset: 0x000954CD
	Private Sub SetCameraEffectDefaults()
		Me.chromaticAberrationEffect = True
		Me.noiseEffect = True
		Me.subtleBlurEffect = True
		Me.brightness = 0F
	End Sub

	' Token: 0x06000F2A RID: 3882 RVA: 0x000970EF File Offset: 0x000954EF
	Private Sub ClampBrightness()
		If Me.brightness < -1F Then
			Me.brightness = -1F
		End If
		If Me.brightness > 1F Then
			Me.brightness = 1F
		End If
	End Sub

	' Token: 0x04001839 RID: 6201
	Public Const KEY As String = "cuphead_settings_data_v1"

	' Token: 0x0400183A RID: 6202
	Private Shared _loadFromCloudHandler As SettingsData.SettingsDataLoadFromCloudHandler

	' Token: 0x0400183B RID: 6203
	Private Shared _data As SettingsData

	' Token: 0x0400183D RID: 6205
	Public hasBootedUpGame As Boolean

	' Token: 0x0400183E RID: 6206
	Public overscan As Single

	' Token: 0x0400183F RID: 6207
	Public chromaticAberration As Single

	' Token: 0x04001840 RID: 6208
	Public screenWidth As Integer

	' Token: 0x04001841 RID: 6209
	Public screenHeight As Integer

	' Token: 0x04001842 RID: 6210
	Public vSyncCount As Integer

	' Token: 0x04001843 RID: 6211
	Public fullScreen As Boolean

	' Token: 0x04001844 RID: 6212
	Public forceOriginalTitleScreen As Boolean

	' Token: 0x04001845 RID: 6213
	Public masterVolume As Single

	' Token: 0x04001846 RID: 6214
	Public sFXVolume As Single

	' Token: 0x04001847 RID: 6215
	Public musicVolume As Single

	' Token: 0x04001848 RID: 6216
	Private Shared originalAudioValuesInitialized As Boolean

	' Token: 0x04001849 RID: 6217
	Private Shared originalMasterVolume As Single

	' Token: 0x0400184A RID: 6218
	Private Shared originalsFXVolume As Single

	' Token: 0x0400184B RID: 6219
	Private Shared originalMusicVolume As Single

	' Token: 0x0400184C RID: 6220
	Public canVibrate As Boolean = True

	' Token: 0x0400184D RID: 6221
	Public rotateControlsWithCamera As Boolean

	' Token: 0x0400184E RID: 6222
	Public language As Integer = -1

	' Token: 0x0400184F RID: 6223
	Public chromaticAberrationEffect As Boolean

	' Token: 0x04001850 RID: 6224
	Public noiseEffect As Boolean

	' Token: 0x04001851 RID: 6225
	Public subtleBlurEffect As Boolean

	' Token: 0x04001852 RID: 6226
	<SerializeField()>
	Private brightness As Single

	' Token: 0x0200041E RID: 1054
	' (Invoke) Token: 0x06000F2D RID: 3885
	Public Delegate Sub SettingsDataLoadFromCloudHandler(success As Boolean)
End Class
