Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000638 RID: 1592
Public Class FlyingBlimpLevelFadeForeground
	Inherits FlyingBlimpLevelScrollingSpriteSpawnerBase

	' Token: 0x060020A5 RID: 8357 RVA: 0x0012D394 File Offset: 0x0012B794
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.fadeTime = 10F
		Me.nightSprite = New Transform(Me.spritePrefabs.Length - 1) {}
		If Me.spawnedChild IsNot Nothing Then
			Me.spawnedChild.transform.gameObject.GetComponent(Of SpriteRenderer)().enabled = False
		End If
		For i As Integer = 0 To Me.nightSprite.Length - 1
			Me.nightSprite(i) = Me.spritePrefabs(i).sprite.transform.GetChild(0)
			Me.nightSprite(i).transform.gameObject.GetComponent(Of SpriteRenderer)().enabled = False
		Next
	End Sub

	' Token: 0x060020A6 RID: 8358 RVA: 0x0012D447 File Offset: 0x0012B847
	Protected Overrides Sub OnSpawn(obj As GameObject)
		MyBase.OnSpawn(obj)
		Me.spawnedChild = obj.transform.GetChild(0)
	End Sub

	' Token: 0x060020A7 RID: 8359 RVA: 0x0012D462 File Offset: 0x0012B862
	Private Sub Update()
		If Me.moonLady.state = FlyingBlimpLevelMoonLady.State.Morph AndAlso Not Me.startedChange Then
			Me.startedChange = True
			Me.StartChange()
		End If
	End Sub

	' Token: 0x060020A8 RID: 8360 RVA: 0x0012D48D File Offset: 0x0012B88D
	Private Sub StartChange()
		MyBase.StartCoroutine(Me.change_cr())
	End Sub

	' Token: 0x060020A9 RID: 8361 RVA: 0x0012D49C File Offset: 0x0012B89C
	Private Iterator Function change_cr() As IEnumerator
		Dim t As Single = 0F
		Dim startSpeed As Single = Me.speed
		Dim endSpeed As Single = Me.speed + Me.speed * 0.3F
		While t < Me.fadeTime
			If Me.spawnedChild IsNot Nothing Then
				Me.spawnedChild.transform.gameObject.GetComponent(Of SpriteRenderer)().enabled = True
				Me.spawnedChild.transform.gameObject.GetComponent(Of SpriteRenderer)().color = New Color(1F, 1F, 1F, t / Me.fadeTime)
			End If
			For j As Integer = 0 To Me.nightSprite.Length - 1
				If Me.nightSprite(j).transform IsNot Nothing Then
					Me.nightSprite(j).transform.gameObject.GetComponent(Of SpriteRenderer)().enabled = True
					Me.nightSprite(j).transform.gameObject.GetComponent(Of SpriteRenderer)().color = New Color(1F, 1F, 1F, t / Me.fadeTime)
				End If
			Next
			Me.speed = Mathf.Lerp(startSpeed, endSpeed, t / Me.fadeTime)
			t += CupheadTime.Delta
			Yield Nothing
		End While
		If Me.spawnedChild IsNot Nothing Then
			Me.spawnedChild.transform.gameObject.GetComponent(Of SpriteRenderer)().color = New Color(1F, 1F, 1F, 1F)
		End If
		For i As Integer = 0 To Me.nightSprite.Length - 1
			If Me.nightSprite(i).transform IsNot Nothing Then
				Me.nightSprite(i).transform.gameObject.GetComponent(Of SpriteRenderer)().color = New Color(1F, 1F, 1F, 1F)
				Yield Nothing
			End If
		Next
		Yield Nothing
		Return
	End Function

	' Token: 0x04002929 RID: 10537
	<SerializeField()>
	Private moonLady As FlyingBlimpLevelMoonLady

	' Token: 0x0400292A RID: 10538
	Private nightSprite As Transform()

	' Token: 0x0400292B RID: 10539
	Private spawnedChild As Transform

	' Token: 0x0400292C RID: 10540
	Private fadeTime As Single

	' Token: 0x0400292D RID: 10541
	Private index As Integer

	' Token: 0x0400292E RID: 10542
	Private startedChange As Boolean
End Class
