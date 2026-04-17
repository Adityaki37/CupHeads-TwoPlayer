Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020006A7 RID: 1703
Public MustInherit Class AbstractFrogsLevelSlotBullet
	Inherits AbstractPausableComponent

	' Token: 0x170003A8 RID: 936
	' (get) Token: 0x06002414 RID: 9236 RVA: 0x00152EDF File Offset: 0x001512DF
	Protected Overridable ReadOnly Property Y As Single
		Get
			Return CSng((Level.Current.Ground + 50))
		End Get
	End Property

	' Token: 0x170003A9 RID: 937
	' (get) Token: 0x06002415 RID: 9237 RVA: 0x00152EEF File Offset: 0x001512EF
	Protected Overridable ReadOnly Property Y_Time As Single
		Get
			Return 0.45F
		End Get
	End Property

	' Token: 0x170003AA RID: 938
	' (get) Token: 0x06002416 RID: 9238 RVA: 0x00152EF6 File Offset: 0x001512F6
	Protected Overridable ReadOnly Property Y_Ease As EaseUtils.EaseType
		Get
			Return EaseUtils.EaseType.easeOutBounce
		End Get
	End Property

	' Token: 0x06002417 RID: 9239 RVA: 0x00152EFC File Offset: 0x001512FC
	Public Function Create(pos As Vector2, speed As Single) As AbstractFrogsLevelSlotBullet
		Dim abstractFrogsLevelSlotBullet As AbstractFrogsLevelSlotBullet = Me.InstantiatePrefab(Of AbstractFrogsLevelSlotBullet)()
		abstractFrogsLevelSlotBullet.transform.SetPosition(New Single?(pos.x), New Single?(pos.y), Nothing)
		abstractFrogsLevelSlotBullet.speed = speed
		Return abstractFrogsLevelSlotBullet
	End Function

	' Token: 0x06002418 RID: 9240 RVA: 0x00152F44 File Offset: 0x00151344
	Protected Overridable Sub Start()
		Me.damageDealer = New DamageDealer(1F, 0.3F, DamageDealer.DamageSource.Enemy, True, False, False)
		Me.damageDealer.SetDirection(DamageDealer.Direction.Neutral, MyBase.transform)
		Dim gameObject As GameObject = New GameObject("Damage Shit!")
		gameObject.transform.SetParent(MyBase.transform)
		gameObject.transform.ResetLocalTransforms()
		Dim boxCollider2D As BoxCollider2D = gameObject.AddComponent(Of BoxCollider2D)()
		boxCollider2D.size = New Vector2(240F, 40F)
		boxCollider2D.isTrigger = True
		Dim collisionChild As CollisionChild = gameObject.AddComponent(Of CollisionChild)()
		AddHandler collisionChild.OnPlayerCollision, AddressOf Me.DealDamage
		MyBase.StartCoroutine(Me.x_cr())
		MyBase.StartCoroutine(Me.y_cr())
	End Sub

	' Token: 0x06002419 RID: 9241 RVA: 0x00152FF9 File Offset: 0x001513F9
	Private Sub Update()
		Me.damageDealer.Update()
	End Sub

	' Token: 0x0600241A RID: 9242 RVA: 0x00153006 File Offset: 0x00151406
	Protected Sub DealDamage(hit As GameObject, phase As CollisionPhase)
		Me.damageDealer.DealDamage(hit)
	End Sub

	' Token: 0x0600241B RID: 9243 RVA: 0x00153015 File Offset: 0x00151415
	Protected Overridable Sub [End]()
		Me.StopAllCoroutines()
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub

	' Token: 0x0600241C RID: 9244 RVA: 0x00153028 File Offset: 0x00151428
	Private Iterator Function x_cr() As IEnumerator
		Dim wait As YieldInstruction = New WaitForFixedUpdate()
		While True
			If MyBase.transform.position.x < -1280F Then
				Me.[End]()
			End If
			MyBase.transform.AddPosition(-Me.speed * CupheadTime.FixedDelta, 0F, 0F)
			Yield wait
		End While
		Return
	End Function

	' Token: 0x0600241D RID: 9245 RVA: 0x00153044 File Offset: 0x00151444
	Private Iterator Function y_cr() As IEnumerator
		Dim wait As YieldInstruction = New WaitForFixedUpdate()
		Dim start As Single = MyBase.transform.position.y
		Dim t As Single = 0F
		While t < Me.Y_Time
			Dim val As Single = t / Me.Y_Time
			Dim y As Single = EaseUtils.Ease(Me.Y_Ease, start, Me.Y, val)
			MyBase.transform.SetPosition(Nothing, New Single?(y), Nothing)
			t += CupheadTime.FixedDelta
			Yield wait
		End While
		Return
	End Function

	' Token: 0x04002CE3 RID: 11491
	Protected damageDealer As DamageDealer

	' Token: 0x04002CE4 RID: 11492
	Protected speed As Single
End Class
