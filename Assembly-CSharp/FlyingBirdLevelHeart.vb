Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x0200061F RID: 1567
Public Class FlyingBirdLevelHeart
	Inherits AbstractCollidableObject

	' Token: 0x06001FE2 RID: 8162 RVA: 0x001249B8 File Offset: 0x00122DB8
	Public Sub InitHeart(properties As LevelProperties.FlyingBird)
		Me.properties = properties
		Me.mainShootIndex = Global.UnityEngine.Random.Range(0, properties.CurrentState.heart.shootString.Length)
		Me.projectileMainIndex = Global.UnityEngine.Random.Range(0, properties.CurrentState.heart.numOfProjectiles.Length)
		Me.thisAnimator = MyBase.GetComponent(Of Animator)()
	End Sub

	' Token: 0x06001FE3 RID: 8163 RVA: 0x00124A14 File Offset: 0x00122E14
	Public Sub StartHeartAttack()
		Dim [next] As AbstractPlayerController = PlayerManager.GetNext()
		Me.faceRight = [next].transform.position.x > MyBase.transform.position.x
		For i As Integer = 0 To Me.renderers.Length - 1
			Me.renderers(i).flipX = Me.faceRight
		Next
		MyBase.gameObject.SetActive(True)
		MyBase.StartCoroutine(Me.accend_cr())
	End Sub

	' Token: 0x06001FE4 RID: 8164 RVA: 0x00124A9C File Offset: 0x00122E9C
	Private Iterator Function accend_cr() As IEnumerator
		Dim start As Single = MyBase.transform.position.y
		Me.FireSpreadshot()
		While MyBase.transform.position.y < start + Me.properties.CurrentState.heart.heartHeight
			MyBase.transform.position += Vector3.up * Me.properties.CurrentState.heart.movementSpeed * CupheadTime.Delta
			If MyBase.transform.localScale.x < 1F Then
				MyBase.transform.localScale += Vector3.one * 1.75F * CupheadTime.Delta
			Else
				MyBase.transform.localScale = Vector3.one * 1F
			End If
			Yield Nothing
			If Me.properties.CurrentHealth <= 0F Then
				Exit While
			End If
		End While
		While MyBase.transform.position.y > start
			MyBase.transform.position += Vector3.down * Me.properties.CurrentState.heart.movementSpeed * CupheadTime.Delta
			If MyBase.transform.position.y < start + 100F Then
				If MyBase.transform.localScale.x > 0.5F Then
					MyBase.transform.localScale -= Vector3.one * 1.75F * CupheadTime.Delta
				Else
					MyBase.transform.localScale = Vector3.one * 0.5F
				End If
			End If
			Yield Nothing
			If Me.properties.CurrentHealth <= 0F Then
				Exit While
			End If
		End While
		MyBase.transform.SetPosition(Nothing, New Single?(start), Nothing)
		MyBase.gameObject.SetActive(False)
		Return
	End Function

	' Token: 0x06001FE5 RID: 8165 RVA: 0x00124AB7 File Offset: 0x00122EB7
	Private Sub FireSpreadshot()
		MyBase.StartCoroutine(Me.spreadShot_cr())
	End Sub

	' Token: 0x06001FE6 RID: 8166 RVA: 0x00124AC6 File Offset: 0x00122EC6
	Private Sub SpawnFX()
		Me.puffFX.Create(MyBase.transform.position)
	End Sub

	' Token: 0x06001FE7 RID: 8167 RVA: 0x00124AE0 File Offset: 0x00122EE0
	Private Iterator Function spreadShot_cr() As IEnumerator
		Dim player As AbstractPlayerController = PlayerManager.GetNext()
		Dim shootString As String() = Me.properties.CurrentState.heart.shootString(Me.mainShootIndex).Split(New Char() { ","c })
		Dim shootIndex As Integer = Global.UnityEngine.Random.Range(0, shootString.Length)
		For i As Integer = 0 To Me.properties.CurrentState.heart.shotCount - 1
			Dim projectileString As String() = Me.properties.CurrentState.heart.numOfProjectiles(Me.projectileMainIndex).Split(New Char() { ","c })
			Dim projectiles As Integer = 0
			Dim projectileDelay As Single = 0F
			If player Is Nothing OrElse player.IsDead Then
				player = PlayerManager.GetNext()
			End If
			Parser.IntTryParse(projectileString(Me.projectileSubIndex), projectiles)
			Parser.FloatTryParse(shootString(shootIndex), projectileDelay)
			Yield CupheadTime.WaitForSeconds(Me, projectileDelay)
			Me.thisAnimator.SetTrigger("Attack")
			Yield CupheadTime.WaitForSeconds(Me, 0.125F)
			Dim directionX As Single = player.transform.position.x - MyBase.transform.position.x
			Dim directionY As Single = player.transform.position.y - MyBase.transform.position.y
			AudioManager.Play("level_flyingbird_stretcher_regurgitate_projectile")
			Me.emitAudioFromObject.Add("level_flyingbird_stretcher_regurgitate_projectile")
			For j As Integer = 0 To projectiles - 1
				Dim num As Single = Me.properties.CurrentState.heart.spreadAngle.GetFloatAt(CSng(j) / (CSng(projectiles) - 1F))
				Dim num2 As Single = Me.properties.CurrentState.heart.spreadAngle.max / 2F
				num -= num2
				Dim num3 As Single = Mathf.Atan2(directionY, directionX) * 57.29578F
				If Me.faceRight AndAlso (num3 < -90F OrElse num3 > 90F) Then
					num3 = 180F - num3
				ElseIf Not Me.faceRight AndAlso num3 > -90F AndAlso num3 < 90F Then
					num3 = -180F - num3
				End If
				Dim vector As Vector3 = New Vector3(72F, 0F, 0F)
				vector *= CSng(If((Not Me.faceRight), 1, (-1)))
				Me.projectilePrefab.Create(MyBase.transform.position - vector, num3 + num, Me.properties.CurrentState.heart.projectileSpeed)
				shootIndex = (shootIndex + 1) Mod shootString.Length
			Next
			If shootIndex < shootString.Length - 1 Then
				shootIndex += 1
			Else
				Me.mainShootIndex = (Me.mainShootIndex + 1) Mod Me.properties.CurrentState.heart.shootString.Length
				shootIndex = 0
			End If
			If Me.projectileSubIndex < projectileString.Length - 1 Then
				Me.projectileSubIndex += 1
			Else
				Me.projectileMainIndex = (Me.projectileMainIndex + 1) Mod Me.properties.CurrentState.heart.numOfProjectiles.Length
				Me.projectileSubIndex = 0
			End If
			Yield Nothing
		Next
		Return
	End Function

	' Token: 0x0400285A RID: 10330
	Private Const ProjectileOffsetX As Single = 72F

	' Token: 0x0400285B RID: 10331
	Private Const ScaleRate As Single = 1.75F

	' Token: 0x0400285C RID: 10332
	Private Const ScaleStartPosition As Single = 100F

	' Token: 0x0400285D RID: 10333
	Private Const InitialScale As Single = 0.5F

	' Token: 0x0400285E RID: 10334
	Private Const TargetScale As Single = 1F

	' Token: 0x0400285F RID: 10335
	<SerializeField()>
	Private puffFX As Effect

	' Token: 0x04002860 RID: 10336
	<SerializeField()>
	Private projectilePrefab As BasicProjectile

	' Token: 0x04002861 RID: 10337
	<SerializeField()>
	Private renderers As SpriteRenderer()

	' Token: 0x04002862 RID: 10338
	Private mainShootIndex As Integer

	' Token: 0x04002863 RID: 10339
	Private projectileMainIndex As Integer

	' Token: 0x04002864 RID: 10340
	Private projectileSubIndex As Integer

	' Token: 0x04002865 RID: 10341
	Private thisAnimator As Animator

	' Token: 0x04002866 RID: 10342
	Private properties As LevelProperties.FlyingBird

	' Token: 0x04002867 RID: 10343
	Private faceRight As Boolean
End Class
