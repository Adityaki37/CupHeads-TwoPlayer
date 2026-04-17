Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports UnityEngine

' Token: 0x020008BF RID: 2239
Public Class FunhousePlatformingLevelTuba
	Inherits AbstractPlatformingLevelEnemy

	' Token: 0x06003443 RID: 13379 RVA: 0x001E5158 File Offset: 0x001E3558
	Protected Overrides Sub Start()
		MyBase.Start()
		MyBase.transform.position = Me.startPos.transform.position
		Me.start = Me.startPos.transform.position
		Me.[end] = Me.endPos.transform.position
		MyBase.StartCoroutine(Me.check_to_start_cr())
	End Sub

	' Token: 0x06003444 RID: 13380 RVA: 0x001E51C9 File Offset: 0x001E35C9
	Protected Overrides Sub OnStart()
		MyBase.StartCoroutine(Me.attack_cr())
	End Sub

	' Token: 0x06003445 RID: 13381 RVA: 0x001E51D8 File Offset: 0x001E35D8
	Private Iterator Function check_to_start_cr() As IEnumerator
		While MyBase.transform.position.x > CupheadLevelCamera.Current.Bounds.xMax + Me.offset
			Yield Nothing
		End While
		Me.OnStart()
		Yield Nothing
		Return
	End Function

	' Token: 0x06003446 RID: 13382 RVA: 0x001E51F4 File Offset: 0x001E35F4
	Private Iterator Function attack_cr() As IEnumerator
		Dim time As Single = MyBase.Properties.MoveSpeed
		Dim t As Single = 0F
		Yield CupheadTime.WaitForSeconds(Me, MyBase.Properties.tubaInitialDelay)
		While True
			While MyBase.transform.position.x > CupheadLevelCamera.Current.Bounds.xMax + Me.offset OrElse MyBase.transform.position.x < CupheadLevelCamera.Current.Bounds.xMin - Me.offset
				Yield Nothing
			End While
			t = 0F
			MyBase.animator.SetBool("isAttacking", True)
			Yield MyBase.animator.WaitForAnimationToEnd(Me, "Tuba_Anti", False, True)
			MyBase.animator.Play("Attack_" + If((Not Rand.Bool()), "B", "A"), 1)
			MyBase.StartCoroutine(Me.shoot_cr())
			While t < time
				t += CupheadTime.Delta
				Dim pos As Vector2 = MyBase.transform.position
				pos.y = Mathf.Lerp(MyBase.transform.position.y, Me.[end].y, t / time)
				MyBase.transform.position = pos
				Yield Nothing
			End While
			t = 0F
			While t < time
				t += CupheadTime.Delta
				Dim pos2 As Vector2 = MyBase.transform.position
				pos2.y = Mathf.Lerp(MyBase.transform.position.y, Me.start.y, t / time)
				MyBase.transform.position = pos2
				Yield Nothing
			End While
			Yield CupheadTime.WaitForSeconds(Me, MyBase.Properties.tubaMainDelayRange.RandomFloat())
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06003447 RID: 13383 RVA: 0x001E5210 File Offset: 0x001E3610
	Private Iterator Function shoot_cr() As IEnumerator
		AudioManager.Play("funhouse_tuba_attack")
		Me.emitAudioFromObject.Add("funhouse_tuba_attack")
		Dim delay As Single = 0F
		Dim p As BasicProjectile = Me.projectile.Create(Me.root.transform.position, 180F, MyBase.Properties.ProjectileSpeed)
		p.animator.Play("BW")
		p.transform.parent = MyBase.transform
		AddHandler p.OnDie, AddressOf Me.OnBwaaDie
		Me.bwaaList.Add(p.gameObject)
		delay = p.transform.GetComponent(Of SpriteRenderer)().bounds.size.x / 1.4F / MyBase.Properties.ProjectileSpeed
		Yield CupheadTime.WaitForSeconds(Me, delay)
		For i As Integer = 0 To MyBase.Properties.tubaACount - 1
			p = Me.projectile.Create(Me.root.transform.position, 180F, MyBase.Properties.ProjectileSpeed)
			p.animator.Play("A" + Global.UnityEngine.Random.Range(1, 4).ToStringInvariant())
			p.transform.parent = MyBase.transform
			AddHandler p.OnDie, AddressOf Me.OnBwaaDie
			Me.bwaaList.Add(p.gameObject)
			delay = p.transform.GetComponent(Of SpriteRenderer)().bounds.size.x / 2F / MyBase.Properties.ProjectileSpeed
			Yield CupheadTime.WaitForSeconds(Me, delay)
		Next
		p = Me.projectile.Create(Me.root.transform.position, 180F, MyBase.Properties.ProjectileSpeed)
		p.animator.Play("EXCLAIM")
		p.transform.parent = MyBase.transform
		AddHandler p.OnDie, AddressOf Me.OnBwaaDie
		Me.bwaaList.Add(p.gameObject)
		Yield CupheadTime.WaitForSeconds(Me, delay)
		MyBase.animator.SetBool("isAttacking", False)
		Yield Nothing
		Return
	End Function

	' Token: 0x06003448 RID: 13384 RVA: 0x001E522B File Offset: 0x001E362B
	Private Sub OnBwaaDie(p As AbstractProjectile)
		RemoveHandler p.OnDie, AddressOf Me.OnBwaaDie
		If Me.bwaaList IsNot Nothing Then
			Me.bwaaList.Remove(p.gameObject)
		End If
	End Sub

	' Token: 0x06003449 RID: 13385 RVA: 0x001E525C File Offset: 0x001E365C
	Protected Overrides Sub OnDrawGizmos()
		MyBase.OnDrawGizmos()
		Gizmos.color = New Color(0F, 1F, 0F, 1F)
		Gizmos.DrawLine(Me.startPos.transform.position, Me.endPos.transform.position)
		Gizmos.color = New Color(1F, 0F, 0F, 1F)
		Gizmos.DrawWireSphere(Me.startPos.transform.position, 10F)
		Gizmos.DrawWireSphere(Me.endPos.transform.position, 10F)
	End Sub

	' Token: 0x0600344A RID: 13386 RVA: 0x001E5304 File Offset: 0x001E3704
	Protected Overrides Sub Die()
		Me.StopAllCoroutines()
		MyBase.animator.SetTrigger("OnDeath")
		MyBase.StartCoroutine(Me.slide_off_cr())
	End Sub

	' Token: 0x0600344B RID: 13387 RVA: 0x001E532C File Offset: 0x001E372C
	Private Iterator Function slide_off_cr() As IEnumerator
		For i As Integer = 0 To Me.bwaaList.Count - 1
			If Me.bwaaList(i) IsNot Nothing Then
				Me.bwaaList(i).transform.SetParent(Nothing)
			End If
		Next
		Dim t As Single = 0F
		Dim time As Single = 3F
		Dim start As Single = MyBase.transform.position.y
		Dim wait As YieldInstruction = New WaitForFixedUpdate()
		While t < time
			t += CupheadTime.FixedDelta
			If MyBase.transform.localScale.y > 0F Then
				MyBase.transform.SetPosition(Nothing, New Single?(Mathf.Lerp(start, -860F, t / time)), Nothing)
			Else
				MyBase.transform.SetPosition(Nothing, New Single?(Mathf.Lerp(start, 1220F, t / time)), Nothing)
			End If
			Yield wait
		End While
		MyBase.Die()
		Yield Nothing
		Return
	End Function

	' Token: 0x0600344C RID: 13388 RVA: 0x001E5347 File Offset: 0x001E3747
	Private Sub SoundTubaAnti()
		AudioManager.Play("funhouse_tuba_anti")
		Me.emitAudioFromObject.Add("funhouse_tuba_anti")
	End Sub

	' Token: 0x0600344D RID: 13389 RVA: 0x001E5363 File Offset: 0x001E3763
	Private Sub SoundTubaDeath()
		AudioManager.Play("funhouse_tuba_death")
		Me.emitAudioFromObject.Add("funhouse_tuba_death")
	End Sub

	' Token: 0x0600344E RID: 13390 RVA: 0x001E537F File Offset: 0x001E377F
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me.projectile = Nothing
	End Sub

	' Token: 0x04003C7D RID: 15485
	<SerializeField()>
	Private root As Transform

	' Token: 0x04003C7E RID: 15486
	<SerializeField()>
	Private projectile As BasicProjectile

	' Token: 0x04003C7F RID: 15487
	<SerializeField()>
	Private startPos As Transform

	' Token: 0x04003C80 RID: 15488
	<SerializeField()>
	Private endPos As Transform

	' Token: 0x04003C81 RID: 15489
	Private offset As Single = 50F

	' Token: 0x04003C82 RID: 15490
	Private start As Vector2

	' Token: 0x04003C83 RID: 15491
	Private [end] As Vector2

	' Token: 0x04003C84 RID: 15492
	Private bwaaList As List(Of GameObject) = New List(Of GameObject)()
End Class
