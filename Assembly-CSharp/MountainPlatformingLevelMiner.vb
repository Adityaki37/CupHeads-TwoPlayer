Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020008E5 RID: 2277
Public Class MountainPlatformingLevelMiner
	Inherits PlatformingLevelGroundMovementEnemy

	' Token: 0x06003557 RID: 13655 RVA: 0x001F1322 File Offset: 0x001EF722
	Protected Overrides Sub Start()
		MyBase.Start()
		Me.startPos = MyBase.transform.position
		MyBase.StartCoroutine(Me.descend_cr())
	End Sub

	' Token: 0x06003558 RID: 13656 RVA: 0x001F1348 File Offset: 0x001EF748
	Private Iterator Function descend_cr() As IEnumerator
		Me.floating = False
		Me.landing = True
		Dim t As Single = 0F
		Dim time As Single = MyBase.Properties.minerDescendTime
		Dim endPos As Vector3 = New Vector3(MyBase.transform.position.x, MyBase.transform.position.y - 400F)
		Dim startPos As Vector3 = MyBase.transform.position
		AudioManager.Play("castle_miner_spawn")
		Me.emitAudioFromObject.Add("castle_miner_spawn")
		While t < time
			t += CupheadTime.Delta
			Dim val As Single = EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, 0F, 1F, t / time)
			MyBase.transform.position = Vector2.Lerp(startPos, endPos, val)
			Yield Nothing
		End While
		Me.rope.transform.parent = Nothing
		Yield CupheadTime.WaitForSeconds(Me, 0.4F)
		MyBase.animator.SetTrigger("Continue")
		Me.rope.animator.SetTrigger("Jump")
		Me.floating = True
		Me.landing = False
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Jump_Start", False, True)
		Me.rope.animator.SetTrigger("Pull")
		While Not MyBase.Grounded
			Yield Nothing
		End While
		MyBase.animator.SetTrigger("Land")
		Me.rope.transform.parent = Nothing
		Me.rope.PullRope(MyBase.Properties.minerRopeAscendTime, startPos)
		Me.leftRope = True
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Jump_End", False, True)
		MyBase.StartCoroutine(Me.shoot_cr())
		MyBase.StartCoroutine(Me.look_direction_cr())
		MyBase.StartCoroutine(Me.face_direction_cr())
		Yield Nothing
		Return
	End Function

	' Token: 0x06003559 RID: 13657 RVA: 0x001F1364 File Offset: 0x001EF764
	Private Iterator Function look_direction_cr() As IEnumerator
		Dim maxDist As Single = 30F
		While Me.player Is Nothing
			Yield Nothing
		End While
		While True
			Dim dist As Single = Me.player.transform.position.y - Me.lookAt.transform.position.y
			If dist < maxDist AndAlso dist > -maxDist Then
				Me.straight.enabled = True
				Me.down.enabled = False
				Me.up.enabled = False
			ElseIf dist > maxDist Then
				Me.straight.enabled = False
				Me.down.enabled = False
				Me.up.enabled = True
			Else
				Me.straight.enabled = False
				Me.down.enabled = True
				Me.up.enabled = False
			End If
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x0600355A RID: 13658 RVA: 0x001F1380 File Offset: 0x001EF780
	Private Iterator Function face_direction_cr() As IEnumerator
		While Me.player Is Nothing
			Yield Nothing
		End While
		While True
			If Not Me.inAttack AndAlso ((Me.player.transform.position.x > MyBase.transform.position.x AndAlso MyBase.direction = PlatformingLevelGroundMovementEnemy.Direction.Left) OrElse (Me.player.transform.position.x < MyBase.transform.position.x AndAlso MyBase.direction = PlatformingLevelGroundMovementEnemy.Direction.Right)) AndAlso Not MyBase.animator.GetCurrentAnimatorStateInfo(0).IsName("Turn") Then
				Me.Turn()
			End If
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x0600355B RID: 13659 RVA: 0x001F139C File Offset: 0x001EF79C
	Private Iterator Function shoot_cr() As IEnumerator
		While True
			While MyBase.transform.position.x > CupheadLevelCamera.Current.Bounds.xMax + Me.offset OrElse MyBase.transform.position.x < CupheadLevelCamera.Current.Bounds.xMin - Me.offset
				Yield Nothing
			End While
			Me.player = PlayerManager.GetNext()
			Yield CupheadTime.WaitForSeconds(Me, MyBase.Properties.minerShotDelay.RandomFloat())
			Me.inAttack = True
			MyBase.animator.SetTrigger("Shoot")
			While Me.currentPickaxe IsNot Nothing OrElse Not Me.isShooting
				Yield Nothing
			End While
			MyBase.animator.Play("Catch")
			Me.isShooting = False
			Me.inAttack = False
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x0600355C RID: 13660 RVA: 0x001F13B8 File Offset: 0x001EF7B8
	Private Sub ShootPickaxe()
		If Me.player Is Nothing OrElse Me.player.IsDead Then
			Me.player = PlayerManager.GetNext()
		End If
		Dim vector As Vector3 = Me.player.transform.position - Me.root.transform.position
		Dim vector2 As Vector3 = Me.root.transform.position + vector.normalized * MyBase.Properties.minerDistance
		Me.currentPickaxe = Me.pickaxe.Create(Me.root.transform.position, MathUtils.DirectionToAngle(vector), MyBase.Properties.minerShootSpeed, Me, vector2, Me.catchRoot.transform.position)
		Me.isShooting = True
	End Sub

	' Token: 0x0600355D RID: 13661 RVA: 0x001F1499 File Offset: 0x001EF899
	Private Sub ShootSFX()
		AudioManager.Play("castle_miner_throw")
		Me.emitAudioFromObject.Add("castle_miner_throw")
	End Sub

	' Token: 0x0600355E RID: 13662 RVA: 0x001F14B5 File Offset: 0x001EF8B5
	Private Sub CatchSFX()
		AudioManager.Play("castle_miner_catch_pick")
		Me.emitAudioFromObject.Add("castle_miner_catch_pick")
	End Sub

	' Token: 0x0600355F RID: 13663 RVA: 0x001F14D4 File Offset: 0x001EF8D4
	Private Sub Offset()
		If MyBase.direction = PlatformingLevelGroundMovementEnemy.Direction.Left Then
			MyBase.transform.AddPosition(47F, 0F, 0F)
		Else
			MyBase.transform.AddPosition(-47F, 0F, 0F)
		End If
	End Sub

	' Token: 0x06003560 RID: 13664 RVA: 0x001F1528 File Offset: 0x001EF928
	Protected Overrides Sub Die()
		MyBase.GetComponent(Of Collider2D)().enabled = False
		Me.StopAllCoroutines()
		If Not Me.leftRope Then
			Me.rope.animator.SetTrigger("Jump")
			Me.rope.animator.SetTrigger("Pull")
			Me.rope.transform.parent = Nothing
			Me.rope.PullRope(MyBase.Properties.minerRopeAscendTime, Me.startPos)
		End If
		AudioManager.Play("castle_generic_death")
		Me.emitAudioFromObject.Add("castle_generic_death")
		MyBase.Die()
	End Sub

	' Token: 0x06003561 RID: 13665 RVA: 0x001F15CE File Offset: 0x001EF9CE
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me.pickaxe = Nothing
	End Sub

	' Token: 0x04003D7A RID: 15738
	<SerializeField()>
	Private pickaxe As MountainPlatformingLevelPickaxeProjectile

	' Token: 0x04003D7B RID: 15739
	<SerializeField()>
	Private straight As SpriteRenderer

	' Token: 0x04003D7C RID: 15740
	<SerializeField()>
	Private up As SpriteRenderer

	' Token: 0x04003D7D RID: 15741
	<SerializeField()>
	Private down As SpriteRenderer

	' Token: 0x04003D7E RID: 15742
	<SerializeField()>
	Private lookAt As Transform

	' Token: 0x04003D7F RID: 15743
	<SerializeField()>
	Private rope As MountainPlatformingLevelMinerRope

	' Token: 0x04003D80 RID: 15744
	<SerializeField()>
	Private root As Transform

	' Token: 0x04003D81 RID: 15745
	<SerializeField()>
	Private catchRoot As Transform

	' Token: 0x04003D82 RID: 15746
	Private startPos As Vector3

	' Token: 0x04003D83 RID: 15747
	Private player As AbstractPlayerController

	' Token: 0x04003D84 RID: 15748
	Private currentPickaxe As MountainPlatformingLevelPickaxeProjectile

	' Token: 0x04003D85 RID: 15749
	Private isShooting As Boolean

	' Token: 0x04003D86 RID: 15750
	Private inAttack As Boolean

	' Token: 0x04003D87 RID: 15751
	Private leftRope As Boolean

	' Token: 0x04003D88 RID: 15752
	Private offset As Single = 50F
End Class
