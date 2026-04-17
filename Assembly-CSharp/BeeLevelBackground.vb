Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports UnityEngine

' Token: 0x02000510 RID: 1296
Public Class BeeLevelBackground
	Inherits LevelProperties.Bee.Entity

	' Token: 0x0600170F RID: 5903 RVA: 0x000CF508 File Offset: 0x000CD908
	Private Sub Start()
		Me.level = TryCast(Level.Current, BeeLevel)
		MyBase.StartCoroutine(Me.middle_cr())
	End Sub

	' Token: 0x06001710 RID: 5904 RVA: 0x000CF527 File Offset: 0x000CD927
	Private Sub Update()
		Me.back.speed = -Me.level.Speed * 0.35F
	End Sub

	' Token: 0x06001711 RID: 5905 RVA: 0x000CF548 File Offset: 0x000CD948
	Public Overrides Sub LevelInit(properties As LevelProperties.Bee)
		MyBase.LevelInit(properties)
		Dim array As Integer() = New Integer(Me.groups.Length - 1) {}
		Dim list As List(Of Integer) = New List(Of Integer)()
		For i As Integer = 0 To Me.groups.Length - 1
			list.Add(i)
		Next
		For j As Integer = 0 To Me.groups.Length - 1
			Dim num As Integer = Global.UnityEngine.Random.Range(0, list.Count)
			array(j) = list(num)
			list.RemoveAt(num)
			Me.groups(array(j)).Init(Me.platformGroup, Me.groups.Length)
			Me.groups(array(j)).SetY(-455F * CSng(j))
		Next
		Me.platformGroup.gameObject.SetActive(False)
	End Sub

	' Token: 0x06001712 RID: 5906 RVA: 0x000CF610 File Offset: 0x000CDA10
	Private Iterator Function middle_cr() As IEnumerator
		Dim sprites As SpriteRenderer() = New SpriteRenderer(Me.middleGroups.Length - 1) {}
		For j As Integer = 0 To Me.middleGroups.Length - 1
			sprites(j) = Me.middleGroups(j).GetComponentInChildren(Of SpriteRenderer)()
			Me.middleGroups(j).gameObject.SetActive(False)
		Next
		Dim scale As Integer = If((Global.UnityEngine.Random.value <= 0.5F), (-1), 1)
		While True
			Dim i As Integer = Global.UnityEngine.Random.Range(0, Me.middleGroups.Length)
			Dim height As Single = CSng(CInt(sprites(i).sprite.bounds.size.y))
			Dim y As Single = (720F + height) / 2F
			Me.middleGroups(i).gameObject.SetActive(True)
			Me.middleGroups(i).SetPosition(New Single?(0F), New Single?(y), New Single?(0F))
			Me.middleGroups(i).SetScale(New Single?(CSng(scale)), New Single?(1F), New Single?(1F))
			While Me.middleGroups(i).position.y >= -y
				Me.middleGroups(i).AddPosition(0F, Me.level.Speed * 0.75F * CupheadTime.Delta, 0F)
				Yield Nothing
			End While
			Me.middleGroups(i).gameObject.SetActive(False)
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x04002056 RID: 8278
	Public Const GROUP_OFFSET As Single = 455F

	' Token: 0x04002057 RID: 8279
	<SerializeField()>
	Private platformGroup As BeeLevelPlatforms

	' Token: 0x04002058 RID: 8280
	<SerializeField()>
	Private groups As BeeLevelBackgroundGroup()

	' Token: 0x04002059 RID: 8281
	<SerializeField()>
	Private middleGroups As Transform()

	' Token: 0x0400205A RID: 8282
	<SerializeField()>
	Private back As ScrollingSprite

	' Token: 0x0400205B RID: 8283
	Private level As BeeLevel
End Class
