Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020005F5 RID: 1525
Public Class DragonLevelMeteor
	Inherits AbstractProjectile

	' Token: 0x06001E60 RID: 7776 RVA: 0x0011849C File Offset: 0x0011689C
	Public Function Create(pos As Vector2, properties As DragonLevelMeteor.Properties) As DragonLevelMeteor
		Dim dragonLevelMeteor As DragonLevelMeteor = TryCast(MyBase.Create(), DragonLevelMeteor)
		dragonLevelMeteor.properties = properties
		dragonLevelMeteor.transform.position = pos
		Return dragonLevelMeteor
	End Function

	' Token: 0x06001E61 RID: 7777 RVA: 0x001184D0 File Offset: 0x001168D0
	Protected Overrides Sub Start()
		MyBase.Start()
		MyBase.StartCoroutine(Me.smoke_cr())
		MyBase.StartCoroutine(Me.moveX_cr())
		If Me.properties.state <> DragonLevelMeteor.State.Forward Then
			MyBase.StartCoroutine(Me.moveY_cr())
		End If
		MyBase.StartCoroutine(Me.rotate_cr())
		AudioManager.PlayLoop("level_dragon_left_dragon_meteor_a_loop")
		Me.emitAudioFromObject.Add("level_dragon_left_dragon_meteor_a_loop")
	End Sub

	' Token: 0x06001E62 RID: 7778 RVA: 0x00118543 File Offset: 0x00116943
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x06001E63 RID: 7779 RVA: 0x00118564 File Offset: 0x00116964
	Private Iterator Function rotate_cr() As IEnumerator
		Dim lastPos As Vector2 = MyBase.transform.position
		While True
			MyBase.transform.LookAt2D(lastPos)
			lastPos = MyBase.transform.position
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06001E64 RID: 7780 RVA: 0x00118580 File Offset: 0x00116980
	Private Iterator Function smoke_cr() As IEnumerator
		While True
			Yield CupheadTime.WaitForSeconds(Me, 0.1F)
			Me.smokePrefab.Create(MyBase.transform.position).transform.SetEulerAngles(New Single?(0F), New Single?(0F), New Single?(MyBase.transform.eulerAngles.z + Global.UnityEngine.Random.Range(-45F, 45F)))
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06001E65 RID: 7781 RVA: 0x0011859C File Offset: 0x0011699C
	Private Iterator Function moveX_cr() As IEnumerator
		Dim wait As YieldInstruction = New WaitForFixedUpdate()
		While MyBase.transform.position.x > -840F
			MyBase.transform.AddPosition(-Me.properties.speedX * CupheadTime.FixedDelta, 0F, 0F)
			Yield wait
		End While
		Me.Die()
		Return
	End Function

	' Token: 0x06001E66 RID: 7782 RVA: 0x001185B8 File Offset: 0x001169B8
	Private Iterator Function moveY_cr() As IEnumerator
		Dim state As Integer = CInt(Me.properties.state)
		Dim start As Vector2 = MyBase.transform.position
		Dim [end] As Vector2 = New Vector2(start.x - Me.properties.speedX / 2F, 300F * CSng(state))
		Yield MyBase.TweenPositionY(start.y, [end].y, Me.properties.timeY / 2F, EaseUtils.EaseType.easeOutSine)
		While True
			state *= -1
			start = MyBase.transform.position
			[end] = New Vector2(start.x - Me.properties.speedX, 300F * CSng(state))
			Yield MyBase.TweenPositionY(start.y, [end].y, Me.properties.timeY, EaseUtils.EaseType.easeInOutSine)
		End While
		Return
	End Function

	' Token: 0x06001E67 RID: 7783 RVA: 0x001185D3 File Offset: 0x001169D3
	Protected Overrides Sub Die()
		MyBase.Die()
		Me.StopAllCoroutines()
		AudioManager.[Stop]("level_dragon_left_dragon_meteor_a_loop")
	End Sub

	' Token: 0x06001E68 RID: 7784 RVA: 0x001185EB File Offset: 0x001169EB
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me.smokePrefab = Nothing
	End Sub

	' Token: 0x0400273E RID: 10046
	Private properties As DragonLevelMeteor.Properties

	' Token: 0x0400273F RID: 10047
	<SerializeField()>
	Private smokePrefab As Effect

	' Token: 0x020005F6 RID: 1526
	Public Enum State
		' Token: 0x04002741 RID: 10049
		Up = 1
		' Token: 0x04002742 RID: 10050
		Down = -1
		' Token: 0x04002743 RID: 10051
		Both
		' Token: 0x04002744 RID: 10052
		Forward = 10
	End Enum

	' Token: 0x020005F7 RID: 1527
	Public Class Properties
		' Token: 0x06001E69 RID: 7785 RVA: 0x001185FA File Offset: 0x001169FA
		Public Sub New(timeY As Single, speedX As Single, state As DragonLevelMeteor.State)
			Me.timeY = timeY
			Me.speedX = speedX
			Me.state = state
		End Sub

		' Token: 0x04002745 RID: 10053
		Public timeY As Single

		' Token: 0x04002746 RID: 10054
		Public speedX As Single

		' Token: 0x04002747 RID: 10055
		Public state As DragonLevelMeteor.State
	End Class
End Class
