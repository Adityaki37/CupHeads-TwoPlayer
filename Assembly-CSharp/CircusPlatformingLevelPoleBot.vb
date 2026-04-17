Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020008A8 RID: 2216
Public Class CircusPlatformingLevelPoleBot
	Inherits AbstractPlatformingLevelEnemy

	' Token: 0x060033A4 RID: 13220 RVA: 0x001DFFF8 File Offset: 0x001DE3F8
	Protected Overrides Sub Start()
		MyBase.Start()
		Me.start = MyBase.transform.position.y
		Me.velocity = New Vector2(Global.UnityEngine.Random.Range(Me.minVelocity.min, Me.minVelocity.max), Global.UnityEngine.Random.Range(Me.maxVelocity.min, Me.maxVelocity.max))
		Me.startVelocity = Me.velocity
		Me.gravity = 1000F
	End Sub

	' Token: 0x060033A5 RID: 13221 RVA: 0x001E007C File Offset: 0x001DE47C
	Public Sub SlideDown()
		MyBase.StartCoroutine(Me.slide_cr())
	End Sub

	' Token: 0x060033A6 RID: 13222 RVA: 0x001E008C File Offset: 0x001DE48C
	Private Iterator Function slide_cr() As IEnumerator
		Me.isSliding = True
		MyBase.animator.SetBool("Falling", True)
		Dim wait As YieldInstruction = New WaitForFixedUpdate()
		Yield CupheadTime.WaitForSeconds(Me, Me.fallDelay)
		While MyBase.transform.position.y > Me.start - MyBase.GetComponent(Of BoxCollider2D)().size.y * 1.38F
			MyBase.transform.AddPosition(0F, -MyBase.Properties.poleSpeedMovement * CupheadTime.FixedDelta, 0F)
			Yield wait
		End While
		Me.start = MyBase.transform.position.y
		Me.isSliding = False
		MyBase.animator.SetBool("Falling", False)
		Yield Nothing
		Return
	End Function

	' Token: 0x060033A7 RID: 13223 RVA: 0x001E00A7 File Offset: 0x001DE4A7
	Protected Overrides Sub OnStart()
	End Sub

	' Token: 0x060033A8 RID: 13224 RVA: 0x001E00A9 File Offset: 0x001DE4A9
	Protected Overrides Sub Die()
		Me.PoleBotDeathSFX()
		Me.isDying = True
		MyBase.animator.SetTrigger("Dead")
		MyBase.GetComponent(Of Collider2D)().enabled = False
		MyBase.StartCoroutine(Me.fly_cr())
	End Sub

	' Token: 0x060033A9 RID: 13225 RVA: 0x001E00E4 File Offset: 0x001DE4E4
	Private Iterator Function fly_cr() As IEnumerator
		MyBase._damageReceiver.enabled = False
		Dim wait As YieldInstruction = New WaitForFixedUpdate()
		Dim timeToApex As Single = Mathf.Sqrt(2F * MyBase.transform.position.y / Me.gravity)
		Me.startVelocity.y = timeToApex * Me.gravity
		While MyBase.transform.position.y > CupheadLevelCamera.Current.Bounds.yMin
			MyBase.transform.AddPosition(-Me.velocity.x * CupheadTime.FixedDelta, Me.velocity.y * CupheadTime.FixedDelta, 0F)
			Me.velocity.y = Me.velocity.y - Me.gravity * CupheadTime.FixedDelta
			MyBase.transform.Rotate(Vector3.forward, Me.deadSpin * CupheadTime.Delta)
			Yield wait
		End While
		MyBase.Die()
		Yield Nothing
		Return
	End Function

	' Token: 0x060033AA RID: 13226 RVA: 0x001E00FF File Offset: 0x001DE4FF
	Private Sub PoleBotIdleSFX()
		If Not AudioManager.CheckIfPlaying("circus_pole_guy_idle") Then
			AudioManager.Play("circus_pole_guy_idle")
			Me.emitAudioFromObject.Add("circus_pole_guy_idle")
		End If
	End Sub

	' Token: 0x060033AB RID: 13227 RVA: 0x001E012A File Offset: 0x001DE52A
	Private Sub PoleBotFallSFX()
		AudioManager.Play("circus_pole_guy_falling")
		Me.emitAudioFromObject.Add("circus_pole_guy_falling")
	End Sub

	' Token: 0x060033AC RID: 13228 RVA: 0x001E0146 File Offset: 0x001DE546
	Private Sub PoleBotDeathSFX()
		AudioManager.Play("circus_pole_guy_death")
		Me.emitAudioFromObject.Add("circus_pole_guy_death")
	End Sub

	' Token: 0x04003BF0 RID: 15344
	Private Const FallingParameterName As String = "Falling"

	' Token: 0x04003BF1 RID: 15345
	Private Const DeadParameterName As String = "Dead"

	' Token: 0x04003BF2 RID: 15346
	Private velocity As Vector2

	' Token: 0x04003BF3 RID: 15347
	Private startVelocity As Vector2

	' Token: 0x04003BF4 RID: 15348
	Private gravity As Single

	' Token: 0x04003BF5 RID: 15349
	Public isDying As Boolean

	' Token: 0x04003BF6 RID: 15350
	Public isSliding As Boolean

	' Token: 0x04003BF7 RID: 15351
	<SerializeField()>
	Private fallDelay As Single

	' Token: 0x04003BF8 RID: 15352
	<SerializeField()>
	Private deadSpin As Single

	' Token: 0x04003BF9 RID: 15353
	<SerializeField()>
	Private minVelocity As MinMax

	' Token: 0x04003BFA RID: 15354
	<SerializeField()>
	Private maxVelocity As MinMax

	' Token: 0x04003BFB RID: 15355
	Private start As Single
End Class
