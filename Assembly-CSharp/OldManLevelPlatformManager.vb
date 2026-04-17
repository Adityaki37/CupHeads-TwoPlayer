Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports UnityEngine

' Token: 0x02000709 RID: 1801
Public Class OldManLevelPlatformManager
	Inherits LevelProperties.OldMan.Entity

	' Token: 0x060026D5 RID: 9941 RVA: 0x0016B588 File Offset: 0x00169988
	Public Function GetPlatformPositions() As Vector3()
		Dim array As Vector3() = New Vector3(Me.allPlatforms.Length - 1) {}
		For i As Integer = 0 To Me.allPlatforms.Length - 1
			array(i) = Me.allPlatforms(i).platform.position
		Next
		Return array
	End Function

	' Token: 0x060026D6 RID: 9942 RVA: 0x0016B5DB File Offset: 0x001699DB
	Public Function GetPlatform(i As Integer) As Transform
		If Me.allPlatforms(i) Is Nothing Then
			Return Nothing
		End If
		Return Me.allPlatforms(i).platform.transform
	End Function

	' Token: 0x060026D7 RID: 9943 RVA: 0x0016B5FE File Offset: 0x001699FE
	Public Function PlatformRemoved(which As Integer) As Boolean
		Return Me.allPlatforms(which).removed
	End Function

	' Token: 0x060026D8 RID: 9944 RVA: 0x0016B610 File Offset: 0x00169A10
	Public Overrides Sub LevelInit(properties As LevelProperties.OldMan)
		MyBase.LevelInit(properties)
		For i As Integer = 0 To Me.allPlatforms.Length - 1
			Me.allPlatforms(i).platform.SetPosition(Nothing, New Single?(properties.CurrentState.platforms.minHeight), Nothing)
		Next
		MyBase.StartCoroutine(Me.handle_platforms_cr())
		MyBase.StartCoroutine(Me.handle_remove_platforms_cr())
	End Sub

	' Token: 0x060026D9 RID: 9945 RVA: 0x0016B690 File Offset: 0x00169A90
	Public Sub EndPhase()
		Me.inPhaseOne = False
		Me.mainBeardTufts.enabled = False
		Me.beardSettles(0).transform.parent.gameObject.SetActive(True)
	End Sub

	' Token: 0x060026DA RID: 9946 RVA: 0x0016B6C4 File Offset: 0x00169AC4
	Private Iterator Function handle_remove_platforms_cr() As IEnumerator
		Dim bossHealthMax As Single = MyBase.properties.CurrentHealth
		Dim bossHealthMin As Single = bossHealthMax * MyBase.properties.GetNextStateHealthTrigger()
		Dim currentCount As Integer = 0
		Dim removeOrder As String() = MyBase.properties.CurrentState.platforms.removeOrder(Global.UnityEngine.Random.Range(0, MyBase.properties.CurrentState.platforms.removeOrder.Length)).Split(New Char() { ","c })
		Dim removeThreshold As String() = MyBase.properties.CurrentState.platforms.removeThreshold.Split(New Char() { ","c })
		If removeOrder.Length <> removeThreshold.Length Then
			Global.Debug.Break()
		End If
		If removeOrder.Length = 0 Then
			Return
		End If
		For i As Integer = 0 To removeOrder.Length - 1
			Dim num As Integer = 0
			Parser.IntTryParse(removeOrder(i), num)
			Dim num2 As Single = 0F
			Parser.FloatTryParse(removeThreshold(i), num2)
			Me.removeOrderList.Add(num)
			Me.removeThresholdList.Add(num2)
		Next
		While currentCount < Me.removeThresholdList.Count
			Dim t As Single = Mathf.InverseLerp(bossHealthMax, bossHealthMin, MyBase.properties.CurrentHealth)
			If t > Me.removeThresholdList(currentCount) Then
				Me.RemovePlatform(Me.removeOrderList(currentCount))
				currentCount += 1
			End If
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x060026DB RID: 9947 RVA: 0x0016B6E0 File Offset: 0x00169AE0
	Private Iterator Function handle_platforms_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 3F)
		Dim p As LevelProperties.OldMan.Platforms = MyBase.properties.CurrentState.platforms
		Dim orderMainIndex As Integer = Global.UnityEngine.Random.Range(0, p.moveOrder.Length)
		Dim orderString As String() = p.moveOrder(orderMainIndex).Split(New Char() { ","c })
		Dim orderIndex As Integer = Global.UnityEngine.Random.Range(0, orderString.Length)
		Dim skipPlatform As Boolean = False
		Dim stoppedMoving As Boolean = False
		While Not stoppedMoving
			If Not Me.inPhaseOne Then
				stoppedMoving = True
				Yield Nothing
			End If
			skipPlatform = False
			orderString = p.moveOrder(orderMainIndex).Split(New Char() { ","c })
			Dim spawnOrder As String() = orderString(orderIndex).Split(New Char() { "-"c })
			For Each text As String In spawnOrder
				Dim num As Integer = 0
				Parser.IntTryParse(text, num)
				If Me.allPlatforms(num).isMoving Then
					skipPlatform = True
				Else
					MyBase.StartCoroutine(Me.move_platform_cr(Me.allPlatforms(num)))
				End If
			Next
			If Not skipPlatform Then
				Yield CupheadTime.WaitForSeconds(Me, p.delayRange.RandomFloat())
			End If
			If orderIndex < orderString.Length - 1 Then
				orderIndex += 1
			Else
				orderMainIndex = (orderMainIndex + 1) Mod p.moveOrder.Length
				orderIndex = 0
			End If
			Yield Nothing
		End While
		MyBase.StartCoroutine(Me.end_phase_cr())
		Return
	End Function

	' Token: 0x060026DC RID: 9948 RVA: 0x0016B6FC File Offset: 0x00169AFC
	Private Iterator Function end_phase_cr() As IEnumerator
		Dim order As List(Of Integer) = New List(Of Integer)(5)
		For j As Integer = 0 To 5 - 1
			If Not Me.allPlatforms(j).removed Then
				order.Add(j)
			End If
		Next
		For k As Integer = 0 To order.Count - 1
			Dim num As Integer = order(k)
			Dim num2 As Integer = Global.UnityEngine.Random.Range(0, order.Count)
			order(k) = order(num2)
			order(num2) = num
		Next
		For i As Integer = 0 To order.Count - 1
			MyBase.StartCoroutine(Me.slide_out_cr(Me.allPlatforms(order(i))))
			Yield CupheadTime.WaitForSeconds(Me, 0.1F)
		Next
		Return
	End Function

	' Token: 0x060026DD RID: 9949 RVA: 0x0016B718 File Offset: 0x00169B18
	Public Sub RemovePlatform(which As Integer)
		Me.allPlatforms(which).removed = True
		Me.mainBeardTufts.enabled = False
		Me.beardSettles(0).transform.parent.gameObject.SetActive(True)
		MyBase.StartCoroutine(Me.slide_out_cr(Me.allPlatforms(which)))
	End Sub

	' Token: 0x060026DE RID: 9950 RVA: 0x0016B771 File Offset: 0x00169B71
	Public Sub AttachGnome(which As Integer, c As OldManLevelGnomeClimber)
		Me.allPlatforms(which).activeClimber = c
	End Sub

	' Token: 0x060026DF RID: 9951 RVA: 0x0016B784 File Offset: 0x00169B84
	Private Iterator Function move_platform_cr(movingPlatform As OldManLevelPlatform) As IEnumerator
		Dim p As LevelProperties.OldMan.Platforms = MyBase.properties.CurrentState.platforms
		Dim t As Single = 0F
		Dim time As Single = p.moveTime / 2F
		movingPlatform.isMoving = True
		While t < time AndAlso Me.inPhaseOne AndAlso Not movingPlatform.removed
			t += CupheadTime.Delta
			Dim val As Single = EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, 0F, 1F, t / time)
			Dim lastPos As Single = movingPlatform.platform.transform.position.y
			movingPlatform.platform.SetPosition(Nothing, New Single?(Mathf.Lerp(p.minHeight, p.maxHeight, val)), Nothing)
			movingPlatform.effectiveVel = movingPlatform.platform.transform.position.y - lastPos
			Yield Nothing
		End While
		If Me.inPhaseOne AndAlso Not movingPlatform.removed Then
			t = 0F
			movingPlatform.platform.SetPosition(Nothing, New Single?(p.maxHeight), Nothing)
		End If
		While t < time AndAlso Me.inPhaseOne AndAlso Not movingPlatform.removed
			t += CupheadTime.Delta
			Dim val2 As Single = EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, 0F, 1F, t / time)
			Dim lastPos2 As Single = movingPlatform.platform.transform.position.y
			movingPlatform.platform.SetPosition(Nothing, New Single?(Mathf.Lerp(p.maxHeight, p.minHeight, val2)), Nothing)
			movingPlatform.effectiveVel = movingPlatform.platform.transform.position.y - lastPos2
			Yield Nothing
		End While
		If Me.inPhaseOne AndAlso Not movingPlatform.removed Then
			movingPlatform.platform.SetPosition(Nothing, New Single?(p.minHeight), Nothing)
			movingPlatform.isMoving = False
		End If
		Yield Nothing
		Return
	End Function

	' Token: 0x060026E0 RID: 9952 RVA: 0x0016B7A8 File Offset: 0x00169BA8
	Private Iterator Function slide_out_cr(movingPlatform As OldManLevelPlatform) As IEnumerator
		Dim p As LevelProperties.OldMan.Platforms = MyBase.properties.CurrentState.platforms
		Dim id As Integer = 4 - Array.IndexOf(Of OldManLevelPlatform)(Me.allPlatforms, movingPlatform)
		Dim wait As YieldInstruction = New WaitForFixedUpdate()
		Dim moveHeight As Single = p.minHeight * 2F - 50F
		If movingPlatform.effectiveVel > 0F Then
			movingPlatform.effectiveVel *= 0.5F
		End If
		Dim t As Single = Me.wobbleBeforeRemoveTime
		While t > 0F OrElse movingPlatform.activeClimber
			t -= CupheadTime.Delta
			movingPlatform.platform.transform.GetChild(0).transform.GetChild(0).localPosition = New Vector3(Mathf.Sin(t * 100F) * 2.5F, 0F)
			Yield Nothing
		End While
		movingPlatform.platform.transform.GetChild(0).transform.GetChild(0).localPosition = Vector3.zero
		While movingPlatform.platform.transform.position.y > moveHeight
			If Not CupheadTime.IsPaused() Then
				movingPlatform.platform.SetPosition(Nothing, New Single?(Mathf.Clamp(movingPlatform.platform.transform.position.y + movingPlatform.effectiveVel, -1000F, 117F)), Nothing)
			End If
			movingPlatform.effectiveVel -= 10F * CupheadTime.FixedDelta
			If movingPlatform.platform.transform.position.y < -384F Then
				Me.beardSettles(id).Play("Settle")
			End If
			Yield wait
		End While
		If movingPlatform.platform.GetComponentInChildren(Of LevelPlayerController)() Then
			Dim componentsInChildren As LevelPlayerController() = movingPlatform.platform.GetComponentsInChildren(Of LevelPlayerController)()
			For Each levelPlayerController As LevelPlayerController In componentsInChildren
				levelPlayerController.transform.parent = Nothing
			Next
		End If
		Global.UnityEngine.[Object].Destroy(movingPlatform.platform.gameObject)
		Yield Nothing
		Return
	End Function

	' Token: 0x04002F81 RID: 12161
	Private Const PLATFORM_EXIT_SPEED As Single = 10F

	' Token: 0x04002F82 RID: 12162
	<SerializeField()>
	Private allPlatforms As OldManLevelPlatform()

	' Token: 0x04002F83 RID: 12163
	<SerializeField()>
	Private beardSettles As Animator()

	' Token: 0x04002F84 RID: 12164
	<SerializeField()>
	Private mainBeardTufts As SpriteRenderer

	' Token: 0x04002F85 RID: 12165
	<SerializeField()>
	Private wobbleBeforeRemoveTime As Single = 1F

	' Token: 0x04002F86 RID: 12166
	Private inPhaseOne As Boolean = True

	' Token: 0x04002F87 RID: 12167
	Private lastPos As Single

	' Token: 0x04002F88 RID: 12168
	Private removeOrderList As List(Of Integer) = New List(Of Integer)()

	' Token: 0x04002F89 RID: 12169
	Private removeThresholdList As List(Of Single) = New List(Of Single)()
End Class
