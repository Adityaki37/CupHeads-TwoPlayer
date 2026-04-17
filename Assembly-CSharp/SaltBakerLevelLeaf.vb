Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020007CD RID: 1997
Public Class SaltBakerLevelLeaf
	Inherits AbstractProjectile

	' Token: 0x06002D55 RID: 11605 RVA: 0x001ABC1C File Offset: 0x001AA01C
	Public Overridable Function Init(pos As Vector3, xTime As Single, xDistance As Single, yGravity As Single, ySpeed As MinMax, parent As SaltbakerLevelSaltbaker, animID As Integer) As SaltBakerLevelLeaf
		MyBase.ResetLifetime()
		MyBase.ResetDistance()
		MyBase.transform.position = pos
		Me.xDistance = xDistance
		Me.xTime = xTime
		Me.ySpeedMinMax = ySpeed
		Me.yGravity = yGravity
		Me.Move()
		Me.parent = parent
		AddHandler Me.parent.OnDeathEvent, AddressOf Me.Death
		Me.animID = animID
		Return Me
	End Function

	' Token: 0x06002D56 RID: 11606 RVA: 0x001ABC8D File Offset: 0x001AA08D
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
		MyBase.OnCollisionPlayer(hit, phase)
	End Sub

	' Token: 0x06002D57 RID: 11607 RVA: 0x001ABCAB File Offset: 0x001AA0AB
	Private Sub Move()
		MyBase.StartCoroutine(Me.move_cr())
	End Sub

	' Token: 0x06002D58 RID: 11608 RVA: 0x001ABCBC File Offset: 0x001AA0BC
	Private Iterator Function move_cr() As IEnumerator
		Dim ground As Single = CSng(Level.Current.Ground)
		Dim wait As YieldInstruction = New WaitForFixedUpdate()
		Dim val As Single = 0F
		Dim yVal As Single = 0F
		Dim xVal As Single = 0F
		Dim t As Single = 0F
		Dim startX As Single = MyBase.transform.position.x
		Dim endX As Single = MyBase.transform.position.x + Me.xDistance
		Dim animationHelper As AnimationHelper = MyBase.GetComponent(Of AnimationHelper)()
		animationHelper.Speed = 0F
		Dim dirRight As Boolean = True
		While MyBase.transform.position.y > ground
			val = t / Me.xTime
			xVal = EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, 0F, 1F, val)
			yVal = If((val >= 0.5F), (1F - val), val)
			Dim ySpeed As Single = Me.ySpeedMinMax.GetFloatAt(yVal)
			Dim yPos As Single = MyBase.transform.position.y - (ySpeed + Me.yGravity) * CupheadTime.FixedDelta
			Dim animName As String = Convert.ToChar(65 + Me.animID).ToString()
			t += CupheadTime.FixedDelta
			MyBase.transform.SetPosition(New Single?(Mathf.Lerp(startX, endX, xVal)), New Single?(yPos), Nothing)
			MyBase.animator.Play(animName, 0, val / 2F + If((Not dirRight), 0.5F, 0F))
			If t >= Me.xTime Then
				t = 0F
				endX = startX
				startX = MyBase.transform.position.x
				dirRight = Not dirRight
			End If
			Yield wait
		End While
		Me.boxColl.enabled = False
		animationHelper.Speed = 1F
		MyBase.animator.SetTrigger("Die")
		Yield MyBase.animator.WaitForAnimationToStart(Me, "None", False)
		Me.Recycle()
		Yield Nothing
		Return
	End Function

	' Token: 0x06002D59 RID: 11609 RVA: 0x001ABCD7 File Offset: 0x001AA0D7
	Private Sub Death()
		Me.Recycle()
	End Sub

	' Token: 0x06002D5A RID: 11610 RVA: 0x001ABCDF File Offset: 0x001AA0DF
	Protected Overrides Sub OnDestroy()
		RemoveHandler Me.parent.OnDeathEvent, AddressOf Me.Death
		MyBase.OnDestroy()
	End Sub

	' Token: 0x040035DF RID: 13791
	Private xTime As Single

	' Token: 0x040035E0 RID: 13792
	Private xDistance As Single

	' Token: 0x040035E1 RID: 13793
	Private yGravity As Single

	' Token: 0x040035E2 RID: 13794
	Private ySpeedMinMax As MinMax

	' Token: 0x040035E3 RID: 13795
	Private parent As SaltbakerLevelSaltbaker

	' Token: 0x040035E4 RID: 13796
	Private animID As Integer

	' Token: 0x040035E5 RID: 13797
	<SerializeField()>
	Private boxColl As BoxCollider2D
End Class
