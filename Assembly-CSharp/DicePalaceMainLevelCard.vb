Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020005D0 RID: 1488
Public Class DicePalaceMainLevelCard
	Inherits AbstractProjectile

	' Token: 0x1700036A RID: 874
	' (get) Token: 0x06001D3B RID: 7483 RVA: 0x0010C55C File Offset: 0x0010A95C
	Public Overrides ReadOnly Property ParryMeterMultiplier As Single
		Get
			Return 0.25F
		End Get
	End Property

	' Token: 0x06001D3C RID: 7484 RVA: 0x0010C564 File Offset: 0x0010A964
	Public Function Create(pos As Vector3, properties As LevelProperties.DicePalaceMain.Cards, onLeft As Boolean) As DicePalaceMainLevelCard
		Dim dicePalaceMainLevelCard As DicePalaceMainLevelCard = TryCast(MyBase.Create(), DicePalaceMainLevelCard)
		dicePalaceMainLevelCard.properties = properties
		dicePalaceMainLevelCard.transform.position = pos
		dicePalaceMainLevelCard.onLeft = onLeft
		Return dicePalaceMainLevelCard
	End Function

	' Token: 0x06001D3D RID: 7485 RVA: 0x0010C598 File Offset: 0x0010A998
	Protected Overrides Sub Start()
		MyBase.Start()
		Me.direction = If((Not Me.onLeft), (-MyBase.transform.right), MyBase.transform.right)
		If MyBase.CanParry AndAlso (PlayerManager.GetPlayer(PlayerId.PlayerOne).stats.isChalice OrElse (PlayerManager.Multiplayer AndAlso PlayerManager.GetPlayer(PlayerId.PlayerTwo).stats.isChalice)) Then
			Me.nextRisingHeart = Global.UnityEngine.Random.Range(0, Me.risingHeartAnimator.Length)
			Me.chaliceParryableHearts.SetActive(True)
			MyBase.StartCoroutine(Me.rising_hearts_cr())
		End If
	End Sub

	' Token: 0x06001D3E RID: 7486 RVA: 0x0010C648 File Offset: 0x0010AA48
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x06001D3F RID: 7487 RVA: 0x0010C666 File Offset: 0x0010AA66
	Protected Overrides Sub Update()
		MyBase.Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x06001D40 RID: 7488 RVA: 0x0010C684 File Offset: 0x0010AA84
	Protected Overrides Sub FixedUpdate()
		MyBase.FixedUpdate()
		MyBase.transform.position += Me.direction * Me.properties.cardSpeed * CupheadTime.FixedDelta
		If MyBase.CanParry AndAlso Me.risingHeartRenderer(0).sortingOrder = -1 AndAlso Mathf.Abs(MyBase.transform.position.x) < 230F Then
			For i As Integer = 0 To Me.risingHeartRenderer.Length - 1
				Me.risingHeartRenderer(i).sortingOrder = 2
			Next
		End If
	End Sub

	' Token: 0x06001D41 RID: 7489 RVA: 0x0010C734 File Offset: 0x0010AB34
	Private Iterator Function rising_hearts_cr() As IEnumerator
		Me.risingHeartAnimator(Me.nextRisingHeart).Play(Global.UnityEngine.Random.Range(0, 6).ToString(), 0, 0.25F)
		Me.nextRisingHeart = (Me.nextRisingHeart + 1) Mod Me.risingHeartAnimator.Length
		Me.risingHeartAnimator(Me.nextRisingHeart).Play(Global.UnityEngine.Random.Range(0, 6).ToString(), 0, 0.5F)
		Me.nextRisingHeart = (Me.nextRisingHeart + 1) Mod Me.risingHeartAnimator.Length
		Me.risingHeartAnimator(Me.nextRisingHeart).Play(Global.UnityEngine.Random.Range(0, 6).ToString(), 0, 0.75F)
		Me.nextRisingHeart = (Me.nextRisingHeart + 1) Mod Me.risingHeartAnimator.Length
		While True
			Yield CupheadTime.WaitForSeconds(Me, Me.risingHeartSpawnTimeRange.RandomFloat())
			Me.risingHeartAnimator(Me.nextRisingHeart).Play(Global.UnityEngine.Random.Range(0, 6).ToString())
			Me.nextRisingHeart = (Me.nextRisingHeart + 1) Mod Me.risingHeartAnimator.Length
		End While
		Return
	End Function

	' Token: 0x06001D42 RID: 7490 RVA: 0x0010C74F File Offset: 0x0010AB4F
	Public Overrides Sub OnParry(player As AbstractPlayerController)
		Me.SetParryable(False)
		MyBase.StartCoroutine(Me.parryCooldown_cr())
	End Sub

	' Token: 0x06001D43 RID: 7491 RVA: 0x0010C768 File Offset: 0x0010AB68
	Private Iterator Function parryCooldown_cr() As IEnumerator
		Dim t As Single = 0F
		While t < Me.coolDown
			t += CupheadTime.Delta
			Yield Nothing
		End While
		Me.SetParryable(True)
		Yield Nothing
		Return
	End Function

	' Token: 0x04002624 RID: 9764
	Private properties As LevelProperties.DicePalaceMain.Cards

	' Token: 0x04002625 RID: 9765
	Private onLeft As Boolean

	' Token: 0x04002626 RID: 9766
	Private direction As Vector3

	' Token: 0x04002627 RID: 9767
	<SerializeField()>
	Private coolDown As Single = 0.4F

	' Token: 0x04002628 RID: 9768
	<SerializeField()>
	Private chaliceParryableHearts As GameObject

	' Token: 0x04002629 RID: 9769
	<SerializeField()>
	Private risingHeartAnimator As Animator()

	' Token: 0x0400262A RID: 9770
	Private nextRisingHeart As Integer

	' Token: 0x0400262B RID: 9771
	<SerializeField()>
	Private risingHeartRenderer As SpriteRenderer()

	' Token: 0x0400262C RID: 9772
	<SerializeField()>
	Private risingHeartSpawnTimeRange As MinMax = New MinMax(0.1667F, 0.2333F)
End Class
