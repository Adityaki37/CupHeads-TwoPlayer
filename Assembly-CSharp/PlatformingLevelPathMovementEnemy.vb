Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x0200086A RID: 2154
Public Class PlatformingLevelPathMovementEnemy
	Inherits AbstractPlatformingLevelEnemy

	' Token: 0x17000438 RID: 1080
	' (get) Token: 0x0600320B RID: 12811 RVA: 0x001D34D9 File Offset: 0x001D18D9
	' (set) Token: 0x0600320C RID: 12812 RVA: 0x001D34E1 File Offset: 0x001D18E1
	Protected Private Property allValues As Single()

	' Token: 0x0600320D RID: 12813 RVA: 0x001D34EC File Offset: 0x001D18EC
	Public Function Spawn(position As Vector3, path As VectorPath, startPosition As Single, destroyEnemyAfterLeavingScreen As Boolean) As PlatformingLevelPathMovementEnemy
		Dim platformingLevelPathMovementEnemy As PlatformingLevelPathMovementEnemy = Me.InstantiatePrefab(Of PlatformingLevelPathMovementEnemy)()
		platformingLevelPathMovementEnemy.transform.position = position
		platformingLevelPathMovementEnemy.startPosition = startPosition
		platformingLevelPathMovementEnemy.path = path
		platformingLevelPathMovementEnemy._destroyEnemyAfterLeavingScreen = destroyEnemyAfterLeavingScreen
		platformingLevelPathMovementEnemy._startCondition = AbstractPlatformingLevelEnemy.StartCondition.Instant
		Return platformingLevelPathMovementEnemy
	End Function

	' Token: 0x17000439 RID: 1081
	' (get) Token: 0x0600320E RID: 12814 RVA: 0x001D352A File Offset: 0x001D192A
	Protected Overridable ReadOnly Property spriteRenderer As SpriteRenderer
		Get
			Return Me._spriteRenderer
		End Get
	End Property

	' Token: 0x1700043A RID: 1082
	' (get) Token: 0x0600320F RID: 12815 RVA: 0x001D3532 File Offset: 0x001D1932
	Protected Overridable ReadOnly Property collider As Collider2D
		Get
			Return Me._collider
		End Get
	End Property

	' Token: 0x06003210 RID: 12816 RVA: 0x001D353C File Offset: 0x001D193C
	Protected Overrides Sub Start()
		MyBase.Start()
		Me._offset = MyBase.transform.position
		Me.MoveCallback(Me.startPosition)
		Me._collider = MyBase.GetComponent(Of Collider2D)()
		Me._spriteRenderer = MyBase.GetComponent(Of SpriteRenderer)()
		If MyBase.Properties.MoveLoopMode = EnemyProperties.LoopMode.DelayAtPoint Then
			Me.SetUp()
		End If
	End Sub

	' Token: 0x06003211 RID: 12817 RVA: 0x001D359C File Offset: 0x001D199C
	Protected Overrides Sub OnStart()
		Me.hasStarted = True
		Select Case MyBase.Properties.MoveLoopMode
			Case EnemyProperties.LoopMode.PingPong
				MyBase.StartCoroutine(Me.pingpong_cr())
			Case EnemyProperties.LoopMode.Repeat
				MyBase.StartCoroutine(Me.repeat_cr())
			Case EnemyProperties.LoopMode.Once
				MyBase.StartCoroutine(Me.once_cr())
			Case EnemyProperties.LoopMode.DelayAtPoint
				MyBase.StartCoroutine(Me.delay_at_point_cr())
		End Select
	End Sub

	' Token: 0x06003212 RID: 12818 RVA: 0x001D3624 File Offset: 0x001D1A24
	Protected Overrides Sub Update()
		MyBase.Update()
		Me.CalculateCollider()
		Me.CalculateDirection()
		Me.CalculateRender()
	End Sub

	' Token: 0x06003213 RID: 12819 RVA: 0x001D3640 File Offset: 0x001D1A40
	Private Sub CalculateRender()
		If CupheadLevelCamera.Current.ContainsPoint(MyBase.transform.position) AndAlso Not Me._enteredScreen Then
			Me._enteredScreen = True
		End If
		If Me._enteredScreen AndAlso Me._destroyEnemyAfterLeavingScreen AndAlso Not CupheadLevelCamera.Current.ContainsPoint(MyBase.transform.position, New Vector2(100F, 100F)) Then
			Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
		End If
	End Sub

	' Token: 0x06003214 RID: 12820 RVA: 0x001D36CD File Offset: 0x001D1ACD
	Private Sub LateUpdate()
		Me.CalculateCollider()
		Me.CalculateDirection()
	End Sub

	' Token: 0x06003215 RID: 12821 RVA: 0x001D36DC File Offset: 0x001D1ADC
	Protected Overridable Sub CalculateCollider()
		If Me.collider Is Nothing OrElse Me.spriteRenderer Is Nothing OrElse MyBase.Dead Then
			Return
		End If
		If Me.spriteRenderer.isVisible Then
			Me.collider.enabled = True
		Else
			Me.collider.enabled = False
		End If
	End Sub

	' Token: 0x06003216 RID: 12822 RVA: 0x001D3744 File Offset: 0x001D1B44
	Private Sub CalculateDirection()
		If Me._direction = PlatformingLevelPathMovementEnemy.Direction.Forward AndAlso Me._hasFacingDirection Then
			Me.spriteRenderer.flipX = True
		Else
			Me.spriteRenderer.flipX = False
		End If
	End Sub

	' Token: 0x06003217 RID: 12823 RVA: 0x001D377A File Offset: 0x001D1B7A
	Private Sub MoveCallback(value As Single)
		MyBase.transform.position = Me._offset + Me.path.Lerp(value)
	End Sub

	' Token: 0x06003218 RID: 12824 RVA: 0x001D37A0 File Offset: 0x001D1BA0
	Private Function CalculateRemainingTime(t As Single, d As PlatformingLevelPathMovementEnemy.Direction) As Single
		Dim num As Single = Me.CalculateTime()
		Return If((d <> PlatformingLevelPathMovementEnemy.Direction.Forward), (t * num), ((1F - t) * num))
	End Function

	' Token: 0x06003219 RID: 12825 RVA: 0x001D37CC File Offset: 0x001D1BCC
	Private Function CalculateTime() As Single
		Return Me.path.Distance / MyBase.Properties.MoveSpeed
	End Function

	' Token: 0x0600321A RID: 12826 RVA: 0x001D37F4 File Offset: 0x001D1BF4
	Private Function CalculatePartTime(current As Integer, [next] As Integer) As Single
		Return Vector3.Distance(Me.path.Points(current), Me.path.Points([next])) / MyBase.Properties.MoveSpeed
	End Function

	' Token: 0x0600321B RID: 12827 RVA: 0x001D3836 File Offset: 0x001D1C36
	Private Function Turn() As Coroutine
		Return MyBase.StartCoroutine(Me.turn_cr())
	End Function

	' Token: 0x0600321C RID: 12828 RVA: 0x001D3844 File Offset: 0x001D1C44
	Private Iterator Function turn_cr() As IEnumerator
		If Me._hasTurnAnimation AndAlso MyBase.animator IsNot Nothing Then
			MyBase.animator.Play("Turn")
			Yield MyBase.animator.WaitForAnimationToEnd(Me, "Turn", False, True)
		End If
		If Me._direction = PlatformingLevelPathMovementEnemy.Direction.Forward Then
			Me._direction = PlatformingLevelPathMovementEnemy.Direction.Back
		Else
			Me._direction = PlatformingLevelPathMovementEnemy.Direction.Forward
		End If
		Return
	End Function

	' Token: 0x0600321D RID: 12829 RVA: 0x001D3860 File Offset: 0x001D1C60
	Private Iterator Function pingpong_cr() As IEnumerator
		If Me._direction = PlatformingLevelPathMovementEnemy.Direction.Back Then
			Yield MyBase.TweenValue(Me.startPosition, 0F, Me.CalculateRemainingTime(Me.startPosition, PlatformingLevelPathMovementEnemy.Direction.Back), Me._easeType, AddressOf Me.MoveCallback)
			Yield CupheadTime.WaitForSeconds(Me, Me.loopRepeatDelay)
			Yield Me.Turn()
		Else
			Yield MyBase.TweenValue(Me.startPosition, 1F, Me.CalculateRemainingTime(Me.startPosition, PlatformingLevelPathMovementEnemy.Direction.Forward), Me._easeType, AddressOf Me.MoveCallback)
			Yield CupheadTime.WaitForSeconds(Me, Me.loopRepeatDelay)
			Yield Me.Turn()
			Yield MyBase.TweenValue(1F, 0F, Me.CalculateTime(), Me._easeType, AddressOf Me.MoveCallback)
			Yield CupheadTime.WaitForSeconds(Me, Me.loopRepeatDelay)
			Yield Me.Turn()
		End If
		While True
			Yield MyBase.TweenValue(0F, 1F, Me.CalculateTime(), Me._easeType, AddressOf Me.MoveCallback)
			Yield CupheadTime.WaitForSeconds(Me, Me.loopRepeatDelay)
			Yield Me.Turn()
			Yield MyBase.TweenValue(1F, 0F, Me.CalculateTime(), Me._easeType, AddressOf Me.MoveCallback)
			Yield CupheadTime.WaitForSeconds(Me, Me.loopRepeatDelay)
			Yield Me.Turn()
		End While
		Return
	End Function

	' Token: 0x0600321E RID: 12830 RVA: 0x001D387C File Offset: 0x001D1C7C
	Private Iterator Function repeat_cr() As IEnumerator
		Dim start As Single = 0F
		Dim [end] As Single = 1F
		If Me._direction = PlatformingLevelPathMovementEnemy.Direction.Back Then
			start = 1F
			[end] = 0F
		End If
		Yield MyBase.TweenValue(Me.startPosition, [end], Me.CalculateRemainingTime(Me.startPosition, Me._direction), Me._easeType, AddressOf Me.MoveCallback)
		While True
			Yield MyBase.TweenValue(start, [end], Me.CalculateTime(), Me._easeType, AddressOf Me.MoveCallback)
			Yield CupheadTime.WaitForSeconds(Me, Me.loopRepeatDelay)
		End While
		Return
	End Function

	' Token: 0x0600321F RID: 12831 RVA: 0x001D3898 File Offset: 0x001D1C98
	Private Sub SetUp()
		Me._easeType = EaseUtils.EaseType.linear
		Dim num As Single = 0F
		Dim num2 As Single = 0F
		Dim num3 As Single = 0F
		Me.allValues = New Single(Me.path.Points.Count - 1) {}
		For i As Integer = 0 To Me.path.Points.Count - 1
			Dim num4 As Integer = If((i <> 0), (i - 1), 0)
			Dim num5 As Single = Me.path.Points(i).y - Me.path.Points(num4).y
			Dim num6 As Single = Me.path.Points(i).x - Me.path.Points(num4).x
			Dim num7 As Single = Mathf.Pow(num5, 2F)
			Dim num8 As Single = Mathf.Pow(num6, 2F)
			Dim num9 As Single = num7 + num8
			num3 += Mathf.Sqrt(num9)
		Next
		For j As Integer = 0 To Me.path.Points.Count - 1
			num2 += num
			Dim num10 As Integer = If((j <> 0), (j - 1), 0)
			Dim num11 As Single = Me.path.Points(j).y - Me.path.Points(num10).y
			Dim num12 As Single = Me.path.Points(j).x - Me.path.Points(num10).x
			Dim num13 As Single = Mathf.Pow(num11, 2F)
			Dim num14 As Single = Mathf.Pow(num12, 2F)
			Dim num15 As Single = num13 + num14
			num = Mathf.Sqrt(num15)
			Me.allValues(j) = (num2 + num) / num3
		Next
	End Sub

	' Token: 0x06003220 RID: 12832 RVA: 0x001D3A90 File Offset: 0x001D1E90
	Private Iterator Function delay_at_point_cr() As IEnumerator
		Dim prevVal As Single = Me.startPosition
		While Me.hasStarted
			Yield MyBase.TweenValue(prevVal, Me.allValues(Me.pathIndex), Me.CalculatePartTime(Me.pathIndex - 1, Me.pathIndex), Me._easeType, AddressOf Me.MoveCallback)
			Yield Nothing
			If Me._hasTurnAnimation Then
				MyBase.animator.SetTrigger("Turn")
				Yield MyBase.animator.WaitForAnimationToEnd(Me, "Turn", False, True)
			Else
				Yield CupheadTime.WaitForSeconds(Me, Me.loopRepeatDelay)
			End If
			Yield Nothing
			If Me.pathIndex = Me.path.Points.Count - 1 Then
				Exit While
			End If
			prevVal = Me.allValues(Me.pathIndex)
			Me.pathIndex += 1
			Yield Nothing
		End While
		Me.EndPath()
		Yield Nothing
		Return
	End Function

	' Token: 0x06003221 RID: 12833 RVA: 0x001D3AAB File Offset: 0x001D1EAB
	Protected Overridable Sub EndPath()
	End Sub

	' Token: 0x06003222 RID: 12834 RVA: 0x001D3AB0 File Offset: 0x001D1EB0
	Private Iterator Function once_cr() As IEnumerator
		If Me._direction = PlatformingLevelPathMovementEnemy.Direction.Back Then
			Yield MyBase.TweenValue(Me.startPosition, 0F, Me.CalculateRemainingTime(Me.startPosition, PlatformingLevelPathMovementEnemy.Direction.Back), Me._easeType, AddressOf Me.MoveCallback)
			Me.Die()
		Else
			Yield MyBase.TweenValue(Me.startPosition, 1F, Me.CalculateRemainingTime(Me.startPosition, PlatformingLevelPathMovementEnemy.Direction.Forward), Me._easeType, AddressOf Me.MoveCallback)
			Me.Die()
		End If
		Return
	End Function

	' Token: 0x06003223 RID: 12835 RVA: 0x001D3ACB File Offset: 0x001D1ECB
	Protected Overrides Sub OnDrawGizmos()
		MyBase.OnDrawGizmos()
		Me.DrawGizmos(0.2F)
	End Sub

	' Token: 0x06003224 RID: 12836 RVA: 0x001D3ADE File Offset: 0x001D1EDE
	Protected Overrides Sub OnDrawGizmosSelected()
		MyBase.OnDrawGizmosSelected()
		Me.DrawGizmos(1F)
	End Sub

	' Token: 0x06003225 RID: 12837 RVA: 0x001D3AF4 File Offset: 0x001D1EF4
	Private Sub DrawGizmos(a As Single)
		If Application.isPlaying Then
			Me.path.DrawGizmos(a, Me._offset)
			Return
		End If
		Me.path.DrawGizmos(a, MyBase.baseTransform.position)
		Gizmos.color = New Color(1F, 0F, 0F, a)
		Gizmos.DrawSphere(Me.path.Lerp(Me.startPosition) + MyBase.baseTransform.position, 10F)
		Gizmos.DrawWireSphere(Me.path.Lerp(Me.startPosition) + MyBase.baseTransform.position, 11F)
	End Sub

	' Token: 0x04003A6F RID: 14959
	Protected pathIndex As Integer

	' Token: 0x04003A70 RID: 14960
	Private Const SCREEN_PADDING As Single = 100F

	' Token: 0x04003A71 RID: 14961
	Public loopRepeatDelay As Single

	' Token: 0x04003A72 RID: 14962
	Public startPosition As Single = 0.5F

	' Token: 0x04003A73 RID: 14963
	Public path As VectorPath

	' Token: 0x04003A74 RID: 14964
	<SerializeField()>
	Protected _direction As PlatformingLevelPathMovementEnemy.Direction = PlatformingLevelPathMovementEnemy.Direction.Forward

	' Token: 0x04003A75 RID: 14965
	<SerializeField()>
	Private _hasTurnAnimation As Boolean

	' Token: 0x04003A76 RID: 14966
	<SerializeField()>
	Private _hasFacingDirection As Boolean

	' Token: 0x04003A77 RID: 14967
	<SerializeField()>
	Private _easeType As EaseUtils.EaseType = EaseUtils.EaseType.linear

	' Token: 0x04003A78 RID: 14968
	Protected _offset As Vector3

	' Token: 0x04003A79 RID: 14969
	Protected hasStarted As Boolean

	' Token: 0x04003A7A RID: 14970
	Private _spriteRenderer As SpriteRenderer

	' Token: 0x04003A7B RID: 14971
	Private _collider As Collider2D

	' Token: 0x04003A7C RID: 14972
	Private _destroyEnemyAfterLeavingScreen As Boolean

	' Token: 0x04003A7D RID: 14973
	Private _enteredScreen As Boolean

	' Token: 0x0200086B RID: 2155
	Public Enum Direction
		' Token: 0x04003A7F RID: 14975
		Forward = 1
		' Token: 0x04003A80 RID: 14976
		Back = -1
	End Enum
End Class
