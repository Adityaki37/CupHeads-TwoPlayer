Imports System
Imports UnityEngine

' Token: 0x020006B3 RID: 1715
Public Class FrogsLevelMorphedSwitch
	Inherits ParrySwitch

	' Token: 0x170003B1 RID: 945
	' (get) Token: 0x06002465 RID: 9317 RVA: 0x00155881 File Offset: 0x00153C81
	' (set) Token: 0x06002466 RID: 9318 RVA: 0x0015588E File Offset: 0x00153C8E
	Public Property enabled As Boolean
		Get
			Return MyBase.GetComponent(Of Collider2D)().enabled
		End Get
		Set(value As Boolean)
			MyBase.GetComponent(Of Collider2D)().enabled = value
		End Set
	End Property

	' Token: 0x06002467 RID: 9319 RVA: 0x0015589C File Offset: 0x00153C9C
	Public Shared Function Create(parent As FrogsLevelMorphed) As FrogsLevelMorphedSwitch
		Dim gameObject As GameObject = New GameObject("Frogs_Morphed_Handle")
		Dim frogsLevelMorphedSwitch As FrogsLevelMorphedSwitch = gameObject.AddComponent(Of FrogsLevelMorphedSwitch)()
		frogsLevelMorphedSwitch.target = parent.switchRoot
		Return frogsLevelMorphedSwitch
	End Function

	' Token: 0x06002468 RID: 9320 RVA: 0x001558C8 File Offset: 0x00153CC8
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Dim circleCollider2D As CircleCollider2D = MyBase.gameObject.AddComponent(Of CircleCollider2D)()
		circleCollider2D.radius = 50F
		circleCollider2D.isTrigger = True
	End Sub

	' Token: 0x06002469 RID: 9321 RVA: 0x001558F9 File Offset: 0x00153CF9
	Private Sub Update()
		Me.UpdateLocation()
	End Sub

	' Token: 0x0600246A RID: 9322 RVA: 0x00155901 File Offset: 0x00153D01
	Private Sub LateUpdate()
		Me.UpdateLocation()
	End Sub

	' Token: 0x0600246B RID: 9323 RVA: 0x00155909 File Offset: 0x00153D09
	Private Sub UpdateLocation()
		If Me.target IsNot Nothing Then
			MyBase.transform.position = Me.target.position
		End If
	End Sub

	' Token: 0x04002D1F RID: 11551
	Private target As Transform
End Class
