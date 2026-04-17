Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020008B3 RID: 2227
Public Class FunhousePlatformingLevelDuck
	Inherits AbstractPlatformingLevelEnemy

	' Token: 0x060033E7 RID: 13287 RVA: 0x001E1AA9 File Offset: 0x001DFEA9
	Protected Overrides Sub OnStart()
	End Sub

	' Token: 0x060033E8 RID: 13288 RVA: 0x001E1AAC File Offset: 0x001DFEAC
	Protected Overrides Sub Start()
		MyBase.Start()
		If Me.child IsNot Nothing Then
			AddHandler Me.child.OnAnyCollision, AddressOf Me.OnCollision
			AddHandler Me.child.OnPlayerCollision, AddressOf Me.OnCollisionPlayer
		End If
		If Me.parryable Then
			Me._canParry = True
		End If
		If Not Me.isBigDuck Then
			MyBase.StartCoroutine(Me.idle_sound_cr())
		End If
		MyBase.StartCoroutine(Me.move_cr())
	End Sub

	' Token: 0x060033E9 RID: 13289 RVA: 0x001E1B37 File Offset: 0x001DFF37
	Protected Overrides Sub OnCollision(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollision(hit, phase)
		If hit.GetComponent(Of FunhousePlatformingLevelCar)() Then
			Me.Die()
		End If
	End Sub

	' Token: 0x060033EA RID: 13290 RVA: 0x001E1B58 File Offset: 0x001DFF58
	Private Iterator Function idle_sound_cr() As IEnumerator
		While True
			Yield CupheadTime.WaitForSeconds(Me, Global.UnityEngine.Random.Range(5F, 15F))
			AudioManager.Play("funhouse_small_duck_idle_sweet")
			Me.emitAudioFromObject.Add("funhouse_small_duck_idle_sweet")
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x060033EB RID: 13291 RVA: 0x001E1B74 File Offset: 0x001DFF74
	Private Iterator Function move_cr() As IEnumerator
		If Me.isBigDuck Then
			AudioManager.PlayLoop("funhouse_big_duck_idle")
			Me.emitAudioFromObject.Add("funhouse_big_duck_idle")
		ElseIf Me.smallFirst Then
			AudioManager.PlayLoop("funhouse_small_duck_idle_loop")
			Me.emitAudioFromObject.Add("funhouse_small_duck_idle_loop")
		End If
		Dim size As Single = MyBase.GetComponent(Of Collider2D)().bounds.size.x
		While MyBase.transform.position.x > CupheadLevelCamera.Current.Bounds.xMin - size
			MyBase.transform.position -= MyBase.transform.right * MyBase.Properties.MoveSpeed * CupheadTime.FixedDelta
			Yield New WaitForFixedUpdate()
		End While
		Me.DoneAnimation()
		Return
	End Function

	' Token: 0x060033EC RID: 13292 RVA: 0x001E1B90 File Offset: 0x001DFF90
	Protected Overrides Sub Die()
		Me.StopAllCoroutines()
		If Me.smallLast Then
			AudioManager.[Stop]("funhouse_small_duck_idle_loop")
		End If
		If Me.isBigDuck Then
			AudioManager.[Stop]("funhouse_big_duck_idle")
			AudioManager.Play("funhouse_big_duck_death")
			Me.emitAudioFromObject.Add("funhouse_big_duck_death")
			MyBase.animator.SetTrigger("OnDeath")
		Else
			AudioManager.Play("funhouse_small_duck_death")
			AudioManager.Play("funhouse_small_duck_death")
			MyBase.Die()
		End If
	End Sub

	' Token: 0x060033ED RID: 13293 RVA: 0x001E1C16 File Offset: 0x001E0016
	Private Sub DoneAnimation()
		If Me.isBigDuck Then
			AudioManager.[Stop]("funhouse_big_duck_idle")
		End If
		If Me.smallLast Then
			AudioManager.[Stop]("funhouse_small_duck_idle_loop")
		End If
		MyBase.Die()
	End Sub

	' Token: 0x04003C2C RID: 15404
	<SerializeField()>
	Private isBigDuck As Boolean

	' Token: 0x04003C2D RID: 15405
	<SerializeField()>
	Private parryable As Boolean

	' Token: 0x04003C2E RID: 15406
	<SerializeField()>
	Private child As CollisionChild

	' Token: 0x04003C2F RID: 15407
	Public smallFirst As Boolean

	' Token: 0x04003C30 RID: 15408
	Public smallLast As Boolean
End Class
