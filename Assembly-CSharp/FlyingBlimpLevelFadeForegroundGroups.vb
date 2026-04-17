Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports UnityEngine

' Token: 0x02000639 RID: 1593
Public Class FlyingBlimpLevelFadeForegroundGroups
	Inherits FlyingBlimpLevelScrollingSpriteSpawnerBase

	' Token: 0x060020AB RID: 8363 RVA: 0x0012D820 File Offset: 0x0012BC20
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.fadeTime = 10F
		Me.daySprites = New List(Of Transform)()
		Me.nightSprites = New List(Of Transform)()
		If Me.spawnedChild IsNot Nothing Then
			Me.spawnedChild.transform.gameObject.GetComponentInChildren(Of SpriteRenderer)().enabled = False
		End If
		For i As Integer = 0 To Me.spritePrefabs.Length - 1
			For Each transform As Transform In Me.spritePrefabs(i).sprite.transform.GetChildTransforms()
				Me.daySprites.Add(transform.transform)
				Me.nightSprites.Add(transform.transform.GetChild(0))
			Next
		Next
		For k As Integer = 0 To Me.nightSprites.Count - 1
			If Me.nightSprites(k).transform IsNot Nothing Then
				Me.nightSprites(k).transform.gameObject.GetComponent(Of SpriteRenderer)().enabled = False
			End If
		Next
	End Sub

	' Token: 0x060020AC RID: 8364 RVA: 0x0012D950 File Offset: 0x0012BD50
	Protected Overrides Sub OnSpawn(obj As GameObject)
		MyBase.OnSpawn(obj)
		Me.spawnedChild = obj.transform.GetChild(0)
	End Sub

	' Token: 0x060020AD RID: 8365 RVA: 0x0012D96B File Offset: 0x0012BD6B
	Private Sub Update()
		If Me.moonLady.state = FlyingBlimpLevelMoonLady.State.Morph AndAlso Not Me.startedChange Then
			Me.startedChange = True
			Me.StartChange()
		End If
	End Sub

	' Token: 0x060020AE RID: 8366 RVA: 0x0012D996 File Offset: 0x0012BD96
	Private Sub StartChange()
		MyBase.StartCoroutine(Me.change_cr())
	End Sub

	' Token: 0x060020AF RID: 8367 RVA: 0x0012D9A8 File Offset: 0x0012BDA8
	Private Iterator Function change_cr() As IEnumerator
		Dim t As Single = 0F
		Dim startSpeed As Single = Me.speed
		Dim endSpeed As Single = Me.speed + Me.speed * 0.3F
		While t < Me.fadeTime
			For i As Integer = 0 To Me.nightSprites.Count - 1
				If Me.nightSprites(i).transform IsNot Nothing Then
					Me.nightSprites(i).transform.gameObject.GetComponent(Of SpriteRenderer)().enabled = True
					Me.nightSprites(i).transform.gameObject.GetComponent(Of SpriteRenderer)().color = New Color(1F, 1F, 1F, t / Me.fadeTime)
				End If
			Next
			Me.speed = Mathf.Lerp(startSpeed, endSpeed, t / Me.fadeTime)
			t += CupheadTime.Delta
			Yield Nothing
		End While
		For j As Integer = 0 To Me.nightSprites.Count - 1
			If Me.nightSprites(j).transform IsNot Nothing Then
				Me.nightSprites(j).transform.gameObject.GetComponent(Of SpriteRenderer)().color = New Color(1F, 1F, 1F, 1F)
			End If
		Next
		Return
	End Function

	' Token: 0x0400292F RID: 10543
	<SerializeField()>
	Private moonLady As FlyingBlimpLevelMoonLady

	' Token: 0x04002930 RID: 10544
	Private daySprites As List(Of Transform)

	' Token: 0x04002931 RID: 10545
	Private nightSprites As List(Of Transform)

	' Token: 0x04002932 RID: 10546
	Private spawnedChild As Transform

	' Token: 0x04002933 RID: 10547
	Private fadeTime As Single

	' Token: 0x04002934 RID: 10548
	Private index As Integer

	' Token: 0x04002935 RID: 10549
	Private allDayChildren As Integer

	' Token: 0x04002936 RID: 10550
	Private startedChange As Boolean
End Class
