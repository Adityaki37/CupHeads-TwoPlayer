Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020008EA RID: 2282
Public Class MountainPlatformingLevelPickaxeProjectile
	Inherits AbstractProjectile

	' Token: 0x06003584 RID: 13700 RVA: 0x001F2D66 File Offset: 0x001F1166
	Protected Overrides Sub Start()
		MyBase.Start()
		MyBase.StartCoroutine(Me.throw_pickaxe_cr())
	End Sub

	' Token: 0x06003585 RID: 13701 RVA: 0x001F2D7B File Offset: 0x001F117B
	Protected Overrides Sub Update()
		MyBase.Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x06003586 RID: 13702 RVA: 0x001F2D9C File Offset: 0x001F119C
	Public Function Create(pos As Vector2, rotation As Single, speed As Single, miner As MountainPlatformingLevelMiner, targetPos As Vector3, minerPos As Vector3) As MountainPlatformingLevelPickaxeProjectile
		Dim mountainPlatformingLevelPickaxeProjectile As MountainPlatformingLevelPickaxeProjectile = TryCast(MyBase.Create(pos, rotation), MountainPlatformingLevelPickaxeProjectile)
		mountainPlatformingLevelPickaxeProjectile.miner = miner
		mountainPlatformingLevelPickaxeProjectile.speed = speed
		mountainPlatformingLevelPickaxeProjectile.minerPosition = minerPos
		mountainPlatformingLevelPickaxeProjectile.targetPos = targetPos
		Return mountainPlatformingLevelPickaxeProjectile
	End Function

	' Token: 0x06003587 RID: 13703 RVA: 0x001F2DD7 File Offset: 0x001F11D7
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x06003588 RID: 13704 RVA: 0x001F2DF8 File Offset: 0x001F11F8
	Private Iterator Function throw_pickaxe_cr() As IEnumerator
		Dim wait As YieldInstruction = New WaitForFixedUpdate()
		Dim startPos As Vector3 = MyBase.transform.position
		Dim time As Single = Vector3.Distance(MyBase.transform.position, Me.targetPos) / Me.speed
		Dim t As Single = 0F
		While t < time
			t += CupheadTime.FixedDelta
			Dim val As Single = EaseUtils.Ease(EaseUtils.EaseType.easeOutSine, 0F, 1F, t / time)
			MyBase.transform.position = Vector3.Lerp(startPos, Me.targetPos, val)
			Yield wait
		End While
		Yield wait
		MyBase.transform.position = Me.targetPos
		t = 0F
		Dim dir As Vector3 = startPos - Me.targetPos
		While True
			If Me.miner IsNot Nothing Then
				If t >= time Then
					Exit For
				End If
				t += CupheadTime.FixedDelta
				Dim num As Single = EaseUtils.Ease(EaseUtils.EaseType.easeInSine, 0F, 1F, t / time)
				MyBase.transform.position = Vector3.Lerp(Me.targetPos, Me.minerPosition, num)
			Else
				MyBase.transform.position += dir.normalized * Me.speed * CupheadTime.FixedDelta
			End If
			Yield wait
		End While
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
		Return
	End Function

	' Token: 0x04003D9C RID: 15772
	Private minerPosition As Vector3

	' Token: 0x04003D9D RID: 15773
	Private targetPos As Vector3

	' Token: 0x04003D9E RID: 15774
	Private miner As MountainPlatformingLevelMiner

	' Token: 0x04003D9F RID: 15775
	Private speed As Single

	' Token: 0x04003DA0 RID: 15776
	Private Const MAX_DIST As Single = 5F
End Class
