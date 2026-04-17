Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x0200075B RID: 1883
Public Class RetroArcadeTentacle
	Inherits AbstractProjectile

	' Token: 0x0600290D RID: 10509 RVA: 0x0017EC20 File Offset: 0x0017D020
	Public Overridable Function Init(pos As Vector3, targetPosY As Single, onLeft As Boolean, verticalSpeed As Single, horizontalSpeed As Single) As AbstractProjectile
		MyBase.ResetLifetime()
		MyBase.ResetDistance()
		Me.target.transform.SetLocalPosition(New Single?(Me.targetRoot.localPosition.x + If((Not onLeft), (-15F), 15F)), New Single?(Me.targetRoot.localPosition.y + targetPosY), Nothing)
		Me.verticalSpeed = verticalSpeed
		Me.horizontalSpeed = horizontalSpeed
		Me.onLeft = onLeft
		MyBase.transform.position = pos
		Me.startPos = pos
		MyBase.StartCoroutine(Me.move_cr())
		Return Me
	End Function

	' Token: 0x0600290E RID: 10510 RVA: 0x0017ECD2 File Offset: 0x0017D0D2
	Protected Overrides Sub Update()
		MyBase.Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x0600290F RID: 10511 RVA: 0x0017ECF0 File Offset: 0x0017D0F0
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x06002910 RID: 10512 RVA: 0x0017ED10 File Offset: 0x0017D110
	Private Iterator Function move_cr() As IEnumerator
		Dim wait As YieldInstruction = New WaitForFixedUpdate()
		Dim direction As Vector3 = If((Not Me.onLeft), Vector3.left, Vector3.right)
		Me.canMove = True
		While MyBase.transform.position.y < 0F
			MyBase.transform.position += Vector3.up * Me.verticalSpeed * CupheadTime.FixedDelta
			Yield wait
		End While
		While Me.canMove AndAlso Not Me.target.IsDead
			MyBase.transform.position += direction * Me.horizontalSpeed * CupheadTime.FixedDelta
			Yield wait
		End While
		While MyBase.transform.position.y > Me.startPos.y
			MyBase.transform.position += Vector3.down * Me.verticalSpeed * CupheadTime.FixedDelta
			Yield wait
		End While
		Me.Recycle()
		Return
	End Function

	' Token: 0x06002911 RID: 10513 RVA: 0x0017ED2B File Offset: 0x0017D12B
	Protected Overrides Sub OnCollision(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollision(hit, phase)
		If hit.GetComponent(Of RetroArcadeTentacle)() Then
			Me.canMove = False
		End If
	End Sub

	' Token: 0x040031F7 RID: 12791
	<SerializeField()>
	Private target As RetroArcadeTentacleTarget

	' Token: 0x040031F8 RID: 12792
	<SerializeField()>
	Private targetRoot As Transform

	' Token: 0x040031F9 RID: 12793
	Private Const OFFSET As Single = 15F

	' Token: 0x040031FA RID: 12794
	Private verticalSpeed As Single

	' Token: 0x040031FB RID: 12795
	Private horizontalSpeed As Single

	' Token: 0x040031FC RID: 12796
	Private onLeft As Boolean

	' Token: 0x040031FD RID: 12797
	Private canMove As Boolean

	' Token: 0x040031FE RID: 12798
	Private startPos As Vector3
End Class
