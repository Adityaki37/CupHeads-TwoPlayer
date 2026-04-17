Imports System
Imports UnityEngine

' Token: 0x02000A46 RID: 2630
Public Class CharmFloatWingsFXAlt
	Inherits Effect

	' Token: 0x06003EB4 RID: 16052 RVA: 0x00225FF4 File Offset: 0x002243F4
	Public Overrides Function Create(position As Vector3, scale As Vector3) As Effect
		Dim charmFloatWingsFXAlt As CharmFloatWingsFXAlt = TryCast(MyBase.Create(position, scale), CharmFloatWingsFXAlt)
		charmFloatWingsFXAlt.anim.speed = 1F
		charmFloatWingsFXAlt.anim.Play("Feather", 0, Global.UnityEngine.Random.Range(0F, 0.5F))
		charmFloatWingsFXAlt.vel = MathUtils.AngleToDirection(CSng((Global.UnityEngine.Random.Range(-45, -145) + If((Not Rand.Bool()), (-50), 50)))) * Me.startSpeed
		charmFloatWingsFXAlt.vel.y = 0F
		charmFloatWingsFXAlt.startVel = charmFloatWingsFXAlt.vel.x
		charmFloatWingsFXAlt.transform.rotation = Quaternion.Euler(0F, 0F, MathUtils.DirectionToAngle(Me.vel) + -90F * Mathf.Sign(Me.startVel))
		Return charmFloatWingsFXAlt
	End Function

	' Token: 0x06003EB5 RID: 16053 RVA: 0x002260DC File Offset: 0x002244DC
	Private Sub FixedUpdate()
		If CupheadTime.FixedDelta > 0F Then
			MyBase.transform.position += Me.vel
			Me.vel -= Me.slowFactor * Me.startVel * Vector3.right
			Me.vel.y = Me.vel.y + Me.riseFactor
			If Mathf.Sign(Me.vel.x) <> Mathf.Sign(Me.startVel) Then
				Me.vel.x = Me.vel.x * 0.95F
			End If
			MyBase.transform.rotation = Quaternion.Euler(0F, 0F, MathUtils.DirectionToAngle(Me.vel) + -90F * Mathf.Sign(Me.startVel))
		End If
	End Sub

	' Token: 0x040045BE RID: 17854
	<SerializeField()>
	Private anim As Animator

	' Token: 0x040045BF RID: 17855
	<SerializeField()>
	Private vel As Vector3

	' Token: 0x040045C0 RID: 17856
	<SerializeField()>
	Private startVel As Single

	' Token: 0x040045C1 RID: 17857
	<SerializeField()>
	Private startSpeed As Single = 30F

	' Token: 0x040045C2 RID: 17858
	<SerializeField()>
	Private slowFactor As Single = 0.95F

	' Token: 0x040045C3 RID: 17859
	<SerializeField()>
	Private riseFactor As Single = 0.02F
End Class
