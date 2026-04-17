Imports System
Imports UnityEngine

' Token: 0x02000B33 RID: 2867
Public Class WinScreenCharacterAnimation
	Inherits AbstractMonoBehaviour

	' Token: 0x06004579 RID: 17785 RVA: 0x00249C78 File Offset: 0x00248078
	Private Sub Start()
		Me.blinkLayer.enabled = False
		If Me.blinkLayer2 IsNot Nothing Then
			Me.blinkLayer2.enabled = False
		End If
		Me.results.SetBool("pickedA", Rand.Bool())
		Me.singleBlinkAmount = Global.UnityEngine.Random.Range(0, 4) + 4
		Me.doubleBlinkAmount = Global.UnityEngine.Random.Range(0, 10) + 16
	End Sub

	' Token: 0x0600457A RID: 17786 RVA: 0x00249CE4 File Offset: 0x002480E4
	Private Sub EndCycle()
		Me.blinkLayer.enabled = False
		If Me.blinkLayer2 IsNot Nothing Then
			Me.blinkLayer2.enabled = False
		End If
		If Me.singleCount < Me.singleBlinkAmount Then
			Me.singleCount += 1
		Else
			If Me.blinkLayer2 IsNot Nothing Then
				If Me.is2Player Then
					Me.blinkLayer2.enabled = True
				Else
					Me.blinkLayer.enabled = True
				End If
				Me.is2Player = Not Me.is2Player
			Else
				Me.blinkLayer.enabled = True
			End If
			Me.singleCount = 0
			Me.singleBlinkAmount = Global.UnityEngine.Random.Range(0, 4) + 4
		End If
		If Me.blinkLayer2 IsNot Nothing Then
			If Me.doubleCount < Me.doubleBlinkAmount Then
				Me.doubleCount += 1
			Else
				Me.blinkLayer2.enabled = True
				Me.blinkLayer.enabled = True
				Me.doubleCount = 0
				Me.doubleBlinkAmount = Global.UnityEngine.Random.Range(0, 10) + 16
			End If
		End If
	End Sub

	' Token: 0x04004B92 RID: 19346
	<SerializeField()>
	Private results As Animator

	' Token: 0x04004B93 RID: 19347
	<SerializeField()>
	Private blinkLayer As SpriteRenderer

	' Token: 0x04004B94 RID: 19348
	<SerializeField()>
	Private blinkLayer2 As SpriteRenderer

	' Token: 0x04004B95 RID: 19349
	Public is2Player As Boolean

	' Token: 0x04004B96 RID: 19350
	Private singleBlinkAmount As Integer

	' Token: 0x04004B97 RID: 19351
	Private doubleBlinkAmount As Integer

	' Token: 0x04004B98 RID: 19352
	Private singleCount As Integer

	' Token: 0x04004B99 RID: 19353
	Private doubleCount As Integer

	' Token: 0x04004B9A RID: 19354
	Private p1Turn As Boolean
End Class
