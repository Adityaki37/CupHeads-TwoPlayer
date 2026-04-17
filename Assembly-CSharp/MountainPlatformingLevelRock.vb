Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020008EC RID: 2284
Public Class MountainPlatformingLevelRock
	Inherits AbstractProjectile

	' Token: 0x06003590 RID: 13712 RVA: 0x001F34F0 File Offset: 0x001F18F0
	Public Function Create(startPos As Vector2, fallPos As Vector2, velocity As Single, delay As Single) As MountainPlatformingLevelRock
		Dim mountainPlatformingLevelRock As MountainPlatformingLevelRock = TryCast(MyBase.Create(), MountainPlatformingLevelRock)
		mountainPlatformingLevelRock.transform.position = startPos
		mountainPlatformingLevelRock.fallPos = fallPos
		mountainPlatformingLevelRock.velocity = velocity
		mountainPlatformingLevelRock.delay = delay
		Return mountainPlatformingLevelRock
	End Function

	' Token: 0x06003591 RID: 13713 RVA: 0x001F3531 File Offset: 0x001F1931
	Protected Overrides Sub Awake()
		MyBase.Awake()
		MyBase.GetComponent(Of Collider2D)().enabled = False
		MyBase.animator.SetBool("PickedA", Rand.Bool())
	End Sub

	' Token: 0x06003592 RID: 13714 RVA: 0x001F355A File Offset: 0x001F195A
	Protected Overrides Sub Start()
		MyBase.Start()
		MyBase.StartCoroutine(Me.launch_cr())
	End Sub

	' Token: 0x06003593 RID: 13715 RVA: 0x001F356F File Offset: 0x001F196F
	Protected Overrides Sub Update()
		MyBase.Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x06003594 RID: 13716 RVA: 0x001F358D File Offset: 0x001F198D
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x06003595 RID: 13717 RVA: 0x001F35AC File Offset: 0x001F19AC
	Private Iterator Function launch_cr() As IEnumerator
		Dim x As Single = Global.UnityEngine.Random.Range(-500F, 500F)
		While MyBase.transform.position.y < CupheadLevelCamera.Current.Bounds.yMax + 100F
			MyBase.transform.AddPosition(x * CupheadTime.Delta, 1000F * CupheadTime.Delta, 0F)
			Yield Nothing
		End While
		MyBase.animator.SetTrigger("getBig")
		MyBase.transform.position = Me.fallPos
		MyBase.GetComponent(Of Collider2D)().enabled = True
		MyBase.GetComponent(Of SpriteRenderer)().sortingLayerName = SpriteLayer.Projectiles.ToString()
		Yield CupheadTime.WaitForSeconds(Me, Me.delay)
		While True
			MyBase.transform.AddPosition(0F, -Me.velocity * CupheadTime.Delta, 0F)
			Me.velocity += 1000F * CupheadTime.Delta
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06003596 RID: 13718 RVA: 0x001F35C8 File Offset: 0x001F19C8
	Protected Overrides Sub Die()
		MyBase.Die()
		AudioManager.Play("castle_giant_rock_smash")
		Me.emitAudioFromObject.Add("castle_giant_rock_smash")
		Me.StopAllCoroutines()
		MyBase.GetComponent(Of SpriteRenderer)().enabled = False
		Me.DeathParts()
		CupheadLevelCamera.Current.Shake(10F, 0.4F, False)
	End Sub

	' Token: 0x06003597 RID: 13719 RVA: 0x001F3624 File Offset: 0x001F1A24
	Public Sub DeathParts()
		Me.explosion.Create(MyBase.transform.position)
		For Each spriteDeathParts As SpriteDeathParts In Me.deathParts
			spriteDeathParts.CreatePart(MyBase.transform.position)
		Next
	End Sub

	' Token: 0x04003DA9 RID: 15785
	<SerializeField()>
	Private explosion As Effect

	' Token: 0x04003DAA RID: 15786
	<SerializeField()>
	Private deathParts As SpriteDeathParts()

	' Token: 0x04003DAB RID: 15787
	Private fallPos As Vector2

	' Token: 0x04003DAC RID: 15788
	Private velocity As Single

	' Token: 0x04003DAD RID: 15789
	Private Const LAUNCH_VELOCITY_Y As Single = 1000F

	' Token: 0x04003DAE RID: 15790
	Private Const LAUNCH_VELOCITY_X As Single = 500F

	' Token: 0x04003DAF RID: 15791
	Private Const GRAVITY As Single = 1000F

	' Token: 0x04003DB0 RID: 15792
	Private delay As Single
End Class
