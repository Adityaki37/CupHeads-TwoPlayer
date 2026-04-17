Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000573 RID: 1395
Public Class DevilLevelEffectSpawner
	Inherits AbstractPausableComponent

	' Token: 0x06001A6F RID: 6767 RVA: 0x000F1DE3 File Offset: 0x000F01E3
	Private Sub Start()
		MyBase.StartCoroutine(Me.main_cr())
	End Sub

	' Token: 0x06001A70 RID: 6768 RVA: 0x000F1DF4 File Offset: 0x000F01F4
	Private Iterator Function main_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, Global.UnityEngine.Random.Range(0F, Me.waitTime.max))
		Yield Nothing
		While True
			Me.effect = Me.effectPrefab.Create(MyBase.transform.position)
			Me.effect.transform.parent = MyBase.transform
			While Me.effect IsNot Nothing
				Yield Nothing
			End While
			Yield CupheadTime.WaitForSeconds(Me, Me.waitTime.RandomFloat())
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06001A71 RID: 6769 RVA: 0x000F1E0F File Offset: 0x000F020F
	Public Sub KillSmoke()
		Me.StopAllCoroutines()
		If Me.isSmoke3 Then
			MyBase.StartCoroutine(Me.fade_out_cr())
		End If
	End Sub

	' Token: 0x06001A72 RID: 6770 RVA: 0x000F1E30 File Offset: 0x000F0230
	Private Iterator Function fade_out_cr() As IEnumerator
		Dim t As Single = 0F
		Dim time As Single = 0.5F
		While t < time
			t += CupheadTime.Delta
			If Me.effect IsNot Nothing Then
				Me.effect.GetComponent(Of SpriteRenderer)().color = New Color(1F, 1F, 1F, 1F - t / time)
			End If
			Yield Nothing
		End While
		Yield Nothing
		Return
	End Function

	' Token: 0x06001A73 RID: 6771 RVA: 0x000F1E4B File Offset: 0x000F024B
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me.effectPrefab = Nothing
	End Sub

	' Token: 0x0400239B RID: 9115
	<SerializeField()>
	Private isSmoke3 As Boolean

	' Token: 0x0400239C RID: 9116
	Public waitTime As MinMax

	' Token: 0x0400239D RID: 9117
	Public effectPrefab As Effect

	' Token: 0x0400239E RID: 9118
	Private effect As Effect
End Class
