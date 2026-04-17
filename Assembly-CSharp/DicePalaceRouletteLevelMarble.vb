Imports System
Imports UnityEngine

' Token: 0x020005E2 RID: 1506
Public Class DicePalaceRouletteLevelMarble
	Inherits BasicProjectile

	' Token: 0x06001DDA RID: 7642 RVA: 0x00112C8A File Offset: 0x0011108A
	Protected Overrides Sub Start()
		MyBase.Start()
		MyBase.animator.Play("Fall", 0, Global.UnityEngine.Random.value)
	End Sub

	' Token: 0x06001DDB RID: 7643 RVA: 0x00112CA8 File Offset: 0x001110A8
	Protected Overrides Sub OnCollisionGround(hit As GameObject, phase As CollisionPhase)
		If phase = CollisionPhase.Enter Then
			MyBase.animator.SetFloat("Variation", CSng(Global.UnityEngine.Random.Range(0, 3)) / 2F)
			MyBase.animator.SetTrigger("Ground")
			Me.move = False
			Me.Speed = 0F
		End If
	End Sub

	' Token: 0x06001DDC RID: 7644 RVA: 0x00112CFB File Offset: 0x001110FB
	Public Sub OnAnimEnd()
		AudioManager.Play("dice_palace_roulette_balls_splat")
		Me.emitAudioFromObject.Add("dice_palace_roulette_balls_splat")
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub

	' Token: 0x040026A9 RID: 9897
	Private Const FallState As String = "Fall"

	' Token: 0x040026AA RID: 9898
	Private Const GroundParameterName As String = "Ground"

	' Token: 0x040026AB RID: 9899
	Private Const VariationParameterName As String = "Variation"

	' Token: 0x040026AC RID: 9900
	Private Const VariationCount As Integer = 3
End Class
