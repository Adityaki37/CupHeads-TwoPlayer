Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000503 RID: 1283
Public Class BatLevelBouncer
	Inherits AbstractCollidableObject

	' Token: 0x060016B0 RID: 5808 RVA: 0x000CC47D File Offset: 0x000CA87D
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.damageDealer = DamageDealer.NewEnemy()
		Me.lastSideHit = BatLevelBouncer.SideHit.None
	End Sub

	' Token: 0x060016B1 RID: 5809 RVA: 0x000CC497 File Offset: 0x000CA897
	Private Sub Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x060016B2 RID: 5810 RVA: 0x000CC4B0 File Offset: 0x000CA8B0
	Public Sub Init(properties As LevelProperties.Bat.BatBouncer, pos As Vector2, angle As Single)
		Me.properties = properties
		MyBase.transform.position = pos
		Me.angle = angle
		MyBase.transform.SetEulerAngles(New Single?(0F), New Single?(0F), New Single?(angle))
		MyBase.StartCoroutine(Me.move_cr())
	End Sub

	' Token: 0x060016B3 RID: 5811 RVA: 0x000CC510 File Offset: 0x000CA910
	Protected Iterator Function move_cr() As IEnumerator
		Me.velocity = -MyBase.transform.right
		While True
			MyBase.transform.position += Me.velocity * Me.properties.mainBounceSpeed * CupheadTime.FixedDelta
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x060016B4 RID: 5812 RVA: 0x000CC52B File Offset: 0x000CA92B
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		Me.damageDealer.DealDamage(hit)
	End Sub

	' Token: 0x060016B5 RID: 5813 RVA: 0x000CC544 File Offset: 0x000CA944
	Protected Overrides Sub OnCollisionCeiling(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionCeiling(hit, phase)
		If phase = CollisionPhase.Enter AndAlso Me.lastSideHit <> BatLevelBouncer.SideHit.Top Then
			Dim position As Vector3 = MyBase.transform.position
			Dim vector As Vector3 = New Vector3(MyBase.transform.position.x, CSng(Level.Current.Ground), 0F)
			Me.collisionPoint = vector - position
			Me.lastSideHit = BatLevelBouncer.SideHit.Top
			MyBase.StartCoroutine(Me.change_direction_cr(Me.collisionPoint))
		End If
	End Sub

	' Token: 0x060016B6 RID: 5814 RVA: 0x000CC5C8 File Offset: 0x000CA9C8
	Protected Overrides Sub OnCollisionWalls(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionCeiling(hit, phase)
		If phase = CollisionPhase.Enter Then
			If MyBase.transform.position.x > 0F Then
				If Me.lastSideHit <> BatLevelBouncer.SideHit.Right Then
					Dim vector As Vector3 = New Vector3(CSng(Level.Current.Left), MyBase.transform.position.y, 0F)
					Dim position As Vector3 = MyBase.transform.position
					Me.collisionPoint = vector - position
					Me.lastSideHit = BatLevelBouncer.SideHit.Right
					MyBase.StartCoroutine(Me.change_direction_cr(Me.collisionPoint))
				End If
			ElseIf Me.lastSideHit <> BatLevelBouncer.SideHit.Left Then
				Dim position2 As Vector3 = MyBase.transform.position
				Dim vector2 As Vector3 = New Vector3(CSng(Level.Current.Right), MyBase.transform.position.y, 0F)
				Me.collisionPoint = vector2 - position2
				Me.lastSideHit = BatLevelBouncer.SideHit.Left
				MyBase.StartCoroutine(Me.change_direction_cr(Me.collisionPoint))
			End If
		End If
	End Sub

	' Token: 0x060016B7 RID: 5815 RVA: 0x000CC6DC File Offset: 0x000CAADC
	Protected Overrides Sub OnCollisionGround(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionCeiling(hit, phase)
		If phase = CollisionPhase.Enter AndAlso Me.lastSideHit <> BatLevelBouncer.SideHit.Bottom Then
			Dim position As Vector3 = MyBase.transform.position
			Dim vector As Vector3 = New Vector3(MyBase.transform.position.x, CSng(Level.Current.Ceiling), 0F)
			Me.collisionPoint = vector - position
			Me.lastSideHit = BatLevelBouncer.SideHit.Bottom
			MyBase.StartCoroutine(Me.change_direction_cr(Me.collisionPoint))
		End If
	End Sub

	' Token: 0x060016B8 RID: 5816 RVA: 0x000CC760 File Offset: 0x000CAB60
	Protected Iterator Function change_direction_cr(collisionPoint As Vector3) As IEnumerator
		Me.velocity = 1F * (-2F * Vector3.Dot(Me.velocity, Vector3.Normalize(collisionPoint.normalized)) * Vector3.Normalize(collisionPoint.normalized) + Me.velocity)
		Me.counter += 1
		If CSng(Me.counter) >= Me.properties.breakCounter AndAlso Not Me.isPink Then
			Me.SpawnPink()
			Me.Die()
		End If
		Yield Nothing
		Return
	End Function

	' Token: 0x060016B9 RID: 5817 RVA: 0x000CC784 File Offset: 0x000CAB84
	Protected Sub SpawnPink()
		Dim batLevelPinkCore As BatLevelPinkCore = Global.UnityEngine.[Object].Instantiate(Of BatLevelPinkCore)(Me.pinkPrefab)
		batLevelPinkCore.Init(Me.properties, MyBase.transform.position, Me.angle)
	End Sub

	' Token: 0x060016BA RID: 5818 RVA: 0x000CC7BF File Offset: 0x000CABBF
	Protected Sub Die()
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub

	' Token: 0x04001FFE RID: 8190
	<SerializeField()>
	Private pinkPrefab As BatLevelPinkCore

	' Token: 0x04001FFF RID: 8191
	Private properties As LevelProperties.Bat.BatBouncer

	' Token: 0x04002000 RID: 8192
	Private damageDealer As DamageDealer

	' Token: 0x04002001 RID: 8193
	Private velocity As Vector3

	' Token: 0x04002002 RID: 8194
	Private collisionPoint As Vector3

	' Token: 0x04002003 RID: 8195
	Public lastSideHit As BatLevelBouncer.SideHit

	' Token: 0x04002004 RID: 8196
	Private isPink As Boolean

	' Token: 0x04002005 RID: 8197
	Private angle As Single

	' Token: 0x04002006 RID: 8198
	Private counter As Integer

	' Token: 0x02000504 RID: 1284
	Public Enum SideHit
		' Token: 0x04002008 RID: 8200
		Top
		' Token: 0x04002009 RID: 8201
		Bottom
		' Token: 0x0400200A RID: 8202
		Left
		' Token: 0x0400200B RID: 8203
		Right
		' Token: 0x0400200C RID: 8204
		None
	End Enum
End Class
