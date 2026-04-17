Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports UnityEngine

' Token: 0x02000889 RID: 2185
Public Class TreePlatformingLevelButterflySpawner
	Inherits AbstractPausableComponent

	' Token: 0x060032CE RID: 13006 RVA: 0x001D81F9 File Offset: 0x001D65F9
	Protected Overrides Sub Awake()
		MyBase.Awake()
		MyBase.StartCoroutine(Me.instantiate_butterflies())
		MyBase.StartCoroutine(Me.spawner_cr())
	End Sub

	' Token: 0x060032CF RID: 13007 RVA: 0x001D821C File Offset: 0x001D661C
	Private Iterator Function instantiate_butterflies() As IEnumerator
		Me.butterflies = New List(Of TreePlatformingLevelButterfly)()
		Yield CupheadTime.WaitForSeconds(Me, 0.1F)
		For i As Integer = 0 To 5 - 1
			Dim treePlatformingLevelButterfly As TreePlatformingLevelButterfly = Global.UnityEngine.[Object].Instantiate(Of TreePlatformingLevelButterfly)(Me.butterflySmall)
			treePlatformingLevelButterfly.transform.parent = MyBase.transform
			treePlatformingLevelButterfly.transform.position = New Vector3(MyBase.transform.position.x, MyBase.transform.position.y - 10000F)
			Me.butterflies.Add(treePlatformingLevelButterfly)
		Next
		Yield Nothing
		Return
	End Function

	' Token: 0x060032D0 RID: 13008 RVA: 0x001D8238 File Offset: 0x001D6638
	Private Iterator Function spawner_cr() As IEnumerator
		Dim keepChecking As Boolean = True
		Dim spawn As TreePlatformingLevelButterfly = Nothing
		Yield CupheadTime.WaitForSeconds(Me, Me.initalDelay)
		While True
			While PlayerManager.GetNext().transform.position.x < Me.startButterflies.position.x
				Yield Nothing
			End While
			If Me.endButterflies IsNot Nothing Then
				While PlayerManager.GetNext().transform.position.y > Me.endButterflies.position.y
					Yield Nothing
				End While
			End If
			keepChecking = True
			While keepChecking
				For Each treePlatformingLevelButterfly As TreePlatformingLevelButterfly In Me.butterflies
					If Not treePlatformingLevelButterfly.isActive Then
						spawn = treePlatformingLevelButterfly
						keepChecking = False
						Exit For
					End If
				Next
				Yield Nothing
			End While
			Dim onLeft As Boolean = Rand.Bool()
			Dim y As Single = Global.UnityEngine.Random.Range(CupheadLevelCamera.Current.Bounds.yMin + 50F, CupheadLevelCamera.Current.Bounds.yMax - 50F)
			Dim x As Single = If((Not onLeft), (CupheadLevelCamera.Current.Bounds.xMax + 50F), (CupheadLevelCamera.Current.Bounds.xMin - 50F))
			Dim scale As Single = CSng(If((Not onLeft), (-1), 1))
			spawn.transform.position = New Vector3(x, y)
			spawn.Init(New Vector2(If((Not onLeft), (-Me.velocityX.RandomFloat()), Me.velocityX.RandomFloat()), CSng(If((Not Rand.Bool()), (-CSng(Me.velocityY)), Me.velocityY))), scale, Global.UnityEngine.Random.Range(1, 5), Me.velocityX)
			Yield CupheadTime.WaitForSeconds(Me, Me.delay.RandomFloat())
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x060032D1 RID: 13009 RVA: 0x001D8254 File Offset: 0x001D6654
	Protected Overrides Sub OnDrawGizmos()
		MyBase.OnDrawGizmos()
		Gizmos.DrawLine(Me.startButterflies.position + New Vector3(0F, 1000F), Me.startButterflies.position + New Vector3(0F, -1000F))
		Gizmos.color = Color.yellow
		If Me.endButterflies IsNot Nothing Then
			Gizmos.DrawLine(Me.endButterflies.position + New Vector3(1000F, 0F), Me.endButterflies.position + New Vector3(-1000F, 0F))
		End If
	End Sub

	' Token: 0x04003AF8 RID: 15096
	Private Const BUTTERFLIES As Integer = 5

	' Token: 0x04003AF9 RID: 15097
	Private Const OFFSET As Single = 50F

	' Token: 0x04003AFA RID: 15098
	<SerializeField()>
	Private butterflySmall As TreePlatformingLevelButterfly

	' Token: 0x04003AFB RID: 15099
	<SerializeField()>
	Private startButterflies As Transform

	' Token: 0x04003AFC RID: 15100
	<SerializeField()>
	Private endButterflies As Transform

	' Token: 0x04003AFD RID: 15101
	Private butterflies As List(Of TreePlatformingLevelButterfly)

	' Token: 0x04003AFE RID: 15102
	Private delay As MinMax = New MinMax(1.5F, 3F)

	' Token: 0x04003AFF RID: 15103
	Private velocityX As MinMax = New MinMax(300F, 600F)

	' Token: 0x04003B00 RID: 15104
	Private velocityY As MinMax = New MinMax(100F, 200F)

	' Token: 0x04003B01 RID: 15105
	Private initalDelay As Single = 6F
End Class
