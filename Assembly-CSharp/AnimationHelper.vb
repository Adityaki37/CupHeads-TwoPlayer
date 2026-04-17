Imports System
Imports UnityEngine

' Token: 0x020003A0 RID: 928
<RequireComponent(GetType(Animator))>
<DisallowMultipleComponent()>
Public Class AnimationHelper
	Inherits AbstractMonoBehaviour

	' Token: 0x17000203 RID: 515
	' (get) Token: 0x06000B4D RID: 2893 RVA: 0x00082CE3 File Offset: 0x000810E3
	' (set) Token: 0x06000B4E RID: 2894 RVA: 0x00082CEB File Offset: 0x000810EB
	Public Property Layer As CupheadTime.Layer
		Get
			Return Me.layer
		End Get
		Set(value As CupheadTime.Layer)
			Me.layer = value
			Me.[Set]()
		End Set
	End Property

	' Token: 0x17000204 RID: 516
	' (get) Token: 0x06000B4F RID: 2895 RVA: 0x00082CFA File Offset: 0x000810FA
	' (set) Token: 0x06000B50 RID: 2896 RVA: 0x00082D07 File Offset: 0x00081107
	Public Property LayerSpeed As Single
		Get
			Return CupheadTime.GetLayerSpeed(Me.Layer)
		End Get
		Set(value As Single)
			CupheadTime.SetLayerSpeed(Me.Layer, value)
			Me.[Set]()
		End Set
	End Property

	' Token: 0x17000205 RID: 517
	' (get) Token: 0x06000B51 RID: 2897 RVA: 0x00082D1B File Offset: 0x0008111B
	' (set) Token: 0x06000B52 RID: 2898 RVA: 0x00082D23 File Offset: 0x00081123
	Public Property Speed As Single
		Get
			Return Me.speed
		End Get
		Set(value As Single)
			Me.speed = value
			Me.[Set]()
		End Set
	End Property

	' Token: 0x17000206 RID: 518
	' (get) Token: 0x06000B53 RID: 2899 RVA: 0x00082D32 File Offset: 0x00081132
	' (set) Token: 0x06000B54 RID: 2900 RVA: 0x00082D3A File Offset: 0x0008113A
	Public Property IgnoreGlobal As Boolean
		Get
			Return Me.ignoreGlobal
		End Get
		Set(value As Boolean)
			Me.ignoreGlobal = value
			Me.[Set]()
		End Set
	End Property

	' Token: 0x06000B55 RID: 2901 RVA: 0x00082D4C File Offset: 0x0008114C
	Protected Overrides Sub Awake()
		MyBase.Awake()
		If MyBase.animator Is Nothing Then
			Global.Debug.LogError("AnimationHelper needs Animator component", Nothing)
			Global.UnityEngine.[Object].Destroy(Me)
			Return
		End If
		CupheadTime.OnChangedEvent.Add(AddressOf Me.[Set])
		Me.[Set]()
	End Sub

	' Token: 0x06000B56 RID: 2902 RVA: 0x00082D9E File Offset: 0x0008119E
	Private Sub Update()
		If Me.autoUpdate Then
			Me.[Set]()
		End If
	End Sub

	' Token: 0x06000B57 RID: 2903 RVA: 0x00082DB1 File Offset: 0x000811B1
	Private Sub OnDestroy()
		CupheadTime.OnChangedEvent.Remove(AddressOf Me.[Set])
	End Sub

	' Token: 0x06000B58 RID: 2904 RVA: 0x00082DCC File Offset: 0x000811CC
	Protected Sub [Set]()
		If Me.IgnoreGlobal Then
			MyBase.animator.speed = Me.Speed * Me.LayerSpeed
		Else
			MyBase.animator.speed = Me.Speed * Me.LayerSpeed * CupheadTime.GlobalSpeed
		End If
	End Sub

	' Token: 0x040014FC RID: 5372
	<SerializeField()>
	Private layer As CupheadTime.Layer

	' Token: 0x040014FD RID: 5373
	<SerializeField()>
	Private speed As Single = 1F

	' Token: 0x040014FE RID: 5374
	<SerializeField()>
	Private ignoreGlobal As Boolean

	' Token: 0x040014FF RID: 5375
	<SerializeField()>
	Private autoUpdate As Boolean
End Class
