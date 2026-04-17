Imports System
Imports UnityEngine
Imports UnityEngine.SceneManagement

' Token: 0x020004A1 RID: 1185
<DefaultExecutionOrder(100)>
<RequireComponent(GetType(Animator))>
Public Class LevelCoin
	Inherits AbstractCollidableObject

	' Token: 0x0600134B RID: 4939 RVA: 0x000AAA6B File Offset: 0x000A8E6B
	Public Shared Sub OnLevelStart()
		PlayerData.Data.ResetLevelCoinManager()
	End Sub

	' Token: 0x0600134C RID: 4940 RVA: 0x000AAA77 File Offset: 0x000A8E77
	Public Shared Sub OnLevelComplete()
		PlayerData.Data.ApplyLevelCoins()
	End Sub

	' Token: 0x1700030A RID: 778
	' (get) Token: 0x0600134D RID: 4941 RVA: 0x000AAA84 File Offset: 0x000A8E84
	Public ReadOnly Property GlobalID As String
		Get
			Return SceneManager.GetActiveScene().name + "::" + MyBase.gameObject.name
		End Get
	End Property

	' Token: 0x0600134E RID: 4942 RVA: 0x000AAAB4 File Offset: 0x000A8EB4
	Protected Overrides Sub Awake()
		Dim platformingLevel As PlatformingLevel = TryCast(Level.Current, PlatformingLevel)
		If platformingLevel Then
			platformingLevel.LevelCoinsIDs.Add(New CoinPositionAndID(Me.GlobalID, MyBase.transform.position.x))
		End If
		MyBase.Awake()
		If PlayerData.Data.GetCoinCollected(Me) Then
			Me._collected = True
			Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
			Return
		End If
		Me._spriteRenderer = MyBase.GetComponent(Of SpriteRenderer)()
	End Sub

	' Token: 0x0600134F RID: 4943 RVA: 0x000AAB38 File Offset: 0x000A8F38
	Private Sub Update()
		If Me._collected Then
			Return
		End If
		Dim player As AbstractPlayerController = PlayerManager.GetPlayer(PlayerId.PlayerOne)
		Dim player2 As AbstractPlayerController = PlayerManager.GetPlayer(PlayerId.PlayerTwo)
		If player IsNot Nothing AndAlso Vector2.Distance(MyBase.transform.position, player.center) < 100F Then
			Me.Collect(player.id)
			Return
		End If
		If player2 IsNot Nothing AndAlso Vector2.Distance(MyBase.transform.position, player2.center) < 100F Then
			Me.Collect(player2.id)
			Return
		End If
	End Sub

	' Token: 0x06001350 RID: 4944 RVA: 0x000AABE8 File Offset: 0x000A8FE8
	Private Sub Collect(player As PlayerId)
		If Me._collected Then
			Return
		End If
		PlayerData.Data.SetLevelCoinCollected(Me, True, player)
		Me._collected = True
		AudioManager.Play("level_coin_pickup")
		MyBase.animator.SetTrigger("OnDeath")
		MyBase.transform.localScale *= 1.2F
		Me._spriteRenderer.flipX = MathUtils.RandomBool()
		Me._spriteRenderer.flipY = MathUtils.RandomBool()
	End Sub

	' Token: 0x06001351 RID: 4945 RVA: 0x000AAC6A File Offset: 0x000A906A
	Private Sub OnDeathAnimComplete()
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub

	' Token: 0x04001C72 RID: 7282
	Private Const COLLECT_RANGE As Single = 100F

	' Token: 0x04001C73 RID: 7283
	Public Const NUM_COINS As Integer = 40

	' Token: 0x04001C74 RID: 7284
	Private _spriteRenderer As SpriteRenderer

	' Token: 0x04001C75 RID: 7285
	Private _collected As Boolean
End Class
