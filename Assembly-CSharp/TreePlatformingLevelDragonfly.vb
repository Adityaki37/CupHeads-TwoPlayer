Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports UnityEngine

' Token: 0x0200088A RID: 2186
Public Class TreePlatformingLevelDragonfly
	Inherits PlatformingLevelBigEnemy

	' Token: 0x060032D3 RID: 13011 RVA: 0x001D8838 File Offset: 0x001D6C38
	Protected Overrides Sub Start()
		MyBase.Start()
		Me.LockDistance = 1550F
		Me.startPos = MyBase.transform.position
		Me.aimIndex = Global.UnityEngine.Random.Range(0, MyBase.Properties.dragonFlyAimString.Split(New Char() { ","c }).Length)
		Me.delayIndex = Global.UnityEngine.Random.Range(0, MyBase.Properties.dragonFlyAtkDelayString.Split(New Char() { ","c }).Length)
		Me.LockDistance -= MyBase.Properties.dragonFlyLockDistOffset
		Me.mosquitos = New List(Of TreePlatformingLevelMosquito)(Me.platforms.GetComponentsInChildren(Of TreePlatformingLevelMosquito)())
		Me.currentMosquitos = Me.randomizeList(Me.mosquitos)
		MyBase.StartCoroutine(Me.enter_cr())
	End Sub

	' Token: 0x060032D4 RID: 13012 RVA: 0x001D8906 File Offset: 0x001D6D06
	Protected Overrides Sub Shoot()
		If Not Me.isShooting Then
			MyBase.StartCoroutine(Me.shoot_cr())
		End If
	End Sub

	' Token: 0x060032D5 RID: 13013 RVA: 0x001D8920 File Offset: 0x001D6D20
	Private Iterator Function enter_cr() As IEnumerator
		MyBase.transform.position = New Vector3(Me.startPos.x + 800F, Me.startPos.y)
		While Not Me.bigEnemyCameraLock
			Yield Nothing
		End While
		Dim t As Single = 0F
		Dim time As Single = MyBase.Properties.dragonFlyInitRiseTime
		While t < time
			Dim val As Single = EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, 0F, 1F, t / time)
			MyBase.transform.position = Vector2.Lerp(MyBase.transform.position, Me.startPos, val)
			t += CupheadTime.Delta
			Yield Nothing
		End While
		MyBase.transform.position = Me.startPos
		MyBase.GetComponent(Of Collider2D)().enabled = True
		MyBase.StartCoroutine(Me.sine_cr())
		Return
	End Function

	' Token: 0x060032D6 RID: 13014 RVA: 0x001D893C File Offset: 0x001D6D3C
	Private Iterator Function shoot_cr() As IEnumerator
		Dim t As Single = 0F
		Dim t2 As Single = 0F
		Dim angle As Single = 0F
		Dim pickDir As Boolean = False
		Dim direction As Vector3 = Vector3.zero
		Me.isShooting = True
		MyBase.animator.SetTrigger("Shoot")
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Warning_Start", False, True)
		Yield CupheadTime.WaitForSeconds(Me, MyBase.Properties.dragonFlyWarningDuration)
		MyBase.animator.SetTrigger("Continue")
		While t < MyBase.Properties.dragonFlyAttackDuration
			pickDir = False
			While t2 < MyBase.Properties.dragonFlyProjectileDelay
				t2 += CupheadTime.Delta
				t += CupheadTime.Delta
				Yield Nothing
			End While
			t2 = 0F
			If MyBase.Properties.dragonFlyAimString.Split(New Char() { ","c })(Me.aimIndex)(0) = "R"c Then
				While Not pickDir
					If Me.currentMosquitos(Me.cycleIndex).isActive Then
						direction = Me.currentMosquitos(Me.cycleIndex).transform.position - MyBase.transform.position
						Me.currentMosquitos.RemoveAt(Me.cycleIndex)
						If Me.currentMosquitos.Count > 0 Then
							Me.cycleIndex = (Me.cycleIndex + 1) Mod Me.currentMosquitos.Count
						Else
							Me.currentMosquitos = Me.randomizeList(Me.mosquitos)
						End If
						pickDir = True
					Else
						Me.cycleIndex = (Me.cycleIndex + 1) Mod Me.currentMosquitos.Count
						Me.currentMosquitos = Me.randomizeList(Me.mosquitos)
					End If
					Yield Nothing
				End While
			ElseIf MyBase.Properties.dragonFlyAimString.Split(New Char() { ","c })(Me.aimIndex)(0) = "P"c AndAlso Me._target.transform.position.x < MyBase.transform.position.x Then
				direction = Me._target.transform.position - MyBase.transform.position
			End If
			angle = MathUtils.DirectionToAngle(direction)
			Me.projectile.Create(Me.projectileRoot.transform.position, angle + 5F, MyBase.Properties.dragonFlyProjectileSpeed)
			Me.aimIndex = (Me.aimIndex + 1) Mod MyBase.Properties.dragonFlyAimString.Split(New Char() { ","c }).Length
			t += CupheadTime.Delta
			Yield Nothing
		End While
		MyBase.animator.SetTrigger("Continue")
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Attack_To_Idle", False, True)
		Yield CupheadTime.WaitForSeconds(Me, Parser.FloatParse(MyBase.Properties.dragonFlyAtkDelayString.Split(New Char() { ","c })(Me.delayIndex)))
		Me.delayIndex = (Me.delayIndex + 1) Mod MyBase.Properties.dragonFlyAtkDelayString.Split(New Char() { ","c }).Length
		Me.isShooting = False
		Yield Nothing
		Return
	End Function

	' Token: 0x060032D7 RID: 13015 RVA: 0x001D8958 File Offset: 0x001D6D58
	Private Function randomizeList(platforms As List(Of TreePlatformingLevelMosquito)) As List(Of TreePlatformingLevelMosquito)
		Dim list As List(Of TreePlatformingLevelMosquito) = New List(Of TreePlatformingLevelMosquito)()
		Dim list2 As List(Of TreePlatformingLevelMosquito) = New List(Of TreePlatformingLevelMosquito)()
		list2.AddRange(platforms)
		For i As Integer = 0 To platforms.Count - 1
			Dim num As Integer = Global.UnityEngine.Random.Range(0, list2.Count)
			list.Add(list2(num))
			list2.RemoveAt(num)
		Next
		Me.cycleIndex = 0
		Return list
	End Function

	' Token: 0x060032D8 RID: 13016 RVA: 0x001D89B8 File Offset: 0x001D6DB8
	Public Iterator Function sine_cr() As IEnumerator
		Dim time As Single = 0.5F
		Dim t As Single = 0F
		Dim val As Single = 1F
		While True
			If Not Me.isShooting AndAlso CupheadTime.Delta IsNot 0F Then
				t += CupheadTime.Delta
				Dim num As Single = Mathf.Sin(t / time)
				MyBase.transform.AddPosition(0F, num * val, 0F)
			End If
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x060032D9 RID: 13017 RVA: 0x001D89D4 File Offset: 0x001D6DD4
	Protected Overrides Sub Die()
		If Not Me.isDead Then
			Me.StopAllCoroutines()
			Me.isDead = True
			MyBase.GetComponent(Of Collider2D)().enabled = False
			MyBase.animator.Play("Death")
			AudioManager.Play("level_platform_dragonfly_death")
			Me.emitAudioFromObject.Add("level_platform_dragonfly_death")
			Me.explosion.StartExplosion()
			MyBase.StartCoroutine(Me.fall_cr())
		End If
	End Sub

	' Token: 0x060032DA RID: 13018 RVA: 0x001D8A48 File Offset: 0x001D6E48
	Private Iterator Function fall_cr() As IEnumerator
		Dim velocity As Single = 0F
		Dim gravity As Single = 1500F
		Yield CupheadTime.WaitForSeconds(Me, 1.5F)
		Me.explosion.StopExplosions()
		While MyBase.transform.position.y > -CupheadLevelCamera.Current.Height - 200F
			MyBase.transform.AddPosition(0F, velocity * CupheadTime.Delta, 0F)
			velocity -= gravity * CupheadTime.Delta
			Yield Nothing
		End While
		MyBase.Die()
		Yield Nothing
		Return
	End Function

	' Token: 0x060032DB RID: 13019 RVA: 0x001D8A63 File Offset: 0x001D6E63
	Private Sub SoundDragonflyAttackWarning()
		AudioManager.Play("level_platform_dragonfly_attack_warning")
		Me.emitAudioFromObject.Add("level_platform_dragonfly_attack_warning")
	End Sub

	' Token: 0x060032DC RID: 13020 RVA: 0x001D8A7F File Offset: 0x001D6E7F
	Private Sub SoundDragonflyAttackStart()
		AudioManager.Play("level_platform_dragonfly_attack_start")
		Me.emitAudioFromObject.Add("level_platform_dragonfly_attack_start")
	End Sub

	' Token: 0x04003B02 RID: 15106
	<SerializeField()>
	Private explosion As LevelBossDeathExploder

	' Token: 0x04003B03 RID: 15107
	<SerializeField()>
	Private projectile As BasicProjectile

	' Token: 0x04003B04 RID: 15108
	<SerializeField()>
	Private projectileRoot As Transform

	' Token: 0x04003B05 RID: 15109
	Public platforms As GameObject

	' Token: 0x04003B06 RID: 15110
	Private mosquitos As List(Of TreePlatformingLevelMosquito)

	' Token: 0x04003B07 RID: 15111
	Private currentMosquitos As List(Of TreePlatformingLevelMosquito)

	' Token: 0x04003B08 RID: 15112
	Private startPos As Vector3

	' Token: 0x04003B09 RID: 15113
	Private delayIndex As Integer

	' Token: 0x04003B0A RID: 15114
	Private aimIndex As Integer

	' Token: 0x04003B0B RID: 15115
	Private cycleIndex As Integer

	' Token: 0x04003B0C RID: 15116
	Private isShooting As Boolean
End Class
