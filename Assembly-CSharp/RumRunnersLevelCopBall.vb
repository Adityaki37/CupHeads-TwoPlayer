Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000789 RID: 1929
Public Class RumRunnersLevelCopBall
	Inherits AbstractProjectile

	' Token: 0x170003EC RID: 1004
	' (get) Token: 0x06002A92 RID: 10898 RVA: 0x0018DDC2 File Offset: 0x0018C1C2
	' (set) Token: 0x06002A93 RID: 10899 RVA: 0x0018DDCA File Offset: 0x0018C1CA
	Public Property leaveScreen As Boolean

	' Token: 0x170003ED RID: 1005
	' (get) Token: 0x06002A94 RID: 10900 RVA: 0x0018DDD3 File Offset: 0x0018C1D3
	Protected Overrides ReadOnly Property DestroyLifetime As Single
		Get
			Return 0F
		End Get
	End Property

	' Token: 0x06002A95 RID: 10901 RVA: 0x0018DDDA File Offset: 0x0018C1DA
	Protected Overrides Sub Start()
		MyBase.Start()
		Me.damageReceiver = MyBase.GetComponent(Of DamageReceiver)()
		AddHandler Me.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
	End Sub

	' Token: 0x06002A96 RID: 10902 RVA: 0x0018DE08 File Offset: 0x0018C208
	Public Sub Init(position As Vector3, velocity As Vector3, speed As Single, health As Single, properties As LevelProperties.RumRunners.CopBall, snoutPos As Transform)
		MyBase.ResetLifetime()
		MyBase.ResetDistance()
		MyBase.transform.position = position
		Me.properties = properties
		Me.health = health
		Me.velocity = velocity
		Me.offset = MyBase.GetComponent(Of Collider2D)().bounds.size.x / 2F
		Me.leaveScreen = False
		Me.circleCollider.enabled = False
		Me.launched = False
		Me.snoutPos = snoutPos
		If properties.constSpeed Then
			Me.speed = speed
		Else
			MyBase.StartCoroutine(Me.gradualSpeed_cr())
		End If
		RumRunnersLevelCopBall.LastSortingIndex -= 1
		If RumRunnersLevelCopBall.LastSortingIndex < 10 Then
			RumRunnersLevelCopBall.LastSortingIndex = 15
		End If
		MyBase.GetComponent(Of SpriteRenderer)().sortingOrder = RumRunnersLevelCopBall.LastSortingIndex
		Me.audioLoopNumber = RumRunnersLevelCopBall.CurrentAudioLoopIndex + 1
		RumRunnersLevelCopBall.CurrentAudioLoopIndex = MathUtilities.NextIndex(RumRunnersLevelCopBall.CurrentAudioLoopIndex, RumRunnersLevelCopBall.AudioLoopCount)
		Me.SFX_RUMRUN_P3_BallCop_VocalShouts_Loop(Me.audioLoopNumber)
	End Sub

	' Token: 0x06002A97 RID: 10903 RVA: 0x0018DF0E File Offset: 0x0018C30E
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
		MyBase.OnCollisionPlayer(hit, phase)
	End Sub

	' Token: 0x06002A98 RID: 10904 RVA: 0x0018DF2C File Offset: 0x0018C32C
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		Me.health -= info.damage
		If Me.health <= 0F Then
			Level.Current.RegisterMinionKilled()
			Me.Death(True)
		End If
	End Sub

	' Token: 0x06002A99 RID: 10905 RVA: 0x0018DF64 File Offset: 0x0018C364
	Public Sub Launch()
		MyBase.StartCoroutine(Me.move_cr())
		MyBase.StartCoroutine(Me.shoot_cr())
		MyBase.StartCoroutine(Me.checkToDie_cr())
		Me.circleCollider.enabled = True
		MyBase.GetComponent(Of SpriteRenderer)().sortingLayerName = "Projectiles"
		Me.launched = True
	End Sub

	' Token: 0x06002A9A RID: 10906 RVA: 0x0018DFBB File Offset: 0x0018C3BB
	Private Sub LateUpdate()
		If Not Me.launched Then
			MyBase.transform.position = Me.snoutPos.position
		End If
	End Sub

	' Token: 0x06002A9B RID: 10907 RVA: 0x0018DFE0 File Offset: 0x0018C3E0
	Private Iterator Function move_cr() As IEnumerator
		Dim wait As YieldInstruction = New WaitForFixedUpdate()
		While True
			MyBase.transform.position += Me.velocity * Me.speed * CupheadTime.FixedDelta
			Me.CheckBounds()
			Yield wait
		End While
		Return
	End Function

	' Token: 0x06002A9C RID: 10908 RVA: 0x0018DFFC File Offset: 0x0018C3FC
	Private Sub CheckBounds()
		Dim flag As Boolean = Me.properties.sideWallBounce AndAlso Not Me.leaveScreen
		Dim quaternion As Quaternion? = Nothing
		Dim zero As Vector3 = Vector3.zero
		If MyBase.transform.position.y > CupheadLevelCamera.Current.Bounds.yMax - Me.offset AndAlso Me.velocity.y > 0F Then
			Me.velocity.y = -Mathf.Abs(Me.velocity.y)
			quaternion = New Quaternion?(Quaternion.identity)
			zero = New Vector3(0F, Me.offset)
			Me.SFX_RUMRUN_P3_BallCop_Bounce()
		End If
		If MyBase.transform.position.y < CSng(Level.Current.Ground) + Me.offset AndAlso Me.velocity.y < 0F Then
			Me.velocity.y = Mathf.Abs(Me.velocity.y)
			quaternion = New Quaternion?(Quaternion.Euler(0F, 0F, 180F))
			zero = New Vector3(0F, -Me.offset)
			Me.SFX_RUMRUN_P3_BallCop_Bounce()
		End If
		If flag AndAlso MyBase.transform.position.x > CupheadLevelCamera.Current.Bounds.xMax - Me.offset AndAlso Me.velocity.x > 0F Then
			Me.velocity.x = -Mathf.Abs(Me.velocity.x)
			quaternion = New Quaternion?(Quaternion.Euler(0F, 0F, 270F))
			zero = New Vector3(Me.offset, 0F)
			Me.SFX_RUMRUN_P3_BallCop_Bounce()
		End If
		If flag AndAlso MyBase.transform.position.x < CupheadLevelCamera.Current.Bounds.xMin + Me.offset AndAlso Me.velocity.x < 0F Then
			Me.velocity.x = Mathf.Abs(Me.velocity.x)
			quaternion = New Quaternion?(Quaternion.Euler(0F, 0F, 90F))
			zero = New Vector3(-Me.offset, 0F)
			Me.SFX_RUMRUN_P3_BallCop_Bounce()
		End If
		If quaternion IsNot Nothing Then
			Dim effect As Effect = Me.dustEffect.Create(MyBase.transform.position + zero)
			effect.transform.rotation = quaternion.Value
			If quaternion.Value = Quaternion.identity Then
				effect.transform.Find("Dirt").gameObject.SetActive(True)
				effect.animator.SetInteger("DirtEffect", Global.UnityEngine.Random.Range(0, 3))
			End If
		End If
	End Sub

	' Token: 0x06002A9D RID: 10909 RVA: 0x0018E30C File Offset: 0x0018C70C
	Private Iterator Function shoot_cr() As IEnumerator
		Dim copBallBulletAngleStringMainIndex As Integer = Global.UnityEngine.Random.Range(0, Me.properties.copBallBulletAngleString.Length)
		Dim copBallBulletAngleString As String() = Me.properties.copBallBulletAngleString(copBallBulletAngleStringMainIndex).Split(New Char() { ","c })
		Dim copBallBulletAngleStringIndex As Integer = Global.UnityEngine.Random.Range(0, copBallBulletAngleString.Length)
		Dim copBallBulletTypeStringMainIndex As Integer = Global.UnityEngine.Random.Range(0, Me.properties.copBallBulletTypeString.Length)
		Dim copBallBulletTypeString As String() = Me.properties.copBallBulletTypeString(copBallBulletTypeStringMainIndex).Split(New Char() { ","c })
		Dim copBallBulletTypeStringIndex As Integer = Global.UnityEngine.Random.Range(0, copBallBulletTypeString.Length)
		Yield CupheadTime.WaitForSeconds(Me, Me.properties.copBallShootHesitate)
		While True
			Dim bullet As BasicProjectile
			If copBallBulletTypeString(copBallBulletTypeStringIndex)(0) = "P"c Then
				bullet = Me.copBulletPink
			Else
				bullet = Me.copBullet
			End If
			Dim angle As Single = 0F
			Parser.FloatTryParse(copBallBulletAngleString(copBallBulletAngleStringIndex), angle)
			bullet.Create(MyBase.transform.position, angle, Me.properties.copBallBulletSpeed)
			Yield CupheadTime.WaitForSeconds(Me, Me.properties.copBallShootDelay)
			If copBallBulletAngleStringIndex < copBallBulletAngleString.Length - 1 Then
				copBallBulletAngleStringIndex += 1
			Else
				copBallBulletAngleStringMainIndex = (copBallBulletAngleStringMainIndex + 1) Mod Me.properties.copBallBulletAngleString.Length
				copBallBulletAngleStringIndex = 0
			End If
			If copBallBulletTypeStringIndex < copBallBulletTypeString.Length - 1 Then
				copBallBulletTypeStringIndex += 1
			Else
				copBallBulletTypeStringMainIndex = (copBallBulletTypeStringMainIndex + 1) Mod Me.properties.copBallBulletTypeString.Length
				copBallBulletTypeStringIndex = 0
			End If
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06002A9E RID: 10910 RVA: 0x0018E327 File Offset: 0x0018C727
	Public Sub Death(playAudio As Boolean)
		Me.SFX_RUMRUN_P3_BallCop_Die(Me.audioLoopNumber, playAudio)
		Me.deathEffect.Create(MyBase.transform.position)
		Me.StopAllCoroutines()
		Me.Recycle()
	End Sub

	' Token: 0x06002A9F RID: 10911 RVA: 0x0018E35C File Offset: 0x0018C75C
	Private Iterator Function checkToDie_cr() As IEnumerator
		While MyBase.transform.position.x >= -1140F AndAlso MyBase.transform.position.x <= 1140F
			Yield Nothing
		End While
		Me.Death(False)
		Return
	End Function

	' Token: 0x06002AA0 RID: 10912 RVA: 0x0018E378 File Offset: 0x0018C778
	Private Iterator Function gradualSpeed_cr() As IEnumerator
		Dim t As Single = 0F
		Dim time As Single = Me.properties.gradualSpeedTime
		Dim val As Single = 1F
		While t < time
			t += CupheadTime.Delta
			Me.speed = Me.properties.gradualSpeed.GetFloatAt(val - t / time)
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06002AA1 RID: 10913 RVA: 0x0018E393 File Offset: 0x0018C793
	Private Sub SFX_RUMRUN_P3_BallCop_Bounce()
		AudioManager.Play("sfx_dlc_rumrun_copball_bounce")
		Me.emitAudioFromObject.Add("sfx_dlc_rumrun_copball_bounce")
	End Sub

	' Token: 0x06002AA2 RID: 10914 RVA: 0x0018E3B0 File Offset: 0x0018C7B0
	Private Sub SFX_RUMRUN_P3_BallCop_VocalShouts_Loop(loopNumber As Integer)
		Dim text As String = "sfx_dlc_rumrun_p3_ballcop_vocalshouts_loop_" + loopNumber
		AudioManager.PlayLoop(text)
		Me.emitAudioFromObject.Add("sfx_dlc_rumrun_p3_ballcop_vocalshouts_loop")
	End Sub

	' Token: 0x06002AA3 RID: 10915 RVA: 0x0018E3E4 File Offset: 0x0018C7E4
	Private Sub SFX_RUMRUN_P3_BallCop_Die(loopNumber As Integer, playAudio As Boolean)
		Dim text As String = "sfx_dlc_rumrun_p3_ballcop_vocalshouts_loop_" + loopNumber
		AudioManager.[Stop](text)
		If playAudio Then
			AudioManager.Play("sfx_dlc_rumrun_copball_bounce")
			Me.emitAudioFromObject.Add("sfx_dlc_rumrun_copball_bounce")
		End If
	End Sub

	' Token: 0x04003358 RID: 13144
	Private Const DIE_OFFSET_X As Single = 500F

	' Token: 0x04003359 RID: 13145
	Private Shared AudioLoopCount As Integer = 6

	' Token: 0x0400335A RID: 13146
	Private Shared CurrentAudioLoopIndex As Integer

	' Token: 0x0400335B RID: 13147
	Private Shared LastSortingIndex As Integer

	' Token: 0x0400335C RID: 13148
	<SerializeField()>
	Private copBullet As BasicProjectile

	' Token: 0x0400335D RID: 13149
	<SerializeField()>
	Private copBulletPink As BasicProjectile

	' Token: 0x0400335E RID: 13150
	<SerializeField()>
	Private dustEffect As Effect

	' Token: 0x0400335F RID: 13151
	<SerializeField()>
	Private deathEffect As Effect

	' Token: 0x04003360 RID: 13152
	Private properties As LevelProperties.RumRunners.CopBall

	' Token: 0x04003361 RID: 13153
	Private velocity As Vector3

	' Token: 0x04003362 RID: 13154
	Private damageReceiver As DamageReceiver

	' Token: 0x04003363 RID: 13155
	<SerializeField()>
	Private circleCollider As CircleCollider2D

	' Token: 0x04003364 RID: 13156
	Private health As Single

	' Token: 0x04003365 RID: 13157
	Private speed As Single

	' Token: 0x04003366 RID: 13158
	Private offset As Single

	' Token: 0x04003367 RID: 13159
	Private audioLoopNumber As Integer

	' Token: 0x04003368 RID: 13160
	Private launched As Boolean

	' Token: 0x04003369 RID: 13161
	Private snoutPos As Transform
End Class
