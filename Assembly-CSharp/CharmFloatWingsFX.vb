Imports System
Imports UnityEngine

' Token: 0x02000A45 RID: 2629
Public Class CharmFloatWingsFX
	Inherits Effect

	' Token: 0x06003EB1 RID: 16049 RVA: 0x00225ED8 File Offset: 0x002242D8
	Public Overrides Function Create(position As Vector3, scale As Vector3) As Effect
		Dim charmFloatWingsFX As CharmFloatWingsFX = TryCast(MyBase.Create(position, scale), CharmFloatWingsFX)
		charmFloatWingsFX.anim.Play("Feather", 0, Global.UnityEngine.Random.Range(0F, 0.5F))
		charmFloatWingsFX.vel = MathUtils.AngleToDirection(CSng(Global.UnityEngine.Random.Range(0, 360))) * (Global.UnityEngine.Random.Range(Me.startSpeedMin, Me.startSpeedMax) + If((Global.UnityEngine.Random.Range(0F, 6F) >= 1F), 0F, Me.startSpeedMax))
		Return charmFloatWingsFX
	End Function

	' Token: 0x06003EB2 RID: 16050 RVA: 0x00225F70 File Offset: 0x00224370
	Private Sub FixedUpdate()
		MyBase.transform.position += Me.vel
		Me.vel *= Me.slowFactor
		Me.vel.y = Me.vel.y + Me.riseFactor
	End Sub

	' Token: 0x040045B8 RID: 17848
	<SerializeField()>
	Private anim As Animator

	' Token: 0x040045B9 RID: 17849
	<SerializeField()>
	Private vel As Vector3

	' Token: 0x040045BA RID: 17850
	<SerializeField()>
	Private startSpeedMin As Single = 10F

	' Token: 0x040045BB RID: 17851
	<SerializeField()>
	Private startSpeedMax As Single = 20F

	' Token: 0x040045BC RID: 17852
	<SerializeField()>
	Private slowFactor As Single = 0.95F

	' Token: 0x040045BD RID: 17853
	<SerializeField()>
	Private riseFactor As Single = 0.02F
End Class
