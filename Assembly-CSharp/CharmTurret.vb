Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000A47 RID: 2631
Public Class CharmTurret
	Inherits AbstractCollidableObject

	' Token: 0x06003EB7 RID: 16055 RVA: 0x002261DC File Offset: 0x002245DC
	Public Sub Init(rootObject As GameObject, circleSpeed As Single, projectileSpeed As Single, delay As Single)
		MyBase.transform.position = rootObject.transform.position
		Me.rootObject = rootObject
		Me.circleSpeed = circleSpeed
		Me.projectileSpeed = projectileSpeed
		Me.delay = delay
		MyBase.StartCoroutine(Me.move_cr())
		MyBase.StartCoroutine(Me.shoot_cr())
	End Sub

	' Token: 0x06003EB8 RID: 16056 RVA: 0x00226238 File Offset: 0x00224638
	Private Iterator Function move_cr() As IEnumerator
		While True
			Me.angle += Me.circleSpeed * CupheadTime.FixedDelta
			Dim handleRotationX As Vector3 = New Vector3(-Mathf.Sin(Me.angle) * Me.loopSize, 0F, 0F)
			Dim handleRotationY As Vector3 = New Vector3(0F, Mathf.Cos(Me.angle) * Me.loopSize, 0F)
			MyBase.transform.position = Me.rootObject.transform.position
			MyBase.transform.position += handleRotationX + handleRotationY
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06003EB9 RID: 16057 RVA: 0x00226254 File Offset: 0x00224654
	Private Iterator Function shoot_cr() As IEnumerator
		While True
			Yield CupheadTime.WaitForSeconds(Me, Me.delay)
			Me.projectile.Create(MyBase.transform.position, 0F, Me.projectileSpeed)
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x040045C4 RID: 17860
	<SerializeField()>
	Private projectile As BasicProjectile

	' Token: 0x040045C5 RID: 17861
	Private circleSpeed As Single

	' Token: 0x040045C6 RID: 17862
	Private projectileSpeed As Single

	' Token: 0x040045C7 RID: 17863
	Private delay As Single

	' Token: 0x040045C8 RID: 17864
	Private angle As Single

	' Token: 0x040045C9 RID: 17865
	Private loopSize As Single = 200F

	' Token: 0x040045CA RID: 17866
	Private rootObject As GameObject
End Class
