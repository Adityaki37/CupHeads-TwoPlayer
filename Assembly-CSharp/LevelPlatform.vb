Imports System
Imports System.Collections.Generic
Imports UnityEngine

' Token: 0x0200043C RID: 1084
Public Class LevelPlatform
	Inherits AbstractCollidableObject

	' Token: 0x1700028D RID: 653
	' (get) Token: 0x06000FEE RID: 4078 RVA: 0x0009D8BB File Offset: 0x0009BCBB
	' (set) Token: 0x06000FEF RID: 4079 RVA: 0x0009D8C3 File Offset: 0x0009BCC3
	Protected Private Property players As List(Of Transform)

	' Token: 0x1700028E RID: 654
	' (get) Token: 0x06000FF0 RID: 4080 RVA: 0x0009D8CC File Offset: 0x0009BCCC
	Public ReadOnly Property AllowShadows As Boolean
		Get
			Return Me.allowShadows
		End Get
	End Property

	' Token: 0x06000FF1 RID: 4081 RVA: 0x0009D8D4 File Offset: 0x0009BCD4
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.players = New List(Of Transform)()
		MyBase.gameObject.layer = LayerMask.NameToLayer(Layers.Bounds_Ground.ToString())
	End Sub

	' Token: 0x06000FF2 RID: 4082 RVA: 0x0009D914 File Offset: 0x0009BD14
	Public Overridable Sub AddChild(player As Transform)
		If Not Me.players.Contains(player) Then
			Me.players.Add(player)
		End If
		player.parent = MyBase.transform
		Dim localScale As Vector3 = player.localScale
		localScale.y = 1F
		Dim component As LevelPlayerMotor = player.GetComponent(Of LevelPlayerMotor)()
		If component IsNot Nothing Then
			localScale.y *= component.GravityReversalMultiplier
		End If
		player.localScale = localScale
	End Sub

	' Token: 0x06000FF3 RID: 4083 RVA: 0x0009D98B File Offset: 0x0009BD8B
	Public Overridable Sub OnPlayerExit(player As Transform)
		If Me.players.Contains(player) Then
			Me.players.Remove(player)
		End If
	End Sub

	' Token: 0x06000FF4 RID: 4084 RVA: 0x0009D9AC File Offset: 0x0009BDAC
	Protected Overrides Sub OnDestroy()
		For Each transform As Transform In Me.players
			If Not(transform Is Nothing) Then
				transform.parent = Nothing
			End If
		Next
		MyBase.OnDestroy()
	End Sub

	' Token: 0x04001984 RID: 6532
	Public canFallThrough As Boolean = True

	' Token: 0x04001986 RID: 6534
	<SerializeField()>
	Private allowShadows As Boolean = True
End Class
