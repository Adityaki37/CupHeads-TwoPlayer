Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020008C5 RID: 2245
Public Class HarbourPlatformingLevelClam
	Inherits PlatformingLevelShootingEnemy

	' Token: 0x0600346F RID: 13423 RVA: 0x001E7146 File Offset: 0x001E5546
	Protected Overrides Sub Start()
		MyBase.Start()
		Me.startPos = MyBase.transform.position
	End Sub

	' Token: 0x06003470 RID: 13424 RVA: 0x001E715F File Offset: 0x001E555F
	Protected Overrides Sub StartShoot()
		If Me.counter < MyBase.Properties.ClamShotCount Then
			Me.counter += 1
			MyBase.StartShoot()
			Me.AttackSFX()
		End If
	End Sub

	' Token: 0x06003471 RID: 13425 RVA: 0x001E7194 File Offset: 0x001E5594
	Protected Overrides Sub Update()
		If Not Me.startClam AndAlso Me.octopus IsNot Nothing AndAlso Me.octopus.Started() Then
			Me.Popup()
			Me.startClam = True
		End If
		If Me._target IsNot Nothing Then
			If Not Me.startClam AndAlso Me.octopus Is Nothing Then
				Me.dist = Me._target.transform.position.x - Me.onTrigger.transform.position.x
				If Me.dist > 0F AndAlso Not Me.startClam Then
					Me.Popup()
					Me.startClam = True
				End If
			Else
				Me.dist = Me._target.transform.position.x - Me.offTrigger.transform.position.x
				If Me.dist > 0F AndAlso Not Me.endClam Then
					Me.startClam = False
					Me.endClam = True
				End If
			End If
		End If
	End Sub

	' Token: 0x06003472 RID: 13426 RVA: 0x001E72CB File Offset: 0x001E56CB
	Private Sub Popup()
		MyBase.animator.SetTrigger("OnPopup")
		MyBase.transform.parent = CupheadLevelCamera.Current.transform
		MyBase.StartCoroutine(Me.pop_up_cr())
	End Sub

	' Token: 0x06003473 RID: 13427 RVA: 0x001E7300 File Offset: 0x001E5700
	Private Iterator Function pop_up_cr() As IEnumerator
		While True
			If Not Me.isDead Then
				Dim ease As EaseUtils.EaseType = EaseUtils.EaseType.easeInOutSine
				Dim t As Single = 0F
				Dim time As Single = MyBase.Properties.ClamTimeSpeedUp
				Dim startY As Single = Me.startPos.y
				Dim endY As Single = MyBase.Properties.ClamMaxPointRange.RandomFloat()
				MyBase.transform.SetPosition(New Single?(CupheadLevelCamera.Current.Bounds.xMin + Me.offset), Nothing, Nothing)
				Me.Show()
				While t < time
					Dim val As Single = t / time
					MyBase.transform.SetPosition(Nothing, New Single?(EaseUtils.Ease(ease, startY, endY, val)), Nothing)
					t += CupheadTime.Delta
					Yield Nothing
				End While
				MyBase.animator.SetTrigger("OnSlowdown")
				MyBase.transform.SetPosition(Nothing, New Single?(endY), Nothing)
				t = 0F
				time = MyBase.Properties.ClamTimeSpeedDown
				Yield Nothing
				While t < time
					Dim val2 As Single = t / time
					MyBase.transform.SetPosition(Nothing, New Single?(EaseUtils.Ease(ease, endY, startY, val2)), Nothing)
					t += CupheadTime.Delta
					Yield Nothing
				End While
				Me.Hide()
				MyBase.transform.SetPosition(Nothing, New Single?(startY), Nothing)
			End If
			Yield CupheadTime.WaitForSeconds(Me, MyBase.Properties.ProjectileDelay.RandomFloat())
			If Not Me.endClam Then
				Me.isDead = False
			Else
				Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
			End If
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06003474 RID: 13428 RVA: 0x001E731C File Offset: 0x001E571C
	Protected Overrides Sub OnDrawGizmos()
		MyBase.OnDrawGizmos()
		Gizmos.color = New Color(0F, 0F, 1F, 1F)
		Gizmos.DrawLine(Me.offTrigger.transform.position, New Vector3(Me.offTrigger.transform.position.x, 5000F, 0F))
		Gizmos.DrawLine(Me.onTrigger.transform.position, New Vector3(Me.onTrigger.transform.position.x, 5000F, 0F))
	End Sub

	' Token: 0x06003475 RID: 13429 RVA: 0x001E73C8 File Offset: 0x001E57C8
	Protected Overrides Sub Die()
		MyBase.GetComponent(Of Collider2D)().enabled = False
		MyBase.GetComponent(Of DamageReceiver)().enabled = False
		MyBase.animator.Play("Off")
		Me.DeathParts()
		Me.isDead = True
		Me.StopAllCoroutines()
		MyBase.StartCoroutine(Me.fall_cr())
		MyBase.Explode()
	End Sub

	' Token: 0x06003476 RID: 13430 RVA: 0x001E7424 File Offset: 0x001E5824
	Private Iterator Function fall_cr() As IEnumerator
		Dim velocity As Vector2 = New Vector2(0F, 200F)
		Dim accumulatedGravity As Single = 0F
		Dim speed As Single = 400F
		While MyBase.transform.position.y > CupheadLevelCamera.Current.Bounds.yMin - 100F
			MyBase.transform.position += (velocity + New Vector2(-speed, accumulatedGravity)) * Time.fixedDeltaTime
			accumulatedGravity += -100F
			Yield Nothing
		End While
		Yield CupheadTime.WaitForSeconds(Me, MyBase.Properties.ClamDespawnDelayRange.RandomFloat())
		MyBase.transform.SetPosition(New Single?(CupheadLevelCamera.Current.Bounds.xMin + Me.offset), New Single?(Me.startPos.y), Nothing)
		Me.Hide()
		Yield Nothing
		Return
	End Function

	' Token: 0x06003477 RID: 13431 RVA: 0x001E7440 File Offset: 0x001E5840
	Private Sub Hide()
		Me.counter = 0
		MyBase.animator.Play("Off")
		MyBase.GetComponent(Of Collider2D)().enabled = False
		MyBase.GetComponent(Of DamageReceiver)().enabled = False
		Me.OnStart()
		If Me.isDead Then
			Me.Popup()
		End If
	End Sub

	' Token: 0x06003478 RID: 13432 RVA: 0x001E7493 File Offset: 0x001E5893
	Private Sub Show()
		MyBase.animator.SetTrigger("OnPopup")
		MyBase.GetComponent(Of Collider2D)().enabled = True
		MyBase.GetComponent(Of DamageReceiver)().enabled = True
	End Sub

	' Token: 0x06003479 RID: 13433 RVA: 0x001E74C0 File Offset: 0x001E58C0
	Private Sub DeathParts()
		For Each spriteDeathParts As SpriteDeathParts In Me.deathParts
			spriteDeathParts.CreatePart(MyBase.transform.position)
		Next
	End Sub

	' Token: 0x0600347A RID: 13434 RVA: 0x001E74FE File Offset: 0x001E58FE
	Private Sub AttackSFX()
		AudioManager.Play("harbour_clam_attack")
		Me.emitAudioFromObject.Add("harbour_clam_attack")
	End Sub

	' Token: 0x0600347B RID: 13435 RVA: 0x001E751A File Offset: 0x001E591A
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me.octopus = Nothing
		Me.deathParts = Nothing
	End Sub

	' Token: 0x04003C98 RID: 15512
	Private Const GRAVITY As Single = -100F

	' Token: 0x04003C99 RID: 15513
	<SerializeField()>
	Private deathParts As SpriteDeathParts()

	' Token: 0x04003C9A RID: 15514
	<SerializeField()>
	Private main As Transform

	' Token: 0x04003C9B RID: 15515
	<SerializeField()>
	Private onTrigger As Transform

	' Token: 0x04003C9C RID: 15516
	<SerializeField()>
	Private offTrigger As Transform

	' Token: 0x04003C9D RID: 15517
	<SerializeField()>
	Private octopus As HarbourPlatformingLevelOctopus

	' Token: 0x04003C9E RID: 15518
	Private startClam As Boolean

	' Token: 0x04003C9F RID: 15519
	Private endClam As Boolean

	' Token: 0x04003CA0 RID: 15520
	Private isDead As Boolean

	' Token: 0x04003CA1 RID: 15521
	Private dist As Single = -1000F

	' Token: 0x04003CA2 RID: 15522
	Private offset As Single = 100F

	' Token: 0x04003CA3 RID: 15523
	Private counter As Integer

	' Token: 0x04003CA4 RID: 15524
	Private startPos As Vector3
End Class
