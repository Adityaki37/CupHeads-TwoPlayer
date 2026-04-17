Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000895 RID: 2197
Public Class TreePlatformingLevelMosquito
	Inherits AbstractCollidableObject

	' Token: 0x17000443 RID: 1091
	' (get) Token: 0x06003315 RID: 13077 RVA: 0x001DB307 File Offset: 0x001D9707
	' (set) Token: 0x06003316 RID: 13078 RVA: 0x001DB30F File Offset: 0x001D970F
	Public Property isActive As Boolean

	' Token: 0x06003317 RID: 13079 RVA: 0x001DB318 File Offset: 0x001D9718
	Private Sub Start()
		Me.startPos = MyBase.transform.position
		AudioManager.PlayLoop("level_platform_mosquito_loop")
		Me.emitAudioFromObject.Add("level_platform_mosquito_loop")
		Me.YPositionDown = Me.YPositionUp - 30F
		Me.YFall = Me.YPositionUp - 35F
		Me.endPos = MyBase.transform.position
		Me.endPos.y = Me.YPositionDown
		MyBase.StartCoroutine(Me.delay_start_cr(Global.UnityEngine.Random.Range(0F, 3F)))
	End Sub

	' Token: 0x06003318 RID: 13080 RVA: 0x001DB3B4 File Offset: 0x001D97B4
	Private Iterator Function delay_start_cr(delay As Single) As IEnumerator
		Yield New WaitForSeconds(delay)
		MyBase.StartCoroutine(Me.activate_cr())
		Return
	End Function

	' Token: 0x06003319 RID: 13081 RVA: 0x001DB3D6 File Offset: 0x001D97D6
	Private Sub SetLetters(one As Integer, two As Integer)
		MyBase.animator.SetInteger("FirstLetter", one)
		MyBase.animator.SetInteger("SecondLetter", two)
	End Sub

	' Token: 0x0600331A RID: 13082 RVA: 0x001DB3FC File Offset: 0x001D97FC
	Private Iterator Function check_platform_cr() As IEnumerator
		While True
			While Me.platform.transform.childCount <= 0
				Yield Nothing
			End While
			Me.StopMoveCoroutines()
			MyBase.StartCoroutine(Me.fall_cr())
			MyBase.animator.SetBool("Struggling", True)
			AudioManager.Play("level_platform_mosquito_step_on")
			Me.emitAudioFromObject.Add("level_platform_mosquito_step_on")
			AudioManager.[Stop]("level_platform_mosquito_loop")
			AudioManager.PlayLoop("level_platform_mosquito_struggle_loop")
			Me.emitAudioFromObject.Add("level_platform_mosquito_struggle_loop")
			If Not Me.projectileShooting Then
				MyBase.StartCoroutine(Me.shoot_up_cr())
			End If
			While Me.platform.transform.childCount > 0
				Yield Nothing
			End While
			Me.StopMoveCoroutines()
			Me.StartUp()
			MyBase.animator.SetBool("Struggling", False)
			AudioManager.[Stop]("level_platform_mosquito_struggle_loop")
			AudioManager.PlayLoop("level_platform_mosquito_loop")
			Me.emitAudioFromObject.Add("level_platform_mosquito_loop")
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x0600331B RID: 13083 RVA: 0x001DB417 File Offset: 0x001D9817
	Protected Overrides Sub OnCollisionEnemyProjectile(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionEnemyProjectile(hit, phase)
		If hit.GetComponent(Of TreePlatformingLevelDragonflyProjectile)() Then
			Me.KillPlatform()
		End If
	End Sub

	' Token: 0x0600331C RID: 13084 RVA: 0x001DB438 File Offset: 0x001D9838
	Private Iterator Function activate_cr() As IEnumerator
		MyBase.animator.Play("Pick_Type")
		MyBase.transform.position = New Vector3(Me.startPos.x, 1200F)
		Dim t As Single = 0F
		MyBase.GetComponent(Of Collider2D)().enabled = True
		Me.platform.gameObject.SetActive(True)
		While t < Me.returnTime
			Dim val As Single = EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, 0F, 1F, t / Me.returnTime)
			MyBase.transform.position = Vector2.Lerp(MyBase.transform.position, Me.startPos, val)
			t += CupheadTime.Delta
			Yield Nothing
		End While
		Me.isActive = True
		MyBase.transform.position = Me.startPos
		Me.StartDown()
		MyBase.StartCoroutine(Me.check_platform_cr())
		Select Case Me.type
			Case TreePlatformingLevelMosquito.Type.AA
				Me.SetLetters(1, 1)
			Case TreePlatformingLevelMosquito.Type.AB
				Me.SetLetters(1, 2)
			Case TreePlatformingLevelMosquito.Type.AC
				Me.SetLetters(1, 3)
			Case TreePlatformingLevelMosquito.Type.BA
				Me.SetLetters(2, 1)
			Case TreePlatformingLevelMosquito.Type.BB
				Me.SetLetters(2, 2)
			Case TreePlatformingLevelMosquito.Type.BC
				Me.SetLetters(2, 3)
			Case TreePlatformingLevelMosquito.Type.CA
				Me.SetLetters(3, 1)
			Case TreePlatformingLevelMosquito.Type.CB
				Me.SetLetters(3, 2)
			Case TreePlatformingLevelMosquito.Type.CC
				Me.SetLetters(3, 3)
		End Select
		Yield Nothing
		Return
	End Function

	' Token: 0x0600331D RID: 13085 RVA: 0x001DB454 File Offset: 0x001D9854
	Public Sub KillPlatform()
		If Me.explosion IsNot Nothing Then
			Me.explosion.Create(MyBase.transform.position, New Vector3(0.85F, 0.85F, 0.85F))
		End If
		Me.platform.transform.DetachChildren()
		Me.platform.gameObject.SetActive(False)
		MyBase.GetComponent(Of Collider2D)().enabled = False
		MyBase.animator.SetBool("Struggling", False)
		AudioManager.[Stop]("level_platform_mosquito_loop")
		AudioManager.[Stop]("level_platform_mosquito_struggle_loop")
		AudioManager.Play("level_platform_mosquito_death")
		Me.emitAudioFromObject.Add("level_platform_mosquito_death")
		Me.isActive = False
		Me.StopAllCoroutines()
		MyBase.StartCoroutine(Me.die_cr())
	End Sub

	' Token: 0x0600331E RID: 13086 RVA: 0x001DB524 File Offset: 0x001D9924
	Private Iterator Function die_cr() As IEnumerator
		MyBase.animator.SetTrigger("Death")
		Dim velocity As Single = 0F
		Dim gravity As Single = 2250F
		While MyBase.transform.position.y > -CupheadLevelCamera.Current.Height - 200F
			MyBase.transform.AddPosition(0F, velocity * CupheadTime.Delta, 0F)
			velocity -= gravity * CupheadTime.Delta
			Yield Nothing
		End While
		Yield CupheadTime.WaitForSeconds(Me, Me.reappearDelay)
		Me.projectileShooting = False
		MyBase.StartCoroutine(Me.activate_cr())
		Yield Nothing
		Return
	End Function

	' Token: 0x0600331F RID: 13087 RVA: 0x001DB540 File Offset: 0x001D9940
	Public Iterator Function sine_cr() As IEnumerator
		Dim time As Single = Global.UnityEngine.Random.Range(1F, 1.5F)
		Dim t As Single = Global.UnityEngine.Random.Range(0F, 0.5F)
		Dim val As Single = 0.5F
		While True
			If CupheadTime.Delta IsNot 0F Then
				t += CupheadTime.Delta
				Dim num As Single = Mathf.Sin(t / time)
				MyBase.transform.AddPosition(0F, num * val, 0F)
			End If
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06003320 RID: 13088 RVA: 0x001DB55C File Offset: 0x001D995C
	Private Iterator Function shoot_up_cr() As IEnumerator
		Me.projectileShooting = True
		Yield CupheadTime.WaitForSeconds(Me, Me.projectileShootUpTime)
		If Me.projectile IsNot Nothing Then
			Me.projectile.Create(New Vector2(MyBase.transform.position.x, MyBase.transform.position.y - 500F), 90F, Me.projectileSpeed)
		End If
		Me.projectileShooting = False
		Yield Nothing
		Return
	End Function

	' Token: 0x06003321 RID: 13089 RVA: 0x001DB578 File Offset: 0x001D9978
	Private Sub StopMoveCoroutines()
		If Me.upCoroutine IsNot Nothing Then
			MyBase.StopCoroutine(Me.upCoroutine)
			Me.upCoroutine = Nothing
		End If
		If Me.downCoroutine IsNot Nothing Then
			MyBase.StopCoroutine(Me.downCoroutine)
			Me.downCoroutine = Nothing
		End If
		If Me.fallCoroutine IsNot Nothing Then
			MyBase.StopCoroutine(Me.fallCoroutine)
			Me.fallCoroutine = Nothing
		End If
		If Me.gotoCoroutine IsNot Nothing Then
			MyBase.StopCoroutine(Me.gotoCoroutine)
			Me.gotoCoroutine = Nothing
		End If
	End Sub

	' Token: 0x06003322 RID: 13090 RVA: 0x001DB5FD File Offset: 0x001D99FD
	Public Sub StartDown()
		Me.StopMoveCoroutines()
		Me.downCoroutine = MyBase.StartCoroutine(Me.down_cr())
	End Sub

	' Token: 0x06003323 RID: 13091 RVA: 0x001DB617 File Offset: 0x001D9A17
	Public Sub StartUp()
		Me.StopMoveCoroutines()
		Me.upCoroutine = MyBase.StartCoroutine(Me.up_cr())
	End Sub

	' Token: 0x06003324 RID: 13092 RVA: 0x001DB634 File Offset: 0x001D9A34
	Private Iterator Function down_cr() As IEnumerator
		Yield New WaitForSeconds(0F)
		Me.gotoCoroutine = MyBase.StartCoroutine(Me.goTo_cr(Me.YPositionUp, Me.YPositionDown, 1.5F, EaseUtils.EaseType.easeInOutSine))
		Yield Me.gotoCoroutine
		Me.StartUp()
		Return
	End Function

	' Token: 0x06003325 RID: 13093 RVA: 0x001DB650 File Offset: 0x001D9A50
	Private Iterator Function up_cr() As IEnumerator
		Yield New WaitForSeconds(0F)
		Me.gotoCoroutine = MyBase.StartCoroutine(Me.goTo_cr(Me.YPositionDown, Me.YPositionUp, 1.5F, EaseUtils.EaseType.easeInOutSine))
		Yield Me.gotoCoroutine
		Me.StartDown()
		Return
	End Function

	' Token: 0x06003326 RID: 13094 RVA: 0x001DB66C File Offset: 0x001D9A6C
	Private Iterator Function fall_cr() As IEnumerator
		Dim time As Single = (1F - (MyBase.transform.position.y - Me.startPos.y) / Me.YFall) * 0.13F
		Me.gotoCoroutine = MyBase.StartCoroutine(Me.goTo_cr(MyBase.transform.position.y - Me.startPos.y, Me.YFall, time, EaseUtils.EaseType.easeOutSine))
		Yield Me.gotoCoroutine
		Me.gotoCoroutine = MyBase.StartCoroutine(Me.goTo_cr(Me.YFall, Me.YPositionDown, 0.12F, EaseUtils.EaseType.easeInOutSine))
		Yield Me.gotoCoroutine
		Return
	End Function

	' Token: 0x06003327 RID: 13095 RVA: 0x001DB688 File Offset: 0x001D9A88
	Private Iterator Function goTo_cr(start As Single, [end] As Single, time As Single, ease As EaseUtils.EaseType) As IEnumerator
		Dim t As Single = 0F
		MyBase.transform.SetPosition(Nothing, New Single?(Me.startPos.y + start), Nothing)
		While t < time
			Dim val As Single = t / time
			MyBase.transform.SetPosition(Nothing, New Single?(Me.startPos.y + EaseUtils.Ease(ease, start, [end], val)), Nothing)
			t += Time.deltaTime
			Yield MyBase.StartCoroutine(MyBase.WaitForPause_CR())
		End While
		MyBase.transform.SetPosition(Nothing, New Single?(Me.startPos.y + [end]), Nothing)
		Return
	End Function

	' Token: 0x04003B42 RID: 15170
	<Header("Projectile Variables")>
	<SerializeField()>
	Private projectile As BasicProjectile

	' Token: 0x04003B43 RID: 15171
	<SerializeField()>
	Private projectileSpeed As Single

	' Token: 0x04003B44 RID: 15172
	<SerializeField()>
	Private projectileShootsUP As Boolean

	' Token: 0x04003B45 RID: 15173
	<SerializeField()>
	Private projectileShootUpTime As Single

	' Token: 0x04003B46 RID: 15174
	<Space(10F)>
	<SerializeField()>
	Private platform As LevelPlatform

	' Token: 0x04003B47 RID: 15175
	<SerializeField()>
	Private reappearDelay As Single = 1F

	' Token: 0x04003B48 RID: 15176
	<SerializeField()>
	Private explosion As PlatformingLevelGenericExplosion

	' Token: 0x04003B49 RID: 15177
	Public returnTime As Single = 1.5F

	' Token: 0x04003B4B RID: 15179
	Private projectileShooting As Boolean

	' Token: 0x04003B4C RID: 15180
	Public type As TreePlatformingLevelMosquito.Type

	' Token: 0x04003B4D RID: 15181
	Public YPositionUp As Single

	' Token: 0x04003B4E RID: 15182
	Public Const TIME As Single = 1.5F

	' Token: 0x04003B4F RID: 15183
	Public Const FALL_TIME As Single = 0.13F

	' Token: 0x04003B50 RID: 15184
	Public Const FALL_BOUNCE_TIME As Single = 0.12F

	' Token: 0x04003B51 RID: 15185
	Public Const DELAY As Single = 0F

	' Token: 0x04003B52 RID: 15186
	Public Const FLOAT_EASE As EaseUtils.EaseType = EaseUtils.EaseType.easeInOutSine

	' Token: 0x04003B53 RID: 15187
	Public Const FALL_EASE As EaseUtils.EaseType = EaseUtils.EaseType.easeOutSine

	' Token: 0x04003B54 RID: 15188
	Public Const FALL_BOUNCE_EASE As EaseUtils.EaseType = EaseUtils.EaseType.easeInOutSine

	' Token: 0x04003B55 RID: 15189
	<SerializeField()>
	Private state As TreePlatformingLevelMosquito.State

	' Token: 0x04003B56 RID: 15190
	Private startPos As Vector3

	' Token: 0x04003B57 RID: 15191
	Private endPos As Vector3

	' Token: 0x04003B58 RID: 15192
	Private YPositionDown As Single

	' Token: 0x04003B59 RID: 15193
	Private YFall As Single

	' Token: 0x04003B5A RID: 15194
	Private upCoroutine As Coroutine

	' Token: 0x04003B5B RID: 15195
	Private downCoroutine As Coroutine

	' Token: 0x04003B5C RID: 15196
	Private fallCoroutine As Coroutine

	' Token: 0x04003B5D RID: 15197
	Private gotoCoroutine As Coroutine

	' Token: 0x02000896 RID: 2198
	Public Enum Type
		' Token: 0x04003B5F RID: 15199
		AA
		' Token: 0x04003B60 RID: 15200
		AB
		' Token: 0x04003B61 RID: 15201
		AC
		' Token: 0x04003B62 RID: 15202
		BA
		' Token: 0x04003B63 RID: 15203
		BB
		' Token: 0x04003B64 RID: 15204
		BC
		' Token: 0x04003B65 RID: 15205
		CA
		' Token: 0x04003B66 RID: 15206
		CB
		' Token: 0x04003B67 RID: 15207
		CC
	End Enum

	' Token: 0x02000897 RID: 2199
	Public Enum State
		' Token: 0x04003B69 RID: 15209
		Up
		' Token: 0x04003B6A RID: 15210
		Down
		' Token: 0x04003B6B RID: 15211
		PlayerOn
	End Enum
End Class
