Imports System
Imports UnityEngine

' Token: 0x020006D5 RID: 1749
Public Class MausoleumLevelCircleGhost
	Inherits MausoleumLevelGhostBase

	' Token: 0x170003BB RID: 955
	' (get) Token: 0x0600253C RID: 9532 RVA: 0x0015D309 File Offset: 0x0015B709
	Protected Overrides ReadOnly Property DestroyedAfterLeavingScreen As Boolean
		Get
			Return False
		End Get
	End Property

	' Token: 0x0600253D RID: 9533 RVA: 0x0015D30C File Offset: 0x0015B70C
	Public Overridable Function Create(position As Vector2, urnPosition As Vector2, rotation As Single, speed As Single, rotationSpeed As Single) As AbstractCollidableObject
		Dim mausoleumLevelCircleGhost As MausoleumLevelCircleGhost = TryCast(MyBase.Create(position, rotation, speed), MausoleumLevelCircleGhost)
		mausoleumLevelCircleGhost.rotationSpeed = rotationSpeed
		mausoleumLevelCircleGhost.rotationBase = New GameObject("CircleGhostBase").transform
		mausoleumLevelCircleGhost.rotationBase.position = urnPosition
		mausoleumLevelCircleGhost.rotation = rotation
		mausoleumLevelCircleGhost.transform.parent = mausoleumLevelCircleGhost.rotationBase
		Return mausoleumLevelCircleGhost
	End Function

	' Token: 0x0600253E RID: 9534 RVA: 0x0015D370 File Offset: 0x0015B770
	Protected Overrides Sub Start()
		MyBase.Start()
		Dim flag As Boolean = Rand.Bool()
		Me.setDirection = CSng(If((Not flag), (-360), 360))
		MyBase.GetComponent(Of SpriteRenderer)().flipY = flag
	End Sub

	' Token: 0x0600253F RID: 9535 RVA: 0x0015D3B4 File Offset: 0x0015B7B4
	Protected Overrides Sub Move()
		MyBase.transform.localPosition += MathUtils.AngleToDirection(Me.rotation) * Me.Speed * CupheadTime.FixedDelta
		Me.rotationBase.AddEulerAngles(0F, 0F, Me.rotationSpeed * Me.setDirection * CupheadTime.FixedDelta)
	End Sub

	' Token: 0x06002540 RID: 9536 RVA: 0x0015D424 File Offset: 0x0015B824
	Public Overrides Sub OnParry(player As AbstractPlayerController)
		MyBase.OnParry(player)
		AudioManager.Play("mausoleum_circle_ghost_death")
		Me.emitAudioFromObject.Add("mausoleum_circle_ghost_death")
		Global.UnityEngine.[Object].Destroy(Me.rotationBase.gameObject)
	End Sub

	' Token: 0x04002DDE RID: 11742
	Private rotationSpeed As Single

	' Token: 0x04002DDF RID: 11743
	Private rotation As Single

	' Token: 0x04002DE0 RID: 11744
	Private setDirection As Single

	' Token: 0x04002DE1 RID: 11745
	Private rotationBase As Transform
End Class
