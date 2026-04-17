Imports System
Imports UnityEngine

' Token: 0x020004D7 RID: 1239
Public Class AirshipJellyLevelKnob
	Inherits ParrySwitch

	' Token: 0x17000318 RID: 792
	' (get) Token: 0x06001527 RID: 5415 RVA: 0x000BDEE7 File Offset: 0x000BC2E7
	' (set) Token: 0x06001528 RID: 5416 RVA: 0x000BDEF4 File Offset: 0x000BC2F4
	Public Property enabled As Boolean
		Get
			Return MyBase.GetComponent(Of Collider2D)().enabled
		End Get
		Set(value As Boolean)
			MyBase.GetComponent(Of Collider2D)().enabled = value
		End Set
	End Property

	' Token: 0x06001529 RID: 5417 RVA: 0x000BDF04 File Offset: 0x000BC304
	Public Shared Function Create(jelly As AirshipJellyLevelJelly) As AirshipJellyLevelKnob
		Dim gameObject As GameObject = New GameObject("Airship_Jelly_Knob")
		Dim airshipJellyLevelKnob As AirshipJellyLevelKnob = gameObject.AddComponent(Of AirshipJellyLevelKnob)()
		airshipJellyLevelKnob.target = jelly.knobRoot
		airshipJellyLevelKnob.tag = "ParrySwitch"
		Return airshipJellyLevelKnob
	End Function

	' Token: 0x0600152A RID: 5418 RVA: 0x000BDF3C File Offset: 0x000BC33C
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Dim circleCollider2D As CircleCollider2D = MyBase.gameObject.AddComponent(Of CircleCollider2D)()
		circleCollider2D.radius = 20F
		circleCollider2D.isTrigger = True
	End Sub

	' Token: 0x0600152B RID: 5419 RVA: 0x000BDF6D File Offset: 0x000BC36D
	Private Sub Update()
		Me.UpdateLocation()
	End Sub

	' Token: 0x0600152C RID: 5420 RVA: 0x000BDF75 File Offset: 0x000BC375
	Private Sub LateUpdate()
		Me.UpdateLocation()
	End Sub

	' Token: 0x0600152D RID: 5421 RVA: 0x000BDF7D File Offset: 0x000BC37D
	Private Sub UpdateLocation()
		If Me.target IsNot Nothing Then
			MyBase.transform.position = Me.target.position
		End If
	End Sub

	' Token: 0x04001E8C RID: 7820
	Private target As Transform
End Class
