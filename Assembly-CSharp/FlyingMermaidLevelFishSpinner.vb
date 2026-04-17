Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000688 RID: 1672
Public Class FlyingMermaidLevelFishSpinner
	Inherits AbstractProjectile

	' Token: 0x06002342 RID: 9026 RVA: 0x0014B21C File Offset: 0x0014961C
	Public Function Create(pos As Vector2, direction As Vector2, properties As LevelProperties.FlyingMermaid.SpinnerFish) As FlyingMermaidLevelFishSpinner
		Dim flyingMermaidLevelFishSpinner As FlyingMermaidLevelFishSpinner = TryCast(MyBase.Create(), FlyingMermaidLevelFishSpinner)
		flyingMermaidLevelFishSpinner.properties = properties
		flyingMermaidLevelFishSpinner.direction = direction
		flyingMermaidLevelFishSpinner.transform.position = pos
		Return flyingMermaidLevelFishSpinner
	End Function

	' Token: 0x06002343 RID: 9027 RVA: 0x0014B255 File Offset: 0x00149655
	Protected Overrides Sub Start()
		MyBase.Start()
		MyBase.StartCoroutine(Me.move_cr())
		MyBase.StartCoroutine(Me.tails_cr())
	End Sub

	' Token: 0x06002344 RID: 9028 RVA: 0x0014B277 File Offset: 0x00149677
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x06002345 RID: 9029 RVA: 0x0014B298 File Offset: 0x00149698
	Private Iterator Function tails_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, Me.properties.timeBeforeTails)
		MyBase.animator.SetTrigger("StartTails")
		Return
	End Function

	' Token: 0x06002346 RID: 9030 RVA: 0x0014B2B4 File Offset: 0x001496B4
	Private Iterator Function move_cr() As IEnumerator
		MyBase.transform.SetEulerAngles(Nothing, Nothing, New Single?(CSng(Global.UnityEngine.Random.Range(0, 360))))
		Dim velocity As Vector2 = Me.direction * Me.properties.bulletSpeed
		Dim wait As YieldInstruction = New WaitForFixedUpdate()
		While True
			MyBase.transform.AddPosition(velocity.x * CupheadTime.FixedDelta, velocity.y * CupheadTime.FixedDelta, 0F)
			MyBase.transform.Rotate(0F, 0F, Me.properties.rotationSpeed * CupheadTime.FixedDelta)
			Yield wait
		End While
		Return
	End Function

	' Token: 0x06002347 RID: 9031 RVA: 0x0014B2CF File Offset: 0x001496CF
	Protected Overrides Sub Die()
		MyBase.Die()
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub

	' Token: 0x04002BE8 RID: 11240
	Private properties As LevelProperties.FlyingMermaid.SpinnerFish

	' Token: 0x04002BE9 RID: 11241
	Private direction As Vector2
End Class
