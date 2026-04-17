Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports UnityEngine

' Token: 0x02000A9C RID: 2716
Public Class PlanePlayerMotor
	Inherits AbstractPlanePlayerComponent

	' Token: 0x170005AF RID: 1455
	' (get) Token: 0x06004118 RID: 16664 RVA: 0x00235D28 File Offset: 0x00234128
	' (set) Token: 0x06004119 RID: 16665 RVA: 0x00235D30 File Offset: 0x00234130
	Public Property MoveDirection As Trilean2

	' Token: 0x0600411A RID: 16666 RVA: 0x00235D3C File Offset: 0x0023413C
	Protected Overrides Sub OnAwake()
		MyBase.OnAwake()
		Me.MoveDirection = Nothing
		Me.properties = New PlanePlayerMotor.Properties()
		AddHandler MyBase.player.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
		AddHandler MyBase.player.OnReviveEvent, AddressOf Me.OnRevive
		Me.pos = MyBase.transform.position
	End Sub

	' Token: 0x0600411B RID: 16667 RVA: 0x00235DB2 File Offset: 0x002341B2
	Private Sub Start()
		Me.pos = MyBase.transform.position
	End Sub

	' Token: 0x0600411C RID: 16668 RVA: 0x00235DCA File Offset: 0x002341CA
	Private Sub FixedUpdate()
		If MyBase.player.stats.StoneTime > 0F Then
			Return
		End If
		Me.HandleInput()
		Me.Move()
		Me.HandleRaycasts()
		Me.ClampPosition()
	End Sub

	' Token: 0x0600411D RID: 16669 RVA: 0x00235DFF File Offset: 0x002341FF
	Private Sub LateUpdate()
		Me.ClampPosition()
	End Sub

	' Token: 0x0600411E RID: 16670 RVA: 0x00235E07 File Offset: 0x00234207
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		If info.damage > 0F Then
			MyBase.StartCoroutine(Me.onDamageTaken_cr())
		End If
	End Sub

	' Token: 0x0600411F RID: 16671 RVA: 0x00235E28 File Offset: 0x00234228
	Private Sub HandleInput()
		Me.timeSinceInputBuffered += CupheadTime.FixedDelta
		If MyBase.player.WeaponBusy Then
			Me.BufferInputs()
		End If
		If Me.damageStun Then
			Me.MoveDirection = New Trilean2(-1, 0)
			Return
		End If
		Dim trilean As Trilean = 0
		Dim trilean2 As Trilean = 0
		Dim axis As Single = MyBase.player.input.actions.GetAxis(0)
		Dim axis2 As Single = MyBase.player.input.actions.GetAxis(1)
		If axis > 0.35F OrElse axis < -0.35F Then
			trilean = axis
		End If
		If axis2 > 0.35F OrElse axis2 < -0.35F Then
			trilean2 = axis2
		End If
		Me.MoveDirection = New Trilean2(trilean.Value, trilean2.Value)
	End Sub

	' Token: 0x06004120 RID: 16672 RVA: 0x00235F05 File Offset: 0x00234305
	Private Sub BufferInput(input As PlanePlayerMotor.BufferedInput)
		Me.bufferedInput = input
		Me.timeSinceInputBuffered = 0F
	End Sub

	' Token: 0x06004121 RID: 16673 RVA: 0x00235F1C File Offset: 0x0023431C
	Public Sub BufferInputs()
		If MyBase.player.input.actions.GetButtonDown(2) Then
			Me.BufferInput(PlanePlayerMotor.BufferedInput.Jump)
		ElseIf MyBase.player.input.actions.GetButtonDown(4) Then
			Me.BufferInput(PlanePlayerMotor.BufferedInput.Super)
		End If
	End Sub

	' Token: 0x06004122 RID: 16674 RVA: 0x00235F72 File Offset: 0x00234372
	Public Sub ClearBufferedInput()
		Me.timeSinceInputBuffered = 0.134F
	End Sub

	' Token: 0x06004123 RID: 16675 RVA: 0x00235F7F File Offset: 0x0023437F
	Public Function HasBufferedInput(input As PlanePlayerMotor.BufferedInput) As Boolean
		Return Me.bufferedInput = input AndAlso Me.timeSinceInputBuffered < 0.134F
	End Function

	' Token: 0x170005B0 RID: 1456
	' (get) Token: 0x06004124 RID: 16676 RVA: 0x00235F9D File Offset: 0x0023439D
	Public ReadOnly Property Velocity As Vector2
		Get
			Return Me._velocity
		End Get
	End Property

	' Token: 0x06004125 RID: 16677 RVA: 0x00235FA8 File Offset: 0x002343A8
	Private Sub Move()
		Dim num As Single = If((Not MyBase.player.Shrunk), Me.properties.speed, Me.properties.shrunkSpeed)
		If Me.MoveDirection.x <> 0 AndAlso Me.MoveDirection.y <> 0 Then
			num *= 0.75F
		End If
		Me.pos.x = Me.pos.x + Me.MoveDirection.x * num * CupheadTime.FixedDelta
		Me.pos.y = Me.pos.y + Me.MoveDirection.y * num * CupheadTime.FixedDelta
		For Each force As PlanePlayerMotor.Force In Me.externalForces
			If force.enabled Then
				Me.pos += force.force * CupheadTime.FixedDelta
			End If
		Next
		Dim vector As Vector2 = MyBase.transform.position
		If PlanePlayerMotor.USE_FALLOFF Then
			Dim num2 As Single = 15F
			MyBase.transform.position = Vector2.Lerp(MyBase.transform.position, Me.pos, num2 * CupheadTime.FixedDelta)
		Else
			MyBase.transform.AddPosition(0F, Me.MoveDirection.y * num * CupheadTime.FixedDelta, 0F)
		End If
		Dim vector2 As Vector2 = MyBase.transform.position
		Me._velocity = (vector2 - vector) / CupheadTime.FixedDelta
	End Sub

	' Token: 0x06004126 RID: 16678 RVA: 0x002361A4 File Offset: 0x002345A4
	Private Sub HandleRaycasts()
		Dim num As Integer = 262144
		Dim num2 As Integer = 524288
		Dim num3 As Integer = 1048576
		Dim vector As Vector2 = MyBase.transform.position + New Vector2(-20F, 15F)
		Dim num4 As Single = 100F
		Dim num5 As Single = 100F
		Dim raycastHit2D As RaycastHit2D = Physics2D.BoxCast(vector, New Vector2(1F, num5), 0F, Vector2.left, num4 * 0.5F, num)
		Dim raycastHit2D2 As RaycastHit2D = Physics2D.BoxCast(vector, New Vector2(1F, num5), 0F, Vector2.right, num4 * 0.5F, num)
		Dim raycastHit2D3 As RaycastHit2D = Physics2D.BoxCast(vector, New Vector2(num4, 1F), 0F, Vector2.up, num5 * 0.5F, num2)
		Dim raycastHit2D4 As RaycastHit2D = Physics2D.BoxCast(vector, New Vector2(num4, 1F), 0F, Vector2.down, num5 * 0.5F, num3)
		If raycastHit2D.collider IsNot Nothing Then
			MyBase.transform.SetPosition(New Single?(raycastHit2D.point.x + 70F), Nothing, Nothing)
		End If
		If raycastHit2D2.collider IsNot Nothing Then
			MyBase.transform.SetPosition(New Single?(raycastHit2D2.point.x - 30F), Nothing, Nothing)
		End If
		If raycastHit2D3.collider IsNot Nothing Then
			MyBase.transform.SetPosition(Nothing, New Single?(raycastHit2D3.point.y - 65F), Nothing)
		End If
		If raycastHit2D4.collider IsNot Nothing Then
			MyBase.transform.SetPosition(Nothing, New Single?(raycastHit2D4.point.y + 35F), Nothing)
		End If
	End Sub

	' Token: 0x06004127 RID: 16679 RVA: 0x002363C4 File Offset: 0x002347C4
	Private Sub ClampPosition()
		Dim vector As Vector2 = Me.pos
		vector.x = Mathf.Clamp(vector.x, CSng(Level.Current.Left) + 70F, CSng(Level.Current.Right) - 30F)
		vector.y = Mathf.Clamp(vector.y, CSng(Level.Current.Ground) + 35F, CSng(Level.Current.Ceiling) - 65F)
		Me.pos = vector
	End Sub

	' Token: 0x06004128 RID: 16680 RVA: 0x0023644C File Offset: 0x0023484C
	Public Sub OnRevive(pos As Vector3)
		MyBase.transform.position = pos
		Me.pos = pos
		Me.MoveDirection = Nothing
		Me.damageStun = False
	End Sub

	' Token: 0x06004129 RID: 16681 RVA: 0x00236488 File Offset: 0x00234888
	Private Iterator Function onDamageTaken_cr() As IEnumerator
		Me.damageStun = True
		Yield CupheadTime.WaitForSeconds(Me, 0.15F)
		Me.damageStun = False
		Return
	End Function

	' Token: 0x0600412A RID: 16682 RVA: 0x002364A3 File Offset: 0x002348A3
	Public Sub AddForce(force As PlanePlayerMotor.Force)
		Me.externalForces.Add(force)
	End Sub

	' Token: 0x0600412B RID: 16683 RVA: 0x002364B1 File Offset: 0x002348B1
	Public Sub RemoveForce(force As PlanePlayerMotor.Force)
		Me.externalForces.Remove(force)
	End Sub

	' Token: 0x040047B7 RID: 18359
	Private Const PADDING_TOP As Single = 65F

	' Token: 0x040047B8 RID: 18360
	Private Const PADDING_BOTTOM As Single = 35F

	' Token: 0x040047B9 RID: 18361
	Private Const PADDING_LEFT As Single = 70F

	' Token: 0x040047BA RID: 18362
	Private Const PADDING_RIGHT As Single = 30F

	' Token: 0x040047BB RID: 18363
	Private Const DIAGONAL_FALLOFF As Single = 0.75F

	' Token: 0x040047BC RID: 18364
	Private Const ANALOG_THRESHOLD As Single = 0.35F

	' Token: 0x040047BD RID: 18365
	Private Const HIT_STUN_TIME As Single = 0.15F

	' Token: 0x040047BE RID: 18366
	Private Const EASING_TIME As Single = 15F

	' Token: 0x040047BF RID: 18367
	Private Shared USE_FALLOFF As Boolean = True

	' Token: 0x040047C0 RID: 18368
	<NonSerialized()>
	Public properties As PlanePlayerMotor.Properties

	' Token: 0x040047C2 RID: 18370
	Private damageStun As Boolean

	' Token: 0x040047C3 RID: 18371
	Private pos As Vector2

	' Token: 0x040047C4 RID: 18372
	Private externalForces As List(Of PlanePlayerMotor.Force) = New List(Of PlanePlayerMotor.Force)()

	' Token: 0x040047C5 RID: 18373
	Private bufferedInput As PlanePlayerMotor.BufferedInput

	' Token: 0x040047C6 RID: 18374
	Private timeSinceInputBuffered As Single = 0.134F

	' Token: 0x040047C7 RID: 18375
	Private _velocity As Vector2

	' Token: 0x02000A9D RID: 2717
	Public Enum BufferedInput
		' Token: 0x040047C9 RID: 18377
		Jump
		' Token: 0x040047CA RID: 18378
		Super
	End Enum

	' Token: 0x02000A9E RID: 2718
	Public Class Force
		' Token: 0x0600412D RID: 16685 RVA: 0x00134CFC File Offset: 0x001330FC
		Public Sub New(force As Vector2, enabled As Boolean)
			Me.force = force
			Me.enabled = enabled
		End Sub

		' Token: 0x170005B1 RID: 1457
		' (get) Token: 0x0600412E RID: 16686 RVA: 0x00134D12 File Offset: 0x00133112
		' (set) Token: 0x0600412F RID: 16687 RVA: 0x00134D1A File Offset: 0x0013311A
		Public Overridable Property force As Vector2

		' Token: 0x040047CC RID: 18380
		Public enabled As Boolean
	End Class

	' Token: 0x02000A9F RID: 2719
	<Serializable()>
	Public Class Properties
		' Token: 0x040047CD RID: 18381
		Public speed As Single = 520F

		' Token: 0x040047CE RID: 18382
		Public shrunkSpeed As Single = 720F
	End Class
End Class
