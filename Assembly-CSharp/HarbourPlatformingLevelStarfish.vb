Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020008D1 RID: 2257
Public Class HarbourPlatformingLevelStarfish
	Inherits AbstractPlatformingLevelEnemy

	' Token: 0x060034D4 RID: 13524 RVA: 0x001EB780 File Offset: 0x001E9B80
	Public Sub Init(rotation As Single, speedX As Single, speedY As Single, loopSize As Single, type As String)
		MyBase.transform.SetEulerAngles(Nothing, Nothing, New Single?(rotation - 90F))
		Me.figureEightSpeed = speedX
		Me.movementSpeed = speedY
		Me.loopSize = loopSize
		Me.type = type
	End Sub

	' Token: 0x060034D5 RID: 13525 RVA: 0x001EB7D4 File Offset: 0x001E9BD4
	Protected Overrides Sub OnStart()
	End Sub

	' Token: 0x060034D6 RID: 13526 RVA: 0x001EB7D8 File Offset: 0x001E9BD8
	Protected Overrides Sub Start()
		MyBase.Start()
		Me.pivotOffset = Vector3.up * 2F * Me.loopSize
		Me.pivotPoint = New GameObject("PivotPoint")
		Me.pivotPoint.transform.position = MyBase.transform.position
		If Me.type = "A" Then
			Me._canParry = True
			Me.bubbles = Me.pinkBubbles
		Else
			Me.bubbles = Me.normalBubbles
		End If
		MyBase.GetComponent(Of PlatformingLevelEnemyAnimationHandler)().SelectAnimation(Me.type)
		MyBase.StartCoroutine(Me.spawn_bubbles_cr())
		MyBase.StartCoroutine(Me.move_cr())
		MyBase.StartCoroutine(Me.figure_eight_cr())
	End Sub

	' Token: 0x060034D7 RID: 13527 RVA: 0x001EB8A8 File Offset: 0x001E9CA8
	Private Iterator Function move_cr() As IEnumerator
		While True
			Me.pivotPoint.transform.position += MyBase.transform.up * Me.movementSpeed * CupheadTime.FixedDelta
			If MyBase.transform.position.y > CupheadLevelCamera.Current.Bounds.yMax + 200F Then
				Exit For
			End If
			Yield Nothing
		End While
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
		Yield Nothing
		Return
	End Function

	' Token: 0x060034D8 RID: 13528 RVA: 0x001EB8C4 File Offset: 0x001E9CC4
	Private Iterator Function figure_eight_cr() As IEnumerator
		Dim invert As Boolean = False
		While True
			Me.angle += Me.figureEightSpeed * CupheadTime.Delta
			If Me.angle > 6.2831855F Then
				invert = Not invert
				Me.angle -= 6.2831855F
			End If
			If Me.angle < 0F Then
				Me.angle += 6.2831855F
			End If
			Dim value As Single
			If invert Then
				MyBase.transform.position = Me.pivotPoint.transform.position + Me.pivotOffset
				value = -1F
			Else
				MyBase.transform.position = Me.pivotPoint.transform.position
				value = 1F
			End If
			Dim handleRotation As Vector3 = New Vector3(-Mathf.Sin(Me.angle) * Me.loopSize, Mathf.Cos(Me.angle) * value * Me.loopSize, 0F)
			MyBase.transform.position += handleRotation
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x060034D9 RID: 13529 RVA: 0x001EB8E0 File Offset: 0x001E9CE0
	Private Iterator Function spawn_bubbles_cr() As IEnumerator
		Dim bubbleTypes As String = "A,B,C,D"
		While True
			Dim bubbleType As String = bubbleTypes.Split(New Char() { ","c })(Global.UnityEngine.Random.Range(0, bubbleTypes.Split(New Char() { ","c }).Length))
			Me.bubbles.Create(MyBase.transform.position).GetComponent(Of PlatformingLevelEnemyAnimationHandler)().SelectAnimation(bubbleType)
			Yield CupheadTime.WaitForSeconds(Me, 1F)
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x060034DA RID: 13530 RVA: 0x001EB8FB File Offset: 0x001E9CFB
	Protected Overrides Sub Die()
		AudioManager.Play("harbour_star_death")
		Me.emitAudioFromObject.Add("harbour_star_death")
		MyBase.Die()
	End Sub

	' Token: 0x060034DB RID: 13531 RVA: 0x001EB91D File Offset: 0x001E9D1D
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Global.UnityEngine.[Object].Destroy(Me.pivotPoint.gameObject)
	End Sub

	' Token: 0x04003CFC RID: 15612
	<SerializeField()>
	Private normalBubbles As Effect

	' Token: 0x04003CFD RID: 15613
	<SerializeField()>
	Private pinkBubbles As Effect

	' Token: 0x04003CFE RID: 15614
	Private pivotPoint As GameObject

	' Token: 0x04003CFF RID: 15615
	Private angle As Single

	' Token: 0x04003D00 RID: 15616
	Private figureEightSpeed As Single

	' Token: 0x04003D01 RID: 15617
	Private movementSpeed As Single

	' Token: 0x04003D02 RID: 15618
	Private loopSize As Single

	' Token: 0x04003D03 RID: 15619
	Private type As String

	' Token: 0x04003D04 RID: 15620
	Private pivotOffset As Vector3

	' Token: 0x04003D05 RID: 15621
	Private bubbles As Effect
End Class
