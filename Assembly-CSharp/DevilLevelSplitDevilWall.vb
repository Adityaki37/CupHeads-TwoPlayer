Imports System
Imports UnityEngine

' Token: 0x02000598 RID: 1432
Public Class DevilLevelSplitDevilWall
	Inherits AbstractProjectile

	' Token: 0x06001B71 RID: 7025 RVA: 0x000FB094 File Offset: 0x000F9494
	Public Function Create(xPos As Single, xVelocity As Single, distance As Single, devil As DevilLevelSplitDevil) As DevilLevelSplitDevilWall
		Dim devilLevelSplitDevilWall As DevilLevelSplitDevilWall = TryCast(MyBase.Create(New Vector2(xPos, 30F)), DevilLevelSplitDevilWall)
		devilLevelSplitDevilWall.xVelocity = xVelocity
		devilLevelSplitDevilWall.DestroyDistance = distance
		devilLevelSplitDevilWall.devil = devil
		devilLevelSplitDevilWall.UpdateColor()
		CupheadLevelCamera.Current.StartShake(4F)
		Return devilLevelSplitDevilWall
	End Function

	' Token: 0x06001B72 RID: 7026 RVA: 0x000FB0E4 File Offset: 0x000F94E4
	Protected Overrides Sub Update()
		MyBase.Update()
		If MyBase.dead Then
			Return
		End If
		If Me.devil Is Nothing Then
			Me.Die()
			Return
		End If
		MyBase.transform.AddPosition(Me.xVelocity * CupheadTime.Delta, 0F, 0F)
		MyBase.transform.SetScale(New Single?(Global.UnityEngine.Random.Range(0.9F, 1F)), Nothing, Nothing)
		Me.UpdateColor()
	End Sub

	' Token: 0x06001B73 RID: 7027 RVA: 0x000FB178 File Offset: 0x000F9578
	Private Sub UpdateColor()
	End Sub

	' Token: 0x06001B74 RID: 7028 RVA: 0x000FB17A File Offset: 0x000F957A
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x06001B75 RID: 7029 RVA: 0x000FB198 File Offset: 0x000F9598
	Protected Overrides Sub Die()
		MyBase.Die()
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub

	' Token: 0x06001B76 RID: 7030 RVA: 0x000FB1AB File Offset: 0x000F95AB
	Protected Overrides Sub OnDestroy()
		If Global.UnityEngine.[Object].FindObjectsOfType(Of DevilLevelSplitDevilWall)().Length <= 1 Then
			CupheadLevelCamera.Current.EndShake(0.5F)
		End If
		MyBase.OnDestroy()
	End Sub

	' Token: 0x040024A4 RID: 9380
	Private xVelocity As Single

	' Token: 0x040024A5 RID: 9381
	Private devil As DevilLevelSplitDevil

	' Token: 0x040024A6 RID: 9382
	Private Const Y_POS As Single = 30F
End Class
