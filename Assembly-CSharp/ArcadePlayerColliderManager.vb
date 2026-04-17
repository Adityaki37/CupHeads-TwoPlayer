Imports System
Imports System.Collections.Generic
Imports UnityEngine

' Token: 0x020009D9 RID: 2521
Public Class ArcadePlayerColliderManager
	Inherits AbstractArcadePlayerComponent

	' Token: 0x170004EB RID: 1259
	' (get) Token: 0x06003B72 RID: 15218 RVA: 0x00215009 File Offset: 0x00213409
	Public ReadOnly Property [default] As ArcadePlayerColliderManager.ColliderProperties
		Get
			Return Me.colliders.[default]
		End Get
	End Property

	' Token: 0x170004EC RID: 1260
	' (get) Token: 0x06003B73 RID: 15219 RVA: 0x00215016 File Offset: 0x00213416
	Public ReadOnly Property DefaultWidth As Single
		Get
			Return Me.colliders.[default].size.x
		End Get
	End Property

	' Token: 0x170004ED RID: 1261
	' (get) Token: 0x06003B74 RID: 15220 RVA: 0x0021502D File Offset: 0x0021342D
	Public ReadOnly Property DefaultHeight As Single
		Get
			Return Me.colliders.[default].size.y
		End Get
	End Property

	' Token: 0x170004EE RID: 1262
	' (get) Token: 0x06003B75 RID: 15221 RVA: 0x00215044 File Offset: 0x00213444
	Public ReadOnly Property Width As Single
		Get
			Return Me.pairs(Me._state).size.x
		End Get
	End Property

	' Token: 0x170004EF RID: 1263
	' (get) Token: 0x06003B76 RID: 15222 RVA: 0x00215061 File Offset: 0x00213461
	Public ReadOnly Property Height As Single
		Get
			Return Me.pairs(Me._state).size.y
		End Get
	End Property

	' Token: 0x170004F0 RID: 1264
	' (get) Token: 0x06003B77 RID: 15223 RVA: 0x0021507E File Offset: 0x0021347E
	Public ReadOnly Property Center As Vector2
		Get
			Return Me.boxCollider.offset + MyBase.transform.position
		End Get
	End Property

	' Token: 0x170004F1 RID: 1265
	' (get) Token: 0x06003B78 RID: 15224 RVA: 0x002150A0 File Offset: 0x002134A0
	' (set) Token: 0x06003B79 RID: 15225 RVA: 0x002150A8 File Offset: 0x002134A8
	Public Property state As ArcadePlayerColliderManager.State
		Get
			Return Me._state
		End Get
		Set(value As ArcadePlayerColliderManager.State)
			If Me._state <> value Then
				Me.pairs(value).SetCollider(Me.boxCollider)
			End If
			Me._state = value
		End Set
	End Property

	' Token: 0x06003B7A RID: 15226 RVA: 0x002150D4 File Offset: 0x002134D4
	Protected Overrides Sub OnAwake()
		MyBase.OnAwake()
		Me.boxCollider = MyBase.GetComponent(Of BoxCollider2D)()
		Me.pairs = New Dictionary(Of ArcadePlayerColliderManager.State, ArcadePlayerColliderManager.ColliderProperties)()
		Me.pairs(ArcadePlayerColliderManager.State.[Default]) = Me.colliders.[default]
		Me.pairs(ArcadePlayerColliderManager.State.Air) = Me.colliders.air
		Me.pairs(ArcadePlayerColliderManager.State.Dashing) = Me.colliders.dashing
		Me.pairs(ArcadePlayerColliderManager.State.Rocket) = Me.colliders.rocket
		Me.state = ArcadePlayerColliderManager.State.[Default]
	End Sub

	' Token: 0x06003B7B RID: 15227 RVA: 0x00215161 File Offset: 0x00213561
	Private Sub Update()
		Me.UpdateColliders()
	End Sub

	' Token: 0x06003B7C RID: 15228 RVA: 0x0021516C File Offset: 0x0021356C
	Private Sub UpdateColliders()
		Me.boxCollider.enabled = MyBase.player.CanTakeDamage
		If MyBase.player.controlScheme = ArcadePlayerController.ControlScheme.Rocket Then
			If Me.state <> ArcadePlayerColliderManager.State.Rocket Then
				Me.state = ArcadePlayerColliderManager.State.Rocket
			End If
			Return
		End If
		If MyBase.player.motor.Dashing Then
			If Me.state <> ArcadePlayerColliderManager.State.Dashing Then
				Me.state = ArcadePlayerColliderManager.State.Dashing
			End If
			Return
		End If
		If Not MyBase.player.motor.Grounded Then
			If Me.state <> ArcadePlayerColliderManager.State.Air Then
				Me.state = ArcadePlayerColliderManager.State.Air
			End If
			Return
		End If
		If Me.state <> ArcadePlayerColliderManager.State.[Default] Then
			Me.state = ArcadePlayerColliderManager.State.[Default]
		End If
	End Sub

	' Token: 0x04004302 RID: 17154
	<SerializeField()>
	Private colliders As ArcadePlayerColliderManager.ColliderPropertiesGroup

	' Token: 0x04004303 RID: 17155
	Private pairs As Dictionary(Of ArcadePlayerColliderManager.State, ArcadePlayerColliderManager.ColliderProperties)

	' Token: 0x04004304 RID: 17156
	Private boxCollider As BoxCollider2D

	' Token: 0x04004305 RID: 17157
	Private _state As ArcadePlayerColliderManager.State

	' Token: 0x020009DA RID: 2522
	Public Enum State
		' Token: 0x04004307 RID: 17159
		[Default]
		' Token: 0x04004308 RID: 17160
		Air
		' Token: 0x04004309 RID: 17161
		Dashing
		' Token: 0x0400430A RID: 17162
		Rocket
	End Enum

	' Token: 0x020009DB RID: 2523
	<Serializable()>
	Public Class ColliderProperties
		' Token: 0x06003B7D RID: 15229 RVA: 0x00215218 File Offset: 0x00213618
		Public Sub New(center As Vector2, size As Vector2)
			Me.center = center
			Me.size = size
		End Sub

		' Token: 0x06003B7E RID: 15230 RVA: 0x00215230 File Offset: 0x00213630
		Public Function CreateCollider(gameObject As GameObject) As BoxCollider2D
			Dim boxCollider2D As BoxCollider2D = gameObject.AddComponent(Of BoxCollider2D)()
			boxCollider2D.offset = Me.center
			boxCollider2D.size = Me.size
			boxCollider2D.isTrigger = True
			Return boxCollider2D
		End Function

		' Token: 0x06003B7F RID: 15231 RVA: 0x00215264 File Offset: 0x00213664
		Public Sub SetCollider(boxCollider As BoxCollider2D)
			boxCollider.offset = Me.center
			boxCollider.size = Me.size
			boxCollider.isTrigger = True
		End Sub

		' Token: 0x0400430B RID: 17163
		Public center As Vector2

		' Token: 0x0400430C RID: 17164
		Public size As Vector2
	End Class

	' Token: 0x020009DC RID: 2524
	<Serializable()>
	Public Class ColliderPropertiesGroup
		' Token: 0x0400430D RID: 17165
		Public [default] As ArcadePlayerColliderManager.ColliderProperties = New ArcadePlayerColliderManager.ColliderProperties(New Vector2(0F, 40F), New Vector2(33F, 70F))

		' Token: 0x0400430E RID: 17166
		Public air As ArcadePlayerColliderManager.ColliderProperties = New ArcadePlayerColliderManager.ColliderProperties(New Vector2(0F, 33F), New Vector2(33F, 33F))

		' Token: 0x0400430F RID: 17167
		Public dashing As ArcadePlayerColliderManager.ColliderProperties = New ArcadePlayerColliderManager.ColliderProperties(New Vector2(0F, 18F), New Vector2(33F, 23F))

		' Token: 0x04004310 RID: 17168
		Public rocket As ArcadePlayerColliderManager.ColliderProperties = New ArcadePlayerColliderManager.ColliderProperties(New Vector2(3.2F, 4F), New Vector2(4F, 66F))
	End Class
End Class
