Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000667 RID: 1639
Public Class FlyingGenieLevelGem
	Inherits AbstractProjectile

	' Token: 0x17000392 RID: 914
	' (get) Token: 0x06002224 RID: 8740 RVA: 0x0013DF3B File Offset: 0x0013C33B
	Protected Overrides ReadOnly Property DestroyedAfterLeavingScreen As Boolean
		Get
			Return True
		End Get
	End Property

	' Token: 0x06002225 RID: 8741 RVA: 0x0013DF40 File Offset: 0x0013C340
	Public Function Create(pos As Vector2, player As AbstractPlayerController, offsetY As Single, speed As Single, parryable As Boolean, isBig As Boolean) As FlyingGenieLevelGem
		Dim flyingGenieLevelGem As FlyingGenieLevelGem = TryCast(MyBase.Create(), FlyingGenieLevelGem)
		flyingGenieLevelGem.transform.position = pos
		flyingGenieLevelGem.player = player
		flyingGenieLevelGem.offsetY = offsetY
		flyingGenieLevelGem.speed = speed
		flyingGenieLevelGem.SetParryable(parryable)
		flyingGenieLevelGem.isBig = isBig
		Return flyingGenieLevelGem
	End Function

	' Token: 0x06002226 RID: 8742 RVA: 0x0013DF94 File Offset: 0x0013C394
	Protected Overrides Sub Start()
		MyBase.Start()
		Dim num As Integer = If((Not Me.isBig), Global.UnityEngine.Random.Range(2, 9), Global.UnityEngine.Random.Range(0, 3))
		If MyBase.CanParry Then
			num = 9
		End If
		MyBase.animator.SetFloat("Variation", CSng(num) / 9F)
		Dim vector As Vector3 = New Vector3(0F, Me.offsetY, 0F)
		Dim num2 As Single = MathUtils.DirectionToAngle(Me.player.transform.position - (MyBase.transform.position + vector))
		MyBase.transform.SetEulerAngles(Nothing, Nothing, New Single?(num2))
		MyBase.StartCoroutine(Me.check_gem_cr())
	End Sub

	' Token: 0x06002227 RID: 8743 RVA: 0x0013E065 File Offset: 0x0013C465
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x06002228 RID: 8744 RVA: 0x0013E084 File Offset: 0x0013C484
	Private Iterator Function check_gem_cr() As IEnumerator
		Dim startPos As Single = (MyBase.transform.position + Vector3.up * Me.outOfChestY).y
		While MyBase.transform.position.y < startPos
			MyBase.transform.AddPosition(0F, Me.outOfChestY * Me.outOfChestSpeed * CupheadTime.Delta, 0F)
			Yield Nothing
		End While
		Me.gemRenderer.sortingLayerName = "Projectiles"
		Me.gemRenderer.sortingOrder = 2
		While MyBase.transform.position.x > -640F AndAlso MyBase.transform.position.y < 360F AndAlso MyBase.transform.position.y > -360F
			MyBase.transform.position += MyBase.transform.right * Me.speed * CupheadTime.Delta
			Yield Nothing
		End While
		Me.Die()
		Yield Nothing
		Return
	End Function

	' Token: 0x06002229 RID: 8745 RVA: 0x0013E09F File Offset: 0x0013C49F
	Protected Overrides Sub Die()
		MyBase.GetComponent(Of SpriteRenderer)().enabled = False
		MyBase.Die()
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub

	' Token: 0x04002ACA RID: 10954
	Private Const BigVariations As Integer = 3

	' Token: 0x04002ACB RID: 10955
	Private Const VariationsTotal As Integer = 10

	' Token: 0x04002ACC RID: 10956
	Private Const VariationParameterName As String = "Variation"

	' Token: 0x04002ACD RID: 10957
	Private Const ProjectilesLayer As String = "Projectiles"

	' Token: 0x04002ACE RID: 10958
	<SerializeField()>
	Private gemRenderer As SpriteRenderer

	' Token: 0x04002ACF RID: 10959
	<SerializeField()>
	Private outOfChestY As Single

	' Token: 0x04002AD0 RID: 10960
	<SerializeField()>
	Private outOfChestSpeed As Single

	' Token: 0x04002AD1 RID: 10961
	Private player As AbstractPlayerController

	' Token: 0x04002AD2 RID: 10962
	Private offsetY As Single

	' Token: 0x04002AD3 RID: 10963
	Private isBig As Boolean

	' Token: 0x04002AD4 RID: 10964
	Private velocityX As Single

	' Token: 0x04002AD5 RID: 10965
	Private speed As Single
End Class
