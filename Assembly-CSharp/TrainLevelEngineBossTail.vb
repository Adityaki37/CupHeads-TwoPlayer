Imports System
Imports UnityEngine

' Token: 0x02000816 RID: 2070
Public Class TrainLevelEngineBossTail
	Inherits ParrySwitch

	' Token: 0x1700041D RID: 1053
	' (get) Token: 0x06002FFE RID: 12286 RVA: 0x001C61C5 File Offset: 0x001C45C5
	' (set) Token: 0x06002FFF RID: 12287 RVA: 0x001C61E9 File Offset: 0x001C45E9
	Public Property tailEnabled As Boolean
		Get
			Return Not(Me.circleCollider Is Nothing) AndAlso Me.circleCollider.enabled
		End Get
		Set(value As Boolean)
			If Me.circleCollider IsNot Nothing Then
				Me.circleCollider.enabled = value
			End If
		End Set
	End Property

	' Token: 0x06003000 RID: 12288 RVA: 0x001C6208 File Offset: 0x001C4608
	Public Shared Function Create(target As Transform) As TrainLevelEngineBossTail
		Dim gameObject As GameObject = New GameObject("Engine_Boss_Tail")
		Dim trainLevelEngineBossTail As TrainLevelEngineBossTail = gameObject.AddComponent(Of TrainLevelEngineBossTail)()
		trainLevelEngineBossTail.target = target
		trainLevelEngineBossTail.tag = "ParrySwitch"
		Return trainLevelEngineBossTail
	End Function

	' Token: 0x06003001 RID: 12289 RVA: 0x001C623A File Offset: 0x001C463A
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.circleCollider = MyBase.gameObject.AddComponent(Of CircleCollider2D)()
		Me.circleCollider.radius = 40F
		Me.circleCollider.isTrigger = True
	End Sub

	' Token: 0x06003002 RID: 12290 RVA: 0x001C626F File Offset: 0x001C466F
	Private Sub Update()
		Me.UpdateLocation()
	End Sub

	' Token: 0x06003003 RID: 12291 RVA: 0x001C6277 File Offset: 0x001C4677
	Private Sub LateUpdate()
		Me.UpdateLocation()
	End Sub

	' Token: 0x06003004 RID: 12292 RVA: 0x001C627F File Offset: 0x001C467F
	Private Sub UpdateLocation()
		If Me.target IsNot Nothing Then
			MyBase.transform.position = Me.target.position
		End If
	End Sub

	' Token: 0x040038DB RID: 14555
	Private circleCollider As CircleCollider2D

	' Token: 0x040038DC RID: 14556
	Private target As Transform
End Class
