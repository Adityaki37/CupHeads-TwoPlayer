Imports System
Imports UnityEngine

' Token: 0x0200070C RID: 1804
Public Class OldManLevelPuppetBallFeather
	Inherits Effect

	' Token: 0x060026F4 RID: 9972 RVA: 0x0016D0B0 File Offset: 0x0016B4B0
	Public Overrides Function Create(position As Vector3, scale As Vector3) As Effect
		Dim oldManLevelPuppetBallFeather As OldManLevelPuppetBallFeather = TryCast(MyBase.Create(position, scale), OldManLevelPuppetBallFeather)
		oldManLevelPuppetBallFeather.vel = MathUtils.AngleToDirection(CSng(Global.UnityEngine.Random.Range(0, 360))) * (Global.UnityEngine.Random.Range(Me.startSpeedMin, Me.startSpeedMax) + If((Global.UnityEngine.Random.Range(0F, 6F) >= 1F), 0F, Me.startSpeedMax))
		oldManLevelPuppetBallFeather.rotateDir = CSng(MathUtils.PlusOrMinus())
		oldManLevelPuppetBallFeather.fallFactor = Global.UnityEngine.Random.Range(oldManLevelPuppetBallFeather.fallFactorMin, oldManLevelPuppetBallFeather.fallFactorMax)
		oldManLevelPuppetBallFeather.fallVel.y = Global.UnityEngine.Random.Range(0F, 0.5F)
		oldManLevelPuppetBallFeather.anim.speed = Global.UnityEngine.Random.Range(0.5F, 0.75F)
		Return oldManLevelPuppetBallFeather
	End Function

	' Token: 0x060026F5 RID: 9973 RVA: 0x0016D180 File Offset: 0x0016B580
	Private Sub PhysicsUpdate()
		If CupheadTime.FixedDelta = 0F Then
			Return
		End If
		MyBase.transform.position += Me.vel + Me.fallVel
		Me.vel *= Me.slowFactor
		Dim magnitude As Single = Me.vel.magnitude
		MyBase.transform.Rotate(New Vector3(0F, 0F, Me.rotateDir * Mathf.InverseLerp(0F, Me.startSpeedMax, magnitude)) * 100F)
		If magnitude < 1F Then
			Me.fallVel.y = Me.fallVel.y - Me.fallFactor
			MyBase.transform.eulerAngles = New Vector3(0F, 0F, Mathf.Lerp(MyBase.transform.eulerAngles.z, 0F, 0.5F))
		End If
		If MyBase.transform.position.y < -560F Then
			Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
		End If
	End Sub

	' Token: 0x060026F6 RID: 9974 RVA: 0x0016D2AA File Offset: 0x0016B6AA
	Private Sub FixedUpdate()
		Me.skipFrame = Not Me.skipFrame
		If Me.skipFrame Then
			Return
		End If
		Me.PhysicsUpdate()
		Me.PhysicsUpdate()
	End Sub

	' Token: 0x04002FA0 RID: 12192
	<SerializeField()>
	Private anim As Animator

	' Token: 0x04002FA1 RID: 12193
	<SerializeField()>
	Private vel As Vector3

	' Token: 0x04002FA2 RID: 12194
	Private fallVel As Vector3

	' Token: 0x04002FA3 RID: 12195
	<SerializeField()>
	Private startSpeedMin As Single = 10F

	' Token: 0x04002FA4 RID: 12196
	<SerializeField()>
	Private startSpeedMax As Single = 20F

	' Token: 0x04002FA5 RID: 12197
	<SerializeField()>
	Private slowFactor As Single = 0.95F

	' Token: 0x04002FA6 RID: 12198
	<SerializeField()>
	Private fallFactorMin As Single = 0.1F

	' Token: 0x04002FA7 RID: 12199
	<SerializeField()>
	Private fallFactorMax As Single = 0.2F

	' Token: 0x04002FA8 RID: 12200
	Private fallFactor As Single

	' Token: 0x04002FA9 RID: 12201
	Private rotateDir As Single

	' Token: 0x04002FAA RID: 12202
	Private skipFrame As Boolean
End Class
