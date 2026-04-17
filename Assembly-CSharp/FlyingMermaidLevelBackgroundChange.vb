Imports System
Imports System.Collections.Generic
Imports UnityEngine

' Token: 0x02000680 RID: 1664
Public Class FlyingMermaidLevelBackgroundChange
	Inherits AbstractPausableComponent

	' Token: 0x06002323 RID: 8995 RVA: 0x0014A07C File Offset: 0x0014847C
	Private Sub Start()
		Me.points = New List(Of Transform)()
		Me.size = Me.toCopy.GetComponent(Of Collider2D)().bounds.size.x
		Me.getOffset.x = MyBase.transform.position.x
		Me.copy1 = Global.UnityEngine.[Object].Instantiate(Of FlyingMermaidLevelCoralCluster)(Me.toCopy)
		Me.copy1.transform.parent = MyBase.transform
		Dim flyingMermaidLevelCoralCluster As FlyingMermaidLevelCoralCluster = Global.UnityEngine.[Object].Instantiate(Of FlyingMermaidLevelCoralCluster)(Me.toCopy)
		flyingMermaidLevelCoralCluster.transform.parent = MyBase.transform
		Me.copy1.transform.SetPosition(New Single?(Me.getOffset.x + Me.size), New Single?(Me.toCopy.transform.position.y), New Single?(0F))
		flyingMermaidLevelCoralCluster.transform.SetPosition(New Single?(Me.getOffset.x + Me.size * 2F), New Single?(Me.toCopy.transform.position.y), New Single?(0F))
		Me.points.AddRange(Me.toCopy.points)
		Me.points.AddRange(Me.copy1.points)
		Me.points.AddRange(flyingMermaidLevelCoralCluster.points)
		Me.copies = New List(Of FlyingMermaidLevelCoralCluster)()
		Me.copies.Add(Me.toCopy)
		Me.copies.Add(Me.copy1)
		Me.copies.Add(flyingMermaidLevelCoralCluster)
	End Sub

	' Token: 0x06002324 RID: 8996 RVA: 0x0014A234 File Offset: 0x00148634
	Private Sub FixedUpdate()
		If MyBase.GetComponent(Of ParallaxLayer)() IsNot Nothing Then
			MyBase.GetComponent(Of ParallaxLayer)().enabled = False
		End If
		Dim localPosition As Vector3 = MyBase.transform.localPosition
		If Me.copies(Me.index).transform.position.x <= -Me.size Then
			Me.copies(Me.index).transform.position = New Vector2(Me.size * 2F, Me.copies(Me.index).transform.position.y)
			Me.index = (Me.index + 1) Mod Me.copies.Count
		End If
		localPosition.x -= Me.speed * CupheadTime.FixedDelta * Me.b_playbackSpeed
		MyBase.transform.localPosition = localPosition
	End Sub

	' Token: 0x04002BC0 RID: 11200
	Public points As List(Of Transform)

	' Token: 0x04002BC1 RID: 11201
	Private size As Single

	' Token: 0x04002BC2 RID: 11202
	Private Const X_OUT As Single = -1280F

	' Token: 0x04002BC3 RID: 11203
	<Range(0F, 2000F)>
	Public speed As Single

	' Token: 0x04002BC4 RID: 11204
	<NonSerialized()>
	Public b_playbackSpeed As Single = 1F

	' Token: 0x04002BC5 RID: 11205
	<SerializeField()>
	Private toCopy As FlyingMermaidLevelCoralCluster

	' Token: 0x04002BC6 RID: 11206
	Private copy1 As FlyingMermaidLevelCoralCluster

	' Token: 0x04002BC7 RID: 11207
	Private copies As List(Of FlyingMermaidLevelCoralCluster)

	' Token: 0x04002BC8 RID: 11208
	Private getOffset As Vector3

	' Token: 0x04002BC9 RID: 11209
	Private _offset As Vector3

	' Token: 0x04002BCA RID: 11210
	Private index As Integer
End Class
