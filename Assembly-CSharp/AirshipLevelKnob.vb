Imports System
Imports UnityEngine

' Token: 0x020004CA RID: 1226
Public Class AirshipLevelKnob
	Inherits ParrySwitch

	' Token: 0x17000315 RID: 789
	' (get) Token: 0x060014C3 RID: 5315 RVA: 0x000BA3DB File Offset: 0x000B87DB
	' (set) Token: 0x060014C4 RID: 5316 RVA: 0x000BA3E8 File Offset: 0x000B87E8
	Public Property enabled As Boolean
		Get
			Return MyBase.GetComponent(Of Collider2D)().enabled
		End Get
		Set(value As Boolean)
			MyBase.GetComponent(Of Collider2D)().enabled = value
		End Set
	End Property

	' Token: 0x060014C5 RID: 5317 RVA: 0x000BA3F8 File Offset: 0x000B87F8
	Public Shared Function Create(root As Transform) As AirshipLevelKnob
		Dim gameObject As GameObject = New GameObject("Airship_Knob")
		Dim airshipLevelKnob As AirshipLevelKnob = gameObject.AddComponent(Of AirshipLevelKnob)()
		airshipLevelKnob.target = root
		airshipLevelKnob.tag = "ParrySwitch"
		Return airshipLevelKnob
	End Function

	' Token: 0x060014C6 RID: 5318 RVA: 0x000BA42C File Offset: 0x000B882C
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Dim circleCollider2D As CircleCollider2D = MyBase.gameObject.AddComponent(Of CircleCollider2D)()
		circleCollider2D.radius = 20F
		circleCollider2D.isTrigger = True
	End Sub

	' Token: 0x060014C7 RID: 5319 RVA: 0x000BA45D File Offset: 0x000B885D
	Private Sub Update()
		Me.UpdateLocation()
	End Sub

	' Token: 0x060014C8 RID: 5320 RVA: 0x000BA465 File Offset: 0x000B8865
	Private Sub LateUpdate()
		Me.UpdateLocation()
	End Sub

	' Token: 0x060014C9 RID: 5321 RVA: 0x000BA46D File Offset: 0x000B886D
	Private Sub UpdateLocation()
		If Me.target IsNot Nothing Then
			MyBase.transform.position = Me.target.position
		End If
	End Sub

	' Token: 0x04001E30 RID: 7728
	Private target As Transform
End Class
