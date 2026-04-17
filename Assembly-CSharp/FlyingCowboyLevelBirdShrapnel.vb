Imports System
Imports UnityEngine

' Token: 0x0200064D RID: 1613
Public Class FlyingCowboyLevelBirdShrapnel
	Inherits BasicProjectile

	' Token: 0x06002137 RID: 8503 RVA: 0x00133410 File Offset: 0x00131810
	Public Overrides Function Create() As AbstractProjectile
		Dim abstractProjectile As AbstractProjectile = MyBase.Create()
		abstractProjectile.animator.Update(0F)
		abstractProjectile.animator.Play(0, 0, Global.UnityEngine.Random.Range(0F, 1F))
		abstractProjectile.animator.Update(0F)
		abstractProjectile.animator.RoundFrame(0)
		abstractProjectile.GetComponent(Of SpriteRenderer)().flipY = Rand.Bool()
		Return abstractProjectile
	End Function

	' Token: 0x06002138 RID: 8504 RVA: 0x0013347D File Offset: 0x0013187D
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		AudioManager.Play("sfx_dlc_cowgirl_p1_dynamitehitplayer")
		Me.emitAudioFromObject.Add("sfx_dlc_cowgirl_p1_dynamitehitplayer")
	End Sub

	' Token: 0x06002139 RID: 8505 RVA: 0x001334A1 File Offset: 0x001318A1
	Private Sub animationEvent_LoopMiddleReached()
		Me.trailBRenderer.enabled = True
		Me.trailBRenderer.flipY = Rand.Bool()
	End Sub

	' Token: 0x0600213A RID: 8506 RVA: 0x001334BF File Offset: 0x001318BF
	Private Sub animationEvent_LoopEndReached()
		Me.trailARenderer.flipY = Rand.Bool()
	End Sub

	' Token: 0x040029CF RID: 10703
	<SerializeField()>
	Private trailARenderer As SpriteRenderer

	' Token: 0x040029D0 RID: 10704
	<SerializeField()>
	Private trailBRenderer As SpriteRenderer
End Class
