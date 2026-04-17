Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000524 RID: 1316
Public Class BeeLevelSecurityGuardBomb
	Inherits AbstractProjectile

	' Token: 0x060017A9 RID: 6057 RVA: 0x000D52A4 File Offset: 0x000D36A4
	Public Function Create(pos As Vector2, direction As Integer, idleTime As Single, warningTime As Single, childSpeed As Single, childCount As Integer) As BeeLevelSecurityGuardBomb
		Dim beeLevelSecurityGuardBomb As BeeLevelSecurityGuardBomb = TryCast(MyBase.Create(), BeeLevelSecurityGuardBomb)
		beeLevelSecurityGuardBomb.direction = direction
		beeLevelSecurityGuardBomb.idleTime = idleTime
		beeLevelSecurityGuardBomb.warningTime = warningTime
		beeLevelSecurityGuardBomb.childSpeed = childSpeed
		beeLevelSecurityGuardBomb.childCount = childCount
		beeLevelSecurityGuardBomb.transform.position = pos
		Return beeLevelSecurityGuardBomb
	End Function

	' Token: 0x060017AA RID: 6058 RVA: 0x000D52F5 File Offset: 0x000D36F5
	Protected Overrides Sub Start()
		MyBase.Start()
		MyBase.StartCoroutine(Me.go_cr())
	End Sub

	' Token: 0x060017AB RID: 6059 RVA: 0x000D530A File Offset: 0x000D370A
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If Me.damageDealer IsNot Nothing AndAlso phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x060017AC RID: 6060 RVA: 0x000D5334 File Offset: 0x000D3734
	Protected Overrides Sub Die()
		MyBase.Die()
		Me.StopAllCoroutines()
		Dim num As Single = CSng((360 / Me.childCount))
		For i As Integer = 0 To Me.childCount - 1
			Dim basicProjectile As BasicProjectile = Me.childPrefab.Create(MyBase.transform.position, num * CSng(i), Vector2.one, Me.childSpeed)
			basicProjectile.SetParryable(i Mod 2 <> 0)
		Next
	End Sub

	' Token: 0x060017AD RID: 6061 RVA: 0x000D53AC File Offset: 0x000D37AC
	Private Iterator Function go_cr() As IEnumerator
		Dim time As Single = 0.3F
		Dim pos As Vector2 = MyBase.transform.position + New Vector2(CSng((50 * Me.direction)), 100F)
		Yield MyBase.TweenPosition(MyBase.transform.position, pos, time, EaseUtils.EaseType.easeOutSine)
		Yield CupheadTime.WaitForSeconds(Me, Me.idleTime)
		AudioManager.PlayLoop("bee_guard_bomb_warning")
		Me.emitAudioFromObject.Add("bee_guard_bomb_warning")
		MyBase.animator.Play("Warning")
		Yield CupheadTime.WaitForSeconds(Me, Me.warningTime)
		AudioManager.[Stop]("bee_guard_bomb_warning")
		AudioManager.Play("bee_guard_bomb_explode")
		Me.emitAudioFromObject.Add("bee_guard_bomb_explode")
		Me.Die()
		Return
	End Function

	' Token: 0x060017AE RID: 6062 RVA: 0x000D53C7 File Offset: 0x000D37C7
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me.childPrefab = Nothing
	End Sub

	' Token: 0x040020D7 RID: 8407
	<SerializeField()>
	Private childPrefab As BasicProjectile

	' Token: 0x040020D8 RID: 8408
	Private direction As Integer

	' Token: 0x040020D9 RID: 8409
	Private idleTime As Single

	' Token: 0x040020DA RID: 8410
	Private warningTime As Single

	' Token: 0x040020DB RID: 8411
	Private childSpeed As Single

	' Token: 0x040020DC RID: 8412
	Private childCount As Integer
End Class
