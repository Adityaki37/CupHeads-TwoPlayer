Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x0200051F RID: 1311
Public Class BeeLevelQueenSpitProjectile
	Inherits AbstractProjectile

	' Token: 0x06001778 RID: 6008 RVA: 0x000D3470 File Offset: 0x000D1870
	Public Function Create(pos As Vector2, scale As Vector2, speed As Single, time As Vector2) As BeeLevelQueenSpitProjectile
		Dim beeLevelQueenSpitProjectile As BeeLevelQueenSpitProjectile = TryCast(MyBase.Create(pos, 0F, scale), BeeLevelQueenSpitProjectile)
		beeLevelQueenSpitProjectile.speed = speed
		beeLevelQueenSpitProjectile.time = time
		Return beeLevelQueenSpitProjectile
	End Function

	' Token: 0x1700032A RID: 810
	' (get) Token: 0x06001779 RID: 6009 RVA: 0x000D34A0 File Offset: 0x000D18A0
	Protected Overrides ReadOnly Property DestroyLifetime As Single
		Get
			Return 1000F
		End Get
	End Property

	' Token: 0x0600177A RID: 6010 RVA: 0x000D34A7 File Offset: 0x000D18A7
	Protected Overrides Sub Start()
		MyBase.Start()
		MyBase.StartCoroutine(Me.rotate_cr())
		MyBase.StartCoroutine(Me.move_cr())
		MyBase.StartCoroutine(Me.trail_cr())
	End Sub

	' Token: 0x0600177B RID: 6011 RVA: 0x000D34D6 File Offset: 0x000D18D6
	Protected Overrides Sub Update()
		MyBase.Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x0600177C RID: 6012 RVA: 0x000D34F4 File Offset: 0x000D18F4
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If Me.damageDealer IsNot Nothing AndAlso phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x0600177D RID: 6013 RVA: 0x000D351D File Offset: 0x000D191D
	Private Sub [End]()
		Me.StopAllCoroutines()
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub

	' Token: 0x0600177E RID: 6014 RVA: 0x000D3530 File Offset: 0x000D1930
	Private Iterator Function trail_cr() As IEnumerator
		While True
			Me.trailPrefab.Create(MyBase.transform.position)
			Yield CupheadTime.WaitForSeconds(Me, 0.25F)
		End While
		Return
	End Function

	' Token: 0x0600177F RID: 6015 RVA: 0x000D354C File Offset: 0x000D194C
	Private Iterator Function move_cr() As IEnumerator
		Dim scale As Single = MyBase.transform.localScale.x
		While True
			Dim move As Vector2 = MyBase.transform.right * Me.speed * CupheadTime.Delta * scale
			MyBase.transform.AddPosition(move.x, move.y, 0F)
			Yield Nothing
			If MyBase.transform.position.y > 720F Then
				Me.[End]()
			End If
		End While
		Return
	End Function

	' Token: 0x06001780 RID: 6016 RVA: 0x000D3568 File Offset: 0x000D1968
	Private Iterator Function rotate_cr() As IEnumerator
		Dim rotTime As Single = 0.15F
		Dim scale As Single = MyBase.transform.localScale.x
		Yield CupheadTime.WaitForSeconds(Me, 0.05F)
		While True
			Yield CupheadTime.WaitForSeconds(Me, Me.time.x)
			Yield MyBase.StartCoroutine(Me.tweenRotation_cr(0F, 90F * scale, rotTime))
			Yield CupheadTime.WaitForSeconds(Me, Me.time.y)
			Yield MyBase.StartCoroutine(Me.tweenRotation_cr(90F * scale, 180F * scale, rotTime))
			AudioManager.Play("bee_spit_bullet_turn")
			Me.emitAudioFromObject.Add("bee_spit_bullet_turn")
			Yield CupheadTime.WaitForSeconds(Me, Me.time.x)
			Yield MyBase.StartCoroutine(Me.tweenRotation_cr(180F * scale, 90F * scale, rotTime))
			Yield CupheadTime.WaitForSeconds(Me, Me.time.y)
			Yield MyBase.StartCoroutine(Me.tweenRotation_cr(90F * scale, 0F, rotTime))
		End While
		Return
	End Function

	' Token: 0x06001781 RID: 6017 RVA: 0x000D3584 File Offset: 0x000D1984
	Private Iterator Function tweenRotation_cr(start As Single, [end] As Single, time As Single) As IEnumerator
		MyBase.transform.SetEulerAngles(Nothing, Nothing, New Single?(start))
		Dim t As Single = 0F
		While t < time
			Dim val As Single = t / time
			MyBase.transform.SetEulerAngles(Nothing, Nothing, New Single?(EaseUtils.Ease(EaseUtils.EaseType.linear, start, [end], val)))
			t += CupheadTime.Delta
			Yield Nothing
		End While
		MyBase.transform.SetEulerAngles(Nothing, Nothing, New Single?([end]))
		Return
	End Function

	' Token: 0x06001782 RID: 6018 RVA: 0x000D35B4 File Offset: 0x000D19B4
	Private Iterator Function tween_cr(start As Vector2, [end] As Vector2, time As Single, ease As EaseUtils.EaseType) As IEnumerator
		MyBase.transform.position = start
		Dim t As Single = 0F
		While t < time
			Dim val As Single = t / time
			Dim x As Single = EaseUtils.Ease(ease, start.x, [end].x, val)
			Dim y As Single = EaseUtils.Ease(ease, start.y, [end].y, val)
			MyBase.transform.SetLocalPosition(New Single?(x), New Single?(y), New Single?(0F))
			t += CupheadTime.Delta
			Yield Nothing
		End While
		MyBase.transform.position = [end]
		Return
	End Function

	' Token: 0x06001783 RID: 6019 RVA: 0x000D35EC File Offset: 0x000D19EC
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me.trailPrefab = Nothing
	End Sub

	' Token: 0x040020B3 RID: 8371
	<SerializeField()>
	Private trailPrefab As Effect

	' Token: 0x040020B4 RID: 8372
	Private time As Vector2 = New Vector2(0.43F, 0.06F)

	' Token: 0x040020B5 RID: 8373
	Private speed As Single = 700F
End Class
