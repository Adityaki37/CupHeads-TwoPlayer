Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x0200052A RID: 1322
Public Class ChessBOldALevelWall
	Inherits AbstractProjectile

	' Token: 0x1700032E RID: 814
	' (get) Token: 0x060017DC RID: 6108 RVA: 0x000D7905 File Offset: 0x000D5D05
	Protected Overrides ReadOnly Property DestroyLifetime As Single
		Get
			Return 0F
		End Get
	End Property

	' Token: 0x060017DD RID: 6109 RVA: 0x000D790C File Offset: 0x000D5D0C
	Public Sub StartRotate(angle As Single, parent As ChessBOldALevelBishop, loopSize As Single, speed As Single, isClockwise As Boolean, scale As Single)
		MyBase.ResetLifetime()
		MyBase.ResetDistance()
		Me.angle = angle
		Me.parent = parent
		Me.speed = speed
		Me.loopSize = loopSize
		Me.isClockwise = isClockwise
		MyBase.transform.SetScale(New Single?(scale), Nothing, Nothing)
		MyBase.StartCoroutine(Me.move_wall_cr())
	End Sub

	' Token: 0x060017DE RID: 6110 RVA: 0x000D797B File Offset: 0x000D5D7B
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
		MyBase.OnCollisionPlayer(hit, phase)
	End Sub

	' Token: 0x060017DF RID: 6111 RVA: 0x000D799C File Offset: 0x000D5D9C
	Private Iterator Function move_wall_cr() As IEnumerator
		Dim handleRotation As Vector3 = Vector3.zero
		If Me.angle = 0F OrElse Me.angle = 180F Then
			MyBase.transform.position = Me.parent.transform.position + MathUtils.AngleToDirection(Me.angle) * Me.loopSize
		Else
			MyBase.transform.position = Me.parent.transform.position + MathUtils.AngleToDirection(Me.angle) * Me.loopSize
		End If
		Me.angle *= 0.017453292F
		While True
			If Me.isClockwise Then
				Me.angle += Me.speed * CupheadTime.FixedDelta
			Else
				Me.angle -= Me.speed * CupheadTime.FixedDelta
			End If
			handleRotation = New Vector3(Mathf.Sin(Me.angle) * Me.loopSize, Mathf.Cos(Me.angle) * Me.loopSize, 0F)
			MyBase.transform.position = Me.parent.transform.position
			MyBase.transform.position += handleRotation
			Dim diff As Vector3 = Me.parent.transform.position - MyBase.transform.position
			diff.Normalize()
			MyBase.transform.rotation = Quaternion.Euler(0F, 0F, Mathf.Atan2(diff.y, diff.x) * 57.29578F + 90F)
			Yield New WaitForFixedUpdate()
		End While
		Return
	End Function

	' Token: 0x060017E0 RID: 6112 RVA: 0x000D79B7 File Offset: 0x000D5DB7
	Public Sub Dead()
		Me.Recycle()
	End Sub

	' Token: 0x04002109 RID: 8457
	Private parent As ChessBOldALevelBishop

	' Token: 0x0400210A RID: 8458
	Private isClockwise As Boolean

	' Token: 0x0400210B RID: 8459
	Private angle As Single

	' Token: 0x0400210C RID: 8460
	Private loopSize As Single

	' Token: 0x0400210D RID: 8461
	Private speed As Single
End Class
