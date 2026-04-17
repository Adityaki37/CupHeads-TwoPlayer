Imports System
Imports Rewired
Imports UnityEngine

' Token: 0x02000AC9 RID: 2761
Public Class PlayerInput
	Inherits AbstractMonoBehaviour

	' Token: 0x170005CF RID: 1487
	' (get) Token: 0x06004242 RID: 16962 RVA: 0x0023C36F File Offset: 0x0023A76F
	' (set) Token: 0x06004243 RID: 16963 RVA: 0x0023C377 File Offset: 0x0023A777
	Public Property playerId As PlayerId

	' Token: 0x170005D0 RID: 1488
	' (get) Token: 0x06004244 RID: 16964 RVA: 0x0023C380 File Offset: 0x0023A780
	Public ReadOnly Property IsDead As Boolean
		Get
			Return Me.player IsNot Nothing AndAlso Me.player.IsDead
		End Get
	End Property

	' Token: 0x170005D1 RID: 1489
	' (get) Token: 0x06004245 RID: 16965 RVA: 0x0023C3A0 File Offset: 0x0023A7A0
	' (set) Token: 0x06004246 RID: 16966 RVA: 0x0023C3A8 File Offset: 0x0023A7A8
	Public Property actions As Player

	' Token: 0x06004247 RID: 16967 RVA: 0x0023C3B1 File Offset: 0x0023A7B1
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.player = MyBase.GetComponent(Of AbstractPlayerController)()
	End Sub

	' Token: 0x06004248 RID: 16968 RVA: 0x0023C3C5 File Offset: 0x0023A7C5
	Private Sub Start()
		If Level.Current IsNot Nothing AndAlso Level.Current.CameraRotates Then
			Me.canRotateInput = True
			Me.cameraTransform = Camera.main.transform
		End If
	End Sub

	' Token: 0x06004249 RID: 16969 RVA: 0x0023C3FD File Offset: 0x0023A7FD
	Public Sub Init(playerId As PlayerId)
		Me.playerId = playerId
		Me.actions = PlayerManager.GetPlayerInput(playerId)
	End Sub

	' Token: 0x0600424A RID: 16970 RVA: 0x0023C412 File Offset: 0x0023A812
	Public Overrides Sub StopAllCoroutines()
	End Sub

	' Token: 0x0600424B RID: 16971 RVA: 0x0023C414 File Offset: 0x0023A814
	Public Function GetAxisInt(axis As PlayerInput.Axis, Optional crampedDiagonal As Boolean = False, Optional duckMod As Boolean = False) As Integer
		Dim vector As Vector2 = New Vector2(Me.actions.GetAxis(0), Me.actions.GetAxis(1))
		If Me.canRotateInput Then
			If SettingsData.Data.rotateControlsWithCamera Then
				vector = Me.cameraTransform.rotation * vector
			ElseIf Mathf.Abs(Me.cameraTransform.rotation.eulerAngles.z - 180F) <= 1F Then
				vector = Me.cameraTransform.rotation * vector
			End If
		End If
		Dim magnitude As Single = vector.magnitude
		Dim num As Single = If((Not crampedDiagonal), 0.38268F, 0.5F)
		If magnitude < 0.375F Then
			Return 0
		End If
		Dim num2 As Single = If((axis <> PlayerInput.Axis.X), vector.y, vector.x) / magnitude
		If num2 > num Then
			Return 1
		End If
		If num2 < If((Not duckMod), (-num), (-0.705F)) Then
			Return -1
		End If
		Return 0
	End Function

	' Token: 0x0600424C RID: 16972 RVA: 0x0023C538 File Offset: 0x0023A938
	Public Function GetAxis(axis As PlayerInput.Axis) As Single
		If axis = PlayerInput.Axis.X Then
			Return Me.actions.GetAxis(0)
		End If
		Return Me.actions.GetAxis(1)
	End Function

	' Token: 0x0600424D RID: 16973 RVA: 0x0023C559 File Offset: 0x0023A959
	Public Function GetButton(button As CupheadButton) As Boolean
		Return Me.actions.GetButton(CInt(button))
	End Function

	' Token: 0x040048B7 RID: 18615
	Private player As AbstractPlayerController

	' Token: 0x040048BA RID: 18618
	Private canRotateInput As Boolean

	' Token: 0x040048BB RID: 18619
	Private cameraTransform As Transform

	' Token: 0x02000ACA RID: 2762
	Public Enum Axis
		' Token: 0x040048BD RID: 18621
		X
		' Token: 0x040048BE RID: 18622
		Y
	End Enum
End Class
