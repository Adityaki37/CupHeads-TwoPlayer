Imports System
Imports System.Collections.Generic
Imports System.Collections.ObjectModel
Imports System.Diagnostics
Imports Rewired.Utils
Imports UnityEngine
Imports UnityEngine.Events
Imports UnityEngine.EventSystems
Imports UnityEngine.UI

Namespace Rewired.UI.ControlMapper
	' Token: 0x02000C0D RID: 3085
	<AddComponentMenu("")>
	Public Class ControlMapper
		Inherits MonoBehaviour

		' Token: 0x140000E4 RID: 228
		' (add) Token: 0x0600499C RID: 18844 RVA: 0x00267793 File Offset: 0x00265B93
		' (remove) Token: 0x0600499D RID: 18845 RVA: 0x002677AC File Offset: 0x00265BAC
		Public Custom Event ScreenClosedEvent As Action
			AddHandler
				Me._ScreenClosedEvent = CType([Delegate].Combine(Me._ScreenClosedEvent, value), Action)
			End AddHandler
			RemoveHandler
				Me._ScreenClosedEvent = CType([Delegate].Remove(Me._ScreenClosedEvent, value), Action)
			End RemoveHandler
		End Event

		' Token: 0x140000E5 RID: 229
		' (add) Token: 0x0600499E RID: 18846 RVA: 0x002677C5 File Offset: 0x00265BC5
		' (remove) Token: 0x0600499F RID: 18847 RVA: 0x002677DE File Offset: 0x00265BDE
		Public Custom Event ScreenOpenedEvent As Action
			AddHandler
				Me._ScreenOpenedEvent = CType([Delegate].Combine(Me._ScreenOpenedEvent, value), Action)
			End AddHandler
			RemoveHandler
				Me._ScreenOpenedEvent = CType([Delegate].Remove(Me._ScreenOpenedEvent, value), Action)
			End RemoveHandler
		End Event

		' Token: 0x140000E6 RID: 230
		' (add) Token: 0x060049A0 RID: 18848 RVA: 0x002677F7 File Offset: 0x00265BF7
		' (remove) Token: 0x060049A1 RID: 18849 RVA: 0x00267810 File Offset: 0x00265C10
		Public Custom Event PopupWindowClosedEvent As Action
			AddHandler
				Me._PopupWindowClosedEvent = CType([Delegate].Combine(Me._PopupWindowClosedEvent, value), Action)
			End AddHandler
			RemoveHandler
				Me._PopupWindowClosedEvent = CType([Delegate].Remove(Me._PopupWindowClosedEvent, value), Action)
			End RemoveHandler
		End Event

		' Token: 0x140000E7 RID: 231
		' (add) Token: 0x060049A2 RID: 18850 RVA: 0x00267829 File Offset: 0x00265C29
		' (remove) Token: 0x060049A3 RID: 18851 RVA: 0x00267842 File Offset: 0x00265C42
		Public Custom Event PopupWindowOpenedEvent As Action
			AddHandler
				Me._PopupWindowOpenedEvent = CType([Delegate].Combine(Me._PopupWindowOpenedEvent, value), Action)
			End AddHandler
			RemoveHandler
				Me._PopupWindowOpenedEvent = CType([Delegate].Remove(Me._PopupWindowOpenedEvent, value), Action)
			End RemoveHandler
		End Event

		' Token: 0x140000E8 RID: 232
		' (add) Token: 0x060049A4 RID: 18852 RVA: 0x0026785B File Offset: 0x00265C5B
		' (remove) Token: 0x060049A5 RID: 18853 RVA: 0x00267874 File Offset: 0x00265C74
		Public Custom Event InputPollingStartedEvent As Action
			AddHandler
				Me._InputPollingStartedEvent = CType([Delegate].Combine(Me._InputPollingStartedEvent, value), Action)
			End AddHandler
			RemoveHandler
				Me._InputPollingStartedEvent = CType([Delegate].Remove(Me._InputPollingStartedEvent, value), Action)
			End RemoveHandler
		End Event

		' Token: 0x140000E9 RID: 233
		' (add) Token: 0x060049A6 RID: 18854 RVA: 0x0026788D File Offset: 0x00265C8D
		' (remove) Token: 0x060049A7 RID: 18855 RVA: 0x002678A6 File Offset: 0x00265CA6
		Public Custom Event InputPollingEndedEvent As Action
			AddHandler
				Me._InputPollingEndedEvent = CType([Delegate].Combine(Me._InputPollingEndedEvent, value), Action)
			End AddHandler
			RemoveHandler
				Me._InputPollingEndedEvent = CType([Delegate].Remove(Me._InputPollingEndedEvent, value), Action)
			End RemoveHandler
		End Event

		' Token: 0x140000EA RID: 234
		' (add) Token: 0x060049A8 RID: 18856 RVA: 0x002678BF File Offset: 0x00265CBF
		' (remove) Token: 0x060049A9 RID: 18857 RVA: 0x002678CD File Offset: 0x00265CCD
		Public Custom Event onScreenClosed As UnityAction
			AddHandler
				Me._onScreenClosed.AddListener(value)
			End AddHandler
			RemoveHandler
				Me._onScreenClosed.RemoveListener(value)
			End RemoveHandler
		End Event

		' Token: 0x140000EB RID: 235
		' (add) Token: 0x060049AA RID: 18858 RVA: 0x002678DB File Offset: 0x00265CDB
		' (remove) Token: 0x060049AB RID: 18859 RVA: 0x002678E9 File Offset: 0x00265CE9
		Public Custom Event onScreenOpened As UnityAction
			AddHandler
				Me._onScreenOpened.AddListener(value)
			End AddHandler
			RemoveHandler
				Me._onScreenOpened.RemoveListener(value)
			End RemoveHandler
		End Event

		' Token: 0x140000EC RID: 236
		' (add) Token: 0x060049AC RID: 18860 RVA: 0x002678F7 File Offset: 0x00265CF7
		' (remove) Token: 0x060049AD RID: 18861 RVA: 0x00267905 File Offset: 0x00265D05
		Public Custom Event onPopupWindowClosed As UnityAction
			AddHandler
				Me._onPopupWindowClosed.AddListener(value)
			End AddHandler
			RemoveHandler
				Me._onPopupWindowClosed.RemoveListener(value)
			End RemoveHandler
		End Event

		' Token: 0x140000ED RID: 237
		' (add) Token: 0x060049AE RID: 18862 RVA: 0x00267913 File Offset: 0x00265D13
		' (remove) Token: 0x060049AF RID: 18863 RVA: 0x00267921 File Offset: 0x00265D21
		Public Custom Event onPopupWindowOpened As UnityAction
			AddHandler
				Me._onPopupWindowOpened.AddListener(value)
			End AddHandler
			RemoveHandler
				Me._onPopupWindowOpened.RemoveListener(value)
			End RemoveHandler
		End Event

		' Token: 0x140000EE RID: 238
		' (add) Token: 0x060049B0 RID: 18864 RVA: 0x0026792F File Offset: 0x00265D2F
		' (remove) Token: 0x060049B1 RID: 18865 RVA: 0x0026793D File Offset: 0x00265D3D
		Public Custom Event onInputPollingStarted As UnityAction
			AddHandler
				Me._onInputPollingStarted.AddListener(value)
			End AddHandler
			RemoveHandler
				Me._onInputPollingStarted.RemoveListener(value)
			End RemoveHandler
		End Event

		' Token: 0x140000EF RID: 239
		' (add) Token: 0x060049B2 RID: 18866 RVA: 0x0026794B File Offset: 0x00265D4B
		' (remove) Token: 0x060049B3 RID: 18867 RVA: 0x00267959 File Offset: 0x00265D59
		Public Custom Event onInputPollingEnded As UnityAction
			AddHandler
				Me._onInputPollingEnded.AddListener(value)
			End AddHandler
			RemoveHandler
				Me._onInputPollingEnded.RemoveListener(value)
			End RemoveHandler
		End Event

		' Token: 0x17000699 RID: 1689
		' (get) Token: 0x060049B4 RID: 18868 RVA: 0x00267967 File Offset: 0x00265D67
		' (set) Token: 0x060049B5 RID: 18869 RVA: 0x0026796F File Offset: 0x00265D6F
		Public Property rewiredInputManager As InputManager
			Get
				Return Me._rewiredInputManager
			End Get
			Set(value As InputManager)
				Me._rewiredInputManager = value
				Me.InspectorPropertyChanged(True)
			End Set
		End Property

		' Token: 0x1700069A RID: 1690
		' (get) Token: 0x060049B6 RID: 18870 RVA: 0x0026797F File Offset: 0x00265D7F
		' (set) Token: 0x060049B7 RID: 18871 RVA: 0x00267987 File Offset: 0x00265D87
		Public Property dontDestroyOnLoad As Boolean
			Get
				Return Me._dontDestroyOnLoad
			End Get
			Set(value As Boolean)
				If value <> Me._dontDestroyOnLoad AndAlso value Then
					Global.UnityEngine.[Object].DontDestroyOnLoad(MyBase.transform.gameObject)
				End If
				Me._dontDestroyOnLoad = value
			End Set
		End Property

		' Token: 0x1700069B RID: 1691
		' (get) Token: 0x060049B8 RID: 18872 RVA: 0x002679B2 File Offset: 0x00265DB2
		' (set) Token: 0x060049B9 RID: 18873 RVA: 0x002679BA File Offset: 0x00265DBA
		Public Property keyboardMapDefaultLayout As Integer
			Get
				Return Me._keyboardMapDefaultLayout
			End Get
			Set(value As Integer)
				Me._keyboardMapDefaultLayout = value
				Me.InspectorPropertyChanged(True)
			End Set
		End Property

		' Token: 0x1700069C RID: 1692
		' (get) Token: 0x060049BA RID: 18874 RVA: 0x002679CA File Offset: 0x00265DCA
		' (set) Token: 0x060049BB RID: 18875 RVA: 0x002679D2 File Offset: 0x00265DD2
		Public Property mouseMapDefaultLayout As Integer
			Get
				Return Me._mouseMapDefaultLayout
			End Get
			Set(value As Integer)
				Me._mouseMapDefaultLayout = value
				Me.InspectorPropertyChanged(True)
			End Set
		End Property

		' Token: 0x1700069D RID: 1693
		' (get) Token: 0x060049BC RID: 18876 RVA: 0x002679E2 File Offset: 0x00265DE2
		' (set) Token: 0x060049BD RID: 18877 RVA: 0x002679EA File Offset: 0x00265DEA
		Public Property joystickMapDefaultLayout As Integer
			Get
				Return Me._joystickMapDefaultLayout
			End Get
			Set(value As Integer)
				Me._joystickMapDefaultLayout = value
				Me.InspectorPropertyChanged(True)
			End Set
		End Property

		' Token: 0x1700069E RID: 1694
		' (get) Token: 0x060049BE RID: 18878 RVA: 0x002679FA File Offset: 0x00265DFA
		' (set) Token: 0x060049BF RID: 18879 RVA: 0x00267A17 File Offset: 0x00265E17
		Public Property showPlayers As Boolean
			Get
				Return Me._showPlayers AndAlso ReInput.players.playerCount > 1
			End Get
			Set(value As Boolean)
				Me._showPlayers = value
				Me.InspectorPropertyChanged(True)
			End Set
		End Property

		' Token: 0x1700069F RID: 1695
		' (get) Token: 0x060049C0 RID: 18880 RVA: 0x00267A27 File Offset: 0x00265E27
		' (set) Token: 0x060049C1 RID: 18881 RVA: 0x00267A2F File Offset: 0x00265E2F
		Public Property showControllers As Boolean
			Get
				Return Me._showControllers
			End Get
			Set(value As Boolean)
				Me._showControllers = value
				Me.InspectorPropertyChanged(True)
			End Set
		End Property

		' Token: 0x170006A0 RID: 1696
		' (get) Token: 0x060049C2 RID: 18882 RVA: 0x00267A3F File Offset: 0x00265E3F
		' (set) Token: 0x060049C3 RID: 18883 RVA: 0x00267A47 File Offset: 0x00265E47
		Public Property showKeyboard As Boolean
			Get
				Return Me._showKeyboard
			End Get
			Set(value As Boolean)
				Me._showKeyboard = value
				Me.InspectorPropertyChanged(True)
			End Set
		End Property

		' Token: 0x170006A1 RID: 1697
		' (get) Token: 0x060049C4 RID: 18884 RVA: 0x00267A57 File Offset: 0x00265E57
		' (set) Token: 0x060049C5 RID: 18885 RVA: 0x00267A5F File Offset: 0x00265E5F
		Public Property showMouse As Boolean
			Get
				Return Me._showMouse
			End Get
			Set(value As Boolean)
				Me._showMouse = value
				Me.InspectorPropertyChanged(True)
			End Set
		End Property

		' Token: 0x170006A2 RID: 1698
		' (get) Token: 0x060049C6 RID: 18886 RVA: 0x00267A6F File Offset: 0x00265E6F
		' (set) Token: 0x060049C7 RID: 18887 RVA: 0x00267A77 File Offset: 0x00265E77
		Public Property maxControllersPerPlayer As Integer
			Get
				Return Me._maxControllersPerPlayer
			End Get
			Set(value As Integer)
				Me._maxControllersPerPlayer = value
				Me.InspectorPropertyChanged(True)
			End Set
		End Property

		' Token: 0x170006A3 RID: 1699
		' (get) Token: 0x060049C8 RID: 18888 RVA: 0x00267A87 File Offset: 0x00265E87
		' (set) Token: 0x060049C9 RID: 18889 RVA: 0x00267A8F File Offset: 0x00265E8F
		Public Property showActionCategoryLabels As Boolean
			Get
				Return Me._showActionCategoryLabels
			End Get
			Set(value As Boolean)
				Me._showActionCategoryLabels = value
				Me.InspectorPropertyChanged(True)
			End Set
		End Property

		' Token: 0x170006A4 RID: 1700
		' (get) Token: 0x060049CA RID: 18890 RVA: 0x00267A9F File Offset: 0x00265E9F
		' (set) Token: 0x060049CB RID: 18891 RVA: 0x00267AA7 File Offset: 0x00265EA7
		Public Property keyboardInputFieldCount As Integer
			Get
				Return Me._keyboardInputFieldCount
			End Get
			Set(value As Integer)
				Me._keyboardInputFieldCount = value
				Me.InspectorPropertyChanged(True)
			End Set
		End Property

		' Token: 0x170006A5 RID: 1701
		' (get) Token: 0x060049CC RID: 18892 RVA: 0x00267AB7 File Offset: 0x00265EB7
		' (set) Token: 0x060049CD RID: 18893 RVA: 0x00267ABF File Offset: 0x00265EBF
		Public Property mouseInputFieldCount As Integer
			Get
				Return Me._mouseInputFieldCount
			End Get
			Set(value As Integer)
				Me._mouseInputFieldCount = value
				Me.InspectorPropertyChanged(True)
			End Set
		End Property

		' Token: 0x170006A6 RID: 1702
		' (get) Token: 0x060049CE RID: 18894 RVA: 0x00267ACF File Offset: 0x00265ECF
		' (set) Token: 0x060049CF RID: 18895 RVA: 0x00267AD7 File Offset: 0x00265ED7
		Public Property controllerInputFieldCount As Integer
			Get
				Return Me._controllerInputFieldCount
			End Get
			Set(value As Integer)
				Me._controllerInputFieldCount = value
				Me.InspectorPropertyChanged(True)
			End Set
		End Property

		' Token: 0x170006A7 RID: 1703
		' (get) Token: 0x060049D0 RID: 18896 RVA: 0x00267AE7 File Offset: 0x00265EE7
		' (set) Token: 0x060049D1 RID: 18897 RVA: 0x00267AEF File Offset: 0x00265EEF
		Public Property showFullAxisInputFields As Boolean
			Get
				Return Me._showFullAxisInputFields
			End Get
			Set(value As Boolean)
				Me._showFullAxisInputFields = value
				Me.InspectorPropertyChanged(True)
			End Set
		End Property

		' Token: 0x170006A8 RID: 1704
		' (get) Token: 0x060049D2 RID: 18898 RVA: 0x00267AFF File Offset: 0x00265EFF
		' (set) Token: 0x060049D3 RID: 18899 RVA: 0x00267B07 File Offset: 0x00265F07
		Public Property showSplitAxisInputFields As Boolean
			Get
				Return Me._showSplitAxisInputFields
			End Get
			Set(value As Boolean)
				Me._showSplitAxisInputFields = value
				Me.InspectorPropertyChanged(True)
			End Set
		End Property

		' Token: 0x170006A9 RID: 1705
		' (get) Token: 0x060049D4 RID: 18900 RVA: 0x00267B17 File Offset: 0x00265F17
		' (set) Token: 0x060049D5 RID: 18901 RVA: 0x00267B1F File Offset: 0x00265F1F
		Public Property allowElementAssignmentConflicts As Boolean
			Get
				Return Me._allowElementAssignmentConflicts
			End Get
			Set(value As Boolean)
				Me._allowElementAssignmentConflicts = value
				Me.InspectorPropertyChanged(False)
			End Set
		End Property

		' Token: 0x170006AA RID: 1706
		' (get) Token: 0x060049D6 RID: 18902 RVA: 0x00267B2F File Offset: 0x00265F2F
		' (set) Token: 0x060049D7 RID: 18903 RVA: 0x00267B37 File Offset: 0x00265F37
		Public Property actionLabelWidth As Integer
			Get
				Return Me._actionLabelWidth
			End Get
			Set(value As Integer)
				Me._actionLabelWidth = value
				Me.InspectorPropertyChanged(True)
			End Set
		End Property

		' Token: 0x170006AB RID: 1707
		' (get) Token: 0x060049D8 RID: 18904 RVA: 0x00267B47 File Offset: 0x00265F47
		' (set) Token: 0x060049D9 RID: 18905 RVA: 0x00267B4F File Offset: 0x00265F4F
		Public Property keyboardColMaxWidth As Integer
			Get
				Return Me._keyboardColMaxWidth
			End Get
			Set(value As Integer)
				Me._keyboardColMaxWidth = value
				Me.InspectorPropertyChanged(True)
			End Set
		End Property

		' Token: 0x170006AC RID: 1708
		' (get) Token: 0x060049DA RID: 18906 RVA: 0x00267B5F File Offset: 0x00265F5F
		' (set) Token: 0x060049DB RID: 18907 RVA: 0x00267B67 File Offset: 0x00265F67
		Public Property mouseColMaxWidth As Integer
			Get
				Return Me._mouseColMaxWidth
			End Get
			Set(value As Integer)
				Me._mouseColMaxWidth = value
				Me.InspectorPropertyChanged(True)
			End Set
		End Property

		' Token: 0x170006AD RID: 1709
		' (get) Token: 0x060049DC RID: 18908 RVA: 0x00267B77 File Offset: 0x00265F77
		' (set) Token: 0x060049DD RID: 18909 RVA: 0x00267B7F File Offset: 0x00265F7F
		Public Property controllerColMaxWidth As Integer
			Get
				Return Me._controllerColMaxWidth
			End Get
			Set(value As Integer)
				Me._controllerColMaxWidth = value
				Me.InspectorPropertyChanged(True)
			End Set
		End Property

		' Token: 0x170006AE RID: 1710
		' (get) Token: 0x060049DE RID: 18910 RVA: 0x00267B8F File Offset: 0x00265F8F
		' (set) Token: 0x060049DF RID: 18911 RVA: 0x00267B97 File Offset: 0x00265F97
		Public Property inputRowHeight As Single
			Get
				Return Me._inputRowHeight
			End Get
			Set(value As Single)
				Me._inputRowHeight = value
				Me.InspectorPropertyChanged(True)
			End Set
		End Property

		' Token: 0x170006AF RID: 1711
		' (get) Token: 0x060049E0 RID: 18912 RVA: 0x00267BA7 File Offset: 0x00265FA7
		' (set) Token: 0x060049E1 RID: 18913 RVA: 0x00267BAF File Offset: 0x00265FAF
		Public Property inputColumnSpacing As Single
			Get
				Return Me._inputColumnSpacing
			End Get
			Set(value As Single)
				Me._inputColumnSpacing = value
				Me.InspectorPropertyChanged(True)
			End Set
		End Property

		' Token: 0x170006B0 RID: 1712
		' (get) Token: 0x060049E2 RID: 18914 RVA: 0x00267BBF File Offset: 0x00265FBF
		' (set) Token: 0x060049E3 RID: 18915 RVA: 0x00267BC7 File Offset: 0x00265FC7
		Public Property inputRowCategorySpacing As Integer
			Get
				Return Me._inputRowCategorySpacing
			End Get
			Set(value As Integer)
				Me._inputRowCategorySpacing = value
				Me.InspectorPropertyChanged(True)
			End Set
		End Property

		' Token: 0x170006B1 RID: 1713
		' (get) Token: 0x060049E4 RID: 18916 RVA: 0x00267BD7 File Offset: 0x00265FD7
		' (set) Token: 0x060049E5 RID: 18917 RVA: 0x00267BDF File Offset: 0x00265FDF
		Public Property invertToggleWidth As Integer
			Get
				Return Me._invertToggleWidth
			End Get
			Set(value As Integer)
				Me._invertToggleWidth = value
				Me.InspectorPropertyChanged(True)
			End Set
		End Property

		' Token: 0x170006B2 RID: 1714
		' (get) Token: 0x060049E6 RID: 18918 RVA: 0x00267BEF File Offset: 0x00265FEF
		' (set) Token: 0x060049E7 RID: 18919 RVA: 0x00267BF7 File Offset: 0x00265FF7
		Public Property defaultWindowWidth As Integer
			Get
				Return Me._defaultWindowWidth
			End Get
			Set(value As Integer)
				Me._defaultWindowWidth = value
				Me.InspectorPropertyChanged(True)
			End Set
		End Property

		' Token: 0x170006B3 RID: 1715
		' (get) Token: 0x060049E8 RID: 18920 RVA: 0x00267C07 File Offset: 0x00266007
		' (set) Token: 0x060049E9 RID: 18921 RVA: 0x00267C0F File Offset: 0x0026600F
		Public Property defaultWindowHeight As Integer
			Get
				Return Me._defaultWindowHeight
			End Get
			Set(value As Integer)
				Me._defaultWindowHeight = value
				Me.InspectorPropertyChanged(True)
			End Set
		End Property

		' Token: 0x170006B4 RID: 1716
		' (get) Token: 0x060049EA RID: 18922 RVA: 0x00267C1F File Offset: 0x0026601F
		' (set) Token: 0x060049EB RID: 18923 RVA: 0x00267C27 File Offset: 0x00266027
		Public Property controllerAssignmentTimeout As Single
			Get
				Return Me._controllerAssignmentTimeout
			End Get
			Set(value As Single)
				Me._controllerAssignmentTimeout = value
				Me.InspectorPropertyChanged(False)
			End Set
		End Property

		' Token: 0x170006B5 RID: 1717
		' (get) Token: 0x060049EC RID: 18924 RVA: 0x00267C37 File Offset: 0x00266037
		' (set) Token: 0x060049ED RID: 18925 RVA: 0x00267C3F File Offset: 0x0026603F
		Public Property preInputAssignmentTimeout As Single
			Get
				Return Me._preInputAssignmentTimeout
			End Get
			Set(value As Single)
				Me._preInputAssignmentTimeout = value
				Me.InspectorPropertyChanged(False)
			End Set
		End Property

		' Token: 0x170006B6 RID: 1718
		' (get) Token: 0x060049EE RID: 18926 RVA: 0x00267C4F File Offset: 0x0026604F
		' (set) Token: 0x060049EF RID: 18927 RVA: 0x00267C57 File Offset: 0x00266057
		Public Property inputAssignmentTimeout As Single
			Get
				Return Me._inputAssignmentTimeout
			End Get
			Set(value As Single)
				Me._inputAssignmentTimeout = value
				Me.InspectorPropertyChanged(False)
			End Set
		End Property

		' Token: 0x170006B7 RID: 1719
		' (get) Token: 0x060049F0 RID: 18928 RVA: 0x00267C67 File Offset: 0x00266067
		' (set) Token: 0x060049F1 RID: 18929 RVA: 0x00267C6F File Offset: 0x0026606F
		Public Property axisCalibrationTimeout As Single
			Get
				Return Me._axisCalibrationTimeout
			End Get
			Set(value As Single)
				Me._axisCalibrationTimeout = value
				Me.InspectorPropertyChanged(False)
			End Set
		End Property

		' Token: 0x170006B8 RID: 1720
		' (get) Token: 0x060049F2 RID: 18930 RVA: 0x00267C7F File Offset: 0x0026607F
		' (set) Token: 0x060049F3 RID: 18931 RVA: 0x00267C87 File Offset: 0x00266087
		Public Property ignoreMouseXAxisAssignment As Boolean
			Get
				Return Me._ignoreMouseXAxisAssignment
			End Get
			Set(value As Boolean)
				Me._ignoreMouseXAxisAssignment = value
				Me.InspectorPropertyChanged(False)
			End Set
		End Property

		' Token: 0x170006B9 RID: 1721
		' (get) Token: 0x060049F4 RID: 18932 RVA: 0x00267C97 File Offset: 0x00266097
		' (set) Token: 0x060049F5 RID: 18933 RVA: 0x00267C9F File Offset: 0x0026609F
		Public Property ignoreMouseYAxisAssignment As Boolean
			Get
				Return Me._ignoreMouseYAxisAssignment
			End Get
			Set(value As Boolean)
				Me._ignoreMouseYAxisAssignment = value
				Me.InspectorPropertyChanged(False)
			End Set
		End Property

		' Token: 0x170006BA RID: 1722
		' (get) Token: 0x060049F6 RID: 18934 RVA: 0x00267CAF File Offset: 0x002660AF
		' (set) Token: 0x060049F7 RID: 18935 RVA: 0x00267CB7 File Offset: 0x002660B7
		Public Property universalCancelClosesScreen As Boolean
			Get
				Return Me._universalCancelClosesScreen
			End Get
			Set(value As Boolean)
				Me._universalCancelClosesScreen = value
				Me.InspectorPropertyChanged(False)
			End Set
		End Property

		' Token: 0x170006BB RID: 1723
		' (get) Token: 0x060049F8 RID: 18936 RVA: 0x00267CC7 File Offset: 0x002660C7
		' (set) Token: 0x060049F9 RID: 18937 RVA: 0x00267CCF File Offset: 0x002660CF
		Public Property showInputBehaviorSettings As Boolean
			Get
				Return Me._showInputBehaviorSettings
			End Get
			Set(value As Boolean)
				Me._showInputBehaviorSettings = value
				Me.InspectorPropertyChanged(True)
			End Set
		End Property

		' Token: 0x170006BC RID: 1724
		' (get) Token: 0x060049FA RID: 18938 RVA: 0x00267CDF File Offset: 0x002660DF
		' (set) Token: 0x060049FB RID: 18939 RVA: 0x00267CE7 File Offset: 0x002660E7
		Public Property useThemeSettings As Boolean
			Get
				Return Me._useThemeSettings
			End Get
			Set(value As Boolean)
				Me._useThemeSettings = value
				Me.InspectorPropertyChanged(True)
			End Set
		End Property

		' Token: 0x170006BD RID: 1725
		' (get) Token: 0x060049FC RID: 18940 RVA: 0x00267CF7 File Offset: 0x002660F7
		' (set) Token: 0x060049FD RID: 18941 RVA: 0x00267CFF File Offset: 0x002660FF
		Public Property language As LanguageData
			Get
				Return Me._language
			End Get
			Set(value As LanguageData)
				Me._language = value
				If Me._language IsNot Nothing Then
					Me._language.Initialize()
				End If
				Me.InspectorPropertyChanged(True)
			End Set
		End Property

		' Token: 0x170006BE RID: 1726
		' (get) Token: 0x060049FE RID: 18942 RVA: 0x00267D2B File Offset: 0x0026612B
		' (set) Token: 0x060049FF RID: 18943 RVA: 0x00267D33 File Offset: 0x00266133
		Public Property showPlayersGroupLabel As Boolean
			Get
				Return Me._showPlayersGroupLabel
			End Get
			Set(value As Boolean)
				Me._showPlayersGroupLabel = value
				Me.InspectorPropertyChanged(True)
			End Set
		End Property

		' Token: 0x170006BF RID: 1727
		' (get) Token: 0x06004A00 RID: 18944 RVA: 0x00267D43 File Offset: 0x00266143
		' (set) Token: 0x06004A01 RID: 18945 RVA: 0x00267D4B File Offset: 0x0026614B
		Public Property showControllerGroupLabel As Boolean
			Get
				Return Me._showControllerGroupLabel
			End Get
			Set(value As Boolean)
				Me._showControllerGroupLabel = value
				Me.InspectorPropertyChanged(True)
			End Set
		End Property

		' Token: 0x170006C0 RID: 1728
		' (get) Token: 0x06004A02 RID: 18946 RVA: 0x00267D5B File Offset: 0x0026615B
		' (set) Token: 0x06004A03 RID: 18947 RVA: 0x00267D63 File Offset: 0x00266163
		Public Property showAssignedControllersGroupLabel As Boolean
			Get
				Return Me._showAssignedControllersGroupLabel
			End Get
			Set(value As Boolean)
				Me._showAssignedControllersGroupLabel = value
				Me.InspectorPropertyChanged(True)
			End Set
		End Property

		' Token: 0x170006C1 RID: 1729
		' (get) Token: 0x06004A04 RID: 18948 RVA: 0x00267D73 File Offset: 0x00266173
		' (set) Token: 0x06004A05 RID: 18949 RVA: 0x00267D7B File Offset: 0x0026617B
		Public Property showSettingsGroupLabel As Boolean
			Get
				Return Me._showSettingsGroupLabel
			End Get
			Set(value As Boolean)
				Me._showSettingsGroupLabel = value
				Me.InspectorPropertyChanged(True)
			End Set
		End Property

		' Token: 0x170006C2 RID: 1730
		' (get) Token: 0x06004A06 RID: 18950 RVA: 0x00267D8B File Offset: 0x0026618B
		' (set) Token: 0x06004A07 RID: 18951 RVA: 0x00267D93 File Offset: 0x00266193
		Public Property showMapCategoriesGroupLabel As Boolean
			Get
				Return Me._showMapCategoriesGroupLabel
			End Get
			Set(value As Boolean)
				Me._showMapCategoriesGroupLabel = value
				Me.InspectorPropertyChanged(True)
			End Set
		End Property

		' Token: 0x170006C3 RID: 1731
		' (get) Token: 0x06004A08 RID: 18952 RVA: 0x00267DA3 File Offset: 0x002661A3
		' (set) Token: 0x06004A09 RID: 18953 RVA: 0x00267DAB File Offset: 0x002661AB
		Public Property showControllerNameLabel As Boolean
			Get
				Return Me._showControllerNameLabel
			End Get
			Set(value As Boolean)
				Me._showControllerNameLabel = value
				Me.InspectorPropertyChanged(True)
			End Set
		End Property

		' Token: 0x170006C4 RID: 1732
		' (get) Token: 0x06004A0A RID: 18954 RVA: 0x00267DBB File Offset: 0x002661BB
		' (set) Token: 0x06004A0B RID: 18955 RVA: 0x00267DC3 File Offset: 0x002661C3
		Public Property showAssignedControllers As Boolean
			Get
				Return Me._showAssignedControllers
			End Get
			Set(value As Boolean)
				Me._showAssignedControllers = value
				Me.InspectorPropertyChanged(True)
			End Set
		End Property

		' Token: 0x170006C5 RID: 1733
		' (get) Token: 0x06004A0C RID: 18956 RVA: 0x00267DD3 File Offset: 0x002661D3
		' (set) Token: 0x06004A0D RID: 18957 RVA: 0x00267DDB File Offset: 0x002661DB
		Public Property showControllerGroupButtons As Boolean

		' Token: 0x170006C6 RID: 1734
		' (get) Token: 0x06004A0E RID: 18958 RVA: 0x00267DE4 File Offset: 0x002661E4
		' (set) Token: 0x06004A0F RID: 18959 RVA: 0x00267DEC File Offset: 0x002661EC
		Public Property restoreDefaultsDelegate As Action
			Get
				Return Me._restoreDefaultsDelegate
			End Get
			Set(value As Action)
				Me._restoreDefaultsDelegate = value
			End Set
		End Property

		' Token: 0x170006C7 RID: 1735
		' (get) Token: 0x06004A10 RID: 18960 RVA: 0x00267DF8 File Offset: 0x002661F8
		Public ReadOnly Property isOpen As Boolean
			Get
				If Not Me.initialized Then
					Return Me.references.canvas IsNot Nothing AndAlso Me.references.canvas.gameObject.activeInHierarchy
				End If
				Return Me.canvas.activeInHierarchy
			End Get
		End Property

		' Token: 0x170006C8 RID: 1736
		' (get) Token: 0x06004A11 RID: 18961 RVA: 0x00267E4D File Offset: 0x0026624D
		Private ReadOnly Property isFocused As Boolean
			Get
				Return Me.initialized AndAlso Not Me.windowManager.isWindowOpen
			End Get
		End Property

		' Token: 0x170006C9 RID: 1737
		' (get) Token: 0x06004A12 RID: 18962 RVA: 0x00267E6A File Offset: 0x0026626A
		Private ReadOnly Property inputAllowed As Boolean
			Get
				Return Me.blockInputOnFocusEndTime <= Time.unscaledTime AndAlso Not InterruptingPrompt.IsInterrupting()
			End Get
		End Property

		' Token: 0x170006CA RID: 1738
		' (get) Token: 0x06004A13 RID: 18963 RVA: 0x00267E8C File Offset: 0x0026628C
		Private ReadOnly Property inputGridColumnCount As Integer
			Get
				Dim num As Integer = 1
				If Me._showKeyboard Then
					num += 1
				End If
				If Me._showMouse Then
					num += 1
				End If
				If Me._showControllers Then
					num += 1
				End If
				Return num
			End Get
		End Property

		' Token: 0x170006CB RID: 1739
		' (get) Token: 0x06004A14 RID: 18964 RVA: 0x00267ECC File Offset: 0x002662CC
		Private ReadOnly Property inputGridWidth As Integer
			Get
				Return Me._actionLabelWidth + If((Not Me._showKeyboard), 0, Me._keyboardColMaxWidth) + If((Not Me._showMouse), 0, Me._mouseColMaxWidth) + If((Not Me._showControllers), 0, Me._controllerColMaxWidth) + CInt((CSng((Me.inputGridColumnCount - 1)) * Me._inputColumnSpacing))
			End Get
		End Property

		' Token: 0x170006CC RID: 1740
		' (get) Token: 0x06004A15 RID: 18965 RVA: 0x00267F39 File Offset: 0x00266339
		Private ReadOnly Property currentPlayer As Player
			Get
				Return ReInput.players.GetPlayer(Me.currentPlayerId)
			End Get
		End Property

		' Token: 0x170006CD RID: 1741
		' (get) Token: 0x06004A16 RID: 18966 RVA: 0x00267F4B File Offset: 0x0026634B
		Private ReadOnly Property currentMapCategory As InputCategory
			Get
				Return ReInput.mapping.GetMapCategory(Me.currentMapCategoryId)
			End Get
		End Property

		' Token: 0x170006CE RID: 1742
		' (get) Token: 0x06004A17 RID: 18967 RVA: 0x00267F60 File Offset: 0x00266360
		Private ReadOnly Property currentMappingSet As ControlMapper.MappingSet
			Get
				If Me.currentMapCategoryId < 0 Then
					Return Nothing
				End If
				For i As Integer = 0 To Me._mappingSets.Length - 1
					If Me._mappingSets(i).mapCategoryId = Me.currentMapCategoryId Then
						Return Me._mappingSets(i)
					End If
				Next
				Return Nothing
			End Get
		End Property

		' Token: 0x170006CF RID: 1743
		' (get) Token: 0x06004A18 RID: 18968 RVA: 0x00267FB6 File Offset: 0x002663B6
		Private ReadOnly Property currentJoystick As Joystick
			Get
				Return ReInput.controllers.GetJoystick(Me.currentJoystickId)
			End Get
		End Property

		' Token: 0x170006D0 RID: 1744
		' (get) Token: 0x06004A19 RID: 18969 RVA: 0x00267FC8 File Offset: 0x002663C8
		Private ReadOnly Property isJoystickSelected As Boolean
			Get
				Return Me.currentJoystickId >= 0
			End Get
		End Property

		' Token: 0x170006D1 RID: 1745
		' (get) Token: 0x06004A1A RID: 18970 RVA: 0x00267FD6 File Offset: 0x002663D6
		Private ReadOnly Property currentUISelection As GameObject
			Get
				Return If((Not(EventSystem.current IsNot Nothing)), Nothing, EventSystem.current.currentSelectedGameObject)
			End Get
		End Property

		' Token: 0x170006D2 RID: 1746
		' (get) Token: 0x06004A1B RID: 18971 RVA: 0x00267FF8 File Offset: 0x002663F8
		Private ReadOnly Property showSettings As Boolean
			Get
				Return Me._showInputBehaviorSettings AndAlso Me._inputBehaviorSettings.Length > 0
			End Get
		End Property

		' Token: 0x170006D3 RID: 1747
		' (get) Token: 0x06004A1C RID: 18972 RVA: 0x00268013 File Offset: 0x00266413
		Private ReadOnly Property showMapCategories As Boolean
			Get
				Return Me._mappingSets IsNot Nothing AndAlso Me._mappingSets.Length > 1
			End Get
		End Property

		' Token: 0x06004A1D RID: 18973 RVA: 0x00268033 File Offset: 0x00266433
		Private Sub Awake()
			If Me._dontDestroyOnLoad Then
				Global.UnityEngine.[Object].DontDestroyOnLoad(MyBase.transform.gameObject)
			End If
			Me.PreInitialize()
			If Me.isOpen Then
				Me.Initialize()
				Me.Open(True)
			End If
		End Sub

		' Token: 0x06004A1E RID: 18974 RVA: 0x0026806E File Offset: 0x0026646E
		Private Sub Start()
			If Me._openOnStart Then
				Me.Open(False)
			End If
		End Sub

		' Token: 0x06004A1F RID: 18975 RVA: 0x00268082 File Offset: 0x00266482
		Private Sub Update()
			If Not Me.isOpen Then
				Return
			End If
			If Not Me.initialized Then
				Return
			End If
			Me.CheckUISelection()
		End Sub

		' Token: 0x06004A20 RID: 18976 RVA: 0x002680A2 File Offset: 0x002664A2
		Private Sub OnDestroy()
			RemoveHandler ReInput.ControllerConnectedEvent, AddressOf Me.OnJoystickConnected
			RemoveHandler ReInput.ControllerDisconnectedEvent, AddressOf Me.OnJoystickDisconnected
			RemoveHandler ReInput.ControllerPreDisconnectEvent, AddressOf Me.OnJoystickPreDisconnect
			Me.UnsubscribeMenuControlInputEvents()
		End Sub

		' Token: 0x06004A21 RID: 18977 RVA: 0x002680DD File Offset: 0x002664DD
		Private Sub PreInitialize()
			If Not ReInput.isReady Then
				Global.UnityEngine.Debug.LogError("Rewired Control Mapper: Rewired has not been initialized! Are you missing a Rewired Input Manager in your scene?")
				Return
			End If
			Me.SubscribeMenuControlInputEvents()
		End Sub

		' Token: 0x06004A22 RID: 18978 RVA: 0x002680FC File Offset: 0x002664FC
		Private Sub Initialize()
			If Me.initialized Then
				Return
			End If
			If Not ReInput.isReady Then
				Return
			End If
			Me.currentPlayerId = Mathf.Clamp(Me.currentPlayerId, 0, 1)
			If Me._rewiredInputManager Is Nothing Then
				Me._rewiredInputManager = Global.UnityEngine.[Object].FindObjectOfType(Of InputManager)()
				If Me._rewiredInputManager Is Nothing Then
					Global.UnityEngine.Debug.LogError("Rewired Control Mapper: A Rewired Input Manager was not assigned in the inspector or found in the current scene! Control Mapper will not function.")
					Return
				End If
			End If
			If ControlMapper.Instance IsNot Nothing Then
				Global.UnityEngine.Debug.LogError("Rewired Control Mapper: Only one ControlMapper can exist at one time!")
				Return
			End If
			ControlMapper.Instance = Me
			If Me.prefabs Is Nothing OrElse Not Me.prefabs.Check() Then
				Global.UnityEngine.Debug.LogError("Rewired Control Mapper: All prefabs must be assigned in the inspector!")
				Return
			End If
			If Me.references Is Nothing OrElse Not Me.references.Check() Then
				Global.UnityEngine.Debug.LogError("Rewired Control Mapper: All references must be assigned in the inspector!")
				Return
			End If
			Me.references.inputGridLayoutElement = Me.references.inputGridContainer.GetComponent(Of LayoutElement)()
			If Me.references.inputGridLayoutElement Is Nothing Then
				Global.UnityEngine.Debug.LogError("Rewired Control Mapper: InputGridContainer is missing LayoutElement component!")
				Return
			End If
			If Me._showKeyboard AndAlso Me._keyboardInputFieldCount < 1 Then
				Global.UnityEngine.Debug.LogWarning("Rewired Control Mapper: Keyboard Input Fields must be at least 1!")
				Me._keyboardInputFieldCount = 1
			End If
			If Me._showMouse AndAlso Me._mouseInputFieldCount < 1 Then
				Global.UnityEngine.Debug.LogWarning("Rewired Control Mapper: Mouse Input Fields must be at least 1!")
				Me._mouseInputFieldCount = 1
			End If
			If Me._showControllers AndAlso Me._controllerInputFieldCount < 1 Then
				Global.UnityEngine.Debug.LogWarning("Rewired Control Mapper: Controller Input Fields must be at least 1!")
				Me._controllerInputFieldCount = 1
			End If
			If Me._maxControllersPerPlayer < 0 Then
				Global.UnityEngine.Debug.LogWarning("Rewired Control Mapper: Max Controllers Per Player must be at least 0 (no limit)!")
				Me._maxControllersPerPlayer = 0
			End If
			If Me._useThemeSettings AndAlso Me._themeSettings Is Nothing Then
				Global.UnityEngine.Debug.LogWarning("Rewired Control Mapper: To use theming, Theme Settings must be set in the inspector! Theming has been disabled.")
				Me._useThemeSettings = False
			End If
			If Me._language Is Nothing Then
				Global.UnityEngine.Debug.LogError("Rawired UI: Language must be set in the inspector!")
				Return
			End If
			Me._language.Initialize()
			Me.inputFieldActivatedDelegate = AddressOf Me.OnInputFieldActivated
			Me.inputFieldInvertToggleStateChangedDelegate = AddressOf Me.OnInputFieldInvertToggleStateChanged
			AddHandler ReInput.ControllerConnectedEvent, AddressOf Me.OnJoystickConnected
			AddHandler ReInput.ControllerDisconnectedEvent, AddressOf Me.OnJoystickDisconnected
			AddHandler ReInput.ControllerPreDisconnectEvent, AddressOf Me.OnJoystickPreDisconnect
			AddHandler PlayerManager.OnControlsChanged, AddressOf Me.OnControlsChanged
			Me.playerCount = ReInput.players.playerCount
			Me.canvas = Me.references.canvas.gameObject
			Me.windowManager = New ControlMapper.WindowManager(Me.prefabs.window, Me.prefabs.fader, Me.references.canvas.transform)
			Me.playerButtons = New List(Of ControlMapper.GUIButton)()
			Me.mapCategoryButtons = New List(Of ControlMapper.GUIButton)()
			Me.assignedControllerButtons = New List(Of ControlMapper.GUIButton)()
			Me.miscInstantiatedObjects = New List(Of GameObject)()
			Me.currentMapCategoryId = Me._mappingSets(0).mapCategoryId
			Me.Draw()
			Me.CreateInputGrid()
			Me.CreateLayout()
			Me.SubscribeFixedUISelectionEvents()
			Me.initialized = True
		End Sub

		' Token: 0x06004A23 RID: 18979 RVA: 0x00268414 File Offset: 0x00266814
		Private Sub OnJoystickConnected(args As ControllerStatusChangedEventArgs)
			If Not Me.initialized Then
				Return
			End If
			If Not Me._showControllers Then
				Return
			End If
			Me.ClearVarsOnJoystickChange()
			Me.ForceRefresh()
		End Sub

		' Token: 0x06004A24 RID: 18980 RVA: 0x0026843A File Offset: 0x0026683A
		Private Sub OnJoystickDisconnected(args As ControllerStatusChangedEventArgs)
			If Not Me.initialized Then
				Return
			End If
			If Not Me._showControllers Then
				Return
			End If
			Me.ClearVarsOnJoystickChange()
			Me.ForceRefresh()
		End Sub

		' Token: 0x06004A25 RID: 18981 RVA: 0x00268460 File Offset: 0x00266860
		Private Sub OnJoystickPreDisconnect(args As ControllerStatusChangedEventArgs)
			If Not Me.initialized Then
				Return
			End If
			If Not Me._showControllers Then
				Return
			End If
		End Sub

		' Token: 0x06004A26 RID: 18982 RVA: 0x0026847C File Offset: 0x0026687C
		Public Sub OnButtonActivated(buttonInfo As ButtonInfo)
			If Not Me.initialized Then
				Return
			End If
			If Not Me.inputAllowed Then
				Return
			End If
			AudioManager.Play("level_menu_select")
			Dim identifier As String = buttonInfo.identifier
			Select Case identifier
				Case "PlayerSelection"
					Me.OnPlayerSelected(buttonInfo.intData, True)
				Case "AssignedControllerSelection"
					Me.OnControllerSelected(buttonInfo.intData)
				Case "RemoveController"
					Me.OnRemoveCurrentController()
				Case "AssignController"
					Me.ShowAssignControllerWindow()
				Case "CalibrateController"
					Me.ShowCalibrateControllerWindow()
				Case "EditInputBehaviors"
					Me.ShowEditInputBehaviorsWindow()
				Case "MapCategorySelection"
					Me.OnMapCategorySelected(buttonInfo.intData, True)
				Case "Done"
					Me.Close(True)
				Case "RestoreDefaults"
					Me.OnRestoreDefaults()
				Case "ToggleRumble"
					Me.ToggleRumble()
			End Select
		End Sub

		' Token: 0x06004A27 RID: 18983 RVA: 0x00268611 File Offset: 0x00266A11
		Private Sub ToggleRumble()
			SettingsData.Data.canVibrate = Not SettingsData.Data.canVibrate
			SettingsData.Save()
			Me.UpdateRumbleText()
		End Sub

		' Token: 0x06004A28 RID: 18984 RVA: 0x00268638 File Offset: 0x00266A38
		Private Sub UpdateRumbleText()
			If Me._rumbleButtonText IsNot Nothing Then
				Me._rumbleButtonText.text = Localization.Translate(If((Not SettingsData.Data.canVibrate), "ToggleRumbleOff", "ToggleRumbleOn")).text
			End If
		End Sub

		' Token: 0x06004A29 RID: 18985 RVA: 0x0026868C File Offset: 0x00266A8C
		Private Sub OnEnable()
			Me.UpdateRumbleText()
		End Sub

		' Token: 0x06004A2A RID: 18986 RVA: 0x00268694 File Offset: 0x00266A94
		Public Sub OnInputFieldActivated(fieldInfo As InputFieldInfo)
			If Not Me.initialized Then
				Return
			End If
			If Not Me.inputAllowed Then
				Return
			End If
			AudioManager.Play("level_menu_select")
			If Me.currentPlayer Is Nothing Then
				Return
			End If
			Dim action As InputAction = ReInput.mapping.GetAction(fieldInfo.actionId)
			If action Is Nothing Then
				Return
			End If
			Dim text As String
			If action.type = InputActionType.Button Then
				text = action.descriptiveName
			Else
				If action.type <> InputActionType.Axis Then
					Throw New NotImplementedException()
				End If
				If fieldInfo.axisRange = AxisRange.Full Then
					text = action.descriptiveName
				ElseIf fieldInfo.axisRange = AxisRange.Positive Then
					If String.IsNullOrEmpty(action.positiveDescriptiveName) Then
						text = action.descriptiveName + " +"
					Else
						text = action.positiveDescriptiveName
					End If
				Else
					If fieldInfo.axisRange <> AxisRange.Negative Then
						Throw New NotImplementedException()
					End If
					If String.IsNullOrEmpty(action.negativeDescriptiveName) Then
						text = action.descriptiveName + " -"
					Else
						text = action.negativeDescriptiveName
					End If
				End If
			End If
			text = Localization.Translate(text).text
			Dim controllerMap As ControllerMap = Me.GetControllerMap(fieldInfo.controllerType)
			If controllerMap Is Nothing Then
				Return
			End If
			Dim actionElementMap As ActionElementMap = If((fieldInfo.actionElementMapId < 0), Nothing, controllerMap.GetElementMap(fieldInfo.actionElementMapId))
			If actionElementMap IsNot Nothing Then
				Me.ShowBeginElementAssignmentReplacementWindow(fieldInfo, action, controllerMap, actionElementMap, text)
			Else
				Me.ShowCreateNewElementAssignmentWindow(fieldInfo, action, controllerMap, text)
			End If
		End Sub

		' Token: 0x06004A2B RID: 18987 RVA: 0x00268813 File Offset: 0x00266C13
		Public Sub OnInputFieldInvertToggleStateChanged(toggleInfo As ToggleInfo, newState As Boolean)
			If Not Me.initialized Then
				Return
			End If
			If Not Me.inputAllowed Then
				Return
			End If
			AudioManager.Play("level_menu_select")
			Me.SetActionAxisInverted(newState, toggleInfo.controllerType, toggleInfo.actionElementMapId)
		End Sub

		' Token: 0x140000F0 RID: 240
		' (add) Token: 0x06004A2C RID: 18988 RVA: 0x0026884C File Offset: 0x00266C4C
		' (remove) Token: 0x06004A2D RID: 18989 RVA: 0x00268880 File Offset: 0x00266C80
		<DebuggerBrowsable(DebuggerBrowsableState.Never)>
		Public Shared Event OnPlayerChange As ControlMapper.PlayerChangeAction

		' Token: 0x06004A2E RID: 18990 RVA: 0x002688B4 File Offset: 0x00266CB4
		Private Sub OnPlayerSelected(playerId As Integer, redraw As Boolean)
			If Not Me.initialized Then
				Return
			End If
			Me.currentPlayerId = playerId
			Me.ClearVarsOnPlayerChange()
			Me.Redraw(True, True)
			For i As Integer = 0 To Me.axisToggleObjects.Count - 1
				Me.axisToggleObjects(i).SetActive(Me.currentPlayer.controllers.joystickCount > 0)
			Next
			For j As Integer = 0 To Me.inactiveAxisToggleObjects.Count - 1
				Me.inactiveAxisToggleObjects(j).SetActive(Me.currentPlayer.controllers.joystickCount = 0)
			Next
			If ControlMapper.OnPlayerChange IsNot Nothing Then
				ControlMapper.OnPlayerChange()
			End If
		End Sub

		' Token: 0x06004A2F RID: 18991 RVA: 0x00268976 File Offset: 0x00266D76
		Private Sub OnControllerSelected(joystickId As Integer)
			If Not Me.initialized Then
				Return
			End If
			Me.currentJoystickId = joystickId
			Me.Redraw(True, True)
		End Sub

		' Token: 0x06004A30 RID: 18992 RVA: 0x00268993 File Offset: 0x00266D93
		Private Sub OnRemoveCurrentController()
			If Me.currentPlayer Is Nothing Then
				Return
			End If
			If Me.currentJoystickId < 0 Then
				Return
			End If
			Me.RemoveController(Me.currentPlayer, Me.currentJoystickId)
			Me.ClearVarsOnJoystickChange()
			Me.Redraw(False, False)
		End Sub

		' Token: 0x06004A31 RID: 18993 RVA: 0x002689CE File Offset: 0x00266DCE
		Private Sub OnMapCategorySelected(id As Integer, redraw As Boolean)
			If Not Me.initialized Then
				Return
			End If
			Me.currentMapCategoryId = id
			If redraw Then
				Me.Redraw(True, True)
			End If
		End Sub

		' Token: 0x06004A32 RID: 18994 RVA: 0x002689F1 File Offset: 0x00266DF1
		Private Sub OnRestoreDefaults()
			If Not Me.initialized Then
				Return
			End If
			Me.ShowRestoreDefaultsWindow()
		End Sub

		' Token: 0x06004A33 RID: 18995 RVA: 0x00268A05 File Offset: 0x00266E05
		Private Sub OnScreenToggleActionPressed(data As InputActionEventData)
			If Not Me.isOpen Then
				Me.Open()
				Return
			End If
			If Not Me.initialized Then
				Return
			End If
			If Not Me.isFocused Then
				Return
			End If
			Me.Close(True)
		End Sub

		' Token: 0x06004A34 RID: 18996 RVA: 0x00268A38 File Offset: 0x00266E38
		Private Sub OnScreenOpenActionPressed(data As InputActionEventData)
			Me.Open()
		End Sub

		' Token: 0x06004A35 RID: 18997 RVA: 0x00268A40 File Offset: 0x00266E40
		Private Sub OnScreenCloseActionPressed(data As InputActionEventData)
			If Not Me.initialized Then
				Return
			End If
			If Not Me.isOpen Then
				Return
			End If
			If Not Me.isFocused Then
				Return
			End If
			Me.Close(True)
		End Sub

		' Token: 0x06004A36 RID: 18998 RVA: 0x00268A70 File Offset: 0x00266E70
		Private Sub OnUniversalCancelActionPressed(data As InputActionEventData)
			If Not Me.initialized Then
				Return
			End If
			If Not Me.isOpen Then
				Return
			End If
			If Me._universalCancelClosesScreen Then
				If Me.isFocused Then
					Me.Close(True)
					Return
				End If
			ElseIf Me.isFocused Then
				Return
			End If
			If Me.isPollingForInput Then
				Return
			End If
			Me.CloseAllWindows()
		End Sub

		' Token: 0x06004A37 RID: 18999 RVA: 0x00268AD6 File Offset: 0x00266ED6
		Private Sub OnWindowCancel(windowId As Integer)
			If Not Me.initialized Then
				Return
			End If
			If windowId < 0 Then
				Return
			End If
			Me.CloseWindow(windowId)
		End Sub

		' Token: 0x06004A38 RID: 19000 RVA: 0x00268AF3 File Offset: 0x00266EF3
		Private Sub OnRemoveElementAssignment(windowId As Integer, map As ControllerMap, aem As ActionElementMap)
			If map Is Nothing OrElse aem Is Nothing Then
				Return
			End If
			map.DeleteElementMap(aem.id)
			Me.CloseWindow(windowId)
		End Sub

		' Token: 0x06004A39 RID: 19001 RVA: 0x00268B18 File Offset: 0x00266F18
		Private Sub OnBeginElementAssignment(fieldInfo As InputFieldInfo, map As ControllerMap, aem As ActionElementMap, actionName As String)
			If fieldInfo Is Nothing OrElse map Is Nothing Then
				Return
			End If
			Me.pendingInputMapping = New ControlMapper.InputMapping(actionName, fieldInfo, map, aem, fieldInfo.controllerType, fieldInfo.controllerId)
			Select Case fieldInfo.controllerType
				Case ControllerType.Keyboard
					Me.ShowElementAssignmentPollingWindow()
				Case ControllerType.Mouse
					Me.ShowElementAssignmentPollingWindow()
				Case ControllerType.Joystick
					Me.ShowElementAssignmentPrePollingWindow()
				Case Else
					Throw New NotImplementedException()
			End Select
		End Sub

		' Token: 0x06004A3A RID: 19002 RVA: 0x00268B99 File Offset: 0x00266F99
		Private Sub OnControllerAssignmentConfirmed(windowId As Integer, player As Player, controllerId As Integer)
			If windowId < 0 OrElse player Is Nothing OrElse controllerId < 0 Then
				Return
			End If
			Me.AssignController(player, controllerId)
			Me.CloseWindow(windowId)
		End Sub

		' Token: 0x06004A3B RID: 19003 RVA: 0x00268BC0 File Offset: 0x00266FC0
		Private Sub OnMouseAssignmentConfirmed(windowId As Integer, player As Player)
			If windowId < 0 OrElse player Is Nothing Then
				Return
			End If
			Dim players As IList(Of Player) = ReInput.players.Players
			For i As Integer = 0 To players.Count - 1
				If players(i) IsNot player Then
					players(i).controllers.hasMouse = False
				End If
			Next
			player.controllers.hasMouse = True
			Me.CloseWindow(windowId)
		End Sub

		' Token: 0x06004A3C RID: 19004 RVA: 0x00268C34 File Offset: 0x00267034
		Private Sub OnElementAssignmentConflictReplaceConfirmed(windowId As Integer, mapping As ControlMapper.InputMapping, assignment As ElementAssignment, skipOtherPlayers As Boolean)
			If Me.currentPlayer Is Nothing OrElse mapping Is Nothing Then
				Return
			End If
			Dim elementAssignmentConflictCheck As ElementAssignmentConflictCheck
			If Not Me.CreateConflictCheck(mapping, assignment, elementAssignmentConflictCheck) Then
				Global.UnityEngine.Debug.LogError("Rewired Control Mapper: Error creating conflict check!")
				Me.CloseWindow(windowId)
				Return
			End If
			If skipOtherPlayers Then
				ReInput.players.SystemPlayer.controllers.conflictChecking.RemoveElementAssignmentConflicts(elementAssignmentConflictCheck)
				Me.currentPlayer.controllers.conflictChecking.RemoveElementAssignmentConflicts(elementAssignmentConflictCheck)
			Else
				ReInput.controllers.conflictChecking.RemoveElementAssignmentConflicts(elementAssignmentConflictCheck)
			End If
			mapping.map.ReplaceOrCreateElementMap(assignment)
			Me.CloseWindow(windowId)
		End Sub

		' Token: 0x06004A3D RID: 19005 RVA: 0x00268CD7 File Offset: 0x002670D7
		Private Sub OnElementAssignmentAddConfirmed(windowId As Integer, mapping As ControlMapper.InputMapping, assignment As ElementAssignment)
			If Me.currentPlayer Is Nothing OrElse mapping Is Nothing Then
				Return
			End If
			mapping.map.ReplaceOrCreateElementMap(assignment)
			Me.CloseWindow(windowId)
		End Sub

		' Token: 0x06004A3E RID: 19006 RVA: 0x00268D00 File Offset: 0x00267100
		Private Sub OnRestoreDefaultsConfirmed(windowId As Integer)
			If Me._restoreDefaultsDelegate Is Nothing Then
				Dim players As IList(Of Player) = ReInput.players.Players
				For i As Integer = 0 To players.Count - 1
					Dim player As Player = players(i)
					If Me._showControllers Then
						player.controllers.maps.LoadDefaultMaps(ControllerType.Joystick)
					End If
					If Me._showKeyboard Then
						player.controllers.maps.LoadDefaultMaps(ControllerType.Keyboard)
					End If
					If Me._showMouse Then
						player.controllers.maps.LoadDefaultMaps(ControllerType.Mouse)
					End If
				Next
			End If
			Me.CloseWindow(windowId)
			If Me._restoreDefaultsDelegate IsNot Nothing Then
				Me._restoreDefaultsDelegate()
			End If
		End Sub

		' Token: 0x06004A3F RID: 19007 RVA: 0x00268DB4 File Offset: 0x002671B4
		Private Sub OnAssignControllerWindowUpdate(windowId As Integer)
			If Me.currentPlayer Is Nothing Then
				Return
			End If
			Dim window As Window = Me.windowManager.GetWindow(windowId)
			If windowId < 0 Then
				Return
			End If
			Me.InputPollingStarted()
			If window.timer.finished Then
				Me.InputPollingStopped()
				Me.CloseWindow(windowId)
				Return
			End If
			Dim controllerPollingInfo As ControllerPollingInfo = ReInput.controllers.polling.PollAllControllersOfTypeForFirstElementDown(ControllerType.Joystick)
			If Not controllerPollingInfo.success Then
				window.SetContentText(Mathf.CeilToInt(window.timer.remaining).ToString(), 1)
				Return
			End If
			Me.InputPollingStopped()
			If ReInput.controllers.IsControllerAssigned(ControllerType.Joystick, controllerPollingInfo.controllerId) AndAlso Not Me.currentPlayer.controllers.ContainsController(ControllerType.Joystick, controllerPollingInfo.controllerId) Then
				Return
			End If
			Me.OnControllerAssignmentConfirmed(windowId, Me.currentPlayer, controllerPollingInfo.controllerId)
		End Sub

		' Token: 0x06004A40 RID: 19008 RVA: 0x00268E98 File Offset: 0x00267298
		Private Sub OnElementAssignmentPrePollingWindowUpdate(windowId As Integer)
			If Me.currentPlayer Is Nothing Then
				Return
			End If
			Dim window As Window = Me.windowManager.GetWindow(windowId)
			If windowId < 0 Then
				Return
			End If
			If Me.pendingInputMapping Is Nothing Then
				Return
			End If
			Me.InputPollingStarted()
			If Not window.timer.finished Then
				window.SetContentText(Mathf.CeilToInt(window.timer.remaining).ToString(), 1)
				Dim controllerPollingInfo As ControllerPollingInfo
				Select Case Me.pendingInputMapping.controllerType
					Case ControllerType.Keyboard, ControllerType.Mouse
						controllerPollingInfo = ReInput.controllers.polling.PollControllerForFirstButtonDown(Me.pendingInputMapping.controllerType, 0)
					Case ControllerType.Joystick
						If Me.currentPlayer.controllers.joystickCount = 0 Then
							Return
						End If
						controllerPollingInfo = ReInput.controllers.polling.PollControllerForFirstButtonDown(Me.pendingInputMapping.controllerType, Me.currentJoystick.id)
					Case Else
						Throw New NotImplementedException()
				End Select
				If Not controllerPollingInfo.success Then
					Return
				End If
			End If
			Me.ShowElementAssignmentPollingWindow()
		End Sub

		' Token: 0x06004A41 RID: 19009 RVA: 0x00268FB0 File Offset: 0x002673B0
		Private Sub OnJoystickElementAssignmentPollingWindowUpdate(windowId As Integer)
			If Me.currentPlayer Is Nothing Then
				Return
			End If
			Dim window As Window = Me.windowManager.GetWindow(windowId)
			If windowId < 0 Then
				Return
			End If
			If Me.pendingInputMapping Is Nothing Then
				Return
			End If
			Me.InputPollingStarted()
			If window.timer.finished Then
				Me.InputPollingStopped()
				Me.CloseWindow(windowId)
				Return
			End If
			window.SetContentText(Mathf.CeilToInt(window.timer.remaining).ToString(), 1)
			If Me.currentPlayer.controllers.joystickCount = 0 Then
				Return
			End If
			Dim controllerPollingInfo As ControllerPollingInfo = ReInput.controllers.polling.PollControllerForFirstElementDown(ControllerType.Joystick, Me.currentJoystick.id)
			If Not controllerPollingInfo.success Then
				Return
			End If
			If Not Me.IsAllowedAssignment(Me.pendingInputMapping, controllerPollingInfo) Then
				Return
			End If
			Dim elementAssignment As ElementAssignment = Me.pendingInputMapping.ToElementAssignment(controllerPollingInfo)
			If controllerPollingInfo.elementIdentifierName.Contains("Trigger") AndAlso elementAssignment.axisRange = AxisRange.Negative Then
				Return
			End If
			If Not Me.HasElementAssignmentConflicts(Me.currentPlayer, Me.pendingInputMapping, elementAssignment, False) Then
				Me.pendingInputMapping.map.ReplaceOrCreateElementMap(elementAssignment)
				Me.InputPollingStopped()
				Me.CloseWindow(windowId)
			Else
				Me.InputPollingStopped()
				Me.ShowElementAssignmentConflictWindow(elementAssignment, False)
			End If
		End Sub

		' Token: 0x06004A42 RID: 19010 RVA: 0x00269104 File Offset: 0x00267504
		Private Sub OnKeyboardElementAssignmentPollingWindowUpdate(windowId As Integer)
			If Me.currentPlayer Is Nothing Then
				Return
			End If
			Dim window As Window = Me.windowManager.GetWindow(windowId)
			If windowId < 0 Then
				Return
			End If
			If Me.pendingInputMapping Is Nothing Then
				Return
			End If
			Me.InputPollingStarted()
			If window.timer.finished Then
				Me.InputPollingStopped()
				Me.CloseWindow(windowId)
				Return
			End If
			Dim controllerPollingInfo As ControllerPollingInfo
			Dim flag As Boolean
			Dim modifierKeyFlags As ModifierKeyFlags
			Dim text As String
			Me.PollKeyboardForAssignment(controllerPollingInfo, flag, modifierKeyFlags, text)
			If flag Then
				window.timer.Start(Me._inputAssignmentTimeout)
			End If
			window.SetContentText(If((Not flag), Mathf.CeilToInt(window.timer.remaining).ToString(), String.Empty), 2)
			window.SetContentText(text, 1)
			If Not controllerPollingInfo.success Then
				Return
			End If
			If Not Me.IsAllowedAssignment(Me.pendingInputMapping, controllerPollingInfo) Then
				Return
			End If
			Dim elementAssignment As ElementAssignment = Me.pendingInputMapping.ToElementAssignment(controllerPollingInfo, modifierKeyFlags)
			If Not Me.HasElementAssignmentConflicts(Me.currentPlayer, Me.pendingInputMapping, elementAssignment, False) Then
				Me.pendingInputMapping.map.ReplaceOrCreateElementMap(elementAssignment)
				Me.InputPollingStopped()
				Me.CloseWindow(windowId)
			Else
				Me.InputPollingStopped()
				Me.ShowElementAssignmentConflictWindow(elementAssignment, False)
			End If
		End Sub

		' Token: 0x06004A43 RID: 19011 RVA: 0x00269244 File Offset: 0x00267644
		Private Sub OnMouseElementAssignmentPollingWindowUpdate(windowId As Integer)
			If Me.currentPlayer Is Nothing Then
				Return
			End If
			Dim window As Window = Me.windowManager.GetWindow(windowId)
			If windowId < 0 Then
				Return
			End If
			If Me.pendingInputMapping Is Nothing Then
				Return
			End If
			Me.InputPollingStarted()
			If window.timer.finished Then
				Me.InputPollingStopped()
				Me.CloseWindow(windowId)
				Return
			End If
			window.SetContentText(Mathf.CeilToInt(window.timer.remaining).ToString(), 1)
			Dim controllerPollingInfo As ControllerPollingInfo
			If Me._ignoreMouseXAxisAssignment OrElse Me._ignoreMouseYAxisAssignment Then
				controllerPollingInfo = Nothing
				For Each controllerPollingInfo2 As ControllerPollingInfo In ReInput.controllers.polling.PollControllerForAllElementsDown(ControllerType.Mouse, 0)
					If controllerPollingInfo2.elementType = ControllerElementType.Axis Then
						If Me._ignoreMouseXAxisAssignment AndAlso controllerPollingInfo2.elementIndex = 0 Then
							Continue For
						End If
						If Me._ignoreMouseYAxisAssignment AndAlso controllerPollingInfo2.elementIndex = 1 Then
							Continue For
						End If
					End If
					controllerPollingInfo = controllerPollingInfo2
					Exit For
				Next
			Else
				controllerPollingInfo = ReInput.controllers.polling.PollControllerForFirstElementDown(ControllerType.Mouse, 0)
			End If
			If Not controllerPollingInfo.success Then
				Return
			End If
			If Not Me.IsAllowedAssignment(Me.pendingInputMapping, controllerPollingInfo) Then
				Return
			End If
			Dim elementAssignment As ElementAssignment = Me.pendingInputMapping.ToElementAssignment(controllerPollingInfo)
			If Not Me.HasElementAssignmentConflicts(Me.currentPlayer, Me.pendingInputMapping, elementAssignment, True) Then
				Me.pendingInputMapping.map.ReplaceOrCreateElementMap(elementAssignment)
				Me.InputPollingStopped()
				Me.CloseWindow(windowId)
			Else
				Me.InputPollingStopped()
				Me.ShowElementAssignmentConflictWindow(elementAssignment, True)
			End If
		End Sub

		' Token: 0x06004A44 RID: 19012 RVA: 0x0026941C File Offset: 0x0026781C
		Private Sub OnCalibrateAxisStep1WindowUpdate(windowId As Integer)
			If Me.currentPlayer Is Nothing Then
				Return
			End If
			Dim window As Window = Me.windowManager.GetWindow(windowId)
			If windowId < 0 Then
				Return
			End If
			If Me.pendingAxisCalibration Is Nothing OrElse Not Me.pendingAxisCalibration.isValid Then
				Return
			End If
			Me.InputPollingStarted()
			If Not window.timer.finished Then
				window.SetContentText(Mathf.CeilToInt(window.timer.remaining).ToString(), 1)
				If Me.currentPlayer.controllers.joystickCount = 0 Then
					Return
				End If
				If Not Me.pendingAxisCalibration.joystick.PollForFirstButtonDown().success Then
					Return
				End If
			End If
			Me.pendingAxisCalibration.RecordZero()
			Me.CloseWindow(windowId)
			Me.ShowCalibrateAxisStep2Window()
		End Sub

		' Token: 0x06004A45 RID: 19013 RVA: 0x002694F4 File Offset: 0x002678F4
		Private Sub OnCalibrateAxisStep2WindowUpdate(windowId As Integer)
			If Me.currentPlayer Is Nothing Then
				Return
			End If
			Dim window As Window = Me.windowManager.GetWindow(windowId)
			If windowId < 0 Then
				Return
			End If
			If Me.pendingAxisCalibration Is Nothing OrElse Not Me.pendingAxisCalibration.isValid Then
				Return
			End If
			If Not window.timer.finished Then
				window.SetContentText(Mathf.CeilToInt(window.timer.remaining).ToString(), 1)
				Me.pendingAxisCalibration.RecordMinMax()
				If Me.currentPlayer.controllers.joystickCount = 0 Then
					Return
				End If
				If Not Me.pendingAxisCalibration.joystick.PollForFirstButtonDown().success Then
					Return
				End If
			End If
			Me.EndAxisCalibration()
			Me.InputPollingStopped()
			Me.CloseWindow(windowId)
		End Sub

		' Token: 0x06004A46 RID: 19014 RVA: 0x002695CC File Offset: 0x002679CC
		Private Sub ShowAssignControllerWindow()
			If Me.currentPlayer Is Nothing Then
				Return
			End If
			If ReInput.controllers.joystickCount = 0 Then
				Return
			End If
			Dim window As Window = Me.OpenWindow(True)
			If window Is Nothing Then
				Return
			End If
			window.SetUpdateCallback(AddressOf Me.OnAssignControllerWindowUpdate)
			window.CreateTitleText(Me.prefabs.windowTitleText, Vector2.zero, Me._language.assignControllerWindowTitle)
			window.AddContentText(Me.prefabs.windowContentText, UIPivot.TopCenter, UIAnchor.TopHStretch, New Vector2(0F, -100F), Me._language.assignControllerWindowMessage)
			window.AddContentText(Me.prefabs.windowContentText, UIPivot.BottomCenter, UIAnchor.BottomHStretch, Vector2.zero, String.Empty)
			window.timer.Start(Me._controllerAssignmentTimeout)
			Me.windowManager.Focus(window)
		End Sub

		' Token: 0x06004A47 RID: 19015 RVA: 0x002696B4 File Offset: 0x00267AB4
		Private Sub ShowControllerAssignmentConflictWindow(controllerId As Integer)
			If Me.currentPlayer Is Nothing Then
				Return
			End If
			If ReInput.controllers.joystickCount = 0 Then
				Return
			End If
			Dim window As Window = Me.OpenWindow(True)
			If window Is Nothing Then
				Return
			End If
			Dim text As String = String.Empty
			Dim players As IList(Of Player) = ReInput.players.Players
			For i As Integer = 0 To players.Count - 1
				If players(i) IsNot Me.currentPlayer Then
					If players(i).controllers.ContainsController(ControllerType.Joystick, controllerId) Then
						text = players(i).descriptiveName
						Exit For
					End If
				End If
			Next
			Dim joystick As Joystick = ReInput.controllers.GetJoystick(controllerId)
			window.CreateTitleText(Me.prefabs.windowTitleText, Vector2.zero, Me._language.controllerAssignmentConflictWindowTitle)
			window.AddContentText(Me.prefabs.windowContentText, UIPivot.TopCenter, UIAnchor.TopHStretch, New Vector2(0F, -100F), Me._language.GetControllerAssignmentConflictWindowMessage(joystick.name, text, Me.currentPlayer.descriptiveName))
			Dim unityAction As UnityAction = Sub()
				Me.OnWindowCancel(window.id)
			End Sub
			window.cancelCallback = unityAction
			window.CreateButton(Me.prefabs.fitButton, UIPivot.BottomLeft, UIAnchor.BottomLeft, Vector2.zero, Me._language.yes, Sub()
				Me.OnControllerAssignmentConfirmed(window.id, Me.currentPlayer, controllerId)
			End Sub, unityAction, True)
			window.CreateButton(Me.prefabs.fitButton, UIPivot.BottomRight, UIAnchor.BottomRight, Vector2.zero, Me._language.no, unityAction, unityAction, False)
			Me.windowManager.Focus(window)
		End Sub

		' Token: 0x06004A48 RID: 19016 RVA: 0x002698A8 File Offset: 0x00267CA8
		Private Sub ShowBeginElementAssignmentReplacementWindow(fieldInfo As InputFieldInfo, action As InputAction, map As ControllerMap, aem As ActionElementMap, actionName As String)
			Dim guiinputField As ControlMapper.GUIInputField = Me.inputGrid.GetGUIInputField(Me.currentMapCategoryId, action.id, fieldInfo.axisRange, fieldInfo.controllerType, fieldInfo.intData)
			If guiinputField Is Nothing Then
				Return
			End If
			Dim window As Window = Me.OpenWindow(True)
			If window Is Nothing Then
				Return
			End If
			window.CreateTitleText(Me.prefabs.windowTitleText, Vector2.zero, actionName)
			window.AddContentText(Me.prefabs.windowContentText, UIPivot.TopCenter, UIAnchor.TopHStretch, New Vector2(0F, -100F), guiinputField.GetLabel())
			Dim unityAction As UnityAction = Sub()
				Me.OnWindowCancel(window.id)
			End Sub
			window.cancelCallback = unityAction
			window.CreateButton(Me.prefabs.fitButton, UIPivot.BottomLeft, UIAnchor.BottomLeft, Vector2.zero, Me._language.replace, Sub()
				Me.OnBeginElementAssignment(fieldInfo, map, aem, actionName)
			End Sub, unityAction, True)
			window.CreateButton(Me.prefabs.fitButton, UIPivot.BottomCenter, UIAnchor.BottomCenter, Vector2.zero, Me._language.remove, Sub()
				Me.OnRemoveElementAssignment(window.id, map, aem)
			End Sub, unityAction, False)
			window.CreateButton(Me.prefabs.fitButton, UIPivot.BottomRight, UIAnchor.BottomRight, Vector2.zero, Me._language.cancel, unityAction, unityAction, False)
			Me.windowManager.Focus(window)
		End Sub

		' Token: 0x06004A49 RID: 19017 RVA: 0x00269A70 File Offset: 0x00267E70
		Private Sub ShowCreateNewElementAssignmentWindow(fieldInfo As InputFieldInfo, action As InputAction, map As ControllerMap, actionName As String)
			If Me.inputGrid.GetGUIInputField(Me.currentMapCategoryId, action.id, fieldInfo.axisRange, fieldInfo.controllerType, fieldInfo.intData) Is Nothing Then
				Return
			End If
			Me.OnBeginElementAssignment(fieldInfo, map, Nothing, actionName)
		End Sub

		' Token: 0x06004A4A RID: 19018 RVA: 0x00269ABC File Offset: 0x00267EBC
		Private Sub ShowElementAssignmentPrePollingWindow()
			If Me.pendingInputMapping Is Nothing Then
				Return
			End If
			Dim window As Window = Me.OpenWindow(True)
			If window Is Nothing Then
				Return
			End If
			window.CreateTitleText(Me.prefabs.windowTitleText, Vector2.zero, Me.pendingInputMapping.actionName)
			window.AddContentText(Me.prefabs.windowContentText, UIPivot.TopCenter, UIAnchor.TopHStretch, New Vector2(0F, -100F), Me._language.elementAssignmentPrePollingWindowMessage)
			If Me.prefabs.centerStickGraphic IsNot Nothing Then
				window.AddContentImage(Me.prefabs.centerStickGraphic, UIPivot.BottomCenter, UIAnchor.BottomCenter, New Vector2(0F, 10F))
			End If
			window.AddContentText(Me.prefabs.windowContentText, UIPivot.BottomCenter, UIAnchor.BottomHStretch, Vector2.zero, String.Empty)
			window.SetUpdateCallback(AddressOf Me.OnElementAssignmentPrePollingWindowUpdate)
			window.timer.Start(Me._preInputAssignmentTimeout)
			Me.windowManager.Focus(window)
		End Sub

		' Token: 0x06004A4B RID: 19019 RVA: 0x00269BD4 File Offset: 0x00267FD4
		Private Sub ShowElementAssignmentPollingWindow()
			If Me.pendingInputMapping Is Nothing Then
				Return
			End If
			Select Case Me.pendingInputMapping.controllerType
				Case ControllerType.Keyboard
					Me.ShowKeyboardElementAssignmentPollingWindow()
				Case ControllerType.Mouse
					If Me.currentPlayer.controllers.hasMouse Then
						Me.ShowMouseElementAssignmentPollingWindow()
					Else
						Me.ShowMouseAssignmentConflictWindow()
					End If
				Case ControllerType.Joystick
					Me.ShowJoystickElementAssignmentPollingWindow()
				Case Else
					Throw New NotImplementedException()
			End Select
		End Sub

		' Token: 0x06004A4C RID: 19020 RVA: 0x00269C58 File Offset: 0x00268058
		Private Sub ShowJoystickElementAssignmentPollingWindow()
			If Me.pendingInputMapping Is Nothing Then
				Return
			End If
			Dim window As Window = Me.OpenWindow(True)
			If window Is Nothing Then
				Return
			End If
			Dim text As String = If((Me.pendingInputMapping.axisRange <> AxisRange.Full OrElse Not Me._showFullAxisInputFields OrElse Me._showSplitAxisInputFields), Me._language.GetJoystickElementAssignmentPollingWindowMessage(Me.pendingInputMapping.actionName), Me._language.GetJoystickElementAssignmentPollingWindowMessage_FullAxisFieldOnly(Me.pendingInputMapping.actionName))
			window.CreateTitleText(Me.prefabs.windowTitleText, Vector2.zero, Me.pendingInputMapping.actionName)
			window.AddContentText(Me.prefabs.windowContentText, UIPivot.TopCenter, UIAnchor.TopHStretch, New Vector2(0F, -100F), text)
			window.AddContentText(Me.prefabs.windowContentText, UIPivot.BottomCenter, UIAnchor.BottomHStretch, Vector2.zero, String.Empty)
			window.SetUpdateCallback(AddressOf Me.OnJoystickElementAssignmentPollingWindowUpdate)
			window.timer.Start(Me._inputAssignmentTimeout)
			Me.windowManager.Focus(window)
		End Sub

		' Token: 0x06004A4D RID: 19021 RVA: 0x00269D80 File Offset: 0x00268180
		Private Sub ShowKeyboardElementAssignmentPollingWindow()
			If Me.pendingInputMapping Is Nothing Then
				Return
			End If
			Dim window As Window = Me.OpenWindow(True)
			If window Is Nothing Then
				Return
			End If
			window.CreateTitleText(Me.prefabs.windowTitleText, Vector2.zero, Me.pendingInputMapping.actionName)
			window.AddContentText(Me.prefabs.windowContentText, UIPivot.TopCenter, UIAnchor.TopHStretch, New Vector2(0F, -100F), Me._language.GetKeyboardElementAssignmentPollingWindowMessage(Me.pendingInputMapping.actionName))
			window.AddContentText(Me.prefabs.windowContentText, UIPivot.TopCenter, UIAnchor.TopHStretch, New Vector2(0F, -(window.GetContentTextHeight(0) + 50F)), String.Empty)
			window.AddContentText(Me.prefabs.windowContentText, UIPivot.BottomCenter, UIAnchor.BottomHStretch, Vector2.zero, String.Empty)
			window.SetUpdateCallback(AddressOf Me.OnKeyboardElementAssignmentPollingWindowUpdate)
			window.timer.Start(Me._inputAssignmentTimeout)
			Me.windowManager.Focus(window)
		End Sub

		' Token: 0x06004A4E RID: 19022 RVA: 0x00269E9C File Offset: 0x0026829C
		Private Sub ShowMouseElementAssignmentPollingWindow()
			If Me.pendingInputMapping Is Nothing Then
				Return
			End If
			Dim window As Window = Me.OpenWindow(True)
			If window Is Nothing Then
				Return
			End If
			Dim text As String = If((Me.pendingInputMapping.axisRange <> AxisRange.Full OrElse Not Me._showFullAxisInputFields OrElse Me._showSplitAxisInputFields), Me._language.GetMouseElementAssignmentPollingWindowMessage(Me.pendingInputMapping.actionName), Me._language.GetMouseElementAssignmentPollingWindowMessage_FullAxisFieldOnly(Me.pendingInputMapping.actionName))
			window.CreateTitleText(Me.prefabs.windowTitleText, Vector2.zero, Me.pendingInputMapping.actionName)
			window.AddContentText(Me.prefabs.windowContentText, UIPivot.TopCenter, UIAnchor.TopHStretch, New Vector2(0F, -100F), text)
			window.AddContentText(Me.prefabs.windowContentText, UIPivot.BottomCenter, UIAnchor.BottomHStretch, Vector2.zero, String.Empty)
			window.SetUpdateCallback(AddressOf Me.OnMouseElementAssignmentPollingWindowUpdate)
			window.timer.Start(Me._inputAssignmentTimeout)
			Me.windowManager.Focus(window)
		End Sub

		' Token: 0x06004A4F RID: 19023 RVA: 0x00269FC4 File Offset: 0x002683C4
		Private Sub ShowElementAssignmentConflictWindow(assignment As ElementAssignment, skipOtherPlayers As Boolean)
			If Me.pendingInputMapping Is Nothing Then
				Return
			End If
			Dim flag As Boolean = Me.IsBlockingAssignmentConflict(Me.pendingInputMapping, assignment, skipOtherPlayers)
			Dim text As String = If((Not flag), Me._language.GetElementAlreadyInUseCanReplace(Me.pendingInputMapping.elementName, Me._allowElementAssignmentConflicts), Me._language.GetElementAlreadyInUseBlocked(Me.pendingInputMapping.elementName))
			Dim elementAlreadyInUseCanReplaceFontSize As Integer = Me._language.GetElementAlreadyInUseCanReplaceFontSize(Me._allowElementAssignmentConflicts)
			Dim window As Window = Me.OpenWindow(True)
			If window Is Nothing Then
				Return
			End If
			window.CreateTitleText(Me.prefabs.windowTitleText, Vector2.zero, Me._language.elementAssignmentConflictWindowMessage)
			window.AddContentText(Me.prefabs.windowContentText, UIPivot.TopCenter, UIAnchor.TopHStretch, New Vector2(0F, -100F), text, elementAlreadyInUseCanReplaceFontSize)
			Dim unityAction As UnityAction = Sub()
				Me.OnWindowCancel(window.id)
			End Sub
			window.cancelCallback = unityAction
			If flag Then
				window.CreateButton(Me.prefabs.fitButton, UIPivot.BottomCenter, UIAnchor.BottomCenter, Vector2.zero, Me._language.okay, unityAction, unityAction, True)
			Else
				window.CreateButton(Me.prefabs.fitButton, UIPivot.BottomLeft, UIAnchor.BottomLeft, Vector2.zero, Me._language.replace, Sub()
					Me.OnElementAssignmentConflictReplaceConfirmed(window.id, Me.pendingInputMapping, assignment, skipOtherPlayers)
				End Sub, unityAction, True)
				If Me._allowElementAssignmentConflicts Then
					window.CreateButton(Me.prefabs.fitButton, UIPivot.BottomCenter, UIAnchor.BottomCenter, Vector2.zero, Me._language.add, Sub()
						Me.OnElementAssignmentAddConfirmed(window.id, Me.pendingInputMapping, assignment)
					End Sub, unityAction, False)
				End If
				window.CreateButton(Me.prefabs.fitButton, UIPivot.BottomRight, UIAnchor.BottomRight, Vector2.zero, Me._language.cancel, unityAction, unityAction, False)
			End If
			Me.windowManager.Focus(window)
		End Sub

		' Token: 0x06004A50 RID: 19024 RVA: 0x0026A204 File Offset: 0x00268604
		Private Sub ShowMouseAssignmentConflictWindow()
			If Me.currentPlayer Is Nothing Then
				Return
			End If
			Dim window As Window = Me.OpenWindow(True)
			If window Is Nothing Then
				Return
			End If
			Dim text As String = String.Empty
			Dim players As IList(Of Player) = ReInput.players.Players
			For i As Integer = 0 To players.Count - 1
				If players(i) IsNot Me.currentPlayer Then
					If players(i).controllers.hasMouse Then
						text = players(i).descriptiveName
						Exit For
					End If
				End If
			Next
			window.CreateTitleText(Me.prefabs.windowTitleText, Vector2.zero, Me._language.mouseAssignmentConflictWindowTitle)
			window.AddContentText(Me.prefabs.windowContentText, UIPivot.TopCenter, UIAnchor.TopHStretch, New Vector2(0F, -100F), Me._language.GetMouseAssignmentConflictWindowMessage(text, Me.currentPlayer.descriptiveName))
			Dim unityAction As UnityAction = Sub()
				Me.OnWindowCancel(window.id)
			End Sub
			window.cancelCallback = unityAction
			window.CreateButton(Me.prefabs.fitButton, UIPivot.BottomLeft, UIAnchor.BottomLeft, Vector2.zero, Me._language.yes, Sub()
				Me.OnMouseAssignmentConfirmed(window.id, Me.currentPlayer)
			End Sub, unityAction, True)
			window.CreateButton(Me.prefabs.fitButton, UIPivot.BottomRight, UIAnchor.BottomRight, Vector2.zero, Me._language.no, unityAction, unityAction, False)
			Me.windowManager.Focus(window)
		End Sub

		' Token: 0x06004A51 RID: 19025 RVA: 0x0026A3C4 File Offset: 0x002687C4
		Private Sub ShowCalibrateControllerWindow()
			If Me.currentPlayer Is Nothing Then
				Return
			End If
			If Me.currentPlayer.controllers.joystickCount = 0 Then
				Return
			End If
			Dim calibrationWindow As CalibrationWindow = TryCast(Me.OpenWindow(Me.prefabs.calibrationWindow, "CalibrationWindow", True), CalibrationWindow)
			If calibrationWindow Is Nothing Then
				Return
			End If
			Dim currentJoystick As Joystick = Me.currentJoystick
			calibrationWindow.CreateTitleText(Me.prefabs.windowTitleText, Vector2.zero, Me._language.calibrateControllerWindowTitle)
			calibrationWindow.SetJoystick(Me.currentPlayer.id, currentJoystick)
			calibrationWindow.SetButtonCallback(CalibrationWindow.ButtonIdentifier.Done, AddressOf Me.CloseWindow)
			calibrationWindow.SetButtonCallback(CalibrationWindow.ButtonIdentifier.Calibrate, AddressOf Me.StartAxisCalibration)
			calibrationWindow.SetButtonCallback(CalibrationWindow.ButtonIdentifier.Cancel, AddressOf Me.CloseWindow)
			Me.windowManager.Focus(calibrationWindow)
		End Sub

		' Token: 0x06004A52 RID: 19026 RVA: 0x0026A49C File Offset: 0x0026889C
		Private Sub ShowCalibrateAxisStep1Window()
			If Me.currentPlayer Is Nothing Then
				Return
			End If
			Dim window As Window = Me.OpenWindow(False)
			If window Is Nothing Then
				Return
			End If
			If Me.pendingAxisCalibration Is Nothing Then
				Return
			End If
			Dim joystick As Joystick = Me.pendingAxisCalibration.joystick
			If joystick.axisCount = 0 Then
				Return
			End If
			Dim axisIndex As Integer = Me.pendingAxisCalibration.axisIndex
			If axisIndex < 0 OrElse axisIndex >= joystick.axisCount Then
				Return
			End If
			window.CreateTitleText(Me.prefabs.windowTitleText, Vector2.zero, Me._language.calibrateAxisStep1WindowTitle)
			window.AddContentText(Me.prefabs.windowContentText, UIPivot.TopCenter, UIAnchor.TopHStretch, New Vector2(0F, -100F), Me._language.GetCalibrateAxisStep1WindowMessage(joystick.AxisElementIdentifiers(axisIndex).name))
			If Me.prefabs.centerStickGraphic IsNot Nothing Then
				window.AddContentImage(Me.prefabs.centerStickGraphic, UIPivot.BottomCenter, UIAnchor.BottomCenter, New Vector2(0F, 10F))
			End If
			window.AddContentText(Me.prefabs.windowContentText, UIPivot.BottomCenter, UIAnchor.BottomHStretch, Vector2.zero, String.Empty)
			window.SetUpdateCallback(AddressOf Me.OnCalibrateAxisStep1WindowUpdate)
			window.timer.Start(Me._axisCalibrationTimeout)
			Me.windowManager.Focus(window)
		End Sub

		' Token: 0x06004A53 RID: 19027 RVA: 0x0026A60C File Offset: 0x00268A0C
		Private Sub ShowCalibrateAxisStep2Window()
			If Me.currentPlayer Is Nothing Then
				Return
			End If
			Dim window As Window = Me.OpenWindow(False)
			If window Is Nothing Then
				Return
			End If
			If Me.pendingAxisCalibration Is Nothing Then
				Return
			End If
			Dim joystick As Joystick = Me.pendingAxisCalibration.joystick
			If joystick.axisCount = 0 Then
				Return
			End If
			Dim axisIndex As Integer = Me.pendingAxisCalibration.axisIndex
			If axisIndex < 0 OrElse axisIndex >= joystick.axisCount Then
				Return
			End If
			window.CreateTitleText(Me.prefabs.windowTitleText, Vector2.zero, Me._language.calibrateAxisStep2WindowTitle)
			window.AddContentText(Me.prefabs.windowContentText, UIPivot.TopCenter, UIAnchor.TopHStretch, New Vector2(0F, -100F), Me._language.GetCalibrateAxisStep2WindowMessage(joystick.AxisElementIdentifiers(axisIndex).name))
			If Me.prefabs.moveStickGraphic IsNot Nothing Then
				window.AddContentImage(Me.prefabs.moveStickGraphic, UIPivot.BottomCenter, UIAnchor.BottomCenter, New Vector2(0F, 40F))
			End If
			window.AddContentText(Me.prefabs.windowContentText, UIPivot.BottomCenter, UIAnchor.BottomHStretch, Vector2.zero, String.Empty)
			window.SetUpdateCallback(AddressOf Me.OnCalibrateAxisStep2WindowUpdate)
			window.timer.Start(Me._axisCalibrationTimeout)
			Me.windowManager.Focus(window)
		End Sub

		' Token: 0x06004A54 RID: 19028 RVA: 0x0026A77C File Offset: 0x00268B7C
		Private Sub ShowEditInputBehaviorsWindow()
			If Me.currentPlayer Is Nothing Then
				Return
			End If
			If Me._inputBehaviorSettings Is Nothing Then
				Return
			End If
			Dim inputBehaviorWindow As InputBehaviorWindow = TryCast(Me.OpenWindow(Me.prefabs.inputBehaviorsWindow, "EditInputBehaviorsWindow", True), InputBehaviorWindow)
			If inputBehaviorWindow Is Nothing Then
				Return
			End If
			inputBehaviorWindow.CreateTitleText(Me.prefabs.windowTitleText, Vector2.zero, Me._language.inputBehaviorSettingsWindowTitle)
			inputBehaviorWindow.SetData(Me.currentPlayer.id, Me._inputBehaviorSettings)
			inputBehaviorWindow.SetButtonCallback(InputBehaviorWindow.ButtonIdentifier.Done, AddressOf Me.CloseWindow)
			inputBehaviorWindow.SetButtonCallback(InputBehaviorWindow.ButtonIdentifier.Cancel, AddressOf Me.CloseWindow)
			Me.windowManager.Focus(inputBehaviorWindow)
		End Sub

		' Token: 0x06004A55 RID: 19029 RVA: 0x0026A838 File Offset: 0x00268C38
		Private Sub ShowRestoreDefaultsWindow()
			If Me.currentPlayer Is Nothing Then
				Return
			End If
			Me.OpenModal(Me._language.restoreDefaultsWindowTitle, Me._language.restoreDefaultsWindowMessage, Me._language.yes, AddressOf Me.OnRestoreDefaultsConfirmed, Me._language.no, AddressOf Me.OnWindowCancel, True)
		End Sub

		' Token: 0x06004A56 RID: 19030 RVA: 0x0026A89C File Offset: 0x00268C9C
		Private Sub CreateInputGrid()
			Me.InitializeInputGrid()
			Me.CreateHeaderLabels()
			Me.CreateActionLabelColumn()
			Me.CreateKeyboardInputFieldColumn()
			Me.CreateMouseInputFieldColumn()
			Me.CreateControllerInputFieldColumn()
			Me.CreateInputActionLabels()
			Me.CreateInputFields()
			Me.inputGrid.HideAll()
		End Sub

		' Token: 0x06004A57 RID: 19031 RVA: 0x0026A8DC File Offset: 0x00268CDC
		Private Sub InitializeInputGrid()
			If Me.inputGrid Is Nothing Then
				Me.inputGrid = New ControlMapper.InputGrid()
			Else
				Me.inputGrid.ClearAll()
			End If
			For i As Integer = 0 To Me._mappingSets.Length - 1
				Dim mappingSet As ControlMapper.MappingSet = Me._mappingSets(i)
				If mappingSet IsNot Nothing AndAlso mappingSet.isValid Then
					Dim mapCategory As InputMapCategory = ReInput.mapping.GetMapCategory(mappingSet.mapCategoryId)
					If mapCategory IsNot Nothing Then
						If mapCategory.userAssignable Then
							Me.inputGrid.AddMapCategory(mappingSet.mapCategoryId)
							If mappingSet.actionListMode = ControlMapper.MappingSet.ActionListMode.ActionCategory Then
								Dim actionCategoryIds As IList(Of Integer) = mappingSet.actionCategoryIds
								For j As Integer = 0 To actionCategoryIds.Count - 1
									Dim num As Integer = actionCategoryIds(j)
									Dim actionCategory As InputCategory = ReInput.mapping.GetActionCategory(num)
									If actionCategory IsNot Nothing Then
										If actionCategory.userAssignable Then
											Me.inputGrid.AddActionCategory(mappingSet.mapCategoryId, num)
											For Each inputAction As InputAction In ReInput.mapping.UserAssignableActionsInCategory(num)
												If inputAction.type = InputActionType.Axis Then
													If Me._showFullAxisInputFields Then
														Me.inputGrid.AddAction(mappingSet.mapCategoryId, inputAction, AxisRange.Full)
													End If
													If Me._showSplitAxisInputFields Then
														Me.inputGrid.AddAction(mappingSet.mapCategoryId, inputAction, AxisRange.Positive)
														Me.inputGrid.AddAction(mappingSet.mapCategoryId, inputAction, AxisRange.Negative)
													End If
												ElseIf inputAction.type = InputActionType.Button Then
													Me.inputGrid.AddAction(mappingSet.mapCategoryId, inputAction, AxisRange.Positive)
												End If
											Next
										End If
									End If
								Next
							Else
								Dim actionIds As IList(Of Integer) = mappingSet.actionIds
								For k As Integer = 0 To actionIds.Count - 1
									Dim action As InputAction = ReInput.mapping.GetAction(actionIds(k))
									If action IsNot Nothing Then
										If action.type = InputActionType.Axis Then
											If Me._showFullAxisInputFields Then
												Me.inputGrid.AddAction(mappingSet.mapCategoryId, action, AxisRange.Full)
											End If
											If Me._showSplitAxisInputFields Then
												Me.inputGrid.AddAction(mappingSet.mapCategoryId, action, AxisRange.Positive)
												Me.inputGrid.AddAction(mappingSet.mapCategoryId, action, AxisRange.Negative)
											End If
										ElseIf action.type = InputActionType.Button Then
											Me.inputGrid.AddAction(mappingSet.mapCategoryId, action, AxisRange.Positive)
										End If
									End If
								Next
							End If
						End If
					End If
				End If
			Next
			Me.references.inputGridLayoutElement.flexibleWidth = 0F
			Me.references.inputGridLayoutElement.preferredWidth = CSng(Me.inputGridWidth)
		End Sub

		' Token: 0x06004A58 RID: 19032 RVA: 0x0026ABC8 File Offset: 0x00268FC8
		Private Sub RefreshInputGridStructure()
			If Me.currentMappingSet Is Nothing Then
				Return
			End If
			Me.inputGrid.HideAll()
			Me.inputGrid.Show(Me.currentMappingSet.mapCategoryId)
			Me.references.inputGridInnerGroup.GetComponent(Of RectTransform)().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Me.inputGrid.GetColumnHeight(Me.currentMappingSet.mapCategoryId))
		End Sub

		' Token: 0x06004A59 RID: 19033 RVA: 0x0026AC30 File Offset: 0x00269030
		Private Sub CreateHeaderLabels()
			Me.references.inputGridHeader1 = Me.CreateNewColumnGroup("ActionsHeader", Me.references.actionsColumnHeadersGroup, Me._actionLabelWidth).transform
			Dim guilabel As ControlMapper.GUILabel = Me.CreateLabel(Me.prefabs.actionsHeaderLabel, Me._language.actionColumnLabel.ToUpper(), Me.references.inputGridHeader1, New Vector2(14F, -10F))
			guilabel.rectTransform.offsetMax = Vector2.zero
			If Me._showKeyboard Then
				Me.references.inputGridHeader2 = Me.CreateNewColumnGroup("KeyboardHeader", Me.references.inputGridHeadersGroup, 226).transform
				Dim guilabel2 As ControlMapper.GUILabel = Me.CreateLabel(Me.prefabs.inputGridHeaderLabel, Me._language.keyboardColumnLabel, Me.references.inputGridHeader2, New Vector2(14F, -10F))
				guilabel2.rectTransform.offsetMax = Vector2.zero
			End If
			If Me._showMouse Then
				Me.references.inputGridHeader3 = Me.CreateNewColumnGroup("MouseHeader", Me.references.inputGridHeadersGroup, Me._mouseColMaxWidth).transform
				Dim guilabel2 As ControlMapper.GUILabel = Me.CreateLabel(Me.prefabs.inputGridHeaderLabel, Me._language.mouseColumnLabel, Me.references.inputGridHeader3, Vector2.zero)
				guilabel2.SetTextAlignment(TextAnchor.MiddleCenter)
			End If
			If Me._showControllers Then
				Me.references.inputGridHeader4 = Me.CreateNewColumnGroup("ControllerHeader", Me.references.inputGridHeadersGroup, 230).transform
				Dim guilabel2 As ControlMapper.GUILabel = Me.CreateLabel(Me.prefabs.inputGridHeaderLabel, Me._language.controllerColumnLabel, Me.references.inputGridHeader4, New Vector2(14F, -10F))
				guilabel2.rectTransform.offsetMax = Vector2.zero
			End If
		End Sub

		' Token: 0x06004A5A RID: 19034 RVA: 0x0026AE1C File Offset: 0x0026921C
		Private Sub CreateActionLabelColumn()
			Dim transform As Transform = Me.CreateNewColumnGroup("ActionLabelColumn", Me.references.actionsColumn, Me._actionLabelWidth).transform
			Me.references.inputGridActionColumn = transform
			Dim component As RectTransform = transform.GetComponent(Of RectTransform)()
			component.anchorMin = New Vector2(0F, 0F)
			component.anchorMax = New Vector2(1F, 1F)
			component.pivot = New Vector2(0.5F, 0.5F)
			component.offsetMin = New Vector2(14F, 0F)
			component.offsetMax = New Vector2(0F, -70F)
		End Sub

		' Token: 0x06004A5B RID: 19035 RVA: 0x0026AEC7 File Offset: 0x002692C7
		Private Sub CreateKeyboardInputFieldColumn()
			If Not Me._showKeyboard Then
				Return
			End If
			Me.CreateInputFieldColumn("KeyboardColumn", ControllerType.Keyboard, Me._keyboardColMaxWidth, Me._keyboardInputFieldCount, True)
		End Sub

		' Token: 0x06004A5C RID: 19036 RVA: 0x0026AEEE File Offset: 0x002692EE
		Private Sub CreateMouseInputFieldColumn()
			If Not Me._showMouse Then
				Return
			End If
			Me.CreateInputFieldColumn("MouseColumn", ControllerType.Mouse, Me._mouseColMaxWidth, Me._mouseInputFieldCount, False)
		End Sub

		' Token: 0x06004A5D RID: 19037 RVA: 0x0026AF15 File Offset: 0x00269315
		Private Sub CreateControllerInputFieldColumn()
			If Not Me._showControllers Then
				Return
			End If
			Me.CreateInputFieldColumn("ControllerColumn", ControllerType.Joystick, Me._controllerColMaxWidth, Me._controllerInputFieldCount, False)
		End Sub

		' Token: 0x06004A5E RID: 19038 RVA: 0x0026AF3C File Offset: 0x0026933C
		Private Sub CreateInputFieldColumn(name As String, controllerType As ControllerType, maxWidth As Integer, cols As Integer, disableFullAxis As Boolean)
			Dim transform As Transform = Me.CreateNewColumnGroup(name, Me.references.inputGridInnerGroup, maxWidth).transform
			Select Case controllerType
				Case ControllerType.Keyboard
					Me.references.inputGridKeyboardColumn = transform
				Case ControllerType.Mouse
					Me.references.inputGridMouseColumn = transform
				Case ControllerType.Joystick
					Me.references.inputGridControllerColumn = transform
				Case Else
					Throw New NotImplementedException()
			End Select
		End Sub

		' Token: 0x06004A5F RID: 19039 RVA: 0x0026AFB4 File Offset: 0x002693B4
		Private Sub CreateInputActionLabels()
			Dim inputGridActionColumn As Transform = Me.references.inputGridActionColumn
			For i As Integer = 0 To Me._mappingSets.Length - 1
				Dim mappingSet As ControlMapper.MappingSet = Me._mappingSets(i)
				If mappingSet IsNot Nothing AndAlso mappingSet.isValid Then
					Dim num As Single = 6F
					If mappingSet.actionListMode = ControlMapper.MappingSet.ActionListMode.ActionCategory Then
						Dim num2 As Integer = 0
						Dim actionCategoryIds As IList(Of Integer) = mappingSet.actionCategoryIds
						For j As Integer = 0 To actionCategoryIds.Count - 1
							Dim actionCategory As InputCategory = ReInput.mapping.GetActionCategory(actionCategoryIds(j))
							If actionCategory IsNot Nothing Then
								If actionCategory.userAssignable Then
									If Me.CountIEnumerable(Of InputAction)(ReInput.mapping.UserAssignableActionsInCategory(actionCategory.id)) <> 0 Then
										If Me._showActionCategoryLabels Then
											If num2 > 0 Then
												num -= CSng(Me._inputRowCategorySpacing)
											End If
											Dim guilabel As ControlMapper.GUILabel = Me.CreateLabel(Localization.Translate(actionCategory.descriptiveName).text, inputGridActionColumn, New Vector2(0F, num))
											guilabel.SetFontStyle(FontStyle.Bold)
											guilabel.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Me._inputRowHeight)
											Me.inputGrid.AddActionCategoryLabel(mappingSet.mapCategoryId, actionCategory.id, guilabel)
											num -= Me._inputRowHeight
										End If
										For Each inputAction As InputAction In ReInput.mapping.UserAssignableActionsInCategory(actionCategory.id, True)
											If inputAction.type = InputActionType.Axis Then
												If Me._showFullAxisInputFields Then
													Dim guilabel2 As ControlMapper.GUILabel = Me.CreateLabel(Localization.Translate(inputAction.descriptiveName).text, inputGridActionColumn, New Vector2(0F, num))
													guilabel2.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Me._inputRowHeight)
													Me.inputGrid.AddActionLabel(mappingSet.mapCategoryId, inputAction.id, AxisRange.Full, guilabel2)
													num -= Me._inputRowHeight
												End If
												If Me._showSplitAxisInputFields Then
													Dim text As String = If(String.IsNullOrEmpty(inputAction.positiveDescriptiveName), (Localization.Translate(inputAction.descriptiveName).text + " +"), Localization.Translate(inputAction.positiveDescriptiveName).text)
													Dim guilabel2 As ControlMapper.GUILabel = Me.CreateLabel(text, inputGridActionColumn, New Vector2(0F, num))
													guilabel2.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Me._inputRowHeight)
													Me.inputGrid.AddActionLabel(mappingSet.mapCategoryId, inputAction.id, AxisRange.Positive, guilabel2)
													num -= Me._inputRowHeight
													Dim text2 As String = If(String.IsNullOrEmpty(inputAction.negativeDescriptiveName), (Localization.Translate(inputAction.descriptiveName).text + " -"), Localization.Translate(inputAction.negativeDescriptiveName).text)
													guilabel2 = Me.CreateLabel(text2, inputGridActionColumn, New Vector2(0F, num))
													guilabel2.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Me._inputRowHeight)
													Me.inputGrid.AddActionLabel(mappingSet.mapCategoryId, inputAction.id, AxisRange.Negative, guilabel2)
													num -= Me._inputRowHeight
												End If
											ElseIf inputAction.type = InputActionType.Button Then
												Dim guilabel2 As ControlMapper.GUILabel = Me.CreateLabel(Localization.Translate(inputAction.descriptiveName).text, inputGridActionColumn, New Vector2(0F, num))
												guilabel2.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Me._inputRowHeight)
												Me.inputGrid.AddActionLabel(mappingSet.mapCategoryId, inputAction.id, AxisRange.Positive, guilabel2)
												num -= Me._inputRowHeight
											End If
										Next
										num2 += 1
									End If
								End If
							End If
						Next
					Else
						Dim actionIds As IList(Of Integer) = mappingSet.actionIds
						For k As Integer = 0 To actionIds.Count - 1
							Dim action As InputAction = ReInput.mapping.GetAction(actionIds(k))
							If action IsNot Nothing Then
								If action.userAssignable Then
									Dim actionCategory2 As InputCategory = ReInput.mapping.GetActionCategory(action.categoryId)
									If actionCategory2 IsNot Nothing Then
										If actionCategory2.userAssignable Then
											If action.type = InputActionType.Axis Then
												If Me._showFullAxisInputFields Then
													Dim guilabel3 As ControlMapper.GUILabel = Me.CreateLabel(Localization.Translate(action.descriptiveName).text, inputGridActionColumn, New Vector2(0F, num))
													guilabel3.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Me._inputRowHeight)
													Me.inputGrid.AddActionLabel(mappingSet.mapCategoryId, action.id, AxisRange.Full, guilabel3)
													num -= Me._inputRowHeight
												End If
												If Me._showSplitAxisInputFields Then
													Dim guilabel3 As ControlMapper.GUILabel = Me.CreateLabel(Localization.Translate(action.positiveDescriptiveName).text, inputGridActionColumn, New Vector2(0F, num))
													guilabel3.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Me._inputRowHeight)
													Me.inputGrid.AddActionLabel(mappingSet.mapCategoryId, action.id, AxisRange.Positive, guilabel3)
													num -= Me._inputRowHeight
													guilabel3 = Me.CreateLabel(Localization.Translate(action.negativeDescriptiveName).text, inputGridActionColumn, New Vector2(0F, num))
													guilabel3.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Me._inputRowHeight)
													Me.inputGrid.AddActionLabel(mappingSet.mapCategoryId, action.id, AxisRange.Negative, guilabel3)
													num -= Me._inputRowHeight
												End If
											ElseIf action.type = InputActionType.Button Then
												Dim guilabel3 As ControlMapper.GUILabel = Me.CreateLabel(Localization.Translate(action.descriptiveName).text, inputGridActionColumn, New Vector2(0F, num))
												guilabel3.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Me._inputRowHeight)
												Me.inputGrid.AddActionLabel(mappingSet.mapCategoryId, action.id, AxisRange.Positive, guilabel3)
												num -= Me._inputRowHeight
											End If
										End If
									End If
								End If
							End If
						Next
						For l As Integer = 0 To 2 - 1
							Dim guilabel4 As ControlMapper.GUILabel = Me.CreateDeactivatedLabel(Localization.Translate(If((l <> 0), "RemapFlipYAxis", "RemapFlipXAxis")).text, inputGridActionColumn, New Vector2(0F, num))
							guilabel4.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Me._inputRowHeight)
							Me.inactiveAxisToggleObjects.Add(guilabel4.gameObject)
							guilabel4 = Me.CreateLabel(Localization.Translate(If((l <> 0), "RemapFlipYAxis", "RemapFlipXAxis")).text, inputGridActionColumn, New Vector2(0F, num))
							guilabel4.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Me._inputRowHeight)
							Me.axisToggleObjects.Add(guilabel4.gameObject)
							num -= Me._inputRowHeight
						Next
					End If
					Me.inputGrid.SetColumnHeight(mappingSet.mapCategoryId, -num)
				End If
			Next
		End Sub

		' Token: 0x06004A60 RID: 19040 RVA: 0x0026B6D4 File Offset: 0x00269AD4
		Private Sub CreateInputFields()
			If Me._showControllers Then
				Me.CreateInputFields(Me.references.inputGridControllerColumn, ControllerType.Joystick, Me._controllerColMaxWidth, Me._controllerInputFieldCount, False)
			End If
			If Me._showKeyboard Then
				Me.CreateInputFields(Me.references.inputGridKeyboardColumn, ControllerType.Keyboard, Me._keyboardColMaxWidth, Me._keyboardInputFieldCount, True)
			End If
			If Me._showMouse Then
				Me.CreateInputFields(Me.references.inputGridMouseColumn, ControllerType.Mouse, Me._mouseColMaxWidth, Me._mouseInputFieldCount, False)
			End If
		End Sub

		' Token: 0x06004A61 RID: 19041 RVA: 0x0026B760 File Offset: 0x00269B60
		Private Sub CreateInputFields(columnXform As Transform, controllerType As ControllerType, maxWidth As Integer, cols As Integer, disableFullAxis As Boolean)
			For i As Integer = 0 To Me._mappingSets.Length - 1
				Dim mappingSet As ControlMapper.MappingSet = Me._mappingSets(i)
				If mappingSet IsNot Nothing AndAlso mappingSet.isValid Then
					Dim num As Integer = maxWidth / cols
					Dim num2 As Single = 6F
					Dim num3 As Integer = 0
					If mappingSet.actionListMode = ControlMapper.MappingSet.ActionListMode.ActionCategory Then
						Dim actionCategoryIds As IList(Of Integer) = mappingSet.actionCategoryIds
						For j As Integer = 0 To actionCategoryIds.Count - 1
							Dim actionCategory As InputCategory = ReInput.mapping.GetActionCategory(actionCategoryIds(j))
							If actionCategory IsNot Nothing Then
								If actionCategory.userAssignable Then
									If Me.CountIEnumerable(Of InputAction)(ReInput.mapping.UserAssignableActionsInCategory(actionCategory.id)) <> 0 Then
										If Me._showActionCategoryLabels Then
											num2 -= If((num3 <= 0), Me._inputRowHeight, (Me._inputRowHeight + CSng(Me._inputRowCategorySpacing)))
										End If
										For Each inputAction As InputAction In ReInput.mapping.UserAssignableActionsInCategory(actionCategory.id, True)
											If inputAction.type = InputActionType.Axis Then
												If Me._showFullAxisInputFields Then
													Me.CreateInputFieldSet(columnXform, mappingSet.mapCategoryId, inputAction, AxisRange.Full, controllerType, cols, num, num2, disableFullAxis)
												End If
												If Me._showSplitAxisInputFields Then
													Me.CreateInputFieldSet(columnXform, mappingSet.mapCategoryId, inputAction, AxisRange.Positive, controllerType, cols, num, num2, False)
													Me.CreateInputFieldSet(columnXform, mappingSet.mapCategoryId, inputAction, AxisRange.Negative, controllerType, cols, num, num2, False)
												End If
											ElseIf inputAction.type = InputActionType.Button Then
												Me.CreateInputFieldSet(columnXform, mappingSet.mapCategoryId, inputAction, AxisRange.Positive, controllerType, cols, num, num2, False)
											End If
											num3 += 1
										Next
									End If
								End If
							End If
						Next
					Else
						Dim actionIds As IList(Of Integer) = mappingSet.actionIds
						For k As Integer = 0 To actionIds.Count - 1
							Dim action As InputAction = ReInput.mapping.GetAction(actionIds(k))
							If action IsNot Nothing Then
								If action.userAssignable Then
									Dim actionCategory2 As InputCategory = ReInput.mapping.GetActionCategory(action.categoryId)
									If actionCategory2 IsNot Nothing Then
										If actionCategory2.userAssignable Then
											If action.type = InputActionType.Axis Then
												If Me._showFullAxisInputFields Then
													Me.CreateInputFieldSet(columnXform, mappingSet.mapCategoryId, action, AxisRange.Full, controllerType, cols, num, num2, disableFullAxis)
												End If
												If Me._showSplitAxisInputFields Then
													Me.CreateInputFieldSet(columnXform, mappingSet.mapCategoryId, action, AxisRange.Positive, controllerType, cols, num, num2, False)
													Me.CreateInputFieldSet(columnXform, mappingSet.mapCategoryId, action, AxisRange.Negative, controllerType, cols, num, num2, False)
												End If
											ElseIf action.type = InputActionType.Button Then
												Me.CreateInputFieldSet(columnXform, mappingSet.mapCategoryId, action, AxisRange.Positive, controllerType, cols, num, num2, False)
											End If
										End If
									End If
								End If
							End If
						Next
					End If
				End If
			Next
		End Sub

		' Token: 0x06004A62 RID: 19042 RVA: 0x0026BA74 File Offset: 0x00269E74
		Private Sub CreateInputFieldSet(parent As Transform, mapCategoryId As Integer, action As InputAction, axisRange As AxisRange, controllerType As ControllerType, cols As Integer, fieldWidth As Integer, ByRef yPos As Single, disableFullAxis As Boolean)
			Dim gameObject As GameObject = Me.CreateNewGUIObject("FieldLayoutGroup", parent, New Vector2(0F, yPos))
			Dim horizontalLayoutGroup As HorizontalLayoutGroup = gameObject.AddComponent(Of HorizontalLayoutGroup)()
			Dim component As RectTransform = gameObject.GetComponent(Of RectTransform)()
			component.anchorMin = New Vector2(0F, 1F)
			component.anchorMax = New Vector2(0F, 1F)
			component.pivot = New Vector2(0F, 1F)
			component.sizeDelta = Vector2.zero
			component.offsetMin = New Vector2(5F, component.offsetMin.y - 1F)
			component.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Me._inputRowHeight)
			component.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 227F)
			Me.inputGrid.AddInputFieldSet(mapCategoryId, action, axisRange, controllerType, gameObject)
			For i As Integer = 0 To cols - 1
				Dim num As Integer = If((axisRange <> AxisRange.Full), 0, Me._invertToggleWidth)
				Dim guiinputField As ControlMapper.GUIInputField = Me.CreateInputField(horizontalLayoutGroup.transform, Vector2.zero, String.Empty, action.id, axisRange, controllerType, i)
				guiinputField.SetFirstChildObjectWidth(ControlMapper.LayoutElementSizeType.PreferredSize, fieldWidth - num)
				Me.inputGrid.AddInputField(mapCategoryId, action, axisRange, controllerType, i, guiinputField)
				If axisRange = AxisRange.Full AndAlso controllerType = ControllerType.Joystick Then
					Dim gameObject2 As GameObject = Me.CreateNewGUIObject("FieldLayoutGroup", parent, New Vector2(0F, yPos - Me._inputRowHeight * CSng(If((Not(action.name = "MoveHorizontal")), 9, 13))))
					Dim horizontalLayoutGroup2 As HorizontalLayoutGroup = gameObject2.AddComponent(Of HorizontalLayoutGroup)()
					Dim component2 As RectTransform = gameObject2.GetComponent(Of RectTransform)()
					component2.anchorMin = New Vector2(0F, 1F)
					component2.anchorMax = New Vector2(0F, 1F)
					component2.pivot = New Vector2(0F, 1F)
					component2.sizeDelta = Vector2.zero
					component2.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Me._inputRowHeight)
					component2.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 227F)
					component2.offsetMin = New Vector2(5F, component2.offsetMin.y)
					Dim guitoggle As ControlMapper.GUIToggle = Me.CreateToggle(Me.prefabs.inputGridFieldInvertToggle, horizontalLayoutGroup2.transform, Vector2.zero, String.Empty, action.id, axisRange, controllerType, i)
					guitoggle.SetFirstChildObjectWidth(ControlMapper.LayoutElementSizeType.MinSize, num)
					guiinputField.AddToggle(guitoggle)
					Me.axisToggleObjects.Add(guitoggle.gameObject)
				End If
			Next
			yPos -= Me._inputRowHeight
		End Sub

		' Token: 0x06004A63 RID: 19043 RVA: 0x0026BD04 File Offset: 0x0026A104
		Private Sub PopulateInputFields()
			Me.inputGrid.InitializeFields(Me.currentMapCategoryId)
			If Me.currentPlayer Is Nothing Then
				Return
			End If
			Me.inputGrid.SetFieldsActive(Me.currentMapCategoryId, True)
			For Each inputActionSet As ControlMapper.InputActionSet In Me.inputGrid.GetActionSets(Me.currentMapCategoryId)
				If Me._showKeyboard Then
					If Me.currentPlayerId = 0 Then
						Dim controllerType As ControllerType = ControllerType.Keyboard
						Dim num As Integer = 0
						Dim num2 As Integer = Me._keyboardMapDefaultLayout
						Dim num3 As Integer = Me._keyboardInputFieldCount
						Dim controllerMapOrCreateNew As ControllerMap = Me.GetControllerMapOrCreateNew(controllerType, num, num2)
						Me.PopulateInputFieldGroup(inputActionSet, controllerMapOrCreateNew, controllerType, num, num3)
					Else
						Me.DisableInputFieldGroup(inputActionSet, ControllerType.Keyboard, Me._keyboardInputFieldCount)
					End If
				End If
				If Me._showMouse Then
					Dim controllerType As ControllerType = ControllerType.Mouse
					Dim num As Integer = 0
					Dim num2 As Integer = Me._mouseMapDefaultLayout
					Dim num3 As Integer = Me._mouseInputFieldCount
					Dim controllerMapOrCreateNew2 As ControllerMap = Me.GetControllerMapOrCreateNew(controllerType, num, num2)
					If Me.currentPlayer.controllers.hasMouse Then
						Me.PopulateInputFieldGroup(inputActionSet, controllerMapOrCreateNew2, controllerType, num, num3)
					End If
				End If
				If Me.isJoystickSelected AndAlso Me.currentPlayer.controllers.joystickCount > 0 Then
					Dim controllerType As ControllerType = ControllerType.Joystick
					Dim num As Integer = Me.currentJoystick.id
					Dim num2 As Integer = Me._joystickMapDefaultLayout
					Dim num3 As Integer = Me._controllerInputFieldCount
					Dim controllerMapOrCreateNew3 As ControllerMap = Me.GetControllerMapOrCreateNew(controllerType, num, num2)
					Me.PopulateInputFieldGroup(inputActionSet, controllerMapOrCreateNew3, controllerType, num, num3)
				Else
					Me.DisableInputFieldGroup(inputActionSet, ControllerType.Joystick, Me._controllerInputFieldCount)
				End If
			Next
		End Sub

		' Token: 0x06004A64 RID: 19044 RVA: 0x0026BEAC File Offset: 0x0026A2AC
		Private Sub PopulateInputFieldGroup(actionSet As ControlMapper.InputActionSet, controllerMap As ControllerMap, controllerType As ControllerType, controllerId As Integer, maxFields As Integer)
			If controllerMap Is Nothing Then
				Return
			End If
			Dim num As Integer = 0
			Me.inputGrid.SetFixedFieldData(Me.currentMapCategoryId, actionSet.actionId, actionSet.axisRange, controllerType, controllerId)
			For Each actionElementMap As ActionElementMap In controllerMap.ElementMapsWithAction(actionSet.actionId)
				If actionElementMap.elementType = ControllerElementType.Button Then
					If actionSet.axisRange = AxisRange.Full Then
						Continue For
					End If
					If actionSet.axisRange = AxisRange.Positive Then
						If actionElementMap.axisContribution = Pole.Negative Then
							Continue For
						End If
					ElseIf actionSet.axisRange = AxisRange.Negative AndAlso actionElementMap.axisContribution = Pole.Positive Then
						Continue For
					End If
					Me.inputGrid.PopulateField(Me.currentMapCategoryId, actionSet.actionId, actionSet.axisRange, controllerType, controllerId, num, actionElementMap.id, actionElementMap.elementIdentifierName, False)
				ElseIf actionElementMap.elementType = ControllerElementType.Axis Then
					If actionSet.axisRange = AxisRange.Full Then
						If actionElementMap.axisRange <> AxisRange.Full Then
							Continue For
						End If
						Me.inputGrid.PopulateField(Me.currentMapCategoryId, actionSet.actionId, actionSet.axisRange, controllerType, controllerId, num, actionElementMap.id, actionElementMap.elementIdentifierName, actionElementMap.invert)
					ElseIf actionSet.axisRange = AxisRange.Positive Then
						If actionElementMap.axisRange = AxisRange.Full Then
							Continue For
						End If
						If actionElementMap.axisContribution = Pole.Negative Then
							Continue For
						End If
						Me.inputGrid.PopulateField(Me.currentMapCategoryId, actionSet.actionId, actionSet.axisRange, controllerType, controllerId, num, actionElementMap.id, actionElementMap.elementIdentifierName, False)
					ElseIf actionSet.axisRange = AxisRange.Negative Then
						If actionElementMap.axisRange = AxisRange.Full Then
							Continue For
						End If
						If actionElementMap.axisContribution = Pole.Positive Then
							Continue For
						End If
						Me.inputGrid.PopulateField(Me.currentMapCategoryId, actionSet.actionId, actionSet.axisRange, controllerType, controllerId, num, actionElementMap.id, actionElementMap.elementIdentifierName, False)
					End If
				End If
				num += 1
				If num > maxFields Then
					Exit For
				End If
			Next
		End Sub

		' Token: 0x06004A65 RID: 19045 RVA: 0x0026C0F0 File Offset: 0x0026A4F0
		Private Sub DisableInputFieldGroup(actionSet As ControlMapper.InputActionSet, controllerType As ControllerType, fieldCount As Integer)
			For i As Integer = 0 To fieldCount - 1
				Dim guiinputField As ControlMapper.GUIInputField = Me.inputGrid.GetGUIInputField(Me.currentMapCategoryId, actionSet.actionId, actionSet.axisRange, controllerType, i)
				If guiinputField IsNot Nothing Then
					guiinputField.SetInteractible(False, False)
				End If
			Next
		End Sub

		' Token: 0x06004A66 RID: 19046 RVA: 0x0026C144 File Offset: 0x0026A544
		Private Sub CreateLayout()
			Me.references.playersGroup.gameObject.SetActive(Me.showPlayers)
			Me.references.controllerGroup.gameObject.SetActive(Me._showControllers)
			Me.references.removeControllerButton.gameObject.SetActive(Me.showControllerGroupButtons)
			Me.references.assignControllerButton.gameObject.SetActive(Me.showControllerGroupButtons)
			Me.references.assignedControllersGroup.gameObject.SetActive(Me._showControllers AndAlso Me.ShowAssignedControllers())
			Me.references.settingsAndMapCategoriesGroup.gameObject.SetActive(Me.showSettings OrElse Me.showMapCategories)
			Me.references.settingsGroup.gameObject.SetActive(Me.showSettings)
			Me.references.mapCategoriesGroup.gameObject.SetActive(Me.showMapCategories)
		End Sub

		' Token: 0x06004A67 RID: 19047 RVA: 0x0026C245 File Offset: 0x0026A645
		Private Sub Draw()
			Me.DrawPlayersGroup()
			Me.DrawControllersGroup()
			Me.DrawSettingsGroup()
			Me.DrawMapCategoriesGroup()
			Me.DrawWindowButtonsGroup()
		End Sub

		' Token: 0x06004A68 RID: 19048 RVA: 0x0026C268 File Offset: 0x0026A668
		Private Sub DrawPlayersGroup()
			Me.references.playersGroup.labelText = Me._language.playersGroupLabel
			Me.references.playersGroup.SetLabelActive(Me._showPlayersGroupLabel)
			For i As Integer = 0 To Me.playerCount - 1
				Dim player As Player = ReInput.players.GetPlayer(i)
				If player IsNot Nothing Then
					Dim gameObject As GameObject = UITools.InstantiateGUIObject(Of ButtonInfo)(Me.prefabs.playerButton, Me.references.playersGroup.content, "Player" + i + "Button")
					Dim guibutton As ControlMapper.GUIButton = New ControlMapper.GUIButton(gameObject)
					guibutton.SetLabel(Localization.Translate(player.descriptiveName).text)
					guibutton.SetButtonInfoData("PlayerSelection", player.id)
					guibutton.SetOnClickCallback(AddressOf Me.OnButtonActivated)
					AddHandler guibutton.buttonInfo.OnSelectedEvent, AddressOf Me.OnUIElementSelected
					guibutton.gameObject.GetComponent(Of CustomButtonPlayerSelect)().mapper = Me
					Me.playerButtons.Add(guibutton)
				End If
			Next
			Me.playerButtons(0).SetInteractible(False, True)
		End Sub

		' Token: 0x06004A69 RID: 19049 RVA: 0x0026C395 File Offset: 0x0026A795
		Public Function GetUnselectedPlayerButton() As Selectable
			Return Me.playerButtons(1 - Me.currentPlayerId).gameObject.GetComponent(Of Selectable)()
		End Function

		' Token: 0x06004A6A RID: 19050 RVA: 0x0026C3B4 File Offset: 0x0026A7B4
		Private Sub DrawControllersGroup()
			If Not Me._showControllers Then
				Return
			End If
			Me.references.controllerSettingsGroup.labelText = Me._language.controllerSettingsGroupLabel
			Me.references.controllerSettingsGroup.SetLabelActive(Me._showControllerGroupLabel)
			Me.references.controllerNameLabel.gameObject.SetActive(Me._showControllerNameLabel)
			Me.references.controllerGroupLabelGroup.gameObject.SetActive(Me._showControllerGroupLabel OrElse Me._showControllerNameLabel)
			If Me.ShowAssignedControllers() Then
				Me.references.assignedControllersGroup.labelText = Me._language.assignedControllersGroupLabel
				Me.references.assignedControllersGroup.SetLabelActive(Me._showAssignedControllersGroupLabel)
			End If
			Dim buttonInfo As ButtonInfo = Me.references.removeControllerButton.GetComponent(Of ButtonInfo)()
			buttonInfo.text.text = Me._language.removeControllerButtonLabel
			buttonInfo = Me.references.calibrateControllerButton.GetComponent(Of ButtonInfo)()
			buttonInfo.text.text = Me._language.calibrateControllerButtonLabel
			buttonInfo = Me.references.assignControllerButton.GetComponent(Of ButtonInfo)()
			buttonInfo.text.text = Me._language.assignControllerButtonLabel
			Dim guibutton As ControlMapper.GUIButton = Me.CreateButton(Me._language.none, Me.references.assignedControllersGroup.content, Vector2.zero)
			guibutton.SetInteractible(False, False, True)
			Me.assignedControllerButtonsPlaceholder = guibutton
		End Sub

		' Token: 0x06004A6B RID: 19051 RVA: 0x0026C52C File Offset: 0x0026A92C
		Private Sub DrawSettingsGroup()
			If Not Me.showSettings Then
				Return
			End If
			Me.references.settingsGroup.labelText = Me._language.settingsGroupLabel
			Me.references.settingsGroup.SetLabelActive(Me._showSettingsGroupLabel)
			Dim guibutton As ControlMapper.GUIButton = Me.CreateButton(Me._language.inputBehaviorSettingsButtonLabel, Me.references.settingsGroup.content, Vector2.zero)
			Me.miscInstantiatedObjects.Add(guibutton.gameObject)
			AddHandler guibutton.buttonInfo.OnSelectedEvent, AddressOf Me.OnUIElementSelected
			guibutton.SetButtonInfoData("EditInputBehaviors", 0)
			guibutton.SetOnClickCallback(AddressOf Me.OnButtonActivated)
		End Sub

		' Token: 0x06004A6C RID: 19052 RVA: 0x0026C5E4 File Offset: 0x0026A9E4
		Private Sub DrawMapCategoriesGroup()
			If Not Me.showMapCategories Then
				Return
			End If
			If Me._mappingSets Is Nothing Then
				Return
			End If
			Me.references.mapCategoriesGroup.labelText = Me._language.mapCategoriesGroupLabel
			Me.references.mapCategoriesGroup.SetLabelActive(Me._showMapCategoriesGroupLabel)
			For i As Integer = 0 To Me._mappingSets.Length - 1
				Dim mappingSet As ControlMapper.MappingSet = Me._mappingSets(i)
				If mappingSet IsNot Nothing Then
					Dim mapCategory As InputMapCategory = ReInput.mapping.GetMapCategory(mappingSet.mapCategoryId)
					If mapCategory IsNot Nothing Then
						Dim gameObject As GameObject = UITools.InstantiateGUIObject(Of ButtonInfo)(Me.prefabs.button, Me.references.mapCategoriesGroup.content, mapCategory.name + "Button")
						Dim guibutton As ControlMapper.GUIButton = New ControlMapper.GUIButton(gameObject)
						guibutton.SetLabel(Localization.Translate(mapCategory.descriptiveName).text)
						guibutton.SetButtonInfoData("MapCategorySelection", mapCategory.id)
						guibutton.SetOnClickCallback(AddressOf Me.OnButtonActivated)
						AddHandler guibutton.buttonInfo.OnSelectedEvent, AddressOf Me.OnUIElementSelected
						Me.mapCategoryButtons.Add(guibutton)
					End If
				End If
			Next
		End Sub

		' Token: 0x06004A6D RID: 19053 RVA: 0x0026C724 File Offset: 0x0026AB24
		Private Sub DrawWindowButtonsGroup()
			Me.references.doneButton.GetComponent(Of ButtonInfo)().text.text = Me._language.doneButtonLabel
			Me.references.restoreDefaultsButton.GetComponent(Of ButtonInfo)().text.text = Me._language.restoreDefaultsButtonLabel
			Me.UpdateRumbleText()
		End Sub

		' Token: 0x06004A6E RID: 19054 RVA: 0x0026C784 File Offset: 0x0026AB84
		Private Sub Redraw(listsChanged As Boolean, playTransitions As Boolean)
			Me.RedrawPlayerGroup(playTransitions)
			Me.RedrawControllerGroup()
			Me.RedrawMapCategoriesGroup(playTransitions)
			Me.RedrawInputGrid(listsChanged)
			If Me.currentUISelection Is Nothing OrElse Not Me.currentUISelection.activeInHierarchy Then
				Me.RestoreLastUISelection()
			End If
		End Sub

		' Token: 0x06004A6F RID: 19055 RVA: 0x0026C7D4 File Offset: 0x0026ABD4
		Private Sub RedrawPlayerGroup(playTransitions As Boolean)
			If Not Me.showPlayers Then
				Return
			End If
			For i As Integer = 0 To Me.playerButtons.Count - 1
				Dim flag As Boolean = Me.currentPlayerId <> Me.playerButtons(i).buttonInfo.intData
				Me.playerButtons(i).SetInteractible(flag, playTransitions)
				Me.playerButtons(i).gameObject.GetComponent(Of CustomButton)().SetNavOnToggle(flag)
			Next
			Me.playerButtons(1).SetInteractible(PlayerManager.Multiplayer, playTransitions)
		End Sub

		' Token: 0x06004A70 RID: 19056 RVA: 0x0026C874 File Offset: 0x0026AC74
		Private Sub RedrawControllerGroup()
			Dim num As Integer = -1
			Me.references.controllerNameLabel.text = Me._language.none
			UITools.SetInteractable(Me.references.removeControllerButton, False, False)
			UITools.SetInteractable(Me.references.assignControllerButton, False, False)
			UITools.SetInteractable(Me.references.calibrateControllerButton, False, False)
			If Me.ShowAssignedControllers() Then
				For Each guibutton As ControlMapper.GUIButton In Me.assignedControllerButtons
					If Not(guibutton.gameObject Is Nothing) Then
						If Me.currentUISelection Is guibutton.gameObject Then
							num = guibutton.buttonInfo.intData
						End If
						Global.UnityEngine.[Object].Destroy(guibutton.gameObject)
					End If
				Next
				Me.assignedControllerButtons.Clear()
				Me.assignedControllerButtonsPlaceholder.SetActive(True)
			End If
			Dim player As Player = ReInput.players.GetPlayer(Me.currentPlayerId)
			If player Is Nothing Then
				Return
			End If
			If Me.ShowAssignedControllers() Then
				If player.controllers.joystickCount > 0 Then
					Me.assignedControllerButtonsPlaceholder.SetActive(False)
				End If
				For Each joystick As Joystick In player.controllers.Joysticks
					Dim guibutton2 As ControlMapper.GUIButton = Me.CreateButton(joystick.name, Me.references.assignedControllersGroup.content, Vector2.zero)
					guibutton2.SetButtonInfoData("AssignedControllerSelection", joystick.id)
					guibutton2.SetOnClickCallback(AddressOf Me.OnButtonActivated)
					AddHandler guibutton2.buttonInfo.OnSelectedEvent, AddressOf Me.OnUIElementSelected
					Me.assignedControllerButtons.Add(guibutton2)
					If joystick.id = Me.currentJoystickId Then
						guibutton2.SetInteractible(False, True)
					End If
				Next
				If player.controllers.joystickCount > 0 AndAlso Not Me.isJoystickSelected Then
					Me.currentJoystickId = player.controllers.Joysticks(0).id
					Me.assignedControllerButtons(0).SetInteractible(False, False)
				End If
				If num >= 0 Then
					For Each guibutton3 As ControlMapper.GUIButton In Me.assignedControllerButtons
						If guibutton3.buttonInfo.intData = num Then
							Me.SetUISelection(guibutton3.gameObject)
							Exit For
						End If
					Next
				End If
			ElseIf player.controllers.joystickCount > 0 AndAlso Not Me.isJoystickSelected Then
				Me.currentJoystickId = player.controllers.Joysticks(0).id
			End If
			If Me.isJoystickSelected AndAlso player.controllers.joystickCount > 0 AndAlso Me.currentPlayerId = 0 Then
				Me.references.removeControllerButton.interactable = True
				Me.references.controllerNameLabel.text = Me.currentJoystick.name
				If Me.currentJoystick.axisCount > 0 Then
					Me.references.calibrateControllerButton.interactable = True
				End If
			End If
			Dim joystickCount As Integer = player.controllers.joystickCount
			Dim joystickCount2 As Integer = ReInput.controllers.joystickCount
			Dim maxControllersPerPlayer As Integer = Me.GetMaxControllersPerPlayer()
			Dim flag As Boolean = maxControllersPerPlayer = 0
			If joystickCount2 > 0 AndAlso joystickCount < joystickCount2 AndAlso (maxControllersPerPlayer = 1 OrElse flag OrElse joystickCount < maxControllersPerPlayer) Then
				UITools.SetInteractable(Me.references.assignControllerButton, True, False)
			End If
		End Sub

		' Token: 0x06004A71 RID: 19057 RVA: 0x0026CC68 File Offset: 0x0026B068
		Private Sub RedrawMapCategoriesGroup(playTransitions As Boolean)
			If Not Me.showMapCategories Then
				Return
			End If
			For i As Integer = 0 To Me.mapCategoryButtons.Count - 1
				Dim flag As Boolean = Me.currentMapCategoryId <> Me.mapCategoryButtons(i).buttonInfo.intData
				Me.mapCategoryButtons(i).SetInteractible(flag, playTransitions)
			Next
		End Sub

		' Token: 0x06004A72 RID: 19058 RVA: 0x0026CCD2 File Offset: 0x0026B0D2
		Private Sub RedrawInputGrid(listsChanged As Boolean)
			If listsChanged Then
				Me.RefreshInputGridStructure()
			End If
			Me.PopulateInputFields()
		End Sub

		' Token: 0x06004A73 RID: 19059 RVA: 0x0026CCE6 File Offset: 0x0026B0E6
		Private Sub ForceRefresh()
			If Me.windowManager.isWindowOpen Then
				Me.CloseAllWindows()
			Else
				Me.Redraw(False, False)
			End If
		End Sub

		' Token: 0x06004A74 RID: 19060 RVA: 0x0026CD0C File Offset: 0x0026B10C
		Private Sub CreateInputCategoryRow(ByRef rowCount As Integer, category As InputCategory)
			Me.CreateLabel(Localization.Translate(category.descriptiveName).text, Me.references.inputGridActionColumn, New Vector2(0F, CSng(rowCount) * Me._inputRowHeight * -1F))
			rowCount += 1
		End Sub

		' Token: 0x06004A75 RID: 19061 RVA: 0x0026CD5E File Offset: 0x0026B15E
		Private Function CreateLabel(labelText As String, parent As Transform, offset As Vector2) As ControlMapper.GUILabel
			Return Me.CreateLabel(Me.prefabs.inputGridLabel, labelText, parent, offset)
		End Function

		' Token: 0x06004A76 RID: 19062 RVA: 0x0026CD74 File Offset: 0x0026B174
		Private Function CreateDeactivatedLabel(labelText As String, parent As Transform, offset As Vector2) As ControlMapper.GUILabel
			Return Me.CreateLabel(Me.prefabs.inputGridDeactivatedLabel, labelText, parent, offset)
		End Function

		' Token: 0x06004A77 RID: 19063 RVA: 0x0026CD8C File Offset: 0x0026B18C
		Private Function CreateLabel(prefab As GameObject, labelText As String, parent As Transform, offset As Vector2) As ControlMapper.GUILabel
			Dim gameObject As GameObject = Me.InstantiateGUIObject(prefab, parent, offset)
			Dim componentInSelfOrChildren As Text = UnityTools.GetComponentInSelfOrChildren(Of Text)(gameObject)
			If componentInSelfOrChildren Is Nothing Then
				Global.UnityEngine.Debug.LogError("Rewired Control Mapper: Label prefab is missing Text component!")
				Return Nothing
			End If
			componentInSelfOrChildren.text = labelText
			Return New ControlMapper.GUILabel(gameObject)
		End Function

		' Token: 0x06004A78 RID: 19064 RVA: 0x0026CDD0 File Offset: 0x0026B1D0
		Private Function CreateButton(labelText As String, parent As Transform, offset As Vector2) As ControlMapper.GUIButton
			Dim guibutton As ControlMapper.GUIButton = New ControlMapper.GUIButton(Me.InstantiateGUIObject(Me.prefabs.button, parent, offset))
			guibutton.SetLabel(labelText)
			Return guibutton
		End Function

		' Token: 0x06004A79 RID: 19065 RVA: 0x0026CE00 File Offset: 0x0026B200
		Private Function CreateFitButton(labelText As String, parent As Transform, offset As Vector2) As ControlMapper.GUIButton
			Dim guibutton As ControlMapper.GUIButton = New ControlMapper.GUIButton(Me.InstantiateGUIObject(Me.prefabs.fitButton, parent, offset))
			guibutton.SetLabel(labelText)
			Return guibutton
		End Function

		' Token: 0x06004A7A RID: 19066 RVA: 0x0026CE30 File Offset: 0x0026B230
		Private Function CreateInputField(parent As Transform, offset As Vector2, label As String, actionId As Integer, axisRange As AxisRange, controllerType As ControllerType, fieldIndex As Integer) As ControlMapper.GUIInputField
			Dim guiinputField As ControlMapper.GUIInputField = Me.CreateInputField(parent, offset)
			guiinputField.SetLabel(String.Empty)
			guiinputField.SetFieldInfoData(actionId, axisRange, controllerType, fieldIndex)
			guiinputField.SetOnClickCallback(Me.inputFieldActivatedDelegate)
			AddHandler guiinputField.fieldInfo.OnSelectedEvent, AddressOf Me.OnUIElementSelected
			Return guiinputField
		End Function

		' Token: 0x06004A7B RID: 19067 RVA: 0x0026CE83 File Offset: 0x0026B283
		Private Function CreateInputField(parent As Transform, offset As Vector2) As ControlMapper.GUIInputField
			Return New ControlMapper.GUIInputField(Me.InstantiateGUIObject(Me.prefabs.inputGridFieldButton, parent, offset))
		End Function

		' Token: 0x06004A7C RID: 19068 RVA: 0x0026CEA0 File Offset: 0x0026B2A0
		Private Function CreateToggle(prefab As GameObject, parent As Transform, offset As Vector2, label As String, actionId As Integer, axisRange As AxisRange, controllerType As ControllerType, fieldIndex As Integer) As ControlMapper.GUIToggle
			Dim guitoggle As ControlMapper.GUIToggle = Me.CreateToggle(prefab, parent, offset)
			guitoggle.SetToggleInfoData(actionId, axisRange, controllerType, fieldIndex)
			guitoggle.SetOnSubmitCallback(Me.inputFieldInvertToggleStateChangedDelegate)
			AddHandler guitoggle.toggleInfo.OnSelectedEvent, AddressOf Me.OnUIElementSelected
			Return guitoggle
		End Function

		' Token: 0x06004A7D RID: 19069 RVA: 0x0026CEE9 File Offset: 0x0026B2E9
		Private Function CreateToggle(prefab As GameObject, parent As Transform, offset As Vector2) As ControlMapper.GUIToggle
			Return New ControlMapper.GUIToggle(Me.InstantiateGUIObject(prefab, parent, offset))
		End Function

		' Token: 0x06004A7E RID: 19070 RVA: 0x0026CEFC File Offset: 0x0026B2FC
		Private Function InstantiateGUIObject(prefab As GameObject, parent As Transform, offset As Vector2) As GameObject
			If prefab Is Nothing Then
				Global.UnityEngine.Debug.LogError("Rewired Control Mapper: Prefab is null!")
				Return Nothing
			End If
			Dim gameObject As GameObject = Global.UnityEngine.[Object].Instantiate(Of GameObject)(prefab)
			Return Me.InitializeNewGUIGameObject(gameObject, parent, offset)
		End Function

		' Token: 0x06004A7F RID: 19071 RVA: 0x0026CF34 File Offset: 0x0026B334
		Private Function CreateNewGUIObject(name As String, parent As Transform, offset As Vector2) As GameObject
			Dim gameObject As GameObject = New GameObject()
			gameObject.name = name
			gameObject.AddComponent(Of RectTransform)()
			Return Me.InitializeNewGUIGameObject(gameObject, parent, offset)
		End Function

		' Token: 0x06004A80 RID: 19072 RVA: 0x0026CF60 File Offset: 0x0026B360
		Private Function InitializeNewGUIGameObject(gameObject As GameObject, parent As Transform, offset As Vector2) As GameObject
			If gameObject Is Nothing Then
				Global.UnityEngine.Debug.LogError("Rewired Control Mapper: GameObject is null!")
				Return Nothing
			End If
			Dim component As RectTransform = gameObject.GetComponent(Of RectTransform)()
			If component Is Nothing Then
				Global.UnityEngine.Debug.LogError("Rewired Control Mapper: GameObject does not have a RectTransform component!")
				Return gameObject
			End If
			If parent IsNot Nothing Then
				component.SetParent(parent, False)
			End If
			component.anchoredPosition = offset
			Return gameObject
		End Function

		' Token: 0x06004A81 RID: 19073 RVA: 0x0026CFC0 File Offset: 0x0026B3C0
		Private Function CreateNewColumnGroup(name As String, parent As Transform, maxWidth As Integer) As GameObject
			Dim gameObject As GameObject = Me.CreateNewGUIObject(name, parent, Vector2.zero)
			Me.inputGrid.AddGroup(gameObject)
			Dim layoutElement As LayoutElement = gameObject.AddComponent(Of LayoutElement)()
			If maxWidth >= 0 Then
				layoutElement.preferredWidth = CSng(maxWidth)
			End If
			Dim component As RectTransform = gameObject.GetComponent(Of RectTransform)()
			component.anchorMin = New Vector2(0F, 0F)
			component.anchorMax = New Vector2(1F, 0F)
			Return gameObject
		End Function

		' Token: 0x06004A82 RID: 19074 RVA: 0x0026D02F File Offset: 0x0026B42F
		Private Function OpenWindow(closeOthers As Boolean) As Window
			Return Me.OpenWindow(String.Empty, closeOthers)
		End Function

		' Token: 0x06004A83 RID: 19075 RVA: 0x0026D040 File Offset: 0x0026B440
		Private Function OpenWindow(name As String, closeOthers As Boolean) As Window
			If closeOthers Then
				Me.windowManager.CancelAll()
			End If
			Dim window As Window = Me.windowManager.OpenWindow(name, Me._defaultWindowWidth, Me._defaultWindowHeight)
			If window Is Nothing Then
				Return Nothing
			End If
			Me.ChildWindowOpened()
			Return window
		End Function

		' Token: 0x06004A84 RID: 19076 RVA: 0x0026D08C File Offset: 0x0026B48C
		Private Function OpenWindow(windowPrefab As GameObject, closeOthers As Boolean) As Window
			Return Me.OpenWindow(windowPrefab, String.Empty, closeOthers)
		End Function

		' Token: 0x06004A85 RID: 19077 RVA: 0x0026D09C File Offset: 0x0026B49C
		Private Function OpenWindow(windowPrefab As GameObject, name As String, closeOthers As Boolean) As Window
			If closeOthers Then
				Me.windowManager.CancelAll()
			End If
			Dim window As Window = Me.windowManager.OpenWindow(windowPrefab, name)
			If window Is Nothing Then
				Return Nothing
			End If
			Me.ChildWindowOpened()
			Return window
		End Function

		' Token: 0x06004A86 RID: 19078 RVA: 0x0026D0E0 File Offset: 0x0026B4E0
		Private Sub OpenModal(title As String, message As String, confirmText As String, confirmAction As Action(Of Integer), cancelText As String, cancelAction As Action(Of Integer), closeOthers As Boolean)
			Dim window As Window = Me.OpenWindow(closeOthers)
			If window Is Nothing Then
				Return
			End If
			window.CreateTitleText(Me.prefabs.windowTitleText, Vector2.zero, title)
			window.AddContentText(Me.prefabs.windowContentText, UIPivot.TopCenter, UIAnchor.TopHStretch, New Vector2(0F, -100F), message)
			Dim unityAction As UnityAction = Sub()
				Me.OnWindowCancel(window.id)
			End Sub
			window.cancelCallback = unityAction
			window.CreateButton(Me.prefabs.fitButton, UIPivot.BottomLeft, UIAnchor.BottomLeft, Vector2.zero, confirmText, Sub()
				Me.OnRestoreDefaultsConfirmed(window.id)
			End Sub, unityAction, False)
			window.CreateButton(Me.prefabs.fitButton, UIPivot.BottomRight, UIAnchor.BottomRight, Vector2.zero, cancelText, unityAction, unityAction, True)
			Me.windowManager.Focus(window)
		End Sub

		' Token: 0x06004A87 RID: 19079 RVA: 0x0026D1EE File Offset: 0x0026B5EE
		Private Sub CloseWindow(windowId As Integer)
			If Not Me.windowManager.isWindowOpen Then
				Return
			End If
			Me.windowManager.CloseWindow(windowId)
			Me.ChildWindowClosed()
		End Sub

		' Token: 0x06004A88 RID: 19080 RVA: 0x0026D213 File Offset: 0x0026B613
		Private Sub CloseTopWindow()
			If Not Me.windowManager.isWindowOpen Then
				Return
			End If
			Me.windowManager.CloseTop()
			Me.ChildWindowClosed()
		End Sub

		' Token: 0x06004A89 RID: 19081 RVA: 0x0026D237 File Offset: 0x0026B637
		Private Sub CloseAllWindows()
			If Not Me.windowManager.isWindowOpen Then
				Return
			End If
			Me.windowManager.CancelAll()
			Me.ChildWindowClosed()
			Me.InputPollingStopped()
		End Sub

		' Token: 0x06004A8A RID: 19082 RVA: 0x0026D264 File Offset: 0x0026B664
		Private Sub ChildWindowOpened()
			If Not Me.windowManager.isWindowOpen Then
				Return
			End If
			Me.SetIsFocused(False)
			If Me._PopupWindowOpenedEvent IsNot Nothing Then
				Me._PopupWindowOpenedEvent()
			End If
			If Me._onPopupWindowOpened IsNot Nothing Then
				Me._onPopupWindowOpened.Invoke()
			End If
		End Sub

		' Token: 0x06004A8B RID: 19083 RVA: 0x0026D2B8 File Offset: 0x0026B6B8
		Private Sub ChildWindowClosed()
			If Me.windowManager.isWindowOpen Then
				Return
			End If
			Me.SetIsFocused(True)
			If Me._PopupWindowClosedEvent IsNot Nothing Then
				Me._PopupWindowClosedEvent()
			End If
			If Me._onPopupWindowClosed IsNot Nothing Then
				Me._onPopupWindowClosed.Invoke()
			End If
		End Sub

		' Token: 0x06004A8C RID: 19084 RVA: 0x0026D30C File Offset: 0x0026B70C
		Private Function HasElementAssignmentConflicts(player As Player, mapping As ControlMapper.InputMapping, assignment As ElementAssignment, skipOtherPlayers As Boolean) As Boolean
			If player Is Nothing OrElse mapping Is Nothing Then
				Return False
			End If
			Dim elementAssignmentConflictCheck As ElementAssignmentConflictCheck
			If Not Me.CreateConflictCheck(mapping, assignment, elementAssignmentConflictCheck) Then
				Return False
			End If
			If skipOtherPlayers Then
				Return ReInput.players.SystemPlayer.controllers.conflictChecking.DoesElementAssignmentConflict(elementAssignmentConflictCheck) OrElse player.controllers.conflictChecking.DoesElementAssignmentConflict(elementAssignmentConflictCheck)
			End If
			Return ReInput.controllers.conflictChecking.DoesElementAssignmentConflict(elementAssignmentConflictCheck)
		End Function

		' Token: 0x06004A8D RID: 19085 RVA: 0x0026D38C File Offset: 0x0026B78C
		Private Function IsBlockingAssignmentConflict(mapping As ControlMapper.InputMapping, assignment As ElementAssignment, skipOtherPlayers As Boolean) As Boolean
			Dim elementAssignmentConflictCheck As ElementAssignmentConflictCheck
			If Not Me.CreateConflictCheck(mapping, assignment, elementAssignmentConflictCheck) Then
				Return False
			End If
			If skipOtherPlayers Then
				For Each elementAssignmentConflictInfo As ElementAssignmentConflictInfo In ReInput.players.SystemPlayer.controllers.conflictChecking.ElementAssignmentConflicts(elementAssignmentConflictCheck)
					If Not elementAssignmentConflictInfo.isUserAssignable Then
						Return True
					End If
				Next
				For Each elementAssignmentConflictInfo2 As ElementAssignmentConflictInfo In Me.currentPlayer.controllers.conflictChecking.ElementAssignmentConflicts(elementAssignmentConflictCheck)
					If Not elementAssignmentConflictInfo2.isUserAssignable Then
						Return True
					End If
				Next
			Else
				For Each elementAssignmentConflictInfo3 As ElementAssignmentConflictInfo In ReInput.controllers.conflictChecking.ElementAssignmentConflicts(elementAssignmentConflictCheck)
					If Not elementAssignmentConflictInfo3.isUserAssignable Then
						Return True
					End If
				Next
			End If
			Return False
		End Function

		' Token: 0x06004A8E RID: 19086 RVA: 0x0026D4F4 File Offset: 0x0026B8F4
		Private Iterator Function ElementAssignmentConflicts(player As Player, mapping As ControlMapper.InputMapping, assignment As ElementAssignment, skipOtherPlayers As Boolean) As IEnumerable(Of ElementAssignmentConflictInfo)
			If player Is Nothing OrElse mapping Is Nothing Then
				Return
			End If
			Dim conflictCheck As ElementAssignmentConflictCheck
			If Not Me.CreateConflictCheck(mapping, assignment, conflictCheck) Then
				Return
			End If
			If skipOtherPlayers Then
				For Each conflict As ElementAssignmentConflictInfo In ReInput.players.SystemPlayer.controllers.conflictChecking.ElementAssignmentConflicts(conflictCheck)
					If Not conflict.isUserAssignable Then
						Yield conflict
					End If
				Next
				For Each conflict2 As ElementAssignmentConflictInfo In player.controllers.conflictChecking.ElementAssignmentConflicts(conflictCheck)
					If Not conflict2.isUserAssignable Then
						Yield conflict2
					End If
				Next
			Else
				For Each conflict3 As ElementAssignmentConflictInfo In ReInput.controllers.conflictChecking.ElementAssignmentConflicts(conflictCheck)
					If Not conflict3.isUserAssignable Then
						Yield conflict3
					End If
				Next
			End If
			Return
		End Function

		' Token: 0x06004A8F RID: 19087 RVA: 0x0026D534 File Offset: 0x0026B934
		Private Function CreateConflictCheck(mapping As ControlMapper.InputMapping, assignment As ElementAssignment, <System.Runtime.InteropServices.OutAttribute()> ByRef conflictCheck As ElementAssignmentConflictCheck) As Boolean
			If mapping Is Nothing OrElse Me.currentPlayer Is Nothing Then
				conflictCheck = Nothing
				Return False
			End If
			conflictCheck = assignment.ToElementAssignmentConflictCheck()
			conflictCheck.playerId = Me.currentPlayer.id
			conflictCheck.controllerType = mapping.controllerType
			conflictCheck.controllerId = mapping.controllerId
			conflictCheck.controllerMapId = mapping.map.id
			conflictCheck.controllerMapCategoryId = mapping.map.categoryId
			If mapping.aem IsNot Nothing Then
				conflictCheck.elementMapId = mapping.aem.id
			End If
			Return True
		End Function

		' Token: 0x06004A90 RID: 19088 RVA: 0x0026D5D0 File Offset: 0x0026B9D0
		Private Sub PollKeyboardForAssignment(<System.Runtime.InteropServices.OutAttribute()> ByRef pollingInfo As ControllerPollingInfo, <System.Runtime.InteropServices.OutAttribute()> ByRef modifierKeyPressed As Boolean, <System.Runtime.InteropServices.OutAttribute()> ByRef modifierFlags As ModifierKeyFlags, <System.Runtime.InteropServices.OutAttribute()> ByRef label As String)
			pollingInfo = Nothing
			label = String.Empty
			modifierKeyPressed = False
			modifierFlags = ModifierKeyFlags.None
			Dim num As Integer = 0
			Dim controllerPollingInfo As ControllerPollingInfo = Nothing
			Dim controllerPollingInfo2 As ControllerPollingInfo = Nothing
			Dim modifierKeyFlags As ModifierKeyFlags = ModifierKeyFlags.None
			For Each controllerPollingInfo3 As ControllerPollingInfo In ReInput.controllers.Keyboard.PollForAllKeys()
				Dim keyboardKey As KeyCode = controllerPollingInfo3.keyboardKey
				If keyboardKey <> KeyCode.AltGr Then
					If Keyboard.IsModifierKey(controllerPollingInfo3.keyboardKey) Then
						If num = 0 Then
							controllerPollingInfo2 = controllerPollingInfo3
							modifierKeyFlags = modifierKeyFlags Or Keyboard.KeyCodeToModifierKeyFlags(keyboardKey)
							num += 1
						End If
					ElseIf controllerPollingInfo.keyboardKey = KeyCode.None Then
						controllerPollingInfo = controllerPollingInfo3
					End If
				End If
			Next
			If controllerPollingInfo.keyboardKey = KeyCode.None Then
				If num > 0 Then
					modifierKeyPressed = True
					If num = 1 Then
						If ReInput.controllers.Keyboard.GetKeyTimePressed(controllerPollingInfo2.keyboardKey) > 1F Then
							pollingInfo = controllerPollingInfo2
							Return
						End If
						label = Localization.Translate(Keyboard.GetKeyName(controllerPollingInfo2.keyboardKey)).text
					Else
						label = Keyboard.ModifierKeyFlagsToString(modifierKeyFlags)
					End If
				End If
				Return
			End If
			If Not ReInput.controllers.Keyboard.GetKeyDown(controllerPollingInfo.keyboardKey) Then
				Return
			End If
			If num = 0 Then
				pollingInfo = controllerPollingInfo
				Return
			End If
			pollingInfo = controllerPollingInfo
			modifierFlags = modifierKeyFlags
		End Sub

		' Token: 0x06004A91 RID: 19089 RVA: 0x0026D75C File Offset: 0x0026BB5C
		Private Sub StartAxisCalibration(axisIndex As Integer)
			If Me.currentPlayer Is Nothing Then
				Return
			End If
			If Me.currentPlayer.controllers.joystickCount = 0 Then
				Return
			End If
			Dim currentJoystick As Joystick = Me.currentJoystick
			If axisIndex < 0 OrElse axisIndex >= currentJoystick.axisCount Then
				Return
			End If
			Me.pendingAxisCalibration = New ControlMapper.AxisCalibrator(currentJoystick, axisIndex)
			Me.ShowCalibrateAxisStep1Window()
		End Sub

		' Token: 0x06004A92 RID: 19090 RVA: 0x0026D7B9 File Offset: 0x0026BBB9
		Private Sub EndAxisCalibration()
			If Me.pendingAxisCalibration Is Nothing Then
				Return
			End If
			Me.pendingAxisCalibration.Commit()
			Me.pendingAxisCalibration = Nothing
		End Sub

		' Token: 0x06004A93 RID: 19091 RVA: 0x0026D7D9 File Offset: 0x0026BBD9
		Private Sub SetUISelection(selection As GameObject)
			If EventSystem.current Is Nothing Then
				Return
			End If
			EventSystem.current.SetSelectedGameObject(selection)
		End Sub

		' Token: 0x06004A94 RID: 19092 RVA: 0x0026D7F7 File Offset: 0x0026BBF7
		Private Sub RestoreLastUISelection()
			If Me.lastUISelection Is Nothing OrElse Not Me.lastUISelection.activeInHierarchy Then
				Me.SetDefaultUISelection()
				Return
			End If
			Me.SetUISelection(Me.lastUISelection)
		End Sub

		' Token: 0x06004A95 RID: 19093 RVA: 0x0026D830 File Offset: 0x0026BC30
		Private Sub SetDefaultUISelection()
			If Not Me.isOpen Then
				Return
			End If
			If Me.references.defaultSelection Is Nothing Then
				Me.SetUISelection(Nothing)
			Else
				Me.SetUISelection(Me.references.defaultSelection.gameObject)
			End If
		End Sub

		' Token: 0x06004A96 RID: 19094 RVA: 0x0026D884 File Offset: 0x0026BC84
		Private Sub SelectDefaultMapCategory(redraw As Boolean)
			Me.currentMapCategoryId = Me.GetDefaultMapCategoryId()
			Me.OnMapCategorySelected(Me.currentMapCategoryId, redraw)
			If Not Me.showMapCategories Then
				Return
			End If
			For i As Integer = 0 To Me._mappingSets.Length - 1
				If ReInput.mapping.GetMapCategory(Me._mappingSets(i).mapCategoryId) IsNot Nothing Then
					Me.currentMapCategoryId = Me._mappingSets(i).mapCategoryId
					Exit For
				End If
			Next
			If Me.currentMapCategoryId < 0 Then
				Return
			End If
			For j As Integer = 0 To Me._mappingSets.Length - 1
				Dim flag As Boolean = Me._mappingSets(j).mapCategoryId <> Me.currentMapCategoryId
				Me.mapCategoryButtons(j).SetInteractible(flag, False)
			Next
		End Sub

		' Token: 0x06004A97 RID: 19095 RVA: 0x0026D963 File Offset: 0x0026BD63
		Private Sub CheckUISelection()
			If Not Me.isFocused Then
				Return
			End If
			If Me.currentUISelection Is Nothing Then
				Me.RestoreLastUISelection()
			End If
		End Sub

		' Token: 0x06004A98 RID: 19096 RVA: 0x0026D988 File Offset: 0x0026BD88
		Private Sub OnUIElementSelected(selectedObject As GameObject)
			Me.lastUISelection = selectedObject
		End Sub

		' Token: 0x06004A99 RID: 19097 RVA: 0x0026D991 File Offset: 0x0026BD91
		Private Sub SetIsFocused(state As Boolean)
			Me.references.mainCanvasGroup.interactable = state
			If state Then
				Me.Redraw(False, False)
				Me.RestoreLastUISelection()
				Me.blockInputOnFocusEndTime = Time.unscaledTime + 0.1F
			End If
		End Sub

		' Token: 0x06004A9A RID: 19098 RVA: 0x0026D9C9 File Offset: 0x0026BDC9
		Public Sub Toggle()
			If Me.isOpen Then
				Me.Close(True)
			Else
				Me.Open()
			End If
		End Sub

		' Token: 0x06004A9B RID: 19099 RVA: 0x0026D9E8 File Offset: 0x0026BDE8
		Public Sub Open()
			Me.Open(False)
		End Sub

		' Token: 0x06004A9C RID: 19100 RVA: 0x0026D9F4 File Offset: 0x0026BDF4
		Private Sub Open(force As Boolean)
			If Not Me.initialized Then
				Me.Initialize()
			End If
			If Not Me.initialized Then
				Return
			End If
			If Not force AndAlso Me.isOpen Then
				Return
			End If
			Me.Clear()
			Me.canvas.SetActive(True)
			Me.OnPlayerSelected(0, False)
			Me.SelectDefaultMapCategory(False)
			Me.SetDefaultUISelection()
			Me.Redraw(True, False)
			If Me._ScreenOpenedEvent IsNot Nothing Then
				Me._ScreenOpenedEvent()
			End If
			If Me._onScreenOpened IsNot Nothing Then
				Me._onScreenOpened.Invoke()
			End If
		End Sub

		' Token: 0x06004A9D RID: 19101 RVA: 0x0026DA8C File Offset: 0x0026BE8C
		Public Sub Close(save As Boolean)
			If Not Me.initialized Then
				Return
			End If
			If Not Me.isOpen Then
				Return
			End If
			If save AndAlso ReInput.userDataStore IsNot Nothing Then
				ReInput.userDataStore.Save()
			End If
			Me.Clear()
			Me.canvas.SetActive(False)
			Me.SetUISelection(Nothing)
			If Me._ScreenClosedEvent IsNot Nothing Then
				Me._ScreenClosedEvent()
			End If
			If Me._onScreenClosed IsNot Nothing Then
				Me._onScreenClosed.Invoke()
			End If
		End Sub

		' Token: 0x06004A9E RID: 19102 RVA: 0x0026DB10 File Offset: 0x0026BF10
		Private Sub Clear()
			Me.windowManager.CancelAll()
			Me.lastUISelection = Nothing
			Me.pendingInputMapping = Nothing
			Me.pendingAxisCalibration = Nothing
			Me.InputPollingStopped()
		End Sub

		' Token: 0x06004A9F RID: 19103 RVA: 0x0026DB38 File Offset: 0x0026BF38
		Private Sub ClearCompletely()
			Me.ClearSpawnedObjects()
			Me.ClearAllVars()
		End Sub

		' Token: 0x06004AA0 RID: 19104 RVA: 0x0026DB48 File Offset: 0x0026BF48
		Private Sub ClearSpawnedObjects()
			Me.windowManager.ClearCompletely()
			Me.inputGrid.ClearAll()
			For Each guibutton As ControlMapper.GUIButton In Me.playerButtons
				Global.UnityEngine.[Object].Destroy(guibutton.gameObject)
			Next
			Me.playerButtons.Clear()
			For Each guibutton2 As ControlMapper.GUIButton In Me.mapCategoryButtons
				Global.UnityEngine.[Object].Destroy(guibutton2.gameObject)
			Next
			Me.mapCategoryButtons.Clear()
			For Each guibutton3 As ControlMapper.GUIButton In Me.assignedControllerButtons
				Global.UnityEngine.[Object].Destroy(guibutton3.gameObject)
			Next
			Me.assignedControllerButtons.Clear()
			If Me.assignedControllerButtonsPlaceholder IsNot Nothing Then
				Global.UnityEngine.[Object].Destroy(Me.assignedControllerButtonsPlaceholder.gameObject)
				Me.assignedControllerButtonsPlaceholder = Nothing
			End If
			For Each gameObject As GameObject In Me.miscInstantiatedObjects
				Global.UnityEngine.[Object].Destroy(gameObject)
			Next
			Me.miscInstantiatedObjects.Clear()
		End Sub

		' Token: 0x06004AA1 RID: 19105 RVA: 0x0026DCFC File Offset: 0x0026C0FC
		Private Sub ClearVarsOnPlayerChange()
			Me.currentJoystickId = -1
		End Sub

		' Token: 0x06004AA2 RID: 19106 RVA: 0x0026DD05 File Offset: 0x0026C105
		Private Sub ClearVarsOnJoystickChange()
			Me.currentJoystickId = -1
		End Sub

		' Token: 0x06004AA3 RID: 19107 RVA: 0x0026DD10 File Offset: 0x0026C110
		Private Sub ClearAllVars()
			Me.initialized = False
			ControlMapper.Instance = Nothing
			Me.playerCount = 0
			Me.inputGrid = Nothing
			Me.windowManager = Nothing
			Me.currentPlayerId = -1
			Me.currentMapCategoryId = -1
			Me.playerButtons = Nothing
			Me.mapCategoryButtons = Nothing
			Me.miscInstantiatedObjects = Nothing
			Me.canvas = Nothing
			Me.lastUISelection = Nothing
			Me.currentJoystickId = -1
			Me.axisToggleObjects.Clear()
			Me.inactiveAxisToggleObjects.Clear()
			Me.pendingInputMapping = Nothing
			Me.pendingAxisCalibration = Nothing
			Me.inputFieldActivatedDelegate = Nothing
			Me.inputFieldInvertToggleStateChangedDelegate = Nothing
			Me.isPollingForInput = False
		End Sub

		' Token: 0x06004AA4 RID: 19108 RVA: 0x0026DDB0 File Offset: 0x0026C1B0
		Public Sub Reset()
			If Not Me.initialized Then
				Return
			End If
			Me.ClearCompletely()
			Me.Initialize()
			If Me.isOpen Then
				Me.Open(True)
			End If
		End Sub

		' Token: 0x06004AA5 RID: 19109 RVA: 0x0026DDDC File Offset: 0x0026C1DC
		Private Sub SetActionAxisInverted(state As Boolean, controllerType As ControllerType, actionElementMapId As Integer)
			If Me.currentPlayer Is Nothing Then
				Return
			End If
			Dim controllerMapWithAxes As ControllerMapWithAxes = TryCast(Me.GetControllerMap(controllerType), ControllerMapWithAxes)
			If controllerMapWithAxes Is Nothing Then
				Return
			End If
			Dim elementMap As ActionElementMap = controllerMapWithAxes.GetElementMap(actionElementMapId)
			If elementMap Is Nothing Then
				Return
			End If
			elementMap.invert = state
		End Sub

		' Token: 0x06004AA6 RID: 19110 RVA: 0x0026DE20 File Offset: 0x0026C220
		Private Function GetControllerMap(type As ControllerType) As ControllerMap
			If Me.currentPlayer Is Nothing Then
				Return Nothing
			End If
			Dim num As Integer = 0
			Select Case type
				Case ControllerType.Keyboard
				Case ControllerType.Mouse
				Case ControllerType.Joystick
					If Me.currentPlayer.controllers.joystickCount <= 0 Then
						Return Nothing
					End If
					num = Me.currentJoystick.id
				Case Else
					Throw New NotImplementedException()
			End Select
			Return Me.currentPlayer.controllers.maps.GetFirstMapInCategory(type, num, Me.currentMapCategoryId)
		End Function

		' Token: 0x06004AA7 RID: 19111 RVA: 0x0026DEB0 File Offset: 0x0026C2B0
		Private Function GetControllerMapOrCreateNew(controllerType As ControllerType, controllerId As Integer, layoutId As Integer) As ControllerMap
			Dim controllerMap As ControllerMap = Me.GetControllerMap(controllerType)
			If controllerMap Is Nothing Then
				Me.currentPlayer.controllers.maps.AddEmptyMap(controllerType, controllerId, Me.currentMapCategoryId, layoutId)
				controllerMap = Me.currentPlayer.controllers.maps.GetMap(controllerType, controllerId, Me.currentMapCategoryId, layoutId)
			End If
			Return controllerMap
		End Function

		' Token: 0x06004AA8 RID: 19112 RVA: 0x0026DF0C File Offset: 0x0026C30C
		Private Function CountIEnumerable(Of T)(enumerable As IEnumerable(Of T)) As Integer
			If enumerable Is Nothing Then
				Return 0
			End If
			Dim enumerator As IEnumerator(Of T) = enumerable.GetEnumerator()
			If enumerator Is Nothing Then
				Return 0
			End If
			Dim num As Integer = 0
			While enumerator.MoveNext()
				num += 1
			End While
			Return num
		End Function

		' Token: 0x06004AA9 RID: 19113 RVA: 0x0026DF48 File Offset: 0x0026C348
		Private Function GetDefaultMapCategoryId() As Integer
			If Me._mappingSets.Length = 0 Then
				Return 0
			End If
			For i As Integer = 0 To Me._mappingSets.Length - 1
				If ReInput.mapping.GetMapCategory(Me._mappingSets(i).mapCategoryId) IsNot Nothing Then
					Return Me._mappingSets(i).mapCategoryId
				End If
			Next
			Return 0
		End Function

		' Token: 0x06004AAA RID: 19114 RVA: 0x0026DFB0 File Offset: 0x0026C3B0
		Private Sub SubscribeFixedUISelectionEvents()
			If Me.references.fixedSelectableUIElements Is Nothing Then
				Return
			End If
			For Each gameObject As GameObject In Me.references.fixedSelectableUIElements
				Dim component As UIElementInfo = UnityTools.GetComponent(Of UIElementInfo)(gameObject)
				If Not(component Is Nothing) Then
					AddHandler component.OnSelectedEvent, AddressOf Me.OnUIElementSelected
				End If
			Next
		End Sub

		' Token: 0x06004AAB RID: 19115 RVA: 0x0026E01C File Offset: 0x0026C41C
		Private Sub SubscribeMenuControlInputEvents()
			Me.SubscribeRewiredInputEventAllPlayers(Me._screenToggleAction, AddressOf Me.OnScreenToggleActionPressed)
			Me.SubscribeRewiredInputEventAllPlayers(Me._screenOpenAction, AddressOf Me.OnScreenOpenActionPressed)
			Me.SubscribeRewiredInputEventAllPlayers(Me._screenCloseAction, AddressOf Me.OnScreenCloseActionPressed)
			Me.SubscribeRewiredInputEventAllPlayers(Me._universalCancelAction, AddressOf Me.OnUniversalCancelActionPressed)
		End Sub

		' Token: 0x06004AAC RID: 19116 RVA: 0x0026E08C File Offset: 0x0026C48C
		Private Sub UnsubscribeMenuControlInputEvents()
			Me.UnsubscribeRewiredInputEventAllPlayers(Me._screenToggleAction, AddressOf Me.OnScreenToggleActionPressed)
			Me.UnsubscribeRewiredInputEventAllPlayers(Me._screenOpenAction, AddressOf Me.OnScreenOpenActionPressed)
			Me.UnsubscribeRewiredInputEventAllPlayers(Me._screenCloseAction, AddressOf Me.OnScreenCloseActionPressed)
			Me.UnsubscribeRewiredInputEventAllPlayers(Me._universalCancelAction, AddressOf Me.OnUniversalCancelActionPressed)
		End Sub

		' Token: 0x06004AAD RID: 19117 RVA: 0x0026E0FC File Offset: 0x0026C4FC
		Private Sub SubscribeRewiredInputEventAllPlayers(actionId As Integer, callback As Action(Of InputActionEventData))
			If actionId < 0 OrElse callback Is Nothing Then
				Return
			End If
			If ReInput.mapping.GetAction(actionId) Is Nothing Then
				Global.UnityEngine.Debug.LogWarning("Rewired Control Mapper: " + actionId + " is not a valid Action id!")
				Return
			End If
			For Each player As Player In ReInput.players.AllPlayers
				player.AddInputEventDelegate(callback, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, actionId)
			Next
		End Sub

		' Token: 0x06004AAE RID: 19118 RVA: 0x0026E198 File Offset: 0x0026C598
		Private Sub UnsubscribeRewiredInputEventAllPlayers(actionId As Integer, callback As Action(Of InputActionEventData))
			If actionId < 0 OrElse callback Is Nothing Then
				Return
			End If
			If Not ReInput.isReady Then
				Return
			End If
			If ReInput.mapping.GetAction(actionId) Is Nothing Then
				Global.UnityEngine.Debug.LogWarning("Rewired Control Mapper: " + actionId + " is not a valid Action id!")
				Return
			End If
			For Each player As Player In ReInput.players.AllPlayers
				player.RemoveInputEventDelegate(callback, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, actionId)
			Next
		End Sub

		' Token: 0x06004AAF RID: 19119 RVA: 0x0026E23C File Offset: 0x0026C63C
		Private Function GetMaxControllersPerPlayer() As Integer
			If Me._rewiredInputManager.userData.ConfigVars.autoAssignJoysticks Then
				Return Me._rewiredInputManager.userData.ConfigVars.maxJoysticksPerPlayer
			End If
			Return Me._maxControllersPerPlayer
		End Function

		' Token: 0x06004AB0 RID: 19120 RVA: 0x0026E274 File Offset: 0x0026C674
		Private Function ShowAssignedControllers() As Boolean
			Return Me._showControllers AndAlso (Me._showAssignedControllers OrElse Me.GetMaxControllersPerPlayer() <> 1)
		End Function

		' Token: 0x06004AB1 RID: 19121 RVA: 0x0026E29F File Offset: 0x0026C69F
		Private Sub InspectorPropertyChanged(Optional reset As Boolean = False)
			If reset Then
				Me.Reset()
			End If
		End Sub

		' Token: 0x06004AB2 RID: 19122 RVA: 0x0026E2B0 File Offset: 0x0026C6B0
		Private Sub AssignController(player As Player, controllerId As Integer)
			If player Is Nothing Then
				Return
			End If
			If player.controllers.ContainsController(ControllerType.Joystick, controllerId) Then
				Return
			End If
			If Me.GetMaxControllersPerPlayer() = 1 Then
				Me.RemoveAllControllers(player)
				Me.ClearVarsOnJoystickChange()
			End If
			For Each player2 As Player In ReInput.players.Players
				If player2 IsNot player Then
					Me.RemoveController(player2, controllerId)
				End If
			Next
			player.controllers.AddController(ControllerType.Joystick, controllerId, False)
			PlayerManager.ControllerRemapped(If((Me.currentPlayerId <> 0), PlayerId.PlayerTwo, PlayerId.PlayerOne), True, controllerId)
			Me.OnPlayerSelected(Me.currentPlayerId, True)
			If ReInput.userDataStore IsNot Nothing Then
				ReInput.userDataStore.LoadControllerData(player.id, ControllerType.Joystick, controllerId)
			End If
		End Sub

		' Token: 0x06004AB3 RID: 19123 RVA: 0x0026E3A0 File Offset: 0x0026C7A0
		Private Sub RemoveAllControllers(player As Player)
			If player Is Nothing Then
				Return
			End If
			Dim joysticks As IList(Of Joystick) = player.controllers.Joysticks
			For i As Integer = joysticks.Count - 1 To 0 Step -1
				Me.RemoveController(player, joysticks(i).id)
			Next
		End Sub

		' Token: 0x06004AB4 RID: 19124 RVA: 0x0026E3EC File Offset: 0x0026C7EC
		Private Sub RemoveController(player As Player, controllerId As Integer)
			If player Is Nothing Then
				Return
			End If
			If Not player.controllers.ContainsController(ControllerType.Joystick, controllerId) Then
				Return
			End If
			If ReInput.userDataStore IsNot Nothing Then
				ReInput.userDataStore.SaveControllerData(player.id, ControllerType.Joystick, controllerId)
			End If
			player.controllers.RemoveController(ControllerType.Joystick, controllerId)
			PlayerManager.ControllerRemapped(If((Me.currentPlayerId <> 0), PlayerId.PlayerTwo, PlayerId.PlayerOne), False, 0)
			Me.OnPlayerSelected(Me.currentPlayerId, True)
		End Sub

		' Token: 0x06004AB5 RID: 19125 RVA: 0x0026E462 File Offset: 0x0026C862
		Private Function IsAllowedAssignment(pendingInputMapping As ControlMapper.InputMapping, pollingInfo As ControllerPollingInfo) As Boolean
			Return pendingInputMapping IsNot Nothing AndAlso (pendingInputMapping.axisRange <> AxisRange.Full OrElse Me._showSplitAxisInputFields OrElse pollingInfo.elementType <> ControllerElementType.Button)
		End Function

		' Token: 0x06004AB6 RID: 19126 RVA: 0x0026E494 File Offset: 0x0026C894
		Private Sub InputPollingStarted()
			Dim flag As Boolean = Me.isPollingForInput
			Me.isPollingForInput = True
			If Not flag Then
				If Me._InputPollingStartedEvent IsNot Nothing Then
					Me._InputPollingStartedEvent()
				End If
				If Me._onInputPollingStarted IsNot Nothing Then
					Me._onInputPollingStarted.Invoke()
				End If
			End If
		End Sub

		' Token: 0x06004AB7 RID: 19127 RVA: 0x0026E4E4 File Offset: 0x0026C8E4
		Private Sub InputPollingStopped()
			Dim flag As Boolean = Me.isPollingForInput
			Me.isPollingForInput = False
			If flag Then
				If Me._InputPollingEndedEvent IsNot Nothing Then
					Me._InputPollingEndedEvent()
				End If
				If Me._onInputPollingEnded IsNot Nothing Then
					Me._onInputPollingEnded.Invoke()
				End If
			End If
		End Sub

		' Token: 0x06004AB8 RID: 19128 RVA: 0x0026E531 File Offset: 0x0026C931
		Private Sub OnControlsChanged()
			If MyBase.gameObject.activeInHierarchy Then
				Me.Redraw(False, False)
			End If
		End Sub

		' Token: 0x06004AB9 RID: 19129 RVA: 0x0026E54C File Offset: 0x0026C94C
		Public Shared Sub ApplyTheme(elementInfo As ThemedElement.ElementInfo())
			If ControlMapper.Instance Is Nothing Then
				Return
			End If
			If ControlMapper.Instance._themeSettings Is Nothing Then
				Return
			End If
			If Not ControlMapper.Instance._useThemeSettings Then
				Return
			End If
			ControlMapper.Instance._themeSettings(Mathf.Clamp(ControlMapper.Instance.currentPlayerId, 0, 1)).Apply(elementInfo)
		End Sub

		' Token: 0x06004ABA RID: 19130 RVA: 0x0026E5AC File Offset: 0x0026C9AC
		Public Shared Function GetLanguage() As LanguageData
			If ControlMapper.Instance Is Nothing Then
				Return Nothing
			End If
			Return ControlMapper.Instance._language
		End Function

		' Token: 0x06004ABB RID: 19131 RVA: 0x0026E5CA File Offset: 0x0026C9CA
		Public Shared Function CurrentPlayer() As Integer
			Return ControlMapper.Instance.currentPlayerId
		End Function

		' Token: 0x04004FB6 RID: 20406
		Private Const blockInputOnFocusTimeout As Single = 0.1F

		' Token: 0x04004FB7 RID: 20407
		Private Const buttonIdentifier_playerSelection As String = "PlayerSelection"

		' Token: 0x04004FB8 RID: 20408
		Private Const buttonIdentifier_removeController As String = "RemoveController"

		' Token: 0x04004FB9 RID: 20409
		Private Const buttonIdentifier_assignController As String = "AssignController"

		' Token: 0x04004FBA RID: 20410
		Private Const buttonIdentifier_calibrateController As String = "CalibrateController"

		' Token: 0x04004FBB RID: 20411
		Private Const buttonIdentifier_editInputBehaviors As String = "EditInputBehaviors"

		' Token: 0x04004FBC RID: 20412
		Private Const buttonIdentifier_mapCategorySelection As String = "MapCategorySelection"

		' Token: 0x04004FBD RID: 20413
		Private Const buttonIdentifier_assignedControllerSelection As String = "AssignedControllerSelection"

		' Token: 0x04004FBE RID: 20414
		Private Const buttonIdentifier_done As String = "Done"

		' Token: 0x04004FBF RID: 20415
		Private Const buttonIdentifier_restoreDefaults As String = "RestoreDefaults"

		' Token: 0x04004FC0 RID: 20416
		Private Const buttonIdentifier_toggleRumble As String = "ToggleRumble"

		' Token: 0x04004FC1 RID: 20417
		<SerializeField()>
		<Tooltip("Must be assigned a Rewired Input Manager scene object or prefab.")>
		Private _rewiredInputManager As InputManager

		' Token: 0x04004FC2 RID: 20418
		<SerializeField()>
		<Tooltip("Set to True to prevent the Game Object from being destroyed when a new scene is loaded." & vbLf & vbLf & "NOTE: Changing this value from True to False at runtime will have no effect because Object.DontDestroyOnLoad cannot be undone once set.")>
		Private _dontDestroyOnLoad As Boolean

		' Token: 0x04004FC3 RID: 20419
		<SerializeField()>
		<Tooltip("Open the control mapping screen immediately on start. Mainly used for testing.")>
		Private _openOnStart As Boolean

		' Token: 0x04004FC4 RID: 20420
		<SerializeField()>
		<Tooltip("The Layout of the Keyboard Maps to be displayed.")>
		Private _keyboardMapDefaultLayout As Integer

		' Token: 0x04004FC5 RID: 20421
		<SerializeField()>
		<Tooltip("The Layout of the Mouse Maps to be displayed.")>
		Private _mouseMapDefaultLayout As Integer

		' Token: 0x04004FC6 RID: 20422
		<SerializeField()>
		<Tooltip("The Layout of the Mouse Maps to be displayed.")>
		Private _joystickMapDefaultLayout As Integer

		' Token: 0x04004FC7 RID: 20423
		<SerializeField()>
		Private _mappingSets As ControlMapper.MappingSet() = New ControlMapper.MappingSet() { ControlMapper.MappingSet.[Default] }

		' Token: 0x04004FC8 RID: 20424
		<SerializeField()>
		<Tooltip("Display a selectable list of Players. If your game only supports 1 player, you can disable this.")>
		Private _showPlayers As Boolean = True

		' Token: 0x04004FC9 RID: 20425
		<SerializeField()>
		<Tooltip("Display the Controller column for input mapping.")>
		Private _showControllers As Boolean = True

		' Token: 0x04004FCA RID: 20426
		<SerializeField()>
		<Tooltip("Display the Keyboard column for input mapping.")>
		Private _showKeyboard As Boolean = True

		' Token: 0x04004FCB RID: 20427
		<SerializeField()>
		<Tooltip("Display the Mouse column for input mapping.")>
		Private _showMouse As Boolean = True

		' Token: 0x04004FCC RID: 20428
		<SerializeField()>
		<Tooltip("The maximum number of controllers allowed to be assigned to a Player. If set to any value other than 1, a selectable list of currently-assigned controller will be displayed to the user. [0 = infinite]")>
		Private _maxControllersPerPlayer As Integer = 1

		' Token: 0x04004FCD RID: 20429
		<SerializeField()>
		<Tooltip("Display section labels for each Action Category in the input field grid. Only applies if Action Categories are used to display the Action list.")>
		Private _showActionCategoryLabels As Boolean

		' Token: 0x04004FCE RID: 20430
		<SerializeField()>
		<Tooltip("The number of input fields to display for the keyboard. If you want to support alternate mappings on the same device, set this to 2 or more.")>
		Private _keyboardInputFieldCount As Integer = 2

		' Token: 0x04004FCF RID: 20431
		<SerializeField()>
		<Tooltip("The number of input fields to display for the mouse. If you want to support alternate mappings on the same device, set this to 2 or more.")>
		Private _mouseInputFieldCount As Integer = 1

		' Token: 0x04004FD0 RID: 20432
		<SerializeField()>
		<Tooltip("The number of input fields to display for joysticks. If you want to support alternate mappings on the same device, set this to 2 or more.")>
		Private _controllerInputFieldCount As Integer = 1

		' Token: 0x04004FD1 RID: 20433
		<SerializeField()>
		<Tooltip("Display a full-axis input assignment field for every axis-type Action in the input field grid. Also displays an invert toggle for the user  to invert the full-axis assignment direction." & vbLf & vbLf & "*IMPORTANT*: This field is required if you have made any full-axis assignments in the Rewired Input Manager or in saved XML user data. Disabling this field when you have full-axis assignments will result in the inability for the user to view, remove, or modify these full-axis assignments. In addition, these assignments may cause conflicts when trying to remap the same axes to Actions.")>
		Private _showFullAxisInputFields As Boolean = True

		' Token: 0x04004FD2 RID: 20434
		<SerializeField()>
		<Tooltip("Display a positive and negative input assignment field for every axis-type Action in the input field grid." & vbLf & vbLf & "*IMPORTANT*: These fields are required to assign buttons, keyboard keys, and hat or D-Pad directions to axis-type Actions. If you have made any split-axis assignments or button/key/D-pad assignments to axis-type Actions in the Rewired Input Manager or in saved XML user data, disabling these fields will result in the inability for the user to view, remove, or modify these assignments. In addition, these assignments may cause conflicts when trying to remap the same elements to Actions.")>
		Private _showSplitAxisInputFields As Boolean = True

		' Token: 0x04004FD3 RID: 20435
		<SerializeField()>
		<Tooltip("If enabled, when an element assignment conflict is found, an option will be displayed that allows the user to make the conflicting assignment anyway.")>
		Private _allowElementAssignmentConflicts As Boolean

		' Token: 0x04004FD4 RID: 20436
		<SerializeField()>
		<Tooltip("The width in relative pixels of the Action label column.")>
		Private _actionLabelWidth As Integer = 360

		' Token: 0x04004FD5 RID: 20437
		<SerializeField()>
		<Tooltip("The width in relative pixels of the Keyboard column.")>
		Private _keyboardColMaxWidth As Integer = 360

		' Token: 0x04004FD6 RID: 20438
		<SerializeField()>
		<Tooltip("The width in relative pixels of the Mouse column.")>
		Private _mouseColMaxWidth As Integer = 200

		' Token: 0x04004FD7 RID: 20439
		<SerializeField()>
		<Tooltip("The width in relative pixels of the Controller column.")>
		Private _controllerColMaxWidth As Integer = 200

		' Token: 0x04004FD8 RID: 20440
		<SerializeField()>
		<Tooltip("The height in relative pixels of the input grid button rows.")>
		Private _inputRowHeight As Single = 40F

		' Token: 0x04004FD9 RID: 20441
		<SerializeField()>
		<Tooltip("The width in relative pixels of spacing between columns.")>
		Private _inputColumnSpacing As Single = 40F

		' Token: 0x04004FDA RID: 20442
		<SerializeField()>
		<Tooltip("The height in relative pixels of the space between Action Category sections. Only applicable if Show Action Category Labels is checked.")>
		Private _inputRowCategorySpacing As Integer = 20

		' Token: 0x04004FDB RID: 20443
		<SerializeField()>
		<Tooltip("The width in relative pixels of the invert toggle buttons.")>
		Private _invertToggleWidth As Integer = 40

		' Token: 0x04004FDC RID: 20444
		<SerializeField()>
		<Tooltip("The width in relative pixels of generated popup windows.")>
		Private _defaultWindowWidth As Integer = 500

		' Token: 0x04004FDD RID: 20445
		<SerializeField()>
		<Tooltip("The height in relative pixels of generated popup windows.")>
		Private _defaultWindowHeight As Integer = 400

		' Token: 0x04004FDE RID: 20446
		<SerializeField()>
		<Tooltip("The time in seconds the user has to press an element on a controller when assigning a controller to a Player. If this time elapses with no user input a controller, the assignment will be canceled.")>
		Private _controllerAssignmentTimeout As Single = 5F

		' Token: 0x04004FDF RID: 20447
		<SerializeField()>
		<Tooltip("The time in seconds the user has to press an element on a controller while waiting for axes to be centered before assigning input.")>
		Private _preInputAssignmentTimeout As Single = 5F

		' Token: 0x04004FE0 RID: 20448
		<SerializeField()>
		<Tooltip("The time in seconds the user has to press an element on a controller when assigning input. If this time elapses with no user input on the target controller, the assignment will be canceled.")>
		Private _inputAssignmentTimeout As Single = 5F

		' Token: 0x04004FE1 RID: 20449
		<SerializeField()>
		<Tooltip("The time in seconds the user has to press an element on a controller during calibration.")>
		Private _axisCalibrationTimeout As Single = 5F

		' Token: 0x04004FE2 RID: 20450
		<SerializeField()>
		<Tooltip("If checked, mouse X-axis movement will always be ignored during input assignment. Check this if you don't want the horizontal mouse axis to be user-assignable to any Actions.")>
		Private _ignoreMouseXAxisAssignment As Boolean = True

		' Token: 0x04004FE3 RID: 20451
		<SerializeField()>
		<Tooltip("If checked, mouse Y-axis movement will always be ignored during input assignment. Check this if you don't want the vertical mouse axis to be user-assignable to any Actions.")>
		Private _ignoreMouseYAxisAssignment As Boolean = True

		' Token: 0x04004FE4 RID: 20452
		<SerializeField()>
		<Tooltip("An Action that when activated will alternately close or open the main screen as long as no popup windows are open.")>
		Private _screenToggleAction As Integer = -1

		' Token: 0x04004FE5 RID: 20453
		<SerializeField()>
		<Tooltip("An Action that when activated will open the main screen if it is closed.")>
		Private _screenOpenAction As Integer = -1

		' Token: 0x04004FE6 RID: 20454
		<SerializeField()>
		<Tooltip("An Action that when activated will close the main screen as long as no popup windows are open.")>
		Private _screenCloseAction As Integer = -1

		' Token: 0x04004FE7 RID: 20455
		<SerializeField()>
		<Tooltip("An Action that when activated will cancel and close any open popup window. Use with care because the element assigned to this Action can never be mapped by the user (because it would just cancel his assignment).")>
		Private _universalCancelAction As Integer = -1

		' Token: 0x04004FE8 RID: 20456
		<SerializeField()>
		<Tooltip("If enabled, Universal Cancel will also close the main screen if pressed when no windows are open.")>
		Private _universalCancelClosesScreen As Boolean = True

		' Token: 0x04004FE9 RID: 20457
		<SerializeField()>
		<Tooltip("If checked, controls will be displayed which will allow the user to customize certain Input Behavior settings.")>
		Private _showInputBehaviorSettings As Boolean

		' Token: 0x04004FEA RID: 20458
		<SerializeField()>
		<Tooltip("Customizable settings for user-modifiable Input Behaviors. This can be used for settings like Mouse Look Sensitivity.")>
		Private _inputBehaviorSettings As ControlMapper.InputBehaviorSettings()

		' Token: 0x04004FEB RID: 20459
		<SerializeField()>
		<Tooltip("If enabled, UI elements will be themed based on the settings in Theme Settings.")>
		Private _useThemeSettings As Boolean = True

		' Token: 0x04004FEC RID: 20460
		<SerializeField()>
		<Tooltip("Must be assigned a ThemeSettings object. Used to theme UI elements.")>
		Private _themeSettings As ThemeSettings()

		' Token: 0x04004FED RID: 20461
		<SerializeField()>
		<Tooltip("Must be assigned a LanguageData object. Used to retrieve language entries for UI elements.")>
		Private _language As LanguageData

		' Token: 0x04004FEE RID: 20462
		<SerializeField()>
		<Tooltip("A list of prefabs. You should not have to modify this.")>
		Private prefabs As ControlMapper.Prefabs

		' Token: 0x04004FEF RID: 20463
		<SerializeField()>
		<Tooltip("A list of references to elements in the hierarchy. You should not have to modify this.")>
		Private references As ControlMapper.References

		' Token: 0x04004FF0 RID: 20464
		<SerializeField()>
		<Tooltip("Show the label for the Players button group?")>
		Private _showPlayersGroupLabel As Boolean = True

		' Token: 0x04004FF1 RID: 20465
		<SerializeField()>
		<Tooltip("Show the label for the Controller button group?")>
		Private _showControllerGroupLabel As Boolean = True

		' Token: 0x04004FF2 RID: 20466
		<SerializeField()>
		<Tooltip("Show the label for the Assigned Controllers button group?")>
		Private _showAssignedControllersGroupLabel As Boolean = True

		' Token: 0x04004FF3 RID: 20467
		<SerializeField()>
		<Tooltip("Show the label for the Settings button group?")>
		Private _showSettingsGroupLabel As Boolean = True

		' Token: 0x04004FF4 RID: 20468
		<SerializeField()>
		<Tooltip("Show the label for the Map Categories button group?")>
		Private _showMapCategoriesGroupLabel As Boolean = True

		' Token: 0x04004FF5 RID: 20469
		<SerializeField()>
		<Tooltip("Show the label for the current controller name?")>
		Private _showControllerNameLabel As Boolean = True

		' Token: 0x04004FF6 RID: 20470
		<SerializeField()>
		<Tooltip("Show the Assigned Controllers group? If joystick auto-assignment is enabled in the Rewired Input Manager and the max joysticks per player is set to any value other than 1, the Assigned Controllers group will always be displayed.")>
		Private _showAssignedControllers As Boolean = True

		' Token: 0x04004FF7 RID: 20471
		<SerializeField()>
		Private _rumbleButtonText As Text

		' Token: 0x04004FF8 RID: 20472
		Private axisToggleObjects As List(Of GameObject) = New List(Of GameObject)()

		' Token: 0x04004FF9 RID: 20473
		Private inactiveAxisToggleObjects As List(Of GameObject) = New List(Of GameObject)()

		' Token: 0x04004FFA RID: 20474
		Private _ScreenClosedEvent As Action

		' Token: 0x04004FFB RID: 20475
		Private _ScreenOpenedEvent As Action

		' Token: 0x04004FFC RID: 20476
		Private _PopupWindowOpenedEvent As Action

		' Token: 0x04004FFD RID: 20477
		Private _PopupWindowClosedEvent As Action

		' Token: 0x04004FFE RID: 20478
		Private _InputPollingStartedEvent As Action

		' Token: 0x04004FFF RID: 20479
		Private _InputPollingEndedEvent As Action

		' Token: 0x04005000 RID: 20480
		<SerializeField()>
		<Tooltip("Event sent when the UI is closed.")>
		Private _onScreenClosed As UnityEvent

		' Token: 0x04005001 RID: 20481
		<SerializeField()>
		<Tooltip("Event sent when the UI is opened.")>
		Private _onScreenOpened As UnityEvent

		' Token: 0x04005002 RID: 20482
		<SerializeField()>
		<Tooltip("Event sent when a popup window is closed.")>
		Private _onPopupWindowClosed As UnityEvent

		' Token: 0x04005003 RID: 20483
		<SerializeField()>
		<Tooltip("Event sent when a popup window is opened.")>
		Private _onPopupWindowOpened As UnityEvent

		' Token: 0x04005004 RID: 20484
		<SerializeField()>
		<Tooltip("Event sent when polling for input has started.")>
		Private _onInputPollingStarted As UnityEvent

		' Token: 0x04005005 RID: 20485
		<SerializeField()>
		<Tooltip("Event sent when polling for input has ended.")>
		Private _onInputPollingEnded As UnityEvent

		' Token: 0x04005006 RID: 20486
		Private Shared Instance As ControlMapper

		' Token: 0x04005007 RID: 20487
		Private initialized As Boolean

		' Token: 0x04005008 RID: 20488
		Private playerCount As Integer

		' Token: 0x04005009 RID: 20489
		Private inputGrid As ControlMapper.InputGrid

		' Token: 0x0400500A RID: 20490
		Private windowManager As ControlMapper.WindowManager

		' Token: 0x0400500B RID: 20491
		Public currentPlayerId As Integer

		' Token: 0x0400500C RID: 20492
		Private currentMapCategoryId As Integer

		' Token: 0x0400500D RID: 20493
		Private playerButtons As List(Of ControlMapper.GUIButton)

		' Token: 0x0400500E RID: 20494
		Private mapCategoryButtons As List(Of ControlMapper.GUIButton)

		' Token: 0x0400500F RID: 20495
		Private assignedControllerButtons As List(Of ControlMapper.GUIButton)

		' Token: 0x04005010 RID: 20496
		Private assignedControllerButtonsPlaceholder As ControlMapper.GUIButton

		' Token: 0x04005011 RID: 20497
		Private miscInstantiatedObjects As List(Of GameObject)

		' Token: 0x04005012 RID: 20498
		Private canvas As GameObject

		' Token: 0x04005013 RID: 20499
		Private lastUISelection As GameObject

		' Token: 0x04005014 RID: 20500
		Private currentJoystickId As Integer = -1

		' Token: 0x04005015 RID: 20501
		Private blockInputOnFocusEndTime As Single

		' Token: 0x04005016 RID: 20502
		Private isPollingForInput As Boolean

		' Token: 0x04005017 RID: 20503
		Private pendingInputMapping As ControlMapper.InputMapping

		' Token: 0x04005018 RID: 20504
		Private pendingAxisCalibration As ControlMapper.AxisCalibrator

		' Token: 0x04005019 RID: 20505
		Private inputFieldActivatedDelegate As Action(Of InputFieldInfo)

		' Token: 0x0400501A RID: 20506
		Private inputFieldInvertToggleStateChangedDelegate As Action(Of ToggleInfo, Boolean)

		' Token: 0x0400501B RID: 20507
		Private _restoreDefaultsDelegate As Action

		' Token: 0x02000C0E RID: 3086
		' (Invoke) Token: 0x06004ABD RID: 19133
		Public Delegate Sub PlayerChangeAction()

		' Token: 0x02000C0F RID: 3087
		Private MustInherit Class GUIElement
			' Token: 0x06004AC0 RID: 19136 RVA: 0x0026E5D8 File Offset: 0x0026C9D8
			Public Sub New(gameObject As GameObject)
				If gameObject Is Nothing Then
					Global.UnityEngine.Debug.LogError("Rewired Control Mapper: gameObject is null!")
					Return
				End If
				Me.selectable = gameObject.GetComponent(Of Selectable)()
				If Me.selectable Is Nothing Then
					Global.UnityEngine.Debug.LogError("Rewired Control Mapper: Selectable is null!")
					Return
				End If
				Me.gameObject = gameObject
				Me.rectTransform = gameObject.GetComponent(Of RectTransform)()
				Me.text = UnityTools.GetComponentInSelfOrChildren(Of Text)(gameObject)
				Me.uiElementInfo = gameObject.GetComponent(Of UIElementInfo)()
				Me.children = New List(Of ControlMapper.GUIElement)()
			End Sub

			' Token: 0x06004AC1 RID: 19137 RVA: 0x0026E660 File Offset: 0x0026CA60
			Public Sub New(selectable As Selectable, label As Text)
				If selectable Is Nothing Then
					Global.UnityEngine.Debug.LogError("Rewired Control Mapper: Selectable is null!")
					Return
				End If
				Me.selectable = selectable
				Me.gameObject = selectable.gameObject
				Me.rectTransform = Me.gameObject.GetComponent(Of RectTransform)()
				Me.text = label
				Me.uiElementInfo = Me.gameObject.GetComponent(Of UIElementInfo)()
				Me.children = New List(Of ControlMapper.GUIElement)()
			End Sub

			' Token: 0x170006D4 RID: 1748
			' (get) Token: 0x06004AC2 RID: 19138 RVA: 0x0026E6D1 File Offset: 0x0026CAD1
			' (set) Token: 0x06004AC3 RID: 19139 RVA: 0x0026E6D9 File Offset: 0x0026CAD9
			Public Property rectTransform As RectTransform

			' Token: 0x06004AC4 RID: 19140 RVA: 0x0026E6E2 File Offset: 0x0026CAE2
			Public Overridable Sub SetInteractible(state As Boolean, playTransition As Boolean)
				Me.SetInteractible(state, playTransition, False)
			End Sub

			' Token: 0x06004AC5 RID: 19141 RVA: 0x0026E6F0 File Offset: 0x0026CAF0
			Public Overridable Sub SetInteractible(state As Boolean, playTransition As Boolean, permanent As Boolean)
				For i As Integer = 0 To Me.children.Count - 1
					If Me.children(i) IsNot Nothing Then
						Me.children(i).SetInteractible(state, playTransition, permanent)
					End If
				Next
				If Me.permanentStateSet Then
					Return
				End If
				If Me.selectable Is Nothing Then
					Return
				End If
				If permanent Then
					Me.permanentStateSet = True
				End If
				If Me.selectable.interactable = state Then
					Return
				End If
				UITools.SetInteractable(Me.selectable, state, playTransition)
			End Sub

			' Token: 0x06004AC6 RID: 19142 RVA: 0x0026E790 File Offset: 0x0026CB90
			Public Overridable Sub SetTextWidth(value As Integer)
				If Me.text Is Nothing Then
					Return
				End If
				Dim layoutElement As LayoutElement = Me.text.GetComponent(Of LayoutElement)()
				If layoutElement Is Nothing Then
					layoutElement = Me.text.gameObject.AddComponent(Of LayoutElement)()
				End If
				layoutElement.preferredWidth = CSng(value)
			End Sub

			' Token: 0x06004AC7 RID: 19143 RVA: 0x0026E7E0 File Offset: 0x0026CBE0
			Public Overridable Sub SetFirstChildObjectWidth(type As ControlMapper.LayoutElementSizeType, value As Integer)
				If Me.rectTransform.childCount = 0 Then
					Return
				End If
				Dim child As Transform = Me.rectTransform.GetChild(0)
				Dim layoutElement As LayoutElement = child.GetComponent(Of LayoutElement)()
				If layoutElement Is Nothing Then
					layoutElement = child.gameObject.AddComponent(Of LayoutElement)()
				End If
				If type = ControlMapper.LayoutElementSizeType.MinSize Then
					layoutElement.minWidth = CSng(value)
				Else
					If type <> ControlMapper.LayoutElementSizeType.PreferredSize Then
						Throw New NotImplementedException()
					End If
					layoutElement.preferredWidth = CSng(value)
				End If
			End Sub

			' Token: 0x06004AC8 RID: 19144 RVA: 0x0026E857 File Offset: 0x0026CC57
			Public Overridable Sub SetLabel(label As String)
				If Me.text Is Nothing Then
					Return
				End If
				Me.text.text = label
			End Sub

			' Token: 0x06004AC9 RID: 19145 RVA: 0x0026E877 File Offset: 0x0026CC77
			Public Overridable Function GetLabel() As String
				If Me.text Is Nothing Then
					Return String.Empty
				End If
				Return Me.text.text
			End Function

			' Token: 0x06004ACA RID: 19146 RVA: 0x0026E89B File Offset: 0x0026CC9B
			Public Overridable Sub AddChild(child As ControlMapper.GUIElement)
				Me.children.Add(child)
			End Sub

			' Token: 0x06004ACB RID: 19147 RVA: 0x0026E8A9 File Offset: 0x0026CCA9
			Public Sub SetElementInfoData(identifier As String, intData As Integer)
				If Me.uiElementInfo Is Nothing Then
					Return
				End If
				Me.uiElementInfo.identifier = identifier
				Me.uiElementInfo.intData = intData
			End Sub

			' Token: 0x06004ACC RID: 19148 RVA: 0x0026E8D5 File Offset: 0x0026CCD5
			Public Overridable Sub SetActive(state As Boolean)
				If Me.gameObject Is Nothing Then
					Return
				End If
				Me.gameObject.SetActive(state)
			End Sub

			' Token: 0x06004ACD RID: 19149 RVA: 0x0026E8F8 File Offset: 0x0026CCF8
			Protected Overridable Function Init() As Boolean
				Dim flag As Boolean = True
				For i As Integer = 0 To Me.children.Count - 1
					If Me.children(i) IsNot Nothing Then
						If Not Me.children(i).Init() Then
							flag = False
						End If
					End If
				Next
				If Me.selectable Is Nothing Then
					Global.UnityEngine.Debug.LogError("Rewired Control Mapper: UI Element is missing Selectable component!")
					flag = False
				End If
				If Me.rectTransform Is Nothing Then
					Global.UnityEngine.Debug.LogError("Rewired Control Mapper: UI Element is missing RectTransform component!")
					flag = False
				End If
				If Me.uiElementInfo Is Nothing Then
					Global.UnityEngine.Debug.LogError("Rewired Control Mapper: UI Element is missing UIElementInfo component!")
					flag = False
				End If
				Return flag
			End Function

			' Token: 0x0400501F RID: 20511
			Public gameObject As GameObject

			' Token: 0x04005020 RID: 20512
			Protected text As Text

			' Token: 0x04005021 RID: 20513
			Public selectable As Selectable

			' Token: 0x04005022 RID: 20514
			Protected uiElementInfo As UIElementInfo

			' Token: 0x04005023 RID: 20515
			Protected permanentStateSet As Boolean

			' Token: 0x04005024 RID: 20516
			Protected children As List(Of ControlMapper.GUIElement)
		End Class

		' Token: 0x02000C10 RID: 3088
		Private Class GUIButton
			Inherits ControlMapper.GUIElement

			' Token: 0x06004ACE RID: 19150 RVA: 0x0026E9A9 File Offset: 0x0026CDA9
			Public Sub New(gameObject As GameObject)
				MyBase.New(gameObject)
				If Not Me.Init() Then
					Return
				End If
			End Sub

			' Token: 0x06004ACF RID: 19151 RVA: 0x0026E9BE File Offset: 0x0026CDBE
			Public Sub New(button As Button, label As Text)
				MyBase.New(button, label)
				If Not Me.Init() Then
					Return
				End If
			End Sub

			' Token: 0x170006D5 RID: 1749
			' (get) Token: 0x06004AD0 RID: 19152 RVA: 0x0026E9D4 File Offset: 0x0026CDD4
			Protected ReadOnly Property button As Button
				Get
					Return TryCast(Me.selectable, Button)
				End Get
			End Property

			' Token: 0x170006D6 RID: 1750
			' (get) Token: 0x06004AD1 RID: 19153 RVA: 0x0026E9E1 File Offset: 0x0026CDE1
			Public ReadOnly Property buttonInfo As ButtonInfo
				Get
					Return TryCast(Me.uiElementInfo, ButtonInfo)
				End Get
			End Property

			' Token: 0x06004AD2 RID: 19154 RVA: 0x0026E9EE File Offset: 0x0026CDEE
			Public Sub SetButtonInfoData(identifier As String, intData As Integer)
				MyBase.SetElementInfoData(identifier, intData)
			End Sub

			' Token: 0x06004AD3 RID: 19155 RVA: 0x0026E9F8 File Offset: 0x0026CDF8
			Public Sub SetOnClickCallback(callback As Action(Of ButtonInfo))
				If Me.button Is Nothing Then
					Return
				End If
				Me.button.onClick.AddListener(Sub()
					callback(Me.buttonInfo)
				End Sub)
			End Sub
		End Class

		' Token: 0x02000C11 RID: 3089
		Private Class GUIInputField
			Inherits ControlMapper.GUIElement

			' Token: 0x06004AD4 RID: 19156 RVA: 0x0026EA67 File Offset: 0x0026CE67
			Public Sub New(gameObject As GameObject)
				MyBase.New(gameObject)
				If Not Me.Init() Then
					Return
				End If
			End Sub

			' Token: 0x06004AD5 RID: 19157 RVA: 0x0026EA7C File Offset: 0x0026CE7C
			Public Sub New(button As Button, label As Text)
				MyBase.New(button, label)
				If Not Me.Init() Then
					Return
				End If
			End Sub

			' Token: 0x170006D7 RID: 1751
			' (get) Token: 0x06004AD6 RID: 19158 RVA: 0x0026EA92 File Offset: 0x0026CE92
			Protected ReadOnly Property button As Button
				Get
					Return TryCast(Me.selectable, Button)
				End Get
			End Property

			' Token: 0x170006D8 RID: 1752
			' (get) Token: 0x06004AD7 RID: 19159 RVA: 0x0026EA9F File Offset: 0x0026CE9F
			Public ReadOnly Property fieldInfo As InputFieldInfo
				Get
					Return TryCast(Me.uiElementInfo, InputFieldInfo)
				End Get
			End Property

			' Token: 0x170006D9 RID: 1753
			' (get) Token: 0x06004AD8 RID: 19160 RVA: 0x0026EAAC File Offset: 0x0026CEAC
			Public ReadOnly Property hasToggle As Boolean
				Get
					Return Me.toggle IsNot Nothing
				End Get
			End Property

			' Token: 0x170006DA RID: 1754
			' (get) Token: 0x06004AD9 RID: 19161 RVA: 0x0026EABA File Offset: 0x0026CEBA
			' (set) Token: 0x06004ADA RID: 19162 RVA: 0x0026EAC2 File Offset: 0x0026CEC2
			Public Property toggle As ControlMapper.GUIToggle

			' Token: 0x170006DB RID: 1755
			' (get) Token: 0x06004ADB RID: 19163 RVA: 0x0026EACB File Offset: 0x0026CECB
			' (set) Token: 0x06004ADC RID: 19164 RVA: 0x0026EAEB File Offset: 0x0026CEEB
			Public Property actionElementMapId As Integer
				Get
					If Me.fieldInfo Is Nothing Then
						Return -1
					End If
					Return Me.fieldInfo.actionElementMapId
				End Get
				Set(value As Integer)
					If Me.fieldInfo Is Nothing Then
						Return
					End If
					Me.fieldInfo.actionElementMapId = value
				End Set
			End Property

			' Token: 0x170006DC RID: 1756
			' (get) Token: 0x06004ADD RID: 19165 RVA: 0x0026EB0B File Offset: 0x0026CF0B
			' (set) Token: 0x06004ADE RID: 19166 RVA: 0x0026EB2B File Offset: 0x0026CF2B
			Public Property controllerId As Integer
				Get
					If Me.fieldInfo Is Nothing Then
						Return -1
					End If
					Return Me.fieldInfo.controllerId
				End Get
				Set(value As Integer)
					If Me.fieldInfo Is Nothing Then
						Return
					End If
					Me.fieldInfo.controllerId = value
				End Set
			End Property

			' Token: 0x06004ADF RID: 19167 RVA: 0x0026EB4C File Offset: 0x0026CF4C
			Public Sub SetFieldInfoData(actionId As Integer, axisRange As AxisRange, controllerType As ControllerType, intData As Integer)
				MyBase.SetElementInfoData(String.Empty, intData)
				If Me.fieldInfo Is Nothing Then
					Return
				End If
				Me.fieldInfo.actionId = actionId
				Me.fieldInfo.axisRange = axisRange
				Me.fieldInfo.controllerType = controllerType
			End Sub

			' Token: 0x06004AE0 RID: 19168 RVA: 0x0026EB9C File Offset: 0x0026CF9C
			Public Sub SetOnClickCallback(callback As Action(Of InputFieldInfo))
				If Me.button Is Nothing Then
					Return
				End If
				Me.button.onClick.AddListener(Sub()
					callback(Me.fieldInfo)
				End Sub)
			End Sub

			' Token: 0x06004AE1 RID: 19169 RVA: 0x0026EBEB File Offset: 0x0026CFEB
			Public Overridable Sub SetInteractable(state As Boolean, playTransition As Boolean, permanent As Boolean)
				If Me.permanentStateSet Then
					Return
				End If
				If Me.hasToggle AndAlso Not state Then
					Me.toggle.SetInteractible(state, playTransition, permanent)
				End If
				MyBase.SetInteractible(state, playTransition, permanent)
			End Sub

			' Token: 0x06004AE2 RID: 19170 RVA: 0x0026EC21 File Offset: 0x0026D021
			Public Sub AddToggle(toggle As ControlMapper.GUIToggle)
				If toggle Is Nothing Then
					Return
				End If
				Me.toggle = toggle
			End Sub
		End Class

		' Token: 0x02000C12 RID: 3090
		Private Class GUIToggle
			Inherits ControlMapper.GUIElement

			' Token: 0x06004AE3 RID: 19171 RVA: 0x0026EC51 File Offset: 0x0026D051
			Public Sub New(gameObject As GameObject)
				MyBase.New(gameObject)
				If Not Me.Init() Then
					Return
				End If
			End Sub

			' Token: 0x06004AE4 RID: 19172 RVA: 0x0026EC66 File Offset: 0x0026D066
			Public Sub New(toggle As Toggle, label As Text)
				MyBase.New(toggle, label)
				If Not Me.Init() Then
					Return
				End If
			End Sub

			' Token: 0x170006DD RID: 1757
			' (get) Token: 0x06004AE5 RID: 19173 RVA: 0x0026EC7C File Offset: 0x0026D07C
			Protected ReadOnly Property toggle As Toggle
				Get
					Return TryCast(Me.selectable, Toggle)
				End Get
			End Property

			' Token: 0x170006DE RID: 1758
			' (get) Token: 0x06004AE6 RID: 19174 RVA: 0x0026EC89 File Offset: 0x0026D089
			Public ReadOnly Property toggleInfo As ToggleInfo
				Get
					Return TryCast(Me.uiElementInfo, ToggleInfo)
				End Get
			End Property

			' Token: 0x170006DF RID: 1759
			' (get) Token: 0x06004AE7 RID: 19175 RVA: 0x0026EC96 File Offset: 0x0026D096
			' (set) Token: 0x06004AE8 RID: 19176 RVA: 0x0026ECB6 File Offset: 0x0026D0B6
			Public Property actionElementMapId As Integer
				Get
					If Me.toggleInfo Is Nothing Then
						Return -1
					End If
					Return Me.toggleInfo.actionElementMapId
				End Get
				Set(value As Integer)
					If Me.toggleInfo Is Nothing Then
						Return
					End If
					Me.toggleInfo.actionElementMapId = value
				End Set
			End Property

			' Token: 0x06004AE9 RID: 19177 RVA: 0x0026ECD8 File Offset: 0x0026D0D8
			Public Sub SetToggleInfoData(actionId As Integer, axisRange As AxisRange, controllerType As ControllerType, intData As Integer)
				MyBase.SetElementInfoData(String.Empty, intData)
				If Me.toggleInfo Is Nothing Then
					Return
				End If
				Me.toggleInfo.actionId = actionId
				Me.toggleInfo.axisRange = axisRange
				Me.toggleInfo.controllerType = controllerType
			End Sub

			' Token: 0x06004AEA RID: 19178 RVA: 0x0026ED28 File Offset: 0x0026D128
			Public Sub SetOnSubmitCallback(callback As Action(Of ToggleInfo, Boolean))
				If Me.toggle Is Nothing Then
					Return
				End If
				Dim eventTrigger As EventTrigger = Me.toggle.GetComponent(Of EventTrigger)()
				If eventTrigger Is Nothing Then
					eventTrigger = Me.toggle.gameObject.AddComponent(Of EventTrigger)()
				End If
				Dim triggerEvent As EventTrigger.TriggerEvent = New EventTrigger.TriggerEvent()
				triggerEvent.AddListener(Sub(data As BaseEventData)
					Dim pointerEventData As PointerEventData = TryCast(data, PointerEventData)
					If pointerEventData IsNot Nothing AndAlso pointerEventData.button <> PointerEventData.InputButton.Left Then
						Return
					End If
					callback(Me.toggleInfo, Me.toggle.isOn)
				End Sub)
				Dim entry As EventTrigger.Entry = New EventTrigger.Entry() With { .callback = triggerEvent, .eventID = EventTriggerType.Submit }
				Dim entry2 As EventTrigger.Entry = New EventTrigger.Entry() With { .callback = triggerEvent, .eventID = EventTriggerType.PointerClick }
				If eventTrigger.triggers IsNot Nothing Then
					eventTrigger.triggers.Clear()
				Else
					eventTrigger.triggers = New List(Of EventTrigger.Entry)()
				End If
				eventTrigger.triggers.Add(entry)
				eventTrigger.triggers.Add(entry2)
			End Sub

			' Token: 0x06004AEB RID: 19179 RVA: 0x0026EE11 File Offset: 0x0026D211
			Public Sub SetToggleState(state As Boolean)
				If Me.toggle Is Nothing Then
					Return
				End If
				Me.toggle.isOn = state
			End Sub
		End Class

		' Token: 0x02000C13 RID: 3091
		Private Class GUILabel
			' Token: 0x06004AEC RID: 19180 RVA: 0x0026EE88 File Offset: 0x0026D288
			Public Sub New(gameObject As GameObject)
				If gameObject Is Nothing Then
					Global.UnityEngine.Debug.LogError("Rewired Control Mapper: gameObject is null!")
					Return
				End If
				Me.text = UnityTools.GetComponentInSelfOrChildren(Of Text)(gameObject)
				Me.Check()
			End Sub

			' Token: 0x06004AED RID: 19181 RVA: 0x0026EEBA File Offset: 0x0026D2BA
			Public Sub New(label As Text)
				Me.text = label
				If Not Me.Check() Then
					Return
				End If
			End Sub

			' Token: 0x170006E0 RID: 1760
			' (get) Token: 0x06004AEE RID: 19182 RVA: 0x0026EED5 File Offset: 0x0026D2D5
			' (set) Token: 0x06004AEF RID: 19183 RVA: 0x0026EEDD File Offset: 0x0026D2DD
			Public Property gameObject As GameObject

			' Token: 0x170006E1 RID: 1761
			' (get) Token: 0x06004AF0 RID: 19184 RVA: 0x0026EEE6 File Offset: 0x0026D2E6
			' (set) Token: 0x06004AF1 RID: 19185 RVA: 0x0026EEEE File Offset: 0x0026D2EE
			Private Property text As Text

			' Token: 0x170006E2 RID: 1762
			' (get) Token: 0x06004AF2 RID: 19186 RVA: 0x0026EEF7 File Offset: 0x0026D2F7
			' (set) Token: 0x06004AF3 RID: 19187 RVA: 0x0026EEFF File Offset: 0x0026D2FF
			Public Property rectTransform As RectTransform

			' Token: 0x06004AF4 RID: 19188 RVA: 0x0026EF08 File Offset: 0x0026D308
			Public Sub SetSize(width As Integer, height As Integer)
				If Me.text Is Nothing Then
					Return
				End If
				Me.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, CSng(width))
				Me.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, CSng(height))
			End Sub

			' Token: 0x06004AF5 RID: 19189 RVA: 0x0026EF38 File Offset: 0x0026D338
			Public Sub SetWidth(width As Integer)
				If Me.text Is Nothing Then
					Return
				End If
				Me.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, CSng(width))
			End Sub

			' Token: 0x06004AF6 RID: 19190 RVA: 0x0026EF5A File Offset: 0x0026D35A
			Public Sub SetHeight(height As Integer)
				If Me.text Is Nothing Then
					Return
				End If
				Me.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, CSng(height))
			End Sub

			' Token: 0x06004AF7 RID: 19191 RVA: 0x0026EF7C File Offset: 0x0026D37C
			Public Sub SetLabel(label As String)
				If Me.text Is Nothing Then
					Return
				End If
				Me.text.text = label
			End Sub

			' Token: 0x06004AF8 RID: 19192 RVA: 0x0026EF9C File Offset: 0x0026D39C
			Public Sub SetFontStyle(style As FontStyle)
				If Me.text Is Nothing Then
					Return
				End If
				Me.text.fontStyle = style
			End Sub

			' Token: 0x06004AF9 RID: 19193 RVA: 0x0026EFBC File Offset: 0x0026D3BC
			Public Sub SetTextAlignment(alignment As TextAnchor)
				If Me.text Is Nothing Then
					Return
				End If
				Me.text.alignment = alignment
			End Sub

			' Token: 0x06004AFA RID: 19194 RVA: 0x0026EFDC File Offset: 0x0026D3DC
			Public Sub SetActive(state As Boolean)
				If Me.gameObject Is Nothing Then
					Return
				End If
				Me.gameObject.SetActive(state)
			End Sub

			' Token: 0x06004AFB RID: 19195 RVA: 0x0026EFFC File Offset: 0x0026D3FC
			Private Function Check() As Boolean
				Dim flag As Boolean = True
				If Me.text Is Nothing Then
					Global.UnityEngine.Debug.LogError("Rewired Control Mapper: Button is missing Text child component!")
					flag = False
				End If
				Me.gameObject = Me.text.gameObject
				Me.rectTransform = Me.text.GetComponent(Of RectTransform)()
				Return flag
			End Function
		End Class

		' Token: 0x02000C14 RID: 3092
		<Serializable()>
		Public Class MappingSet
			' Token: 0x06004AFC RID: 19196 RVA: 0x0026F04B File Offset: 0x0026D44B
			Public Sub New()
				Me._mapCategoryId = -1
				Me._actionCategoryIds = New Integer(-1) {}
				Me._actionIds = New Integer(-1) {}
				Me._actionListMode = ControlMapper.MappingSet.ActionListMode.ActionCategory
			End Sub

			' Token: 0x06004AFD RID: 19197 RVA: 0x0026F079 File Offset: 0x0026D479
			Private Sub New(mapCategoryId As Integer, actionListMode As ControlMapper.MappingSet.ActionListMode, actionCategoryIds As Integer(), actionIds As Integer())
				Me._mapCategoryId = mapCategoryId
				Me._actionListMode = actionListMode
				Me._actionCategoryIds = actionCategoryIds
				Me._actionIds = actionIds
			End Sub

			' Token: 0x170006E3 RID: 1763
			' (get) Token: 0x06004AFE RID: 19198 RVA: 0x0026F09E File Offset: 0x0026D49E
			Public ReadOnly Property mapCategoryId As Integer
				Get
					Return Me._mapCategoryId
				End Get
			End Property

			' Token: 0x170006E4 RID: 1764
			' (get) Token: 0x06004AFF RID: 19199 RVA: 0x0026F0A6 File Offset: 0x0026D4A6
			Public ReadOnly Property actionListMode As ControlMapper.MappingSet.ActionListMode
				Get
					Return Me._actionListMode
				End Get
			End Property

			' Token: 0x170006E5 RID: 1765
			' (get) Token: 0x06004B00 RID: 19200 RVA: 0x0026F0AE File Offset: 0x0026D4AE
			Public ReadOnly Property actionCategoryIds As IList(Of Integer)
				Get
					If Me._actionCategoryIds Is Nothing Then
						Return Nothing
					End If
					If Me._actionCategoryIdsReadOnly Is Nothing Then
						Me._actionCategoryIdsReadOnly = New ReadOnlyCollection(Of Integer)(Me._actionCategoryIds)
					End If
					Return Me._actionCategoryIdsReadOnly
				End Get
			End Property

			' Token: 0x170006E6 RID: 1766
			' (get) Token: 0x06004B01 RID: 19201 RVA: 0x0026F0DF File Offset: 0x0026D4DF
			Public ReadOnly Property actionIds As IList(Of Integer)
				Get
					If Me._actionIds Is Nothing Then
						Return Nothing
					End If
					If Me._actionIdsReadOnly Is Nothing Then
						Me._actionIdsReadOnly = New ReadOnlyCollection(Of Integer)(Me._actionIds)
					End If
					Return Me._actionIds
				End Get
			End Property

			' Token: 0x170006E7 RID: 1767
			' (get) Token: 0x06004B02 RID: 19202 RVA: 0x0026F110 File Offset: 0x0026D510
			Public ReadOnly Property isValid As Boolean
				Get
					Return Me._mapCategoryId >= 0 AndAlso ReInput.mapping.GetMapCategory(Me._mapCategoryId) IsNot Nothing
				End Get
			End Property

			' Token: 0x170006E8 RID: 1768
			' (get) Token: 0x06004B03 RID: 19203 RVA: 0x0026F136 File Offset: 0x0026D536
			Public Shared ReadOnly Property [Default] As ControlMapper.MappingSet
				Get
					Return New ControlMapper.MappingSet(0, ControlMapper.MappingSet.ActionListMode.ActionCategory, New Integer(0) {}, New Integer(-1) {})
				End Get
			End Property

			' Token: 0x0400502A RID: 20522
			<SerializeField()>
			<Tooltip("The Map Category that will be displayed to the user for remapping.")>
			Private _mapCategoryId As Integer

			' Token: 0x0400502B RID: 20523
			<SerializeField()>
			<Tooltip("Choose whether you want to list Actions to display for this Map Category by individual Action or by all the Actions in an Action Category.")>
			Private _actionListMode As ControlMapper.MappingSet.ActionListMode

			' Token: 0x0400502C RID: 20524
			<SerializeField()>
			Private _actionCategoryIds As Integer()

			' Token: 0x0400502D RID: 20525
			<SerializeField()>
			Private _actionIds As Integer()

			' Token: 0x0400502E RID: 20526
			Private _actionCategoryIdsReadOnly As IList(Of Integer)

			' Token: 0x0400502F RID: 20527
			Private _actionIdsReadOnly As IList(Of Integer)

			' Token: 0x02000C15 RID: 3093
			Public Enum ActionListMode
				' Token: 0x04005031 RID: 20529
				ActionCategory
				' Token: 0x04005032 RID: 20530
				Action
			End Enum
		End Class

		' Token: 0x02000C16 RID: 3094
		<Serializable()>
		Public Class InputBehaviorSettings
			' Token: 0x170006E9 RID: 1769
			' (get) Token: 0x06004B05 RID: 19205 RVA: 0x0026F1AB File Offset: 0x0026D5AB
			Public ReadOnly Property inputBehaviorId As Integer
				Get
					Return Me._inputBehaviorId
				End Get
			End Property

			' Token: 0x170006EA RID: 1770
			' (get) Token: 0x06004B06 RID: 19206 RVA: 0x0026F1B3 File Offset: 0x0026D5B3
			Public ReadOnly Property showJoystickAxisSensitivity As Boolean
				Get
					Return Me._showJoystickAxisSensitivity
				End Get
			End Property

			' Token: 0x170006EB RID: 1771
			' (get) Token: 0x06004B07 RID: 19207 RVA: 0x0026F1BB File Offset: 0x0026D5BB
			Public ReadOnly Property showMouseXYAxisSensitivity As Boolean
				Get
					Return Me._showMouseXYAxisSensitivity
				End Get
			End Property

			' Token: 0x170006EC RID: 1772
			' (get) Token: 0x06004B08 RID: 19208 RVA: 0x0026F1C3 File Offset: 0x0026D5C3
			Public ReadOnly Property labelLanguageKey As String
				Get
					Return Me._labelLanguageKey
				End Get
			End Property

			' Token: 0x170006ED RID: 1773
			' (get) Token: 0x06004B09 RID: 19209 RVA: 0x0026F1CB File Offset: 0x0026D5CB
			Public ReadOnly Property joystickAxisSensitivityLabelLanguageKey As String
				Get
					Return Me._joystickAxisSensitivityLabelLanguageKey
				End Get
			End Property

			' Token: 0x170006EE RID: 1774
			' (get) Token: 0x06004B0A RID: 19210 RVA: 0x0026F1D3 File Offset: 0x0026D5D3
			Public ReadOnly Property mouseXYAxisSensitivityLabelLanguageKey As String
				Get
					Return Me._mouseXYAxisSensitivityLabelLanguageKey
				End Get
			End Property

			' Token: 0x170006EF RID: 1775
			' (get) Token: 0x06004B0B RID: 19211 RVA: 0x0026F1DB File Offset: 0x0026D5DB
			Public ReadOnly Property joystickAxisSensitivityIcon As Sprite
				Get
					Return Me._joystickAxisSensitivityIcon
				End Get
			End Property

			' Token: 0x170006F0 RID: 1776
			' (get) Token: 0x06004B0C RID: 19212 RVA: 0x0026F1E3 File Offset: 0x0026D5E3
			Public ReadOnly Property mouseXYAxisSensitivityIcon As Sprite
				Get
					Return Me._mouseXYAxisSensitivityIcon
				End Get
			End Property

			' Token: 0x170006F1 RID: 1777
			' (get) Token: 0x06004B0D RID: 19213 RVA: 0x0026F1EB File Offset: 0x0026D5EB
			Public ReadOnly Property joystickAxisSensitivityMin As Single
				Get
					Return Me._joystickAxisSensitivityMin
				End Get
			End Property

			' Token: 0x170006F2 RID: 1778
			' (get) Token: 0x06004B0E RID: 19214 RVA: 0x0026F1F3 File Offset: 0x0026D5F3
			Public ReadOnly Property joystickAxisSensitivityMax As Single
				Get
					Return Me._joystickAxisSensitivityMax
				End Get
			End Property

			' Token: 0x170006F3 RID: 1779
			' (get) Token: 0x06004B0F RID: 19215 RVA: 0x0026F1FB File Offset: 0x0026D5FB
			Public ReadOnly Property mouseXYAxisSensitivityMin As Single
				Get
					Return Me._mouseXYAxisSensitivityMin
				End Get
			End Property

			' Token: 0x170006F4 RID: 1780
			' (get) Token: 0x06004B10 RID: 19216 RVA: 0x0026F203 File Offset: 0x0026D603
			Public ReadOnly Property mouseXYAxisSensitivityMax As Single
				Get
					Return Me._mouseXYAxisSensitivityMax
				End Get
			End Property

			' Token: 0x170006F5 RID: 1781
			' (get) Token: 0x06004B11 RID: 19217 RVA: 0x0026F20B File Offset: 0x0026D60B
			Public ReadOnly Property isValid As Boolean
				Get
					Return Me._inputBehaviorId >= 0 AndAlso (Me._showJoystickAxisSensitivity OrElse Me._showMouseXYAxisSensitivity)
				End Get
			End Property

			' Token: 0x04005033 RID: 20531
			<SerializeField()>
			<Tooltip("The Input Behavior that will be displayed to the user for modification.")>
			Private _inputBehaviorId As Integer = -1

			' Token: 0x04005034 RID: 20532
			<SerializeField()>
			<Tooltip("If checked, a slider will be displayed so the user can change this value.")>
			Private _showJoystickAxisSensitivity As Boolean = True

			' Token: 0x04005035 RID: 20533
			<SerializeField()>
			<Tooltip("If checked, a slider will be displayed so the user can change this value.")>
			Private _showMouseXYAxisSensitivity As Boolean = True

			' Token: 0x04005036 RID: 20534
			<SerializeField()>
			<Tooltip("If set to a non-blank value, this key will be used to look up the name in Language to be displayed as the title for the Input Behavior control set. Otherwise, the name field of the InputBehavior will be used.")>
			Private _labelLanguageKey As String = String.Empty

			' Token: 0x04005037 RID: 20535
			<SerializeField()>
			<Tooltip("If set to a non-blank value, this name will be displayed above the individual slider control. Otherwise, no name will be displayed.")>
			Private _joystickAxisSensitivityLabelLanguageKey As String = String.Empty

			' Token: 0x04005038 RID: 20536
			<SerializeField()>
			<Tooltip("If set to a non-blank value, this key will be used to look up the name in Language to be displayed above the individual slider control. Otherwise, no name will be displayed.")>
			Private _mouseXYAxisSensitivityLabelLanguageKey As String = String.Empty

			' Token: 0x04005039 RID: 20537
			<SerializeField()>
			<Tooltip("The icon to display next to the slider. Set to none for no icon.")>
			Private _joystickAxisSensitivityIcon As Sprite

			' Token: 0x0400503A RID: 20538
			<SerializeField()>
			<Tooltip("The icon to display next to the slider. Set to none for no icon.")>
			Private _mouseXYAxisSensitivityIcon As Sprite

			' Token: 0x0400503B RID: 20539
			<SerializeField()>
			<Tooltip("Minimum value the user is allowed to set for this property.")>
			Private _joystickAxisSensitivityMin As Single

			' Token: 0x0400503C RID: 20540
			<SerializeField()>
			<Tooltip("Maximum value the user is allowed to set for this property.")>
			Private _joystickAxisSensitivityMax As Single = 2F

			' Token: 0x0400503D RID: 20541
			<SerializeField()>
			<Tooltip("Minimum value the user is allowed to set for this property.")>
			Private _mouseXYAxisSensitivityMin As Single

			' Token: 0x0400503E RID: 20542
			<SerializeField()>
			<Tooltip("Maximum value the user is allowed to set for this property.")>
			Private _mouseXYAxisSensitivityMax As Single = 2F
		End Class

		' Token: 0x02000C17 RID: 3095
		<Serializable()>
		Private Class Prefabs
			' Token: 0x170006F6 RID: 1782
			' (get) Token: 0x06004B13 RID: 19219 RVA: 0x0026F238 File Offset: 0x0026D638
			Public ReadOnly Property button As GameObject
				Get
					Return Me._button
				End Get
			End Property

			' Token: 0x170006F7 RID: 1783
			' (get) Token: 0x06004B14 RID: 19220 RVA: 0x0026F240 File Offset: 0x0026D640
			Public ReadOnly Property playerButton As GameObject
				Get
					Return Me._playerButton
				End Get
			End Property

			' Token: 0x170006F8 RID: 1784
			' (get) Token: 0x06004B15 RID: 19221 RVA: 0x0026F248 File Offset: 0x0026D648
			Public ReadOnly Property fitButton As GameObject
				Get
					Return Me._fitButton
				End Get
			End Property

			' Token: 0x170006F9 RID: 1785
			' (get) Token: 0x06004B16 RID: 19222 RVA: 0x0026F250 File Offset: 0x0026D650
			Public ReadOnly Property inputGridLabel As GameObject
				Get
					Return Me._inputGridLabel
				End Get
			End Property

			' Token: 0x170006FA RID: 1786
			' (get) Token: 0x06004B17 RID: 19223 RVA: 0x0026F258 File Offset: 0x0026D658
			Public ReadOnly Property inputGridDeactivatedLabel As GameObject
				Get
					Return Me._inputGridDeactivatedLabel
				End Get
			End Property

			' Token: 0x170006FB RID: 1787
			' (get) Token: 0x06004B18 RID: 19224 RVA: 0x0026F260 File Offset: 0x0026D660
			Public ReadOnly Property inputGridHeaderLabel As GameObject
				Get
					Return Me._inputGridHeaderLabel
				End Get
			End Property

			' Token: 0x170006FC RID: 1788
			' (get) Token: 0x06004B19 RID: 19225 RVA: 0x0026F268 File Offset: 0x0026D668
			Public ReadOnly Property actionsHeaderLabel As GameObject
				Get
					Return Me._actionsHeaderLabel
				End Get
			End Property

			' Token: 0x170006FD RID: 1789
			' (get) Token: 0x06004B1A RID: 19226 RVA: 0x0026F270 File Offset: 0x0026D670
			Public ReadOnly Property inputGridFieldButton As GameObject
				Get
					Return Me._inputGridFieldButton
				End Get
			End Property

			' Token: 0x170006FE RID: 1790
			' (get) Token: 0x06004B1B RID: 19227 RVA: 0x0026F278 File Offset: 0x0026D678
			Public ReadOnly Property inputGridFieldInvertToggle As GameObject
				Get
					Return Me._inputGridFieldInvertToggle
				End Get
			End Property

			' Token: 0x170006FF RID: 1791
			' (get) Token: 0x06004B1C RID: 19228 RVA: 0x0026F280 File Offset: 0x0026D680
			Public ReadOnly Property window As GameObject
				Get
					Return Me._window
				End Get
			End Property

			' Token: 0x17000700 RID: 1792
			' (get) Token: 0x06004B1D RID: 19229 RVA: 0x0026F288 File Offset: 0x0026D688
			Public ReadOnly Property windowTitleText As GameObject
				Get
					Return Me._windowTitleText
				End Get
			End Property

			' Token: 0x17000701 RID: 1793
			' (get) Token: 0x06004B1E RID: 19230 RVA: 0x0026F290 File Offset: 0x0026D690
			Public ReadOnly Property windowContentText As GameObject
				Get
					Return Me._windowContentText
				End Get
			End Property

			' Token: 0x17000702 RID: 1794
			' (get) Token: 0x06004B1F RID: 19231 RVA: 0x0026F298 File Offset: 0x0026D698
			Public ReadOnly Property fader As GameObject
				Get
					Return Me._fader
				End Get
			End Property

			' Token: 0x17000703 RID: 1795
			' (get) Token: 0x06004B20 RID: 19232 RVA: 0x0026F2A0 File Offset: 0x0026D6A0
			Public ReadOnly Property calibrationWindow As GameObject
				Get
					Return Me._calibrationWindow
				End Get
			End Property

			' Token: 0x17000704 RID: 1796
			' (get) Token: 0x06004B21 RID: 19233 RVA: 0x0026F2A8 File Offset: 0x0026D6A8
			Public ReadOnly Property inputBehaviorsWindow As GameObject
				Get
					Return Me._inputBehaviorsWindow
				End Get
			End Property

			' Token: 0x17000705 RID: 1797
			' (get) Token: 0x06004B22 RID: 19234 RVA: 0x0026F2B0 File Offset: 0x0026D6B0
			Public ReadOnly Property centerStickGraphic As GameObject
				Get
					Return Me._centerStickGraphic
				End Get
			End Property

			' Token: 0x17000706 RID: 1798
			' (get) Token: 0x06004B23 RID: 19235 RVA: 0x0026F2B8 File Offset: 0x0026D6B8
			Public ReadOnly Property moveStickGraphic As GameObject
				Get
					Return Me._moveStickGraphic
				End Get
			End Property

			' Token: 0x06004B24 RID: 19236 RVA: 0x0026F2C0 File Offset: 0x0026D6C0
			Public Function Check() As Boolean
				Return Not(Me._button Is Nothing) AndAlso Not(Me._fitButton Is Nothing) AndAlso Not(Me._inputGridLabel Is Nothing) AndAlso Not(Me._inputGridHeaderLabel Is Nothing) AndAlso Not(Me._inputGridFieldButton Is Nothing) AndAlso Not(Me._inputGridFieldInvertToggle Is Nothing) AndAlso Not(Me._window Is Nothing) AndAlso Not(Me._windowTitleText Is Nothing) AndAlso Not(Me._windowContentText Is Nothing) AndAlso Not(Me._fader Is Nothing) AndAlso Not(Me._calibrationWindow Is Nothing) AndAlso Not(Me._inputBehaviorsWindow Is Nothing)
			End Function

			' Token: 0x0400503F RID: 20543
			<SerializeField()>
			Private _button As GameObject

			' Token: 0x04005040 RID: 20544
			<SerializeField()>
			Private _playerButton As GameObject

			' Token: 0x04005041 RID: 20545
			<SerializeField()>
			Private _fitButton As GameObject

			' Token: 0x04005042 RID: 20546
			<SerializeField()>
			Private _inputGridLabel As GameObject

			' Token: 0x04005043 RID: 20547
			<SerializeField()>
			Private _inputGridDeactivatedLabel As GameObject

			' Token: 0x04005044 RID: 20548
			<SerializeField()>
			Private _inputGridHeaderLabel As GameObject

			' Token: 0x04005045 RID: 20549
			<SerializeField()>
			Private _actionsHeaderLabel As GameObject

			' Token: 0x04005046 RID: 20550
			<SerializeField()>
			Private _inputGridFieldButton As GameObject

			' Token: 0x04005047 RID: 20551
			<SerializeField()>
			Private _inputGridFieldInvertToggle As GameObject

			' Token: 0x04005048 RID: 20552
			<SerializeField()>
			Private _window As GameObject

			' Token: 0x04005049 RID: 20553
			<SerializeField()>
			Private _windowTitleText As GameObject

			' Token: 0x0400504A RID: 20554
			<SerializeField()>
			Private _windowContentText As GameObject

			' Token: 0x0400504B RID: 20555
			<SerializeField()>
			Private _fader As GameObject

			' Token: 0x0400504C RID: 20556
			<SerializeField()>
			Private _calibrationWindow As GameObject

			' Token: 0x0400504D RID: 20557
			<SerializeField()>
			Private _inputBehaviorsWindow As GameObject

			' Token: 0x0400504E RID: 20558
			<SerializeField()>
			Private _centerStickGraphic As GameObject

			' Token: 0x0400504F RID: 20559
			<SerializeField()>
			Private _moveStickGraphic As GameObject
		End Class

		' Token: 0x02000C18 RID: 3096
		<Serializable()>
		Private Class References
			' Token: 0x17000707 RID: 1799
			' (get) Token: 0x06004B26 RID: 19238 RVA: 0x0026F3A4 File Offset: 0x0026D7A4
			Public ReadOnly Property canvas As Canvas
				Get
					Return Me._canvas
				End Get
			End Property

			' Token: 0x17000708 RID: 1800
			' (get) Token: 0x06004B27 RID: 19239 RVA: 0x0026F3AC File Offset: 0x0026D7AC
			Public ReadOnly Property mainCanvasGroup As CanvasGroup
				Get
					Return Me._mainCanvasGroup
				End Get
			End Property

			' Token: 0x17000709 RID: 1801
			' (get) Token: 0x06004B28 RID: 19240 RVA: 0x0026F3B4 File Offset: 0x0026D7B4
			Public ReadOnly Property mainContent As Transform
				Get
					Return Me._mainContent
				End Get
			End Property

			' Token: 0x1700070A RID: 1802
			' (get) Token: 0x06004B29 RID: 19241 RVA: 0x0026F3BC File Offset: 0x0026D7BC
			Public ReadOnly Property mainContentInner As Transform
				Get
					Return Me._mainContentInner
				End Get
			End Property

			' Token: 0x1700070B RID: 1803
			' (get) Token: 0x06004B2A RID: 19242 RVA: 0x0026F3C4 File Offset: 0x0026D7C4
			Public ReadOnly Property playersGroup As UIGroup
				Get
					Return Me._playersGroup
				End Get
			End Property

			' Token: 0x1700070C RID: 1804
			' (get) Token: 0x06004B2B RID: 19243 RVA: 0x0026F3CC File Offset: 0x0026D7CC
			Public ReadOnly Property controllerGroup As Transform
				Get
					Return Me._controllerGroup
				End Get
			End Property

			' Token: 0x1700070D RID: 1805
			' (get) Token: 0x06004B2C RID: 19244 RVA: 0x0026F3D4 File Offset: 0x0026D7D4
			Public ReadOnly Property controllerGroupLabelGroup As Transform
				Get
					Return Me._controllerGroupLabelGroup
				End Get
			End Property

			' Token: 0x1700070E RID: 1806
			' (get) Token: 0x06004B2D RID: 19245 RVA: 0x0026F3DC File Offset: 0x0026D7DC
			Public ReadOnly Property controllerSettingsGroup As UIGroup
				Get
					Return Me._controllerSettingsGroup
				End Get
			End Property

			' Token: 0x1700070F RID: 1807
			' (get) Token: 0x06004B2E RID: 19246 RVA: 0x0026F3E4 File Offset: 0x0026D7E4
			Public ReadOnly Property assignedControllersGroup As UIGroup
				Get
					Return Me._assignedControllersGroup
				End Get
			End Property

			' Token: 0x17000710 RID: 1808
			' (get) Token: 0x06004B2F RID: 19247 RVA: 0x0026F3EC File Offset: 0x0026D7EC
			Public ReadOnly Property settingsAndMapCategoriesGroup As Transform
				Get
					Return Me._settingsAndMapCategoriesGroup
				End Get
			End Property

			' Token: 0x17000711 RID: 1809
			' (get) Token: 0x06004B30 RID: 19248 RVA: 0x0026F3F4 File Offset: 0x0026D7F4
			Public ReadOnly Property settingsGroup As UIGroup
				Get
					Return Me._settingsGroup
				End Get
			End Property

			' Token: 0x17000712 RID: 1810
			' (get) Token: 0x06004B31 RID: 19249 RVA: 0x0026F3FC File Offset: 0x0026D7FC
			Public ReadOnly Property mapCategoriesGroup As UIGroup
				Get
					Return Me._mapCategoriesGroup
				End Get
			End Property

			' Token: 0x17000713 RID: 1811
			' (get) Token: 0x06004B32 RID: 19250 RVA: 0x0026F404 File Offset: 0x0026D804
			Public ReadOnly Property inputGridGroup As Transform
				Get
					Return Me._inputGridGroup
				End Get
			End Property

			' Token: 0x17000714 RID: 1812
			' (get) Token: 0x06004B33 RID: 19251 RVA: 0x0026F40C File Offset: 0x0026D80C
			Public ReadOnly Property inputGridContainer As Transform
				Get
					Return Me._inputGridContainer
				End Get
			End Property

			' Token: 0x17000715 RID: 1813
			' (get) Token: 0x06004B34 RID: 19252 RVA: 0x0026F414 File Offset: 0x0026D814
			Public ReadOnly Property inputGridHeadersGroup As Transform
				Get
					Return Me._inputGridHeadersGroup
				End Get
			End Property

			' Token: 0x17000716 RID: 1814
			' (get) Token: 0x06004B35 RID: 19253 RVA: 0x0026F41C File Offset: 0x0026D81C
			Public ReadOnly Property inputGridInnerGroup As Transform
				Get
					Return Me._inputGridInnerGroup
				End Get
			End Property

			' Token: 0x17000717 RID: 1815
			' (get) Token: 0x06004B36 RID: 19254 RVA: 0x0026F424 File Offset: 0x0026D824
			Public ReadOnly Property actionsColumnHeadersGroup As Transform
				Get
					Return Me._actionsColumnHeadersGroup
				End Get
			End Property

			' Token: 0x17000718 RID: 1816
			' (get) Token: 0x06004B37 RID: 19255 RVA: 0x0026F42C File Offset: 0x0026D82C
			Public ReadOnly Property actionsColumn As Transform
				Get
					Return Me._actionsColumn
				End Get
			End Property

			' Token: 0x17000719 RID: 1817
			' (get) Token: 0x06004B38 RID: 19256 RVA: 0x0026F434 File Offset: 0x0026D834
			Public ReadOnly Property controllerNameLabel As Text
				Get
					Return Me._controllerNameLabel
				End Get
			End Property

			' Token: 0x1700071A RID: 1818
			' (get) Token: 0x06004B39 RID: 19257 RVA: 0x0026F43C File Offset: 0x0026D83C
			Public ReadOnly Property removeControllerButton As Button
				Get
					Return Me._removeControllerButton
				End Get
			End Property

			' Token: 0x1700071B RID: 1819
			' (get) Token: 0x06004B3A RID: 19258 RVA: 0x0026F444 File Offset: 0x0026D844
			Public ReadOnly Property assignControllerButton As Button
				Get
					Return Me._assignControllerButton
				End Get
			End Property

			' Token: 0x1700071C RID: 1820
			' (get) Token: 0x06004B3B RID: 19259 RVA: 0x0026F44C File Offset: 0x0026D84C
			Public ReadOnly Property calibrateControllerButton As Button
				Get
					Return Me._calibrateControllerButton
				End Get
			End Property

			' Token: 0x1700071D RID: 1821
			' (get) Token: 0x06004B3C RID: 19260 RVA: 0x0026F454 File Offset: 0x0026D854
			Public ReadOnly Property doneButton As Button
				Get
					Return Me._doneButton
				End Get
			End Property

			' Token: 0x1700071E RID: 1822
			' (get) Token: 0x06004B3D RID: 19261 RVA: 0x0026F45C File Offset: 0x0026D85C
			Public ReadOnly Property restoreDefaultsButton As Button
				Get
					Return Me._restoreDefaultsButton
				End Get
			End Property

			' Token: 0x1700071F RID: 1823
			' (get) Token: 0x06004B3E RID: 19262 RVA: 0x0026F464 File Offset: 0x0026D864
			Public ReadOnly Property defaultSelection As Selectable
				Get
					Return Me._defaultSelection
				End Get
			End Property

			' Token: 0x17000720 RID: 1824
			' (get) Token: 0x06004B3F RID: 19263 RVA: 0x0026F46C File Offset: 0x0026D86C
			Public ReadOnly Property fixedSelectableUIElements As GameObject()
				Get
					Return Me._fixedSelectableUIElements
				End Get
			End Property

			' Token: 0x17000721 RID: 1825
			' (get) Token: 0x06004B40 RID: 19264 RVA: 0x0026F474 File Offset: 0x0026D874
			Public ReadOnly Property mainBackgroundImage As Image
				Get
					Return Me._mainBackgroundImage
				End Get
			End Property

			' Token: 0x17000722 RID: 1826
			' (get) Token: 0x06004B41 RID: 19265 RVA: 0x0026F47C File Offset: 0x0026D87C
			' (set) Token: 0x06004B42 RID: 19266 RVA: 0x0026F484 File Offset: 0x0026D884
			Public Property inputGridLayoutElement As LayoutElement

			' Token: 0x17000723 RID: 1827
			' (get) Token: 0x06004B43 RID: 19267 RVA: 0x0026F48D File Offset: 0x0026D88D
			' (set) Token: 0x06004B44 RID: 19268 RVA: 0x0026F495 File Offset: 0x0026D895
			Public Property inputGridActionColumn As Transform

			' Token: 0x17000724 RID: 1828
			' (get) Token: 0x06004B45 RID: 19269 RVA: 0x0026F49E File Offset: 0x0026D89E
			' (set) Token: 0x06004B46 RID: 19270 RVA: 0x0026F4A6 File Offset: 0x0026D8A6
			Public Property inputGridKeyboardColumn As Transform

			' Token: 0x17000725 RID: 1829
			' (get) Token: 0x06004B47 RID: 19271 RVA: 0x0026F4AF File Offset: 0x0026D8AF
			' (set) Token: 0x06004B48 RID: 19272 RVA: 0x0026F4B7 File Offset: 0x0026D8B7
			Public Property inputGridMouseColumn As Transform

			' Token: 0x17000726 RID: 1830
			' (get) Token: 0x06004B49 RID: 19273 RVA: 0x0026F4C0 File Offset: 0x0026D8C0
			' (set) Token: 0x06004B4A RID: 19274 RVA: 0x0026F4C8 File Offset: 0x0026D8C8
			Public Property inputGridControllerColumn As Transform

			' Token: 0x17000727 RID: 1831
			' (get) Token: 0x06004B4B RID: 19275 RVA: 0x0026F4D1 File Offset: 0x0026D8D1
			' (set) Token: 0x06004B4C RID: 19276 RVA: 0x0026F4D9 File Offset: 0x0026D8D9
			Public Property inputGridHeader1 As Transform

			' Token: 0x17000728 RID: 1832
			' (get) Token: 0x06004B4D RID: 19277 RVA: 0x0026F4E2 File Offset: 0x0026D8E2
			' (set) Token: 0x06004B4E RID: 19278 RVA: 0x0026F4EA File Offset: 0x0026D8EA
			Public Property inputGridHeader2 As Transform

			' Token: 0x17000729 RID: 1833
			' (get) Token: 0x06004B4F RID: 19279 RVA: 0x0026F4F3 File Offset: 0x0026D8F3
			' (set) Token: 0x06004B50 RID: 19280 RVA: 0x0026F4FB File Offset: 0x0026D8FB
			Public Property inputGridHeader3 As Transform

			' Token: 0x1700072A RID: 1834
			' (get) Token: 0x06004B51 RID: 19281 RVA: 0x0026F504 File Offset: 0x0026D904
			' (set) Token: 0x06004B52 RID: 19282 RVA: 0x0026F50C File Offset: 0x0026D90C
			Public Property inputGridHeader4 As Transform

			' Token: 0x06004B53 RID: 19283 RVA: 0x0026F518 File Offset: 0x0026D918
			Public Function Check() As Boolean
				Return Not(Me._canvas Is Nothing) AndAlso Not(Me._mainCanvasGroup Is Nothing) AndAlso Not(Me._mainContent Is Nothing) AndAlso Not(Me._mainContentInner Is Nothing) AndAlso Not(Me._playersGroup Is Nothing) AndAlso Not(Me._controllerGroup Is Nothing) AndAlso Not(Me._controllerGroupLabelGroup Is Nothing) AndAlso Not(Me._controllerSettingsGroup Is Nothing) AndAlso Not(Me._assignedControllersGroup Is Nothing) AndAlso Not(Me._settingsAndMapCategoriesGroup Is Nothing) AndAlso Not(Me._settingsGroup Is Nothing) AndAlso Not(Me._mapCategoriesGroup Is Nothing) AndAlso Not(Me._inputGridGroup Is Nothing) AndAlso Not(Me._inputGridContainer Is Nothing) AndAlso Not(Me._inputGridHeadersGroup Is Nothing) AndAlso Not(Me._inputGridInnerGroup Is Nothing) AndAlso Not(Me._controllerNameLabel Is Nothing) AndAlso Not(Me._removeControllerButton Is Nothing) AndAlso Not(Me._assignControllerButton Is Nothing) AndAlso Not(Me._calibrateControllerButton Is Nothing) AndAlso Not(Me._doneButton Is Nothing) AndAlso Not(Me._restoreDefaultsButton Is Nothing) AndAlso Not(Me._defaultSelection Is Nothing)
			End Function

			' Token: 0x04005050 RID: 20560
			<SerializeField()>
			Private _canvas As Canvas

			' Token: 0x04005051 RID: 20561
			<SerializeField()>
			Private _mainCanvasGroup As CanvasGroup

			' Token: 0x04005052 RID: 20562
			<SerializeField()>
			Private _mainContent As Transform

			' Token: 0x04005053 RID: 20563
			<SerializeField()>
			Private _mainContentInner As Transform

			' Token: 0x04005054 RID: 20564
			<SerializeField()>
			Private _playersGroup As UIGroup

			' Token: 0x04005055 RID: 20565
			<SerializeField()>
			Private _controllerGroup As Transform

			' Token: 0x04005056 RID: 20566
			<SerializeField()>
			Private _controllerGroupLabelGroup As Transform

			' Token: 0x04005057 RID: 20567
			<SerializeField()>
			Private _controllerSettingsGroup As UIGroup

			' Token: 0x04005058 RID: 20568
			<SerializeField()>
			Private _assignedControllersGroup As UIGroup

			' Token: 0x04005059 RID: 20569
			<SerializeField()>
			Private _settingsAndMapCategoriesGroup As Transform

			' Token: 0x0400505A RID: 20570
			<SerializeField()>
			Private _settingsGroup As UIGroup

			' Token: 0x0400505B RID: 20571
			<SerializeField()>
			Private _mapCategoriesGroup As UIGroup

			' Token: 0x0400505C RID: 20572
			<SerializeField()>
			Private _inputGridGroup As Transform

			' Token: 0x0400505D RID: 20573
			<SerializeField()>
			Private _inputGridContainer As Transform

			' Token: 0x0400505E RID: 20574
			<SerializeField()>
			Private _inputGridHeadersGroup As Transform

			' Token: 0x0400505F RID: 20575
			<SerializeField()>
			Private _inputGridInnerGroup As Transform

			' Token: 0x04005060 RID: 20576
			<SerializeField()>
			Private _actionsColumnHeadersGroup As Transform

			' Token: 0x04005061 RID: 20577
			<SerializeField()>
			Private _actionsColumn As Transform

			' Token: 0x04005062 RID: 20578
			<SerializeField()>
			Private _controllerNameLabel As Text

			' Token: 0x04005063 RID: 20579
			<SerializeField()>
			Private _removeControllerButton As Button

			' Token: 0x04005064 RID: 20580
			<SerializeField()>
			Private _assignControllerButton As Button

			' Token: 0x04005065 RID: 20581
			<SerializeField()>
			Private _calibrateControllerButton As Button

			' Token: 0x04005066 RID: 20582
			<SerializeField()>
			Private _doneButton As Button

			' Token: 0x04005067 RID: 20583
			<SerializeField()>
			Private _restoreDefaultsButton As Button

			' Token: 0x04005068 RID: 20584
			<SerializeField()>
			Private _defaultSelection As Selectable

			' Token: 0x04005069 RID: 20585
			<SerializeField()>
			Private _fixedSelectableUIElements As GameObject()

			' Token: 0x0400506A RID: 20586
			<SerializeField()>
			Private _mainBackgroundImage As Image
		End Class

		' Token: 0x02000C19 RID: 3097
		Private Class InputActionSet
			' Token: 0x06004B54 RID: 19284 RVA: 0x0026F6AF File Offset: 0x0026DAAF
			Public Sub New(actionId As Integer, axisRange As AxisRange)
				Me._actionId = actionId
				Me._axisRange = axisRange
			End Sub

			' Token: 0x1700072B RID: 1835
			' (get) Token: 0x06004B55 RID: 19285 RVA: 0x0026F6C5 File Offset: 0x0026DAC5
			Public ReadOnly Property actionId As Integer
				Get
					Return Me._actionId
				End Get
			End Property

			' Token: 0x1700072C RID: 1836
			' (get) Token: 0x06004B56 RID: 19286 RVA: 0x0026F6CD File Offset: 0x0026DACD
			Public ReadOnly Property axisRange As AxisRange
				Get
					Return Me._axisRange
				End Get
			End Property

			' Token: 0x04005074 RID: 20596
			Private _actionId As Integer

			' Token: 0x04005075 RID: 20597
			Private _axisRange As AxisRange
		End Class

		' Token: 0x02000C1A RID: 3098
		Private Class InputMapping
			' Token: 0x06004B57 RID: 19287 RVA: 0x0026F6D5 File Offset: 0x0026DAD5
			Public Sub New(actionName As String, fieldInfo As InputFieldInfo, map As ControllerMap, aem As ActionElementMap, controllerType As ControllerType, controllerId As Integer)
				Me.actionName = actionName
				Me.fieldInfo = fieldInfo
				Me.map = map
				Me.aem = aem
				Me.controllerType = controllerType
				Me.controllerId = controllerId
			End Sub

			' Token: 0x1700072D RID: 1837
			' (get) Token: 0x06004B58 RID: 19288 RVA: 0x0026F70A File Offset: 0x0026DB0A
			' (set) Token: 0x06004B59 RID: 19289 RVA: 0x0026F712 File Offset: 0x0026DB12
			Public Property actionName As String

			' Token: 0x1700072E RID: 1838
			' (get) Token: 0x06004B5A RID: 19290 RVA: 0x0026F71B File Offset: 0x0026DB1B
			' (set) Token: 0x06004B5B RID: 19291 RVA: 0x0026F723 File Offset: 0x0026DB23
			Public Property fieldInfo As InputFieldInfo

			' Token: 0x1700072F RID: 1839
			' (get) Token: 0x06004B5C RID: 19292 RVA: 0x0026F72C File Offset: 0x0026DB2C
			' (set) Token: 0x06004B5D RID: 19293 RVA: 0x0026F734 File Offset: 0x0026DB34
			Public Property map As ControllerMap

			' Token: 0x17000730 RID: 1840
			' (get) Token: 0x06004B5E RID: 19294 RVA: 0x0026F73D File Offset: 0x0026DB3D
			' (set) Token: 0x06004B5F RID: 19295 RVA: 0x0026F745 File Offset: 0x0026DB45
			Public Property aem As ActionElementMap

			' Token: 0x17000731 RID: 1841
			' (get) Token: 0x06004B60 RID: 19296 RVA: 0x0026F74E File Offset: 0x0026DB4E
			' (set) Token: 0x06004B61 RID: 19297 RVA: 0x0026F756 File Offset: 0x0026DB56
			Public Property controllerType As ControllerType

			' Token: 0x17000732 RID: 1842
			' (get) Token: 0x06004B62 RID: 19298 RVA: 0x0026F75F File Offset: 0x0026DB5F
			' (set) Token: 0x06004B63 RID: 19299 RVA: 0x0026F767 File Offset: 0x0026DB67
			Public Property controllerId As Integer

			' Token: 0x17000733 RID: 1843
			' (get) Token: 0x06004B64 RID: 19300 RVA: 0x0026F770 File Offset: 0x0026DB70
			' (set) Token: 0x06004B65 RID: 19301 RVA: 0x0026F778 File Offset: 0x0026DB78
			Public Property pollingInfo As ControllerPollingInfo

			' Token: 0x17000734 RID: 1844
			' (get) Token: 0x06004B66 RID: 19302 RVA: 0x0026F781 File Offset: 0x0026DB81
			' (set) Token: 0x06004B67 RID: 19303 RVA: 0x0026F789 File Offset: 0x0026DB89
			Public Property modifierKeyFlags As ModifierKeyFlags

			' Token: 0x17000735 RID: 1845
			' (get) Token: 0x06004B68 RID: 19304 RVA: 0x0026F794 File Offset: 0x0026DB94
			Public ReadOnly Property axisRange As AxisRange
				Get
					Dim axisRange As AxisRange = AxisRange.Positive
					If Me.pollingInfo.elementType = ControllerElementType.Axis Then
						If Me.fieldInfo.axisRange = AxisRange.Full Then
							axisRange = AxisRange.Full
						Else
							axisRange = If((Me.pollingInfo.axisPole <> Pole.Positive), AxisRange.Negative, AxisRange.Positive)
						End If
					End If
					Return axisRange
				End Get
			End Property

			' Token: 0x17000736 RID: 1846
			' (get) Token: 0x06004B69 RID: 19305 RVA: 0x0026F7EC File Offset: 0x0026DBEC
			Public ReadOnly Property elementName As String
				Get
					If Me.controllerType = ControllerType.Keyboard AndAlso Me.modifierKeyFlags <> ModifierKeyFlags.None Then
						Return String.Format("{0} + {1}", Keyboard.ModifierKeyFlagsToString(Me.modifierKeyFlags), Me.pollingInfo.elementIdentifierName)
					End If
					Dim text As String = Me.pollingInfo.elementIdentifierName
					If Me.pollingInfo.elementType = ControllerElementType.Axis Then
						If Me.axisRange = AxisRange.Positive Then
							text = Me.pollingInfo.elementIdentifier.positiveName
						ElseIf Me.axisRange = AxisRange.Negative Then
							text = Me.pollingInfo.elementIdentifier.negativeName
						End If
					End If
					Dim translationElement As TranslationElement = Localization.Find(text)
					If translationElement IsNot Nothing Then
						Return translationElement.translations(CInt(Localization.language)).text
					End If
					Return text
				End Get
			End Property

			' Token: 0x06004B6A RID: 19306 RVA: 0x0026F8C3 File Offset: 0x0026DCC3
			Public Function ToElementAssignment(pollingInfo As ControllerPollingInfo) As ElementAssignment
				Me.pollingInfo = pollingInfo
				Return Me.ToElementAssignment()
			End Function

			' Token: 0x06004B6B RID: 19307 RVA: 0x0026F8D2 File Offset: 0x0026DCD2
			Public Function ToElementAssignment(pollingInfo As ControllerPollingInfo, modifierKeyFlags As ModifierKeyFlags) As ElementAssignment
				Me.pollingInfo = pollingInfo
				Me.modifierKeyFlags = modifierKeyFlags
				Return Me.ToElementAssignment()
			End Function

			' Token: 0x06004B6C RID: 19308 RVA: 0x0026F8E8 File Offset: 0x0026DCE8
			Public Function ToElementAssignment() As ElementAssignment
				Return New ElementAssignment(Me.controllerType, Me.pollingInfo.elementType, Me.pollingInfo.elementIdentifierId, Me.axisRange, Me.pollingInfo.keyboardKey, Me.modifierKeyFlags, Me.fieldInfo.actionId, If((Me.fieldInfo.axisRange <> AxisRange.Negative), Pole.Positive, Pole.Negative), False, If((Me.aem Is Nothing), (-1), Me.aem.id))
			End Function
		End Class

		' Token: 0x02000C1B RID: 3099
		Private Class AxisCalibrator
			' Token: 0x06004B6D RID: 19309 RVA: 0x0026F978 File Offset: 0x0026DD78
			Public Sub New(joystick As Joystick, axisIndex As Integer)
				Me.data = Nothing
				Me.joystick = joystick
				Me.axisIndex = axisIndex
				If joystick IsNot Nothing AndAlso axisIndex >= 0 AndAlso joystick.axisCount > axisIndex Then
					Me.axis = joystick.Axes(axisIndex)
					Me.data = joystick.calibrationMap.GetAxis(axisIndex).GetData()
				End If
				Me.firstRun = True
			End Sub

			' Token: 0x17000737 RID: 1847
			' (get) Token: 0x06004B6E RID: 19310 RVA: 0x0026F9F1 File Offset: 0x0026DDF1
			Public ReadOnly Property isValid As Boolean
				Get
					Return Me.axis IsNot Nothing
				End Get
			End Property

			' Token: 0x06004B6F RID: 19311 RVA: 0x0026FA00 File Offset: 0x0026DE00
			Public Sub RecordMinMax()
				If Me.axis Is Nothing Then
					Return
				End If
				Dim valueRaw As Single = Me.axis.valueRaw
				If Me.firstRun OrElse valueRaw < Me.data.min Then
					Me.data.min = valueRaw
				End If
				If Me.firstRun OrElse valueRaw > Me.data.max Then
					Me.data.max = valueRaw
				End If
				Me.firstRun = False
			End Sub

			' Token: 0x06004B70 RID: 19312 RVA: 0x0026FA7C File Offset: 0x0026DE7C
			Public Sub RecordZero()
				If Me.axis Is Nothing Then
					Return
				End If
				Me.data.zero = Me.axis.valueRaw
			End Sub

			' Token: 0x06004B71 RID: 19313 RVA: 0x0026FAA0 File Offset: 0x0026DEA0
			Public Sub Commit()
				If Me.axis Is Nothing Then
					Return
				End If
				Dim axisCalibration As AxisCalibration = Me.joystick.calibrationMap.GetAxis(Me.axisIndex)
				If axisCalibration Is Nothing Then
					Return
				End If
				If CDbl(Mathf.Abs(Me.data.max - Me.data.min)) < 0.1 Then
					Return
				End If
				axisCalibration.SetData(Me.data)
			End Sub

			' Token: 0x0400507E RID: 20606
			Public data As AxisCalibrationData

			' Token: 0x0400507F RID: 20607
			Public joystick As Joystick

			' Token: 0x04005080 RID: 20608
			Public axisIndex As Integer

			' Token: 0x04005081 RID: 20609
			Private axis As Controller.Axis

			' Token: 0x04005082 RID: 20610
			Private firstRun As Boolean
		End Class

		' Token: 0x02000C1C RID: 3100
		Private Class IndexedDictionary(Of TKey, TValue)
			' Token: 0x06004B72 RID: 19314 RVA: 0x0026FB0F File Offset: 0x0026DF0F
			Public Sub New()
				Me.list = New List(Of ControlMapper.IndexedDictionary(Of TKey, TValue).Entry)()
			End Sub

			' Token: 0x17000738 RID: 1848
			' (get) Token: 0x06004B73 RID: 19315 RVA: 0x0026FB22 File Offset: 0x0026DF22
			Public ReadOnly Property Count As Integer
				Get
					Return Me.list.Count
				End Get
			End Property

			' Token: 0x17000739 RID: 1849
			Public ReadOnly Default Property Item(index As Integer) As TValue
				Get
					Return Me.list(index).value
				End Get
			End Property

			' Token: 0x06004B75 RID: 19317 RVA: 0x0026FB44 File Offset: 0x0026DF44
			Public Function [Get](key As TKey) As TValue
				Dim num As Integer = Me.IndexOfKey(key)
				If num < 0 Then
					Throw New Exception("Key does not exist!")
				End If
				Return Me.list(num).value
			End Function

			' Token: 0x06004B76 RID: 19318 RVA: 0x0026FB7C File Offset: 0x0026DF7C
			Public Function TryGet(key As TKey, <System.Runtime.InteropServices.OutAttribute()> ByRef value As TValue) As Boolean
				value = Nothing
				Dim num As Integer = Me.IndexOfKey(key)
				If num < 0 Then
					Return False
				End If
				value = Me.list(num).value
				Return True
			End Function

			' Token: 0x06004B77 RID: 19319 RVA: 0x0026FBC4 File Offset: 0x0026DFC4
			Public Sub Add(key As TKey, value As TValue)
				If Me.ContainsKey(key) Then
					Throw New Exception("Key " + key.ToString() + " is already in use!")
				End If
				Me.list.Add(New ControlMapper.IndexedDictionary(Of TKey, TValue).Entry(key, value))
			End Sub

			' Token: 0x06004B78 RID: 19320 RVA: 0x0026FC14 File Offset: 0x0026E014
			Public Function IndexOfKey(key As TKey) As Integer
				Dim count As Integer = Me.list.Count
				For i As Integer = 0 To count - 1
					If EqualityComparer(Of TKey).[Default].Equals(Me.list(i).key, key) Then
						Return i
					End If
				Next
				Return -1
			End Function

			' Token: 0x06004B79 RID: 19321 RVA: 0x0026FC64 File Offset: 0x0026E064
			Public Function ContainsKey(key As TKey) As Boolean
				Dim count As Integer = Me.list.Count
				For i As Integer = 0 To count - 1
					If EqualityComparer(Of TKey).[Default].Equals(Me.list(i).key, key) Then
						Return True
					End If
				Next
				Return False
			End Function

			' Token: 0x06004B7A RID: 19322 RVA: 0x0026FCB3 File Offset: 0x0026E0B3
			Public Sub Clear()
				Me.list.Clear()
			End Sub

			' Token: 0x04005083 RID: 20611
			Private list As List(Of ControlMapper.IndexedDictionary(Of TKey, TValue).Entry)

			' Token: 0x02000C1D RID: 3101
			Private Class Entry
				' Token: 0x06004B7B RID: 19323 RVA: 0x0026FCC0 File Offset: 0x0026E0C0
				Public Sub New(key As TKey, value As TValue)
					Me.key = key
					Me.value = value
				End Sub

				' Token: 0x04005084 RID: 20612
				Public key As TKey

				' Token: 0x04005085 RID: 20613
				Public value As TValue
			End Class
		End Class

		' Token: 0x02000C1E RID: 3102
		Private Enum LayoutElementSizeType
			' Token: 0x04005087 RID: 20615
			MinSize
			' Token: 0x04005088 RID: 20616
			PreferredSize
		End Enum

		' Token: 0x02000C1F RID: 3103
		Private Enum WindowType
			' Token: 0x0400508A RID: 20618
			None
			' Token: 0x0400508B RID: 20619
			ChooseJoystick
			' Token: 0x0400508C RID: 20620
			JoystickAssignmentConflict
			' Token: 0x0400508D RID: 20621
			ElementAssignment
			' Token: 0x0400508E RID: 20622
			ElementAssignmentPrePolling
			' Token: 0x0400508F RID: 20623
			ElementAssignmentPolling
			' Token: 0x04005090 RID: 20624
			ElementAssignmentResult
			' Token: 0x04005091 RID: 20625
			ElementAssignmentConflict
			' Token: 0x04005092 RID: 20626
			Calibration
			' Token: 0x04005093 RID: 20627
			CalibrateStep1
			' Token: 0x04005094 RID: 20628
			CalibrateStep2
		End Enum

		' Token: 0x02000C20 RID: 3104
		Private Class InputGrid
			' Token: 0x06004B7C RID: 19324 RVA: 0x0026FCD6 File Offset: 0x0026E0D6
			Public Sub New()
				Me.list = New ControlMapper.InputGridEntryList()
				Me.groups = New List(Of GameObject)()
			End Sub

			' Token: 0x06004B7D RID: 19325 RVA: 0x0026FCF4 File Offset: 0x0026E0F4
			Public Sub AddMapCategory(mapCategoryId As Integer)
				Me.list.AddMapCategory(mapCategoryId)
			End Sub

			' Token: 0x06004B7E RID: 19326 RVA: 0x0026FD02 File Offset: 0x0026E102
			Public Sub AddAction(mapCategoryId As Integer, action As InputAction, axisRange As AxisRange)
				Me.list.AddAction(mapCategoryId, action, axisRange)
			End Sub

			' Token: 0x06004B7F RID: 19327 RVA: 0x0026FD12 File Offset: 0x0026E112
			Public Sub AddActionCategory(mapCategoryId As Integer, actionCategoryId As Integer)
				Me.list.AddActionCategory(mapCategoryId, actionCategoryId)
			End Sub

			' Token: 0x06004B80 RID: 19328 RVA: 0x0026FD21 File Offset: 0x0026E121
			Public Sub AddInputFieldSet(mapCategoryId As Integer, action As InputAction, axisRange As AxisRange, controllerType As ControllerType, fieldSetContainer As GameObject)
				Me.list.AddInputFieldSet(mapCategoryId, action, axisRange, controllerType, fieldSetContainer)
			End Sub

			' Token: 0x06004B81 RID: 19329 RVA: 0x0026FD35 File Offset: 0x0026E135
			Public Sub AddInputField(mapCategoryId As Integer, action As InputAction, axisRange As AxisRange, controllerType As ControllerType, fieldIndex As Integer, inputField As ControlMapper.GUIInputField)
				Me.list.AddInputField(mapCategoryId, action, axisRange, controllerType, fieldIndex, inputField)
			End Sub

			' Token: 0x06004B82 RID: 19330 RVA: 0x0026FD4B File Offset: 0x0026E14B
			Public Sub AddGroup(group As GameObject)
				Me.groups.Add(group)
			End Sub

			' Token: 0x06004B83 RID: 19331 RVA: 0x0026FD59 File Offset: 0x0026E159
			Public Sub AddActionLabel(mapCategoryId As Integer, actionId As Integer, axisRange As AxisRange, label As ControlMapper.GUILabel)
				Me.list.AddActionLabel(mapCategoryId, actionId, axisRange, label)
			End Sub

			' Token: 0x06004B84 RID: 19332 RVA: 0x0026FD6B File Offset: 0x0026E16B
			Public Sub AddActionCategoryLabel(mapCategoryId As Integer, actionCategoryId As Integer, label As ControlMapper.GUILabel)
				Me.list.AddActionCategoryLabel(mapCategoryId, actionCategoryId, label)
			End Sub

			' Token: 0x06004B85 RID: 19333 RVA: 0x0026FD7B File Offset: 0x0026E17B
			Public Function Contains(mapCategoryId As Integer, actionId As Integer, axisRange As AxisRange, controllerType As ControllerType, fieldIndex As Integer) As Boolean
				Return Me.list.Contains(mapCategoryId, actionId, axisRange, controllerType, fieldIndex)
			End Function

			' Token: 0x06004B86 RID: 19334 RVA: 0x0026FD8F File Offset: 0x0026E18F
			Public Function GetGUIInputField(mapCategoryId As Integer, actionId As Integer, axisRange As AxisRange, controllerType As ControllerType, fieldIndex As Integer) As ControlMapper.GUIInputField
				Return Me.list.GetGUIInputField(mapCategoryId, actionId, axisRange, controllerType, fieldIndex)
			End Function

			' Token: 0x06004B87 RID: 19335 RVA: 0x0026FDA3 File Offset: 0x0026E1A3
			Public Function GetActionSets(mapCategoryId As Integer) As IEnumerable(Of ControlMapper.InputActionSet)
				Return Me.list.GetActionSets(mapCategoryId)
			End Function

			' Token: 0x06004B88 RID: 19336 RVA: 0x0026FDB1 File Offset: 0x0026E1B1
			Public Sub SetColumnHeight(mapCategoryId As Integer, height As Single)
				Me.list.SetColumnHeight(mapCategoryId, height)
			End Sub

			' Token: 0x06004B89 RID: 19337 RVA: 0x0026FDC0 File Offset: 0x0026E1C0
			Public Function GetColumnHeight(mapCategoryId As Integer) As Single
				Return Me.list.GetColumnHeight(mapCategoryId)
			End Function

			' Token: 0x06004B8A RID: 19338 RVA: 0x0026FDCE File Offset: 0x0026E1CE
			Public Sub SetFieldsActive(mapCategoryId As Integer, state As Boolean)
				Me.list.SetFieldsActive(mapCategoryId, state)
			End Sub

			' Token: 0x06004B8B RID: 19339 RVA: 0x0026FDDD File Offset: 0x0026E1DD
			Public Sub SetFieldLabel(mapCategoryId As Integer, actionId As Integer, axisRange As AxisRange, controllerType As ControllerType, index As Integer, label As String)
				Me.list.SetLabel(mapCategoryId, actionId, axisRange, controllerType, index, label)
			End Sub

			' Token: 0x06004B8C RID: 19340 RVA: 0x0026FDF4 File Offset: 0x0026E1F4
			Public Sub PopulateField(mapCategoryId As Integer, actionId As Integer, axisRange As AxisRange, controllerType As ControllerType, controllerId As Integer, index As Integer, actionElementMapId As Integer, label As String, invert As Boolean)
				Me.list.PopulateField(mapCategoryId, actionId, axisRange, controllerType, controllerId, index, actionElementMapId, label, invert)
			End Sub

			' Token: 0x06004B8D RID: 19341 RVA: 0x0026FE1B File Offset: 0x0026E21B
			Public Sub SetFixedFieldData(mapCategoryId As Integer, actionId As Integer, axisRange As AxisRange, controllerType As ControllerType, controllerId As Integer)
				Me.list.SetFixedFieldData(mapCategoryId, actionId, axisRange, controllerType, controllerId)
			End Sub

			' Token: 0x06004B8E RID: 19342 RVA: 0x0026FE2F File Offset: 0x0026E22F
			Public Sub InitializeFields(mapCategoryId As Integer)
				Me.list.InitializeFields(mapCategoryId)
			End Sub

			' Token: 0x06004B8F RID: 19343 RVA: 0x0026FE3D File Offset: 0x0026E23D
			Public Sub Show(mapCategoryId As Integer)
				Me.list.Show(mapCategoryId)
			End Sub

			' Token: 0x06004B90 RID: 19344 RVA: 0x0026FE4B File Offset: 0x0026E24B
			Public Sub HideAll()
				Me.list.HideAll()
			End Sub

			' Token: 0x06004B91 RID: 19345 RVA: 0x0026FE58 File Offset: 0x0026E258
			Public Sub ClearLabels(mapCategoryId As Integer)
				Me.list.ClearLabels(mapCategoryId)
			End Sub

			' Token: 0x06004B92 RID: 19346 RVA: 0x0026FE68 File Offset: 0x0026E268
			Private Sub ClearGroups()
				For i As Integer = 0 To Me.groups.Count - 1
					If Not(Me.groups(i) Is Nothing) Then
						Global.UnityEngine.[Object].Destroy(Me.groups(i))
					End If
				Next
			End Sub

			' Token: 0x06004B93 RID: 19347 RVA: 0x0026FEBE File Offset: 0x0026E2BE
			Public Sub ClearAll()
				Me.ClearGroups()
				Me.list.Clear()
			End Sub

			' Token: 0x04005095 RID: 20629
			Private list As ControlMapper.InputGridEntryList

			' Token: 0x04005096 RID: 20630
			Private groups As List(Of GameObject)
		End Class

		' Token: 0x02000C21 RID: 3105
		Private Class InputGridEntryList
			' Token: 0x06004B94 RID: 19348 RVA: 0x0026FED1 File Offset: 0x0026E2D1
			Public Sub New()
				Me.entries = New ControlMapper.IndexedDictionary(Of Integer, ControlMapper.InputGridEntryList.MapCategoryEntry)()
			End Sub

			' Token: 0x06004B95 RID: 19349 RVA: 0x0026FEE4 File Offset: 0x0026E2E4
			Public Sub AddMapCategory(mapCategoryId As Integer)
				If mapCategoryId < 0 Then
					Return
				End If
				If Me.entries.ContainsKey(mapCategoryId) Then
					Return
				End If
				Me.entries.Add(mapCategoryId, New ControlMapper.InputGridEntryList.MapCategoryEntry())
			End Sub

			' Token: 0x06004B96 RID: 19350 RVA: 0x0026FF11 File Offset: 0x0026E311
			Public Sub AddAction(mapCategoryId As Integer, action As InputAction, axisRange As AxisRange)
				Me.AddActionEntry(mapCategoryId, action, axisRange)
			End Sub

			' Token: 0x06004B97 RID: 19351 RVA: 0x0026FF20 File Offset: 0x0026E320
			Private Function AddActionEntry(mapCategoryId As Integer, action As InputAction, axisRange As AxisRange) As ControlMapper.InputGridEntryList.ActionEntry
				If action Is Nothing Then
					Return Nothing
				End If
				Dim mapCategoryEntry As ControlMapper.InputGridEntryList.MapCategoryEntry
				If Not Me.entries.TryGet(mapCategoryId, mapCategoryEntry) Then
					Return Nothing
				End If
				Return mapCategoryEntry.AddAction(action, axisRange)
			End Function

			' Token: 0x06004B98 RID: 19352 RVA: 0x0026FF54 File Offset: 0x0026E354
			Public Sub AddActionLabel(mapCategoryId As Integer, actionId As Integer, axisRange As AxisRange, label As ControlMapper.GUILabel)
				Dim mapCategoryEntry As ControlMapper.InputGridEntryList.MapCategoryEntry
				If Not Me.entries.TryGet(mapCategoryId, mapCategoryEntry) Then
					Return
				End If
				Dim actionEntry As ControlMapper.InputGridEntryList.ActionEntry = mapCategoryEntry.GetActionEntry(actionId, axisRange)
				If actionEntry Is Nothing Then
					Return
				End If
				actionEntry.SetLabel(label)
			End Sub

			' Token: 0x06004B99 RID: 19353 RVA: 0x0026FF8D File Offset: 0x0026E38D
			Public Sub AddActionCategory(mapCategoryId As Integer, actionCategoryId As Integer)
				Me.AddActionCategoryEntry(mapCategoryId, actionCategoryId)
			End Sub

			' Token: 0x06004B9A RID: 19354 RVA: 0x0026FF98 File Offset: 0x0026E398
			Private Function AddActionCategoryEntry(mapCategoryId As Integer, actionCategoryId As Integer) As ControlMapper.InputGridEntryList.ActionCategoryEntry
				Dim mapCategoryEntry As ControlMapper.InputGridEntryList.MapCategoryEntry
				If Not Me.entries.TryGet(mapCategoryId, mapCategoryEntry) Then
					Return Nothing
				End If
				Return mapCategoryEntry.AddActionCategory(actionCategoryId)
			End Function

			' Token: 0x06004B9B RID: 19355 RVA: 0x0026FFC4 File Offset: 0x0026E3C4
			Public Sub AddActionCategoryLabel(mapCategoryId As Integer, actionCategoryId As Integer, label As ControlMapper.GUILabel)
				Dim mapCategoryEntry As ControlMapper.InputGridEntryList.MapCategoryEntry
				If Not Me.entries.TryGet(mapCategoryId, mapCategoryEntry) Then
					Return
				End If
				Dim actionCategoryEntry As ControlMapper.InputGridEntryList.ActionCategoryEntry = mapCategoryEntry.GetActionCategoryEntry(actionCategoryId)
				If actionCategoryEntry Is Nothing Then
					Return
				End If
				actionCategoryEntry.SetLabel(label)
			End Sub

			' Token: 0x06004B9C RID: 19356 RVA: 0x0026FFFC File Offset: 0x0026E3FC
			Public Sub AddInputFieldSet(mapCategoryId As Integer, action As InputAction, axisRange As AxisRange, controllerType As ControllerType, fieldSetContainer As GameObject)
				Dim actionEntry As ControlMapper.InputGridEntryList.ActionEntry = Me.GetActionEntry(mapCategoryId, action, axisRange)
				If actionEntry Is Nothing Then
					Return
				End If
				actionEntry.AddInputFieldSet(controllerType, fieldSetContainer)
			End Sub

			' Token: 0x06004B9D RID: 19357 RVA: 0x00270024 File Offset: 0x0026E424
			Public Sub AddInputField(mapCategoryId As Integer, action As InputAction, axisRange As AxisRange, controllerType As ControllerType, fieldIndex As Integer, inputField As ControlMapper.GUIInputField)
				Dim actionEntry As ControlMapper.InputGridEntryList.ActionEntry = Me.GetActionEntry(mapCategoryId, action, axisRange)
				If actionEntry Is Nothing Then
					Return
				End If
				actionEntry.AddInputField(controllerType, fieldIndex, inputField)
			End Sub

			' Token: 0x06004B9E RID: 19358 RVA: 0x0027004E File Offset: 0x0026E44E
			Public Function Contains(mapCategoryId As Integer, actionId As Integer, axisRange As AxisRange) As Boolean
				Return Me.GetActionEntry(mapCategoryId, actionId, axisRange) IsNot Nothing
			End Function

			' Token: 0x06004B9F RID: 19359 RVA: 0x00270060 File Offset: 0x0026E460
			Public Function Contains(mapCategoryId As Integer, actionId As Integer, axisRange As AxisRange, controllerType As ControllerType, fieldIndex As Integer) As Boolean
				Dim actionEntry As ControlMapper.InputGridEntryList.ActionEntry = Me.GetActionEntry(mapCategoryId, actionId, axisRange)
				Return actionEntry IsNot Nothing AndAlso actionEntry.Contains(controllerType, fieldIndex)
			End Function

			' Token: 0x06004BA0 RID: 19360 RVA: 0x0027008C File Offset: 0x0026E48C
			Public Sub SetColumnHeight(mapCategoryId As Integer, height As Single)
				Dim mapCategoryEntry As ControlMapper.InputGridEntryList.MapCategoryEntry
				If Not Me.entries.TryGet(mapCategoryId, mapCategoryEntry) Then
					Return
				End If
				mapCategoryEntry.columnHeight = height
			End Sub

			' Token: 0x06004BA1 RID: 19361 RVA: 0x002700B4 File Offset: 0x0026E4B4
			Public Function GetColumnHeight(mapCategoryId As Integer) As Single
				Dim mapCategoryEntry As ControlMapper.InputGridEntryList.MapCategoryEntry
				If Not Me.entries.TryGet(mapCategoryId, mapCategoryEntry) Then
					Return 0F
				End If
				Return mapCategoryEntry.columnHeight
			End Function

			' Token: 0x06004BA2 RID: 19362 RVA: 0x002700E0 File Offset: 0x0026E4E0
			Public Function GetGUIInputField(mapCategoryId As Integer, actionId As Integer, axisRange As AxisRange, controllerType As ControllerType, fieldIndex As Integer) As ControlMapper.GUIInputField
				Dim actionEntry As ControlMapper.InputGridEntryList.ActionEntry = Me.GetActionEntry(mapCategoryId, actionId, axisRange)
				If actionEntry Is Nothing Then
					Return Nothing
				End If
				Return actionEntry.GetGUIInputField(controllerType, fieldIndex)
			End Function

			' Token: 0x06004BA3 RID: 19363 RVA: 0x0027010C File Offset: 0x0026E50C
			Private Function GetActionEntry(mapCategoryId As Integer, actionId As Integer, axisRange As AxisRange) As ControlMapper.InputGridEntryList.ActionEntry
				If actionId < 0 Then
					Return Nothing
				End If
				Dim mapCategoryEntry As ControlMapper.InputGridEntryList.MapCategoryEntry
				If Not Me.entries.TryGet(mapCategoryId, mapCategoryEntry) Then
					Return Nothing
				End If
				Return mapCategoryEntry.GetActionEntry(actionId, axisRange)
			End Function

			' Token: 0x06004BA4 RID: 19364 RVA: 0x00270141 File Offset: 0x0026E541
			Private Function GetActionEntry(mapCategoryId As Integer, action As InputAction, axisRange As AxisRange) As ControlMapper.InputGridEntryList.ActionEntry
				If action Is Nothing Then
					Return Nothing
				End If
				Return Me.GetActionEntry(mapCategoryId, action.id, axisRange)
			End Function

			' Token: 0x06004BA5 RID: 19365 RVA: 0x0027015C File Offset: 0x0026E55C
			Public Iterator Function GetActionSets(mapCategoryId As Integer) As IEnumerable(Of ControlMapper.InputActionSet)
				Dim entry As ControlMapper.InputGridEntryList.MapCategoryEntry
				If Not Me.entries.TryGet(mapCategoryId, entry) Then
					Return
				End If
				Dim list As List(Of ControlMapper.InputGridEntryList.ActionEntry) = entry.actionList
				Dim count As Integer = If((list Is Nothing), 0, list.Count)
				For i As Integer = 0 To count - 1
					Yield list(i).actionSet
				Next
				Return
			End Function

			' Token: 0x06004BA6 RID: 19366 RVA: 0x00270188 File Offset: 0x0026E588
			Public Sub SetFieldsActive(mapCategoryId As Integer, state As Boolean)
				Dim mapCategoryEntry As ControlMapper.InputGridEntryList.MapCategoryEntry
				If Not Me.entries.TryGet(mapCategoryId, mapCategoryEntry) Then
					Return
				End If
				Dim actionList As List(Of ControlMapper.InputGridEntryList.ActionEntry) = mapCategoryEntry.actionList
				Dim num As Integer = If((actionList Is Nothing), 0, actionList.Count)
				For i As Integer = 0 To num - 1
					actionList(i).SetFieldsActive(state)
				Next
			End Sub

			' Token: 0x06004BA7 RID: 19367 RVA: 0x002701E4 File Offset: 0x0026E5E4
			Public Sub SetLabel(mapCategoryId As Integer, actionId As Integer, axisRange As AxisRange, controllerType As ControllerType, index As Integer, label As String)
				Dim actionEntry As ControlMapper.InputGridEntryList.ActionEntry = Me.GetActionEntry(mapCategoryId, actionId, axisRange)
				If actionEntry Is Nothing Then
					Return
				End If
				actionEntry.SetFieldLabel(controllerType, index, label)
			End Sub

			' Token: 0x06004BA8 RID: 19368 RVA: 0x00270210 File Offset: 0x0026E610
			Public Sub PopulateField(mapCategoryId As Integer, actionId As Integer, axisRange As AxisRange, controllerType As ControllerType, controllerId As Integer, index As Integer, actionElementMapId As Integer, label As String, invert As Boolean)
				Dim actionEntry As ControlMapper.InputGridEntryList.ActionEntry = Me.GetActionEntry(mapCategoryId, actionId, axisRange)
				If actionEntry Is Nothing Then
					Return
				End If
				actionEntry.PopulateField(controllerType, controllerId, index, actionElementMapId, label, invert)
			End Sub

			' Token: 0x06004BA9 RID: 19369 RVA: 0x00270240 File Offset: 0x0026E640
			Public Sub SetFixedFieldData(mapCategoryId As Integer, actionId As Integer, axisRange As AxisRange, controllerType As ControllerType, controllerId As Integer)
				Dim actionEntry As ControlMapper.InputGridEntryList.ActionEntry = Me.GetActionEntry(mapCategoryId, actionId, axisRange)
				If actionEntry Is Nothing Then
					Return
				End If
				actionEntry.SetFixedFieldData(controllerType, controllerId)
			End Sub

			' Token: 0x06004BAA RID: 19370 RVA: 0x00270268 File Offset: 0x0026E668
			Public Sub InitializeFields(mapCategoryId As Integer)
				Dim mapCategoryEntry As ControlMapper.InputGridEntryList.MapCategoryEntry
				If Not Me.entries.TryGet(mapCategoryId, mapCategoryEntry) Then
					Return
				End If
				Dim actionList As List(Of ControlMapper.InputGridEntryList.ActionEntry) = mapCategoryEntry.actionList
				Dim num As Integer = If((actionList Is Nothing), 0, actionList.Count)
				For i As Integer = 0 To num - 1
					actionList(i).Initialize()
				Next
			End Sub

			' Token: 0x06004BAB RID: 19371 RVA: 0x002702C4 File Offset: 0x0026E6C4
			Public Sub Show(mapCategoryId As Integer)
				Dim mapCategoryEntry As ControlMapper.InputGridEntryList.MapCategoryEntry
				If Not Me.entries.TryGet(mapCategoryId, mapCategoryEntry) Then
					Return
				End If
				mapCategoryEntry.SetAllActive(True)
			End Sub

			' Token: 0x06004BAC RID: 19372 RVA: 0x002702EC File Offset: 0x0026E6EC
			Public Sub HideAll()
				For i As Integer = 0 To Me.entries.Count - 1
					Me.entries(i).SetAllActive(False)
				Next
			End Sub

			' Token: 0x06004BAD RID: 19373 RVA: 0x00270328 File Offset: 0x0026E728
			Public Sub ClearLabels(mapCategoryId As Integer)
				Dim mapCategoryEntry As ControlMapper.InputGridEntryList.MapCategoryEntry
				If Not Me.entries.TryGet(mapCategoryId, mapCategoryEntry) Then
					Return
				End If
				Dim actionList As List(Of ControlMapper.InputGridEntryList.ActionEntry) = mapCategoryEntry.actionList
				Dim num As Integer = If((actionList Is Nothing), 0, actionList.Count)
				For i As Integer = 0 To num - 1
					actionList(i).ClearLabels()
				Next
			End Sub

			' Token: 0x06004BAE RID: 19374 RVA: 0x00270381 File Offset: 0x0026E781
			Public Sub Clear()
				Me.entries.Clear()
			End Sub

			' Token: 0x04005097 RID: 20631
			Private entries As ControlMapper.IndexedDictionary(Of Integer, ControlMapper.InputGridEntryList.MapCategoryEntry)

			' Token: 0x02000C22 RID: 3106
			Private Class MapCategoryEntry
				' Token: 0x06004BAF RID: 19375 RVA: 0x0027038E File Offset: 0x0026E78E
				Public Sub New()
					Me._actionList = New List(Of ControlMapper.InputGridEntryList.ActionEntry)()
					Me._actionCategoryList = New ControlMapper.IndexedDictionary(Of Integer, ControlMapper.InputGridEntryList.ActionCategoryEntry)()
				End Sub

				' Token: 0x1700073A RID: 1850
				' (get) Token: 0x06004BB0 RID: 19376 RVA: 0x002703AC File Offset: 0x0026E7AC
				Public ReadOnly Property actionList As List(Of ControlMapper.InputGridEntryList.ActionEntry)
					Get
						Return Me._actionList
					End Get
				End Property

				' Token: 0x1700073B RID: 1851
				' (get) Token: 0x06004BB1 RID: 19377 RVA: 0x002703B4 File Offset: 0x0026E7B4
				Public ReadOnly Property actionCategoryList As ControlMapper.IndexedDictionary(Of Integer, ControlMapper.InputGridEntryList.ActionCategoryEntry)
					Get
						Return Me._actionCategoryList
					End Get
				End Property

				' Token: 0x1700073C RID: 1852
				' (get) Token: 0x06004BB2 RID: 19378 RVA: 0x002703BC File Offset: 0x0026E7BC
				' (set) Token: 0x06004BB3 RID: 19379 RVA: 0x002703C4 File Offset: 0x0026E7C4
				Public Property columnHeight As Single
					Get
						Return Me._columnHeight
					End Get
					Set(value As Single)
						Me._columnHeight = value
					End Set
				End Property

				' Token: 0x06004BB4 RID: 19380 RVA: 0x002703D0 File Offset: 0x0026E7D0
				Public Function GetActionEntry(actionId As Integer, axisRange As AxisRange) As ControlMapper.InputGridEntryList.ActionEntry
					Dim num As Integer = Me.IndexOfActionEntry(actionId, axisRange)
					If num < 0 Then
						Return Nothing
					End If
					Return Me._actionList(num)
				End Function

				' Token: 0x06004BB5 RID: 19381 RVA: 0x002703FC File Offset: 0x0026E7FC
				Public Function IndexOfActionEntry(actionId As Integer, axisRange As AxisRange) As Integer
					Dim count As Integer = Me._actionList.Count
					For i As Integer = 0 To count - 1
						If Me._actionList(i).Matches(actionId, axisRange) Then
							Return i
						End If
					Next
					Return -1
				End Function

				' Token: 0x06004BB6 RID: 19382 RVA: 0x00270442 File Offset: 0x0026E842
				Public Function ContainsActionEntry(actionId As Integer, axisRange As AxisRange) As Boolean
					Return Me.IndexOfActionEntry(actionId, axisRange) >= 0
				End Function

				' Token: 0x06004BB7 RID: 19383 RVA: 0x00270454 File Offset: 0x0026E854
				Public Function AddAction(action As InputAction, axisRange As AxisRange) As ControlMapper.InputGridEntryList.ActionEntry
					If action Is Nothing Then
						Return Nothing
					End If
					If Me.ContainsActionEntry(action.id, axisRange) Then
						Return Nothing
					End If
					Me._actionList.Add(New ControlMapper.InputGridEntryList.ActionEntry(action, axisRange))
					Return Me._actionList(Me._actionList.Count - 1)
				End Function

				' Token: 0x06004BB8 RID: 19384 RVA: 0x002704A7 File Offset: 0x0026E8A7
				Public Function GetActionCategoryEntry(actionCategoryId As Integer) As ControlMapper.InputGridEntryList.ActionCategoryEntry
					If Not Me._actionCategoryList.ContainsKey(actionCategoryId) Then
						Return Nothing
					End If
					Return Me._actionCategoryList.[Get](actionCategoryId)
				End Function

				' Token: 0x06004BB9 RID: 19385 RVA: 0x002704C8 File Offset: 0x0026E8C8
				Public Function AddActionCategory(actionCategoryId As Integer) As ControlMapper.InputGridEntryList.ActionCategoryEntry
					If actionCategoryId < 0 Then
						Return Nothing
					End If
					If Me._actionCategoryList.ContainsKey(actionCategoryId) Then
						Return Nothing
					End If
					Me._actionCategoryList.Add(actionCategoryId, New ControlMapper.InputGridEntryList.ActionCategoryEntry(actionCategoryId))
					Return Me._actionCategoryList.[Get](actionCategoryId)
				End Function

				' Token: 0x06004BBA RID: 19386 RVA: 0x00270504 File Offset: 0x0026E904
				Public Sub SetAllActive(state As Boolean)
					For i As Integer = 0 To Me._actionCategoryList.Count - 1
						Me._actionCategoryList(i).SetActive(state)
					Next
					For j As Integer = 0 To Me._actionList.Count - 1
						Me._actionList(j).SetActive(state)
					Next
				End Sub

				' Token: 0x04005098 RID: 20632
				Private _actionList As List(Of ControlMapper.InputGridEntryList.ActionEntry)

				' Token: 0x04005099 RID: 20633
				Private _actionCategoryList As ControlMapper.IndexedDictionary(Of Integer, ControlMapper.InputGridEntryList.ActionCategoryEntry)

				' Token: 0x0400509A RID: 20634
				Private _columnHeight As Single
			End Class

			' Token: 0x02000C23 RID: 3107
			Private Class ActionEntry
				' Token: 0x06004BBB RID: 19387 RVA: 0x0027056D File Offset: 0x0026E96D
				Public Sub New(action As InputAction, axisRange As AxisRange)
					Me.action = action
					Me.axisRange = axisRange
					Me.actionSet = New ControlMapper.InputActionSet(action.id, axisRange)
					Me.fieldSets = New ControlMapper.IndexedDictionary(Of Integer, ControlMapper.InputGridEntryList.FieldSet)()
				End Sub

				' Token: 0x06004BBC RID: 19388 RVA: 0x002705A0 File Offset: 0x0026E9A0
				Public Sub SetLabel(label As ControlMapper.GUILabel)
					Me.label = label
				End Sub

				' Token: 0x06004BBD RID: 19389 RVA: 0x002705A9 File Offset: 0x0026E9A9
				Public Function Matches(actionId As Integer, axisRange As AxisRange) As Boolean
					Return Me.action.id = actionId AndAlso Me.axisRange = axisRange
				End Function

				' Token: 0x06004BBE RID: 19390 RVA: 0x002705CD File Offset: 0x0026E9CD
				Public Sub AddInputFieldSet(controllerType As ControllerType, fieldSetContainer As GameObject)
					If Me.fieldSets.ContainsKey(CInt(controllerType)) Then
						Return
					End If
					Me.fieldSets.Add(CInt(controllerType), New ControlMapper.InputGridEntryList.FieldSet(fieldSetContainer))
				End Sub

				' Token: 0x06004BBF RID: 19391 RVA: 0x002705F4 File Offset: 0x0026E9F4
				Public Sub AddInputField(controllerType As ControllerType, fieldIndex As Integer, inputField As ControlMapper.GUIInputField)
					If Not Me.fieldSets.ContainsKey(CInt(controllerType)) Then
						Return
					End If
					Dim fieldSet As ControlMapper.InputGridEntryList.FieldSet = Me.fieldSets.[Get](CInt(controllerType))
					If fieldSet.fields.ContainsKey(fieldIndex) Then
						Return
					End If
					fieldSet.fields.Add(fieldIndex, inputField)
				End Sub

				' Token: 0x06004BC0 RID: 19392 RVA: 0x00270640 File Offset: 0x0026EA40
				Public Function GetGUIInputField(controllerType As ControllerType, fieldIndex As Integer) As ControlMapper.GUIInputField
					If Not Me.fieldSets.ContainsKey(CInt(controllerType)) Then
						Return Nothing
					End If
					If Not Me.fieldSets.[Get](CInt(controllerType)).fields.ContainsKey(fieldIndex) Then
						Return Nothing
					End If
					Return Me.fieldSets.[Get](CInt(controllerType)).fields.[Get](fieldIndex)
				End Function

				' Token: 0x06004BC1 RID: 19393 RVA: 0x00270695 File Offset: 0x0026EA95
				Public Function Contains(controllerType As ControllerType, fieldId As Integer) As Boolean
					Return Me.fieldSets.ContainsKey(CInt(controllerType)) AndAlso Me.fieldSets.[Get](CInt(controllerType)).fields.ContainsKey(fieldId)
				End Function

				' Token: 0x06004BC2 RID: 19394 RVA: 0x002706CC File Offset: 0x0026EACC
				Public Sub SetFieldLabel(controllerType As ControllerType, index As Integer, label As String)
					If Not Me.fieldSets.ContainsKey(CInt(controllerType)) Then
						Return
					End If
					If Not Me.fieldSets.[Get](CInt(controllerType)).fields.ContainsKey(index) Then
						Return
					End If
					Me.fieldSets.[Get](CInt(controllerType)).fields.[Get](index).SetLabel(label)
				End Sub

				' Token: 0x06004BC3 RID: 19395 RVA: 0x00270728 File Offset: 0x0026EB28
				Public Sub PopulateField(controllerType As ControllerType, controllerId As Integer, index As Integer, actionElementMapId As Integer, label As String, invert As Boolean)
					If Not Me.fieldSets.ContainsKey(CInt(controllerType)) Then
						Return
					End If
					If Not Me.fieldSets.[Get](CInt(controllerType)).fields.ContainsKey(index) Then
						Return
					End If
					Dim guiinputField As ControlMapper.GUIInputField = Me.fieldSets.[Get](CInt(controllerType)).fields.[Get](index)
					guiinputField.SetLabel(label)
					guiinputField.actionElementMapId = actionElementMapId
					guiinputField.controllerId = controllerId
					If guiinputField.hasToggle Then
						guiinputField.toggle.SetInteractible(True, False)
						guiinputField.toggle.SetToggleState(invert)
						guiinputField.toggle.actionElementMapId = actionElementMapId
					End If
				End Sub

				' Token: 0x06004BC4 RID: 19396 RVA: 0x002707C8 File Offset: 0x0026EBC8
				Public Sub SetFixedFieldData(controllerType As ControllerType, controllerId As Integer)
					If Not Me.fieldSets.ContainsKey(CInt(controllerType)) Then
						Return
					End If
					Dim fieldSet As ControlMapper.InputGridEntryList.FieldSet = Me.fieldSets.[Get](CInt(controllerType))
					Dim count As Integer = fieldSet.fields.Count
					For i As Integer = 0 To count - 1
						fieldSet.fields(i).controllerId = controllerId
					Next
				End Sub

				' Token: 0x06004BC5 RID: 19397 RVA: 0x00270824 File Offset: 0x0026EC24
				Public Sub Initialize()
					For i As Integer = 0 To Me.fieldSets.Count - 1
						Dim fieldSet As ControlMapper.InputGridEntryList.FieldSet = Me.fieldSets(i)
						Dim count As Integer = fieldSet.fields.Count
						For j As Integer = 0 To count - 1
							Dim guiinputField As ControlMapper.GUIInputField = fieldSet.fields(j)
							If guiinputField.hasToggle Then
								guiinputField.toggle.SetInteractible(False, False)
								guiinputField.toggle.SetToggleState(False)
								guiinputField.toggle.actionElementMapId = -1
							End If
							guiinputField.SetLabel(String.Empty)
							guiinputField.actionElementMapId = -1
							guiinputField.controllerId = -1
						Next
					Next
				End Sub

				' Token: 0x06004BC6 RID: 19398 RVA: 0x002708D8 File Offset: 0x0026ECD8
				Public Sub SetActive(state As Boolean)
					If Me.label IsNot Nothing Then
						Me.label.SetActive(state)
					End If
					Dim count As Integer = Me.fieldSets.Count
					For i As Integer = 0 To count - 1
						Me.fieldSets(i).groupContainer.SetActive(state)
					Next
				End Sub

				' Token: 0x06004BC7 RID: 19399 RVA: 0x00270934 File Offset: 0x0026ED34
				Public Sub ClearLabels()
					For i As Integer = 0 To Me.fieldSets.Count - 1
						Dim fieldSet As ControlMapper.InputGridEntryList.FieldSet = Me.fieldSets(i)
						Dim count As Integer = fieldSet.fields.Count
						For j As Integer = 0 To count - 1
							Dim guiinputField As ControlMapper.GUIInputField = fieldSet.fields(j)
							guiinputField.SetLabel(String.Empty)
						Next
					Next
				End Sub

				' Token: 0x06004BC8 RID: 19400 RVA: 0x002709A4 File Offset: 0x0026EDA4
				Public Sub SetFieldsActive(state As Boolean)
					For i As Integer = 0 To Me.fieldSets.Count - 1
						Dim fieldSet As ControlMapper.InputGridEntryList.FieldSet = Me.fieldSets(i)
						Dim count As Integer = fieldSet.fields.Count
						For j As Integer = 0 To count - 1
							Dim guiinputField As ControlMapper.GUIInputField = fieldSet.fields(j)
							guiinputField.SetInteractible(state, False)
							If guiinputField.hasToggle Then
								guiinputField.toggle.SetInteractible(state, False)
							End If
						Next
					Next
				End Sub

				' Token: 0x0400509B RID: 20635
				Private fieldSets As ControlMapper.IndexedDictionary(Of Integer, ControlMapper.InputGridEntryList.FieldSet)

				' Token: 0x0400509C RID: 20636
				Public label As ControlMapper.GUILabel

				' Token: 0x0400509D RID: 20637
				Public action As InputAction

				' Token: 0x0400509E RID: 20638
				Public axisRange As AxisRange

				' Token: 0x0400509F RID: 20639
				Public actionSet As ControlMapper.InputActionSet
			End Class

			' Token: 0x02000C24 RID: 3108
			Private Class FieldSet
				' Token: 0x06004BC9 RID: 19401 RVA: 0x00270A29 File Offset: 0x0026EE29
				Public Sub New(groupContainer As GameObject)
					Me.groupContainer = groupContainer
					Me.fields = New ControlMapper.IndexedDictionary(Of Integer, ControlMapper.GUIInputField)()
				End Sub

				' Token: 0x040050A0 RID: 20640
				Public groupContainer As GameObject

				' Token: 0x040050A1 RID: 20641
				Public fields As ControlMapper.IndexedDictionary(Of Integer, ControlMapper.GUIInputField)
			End Class

			' Token: 0x02000C25 RID: 3109
			Private Class ActionCategoryEntry
				' Token: 0x06004BCA RID: 19402 RVA: 0x00270A43 File Offset: 0x0026EE43
				Public Sub New(actionCategoryId As Integer)
					Me.actionCategoryId = actionCategoryId
				End Sub

				' Token: 0x06004BCB RID: 19403 RVA: 0x00270A52 File Offset: 0x0026EE52
				Public Sub SetLabel(label As ControlMapper.GUILabel)
					Me.label = label
				End Sub

				' Token: 0x06004BCC RID: 19404 RVA: 0x00270A5B File Offset: 0x0026EE5B
				Public Sub SetActive(state As Boolean)
					If Me.label IsNot Nothing Then
						Me.label.SetActive(state)
					End If
				End Sub

				' Token: 0x040050A2 RID: 20642
				Public actionCategoryId As Integer

				' Token: 0x040050A3 RID: 20643
				Public label As ControlMapper.GUILabel
			End Class
		End Class

		' Token: 0x02000C26 RID: 3110
		Private Class WindowManager
			' Token: 0x06004BCD RID: 19405 RVA: 0x00270BD8 File Offset: 0x0026EFD8
			Public Sub New(windowPrefab As GameObject, faderPrefab As GameObject, parent As Transform)
				Me.windowPrefab = windowPrefab
				Me.parent = parent
				Me.windows = New List(Of Window)()
				Me.fader = Global.UnityEngine.[Object].Instantiate(Of GameObject)(faderPrefab)
				Me.fader.transform.SetParent(parent, False)
				Me.fader.GetComponent(Of RectTransform)().localScale = Vector2.one
				Me.SetFaderActive(False)
			End Sub

			' Token: 0x1700073D RID: 1853
			' (get) Token: 0x06004BCE RID: 19406 RVA: 0x00270C44 File Offset: 0x0026F044
			Public ReadOnly Property isWindowOpen As Boolean
				Get
					For i As Integer = Me.windows.Count - 1 To 0 Step -1
						If Not(Me.windows(i) Is Nothing) Then
							Return True
						End If
					Next
					Return False
				End Get
			End Property

			' Token: 0x1700073E RID: 1854
			' (get) Token: 0x06004BCF RID: 19407 RVA: 0x00270C90 File Offset: 0x0026F090
			Public ReadOnly Property topWindow As Window
				Get
					For i As Integer = Me.windows.Count - 1 To 0 Step -1
						If Not(Me.windows(i) Is Nothing) Then
							Return Me.windows(i)
						End If
					Next
					Return Nothing
				End Get
			End Property

			' Token: 0x06004BD0 RID: 19408 RVA: 0x00270CE8 File Offset: 0x0026F0E8
			Public Function OpenWindow(name As String, width As Integer, height As Integer) As Window
				Dim window As Window = Me.InstantiateWindow(name, width, height)
				Me.UpdateFader()
				Return window
			End Function

			' Token: 0x06004BD1 RID: 19409 RVA: 0x00270D08 File Offset: 0x0026F108
			Public Function OpenWindow(windowPrefab As GameObject, name As String) As Window
				If windowPrefab Is Nothing Then
					Global.UnityEngine.Debug.LogError("Rewired Control Mapper: Window Prefab is null!")
					Return Nothing
				End If
				Dim window As Window = Me.InstantiateWindow(name, windowPrefab)
				Me.UpdateFader()
				Return window
			End Function

			' Token: 0x06004BD2 RID: 19410 RVA: 0x00270D40 File Offset: 0x0026F140
			Public Sub CloseTop()
				For i As Integer = Me.windows.Count - 1 To 0 Step -1
					If Not(Me.windows(i) Is Nothing) Then
						Me.DestroyWindow(Me.windows(i))
						Me.windows.RemoveAt(i)
						Exit For
					End If
					Me.windows.RemoveAt(i)
				Next
				Me.UpdateFader()
			End Sub

			' Token: 0x06004BD3 RID: 19411 RVA: 0x00270DBC File Offset: 0x0026F1BC
			Public Sub CloseWindow(windowId As Integer)
				Me.CloseWindow(Me.GetWindow(windowId))
			End Sub

			' Token: 0x06004BD4 RID: 19412 RVA: 0x00270DCC File Offset: 0x0026F1CC
			Public Sub CloseWindow(window As Window)
				If window Is Nothing Then
					Return
				End If
				For i As Integer = Me.windows.Count - 1 To 0 Step -1
					If Me.windows(i) Is Nothing Then
						Me.windows.RemoveAt(i)
					ElseIf Not(Me.windows(i) IsNot window) Then
						Me.DestroyWindow(Me.windows(i))
						Me.windows.RemoveAt(i)
						Exit For
					End If
				Next
				Me.UpdateFader()
				Me.FocusTopWindow()
			End Sub

			' Token: 0x06004BD5 RID: 19413 RVA: 0x00270E78 File Offset: 0x0026F278
			Public Sub CloseAll()
				Me.SetFaderActive(False)
				For i As Integer = Me.windows.Count - 1 To 0 Step -1
					If Me.windows(i) Is Nothing Then
						Me.windows.RemoveAt(i)
					Else
						Me.DestroyWindow(Me.windows(i))
						Me.windows.RemoveAt(i)
					End If
				Next
				Me.UpdateFader()
			End Sub

			' Token: 0x06004BD6 RID: 19414 RVA: 0x00270EF8 File Offset: 0x0026F2F8
			Public Sub CancelAll()
				If Not Me.isWindowOpen Then
					Return
				End If
				For i As Integer = Me.windows.Count - 1 To 0 Step -1
					If Not(Me.windows(i) Is Nothing) Then
						Me.windows(i).Cancel()
					End If
				Next
				Me.CloseAll()
			End Sub

			' Token: 0x06004BD7 RID: 19415 RVA: 0x00270F64 File Offset: 0x0026F364
			Public Function GetWindow(windowId As Integer) As Window
				If windowId < 0 Then
					Return Nothing
				End If
				For i As Integer = Me.windows.Count - 1 To 0 Step -1
					If Not(Me.windows(i) Is Nothing) Then
						If Me.windows(i).id = windowId Then
							Return Me.windows(i)
						End If
					End If
				Next
				Return Nothing
			End Function

			' Token: 0x06004BD8 RID: 19416 RVA: 0x00270FDE File Offset: 0x0026F3DE
			Public Function IsFocused(windowId As Integer) As Boolean
				Return windowId >= 0 AndAlso Not(Me.topWindow Is Nothing) AndAlso Me.topWindow.id = windowId
			End Function

			' Token: 0x06004BD9 RID: 19417 RVA: 0x0027100A File Offset: 0x0026F40A
			Public Sub Focus(windowId As Integer)
				Me.Focus(Me.GetWindow(windowId))
			End Sub

			' Token: 0x06004BDA RID: 19418 RVA: 0x00271019 File Offset: 0x0026F419
			Public Sub Focus(window As Window)
				If window Is Nothing Then
					Return
				End If
				window.TakeInputFocus()
				Me.DefocusOtherWindows(window.id)
			End Sub

			' Token: 0x06004BDB RID: 19419 RVA: 0x0027103C File Offset: 0x0026F43C
			Private Sub DefocusOtherWindows(focusedWindowId As Integer)
				If focusedWindowId < 0 Then
					Return
				End If
				For i As Integer = Me.windows.Count - 1 To 0 Step -1
					If Not(Me.windows(i) Is Nothing) Then
						If Me.windows(i).id <> focusedWindowId Then
							Me.windows(i).Disable()
						End If
					End If
				Next
			End Sub

			' Token: 0x06004BDC RID: 19420 RVA: 0x002710B8 File Offset: 0x0026F4B8
			Private Sub UpdateFader()
				If Not Me.isWindowOpen Then
					Me.SetFaderActive(False)
					Return
				End If
				Dim transform As Transform = Me.topWindow.transform.parent
				If transform Is Nothing Then
					Return
				End If
				Me.SetFaderActive(True)
				Me.fader.transform.SetAsLastSibling()
				Dim siblingIndex As Integer = Me.topWindow.transform.GetSiblingIndex()
				Me.fader.transform.SetSiblingIndex(siblingIndex)
			End Sub

			' Token: 0x06004BDD RID: 19421 RVA: 0x0027112F File Offset: 0x0026F52F
			Private Sub FocusTopWindow()
				If Me.topWindow Is Nothing Then
					Return
				End If
				Me.topWindow.TakeInputFocus()
			End Sub

			' Token: 0x06004BDE RID: 19422 RVA: 0x0027114E File Offset: 0x0026F54E
			Private Sub SetFaderActive(state As Boolean)
				Me.fader.SetActive(state)
			End Sub

			' Token: 0x06004BDF RID: 19423 RVA: 0x0027115C File Offset: 0x0026F55C
			Private Function InstantiateWindow(name As String, width As Integer, height As Integer) As Window
				If String.IsNullOrEmpty(name) Then
					name = "Window"
				End If
				Dim gameObject As GameObject = UITools.InstantiateGUIObject(Of Window)(Me.windowPrefab, Me.parent, name)
				If gameObject Is Nothing Then
					Return Nothing
				End If
				Dim component As Window = gameObject.GetComponent(Of Window)()
				If component IsNot Nothing Then
					component.Initialize(Me.GetNewId(), AddressOf Me.IsFocused)
					Me.windows.Add(component)
					component.SetSize(width, height)
				End If
				Return component
			End Function

			' Token: 0x06004BE0 RID: 19424 RVA: 0x002711DC File Offset: 0x0026F5DC
			Private Function InstantiateWindow(name As String, windowPrefab As GameObject) As Window
				If String.IsNullOrEmpty(name) Then
					name = "Window"
				End If
				If windowPrefab Is Nothing Then
					Return Nothing
				End If
				Dim gameObject As GameObject = UITools.InstantiateGUIObject(Of Window)(windowPrefab, Me.parent, name)
				If gameObject Is Nothing Then
					Return Nothing
				End If
				Dim component As Window = gameObject.GetComponent(Of Window)()
				If component IsNot Nothing Then
					component.Initialize(Me.GetNewId(), AddressOf Me.IsFocused)
					Me.windows.Add(component)
				End If
				Return component
			End Function

			' Token: 0x06004BE1 RID: 19425 RVA: 0x0027125D File Offset: 0x0026F65D
			Private Sub DestroyWindow(window As Window)
				If window Is Nothing Then
					Return
				End If
				Global.UnityEngine.[Object].Destroy(window.gameObject)
			End Sub

			' Token: 0x06004BE2 RID: 19426 RVA: 0x00271278 File Offset: 0x0026F678
			Private Function GetNewId() As Integer
				Dim num As Integer = Me.idCounter
				Me.idCounter += 1
				Return num
			End Function

			' Token: 0x06004BE3 RID: 19427 RVA: 0x0027129B File Offset: 0x0026F69B
			Public Sub ClearCompletely()
				Me.CloseAll()
				If Me.fader IsNot Nothing Then
					Global.UnityEngine.[Object].Destroy(Me.fader)
				End If
			End Sub

			' Token: 0x040050A4 RID: 20644
			Private windows As List(Of Window)

			' Token: 0x040050A5 RID: 20645
			Private windowPrefab As GameObject

			' Token: 0x040050A6 RID: 20646
			Private parent As Transform

			' Token: 0x040050A7 RID: 20647
			Private fader As GameObject

			' Token: 0x040050A8 RID: 20648
			Private idCounter As Integer
		End Class
	End Class
End Namespace
