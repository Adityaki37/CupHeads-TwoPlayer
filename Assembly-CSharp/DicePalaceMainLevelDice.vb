Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020005D2 RID: 1490
Public Class DicePalaceMainLevelDice
	Inherits ParrySwitch

	' Token: 0x06001D50 RID: 7504 RVA: 0x0010CCB8 File Offset: 0x0010B0B8
	Public Sub Init(pos As Vector2, properties As LevelProperties.DicePalaceMain.Dice, pivotPoint As Transform)
		MyBase.transform.position = pos
		Me.pivotPoint = pivotPoint
		Me.properties = properties
		MyBase.GetComponent(Of Collider2D)().enabled = False
		Me.waitingToRoll = True
		MyBase.StartCoroutine(Me.move_cr())
	End Sub

	' Token: 0x06001D51 RID: 7505 RVA: 0x0010CD04 File Offset: 0x0010B104
	Public Sub StartRoll()
		MyBase.animator.SetTrigger("StartRoll")
		Me.waitingToRoll = True
		MyBase.animator.SetBool("Reverse", Rand.Bool())
		MyBase.GetComponent(Of Collider2D)().enabled = True
	End Sub

	' Token: 0x06001D52 RID: 7506 RVA: 0x0010CD3E File Offset: 0x0010B13E
	Public Sub RollOne()
		Me.roll = DicePalaceMainLevelDice.Roll.One
		Me.PostRoll()
	End Sub

	' Token: 0x06001D53 RID: 7507 RVA: 0x0010CD4D File Offset: 0x0010B14D
	Public Sub RollTwo()
		Me.roll = DicePalaceMainLevelDice.Roll.Two
		Me.PostRoll()
	End Sub

	' Token: 0x06001D54 RID: 7508 RVA: 0x0010CD5C File Offset: 0x0010B15C
	Public Sub RollThree()
		Me.roll = DicePalaceMainLevelDice.Roll.Three
		Me.PostRoll()
	End Sub

	' Token: 0x06001D55 RID: 7509 RVA: 0x0010CD6B File Offset: 0x0010B16B
	Private Sub PostRoll()
		Me.waitingToRoll = False
		DicePalaceMainLevelGameInfo.TURN_COUNTER += 1
	End Sub

	' Token: 0x06001D56 RID: 7510 RVA: 0x0010CD80 File Offset: 0x0010B180
	Private Iterator Function move_cr() As IEnumerator
		Dim loopSize As Single = 20F
		Dim speed As Single = Me.properties.movementSpeed
		Dim angle As Single = 0F
		While True
			Dim pivotOffset As Vector3 = Vector3.left * 2F * loopSize
			angle += speed * CupheadTime.Delta
			If angle > 6.2831855F Then
				Me.reverse = Not Me.reverse
				angle -= 6.2831855F
			End If
			If angle < 0F Then
				angle += 6.2831855F
			End If
			Dim value As Single
			If Me.reverse Then
				MyBase.transform.position = Me.pivotPoint.position + pivotOffset
				value = 1F
			Else
				MyBase.transform.position = Me.pivotPoint.position
				value = -1F
			End If
			Dim handleRotationX As Vector3 = New Vector3(Mathf.Cos(angle) * value * loopSize, 0F, 0F)
			Dim handleRotationY As Vector3 = New Vector3(0F, Mathf.Sin(angle) * loopSize, 0F)
			MyBase.transform.position += handleRotationX + handleRotationY
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06001D57 RID: 7511 RVA: 0x0010CD9B File Offset: 0x0010B19B
	Public Overrides Sub OnParryPostPause(player As AbstractPlayerController)
		MyBase.OnParryPostPause(player)
		MyBase.animator.SetTrigger("Hit")
		MyBase.GetComponent(Of Collider2D)().enabled = False
	End Sub

	' Token: 0x04002631 RID: 9777
	Public roll As DicePalaceMainLevelDice.Roll

	' Token: 0x04002632 RID: 9778
	Public waitingToRoll As Boolean

	' Token: 0x04002633 RID: 9779
	Private properties As LevelProperties.DicePalaceMain.Dice

	' Token: 0x04002634 RID: 9780
	Private pivotPoint As Transform

	' Token: 0x04002635 RID: 9781
	Private reverse As Boolean

	' Token: 0x020005D3 RID: 1491
	Public Enum Roll
		' Token: 0x04002637 RID: 9783
		One
		' Token: 0x04002638 RID: 9784
		Two
		' Token: 0x04002639 RID: 9785
		Three
	End Enum
End Class
