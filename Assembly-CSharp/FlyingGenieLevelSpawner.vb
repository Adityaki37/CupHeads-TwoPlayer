Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000679 RID: 1657
Public Class FlyingGenieLevelSpawner
	Inherits AbstractProjectile

	' Token: 0x1700039C RID: 924
	' (get) Token: 0x060022E8 RID: 8936 RVA: 0x00147A08 File Offset: 0x00145E08
	Protected Overrides ReadOnly Property DestroyLifetime As Single
		Get
			Return 0F
		End Get
	End Property

	' Token: 0x060022E9 RID: 8937 RVA: 0x00147A10 File Offset: 0x00145E10
	Public Function Create(pos As Vector2, player As AbstractPlayerController, properties As LevelProperties.FlyingGenie.Bullets) As FlyingGenieLevelSpawner
		Dim flyingGenieLevelSpawner As FlyingGenieLevelSpawner = TryCast(MyBase.Create(pos), FlyingGenieLevelSpawner)
		flyingGenieLevelSpawner.properties = properties
		flyingGenieLevelSpawner.player = player
		Return flyingGenieLevelSpawner
	End Function

	' Token: 0x060022EA RID: 8938 RVA: 0x00147A39 File Offset: 0x00145E39
	Protected Overrides Sub Start()
		MyBase.Start()
		Me.SetUpSpawnPoints()
		MyBase.StartCoroutine(Me.move_cr())
	End Sub

	' Token: 0x060022EB RID: 8939 RVA: 0x00147A54 File Offset: 0x00145E54
	Protected Overrides Sub Update()
		MyBase.Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
		For i As Integer = 0 To Me.points.Length - 1
			Me.points(i).transform.Rotate(0F, 0F, Me.properties.spawnerRotateSpeed * CupheadTime.Delta)
		Next
	End Sub

	' Token: 0x060022EC RID: 8940 RVA: 0x00147AC8 File Offset: 0x00145EC8
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x060022ED RID: 8941 RVA: 0x00147AE0 File Offset: 0x00145EE0
	Private Sub SetUpSpawnPoints()
		Dim num As Single = Global.UnityEngine.Random.Range(0F, 6.2831855F)
		Dim num2 As Single = 0F
		Dim x As Single = MyBase.transform.position.x
		Dim y As Single = MyBase.transform.position.y
		Me.points = New FlyingGenieLevelSpawnerPoint(Me.properties.spawnerCount - 1) {}
		For i As Integer = 0 To Me.properties.spawnerCount - 1
			If i = 0 Then
				num2 = num * 57.29578F + 90F
			ElseIf i = 1 Then
				num2 = num * 57.29578F - 90F
			ElseIf i = 2 Then
				num2 = num * 57.29578F + 360F
			ElseIf i = 3 Then
				num2 = num * 57.29578F - 180F
			End If
			Me.points(i) = Me.pointPrefab.Create(New Vector3(x, y), num2, Me.properties)
			Me.points(i).transform.parent = MyBase.transform
		Next
	End Sub

	' Token: 0x060022EE RID: 8942 RVA: 0x00147C0C File Offset: 0x0014600C
	Private Iterator Function move_cr() As IEnumerator
		Dim offset As Single = 200F
		Dim size As Single = MyBase.GetComponent(Of SpriteRenderer)().bounds.size.x / 2F
		Dim count As Integer = 0
		Dim maxCount As Integer = Me.properties.spawnerMoveCountRange.RandomInt()
		Dim wait As YieldInstruction = New WaitForFixedUpdate()
		Dim startDir As Vector3 = Vector3.zero - MyBase.transform.position
		While MyBase.transform.position.y > 0F
			MyBase.transform.position += startDir.normalized * Me.properties.spawnerSpeed * CupheadTime.FixedDelta
			Yield wait
		End While
		While True
			Dim start As Vector3 = MyBase.transform.position
			Dim dir As Vector3 = Me.player.transform.position - MyBase.transform.position
			Dim endDist As Vector3 = start + dir.normalized * Me.properties.spawnerDistance
			If Me.isDead Then
				While True
					MyBase.transform.position += dir.normalized * Me.properties.spawnerSpeed * CupheadTime.FixedDelta
					If MyBase.transform.position.x < -640F - offset OrElse MyBase.transform.position.x > 640F + offset OrElse MyBase.transform.position.y > 360F + offset OrElse MyBase.transform.position.y < -360F - offset Then
						Exit For
					End If
					Yield wait
				End While
				Me.Kill()
				Me.StopAllCoroutines()
			End If
			While MyBase.transform.position <> endDist
				MyBase.transform.position = Vector3.MoveTowards(MyBase.transform.position, endDist, Me.properties.spawnerSpeed * CupheadTime.FixedDelta)
				If MyBase.transform.position.x < -640F + size OrElse MyBase.transform.position.x > 640F - size OrElse MyBase.transform.position.y > 360F - size OrElse MyBase.transform.position.y < -360F + size Then
					Exit While
				End If
				Yield wait
			End While
			Yield CupheadTime.WaitForSeconds(Me, Me.properties.spawnerMoveDelay)
			If Me.player Is Nothing OrElse Me.player.IsDead Then
				Me.player = PlayerManager.GetNext()
			End If
			count += 1
			If count >= maxCount Then
				While Me.attackCount < Me.properties.spawnerShotCount
					MyBase.animator.SetTrigger("Attack")
					Yield CupheadTime.WaitForSeconds(Me, Me.properties.spawnerShotDelay)
				End While
				Yield CupheadTime.WaitForSeconds(Me, Me.properties.spawnerHesitate)
				count = 0
				Me.attackCount = 0
				maxCount = Me.properties.spawnerMoveCountRange.RandomInt()
			End If
			Yield wait
		End While
		Return
	End Function

	' Token: 0x060022EF RID: 8943 RVA: 0x00147C28 File Offset: 0x00146028
	Private Sub Kill()
		For Each flyingGenieLevelSpawnerPoint As FlyingGenieLevelSpawnerPoint In Me.points
			flyingGenieLevelSpawnerPoint.Dead()
		Next
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub

	' Token: 0x060022F0 RID: 8944 RVA: 0x00147C68 File Offset: 0x00146068
	Public Sub Attack()
		For Each flyingGenieLevelSpawnerPoint As FlyingGenieLevelSpawnerPoint In Me.points
			flyingGenieLevelSpawnerPoint.Shoot()
		Next
		Me.attackCount += 1
		If Me.attackCount >= Me.properties.spawnerShotCount Then
			MyBase.animator.SetTrigger("End")
		End If
	End Sub

	' Token: 0x04002B84 RID: 11140
	Private Const AttackParameterName As String = "Attack"

	' Token: 0x04002B85 RID: 11141
	Private Const EndAttackParameterName As String = "End"

	' Token: 0x04002B86 RID: 11142
	<SerializeField()>
	Private pointPrefab As FlyingGenieLevelSpawnerPoint

	' Token: 0x04002B87 RID: 11143
	Private points As FlyingGenieLevelSpawnerPoint()

	' Token: 0x04002B88 RID: 11144
	Private properties As LevelProperties.FlyingGenie.Bullets

	' Token: 0x04002B89 RID: 11145
	Private player As AbstractPlayerController

	' Token: 0x04002B8A RID: 11146
	Private speed As Single

	' Token: 0x04002B8B RID: 11147
	Private attackCount As Integer

	' Token: 0x04002B8C RID: 11148
	Public isDead As Boolean
End Class
