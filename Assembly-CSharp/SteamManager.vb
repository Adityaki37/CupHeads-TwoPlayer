Imports System
Imports System.Text
Imports Steamworks
Imports UnityEngine

' Token: 0x02000C5F RID: 3167
<DisallowMultipleComponent()>
Friend Class SteamManager
	Inherits MonoBehaviour

	' Token: 0x17000808 RID: 2056
	' (get) Token: 0x06004ECC RID: 20172 RVA: 0x0027AA09 File Offset: 0x00278E09
	Private Shared ReadOnly Property Instance As SteamManager
		Get
			Return If(SteamManager.s_instance, New GameObject("SteamManager").AddComponent(Of SteamManager)())
		End Get
	End Property

	' Token: 0x17000809 RID: 2057
	' (get) Token: 0x06004ECD RID: 20173 RVA: 0x0027AA26 File Offset: 0x00278E26
	Public Shared ReadOnly Property Initialized As Boolean
		Get
			Return SteamManager.Instance.m_bInitialized
		End Get
	End Property

	' Token: 0x06004ECE RID: 20174 RVA: 0x0027AA32 File Offset: 0x00278E32
	Private Shared Sub SteamAPIDebugTextHook(nSeverity As Integer, pchDebugText As StringBuilder)
	End Sub

	' Token: 0x06004ECF RID: 20175 RVA: 0x0027AA34 File Offset: 0x00278E34
	Private Sub Awake()
		If SteamManager.s_instance IsNot Nothing Then
			Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
			Return
		End If
		SteamManager.s_instance = Me
		If SteamManager.s_EverInialized Then
			Throw New Exception("Tried to Initialize the SteamAPI twice in one session!")
		End If
		If Not Packsize.Test() Then
			Global.Debug.LogError("[Steamworks.NET] Packsize Test returned false, the wrong version of Steamworks.NET is being run in this platform.", Me)
		End If
		If Not DllCheck.Test() Then
			Global.Debug.LogError("[Steamworks.NET] DllCheck Test returned false, One or more of the Steamworks binaries seems to be the wrong version.", Me)
		End If
		Try
			If SteamAPI.RestartAppIfNecessary(CType(268910UI, AppId_t)) Then
				Application.Quit()
				Return
			End If
		Catch ex As DllNotFoundException
			Global.Debug.LogError("[Steamworks.NET] Could not load [lib]steam_api.dll/so/dylib. It's likely not in the correct location. Refer to the README for more details." & vbLf + ex, Me)
			Application.Quit()
			Return
		End Try
		Me.m_bInitialized = SteamAPI.Init()
		If Not Me.m_bInitialized Then
			Global.Debug.LogError("[Steamworks.NET] SteamAPI_Init() failed. Refer to Valve's documentation or the comment above this line for more information.", Me)
			Return
		End If
		SteamManager.s_EverInialized = True
	End Sub

	' Token: 0x06004ED0 RID: 20176 RVA: 0x0027AB1C File Offset: 0x00278F1C
	Private Sub OnEnable()
		If SteamManager.s_instance Is Nothing Then
			SteamManager.s_instance = Me
		End If
		If Not Me.m_bInitialized Then
			Return
		End If
		If Me.m_SteamAPIWarningMessageHook Is Nothing Then
			Me.m_SteamAPIWarningMessageHook = AddressOf SteamManager.SteamAPIDebugTextHook
			SteamClient.SetWarningMessageHook(Me.m_SteamAPIWarningMessageHook)
		End If
	End Sub

	' Token: 0x06004ED1 RID: 20177 RVA: 0x0027AB73 File Offset: 0x00278F73
	Private Sub OnDestroy()
		If SteamManager.s_instance IsNot Me Then
			Return
		End If
		SteamManager.s_instance = Nothing
		If Not Me.m_bInitialized Then
			Return
		End If
		SteamAPI.Shutdown()
	End Sub

	' Token: 0x06004ED2 RID: 20178 RVA: 0x0027AB9D File Offset: 0x00278F9D
	Private Sub Update()
		If Not Me.m_bInitialized Then
			Return
		End If
		SteamAPI.RunCallbacks()
	End Sub

	' Token: 0x040051FA RID: 20986
	Private Shared s_instance As SteamManager

	' Token: 0x040051FB RID: 20987
	Private Shared s_EverInialized As Boolean

	' Token: 0x040051FC RID: 20988
	Private m_bInitialized As Boolean

	' Token: 0x040051FD RID: 20989
	Private m_SteamAPIWarningMessageHook As SteamAPIWarningMessageHook_t
End Class
