Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000718 RID: 1816
Public Class PirateLevelBarrel
	Inherits LevelProperties.Pirate.Entity

	' Token: 0x06002782 RID: 10114 RVA: 0x00172AA4 File Offset: 0x00170EA4
	Public Overrides Sub LevelInit(properties As LevelProperties.Pirate)
		MyBase.LevelInit(properties)
		AddHandler Level.Current.OnStateChangedEvent, AddressOf Me.OnStateChanged
		AddHandler Level.Current.OnLevelStartEvent, AddressOf Me.OnLevelStart
		Me.damageDealer = New DamageDealer(MyBase.properties.CurrentState.barrel.damage, 1F)
		Me.damageDealer.SetDirection(DamageDealer.Direction.Neutral, MyBase.transform)
		Me.state = PirateLevelBarrel.State.Move
	End Sub

	' Token: 0x06002783 RID: 10115 RVA: 0x00172B22 File Offset: 0x00170F22
	Private Sub OnLevelStart()
		Me.moveCoroutine = Me.move_cr()
		MyBase.StartCoroutine(Me.moveCoroutine)
	End Sub

	' Token: 0x06002784 RID: 10116 RVA: 0x00172B40 File Offset: 0x00170F40
	Private Sub OnStateChanged()
		Me.damageDealer.SetDamage(MyBase.properties.CurrentState.barrel.damage)
		MyBase.StopCoroutine(Me.moveCoroutine)
		Me.moveCoroutine = Me.move_cr()
		MyBase.StartCoroutine(Me.moveCoroutine)
	End Sub

	' Token: 0x06002785 RID: 10117 RVA: 0x00172B94 File Offset: 0x00170F94
	Private Sub Update()
		Dim array As AbstractPlayerController() = New AbstractPlayerController() { PlayerManager.GetPlayer(PlayerId.PlayerOne), PlayerManager.GetPlayer(PlayerId.PlayerTwo) }
		If Me.state = PirateLevelBarrel.State.Move Then
			Dim num As Single = MyBase.transform.position.x - 60F
			Dim num2 As Single = MyBase.transform.position.x + 60F
			For Each abstractPlayerController As AbstractPlayerController In array
				If Not(abstractPlayerController Is Nothing) AndAlso Not(abstractPlayerController.transform Is Nothing) AndAlso Not abstractPlayerController.IsDead Then
					If abstractPlayerController.center.x > num AndAlso abstractPlayerController.center.x < num2 Then
						Me.PlayerFound()
						Exit For
					End If
				End If
			Next
		End If
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x06002786 RID: 10118 RVA: 0x00172C9B File Offset: 0x0017109B
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x06002787 RID: 10119 RVA: 0x00172CB9 File Offset: 0x001710B9
	Private Sub PlayerFound()
		Me.state = PirateLevelBarrel.State.Fall
		MyBase.StartCoroutine(Me.fall_cr())
	End Sub

	' Token: 0x06002788 RID: 10120 RVA: 0x00172CD0 File Offset: 0x001710D0
	Private Iterator Function move_cr() As IEnumerator
		Dim time As Single = MyBase.properties.CurrentState.barrel.moveTime
		Dim p As Single = (MyBase.transform.position.x - -570F) / 805F
		If Me.direction = PirateLevelBarrel.Direction.Left Then
			p = 1F - p
		End If
		Dim t As Single = time * p
		While True
			If Me.direction = PirateLevelBarrel.Direction.Right Then
				While t < time
					Yield MyBase.StartCoroutine(Me.waitForMove_cr())
					Dim val As Single = t / time
					Dim x As Single = EaseUtils.Ease(EaseUtils.EaseType.linear, -570F, 235F, val)
					MyBase.transform.SetPosition(New Single?(x), Nothing, Nothing)
					t += CupheadTime.Delta
					Yield Nothing
				End While
				t = 0F
				Me.direction = PirateLevelBarrel.Direction.Left
			End If
			If Me.direction = PirateLevelBarrel.Direction.Left Then
				While t < time
					Yield MyBase.StartCoroutine(Me.waitForMove_cr())
					Dim val2 As Single = t / time
					Dim x2 As Single = EaseUtils.Ease(EaseUtils.EaseType.linear, 235F, -570F, val2)
					MyBase.transform.SetPosition(New Single?(x2), Nothing, Nothing)
					t += CupheadTime.Delta
					Yield Nothing
				End While
				t = 0F
				Me.direction = PirateLevelBarrel.Direction.Right
			End If
		End While
		Return
	End Function

	' Token: 0x06002789 RID: 10121 RVA: 0x00172CEC File Offset: 0x001710EC
	Private Iterator Function waitForMove_cr() As IEnumerator
		While Me.state <> PirateLevelBarrel.State.Move AndAlso Me.state <> PirateLevelBarrel.State.Safe
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x0600278A RID: 10122 RVA: 0x00172D08 File Offset: 0x00171108
	Private Iterator Function fall_cr() As IEnumerator
		AudioManager.Play("level_pirate_barrel_drop_attack")
		Me.emitAudioFromObject.Add("level_pirate_barrel_drop_attack")
		MyBase.animator.SetTrigger("OnFall")
		Me.state = PirateLevelBarrel.State.Fall
		MyBase.GetComponent(Of Collider2D)().enabled = True
		Dim properties As LevelProperties.Pirate.State = MyBase.properties.CurrentState
		Dim t As Single = 0F
		Dim time As Single = properties.barrel.fallTime
		While t < time
			Dim val As Single = t / time
			Dim y As Single = EaseUtils.Ease(EaseUtils.EaseType.easeInQuart, 250F, -225F, val)
			MyBase.transform.SetPosition(Nothing, New Single?(y), Nothing)
			t += CupheadTime.Delta
			Yield Nothing
		End While
		MyBase.transform.SetPosition(Nothing, New Single?(-225F), Nothing)
		Me.dustEffect.Create(MyBase.transform.position)
		Me.particlesEffect.Create(MyBase.transform.position)
		MyBase.animator.SetTrigger("OnSmash")
		Me.state = PirateLevelBarrel.State.Hold
		CupheadLevelCamera.Current.Shake(8F, 0.6F, False)
		Yield CupheadTime.WaitForSeconds(Me, properties.barrel.groundHold)
		t = 0F
		time = properties.barrel.riseTime
		MyBase.animator.SetTrigger("OnUp")
		Me.state = PirateLevelBarrel.State.Up
		While t < time
			Dim val2 As Single = t / time
			Dim y2 As Single = EaseUtils.Ease(EaseUtils.EaseType.linear, -225F, 250F, val2)
			MyBase.transform.SetPosition(Nothing, New Single?(y2), Nothing)
			t += CupheadTime.Delta
			Yield Nothing
		End While
		MyBase.transform.SetPosition(Nothing, New Single?(250F), Nothing)
		MyBase.animator.SetTrigger("OnSafe")
		Me.state = PirateLevelBarrel.State.Safe
		Yield CupheadTime.WaitForSeconds(Me, properties.barrel.safeTime)
		MyBase.animator.SetTrigger("OnReady")
		Me.state = PirateLevelBarrel.State.Move
		Return
	End Function

	' Token: 0x0600278B RID: 10123 RVA: 0x00172D23 File Offset: 0x00171123
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me.dustEffect = Nothing
		Me.particlesEffect = Nothing
	End Sub

	' Token: 0x04003042 RID: 12354
	Public Const MIN_X As Single = -570F

	' Token: 0x04003043 RID: 12355
	Public Const MAX_X As Single = 235F

	' Token: 0x04003044 RID: 12356
	Public Const UP_Y As Single = 250F

	' Token: 0x04003045 RID: 12357
	Public Const DOWN_Y As Single = -225F

	' Token: 0x04003046 RID: 12358
	Public Const RANGE As Single = 120F

	' Token: 0x04003047 RID: 12359
	<SerializeField()>
	Private particlesEffect As Effect

	' Token: 0x04003048 RID: 12360
	<SerializeField()>
	Private dustEffect As Effect

	' Token: 0x04003049 RID: 12361
	Private state As PirateLevelBarrel.State

	' Token: 0x0400304A RID: 12362
	Private direction As PirateLevelBarrel.Direction = PirateLevelBarrel.Direction.Left

	' Token: 0x0400304B RID: 12363
	Private moveCoroutine As IEnumerator

	' Token: 0x0400304C RID: 12364
	Private damageDealer As DamageDealer

	' Token: 0x02000719 RID: 1817
	Public Enum State
		' Token: 0x0400304E RID: 12366
		Init
		' Token: 0x0400304F RID: 12367
		Move
		' Token: 0x04003050 RID: 12368
		Fall
		' Token: 0x04003051 RID: 12369
		Hold
		' Token: 0x04003052 RID: 12370
		Up
		' Token: 0x04003053 RID: 12371
		Safe
	End Enum

	' Token: 0x0200071A RID: 1818
	Public Enum Direction
		' Token: 0x04003055 RID: 12373
		Right
		' Token: 0x04003056 RID: 12374
		Left
	End Enum
End Class
