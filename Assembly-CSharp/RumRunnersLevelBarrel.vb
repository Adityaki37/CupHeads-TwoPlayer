Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000786 RID: 1926
Public Class RumRunnersLevelBarrel
	Inherits LevelProperties.RumRunners.Entity

	' Token: 0x06002A6C RID: 10860 RVA: 0x0018C944 File Offset: 0x0018AD44
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.damageDealer = DamageDealer.NewEnemy()
		Me.damageReceiver = MyBase.GetComponent(Of DamageReceiver)()
		AddHandler Me.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
		Me.coll = MyBase.GetComponent(Of Collider2D)()
	End Sub

	' Token: 0x06002A6D RID: 10861 RVA: 0x0018C991 File Offset: 0x0018AD91
	Public Overrides Sub LevelInit(properties As LevelProperties.RumRunners)
		MyBase.LevelInit(properties)
		AddHandler CType(Level.Current, RumRunnersLevel).OnUpperBridgeDestroy, AddressOf Me.onUpperBridgeDestroy
	End Sub

	' Token: 0x06002A6E RID: 10862 RVA: 0x0018C9B8 File Offset: 0x0018ADB8
	Public Sub Initialize(dir As Single, spawnPos As Vector3, parent As RumRunnersLevelWorm, parryable As Boolean, isCop As Boolean)
		Me.isCop = isCop
		Me.facingDirection = dir
		MyBase.transform.position = spawnPos
		MyBase.transform.localScale = New Vector3(dir, 1F)
		Me.parent = parent
		Me.runSpeed = MyBase.properties.CurrentState.barrels.barrelSpeed
		Me.HP = CSng(MyBase.properties.CurrentState.barrels.barrelHP)
		Me._canParry = parryable
		If isCop Then
			MyBase.animator.Play("Cop")
		ElseIf Rand.Bool() Then
			MyBase.animator.Play(If((Not MyBase.canParry), "DanceA", "DanceAParry"))
		Else
			MyBase.animator.Play(If((Not MyBase.canParry), "DanceB", "DanceBParry"))
		End If
		MyBase.StartCoroutine(Me.move_cr())
	End Sub

	' Token: 0x06002A6F RID: 10863 RVA: 0x0018CABE File Offset: 0x0018AEBE
	Public Overrides Sub OnParry(player As AbstractPlayerController)
		player.stats.OnParry(1F, True)
		Me.Die(False, False)
		Me._canParry = False
	End Sub

	' Token: 0x06002A70 RID: 10864 RVA: 0x0018CAE0 File Offset: 0x0018AEE0
	Private Sub Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x06002A71 RID: 10865 RVA: 0x0018CAF8 File Offset: 0x0018AEF8
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		Me.HP -= info.damage
		If Me.HP <= 0F Then
			Level.Current.RegisterMinionKilled()
			Me.Die(False, True)
		End If
	End Sub

	' Token: 0x06002A72 RID: 10866 RVA: 0x0018CB2F File Offset: 0x0018AF2F
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		Me.damageDealer.DealDamage(hit)
	End Sub

	' Token: 0x06002A73 RID: 10867 RVA: 0x0018CB48 File Offset: 0x0018AF48
	Private Iterator Function move_cr() As IEnumerator
		While MyBase.transform.position.x * Me.facingDirection < 960F
			MyBase.transform.position += Vector3.right * Me.facingDirection * Me.runSpeed * CupheadTime.FixedDelta
			MyBase.transform.SetPosition(Nothing, New Single?(RumRunnersLevel.GroundWalkingPosY(MyBase.transform.position, Me.coll, Me.verticalOffset, 200F)), Nothing)
			If Level.Current.mode = Level.Mode.Easy AndAlso Me.parent.isDead Then
				Me.Die(False, True)
				Me._canParry = False
			End If
			Yield New WaitForFixedUpdate()
		End While
		Me.Die(True, True)
		Return
	End Function

	' Token: 0x06002A74 RID: 10868 RVA: 0x0018CB64 File Offset: 0x0018AF64
	Public Sub Die(immediate As Boolean, Optional spawnShrapnel As Boolean = True)
		RemoveHandler CType(Level.Current, RumRunnersLevel).OnUpperBridgeDestroy, AddressOf Me.onUpperBridgeDestroy
		Me.StopAllCoroutines()
		If immediate Then
			Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
			Return
		End If
		If Me.isCop Then
			MyBase.StartCoroutine(Me.copDeath_cr())
		Else
			If MyBase.transform.position.x * Me.facingDirection < 960F Then
				Dim effect As Effect = Me.deathPoof.Create(MyBase.transform.position)
				If Not spawnShrapnel Then
					effect.GetComponent(Of Animator)().Play("Poof", 0, 0.083333336F)
				End If
				Me.SFX_RUMRUN_BarrelExplode()
				If spawnShrapnel Then
					Dim num As Single = Global.UnityEngine.Random.Range(0F, 6.2831855F)
					For i As Integer = 0 To 2 - 1
						For j As Integer = 0 To 4 - 1
							Dim num2 As Single = num + 6.2831855F * CSng(j) / 4F
							Dim vector As Vector3 = New Vector3(Mathf.Cos(num2) * 50F, Mathf.Sin(num2) * 50F)
							Dim effect2 As Effect = Me.deathShrapnel.Create(MyBase.transform.position + vector)
							effect2.animator.SetInteger("Effect", j)
							effect2.animator.SetBool("Parry", Me._canParry)
							If i > 0 Then
								Dim component As SpriteRenderer = effect2.GetComponent(Of SpriteRenderer)()
								component.sortingLayerName = "Background"
								component.sortingOrder = 95
								component.color = New Color(0.7F, 0.7F, 0.7F, 1F)
								effect2.transform.SetScale(New Single?(0.75F), New Single?(0.75F), Nothing)
							End If
							Dim component2 As SpriteDeathParts = effect2.GetComponent(Of SpriteDeathParts)()
							If vector.x > 0F Then
								component2.SetVelocityX(0F, component2.VelocityXMax)
							Else
								component2.SetVelocityX(component2.VelocityXMin, 0F)
							End If
						Next
					Next
				End If
			End If
			If Not spawnShrapnel Then
				MyBase.GetComponent(Of Collider2D)().enabled = False
				Me.runSpeed = 0F
				MyBase.StartCoroutine(Me.destroy_with_delay_cr())
			Else
				Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
			End If
		End If
	End Sub

	' Token: 0x06002A75 RID: 10869 RVA: 0x0018CDC8 File Offset: 0x0018B1C8
	Private Iterator Function destroy_with_delay_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 0.041666668F)
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
		Return
	End Function

	' Token: 0x06002A76 RID: 10870 RVA: 0x0018CDE4 File Offset: 0x0018B1E4
	Private Iterator Function copDeath_cr() As IEnumerator
		Me.SFX_RUMRUN_Police_DiePoof()
		MyBase.GetComponent(Of BoxCollider2D)().enabled = False
		MyBase.animator.SetTrigger("CopDeath")
		Yield MyBase.animator.WaitForNormalizedTime(Me, 1F, "CopDeath", 0, False, False, True)
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
		Return
	End Function

	' Token: 0x06002A77 RID: 10871 RVA: 0x0018CE00 File Offset: 0x0018B200
	Private Sub onUpperBridgeDestroy(effectRange As Rangef)
		If MyBase.transform.position.y < 0F Then
			Return
		End If
		If effectRange.ContainsInclusive(MyBase.transform.position.x) Then
			Me.Die(False, True)
			Me._canParry = False
		End If
	End Sub

	' Token: 0x06002A78 RID: 10872 RVA: 0x0018CE59 File Offset: 0x0018B259
	Private Sub SFX_RUMRUN_BarrelExplode()
		AudioManager.Play("sfx_dlc_rumrun_barrel_explode")
		Me.emitAudioFromObject.Add("sfx_dlc_rumrun_barrel_explode")
	End Sub

	' Token: 0x06002A79 RID: 10873 RVA: 0x0018CE75 File Offset: 0x0018B275
	Private Sub SFX_RUMRUN_Police_DiePoof()
		AudioManager.Play("sfx_dlc_rumrun_lackey_poof")
		Me.emitAudioFromObject.Add("sfx_dlc_rumrun_lackey_poof")
		AudioManager.[Stop]("sfx_dlc_rumrun_policegun_shoot")
	End Sub

	' Token: 0x04003338 RID: 13112
	<SerializeField()>
	Private deathPoof As Effect

	' Token: 0x04003339 RID: 13113
	<SerializeField()>
	Private deathShrapnel As Effect

	' Token: 0x0400333A RID: 13114
	<SerializeField()>
	Private verticalOffset As Single

	' Token: 0x0400333B RID: 13115
	Private damageDealer As DamageDealer

	' Token: 0x0400333C RID: 13116
	Private damageReceiver As DamageReceiver

	' Token: 0x0400333D RID: 13117
	Private runSpeed As Single

	' Token: 0x0400333E RID: 13118
	Private HP As Single

	' Token: 0x0400333F RID: 13119
	Private facingDirection As Single

	' Token: 0x04003340 RID: 13120
	Private coll As Collider2D

	' Token: 0x04003341 RID: 13121
	Private parent As RumRunnersLevelWorm

	' Token: 0x04003342 RID: 13122
	Private isCop As Boolean
End Class
