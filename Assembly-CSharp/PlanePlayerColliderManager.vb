Imports System
Imports System.Collections.Generic
Imports UnityEngine

' Token: 0x02000A95 RID: 2709
Public Class PlanePlayerColliderManager
	Inherits AbstractPlanePlayerComponent

	' Token: 0x170005A4 RID: 1444
	' (get) Token: 0x060040EC RID: 16620 RVA: 0x0023523C File Offset: 0x0023363C
	' (set) Token: 0x060040ED RID: 16621 RVA: 0x00235244 File Offset: 0x00233644
	Public Property state As PlanePlayerColliderManager.State
		Get
			Return Me._state
		End Get
		Set(value As PlanePlayerColliderManager.State)
			If Me._state <> value Then
				Me.pairs(value).SetCollider(Me.boxCollider)
			End If
			Me._state = value
		End Set
	End Property

	' Token: 0x170005A5 RID: 1445
	' (get) Token: 0x060040EE RID: 16622 RVA: 0x00235270 File Offset: 0x00233670
	Public ReadOnly Property [default] As PlanePlayerColliderManager.ColliderProperties
		Get
			Return Me.colliders.[default]
		End Get
	End Property

	' Token: 0x060040EF RID: 16623 RVA: 0x00235280 File Offset: 0x00233680
	Protected Overrides Sub OnAwake()
		MyBase.OnAwake()
		Me.boxCollider = MyBase.GetComponent(Of BoxCollider2D)()
		Me.pairs = New Dictionary(Of PlanePlayerColliderManager.State, PlanePlayerColliderManager.ColliderProperties)()
		Me.pairs(PlanePlayerColliderManager.State.[Default]) = Me.colliders.[default]
		Me.pairs(PlanePlayerColliderManager.State.Shrunk) = Me.colliders.shrunk
		Me.state = PlanePlayerColliderManager.State.[Default]
	End Sub

	' Token: 0x060040F0 RID: 16624 RVA: 0x002352DF File Offset: 0x002336DF
	Private Sub Update()
		Me.UpdateColliders()
	End Sub

	' Token: 0x060040F1 RID: 16625 RVA: 0x002352E8 File Offset: 0x002336E8
	Private Sub UpdateColliders()
		Me.boxCollider.enabled = MyBase.player.CanTakeDamage
		If MyBase.player.Shrunk Then
			If Me.state <> PlanePlayerColliderManager.State.Shrunk Then
				Me.state = PlanePlayerColliderManager.State.Shrunk
			End If
			Return
		End If
		If Me.state <> PlanePlayerColliderManager.State.[Default] Then
			Me.state = PlanePlayerColliderManager.State.[Default]
		End If
	End Sub

	' Token: 0x04004793 RID: 18323
	<SerializeField()>
	Private colliders As PlanePlayerColliderManager.ColliderPropertiesGroup

	' Token: 0x04004794 RID: 18324
	Private pairs As Dictionary(Of PlanePlayerColliderManager.State, PlanePlayerColliderManager.ColliderProperties)

	' Token: 0x04004795 RID: 18325
	Private boxCollider As BoxCollider2D

	' Token: 0x04004796 RID: 18326
	Private _state As PlanePlayerColliderManager.State

	' Token: 0x02000A96 RID: 2710
	Public Enum State
		' Token: 0x04004798 RID: 18328
		[Default]
		' Token: 0x04004799 RID: 18329
		Shrunk
	End Enum

	' Token: 0x02000A97 RID: 2711
	<Serializable()>
	Public Class ColliderProperties
		' Token: 0x060040F2 RID: 16626 RVA: 0x00235341 File Offset: 0x00233741
		Public Sub New(center As Vector2, size As Vector2)
			Me.center = center
			Me.size = size
		End Sub

		' Token: 0x060040F3 RID: 16627 RVA: 0x00235358 File Offset: 0x00233758
		Public Function CreateCollider(gameObject As GameObject) As BoxCollider2D
			Dim boxCollider2D As BoxCollider2D = gameObject.AddComponent(Of BoxCollider2D)()
			boxCollider2D.offset = Me.center
			boxCollider2D.size = Me.size
			boxCollider2D.isTrigger = True
			Return boxCollider2D
		End Function

		' Token: 0x060040F4 RID: 16628 RVA: 0x0023538C File Offset: 0x0023378C
		Public Sub SetCollider(boxCollider As BoxCollider2D)
			boxCollider.offset = Me.center
			boxCollider.size = Me.size
			boxCollider.isTrigger = True
		End Sub

		' Token: 0x0400479A RID: 18330
		Public center As Vector2

		' Token: 0x0400479B RID: 18331
		Public size As Vector2
	End Class

	' Token: 0x02000A98 RID: 2712
	<Serializable()>
	Public Class ColliderPropertiesGroup
		' Token: 0x0400479C RID: 18332
		Public [default] As PlanePlayerColliderManager.ColliderProperties = New PlanePlayerColliderManager.ColliderProperties(New Vector2(-10F, 20F), New Vector2(75F, 75F))

		' Token: 0x0400479D RID: 18333
		Public shrunk As PlanePlayerColliderManager.ColliderProperties = New PlanePlayerColliderManager.ColliderProperties(New Vector2(-10F, 20F), New Vector2(45F, 45F))
	End Class
End Class
