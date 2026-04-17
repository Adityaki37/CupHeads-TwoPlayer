Imports System
Imports System.Collections.Generic
Imports UnityEngine

' Token: 0x02000A19 RID: 2585
Public Class LevelPlayerColliderManager
	Inherits AbstractLevelPlayerComponent

	' Token: 0x17000529 RID: 1321
	' (get) Token: 0x06003D46 RID: 15686 RVA: 0x0021E4D1 File Offset: 0x0021C8D1
	Public ReadOnly Property [default] As LevelPlayerColliderManager.ColliderProperties
		Get
			Return Me.colliders.[default]
		End Get
	End Property

	' Token: 0x1700052A RID: 1322
	' (get) Token: 0x06003D47 RID: 15687 RVA: 0x0021E4DE File Offset: 0x0021C8DE
	Public ReadOnly Property DefaultWidth As Single
		Get
			Return Me.colliders.[default].size.x
		End Get
	End Property

	' Token: 0x1700052B RID: 1323
	' (get) Token: 0x06003D48 RID: 15688 RVA: 0x0021E4F5 File Offset: 0x0021C8F5
	Public ReadOnly Property DefaultHeight As Single
		Get
			Return Me.colliders.[default].size.y
		End Get
	End Property

	' Token: 0x1700052C RID: 1324
	' (get) Token: 0x06003D49 RID: 15689 RVA: 0x0021E50C File Offset: 0x0021C90C
	Public ReadOnly Property Width As Single
		Get
			Return Me.pairs(CInt(Me._state)).size.x
		End Get
	End Property

	' Token: 0x1700052D RID: 1325
	' (get) Token: 0x06003D4A RID: 15690 RVA: 0x0021E529 File Offset: 0x0021C929
	Public ReadOnly Property Height As Single
		Get
			Return Me.pairs(CInt(Me._state)).size.y
		End Get
	End Property

	' Token: 0x1700052E RID: 1326
	' (get) Token: 0x06003D4B RID: 15691 RVA: 0x0021E548 File Offset: 0x0021C948
	Public ReadOnly Property Center As Vector2
		Get
			Return New Vector2(Me.boxCollider.offset.x, Me.boxCollider.offset.y * MyBase.player.motor.GravityReversalMultiplier) + MyBase.transform.position
		End Get
	End Property

	' Token: 0x1700052F RID: 1327
	' (get) Token: 0x06003D4C RID: 15692 RVA: 0x0021E5A8 File Offset: 0x0021C9A8
	Public ReadOnly Property DefaultCenter As Vector2
		Get
			Return New Vector2(Me.colliders.[default].center.x, Me.colliders.[default].center.y * MyBase.player.motor.GravityReversalMultiplier) + MyBase.transform.position
		End Get
	End Property

	' Token: 0x17000530 RID: 1328
	' (get) Token: 0x06003D4D RID: 15693 RVA: 0x0021E60A File Offset: 0x0021CA0A
	' (set) Token: 0x06003D4E RID: 15694 RVA: 0x0021E612 File Offset: 0x0021CA12
	Public Property state As LevelPlayerColliderManager.State
		Get
			Return Me._state
		End Get
		Set(value As LevelPlayerColliderManager.State)
			If Me._state <> value Then
				Me.pairs(CInt(value)).SetCollider(Me.boxCollider)
			End If
			Me._state = value
		End Set
	End Property

	' Token: 0x06003D4F RID: 15695 RVA: 0x0021E640 File Offset: 0x0021CA40
	Protected Overrides Sub OnAwake()
		MyBase.OnAwake()
		Me.boxCollider = MyBase.GetComponent(Of BoxCollider2D)()
		Me.pairs = New Dictionary(Of Integer, LevelPlayerColliderManager.ColliderProperties)()
		Me.pairs(0) = Me.colliders.[default]
		Me.pairs(1) = Me.colliders.air
		Me.pairs(2) = Me.colliders.ducking
		Me.pairs(3) = Me.colliders.ducking
		Me.pairs(4) = Me.colliders.chaliceFirstJump
		Me.state = LevelPlayerColliderManager.State.[Default]
	End Sub

	' Token: 0x06003D50 RID: 15696 RVA: 0x0021E6E4 File Offset: 0x0021CAE4
	Private Sub FixedUpdate()
		Me.UpdateColliders()
	End Sub

	' Token: 0x06003D51 RID: 15697 RVA: 0x0021E6EC File Offset: 0x0021CAEC
	Private Sub UpdateColliders()
		MyBase.gameObject.layer = If((Not MyBase.player.CanTakeDamage), 9, 8)
		If MyBase.player.motor.Dashing Then
			If Me.state <> LevelPlayerColliderManager.State.Dashing Then
				Me.state = LevelPlayerColliderManager.State.Dashing
			End If
			Return
		End If
		If Not MyBase.player.motor.Grounded Then
			If Not MyBase.player.stats.isChalice Then
				If Me.state <> LevelPlayerColliderManager.State.Air Then
					Me.state = LevelPlayerColliderManager.State.Air
				End If
				Return
			End If
			If Not MyBase.player.motor.ChaliceDoubleJumped Then
				If Me.state <> LevelPlayerColliderManager.State.ChaliceFirstJump Then
					Me.state = LevelPlayerColliderManager.State.ChaliceFirstJump
				End If
				Return
			End If
			If Me.state <> LevelPlayerColliderManager.State.Air Then
				Me.state = LevelPlayerColliderManager.State.Air
			End If
			Return
		Else
			If MyBase.player.motor.Ducking Then
				If Me.state <> LevelPlayerColliderManager.State.Ducking Then
					Me.state = LevelPlayerColliderManager.State.Ducking
				End If
				Return
			End If
			If Me.state <> LevelPlayerColliderManager.State.[Default] Then
				Me.state = LevelPlayerColliderManager.State.[Default]
			End If
			Return
		End If
	End Sub

	' Token: 0x04004494 RID: 17556
	<SerializeField()>
	Private colliders As LevelPlayerColliderManager.ColliderPropertiesGroup

	' Token: 0x04004495 RID: 17557
	Private pairs As Dictionary(Of Integer, LevelPlayerColliderManager.ColliderProperties)

	' Token: 0x04004496 RID: 17558
	Private boxCollider As BoxCollider2D

	' Token: 0x04004497 RID: 17559
	Private _state As LevelPlayerColliderManager.State

	' Token: 0x02000A1A RID: 2586
	Public Enum State
		' Token: 0x04004499 RID: 17561
		[Default]
		' Token: 0x0400449A RID: 17562
		Air
		' Token: 0x0400449B RID: 17563
		Ducking
		' Token: 0x0400449C RID: 17564
		Dashing
		' Token: 0x0400449D RID: 17565
		ChaliceFirstJump
	End Enum

	' Token: 0x02000A1B RID: 2587
	<Serializable()>
	Public Class ColliderProperties
		' Token: 0x06003D52 RID: 15698 RVA: 0x0021E7FB File Offset: 0x0021CBFB
		Public Sub New(center As Vector2, size As Vector2)
			Me.center = center
			Me.size = size
		End Sub

		' Token: 0x06003D53 RID: 15699 RVA: 0x0021E814 File Offset: 0x0021CC14
		Public Function CreateCollider(gameObject As GameObject) As BoxCollider2D
			Dim boxCollider2D As BoxCollider2D = gameObject.AddComponent(Of BoxCollider2D)()
			boxCollider2D.offset = Me.center
			boxCollider2D.size = Me.size
			boxCollider2D.isTrigger = True
			Return boxCollider2D
		End Function

		' Token: 0x06003D54 RID: 15700 RVA: 0x0021E848 File Offset: 0x0021CC48
		Public Sub SetCollider(boxCollider As BoxCollider2D)
			boxCollider.offset = Me.center
			boxCollider.size = Me.size
			boxCollider.isTrigger = True
		End Sub

		' Token: 0x0400449E RID: 17566
		Public center As Vector2

		' Token: 0x0400449F RID: 17567
		Public size As Vector2
	End Class

	' Token: 0x02000A1C RID: 2588
	<Serializable()>
	Public Class ColliderPropertiesGroup
		' Token: 0x040044A0 RID: 17568
		Public [default] As LevelPlayerColliderManager.ColliderProperties = New LevelPlayerColliderManager.ColliderProperties(New Vector2(0F, 62F), New Vector2(50F, 105F))

		' Token: 0x040044A1 RID: 17569
		Public air As LevelPlayerColliderManager.ColliderProperties = New LevelPlayerColliderManager.ColliderProperties(New Vector2(0F, 50F), New Vector2(50F, 50F))

		' Token: 0x040044A2 RID: 17570
		Public ducking As LevelPlayerColliderManager.ColliderProperties = New LevelPlayerColliderManager.ColliderProperties(New Vector2(0F, 27F), New Vector2(50F, 35F))

		' Token: 0x040044A3 RID: 17571
		Public dashing As LevelPlayerColliderManager.ColliderProperties

		' Token: 0x040044A4 RID: 17572
		Public chaliceFirstJump As LevelPlayerColliderManager.ColliderProperties = New LevelPlayerColliderManager.ColliderProperties(New Vector2(0F, 78F), New Vector2(50F, 75F))
	End Class
End Class
